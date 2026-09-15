Enter password: 
-- MySQL dump 10.13  Distrib 5.7.41, for Linux (x86_64)
--
-- Host: 192.168.104.253    Database: vg_autodrill_db
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_department`
--

LOCK TABLES `sys_department` WRITE;
/*!40000 ALTER TABLE `sys_department` DISABLE KEYS */;
INSERT INTO `sys_department` VALUES (1,'Boss科技',0,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:05:59','2023-03-23 03:12:42',1);
INSERT INTO `sys_department` VALUES (2,'深圳总公司',1,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-03-26 20:38:38',1);
INSERT INTO `sys_department` VALUES (3,'长沙分公司',1,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-03-23 03:12:32',1);
INSERT INTO `sys_department` VALUES (4,'研发部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-03-23 03:12:16',1);
INSERT INTO `sys_department` VALUES (5,'市场部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (6,'测试部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (7,'财务部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (8,'运维部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (9,'研发部门',2,'18037458723','2230485082@qq.com',NULL,'xm',0,0,1,'2023-02-23 03:10:17','2023-06-06 16:31:43',1);
INSERT INTO `sys_department` VALUES (10,'北京分公司',1,'18890899089','18890899089@163.com',NULL,'王五',0,1,1,'2023-03-23 03:01:13','2023-05-05 03:08:15',1);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
) ENGINE=InnoDB AUTO_INCREMENT=1352 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
INSERT INTO `sys_menu` VALUES (8,'DRILLAMIN','应用管理',1,'system','app','system/app/index',1,NULL,NULL,7,0,0,0,'2022-09-26 14:56:09','2023-04-04 09:49:44',1);
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
INSERT INTO `sys_menu` VALUES (1024,'DRILLAMIN','生产管理',0,'switch','produce','#',1,NULL,NULL,2,0,1,1,'2023-02-20 23:01:11','2023-05-08 05:18:00',1);
INSERT INTO `sys_menu` VALUES (1025,'DRILLAMIN','排刀管理',0,'build','material','#',1,NULL,NULL,3,0,1,1,'2023-02-20 23:01:59','2023-04-10 05:38:08',1);
INSERT INTO `sys_menu` VALUES (1026,'DRILLAMIN','设备管理',0,'star','device','#',1,NULL,NULL,4,0,1,1,'2023-02-20 23:02:35','2023-05-08 20:44:54',1);
INSERT INTO `sys_menu` VALUES (1027,'DRILLAMIN','告警管理',0,'email','alarm','#',1,NULL,NULL,5,0,1,1,'2023-02-20 23:03:32','2023-03-01 14:09:18',1);
INSERT INTO `sys_menu` VALUES (1029,'DRILLAMIN','生产排产',1024,'#','task','produce/task/index',2,'produce:task:list',NULL,6,0,1,1,'2023-02-20 23:06:58','2023-06-12 14:22:40',1);
INSERT INTO `sys_menu` VALUES (1030,'DRILLAMIN','板料追溯',1024,'#','board','produce/board/index',2,'produce:board:list',NULL,10,0,1,1,'2023-02-20 23:09:15','2023-06-12 14:23:06',1);
INSERT INTO `sys_menu` VALUES (1031,'DRILLAMIN','钻孔任务',1024,'#','drillTask','produce/drillTask/index',2,'produce:drillTask:list',NULL,9,0,1,1,'2023-02-20 23:11:13','2023-06-12 14:25:11',1);
INSERT INTO `sys_menu` VALUES (1032,'DRILLAMIN','刀盘管理',1025,'#','cutter','material/cutter/index',2,'material:cutter:list',NULL,1,0,0,1,'2023-02-20 23:15:40','2023-04-18 02:15:39',1);
INSERT INTO `sys_menu` VALUES (1033,'DRILLAMIN','设备类型',1026,'#','equipmentType','device/equipmentType/index',2,'device:equipmentType:list',NULL,1,0,1,1,'2023-02-20 23:18:29','2023-04-02 21:21:28',1);
INSERT INTO `sys_menu` VALUES (1034,'DRILLAMIN','配方管理',1024,'#','recipe','produce/recipe/index',2,'produce:recipe:list',NULL,3,0,0,1,'2023-02-20 23:20:37','2023-06-12 14:22:27',1);
INSERT INTO `sys_menu` VALUES (1035,'DRILLAMIN','设备列表',1026,'#','equipment','device/equipment/index',2,'device:equipment:list',NULL,3,0,1,1,'2023-02-20 23:22:03','2023-02-21 02:49:47',1);
INSERT INTO `sys_menu` VALUES (1036,'DRILLAMIN','刀具参数',1025,'#','config','material/config/index',2,'material:config:list',NULL,2,0,1,1,'2023-02-20 23:23:49','2023-03-28 05:30:47',1);
INSERT INTO `sys_menu` VALUES (1037,'DRILLAMIN','调度记录',1026,'#','schedulement','device/schedulement/index',2,'device:schedulement:edit',NULL,5,0,1,1,'2023-02-20 23:25:16','2023-06-09 09:41:36',1);
INSERT INTO `sys_menu` VALUES (1038,'DRILLAMIN','事件管理',1027,'#','event','alarm/event/index',2,'alarm:event:list',NULL,1,0,1,1,'2023-02-20 23:27:03','2023-02-23 22:04:19',1);
INSERT INTO `sys_menu` VALUES (1039,'DRILLAMIN','告警记录',1027,'#','warn','alarm/warn/index',2,'alarm:warn:list',NULL,3,0,1,1,'2023-02-20 23:28:17','2023-03-23 22:13:35',1);
INSERT INTO `sys_menu` VALUES (1043,'DRILLAMIN','修改',1033,NULL,'','#',3,'device:product:edit',NULL,3,0,1,1,'2023-02-21 20:32:06','2023-05-23 02:13:47',1);
INSERT INTO `sys_menu` VALUES (1044,'DRILLAMIN','删除',1033,NULL,NULL,'#',3,'device:product:del',NULL,4,0,1,1,'2023-02-21 20:32:58','2023-05-23 02:13:51',1);
INSERT INTO `sys_menu` VALUES (1045,'DRILLAMIN','查询',1033,NULL,NULL,'#',3,'device:product:list',NULL,1,0,1,1,'2023-02-21 20:33:18','2023-05-23 02:13:30',1);
INSERT INTO `sys_menu` VALUES (1046,'DRILLAMIN','查询',1034,NULL,NULL,'#',3,'produce:recipe:list',NULL,1,0,1,1,'2023-02-21 22:13:46','2023-05-23 02:10:45',1);
INSERT INTO `sys_menu` VALUES (1047,'DRILLAMIN','新增',1034,NULL,NULL,'#',3,'produce:recipe:add',NULL,2,0,1,1,'2023-02-21 22:14:27','2023-05-23 02:10:49',1);
INSERT INTO `sys_menu` VALUES (1048,'DRILLAMIN','修改',1034,NULL,NULL,'#',3,'produce:recipe:edit',NULL,3,0,1,1,'2023-02-21 22:15:18','2023-05-23 02:10:52',1);
INSERT INTO `sys_menu` VALUES (1049,'DRILLAMIN','删除',1034,NULL,NULL,'#',3,'produce:recipe:remove',NULL,4,0,1,1,'2023-02-21 22:15:58','2023-05-23 02:10:57',1);
INSERT INTO `sys_menu` VALUES (1052,'DRILLAMIN','重置',1033,NULL,NULL,'#',3,'device:product:reset',NULL,5,0,1,1,'2023-02-21 22:22:09','2023-05-23 02:13:56',1);
INSERT INTO `sys_menu` VALUES (1053,'DRILLAMIN','重置',1034,NULL,NULL,'#',3,'produce:recipe:reset',NULL,5,0,1,1,'2023-02-21 22:25:29','2023-05-23 02:11:01',1);
INSERT INTO `sys_menu` VALUES (1054,'DRILLAMIN','通知管理',0,'log','','#',1,NULL,NULL,6,0,1,1,'2023-02-22 01:16:01','2023-03-01 14:09:29',1);
INSERT INTO `sys_menu` VALUES (1055,'DRILLAMIN','通知设置',1054,'#','setting','notify/setting/index',2,'notify:setting:list',NULL,1,0,1,1,'2023-02-22 01:21:12','2023-03-23 02:15:26',1);
INSERT INTO `sys_menu` VALUES (1056,'DRILLAMIN','通知记录',1054,'#','notify','notify/record/index',2,'notify:record:list',NULL,2,0,1,1,'2023-02-22 01:23:31','2023-03-23 02:14:43',1);
INSERT INTO `sys_menu` VALUES (1057,'DRILLAMIN','查询',1035,NULL,NULL,'#',3,'device:equipment:list',NULL,1,0,1,1,'2023-02-22 02:27:33','2023-05-23 02:14:02',1);
INSERT INTO `sys_menu` VALUES (1058,'DRILLAMIN','新增',1035,NULL,NULL,'#',3,'device:equipment:add',NULL,2,0,1,1,'2023-02-22 02:30:35','2023-05-23 02:14:07',1);
INSERT INTO `sys_menu` VALUES (1059,'DRILLAMIN','修改',1035,NULL,NULL,'#',3,'device:equipment:edit',NULL,3,0,1,1,'2023-02-22 02:32:17','2023-05-23 02:14:12',1);
INSERT INTO `sys_menu` VALUES (1060,'DRILLAMIN','删除',1035,NULL,NULL,'#',3,'device:equipment:remove',NULL,4,0,1,1,'2023-02-22 02:33:41','2023-05-23 02:14:17',1);
INSERT INTO `sys_menu` VALUES (1061,'DRILLAMIN','查看',1035,NULL,NULL,'#',3,'device:equipment:view',NULL,5,0,1,1,'2023-02-22 02:35:07','2023-05-23 02:14:21',1);
INSERT INTO `sys_menu` VALUES (1062,'DRILLAMIN','查询',1036,NULL,NULL,'#',3,'material:config:list',NULL,1,0,1,1,'2023-02-22 02:36:27','2023-05-23 01:19:43',43);
INSERT INTO `sys_menu` VALUES (1063,'DRILLAMIN','新增',1036,NULL,NULL,'#',3,'material:config:add',NULL,2,0,1,1,'2023-02-22 02:37:31','2023-05-23 01:19:56',43);
INSERT INTO `sys_menu` VALUES (1064,'DRILLAMIN','修改',1036,NULL,NULL,'#',3,'material:config:edit',NULL,3,0,1,1,'2023-02-22 02:38:13','2023-05-23 01:20:07',43);
INSERT INTO `sys_menu` VALUES (1065,'DRILLAMIN','删除',1036,NULL,NULL,'#',3,'material:config:remove',NULL,4,0,1,1,'2023-02-22 02:38:57','2023-05-23 01:20:19',43);
INSERT INTO `sys_menu` VALUES (1066,'DRILLAMIN','重置',1036,NULL,NULL,'#',3,'material:config:reset',NULL,5,0,1,1,'2023-02-22 02:39:33','2023-05-23 01:20:30',43);
INSERT INTO `sys_menu` VALUES (1067,'DRILLAMIN','查询',1037,NULL,NULL,'#',3,'device:schedulement:list',NULL,1,0,1,1,'2023-02-22 02:40:25','2023-05-23 02:14:27',1);
INSERT INTO `sys_menu` VALUES (1068,'DRILLAMIN','新增',1037,NULL,NULL,'#',3,'device:schedulement:add',NULL,2,0,1,1,'2023-02-22 02:41:06','2023-05-23 02:14:32',1);
INSERT INTO `sys_menu` VALUES (1069,'DRILLAMIN','修改',1037,NULL,NULL,'#',3,'device:schedulement:edit',NULL,3,0,1,1,'2023-02-22 02:41:32','2023-05-23 02:14:36',1);
INSERT INTO `sys_menu` VALUES (1070,'DRILLAMIN','删除',1037,NULL,NULL,'#',3,'device:schedulement:delete',NULL,4,0,1,1,'2023-02-22 02:42:02','2023-05-23 02:14:40',1);
INSERT INTO `sys_menu` VALUES (1071,'DRILLAMIN','重置',1037,NULL,NULL,'#',3,'device:schedulement:reset',NULL,5,0,1,1,'2023-02-22 02:43:03','2023-05-23 02:14:43',1);
INSERT INTO `sys_menu` VALUES (1072,'DRILLAMIN','查询',1030,NULL,NULL,'#',3,'produce:board:list',NULL,1,0,1,1,'2023-02-22 04:19:39','2023-05-23 02:12:20',1);
INSERT INTO `sys_menu` VALUES (1073,'DRILLAMIN','新增',1030,NULL,NULL,'#',3,'produce:board:add',NULL,2,0,1,1,'2023-02-22 04:20:16','2023-05-23 02:12:26',1);
INSERT INTO `sys_menu` VALUES (1074,'DRILLAMIN','修改',1030,NULL,NULL,'#',3,'produce:board:edit',NULL,3,0,1,1,'2023-02-22 04:20:47','2023-05-23 02:12:33',1);
INSERT INTO `sys_menu` VALUES (1075,'DRILLAMIN','删除',1030,NULL,NULL,'#',3,'produce:board:remove',NULL,4,0,1,1,'2023-02-22 04:21:23','2023-05-23 02:12:42',1);
INSERT INTO `sys_menu` VALUES (1076,'DRILLAMIN','重置',1030,NULL,NULL,'#',3,'produce:board:reset',NULL,5,0,1,1,'2023-02-22 04:22:07','2023-05-23 02:12:48',1);
INSERT INTO `sys_menu` VALUES (1077,'DRILLAMIN','查询',1029,NULL,NULL,'#',3,'produce:task:list',NULL,1,0,1,1,'2023-02-22 04:23:27','2023-05-23 02:11:17',1);
INSERT INTO `sys_menu` VALUES (1078,'DRILLAMIN','新增',1029,NULL,NULL,'#',3,'produce:task:add',NULL,2,0,1,1,'2023-02-22 04:23:59','2023-05-23 02:11:21',1);
INSERT INTO `sys_menu` VALUES (1079,'DRILLAMIN','修改',1029,NULL,NULL,'#',3,'produce:task:edit',NULL,3,0,1,1,'2023-02-22 04:24:34','2023-05-23 02:11:25',1);
INSERT INTO `sys_menu` VALUES (1080,'DRILLAMIN','删除',1029,NULL,NULL,'#',3,'produce:task:remove',NULL,4,0,1,1,'2023-02-22 04:25:14','2023-05-23 02:11:29',1);
INSERT INTO `sys_menu` VALUES (1081,'DRILLAMIN','重置',1029,NULL,NULL,'#',3,'produce:task:reset',NULL,5,0,1,1,'2023-02-22 04:26:03','2023-05-23 02:11:33',1);
INSERT INTO `sys_menu` VALUES (1082,'DRILLAMIN','查询',1031,NULL,NULL,'#',3,'produce:drillTask:list',NULL,1,0,1,1,'2023-02-22 04:28:02','2023-06-12 14:25:19',1);
INSERT INTO `sys_menu` VALUES (1083,'DRILLAMIN','新增',1031,NULL,NULL,'#',3,'produce:drillTask:add',NULL,2,0,1,1,'2023-02-22 04:28:41','2023-06-12 14:25:32',1);
INSERT INTO `sys_menu` VALUES (1084,'DRILLAMIN','修改',1031,NULL,NULL,'#',3,'produce:drillTask:edit',NULL,3,0,1,1,'2023-02-22 04:29:10','2023-06-12 14:25:40',1);
INSERT INTO `sys_menu` VALUES (1085,'DRILLAMIN','删除',1031,NULL,NULL,'#',3,'produce:drillTask:remove',NULL,4,0,1,1,'2023-02-22 04:29:43','2023-06-12 14:25:47',1);
INSERT INTO `sys_menu` VALUES (1086,'DRILLAMIN','重置',1031,NULL,NULL,'#',3,'produce:drillTask:reset',NULL,5,0,1,1,'2023-02-22 04:30:12','2023-06-12 14:25:53',1);
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
INSERT INTO `sys_menu` VALUES (1128,'DRILLAMIN','工序管理',1024,'#','process','produce/process/index',2,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:37:07','2023-06-12 14:22:19',1);
INSERT INTO `sys_menu` VALUES (1129,'DRILLAMIN','查询',1128,'#',NULL,'#',3,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:42:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1130,'DRILLAMIN','新增',1128,'#',NULL,'#',3,'produce:process:add',NULL,2,0,1,1,'2023-03-01 11:43:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1131,'DRILLAMIN','修改',1128,'#',NULL,'#',3,'produce:process:edit',NULL,3,0,1,1,'2023-03-01 11:43:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1132,'DRILLAMIN','删除',1128,'#',NULL,'#',3,'produce:process:remove',NULL,4,0,1,1,'2023-03-01 11:43:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1133,'DRILLAMIN','重置',1128,'#',NULL,'#',3,'produce:process:reset',NULL,5,0,1,1,'2023-03-01 11:44:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1134,'DRILLAMIN','字典管理',1,'dict','dict','system/dict/index',2,'system:dict:list',NULL,3,0,0,1,'2023-03-01 13:53:42','2023-04-04 09:40:08',1);
INSERT INTO `sys_menu` VALUES (1136,'DRILLAMIN','告警设置',1027,'#','setting','alarm/setting/index',2,'alarm:setting:index',NULL,2,0,1,1,'2023-03-23 22:13:23','2023-03-23 22:13:48',1);
INSERT INTO `sys_menu` VALUES (1137,'DRILLAMIN','查询',1136,'#',NULL,'#',3,'alarm:setting:list',NULL,1,0,1,1,'2023-03-23 22:15:45','2023-05-23 02:15:33',1);
INSERT INTO `sys_menu` VALUES (1138,'DRILLAMIN','新增',1136,'#',NULL,'#',3,'alarm:setting:add',NULL,2,0,1,1,'2023-03-23 22:16:21','2023-05-23 02:15:40',1);
INSERT INTO `sys_menu` VALUES (1139,'DRILLAMIN','修改',1136,'#',NULL,'#',3,'alarm:setting:edit',NULL,3,0,1,1,'2023-03-23 22:16:59','2023-05-23 02:15:45',1);
INSERT INTO `sys_menu` VALUES (1140,'DRILLAMIN','删除',1136,'#',NULL,'#',3,'alarm:setting:remove',NULL,4,0,1,1,'2023-03-23 22:17:26','2023-05-23 02:15:49',1);
INSERT INTO `sys_menu` VALUES (1147,'DRILLAMIN','排刀计划',1024,'#','drillCutterPlan','produce/drillCutterPlan/index',1,'produce:drillCutterPlan:list',NULL,5,0,0,1,'2023-03-28 04:56:52','2023-06-12 14:22:35',1);
INSERT INTO `sys_menu` VALUES (1148,'DRILLAMIN','主数据',0,'component','masterData','#',1,NULL,NULL,1,0,1,1,'2023-03-29 22:00:59','2023-04-03 22:07:28',1);
INSERT INTO `sys_menu` VALUES (1149,'DRILLAMIN','计量单位',1148,'','unitMeasure','masterData/unitMeasure/index',2,'masterData:unitMeasure:list',NULL,1,0,1,1,'2023-03-29 22:05:55','2023-05-08 05:21:07',1);
INSERT INTO `sys_menu` VALUES (1150,'DRILLAMIN','客户管理',1148,'','client','masterData/client/index',2,'masterData:client:list',NULL,50,0,1,1,'2023-03-29 22:07:26','2023-05-08 05:21:20',1);
INSERT INTO `sys_menu` VALUES (1151,'DRILLAMIN','供应商管理',1148,'','vendor','masterData/vendor/index',2,'masterData:vendor:list',NULL,60,0,1,1,'2023-03-29 22:09:03','2023-05-08 05:21:23',1);
INSERT INTO `sys_menu` VALUES (1152,'DRILLAMIN','车间管理',1148,'','workshop','masterData/workShop/index',2,'masterData:workshop:list',NULL,40,0,1,1,'2023-03-29 22:10:20','2023-05-08 05:21:16',1);
INSERT INTO `sys_menu` VALUES (1153,'DRILLAMIN','工作站',1148,'','workstation','masterData/workStation/index',2,'masterData:workstation:index',NULL,41,0,1,1,'2023-03-29 22:11:24','2023-05-08 05:21:18',1);
INSERT INTO `sys_menu` VALUES (1154,'DRILLAMIN','物料产品分类',1148,'','itemType','masterData/itemType/index',2,'masterData:itemType:list',NULL,10,0,1,1,'2023-03-29 22:12:21','2023-05-08 05:21:12',1);
INSERT INTO `sys_menu` VALUES (1155,'DRILLAMIN','物料产品管理',1148,'','item','masterData/item/index',2,'masterData:item:list',NULL,11,0,1,1,'2023-03-29 22:13:20','2023-05-08 05:21:14',1);
INSERT INTO `sys_menu` VALUES (1156,'DRILLAMIN','产品大类',1148,'','productCategory','masterData/productCategory/index',2,'masterData:productCategory:list',NULL,5,0,1,1,'2023-04-01 02:41:29','2023-05-08 21:42:17',1);
INSERT INTO `sys_menu` VALUES (1157,'DRILLAMIN','工艺流程',1024,'#','route','produce/route/index',2,'produce:route:list',NULL,2,0,1,1,'2023-04-01 02:46:56','2023-06-12 14:22:23',1);
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'tree','wm','#',1,NULL,NULL,50,0,1,1,'2023-04-01 02:50:34','2023-04-19 02:36:29',1);
INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','仓库设置',1158,'#','warehouse','wareHouse/wareHouse/index',2,'warehouse:warehouse:list',NULL,1,0,1,1,'2023-04-01 02:53:42','2023-04-18 03:33:51',1);
INSERT INTO `sys_menu` VALUES (1160,'DRILLAMIN','库存现有量',1158,'#','materialStock','wareHouse/materialStock/index',2,'warehouse:materialStock:list',NULL,2,0,1,1,'2023-04-01 02:54:57','2023-05-11 04:28:18',1);
INSERT INTO `sys_menu` VALUES (1161,'DRILLAMIN','钻带参数',1025,'#','drillFile','material/drillFile/index',2,'material:drillFile:list',NULL,3,0,1,1,'2023-04-01 03:20:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1162,'DRILLAMIN','排刀文件',1025,'#','atpFile','material/atpFile/index',2,'material:atpFile:list',NULL,4,0,1,1,'2023-04-01 03:21:21','2023-04-10 23:34:21',1);
INSERT INTO `sys_menu` VALUES (1165,'DRILLAMIN','报工记录',1024,'#','reportrecords','produce/reportRecords/index',2,'produce:reportRecords:list',NULL,8,0,1,1,'2023-04-04 00:47:50','2023-06-12 14:22:58',1);
INSERT INTO `sys_menu` VALUES (1166,'DRILLAMIN','查询',1165,'#',NULL,'#',3,'produce:feedback:list',NULL,1,0,1,1,'2023-04-04 00:50:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1167,'DRILLAMIN','新增',1165,'#',NULL,'#',3,'produce:feedback:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1168,'DRILLAMIN','修改',1165,'#',NULL,'#',3,'produce:feedback:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1169,'DRILLAMIN','删除',1165,'#',NULL,'#',3,'produce:feedback:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1170,'DRILLAMIN','重置',1165,'#',NULL,'#',3,'produce:feedback:reset',NULL,5,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
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
INSERT INTO `sys_menu` VALUES (1279,'DRILLAMIN','提交',1122,'#',NULL,'#',3,'produce:workorder:commit',NULL,6,0,1,1,'2023-04-05 22:19:41','2023-04-05 23:37:39',33);
INSERT INTO `sys_menu` VALUES (1280,'DRILLAMIN','提交',1029,'#',NULL,'#',3,'produce:task:commit',NULL,6,0,1,33,'2023-04-05 22:53:40','2023-05-23 02:11:38',1);
INSERT INTO `sys_menu` VALUES (1281,'DRILLAMIN','提交',1165,'#',NULL,'#',3,'produce:feedback:commit',NULL,6,0,1,33,'2023-04-05 23:38:44',NULL,NULL);
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
INSERT INTO `sys_menu` VALUES (1309,'DRILLAMIN','查看',1122,'#',NULL,'#',3,'produce:workorder:view',NULL,7,0,1,33,'2023-04-10 04:22:28',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1310,'DRILLAMIN','查看',1029,'#',NULL,'#',3,'produce:task:view',NULL,7,0,1,33,'2023-04-10 04:23:37','2023-05-23 02:11:42',1);
INSERT INTO `sys_menu` VALUES (1311,'DRILLAMIN','查看',1031,'#',NULL,'#',3,'produce:drillTask:view',NULL,6,0,1,33,'2023-04-10 04:25:02','2023-06-12 14:25:59',1);
INSERT INTO `sys_menu` VALUES (1312,'DRILLAMIN','查看',1165,'#',NULL,'#',3,'produce:feedback:view',NULL,7,0,1,33,'2023-04-10 04:25:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1313,'DRILLAMIN','查看',1030,'#',NULL,'#',3,'produce:board:view',NULL,6,0,1,33,'2023-04-10 04:28:13','2023-05-23 02:12:52',1);
INSERT INTO `sys_menu` VALUES (1314,'DRILLAMIN','检验记录',1024,'#','checkrecords','produce/checkRecords/index',2,'produce:checkrecords:list',NULL,11,0,1,1,'2023-04-14 02:34:00','2023-06-12 14:23:10',1);
INSERT INTO `sys_menu` VALUES (1315,'DRILLAMIN','查询',1314,'#',NULL,'#',3,'produce:checkrecords:list',NULL,1,0,1,1,'2023-05-06 01:00:46','2023-05-23 02:13:01',1);
INSERT INTO `sys_menu` VALUES (1317,'DRILLAMIN','设备维护',1026,'#','repair','device/repair/index',1,'device:repair:list',NULL,6,0,1,1,'2023-05-06 03:11:39','2023-05-16 02:48:18',1);
INSERT INTO `sys_menu` VALUES (1319,'DRILLAMIN','点检项目',1026,'#','subject','device/subject/index',1,'device:subject:list',NULL,5,0,1,1,'2023-05-16 02:46:43','2023-05-16 02:47:33',1);
INSERT INTO `sys_menu` VALUES (1320,'DRILLAMIN','编码规则',1148,'','autocodeRule','masterData/autocodeRule/index',1,'masterData:autocodeRule:list',NULL,1,0,1,1,'2023-05-17 02:29:46','2023-05-17 02:41:36',1);
INSERT INTO `sys_menu` VALUES (1321,'DRILLAMIN','导出',1149,'#',NULL,'#',3,'masterData:unitMeasure:export',NULL,1,0,1,1,'2023-05-21 22:53:13','2023-05-21 22:53:58',1);
INSERT INTO `sys_menu` VALUES (1322,'DRILLAMIN','查询',1320,'#',NULL,'#',3,'masterData:encodeBuildRules:list',NULL,1,0,1,1,'2023-05-22 04:07:44','2023-05-22 21:55:42',1);
INSERT INTO `sys_menu` VALUES (1323,'DRILLAMIN','修改',1320,'#',NULL,'#',3,'masterData:autocodeRule:edit',NULL,1,0,1,1,'2023-05-22 04:08:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1324,'DRILLAMIN','导入',1149,'#',NULL,'#',3,'masterData:unitMeasure:import',NULL,1,0,1,43,'2023-05-22 22:58:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1325,'DRILLAMIN','导入',1154,'#',NULL,'#',3,'masterData:itemType:import',NULL,1,0,1,43,'2023-05-22 23:02:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1326,'DRILLAMIN','导入',1153,'#',NULL,'#',3,'masterData:workstation:import',NULL,1,0,1,43,'2023-05-22 23:06:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1327,'DRILLAMIN','导入',1150,'#',NULL,'#',3,'masterData:client:import',NULL,1,0,1,43,'2023-05-22 23:08:11',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1328,'DRILLAMIN','导入',1151,'#',NULL,'#',3,'masterData:vendor:import',NULL,1,0,1,43,'2023-05-22 23:09:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1329,'DRILLAMIN','导入',1128,'#',NULL,'#',3,'produce:process:import',NULL,1,0,1,43,'2023-05-22 23:13:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1330,'DRILLAMIN','导入',1122,'#',NULL,'#',3,'produce:workorder:import',NULL,1,0,1,43,'2023-05-22 23:17:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1331,'DRILLAMIN','导出',1165,'#',NULL,'#',3,'produce:feedback:export',NULL,1,0,1,43,'2023-05-22 23:20:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1332,'DRILLAMIN','导出',1030,'#',NULL,'#',3,'produce:board:export',NULL,1,0,1,43,'2023-05-22 23:21:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1333,'DRILLAMIN','查看',1314,'#',NULL,'#',3,'produce:checkrecords:view',NULL,1,0,1,43,'2023-05-22 23:22:44',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1334,'DRILLAMIN','编辑',1314,'#',NULL,'#',3,'produce:checkrecords:edit',NULL,1,0,1,43,'2023-05-22 23:23:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1335,'DRILLAMIN','导出',1038,'#',NULL,'#',3,'alarm:event:export',NULL,1,0,1,43,'2023-05-22 23:35:43','2023-05-23 02:15:06',1);
INSERT INTO `sys_menu` VALUES (1336,'DRILLAMIN','导出',1039,'#',NULL,'#',3,'alarm:warn:export',NULL,1,0,1,43,'2023-05-22 23:37:48','2023-05-23 02:16:00',1);
INSERT INTO `sys_menu` VALUES (1337,'DRILLAMIN','导入',1156,'#',NULL,'#',3,'masterData:productCategory:import',NULL,1,0,1,43,'2023-05-23 01:10:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1338,'DRILLAMIN','导入',1155,'#',NULL,'#',3,'masterData:item:import',NULL,1,0,1,43,'2023-05-23 01:11:55',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1339,'DRILLAMIN','导出',1031,'#',NULL,'#',3,'produce:drillTask:export',NULL,1,0,1,43,'2023-05-23 01:13:41','2023-06-12 14:25:26',1);
INSERT INTO `sys_menu` VALUES (1340,'DRILLAMIN','导出',1314,'#',NULL,'#',3,'produce:checkrecords:export',NULL,1,0,1,43,'2023-05-23 01:15:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1341,'DRILLAMIN','导入',1036,'#',NULL,'#',3,'material:config:import',NULL,1,0,1,43,'2023-05-23 01:16:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1342,'DRILLAMIN','导入',1161,'#',NULL,'#',3,'material:drillFile:import',NULL,1,0,1,43,'2023-05-23 01:17:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1343,'DRILLAMIN','查看',1036,'#',NULL,'#',3,'material:config:view',NULL,1,0,1,43,'2023-05-23 01:21:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1344,'DRILLAMIN','查询',1317,'#',NULL,'#',3,'device:repair:list',NULL,1,0,1,43,'2023-05-23 01:23:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1345,'DRILLAMIN','新增',1317,'#',NULL,'#',3,'device:repair:add',NULL,1,0,1,43,'2023-05-23 01:23:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1346,'DRILLAMIN','删除',1317,'#',NULL,'#',3,'device:repair:remove',NULL,1,0,1,43,'2023-05-23 01:23:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1347,'DRILLAMIN','编辑',1317,'#',NULL,'#',3,'device:repair:edit',NULL,1,0,1,43,'2023-05-23 01:24:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1348,'DRILLAMIN','导出',1317,'#',NULL,'#',3,'device:repair:export',NULL,1,0,1,43,'2023-05-23 01:24:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1349,'DRILLAMIN','导出',1056,'#',NULL,'#',3,'notify:record:export',NULL,1,0,1,43,'2023-05-23 01:25:40','2023-05-23 01:26:26',43);
INSERT INTO `sys_menu` VALUES (1350,'DRILLAMIN','生产报工',1024,'#','feedback','produce/feedback/index',2,'produce:feedback:list',NULL,7,0,1,1,'2023-06-12 11:24:59','2023-06-12 14:22:50',1);
INSERT INTO `sys_menu` VALUES (1351,'DRILLAMIN','查看',1350,'#',NULL,'#',3,'produce:feedback:view',NULL,1,0,1,1,'2023-06-12 05:23:38',NULL,NULL);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu_auth`
--

