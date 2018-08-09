#coding=utf8
__author__ = 'WD'

import sys
import os


def Main():
    packetFile = "PBMessage.proto"
    clientPath = "../../Robot/Assets/Robot/Script/Robot/RobotBehavior_AutoCG.cs"

    path = ""
    curTxt = open(packetFile, "r")
    lines = curTxt.readlines()
    curTxt.close()
    lineindex = 0
    strOutPut = '''using ProtobufPacket;\nusing System;\nusing UnityEngine;\npublic partial class Robot {\n''' + \
        '''\tpublic bool SendRandomPacket(int packetID)\n\t{\n\t\tswitch(packetID)\n\t\t{\n'''
    while True:
        if lineindex >= len(lines):
            break
        curline = lines[lineindex]
        curline = curline.strip('\n').strip('\r')
        if not curline.startswith("message"):
            lineindex += 1
            continue

        outputCount = 0
        while True:
            if lineindex >= len(lines):
                break
            curline = lines[lineindex]
            lineindex += 1
            if curline.startswith("message"):
                strMessage = curline[7:]
                strMessage = strMessage.strip(' ').strip('\n').strip('\r')
                if strMessage.startswith("CG"):
                    strOutPut += "\t\t\tcase ((int)MessageID.PACKETID_" + strMessage + "): \n\t\t\t{\n"
                    strOutPut += "\t\t\t\t" + strMessage + "_PAK packet = new " + strMessage + "_PAK();\n"
                    while True:
                        curMsgLine = lines[lineindex]
                        lineindex += 1
                        if curMsgLine.startswith("}"):
                            break
                        curMsgLine.replace('\t', ' ')
                        curMsgSplits = curMsgLine.split()
                        if len(curMsgSplits) < 5:
                            continue
                        if curMsgSplits[0] != "required" and curMsgSplits[0] != "repeated" and curMsgSplits[0] != "optional":
                            continue
                        strOutPut += GetVarDef(curMsgSplits[0], curMsgSplits[1], curMsgSplits[2])
                    strOutPut += "\t\t\t\tpacket.SendPacket(NetManager);\n"
                    strOutPut += "\n\t\t\t}\n\t\t\tbreak;\n"
            continue
        lineindex += 1
        outputCount += 1

    strOutPut += "\t\t\tdefault:\n\t\t\t\treturn false;"
    strOutPut += "\n\t\t}\n\t\treturn true;\n\t}\n"
    strOutPut += '''\tpublic string GetRandomString()\n\t{\n\t\tstring retStr = "";\n\t\tfor(int i=0; i< UnityEngine.Random.Range(0, 1000); i++)\n\t\t''' + \
        '''{\n\t\t\tretStr += (Char)UnityEngine.Random.Range(0, Char.MaxValue-1);\n\t\t}\n\t\treturn retStr;\n\t}\n'''

    strOutPut += '''\tpublic byte[] GetRandomByte()\n\t{\n''' + \
                 '''\t\tint randomValue = UnityEngine.Random.Range(0, 1000);\n''' + \
                 '''\t\tif(randomValue == 0) {return null;}\n''' + \
                 '''\t\tbyte[] retByte = new byte[randomValue];\n''' + \
                 '''\t\tfor(int i=0; i<randomValue; i++)\n\t\t{\n\t\t\tretByte[i] = (byte)UnityEngine.Random.Range(0, byte.MaxValue - 1);\n\t\t}\n''' + \
                 '''\t\treturn retByte;\n\t}\n'''
    strOutPut += "}\n"
    fileFinal = open(clientPath, "w+")
    fileFinal.write(strOutPut)
    fileFinal.close()

def GetVarDef(varType, varKind, varName):
    if varKind == "uint64":
        strValue = '''(ulong)UnityEngine.Random.Range(0, ulong.MaxValue - 1)'''
    elif varKind == "int32":
        strValue = '''UnityEngine.Random.Range(0, int.MaxValue - 1)'''
    elif varKind == "int64":
        strValue = '''(long)UnityEngine.Random.Range(0, long.MaxValue - 1)'''
    elif varKind == "uint32":
        strValue = '''(uint)UnityEngine.Random.Range(0, uint.MaxValue - 1)'''
    elif varKind == "string":
        strValue = '''GetRandomString()'''
    elif varKind == "float":
        strValue = '''UnityEngine.Random.Range(0, float.MaxValue - 1)'''
    elif varKind == "bool":
        strValue = "UnityEngine.Random.Range(0, 1) == 0"
    elif varKind == "bytes":
        strValue = '''GetRandomByte()'''
    else:
        strValue = "new " + varKind + "()"

    if varType == "repeated":
        retString = '''\t\t\t\tfor(int i=0; i<UnityEngine.Random.Range(0, 100); i++) {\n\t\t\t\t\tpacket.data.''' + varName + ".Add(" + strValue + ");\n\t\t\t\t}\n"
    else:
        retString = '''\t\t\t\tpacket.data.''' + varName + " =" + strValue + ";\n"

    return retString

if __name__ == '__main__':
    Main()