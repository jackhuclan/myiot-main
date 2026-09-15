-- MySQL dump 10.13  Distrib 5.7.42, for Linux (x86_64)
--
-- Host: 192.168.102.253    Database: vg_autodrill_db
-- ------------------------------------------------------
-- Server version	5.7.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `sys_app_secret`
--

DROP TABLE IF EXISTS `sys_app_secret`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_app_secret` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `app_id` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '应用Id',
  `app_secret` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '应用密钥',
  `app_code` varchar(20) CHARACTER SET utf8 NOT NULL COMMENT '应用Code(唯一值)',
  `app_name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '应用名',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_app_secret`
--

LOCK TABLES `sys_app_secret` WRITE;
/*!40000 ALTER TABLE `sys_app_secret` DISABLE KEYS */;
INSERT INTO `sys_app_secret` VALUES (1,'CA6D50S9','0cc773086abb599f38432fca96c767c621200694','DRILLAMIN','智慧工厂自动化平台',0,1,0,'2022-12-05 10:40:05','2023-03-23 02:53:24',1);
/*!40000 ALTER TABLE `sys_app_secret` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_config`
--

DROP TABLE IF EXISTS `sys_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_config` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `config_code` varchar(100) NOT NULL COMMENT '配置项编号',
  `config_value` varchar(100) NOT NULL COMMENT '配置项的值',
  `config_type` varchar(50) DEFAULT NULL COMMENT '配置项的类型',
  `config_descript` varchar(500) DEFAULT '' COMMENT '配置项值描述',
  `config_enum_value` varchar(100) DEFAULT NULL COMMENT '枚举配置项值',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `is_system` int(11) DEFAULT '0' COMMENT '是否系统自带',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`config_code`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COMMENT='系统配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_config`
--

