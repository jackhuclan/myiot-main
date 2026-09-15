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
) ENGINE=InnoDB AUTO_INCREMENT=1165 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu`
--

LOCK TABLES `sys_menu` WRITE;
/*!40000 ALTER TABLE `sys_menu` DISABLE KEYS */;
INSERT INTO `sys_menu` VALUES (1,'DRILLAMIN','系统管理',0,'system','system','#',1,NULL,NULL,100,0,1,0,'2022-09-26 14:56:09','2023-04-01 03:13:43',1);
INSERT INTO `sys_menu` VALUES (2,'DRILLAMIN','用户管理',1,'user','user','system/user/index',2,'system:user:list',NULL,1,0,1,0,'2022-09-26 15:09:01','2022-12-26 16:53:55',1);
INSERT INTO `sys_menu` VALUES (3,'DRILLAMIN','角色管理',1,'peoples','role','system/role/index',2,'system:role:list',NULL,2,0,1,0,'2022-09-26 15:10:37','2022-12-26 16:54:36',1);
INSERT INTO `sys_menu` VALUES (4,'DRILLAMIN','菜单管理',1,'tree-table','menu','system/menu/index',2,'system:menu:list',NULL,4,0,1,NULL,'2022-09-26 15:11:14','2023-03-01 13:53:58',1);
INSERT INTO `sys_menu` VALUES (5,'DRILLAMIN','部门管理',1,'tree','dept','system/dept/index',2,'system:dept:list',NULL,5,0,1,NULL,'2022-09-26 15:12:26','2023-03-01 13:57:19',1);
INSERT INTO `sys_menu` VALUES (6,'DRILLAMIN','岗位管理',1,'post','post','system/post/index',2,'system:post:list',NULL,6,0,1,NULL,'2022-09-26 15:13:20','2023-03-01 13:57:26',1);
INSERT INTO `sys_menu` VALUES (7,'CRM','系统管理',0,'system',NULL,'',1,NULL,NULL,1,0,1,0,'2022-09-26 14:56:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (8,'DRILLAMIN','应用管理',1,'system','app','system/app/index',1,NULL,NULL,7,0,1,0,'2022-09-26 14:56:09','2023-03-01 14:07:32',1);
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
INSERT INTO `sys_menu` VALUES (1025,'DRILLAMIN','物料管理',0,'build','material','#',1,NULL,NULL,3,0,1,1,'2023-02-20 23:01:59','2023-03-01 13:18:59',1);
INSERT INTO `sys_menu` VALUES (1026,'DRILLAMIN','设备管理',0,'phone','device','#',1,NULL,NULL,4,0,1,1,'2023-02-20 23:02:35','2023-03-01 14:09:09',1);
INSERT INTO `sys_menu` VALUES (1027,'DRILLAMIN','告警管理',0,'email','alarm','#',1,NULL,NULL,5,0,1,1,'2023-02-20 23:03:32','2023-03-01 14:09:18',1);
INSERT INTO `sys_menu` VALUES (1029,'DRILLAMIN','排产管理',1024,'#','plan','produce/plan/index',2,'produce:plan:list',NULL,2,0,1,1,'2023-02-20 23:06:58','2023-02-21 01:51:15',1);
INSERT INTO `sys_menu` VALUES (1030,'DRILLAMIN','板料管理',1024,'#','board','produce/board/index',2,'produce:board:list',NULL,1,0,1,1,'2023-02-20 23:09:15','2023-02-21 02:46:42',1);
INSERT INTO `sys_menu` VALUES (1031,'DRILLAMIN','生产记录',1024,'#','record','produce/record/index',2,'produce:record:list',NULL,3,0,1,1,'2023-02-20 23:11:13','2023-02-21 02:52:12',1);
INSERT INTO `sys_menu` VALUES (1032,'DRILLAMIN','刀盘管理',1025,'#','cutter','material/cutter/index',2,'material:cutter:list',NULL,1,0,1,1,'2023-02-20 23:15:40','2023-03-28 04:35:00',1);
INSERT INTO `sys_menu` VALUES (1033,'DRILLAMIN','设备类型',1026,'#','equipmentType','device/equipmentType/index',2,'device:equipmentType:list',NULL,1,0,1,1,'2023-02-20 23:18:29','2023-04-02 21:21:28',1);
INSERT INTO `sys_menu` VALUES (1034,'DRILLAMIN','配方管理',1024,'#','recipe','device/recipe/index',2,'device:recipe:list',NULL,6,0,1,1,'2023-02-20 23:20:37','2023-03-27 01:24:29',1);
INSERT INTO `sys_menu` VALUES (1035,'DRILLAMIN','设备列表',1026,'#','equipment','device/equipment/index',2,'device:equipment:list',NULL,3,0,1,1,'2023-02-20 23:22:03','2023-02-21 02:49:47',1);
INSERT INTO `sys_menu` VALUES (1036,'DRILLAMIN','刀具参数',1025,'#','config','material/config/index',2,'material:config:list',NULL,2,0,1,1,'2023-02-20 23:23:49','2023-03-28 05:30:47',1);
INSERT INTO `sys_menu` VALUES (1037,'DRILLAMIN','调度记录',1026,'#','schedulement','device/schedulement/index',2,'device:schedulement:edit',NULL,5,0,1,1,'2023-02-20 23:25:16','2023-02-21 02:50:22',1);
INSERT INTO `sys_menu` VALUES (1038,'DRILLAMIN','事件管理',1027,'#','event','alarm/event/index',2,'alarm:event:list',NULL,1,0,1,1,'2023-02-20 23:27:03','2023-02-23 22:04:19',1);
INSERT INTO `sys_menu` VALUES (1039,'DRILLAMIN','告警记录',1027,'#','warn','alarm/warn/index',2,'alarm:warn:list',NULL,3,0,1,1,'2023-02-20 23:28:17','2023-03-23 22:13:35',1);
INSERT INTO `sys_menu` VALUES (1043,'DRILLAMIN','产品修改',1033,NULL,'','#',3,'device:product:edit',NULL,3,0,1,1,'2023-02-21 20:32:06','2023-02-21 22:09:43',1);
INSERT INTO `sys_menu` VALUES (1044,'DRILLAMIN','产品删除',1033,NULL,NULL,'#',3,'device:product:del',NULL,4,0,1,1,'2023-02-21 20:32:58','2023-02-21 22:09:52',1);
INSERT INTO `sys_menu` VALUES (1045,'DRILLAMIN','产品查询',1033,NULL,NULL,'#',3,'device:product:list',NULL,1,0,1,1,'2023-02-21 20:33:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1046,'DRILLAMIN','配方查询',1034,NULL,NULL,'#',3,'device:recipe:list',NULL,1,0,1,1,'2023-02-21 22:13:46',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1047,'DRILLAMIN','配方新增',1034,NULL,NULL,'#',3,'device:recipe:add',NULL,2,0,1,1,'2023-02-21 22:14:27','2023-02-21 22:14:35',1);
INSERT INTO `sys_menu` VALUES (1048,'DRILLAMIN','配方修改',1034,NULL,NULL,'#',3,'device:recipe:edit',NULL,3,0,1,1,'2023-02-21 22:15:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1049,'DRILLAMIN','配方删除',1034,NULL,NULL,'#',3,'device:recipe:remove',NULL,4,0,1,1,'2023-02-21 22:15:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1052,'DRILLAMIN','产品重置',1033,NULL,NULL,'#',3,'device:product:reset',NULL,5,0,1,1,'2023-02-21 22:22:09','2023-02-21 22:24:34',1);
INSERT INTO `sys_menu` VALUES (1053,'DRILLAMIN','配方重置',1034,NULL,NULL,'#',3,'device:recipe:reset',NULL,5,0,1,1,'2023-02-21 22:25:29',NULL,NULL);
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
INSERT INTO `sys_menu` VALUES (1077,'DRILLAMIN','排产查询',1029,NULL,NULL,'#',3,'produce:plan:list',NULL,1,0,1,1,'2023-02-22 04:23:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1078,'DRILLAMIN','排产新增',1029,NULL,NULL,'#',3,'produce:plan:add',NULL,2,0,1,1,'2023-02-22 04:23:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1079,'DRILLAMIN','排产修改',1029,NULL,NULL,'#',3,'produce:plan:edit',NULL,3,0,1,1,'2023-02-22 04:24:34',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1080,'DRILLAMIN','排产删除',1029,NULL,NULL,'#',3,'produce:plan:remove',NULL,4,0,1,1,'2023-02-22 04:25:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1081,'DRILLAMIN','排产重置',1029,NULL,NULL,'#',3,'produce:plan:reset',NULL,5,0,1,1,'2023-02-22 04:26:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1082,'DRILLAMIN','生产查询',1031,NULL,NULL,'#',3,'produce:record:list',NULL,1,0,1,1,'2023-02-22 04:28:02',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1083,'DRILLAMIN','生产新增',1031,NULL,NULL,'#',3,'produce:record:add',NULL,2,0,1,1,'2023-02-22 04:28:41',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1084,'DRILLAMIN','生产修改',1031,NULL,NULL,'#',3,'produce:record:edit',NULL,3,0,1,1,'2023-02-22 04:29:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1085,'DRILLAMIN','生产删除',1031,NULL,NULL,'#',3,'produce:record:remove',NULL,4,0,1,1,'2023-02-22 04:29:43',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1086,'DRILLAMIN','生产重置',1031,NULL,NULL,'#',3,'produce:record:reset',NULL,5,0,1,1,'2023-02-22 04:30:12',NULL,NULL);
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
INSERT INTO `sys_menu` VALUES (1122,'DRILLAMIN','生产工单',1024,'#','workOrder','produce/workOrder/index',2,'produce:workorder:list',NULL,4,0,1,1,'2023-03-01 11:03:28','2023-03-01 11:20:52',1);
INSERT INTO `sys_menu` VALUES (1123,'DRILLAMIN','查询',1122,NULL,NULL,'#',3,'produce:workorder:list',NULL,1,0,1,1,'2023-03-01 11:04:30','2023-03-01 11:05:22',1);
INSERT INTO `sys_menu` VALUES (1124,'DRILLAMIN','新增',1122,NULL,NULL,'#',3,'produce:workorder:add',NULL,2,0,1,1,'2023-03-01 11:06:03',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1125,'DRILLAMIN','修改',1122,NULL,NULL,'#',3,'produce:workorder:edit',NULL,3,0,1,1,'2023-03-01 11:06:41',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1126,'DRILLAMIN','删除',1122,NULL,NULL,'#',3,'produce:workorder:remove',NULL,4,0,1,1,'2023-03-01 11:07:08','2023-03-01 11:07:17',1);
INSERT INTO `sys_menu` VALUES (1127,'DRILLAMIN','重置',1122,NULL,NULL,'#',3,'produce:workorder:reset',NULL,5,0,1,1,'2023-03-01 11:07:46',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1128,'DRILLAMIN','工序管理',1024,'#','process','produce/process/index',2,'produce:process:list',NULL,5,0,1,1,'2023-03-01 11:37:07','2023-03-01 11:40:04',1);
INSERT INTO `sys_menu` VALUES (1129,'DRILLAMIN','查询',1128,'#',NULL,'#',3,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:42:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1130,'DRILLAMIN','新增',1128,'#',NULL,'#',3,'produce:process:add',NULL,2,0,1,1,'2023-03-01 11:43:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1131,'DRILLAMIN','修改',1128,'#',NULL,'#',3,'produce:process:edit',NULL,3,0,1,1,'2023-03-01 11:43:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1132,'DRILLAMIN','删除',1128,'#',NULL,'#',3,'produce:process:remove',NULL,4,0,1,1,'2023-03-01 11:43:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1133,'DRILLAMIN','重置',1128,'#',NULL,'#',3,'produce:process:reset',NULL,5,0,1,1,'2023-03-01 11:44:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1134,'DRILLAMIN','字典管理',1,'dict','dict','system/dict/index',2,'system:dict:list',NULL,3,0,1,1,'2023-03-01 13:53:42','2023-04-01 02:50:07',1);
INSERT INTO `sys_menu` VALUES (1136,'DRILLAMIN','告警设置',1027,'#','setting','alarm/setting/index',2,'alarm:setting:index',NULL,2,0,1,1,'2023-03-23 22:13:23','2023-03-23 22:13:48',1);
INSERT INTO `sys_menu` VALUES (1137,'DRILLAMIN','设置列表',1136,'#',NULL,'#',3,'alarm:setting:list',NULL,1,0,1,1,'2023-03-23 22:15:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1138,'DRILLAMIN','设置添加',1136,'#',NULL,'#',3,'alarm:setting:add',NULL,2,0,1,1,'2023-03-23 22:16:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1139,'DRILLAMIN','设置修改',1136,'#',NULL,'#',3,'alarm:setting:edit',NULL,3,0,1,1,'2023-03-23 22:16:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1140,'DRILLAMIN','设置删除',1136,'#',NULL,'#',3,'alarm:setting:remove',NULL,4,0,1,1,'2023-03-23 22:17:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1147,'DRILLAMIN','排刀计划',1024,'#','drillCutter','produce/drillCutter/index',1,'produce:drillCutter:list',NULL,1,0,1,1,'2023-03-28 04:56:52','2023-03-29 02:33:47',1);
INSERT INTO `sys_menu` VALUES (1148,'DRILLAMIN','主数据',0,'component','masterData','#',1,NULL,NULL,2,0,1,1,'2023-03-29 22:00:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1149,'DRILLAMIN','计量单位',1148,'#','unitMeasure','masterData/unitMeasure/index',2,'masterData:unitMeasure:list',NULL,1,0,1,1,'2023-03-29 22:05:55',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1150,'DRILLAMIN','客户管理',1148,'#','client','masterData/client/index',2,'masterData:client:list',NULL,1,0,1,1,'2023-03-29 22:07:26','2023-03-29 22:09:14',1);
INSERT INTO `sys_menu` VALUES (1151,'DRILLAMIN','供应商管理',1148,'#','vendor','masterData/vendor/index',2,'masterData:vendor:list',NULL,1,0,1,1,'2023-03-29 22:09:03','2023-03-31 04:21:06',1);
INSERT INTO `sys_menu` VALUES (1152,'DRILLAMIN','车间管理',1148,'#','workshop','masterData/workshop/index',2,'masterData:workshop:list',NULL,1,0,1,1,'2023-03-29 22:10:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1153,'DRILLAMIN','工作站',1148,'#','workstation','masterData/workstation/index',2,'masterData:workstation:index',NULL,1,0,1,1,'2023-03-29 22:11:24','2023-03-31 04:13:23',1);
INSERT INTO `sys_menu` VALUES (1154,'DRILLAMIN','物料产品分类',1148,'#','itemType','masterData/itemType/index',2,'masterData:itemType:list',NULL,1,0,1,1,'2023-03-29 22:12:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1155,'DRILLAMIN','物料产品管理',1148,'#','item','masterData/item/index',2,'masterData:item:list',NULL,1,0,1,1,'2023-03-29 22:13:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1156,'DRILLAMIN','产品大类',1148,'#','productCategory','masterData/productCategory/index',2,'masterData:productCategory:list',NULL,6,0,1,1,'2023-04-01 02:41:29','2023-04-01 02:42:23',1);
INSERT INTO `sys_menu` VALUES (1157,'DRILLAMIN','工序流程',1024,'#','route','produce/route/index',2,'produce:route:list',NULL,5,0,1,1,'2023-04-01 02:46:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'system','wm','#',1,NULL,NULL,50,0,1,1,'2023-04-01 02:50:34','2023-04-01 03:13:54',1);
INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','仓库设置',1158,'#','warehouse','warehouse/warehouse/index',2,'warehouse:warehouse:list',NULL,1,0,1,1,'2023-04-01 02:53:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1160,'DRILLAMIN','库存现有量',1158,'#','wmstock','warehouse/wmstock/index',2,'warehouse:wmstock:list',NULL,2,0,1,1,'2023-04-01 02:54:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1161,'DRILLAMIN','钻带参数',1025,'#','drillFile','material/drillFile/index',2,'material:drillFile:list',NULL,3,0,1,1,'2023-04-01 03:20:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1162,'DRILLAMIN','ATP文件',1025,'#','atpFile','material/atpFile/index',2,'material:atpFile:list',NULL,4,0,1,1,'2023-04-01 03:21:21',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=132 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu_auth`
--

