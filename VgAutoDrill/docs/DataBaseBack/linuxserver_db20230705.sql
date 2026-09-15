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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_department`
--

LOCK TABLES `sys_department` WRITE;
/*!40000 ALTER TABLE `sys_department` DISABLE KEYS */;
INSERT INTO `sys_department` VALUES (1,'Boss科技',0,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:05:59','2023-03-23 03:12:42',1);
INSERT INTO `sys_department` VALUES (2,'深圳总公司',1,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-03-26 20:38:38',1);
INSERT INTO `sys_department` VALUES (3,'长沙分公司',2,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-06-18 03:24:07',1);
INSERT INTO `sys_department` VALUES (4,'研发部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-03-23 03:12:16',1);
INSERT INTO `sys_department` VALUES (5,'市场部门',11,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00','2023-06-18 21:21:48',1);
INSERT INTO `sys_department` VALUES (6,'测试部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (7,'财务部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (8,'运维部门',3,'15888888888','boss@qq.com',NULL,NULL,0,1,0,'2022-09-26 11:06:00',NULL,NULL);
INSERT INTO `sys_department` VALUES (9,'研发部门',2,'18037458723','2230485082@qq.com',NULL,'xm',0,1,1,'2023-02-23 03:10:17','2023-06-28 15:25:30',1);
INSERT INTO `sys_department` VALUES (11,'北京分公司',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-06-18 01:16:22','2023-06-18 03:23:48',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=1354 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
INSERT INTO `sys_menu` VALUES (1031,'DRILLAMIN','钻孔任务',1024,'#','drillTask','produce/drillTask/index',2,'produce:drillTask:list',NULL,7,0,1,1,'2023-02-20 23:11:13','2023-06-19 16:13:44',1);
INSERT INTO `sys_menu` VALUES (1032,'DRILLAMIN','刀盘管理',1025,'#','cutter','material/cutter/index',2,'material:cutter:list',NULL,1,0,0,1,'2023-02-20 23:15:40','2023-04-18 02:15:39',1);
INSERT INTO `sys_menu` VALUES (1033,'DRILLAMIN','设备类型',1026,'#','equipmentType','device/equipmentType/index',2,'device:equipmentType:list',NULL,1,0,1,1,'2023-02-20 23:18:29','2023-04-02 21:21:28',1);
INSERT INTO `sys_menu` VALUES (1034,'DRILLAMIN','配方管理',1024,'#','recipe','produce/recipe/index',2,'produce:recipe:list',NULL,3,0,0,1,'2023-02-20 23:20:37','2023-06-12 14:22:27',1);
INSERT INTO `sys_menu` VALUES (1035,'DRILLAMIN','设备列表',1026,'#','equipment','device/equipment/index',2,'device:equipment:list',NULL,3,0,1,1,'2023-02-20 23:22:03','2023-02-21 02:49:47',1);
INSERT INTO `sys_menu` VALUES (1036,'DRILLAMIN','刀具参数',1025,'#','config','material/config/index',2,'material:config:list',NULL,2,0,1,1,'2023-02-20 23:23:49','2023-03-28 05:30:47',1);
INSERT INTO `sys_menu` VALUES (1037,'DRILLAMIN','调度记录',1026,'#','schedulement','device/schedulement/index',2,'device:schedulement:edit',NULL,7,0,1,1,'2023-02-20 23:25:16','2023-06-18 01:14:12',1);
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
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'tree','wm','#',1,NULL,NULL,50,0,1,1,'2023-04-01 02:50:34','2023-06-25 16:32:58',1);
INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','仓库设置',1158,'#','warehouse','wareHouse/wareHouse/index',2,'warehouse:warehouse:list',NULL,1,0,1,1,'2023-04-01 02:53:42','2023-04-18 03:33:51',1);
INSERT INTO `sys_menu` VALUES (1160,'DRILLAMIN','库存现有量',1158,'#','materialStock','wareHouse/materialStock/index',2,'warehouse:materialStock:list',NULL,2,0,1,1,'2023-04-01 02:54:57','2023-05-11 04:28:18',1);
INSERT INTO `sys_menu` VALUES (1161,'DRILLAMIN','钻带参数',1025,'#','drillFile','material/drillFile/index',2,'material:drillFile:list',NULL,3,0,1,1,'2023-04-01 03:20:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1162,'DRILLAMIN','排刀文件',1025,'#','atpFile','material/atpFile/index',2,'material:atpFile:list',NULL,4,0,1,1,'2023-04-01 03:21:21','2023-04-10 23:34:21',1);
INSERT INTO `sys_menu` VALUES (1165,'DRILLAMIN','报工记录',1024,'#','reportrecords','produce/reportRecords/index',2,'produce:reportRecords:list',NULL,9,0,1,1,'2023-04-04 00:47:50','2023-06-19 16:14:13',1);
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
INSERT INTO `sys_menu` VALUES (1350,'DRILLAMIN','生产报工',1024,'#','feedback','produce/feedback/index',2,'produce:feedback:list',NULL,8,0,1,1,'2023-06-12 11:24:59','2023-06-19 16:14:18',1);
INSERT INTO `sys_menu` VALUES (1351,'DRILLAMIN','查看',1350,'#',NULL,'#',3,'produce:feedback:view',NULL,1,0,1,1,'2023-06-12 05:23:38',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1352,'DRILLAMIN','取消',1037,'#',NULL,'#',3,'device:schedulement:cancel',NULL,5,0,1,1,'2023-06-30 13:54:41','2023-06-30 14:27:53',1);
INSERT INTO `sys_menu` VALUES (1353,'DRILLAMIN','查询',1037,'#',NULL,'#',3,'device:schedulement:list',NULL,1,0,1,1,'2023-06-30 14:28:08',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=20947 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
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
INSERT INTO `sys_menu_auth` VALUES (19513,1,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19514,2,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19515,1000,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19516,1001,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19517,1002,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19518,1003,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19519,1004,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19520,1005,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19521,1006,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19522,3,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19523,1007,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19524,1008,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19525,1009,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19526,1010,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19527,1011,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19528,4,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19529,1012,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19530,1013,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19531,1014,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19532,1015,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19533,5,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19534,1016,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19535,1017,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19536,1018,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19537,1019,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19538,6,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19539,1020,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19540,1021,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19541,1022,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19542,1023,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19543,1024,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19544,1029,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19545,1077,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19546,1078,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19547,1079,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19548,1080,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19549,1081,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19550,1280,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19551,1310,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19552,1030,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19553,1072,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19554,1073,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19555,1074,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19556,1075,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19557,1076,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19558,1313,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19559,1332,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19560,1031,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19561,1082,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19562,1083,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19563,1084,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19564,1085,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19565,1086,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19566,1311,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19567,1339,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19568,1122,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19569,1123,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19570,1124,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19571,1125,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19572,1126,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19573,1127,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19574,1279,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19575,1309,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19576,1330,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19577,1128,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19578,1129,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19579,1130,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19580,1131,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19581,1132,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19582,1133,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19583,1307,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19584,1329,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19585,1157,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19586,1219,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19587,1220,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19588,1221,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19589,1222,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19590,1223,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19591,1224,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19592,1292,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19593,1293,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19594,1294,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19595,1295,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19596,1296,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19597,1297,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19598,1298,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19599,1299,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19600,1300,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19601,1301,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19602,1302,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19603,1303,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19604,1304,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19605,1305,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19606,1306,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19607,1165,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19608,1166,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19609,1167,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19610,1168,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19611,1169,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19612,1170,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19613,1281,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19614,1312,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19615,1331,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19616,1314,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19617,1315,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19618,1333,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19619,1334,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19620,1340,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19621,1350,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19622,1351,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19623,1025,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19624,1036,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19625,1062,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19626,1063,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19627,1064,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19628,1065,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19629,1066,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19630,1341,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19631,1343,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19632,1161,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19633,1171,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19634,1172,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19635,1173,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19636,1174,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19637,1175,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19638,1176,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19639,1342,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19640,1162,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19641,1177,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19642,1178,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19643,1179,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19644,1180,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19645,1181,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19646,1182,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19647,1282,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19648,1283,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19649,1284,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19650,1285,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19651,1286,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19652,1026,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19653,1033,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19654,1043,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19655,1044,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19656,1045,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19657,1052,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19658,1035,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19659,1057,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19660,1058,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19661,1059,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19662,1060,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19663,1061,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19664,1037,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19665,1068,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19666,1069,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19667,1070,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19668,1071,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19669,1352,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19670,1353,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19671,1317,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19672,1344,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19673,1345,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19674,1346,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19675,1347,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19676,1348,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19677,1319,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19678,1027,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19679,1038,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19680,1097,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19681,1098,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19682,1099,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19683,1100,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19684,1101,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19685,1335,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19686,1039,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19687,1102,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19688,1103,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19689,1104,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19690,1105,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19691,1106,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19692,1107,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19693,1336,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19694,1136,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19695,1137,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19696,1138,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19697,1139,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19698,1140,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19699,1054,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19700,1055,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19701,1108,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19702,1109,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19703,1110,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19704,1111,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19705,1112,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19706,1056,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19707,1113,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19708,1114,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19709,1115,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19710,1116,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19711,1117,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19712,1349,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19713,1148,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19714,1149,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19715,1231,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19716,1232,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19717,1233,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19718,1234,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19719,1235,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19720,1236,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19721,1321,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19722,1324,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19723,1150,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19724,1237,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19725,1238,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19726,1239,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19727,1240,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19728,1241,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19729,1242,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19730,1327,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19731,1151,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19732,1243,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19733,1244,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19734,1245,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19735,1246,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19736,1247,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19737,1248,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19738,1328,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19739,1152,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19740,1249,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19741,1250,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19742,1251,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19743,1252,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19744,1253,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19745,1254,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19746,1153,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19747,1255,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19748,1256,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19749,1257,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19750,1258,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19751,1259,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19752,1260,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19753,1326,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19754,1154,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19755,1261,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19756,1262,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19757,1263,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19758,1264,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19759,1265,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19760,1266,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19761,1325,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19762,1155,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19763,1267,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19764,1268,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19765,1269,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19766,1270,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19767,1271,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19768,1272,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19769,1287,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19770,1288,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19771,1289,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19772,1290,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19773,1291,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19774,1338,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19775,1156,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19776,1273,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19777,1274,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19778,1275,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19779,1276,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19780,1277,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19781,1278,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19782,1337,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19783,1320,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19784,1322,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19785,1323,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19786,1158,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19787,1159,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19788,1207,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19789,1208,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19790,1209,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19791,1210,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19792,1211,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19793,1212,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19794,1160,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19795,1213,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19796,1214,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19797,1215,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19798,1216,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19799,1217,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19800,1218,2,1,1,'2023-06-30 14:44:45');
INSERT INTO `sys_menu_auth` VALUES (19801,1,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19802,2,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19803,1000,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19804,1001,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19805,1002,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19806,1003,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19807,1004,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19808,1005,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19809,1006,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19810,3,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19811,1007,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19812,1008,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19813,1009,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19814,1010,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19815,1011,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19816,4,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19817,1012,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19818,1013,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19819,1014,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19820,1015,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19821,5,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19822,1016,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19823,1017,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19824,1018,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19825,1019,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19826,6,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19827,1020,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19828,1021,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19829,1022,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19830,1023,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19831,1024,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19832,1029,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19833,1077,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19834,1078,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19835,1079,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19836,1080,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19837,1081,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19838,1280,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19839,1310,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19840,1030,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19841,1072,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19842,1073,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19843,1074,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19844,1075,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19845,1076,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19846,1313,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19847,1332,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19848,1031,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19849,1082,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19850,1083,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19851,1084,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19852,1085,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19853,1086,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19854,1311,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19855,1339,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19856,1122,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19857,1123,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19858,1124,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19859,1125,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19860,1126,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19861,1127,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19862,1279,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19863,1309,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19864,1330,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19865,1128,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19866,1129,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19867,1130,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19868,1131,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19869,1132,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19870,1133,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19871,1307,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19872,1329,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19873,1157,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19874,1219,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19875,1220,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19876,1221,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19877,1222,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19878,1223,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19879,1224,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19880,1292,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19881,1293,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19882,1294,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19883,1295,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19884,1296,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19885,1297,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19886,1298,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19887,1299,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19888,1300,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19889,1301,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19890,1302,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19891,1303,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19892,1304,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19893,1305,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19894,1306,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19895,1165,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19896,1166,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19897,1167,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19898,1168,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19899,1169,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19900,1170,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19901,1281,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19902,1312,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19903,1331,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19904,1314,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19905,1315,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19906,1333,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19907,1334,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19908,1340,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19909,1350,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19910,1351,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19911,1025,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19912,1036,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19913,1062,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19914,1063,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19915,1064,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19916,1065,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19917,1066,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19918,1341,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19919,1343,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19920,1161,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19921,1171,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19922,1172,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19923,1173,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19924,1174,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19925,1175,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19926,1176,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19927,1342,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19928,1162,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19929,1177,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19930,1178,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19931,1179,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19932,1180,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19933,1181,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19934,1182,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19935,1282,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19936,1283,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19937,1284,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19938,1285,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19939,1286,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19940,1026,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19941,1033,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19942,1043,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19943,1044,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19944,1045,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19945,1052,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19946,1035,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19947,1057,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19948,1058,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19949,1059,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19950,1060,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19951,1061,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19952,1037,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19953,1068,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19954,1069,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19955,1070,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19956,1071,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19957,1352,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19958,1353,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19959,1317,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19960,1344,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19961,1345,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19962,1346,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19963,1347,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19964,1348,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19965,1319,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19966,1027,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19967,1038,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19968,1097,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19969,1098,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19970,1099,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19971,1100,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19972,1101,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19973,1335,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19974,1039,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19975,1102,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19976,1103,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19977,1104,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19978,1105,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19979,1106,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19980,1107,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19981,1336,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19982,1136,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19983,1137,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19984,1138,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19985,1139,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19986,1140,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19987,1054,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19988,1055,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19989,1108,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19990,1109,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19991,1110,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19992,1111,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19993,1112,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19994,1056,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19995,1113,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19996,1114,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19997,1115,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19998,1116,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (19999,1117,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20000,1349,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20001,1148,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20002,1149,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20003,1231,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20004,1232,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20005,1233,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20006,1234,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20007,1235,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20008,1236,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20009,1321,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20010,1324,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20011,1150,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20012,1237,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20013,1238,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20014,1239,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20015,1240,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20016,1241,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20017,1242,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20018,1327,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20019,1151,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20020,1243,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20021,1244,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20022,1245,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20023,1246,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20024,1247,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20025,1248,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20026,1328,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20027,1152,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20028,1249,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20029,1250,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20030,1251,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20031,1252,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20032,1253,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20033,1254,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20034,1153,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20035,1255,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20036,1256,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20037,1257,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20038,1258,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20039,1259,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20040,1260,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20041,1326,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20042,1154,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20043,1261,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20044,1262,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20045,1263,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20046,1264,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20047,1265,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20048,1266,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20049,1325,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20050,1155,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20051,1267,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20052,1268,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20053,1269,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20054,1270,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20055,1271,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20056,1272,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20057,1287,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20058,1288,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20059,1289,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20060,1290,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20061,1291,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20062,1338,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20063,1156,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20064,1273,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20065,1274,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20066,1275,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20067,1276,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20068,1277,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20069,1278,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20070,1337,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20071,1320,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20072,1322,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20073,1323,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20074,1158,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20075,1159,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20076,1207,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20077,1208,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20078,1209,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20079,1210,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20080,1211,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20081,1212,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20082,1160,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20083,1213,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20084,1214,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20085,1215,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20086,1216,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20087,1217,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20088,1218,3,1,1,'2023-06-30 14:44:49');
INSERT INTO `sys_menu_auth` VALUES (20089,1,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20090,2,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20091,1000,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20092,1001,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20093,1002,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20094,1003,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20095,1004,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20096,1005,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20097,1006,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20098,3,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20099,1007,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20100,1008,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20101,1009,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20102,1010,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20103,1011,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20104,4,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20105,1012,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20106,1013,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20107,1014,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20108,1015,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20109,5,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20110,1016,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20111,1017,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20112,1018,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20113,1019,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20114,6,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20115,1020,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20116,1021,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20117,1022,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20118,1023,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20119,1024,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20120,1029,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20121,1077,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20122,1078,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20123,1079,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20124,1080,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20125,1081,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20126,1280,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20127,1310,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20128,1030,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20129,1072,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20130,1073,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20131,1074,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20132,1075,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20133,1076,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20134,1313,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20135,1332,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20136,1031,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20137,1082,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20138,1083,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20139,1084,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20140,1085,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20141,1086,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20142,1311,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20143,1339,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20144,1122,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20145,1123,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20146,1124,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20147,1125,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20148,1126,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20149,1127,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20150,1279,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20151,1309,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20152,1330,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20153,1128,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20154,1129,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20155,1130,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20156,1131,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20157,1132,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20158,1133,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20159,1307,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20160,1329,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20161,1157,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20162,1219,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20163,1220,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20164,1221,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20165,1222,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20166,1223,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20167,1224,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20168,1292,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20169,1293,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20170,1294,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20171,1295,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20172,1296,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20173,1297,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20174,1298,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20175,1299,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20176,1300,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20177,1301,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20178,1302,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20179,1303,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20180,1304,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20181,1305,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20182,1306,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20183,1165,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20184,1166,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20185,1167,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20186,1168,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20187,1169,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20188,1170,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20189,1281,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20190,1312,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20191,1331,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20192,1314,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20193,1315,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20194,1333,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20195,1334,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20196,1340,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20197,1350,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20198,1351,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20199,1025,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20200,1036,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20201,1062,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20202,1063,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20203,1064,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20204,1065,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20205,1066,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20206,1341,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20207,1343,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20208,1161,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20209,1171,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20210,1172,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20211,1173,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20212,1174,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20213,1175,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20214,1176,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20215,1342,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20216,1162,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20217,1177,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20218,1178,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20219,1179,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20220,1180,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20221,1181,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20222,1182,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20223,1282,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20224,1283,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20225,1284,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20226,1285,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20227,1286,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20228,1026,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20229,1033,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20230,1043,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20231,1044,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20232,1045,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20233,1052,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20234,1035,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20235,1057,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20236,1058,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20237,1059,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20238,1060,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20239,1061,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20240,1037,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20241,1068,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20242,1069,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20243,1070,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20244,1071,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20245,1352,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20246,1353,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20247,1317,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20248,1344,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20249,1345,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20250,1346,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20251,1347,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20252,1348,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20253,1319,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20254,1027,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20255,1038,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20256,1097,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20257,1098,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20258,1099,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20259,1100,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20260,1101,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20261,1335,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20262,1039,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20263,1102,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20264,1103,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20265,1104,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20266,1105,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20267,1106,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20268,1107,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20269,1336,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20270,1136,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20271,1137,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20272,1138,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20273,1139,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20274,1140,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20275,1054,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20276,1055,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20277,1108,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20278,1109,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20279,1110,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20280,1111,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20281,1112,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20282,1056,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20283,1113,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20284,1114,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20285,1115,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20286,1116,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20287,1117,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20288,1349,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20289,1148,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20290,1149,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20291,1231,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20292,1232,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20293,1233,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20294,1234,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20295,1235,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20296,1236,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20297,1321,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20298,1324,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20299,1150,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20300,1237,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20301,1238,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20302,1239,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20303,1240,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20304,1241,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20305,1242,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20306,1327,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20307,1151,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20308,1243,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20309,1244,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20310,1245,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20311,1246,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20312,1247,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20313,1248,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20314,1328,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20315,1152,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20316,1249,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20317,1250,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20318,1251,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20319,1252,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20320,1253,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20321,1254,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20322,1153,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20323,1255,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20324,1256,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20325,1257,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20326,1258,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20327,1259,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20328,1260,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20329,1326,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20330,1154,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20331,1261,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20332,1262,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20333,1263,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20334,1264,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20335,1265,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20336,1266,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20337,1325,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20338,1155,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20339,1267,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20340,1268,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20341,1269,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20342,1270,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20343,1271,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20344,1272,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20345,1287,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20346,1288,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20347,1289,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20348,1290,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20349,1291,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20350,1338,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20351,1156,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20352,1273,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20353,1274,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20354,1275,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20355,1276,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20356,1277,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20357,1278,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20358,1337,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20359,1320,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20360,1322,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20361,1323,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20362,1158,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20363,1159,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20364,1207,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20365,1208,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20366,1209,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20367,1210,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20368,1211,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20369,1212,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20370,1160,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20371,1213,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20372,1214,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20373,1215,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20374,1216,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20375,1217,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20376,1218,6,1,1,'2023-06-30 14:44:52');
INSERT INTO `sys_menu_auth` VALUES (20665,1,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20666,2,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20667,1000,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20668,3,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20669,1007,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20670,1008,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20671,1009,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20672,1010,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20673,1011,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20674,4,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20675,1012,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20676,1013,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20677,1014,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20678,1015,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20679,5,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20680,1016,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20681,1017,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20682,1018,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20683,1019,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20684,6,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20685,1020,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20686,1021,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20687,1022,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20688,1023,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20689,1024,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20690,1029,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20691,1077,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20692,1078,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20693,1079,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20694,1080,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20695,1081,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20696,1280,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20697,1310,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20698,1030,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20699,1072,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20700,1073,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20701,1074,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20702,1075,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20703,1076,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20704,1313,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20705,1332,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20706,1031,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20707,1082,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20708,1083,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20709,1084,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20710,1085,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20711,1086,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20712,1311,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20713,1339,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20714,1122,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20715,1123,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20716,1124,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20717,1125,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20718,1126,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20719,1127,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20720,1279,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20721,1309,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20722,1330,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20723,1128,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20724,1129,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20725,1130,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20726,1131,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20727,1132,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20728,1133,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20729,1307,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20730,1329,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20731,1157,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20732,1219,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20733,1220,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20734,1221,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20735,1222,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20736,1223,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20737,1224,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20738,1292,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20739,1293,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20740,1294,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20741,1295,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20742,1296,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20743,1297,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20744,1298,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20745,1299,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20746,1300,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20747,1301,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20748,1302,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20749,1303,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20750,1304,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20751,1305,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20752,1306,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20753,1165,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20754,1166,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20755,1167,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20756,1168,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20757,1169,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20758,1170,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20759,1281,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20760,1312,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20761,1331,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20762,1314,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20763,1315,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20764,1333,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20765,1334,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20766,1340,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20767,1350,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20768,1351,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20769,1025,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20770,1036,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20771,1062,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20772,1063,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20773,1064,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20774,1065,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20775,1066,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20776,1341,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20777,1343,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20778,1161,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20779,1171,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20780,1172,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20781,1173,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20782,1174,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20783,1175,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20784,1176,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20785,1342,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20786,1162,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20787,1177,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20788,1178,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20789,1179,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20790,1180,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20791,1181,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20792,1182,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20793,1282,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20794,1283,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20795,1284,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20796,1285,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20797,1286,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20798,1026,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20799,1033,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20800,1043,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20801,1044,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20802,1045,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20803,1052,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20804,1035,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20805,1057,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20806,1058,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20807,1059,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20808,1060,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20809,1061,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20810,1037,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20811,1068,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20812,1069,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20813,1070,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20814,1071,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20815,1352,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20816,1353,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20817,1317,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20818,1344,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20819,1345,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20820,1346,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20821,1347,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20822,1348,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20823,1319,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20824,1027,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20825,1038,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20826,1097,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20827,1098,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20828,1099,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20829,1100,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20830,1101,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20831,1335,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20832,1039,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20833,1102,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20834,1103,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20835,1104,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20836,1105,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20837,1106,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20838,1107,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20839,1336,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20840,1136,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20841,1137,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20842,1138,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20843,1139,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20844,1140,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20845,1054,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20846,1055,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20847,1108,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20848,1109,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20849,1110,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20850,1111,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20851,1112,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20852,1056,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20853,1113,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20854,1114,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20855,1115,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20856,1116,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20857,1117,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20858,1349,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20859,1148,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20860,1149,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20861,1231,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20862,1232,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20863,1233,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20864,1234,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20865,1235,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20866,1236,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20867,1321,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20868,1324,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20869,1150,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20870,1237,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20871,1238,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20872,1239,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20873,1240,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20874,1241,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20875,1242,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20876,1327,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20877,1151,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20878,1243,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20879,1244,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20880,1245,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20881,1246,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20882,1247,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20883,1248,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20884,1328,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20885,1152,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20886,1249,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20887,1250,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20888,1251,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20889,1252,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20890,1253,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20891,1254,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20892,1153,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20893,1255,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20894,1256,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20895,1257,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20896,1258,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20897,1259,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20898,1260,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20899,1326,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20900,1154,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20901,1261,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20902,1262,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20903,1263,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20904,1264,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20905,1265,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20906,1266,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20907,1325,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20908,1155,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20909,1267,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20910,1268,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20911,1269,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20912,1270,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20913,1271,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20914,1272,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20915,1287,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20916,1288,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20917,1289,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20918,1290,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20919,1291,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20920,1338,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20921,1156,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20922,1273,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20923,1274,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20924,1275,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20925,1276,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20926,1277,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20927,1278,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20928,1337,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20929,1320,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20930,1322,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20931,1323,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20932,1158,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20933,1159,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20934,1207,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20935,1208,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20936,1209,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20937,1210,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20938,1211,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20939,1212,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20940,1160,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20941,1213,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20942,1214,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20943,1215,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20944,1216,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20945,1217,8,1,1,'2023-06-30 14:51:06');
INSERT INTO `sys_menu_auth` VALUES (20946,1218,8,1,1,'2023-06-30 14:51:06');
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
INSERT INTO `sys_position` VALUES (1,'董事长',NULL,1,0,1,NULL,'2022-11-08 13:56:35','2023-06-28 15:24:26',1);
INSERT INTO `sys_position` VALUES (2,'项目经理',NULL,2,0,1,NULL,'2022-11-08 13:56:56','2023-06-28 15:24:39',1);
INSERT INTO `sys_position` VALUES (3,'人力资源',NULL,4,0,1,NULL,'2022-11-08 13:57:24','2023-06-28 15:25:08',1);
INSERT INTO `sys_position` VALUES (4,'普通员工','111',5,0,1,NULL,'2022-11-08 13:57:48','2023-06-28 15:25:04',1);
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
INSERT INTO `sys_role` VALUES (2,'车间管理',3,0,1,0,'2022-11-17 08:58:18','2023-06-30 14:44:45',1);
INSERT INTO `sys_role` VALUES (3,'生产报工',4,0,1,1,'2023-02-23 02:01:07','2023-06-30 14:44:49',1);
INSERT INTO `sys_role` VALUES (6,'质检组长',4,0,1,33,'2023-04-18 02:02:51','2023-06-30 14:44:51',1);
INSERT INTO `sys_role` VALUES (8,'质检员',8,0,1,33,'2023-04-18 02:02:57','2023-06-30 14:51:06',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (1,'admin','5794620ce8325bc4e7b67677da3daeaa','c5fc028a23cd41c696a004f9dba65bff','ADMIN',1,NULL,0,NULL,NULL,'13400000001','13400000001@qq.com',NULL,NULL,1,NULL,NULL,0,1,NULL,'2022-09-27 16:07:39','2023-04-19 20:53:20',1);
INSERT INTO `sys_user` VALUES (27,'lisi','9438571d588b26e234709e6f975ba3e4','c84681f4183543f4910f9f34683f73d8','1',1,4,0,NULL,NULL,'18339475689','23459378@qq.com',NULL,NULL,NULL,'2',NULL,0,0,1,'2023-02-22 01:38:14',NULL,NULL);
INSERT INTO `sys_user` VALUES (28,'xiaoming','8df2f4462922bca05ea29b079065fc48','01266758ce8a46b09004a4185f6934b1','xt',2,2,0,NULL,NULL,'15634785930','22348340@qq.com',NULL,NULL,NULL,'',NULL,0,0,1,'2023-02-22 01:40:03',NULL,NULL);
INSERT INTO `sys_user` VALUES (32,'tony','9d60e44e87dedc09476161d7764a5381','9108f7b5cecd49a78d5fecff21d997fb','tony',1,2,1,NULL,NULL,'18900002222','13400000001@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:41:31',NULL,NULL);
INSERT INTO `sys_user` VALUES (33,'yp','8082ae129b31d31ab60cf9496a4eeac5','bc440cb822324fda9fcd6b64ef46fbf7','yp',1,4,0,NULL,NULL,'18900000000','yp@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-04-05 22:50:52','2023-04-19 17:03:58',1);
INSERT INTO `sys_user` VALUES (34,'yp1','2caa82251c047aac1562c22600d30121','116f35c84d344addb7d7bb5f5b972b13','yp1',1,4,1,NULL,NULL,'15789098767','yp@qq.com',NULL,NULL,NULL,'',NULL,0,1,33,'2023-04-18 02:09:21','2023-06-18 22:58:34',1);
INSERT INTO `sys_user` VALUES (35,'yp2','41edd5c624bb664854da6facdfa8a640','8d53855246d74a82bb46f30d0cdb3816','yp2',2,4,1,NULL,NULL,'15789098767','yp@qq.com',NULL,NULL,NULL,'',NULL,0,1,33,'2023-04-18 02:09:42','2023-06-18 05:05:06',1);
INSERT INTO `sys_user` VALUES (49,'zhangsan','d8213a568c0221a7a6c2a1d013e5a05d','d11095555f7c4563ad0b6af1e877fbc9','zhangsan',11,4,1,NULL,NULL,'13980809876','13400000001@qq.com',NULL,NULL,NULL,'1111',NULL,0,1,1,'2023-06-25 15:59:21',NULL,NULL);
INSERT INTO `sys_user` VALUES (50,'zhaoyanyan','daa41f1521c3816a500ae0f3bb899df5','f26fab5fec4447a99896092e9cd45ff0','zyy',1,4,0,NULL,NULL,'18897899878','13400000001@qq.com',NULL,NULL,NULL,'',NULL,0,1,1,'2023-07-05 08:39:13','2023-07-05 09:55:32',50);
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
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (4,1,1,1,'2022-12-26 17:17:36');
INSERT INTO `sys_user_role` VALUES (7,27,2,1,'2023-02-22 01:38:14');
INSERT INTO `sys_user_role` VALUES (8,28,2,1,'2023-02-22 01:40:03');
INSERT INTO `sys_user_role` VALUES (12,32,1,1,'2023-04-05 22:41:31');
INSERT INTO `sys_user_role` VALUES (13,33,1,1,'2023-04-05 22:50:52');
INSERT INTO `sys_user_role` VALUES (14,34,6,33,'2023-04-18 02:09:21');
INSERT INTO `sys_user_role` VALUES (15,35,8,33,'2023-04-18 02:09:42');
INSERT INTO `sys_user_role` VALUES (29,49,8,1,'2023-06-25 15:59:21');
INSERT INTO `sys_user_role` VALUES (30,50,1,1,'2023-07-05 08:39:13');
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
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm_setting`
--

