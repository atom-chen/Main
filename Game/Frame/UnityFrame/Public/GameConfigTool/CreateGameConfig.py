# -*- encoding: utf-8 -*-

import os
import sys, time


class VarData:
    def __init__(self, VarDomian, Name, VarName, VarType, VarValue, VarReload):
        self.VarDomian = VarDomian
        self.Name = Name
        self.VarName = VarName
        self.VarType = VarType
        self.VarValue = VarValue
        self.VarReload = VarReload


# 数据类型
typeMap = {"b": ["bool", "false", "m_b"],
           "i": ["int32_t", "0", "m_n"],
           "u": ["uint32_t", "0", "m_u"],
           "f": ["float", "0.0f", "m_f"],
           "s": ["solar::string<128>", "'\\0'", "m_sz"],
           "i2": ["int64_t", "0", "m_n"]}

# 需要过滤的ini文件名
filterFileNameList = ["GMConfig"]


def ContainSpace(Str):
    if Str:
        for i in Str:
            if i.isspace():
                return 1
    return 0


def find_in_list(myList, value):
    pos = -1
    if myList:
        for v in range(0, len(myList)):
            if value == myList[v]:
                pos = v
                break
    return pos


def GeneratePakIncFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 2
    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$pakInc_end$"):
            break
        strout += repeatLine
    return strout, 2


def GeneratePakResetFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 3

    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$pakReset_end$"):
            break
        strout += repeatLine
    return strout, 3


def GenerateMsgIncFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 2
    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$msgInc_end$"):
            break
        strout += repeatLine
    return strout, 2


def GenerateMsgResetFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 3

    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$msgReset_end$"):
            break
        strout += repeatLine
    return strout, 3


def GenerateRoutineIncFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 2
    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$routineInc_end$"):
            break
        strout += repeatLine
    return strout, 2


def GenerateRoutineResetFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 3

    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$routineReset_end$"):
            break
        strout += repeatLine
    return strout, 3


def GenerateRelushIncFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 4
    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$reflushInc_end$"):
            break
        strout += repeatLine
    return strout, 4


def GenerateRelushBroadFromTemplate(readLines, readIndex, fileName):
    if (fileName != 'GameConfig'):
        return "", 4

    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$reflushBroad_end$"):
            break
        strout += repeatLine
    return strout, 4


def GenerateFileNameFromTemplate(readLines, readIndex, fileName):
    strout = ""
    repeatIndex = 0
    while True:
        curLineIndex = repeatIndex + readIndex
        if curLineIndex >= len(readLines):
            break
        repeatLine = readLines[curLineIndex]
        repeatIndex += 1
        if repeatLine.startswith("$fileName_end$"):
            break
        repeatLine = repeatLine.replace("$fileName$", fileName)
        repeatLine = repeatLine.replace("$fileDefine$", fileName.upper())
        strout += repeatLine
    return strout, repeatIndex


def GenerateHVarFromTemplate(VarList, readLines, readIndex):
    strout = ""
    repeatIndex = 0
    global typeMap
    for curVar in VarList:
        varName = curVar.VarName
        typeKey = curVar.VarType
        varType = typeMap[typeKey][0]
        repeatIndex = 0
        while True:
            curLineIndex = repeatIndex + readIndex
            if curLineIndex >= len(readLines):
                break
            repeatLine = readLines[curLineIndex]
            repeatIndex += 1
            if repeatLine.startswith("$decl_end$"):
                break
            repeatLine = repeatLine.replace("$var_type$", varType)
            repeatLine = repeatLine.replace("$var_name$", varName)
            strout += repeatLine
    return strout, repeatIndex


def GenerateHFromTemplate(VarList, templatePath, outputPath, fileName):
    curTxt = open(templatePath, "r")
    readLines = curTxt.readlines()
    curTxt.close()

    strOutput = ""
    readIndex = 0
    while True:
        if readIndex >= len(readLines):
            break
        curLine = readLines[readIndex]
        readIndex += 1

        # repatType 0-define
        repeatType = -1
        if curLine.startswith("$fileName_begin$"):
            repeatType = 0
        elif curLine.startswith("$decl_begin$"):
            repeatType = 1

        if repeatType == 0:
            str, repeatIndex = GenerateFileNameFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex
        elif repeatType == 1:
            str, repeatIndex = GenerateHVarFromTemplate(VarList, readLines, readIndex)
            strOutput += str
            readIndex += repeatIndex
        else:
            strOutput += curLine

    fileFinal = open(outputPath, "w+")
    fileFinal.write(strOutput)
    fileFinal.close();