LOCK TABLES `sys_menu_auth` WRITE;
/*!40000 ALTER TABLE `sys_menu_auth` DISABLE KEYS */;
INSERT INTO `sys_menu_auth` VALUES (106,1,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (107,2,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (108,1000,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (109,1001,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (110,1002,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (111,1003,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (112,1004,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (113,1005,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (114,1006,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (115,3,7,1,0,'2022-12-05 16:50:54');
INSERT INTO `sys_menu_auth` VALUES (116,1007,7,1,0,'2022-12-05 16:50:55');
INSERT INTO `sys_menu_auth` VALUES (117,1008,7,1,0,'2022-12-05 16:50:55');
INSERT INTO `sys_menu_auth` VALUES (118,1009,7,1,0,'2022-12-05 16:50:55');
INSERT INTO `sys_menu_auth` VALUES (119,1010,7,1,0,'2022-12-05 16:50:55');
INSERT INTO `sys_menu_auth` VALUES (120,1011,7,1,0,'2022-12-05 16:50:55');
INSERT INTO `sys_menu_auth` VALUES (121,1,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (122,5,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (123,1016,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (124,1017,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (125,1018,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (126,1019,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (127,6,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (128,1020,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (129,1021,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (130,1022,2,1,1,'2022-12-27 15:44:54');
INSERT INTO `sys_menu_auth` VALUES (131,1023,2,1,1,'2022-12-27 15:44:54');
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role`
--

LOCK TABLES `sys_role` WRITE;
/*!40000 ALTER TABLE `sys_role` DISABLE KEYS */;
INSERT INTO `sys_role` VALUES (1,'超级管理员',1,0,1,0,'2022-11-08 11:26:24',NULL,NULL);
INSERT INTO `sys_role` VALUES (2,'普通角色',2,0,1,0,'2022-11-17 08:58:18','2023-03-26 20:59:09',1);
INSERT INTO `sys_role` VALUES (3,'大理石',3,0,1,1,'2023-02-23 02:01:07','2023-03-23 22:10:15',1);
INSERT INTO `sys_role` VALUES (5,'2',4,0,1,1,'2023-02-23 04:00:51',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
INSERT INTO `sys_user` VALUES (30,'小昭','e1b161f8020fc0190e98e8864cae55e7','f4f02efeedb4449d82da200ae48e1992','zzz',9,4,0,NULL,NULL,'15614151678','15614151678@163.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-03-23 02:55:24',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
/*!40000 ALTER TABLE `sys_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_alarm`
--

DROP TABLE IF EXISTS `t_alarm`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_alarm` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm`
--

LOCK TABLES `t_alarm` WRITE;
/*!40000 ALTER TABLE `t_alarm` DISABLE KEYS */;
INSERT INTO `t_alarm` VALUES (25,'2023-03-29 09:13:48','1','1',3,NULL,0,1,1,'2023-03-28 21:14:26',NULL,NULL,0,'1');
INSERT INTO `t_alarm` VALUES (26,'2023-03-29 09:14:00','2','2',1,NULL,0,1,1,'2023-03-28 21:14:38',NULL,NULL,0,'2');
INSERT INTO `t_alarm` VALUES (27,'2023-03-29 09:14:05','3','3',1,NULL,0,1,1,'2023-03-28 21:14:43','2023-03-28 21:14:56',1,0,'3');
INSERT INTO `t_alarm` VALUES (28,'2023-03-29 09:14:59','22','22',1,NULL,0,1,1,'2023-03-28 21:15:37',NULL,NULL,0,'3');
INSERT INTO `t_alarm` VALUES (29,'2023-03-29 13:31:53','11','11',2,NULL,0,1,1,'2023-03-29 01:32:32','2023-03-29 01:41:39',1,0,'');
/*!40000 ALTER TABLE `t_alarm` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_alarm_setting`
--

DROP TABLE IF EXISTS `t_alarm_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_alarm_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
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
  `notify_ways` varchar(1000) CHARACTER SET utf8 DEFAULT NULL COMMENT '通知方式',
  `event_rules` varchar(2000) CHARACTER SET utf8 DEFAULT NULL COMMENT '触发规则',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm_setting`
--

LOCK TABLES `t_alarm_setting` WRITE;
/*!40000 ALTER TABLE `t_alarm_setting` DISABLE KEYS */;
INSERT INTO `t_alarm_setting` VALUES (12,'AGV电量水平低于30%',0,'LOWER_THAN_30','AGV电量水平低于30%',1,0,0,1,'2023-02-27 16:26:42','2023-03-29 01:41:52',1,65,'BIG_SCREEN_WARNING','AGV电量水平低于30%',NULL);
INSERT INTO `t_alarm_setting` VALUES (14,'AGV电量水平低于20%',0,'LOWER_THAN_20','AGV电量水平低于20%',2,0,1,1,'2023-02-27 16:26:42','2023-03-28 21:03:52',1,65,'LIGHT_WARNING',' AGV电量水平低于20%',NULL);
INSERT INTO `t_alarm_setting` VALUES (15,'AGV电量水平低于10%',0,'LOWER_THAN_10','AGV电量水平低于10%',3,0,1,1,'2023-02-27 16:26:42','2023-03-28 21:03:58',1,65,'LIGHT_WARNING','AGV电量水平低于10%',NULL);
/*!40000 ALTER TABLE `t_alarm_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_board_trace`
--

DROP TABLE IF EXISTS `t_board_trace`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_board_trace` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '板料编号',
  `batch_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '批次号',
  `board_location` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '板料位置',
  `finish_status` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '完成状态',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='板料追踪';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_board_trace`
--

LOCK TABLES `t_board_trace` WRITE;
/*!40000 ALTER TABLE `t_board_trace` DISABLE KEYS */;
INSERT INTO `t_board_trace` VALUES (13,'222','22','2',NULL,1,0,1,'2023-02-28 13:22:25',NULL,NULL);
INSERT INTO `t_board_trace` VALUES (14,'板料1','板料1','3',NULL,1,0,1,'2023-03-24 02:50:27',NULL,NULL);
/*!40000 ALTER TABLE `t_board_trace` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_client`
--

DROP TABLE IF EXISTS `t_client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_client` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '客户ID',
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COMMENT='客户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_client`
--

LOCK TABLES `t_client` WRITE;
/*!40000 ALTER TABLE `t_client` DISABLE KEYS */;
INSERT INTO `t_client` VALUES (1,'string','string','string',0,1,1,'2023-03-30 13:43:09',NULL,NULL);
INSERT INTO `t_client` VALUES (2,'string','string','string',0,1,1,'2023-03-31 03:50:58',NULL,NULL);
INSERT INTO `t_client` VALUES (3,'string','string','string',0,1,1,'2023-03-31 03:53:54',NULL,NULL);
INSERT INTO `t_client` VALUES (5,'11','11','11',0,1,1,'2023-03-31 03:54:11',NULL,NULL);
INSERT INTO `t_client` VALUES (6,'11','111','11',0,1,1,'2023-03-31 05:06:44',NULL,NULL);
/*!40000 ALTER TABLE `t_client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter`
--

DROP TABLE IF EXISTS `t_cutter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '名称',
  `parent_id` int(11) DEFAULT NULL COMMENT '父对象Id(0表示是根对象)',
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
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='刀具';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter`
--

LOCK TABLES `t_cutter` WRITE;
/*!40000 ALTER TABLE `t_cutter` DISABLE KEYS */;
INSERT INTO `t_cutter` VALUES (11,'aa',0,'xx',NULL,NULL,NULL,'bb','1',1,1,'2022-02-02 00:00:00',NULL,NULL,0,0,1,'2023-02-27 15:18:41','2023-03-23 04:46:31',1,NULL);
INSERT INTO `t_cutter` VALUES (12,'cc',0,'xx',NULL,NULL,NULL,'ddd','1',1,1,'2022-02-02 00:00:00',NULL,NULL,0,0,1,'2023-02-27 15:18:50','2023-03-23 22:09:08',1,NULL);
INSERT INTO `t_cutter` VALUES (13,'ddsaa',0,'xx',11,11,1,'afasfd','1',12,1,'2022-02-02 00:00:00',NULL,NULL,0,1,1,'2023-02-27 15:18:54',NULL,NULL,NULL);
INSERT INTO `t_cutter` VALUES (15,'xx111',0,'xx111',12,11,1,'xx111','1',12,1,'2023-02-28 08:15:39','111',NULL,0,0,1,'2023-02-27 16:15:39','2023-03-29 03:31:20',1,NULL);
INSERT INTO `t_cutter` VALUES (18,'string1',0,'string1',0,0,0,'string','string',0,0,'2023-03-29 02:53:56','string','string',0,1,1,'2023-03-29 02:53:56','2023-03-29 03:05:41',1,NULL);
INSERT INTO `t_cutter` VALUES (19,'string33333333',0,'string3333333333333',2,3,3,'string','string',0,0,'2023-03-22 07:30:53','string','string',0,0,1,'2023-03-29 03:06:02','2023-03-29 03:31:32',1,NULL);
INSERT INTO `t_cutter` VALUES (20,'11',0,'11',1,1,1,'111','1111',111,1111,'2023-03-29 03:32:19','1111',NULL,0,1,1,'2023-03-29 03:32:19',NULL,NULL,NULL);
/*!40000 ALTER TABLE `t_cutter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter_config_detail`
--

DROP TABLE IF EXISTS `t_cutter_config_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter_config_detail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `master_id` int(11) DEFAULT NULL COMMENT '刀具主表Id',
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
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_detail`
--

LOCK TABLES `t_cutter_config_detail` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_detail` DISABLE KEYS */;
INSERT INTO `t_cutter_config_detail` VALUES (4,3,1.000,3.000,2.000,3.000,0.000,0,0,1,1,'2023-03-28 16:46:30','2023-03-29 23:28:52',1);
INSERT INTO `t_cutter_config_detail` VALUES (5,4,0.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:09:22',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (6,5,0.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:10:08',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (21,5,3.000,3.000,3.000,3.000,3.000,0,0,1,1,'2023-03-29 23:31:01','2023-03-29 23:31:31',1);
INSERT INTO `t_cutter_config_detail` VALUES (22,5,2.000,2.000,2.000,2.000,2.000,0,0,1,1,'2023-03-29 23:31:04','2023-03-29 23:31:12',1);
INSERT INTO `t_cutter_config_detail` VALUES (23,4,-1.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:31:51','2023-03-29 23:42:52',1);
INSERT INTO `t_cutter_config_detail` VALUES (24,3,-2.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:41:01','2023-03-30 01:15:02',1);
INSERT INTO `t_cutter_config_detail` VALUES (25,3,-3.000,0.000,0.000,0.000,0.000,0,0,1,1,'2023-03-29 23:45:04','2023-03-30 02:56:30',1);
/*!40000 ALTER TABLE `t_cutter_config_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter_config_master`
--

DROP TABLE IF EXISTS `t_cutter_config_master`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter_config_master` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `config_name` varchar(100) DEFAULT NULL COMMENT '配置名称',
  `config_desc` varchar(200) DEFAULT NULL COMMENT '配置描述',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数主表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_master`
--

LOCK TABLES `t_cutter_config_master` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_master` DISABLE KEYS */;
INSERT INTO `t_cutter_config_master` VALUES (3,'string2211','string2211',0,1,1,'2023-03-28 16:38:31','2023-03-29 03:53:02',1);
INSERT INTO `t_cutter_config_master` VALUES (6,'111','111222',0,1,1,'2023-03-30 01:02:59','2023-03-30 02:51:34',1);
INSERT INTO `t_cutter_config_master` VALUES (7,'11','11',0,1,1,'2023-03-30 02:51:40',NULL,NULL);
/*!40000 ALTER TABLE `t_cutter_config_master` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_cutter_plan`
--

DROP TABLE IF EXISTS `t_cutter_plan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_cutter_plan` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `disk_code` varchar(100) NOT NULL COMMENT '刀盘二维码',
  `atp` varchar(500) DEFAULT NULL COMMENT '刀盘参数文件路径',
  `item_code` varchar(100) DEFAULT NULL COMMENT '料号',
  `change_rule` tinyint(4) DEFAULT NULL COMMENT '换刀规则1按料号，2按加工板次，3按使用寿命',
  `rule_board_limit` int(11) DEFAULT NULL COMMENT '板次',
  `rule_age_limit` int(11) DEFAULT NULL COMMENT '使用寿命',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COMMENT='钻机排刀计划';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_plan`
--

LOCK TABLES `t_cutter_plan` WRITE;
/*!40000 ALTER TABLE `t_cutter_plan` DISABLE KEYS */;
INSERT INTO `t_cutter_plan` VALUES (1,'string','string','string',0,0,0,0,1,1,'2023-03-28 16:47:00',NULL,NULL);
INSERT INTO `t_cutter_plan` VALUES (2,'string','string','string',0,0,0,0,0,1,'2023-03-28 16:47:02','2023-03-29 02:36:55',1);
/*!40000 ALTER TABLE `t_cutter_plan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device`
--

DROP TABLE IF EXISTS `t_device`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `name` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `tag` varchar(1000) CHARACTER SET utf8 DEFAULT NULL COMMENT '设备标签, 类似a=b键值对',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
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
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
INSERT INTO `t_device` VALUES (11,'drill01','drill01',NULL,7,0,1,1,'2023-02-28 11:40:13','2023-03-22 03:19:19',1,8,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (13,'agv01','agv01',NULL,0,0,1,1,'2023-02-28 11:40:21','2023-03-27 02:03:03',1,8,0,'string','string',0,NULL,NULL);
INSERT INTO `t_device` VALUES (14,'MockAgv02','MockAgv02',NULL,7,0,0,1,'2023-03-03 00:33:58','2023-03-22 02:31:17',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (15,'MockDrill01','MockDrill01',NULL,8,0,1,1,'2023-03-03 00:34:24','2023-03-22 07:52:58',NULL,NULL,NULL,NULL,NULL,2,NULL,NULL);
INSERT INTO `t_device` VALUES (16,'P0001','P0001',NULL,8,0,1,1,'2023-03-03 01:32:19','2023-03-08 16:46:09',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (17,'MockAgv06','MockAgv06',NULL,8,0,2,1,'2023-03-07 10:20:48','2023-03-07 10:54:07',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (18,'MockAgv03','MockAgv03',NULL,8,0,1,1,'2023-03-07 10:21:29','2023-03-17 10:37:59',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (19,'MockAgv04','MockAgv04',NULL,0,0,1,1,'2023-03-07 17:09:12','2023-03-15 15:39:26',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (20,'P0002','P0002',NULL,0,0,1,1,'2023-03-07 20:19:06','2023-03-07 21:14:21',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (21,'MockAgv104','MockAgv104',NULL,0,0,0,1,'2023-03-09 10:15:32','2023-03-23 02:49:06',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (22,'MockAgv103','MockAgv103',NULL,0,0,1,1,'2023-03-09 10:15:32','2023-03-22 07:52:58',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (23,'MockDrill02','MockDrill02',NULL,0,0,1,1,'2023-03-13 13:54:18','2023-03-22 07:52:58',NULL,NULL,NULL,NULL,NULL,4,NULL,NULL);
INSERT INTO `t_device` VALUES (24,'MockDrill03','MockDrill03',NULL,0,0,1,1,'2023-03-14 08:48:10','2023-03-20 11:46:56',NULL,NULL,NULL,NULL,NULL,5,NULL,NULL);
INSERT INTO `t_device` VALUES (25,'MockAgv01','MockAgv01',NULL,0,0,1,1,'2023-03-14 08:51:18','2023-03-14 11:21:03',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (26,'MockDrill04','MockDrill04',NULL,0,0,1,1,'2023-03-14 09:22:53','2023-03-20 11:46:56',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (27,'MockDrill05','MockDrill05',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:03',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (28,'MockDrill06','MockDrill06',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:02',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (29,'Processed0001','Processed0001',NULL,0,0,1,1,'2023-03-14 11:19:38','2023-03-16 10:25:40',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (30,'Processed10001','Processed10001',NULL,0,0,1,1,'2023-03-14 15:50:43','2023-03-20 14:21:40',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (31,'vegaDrill001','vegaDrill001',NULL,0,0,1,1,'2023-03-14 16:18:04','2023-03-14 16:27:45',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (32,'MockAgv1003','MockAgv1003',NULL,0,0,1,1,'2023-03-14 16:28:34','2023-03-14 16:33:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (33,'vegaDrill0601','vegaDrill0601',NULL,0,0,1,1,'2023-03-14 16:28:35','2023-03-14 16:33:37',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (34,'MockAgv11103','MockAgv11103',NULL,0,0,1,1,'2023-03-14 16:52:45','2023-03-15 08:43:45',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (35,'MockTonyAgv001','MockTonyAgv001',NULL,0,0,1,1,'2023-03-15 09:41:29','2023-03-22 23:45:47',1,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (36,'MockSimpleAgv001','MockSimpleAgv001',NULL,0,0,1,1,'2023-03-15 10:12:33','2023-03-16 14:30:31',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (37,'MockSimpleAgv002','MockSimpleAgv002',NULL,0,0,1,1,'2023-03-15 10:24:25','2023-03-15 10:36:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (38,'Processed002','Processed002',NULL,0,0,1,1,'2023-03-15 10:24:26','2023-03-15 10:36:38',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (39,'Raw10001','Raw10001',NULL,0,0,1,1,'2023-03-15 14:07:38','2023-03-15 14:28:20',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (40,'Processed100012','Processed100012',NULL,0,0,1,1,'2023-03-15 14:32:46','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (41,'Raw100012','Raw100012',NULL,0,0,1,1,'2023-03-15 14:32:47','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (42,'Processed1000122','Processed1000122',NULL,0,0,1,1,'2023-03-15 16:21:35','2023-03-15 17:58:59',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (43,'Raw1000123','Raw1000123',NULL,0,0,1,1,'2023-03-15 16:21:36','2023-03-23 01:42:59',1,13,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (44,'D0001','D0001',NULL,0,0,1,1,'2023-03-16 11:12:39','2023-03-23 01:43:17',1,13,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (45,'Processed10002','Processed10002',NULL,0,0,1,1,'2023-03-20 14:16:50','2023-03-22 23:46:24',1,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (46,'SiloShelf100012','SiloShelf100012',NULL,0,0,1,1,'2023-03-21 07:01:46','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (49,'UP000102','UP000102',NULL,0,0,1,1,'2023-03-22 07:51:36','2023-03-22 07:52:57',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL);
INSERT INTO `t_device` VALUES (51,'A00','A00',NULL,0,0,1,1,'2023-03-22 23:02:07','2023-03-23 01:17:05',1,8,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (52,'P00002','P00002',NULL,0,0,1,1,'2023-03-22 23:16:16','2023-03-23 01:36:23',1,8,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (54,'Q0001','Q0001',NULL,0,0,1,1,'2023-03-23 01:19:33','2023-03-23 02:21:09',1,8,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (55,'生料1','生料1',NULL,0,0,1,1,'2023-03-23 02:50:16','2023-03-23 02:50:29',1,16,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (56,'熟料1111111111111111111','熟料111111111111111',NULL,0,0,1,1,'2023-03-23 21:04:32','2023-03-29 03:19:39',1,17,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (57,'拆板机1','拆板机1',NULL,0,0,1,1,'2023-03-23 21:05:05',NULL,NULL,15,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (58,'叠板机1','叠板机1',NULL,0,0,1,1,'2023-03-23 21:05:22',NULL,NULL,14,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `t_device` VALUES (62,'钻机1','钻机1',NULL,0,0,1,1,'2023-03-27 21:42:13',NULL,NULL,7,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `t_device` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_maintain`
--

DROP TABLE IF EXISTS `t_device_maintain`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_maintain` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备类型';
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
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `device_type_id` int(11) DEFAULT NULL COMMENT '设备类型',
  `is_deleted` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=240 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='事件管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_event_define`
--

LOCK TABLES `t_event_define` WRITE;
/*!40000 ALTER TABLE `t_event_define` DISABLE KEYS */;
INSERT INTO `t_event_define` VALUES (11,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-03 00:18:07','2023-03-28 04:02:25',1,NULL,0);
INSERT INTO `t_event_define` VALUES (12,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 13:46:34',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (13,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 13:47:38',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (14,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 13:47:52',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (15,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 13:48:05',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (16,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 13:49:26',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (17,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 13:56:34',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (18,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',1,1,'2023-03-03 14:12:26',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (19,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 14:14:52',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (20,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 14:16:19',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (21,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-03 14:17:26',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (22,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 14:18:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (23,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 15:47:40',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (24,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 15:54:08',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (25,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-03 15:55:45',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (26,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 15:57:13',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (27,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:00:31',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (28,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,1,'2023-03-03 16:03:06',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (29,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:28:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (30,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:33:47',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (31,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,1,'2023-03-03 16:42:39',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (32,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:45:14',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (33,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',3,1,'2023-03-03 16:45:50',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (34,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:48:55',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (35,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:49:10',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (36,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-03 16:54:35',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (37,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:56:18',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (38,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 16:59:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (39,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 17:06:20',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (40,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 17:13:07',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (41,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-03 17:14:20',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (42,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 17:15:05',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (43,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-03 17:24:46',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (44,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-03 17:26:31',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (45,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 09:47:49',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (46,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 09:50:53',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (47,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-07 10:22:19',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (48,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:25:05',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (49,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:25:53',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (50,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:26:08',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (51,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:26:24',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (52,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-07 10:26:26',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (53,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:26:53',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (54,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:32:51',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (55,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:39:09',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (56,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:40:34',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (57,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:42:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (58,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:43:22',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (59,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:44:09',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (60,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:46:04',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (61,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',2,1,'2023-03-07 10:46:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (62,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:47:32',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (63,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:49:18',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (64,'DRILL_REQUEST_LOAD_RAW_MATERIAL','DRILL_REQUEST_LOAD_RAW_MATERIAL',0,1,'2023-03-07 10:53:45',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (65,'AGV_LOW_BATTERY','AGV_LOW_BATTERY',0,1,'2023-03-07 20:20:49',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (66,NULL,NULL,0,1,'2023-03-07 20:21:55',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (67,NULL,NULL,0,1,'2023-03-07 20:22:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (68,NULL,NULL,0,1,'2023-03-07 20:23:59',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (69,NULL,NULL,0,1,'2023-03-07 20:26:03',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (70,NULL,NULL,2,1,'2023-03-07 20:27:25',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (71,NULL,NULL,0,1,'2023-03-07 20:27:33',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (72,NULL,NULL,0,1,'2023-03-07 20:31:00',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (73,NULL,NULL,0,1,'2023-03-07 20:31:43',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (74,NULL,NULL,0,1,'2023-03-07 20:32:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (75,NULL,NULL,0,1,'2023-03-07 20:32:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (76,NULL,NULL,0,1,'2023-03-07 20:33:40',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (77,NULL,NULL,0,1,'2023-03-07 20:33:44',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (78,NULL,NULL,0,1,'2023-03-07 20:34:06',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (79,NULL,NULL,0,1,'2023-03-07 20:34:44',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (80,NULL,NULL,0,1,'2023-03-07 20:35:41',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (81,NULL,NULL,0,1,'2023-03-07 20:36:43',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (82,NULL,NULL,0,1,'2023-03-07 20:39:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (83,'','',0,1,'2023-03-07 20:41:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (84,'','',0,1,'2023-03-07 20:43:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (85,NULL,NULL,0,1,'2023-03-07 20:45:04',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (86,NULL,NULL,0,1,'2023-03-07 20:47:15',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (87,'','',0,1,'2023-03-07 20:50:18',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (88,'','',0,1,'2023-03-08 09:53:23',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (89,'','',0,1,'2023-03-08 09:55:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (90,'','',0,1,'2023-03-08 10:28:39',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (91,'','',0,1,'2023-03-08 10:29:01',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (92,'','',0,1,'2023-03-08 10:29:24',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (93,'','',0,1,'2023-03-08 10:30:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (94,'','',0,1,'2023-03-08 10:30:48',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (95,'','',0,1,'2023-03-08 10:31:13',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (96,'','',0,1,'2023-03-08 11:03:44',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (97,'','',0,1,'2023-03-08 11:07:01',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (98,'','',0,1,'2023-03-08 11:47:10',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (99,'','',0,1,'2023-03-08 11:51:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (100,'','',0,1,'2023-03-08 13:03:24',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (101,'','',0,1,'2023-03-08 13:05:32',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (102,'','',0,1,'2023-03-08 13:07:27',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (103,'','',0,1,'2023-03-08 13:10:50',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (104,NULL,NULL,0,1,'2023-03-08 13:30:57',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (105,NULL,NULL,0,1,'2023-03-08 13:30:57',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (106,'','',0,1,'2023-03-08 15:42:57',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (107,'','',0,1,'2023-03-08 17:24:30',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (108,'','',0,1,'2023-03-09 11:49:31',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (109,'','',0,1,'2023-03-09 11:51:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (110,'','',0,1,'2023-03-09 11:51:44',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (111,'','',0,1,'2023-03-09 11:52:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (112,'','',0,1,'2023-03-09 11:53:36',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (113,'string','string',0,1,'2023-03-10 10:16:11',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (114,'string','string',0,1,'2023-03-10 10:16:48',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (115,'string','string',0,1,'2023-03-10 10:17:26',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (116,'string','string',0,1,'2023-03-10 11:11:40',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (117,'string','string',0,1,'2023-03-10 11:11:47',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (118,'string','string',0,1,'2023-03-10 11:12:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (119,'string','string',0,1,'2023-03-10 11:12:33',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (120,'string','string',0,1,'2023-03-10 11:14:55',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (121,'string','string',0,1,'2023-03-10 11:15:22',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (122,'string','string',0,1,'2023-03-10 13:01:08',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (123,'string','string',0,1,'2023-03-10 13:02:58',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (124,'string','string',0,1,'2023-03-10 13:04:51',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (125,'string','string',0,1,'2023-03-10 13:06:41',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (126,'string','string',0,1,'2023-03-10 13:07:19',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (127,'string','string',0,1,'2023-03-10 13:07:27',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (128,'string','string',0,1,'2023-03-10 13:08:14',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (129,'string','string',0,1,'2023-03-10 13:21:25',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (130,'string','string',0,1,'2023-03-10 13:22:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (131,'string','string',0,1,'2023-03-10 13:25:48',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (132,'string','string',0,1,'2023-03-10 13:25:52',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (133,'string','string',0,1,'2023-03-10 13:25:54',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (134,'string','string',0,1,'2023-03-10 13:26:10',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (135,'string','string',0,1,'2023-03-10 13:26:12',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (136,'string','string',0,1,'2023-03-10 13:26:13',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (137,'string','string',0,1,'2023-03-10 13:26:14',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (138,'string','string',0,1,'2023-03-10 13:26:14',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (139,'string','string',0,1,'2023-03-10 13:26:15',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (140,'string','',0,1,'2023-03-10 13:26:25',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (141,'string','',0,1,'2023-03-10 13:26:27',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (142,'string','',0,1,'2023-03-10 13:26:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (143,'string','',0,1,'2023-03-10 13:26:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (144,NULL,NULL,0,1,'2023-03-10 13:26:39',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (145,NULL,NULL,0,1,'2023-03-10 13:26:41',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (146,NULL,NULL,0,1,'2023-03-10 13:26:41',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (147,NULL,NULL,0,1,'2023-03-10 13:26:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (148,NULL,NULL,0,1,'2023-03-10 13:26:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (149,NULL,NULL,0,1,'2023-03-10 13:26:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (150,NULL,NULL,0,1,'2023-03-10 13:26:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (151,NULL,NULL,0,1,'2023-03-10 13:26:43',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (152,NULL,NULL,0,1,'2023-03-10 13:26:44',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (153,NULL,NULL,0,1,'2023-03-10 13:27:49',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (154,NULL,NULL,0,1,'2023-03-10 13:27:58',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (155,NULL,NULL,0,1,'2023-03-10 13:28:24',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (156,NULL,NULL,0,1,'2023-03-10 13:29:46',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (157,NULL,NULL,0,1,'2023-03-10 13:29:47',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (158,NULL,NULL,0,1,'2023-03-10 13:29:48',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (159,NULL,NULL,0,1,'2023-03-10 13:30:05',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (160,NULL,NULL,0,1,'2023-03-10 13:30:09',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (161,NULL,NULL,0,1,'2023-03-10 14:00:23',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (162,NULL,NULL,0,1,'2023-03-10 14:01:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (163,NULL,NULL,0,1,'2023-03-10 14:02:12',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (164,NULL,NULL,0,1,'2023-03-10 14:02:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (165,NULL,NULL,0,1,'2023-03-10 14:02:18',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (166,NULL,NULL,0,1,'2023-03-10 14:02:19',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (167,NULL,NULL,0,1,'2023-03-10 14:02:19',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (168,NULL,NULL,0,1,'2023-03-10 14:02:20',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (169,NULL,NULL,0,1,'2023-03-10 14:02:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (170,NULL,NULL,0,1,'2023-03-10 14:02:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (171,NULL,NULL,0,1,'2023-03-10 14:02:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (172,NULL,NULL,0,1,'2023-03-10 14:02:21',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (173,NULL,NULL,0,1,'2023-03-10 14:02:22',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (174,NULL,NULL,0,1,'2023-03-10 14:02:22',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (175,NULL,NULL,0,1,'2023-03-10 14:02:35',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (176,'','',0,1,'2023-03-10 14:03:58',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (177,'string','string',0,1,'2023-03-14 16:18:35',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (178,'string','string',0,1,'2023-03-14 16:22:35',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (179,'string','string',0,1,'2023-03-14 16:25:59',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (180,'string','string',0,1,'2023-03-14 16:26:50',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (181,'string','string',0,1,'2023-03-14 16:27:12',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (182,'string','string',0,1,'2023-03-14 16:28:54',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (183,'string','string',0,1,'2023-03-14 16:31:08',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (184,'string','string',0,1,'2023-03-14 16:31:44',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (185,'string','string',0,1,'2023-03-14 16:33:03',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (186,'string','string',0,1,'2023-03-14 16:35:41',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (187,'string','string',0,1,'2023-03-14 16:37:10',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (188,'string','string',0,1,'2023-03-14 16:47:39',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (189,'string','string',0,1,'2023-03-14 16:50:01',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (190,'string','string',0,1,'2023-03-14 16:54:13',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (191,'string','string',0,1,'2023-03-14 16:54:51',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (192,'string','string',0,1,'2023-03-14 16:55:15',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (193,'string','string',0,1,'2023-03-14 16:57:42',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (194,'string','string',0,1,'2023-03-14 16:58:10',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (195,'string','string',0,1,'2023-03-14 17:03:29',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (196,'string','string',0,1,'2023-03-14 17:07:20',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (197,'string','string',0,1,'2023-03-14 17:12:46',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (198,'string','string',0,1,'2023-03-14 17:19:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (199,'string','string',0,1,'2023-03-14 17:21:49',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (200,NULL,NULL,0,1,'2023-03-15 09:41:28',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (201,NULL,NULL,0,1,'2023-03-15 09:58:45',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (202,NULL,NULL,0,1,'2023-03-15 10:00:30',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (203,NULL,NULL,0,1,'2023-03-15 10:15:29',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (204,NULL,NULL,0,1,'2023-03-15 10:16:38',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (205,NULL,NULL,0,1,'2023-03-15 10:19:48',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (206,NULL,NULL,0,1,'2023-03-15 10:20:14',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (207,NULL,NULL,0,1,'2023-03-15 10:27:13',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (208,NULL,NULL,0,1,'2023-03-15 10:27:56',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (209,NULL,NULL,0,1,'2023-03-15 10:29:22',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (210,NULL,NULL,0,1,'2023-03-15 10:30:13',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (211,NULL,NULL,0,1,'2023-03-15 10:30:48',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (212,NULL,NULL,0,1,'2023-03-15 10:32:00',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (213,NULL,NULL,0,1,'2023-03-15 10:45:29',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (214,NULL,NULL,0,1,'2023-03-15 10:47:24',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (215,NULL,NULL,0,1,'2023-03-15 10:48:39',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (216,NULL,NULL,0,1,'2023-03-15 10:49:15',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (217,NULL,NULL,0,1,'2023-03-15 10:49:52',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (218,NULL,NULL,0,1,'2023-03-15 10:50:54',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (219,NULL,NULL,0,1,'2023-03-15 10:52:18',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (220,NULL,NULL,0,1,'2023-03-15 10:55:38',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (221,NULL,NULL,0,1,'2023-03-15 10:56:27',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (222,NULL,NULL,0,1,'2023-03-15 10:56:43',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (223,NULL,NULL,0,1,'2023-03-15 10:56:58',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (224,NULL,NULL,0,1,'2023-03-15 11:04:18',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (225,NULL,NULL,0,1,'2023-03-15 11:05:10',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (226,NULL,NULL,0,1,'2023-03-15 11:05:54',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (227,NULL,NULL,0,1,'2023-03-15 11:07:17',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (228,NULL,NULL,0,1,'2023-03-15 11:09:01',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (229,'gg','gg',3,1,'2023-03-15 11:12:06','2023-03-29 01:18:46',1,NULL,0);
INSERT INTO `t_event_define` VALUES (230,'22','22',1,1,'2023-03-15 11:20:04','2023-03-29 01:18:40',1,NULL,0);
INSERT INTO `t_event_define` VALUES (231,'11','11',1,1,'2023-03-15 11:21:43','2023-03-29 01:18:30',1,NULL,0);
INSERT INTO `t_event_define` VALUES (232,'11','11',2,1,'2023-03-15 12:40:16','2023-03-28 04:05:03',1,NULL,0);
INSERT INTO `t_event_define` VALUES (233,'222','222',2,1,'2023-03-15 12:44:02',NULL,NULL,NULL,0);
INSERT INTO `t_event_define` VALUES (234,'1112222','1112222',3,1,'2023-03-15 12:45:10','2023-03-28 03:58:15',1,NULL,0);
/*!40000 ALTER TABLE `t_event_define` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_item`
--

DROP TABLE IF EXISTS `t_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `item_or_product` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '物料或产品',
  `item_type_id` int(11) DEFAULT NULL COMMENT '物料产品类型Id',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品定义表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item`
--

LOCK TABLES `t_item` WRITE;
/*!40000 ALTER TABLE `t_item` DISABLE KEYS */;
INSERT INTO `t_item` VALUES (11,'1',0,'1',NULL,NULL,NULL,NULL,0,1,1,'2023-03-31 05:29:36',NULL,NULL,NULL);
INSERT INTO `t_item` VALUES (12,'1',0,'1','1',NULL,NULL,NULL,0,1,1,'2023-03-31 05:29:42','2023-03-31 05:30:06',1,NULL);
/*!40000 ALTER TABLE `t_item` ENABLE KEYS */;
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
  `item_or_product` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '物料或产品',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_type`
--

LOCK TABLES `t_item_type` WRITE;
/*!40000 ALTER TABLE `t_item_type` DISABLE KEYS */;
INSERT INTO `t_item_type` VALUES (1,'成品',3,'11','111',0,1,1,'2023-03-30 01:39:48','2023-03-30 05:10:28',1,NULL);
INSERT INTO `t_item_type` VALUES (2,'原料',3,'22','22',0,0,1,'2023-03-30 01:44:39','2023-03-31 03:59:14',1,NULL);
INSERT INTO `t_item_type` VALUES (3,'物料产品分类',0,'11','11',0,1,1,'2023-03-30 05:08:07',NULL,NULL,NULL);
INSERT INTO `t_item_type` VALUES (4,'半成品',3,'22','333',0,1,1,'2023-03-30 05:10:36',NULL,NULL,NULL);
INSERT INTO `t_item_type` VALUES (5,'11',3,'11','11',0,1,1,'2023-03-30 05:12:16',NULL,NULL,NULL);
INSERT INTO `t_item_type` VALUES (6,'2',3,'2','2',0,1,1,'2023-03-31 05:30:13',NULL,NULL,NULL);
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
  `warehouse_id` bigint(20) NOT NULL COMMENT '仓库ID',
  `warehouse_code` varchar(64) DEFAULT NULL COMMENT '仓库编码',
  `warehouse_name` varchar(255) DEFAULT NULL COMMENT '仓库名称',
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
-- Table structure for table `t_notify`
--

DROP TABLE IF EXISTS `t_notify`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_notify` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify`
--

LOCK TABLES `t_notify` WRITE;
/*!40000 ALTER TABLE `t_notify` DISABLE KEYS */;
INSERT INTO `t_notify` VALUES (14,'2023-03-27 05:54:07','111','111','内容1',1,0,1,1,'2023-03-27 05:54:07',NULL,NULL,NULL);
INSERT INTO `t_notify` VALUES (15,'2023-03-27 05:54:07','1112','1112','人1',2,0,0,1,'2023-03-27 21:21:47','2023-03-28 01:15:03',1,NULL);
INSERT INTO `t_notify` VALUES (16,'2023-03-27 05:54:07','111','11','2',2,0,0,1,'2023-03-28 01:15:08','2023-03-28 01:19:57',1,NULL);
INSERT INTO `t_notify` VALUES (17,'2023-03-27 05:54:07','333','333','3',2,0,0,1,'2023-03-28 01:22:34','2023-03-28 01:28:25',1,NULL);
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
INSERT INTO `t_notify_setting` VALUES (11,'大屏告警',0,'BIG_SCREEN_WARNING',NULL,NULL,NULL,0,0,1,'2023-02-28 09:14:03','2023-02-28 09:19:33',1,NULL);
INSERT INTO `t_notify_setting` VALUES (12,'警报',0,'LIGHT_WARNING',NULL,NULL,NULL,0,1,1,'2023-02-28 09:19:38','2023-03-28 21:29:29',1,NULL);
INSERT INTO `t_notify_setting` VALUES (21,'11',0,'2222',NULL,NULL,NULL,0,1,1,'2023-03-28 21:29:34','2023-03-29 01:11:03',1,NULL);
INSERT INTO `t_notify_setting` VALUES (22,'1122',0,'2222',NULL,NULL,NULL,0,0,1,'2023-03-29 01:11:08','2023-03-29 01:42:08',1,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (11,'sss','ss',NULL,0,1,1,'2023-03-01 13:36:57',NULL,NULL);
INSERT INTO `t_process` VALUES (13,'cc112','cc11',NULL,0,0,1,'2023-03-01 13:37:05','2023-03-23 03:35:01',1);
/*!40000 ALTER TABLE `t_process` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_process_recipe`
--

DROP TABLE IF EXISTS `t_process_recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_process_recipe` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工艺参数、配方管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process_recipe`
--

LOCK TABLES `t_process_recipe` WRITE;
/*!40000 ALTER TABLE `t_process_recipe` DISABLE KEYS */;
INSERT INTO `t_process_recipe` VALUES (12,'yy','yy',NULL,NULL,NULL,NULL,NULL,0,1,1,'2023-02-28 10:23:22',NULL,NULL);
INSERT INTO `t_process_recipe` VALUES (13,'zz11','zz11111',NULL,NULL,NULL,NULL,NULL,0,0,1,'2023-02-28 10:23:25','2023-03-24 05:15:59',1);
/*!40000 ALTER TABLE `t_process_recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_product_category`
--

DROP TABLE IF EXISTS `t_product_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_product_category` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `code` varchar(64) NOT NULL COMMENT '产品大类编码',
  `name` varchar(255) NOT NULL COMMENT '产品大类名称',
  `remark` varchar(500) DEFAULT '' COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_product_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_schedulement`
--

DROP TABLE IF EXISTS `t_schedulement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_schedulement` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产任务';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_task`
--

LOCK TABLES `t_task` WRITE;
/*!40000 ALTER TABLE `t_task` DISABLE KEYS */;
INSERT INTO `t_task` VALUES (11,'1112',0,'qqq',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,1,'2023-03-01 09:19:18','2023-03-23 03:31:11',1,NULL);
INSERT INTO `t_task` VALUES (13,'dd11',0,'dd112',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,1,'2023-03-01 13:31:13','2023-03-23 03:31:06',1,NULL);
/*!40000 ALTER TABLE `t_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_trans_order`
--

DROP TABLE IF EXISTS `t_trans_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_trans_order` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
INSERT INTO `t_trans_order` VALUES (11,'ss',0,'ss',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,1,1,'2023-03-01 13:40:03',NULL,NULL);
INSERT INTO `t_trans_order` VALUES (13,'xx11',0,'xx22',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,1,'2023-03-01 13:40:18','2023-03-24 02:51:16',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (11,'11','11','Y',NULL,NULL,'1111',0,1,1,'2023-03-31 05:05:06','2023-03-31 05:08:27',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_vendor`
--

LOCK TABLES `t_vendor` WRITE;
/*!40000 ALTER TABLE `t_vendor` DISABLE KEYS */;
INSERT INTO `t_vendor` VALUES (1,'string','string','string',0,1,1,'2023-03-30 13:48:13',NULL,NULL);
INSERT INTO `t_vendor` VALUES (2,'string','string','string',0,1,1,'2023-03-31 04:24:42',NULL,NULL);
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
  `code` varchar(64) NOT NULL COMMENT '仓库编码',
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
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `parent_id` int(11) NOT NULL COMMENT '父对象Id(0表示是根对象)',
  `code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '编号',
  `order_source` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '来源类型',
  `source_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '来源单据',
  `item_id` int(11) DEFAULT NULL COMMENT '产品Id',
  `item_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
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
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产工单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_work_order`
--

LOCK TABLES `t_work_order` WRITE;
/*!40000 ALTER TABLE `t_work_order` DISABLE KEYS */;
INSERT INTO `t_work_order` VALUES (12,'dd11',0,'dd11','1','2',2,'22','22','22','11',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-02-28 10:10:19',0,0,1,'2023-02-28 10:10:19','2023-02-28 10:10:24',1,NULL);
INSERT INTO `t_work_order` VALUES (13,'22',0,'22','1','2',2,'22','22','22','11',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2023-02-28 10:10:19',0,0,1,'2023-02-28 13:23:27','2023-03-23 03:34:41',1,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workshop`
--

LOCK TABLES `t_workshop` WRITE;
/*!40000 ALTER TABLE `t_workshop` DISABLE KEYS */;
INSERT INTO `t_workshop` VALUES (1,'string','string','string','string',0,1,1,'2023-03-30 13:48:31',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=202 DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workstation`
--

LOCK TABLES `t_workstation` WRITE;
/*!40000 ALTER TABLE `t_workstation` DISABLE KEYS */;
INSERT INTO `t_workstation` VALUES (200,'string','string',1,'string','string','string',0,1,1,'2023-03-30 13:48:50',NULL,NULL);
INSERT INTO `t_workstation` VALUES (201,'string','string',1,'string','string','string2',0,1,1,'2023-03-31 05:32:09',NULL,NULL);
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

-- Dump completed on 2023-04-03  1:54:06
