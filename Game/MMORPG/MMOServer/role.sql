# Host: 127.0.0.1  (Version: 5.6.23-log)
# Date: 2018-06-03 23:30:05
# Generator: MySQL-Front 5.3  (Build 4.205)

/*!40101 SET NAMES utf8 */;

#
# Structure for table "role"
#

CREATE TABLE `role` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(8) NOT NULL DEFAULT '',
  `Level` int(11) unsigned NOT NULL DEFAULT '1',
  `Sex` bit(1) NOT NULL DEFAULT b'0',
  `UserID` int(11) NOT NULL DEFAULT '0',
  `HeadIcon` varchar(30) NOT NULL DEFAULT '3' COMMENT '头像spriteName',
  `Exp` int(11) NOT NULL DEFAULT '0' COMMENT '经验',
  `coin` int(11) NOT NULL DEFAULT '0' COMMENT '金币',
  `YuanBao` int(11) NOT NULL DEFAULT '0' COMMENT '元宝',
  `Energy` int(11) NOT NULL DEFAULT '0' COMMENT '体力',
  `Toughen` int(11) NOT NULL DEFAULT '0' COMMENT '历练',
  `LastDownLine` datetime DEFAULT '0000-00-00 00:00:00' COMMENT '上次下线时间',
  PRIMARY KEY (`Id`),
  KEY `外键` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

#
# Data for table "role"
#

INSERT INTO `role` VALUES (1,'里白吉',100,b'1',4,'3',0,0,0,160,160,'2018-05-30 09:54:17'),(2,'吉百利',100,b'0',4,'3',0,0,4,160,160,'2018-05-30 09:48:25'),(3,'Mlxg',1,b'0',6,'3',0,0,0,0,0,NULL),(4,'立白吉',1,b'1',8,'3',0,0,0,0,0,NULL),(5,'Ming',1,b'0',9,'3',0,0,0,0,0,NULL),(6,'Uzi的卡莎',1,b'0',6,'3',0,0,1,61,61,'2018-05-24 16:08:28');
