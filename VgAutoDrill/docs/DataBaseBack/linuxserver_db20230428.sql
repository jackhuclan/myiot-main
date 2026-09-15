Enter password: 
-- MySQL dump 10.13  Distrib 5.7.41, for Linux (x86_64)
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
INSERT INTO `sys_department` VALUES (9,'研发部门',2,'18037458723','2230485082@qq.com',NULL,'xm',0,1,1,'2023-02-23 03:10:17',NULL,NULL);
INSERT INTO `sys_department` VALUES (10,'北京分公司',3,'18890899089','18890899089@163.com',NULL,'王五',0,1,1,'2023-03-23 03:01:13','2023-03-23 03:12:04',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=1315 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
INSERT INTO `sys_menu` VALUES (1024,'DRILLAMIN','生产管理',0,'cascader','produce','#',1,NULL,NULL,2,0,1,1,'2023-02-20 23:01:11','2023-03-01 13:19:07',1);
INSERT INTO `sys_menu` VALUES (1025,'DRILLAMIN','排刀管理',0,'build','material','#',1,NULL,NULL,3,0,1,1,'2023-02-20 23:01:59','2023-04-10 05:38:08',1);
INSERT INTO `sys_menu` VALUES (1026,'DRILLAMIN','设备管理',0,'phone','device','#',1,NULL,NULL,4,0,1,1,'2023-02-20 23:02:35','2023-03-01 14:09:09',1);
INSERT INTO `sys_menu` VALUES (1027,'DRILLAMIN','告警管理',0,'email','alarm','#',1,NULL,NULL,5,0,1,1,'2023-02-20 23:03:32','2023-03-01 14:09:18',1);
INSERT INTO `sys_menu` VALUES (1029,'DRILLAMIN','生产排产',1024,'#','task','produce/task/index',2,'produce:task:list',NULL,55,0,1,1,'2023-02-20 23:06:58','2023-04-06 04:58:11',1);
INSERT INTO `sys_menu` VALUES (1030,'DRILLAMIN','板料追溯',1024,'#','board','produce/board/index',2,'produce:board:list',NULL,70,0,1,1,'2023-02-20 23:09:15','2023-04-10 23:34:50',1);
INSERT INTO `sys_menu` VALUES (1031,'DRILLAMIN','生产记录',1024,'#','record','produce/record/index',2,'produce:transorder:list',NULL,60,0,0,1,'2023-02-20 23:11:13','2023-04-03 23:26:27',1);
INSERT INTO `sys_menu` VALUES (1032,'DRILLAMIN','刀盘管理',1025,'#','cutter','material/cutter/index',2,'material:cutter:list',NULL,1,0,0,1,'2023-02-20 23:15:40','2023-04-18 02:15:39',1);
INSERT INTO `sys_menu` VALUES (1033,'DRILLAMIN','设备类型',1026,'#','equipmentType','device/equipmentType/index',2,'device:equipmentType:list',NULL,1,0,1,1,'2023-02-20 23:18:29','2023-04-02 21:21:28',1);
INSERT INTO `sys_menu` VALUES (1034,'DRILLAMIN','配方管理',1024,'#','recipe','produce/recipe/index',2,'produce:recipe:list',NULL,6,0,0,1,'2023-02-20 23:20:37','2023-04-17 21:17:52',1);
INSERT INTO `sys_menu` VALUES (1035,'DRILLAMIN','设备列表',1026,'#','equipment','device/equipment/index',2,'device:equipment:list',NULL,3,0,1,1,'2023-02-20 23:22:03','2023-02-21 02:49:47',1);
INSERT INTO `sys_menu` VALUES (1036,'DRILLAMIN','刀具参数',1025,'#','config','material/config/index',2,'material:config:list',NULL,2,0,1,1,'2023-02-20 23:23:49','2023-03-28 05:30:47',1);
INSERT INTO `sys_menu` VALUES (1037,'DRILLAMIN','调度记录',1026,'#','schedulement','device/schedulement/index',2,'device:schedulement:edit',NULL,5,0,0,1,'2023-02-20 23:25:16','2023-04-17 21:18:06',1);
INSERT INTO `sys_menu` VALUES (1038,'DRILLAMIN','事件管理',1027,'#','event','alarm/event/index',2,'alarm:event:list',NULL,1,0,1,1,'2023-02-20 23:27:03','2023-02-23 22:04:19',1);
INSERT INTO `sys_menu` VALUES (1039,'DRILLAMIN','告警记录',1027,'#','warn','alarm/warn/index',2,'alarm:warn:list',NULL,3,0,1,1,'2023-02-20 23:28:17','2023-03-23 22:13:35',1);
INSERT INTO `sys_menu` VALUES (1043,'DRILLAMIN','产品修改',1033,NULL,'','#',3,'device:product:edit',NULL,3,0,1,1,'2023-02-21 20:32:06','2023-02-21 22:09:43',1);
INSERT INTO `sys_menu` VALUES (1044,'DRILLAMIN','产品删除',1033,NULL,NULL,'#',3,'device:product:del',NULL,4,0,1,1,'2023-02-21 20:32:58','2023-02-21 22:09:52',1);
INSERT INTO `sys_menu` VALUES (1045,'DRILLAMIN','产品查询',1033,NULL,NULL,'#',3,'device:product:list',NULL,1,0,1,1,'2023-02-21 20:33:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1046,'DRILLAMIN','配方查询',1034,NULL,NULL,'#',3,'produce:recipe:list',NULL,1,0,1,1,'2023-02-21 22:13:46','2023-04-03 21:32:00',1);
INSERT INTO `sys_menu` VALUES (1047,'DRILLAMIN','配方新增',1034,NULL,NULL,'#',3,'produce:recipe:add',NULL,2,0,1,1,'2023-02-21 22:14:27','2023-04-03 21:32:38',1);
INSERT INTO `sys_menu` VALUES (1048,'DRILLAMIN','配方修改',1034,NULL,NULL,'#',3,'produce:recipe:edit',NULL,3,0,1,1,'2023-02-21 22:15:18','2023-04-03 21:32:13',1);
INSERT INTO `sys_menu` VALUES (1049,'DRILLAMIN','配方删除',1034,NULL,NULL,'#',3,'produce:recipe:remove',NULL,4,0,1,1,'2023-02-21 22:15:58','2023-04-03 21:32:22',1);
INSERT INTO `sys_menu` VALUES (1052,'DRILLAMIN','产品重置',1033,NULL,NULL,'#',3,'device:product:reset',NULL,5,0,1,1,'2023-02-21 22:22:09','2023-02-21 22:24:34',1);
INSERT INTO `sys_menu` VALUES (1053,'DRILLAMIN','配方重置',1034,NULL,NULL,'#',3,'produce:recipe:reset',NULL,5,0,1,1,'2023-02-21 22:25:29','2023-04-03 21:32:29',1);
INSERT INTO `sys_menu` VALUES (1054,'DRILLAMIN','通知管理',0,'log','','#',1,NULL,NULL,6,0,1,1,'2023-02-22 01:16:01','2023-03-01 14:09:29',1);
INSERT INTO `sys_menu` VALUES (1055,'DRILLAMIN','通知设置',1054,'#','setting','notify/setting/index',2,'notify:setting:list',NULL,1,0,1,1,'2023-02-22 01:21:12','2023-03-23 02:15:26',1);
INSERT INTO `sys_menu` VALUES (1056,'DRILLAMIN','通知记录',1054,'#','notify','notify/record/index',2,'notify:record:list',NULL,2,0,1,1,'2023-02-22 01:23:31','2023-03-23 02:14:43',1);
INSERT INTO `sys_menu` VALUES (1057,'DRILLAMIN','设备查询',1035,NULL,NULL,'#',3,'device:equipment:list',NULL,1,0,1,1,'2023-02-22 02:27:33','2023-02-22 02:34:04',1);
INSERT INTO `sys_menu` VALUES (1058,'DRILLAMIN','设备新增',1035,NULL,NULL,'#',3,'device:equipment:add',NULL,2,0,1,1,'2023-02-22 02:30:35','2023-02-22 02:31:48',1);
INSERT INTO `sys_menu` VALUES (1059,'DRILLAMIN','设备修改',1035,NULL,NULL,'#',3,'device:equipment:edit',NULL,3,0,1,1,'2023-02-22 02:32:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1060,'DRILLAMIN','设备删除',1035,NULL,NULL,'#',3,'device:equipment:remove',NULL,4,0,1,1,'2023-02-22 02:33:41',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1061,'DRILLAMIN','设备查看',1035,NULL,NULL,'#',3,'device:equipment:view',NULL,5,0,1,1,'2023-02-22 02:35:07','2023-03-27 01:33:36',1);
INSERT INTO `sys_menu` VALUES (1062,'DRILLAMIN','参数查询',1036,NULL,NULL,'#',3,'device:parameter:list',NULL,1,0,1,1,'2023-02-22 02:36:27','2023-02-22 02:36:51',1);
INSERT INTO `sys_menu` VALUES (1063,'DRILLAMIN','参数新增',1036,NULL,NULL,'#',3,'device:parameter:add',NULL,2,0,1,1,'2023-02-22 02:37:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1064,'DRILLAMIN','参数修改',1036,NULL,NULL,'#',3,'device:parameter:edit',NULL,3,0,1,1,'2023-02-22 02:38:13',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1065,'DRILLAMIN','参数删除',1036,NULL,NULL,'#',3,'device:parameter:remove',NULL,4,0,1,1,'2023-02-22 02:38:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1066,'DRILLAMIN','参数重置',1036,NULL,NULL,'#',3,'device:parameter:reset',NULL,5,0,1,1,'2023-02-22 02:39:33',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1067,'DRILLAMIN','调查查询',1037,NULL,NULL,'#',3,'device:schedulement:list',NULL,1,0,1,1,'2023-02-22 02:40:25',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1068,'DRILLAMIN','调查新增',1037,NULL,NULL,'#',3,'device:schedulement:add',NULL,2,0,1,1,'2023-02-22 02:41:06',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1069,'DRILLAMIN','调查修改',1037,NULL,NULL,'#',3,'device:schedulement:edit',NULL,3,0,1,1,'2023-02-22 02:41:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1070,'DRILLAMIN','调查删除',1037,NULL,NULL,'#',3,'device:schedulement:delete',NULL,4,0,1,1,'2023-02-22 02:42:02',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1071,'DRILLAMIN','调查重置',1037,NULL,NULL,'#',3,'device:schedulement:reset',NULL,5,0,1,1,'2023-02-22 02:43:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1072,'DRILLAMIN','板料查询',1030,NULL,NULL,'#',3,'produce:board:list',NULL,1,0,1,1,'2023-02-22 04:19:39',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1073,'DRILLAMIN','板料新增',1030,NULL,NULL,'#',3,'produce:board:add',NULL,2,0,1,1,'2023-02-22 04:20:16',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1074,'DRILLAMIN','板料修改',1030,NULL,NULL,'#',3,'produce:board:edit',NULL,3,0,1,1,'2023-02-22 04:20:47',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1075,'DRILLAMIN','板料删除',1030,NULL,NULL,'#',3,'produce:board:remove',NULL,4,0,1,1,'2023-02-22 04:21:23',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1076,'DRILLAMIN','板料重置',1030,NULL,NULL,'#',3,'produce:board:reset',NULL,5,0,1,1,'2023-02-22 04:22:07',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1077,'DRILLAMIN','排产查询',1029,NULL,NULL,'#',3,'produce:task:list',NULL,1,0,1,1,'2023-02-22 04:23:27','2023-04-05 22:52:09',33);
INSERT INTO `sys_menu` VALUES (1078,'DRILLAMIN','排产新增',1029,NULL,NULL,'#',3,'produce:task:add',NULL,2,0,1,1,'2023-02-22 04:23:59','2023-04-05 22:52:17',33);
INSERT INTO `sys_menu` VALUES (1079,'DRILLAMIN','排产修改',1029,NULL,NULL,'#',3,'produce:task:edit',NULL,3,0,1,1,'2023-02-22 04:24:34','2023-04-05 22:52:29',33);
INSERT INTO `sys_menu` VALUES (1080,'DRILLAMIN','排产删除',1029,NULL,NULL,'#',3,'produce:task:remove',NULL,4,0,1,1,'2023-02-22 04:25:14','2023-04-05 22:52:39',33);
INSERT INTO `sys_menu` VALUES (1081,'DRILLAMIN','排产重置',1029,NULL,NULL,'#',3,'produce:task:reset',NULL,5,0,1,1,'2023-02-22 04:26:03','2023-04-05 22:52:51',33);
INSERT INTO `sys_menu` VALUES (1082,'DRILLAMIN','生产查询',1031,NULL,NULL,'#',3,'produce:transorder:list',NULL,1,0,1,1,'2023-02-22 04:28:02','2023-04-03 23:25:02',1);
INSERT INTO `sys_menu` VALUES (1083,'DRILLAMIN','生产新增',1031,NULL,NULL,'#',3,'produce:transorder:add',NULL,2,0,1,1,'2023-02-22 04:28:41','2023-04-03 23:25:10',1);
INSERT INTO `sys_menu` VALUES (1084,'DRILLAMIN','生产修改',1031,NULL,NULL,'#',3,'produce:transorder:edit',NULL,3,0,1,1,'2023-02-22 04:29:10','2023-04-03 23:25:55',1);
INSERT INTO `sys_menu` VALUES (1085,'DRILLAMIN','生产删除',1031,NULL,NULL,'#',3,'produce:transorder:remove',NULL,4,0,1,1,'2023-02-22 04:29:43','2023-04-03 23:25:39',1);
INSERT INTO `sys_menu` VALUES (1086,'DRILLAMIN','生产重置',1031,NULL,NULL,'#',3,'produce:transorder:reset',NULL,5,0,1,1,'2023-02-22 04:30:12','2023-04-03 23:25:29',1);
INSERT INTO `sys_menu` VALUES (1087,'DRILLAMIN','刀具查询',1032,NULL,NULL,'#',3,'material:cutter:list',NULL,1,0,1,1,'2023-02-22 04:31:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1088,'DRILLAMIN','刀具新增',1032,NULL,NULL,'#',3,'material:cutter:add',NULL,2,0,1,1,'2023-02-22 04:32:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1089,'DRILLAMIN','刀具修改',1032,NULL,NULL,'#',3,'material:cutter:edit',NULL,3,0,1,1,'2023-02-22 04:33:24',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1090,'DRILLAMIN','刀具删除',1032,NULL,NULL,'#',3,'material:cutter:remove',NULL,4,0,1,1,'2023-02-22 04:33:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1091,'DRILLAMIN','刀具重置',1032,NULL,NULL,'#',3,'material:cutter:reset',NULL,5,0,1,1,'2023-02-22 04:34:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1097,'DRILLAMIN','事件列表',1038,NULL,NULL,'#',3,'alarm:event:list',NULL,1,0,1,1,'2023-02-24 00:07:17',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1098,'DRILLAMIN','事件添加',1038,NULL,NULL,'#',3,'alarm:event:add',NULL,2,0,1,1,'2023-02-24 00:08:03','2023-02-24 00:08:49',1);
INSERT INTO `sys_menu` VALUES (1099,'DRILLAMIN','事件修改',1038,NULL,NULL,'#',3,'alarm:event:edit',NULL,3,0,1,1,'2023-02-24 00:08:38',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1100,'DRILLAMIN','事件删除',1038,NULL,NULL,'#',3,'alarm:event:remove',NULL,4,0,1,1,'2023-02-24 00:09:16',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1101,'DRILLAMIN','事件重置',1038,NULL,NULL,'#',3,'alarm:event:reset',NULL,5,0,1,1,'2023-02-24 00:09:55',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1102,'DRILLAMIN','警告查询',1039,NULL,NULL,'#',3,'alarm:warn:list',NULL,1,0,1,1,'2023-02-24 00:10:40',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1103,'DRILLAMIN','警告添加',1039,NULL,NULL,'#',3,'alarm:warn:add',NULL,2,0,1,1,'2023-02-24 00:11:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1104,'DRILLAMIN','警告修改',1039,NULL,NULL,'#',3,'alarm:warn:edit',NULL,3,0,1,1,'2023-02-24 00:12:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1105,'DRILLAMIN','警告删除',1039,NULL,NULL,'#',3,'alarm:warn:remove',NULL,4,0,1,1,'2023-02-24 00:12:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1106,'DRILLAMIN','警告重置',1039,NULL,NULL,'#',3,'alarm:warn:reset',NULL,5,0,1,1,'2023-02-24 00:12:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1107,'DRILLAMIN','警告导入',1039,NULL,NULL,'#',3,'alarm:warn:import',NULL,6,0,1,1,'2023-02-24 00:13:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1108,'DRILLAMIN','通知查询',1055,NULL,NULL,'#',3,'notify:record:list',NULL,1,0,1,1,'2023-02-24 00:14:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1109,'DRILLAMIN','通知添加',1055,NULL,NULL,'#',3,'notify:record:add',NULL,2,0,1,1,'2023-02-24 00:15:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1110,'DRILLAMIN','通知修改',1055,NULL,NULL,'#',3,'notify:record:edit',NULL,3,0,1,1,'2023-02-24 00:16:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1111,'DRILLAMIN','通知删除',1055,NULL,NULL,'#',3,'notify:record:remove',NULL,4,0,1,1,'2023-02-24 00:17:25',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1112,'DRILLAMIN','通知重置',1055,NULL,NULL,'#',3,'notify:record:reset',NULL,5,0,1,1,'2023-02-24 00:17:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1113,'DRILLAMIN','通知查询',1056,NULL,NULL,'#',3,'notify:setting:list',NULL,1,0,1,1,'2023-02-24 00:18:25',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1114,'DRILLAMIN','通知新增',1056,NULL,NULL,'#',3,'notify:setting:add',NULL,2,0,1,1,'2023-02-24 00:18:55',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1115,'DRILLAMIN','通知修改',1056,NULL,NULL,'#',3,'notify:setting:edit',NULL,3,0,1,1,'2023-02-24 00:19:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1116,'DRILLAMIN','通知删除',1056,NULL,NULL,'#',3,'notify:setting:remove',NULL,4,0,1,1,'2023-02-24 00:19:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1117,'DRILLAMIN','通知重置',1056,NULL,NULL,'#',3,'notify:setting:reset',NULL,5,0,1,1,'2023-02-24 00:20:16',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1122,'DRILLAMIN','生产工单',1024,'#','workOrder','produce/workOrder/index',2,'produce:workorder:list',NULL,50,0,1,1,'2023-03-01 11:03:28','2023-04-03 22:10:46',1);
INSERT INTO `sys_menu` VALUES (1123,'DRILLAMIN','查询',1122,NULL,NULL,'#',3,'produce:workorder:list',NULL,1,0,1,1,'2023-03-01 11:04:30','2023-03-01 11:05:22',1);
INSERT INTO `sys_menu` VALUES (1124,'DRILLAMIN','新增',1122,NULL,NULL,'#',3,'produce:workorder:add',NULL,2,0,1,1,'2023-03-01 11:06:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1125,'DRILLAMIN','修改',1122,NULL,NULL,'#',3,'produce:workorder:edit',NULL,3,0,1,1,'2023-03-01 11:06:41',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1126,'DRILLAMIN','删除',1122,NULL,NULL,'#',3,'produce:workorder:remove',NULL,4,0,1,1,'2023-03-01 11:07:08','2023-03-01 11:07:17',1);
INSERT INTO `sys_menu` VALUES (1127,'DRILLAMIN','重置',1122,NULL,NULL,'#',3,'produce:workorder:reset',NULL,5,0,1,1,'2023-03-01 11:07:46',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1128,'DRILLAMIN','工序管理',1024,'#','process','produce/process/index',2,'produce:process:list',NULL,3,0,1,1,'2023-03-01 11:37:07','2023-04-03 22:12:58',1);
INSERT INTO `sys_menu` VALUES (1129,'DRILLAMIN','查询',1128,'#',NULL,'#',3,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:42:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1130,'DRILLAMIN','新增',1128,'#',NULL,'#',3,'produce:process:add',NULL,2,0,1,1,'2023-03-01 11:43:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1131,'DRILLAMIN','修改',1128,'#',NULL,'#',3,'produce:process:edit',NULL,3,0,1,1,'2023-03-01 11:43:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1132,'DRILLAMIN','删除',1128,'#',NULL,'#',3,'produce:process:remove',NULL,4,0,1,1,'2023-03-01 11:43:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1133,'DRILLAMIN','重置',1128,'#',NULL,'#',3,'produce:process:reset',NULL,5,0,1,1,'2023-03-01 11:44:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1134,'DRILLAMIN','字典管理',1,'dict','dict','system/dict/index',2,'system:dict:list',NULL,3,0,0,1,'2023-03-01 13:53:42','2023-04-04 09:40:08',1);
INSERT INTO `sys_menu` VALUES (1136,'DRILLAMIN','告警设置',1027,'#','setting','alarm/setting/index',2,'alarm:setting:index',NULL,2,0,1,1,'2023-03-23 22:13:23','2023-03-23 22:13:48',1);
INSERT INTO `sys_menu` VALUES (1137,'DRILLAMIN','设置列表',1136,'#',NULL,'#',3,'alarm:setting:list',NULL,1,0,1,1,'2023-03-23 22:15:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1138,'DRILLAMIN','设置添加',1136,'#',NULL,'#',3,'alarm:setting:add',NULL,2,0,1,1,'2023-03-23 22:16:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1139,'DRILLAMIN','设置修改',1136,'#',NULL,'#',3,'alarm:setting:edit',NULL,3,0,1,1,'2023-03-23 22:16:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1140,'DRILLAMIN','设置删除',1136,'#',NULL,'#',3,'alarm:setting:remove',NULL,4,0,1,1,'2023-03-23 22:17:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1147,'DRILLAMIN','排刀计划',1024,'#','drillCutterPlan','produce/drillCutterPlan/index',1,'produce:drillCutterPlan:list',NULL,53,0,0,1,'2023-03-28 04:56:52','2023-04-17 21:17:39',1);
INSERT INTO `sys_menu` VALUES (1148,'DRILLAMIN','主数据',0,'component','masterData','#',1,NULL,NULL,1,0,1,1,'2023-03-29 22:00:59','2023-04-03 22:07:28',1);
INSERT INTO `sys_menu` VALUES (1149,'DRILLAMIN','计量单位',1148,'#','unitMeasure','masterData/unitMeasure/index',2,'masterData:unitMeasure:list',NULL,1,0,1,1,'2023-03-29 22:05:55',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1150,'DRILLAMIN','客户管理',1148,'#','client','masterData/client/index',2,'masterData:client:list',NULL,50,0,1,1,'2023-03-29 22:07:26','2023-04-03 22:08:13',1);
INSERT INTO `sys_menu` VALUES (1151,'DRILLAMIN','供应商管理',1148,'#','vendor','masterData/vendor/index',2,'masterData:vendor:list',NULL,60,0,1,1,'2023-03-29 22:09:03','2023-04-03 22:08:24',1);
INSERT INTO `sys_menu` VALUES (1152,'DRILLAMIN','车间管理',1148,'#','workshop','masterData/workShop/index',2,'masterData:workshop:list',NULL,40,0,1,1,'2023-03-29 22:10:20','2023-04-12 05:15:41',1);
INSERT INTO `sys_menu` VALUES (1153,'DRILLAMIN','工作站',1148,'#','workstation','masterData/workStation/index',2,'masterData:workstation:index',NULL,41,0,1,1,'2023-03-29 22:11:24','2023-04-12 05:17:07',1);
INSERT INTO `sys_menu` VALUES (1154,'DRILLAMIN','物料产品分类',1148,'#','itemType','masterData/itemType/index',2,'masterData:itemType:list',NULL,10,0,1,1,'2023-03-29 22:12:21','2023-04-03 22:09:16',1);
INSERT INTO `sys_menu` VALUES (1155,'DRILLAMIN','物料产品管理',1148,'#','item','masterData/item/index',2,'masterData:item:list',NULL,11,0,1,1,'2023-03-29 22:13:20','2023-04-03 22:09:23',1);
INSERT INTO `sys_menu` VALUES (1156,'DRILLAMIN','产品大类',1148,'#','productCategory','masterData/productCategory/index',2,'masterData:productCategory:list',NULL,5,0,1,1,'2023-04-01 02:41:29','2023-04-03 22:09:08',1);
INSERT INTO `sys_menu` VALUES (1157,'DRILLAMIN','工艺流程',1024,'#','route','produce/route/index',2,'produce:route:list',NULL,5,0,1,1,'2023-04-01 02:46:56','2023-04-03 02:09:33',1);
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'tree','wm','#',1,NULL,NULL,50,0,1,1,'2023-04-01 02:50:34','2023-04-19 02:36:29',1);
INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','仓库设置',1158,'#','warehouse','wareHouse/wareHouse/index',2,'warehouse:warehouse:list',NULL,1,0,1,1,'2023-04-01 02:53:42','2023-04-18 03:33:51',1);
INSERT INTO `sys_menu` VALUES (1160,'DRILLAMIN','库存现有量',1158,'#','materialStock','wareHouse/materialStock/index',2,'warehouse:materialStock:list',NULL,2,0,1,1,'2023-04-01 02:54:57','2023-04-18 03:34:01',1);
INSERT INTO `sys_menu` VALUES (1161,'DRILLAMIN','钻带参数',1025,'#','drillFile','material/drillFile/index',2,'material:drillFile:list',NULL,3,0,1,1,'2023-04-01 03:20:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1162,'DRILLAMIN','排刀文件',1025,'#','atpFile','material/atpFile/index',2,'material:atpFile:list',NULL,4,0,1,1,'2023-04-01 03:21:21','2023-04-10 23:34:21',1);
INSERT INTO `sys_menu` VALUES (1165,'DRILLAMIN','生产报工',1024,'#','feedback','produce/feedback/index',2,'produce:feedback:list',NULL,60,0,1,1,'2023-04-04 00:47:50','2023-04-04 00:48:01',1);
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
INSERT INTO `sys_menu` VALUES (1280,'DRILLAMIN','排产提交',1029,'#',NULL,'#',3,'produce:task:commit',NULL,6,0,1,33,'2023-04-05 22:53:40','2023-04-05 22:53:57',33);
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
INSERT INTO `sys_menu` VALUES (1308,'DRILLAMIN','配方查看',1034,'#',NULL,'#',3,'produce:recipe:view',NULL,6,0,1,33,'2023-04-10 04:21:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1309,'DRILLAMIN','查看',1122,'#',NULL,'#',3,'produce:workorder:view',NULL,7,0,1,33,'2023-04-10 04:22:28',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1310,'DRILLAMIN','排产查看',1029,'#',NULL,'#',3,'produce:task:view',NULL,7,0,1,33,'2023-04-10 04:23:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1311,'DRILLAMIN','生产查看',1031,'#',NULL,'#',3,'produce:transorder:view',NULL,6,0,1,33,'2023-04-10 04:25:02',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1312,'DRILLAMIN','查看',1165,'#',NULL,'#',3,'produce:feedback:view',NULL,7,0,1,33,'2023-04-10 04:25:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1313,'DRILLAMIN','板料查看',1030,'#',NULL,'#',3,'produce:board:view',NULL,6,0,1,33,'2023-04-10 04:28:13',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1314,'DRILLAMIN','检验记录',1024,'#','checkrecords','produce/checkRecords/index',2,NULL,NULL,71,0,1,1,'2023-04-14 02:34:00','2023-04-14 03:29:31',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=5506 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu_auth`
--

LOCK TABLES `sys_menu_auth` WRITE;
/*!40000 ALTER TABLE `sys_menu_auth` DISABLE KEYS */;
INSERT INTO `sys_menu_auth` VALUES (132,1024,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (133,1029,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (134,1030,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (135,1034,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (136,1122,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (137,1128,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (138,1147,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (139,1157,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (140,1025,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (141,1032,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (142,1036,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (143,1161,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (144,1162,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (145,1026,3,1,1,'2023-04-04 02:36:35');
INSERT INTO `sys_menu_auth` VALUES (146,1033,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (147,1035,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (148,1037,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (149,1077,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (150,1072,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (151,1046,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (152,1123,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (153,1129,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (154,1225,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (155,1219,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (156,1224,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (157,1165,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (158,1166,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (159,1167,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (160,1168,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (161,1169,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (162,1170,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (163,1087,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (164,1062,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (165,1171,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (166,1176,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (167,1177,3,1,1,'2023-04-04 02:36:36');
INSERT INTO `sys_menu_auth` VALUES (168,1182,3,1,1,'2023-04-04 02:36:37');
INSERT INTO `sys_menu_auth` VALUES (169,1045,3,1,1,'2023-04-04 02:36:37');
INSERT INTO `sys_menu_auth` VALUES (170,1057,3,1,1,'2023-04-04 02:36:37');
INSERT INTO `sys_menu_auth` VALUES (171,1067,3,1,1,'2023-04-04 02:36:37');
INSERT INTO `sys_menu_auth` VALUES (1705,1,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1706,2,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1707,1000,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1708,1001,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1709,1002,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1710,1003,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1711,1004,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1712,1005,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1713,1006,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1714,5,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1715,1016,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1716,1017,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1717,1018,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1718,1019,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1719,6,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1720,1020,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1721,1021,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1722,1022,2,1,1,'2023-04-04 02:55:46');
INSERT INTO `sys_menu_auth` VALUES (1723,1023,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1724,1024,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1725,1029,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1726,1077,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1727,1078,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1728,1079,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1729,1080,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1730,1081,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1731,1030,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1732,1072,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1733,1073,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1734,1074,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1735,1075,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1736,1076,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1737,1034,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1738,1046,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1739,1047,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1740,1048,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1741,1049,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1742,1053,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1743,1122,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1744,1123,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1745,1124,2,1,1,'2023-04-04 02:55:47');
INSERT INTO `sys_menu_auth` VALUES (1746,1125,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1747,1126,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1748,1127,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1749,1128,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1750,1129,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1751,1130,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1752,1131,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1753,1132,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1754,1133,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1755,1147,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1756,1225,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1757,1226,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1758,1227,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1759,1228,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1760,1229,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1761,1230,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1762,1157,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1763,1219,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1764,1220,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1765,1221,2,1,1,'2023-04-04 02:55:48');
INSERT INTO `sys_menu_auth` VALUES (1766,1222,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1767,1223,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1768,1224,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1769,1165,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1770,1166,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1771,1167,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1772,1168,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1773,1169,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1774,1170,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1775,1025,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1776,1032,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1777,1087,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1778,1088,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1779,1089,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1780,1090,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1781,1091,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1782,1036,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1783,1062,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1784,1063,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1785,1064,2,1,1,'2023-04-04 02:55:49');
INSERT INTO `sys_menu_auth` VALUES (1786,1065,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1787,1066,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1788,1161,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1789,1171,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1790,1172,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1791,1173,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1792,1174,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1793,1175,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1794,1176,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1795,1162,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1796,1177,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1797,1178,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1798,1179,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1799,1180,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1800,1181,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1801,1182,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1802,1026,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1803,1033,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1804,1043,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1805,1044,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1806,1045,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1807,1052,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1808,1035,2,1,1,'2023-04-04 02:55:50');
INSERT INTO `sys_menu_auth` VALUES (1809,1057,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1810,1058,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1811,1059,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1812,1060,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1813,1061,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1814,1037,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1815,1067,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1816,1068,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1817,1069,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1818,1070,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1819,1071,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1820,1027,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1821,1038,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1822,1097,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1823,1098,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1824,1099,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1825,1100,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1826,1101,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1827,1039,2,1,1,'2023-04-04 02:55:51');
INSERT INTO `sys_menu_auth` VALUES (1828,1102,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1829,1103,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1830,1104,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1831,1105,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1832,1106,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1833,1107,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1834,1136,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1835,1137,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1836,1138,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1837,1139,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1838,1140,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1839,1054,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1840,1055,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1841,1108,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1842,1109,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1843,1110,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1844,1111,2,1,1,'2023-04-04 02:55:52');
INSERT INTO `sys_menu_auth` VALUES (1845,1112,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1846,1056,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1847,1113,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1848,1114,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1849,1115,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1850,1116,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1851,1117,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1852,1148,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1853,1149,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1854,1231,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1855,1232,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1856,1233,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1857,1234,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1858,1235,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1859,1236,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1860,1150,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1861,1237,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1862,1238,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1863,1239,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1864,1240,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1865,1241,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1866,1242,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1867,1151,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1868,1243,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1869,1244,2,1,1,'2023-04-04 02:55:53');
INSERT INTO `sys_menu_auth` VALUES (1870,1245,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1871,1246,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1872,1247,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1873,1248,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1874,1152,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1875,1249,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1876,1250,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1877,1251,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1878,1252,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1879,1253,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1880,1254,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1881,1153,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1882,1255,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1883,1256,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1884,1257,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1885,1258,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1886,1259,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1887,1260,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1888,1154,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1889,1261,2,1,1,'2023-04-04 02:55:54');
INSERT INTO `sys_menu_auth` VALUES (1890,1262,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1891,1263,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1892,1264,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1893,1265,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1894,1266,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1895,1155,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1896,1267,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1897,1268,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1898,1269,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1899,1270,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1900,1271,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1901,1272,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1902,1156,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1903,1273,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1904,1274,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1905,1275,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1906,1276,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1907,1277,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1908,1278,2,1,1,'2023-04-04 02:55:55');
INSERT INTO `sys_menu_auth` VALUES (1909,1158,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1910,1159,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1911,1207,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1912,1208,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1913,1209,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1914,1210,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1915,1211,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1916,1212,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1917,1160,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1918,1213,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1919,1214,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1920,1215,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1921,1216,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1922,1217,2,1,1,'2023-04-04 02:55:56');
INSERT INTO `sys_menu_auth` VALUES (1923,1218,2,1,1,'2023-04-04 02:55:56');
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
INSERT INTO `sys_menu_auth` VALUES (2114,1,8,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2115,1059,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2116,2,8,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2118,1060,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2120,1000,8,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2121,1061,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2123,1001,8,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2124,1027,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2126,1002,8,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2127,1038,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2129,1003,8,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2130,1097,6,1,33,'2023-04-18 02:03:01');
INSERT INTO `sys_menu_auth` VALUES (2131,1,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2132,1004,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2134,1098,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2135,1005,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2137,2,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2138,1099,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2140,1000,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2141,1006,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2142,1100,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2143,1001,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2144,3,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2146,1101,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2147,1,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2149,1007,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2150,1002,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2151,1039,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2152,1003,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2153,1008,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2155,2,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2156,1102,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2157,1000,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2159,1009,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2160,1004,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2161,1103,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2162,1005,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2164,1010,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2165,1001,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2166,1104,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2167,1002,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2168,1011,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2170,1006,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2171,1105,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2173,4,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2174,3,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2175,1003,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2176,1106,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2177,1004,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2179,1012,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2180,1007,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2181,1107,6,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2182,1013,8,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2183,1008,9,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2185,1005,10,1,33,'2023-04-18 02:03:02');
INSERT INTO `sys_menu_auth` VALUES (2186,1136,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2187,1009,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2189,1014,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2190,1006,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2191,1137,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2192,3,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2193,1015,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2194,1010,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2196,1138,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2197,1011,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2199,5,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2200,1007,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2201,1139,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2202,1016,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2203,1008,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2205,4,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2206,1140,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2207,1012,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2209,1009,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2210,1017,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2211,1054,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2213,1010,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2214,1018,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2215,1013,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2216,1055,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2217,1014,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2218,1019,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2219,1011,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2221,1108,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2222,4,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2224,6,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2225,1015,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2226,1109,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2227,1020,8,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2228,5,9,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2230,1012,10,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2231,1110,6,1,33,'2023-04-18 02:03:03');
INSERT INTO `sys_menu_auth` VALUES (2233,1013,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2234,1016,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2235,1,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2236,1021,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2237,1111,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2238,1017,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2239,1022,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2240,1014,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2241,2,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2243,1112,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2245,1023,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2246,1000,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2247,1015,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2248,1018,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2249,1056,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2250,5,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2251,1019,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2252,1024,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2253,1001,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2255,1113,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2257,1002,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2258,1016,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2259,1029,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2260,6,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2261,1114,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2262,1020,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2263,1077,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2264,1017,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2265,1003,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2267,1115,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2268,1,12,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2270,1018,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2271,1078,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2272,1004,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2273,1021,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2274,1116,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2275,2,12,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2276,1022,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2277,1079,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2278,1019,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2280,1005,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2281,1000,12,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2282,1117,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2284,6,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2285,1080,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2286,1006,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2287,1023,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2288,1148,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2289,1001,12,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2290,1081,8,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2291,1024,9,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2292,1020,10,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2293,3,11,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2295,1149,6,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2296,1002,12,1,33,'2023-04-18 02:03:04');
INSERT INTO `sys_menu_auth` VALUES (2297,1021,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2299,1007,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2300,1029,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2301,1280,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2302,1003,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2303,1231,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2304,1310,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2305,1077,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2307,1008,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2308,1022,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2309,1232,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2310,1004,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2311,1023,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2313,1078,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2314,1009,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2315,1030,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2316,1005,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2317,1233,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2318,1079,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2319,1072,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2321,1010,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2322,1024,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2323,1234,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2324,1006,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2325,1029,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2327,1073,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2328,1011,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2329,1080,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2330,3,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2331,1235,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2332,1081,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2334,1074,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2335,4,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2336,1077,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2337,1236,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2338,1007,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2339,1078,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2340,1075,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2342,1012,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2343,1280,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2344,1008,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2345,1150,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2346,1076,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2347,1013,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2348,1310,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2349,1079,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2351,1237,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2352,1009,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2354,1030,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2355,1080,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2356,1014,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2357,1313,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2358,1010,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2359,1238,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2360,1122,8,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2361,1081,10,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2362,1072,9,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2363,1015,11,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2365,1239,6,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2366,1011,12,1,33,'2023-04-18 02:03:05');
INSERT INTO `sys_menu_auth` VALUES (2367,1073,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2368,1280,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2370,5,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2371,1123,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2372,1240,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2373,4,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2374,1124,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2376,1310,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2377,1016,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2378,1074,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2379,1241,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2380,1012,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2381,1075,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2382,1030,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2384,1017,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2385,1125,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2386,1013,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2387,1242,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2388,1126,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2389,1072,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2390,1018,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2392,1076,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2393,1151,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2394,1014,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2395,1313,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2397,1073,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2398,1019,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2399,1127,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2400,1243,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2401,1015,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2402,1279,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2404,1074,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2405,6,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2406,1122,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2407,1244,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2408,5,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2409,1123,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2410,1075,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2412,1020,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2413,1309,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2414,1245,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2415,1016,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2416,1076,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2417,1128,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2419,1021,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2420,1124,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2421,1017,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2422,1246,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2424,1125,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2425,1129,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2426,1022,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2427,1313,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2428,1018,12,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2429,1247,6,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2430,1130,8,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2431,1122,10,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2432,1126,9,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2433,1023,11,1,33,'2023-04-18 02:03:06');
INSERT INTO `sys_menu_auth` VALUES (2435,1019,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2436,1248,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2437,1024,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2439,1127,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2440,1131,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2441,1152,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2442,1,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2443,1123,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2444,6,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2445,1279,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2446,1132,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2448,1029,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2449,1124,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2450,1020,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2451,1249,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2452,2,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2454,1133,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2455,1077,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2456,1309,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2457,1250,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2458,1000,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2459,1021,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2460,1125,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2461,1307,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2462,1128,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2463,1078,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2465,1126,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2466,1001,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2467,1022,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2468,1251,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2470,1079,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2471,1129,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2472,1157,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2473,1002,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2474,1023,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2475,1127,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2476,1252,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2477,1080,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2478,1130,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2480,1219,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2481,1279,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2482,1253,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2483,1024,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2484,1003,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2485,1081,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2487,1131,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2488,1220,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2489,1309,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2490,1004,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2491,1029,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2492,1254,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2494,1280,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2495,1132,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2496,1221,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2497,1005,13,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2498,1153,6,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2499,1077,12,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2500,1128,10,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2502,1133,9,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2503,1310,11,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2504,1222,8,1,33,'2023-04-18 02:03:07');
INSERT INTO `sys_menu_auth` VALUES (2505,1006,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2506,1255,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2507,1129,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2508,1078,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2509,1307,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2511,1030,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2512,1223,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2513,1130,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2514,3,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2515,1256,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2516,1079,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2518,1157,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2519,1224,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2520,1072,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2521,1007,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2522,1257,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2523,1080,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2524,1131,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2525,1292,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2526,1219,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2527,1073,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2529,1258,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2530,1132,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2531,1081,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2532,1008,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2533,1220,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2535,1074,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2536,1293,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2537,1009,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2538,1133,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2539,1280,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2540,1259,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2542,1294,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2543,1075,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2544,1221,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2545,1307,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2546,1260,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2547,1310,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2548,1010,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2550,1222,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2551,1076,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2552,1295,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2553,1154,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2554,1011,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2555,1157,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2556,1030,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2557,1223,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2559,1313,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2560,1296,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2561,4,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2562,1219,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2563,1072,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2564,1261,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2566,1122,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2567,1297,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2568,1224,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2569,1073,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2570,1262,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2571,1220,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2572,1012,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2573,1123,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2574,1298,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2575,1292,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2577,1263,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2578,1013,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2579,1221,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2580,1074,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2581,1293,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2583,1299,8,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2584,1124,11,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2585,1014,13,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2586,1222,10,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2587,1264,6,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2588,1075,12,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2589,1300,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2590,1294,9,1,33,'2023-04-18 02:03:08');
INSERT INTO `sys_menu_auth` VALUES (2592,1125,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2593,1223,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2594,1076,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2595,1265,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2596,1015,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2597,1301,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2598,1295,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2600,1126,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2601,5,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2602,1224,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2603,1266,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2604,1313,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2606,1302,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2607,1296,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2608,1127,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2609,1155,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2610,1292,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2611,1122,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2612,1016,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2613,1297,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2614,1303,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2616,1279,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2617,1293,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2618,1017,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2619,1267,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2620,1123,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2622,1304,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2623,1298,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2624,1309,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2625,1294,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2626,1124,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2627,1268,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2628,1018,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2630,1305,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2631,1299,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2632,1128,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2633,1019,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2634,1269,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2635,1295,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2636,1125,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2637,1300,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2638,1306,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2640,1129,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2641,1296,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2642,1270,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2643,1126,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2644,6,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2646,1130,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2647,1165,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2648,1301,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2649,1020,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2650,1271,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2651,1127,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2652,1297,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2653,1166,8,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2654,1302,9,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2655,1131,11,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2657,1279,12,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2658,1298,10,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2659,1272,6,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2660,1021,13,1,33,'2023-04-18 02:03:09');
INSERT INTO `sys_menu_auth` VALUES (2661,1167,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2662,1132,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2664,1303,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2665,1299,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2666,1287,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2667,1309,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2668,1022,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2670,1304,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2671,1133,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2672,1168,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2673,1288,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2674,1300,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2675,1128,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2676,1023,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2677,1305,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2678,1307,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2679,1169,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2681,1301,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2682,1024,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2683,1129,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2684,1289,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2685,1170,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2686,1157,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2688,1306,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2689,1130,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2690,1290,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2691,1029,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2692,1302,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2693,1165,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2695,1219,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2696,1281,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2697,1291,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2698,1077,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2699,1131,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2700,1303,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2702,1220,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2703,1166,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2704,1312,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2705,1156,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2706,1078,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2707,1304,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2708,1132,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2709,1167,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2710,1314,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2711,1221,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2713,1305,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2714,1079,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2715,1133,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2716,1273,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2718,1025,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2719,1222,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2720,1168,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2721,1274,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2722,1080,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2723,1307,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2724,1306,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2725,1032,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2726,1169,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2727,1223,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2729,1081,13,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2730,1157,12,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2731,1275,6,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2732,1165,10,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2733,1170,9,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2735,1224,11,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2736,1087,8,1,33,'2023-04-18 02:03:10');
INSERT INTO `sys_menu_auth` VALUES (2737,1276,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2738,1166,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2739,1219,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2740,1280,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2742,1088,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2743,1292,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2744,1281,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2745,1167,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2746,1310,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2747,1220,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2748,1277,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2749,1312,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2750,1089,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2751,1293,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2753,1030,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2754,1278,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2755,1168,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2756,1221,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2757,1090,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2759,1314,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2760,1294,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2761,1169,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2762,1158,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2763,1222,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2764,1072,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2765,1025,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2767,1295,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2768,1091,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2769,1073,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2770,1223,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2771,1159,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2772,1170,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2774,1036,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2775,1032,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2776,1296,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2777,1207,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2778,1281,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2779,1074,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2780,1224,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2781,1087,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2782,1297,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2783,1062,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2785,1075,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2786,1312,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2787,1292,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2788,1208,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2789,1088,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2790,1063,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2791,1298,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2793,1209,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2794,1314,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2795,1,14,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2796,1293,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2797,1076,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2798,1089,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2800,1064,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2801,1299,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2802,1025,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2803,1313,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2804,1294,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2805,2,14,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2806,1210,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2807,1065,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2809,1300,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2810,1090,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2811,1122,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2812,1032,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2813,1295,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2814,1000,14,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2815,1211,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2816,1091,9,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2818,1301,11,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2819,1066,8,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2820,1212,6,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2821,1087,10,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2822,1001,14,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2823,1296,12,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2824,1123,13,1,33,'2023-04-18 02:03:11');
INSERT INTO `sys_menu_auth` VALUES (2825,1161,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2827,1302,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2828,1036,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2829,1124,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2830,1088,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2831,1160,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2832,1002,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2833,1297,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2834,1062,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2836,1303,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2837,1171,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2838,1213,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2839,1089,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2840,1003,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2841,1298,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2842,1125,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2843,1172,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2845,1304,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2846,1063,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2847,1004,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2848,1126,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2849,1214,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2850,1090,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2851,1299,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2853,1064,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2854,1305,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2855,1173,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2856,1174,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2857,1091,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2859,1065,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2860,1127,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2861,1215,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2862,1300,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2863,1306,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2864,1005,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2865,1175,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2866,1066,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2868,1279,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2869,1006,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2870,1165,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2871,1,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2872,1216,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2873,1301,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2874,1036,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2875,1176,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2876,1062,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2877,1217,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2878,1302,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2879,1309,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2881,2,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2882,1166,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2883,3,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2884,1161,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2885,1,16,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2886,1162,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2887,1171,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2888,1128,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2890,1007,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2891,1167,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2892,1218,6,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2893,1303,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2894,1000,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2895,1063,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2896,1177,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2897,2,16,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2898,1064,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2899,1168,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2900,1129,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2902,1001,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2903,1304,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2904,1008,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2905,1172,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2906,1000,16,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2907,1178,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2908,1173,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2909,1130,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2911,1009,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2912,1305,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2913,1169,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2914,1002,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2915,1065,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2916,1179,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2917,1001,16,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2918,1066,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2919,1003,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2920,1170,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2921,1306,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2923,1010,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2924,1131,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2925,1174,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2926,1002,16,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2927,1180,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2928,1175,9,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2929,1132,13,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2931,1011,14,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2932,1281,11,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2933,1165,12,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2934,1004,15,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2935,1161,10,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2936,1181,8,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2937,1003,16,1,33,'2023-04-18 02:03:12');
INSERT INTO `sys_menu_auth` VALUES (2938,1171,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2939,1005,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2940,1166,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2942,1133,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2943,1312,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2944,4,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2945,1176,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2946,1004,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2947,1182,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2948,1307,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2949,1162,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2951,1012,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2952,1314,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2953,1167,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2954,1006,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2955,1172,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2956,1282,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2957,1005,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2958,1173,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2960,1168,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2961,3,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2962,1025,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2963,1013,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2964,1177,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2965,1157,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2966,1006,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2967,1283,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2968,1219,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2969,1032,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2970,1014,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2971,1178,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2973,1169,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2974,1174,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2975,1007,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2976,3,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2977,1284,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2979,1175,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2980,1008,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2981,1170,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2982,1015,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2983,1087,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2984,1220,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2985,1179,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2986,1285,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2987,1007,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2988,1180,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2989,1221,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2990,1088,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2991,5,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2992,1281,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2993,1176,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2994,1009,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2996,1008,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2997,1286,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (2999,1162,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3000,1010,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3001,1312,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3002,1222,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3003,1016,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3004,1089,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3005,1181,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3006,1026,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3007,1009,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3008,1182,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3009,1223,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3010,1090,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3011,1017,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3012,1177,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3013,1314,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3014,1011,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3016,1010,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3017,1033,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3019,1178,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3020,1025,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3021,4,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3022,1018,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3023,1091,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3024,1224,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3025,1282,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3026,1043,8,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3027,1011,16,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3028,1283,9,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3029,1292,13,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3030,1036,11,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3031,1012,15,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3032,1179,10,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3033,1019,14,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3034,1032,12,1,33,'2023-04-18 02:03:13');
INSERT INTO `sys_menu_auth` VALUES (3036,4,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3037,1044,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3039,1180,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3040,1293,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3041,6,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3042,1087,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3043,1013,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3044,1062,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3045,1284,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3046,1045,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3047,1012,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3048,1285,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3049,1294,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3050,1063,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3051,1088,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3052,1014,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3053,1181,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3054,1020,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3056,1052,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3057,1013,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3059,1182,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3060,1021,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3061,1015,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3062,1089,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3063,1295,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3064,1064,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3065,1286,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3066,1014,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3067,1035,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3068,1296,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3069,1026,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3070,1065,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3071,1090,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3072,5,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3073,1282,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3074,1022,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3076,1057,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3077,1015,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3079,1283,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3080,1023,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3081,1016,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3082,1033,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3083,1091,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3084,1066,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3085,1297,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3086,5,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3087,1058,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3088,1298,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3089,1043,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3090,1161,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3091,1036,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3092,1284,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3093,1017,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3094,1024,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3096,1059,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3097,1016,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3099,1029,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3100,1285,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3101,1018,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3102,1044,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3103,1062,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3104,1171,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3105,1299,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3106,1017,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3107,1060,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3108,1300,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3109,1045,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3110,1286,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3111,1172,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3112,1019,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3113,1063,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3114,1077,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3116,1061,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3117,1018,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3119,1078,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3120,1026,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3121,1052,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3122,1064,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3123,6,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3124,1173,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3125,1301,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3126,1019,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3127,1027,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3128,1302,13,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3129,1035,9,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3130,1174,11,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3131,1033,10,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3132,1020,15,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3133,1065,12,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3134,1079,14,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3136,1038,8,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3137,6,16,1,33,'2023-04-18 02:03:14');
INSERT INTO `sys_menu_auth` VALUES (3139,1043,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3140,1080,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3141,1066,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3142,1021,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3143,1057,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3144,1175,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3145,1303,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3146,1097,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3147,1020,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3148,1081,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3149,1022,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3150,1161,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3151,1304,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3152,1176,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3153,1058,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3155,1044,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3156,1021,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3157,1098,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3158,1045,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3160,1059,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3161,1305,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3162,1162,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3163,1171,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3164,1023,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3165,1280,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3166,1099,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3167,1022,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3168,1024,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3169,1172,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3170,1310,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3171,1306,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3172,1060,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3174,1177,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3175,1052,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3176,1023,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3177,1100,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3179,1035,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3180,1178,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3181,1165,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3182,1061,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3183,1173,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3184,1030,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3185,1029,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3186,1101,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3187,1024,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3188,1027,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3189,1072,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3190,1166,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3191,1077,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3192,1174,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3193,1057,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3194,1179,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3196,1029,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3197,1039,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3198,1038,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3199,1102,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3201,1058,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3202,1077,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3203,1180,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3204,1167,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3205,1078,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3206,1175,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3207,1073,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3208,1097,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3209,1168,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3210,1079,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3211,1059,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3212,1074,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3213,1176,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3215,1181,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3216,1078,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3217,1103,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3218,1098,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3219,1104,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3221,1079,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3222,1182,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3223,1060,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3224,1075,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3225,1162,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3226,1080,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3227,1169,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3228,1099,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3229,1076,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3230,1282,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3231,1061,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3233,1177,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3234,1080,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3235,1170,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3236,1105,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3237,1081,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3238,1100,9,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3240,1106,8,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3241,1281,13,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3242,1280,15,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3243,1178,12,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3244,1081,16,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3245,1283,11,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3246,1313,14,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3247,1027,10,1,33,'2023-04-18 02:03:15');
INSERT INTO `sys_menu_auth` VALUES (3248,1101,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3249,1038,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3250,1284,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3251,1280,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3252,1179,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3253,1122,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3254,1312,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3255,1310,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3256,1107,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3258,1039,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3259,1136,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3261,1314,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3262,1030,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3263,1123,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3264,1310,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3265,1180,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3266,1285,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3267,1097,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3268,1102,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3269,1098,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3270,1181,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3271,1124,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3272,1286,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3273,1030,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3274,1072,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3276,1137,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3277,1025,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3278,1103,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3279,1138,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3280,1032,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3282,1072,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3283,1073,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3284,1026,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3285,1125,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3286,1182,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3287,1099,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3288,1104,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3289,1100,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3290,1282,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3292,1126,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3293,1033,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3294,1074,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3295,1087,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3296,1073,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3297,1139,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3298,1105,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3299,1140,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3301,1074,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3302,1075,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3303,1088,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3304,1043,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3305,1127,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3306,1283,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3307,1101,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3308,1106,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3309,1039,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3310,1284,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3311,1089,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3312,1044,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3313,1076,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3314,1279,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3316,1075,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3317,1054,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3318,1107,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3320,1055,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3321,1076,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3322,1090,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3323,1309,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3324,1313,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3325,1045,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3326,1285,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3327,1102,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3328,1136,9,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3329,1103,10,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3330,1091,13,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3331,1286,12,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3332,1128,14,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3333,1122,15,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3334,1052,11,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3335,1313,16,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3336,1108,8,1,33,'2023-04-18 02:03:16');
INSERT INTO `sys_menu_auth` VALUES (3338,1137,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3339,1109,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3341,1122,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3342,1036,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3343,1035,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3344,1123,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3345,1026,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3346,1129,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3347,1104,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3348,1138,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3349,1105,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3350,1062,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3351,1033,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3352,1130,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3353,1124,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3354,1057,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3356,1123,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3357,1110,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3358,1139,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3360,1111,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3361,1124,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3362,1058,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3363,1063,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3364,1125,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3365,1131,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3366,1043,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3367,1106,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3368,1140,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3369,1107,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3370,1064,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3371,1044,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3372,1132,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3373,1126,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3374,1059,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3375,1125,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3376,1112,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3378,1054,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3379,1056,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3381,1060,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3382,1133,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3383,1127,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3384,1126,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3385,1065,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3386,1045,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3387,1136,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3388,1055,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3389,1066,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3390,1137,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3391,1052,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3392,1279,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3393,1127,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3395,1307,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3396,1061,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3397,1113,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3398,1108,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3399,1114,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3400,1157,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3401,1027,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3403,1279,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3404,1309,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3405,1138,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3406,1035,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3407,1161,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3408,1109,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3409,1171,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3410,1139,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3412,1057,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3413,1128,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3414,1309,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3415,1038,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3416,1219,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3417,1115,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3418,1110,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3419,1116,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3420,1097,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3421,1128,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3422,1058,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3424,1129,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3425,1140,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3426,1172,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3427,1220,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3428,1111,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3429,1173,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3430,1054,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3432,1221,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3433,1059,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3434,1130,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3435,1129,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3436,1098,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3437,1117,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3438,1112,9,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3439,1148,8,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3440,1130,16,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3441,1099,11,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3443,1131,15,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3444,1060,12,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3445,1222,14,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3446,1055,10,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3447,1174,13,1,33,'2023-04-18 02:03:17');
INSERT INTO `sys_menu_auth` VALUES (3448,1056,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3450,1061,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3451,1175,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3452,1132,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3453,1108,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3454,1223,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3455,1131,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3456,1149,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3457,1100,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3458,1113,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3459,1231,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3460,1109,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3461,1224,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3462,1101,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3463,1132,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3464,1176,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3465,1133,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3466,1027,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3468,1114,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3470,1162,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3471,1038,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3472,1039,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3473,1307,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3474,1133,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3475,1292,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3476,1110,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3477,1232,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3478,1115,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3479,1233,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3480,1111,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3481,1157,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3482,1102,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3483,1097,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3484,1293,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3485,1177,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3486,1307,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3488,1116,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3489,1178,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3491,1157,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3492,1294,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3493,1098,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3494,1103,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3495,1112,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3496,1219,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3497,1234,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3498,1117,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3499,1235,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3500,1056,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3501,1220,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3502,1104,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3503,1099,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3504,1295,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3505,1219,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3507,1179,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3508,1148,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3509,1180,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3511,1220,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3512,1296,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3513,1100,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3514,1105,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3515,1113,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3516,1221,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3517,1236,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3518,1149,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3519,1150,8,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3520,1114,10,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3521,1222,15,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3522,1101,12,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3523,1106,11,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3525,1297,14,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3526,1221,16,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3527,1181,13,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3528,1231,9,1,33,'2023-04-18 02:03:18');
INSERT INTO `sys_menu_auth` VALUES (3529,1182,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3531,1222,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3532,1107,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3533,1039,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3534,1298,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3535,1115,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3536,1223,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3537,1237,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3538,1232,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3540,1282,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3541,1233,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3542,1238,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3543,1116,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3544,1224,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3545,1299,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3546,1102,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3547,1136,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3548,1223,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3549,1283,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3551,1103,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3552,1137,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3553,1300,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3554,1224,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3555,1239,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3556,1117,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3557,1292,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3558,1234,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3560,1284,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3561,1235,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3562,1240,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3563,1148,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3564,1293,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3565,1292,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3566,1301,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3567,1138,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3568,1104,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3569,1285,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3571,1105,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3572,1139,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3573,1149,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3574,1293,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3575,1302,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3576,1294,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3577,1241,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3578,1236,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3580,1286,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3581,1150,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3582,1242,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3583,1295,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3584,1231,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3585,1303,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3586,1294,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3587,1140,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3588,1106,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3589,1026,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3591,1054,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3592,1232,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3593,1107,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3594,1295,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3595,1304,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3596,1151,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3597,1296,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3598,1237,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3600,1033,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3601,1238,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3602,1243,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3603,1297,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3604,1305,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3605,1233,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3606,1296,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3607,1136,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3608,1055,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3609,1043,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3611,1234,10,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3612,1137,12,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3613,1108,11,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3614,1297,16,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3615,1306,14,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3616,1244,8,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3617,1298,15,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3618,1239,9,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3620,1044,13,1,33,'2023-04-18 02:03:19');
INSERT INTO `sys_menu_auth` VALUES (3621,1240,9,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3622,1245,8,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3623,1299,15,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3624,1165,14,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3625,1298,16,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3626,1138,12,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3627,1109,11,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3628,1235,10,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3629,1045,13,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3631,1236,10,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3632,1110,11,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3633,1139,12,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3634,1299,16,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3635,1246,8,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3636,1300,15,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3637,1166,14,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3638,1241,9,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3640,1052,13,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3641,1242,9,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3642,1247,8,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3643,1167,14,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3644,1301,15,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3645,1140,12,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3646,1300,16,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3647,1111,11,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3648,1150,10,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3649,1035,13,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3651,1112,11,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3652,1054,12,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3653,1302,15,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3654,1248,8,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3655,1237,10,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3656,1168,14,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3657,1151,9,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3658,1301,16,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3659,1057,13,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3660,1238,10,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3661,1243,9,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3662,1152,8,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3663,1303,15,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3664,1169,14,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3665,1302,16,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3666,1056,11,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3667,1055,12,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3668,1058,13,1,33,'2023-04-18 02:03:20');
INSERT INTO `sys_menu_auth` VALUES (3669,1249,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3670,1113,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3671,1170,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3672,1303,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3673,1108,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3674,1304,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3675,1244,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3676,1239,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3677,1059,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3678,1240,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3679,1245,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3680,1305,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3681,1109,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3682,1304,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3683,1281,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3684,1114,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3685,1250,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3686,1060,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3687,1251,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3688,1115,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3689,1312,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3690,1110,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3691,1305,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3692,1306,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3693,1246,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3694,1241,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3695,1061,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3696,1242,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3697,1247,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3698,1165,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3699,1111,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3700,1306,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3701,1314,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3702,1116,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3703,1252,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3704,1027,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3705,1253,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3706,1025,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3707,1165,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3708,1117,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3709,1248,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3710,1112,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3711,1166,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3712,1151,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3713,1038,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3714,1243,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3715,1056,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3716,1167,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3717,1148,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3718,1166,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3719,1032,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3720,1152,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3721,1254,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3722,1097,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3723,1153,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3724,1249,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3725,1087,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3726,1149,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3727,1168,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3728,1167,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3729,1113,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3730,1244,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3731,1098,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3732,1245,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3733,1114,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3734,1231,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3735,1088,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3736,1168,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3737,1169,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3738,1250,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3739,1255,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3740,1099,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3741,1256,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3742,1251,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3743,1170,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3744,1169,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3745,1232,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3746,1089,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3747,1115,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3748,1246,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3749,1100,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3750,1247,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3751,1090,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3752,1233,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3753,1281,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3754,1252,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3755,1170,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3756,1116,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3757,1257,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3758,1101,13,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3759,1258,8,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3760,1253,9,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3761,1117,12,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3762,1281,16,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3763,1312,15,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3764,1234,11,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3765,1091,14,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3766,1248,10,1,33,'2023-04-18 02:03:21');
INSERT INTO `sys_menu_auth` VALUES (3767,1039,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3768,1152,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3769,1036,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3770,1235,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3771,1254,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3772,1314,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3773,1312,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3774,1148,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3775,1259,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3776,1102,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3777,1260,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3778,1149,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3779,1314,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3780,1153,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3781,1025,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3782,1236,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3783,1062,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3784,1249,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3785,1103,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3786,1250,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3787,1255,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3788,1150,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3789,1063,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3790,1032,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3791,1025,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3792,1231,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3793,1154,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3794,1104,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3795,1261,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3796,1232,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3797,1256,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3798,1087,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3799,1032,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3800,1064,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3801,1237,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3802,1251,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3803,1105,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3804,1252,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3805,1257,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3806,1238,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3807,1065,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3808,1087,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3809,1233,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3810,1088,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3811,1262,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3812,1106,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3813,1263,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3814,1089,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3815,1088,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3816,1234,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3817,1258,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3818,1066,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3819,1239,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3820,1253,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3821,1107,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3822,1254,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3823,1259,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3824,1240,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3825,1161,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3826,1089,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3827,1235,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3828,1090,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3829,1264,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3830,1136,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3831,1265,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3832,1171,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3833,1236,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3834,1090,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3835,1091,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3836,1260,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3837,1241,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3838,1153,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3839,1137,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3840,1255,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3841,1154,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3842,1242,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3843,1150,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3844,1036,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3845,1091,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3846,1172,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3847,1266,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3848,1138,13,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3849,1155,8,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3850,1173,14,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3851,1062,15,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3852,1237,12,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3853,1036,16,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3854,1261,9,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3855,1151,11,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3856,1256,10,1,33,'2023-04-18 02:03:22');
INSERT INTO `sys_menu_auth` VALUES (3857,1139,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3858,1257,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3859,1262,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3860,1243,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3861,1062,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3862,1238,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3863,1063,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3864,1174,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3865,1267,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3866,1140,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3867,1268,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3868,1239,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3869,1064,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3870,1263,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3871,1244,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3872,1175,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3873,1258,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3874,1063,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3875,1054,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3876,1264,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3877,1176,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3878,1245,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3879,1065,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3880,1240,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3881,1269,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3882,1055,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3883,1064,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3884,1259,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3885,1066,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3886,1246,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3887,1270,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3888,1162,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3889,1265,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3890,1241,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3891,1260,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3892,1065,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3893,1108,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3894,1266,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3895,1271,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3896,1177,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3897,1242,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3898,1247,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3899,1161,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3900,1109,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3901,1066,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3902,1154,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3903,1248,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3904,1171,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3905,1151,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3906,1272,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3907,1178,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3908,1155,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3909,1261,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3910,1161,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3911,1110,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3912,1267,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3913,1287,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3914,1179,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3915,1243,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3916,1172,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3917,1152,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3918,1111,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3919,1171,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3920,1262,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3921,1249,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3922,1173,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3923,1244,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3924,1180,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3925,1288,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3926,1268,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3927,1263,10,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3928,1112,13,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3929,1172,16,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3930,1269,9,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3931,1289,8,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3932,1245,12,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3933,1181,14,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3934,1174,15,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3935,1250,11,1,33,'2023-04-18 02:03:23');
INSERT INTO `sys_menu_auth` VALUES (3936,1056,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3937,1173,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3938,1264,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3939,1175,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3940,1251,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3941,1290,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3942,1182,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3943,1246,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3944,1270,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3945,1265,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3946,1174,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3947,1113,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3948,1271,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3949,1291,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3950,1282,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3951,1247,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3952,1252,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3953,1176,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3954,1114,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3955,1175,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3956,1266,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3957,1253,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3958,1162,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3959,1248,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3960,1283,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3961,1156,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3962,1272,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3963,1155,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3964,1176,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3965,1115,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3966,1273,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3967,1287,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3968,1152,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3969,1284,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3970,1177,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3971,1254,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3972,1116,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3973,1162,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3974,1267,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3975,1178,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3976,1153,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3977,1288,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3978,1249,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3979,1285,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3980,1274,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3981,1268,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3982,1177,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3983,1117,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3984,1275,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3985,1289,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3986,1250,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3987,1286,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3988,1255,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3989,1179,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3990,1148,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3991,1178,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3992,1269,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3993,1180,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3994,1256,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3995,1026,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3996,1290,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3997,1251,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3998,1276,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (3999,1270,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4000,1179,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4001,1149,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4002,1277,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4003,1291,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4004,1252,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4005,1033,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4006,1257,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4007,1181,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4008,1231,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4009,1180,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4010,1271,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4011,1182,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4012,1156,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4013,1258,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4014,1043,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4015,1253,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4016,1278,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4017,1272,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4018,1181,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4019,1232,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4020,1158,8,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4021,1254,12,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4022,1044,14,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4023,1259,11,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4024,1273,9,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4025,1282,15,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4026,1233,13,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4027,1182,16,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4028,1287,10,1,33,'2023-04-18 02:03:24');
INSERT INTO `sys_menu_auth` VALUES (4029,1274,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4030,1283,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4031,1260,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4032,1045,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4033,1153,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4034,1159,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4035,1288,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4036,1282,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4037,1234,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4038,1207,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4039,1255,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4040,1052,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4041,1154,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4042,1284,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4043,1275,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4044,1235,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4045,1283,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4046,1289,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4047,1276,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4048,1285,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4049,1035,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4050,1261,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4051,1256,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4052,1208,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4053,1290,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4054,1284,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4055,1236,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4056,1209,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4057,1257,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4058,1057,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4059,1262,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4060,1286,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4061,1277,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4062,1150,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4063,1285,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4064,1291,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4065,1278,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4066,1026,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4067,1263,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4068,1058,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4069,1258,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4070,1210,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4071,1156,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4072,1286,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4073,1237,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4074,1211,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4075,1259,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4076,1059,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4077,1264,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4078,1033,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4079,1158,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4080,1238,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4081,1026,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4082,1273,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4083,1159,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4084,1260,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4085,1043,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4086,1265,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4087,1060,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4088,1212,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4089,1274,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4090,1033,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4091,1239,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4092,1160,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4093,1061,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4094,1044,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4095,1154,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4096,1266,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4097,1207,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4098,1240,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4099,1043,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4100,1275,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4101,1208,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4102,1155,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4103,1261,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4104,1027,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4105,1045,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4106,1213,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4107,1276,10,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4108,1044,16,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4109,1241,13,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4110,1052,15,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4111,1262,12,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4112,1038,14,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4113,1267,11,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4114,1209,9,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4115,1214,8,1,33,'2023-04-18 02:03:25');
INSERT INTO `sys_menu_auth` VALUES (4116,1242,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4117,1045,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4118,1277,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4119,1215,8,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4120,1210,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4121,1097,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4122,1263,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4123,1268,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4124,1035,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4125,1278,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4126,1052,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4127,1151,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4128,1057,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4129,1269,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4130,1211,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4131,1264,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4132,1098,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4133,1216,8,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4134,1243,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4135,1035,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4136,1158,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4137,1217,8,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4138,1212,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4139,1099,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4140,1265,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4141,1270,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4142,1058,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4143,1159,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4144,1057,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4145,1244,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4146,1059,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4147,1266,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4148,1100,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4149,1271,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4150,1160,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4151,1218,8,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4152,1245,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4153,1058,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4154,1207,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4155,1213,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4156,1101,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4157,1272,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4158,1155,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4159,1060,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4160,1208,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4161,1059,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4162,1246,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4163,1061,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4164,1267,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4165,1287,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4166,1039,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4167,1214,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4168,1247,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4169,1060,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4170,1209,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4171,1215,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4172,1288,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4173,1102,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4174,1268,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4175,1027,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4176,1210,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4177,1061,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4178,1248,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4179,1269,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4180,1103,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4181,1038,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4182,1289,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4183,1216,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4184,1152,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4185,1027,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4186,1211,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4187,1217,9,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4188,1097,15,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4189,1290,11,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4190,1104,14,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4191,1270,12,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4192,1212,10,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4193,1038,16,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4194,1249,13,1,33,'2023-04-18 02:03:26');
INSERT INTO `sys_menu_auth` VALUES (4195,1271,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4196,1105,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4197,1291,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4198,1098,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4199,1218,9,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4200,1250,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4201,1097,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4202,1160,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4203,1099,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4204,1106,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4205,1156,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4206,1272,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4207,1213,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4208,1098,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4209,1251,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4210,1287,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4211,1273,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4212,1107,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4213,1100,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4214,1252,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4215,1099,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4216,1214,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4217,1101,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4218,1136,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4219,1274,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4220,1288,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4221,1253,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4222,1289,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4223,1215,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4224,1275,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4225,1039,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4226,1137,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4227,1100,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4228,1254,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4229,1102,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4230,1138,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4231,1101,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4232,1276,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4233,1216,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4234,1290,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4235,1153,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4236,1217,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4237,1291,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4238,1277,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4239,1039,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4240,1139,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4241,1103,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4242,1255,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4243,1140,14,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4244,1104,15,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4245,1102,16,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4246,1278,11,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4247,1156,12,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4248,1218,10,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4249,1256,13,1,33,'2023-04-18 02:03:27');
INSERT INTO `sys_menu_auth` VALUES (4250,1273,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4251,1103,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4252,1158,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4253,1105,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4254,1054,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4255,1257,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4256,1055,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4257,1106,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4258,1159,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4259,1104,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4260,1274,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4261,1258,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4262,1105,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4263,1275,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4264,1207,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4265,1107,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4266,1108,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4267,1259,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4268,1109,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4269,1136,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4270,1208,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4271,1276,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4272,1106,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4273,1260,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4274,1107,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4275,1277,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4276,1209,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4277,1137,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4278,1110,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4279,1154,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4280,1111,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4281,1138,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4282,1210,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4283,1278,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4284,1136,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4285,1261,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4286,1211,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4287,1137,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4288,1158,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4289,1139,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4290,1112,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4291,1262,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4292,1159,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4293,1140,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4294,1138,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4295,1056,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4296,1212,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4297,1263,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4298,1160,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4299,1139,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4300,1113,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4301,1054,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4302,1207,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4303,1264,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4304,1208,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4305,1114,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4306,1055,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4307,1140,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4308,1213,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4309,1265,13,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4310,1214,11,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4311,1054,16,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4312,1108,15,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4313,1115,14,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4314,1209,12,1,33,'2023-04-18 02:03:28');
INSERT INTO `sys_menu_auth` VALUES (4315,1266,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4316,1116,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4317,1055,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4318,1109,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4319,1215,11,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4320,1210,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4321,1155,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4322,1211,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4323,1110,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4324,1216,11,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4325,1108,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4326,1117,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4327,1267,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4328,1109,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4329,1148,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4330,1217,11,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4331,1111,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4332,1212,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4333,1268,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4334,1160,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4335,1149,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4336,1112,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4337,1218,11,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4338,1110,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4339,1269,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4340,1056,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4341,1111,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4342,1231,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4343,1213,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4344,1270,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4345,1112,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4346,1214,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4347,1232,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4348,1113,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4349,1271,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4350,1233,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4351,1114,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4352,1215,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4353,1056,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4354,1272,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4355,1113,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4356,1216,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4357,1115,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4358,1234,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4359,1287,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4360,1116,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4361,1235,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4362,1217,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4363,1114,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4364,1288,13,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4365,1115,16,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4366,1218,12,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4367,1236,14,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4368,1117,15,1,33,'2023-04-18 02:03:29');
INSERT INTO `sys_menu_auth` VALUES (4369,1289,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4370,1150,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4371,1148,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4372,1116,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4373,1290,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4374,1117,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4375,1149,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4376,1237,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4377,1291,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4378,1238,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4379,1148,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4380,1231,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4381,1156,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4382,1232,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4383,1149,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4384,1239,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4385,1273,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4386,1231,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4387,1240,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4388,1233,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4389,1274,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4390,1241,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4391,1234,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4392,1232,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4393,1275,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4394,1235,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4395,1233,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4396,1242,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4397,1276,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4398,1236,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4399,1151,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4400,1234,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4401,1277,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4402,1235,16,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4403,1243,14,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4404,1150,15,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4405,1,17,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4406,1278,13,1,33,'2023-04-18 02:03:30');
INSERT INTO `sys_menu_auth` VALUES (4407,2,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4408,1237,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4409,1244,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4410,1236,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4411,1158,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4412,1245,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4413,1150,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4414,1238,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4415,1000,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4416,1159,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4417,1001,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4418,1239,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4419,1237,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4420,1246,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4421,1207,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4422,1247,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4423,1240,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4424,1238,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4425,1002,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4426,1208,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4427,1003,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4428,1241,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4429,1239,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4430,1248,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4431,1209,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4432,1152,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4433,1240,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4434,1242,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4435,1004,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4436,1210,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4437,1005,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4438,1151,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4439,1241,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4440,1249,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4441,1211,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4442,1242,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4443,1250,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4444,1243,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4445,1006,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4446,1212,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4447,3,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4448,1244,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4449,1251,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4450,1151,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4451,1160,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4452,1243,16,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4453,1252,14,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4454,1245,15,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4455,1007,17,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4456,1213,13,1,33,'2023-04-18 02:03:31');
INSERT INTO `sys_menu_auth` VALUES (4457,1008,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4458,1253,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4459,1246,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4460,1244,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4461,1214,13,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4462,1247,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4463,1245,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4464,1254,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4465,1009,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4466,1215,13,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4467,1010,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4468,1153,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4469,1246,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4470,1248,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4471,1216,13,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4472,1247,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4473,1152,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4474,1255,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4475,1011,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4476,1217,13,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4477,1256,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4478,4,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4479,1249,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4480,1248,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4481,1218,13,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4482,1012,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4483,1250,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4484,1152,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4485,1257,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4486,1251,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4487,1013,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4488,1258,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4489,1249,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4490,1252,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4491,1014,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4492,1259,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4493,1250,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4494,1253,15,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4495,1251,16,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4496,1260,14,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4497,1015,17,1,33,'2023-04-18 02:03:32');
INSERT INTO `sys_menu_auth` VALUES (4498,1254,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4499,5,17,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4500,1154,14,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4501,1252,16,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4502,1153,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4503,1016,17,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4504,1253,16,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4505,1261,14,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4506,1255,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4507,1254,16,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4508,1262,14,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4509,1017,17,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4510,1256,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4511,1018,17,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4512,1263,14,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4513,1153,16,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4514,1257,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4515,1264,14,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4516,1255,16,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4517,1019,17,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4518,1258,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4519,1256,16,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4520,6,17,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4521,1265,14,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4522,1259,15,1,33,'2023-04-18 02:03:33');
INSERT INTO `sys_menu_auth` VALUES (4523,1257,16,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4524,1020,17,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4525,1266,14,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4526,1258,16,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4527,1021,17,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4528,1260,15,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4529,1155,14,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4530,1259,16,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4531,1022,17,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4532,1154,15,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4533,1267,14,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4534,1023,17,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4535,1261,15,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4536,1260,16,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4537,1268,14,1,33,'2023-04-18 02:03:34');
INSERT INTO `sys_menu_auth` VALUES (4538,1154,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4539,1262,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4540,1024,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4541,1269,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4542,1029,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4543,1261,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4544,1263,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4545,1270,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4546,1262,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4547,1264,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4548,1077,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4549,1271,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4550,1265,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4551,1078,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4552,1263,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4553,1272,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4554,1079,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4555,1264,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4556,1266,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4557,1287,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4558,1265,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4559,1155,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4560,1080,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4561,1081,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4562,1266,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4563,1267,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4564,1288,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4565,1280,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4566,1268,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4567,1289,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4568,1155,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4569,1310,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4570,1267,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4571,1290,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4572,1269,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4573,1030,17,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4574,1291,14,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4575,1270,15,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4576,1268,16,1,33,'2023-04-18 02:03:35');
INSERT INTO `sys_menu_auth` VALUES (4577,1072,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4578,1271,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4579,1269,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4580,1156,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4581,1073,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4582,1270,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4583,1273,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4584,1272,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4585,1074,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4586,1274,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4587,1287,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4588,1271,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4589,1075,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4590,1288,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4591,1272,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4592,1275,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4593,1076,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4594,1289,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4595,1276,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4596,1287,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4597,1313,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4598,1277,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4599,1290,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4600,1288,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4601,1122,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4602,1289,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4603,1291,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4604,1278,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4605,1123,17,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4606,1156,15,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4607,1158,14,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4608,1290,16,1,33,'2023-04-18 02:03:36');
INSERT INTO `sys_menu_auth` VALUES (4609,1124,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4610,1159,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4611,1291,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4612,1273,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4613,1125,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4614,1156,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4615,1274,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4616,1207,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4617,1126,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4618,1208,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4619,1275,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4620,1273,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4621,1127,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4622,1276,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4623,1274,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4624,1209,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4625,1279,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4626,1210,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4627,1275,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4628,1277,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4629,1309,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4630,1278,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4631,1276,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4632,1211,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4633,1128,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4634,1277,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4635,1212,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4636,1158,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4637,1129,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4638,1159,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4639,1160,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4640,1278,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4641,1130,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4642,1158,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4643,1213,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4644,1207,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4645,1131,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4646,1214,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4647,1159,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4648,1208,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4649,1132,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4650,1209,15,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4651,1207,16,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4652,1215,14,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4653,1133,17,1,33,'2023-04-18 02:03:37');
INSERT INTO `sys_menu_auth` VALUES (4654,1216,14,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4655,1208,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4656,1210,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4657,1307,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4658,1209,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4659,1211,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4660,1217,14,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4661,1157,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4662,1218,14,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4663,1212,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4664,1210,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4665,1219,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4666,1211,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4667,1160,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4668,1220,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4669,1213,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4670,1212,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4671,1221,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4672,1160,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4673,1214,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4674,1222,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4675,1215,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4676,1213,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4677,1223,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4678,1214,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4679,1216,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4680,1224,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4681,1215,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4682,1217,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4683,1292,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4684,1218,15,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4685,1216,16,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4686,1293,17,1,33,'2023-04-18 02:03:38');
INSERT INTO `sys_menu_auth` VALUES (4687,1217,16,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4688,1294,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4689,1218,16,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4690,1295,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4691,1296,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4692,1297,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4693,1298,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4694,1299,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4695,1300,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4696,1301,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4697,1302,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4698,1303,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4699,1304,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4700,1305,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4701,1306,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4702,1165,17,1,33,'2023-04-18 02:03:39');
INSERT INTO `sys_menu_auth` VALUES (4703,1166,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4704,1167,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4705,1168,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4706,1169,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4707,1170,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4708,1281,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4709,1312,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4710,1314,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4711,1025,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4712,1032,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4713,1087,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4714,1088,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4715,1089,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4716,1090,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4717,1091,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4718,1036,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4719,1062,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4720,1063,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4721,1064,17,1,33,'2023-04-18 02:03:40');
INSERT INTO `sys_menu_auth` VALUES (4722,1065,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4723,1066,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4724,1161,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4725,1171,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4726,1172,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4727,1173,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4728,1174,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4729,1175,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4730,1176,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4731,1162,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4732,1177,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4733,1178,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4734,1179,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4735,1180,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4736,1181,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4737,1182,17,1,33,'2023-04-18 02:03:41');
INSERT INTO `sys_menu_auth` VALUES (4738,1282,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4739,1283,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4740,1284,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4741,1285,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4742,1286,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4743,1026,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4744,1033,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4745,1043,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4746,1044,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4747,1045,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4748,1052,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4749,1035,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4750,1057,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4751,1058,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4752,1059,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4753,1060,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4754,1061,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4755,1027,17,1,33,'2023-04-18 02:03:42');
INSERT INTO `sys_menu_auth` VALUES (4756,1038,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4757,1097,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4758,1098,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4759,1099,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4760,1100,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4761,1101,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4762,1039,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4763,1102,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4764,1103,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4765,1104,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4766,1105,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4767,1106,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4768,1107,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4769,1136,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4770,1137,17,1,33,'2023-04-18 02:03:43');
INSERT INTO `sys_menu_auth` VALUES (4771,1138,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4772,1139,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4773,1140,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4774,1054,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4775,1055,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4776,1108,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4777,1109,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4778,1110,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4779,1111,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4780,1112,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4781,1056,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4782,1113,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4783,1114,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4784,1115,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4785,1116,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4786,1117,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4787,1148,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4788,1149,17,1,33,'2023-04-18 02:03:44');
INSERT INTO `sys_menu_auth` VALUES (4789,1231,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4790,1232,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4791,1233,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4792,1234,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4793,1235,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4794,1236,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4795,1150,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4796,1237,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4797,1238,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4798,1239,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4799,1240,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4800,1241,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4801,1242,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4802,1151,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4803,1243,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4804,1244,17,1,33,'2023-04-18 02:03:45');
INSERT INTO `sys_menu_auth` VALUES (4805,1245,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4806,1246,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4807,1247,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4808,1248,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4809,1152,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4810,1249,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4811,1250,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4812,1251,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4813,1252,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4814,1253,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4815,1254,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4816,1153,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4817,1255,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4818,1256,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4819,1257,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4820,1258,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4821,1259,17,1,33,'2023-04-18 02:03:46');
INSERT INTO `sys_menu_auth` VALUES (4822,1260,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4823,1154,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4824,1261,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4825,1262,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4826,1263,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4827,1264,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4828,1265,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4829,1266,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4830,1155,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4831,1267,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4832,1268,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4833,1269,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4834,1270,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4835,1271,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4836,1272,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4837,1287,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4838,1288,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4839,1289,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4840,1290,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4841,1291,17,1,33,'2023-04-18 02:03:47');
INSERT INTO `sys_menu_auth` VALUES (4842,1156,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4843,1273,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4844,1274,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4845,1275,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4846,1276,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4847,1277,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4848,1278,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4849,1158,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4850,1159,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4851,1207,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4852,1208,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4853,1209,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4854,1210,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4855,1211,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4856,1212,17,1,33,'2023-04-18 02:03:48');
INSERT INTO `sys_menu_auth` VALUES (4857,1160,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (4858,1213,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (4859,1214,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (4860,1215,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (4861,1216,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (4862,1217,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (4863,1218,17,1,33,'2023-04-18 02:03:49');
INSERT INTO `sys_menu_auth` VALUES (5245,1138,19,1,1,'2023-04-18 02:06:01');
INSERT INTO `sys_menu_auth` VALUES (5247,1139,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5249,1140,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5251,1054,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5253,1055,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5255,1108,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5257,1109,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5259,1110,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5261,1111,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5263,1112,19,1,1,'2023-04-18 02:06:02');
INSERT INTO `sys_menu_auth` VALUES (5265,1056,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5267,1113,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5269,1114,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5271,1115,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5273,1116,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5275,1117,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5277,1148,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5278,1149,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5279,1231,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5280,1232,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5281,1233,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5282,1234,19,1,1,'2023-04-18 02:06:03');
INSERT INTO `sys_menu_auth` VALUES (5283,1235,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5284,1236,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5285,1150,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5286,1237,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5287,1238,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5288,1239,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5289,1240,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5290,1241,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5291,1242,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5292,1151,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5293,1243,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5294,1244,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5295,1245,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5296,1246,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5297,1247,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5298,1248,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5299,1152,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5300,1249,19,1,1,'2023-04-18 02:06:04');
INSERT INTO `sys_menu_auth` VALUES (5301,1250,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5302,1251,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5303,1252,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5304,1253,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5305,1254,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5306,1153,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5307,1255,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5308,1256,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5309,1257,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5310,1258,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5311,1259,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5312,1260,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5313,1154,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5314,1261,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5315,1262,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5316,1263,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5317,1264,19,1,1,'2023-04-18 02:06:05');
INSERT INTO `sys_menu_auth` VALUES (5318,1265,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5319,1266,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5320,1155,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5321,1267,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5322,1268,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5323,1269,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5324,1270,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5325,1271,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5326,1272,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5327,1287,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5328,1288,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5329,1289,19,1,1,'2023-04-18 02:06:06');
INSERT INTO `sys_menu_auth` VALUES (5330,1290,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5331,1291,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5332,1156,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5333,1273,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5334,1274,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5335,1275,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5336,1276,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5337,1277,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5338,1278,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5339,1158,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5340,1159,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5341,1207,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5342,1208,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5343,1209,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5344,1210,19,1,1,'2023-04-18 02:06:07');
INSERT INTO `sys_menu_auth` VALUES (5345,1211,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5346,1212,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5347,1160,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5348,1213,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5349,1214,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5350,1215,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5351,1216,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5352,1217,19,1,1,'2023-04-18 02:06:08');
INSERT INTO `sys_menu_auth` VALUES (5353,1218,19,1,1,'2023-04-18 02:06:08');
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_position`
--

LOCK TABLES `sys_position` WRITE;
/*!40000 ALTER TABLE `sys_position` DISABLE KEYS */;
INSERT INTO `sys_position` VALUES (1,'董事长',NULL,5,0,1,NULL,'2022-11-08 13:56:35','2023-03-24 02:49:54',1);
INSERT INTO `sys_position` VALUES (2,'项目经理',NULL,2,0,1,NULL,'2022-11-08 13:56:56','2023-03-26 20:38:49',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role`
--

LOCK TABLES `sys_role` WRITE;
/*!40000 ALTER TABLE `sys_role` DISABLE KEYS */;
INSERT INTO `sys_role` VALUES (1,'超级管理员',1,0,1,0,'2022-11-08 11:26:24',NULL,NULL);
INSERT INTO `sys_role` VALUES (2,'车间管理',2,0,1,0,'2022-11-17 08:58:18','2023-04-04 02:55:46',1);
INSERT INTO `sys_role` VALUES (3,'生产报工',3,0,1,1,'2023-02-23 02:01:07','2023-04-04 02:36:35',1);
INSERT INTO `sys_role` VALUES (6,'质检组长',4,0,1,33,'2023-04-18 02:02:51',NULL,NULL);
INSERT INTO `sys_role` VALUES (8,'质检员',5,0,1,33,'2023-04-18 02:02:57',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (1,'admin','5794620ce8325bc4e7b67677da3daeaa','c5fc028a23cd41c696a004f9dba65bff','ADMIN',1,NULL,0,NULL,NULL,'13400000001','13400000001@qq.com',NULL,NULL,1,NULL,NULL,0,1,NULL,'2022-09-27 16:07:39','2023-04-19 20:53:20',1);
INSERT INTO `sys_user` VALUES (24,'txc','a445250568e2f7929e8cd1e7b2fff26e','99fef4d1addf4cc9b475f65f7ec35740','txcw',1,2,0,NULL,NULL,'13411111111','',NULL,NULL,NULL,'2222',NULL,0,1,1,'2022-12-26 16:13:18','2023-03-23 02:53:53',1);
INSERT INTO `sys_user` VALUES (27,'lisi','9438571d588b26e234709e6f975ba3e4','c84681f4183543f4910f9f34683f73d8','1',1,4,0,NULL,NULL,'18339475689','23459378@qq.com',NULL,NULL,NULL,'2',NULL,0,1,1,'2023-02-22 01:38:14',NULL,NULL);
INSERT INTO `sys_user` VALUES (28,'xiaoming','8df2f4462922bca05ea29b079065fc48','01266758ce8a46b09004a4185f6934b1','xt',2,2,0,NULL,NULL,'15634785930','22348340@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-02-22 01:40:03',NULL,NULL);
INSERT INTO `sys_user` VALUES (30,'小昭','e1b161f8020fc0190e98e8864cae55e7','f4f02efeedb4449d82da200ae48e1992','zzz111',9,4,0,NULL,NULL,'15614151678','15614151678@163.com',NULL,NULL,NULL,'111',NULL,0,1,1,'2023-03-23 02:55:24','2023-04-18 02:27:52',1);
INSERT INTO `sys_user` VALUES (32,'tony','c729d9309ebfc53041158593ddceafe4','933fffddb1b64f96ba4a8b3fba7ec204','tony',1,2,1,NULL,NULL,'18900002222','',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:41:31',NULL,NULL);
INSERT INTO `sys_user` VALUES (33,'yp','8082ae129b31d31ab60cf9496a4eeac5','bc440cb822324fda9fcd6b64ef46fbf7','yp',1,4,0,NULL,NULL,'18900000000','yp@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:50:52','2023-04-19 17:03:58',1);
INSERT INTO `sys_user` VALUES (34,'yp1','2caa82251c047aac1562c22600d30121','116f35c84d344addb7d7bb5f5b972b13','yp1',NULL,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,33,'2023-04-18 02:09:21',NULL,NULL);
INSERT INTO `sys_user` VALUES (35,'yp2','efcdb895983cb6fa6443212a208c1681','24eb81b8b3ec42919ae3b01f111df643','yp2',NULL,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,33,'2023-04-18 02:09:42',NULL,NULL);
INSERT INTO `sys_user` VALUES (36,'zyy','1bb7f30db2bce90ade4ec1a3a79bd96a','b6c79120a9b64610a45a124b7aae26eb','zyy',9,4,0,NULL,NULL,'15613141385','15613141385@qq.com',NULL,NULL,NULL,'测试使用',NULL,0,1,1,'2023-04-19 02:38:06',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (3,24,2,1,'2022-12-26 16:13:18');
INSERT INTO `sys_user_role` VALUES (4,1,1,1,'2022-12-26 17:17:36');
INSERT INTO `sys_user_role` VALUES (7,27,2,1,'2023-02-22 01:38:14');
INSERT INTO `sys_user_role` VALUES (8,28,2,1,'2023-02-22 01:40:03');
INSERT INTO `sys_user_role` VALUES (10,30,2,1,'2023-03-23 02:55:24');
INSERT INTO `sys_user_role` VALUES (12,32,1,1,'2023-04-05 22:41:31');
INSERT INTO `sys_user_role` VALUES (13,33,1,1,'2023-04-05 22:50:52');
INSERT INTO `sys_user_role` VALUES (14,34,6,33,'2023-04-18 02:09:21');
INSERT INTO `sys_user_role` VALUES (15,35,8,33,'2023-04-18 02:09:42');
INSERT INTO `sys_user_role` VALUES (16,36,1,1,'2023-04-19 02:38:06');
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
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm`
--

LOCK TABLES `t_alarm` WRITE;
/*!40000 ALTER TABLE `t_alarm` DISABLE KEYS */;
INSERT INTO `t_alarm` VALUES (42,'2023-04-19 13:19:40','1','钻机扫条码',2,11,0,1,1,'2023-04-19 01:19:53',NULL,NULL,0,'');
INSERT INTO `t_alarm` VALUES (43,'2023-04-20 16:58:35','2','AGV移动',1,13,0,1,1,'2023-04-20 04:58:48',NULL,NULL,0,'');
INSERT INTO `t_alarm` VALUES (44,'2023-04-27 16:50:35','3','钻机请求配方',1,11,0,1,1,'2023-04-27 16:50:35',NULL,NULL,0,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm_setting`
--

LOCK TABLES `t_alarm_setting` WRITE;
/*!40000 ALTER TABLE `t_alarm_setting` DISABLE KEYS */;
INSERT INTO `t_alarm_setting` VALUES (12,'AGV电量水平低于30%',0,'LOWER_THAN_30','AGV电量水平低于30%',1,0,0,1,'2023-02-27 16:26:42','2023-04-18 05:22:10',1,57,'DRILL_REQUEST_LOAD_RAW_MATERIAL','11','大屏告警','AGV电量水平低于30%','0');
INSERT INTO `t_alarm_setting` VALUES (14,'AGV电量水平低于20%',0,'LOWER_THAN_20','AGV电量水平低于20%',2,0,1,1,'2023-02-27 16:26:42','2023-04-18 05:22:00',1,61,'DRILL_REQUEST_LOAD_RAW_MATERIAL','11,22','大屏告警,钉钉',' AGV电量水平低于20%','0');
INSERT INTO `t_alarm_setting` VALUES (15,'AGV电量水平低于10%',0,'LOWER_THAN_10','AGV电量水平低于10%',1,0,1,1,'2023-02-27 16:26:42','2023-04-21 04:58:17',1,64,'DRILL_REQUEST_LOAD_RAW_MATERIAL','21,22,11,12','微信,钉钉,大屏告警,短信','AGV电量水平低于10%11111111','0');
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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COMMENT='检验记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_check_records`
--

LOCK TABLES `t_check_records` WRITE;
/*!40000 ALTER TABLE `t_check_records` DISABLE KEYS */;
INSERT INTO `t_check_records` VALUES (13,16,NULL,NULL,0,NULL,NULL,1,'admin1','1','2023-04-19 00:50:07','合格品',1,0,1,'2023-04-12 15:06:18','2023-04-17 05:23:28',1);
INSERT INTO `t_check_records` VALUES (15,1,NULL,NULL,1,NULL,NULL,1,'1','-1','2023-04-19 00:49:43','抽检不合格',1,0,1,'2023-04-14 04:16:04','2023-04-17 05:23:22',1);
INSERT INTO `t_check_records` VALUES (16,54,'11','11',23,NULL,'PCB单层',NULL,NULL,'1','2023-04-21 01:20:38','合格品',1,0,1,'2023-04-18 01:11:59',NULL,NULL);
INSERT INTO `t_check_records` VALUES (17,54,'11','11',23,NULL,'PCB单层',NULL,NULL,'-1','2023-04-19 00:49:36',NULL,1,0,1,'2023-04-18 01:14:58',NULL,NULL);
INSERT INTO `t_check_records` VALUES (18,54,'11','11',23,NULL,'PCB单层',NULL,NULL,'0','2023-04-21 03:42:13',NULL,1,0,1,'2023-04-18 01:16:33',NULL,NULL);
INSERT INTO `t_check_records` VALUES (19,54,'T2522222','钻孔',67890,'MO202222','PCB单层',100025,'张三','1','2023-04-21 03:42:08','不合格',1,0,1,'2023-04-18 01:16:58','2023-04-18 01:43:12',1);
INSERT INTO `t_check_records` VALUES (20,63,'#0A1C23','#0A1C23',26,'MO202304020001','PCB单层',NULL,NULL,'1','2023-04-21 03:40:54','111',1,0,1,'2023-04-18 01:52:07',NULL,NULL);
INSERT INTO `t_check_records` VALUES (21,65,'#6E7F86','#6E7F86',26,'MO202304020001','PCB单层',NULL,NULL,'1','2023-04-21 01:21:49','强强强强1',1,0,1,'2023-04-18 02:01:04','2023-04-18 03:01:25',1);
INSERT INTO `t_check_records` VALUES (22,64,'#0A1C23','#0A1C23',25,'MO202304030001','PCB单层',NULL,NULL,'1',NULL,NULL,1,0,1,'2023-04-21 02:50:25',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COMMENT='客户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_client`
--

LOCK TABLES `t_client` WRITE;
/*!40000 ALTER TABLE `t_client` DISABLE KEYS */;
INSERT INTO `t_client` VALUES (41,'001','张老板','这是张老板',0,1,1,'2023-04-18 22:18:28','2023-04-20 22:01:56',1);
INSERT INTO `t_client` VALUES (44,'1','1','这是111',0,1,1,'2023-04-20 22:02:56',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='刀具';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter`
--

LOCK TABLES `t_cutter` WRITE;
/*!40000 ALTER TABLE `t_cutter` DISABLE KEYS */;
INSERT INTO `t_cutter` VALUES (18,'string1','string1',0,0,0,'string','string',0,0,'2023-03-29 02:53:56','string','string',0,1,1,'2023-03-29 02:53:56','2023-03-29 03:05:41',1);
INSERT INTO `t_cutter` VALUES (19,'string33333333','string3333333333333',2,3,3,'string','string',0,0,'2023-03-22 07:30:53','string','string',0,1,1,'2023-03-29 03:06:02','2023-04-14 01:40:55',1);
INSERT INTO `t_cutter` VALUES (26,'22221','1',1,11,11,'1','1',1,1,'2023-04-14 01:10:58','1',NULL,0,0,1,'2023-04-14 01:10:58','2023-04-14 01:40:47',1);
INSERT INTO `t_cutter` VALUES (27,'1','1',1,1,1,'1','1',1,1,'2023-04-14 04:33:09','1',NULL,0,1,1,'2023-04-14 04:33:09',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_detail`
--

LOCK TABLES `t_cutter_config_detail` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_detail` DISABLE KEYS */;
INSERT INTO `t_cutter_config_detail` VALUES (4,16,0.275,185.000,3.300,25.000,0.300,2000,0,1,1,'2023-03-28 16:46:30','2023-04-10 04:11:07',1);
INSERT INTO `t_cutter_config_detail` VALUES (5,16,0.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:09:22',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (6,16,0.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:10:08',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (21,16,3.000,3.000,3.000,3.000,3.000,0,0,1,1,'2023-03-29 23:31:01','2023-03-29 23:31:31',1);
INSERT INTO `t_cutter_config_detail` VALUES (22,16,2.000,2.000,2.000,2.000,2.000,0,0,1,1,'2023-03-29 23:31:04','2023-03-29 23:31:12',1);
INSERT INTO `t_cutter_config_detail` VALUES (23,16,-1.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:31:51','2023-03-29 23:42:52',1);
INSERT INTO `t_cutter_config_detail` VALUES (24,16,0.250,190.000,3.300,25.000,0.300,2100,0,1,1,'2023-03-29 23:41:01','2023-04-13 02:40:03',1);
INSERT INTO `t_cutter_config_detail` VALUES (25,16,0.200,190.000,3.210,25.000,0.400,2100,0,1,1,'2023-03-29 23:45:04','2023-04-13 03:07:25',1);
INSERT INTO `t_cutter_config_detail` VALUES (48,16,1.000,1.000,1.000,1.000,1.000,4,0,1,1,'2023-04-13 03:13:38','2023-04-13 03:47:33',1);
INSERT INTO `t_cutter_config_detail` VALUES (49,16,1.000,1.000,1.000,1.000,1.000,1,0,1,1,'2023-04-19 00:57:56','2023-04-19 00:58:07',1);
INSERT INTO `t_cutter_config_detail` VALUES (50,16,1.000,1.000,1.000,1.000,1.000,1,0,1,1,'2023-04-20 04:56:06',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (51,16,1.000,1.000,1.000,1.000,1.000,1,0,1,1,'2023-04-21 01:27:41',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数主表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_master`
--

LOCK TABLES `t_cutter_config_master` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_master` DISABLE KEYS */;
INSERT INTO `t_cutter_config_master` VALUES (3,'主轴转速200KRPM钻孔','主轴转速200KRPM钻孔','1111','\\\\fileServer\\IF200001111\\IF200001111.dia',8,'p05','柔性电路板',0,1,1,'2023-03-28 16:38:31','2023-04-12 21:08:43',1);
INSERT INTO `t_cutter_config_master` VALUES (16,'11','11','11','11',7,'p01','单面板',0,1,1,'2023-04-20 04:55:57','2023-04-21 01:31:25',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COMMENT='钻机排刀计划';
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
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  `parameters` varchar(5000) CHARACTER SET utf8 DEFAULT NULL COMMENT '设备参数',
  `device_status` varchar(255) DEFAULT NULL COMMENT '设备状态',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
INSERT INTO `t_device` VALUES (11,'drill01','drill01',NULL,0,0,1,1,'2023-02-28 11:40:13','2023-04-20 22:29:22',1,7,NULL,NULL,NULL,NULL,NULL,NULL,'{\n        \"ProductId\": \"agv\",\n        \"DeviceId\": \"AGV01\",\n        \"DeviceName\": \"AGV#01\",\n        \"DeviceClazz\": \"VgAutoDrill.Fundation.Vendor.Agvsz.AGV2530\",\n        \"AutoMode\": true,\n        \"DebugMode\": false,\n        \"DataCollectingPerSeconds\": 30,\n        \"KeepingPlcConnectionPerSeconds\": 30,\n        \"DeviceKind\": \"Auxiliary\",\n        \"InputCapabilities\": [ \"EmptySiloBox\", \"Raw\" ],\n        \"OutputCapabilities\": [ \"PRE_DRILL_TRANSFER_AGV_OUTPUT_1\", \"EmptySiloBox\" ],\n        \"Extra\": {\n          \"modbusTcpUri\": \"http://192.168.3.20:502\",\n          \"modbusTcpSlaveId\": 1,\n          \"MoveTimeout\": 30000,\n          \"PostAndGetTimeout\": 10000,\n          \"LowBattery\": 30,\n          \"AGVMoveStart\": \"http://192.168.3.15:9502/api/wcstask/AddTask\",\n          \"CarAllInfo\": \"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\n          \"RequestCharge\": \"http://192.168.3.15:9701/api/v2/CarState/SetToCharge\"\n        }\n      }','3');
INSERT INTO `t_device` VALUES (13,'agv01','agv01',NULL,0,0,1,1,'2023-02-28 11:40:21','2023-04-20 22:28:54',1,8,0,'string','string',0,NULL,NULL,'111111111','3');
INSERT INTO `t_device` VALUES (14,'MockAgv02','MockAgv02',NULL,0,0,0,1,'2023-03-03 00:33:58','2023-03-22 02:31:17',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'-1');
INSERT INTO `t_device` VALUES (15,'MockDrill01','MockDrill01',NULL,0,0,1,1,'2023-03-03 00:34:24','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,2,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (16,'P0001','P0001',NULL,0,0,1,1,'2023-03-03 01:32:19','2023-03-08 16:46:09',NULL,14,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (17,'MockAgv06','MockAgv06',NULL,0,0,2,1,'2023-03-07 10:20:48','2023-03-07 10:54:07',NULL,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (18,'MockAgv03','MockAgv03',NULL,0,0,1,1,'2023-03-07 10:21:29','2023-03-17 10:37:59',NULL,8,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (19,'MockAgv04','MockAgv04',NULL,0,0,1,1,'2023-03-07 17:09:12','2023-03-15 15:39:26',NULL,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'4');
INSERT INTO `t_device` VALUES (20,'P0002','P0002',NULL,0,0,1,1,'2023-03-07 20:19:06','2023-04-23 09:50:35',1,14,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (21,'MockAgv104','MockAgv104',NULL,0,0,0,1,'2023-03-09 10:15:32','2023-04-20 22:28:40',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (22,'MockAgv103','MockAgv103',NULL,0,0,1,1,'2023-03-09 10:15:32','2023-03-22 07:52:58',NULL,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (23,'MockDrill02','MockDrill02',NULL,0,0,1,1,'2023-03-13 13:54:18','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,4,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (24,'MockDrill03','MockDrill03',NULL,0,0,1,1,'2023-03-14 08:48:10','2023-03-20 11:46:56',NULL,7,NULL,NULL,NULL,5,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (25,'MockAgv01','MockAgv01',NULL,0,0,1,1,'2023-03-14 08:51:18','2023-04-17 11:34:58',NULL,8,NULL,NULL,NULL,1,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (26,'MockDrill04','MockDrill04',NULL,0,0,1,1,'2023-03-14 09:22:53','2023-03-20 11:46:56',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (27,'MockDrill05','MockDrill05',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:03',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (28,'MockDrill06','MockDrill06',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:02',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (29,'Processed0001','Processed0001',NULL,0,0,1,1,'2023-03-14 11:19:38','2023-03-16 10:25:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (30,'Processed10001','Processed10001',NULL,0,0,1,1,'2023-03-14 15:50:43','2023-03-20 14:21:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (31,'vegaDrill001','vegaDrill001',NULL,0,0,1,1,'2023-03-14 16:18:04','2023-03-14 16:27:45',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (32,'MockAgv1003','MockAgv1003',NULL,0,0,1,1,'2023-03-14 16:28:34','2023-03-14 16:33:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (33,'vegaDrill0601','vegaDrill0601',NULL,0,0,1,1,'2023-03-14 16:28:35','2023-03-14 16:33:37',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL,'4');
INSERT INTO `t_device` VALUES (34,'MockAgv11103','MockAgv11103',NULL,0,0,1,1,'2023-03-14 16:52:45','2023-03-15 08:43:45',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (35,'MockTonyAgv001','MockTonyAgv001',NULL,0,0,1,1,'2023-03-15 09:41:29','2023-03-22 23:45:47',1,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (36,'MockSimpleAgv001','MockSimpleAgv001',NULL,0,0,1,1,'2023-03-15 10:12:33','2023-03-16 14:30:31',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (37,'MockSimpleAgv002','MockSimpleAgv002',NULL,0,0,1,1,'2023-03-15 10:24:25','2023-03-15 10:36:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (38,'Processed002','Processed002',NULL,0,0,1,1,'2023-03-15 10:24:26','2023-03-15 10:36:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (39,'Raw10001','Raw10001',NULL,0,0,1,1,'2023-03-15 14:07:38','2023-03-15 14:28:20',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (40,'Processed100012','Processed100012',NULL,0,0,1,1,'2023-03-15 14:32:46','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (41,'Raw100012','Raw100012',NULL,0,0,1,1,'2023-03-15 14:32:47','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (42,'Processed1000122','Processed1000122',NULL,0,0,1,1,'2023-03-15 16:21:35','2023-03-15 17:58:59',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (43,'Raw1000123','Raw1000123',NULL,0,0,1,1,'2023-03-15 16:21:36','2023-03-23 01:42:59',1,13,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (44,'D0001','D0001',NULL,0,0,1,1,'2023-03-16 11:12:39','2023-03-23 01:43:17',1,13,NULL,NULL,NULL,1,NULL,NULL,NULL,'-1');
INSERT INTO `t_device` VALUES (45,'Processed10002','Processed10002',NULL,0,0,1,1,'2023-03-20 14:16:50','2023-03-22 23:46:24',1,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (46,'SiloShelf100012','SiloShelf100012',NULL,0,0,1,1,'2023-03-21 07:01:46','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (49,'UP000102','UP000102',NULL,0,0,1,1,'2023-03-22 07:51:36','2023-03-22 07:52:57',NULL,15,NULL,NULL,NULL,1,NULL,NULL,NULL,'2');
INSERT INTO `t_device` VALUES (51,'A00','A00',NULL,0,0,1,1,'2023-03-22 23:02:07','2023-03-23 01:17:05',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (52,'P00002','P00002',NULL,0,0,1,1,'2023-03-22 23:16:16','2023-03-23 01:36:23',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'4');
INSERT INTO `t_device` VALUES (54,'Q0001','Q0001',NULL,0,0,1,1,'2023-03-23 01:19:33','2023-03-23 02:21:09',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (55,'生料1','生料1',NULL,0,0,1,1,'2023-03-23 02:50:16','2023-03-23 02:50:29',1,16,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (56,'熟料1111111111111111111','熟料111111111111111',NULL,0,0,1,1,'2023-03-23 21:04:32','2023-03-29 03:19:39',1,17,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'3');
INSERT INTO `t_device` VALUES (57,'拆板机1','拆板机1',NULL,0,0,1,1,'2023-03-23 21:05:05',NULL,NULL,15,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'1');
INSERT INTO `t_device` VALUES (58,'叠板机1','叠板机1',NULL,0,0,1,1,'2023-03-23 21:05:22',NULL,NULL,14,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (62,'钻机1','钻机1',NULL,0,0,1,1,'2023-03-27 21:42:13',NULL,NULL,7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0');
INSERT INTO `t_device` VALUES (67,'22','22',NULL,0,0,1,1,'2023-04-20 22:16:20',NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'0','222','2');
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
  `maintain_time` datetime DEFAULT NULL COMMENT '维护时间',
  `maintain_person` varchar(100) DEFAULT NULL COMMENT '维护人',
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备负载板料表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_panel`
--

LOCK TABLES `t_device_panel` WRITE;
/*!40000 ALTER TABLE `t_device_panel` DISABLE KEYS */;
INSERT INTO `t_device_panel` VALUES (2,11,'string','123456787654321','string','string','string','string',0,0,0,NULL,1,0,1,'2023-04-20 21:14:43',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (3,13,'string','123456787654322','string','string','string','string',0,0,0,NULL,1,0,1,'2023-04-20 21:18:46',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (4,14,'string','P202304210001',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-27 08:15:23',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (5,17,NULL,'P202304210002',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-27 10:25:11',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (6,18,NULL,'P202304210003',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-27 10:34:46',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (7,19,NULL,'P202304210004',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-27 12:19:11',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (8,57,NULL,'P202304210005',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-27 15:19:11',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (9,58,NULL,'P202304210006',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-28 09:24:18',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (10,62,NULL,'P202304210007',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-28 11:27:31',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (11,11,NULL,'P202304210008',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,1,0,1,'2023-04-28 15:55:27',NULL,NULL);
/*!40000 ALTER TABLE `t_device_panel` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备类型';
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
INSERT INTO `t_device_type` VALUES (17,'熟料仓暂存台',1,0,0,1,'2023-03-23 02:46:28','2023-04-19 23:06:11',1,'ProcessedStagingDesk','0,1');
/*!40000 ALTER TABLE `t_device_type` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=243 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='事件管理';
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
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COMMENT='生产报工记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_feedback`
--

LOCK TABLES `t_feedback` WRITE;
/*!40000 ALTER TABLE `t_feedback` DISABLE KEYS */;
INSERT INTO `t_feedback` VALUES (28,'统一报工',212,'drill01-0001','钻机01-0001',21,'MO202304050003','PCB单层',21,'pin','叠板',38,'1',36,'IF2023040500002','PCB单层板',22,'Panel','DDD XXXX DD',NULL,110.00,84.00,26.00,'admin','txc',NULL,'2023-04-19 00:00:00','1111','COMMITED',NULL,0,1,1,'2023-04-18 23:28:50','2023-04-21 03:48:57',1);
INSERT INTO `t_feedback` VALUES (29,'自行报工',212,'drill01-0001','钻机01-0001',21,'MO202304050003','PCB单层',21,'pin','叠板',38,'1',36,'IF2023040500002','PCB单层板',22,'Panel','DDD XXXX DD',NULL,200.00,195.00,5.00,'yp','yp',NULL,'2023-04-11 00:00:00','wefbsdvf','DRAFT',NULL,0,1,1,'2023-04-20 04:53:18','2023-04-20 04:53:27',1);
INSERT INTO `t_feedback` VALUES (30,'自行报工',219,'unpin01-0001','拆板机01-0001',21,'MO202304050003','PCB单层',23,'unpin','拆板',29,'拆板1',36,'IF2023040500002','PCB单层板',22,'Panel','DDD XXXX DD',NULL,80.00,80.00,0.00,'yp',NULL,NULL,'2023-04-25 00:00:00','11111111111111','DRAFT',NULL,0,1,1,'2023-04-20 05:23:03','2023-04-20 05:23:15',1);
INSERT INTO `t_feedback` VALUES (31,'2',212,'drill01-0001','钻机01-0001',21,'MO202304050003','PCB单层',22,'drill','钻孔',40,'钻孔1',36,'IF2023040500002','PCB单层板',22,'Panel','DDD XXXX DD',NULL,130.00,119.00,21.00,'xiaoming','yp1',NULL,'2023-04-26 00:00:00',NULL,'DRAFT',NULL,0,1,1,'2023-04-21 01:05:40',NULL,NULL);
INSERT INTO `t_feedback` VALUES (32,'统一报工',218,'pin01-0001','叠板机01-0001',44,'MO202304270001','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,150.00,NULL,NULL,NULL,NULL,NULL,'2023-04-27 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (33,'统一报工',218,'pin01-0001','叠板机01-0001',45,'MO202304270002','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,145.00,NULL,NULL,NULL,NULL,NULL,'2023-04-28 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (34,'统一报工',218,'pin01-0001','叠板机01-0001',46,'MO202304270003','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,113.00,NULL,NULL,NULL,NULL,NULL,'2023-04-29 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (35,'统一报工',218,'pin01-0001','叠板机01-0001',47,'MO202304280001','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,160.00,NULL,NULL,NULL,NULL,NULL,'2023-05-04 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (36,'统一报工',218,'pin01-0001','叠板机01-0001',48,'MO202304280002','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,140.00,NULL,NULL,NULL,NULL,NULL,'2023-05-05 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (37,'统一报工',218,'pin01-0001','叠板机01-0001',49,'MO202304280003','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,125.00,NULL,NULL,NULL,NULL,NULL,'2023-05-06 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (38,'统一报工',218,'pin01-0001','叠板机01-0001',50,'MO202304280004','PCB单层',21,'pin','叠板',78,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,138.00,NULL,NULL,NULL,NULL,NULL,'2023-05-07 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (39,'统一报工',216,'dril01-0002','钻机01-0002',44,'MO202304270001','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,140.00,110.00,NULL,NULL,NULL,NULL,'2023-04-27 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (40,'统一报工',217,'drill01-0003','钻机01-0003',45,'MO202304270002','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,155.00,140.00,NULL,NULL,NULL,NULL,'2023-04-28 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (41,'统一报工',216,'dril01-0002','钻机01-0002',46,'MO202304270003','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,110.00,105.00,NULL,NULL,NULL,NULL,'2023-04-29 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (42,'统一报工',217,'drill01-0003','钻机01-0003',47,'MO202304280001','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,140.00,130.00,NULL,NULL,NULL,NULL,'2023-05-04 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (43,'统一报工',216,'dril01-0002','钻机01-0002',48,'MO202304280002','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,135.00,120.00,NULL,NULL,NULL,NULL,'2023-05-05 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (44,'统一报工',217,'drill01-0003','钻机01-0003',49,'MO202304280003','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,145.00,130.00,NULL,NULL,NULL,NULL,'2023-05-06 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (45,'统一报工',212,'drill01-0001','钻机01-0001',50,'MO202304280004','PCB单层',22,'drill','钻孔',79,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,130.00,115.00,NULL,NULL,NULL,NULL,'2023-05-07 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (46,'统一报工',219,'unpin01-0001','拆板机01-0001',44,'MO202304270001','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,140.00,NULL,NULL,NULL,NULL,NULL,'2023-04-27 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (47,'统一报工',219,'unpin01-0001','拆板机01-0001',45,'MO202304270002','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,142.00,NULL,NULL,NULL,NULL,NULL,'2023-04-28 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (48,'统一报工',219,'unpin01-0001','拆板机01-0001',46,'MO202304270003','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,110.00,NULL,NULL,NULL,NULL,NULL,'2023-04-29 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (49,'统一报工',219,'unpin01-0001','拆板机01-0001',47,'MO202304280001','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,140.00,NULL,NULL,NULL,NULL,NULL,'2023-05-04 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (50,'统一报工',219,'unpin01-0001','拆板机01-0001',48,'MO202304280002','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,130.00,NULL,NULL,NULL,NULL,NULL,'2023-05-05 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (51,'统一报工',219,'unpin01-0001','拆板机01-0001',49,'MO202304280003','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,135.00,NULL,NULL,NULL,NULL,NULL,'2023-05-06 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_feedback` VALUES (52,'统一报工',219,'unpin01-0001','拆板机01-0001',50,'MO202304280004','PCB单层',23,'unpin','拆板',29,NULL,36,'IF2023040500002','PCB单层板',22,'Panel',NULL,NULL,140.00,NULL,NULL,NULL,NULL,NULL,'2023-05-07 00:00:00','','DRAFT','N',0,1,NULL,NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品定义表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item`
--

LOCK TABLES `t_item` WRITE;
/*!40000 ALTER TABLE `t_item` DISABLE KEYS */;
INSERT INTO `t_item` VALUES (35,'WareHouse3',0,'WareHouse322','WareHouse3','WareHouse3',1,1,'0',0,1,1,'2023-04-04 02:06:00','2023-04-21 03:04:28',1,6,'p02','双面板',NULL,NULL,'WareHouse');
INSERT INTO `t_item` VALUES (36,'PCB单层板',0,'IF2023040500002','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-04-05 01:25:38','2023-04-20 04:44:00',1,12,'P08-01','高速电路板--客户001指定',NULL,NULL,'WareHouse3');
INSERT INTO `t_item` VALUES (40,'4567890',35,'23456以uiop','3456789','单层Panel',1,1,'0,35',0,1,1,'2023-04-11 23:35:00','2023-04-21 04:43:51',1,6,'p02','双面板',NULL,NULL,'WareHouse2');
INSERT INTO `t_item` VALUES (60,'铜单面板',0,'IF2023042300001','DDD XXXX DD','单层Panel',1,25,'0',0,1,33,'2023-04-23 02:27:08','2023-04-23 02:27:50',33,7,'p01','单面板',NULL,NULL,'WareHouse');
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
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_atp_file`
--

LOCK TABLES `t_item_atp_file` WRITE;
/*!40000 ALTER TABLE `t_item_atp_file` DISABLE KEYS */;
INSERT INTO `t_item_atp_file` VALUES (2,'IF2023040500002','PCB单层板',36,22,NULL,'\\\\fileServer\\IF200001111\\IF200001111.atp','string',0,NULL,'\\\\fileServer\\IF200001111\\IF200001111.drl',0,NULL,'\\\\fileServer\\IF200001111\\IF200001111.dia',2,2,2,0,'2023-04-07 07:35:18',0,0,1,'2023-04-07 15:35:28','2023-04-21 01:36:51',1);
INSERT INTO `t_item_atp_file` VALUES (15,'IF','PCB单层板',36,22,NULL,'\\\\\\\\fileServer\\\\IF200001111\\\\IF200001111.atp',NULL,NULL,NULL,'\\\\\\\\fileServer\\\\IF200001111\\\\IF200001111.drl',NULL,NULL,'\\\\\\\\fileServer\\\\IF200001111\\\\IF200001111.dia',1,5,10,0,'2023-04-12 01:04:00',0,1,33,'2023-04-11 21:07:20','2023-04-19 02:51:28',1);
INSERT INTO `t_item_atp_file` VALUES (36,'IF2023040500003','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'11',NULL,NULL,'11',7,9,10,0,NULL,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file` VALUES (37,'IF2023040500004','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'11',NULL,NULL,'11',7,9,10,0,NULL,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file` VALUES (38,'IF2023040500005','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'11',NULL,NULL,'11',7,9,10,0,NULL,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file` VALUES (39,'IF2023040500006','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'11',NULL,NULL,'11',7,9,10,0,NULL,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file` VALUES (40,'IF2023040500007','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'11',NULL,NULL,'11',7,8,5,0,NULL,0,1,1,'2023-04-20 04:57:43','2023-04-23 16:43:30',1);
INSERT INTO `t_item_atp_file` VALUES (41,'IF2023040500008','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'1',NULL,NULL,'1',1,1,0,0,NULL,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file` VALUES (42,'2','2',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2',NULL,NULL,'2',2,2,2,0,NULL,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file` VALUES (43,'IF2023040500007','PCB单层板',36,22,NULL,NULL,NULL,NULL,NULL,'11',NULL,NULL,'11',7,8,5,0,NULL,0,1,1,'2023-04-23 16:44:14',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=9445 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_atp_file_detail`
--

LOCK TABLES `t_item_atp_file_detail` WRITE;
/*!40000 ALTER TABLE `t_item_atp_file_detail` DISABLE KEYS */;
INSERT INTO `t_item_atp_file_detail` VALUES (106,17,NULL,NULL,1,0,1,1,'2023-04-12 21:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (108,17,NULL,NULL,2,0,1,1,'2023-04-12 21:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (110,17,NULL,NULL,3,0,1,1,'2023-04-12 21:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (112,17,NULL,NULL,4,0,1,1,'2023-04-12 21:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (114,17,NULL,NULL,5,0,1,1,'2023-04-12 21:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (116,17,NULL,NULL,6,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (118,18,NULL,NULL,1,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (119,17,NULL,NULL,7,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (121,18,NULL,NULL,2,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (122,17,NULL,NULL,8,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (124,18,NULL,NULL,3,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (125,17,NULL,NULL,9,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (126,18,NULL,NULL,4,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (127,19,NULL,NULL,1,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (129,17,NULL,NULL,10,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (130,19,NULL,NULL,2,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (132,18,NULL,NULL,5,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (133,17,NULL,NULL,11,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (134,18,NULL,NULL,6,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (136,19,NULL,NULL,3,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (137,17,NULL,NULL,12,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (139,19,NULL,NULL,4,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (140,18,NULL,NULL,7,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (141,17,NULL,NULL,13,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (143,18,NULL,NULL,8,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (144,19,NULL,NULL,5,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (145,17,NULL,NULL,14,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (146,18,NULL,NULL,9,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (147,19,NULL,NULL,6,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (149,17,NULL,NULL,15,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (151,19,NULL,NULL,7,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (152,18,NULL,NULL,10,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (153,17,NULL,NULL,16,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (154,18,NULL,NULL,11,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (155,19,NULL,NULL,8,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (157,17,NULL,NULL,17,0,1,1,'2023-04-12 21:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (158,19,NULL,NULL,9,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (160,18,NULL,NULL,12,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (161,17,NULL,NULL,18,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (162,18,NULL,NULL,13,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (164,19,NULL,NULL,10,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (165,17,NULL,NULL,19,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (167,19,NULL,NULL,11,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (168,18,NULL,NULL,14,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (169,17,NULL,NULL,20,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (170,19,NULL,NULL,12,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (171,18,NULL,NULL,15,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (173,17,NULL,NULL,21,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (174,18,NULL,NULL,16,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (176,19,NULL,NULL,13,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (177,17,NULL,NULL,22,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (179,19,NULL,NULL,14,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (180,18,NULL,NULL,17,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (181,17,NULL,NULL,23,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (183,18,NULL,NULL,18,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (184,19,NULL,NULL,15,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (185,17,NULL,NULL,24,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (186,18,NULL,NULL,19,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (187,19,NULL,NULL,16,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (189,17,NULL,NULL,25,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (190,18,NULL,NULL,20,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (192,19,NULL,NULL,17,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (193,17,NULL,NULL,26,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (195,19,NULL,NULL,18,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (196,18,NULL,NULL,21,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (197,17,NULL,NULL,27,0,1,1,'2023-04-12 21:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (198,19,NULL,NULL,19,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (199,18,NULL,NULL,22,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (201,17,NULL,NULL,28,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (202,18,NULL,NULL,23,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (204,19,NULL,NULL,20,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (205,17,NULL,NULL,29,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (207,19,NULL,NULL,21,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (208,18,NULL,NULL,24,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (209,17,NULL,NULL,30,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (210,19,NULL,NULL,22,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (212,18,NULL,NULL,25,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (213,17,NULL,NULL,31,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (215,18,NULL,NULL,26,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (216,19,NULL,NULL,23,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (217,17,NULL,NULL,32,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (218,18,NULL,NULL,27,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (219,19,NULL,NULL,24,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (221,17,NULL,NULL,33,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (222,18,NULL,NULL,28,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (224,19,NULL,NULL,25,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (225,17,NULL,NULL,34,0,1,1,'2023-04-12 21:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (227,19,NULL,NULL,26,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (228,18,NULL,NULL,29,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (229,17,NULL,NULL,35,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (231,18,NULL,NULL,30,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (232,19,NULL,NULL,27,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (233,17,NULL,NULL,36,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (234,18,NULL,NULL,31,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (235,19,NULL,NULL,28,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (237,17,NULL,NULL,37,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (239,19,NULL,NULL,29,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (240,18,NULL,NULL,32,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (241,17,NULL,NULL,38,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (242,19,NULL,NULL,30,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (243,18,NULL,NULL,33,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (245,17,NULL,NULL,39,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (247,18,NULL,NULL,34,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (248,19,NULL,NULL,31,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (249,17,NULL,NULL,40,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (250,18,NULL,NULL,35,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (251,19,NULL,NULL,32,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (253,17,NULL,NULL,41,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (255,19,NULL,NULL,33,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (256,18,NULL,NULL,36,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (257,17,NULL,NULL,42,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (258,18,NULL,NULL,37,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (259,19,NULL,NULL,34,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (261,17,NULL,NULL,43,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (263,19,NULL,NULL,35,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (264,18,NULL,NULL,38,0,1,1,'2023-04-12 21:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (265,17,NULL,NULL,44,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (266,18,NULL,NULL,39,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (267,19,NULL,NULL,36,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (269,17,NULL,NULL,45,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (271,19,NULL,NULL,37,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (272,18,NULL,NULL,40,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (273,17,NULL,NULL,46,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (274,19,NULL,NULL,38,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (275,18,NULL,NULL,41,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (277,17,NULL,NULL,47,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (279,18,NULL,NULL,42,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (280,19,NULL,NULL,39,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (281,17,NULL,NULL,48,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (282,18,NULL,NULL,43,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (283,19,NULL,NULL,40,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (285,17,NULL,NULL,49,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (287,19,NULL,NULL,41,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (288,18,NULL,NULL,44,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (289,17,NULL,NULL,50,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (290,18,NULL,NULL,45,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (291,19,NULL,NULL,42,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (293,17,NULL,NULL,51,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (295,19,NULL,NULL,43,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (296,18,NULL,NULL,46,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (297,17,NULL,NULL,52,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (298,19,NULL,NULL,44,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (299,18,NULL,NULL,47,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (301,17,NULL,NULL,53,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (302,18,NULL,NULL,48,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (304,19,NULL,NULL,45,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (305,17,NULL,NULL,54,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (307,19,NULL,NULL,46,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (308,18,NULL,NULL,49,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (309,17,NULL,NULL,55,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (310,19,NULL,NULL,47,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (311,18,NULL,NULL,50,0,1,1,'2023-04-12 21:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (313,17,NULL,NULL,56,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (314,18,NULL,NULL,51,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (316,19,NULL,NULL,48,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (317,17,NULL,NULL,57,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (319,19,NULL,NULL,49,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (320,18,NULL,NULL,52,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (321,17,NULL,NULL,58,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (322,19,NULL,NULL,50,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (323,18,NULL,NULL,53,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (325,17,NULL,NULL,59,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (326,18,NULL,NULL,54,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (328,19,NULL,NULL,51,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (329,17,NULL,NULL,60,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (331,19,NULL,NULL,52,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (332,18,NULL,NULL,55,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (333,17,NULL,NULL,61,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (334,18,NULL,NULL,56,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (335,19,NULL,NULL,53,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (337,17,NULL,NULL,62,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (338,19,NULL,NULL,54,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (340,18,NULL,NULL,57,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (341,17,NULL,NULL,63,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (343,18,NULL,NULL,58,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (344,19,NULL,NULL,55,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (345,17,NULL,NULL,64,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (346,18,NULL,NULL,59,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (347,19,NULL,NULL,56,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (349,17,NULL,NULL,65,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (351,19,NULL,NULL,57,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (352,18,NULL,NULL,60,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (353,17,NULL,NULL,66,0,1,1,'2023-04-12 21:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (354,19,NULL,NULL,58,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (355,18,NULL,NULL,61,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (357,17,NULL,NULL,67,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (358,18,NULL,NULL,62,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (360,19,NULL,NULL,59,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (361,17,NULL,NULL,68,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (363,19,NULL,NULL,60,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (364,18,NULL,NULL,63,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (365,17,NULL,NULL,69,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (366,18,NULL,NULL,64,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (367,19,NULL,NULL,61,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (369,17,NULL,NULL,70,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (370,19,NULL,NULL,62,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (372,18,NULL,NULL,65,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (373,17,NULL,NULL,71,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (374,18,NULL,NULL,66,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (376,19,NULL,NULL,63,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (377,17,NULL,NULL,72,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (379,19,NULL,NULL,64,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (380,18,NULL,NULL,67,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (381,17,NULL,NULL,73,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (382,18,NULL,NULL,68,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (383,19,NULL,NULL,65,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (385,17,NULL,NULL,74,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (387,19,NULL,NULL,66,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (388,18,NULL,NULL,69,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (389,17,NULL,NULL,75,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (390,19,NULL,NULL,67,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (391,18,NULL,NULL,70,0,1,1,'2023-04-12 21:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (393,17,NULL,NULL,76,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (395,18,NULL,NULL,71,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (396,19,NULL,NULL,68,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (397,17,NULL,NULL,77,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (398,18,NULL,NULL,72,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (399,19,NULL,NULL,69,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (401,17,NULL,NULL,78,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (403,19,NULL,NULL,70,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (404,18,NULL,NULL,73,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (405,17,NULL,NULL,79,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (406,18,NULL,NULL,74,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (407,19,NULL,NULL,71,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (409,17,NULL,NULL,80,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (410,19,NULL,NULL,72,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (412,18,NULL,NULL,75,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (413,17,NULL,NULL,81,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (414,18,NULL,NULL,76,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (416,19,NULL,NULL,73,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (417,17,NULL,NULL,82,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (419,19,NULL,NULL,74,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (420,18,NULL,NULL,77,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (421,17,NULL,NULL,83,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (422,19,NULL,NULL,75,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (423,18,NULL,NULL,78,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (425,17,NULL,NULL,84,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (426,18,NULL,NULL,79,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (428,19,NULL,NULL,76,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (429,17,NULL,NULL,85,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (431,19,NULL,NULL,77,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (432,18,NULL,NULL,80,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (433,17,NULL,NULL,86,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (434,19,NULL,NULL,78,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (435,18,NULL,NULL,81,0,1,1,'2023-04-12 21:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (437,17,NULL,NULL,87,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (438,18,NULL,NULL,82,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (440,19,NULL,NULL,79,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (441,17,NULL,NULL,88,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (443,19,NULL,NULL,80,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (444,18,NULL,NULL,83,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (445,17,NULL,NULL,89,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (446,19,NULL,NULL,81,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (447,18,NULL,NULL,84,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (449,17,NULL,NULL,90,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (451,19,NULL,NULL,82,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (452,18,NULL,NULL,85,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (453,17,NULL,NULL,91,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (454,19,NULL,NULL,83,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (455,18,NULL,NULL,86,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (457,17,NULL,NULL,92,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (458,18,NULL,NULL,87,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (460,19,NULL,NULL,84,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (461,17,NULL,NULL,93,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (463,19,NULL,NULL,85,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (464,18,NULL,NULL,88,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (465,17,NULL,NULL,94,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (466,19,NULL,NULL,86,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (467,18,NULL,NULL,89,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (469,17,NULL,NULL,95,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (470,18,NULL,NULL,90,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (472,19,NULL,NULL,87,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (473,17,NULL,NULL,96,0,1,1,'2023-04-12 21:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (475,19,NULL,NULL,88,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (476,18,NULL,NULL,91,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (477,17,NULL,NULL,97,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (478,19,NULL,NULL,89,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (479,18,NULL,NULL,92,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (481,17,NULL,NULL,98,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (483,18,NULL,NULL,93,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (484,19,NULL,NULL,90,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (485,17,NULL,NULL,99,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (486,18,NULL,NULL,94,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (487,19,NULL,NULL,91,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (489,17,NULL,NULL,100,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (491,19,NULL,NULL,92,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (492,18,NULL,NULL,95,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (493,17,NULL,NULL,101,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (494,18,NULL,NULL,96,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (495,19,NULL,NULL,93,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (497,17,NULL,NULL,102,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (499,19,NULL,NULL,94,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (500,18,NULL,NULL,97,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (501,17,NULL,NULL,103,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (502,18,NULL,NULL,98,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (503,19,NULL,NULL,95,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (505,17,NULL,NULL,104,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (507,19,NULL,NULL,96,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (508,18,NULL,NULL,99,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (509,17,NULL,NULL,105,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (510,18,NULL,NULL,100,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (511,19,NULL,NULL,97,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (513,17,NULL,NULL,106,0,1,1,'2023-04-12 21:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (515,19,NULL,NULL,98,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (516,18,NULL,NULL,101,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (517,17,NULL,NULL,107,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (518,18,NULL,NULL,102,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (519,19,NULL,NULL,99,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (521,17,NULL,NULL,108,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (522,19,NULL,NULL,100,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (524,18,NULL,NULL,103,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (525,17,NULL,NULL,109,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (526,19,NULL,NULL,101,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (528,18,NULL,NULL,104,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (529,17,NULL,NULL,110,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (530,18,NULL,NULL,105,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (532,19,NULL,NULL,102,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (533,17,NULL,NULL,111,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (535,19,NULL,NULL,103,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (536,18,NULL,NULL,106,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (537,17,NULL,NULL,112,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (538,18,NULL,NULL,107,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (540,19,NULL,NULL,104,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (541,17,NULL,NULL,113,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (543,19,NULL,NULL,105,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (544,18,NULL,NULL,108,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (545,17,NULL,NULL,114,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (546,18,NULL,NULL,109,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (547,19,NULL,NULL,106,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (549,17,NULL,NULL,115,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (550,18,NULL,NULL,110,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (552,19,NULL,NULL,107,0,1,1,'2023-04-12 21:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (553,17,NULL,NULL,116,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (555,19,NULL,NULL,108,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (556,18,NULL,NULL,111,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (557,17,NULL,NULL,117,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (558,18,NULL,NULL,112,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (559,19,NULL,NULL,109,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (561,17,NULL,NULL,118,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (563,19,NULL,NULL,110,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (564,18,NULL,NULL,113,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (565,17,NULL,NULL,119,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (566,18,NULL,NULL,114,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (567,19,NULL,NULL,111,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (569,17,NULL,NULL,120,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (571,19,NULL,NULL,112,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (572,18,NULL,NULL,115,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (573,17,NULL,NULL,121,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (574,18,NULL,NULL,116,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (575,19,NULL,NULL,113,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (577,17,NULL,NULL,122,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (579,19,NULL,NULL,114,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (580,18,NULL,NULL,117,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (581,17,NULL,NULL,123,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (582,18,NULL,NULL,118,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (583,19,NULL,NULL,115,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (585,17,NULL,NULL,124,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (587,19,NULL,NULL,116,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (588,18,NULL,NULL,119,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (589,17,NULL,NULL,125,0,1,1,'2023-04-12 21:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (590,18,NULL,NULL,120,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (591,19,NULL,NULL,117,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (593,17,NULL,NULL,126,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (595,19,NULL,NULL,118,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (596,18,NULL,NULL,121,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (597,17,NULL,NULL,127,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (598,18,NULL,NULL,122,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (599,19,NULL,NULL,119,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (601,17,NULL,NULL,128,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (602,19,NULL,NULL,120,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (604,18,NULL,NULL,123,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (605,17,NULL,NULL,129,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (607,18,NULL,NULL,124,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (608,19,NULL,NULL,121,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (609,17,NULL,NULL,130,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (610,18,NULL,NULL,125,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (611,19,NULL,NULL,122,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (613,17,NULL,NULL,131,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (615,19,NULL,NULL,123,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (616,18,NULL,NULL,126,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (617,17,NULL,NULL,132,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (618,18,NULL,NULL,127,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (619,19,NULL,NULL,124,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (621,17,NULL,NULL,133,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (623,18,NULL,NULL,128,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (624,19,NULL,NULL,125,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (625,17,NULL,NULL,134,0,1,1,'2023-04-12 21:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (626,18,NULL,NULL,129,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (627,19,NULL,NULL,126,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (629,17,NULL,NULL,135,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (631,19,NULL,NULL,127,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (632,18,NULL,NULL,130,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (633,17,NULL,NULL,136,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (634,19,NULL,NULL,128,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (635,18,NULL,NULL,131,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (637,17,NULL,NULL,137,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (638,18,NULL,NULL,132,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (640,19,NULL,NULL,129,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (641,17,NULL,NULL,138,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (643,18,NULL,NULL,133,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (644,19,NULL,NULL,130,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (645,17,NULL,NULL,139,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (646,19,NULL,NULL,131,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (647,18,NULL,NULL,134,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (649,17,NULL,NULL,140,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (650,18,NULL,NULL,135,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (652,19,NULL,NULL,132,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (653,17,NULL,NULL,141,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (655,19,NULL,NULL,133,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (656,18,NULL,NULL,136,0,1,1,'2023-04-12 21:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (657,17,NULL,NULL,142,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (658,19,NULL,NULL,134,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (659,18,NULL,NULL,137,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (661,17,NULL,NULL,143,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (663,19,NULL,NULL,135,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (664,18,NULL,NULL,138,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (665,17,NULL,NULL,144,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (666,18,NULL,NULL,139,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (667,19,NULL,NULL,136,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (669,17,NULL,NULL,145,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (670,19,NULL,NULL,137,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (672,18,NULL,NULL,140,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (673,17,NULL,NULL,146,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (674,18,NULL,NULL,141,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (676,19,NULL,NULL,138,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (677,17,NULL,NULL,147,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (679,19,NULL,NULL,139,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (680,18,NULL,NULL,142,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (681,17,NULL,NULL,148,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (683,18,NULL,NULL,143,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (684,19,NULL,NULL,140,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (685,17,NULL,NULL,149,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (686,18,NULL,NULL,144,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (687,19,NULL,NULL,141,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (689,17,NULL,NULL,150,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (690,19,NULL,NULL,142,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (692,18,NULL,NULL,145,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (693,17,NULL,NULL,151,0,1,1,'2023-04-12 21:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (694,18,NULL,NULL,146,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (696,19,NULL,NULL,143,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (697,17,NULL,NULL,152,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (699,19,NULL,NULL,144,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (700,18,NULL,NULL,147,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (701,17,NULL,NULL,153,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (703,18,NULL,NULL,148,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (704,19,NULL,NULL,145,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (705,17,NULL,NULL,154,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (706,18,NULL,NULL,149,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (707,19,NULL,NULL,146,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (709,17,NULL,NULL,155,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (710,19,NULL,NULL,147,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (712,18,NULL,NULL,150,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (713,17,NULL,NULL,156,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (715,18,NULL,NULL,151,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (716,19,NULL,NULL,148,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (717,17,NULL,NULL,157,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (718,18,NULL,NULL,152,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (719,19,NULL,NULL,149,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (721,17,NULL,NULL,158,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (723,19,NULL,NULL,150,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (724,18,NULL,NULL,153,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (725,17,NULL,NULL,159,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (726,18,NULL,NULL,154,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (727,19,NULL,NULL,151,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (729,17,NULL,NULL,160,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (730,19,NULL,NULL,152,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (732,18,NULL,NULL,155,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (733,17,NULL,NULL,161,0,1,1,'2023-04-12 21:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (734,18,NULL,NULL,156,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (736,19,NULL,NULL,153,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (737,17,NULL,NULL,162,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (739,19,NULL,NULL,154,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (740,18,NULL,NULL,157,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (741,17,NULL,NULL,163,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (742,19,NULL,NULL,155,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (744,18,NULL,NULL,158,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (745,17,NULL,NULL,164,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (746,18,NULL,NULL,159,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (748,19,NULL,NULL,156,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (749,17,NULL,NULL,165,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (751,19,NULL,NULL,157,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (752,18,NULL,NULL,160,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (753,17,NULL,NULL,166,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (754,18,NULL,NULL,161,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (755,19,NULL,NULL,158,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (757,17,NULL,NULL,167,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (758,19,NULL,NULL,159,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (760,18,NULL,NULL,162,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (761,17,NULL,NULL,168,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (763,19,NULL,NULL,160,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (764,18,NULL,NULL,163,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (765,17,NULL,NULL,169,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (766,18,NULL,NULL,164,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (767,19,NULL,NULL,161,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (769,17,NULL,NULL,170,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (770,19,NULL,NULL,162,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (772,18,NULL,NULL,165,0,1,1,'2023-04-12 21:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (773,17,NULL,NULL,171,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (775,19,NULL,NULL,163,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (776,18,NULL,NULL,166,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (777,17,NULL,NULL,172,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (778,19,NULL,NULL,164,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (780,18,NULL,NULL,167,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (781,17,NULL,NULL,173,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (783,18,NULL,NULL,168,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (784,19,NULL,NULL,165,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (785,17,NULL,NULL,174,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (786,18,NULL,NULL,169,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (787,19,NULL,NULL,166,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (789,17,NULL,NULL,175,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (791,19,NULL,NULL,167,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (792,18,NULL,NULL,170,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (793,17,NULL,NULL,176,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (794,19,NULL,NULL,168,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (795,18,NULL,NULL,171,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (797,17,NULL,NULL,177,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (798,18,NULL,NULL,172,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (800,19,NULL,NULL,169,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (801,17,NULL,NULL,178,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (803,19,NULL,NULL,170,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (804,18,NULL,NULL,173,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (805,17,NULL,NULL,179,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (806,19,NULL,NULL,171,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (808,18,NULL,NULL,174,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (809,17,NULL,NULL,180,0,1,1,'2023-04-12 21:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (810,18,NULL,NULL,175,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (812,19,NULL,NULL,172,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (813,17,NULL,NULL,181,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (815,19,NULL,NULL,173,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (816,18,NULL,NULL,176,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (817,17,NULL,NULL,182,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (818,18,NULL,NULL,177,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (819,19,NULL,NULL,174,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (821,17,NULL,NULL,183,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (823,19,NULL,NULL,175,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (824,18,NULL,NULL,178,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (825,17,NULL,NULL,184,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (826,18,NULL,NULL,179,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (827,19,NULL,NULL,176,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (829,17,NULL,NULL,185,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (831,19,NULL,NULL,177,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (832,18,NULL,NULL,180,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (833,17,NULL,NULL,186,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (834,18,NULL,NULL,181,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (835,19,NULL,NULL,178,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (837,17,NULL,NULL,187,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (839,19,NULL,NULL,179,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (840,18,NULL,NULL,182,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (841,17,NULL,NULL,188,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (842,18,NULL,NULL,183,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (843,19,NULL,NULL,180,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (845,17,NULL,NULL,189,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (846,19,NULL,NULL,181,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (848,18,NULL,NULL,184,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (849,17,NULL,NULL,190,0,1,1,'2023-04-12 21:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (850,18,NULL,NULL,185,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (852,19,NULL,NULL,182,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (853,17,NULL,NULL,191,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (855,19,NULL,NULL,183,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (856,18,NULL,NULL,186,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (857,17,NULL,NULL,192,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (858,18,NULL,NULL,187,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (859,19,NULL,NULL,184,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (861,17,NULL,NULL,193,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (863,19,NULL,NULL,185,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (864,18,NULL,NULL,188,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (865,17,NULL,NULL,194,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (866,18,NULL,NULL,189,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (867,19,NULL,NULL,186,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (869,17,NULL,NULL,195,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (871,19,NULL,NULL,187,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (872,18,NULL,NULL,190,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (873,17,NULL,NULL,196,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (874,18,NULL,NULL,191,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (875,19,NULL,NULL,188,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (877,17,NULL,NULL,197,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (879,19,NULL,NULL,189,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (880,18,NULL,NULL,192,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (881,17,NULL,NULL,198,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (882,18,NULL,NULL,193,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (883,19,NULL,NULL,190,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (885,17,NULL,NULL,199,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (887,19,NULL,NULL,191,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (888,18,NULL,NULL,194,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (889,17,NULL,NULL,200,0,1,1,'2023-04-12 21:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (890,18,NULL,NULL,195,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (891,19,NULL,NULL,192,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (893,17,NULL,NULL,201,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (895,19,NULL,NULL,193,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (896,18,NULL,NULL,196,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (897,17,NULL,NULL,202,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (898,18,NULL,NULL,197,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (899,19,NULL,NULL,194,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (901,17,NULL,NULL,203,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (903,19,NULL,NULL,195,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (904,18,NULL,NULL,198,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (905,17,NULL,NULL,204,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (906,19,NULL,NULL,196,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (907,18,NULL,NULL,199,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (909,17,NULL,NULL,205,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (911,18,NULL,NULL,200,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (912,19,NULL,NULL,197,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (913,17,NULL,NULL,206,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (914,18,NULL,NULL,201,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (915,19,NULL,NULL,198,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (917,17,NULL,NULL,207,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (918,18,NULL,NULL,202,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (920,19,NULL,NULL,199,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (921,17,NULL,NULL,208,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (923,19,NULL,NULL,200,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (924,18,NULL,NULL,203,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (925,17,NULL,NULL,209,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (926,19,NULL,NULL,201,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (927,18,NULL,NULL,204,0,1,1,'2023-04-12 21:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (929,17,NULL,NULL,210,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (931,18,NULL,NULL,205,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (932,19,NULL,NULL,202,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (933,17,NULL,NULL,211,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (934,18,NULL,NULL,206,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (935,19,NULL,NULL,203,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (937,17,NULL,NULL,212,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (938,19,NULL,NULL,204,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (940,18,NULL,NULL,207,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (941,17,NULL,NULL,213,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (942,18,NULL,NULL,208,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (944,19,NULL,NULL,205,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (945,17,NULL,NULL,214,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (947,19,NULL,NULL,206,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (948,18,NULL,NULL,209,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (949,17,NULL,NULL,215,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (950,18,NULL,NULL,210,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (951,19,NULL,NULL,207,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (953,17,NULL,NULL,216,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (954,19,NULL,NULL,208,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (955,18,NULL,NULL,211,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (957,17,NULL,NULL,217,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (958,18,NULL,NULL,212,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (960,19,NULL,NULL,209,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (961,17,NULL,NULL,218,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (963,19,NULL,NULL,210,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (964,18,NULL,NULL,213,0,1,1,'2023-04-12 21:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (965,17,NULL,NULL,219,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (966,19,NULL,NULL,211,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (967,18,NULL,NULL,214,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (969,17,NULL,NULL,220,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (971,18,NULL,NULL,215,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (972,19,NULL,NULL,212,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (973,17,NULL,NULL,221,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (974,18,NULL,NULL,216,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (975,19,NULL,NULL,213,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (977,17,NULL,NULL,222,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (978,19,NULL,NULL,214,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (980,18,NULL,NULL,217,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (981,17,NULL,NULL,223,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (983,18,NULL,NULL,218,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (984,19,NULL,NULL,215,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (985,17,NULL,NULL,224,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (986,18,NULL,NULL,219,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (987,19,NULL,NULL,216,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (989,17,NULL,NULL,225,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (991,19,NULL,NULL,217,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (992,18,NULL,NULL,220,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (993,17,NULL,NULL,226,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (994,18,NULL,NULL,221,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (995,19,NULL,NULL,218,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (997,17,NULL,NULL,227,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (998,19,NULL,NULL,219,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1000,18,NULL,NULL,222,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1001,17,NULL,NULL,228,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1002,18,NULL,NULL,223,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1004,19,NULL,NULL,220,0,1,1,'2023-04-12 21:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1005,17,NULL,NULL,229,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1007,19,NULL,NULL,221,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1008,18,NULL,NULL,224,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1009,17,NULL,NULL,230,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1010,18,NULL,NULL,225,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1011,19,NULL,NULL,222,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1013,17,NULL,NULL,231,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1015,19,NULL,NULL,223,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1016,18,NULL,NULL,226,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1017,17,NULL,NULL,232,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1018,19,NULL,NULL,224,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1019,18,NULL,NULL,227,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1021,17,NULL,NULL,233,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1023,18,NULL,NULL,228,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1024,19,NULL,NULL,225,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1025,17,NULL,NULL,234,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1026,18,NULL,NULL,229,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1027,19,NULL,NULL,226,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1029,17,NULL,NULL,235,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1031,19,NULL,NULL,227,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1032,18,NULL,NULL,230,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1033,17,NULL,NULL,236,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1034,18,NULL,NULL,231,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1035,19,NULL,NULL,228,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1037,17,NULL,NULL,237,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1039,19,NULL,NULL,229,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1040,18,NULL,NULL,232,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1041,17,NULL,NULL,238,0,1,1,'2023-04-12 21:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1042,18,NULL,NULL,233,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1043,19,NULL,NULL,230,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1045,17,NULL,NULL,239,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1046,19,NULL,NULL,231,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1048,18,NULL,NULL,234,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1049,17,NULL,NULL,240,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1051,18,NULL,NULL,235,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1052,19,NULL,NULL,232,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1053,17,NULL,NULL,241,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1054,18,NULL,NULL,236,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1055,19,NULL,NULL,233,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1057,17,NULL,NULL,242,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1059,19,NULL,NULL,234,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1060,18,NULL,NULL,237,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1061,17,NULL,NULL,243,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1062,18,NULL,NULL,238,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1063,19,NULL,NULL,235,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1064,17,NULL,NULL,244,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1065,19,NULL,NULL,236,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1066,18,NULL,NULL,239,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1067,17,NULL,NULL,245,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1068,18,NULL,NULL,240,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1069,19,NULL,NULL,237,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1070,17,NULL,NULL,246,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1071,19,NULL,NULL,238,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1072,18,NULL,NULL,241,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1073,17,NULL,NULL,247,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1074,19,NULL,NULL,239,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1075,18,NULL,NULL,242,0,1,1,'2023-04-12 21:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1076,17,NULL,NULL,248,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1077,18,NULL,NULL,243,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1078,19,NULL,NULL,240,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1079,17,NULL,NULL,249,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1080,19,NULL,NULL,241,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1081,18,NULL,NULL,244,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1082,17,NULL,NULL,250,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1083,18,NULL,NULL,245,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1084,19,NULL,NULL,242,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1085,17,NULL,NULL,251,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1086,18,NULL,NULL,246,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1087,19,NULL,NULL,243,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1088,17,NULL,NULL,252,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1089,19,NULL,NULL,244,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1090,18,NULL,NULL,247,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1091,17,NULL,NULL,253,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1092,19,NULL,NULL,245,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1093,18,NULL,NULL,248,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1094,17,NULL,NULL,254,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1095,18,NULL,NULL,249,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1096,19,NULL,NULL,246,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1097,17,NULL,NULL,255,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1098,19,NULL,NULL,247,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1099,18,NULL,NULL,250,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1100,17,NULL,NULL,256,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1101,19,NULL,NULL,248,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1102,18,NULL,NULL,251,0,1,1,'2023-04-12 21:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1103,17,NULL,NULL,257,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1104,18,NULL,NULL,252,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1105,19,NULL,NULL,249,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1106,17,NULL,NULL,258,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1107,19,NULL,NULL,250,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1108,18,NULL,NULL,253,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1109,17,NULL,NULL,259,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1110,19,NULL,NULL,251,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1111,18,NULL,NULL,254,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1112,17,NULL,NULL,260,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1113,18,NULL,NULL,255,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1114,19,NULL,NULL,252,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1115,17,NULL,NULL,261,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1116,19,NULL,NULL,253,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1117,18,NULL,NULL,256,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1118,17,NULL,NULL,262,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1119,19,NULL,NULL,254,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1120,18,NULL,NULL,257,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1121,17,NULL,NULL,263,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1122,18,NULL,NULL,258,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1123,19,NULL,NULL,255,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1124,17,NULL,NULL,264,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1125,19,NULL,NULL,256,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1126,18,NULL,NULL,259,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1127,17,NULL,NULL,265,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1128,18,NULL,NULL,260,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1129,19,NULL,NULL,257,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1130,17,NULL,NULL,266,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1131,18,NULL,NULL,261,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1132,19,NULL,NULL,258,0,1,1,'2023-04-12 21:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1133,17,NULL,NULL,267,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1134,18,NULL,NULL,262,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1135,19,NULL,NULL,259,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1136,17,NULL,NULL,268,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1137,19,NULL,NULL,260,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1138,18,NULL,NULL,263,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1139,17,NULL,NULL,269,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1140,18,NULL,NULL,264,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1141,19,NULL,NULL,261,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1142,17,NULL,NULL,270,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1143,18,NULL,NULL,265,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1144,19,NULL,NULL,262,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1145,17,NULL,NULL,271,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1146,19,NULL,NULL,263,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1147,18,NULL,NULL,266,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1148,17,NULL,NULL,272,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1149,18,NULL,NULL,267,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1150,19,NULL,NULL,264,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1151,17,NULL,NULL,273,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1152,18,NULL,NULL,268,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1153,19,NULL,NULL,265,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1154,17,NULL,NULL,274,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1155,18,NULL,NULL,269,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1156,19,NULL,NULL,266,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1157,17,NULL,NULL,275,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1158,19,NULL,NULL,267,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1159,18,NULL,NULL,270,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1160,17,NULL,NULL,276,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1161,18,NULL,NULL,271,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1162,19,NULL,NULL,268,0,1,1,'2023-04-12 21:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1163,17,NULL,NULL,277,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1164,18,NULL,NULL,272,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1165,19,NULL,NULL,269,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1166,17,NULL,NULL,278,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1167,19,NULL,NULL,270,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1168,18,NULL,NULL,273,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1169,17,NULL,NULL,279,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1170,18,NULL,NULL,274,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1171,19,NULL,NULL,271,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1172,17,NULL,NULL,280,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1173,18,NULL,NULL,275,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1174,19,NULL,NULL,272,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1175,17,NULL,NULL,281,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1176,19,NULL,NULL,273,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1177,18,NULL,NULL,276,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1178,17,NULL,NULL,282,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1179,18,NULL,NULL,277,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1180,19,NULL,NULL,274,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1181,17,NULL,NULL,283,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1182,19,NULL,NULL,275,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1183,18,NULL,NULL,278,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1184,17,NULL,NULL,284,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1185,19,NULL,NULL,276,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1186,18,NULL,NULL,279,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1187,17,NULL,NULL,285,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1188,18,NULL,NULL,280,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1189,19,NULL,NULL,277,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1190,17,NULL,NULL,286,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1191,18,NULL,NULL,281,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1192,19,NULL,NULL,278,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1193,17,NULL,NULL,287,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1194,19,NULL,NULL,279,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1195,18,NULL,NULL,282,0,1,1,'2023-04-12 21:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1196,17,NULL,NULL,288,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1197,18,NULL,NULL,283,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1198,19,NULL,NULL,280,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1199,17,NULL,NULL,289,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1200,19,NULL,NULL,281,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1201,18,NULL,NULL,284,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1202,17,NULL,NULL,290,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1203,18,NULL,NULL,285,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1204,19,NULL,NULL,282,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1205,17,NULL,NULL,291,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1206,18,NULL,NULL,286,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1207,19,NULL,NULL,283,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1208,20,NULL,NULL,1,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1209,17,NULL,NULL,292,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1210,19,NULL,NULL,284,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1211,18,NULL,NULL,287,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1212,17,NULL,NULL,293,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1213,20,NULL,NULL,2,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1214,18,NULL,NULL,288,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1215,19,NULL,NULL,285,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1216,17,NULL,NULL,294,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1217,20,NULL,NULL,3,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1218,19,NULL,NULL,286,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1219,18,NULL,NULL,289,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1220,20,NULL,NULL,4,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1221,17,NULL,NULL,295,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1222,18,NULL,NULL,290,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1223,19,NULL,NULL,287,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1224,20,NULL,NULL,5,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1225,17,NULL,NULL,296,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1226,18,NULL,NULL,291,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1227,19,NULL,NULL,288,0,1,1,'2023-04-12 21:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1228,17,NULL,NULL,297,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1229,20,NULL,NULL,6,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1230,19,NULL,NULL,289,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1231,18,NULL,NULL,292,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1232,17,NULL,NULL,298,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1233,20,NULL,NULL,7,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1234,18,NULL,NULL,293,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1235,19,NULL,NULL,290,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1236,20,NULL,NULL,8,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1237,17,NULL,NULL,299,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1238,19,NULL,NULL,291,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1239,18,NULL,NULL,294,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1240,17,NULL,NULL,300,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1241,20,NULL,NULL,9,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1242,19,NULL,NULL,292,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1243,18,NULL,NULL,295,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1244,20,NULL,NULL,10,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1245,19,NULL,NULL,293,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1246,18,NULL,NULL,296,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1247,20,NULL,NULL,11,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1248,18,NULL,NULL,297,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1249,19,NULL,NULL,294,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1250,20,NULL,NULL,12,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1251,18,NULL,NULL,298,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1252,19,NULL,NULL,295,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1253,20,NULL,NULL,13,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1254,18,NULL,NULL,299,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1255,19,NULL,NULL,296,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1256,20,NULL,NULL,14,0,1,1,'2023-04-12 21:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1257,19,NULL,NULL,297,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1258,18,NULL,NULL,300,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1259,20,NULL,NULL,15,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1260,19,NULL,NULL,298,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1261,20,NULL,NULL,16,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1262,19,NULL,NULL,299,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1263,20,NULL,NULL,17,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1264,19,NULL,NULL,300,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1265,20,NULL,NULL,18,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1266,20,NULL,NULL,19,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1267,20,NULL,NULL,20,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1268,20,NULL,NULL,21,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1269,20,NULL,NULL,22,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1270,20,NULL,NULL,23,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1271,20,NULL,NULL,24,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1272,20,NULL,NULL,25,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1273,20,NULL,NULL,26,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1274,20,NULL,NULL,27,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1275,20,NULL,NULL,28,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1276,20,NULL,NULL,29,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1277,20,NULL,NULL,30,0,1,1,'2023-04-12 21:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1278,20,NULL,NULL,31,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1279,20,NULL,NULL,32,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1280,20,NULL,NULL,33,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1281,20,NULL,NULL,34,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1282,20,NULL,NULL,35,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1283,20,NULL,NULL,36,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1284,20,NULL,NULL,37,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1285,20,NULL,NULL,38,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1286,20,NULL,NULL,39,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1287,20,NULL,NULL,40,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1288,20,NULL,NULL,41,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1289,20,NULL,NULL,42,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1290,20,NULL,NULL,43,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1291,20,NULL,NULL,44,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1292,20,NULL,NULL,45,0,1,1,'2023-04-12 21:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1293,20,NULL,NULL,46,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1294,20,NULL,NULL,47,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1295,20,NULL,NULL,48,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1296,20,NULL,NULL,49,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1297,20,NULL,NULL,50,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1298,20,NULL,NULL,51,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1299,20,NULL,NULL,52,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1300,20,NULL,NULL,53,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1301,20,NULL,NULL,54,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1302,20,NULL,NULL,55,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1303,20,NULL,NULL,56,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1304,20,NULL,NULL,57,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1305,20,NULL,NULL,58,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1306,20,NULL,NULL,59,0,1,1,'2023-04-12 21:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1307,20,NULL,NULL,60,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1308,20,NULL,NULL,61,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1309,20,NULL,NULL,62,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1310,20,NULL,NULL,63,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1311,20,NULL,NULL,64,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1312,20,NULL,NULL,65,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1313,20,NULL,NULL,66,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1314,20,NULL,NULL,67,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1315,20,NULL,NULL,68,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1316,20,NULL,NULL,69,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1317,20,NULL,NULL,70,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1318,20,NULL,NULL,71,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1319,20,NULL,NULL,72,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1320,20,NULL,NULL,73,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1321,20,NULL,NULL,74,0,1,1,'2023-04-12 21:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1322,20,NULL,NULL,75,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1323,20,NULL,NULL,76,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1324,20,NULL,NULL,77,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1325,20,NULL,NULL,78,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1326,20,NULL,NULL,79,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1327,20,NULL,NULL,80,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1328,20,NULL,NULL,81,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1329,20,NULL,NULL,82,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1330,20,NULL,NULL,83,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1331,20,NULL,NULL,84,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1332,20,NULL,NULL,85,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1333,20,NULL,NULL,86,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1334,20,NULL,NULL,87,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1335,20,NULL,NULL,88,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1336,20,NULL,NULL,89,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1337,20,NULL,NULL,90,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1338,20,NULL,NULL,91,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1339,20,NULL,NULL,92,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1340,20,NULL,NULL,93,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1341,20,NULL,NULL,94,0,1,1,'2023-04-12 21:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1342,20,NULL,NULL,95,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1343,20,NULL,NULL,96,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1344,20,NULL,NULL,97,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1345,20,NULL,NULL,98,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1346,20,NULL,NULL,99,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1347,20,NULL,NULL,100,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1348,20,NULL,NULL,101,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1349,20,NULL,NULL,102,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1350,20,NULL,NULL,103,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1351,20,NULL,NULL,104,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1352,20,NULL,NULL,105,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1353,20,NULL,NULL,106,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1354,20,NULL,NULL,107,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1355,20,NULL,NULL,108,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1356,20,NULL,NULL,109,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1357,20,NULL,NULL,110,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1358,20,NULL,NULL,111,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1359,20,NULL,NULL,112,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1360,20,NULL,NULL,113,0,1,1,'2023-04-12 21:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1361,20,NULL,NULL,114,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1362,20,NULL,NULL,115,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1363,20,NULL,NULL,116,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1364,20,NULL,NULL,117,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1365,20,NULL,NULL,118,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1366,20,NULL,NULL,119,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1367,20,NULL,NULL,120,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1368,20,NULL,NULL,121,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1369,20,NULL,NULL,122,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1370,20,NULL,NULL,123,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1371,20,NULL,NULL,124,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1372,20,NULL,NULL,125,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1373,20,NULL,NULL,126,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1374,20,NULL,NULL,127,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1375,20,NULL,NULL,128,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1376,20,NULL,NULL,129,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1377,20,NULL,NULL,130,0,1,1,'2023-04-12 21:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1378,20,NULL,NULL,131,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1379,20,NULL,NULL,132,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1380,20,NULL,NULL,133,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1381,20,NULL,NULL,134,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1382,20,NULL,NULL,135,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1383,20,NULL,NULL,136,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1384,20,NULL,NULL,137,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1385,20,NULL,NULL,138,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1386,20,NULL,NULL,139,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1387,20,NULL,NULL,140,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1388,20,NULL,NULL,141,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1389,20,NULL,NULL,142,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1390,20,NULL,NULL,143,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1391,20,NULL,NULL,144,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1392,20,NULL,NULL,145,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1393,20,NULL,NULL,146,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1394,20,NULL,NULL,147,0,1,1,'2023-04-12 21:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1395,20,NULL,NULL,148,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1396,20,NULL,NULL,149,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1397,20,NULL,NULL,150,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1398,20,NULL,NULL,151,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1399,20,NULL,NULL,152,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1400,20,NULL,NULL,153,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1401,20,NULL,NULL,154,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1402,20,NULL,NULL,155,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1403,20,NULL,NULL,156,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1404,20,NULL,NULL,157,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1405,20,NULL,NULL,158,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1406,20,NULL,NULL,159,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1407,20,NULL,NULL,160,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1408,20,NULL,NULL,161,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1409,20,NULL,NULL,162,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1410,20,NULL,NULL,163,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1411,20,NULL,NULL,164,0,1,1,'2023-04-12 21:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1412,20,NULL,NULL,165,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1413,20,NULL,NULL,166,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1414,20,NULL,NULL,167,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1415,20,NULL,NULL,168,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1416,20,NULL,NULL,169,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1417,20,NULL,NULL,170,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1418,20,NULL,NULL,171,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1419,20,NULL,NULL,172,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1420,20,NULL,NULL,173,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1421,20,NULL,NULL,174,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1422,20,NULL,NULL,175,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1423,20,NULL,NULL,176,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1424,20,NULL,NULL,177,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1425,20,NULL,NULL,178,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1426,20,NULL,NULL,179,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1427,20,NULL,NULL,180,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1428,20,NULL,NULL,181,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1429,20,NULL,NULL,182,0,1,1,'2023-04-12 21:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1430,20,NULL,NULL,183,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1431,20,NULL,NULL,184,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1432,20,NULL,NULL,185,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1433,20,NULL,NULL,186,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1434,20,NULL,NULL,187,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1435,20,NULL,NULL,188,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1436,20,NULL,NULL,189,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1437,20,NULL,NULL,190,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1438,20,NULL,NULL,191,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1439,20,NULL,NULL,192,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1440,20,NULL,NULL,193,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1441,20,NULL,NULL,194,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1442,20,NULL,NULL,195,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1443,20,NULL,NULL,196,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1444,20,NULL,NULL,197,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1445,20,NULL,NULL,198,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1446,20,NULL,NULL,199,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1447,20,NULL,NULL,200,0,1,1,'2023-04-12 21:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1448,20,NULL,NULL,201,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1449,20,NULL,NULL,202,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1450,20,NULL,NULL,203,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1451,20,NULL,NULL,204,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1452,20,NULL,NULL,205,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1453,20,NULL,NULL,206,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1454,20,NULL,NULL,207,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1455,20,NULL,NULL,208,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1456,20,NULL,NULL,209,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1457,20,NULL,NULL,210,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1458,20,NULL,NULL,211,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1459,20,NULL,NULL,212,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1460,20,NULL,NULL,213,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1461,20,NULL,NULL,214,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1462,20,NULL,NULL,215,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1463,20,NULL,NULL,216,0,1,1,'2023-04-12 21:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1464,20,NULL,NULL,217,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1465,20,NULL,NULL,218,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1466,20,NULL,NULL,219,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1467,20,NULL,NULL,220,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1468,20,NULL,NULL,221,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1469,20,NULL,NULL,222,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1470,20,NULL,NULL,223,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1471,20,NULL,NULL,224,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1472,20,NULL,NULL,225,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1473,20,NULL,NULL,226,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1474,20,NULL,NULL,227,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1475,20,NULL,NULL,228,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1476,20,NULL,NULL,229,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1477,20,NULL,NULL,230,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1478,20,NULL,NULL,231,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1479,20,NULL,NULL,232,0,1,1,'2023-04-12 21:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1480,20,NULL,NULL,233,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1481,20,NULL,NULL,234,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1482,20,NULL,NULL,235,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1483,20,NULL,NULL,236,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1484,20,NULL,NULL,237,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1485,20,NULL,NULL,238,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1486,20,NULL,NULL,239,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1487,20,NULL,NULL,240,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1488,20,NULL,NULL,241,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1489,20,NULL,NULL,242,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1490,20,NULL,NULL,243,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1491,20,NULL,NULL,244,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1492,20,NULL,NULL,245,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1493,20,NULL,NULL,246,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1494,20,NULL,NULL,247,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1495,20,NULL,NULL,248,0,1,1,'2023-04-12 21:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1496,20,NULL,NULL,249,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1497,20,NULL,NULL,250,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1498,20,NULL,NULL,251,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1499,20,NULL,NULL,252,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1500,20,NULL,NULL,253,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1501,20,NULL,NULL,254,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1502,20,NULL,NULL,255,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1503,20,NULL,NULL,256,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1504,20,NULL,NULL,257,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1505,20,NULL,NULL,258,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1506,20,NULL,NULL,259,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1507,20,NULL,NULL,260,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1508,20,NULL,NULL,261,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1509,20,NULL,NULL,262,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1510,20,NULL,NULL,263,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1511,20,NULL,NULL,264,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1512,20,NULL,NULL,265,0,1,1,'2023-04-12 21:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1513,20,NULL,NULL,266,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1514,20,NULL,NULL,267,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1515,20,NULL,NULL,268,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1516,20,NULL,NULL,269,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1517,20,NULL,NULL,270,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1518,20,NULL,NULL,271,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1519,20,NULL,NULL,272,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1520,20,NULL,NULL,273,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1521,20,NULL,NULL,274,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1522,20,NULL,NULL,275,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1523,20,NULL,NULL,276,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1524,20,NULL,NULL,277,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1525,20,NULL,NULL,278,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1526,20,NULL,NULL,279,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1527,20,NULL,NULL,280,0,1,1,'2023-04-12 21:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1528,20,NULL,NULL,281,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1529,20,NULL,NULL,282,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1530,20,NULL,NULL,283,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1531,20,NULL,NULL,284,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1532,20,NULL,NULL,285,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1533,20,NULL,NULL,286,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1534,20,NULL,NULL,287,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1535,20,NULL,NULL,288,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1536,20,NULL,NULL,289,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1537,20,NULL,NULL,290,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1538,20,NULL,NULL,291,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1539,20,NULL,NULL,292,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1540,20,NULL,NULL,293,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1541,20,NULL,NULL,294,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1542,20,NULL,NULL,295,0,1,1,'2023-04-12 21:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1543,20,NULL,NULL,296,0,1,1,'2023-04-12 21:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1544,20,NULL,NULL,297,0,1,1,'2023-04-12 21:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1545,20,NULL,NULL,298,0,1,1,'2023-04-12 21:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1546,20,NULL,NULL,299,0,1,1,'2023-04-12 21:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1547,20,NULL,NULL,300,0,1,1,'2023-04-12 21:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1848,16,NULL,NULL,1,0,1,1,'2023-04-12 21:59:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1849,16,NULL,NULL,2,0,1,1,'2023-04-12 21:59:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1850,16,NULL,NULL,3,0,1,1,'2023-04-12 21:59:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1851,16,NULL,NULL,4,0,1,1,'2023-04-12 21:59:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1852,16,NULL,NULL,5,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1853,16,NULL,NULL,6,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1854,16,NULL,NULL,7,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1855,16,NULL,NULL,8,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1856,16,NULL,NULL,9,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1857,16,NULL,NULL,10,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1858,16,NULL,NULL,11,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1859,16,NULL,NULL,12,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1860,16,NULL,NULL,13,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1861,16,NULL,NULL,14,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1862,16,NULL,NULL,15,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1863,16,NULL,NULL,16,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1864,16,NULL,NULL,17,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1865,16,NULL,NULL,18,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1866,16,NULL,NULL,19,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1867,16,NULL,NULL,20,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1868,16,NULL,NULL,21,0,1,1,'2023-04-12 21:59:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1869,16,NULL,NULL,22,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1870,16,NULL,NULL,23,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1871,16,NULL,NULL,24,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1872,16,NULL,NULL,25,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1873,16,NULL,NULL,26,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1874,16,NULL,NULL,27,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1875,16,NULL,NULL,28,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1876,16,NULL,NULL,29,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1877,16,NULL,NULL,30,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1878,16,NULL,NULL,31,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1879,16,NULL,NULL,32,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1880,16,NULL,NULL,33,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1881,16,NULL,NULL,34,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1882,16,NULL,NULL,35,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1883,16,NULL,NULL,36,0,1,1,'2023-04-12 21:59:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1884,21,NULL,NULL,1,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1885,21,NULL,NULL,2,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1886,21,NULL,NULL,3,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1887,21,NULL,NULL,4,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1888,21,NULL,NULL,5,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1889,21,NULL,NULL,6,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1890,21,NULL,NULL,7,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1891,21,NULL,NULL,8,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1892,21,NULL,NULL,9,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1893,21,NULL,NULL,10,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1894,21,NULL,NULL,11,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1895,21,NULL,NULL,12,0,1,1,'2023-04-12 22:00:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1896,22,NULL,NULL,1,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1897,22,NULL,NULL,2,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1898,22,NULL,NULL,3,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1899,22,NULL,NULL,4,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1900,22,NULL,NULL,5,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1901,22,NULL,NULL,6,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1902,22,NULL,NULL,7,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1903,22,NULL,NULL,8,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1904,22,NULL,NULL,9,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1905,22,NULL,NULL,10,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1906,22,NULL,NULL,11,0,1,1,'2023-04-12 22:01:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1907,22,NULL,NULL,12,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1908,22,NULL,NULL,13,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1909,22,NULL,NULL,14,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1910,22,NULL,NULL,15,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1911,22,NULL,NULL,16,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1912,22,NULL,NULL,17,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1913,22,NULL,NULL,18,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1914,22,NULL,NULL,19,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1915,22,NULL,NULL,20,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1916,22,NULL,NULL,21,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1917,22,NULL,NULL,22,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1918,22,NULL,NULL,23,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (1919,22,NULL,NULL,24,0,1,1,'2023-04-12 22:01:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4632,25,NULL,NULL,1,0,1,1,'2023-04-16 22:39:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4633,24,NULL,NULL,1,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4634,24,NULL,NULL,2,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4635,24,NULL,NULL,3,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4636,24,NULL,NULL,4,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4637,24,NULL,NULL,5,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4638,24,NULL,NULL,6,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4639,24,NULL,NULL,7,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4640,24,NULL,NULL,8,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4641,24,NULL,NULL,9,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4642,24,NULL,NULL,10,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4643,24,NULL,NULL,11,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4644,24,NULL,NULL,12,0,1,1,'2023-04-18 02:02:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4645,24,NULL,NULL,13,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4646,24,NULL,NULL,14,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4647,24,NULL,NULL,15,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4648,24,NULL,NULL,16,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4649,24,NULL,NULL,17,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4650,24,NULL,NULL,18,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4651,24,NULL,NULL,19,0,1,1,'2023-04-18 02:02:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4652,24,NULL,NULL,20,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4653,24,NULL,NULL,21,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4654,24,NULL,NULL,22,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4655,24,NULL,NULL,23,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4656,24,NULL,NULL,24,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4657,24,NULL,NULL,25,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4658,24,NULL,NULL,26,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4659,24,NULL,NULL,27,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4660,24,NULL,NULL,28,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4661,24,NULL,NULL,29,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4662,24,NULL,NULL,30,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4663,24,NULL,NULL,31,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4664,24,NULL,NULL,32,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4665,24,NULL,NULL,33,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4666,24,NULL,NULL,34,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4667,24,NULL,NULL,35,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4668,24,NULL,NULL,36,0,1,1,'2023-04-18 02:02:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4669,24,NULL,NULL,37,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4670,24,NULL,NULL,38,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4671,24,NULL,NULL,39,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4672,24,NULL,NULL,40,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4673,24,NULL,NULL,41,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4674,24,NULL,NULL,42,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4675,24,NULL,NULL,43,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4676,24,NULL,NULL,44,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4677,24,NULL,NULL,45,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4678,24,NULL,NULL,46,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4679,24,NULL,NULL,47,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4680,24,NULL,NULL,48,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4681,24,NULL,NULL,49,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4682,24,NULL,NULL,50,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4683,24,NULL,NULL,51,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4684,24,NULL,NULL,52,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4685,24,NULL,NULL,53,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4686,24,NULL,NULL,54,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4687,24,NULL,NULL,55,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4688,24,NULL,NULL,56,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4689,24,NULL,NULL,57,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4690,24,NULL,NULL,58,0,1,1,'2023-04-18 02:02:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4691,24,NULL,NULL,59,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4692,24,NULL,NULL,60,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4693,24,NULL,NULL,61,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4694,24,NULL,NULL,62,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4695,24,NULL,NULL,63,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4696,24,NULL,NULL,64,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4697,24,NULL,NULL,65,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4698,24,NULL,NULL,66,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4699,24,NULL,NULL,67,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4700,24,NULL,NULL,68,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4701,24,NULL,NULL,69,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4702,24,NULL,NULL,70,0,1,1,'2023-04-18 02:02:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4703,24,NULL,NULL,71,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4704,24,NULL,NULL,72,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4705,24,NULL,NULL,73,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4706,24,NULL,NULL,74,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4707,24,NULL,NULL,75,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4708,24,NULL,NULL,76,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4709,24,NULL,NULL,77,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4710,24,NULL,NULL,78,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4711,24,NULL,NULL,79,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4712,24,NULL,NULL,80,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4713,24,NULL,NULL,81,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4714,24,NULL,NULL,82,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4715,24,NULL,NULL,83,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4716,24,NULL,NULL,84,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4717,24,NULL,NULL,85,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4718,24,NULL,NULL,86,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4719,24,NULL,NULL,87,0,1,1,'2023-04-18 02:02:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4720,24,NULL,NULL,88,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4721,24,NULL,NULL,89,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4722,24,NULL,NULL,90,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4723,24,NULL,NULL,91,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4724,24,NULL,NULL,92,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4725,24,NULL,NULL,93,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4726,24,NULL,NULL,94,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4727,24,NULL,NULL,95,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4728,24,NULL,NULL,96,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4729,24,NULL,NULL,97,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4730,24,NULL,NULL,98,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4731,24,NULL,NULL,99,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4732,24,NULL,NULL,100,0,1,1,'2023-04-18 02:02:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4733,15,NULL,NULL,1,0,1,1,'2023-04-19 02:51:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4734,15,NULL,NULL,2,0,1,1,'2023-04-19 02:51:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4735,15,NULL,NULL,3,0,1,1,'2023-04-19 02:51:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4736,15,NULL,NULL,4,0,1,1,'2023-04-19 02:51:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4737,15,NULL,NULL,5,0,1,1,'2023-04-19 02:51:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4738,15,NULL,NULL,6,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4739,15,NULL,NULL,7,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4740,15,NULL,NULL,8,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4741,15,NULL,NULL,9,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4742,15,'T03',0.90,10,0,0,1,'2023-04-19 02:51:29','2023-04-19 02:54:07',1);
INSERT INTO `t_item_atp_file_detail` VALUES (4743,15,NULL,NULL,11,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4744,15,NULL,NULL,12,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4745,15,NULL,NULL,13,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4746,15,NULL,NULL,14,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4747,15,NULL,NULL,15,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4748,15,NULL,NULL,16,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4749,15,NULL,NULL,17,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4750,15,NULL,NULL,18,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4751,15,NULL,NULL,19,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4752,15,NULL,NULL,20,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4753,15,NULL,NULL,21,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4754,15,NULL,NULL,22,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4755,15,NULL,NULL,23,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4756,15,NULL,NULL,24,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4757,15,NULL,NULL,25,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4758,15,NULL,NULL,26,0,1,1,'2023-04-19 02:51:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4759,15,NULL,NULL,27,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4760,15,NULL,NULL,28,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4761,15,NULL,NULL,29,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4762,15,NULL,NULL,30,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4763,15,NULL,NULL,31,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4764,15,NULL,NULL,32,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4765,15,NULL,NULL,33,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4766,15,NULL,NULL,34,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4767,15,NULL,NULL,35,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4768,15,NULL,NULL,36,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4769,15,NULL,NULL,37,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4770,15,NULL,NULL,38,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4771,15,NULL,NULL,39,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4772,15,NULL,NULL,40,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4773,15,NULL,NULL,41,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4774,15,NULL,NULL,42,0,1,1,'2023-04-19 02:51:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4775,15,NULL,NULL,43,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4776,15,NULL,NULL,44,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4777,15,NULL,NULL,45,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4778,15,NULL,NULL,46,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4779,15,NULL,NULL,47,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4780,15,NULL,NULL,48,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4781,15,NULL,NULL,49,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4782,15,NULL,NULL,50,0,1,1,'2023-04-19 02:51:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4783,36,NULL,NULL,1,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4784,36,NULL,NULL,2,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4785,36,NULL,NULL,3,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4786,36,NULL,NULL,4,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4787,36,NULL,NULL,5,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4788,36,NULL,NULL,6,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4789,36,NULL,NULL,7,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4790,36,NULL,NULL,8,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4791,36,NULL,NULL,9,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4792,36,NULL,NULL,10,0,1,1,'2023-04-20 04:57:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4793,36,NULL,NULL,11,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4794,36,NULL,NULL,12,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4795,36,NULL,NULL,13,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4796,36,NULL,NULL,14,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4797,36,NULL,NULL,15,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4798,36,NULL,NULL,16,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4799,36,NULL,NULL,17,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4800,36,NULL,NULL,18,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4801,36,NULL,NULL,19,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4802,36,NULL,NULL,20,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4803,36,NULL,NULL,21,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4804,36,NULL,NULL,22,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4805,37,NULL,NULL,1,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4806,36,NULL,NULL,23,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4807,37,NULL,NULL,2,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4808,36,NULL,NULL,24,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4809,37,NULL,NULL,3,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4810,36,NULL,NULL,25,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4811,37,NULL,NULL,4,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4812,36,NULL,NULL,26,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4813,37,NULL,NULL,5,0,1,1,'2023-04-20 04:57:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4814,36,NULL,NULL,27,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4815,37,NULL,NULL,6,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4816,36,NULL,NULL,28,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4817,37,NULL,NULL,7,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4818,36,NULL,NULL,29,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4819,37,NULL,NULL,8,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4820,36,NULL,NULL,30,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4821,37,NULL,NULL,9,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4822,36,NULL,NULL,31,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4823,37,NULL,NULL,10,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4824,36,NULL,NULL,32,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4825,37,NULL,NULL,11,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4826,36,NULL,NULL,33,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4827,37,NULL,NULL,12,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4828,36,NULL,NULL,34,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4829,37,NULL,NULL,13,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4830,36,NULL,NULL,35,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4831,37,NULL,NULL,14,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4832,36,NULL,NULL,36,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4833,37,NULL,NULL,15,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4834,36,NULL,NULL,37,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4835,37,NULL,NULL,16,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4836,36,NULL,NULL,38,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4837,37,NULL,NULL,17,0,1,1,'2023-04-20 04:57:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4838,36,NULL,NULL,39,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4839,37,NULL,NULL,18,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4840,36,NULL,NULL,40,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4841,37,NULL,NULL,19,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4842,36,NULL,NULL,41,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4843,37,NULL,NULL,20,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4844,36,NULL,NULL,42,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4845,37,NULL,NULL,21,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4846,36,NULL,NULL,43,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4847,37,NULL,NULL,22,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4848,36,NULL,NULL,44,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4849,38,NULL,NULL,1,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4850,37,NULL,NULL,23,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4851,36,NULL,NULL,45,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4852,38,NULL,NULL,2,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4853,37,NULL,NULL,24,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4854,36,NULL,NULL,46,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4855,38,NULL,NULL,3,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4856,37,NULL,NULL,25,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4857,38,NULL,NULL,4,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4858,36,NULL,NULL,47,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4859,37,NULL,NULL,26,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4860,36,NULL,NULL,48,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4861,38,NULL,NULL,5,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4862,37,NULL,NULL,27,0,1,1,'2023-04-20 04:57:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4863,36,NULL,NULL,49,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4864,38,NULL,NULL,6,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4865,37,NULL,NULL,28,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4866,36,NULL,NULL,50,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4867,38,NULL,NULL,7,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4868,37,NULL,NULL,29,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4869,36,NULL,NULL,51,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4870,38,NULL,NULL,8,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4871,37,NULL,NULL,30,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4872,36,NULL,NULL,52,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4873,38,NULL,NULL,9,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4874,37,NULL,NULL,31,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4875,38,NULL,NULL,10,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4876,36,NULL,NULL,53,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4877,37,NULL,NULL,32,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4878,38,NULL,NULL,11,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4879,36,NULL,NULL,54,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4880,37,NULL,NULL,33,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4881,38,NULL,NULL,12,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4882,36,NULL,NULL,55,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4883,37,NULL,NULL,34,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4884,36,NULL,NULL,56,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4885,38,NULL,NULL,13,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4886,37,NULL,NULL,35,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4887,38,NULL,NULL,14,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4888,36,NULL,NULL,57,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4889,37,NULL,NULL,36,0,1,1,'2023-04-20 04:57:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4890,36,NULL,NULL,58,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4891,38,NULL,NULL,15,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4892,37,NULL,NULL,37,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4893,36,NULL,NULL,59,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4894,38,NULL,NULL,16,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4895,37,NULL,NULL,38,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4896,38,NULL,NULL,17,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4897,36,NULL,NULL,60,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4898,37,NULL,NULL,39,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4899,36,NULL,NULL,61,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4900,38,NULL,NULL,18,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4901,37,NULL,NULL,40,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4902,38,NULL,NULL,19,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4903,36,NULL,NULL,62,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4904,37,NULL,NULL,41,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4905,36,NULL,NULL,63,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4906,38,NULL,NULL,20,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4907,37,NULL,NULL,42,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4908,38,NULL,NULL,21,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4909,36,NULL,NULL,64,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4910,37,NULL,NULL,43,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4911,36,NULL,NULL,65,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4912,38,NULL,NULL,22,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4913,37,NULL,NULL,44,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4914,36,NULL,NULL,66,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4915,38,NULL,NULL,23,0,1,1,'2023-04-20 04:57:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4916,37,NULL,NULL,45,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4917,36,NULL,NULL,67,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4918,38,NULL,NULL,24,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4919,37,NULL,NULL,46,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4920,38,NULL,NULL,25,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4921,36,NULL,NULL,68,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4922,37,NULL,NULL,47,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4923,38,NULL,NULL,26,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4924,36,NULL,NULL,69,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4925,37,NULL,NULL,48,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4926,38,NULL,NULL,27,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4927,36,NULL,NULL,70,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4928,37,NULL,NULL,49,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4929,36,NULL,NULL,71,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4930,38,NULL,NULL,28,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4931,37,NULL,NULL,50,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4932,36,NULL,NULL,72,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4933,38,NULL,NULL,29,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4934,37,NULL,NULL,51,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4935,36,NULL,NULL,73,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4936,38,NULL,NULL,30,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4937,37,NULL,NULL,52,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4938,36,NULL,NULL,74,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4939,38,NULL,NULL,31,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4940,37,NULL,NULL,53,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4941,38,NULL,NULL,32,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4942,36,NULL,NULL,75,0,1,1,'2023-04-20 04:57:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4943,37,NULL,NULL,54,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4944,38,NULL,NULL,33,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4945,36,NULL,NULL,76,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4946,37,NULL,NULL,55,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4947,36,NULL,NULL,77,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4948,38,NULL,NULL,34,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4949,37,NULL,NULL,56,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4950,38,NULL,NULL,35,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4951,36,NULL,NULL,78,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4952,37,NULL,NULL,57,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4953,36,NULL,NULL,79,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4954,38,NULL,NULL,36,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4955,37,NULL,NULL,58,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4956,36,NULL,NULL,80,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4957,38,NULL,NULL,37,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4958,37,NULL,NULL,59,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4959,38,NULL,NULL,38,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4960,36,NULL,NULL,81,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4961,37,NULL,NULL,60,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4962,36,NULL,NULL,82,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4963,38,NULL,NULL,39,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4964,37,NULL,NULL,61,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4965,38,NULL,NULL,40,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4966,36,NULL,NULL,83,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4967,37,NULL,NULL,62,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4968,38,NULL,NULL,41,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4969,36,NULL,NULL,84,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4970,37,NULL,NULL,63,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4971,36,NULL,NULL,85,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4972,38,NULL,NULL,42,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4973,37,NULL,NULL,64,0,1,1,'2023-04-20 04:57:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4974,38,NULL,NULL,43,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4975,36,NULL,NULL,86,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4976,37,NULL,NULL,65,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4977,36,NULL,NULL,87,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4978,38,NULL,NULL,44,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4979,37,NULL,NULL,66,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4980,36,NULL,NULL,88,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4981,38,NULL,NULL,45,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4982,37,NULL,NULL,67,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4983,38,NULL,NULL,46,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4984,36,NULL,NULL,89,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4985,37,NULL,NULL,68,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4986,36,NULL,NULL,90,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4987,38,NULL,NULL,47,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4988,37,NULL,NULL,69,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4989,36,NULL,NULL,91,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4990,38,NULL,NULL,48,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4991,37,NULL,NULL,70,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4992,38,NULL,NULL,49,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4993,36,NULL,NULL,92,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4994,37,NULL,NULL,71,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4995,38,NULL,NULL,50,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4996,36,NULL,NULL,93,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4997,37,NULL,NULL,72,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4998,38,NULL,NULL,51,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (4999,36,NULL,NULL,94,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5000,37,NULL,NULL,73,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5001,36,NULL,NULL,95,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5002,38,NULL,NULL,52,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5003,37,NULL,NULL,74,0,1,1,'2023-04-20 04:57:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5004,36,NULL,NULL,96,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5005,38,NULL,NULL,53,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5006,37,NULL,NULL,75,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5007,36,NULL,NULL,97,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5008,38,NULL,NULL,54,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5009,37,NULL,NULL,76,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5010,36,NULL,NULL,98,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5011,38,NULL,NULL,55,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5012,37,NULL,NULL,77,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5013,36,NULL,NULL,99,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5014,38,NULL,NULL,56,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5015,37,NULL,NULL,78,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5016,36,NULL,NULL,100,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5017,38,NULL,NULL,57,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5018,37,NULL,NULL,79,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5019,36,NULL,NULL,101,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5020,38,NULL,NULL,58,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5021,37,NULL,NULL,80,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5022,36,NULL,NULL,102,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5023,38,NULL,NULL,59,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5024,37,NULL,NULL,81,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5025,38,NULL,NULL,60,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5026,36,NULL,NULL,103,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5027,37,NULL,NULL,82,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5028,36,NULL,NULL,104,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5029,38,NULL,NULL,61,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5030,37,NULL,NULL,83,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5031,36,NULL,NULL,105,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5032,38,NULL,NULL,62,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5033,37,NULL,NULL,84,0,1,1,'2023-04-20 04:57:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5034,38,NULL,NULL,63,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5035,36,NULL,NULL,106,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5036,37,NULL,NULL,85,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5037,36,NULL,NULL,107,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5038,38,NULL,NULL,64,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5039,37,NULL,NULL,86,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5040,36,NULL,NULL,108,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5041,38,NULL,NULL,65,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5042,37,NULL,NULL,87,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5043,36,NULL,NULL,109,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5044,38,NULL,NULL,66,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5045,37,NULL,NULL,88,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5046,38,NULL,NULL,67,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5047,36,NULL,NULL,110,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5048,37,NULL,NULL,89,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5049,36,NULL,NULL,111,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5050,38,NULL,NULL,68,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5051,37,NULL,NULL,90,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5052,36,NULL,NULL,112,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5053,38,NULL,NULL,69,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5054,37,NULL,NULL,91,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5055,39,NULL,NULL,1,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5056,38,NULL,NULL,70,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5057,36,NULL,NULL,113,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5058,37,NULL,NULL,92,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5059,38,NULL,NULL,71,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5060,36,NULL,NULL,114,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5061,39,NULL,NULL,2,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5062,37,NULL,NULL,93,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5063,36,NULL,NULL,115,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5064,39,NULL,NULL,3,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5065,38,NULL,NULL,72,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5066,37,NULL,NULL,94,0,1,1,'2023-04-20 04:57:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5067,36,NULL,NULL,116,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5068,39,NULL,NULL,4,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5069,38,NULL,NULL,73,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5070,37,NULL,NULL,95,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5071,36,NULL,NULL,117,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5072,39,NULL,NULL,5,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5073,38,NULL,NULL,74,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5074,37,NULL,NULL,96,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5075,39,NULL,NULL,6,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5076,38,NULL,NULL,75,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5077,36,NULL,NULL,118,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5078,37,NULL,NULL,97,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5079,39,NULL,NULL,7,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5080,38,NULL,NULL,76,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5081,36,NULL,NULL,119,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5082,37,NULL,NULL,98,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5083,38,NULL,NULL,77,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5084,36,NULL,NULL,120,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5085,39,NULL,NULL,8,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5086,37,NULL,NULL,99,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5087,36,NULL,NULL,121,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5088,39,NULL,NULL,9,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5089,38,NULL,NULL,78,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5090,37,NULL,NULL,100,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5091,39,NULL,NULL,10,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5092,38,NULL,NULL,79,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5093,36,NULL,NULL,122,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5094,37,NULL,NULL,101,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5095,38,NULL,NULL,80,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5096,36,NULL,NULL,123,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5097,39,NULL,NULL,11,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5098,37,NULL,NULL,102,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5099,36,NULL,NULL,124,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5100,39,NULL,NULL,12,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5101,38,NULL,NULL,81,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5102,37,NULL,NULL,103,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5103,39,NULL,NULL,13,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5104,38,NULL,NULL,82,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5105,36,NULL,NULL,125,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5106,37,NULL,NULL,104,0,1,1,'2023-04-20 04:57:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5107,38,NULL,NULL,83,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5108,36,NULL,NULL,126,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5109,39,NULL,NULL,14,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5110,37,NULL,NULL,105,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5111,36,NULL,NULL,127,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5112,39,NULL,NULL,15,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5113,38,NULL,NULL,84,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5114,37,NULL,NULL,106,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5115,39,NULL,NULL,16,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5116,38,NULL,NULL,85,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5117,36,NULL,NULL,128,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5118,37,NULL,NULL,107,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5119,38,NULL,NULL,86,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5120,36,NULL,NULL,129,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5121,39,NULL,NULL,17,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5122,37,NULL,NULL,108,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5123,36,NULL,NULL,130,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5124,39,NULL,NULL,18,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5125,38,NULL,NULL,87,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5126,37,NULL,NULL,109,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5127,39,NULL,NULL,19,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5128,38,NULL,NULL,88,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5129,36,NULL,NULL,131,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5130,37,NULL,NULL,110,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5131,38,NULL,NULL,89,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5132,36,NULL,NULL,132,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5133,39,NULL,NULL,20,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5134,37,NULL,NULL,111,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5135,36,NULL,NULL,133,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5136,39,NULL,NULL,21,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5137,38,NULL,NULL,90,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5138,37,NULL,NULL,112,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5139,39,NULL,NULL,22,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5140,38,NULL,NULL,91,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5141,36,NULL,NULL,134,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5142,37,NULL,NULL,113,0,1,1,'2023-04-20 04:57:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5143,38,NULL,NULL,92,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5144,36,NULL,NULL,135,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5145,39,NULL,NULL,23,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5146,37,NULL,NULL,114,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5147,36,NULL,NULL,136,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5148,39,NULL,NULL,24,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5149,38,NULL,NULL,93,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5150,37,NULL,NULL,115,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5151,39,NULL,NULL,25,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5152,38,NULL,NULL,94,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5153,36,NULL,NULL,137,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5154,37,NULL,NULL,116,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5155,38,NULL,NULL,95,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5156,36,NULL,NULL,138,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5157,39,NULL,NULL,26,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5158,36,NULL,NULL,139,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5159,39,NULL,NULL,27,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5160,37,NULL,NULL,117,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5161,38,NULL,NULL,96,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5162,36,NULL,NULL,140,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5163,37,NULL,NULL,118,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5164,38,NULL,NULL,97,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5165,39,NULL,NULL,28,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5166,36,NULL,NULL,141,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5167,39,NULL,NULL,29,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5168,38,NULL,NULL,98,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5169,37,NULL,NULL,119,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5170,36,NULL,NULL,142,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5171,38,NULL,NULL,99,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5172,37,NULL,NULL,120,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5173,39,NULL,NULL,30,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5174,36,NULL,NULL,143,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5175,37,NULL,NULL,121,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5176,39,NULL,NULL,31,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5177,38,NULL,NULL,100,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5178,36,NULL,NULL,144,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5179,39,NULL,NULL,32,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5180,38,NULL,NULL,101,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5181,37,NULL,NULL,122,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5182,36,NULL,NULL,145,0,1,1,'2023-04-20 04:57:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5183,38,NULL,NULL,102,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5184,37,NULL,NULL,123,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5185,39,NULL,NULL,33,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5186,36,NULL,NULL,146,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5187,37,NULL,NULL,124,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5188,39,NULL,NULL,34,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5189,38,NULL,NULL,103,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5190,36,NULL,NULL,147,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5191,39,NULL,NULL,35,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5192,38,NULL,NULL,104,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5193,37,NULL,NULL,125,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5194,36,NULL,NULL,148,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5195,38,NULL,NULL,105,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5196,37,NULL,NULL,126,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5197,39,NULL,NULL,36,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5198,36,NULL,NULL,149,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5199,37,NULL,NULL,127,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5200,39,NULL,NULL,37,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5201,38,NULL,NULL,106,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5202,36,NULL,NULL,150,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5203,39,NULL,NULL,38,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5204,38,NULL,NULL,107,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5205,37,NULL,NULL,128,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5206,36,NULL,NULL,151,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5207,38,NULL,NULL,108,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5208,37,NULL,NULL,129,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5209,39,NULL,NULL,39,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5210,36,NULL,NULL,152,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5211,37,NULL,NULL,130,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5212,39,NULL,NULL,40,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5213,38,NULL,NULL,109,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5214,36,NULL,NULL,153,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5215,39,NULL,NULL,41,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5216,38,NULL,NULL,110,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5217,37,NULL,NULL,131,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5218,36,NULL,NULL,154,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5219,37,NULL,NULL,132,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5220,38,NULL,NULL,111,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5221,39,NULL,NULL,42,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5222,36,NULL,NULL,155,0,1,1,'2023-04-20 04:57:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5223,39,NULL,NULL,43,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5224,38,NULL,NULL,112,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5225,37,NULL,NULL,133,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5226,36,NULL,NULL,156,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5227,37,NULL,NULL,134,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5228,38,NULL,NULL,113,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5229,39,NULL,NULL,44,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5230,36,NULL,NULL,157,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5231,39,NULL,NULL,45,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5232,38,NULL,NULL,114,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5233,37,NULL,NULL,135,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5234,36,NULL,NULL,158,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5235,37,NULL,NULL,136,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5236,38,NULL,NULL,115,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5237,39,NULL,NULL,46,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5238,36,NULL,NULL,159,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5239,39,NULL,NULL,47,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5240,38,NULL,NULL,116,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5241,37,NULL,NULL,137,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5242,36,NULL,NULL,160,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5243,37,NULL,NULL,138,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5244,38,NULL,NULL,117,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5245,39,NULL,NULL,48,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5246,36,NULL,NULL,161,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5247,39,NULL,NULL,49,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5248,38,NULL,NULL,118,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5249,37,NULL,NULL,139,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5250,36,NULL,NULL,162,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5251,37,NULL,NULL,140,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5252,38,NULL,NULL,119,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5253,39,NULL,NULL,50,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5254,36,NULL,NULL,163,0,1,1,'2023-04-20 04:57:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5255,39,NULL,NULL,51,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5256,38,NULL,NULL,120,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5257,37,NULL,NULL,141,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5258,36,NULL,NULL,164,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5259,37,NULL,NULL,142,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5260,38,NULL,NULL,121,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5261,39,NULL,NULL,52,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5262,36,NULL,NULL,165,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5263,38,NULL,NULL,122,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5264,39,NULL,NULL,53,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5265,37,NULL,NULL,143,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5266,36,NULL,NULL,166,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5267,37,NULL,NULL,144,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5268,39,NULL,NULL,54,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5269,38,NULL,NULL,123,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5270,36,NULL,NULL,167,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5271,39,NULL,NULL,55,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5272,38,NULL,NULL,124,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5273,37,NULL,NULL,145,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5274,36,NULL,NULL,168,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5275,38,NULL,NULL,125,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5276,37,NULL,NULL,146,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5277,39,NULL,NULL,56,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5278,36,NULL,NULL,169,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5279,37,NULL,NULL,147,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5280,39,NULL,NULL,57,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5281,38,NULL,NULL,126,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5282,36,NULL,NULL,170,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5283,39,NULL,NULL,58,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5284,38,NULL,NULL,127,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5285,37,NULL,NULL,148,0,1,1,'2023-04-20 04:57:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5286,36,NULL,NULL,171,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5287,37,NULL,NULL,149,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5288,38,NULL,NULL,128,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5289,39,NULL,NULL,59,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5290,36,NULL,NULL,172,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5291,38,NULL,NULL,129,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5292,39,NULL,NULL,60,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5293,37,NULL,NULL,150,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5294,36,NULL,NULL,173,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5295,39,NULL,NULL,61,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5296,37,NULL,NULL,151,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5297,38,NULL,NULL,130,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5298,36,NULL,NULL,174,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5299,37,NULL,NULL,152,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5300,38,NULL,NULL,131,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5301,39,NULL,NULL,62,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5302,36,NULL,NULL,175,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5303,39,NULL,NULL,63,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5304,38,NULL,NULL,132,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5305,37,NULL,NULL,153,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5306,36,NULL,NULL,176,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5307,38,NULL,NULL,133,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5308,37,NULL,NULL,154,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5309,39,NULL,NULL,64,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5310,36,NULL,NULL,177,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5311,37,NULL,NULL,155,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5312,39,NULL,NULL,65,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5313,38,NULL,NULL,134,0,1,1,'2023-04-20 04:57:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5314,36,NULL,NULL,178,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5315,39,NULL,NULL,66,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5316,38,NULL,NULL,135,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5317,37,NULL,NULL,156,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5318,36,NULL,NULL,179,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5319,38,NULL,NULL,136,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5320,37,NULL,NULL,157,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5321,39,NULL,NULL,67,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5322,36,NULL,NULL,180,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5323,37,NULL,NULL,158,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5324,39,NULL,NULL,68,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5325,38,NULL,NULL,137,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5326,36,NULL,NULL,181,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5327,39,NULL,NULL,69,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5328,38,NULL,NULL,138,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5329,37,NULL,NULL,159,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5330,36,NULL,NULL,182,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5331,38,NULL,NULL,139,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5332,37,NULL,NULL,160,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5333,39,NULL,NULL,70,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5334,36,NULL,NULL,183,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5336,37,NULL,NULL,161,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5337,39,NULL,NULL,71,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5338,38,NULL,NULL,140,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5339,36,NULL,NULL,184,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5341,37,NULL,NULL,162,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5342,38,NULL,NULL,141,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5343,39,NULL,NULL,72,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5344,36,NULL,NULL,185,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5346,38,NULL,NULL,142,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5347,39,NULL,NULL,73,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5348,37,NULL,NULL,163,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5349,36,NULL,NULL,186,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5351,39,NULL,NULL,74,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5352,37,NULL,NULL,164,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5353,38,NULL,NULL,143,0,1,1,'2023-04-20 04:57:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5354,36,NULL,NULL,187,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5356,37,NULL,NULL,165,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5357,38,NULL,NULL,144,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5358,39,NULL,NULL,75,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5359,36,NULL,NULL,188,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5361,39,NULL,NULL,76,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5362,38,NULL,NULL,145,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5363,37,NULL,NULL,166,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5364,36,NULL,NULL,189,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5366,38,NULL,NULL,146,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5367,37,NULL,NULL,167,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5368,39,NULL,NULL,77,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5369,36,NULL,NULL,190,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5371,37,NULL,NULL,168,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5372,39,NULL,NULL,78,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5373,38,NULL,NULL,147,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5374,36,NULL,NULL,191,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5376,38,NULL,NULL,148,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5377,39,NULL,NULL,79,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5378,37,NULL,NULL,169,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5379,36,NULL,NULL,192,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5381,39,NULL,NULL,80,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5382,37,NULL,NULL,170,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5383,38,NULL,NULL,149,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5384,36,NULL,NULL,193,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5386,37,NULL,NULL,171,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5387,38,NULL,NULL,150,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5388,39,NULL,NULL,81,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5389,36,NULL,NULL,194,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5391,38,NULL,NULL,151,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5392,39,NULL,NULL,82,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5393,37,NULL,NULL,172,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5395,36,NULL,NULL,195,0,1,1,'2023-04-20 04:57:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5396,39,NULL,NULL,83,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5397,37,NULL,NULL,173,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5398,38,NULL,NULL,152,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5400,36,NULL,NULL,196,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5401,37,NULL,NULL,174,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5402,38,NULL,NULL,153,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5403,39,NULL,NULL,84,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5404,36,NULL,NULL,197,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5406,38,NULL,NULL,154,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5407,39,NULL,NULL,85,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5408,37,NULL,NULL,175,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5409,36,NULL,NULL,198,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5411,37,NULL,NULL,176,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5412,39,NULL,NULL,86,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5413,38,NULL,NULL,155,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5415,36,NULL,NULL,199,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5416,39,NULL,NULL,87,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5417,38,NULL,NULL,156,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5418,37,NULL,NULL,177,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5419,36,NULL,NULL,200,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5421,37,NULL,NULL,178,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5422,38,NULL,NULL,157,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5423,39,NULL,NULL,88,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5425,36,NULL,NULL,201,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5426,39,NULL,NULL,89,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5427,38,NULL,NULL,158,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5428,37,NULL,NULL,179,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5429,36,NULL,NULL,202,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5431,37,NULL,NULL,180,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5432,38,NULL,NULL,159,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5433,39,NULL,NULL,90,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5435,36,NULL,NULL,203,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5436,39,NULL,NULL,91,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5437,38,NULL,NULL,160,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5438,37,NULL,NULL,181,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5439,36,NULL,NULL,204,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5441,37,NULL,NULL,182,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5442,38,NULL,NULL,161,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5443,39,NULL,NULL,92,0,1,1,'2023-04-20 04:57:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5445,36,NULL,NULL,205,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5446,39,NULL,NULL,93,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5447,38,NULL,NULL,162,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5448,37,NULL,NULL,183,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5449,36,NULL,NULL,206,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5451,37,NULL,NULL,184,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5452,38,NULL,NULL,163,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5453,39,NULL,NULL,94,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5455,36,NULL,NULL,207,0,1,1,'2023-04-20 04:57:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5456,38,NULL,NULL,164,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5457,39,NULL,NULL,95,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5458,37,NULL,NULL,185,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5460,36,NULL,NULL,208,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5461,39,NULL,NULL,96,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5462,37,NULL,NULL,186,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5463,38,NULL,NULL,165,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5464,36,NULL,NULL,209,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5466,37,NULL,NULL,187,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5467,38,NULL,NULL,166,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5468,39,NULL,NULL,97,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5470,36,NULL,NULL,210,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5471,39,NULL,NULL,98,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5472,38,NULL,NULL,167,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5473,37,NULL,NULL,188,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5474,36,NULL,NULL,211,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5476,37,NULL,NULL,189,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5477,38,NULL,NULL,168,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5478,39,NULL,NULL,99,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5480,36,NULL,NULL,212,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5481,39,NULL,NULL,100,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5482,38,NULL,NULL,169,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5483,37,NULL,NULL,190,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5484,36,NULL,NULL,213,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5486,37,NULL,NULL,191,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5487,38,NULL,NULL,170,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5488,39,NULL,NULL,101,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5490,36,NULL,NULL,214,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5491,39,NULL,NULL,102,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5492,38,NULL,NULL,171,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5493,37,NULL,NULL,192,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5494,36,NULL,NULL,215,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5496,37,NULL,NULL,193,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5497,38,NULL,NULL,172,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5498,39,NULL,NULL,103,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5500,36,NULL,NULL,216,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5501,39,NULL,NULL,104,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5502,38,NULL,NULL,173,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5503,37,NULL,NULL,194,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5504,36,NULL,NULL,217,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5506,37,NULL,NULL,195,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5507,38,NULL,NULL,174,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5508,39,NULL,NULL,105,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5509,36,NULL,NULL,218,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5511,39,NULL,NULL,106,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5512,38,NULL,NULL,175,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5513,37,NULL,NULL,196,0,1,1,'2023-04-20 04:57:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5514,36,NULL,NULL,219,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5516,37,NULL,NULL,197,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5517,38,NULL,NULL,176,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5518,39,NULL,NULL,107,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5520,36,NULL,NULL,220,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5521,39,NULL,NULL,108,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5522,38,NULL,NULL,177,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5523,37,NULL,NULL,198,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5524,36,NULL,NULL,221,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5526,37,NULL,NULL,199,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5527,38,NULL,NULL,178,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5528,39,NULL,NULL,109,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5530,36,NULL,NULL,222,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5531,39,NULL,NULL,110,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5532,38,NULL,NULL,179,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5533,37,NULL,NULL,200,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5534,36,NULL,NULL,223,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5536,37,NULL,NULL,201,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5537,38,NULL,NULL,180,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5538,39,NULL,NULL,111,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5540,36,NULL,NULL,224,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5541,39,NULL,NULL,112,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5542,38,NULL,NULL,181,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5543,37,NULL,NULL,202,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5544,36,NULL,NULL,225,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5546,37,NULL,NULL,203,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5547,38,NULL,NULL,182,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5548,39,NULL,NULL,113,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5550,36,NULL,NULL,226,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5551,39,NULL,NULL,114,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5552,38,NULL,NULL,183,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5553,37,NULL,NULL,204,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5554,36,NULL,NULL,227,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5556,38,NULL,NULL,184,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5557,37,NULL,NULL,205,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5558,39,NULL,NULL,115,0,1,1,'2023-04-20 04:57:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5560,36,NULL,NULL,228,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5561,39,NULL,NULL,116,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5562,37,NULL,NULL,206,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5563,38,NULL,NULL,185,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5564,36,NULL,NULL,229,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5566,37,NULL,NULL,207,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5567,38,NULL,NULL,186,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5568,39,NULL,NULL,117,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5570,36,NULL,NULL,230,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5571,39,NULL,NULL,118,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5572,38,NULL,NULL,187,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5573,37,NULL,NULL,208,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5574,36,NULL,NULL,231,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5576,38,NULL,NULL,188,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5577,37,NULL,NULL,209,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5578,39,NULL,NULL,119,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5580,36,NULL,NULL,232,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5581,39,NULL,NULL,120,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5582,37,NULL,NULL,210,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5583,38,NULL,NULL,189,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5584,36,NULL,NULL,233,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5586,37,NULL,NULL,211,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5587,38,NULL,NULL,190,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5588,39,NULL,NULL,121,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5590,36,NULL,NULL,234,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5591,39,NULL,NULL,122,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5592,38,NULL,NULL,191,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5593,37,NULL,NULL,212,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5594,36,NULL,NULL,235,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5596,37,NULL,NULL,213,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5597,38,NULL,NULL,192,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5598,39,NULL,NULL,123,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5600,36,NULL,NULL,236,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5601,38,NULL,NULL,193,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5602,39,NULL,NULL,124,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5603,37,NULL,NULL,214,0,1,1,'2023-04-20 04:57:50',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5605,36,NULL,NULL,237,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5606,37,NULL,NULL,215,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5607,39,NULL,NULL,125,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5608,38,NULL,NULL,194,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5609,36,NULL,NULL,238,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5611,39,NULL,NULL,126,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5612,38,NULL,NULL,195,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5613,37,NULL,NULL,216,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5615,36,NULL,NULL,239,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5616,37,NULL,NULL,217,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5617,38,NULL,NULL,196,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5618,39,NULL,NULL,127,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5619,36,NULL,NULL,240,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5621,39,NULL,NULL,128,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5622,38,NULL,NULL,197,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5623,37,NULL,NULL,218,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5625,36,NULL,NULL,241,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5626,37,NULL,NULL,219,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5627,38,NULL,NULL,198,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5628,39,NULL,NULL,129,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5629,36,NULL,NULL,242,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5631,39,NULL,NULL,130,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5632,38,NULL,NULL,199,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5633,37,NULL,NULL,220,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5635,36,NULL,NULL,243,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5636,37,NULL,NULL,221,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5637,38,NULL,NULL,200,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5638,39,NULL,NULL,131,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5639,36,NULL,NULL,244,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5641,38,NULL,NULL,201,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5642,39,NULL,NULL,132,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5643,37,NULL,NULL,222,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5645,36,NULL,NULL,245,0,1,1,'2023-04-20 04:57:51',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5646,37,NULL,NULL,223,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5647,39,NULL,NULL,133,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5648,38,NULL,NULL,202,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5649,36,NULL,NULL,246,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5651,38,NULL,NULL,203,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5652,37,NULL,NULL,224,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5653,39,NULL,NULL,134,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5655,36,NULL,NULL,247,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5656,39,NULL,NULL,135,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5657,37,NULL,NULL,225,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5658,38,NULL,NULL,204,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5659,36,NULL,NULL,248,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5661,37,NULL,NULL,226,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5662,38,NULL,NULL,205,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5663,39,NULL,NULL,136,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5665,36,NULL,NULL,249,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5666,39,NULL,NULL,137,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5667,38,NULL,NULL,206,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5668,37,NULL,NULL,227,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5669,36,NULL,NULL,250,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5671,37,NULL,NULL,228,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5672,38,NULL,NULL,207,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5673,39,NULL,NULL,138,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5675,36,NULL,NULL,251,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5676,38,NULL,NULL,208,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5677,39,NULL,NULL,139,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5678,37,NULL,NULL,229,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5679,36,NULL,NULL,252,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5681,37,NULL,NULL,230,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5682,39,NULL,NULL,140,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5683,38,NULL,NULL,209,0,1,1,'2023-04-20 04:57:52',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5685,36,NULL,NULL,253,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5686,39,NULL,NULL,141,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5687,38,NULL,NULL,210,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5688,37,NULL,NULL,231,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5689,36,NULL,NULL,254,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5691,37,NULL,NULL,232,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5692,38,NULL,NULL,211,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5693,39,NULL,NULL,142,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5695,36,NULL,NULL,255,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5696,39,NULL,NULL,143,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5697,38,NULL,NULL,212,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5698,37,NULL,NULL,233,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5699,36,NULL,NULL,256,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5701,39,NULL,NULL,144,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5703,36,NULL,NULL,257,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5704,37,NULL,NULL,234,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5705,38,NULL,NULL,213,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5706,39,NULL,NULL,145,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5707,37,NULL,NULL,235,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5708,38,NULL,NULL,214,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5709,36,NULL,NULL,258,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5711,39,NULL,NULL,146,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5713,36,NULL,NULL,259,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5714,38,NULL,NULL,215,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5715,37,NULL,NULL,236,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5716,39,NULL,NULL,147,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5717,37,NULL,NULL,237,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5718,36,NULL,NULL,260,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5719,38,NULL,NULL,216,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5721,39,NULL,NULL,148,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5723,36,NULL,NULL,261,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5724,38,NULL,NULL,217,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5725,37,NULL,NULL,238,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5726,39,NULL,NULL,149,0,1,1,'2023-04-20 04:57:53',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5727,37,NULL,NULL,239,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5728,36,NULL,NULL,262,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5729,38,NULL,NULL,218,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5731,39,NULL,NULL,150,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5733,36,NULL,NULL,263,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5734,37,NULL,NULL,240,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5735,38,NULL,NULL,219,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5736,39,NULL,NULL,151,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5737,37,NULL,NULL,241,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5738,36,NULL,NULL,264,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5739,38,NULL,NULL,220,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5741,39,NULL,NULL,152,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5742,36,NULL,NULL,265,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5744,38,NULL,NULL,221,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5745,37,NULL,NULL,242,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5746,39,NULL,NULL,153,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5747,37,NULL,NULL,243,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5749,38,NULL,NULL,222,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5750,36,NULL,NULL,266,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5751,39,NULL,NULL,154,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5752,36,NULL,NULL,267,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5754,38,NULL,NULL,223,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5755,37,NULL,NULL,244,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5756,39,NULL,NULL,155,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5757,37,NULL,NULL,245,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5759,38,NULL,NULL,224,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5760,36,NULL,NULL,268,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5761,39,NULL,NULL,156,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5762,36,NULL,NULL,269,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5764,38,NULL,NULL,225,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5765,37,NULL,NULL,246,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5766,39,NULL,NULL,157,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5767,37,NULL,NULL,247,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5769,36,NULL,NULL,270,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5770,38,NULL,NULL,226,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5771,39,NULL,NULL,158,0,1,1,'2023-04-20 04:57:54',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5773,36,NULL,NULL,271,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5774,38,NULL,NULL,227,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5775,37,NULL,NULL,248,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5776,39,NULL,NULL,159,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5777,37,NULL,NULL,249,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5778,36,NULL,NULL,272,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5779,38,NULL,NULL,228,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5781,39,NULL,NULL,160,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5783,36,NULL,NULL,273,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5784,38,NULL,NULL,229,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5785,37,NULL,NULL,250,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5786,39,NULL,NULL,161,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5787,37,NULL,NULL,251,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5788,36,NULL,NULL,274,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5789,38,NULL,NULL,230,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5791,39,NULL,NULL,162,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5792,36,NULL,NULL,275,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5793,38,NULL,NULL,231,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5794,37,NULL,NULL,252,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5796,39,NULL,NULL,163,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5797,37,NULL,NULL,253,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5799,36,NULL,NULL,276,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5800,38,NULL,NULL,232,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5801,39,NULL,NULL,164,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5802,36,NULL,NULL,277,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5804,38,NULL,NULL,233,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5805,37,NULL,NULL,254,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5806,39,NULL,NULL,165,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5807,37,NULL,NULL,255,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5809,38,NULL,NULL,234,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5810,36,NULL,NULL,278,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5811,39,NULL,NULL,166,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5812,36,NULL,NULL,279,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5814,38,NULL,NULL,235,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5815,37,NULL,NULL,256,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5816,39,NULL,NULL,167,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5817,37,NULL,NULL,257,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5819,38,NULL,NULL,236,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5820,36,NULL,NULL,280,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5821,39,NULL,NULL,168,0,1,1,'2023-04-20 04:57:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5822,36,NULL,NULL,281,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5824,38,NULL,NULL,237,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5825,37,NULL,NULL,258,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5826,39,NULL,NULL,169,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5827,37,NULL,NULL,259,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5829,38,NULL,NULL,238,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5830,36,NULL,NULL,282,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5831,39,NULL,NULL,170,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5832,36,NULL,NULL,283,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5834,38,NULL,NULL,239,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5835,37,NULL,NULL,260,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5836,39,NULL,NULL,171,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5837,37,NULL,NULL,261,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5838,38,NULL,NULL,240,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5840,36,NULL,NULL,284,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5841,39,NULL,NULL,172,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5842,36,NULL,NULL,285,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5844,38,NULL,NULL,241,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5845,37,NULL,NULL,262,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5846,39,NULL,NULL,173,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5847,37,NULL,NULL,263,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5849,38,NULL,NULL,242,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5850,36,NULL,NULL,286,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5851,39,NULL,NULL,174,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5852,36,NULL,NULL,287,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5854,38,NULL,NULL,243,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5855,37,NULL,NULL,264,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5856,39,NULL,NULL,175,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5857,37,NULL,NULL,265,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5859,38,NULL,NULL,244,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5860,36,NULL,NULL,288,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5861,39,NULL,NULL,176,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5862,36,NULL,NULL,289,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5864,38,NULL,NULL,245,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5865,37,NULL,NULL,266,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5866,39,NULL,NULL,177,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5867,37,NULL,NULL,267,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5869,38,NULL,NULL,246,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5870,36,NULL,NULL,290,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5871,39,NULL,NULL,178,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5872,36,NULL,NULL,291,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5873,38,NULL,NULL,247,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5875,37,NULL,NULL,268,0,1,1,'2023-04-20 04:57:56',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5876,39,NULL,NULL,179,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5877,37,NULL,NULL,269,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5878,38,NULL,NULL,248,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5879,36,NULL,NULL,292,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5881,39,NULL,NULL,180,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5883,36,NULL,NULL,293,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5884,38,NULL,NULL,249,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5885,37,NULL,NULL,270,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5886,39,NULL,NULL,181,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5887,37,NULL,NULL,271,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5888,36,NULL,NULL,294,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5889,38,NULL,NULL,250,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5891,39,NULL,NULL,182,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5893,36,NULL,NULL,295,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5894,38,NULL,NULL,251,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5895,37,NULL,NULL,272,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5896,39,NULL,NULL,183,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5897,37,NULL,NULL,273,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5898,36,NULL,NULL,296,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5899,38,NULL,NULL,252,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5901,39,NULL,NULL,184,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5902,36,NULL,NULL,297,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5903,37,NULL,NULL,274,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5905,38,NULL,NULL,253,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5906,39,NULL,NULL,185,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5908,37,NULL,NULL,275,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5909,38,NULL,NULL,254,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5910,36,NULL,NULL,298,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5911,39,NULL,NULL,186,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5912,36,NULL,NULL,299,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5913,37,NULL,NULL,276,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5914,38,NULL,NULL,255,0,1,1,'2023-04-20 04:57:57',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5916,39,NULL,NULL,187,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5918,37,NULL,NULL,277,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5919,38,NULL,NULL,256,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5920,36,NULL,NULL,300,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5921,39,NULL,NULL,188,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5922,36,NULL,NULL,301,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5923,37,NULL,NULL,278,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5924,38,NULL,NULL,257,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5926,39,NULL,NULL,189,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5928,37,NULL,NULL,279,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5929,38,NULL,NULL,258,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5930,36,NULL,NULL,302,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5931,39,NULL,NULL,190,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5932,36,NULL,NULL,303,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5933,37,NULL,NULL,280,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5934,38,NULL,NULL,259,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5936,39,NULL,NULL,191,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5937,37,NULL,NULL,281,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5939,38,NULL,NULL,260,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5940,36,NULL,NULL,304,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5941,39,NULL,NULL,192,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5942,37,NULL,NULL,282,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5943,36,NULL,NULL,305,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5944,38,NULL,NULL,261,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5946,39,NULL,NULL,193,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5947,37,NULL,NULL,283,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5948,39,NULL,NULL,194,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5950,38,NULL,NULL,262,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5951,36,NULL,NULL,306,0,1,1,'2023-04-20 04:57:58',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5952,37,NULL,NULL,284,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5953,36,NULL,NULL,307,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5955,38,NULL,NULL,263,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5956,39,NULL,NULL,195,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5957,37,NULL,NULL,285,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5958,39,NULL,NULL,196,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5960,38,NULL,NULL,264,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5961,36,NULL,NULL,308,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5962,37,NULL,NULL,286,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5963,36,NULL,NULL,309,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5965,38,NULL,NULL,265,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5966,39,NULL,NULL,197,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5967,37,NULL,NULL,287,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5968,39,NULL,NULL,198,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5970,38,NULL,NULL,266,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5971,36,NULL,NULL,310,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5972,37,NULL,NULL,288,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5973,36,NULL,NULL,311,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5975,38,NULL,NULL,267,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5976,39,NULL,NULL,199,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5977,37,NULL,NULL,289,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5978,39,NULL,NULL,200,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5980,38,NULL,NULL,268,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5981,36,NULL,NULL,312,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5982,37,NULL,NULL,290,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5984,39,NULL,NULL,201,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5985,37,NULL,NULL,291,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5986,36,NULL,NULL,313,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5987,38,NULL,NULL,269,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5988,39,NULL,NULL,202,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5990,36,NULL,NULL,314,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5991,38,NULL,NULL,270,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5992,37,NULL,NULL,292,0,1,1,'2023-04-20 04:57:59',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5994,39,NULL,NULL,203,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5995,37,NULL,NULL,293,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5996,38,NULL,NULL,271,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5997,36,NULL,NULL,315,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (5998,39,NULL,NULL,204,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6000,36,NULL,NULL,316,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6001,38,NULL,NULL,272,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6002,37,NULL,NULL,294,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6004,39,NULL,NULL,205,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6005,37,NULL,NULL,295,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6006,38,NULL,NULL,273,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6007,36,NULL,NULL,317,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6008,39,NULL,NULL,206,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6010,36,NULL,NULL,318,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6011,38,NULL,NULL,274,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6012,37,NULL,NULL,296,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6014,39,NULL,NULL,207,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6015,37,NULL,NULL,297,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6016,36,NULL,NULL,319,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6017,38,NULL,NULL,275,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6018,39,NULL,NULL,208,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6020,36,NULL,NULL,320,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6021,38,NULL,NULL,276,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6022,37,NULL,NULL,298,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6024,39,NULL,NULL,209,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6025,37,NULL,NULL,299,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6026,38,NULL,NULL,277,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6027,36,NULL,NULL,321,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6028,39,NULL,NULL,210,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6030,36,NULL,NULL,322,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6031,38,NULL,NULL,278,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6032,37,NULL,NULL,300,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6034,39,NULL,NULL,211,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6035,37,NULL,NULL,301,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6036,38,NULL,NULL,279,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6037,36,NULL,NULL,323,0,1,1,'2023-04-20 04:58:00',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6038,39,NULL,NULL,212,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6040,36,NULL,NULL,324,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6041,38,NULL,NULL,280,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6042,37,NULL,NULL,302,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6044,39,NULL,NULL,213,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6045,37,NULL,NULL,303,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6046,38,NULL,NULL,281,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6047,36,NULL,NULL,325,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6048,39,NULL,NULL,214,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6050,38,NULL,NULL,282,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6051,36,NULL,NULL,326,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6052,37,NULL,NULL,304,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6054,39,NULL,NULL,215,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6055,37,NULL,NULL,305,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6056,36,NULL,NULL,327,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6057,38,NULL,NULL,283,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6059,39,NULL,NULL,216,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6060,38,NULL,NULL,284,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6061,36,NULL,NULL,328,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6062,37,NULL,NULL,306,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6063,39,NULL,NULL,217,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6065,37,NULL,NULL,307,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6066,36,NULL,NULL,329,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6067,38,NULL,NULL,285,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6069,39,NULL,NULL,218,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6070,36,NULL,NULL,330,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6071,38,NULL,NULL,286,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6072,37,NULL,NULL,308,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6073,39,NULL,NULL,219,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6075,37,NULL,NULL,309,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6076,38,NULL,NULL,287,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6077,36,NULL,NULL,331,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6079,39,NULL,NULL,220,0,1,1,'2023-04-20 04:58:01',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6080,36,NULL,NULL,332,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6081,38,NULL,NULL,288,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6082,37,NULL,NULL,310,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6083,39,NULL,NULL,221,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6085,37,NULL,NULL,311,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6086,38,NULL,NULL,289,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6087,36,NULL,NULL,333,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6089,39,NULL,NULL,222,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6090,36,NULL,NULL,334,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6091,38,NULL,NULL,290,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6092,37,NULL,NULL,312,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6093,39,NULL,NULL,223,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6095,37,NULL,NULL,313,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6096,38,NULL,NULL,291,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6097,36,NULL,NULL,335,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6099,39,NULL,NULL,224,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6100,36,NULL,NULL,336,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6101,38,NULL,NULL,292,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6102,37,NULL,NULL,314,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6103,39,NULL,NULL,225,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6105,37,NULL,NULL,315,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6106,38,NULL,NULL,293,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6107,36,NULL,NULL,337,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6109,39,NULL,NULL,226,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6110,37,NULL,NULL,316,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6111,36,NULL,NULL,338,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6112,38,NULL,NULL,294,0,1,1,'2023-04-20 04:58:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6114,39,NULL,NULL,227,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6115,38,NULL,NULL,295,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6116,36,NULL,NULL,339,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6117,37,NULL,NULL,317,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6118,39,NULL,NULL,228,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6120,37,NULL,NULL,318,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6121,36,NULL,NULL,340,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6122,38,NULL,NULL,296,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6124,39,NULL,NULL,229,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6125,36,NULL,NULL,341,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6126,37,NULL,NULL,319,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6127,38,NULL,NULL,297,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6128,39,NULL,NULL,230,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6130,38,NULL,NULL,298,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6131,37,NULL,NULL,320,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6132,36,NULL,NULL,342,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6134,39,NULL,NULL,231,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6135,36,NULL,NULL,343,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6136,37,NULL,NULL,321,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6137,38,NULL,NULL,299,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6138,39,NULL,NULL,232,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6140,38,NULL,NULL,300,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6141,37,NULL,NULL,322,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6142,36,NULL,NULL,344,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6144,39,NULL,NULL,233,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6145,36,NULL,NULL,345,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6146,37,NULL,NULL,323,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6147,38,NULL,NULL,301,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6148,39,NULL,NULL,234,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6150,37,NULL,NULL,324,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6151,38,NULL,NULL,302,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6152,36,NULL,NULL,346,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6153,39,NULL,NULL,235,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6155,36,NULL,NULL,347,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6156,38,NULL,NULL,303,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6157,37,NULL,NULL,325,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6159,39,NULL,NULL,236,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6160,37,NULL,NULL,326,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6161,38,NULL,NULL,304,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6162,36,NULL,NULL,348,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6163,39,NULL,NULL,237,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6165,36,NULL,NULL,349,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6166,38,NULL,NULL,305,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6167,37,NULL,NULL,327,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6169,39,NULL,NULL,238,0,1,1,'2023-04-20 04:58:03',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6170,37,NULL,NULL,328,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6171,38,NULL,NULL,306,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6172,36,NULL,NULL,350,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6173,39,NULL,NULL,239,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6175,36,NULL,NULL,351,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6176,38,NULL,NULL,307,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6177,37,NULL,NULL,329,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6179,39,NULL,NULL,240,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6180,37,NULL,NULL,330,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6181,38,NULL,NULL,308,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6182,36,NULL,NULL,352,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6183,39,NULL,NULL,241,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6185,36,NULL,NULL,353,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6186,38,NULL,NULL,309,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6187,37,NULL,NULL,331,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6189,39,NULL,NULL,242,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6190,37,NULL,NULL,332,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6191,38,NULL,NULL,310,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6192,36,NULL,NULL,354,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6193,39,NULL,NULL,243,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6195,36,NULL,NULL,355,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6196,38,NULL,NULL,311,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6197,37,NULL,NULL,333,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6199,39,NULL,NULL,244,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6200,37,NULL,NULL,334,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6201,38,NULL,NULL,312,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6202,36,NULL,NULL,356,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6203,39,NULL,NULL,245,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6205,36,NULL,NULL,357,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6206,38,NULL,NULL,313,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6207,37,NULL,NULL,335,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6209,39,NULL,NULL,246,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6210,37,NULL,NULL,336,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6211,38,NULL,NULL,314,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6212,36,NULL,NULL,358,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6213,39,NULL,NULL,247,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6215,36,NULL,NULL,359,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6216,38,NULL,NULL,315,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6217,37,NULL,NULL,337,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6219,39,NULL,NULL,248,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6220,37,NULL,NULL,338,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6221,38,NULL,NULL,316,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6222,36,NULL,NULL,360,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6223,39,NULL,NULL,249,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6225,36,NULL,NULL,361,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6226,38,NULL,NULL,317,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6227,37,NULL,NULL,339,0,1,1,'2023-04-20 04:58:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6229,39,NULL,NULL,250,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6230,37,NULL,NULL,340,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6231,38,NULL,NULL,318,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6232,36,NULL,NULL,362,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6233,39,NULL,NULL,251,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6235,36,NULL,NULL,363,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6236,38,NULL,NULL,319,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6237,37,NULL,NULL,341,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6239,39,NULL,NULL,252,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6240,37,NULL,NULL,342,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6241,38,NULL,NULL,320,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6242,36,NULL,NULL,364,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6243,37,NULL,NULL,343,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6245,38,NULL,NULL,321,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6246,39,NULL,NULL,253,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6247,36,NULL,NULL,365,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6248,37,NULL,NULL,344,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6249,36,NULL,NULL,366,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6250,39,NULL,NULL,254,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6251,38,NULL,NULL,322,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6253,37,NULL,NULL,345,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6255,39,NULL,NULL,255,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6256,38,NULL,NULL,323,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6257,36,NULL,NULL,367,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6258,37,NULL,NULL,346,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6259,36,NULL,NULL,368,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6260,38,NULL,NULL,324,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6261,39,NULL,NULL,256,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6263,37,NULL,NULL,347,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6264,36,NULL,NULL,369,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6266,38,NULL,NULL,325,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6267,39,NULL,NULL,257,0,1,1,'2023-04-20 04:58:05',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6268,37,NULL,NULL,348,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6269,36,NULL,NULL,370,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6270,37,NULL,NULL,349,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6271,39,NULL,NULL,258,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6272,38,NULL,NULL,326,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6274,36,NULL,NULL,371,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6276,39,NULL,NULL,259,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6277,38,NULL,NULL,327,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6278,37,NULL,NULL,350,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6279,36,NULL,NULL,372,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6280,37,NULL,NULL,351,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6281,39,NULL,NULL,260,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6282,38,NULL,NULL,328,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6284,36,NULL,NULL,373,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6286,39,NULL,NULL,261,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6287,38,NULL,NULL,329,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6288,37,NULL,NULL,352,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6289,36,NULL,NULL,374,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6290,37,NULL,NULL,353,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6291,39,NULL,NULL,262,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6292,38,NULL,NULL,330,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6294,36,NULL,NULL,375,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6296,39,NULL,NULL,263,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6297,38,NULL,NULL,331,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6298,37,NULL,NULL,354,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6299,36,NULL,NULL,376,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6300,37,NULL,NULL,355,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6301,39,NULL,NULL,264,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6302,38,NULL,NULL,332,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6304,36,NULL,NULL,377,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6306,38,NULL,NULL,333,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6307,39,NULL,NULL,265,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6308,37,NULL,NULL,356,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6309,36,NULL,NULL,378,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6310,37,NULL,NULL,357,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6311,39,NULL,NULL,266,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6312,38,NULL,NULL,334,0,1,1,'2023-04-20 04:58:06',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6314,36,NULL,NULL,379,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6316,39,NULL,NULL,267,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6317,38,NULL,NULL,335,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6318,37,NULL,NULL,358,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6319,36,NULL,NULL,380,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6320,37,NULL,NULL,359,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6321,39,NULL,NULL,268,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6322,38,NULL,NULL,336,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6324,36,NULL,NULL,381,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6326,39,NULL,NULL,269,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6327,38,NULL,NULL,337,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6328,37,NULL,NULL,360,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6329,36,NULL,NULL,382,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6330,39,NULL,NULL,270,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6331,37,NULL,NULL,361,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6332,38,NULL,NULL,338,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6334,36,NULL,NULL,383,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6336,38,NULL,NULL,339,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6337,37,NULL,NULL,362,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6338,39,NULL,NULL,271,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6339,36,NULL,NULL,384,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6340,39,NULL,NULL,272,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6341,37,NULL,NULL,363,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6342,38,NULL,NULL,340,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6344,36,NULL,NULL,385,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6346,37,NULL,NULL,364,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6347,38,NULL,NULL,341,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6348,39,NULL,NULL,273,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6349,36,NULL,NULL,386,0,1,1,'2023-04-20 04:58:07',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6350,37,NULL,NULL,365,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6351,39,NULL,NULL,274,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6352,38,NULL,NULL,342,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6354,36,NULL,NULL,387,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6356,38,NULL,NULL,343,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6357,39,NULL,NULL,275,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6358,37,NULL,NULL,366,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6359,36,NULL,NULL,388,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6360,37,NULL,NULL,367,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6361,39,NULL,NULL,276,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6362,38,NULL,NULL,344,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6364,36,NULL,NULL,389,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6365,39,NULL,NULL,277,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6367,38,NULL,NULL,345,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6368,37,NULL,NULL,368,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6369,36,NULL,NULL,390,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6370,37,NULL,NULL,369,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6372,38,NULL,NULL,346,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6373,39,NULL,NULL,278,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6374,36,NULL,NULL,391,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6376,39,NULL,NULL,279,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6377,38,NULL,NULL,347,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6378,37,NULL,NULL,370,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6379,36,NULL,NULL,392,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6380,37,NULL,NULL,371,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6381,38,NULL,NULL,348,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6382,39,NULL,NULL,280,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6384,36,NULL,NULL,393,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6386,39,NULL,NULL,281,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6387,38,NULL,NULL,349,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6388,37,NULL,NULL,372,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6389,36,NULL,NULL,394,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6390,37,NULL,NULL,373,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6391,39,NULL,NULL,282,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6392,38,NULL,NULL,350,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6394,36,NULL,NULL,395,0,1,1,'2023-04-20 04:58:08',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6396,38,NULL,NULL,351,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6397,39,NULL,NULL,283,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6398,37,NULL,NULL,374,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6399,36,NULL,NULL,396,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6400,39,NULL,NULL,284,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6401,37,NULL,NULL,375,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6402,38,NULL,NULL,352,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6404,36,NULL,NULL,397,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6406,37,NULL,NULL,376,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6407,38,NULL,NULL,353,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6408,39,NULL,NULL,285,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6409,36,NULL,NULL,398,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6410,39,NULL,NULL,286,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6411,37,NULL,NULL,377,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6412,38,NULL,NULL,354,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6414,36,NULL,NULL,399,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6416,37,NULL,NULL,378,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6417,38,NULL,NULL,355,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6418,39,NULL,NULL,287,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6419,36,NULL,NULL,400,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6420,39,NULL,NULL,288,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6421,37,NULL,NULL,379,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6422,38,NULL,NULL,356,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6424,36,NULL,NULL,401,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6426,37,NULL,NULL,380,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6427,38,NULL,NULL,357,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6428,39,NULL,NULL,289,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6429,36,NULL,NULL,402,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6430,37,NULL,NULL,381,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6431,39,NULL,NULL,290,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6432,38,NULL,NULL,358,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6434,36,NULL,NULL,403,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6436,38,NULL,NULL,359,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6437,39,NULL,NULL,291,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6438,37,NULL,NULL,382,0,1,1,'2023-04-20 04:58:09',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6439,36,NULL,NULL,404,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6440,38,NULL,NULL,360,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6441,37,NULL,NULL,383,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6443,39,NULL,NULL,292,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6444,36,NULL,NULL,405,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6446,37,NULL,NULL,384,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6447,39,NULL,NULL,293,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6448,38,NULL,NULL,361,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6449,36,NULL,NULL,406,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6450,39,NULL,NULL,294,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6451,37,NULL,NULL,385,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6452,38,NULL,NULL,362,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6454,36,NULL,NULL,407,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6455,37,NULL,NULL,386,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6457,38,NULL,NULL,363,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6458,39,NULL,NULL,295,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6459,36,NULL,NULL,408,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6461,39,NULL,NULL,296,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6462,37,NULL,NULL,387,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6463,38,NULL,NULL,364,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6464,36,NULL,NULL,409,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6465,39,NULL,NULL,297,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6466,37,NULL,NULL,388,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6468,38,NULL,NULL,365,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6469,36,NULL,NULL,410,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6471,37,NULL,NULL,389,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6472,38,NULL,NULL,366,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6473,39,NULL,NULL,298,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6474,36,NULL,NULL,411,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6475,39,NULL,NULL,299,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6476,37,NULL,NULL,390,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6477,38,NULL,NULL,367,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6479,36,NULL,NULL,412,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6480,37,NULL,NULL,391,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6482,38,NULL,NULL,368,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6483,39,NULL,NULL,300,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6484,36,NULL,NULL,413,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6485,39,NULL,NULL,301,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6486,38,NULL,NULL,369,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6488,37,NULL,NULL,392,0,1,1,'2023-04-20 04:58:10',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6489,36,NULL,NULL,414,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6491,37,NULL,NULL,393,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6492,38,NULL,NULL,370,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6493,39,NULL,NULL,302,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6494,36,NULL,NULL,415,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6495,37,NULL,NULL,394,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6496,39,NULL,NULL,303,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6497,38,NULL,NULL,371,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6499,36,NULL,NULL,416,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6500,39,NULL,NULL,304,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6502,38,NULL,NULL,372,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6503,37,NULL,NULL,395,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6504,36,NULL,NULL,417,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6506,37,NULL,NULL,396,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6507,38,NULL,NULL,373,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6508,39,NULL,NULL,305,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6509,36,NULL,NULL,418,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6510,37,NULL,NULL,397,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6511,38,NULL,NULL,374,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6513,39,NULL,NULL,306,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6514,36,NULL,NULL,419,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6516,39,NULL,NULL,307,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6517,38,NULL,NULL,375,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6518,37,NULL,NULL,398,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6519,36,NULL,NULL,420,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6520,39,NULL,NULL,308,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6521,37,NULL,NULL,399,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6522,38,NULL,NULL,376,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6524,36,NULL,NULL,421,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6526,37,NULL,NULL,400,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6527,38,NULL,NULL,377,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6528,39,NULL,NULL,309,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6529,36,NULL,NULL,422,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6530,37,NULL,NULL,401,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6531,39,NULL,NULL,310,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6532,38,NULL,NULL,378,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6534,36,NULL,NULL,423,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6535,39,NULL,NULL,311,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6537,38,NULL,NULL,379,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6538,37,NULL,NULL,402,0,1,1,'2023-04-20 04:58:11',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6539,36,NULL,NULL,424,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6540,37,NULL,NULL,403,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6542,38,NULL,NULL,380,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6543,39,NULL,NULL,312,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6544,36,NULL,NULL,425,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6545,39,NULL,NULL,313,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6547,38,NULL,NULL,381,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6548,37,NULL,NULL,404,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6549,36,NULL,NULL,426,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6551,38,NULL,NULL,382,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6552,39,NULL,NULL,314,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6553,37,NULL,NULL,405,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6554,36,NULL,NULL,427,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6555,38,NULL,NULL,383,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6556,39,NULL,NULL,315,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6558,37,NULL,NULL,406,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6559,36,NULL,NULL,428,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6561,39,NULL,NULL,316,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6562,38,NULL,NULL,384,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6563,37,NULL,NULL,407,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6564,36,NULL,NULL,429,0,1,1,'2023-04-20 04:58:12',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6565,39,NULL,NULL,317,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6566,38,NULL,NULL,385,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6568,37,NULL,NULL,408,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6569,36,NULL,NULL,430,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6570,38,NULL,NULL,386,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6572,39,NULL,NULL,318,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6573,36,NULL,NULL,431,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6574,37,NULL,NULL,409,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6576,39,NULL,NULL,319,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6577,38,NULL,NULL,387,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6578,37,NULL,NULL,410,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6579,36,NULL,NULL,432,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6581,39,NULL,NULL,320,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6582,38,NULL,NULL,388,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6583,36,NULL,NULL,433,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6584,37,NULL,NULL,411,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6585,39,NULL,NULL,321,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6587,38,NULL,NULL,389,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6588,37,NULL,NULL,412,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6589,36,NULL,NULL,434,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6590,39,NULL,NULL,322,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6591,38,NULL,NULL,390,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6593,37,NULL,NULL,413,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6594,36,NULL,NULL,435,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6595,38,NULL,NULL,391,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6597,39,NULL,NULL,323,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6598,37,NULL,NULL,414,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6599,36,NULL,NULL,436,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6600,38,NULL,NULL,392,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6601,39,NULL,NULL,324,0,1,1,'2023-04-20 04:58:13',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6603,36,NULL,NULL,437,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6604,37,NULL,NULL,415,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6605,39,NULL,NULL,325,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6607,38,NULL,NULL,393,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6608,36,NULL,NULL,438,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6609,37,NULL,NULL,416,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6611,38,NULL,NULL,394,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6612,39,NULL,NULL,326,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6613,36,NULL,NULL,439,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6614,37,NULL,NULL,417,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6615,38,NULL,NULL,395,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6616,39,NULL,NULL,327,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6618,36,NULL,NULL,440,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6619,37,NULL,NULL,418,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6621,38,NULL,NULL,396,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6622,39,NULL,NULL,328,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6623,36,NULL,NULL,441,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6624,37,NULL,NULL,419,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6625,39,NULL,NULL,329,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6626,38,NULL,NULL,397,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6628,37,NULL,NULL,420,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6629,36,NULL,NULL,442,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6631,38,NULL,NULL,398,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6632,39,NULL,NULL,330,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6633,37,NULL,NULL,421,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6635,38,NULL,NULL,399,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6636,36,NULL,NULL,443,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6637,37,NULL,NULL,422,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6638,39,NULL,NULL,331,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6639,38,NULL,NULL,400,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6640,36,NULL,NULL,444,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6642,37,NULL,NULL,423,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6643,39,NULL,NULL,332,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6644,36,NULL,NULL,445,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6646,38,NULL,NULL,401,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6647,2,NULL,NULL,1,0,1,1,'2023-04-20 04:58:14',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6648,37,NULL,NULL,424,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6649,39,NULL,NULL,333,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6651,38,NULL,NULL,402,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6652,36,NULL,NULL,446,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6653,2,'T01',0.80,2,0,0,1,'2023-04-20 04:58:15','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (6654,39,NULL,NULL,334,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6655,37,NULL,NULL,425,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6656,36,NULL,NULL,447,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6657,38,NULL,NULL,403,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6659,2,'T02',0.85,3,0,0,1,'2023-04-20 04:58:15','2023-04-24 02:14:59',33);
INSERT INTO `t_item_atp_file_detail` VALUES (6660,39,NULL,NULL,335,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6661,37,NULL,NULL,426,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6662,38,NULL,NULL,404,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6663,2,NULL,NULL,4,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6664,36,NULL,NULL,448,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6666,37,NULL,NULL,427,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6667,39,NULL,NULL,336,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6668,2,'T03',0.90,5,0,0,1,'2023-04-20 04:58:15','2023-04-24 02:15:07',33);
INSERT INTO `t_item_atp_file_detail` VALUES (6669,36,NULL,NULL,449,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6670,38,NULL,NULL,405,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6672,37,NULL,NULL,428,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6673,39,NULL,NULL,337,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6674,36,NULL,NULL,450,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6675,2,NULL,NULL,6,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6676,38,NULL,NULL,406,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6678,39,NULL,NULL,338,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6679,37,NULL,NULL,429,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6681,2,NULL,NULL,7,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6682,38,NULL,NULL,407,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6683,36,NULL,NULL,451,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6684,39,NULL,NULL,339,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6685,37,NULL,NULL,430,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6686,36,NULL,NULL,452,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6687,2,'T03',0.90,8,0,0,1,'2023-04-20 04:58:15','2023-04-24 02:15:07',33);
INSERT INTO `t_item_atp_file_detail` VALUES (6688,38,NULL,NULL,408,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6690,39,NULL,NULL,340,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6691,37,NULL,NULL,431,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6693,38,NULL,NULL,409,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6694,36,NULL,NULL,453,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6695,37,NULL,NULL,432,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6696,39,NULL,NULL,341,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6697,36,NULL,NULL,454,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6698,38,NULL,NULL,410,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6700,39,NULL,NULL,342,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6701,37,NULL,NULL,433,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6703,38,NULL,NULL,411,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6704,36,NULL,NULL,455,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6705,37,NULL,NULL,434,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6706,39,NULL,NULL,343,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6707,36,NULL,NULL,456,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6709,38,NULL,NULL,412,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6710,39,NULL,NULL,344,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6711,37,NULL,NULL,435,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6713,38,NULL,NULL,413,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6714,36,NULL,NULL,457,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6715,37,NULL,NULL,436,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6716,39,NULL,NULL,345,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6717,36,NULL,NULL,458,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6718,38,NULL,NULL,414,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6720,39,NULL,NULL,346,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6721,37,NULL,NULL,437,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6723,38,NULL,NULL,415,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6724,36,NULL,NULL,459,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6725,37,NULL,NULL,438,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6726,39,NULL,NULL,347,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6727,36,NULL,NULL,460,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6728,38,NULL,NULL,416,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6730,39,NULL,NULL,348,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6731,37,NULL,NULL,439,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6733,38,NULL,NULL,417,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6734,36,NULL,NULL,461,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6735,37,NULL,NULL,440,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6736,39,NULL,NULL,349,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6737,36,NULL,NULL,462,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6738,38,NULL,NULL,418,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6740,39,NULL,NULL,350,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6741,37,NULL,NULL,441,0,1,1,'2023-04-20 04:58:16',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6743,38,NULL,NULL,419,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6744,36,NULL,NULL,463,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6745,37,NULL,NULL,442,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6746,39,NULL,NULL,351,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6747,36,NULL,NULL,464,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6748,38,NULL,NULL,420,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6750,39,NULL,NULL,352,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6751,37,NULL,NULL,443,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6753,38,NULL,NULL,421,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6754,36,NULL,NULL,465,0,1,1,'2023-04-20 04:58:17',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6755,39,NULL,NULL,353,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6756,37,NULL,NULL,444,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6757,36,NULL,NULL,466,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6758,38,NULL,NULL,422,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6760,37,NULL,NULL,445,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6761,39,NULL,NULL,354,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6763,38,NULL,NULL,423,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6764,36,NULL,NULL,467,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6765,37,NULL,NULL,446,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6766,39,NULL,NULL,355,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6767,36,NULL,NULL,468,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6769,38,NULL,NULL,424,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6770,39,NULL,NULL,356,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6771,37,NULL,NULL,447,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6773,38,NULL,NULL,425,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6774,36,NULL,NULL,469,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6775,37,NULL,NULL,448,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6776,39,NULL,NULL,357,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6777,36,NULL,NULL,470,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6778,38,NULL,NULL,426,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6780,39,NULL,NULL,358,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6781,37,NULL,NULL,449,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6783,38,NULL,NULL,427,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6784,36,NULL,NULL,471,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6785,37,NULL,NULL,450,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6786,39,NULL,NULL,359,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6787,36,NULL,NULL,472,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6788,38,NULL,NULL,428,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6790,37,NULL,NULL,451,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6791,39,NULL,NULL,360,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6793,38,NULL,NULL,429,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6794,36,NULL,NULL,473,0,1,1,'2023-04-20 04:58:18',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6795,39,NULL,NULL,361,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6796,37,NULL,NULL,452,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6797,36,NULL,NULL,474,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6798,38,NULL,NULL,430,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6800,37,NULL,NULL,453,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6801,39,NULL,NULL,362,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6803,38,NULL,NULL,431,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6804,36,NULL,NULL,475,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6805,39,NULL,NULL,363,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6806,37,NULL,NULL,454,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6807,36,NULL,NULL,476,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6808,38,NULL,NULL,432,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6810,39,NULL,NULL,364,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6811,37,NULL,NULL,455,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6813,38,NULL,NULL,433,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6814,36,NULL,NULL,477,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6815,37,NULL,NULL,456,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6816,39,NULL,NULL,365,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6817,36,NULL,NULL,478,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6818,38,NULL,NULL,434,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6820,39,NULL,NULL,366,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6821,37,NULL,NULL,457,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6823,38,NULL,NULL,435,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6824,36,NULL,NULL,479,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6825,37,NULL,NULL,458,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6826,39,NULL,NULL,367,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6827,36,NULL,NULL,480,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6828,38,NULL,NULL,436,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6830,39,NULL,NULL,368,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6831,37,NULL,NULL,459,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6833,38,NULL,NULL,437,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6834,36,NULL,NULL,481,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6835,37,NULL,NULL,460,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6836,39,NULL,NULL,369,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6837,37,NULL,NULL,461,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6838,36,NULL,NULL,482,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6839,38,NULL,NULL,438,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6841,39,NULL,NULL,370,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6842,37,NULL,NULL,462,0,1,1,'2023-04-20 04:58:19',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6843,39,NULL,NULL,371,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6845,38,NULL,NULL,439,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6846,36,NULL,NULL,483,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6847,37,NULL,NULL,463,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6848,36,NULL,NULL,484,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6850,38,NULL,NULL,440,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6851,39,NULL,NULL,372,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6852,37,NULL,NULL,464,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6853,39,NULL,NULL,373,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6855,38,NULL,NULL,441,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6856,36,NULL,NULL,485,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6857,37,NULL,NULL,465,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6858,36,NULL,NULL,486,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6860,38,NULL,NULL,442,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6861,39,NULL,NULL,374,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6862,37,NULL,NULL,466,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6863,39,NULL,NULL,375,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6865,38,NULL,NULL,443,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6866,36,NULL,NULL,487,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6867,37,NULL,NULL,467,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6868,36,NULL,NULL,488,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6870,38,NULL,NULL,444,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6871,39,NULL,NULL,376,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6872,37,NULL,NULL,468,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6873,39,NULL,NULL,377,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6875,38,NULL,NULL,445,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6876,36,NULL,NULL,489,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6877,37,NULL,NULL,469,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6878,36,NULL,NULL,490,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6880,38,NULL,NULL,446,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6881,39,NULL,NULL,378,0,1,1,'2023-04-20 04:58:20',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6882,37,NULL,NULL,470,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6883,39,NULL,NULL,379,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6884,38,NULL,NULL,447,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6886,36,NULL,NULL,491,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6887,37,NULL,NULL,471,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6888,36,NULL,NULL,492,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6890,38,NULL,NULL,448,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6891,39,NULL,NULL,380,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6892,37,NULL,NULL,472,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6893,39,NULL,NULL,381,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6895,38,NULL,NULL,449,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6896,36,NULL,NULL,493,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6897,37,NULL,NULL,473,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6898,36,NULL,NULL,494,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6899,38,NULL,NULL,450,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6901,39,NULL,NULL,382,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6902,37,NULL,NULL,474,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6904,39,NULL,NULL,383,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6905,38,NULL,NULL,451,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6906,36,NULL,NULL,495,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6907,37,NULL,NULL,475,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6908,36,NULL,NULL,496,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6909,39,NULL,NULL,384,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6910,38,NULL,NULL,452,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6912,37,NULL,NULL,476,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6914,39,NULL,NULL,385,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6915,38,NULL,NULL,453,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6916,36,NULL,NULL,497,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6917,37,NULL,NULL,477,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6918,39,NULL,NULL,386,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6919,36,NULL,NULL,498,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6920,38,NULL,NULL,454,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6922,37,NULL,NULL,478,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6924,36,NULL,NULL,499,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6925,38,NULL,NULL,455,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6926,39,NULL,NULL,387,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6927,37,NULL,NULL,479,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6928,39,NULL,NULL,388,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6929,36,NULL,NULL,500,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6930,38,NULL,NULL,456,0,1,1,'2023-04-20 04:58:21',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6932,37,NULL,NULL,480,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6934,36,NULL,NULL,501,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6935,38,NULL,NULL,457,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6936,39,NULL,NULL,389,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6937,37,NULL,NULL,481,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6938,39,NULL,NULL,390,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6939,36,NULL,NULL,502,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6940,38,NULL,NULL,458,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6942,37,NULL,NULL,482,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6944,36,NULL,NULL,503,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6945,38,NULL,NULL,459,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6946,39,NULL,NULL,391,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6947,37,NULL,NULL,483,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6948,39,NULL,NULL,392,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6949,36,NULL,NULL,504,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6950,38,NULL,NULL,460,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6952,37,NULL,NULL,484,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6954,36,NULL,NULL,505,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6955,38,NULL,NULL,461,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6956,39,NULL,NULL,393,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6957,37,NULL,NULL,485,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6958,39,NULL,NULL,394,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6959,36,NULL,NULL,506,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6960,38,NULL,NULL,462,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6962,37,NULL,NULL,486,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6963,36,NULL,NULL,507,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6965,38,NULL,NULL,463,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6966,39,NULL,NULL,395,0,1,1,'2023-04-20 04:58:22',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6967,37,NULL,NULL,487,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6968,39,NULL,NULL,396,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6970,36,NULL,NULL,508,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6971,38,NULL,NULL,464,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6972,37,NULL,NULL,488,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6973,36,NULL,NULL,509,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6975,38,NULL,NULL,465,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6976,39,NULL,NULL,397,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6977,37,NULL,NULL,489,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6978,39,NULL,NULL,398,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6980,38,NULL,NULL,466,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6981,36,NULL,NULL,510,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6982,37,NULL,NULL,490,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6983,36,NULL,NULL,511,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6985,38,NULL,NULL,467,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6986,39,NULL,NULL,399,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6987,37,NULL,NULL,491,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6988,39,NULL,NULL,400,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6990,38,NULL,NULL,468,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6991,36,NULL,NULL,512,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6992,37,NULL,NULL,492,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6994,36,NULL,NULL,513,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6995,38,NULL,NULL,469,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6996,39,NULL,NULL,401,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6997,37,NULL,NULL,493,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6998,39,NULL,NULL,402,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6999,36,NULL,NULL,514,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7000,38,NULL,NULL,470,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7002,37,NULL,NULL,494,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7004,36,NULL,NULL,515,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7005,38,NULL,NULL,471,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7006,39,NULL,NULL,403,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7007,37,NULL,NULL,495,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7008,39,NULL,NULL,404,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7009,36,NULL,NULL,516,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7010,38,NULL,NULL,472,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7012,37,NULL,NULL,496,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7013,36,NULL,NULL,517,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7015,38,NULL,NULL,473,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7016,39,NULL,NULL,405,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7017,37,NULL,NULL,497,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7018,39,NULL,NULL,406,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7020,38,NULL,NULL,474,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7021,36,NULL,NULL,518,0,1,1,'2023-04-20 04:58:23',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7022,37,NULL,NULL,498,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7024,36,NULL,NULL,519,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7025,38,NULL,NULL,475,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7026,39,NULL,NULL,407,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7027,37,NULL,NULL,499,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7028,39,NULL,NULL,408,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7029,36,NULL,NULL,520,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7030,38,NULL,NULL,476,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7032,37,NULL,NULL,500,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7034,36,NULL,NULL,521,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7035,38,NULL,NULL,477,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7036,39,NULL,NULL,409,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7037,37,NULL,NULL,501,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7038,36,NULL,NULL,522,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7039,39,NULL,NULL,410,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7041,38,NULL,NULL,478,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7042,37,NULL,NULL,502,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7044,39,NULL,NULL,411,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7045,38,NULL,NULL,479,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7046,36,NULL,NULL,523,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7047,37,NULL,NULL,503,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7048,38,NULL,NULL,480,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7049,36,NULL,NULL,524,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7051,39,NULL,NULL,412,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7052,37,NULL,NULL,504,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7054,39,NULL,NULL,413,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7055,36,NULL,NULL,525,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7056,38,NULL,NULL,481,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7057,37,NULL,NULL,505,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7058,39,NULL,NULL,414,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7059,36,NULL,NULL,526,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7060,38,NULL,NULL,482,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7062,37,NULL,NULL,506,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7064,36,NULL,NULL,527,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7065,38,NULL,NULL,483,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7066,39,NULL,NULL,415,0,1,1,'2023-04-20 04:58:24',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7067,37,NULL,NULL,507,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7068,39,NULL,NULL,416,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7069,36,NULL,NULL,528,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7070,38,NULL,NULL,484,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7072,37,NULL,NULL,508,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7073,36,NULL,NULL,529,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7075,38,NULL,NULL,485,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7076,39,NULL,NULL,417,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7077,37,NULL,NULL,509,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7078,42,NULL,NULL,1,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7079,39,NULL,NULL,418,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7081,38,NULL,NULL,486,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7082,36,NULL,NULL,530,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7083,37,NULL,NULL,510,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7084,36,NULL,NULL,531,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7085,39,NULL,NULL,419,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7087,38,NULL,NULL,487,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7088,42,NULL,NULL,2,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7089,37,NULL,NULL,511,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7090,42,NULL,NULL,3,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7092,38,NULL,NULL,488,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7093,39,NULL,NULL,420,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7094,36,NULL,NULL,532,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7095,37,NULL,NULL,512,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7096,39,NULL,NULL,421,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7098,38,NULL,NULL,489,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7099,42,NULL,NULL,4,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7100,36,NULL,NULL,533,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7101,37,NULL,NULL,513,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7102,42,NULL,NULL,5,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7103,36,NULL,NULL,534,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7105,38,NULL,NULL,490,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7106,39,NULL,NULL,422,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7107,37,NULL,NULL,514,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7109,39,NULL,NULL,423,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7110,36,NULL,NULL,535,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7111,38,NULL,NULL,491,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7112,42,NULL,NULL,6,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7113,37,NULL,NULL,515,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7114,42,NULL,NULL,7,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7115,36,NULL,NULL,536,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7116,39,NULL,NULL,424,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7117,38,NULL,NULL,492,0,1,1,'2023-04-20 04:58:25',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7119,37,NULL,NULL,516,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7120,42,NULL,NULL,8,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7121,36,NULL,NULL,537,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7123,37,NULL,NULL,517,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7124,38,NULL,NULL,493,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7125,39,NULL,NULL,425,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7126,36,NULL,NULL,538,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7127,37,NULL,NULL,518,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7128,39,NULL,NULL,426,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7129,38,NULL,NULL,494,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7131,36,NULL,NULL,539,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7132,39,NULL,NULL,427,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7134,37,NULL,NULL,519,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7135,38,NULL,NULL,495,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7136,36,NULL,NULL,540,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7137,37,NULL,NULL,520,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7139,38,NULL,NULL,496,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7140,39,NULL,NULL,428,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7141,36,NULL,NULL,541,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7142,39,NULL,NULL,429,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7144,38,NULL,NULL,497,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7145,37,NULL,NULL,521,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7146,36,NULL,NULL,542,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7148,37,NULL,NULL,522,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7149,38,NULL,NULL,498,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7150,39,NULL,NULL,430,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7151,36,NULL,NULL,543,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7152,39,NULL,NULL,431,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7153,37,NULL,NULL,523,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7154,38,NULL,NULL,499,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7156,36,NULL,NULL,544,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7158,37,NULL,NULL,524,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7159,38,NULL,NULL,500,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7160,39,NULL,NULL,432,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7161,36,NULL,NULL,545,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7162,39,NULL,NULL,433,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7163,37,NULL,NULL,525,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7164,38,NULL,NULL,501,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7166,36,NULL,NULL,546,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7168,37,NULL,NULL,526,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7169,38,NULL,NULL,502,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7170,39,NULL,NULL,434,0,1,1,'2023-04-20 04:58:26',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7171,36,NULL,NULL,547,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7172,39,NULL,NULL,435,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7173,37,NULL,NULL,527,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7174,38,NULL,NULL,503,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7176,36,NULL,NULL,548,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7178,37,NULL,NULL,528,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7179,38,NULL,NULL,504,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7180,39,NULL,NULL,436,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7181,36,NULL,NULL,549,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7182,39,NULL,NULL,437,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7183,37,NULL,NULL,529,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7184,38,NULL,NULL,505,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7186,36,NULL,NULL,550,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7188,37,NULL,NULL,530,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7189,39,NULL,NULL,438,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7190,38,NULL,NULL,506,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7191,36,NULL,NULL,551,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7192,39,NULL,NULL,439,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7193,37,NULL,NULL,531,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7194,38,NULL,NULL,507,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7196,36,NULL,NULL,552,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7198,37,NULL,NULL,532,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7199,38,NULL,NULL,508,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7200,39,NULL,NULL,440,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7201,36,NULL,NULL,553,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7202,39,NULL,NULL,441,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7203,37,NULL,NULL,533,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7204,38,NULL,NULL,509,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7206,36,NULL,NULL,554,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7208,37,NULL,NULL,534,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7209,38,NULL,NULL,510,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7210,39,NULL,NULL,442,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7211,36,NULL,NULL,555,0,1,1,'2023-04-20 04:58:27',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7212,39,NULL,NULL,443,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7213,37,NULL,NULL,535,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7214,38,NULL,NULL,511,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7216,36,NULL,NULL,556,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7218,37,NULL,NULL,536,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7219,38,NULL,NULL,512,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7220,39,NULL,NULL,444,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7221,36,NULL,NULL,557,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7222,39,NULL,NULL,445,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7223,37,NULL,NULL,537,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7224,38,NULL,NULL,513,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7226,36,NULL,NULL,558,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7228,37,NULL,NULL,538,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7229,38,NULL,NULL,514,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7230,39,NULL,NULL,446,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7231,36,NULL,NULL,559,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7232,39,NULL,NULL,447,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7233,38,NULL,NULL,515,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7234,37,NULL,NULL,539,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7236,36,NULL,NULL,560,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7238,37,NULL,NULL,540,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7239,38,NULL,NULL,516,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7240,39,NULL,NULL,448,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7241,36,NULL,NULL,561,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7242,39,NULL,NULL,449,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7243,37,NULL,NULL,541,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7244,38,NULL,NULL,517,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7246,36,NULL,NULL,562,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7248,37,NULL,NULL,542,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7249,38,NULL,NULL,518,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7250,39,NULL,NULL,450,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7251,36,NULL,NULL,563,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7252,39,NULL,NULL,451,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7253,38,NULL,NULL,519,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7254,37,NULL,NULL,543,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7256,36,NULL,NULL,564,0,1,1,'2023-04-20 04:58:28',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7257,37,NULL,NULL,544,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7259,38,NULL,NULL,520,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7260,39,NULL,NULL,452,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7261,36,NULL,NULL,565,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7262,39,NULL,NULL,453,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7264,38,NULL,NULL,521,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7265,37,NULL,NULL,545,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7266,36,NULL,NULL,566,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7267,37,NULL,NULL,546,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7269,38,NULL,NULL,522,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7270,39,NULL,NULL,454,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7271,36,NULL,NULL,567,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7272,39,NULL,NULL,455,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7274,38,NULL,NULL,523,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7275,37,NULL,NULL,547,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7276,36,NULL,NULL,568,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7277,37,NULL,NULL,548,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7279,38,NULL,NULL,524,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7280,39,NULL,NULL,456,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7281,36,NULL,NULL,569,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7282,39,NULL,NULL,457,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7284,38,NULL,NULL,525,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7285,37,NULL,NULL,549,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7286,36,NULL,NULL,570,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7287,37,NULL,NULL,550,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7289,38,NULL,NULL,526,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7290,39,NULL,NULL,458,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7291,36,NULL,NULL,571,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7292,39,NULL,NULL,459,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7294,38,NULL,NULL,527,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7295,37,NULL,NULL,551,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7296,36,NULL,NULL,572,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7297,37,NULL,NULL,552,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7299,38,NULL,NULL,528,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7300,39,NULL,NULL,460,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7301,36,NULL,NULL,573,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7302,39,NULL,NULL,461,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7303,38,NULL,NULL,529,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7305,37,NULL,NULL,553,0,1,1,'2023-04-20 04:58:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7306,36,NULL,NULL,574,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7308,37,NULL,NULL,554,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7309,38,NULL,NULL,530,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7310,39,NULL,NULL,462,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7311,36,NULL,NULL,575,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7312,39,NULL,NULL,463,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7313,37,NULL,NULL,555,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7314,38,NULL,NULL,531,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7316,36,NULL,NULL,576,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7318,37,NULL,NULL,556,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7319,38,NULL,NULL,532,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7320,39,NULL,NULL,464,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7321,36,NULL,NULL,577,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7322,39,NULL,NULL,465,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7323,37,NULL,NULL,557,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7324,38,NULL,NULL,533,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7326,36,NULL,NULL,578,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7328,37,NULL,NULL,558,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7329,38,NULL,NULL,534,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7330,39,NULL,NULL,466,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7331,36,NULL,NULL,579,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7332,39,NULL,NULL,467,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7333,37,NULL,NULL,559,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7334,38,NULL,NULL,535,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7336,36,NULL,NULL,580,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7338,37,NULL,NULL,560,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7339,38,NULL,NULL,536,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7340,39,NULL,NULL,468,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7341,36,NULL,NULL,581,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7342,39,NULL,NULL,469,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7343,38,NULL,NULL,537,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7344,37,NULL,NULL,561,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7346,36,NULL,NULL,582,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7347,37,NULL,NULL,562,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7349,38,NULL,NULL,538,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7350,39,NULL,NULL,470,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7351,36,NULL,NULL,583,0,1,1,'2023-04-20 04:58:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7352,39,NULL,NULL,471,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7354,38,NULL,NULL,539,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7355,37,NULL,NULL,563,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7356,36,NULL,NULL,584,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7357,37,NULL,NULL,564,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7359,38,NULL,NULL,540,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7360,39,NULL,NULL,472,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7361,36,NULL,NULL,585,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7362,39,NULL,NULL,473,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7364,38,NULL,NULL,541,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7365,37,NULL,NULL,565,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7366,36,NULL,NULL,586,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7367,37,NULL,NULL,566,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7369,38,NULL,NULL,542,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7370,39,NULL,NULL,474,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7371,36,NULL,NULL,587,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7372,39,NULL,NULL,475,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7374,38,NULL,NULL,543,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7375,37,NULL,NULL,567,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7376,36,NULL,NULL,588,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7377,37,NULL,NULL,568,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7379,38,NULL,NULL,544,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7380,39,NULL,NULL,476,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7381,36,NULL,NULL,589,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7382,39,NULL,NULL,477,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7384,38,NULL,NULL,545,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7385,37,NULL,NULL,569,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7386,36,NULL,NULL,590,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7387,37,NULL,NULL,570,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7389,38,NULL,NULL,546,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7390,39,NULL,NULL,478,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7391,36,NULL,NULL,591,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7392,39,NULL,NULL,479,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7394,38,NULL,NULL,547,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7395,37,NULL,NULL,571,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7396,36,NULL,NULL,592,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7397,37,NULL,NULL,572,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7399,38,NULL,NULL,548,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7400,39,NULL,NULL,480,0,1,1,'2023-04-20 04:58:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7401,36,NULL,NULL,593,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7403,38,NULL,NULL,549,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7404,37,NULL,NULL,573,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7405,36,NULL,NULL,594,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7406,39,NULL,NULL,481,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7407,37,NULL,NULL,574,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7408,38,NULL,NULL,550,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7410,39,NULL,NULL,482,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7411,36,NULL,NULL,595,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7413,38,NULL,NULL,551,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7414,37,NULL,NULL,575,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7415,36,NULL,NULL,596,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7416,39,NULL,NULL,483,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7417,37,NULL,NULL,576,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7418,38,NULL,NULL,552,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7420,39,NULL,NULL,484,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7421,36,NULL,NULL,597,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7423,38,NULL,NULL,553,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7424,37,NULL,NULL,577,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7425,36,NULL,NULL,598,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7426,39,NULL,NULL,485,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7427,37,NULL,NULL,578,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7428,38,NULL,NULL,554,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7430,39,NULL,NULL,486,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7431,36,NULL,NULL,599,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7433,38,NULL,NULL,555,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7434,37,NULL,NULL,579,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7435,36,NULL,NULL,600,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7436,39,NULL,NULL,487,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7437,38,NULL,NULL,556,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7439,37,NULL,NULL,580,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7440,39,NULL,NULL,488,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7441,36,NULL,NULL,601,0,1,1,'2023-04-20 04:58:32',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7443,37,NULL,NULL,581,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7444,38,NULL,NULL,557,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7445,36,NULL,NULL,602,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7446,39,NULL,NULL,489,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7447,37,NULL,NULL,582,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7448,38,NULL,NULL,558,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7450,39,NULL,NULL,490,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7451,36,NULL,NULL,603,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7453,38,NULL,NULL,559,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7454,37,NULL,NULL,583,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7455,36,NULL,NULL,604,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7456,39,NULL,NULL,491,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7457,37,NULL,NULL,584,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7458,38,NULL,NULL,560,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7460,39,NULL,NULL,492,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7461,36,NULL,NULL,605,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7463,38,NULL,NULL,561,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7464,37,NULL,NULL,585,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7465,36,NULL,NULL,606,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7466,39,NULL,NULL,493,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7467,37,NULL,NULL,586,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7468,38,NULL,NULL,562,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7470,39,NULL,NULL,494,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7471,36,NULL,NULL,607,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7473,38,NULL,NULL,563,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7474,37,NULL,NULL,587,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7475,36,NULL,NULL,608,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7476,39,NULL,NULL,495,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7477,37,NULL,NULL,588,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7478,38,NULL,NULL,564,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7480,39,NULL,NULL,496,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7481,36,NULL,NULL,609,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7483,38,NULL,NULL,565,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7484,37,NULL,NULL,589,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7485,36,NULL,NULL,610,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7486,39,NULL,NULL,497,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7487,37,NULL,NULL,590,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7488,38,NULL,NULL,566,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7490,39,NULL,NULL,498,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7491,36,NULL,NULL,611,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7493,38,NULL,NULL,567,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7494,37,NULL,NULL,591,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7495,36,NULL,NULL,612,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7496,39,NULL,NULL,499,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7497,37,NULL,NULL,592,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7498,38,NULL,NULL,568,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7500,39,NULL,NULL,500,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7501,36,NULL,NULL,613,0,1,1,'2023-04-20 04:58:33',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7502,38,NULL,NULL,569,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7503,37,NULL,NULL,593,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7505,36,NULL,NULL,614,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7506,39,NULL,NULL,501,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7508,37,NULL,NULL,594,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7509,38,NULL,NULL,570,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7510,39,NULL,NULL,502,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7511,36,NULL,NULL,615,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7512,37,NULL,NULL,595,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7513,38,NULL,NULL,571,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7515,36,NULL,NULL,616,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7516,39,NULL,NULL,503,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7518,38,NULL,NULL,572,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7519,37,NULL,NULL,596,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7520,39,NULL,NULL,504,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7521,36,NULL,NULL,617,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7522,37,NULL,NULL,597,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7523,38,NULL,NULL,573,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7525,39,NULL,NULL,505,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7526,36,NULL,NULL,618,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7528,38,NULL,NULL,574,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7529,37,NULL,NULL,598,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7530,36,NULL,NULL,619,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7531,39,NULL,NULL,506,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7532,37,NULL,NULL,599,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7533,38,NULL,NULL,575,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7535,39,NULL,NULL,507,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7536,36,NULL,NULL,620,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7538,38,NULL,NULL,576,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7539,37,NULL,NULL,600,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7540,36,NULL,NULL,621,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7541,39,NULL,NULL,508,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7542,37,NULL,NULL,601,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7543,38,NULL,NULL,577,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7545,39,NULL,NULL,509,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7546,36,NULL,NULL,622,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7547,38,NULL,NULL,578,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7549,37,NULL,NULL,602,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7550,36,NULL,NULL,623,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7551,39,NULL,NULL,510,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7552,37,NULL,NULL,603,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7554,38,NULL,NULL,579,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7555,39,NULL,NULL,511,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7556,36,NULL,NULL,624,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7558,38,NULL,NULL,580,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7559,37,NULL,NULL,604,0,1,1,'2023-04-20 04:58:34',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7560,36,NULL,NULL,625,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7561,39,NULL,NULL,512,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7562,37,NULL,NULL,605,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7563,38,NULL,NULL,581,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7565,39,NULL,NULL,513,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7566,36,NULL,NULL,626,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7568,38,NULL,NULL,582,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7569,37,NULL,NULL,606,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7570,36,NULL,NULL,627,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7571,39,NULL,NULL,514,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7572,37,NULL,NULL,607,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7573,38,NULL,NULL,583,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7575,39,NULL,NULL,515,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7576,36,NULL,NULL,628,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7578,38,NULL,NULL,584,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7579,37,NULL,NULL,608,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7580,39,NULL,NULL,516,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7581,36,NULL,NULL,629,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7582,37,NULL,NULL,609,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7583,38,NULL,NULL,585,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7585,36,NULL,NULL,630,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7586,39,NULL,NULL,517,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7588,38,NULL,NULL,586,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7589,37,NULL,NULL,610,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7590,39,NULL,NULL,518,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7591,37,NULL,NULL,611,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7592,38,NULL,NULL,587,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7594,39,NULL,NULL,519,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7596,38,NULL,NULL,588,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7597,37,NULL,NULL,612,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7598,39,NULL,NULL,520,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7599,37,NULL,NULL,613,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7600,38,NULL,NULL,589,0,1,1,'2023-04-20 04:58:35',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7602,39,NULL,NULL,521,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7604,38,NULL,NULL,590,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7605,37,NULL,NULL,614,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7606,39,NULL,NULL,522,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7607,37,NULL,NULL,615,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7608,38,NULL,NULL,591,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7610,39,NULL,NULL,523,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7612,38,NULL,NULL,592,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7613,37,NULL,NULL,616,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7614,39,NULL,NULL,524,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7615,37,NULL,NULL,617,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7616,38,NULL,NULL,593,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7618,39,NULL,NULL,525,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7620,38,NULL,NULL,594,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7621,37,NULL,NULL,618,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7622,39,NULL,NULL,526,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7623,37,NULL,NULL,619,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7624,38,NULL,NULL,595,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7626,39,NULL,NULL,527,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7628,38,NULL,NULL,596,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7629,37,NULL,NULL,620,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7630,39,NULL,NULL,528,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7631,37,NULL,NULL,621,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7632,38,NULL,NULL,597,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7634,39,NULL,NULL,529,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7636,38,NULL,NULL,598,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7637,37,NULL,NULL,622,0,1,1,'2023-04-20 04:58:36',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7638,39,NULL,NULL,530,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7639,38,NULL,NULL,599,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7640,37,NULL,NULL,623,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7642,39,NULL,NULL,531,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7644,37,NULL,NULL,624,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7645,38,NULL,NULL,600,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7646,39,NULL,NULL,532,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7647,37,NULL,NULL,625,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7648,38,NULL,NULL,601,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7650,39,NULL,NULL,533,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7652,38,NULL,NULL,602,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7653,37,NULL,NULL,626,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7654,39,NULL,NULL,534,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7655,37,NULL,NULL,627,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7656,38,NULL,NULL,603,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7658,39,NULL,NULL,535,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7660,38,NULL,NULL,604,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7661,37,NULL,NULL,628,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7662,39,NULL,NULL,536,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7663,37,NULL,NULL,629,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7664,38,NULL,NULL,605,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7666,39,NULL,NULL,537,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7668,38,NULL,NULL,606,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7669,37,NULL,NULL,630,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7670,39,NULL,NULL,538,0,1,1,'2023-04-20 04:58:37',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7671,38,NULL,NULL,607,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7673,39,NULL,NULL,539,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7674,38,NULL,NULL,608,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7676,39,NULL,NULL,540,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7678,38,NULL,NULL,609,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7679,39,NULL,NULL,541,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7681,38,NULL,NULL,610,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7682,39,NULL,NULL,542,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7683,38,NULL,NULL,611,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7685,39,NULL,NULL,543,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7686,38,NULL,NULL,612,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7687,39,NULL,NULL,544,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7689,38,NULL,NULL,613,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7690,39,NULL,NULL,545,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7692,38,NULL,NULL,614,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7694,39,NULL,NULL,546,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7695,38,NULL,NULL,615,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7696,39,NULL,NULL,547,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7698,38,NULL,NULL,616,0,1,1,'2023-04-20 04:58:38',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7699,39,NULL,NULL,548,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7701,38,NULL,NULL,617,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7703,39,NULL,NULL,549,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7704,38,NULL,NULL,618,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7705,39,NULL,NULL,550,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7707,38,NULL,NULL,619,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7709,39,NULL,NULL,551,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7710,38,NULL,NULL,620,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7712,39,NULL,NULL,552,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7713,38,NULL,NULL,621,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7714,39,NULL,NULL,553,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7716,38,NULL,NULL,622,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7717,39,NULL,NULL,554,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7719,38,NULL,NULL,623,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7721,39,NULL,NULL,555,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7722,38,NULL,NULL,624,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7723,39,NULL,NULL,556,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7725,38,NULL,NULL,625,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7726,39,NULL,NULL,557,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7728,38,NULL,NULL,626,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7730,39,NULL,NULL,558,0,1,1,'2023-04-20 04:58:39',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7731,38,NULL,NULL,627,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7732,39,NULL,NULL,559,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7734,38,NULL,NULL,628,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7736,39,NULL,NULL,560,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7737,38,NULL,NULL,629,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7738,39,NULL,NULL,561,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7740,38,NULL,NULL,630,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7741,39,NULL,NULL,562,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7744,39,NULL,NULL,563,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7746,39,NULL,NULL,564,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7748,39,NULL,NULL,565,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7750,39,NULL,NULL,566,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7752,39,NULL,NULL,567,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7754,39,NULL,NULL,568,0,1,1,'2023-04-20 04:58:40',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7756,39,NULL,NULL,569,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7758,39,NULL,NULL,570,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7760,39,NULL,NULL,571,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7762,39,NULL,NULL,572,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7764,39,NULL,NULL,573,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7766,39,NULL,NULL,574,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7768,39,NULL,NULL,575,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7770,39,NULL,NULL,576,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7772,39,NULL,NULL,577,0,1,1,'2023-04-20 04:58:41',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7774,39,NULL,NULL,578,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7776,39,NULL,NULL,579,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7778,39,NULL,NULL,580,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7780,39,NULL,NULL,581,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7782,39,NULL,NULL,582,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7784,39,NULL,NULL,583,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7786,39,NULL,NULL,584,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7788,39,NULL,NULL,585,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7790,39,NULL,NULL,586,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7792,39,NULL,NULL,587,0,1,1,'2023-04-20 04:58:42',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7794,39,NULL,NULL,588,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7796,39,NULL,NULL,589,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7798,39,NULL,NULL,590,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7800,39,NULL,NULL,591,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7802,39,NULL,NULL,592,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7804,39,NULL,NULL,593,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7806,39,NULL,NULL,594,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7808,39,NULL,NULL,595,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7810,39,NULL,NULL,596,0,1,1,'2023-04-20 04:58:43',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7812,39,NULL,NULL,597,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7814,39,NULL,NULL,598,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7816,39,NULL,NULL,599,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7818,39,NULL,NULL,600,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7820,39,NULL,NULL,601,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7822,39,NULL,NULL,602,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7824,39,NULL,NULL,603,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7826,39,NULL,NULL,604,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7828,39,NULL,NULL,605,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7830,39,NULL,NULL,606,0,1,1,'2023-04-20 04:58:44',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7832,39,NULL,NULL,607,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7834,39,NULL,NULL,608,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7836,39,NULL,NULL,609,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7838,39,NULL,NULL,610,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7840,39,NULL,NULL,611,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7842,39,NULL,NULL,612,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7844,39,NULL,NULL,613,0,1,1,'2023-04-20 04:58:45',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7846,39,NULL,NULL,614,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7848,39,NULL,NULL,615,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7850,39,NULL,NULL,616,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7852,39,NULL,NULL,617,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7854,39,NULL,NULL,618,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7856,39,NULL,NULL,619,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7858,39,NULL,NULL,620,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7860,39,NULL,NULL,621,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7862,39,NULL,NULL,622,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7864,39,NULL,NULL,623,0,1,1,'2023-04-20 04:58:46',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7866,39,NULL,NULL,624,0,1,1,'2023-04-20 04:58:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7868,39,NULL,NULL,625,0,1,1,'2023-04-20 04:58:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7870,39,NULL,NULL,626,0,1,1,'2023-04-20 04:58:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7872,39,NULL,NULL,627,0,1,1,'2023-04-20 04:58:47',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7874,39,NULL,NULL,628,0,1,1,'2023-04-20 04:58:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7876,39,NULL,NULL,629,0,1,1,'2023-04-20 04:58:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (7878,39,NULL,NULL,630,0,1,1,'2023-04-20 04:58:48',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8885,40,NULL,NULL,1,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8886,40,'T01',0.80,2,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8887,40,'T03',0.90,3,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:45',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8888,40,'T03',0.90,4,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:08',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8889,40,'T01',0.80,5,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8890,40,'T01',0.80,6,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8891,40,NULL,NULL,7,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8892,40,'T01',0.80,8,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8893,40,'T01',0.80,9,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8894,40,NULL,NULL,10,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8895,40,NULL,NULL,11,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8896,40,'T01',0.80,12,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8897,40,'T03',0.90,13,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:33',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8898,40,'T03',0.90,14,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:08',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8899,40,'T01',0.80,15,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8900,40,'T01',0.80,16,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:51',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8901,40,NULL,NULL,17,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8902,40,'T01',0.80,18,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8903,40,'T01',0.80,19,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8904,40,NULL,NULL,20,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8905,40,NULL,NULL,21,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8906,40,'T01',0.80,22,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8907,40,'T03',0.90,23,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:08',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8908,40,'T03',0.90,24,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:08',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8909,40,'T01',0.80,25,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8910,40,'T01',0.80,26,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:51',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8911,40,NULL,NULL,27,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8912,40,'T01',0.80,28,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8913,40,'T01',0.80,29,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8914,40,NULL,NULL,30,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8915,40,NULL,NULL,31,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8916,40,'T01',0.80,32,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8917,40,'T03',0.90,33,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:33',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8918,40,'T03',0.90,34,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:34:33',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8919,40,'T01',0.80,35,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8920,40,'T01',0.80,36,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:51',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8921,40,NULL,NULL,37,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8922,40,'T01',0.80,38,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8923,40,'T01',0.80,39,0,0,1,'2023-04-23 16:43:04','2023-04-24 02:32:50',33);
INSERT INTO `t_item_atp_file_detail` VALUES (8924,40,NULL,NULL,40,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8925,40,NULL,NULL,41,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8926,40,NULL,NULL,42,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8927,40,NULL,NULL,43,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8928,40,NULL,NULL,44,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8929,40,NULL,NULL,45,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8930,40,NULL,NULL,46,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8931,40,NULL,NULL,47,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8932,40,NULL,NULL,48,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8933,40,NULL,NULL,49,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8934,40,NULL,NULL,50,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8935,40,NULL,NULL,51,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8936,40,NULL,NULL,52,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8937,40,NULL,NULL,53,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8938,40,NULL,NULL,54,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8939,40,NULL,NULL,55,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8940,40,NULL,NULL,56,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8941,40,NULL,NULL,57,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8942,40,NULL,NULL,58,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8943,40,NULL,NULL,59,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8944,40,NULL,NULL,60,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8945,40,NULL,NULL,61,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8946,40,NULL,NULL,62,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8947,40,NULL,NULL,63,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8948,40,NULL,NULL,64,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8949,40,NULL,NULL,65,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8950,40,NULL,NULL,66,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8951,40,NULL,NULL,67,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8952,40,NULL,NULL,68,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8953,40,NULL,NULL,69,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8954,40,NULL,NULL,70,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8955,40,NULL,NULL,71,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8956,40,NULL,NULL,72,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8957,40,NULL,NULL,73,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8958,40,NULL,NULL,74,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8959,40,NULL,NULL,75,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8960,40,NULL,NULL,76,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8961,40,NULL,NULL,77,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8962,40,NULL,NULL,78,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8963,40,NULL,NULL,79,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8964,40,NULL,NULL,80,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8965,40,NULL,NULL,81,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8966,40,NULL,NULL,82,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8967,40,NULL,NULL,83,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8968,40,NULL,NULL,84,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8969,40,NULL,NULL,85,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8970,40,NULL,NULL,86,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8971,40,NULL,NULL,87,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8972,40,NULL,NULL,88,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8973,40,NULL,NULL,89,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8974,40,NULL,NULL,90,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8975,40,NULL,NULL,91,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8976,40,NULL,NULL,92,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8977,40,NULL,NULL,93,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8978,40,NULL,NULL,94,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8979,40,NULL,NULL,95,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8980,40,NULL,NULL,96,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8981,40,NULL,NULL,97,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8982,40,NULL,NULL,98,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8983,40,NULL,NULL,99,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8984,40,NULL,NULL,100,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8985,40,NULL,NULL,101,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8986,40,NULL,NULL,102,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8987,40,NULL,NULL,103,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8988,40,NULL,NULL,104,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8989,40,NULL,NULL,105,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8990,40,NULL,NULL,106,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8991,40,NULL,NULL,107,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8992,40,NULL,NULL,108,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8993,40,NULL,NULL,109,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8994,40,NULL,NULL,110,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8995,40,NULL,NULL,111,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8996,40,NULL,NULL,112,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8997,40,NULL,NULL,113,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8998,40,NULL,NULL,114,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (8999,40,NULL,NULL,115,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9000,40,NULL,NULL,116,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9001,40,NULL,NULL,117,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9002,40,NULL,NULL,118,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9003,40,NULL,NULL,119,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9004,40,NULL,NULL,120,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9005,40,NULL,NULL,121,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9006,40,NULL,NULL,122,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9007,40,NULL,NULL,123,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9008,40,NULL,NULL,124,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9009,40,NULL,NULL,125,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9010,40,NULL,NULL,126,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9011,40,NULL,NULL,127,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9012,40,NULL,NULL,128,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9013,40,NULL,NULL,129,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9014,40,NULL,NULL,130,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9015,40,NULL,NULL,131,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9016,40,NULL,NULL,132,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9017,40,NULL,NULL,133,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9018,40,NULL,NULL,134,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9019,40,NULL,NULL,135,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9020,40,NULL,NULL,136,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9021,40,NULL,NULL,137,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9022,40,NULL,NULL,138,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9023,40,NULL,NULL,139,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9024,40,NULL,NULL,140,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9025,40,NULL,NULL,141,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9026,40,NULL,NULL,142,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9027,40,NULL,NULL,143,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9028,40,NULL,NULL,144,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9029,40,NULL,NULL,145,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9030,40,NULL,NULL,146,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9031,40,NULL,NULL,147,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9032,40,NULL,NULL,148,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9033,40,NULL,NULL,149,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9034,40,NULL,NULL,150,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9035,40,NULL,NULL,151,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9036,40,NULL,NULL,152,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9037,40,NULL,NULL,153,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9038,40,NULL,NULL,154,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9039,40,NULL,NULL,155,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9040,40,NULL,NULL,156,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9041,40,NULL,NULL,157,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9042,40,NULL,NULL,158,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9043,40,NULL,NULL,159,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9044,40,NULL,NULL,160,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9045,40,NULL,NULL,161,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9046,40,NULL,NULL,162,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9047,40,NULL,NULL,163,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9048,40,NULL,NULL,164,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9049,40,NULL,NULL,165,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9050,40,NULL,NULL,166,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9051,40,NULL,NULL,167,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9052,40,NULL,NULL,168,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9053,40,NULL,NULL,169,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9054,40,NULL,NULL,170,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9055,40,NULL,NULL,171,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9056,40,NULL,NULL,172,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9057,40,NULL,NULL,173,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9058,40,NULL,NULL,174,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9059,40,NULL,NULL,175,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9060,40,NULL,NULL,176,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9061,40,NULL,NULL,177,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9062,40,NULL,NULL,178,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9063,40,NULL,NULL,179,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9064,40,NULL,NULL,180,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9065,40,NULL,NULL,181,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9066,40,NULL,NULL,182,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9067,40,NULL,NULL,183,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9068,40,NULL,NULL,184,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9069,40,NULL,NULL,185,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9070,40,NULL,NULL,186,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9071,40,NULL,NULL,187,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9072,40,NULL,NULL,188,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9073,40,NULL,NULL,189,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9074,40,NULL,NULL,190,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9075,40,NULL,NULL,191,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9076,40,NULL,NULL,192,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9077,40,NULL,NULL,193,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9078,40,NULL,NULL,194,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9079,40,NULL,NULL,195,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9080,40,NULL,NULL,196,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9081,40,NULL,NULL,197,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9082,40,NULL,NULL,198,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9083,40,NULL,NULL,199,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9084,40,NULL,NULL,200,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9085,40,NULL,NULL,201,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9086,40,NULL,NULL,202,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9087,40,NULL,NULL,203,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9088,40,NULL,NULL,204,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9089,40,NULL,NULL,205,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9090,40,NULL,NULL,206,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9091,40,NULL,NULL,207,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9092,40,NULL,NULL,208,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9093,40,NULL,NULL,209,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9094,40,NULL,NULL,210,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9095,40,NULL,NULL,211,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9096,40,NULL,NULL,212,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9097,40,NULL,NULL,213,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9098,40,NULL,NULL,214,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9099,40,NULL,NULL,215,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9100,40,NULL,NULL,216,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9101,40,NULL,NULL,217,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9102,40,NULL,NULL,218,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9103,40,NULL,NULL,219,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9104,40,NULL,NULL,220,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9105,40,NULL,NULL,221,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9106,40,NULL,NULL,222,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9107,40,NULL,NULL,223,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9108,40,NULL,NULL,224,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9109,40,NULL,NULL,225,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9110,40,NULL,NULL,226,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9111,40,NULL,NULL,227,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9112,40,NULL,NULL,228,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9113,40,NULL,NULL,229,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9114,40,NULL,NULL,230,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9115,40,NULL,NULL,231,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9116,40,NULL,NULL,232,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9117,40,NULL,NULL,233,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9118,40,NULL,NULL,234,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9119,40,NULL,NULL,235,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9120,40,NULL,NULL,236,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9121,40,NULL,NULL,237,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9122,40,NULL,NULL,238,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9123,40,NULL,NULL,239,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9124,40,NULL,NULL,240,0,1,1,'2023-04-23 16:43:04',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9125,40,NULL,NULL,241,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9126,40,NULL,NULL,242,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9127,40,NULL,NULL,243,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9128,40,NULL,NULL,244,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9129,40,NULL,NULL,245,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9130,40,NULL,NULL,246,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9131,40,NULL,NULL,247,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9132,40,NULL,NULL,248,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9133,40,NULL,NULL,249,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9134,40,NULL,NULL,250,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9135,40,NULL,NULL,251,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9136,40,NULL,NULL,252,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9137,40,NULL,NULL,253,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9138,40,NULL,NULL,254,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9139,40,NULL,NULL,255,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9140,40,NULL,NULL,256,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9141,40,NULL,NULL,257,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9142,40,NULL,NULL,258,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9143,40,NULL,NULL,259,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9144,40,NULL,NULL,260,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9145,40,NULL,NULL,261,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9146,40,NULL,NULL,262,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9147,40,NULL,NULL,263,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9148,40,NULL,NULL,264,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9149,40,NULL,NULL,265,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9150,40,NULL,NULL,266,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9151,40,NULL,NULL,267,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9152,40,NULL,NULL,268,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9153,40,NULL,NULL,269,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9154,40,NULL,NULL,270,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9155,40,NULL,NULL,271,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9156,40,NULL,NULL,272,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9157,40,NULL,NULL,273,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9158,40,NULL,NULL,274,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9159,40,NULL,NULL,275,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9160,40,NULL,NULL,276,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9161,40,NULL,NULL,277,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9162,40,NULL,NULL,278,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9163,40,NULL,NULL,279,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9164,40,NULL,NULL,280,0,1,1,'2023-04-23 16:43:30',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9165,43,'T01',0.80,1,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9166,43,'T01',0.80,2,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9167,43,'T01',0.80,3,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9168,43,'T01',0.80,4,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9169,43,'T01',0.80,5,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9170,43,'',0.00,6,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9171,43,'T01',0.80,7,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9172,43,'',0.00,8,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9173,43,'T01',0.80,9,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9174,43,'T01',0.80,10,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9175,43,'T01',0.80,11,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9176,43,'T01',0.80,12,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9177,43,'T01',0.80,13,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9178,43,'T01',0.80,14,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9179,43,'T01',0.80,15,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9180,43,'',0.00,16,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9181,43,'T01',0.80,17,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9182,43,'',0.00,18,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9183,43,'T01',0.80,19,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9184,43,'T01',0.80,20,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9185,43,'T01',0.80,21,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9186,43,'T01',0.80,22,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9187,43,'T01',0.80,23,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9188,43,'T01',0.80,24,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9189,43,'T01',0.80,25,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9190,43,'',0.00,26,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9191,43,'T01',0.80,27,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9192,43,'',0.00,28,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9193,43,'T01',0.80,29,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9194,43,'T01',0.80,30,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9195,43,'T01',0.80,31,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9196,43,'T01',0.80,32,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9197,43,'T01',0.80,33,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9198,43,'T01',0.80,34,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9199,43,'T01',0.80,35,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9200,43,'',0.00,36,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9201,43,'T01',0.80,37,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9202,43,'T01',0.80,38,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9203,43,'T01',0.80,39,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9204,43,'T01',0.80,40,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:14:54',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9205,43,'',0.00,41,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9206,43,'',0.00,42,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9207,43,'',0.00,43,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9208,43,'',0.00,44,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9209,43,'',0.00,45,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9210,43,'',0.00,46,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9211,43,'',0.00,47,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9212,43,'',0.00,48,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9213,43,'',0.00,49,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9214,43,'',0.00,50,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9215,43,'',0.00,51,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9216,43,'',0.00,52,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9217,43,'',0.00,53,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9218,43,'',0.00,54,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9219,43,'',0.00,55,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9220,43,'',0.00,56,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9221,43,'',0.00,57,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9222,43,'',0.00,58,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9223,43,'',0.00,59,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9224,43,'',0.00,60,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9225,43,'',0.00,61,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9226,43,'',0.00,62,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9227,43,'',0.00,63,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9228,43,'',0.00,64,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9229,43,'',0.00,65,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9230,43,'',0.00,66,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9231,43,'',0.00,67,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9232,43,'',0.00,68,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9233,43,'',0.00,69,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9234,43,'',0.00,70,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9235,43,'',0.00,71,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9236,43,'',0.00,72,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9237,43,'',0.00,73,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9238,43,'',0.00,74,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9239,43,'',0.00,75,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9240,43,'',0.00,76,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9241,43,'',0.00,77,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9242,43,'',0.00,78,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9243,43,'',0.00,79,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9244,43,'',0.00,80,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9245,43,'',0.00,81,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9246,43,'',0.00,82,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9247,43,'',0.00,83,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9248,43,'',0.00,84,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9249,43,'',0.00,85,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9250,43,'',0.00,86,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9251,43,'',0.00,87,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9252,43,'',0.00,88,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9253,43,'',0.00,89,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9254,43,'',0.00,90,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9255,43,'',0.00,91,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9256,43,'',0.00,92,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9257,43,'',0.00,93,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9258,43,'',0.00,94,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9259,43,'',0.00,95,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9260,43,'',0.00,96,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9261,43,'',0.00,97,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9262,43,'',0.00,98,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9263,43,'',0.00,99,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9264,43,'',0.00,100,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9265,43,'',0.00,101,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9266,43,'',0.00,102,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9267,43,'',0.00,103,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9268,43,'',0.00,104,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9269,43,'',0.00,105,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9270,43,'',0.00,106,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9271,43,'',0.00,107,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9272,43,'',0.00,108,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9273,43,'',0.00,109,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9274,43,'',0.00,110,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9275,43,'',0.00,111,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9276,43,'',0.00,112,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9277,43,'',0.00,113,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9278,43,'',0.00,114,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9279,43,'',0.00,115,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9280,43,'',0.00,116,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9281,43,'',0.00,117,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9282,43,'',0.00,118,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9283,43,'',0.00,119,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9284,43,'',0.00,120,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9285,43,'',0.00,121,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9286,43,'',0.00,122,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9287,43,'',0.00,123,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9288,43,'',0.00,124,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9289,43,'',0.00,125,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9290,43,'',0.00,126,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9291,43,'',0.00,127,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9292,43,'',0.00,128,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9293,43,'',0.00,129,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9294,43,'',0.00,130,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9295,43,'',0.00,131,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9296,43,'',0.00,132,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9297,43,'',0.00,133,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9298,43,'',0.00,134,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9299,43,'',0.00,135,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9300,43,'',0.00,136,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9301,43,'',0.00,137,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9302,43,'',0.00,138,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9303,43,'',0.00,139,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9304,43,'',0.00,140,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9305,43,'',0.00,141,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9306,43,'',0.00,142,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9307,43,'',0.00,143,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9308,43,'',0.00,144,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9309,43,'',0.00,145,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9310,43,'',0.00,146,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9311,43,'',0.00,147,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9312,43,'',0.00,148,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9313,43,'',0.00,149,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9314,43,'',0.00,150,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9315,43,'',0.00,151,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9316,43,'',0.00,152,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9317,43,'',0.00,153,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9318,43,'',0.00,154,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9319,43,'',0.00,155,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9320,43,'',0.00,156,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9321,43,'',0.00,157,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9322,43,'',0.00,158,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9323,43,'',0.00,159,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9324,43,'',0.00,160,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9325,43,'',0.00,161,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9326,43,'',0.00,162,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9327,43,'',0.00,163,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9328,43,'',0.00,164,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9329,43,'',0.00,165,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9330,43,'',0.00,166,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9331,43,'',0.00,167,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9332,43,'',0.00,168,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9333,43,'',0.00,169,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9334,43,'',0.00,170,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9335,43,'',0.00,171,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9336,43,'',0.00,172,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9337,43,'',0.00,173,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9338,43,'',0.00,174,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9339,43,'',0.00,175,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9340,43,'',0.00,176,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9341,43,'',0.00,177,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9342,43,'',0.00,178,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9343,43,'',0.00,179,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9344,43,'',0.00,180,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9345,43,'',0.00,181,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9346,43,'',0.00,182,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9347,43,'',0.00,183,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9348,43,'',0.00,184,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9349,43,'',0.00,185,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9350,43,'',0.00,186,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9351,43,'',0.00,187,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9352,43,'',0.00,188,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9353,43,'',0.00,189,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9354,43,'',0.00,190,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9355,43,'',0.00,191,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9356,43,'',0.00,192,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9357,43,'',0.00,193,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9358,43,'',0.00,194,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9359,43,'',0.00,195,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9360,43,'',0.00,196,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9361,43,'',0.00,197,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9362,43,'',0.00,198,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9363,43,'',0.00,199,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9364,43,'',0.00,200,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9365,43,'',0.00,201,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9366,43,'',0.00,202,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9367,43,'',0.00,203,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9368,43,'',0.00,204,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9369,43,'',0.00,205,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9370,43,'',0.00,206,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9371,43,'',0.00,207,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9372,43,'',0.00,208,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9373,43,'',0.00,209,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9374,43,'',0.00,210,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9375,43,'',0.00,211,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9376,43,'',0.00,212,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9377,43,'',0.00,213,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9378,43,'',0.00,214,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9379,43,'',0.00,215,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9380,43,'',0.00,216,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9381,43,'',0.00,217,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9382,43,'',0.00,218,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9383,43,'',0.00,219,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9384,43,'',0.00,220,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9385,43,'',0.00,221,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9386,43,'',0.00,222,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9387,43,'',0.00,223,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9388,43,'',0.00,224,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9389,43,'',0.00,225,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9390,43,'',0.00,226,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9391,43,'',0.00,227,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9392,43,'',0.00,228,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9393,43,'',0.00,229,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9394,43,'',0.00,230,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9395,43,'',0.00,231,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9396,43,'',0.00,232,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9397,43,'',0.00,233,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9398,43,'',0.00,234,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9399,43,'',0.00,235,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9400,43,'',0.00,236,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9401,43,'',0.00,237,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9402,43,'',0.00,238,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9403,43,'',0.00,239,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9404,43,'',0.00,240,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9405,43,'',0.00,241,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9406,43,'',0.00,242,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9407,43,'',0.00,243,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9408,43,'',0.00,244,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9409,43,'',0.00,245,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9410,43,'',0.00,246,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9411,43,'',0.00,247,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9412,43,'',0.00,248,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9413,43,'',0.00,249,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9414,43,'',0.00,250,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9415,43,'',0.00,251,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9416,43,'',0.00,252,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9417,43,'',0.00,253,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9418,43,'',0.00,254,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9419,43,'',0.00,255,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9420,43,'',0.00,256,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9421,43,'',0.00,257,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9422,43,'',0.00,258,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9423,43,'',0.00,259,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9424,43,'',0.00,260,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9425,43,'',0.00,261,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9426,43,'',0.00,262,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9427,43,'',0.00,263,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9428,43,'',0.00,264,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9429,43,'',0.00,265,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9430,43,'',0.00,266,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9431,43,'',0.00,267,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9432,43,'',0.00,268,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9433,43,'',0.00,269,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9434,43,'',0.00,270,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9435,43,'',0.00,271,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9436,43,'',0.00,272,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9437,43,'',0.00,273,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9438,43,'',0.00,274,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9439,43,'',0.00,275,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9440,43,'',0.00,276,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9441,43,'',0.00,277,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9442,43,'',0.00,278,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9443,43,'',0.00,279,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
INSERT INTO `t_item_atp_file_detail` VALUES (9444,43,'',0.00,280,0,0,1,'2023-04-23 16:44:14','2023-04-24 02:12:16',33);
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file`
--

LOCK TABLES `t_item_drill_file` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file` DISABLE KEYS */;
INSERT INTO `t_item_drill_file` VALUES (16,'IF2023040500002','PCB单层板',36,22,NULL,'\\\\fileServer\\IF200001111\\IF200001111.drl',0,1,1,'2023-04-19 01:03:56',NULL,NULL);
INSERT INTO `t_item_drill_file` VALUES (18,'IF2023040500002','PCB单层板',36,22,NULL,'11',0,1,1,'2023-04-21 01:34:09',NULL,NULL);
INSERT INTO `t_item_drill_file` VALUES (19,'1','1',57,2,NULL,'1',0,1,1,'2023-04-21 01:35:10',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数文件明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file_detail`
--

LOCK TABLES `t_item_drill_file_detail` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file_detail` DISABLE KEYS */;
INSERT INTO `t_item_drill_file_detail` VALUES (3,2,'T01',0.80,0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_item_drill_file_detail` VALUES (4,2,'T02',0.85,0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_item_drill_file_detail` VALUES (5,2,'T03',0.90,0,1,NULL,NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_type`
--

LOCK TABLES `t_item_type` WRITE;
/*!40000 ALTER TABLE `t_item_type` DISABLE KEYS */;
INSERT INTO `t_item_type` VALUES (1,'物料产品分类',0,'00',1,'Y','0',0,1,1,'2023-03-30 05:08:07','2023-04-03 02:31:48',1);
INSERT INTO `t_item_type` VALUES (2,'原料',1,'01',1,'Y','0,1',0,1,1,'2023-03-30 01:44:39','2023-04-04 02:11:29',1);
INSERT INTO `t_item_type` VALUES (4,'半成品',1,'02',1,'Y','0,1',0,1,1,'2023-03-30 05:10:36','2023-04-04 02:11:22',1);
INSERT INTO `t_item_type` VALUES (22,'成品',1,'03',2,'Y','0,1',0,1,1,'2023-04-05 01:43:14','2023-04-05 01:44:38',1);
INSERT INTO `t_item_type` VALUES (25,'铜',2,'tong',1,NULL,'0,1,2,25',0,1,1,'2023-04-12 01:02:53','2023-04-22 22:23:22',1);
INSERT INTO `t_item_type` VALUES (26,'铝',2,'lv',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:10:20','2023-04-12 01:27:20',1);
INSERT INTO `t_item_type` VALUES (27,'板材',2,'panel',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:27:59',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COMMENT='库存记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock`
--

LOCK TABLES `t_material_stock` WRITE;
/*!40000 ALTER TABLE `t_material_stock` DISABLE KEYS */;
INSERT INTO `t_material_stock` VALUES (11,'123','1',1,1,'in',22,36,'IF2023040500002','Panel单层02','11','Panel',1.00,NULL,'22',2,'WareHouse2','WareHouse2',NULL,25,'MO202304030001',NULL,NULL,NULL,NULL,0,1,1,'2023-04-21 01:57:25','2023-04-22 21:25:18',1);
INSERT INTO `t_material_stock` VALUES (12,'1','1',1,1,'out',22,36,'IF2023040500002','PCB单层板','DDD XXXX DD','Panel',1.00,NULL,'22',14,'WareHouse','WareHouse',NULL,21,'MO202304050003',NULL,NULL,11,NULL,0,1,1,'2023-04-22 21:19:29',NULL,NULL);
INSERT INTO `t_material_stock` VALUES (13,'111','11',11,11,'out',22,36,'IF2023040500002','Panel单层02','11','Panel',1.00,NULL,'22',14,'WareHouse','WareHouse',NULL,25,'MO202304030001',NULL,NULL,11,'123',0,1,1,'2023-04-22 21:25:18',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COMMENT='出入库明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock_detail`
--

LOCK TABLES `t_material_stock_detail` WRITE;
/*!40000 ALTER TABLE `t_material_stock_detail` DISABLE KEYS */;
INSERT INTO `t_material_stock_detail` VALUES (1,0,'boardCode1','itemCode',0,0,'siloCode1',16,0,1,1,'2023-04-04 13:34:59','2023-04-04 13:37:04',1);
INSERT INTO `t_material_stock_detail` VALUES (3,0,'boardCode2','itemCode3',0,0,'siloCode1',5,0,1,1,'2023-04-04 13:35:16',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify`
--

LOCK TABLES `t_notify` WRITE;
/*!40000 ALTER TABLE `t_notify` DISABLE KEYS */;
INSERT INTO `t_notify` VALUES (31,'2023-04-19 13:31:28','1','1','1',22,'钉钉',0,1,1,'2023-04-19 01:31:42','2023-04-21 05:02:06',1,NULL);
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
INSERT INTO `t_notify_setting` VALUES (11,'大屏告警',0,'BIG_SCREEN_WARNING',NULL,NULL,NULL,0,NULL,0,0,1,'2023-02-28 09:14:03','2023-02-28 09:19:33',1,NULL);
INSERT INTO `t_notify_setting` VALUES (12,'短信',0,'LIGHT_WARNING',NULL,NULL,NULL,0,NULL,0,1,1,'2023-02-28 09:19:38','2023-04-17 04:13:26',1,'0');
INSERT INTO `t_notify_setting` VALUES (21,'微信',0,'2222',NULL,NULL,NULL,1,5,0,1,1,'2023-03-28 21:29:34','2023-04-18 03:22:31',1,'0');
INSERT INTO `t_notify_setting` VALUES (22,'钉钉',0,'2222',NULL,NULL,NULL,0,5,0,0,1,'2023-03-29 01:11:08','2023-04-21 01:53:22',1,'0');
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
  `finish_status` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '完成状态',
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
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='板料追踪';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_panel`
--

LOCK TABLES `t_panel` WRITE;
/*!40000 ALTER TABLE `t_panel` DISABLE KEYS */;
INSERT INTO `t_panel` VALUES (16,'123456787654321','PH2345678A1','1001','成品仓库','1',0,0,0,1,0,1,'2023-04-04 14:20:00','2023-04-18 01:40:39',1);
INSERT INTO `t_panel` VALUES (17,'123456787654322','PH2345678A1','1002','待钻孔暂存区','0',NULL,NULL,16,1,0,1,'2023-04-04 03:03:05','2023-04-18 01:40:49',1);
INSERT INTO `t_panel` VALUES (31,'P202304210001','PH2345678A1','1122','1122','1',NULL,NULL,0,1,0,1,'2023-04-27 04:55:37','2023-04-27 05:13:52',33);
INSERT INTO `t_panel` VALUES (35,'P202304210002','PH2345678A1','1133',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-27 05:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (36,'P202304210003','PH2345678A1','1144',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-27 08:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (37,'P202304210004','PH2345678A1','1155',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-27 09:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (38,'P202304210005','PH2345678A1','1166',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-27 14:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (39,'P202304210006','PH2345678A1','1177',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-28 10:41:31',NULL,NULL);
INSERT INTO `t_panel` VALUES (40,'P202304210007','PH2345678A1','1178',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-28 12:02:55',NULL,NULL);
INSERT INTO `t_panel` VALUES (41,'P202304210008','PH2345678A1','1179',NULL,NULL,NULL,NULL,0,1,0,1,'2023-04-28 15:54:31',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (21,'叠板','pin','111',0,1,1,'2023-04-05 02:30:19',NULL,NULL);
INSERT INTO `t_process` VALUES (22,'钻孔','drill','111',0,1,1,'2023-04-05 02:30:42',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工艺参数、配方管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process_recipe`
--

LOCK TABLES `t_process_recipe` WRITE;
/*!40000 ALTER TABLE `t_process_recipe` DISABLE KEYS */;
INSERT INTO `t_process_recipe` VALUES (12,'yy','yy',NULL,NULL,NULL,NULL,NULL,0,1,1,'2023-02-28 10:23:22',NULL,NULL);
INSERT INTO `t_process_recipe` VALUES (13,'zz11','zz11111',NULL,NULL,NULL,NULL,NULL,0,0,1,'2023-02-28 10:23:25','2023-03-24 05:15:59',1);
INSERT INTO `t_process_recipe` VALUES (17,'11','11',NULL,NULL,NULL,NULL,NULL,0,1,1,'2023-04-12 05:20:17',NULL,NULL);
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=60 DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
INSERT INTO `t_product_category` VALUES (4,0,'p04','刚性电路板','刚性电路板',0,0,1,'2023-03-28 16:47:00','2023-04-23 02:16:03',33,'0');
INSERT INTO `t_product_category` VALUES (5,0,'p03','多层板','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
INSERT INTO `t_product_category` VALUES (6,0,'p02','双面板','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
INSERT INTO `t_product_category` VALUES (7,0,'p01','单面板','111',0,1,1,'2023-03-28 16:47:00','2023-04-12 05:12:53',1,'0');
INSERT INTO `t_product_category` VALUES (8,0,'p05','柔性电路板','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
INSERT INTO `t_product_category` VALUES (9,0,'p06','刚柔结合电路板','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
INSERT INTO `t_product_category` VALUES (10,0,'p07','高频电路板','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
INSERT INTO `t_product_category` VALUES (11,0,'p08','高速电路板','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
INSERT INTO `t_product_category` VALUES (12,11,'P08-01','高速电路板--客户001指定','',0,1,1,'2023-03-28 16:47:00',NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route`
--

LOCK TABLES `t_route` WRITE;
/*!40000 ALTER TABLE `t_route` DISABLE KEYS */;
INSERT INTO `t_route` VALUES (2,'B0002','工艺路线B0002','全自动','测试使用',1,0,1,'2023-04-04 15:00:09','2023-04-21 04:33:15',33);
INSERT INTO `t_route` VALUES (3,'A0001','工艺路线A0001','半自动',NULL,1,0,1,'2023-04-05 01:57:50','2023-04-19 02:28:46',1);
INSERT INTO `t_route` VALUES (16,'22222','333333','333333333','333333333333',1,0,1,'2023-04-20 04:46:45','2023-04-21 04:31:59',33);
INSERT INTO `t_route` VALUES (17,'1','111','1','1',1,0,1,'2023-04-20 22:37:32','2023-04-21 04:31:54',33);
INSERT INTO `t_route` VALUES (18,'2','2','2','2',1,0,1,'2023-04-20 22:48:10',NULL,NULL);
INSERT INTO `t_route` VALUES (19,'C0003','工艺路线C0003',NULL,NULL,1,0,33,'2023-04-23 02:34:38','2023-04-23 02:45:29',33);
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与工序关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_process`
--

LOCK TABLES `t_route_and_process` WRITE;
/*!40000 ALTER TABLE `t_route_and_process` DISABLE KEYS */;
INSERT INTO `t_route_and_process` VALUES (5,2,13,3,'1','#00AEF3',80,'0',4,1,0,1,'2023-04-05 02:41:06','2023-04-14 02:42:32',1);
INSERT INTO `t_route_and_process` VALUES (9,3,13,3,'1','#F31800',840,'0',NULL,0,0,1,'2023-04-07 02:40:29','2023-04-11 22:36:51',1);
INSERT INTO `t_route_and_process` VALUES (10,3,21,2,'0','#00AEF3',960,'0',NULL,0,0,1,'2023-04-07 03:30:26','2023-04-19 23:31:55',1);
INSERT INTO `t_route_and_process` VALUES (11,2,22,2,'1','#00AEF3',40,'0',3,1,0,1,'2023-04-10 22:58:49','2023-04-21 04:33:06',33);
INSERT INTO `t_route_and_process` VALUES (12,2,21,1,'0','#0A1C23',60,'0',2,1,0,1,'2023-04-10 22:59:09','2023-04-21 04:33:13',33);
INSERT INTO `t_route_and_process` VALUES (13,3,21,1,'1','#444C4F',840,'0',1,1,0,1,'2023-04-12 02:01:11','2023-04-20 01:33:52',1);
INSERT INTO `t_route_and_process` VALUES (23,16,23,1,'1','#00AEF3',123,'1',4,1,0,1,'2023-04-20 04:47:28',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (27,19,22,1,'1','#D22525',180,'1',2,1,0,33,'2023-04-23 02:41:28','2023-04-23 02:42:20',33);
INSERT INTO `t_route_and_process` VALUES (28,19,21,2,'0','#6CD228',10,'0',2,1,0,33,'2023-04-23 02:42:00','2023-04-23 02:44:29',33);
INSERT INTO `t_route_and_process` VALUES (29,19,23,3,'0','#1849DC',10,'0',2,1,0,33,'2023-04-23 02:44:55',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与产品大类关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_product_category`
--

LOCK TABLES `t_route_and_product_category` WRITE;
/*!40000 ALTER TABLE `t_route_and_product_category` DISABLE KEYS */;
INSERT INTO `t_route_and_product_category` VALUES (5,2,7,1,0,1,'2023-04-06 01:37:07','2023-04-06 02:23:56',1);
INSERT INTO `t_route_and_product_category` VALUES (8,2,12,1,0,1,'2023-04-06 02:25:29',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (9,2,11,1,0,1,'2023-04-07 02:50:51',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (10,2,9,1,0,1,'2023-04-10 22:54:31',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (12,2,10,1,0,1,'2023-04-10 22:58:35',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (22,3,6,1,0,1,'2023-04-18 23:01:24',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (23,3,10,1,0,1,'2023-04-18 23:01:29',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (25,17,7,1,0,1,'2023-04-20 22:40:07',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (26,17,6,1,0,1,'2023-04-20 22:49:04',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (27,19,7,1,0,33,'2023-04-23 02:45:25',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COMMENT='工序与工作站关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_process_and_work_station`
--

LOCK TABLES `t_route_process_and_work_station` WRITE;
/*!40000 ALTER TABLE `t_route_process_and_work_station` DISABLE KEYS */;
INSERT INTO `t_route_process_and_work_station` VALUES (1,1,1,0,1,0,1,'2023-04-05 13:38:09',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (2,1,13,0,0,0,1,'2023-04-05 13:38:18','2023-04-07 08:58:07',1);
INSERT INTO `t_route_process_and_work_station` VALUES (9,5,219,1,1,0,1,'2023-04-11 22:35:32',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (10,11,212,1,1,0,1,'2023-04-11 22:35:54',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (11,12,218,1,1,0,1,'2023-04-11 22:36:12',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (13,9,219,1,1,0,1,'2023-04-11 22:36:50',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (14,8,216,1,1,0,1,'2023-04-11 22:37:23',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (28,10,216,1,1,0,1,'2023-04-18 23:05:31',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (31,24,216,1,1,0,1,'2023-04-20 22:39:49',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (32,24,219,1,1,0,1,'2023-04-20 22:50:09',NULL,NULL);
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
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_schedulement`
--

LOCK TABLES `t_schedulement` WRITE;
/*!40000 ALTER TABLE `t_schedulement` DISABLE KEYS */;
INSERT INTO `t_schedulement` VALUES (1,'0',NULL,NULL,NULL,NULL,NULL,NULL,0,1,1,'2023-04-03 02:48:24',NULL,NULL);
INSERT INTO `t_schedulement` VALUES (2,'2',NULL,NULL,NULL,NULL,NULL,NULL,0,1,1,'2023-04-03 02:48:36','2023-04-03 02:48:43',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='点检保养项目';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_subject`
--

LOCK TABLES `t_subject` WRITE;
/*!40000 ALTER TABLE `t_subject` DISABLE KEYS */;
INSERT INTO `t_subject` VALUES (1,'subject1',0,'subject1','string','string','string','0',0,1,1,'2023-04-06 12:46:55',NULL,NULL);
INSERT INTO `t_subject` VALUES (2,'subject2',1,'subject2','string','string','string','0,1',0,1,1,'2023-04-06 12:47:08',NULL,NULL);
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产任务';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_task`
--

LOCK TABLES `t_task` WRITE;
/*!40000 ALTER TABLE `t_task` DISABLE KEYS */;
INSERT INTO `t_task` VALUES (11,'1112111',0,'qqq',12,'1','1','1',1,'钻机','1',NULL,'钻孔','12',NULL,'item01',NULL,1,NULL,'pcs',120,NULL,100,NULL,NULL,NULL,NULL,NULL,'2023-04-06 00:00:00',8,'2023-04-26 00:00:00',NULL,'COMMIT','#00AEF3','N',0,1,1,'2023-03-01 09:19:18','2023-04-12 01:01:14',1,'0');
INSERT INTO `t_task` VALUES (13,'dd1111',0,'dd112',12,'1','1','1',1,'钻机','1',NULL,'钻孔','12',NULL,'item02',NULL,1,NULL,'pcs',120,NULL,110,NULL,NULL,NULL,NULL,NULL,'2023-04-06 00:00:00',3,'2023-04-26 00:00:00',NULL,'DRAFT','#00AEF3','N',0,1,1,'2023-03-01 13:31:13','2023-04-10 05:26:17',1,'0');
INSERT INTO `t_task` VALUES (14,'string',0,'string',16,'string','string','string',0,'钻机','string',0,'钻孔','string',0,'item03','string',1,'string','pcs',120,0,90,0,0,0,'string','string','2023-04-10 02:00:00',2,'2023-04-26 18:00:00','2023-04-06 03:10:47','DRAFT','#00AEF3','N',0,1,1,'2023-04-05 23:11:43','2023-04-16 23:15:59',1,'0,11');
INSERT INTO `t_task` VALUES (15,'string',0,'string',16,'string','string','string',0,'钻机','string',0,'钻孔','string',0,'item04','string',1,'string','pcs',120,150,150,0,0,0,'string','string','2023-04-10 19:00:00',2,'2023-04-26 11:00:00','2023-04-06 03:10:47','DRAFT','#00AEF3','N',0,1,1,'2023-04-05 23:12:00','2023-04-17 02:04:27',1,'0,13');
INSERT INTO `t_task` VALUES (24,'mock1',0,'mock1',18,NULL,'测试001',NULL,217,'钻机01-0003','drill01-0003',21,'钻孔',NULL,NULL,'item05',NULL,1,NULL,NULL,120,NULL,80,NULL,NULL,NULL,NULL,NULL,'2023-04-07 16:00:00',5,'2023-04-27 08:00:00',NULL,'DRAFT','#533737','0',0,1,1,'2023-04-07 04:55:56',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (28,'钻孔1',0,'钻孔1',18,NULL,'测试001',NULL,217,'钻机01-0003','drill01-0003',22,'钻孔','测试001',36,'item07',NULL,22,NULL,NULL,120,NULL,110,NULL,NULL,0,'string','string','2023-04-10 08:00:00',3,'2023-04-27 16:00:00','2023-04-10 00:39:04','DRAFT','#DD1313','0',0,1,1,'2023-04-09 20:43:49','2023-04-09 20:44:04',1,'0');
INSERT INTO `t_task` VALUES (29,'拆板1',0,'拆板1',18,NULL,NULL,NULL,219,'拆板机01-0001','unpin01-0001',13,'拆板',NULL,NULL,'item08',NULL,1,NULL,NULL,120,NULL,100,NULL,NULL,NULL,NULL,NULL,'2023-04-10 08:00:00',3,'2023-04-27 16:00:00','2023-04-10 00:39:04','DRAFT','#604242',NULL,0,1,1,'2023-04-09 20:44:57',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (30,'叠板',0,'22',19,NULL,'PCB单层','01',218,'叠板机01-0001','pin01-0001',21,'叠板','MO202304050002',36,'item09','IF2023040500002',1,'DDD XXXX DD','pcs',120,NULL,90,NULL,NULL,1,'string','string','2023-04-09 19:00:00',3,'2023-04-27 19:00:00','2023-04-10 00:39:04','DRAFT','#0079F3','0',0,1,1,'2023-04-10 05:15:53','2023-04-14 05:08:17',1,'0');
INSERT INTO `t_task` VALUES (31,'叠板',0,'222',19,NULL,'PCB单层','01',218,'叠板机01-0001','pin01-0001',21,'叠板','MO202304050002',36,'item10','IF2023040500002',1,'DDD XXXX DD','pcs',120,NULL,80,NULL,NULL,1,'string','string','2023-04-10 03:00:00',3,'2023-04-11 03:00:00','2023-04-10 00:39:04','DRAFT','#424998','0',0,1,1,'2023-04-10 05:22:34','2023-04-14 03:23:05',1,'0');
INSERT INTO `t_task` VALUES (34,'钻孔',0,'zuankong002',19,NULL,NULL,NULL,212,'钻机01-0001','drill01-0001',22,'钻孔',NULL,NULL,NULL,NULL,1,NULL,NULL,2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-10 03:00:00',1,'2023-04-10 11:00:00','2023-04-10 00:39:04','DRAFT','#FF0000',NULL,0,1,1,'2023-04-10 21:20:55','2023-04-17 02:07:36',1,'0');
INSERT INTO `t_task` VALUES (35,'拆板',0,'unpin',19,NULL,NULL,NULL,219,'拆板机01-0001','unpin01-0001',13,'拆板',NULL,NULL,NULL,NULL,1,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-11 08:00:00',1,'2023-04-11 16:00:00','2023-04-10 00:39:04','DRAFT','#00FF48',NULL,0,1,1,'2023-04-10 21:21:29',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (36,'钻孔',0,'drill001',20,NULL,NULL,NULL,217,'钻机01-0003','drill01-0003',22,'钻孔','drill',NULL,NULL,NULL,1,NULL,NULL,120,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-12 11:00:00',1,'2023-04-12 19:00:00','2023-04-10 00:39:04','DRAFT','#FF0000',NULL,0,1,1,'2023-04-10 21:29:16','2023-04-17 02:05:51',1,'0');
INSERT INTO `t_task` VALUES (38,'1',0,'1',21,NULL,'PCB单层','22',212,'钻机01-0001','drill01-0001',21,'叠板','pin',36,'PCB单层板','IF2023040500002',22,'DDD XXXX DD','Panel',1,NULL,NULL,NULL,NULL,3,'string','string','2023-04-11 00:00:00',1,'2023-04-11 08:00:00',NULL,'DRAFT','#56AED0','0',0,1,1,'2023-04-12 03:22:05','2023-04-21 03:48:57',1,'0');
INSERT INTO `t_task` VALUES (40,'钻孔1',0,'钻孔1',21,NULL,NULL,NULL,212,'钻机01-0001','drill01-0001',22,'钻孔','drill',NULL,NULL,NULL,1,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-12 16:00:00',1,'2023-04-13 00:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-12 04:41:17','2023-04-21 00:58:46',1,'0');
INSERT INTO `t_task` VALUES (56,'22',0,'22',23,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:17:56',1,'2023-04-18 14:17:56',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:18:21',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (57,'11',0,'11',23,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:19:08',1,'2023-04-18 14:19:07',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:19:31',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (58,'11',0,'11',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 16:00:00',1,'2023-04-19 00:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:20:08','2023-04-21 00:57:27',1,'0');
INSERT INTO `t_task` VALUES (59,'2222222',0,'22222222',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 16:00:00',1,'2023-04-19 00:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:27:46','2023-04-21 00:57:16',1,'0');
INSERT INTO `t_task` VALUES (60,'12',0,'12',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-19 00:00:00',1,'2023-04-19 08:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:34:20','2023-04-21 00:57:11',1,'0');
INSERT INTO `t_task` VALUES (61,'222221111',0,'22221111',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 08:00:00',1,'2023-04-18 16:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:35:06','2023-04-21 00:57:13',1,'0');
INSERT INTO `t_task` VALUES (62,'11',0,'11',26,'MO202304020001','PCB单层','22',218,'叠板机01-0001','pin01-0001',21,'PCB单层','MO202304020001',36,'Panel单层01','IF2023040500002',NULL,'11','Panel',2,NULL,NULL,NULL,NULL,1,'string','string','2023-04-18 00:00:00',50,'2023-05-04 16:00:00',NULL,'DRAFT','11','0',0,1,1,'2023-04-18 01:36:43','2023-04-21 00:59:54',1,'0');
INSERT INTO `t_task` VALUES (64,'#0A1C23',0,'#0A1C23',25,'MO202304030001','PCB单层',NULL,218,'叠板机01-0001','pin01-0001',21,'PCB单层','MO202304030001',36,'Panel单层02','IF2023040500002',22,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 08:00:00',1,'2023-04-18 16:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:55:09','2023-04-21 00:57:23',1,'0');
INSERT INTO `t_task` VALUES (77,'1',0,'1',26,'MO202304020001','PCB单层','22',218,'叠板机01-0001','pin01-0001',21,'PCB单层','MO202304020001',36,'Panel单层01','IF2023040500002',22,'11','Panel',1,NULL,NULL,NULL,NULL,1,'string','string','2023-04-18 08:00:00',1,'2023-04-18 08:00:00','2023-02-28 10:10:19','DRAFT','#0A1C23','0',0,1,1,'2023-04-20 05:20:28','2023-04-21 01:02:04',1,'0');
INSERT INTO `t_task` VALUES (78,'Task0001',0,'Task0001',39,'MO202304230001','20230423客户订单',NULL,218,'叠板机01-0001','pin01-0001',21,'20230423客户订单','MO202304230001',36,'PCB单层板','IF2023040500002',22,'DDD XXXX DD','Panel',50,NULL,NULL,NULL,NULL,41,'001','001','2023-04-24 14:54:47',3,'2023-04-30 12:54:47','2023-04-23 00:00:00','DRAFT','#0A1C23','0',0,1,33,'2023-04-23 02:56:18','2023-04-23 02:59:35',33,'0');
INSERT INTO `t_task` VALUES (79,'Task0002',0,'Task0002',39,'MO202304230001','20230423客户订单',NULL,212,'钻机01-0001','drill01-0001',22,'20230423客户订单','MO202304230001',36,'PCB单层板','IF2023040500002',22,'DDD XXXX DD','Panel',50,NULL,NULL,NULL,NULL,41,'001','001','2023-04-24 06:56:09',5,'2023-04-30 21:36:09','2023-04-23 00:00:00','DRAFT','#13B1EE','0',0,1,33,'2023-04-23 02:57:29','2023-04-23 02:59:44',33,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产流转单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_trans_order`
--

LOCK TABLES `t_trans_order` WRITE;
/*!40000 ALTER TABLE `t_trans_order` DISABLE KEYS */;
INSERT INTO `t_trans_order` VALUES (11,'ss',0,'ss',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,22,NULL,NULL,NULL,NULL,0,1,1,'2023-03-01 13:40:03',NULL,NULL);
INSERT INTO `t_trans_order` VALUES (13,'xx11',0,'xx22',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,0,0,1,'2023-03-01 13:40:18','2023-03-24 02:51:16',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (33,'Panel','单层Panel','Y',34,2.0000,NULL,0,1,1,'2023-04-18 22:11:38','2023-04-20 21:01:06',1);
INSERT INTO `t_unit_measure` VALUES (34,'叠板2层','叠板2层','Y',NULL,1.0000,NULL,0,1,1,'2023-04-18 22:13:06','2023-04-20 02:42:16',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_vendor`
--

LOCK TABLES `t_vendor` WRITE;
/*!40000 ALTER TABLE `t_vendor` DISABLE KEYS */;
INSERT INTO `t_vendor` VALUES (37,'李老板','李老板',NULL,0,1,1,'2023-04-18 22:22:01',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COMMENT='仓库表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_warehouse`
--

LOCK TABLES `t_warehouse` WRITE;
/*!40000 ALTER TABLE `t_warehouse` DISABLE KEYS */;
INSERT INTO `t_warehouse` VALUES (2,'WareHouse2','WareHouse2','string','admin','WareHouse2',0,1,1,'2023-04-03 14:03:07','2023-04-04 02:17:25',1);
INSERT INTO `t_warehouse` VALUES (14,'WareHouse','WareHouse',NULL,NULL,NULL,0,1,1,'2023-04-18 23:36:14',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (15,'2','2','钻机01-0002','2','2',0,1,1,'2023-04-20 04:59:06',NULL,NULL);
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
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产工单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_work_order`
--

LOCK TABLES `t_work_order` WRITE;
/*!40000 ALTER TABLE `t_work_order` DISABLE KEYS */;
INSERT INTO `t_work_order` VALUES (21,'PCB单层',0,'MO202304050003','客户订单','',36,'PCB单层板','IF2023040500002',22,'22','DDD XXXX DD','Panel',130,130,100,2,3,'string','string','2023-02-28 10:10:19','DRAFT',0,1,1,'2023-04-25 02:55:53','2023-04-21 03:53:01',1,'0');
INSERT INTO `t_work_order` VALUES (25,'PCB单层',0,'MO202304030001','库存需求','',36,'Panel单层02','IF2023040500002',22,'22','11','Panel',144,150,100,NULL,NULL,NULL,NULL,'2023-02-28 10:10:19','DRAFT',0,1,1,'2023-04-26 01:20:02','2023-04-21 03:53:06',1,'0');
INSERT INTO `t_work_order` VALUES (26,'PCB单层',0,'MO202304020001','客户订单','',36,'Panel单层01','IF2023040500002',22,'22','11','Panel',120,2,80,2,1,'string','string','2023-02-28 10:10:19','DRAFT',0,1,1,'2023-04-18 01:20:35','2023-04-21 03:53:11',1,'0');
INSERT INTO `t_work_order` VALUES (39,'20230423客户订单',0,'MO202304230001','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22','DDD XXXX DD','Panel',260,260,200,NULL,41,'张老板','001','2023-04-23 00:00:00','COMMITED',0,1,33,'2023-04-23 02:53:50','2023-04-23 02:54:17',33,'0');
INSERT INTO `t_work_order` VALUES (44,'PCB单层',0,'MO202304270001','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',180,185,170,NULL,NULL,NULL,NULL,'2023-04-28 12:00:00','DRAFT',0,1,1,'2023-04-26 02:53:50',NULL,NULL,NULL);
INSERT INTO `t_work_order` VALUES (45,'PCB单层',0,'MO202304270002','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',220,220,215,NULL,NULL,NULL,NULL,'2023-04-28 12:00:00','DRAFT',0,1,1,'2023-04-27 02:53:50',NULL,NULL,NULL);
INSERT INTO `t_work_order` VALUES (46,'PCB单层',0,'MO202304270003','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',260,268,200,NULL,NULL,NULL,NULL,'2023-04-29 18:00:00','DRAFT',0,1,1,'2023-04-28 02:53:50',NULL,NULL,NULL);
INSERT INTO `t_work_order` VALUES (47,'PCB单层',0,'MO202304280001','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',100,110,90,NULL,NULL,NULL,NULL,'2023-04-29 18:00:00','DRAFT',0,1,1,'2023-04-29 08:38:15',NULL,NULL,NULL);
INSERT INTO `t_work_order` VALUES (48,'PCB单层',0,'MO202304280002','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',160,180,30,NULL,NULL,NULL,NULL,'2023-05-10 18:00:00','DRAFT',0,1,1,'2023-04-30 08:38:15',NULL,NULL,NULL);
INSERT INTO `t_work_order` VALUES (49,'PCB单层',0,'MO202304280003','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',210,215,89,NULL,NULL,NULL,NULL,'2023-05-10 18:00:00','DRAFT',0,1,1,'2023-05-01 08:38:15',NULL,NULL,NULL);
INSERT INTO `t_work_order` VALUES (50,'PCB单层',0,'MO202304280004','客户订单',NULL,36,'PCB单层板','IF2023040500002',22,'22',NULL,'Panel',240,260,113,NULL,NULL,NULL,NULL,'2023-05-12 18:00:00','DRAFT',0,1,1,'2023-05-02 08:38:15',NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
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
) ENGINE=InnoDB AUTO_INCREMENT=232 DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workstation`
--

LOCK TABLES `t_workstation` WRITE;
/*!40000 ALTER TABLE `t_workstation` DISABLE KEYS */;
INSERT INTO `t_workstation` VALUES (212,'drill01-0001','钻机01-0001',9,NULL,'车间01（测试）','',0,1,1,'2023-04-04 02:32:34','2023-04-05 22:36:14',1);
INSERT INTO `t_workstation` VALUES (216,'dril01-0002','钻机01-0002',NULL,NULL,'车间01（测试）',NULL,0,1,1,'2023-04-05 22:36:39',NULL,NULL);
INSERT INTO `t_workstation` VALUES (217,'drill01-0003','钻机01-0003',NULL,NULL,'车间01（测试）',NULL,0,1,1,'2023-04-05 22:37:47',NULL,NULL);
INSERT INTO `t_workstation` VALUES (218,'pin01-0001','叠板机01-0001',NULL,NULL,'车间01（测试）',NULL,0,1,1,'2023-04-05 22:39:06',NULL,NULL);
INSERT INTO `t_workstation` VALUES (219,'unpin01-0001','拆板机01-0001',NULL,NULL,'车间01（测试）',NULL,0,1,1,'2023-04-05 22:39:38',NULL,NULL);
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

-- Dump completed on 2023-04-28  1:01:18
