# -*- coding: utf-8 -*-
import shutil
import sys
import os

ignores = [
    'startup.lua','startup_server.lua',"hotfix.lua",
    'BattleCore.lua',
    'protobuf.lua',
    'serpent.lua',
    'BattleSimulator.lua',
    'Random.lua',
    'class.lua',
]

def GetAllLua(dir):
    all = []
    for root,dirs,files in os.walk(dir):
        for f in files:
            if (f.endswith(".lua")) and (f not in(ignores)):
                path = (os.path.join(root,f))
                path = path.replace("\\","/")
                path = path.replace(dir,"")
                path = path.replace('.lua',"")
                all.append(path)
    return all

if __name__ == '__main__':
    #print(sys.argv[0])
    print(sys.argv[1],sys.argv[2])
    dir = sys.argv[1]
    out = sys.argv[2]

    scripts = GetAllLua(dir)
    tab = open(out,"w")
    tab.write("Id\tDescription\tPath\n")
    tab.write("INT\tSTRING\tSTRING\n")
    tab.write("#MAX_ID=99999;MAX_RECORD=4096;TableType=Hash;\n")
    tab.write("#id\tdesc\tpath\n")
    id = 1
    for script in scripts:
        tab.write("{0}\t\t{1}\n".format(id,script))
        id = id + 1
    tab.close()