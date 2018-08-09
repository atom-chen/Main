# -*- coding: utf-8 -*-
import os
import re

def parse_tab(tabPath):
    meta = {
        'path' : tabPath,
    }

    if tabPath.find('ClientTables') > 0:
        meta['check_line_len'] = False
        meta['dumplicate_id'] = True
        meta['check_last_string_null'] = False
    else:
        meta['dumplicate_id'] = False
        meta['check_line_len'] = True
        meta['check_last_string_null'] = True


    fp = open(tabPath,'r')
    colum_names = fp.readline().strip().split('\t')
    colum_types = fp.readline().strip().split('\t')
    server_tab_cfg = fp.readline()

    m = re.search(r"MAX_ID=([0-9]+);",server_tab_cfg)

    if m:
        meta['max_id'] = int(m.group(1))
    m = re.search(r"MAX_RECORD=([0-9]+);",server_tab_cfg)
    if m:
        meta['max_record'] = (m.group(1))

    colum_desc = fp.readline().strip().split('\t')

    l1,l2,l3 = len(colum_names),len(colum_types),len(colum_desc)
    if l1 != l2 or l1 != l3 or l2 != l3:
        print( ('解析表头失败，表错误:' + tabPath).decode('utf-8'))
        return

    colums = []
    for i in range(0,l1):
        ctype = colum_types[i].lower()
        cname = colum_names[i]
        colums.append({
            'name' : cname,
            'type' : ctype,
            'index' : i,
            'desc' : colum_desc[i]
        })
    meta['colums'] = colums
    fp.close()
    return meta

def check(meta):
    fp = open(meta['path'],'r')

    #跳过前4行
    for i in range(0,4):
        fp.readline()

    lineCounter = 4
    msg = ""
    error = False
    line = fp.readline()
    id_set = set()
    while line:
        lineCounter = lineCounter + 1
        if (meta['check_line_len'] and len(line) >= 512):
            msg = str.format("行字符串超长，行:%d" % lineCounter)
            error = True
            break

        if line[0] == '#':
            line = fp.readline()
            continue

        vals = line.split('\t')
        colums = meta['colums']
        #检查列是否匹配（串行、解析失败）
        if len(vals) != len(colums):
            msg = str.format("解析行错误，列数不匹配,行:%d" % lineCounter)
            error = True
            break

        recordCount = 0
        #类型是否正确
        for i in range(len(vals)):
            val = vals[i]
            colum = colums[i]
            ctype = colum['type']
            if i == 0:
                dumplicate_id = meta['dumplicate_id']
                if dumplicate_id and val == '':             #客户端表特殊写法
                    continue
                else:
                    try:
                        id = int(val)
                        if dumplicate_id == False and id in id_set:
                            msg = format("id重复，行:%d,id:%d" % (lineCounter,id))
                            error = True
                            break
                        id_set.add(id)
                        if meta.has_key("max_id"):
                            if id > meta['max_id']:
                                msg = str.format("id超过上限，行%d,max id:%d,id:%d" % (lineCounter,meta['max_id'],id))
                                error = True
                                break
                    except:
                        msg = str.format('id解析错误，行:%d,列:%d' % (lineCounter , (i+1)))
                        error = True
                        break
            else:
                try:
                    if ctype == 'int' or ctype == 'bool':
                        tmp = int(val)
                    elif ctype == 'float':
                        tmp = float(val)
                    elif ctype == 'string':
                        tmp = str(val)
                except:
                    msg = str.format('列解析错误，类型错误，行:%d,列:%d' % (lineCounter , (i+1)))
                    error = True
                    break
            #最后一列不能围字符串，c++端读表器有bug
            if meta['check_last_string_null']:
                col_len = len(vals)
                last_colum = colums[col_len-1]
                last_colum_val = vals[col_len-1]
                if last_colum['type'] == 'string' and str(last_colum_val).strip() == "":
                    msg = str.format('最后一列是字符串时，不能空，行:%d' % (lineCounter))
                    error = True
                    break

            recordCount = recordCount + 1
            if meta.has_key('max_record'):
                if meta['max_record'] < recordCount:
                    msg = str.format('超过最大上限，行:%d,Max_Record:%d,当前:%d' % (lineCounter,meta['max_record'],recordCount))
                    error = True
                    break

        line = fp.readline()
    
    fp.close()
    if error:
        err_msg = str.format("表错误:%s,Error:%s"%(meta['path'],msg ))
        print(err_msg.decode("utf-8"))

def check_dir(dirPath):
    pathDirs =  os.listdir(dirPath)
    for path in pathDirs:
        meta = parse_tab(dirPath + path)
        if not meta is None:
            check(meta)

check_dir("./PublicTables/")
check_dir("./ServerTables/")
check_dir("./ClientTables/")