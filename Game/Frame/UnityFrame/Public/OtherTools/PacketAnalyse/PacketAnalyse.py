from __future__ import division
import re
import os

from collections import defaultdict
from heapq import nlargest

def ScanFiles(path): #fun
	for dirs,subdir,files in os.walk(path):
		for fn in files:
			if fn.find("PacketStat") >= 0:
				yield fn

def PrintToppestInfo(dict, topIndex, OutPutFile, printName):
	Toppest_List = nlargest(topIndex, dict.items(),key=lambda x: x[1]);
	print("Toppest_"+ printName +": ")
	OutPutFile.write("Toppest_"+printName+": \n");
	OutPutFile.write("RankIndex PacketName "+printName+"\n");
	index = 1;
	for item in Toppest_List:
			print(index, item[0],item[1])
			OutPutFile.write(str(index)+" "+str(item[0])+" "+str(item[1])+"\n");
			index += 1;
	OutPutFile.write("\n");
	return;

receivecountDict = defaultdict(int); 
receivesizeDict = defaultdict(int);	
sendcountDict = defaultdict(int);		
sendsizeDict = defaultdict(int);
pakcetNameDict = defaultdict(str);
packetSizeDict = defaultdict(int);

OutPutFile = open("./PacketLogAnalyseResult.txt", "w");

TopIndex = 10;
print(os.getcwd())
with open(os.getcwd()+"/Config.txt") as configFile:
	for configline in configFile:
			TopIndex = (int)(configline);
			print(TopIndex);

allFiles = list(ScanFiles(os.getcwd()))
for lfile in allFiles:
	with open(lfile) as logfile:
		for line in logfile:
			if line.find("CG_")>=0 or line.find("GC_")>=0 or line.find("XX_")>=0:
				PacketName, szreceivecount, szreceivesize, szsendcount, szsendsize = line.split();
				receivecount = (int)(szreceivecount);
				receivesize = (int)(szreceivesize);
				sendcount = (int)(szsendcount);
				sendsize = (int)(szsendsize);
				receivecountDict[PacketName] += receivecount;
				receivesizeDict[PacketName] += receivesize;
				sendcountDict[PacketName] += sendcount;
				sendsizeDict[PacketName] += sendsize;
				pakcetNameDict[PacketName] = PacketName;
				if receivecount > 0 and receivesize >0:
					packetSizeDict[PacketName] = receivesize/receivecount;
				elif sendcount >0 and sendsize >0:
					packetSizeDict[PacketName] = sendsize/sendcount;


OutPutFile.write("Toppest "+str(TopIndex)+" OutPut: \n");

PrintToppestInfo(receivecountDict,TopIndex, OutPutFile, "receivecount");

PrintToppestInfo(receivesizeDict,TopIndex, OutPutFile, "receivesize");

PrintToppestInfo(sendcountDict,TopIndex, OutPutFile, "sendcount");

PrintToppestInfo(sendsizeDict,TopIndex, OutPutFile, "sendsize");

PrintToppestInfo(packetSizeDict,TopIndex, OutPutFile, "packetsize");

OutPutFile.close();