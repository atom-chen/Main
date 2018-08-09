


python ProtobufUtils.py -clientPath "../../Client/Assets/Game/Script/GlobalSystem/NetWork" -r "./PBMessage.proto" ^
-v -all "Template/GameClient/templateCSharp.txt","$clientPath$/ProtoPackets.cs" ^
-f -a -gc "Template/GameClient/templateCSharpHandler.txt","$clientPath$/PacketHandler","$name$_Handler.cs" ^
-f -a -xx "Template/GameClient/templateCSharpHandler.txt","$clientPath$/PacketHandler","$name$_Handler.cs"
pause