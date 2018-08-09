import os

def modify_filename(path):
    for p in os.listdir(path):
        p = os.path.join(path, p)
        if os.path.isdir(p):
            modify_filename(p)
        else:
            if p.endswith(".lua"):
                os.rename(p, os.path.join(os.path.dirname(p), os.path.basename(p) + ".txt"))
            elif p.endswith(".pb"):
                os.rename(p, os.path.join(os.path.dirname(p), os.path.basename(p) + ".bytes"))

modify_filename('../Client/Assets/Game/Lua/Resources/')