LOCK TABLES `sys_config` WRITE;
/*!40000 ALTER TABLE `sys_config` DISABLE KEYS */;
INSERT INTO `sys_config` VALUES (1,'CentralControlSystemIsMaintain','false','bool','中控系统是否在维护',NULL,'中控系统是否在维护',0,1,1,'2023-10-18 15:35:48','2023-11-26 10:25:02',1,NULL);
INSERT INTO `sys_config` VALUES (2,'CentralControlSystemCreatedTimeout','300','int','中控系统已上报调度的超时设定(秒)',NULL,'中控系统已上报调度的超时设定(秒)',0,1,1,'2023-10-18 15:37:29','2023-10-27 16:43:16',57,1);
INSERT INTO `sys_config` VALUES (3,'CentralControlSystemAllocatedTimeout','300','int','中控系统已分配的调度的超时设定(秒)',NULL,'中控系统已分配的调度的超时设定(秒)',0,1,1,'2023-10-18 15:38:04','2023-10-27 16:43:25',57,1);
INSERT INTO `sys_config` VALUES (21,'AGVDeviceNowStatus','1','enum','已离线','-1,未知;0,在线;1,已离线;2,待机;3,工作中;4,故障;5,低电量;6,充电中;7,设备维护;8,停机','AGV当前状态',0,1,1,'2023-10-19 11:40:10','2023-10-19 15:43:45',1,1);
INSERT INTO `sys_config` VALUES (22,'IsShowTimelyInformation','false','bool','是否显示告警等及时信息',NULL,'是否显示告警等及时信息',0,1,1,'2023-10-19 11:40:10','2023-11-26 10:41:56',1,NULL);
INSERT INTO `sys_config` VALUES (23,'AGVLatestScheduleCount','5','int','AGV最近调度记录条数',NULL,'AGV最近调度记录条数',0,1,1,'2023-11-09 11:40:10',NULL,NULL,1);
INSERT INTO `sys_config` VALUES (24,'DrillTaskShaftCount','6','int','钻孔任务轴数',NULL,'钻孔任务轴数',0,1,1,'2023-11-13 11:40:10','2023-11-13 16:35:27',1,NULL);
INSERT INTO `sys_config` VALUES (25,'DrillDeviceTaskShowCount','100','int','在线钻机待做任务显示个数',NULL,'在线钻机待做任务显示个数',0,1,1,'2023-11-13 11:40:10',NULL,NULL,1);
INSERT INTO `sys_config` VALUES (26,'AGVDeviceSiloInfoShowCount','20','int','在线AGV板料信息及最近调度记录显示个数',NULL,'在线AGV板料信息及最近调度记录显示个数',0,1,1,'2023-11-13 11:40:10',NULL,NULL,1);
/*!40000 ALTER TABLE `sys_config` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_department`
--

DROP TABLE IF EXISTS `sys_department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_department` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `department_name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '部门名称',
  `parent_id` int(11) NOT NULL COMMENT '父部门Id(0表示是根部门)',
  `telephone` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '电话/手机',
  `email` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '邮箱',
  `qq` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT 'QQ',
  `leader` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '负责人ID',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='部门';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_department`
--

LOCK TABLES `sys_department` WRITE;
/*!40000 ALTER TABLE `sys_department` DISABLE KEYS */;
INSERT INTO `sys_department` VALUES (1,'维嘉科技',0,'15888888888','boss@qq.com',NULL,'张三',0,1,0,'2022-09-26 11:05:59','2023-08-29 16:10:05',1);
INSERT INTO `sys_department` VALUES (2,'深圳总公司',1,'15888888888','boss@qq.com',NULL,'王五',0,1,0,'2022-09-26 11:06:00','2023-07-28 10:57:53',1);
INSERT INTO `sys_department` VALUES (13,'研发部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:57:35','2023-10-24 08:48:51',1);
INSERT INTO `sys_department` VALUES (14,'市场部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:58:27',NULL,NULL);
INSERT INTO `sys_department` VALUES (15,'运营部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:59:14',NULL,NULL);
INSERT INTO `sys_department` VALUES (16,'财务部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:59:27',NULL,NULL);
INSERT INTO `sys_department` VALUES (18,'qqqqqqqqqq',18,'13909877869','13400000001@qq.com',NULL,'qqqq',0,1,1,'2023-07-28 15:37:21','2023-07-28 15:37:48',1);
INSERT INTO `sys_department` VALUES (23,'出纳',16,'18913742716','120940367@qq.com',NULL,'frr',0,1,1,'2023-08-07 09:09:39','2023-08-07 09:10:40',1);
/*!40000 ALTER TABLE `sys_department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_dic_data`
--

DROP TABLE IF EXISTS `sys_dic_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_dic_data` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `dic_name` varchar(25) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `dic_code` varchar(25) CHARACTER SET utf8 DEFAULT NULL COMMENT '字典Key',
  `dic_value` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '字典值',
  `sort` int(11) DEFAULT NULL COMMENT '排序',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='字典';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_dic_data`
--

LOCK TABLES `sys_dic_data` WRITE;
/*!40000 ALTER TABLE `sys_dic_data` DISABLE KEYS */;
/*!40000 ALTER TABLE `sys_dic_data` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_menu`
--

DROP TABLE IF EXISTS `sys_menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_menu` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `app_code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '应用Code',
  `menu_name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '菜单名称',
  `parent_id` int(11) NOT NULL DEFAULT '0' COMMENT '父菜单Id(0表示是根菜单)',
  `menu_icon` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '菜单图标',
  `path` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '路由地址',
  `menu_url` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '菜单Url',
  `menu_type` int(11) DEFAULT NULL COMMENT '菜单类型(1目录 2页面 3按钮)',
  `authorize` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '菜单权限标识',
  `remark` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '备注',
  `sort` int(11) DEFAULT NULL COMMENT '排序',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1472 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='菜单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu`
--

LOCK TABLES `sys_menu` WRITE;
/*!40000 ALTER TABLE `sys_menu` DISABLE KEYS */;
INSERT INTO `sys_menu` VALUES (1,'DRILLAMIN','系统管理',0,'system','system','#',1,NULL,NULL,100,0,1,0,'2022-09-26 14:56:09','2023-04-01 03:13:43',1);
INSERT INTO `sys_menu` VALUES (2,'DRILLAMIN','用户管理',1,'user','user','system/user/index',2,'system:user:list',NULL,1,0,1,0,'2022-09-26 15:09:01','2022-12-26 16:53:55',1);
INSERT INTO `sys_menu` VALUES (3,'DRILLAMIN','角色管理',1,'peoples','role','system/role/index',2,'system:role:list',NULL,2,0,1,0,'2022-09-26 15:10:37','2023-04-04 09:36:27',1);
INSERT INTO `sys_menu` VALUES (4,'DRILLAMIN','菜单管理',1,'tree-table','menu','system/menu/index',2,'system:menu:list',NULL,4,0,1,NULL,'2022-09-26 15:11:14','2023-03-01 13:53:58',1);
INSERT INTO `sys_menu` VALUES (5,'DRILLAMIN','部门管理',1,'tree','dept','system/dept/index',2,'system:dept:list',NULL,5,0,1,NULL,'2022-09-26 15:12:26','2023-03-01 13:57:19',1);
INSERT INTO `sys_menu` VALUES (6,'DRILLAMIN','岗位管理',1,'post','post','system/post/index',2,'system:post:list',NULL,6,0,1,NULL,'2022-09-26 15:13:20','2023-03-01 13:57:26',1);
INSERT INTO `sys_menu` VALUES (7,'CRM','系统管理',0,'system',NULL,'',1,NULL,NULL,1,0,1,0,'2022-09-26 14:56:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (8,'DRILLAMIN','应用管理',1,'','app','system/app/index',2,NULL,NULL,7,0,0,0,'2022-09-26 14:56:09','2023-08-30 09:17:29',1);
INSERT INTO `sys_menu` VALUES (9,'DRILLAMIN','应用查询',8,'#',NULL,'#',3,'system:app:list',NULL,1,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (10,'DRILLAMIN','应用新增',8,'#',NULL,'#',3,'system:app:add',NULL,2,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (11,'DRILLAMIN','应用修改',8,'#',NULL,'#',3,'system:app:edit',NULL,3,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (12,'DRILLAMIN','应用删除',8,'#',NULL,'#',3,'system:app:remove',NULL,4,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1000,'DRILLAMIN','用户查询',2,'#',NULL,'#',3,'system:user:list',NULL,1,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1001,'DRILLAMIN','用户新增',2,'#',NULL,'#',3,'system:user:add',NULL,2,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1002,'DRILLAMIN','用户修改',2,'#',NULL,'#',3,'system:user:edit',NULL,3,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1003,'DRILLAMIN','用户删除',2,'#',NULL,'#',3,'system:user:remove',NULL,4,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1004,'DRILLAMIN','用户导出',2,'#',NULL,'#',3,'system:user:export',NULL,5,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1005,'DRILLAMIN','用户导入',2,'#',NULL,'#',3,'system:user:import',NULL,6,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1006,'DRILLAMIN','重置密码',2,'#',NULL,'#',3,'system:user:resetPwd',NULL,7,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1007,'DRILLAMIN','角色查询',3,'#',NULL,'#',3,'system:role:list',NULL,1,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1008,'DRILLAMIN','角色新增',3,'#',NULL,'#',3,'system:role:add',NULL,2,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1009,'DRILLAMIN','角色修改',3,'#',NULL,'#',3,'system:role:edit',NULL,3,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1010,'DRILLAMIN','角色删除',3,'#',NULL,'#',3,'system:role:remove',NULL,4,0,1,NULL,'2022-09-26 15:48:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1011,'DRILLAMIN','角色导出',3,'#',NULL,'#',3,'system:role:export',NULL,5,0,1,NULL,'2022-09-26 15:48:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1012,'DRILLAMIN','菜单查询',4,'#',NULL,'#',3,'system:menu:list',NULL,1,0,1,NULL,'2022-09-26 15:48:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1013,'DRILLAMIN','菜单新增',4,'#',NULL,'#',3,'system:menu:add',NULL,2,0,1,NULL,'2022-09-26 15:48:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1014,'DRILLAMIN','菜单修改',4,'#',NULL,'#',3,'system:menu:edit',NULL,3,0,1,NULL,'2022-09-26 15:48:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1015,'DRILLAMIN','菜单删除',4,'#',NULL,'#',3,'system:menu:remove',NULL,4,0,1,NULL,'2022-09-26 15:48:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1016,'DRILLAMIN','部门查询',5,'#',NULL,'#',3,'system:dept:list',NULL,1,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1017,'DRILLAMIN','部门新增',5,'#',NULL,'#',3,'system:dept:add',NULL,2,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1018,'DRILLAMIN','部门修改',5,'#',NULL,'#',3,'system:dept:edit',NULL,3,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1019,'DRILLAMIN','部门删除',5,'#',NULL,'#',3,'system:dept:remove',NULL,4,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1020,'DRILLAMIN','岗位查询',6,'#',NULL,'#',3,'system:post:list',NULL,1,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1021,'DRILLAMIN','岗位新增',6,'#',NULL,'#',3,'system:post:add',NULL,2,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1022,'DRILLAMIN','岗位修改',6,'#',NULL,'#',3,'system:post:edit',NULL,3,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1023,'DRILLAMIN','岗位删除',6,'#',NULL,'#',3,'system:post:remove',NULL,4,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1024,'DRILLAMIN','生产管理',0,'switch','produce','#',1,NULL,NULL,2,0,1,1,'2023-02-20 23:01:11','2023-10-24 10:21:37',1);
INSERT INTO `sys_menu` VALUES (1025,'DRILLAMIN','排刀管理',0,'build','material','#',1,NULL,NULL,3,0,1,1,'2023-02-20 23:01:59','2023-04-10 05:38:08',1);
INSERT INTO `sys_menu` VALUES (1026,'DRILLAMIN','设备管理',0,'star','device','#',1,NULL,NULL,4,0,1,1,'2023-02-20 23:02:35','2023-05-08 20:44:54',1);
INSERT INTO `sys_menu` VALUES (1027,'DRILLAMIN','告警管理',0,'email','alarm','#',1,NULL,NULL,5,0,1,1,'2023-02-20 23:03:32','2023-03-01 14:09:18',1);
INSERT INTO `sys_menu` VALUES (1029,'DRILLAMIN','生产排产',1024,'#','schedule','produce/schedule/index',2,'produce:schedule:list',NULL,6,0,1,1,'2023-02-20 23:06:58','2023-09-05 11:26:08',1);
INSERT INTO `sys_menu` VALUES (1030,'DRILLAMIN','板料追溯',1024,'#','board','produce/board/index',2,'produce:board:list',NULL,10,0,1,1,'2023-02-20 23:09:15','2023-06-12 14:23:06',1);
INSERT INTO `sys_menu` VALUES (1031,'DRILLAMIN','钻孔任务',1024,'#','drillWorkOrder','produce/drillWorkOrder/index',2,'produce:drillWorkOrder:list',NULL,7,0,1,1,'2023-02-20 23:11:13','2023-08-28 09:11:34',1);
INSERT INTO `sys_menu` VALUES (1032,'DRILLAMIN','刀盘管理',1025,'#','cutter','material/cutter/index',2,'material:cutter:list',NULL,1,0,0,1,'2023-02-20 23:15:40','2023-07-31 14:44:36',1);
INSERT INTO `sys_menu` VALUES (1033,'DRILLAMIN','设备类型',1026,'#','deviceType','device/deviceType/index',2,'device:deviceType:list',NULL,2,0,1,1,'2023-02-20 23:18:29','2023-11-16 16:33:08',1);
INSERT INTO `sys_menu` VALUES (1034,'DRILLAMIN','配方管理',1024,'#','recipe','produce/recipe/index',2,'produce:recipe:list',NULL,3,0,0,1,'2023-02-20 23:20:37','2023-06-12 14:22:27',1);
INSERT INTO `sys_menu` VALUES (1035,'DRILLAMIN','设备列表',1026,'#','list','device/device/index',2,'device:device:list',NULL,11,0,1,1,'2023-02-20 23:22:03','2023-11-27 10:06:30',1);
INSERT INTO `sys_menu` VALUES (1036,'DRILLAMIN','刀具参数',1025,'#','diaFile','material/diaFile/index',2,'material:config:list',NULL,2,0,1,1,'2023-02-20 23:23:49','2023-10-26 15:53:54',1);
INSERT INTO `sys_menu` VALUES (1037,'DRILLAMIN','调度记录',1026,'#','schedulement','device/schedulement/index',2,'device:schedulement:edit',NULL,8,0,1,1,'2023-02-20 23:25:16','2023-10-09 13:00:13',1);
INSERT INTO `sys_menu` VALUES (1038,'DRILLAMIN','事件管理',1027,'#','event','alarm/event/index',2,'alarm:event:list',NULL,1,0,1,1,'2023-02-20 23:27:03','2023-02-23 22:04:19',1);
INSERT INTO `sys_menu` VALUES (1039,'DRILLAMIN','告警记录',1027,'#','warn','alarm/warn/index',2,'alarm:warn:list',NULL,3,0,1,1,'2023-02-20 23:28:17','2023-03-23 22:13:35',1);
INSERT INTO `sys_menu` VALUES (1043,'DRILLAMIN','修改',1033,NULL,'','#',3,'device:deviceType:edit',NULL,3,0,1,1,'2023-02-21 20:32:06','2023-08-28 14:18:37',1);
INSERT INTO `sys_menu` VALUES (1044,'DRILLAMIN','删除',1033,NULL,NULL,'#',3,'device:deviceType:remove',NULL,4,0,1,1,'2023-02-21 20:32:58','2023-08-28 14:18:42',1);
INSERT INTO `sys_menu` VALUES (1045,'DRILLAMIN','查询',1033,NULL,NULL,'#',3,'device:deviceType:list',NULL,1,0,1,1,'2023-02-21 20:33:18','2023-08-28 14:18:33',1);
INSERT INTO `sys_menu` VALUES (1046,'DRILLAMIN','查询',1034,NULL,NULL,'#',3,'produce:recipe:list',NULL,1,0,1,1,'2023-02-21 22:13:46','2023-05-23 02:10:45',1);
INSERT INTO `sys_menu` VALUES (1047,'DRILLAMIN','新增',1034,NULL,NULL,'#',3,'produce:recipe:add',NULL,2,0,1,1,'2023-02-21 22:14:27','2023-05-23 02:10:49',1);
INSERT INTO `sys_menu` VALUES (1048,'DRILLAMIN','修改',1034,NULL,NULL,'#',3,'produce:recipe:edit',NULL,3,0,1,1,'2023-02-21 22:15:18','2023-05-23 02:10:52',1);
INSERT INTO `sys_menu` VALUES (1049,'DRILLAMIN','删除',1034,NULL,NULL,'#',3,'produce:recipe:remove',NULL,4,0,1,1,'2023-02-21 22:15:58','2023-05-23 02:10:57',1);
INSERT INTO `sys_menu` VALUES (1052,'DRILLAMIN','重置',1033,NULL,NULL,'#',3,'device:deviceType:reset',NULL,5,0,1,1,'2023-02-21 22:22:09','2023-08-28 14:18:51',1);
INSERT INTO `sys_menu` VALUES (1053,'DRILLAMIN','重置',1034,NULL,NULL,'#',3,'produce:recipe:reset',NULL,5,0,1,1,'2023-02-21 22:25:29','2023-05-23 02:11:01',1);
INSERT INTO `sys_menu` VALUES (1054,'DRILLAMIN','通知管理',0,'log','notify','#',1,NULL,NULL,6,0,1,1,'2023-02-22 01:16:01','2023-07-10 14:32:33',1);
INSERT INTO `sys_menu` VALUES (1055,'DRILLAMIN','通知设置',1054,'#','setting','notify/setting/index',2,'notify:setting:list',NULL,1,0,1,1,'2023-02-22 01:21:12','2023-03-23 02:15:26',1);
INSERT INTO `sys_menu` VALUES (1056,'DRILLAMIN','通知记录',1054,'#','record','notify/record/index',2,'notify:record:list',NULL,2,0,1,1,'2023-02-22 01:23:31','2023-07-10 14:35:19',1);
INSERT INTO `sys_menu` VALUES (1057,'DRILLAMIN','查询',1035,NULL,NULL,'#',3,'device:device:list',NULL,1,0,1,1,'2023-02-22 02:27:33','2023-08-28 14:17:49',1);
INSERT INTO `sys_menu` VALUES (1058,'DRILLAMIN','新增',1035,NULL,NULL,'#',3,'device:device:add',NULL,2,0,1,1,'2023-02-22 02:30:35','2023-08-28 14:18:06',1);
INSERT INTO `sys_menu` VALUES (1059,'DRILLAMIN','修改',1035,NULL,NULL,'#',3,'device:device:edit',NULL,3,0,1,1,'2023-02-22 02:32:17','2023-08-28 14:18:10',1);
INSERT INTO `sys_menu` VALUES (1060,'DRILLAMIN','删除',1035,NULL,NULL,'#',3,'device:device:remove',NULL,4,0,1,1,'2023-02-22 02:33:41','2023-08-28 14:18:17',1);
INSERT INTO `sys_menu` VALUES (1061,'DRILLAMIN','查看',1035,NULL,NULL,'#',3,'device:device:view',NULL,5,0,1,1,'2023-02-22 02:35:07','2023-08-28 14:18:21',1);
INSERT INTO `sys_menu` VALUES (1062,'DRILLAMIN','查询',1036,NULL,NULL,'#',3,'material:config:list',NULL,1,0,1,1,'2023-02-22 02:36:27','2023-05-23 01:19:43',43);
INSERT INTO `sys_menu` VALUES (1063,'DRILLAMIN','新增',1036,NULL,NULL,'#',3,'material:config:add',NULL,2,0,1,1,'2023-02-22 02:37:31','2023-05-23 01:19:56',43);
INSERT INTO `sys_menu` VALUES (1064,'DRILLAMIN','修改',1036,NULL,NULL,'#',3,'material:config:edit',NULL,3,0,1,1,'2023-02-22 02:38:13','2023-05-23 01:20:07',43);
INSERT INTO `sys_menu` VALUES (1065,'DRILLAMIN','删除',1036,NULL,NULL,'#',3,'material:config:remove',NULL,4,0,1,1,'2023-02-22 02:38:57','2023-05-23 01:20:19',43);
INSERT INTO `sys_menu` VALUES (1066,'DRILLAMIN','重置',1036,NULL,NULL,'#',3,'material:config:reset',NULL,5,0,1,1,'2023-02-22 02:39:33','2023-05-23 01:20:30',43);
INSERT INTO `sys_menu` VALUES (1068,'DRILLAMIN','新增',1037,NULL,NULL,'#',3,'device:schedulement:add',NULL,2,0,1,1,'2023-02-22 02:41:06','2023-05-23 02:14:32',1);
INSERT INTO `sys_menu` VALUES (1069,'DRILLAMIN','修改',1037,NULL,NULL,'#',3,'device:schedulement:edit',NULL,3,0,1,1,'2023-02-22 02:41:32','2023-05-23 02:14:36',1);
INSERT INTO `sys_menu` VALUES (1070,'DRILLAMIN','删除',1037,NULL,NULL,'#',3,'device:schedulement:remove',NULL,4,0,1,1,'2023-02-22 02:42:02','2023-08-09 14:09:55',1);
INSERT INTO `sys_menu` VALUES (1071,'DRILLAMIN','重置',1037,NULL,NULL,'#',3,'device:schedulement:reset',NULL,5,0,1,1,'2023-02-22 02:43:03','2023-05-23 02:14:43',1);
INSERT INTO `sys_menu` VALUES (1072,'DRILLAMIN','查询',1030,NULL,NULL,'#',3,'produce:board:list',NULL,1,0,1,1,'2023-02-22 04:19:39','2023-05-23 02:12:20',1);
INSERT INTO `sys_menu` VALUES (1073,'DRILLAMIN','新增',1030,NULL,NULL,'#',3,'produce:board:add',NULL,2,0,1,1,'2023-02-22 04:20:16','2023-05-23 02:12:26',1);
INSERT INTO `sys_menu` VALUES (1074,'DRILLAMIN','修改',1030,NULL,NULL,'#',3,'produce:board:edit',NULL,3,0,1,1,'2023-02-22 04:20:47','2023-05-23 02:12:33',1);
INSERT INTO `sys_menu` VALUES (1075,'DRILLAMIN','删除',1030,NULL,NULL,'#',3,'produce:board:remove',NULL,4,0,1,1,'2023-02-22 04:21:23','2023-05-23 02:12:42',1);
INSERT INTO `sys_menu` VALUES (1076,'DRILLAMIN','重置',1030,NULL,NULL,'#',3,'produce:board:reset',NULL,5,0,1,1,'2023-02-22 04:22:07','2023-05-23 02:12:48',1);
INSERT INTO `sys_menu` VALUES (1077,'DRILLAMIN','查询',1029,NULL,NULL,'#',3,'produce:schedule:list',NULL,1,0,1,1,'2023-02-22 04:23:27','2023-09-05 11:26:15',1);
INSERT INTO `sys_menu` VALUES (1078,'DRILLAMIN','新增',1029,NULL,NULL,'#',3,'produce:schedule:add',NULL,2,0,1,1,'2023-02-22 04:23:59','2023-09-05 11:26:20',1);
INSERT INTO `sys_menu` VALUES (1079,'DRILLAMIN','修改',1029,NULL,NULL,'#',3,'produce:schedule:edit',NULL,3,0,1,1,'2023-02-22 04:24:34','2023-09-05 11:26:25',1);
INSERT INTO `sys_menu` VALUES (1080,'DRILLAMIN','删除',1029,NULL,NULL,'#',3,'produce:schedule:remove',NULL,4,0,1,1,'2023-02-22 04:25:14','2023-09-05 11:26:29',1);
INSERT INTO `sys_menu` VALUES (1081,'DRILLAMIN','重置',1029,NULL,NULL,'#',3,'produce:schedule:reset',NULL,5,0,1,1,'2023-02-22 04:26:03','2023-09-05 11:26:33',1);
INSERT INTO `sys_menu` VALUES (1082,'DRILLAMIN','查询',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:list',NULL,1,0,1,1,'2023-02-22 04:28:02','2023-08-04 10:38:52',1);
INSERT INTO `sys_menu` VALUES (1083,'DRILLAMIN','新增',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:add',NULL,2,0,1,1,'2023-02-22 04:28:41','2023-08-04 10:39:10',1);
INSERT INTO `sys_menu` VALUES (1084,'DRILLAMIN','修改',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:edit',NULL,3,0,1,1,'2023-02-22 04:29:10','2023-08-04 10:39:17',1);
INSERT INTO `sys_menu` VALUES (1085,'DRILLAMIN','删除',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:remove',NULL,4,0,1,1,'2023-02-22 04:29:43','2023-08-04 10:39:52',1);
INSERT INTO `sys_menu` VALUES (1086,'DRILLAMIN','重置',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:reset',NULL,5,0,1,1,'2023-02-22 04:30:12','2023-08-04 10:39:30',1);
INSERT INTO `sys_menu` VALUES (1087,'DRILLAMIN','刀具查询',1032,NULL,NULL,'#',3,'material:cutter:list',NULL,1,0,1,1,'2023-02-22 04:31:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1088,'DRILLAMIN','刀具新增',1032,NULL,NULL,'#',3,'material:cutter:add',NULL,2,0,1,1,'2023-02-22 04:32:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1089,'DRILLAMIN','刀具修改',1032,NULL,NULL,'#',3,'material:cutter:edit',NULL,3,0,1,1,'2023-02-22 04:33:24',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1090,'DRILLAMIN','刀具删除',1032,NULL,NULL,'#',3,'material:cutter:remove',NULL,4,0,1,1,'2023-02-22 04:33:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1091,'DRILLAMIN','刀具重置',1032,NULL,NULL,'#',3,'material:cutter:reset',NULL,5,0,1,1,'2023-02-22 04:34:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1097,'DRILLAMIN','查询',1038,NULL,NULL,'#',3,'alarm:event:list',NULL,1,0,1,1,'2023-02-24 00:07:17','2023-05-23 02:15:02',1);
INSERT INTO `sys_menu` VALUES (1098,'DRILLAMIN','新增',1038,NULL,NULL,'#',3,'alarm:event:add',NULL,2,0,1,1,'2023-02-24 00:08:03','2023-05-23 02:15:13',1);
INSERT INTO `sys_menu` VALUES (1099,'DRILLAMIN','修改',1038,NULL,NULL,'#',3,'alarm:event:edit',NULL,3,0,1,1,'2023-02-24 00:08:38','2023-05-23 02:15:17',1);
INSERT INTO `sys_menu` VALUES (1100,'DRILLAMIN','删除',1038,NULL,NULL,'#',3,'alarm:event:remove',NULL,4,0,1,1,'2023-02-24 00:09:16','2023-05-23 02:15:21',1);
INSERT INTO `sys_menu` VALUES (1101,'DRILLAMIN','重置',1038,NULL,NULL,'#',3,'alarm:event:reset',NULL,5,0,1,1,'2023-02-24 00:09:55','2023-05-23 02:15:25',1);
INSERT INTO `sys_menu` VALUES (1102,'DRILLAMIN','查询',1039,NULL,NULL,'#',3,'alarm:warn:list',NULL,1,0,1,1,'2023-02-24 00:10:40','2023-05-23 02:15:55',1);
INSERT INTO `sys_menu` VALUES (1103,'DRILLAMIN','新增',1039,NULL,NULL,'#',3,'alarm:warn:add',NULL,2,0,1,1,'2023-02-24 00:11:32','2023-05-23 02:16:08',1);
INSERT INTO `sys_menu` VALUES (1104,'DRILLAMIN','修改',1039,NULL,NULL,'#',3,'alarm:warn:edit',NULL,3,0,1,1,'2023-02-24 00:12:00','2023-05-23 02:16:12',1);
INSERT INTO `sys_menu` VALUES (1105,'DRILLAMIN','删除',1039,NULL,NULL,'#',3,'alarm:warn:remove',NULL,4,0,1,1,'2023-02-24 00:12:26','2023-05-23 02:16:16',1);
INSERT INTO `sys_menu` VALUES (1106,'DRILLAMIN','重置',1039,NULL,NULL,'#',3,'alarm:warn:reset',NULL,5,0,1,1,'2023-02-24 00:12:54','2023-05-23 02:16:20',1);
INSERT INTO `sys_menu` VALUES (1107,'DRILLAMIN','导入',1039,NULL,NULL,'#',3,'alarm:warn:import',NULL,6,0,1,1,'2023-02-24 00:13:42','2023-05-23 02:16:23',1);
INSERT INTO `sys_menu` VALUES (1108,'DRILLAMIN','查询',1055,NULL,NULL,'#',3,'notify:record:list',NULL,1,0,1,1,'2023-02-24 00:14:37','2023-05-23 02:16:31',1);
INSERT INTO `sys_menu` VALUES (1109,'DRILLAMIN','新增',1055,NULL,NULL,'#',3,'notify:record:add',NULL,2,0,1,1,'2023-02-24 00:15:14','2023-05-23 02:16:38',1);
INSERT INTO `sys_menu` VALUES (1110,'DRILLAMIN','修改',1055,NULL,NULL,'#',3,'notify:record:edit',NULL,3,0,1,1,'2023-02-24 00:16:57','2023-05-23 02:16:44',1);
INSERT INTO `sys_menu` VALUES (1111,'DRILLAMIN','删除',1055,NULL,NULL,'#',3,'notify:record:remove',NULL,4,0,1,1,'2023-02-24 00:17:25','2023-05-23 02:16:47',1);
INSERT INTO `sys_menu` VALUES (1112,'DRILLAMIN','重置',1055,NULL,NULL,'#',3,'notify:record:reset',NULL,5,0,1,1,'2023-02-24 00:17:50','2023-05-23 02:16:53',1);
INSERT INTO `sys_menu` VALUES (1113,'DRILLAMIN','查询',1056,NULL,NULL,'#',3,'notify:setting:list',NULL,1,0,1,1,'2023-02-24 00:18:25','2023-05-23 02:17:00',1);
INSERT INTO `sys_menu` VALUES (1114,'DRILLAMIN','新增',1056,NULL,NULL,'#',3,'notify:setting:add',NULL,2,0,1,1,'2023-02-24 00:18:55','2023-05-23 02:17:04',1);
INSERT INTO `sys_menu` VALUES (1115,'DRILLAMIN','修改',1056,NULL,NULL,'#',3,'notify:setting:edit',NULL,3,0,1,1,'2023-02-24 00:19:26','2023-05-23 02:17:08',1);
INSERT INTO `sys_menu` VALUES (1116,'DRILLAMIN','删除',1056,NULL,NULL,'#',3,'notify:setting:remove',NULL,4,0,1,1,'2023-02-24 00:19:50','2023-05-23 02:17:12',1);
INSERT INTO `sys_menu` VALUES (1117,'DRILLAMIN','重置',1056,NULL,NULL,'#',3,'notify:setting:reset',NULL,5,0,1,1,'2023-02-24 00:20:16','2023-05-23 02:17:15',1);
INSERT INTO `sys_menu` VALUES (1122,'DRILLAMIN','生产工单',1024,'#','workOrder','produce/workOrder/index',2,'produce:workorder:list',NULL,4,0,1,1,'2023-03-01 11:03:28','2023-06-12 14:22:31',1);
INSERT INTO `sys_menu` VALUES (1123,'DRILLAMIN','查询',1122,NULL,NULL,'#',3,'produce:workorder:list',NULL,1,0,1,1,'2023-03-01 11:04:30','2023-03-01 11:05:22',1);
INSERT INTO `sys_menu` VALUES (1124,'DRILLAMIN','新增',1122,NULL,NULL,'#',3,'produce:workorder:add',NULL,2,0,1,1,'2023-03-01 11:06:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1125,'DRILLAMIN','修改',1122,NULL,NULL,'#',3,'produce:workorder:edit',NULL,3,0,1,1,'2023-03-01 11:06:41',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1126,'DRILLAMIN','删除',1122,NULL,NULL,'#',3,'produce:workorder:remove',NULL,4,0,1,1,'2023-03-01 11:07:08','2023-03-01 11:07:17',1);
INSERT INTO `sys_menu` VALUES (1127,'DRILLAMIN','重置',1122,NULL,NULL,'#',3,'produce:workorder:reset',NULL,5,0,1,1,'2023-03-01 11:07:46',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1128,'DRILLAMIN','工序管理',1024,'#','process','produce/process/index',2,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:37:07','2023-10-27 10:35:14',1);
INSERT INTO `sys_menu` VALUES (1129,'DRILLAMIN','查询',1128,'#',NULL,'#',3,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:42:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1130,'DRILLAMIN','新增',1128,'#',NULL,'#',3,'produce:process:add',NULL,2,0,1,1,'2023-03-01 11:43:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1131,'DRILLAMIN','修改',1128,'#',NULL,'#',3,'produce:process:edit',NULL,3,0,1,1,'2023-03-01 11:43:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1132,'DRILLAMIN','删除',1128,'#',NULL,'#',3,'produce:process:remove',NULL,4,0,1,1,'2023-03-01 11:43:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1133,'DRILLAMIN','重置',1128,'#',NULL,'#',3,'produce:process:reset',NULL,5,0,1,1,'2023-03-01 11:44:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1134,'DRILLAMIN','字典管理',1,'dict','dict','system/dict/index',2,'system:dict:list',NULL,3,0,0,1,'2023-03-01 13:53:42','2023-11-24 15:35:19',1);
INSERT INTO `sys_menu` VALUES (1136,'DRILLAMIN','告警设置',1027,'#','settings','alarm/setting/index',2,'alarm:setting:index',NULL,2,0,1,1,'2023-03-23 22:13:23','2023-07-10 14:36:58',1);
INSERT INTO `sys_menu` VALUES (1137,'DRILLAMIN','查询',1136,'#',NULL,'#',3,'alarm:setting:list',NULL,1,0,1,1,'2023-03-23 22:15:45','2023-05-23 02:15:33',1);
INSERT INTO `sys_menu` VALUES (1138,'DRILLAMIN','新增',1136,'#',NULL,'#',3,'alarm:setting:add',NULL,2,0,1,1,'2023-03-23 22:16:21','2023-05-23 02:15:40',1);
INSERT INTO `sys_menu` VALUES (1139,'DRILLAMIN','修改',1136,'#',NULL,'#',3,'alarm:setting:edit',NULL,3,0,1,1,'2023-03-23 22:16:59','2023-05-23 02:15:45',1);
INSERT INTO `sys_menu` VALUES (1140,'DRILLAMIN','删除',1136,'#',NULL,'#',3,'alarm:setting:remove',NULL,4,0,1,1,'2023-03-23 22:17:26','2023-05-23 02:15:49',1);
INSERT INTO `sys_menu` VALUES (1147,'DRILLAMIN','排刀计划',1024,'#','drillCutterPlan','produce/drillCutterPlan/index',2,'produce:drillCutterPlan:list',NULL,5,0,0,1,'2023-03-28 04:56:52','2023-08-24 14:54:12',1);
INSERT INTO `sys_menu` VALUES (1148,'DRILLAMIN','主数据',0,'component','masterData','#',1,NULL,NULL,1,0,1,1,'2023-03-29 22:00:59','2023-08-14 13:44:31',1);
INSERT INTO `sys_menu` VALUES (1149,'DRILLAMIN','计量单位',1148,'','unitMeasure','masterData/unitMeasure/index',2,'masterData:unitMeasure:list',NULL,1,0,1,1,'2023-03-29 22:05:55','2023-05-08 05:21:07',1);
INSERT INTO `sys_menu` VALUES (1150,'DRILLAMIN','客户管理',1148,'','client','masterData/client/index',2,'masterData:client:list',NULL,50,0,1,1,'2023-03-29 22:07:26','2023-05-08 05:21:20',1);
INSERT INTO `sys_menu` VALUES (1151,'DRILLAMIN','供应商管理',1148,'','vendor','masterData/vendor/index',2,'masterData:vendor:list',NULL,60,0,1,1,'2023-03-29 22:09:03','2023-05-08 05:21:23',1);
INSERT INTO `sys_menu` VALUES (1152,'DRILLAMIN','车间管理',1148,'','workshop','masterData/workShop/index',2,'masterData:workshop:list',NULL,40,0,1,1,'2023-03-29 22:10:20','2023-05-08 05:21:16',1);
INSERT INTO `sys_menu` VALUES (1153,'DRILLAMIN','工作站',1148,'','workstation','masterData/workStation/index',2,'masterData:workstation:list',NULL,41,0,1,1,'2023-03-29 22:11:24','2023-09-22 16:20:17',1);
INSERT INTO `sys_menu` VALUES (1154,'DRILLAMIN','物料产品分类',1148,'','itemType','masterData/itemType/index',2,'masterData:itemType:list',NULL,10,0,1,1,'2023-03-29 22:12:21','2023-05-08 05:21:12',1);
INSERT INTO `sys_menu` VALUES (1155,'DRILLAMIN','物料产品管理',1148,'','item','masterData/item/index',2,'masterData:item:list',NULL,11,0,1,1,'2023-03-29 22:13:20','2023-05-08 05:21:14',1);
INSERT INTO `sys_menu` VALUES (1156,'DRILLAMIN','产品大类',1148,'','productCategory','masterData/productCategory/index',2,'masterData:productCategory:list',NULL,5,0,1,1,'2023-04-01 02:41:29','2023-05-08 21:42:17',1);
INSERT INTO `sys_menu` VALUES (1157,'DRILLAMIN','工艺路线',1024,'#','route','produce/route/index',2,'produce:route:list',NULL,2,0,1,1,'2023-04-01 02:46:56','2023-09-07 10:12:33',1);
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'tree','wm','#',1,NULL,NULL,50,0,1,1,'2023-04-01 02:50:34','2023-06-25 16:32:58',1);
INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','仓库设置',1158,'#','warehouse','wareHouse/wareHouse/index',2,'warehouse:warehouse:list',NULL,1,0,1,1,'2023-04-01 02:53:42','2023-04-18 03:33:51',1);
INSERT INTO `sys_menu` VALUES (1160,'DRILLAMIN','库存现有量',1158,'#','materialStock','wareHouse/materialStock/index',2,'warehouse:materialStock:list',NULL,2,0,1,1,'2023-04-01 02:54:57','2023-05-11 04:28:18',1);
INSERT INTO `sys_menu` VALUES (1161,'DRILLAMIN','钻带参数',1025,'#','drillFile','material/drillFile/index',2,'material:drillFile:list',NULL,3,0,1,1,'2023-04-01 03:20:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1162,'DRILLAMIN','排刀文件',1025,'#','atpFile','material/atpFile/index',2,'material:atpFile:list',NULL,4,0,1,1,'2023-04-01 03:21:21','2023-04-10 23:34:21',1);
INSERT INTO `sys_menu` VALUES (1165,'DRILLAMIN','报工记录',1024,'#','reportrecords','produce/reportRecords/index',2,'produce:reportRecords:list',NULL,9,0,1,1,'2023-04-04 00:47:50','2023-06-19 16:14:13',1);
INSERT INTO `sys_menu` VALUES (1166,'DRILLAMIN','查询',1165,'#',NULL,'#',3,'produce:reportRecords:list',NULL,1,0,1,1,'2023-04-04 00:50:20','2023-08-09 16:00:30',50);
INSERT INTO `sys_menu` VALUES (1167,'DRILLAMIN','新增',1165,'#',NULL,'#',3,'produce:reportRecords:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2023-08-09 16:00:21',50);
INSERT INTO `sys_menu` VALUES (1168,'DRILLAMIN','修改',1165,'#',NULL,'#',3,'produce:reportRecords:edit',NULL,3,0,1,1,'2023-04-04 00:51:30','2023-08-09 16:00:17',50);
INSERT INTO `sys_menu` VALUES (1169,'DRILLAMIN','删除',1165,'#',NULL,'#',3,'produce:reportRecords:remove',NULL,4,0,1,1,'2023-04-04 00:51:58','2023-08-09 16:00:12',50);
INSERT INTO `sys_menu` VALUES (1170,'DRILLAMIN','重置',1165,'#',NULL,'#',3,'produce:reportRecords:reset',NULL,5,0,1,1,'2023-04-04 00:52:22','2023-08-09 16:00:07',50);
INSERT INTO `sys_menu` VALUES (1171,'DRILLAMIN','查询',1161,'#',NULL,'#',3,'material:drillFile:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1172,'DRILLAMIN','新增',1161,'#',NULL,'#',3,'material:drillFile:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1173,'DRILLAMIN','修改',1161,'#',NULL,'#',3,'material:drillFile:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1174,'DRILLAMIN','删除',1161,'#',NULL,'#',3,'material:drillFile:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1175,'DRILLAMIN','重置',1161,'#',NULL,'#',3,'material:drillFile:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1176,'DRILLAMIN','查看',1161,'#',NULL,'#',3,'material:drillFile:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1177,'DRILLAMIN','查询',1162,'#',NULL,'#',3,'material:atpFile:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1178,'DRILLAMIN','新增',1162,'#',NULL,'#',3,'material:atpFile:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1179,'DRILLAMIN','修改',1162,'#',NULL,'#',3,'material:atpFile:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1180,'DRILLAMIN','删除',1162,'#',NULL,'#',3,'material:atpFile:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1181,'DRILLAMIN','重置',1162,'#',NULL,'#',3,'material:atpFile:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1182,'DRILLAMIN','查看',1162,'#',NULL,'#',3,'material:atpFile:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1207,'DRILLAMIN','查询',1159,'#',NULL,'#',3,'warehouse:warehouse:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1208,'DRILLAMIN','新增',1159,'#',NULL,'#',3,'warehouse:warehouse:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1209,'DRILLAMIN','修改',1159,'#',NULL,'#',3,'warehouse:warehouse:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1210,'DRILLAMIN','删除',1159,'#',NULL,'#',3,'warehouse:warehouse:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1211,'DRILLAMIN','重置',1159,'#',NULL,'#',3,'warehouse:warehouse:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1212,'DRILLAMIN','查看',1159,'#',NULL,'#',3,'warehouse:warehouse:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1213,'DRILLAMIN','查询',1160,'#',NULL,'#',3,'warehouse:materialStock:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1214,'DRILLAMIN','新增',1160,'#',NULL,'#',3,'warehouse:materialStock:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1215,'DRILLAMIN','修改',1160,'#',NULL,'#',3,'warehouse:materialStock:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1216,'DRILLAMIN','删除',1160,'#',NULL,'#',3,'warehouse:materialStock:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1217,'DRILLAMIN','重置',1160,'#',NULL,'#',3,'warehouse:materialStock:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1218,'DRILLAMIN','查看',1160,'#',NULL,'#',3,'warehouse:materialStock:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1219,'DRILLAMIN','查询',1157,'#',NULL,'#',3,'produce:route:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1220,'DRILLAMIN','新增',1157,'#',NULL,'#',3,'produce:route:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1221,'DRILLAMIN','修改',1157,'#',NULL,'#',3,'produce:route:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1222,'DRILLAMIN','删除',1157,'#',NULL,'#',3,'produce:route:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1223,'DRILLAMIN','重置',1157,'#',NULL,'#',3,'produce:route:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1224,'DRILLAMIN','查看',1157,'#',NULL,'#',3,'produce:route:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1225,'DRILLAMIN','查询',1147,'#',NULL,'#',3,'produce:drillCutterPlan:list',NULL,1,0,1,1,'2023-04-04 00:50:20','2023-04-04 01:44:05',1);
INSERT INTO `sys_menu` VALUES (1226,'DRILLAMIN','新增',1147,'#',NULL,'#',3,'produce:drillCutterPlan:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2023-04-04 01:44:10',1);
INSERT INTO `sys_menu` VALUES (1227,'DRILLAMIN','修改',1147,'#',NULL,'#',3,'produce:drillCutterPlan:edit',NULL,3,0,1,1,'2023-04-04 00:51:30','2023-04-04 01:44:15',1);
INSERT INTO `sys_menu` VALUES (1228,'DRILLAMIN','删除',1147,'#',NULL,'#',3,'produce:drillCutterPlan:remove',NULL,4,0,1,1,'2023-04-04 00:51:58','2023-04-04 01:44:19',1);
INSERT INTO `sys_menu` VALUES (1229,'DRILLAMIN','重置',1147,'#',NULL,'#',3,'produce:drillCutterPlan:reset',NULL,5,0,1,1,'2023-04-04 00:52:22','2023-04-04 01:44:23',1);
INSERT INTO `sys_menu` VALUES (1230,'DRILLAMIN','查看',1147,'#',NULL,'#',3,'produce:drillCutterPlan:view',NULL,6,0,1,1,'2023-04-04 00:52:22','2023-04-04 01:44:28',1);
INSERT INTO `sys_menu` VALUES (1231,'DRILLAMIN','查询',1149,'#',NULL,'#',3,'masterData:unitMeasure:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1232,'DRILLAMIN','新增',1149,'#',NULL,'#',3,'masterData:unitMeasure:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1233,'DRILLAMIN','修改',1149,'#',NULL,'#',3,'masterData:unitMeasure:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1234,'DRILLAMIN','删除',1149,'#',NULL,'#',3,'masterData:unitMeasure:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1235,'DRILLAMIN','重置',1149,'#',NULL,'#',3,'masterData:unitMeasure:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1236,'DRILLAMIN','查看',1149,'#',NULL,'#',3,'masterData:unitMeasure:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1237,'DRILLAMIN','查询',1150,'#',NULL,'#',3,'masterData:client:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1238,'DRILLAMIN','新增',1150,'#',NULL,'#',3,'masterData:client:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1239,'DRILLAMIN','修改',1150,'#',NULL,'#',3,'masterData:client:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1240,'DRILLAMIN','删除',1150,'#',NULL,'#',3,'masterData:client:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1241,'DRILLAMIN','重置',1150,'#',NULL,'#',3,'masterData:client:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1242,'DRILLAMIN','查看',1150,'#',NULL,'#',3,'masterData:client:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1243,'DRILLAMIN','查询',1151,'#',NULL,'#',3,'masterData:vendor:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1244,'DRILLAMIN','新增',1151,'#',NULL,'#',3,'masterData:vendor:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1245,'DRILLAMIN','修改',1151,'#',NULL,'#',3,'masterData:vendor:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1246,'DRILLAMIN','删除',1151,'#',NULL,'#',3,'masterData:vendor:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1247,'DRILLAMIN','重置',1151,'#',NULL,'#',3,'masterData:vendor:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1248,'DRILLAMIN','查看',1151,'#',NULL,'#',3,'masterData:vendor:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1249,'DRILLAMIN','查询',1152,'#',NULL,'#',3,'masterData:workshop:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1250,'DRILLAMIN','新增',1152,'#',NULL,'#',3,'masterData:workshop:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1251,'DRILLAMIN','修改',1152,'#',NULL,'#',3,'masterData:workshop:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1252,'DRILLAMIN','删除',1152,'#',NULL,'#',3,'masterData:workshop:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1253,'DRILLAMIN','重置',1152,'#',NULL,'#',3,'masterData:workshop:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1254,'DRILLAMIN','查看',1152,'#',NULL,'#',3,'masterData:workshop:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1255,'DRILLAMIN','查询',1153,'#',NULL,'#',3,'masterData:workstation:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1256,'DRILLAMIN','新增',1153,'#',NULL,'#',3,'masterData:workstation:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1257,'DRILLAMIN','修改',1153,'#',NULL,'#',3,'masterData:workstation:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1258,'DRILLAMIN','删除',1153,'#',NULL,'#',3,'masterData:workstation:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1259,'DRILLAMIN','重置',1153,'#',NULL,'#',3,'masterData:workstation:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1260,'DRILLAMIN','查看',1153,'#',NULL,'#',3,'masterData:workstation:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1261,'DRILLAMIN','查询',1154,'#',NULL,'#',3,'masterData:itemType:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1262,'DRILLAMIN','新增',1154,'#',NULL,'#',3,'masterData:itemType:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1263,'DRILLAMIN','修改',1154,'#',NULL,'#',3,'masterData:itemType:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1264,'DRILLAMIN','删除',1154,'#',NULL,'#',3,'masterData:itemType:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1265,'DRILLAMIN','重置',1154,'#',NULL,'#',3,'masterData:itemType:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1266,'DRILLAMIN','查看',1154,'#',NULL,'#',3,'masterData:itemType:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1267,'DRILLAMIN','查询',1155,'#',NULL,'#',3,'masterData:item:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1268,'DRILLAMIN','新增',1155,'#',NULL,'#',3,'masterData:item:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1269,'DRILLAMIN','修改',1155,'#',NULL,'#',3,'masterData:item:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1270,'DRILLAMIN','删除',1155,'#',NULL,'#',3,'masterData:item:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1271,'DRILLAMIN','重置',1155,'#',NULL,'#',3,'masterData:item:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1272,'DRILLAMIN','查看',1155,'#',NULL,'#',3,'masterData:item:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1273,'DRILLAMIN','查询',1156,'#',NULL,'#',3,'masterData:productCategory:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1274,'DRILLAMIN','新增',1156,'#',NULL,'#',3,'masterData:productCategory:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1275,'DRILLAMIN','修改',1156,'#',NULL,'#',3,'masterData:productCategory:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1276,'DRILLAMIN','删除',1156,'#',NULL,'#',3,'masterData:productCategory:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1277,'DRILLAMIN','重置',1156,'#',NULL,'#',3,'masterData:productCategory:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1278,'DRILLAMIN','查看',1156,'#',NULL,'#',3,'masterData:productCategory:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1279,'DRILLAMIN','提交',1122,'#',NULL,'#',3,'produce:workorder:commit',NULL,7,0,1,1,'2023-04-05 22:19:41','2023-09-22 16:25:28',1);
INSERT INTO `sys_menu` VALUES (1280,'DRILLAMIN','提交',1029,'#',NULL,'#',3,'produce:schedule:commit',NULL,6,0,1,33,'2023-04-05 22:53:40','2023-09-05 11:26:39',1);
INSERT INTO `sys_menu` VALUES (1281,'DRILLAMIN','提交',1165,'#',NULL,'#',3,'produce:reportRecords:commit',NULL,7,0,1,33,'2023-04-05 23:38:44','2023-09-22 16:40:07',1);
INSERT INTO `sys_menu` VALUES (1282,'DRILLAMIN','详细查询',1162,'#',NULL,'#',3,'material:atpFileDetail:list',NULL,7,0,1,33,'2023-04-10 02:57:47',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1283,'DRILLAMIN','详细新增',1162,'#',NULL,'#',3,'material:atpFileDetail:add',NULL,8,0,1,33,'2023-04-10 03:01:06',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1284,'DRILLAMIN','详细修改',1162,'#',NULL,'#',3,'material:atpFileDetail:edit',NULL,9,0,1,33,'2023-04-10 03:01:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1285,'DRILLAMIN','详细删除',1162,'#',NULL,'#',3,'material:atpFileDetail:remove',NULL,10,0,1,33,'2023-04-10 03:02:36',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1286,'DRILLAMIN','详细查看',1162,'#',NULL,'#',3,'material:atpFileDetail:view',NULL,11,0,1,33,'2023-04-10 03:04:33',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1287,'DRILLAMIN','产品结构查询',1155,'#',NULL,'#',3,'masterData:productBom:list',NULL,7,0,1,33,'2023-04-10 03:11:33',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1288,'DRILLAMIN','产品结构新增',1155,'#',NULL,'#',3,'masterData:productBom:add',NULL,8,0,1,33,'2023-04-10 03:12:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1289,'DRILLAMIN','产品结构修改',1155,'#',NULL,'#',3,'masterData:productBom:edit',NULL,9,0,1,33,'2023-04-10 03:13:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1290,'DRILLAMIN','产品结构删除',1155,'#',NULL,'#',3,'masterData:productBom:remove',NULL,10,0,1,33,'2023-04-10 03:14:01',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1291,'DRILLAMIN','产品结构查看',1155,'#',NULL,'#',3,'masterData:productBom:view',NULL,11,0,1,33,'2023-04-10 03:14:47',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1292,'DRILLAMIN','查询工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:list',NULL,7,0,1,33,'2023-04-10 03:19:01',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1293,'DRILLAMIN','新增工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:add',NULL,8,0,1,33,'2023-04-10 03:19:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1294,'DRILLAMIN','修改工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:edit',NULL,9,0,1,33,'2023-04-10 03:20:38',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1295,'DRILLAMIN','删除工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:remove',NULL,10,0,1,33,'2023-04-10 03:21:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1296,'DRILLAMIN','查看工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:view',NULL,11,0,1,33,'2023-04-10 03:22:11',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1297,'DRILLAMIN','查询工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:list',NULL,12,0,1,33,'2023-04-10 03:23:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1298,'DRILLAMIN','新增工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:add',NULL,13,0,1,33,'2023-04-10 03:23:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1299,'DRILLAMIN','修改工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:edit',NULL,14,0,1,33,'2023-04-10 03:24:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1300,'DRILLAMIN','删除工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:remove',NULL,15,0,1,33,'2023-04-10 03:25:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1301,'DRILLAMIN','查看工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:view',NULL,16,0,1,33,'2023-04-10 03:27:49',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1302,'DRILLAMIN','查询工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:list',NULL,17,0,1,33,'2023-04-10 03:30:16',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1303,'DRILLAMIN','新增工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:add',NULL,18,0,1,33,'2023-04-10 03:30:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1304,'DRILLAMIN','修改工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:edit',NULL,19,0,1,33,'2023-04-10 03:31:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1305,'DRILLAMIN','删除工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:remove',NULL,20,0,1,33,'2023-04-10 03:32:34',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1306,'DRILLAMIN','查看工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:view',NULL,21,0,1,33,'2023-04-10 03:33:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1307,'DRILLAMIN','查看',1128,'#',NULL,'#',3,'produce:process:view',NULL,6,0,1,33,'2023-04-10 03:35:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1308,'DRILLAMIN','查看',1034,'#',NULL,'#',3,'produce:recipe:view',NULL,6,0,1,33,'2023-04-10 04:21:20','2023-05-23 02:11:05',1);
INSERT INTO `sys_menu` VALUES (1309,'DRILLAMIN','查看',1122,'#',NULL,'#',3,'produce:workorder:view',NULL,6,0,1,33,'2023-04-10 04:22:28','2023-09-22 16:25:25',1);
INSERT INTO `sys_menu` VALUES (1310,'DRILLAMIN','查看',1029,'#',NULL,'#',3,'produce:schedule:view',NULL,7,0,1,33,'2023-04-10 04:23:37','2023-09-05 11:26:44',1);
INSERT INTO `sys_menu` VALUES (1311,'DRILLAMIN','查看',1031,'#',NULL,'#',3,'produce:drillWorkOrder:view',NULL,6,0,1,33,'2023-04-10 04:25:02','2023-08-04 10:39:23',1);
INSERT INTO `sys_menu` VALUES (1312,'DRILLAMIN','查看',1165,'#',NULL,'#',3,'produce:reportRecords:view',NULL,6,0,1,33,'2023-04-10 04:25:58','2023-09-22 16:40:12',1);
INSERT INTO `sys_menu` VALUES (1313,'DRILLAMIN','查看',1030,'#',NULL,'#',3,'produce:board:view',NULL,6,0,1,33,'2023-04-10 04:28:13','2023-05-23 02:12:52',1);
INSERT INTO `sys_menu` VALUES (1314,'DRILLAMIN','检验记录',1024,'#','checkrecords','produce/checkRecords/index',2,'produce:checkrecords:list',NULL,11,0,1,1,'2023-04-14 02:34:00','2023-06-12 14:23:10',1);
INSERT INTO `sys_menu` VALUES (1315,'DRILLAMIN','查询',1314,'#',NULL,'#',3,'produce:checkrecords:list',NULL,1,0,1,1,'2023-05-06 01:00:46','2023-05-23 02:13:01',1);
INSERT INTO `sys_menu` VALUES (1317,'DRILLAMIN','设备维护',1026,'#','repair','device/repair/index',2,'device:repair:list',NULL,6,0,1,1,'2023-05-06 03:11:39','2023-08-24 14:54:37',1);
INSERT INTO `sys_menu` VALUES (1319,'DRILLAMIN','点检项目',1026,'#','subject','device/subject/index',2,'device:subject:list',NULL,5,0,1,1,'2023-05-16 02:46:43','2023-08-24 14:54:32',1);
INSERT INTO `sys_menu` VALUES (1320,'DRILLAMIN','编码规则',1148,'','autocodeRule','masterData/autocodeRule/index',2,'masterData:autocodeRule:list',NULL,1,0,1,1,'2023-05-17 02:29:46','2023-08-24 14:54:02',1);
INSERT INTO `sys_menu` VALUES (1321,'DRILLAMIN','导出',1149,'#',NULL,'#',3,'masterData:unitMeasure:export',NULL,1,0,1,1,'2023-05-21 22:53:13','2023-05-21 22:53:58',1);
INSERT INTO `sys_menu` VALUES (1322,'DRILLAMIN','查询',1320,'#',NULL,'#',3,'masterData:autocodeRule:list',NULL,1,0,1,1,'2023-05-22 04:07:44','2023-08-09 14:11:46',1);
INSERT INTO `sys_menu` VALUES (1323,'DRILLAMIN','修改',1320,'#',NULL,'#',3,'masterData:autocodeRule:edit',NULL,2,0,1,1,'2023-05-22 04:08:17','2023-09-22 15:38:19',1);
INSERT INTO `sys_menu` VALUES (1324,'DRILLAMIN','导入',1149,'#',NULL,'#',3,'masterData:unitMeasure:import',NULL,1,0,1,43,'2023-05-22 22:58:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1325,'DRILLAMIN','导入',1154,'#',NULL,'#',3,'masterData:itemType:import',NULL,1,0,1,43,'2023-05-22 23:02:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1326,'DRILLAMIN','导入',1153,'#',NULL,'#',3,'masterData:workstation:import',NULL,1,0,1,43,'2023-05-22 23:06:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1327,'DRILLAMIN','导入',1150,'#',NULL,'#',3,'masterData:client:import',NULL,1,0,1,43,'2023-05-22 23:08:11',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1328,'DRILLAMIN','导入',1151,'#',NULL,'#',3,'masterData:vendor:import',NULL,1,0,1,43,'2023-05-22 23:09:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1329,'DRILLAMIN','导入',1128,'#',NULL,'#',3,'produce:process:import',NULL,1,0,1,43,'2023-05-22 23:13:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1330,'DRILLAMIN','导入',1122,'#',NULL,'#',3,'produce:workorder:import',NULL,1,0,1,43,'2023-05-22 23:17:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1331,'DRILLAMIN','导出',1165,'#',NULL,'#',3,'produce:reportRecords:export',NULL,1,0,1,43,'2023-05-22 23:20:22','2023-08-09 16:00:26',50);
INSERT INTO `sys_menu` VALUES (1332,'DRILLAMIN','导出',1030,'#',NULL,'#',3,'produce:board:export',NULL,1,0,1,43,'2023-05-22 23:21:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1333,'DRILLAMIN','查看',1314,'#',NULL,'#',3,'produce:checkrecords:view',NULL,1,0,1,43,'2023-05-22 23:22:44',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1334,'DRILLAMIN','编辑',1314,'#',NULL,'#',3,'produce:checkrecords:edit',NULL,1,0,1,43,'2023-05-22 23:23:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1335,'DRILLAMIN','导出',1038,'#',NULL,'#',3,'alarm:event:export',NULL,6,0,1,43,'2023-05-22 23:35:43','2023-09-22 17:00:36',1);
INSERT INTO `sys_menu` VALUES (1336,'DRILLAMIN','导出',1039,'#',NULL,'#',3,'alarm:warn:export',NULL,6,0,1,43,'2023-05-22 23:37:48','2023-09-22 17:02:03',1);
INSERT INTO `sys_menu` VALUES (1337,'DRILLAMIN','导入',1156,'#',NULL,'#',3,'masterData:productCategory:import',NULL,1,0,1,43,'2023-05-23 01:10:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1338,'DRILLAMIN','导入',1155,'#',NULL,'#',3,'masterData:item:import',NULL,1,0,1,43,'2023-05-23 01:11:55',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1339,'DRILLAMIN','导出',1031,'#',NULL,'#',3,'produce:drillWorkOrder:export',NULL,1,0,1,43,'2023-05-23 01:13:41','2023-08-04 10:38:58',1);
INSERT INTO `sys_menu` VALUES (1340,'DRILLAMIN','导出',1314,'#',NULL,'#',3,'produce:checkrecords:export',NULL,1,0,1,43,'2023-05-23 01:15:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1341,'DRILLAMIN','导入',1036,'#',NULL,'#',3,'material:config:import',NULL,1,0,1,43,'2023-05-23 01:16:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1342,'DRILLAMIN','导入',1161,'#',NULL,'#',3,'material:drillFile:import',NULL,1,0,1,43,'2023-05-23 01:17:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1343,'DRILLAMIN','查看',1036,'#',NULL,'#',3,'material:config:view',NULL,1,0,1,43,'2023-05-23 01:21:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1344,'DRILLAMIN','查询',1317,'#',NULL,'#',3,'device:repair:list',NULL,1,0,1,43,'2023-05-23 01:23:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1345,'DRILLAMIN','新增',1317,'#',NULL,'#',3,'device:repair:add',NULL,1,0,1,43,'2023-05-23 01:23:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1346,'DRILLAMIN','删除',1317,'#',NULL,'#',3,'device:repair:remove',NULL,1,0,1,43,'2023-05-23 01:23:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1347,'DRILLAMIN','修改',1317,'#',NULL,'#',3,'device:repair:edit',NULL,1,0,1,43,'2023-05-23 01:24:04','2023-08-09 14:09:26',1);
INSERT INTO `sys_menu` VALUES (1348,'DRILLAMIN','导出',1317,'#',NULL,'#',3,'device:repair:export',NULL,1,0,1,43,'2023-05-23 01:24:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1349,'DRILLAMIN','导出',1056,'#',NULL,'#',3,'notify:record:export',NULL,6,0,1,43,'2023-05-23 01:25:40','2023-09-22 17:03:21',1);
INSERT INTO `sys_menu` VALUES (1350,'DRILLAMIN','生产报工',1024,'#','reportWork','produce/reportWork/index',2,'produce:reportWork:list',NULL,8,0,1,1,'2023-06-12 11:24:59','2023-09-05 11:40:29',1);
INSERT INTO `sys_menu` VALUES (1351,'DRILLAMIN','查看',1350,'#',NULL,'#',3,'produce:reportWork:view',NULL,1,0,1,1,'2023-06-12 05:23:38','2023-09-05 11:42:20',1);
INSERT INTO `sys_menu` VALUES (1352,'DRILLAMIN','取消',1037,'#',NULL,'#',3,'device:schedulement:cancel',NULL,7,0,1,1,'2023-06-30 13:54:41','2023-09-22 16:59:33',1);
INSERT INTO `sys_menu` VALUES (1353,'DRILLAMIN','查询',1037,'#',NULL,'#',3,'device:schedulement:list',NULL,1,0,1,1,'2023-06-30 14:28:08',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1354,'DRILLAMIN','权限修改检验记录',1314,'#',NULL,'#',3,'produce:checkrecords:hasPermissions',NULL,1,0,1,1,'2023-08-02 10:02:26','2023-08-09 15:26:52',50);
INSERT INTO `sys_menu` VALUES (1355,'DRILLAMIN','点检',1035,'#',NULL,'#',3,'device:device:check',NULL,6,0,1,1,'2023-08-04 11:23:41','2023-09-22 16:53:20',1);
INSERT INTO `sys_menu` VALUES (1356,'DRILLAMIN','重置',1136,'#',NULL,'#',3,'alarm:setting:reset',NULL,8,0,1,1,'2023-08-09 14:03:39',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1357,'DRILLAMIN','查询',1319,'#',NULL,'#',3,'device:subject:list',NULL,1,0,1,1,'2023-08-09 14:07:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1358,'DRILLAMIN','新增',1319,'#',NULL,'#',3,'device:subject:add',NULL,2,0,1,1,'2023-08-09 14:07:40',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1359,'DRILLAMIN','修改',1319,'#',NULL,'#',3,'device:subject:edit',NULL,4,0,1,1,'2023-08-09 14:08:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1360,'DRILLAMIN','删除',1319,'#',NULL,'#',3,'device:subject:remove',NULL,5,0,1,1,'2023-08-09 14:08:28',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1361,'DRILLAMIN','导出',1319,'#',NULL,'#',3,'device:subject:export',NULL,6,0,1,1,'2023-08-09 14:09:01',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1362,'DRILLAMIN','详情',1037,'#',NULL,'#',3,'device:schedulement:detail',NULL,8,0,1,1,'2023-08-09 14:10:35','2023-09-22 16:59:57',1);
INSERT INTO `sys_menu` VALUES (1363,'DRILLAMIN','发起',1037,'#',NULL,'#',3,'device:schedulement:start',NULL,7,0,1,1,'2023-08-09 14:11:04','2023-08-09 14:11:12',1);
INSERT INTO `sys_menu` VALUES (1364,'DRILLAMIN','新增',1320,'#',NULL,'#',3,'masterData:autocodeRule:add',NULL,1,0,1,1,'2023-08-09 14:12:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1365,'DRILLAMIN','删除',1320,'#',NULL,'#',3,'masterData:autocodeRule:remove',NULL,5,0,1,1,'2023-08-09 14:12:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1366,'DRILLAMIN','复制',1155,'#',NULL,'#',3,'masterData:item:copy',NULL,6,0,1,1,'2023-08-09 14:14:28','2023-08-09 14:14:44',1);
INSERT INTO `sys_menu` VALUES (1367,'DRILLAMIN','重新计算库存',1031,'#',NULL,'#',3,'produce:drillWorkOrder:recalculate',NULL,1,0,1,50,'2023-08-09 15:30:02','2023-08-09 15:30:17',50);
INSERT INTO `sys_menu` VALUES (1368,'DRILLAMIN','跳转任务拖拽',1031,'#',NULL,'#',3,'produce:drillWorkOrder:editDrillTask',NULL,4,0,1,50,'2023-08-09 15:31:01',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1369,'DRILLAMIN','选中',1031,'#',NULL,'#',3,'produce:drillWorkOrder:openLeft',NULL,1,0,1,50,'2023-08-09 15:33:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1370,'DRILLAMIN','提交',1031,'#',NULL,'#',3,'produce:drillWorkOrder:commit',NULL,6,0,1,50,'2023-08-09 15:36:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1371,'DRILLAMIN','重置任务',1031,'#',NULL,'#',3,'produce:drillWorkOrder:resetTask',NULL,7,0,1,50,'2023-08-09 15:37:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1372,'DRILLAMIN','报工',1350,'#',NULL,'#',3,'produce:reportWork:report',NULL,1,0,1,50,'2023-08-09 15:53:10','2023-09-05 11:42:24',1);
INSERT INTO `sys_menu` VALUES (1373,'DRILLAMIN','编辑',1350,'#',NULL,'#',3,'produce:reportWork:edit',NULL,1,0,1,50,'2023-08-09 15:53:24','2023-09-05 11:42:28',1);
INSERT INTO `sys_menu` VALUES (1374,'DRILLAMIN','新增',1350,'#',NULL,'#',3,'produce:reportWork:add',NULL,1,0,1,50,'2023-08-09 15:53:38','2023-09-05 11:42:32',1);
INSERT INTO `sys_menu` VALUES (1375,'DRILLAMIN','删除',1350,'#',NULL,'#',3,'produce:reportWork:remove',NULL,1,0,1,50,'2023-08-09 15:53:52','2023-09-05 11:42:36',1);
INSERT INTO `sys_menu` VALUES (1376,'DRILLAMIN','生成检验记录',1350,'#',NULL,'#',3,'produce:reportWork:checkRecord',NULL,1,0,1,50,'2023-08-09 15:54:47','2023-09-05 11:42:40',1);
INSERT INTO `sys_menu` VALUES (1377,'DRILLAMIN','撤销',1165,'#',NULL,'#',3,'produce:reportRecords:revoke',NULL,8,0,1,50,'2023-08-09 15:59:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1378,'DRILLAMIN','审批',1165,'#',NULL,'#',3,'produce:reportRecords:commit',NULL,9,0,1,50,'2023-08-09 15:59:36',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1379,'DRILLAMIN','调整路线',1122,'#',NULL,'#',3,'produce:workorder:config',NULL,8,0,1,50,'2023-08-09 16:19:55','2023-09-15 10:18:39',1);
INSERT INTO `sys_menu` VALUES (1380,'DRILLAMIN','跳转甘特图',1029,'#',NULL,'#',3,'produce:schedule:editGantt',NULL,9,0,1,50,'2023-08-09 16:20:48','2023-09-05 11:26:49',1);
INSERT INTO `sys_menu` VALUES (1381,'DRILLAMIN','查看',1320,'#',NULL,'#',3,'masterData:autocodeRule:view',NULL,7,0,1,1,'2023-08-12 17:10:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1382,'DRILLAMIN','重置',1320,'#',NULL,'#',3,'masterData:autocodeRule:reset',NULL,6,0,1,1,'2023-08-12 17:10:51','2023-09-22 15:38:36',1);
INSERT INTO `sys_menu` VALUES (1383,'DRILLAMIN','查看',1033,'#',NULL,'#',3,'device:deviceType:view',NULL,6,0,1,1,'2023-08-19 14:41:31','2023-08-28 14:18:55',1);
INSERT INTO `sys_menu` VALUES (1384,'DRILLAMIN','查看',1319,'#',NULL,'#',3,'device:subject:view',NULL,5,0,1,1,'2023-08-19 14:42:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1385,'DRILLAMIN','查看',1317,'#',NULL,'#',3,'device:repair:view',NULL,5,0,1,1,'2023-08-19 14:42:53',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1386,'DRILLAMIN','查看',1037,'#',NULL,'#',3,'device:schedulement:view',NULL,6,0,1,1,'2023-08-19 14:43:25',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1387,'DRILLAMIN','查看',1038,'#',NULL,'#',3,'alarm:event:view',NULL,5,0,1,1,'2023-08-19 14:43:51',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1388,'DRILLAMIN','查看',1136,'#',NULL,'#',3,'alarm:setting:view',NULL,5,0,1,1,'2023-08-19 14:44:15',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1389,'DRILLAMIN','查看',1039,'#',NULL,'#',3,'alarm:warn:view',NULL,5,0,1,1,'2023-08-19 14:44:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1390,'DRILLAMIN','查看',1055,'#',NULL,'#',3,'notify:setting:view',NULL,5,0,1,1,'2023-08-19 14:45:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1391,'DRILLAMIN','查看',1056,'#',NULL,'#',3,'notify:record:view',NULL,5,0,1,1,'2023-08-19 14:45:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1392,'DRILLAMIN','用户查看',2,'#',NULL,'#',3,'system:user:view',NULL,6,0,1,1,'2023-08-19 14:46:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1393,'DRILLAMIN','角色查看',3,'#',NULL,'#',3,'system:role:view',NULL,6,0,1,1,'2023-08-19 14:46:39',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1394,'DRILLAMIN','菜单查看',4,'#',NULL,'#',3,'system:menu:view',NULL,6,0,1,1,'2023-08-19 14:47:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1395,'DRILLAMIN','部门查看',5,'#',NULL,'#',3,'system:dept:view',NULL,6,0,1,1,'2023-08-19 14:47:49',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1396,'DRILLAMIN','岗位查看',6,'#',NULL,'#',3,'system:post:view',NULL,5,0,1,1,'2023-08-19 14:48:12',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1397,'DRILLAMIN','应用查看',8,'#',NULL,'#',3,'system:app:view',NULL,6,0,1,1,'2023-08-19 14:48:39',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1399,'DRILLAMIN','在线设备',1026,'#','onlineDevice','device/onlineDevice/index',2,'device:onlineDevice:list',NULL,7,0,1,1,'2023-08-21 13:44:42','2023-10-09 13:00:06',1);
INSERT INTO `sys_menu` VALUES (1400,'DRILLAMIN','历史数据',1158,'#','historicalData','wareHouse/historicalData/index',2,'wareHouse:historicalData:list',NULL,1,0,0,1,'2023-08-21 13:54:34','2023-08-21 14:10:56',1);
INSERT INTO `sys_menu` VALUES (1402,'DRILLAMIN','开始',1350,'#',NULL,'#',3,'produce:reportWork:start',NULL,1,0,1,1,'2023-08-28 15:00:29','2023-09-05 11:42:46',1);
INSERT INTO `sys_menu` VALUES (1403,'DRILLAMIN','完成',1350,'#',NULL,'#',3,'produce:reportWork:finish',NULL,1,0,1,1,'2023-08-28 15:01:32','2023-09-05 11:42:51',1);
INSERT INTO `sys_menu` VALUES (1417,'DRILLAMIN','生产任务',1024,'#','tasks','produce/tasks/index',2,'produce:tasks:list',NULL,7,0,1,1,'2023-09-05 10:57:57','2023-09-08 10:01:46',1);
INSERT INTO `sys_menu` VALUES (1419,'DRILLAMIN','任务图表',1024,'#','taskChart','produce/drillTask/index',2,'produce:drillTask:list',NULL,7,0,1,1,'2023-09-08 10:01:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1420,'DRILLAMIN','撤销',1417,'#',NULL,NULL,3,'produce:tasks:revoke',NULL,7,0,1,1,'2023-09-13 15:29:47',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1421,'DRILLAMIN','提交',1417,'#',NULL,NULL,3,'produce:tasks:commit',NULL,5,0,1,1,'2023-09-13 15:30:07',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1422,'DRILLAMIN','查询',1417,'#',NULL,NULL,3,'produce:tasks:list',NULL,1,0,1,1,'2023-09-13 15:30:36',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1423,'DRILLAMIN','查看',1417,'#',NULL,NULL,3,'produce:tasks:view',NULL,3,0,1,1,'2023-09-13 15:30:51',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1424,'DRILLAMIN','新增',1417,'#',NULL,NULL,3,'produce:tasks:add',NULL,1,0,1,1,'2023-09-13 15:31:06',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1425,'DRILLAMIN','删除',1417,'#',NULL,NULL,3,'produce:tasks:remove',NULL,4,0,1,1,'2023-09-13 15:31:25','2023-09-13 15:34:09',1);
INSERT INTO `sys_menu` VALUES (1426,'DRILLAMIN','开始',1417,'#',NULL,NULL,3,'produce:tasks:start',NULL,6,0,1,1,'2023-09-13 15:33:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1427,'DRILLAMIN','完成',1417,'#',NULL,NULL,3,'produce:tasks:finish',NULL,6,0,1,1,'2023-09-13 15:33:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1428,'DRILLAMIN','解除锁定',1399,'#',NULL,NULL,3,'device:onlineDevice:unlockIt',NULL,1,0,1,1,'2023-09-22 15:24:35',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1429,'DRILLAMIN','重置信号',1399,'#',NULL,NULL,3,'device:onlineDevice:resetSignal',NULL,1,0,1,1,'2023-09-22 15:24:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1430,'DRILLAMIN','审批',1156,'#',NULL,NULL,3,'masterData:productCategory:commit',NULL,7,0,1,1,'2023-09-22 16:17:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1431,'DRILLAMIN','审批',1157,'#',NULL,NULL,3,'produce:route:commit',NULL,6,0,1,1,'2023-09-22 16:24:07',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1432,'DRILLAMIN','解析',1161,'#',NULL,NULL,3,'material:drillFile:parse',NULL,6,0,1,1,'2023-09-22 16:48:52',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1433,'DRILLAMIN','下载文件',1162,'#',NULL,NULL,3,'material:atpFile:download',NULL,6,0,1,1,'2023-09-22 16:50:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1434,'DRILLAMIN','排刀',1162,'#',NULL,NULL,3,'material:atpFile:rowknives',NULL,6,0,1,1,'2023-09-22 16:50:46',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1435,'DRILLAMIN','出库',1160,'#',NULL,NULL,3,'warehouse:materialStock:outbound',NULL,7,0,1,1,'2023-09-22 17:05:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1436,'DRILLAMIN','撤销',1156,'#',NULL,NULL,3,'masterData:productCategory:revoke',NULL,8,0,1,1,'2023-09-25 09:53:43',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1439,'DRILLAMIN','撤销',1157,'#',NULL,NULL,3,'produce:route:revoke',NULL,6,0,1,1,'2023-09-25 09:55:29','2023-09-25 09:56:12',1);
INSERT INTO `sys_menu` VALUES (1440,'DRILLAMIN','调度配置',1026,'#','scheduleConfig','device/scheduleConfig/index',2,'device:scheduleConfig:list',NULL,12,0,1,1,'2023-10-09 15:10:20','2023-11-27 10:03:39',1);
INSERT INTO `sys_menu` VALUES (1441,'DRILLAMIN','测试页面1',1026,'#','test1','device/test/index',2,NULL,NULL,2,0,1,1,'2023-10-10 13:45:24','2023-11-16 16:33:13',1);
INSERT INTO `sys_menu` VALUES (1442,'DRILLAMIN','关联工艺路线设置',1156,'#',NULL,NULL,3,'masterData:productCategory:setRoute',NULL,12,0,1,1,'2023-10-13 09:38:24',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1443,'DRILLAMIN','关联工艺路线上移',1156,'#',NULL,NULL,3,'masterData:productCategory:moveUp',NULL,12,0,1,1,'2023-10-13 09:39:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1444,'DRILLAMIN','关联工艺路线删除',1156,'#',NULL,NULL,3,'masterData:productCategory:removeRoute',NULL,13,0,1,1,'2023-10-13 09:41:01','2023-10-13 09:43:33',1);
INSERT INTO `sys_menu` VALUES (1445,'DRILLAMIN','关联工艺路线下移',1156,'#',NULL,NULL,3,'masterData:productCategory:moveDown',NULL,12,0,1,1,'2023-10-13 09:41:43',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1446,'DRILLAMIN','关联工艺路线添加',1156,'#',NULL,NULL,3,'masterData:productCategory:addRoute',NULL,11,0,1,1,'2023-10-13 09:44:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1447,'DRILLAMIN','关联工艺路线新增',1153,'#',NULL,NULL,3,'masterData:workstation:addRoute',NULL,10,0,1,1,'2023-10-13 09:50:07',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1448,'DRILLAMIN','关联工艺路线删除',1153,'#',NULL,NULL,3,'masterData:workstation:removeRoute',NULL,11,0,1,1,'2023-10-13 09:50:34',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1449,'DRILLAMIN','新增任务',1029,'#',NULL,NULL,3,'produce:schedule:addTask',NULL,9,0,1,1,'2023-10-13 10:02:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1450,'DRILLAMIN','删除任务',1029,'#',NULL,NULL,3,'produce:schedule:removeTask',NULL,10,0,1,1,'2023-10-13 10:02:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1451,'DRILLAMIN','修改任务',1029,'#',NULL,NULL,3,'produce:schedule:editTask',NULL,11,0,1,1,'2023-10-13 10:02:47','2023-10-13 10:04:30',1);
INSERT INTO `sys_menu` VALUES (1452,'DRILLAMIN','提交任务',1029,'#',NULL,NULL,3,'produce:schedule:commitTask',NULL,12,0,1,1,'2023-10-13 10:03:43',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1453,'DRILLAMIN','撤销任务',1029,'#',NULL,NULL,3,'produce:schedule:revokeTask',NULL,13,0,1,1,'2023-10-13 10:04:06',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1456,'DRILLAMIN','转发任务',1417,'#',NULL,NULL,3,'produce:tasks:transferTask',NULL,8,0,1,1,'2023-10-15 17:11:32','2023-10-15 17:11:52',1);
INSERT INTO `sys_menu` VALUES (1457,'DRILLAMIN','系统配置',1,'form','parameter','system/parameter/index',2,'system:parameter:list',NULL,12,0,1,1,'2023-10-18 08:56:24','2023-11-20 15:35:05',1);
INSERT INTO `sys_menu` VALUES (1458,'DRILLAMIN','新增',1457,'#',NULL,NULL,3,'system:parameter:add',NULL,1,0,1,1,'2023-10-27 11:38:38',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1459,'DRILLAMIN','编辑',1457,'#',NULL,NULL,3,'system:parameter:edit',NULL,1,0,1,1,'2023-10-27 11:38:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1460,'DRILLAMIN','删除',1457,'#',NULL,NULL,3,'system:parameter:remove',NULL,1,0,1,1,'2023-10-27 11:39:13',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1461,'DRILLAMIN','查询',1440,'#',NULL,NULL,3,'device:scheduleConfig:list',NULL,1,0,1,1,'2023-11-03 09:48:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1462,'DRILLAMIN','新增',1440,'#',NULL,NULL,3,'device:scheduleConfig:add',NULL,1,0,1,1,'2023-11-03 09:48:35',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1463,'DRILLAMIN','修改',1440,'#',NULL,NULL,3,'device:scheduleConfig:edit',NULL,1,0,1,1,'2023-11-03 09:48:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1464,'DRILLAMIN','删除',1440,'#',NULL,NULL,3,'device:scheduleConfig:remove',NULL,1,0,1,1,'2023-11-03 09:49:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1465,'DRILLAMIN','查看',1440,'#',NULL,NULL,3,'device:scheduleConfig:view',NULL,1,0,1,1,'2023-11-03 09:49:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1466,'DRILLAMIN','配置路线',1440,'#',NULL,NULL,3,'device:scheduleConfig:config',NULL,1,0,1,1,'2023-11-03 09:49:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1467,'DRILLAMIN','查询',1457,'#',NULL,NULL,3,'system:parameter:list',NULL,1,0,1,1,'2023-11-03 09:53:49',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1468,'DRILLAMIN','调度大屏',1026,'#','device/dashboard','device/dashboard',2,'device:dashboard:list',NULL,1,0,1,1,'2023-11-08 17:18:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1469,'DRILLAMIN','详情',1399,'#',NULL,NULL,3,'device:onlineDevice:detail',NULL,1,0,1,1,'2023-11-15 08:47:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1470,'DRILLAMIN','设备负载',1026,'#','deviceLoad','device/deviceLoad/index',2,'device:deviceLoad:list',NULL,10,0,1,1,'2023-11-16 13:59:11','2023-11-27 10:03:24',1);
INSERT INTO `sys_menu` VALUES (1471,'DRILLAMIN','查看任务',1470,'#',NULL,NULL,3,'device:deviceLoad:view',NULL,1,0,1,1,'2023-11-16 16:32:36',NULL,NULL);
/*!40000 ALTER TABLE `sys_menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_menu_auth`
--

DROP TABLE IF EXISTS `sys_menu_auth`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_menu_auth` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `menu_id` int(11) NOT NULL COMMENT '菜单Id',
  `authorize_id` int(11) NOT NULL COMMENT '授权Id(角色Id或者用户Id)',
  `authorize_type` int(11) DEFAULT NULL COMMENT '授权类型(1角色 2用户)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=27422 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='角色--菜单权限';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu_auth`
--

LOCK TABLES `sys_menu_auth` WRITE;
/*!40000 ALTER TABLE `sys_menu_auth` DISABLE KEYS */;
INSERT INTO `sys_menu_auth` VALUES (11898,1,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11899,2,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11900,1000,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11901,1001,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11902,1002,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11903,1003,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11904,1004,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11905,1005,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11906,1006,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11907,3,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11908,1007,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11909,1008,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11910,1009,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11911,1010,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11912,1011,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11913,4,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11914,1012,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11915,1013,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11916,1014,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11917,1015,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11918,5,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11919,1016,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11920,1017,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11921,1018,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11922,1019,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11923,6,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11924,1020,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11925,1021,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11926,1022,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11927,1023,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11928,1024,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11929,1029,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11930,1077,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11931,1078,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11932,1079,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11933,1080,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11934,1081,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11935,1280,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11936,1310,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11937,1030,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11938,1072,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11939,1073,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11940,1074,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11941,1075,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11942,1076,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11943,1313,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11944,1031,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11945,1082,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11946,1083,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11947,1084,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11948,1085,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11949,1086,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11950,1311,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11951,1122,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11952,1123,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11953,1124,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11954,1125,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11955,1126,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11956,1127,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11957,1279,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11958,1309,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11959,1128,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11960,1129,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11961,1130,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11962,1131,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11963,1132,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11964,1133,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11965,1307,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11966,1157,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11967,1219,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11968,1220,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11969,1221,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11970,1222,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11971,1223,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11972,1224,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11973,1292,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11974,1293,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11975,1294,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11976,1295,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11977,1296,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11978,1297,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11979,1298,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11980,1299,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11981,1300,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11982,1301,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11983,1302,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11984,1303,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11985,1304,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11986,1305,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11987,1306,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11988,1165,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11989,1166,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11990,1167,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11991,1168,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11992,1169,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11993,1170,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11994,1281,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11995,1312,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11996,1314,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11997,1315,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11998,1025,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (11999,1036,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12000,1062,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12001,1063,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12002,1064,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12003,1065,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12004,1066,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12005,1161,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12006,1171,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12007,1172,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12008,1173,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12009,1174,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12010,1175,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12011,1176,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12012,1162,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12013,1177,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12014,1178,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12015,1179,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12016,1180,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12017,1181,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12018,1182,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12019,1282,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12020,1283,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12021,1284,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12022,1285,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12023,1286,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12024,1026,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12025,1033,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12026,1043,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12027,1044,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12028,1045,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12029,1052,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12030,1035,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12031,1057,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12032,1058,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12033,1059,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12034,1060,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12035,1061,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12036,1317,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12037,1319,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12038,1027,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12039,1038,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12040,1097,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12041,1098,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12042,1099,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12043,1100,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12044,1101,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12045,1039,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12046,1102,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12047,1103,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12048,1104,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12049,1105,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12050,1106,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12051,1107,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12052,1136,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12053,1137,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12054,1138,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12055,1139,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12056,1140,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12057,1054,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12058,1055,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12059,1108,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12060,1109,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12061,1110,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12062,1111,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12063,1112,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12064,1056,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12065,1113,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12066,1114,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12067,1115,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12068,1116,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12069,1117,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12070,1148,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12071,1149,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12072,1231,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12073,1232,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12074,1233,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12075,1234,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12076,1235,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12077,1236,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12078,1321,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12079,1150,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12080,1237,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12081,1238,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12082,1239,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12083,1240,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12084,1241,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12085,1242,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12086,1151,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12087,1243,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12088,1244,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12089,1245,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12090,1246,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12091,1247,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12092,1248,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12093,1152,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12094,1249,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12095,1250,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12096,1251,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12097,1252,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12098,1253,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12099,1254,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12100,1153,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12101,1255,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12102,1256,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12103,1257,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12104,1258,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12105,1259,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12106,1260,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12107,1154,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12108,1261,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12109,1262,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12110,1263,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12111,1264,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12112,1265,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12113,1266,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12114,1155,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12115,1267,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12116,1268,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12117,1269,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12118,1270,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12119,1271,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12120,1272,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12121,1287,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12122,1288,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12123,1289,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12124,1290,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12125,1291,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12126,1156,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12127,1273,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12128,1274,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12129,1275,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12130,1276,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12131,1277,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12132,1278,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12133,1320,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12134,1158,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12135,1159,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12136,1207,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12137,1208,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12138,1209,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12139,1210,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12140,1211,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12141,1212,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12142,1160,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12143,1213,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12144,1214,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12145,1215,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12146,1216,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12147,1217,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12148,1218,60,1,33,'2023-05-22 03:03:11');
INSERT INTO `sys_menu_auth` VALUES (12149,1,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12150,2,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12151,1000,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12152,1001,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12153,1002,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12154,1003,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12155,1004,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12156,1005,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12157,1006,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12158,3,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12159,1007,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12160,1008,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12161,1009,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12162,1010,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12163,1011,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12164,4,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12165,1012,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12166,1013,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12167,1014,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12168,1015,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12169,5,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12170,1016,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12171,1017,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12172,1018,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12173,1019,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12174,6,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12175,1020,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12176,1021,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12177,1022,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12178,1023,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12179,1024,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12180,1029,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12181,1077,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12182,1078,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12183,1079,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12184,1080,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12185,1081,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12186,1280,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12187,1310,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12188,1030,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12189,1072,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12190,1073,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12191,1074,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12192,1075,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12193,1076,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12194,1313,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12195,1031,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12196,1082,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12197,1083,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12198,1084,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12199,1085,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12200,1086,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12201,1311,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12202,1122,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12203,1123,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12204,1124,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12205,1125,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12206,1126,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12207,1127,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12208,1279,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12209,1309,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12210,1128,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12211,1129,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12212,1130,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12213,1131,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12214,1132,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12215,1133,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12216,1307,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12217,1157,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12218,1219,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12219,1220,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12220,1221,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12221,1222,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12222,1223,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12223,1224,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12224,1292,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12225,1293,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12226,1294,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12227,1295,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12228,1296,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12229,1297,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12230,1298,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12231,1299,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12232,1300,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12233,1301,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12234,1302,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12235,1303,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12236,1304,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12237,1305,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12238,1306,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12239,1165,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12240,1166,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12241,1167,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12242,1168,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12243,1169,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12244,1170,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12245,1281,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12246,1312,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12247,1314,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12248,1315,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12249,1025,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12250,1036,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12251,1062,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12252,1063,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12253,1064,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12254,1065,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12255,1066,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12256,1161,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12257,1171,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12258,1172,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12259,1173,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12260,1174,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12261,1175,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12262,1176,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12263,1162,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12264,1177,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12265,1178,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12266,1179,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12267,1180,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12268,1181,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12269,1182,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12270,1282,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12271,1283,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12272,1284,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12273,1285,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12274,1286,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12275,1026,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12276,1033,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12277,1043,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12278,1044,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12279,1045,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12280,1052,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12281,1035,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12282,1057,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12283,1058,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12284,1059,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12285,1060,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12286,1061,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12287,1317,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12288,1319,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12289,1027,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12290,1038,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12291,1097,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12292,1098,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12293,1099,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12294,1100,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12295,1101,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12296,1039,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12297,1102,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12298,1103,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12299,1104,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12300,1105,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12301,1106,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12302,1107,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12303,1136,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12304,1137,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12305,1138,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12306,1139,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12307,1140,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12308,1054,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12309,1055,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12310,1108,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12311,1109,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12312,1110,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12313,1111,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12314,1112,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12315,1056,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12316,1113,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12317,1114,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12318,1115,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12319,1116,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12320,1117,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12321,1148,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12322,1149,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12323,1231,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12324,1232,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12325,1233,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12326,1234,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12327,1235,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12328,1236,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12329,1321,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12330,1150,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12331,1237,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12332,1238,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12333,1239,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12334,1240,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12335,1241,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12336,1242,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12337,1151,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12338,1243,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12339,1244,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12340,1245,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12341,1246,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12342,1247,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12343,1248,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12344,1152,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12345,1249,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12346,1250,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12347,1251,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12348,1252,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12349,1253,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12350,1254,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12351,1153,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12352,1255,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12353,1256,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12354,1257,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12355,1258,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12356,1259,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12357,1260,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12358,1154,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12359,1261,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12360,1262,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12361,1263,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12362,1264,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12363,1265,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12364,1266,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12365,1155,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12366,1267,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12367,1268,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12368,1269,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12369,1270,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12370,1271,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12371,1272,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12372,1287,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12373,1288,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12374,1289,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12375,1290,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12376,1291,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12377,1156,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12378,1273,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12379,1274,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12380,1275,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12381,1276,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12382,1277,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12383,1278,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12384,1320,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12385,1158,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12386,1159,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12387,1207,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12388,1208,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12389,1209,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12390,1210,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12391,1211,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12392,1212,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12393,1160,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12394,1213,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12395,1214,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12396,1215,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12397,1216,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12398,1217,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (12399,1218,61,1,33,'2023-05-22 03:03:33');
INSERT INTO `sys_menu_auth` VALUES (24646,1,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24647,2,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24648,1000,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24649,1001,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24650,1002,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24651,1003,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24652,1004,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24653,1005,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24654,1006,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24655,1392,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24656,3,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24657,1007,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24658,1008,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24659,1009,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24660,1010,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24661,1011,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24662,1393,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24663,4,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24664,1012,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24665,1013,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24666,1014,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24667,1015,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24668,1394,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24669,5,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24670,1016,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24671,1017,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24672,1018,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24673,1019,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24674,1395,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24675,6,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24676,1020,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24677,1021,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24678,1022,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24679,1023,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24680,1396,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24681,1024,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24682,1029,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24683,1077,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24684,1078,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24685,1079,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24686,1080,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24687,1081,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24688,1280,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24689,1310,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24690,1380,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24691,1030,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24692,1072,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24693,1073,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24694,1074,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24695,1075,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24696,1076,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24697,1313,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24698,1332,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24699,1031,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24700,1082,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24701,1083,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24702,1084,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24703,1085,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24704,1086,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24705,1311,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24706,1339,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24707,1367,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24708,1368,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24709,1369,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24710,1370,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24711,1371,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24712,1122,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24713,1123,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24714,1124,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24715,1125,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24716,1126,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24717,1127,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24718,1279,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24719,1309,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24720,1330,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24721,1379,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24722,1128,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24723,1129,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24724,1130,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24725,1131,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24726,1132,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24727,1133,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24728,1307,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24729,1329,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24730,1157,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24731,1219,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24732,1220,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24733,1221,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24734,1222,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24735,1223,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24736,1224,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24737,1292,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24738,1293,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24739,1294,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24740,1295,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24741,1296,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24742,1297,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24743,1298,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24744,1299,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24745,1300,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24746,1301,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24747,1302,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24748,1303,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24749,1304,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24750,1305,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24751,1306,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24752,1165,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24753,1166,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24754,1167,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24755,1168,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24756,1169,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24757,1170,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24758,1281,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24759,1312,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24760,1331,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24761,1377,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24762,1378,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24763,1314,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24764,1315,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24765,1333,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24766,1334,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24767,1340,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24768,1354,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24769,1350,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24770,1351,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24771,1372,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24772,1373,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24773,1374,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24774,1375,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24775,1376,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24776,1402,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24777,1403,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24778,1417,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24779,1420,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24780,1421,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24781,1422,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24782,1423,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24783,1424,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24784,1425,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24785,1426,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24786,1427,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24787,1419,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24788,1025,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24789,1036,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24790,1062,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24791,1063,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24792,1064,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24793,1065,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24794,1066,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24795,1341,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24796,1343,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24797,1161,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24798,1171,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24799,1172,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24800,1173,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24801,1174,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24802,1175,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24803,1176,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24804,1342,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24805,1162,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24806,1177,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24807,1178,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24808,1179,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24809,1180,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24810,1181,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24811,1182,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24812,1282,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24813,1283,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24814,1284,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24815,1285,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24816,1286,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24817,1026,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24818,1033,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24819,1043,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24820,1044,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24821,1045,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24822,1052,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24823,1383,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24824,1035,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24825,1057,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24826,1058,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24827,1059,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24828,1060,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24829,1061,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24830,1355,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24831,1037,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24832,1068,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24833,1069,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24834,1070,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24835,1071,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24836,1352,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24837,1353,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24838,1362,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24839,1363,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24840,1386,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24841,1317,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24842,1344,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24843,1345,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24844,1346,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24845,1347,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24846,1348,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24847,1385,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24848,1319,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24849,1357,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24850,1358,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24851,1359,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24852,1360,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24853,1361,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24854,1384,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24855,1399,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24856,1428,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24857,1429,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24858,1027,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24859,1038,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24860,1097,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24861,1098,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24862,1099,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24863,1100,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24864,1101,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24865,1335,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24866,1387,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24867,1039,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24868,1102,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24869,1103,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24870,1104,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24871,1105,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24872,1106,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24873,1107,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24874,1336,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24875,1389,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24876,1136,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24877,1137,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24878,1138,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24879,1139,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24880,1140,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24881,1356,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24882,1388,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24883,1054,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24884,1055,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24885,1108,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24886,1109,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24887,1110,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24888,1111,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24889,1112,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24890,1390,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24891,1056,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24892,1113,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24893,1114,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24894,1115,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24895,1116,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24896,1117,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24897,1349,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24898,1391,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24899,1148,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24900,1149,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24901,1231,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24902,1232,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24903,1233,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24904,1234,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24905,1235,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24906,1236,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24907,1321,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24908,1324,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24909,1150,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24910,1237,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24911,1238,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24912,1239,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24913,1240,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24914,1241,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24915,1242,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24916,1327,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24917,1151,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24918,1243,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24919,1244,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24920,1245,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24921,1246,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24922,1247,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24923,1248,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24924,1328,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24925,1152,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24926,1249,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24927,1250,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24928,1251,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24929,1252,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24930,1253,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24931,1254,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24932,1153,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24933,1255,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24934,1256,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24935,1257,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24936,1258,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24937,1259,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24938,1260,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24939,1326,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24940,1154,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24941,1261,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24942,1262,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24943,1263,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24944,1264,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24945,1265,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24946,1266,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24947,1325,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24948,1155,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24949,1267,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24950,1268,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24951,1269,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24952,1270,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24953,1271,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24954,1272,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24955,1287,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24956,1288,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24957,1289,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24958,1290,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24959,1291,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24960,1338,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24961,1366,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24962,1156,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24963,1273,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24964,1274,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24965,1275,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24966,1276,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24967,1277,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24968,1278,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24969,1337,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24970,1320,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24971,1322,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24972,1323,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24973,1364,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24974,1365,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24975,1381,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24976,1382,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24977,1158,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24978,1159,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24979,1207,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24980,1208,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24981,1209,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24982,1210,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24983,1211,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24984,1212,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24985,1160,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24986,1213,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24987,1214,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24988,1215,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24989,1216,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24990,1217,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24991,1218,2,1,1,'2023-09-22 15:50:02');
INSERT INTO `sys_menu_auth` VALUES (24994,1,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (24995,2,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (24996,1000,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (24997,1001,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (24998,1002,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (24999,1003,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25000,1004,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25001,1005,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25002,1006,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25003,1392,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25004,3,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25005,1007,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25006,1008,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25007,1009,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25008,1010,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25009,1011,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25010,1393,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25011,4,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25012,1012,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25013,1013,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25014,1014,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25015,1015,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25016,1394,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25017,5,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25018,1016,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25019,1017,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25020,1018,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25021,1019,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25022,1395,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25023,6,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25024,1020,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25025,1021,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25026,1022,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25027,1023,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25028,1396,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25029,1024,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25030,1029,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25031,1077,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25032,1078,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25033,1079,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25034,1080,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25035,1081,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25036,1280,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25037,1310,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25038,1380,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25039,1030,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25040,1072,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25041,1073,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25042,1074,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25043,1075,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25044,1076,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25045,1313,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25046,1332,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25047,1031,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25048,1082,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25049,1083,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25050,1084,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25051,1085,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25052,1086,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25053,1311,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25054,1339,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25055,1367,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25056,1368,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25057,1369,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25058,1370,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25059,1371,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25060,1122,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25061,1123,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25062,1124,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25063,1125,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25064,1126,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25065,1127,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25066,1279,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25067,1309,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25068,1330,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25069,1379,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25070,1128,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25071,1129,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25072,1130,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25073,1131,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25074,1132,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25075,1133,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25076,1307,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25077,1329,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25078,1157,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25079,1219,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25080,1220,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25081,1221,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25082,1222,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25083,1223,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25084,1224,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25085,1292,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25086,1293,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25087,1294,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25088,1295,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25089,1296,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25090,1297,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25091,1298,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25092,1299,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25093,1300,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25094,1301,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25095,1302,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25096,1303,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25097,1304,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25098,1305,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25099,1306,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25100,1165,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25101,1166,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25102,1167,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25103,1168,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25104,1169,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25105,1170,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25106,1281,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25107,1312,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25108,1331,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25109,1377,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25110,1378,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25111,1314,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25112,1315,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25113,1333,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25114,1334,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25115,1340,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25116,1354,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25117,1350,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25118,1351,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25119,1372,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25120,1373,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25121,1374,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25122,1375,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25123,1376,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25124,1402,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25125,1403,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25126,1417,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25127,1420,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25128,1421,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25129,1422,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25130,1423,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25131,1424,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25132,1425,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25133,1426,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25134,1427,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25135,1419,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25136,1025,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25137,1036,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25138,1062,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25139,1063,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25140,1064,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25141,1065,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25142,1066,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25143,1341,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25144,1343,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25145,1161,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25146,1171,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25147,1172,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25148,1173,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25149,1174,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25150,1175,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25151,1176,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25152,1342,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25153,1162,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25154,1177,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25155,1178,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25156,1179,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25157,1180,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25158,1181,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25159,1182,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25160,1282,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25161,1283,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25162,1284,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25163,1285,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25164,1286,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25165,1026,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25166,1033,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25167,1043,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25168,1044,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25169,1045,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25170,1052,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25171,1383,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25172,1035,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25173,1057,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25174,1058,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25175,1059,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25176,1060,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25177,1061,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25178,1355,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25179,1037,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25180,1068,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25181,1069,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25182,1070,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25183,1071,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25184,1352,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25185,1353,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25186,1362,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25187,1363,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25188,1386,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25189,1317,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25190,1344,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25191,1345,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25192,1346,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25193,1347,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25194,1348,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25195,1385,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25196,1319,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25197,1357,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25198,1358,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25199,1359,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25200,1360,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25201,1361,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25202,1384,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25203,1399,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25204,1428,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25205,1429,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25206,1027,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25207,1038,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25208,1097,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25209,1098,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25210,1099,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25211,1100,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25212,1101,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25213,1335,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25214,1387,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25215,1039,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25216,1102,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25217,1103,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25218,1104,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25219,1105,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25220,1106,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25221,1107,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25222,1336,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25223,1389,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25224,1136,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25225,1137,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25226,1138,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25227,1139,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25228,1140,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25229,1356,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25230,1388,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25231,1054,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25232,1055,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25233,1108,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25234,1109,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25235,1110,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25236,1111,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25237,1112,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25238,1390,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25239,1056,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25240,1113,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25241,1114,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25242,1115,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25243,1116,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25244,1117,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25245,1349,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25246,1391,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25247,1148,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25248,1149,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25249,1231,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25250,1232,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25251,1233,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25252,1234,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25253,1235,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25254,1236,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25255,1321,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25256,1324,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25257,1150,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25258,1237,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25259,1238,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25260,1239,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25261,1240,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25262,1241,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25263,1242,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25264,1327,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25265,1151,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25266,1243,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25267,1244,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25268,1245,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25269,1246,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25270,1247,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25271,1248,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25272,1328,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25273,1152,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25274,1249,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25275,1250,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25276,1251,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25277,1252,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25278,1253,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25279,1254,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25280,1153,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25281,1255,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25282,1256,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25283,1257,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25284,1258,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25285,1259,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25286,1260,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25287,1326,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25288,1154,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25289,1261,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25290,1262,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25291,1263,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25292,1264,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25293,1265,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25294,1266,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25295,1325,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25296,1155,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25297,1267,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25298,1268,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25299,1269,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25300,1270,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25301,1271,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25302,1272,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25303,1287,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25304,1288,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25305,1289,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25306,1290,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25307,1291,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25308,1338,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25309,1366,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25310,1156,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25311,1273,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25312,1274,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25313,1275,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25314,1276,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25315,1277,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25316,1278,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25317,1337,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25318,1320,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25319,1322,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25320,1323,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25321,1364,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25322,1365,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25323,1381,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25324,1382,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25325,1158,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25326,1159,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25327,1207,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25328,1208,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25329,1209,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25330,1210,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25331,1211,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25332,1212,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25333,1160,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25334,1213,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25335,1214,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25336,1215,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25337,1216,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25338,1217,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25339,1218,3,1,1,'2023-09-22 15:51:09');
INSERT INTO `sys_menu_auth` VALUES (25340,1,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25341,2,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25342,1000,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25343,1001,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25344,1002,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25345,1003,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25346,1004,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25347,1005,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25348,1006,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25349,1392,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25350,3,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25351,1007,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25352,1008,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25353,1009,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25354,1010,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25355,1011,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25356,1393,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25357,4,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25358,1012,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25359,1013,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25360,1014,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25361,1015,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25362,1394,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25363,5,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25364,1016,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25365,1017,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25366,1018,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25367,1019,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25368,1395,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25369,6,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25370,1020,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25371,1021,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25372,1022,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25373,1023,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25374,1396,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25375,1024,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25376,1029,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25377,1077,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25378,1078,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25379,1079,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25380,1080,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25381,1081,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25382,1280,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25383,1310,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25384,1380,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25385,1030,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25386,1072,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25387,1073,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25388,1074,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25389,1075,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25390,1076,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25391,1313,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25392,1332,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25393,1031,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25394,1082,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25395,1083,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25396,1084,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25397,1085,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25398,1086,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25399,1311,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25400,1339,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25401,1367,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25402,1368,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25403,1369,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25404,1370,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25405,1371,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25406,1122,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25407,1123,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25408,1124,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25409,1125,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25410,1126,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25411,1127,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25412,1279,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25413,1309,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25414,1330,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25415,1379,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25416,1128,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25417,1129,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25418,1130,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25419,1131,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25420,1132,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25421,1133,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25422,1307,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25423,1329,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25424,1157,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25425,1219,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25426,1220,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25427,1221,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25428,1222,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25429,1223,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25430,1224,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25431,1292,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25432,1293,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25433,1294,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25434,1295,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25435,1296,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25436,1297,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25437,1298,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25438,1299,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25439,1300,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25440,1301,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25441,1302,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25442,1303,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25443,1304,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25444,1305,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25445,1306,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25446,1165,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25447,1166,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25448,1167,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25449,1168,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25450,1169,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25451,1170,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25452,1281,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25453,1312,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25454,1331,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25455,1377,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25456,1378,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25457,1314,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25458,1315,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25459,1333,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25460,1334,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25461,1340,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25462,1354,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25463,1350,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25464,1351,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25465,1372,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25466,1373,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25467,1374,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25468,1375,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25469,1376,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25470,1402,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25471,1403,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25472,1417,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25473,1420,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25474,1421,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25475,1422,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25476,1423,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25477,1424,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25478,1425,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25479,1426,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25480,1427,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25481,1419,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25482,1025,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25483,1036,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25484,1062,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25485,1063,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25486,1064,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25487,1065,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25488,1066,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25489,1341,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25490,1343,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25491,1161,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25492,1171,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25493,1172,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25494,1173,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25495,1174,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25496,1175,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25497,1176,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25498,1342,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25499,1162,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25500,1177,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25501,1178,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25502,1179,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25503,1180,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25504,1181,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25505,1182,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25506,1282,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25507,1283,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25508,1284,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25509,1285,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25510,1286,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25511,1026,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25512,1033,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25513,1043,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25514,1044,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25515,1045,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25516,1052,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25517,1383,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25518,1035,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25519,1057,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25520,1058,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25521,1059,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25522,1060,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25523,1061,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25524,1355,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25525,1037,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25526,1068,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25527,1069,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25528,1070,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25529,1071,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25530,1352,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25531,1353,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25532,1362,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25533,1363,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25534,1386,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25535,1317,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25536,1344,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25537,1345,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25538,1346,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25539,1347,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25540,1348,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25541,1385,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25542,1319,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25543,1357,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25544,1358,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25545,1359,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25546,1360,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25547,1361,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25548,1384,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25549,1399,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25550,1428,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25551,1429,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25552,1027,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25553,1038,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25554,1097,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25555,1098,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25556,1099,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25557,1100,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25558,1101,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25559,1335,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25560,1387,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25561,1039,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25562,1102,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25563,1103,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25564,1104,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25565,1105,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25566,1106,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25567,1107,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25568,1336,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25569,1389,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25570,1136,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25571,1137,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25572,1138,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25573,1139,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25574,1140,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25575,1356,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25576,1388,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25577,1054,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25578,1055,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25579,1108,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25580,1109,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25581,1110,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25582,1111,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25583,1112,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25584,1390,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25585,1056,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25586,1113,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25587,1114,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25588,1115,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25589,1116,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25590,1117,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25591,1349,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25592,1391,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25593,1148,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25594,1149,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25595,1231,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25596,1232,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25597,1233,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25598,1234,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25599,1235,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25600,1236,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25601,1321,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25602,1324,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25603,1150,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25604,1237,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25605,1238,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25606,1239,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25607,1240,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25608,1241,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25609,1242,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25610,1327,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25611,1151,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25612,1243,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25613,1244,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25614,1245,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25615,1246,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25616,1247,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25617,1248,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25618,1328,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25619,1152,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25620,1249,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25621,1250,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25622,1251,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25623,1252,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25624,1253,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25625,1254,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25626,1153,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25627,1255,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25628,1256,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25629,1257,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25630,1258,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25631,1259,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25632,1260,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25633,1326,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25634,1154,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25635,1261,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25636,1262,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25637,1263,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25638,1264,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25639,1265,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25640,1266,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25641,1325,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25642,1155,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25643,1267,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25644,1268,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25645,1269,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25646,1270,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25647,1271,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25648,1272,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25649,1287,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25650,1288,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25651,1289,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25652,1290,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25653,1291,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25654,1338,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25655,1366,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25656,1156,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25657,1273,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25658,1274,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25659,1275,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25660,1276,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25661,1277,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25662,1278,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25663,1337,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25664,1320,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25665,1322,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25666,1323,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25667,1364,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25668,1365,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25669,1381,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25670,1382,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25671,1158,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25672,1159,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25673,1207,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25674,1208,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25675,1209,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25676,1210,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25677,1211,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25678,1212,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25679,1160,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25680,1213,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25681,1214,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25682,1215,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25683,1216,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25684,1217,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25685,1218,6,1,1,'2023-09-22 15:51:12');
INSERT INTO `sys_menu_auth` VALUES (25686,1,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25687,1024,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25688,1025,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25689,1026,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25690,1027,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25691,1054,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25692,1148,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (25693,1158,8,1,1,'2023-09-22 15:51:23');
INSERT INTO `sys_menu_auth` VALUES (27027,1026,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27028,1,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27029,2,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27030,1000,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27031,1001,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27032,1002,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27033,1003,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27034,1004,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27035,1005,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27036,1006,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27037,1392,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27038,3,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27039,1007,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27040,1008,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27041,1009,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27042,1010,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27043,1011,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27044,1393,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27045,4,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27046,1012,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27047,1013,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27048,1014,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27049,1015,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27050,1394,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27051,5,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27052,1016,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27053,1017,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27054,1018,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27055,1019,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27056,1395,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27057,6,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27058,1020,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27059,1021,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27060,1022,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27061,1023,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27062,1396,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27063,1457,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27064,1458,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27065,1459,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27066,1460,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27067,1467,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27068,1468,12,1,1,'2023-11-21 16:09:19');
INSERT INTO `sys_menu_auth` VALUES (27242,1024,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27243,1026,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27244,1148,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27245,1,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27246,2,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27247,1000,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27248,1001,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27249,1002,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27250,1003,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27251,1004,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27252,1005,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27253,1006,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27254,1392,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27255,3,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27256,1007,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27257,1008,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27258,1009,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27259,1010,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27260,1011,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27261,1393,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27262,4,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27263,1012,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27264,1013,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27265,1014,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27266,1015,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27267,1394,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27268,5,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27269,1016,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27270,1017,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27271,1018,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27272,1019,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27273,1395,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27274,6,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27275,1020,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27276,1021,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27277,1022,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27278,1023,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27279,1396,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27280,1457,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27281,1458,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27282,1459,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27283,1460,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27284,1467,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27285,1122,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27286,1123,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27287,1124,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27288,1125,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27289,1126,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27290,1127,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27291,1279,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27292,1309,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27293,1330,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27294,1379,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27295,1128,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27296,1129,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27297,1130,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27298,1131,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27299,1132,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27300,1133,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27301,1307,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27302,1329,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27303,1157,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27304,1219,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27305,1220,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27306,1221,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27307,1222,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27308,1223,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27309,1224,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27310,1292,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27311,1293,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27312,1294,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27313,1295,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27314,1296,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27315,1297,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27316,1298,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27317,1299,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27318,1300,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27319,1301,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27320,1302,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27321,1303,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27322,1304,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27323,1305,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27324,1306,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27325,1431,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27326,1439,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27327,1417,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27328,1420,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27329,1421,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27330,1422,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27331,1423,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27332,1424,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27333,1425,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27334,1426,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27335,1427,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27336,1456,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27337,1035,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27338,1057,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27339,1058,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27340,1059,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27341,1060,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27342,1061,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27343,1355,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27344,1037,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27345,1068,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27346,1069,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27347,1070,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27348,1071,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27349,1352,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27350,1353,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27351,1362,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27352,1363,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27353,1386,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27354,1399,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27355,1428,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27356,1429,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27357,1469,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27358,1440,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27359,1461,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27360,1462,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27361,1463,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27362,1464,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27363,1465,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27364,1466,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27365,1468,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27366,1470,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27367,1471,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27368,1149,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27369,1231,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27370,1232,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27371,1233,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27372,1234,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27373,1235,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27374,1236,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27375,1321,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27376,1324,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27377,1150,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27378,1237,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27379,1238,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27380,1239,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27381,1240,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27382,1241,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27383,1242,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27384,1327,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27385,1154,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27386,1261,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27387,1262,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27388,1263,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27389,1264,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27390,1265,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27391,1266,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27392,1325,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27393,1155,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27394,1267,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27395,1268,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27396,1269,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27397,1270,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27398,1271,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27399,1272,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27400,1287,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27401,1288,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27402,1289,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27403,1290,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27404,1291,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27405,1338,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27406,1366,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27407,1156,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27408,1273,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27409,1274,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27410,1275,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27411,1276,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27412,1277,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27413,1278,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27414,1337,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27415,1430,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27416,1436,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27417,1442,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27418,1443,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27419,1444,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27420,1445,14,1,1,'2023-11-27 10:05:55');
INSERT INTO `sys_menu_auth` VALUES (27421,1446,14,1,1,'2023-11-27 10:05:55');
/*!40000 ALTER TABLE `sys_menu_auth` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_position`
--

DROP TABLE IF EXISTS `sys_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_position` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `position_name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '职位名称',
  `remark` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '备注',
  `sort` int(11) DEFAULT NULL COMMENT '排序',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='岗位';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_position`
--

LOCK TABLES `sys_position` WRITE;
/*!40000 ALTER TABLE `sys_position` DISABLE KEYS */;
INSERT INTO `sys_position` VALUES (1,'string','string',0,0,0,NULL,'2022-11-08 13:56:35','2023-11-20 16:34:01',1);
INSERT INTO `sys_position` VALUES (2,'项目经理',NULL,2,0,1,NULL,'2022-11-08 13:56:56','2023-06-28 15:24:39',1);
INSERT INTO `sys_position` VALUES (3,'人力资源',NULL,4,0,1,NULL,'2022-11-08 13:57:24','2023-06-28 15:25:08',1);
INSERT INTO `sys_position` VALUES (4,'普通员工','111',5,0,1,NULL,'2022-11-08 13:57:48','2023-06-28 15:25:04',1);
INSERT INTO `sys_position` VALUES (5,'CEO','CEO',8,0,1,1,'2023-08-29 16:51:58','2023-10-07 09:26:17',1);
/*!40000 ALTER TABLE `sys_position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_role`
--

DROP TABLE IF EXISTS `sys_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `role_name` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '角色名',
  `sort` int(11) DEFAULT NULL COMMENT '排序',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='角色';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role`
--

LOCK TABLES `sys_role` WRITE;
/*!40000 ALTER TABLE `sys_role` DISABLE KEYS */;
INSERT INTO `sys_role` VALUES (1,'超级管理员',1,0,1,0,'2022-11-08 11:26:24',NULL,NULL);
INSERT INTO `sys_role` VALUES (2,'车间管理',31,0,1,0,'2022-11-17 08:58:18','2023-09-22 15:50:02',1);
INSERT INTO `sys_role` VALUES (3,'生产报工',4,0,1,1,'2023-02-23 02:01:07','2023-09-22 15:51:08',1);
INSERT INTO `sys_role` VALUES (6,'质检组长',4,0,1,33,'2023-04-18 02:02:51','2023-09-22 15:51:12',1);
INSERT INTO `sys_role` VALUES (8,'质检员',8,0,1,33,'2023-04-18 02:02:57','2023-09-22 15:51:23',1);
INSERT INTO `sys_role` VALUES (12,'test',12,0,1,1,'2023-08-07 08:59:59','2023-11-21 16:09:18',1);
INSERT INTO `sys_role` VALUES (14,'jingwang',1,0,1,1,'2023-09-27 10:04:23','2023-11-27 10:05:55',1);
/*!40000 ALTER TABLE `sys_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_user`
--

DROP TABLE IF EXISTS `sys_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_name` varchar(20) CHARACTER SET utf8 NOT NULL COMMENT '登录名',
  `password` varchar(150) CHARACTER SET utf8 NOT NULL COMMENT '密码',
  `salt` varchar(150) CHARACTER SET utf8 NOT NULL COMMENT '密码加盐',
  `real_name` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '真实姓名',
  `department_id` int(11) DEFAULT NULL COMMENT '部门',
  `position_id` int(11) DEFAULT NULL COMMENT '岗位',
  `sex` int(11) DEFAULT NULL COMMENT '性别(1:男 0:女)',
  `birthday` varchar(10) CHARACTER SET utf8 DEFAULT NULL COMMENT '生日',
  `portrait` varchar(150) CHARACTER SET utf8 DEFAULT NULL COMMENT '头像',
  `mobile` varchar(11) CHARACTER SET utf8 DEFAULT NULL COMMENT '手机',
  `email` varchar(25) CHARACTER SET utf8 DEFAULT NULL COMMENT '邮箱 ',
  `qq` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT 'QQ',
  `we_chat` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '微信',
  `is_admin` tinyint(4) DEFAULT '0' COMMENT '是否超级管理员(1:是 0:否)',
  `remark` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '备注',
  `sort` int(11) DEFAULT NULL COMMENT '排序',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=60 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='用户';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (1,'admin','72bd6dc3db1ec83f0111c54a0db0453f','c5d97b968d1b4f269f6f46db9e7b0997','超级管理员',1,NULL,1,NULL,NULL,'13400000002','13400000002@qq.com',NULL,NULL,1,NULL,NULL,0,1,NULL,'2022-09-27 16:07:39','2023-08-03 08:32:24',1);
INSERT INTO `sys_user` VALUES (32,'tony','9d60e44e87dedc09476161d7764a5381','9108f7b5cecd49a78d5fecff21d997fb','tony',13,2,1,NULL,NULL,'18900002222','13400000001@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:41:31','2023-08-07 08:46:50',1);
INSERT INTO `sys_user` VALUES (57,'jingwang','ec008e2cc6ca51f88500db7ae8eb65cb','ed1b0c6e2eb1424a9adebe9268f9386e','jingwang',1,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,1,'2023-09-27 10:04:51',NULL,NULL);
INSERT INTO `sys_user` VALUES (59,'vega','bb6b66873616cd8b54d089aac406f694','409b25067ee44e70b70d7c40194af2ac','vega',1,2,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,1,'2023-11-24 19:23:01',NULL,NULL);
/*!40000 ALTER TABLE `sys_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_user_role`
--

DROP TABLE IF EXISTS `sys_user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_user_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT '用户ID',
  `role_id` int(11) NOT NULL COMMENT '角色ID',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='用户对应角色';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (4,1,1,1,'2022-12-26 17:17:36');
INSERT INTO `sys_user_role` VALUES (12,32,1,1,'2023-04-05 22:41:31');
INSERT INTO `sys_user_role` VALUES (37,57,14,1,'2023-09-27 10:04:52');
INSERT INTO `sys_user_role` VALUES (39,59,14,1,'2023-11-24 19:23:01');
/*!40000 ALTER TABLE `sys_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_alarm`
--

DROP TABLE IF EXISTS `t_alarm`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_alarm` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `alarm_time` datetime DEFAULT NULL COMMENT '告警时间',
  `alarm_code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '告警编号',
  `alarm_name` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '告警名称',
  `alarm_level` tinyint(4) DEFAULT NULL COMMENT '告警级别（0通知、1告警、2紧急、3严重）',
  `device_id` int(11) DEFAULT NULL COMMENT '告警设备ID',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `event_id` int(11) NOT NULL COMMENT '事件Id',
  `event_data` varchar(2000) CHARACTER SET utf8 DEFAULT NULL COMMENT '事件数据',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `alarm_code_UNIQUE` (`alarm_code`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm`
--

LOCK TABLES `t_alarm` WRITE;
/*!40000 ALTER TABLE `t_alarm` DISABLE KEYS */;
INSERT INTO `t_alarm` VALUES (42,'2023-04-19 13:19:40','1','钻机扫条码',2,11,0,1,1,'2023-04-19 01:19:53',NULL,NULL,31,'DRILL_SCAN_CODE_EVENT');
INSERT INTO `t_alarm` VALUES (43,'2023-04-20 16:58:35','2','AGV移动',1,13,0,1,1,'2023-04-20 04:58:48',NULL,NULL,0,'');
INSERT INTO `t_alarm` VALUES (44,'2023-04-27 16:50:35','3','钻机请求配方',1,11,0,1,1,'2023-04-27 16:50:35',NULL,NULL,26,'DRILL_REQUEST_RECIPE');
INSERT INTO `t_alarm` VALUES (45,'2023-04-27 17:18:35','4','钻机开门',3,11,0,1,1,'2023-04-27 17:18:35',NULL,NULL,0,NULL);
INSERT INTO `t_alarm` VALUES (46,'2023-04-28 10:21:35','5','钻机开始加载文件',1,11,0,1,1,'2023-04-28 10:21:35',NULL,NULL,0,NULL);
INSERT INTO `t_alarm` VALUES (47,'2023-04-28 13:05:35','6','AGV充电',3,13,0,1,1,'2023-04-28 13:05:35',NULL,NULL,0,NULL);
INSERT INTO `t_alarm` VALUES (48,'2023-04-28 15:25:35','7','AGV状态上报',3,13,0,1,1,'2023-04-28 15:25:35',NULL,NULL,0,NULL);
INSERT INTO `t_alarm` VALUES (49,'2023-04-28 18:00:35','8','AGV上料仓',1,13,0,1,1,'2023-04-28 18:00:35',NULL,NULL,0,NULL);
INSERT INTO `t_alarm` VALUES (50,'2023-04-28 18:22:35','9','拆板机发生异常',1,NULL,0,1,1,'2023-04-28 18:22:35','2023-10-12 09:39:06',1,0,NULL);
/*!40000 ALTER TABLE `t_alarm` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_alarm_setting`
--

DROP TABLE IF EXISTS `t_alarm_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_alarm_setting` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` bigint(20) NOT NULL DEFAULT '0',
  `code` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `alarm_desc` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '描述',
  `alarm_level` tinyint(4) DEFAULT NULL COMMENT '告警级别（0通知、1告警、2紧急、3严重）',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `event_id` int(11) NOT NULL COMMENT '事件id',
  `event_name` varchar(100) DEFAULT NULL COMMENT '事件名称',
  `notify_way_ids` varchar(1000) CHARACTER SET utf8 DEFAULT NULL COMMENT '通知方式',
  `notify_way_names` varchar(2000) DEFAULT NULL COMMENT '通知方式名称',
  `event_rules` varchar(2000) CHARACTER SET utf8 DEFAULT NULL COMMENT '触发规则',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm_setting`
--

LOCK TABLES `t_alarm_setting` WRITE;
/*!40000 ALTER TABLE `t_alarm_setting` DISABLE KEYS */;
INSERT INTO `t_alarm_setting` VALUES (12,'AGV电量水平低于30%',0,'LOWER_THAN_30','AGV电量水平低于30%',1,0,0,1,'2023-02-27 16:26:42','2023-07-04 09:48:56',1,57,'DRILL_REQUEST_LOAD_RAW_MATERIAL','11','大屏告警','AGV电量水平低于30%','0');
INSERT INTO `t_alarm_setting` VALUES (14,'AGV电量水平低于20%',0,'LOWER_THAN_20','AGV电量水平低于20%',2,0,1,1,'2023-02-27 16:26:42','2023-07-04 09:48:22',1,61,'DRILL_REQUEST_LOAD_RAW_MATERIAL','12','短信',' AGV电量水平低于20%','0');
INSERT INTO `t_alarm_setting` VALUES (15,'AGV电量水平低于10%',0,'LOWER_THAN_10','AGV电量水平低于10%',3,0,1,1,'2023-02-27 16:26:42','2023-07-21 11:06:18',1,38,'AGV充电','11','大屏告警','AGV电量水平低于10%','0');
/*!40000 ALTER TABLE `t_alarm_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_check_records`
--

DROP TABLE IF EXISTS `t_check_records`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_check_records` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `task_id` bigint(20) NOT NULL COMMENT '生产任务ID',
  `task_code` varchar(50) DEFAULT NULL COMMENT '生产任务编号',
  `task_name` varchar(50) DEFAULT NULL COMMENT '生产任务名称',
  `work_order_id` bigint(20) NOT NULL COMMENT '生产工单ID',
  `work_order_code` varchar(50) DEFAULT NULL COMMENT '生产工单编号',
  `work_order_name` varchar(50) DEFAULT NULL COMMENT '生产工单名称',
  `user_id` bigint(20) DEFAULT NULL COMMENT '检验人员ID',
  `user_name` varchar(50) DEFAULT NULL COMMENT '检验人员姓名',
  `is_check_ok` char(2) DEFAULT '-1' COMMENT '是否检验合格(-1/0/1)',
  `check_time` datetime DEFAULT NULL COMMENT '检验日期',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='检验记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_check_records`
--

LOCK TABLES `t_check_records` WRITE;
/*!40000 ALTER TABLE `t_check_records` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_check_records` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_client`
--

DROP TABLE IF EXISTS `t_client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_client` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(64) NOT NULL COMMENT '客户编码',
  `name` varchar(255) NOT NULL COMMENT '客户简称',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COMMENT='客户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_client`
--

LOCK TABLES `t_client` WRITE;
/*!40000 ALTER TABLE `t_client` DISABLE KEYS */;
INSERT INTO `t_client` VALUES (56,'ceshicustom001','测试客户001','测试',0,1,1,'2023-08-28 17:25:05','2023-09-12 13:34:11',1);
INSERT INTO `t_client` VALUES (61,'15200000001','张老板','XX公司000',0,1,1,'2023-10-30 14:51:20','2023-11-13 11:03:07',1);
INSERT INTO `t_client` VALUES (62,'KH11000001','KH11000001','KH11000001',0,1,1,'2023-11-03 10:21:12',NULL,NULL);
/*!40000 ALTER TABLE `t_client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter`
--

DROP TABLE IF EXISTS `t_cutter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '名称',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `max_stock` int(11) DEFAULT '0' COMMENT '最高库存',
  `current_stock` int(11) DEFAULT '0' COMMENT '现有库存',
  `min_stock` int(11) DEFAULT '0' COMMENT '最低库存',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `life` int(11) DEFAULT NULL COMMENT '刀片寿命',
  `estimated_pieces` int(11) DEFAULT NULL COMMENT '预计加工件数',
  `inbound_date` datetime NOT NULL COMMENT '入库日期',
  `purpose` varchar(100) CHARACTER SET utf8 DEFAULT NULL COMMENT '用途位置',
  `cutter_status` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '刀具状态',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='刀具';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter`
--

LOCK TABLES `t_cutter` WRITE;
/*!40000 ALTER TABLE `t_cutter` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_cutter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter_config_detail`
--

DROP TABLE IF EXISTS `t_cutter_config_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter_config_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `master_id` bigint(20) NOT NULL DEFAULT '0',
  `d` decimal(10,3) DEFAULT NULL COMMENT 'D直径MM',
  `s` decimal(10,3) DEFAULT NULL COMMENT 'S转速KRPM',
  `f` decimal(10,3) DEFAULT NULL COMMENT 'F(进刀速)M/MIN',
  `r` decimal(10,3) DEFAULT NULL COMMENT 'R(退刀速)M/MIN',
  `z` decimal(10,3) DEFAULT NULL COMMENT 'Z(深度补偿)MM',
  `age` int(11) DEFAULT NULL COMMENT '寿命',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_detail`
--

LOCK TABLES `t_cutter_config_detail` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_cutter_config_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter_config_master`
--

DROP TABLE IF EXISTS `t_cutter_config_master`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter_config_master` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `config_name` varchar(100) DEFAULT NULL COMMENT '配置名称',
  `config_desc` varchar(200) DEFAULT NULL COMMENT '配置描述',
  `dia_file_name` varchar(100) DEFAULT NULL COMMENT 'dia文件名',
  `dia_file_path` varchar(500) DEFAULT NULL COMMENT 'dia文件路径',
  `product_category_id` bigint(20) DEFAULT NULL COMMENT '产品大类ID',
  `product_category_code` varchar(255) DEFAULT NULL COMMENT '产品大类编码',
  `product_category_name` varchar(255) DEFAULT NULL COMMENT '产品大类名称',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数主表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_master`
--

LOCK TABLES `t_cutter_config_master` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_master` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_cutter_config_master` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter_plan`
--

DROP TABLE IF EXISTS `t_cutter_plan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter_plan` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `disk_code` varchar(100) NOT NULL COMMENT '刀盘二维码',
  `atp` varchar(500) DEFAULT NULL COMMENT '刀盘参数文件路径',
  `item_code` varchar(100) DEFAULT NULL COMMENT '料号',
  `change_rule` tinyint(4) DEFAULT NULL COMMENT '换刀规则1按料号，2按加工板次，3按使用寿命',
  `rule_board_limit` int(11) DEFAULT NULL COMMENT '加工板次',
  `rule_age_limit` int(11) DEFAULT NULL COMMENT '使用寿命',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻机排刀计划';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_plan`
--

LOCK TABLES `t_cutter_plan` WRITE;
/*!40000 ALTER TABLE `t_cutter_plan` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_cutter_plan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device`
--

DROP TABLE IF EXISTS `t_device`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `name` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `tag` varchar(1000) CHARACTER SET utf8 DEFAULT NULL COMMENT '设备标签, 类似a=b键值对',
  `parent_id` bigint(20) NOT NULL DEFAULT '0',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `device_type_id` int(11) DEFAULT NULL COMMENT '设备类型ID',
  `device_vendor_id` int(11) DEFAULT NULL COMMENT '供应商',
  `device_brand` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '品牌',
  `device_spec` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `work_station_id` int(11) DEFAULT NULL COMMENT '工作站、工位',
  `maintain_period_days` int(11) DEFAULT NULL COMMENT '维护周期天',
  `production_time` datetime DEFAULT NULL COMMENT '设备投产日期',
  `last_maintain_time` date DEFAULT NULL COMMENT '下次设备维护时间',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `parameters` varchar(5000) CHARACTER SET utf8 DEFAULT NULL COMMENT '设备参数',
  `device_status` varchar(255) DEFAULT NULL COMMENT '设备状态',
  `device_type_code` varchar(50) DEFAULT NULL COMMENT '设备类型代码',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=147 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
INSERT INTO `t_device` VALUES (13,'agv01','agv01',NULL,0,0,1,1,'2023-02-28 11:40:21','2023-10-31 16:07:54',1,8,0,'string','string',234,3,NULL,NULL,NULL,'1111112\n456123','1','agv');
INSERT INTO `t_device` VALUES (14,'MockAgv02','MockAgv02',NULL,0,0,0,1,'2023-03-03 00:33:58','2023-11-21 16:55:05',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (15,'MockDrill01','MockDrill01',NULL,0,0,1,1,'2023-03-03 00:34:24','2023-09-05 09:50:47',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (16,'P0001','P0001',NULL,0,0,1,1,'2023-03-03 01:32:19','2023-03-08 16:46:09',NULL,14,NULL,NULL,NULL,218,NULL,NULL,NULL,NULL,NULL,'7','pin');
INSERT INTO `t_device` VALUES (17,'MockAgv06','MockAgv06',NULL,0,0,2,1,'2023-03-07 10:20:48','2023-11-16 08:59:18',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (18,'MockAgv03','MockAgv03',NULL,0,0,1,1,'2023-03-07 10:21:29','2023-11-21 16:55:05',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (19,'MockAgv04','MockAgv04',NULL,0,0,1,1,'2023-03-07 17:09:12','2023-11-21 16:55:05',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (20,'P0002','P0002',NULL,0,0,1,1,'2023-03-07 20:19:06','2023-10-16 09:59:21',1,14,NULL,NULL,NULL,218,NULL,NULL,NULL,NULL,NULL,'1','pin');
INSERT INTO `t_device` VALUES (21,'MockAgv104','MockAgv104',NULL,0,0,0,1,'2023-03-09 10:15:32','2023-07-05 14:30:43',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (22,'MockAgv103','MockAgv103',NULL,0,0,1,1,'2023-03-09 10:15:32','2023-10-16 09:59:21',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (23,'MockDrill02','MockDrill02',NULL,0,0,1,1,'2023-03-13 13:54:18','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,212,NULL,NULL,NULL,NULL,NULL,'7','drill');
INSERT INTO `t_device` VALUES (24,'MockDrill03','MockDrill03',NULL,0,0,1,1,'2023-03-14 08:48:10','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (25,'MockAgv01','MockAgv01',NULL,0,0,1,1,'2023-03-14 08:51:18','2023-11-27 10:13:25',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,'{\"A\":\"123\"}','3','agv');
INSERT INTO `t_device` VALUES (26,'MockDrill04','MockDrill04',NULL,0,0,1,1,'2023-03-14 09:22:53','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (27,'MockDrill05','MockDrill05',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (28,'MockDrill06','MockDrill06',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (29,'Processed0001','Processed0001',NULL,0,0,1,1,'2023-03-14 11:19:38','2023-03-16 10:25:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (30,'Processed10001','Processed10001',NULL,0,0,1,1,'2023-03-14 15:50:43','2023-10-16 09:59:21',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (31,'vegaDrill001','vegaDrill001',NULL,0,0,1,1,'2023-03-14 16:18:04','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (32,'MockAgv1003','MockAgv1003',NULL,0,0,1,1,'2023-03-14 16:28:34','2023-10-16 09:59:21',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (33,'vegaDrill0601','vegaDrill0601',NULL,0,0,1,1,'2023-03-14 16:28:35','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (34,'MockAgv11103','MockAgv11103',NULL,0,0,1,1,'2023-03-14 16:52:45','2023-03-15 08:43:45',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (35,'MockTonyAgv001','MockTonyAgv001',NULL,0,0,1,1,'2023-03-15 09:41:29','2023-03-22 23:45:47',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'7','agv');
INSERT INTO `t_device` VALUES (36,'MockSimpleAgv001','MockSimpleAgv001',NULL,0,0,1,1,'2023-03-15 10:12:33','2023-10-16 09:59:21',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (37,'MockSimpleAgv002','MockSimpleAgv002',NULL,0,0,1,1,'2023-03-15 10:24:25','2023-10-16 09:59:21',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1','agv');
INSERT INTO `t_device` VALUES (38,'Processed002','Processed002',NULL,0,0,1,1,'2023-03-15 10:24:26','2023-10-16 09:59:21',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (39,'Raw10001','Raw10001',NULL,0,0,1,1,'2023-03-15 14:07:38','2023-03-15 14:28:20',NULL,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','RawStagingDesk');
INSERT INTO `t_device` VALUES (40,'Processed100012','Processed100012',NULL,0,0,1,1,'2023-03-15 14:32:46','2023-10-16 09:59:21',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (41,'Raw100012','Raw100012',NULL,0,0,1,1,'2023-03-15 14:32:47','2023-10-16 09:59:21',1,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','RawStagingDesk');
INSERT INTO `t_device` VALUES (42,'Processed1000122','Processed1000122',NULL,0,0,1,1,'2023-03-15 16:21:35','2023-06-21 15:22:37',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (43,'Raw1000123','Raw1000123',NULL,0,0,1,1,'2023-03-15 16:21:36','2023-10-16 09:59:21',1,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','RawStagingDesk');
INSERT INTO `t_device` VALUES (45,'Processed10002','Processed10002',NULL,0,0,1,1,'2023-03-20 14:16:50','2023-10-16 09:59:21',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (46,'SiloShelf100012','SiloShelf100012',NULL,0,0,1,1,'2023-03-21 07:01:46','2023-10-16 09:59:21',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1','ProcessedStagingDesk');
INSERT INTO `t_device` VALUES (49,'UP000102','UP000102',NULL,0,0,1,1,'2023-03-22 07:51:36','2023-10-16 09:59:21',1,15,NULL,NULL,NULL,219,NULL,NULL,NULL,NULL,NULL,'1','unpin');
INSERT INTO `t_device` VALUES (51,'drill01-0001','钻机01-0001',NULL,0,0,1,1,'2023-03-22 23:02:07','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,212,1,'2023-06-13 11:12:26','2023-06-15',NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (52,'drill01-0002','钻机01-0002',NULL,0,0,1,1,'2023-03-22 23:16:16','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,217,2,'2023-06-13 11:12:30','2023-06-15',NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (54,'drill01-0003','钻机01-0003',NULL,0,0,1,1,'2023-03-23 01:19:33','2023-10-16 09:59:21',1,7,NULL,NULL,NULL,233,3,'2023-06-13 11:12:33','2023-06-15',NULL,NULL,'1','drill');
INSERT INTO `t_device` VALUES (60,'MockAGV02jinlu','MockAGV02jinlu',NULL,0,0,1,1,'2023-07-25 16:03:34','2023-09-05 15:14:51',NULL,8,NULL,NULL,NULL,245,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://192.168.104.102:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"ModbusTcpSlaveId\":\"1\",\"ModbusTcpUri\":\"http://192.168.3.20:502\",\"MoveTimeout\":\"300\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://192.168.104.102:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"2\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGV02jinlu\",\"DeviceName\":\"MockAGV02jinlu\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockAGV\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":1,\"InputCapabilities\":[1,301,400],\"OutputCapabilities\":[302,1]}','1','agv');
INSERT INTO `t_device` VALUES (61,'MockDrill0508jinlu','MockDrill0508jinlu',NULL,0,0,1,1,'2023-07-25 16:03:34','2023-09-05 16:27:27',NULL,7,NULL,NULL,NULL,246,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jinlu\",\"DeviceName\":\"MockDrill0508jinlu\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":0,\"InputCapabilities\":[302],\"OutputCapabilities\":[400]}','1','drill');
INSERT INTO `t_device` VALUES (62,'MockDrill0508jinlu-3','MockDrill0508jinlu',NULL,0,0,1,1,'2023-08-24 13:39:06','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,249,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jinlu-3\",\"DeviceName\":\"MockDrill0508jinlu\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":0,\"InputCapabilities\":[301],\"OutputCapabilities\":[400]}','1','drill');
INSERT INTO `t_device` VALUES (63,'MockAGV02-jingwang-backup','MockAGV02-jingwang-backup',NULL,0,0,1,1,'2023-08-24 13:39:06','2023-09-04 09:40:48',NULL,8,NULL,NULL,NULL,250,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://192.168.104.102:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"ModbusTcpSlaveId\":\"1\",\"ModbusTcpUri\":\"http://192.168.3.20:502\",\"MoveTimeout\":\"300\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://192.168.104.102:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"2\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGV02-jingwang-backup\",\"DeviceName\":\"MockAGV02-jingwang-backup\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockAGV\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":1,\"InputCapabilities\":[1,400],\"OutputCapabilities\":[301,1]}','1','agv');
INSERT INTO `t_device` VALUES (64,'MockDrill0508jinlu-1','MockDrill0508jinlu',NULL,0,0,1,1,'2023-08-24 16:01:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,251,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jinlu-1\",\"DeviceName\":\"MockDrill0508jinlu\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":0,\"InputCapabilities\":[301],\"OutputCapabilities\":[400]}','1','drill');
INSERT INTO `t_device` VALUES (65,'MockDrill0508jinlu-2','MockDrill0508jinlu',NULL,0,0,1,1,'2023-08-24 16:25:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,252,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jinlu-2\",\"DeviceName\":\"MockDrill0508jinlu\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":0,\"InputCapabilities\":[301],\"OutputCapabilities\":[400]}','1','drill');
INSERT INTO `t_device` VALUES (66,'D5-2849-242','Vega-Auto-D5-2849',NULL,0,0,1,1,'2023-08-25 10:01:33','2023-11-27 10:13:40',1,7,NULL,NULL,NULL,253,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"3225.atp\",\"BoardLengthInfOnPlc\":\"1000\",\"BoardMaxLength\":\"625\",\"BufferAgvArrivalEnd\":\"509\",\"BufferAgvPositionEnd\":\"412\",\"BufferAgvUnLoadingAndLoading\":\"508\",\"BufferAllUnloadAndLoadEnd\":\"513\",\"BufferClinkerlLayerLoadEnd\":\"3000\",\"BufferOnAgvPositionFlag\":\"0\",\"BufferOnWorking\":\"100\",\"BufferRawMaterialLayerLoadEnd\":\"2000\",\"BufferReadyFlag\":\"410\",\"BufferReceiveBoardFlag\":\"411\",\"BufferStartLoadingBoard\":\"510\",\"BufferStartUnLoadingBoard\":\"511\",\"BufferStatusFlag\":\"401\",\"BufferUnloadAndLoadSpline\":\"514\",\"CallAgvTimeInterval\":\"20\",\"CanUnloadAndLoadFlagWritePlc\":\"181\",\"Cnc84BufferLockOnCnc84\":\"75\",\"Cnc84DiaFileOnOff\":\"True\",\"CNC84Ip\":\"127.0.0.1\",\"CNC84Port\":\"33822\",\"Cnc84WorkEndWritePlc\":\"23\",\"CodeReaderNum\":\"6\",\"CodeReaderOnOff\":\"False\",\"DefalutBoardLength\":\"800\",\"DiaPath\":\"3225.dia\",\"DiaSearchFileExt\":\"dia\",\"DiaSearchPath\":\"f:\\\\09_test\\\\\",\"DrilBoardEndFlag\":\"58\",\"DrillBoardOnOff\":\"True\",\"DrlPath\":\"3225.drl\",\"DrlSearchFileExt\":\"drl\",\"DrlSearchPath\":\"f:\\\\09_test\\\\\",\"ExistBoardOnDrill\":\"116\",\"ExistBoardOnPlc\":\"144\",\"FrontMushroomCloseLocalTionFlag\":\"64\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"InteractivePosition\":\"RIGHT\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"127.0.0.1\",\"IsAutoCallAgv\":\"False\",\"MiddleMushroomCloseLocalTionFlag\":\"55\",\"MiddleMushroomExist\":\"True\",\"MushroomOnOff\":\"True\",\"P4FrontPostion\":\"85\",\"P4RearPostion\":\"86\",\"Port\":\"502\",\"PressBoardEndFlagOnCNC84\":\"68\",\"PressBoardOnOff\":\"True\",\"RetractToolEndTimeout\":\"5000\",\"ScanCom\":\"Com4\",\"ScanGunInfoNumOnPlc\":\"209\",\"ScanGunInfoStartPositionOnPlc\":\"210\",\"ScanGunOnOff\":\"False\",\"SelectSplineMFunction\":\"107\",\"SelectSplineOnOff\":\"True\",\"SlaveID\":\"1\",\"SmallBoardMaxLength\":\"740\",\"SmallBoardUserFlag\":\"59\",\"Spindles\":\"00001,00002,00003,00004,00005\",\"Spline1CodeReaderCodeNum\":\"250\",\"Spline1CodeReaderCodeStartPosition\":\"251\",\"Spline1CodeReaderIP\":\"192.168.3.111\",\"Spline1CodeReaderPort\":\"2001\",\"Spline2CodeReaderCodeNum\":\"260\",\"Spline2CodeReaderCodeStartPosition\":\"261\",\"Spline2CodeReaderIP\":\"192.168.3.111\",\"Spline2CodeReaderPort\":\"2001\",\"Spline3CodeReaderCodeNum\":\"270\",\"Spline3CodeReaderCodeStartPosition\":\"271\",\"Spline3CodeReaderIP\":\"192.168.3.111\",\"Spline3CodeReaderPort\":\"2001\",\"Spline4CodeReaderCodeNum\":\"280\",\"Spline4CodeReaderCodeStartPosition\":\"281\",\"Spline4CodeReaderIP\":\"192.168.3.111\",\"Spline4CodeReaderPort\":\"2001\",\"Spline5CodeReaderCodeNum\":\"290\",\"Spline5CodeReaderCodeStartPosition\":\"291\",\"Spline5CodeReaderIP\":\"192.168.3.111\",\"Spline5CodeReaderPort\":\"2001\",\"Spline6CodeReaderCodeNum\":\"300\",\"Spline6CodeReaderCodeStartPosition\":\"301\",\"Spline6CodeReaderIP\":\"192.168.3.111\",\"Spline6CodeReaderPort\":\"2001\",\"UnloadAndLoadEndOnPlc\":\"21\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidateEscCommandTimeOut\":\"120000\",\"ValidateMushroomEndTimeOut\":\"90000\",\"ValidatePressBoardEndTimeout\":\"90000\",\"WarningInfoWriteOnPlc\":\"109\"},\"ProductId\":\"drill\",\"DeviceId\":\"D5-2849-242\",\"DeviceName\":\"Vega-Auto-D5-2849\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Fundation.Vendor.Vega.RearDrill2849\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":5,\"SiloLimit\":1,\"PanelLimit\":5,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":0,\"InputCapabilities\":[301],\"OutputCapabilities\":[400]}','0','drill');
INSERT INTO `t_device` VALUES (67,'D5-2849-243','Vega-Auto-D5-2849',NULL,0,0,1,1,'2023-08-25 10:02:42','2023-11-27 10:13:40',1,7,NULL,NULL,NULL,254,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"3225.atp\",\"BoardLengthInfOnPlc\":\"1000\",\"BoardMaxLength\":\"625\",\"BufferAgvArrivalEnd\":\"509\",\"BufferAgvPositionEnd\":\"412\",\"BufferAgvUnLoadingAndLoading\":\"508\",\"BufferAllUnloadAndLoadEnd\":\"513\",\"BufferClinkerlLayerLoadEnd\":\"3000\",\"BufferOnAgvPositionFlag\":\"0\",\"BufferOnWorking\":\"100\",\"BufferRawMaterialLayerLoadEnd\":\"2000\",\"BufferReadyFlag\":\"410\",\"BufferReceiveBoardFlag\":\"411\",\"BufferStartLoadingBoard\":\"510\",\"BufferStartUnLoadingBoard\":\"511\",\"BufferStatusFlag\":\"401\",\"BufferUnloadAndLoadSpline\":\"514\",\"CallAgvTimeInterval\":\"20\",\"CanUnloadAndLoadFlagWritePlc\":\"181\",\"Cnc84BufferLockOnCnc84\":\"75\",\"Cnc84DiaFileOnOff\":\"True\",\"CNC84Ip\":\"127.0.0.1\",\"CNC84Port\":\"33822\",\"Cnc84WorkEndWritePlc\":\"23\",\"CodeReaderNum\":\"6\",\"CodeReaderOnOff\":\"False\",\"DefalutBoardLength\":\"800\",\"DiaPath\":\"3225.dia\",\"DiaSearchFileExt\":\"dia\",\"DiaSearchPath\":\"f:\\\\09_test\\\\\",\"DrilBoardEndFlag\":\"58\",\"DrillBoardOnOff\":\"True\",\"DrlPath\":\"3225.drl\",\"DrlSearchFileExt\":\"drl\",\"DrlSearchPath\":\"f:\\\\09_test\\\\\",\"ExistBoardOnDrill\":\"116\",\"ExistBoardOnPlc\":\"144\",\"FrontMushroomCloseLocalTionFlag\":\"64\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"InteractivePosition\":\"RIGHT\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"127.0.0.1\",\"IsAutoCallAgv\":\"False\",\"MiddleMushroomCloseLocalTionFlag\":\"55\",\"MiddleMushroomExist\":\"True\",\"MushroomOnOff\":\"True\",\"P4FrontPostion\":\"85\",\"P4RearPostion\":\"86\",\"Port\":\"502\",\"PressBoardEndFlagOnCNC84\":\"68\",\"PressBoardOnOff\":\"True\",\"RetractToolEndTimeout\":\"5000\",\"ScanCom\":\"Com4\",\"ScanGunInfoNumOnPlc\":\"209\",\"ScanGunInfoStartPositionOnPlc\":\"210\",\"ScanGunOnOff\":\"False\",\"SelectSplineMFunction\":\"107\",\"SelectSplineOnOff\":\"True\",\"SlaveID\":\"1\",\"SmallBoardMaxLength\":\"740\",\"SmallBoardUserFlag\":\"59\",\"Spindles\":\"00001,00002,00003,00004,00005\",\"Spline1CodeReaderCodeNum\":\"250\",\"Spline1CodeReaderCodeStartPosition\":\"251\",\"Spline1CodeReaderIP\":\"192.168.3.111\",\"Spline1CodeReaderPort\":\"2001\",\"Spline2CodeReaderCodeNum\":\"260\",\"Spline2CodeReaderCodeStartPosition\":\"261\",\"Spline2CodeReaderIP\":\"192.168.3.111\",\"Spline2CodeReaderPort\":\"2001\",\"Spline3CodeReaderCodeNum\":\"270\",\"Spline3CodeReaderCodeStartPosition\":\"271\",\"Spline3CodeReaderIP\":\"192.168.3.111\",\"Spline3CodeReaderPort\":\"2001\",\"Spline4CodeReaderCodeNum\":\"280\",\"Spline4CodeReaderCodeStartPosition\":\"281\",\"Spline4CodeReaderIP\":\"192.168.3.111\",\"Spline4CodeReaderPort\":\"2001\",\"Spline5CodeReaderCodeNum\":\"290\",\"Spline5CodeReaderCodeStartPosition\":\"291\",\"Spline5CodeReaderIP\":\"192.168.3.111\",\"Spline5CodeReaderPort\":\"2001\",\"Spline6CodeReaderCodeNum\":\"300\",\"Spline6CodeReaderCodeStartPosition\":\"301\",\"Spline6CodeReaderIP\":\"192.168.3.111\",\"Spline6CodeReaderPort\":\"2001\",\"UnloadAndLoadEndOnPlc\":\"21\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidateEscCommandTimeOut\":\"120000\",\"ValidateMushroomEndTimeOut\":\"90000\",\"ValidatePressBoardEndTimeout\":\"90000\",\"WarningInfoWriteOnPlc\":\"109\"},\"ProductId\":\"drill\",\"DeviceId\":\"D5-2849-243\",\"DeviceName\":\"Vega-Auto-D5-2849\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Fundation.Vendor.Vega.RearDrill2849\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":5,\"SiloLimit\":1,\"PanelLimit\":5,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":0,\"InputCapabilities\":[301],\"OutputCapabilities\":[400]}','0','drill');
INSERT INTO `t_device` VALUES (68,'MockAGVExample01','MockAGVExample01',NULL,0,0,1,1,'2023-08-31 13:12:57','2023-09-26 16:23:07',NULL,8,NULL,NULL,NULL,258,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://192.168.104.102:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"ModbusTcpSlaveId\":\"1\",\"ModbusTcpUri\":\"http://192.168.3.20:502\",\"MoveTimeout\":\"300\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://192.168.104.102:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"1\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGVExample01\",\"DeviceName\":\"MockAGVExample01\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.AgentExample.Devices.Mockers.MockAGVExample\",\"DataCollectingPerSeconds\":30,\"KeepingPlcConnectionPerSeconds\":30,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":1,\"InputCapabilities\":[1,201,300],\"OutputCapabilities\":[300,301,1]}','1','agv');
INSERT INTO `t_device` VALUES (69,'D5-2849-241','Vega-Auto-D5-2849',NULL,0,0,1,1,'2023-09-18 11:39:50','2023-11-27 10:13:40',1,7,NULL,NULL,NULL,259,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"AtpPath\":\"3225.atp\",\"BoardLengthInfOnPlc\":\"1000\",\"BoardMaxLength\":\"625\",\"BufferAgvArrivalEnd\":\"509\",\"BufferAgvPositionEnd\":\"412\",\"BufferAgvUnLoadingAndLoading\":\"508\",\"BufferAllUnloadAndLoadEnd\":\"513\",\"BufferClinkerlLayerLoadEnd\":\"3000\",\"BufferLoadToDrillFlag\":\"406\",\"BufferOnAgvPositionFlag\":\"0\",\"BufferOnWorking\":\"100\",\"BufferRawMaterialLayerLoadEnd\":\"2000\",\"BufferReadyFlag\":\"410\",\"BufferReceiveBoardFlag\":\"411\",\"BufferStartLoadingBoard\":\"510\",\"BufferStartUnLoadingBoard\":\"511\",\"BufferStatusFlag\":\"401\",\"BufferUnloadAndLoadSpline\":\"514\",\"CallAgvTimeInterval\":\"20\",\"CanUnloadAndLoadFlagWritePlc\":\"181\",\"Cnc84BufferLockOnCnc84\":\"75\",\"Cnc84DiaFileOnOff\":\"True\",\"CNC84Ip\":\"127.0.0.1\",\"CNC84Port\":\"33822\",\"Cnc84WorkEndWritePlc\":\"23\",\"CodeReaderNum\":\"6\",\"CodeReaderOnOff\":\"False\",\"DefalutBoardLength\":\"800\",\"DiaPath\":\"3225.dia\",\"DiaSearchFileExt\":\"dia\",\"DiaSearchPath\":\"f:\\\\09_test\\\\\",\"DrilBoardEndFlag\":\"58\",\"DrillBoardOnOff\":\"True\",\"DrlPath\":\"3225.drl\",\"DrlSearchFileExt\":\"drl\",\"DrlSearchPath\":\"f:\\\\09_test\\\\\",\"ExistBoardOnDrill\":\"116\",\"ExistBoardOnPlc\":\"144\",\"FrontMushroomCloseLocalTionFlag\":\"64\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"InteractivePosition\":\"RIGHT\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"127.0.0.1\",\"IsAutoCallAgv\":\"False\",\"MiddleMushroomCloseLocalTionFlag\":\"55\",\"MiddleMushroomExist\":\"True\",\"MushroomOnOff\":\"True\",\"P4FrontPostion\":\"85\",\"P4RearPostion\":\"86\",\"Port\":\"502\",\"PressBoardEndFlagOnCNC84\":\"68\",\"PressBoardOnOff\":\"True\",\"RetractToolEndTimeout\":\"5000\",\"ScanCom\":\"Com4\",\"ScanGunInfoNumOnPlc\":\"209\",\"ScanGunInfoStartPositionOnPlc\":\"210\",\"ScanGunOnOff\":\"False\",\"SelectSplineMFunction\":\"107\",\"SelectSplineOnOff\":\"True\",\"SlaveID\":\"1\",\"SmallBoardMaxLength\":\"740\",\"SmallBoardUserFlag\":\"59\",\"Spindles\":\"00001,00002,00003,00004,00005\",\"Spline1CodeReaderCodeNum\":\"250\",\"Spline1CodeReaderCodeStartPosition\":\"251\",\"Spline1CodeReaderIP\":\"192.168.3.111\",\"Spline1CodeReaderPort\":\"2001\",\"Spline2CodeReaderCodeNum\":\"260\",\"Spline2CodeReaderCodeStartPosition\":\"261\",\"Spline2CodeReaderIP\":\"192.168.3.111\",\"Spline2CodeReaderPort\":\"2001\",\"Spline3CodeReaderCodeNum\":\"270\",\"Spline3CodeReaderCodeStartPosition\":\"271\",\"Spline3CodeReaderIP\":\"192.168.3.111\",\"Spline3CodeReaderPort\":\"2001\",\"Spline4CodeReaderCodeNum\":\"280\",\"Spline4CodeReaderCodeStartPosition\":\"281\",\"Spline4CodeReaderIP\":\"192.168.3.111\",\"Spline4CodeReaderPort\":\"2001\",\"Spline5CodeReaderCodeNum\":\"290\",\"Spline5CodeReaderCodeStartPosition\":\"291\",\"Spline5CodeReaderIP\":\"192.168.3.111\",\"Spline5CodeReaderPort\":\"2001\",\"Spline6CodeReaderCodeNum\":\"300\",\"Spline6CodeReaderCodeStartPosition\":\"301\",\"Spline6CodeReaderIP\":\"192.168.3.111\",\"Spline6CodeReaderPort\":\"2001\",\"UnloadAndLoadEndOnPlc\":\"21\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidateEscCommandTimeOut\":\"120000\",\"ValidateMushroomEndTimeOut\":\"90000\",\"ValidatePressBoardEndTimeout\":\"90000\",\"WarningInfoWriteOnPlc\":\"109\"},\"ProductId\":\"drill\",\"DeviceId\":\"D5-2849-241\",\"DeviceName\":\"Vega-Auto-D5-2849\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.JingWangAgent.Devices.RearDrill2849\",\"DeviceDllFilePath\":\"Vega.JingWangAgent.dll\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":5,\"SiloLimit\":1,\"PanelLimit\":5,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','0','drill');
INSERT INTO `t_device` VALUES (70,'SiloShelf01','Vega-Auto-SiloShelf01',NULL,0,0,1,1,'2023-09-19 13:36:46','2023-10-22 14:53:49',NULL,1,NULL,NULL,NULL,260,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"CallAgvTimeInterval\":\"20\",\"DefaultTimeout\":\"120\",\"GetNextPanelNumber\":\"http://192.168.3.251:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"PostAndGetTimeout\":\"120\",\"ShelfAgvPositionList\":\"00312,22222,33333\",\"ShelfCodeList\":\"Shelf001,Shelf002,Shelf003\",\"ShelfInnerPositionList\":\"00313,44444,55555\"},\"ProductId\":\"siloshelf\",\"DeviceId\":\"SiloShelf01\",\"DeviceName\":\"Vega-Auto-SiloShelf01\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.AgentExample.Devices.Mockers.MockSiloShelf\",\"DeviceDllFilePath\":\"Vega.AgentExample.dll\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":3,\"SiloLimit\":1,\"PanelLimit\":54,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":9,\"InputCapabilities\":[20100,40100],\"OutputCapabilities\":[30000,50000]}','1','000');
INSERT INTO `t_device` VALUES (71,'SiloShelf02','Vega-Auto-SiloShelf01',NULL,0,0,1,1,'2023-09-20 09:34:10','2023-10-16 09:59:21',1,1,NULL,NULL,NULL,261,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"CallAgvTimeInterval\":\"20\",\"DefaultTimeout\":\"120\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"PostAndGetTimeout\":\"120\",\"ShelfAgvPositionList\":\"00312,22222,33333\",\"ShelfInnerPositionList\":\"00313,44444,55555\"},\"ProductId\":\"siloshelf\",\"DeviceId\":\"SiloShelf02\",\"DeviceName\":\"Vega-Auto-SiloShelf01\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.AgentExample.Devices.Mockers.MockSiloShelf\",\"DeviceDllFilePath\":\"Vega.AgentExample.dll\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":3,\"SiloLimit\":1,\"PanelLimit\":54,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":9,\"InputCapabilities\":[40100],\"OutputCapabilities\":[30000]}','1','000');
INSERT INTO `t_device` VALUES (72,'SiloShelf03','Vega-Auto-SiloShelf01',NULL,0,0,1,1,'2023-09-21 11:17:33','2023-09-21 13:34:30',NULL,1,NULL,NULL,NULL,262,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"CallAgvTimeInterval\":\"20\",\"DefaultTimeout\":\"120\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"PostAndGetTimeout\":\"120\",\"ShelfAgvPositionList\":\"00312,00313,00314\",\"ShelfInnerPositionList\":\"00322,00323,00324\",\"ShelfNames\":\"Silo01,Silo02,Silo03\"},\"ProductId\":\"siloshelf\",\"DeviceId\":\"SiloShelf03\",\"DeviceName\":\"Vega-Auto-SiloShelf01\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.AgentExample.Devices.Mockers.MockSiloShelf\",\"DeviceDllFilePath\":\"Vega.AgentExample.dll\",\"DataCollectingPerSeconds\":5,\"KeepingPlcConnectionPerSeconds\":5,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":3,\"SiloLimit\":1,\"PanelLimit\":54,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":9,\"InputCapabilities\":[40100],\"OutputCapabilities\":[30000]}','1','000');
INSERT INTO `t_device` VALUES (73,'SiloShelf11','Vega-Auto-SiloShelf01',NULL,0,0,1,1,'2023-09-22 16:23:44','2023-09-27 10:43:15',NULL,1,NULL,NULL,NULL,263,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"CallAgvTimeInterval\":\"20\",\"DefaultTimeout\":\"120\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"PostAndGetTimeout\":\"120\",\"ShelfAgvPositionList\":\"00312,00313,00314\",\"ShelfInnerPositionList\":\"00322,00323,00324\",\"ShelfNames\":\"Silo01\"},\"ProductId\":\"siloshelf\",\"DeviceId\":\"SiloShelf11\",\"DeviceName\":\"Vega-Auto-SiloShelf01\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.AgentExample.Devices.Mockers.MockSiloShelf\",\"DeviceDllFilePath\":\"Vega.AgentExample.dll\",\"DataCollectingPerSeconds\":5,\"KeepingPlcConnectionPerSeconds\":5,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":9,\"InputCapabilities\":[40100],\"OutputCapabilities\":[30000]}','1','000');
INSERT INTO `t_device` VALUES (75,'SiloShelf09','Vega-Auto-SiloShelf01',NULL,0,0,1,1,'2023-09-27 10:44:57','2023-10-09 17:01:39',NULL,1,NULL,NULL,NULL,264,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"CallAgvTimeInterval\":\"20\",\"DefaultTimeout\":\"120\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"PostAndGetTimeout\":\"120\",\"ShelfAgvPositionList\":\"00312,00313,00314\",\"ShelfInnerPositionList\":\"00322,00323,00324\",\"ShelfNames\":\"Silo01\"},\"ProductId\":\"siloshelf\",\"DeviceId\":\"SiloShelf09\",\"DeviceName\":\"Vega-Auto-SiloShelf01\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.AgentExample.Devices.Mockers.MockSiloShelf\",\"DeviceDllFilePath\":\"Vega.AgentExample.dll\",\"DataCollectingPerSeconds\":5,\"KeepingPlcConnectionPerSeconds\":5,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":9,\"InputCapabilities\":[40100],\"OutputCapabilities\":[30000]}','1','000');
INSERT INTO `t_device` VALUES (76,'D10-2849-243','Vega-Auto-D10-2849',NULL,0,0,1,1,'2023-10-08 10:56:35','2023-11-27 10:13:40',1,7,NULL,NULL,NULL,266,NULL,NULL,NULL,NULL,'{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"AtpPath\":\"3225.atp\",\"BoardLengthInfOnPlc\":\"1000\",\"BoardMaxLength\":\"625\",\"BufferAgvArrivalEnd\":\"509\",\"BufferAgvPositionEnd\":\"412\",\"BufferAgvUnLoadingAndLoading\":\"508\",\"BufferAllUnloadAndLoadEnd\":\"513\",\"BufferClinkerlLayerLoadEnd\":\"3000\",\"BufferLoadToDrillFlag\":\"406\",\"BufferOnAgvPositionFlag\":\"0\",\"BufferOnWorking\":\"100\",\"BufferRawMaterialLayerLoadEnd\":\"2000\",\"BufferReadyFlag\":\"410\",\"BufferReceiveBoardFlag\":\"411\",\"BufferStartLoadingBoard\":\"510\",\"BufferStartUnLoadingBoard\":\"511\",\"BufferStatusFlag\":\"401\",\"BufferUnloadAndLoadSpline\":\"514\",\"CallAgvTimeInterval\":\"20\",\"CanUnloadAndLoadFlagWritePlc\":\"181\",\"Cnc84BufferLockOnCnc84\":\"75\",\"Cnc84DiaFileOnOff\":\"True\",\"CNC84Ip\":\"127.0.0.1\",\"CNC84Port\":\"16664\",\"Cnc84WorkEndWritePlc\":\"23\",\"CodeReaderNum\":\"6\",\"CodeReaderOnOff\":\"False\",\"DefalutBoardLength\":\"800\",\"DiaPath\":\"3225.dia\",\"DiaSearchFileExt\":\"dia\",\"DiaSearchPath\":\"f:\\\\09_test\\\\\",\"DrilBoardEndFlag\":\"58\",\"DrillBoardOnOff\":\"True\",\"DrlPath\":\"3225.drl\",\"DrlSearchFileExt\":\"drl\",\"DrlSearchPath\":\"f:\\\\09_test\\\\\",\"ExistBoardOnDrill\":\"116\",\"ExistBoardOnPlc\":\"144\",\"FrontMushroomCloseLocalTionFlag\":\"64\",\"GetNextPanelNumber\":\"192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"InteractivePosition\":\"RIGHT\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"192.168.3.131\",\"IsAutoCallAgv\":\"False\",\"MiddleMushroomCloseLocalTionFlag\":\"55\",\"MiddleMushroomExist\":\"True\",\"MushroomOnOff\":\"True\",\"P4FrontPostion\":\"12313\",\"P4RearPostion\":\"12312\",\"Port\":\"502\",\"PressBoardEndFlagOnCNC84\":\"68\",\"PressBoardOnOff\":\"True\",\"RetractToolEndTimeout\":\"5000\",\"ScanCom\":\"Com4\",\"ScanGunInfoNumOnPlc\":\"209\",\"ScanGunInfoStartPositionOnPlc\":\"210\",\"ScanGunOnOff\":\"False\",\"SelectSplineMFunction\":\"107\",\"SelectSplineOnOff\":\"False\",\"SlaveID\":\"1\",\"SmallBoardMaxLength\":\"400\",\"SmallBoardUserFlag\":\"59\",\"Spindles\":\"00364,00363,00362,00361,00360\",\"Spline1CodeReaderCodeNum\":\"250\",\"Spline1CodeReaderCodeStartPosition\":\"251\",\"Spline1CodeReaderIP\":\"192.168.3.131\",\"Spline1CodeReaderPort\":\"2001\",\"Spline2CodeReaderCodeNum\":\"260\",\"Spline2CodeReaderCodeStartPosition\":\"261\",\"Spline2CodeReaderIP\":\"192.168.3.131\",\"Spline2CodeReaderPort\":\"2001\",\"Spline3CodeReaderCodeNum\":\"270\",\"Spline3CodeReaderCodeStartPosition\":\"271\",\"Spline3CodeReaderIP\":\"192.168.3.131\",\"Spline3CodeReaderPort\":\"2001\",\"Spline4CodeReaderCodeNum\":\"280\",\"Spline4CodeReaderCodeStartPosition\":\"281\",\"Spline4CodeReaderIP\":\"192.168.3.131\",\"Spline4CodeReaderPort\":\"2001\",\"Spline5CodeReaderCodeNum\":\"290\",\"Spline5CodeReaderCodeStartPosition\":\"291\",\"Spline5CodeReaderIP\":\"192.168.3.131\",\"Spline5CodeReaderPort\":\"2001\",\"Spline6CodeReaderCodeNum\":\"300\",\"Spline6CodeReaderCodeStartPosition\":\"301\",\"Spline6CodeReaderIP\":\"192.168.3.131\",\"Spline6CodeReaderPort\":\"2001\",\"UnloadAndLoadEndOnPlc\":\"21\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidateEscCommandTimeOut\":\"120000\",\"ValidateMushroomEndTimeOut\":\"90000\",\"ValidatePressBoardEndTimeout\":\"90000\",\"WarningInfoWriteOnPlc\":\"109\"},\"ProductId\":\"drill\",\"DeviceId\":\"D10-2849-243\",\"DeviceName\":\"Vega-Auto-D10-2849\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.JingWangAgent.Devices.CNC95Drill.RearDrill2849D10\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":5,\"SiloLimit\":1,\"PanelLimit\":5,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":3,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','0','drill');
INSERT INTO `t_device` VALUES (77,'MockDrill0508jingwang-4','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:08:41','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,268,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-4\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (78,'MockDrill0508jingwang-6','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:08:41','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,269,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-6\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (79,'MockDrill0508jingwang-5','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:08:41','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,270,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-5\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (80,'MockDrill0508jingwang-15','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,277,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-15\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (81,'MockDrill0508jingwang-11','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,271,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-11\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (82,'MockDrill0508jingwang-7','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,272,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-7\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (83,'MockDrill0508jingwang-14','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,276,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-14\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (84,'MockDrill0508jingwang-13','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,275,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-13\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (85,'MockDrill0508jingwang-12','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,274,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-12\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (86,'MockDrill0508jingwang-8','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,273,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-8\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (87,'MockDrill0508jingwang-9','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,278,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-9\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (88,'MockDrill0508jingwang-10','MockDrill0508jingwang',NULL,0,0,1,1,'2023-10-09 14:32:48','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,279,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-10\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (90,'MockAGV05','MockAGV',NULL,0,0,1,1,'2023-10-13 09:37:51','2023-11-27 10:13:40',1,8,NULL,NULL,NULL,0,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://localhost:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://localhost:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"ModbusTcpSlaveId\":\"1\",\"ModbusTcpUri\":\"http://192.168.3.20:502\",\"MoveTimeout\":\"300\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://localhost:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"1\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGV05\",\"DeviceName\":\"MockAGV\",\"HostAddress\":\"\",\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockAGV\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":30,\"KeepingPlcConnectionPerSeconds\":30,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":6,\"InputCapabilities\":[1,40000,30000],\"OutputCapabilities\":[30100,40100,1]}','3','agv');
INSERT INTO `t_device` VALUES (93,'cs1','测试1',NULL,0,0,1,1,'2023-10-18 14:29:01','2023-10-31 16:07:44',1,7,NULL,NULL,NULL,NULL,2,'2023-10-19 00:00:00','2023-10-21',NULL,'','1','drill');
INSERT INTO `t_device` VALUES (94,'2222','2222',NULL,0,0,1,1,'2023-10-25 13:45:06','2023-11-23 09:42:03',1,8,NULL,NULL,NULL,NULL,2,'2023-10-28 00:00:00','2023-10-30',NULL,'测试','7','agv');
INSERT INTO `t_device` VALUES (95,'MockAGV-1102','MockAGV',NULL,0,0,1,1,'2023-10-31 14:02:16','2023-10-31 14:13:40',NULL,8,NULL,NULL,NULL,0,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://192.168.104.102:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"Ip\":\"127.0.0.1\",\"ModbusTcpSlaveId\":\"2\",\"ModbusTcpUri\":\"http://127.0.0.1:502\",\"MoveTimeout\":\"300\",\"Port\":\"502\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://192.168.104.102:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"1\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGV-1102\",\"DeviceName\":\"MockAGV\",\"HostAddress\":\"http://192.168.104.102:8004\",\"HostPort\":8004,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockAGV\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":30,\"KeepingPlcConnectionPerSeconds\":30,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":6,\"InputCapabilities\":[1,40000],\"OutputCapabilities\":[30100,40100,1]}','1','agv');
INSERT INTO `t_device` VALUES (96,'D5-2849-141','Vega-Auto-D5-2849',NULL,0,0,1,1,'2023-11-04 14:16:14','2023-11-04 14:16:47',NULL,7,NULL,NULL,NULL,280,NULL,NULL,NULL,'0','{\"Extra\":{\"AgvEndTimeInterval\":\"20\",\"AtpPath\":\"3225.atp\",\"BoardLengthInfOnPlc\":\"1000\",\"BoardMaxLength\":\"625\",\"BufferAgvArrivalEnd\":\"509\",\"BufferAgvPositionEnd\":\"412\",\"BufferAgvUnLoadingAndLoading\":\"508\",\"BufferAllUnloadAndLoadEnd\":\"513\",\"BufferClinkerlLayerLoadEnd\":\"3000\",\"BufferForceLoadToDrillFlag\":\"611\",\"BufferLoadToDrillFlag\":\"406\",\"BufferOnAgvPositionFlag\":\"0\",\"BufferOnWorking\":\"100\",\"BufferRawMaterialLayerLoadEnd\":\"2000\",\"BufferReadyFlag\":\"410\",\"BufferReceiveBoardFlag\":\"411\",\"BufferStartLoadingBoard\":\"510\",\"BufferStartUnLoadingBoard\":\"511\",\"BufferStatusFlag\":\"401\",\"BufferUnloadAndLoadSpline\":\"514\",\"CallAgvTimeInterval\":\"20\",\"CanUnloadAndLoadFlagWritePlc\":\"181\",\"Cnc84BufferLockOnCnc84\":\"75\",\"Cnc84DiaFileOnOff\":\"True\",\"CNC84Ip\":\"127.0.0.1\",\"CNC84Port\":\"33822\",\"Cnc84WorkEndWritePlc\":\"23\",\"CodeReaderNum\":\"6\",\"CodeReaderOnOff\":\"False\",\"DefalutBoardLength\":\"800\",\"DiaPath\":\"3225.dia\",\"DiaSearchFileExt\":\"dia\",\"DiaSearchPath\":\"f:\\\\09_test\\\\\",\"DrilBoardEndFlag\":\"58\",\"DrillBoardOnOff\":\"True\",\"DrlPath\":\"3225.drl\",\"DrlSearchFileExt\":\"drl\",\"DrlSearchPath\":\"f:\\\\09_test\\\\\",\"ExistBoardOnDrill\":\"116\",\"ExistBoardOnPlc\":\"144\",\"FrontMushroomCloseLocalTionFlag\":\"64\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"InteractivePosition\":\"RIGHT\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"127.0.0.1\",\"IsAutoCallAgv\":\"False\",\"MiddleMushroomCloseLocalTionFlag\":\"55\",\"MiddleMushroomExist\":\"True\",\"MushroomOnOff\":\"True\",\"P4FrontPostion\":\"85\",\"P4RearPostion\":\"86\",\"Port\":\"502\",\"PressBoardEndFlagOnCNC84\":\"68\",\"PressBoardOnOff\":\"True\",\"RetractToolEndTimeout\":\"5000\",\"ScanCom\":\"Com4\",\"ScanGunInfoNumOnPlc\":\"209\",\"ScanGunInfoStartPositionOnPlc\":\"210\",\"ScanGunOnOff\":\"False\",\"SelectSplineMFunction\":\"107\",\"SelectSplineOnOff\":\"True\",\"SlaveID\":\"1\",\"SmallBoardMaxLength\":\"740\",\"SmallBoardUserFlag\":\"59\",\"Spindles\":\"00001,00002,00003,00004,00005\",\"Spline1CodeReaderCodeNum\":\"250\",\"Spline1CodeReaderCodeStartPosition\":\"251\",\"Spline1CodeReaderIP\":\"192.168.3.111\",\"Spline1CodeReaderPort\":\"2001\",\"Spline2CodeReaderCodeNum\":\"260\",\"Spline2CodeReaderCodeStartPosition\":\"261\",\"Spline2CodeReaderIP\":\"192.168.3.111\",\"Spline2CodeReaderPort\":\"2001\",\"Spline3CodeReaderCodeNum\":\"270\",\"Spline3CodeReaderCodeStartPosition\":\"271\",\"Spline3CodeReaderIP\":\"192.168.3.111\",\"Spline3CodeReaderPort\":\"2001\",\"Spline4CodeReaderCodeNum\":\"280\",\"Spline4CodeReaderCodeStartPosition\":\"281\",\"Spline4CodeReaderIP\":\"192.168.3.111\",\"Spline4CodeReaderPort\":\"2001\",\"Spline5CodeReaderCodeNum\":\"290\",\"Spline5CodeReaderCodeStartPosition\":\"291\",\"Spline5CodeReaderIP\":\"192.168.3.111\",\"Spline5CodeReaderPort\":\"2001\",\"Spline6CodeReaderCodeNum\":\"300\",\"Spline6CodeReaderCodeStartPosition\":\"301\",\"Spline6CodeReaderIP\":\"192.168.3.111\",\"Spline6CodeReaderPort\":\"2001\",\"UnloadAndLoadEndOnPlc\":\"21\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidateEscCommandTimeOut\":\"120000\",\"ValidateMushroomEndTimeOut\":\"90000\",\"ValidatePressBoardEndTimeout\":\"90000\",\"WarningInfoWriteOnPlc\":\"109\"},\"ProductId\":\"drill\",\"DeviceId\":\"D5-2849-141\",\"DeviceName\":\"Vega-Auto-D5-2849\",\"HostAddress\":\"http://192.168.104.102:8004\",\"HostPort\":8004,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"Vega.JingWangAgent.Devices.CNC84Drill.RearDrill2849\",\"DeviceDllFilePath\":\"Vega.JingWangAgent.dll\",\"DataCollectingPerSeconds\":1,\"KeepingPlcConnectionPerSeconds\":1,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":5,\"SiloLimit\":1,\"PanelLimit\":5,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (97,'D10-2849-241','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-07 17:19:27','2023-11-27 10:13:40',1,7,NULL,NULL,NULL,281,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"D10-2849-241\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://localhost:8565\",\"HostPort\":8565,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','0','drill');
INSERT INTO `t_device` VALUES (98,'D10-2849-242','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-07 17:19:27','2023-11-27 10:13:40',1,7,NULL,NULL,NULL,282,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"D10-2849-242\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://localhost:8565\",\"HostPort\":8565,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','0','drill');
INSERT INTO `t_device` VALUES (99,'D5-2849-244','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-09 14:06:23','2023-11-26 11:37:21',1,7,NULL,NULL,NULL,283,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"QuerySchedule\":\"http://192.168.102.58:8001/v1/central/mes/QuerySchedule?traceId={0}\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"D5-2849-244\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://192.168.104.102:8004\",\"HostPort\":8004,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerMilliSeconds\":500,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (100,'MockAGV07','MockAGV',NULL,0,0,1,1,'2023-11-15 11:09:49','2023-11-16 08:59:18',1,8,NULL,NULL,NULL,0,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://192.168.104.102:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"ModbusTcpSlaveId\":\"1\",\"ModbusTcpUri\":\"http://127.0.0.1:502\",\"MoveTimeout\":\"300\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://192.168.104.102:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"1\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGV07\",\"DeviceName\":\"MockAGV\",\"HostAddress\":\"http://192.168.104.102:8508\",\"HostPort\":8508,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockAGV\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":100,\"ScheduleTaskExecutingPerSeconds\":1,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":6,\"InputCapabilities\":[1,40000,30000],\"OutputCapabilities\":[30100,40100,1]}','1','agv');
INSERT INTO `t_device` VALUES (101,'MockAGV08','MockAGV',NULL,0,0,1,1,'2023-11-15 11:09:51','2023-11-16 08:59:18',1,8,NULL,NULL,NULL,0,NULL,NULL,NULL,'0','{\"Extra\":{\"AGVMoveInfo\":\"http://192.168.102.59:5000/v1/central/agv/status\",\"AGVMoveStart\":\"http://192.168.3.15:9502/api/wcstask/AddTask\",\"AgvToken\":\"YWRtaW4sMTk5ODg4NTcxMDk4Niw4YTRlNmJiN2Y5NjE1OTRkNjUzNjQ1OTIwODQyYTczMQ==\",\"CarAllInfo\":\"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\"CheckStatus\":\"http://192.168.104.102:8001/v1/central/Device/CheckStatus\",\"GetNextPanelNumber\":\"http://192.168.104.102:8001/v1/central/device/GetNextPanelNumber?count={0}\",\"ModbusTcpSlaveId\":\"1\",\"ModbusTcpUri\":\"http://127.0.0.1:502\",\"MoveTimeout\":\"300\",\"PostAndGetTimeout\":\"10\",\"RequestCharge\":\"http://192.168.104.102:9701/api/v2/CarState/SetToCharge\",\"RequestPickUpSilo\":\"http://192.168.102.59:5000/v1/central/agv/PickUpSilo\",\"RequestPutDownSilo\":\"http://192.168.102.59:5000/v1/central/agv/PutDownSilo\",\"SiloLayerCount\":\"18\",\"Spindles\":\"1\"},\"ProductId\":\"agv\",\"DeviceId\":\"MockAGV08\",\"DeviceName\":\"MockAGV\",\"HostAddress\":\"http://192.168.104.102:8508\",\"HostPort\":8508,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockAGV\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":100,\"ScheduleTaskExecutingPerSeconds\":1,\"SpindleNum\":1,\"SiloLimit\":1,\"PanelLimit\":18,\"LotLimit\":144,\"LayerLimit\":18,\"DeviceKind\":6,\"InputCapabilities\":[1,40000,30000],\"OutputCapabilities\":[30100,40100,1]}','1','agv');
INSERT INTO `t_device` VALUES (102,'MockDrill0508jingwang-22','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,298,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-22\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (103,'MockDrill0508jingwang-30','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,289,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-30\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (104,'MockDrill0508jingwang-29','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,294,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-29\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (105,'MockDrill0508jingwang-19','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,295,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-19\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (106,'MockDrill0508jingwang-23','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,292,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-23\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (107,'MockDrill0508jingwang-16','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,293,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-16\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (108,'MockDrill0508jingwang-25','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,287,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-25\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (109,'MockDrill0508jingwang-18','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,286,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-18\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (110,'MockDrill0508jingwang-17','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,288,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-17\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (111,'MockDrill0508jingwang-21','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,299,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-21\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (112,'MockDrill0508jingwang-24','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,291,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-24\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (113,'MockDrill0508jingwang-28','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,300,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-28\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (114,'MockDrill0508jingwang-27','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,297,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-27\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (115,'MockDrill0508jingwang-26','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,296,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-26\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (116,'MockDrill0508jingwang-20','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:44:11','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,290,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-20\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8513,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (117,'MockDrill0508jingwang-31','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,301,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-31\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (118,'MockDrill0508jingwang-40','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,309,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-40\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (119,'MockDrill0508jingwang-33','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,303,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-33\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (120,'MockDrill0508jingwang-37','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,307,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-37\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (121,'MockDrill0508jingwang-38','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,306,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-38\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (122,'MockDrill0508jingwang-32','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,302,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-32\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (123,'MockDrill0508jingwang-35','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,305,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-35\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (124,'MockDrill0508jingwang-41','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,308,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-41\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (125,'MockDrill0508jingwang-34','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,304,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-34\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (126,'MockDrill0508jingwang-44','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,310,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-44\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (127,'MockDrill0508jingwang-51','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,316,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-51\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (128,'MockDrill0508jingwang-50','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,315,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-50\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (129,'MockDrill0508jingwang-56','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,320,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-56\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (130,'MockDrill0508jingwang-59','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,322,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-59\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (131,'MockDrill0508jingwang-53','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,318,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-53\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (132,'MockDrill0508jingwang-52','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,317,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-52\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (133,'MockDrill0508jingwang-55','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,319,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-55\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (134,'MockDrill0508jingwang-49','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,314,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-49\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (135,'MockDrill0508jingwang-46','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,312,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-46\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (136,'MockDrill0508jingwang-48','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,313,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-48\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (137,'MockDrill0508jingwang-43','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,311,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-43\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (138,'MockDrill0508jingwang-58','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,321,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-58\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (139,'MockDrill0508jingwang-42','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,323,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-42\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (140,'MockDrill0508jingwang-47','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,326,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-47\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (141,'MockDrill0508jingwang-60','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,327,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-60\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (142,'MockDrill0508jingwang-36','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,324,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-36\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"http://DESKTOP-2VL2BDL:8514\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (143,'MockDrill0508jingwang-45','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,325,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-45\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (144,'MockDrill0508jingwang-39','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,328,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-39\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (145,'MockDrill0508jingwang-54','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,329,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-54\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
INSERT INTO `t_device` VALUES (146,'MockDrill0508jingwang-57','MockDrill0508jingwang',NULL,0,0,1,1,'2023-11-21 09:50:12','2023-11-21 16:55:05',1,7,NULL,NULL,NULL,330,NULL,NULL,NULL,'0','{\"Extra\":{\"AtpPath\":\"F:\\\\09_TEST\\\\3225.atp\",\"DefaultPanelWidth\":\"622\",\"DefaultPinOffset\":\"0\",\"DiaPath\":\"F:\\\\09_TEST\\\\3225.dia\",\"DrlPath\":\"F:\\\\09_TEST\\\\3225.drl\",\"InvokeLoadMaterialTimeout\":\"5000\",\"InvokeUnloadMaterialTimeout\":\"5000\",\"Ip\":\"localhost\",\"Port\":\"502\",\"RetractToolEndTimeout\":\"5000\",\"SlaveID\":\"1\",\"Spindles\":\"AGV#91,AGV#91,AGV#91,AGV#92,AGV#92,AGV#92\",\"ValidateCmFileTimeout\":\"5000\",\"ValidateDiaFileTimeout\":\"5000\",\"ValidateDrlFileTimeout\":\"5000\",\"ValidatePressBoardEnd\":\"5000\"},\"ProductId\":\"drill\",\"DeviceId\":\"MockDrill0508jingwang-57\",\"DeviceName\":\"MockDrill0508jingwang\",\"HostAddress\":\"\",\"HostPort\":8514,\"AutoMode\":true,\"SoloMode\":false,\"DeviceClazz\":\"VgAutoDrill.Iot.Mock.MockDrill\",\"DeviceDllFilePath\":\"VgAutoDrill.Iot.Mock.dll\",\"DataCollectingPerSeconds\":3,\"KeepingPlcConnectionPerSeconds\":10,\"KeepingPlcConnectionPerMilliSeconds\":500,\"KeepingPlcHeartPerSeconds\":1,\"ScheduleTaskExecutingPerSeconds\":10,\"SpindleNum\":6,\"SiloLimit\":1,\"PanelLimit\":6,\"LotLimit\":144,\"LayerLimit\":1,\"DeviceKind\":2,\"InputCapabilities\":[30100],\"OutputCapabilities\":[40000]}','1','drill');
/*!40000 ALTER TABLE `t_device` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_and_route`
--

DROP TABLE IF EXISTS `t_device_and_route`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_and_route` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` bigint(20) DEFAULT NULL COMMENT '设备ID',
  `device_code` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '设备编号',
  `device_name` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '设备名称',
  `device_type_id` int(11) DEFAULT NULL COMMENT '设备类型ID',
  `device_type_code` varchar(50) DEFAULT NULL COMMENT '设备类型代码',
  `route_id` bigint(20) DEFAULT NULL COMMENT '工艺路线ID',
  `route_code` varchar(64) DEFAULT NULL COMMENT '工艺路线编码',
  `route_name` varchar(255) DEFAULT NULL COMMENT '工艺路线名称',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `index_unique` (`device_id`,`route_id`)
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备关联工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_and_route`
--

LOCK TABLES `t_device_and_route` WRITE;
/*!40000 ALTER TABLE `t_device_and_route` DISABLE KEYS */;
INSERT INTO `t_device_and_route` VALUES (4,0,'string','string',0,'string',0,'string','string',0,1,1,'2023-10-10 10:20:36',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (73,90,'MockAGV05','MockAGV',8,'agv',2,'B0002','工艺路线B0002',0,1,1,'2023-10-13 15:44:07',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (74,32,'MockAgv1003','MockAgv1003',8,'agv',20,'D0001','单钻孔',0,1,1,'2023-10-20 15:37:52',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (75,32,'MockAgv1003','MockAgv1003',8,'agv',19,'C0003','工艺路线C0003',0,1,1,'2023-10-20 15:37:52',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (78,60,'MockAGV02jinlu','MockAGV02jinlu',8,'agv',2,'B0002','工艺路线B0002',0,1,57,'2023-11-22 09:14:20',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (79,60,'MockAGV02jinlu','MockAGV02jinlu',8,'agv',19,'C0003','工艺路线C0003',0,1,57,'2023-11-22 09:14:20',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (84,18,'MockAgv03','MockAgv03',8,'agv',2,'B0002','工艺路线B0002',0,1,57,'2023-11-22 09:16:30',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (98,94,'2222','2222',8,'agv',3,'A0001','工艺路线A0001',0,1,1,'2023-11-22 09:30:17',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (99,14,'MockAgv02','MockAgv02',8,'agv',2,'B0002','工艺路线B0002',0,1,1,'2023-11-22 16:54:36',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (100,14,'MockAgv02','MockAgv02',8,'agv',19,'C0003','工艺路线C0003',0,1,1,'2023-11-22 16:54:36',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (101,14,'MockAgv02','MockAgv02',8,'agv',20,'D0001','单钻孔',0,1,1,'2023-11-22 16:54:36',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (102,14,'MockAgv02','MockAgv02',8,'agv',3,'A0001','工艺路线A0001',0,1,1,'2023-11-22 16:54:36',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (103,35,'MockTonyAgv001','MockTonyAgv001',8,'agv',3,'A0001','工艺路线A0001',0,1,1,'2023-11-22 16:54:59',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (104,35,'MockTonyAgv001','MockTonyAgv001',8,'agv',2,'B0002','工艺路线B0002',0,1,1,'2023-11-22 16:54:59',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (105,35,'MockTonyAgv001','MockTonyAgv001',8,'agv',19,'C0003','工艺路线C0003',0,1,1,'2023-11-22 16:54:59',NULL,NULL);
INSERT INTO `t_device_and_route` VALUES (106,35,'MockTonyAgv001','MockTonyAgv001',8,'agv',20,'D0001','单钻孔',0,1,1,'2023-11-22 16:54:59',NULL,NULL);
/*!40000 ALTER TABLE `t_device_and_route` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_maintain`
--

DROP TABLE IF EXISTS `t_device_maintain`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_maintain` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` int(11) NOT NULL COMMENT '设备id',
  `device_code` varchar(200) DEFAULT NULL COMMENT '设备编码',
  `device_name` varchar(200) DEFAULT NULL COMMENT '设备名称',
  `maintain_time` datetime DEFAULT NULL COMMENT '维护时间',
  `maintain_id` int(11) DEFAULT NULL COMMENT '维护人ID',
  `maintain_person` varchar(100) DEFAULT NULL COMMENT '维护人',
  `maintain_status` char(2) DEFAULT '-1' COMMENT '维护状态(-1/0/1)',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='维护记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_maintain`
--

LOCK TABLES `t_device_maintain` WRITE;
/*!40000 ALTER TABLE `t_device_maintain` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_maintain` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_maintain_detail`
--

DROP TABLE IF EXISTS `t_device_maintain_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_maintain_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `master_id` int(11) NOT NULL COMMENT '维护主表ID',
  `device_id` int(11) NOT NULL COMMENT '设备id',
  `subject_id` int(11) NOT NULL COMMENT '点检保养项目id',
  `subject_code` varchar(50) DEFAULT NULL COMMENT '点检保养项目编号',
  `subject_name` varchar(50) DEFAULT NULL COMMENT '点检保养项目名称',
  `maintain_result` varchar(100) DEFAULT NULL COMMENT '检验结果',
  `better_steps` varchar(500) DEFAULT NULL COMMENT '改善措施',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='设备维护明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_maintain_detail`
--

LOCK TABLES `t_device_maintain_detail` WRITE;
/*!40000 ALTER TABLE `t_device_maintain_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_maintain_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_panel`
--

DROP TABLE IF EXISTS `t_device_panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_panel` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` bigint(20) DEFAULT NULL COMMENT '设备ID',
  `device_code` varchar(50) DEFAULT NULL COMMENT '设备编号',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料二维码',
  `silo_code` varchar(50) DEFAULT NULL COMMENT '料仓二维码',
  `item_no` varchar(50) DEFAULT NULL COMMENT '料号',
  `lot_id` varchar(50) DEFAULT NULL COMMENT 'Lot二维码',
  `batch_id` varchar(50) DEFAULT NULL COMMENT '批次号',
  `layer` int(11) DEFAULT NULL COMMENT '第几层',
  `position` int(20) DEFAULT NULL COMMENT '位置ID',
  `product_status` int(11) DEFAULT '0' COMMENT '成品状态',
  `drill_state` int(11) DEFAULT NULL COMMENT 'Panel的钻孔状态',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备负载板料表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_panel`
--

LOCK TABLES `t_device_panel` WRITE;
/*!40000 ALTER TABLE `t_device_panel` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_panel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_panel_history`
--

DROP TABLE IF EXISTS `t_device_panel_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_panel_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` bigint(20) DEFAULT NULL COMMENT '设备ID',
  `device_code` varchar(50) DEFAULT NULL COMMENT '设备编号',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料二维码',
  `silo_code` varchar(50) DEFAULT NULL COMMENT '料仓二维码',
  `item_no` varchar(50) DEFAULT NULL COMMENT '料号',
  `lot_id` varchar(50) DEFAULT NULL COMMENT 'Lot二维码',
  `batch_id` varchar(50) DEFAULT NULL COMMENT '批次号',
  `layer` int(11) DEFAULT NULL COMMENT '第几层',
  `position` int(20) DEFAULT NULL COMMENT '位置ID',
  `product_status` int(11) DEFAULT '0' COMMENT '成品状态',
  `drill_state` int(11) DEFAULT NULL COMMENT 'Panel的钻孔状态',
  `device_panel_time` datetime DEFAULT NULL COMMENT '设备负载板料时间',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备负载板料历史表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_panel_history`
--

LOCK TABLES `t_device_panel_history` WRITE;
/*!40000 ALTER TABLE `t_device_panel_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_panel_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_parameter`
--

DROP TABLE IF EXISTS `t_device_parameter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_parameter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `device_type_id` int(11) NOT NULL COMMENT '设备类型',
  `device_id` int(11) DEFAULT NULL COMMENT '设备编号',
  `code` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '配置标识',
  `name` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '名称',
  `parameters` varchar(2000) CHARACTER SET utf8 DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备参数';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_parameter`
--

LOCK TABLES `t_device_parameter` WRITE;
/*!40000 ALTER TABLE `t_device_parameter` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_parameter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_subject`
--

DROP TABLE IF EXISTS `t_device_subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_subject` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` int(11) NOT NULL COMMENT '设备id',
  `subject_id` int(11) NOT NULL COMMENT '点检保养项目id',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='设备点检项目模板';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_subject`
--

LOCK TABLES `t_device_subject` WRITE;
/*!40000 ALTER TABLE `t_device_subject` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_subject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_type`
--

DROP TABLE IF EXISTS `t_device_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `is_manufacture` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否属于生产设备',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_type`
--

LOCK TABLES `t_device_type` WRITE;
/*!40000 ALTER TABLE `t_device_type` DISABLE KEYS */;
INSERT INTO `t_device_type` VALUES (1,'全部',0,0,1,1,'2023-03-23 01:41:25','2023-04-01 11:03:28',1,'000','0',0);
INSERT INTO `t_device_type` VALUES (7,'钻机',1,0,1,1,'2023-02-28 10:26:41','2023-10-17 10:32:57',1,'drill','0,1,14',1);
INSERT INTO `t_device_type` VALUES (8,'AGV',1,0,1,1,'2023-02-28 13:27:37','2023-04-01 11:01:25',1,'agv','0,1',0);
INSERT INTO `t_device_type` VALUES (14,'叠板机',1,0,1,1,'2023-03-23 02:45:57','2023-04-01 11:01:48',1,'pin','0,1',1);
INSERT INTO `t_device_type` VALUES (15,'拆板机',1,0,1,1,'2023-03-23 02:46:09','2023-04-01 11:01:19',1,'unpin','0,1',1);
INSERT INTO `t_device_type` VALUES (16,'生料仓暂存台',1,0,1,1,'2023-03-23 02:46:19','2023-04-01 11:01:13',1,'RawStagingDesk','0,1',0);
INSERT INTO `t_device_type` VALUES (17,'熟料仓暂存台',1,0,0,1,'2023-03-23 02:46:28','2023-07-28 09:30:48',1,'ProcessedStagingDesk','0,1',0);
/*!40000 ALTER TABLE `t_device_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_drill_work_order`
--

DROP TABLE IF EXISTS `t_drill_work_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_drill_work_order` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '记录ID',
  `workorder_id` bigint(20) DEFAULT NULL COMMENT '生产工单ID',
  `workorder_code` varchar(64) DEFAULT NULL COMMENT '生产工单编号',
  `item_id` bigint(20) DEFAULT NULL COMMENT '产品物料ID',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `panel_count` decimal(12,2) DEFAULT NULL COMMENT '叠板层数',
  `quantity` decimal(12,2) DEFAULT NULL COMMENT '数量',
  `wad_count` decimal(12,2) DEFAULT NULL COMMENT '计划叠数',
  `shaft_count` decimal(12,2) DEFAULT NULL COMMENT '轴数',
  `all_passes_count` decimal(12,2) DEFAULT NULL COMMENT '总趟数',
  `remainder_passes_count` decimal(12,2) DEFAULT NULL COMMENT '本次要排趟数',
  `drill_count` decimal(12,2) DEFAULT NULL COMMENT '孔数',
  `scheduled_count` decimal(12,2) DEFAULT NULL COMMENT '已排趟数',
  `usable_count` decimal(12,2) DEFAULT NULL COMMENT '本次可用叠数',
  `single_trip_time` decimal(12,2) DEFAULT NULL COMMENT '单趟预计耗时',
  `drill_all_time` decimal(12,2) DEFAULT NULL COMMENT '钻孔总耗时',
  `single_trips` decimal(12,2) DEFAULT NULL COMMENT '单机趟数',
  `dispense_machines` decimal(12,2) DEFAULT NULL COMMENT '建议机台数',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `is_submited` tinyint(4) DEFAULT '0' COMMENT '是否已提交',
  `submit_user` int(11) DEFAULT NULL COMMENT '提交人',
  `submit_time` datetime DEFAULT NULL COMMENT '提交时间',
  `is_add_task` tinyint(4) DEFAULT '0' COMMENT '是否添加任务',
  `add_task_user` int(11) DEFAULT NULL COMMENT '添加任务人',
  `add_task_time` datetime DEFAULT NULL COMMENT '添加任务时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻孔工单表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_drill_work_order`
--

LOCK TABLES `t_drill_work_order` WRITE;
/*!40000 ALTER TABLE `t_drill_work_order` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_drill_work_order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_encode_build_rules`
--

DROP TABLE IF EXISTS `t_encode_build_rules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_encode_build_rules` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `rules_code` varchar(100) NOT NULL COMMENT '规则编号',
  `rules_name` varchar(100) NOT NULL COMMENT '规则名称',
  `prefix` varchar(50) DEFAULT NULL COMMENT '前缀',
  `number_length` int(11) NOT NULL COMMENT '流水号长度',
  `is_padded` tinyint(4) NOT NULL DEFAULT '1' COMMENT '是否补齐',
  `suffix` varchar(50) DEFAULT NULL COMMENT '后缀',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `has_year` tinyint(1) DEFAULT NULL COMMENT '是否包含年份',
  `has_month` tinyint(1) DEFAULT NULL COMMENT '是否包含月份',
  `has_day` tinyint(1) DEFAULT NULL COMMENT '是否包含天数',
  PRIMARY KEY (`id`),
  UNIQUE KEY `rules_code_UNIQUE` (`rules_code`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COMMENT='编码生成规则';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_encode_build_rules`
--

LOCK TABLES `t_encode_build_rules` WRITE;
/*!40000 ALTER TABLE `t_encode_build_rules` DISABLE KEYS */;
INSERT INTO `t_encode_build_rules` VALUES (1,'UNITMEASURE_CODE','计量单位编码','JL',6,1,'DW','',0,1,1,'2023-05-18 02:58:43','2023-11-03 10:00:34',1,0,1,1);
INSERT INTO `t_encode_build_rules` VALUES (2,'ITEMTYPE_CODE','物料产品分类编码','IT',6,1,'BM','',0,1,1,'2023-05-18 02:58:43','2023-09-04 09:54:52',1,1,0,0);
INSERT INTO `t_encode_build_rules` VALUES (3,'ITEM_CODE','物料产品编码','IF',6,1,'A1','',0,1,1,'2023-05-18 02:58:43','2023-09-04 09:54:55',1,0,0,1);
INSERT INTO `t_encode_build_rules` VALUES (4,'WORKSTATION_CODE','工作站编码','WS',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-09-04 09:54:59',1,1,1,0);
INSERT INTO `t_encode_build_rules` VALUES (5,'CLIENT_CODE','客户管理编码','KH',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-08-31 10:33:15',1,0,1,0);
INSERT INTO `t_encode_build_rules` VALUES (6,'VENDOR_CODE','供应商编码','GYS',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-08-31 10:27:00',1,1,0,1);
INSERT INTO `t_encode_build_rules` VALUES (7,'WORKORDER_CODE','生产工单分类编码','MO',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-09-14 17:02:51',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (8,'PANEL_CODE','板料追溯编码','P',2,1,'','',0,1,1,'2023-05-18 02:58:43','2023-08-31 10:26:17',1,NULL,1,1);
INSERT INTO `t_encode_build_rules` VALUES (9,'TASK_CODE','任务编码','TO',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-09-14 17:02:02',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (10,'WAREHOUSE_CODE','仓库编码','WH',10,1,NULL,'',0,1,1,'2023-05-18 02:58:43','2023-09-20 15:21:28',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (12,'MATERIALSTOCK_IN_CODE','库存入库','MSIN',3,1,NULL,NULL,0,1,1,'2023-06-19 15:52:32','2023-09-04 09:54:36',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (13,'MATERIALSTOCK_OUT_CODE','库存出库','MSOUT',6,1,NULL,NULL,0,1,1,'2023-06-19 15:53:18','2023-09-15 09:01:16',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (14,'WORKSHOP_CODE','车间编码','CJ',6,1,'C','',0,1,1,'2023-06-19 15:53:18','2023-11-13 10:06:59',1,1,1,0);
/*!40000 ALTER TABLE `t_encode_build_rules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_event_define`
--

DROP TABLE IF EXISTS `t_event_define`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_event_define` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `event_id` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '事件ID',
  `event_name` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '事件名称',
  `event_level` tinyint(4) NOT NULL COMMENT '事件级别（1普通、2严重、3紧急）',
  `parameter_json` varchar(2000) DEFAULT NULL COMMENT '参数配置',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `device_type_id` int(11) DEFAULT NULL COMMENT '设备类型',
  `is_deleted` tinyint(4) DEFAULT '0',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='事件管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_event_define`
--

LOCK TABLES `t_event_define` WRITE;
/*!40000 ALTER TABLE `t_event_define` DISABLE KEYS */;
INSERT INTO `t_event_define` VALUES (11,'REQUEST_AGV_LOAD_MATERIAL_ONLY','只请求AGV上料',2,NULL,1,'2023-03-03 00:18:07','2023-03-28 04:02:25',1,NULL,0,1);
INSERT INTO `t_event_define` VALUES (12,'REQUEST_AGV_UNLOAD_MATERIAL_ONLY','只请求AGV下料',2,NULL,1,'2023-03-03 13:46:34',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (13,'REQUEST_AGV_LOAD_MATERIAL_THEN_UNLOAD_MATERIAL','下料后请求AGV上料',2,NULL,1,'2023-03-03 13:47:38',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (14,'REQUEST_AGV_UNLOAD_MATERIAL_THEN_LOAD_MATERIAL','上料后请求AGV下料',2,NULL,1,'2023-03-03 13:47:52',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (15,'REQUEST_AGV_CHANGE_CUTTER','请求AGV更换刀具',3,NULL,1,'2023-03-03 13:48:05',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (16,'SCAN_BOARD','扫描板料',1,NULL,1,'2023-03-03 13:49:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (17,'BUFFER_LACK_RAW_MATERIAL','BUFFER缺生料，呼叫AGV',2,NULL,1,'2023-03-03 13:56:34',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (18,'BUFFER_EXIST_CLINKER','BUFFER有熟料，呼叫AGV',2,NULL,1,'2023-03-03 14:12:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (19,'BUFFER_START_LOAD_RAW_MATERIAL','BUFFER开始上生料',1,NULL,1,'2023-03-03 14:14:52',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (20,'BUFFER_END_LOAD_RAW_MATERIAL','BUFFER结束上生料',1,NULL,1,'2023-03-03 14:16:19',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (21,'BUFFER_START_UNLOAD_CLINKER','BUFFER开始下熟料',1,NULL,1,'2023-03-03 14:17:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (22,'BUFFER_END_UNLOAD_CLINKER','BUFFER结束下熟料',1,NULL,1,'2023-03-03 14:18:02',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (23,'DRILL_START_OPEN_MUSHROOM','钻机打开蘑菇头',3,NULL,1,'2023-03-03 15:47:40',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (24,'DRILL_HALT','钻机急停',3,NULL,1,'2023-03-03 15:54:08',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (25,'DRILL_START_LOAD_FILE','钻机开始加载文件',1,NULL,1,'2023-03-03 15:55:45',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (26,'DRILL_REQUEST_RECIPE','钻机请求配方',1,NULL,1,'2023-03-03 15:57:13',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (27,'DRILL_START_WORK','钻机开始工作',1,NULL,1,'2023-03-03 16:00:31',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (28,'DRILL_ON_P1_PARKING_POSITION','钻机到达P1停车位',1,NULL,1,'2023-03-03 16:03:06',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (29,'DRILL_ON_P2_PARKING_POSITION','钻机到达P2停车位',1,NULL,1,'2023-03-03 16:28:17',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (30,'DRILL_OPEN_DOOR_EVENT','钻机开门',3,NULL,1,'2023-03-03 16:33:47',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (31,'DRILL_SCAN_CODE_EVENT','钻机扫条码',2,NULL,1,'2023-03-03 16:42:39',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (32,'PIN_REQUEST_LOAD_RAW_MATERIAL_EVENT','叠扳机上料，呼叫AGV',2,NULL,1,'2023-03-03 16:45:14',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (33,'PIN_REQUEST_UNLOAD_PROCESSED_MATERIAL_EVENT','叠扳机下料，呼叫AGV',2,NULL,1,'2023-03-03 16:45:50',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (34,'PIN_STEP_ERROR_EVENT','叠扳机发生异常',3,NULL,1,'2023-03-03 16:48:55',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (35,'UNPIN_REQUEST_LOAD_PROCESSED_MATERIAL_EVENT','拆板机上料，呼叫AGV',2,NULL,1,'2023-03-03 16:49:10',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (36,'UNPIN_REQUEST_UNLOAD_RAW_MATERIAL_EVENT','拆板机下料，呼叫AGV',2,NULL,1,'2023-03-03 16:54:35',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (37,'UNPIN_STEP_ERROR_EVENT','拆板机发生异常',3,NULL,1,'2023-03-03 16:56:18',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (39,'AGV_REQUEST_PUT_DOWN_SILO_EVENT','AGV下料仓',2,NULL,1,'2023-03-03 17:06:20',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (40,'AGV_REQUEST_PICK_UP_SILO_EVENT','AGV上料仓',2,NULL,1,'2023-03-03 17:13:07',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (41,'AGV_REQUEST_MOVE_EVENT','AGV移动',1,NULL,1,'2023-03-03 17:14:20',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (42,'AGV_REQUEST_STATUSREPORT_EVENT','AGV状态上报',3,NULL,1,'2023-03-03 17:15:05',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (43,'RAWSTAGINGDESK_REQUEST_LOAD_RAW_MATERIAL_EVENT','料仓允许进生料暂存台，呼叫AGV',2,NULL,1,'2023-03-03 17:24:46',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (44,'RAWSTAGINGDESK_REQUEST_UNLOAD_RAW_MATERIAL_EVENT','料仓允许出生料暂存台，呼叫AGV',2,NULL,1,'2023-03-03 17:26:31',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (45,'PROCESSEDSTAGINGDESK_REQUEST_LOAD_PROCESSED_MATERIAL_EVENT','料仓允许进熟料暂存台，呼叫AGV',2,NULL,1,'2023-03-07 09:47:49',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (46,'PROCESSEDSTAGINGDESK_REQUEST_UNLOAD_PROCESSED_MATERIAL_EVENT','料仓允许出熟料暂存台，呼叫AGV',2,NULL,1,'2023-03-07 09:50:53',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (47,'AGV_REQUEST_CHARGE_EVENT','AGV充电',3,'123',1,'2023-05-23 03:12:27','2023-10-08 09:55:08',1,NULL,0,0);
/*!40000 ALTER TABLE `t_event_define` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_feedback`
--

DROP TABLE IF EXISTS `t_feedback`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_feedback` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '记录ID',
  `feedback_type` varchar(64) DEFAULT NULL COMMENT '报工类型',
  `workstation_id` bigint(20) DEFAULT NULL COMMENT '工作站ID',
  `workstation_code` varchar(64) DEFAULT NULL COMMENT '工作站编号',
  `workstation_name` varchar(255) DEFAULT NULL COMMENT '工作站名称',
  `workorder_id` bigint(20) DEFAULT NULL COMMENT '生产工单ID',
  `workorder_code` varchar(64) DEFAULT NULL COMMENT '生产工单编号',
  `workorder_name` varchar(255) DEFAULT NULL COMMENT '生产工单名称',
  `process_id` bigint(20) DEFAULT NULL COMMENT '工序ID',
  `process_code` varchar(64) DEFAULT NULL COMMENT '工序编码',
  `process_name` varchar(255) DEFAULT NULL COMMENT '工序名称',
  `task_id` bigint(20) DEFAULT NULL COMMENT '生产任务ID',
  `task_code` varchar(64) DEFAULT NULL COMMENT '生产任务编号',
  `item_id` bigint(20) DEFAULT NULL COMMENT '产品物料ID',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `unit_of_measure` varchar(64) DEFAULT NULL COMMENT '单位',
  `specification` varchar(500) DEFAULT NULL COMMENT '规格型号',
  `quantity` decimal(12,2) DEFAULT NULL COMMENT '排产数量',
  `quantity_feedback` decimal(12,2) DEFAULT NULL COMMENT '本次报工数量',
  `quantity_qualified` decimal(12,2) DEFAULT NULL COMMENT '良品数量',
  `quantity_unquanlified` decimal(12,2) DEFAULT NULL COMMENT '不良品数量',
  `user_name` varchar(64) DEFAULT NULL COMMENT '报工用户名',
  `nick_name` varchar(64) DEFAULT NULL COMMENT '昵称',
  `feedback_channel` varchar(64) DEFAULT NULL COMMENT '报工途径',
  `feedback_time` datetime DEFAULT NULL COMMENT '报工时间',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `feedback_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED)',
  `key_flag` char(1) DEFAULT 'N' COMMENT '是否关键工序(N/Y)',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `panel_count` decimal(12,4) DEFAULT '1.0000' COMMENT '叠板层数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='生产报工记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_feedback`
--

LOCK TABLES `t_feedback` WRITE;
/*!40000 ALTER TABLE `t_feedback` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_feedback` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item`
--

DROP TABLE IF EXISTS `t_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `item_or_product` int(11) DEFAULT NULL COMMENT '物料1产品2',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `product_category_id` int(11) DEFAULT NULL COMMENT '产品大类ID',
  `product_category_code` varchar(255) DEFAULT NULL COMMENT '产品大类编码',
  `product_category_name` varchar(45) DEFAULT NULL COMMENT '产品大类名称',
  `warehouse_id` int(11) DEFAULT NULL COMMENT '库房ID',
  `warehouse_code` varchar(255) DEFAULT NULL COMMENT '库房编码',
  `warehouse_name` varchar(255) DEFAULT NULL COMMENT '库房名称',
  `dispense_machines` decimal(12,2) DEFAULT NULL COMMENT '建议机台数',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=91 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品定义表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item`
--

LOCK TABLES `t_item` WRITE;
/*!40000 ALTER TABLE `t_item` DISABLE KEYS */;
INSERT INTO `t_item` VALUES (36,'PCB单层板1',0,'IF2023040500002','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-04-05 01:25:38','2023-08-24 15:25:47',1,72,'P0','单面板111',NULL,NULL,'成品一库',6.00);
INSERT INTO `t_item` VALUES (60,'铜单面板',0,'IF2023042300001','DDD XXXX DD','单层Panel',1,25,'0',0,1,33,'2023-04-23 02:27:08','2023-04-23 02:27:50',33,7,'p01','单面板',NULL,NULL,'WareHouse',NULL);
INSERT INTO `t_item` VALUES (67,'板材单钻孔（叠板数2）',0,'IF20230620000002A1','叠板数2','叠板2层',2,40,'0',0,1,32,'2023-06-20 10:42:30',NULL,NULL,74,'P02','双面板',NULL,NULL,'成品一库',NULL);
INSERT INTO `t_item` VALUES (68,'板材单钻孔（叠板数2）',0,'IF20230620000005A1','叠板数2','叠板2层',2,40,'0',0,1,1,'2023-06-20 14:50:49','2023-09-22 10:35:45',1,81,'P08-01','高速电路板--客户001指定',NULL,NULL,'成品一库',0.00);
INSERT INTO `t_item` VALUES (70,'铜单面板',0,'IF2023042300002','DDD XXXX DD','单层Panel',1,25,'0',0,1,1,'2023-06-25 10:26:22',NULL,NULL,7,'p01','单面板',NULL,NULL,'WareHouse',NULL);
INSERT INTO `t_item` VALUES (73,'PCB单层板',0,'IF20230704000001A1','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-07-04 17:07:12','2023-08-10 09:22:11',1,81,'P08-01','高速电路板--客户001指定',NULL,NULL,'成品二库',NULL);
INSERT INTO `t_item` VALUES (74,'板材单钻孔（叠板数2）',0,'IF20230704000002A1','叠板数2','叠板2层',2,40,'0',0,1,1,'2023-07-04 17:07:37',NULL,NULL,74,'P02','双面板',NULL,NULL,'成品一库',NULL);
INSERT INTO `t_item` VALUES (83,'板材单钻孔0810复制（叠板数2）',0,'IF20230810000002A1','叠板数2','叠板2层',2,40,'0',0,1,1,'2023-08-10 09:21:23',NULL,NULL,74,'P02','双面板',NULL,NULL,'成品一库',NULL);
INSERT INTO `t_item` VALUES (84,'PCB单层板0810复制',0,'IF20230810000003A1','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-08-10 09:21:53','2023-10-26 14:06:25',1,81,'P08-01','高速电路板--客户001指定',NULL,NULL,'成品一库',NULL);
INSERT INTO `t_item` VALUES (87,'PCB单层板1',0,'IF06000001A1','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-09-06 10:24:24',NULL,NULL,72,'P0','单面板111',NULL,NULL,'成品一库',6.00);
INSERT INTO `t_item` VALUES (88,'PCB单层板1',0,'IF06000002A1','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-09-06 10:25:17',NULL,NULL,72,'P0','单面板111',NULL,NULL,'成品一库',6.00);
/*!40000 ALTER TABLE `t_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item_atp_file`
--

DROP TABLE IF EXISTS `t_item_atp_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item_atp_file` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `item_code` varchar(50) DEFAULT NULL COMMENT '物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '物料名称',
  `item_id` bigint(20) DEFAULT NULL COMMENT '物料ID',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `atp_file_name` varchar(100) DEFAULT NULL COMMENT 'ATP文件名',
  `atp_file_path` varchar(500) DEFAULT NULL COMMENT 'ATP文件路径',
  `atp_parameters` varchar(2000) DEFAULT NULL COMMENT 'ATP参数',
  `item_drill_file_id` bigint(20) DEFAULT NULL COMMENT '钻带参数ID',
  `item_drill_file_name` varchar(100) DEFAULT NULL COMMENT '钻带参数文件名',
  `item_drill_file_path` varchar(500) DEFAULT NULL COMMENT '钻带参数路径',
  `cutter_config_master_id` bigint(20) DEFAULT NULL COMMENT '钻孔刀具参数主表ID',
  `dia_file_name` varchar(100) DEFAULT NULL COMMENT 'dia文件名',
  `dia_file_path` varchar(500) DEFAULT NULL COMMENT 'dia文件路径',
  `disk_count` int(11) DEFAULT '6' COMMENT '刀盘数量',
  `columns_limit` int(11) DEFAULT '5' COMMENT '单盘刀列数',
  `rows_limit` int(11) DEFAULT '10' COMMENT '单盘刀行数',
  `is_generated` int(11) DEFAULT '0' COMMENT '是否自动生成',
  `generate_time` datetime DEFAULT NULL COMMENT '自动生成时间',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_atp_file`
--

LOCK TABLES `t_item_atp_file` WRITE;
/*!40000 ALTER TABLE `t_item_atp_file` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_item_atp_file` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item_atp_file_detail`
--

DROP TABLE IF EXISTS `t_item_atp_file_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item_atp_file_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `item_atp_file_id` bigint(20) NOT NULL COMMENT 'ATP文件ID',
  `code` varchar(50) DEFAULT NULL COMMENT 'ATP文件明细编号',
  `diameter` decimal(12,2) DEFAULT NULL COMMENT '直径',
  `order_num` int(11) NOT NULL COMMENT '序号',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '否已删除',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_atp_file_detail`
--

LOCK TABLES `t_item_atp_file_detail` WRITE;
/*!40000 ALTER TABLE `t_item_atp_file_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_item_atp_file_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item_drill_file`
--

DROP TABLE IF EXISTS `t_item_drill_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item_drill_file` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `item_code` varchar(50) NOT NULL COMMENT '物料编码',
  `item_name` varchar(255) NOT NULL COMMENT '物料名称',
  `item_id` bigint(20) NOT NULL COMMENT '物料ID',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `drill_file_name` varchar(100) DEFAULT NULL COMMENT '钻带参数文件名',
  `drill_file_path` varchar(500) NOT NULL COMMENT '钻带参数路径',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file`
--

LOCK TABLES `t_item_drill_file` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_item_drill_file` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item_drill_file_detail`
--

DROP TABLE IF EXISTS `t_item_drill_file_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item_drill_file_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `item_drill_file_id` bigint(20) NOT NULL COMMENT '主表文件ID',
  `code` varchar(50) NOT NULL COMMENT '钻带参数文件明细编号',
  `diameter` decimal(12,2) NOT NULL COMMENT '直径',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '否已删除',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数文件明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file_detail`
--

LOCK TABLES `t_item_drill_file_detail` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_item_drill_file_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item_type`
--

DROP TABLE IF EXISTS `t_item_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '物料类型编号',
  `item_or_product` int(11) DEFAULT NULL COMMENT '物料1产品2',
  `is_system` char(1) DEFAULT 'N' COMMENT '是否系统自带',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_type`
--

LOCK TABLES `t_item_type` WRITE;
/*!40000 ALTER TABLE `t_item_type` DISABLE KEYS */;
INSERT INTO `t_item_type` VALUES (1,'全部',0,'00',0,'Y','0',0,1,1,'2023-03-30 05:08:07','2023-04-03 02:31:48',1);
INSERT INTO `t_item_type` VALUES (2,'原料',1,'01',1,'Y','0,1',0,1,1,'2023-03-30 01:44:39','2023-04-04 02:11:29',1);
INSERT INTO `t_item_type` VALUES (4,'半成品',1,'02',1,'Y','0,1',0,1,1,'2023-03-30 05:10:36','2023-04-04 02:11:22',1);
INSERT INTO `t_item_type` VALUES (22,'成品',1,'03',2,'Y','0,1',0,1,1,'2023-04-05 01:43:14','2023-04-05 01:44:38',1);
INSERT INTO `t_item_type` VALUES (25,'铜',2,'tong',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:02:53','2023-07-25 11:53:57',1);
INSERT INTO `t_item_type` VALUES (26,'铝',2,'lv',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:10:20','2023-10-09 13:26:24',1);
INSERT INTO `t_item_type` VALUES (27,'板材',2,'panel',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:27:59',NULL,NULL);
INSERT INTO `t_item_type` VALUES (40,'单钻孔',1,'onlyDrill',2,NULL,'0,1',0,1,1,'2023-05-05 04:03:54','2023-10-19 14:26:16',1);
INSERT INTO `t_item_type` VALUES (41,'原料1',2,'yuanliao001',1,NULL,'0,1,2',0,1,1,'2023-08-28 11:24:59',NULL,NULL);
/*!40000 ALTER TABLE `t_item_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_material_stock`
--

DROP TABLE IF EXISTS `t_material_stock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_material_stock` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '库存记录ID',
  `code` varchar(255) NOT NULL COMMENT '出入库单号',
  `silo_code` varchar(255) DEFAULT NULL COMMENT '料仓编号',
  `max_layers` int(11) DEFAULT NULL COMMENT '料仓最大层数',
  `current_layers` int(11) DEFAULT NULL COMMENT '料仓实际层数',
  `in_or_out` varchar(50) DEFAULT NULL COMMENT '入库或出库（in/out）',
  `item_type_id` bigint(20) DEFAULT NULL COMMENT '物料类型ID',
  `item_id` bigint(20) NOT NULL COMMENT '产品物料ID',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `specification` varchar(500) DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(64) DEFAULT NULL COMMENT '单位',
  `quantity_transaction` decimal(12,2) DEFAULT NULL COMMENT '数量(叠数或片数)',
  `quantity_onhand` decimal(12,2) DEFAULT NULL COMMENT '在库数量(叠数或片数)',
  `batch_code` varchar(255) DEFAULT NULL COMMENT '入库批次号',
  `warehouse_id` bigint(20) DEFAULT NULL COMMENT '仓库ID',
  `warehouse_code` varchar(64) DEFAULT NULL COMMENT '仓库编码',
  `warehouse_name` varchar(255) DEFAULT NULL COMMENT '仓库名称',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `station_code` varchar(64) DEFAULT NULL COMMENT '工作站编码',
  `workorder_id` bigint(20) DEFAULT NULL COMMENT '生产工单ID',
  `workorder_code` varchar(64) DEFAULT NULL COMMENT '生产工单编号',
  `task_id` bigint(20) DEFAULT NULL COMMENT '生产任务ID',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `parent_id` bigint(20) DEFAULT '0' COMMENT '出库匹配入库ID',
  `parent_code` varchar(255) DEFAULT NULL COMMENT '出库匹配入库Code',
  `process_code` varchar(50) DEFAULT NULL COMMENT '工序编号',
  `process_name` varchar(50) DEFAULT NULL COMMENT '工序名称',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='库存记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock`
--

LOCK TABLES `t_material_stock` WRITE;
/*!40000 ALTER TABLE `t_material_stock` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_material_stock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_material_stock_detail`
--

DROP TABLE IF EXISTS `t_material_stock_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_material_stock_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `stockId` bigint(20) DEFAULT NULL COMMENT '主表ID',
  `board_code` varchar(255) DEFAULT NULL COMMENT '板料编号',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `pcs` int(11) DEFAULT NULL COMMENT '单叠数量',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `silo_code` varchar(255) DEFAULT NULL COMMENT '料仓编号',
  `layer` int(11) DEFAULT NULL COMMENT '当前层（从上往下）',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='出入库明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock_detail`
--

LOCK TABLES `t_material_stock_detail` WRITE;
/*!40000 ALTER TABLE `t_material_stock_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_material_stock_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_material_stock_overview`
--

DROP TABLE IF EXISTS `t_material_stock_overview`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_material_stock_overview` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `sum_onhand` decimal(12,2) DEFAULT NULL COMMENT '在库总数量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `sum_plan` decimal(12,2) DEFAULT NULL COMMENT '计划占用量',
  `usable_count` decimal(12,2) DEFAULT NULL COMMENT '预计可用数量',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`item_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='库存总览表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock_overview`
--

LOCK TABLES `t_material_stock_overview` WRITE;
/*!40000 ALTER TABLE `t_material_stock_overview` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_material_stock_overview` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_material_stock_overview_history`
--

DROP TABLE IF EXISTS `t_material_stock_overview_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_material_stock_overview_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `sum_onhand` decimal(12,2) DEFAULT NULL COMMENT '在库总数量',
  `sum_plan` decimal(12,2) DEFAULT NULL COMMENT '计划占用量',
  `usable_count` decimal(12,2) DEFAULT NULL COMMENT '预计可用数量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='库存总览历史表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock_overview_history`
--

LOCK TABLES `t_material_stock_overview_history` WRITE;
/*!40000 ALTER TABLE `t_material_stock_overview_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_material_stock_overview_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_material_stock_storage`
--

DROP TABLE IF EXISTS `t_material_stock_storage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_material_stock_storage` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `material_stock_code` varchar(64) DEFAULT NULL COMMENT '出入库单号',
  `batch_code` varchar(255) DEFAULT NULL COMMENT '入库批次号',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `quantity_onhand` decimal(12,2) DEFAULT NULL COMMENT '在库数量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`material_stock_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='库存入库单表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock_storage`
--

LOCK TABLES `t_material_stock_storage` WRITE;
/*!40000 ALTER TABLE `t_material_stock_storage` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_material_stock_storage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_material_stock_storage_history`
--

DROP TABLE IF EXISTS `t_material_stock_storage_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_material_stock_storage_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `material_stock_code` varchar(64) DEFAULT NULL COMMENT '出入库单号',
  `batch_code` varchar(255) DEFAULT NULL COMMENT '入库批次号',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `quantity_onhand` decimal(12,2) DEFAULT NULL COMMENT '在库数量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='库存入库单历史表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock_storage_history`
--

LOCK TABLES `t_material_stock_storage_history` WRITE;
/*!40000 ALTER TABLE `t_material_stock_storage_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_material_stock_storage_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_notify`
--

DROP TABLE IF EXISTS `t_notify`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_notify` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `notify_time` datetime DEFAULT NULL COMMENT '通知时间',
  `notify_code` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '通知类型编号',
  `notify_name` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '通知类型名称',
  `notify_msg` varchar(2000) CHARACTER SET utf8 DEFAULT NULL COMMENT '通知内容',
  `notify_ways` tinyint(4) DEFAULT NULL COMMENT '通知方式',
  `notify_ways_name` varchar(200) DEFAULT NULL COMMENT '通知方式名称',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `alarm_record_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `notify_code_UNIQUE` (`notify_code`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify`
--

LOCK TABLES `t_notify` WRITE;
/*!40000 ALTER TABLE `t_notify` DISABLE KEYS */;
INSERT INTO `t_notify` VALUES (31,'2023-04-19 13:31:28','0001','AGV充电','AGV充电',22,'钉钉',0,1,1,'2023-04-19 01:31:42','2023-05-05 02:48:27',1,NULL);
INSERT INTO `t_notify` VALUES (34,'2023-07-25 13:55:41','0002','1111','222',21,'微信',0,1,1,'2023-07-25 13:55:12',NULL,NULL,NULL);
/*!40000 ALTER TABLE `t_notify` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_notify_setting`
--

DROP TABLE IF EXISTS `t_notify_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_notify_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `notify_desc` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '描述',
  `notify_ways` tinyint(4) DEFAULT NULL COMMENT '通知方式',
  `notify_params` varchar(1000) CHARACTER SET utf8 DEFAULT NULL COMMENT '通知参数',
  `is_repeat_send` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否重复发送',
  `send_frequency` int(11) DEFAULT NULL COMMENT '发送频率',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify_setting`
--

LOCK TABLES `t_notify_setting` WRITE;
/*!40000 ALTER TABLE `t_notify_setting` DISABLE KEYS */;
INSERT INTO `t_notify_setting` VALUES (11,'大屏告警',0,'BIG_SCREEN_WARNING',NULL,NULL,NULL,0,10,0,0,1,'2023-02-28 09:14:03','2023-05-05 02:43:27',1,'0');
INSERT INTO `t_notify_setting` VALUES (12,'短信',0,'LIGHT_WARNING',NULL,NULL,NULL,0,5,0,1,1,'2023-02-28 09:19:38','2023-06-28 15:11:24',1,'0');
INSERT INTO `t_notify_setting` VALUES (21,'微信',0,'LIGHT_WARNING',NULL,NULL,NULL,1,5,0,1,1,'2023-03-28 21:29:34','2023-04-18 03:22:31',1,'0');
INSERT INTO `t_notify_setting` VALUES (22,'钉钉',0,'BIG_SCREEN_WARNING',NULL,NULL,NULL,1,5,0,0,1,'2023-03-29 01:11:08','2023-05-05 02:43:32',1,'0');
/*!40000 ALTER TABLE `t_notify_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_panel`
--

DROP TABLE IF EXISTS `t_panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_panel` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `panel_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '板料编号',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '物料代码',
  `batch_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '批次号',
  `board_location` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '板料位置',
  `product_status` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品状态',
  `pcs` int(11) DEFAULT NULL COMMENT '单叠数量',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `parent_id` bigint(20) DEFAULT '0' COMMENT '父类ID',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `panel_code_UNIQUE` (`panel_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='板料追踪';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_panel`
--

LOCK TABLES `t_panel` WRITE;
/*!40000 ALTER TABLE `t_panel` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_panel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_process`
--

DROP TABLE IF EXISTS `t_process`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_process` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `attention` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工艺要求',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (21,'叠板','pin','',0,1,1,'2023-04-05 02:30:19','2023-06-20 10:00:19',32);
INSERT INTO `t_process` VALUES (23,'拆板','unpin','',0,1,1,'2023-04-18 21:35:18','2023-06-20 10:00:22',32);
INSERT INTO `t_process` VALUES (24,'钻孔','drill','钻孔要求',0,1,1,'2023-06-25 13:07:09','2023-09-06 10:16:54',1);
INSERT INTO `t_process` VALUES (25,'组装测试001','ZZTest001','组装001',0,1,1,'2023-08-28 17:26:16','2023-08-28 17:26:29',1);
/*!40000 ALTER TABLE `t_process` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_process_recipe`
--

DROP TABLE IF EXISTS `t_process_recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_process_recipe` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `product_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
  `product_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `parameters` varchar(2000) CHARACTER SET utf8 DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工艺参数、配方管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process_recipe`
--

LOCK TABLES `t_process_recipe` WRITE;
/*!40000 ALTER TABLE `t_process_recipe` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_process_recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_produce_task`
--

DROP TABLE IF EXISTS `t_produce_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_produce_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `task_code` varchar(64) DEFAULT NULL COMMENT '任务编号',
  `work_order_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单编号',
  `work_order_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单名称',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `task_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED/BEGIN/FINISH)',
  `now_wad_count` decimal(12,2) DEFAULT NULL COMMENT '本次排产叠数',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`task_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='生产任务表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_produce_task`
--

LOCK TABLES `t_produce_task` WRITE;
/*!40000 ALTER TABLE `t_produce_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_produce_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_produce_task_history`
--

DROP TABLE IF EXISTS `t_produce_task_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_produce_task_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `task_code` varchar(64) DEFAULT NULL COMMENT '任务编号',
  `work_order_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单编号',
  `work_order_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单名称',
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `task_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED/BEGIN/FINISH)',
  `now_wad_count` decimal(12,2) DEFAULT NULL COMMENT '本次排产叠数',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='生产任务历史表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_produce_task_history`
--

LOCK TABLES `t_produce_task_history` WRITE;
/*!40000 ALTER TABLE `t_produce_task_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_produce_task_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_product_bom`
--

DROP TABLE IF EXISTS `t_product_bom`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_product_bom` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `code` varchar(64) NOT NULL COMMENT '产品结构编码',
  `name` varchar(64) DEFAULT NULL COMMENT '产品结构名称',
  `parent_id` bigint(20) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `item_id` bigint(20) NOT NULL COMMENT '物料ID',
  `quantity` decimal(12,2) DEFAULT NULL COMMENT '数量',
  `order_num` int(11) DEFAULT NULL COMMENT '序号',
  `ancestors` varchar(255) NOT NULL COMMENT '所有层级父节点',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COMMENT='产品结构表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_bom`
--

LOCK TABLES `t_product_bom` WRITE;
/*!40000 ALTER TABLE `t_product_bom` DISABLE KEYS */;
INSERT INTO `t_product_bom` VALUES (1,'code1','产品1',0,0,0.00,0,'0',1,0,1,'2023-04-05 09:22:08',NULL,NULL);
INSERT INTO `t_product_bom` VALUES (3,'code3','产品3',2,0,0.00,0,'0,2',1,0,1,'2023-04-05 09:22:54','2023-04-05 09:25:35',1);
INSERT INTO `t_product_bom` VALUES (4,'code4','产品4',3,0,0.00,0,'0,3',1,0,1,'2023-04-05 09:23:44',NULL,NULL);
INSERT INTO `t_product_bom` VALUES (5,'product001','product001',0,35,0.00,0,'0',1,0,1,'2023-04-06 13:50:59',NULL,NULL);
/*!40000 ALTER TABLE `t_product_bom` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_product_category`
--

DROP TABLE IF EXISTS `t_product_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_product_category` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `parent_id` bigint(20) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(255) NOT NULL COMMENT '产品大类编码',
  `name` varchar(255) NOT NULL COMMENT '产品大类名称',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL,
  `dispense_machines` decimal(12,2) DEFAULT NULL COMMENT '建议机台数',
  `vetting_status` tinyint(4) DEFAULT '0' COMMENT '审批状态(0:未审批;1:已审批)',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
INSERT INTO `t_product_category` VALUES (74,0,'P02','双面板',NULL,0,1,1,'2023-05-23 03:23:35','2023-10-11 08:57:29',1,'0',4.00,1);
INSERT INTO `t_product_category` VALUES (75,0,'P03','多层板',NULL,0,1,1,'2023-05-23 03:24:03','2023-09-19 10:08:40',33,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (76,0,'P04','刚性电路板',NULL,0,1,1,'2023-05-23 03:24:18','2023-10-12 17:25:43',1,'0',3.00,1);
INSERT INTO `t_product_category` VALUES (77,0,'P05','柔性电路板',NULL,0,1,1,'2023-05-23 03:24:30','2023-09-19 10:08:53',33,'0',1.00,1);
INSERT INTO `t_product_category` VALUES (78,0,'P06','刚柔结合电路板',NULL,0,1,1,'2023-05-23 03:24:44','2023-09-19 10:08:53',33,'0',1.00,1);
INSERT INTO `t_product_category` VALUES (79,0,'P07','高频电路板',NULL,0,1,1,'2023-05-23 03:24:57','2023-11-13 10:21:25',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (80,0,'P08','高速电路板',NULL,0,1,1,'2023-05-23 03:25:11','2023-09-19 10:08:53',33,'0',NULL,1);
INSERT INTO `t_product_category` VALUES (81,0,'P08-01','高速电路板--客户001指定',NULL,0,1,1,'2023-05-23 03:25:25','2023-11-13 10:12:54',1,'0',1.00,1);
INSERT INTO `t_product_category` VALUES (82,0,'producttype001','产品类型1','2222',0,1,1,'2023-08-28 11:11:09','2023-10-09 09:05:39',1,'0',2.00,0);
INSERT INTO `t_product_category` VALUES (86,0,'p01','单面版','单面板',0,1,1,'2023-09-18 14:36:44','2023-10-26 13:42:34',1,'0',2.00,1);
/*!40000 ALTER TABLE `t_product_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_route`
--

DROP TABLE IF EXISTS `t_route`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_route` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '工艺路线ID',
  `code` varchar(64) NOT NULL COMMENT '工艺路线编号',
  `name` varchar(255) NOT NULL COMMENT '工艺路线名称',
  `route_desc` varchar(500) DEFAULT NULL COMMENT '工艺路线说明',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `vetting_status` tinyint(4) DEFAULT '0' COMMENT '审批状态(0:未审批;1:已审批)',
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_unique` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route`
--

LOCK TABLES `t_route` WRITE;
/*!40000 ALTER TABLE `t_route` DISABLE KEYS */;
INSERT INTO `t_route` VALUES (2,'B0002','工艺路线B0002','叠板+钻孔+拆板','',1,0,1,'2023-04-04 15:00:09','2023-09-19 10:10:54',33,1);
INSERT INTO `t_route` VALUES (3,'A0001','工艺路线A0001','叠板+钻孔+拆板',NULL,1,0,1,'2023-04-05 01:57:50','2023-09-19 10:10:54',33,1);
INSERT INTO `t_route` VALUES (19,'C0003','工艺路线C0003','叠板+钻孔',NULL,1,0,33,'2023-04-23 02:34:38','2023-11-09 11:25:47',1,1);
INSERT INTO `t_route` VALUES (20,'D0001','单钻孔','钻孔',NULL,1,0,32,'2023-06-20 10:04:54','2023-10-11 09:54:18',1,1);
INSERT INTO `t_route` VALUES (22,'GYTest001','供应测试001','1111','1111',1,0,1,'2023-08-29 09:01:16','2023-10-13 15:45:11',1,0);
INSERT INTO `t_route` VALUES (40,'测试01','测试01','测试01111111111111111111111111','测试011111111111111111111111',1,0,1,'2023-10-09 15:50:10','2023-10-27 10:21:43',1,0);
INSERT INTO `t_route` VALUES (41,'测试02','测试02','测试02','测试02',1,0,1,'2023-10-09 15:50:19',NULL,NULL,0);
INSERT INTO `t_route` VALUES (42,'测试03','测试03','测试03','测试03',1,0,1,'2023-10-09 15:50:29',NULL,NULL,0);
INSERT INTO `t_route` VALUES (43,'测试04','测试04','测试04','测试04',1,0,1,'2023-10-09 15:50:41',NULL,NULL,0);
INSERT INTO `t_route` VALUES (44,'测试05','测试05','测试05','测试05',1,0,1,'2023-10-09 15:50:50',NULL,NULL,0);
INSERT INTO `t_route` VALUES (45,'测试06','测试06','测试06','测试06',1,0,1,'2023-10-09 15:51:01',NULL,NULL,0);
/*!40000 ALTER TABLE `t_route` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_route_and_process`
--

DROP TABLE IF EXISTS `t_route_and_process`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_route_and_process` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `route_id` bigint(20) NOT NULL COMMENT '工艺路线ID',
  `process_id` bigint(20) NOT NULL COMMENT '工序ID',
  `order_num` int(11) DEFAULT NULL COMMENT '序号',
  `key_flag` char(1) DEFAULT '0' COMMENT '是否关键工序(0/1)',
  `color` char(7) DEFAULT '#00AEF3' COMMENT '甘特图显示颜色',
  `required_time` int(11) DEFAULT NULL COMMENT '本工序耗时(Min)',
  `is_manual_check` char(1) DEFAULT NULL COMMENT '是否需要手动检查0/1',
  `self_check_num` int(11) DEFAULT NULL COMMENT '自检数量',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_unique` (`route_id`,`process_id`)
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与工序关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_process`
--

LOCK TABLES `t_route_and_process` WRITE;
/*!40000 ALTER TABLE `t_route_and_process` DISABLE KEYS */;
INSERT INTO `t_route_and_process` VALUES (44,3,24,2,'1','#75AF25',40,'0',1,1,0,50,'2023-07-06 14:24:13','2023-11-25 16:47:33',1);
INSERT INTO `t_route_and_process` VALUES (47,2,24,2,'1','#1E96AB',60,'0',0,1,0,50,'2023-07-06 16:32:21','2023-11-09 11:25:28',1);
INSERT INTO `t_route_and_process` VALUES (49,19,21,1,'0','#6D1E7F',40,'0',0,1,0,50,'2023-07-06 16:33:15','2023-10-09 10:04:27',1);
INSERT INTO `t_route_and_process` VALUES (50,19,24,2,'1','#7F1E35',120,'0',0,1,0,50,'2023-07-06 16:33:32','2023-11-09 11:25:44',1);
INSERT INTO `t_route_and_process` VALUES (51,20,24,1,'1','#E2961B',40,'0',0,1,0,50,'2023-07-06 16:33:56','2023-09-22 09:20:21',1);
INSERT INTO `t_route_and_process` VALUES (52,2,21,1,'0','#F24040',40,'0',0,1,0,50,'2023-07-06 16:35:45','2023-10-09 10:37:13',33);
INSERT INTO `t_route_and_process` VALUES (53,2,23,3,'0','#550B0B',60,'1',0,1,0,50,'2023-07-06 16:35:58','2023-10-09 10:38:03',33);
INSERT INTO `t_route_and_process` VALUES (54,21,21,2,'1','#2A1616',1232,'1',1,1,0,1,'2023-07-10 09:26:34',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (57,30,24,1,'1','#00FFDD',50,'1',0,1,0,33,'2023-09-20 16:06:10','2023-09-20 16:06:42',33);
INSERT INTO `t_route_and_process` VALUES (65,22,24,1,'0','#220606',80,'0',2,1,0,1,'2023-10-09 10:21:22','2023-10-31 13:42:11',1);
INSERT INTO `t_route_and_process` VALUES (66,22,21,2,'0','#6B2121',40,'0',0,1,0,1,'2023-10-09 10:22:20',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (67,3,21,1,'0','#5D0B0B',34,'0',1,1,0,1,'2023-10-13 15:18:09',NULL,NULL);
/*!40000 ALTER TABLE `t_route_and_process` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_route_and_product_category`
--

DROP TABLE IF EXISTS `t_route_and_product_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_route_and_product_category` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `route_id` bigint(20) NOT NULL COMMENT '工艺路线ID',
  `product_category_id` bigint(20) NOT NULL COMMENT '产品大类ID',
  `order_num` int(11) DEFAULT NULL COMMENT '序号',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_unique` (`route_id`,`product_category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与产品大类关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_product_category`
--

LOCK TABLES `t_route_and_product_category` WRITE;
/*!40000 ALTER TABLE `t_route_and_product_category` DISABLE KEYS */;
INSERT INTO `t_route_and_product_category` VALUES (254,21,77,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (255,21,78,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (256,21,79,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (258,45,80,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (270,20,79,NULL,1,0,50,'2023-07-10 13:22:06',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (271,20,80,NULL,1,0,50,'2023-07-10 13:22:06',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (289,20,78,1,1,0,1,'2023-08-14 13:16:46',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (376,20,76,3,1,0,1,'2023-08-24 10:33:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (388,20,75,4,1,0,1,'2023-08-24 15:07:00',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (389,20,82,1,1,0,1,'2023-08-28 11:12:30',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (393,22,82,5,1,0,1,'2023-08-29 09:02:17',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (397,20,84,4,1,0,1,'2023-09-12 13:28:29',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (398,22,84,5,1,0,1,'2023-09-12 13:28:29',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (442,22,72,3,1,0,1,'2023-09-18 09:34:00',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (458,19,82,7,1,0,1,'2023-09-19 10:15:43',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (461,30,87,1,1,0,33,'2023-09-20 16:07:39',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (887,19,97,3,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (888,20,97,4,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (889,22,97,5,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (891,41,97,7,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (892,42,97,8,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (893,43,97,9,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (894,44,97,10,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (905,40,76,4,1,0,1,'2023-10-15 16:26:19',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (906,40,77,2,1,0,1,'2023-10-15 16:26:19',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (907,40,78,3,1,0,1,'2023-10-15 16:26:19',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (908,3,80,2,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (910,3,81,0,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (911,3,75,5,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (912,3,82,8,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (913,3,77,3,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (914,3,78,4,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (915,3,79,1,1,0,1,'2023-10-16 14:56:14',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (978,3,86,2,1,0,1,'2023-10-27 10:21:50',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (979,19,86,5,1,0,1,'2023-10-27 10:21:50','2023-11-17 13:34:43',1);
INSERT INTO `t_route_and_product_category` VALUES (980,20,86,3,1,0,1,'2023-10-27 10:21:50','2023-11-17 13:34:43',1);
INSERT INTO `t_route_and_product_category` VALUES (982,3,74,1,1,0,1,'2023-11-13 10:27:49',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (983,19,74,2,1,0,1,'2023-11-13 10:27:49',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (985,22,74,4,1,0,1,'2023-11-13 10:27:49',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (986,3,88,1,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (988,19,88,3,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (989,20,88,4,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (990,22,88,5,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (991,40,88,6,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (992,41,88,7,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (993,42,88,8,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (994,43,88,9,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (995,44,88,10,1,0,1,'2023-11-13 10:29:38',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (996,2,74,5,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (997,2,82,9,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (998,2,75,6,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (999,2,76,5,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
/*!40000 ALTER TABLE `t_route_and_product_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_route_process_and_work_station`
--

DROP TABLE IF EXISTS `t_route_process_and_work_station`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_route_process_and_work_station` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `route_and_process_id` bigint(20) NOT NULL COMMENT '工艺路线与工序关系ID',
  `work_station_id` bigint(20) NOT NULL COMMENT '工作站ID',
  `order_num` int(11) DEFAULT NULL COMMENT '序号',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_unique` (`route_and_process_id`,`work_station_id`)
) ENGINE=InnoDB AUTO_INCREMENT=759 DEFAULT CHARSET=utf8mb4 COMMENT='工序与工作站关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_process_and_work_station`
--

LOCK TABLES `t_route_process_and_work_station` WRITE;
/*!40000 ALTER TABLE `t_route_process_and_work_station` DISABLE KEYS */;
INSERT INTO `t_route_process_and_work_station` VALUES (129,45,219,0,1,0,1,'2023-08-09 09:42:38',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (144,53,219,0,1,0,1,'2023-08-28 09:31:47',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (159,57,234,0,1,0,33,'2023-09-20 16:06:40',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (160,57,256,0,1,0,33,'2023-09-20 16:06:40',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (161,57,257,0,1,0,33,'2023-09-20 16:06:40',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (306,51,251,0,1,0,1,'2023-09-22 09:20:19',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (307,51,252,0,1,0,1,'2023-09-22 09:20:19',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (308,51,249,0,1,0,1,'2023-09-22 09:20:19',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (411,54,233,0,1,0,1,'2023-10-08 10:21:11',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (467,51,212,0,1,0,1,'2023-10-08 14:40:31',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (492,49,218,0,1,0,1,'2023-10-09 10:04:15',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (493,66,218,0,1,0,1,'2023-10-09 10:22:33',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (496,43,218,0,1,0,33,'2023-10-09 10:36:33',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (497,52,256,0,1,0,33,'2023-10-09 10:37:11',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (498,52,218,0,1,0,33,'2023-10-09 10:37:11',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (531,65,259,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (532,65,253,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (534,65,212,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (535,65,217,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (536,65,233,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (537,65,246,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (538,65,251,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (539,65,252,0,1,0,1,'2023-10-09 10:53:14',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (543,51,268,0,1,0,1,'2023-10-09 14:16:16',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (547,51,270,0,1,0,1,'2023-10-09 14:16:40',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (551,51,269,0,1,0,1,'2023-10-09 14:16:55',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (626,65,254,0,1,0,1,'2023-10-19 16:33:33',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (629,65,266,0,1,0,1,'2023-10-26 14:40:46',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (677,47,259,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (678,47,253,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (679,47,254,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (680,47,212,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (681,47,217,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (682,47,233,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (683,47,271,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (684,47,268,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (685,47,270,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (686,47,269,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (687,47,281,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (688,47,282,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (689,47,266,0,1,0,1,'2023-11-09 11:25:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (690,50,259,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (691,50,253,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (692,50,254,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (693,50,268,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (694,50,270,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (695,50,269,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (696,50,251,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (697,50,252,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (698,50,249,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (699,50,281,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (700,50,282,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (701,50,266,0,1,0,1,'2023-11-09 11:25:42',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (731,44,281,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (732,44,282,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (733,44,266,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (734,44,280,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (735,44,259,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (736,44,253,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (737,44,254,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (738,44,283,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (739,44,212,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (740,44,217,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (741,44,233,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (742,44,279,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (743,44,271,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (744,44,274,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (745,44,275,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (746,44,276,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (747,44,277,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (748,44,268,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (749,44,270,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (750,44,269,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (751,44,272,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (752,44,273,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (753,44,278,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (754,44,246,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (755,44,251,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (756,44,252,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (757,44,249,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (758,44,262,0,1,0,1,'2023-11-25 16:47:22',NULL,NULL);
/*!40000 ALTER TABLE `t_route_process_and_work_station` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_schedulement`
--

DROP TABLE IF EXISTS `t_schedulement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_schedulement` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '任务编号',
  `source_device_id` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `require_device_id` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `task_id` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `start_location` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '起点',
  `end_location` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '终点',
  `priority` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '优先级',
  `request_json` varchar(5000) CHARACTER SET utf8 DEFAULT NULL COMMENT 'request json',
  `need_republish` tinyint(4) DEFAULT '0' COMMENT '是否需要再次发布任务',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `scheduled_task_status` int(11) DEFAULT '0' COMMENT '调度任务状态',
  `allocate_time` datetime DEFAULT NULL COMMENT '分配时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `routing_key` varchar(100) DEFAULT NULL COMMENT '主叫设备的route key',
  `remark` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '备注',
  `changed_spindles` varchar(200) DEFAULT NULL COMMENT '变化后的Spindles，如null,null,00003,null,null',
  `changed_behavior` varchar(200) DEFAULT NULL COMMENT '调整后的上下料行为，如-1,0,0,-1,0',
  `item_id` int(11) DEFAULT NULL COMMENT '产品Id',
  `item_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
  `warnning_code` varchar(200) DEFAULT NULL COMMENT '告警编码',
  `warnning_message` varchar(1000) DEFAULT NULL COMMENT '告警详细信息',
  `RequestInteractionBehavior` varchar(50) DEFAULT NULL COMMENT '交互方式编码',
  `RequestInteractionBehaviorName` varchar(200) DEFAULT NULL COMMENT '交互方式描述',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_schedulement`
--

LOCK TABLES `t_schedulement` WRITE;
/*!40000 ALTER TABLE `t_schedulement` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_schedulement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_schedulement_detail`
--

DROP TABLE IF EXISTS `t_schedulement_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_schedulement_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `master_id` bigint(20) NOT NULL COMMENT '主表Id',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `message` varchar(5000) CHARACTER SET utf8 DEFAULT NULL COMMENT '调度记录明细',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_schedulement_detail`
--

LOCK TABLES `t_schedulement_detail` WRITE;
/*!40000 ALTER TABLE `t_schedulement_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_schedulement_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_subject`
--

DROP TABLE IF EXISTS `t_subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_subject` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `subject_type` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '类型 点检or保养',
  `subject_content` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '内容',
  `standard` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '标准',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='点检保养项目';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_subject`
--

LOCK TABLES `t_subject` WRITE;
/*!40000 ALTER TABLE `t_subject` DISABLE KEYS */;
INSERT INTO `t_subject` VALUES (6,'总进气压',0,'1','点检,保养','','(6-7)kg/cm²','0',0,1,1,'2023-06-21 15:55:08','2023-06-28 15:10:04',1);
INSERT INTO `t_subject` VALUES (7,'集尘负压',0,'2','点检',NULL,'(1500±500)mH20','0',0,1,1,'2023-06-21 16:01:23','2023-06-21 16:01:58',1);
INSERT INTO `t_subject` VALUES (8,'冰水温度',0,'3','点检',NULL,'(20±2)°c','0',0,1,1,'2023-06-21 16:02:56',NULL,NULL);
INSERT INTO `t_subject` VALUES (9,'吸屑罩气压',0,'4','点检',NULL,'(0.25-0.3)Mpa','0',0,1,1,'2023-06-21 16:03:44',NULL,NULL);
INSERT INTO `t_subject` VALUES (10,'主轴压头气压',0,'5','点检',NULL,'(0.58-0.65)Mpa','0',0,1,1,'2023-06-21 16:04:59',NULL,NULL);
INSERT INTO `t_subject` VALUES (11,'主轴气压',0,'6','点检',NULL,'(0.55-0.65)Mpa','0',0,1,1,'2023-06-21 16:05:24',NULL,NULL);
/*!40000 ALTER TABLE `t_subject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_task`
--

DROP TABLE IF EXISTS `t_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `work_order_id` int(11) DEFAULT NULL COMMENT '生产工单Id',
  `work_order_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单编号',
  `work_order_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单名称',
  `batch_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '批次号',
  `work_station_id` int(11) DEFAULT NULL COMMENT '工作站Id',
  `work_station_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作站名称',
  `work_station_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作站编号',
  `process_id` int(11) DEFAULT NULL COMMENT '工序Id',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `item_id` int(11) DEFAULT NULL COMMENT '产品Id',
  `item_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `quantity` decimal(11,0) DEFAULT NULL COMMENT '排产数量',
  `quantity_produced` decimal(11,0) DEFAULT NULL COMMENT '已生产数量',
  `quantity_quanlify` decimal(11,0) DEFAULT NULL COMMENT '良品数量',
  `quantity_unquanlify` decimal(11,0) DEFAULT NULL COMMENT '不良品数量',
  `client_id` int(11) DEFAULT NULL COMMENT '客户Id',
  `client_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '客户名称',
  `client_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '客户编号',
  `start_time` datetime DEFAULT NULL COMMENT '开始日期',
  `duration` int(11) DEFAULT NULL COMMENT '生产时长',
  `end_time` datetime DEFAULT NULL COMMENT '结束日期',
  `request_date` datetime DEFAULT NULL COMMENT '需求日期',
  `task_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED/BEGIN/FINISH)',
  `color` char(7) DEFAULT '#00AEF3' COMMENT '甘特图显示颜色',
  `key_flag` char(1) DEFAULT '0' COMMENT '是否关键工序(0/1)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `real_start_time` datetime DEFAULT NULL COMMENT '实际开始时间',
  `real_end_time` datetime DEFAULT NULL COMMENT '实际结束时间',
  `real_duration` int(11) DEFAULT NULL COMMENT '实际耗时',
  `is_started` tinyint(4) DEFAULT '0' COMMENT '任务是否开启',
  `now_wad_count` decimal(12,2) DEFAULT NULL COMMENT '本次排产叠数',
  `panel_count` decimal(12,4) DEFAULT '1.0000' COMMENT '叠板层数',
  `route_id` bigint(20) DEFAULT NULL COMMENT '工艺路线ID',
  `route_code` varchar(64) DEFAULT NULL COMMENT '工艺路线编码',
  `route_name` varchar(255) DEFAULT NULL COMMENT '工艺路线名称',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产任务';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_task`
--

LOCK TABLES `t_task` WRITE;
/*!40000 ALTER TABLE `t_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_trans_order`
--

DROP TABLE IF EXISTS `t_trans_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_trans_order` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `task_id` int(11) DEFAULT NULL COMMENT '任务单Id',
  `task_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '任务单编号',
  `work_order_id` int(11) DEFAULT NULL COMMENT '生产工单Id',
  `work_order_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单编号',
  `work_order_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单名称',
  `batch_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '批次号',
  `work_station_id` int(11) DEFAULT NULL COMMENT '工作站Id',
  `work_station_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作站名称',
  `work_station_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作站编号',
  `process_id` int(11) DEFAULT NULL COMMENT '工序Id',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `item_id` int(11) DEFAULT NULL COMMENT '产品Id',
  `item_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `quantity_transfered` decimal(11,0) DEFAULT NULL COMMENT '流转数量',
  `produce_date` date DEFAULT NULL COMMENT '生产日期',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产流转单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_trans_order`
--

LOCK TABLES `t_trans_order` WRITE;
/*!40000 ALTER TABLE `t_trans_order` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_trans_order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_unit_measure`
--

DROP TABLE IF EXISTS `t_unit_measure`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_unit_measure` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '单位ID',
  `code` varchar(64) NOT NULL COMMENT '单位编码',
  `name` varchar(255) NOT NULL COMMENT '单位名称',
  `primary_flag` char(1) NOT NULL DEFAULT 'Y' COMMENT '是否是主单位',
  `primary_id` bigint(20) DEFAULT NULL COMMENT '主单位ID',
  `change_rate` decimal(12,4) DEFAULT '1.0000' COMMENT '与主单位换算比例，默认为1',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=98 DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (34,'叠板2层','叠板2层','N',74,2.0000,NULL,0,1,1,'2023-04-18 22:13:06','2023-09-06 10:18:15',1);
INSERT INTO `t_unit_measure` VALUES (74,'Panel','单层Panel','Y',34,1.0000,'单位',0,1,1,'2023-05-21 22:46:43','2023-09-06 10:18:20',1);
INSERT INTO `t_unit_measure` VALUES (94,'JL20230810000002DW','Kg','Y',NULL,1.0000,NULL,0,1,1,'2023-08-10 09:11:49','2023-10-23 16:46:58',1);
INSERT INTO `t_unit_measure` VALUES (95,'JL20230810000003DW','公斤','N',94,1.0000,NULL,0,1,1,'2023-08-10 09:12:05','2023-08-10 09:17:11',1);
INSERT INTO `t_unit_measure` VALUES (96,'JL20230810000004DW','吨','N',94,1000.0000,NULL,0,1,1,'2023-08-10 09:16:07','2023-09-14 13:34:52',1);
INSERT INTO `t_unit_measure` VALUES (97,'JL20230810000005DW','克','N',94,0.0010,NULL,0,1,1,'2023-08-10 09:16:31',NULL,NULL);
/*!40000 ALTER TABLE `t_unit_measure` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_vendor`
--

DROP TABLE IF EXISTS `t_vendor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_vendor` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '供应商ID',
  `code` varchar(64) NOT NULL COMMENT '供应商编码',
  `name` varchar(255) NOT NULL COMMENT '供应商名称',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_vendor`
--

LOCK TABLES `t_vendor` WRITE;
/*!40000 ALTER TABLE `t_vendor` DISABLE KEYS */;
INSERT INTO `t_vendor` VALUES (37,'GYS20230613000001','李老板',NULL,0,1,1,'2023-04-18 22:22:01','2023-09-12 13:34:25',1);
INSERT INTO `t_vendor` VALUES (47,'GYS20230613000002','张老板','铜料板供应',0,1,1,'2023-06-06 16:10:11','2023-07-20 11:32:34',1);
INSERT INTO `t_vendor` VALUES (48,'wendor001','供应商001','测试001',0,1,1,'2023-08-28 15:31:49',NULL,NULL);
INSERT INTO `t_vendor` VALUES (49,'GYS202303000001','GYS202303000001','GYS202303000001',0,1,1,'2023-11-03 10:21:31',NULL,NULL);
/*!40000 ALTER TABLE `t_vendor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_warehouse`
--

DROP TABLE IF EXISTS `t_warehouse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_warehouse` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '仓库ID',
  `code` varchar(255) NOT NULL COMMENT '仓库编码',
  `name` varchar(255) NOT NULL COMMENT '仓库名称',
  `work_station_id` bigint(20) DEFAULT NULL COMMENT '工位位置',
  `charge` varchar(64) DEFAULT NULL COMMENT '负责人',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `work_station_code` varchar(64) DEFAULT NULL COMMENT '工位编码',
  `work_station_name` varchar(255) DEFAULT NULL COMMENT '工位名称',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COMMENT='仓库表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_warehouse`
--

LOCK TABLES `t_warehouse` WRITE;
/*!40000 ALTER TABLE `t_warehouse` DISABLE KEYS */;
INSERT INTO `t_warehouse` VALUES (16,'WH01','WH01',217,'yp1','123',0,1,1,'2023-08-23 15:06:57',NULL,NULL,'drill01-0002','钻机01-0002');
INSERT INTO `t_warehouse` VALUES (17,'changkutest001','仓库测试001',282,'shiwanli','111',0,1,1,'2023-08-29 16:59:05','2023-11-17 17:08:44',1,'D10-2849-242','MockDrill0508jingwang');
INSERT INTO `t_warehouse` VALUES (19,'WH202309200000000001','232432',238,'admin',NULL,0,1,1,'2023-09-20 15:22:01',NULL,NULL,'WS20230620000004','钻孔生料缓存二区');
INSERT INTO `t_warehouse` VALUES (20,'WH202310080000000001','123',265,'admin','123',0,1,1,'2023-10-08 10:11:25',NULL,NULL,'WH202310080000000001','123');
/*!40000 ALTER TABLE `t_warehouse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_work_order`
--

DROP TABLE IF EXISTS `t_work_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_work_order` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `order_source` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '来源类型',
  `source_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '来源单据',
  `item_id` int(11) DEFAULT NULL COMMENT '产品Id',
  `item_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `batch_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '批次号',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `quantity` decimal(11,0) DEFAULT NULL COMMENT '生产数量',
  `quantity_changed` decimal(11,0) DEFAULT NULL COMMENT '调整数量',
  `quantity_produced` decimal(11,0) DEFAULT NULL COMMENT '已生产数量',
  `quantity_scheduled` decimal(11,0) DEFAULT NULL COMMENT '已排产数量',
  `client_id` int(11) DEFAULT NULL COMMENT '客户Id',
  `client_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '客户名称',
  `client_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '客户编号',
  `request_date` datetime DEFAULT NULL COMMENT '需求日期',
  `manu_order_status` tinyint(4) DEFAULT '0' COMMENT '生产工单的状态(0-DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)',
  `work_order_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `is_add_work_order` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否增加工单任务',
  `add_work_order_id` int(11) DEFAULT NULL COMMENT '增加工单任务人id',
  `add_work_order_time` datetime DEFAULT NULL COMMENT '增加工单任务时间',
  `panel_count` decimal(12,2) DEFAULT NULL COMMENT '叠板层数',
  `wad_count` decimal(12,2) DEFAULT NULL COMMENT '待排产叠数',
  `drill_count` decimal(12,2) DEFAULT NULL COMMENT '孔数',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `route_id` bigint(20) DEFAULT NULL COMMENT '工艺路线ID',
  `route_code` varchar(64) DEFAULT NULL COMMENT '工艺路线编码',
  `route_name` varchar(255) DEFAULT NULL COMMENT '工艺路线名称',
  `dispense_machines` decimal(12,2) DEFAULT NULL COMMENT '建议机台数',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产工单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_work_order`
--

LOCK TABLES `t_work_order` WRITE;
/*!40000 ALTER TABLE `t_work_order` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_work_order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_workorder_and_workstation`
--

DROP TABLE IF EXISTS `t_workorder_and_workstation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_workorder_and_workstation` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `work_order_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '生产工单编号',
  `work_station_id` bigint(20) NOT NULL COMMENT '工作站ID',
  `work_station_code` varchar(64) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作站编码',
  `work_station_name` varchar(255) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作站名称',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='生产工单与工作站关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workorder_and_workstation`
--

LOCK TABLES `t_workorder_and_workstation` WRITE;
/*!40000 ALTER TABLE `t_workorder_and_workstation` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_workorder_and_workstation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_workshop`
--

DROP TABLE IF EXISTS `t_workshop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_workshop` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '车间ID',
  `code` varchar(64) NOT NULL COMMENT '车间编码',
  `name` varchar(255) NOT NULL COMMENT '车间名称',
  `charge` varchar(64) DEFAULT NULL COMMENT '负责人',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workshop`
--

LOCK TABLES `t_workshop` WRITE;
/*!40000 ALTER TABLE `t_workshop` DISABLE KEYS */;
INSERT INTO `t_workshop` VALUES (9,'shop02','车间02（测试）','yp','此车间的备注信息（选填）',0,1,1,'2023-04-03 22:03:36','2023-09-08 16:13:17',1);
INSERT INTO `t_workshop` VALUES (11,'shop01','车间01（测试）','admin','此车间的备注信息（选填）',0,1,1,'2023-04-10 23:31:47','2023-10-19 14:26:43',1);
INSERT INTO `t_workshop` VALUES (39,'shop03','车间03（测试）','李四',NULL,0,1,33,'2023-04-23 02:28:30','2023-11-03 10:16:31',1);
INSERT INTO `t_workshop` VALUES (40,'SMT001','SMT001车间','swl',NULL,0,1,1,'2023-08-28 11:38:31',NULL,NULL);
/*!40000 ALTER TABLE `t_workshop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_workstation`
--

DROP TABLE IF EXISTS `t_workstation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_workstation` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '工作站ID',
  `code` varchar(64) NOT NULL COMMENT '工作站编码',
  `name` varchar(255) NOT NULL COMMENT '工作站名称',
  `workshop_id` bigint(20) DEFAULT NULL COMMENT '所在车间ID',
  `workshop_code` varchar(64) DEFAULT NULL COMMENT '所在车间编码',
  `workshop_name` varchar(255) DEFAULT NULL COMMENT '所在车间名称',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `process_id` int(11) DEFAULT NULL COMMENT '默认工序名称',
  `process_code` varchar(50) DEFAULT NULL COMMENT '默认工序编码',
  `process_name` varchar(50) DEFAULT NULL COMMENT '默认工序名称',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=331 DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workstation`
--

LOCK TABLES `t_workstation` WRITE;
/*!40000 ALTER TABLE `t_workstation` DISABLE KEYS */;
INSERT INTO `t_workstation` VALUES (212,'drill01-0001','钻机01-0001',9,'shop02','车间02（测试）','',24,'drill','钻孔',0,1,1,'2023-04-04 02:32:34','2023-08-12 15:50:03',1);
INSERT INTO `t_workstation` VALUES (217,'drill01-0002','钻机01-0002',11,'shop01','车间01（测试）',NULL,24,'drill','钻孔',0,1,1,'2023-04-05 22:37:47','2023-08-12 15:50:15',1);
INSERT INTO `t_workstation` VALUES (218,'pin01-0001','叠板机01-0001',39,'shop03','车间03（测试）',NULL,21,'pin','叠板',0,1,1,'2023-04-05 22:39:06','2023-08-12 16:18:15',1);
INSERT INTO `t_workstation` VALUES (219,'unpin01-0001','拆板机01-0001',11,'shop01','车间01（测试）',NULL,23,'unpin','拆板',0,1,1,'2023-04-05 22:39:38','2023-08-12 16:18:28',1);
INSERT INTO `t_workstation` VALUES (233,'drill01-0003','钻机01-0003',11,'shop01','车间01（测试）','钻机工作站',24,'drill','钻孔',0,1,1,'2023-05-09 03:07:07','2023-08-12 15:50:20',1);
INSERT INTO `t_workstation` VALUES (235,'WS20230620000001','成品一库',9,'shop02','车间02（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 10:31:52','2023-08-12 15:20:20',1);
INSERT INTO `t_workstation` VALUES (236,'WS20230620000002','成品二库',9,'shop02','车间02（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 10:32:09','2023-08-12 15:20:16',1);
INSERT INTO `t_workstation` VALUES (237,'WS20230620000003','钻孔生料缓存一区',11,'shop01','车间01（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 10:33:10','2023-10-09 10:22:58',1);
INSERT INTO `t_workstation` VALUES (238,'WS20230620000004','钻孔生料缓存二区',9,'shop02','车间02（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 10:33:22','2023-08-12 15:21:47',1);
INSERT INTO `t_workstation` VALUES (239,'WS20230620000005','钻孔熟料缓存一区',39,'shop03','车间03（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 10:34:39','2023-08-12 15:20:40',1);
INSERT INTO `t_workstation` VALUES (240,'WS20230620000007','钻孔熟料缓存二区',9,'shop02','车间02（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 10:34:49','2023-08-12 15:20:44',1);
INSERT INTO `t_workstation` VALUES (242,'WS20230620000008','原料一库',11,'shop01','车间01（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 11:01:23','2023-08-12 15:20:48',1);
INSERT INTO `t_workstation` VALUES (243,'WS20230620000009','原料二库',9,'shop02','车间02（测试）',NULL,NULL,NULL,NULL,0,1,32,'2023-06-20 11:01:37','2023-08-12 15:20:52',1);
INSERT INTO `t_workstation` VALUES (246,'MockDrill0508jinlu','MockDrill0508jinlu',9,'shop02','车间02（测试）',NULL,24,'drill','钻孔',0,1,1,'2023-07-25 16:03:34','2023-08-12 16:18:34',1);
INSERT INTO `t_workstation` VALUES (249,'MockDrill0508jinlu-3','MockDrill0508jinlu',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-08-24 13:39:06','2023-10-08 15:41:46',1);
INSERT INTO `t_workstation` VALUES (251,'MockDrill0508jinlu-1','MockDrill0508jinlu',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-08-24 16:01:48','2023-10-08 15:41:41',1);
INSERT INTO `t_workstation` VALUES (252,'MockDrill0508jinlu-2','MockDrill0508jinlu',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-08-24 16:25:12','2023-10-08 15:41:51',1);
INSERT INTO `t_workstation` VALUES (253,'D5-2849-242','Vega-Auto-D5-2849',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-08-25 10:01:33','2023-09-20 09:06:12',1);
INSERT INTO `t_workstation` VALUES (254,'D5-2849-243','Vega-Auto-D5-2849',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-08-25 10:02:42','2023-10-09 09:59:52',1);
INSERT INTO `t_workstation` VALUES (255,'tiepianji001','贴片机001',40,'SMT001','SMT001车间',NULL,25,'ZZTest001','组装测试001',0,1,1,'2023-08-28 13:20:32','2023-08-29 15:43:06',1);
INSERT INTO `t_workstation` VALUES (259,'D5-2849-241','Vega-Auto-D5-2849',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-09-18 11:39:49',NULL,NULL);
INSERT INTO `t_workstation` VALUES (260,'SiloShelf01','Vega-Auto-SiloShelf01',NULL,NULL,NULL,NULL,0,'siloshelf','',0,1,1,'2023-09-19 13:36:46',NULL,NULL);
INSERT INTO `t_workstation` VALUES (261,'SiloShelf02','Vega-Auto-SiloShelf01',NULL,NULL,NULL,NULL,0,'siloshelf','',0,1,1,'2023-09-20 09:34:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (262,'SiloShelf03','Vega-Auto-SiloShelf01',39,'shop03','车间03（测试）',NULL,24,'drill','钻孔',0,1,1,'2023-09-21 11:17:33','2023-11-13 10:40:51',1);
INSERT INTO `t_workstation` VALUES (263,'SiloShelf11','Vega-Auto-SiloShelf01',NULL,NULL,NULL,NULL,0,'siloshelf','',0,1,1,'2023-09-22 16:23:44',NULL,NULL);
INSERT INTO `t_workstation` VALUES (264,'SiloShelf09','Vega-Auto-SiloShelf01',NULL,NULL,NULL,NULL,0,'siloshelf','',0,1,1,'2023-09-27 10:44:57',NULL,NULL);
INSERT INTO `t_workstation` VALUES (266,'D10-2849-243','Vega-Auto-D10-2849',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-08 10:56:35',NULL,NULL);
INSERT INTO `t_workstation` VALUES (268,'MockDrill0508jingwang-4','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:08:41',NULL,NULL);
INSERT INTO `t_workstation` VALUES (269,'MockDrill0508jingwang-6','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:08:41',NULL,NULL);
INSERT INTO `t_workstation` VALUES (270,'MockDrill0508jingwang-5','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:08:41',NULL,NULL);
INSERT INTO `t_workstation` VALUES (271,'MockDrill0508jingwang-11','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (272,'MockDrill0508jingwang-7','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (273,'MockDrill0508jingwang-8','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (274,'MockDrill0508jingwang-12','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (275,'MockDrill0508jingwang-13','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (276,'MockDrill0508jingwang-14','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (277,'MockDrill0508jingwang-15','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (278,'MockDrill0508jingwang-9','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (279,'MockDrill0508jingwang-10','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-10-09 14:32:48',NULL,NULL);
INSERT INTO `t_workstation` VALUES (280,'D5-2849-141','Vega-Auto-D5-2849',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-04 14:16:14',NULL,NULL);
INSERT INTO `t_workstation` VALUES (281,'D10-2849-241','MockDrill0508jingwang',39,'shop03','车间03（测试）',NULL,24,'drill','钻孔',0,1,1,'2023-11-07 17:19:27','2023-11-13 10:39:58',1);
INSERT INTO `t_workstation` VALUES (282,'D10-2849-242','MockDrill0508jingwang',9,'shop02','车间02（测试）',NULL,24,'drill','钻孔',0,1,1,'2023-11-07 17:19:27','2023-11-13 10:40:11',1);
INSERT INTO `t_workstation` VALUES (283,'D5-2849-244','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-09 14:06:22',NULL,NULL);
INSERT INTO `t_workstation` VALUES (286,'MockDrill0508jingwang-18','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (287,'MockDrill0508jingwang-25','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (288,'MockDrill0508jingwang-17','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (289,'MockDrill0508jingwang-30','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (290,'MockDrill0508jingwang-20','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (291,'MockDrill0508jingwang-24','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (292,'MockDrill0508jingwang-23','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (293,'MockDrill0508jingwang-16','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (294,'MockDrill0508jingwang-29','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (295,'MockDrill0508jingwang-19','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (296,'MockDrill0508jingwang-26','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (297,'MockDrill0508jingwang-27','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (298,'MockDrill0508jingwang-22','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (299,'MockDrill0508jingwang-21','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (300,'MockDrill0508jingwang-28','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:44:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (301,'MockDrill0508jingwang-31','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (302,'MockDrill0508jingwang-32','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (303,'MockDrill0508jingwang-33','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (304,'MockDrill0508jingwang-34','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (305,'MockDrill0508jingwang-35','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (306,'MockDrill0508jingwang-38','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (307,'MockDrill0508jingwang-37','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (308,'MockDrill0508jingwang-41','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (309,'MockDrill0508jingwang-40','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (310,'MockDrill0508jingwang-44','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (311,'MockDrill0508jingwang-43','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (312,'MockDrill0508jingwang-46','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (313,'MockDrill0508jingwang-48','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (314,'MockDrill0508jingwang-49','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (315,'MockDrill0508jingwang-50','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (316,'MockDrill0508jingwang-51','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (317,'MockDrill0508jingwang-52','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (318,'MockDrill0508jingwang-53','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (319,'MockDrill0508jingwang-55','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (320,'MockDrill0508jingwang-56','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (321,'MockDrill0508jingwang-58','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (322,'MockDrill0508jingwang-59','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (323,'MockDrill0508jingwang-42','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (324,'MockDrill0508jingwang-36','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (325,'MockDrill0508jingwang-45','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (326,'MockDrill0508jingwang-47','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (327,'MockDrill0508jingwang-60','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (328,'MockDrill0508jingwang-39','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (329,'MockDrill0508jingwang-54','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
INSERT INTO `t_workstation` VALUES (330,'MockDrill0508jingwang-57','MockDrill0508jingwang',NULL,NULL,NULL,NULL,24,'drill','钻孔',0,1,1,'2023-11-21 09:50:12',NULL,NULL);
/*!40000 ALTER TABLE `t_workstation` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-11-27 10:14:02
