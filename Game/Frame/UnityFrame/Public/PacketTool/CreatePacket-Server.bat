


python ProtobufUtils.py -serverPath "..\..\..\..\..\Server\Branches\Main\Server\Public\Packet" -r "./PBMessage.proto" ^
-v -cg "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/CG_Common_PAK.cpp" ^
-v -gc "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/GC_Common_PAK.cpp" ^
-v -cm "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/CM_Common_PAK.cpp" ^
-v -mc "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/MC_Common_PAK.cpp" ^
-v -gm "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/GM_Common_PAK.cpp" ^
-v -mg "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/MG_Common_PAK.cpp" ^
-v -hg "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/HG_Common_PAK.cpp" ^
-v -dg "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/DG_Common_PAK.cpp" ^
-v -wg "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/WG_Common_PAK.cpp" ^
-v -gh "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/GH_Common_PAK.cpp" ^
-v -gd "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/GD_Common_PAK.cpp" ^
-v -gw "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/GW_Common_PAK.cpp" ^
-v -xx "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/XX_Common_PAK.cpp" ^
-v -bo "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/BO_Common_PAK.cpp" ^
-v -ob "Template/GameServer/templatePacketCC.txt","$serverPath$/Packet/OB_Common_PAK.cpp" ^
-v -cg "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/CG_Common_Handler.cpp" ^
-v -gc "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/GC_Common_Handler.cpp" ^
-v -cm "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/CM_Common_Handler.cpp" ^
-v -mc "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/MC_Common_Handler.cpp" ^
-v -gm "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/GM_Common_Handler.cpp" ^
-v -mg "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/MG_Common_Handler.cpp" ^
-v -hg "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/HG_Common_Handler.cpp" ^
-v -dg "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/DG_Common_Handler.cpp" ^
-v -wg "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/WG_Common_Handler.cpp" ^
-v -gh "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/GH_Common_Handler.cpp" ^
-v -gd "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/GD_Common_Handler.cpp" ^
-v -gw "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/GW_Common_Handler.cpp" ^
-v -xx "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/XX_Common_Handler.cpp" ^
-v -bo "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/BO_Common_Handler.cpp" ^
-v -ob "Template/GameServer/templatePacketHandler.txt","$serverPath$/PacketHandler/OB_Common_Handler.cpp" ^
-v -all "Template/GameServer/templatePacketDefineH.txt","$serverPath$/Packet/PacketDefine.h" ^
-v -all "Template/GameServer/templatePacketDefineCpp.txt","$serverPath$/Packet/PacketDefine.cpp" ^
-v -all "Template/GameServer/templatePacketFactory.txt","$serverPath$/Packet/PacketFactoryManager.cpp" ^
-f -w -all "Template/GameServer/templatePacketH.txt","$serverPath$/Packet/","$name$_PAK.h"
pause