# -*- coding: utf-8 -*-
import os
import re
import codecs
import shutil
from jinja2 import Environment, PackageLoader, select_autoescape
from enums import enums


settings = {
    'lua' : {
        'output_path' : "Public/LuaScript/BattleCore/Common/",
        'ends' : "lua",
        'temp_ends' : 'lua',
        'single_enum_file' : 'Enums',
    },
    'csharp' : {
        'output_path' : "Client/Assets/Game/Script/GenCode/",
        'ends' : "cs",
        'temp_ends' : 'cs',
        'single_enum_file' : "Global_Enums",
        'tab_reader_file' : "LuaTabReader",
    },
    'cplus' : {
        'output_path' : "./",
        'ends' : "cpp",
        'temp_ends' : "cpp",
        'tab_reader_file' : "LuaTabReader",
        'single_enum_file' : "Global_Define",
    },
}
 
#需要生成读表器的表
export_tabs = [
    'SkillEx',
    'SkillBase',
    'SkillHit',
    'Impact',
    'Battle',
    'BattleWave',
    'RoleBaseAttr',
    'RoleAttrEx',
    'RoleAttrGrowUp',
    'Monster',
    'BattleCutscene',
    'AI', 
    "SkillAI",
    "SummonMonster",
    "SkillLevels",
    "BattleBubbleCard",
    "SkillParams",
]

root = "../../"
table_path = "../PublicTables/"

env = Environment(
    loader=PackageLoader('main', 'templates', 'utf-8'),
    autoescape=select_autoescape(['j2'])
)

def gen_enums(lang,arg):
    cfg = settings[lang]
    temp_enum = env.get_template('enum' + "_" + arg  + "_" + cfg['temp_ends'] + '.j2')
    fdir = root + cfg['output_path']
    if os.path.exists(fdir) and arg == 'm':
        shutil.rmtree(fdir)
        os.mkdir(fdir)
    else:
        if not os.path.exists(fdir):
            os.mkdir(fdir)
    
    filterByNoGen = []
    for enum in enums:
        if not (enum.has_key('no_gen') and (lang in enum['no_gen'])):
            filterByNoGen.append(enum)

    if arg == 'm':
        for enum in filterByNoGen:
            path = fdir + enum['name'] + '.' + cfg['ends']
            fp = codecs.open(path,'wb','utf-8')
            fp.write(temp_enum.render(enum = enum))
            fp.close()
            print("生成枚举成功:" + path)
    else:
        path = fdir + cfg['single_enum_file'] + "." + cfg['ends']
        fp = codecs.open(path,'wb','utf-8') 
        fp.write(temp_enum.render(enums = filterByNoGen))
        fp.close()
        print("生成枚举成功:" + path)

def gen_tab_reader(lang):
    tabs = parse_tabs()
    cfg = settings[lang]
    temp = env.get_template('tab_reader' + "_" + cfg['temp_ends'] + '.j2')
    fdir = root + cfg['output_path']
    if not os.path.exists(fdir):
        os.mkdir(fdir)
    
    path = fdir + cfg['tab_reader_file'] + "." + cfg['ends']
    fp = codecs.open(path,'wb','utf-8') 
    fp.write(temp.render(tabs = tabs))
    fp.close()
    print('生成成功读表器:' + path)

def parse_tabs():
    tabs = []

    for tab_name in export_tabs:
        tab = {
            'name' : tab_name,
            'colums' : [],
        }
        path = table_path + tab_name + '.txt'
        fp = open(path,'r')
        colum_names = fp.readline().strip().split('\t')
        colum_types = fp.readline().strip().split('\t')
        fp.readline()
        colum_desc = fp.readline().strip().split('\t')

        l1,l2,l3 = len(colum_names),len(colum_types),len(colum_desc)
        if l1 != l2 or l1 != l3 or l2 != l3:
            print('表错误:' + tab_name)
            continue

        listMap = []
        for i in range(0,l1):
            ctype = colum_types[i].lower()
            #字符串不导出
            if ctype == 'string':
                continue
            cname = colum_names[i]
            #匹配 xxx_123  xx123
            m = re.match(r'([a-zA-Z]+)_?[0-9]+',cname)
            is_list = False
            new_colum = True
            if m:
                cname = m.group(1)
                is_list = True
                if cname in listMap:
                    new_colum = False
                else:
                    listMap.append(cname)
            
            cdesc = colum_desc[i].decode('gbk').encode('utf-8')
            #print(cdesc)
            if new_colum:
                tab['colums'].append({
                    'name' : cname,
                    'type' : ctype,
                    'comment' : unicode(cdesc,'utf-8'),
                    'is_list' : is_list,
                })
        
        fp.close()
        tabs.append(tab)

    return tabs

gen_enums('lua','m')
gen_enums('csharp','s')
gen_tab_reader('csharp')

# gen_tab_reader('cplus')
# gen_enums("cplus",'s')
