


python ProtobufUtils.py -clientPath "../../Robot/Assets/Robot/Script/Network" -r "./PBMessage.proto" ^
-v -all "Template/Robot/templateCSharp.txt","$clientPath$/ProtoPackets.cs" ^
-f -a -gc "Template/Robot/templateCSharpHandler.txt","$clientPath$/PacketHandler","$name$_Handler.cs" ^
-f -a -xx "Template/Robot/templateCSharpHandler.txt","$clientPath$/PacketHandler","$name$_Handler.cs"

python RobotRandomCG.py
pause