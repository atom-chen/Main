# Host: 127.0.0.1  (Version: 5.6.23-log)
# Date: 2018-06-03 23:30:18
# Generator: MySQL-Front 5.3  (Build 4.205)

/*!40101 SET NAMES utf8 */;

#
# Structure for table "serverpropert"
#

CREATE TABLE `serverpropert` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IP` varchar(16) NOT NULL DEFAULT '127.0.0.1',
  `Name` varchar(30) NOT NULL DEFAULT '',
  `Count` int(11) NOT NULL DEFAULT '20',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

#
# Data for table "serverpropert"
#

INSERT INTO `serverpropert` VALUES (1,'192.168.2.102','火云洞',20),(2,'192.168.2.102','盘丝洞',50),(3,'192.168.2.102','魔王寨',50),(4,'192.168.2.102','狮驼岭',50),(5,'192.168.2.102','大唐官府',2000),(6,'192.168.2.102','化生寺',20),(7,'192.168.2.102','女儿村',90),(8,'192.168.2.102','方寸山',10);