def GenerateCppDefaultFromTemplate(VarList, readLines, readIndex):
    str = ""
    repeatIndex = 2
    global typeMap
    for curVar in VarList:
        varName = curVar.VarName
        typeKey = curVar.VarType
        # if typeKey == "s":
        #	continue

        typeDefault = typeMap[typeKey][1]
        repeatIndex = 0
        while True:
            curLineIndex = repeatIndex + readIndex
            if curLineIndex >= len(readLines):
                break
            repeatLine = readLines[curLineIndex]
            repeatIndex += 1
            if repeatLine.startswith("$default_end$"):
                break
            repeatLine = repeatLine.replace("$var_name$", varName)
            repeatLine = repeatLine.replace("$default$", typeDefault)
            str += repeatLine
    return str, repeatIndex


def GenerateCppLoadFromTemplate(VarList, readLines, readIndex, isReload):
    str = ""
    repeatLines = 0

    while True:
        if readIndex >= len(readLines):
            break;
        curLine = readLines[readIndex]
        readIndex += 1
        repeatLines += 1

        if curLine.startswith("$load_end$") or curLine.startswith("$reload_end$"):
            break

        # repeatFieldType :
        repeatFieldType = -1

        repeatIndex = 2
        if curLine.startswith("$int32_begin$"):
            repeatFieldType = 0
        elif curLine.startswith("$int64_begin$"):
            repeatFieldType = 1
        elif curLine.startswith("$uint_begin$"):
            repeatFieldType = 2
        elif curLine.startswith("$bool_begin$"):
            repeatFieldType = 3
        elif curLine.startswith("$float_begin$"):
            repeatFieldType = 4
        elif curLine.startswith("$char_begin$"):
            repeatFieldType = 5
            if isReload:
                repeatIndex = 3

        if repeatFieldType >= 0:
            for curVar in VarList:
                if repeatFieldType == 0 and curVar.VarType != "i":
                    continue
                elif repeatFieldType == 1 and curVar.VarType != "i2":
                    continue
                elif repeatFieldType == 2 and curVar.VarType != "u":
                    continue
                elif repeatFieldType == 3 and curVar.VarType != "b":
                    continue
                elif repeatFieldType == 4 and curVar.VarType != "f":
                    continue
                elif repeatFieldType == 5 and curVar.VarType != "s":
                    continue

                if isReload and curVar.VarReload != 1:
                    continue

                repeatIndex = 0
                while True:
                    curLineIndex = repeatIndex + readIndex
                    if curLineIndex >= len(readLines):
                        break
                    repeatLine = readLines[curLineIndex]
                    repeatIndex += 1
                    if repeatLine.startswith("$int32_end$") or repeatLine.startswith("$int64_end$") or \
                            repeatLine.startswith("$uint_end$") or repeatLine.startswith("$bool_end$") or \
                            repeatLine.startswith("$float_end$") or repeatLine.startswith("$char_end$"):
                        break

                    repeatLine = repeatLine.replace("$domian$", curVar.VarDomian)
                    repeatLine = repeatLine.replace("$name$", curVar.Name)
                    repeatLine = repeatLine.replace("$var_name$", curVar.VarName)
                    str += repeatLine
            repeatLines += repeatIndex
            readIndex += repeatIndex
        else:
            str += curLine

    return str, repeatLines