LOCK TABLES `t_alarm_setting` WRITE;
/*!40000 ALTER TABLE `t_alarm_setting` DISABLE KEYS */;
INSERT INTO `t_alarm_setting` VALUES (12,'AGV电量水平低于30%',0,'LOWER_THAN_30','AGV电量水平低于30%',1,0,0,1,'2023-02-27 16:26:42','2023-07-04 09:48:56',1,57,'DRILL_REQUEST_LOAD_RAW_MATERIAL','11','大屏告警','AGV电量水平低于30%','0');
INSERT INTO `t_alarm_setting` VALUES (14,'AGV电量水平低于20%',0,'LOWER_THAN_20','AGV电量水平低于20%',2,0,1,1,'2023-02-27 16:26:42','2023-07-04 09:48:22',1,61,'DRILL_REQUEST_LOAD_RAW_MATERIAL','12','短信',' AGV电量水平低于20%','0');
INSERT INTO `t_alarm_setting` VALUES (15,'AGV电量水平低于10%',0,'LOWER_THAN_10','AGV电量水平低于10%',3,0,1,1,'2023-02-27 16:26:42','2023-07-04 09:48:47',1,38,'AGV充电','12,21','短信,微信','AGV电量水平低于10%','0');
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
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8mb4 COMMENT='检验记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_check_records`
--

