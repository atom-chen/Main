

python ProtobufUtils.py -clientPath "..\..\GMTool\GMClient\GMClient\src\NetWork" -r "./PBMessage.proto" ^
-v -all "Template/GMClient/templateCSharp.txt","$clientPath$/ProtoPackets.cs" ^
-f -a -mc "Template/GMClient/templateCSharpHandler.txt","$clientPath$/PacketHandler","$name$_Handler.cs" ^
-f -a -xx "Template/GMClient/templateCSharpHandler.txt","$clientPath$/PacketHandler","$name$_Handler.cs"
pause