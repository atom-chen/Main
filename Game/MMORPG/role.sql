# Host: 127.0.0.1  (Version: 5.0.22-community-nt)
# Date: 2018-07-19 22:35:42
# Generator: MySQL-Front 5.3  (Build 4.205)

/*!40101 SET NAMES utf8 */;

#
# Structure for table "role"
#

DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `Id` int(11) NOT NULL auto_increment,
  `name` varchar(8) NOT NULL default '',
  `Level` int(11) unsigned NOT NULL default '1',
  `Sex` bit(1) NOT NULL default '\0',
  `UserID` int(11) NOT NULL default '0',
  `HeadIcon` varchar(30) NOT NULL default '3',
  `Exp` int(11) NOT NULL default '0',
  `coin` int(11) NOT NULL default '0',
  `YuanBao` int(11) NOT NULL default '0',
  `Energy` int(11) NOT NULL default '0',
  `Toughen` int(11) NOT NULL default '0',
  `LastDownLine` datetime default '0000-00-00 00:00:00',
  `鏂板瓧娈礰 geometrycollection NOT NULL,
  PRIMARY KEY  (`Id`),
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# Data for table "role"
#

INSERT INTO `role` VALUES (1,'里白吉',100,b'1',4,'3',0,0,0,160,160,'2018-05-30 09:54:17',X''),(2,'???',100,b'0',4,'3',0,0,4,160,160,'2018-06-04 16:34:29',X''),(3,'Mlxg',1,b'0',6,'3',0,0,0,0,0,NULL,X''),(4,'立白吉',1,b'1',8,'3',0,0,0,0,0,NULL,X''),(5,'Ming',1,b'0',9,'3',0,0,0,0,0,NULL,X''),(6,'Uzi的卡莎',1,b'0',6,'3',0,0,1,61,61,'2018-05-24 16:08:28',X'');