def GenerateCppFromTemplate(VarList, templatePath, outputPath, fileName):
    curTxt = open(templatePath, "r")
    readLines = curTxt.readlines()
    curTxt.close()

    strOutput = ""
    readIndex = 0
    while True:
        if readIndex >= len(readLines):
            break
        curLine = readLines[readIndex]
        readIndex += 1

        # repatType -1 all
        repeatType = -1

        isReload = False
        if curLine.startswith("$fileName_begin$"):
            repeatType = 0
        elif curLine.startswith("$default_begin$"):
            repeatType = 1
        elif curLine.startswith("$pakInc_begin$"):
            repeatType = 2
        elif curLine.startswith("$pakReset_begin$"):
            repeatType = 3
        elif curLine.startswith("$msgInc_begin$"):
            repeatType = 4
        elif curLine.startswith("$msgReset_begin$"):
            repeatType = 5
        elif curLine.startswith("$routineInc_begin$"):
            repeatType = 6
        elif curLine.startswith("$routineReset_begin$"):
            repeatType = 7
        elif curLine.startswith("$reflushInc_begin$"):
            repeatType = 8
        elif curLine.startswith("$reflushBroad_begin$"):
            repeatType = 9
        elif curLine.startswith("$load_begin$"):
            repeatType = 10
        elif curLine.startswith("$reload_begin$"):
            repeatType = 11
            isReload = True

        if repeatType >= 10:
            str, repeatIndex = GenerateCppLoadFromTemplate(VarList, readLines, readIndex, isReload)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 9:
            str, repeatIndex = GenerateRelushBroadFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 8:
            str, repeatIndex = GenerateRelushIncFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 7:
            str, repeatIndex = GenerateRoutineResetFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 6:
            str, repeatIndex = GenerateRoutineIncFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 5:
            str, repeatIndex = GenerateMsgResetFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 4:
            str, repeatIndex = GenerateMsgIncFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 3:
            str, repeatIndex = GeneratePakResetFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 2:
            str, repeatIndex = GeneratePakIncFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 1:
            str, repeatIndex = GenerateCppDefaultFromTemplate(VarList, readLines, readIndex)
            strOutput += str
            readIndex += repeatIndex;
        elif repeatType == 0:
            str, repeatIndex = GenerateFileNameFromTemplate(readLines, readIndex, fileName)
            strOutput += str
            readIndex += repeatIndex;
        else:
            strOutput += curLine

    fileFinal = open(outputPath, "w+")
    fileFinal.write(strOutput)
    fileFinal.close();


def GenerateConifgHFromTemplate(configList, tempConfigHPath, outputPath):
    curTxt = open(tempConfigHPath, "r")
    readLines = curTxt.readlines()
    curTxt.close()

    strOutput = ""
    readIndex = 0
    while True:
        if readIndex >= len(readLines):
            break
        curLine = readLines[readIndex]
        readIndex += 1

        # repatType 0-define
        repeatType = -1
        if curLine.startswith("$fileName_begin$"):
            repeatType = 0

        if repeatType == 0:
            repeatIndex = 0
            for configName in configList:
                repeatIndex = 0
                while True:
                    curLineIndex = repeatIndex + readIndex
                    if curLineIndex >= len(readLines):
                        break
                    repeatLine = readLines[curLineIndex]
                    repeatIndex += 1
                    if repeatLine.startswith("$fileName_end$"):
                        break
                    repeatLine = repeatLine.replace("$fileName$", configName)
                    strOutput += repeatLine
            readIndex += repeatIndex
        else:
            strOutput += curLine

    fileFinal = open(outputPath, "w+")
    fileFinal.write(strOutput)
    fileFinal.close();


