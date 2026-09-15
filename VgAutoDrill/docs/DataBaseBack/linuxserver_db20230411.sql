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
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'system','wm','#',1,NULL,NULL,50,0,1,1,'2023-04-01 02:50:34','2023-04-01 03:13:54',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (1,'admin','5794620ce8325bc4e7b67677da3daeaa','c5fc028a23cd41c696a004f9dba65bff','ADMIN',1,NULL,1,NULL,NULL,'13400000001',NULL,NULL,NULL,1,NULL,NULL,0,1,NULL,'2022-09-27 16:07:39',NULL,NULL);
INSERT INTO `sys_user` VALUES (24,'txc','a445250568e2f7929e8cd1e7b2fff26e','99fef4d1addf4cc9b475f65f7ec35740','txcw',1,2,0,NULL,NULL,'13411111111','',NULL,NULL,NULL,'2222',NULL,0,1,1,'2022-12-26 16:13:18','2023-03-23 02:53:53',1);
INSERT INTO `sys_user` VALUES (26,'123','43f9621a94bf12a01a0af37f2d957600','5de8c272c748432d86bd028afba0639d','bb',1,3,1,NULL,NULL,'13346859237','7328910@qq.com',NULL,NULL,NULL,'1',NULL,0,1,1,'2023-02-22 01:37:04',NULL,NULL);
INSERT INTO `sys_user` VALUES (27,'lisi','9438571d588b26e234709e6f975ba3e4','c84681f4183543f4910f9f34683f73d8','1',1,4,0,NULL,NULL,'18339475689','23459378@qq.com',NULL,NULL,NULL,'2',NULL,0,1,1,'2023-02-22 01:38:14',NULL,NULL);
INSERT INTO `sys_user` VALUES (28,'xiaoming','8df2f4462922bca05ea29b079065fc48','01266758ce8a46b09004a4185f6934b1','xt',2,2,0,NULL,NULL,'15634785930','22348340@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-02-22 01:40:03',NULL,NULL);
INSERT INTO `sys_user` VALUES (29,'xxx','02cf88a12801b31963c3476bb40632b2','0a73cbd4e99b42fcbcfc4f4d41e14f55','1',1,2,0,NULL,NULL,'18839654338','2234785490@qq.com',NULL,NULL,NULL,'2',NULL,0,1,1,'2023-02-22 21:07:10',NULL,NULL);
INSERT INTO `sys_user` VALUES (30,'小昭','e1b161f8020fc0190e98e8864cae55e7','f4f02efeedb4449d82da200ae48e1992','zzz111',9,4,0,NULL,NULL,'15614151678','15614151678@163.com',NULL,NULL,NULL,'111',NULL,0,1,1,'2023-03-23 02:55:24','2023-04-18 02:27:52',1);
INSERT INTO `sys_user` VALUES (32,'tony','c729d9309ebfc53041158593ddceafe4','933fffddb1b64f96ba4a8b3fba7ec204','tony',1,2,1,NULL,NULL,'18900002222','',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:41:31',NULL,NULL);
INSERT INTO `sys_user` VALUES (33,'yp','cc2e5c672fe9687ca8b0070316b0cc60','0d03255aefbd455d96c6191e562b4042','yp',1,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:50:52',NULL,NULL);
INSERT INTO `sys_user` VALUES (34,'yp1','2caa82251c047aac1562c22600d30121','116f35c84d344addb7d7bb5f5b972b13','yp1',NULL,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,33,'2023-04-18 02:09:21',NULL,NULL);
INSERT INTO `sys_user` VALUES (35,'yp2','efcdb895983cb6fa6443212a208c1681','24eb81b8b3ec42919ae3b01f111df643','yp2',NULL,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,33,'2023-04-18 02:09:42',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (3,24,2,1,'2022-12-26 16:13:18');
INSERT INTO `sys_user_role` VALUES (4,1,1,1,'2022-12-26 17:17:36');
INSERT INTO `sys_user_role` VALUES (6,26,2,1,'2023-02-22 01:37:04');
INSERT INTO `sys_user_role` VALUES (7,27,2,1,'2023-02-22 01:38:14');
INSERT INTO `sys_user_role` VALUES (8,28,2,1,'2023-02-22 01:40:03');
INSERT INTO `sys_user_role` VALUES (9,29,2,1,'2023-02-22 21:07:10');
INSERT INTO `sys_user_role` VALUES (10,30,2,1,'2023-03-23 02:55:24');
INSERT INTO `sys_user_role` VALUES (12,32,1,1,'2023-04-05 22:41:31');
INSERT INTO `sys_user_role` VALUES (13,33,1,1,'2023-04-05 22:50:52');
INSERT INTO `sys_user_role` VALUES (14,34,6,33,'2023-04-18 02:09:21');
INSERT INTO `sys_user_role` VALUES (15,35,8,33,'2023-04-18 02:09:42');
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
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm`
--

LOCK TABLES `t_alarm` WRITE;
/*!40000 ALTER TABLE `t_alarm` DISABLE KEYS */;
INSERT INTO `t_alarm` VALUES (42,'2023-04-19 13:19:40','1','1',2,NULL,0,1,1,'2023-04-19 01:19:53',NULL,NULL,0,'');
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
INSERT INTO `t_alarm_setting` VALUES (15,'AGV电量水平低于10%',0,'LOWER_THAN_10','AGV电量水平低于10%',1,0,1,1,'2023-02-27 16:26:42','2023-04-18 05:16:43',1,64,'DRILL_REQUEST_LOAD_RAW_MATERIAL','22,11','钉钉,大屏告警','AGV电量水平低于10%11111111','0');
/*!40000 ALTER TABLE `t_alarm_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_board_trace`
--

DROP TABLE IF EXISTS `t_board_trace`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_board_trace` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `board_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '板料编号',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '物料代码',
  `batch_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '批次号',
  `board_location` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '板料位置',
  `finish_status` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '完成状态',
  `pcs` int(11) DEFAULT NULL COMMENT '单叠数量',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='板料追踪';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_board_trace`
--

LOCK TABLES `t_board_trace` WRITE;
/*!40000 ALTER TABLE `t_board_trace` DISABLE KEYS */;
INSERT INTO `t_board_trace` VALUES (16,'123456787654321','PH2345678A1','1001','成品仓库','1',0,0,1,0,1,'2023-04-04 14:20:00','2023-04-18 01:40:39',1);
INSERT INTO `t_board_trace` VALUES (17,'123456787654322','PH2345678A1','1002','待钻孔暂存区','0',NULL,NULL,1,0,1,'2023-04-04 03:03:05','2023-04-18 01:40:49',1);
/*!40000 ALTER TABLE `t_board_trace` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COMMENT='检验记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_check_records`
--

LOCK TABLES `t_check_records` WRITE;
/*!40000 ALTER TABLE `t_check_records` DISABLE KEYS */;
INSERT INTO `t_check_records` VALUES (13,16,NULL,NULL,0,NULL,NULL,1,'admin1','1','2023-04-19 00:50:07','合格品',1,0,1,'2023-04-12 15:06:18','2023-04-17 05:23:28',1);
INSERT INTO `t_check_records` VALUES (15,1,NULL,NULL,1,NULL,NULL,1,'1','-1','2023-04-19 00:49:43','抽检不合格',1,0,1,'2023-04-14 04:16:04','2023-04-17 05:23:22',1);
INSERT INTO `t_check_records` VALUES (16,54,'11','11',23,NULL,'PCB单层',NULL,NULL,'1','2023-04-19 00:50:20','合格品',1,0,1,'2023-04-18 01:11:59',NULL,NULL);
INSERT INTO `t_check_records` VALUES (17,54,'11','11',23,NULL,'PCB单层',NULL,NULL,'-1','2023-04-19 00:49:36',NULL,1,0,1,'2023-04-18 01:14:58',NULL,NULL);
INSERT INTO `t_check_records` VALUES (18,54,'11','11',23,NULL,'PCB单层',NULL,NULL,'-1','2023-04-19 00:49:33',NULL,1,0,1,'2023-04-18 01:16:33',NULL,NULL);
INSERT INTO `t_check_records` VALUES (19,54,'T2522222','钻孔',67890,'MO202222','PCB单层',100025,'张三','-1','2023-04-19 00:49:30',NULL,1,0,1,'2023-04-18 01:16:58','2023-04-18 01:43:12',1);
INSERT INTO `t_check_records` VALUES (20,63,'#0A1C23','#0A1C23',26,'MO202304020001','PCB单层',NULL,NULL,'-1','2023-04-19 00:49:03','111',1,0,1,'2023-04-18 01:52:07',NULL,NULL);
INSERT INTO `t_check_records` VALUES (21,65,'#6E7F86','#6E7F86',26,'MO202304020001','PCB单层',NULL,NULL,'-1','2023-04-19 00:48:30','强强强强1',1,0,1,'2023-04-18 02:01:04','2023-04-18 03:01:25',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COMMENT='客户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_client`
--

LOCK TABLES `t_client` WRITE;
/*!40000 ALTER TABLE `t_client` DISABLE KEYS */;
INSERT INTO `t_client` VALUES (41,'001','张老板',NULL,0,1,1,'2023-04-18 22:18:28',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_detail`
--

LOCK TABLES `t_cutter_config_detail` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_detail` DISABLE KEYS */;
INSERT INTO `t_cutter_config_detail` VALUES (4,3,0.275,185.000,3.300,25.000,0.300,2000,0,1,1,'2023-03-28 16:46:30','2023-04-10 04:11:07',1);
INSERT INTO `t_cutter_config_detail` VALUES (5,4,0.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:09:22',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (6,5,0.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:10:08',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (21,5,3.000,3.000,3.000,3.000,3.000,0,0,1,1,'2023-03-29 23:31:01','2023-03-29 23:31:31',1);
INSERT INTO `t_cutter_config_detail` VALUES (22,5,2.000,2.000,2.000,2.000,2.000,0,0,1,1,'2023-03-29 23:31:04','2023-03-29 23:31:12',1);
INSERT INTO `t_cutter_config_detail` VALUES (23,4,-1.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:31:51','2023-03-29 23:42:52',1);
INSERT INTO `t_cutter_config_detail` VALUES (24,3,0.250,190.000,3.300,25.000,0.300,2100,0,1,1,'2023-03-29 23:41:01','2023-04-13 02:40:03',1);
INSERT INTO `t_cutter_config_detail` VALUES (25,3,0.200,190.000,3.210,25.000,0.400,2100,0,1,1,'2023-03-29 23:45:04','2023-04-13 03:07:25',1);
INSERT INTO `t_cutter_config_detail` VALUES (48,3,1.000,1.000,1.000,1.000,1.000,4,0,1,1,'2023-04-13 03:13:38','2023-04-13 03:47:33',1);
INSERT INTO `t_cutter_config_detail` VALUES (49,6,1.000,1.000,1.000,1.000,1.000,1,0,1,1,'2023-04-19 00:57:56','2023-04-19 00:58:07',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数主表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_master`
--

LOCK TABLES `t_cutter_config_master` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_master` DISABLE KEYS */;
INSERT INTO `t_cutter_config_master` VALUES (3,'主轴转速200KRPM钻孔','主轴转速200KRPM钻孔','1111','\\\\fileServer\\IF200001111\\IF200001111.dia',8,'p05','柔性电路板',0,1,1,'2023-03-28 16:38:31','2023-04-12 21:08:43',1);
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=67 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
INSERT INTO `t_device` VALUES (11,'drill01','drill01',NULL,7,0,1,1,'2023-02-28 11:40:13','2023-03-22 03:19:19',1,7,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (13,'agv01','agv01',NULL,0,0,1,1,'2023-02-28 11:40:21','2023-04-10 17:29:38',1,8,0,'string','string',0,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (14,'MockAgv02','MockAgv02',NULL,7,0,0,1,'2023-03-03 00:33:58','2023-03-22 02:31:17',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (15,'MockDrill01','MockDrill01',NULL,8,0,1,1,'2023-03-03 00:34:24','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,2,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (16,'P0001','P0001',NULL,8,0,1,1,'2023-03-03 01:32:19','2023-03-08 16:46:09',NULL,14,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (17,'MockAgv06','MockAgv06',NULL,8,0,2,1,'2023-03-07 10:20:48','2023-03-07 10:54:07',NULL,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (18,'MockAgv03','MockAgv03',NULL,8,0,1,1,'2023-03-07 10:21:29','2023-03-17 10:37:59',NULL,8,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (19,'MockAgv04','MockAgv04',NULL,0,0,1,1,'2023-03-07 17:09:12','2023-03-15 15:39:26',NULL,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (20,'P0002','P0002',NULL,0,0,1,1,'2023-03-07 20:19:06','2023-03-07 21:14:21',NULL,14,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (21,'MockAgv104','MockAgv104',NULL,0,0,0,1,'2023-03-09 10:15:32','2023-03-23 02:49:06',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (22,'MockAgv103','MockAgv103',NULL,0,0,1,1,'2023-03-09 10:15:32','2023-03-22 07:52:58',NULL,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (23,'MockDrill02','MockDrill02',NULL,0,0,1,1,'2023-03-13 13:54:18','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,4,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (24,'MockDrill03','MockDrill03',NULL,0,0,1,1,'2023-03-14 08:48:10','2023-03-20 11:46:56',NULL,7,NULL,NULL,NULL,5,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (25,'MockAgv01','MockAgv01',NULL,0,0,1,1,'2023-03-14 08:51:18','2023-04-17 11:34:58',NULL,8,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (26,'MockDrill04','MockDrill04',NULL,0,0,1,1,'2023-03-14 09:22:53','2023-03-20 11:46:56',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (27,'MockDrill05','MockDrill05',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:03',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (28,'MockDrill06','MockDrill06',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:02',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (29,'Processed0001','Processed0001',NULL,0,0,1,1,'2023-03-14 11:19:38','2023-03-16 10:25:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (30,'Processed10001','Processed10001',NULL,0,0,1,1,'2023-03-14 15:50:43','2023-03-20 14:21:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (31,'vegaDrill001','vegaDrill001',NULL,0,0,1,1,'2023-03-14 16:18:04','2023-03-14 16:27:45',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (32,'MockAgv1003','MockAgv1003',NULL,0,0,1,1,'2023-03-14 16:28:34','2023-03-14 16:33:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (33,'vegaDrill0601','vegaDrill0601',NULL,0,0,1,1,'2023-03-14 16:28:35','2023-03-14 16:33:37',NULL,7,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (34,'MockAgv11103','MockAgv11103',NULL,0,0,1,1,'2023-03-14 16:52:45','2023-03-15 08:43:45',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (35,'MockTonyAgv001','MockTonyAgv001',NULL,0,0,1,1,'2023-03-15 09:41:29','2023-03-22 23:45:47',1,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (36,'MockSimpleAgv001','MockSimpleAgv001',NULL,0,0,1,1,'2023-03-15 10:12:33','2023-03-16 14:30:31',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (37,'MockSimpleAgv002','MockSimpleAgv002',NULL,0,0,1,1,'2023-03-15 10:24:25','2023-03-15 10:36:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (38,'Processed002','Processed002',NULL,0,0,1,1,'2023-03-15 10:24:26','2023-03-15 10:36:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (39,'Raw10001','Raw10001',NULL,0,0,1,1,'2023-03-15 14:07:38','2023-03-15 14:28:20',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (40,'Processed100012','Processed100012',NULL,0,0,1,1,'2023-03-15 14:32:46','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (41,'Raw100012','Raw100012',NULL,0,0,1,1,'2023-03-15 14:32:47','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (42,'Processed1000122','Processed1000122',NULL,0,0,1,1,'2023-03-15 16:21:35','2023-03-15 17:58:59',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (43,'Raw1000123','Raw1000123',NULL,0,0,1,1,'2023-03-15 16:21:36','2023-03-23 01:42:59',1,13,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (44,'D0001','D0001',NULL,0,0,1,1,'2023-03-16 11:12:39','2023-03-23 01:43:17',1,13,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (45,'Processed10002','Processed10002',NULL,0,0,1,1,'2023-03-20 14:16:50','2023-03-22 23:46:24',1,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (46,'SiloShelf100012','SiloShelf100012',NULL,0,0,1,1,'2023-03-21 07:01:46','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (49,'UP000102','UP000102',NULL,0,0,1,1,'2023-03-22 07:51:36','2023-03-22 07:52:57',NULL,15,NULL,NULL,NULL,1,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (51,'A00','A00',NULL,0,0,1,1,'2023-03-22 23:02:07','2023-03-23 01:17:05',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (52,'P00002','P00002',NULL,0,0,1,1,'2023-03-22 23:16:16','2023-03-23 01:36:23',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (54,'Q0001','Q0001',NULL,0,0,1,1,'2023-03-23 01:19:33','2023-03-23 02:21:09',1,8,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (55,'生料1','生料1',NULL,0,0,1,1,'2023-03-23 02:50:16','2023-03-23 02:50:29',1,16,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (56,'熟料1111111111111111111','熟料111111111111111',NULL,0,0,1,1,'2023-03-23 21:04:32','2023-03-29 03:19:39',1,17,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (57,'拆板机1','拆板机1',NULL,0,0,1,1,'2023-03-23 21:05:05',NULL,NULL,15,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (58,'叠板机1','叠板机1',NULL,0,0,1,1,'2023-03-23 21:05:22',NULL,NULL,14,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (62,'钻机1','钻机1',NULL,0,0,1,1,'2023-03-27 21:42:13',NULL,NULL,7,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
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
INSERT INTO `t_device_type` VALUES (17,'熟料仓暂存台',1,0,1,1,'2023-03-23 02:46:28','2023-04-01 11:01:10',1,'ProcessedStagingDesk','0,1');
INSERT INTO `t_device_type` VALUES (27,'string01',26,0,1,1,'2023-03-31 17:02:49',NULL,NULL,'string01','0,26');
INSERT INTO `t_device_type` VALUES (28,'11',0,0,1,1,'2023-04-14 04:31:44',NULL,NULL,'11','0');
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
INSERT INTO `t_event_define` VALUES (11,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-03 00:18:07','2023-03-28 04:02:25',1,NULL,0,1);
INSERT INTO `t_event_define` VALUES (12,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 13:46:34',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (13,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 13:47:38',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (14,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 13:47:52',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (15,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 13:48:05',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (16,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 13:49:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (17,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 13:56:34',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (18,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 14:12:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (19,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 14:14:52',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (20,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 14:16:19',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (21,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-03 14:17:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (22,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 14:18:02',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (23,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 15:47:40',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (24,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 15:54:08',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (25,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-03 15:55:45',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (26,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 15:57:13',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (27,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 16:00:31',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (28,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 16:03:06',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (29,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 16:28:17',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (30,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 16:33:47',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (31,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 16:42:39',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (32,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 16:45:14',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (33,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 16:45:50',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (34,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 16:48:55',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (35,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-03 16:49:10',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (36,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 16:54:35',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (37,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 16:56:18',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (38,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 16:59:17',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (39,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 17:06:20',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (40,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 17:13:07',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (41,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-03 17:14:20',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (42,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 17:15:05',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (43,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-03 17:24:46',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (44,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,NULL,1,'2023-03-03 17:26:31',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (45,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-07 09:47:49',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (46,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 09:50:53',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (47,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-07 10:22:19',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (48,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:25:05',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (49,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:25:53',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (50,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:26:08',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (51,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:26:24',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (52,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-07 10:26:26',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (53,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:26:53',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (54,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:32:51',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (55,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:39:09',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (56,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:40:34',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (57,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:42:02',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (58,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:43:22',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (59,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:44:09',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (60,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:46:04',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (61,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,NULL,1,'2023-03-07 10:46:42',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (62,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:47:32',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (63,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:49:18',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (64,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,NULL,1,'2023-03-07 10:53:45',NULL,NULL,NULL,0,1);
INSERT INTO `t_event_define` VALUES (65,'AGV_LOW_BATTERY','AGV_LOW_BATTERY',1,'111',1,'2023-03-07 20:20:49','2023-04-18 03:16:05',1,NULL,0,0);
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
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COMMENT='生产报工记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_feedback`
--

LOCK TABLES `t_feedback` WRITE;
/*!40000 ALTER TABLE `t_feedback` DISABLE KEYS */;
INSERT INTO `t_feedback` VALUES (28,'1',212,'drill01-0001','钻机01-0001',21,'MO202304050003','PCB单层',21,'pin','叠板',38,'1',36,'IF2023040500002','PCB单层板',22,'Panel','DDD XXXX DD',NULL,1.00,1.00,0.00,'admin','txc',NULL,'2023-04-19 00:00:00',NULL,'DRAFT',NULL,0,1,1,'2023-04-18 23:28:50',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品定义表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item`
--

LOCK TABLES `t_item` WRITE;
/*!40000 ALTER TABLE `t_item` DISABLE KEYS */;
INSERT INTO `t_item` VALUES (35,'WareHouse3',0,'WareHouse322','WareHouse3','WareHouse3',1,1,'0',0,1,1,'2023-04-04 02:06:00','2023-04-05 05:05:54',1,7,NULL,'6',NULL,NULL,'11');
INSERT INTO `t_item` VALUES (36,'PCB单层板',0,'IF2023040500002','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-04-05 01:25:38','2023-04-10 22:52:57',1,12,NULL,'12',NULL,NULL,'WareHouse3');
INSERT INTO `t_item` VALUES (40,'4567890',0,'23456以uiop','3456789','6789',1,1,'0',0,1,1,'2023-04-11 23:35:00','2023-04-12 00:55:00',1,5,NULL,NULL,NULL,NULL,'WareHouse2');
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
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_atp_file`
--

LOCK TABLES `t_item_atp_file` WRITE;
/*!40000 ALTER TABLE `t_item_atp_file` DISABLE KEYS */;
INSERT INTO `t_item_atp_file` VALUES (2,'IF2023040500002','PCB单层板',36,22,NULL,'\\\\fileServer\\IF200001111\\IF200001111.atp','string',0,NULL,'\\\\fileServer\\IF200001111\\IF200001111.drl',0,NULL,'\\\\fileServer\\IF200001111\\IF200001111.dia',0,6,1,0,'2023-04-07 07:35:18',0,0,1,'2023-04-07 15:35:28','2023-04-16 22:34:51',1);
INSERT INTO `t_item_atp_file` VALUES (15,'IF','PCB单层板',36,22,NULL,'\\\\\\\\fileServer\\\\IF200001111\\\\IF200001111.atp',NULL,NULL,NULL,'\\\\\\\\fileServer\\\\IF200001111\\\\IF200001111.drl',NULL,NULL,'\\\\\\\\fileServer\\\\IF200001111\\\\IF200001111.dia',1,1,2,0,'2023-04-12 01:04:00',0,1,33,'2023-04-11 21:07:20','2023-04-18 02:46:25',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=4733 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件明细';
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
INSERT INTO `t_item_atp_file_detail` VALUES (3036,15,'T02',0.85,1,0,0,1,'2023-04-12 22:43:52','2023-04-18 02:46:34',1);
INSERT INTO `t_item_atp_file_detail` VALUES (3037,15,'T02',0.85,2,0,0,1,'2023-04-12 22:43:52','2023-04-18 02:46:33',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file`
--

LOCK TABLES `t_item_drill_file` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file` DISABLE KEYS */;
INSERT INTO `t_item_drill_file` VALUES (16,'IF2023040500002','PCB单层板',36,22,NULL,'\\\\fileServer\\IF200001111\\IF200001111.drl',0,1,1,'2023-04-19 01:03:56',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
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
INSERT INTO `t_item_type` VALUES (25,'铜',2,'tong',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:02:53',NULL,NULL);
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
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '事务ID',
  `silo_code` varchar(255) DEFAULT NULL COMMENT '料仓编号',
  `max_layers` int(11) DEFAULT NULL COMMENT '料仓最大层数',
  `current_layers` int(11) DEFAULT NULL COMMENT '料仓实际层数',
  `in_or_out` varchar(50) DEFAULT NULL COMMENT '入库或出库',
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
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COMMENT='库存记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock`
--

LOCK TABLES `t_material_stock` WRITE;
/*!40000 ALTER TABLE `t_material_stock` DISABLE KEYS */;
INSERT INTO `t_material_stock` VALUES (2,'siloCode2',24,18,'out',22,36,'string','string','string','string',0.00,0.00,'string',0,'string','string',NULL,0,'string',0,'string',0,1,1,'2023-04-04 09:56:22',NULL,NULL);
INSERT INTO `t_material_stock` VALUES (3,'siloCode3',24,18,'in',1,38,'string','string','string','string',0.00,0.00,'string',0,'string','string',NULL,0,'string',0,'string',0,1,1,'2023-04-04 09:56:33',NULL,NULL);
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
INSERT INTO `t_notify` VALUES (31,'2023-04-19 13:31:28','1','1','1',NULL,0,1,1,'2023-04-19 01:31:42','2023-04-19 01:34:48',1,NULL);
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
INSERT INTO `t_notify_setting` VALUES (22,'钉钉',0,'2222',NULL,NULL,NULL,0,NULL,0,0,1,'2023-03-29 01:11:08','2023-04-17 04:13:13',1,'0');
/*!40000 ALTER TABLE `t_notify_setting` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (21,'叠板','pin','111',0,1,1,'2023-04-05 02:30:19',NULL,NULL);
INSERT INTO `t_process` VALUES (22,'钻孔','drill','111',0,1,1,'2023-04-05 02:30:42',NULL,NULL);
INSERT INTO `t_process` VALUES (23,'拆板','222',NULL,0,1,1,'2023-04-18 21:35:18','2023-04-18 22:49:29',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
INSERT INTO `t_product_category` VALUES (4,0,'p04','刚性电路板','111',0,0,1,'2023-03-28 16:47:00','2023-04-05 03:47:42',1,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route`
--

LOCK TABLES `t_route` WRITE;
/*!40000 ALTER TABLE `t_route` DISABLE KEYS */;
INSERT INTO `t_route` VALUES (2,'B0002','工艺路线B0002','钻机001','测试使用',1,0,1,'2023-04-04 15:00:09','2023-04-11 22:37:47',1);
INSERT INTO `t_route` VALUES (3,'A0001','工艺路线A0001','钻机002',NULL,1,0,1,'2023-04-05 01:57:50','2023-04-12 02:01:15',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与工序关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_process`
--

LOCK TABLES `t_route_and_process` WRITE;
/*!40000 ALTER TABLE `t_route_and_process` DISABLE KEYS */;
INSERT INTO `t_route_and_process` VALUES (5,2,13,3,'1','#00AEF3',80,'0',4,1,0,1,'2023-04-05 02:41:06','2023-04-14 02:42:32',1);
INSERT INTO `t_route_and_process` VALUES (9,3,13,3,'1','#F31800',840,'0',NULL,0,0,1,'2023-04-07 02:40:29','2023-04-11 22:36:51',1);
INSERT INTO `t_route_and_process` VALUES (10,3,21,1,'0','#00AEF3',960,'0',NULL,0,0,1,'2023-04-07 03:30:26','2023-04-11 22:36:40',1);
INSERT INTO `t_route_and_process` VALUES (11,2,22,2,'0','#00AEF3',40,'0',3,1,0,1,'2023-04-10 22:58:49','2023-04-18 01:15:17',1);
INSERT INTO `t_route_and_process` VALUES (12,2,21,1,'0','#0A1C23',60,'0',2,1,0,1,'2023-04-10 22:59:09','2023-04-12 03:59:09',1);
INSERT INTO `t_route_and_process` VALUES (13,3,21,1,'1','#444C4F',840,'0',NULL,1,0,1,'2023-04-12 02:01:11',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与产品大类关系表';
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
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COMMENT='工序与工作站关系表';
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
) ENGINE=InnoDB AUTO_INCREMENT=76 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产任务';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_task`
--

LOCK TABLES `t_task` WRITE;
/*!40000 ALTER TABLE `t_task` DISABLE KEYS */;
INSERT INTO `t_task` VALUES (11,'1112111',0,'qqq',12,'1','1','1',1,'钻机','1',NULL,'钻孔','12',NULL,'item01',NULL,1,NULL,'pcs',120,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-06 00:00:00',8,'2023-04-08 00:00:00',NULL,'COMMIT','#00AEF3','N',0,1,1,'2023-03-01 09:19:18','2023-04-12 01:01:14',1,'0');
INSERT INTO `t_task` VALUES (13,'dd1111',0,'dd112',12,'1','1','1',1,'钻机','1',NULL,'钻孔','12',NULL,'item02',NULL,1,NULL,'pcs',120,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-06 00:00:00',3,'2023-04-08 00:00:00',NULL,'DRAFT','#00AEF3','N',0,1,1,'2023-03-01 13:31:13','2023-04-10 05:26:17',1,'0');
INSERT INTO `t_task` VALUES (14,'string',0,'string',16,'string','string','string',0,'钻机','string',0,'钻孔','string',0,'item03','string',1,'string','pcs',120,0,0,0,0,0,'string','string','2023-04-10 02:00:00',2,'2023-04-10 18:00:00','2023-04-06 03:10:47','DRAFT','#00AEF3','N',0,1,1,'2023-04-05 23:11:43','2023-04-16 23:15:59',1,'0,11');
INSERT INTO `t_task` VALUES (15,'string',0,'string',16,'string','string','string',0,'钻机','string',0,'钻孔','string',0,'item04','string',1,'string','pcs',120,0,0,0,0,0,'string','string','2023-04-10 19:00:00',2,'2023-04-11 11:00:00','2023-04-06 03:10:47','DRAFT','#00AEF3','N',0,1,1,'2023-04-05 23:12:00','2023-04-17 02:04:27',1,'0,13');
INSERT INTO `t_task` VALUES (24,'mock1',0,'mock1',18,NULL,'测试001',NULL,217,'钻机01-0003','drill01-0003',21,'钻孔',NULL,NULL,'item05',NULL,1,NULL,NULL,120,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-07 16:00:00',5,'2023-04-09 08:00:00',NULL,'DRAFT','#533737','0',0,1,1,'2023-04-07 04:55:56',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (28,'钻孔1',0,'钻孔1',18,NULL,'测试001',NULL,217,'钻机01-0003','drill01-0003',22,'钻孔','测试001',36,'item07',NULL,22,NULL,NULL,120,NULL,NULL,NULL,NULL,0,'string','string','2023-04-10 08:00:00',3,'2023-04-10 16:00:00','2023-04-10 00:39:04','DRAFT','#DD1313','0',0,1,1,'2023-04-09 20:43:49','2023-04-09 20:44:04',1,'0');
INSERT INTO `t_task` VALUES (29,'拆板1',0,'拆板1',18,NULL,NULL,NULL,219,'拆板机01-0001','unpin01-0001',13,'拆板',NULL,NULL,'item08',NULL,1,NULL,NULL,120,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-10 08:00:00',3,'2023-04-10 16:00:00','2023-04-10 00:39:04','DRAFT','#604242',NULL,0,1,1,'2023-04-09 20:44:57',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (30,'叠板',0,'22',19,NULL,'PCB单层','01',218,'叠板机01-0001','pin01-0001',21,'叠板','MO202304050002',36,'item09','IF2023040500002',1,'DDD XXXX DD','pcs',120,NULL,NULL,NULL,NULL,1,'string','string','2023-04-09 19:00:00',3,'2023-04-10 19:00:00','2023-04-10 00:39:04','DRAFT','#0079F3','0',0,1,1,'2023-04-10 05:15:53','2023-04-14 05:08:17',1,'0');
INSERT INTO `t_task` VALUES (31,'叠板',0,'222',19,NULL,'PCB单层','01',218,'叠板机01-0001','pin01-0001',21,'叠板','MO202304050002',36,'item10','IF2023040500002',1,'DDD XXXX DD','pcs',120,NULL,NULL,NULL,NULL,1,'string','string','2023-04-10 03:00:00',3,'2023-04-11 03:00:00','2023-04-10 00:39:04','DRAFT','#424998','0',0,1,1,'2023-04-10 05:22:34','2023-04-14 03:23:05',1,'0');
INSERT INTO `t_task` VALUES (34,'钻孔',0,'zuankong002',19,NULL,NULL,NULL,212,'钻机01-0001','drill01-0001',22,'钻孔',NULL,NULL,NULL,NULL,1,NULL,NULL,2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-10 03:00:00',1,'2023-04-10 11:00:00','2023-04-10 00:39:04','DRAFT','#FF0000',NULL,0,1,1,'2023-04-10 21:20:55','2023-04-17 02:07:36',1,'0');
INSERT INTO `t_task` VALUES (35,'拆板',0,'unpin',19,NULL,NULL,NULL,219,'拆板机01-0001','unpin01-0001',13,'拆板',NULL,NULL,NULL,NULL,1,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-11 08:00:00',1,'2023-04-11 16:00:00','2023-04-10 00:39:04','DRAFT','#00FF48',NULL,0,1,1,'2023-04-10 21:21:29',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (36,'钻孔',0,'drill001',20,NULL,NULL,NULL,217,'钻机01-0003','drill01-0003',22,'钻孔','drill',NULL,NULL,NULL,1,NULL,NULL,120,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-12 11:00:00',1,'2023-04-12 19:00:00','2023-04-10 00:39:04','DRAFT','#FF0000',NULL,0,1,1,'2023-04-10 21:29:16','2023-04-17 02:05:51',1,'0');
INSERT INTO `t_task` VALUES (38,'1',0,'1',21,NULL,'PCB单层','22',212,'钻机01-0001','drill01-0001',21,'叠板','pin',36,'PCB单层板','IF2023040500002',22,'DDD XXXX DD','Panel',1,NULL,NULL,NULL,NULL,3,'string','string','2023-04-12 00:00:00',1,'2023-04-12 08:00:00',NULL,'DRAFT','#56AED0','0',0,1,1,'2023-04-12 03:22:05','2023-04-18 01:24:55',1,'0');
INSERT INTO `t_task` VALUES (40,'钻孔1',0,'钻孔1',21,NULL,NULL,NULL,212,'钻机01-0001','drill01-0001',22,'钻孔','drill',NULL,NULL,NULL,1,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-12 08:00:00',1,'2023-04-12 16:00:00',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-12 04:41:17','2023-04-18 01:24:58',1,'0');
INSERT INTO `t_task` VALUES (56,'22',0,'22',23,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:17:56',1,'2023-04-18 14:17:56',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:18:21',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (57,'11',0,'11',23,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:19:08',1,'2023-04-18 14:19:07',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:19:31',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (58,'11',0,'11',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:19:45',1,'2023-04-18 14:19:44',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:20:08',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (59,'2222222',0,'22222222',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:27:20',1,'2023-04-18 14:27:20',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:27:46',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (60,'12',0,'12',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:33:52',1,'2023-04-18 14:33:52',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:34:20',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (61,'222221111',0,'22221111',26,NULL,NULL,NULL,218,'叠板机01-0001','pin01-0001',21,'叠板','pin',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:34:34',1,'2023-04-18 14:34:34',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:35:06',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (62,'11',0,'11',26,'MO202304020001','PCB单层','22',218,'叠板机01-0001','pin01-0001',21,'PCB单层','MO202304020001',36,'Panel单层01','IF2023040500002',NULL,'11','Panel',2,NULL,NULL,NULL,NULL,1,'string','string','2023-04-18 05:36:19',50,'2023-04-22 17:36:19',NULL,'DRAFT','11','0',0,1,1,'2023-04-18 01:36:43',NULL,NULL,'0');
INSERT INTO `t_task` VALUES (64,'#0A1C23',0,'#0A1C23',25,'MO202304030001','PCB单层',NULL,218,'叠板机01-0001','pin01-0001',21,'PCB单层','MO202304030001',36,'Panel单层02','IF2023040500002',22,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-04-18 05:54:51',1,'2023-04-18 14:54:51',NULL,'DRAFT','#0A1C23',NULL,0,1,1,'2023-04-18 01:55:09',NULL,NULL,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (33,'Panel','单层Panel','Y',NULL,NULL,NULL,0,1,1,'2023-04-18 22:11:38','2023-04-18 22:12:26',1);
INSERT INTO `t_unit_measure` VALUES (34,'叠板2层','叠板2层','Y',NULL,NULL,NULL,0,1,1,'2023-04-18 22:13:06',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COMMENT='仓库表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_warehouse`
--

LOCK TABLES `t_warehouse` WRITE;
/*!40000 ALTER TABLE `t_warehouse` DISABLE KEYS */;
INSERT INTO `t_warehouse` VALUES (2,'WareHouse2','WareHouse2','string','admin','WareHouse2',0,1,1,'2023-04-03 14:03:07','2023-04-04 02:17:25',1);
INSERT INTO `t_warehouse` VALUES (14,'WareHouse','WareHouse',NULL,NULL,NULL,0,1,1,'2023-04-18 23:36:14',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产工单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_work_order`
--

LOCK TABLES `t_work_order` WRITE;
/*!40000 ALTER TABLE `t_work_order` DISABLE KEYS */;
INSERT INTO `t_work_order` VALUES (21,'PCB单层',0,'MO202304050003','1','',36,'PCB单层板','IF2023040500002',22,'22','DDD XXXX DD','Panel',2,2,3,2,3,'string','string','2023-02-28 10:10:19','DRAFT',0,1,1,'2023-04-11 02:55:53',NULL,NULL,'0');
INSERT INTO `t_work_order` VALUES (25,'PCB单层',0,'MO202304030001','1','',36,'Panel单层02','IF2023040500002',22,'22','11','Panel',144,144,NULL,NULL,NULL,NULL,NULL,'2023-02-28 10:10:19','DRAFT',0,1,1,'2023-04-18 01:20:02',NULL,NULL,'0');
INSERT INTO `t_work_order` VALUES (26,'PCB单层',0,'MO202304020001','1','',36,'Panel单层01','IF2023040500002',22,'22','11','Panel',120,2,3,2,1,'string','string','2023-02-28 10:10:19','DRAFT',0,1,1,'2023-04-18 01:20:35',NULL,NULL,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workshop`
--

LOCK TABLES `t_workshop` WRITE;
/*!40000 ALTER TABLE `t_workshop` DISABLE KEYS */;
INSERT INTO `t_workshop` VALUES (9,'shop02','车间02（测试）','张三','此车间的备注信息（选填）',0,2,1,'2023-04-03 22:03:36','2023-04-13 01:21:56',1);
INSERT INTO `t_workshop` VALUES (11,'shop01','车间01（测试）','张三','此车间的备注信息（选填）',0,1,1,'2023-04-10 23:31:47',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=231 DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
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

-- Dump completed on 2023-04-19  5:34:27