LOCK TABLES `t_check_records` WRITE;
/*!40000 ALTER TABLE `t_check_records` DISABLE KEYS */;
INSERT INTO `t_check_records` VALUES (58,362,'TO20230621000010','TO20230621000010',75,'MO20230621000001','物料',NULL,'yp1','1','2023-06-21 15:28:00',NULL,1,0,1,'2023-06-21 15:28:00',NULL,NULL);
INSERT INTO `t_check_records` VALUES (59,360,'TO20230621000008','TO20230621000008',75,'MO20230621000001','物料',1,'admin','1','2023-06-26 09:11:09',NULL,1,0,1,'2023-06-21 15:28:02',NULL,NULL);
INSERT INTO `t_check_records` VALUES (60,354,'TO20230621000002','TO20230621000002',75,'MO20230621000001','物料',1,'admin','0','2023-06-25 16:48:07',NULL,1,0,1,'2023-06-25 10:49:25',NULL,NULL);
INSERT INTO `t_check_records` VALUES (61,371,'34132434','124322132',76,'MO20230621000002','王老板2023/6/21需求3000',1,'admin','1','2023-06-26 09:15:49',NULL,1,0,1,'2023-06-25 16:41:18',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8mb4 COMMENT='客户表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_client`
--

LOCK TABLES `t_client` WRITE;
/*!40000 ALTER TABLE `t_client` DISABLE KEYS */;
INSERT INTO `t_client` VALUES (55,'KH20230621000001','张老板','XX公司',0,0,1,'2023-06-06 16:08:10','2023-06-21 13:20:59',1);
INSERT INTO `t_client` VALUES (56,'KH20230613000001','王老板','11111',0,1,1,'2023-06-13 01:33:42',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_detail`
--

LOCK TABLES `t_cutter_config_detail` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_detail` DISABLE KEYS */;
INSERT INTO `t_cutter_config_detail` VALUES (54,3,2.000,2.000,2.000,2.000,2.000,2,0,1,1,'2023-05-04 22:33:20','2023-06-21 15:29:02',1);
INSERT INTO `t_cutter_config_detail` VALUES (56,16,4.040,1.000,1.000,1.000,1.000,1,0,1,1,'2023-05-04 22:48:20',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (57,16,3.110,4.000,5.000,1.000,1.000,1,0,1,1,'2023-05-04 22:48:36','2023-06-13 01:39:04',1);
INSERT INTO `t_cutter_config_detail` VALUES (58,16,1.000,1.000,1.000,1.000,1.000,1,0,1,1,'2023-06-13 01:39:47',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (59,17,3.000,3.000,2.000,3.000,2.000,3,0,1,1,'2023-06-17 21:42:57',NULL,NULL);
INSERT INTO `t_cutter_config_detail` VALUES (62,18,0.100,0.100,0.100,0.100,0.100,1,0,1,1,'2023-06-27 09:05:23',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔刀具参数主表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_cutter_config_master`
--

LOCK TABLES `t_cutter_config_master` WRITE;
/*!40000 ALTER TABLE `t_cutter_config_master` DISABLE KEYS */;
INSERT INTO `t_cutter_config_master` VALUES (3,'主轴转速200KRPM钻孔','主轴转速200KRPM钻孔','生产工单模板 (2).xlsx','/root/admin1.0/data/dia/生产工单模板 (2).xlsx',8,'p05','柔性电路板',0,1,1,'2023-03-28 16:38:31','2023-07-04 16:09:59',1);
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
  `device_type_code` varchar(50) DEFAULT NULL COMMENT '设备类型代码',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=58 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
INSERT INTO `t_device` VALUES (11,'drill01','drill01',NULL,0,0,1,1,'2023-02-28 11:40:13','2023-04-20 22:29:22',1,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,'{\n        \"ProductId\": \"agv\",\n        \"DeviceId\": \"AGV01\",\n        \"DeviceName\": \"AGV#01\",\n        \"DeviceClazz\": \"VgAutoDrill.Fundation.Vendor.Agvsz.AGV2530\",\n        \"AutoMode\": true,\n        \"DebugMode\": false,\n        \"DataCollectingPerSeconds\": 30,\n        \"KeepingPlcConnectionPerSeconds\": 30,\n        \"DeviceKind\": \"Auxiliary\",\n        \"InputCapabilities\": [ \"EmptySiloBox\", \"Raw\" ],\n        \"OutputCapabilities\": [ \"PRE_DRILL_TRANSFER_AGV_OUTPUT_1\", \"EmptySiloBox\" ],\n        \"Extra\": {\n          \"modbusTcpUri\": \"http://192.168.3.20:502\",\n          \"modbusTcpSlaveId\": 1,\n          \"MoveTimeout\": 30000,\n          \"PostAndGetTimeout\": 10000,\n          \"LowBattery\": 30,\n          \"AGVMoveStart\": \"http://192.168.3.15:9502/api/wcstask/AddTask\",\n          \"CarAllInfo\": \"http://192.168.3.15:9701/api/v2/CarState/GetCarStates\",\n          \"RequestCharge\": \"http://192.168.3.15:9701/api/v2/CarState/SetToCharge\"\n        }\n      }','3',NULL);
INSERT INTO `t_device` VALUES (13,'agv01','agv01',NULL,0,0,1,1,'2023-02-28 11:40:21','2023-04-20 22:28:54',1,8,0,'string','string',234,NULL,NULL,NULL,NULL,'111111111','3',NULL);
INSERT INTO `t_device` VALUES (14,'MockAgv02','MockAgv02',NULL,0,0,0,1,'2023-03-03 00:33:58','2023-07-03 15:16:03',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (15,'MockDrill01','MockDrill01',NULL,0,0,1,1,'2023-03-03 00:34:24','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (16,'P0001','P0001',NULL,0,0,1,1,'2023-03-03 01:32:19','2023-03-08 16:46:09',NULL,14,NULL,NULL,NULL,218,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (17,'MockAgv06','MockAgv06',NULL,0,0,2,1,'2023-03-07 10:20:48','2023-03-07 10:54:07',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (18,'MockAgv03','MockAgv03',NULL,0,0,1,1,'2023-03-07 10:21:29','2023-03-17 10:37:59',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (19,'MockAgv04','MockAgv04',NULL,0,0,1,1,'2023-03-07 17:09:12','2023-03-15 15:39:26',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'4',NULL);
INSERT INTO `t_device` VALUES (20,'P0002','P0002',NULL,0,0,1,1,'2023-03-07 20:19:06','2023-04-23 09:50:35',1,14,NULL,NULL,NULL,218,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (21,'MockAgv104','MockAgv104',NULL,0,0,0,1,'2023-03-09 10:15:32','2023-04-20 22:28:40',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1',NULL);
INSERT INTO `t_device` VALUES (22,'MockAgv103','MockAgv103',NULL,0,0,1,1,'2023-03-09 10:15:32','2023-03-22 07:52:58',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (23,'MockDrill02','MockDrill02',NULL,0,0,1,1,'2023-03-13 13:54:18','2023-03-22 07:52:58',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (24,'MockDrill03','MockDrill03',NULL,0,0,1,1,'2023-03-14 08:48:10','2023-03-20 11:46:56',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (25,'MockAgv01','MockAgv01',NULL,0,0,1,1,'2023-03-14 08:51:18','2023-04-17 11:34:58',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (26,'MockDrill04','MockDrill04',NULL,0,0,1,1,'2023-03-14 09:22:53','2023-03-20 11:46:56',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'1',NULL);
INSERT INTO `t_device` VALUES (27,'MockDrill05','MockDrill05',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:03',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (28,'MockDrill06','MockDrill06',NULL,0,0,1,1,'2023-03-14 09:22:54','2023-03-14 11:21:02',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (29,'Processed0001','Processed0001',NULL,0,0,1,1,'2023-03-14 11:19:38','2023-03-16 10:25:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1',NULL);
INSERT INTO `t_device` VALUES (30,'Processed10001','Processed10001',NULL,0,0,1,1,'2023-03-14 15:50:43','2023-03-20 14:21:40',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (31,'vegaDrill001','vegaDrill001',NULL,0,0,1,1,'2023-03-14 16:18:04','2023-03-14 16:27:45',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (32,'MockAgv1003','MockAgv1003',NULL,0,0,1,1,'2023-03-14 16:28:34','2023-03-14 16:33:38',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (33,'vegaDrill0601','vegaDrill0601',NULL,0,0,1,1,'2023-03-14 16:28:35','2023-03-14 16:33:37',NULL,7,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'4',NULL);
INSERT INTO `t_device` VALUES (34,'MockAgv11103','MockAgv11103',NULL,0,0,1,1,'2023-03-14 16:52:45','2023-03-15 08:43:45',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'1',NULL);
INSERT INTO `t_device` VALUES (35,'MockTonyAgv001','MockTonyAgv001',NULL,0,0,1,1,'2023-03-15 09:41:29','2023-03-22 23:45:47',1,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (36,'MockSimpleAgv001','MockSimpleAgv001',NULL,0,0,1,1,'2023-03-15 10:12:33','2023-03-16 14:30:31',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (37,'MockSimpleAgv002','MockSimpleAgv002',NULL,0,0,1,1,'2023-03-15 10:24:25','2023-03-15 10:36:38',NULL,8,NULL,NULL,NULL,234,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (38,'Processed002','Processed002',NULL,0,0,1,1,'2023-03-15 10:24:26','2023-03-15 10:36:38',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (39,'Raw10001','Raw10001',NULL,0,0,1,1,'2023-03-15 14:07:38','2023-03-15 14:28:20',NULL,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1',NULL);
INSERT INTO `t_device` VALUES (40,'Processed100012','Processed100012',NULL,0,0,1,1,'2023-03-15 14:32:46','2023-03-22 07:52:57',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (41,'Raw100012','Raw100012',NULL,0,0,1,1,'2023-03-15 14:32:47','2023-03-22 07:52:57',NULL,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (42,'Processed1000122','Processed1000122',NULL,0,0,1,1,'2023-03-15 16:21:35','2023-06-21 15:22:37',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'1',NULL);
INSERT INTO `t_device` VALUES (43,'Raw1000123','Raw1000123',NULL,0,0,1,1,'2023-03-15 16:21:36','2023-03-23 01:42:59',1,16,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (45,'Processed10002','Processed10002',NULL,0,0,1,1,'2023-03-20 14:16:50','2023-03-22 23:46:24',1,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (46,'SiloShelf100012','SiloShelf100012',NULL,0,0,1,1,'2023-03-21 07:01:46','2023-03-22 07:52:57',NULL,17,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,'0',NULL);
INSERT INTO `t_device` VALUES (49,'UP000102','UP000102',NULL,0,0,1,1,'2023-03-22 07:51:36','2023-03-22 07:52:57',NULL,15,NULL,NULL,NULL,219,NULL,NULL,NULL,NULL,NULL,'2',NULL);
INSERT INTO `t_device` VALUES (51,'drill01-0001','钻机01-0001',NULL,0,0,1,1,'2023-03-22 23:02:07','2023-06-14 16:35:54',1,7,NULL,NULL,NULL,212,1,'2023-06-13 11:12:26','2023-06-15',NULL,NULL,'3',NULL);
INSERT INTO `t_device` VALUES (52,'drill01-0002','钻机01-0002',NULL,0,0,1,1,'2023-03-22 23:16:16','2023-03-23 01:36:23',1,7,NULL,NULL,NULL,217,2,'2023-06-13 11:12:30','2023-06-15',NULL,NULL,'4',NULL);
INSERT INTO `t_device` VALUES (54,'drill01-0003','钻机01-0003',NULL,0,0,1,1,'2023-03-23 01:19:33','2023-03-23 02:21:09',1,7,NULL,NULL,NULL,233,3,'2023-06-13 11:12:33','2023-06-15',NULL,NULL,'0',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COMMENT='维护记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_maintain`
--

LOCK TABLES `t_device_maintain` WRITE;
/*!40000 ALTER TABLE `t_device_maintain` DISABLE KEYS */;
INSERT INTO `t_device_maintain` VALUES (6,14,'drill01-0001','钻机01-0001','2023-05-24 04:37:43',NULL,'yp1','1','',0,1,1,'2023-05-17 01:38:29','2023-06-25 11:39:17',1);
INSERT INTO `t_device_maintain` VALUES (7,16,'drill01-0002','钻机01-0002','2023-05-17 00:00:00',NULL,'admin','1','',0,1,1,'2023-05-17 05:22:21','2023-06-25 11:39:06',1);
INSERT INTO `t_device_maintain` VALUES (8,14,'drill01-0003','钻机01-0003','2023-05-24 02:22:52',NULL,'yp','-1','',0,1,1,'2023-05-23 22:22:32','2023-06-25 11:39:10',1);
INSERT INTO `t_device_maintain` VALUES (11,13,'agv01','agv01','2023-06-20 16:10:42',0,'string','0','',0,1,1,'2023-06-20 16:01:01','2023-06-25 11:39:13',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COMMENT='设备维护明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_maintain_detail`
--

LOCK TABLES `t_device_maintain_detail` WRITE;
/*!40000 ALTER TABLE `t_device_maintain_detail` DISABLE KEYS */;
INSERT INTO `t_device_maintain_detail` VALUES (3,6,11,1,'subject1',NULL,'正常','无','',0,1,1,'2023-05-15 11:40:13',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (4,2,13,2,'subject2',NULL,'无法校正','重新对应 点位','',0,1,1,'2023-05-15 11:40:13',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (5,6,0,5,'11','11','11','11',NULL,0,1,1,'2023-05-17 05:20:29',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (6,6,14,5,'11','11','111','222',NULL,0,1,1,'2023-05-17 05:22:06',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (8,7,16,5,'11','11','222','2222',NULL,0,1,1,'2023-05-17 20:56:36',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (9,8,14,4,'subject1','subject1',NULL,NULL,NULL,0,1,1,'2023-06-13 01:47:03',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (10,9,51,5,'11','11',NULL,NULL,NULL,0,1,1,'2023-06-16 04:11:38',NULL,NULL);
INSERT INTO `t_device_maintain_detail` VALUES (11,11,13,6,'1','总进气压',NULL,NULL,NULL,0,1,1,'2023-06-25 11:39:29',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备负载板料表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_panel`
--

LOCK TABLES `t_device_panel` WRITE;
/*!40000 ALTER TABLE `t_device_panel` DISABLE KEYS */;
INSERT INTO `t_device_panel` VALUES (2,14,'MockAgv02','P202304210008','string','string','string','string',0,0,100,NULL,1,0,1,'2023-04-26 15:54:31',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (3,13,'agv01','P202304210007','string','string','string','string',0,0,100,NULL,1,0,1,'2023-04-26 12:02:55',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (4,11,'drill01','P20230518000008',NULL,NULL,NULL,NULL,NULL,NULL,300,NULL,1,0,1,'2023-04-28 08:15:23',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (5,15,'MockDrill01','P202304210006',NULL,NULL,NULL,NULL,NULL,NULL,300,NULL,1,0,1,'2023-04-27 10:25:11',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (6,23,'MockDrill02','P202304210005',NULL,NULL,NULL,NULL,NULL,NULL,300,NULL,1,0,1,'2023-04-27 10:34:46',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (7,49,'UP000102','P202304210004',NULL,NULL,NULL,NULL,NULL,NULL,400,1,1,0,1,'2023-04-28 12:19:11',NULL,NULL);
INSERT INTO `t_device_panel` VALUES (8,16,'P0001','P202304210003',NULL,NULL,NULL,NULL,NULL,NULL,200,NULL,1,0,1,'2023-04-28 15:19:11',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备负载板料历史表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_panel_history`
--

LOCK TABLES `t_device_panel_history` WRITE;
/*!40000 ALTER TABLE `t_device_panel_history` DISABLE KEYS */;
INSERT INTO `t_device_panel_history` VALUES (12,14,'MockAgv02','P20230518000008',NULL,NULL,NULL,NULL,NULL,NULL,100,0,'2023-04-28 06:15:23',1,0,1,'2023-04-28 07:15:23',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (13,16,'P0001','P20230518000008',NULL,NULL,NULL,NULL,NULL,NULL,200,0,'2023-04-28 07:15:23',1,0,NULL,'2023-04-28 08:15:23',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (14,13,'agv01','P202304210006',NULL,NULL,NULL,NULL,NULL,NULL,100,0,'2023-04-27 09:25:11',1,0,NULL,'2023-04-27 09:40:11',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (15,16,'P0001','P202304210006',NULL,NULL,NULL,NULL,NULL,NULL,200,0,'2023-04-27 09:40:11',1,0,NULL,'2023-04-27 10:25:11',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (16,14,'MockAgv02','P202304210005',NULL,NULL,NULL,NULL,NULL,NULL,100,0,'2023-04-27 07:34:46',1,0,NULL,'2023-04-27 08:34:46',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (17,16,'P0001','P202304210005',NULL,NULL,NULL,NULL,NULL,NULL,200,0,'2023-04-27 08:34:46',1,0,NULL,'2023-04-27 09:34:46',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (18,13,'agv01','P202304210004',NULL,NULL,NULL,NULL,NULL,NULL,100,0,'2023-04-28 08:19:11',1,0,NULL,'2023-04-28 09:19:11',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (19,16,'P0001','P202304210004',NULL,NULL,NULL,NULL,NULL,NULL,200,0,'2023-04-28 09:19:11',1,0,NULL,'2023-04-28 10:19:11',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (20,11,'drill01','P202304210004',NULL,NULL,NULL,NULL,NULL,NULL,300,0,'2023-04-28 10:19:11',1,0,NULL,'2023-04-28 12:19:11',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (21,14,'MockAgv02','P202304210003',NULL,NULL,NULL,NULL,NULL,NULL,100,0,'2023-04-28 14:19:11',1,0,NULL,'2023-04-28 15:19:11',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (22,14,'MockAgv02','P202304210002',NULL,NULL,NULL,NULL,NULL,NULL,100,0,'2023-04-27 01:55:37',1,0,NULL,'2023-04-27 02:55:37',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (23,16,'P0001','P202304210002',NULL,NULL,NULL,NULL,NULL,NULL,200,0,'2023-04-27 02:55:37',1,0,NULL,'2023-04-27 03:15:37',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (24,15,'MockDrill01','P202304210002',NULL,NULL,NULL,NULL,NULL,NULL,300,0,'2023-04-27 03:15:37',1,0,NULL,'2023-04-27 04:55:37',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (25,49,'UP000102','P202304210002',NULL,NULL,NULL,NULL,NULL,NULL,400,1,'2023-04-27 04:55:37',1,0,NULL,'2023-04-27 05:55:37',NULL,NULL);
INSERT INTO `t_device_panel_history` VALUES (26,14,'MockAgv02','P20230518000008',NULL,NULL,NULL,NULL,NULL,NULL,400,1,'2023-04-27 04:55:37',1,0,NULL,'2023-04-27 05:55:37',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=115 DEFAULT CHARSET=utf8mb4 COMMENT='设备点检项目模板';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_subject`
--

LOCK TABLES `t_device_subject` WRITE;
/*!40000 ALTER TABLE `t_device_subject` DISABLE KEYS */;
INSERT INTO `t_device_subject` VALUES (25,15,4,0,1,1,'2023-05-17 15:06:02',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (26,15,5,0,1,1,'2023-05-17 15:06:02',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (32,16,5,0,1,1,'2023-05-17 03:16:33',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (79,20,4,0,1,1,'2023-05-17 17:03:25',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (80,20,5,0,1,1,'2023-05-17 17:03:57',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (83,11,5,0,1,1,'2023-05-17 05:15:05',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (84,11,4,0,1,33,'2023-05-17 05:16:00',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (85,14,5,0,1,1,'2023-06-16 05:14:03',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (90,18,6,0,1,1,'2023-07-03 14:29:36',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (91,18,7,0,1,1,'2023-07-03 14:29:36',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (92,18,8,0,1,1,'2023-07-03 14:29:36',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (93,18,9,0,1,1,'2023-07-03 14:29:36',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (94,18,10,0,1,1,'2023-07-03 14:29:36',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (95,18,11,0,1,1,'2023-07-03 14:29:36',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (105,11,6,0,1,1,'2023-07-04 11:48:56',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (106,11,9,0,1,1,'2023-07-04 11:48:56',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (107,11,10,0,1,1,'2023-07-04 11:48:56',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (108,11,7,0,1,1,'2023-07-04 11:48:56',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (109,11,8,0,1,1,'2023-07-04 11:48:56',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (113,14,8,0,1,1,'2023-07-04 11:49:08',NULL,NULL);
INSERT INTO `t_device_subject` VALUES (114,14,10,0,1,1,'2023-07-04 11:49:08',NULL,NULL);
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
INSERT INTO `t_device_type` VALUES (7,'钻机',1,0,1,1,'2023-02-28 10:26:41','2023-06-19 14:02:09',1,'drill','0,1');
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
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COMMENT='钻孔工单表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_drill_work_order`
--

LOCK TABLES `t_drill_work_order` WRITE;
/*!40000 ALTER TABLE `t_drill_work_order` DISABLE KEYS */;
INSERT INTO `t_drill_work_order` VALUES (51,100,'MO20230629000001',68,'IF20230620000005A1',6.00,1200.00,200.00,6.00,34.00,33.00,10000.00,40.00,1360.00,0.00,3.00,0,1,1,'2023-06-29 09:54:37','2023-06-30 13:47:04',1);
INSERT INTO `t_drill_work_order` VALUES (52,100,'MO20230629000001',68,'IF20230620000005A1',6.00,1200.00,200.00,6.00,34.00,34.00,10000.00,40.00,1360.00,0.00,2.00,0,1,1,'2023-07-01 13:36:15',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COMMENT='编码生成规则';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_encode_build_rules`
--

LOCK TABLES `t_encode_build_rules` WRITE;
/*!40000 ALTER TABLE `t_encode_build_rules` DISABLE KEYS */;
INSERT INTO `t_encode_build_rules` VALUES (1,'UNITMEASURE_CODE','计量单位编码','JL',6,1,'DW','',0,1,1,'2023-05-18 02:58:43','2023-07-04 17:02:18',1);
INSERT INTO `t_encode_build_rules` VALUES (2,'ITEMTYPE_CODE','物料产品分类编码','IT',6,1,'BM','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (3,'ITEM_CODE','物料产品编码','IF',6,1,'A1','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (4,'WORKSTATION_CODE','工作站编码','WS',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (5,'CLIENT_CODE','客户管理编码','KH',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (6,'VENDOR_CODE','供应商编码','GYS',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (7,'WORKORDER_CODE','生产工单分类编码','MO',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (8,'PANEL_CODE','板料追溯编码','P',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (9,'TASK_CODE','任务编码','TO',6,1,'','',0,1,1,'2023-05-18 02:58:43',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (10,'WAREHOUSE_CODE','仓库编码','WH',6,1,NULL,'',0,1,1,'2023-05-18 02:58:43','2023-06-28 14:51:46',1);
INSERT INTO `t_encode_build_rules` VALUES (12,'MATERIALSTOCK_IN_CODE','库存入库','MSIN',3,1,NULL,NULL,0,1,1,'2023-06-19 15:52:32','2023-06-19 15:53:24',1);
INSERT INTO `t_encode_build_rules` VALUES (13,'MATERIALSTOCK_OUT_CODE','库存出库','MSOUT',5,1,NULL,NULL,0,1,1,'2023-06-19 15:53:18',NULL,NULL);
INSERT INTO `t_encode_build_rules` VALUES (14,'WORKSHOP_CODE','车间编码','CJ',6,0,'C','',0,1,1,'2023-06-19 15:53:18','2023-06-28 11:06:10',1);
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
INSERT INTO `t_event_define` VALUES (47,'AGV_REQUEST_CHARGE_EVENT','AGV充电',2,'',1,'2023-05-23 03:12:27','2023-06-28 15:10:51',1,NULL,0,0);
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
) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb4 COMMENT='生产报工记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_feedback`
--

LOCK TABLES `t_feedback` WRITE;
/*!40000 ALTER TABLE `t_feedback` DISABLE KEYS */;
INSERT INTO `t_feedback` VALUES (74,'统一报工',212,'drill01-0001','钻机01-0001',75,'MO20230621000001','物料',22,'drill','钻孔',354,'TO20230621000002',67,'MO20230621000001','物料',40,'叠板2层','叠板数2',0.00,0.00,0.00,0.00,'yp2','tony',NULL,'2023-06-25 00:00:00',NULL,'COMMITED',NULL,0,1,1,'2023-06-25 09:45:22','2023-06-30 13:45:14',1);
INSERT INTO `t_feedback` VALUES (75,'自行报工',212,'drill01-0001','钻机01-0001',75,'MO20230621000001','物料',22,'drill','钻孔',354,'TO20230621000002',67,'MO20230621000001','物料',40,'叠板2层','叠板数2',0.00,143.00,122.00,21.00,'yp2','yp',NULL,'2023-06-25 00:00:00',NULL,'COMMITED',NULL,0,1,1,'2023-06-25 09:45:34','2023-06-30 13:45:59',1);
INSERT INTO `t_feedback` VALUES (76,'自行报工',212,'drill01-0001','钻机01-0001',75,'MO20230621000001','物料',22,'drill','钻孔',354,'TO20230621000002',67,'MO20230621000001','物料',40,'叠板2层','叠板数2',0.00,230.00,200.00,30.00,'yp1','yp',NULL,'2023-06-25 00:00:00',NULL,'COMMITED',NULL,0,1,1,'2023-06-25 13:14:09','2023-06-30 13:47:04',1);
INSERT INTO `t_feedback` VALUES (79,'自行报工',233,'drill01-0003','钻机01-0003',100,'MO20230629000001','6/29测试001',24,'drill','钻孔',510,'TO20230629000001',68,'MO20230629000001','6/29测试001',40,'叠板2层','叠板数2',0.00,32.00,11.00,21.00,'yp1','yp',NULL,'2023-06-30 00:00:00',NULL,'COMMITED',NULL,0,1,1,'2023-06-30 13:39:01','2023-06-30 13:47:04',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品定义表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item`
--

LOCK TABLES `t_item` WRITE;
/*!40000 ALTER TABLE `t_item` DISABLE KEYS */;
INSERT INTO `t_item` VALUES (36,'PCB单层板',0,'IF2023040500002','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-04-05 01:25:38','2023-07-04 17:07:31',1,81,'P08-01','高速电路板--客户001指定',NULL,NULL,'WareHouse3');
INSERT INTO `t_item` VALUES (60,'铜单面板',0,'IF2023042300001','DDD XXXX DD','单层Panel',1,25,'0',0,1,33,'2023-04-23 02:27:08','2023-04-23 02:27:50',33,7,'p01','单面板',NULL,NULL,'WareHouse');
INSERT INTO `t_item` VALUES (63,'PCB单层板',35,'IF2023040500002','DDD XXXX DD','Panel',1,22,NULL,0,0,1,'2023-05-09 03:03:02',NULL,NULL,6,'产品大类编码','产品大类名称',6,'库房编码','库房名称');
INSERT INTO `t_item` VALUES (67,'板材单钻孔（叠板数2）',0,'IF20230620000002A1','叠板数2','叠板2层',2,40,'0',0,1,32,'2023-06-20 10:42:30',NULL,NULL,74,'P02','双面板',NULL,NULL,'成品一库');
INSERT INTO `t_item` VALUES (68,'板材单钻孔（叠板数2）',0,'IF20230620000005A1','叠板数2','叠板2层',2,40,'0',0,1,1,'2023-06-20 14:50:49','2023-07-04 17:07:20',1,74,'P02','双面板',NULL,NULL,'成品一库');
INSERT INTO `t_item` VALUES (70,'铜单面板',0,'IF2023042300001','DDD XXXX DD','单层Panel',1,25,'0',0,1,1,'2023-06-25 10:26:22',NULL,NULL,7,'p01','单面板',NULL,NULL,'WareHouse');
INSERT INTO `t_item` VALUES (73,'PCB单层板',0,'IF20230704000001A1','DDD XXXX DD','Panel',2,22,'0',0,1,1,'2023-07-04 17:07:12',NULL,NULL,81,'P08-01','高速电路板--客户001指定',NULL,NULL,'WareHouse3');
INSERT INTO `t_item` VALUES (74,'板材单钻孔（叠板数2）',0,'IF20230704000002A1','叠板数2','叠板2层',2,40,'0',0,1,1,'2023-07-04 17:07:37',NULL,NULL,74,'P02','双面板',NULL,NULL,'成品一库');
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
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_atp_file`
--

LOCK TABLES `t_item_atp_file` WRITE;
/*!40000 ALTER TABLE `t_item_atp_file` DISABLE KEYS */;
INSERT INTO `t_item_atp_file` VALUES (44,'IF2023040500002','PCB单层板',36,22,'ATP2963280','ATP7754144',NULL,NULL,'工序模板.xlsx','/root/admin1.0/data/dia/工序模板.xlsx',NULL,'告警记录.xlsx','/root/admin1.0/data/dia/告警记录.xlsx',4,4,4,1,'2023-06-26 16:43:12',0,1,1,'2023-05-04 23:11:14','2023-07-04 15:28:40',1);
INSERT INTO `t_item_atp_file` VALUES (45,'IF20230620000005A1','板材单钻孔（叠板数2）',68,40,'ATP9561633','ATP3378641',NULL,NULL,'1-23021Q5360J93.docx','/root/admin1.0/data/dia/1-23021Q5360J93.docx',NULL,'告警记录.xlsx','/root/admin1.0/data/dia/告警记录.xlsx',2,2,2,1,'2023-06-26 16:43:24',0,1,1,'2023-05-04 23:31:47','2023-07-04 15:14:29',1);
INSERT INTO `t_item_atp_file` VALUES (49,'IF20230620000002A1','板材单钻孔（叠板数2）',67,40,NULL,NULL,NULL,NULL,'1-23021Q5360J93.docx','/root/admin1.0/data/dia/1-23021Q5360J93.docx',NULL,'生产工单模板.xlsx','/root/admin1.0/data/dia/生产工单模板.xlsx',6,5,10,0,NULL,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=11361 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='ATP文件明细';
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
INSERT INTO `t_item_atp_file_detail` VALUES (6648,37,NULL,NULL,424,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6649,39,NULL,NULL,333,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6651,38,NULL,NULL,402,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6652,36,NULL,NULL,446,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6654,39,NULL,NULL,334,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6655,37,NULL,NULL,425,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6656,36,NULL,NULL,447,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6657,38,NULL,NULL,403,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6660,39,NULL,NULL,335,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6661,37,NULL,NULL,426,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6662,38,NULL,NULL,404,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6664,36,NULL,NULL,448,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6666,37,NULL,NULL,427,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6667,39,NULL,NULL,336,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6669,36,NULL,NULL,449,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6670,38,NULL,NULL,405,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6672,37,NULL,NULL,428,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6673,39,NULL,NULL,337,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6674,36,NULL,NULL,450,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6676,38,NULL,NULL,406,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6678,39,NULL,NULL,338,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6679,37,NULL,NULL,429,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6682,38,NULL,NULL,407,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6683,36,NULL,NULL,451,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6684,39,NULL,NULL,339,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6685,37,NULL,NULL,430,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (6686,36,NULL,NULL,452,0,1,1,'2023-04-20 04:58:15',NULL,NULL);
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
INSERT INTO `t_item_atp_file_detail` VALUES (9787,2,'',0.00,1,0,0,1,'2023-05-22 03:30:07','2023-06-13 01:40:27',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9788,2,'',0.00,2,0,0,1,'2023-05-22 03:30:07','2023-06-13 01:40:27',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9789,46,NULL,NULL,1,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9790,46,NULL,NULL,2,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9791,46,NULL,NULL,3,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9792,46,NULL,NULL,4,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9793,46,NULL,NULL,5,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9794,46,NULL,NULL,6,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9795,46,NULL,NULL,7,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9796,46,'T02',0.85,8,0,0,1,'2023-06-16 05:09:31','2023-06-19 17:28:09',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9797,46,'T02',0.85,9,0,0,1,'2023-06-16 05:09:31','2023-06-19 17:28:09',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9798,46,'T02',0.85,10,0,0,1,'2023-06-16 05:09:31','2023-06-19 17:28:09',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9799,46,NULL,NULL,11,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9800,46,NULL,NULL,12,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9801,46,NULL,NULL,13,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9802,46,NULL,NULL,14,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9803,46,NULL,NULL,15,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9804,46,NULL,NULL,16,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9805,46,NULL,NULL,17,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9806,46,NULL,NULL,18,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9807,46,NULL,NULL,19,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9808,46,NULL,NULL,20,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9809,46,NULL,NULL,21,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9810,46,NULL,NULL,22,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9811,46,NULL,NULL,23,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9812,46,NULL,NULL,24,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9813,46,NULL,NULL,25,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9814,46,NULL,NULL,26,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9815,46,NULL,NULL,27,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9816,46,NULL,NULL,28,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9817,46,NULL,NULL,29,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9818,46,NULL,NULL,30,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9819,46,NULL,NULL,31,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9820,46,NULL,NULL,32,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9821,46,NULL,NULL,33,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9822,46,NULL,NULL,34,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9823,46,NULL,NULL,35,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9824,46,NULL,NULL,36,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9825,46,NULL,NULL,37,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9826,46,NULL,NULL,38,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9827,46,NULL,NULL,39,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9828,46,NULL,NULL,40,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9829,46,NULL,NULL,41,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9830,46,NULL,NULL,42,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9831,46,NULL,NULL,43,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9832,46,NULL,NULL,44,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9833,46,NULL,NULL,45,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9834,46,NULL,NULL,46,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9835,46,NULL,NULL,47,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9836,46,'T01',0.81,48,0,0,1,'2023-06-16 05:09:31','2023-06-19 17:26:02',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9837,46,'T01',0.81,49,0,0,1,'2023-06-16 05:09:31','2023-06-19 17:26:02',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9838,46,'T01',0.81,50,0,0,1,'2023-06-16 05:09:31','2023-06-19 17:26:02',1);
INSERT INTO `t_item_atp_file_detail` VALUES (9839,46,NULL,NULL,51,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9840,46,NULL,NULL,52,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9841,46,NULL,NULL,53,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9842,46,NULL,NULL,54,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9843,46,NULL,NULL,55,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9844,46,NULL,NULL,56,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9845,46,NULL,NULL,57,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9846,46,NULL,NULL,58,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9847,46,NULL,NULL,59,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9848,46,NULL,NULL,60,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9849,46,NULL,NULL,61,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9850,46,NULL,NULL,62,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9851,46,NULL,NULL,63,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9852,46,NULL,NULL,64,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9853,46,NULL,NULL,65,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9854,46,NULL,NULL,66,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9855,46,NULL,NULL,67,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9856,46,NULL,NULL,68,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9857,46,NULL,NULL,69,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9858,46,NULL,NULL,70,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9859,46,NULL,NULL,71,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9860,46,NULL,NULL,72,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9861,46,NULL,NULL,73,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9862,46,NULL,NULL,74,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9863,46,NULL,NULL,75,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9864,46,NULL,NULL,76,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9865,46,NULL,NULL,77,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9866,46,NULL,NULL,78,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9867,46,NULL,NULL,79,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9868,46,NULL,NULL,80,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9869,46,NULL,NULL,81,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9870,46,NULL,NULL,82,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9871,46,NULL,NULL,83,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9872,46,NULL,NULL,84,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9873,46,NULL,NULL,85,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9874,46,NULL,NULL,86,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9875,46,NULL,NULL,87,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9876,46,NULL,NULL,88,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9877,46,NULL,NULL,89,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9878,46,NULL,NULL,90,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9879,46,NULL,NULL,91,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9880,46,NULL,NULL,92,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9881,46,NULL,NULL,93,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9882,46,NULL,NULL,94,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9883,46,NULL,NULL,95,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9884,46,NULL,NULL,96,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9885,46,NULL,NULL,97,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9886,46,NULL,NULL,98,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9887,46,NULL,NULL,99,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9888,46,NULL,NULL,100,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9889,46,NULL,NULL,101,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9890,46,NULL,NULL,102,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9891,46,NULL,NULL,103,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9892,46,NULL,NULL,104,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9893,46,NULL,NULL,105,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9894,46,NULL,NULL,106,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9895,46,NULL,NULL,107,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9896,46,NULL,NULL,108,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9897,46,NULL,NULL,109,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9898,46,NULL,NULL,110,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9899,46,NULL,NULL,111,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9900,46,NULL,NULL,112,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9901,46,NULL,NULL,113,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9902,46,NULL,NULL,114,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9903,46,NULL,NULL,115,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9904,46,NULL,NULL,116,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9905,46,NULL,NULL,117,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9906,46,NULL,NULL,118,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9907,46,NULL,NULL,119,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9908,46,NULL,NULL,120,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9909,46,NULL,NULL,121,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9910,46,NULL,NULL,122,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9911,46,NULL,NULL,123,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9912,46,NULL,NULL,124,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9913,46,NULL,NULL,125,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9914,46,NULL,NULL,126,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9915,46,NULL,NULL,127,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9916,46,NULL,NULL,128,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9917,46,NULL,NULL,129,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9918,46,NULL,NULL,130,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9919,46,NULL,NULL,131,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9920,46,NULL,NULL,132,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9921,46,NULL,NULL,133,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9922,46,NULL,NULL,134,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9923,46,NULL,NULL,135,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9924,46,NULL,NULL,136,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9925,46,NULL,NULL,137,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9926,46,NULL,NULL,138,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9927,46,NULL,NULL,139,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9928,46,NULL,NULL,140,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9929,46,NULL,NULL,141,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9930,46,NULL,NULL,142,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9931,46,NULL,NULL,143,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9932,46,NULL,NULL,144,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9933,46,NULL,NULL,145,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9934,46,NULL,NULL,146,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9935,46,NULL,NULL,147,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9936,46,NULL,NULL,148,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9937,46,NULL,NULL,149,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9938,46,NULL,NULL,150,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9939,46,NULL,NULL,151,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9940,46,NULL,NULL,152,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9941,46,NULL,NULL,153,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9942,46,NULL,NULL,154,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9943,46,NULL,NULL,155,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9944,46,NULL,NULL,156,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9945,46,NULL,NULL,157,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9946,46,NULL,NULL,158,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9947,46,NULL,NULL,159,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9948,46,NULL,NULL,160,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9949,46,NULL,NULL,161,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9950,46,NULL,NULL,162,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9951,46,NULL,NULL,163,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9952,46,NULL,NULL,164,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9953,46,NULL,NULL,165,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9954,46,NULL,NULL,166,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9955,46,NULL,NULL,167,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9956,46,NULL,NULL,168,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9957,46,NULL,NULL,169,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9958,46,NULL,NULL,170,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9959,46,NULL,NULL,171,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9960,46,NULL,NULL,172,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9961,46,NULL,NULL,173,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9962,46,NULL,NULL,174,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9963,46,NULL,NULL,175,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9964,46,NULL,NULL,176,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9965,46,NULL,NULL,177,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9966,46,NULL,NULL,178,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9967,46,NULL,NULL,179,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9968,46,NULL,NULL,180,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9969,46,NULL,NULL,181,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9970,46,NULL,NULL,182,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9971,46,NULL,NULL,183,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9972,46,NULL,NULL,184,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9973,46,NULL,NULL,185,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9974,46,NULL,NULL,186,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9975,46,NULL,NULL,187,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9976,46,NULL,NULL,188,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9977,46,NULL,NULL,189,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9978,46,NULL,NULL,190,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9979,46,NULL,NULL,191,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9980,46,NULL,NULL,192,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9981,46,NULL,NULL,193,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9982,46,NULL,NULL,194,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9983,46,NULL,NULL,195,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9984,46,NULL,NULL,196,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9985,46,NULL,NULL,197,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9986,46,NULL,NULL,198,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9987,46,NULL,NULL,199,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9988,46,NULL,NULL,200,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9989,46,NULL,NULL,201,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9990,46,NULL,NULL,202,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9991,46,NULL,NULL,203,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9992,46,NULL,NULL,204,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9993,46,NULL,NULL,205,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9994,46,NULL,NULL,206,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9995,46,NULL,NULL,207,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9996,46,NULL,NULL,208,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9997,46,NULL,NULL,209,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9998,46,NULL,NULL,210,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (9999,46,NULL,NULL,211,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10000,46,NULL,NULL,212,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10001,46,NULL,NULL,213,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10002,46,NULL,NULL,214,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10003,46,NULL,NULL,215,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10004,46,NULL,NULL,216,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10005,46,NULL,NULL,217,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10006,46,NULL,NULL,218,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10007,46,NULL,NULL,219,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10008,46,NULL,NULL,220,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10009,46,NULL,NULL,221,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10010,46,NULL,NULL,222,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10011,46,NULL,NULL,223,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10012,46,NULL,NULL,224,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10013,46,NULL,NULL,225,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10014,46,NULL,NULL,226,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10015,46,NULL,NULL,227,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10016,46,NULL,NULL,228,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10017,46,NULL,NULL,229,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10018,46,NULL,NULL,230,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10019,46,NULL,NULL,231,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10020,46,NULL,NULL,232,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10021,46,NULL,NULL,233,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10022,46,NULL,NULL,234,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10023,46,NULL,NULL,235,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10024,46,NULL,NULL,236,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10025,46,NULL,NULL,237,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10026,46,NULL,NULL,238,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10027,46,NULL,NULL,239,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10028,46,NULL,NULL,240,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10029,46,NULL,NULL,241,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10030,46,NULL,NULL,242,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10031,46,NULL,NULL,243,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10032,46,NULL,NULL,244,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10033,46,NULL,NULL,245,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10034,46,NULL,NULL,246,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10035,46,NULL,NULL,247,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10036,46,NULL,NULL,248,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10037,46,NULL,NULL,249,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10038,46,NULL,NULL,250,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10039,46,NULL,NULL,251,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10040,46,NULL,NULL,252,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10041,46,NULL,NULL,253,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10042,46,NULL,NULL,254,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10043,46,NULL,NULL,255,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10044,46,NULL,NULL,256,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10045,46,NULL,NULL,257,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10046,46,NULL,NULL,258,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10047,46,NULL,NULL,259,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10048,46,NULL,NULL,260,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10049,46,NULL,NULL,261,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10050,46,NULL,NULL,262,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10051,46,NULL,NULL,263,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10052,46,NULL,NULL,264,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10053,46,NULL,NULL,265,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10054,46,NULL,NULL,266,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10055,46,NULL,NULL,267,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10056,46,NULL,NULL,268,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10057,46,NULL,NULL,269,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10058,46,NULL,NULL,270,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10059,46,NULL,NULL,271,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10060,46,NULL,NULL,272,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10061,46,NULL,NULL,273,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10062,46,NULL,NULL,274,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10063,46,NULL,NULL,275,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10064,46,NULL,NULL,276,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10065,46,NULL,NULL,277,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10066,46,NULL,NULL,278,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10067,46,NULL,NULL,279,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10068,46,NULL,NULL,280,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10069,46,NULL,NULL,281,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10070,46,NULL,NULL,282,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10071,46,NULL,NULL,283,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10072,46,NULL,NULL,284,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10073,46,NULL,NULL,285,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10074,46,NULL,NULL,286,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10075,46,NULL,NULL,287,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10076,46,NULL,NULL,288,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10077,46,NULL,NULL,289,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10078,46,NULL,NULL,290,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10079,46,NULL,NULL,291,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10080,46,NULL,NULL,292,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10081,46,NULL,NULL,293,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10082,46,NULL,NULL,294,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10083,46,NULL,NULL,295,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10084,46,NULL,NULL,296,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10085,46,NULL,NULL,297,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10086,46,NULL,NULL,298,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10087,46,NULL,NULL,299,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10088,46,NULL,NULL,300,0,1,1,'2023-06-16 05:09:31',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10089,47,NULL,NULL,1,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10090,47,NULL,NULL,2,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10091,47,NULL,NULL,3,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10092,47,NULL,NULL,4,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10093,47,NULL,NULL,5,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10094,47,NULL,NULL,6,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10095,47,NULL,NULL,7,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10096,47,NULL,NULL,8,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10097,47,NULL,NULL,9,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10098,47,'18',0.20,10,0,0,32,'2023-06-20 09:40:29','2023-06-20 09:40:46',32);
INSERT INTO `t_item_atp_file_detail` VALUES (10099,47,NULL,NULL,11,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10100,47,NULL,NULL,12,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10101,47,NULL,NULL,13,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10102,47,NULL,NULL,14,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10103,47,NULL,NULL,15,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10104,47,NULL,NULL,16,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10105,47,NULL,NULL,17,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10106,47,NULL,NULL,18,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10107,47,NULL,NULL,19,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10108,47,NULL,NULL,20,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10109,47,NULL,NULL,21,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10110,47,NULL,NULL,22,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10111,47,NULL,NULL,23,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10112,47,NULL,NULL,24,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10113,47,NULL,NULL,25,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10114,47,NULL,NULL,26,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10115,47,NULL,NULL,27,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10116,47,NULL,NULL,28,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10117,47,NULL,NULL,29,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10118,47,'22',0.50,30,0,0,32,'2023-06-20 09:40:29','2023-06-20 09:40:52',32);
INSERT INTO `t_item_atp_file_detail` VALUES (10119,47,NULL,NULL,31,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10120,47,NULL,NULL,32,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10121,47,NULL,NULL,33,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10122,47,NULL,NULL,34,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10123,47,NULL,NULL,35,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10124,47,NULL,NULL,36,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10125,47,NULL,NULL,37,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10126,47,NULL,NULL,38,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10127,47,NULL,NULL,39,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10128,47,NULL,NULL,40,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10129,47,NULL,NULL,41,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10130,47,NULL,NULL,42,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10131,47,NULL,NULL,43,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10132,47,NULL,NULL,44,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10133,47,NULL,NULL,45,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10134,47,NULL,NULL,46,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10135,47,NULL,NULL,47,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10136,47,NULL,NULL,48,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10137,47,NULL,NULL,49,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10138,47,NULL,NULL,50,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10139,47,NULL,NULL,51,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10140,47,NULL,NULL,52,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10141,47,NULL,NULL,53,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10142,47,NULL,NULL,54,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10143,47,NULL,NULL,55,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10144,47,NULL,NULL,56,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10145,47,NULL,NULL,57,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10146,47,NULL,NULL,58,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10147,47,NULL,NULL,59,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10148,47,NULL,NULL,60,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10149,47,NULL,NULL,61,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10150,47,NULL,NULL,62,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10151,47,NULL,NULL,63,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10152,47,NULL,NULL,64,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10153,47,NULL,NULL,65,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10154,47,NULL,NULL,66,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10155,47,NULL,NULL,67,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10156,47,NULL,NULL,68,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10157,47,NULL,NULL,69,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10158,47,NULL,NULL,70,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10159,47,NULL,NULL,71,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10160,47,NULL,NULL,72,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10161,47,NULL,NULL,73,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10162,47,NULL,NULL,74,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10163,47,NULL,NULL,75,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10164,47,NULL,NULL,76,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10165,47,NULL,NULL,77,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10166,47,NULL,NULL,78,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10167,47,NULL,NULL,79,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10168,47,NULL,NULL,80,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10169,47,NULL,NULL,81,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10170,47,NULL,NULL,82,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10171,47,NULL,NULL,83,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10172,47,NULL,NULL,84,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10173,47,NULL,NULL,85,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10174,47,NULL,NULL,86,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10175,47,NULL,NULL,87,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10176,47,NULL,NULL,88,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10177,47,NULL,NULL,89,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10178,47,NULL,NULL,90,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10179,47,NULL,NULL,91,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10180,47,NULL,NULL,92,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10181,47,NULL,NULL,93,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10182,47,NULL,NULL,94,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10183,47,NULL,NULL,95,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10184,47,NULL,NULL,96,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10185,47,NULL,NULL,97,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10186,47,NULL,NULL,98,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10187,47,NULL,NULL,99,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10188,47,NULL,NULL,100,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10189,47,NULL,NULL,101,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10190,47,NULL,NULL,102,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10191,47,NULL,NULL,103,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10192,47,NULL,NULL,104,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10193,47,NULL,NULL,105,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10194,47,NULL,NULL,106,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10195,47,NULL,NULL,107,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10196,47,NULL,NULL,108,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10197,47,NULL,NULL,109,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10198,47,NULL,NULL,110,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10199,47,NULL,NULL,111,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10200,47,NULL,NULL,112,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10201,47,NULL,NULL,113,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10202,47,NULL,NULL,114,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10203,47,NULL,NULL,115,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10204,47,NULL,NULL,116,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10205,47,NULL,NULL,117,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10206,47,NULL,NULL,118,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10207,47,NULL,NULL,119,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10208,47,NULL,NULL,120,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10209,47,NULL,NULL,121,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10210,47,NULL,NULL,122,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10211,47,NULL,NULL,123,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10212,47,NULL,NULL,124,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10213,47,NULL,NULL,125,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10214,47,NULL,NULL,126,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10215,47,NULL,NULL,127,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10216,47,NULL,NULL,128,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10217,47,NULL,NULL,129,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10218,47,NULL,NULL,130,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10219,47,NULL,NULL,131,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10220,47,NULL,NULL,132,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10221,47,NULL,NULL,133,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10222,47,NULL,NULL,134,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10223,47,NULL,NULL,135,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10224,47,NULL,NULL,136,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10225,47,NULL,NULL,137,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10226,47,NULL,NULL,138,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10227,47,NULL,NULL,139,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10228,47,NULL,NULL,140,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10229,47,NULL,NULL,141,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10230,47,NULL,NULL,142,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10231,47,NULL,NULL,143,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10232,47,NULL,NULL,144,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10233,47,NULL,NULL,145,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10234,47,NULL,NULL,146,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10235,47,NULL,NULL,147,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10236,47,NULL,NULL,148,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10237,47,NULL,NULL,149,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10238,47,NULL,NULL,150,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10239,47,NULL,NULL,151,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10240,47,NULL,NULL,152,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10241,47,NULL,NULL,153,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10242,47,NULL,NULL,154,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10243,47,NULL,NULL,155,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10244,47,NULL,NULL,156,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10245,47,NULL,NULL,157,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10246,47,NULL,NULL,158,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10247,47,NULL,NULL,159,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10248,47,NULL,NULL,160,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10249,47,NULL,NULL,161,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10250,47,NULL,NULL,162,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10251,47,NULL,NULL,163,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10252,47,NULL,NULL,164,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10253,47,NULL,NULL,165,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10254,47,NULL,NULL,166,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10255,47,NULL,NULL,167,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10256,47,NULL,NULL,168,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10257,47,NULL,NULL,169,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10258,47,NULL,NULL,170,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10259,47,NULL,NULL,171,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10260,47,NULL,NULL,172,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10261,47,NULL,NULL,173,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10262,47,NULL,NULL,174,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10263,47,NULL,NULL,175,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10264,47,NULL,NULL,176,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10265,47,NULL,NULL,177,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10266,47,NULL,NULL,178,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10267,47,NULL,NULL,179,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10268,47,NULL,NULL,180,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10269,47,NULL,NULL,181,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10270,47,NULL,NULL,182,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10271,47,NULL,NULL,183,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10272,47,NULL,NULL,184,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10273,47,NULL,NULL,185,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10274,47,NULL,NULL,186,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10275,47,NULL,NULL,187,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10276,47,NULL,NULL,188,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10277,47,NULL,NULL,189,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10278,47,NULL,NULL,190,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10279,47,NULL,NULL,191,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10280,47,NULL,NULL,192,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10281,47,NULL,NULL,193,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10282,47,NULL,NULL,194,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10283,47,NULL,NULL,195,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10284,47,NULL,NULL,196,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10285,47,NULL,NULL,197,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10286,47,NULL,NULL,198,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10287,47,NULL,NULL,199,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10288,47,NULL,NULL,200,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10289,47,NULL,NULL,201,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10290,47,NULL,NULL,202,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10291,47,NULL,NULL,203,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10292,47,NULL,NULL,204,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10293,47,NULL,NULL,205,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10294,47,NULL,NULL,206,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10295,47,NULL,NULL,207,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10296,47,NULL,NULL,208,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10297,47,NULL,NULL,209,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10298,47,NULL,NULL,210,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10299,47,NULL,NULL,211,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10300,47,NULL,NULL,212,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10301,47,NULL,NULL,213,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10302,47,NULL,NULL,214,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10303,47,NULL,NULL,215,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10304,47,NULL,NULL,216,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10305,47,NULL,NULL,217,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10306,47,NULL,NULL,218,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10307,47,NULL,NULL,219,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10308,47,NULL,NULL,220,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10309,47,NULL,NULL,221,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10310,47,NULL,NULL,222,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10311,47,NULL,NULL,223,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10312,47,NULL,NULL,224,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10313,47,NULL,NULL,225,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10314,47,NULL,NULL,226,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10315,47,NULL,NULL,227,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10316,47,NULL,NULL,228,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10317,47,NULL,NULL,229,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10318,47,NULL,NULL,230,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10319,47,NULL,NULL,231,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10320,47,NULL,NULL,232,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10321,47,NULL,NULL,233,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10322,47,NULL,NULL,234,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10323,47,NULL,NULL,235,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10324,47,NULL,NULL,236,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10325,47,NULL,NULL,237,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10326,47,NULL,NULL,238,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10327,47,NULL,NULL,239,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10328,47,NULL,NULL,240,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10329,47,NULL,NULL,241,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10330,47,NULL,NULL,242,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10331,47,NULL,NULL,243,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10332,47,NULL,NULL,244,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10333,47,NULL,NULL,245,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10334,47,NULL,NULL,246,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10335,47,NULL,NULL,247,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10336,47,NULL,NULL,248,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10337,47,NULL,NULL,249,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10338,47,NULL,NULL,250,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10339,47,NULL,NULL,251,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10340,47,NULL,NULL,252,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10341,47,NULL,NULL,253,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10342,47,NULL,NULL,254,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10343,47,NULL,NULL,255,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10344,47,NULL,NULL,256,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10345,47,NULL,NULL,257,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10346,47,NULL,NULL,258,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10347,47,NULL,NULL,259,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10348,47,NULL,NULL,260,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10349,47,NULL,NULL,261,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10350,47,NULL,NULL,262,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10351,47,NULL,NULL,263,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10352,47,NULL,NULL,264,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10353,47,NULL,NULL,265,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10354,47,NULL,NULL,266,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10355,47,NULL,NULL,267,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10356,47,NULL,NULL,268,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10357,47,NULL,NULL,269,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10358,47,NULL,NULL,270,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10359,47,NULL,NULL,271,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10360,47,NULL,NULL,272,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10361,47,NULL,NULL,273,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10362,47,NULL,NULL,274,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10363,47,NULL,NULL,275,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10364,47,NULL,NULL,276,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10365,47,NULL,NULL,277,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10366,47,NULL,NULL,278,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10367,47,NULL,NULL,279,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10368,47,NULL,NULL,280,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10369,47,NULL,NULL,281,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10370,47,NULL,NULL,282,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10371,47,NULL,NULL,283,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10372,47,NULL,NULL,284,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10373,47,NULL,NULL,285,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10374,47,NULL,NULL,286,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10375,47,NULL,NULL,287,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10376,47,NULL,NULL,288,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10377,47,NULL,NULL,289,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10378,47,NULL,NULL,290,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10379,47,NULL,NULL,291,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10380,47,NULL,NULL,292,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10381,47,NULL,NULL,293,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10382,47,NULL,NULL,294,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10383,47,NULL,NULL,295,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10384,47,NULL,NULL,296,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10385,47,NULL,NULL,297,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10386,47,NULL,NULL,298,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10387,47,NULL,NULL,299,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10388,47,NULL,NULL,300,0,1,32,'2023-06-20 09:40:29',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10689,45,NULL,NULL,1,0,1,1,'2023-06-25 11:38:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10690,45,'22',0.50,2,0,0,1,'2023-06-25 11:38:02','2023-06-25 11:38:11',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10691,45,NULL,NULL,3,0,1,1,'2023-06-25 11:38:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10692,45,'22',0.50,4,0,0,1,'2023-06-25 11:38:02','2023-06-25 11:38:11',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10693,45,'22',0.50,5,0,0,1,'2023-06-25 11:38:02','2023-06-26 16:42:16',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10694,45,'22',0.50,6,0,0,1,'2023-06-25 11:38:02','2023-06-25 11:38:11',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10695,45,NULL,NULL,7,0,1,1,'2023-06-25 11:38:02',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10696,45,'22',0.50,8,0,0,1,'2023-06-25 11:38:02','2023-06-25 11:38:11',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10697,44,'',0.00,1,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10698,44,'',0.00,2,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10699,44,'',0.00,3,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10700,44,'18',0.20,4,0,0,1,'2023-06-25 13:21:49','2023-06-25 14:12:06',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10701,44,'',0.00,5,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10702,44,'',0.00,6,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10703,44,'',0.00,7,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10704,44,'18',0.20,8,0,0,1,'2023-06-25 13:21:49','2023-06-25 14:12:06',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10705,44,'',0.00,9,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10706,44,'',0.00,10,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10707,44,'',0.00,11,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10708,44,'T02',0.85,12,0,0,1,'2023-06-25 13:21:49','2023-07-04 14:07:14',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10709,44,'',0.00,13,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10710,44,'',0.00,14,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10711,44,'',0.00,15,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10712,44,'T02',0.85,16,0,0,1,'2023-06-25 13:21:49','2023-07-04 14:07:14',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10713,44,'',0.00,17,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10714,44,'',0.00,18,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10715,44,'',0.00,19,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10716,44,'',0.00,20,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10717,44,'',0.00,21,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10718,44,'',0.00,22,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10719,44,'21',0.65,23,0,0,1,'2023-06-25 13:21:49','2023-07-04 14:08:34',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10720,44,'',0.00,24,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10721,44,'',0.00,25,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10722,44,'',0.00,26,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10723,44,'21',0.65,27,0,0,1,'2023-06-25 13:21:49','2023-07-04 14:08:34',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10724,44,'',0.00,28,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10725,44,'',0.00,29,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10726,44,'',0.00,30,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10727,44,'',0.00,31,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10728,44,'',0.00,32,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10729,44,'',0.00,33,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10730,44,'',0.00,34,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10731,44,'',0.00,35,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10732,44,'',0.00,36,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10733,44,'',0.00,37,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10734,44,'',0.00,38,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10735,44,'',0.00,39,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10736,44,'',0.00,40,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10737,44,'',0.00,41,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10738,44,'',0.00,42,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10739,44,'',0.00,43,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10740,44,'',0.00,44,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10741,44,'',0.00,45,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10742,44,'',0.00,46,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10743,44,'',0.00,47,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10744,44,'',0.00,48,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10745,44,'',0.00,49,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10746,44,'',0.00,50,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10747,44,'',0.00,51,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10748,44,'',0.00,52,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10749,44,'',0.00,53,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10750,44,'',0.00,54,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10751,44,'',0.00,55,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10752,44,'',0.00,56,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10753,44,'',0.00,57,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10754,44,'',0.00,58,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10755,44,'',0.00,59,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10756,44,'',0.00,60,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10757,44,'',0.00,61,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10758,44,'',0.00,62,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10759,44,'',0.00,63,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10760,44,'',0.00,64,0,0,1,'2023-06-25 13:21:49','2023-06-25 13:24:39',1);
INSERT INTO `t_item_atp_file_detail` VALUES (10761,48,NULL,NULL,1,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10762,48,NULL,NULL,2,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10763,48,NULL,NULL,3,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10764,48,NULL,NULL,4,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10765,48,NULL,NULL,5,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10766,48,NULL,NULL,6,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10767,48,NULL,NULL,7,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10768,48,NULL,NULL,8,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10769,48,NULL,NULL,9,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10770,48,NULL,NULL,10,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10771,48,NULL,NULL,11,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10772,48,NULL,NULL,12,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10773,48,NULL,NULL,13,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10774,48,NULL,NULL,14,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10775,48,NULL,NULL,15,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10776,48,NULL,NULL,16,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10777,48,NULL,NULL,17,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10778,48,NULL,NULL,18,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10779,48,NULL,NULL,19,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10780,48,NULL,NULL,20,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10781,48,NULL,NULL,21,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10782,48,NULL,NULL,22,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10783,48,NULL,NULL,23,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10784,48,NULL,NULL,24,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10785,48,NULL,NULL,25,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10786,48,NULL,NULL,26,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10787,48,NULL,NULL,27,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10788,48,NULL,NULL,28,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10789,48,NULL,NULL,29,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10790,48,NULL,NULL,30,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10791,48,NULL,NULL,31,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10792,48,NULL,NULL,32,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10793,48,NULL,NULL,33,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10794,48,NULL,NULL,34,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10795,48,NULL,NULL,35,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10796,48,NULL,NULL,36,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10797,48,NULL,NULL,37,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10798,48,NULL,NULL,38,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10799,48,NULL,NULL,39,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10800,48,NULL,NULL,40,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10801,48,NULL,NULL,41,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10802,48,NULL,NULL,42,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10803,48,NULL,NULL,43,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10804,48,NULL,NULL,44,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10805,48,NULL,NULL,45,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10806,48,NULL,NULL,46,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10807,48,NULL,NULL,47,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10808,48,NULL,NULL,48,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10809,48,NULL,NULL,49,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10810,48,NULL,NULL,50,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10811,48,NULL,NULL,51,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10812,48,NULL,NULL,52,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10813,48,NULL,NULL,53,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10814,48,NULL,NULL,54,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10815,48,NULL,NULL,55,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10816,48,NULL,NULL,56,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10817,48,NULL,NULL,57,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10818,48,NULL,NULL,58,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10819,48,NULL,NULL,59,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10820,48,NULL,NULL,60,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10821,48,NULL,NULL,61,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10822,48,NULL,NULL,62,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10823,48,NULL,NULL,63,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10824,48,NULL,NULL,64,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10825,48,NULL,NULL,65,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10826,48,NULL,NULL,66,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10827,48,NULL,NULL,67,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10828,48,NULL,NULL,68,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10829,48,NULL,NULL,69,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10830,48,NULL,NULL,70,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10831,48,NULL,NULL,71,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10832,48,NULL,NULL,72,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10833,48,NULL,NULL,73,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10834,48,NULL,NULL,74,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10835,48,NULL,NULL,75,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10836,48,NULL,NULL,76,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10837,48,NULL,NULL,77,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10838,48,NULL,NULL,78,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10839,48,NULL,NULL,79,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10840,48,NULL,NULL,80,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10841,48,NULL,NULL,81,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10842,48,NULL,NULL,82,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10843,48,NULL,NULL,83,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10844,48,NULL,NULL,84,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10845,48,NULL,NULL,85,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10846,48,NULL,NULL,86,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10847,48,NULL,NULL,87,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10848,48,NULL,NULL,88,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10849,48,NULL,NULL,89,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10850,48,NULL,NULL,90,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10851,48,NULL,NULL,91,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10852,48,NULL,NULL,92,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10853,48,NULL,NULL,93,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10854,48,NULL,NULL,94,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10855,48,NULL,NULL,95,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10856,48,NULL,NULL,96,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10857,48,NULL,NULL,97,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10858,48,NULL,NULL,98,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10859,48,NULL,NULL,99,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10860,48,NULL,NULL,100,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10861,48,NULL,NULL,101,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10862,48,NULL,NULL,102,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10863,48,NULL,NULL,103,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10864,48,NULL,NULL,104,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10865,48,NULL,NULL,105,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10866,48,NULL,NULL,106,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10867,48,NULL,NULL,107,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10868,48,NULL,NULL,108,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10869,48,NULL,NULL,109,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10870,48,NULL,NULL,110,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10871,48,NULL,NULL,111,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10872,48,NULL,NULL,112,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10873,48,NULL,NULL,113,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10874,48,NULL,NULL,114,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10875,48,NULL,NULL,115,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10876,48,NULL,NULL,116,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10877,48,NULL,NULL,117,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10878,48,NULL,NULL,118,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10879,48,NULL,NULL,119,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10880,48,NULL,NULL,120,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10881,48,NULL,NULL,121,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10882,48,NULL,NULL,122,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10883,48,NULL,NULL,123,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10884,48,NULL,NULL,124,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10885,48,NULL,NULL,125,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10886,48,NULL,NULL,126,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10887,48,NULL,NULL,127,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10888,48,NULL,NULL,128,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10889,48,NULL,NULL,129,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10890,48,NULL,NULL,130,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10891,48,NULL,NULL,131,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10892,48,NULL,NULL,132,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10893,48,NULL,NULL,133,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10894,48,NULL,NULL,134,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10895,48,NULL,NULL,135,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10896,48,NULL,NULL,136,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10897,48,NULL,NULL,137,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10898,48,NULL,NULL,138,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10899,48,NULL,NULL,139,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10900,48,NULL,NULL,140,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10901,48,NULL,NULL,141,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10902,48,NULL,NULL,142,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10903,48,NULL,NULL,143,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10904,48,NULL,NULL,144,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10905,48,NULL,NULL,145,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10906,48,NULL,NULL,146,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10907,48,NULL,NULL,147,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10908,48,NULL,NULL,148,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10909,48,NULL,NULL,149,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10910,48,NULL,NULL,150,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10911,48,NULL,NULL,151,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10912,48,NULL,NULL,152,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10913,48,NULL,NULL,153,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10914,48,NULL,NULL,154,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10915,48,NULL,NULL,155,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10916,48,NULL,NULL,156,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10917,48,NULL,NULL,157,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10918,48,NULL,NULL,158,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10919,48,NULL,NULL,159,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10920,48,NULL,NULL,160,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10921,48,NULL,NULL,161,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10922,48,NULL,NULL,162,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10923,48,NULL,NULL,163,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10924,48,NULL,NULL,164,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10925,48,NULL,NULL,165,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10926,48,NULL,NULL,166,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10927,48,NULL,NULL,167,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10928,48,NULL,NULL,168,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10929,48,NULL,NULL,169,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10930,48,NULL,NULL,170,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10931,48,NULL,NULL,171,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10932,48,NULL,NULL,172,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10933,48,NULL,NULL,173,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10934,48,NULL,NULL,174,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10935,48,NULL,NULL,175,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10936,48,NULL,NULL,176,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10937,48,NULL,NULL,177,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10938,48,NULL,NULL,178,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10939,48,NULL,NULL,179,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10940,48,NULL,NULL,180,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10941,48,NULL,NULL,181,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10942,48,NULL,NULL,182,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10943,48,NULL,NULL,183,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10944,48,NULL,NULL,184,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10945,48,NULL,NULL,185,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10946,48,NULL,NULL,186,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10947,48,NULL,NULL,187,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10948,48,NULL,NULL,188,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10949,48,NULL,NULL,189,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10950,48,NULL,NULL,190,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10951,48,NULL,NULL,191,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10952,48,NULL,NULL,192,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10953,48,NULL,NULL,193,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10954,48,NULL,NULL,194,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10955,48,NULL,NULL,195,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10956,48,NULL,NULL,196,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10957,48,NULL,NULL,197,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10958,48,NULL,NULL,198,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10959,48,NULL,NULL,199,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10960,48,NULL,NULL,200,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10961,48,NULL,NULL,201,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10962,48,NULL,NULL,202,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10963,48,NULL,NULL,203,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10964,48,NULL,NULL,204,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10965,48,NULL,NULL,205,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10966,48,NULL,NULL,206,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10967,48,NULL,NULL,207,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10968,48,NULL,NULL,208,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10969,48,NULL,NULL,209,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10970,48,NULL,NULL,210,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10971,48,NULL,NULL,211,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10972,48,NULL,NULL,212,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10973,48,NULL,NULL,213,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10974,48,NULL,NULL,214,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10975,48,NULL,NULL,215,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10976,48,NULL,NULL,216,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10977,48,NULL,NULL,217,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10978,48,NULL,NULL,218,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10979,48,NULL,NULL,219,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10980,48,NULL,NULL,220,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10981,48,NULL,NULL,221,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10982,48,NULL,NULL,222,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10983,48,NULL,NULL,223,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10984,48,NULL,NULL,224,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10985,48,NULL,NULL,225,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10986,48,NULL,NULL,226,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10987,48,NULL,NULL,227,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10988,48,NULL,NULL,228,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10989,48,NULL,NULL,229,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10990,48,NULL,NULL,230,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10991,48,NULL,NULL,231,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10992,48,NULL,NULL,232,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10993,48,NULL,NULL,233,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10994,48,NULL,NULL,234,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10995,48,NULL,NULL,235,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10996,48,NULL,NULL,236,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10997,48,NULL,NULL,237,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10998,48,NULL,NULL,238,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (10999,48,NULL,NULL,239,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11000,48,NULL,NULL,240,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11001,48,NULL,NULL,241,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11002,48,NULL,NULL,242,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11003,48,NULL,NULL,243,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11004,48,NULL,NULL,244,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11005,48,NULL,NULL,245,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11006,48,NULL,NULL,246,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11007,48,NULL,NULL,247,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11008,48,NULL,NULL,248,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11009,48,NULL,NULL,249,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11010,48,NULL,NULL,250,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11011,48,NULL,NULL,251,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11012,48,NULL,NULL,252,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11013,48,NULL,NULL,253,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11014,48,NULL,NULL,254,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11015,48,NULL,NULL,255,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11016,48,NULL,NULL,256,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11017,48,NULL,NULL,257,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11018,48,NULL,NULL,258,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11019,48,NULL,NULL,259,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11020,48,NULL,NULL,260,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11021,48,NULL,NULL,261,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11022,48,NULL,NULL,262,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11023,48,NULL,NULL,263,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11024,48,NULL,NULL,264,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11025,48,NULL,NULL,265,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11026,48,NULL,NULL,266,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11027,48,NULL,NULL,267,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11028,48,NULL,NULL,268,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11029,48,NULL,NULL,269,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11030,48,NULL,NULL,270,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11031,48,NULL,NULL,271,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11032,48,NULL,NULL,272,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11033,48,NULL,NULL,273,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11034,48,NULL,NULL,274,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11035,48,NULL,NULL,275,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11036,48,NULL,NULL,276,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11037,48,NULL,NULL,277,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11038,48,NULL,NULL,278,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11039,48,NULL,NULL,279,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11040,48,NULL,NULL,280,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11041,48,NULL,NULL,281,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11042,48,NULL,NULL,282,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11043,48,NULL,NULL,283,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11044,48,NULL,NULL,284,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11045,48,NULL,NULL,285,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11046,48,NULL,NULL,286,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11047,48,NULL,NULL,287,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11048,48,NULL,NULL,288,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11049,48,NULL,NULL,289,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11050,48,NULL,NULL,290,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11051,48,NULL,NULL,291,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11052,48,NULL,NULL,292,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11053,48,NULL,NULL,293,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11054,48,NULL,NULL,294,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11055,48,NULL,NULL,295,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11056,48,NULL,NULL,296,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11057,48,NULL,NULL,297,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11058,48,NULL,NULL,298,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11059,48,NULL,NULL,299,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11060,48,NULL,NULL,300,0,1,1,'2023-07-04 13:49:55',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11061,49,NULL,NULL,1,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11062,49,NULL,NULL,2,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11063,49,NULL,NULL,3,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11064,49,NULL,NULL,4,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11065,49,NULL,NULL,5,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11066,49,NULL,NULL,6,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11067,49,NULL,NULL,7,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11068,49,NULL,NULL,8,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11069,49,NULL,NULL,9,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11070,49,NULL,NULL,10,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11071,49,NULL,NULL,11,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11072,49,NULL,NULL,12,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11073,49,NULL,NULL,13,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11074,49,NULL,NULL,14,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11075,49,NULL,NULL,15,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11076,49,NULL,NULL,16,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11077,49,NULL,NULL,17,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11078,49,NULL,NULL,18,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11079,49,NULL,NULL,19,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11080,49,NULL,NULL,20,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11081,49,NULL,NULL,21,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11082,49,NULL,NULL,22,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11083,49,NULL,NULL,23,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11084,49,NULL,NULL,24,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11085,49,NULL,NULL,25,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11086,49,NULL,NULL,26,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11087,49,NULL,NULL,27,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11088,49,NULL,NULL,28,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11089,49,NULL,NULL,29,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11090,49,NULL,NULL,30,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11091,49,NULL,NULL,31,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11092,49,NULL,NULL,32,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11093,49,NULL,NULL,33,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11094,49,NULL,NULL,34,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11095,49,NULL,NULL,35,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11096,49,NULL,NULL,36,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11097,49,NULL,NULL,37,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11098,49,NULL,NULL,38,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11099,49,NULL,NULL,39,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11100,49,NULL,NULL,40,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11101,49,NULL,NULL,41,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11102,49,NULL,NULL,42,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11103,49,NULL,NULL,43,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11104,49,NULL,NULL,44,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11105,49,NULL,NULL,45,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11106,49,NULL,NULL,46,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11107,49,NULL,NULL,47,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11108,49,NULL,NULL,48,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11109,49,NULL,NULL,49,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11110,49,NULL,NULL,50,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11111,49,NULL,NULL,51,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11112,49,NULL,NULL,52,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11113,49,NULL,NULL,53,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11114,49,NULL,NULL,54,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11115,49,NULL,NULL,55,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11116,49,NULL,NULL,56,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11117,49,NULL,NULL,57,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11118,49,NULL,NULL,58,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11119,49,NULL,NULL,59,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11120,49,NULL,NULL,60,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11121,49,NULL,NULL,61,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11122,49,NULL,NULL,62,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11123,49,NULL,NULL,63,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11124,49,NULL,NULL,64,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11125,49,NULL,NULL,65,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11126,49,NULL,NULL,66,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11127,49,NULL,NULL,67,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11128,49,NULL,NULL,68,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11129,49,NULL,NULL,69,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11130,49,NULL,NULL,70,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11131,49,NULL,NULL,71,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11132,49,NULL,NULL,72,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11133,49,NULL,NULL,73,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11134,49,NULL,NULL,74,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11135,49,NULL,NULL,75,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11136,49,NULL,NULL,76,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11137,49,NULL,NULL,77,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11138,49,NULL,NULL,78,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11139,49,NULL,NULL,79,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11140,49,NULL,NULL,80,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11141,49,NULL,NULL,81,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11142,49,NULL,NULL,82,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11143,49,NULL,NULL,83,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11144,49,NULL,NULL,84,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11145,49,NULL,NULL,85,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11146,49,NULL,NULL,86,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11147,49,NULL,NULL,87,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11148,49,NULL,NULL,88,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11149,49,NULL,NULL,89,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11150,49,NULL,NULL,90,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11151,49,NULL,NULL,91,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11152,49,NULL,NULL,92,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11153,49,NULL,NULL,93,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11154,49,NULL,NULL,94,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11155,49,NULL,NULL,95,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11156,49,NULL,NULL,96,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11157,49,NULL,NULL,97,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11158,49,NULL,NULL,98,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11159,49,NULL,NULL,99,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11160,49,NULL,NULL,100,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11161,49,NULL,NULL,101,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11162,49,NULL,NULL,102,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11163,49,NULL,NULL,103,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11164,49,NULL,NULL,104,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11165,49,NULL,NULL,105,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11166,49,NULL,NULL,106,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11167,49,NULL,NULL,107,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11168,49,NULL,NULL,108,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11169,49,NULL,NULL,109,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11170,49,NULL,NULL,110,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11171,49,NULL,NULL,111,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11172,49,NULL,NULL,112,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11173,49,NULL,NULL,113,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11174,49,NULL,NULL,114,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11175,49,NULL,NULL,115,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11176,49,NULL,NULL,116,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11177,49,NULL,NULL,117,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11178,49,NULL,NULL,118,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11179,49,NULL,NULL,119,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11180,49,NULL,NULL,120,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11181,49,NULL,NULL,121,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11182,49,NULL,NULL,122,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11183,49,NULL,NULL,123,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11184,49,NULL,NULL,124,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11185,49,NULL,NULL,125,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11186,49,NULL,NULL,126,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11187,49,NULL,NULL,127,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11188,49,NULL,NULL,128,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11189,49,NULL,NULL,129,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11190,49,NULL,NULL,130,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11191,49,NULL,NULL,131,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11192,49,NULL,NULL,132,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11193,49,NULL,NULL,133,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11194,49,NULL,NULL,134,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11195,49,NULL,NULL,135,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11196,49,NULL,NULL,136,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11197,49,NULL,NULL,137,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11198,49,NULL,NULL,138,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11199,49,NULL,NULL,139,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11200,49,NULL,NULL,140,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11201,49,NULL,NULL,141,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11202,49,NULL,NULL,142,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11203,49,NULL,NULL,143,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11204,49,NULL,NULL,144,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11205,49,NULL,NULL,145,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11206,49,NULL,NULL,146,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11207,49,NULL,NULL,147,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11208,49,NULL,NULL,148,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11209,49,NULL,NULL,149,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11210,49,NULL,NULL,150,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11211,49,NULL,NULL,151,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11212,49,NULL,NULL,152,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11213,49,NULL,NULL,153,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11214,49,NULL,NULL,154,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11215,49,NULL,NULL,155,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11216,49,NULL,NULL,156,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11217,49,NULL,NULL,157,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11218,49,NULL,NULL,158,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11219,49,NULL,NULL,159,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11220,49,NULL,NULL,160,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11221,49,NULL,NULL,161,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11222,49,NULL,NULL,162,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11223,49,NULL,NULL,163,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11224,49,NULL,NULL,164,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11225,49,NULL,NULL,165,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11226,49,NULL,NULL,166,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11227,49,NULL,NULL,167,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11228,49,NULL,NULL,168,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11229,49,NULL,NULL,169,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11230,49,NULL,NULL,170,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11231,49,NULL,NULL,171,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11232,49,NULL,NULL,172,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11233,49,NULL,NULL,173,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11234,49,NULL,NULL,174,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11235,49,NULL,NULL,175,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11236,49,NULL,NULL,176,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11237,49,NULL,NULL,177,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11238,49,NULL,NULL,178,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11239,49,NULL,NULL,179,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11240,49,NULL,NULL,180,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11241,49,NULL,NULL,181,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11242,49,NULL,NULL,182,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11243,49,NULL,NULL,183,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11244,49,NULL,NULL,184,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11245,49,NULL,NULL,185,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11246,49,NULL,NULL,186,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11247,49,NULL,NULL,187,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11248,49,NULL,NULL,188,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11249,49,NULL,NULL,189,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11250,49,NULL,NULL,190,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11251,49,NULL,NULL,191,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11252,49,NULL,NULL,192,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11253,49,NULL,NULL,193,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11254,49,NULL,NULL,194,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11255,49,NULL,NULL,195,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11256,49,NULL,NULL,196,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11257,49,NULL,NULL,197,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11258,49,NULL,NULL,198,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11259,49,NULL,NULL,199,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11260,49,NULL,NULL,200,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11261,49,NULL,NULL,201,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11262,49,NULL,NULL,202,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11263,49,NULL,NULL,203,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11264,49,NULL,NULL,204,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11265,49,NULL,NULL,205,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11266,49,NULL,NULL,206,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11267,49,NULL,NULL,207,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11268,49,NULL,NULL,208,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11269,49,NULL,NULL,209,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11270,49,NULL,NULL,210,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11271,49,NULL,NULL,211,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11272,49,NULL,NULL,212,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11273,49,NULL,NULL,213,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11274,49,NULL,NULL,214,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11275,49,NULL,NULL,215,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11276,49,NULL,NULL,216,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11277,49,NULL,NULL,217,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11278,49,NULL,NULL,218,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11279,49,NULL,NULL,219,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11280,49,NULL,NULL,220,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11281,49,NULL,NULL,221,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11282,49,NULL,NULL,222,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11283,49,NULL,NULL,223,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11284,49,NULL,NULL,224,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11285,49,NULL,NULL,225,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11286,49,NULL,NULL,226,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11287,49,NULL,NULL,227,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11288,49,NULL,NULL,228,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11289,49,NULL,NULL,229,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11290,49,NULL,NULL,230,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11291,49,NULL,NULL,231,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11292,49,NULL,NULL,232,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11293,49,NULL,NULL,233,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11294,49,NULL,NULL,234,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11295,49,NULL,NULL,235,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11296,49,NULL,NULL,236,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11297,49,NULL,NULL,237,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11298,49,NULL,NULL,238,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11299,49,NULL,NULL,239,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11300,49,NULL,NULL,240,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11301,49,NULL,NULL,241,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11302,49,NULL,NULL,242,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11303,49,NULL,NULL,243,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11304,49,NULL,NULL,244,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11305,49,NULL,NULL,245,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11306,49,NULL,NULL,246,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11307,49,NULL,NULL,247,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11308,49,NULL,NULL,248,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11309,49,NULL,NULL,249,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11310,49,NULL,NULL,250,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11311,49,NULL,NULL,251,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11312,49,NULL,NULL,252,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11313,49,NULL,NULL,253,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11314,49,NULL,NULL,254,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11315,49,NULL,NULL,255,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11316,49,NULL,NULL,256,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11317,49,NULL,NULL,257,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11318,49,NULL,NULL,258,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11319,49,NULL,NULL,259,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11320,49,NULL,NULL,260,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11321,49,NULL,NULL,261,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11322,49,NULL,NULL,262,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11323,49,NULL,NULL,263,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11324,49,NULL,NULL,264,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11325,49,NULL,NULL,265,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11326,49,NULL,NULL,266,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11327,49,NULL,NULL,267,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11328,49,NULL,NULL,268,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11329,49,NULL,NULL,269,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11330,49,NULL,NULL,270,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11331,49,NULL,NULL,271,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11332,49,NULL,NULL,272,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11333,49,NULL,NULL,273,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11334,49,NULL,NULL,274,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11335,49,NULL,NULL,275,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11336,49,NULL,NULL,276,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11337,49,NULL,NULL,277,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11338,49,NULL,NULL,278,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11339,49,NULL,NULL,279,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11340,49,NULL,NULL,280,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11341,49,NULL,NULL,281,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11342,49,NULL,NULL,282,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11343,49,NULL,NULL,283,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11344,49,NULL,NULL,284,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11345,49,NULL,NULL,285,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11346,49,NULL,NULL,286,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11347,49,NULL,NULL,287,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11348,49,NULL,NULL,288,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11349,49,NULL,NULL,289,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11350,49,NULL,NULL,290,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11351,49,NULL,NULL,291,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11352,49,NULL,NULL,292,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11353,49,NULL,NULL,293,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11354,49,NULL,NULL,294,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11355,49,NULL,NULL,295,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11356,49,NULL,NULL,296,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11357,49,NULL,NULL,297,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11358,49,NULL,NULL,298,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11359,49,NULL,NULL,299,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
INSERT INTO `t_item_atp_file_detail` VALUES (11360,49,NULL,NULL,300,0,1,1,'2023-07-04 14:37:49',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file`
--

LOCK TABLES `t_item_drill_file` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file` DISABLE KEYS */;
INSERT INTO `t_item_drill_file` VALUES (16,'IF2023040500002','PCB单层板',36,22,'生产工单模板 (2).xlsx','/root/admin1.0/data/dia/生产工单模板 (2).xlsx',0,1,1,'2023-04-19 01:03:56','2023-07-04 16:14:31',1);
INSERT INTO `t_item_drill_file` VALUES (18,'IF2023040500002','PCB单层板',36,22,'PCB单层板222','\\\\fileServer\\IF200001111\\IF200001111.drl',0,1,1,'2023-04-21 01:34:09','2023-06-19 17:27:17',1);
INSERT INTO `t_item_drill_file` VALUES (23,'IF2023040500002','PCB单层板',36,22,'111','/root/admin1.0/data/dia/生产工单模板 (2).xlsx',0,1,1,'2023-06-26 16:41:55',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='钻带参数文件明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_drill_file_detail`
--

LOCK TABLES `t_item_drill_file_detail` WRITE;
/*!40000 ALTER TABLE `t_item_drill_file_detail` DISABLE KEYS */;
INSERT INTO `t_item_drill_file_detail` VALUES (3,16,'T01',0.81,0,1,NULL,'1900-01-01 00:00:00','2023-05-07 21:59:03',1);
INSERT INTO `t_item_drill_file_detail` VALUES (4,16,'T02',0.85,0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_item_drill_file_detail` VALUES (5,16,'T03',0.90,0,1,NULL,NULL,NULL,NULL);
INSERT INTO `t_item_drill_file_detail` VALUES (7,21,'21',0.65,0,1,1,'2023-05-07 21:58:43','2023-06-19 17:27:42',1);
INSERT INTO `t_item_drill_file_detail` VALUES (8,18,'18',0.20,0,1,1,'2023-05-10 01:50:19','2023-06-19 17:27:10',1);
INSERT INTO `t_item_drill_file_detail` VALUES (9,18,'22',0.50,0,1,1,'2023-05-15 04:36:52','2023-06-19 17:27:16',1);
INSERT INTO `t_item_drill_file_detail` VALUES (10,21,'11',0.60,0,1,1,'2023-05-15 04:37:03','2023-06-19 17:27:34',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
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
INSERT INTO `t_item_type` VALUES (25,'铜',2,'tong',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:02:53','2023-06-21 09:17:04',1);
INSERT INTO `t_item_type` VALUES (26,'铝',2,'lv',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:10:20','2023-04-12 01:27:20',1);
INSERT INTO `t_item_type` VALUES (27,'板材',2,'panel',1,NULL,'0,1,2',0,1,1,'2023-04-12 01:27:59',NULL,NULL);
INSERT INTO `t_item_type` VALUES (40,'单钻孔',22,'onlyDrill',2,NULL,'0,1,22',0,1,1,'2023-05-05 04:03:54','2023-06-20 10:26:15',32);
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
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COMMENT='库存记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_material_stock`
--

LOCK TABLES `t_material_stock` WRITE;
/*!40000 ALTER TABLE `t_material_stock` DISABLE KEYS */;
INSERT INTO `t_material_stock` VALUES (29,'MSIN20230628001','001',2,3,'in',22,36,'IF2023040500002','PCB单层板','DDD XXXX DD','Panel',3.00,NULL,'123',2,'WareHouse2','WareHouse2',NULL,57,'拖拽123',NULL,NULL,NULL,NULL,0,1,1,'2023-06-19 00:58:00','2023-06-28 15:11:57',1);
INSERT INTO `t_material_stock` VALUES (30,'MSIN20230628002','4567890',1,NULL,'in',4,64,'IF20230612000001A1','wu','111','叠板2层',6.00,NULL,'无',14,'WareHouse','WareHouse',NULL,64,'MO20230615000001',NULL,NULL,NULL,NULL,0,1,1,'2023-06-19 15:47:22','2023-06-28 15:14:00',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify`
--

LOCK TABLES `t_notify` WRITE;
/*!40000 ALTER TABLE `t_notify` DISABLE KEYS */;
INSERT INTO `t_notify` VALUES (31,'2023-04-19 13:31:28','0001','AGV充电','AGV充电',22,'钉钉',0,1,1,'2023-04-19 01:31:42','2023-05-05 02:48:27',1,NULL);
INSERT INTO `t_notify` VALUES (32,'2023-06-18 13:05:31','0002','0002','002',23,'微信',0,1,1,'2023-06-18 01:04:38',NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知类型';
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
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='板料追踪';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_panel`
--

LOCK TABLES `t_panel` WRITE;
/*!40000 ALTER TABLE `t_panel` DISABLE KEYS */;
INSERT INTO `t_panel` VALUES (16,'P202304210001','PH2345678A1','1001','成品仓库','500',0,0,0,1,0,1,'2023-04-04 14:20:00','2023-05-05 02:04:22',1);
INSERT INTO `t_panel` VALUES (17,'123456787654322','PH2345678A1','1002','成品仓库','500',NULL,NULL,16,1,0,1,'2023-04-04 03:03:05','2023-04-18 01:40:49',1);
INSERT INTO `t_panel` VALUES (35,'P202304210002','PH2345678A1','1133','成品仓库','500',NULL,NULL,16,1,0,1,'2023-04-27 05:55:37','2023-05-05 02:04:33',1);
INSERT INTO `t_panel` VALUES (36,'P202304210003','PH2345678A1','1144','叠板区','200',NULL,NULL,16,1,0,1,'2023-04-27 08:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (37,'P202304210004','PH2345678A1','1155','拆板区','400',NULL,NULL,16,1,0,1,'2023-04-27 09:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (38,'P202304210005','PH2345678A1','1166','钻孔区','300',NULL,NULL,0,1,0,1,'2023-04-27 14:55:37',NULL,NULL);
INSERT INTO `t_panel` VALUES (39,'P202304210006','PH2345678A1','1177','钻孔区','300',NULL,NULL,0,1,0,1,'2023-04-28 10:41:31',NULL,NULL);
INSERT INTO `t_panel` VALUES (40,'P202304210007','PH2345678A1','1178','AGV','100',NULL,NULL,0,1,0,1,'2023-04-26 12:02:55',NULL,NULL);
INSERT INTO `t_panel` VALUES (41,'P202304210008','PH2345678A1','1179','AGV','100',NULL,NULL,0,1,0,1,'2023-04-26 15:54:31',NULL,NULL);
INSERT INTO `t_panel` VALUES (43,'P20230518000008','PH2345678A1','1180','钻孔区','300',NULL,NULL,NULL,1,0,1,'2023-05-11 04:53:25',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (21,'叠板','pin','',0,1,1,'2023-04-05 02:30:19','2023-06-20 10:00:19',32);
INSERT INTO `t_process` VALUES (23,'拆板','unpin','',0,1,1,'2023-04-18 21:35:18','2023-06-20 10:00:22',32);
INSERT INTO `t_process` VALUES (24,'钻孔','drill','钻孔要求',0,1,1,'2023-06-25 13:07:09','2023-06-28 14:56:38',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=85 DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
INSERT INTO `t_product_category` VALUES (72,0,'P01','单面板','单面板',0,0,1,'2023-05-09 03:40:40','2023-06-30 10:07:54',1,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route`
--

LOCK TABLES `t_route` WRITE;
/*!40000 ALTER TABLE `t_route` DISABLE KEYS */;
INSERT INTO `t_route` VALUES (2,'B0002','工艺路线B0002','叠板+钻孔+拆板','',1,0,1,'2023-04-04 15:00:09','2023-06-21 14:00:46',1);
INSERT INTO `t_route` VALUES (3,'A0001','工艺路线A0001','叠板+钻孔+拆板',NULL,1,0,1,'2023-04-05 01:57:50','2023-06-25 09:55:11',1);
INSERT INTO `t_route` VALUES (19,'C0003','工艺路线C0003','叠板+钻孔',NULL,1,0,33,'2023-04-23 02:34:38','2023-06-21 13:51:01',1);
INSERT INTO `t_route` VALUES (20,'D0001','单钻孔','钻孔',NULL,1,0,32,'2023-06-20 10:04:54','2023-06-21 09:27:33',33);
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_unique` (`route_id`,`process_id`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与工序关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_process`
--

LOCK TABLES `t_route_and_process` WRITE;
/*!40000 ALTER TABLE `t_route_and_process` DISABLE KEYS */;
INSERT INTO `t_route_and_process` VALUES (5,2,13,3,'1','#00AEF3',80,'0',4,1,0,1,'2023-04-05 02:41:06','2023-04-14 02:42:32',1);
INSERT INTO `t_route_and_process` VALUES (9,3,22,4,'1','#F31800',840,'0',0,0,0,1,'2023-04-07 02:40:29','2023-06-20 10:03:30',32);
INSERT INTO `t_route_and_process` VALUES (11,2,22,2,'1','#00AEF3',40,'0',3,1,0,1,'2023-04-10 22:58:49','2023-06-21 13:59:04',1);
INSERT INTO `t_route_and_process` VALUES (12,2,21,1,'0','#0A1C23',60,'0',2,1,0,1,'2023-04-10 22:59:09','2023-06-26 09:12:51',1);
INSERT INTO `t_route_and_process` VALUES (23,16,23,1,'1','#00AEF3',123,'1',4,1,0,1,'2023-04-20 04:47:28',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (27,19,22,2,'1','#D22525',180,'1',2,1,0,33,'2023-04-23 02:41:28','2023-05-18 02:18:28',1);
INSERT INTO `t_route_and_process` VALUES (28,19,21,1,'0','#6CD228',10,'0',2,1,0,33,'2023-04-23 02:42:00','2023-06-21 13:50:48',1);
INSERT INTO `t_route_and_process` VALUES (31,17,22,3,'1','#462828',20,'1',3,1,0,1,'2023-05-05 01:01:49','2023-05-05 01:01:59',1);
INSERT INTO `t_route_and_process` VALUES (32,18,21,1,'1','#4D1E1E',120,'1',1,1,0,1,'2023-05-05 01:09:44',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (33,2,23,3,'0','#853C3C',10,'1',3,1,0,1,'2023-06-13 21:44:55','2023-06-26 08:58:02',1);
INSERT INTO `t_route_and_process` VALUES (34,3,23,3,'0','#704848',120,'0',1,1,0,1,'2023-06-16 04:42:46','2023-06-16 04:47:17',1);
INSERT INTO `t_route_and_process` VALUES (38,3,21,1,'0','#D33737',120,'0',0,1,0,1,'2023-06-16 04:54:26','2023-07-04 11:33:48',1);
INSERT INTO `t_route_and_process` VALUES (39,20,22,1,'1','#061866',120,'0',0,1,0,32,'2023-06-20 10:20:55',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (40,3,24,2,'1','#907979',120,'0',2,1,0,1,'2023-06-25 13:10:05','2023-07-04 11:08:33',1);
INSERT INTO `t_route_and_process` VALUES (41,20,24,1,'1','#16274F',120,'1',0,1,0,1,'2023-06-25 16:35:43',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (42,2,24,2,'1','#EEC1C1',40,'1',0,1,0,1,'2023-06-25 16:40:07','2023-06-25 16:40:20',1);
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_unique` (`route_id`,`product_category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=186 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与产品大类关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_product_category`
--

LOCK TABLES `t_route_and_product_category` WRITE;
/*!40000 ALTER TABLE `t_route_and_product_category` DISABLE KEYS */;
INSERT INTO `t_route_and_product_category` VALUES (52,20,77,0,0,33,'2023-06-21 09:27:22','2023-06-21 09:27:28',33);
INSERT INTO `t_route_and_product_category` VALUES (53,20,79,1,0,1,'2023-06-25 11:00:57',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (54,20,80,1,0,1,'2023-06-25 11:00:57',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (55,2,72,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (56,2,74,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (57,2,75,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (58,2,76,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (59,2,77,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (60,2,78,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (61,2,79,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (62,2,80,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (63,2,81,1,0,1,'2023-06-27 17:04:25',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (163,19,74,1,0,1,'2023-07-04 11:41:49',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (164,19,75,1,0,1,'2023-07-04 11:41:49',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (165,19,80,1,0,1,'2023-07-04 11:41:49',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (181,3,74,1,0,1,'2023-07-04 11:42:59',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (182,3,78,1,0,1,'2023-07-04 11:42:59',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (183,3,79,1,0,1,'2023-07-04 11:42:59',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (184,3,80,1,0,1,'2023-07-04 11:42:59',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (185,3,81,1,0,1,'2023-07-04 11:42:59',NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COMMENT='工序与工作站关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_process_and_work_station`
--

LOCK TABLES `t_route_process_and_work_station` WRITE;
/*!40000 ALTER TABLE `t_route_process_and_work_station` DISABLE KEYS */;
INSERT INTO `t_route_process_and_work_station` VALUES (1,27,217,0,1,0,1,'2023-04-05 13:38:09',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (2,27,233,0,0,0,1,'2023-04-05 13:38:18','2023-04-07 08:58:07',1);
INSERT INTO `t_route_process_and_work_station` VALUES (10,27,212,1,1,0,1,'2023-04-11 22:35:54',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (11,28,218,1,1,0,1,'2023-04-11 22:36:12',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (28,29,219,1,1,0,1,'2023-04-18 23:05:31',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (34,13,233,1,1,0,1,'2023-05-17 03:55:49',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (36,10,218,1,1,0,1,'2023-06-16 04:40:26',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (37,9,233,1,1,0,1,'2023-06-16 04:40:43',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (38,38,218,1,1,0,1,'2023-06-18 02:28:29',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (39,34,219,1,1,0,1,'2023-06-18 02:28:40',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (41,9,217,1,1,0,1,'2023-06-18 02:28:57',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (49,12,218,1,1,0,1,'2023-06-18 22:12:54','2023-06-21 13:58:42',1);
INSERT INTO `t_route_process_and_work_station` VALUES (54,39,217,1,1,0,1,'2023-06-21 13:56:11',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (55,11,217,1,1,0,1,'2023-06-21 13:59:03',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (56,33,219,1,1,0,1,'2023-06-21 14:00:39',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (58,42,212,1,1,0,1,'2023-06-25 16:40:19',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (59,40,217,1,1,0,1,'2023-06-25 16:40:28',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (60,40,233,1,1,0,1,'2023-06-25 16:40:31',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (61,40,212,1,1,0,1,'2023-06-25 16:40:34',NULL,NULL);
INSERT INTO `t_route_process_and_work_station` VALUES (62,41,233,1,1,0,1,'2023-06-25 16:40:48',NULL,NULL);
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
  `scheduled_task_status` int(11) DEFAULT '0' COMMENT '调度任务状态',
  `allocate_time` datetime DEFAULT NULL COMMENT '分配时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `routing_key` varchar(100) DEFAULT NULL COMMENT '主叫设备的route key',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_schedulement`
--

LOCK TABLES `t_schedulement` WRITE;
/*!40000 ALTER TABLE `t_schedulement` DISABLE KEYS */;
INSERT INTO `t_schedulement` VALUES (3,'code1','MockDrill01','MockAgv02',NULL,'1','17',NULL,NULL,0,0,0,1,'2023-06-12 13:25:38','2023-07-01 15:55:55',1,0,NULL,NULL,NULL,NULL,'2023-07-01 15:55:55',NULL);
INSERT INTO `t_schedulement` VALUES (4,'code2','MockDrill02','MockAgv02',NULL,'18','19',NULL,NULL,0,0,0,1,'2023-06-12 13:25:48','2023-07-01 14:55:04',NULL,1,NULL,NULL,NULL,NULL,'2023-07-01 14:55:04',NULL);
INSERT INTO `t_schedulement` VALUES (5,'code4','MockDrill04','MockAgv04',NULL,'20','23',NULL,NULL,NULL,0,0,1,'2023-06-12 23:36:46','2023-07-01 09:11:21',NULL,-1,NULL,NULL,NULL,NULL,'2023-07-01 09:11:21',NULL);
INSERT INTO `t_schedulement` VALUES (6,'code3','MockDrill03','MockAgv03',NULL,'19','21',NULL,NULL,0,0,0,1,'2023-06-12 13:25:48','2023-07-01 14:55:04',NULL,4,NULL,NULL,NULL,NULL,'2023-07-01 14:55:04',NULL);
INSERT INTO `t_schedulement` VALUES (7,'code5','MockDrill05','MockAgv05',NULL,'24','26',NULL,NULL,0,0,0,1,'2023-06-12 23:36:46','2023-07-01 09:11:21',NULL,2,NULL,NULL,NULL,NULL,'2023-07-01 09:11:21',NULL);
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
  `is_started` tinyint(4) DEFAULT '0' COMMENT '任务是否开启',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=578 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产任务';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_task`
--

LOCK TABLES `t_task` WRITE;
/*!40000 ALTER TABLE `t_task` DISABLE KEYS */;
INSERT INTO `t_task` VALUES (510,'TO20230629000001',0,'TO20230629000001',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',100,1200,232,NULL,21,56,'王老板','KH20230613000001','2023-06-29 09:54:37',1,'2023-07-02 04:34:37',NULL,'COMMITED','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-30 13:47:04',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (511,'TO20230629000002',0,'TO20230629000002',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-06-29 09:54:37',1,'2023-06-29 09:54:37',NULL,'COMMITED','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-30 13:32:45',1,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (512,'TO20230629000003',0,'TO20230629000003',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-06-29 09:54:37',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (513,'TO20230629000004',0,'TO20230629000004',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (514,'TO20230629000005',0,'TO20230629000005',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (515,'TO20230629000006',0,'TO20230629000006',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-06-29 09:54:37',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (516,'TO20230629000007',0,'TO20230629000007',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (517,'TO20230629000008',0,'TO20230629000008',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-06-30 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (518,'TO20230629000009',0,'TO20230629000009',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (519,'TO20230629000010',0,'TO20230629000010',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (520,'TO20230629000011',0,'TO20230629000011',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (521,'TO20230629000012',0,'TO20230629000012',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (522,'TO20230629000013',0,'TO20230629000013',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (523,'TO20230629000014',0,'TO20230629000014',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (524,'TO20230629000015',0,'TO20230629000015',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (525,'TO20230629000016',0,'TO20230629000016',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (526,'TO20230629000017',0,'TO20230629000017',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (527,'TO20230629000018',0,'TO20230629000018',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (528,'TO20230629000019',0,'TO20230629000019',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (529,'TO20230629000020',0,'TO20230629000020',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (530,'TO20230629000021',0,'TO20230629000021',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (531,'TO20230629000022',0,'TO20230629000022',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (532,'TO20230629000023',0,'TO20230629000023',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (533,'TO20230629000024',0,'TO20230629000024',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (534,'TO20230629000025',0,'TO20230629000025',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (535,'TO20230629000026',0,'TO20230629000026',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (536,'TO20230629000027',0,'TO20230629000027',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (537,'TO20230629000028',0,'TO20230629000028',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (538,'TO20230629000029',0,'TO20230629000029',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (539,'TO20230629000030',0,'TO20230629000030',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (540,'TO20230629000031',0,'TO20230629000031',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (541,'TO20230629000032',0,'TO20230629000032',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-03 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (542,'TO20230629000033',0,'TO20230629000033',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (543,'TO20230629000034',0,'TO20230629000034',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-06-29 09:54:37',NULL,'DRAFT','#EEC1C1','1',1,1,1,'2023-06-29 09:54:37','2023-06-29 09:56:02',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (544,'TO20230701000001',0,'TO20230701000001',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:27:44',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (545,'TO20230701000002',0,'TO20230701000002',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:27:45',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (546,'TO20230701000003',0,'TO20230701000003',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 13:36:15',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15',NULL,NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (547,'TO20230701000004',0,'TO20230701000004',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-07 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:07',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (548,'TO20230701000005',0,'TO20230701000005',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-08 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:31',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (549,'TO20230701000006',0,'TO20230701000006',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 13:36:15',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15',NULL,NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (550,'TO20230701000007',0,'TO20230701000007',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-08 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:31',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (551,'TO20230701000008',0,'TO20230701000008',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:35',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (552,'TO20230701000009',0,'TO20230701000009',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:12',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (553,'TO20230701000010',0,'TO20230701000010',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:25',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (554,'TO20230701000011',0,'TO20230701000011',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:25',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (555,'TO20230701000012',0,'TO20230701000012',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 13:36:15',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15',NULL,NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (556,'TO20230701000013',0,'TO20230701000013',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:25',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (557,'TO20230701000014',0,'TO20230701000014',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:25',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (558,'TO20230701000015',0,'TO20230701000015',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:12',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (559,'TO20230701000016',0,'TO20230701000016',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-04 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:35',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (560,'TO20230701000017',0,'TO20230701000017',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-11 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-05 09:06:41',50,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (561,'TO20230701000018',0,'TO20230701000018',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:12',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (562,'TO20230701000019',0,'TO20230701000019',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-07 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:30',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (563,'TO20230701000020',0,'TO20230701000020',100,'MO20230629000001','6/29测试001',NULL,233,'钻机01-0003','drill01-0003',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-07 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:30',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (564,'TO20230701000021',0,'TO20230701000021',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:12',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (565,'TO20230701000022',0,'TO20230701000022',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:19',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (566,'TO20230701000023',0,'TO20230701000023',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-05 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:19',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (567,'TO20230701000024',0,'TO20230701000024',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:12',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (568,'TO20230701000025',0,'TO20230701000025',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-11 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-05 09:06:41',50,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (569,'TO20230701000026',0,'TO20230701000026',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-06 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:17',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (570,'TO20230701000027',0,'TO20230701000027',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-02 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:12',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (571,'TO20230701000028',0,'TO20230701000028',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-06 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:17',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (572,'TO20230701000029',0,'TO20230701000029',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 13:36:15',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15',NULL,NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (573,'TO20230701000030',0,'TO20230701000030',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:41',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (574,'TO20230701000031',0,'TO20230701000031',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-09 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:26',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (575,'TO20230701000032',0,'TO20230701000032',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 13:36:15',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15',NULL,NULL,NULL,NULL,NULL,NULL,0);
INSERT INTO `t_task` VALUES (576,'TO20230701000033',0,'TO20230701000033',100,'MO20230629000001','6/29测试001',NULL,212,'钻机01-0001','drill01-0001',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-01 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:28:41',1,'0',NULL,NULL,NULL,NULL);
INSERT INTO `t_task` VALUES (577,'TO20230701000034',0,'TO20230701000034',100,'MO20230629000001','6/29测试001',NULL,217,'钻机01-0002','drill01-0002',24,'钻孔','drill',68,'6/29测试001','MO20230629000001',40,'叠板数2','叠板2层',1000,1200,200,NULL,0,56,'王老板','KH20230613000001','2023-07-09 00:00:00',1,'2023-07-01 13:36:15',NULL,'DRAFT','#EEC1C1','1',0,1,1,'2023-07-01 13:36:15','2023-07-04 14:29:26',1,'0',NULL,NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (34,'叠板2层','叠板2层','N',74,2.0000,NULL,0,1,1,'2023-04-18 22:13:06','2023-06-30 10:07:09',1);
INSERT INTO `t_unit_measure` VALUES (74,'Panel','单层Panel','Y',34,0.0000,'单位',0,1,1,'2023-05-21 22:46:43','2023-06-21 13:44:26',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COMMENT='仓库表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_warehouse`
--

LOCK TABLES `t_warehouse` WRITE;
/*!40000 ALTER TABLE `t_warehouse` DISABLE KEYS */;
INSERT INTO `t_warehouse` VALUES (2,'WH02','成品二库','string','','',0,1,1,'2023-04-03 14:03:07','2023-06-20 10:31:12',32);
INSERT INTO `t_warehouse` VALUES (14,'WH01','成品一库','钻机01-0003','tony','',0,1,1,'2023-04-18 23:36:14','2023-06-28 15:17:44',1);
INSERT INTO `t_warehouse` VALUES (15,'WH03','钻孔生料暂存一库','钻孔生料缓存一区',NULL,NULL,0,1,32,'2023-06-20 10:59:22',NULL,NULL);
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
  `panel_count` decimal(12,2) DEFAULT NULL COMMENT '叠板数',
  `wad_count` decimal(12,2) DEFAULT NULL COMMENT '待排产叠数',
  `drill_count` decimal(12,2) DEFAULT NULL COMMENT '孔数',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `ancestors` varchar(255) DEFAULT NULL COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=101 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产工单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_work_order`
--

LOCK TABLES `t_work_order` WRITE;
/*!40000 ALTER TABLE `t_work_order` DISABLE KEYS */;
INSERT INTO `t_work_order` VALUES (100,'6/29测试001',0,'MO20230629000001','库存需求','1231',68,'板材单钻孔（叠板数2）','IF20230620000005A1',40,NULL,'叠板数2','叠板2层',1000,1200,200,6800,56,'王老板','KH20230613000001','2023-07-08 00:00:00','DRAFT',0,1,1,'2023-07-01 13:36:15',6.00,0.00,10000.00,1,1,'2023-06-29 09:54:07','2023-06-30 13:32:45',1,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
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
) ENGINE=InnoDB AUTO_INCREMENT=245 DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workstation`
--

LOCK TABLES `t_workstation` WRITE;
/*!40000 ALTER TABLE `t_workstation` DISABLE KEYS */;
INSERT INTO `t_workstation` VALUES (212,'drill01-0001','钻机01-0001',9,'shop02','车间02（测试）','',0,1,1,'2023-04-04 02:32:34','2023-05-23 21:31:20',1);
INSERT INTO `t_workstation` VALUES (217,'drill01-0002','钻机01-0002',9,'shop02','车间02（测试）',NULL,0,1,1,'2023-04-05 22:37:47','2023-05-23 21:31:14',1);
INSERT INTO `t_workstation` VALUES (218,'pin01-0001','叠板机01-0001',9,'shop02','车间01（测试）',NULL,0,1,1,'2023-04-05 22:39:06','2023-06-20 10:33:40',32);
INSERT INTO `t_workstation` VALUES (219,'unpin01-0001','拆板机01-0001',9,'shop02','车间03（测试）',NULL,0,1,1,'2023-04-05 22:39:38','2023-06-21 13:59:36',1);
INSERT INTO `t_workstation` VALUES (233,'drill01-0003','钻机01-0003',9,'shop02','车间02（测试）','钻机工作站',0,1,1,'2023-05-09 03:07:07',NULL,NULL);
INSERT INTO `t_workstation` VALUES (234,'agv01-001','AGV01-001',9,'shop02','车间02（测试）','AGV',0,1,1,'2023-05-09 03:07:07','2023-06-20 10:33:50',32);
INSERT INTO `t_workstation` VALUES (235,'WS20230620000001','成品一库',NULL,NULL,'车间03（测试）',NULL,0,1,32,'2023-06-20 10:31:52','2023-06-20 10:34:09',32);
INSERT INTO `t_workstation` VALUES (236,'WS20230620000002','成品二库',NULL,NULL,'车间03（测试）',NULL,0,1,32,'2023-06-20 10:32:09','2023-06-20 10:34:13',32);
INSERT INTO `t_workstation` VALUES (237,'WS20230620000003','钻孔生料缓存一区',NULL,NULL,'车间02（测试）',NULL,0,1,32,'2023-06-20 10:33:10',NULL,NULL);
INSERT INTO `t_workstation` VALUES (238,'WS20230620000004','钻孔生料缓存二区',NULL,NULL,'车间02（测试）',NULL,0,1,32,'2023-06-20 10:33:22',NULL,NULL);
INSERT INTO `t_workstation` VALUES (239,'WS20230620000005','钻孔熟料缓存一区',NULL,NULL,'车间03（测试）',NULL,0,1,32,'2023-06-20 10:34:39',NULL,NULL);
INSERT INTO `t_workstation` VALUES (240,'WS20230620000007','钻孔熟料缓存二区',NULL,NULL,'车间03（测试）',NULL,0,1,32,'2023-06-20 10:34:49',NULL,NULL);
INSERT INTO `t_workstation` VALUES (242,'WS20230620000008','原料一库',NULL,NULL,'车间01（测试）',NULL,0,1,32,'2023-06-20 11:01:23',NULL,NULL);
INSERT INTO `t_workstation` VALUES (243,'WS20230620000009','原料二库',NULL,NULL,'车间01（测试）',NULL,0,1,32,'2023-06-20 11:01:37',NULL,NULL);
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

-- Dump completed on 2023-07-05  2:21:22