def onefile(fileName):
    fin = open(fileName, "r")
    lines = fin.readlines()
    fin.close()

    VarList = []
    configMap = {}
    domian = ""
    lineindex = 0
    global typeMap

    while True:
        if lineindex >= len(lines):
            break

        curLine = lines[lineindex]
        lineindex += 1

        curLine = curLine.strip('\n').strip('\r').strip()
        if not curLine:
            continue

        if curLine.startswith("[") and curLine.endswith("]"):
            domian = curLine.strip('[').strip(']').strip()
            continue

        if not curLine.startswith("[") and not curLine.endswith("]"):
            strlist = curLine.split(';')
            if len(strlist) < 4:
                print "Error:Statement format is not correct!\nFileName:", fileName, "\nLineNo:", lineindex ,"\nLineInfo:", curLine
                return []

            keyInfo = strlist[0].strip().split('=')
            if len(keyInfo) != 2:
                print "Error:Statement format is not correct!\nFileName:", fileName, "\nLineNo:", lineindex ,"\nLineInfo:", curLine
                return []

            key = keyInfo[0]
            value = keyInfo[1]
            # key,value中不能包括空格
            if ContainSpace(key) == 1 or ContainSpace(value) == 1:
                print "Error:contains space!\nFileName:", fileName,"\nLineNo:", lineindex ,"\nLineInfo:", curLine
                return []
            if configMap.has_key(key):
                print "Error:field has already define!\nFileName:", fileName,"\nLineNo:", lineindex , "\nLineInfo:", curLine
                return []

            type = strlist[1].strip()
            if not typeMap.has_key(type):
                print "Error:type is null or incorrect!\nFileName:", fileName,"\nLineNo:", lineindex , "LineInfo:", curLine
                return []

            reloadStr = strlist[2].strip()
            if not reloadStr or not reloadStr.isdigit():
                print "Error:reloadType incorrect! \nFileName:", fileName, "\nLineNo:", lineindex ,"LineInfo:", curLine
                return []
            reloadFlag = int(reloadStr)
            if not reloadFlag == 0 and not reloadFlag == 1:
                print "Error:reloadValue incorrect! \nFileName:", fileName,"\nLineNo:", lineindex , "LineInfo:", curLine
                return []

            comment = strlist[3].strip()
            type_tag = typeMap[type][2]

            if domian and key and value and type:
                # print "Read Ok!:",fileName,"LineInfo:",curLine
                pass
            else:
                print "Error:domian or field or value or type is null !\nFileName:", fileName, "LineInfo:", curLine
                return []

            newVarData = VarData(domian, key, type_tag + key, type, value, reloadFlag)
            VarList.append(newVarData)
            configMap[key] = newVarData

            continue
        print "Error:[] Don't match! \nFileName:", fileName, "\nLineInfo:", curLine
        return []
    return VarList


def main(foldername):
    global filterFileNameList

    tempCppPath = os.getcwd() + "/Template/template_cpp.txt"
    tempHPath = os.getcwd() + "/Template/template_h.txt"
    # print "tempCppPath:",tempCppPath,",tempHPath:",tempHPath

    configList = []
    filelists = os.listdir(foldername)
    for afile in filelists:
        if afile.endswith(".ini"):
            fileName = afile.strip(".ini").strip()
            configList.append(fileName)

            # 需要自动生成对应的XxxConfig.h和XxxConfig.cpp文件
            if find_in_list(filterFileNameList, afile.strip(".ini").strip()) < 0:
                # generate configFile
                VarList = onefile("%s/%s" % (foldername, afile))
                # generate code
                assert VarList
                if len(VarList) == 0:
                    continue
                tmpHPath = os.getcwd() + "/tmp/cpp/" + fileName + ".h"
                tmpCppPath = os.getcwd() + "/tmp/cpp/" + fileName + ".cpp"
                GenerateHFromTemplate(VarList, tempHPath, tmpHPath, fileName)
                GenerateCppFromTemplate(VarList, tempCppPath, tmpCppPath, fileName)

    if len(configList) == 0:
        return
    tempConfigHPath = os.getcwd() + "/Template/templateConfigCpp.txt"
    tmpConbfigHPath = os.getcwd() + "/tmp/cpp/Config.cpp"
    GenerateConifgHFromTemplate(configList, tempConfigHPath, tmpConbfigHPath)


if __name__ == '__main__':
    print "\n"
    print '*' * 60
    if not len(sys.argv) == 2:
        print"\narg Error! usage: python CreateGameConfig.py [iniFolder]\n"
    elif not os.path.exists(sys.argv[1]):
        print"No this file:", sys.argv[1]
    elif not os.path.isdir(sys.argv[1]):
        print"No a folder:", sys.argv[1]
    else:
        try:
            main(sys.argv[1])
        except:
            print '\n\n====>ERROR! '
            print '*' * 60
            time.sleep(5)