LOCK TABLES `sys_menu_auth` WRITE;
/*!40000 ALTER TABLE `sys_menu_auth` DISABLE KEYS */;
INSERT INTO `sys_menu_auth` VALUES (1924,1,6,1,33,'2023-04-18 02:02:51');
INSERT INTO `sys_menu_auth` VALUES (1925,2,6,1,33,'2023-04-18 02:02:51');
INSERT INTO `sys_menu_auth` VALUES (1926,1000,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1927,1001,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1928,1002,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1929,1003,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1930,1004,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1931,1005,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1932,1006,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1933,3,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1934,1007,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1935,1008,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1936,1009,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1937,1010,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1938,1011,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1939,4,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1940,1012,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1941,1013,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1942,1014,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1943,1015,6,1,33,'2023-04-18 02:02:52');
INSERT INTO `sys_menu_auth` VALUES (1944,5,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1945,1016,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1946,1017,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1947,1018,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1948,1019,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1949,6,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1950,1020,6,1,33,'2023-04-18 02:02:53');
INSERT INTO `sys_menu_auth` VALUES (1951,1021,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1952,1022,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1953,1023,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1954,1024,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1955,1029,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1956,1077,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1957,1078,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1958,1079,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1959,1080,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1960,1081,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1961,1280,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1962,1310,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1963,1030,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1964,1072,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1965,1073,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1966,1074,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1967,1075,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1968,1076,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1969,1313,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1970,1122,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1971,1123,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1972,1124,6,1,33,'2023-04-18 02:02:54');
INSERT INTO `sys_menu_auth` VALUES (1973,1125,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1974,1126,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1975,1127,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1976,1279,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1977,1309,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1978,1128,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1979,1129,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1980,1130,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1981,1131,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1982,1132,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1983,1133,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1984,1307,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1985,1157,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1986,1219,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1987,1220,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1988,1221,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1989,1222,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1990,1223,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1991,1224,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1992,1292,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1993,1293,6,1,33,'2023-04-18 02:02:55');
INSERT INTO `sys_menu_auth` VALUES (1994,1294,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (1995,1295,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (1996,1296,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (1997,1297,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (1998,1298,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (1999,1299,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2000,1300,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2002,1301,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2004,1302,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2006,1303,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2008,1304,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2010,1305,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2012,1306,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2014,1165,6,1,33,'2023-04-18 02:02:56');
INSERT INTO `sys_menu_auth` VALUES (2016,1166,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2018,1167,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2020,1168,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2022,1169,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2024,1170,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2026,1281,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2028,1312,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2030,1314,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2032,1025,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2034,1032,6,1,33,'2023-04-18 02:02:57');
INSERT INTO `sys_menu_auth` VALUES (2036,1087,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2038,1088,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2040,1089,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2042,1090,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2044,1091,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2046,1036,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2048,1062,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2050,1063,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2052,1064,6,1,33,'2023-04-18 02:02:58');
INSERT INTO `sys_menu_auth` VALUES (2054,1065,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2056,1066,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2058,1161,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2060,1171,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2062,1172,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2064,1173,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2066,1174,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2068,1175,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2070,1176,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2072,1162,6,1,33,'2023-04-18 02:02:59');
INSERT INTO `sys_menu_auth` VALUES (2074,1177,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2076,1178,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2078,1179,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2080,1180,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2082,1181,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2084,1182,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2086,1282,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2088,1283,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2090,1284,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2092,1285,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2094,1286,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2096,1026,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2098,1033,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2100,1043,6,1,33,'2023-04-18 02:03:00');
INSERT INTO `sys_menu_auth` VALUES (2102,1044,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2104,1045,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2106,1052,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2108,1035,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2110,1057,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2112,1058,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2115,1059,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2118,1060,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2121,1061,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2124,1027,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2127,1038,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2130,1097,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2134,1098,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2138,1099,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2142,1100,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2146,1101,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2151,1039,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2156,1102,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2161,1103,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2166,1104,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2171,1105,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2176,1106,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2181,1107,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2186,1136,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2191,1137,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2196,1138,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2201,1139,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2206,1140,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2211,1054,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2216,1055,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2221,1108,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2226,1109,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2231,1110,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2237,1111,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2243,1112,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2249,1056,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2255,1113,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2261,1114,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2267,1115,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2274,1116,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2282,1117,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2288,1148,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2295,1149,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2303,1231,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2309,1232,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2317,1233,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2323,1234,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2331,1235,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2337,1236,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2345,1150,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2351,1237,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2359,1238,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2365,1239,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2372,1240,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2379,1241,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2387,1242,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2393,1151,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2400,1243,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2407,1244,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2414,1245,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2422,1246,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2429,1247,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2436,1248,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2441,1152,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2451,1249,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2457,1250,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2468,1251,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2476,1252,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2482,1253,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2492,1254,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2498,1153,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2506,1255,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2515,1256,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2522,1257,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2529,1258,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2540,1259,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2546,1260,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2553,1154,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2564,1261,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2570,1262,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2577,1263,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2587,1264,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2595,1265,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2603,1266,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2609,1155,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2619,1267,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2627,1268,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2634,1269,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2642,1270,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2650,1271,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2659,1272,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2666,1287,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2673,1288,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2684,1289,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2690,1290,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2697,1291,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2705,1156,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2716,1273,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2721,1274,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2731,1275,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2737,1276,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2748,1277,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2754,1278,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2762,1158,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2771,1159,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2777,1207,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2788,1208,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2793,1209,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2806,1210,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2815,1211,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2820,1212,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2831,1160,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2838,1213,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2849,1214,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2861,1215,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2872,1216,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2877,1217,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2892,1218,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (6779,1,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6780,2,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6781,1000,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6782,1001,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6783,1002,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6784,1003,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6785,1004,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6786,1005,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6787,1006,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6788,3,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6789,1007,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6790,1008,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6791,1009,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6792,1010,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6793,1011,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6794,4,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6795,1012,3,1,1,'2023-05-19 01:43:25');
INSERT INTO `sys_menu_auth` VALUES (6796,1013,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6797,1014,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6798,1015,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6799,5,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6800,1016,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6801,1017,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6802,1018,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6803,1019,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6804,6,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6805,1020,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6806,1021,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6807,1022,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6808,1023,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6809,1024,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6810,1029,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6811,1077,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6812,1078,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6813,1079,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6814,1080,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6815,1081,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6816,1280,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6817,1310,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6818,1030,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6819,1072,3,1,1,'2023-05-19 01:43:26');
INSERT INTO `sys_menu_auth` VALUES (6820,1073,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6821,1074,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6822,1075,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6823,1076,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6824,1313,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6825,1031,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6826,1082,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6827,1083,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6828,1084,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6829,1085,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6830,1086,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6831,1311,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6832,1122,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6833,1123,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6834,1124,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6835,1125,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6836,1126,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6837,1127,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6838,1279,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6839,1309,3,1,1,'2023-05-19 01:43:27');
INSERT INTO `sys_menu_auth` VALUES (6840,1128,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6841,1129,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6842,1130,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6843,1131,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6844,1132,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6845,1133,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6846,1307,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6847,1157,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6848,1219,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6849,1220,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6850,1221,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6851,1222,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6852,1223,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6853,1224,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6854,1292,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6855,1293,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6856,1294,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6857,1295,3,1,1,'2023-05-19 01:43:28');
INSERT INTO `sys_menu_auth` VALUES (6858,1296,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6859,1297,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6860,1298,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6861,1299,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6862,1300,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6863,1301,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6864,1302,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6865,1303,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6866,1304,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6867,1305,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6868,1306,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6869,1165,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6870,1166,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6871,1167,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6872,1168,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6873,1169,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6874,1170,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6875,1281,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6876,1312,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6877,1314,3,1,1,'2023-05-19 01:43:29');
INSERT INTO `sys_menu_auth` VALUES (6878,1315,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6879,1025,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6880,1036,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6881,1062,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6882,1063,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6883,1064,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6884,1065,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6885,1066,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6886,1161,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6887,1171,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6888,1172,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6889,1173,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6890,1174,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6891,1175,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6892,1176,3,1,1,'2023-05-19 01:43:30');
INSERT INTO `sys_menu_auth` VALUES (6893,1162,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6894,1177,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6895,1178,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6896,1179,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6897,1180,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6898,1181,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6899,1182,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6900,1282,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6901,1283,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6902,1284,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6903,1285,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6904,1286,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6905,1026,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6906,1033,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6907,1043,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6908,1044,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6909,1045,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6910,1052,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6911,1035,3,1,1,'2023-05-19 01:43:31');
INSERT INTO `sys_menu_auth` VALUES (6912,1057,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6913,1058,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6914,1059,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6915,1060,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6916,1061,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6917,1317,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6918,1319,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6919,1027,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6920,1038,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6921,1097,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6922,1098,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6923,1099,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6924,1100,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6925,1101,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6926,1039,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6927,1102,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6928,1103,3,1,1,'2023-05-19 01:43:32');
INSERT INTO `sys_menu_auth` VALUES (6929,1104,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6930,1105,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6931,1106,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6932,1107,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6933,1136,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6934,1137,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6935,1138,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6936,1139,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6937,1140,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6938,1054,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6939,1055,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6940,1108,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6941,1109,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6942,1110,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6943,1111,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6944,1112,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6945,1056,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6946,1113,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6947,1114,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6948,1115,3,1,1,'2023-05-19 01:43:33');
INSERT INTO `sys_menu_auth` VALUES (6949,1116,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6950,1117,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6951,1148,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6952,1149,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6953,1231,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6954,1232,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6955,1233,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6956,1234,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6957,1235,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6958,1236,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6959,1150,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6960,1237,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6961,1238,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6962,1239,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6963,1240,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6964,1241,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6965,1242,3,1,1,'2023-05-19 01:43:34');
INSERT INTO `sys_menu_auth` VALUES (6966,1151,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6967,1243,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6968,1244,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6969,1245,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6970,1246,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6971,1247,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6972,1248,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6973,1152,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6974,1249,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6975,1250,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6976,1251,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6977,1252,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6978,1253,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6979,1254,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6980,1153,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6981,1255,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6982,1256,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6983,1257,3,1,1,'2023-05-19 01:43:35');
INSERT INTO `sys_menu_auth` VALUES (6984,1258,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6985,1259,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6986,1260,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6987,1154,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6988,1261,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6989,1262,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6990,1263,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6991,1264,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6992,1265,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6993,1266,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6994,1155,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6995,1267,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6996,1268,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6997,1269,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6998,1270,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (6999,1271,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (7000,1272,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (7001,1287,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (7002,1288,3,1,1,'2023-05-19 01:43:36');
INSERT INTO `sys_menu_auth` VALUES (7003,1289,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7004,1290,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7005,1291,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7006,1156,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7007,1273,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7008,1274,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7009,1275,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7010,1276,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7011,1277,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7012,1278,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7013,1320,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7014,1158,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7015,1159,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7016,1207,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7017,1208,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7018,1209,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7019,1210,3,1,1,'2023-05-19 01:43:37');
INSERT INTO `sys_menu_auth` VALUES (7020,1211,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7021,1212,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7022,1160,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7023,1213,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7024,1214,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7025,1215,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7026,1216,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7027,1217,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7028,1218,3,1,1,'2023-05-19 01:43:38');
INSERT INTO `sys_menu_auth` VALUES (7916,1,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7917,1024,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7918,1029,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7919,1030,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7920,1122,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7921,1128,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7922,1157,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7923,1165,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7924,1025,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7925,1162,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7926,1026,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7927,1148,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7928,1155,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7929,2,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7930,1000,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7931,1001,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7932,1002,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7933,1003,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7934,1004,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7935,1005,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7936,1006,2,1,1,'2023-05-19 02:33:21');
INSERT INTO `sys_menu_auth` VALUES (7937,5,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7938,1016,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7939,1017,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7940,1018,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7941,1019,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7942,6,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7943,1020,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7944,1021,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7945,1022,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7946,1023,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7947,1077,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7948,1078,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7949,1079,2,1,1,'2023-05-19 02:33:22');
INSERT INTO `sys_menu_auth` VALUES (7950,1080,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7951,1081,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7952,1072,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7953,1073,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7954,1074,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7955,1075,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7956,1076,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7957,1123,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7958,1124,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7959,1125,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7960,1126,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7961,1127,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7962,1129,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7963,1130,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7964,1131,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7965,1132,2,1,1,'2023-05-19 02:33:23');
INSERT INTO `sys_menu_auth` VALUES (7966,1133,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7967,1219,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7968,1220,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7969,1221,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7970,1222,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7971,1223,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7972,1224,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7973,1166,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7974,1167,2,1,1,'2023-05-19 02:33:24');
INSERT INTO `sys_menu_auth` VALUES (7975,1168,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7976,1169,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7977,1170,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7978,1036,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7979,1062,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7980,1063,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7981,1064,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7982,1065,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7983,1066,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7984,1161,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7985,1171,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7986,1172,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7987,1173,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7988,1174,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7989,1175,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7990,1176,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7991,1177,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7992,1178,2,1,1,'2023-05-19 02:33:25');
INSERT INTO `sys_menu_auth` VALUES (7993,1179,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (7994,1180,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (7995,1181,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (7996,1182,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (7997,1033,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (7998,1043,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (7999,1044,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8000,1045,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8001,1052,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8002,1035,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8003,1057,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8004,1058,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8005,1059,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8006,1060,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8007,1061,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8008,1027,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8009,1038,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8010,1097,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8011,1098,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8012,1099,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8013,1100,2,1,1,'2023-05-19 02:33:26');
INSERT INTO `sys_menu_auth` VALUES (8014,1101,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8015,1039,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8016,1102,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8017,1103,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8018,1104,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8019,1105,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8020,1106,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8021,1107,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8022,1136,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8023,1137,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8024,1138,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8025,1139,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8026,1140,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8027,1054,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8028,1055,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8029,1108,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8030,1109,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8031,1110,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8032,1111,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8033,1112,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8034,1056,2,1,1,'2023-05-19 02:33:27');
INSERT INTO `sys_menu_auth` VALUES (8035,1113,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8036,1114,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8037,1115,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8038,1116,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8039,1117,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8040,1149,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8041,1231,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8042,1232,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8043,1233,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8044,1234,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8045,1235,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8046,1236,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8047,1150,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8048,1237,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8049,1238,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8050,1239,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8051,1240,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8052,1241,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8053,1242,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8054,1151,2,1,1,'2023-05-19 02:33:28');
INSERT INTO `sys_menu_auth` VALUES (8055,1243,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8056,1244,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8057,1245,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8058,1246,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8059,1247,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8060,1248,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8061,1152,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8062,1249,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8063,1250,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8064,1251,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8065,1252,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8066,1253,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8067,1254,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8068,1153,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8069,1255,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8070,1256,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8071,1257,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8072,1258,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8073,1259,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8074,1260,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8075,1154,2,1,1,'2023-05-19 02:33:29');
INSERT INTO `sys_menu_auth` VALUES (8076,1261,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8077,1262,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8078,1263,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8079,1264,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8080,1265,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8081,1266,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8082,1267,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8083,1268,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8084,1269,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8085,1270,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8086,1271,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8087,1272,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8088,1156,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8089,1273,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8090,1274,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8091,1275,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8092,1276,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8093,1277,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8094,1278,2,1,1,'2023-05-19 02:33:30');
INSERT INTO `sys_menu_auth` VALUES (8095,1158,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8096,1159,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8097,1207,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8098,1208,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8099,1209,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8100,1210,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8101,1211,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8102,1212,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8103,1160,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8104,1213,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8105,1214,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8106,1215,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8107,1216,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8108,1217,2,1,1,'2023-05-19 02:33:31');
INSERT INTO `sys_menu_auth` VALUES (8109,1218,2,1,1,'2023-05-19 02:33:31');
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
INSERT INTO `sys_menu_auth` VALUES (16465,1,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16466,2,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16467,3,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16468,4,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16469,5,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16470,6,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16471,1024,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16472,1029,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16473,1030,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16474,1031,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16475,1122,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16476,1128,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16477,1157,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16478,1165,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16479,1314,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16480,1025,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16481,1036,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16482,1161,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16483,1162,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16484,1026,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16485,1035,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16486,1317,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16487,1027,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16488,1038,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16489,1039,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16490,1136,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16491,1054,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16492,1055,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16493,1056,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16494,1000,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16495,1004,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16496,1005,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16497,1007,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16498,1011,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16499,1012,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16500,1016,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16501,1020,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16502,1077,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16503,1310,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16504,1072,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16505,1313,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16506,1332,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16507,1082,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16508,1311,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16509,1339,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16510,1123,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16511,1309,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16512,1330,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16513,1129,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16514,1307,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16515,1329,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16516,1219,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16517,1224,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16518,1166,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16519,1312,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16520,1331,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16521,1315,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16522,1333,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16523,1340,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16524,1062,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16525,1341,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16526,1343,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16527,1171,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16528,1176,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16529,1342,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16530,1177,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16531,1182,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16532,1057,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16533,1061,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16534,1344,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16535,1348,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16536,1319,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16537,1097,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16538,1335,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16539,1102,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16540,1107,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16541,1336,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16542,1137,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16543,1108,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16544,1113,8,1,1,'2023-06-06 16:30:02');
INSERT INTO `sys_menu_auth` VALUES (16545,1349,8,1,1,'2023-06-06 16:30:02');
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_position`
--

LOCK TABLES `sys_position` WRITE;
/*!40000 ALTER TABLE `sys_position` DISABLE KEYS */;
INSERT INTO `sys_position` VALUES (1,'董事长',NULL,5,0,1,NULL,'2022-11-08 13:56:35','2023-03-24 02:49:54',1);
INSERT INTO `sys_position` VALUES (2,'项目经理',NULL,5,0,1,NULL,'2022-11-08 13:56:56','2023-06-06 16:32:10',1);
INSERT INTO `sys_position` VALUES (3,'人力资源',NULL,3,0,1,NULL,'2022-11-08 13:57:24',NULL,NULL);
INSERT INTO `sys_position` VALUES (4,'普通员工','111',4,0,1,NULL,'2022-11-08 13:57:48','2022-11-16 16:27:21',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role`
--

LOCK TABLES `sys_role` WRITE;
/*!40000 ALTER TABLE `sys_role` DISABLE KEYS */;
INSERT INTO `sys_role` VALUES (1,'超级管理员',1,0,1,0,'2022-11-08 11:26:24',NULL,NULL);
INSERT INTO `sys_role` VALUES (2,'车间管理',3,0,1,0,'2022-11-17 08:58:18','2023-05-19 02:33:20',1);
INSERT INTO `sys_role` VALUES (3,'生产报工',4,0,1,1,'2023-02-23 02:01:07','2023-05-19 01:43:24',1);
INSERT INTO `sys_role` VALUES (6,'质检组长',4,0,1,33,'2023-04-18 02:02:51',NULL,NULL);
INSERT INTO `sys_role` VALUES (8,'质检员',8,0,0,33,'2023-04-18 02:02:57','2023-06-06 16:30:01',1);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (1,'admin','5794620ce8325bc4e7b67677da3daeaa','c5fc028a23cd41c696a004f9dba65bff','ADMIN',1,NULL,0,NULL,NULL,'13400000001','13400000001@qq.com',NULL,NULL,1,NULL,NULL,0,1,NULL,'2022-09-27 16:07:39','2023-04-19 20:53:20',1);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (4,1,1,1,'2022-12-26 17:17:36');
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
  `alarm_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '告警编号',
  `alarm_name` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '告警名称',
  `alarm_level` tinyint(4) DEFAULT NULL COMMENT '告警级别（1普通、2严重、3紧急）',
  `device_id` int(11) DEFAULT NULL COMMENT '告警设备ID',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `event_id` int(11) NOT NULL COMMENT '事件Id',
  `event_data` varchar(2000) CHARACTER SET utf8 DEFAULT NULL COMMENT '事件数据',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
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
INSERT INTO `t_alarm` VALUES (50,'2023-04-28 18:22:35','9','拆板机发生异常',3,57,0,1,1,'2023-04-28 18:22:35',NULL,NULL,0,NULL);
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
  `code` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `alarm_desc` varchar(500) CHARACTER SET utf8 DEFAULT NULL COMMENT '描述',
  `alarm_level` tinyint(4) DEFAULT NULL COMMENT '事件级别（1普通、2严重、3紧急）',
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm_setting`
--

LOCK TABLES `t_alarm_setting` WRITE;
/*!40000 ALTER TABLE `t_alarm_setting` DISABLE KEYS */;
INSERT INTO `t_alarm_setting` VALUES (12,'AGV电量水平低于30%',0,'LOWER_THAN_30','AGV电量水平低于30%',1,0,0,1,'2023-02-27 16:26:42','2023-04-18 05:22:10',1,57,'DRILL_REQUEST_LOAD_RAW_MATERIAL','11','大屏告警','AGV电量水平低于30%','0');
INSERT INTO `t_alarm_setting` VALUES (14,'AGV电量水平低于20%',0,'LOWER_THAN_20','AGV电量水平低于20%',2,0,1,1,'2023-02-27 16:26:42','2023-04-18 05:22:00',1,61,'DRILL_REQUEST_LOAD_RAW_MATERIAL','11,22','大屏告警,钉钉',' AGV电量水平低于20%','0');
INSERT INTO `t_alarm_setting` VALUES (15,'AGV电量水平低于10%',0,'LOWER_THAN_10','AGV电量水平低于10%',3,0,1,1,'2023-02-27 16:26:42','2023-06-06 16:25:38',1,38,'AGV充电','21,22,11,12','微信,钉钉,大屏告警,短信','AGV电量水平低于10%','0');
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_client`
--

LOCK TABLES `t_client` WRITE;
/*!40000 ALTER TABLE `t_client` DISABLE KEYS */;
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  PRIMARY KEY (`id`) USING BTREE
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
INSERT INTO `t_cutter_config_master` VALUES (3,'主轴转速200KRPM钻孔','主轴转速200KRPM钻孔','1111','\\\\fileServer\\IF200001111\\IF200001111.dia',8,'p05','柔性电路板',0,1,1,'2023-03-28 16:38:31','2023-05-04 22:48:59',1);
INSERT INTO `t_cutter_config_master` VALUES (17,'11','11','11','11',74,'P02','双面板',0,1,1,'2023-06-13 22:02:56',NULL,NULL);
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
  `code` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
INSERT INTO `t_device` VALUES (11,'drill01','drill01',NULL,0,0,1,1,'2023-02-28 11:40:13','2023-04-20 22:29:22',1,7,NULL,NULL,NULL,212,NULL,NULL,NULL,NULL,'{\n        \"ProductId\": \"agv\",\n        \"DeviceId\": \"AGV01\",\n        \"DeviceName\": \"AGV#01\",\n        \"DeviceClazz\": \"VgAutoDrill.Fundation.Vendor.Agvsz.AGV2530\",\n        \"AutoMode\": true,\n        \"DebugMode\": false,\n        \"DataCollectingPerSeconds\": 30,\n        \"KeepingPlcConnectionPerSeconds\": 30,\n        \"DeviceKind\": \"Auxiliary\",\n        \"InputCapabilities\": [ \"EmptySiloBox\", \"Raw\" ],\n        \"OutputCapabilities\": [ \"PRE_DRILL_TRANSFER_AGV_OUTPUT_1\", \"EmptySiloBox\" ],\n        \"Extra\": {\n          \"modbusTcpUri\": \"http://192.168.3.20:502\",\n          \"modbusTcpSlaveId\": 1,\n          \"MoveTimeout\": 30000,\n          \"PostAndGetTimeout\": 10000,\n          \"LowBattery\": 30,\n          \"AGVMoveStart\": \"http://192.168.3.15:9502/api/wcstask/AddTask\",\n          \"CarAllInfo\": \"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\n          \"RequestCharge\": \"http://192.168.3.15:9701/api/v2/CarState/SetToCharge\"\n        }\n      }','3');
INSERT INTO `t_device` VALUES (13,'agv01','agv01',NULL,0,0,1,1,'2023-02-28 11:40:21','2023-04-20 22:28:54',1,8,0,'string','string',234,NULL,NULL,NULL,NULL,'111111111','3');
INSERT INTO `t_device` VALUES (14,'MockAgv02','MockAgv02',NULL,0,0,0,1,'2023-03-03 00:33:58','2023-03-22 02:31:17',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'-1');
INSERT INTO `t_device` VALUES (15,'MockDrill01','MockDrill01',NULL,0,0,1,1,'2023-03-03 00:34:24','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,217,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (16,'P0001','P0001',NULL,0,0,1,1,'2023-03-03 01:32:19','2023-03-08 16:46:09',NULL,14,NULL,NULL,NULL,218,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (20,'P0002','P0002',NULL,0,0,1,1,'2023-03-07 20:19:06','2023-04-23 09:50:35',1,14,NULL,NULL,NULL,218,NULL,NULL,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (38,'Processed002','Processed002',NULL,0,0,1,1,'2023-03-15 10:24:26','2023-03-15 10:36:38',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (39,'Raw10001','Raw10001',NULL,0,0,1,1,'2023-03-15 14:07:38','2023-03-15 14:28:20',NULL,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (49,'UP000102','UP000102',NULL,0,0,1,1,'2023-03-22 07:51:36','2023-03-22 07:52:57',NULL,15,NULL,NULL,NULL,219,NULL,NULL,NULL,NULL,NULL,'2');
/*!40000 ALTER TABLE `t_device` ENABLE KEYS */;
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
) ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备负载板料表';
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_type`
--

LOCK TABLES `t_device_type` WRITE;
/*!40000 ALTER TABLE `t_device_type` DISABLE KEYS */;
INSERT INTO `t_device_type` VALUES (1,'全部',0,0,1,1,'2023-03-23 01:41:25','2023-04-01 11:03:28',1,'000','0');
INSERT INTO `t_device_type` VALUES (7,'钻机',1,0,1,1,'2023-02-28 10:26:41','2023-04-01 11:01:27',1,'drill','0,1');
INSERT INTO `t_device_type` VALUES (8,'AGV',1,0,1,1,'2023-02-28 13:27:37','2023-04-01 11:01:25',1,'agv','0,1');
INSERT INTO `t_device_type` VALUES (14,'叠板机',1,0,1,1,'2023-03-23 02:45:57','2023-04-01 11:01:48',1,'pin','0,1');
INSERT INTO `t_device_type` VALUES (15,'拆板机',1,0,1,1,'2023-03-23 02:46:09','2023-04-01 11:01:19',1,'unpin','0,1');
INSERT INTO `t_device_type` VALUES (16,'生料仓暂存台',1,0,1,1,'2023-03-23 02:46:19','2023-04-01 11:01:13',1,'RawStagingDesk','0,1');
INSERT INTO `t_device_type` VALUES (17,'熟料仓暂存台',1,0,1,1,'2023-03-23 02:46:28','2023-05-04 21:37:02',1,'ProcessedStagingDesk','0,1');
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
  `panel_count` decimal(12,2) DEFAULT NULL COMMENT '叠板数',
  `quantity` decimal(12,2) DEFAULT NULL COMMENT '数量',
  `wad_count` decimal(12,2) DEFAULT NULL COMMENT '叠数',
  `shaft_count` decimal(12,2) DEFAULT NULL COMMENT '轴数',
  `all_passes_count` decimal(12,2) DEFAULT NULL COMMENT '总趟数',
  `remainder_passes_count` decimal(12,2) DEFAULT NULL COMMENT '剩余趟数',
  `drill_count` decimal(12,2) DEFAULT NULL COMMENT '孔数',
  `single_trip_time` decimal(12,2) DEFAULT NULL COMMENT '单趟预计耗时',
  `drill_all_time` decimal(12,2) DEFAULT NULL COMMENT '钻孔总耗时',
  `single_trips` decimal(12,2) DEFAULT NULL COMMENT '单机趟数',
  `dispense_machines` decimal(12,2) DEFAULT NULL COMMENT '分配机台数',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `rules_code_UNIQUE` (`rules_code`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COMMENT='编码生成规则';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_encode_build_rules`
--

LOCK TABLES `t_encode_build_rules` WRITE;
/*!40000 ALTER TABLE `t_encode_build_rules` DISABLE KEYS */;
INSERT INTO `t_encode_build_rules` VALUES (1,'UNITMEASURE_CODE','计量单位编码','JL',6,1,'JL','',0,1,1,'2023-05-18 02:58:43','2023-06-06 16:01:36',1);
INSERT INTO `t_encode_build_rules` VALUES (2,'ITEMTYPE_CODE','物料产品分类编码','IT',6,1,'BM','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (3,'ITEM_CODE','物料产品编码','IF',6,1,'A1','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (4,'WORKSTATION_CODE','工作站编码','WS',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (5,'CLIENT_CODE','客户管理编码','KH',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (6,'VENDOR_CODE','供应商编码','GYS',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (7,'WORKORDER_CODE','生产工单分类编码','MO',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (8,'PANEL_CODE','板料追溯编码','P',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (9,'TASK_CODE','任务编码','TO',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
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
INSERT INTO `t_event_define` VALUES (38,'AGV_REQUEST_CHARGE_EVENT','AGV充电',3,NULL,1,'2023-03-03 16:59:17',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (39,'AGV_REQUEST_PUT_DOWN_SILO_EVENT','AGV下料仓',2,NULL,1,'2023-03-03 17:06:20',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (40,'AGV_REQUEST_PICK_UP_SILO_EVENT','AGV上料仓',2,NULL,1,'2023-03-03 17:13:07',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (41,'AGV_REQUEST_MOVE_EVENT','AGV移动',1,NULL,1,'2023-03-03 17:14:20',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (42,'AGV_REQUEST_STATUSREPORT_EVENT','AGV状态上报',3,NULL,1,'2023-03-03 17:15:05',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (43,'RAWSTAGINGDESK_REQUEST_LOAD_RAW_MATERIAL_EVENT','料仓允许进生料暂存台，呼叫AGV',2,NULL,1,'2023-03-03 17:24:46',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (44,'RAWSTAGINGDESK_REQUEST_UNLOAD_RAW_MATERIAL_EVENT','料仓允许出生料暂存台，呼叫AGV',2,NULL,1,'2023-03-03 17:26:31',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (45,'PROCESSEDSTAGINGDESK_REQUEST_LOAD_PROCESSED_MATERIAL_EVENT','料仓允许进熟料暂存台，呼叫AGV',2,NULL,1,'2023-03-07 09:47:49',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (46,'PROCESSEDSTAGINGDESK_REQUEST_UNLOAD_PROCESSED_MATERIAL_EVENT','料仓允许出熟料暂存台，呼叫AGV',2,NULL,1,'2023-03-07 09:50:53',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (47,'AGV_REQUEST_CHARGE_EVENT','AGV充电',2,'11111111111111111111111111111111111111111',1,'2023-05-23 03:12:27',NULL,NULL,NULL,0,1);
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
  `quantity_qualified` decimal(12,2) DEFAULT NULL COMMENT '合格品数量',
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品定义表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item`
--

LOCK TABLES `t_item` WRITE;
/*!40000 ALTER TABLE `t_item` DISABLE KEYS */;
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '物料类型编号',
  `item_or_product` int(11) DEFAULT NULL COMMENT '物料1产品2',
  `is_system` char(1) DEFAULT 'N' COMMENT '是否系统自带',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_type`
--

LOCK TABLES `t_item_type` WRITE;
/*!40000 ALTER TABLE `t_item_type` DISABLE KEYS */;
INSERT INTO `t_item_type` VALUES (1,'物料产品分类',0,'00',0,'Y','0',0,1,1,'2023-03-30 05:08:07','2023-04-03 02:31:48',1);
INSERT INTO `t_item_type` VALUES (2,'原料',1,'01',1,'Y','0,1',0,1,1,'2023-03-30 01:44:39','2023-04-04 02:11:29',1);
INSERT INTO `t_item_type` VALUES (4,'半成品',1,'02',1,'Y','0,1',0,1,1,'2023-03-30 05:10:36','2023-04-04 02:11:22',1);
INSERT INTO `t_item_type` VALUES (22,'成品',1,'03',2,'Y','0,1',0,1,1,'2023-04-05 01:43:14','2023-04-05 01:44:38',1);
INSERT INTO `t_item_type` VALUES (25,'铜',2,'tong',1,'','0,1,2',0,1,1,'2023-04-12 01:02:53','2023-05-05 01:41:22',1);
INSERT INTO `t_item_type` VALUES (26,'铝',2,'lv',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:10:20','2023-04-12 01:27:20',1);
INSERT INTO `t_item_type` VALUES (27,'板材',2,'panel',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:27:59',NULL,NULL);
INSERT INTO `t_item_type` VALUES (40,'全自动',22,'IT20230523000003BM',2,NULL,'0,1',0,1,1,'2023-05-05 04:03:54','2023-05-23 03:30:38',1);
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
  `code` varchar(255) DEFAULT NULL COMMENT '出入库单号',
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
  `quantity_transaction` decimal(12,2) DEFAULT NULL COMMENT '数量',
  `quantity_onhand` decimal(12,2) DEFAULT NULL COMMENT '在库数量',
  `batch_code` varchar(255) DEFAULT NULL COMMENT '入库批次号',
  `warehouse_id` bigint(20) DEFAULT NULL COMMENT '仓库ID',
  `warehouse_code` varchar(64) DEFAULT NULL COMMENT '仓库编码',
  `warehouse_name` varchar(255) DEFAULT NULL COMMENT '仓库名称',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `workorder_id` bigint(20) DEFAULT NULL COMMENT '生产工单ID',
  `workorder_code` varchar(64) DEFAULT NULL COMMENT '生产工单编号',
  `task_id` bigint(20) DEFAULT NULL COMMENT '生产任务ID',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `parent_id` bigint(20) DEFAULT '0' COMMENT '出库匹配入库ID',
  `parent_code` varchar(255) DEFAULT NULL COMMENT '出库匹配入库Code',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
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
-- Table structure for table `t_notify`
--

DROP TABLE IF EXISTS `t_notify`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_notify` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `notify_time` datetime DEFAULT NULL COMMENT '通知时间',
  `notify_code` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '通知类型编号',
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify`
--

LOCK TABLES `t_notify` WRITE;
/*!40000 ALTER TABLE `t_notify` DISABLE KEYS */;
INSERT INTO `t_notify` VALUES (31,'2023-04-19 13:31:28','0001','AGV充电','AGV充电',22,'钉钉',0,1,1,'2023-04-19 01:31:42','2023-05-05 02:48:27',1,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify_setting`
--

LOCK TABLES `t_notify_setting` WRITE;
/*!40000 ALTER TABLE `t_notify_setting` DISABLE KEYS */;
INSERT INTO `t_notify_setting` VALUES (11,'大屏告警',0,'BIG_SCREEN_WARNING',NULL,NULL,NULL,0,10,0,0,1,'2023-02-28 09:14:03','2023-05-05 02:43:27',1,'0');
INSERT INTO `t_notify_setting` VALUES (12,'短信',0,'LIGHT_WARNING',NULL,NULL,NULL,0,NULL,0,1,1,'2023-02-28 09:19:38','2023-04-17 04:13:26',1,'0');
INSERT INTO `t_notify_setting` VALUES (21,'微信',0,'2222',NULL,NULL,NULL,1,5,0,1,1,'2023-03-28 21:29:34','2023-04-18 03:22:31',1,'0');
INSERT INTO `t_notify_setting` VALUES (22,'钉钉',0,'2222',NULL,NULL,NULL,1,5,0,0,1,'2023-03-29 01:11:08','2023-05-05 02:43:32',1,'0');
INSERT INTO `t_notify_setting` VALUES (23,'微信',0,'2222',NULL,NULL,NULL,1,5,0,1,1,'2023-05-23 03:08:26',NULL,NULL,'0');
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `attention` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工艺要求',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (21,'叠板','pin','叠板',0,1,1,'2023-04-05 02:30:19','2023-05-05 01:56:04',1);
INSERT INTO `t_process` VALUES (22,'钻孔','drill','钻孔要求',0,1,1,'2023-05-10 02:05:50','2023-05-16 20:51:41',1);
INSERT INTO `t_process` VALUES (23,'拆板','unpin',NULL,0,1,1,'2023-04-18 21:35:18','2023-04-23 02:32:27',33);
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  PRIMARY KEY (`id`) USING BTREE
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
-- Table structure for table `t_product_bom`
--

DROP TABLE IF EXISTS `t_product_bom`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_product_bom` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `code` varchar(64) DEFAULT NULL COMMENT '产品结构编码',
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='产品结构表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_bom`
--

LOCK TABLES `t_product_bom` WRITE;
/*!40000 ALTER TABLE `t_product_bom` DISABLE KEYS */;
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=84 DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
INSERT INTO `t_product_category` VALUES (72,0,'P01','单面版','单面板',0,0,1,'2023-05-09 03:40:40','2023-05-23 03:25:30',1,'0');
INSERT INTO `t_product_category` VALUES (74,0,'P02','双面板',NULL,0,1,1,'2023-05-23 03:23:35','2023-05-23 03:25:34',1,'0');
INSERT INTO `t_product_category` VALUES (75,0,'P03','多层板',NULL,0,1,1,'2023-05-23 03:24:03','2023-05-23 03:25:40',1,'0');
INSERT INTO `t_product_category` VALUES (76,0,'P04','刚性电路板',NULL,0,1,1,'2023-05-23 03:24:18','2023-05-23 03:26:10',1,'0');
INSERT INTO `t_product_category` VALUES (77,0,'P05','柔性电路板',NULL,0,1,1,'2023-05-23 03:24:30','2023-05-23 03:26:04',1,'0');
INSERT INTO `t_product_category` VALUES (78,0,'P06','刚柔结合电路板',NULL,0,1,1,'2023-05-23 03:24:44','2023-05-23 03:25:58',1,'0');
INSERT INTO `t_product_category` VALUES (79,0,'P07','高频电路板',NULL,0,1,1,'2023-05-23 03:24:57','2023-05-23 03:25:53',1,'0');
INSERT INTO `t_product_category` VALUES (80,0,'P08','高速电路板',NULL,0,1,1,'2023-05-23 03:25:11','2023-05-23 03:25:49',1,'0');
INSERT INTO `t_product_category` VALUES (81,0,'P08-01','高速电路板--客户001指定',NULL,0,1,1,'2023-05-23 03:25:25',NULL,NULL,'0');
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route`
--

LOCK TABLES `t_route` WRITE;
/*!40000 ALTER TABLE `t_route` DISABLE KEYS */;
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
  `required_time` int(11) DEFAULT NULL COMMENT '本工序耗时',
  `is_manual_check` char(1) DEFAULT NULL COMMENT '是否需要手动检查0/1',
  `self_check_num` int(11) DEFAULT NULL COMMENT '自检数量',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与工序关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_process`
--

LOCK TABLES `t_route_and_process` WRITE;
/*!40000 ALTER TABLE `t_route_and_process` DISABLE KEYS */;
INSERT INTO `t_route_and_process` VALUES (5,2,13,3,'1','#00AEF3',80,'0',4,1,0,1,'2023-04-05 02:41:06','2023-04-14 02:42:32',1);
INSERT INTO `t_route_and_process` VALUES (9,3,22,3,'1','#F31800',840,'0',NULL,0,0,1,'2023-04-07 02:40:29','2023-04-11 22:36:51',1);
INSERT INTO `t_route_and_process` VALUES (10,3,21,2,'0','#00AEF3',960,'0',2,0,0,1,'2023-04-07 03:30:26','2023-05-17 22:22:49',1);
INSERT INTO `t_route_and_process` VALUES (11,2,22,2,'1','#00AEF3',40,'0',3,1,0,1,'2023-04-10 22:58:49','2023-04-21 04:33:06',33);
INSERT INTO `t_route_and_process` VALUES (12,2,21,1,'0','#0A1C23',60,'0',2,1,0,1,'2023-04-10 22:59:09','2023-04-21 04:33:13',33);
INSERT INTO `t_route_and_process` VALUES (13,3,21,2,'1','#444C4F',840,'0',1,1,0,1,'2023-04-12 02:01:11','2023-05-17 22:22:43',1);
INSERT INTO `t_route_and_process` VALUES (23,16,23,1,'1','#00AEF3',123,'1',4,1,0,1,'2023-04-20 04:47:28',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (27,19,22,2,'1','#D22525',180,'1',2,1,0,33,'2023-04-23 02:41:28','2023-05-18 02:18:28',1);
INSERT INTO `t_route_and_process` VALUES (28,19,21,2,'0','#6CD228',10,'0',2,1,0,33,'2023-04-23 02:42:00','2023-04-23 02:44:29',33);
INSERT INTO `t_route_and_process` VALUES (29,19,23,3,'0','#1849DC',10,'0',2,1,0,33,'2023-04-23 02:44:55',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (31,17,22,3,'1','#462828',20,'1',3,1,0,1,'2023-05-05 01:01:49','2023-05-05 01:01:59',1);
INSERT INTO `t_route_and_process` VALUES (32,18,21,1,'1','#4D1E1E',120,'1',1,1,0,1,'2023-05-05 01:09:44',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (33,2,23,1,'1','#853C3C',10,'1',3,1,0,1,'2023-06-13 21:44:55',NULL,NULL);
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
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与产品大类关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_product_category`
--

LOCK TABLES `t_route_and_product_category` WRITE;
/*!40000 ALTER TABLE `t_route_and_product_category` DISABLE KEYS */;
INSERT INTO `t_route_and_product_category` VALUES (5,19,12,1,0,1,'2023-04-06 01:37:07','2023-04-06 02:23:56',1);
INSERT INTO `t_route_and_product_category` VALUES (25,17,7,1,0,1,'2023-04-20 22:40:07',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (26,17,6,1,0,1,'2023-04-20 22:49:04',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (27,19,7,1,0,33,'2023-04-23 02:45:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (28,17,10,1,0,1,'2023-05-05 00:54:13',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (30,2,11,1,0,1,'2023-05-16 04:55:15',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (31,3,78,1,0,1,'2023-05-16 04:55:46','2023-06-13 21:36:06',1);
INSERT INTO `t_route_and_product_category` VALUES (32,3,4,1,0,1,'2023-05-16 04:56:22','2023-05-16 05:02:08',1);
INSERT INTO `t_route_and_product_category` VALUES (33,3,79,1,0,1,'2023-06-13 21:27:36','2023-06-13 21:37:59',1);
INSERT INTO `t_route_and_product_category` VALUES (34,3,78,1,0,1,'2023-06-13 21:39:21',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (35,2,81,1,0,1,'2023-06-13 21:44:36',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (36,19,72,1,0,1,'2023-06-13 21:55:48',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (37,2,80,1,0,1,'2023-06-13 21:57:22',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (38,2,81,1,0,1,'2023-06-13 22:29:11',NULL,NULL);
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
  KEY `index_unique` (`route_and_process_id`,`work_station_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工序与工作站关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_process_and_work_station`
--

LOCK TABLES `t_route_process_and_work_station` WRITE;
/*!40000 ALTER TABLE `t_route_process_and_work_station` DISABLE KEYS */;
INSERT INTO `t_route_process_and_work_station` VALUES (1,27,217,0,1,0,1,'2023-04-05 13:38:09',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (2,27,233,0,0,0,1,'2023-04-05 13:38:18','2023-04-07 08:58:07',1);
INSERT INTO `t_route_process_and_work_station` VALUES (9,27,233,1,1,0,1,'2023-04-11 22:35:32',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (10,27,212,1,1,0,1,'2023-04-11 22:35:54',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (11,28,218,1,1,0,1,'2023-04-11 22:36:12',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (13,28,218,1,1,0,1,'2023-04-11 22:36:50',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (14,28,218,1,1,0,1,'2023-04-11 22:37:23',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (28,29,219,1,1,0,1,'2023-04-18 23:05:31',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (31,29,219,1,1,0,1,'2023-04-20 22:39:49',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (32,29,219,1,1,0,1,'2023-04-20 22:50:09',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (33,29,219,1,1,0,1,'2023-05-08 21:27:15',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (34,13,233,1,1,0,1,'2023-05-17 03:55:49',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (35,10,233,1,1,0,43,'2023-05-22 23:16:11','2023-05-22 23:16:14',43);
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '任务编号',
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
  PRIMARY KEY (`id`) USING BTREE
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
-- Table structure for table `t_subject`
--

DROP TABLE IF EXISTS `t_subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_subject` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='点检保养项目';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_subject`
--

LOCK TABLES `t_subject` WRITE;
/*!40000 ALTER TABLE `t_subject` DISABLE KEYS */;
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  `quantity_changed` decimal(11,0) DEFAULT NULL COMMENT '调整数量',
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
  `task_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED)',
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
  `is_started` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
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
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  PRIMARY KEY (`id`) USING BTREE
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
  `change_rate` decimal(12,4) DEFAULT NULL COMMENT '与主单位换算比例',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (34,'叠板2层','叠板2层','Y',NULL,1.0000,NULL,0,1,1,'2023-04-18 22:13:06','2023-04-20 02:42:16',1);
INSERT INTO `t_unit_measure` VALUES (74,'Panel','单层Panel','Y',34,2.0000,'单位',0,0,1,'2023-05-21 22:46:43',NULL,NULL);
INSERT INTO `t_unit_measure` VALUES (76,'JL20230613000001JL','pen','Y',NULL,0.0000,NULL,0,1,1,'2023-06-13 21:47:59',NULL,NULL);
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_vendor`
--

LOCK TABLES `t_vendor` WRITE;
/*!40000 ALTER TABLE `t_vendor` DISABLE KEYS */;
INSERT INTO `t_vendor` VALUES (37,'GYS20230613000001','李老板',NULL,0,1,1,'2023-04-18 22:22:01','2023-06-13 01:34:51',1);
INSERT INTO `t_vendor` VALUES (47,'GYS20230613000002','张老板','铜料板供应',0,1,1,'2023-06-06 16:10:11','2023-06-13 01:35:04',1);
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
  `workStationId` varchar(500) DEFAULT NULL COMMENT '工位位置',
  `charge` varchar(64) DEFAULT NULL COMMENT '负责人',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='仓库表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_warehouse`
--

LOCK TABLES `t_warehouse` WRITE;
/*!40000 ALTER TABLE `t_warehouse` DISABLE KEYS */;
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
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
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
  `work_order_status` varchar(20) DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `is_add_work_order` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否增加工单任务',
  `add_work_order_id` int(11) DEFAULT NULL COMMENT '增加工单任务人id',
  `add_work_order_time` datetime DEFAULT NULL COMMENT '增加工单任务时间',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workshop`
--

LOCK TABLES `t_workshop` WRITE;
/*!40000 ALTER TABLE `t_workshop` DISABLE KEYS */;
INSERT INTO `t_workshop` VALUES (9,'shop02','车间02（测试）','张三','此车间的备注信息（选填）',0,2,1,'2023-04-03 22:03:36','2023-04-13 01:21:56',1);
INSERT INTO `t_workshop` VALUES (11,'shop01','车间01（测试）','张三','此车间的备注信息（选填）',0,1,1,'2023-04-10 23:31:47',NULL,NULL);
INSERT INTO `t_workshop` VALUES (39,'shop03','车间03（测试）','李四',NULL,0,1,33,'2023-04-23 02:28:30','2023-04-23 02:28:53',33);
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
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workstation`
--

LOCK TABLES `t_workstation` WRITE;
/*!40000 ALTER TABLE `t_workstation` DISABLE KEYS */;
INSERT INTO `t_workstation` VALUES (212,'drill01-0001','钻机01-0001',9,'shop02','车间02（测试）','',0,1,1,'2023-04-04 02:32:34','2023-05-23 21:31:20',1);
INSERT INTO `t_workstation` VALUES (217,'drill01-0002','钻机01-0002',9,'shop02','车间02（测试）',NULL,0,1,1,'2023-04-05 22:37:47','2023-05-23 21:31:14',1);
INSERT INTO `t_workstation` VALUES (218,'pin01-0001','叠板机01-0001',9,'shop02','车间02（测试）',NULL,0,1,1,'2023-04-05 22:39:06',NULL,NULL);
INSERT INTO `t_workstation` VALUES (219,'unpin01-0001','拆板机01-0001',9,'shop02','车间02（测试）',NULL,0,1,1,'2023-04-05 22:39:38','2023-05-08 22:27:23',1);
INSERT INTO `t_workstation` VALUES (233,'drill01-0003','钻机01-0003',9,'shop02','车间02（测试）','钻机工作站',0,1,1,'2023-05-09 03:07:07',NULL,NULL);
INSERT INTO `t_workstation` VALUES (234,'agv01-001','AGV01-001',9,'shop02','车间02（测试）','AGV',0,1,1,'2023-05-09 03:07:07',NULL,NULL);
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

-- Dump completed on 2023-06-16  3:36:11
