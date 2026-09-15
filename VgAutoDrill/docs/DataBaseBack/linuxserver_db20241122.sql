Enter password: 
-- MySQL dump 10.13  Distrib 5.7.42, for Linux (x86_64)
--
-- Host: 127.0.0.1    Database: vg_autodrill_db
-- ------------------------------------------------------
-- Server version	5.7.42

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
  `module` int(11) DEFAULT NULL COMMENT '模块',
  `is_quick_config` tinyint(1) DEFAULT '0' COMMENT '是否快速配置',
  `category` varchar(20) DEFAULT '1' COMMENT '类别（1-default,2-Kinwong,3-Chognda)',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`config_code`)
) ENGINE=InnoDB AUTO_INCREMENT=126 DEFAULT CHARSET=utf8mb4 COMMENT='系统配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_config`
--

LOCK TABLES `sys_config` WRITE;
/*!40000 ALTER TABLE `sys_config` DISABLE KEYS */;
INSERT INTO `sys_config` VALUES (1,'CentralControlSystemIsMaintain','false','bool','中控系统是否在维护',NULL,'中控系统是否在维护',0,1,1,'2023-10-18 15:35:48','2024-10-22 10:36:48',1,0,NULL,1,'1');
INSERT INTO `sys_config` VALUES (2,'CentralControlSystemCreatedTimeout','3600','int','中控系统已上报调度的超时设定(秒)',NULL,'中控系统已上报调度的超时设定(秒)',0,1,1,'2023-10-18 15:37:29','2024-10-22 10:36:48',1,0,NULL,1,'1');
INSERT INTO `sys_config` VALUES (3,'CentralControlSystemAllocatedTimeout','100','int','中控系统已分配的调度的超时设定(秒)',NULL,'中控系统已分配的调度的超时设定(秒)',0,1,1,'2023-10-18 15:38:04','2024-04-11 13:58:27',59,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (21,'AGVDeviceNowStatus','0','enum','在线','-1,未知;0,在线;1,已离线;2,待机;3,工作中;4,故障;5,低电量;6,充电中;7,设备维护;8,停机','AGV',0,1,1,'2023-10-19 11:40:10','2024-10-17 14:32:54',1,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (22,'IsShowTimelyInformation','false','bool','是否显示告警等及时信息',NULL,'是否显示告警等及时信息',0,1,1,'2023-10-19 11:40:10','2024-10-22 10:36:48',1,0,NULL,1,'1');
INSERT INTO `sys_config` VALUES (23,'AGVLatestScheduleCount','5','int','AGV最近调度记录条数',NULL,'AGV最近调度记录条数',0,1,1,'2023-11-09 11:40:10','2024-07-29 14:25:40',1,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (24,'DrillTaskShaftCount','5','int','钻孔任务轴数',NULL,'钻孔任务轴数',0,1,1,'2023-11-13 11:40:10','2024-07-29 14:25:44',1,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (25,'DrillDeviceTaskShowCount','100','int','在线钻机待做任务显示个数',NULL,'在线钻机待做任务显示个数',0,1,1,'2023-11-13 11:40:10','2024-03-26 10:06:09',59,NULL,NULL,0,'1');
INSERT INTO `sys_config` VALUES (26,'AGVDeviceSiloInfoShowCount','20','int','在线AGV板料信息及最近调度记录显示个数2',NULL,'在线AGV板料信息及最近调度记录显示个数',0,1,1,'2023-11-13 11:40:10','2024-03-30 15:03:43',59,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (27,'ExternalWorkOrderInterval','300','int','外部工单轮询处理间隔时间（秒）',NULL,'外部工单轮询处理间隔时间（秒）',0,1,1,'2024-02-29 13:21:21','2024-04-28 11:36:38',59,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (30,'QueryLayerNumList','0|1','string','导入指定层数的外部工单',NULL,'导入指定层数的外部工单，分隔符|',0,1,1,'2024-09-11 14:08:43',NULL,NULL,1,NULL,0,'1');
INSERT INTO `sys_config` VALUES (31,'AgvChargingPosition','00030|00010','string','Agv充电点位',NULL,'Agv充电点位，分隔符|',0,1,1,'2024-09-11 14:08:43',NULL,NULL,1,NULL,0,'1');
INSERT INTO `sys_config` VALUES (44,'崇达123','1','int','','','崇达123',0,1,1,'2024-10-17 14:20:50','2024-10-17 16:19:04',1,0,NULL,0,'3');
INSERT INTO `sys_config` VALUES (48,'景旺123','1','int','','','景旺123',0,1,1,'2024-10-17 14:45:27','2024-10-17 14:45:27',NULL,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (49,'EnableKinwongStockInfoQuery','false','bool','是否开启库存信息查询',NULL,'是否开启库存信息查询',0,1,1,'2024-10-18 16:00:22','2024-11-08 08:48:41',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (50,'URLKinwongStockInfoQuery','http://192.168.102.68:5001/api/preproc/stockInfoQuery','string','库存信息查询地址',NULL,'库存信息查询地址',0,1,1,'2024-10-18 16:00:22','2024-10-18 10:34:34',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (52,'StockInfoQueryTargetPosAreaCode','J03-DR-IN-Schedule|J04-DR-IN-Schedule','string','库存信息查询区域或策略编码',NULL,'库存信息查询区域或策略编码，分隔符|',0,1,1,'2024-10-18 16:00:22',NULL,NULL,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (53,'StockInfoQueryTargetPosArea','${04}','string','库存信息查询目标区域',NULL,'库存信息查询目标区域，区域编号${04}，策略编号${02}',0,1,1,'2024-10-18 16:00:22',NULL,NULL,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (54,'DrillCount','10000','int','孔数',NULL,'生产工单默认钻孔孔数',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'20');
INSERT INTO `sys_config` VALUES (55,'SingleTripTime','40','int','单趟耗时 单位min',NULL,'单趟耗时 单位min',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'20');
INSERT INTO `sys_config` VALUES (56,'ProduceTaskType','WadCount','string','生成钻孔任务的方式：UsableCount可用叠数/WadCount计划叠数',NULL,'生成钻孔任务的方式：UsableCount可用叠数/WadCount计划叠数',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'20');
INSERT INTO `sys_config` VALUES (57,'RefreshStock','false','bool','是否刷新库存',NULL,'是否刷新库存',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'20');
INSERT INTO `sys_config` VALUES (58,'AutoVettingDrillTask','true','bool','是否自动审批钻孔任务',NULL,'是否自动审批钻孔任务',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'20');
INSERT INTO `sys_config` VALUES (59,'ImportStatus','true','bool','导入的数据默认状态为:可用',NULL,'导入的数据默认状态为:可用',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'1');
INSERT INTO `sys_config` VALUES (60,'DirectBuildTask','true','bool','导入外部工单，是否直接生成排产任务',NULL,'导入外部工单，是否直接生成排产任务',0,1,1,'2024-10-22 16:00:22','2024-11-09 09:56:38',1,0,NULL,0,'1');
INSERT INTO `sys_config` VALUES (61,'DefaultInteractionPosition','0','int','钻机交互位置 0：后上料；1：前上料',NULL,'钻机交互位置 0：后上料；1：前上料',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'30');
INSERT INTO `sys_config` VALUES (87,'JingWangTransferDrillFileEnable','false','bool','是否开启获取钻带参数',NULL,'是否开启获取钻带参数',0,1,1,'2024-10-22 16:00:22','2024-10-25 08:47:32',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (88,'JingWangTransferDrillFileUrl','https://192.168.1.39:5001/api/preproc/convstatus?pgm=','string','获取钻带参数请求路径',NULL,'获取钻带参数请求路径',0,1,1,'2024-10-22 16:00:22','2024-10-23 13:38:02',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (89,'JiangXiKinWongWIPEnable','true','bool','是否开启导入WIP外部工单',NULL,'是否开启导入WIP外部工单',0,1,1,'2024-10-22 16:00:22','2024-10-27 15:29:23',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (90,'JiangXiKinWongWIPUrl','http://192.168.102.68:5001/api/preproc/kwDrWipQuery','string','导入WIP外部工单请求路径',NULL,'导入WIP外部工单请求路径',0,1,1,'2024-10-22 16:00:22','2024-10-27 13:23:44',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (91,'JingWangSetMoveInLotEnable','false','bool','是否开启SetMoveInLot',NULL,'是否开启SetMoveInLot',0,1,1,'2024-10-22 16:00:22','2024-11-09 09:55:58',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (92,'JingWangSetMoveInLotUrl','http://192.168.102.68:5001/api/TestJWangApiController/SignMoveInLot','string','SetMoveInLot请求路径',NULL,'SetMoveInLot请求路径',0,1,1,'2024-10-22 16:00:22','2024-10-27 15:14:46',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (93,'JingWangSetMoveOutLotEnable','false','bool','是否开启SetMoveOutLot',NULL,'是否开启SetMoveOutLot',0,1,1,'2024-10-22 16:00:22','2024-10-27 15:28:31',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (94,'JingWangSetMoveOutLotUrl','http://192.168.102.68:5001/api/TestJWangApiController/SignMoveOutLot','string','SetMoveOutLot请求路径',NULL,'SetMoveOutLot请求路径',0,1,1,'2024-10-22 16:00:22','2024-10-27 15:14:39',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (95,'JingWangSetTrackInLotEnable','false','bool','是否开启SetTrackInLot',NULL,'是否开启SetTrackInLot',0,1,1,'2024-10-22 16:00:22','2024-11-09 09:56:02',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (96,'JingWangSetTrackInLotUrl','http://192.168.102.68:5001/api/TestJWangApiController/SignTrackInLot','string','SetTrackInLot请求路径',NULL,'SetTrackInLot请求路径',0,1,1,'2024-10-22 16:00:22','2024-10-27 15:14:12',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (97,'JingWangSetTrackOutLotEnable','false','bool','是否开启SetTrackOutLot',NULL,'是否开启SetTrackOutLot',0,1,1,'2024-10-22 16:00:22','2024-11-09 09:56:06',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (98,'JingWangSetTrackOutLotUrl','http://192.168.102.68:5001/api/TestJWangApiController/SignTrackOutLot','string','SetTrackOutLot请求路径',NULL,'SetTrackOutLot请求路径',0,1,1,'2024-10-22 16:00:22','2024-10-27 15:14:24',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (99,'CentralGetRackPanelsUrl','http://192.168.102.178:8001/central/stat/location?code=','string','获取料仓实时板料信息请求路径',NULL,'获取料仓实时板料信息请求路径',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (100,'CentralAllotsDeviceCommand','http://192.168.102.178:8001/v1/central/app/command','string','下发设备指令请求路径',NULL,'下发设备指令请求路径',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (102,'CentralAllotsPanelData','http://192.168.102.178:8001/v1/central/schedule/AllotsPanelData','string','下发板料数据到设备请求路径',NULL,'下发板料数据到设备请求路径',0,1,1,'2024-10-22 16:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (103,'JingWangGenAgvSchedulingTaskEnable','false','bool','是否开启移动料仓',NULL,'是否开启移动料仓',0,1,1,'2024-10-25 16:00:22','2024-11-08 08:48:02',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (104,'JingWangGenAgvSchedulingTaskUrl','http://192.168.102.68:5001/api/preproc/GenAgvSchedulingTaskBatch','string','移动料仓请求路径',NULL,'移动料仓请求路径',0,1,1,'2024-10-25 16:00:22','2024-10-27 13:07:23',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (105,'CountOfEmptyForkAtLeast','3','int','叉齿分区最少空库位数量',NULL,'叉齿分区最少空库位数量，默认1',0,1,1,'2024-10-25 16:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (106,'JingWangGenAgvSchedulingTask_wbCode','A_01','string','移动料仓：呼叫站点',NULL,'移动料仓：呼叫站点',0,1,1,'2024-10-25 16:00:22',NULL,NULL,1,NULL,0,'2');
INSERT INTO `sys_config` VALUES (107,'JingWangGenAgvSchedulingTask_taskTyp','P011','string','移动料仓：任务类型',NULL,'移动料仓：任务类型，与在RCS-2000端配置的主任务类型编号一致',0,1,1,'2024-10-25 16:00:22',NULL,NULL,1,NULL,0,'2');
INSERT INTO `sys_config` VALUES (108,'CentralGetSimpleLocations','http://192.168.102.178:8001/central/stat/location/simple','string','获取料仓数据请求路径',NULL,'获取料仓数据请求路径',0,1,1,'2024-10-29 16:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (109,'EnableNotWIPData','true','bool','是否显示非WIP外部工单',NULL,'是否显示非WIP外部工单',0,1,1,'2024-10-31 16:00:22','2024-11-01 16:11:58',1,0,NULL,0,'2');
INSERT INTO `sys_config` VALUES (110,'StockInfoQueryWithoutAreaEnable','true','bool','是否过滤接口无法识别的传入区域',NULL,'是否过滤接口无法识别的传入区域',0,1,1,'2024-10-31 16:00:22',NULL,NULL,1,NULL,0,'2');
INSERT INTO `sys_config` VALUES (111,'XianJinSetBeginLoadPanel','http://localhost:8001/v1/external/xianjin/SetBeginLoadPanel','string','设置上料开始请求路径',NULL,'设置上料开始请求路径',0,1,1,'2024-11-07 16:00:22','2024-11-09 10:05:48',1,0,NULL,0,'4');
INSERT INTO `sys_config` VALUES (112,'XianJinUnderClinkerPanel','http://localhost:8001/v1/external/xianjin/UnderClinkerPanel?deviceId=','string','下熟料请求路径',NULL,'下熟料请求路径',0,1,1,'2024-11-07 16:00:22','2024-11-09 10:06:06',1,0,NULL,0,'4');
INSERT INTO `sys_config` VALUES (113,'XianJinSetCompleteUnderClinkerPanel','http://localhost:8001/v1/external/xianjin/SetCompleteUnderClinkerPanel?deviceId=','string','下熟料结束请求路径',NULL,'下熟料结束请求路径',0,1,1,'2024-11-07 16:00:22','2024-11-09 10:05:57',1,0,NULL,0,'4');
INSERT INTO `sys_config` VALUES (114,'MaxPartitionEmptySiloNum','2','int','最多空料仓的数量',NULL,'最多空料仓的数量',0,1,1,'2023-11-06 11:40:10',NULL,NULL,10,NULL,0,'1');
INSERT INTO `sys_config` VALUES (115,'MinPartitionEmptySiloNum','1','int','最少空料仓的数量',NULL,'最少空料仓的数量',0,1,1,'2023-11-06 11:40:10',NULL,NULL,10,NULL,0,'1');
INSERT INTO `sys_config` VALUES (116,'EnableSystemAnalysis','false','bool','是否启用系统分析报告',NULL,'是否启用系统分析报告',0,1,1,'2024-11-06 00:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (117,'AgvStandbyTimeout','10','int','Agv等待超时时长(秒)',NULL,'Agv等待超时时长(秒)',0,1,1,'2023-11-05 11:40:10',NULL,NULL,10,NULL,0,'1');
INSERT INTO `sys_config` VALUES (118,'CentralVerifyFunction01','false','bool','CentralVerifyFunction01',NULL,'CentralVerifyFunction01',0,1,1,'2024-11-05 00:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (119,'CentralVerifyFunction02','false','bool','CentralVerifyFunction02',NULL,'CentralVerifyFunction02',0,1,1,'2024-11-05 00:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (120,'CentralVerifyFunction03','false','bool','CentralVerifyFunction03',NULL,'CentralVerifyFunction03',0,1,1,'2024-11-05 00:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (121,'CentralVerifyFunction04','false','bool','CentralVerifyFunction04',NULL,'CentralVerifyFunction04',0,1,1,'2024-11-05 00:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (122,'CentralVerifyFunction05','false','bool','CentralVerifyFunction05',NULL,'CentralVerifyFunction05',0,1,1,'2024-11-05 00:00:22',NULL,NULL,1,NULL,0,'10');
INSERT INTO `sys_config` VALUES (123,'XianJinSendDeviceCommand','true','bool','中控是否下发板料校验结果、板料进度',NULL,'中控是否下发板料校验结果、板料进度',0,1,1,'2024-11-08 00:00:22','2024-11-08 11:03:59',1,0,NULL,0,'4');
INSERT INTO `sys_config` VALUES (124,'JingWangkwAGVStockInUrl','http://10.50.220.20:8008/MesService.svc/kwAGVStockIn','string','上传mes转仓接口请求路径',NULL,'上传mes转仓接口请求路径',0,1,1,'2024-11-14 16:00:22',NULL,NULL,1,NULL,0,'2');
INSERT INTO `sys_config` VALUES (125,'JingWangHoldLotUrl','http://10.50.220.20:8008/MesService.svc/HoldLot','string','上传mes暂停该lot请求路径',NULL,'上传mes暂停该lot请求路径',0,1,1,'2024-11-14 16:00:22',NULL,NULL,1,NULL,0,'2');
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
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='部门';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_department`
--

LOCK TABLES `sys_department` WRITE;
/*!40000 ALTER TABLE `sys_department` DISABLE KEYS */;
INSERT INTO `sys_department` VALUES (1,'维嘉科技',0,'15888888888','boss@qq.com',NULL,'张三',0,1,0,'2022-09-26 11:05:59','2024-08-23 11:49:05',1);
INSERT INTO `sys_department` VALUES (2,'苏州总公司',1,'15888888888','boss@qq.com',NULL,'王五',0,1,0,'2022-09-26 11:06:00','2024-09-19 17:14:47',1);
INSERT INTO `sys_department` VALUES (13,'研发部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:57:35','2023-10-24 08:48:51',1);
INSERT INTO `sys_department` VALUES (14,'市场部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:58:27',NULL,NULL);
INSERT INTO `sys_department` VALUES (15,'运营部门',2,'13909877869','13400000001@qq.com',NULL,'张三',0,1,1,'2023-07-28 10:59:14',NULL,NULL);
INSERT INTO `sys_department` VALUES (18,'qqqqqqqqqq',18,'13909877869','13400000001@qq.com',NULL,'qqqq',0,1,1,'2023-07-28 15:37:21','2023-07-28 15:37:48',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=1603 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='菜单';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu`
--

LOCK TABLES `sys_menu` WRITE;
/*!40000 ALTER TABLE `sys_menu` DISABLE KEYS */;
INSERT INTO `sys_menu` VALUES (1,'DRILLAMIN','系统管理',0,'system','system','#',1,NULL,NULL,100,0,1,0,'2022-09-26 14:56:09','2024-08-23 11:35:03',1);
INSERT INTO `sys_menu` VALUES (2,'DRILLAMIN','用户管理',1,'user','user','system/user/index',2,'system:user:list',NULL,1,0,1,0,'2022-09-26 15:09:01','2022-12-26 16:53:55',1);
INSERT INTO `sys_menu` VALUES (3,'DRILLAMIN','角色管理',1,'peoples','role','system/role/index',2,'system:role:list',NULL,2,0,1,0,'2022-09-26 15:10:37','2023-04-04 09:36:27',1);
INSERT INTO `sys_menu` VALUES (4,'DRILLAMIN','菜单管理',1,'tree-table','menu','system/menu/index',2,'system:menu:list',NULL,4,0,1,NULL,'2022-09-26 15:11:14','2023-03-01 13:53:58',1);
INSERT INTO `sys_menu` VALUES (5,'DRILLAMIN','部门管理',1,'tree','dept','system/dept/index',2,'system:dept:list',NULL,5,0,1,NULL,'2022-09-26 15:12:26','2023-03-01 13:57:19',1);
INSERT INTO `sys_menu` VALUES (6,'DRILLAMIN','岗位管理',1,'post','post','system/post/index',2,'system:post:list',NULL,6,0,1,NULL,'2022-09-26 15:13:20','2023-03-01 13:57:26',1);
INSERT INTO `sys_menu` VALUES (7,'CRM','系统管理',0,'system',NULL,'',1,NULL,NULL,1,0,1,0,'2022-09-26 14:56:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (8,'DRILLAMIN','应用管理',1,'','app','system/app/index',2,'system:app:list',NULL,7,0,0,0,'2022-09-26 14:56:09','2024-08-07 11:47:33',1);
INSERT INTO `sys_menu` VALUES (10,'DRILLAMIN','应用新增',8,'#',NULL,'#',3,'system:app:add',NULL,2,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (11,'DRILLAMIN','应用修改',8,'#',NULL,'#',3,'system:app:edit',NULL,3,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (12,'DRILLAMIN','应用删除',8,'#',NULL,'#',3,'system:app:remove',NULL,4,0,1,NULL,'2022-09-26 15:50:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1001,'DRILLAMIN','新增',2,'#',NULL,'#',3,'system:user:add',NULL,2,0,1,NULL,'2022-09-26 15:48:09','2024-09-30 11:36:33',1);
INSERT INTO `sys_menu` VALUES (1002,'DRILLAMIN','修改',2,'#',NULL,'#',3,'system:user:edit',NULL,2,0,1,NULL,'2022-09-26 15:48:09','2024-08-05 15:04:37',1);
INSERT INTO `sys_menu` VALUES (1003,'DRILLAMIN','删除',2,'#',NULL,'#',3,'system:user:remove',NULL,3,0,1,NULL,'2022-09-26 15:48:09','2024-08-05 15:04:50',1);
INSERT INTO `sys_menu` VALUES (1004,'DRILLAMIN','导出',2,'#',NULL,'#',3,'system:user:export',NULL,4,0,1,NULL,'2022-09-26 15:48:09','2024-08-05 15:04:58',1);
INSERT INTO `sys_menu` VALUES (1006,'DRILLAMIN','重置密码',2,'#',NULL,'#',3,'system:user:resetPwd',NULL,5,0,1,NULL,'2022-09-26 15:48:09','2024-08-05 15:14:29',1);
INSERT INTO `sys_menu` VALUES (1008,'DRILLAMIN','新增',3,'#',NULL,'#',3,'system:role:add',NULL,2,0,1,NULL,'2022-09-26 15:48:09','2024-09-30 11:36:53',1);
INSERT INTO `sys_menu` VALUES (1009,'DRILLAMIN','修改',3,'#',NULL,'#',3,'system:role:edit',NULL,2,0,1,NULL,'2022-09-26 15:48:09','2024-08-05 15:14:20',1);
INSERT INTO `sys_menu` VALUES (1010,'DRILLAMIN','删除',3,'#',NULL,'#',3,'system:role:remove',NULL,3,0,1,NULL,'2022-09-26 15:48:09','2024-08-05 15:14:15',1);
INSERT INTO `sys_menu` VALUES (1013,'DRILLAMIN','新增',4,'#',NULL,'#',3,'system:menu:add',NULL,2,0,1,NULL,'2022-09-26 15:48:10','2024-09-30 11:37:14',1);
INSERT INTO `sys_menu` VALUES (1014,'DRILLAMIN','修改',4,'#',NULL,'#',3,'system:menu:edit',NULL,2,0,1,NULL,'2022-09-26 15:48:10','2024-08-05 15:10:54',1);
INSERT INTO `sys_menu` VALUES (1015,'DRILLAMIN','删除',4,'#',NULL,'#',3,'system:menu:remove',NULL,3,0,1,NULL,'2022-09-26 15:48:10','2024-08-05 15:11:00',1);
INSERT INTO `sys_menu` VALUES (1017,'DRILLAMIN','新增',5,'#',NULL,'#',3,'system:dept:add',NULL,2,0,1,NULL,'2022-09-26 15:50:31','2024-09-30 11:38:09',1);
INSERT INTO `sys_menu` VALUES (1018,'DRILLAMIN','修改',5,'#',NULL,'#',3,'system:dept:edit',NULL,2,0,1,NULL,'2022-09-26 15:50:31','2024-08-05 15:11:41',1);
INSERT INTO `sys_menu` VALUES (1019,'DRILLAMIN','删除',5,'#',NULL,'#',3,'system:dept:remove',NULL,3,0,1,NULL,'2022-09-26 15:50:31','2024-08-05 15:11:48',1);
INSERT INTO `sys_menu` VALUES (1021,'DRILLAMIN','新增',6,'#',NULL,'#',3,'system:post:add',NULL,2,0,1,NULL,'2022-09-26 15:50:31','2024-09-30 11:38:32',1);
INSERT INTO `sys_menu` VALUES (1022,'DRILLAMIN','修改',6,'#',NULL,'#',3,'system:post:edit',NULL,2,0,1,NULL,'2022-09-26 15:50:31','2024-08-05 15:12:21',1);
INSERT INTO `sys_menu` VALUES (1023,'DRILLAMIN','删除',6,'#',NULL,'#',3,'system:post:remove',NULL,3,0,1,NULL,'2022-09-26 15:50:31','2024-08-05 15:12:28',1);
INSERT INTO `sys_menu` VALUES (1024,'DRILLAMIN','生产管理',0,'produce','produce','#',1,NULL,NULL,2,0,1,1,'2023-02-20 23:01:11','2024-04-11 16:16:33',1);
INSERT INTO `sys_menu` VALUES (1025,'DRILLAMIN','排刀管理',0,'material','material','#',1,NULL,NULL,3,0,1,1,'2023-02-20 23:01:59','2024-04-11 16:16:54',1);
INSERT INTO `sys_menu` VALUES (1026,'DRILLAMIN','设备管理',0,'device1','device','#',1,NULL,NULL,4,0,1,1,'2023-02-20 23:02:35','2024-04-11 16:17:09',1);
INSERT INTO `sys_menu` VALUES (1027,'DRILLAMIN','告警管理',0,'alarm','alarm','#',1,NULL,NULL,5,0,1,1,'2023-02-20 23:03:32','2024-04-11 16:17:24',1);
INSERT INTO `sys_menu` VALUES (1029,'DRILLAMIN','生产排产',1024,'#','schedule','produce/schedule/index',2,'produce:schedule:list',NULL,60,0,1,1,'2023-02-20 23:06:58','2024-08-22 14:35:47',1);
INSERT INTO `sys_menu` VALUES (1030,'DRILLAMIN','板料追溯',1158,'#','board','wareHouse/board/index',2,'warehouse:board:list',NULL,10,0,1,1,'2023-02-20 23:09:15','2024-08-09 09:31:04',1);
INSERT INTO `sys_menu` VALUES (1031,'DRILLAMIN','钻孔任务',1024,'#','drillWorkOrder','produce/drillWorkOrder/index',2,'produce:drillWorkOrder:list',NULL,65,0,1,1,'2023-02-20 23:11:13','2023-12-23 20:27:42',1);
INSERT INTO `sys_menu` VALUES (1032,'DRILLAMIN','刀盘管理',1025,'#','cutter','material/cutter/index',2,'material:cutter:list',NULL,1,0,0,1,'2023-02-20 23:15:40','2024-08-05 14:49:41',1);
INSERT INTO `sys_menu` VALUES (1033,'DRILLAMIN','设备类型',1026,'#','deviceType','device/deviceType/index',2,'device:deviceType:list',NULL,2,0,1,1,'2023-02-20 23:18:29','2023-11-16 16:33:08',1);
INSERT INTO `sys_menu` VALUES (1035,'DRILLAMIN','设备列表',1026,'#','list','device/device/index',2,'device:device:list',NULL,11,0,1,1,'2023-02-20 23:22:03','2023-11-27 10:06:30',1);
INSERT INTO `sys_menu` VALUES (1036,'DRILLAMIN','刀具参数',1025,'#','diaFile','material/diaFile/index',2,'material:config:list',NULL,2,0,1,1,'2023-02-20 23:23:49','2023-10-26 15:53:54',1);
INSERT INTO `sys_menu` VALUES (1037,'DRILLAMIN','调度记录',1026,'#','schedule','device/schedulement/index',2,'device:schedulement:edit',NULL,8,0,1,1,'2023-02-20 23:25:16','2024-04-26 09:41:29',1);
INSERT INTO `sys_menu` VALUES (1038,'DRILLAMIN','事件管理',1027,'#','event','alarm/event/index',2,'alarm:event:list',NULL,1,0,1,1,'2023-02-20 23:27:03','2023-02-23 22:04:19',1);
INSERT INTO `sys_menu` VALUES (1039,'DRILLAMIN','告警记录',1027,'#','warn','alarm/warn/index',2,'alarm:warn:list',NULL,3,0,1,1,'2023-02-20 23:28:17','2023-03-23 22:13:35',1);
INSERT INTO `sys_menu` VALUES (1043,'DRILLAMIN','修改',1033,NULL,'','#',3,'device:deviceType:edit',NULL,2,0,1,1,'2023-02-21 20:32:06','2024-09-29 10:20:38',1);
INSERT INTO `sys_menu` VALUES (1044,'DRILLAMIN','删除',1033,NULL,NULL,'#',3,'device:deviceType:remove',NULL,2,0,1,1,'2023-02-21 20:32:58','2024-08-05 15:43:26',1);
INSERT INTO `sys_menu` VALUES (1054,'DRILLAMIN','通知管理',0,'notify','notify','#',1,NULL,NULL,6,0,1,1,'2023-02-22 01:16:01','2024-04-11 16:17:34',1);
INSERT INTO `sys_menu` VALUES (1055,'DRILLAMIN','通知设置',1054,'#','setting','notify/setting/index',2,'notify:setting:list',NULL,1,0,1,1,'2023-02-22 01:21:12','2023-03-23 02:15:26',1);
INSERT INTO `sys_menu` VALUES (1056,'DRILLAMIN','通知记录',1054,'#','record','notify/record/index',2,'notify:record:list',NULL,2,0,1,1,'2023-02-22 01:23:31','2023-07-10 14:35:19',1);
INSERT INTO `sys_menu` VALUES (1058,'DRILLAMIN','新增',1035,NULL,NULL,'#',3,'device:device:add',NULL,2,0,1,1,'2023-02-22 02:30:35','2024-09-29 10:23:23',1);
INSERT INTO `sys_menu` VALUES (1059,'DRILLAMIN','修改',1035,NULL,NULL,'#',3,'device:device:edit',NULL,2,0,1,1,'2023-02-22 02:32:17','2024-08-05 15:54:08',1);
INSERT INTO `sys_menu` VALUES (1060,'DRILLAMIN','删除',1035,NULL,NULL,'#',3,'device:device:remove',NULL,3,0,1,1,'2023-02-22 02:33:41','2024-08-05 15:54:11',1);
INSERT INTO `sys_menu` VALUES (1061,'DRILLAMIN','查看',1035,NULL,NULL,'#',3,'device:device:view',NULL,4,0,1,1,'2023-02-22 02:35:07','2024-08-05 15:54:14',1);
INSERT INTO `sys_menu` VALUES (1063,'DRILLAMIN','新增',1036,NULL,NULL,'#',3,'material:config:add',NULL,2,0,1,1,'2023-02-22 02:37:31','2023-05-23 01:19:56',43);
INSERT INTO `sys_menu` VALUES (1064,'DRILLAMIN','修改',1036,NULL,NULL,'#',3,'material:config:edit',NULL,3,0,1,1,'2023-02-22 02:38:13','2023-05-23 01:20:07',43);
INSERT INTO `sys_menu` VALUES (1065,'DRILLAMIN','删除',1036,NULL,NULL,'#',3,'material:config:remove',NULL,4,0,1,1,'2023-02-22 02:38:57','2023-05-23 01:20:19',43);
INSERT INTO `sys_menu` VALUES (1068,'DRILLAMIN','新增',1037,NULL,NULL,'#',3,'device:schedulement:add',NULL,2,0,1,1,'2023-02-22 02:41:06','2024-09-29 10:22:31',1);
INSERT INTO `sys_menu` VALUES (1069,'DRILLAMIN','修改',1037,NULL,NULL,'#',3,'device:schedulement:edit',NULL,2,0,1,1,'2023-02-22 02:41:32','2024-08-05 15:54:41',1);
INSERT INTO `sys_menu` VALUES (1070,'DRILLAMIN','删除',1037,NULL,NULL,'#',3,'device:schedulement:remove',NULL,3,0,1,1,'2023-02-22 02:42:02','2024-08-05 15:54:45',1);
INSERT INTO `sys_menu` VALUES (1073,'DRILLAMIN','新增',1030,NULL,NULL,'#',3,'produce:board:add',NULL,2,0,1,1,'2023-02-22 04:20:16','2023-05-23 02:12:26',1);
INSERT INTO `sys_menu` VALUES (1074,'DRILLAMIN','修改',1030,NULL,NULL,'#',3,'produce:board:edit',NULL,3,0,1,1,'2023-02-22 04:20:47','2023-05-23 02:12:33',1);
INSERT INTO `sys_menu` VALUES (1075,'DRILLAMIN','删除',1030,NULL,NULL,'#',3,'produce:board:remove',NULL,4,0,1,1,'2023-02-22 04:21:23','2023-05-23 02:12:42',1);
INSERT INTO `sys_menu` VALUES (1078,'DRILLAMIN','新增',1029,NULL,NULL,'#',3,'produce:schedule:add',NULL,2,0,1,1,'2023-02-22 04:23:59','2024-09-29 10:14:49',1);
INSERT INTO `sys_menu` VALUES (1079,'DRILLAMIN','修改',1029,NULL,NULL,'#',3,'produce:schedule:edit',NULL,2,0,1,1,'2023-02-22 04:24:34','2024-08-05 16:09:51',1);
INSERT INTO `sys_menu` VALUES (1080,'DRILLAMIN','删除',1029,NULL,NULL,'#',3,'produce:schedule:remove',NULL,3,0,1,1,'2023-02-22 04:25:14','2024-08-05 16:09:54',1);
INSERT INTO `sys_menu` VALUES (1083,'DRILLAMIN','新增',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:add',NULL,2,0,1,1,'2023-02-22 04:28:41','2023-08-04 10:39:10',1);
INSERT INTO `sys_menu` VALUES (1084,'DRILLAMIN','修改',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:edit',NULL,3,0,1,1,'2023-02-22 04:29:10','2023-08-04 10:39:17',1);
INSERT INTO `sys_menu` VALUES (1085,'DRILLAMIN','删除',1031,NULL,NULL,'#',3,'produce:drillWorkOrder:remove',NULL,4,0,1,1,'2023-02-22 04:29:43','2023-08-04 10:39:52',1);
INSERT INTO `sys_menu` VALUES (1088,'DRILLAMIN','新增',1032,NULL,NULL,'#',3,'material:cutter:add',NULL,2,0,1,1,'2023-02-22 04:32:50','2024-09-29 10:18:48',1);
INSERT INTO `sys_menu` VALUES (1089,'DRILLAMIN','修改',1032,NULL,NULL,'#',3,'material:cutter:edit',NULL,2,0,1,1,'2023-02-22 04:33:24','2024-08-05 15:55:37',1);
INSERT INTO `sys_menu` VALUES (1090,'DRILLAMIN','删除',1032,NULL,NULL,'#',3,'material:cutter:remove',NULL,3,0,1,1,'2023-02-22 04:33:57','2024-08-05 15:55:43',1);
INSERT INTO `sys_menu` VALUES (1098,'DRILLAMIN','新增',1038,NULL,NULL,'#',3,'alarm:event:add',NULL,2,0,1,1,'2023-02-24 00:08:03','2024-09-30 11:30:36',1);
INSERT INTO `sys_menu` VALUES (1099,'DRILLAMIN','修改',1038,NULL,NULL,'#',3,'alarm:event:edit',NULL,2,0,1,1,'2023-02-24 00:08:38','2024-08-05 15:34:11',1);
INSERT INTO `sys_menu` VALUES (1100,'DRILLAMIN','删除',1038,NULL,NULL,'#',3,'alarm:event:remove',NULL,3,0,1,1,'2023-02-24 00:09:16','2024-08-05 15:34:41',1);
INSERT INTO `sys_menu` VALUES (1103,'DRILLAMIN','新增',1039,NULL,NULL,'#',3,'alarm:warn:add',NULL,2,0,1,1,'2023-02-24 00:11:32','2024-09-30 11:31:25',1);
INSERT INTO `sys_menu` VALUES (1104,'DRILLAMIN','修改',1039,NULL,NULL,'#',3,'alarm:warn:edit',NULL,2,0,1,1,'2023-02-24 00:12:00','2024-08-05 15:35:12',1);
INSERT INTO `sys_menu` VALUES (1105,'DRILLAMIN','删除',1039,NULL,NULL,'#',3,'alarm:warn:remove',NULL,3,0,1,1,'2023-02-24 00:12:26','2024-08-05 15:35:15',1);
INSERT INTO `sys_menu` VALUES (1109,'DRILLAMIN','新增',1055,NULL,NULL,'#',3,'notify:record:add',NULL,2,0,1,1,'2023-02-24 00:15:14','2024-09-30 11:32:18',1);
INSERT INTO `sys_menu` VALUES (1110,'DRILLAMIN','修改',1055,NULL,NULL,'#',3,'notify:record:edit',NULL,2,0,1,1,'2023-02-24 00:16:57','2024-08-05 15:18:41',1);
INSERT INTO `sys_menu` VALUES (1111,'DRILLAMIN','删除',1055,NULL,NULL,'#',3,'notify:record:remove',NULL,3,0,1,1,'2023-02-24 00:17:25','2024-08-05 15:18:45',1);
INSERT INTO `sys_menu` VALUES (1114,'DRILLAMIN','新增',1056,NULL,NULL,'#',3,'notify:setting:add',NULL,2,0,1,1,'2023-02-24 00:18:55','2024-09-30 11:33:13',1);
INSERT INTO `sys_menu` VALUES (1115,'DRILLAMIN','修改',1056,NULL,NULL,'#',3,'notify:setting:edit',NULL,2,0,1,1,'2023-02-24 00:19:26','2024-08-05 15:19:06',1);
INSERT INTO `sys_menu` VALUES (1116,'DRILLAMIN','删除',1056,NULL,NULL,'#',3,'notify:setting:remove',NULL,3,0,1,1,'2023-02-24 00:19:50','2024-08-05 15:19:03',1);
INSERT INTO `sys_menu` VALUES (1122,'DRILLAMIN','生产工单',1024,'#','workOrder','produce/workOrder/index',2,'produce:workorder:list',NULL,15,0,1,1,'2023-03-01 11:03:28','2023-12-23 20:29:25',1);
INSERT INTO `sys_menu` VALUES (1124,'DRILLAMIN','新增',1122,NULL,NULL,'#',3,'produce:workorder:add',NULL,2,0,1,1,'2023-03-01 11:06:03','2024-08-06 16:38:56',1);
INSERT INTO `sys_menu` VALUES (1125,'DRILLAMIN','修改',1122,NULL,NULL,'#',3,'produce:workorder:edit',NULL,3,0,1,1,'2023-03-01 11:06:41','2024-08-06 16:39:00',1);
INSERT INTO `sys_menu` VALUES (1126,'DRILLAMIN','删除',1122,NULL,NULL,'#',3,'produce:workorder:remove',NULL,4,0,1,1,'2023-03-01 11:07:08','2024-08-06 16:39:03',1);
INSERT INTO `sys_menu` VALUES (1128,'DRILLAMIN','工序管理',1024,'#','process','produce/process/index',2,'produce:process:list',NULL,1,0,1,1,'2023-03-01 11:37:07','2023-10-27 10:35:14',1);
INSERT INTO `sys_menu` VALUES (1130,'DRILLAMIN','新增',1128,'#',NULL,'#',3,'produce:process:add',NULL,2,0,1,1,'2023-03-01 11:43:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1131,'DRILLAMIN','修改',1128,'#',NULL,'#',3,'produce:process:edit',NULL,3,0,1,1,'2023-03-01 11:43:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1132,'DRILLAMIN','删除',1128,'#',NULL,'#',3,'produce:process:remove',NULL,4,0,1,1,'2023-03-01 11:43:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1134,'DRILLAMIN','字典管理',1,'dict','dict','system/dict/index',2,'system:dict:list',NULL,3,0,0,1,'2023-03-01 13:53:42','2023-11-24 15:35:19',1);
INSERT INTO `sys_menu` VALUES (1136,'DRILLAMIN','告警设置',1027,'#','settings','alarm/setting/index',2,'alarm:setting:index',NULL,2,0,1,1,'2023-03-23 22:13:23','2023-07-10 14:36:58',1);
INSERT INTO `sys_menu` VALUES (1138,'DRILLAMIN','新增',1136,'#',NULL,'#',3,'alarm:setting:add',NULL,2,0,1,1,'2023-03-23 22:16:21','2024-09-30 11:31:04',1);
INSERT INTO `sys_menu` VALUES (1139,'DRILLAMIN','修改',1136,'#',NULL,'#',3,'alarm:setting:edit',NULL,2,0,1,1,'2023-03-23 22:16:59','2024-08-05 15:34:52',1);
INSERT INTO `sys_menu` VALUES (1140,'DRILLAMIN','删除',1136,'#',NULL,'#',3,'alarm:setting:remove',NULL,3,0,1,1,'2023-03-23 22:17:26','2024-08-05 15:34:55',1);
INSERT INTO `sys_menu` VALUES (1148,'DRILLAMIN','主数据',0,'masterData','masterData','#',1,NULL,NULL,1,0,1,1,'2023-03-29 22:00:59','2024-04-11 16:16:16',1);
INSERT INTO `sys_menu` VALUES (1149,'DRILLAMIN','计量单位',1148,'','unitMeasure','masterData/unitMeasure/index',2,'masterData:unitMeasure:list',NULL,1,0,1,1,'2023-03-29 22:05:55','2023-05-08 05:21:07',1);
INSERT INTO `sys_menu` VALUES (1150,'DRILLAMIN','客户管理',1148,'','client','masterData/client/index',2,'masterData:client:list',NULL,50,0,1,1,'2023-03-29 22:07:26','2023-05-08 05:21:20',1);
INSERT INTO `sys_menu` VALUES (1151,'DRILLAMIN','供应商管理',1148,'','vendor','masterData/vendor/index',2,'masterData:vendor:list',NULL,59,0,1,1,'2023-03-29 22:09:03','2024-08-01 15:43:00',1);
INSERT INTO `sys_menu` VALUES (1152,'DRILLAMIN','车间管理',1148,'','workshop','masterData/workShop/index',2,'masterData:workshop:list',NULL,40,0,1,1,'2023-03-29 22:10:20','2023-05-08 05:21:16',1);
INSERT INTO `sys_menu` VALUES (1153,'DRILLAMIN','工作站管理',1148,'','workstation','masterData/workStation/index',2,'masterData:workstation:list',NULL,41,0,1,1,'2023-03-29 22:11:24','2024-08-05 14:20:59',1);
INSERT INTO `sys_menu` VALUES (1154,'DRILLAMIN','物料产品分类',1148,'','itemType','masterData/itemType/index',2,'masterData:itemType:list',NULL,10,0,1,1,'2023-03-29 22:12:21','2023-05-08 05:21:12',1);
INSERT INTO `sys_menu` VALUES (1155,'DRILLAMIN','物料产品管理',1148,'','item','masterData/item/index',2,'masterData:item:list',NULL,11,0,1,1,'2023-03-29 22:13:20','2023-05-08 05:21:14',1);
INSERT INTO `sys_menu` VALUES (1156,'DRILLAMIN','产品大类',1148,'','productCategory','masterData/productCategory/index',2,'masterData:productCategory:list',NULL,5,0,1,1,'2023-04-01 02:41:29','2023-05-08 21:42:17',1);
INSERT INTO `sys_menu` VALUES (1157,'DRILLAMIN','工艺路线',1024,'#','route','produce/route/index',2,'produce:route:list',NULL,2,0,1,1,'2023-04-01 02:46:56','2023-09-07 10:12:33',1);
INSERT INTO `sys_menu` VALUES (1158,'DRILLAMIN','仓储管理',0,'warehouse1','wm','#',1,NULL,NULL,7,0,1,1,'2023-04-01 02:50:34','2024-08-21 14:26:53',1);
INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','分区设置',1158,'#','partition','wareHouse/partition/index',2,'warehouse:partition:list',NULL,7,0,1,1,'2023-04-01 02:53:42','2024-08-27 10:35:59',1);
INSERT INTO `sys_menu` VALUES (1160,'DRILLAMIN','库存现有量',1158,'#','materialStock','wareHouse/materialStock/index',2,'warehouse:materialStock:list',NULL,8,0,1,1,'2023-04-01 02:54:57','2024-08-09 09:30:36',1);
INSERT INTO `sys_menu` VALUES (1161,'DRILLAMIN','钻带参数',1025,'#','drillFile','material/drillFile/index',2,'material:drillFile:list',NULL,3,0,1,1,'2023-04-01 03:20:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1162,'DRILLAMIN','排刀文件',1025,'#','atpFile','material/atpFile/index',2,'material:atpFile:list',NULL,4,0,1,1,'2023-04-01 03:21:21','2023-04-10 23:34:21',1);
INSERT INTO `sys_menu` VALUES (1165,'DRILLAMIN','报工记录',1024,'#','reportrecords','produce/reportRecords/index',2,'produce:reportRecords:list',NULL,45,0,1,1,'2023-04-04 00:47:50','2023-12-23 20:28:25',1);
INSERT INTO `sys_menu` VALUES (1167,'DRILLAMIN','新增',1165,'#',NULL,'#',3,'produce:reportRecords:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-09-29 10:14:33',1);
INSERT INTO `sys_menu` VALUES (1168,'DRILLAMIN','修改',1165,'#',NULL,'#',3,'produce:reportRecords:edit',NULL,2,0,1,1,'2023-04-04 00:51:30','2024-08-05 16:09:13',1);
INSERT INTO `sys_menu` VALUES (1169,'DRILLAMIN','删除',1165,'#',NULL,'#',3,'produce:reportRecords:remove',NULL,3,0,1,1,'2023-04-04 00:51:58','2024-08-05 16:09:16',1);
INSERT INTO `sys_menu` VALUES (1172,'DRILLAMIN','新增',1161,'#',NULL,'#',3,'material:drillFile:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1173,'DRILLAMIN','修改',1161,'#',NULL,'#',3,'material:drillFile:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1174,'DRILLAMIN','删除',1161,'#',NULL,'#',3,'material:drillFile:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1176,'DRILLAMIN','查看',1161,'#',NULL,'#',3,'material:drillFile:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-05 15:56:04',1);
INSERT INTO `sys_menu` VALUES (1178,'DRILLAMIN','新增',1162,'#',NULL,'#',3,'material:atpFile:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-09-29 10:20:08',1);
INSERT INTO `sys_menu` VALUES (1179,'DRILLAMIN','修改',1162,'#',NULL,'#',3,'material:atpFile:edit',NULL,2,0,1,1,'2023-04-04 00:51:30','2024-08-05 15:56:14',1);
INSERT INTO `sys_menu` VALUES (1180,'DRILLAMIN','删除',1162,'#',NULL,'#',3,'material:atpFile:remove',NULL,3,0,1,1,'2023-04-04 00:51:58','2024-08-05 15:56:17',1);
INSERT INTO `sys_menu` VALUES (1182,'DRILLAMIN','查看',1162,'#',NULL,'#',3,'material:atpFile:view',NULL,4,0,1,1,'2023-04-04 00:52:22','2024-08-05 15:56:22',1);
INSERT INTO `sys_menu` VALUES (1208,'DRILLAMIN','新增',1159,'#',NULL,'#',3,'warehouse:partition:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-08-27 10:36:17',1);
INSERT INTO `sys_menu` VALUES (1209,'DRILLAMIN','修改',1159,'#',NULL,'#',3,'warehouse:partition:edit',NULL,3,0,1,1,'2023-04-04 00:51:30','2024-08-27 10:36:23',1);
INSERT INTO `sys_menu` VALUES (1210,'DRILLAMIN','删除',1159,'#',NULL,'#',3,'warehouse:partition:remove',NULL,4,0,1,1,'2023-04-04 00:51:58','2024-08-27 10:36:28',1);
INSERT INTO `sys_menu` VALUES (1212,'DRILLAMIN','查看',1159,'#',NULL,'#',3,'warehouse:partition:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-27 10:36:33',1);
INSERT INTO `sys_menu` VALUES (1214,'DRILLAMIN','新增',1160,'#',NULL,'#',3,'warehouse:materialStock:add',NULL,1,0,1,1,'2023-04-04 00:51:00','2024-08-05 15:14:58',1);
INSERT INTO `sys_menu` VALUES (1215,'DRILLAMIN','修改',1160,'#',NULL,'#',3,'warehouse:materialStock:edit',NULL,2,0,1,1,'2023-04-04 00:51:30','2024-08-05 15:15:03',1);
INSERT INTO `sys_menu` VALUES (1216,'DRILLAMIN','删除',1160,'#',NULL,'#',3,'warehouse:materialStock:remove',NULL,3,0,1,1,'2023-04-04 00:51:58','2024-08-05 15:15:07',1);
INSERT INTO `sys_menu` VALUES (1218,'DRILLAMIN','查看',1160,'#',NULL,'#',3,'warehouse:materialStock:view',NULL,4,0,1,1,'2023-04-04 00:52:22','2024-08-05 15:15:13',1);
INSERT INTO `sys_menu` VALUES (1220,'DRILLAMIN','新增',1157,'#',NULL,'#',3,'produce:route:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-09-29 10:12:17',1);
INSERT INTO `sys_menu` VALUES (1221,'DRILLAMIN','修改',1157,'#',NULL,'#',3,'produce:route:edit',NULL,2,0,1,1,'2023-04-04 00:51:30','2024-08-05 15:59:18',1);
INSERT INTO `sys_menu` VALUES (1222,'DRILLAMIN','删除',1157,'#',NULL,'#',3,'produce:route:remove',NULL,3,0,1,1,'2023-04-04 00:51:58','2024-08-05 15:59:22',1);
INSERT INTO `sys_menu` VALUES (1224,'DRILLAMIN','查看',1157,'#',NULL,'#',3,'produce:route:view',NULL,4,0,1,1,'2023-04-04 00:52:22','2024-08-05 15:59:32',1);
INSERT INTO `sys_menu` VALUES (1232,'DRILLAMIN','新增',1149,'#',NULL,'#',3,'masterData:unitMeasure:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-08-06 10:18:15',1);
INSERT INTO `sys_menu` VALUES (1233,'DRILLAMIN','修改',1149,'#',NULL,'#',3,'masterData:unitMeasure:edit',NULL,3,0,1,1,'2023-04-04 00:51:30','2024-08-06 10:18:18',1);
INSERT INTO `sys_menu` VALUES (1234,'DRILLAMIN','删除',1149,'#',NULL,'#',3,'masterData:unitMeasure:remove',NULL,4,0,1,1,'2023-04-04 00:51:58','2024-08-06 10:18:22',1);
INSERT INTO `sys_menu` VALUES (1236,'DRILLAMIN','查看',1149,'#',NULL,'#',3,'masterData:unitMeasure:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-06 10:18:26',1);
INSERT INTO `sys_menu` VALUES (1238,'DRILLAMIN','新增',1150,'#',NULL,'#',3,'masterData:client:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1239,'DRILLAMIN','修改',1150,'#',NULL,'#',3,'masterData:client:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1240,'DRILLAMIN','删除',1150,'#',NULL,'#',3,'masterData:client:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1242,'DRILLAMIN','查看',1150,'#',NULL,'#',3,'masterData:client:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-05 16:22:11',1);
INSERT INTO `sys_menu` VALUES (1244,'DRILLAMIN','新增',1151,'#',NULL,'#',3,'masterData:vendor:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1245,'DRILLAMIN','修改',1151,'#',NULL,'#',3,'masterData:vendor:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1246,'DRILLAMIN','删除',1151,'#',NULL,'#',3,'masterData:vendor:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1248,'DRILLAMIN','查看',1151,'#',NULL,'#',3,'masterData:vendor:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-05 16:22:17',1);
INSERT INTO `sys_menu` VALUES (1250,'DRILLAMIN','新增',1152,'#',NULL,'#',3,'masterData:workshop:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-09-29 10:09:54',1);
INSERT INTO `sys_menu` VALUES (1251,'DRILLAMIN','修改',1152,'#',NULL,'#',3,'masterData:workshop:edit',NULL,2,0,1,1,'2023-04-04 00:51:30','2024-08-05 16:21:46',1);
INSERT INTO `sys_menu` VALUES (1252,'DRILLAMIN','删除',1152,'#',NULL,'#',3,'masterData:workshop:remove',NULL,3,0,1,1,'2023-04-04 00:51:58','2024-08-05 16:21:50',1);
INSERT INTO `sys_menu` VALUES (1254,'DRILLAMIN','查看',1152,'#',NULL,'#',3,'masterData:workshop:view',NULL,4,0,1,1,'2023-04-04 00:52:22','2024-08-05 16:21:59',1);
INSERT INTO `sys_menu` VALUES (1256,'DRILLAMIN','新增',1153,'#',NULL,'#',3,'masterData:workstation:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1257,'DRILLAMIN','修改',1153,'#',NULL,'#',3,'masterData:workstation:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1258,'DRILLAMIN','删除',1153,'#',NULL,'#',3,'masterData:workstation:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1260,'DRILLAMIN','查看',1153,'#',NULL,'#',3,'masterData:workstation:view',NULL,6,0,1,1,'2023-04-04 00:52:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1262,'DRILLAMIN','新增',1154,'#',NULL,'#',3,'masterData:itemType:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1263,'DRILLAMIN','修改',1154,'#',NULL,'#',3,'masterData:itemType:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1264,'DRILLAMIN','删除',1154,'#',NULL,'#',3,'masterData:itemType:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1266,'DRILLAMIN','查看',1154,'#',NULL,'#',3,'masterData:itemType:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-05 16:21:11',1);
INSERT INTO `sys_menu` VALUES (1268,'DRILLAMIN','新增',1155,'#',NULL,'#',3,'masterData:item:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1269,'DRILLAMIN','修改',1155,'#',NULL,'#',3,'masterData:item:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1270,'DRILLAMIN','删除',1155,'#',NULL,'#',3,'masterData:item:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1272,'DRILLAMIN','查看',1155,'#',NULL,'#',3,'masterData:item:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-05 16:21:26',1);
INSERT INTO `sys_menu` VALUES (1274,'DRILLAMIN','新增',1156,'#',NULL,'#',3,'masterData:productCategory:add',NULL,2,0,1,1,'2023-04-04 00:51:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1275,'DRILLAMIN','修改',1156,'#',NULL,'#',3,'masterData:productCategory:edit',NULL,3,0,1,1,'2023-04-04 00:51:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1276,'DRILLAMIN','删除',1156,'#',NULL,'#',3,'masterData:productCategory:remove',NULL,4,0,1,1,'2023-04-04 00:51:58',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1278,'DRILLAMIN','查看',1156,'#',NULL,'#',3,'masterData:productCategory:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-05 14:03:04',1);
INSERT INTO `sys_menu` VALUES (1279,'DRILLAMIN','审批',1122,'#',NULL,'#',3,'produce:workorder:approval',NULL,7,0,1,1,'2023-04-05 22:19:41','2024-09-24 10:53:02',1);
INSERT INTO `sys_menu` VALUES (1293,'DRILLAMIN','新增工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:add',NULL,7,0,1,33,'2023-04-10 03:19:48','2024-08-05 16:00:55',1);
INSERT INTO `sys_menu` VALUES (1294,'DRILLAMIN','修改工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:edit',NULL,8,0,1,33,'2023-04-10 03:20:38','2024-08-05 16:00:58',1);
INSERT INTO `sys_menu` VALUES (1295,'DRILLAMIN','删除工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:remove',NULL,9,0,1,33,'2023-04-10 03:21:26','2024-08-05 16:01:01',1);
INSERT INTO `sys_menu` VALUES (1296,'DRILLAMIN','查看工艺路线与工序关系',1157,'#',NULL,'#',3,'produce:routeAndProcess:view',NULL,10,0,1,33,'2023-04-10 03:22:11','2024-08-05 16:01:04',1);
INSERT INTO `sys_menu` VALUES (1298,'DRILLAMIN','新增工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:add',NULL,11,0,1,33,'2023-04-10 03:23:54','2024-08-05 16:01:08',1);
INSERT INTO `sys_menu` VALUES (1299,'DRILLAMIN','修改工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:edit',NULL,12,0,1,33,'2023-04-10 03:24:37','2024-08-05 16:01:12',1);
INSERT INTO `sys_menu` VALUES (1300,'DRILLAMIN','删除工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:remove',NULL,13,0,1,33,'2023-04-10 03:25:19','2024-08-05 16:01:16',1);
INSERT INTO `sys_menu` VALUES (1301,'DRILLAMIN','查看工艺路线与产品大类关系',1157,'#',NULL,'#',3,'produce:routeAndProductCategory:view',NULL,14,0,1,33,'2023-04-10 03:27:49','2024-08-05 16:01:21',1);
INSERT INTO `sys_menu` VALUES (1303,'DRILLAMIN','新增工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:add',NULL,15,0,1,33,'2023-04-10 03:30:59','2024-08-05 16:01:42',1);
INSERT INTO `sys_menu` VALUES (1304,'DRILLAMIN','修改工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:edit',NULL,16,0,1,33,'2023-04-10 03:31:42','2024-08-05 16:01:47',1);
INSERT INTO `sys_menu` VALUES (1305,'DRILLAMIN','删除工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:remove',NULL,17,0,1,33,'2023-04-10 03:32:34','2024-08-05 16:01:51',1);
INSERT INTO `sys_menu` VALUES (1306,'DRILLAMIN','查看工序与工作站关系',1157,'#',NULL,'#',3,'produce:routeProcessAndWorkStation:view',NULL,18,0,1,33,'2023-04-10 03:33:14','2024-08-05 16:01:54',1);
INSERT INTO `sys_menu` VALUES (1307,'DRILLAMIN','查看',1128,'#',NULL,'#',3,'produce:process:view',NULL,5,0,1,33,'2023-04-10 03:35:22','2024-08-05 15:58:11',1);
INSERT INTO `sys_menu` VALUES (1309,'DRILLAMIN','查看',1122,'#',NULL,'#',3,'produce:workorder:view',NULL,5,0,1,33,'2023-04-10 04:22:28','2024-08-06 16:39:07',1);
INSERT INTO `sys_menu` VALUES (1310,'DRILLAMIN','查看',1029,'#',NULL,'#',3,'produce:schedule:view',NULL,4,0,1,33,'2023-04-10 04:23:37','2024-08-06 17:10:02',1);
INSERT INTO `sys_menu` VALUES (1311,'DRILLAMIN','查看',1031,'#',NULL,'#',3,'produce:drillWorkOrder:view',NULL,5,0,1,33,'2023-04-10 04:25:02','2024-08-05 16:15:42',1);
INSERT INTO `sys_menu` VALUES (1312,'DRILLAMIN','查看',1165,'#',NULL,'#',3,'produce:reportRecords:view',NULL,4,0,1,33,'2023-04-10 04:25:58','2024-08-05 16:09:20',1);
INSERT INTO `sys_menu` VALUES (1313,'DRILLAMIN','查看',1030,'#',NULL,'#',3,'produce:board:view',NULL,6,0,1,33,'2023-04-10 04:28:13','2023-05-23 02:12:52',1);
INSERT INTO `sys_menu` VALUES (1314,'DRILLAMIN','检验记录',1024,'#','checkrecords','produce/checkRecords/index',2,'produce:checkrecords:list',NULL,80,0,1,1,'2023-04-14 02:34:00','2023-12-23 20:28:00',1);
INSERT INTO `sys_menu` VALUES (1317,'DRILLAMIN','设备维护',1026,'#','repair','device/repair/index',2,'device:repair:list',NULL,6,0,1,1,'2023-05-06 03:11:39','2023-08-24 14:54:37',1);
INSERT INTO `sys_menu` VALUES (1319,'DRILLAMIN','点检项目',1026,'#','subject','device/subject/index',2,'device:subject:list',NULL,5,0,1,1,'2023-05-16 02:46:43','2023-08-24 14:54:32',1);
INSERT INTO `sys_menu` VALUES (1320,'DRILLAMIN','编码规则',1148,'','autocodeRule','masterData/autocodeRule/index',2,'masterData:autocodeRule:list',NULL,2,0,1,1,'2023-05-17 02:29:46','2024-08-05 16:20:12',1);
INSERT INTO `sys_menu` VALUES (1323,'DRILLAMIN','修改',1320,'#',NULL,'#',3,'masterData:autocodeRule:edit',NULL,2,0,1,1,'2023-05-22 04:08:17','2023-09-22 15:38:19',1);
INSERT INTO `sys_menu` VALUES (1324,'DRILLAMIN','导入',1149,'#',NULL,'#',3,'masterData:unitMeasure:import',NULL,2,0,1,43,'2023-05-22 22:58:04','2024-09-29 10:07:10',1);
INSERT INTO `sys_menu` VALUES (1325,'DRILLAMIN','导入',1154,'#',NULL,'#',3,'masterData:itemType:import',NULL,2,0,1,43,'2023-05-22 23:02:10','2024-09-29 10:09:08',1);
INSERT INTO `sys_menu` VALUES (1326,'DRILLAMIN','导入',1153,'#',NULL,'#',3,'masterData:workstation:import',NULL,2,0,1,43,'2023-05-22 23:06:45','2024-09-29 10:10:15',1);
INSERT INTO `sys_menu` VALUES (1327,'DRILLAMIN','导入',1150,'#',NULL,'#',3,'masterData:client:import',NULL,2,0,1,43,'2023-05-22 23:08:11','2024-09-29 10:10:33',1);
INSERT INTO `sys_menu` VALUES (1328,'DRILLAMIN','导入',1151,'#',NULL,'#',3,'masterData:vendor:import',NULL,2,0,1,43,'2023-05-22 23:09:17','2024-09-29 10:10:52',1);
INSERT INTO `sys_menu` VALUES (1329,'DRILLAMIN','导入',1128,'#',NULL,'#',3,'produce:process:import',NULL,2,0,1,43,'2023-05-22 23:13:27','2024-09-29 10:11:46',1);
INSERT INTO `sys_menu` VALUES (1330,'DRILLAMIN','导入',1122,'#',NULL,'#',3,'produce:workorder:import',NULL,2,0,1,43,'2023-05-22 23:17:45','2024-09-29 10:12:39',1);
INSERT INTO `sys_menu` VALUES (1331,'DRILLAMIN','导出',1165,'#',NULL,'#',3,'produce:reportRecords:export',NULL,5,0,1,43,'2023-05-22 23:20:22','2024-08-05 16:09:05',1);
INSERT INTO `sys_menu` VALUES (1332,'DRILLAMIN','导出',1030,'#',NULL,'#',3,'produce:board:export',NULL,2,0,1,43,'2023-05-22 23:21:30','2024-09-30 11:35:07',1);
INSERT INTO `sys_menu` VALUES (1333,'DRILLAMIN','查看',1314,'#',NULL,'#',3,'produce:checkrecords:view',NULL,3,0,1,43,'2023-05-22 23:22:44','2024-08-05 16:18:13',1);
INSERT INTO `sys_menu` VALUES (1334,'DRILLAMIN','编辑',1314,'#',NULL,'#',3,'produce:checkrecords:edit',NULL,2,0,1,43,'2023-05-22 23:23:29','2024-08-07 09:55:00',1);
INSERT INTO `sys_menu` VALUES (1335,'DRILLAMIN','导出',1038,'#',NULL,'#',3,'alarm:event:export',NULL,5,0,1,43,'2023-05-22 23:35:43','2024-08-07 10:57:28',1);
INSERT INTO `sys_menu` VALUES (1336,'DRILLAMIN','导出',1039,'#',NULL,'#',3,'alarm:warn:export',NULL,5,0,1,43,'2023-05-22 23:37:48','2024-08-05 15:35:27',1);
INSERT INTO `sys_menu` VALUES (1337,'DRILLAMIN','导入',1156,'#',NULL,'#',3,'masterData:productCategory:import',NULL,2,0,1,43,'2023-05-23 01:10:54','2024-09-29 10:08:07',1);
INSERT INTO `sys_menu` VALUES (1338,'DRILLAMIN','导入',1155,'#',NULL,'#',3,'masterData:item:import',NULL,2,0,1,43,'2023-05-23 01:11:55','2024-09-29 10:09:35',1);
INSERT INTO `sys_menu` VALUES (1339,'DRILLAMIN','导出',1031,'#',NULL,'#',3,'produce:drillWorkOrder:export',NULL,2,0,1,43,'2023-05-23 01:13:41','2024-09-29 10:15:05',1);
INSERT INTO `sys_menu` VALUES (1340,'DRILLAMIN','导出',1314,'#',NULL,'#',3,'produce:checkrecords:export',NULL,2,0,1,43,'2023-05-23 01:15:58','2024-09-29 10:15:31',1);
INSERT INTO `sys_menu` VALUES (1341,'DRILLAMIN','导入',1036,'#',NULL,'#',3,'material:config:import',NULL,2,0,1,43,'2023-05-23 01:16:54','2024-09-29 10:19:16',1);
INSERT INTO `sys_menu` VALUES (1342,'DRILLAMIN','导入',1161,'#',NULL,'#',3,'material:drillFile:import',NULL,2,0,1,43,'2023-05-23 01:17:21','2024-09-29 10:19:35',1);
INSERT INTO `sys_menu` VALUES (1343,'DRILLAMIN','查看',1036,'#',NULL,'#',3,'material:config:view',NULL,5,0,1,43,'2023-05-23 01:21:48','2024-08-07 10:09:55',1);
INSERT INTO `sys_menu` VALUES (1345,'DRILLAMIN','新增',1317,'#',NULL,'#',3,'device:repair:add',NULL,2,0,1,43,'2023-05-23 01:23:32','2024-09-29 10:21:37',1);
INSERT INTO `sys_menu` VALUES (1346,'DRILLAMIN','删除',1317,'#',NULL,'#',3,'device:repair:remove',NULL,3,0,1,43,'2023-05-23 01:23:48','2024-08-05 15:44:10',1);
INSERT INTO `sys_menu` VALUES (1347,'DRILLAMIN','修改',1317,'#',NULL,'#',3,'device:repair:edit',NULL,2,0,1,43,'2023-05-23 01:24:04','2024-08-05 15:43:44',1);
INSERT INTO `sys_menu` VALUES (1348,'DRILLAMIN','导出',1317,'#',NULL,'#',3,'device:repair:export',NULL,5,0,1,43,'2023-05-23 01:24:21','2024-08-07 10:36:53',1);
INSERT INTO `sys_menu` VALUES (1349,'DRILLAMIN','导出',1056,'#',NULL,'#',3,'notify:record:export',NULL,5,0,1,43,'2023-05-23 01:25:40','2024-08-05 15:18:55',1);
INSERT INTO `sys_menu` VALUES (1350,'DRILLAMIN','生产报工',1024,'#','reportWork','produce/reportWork/index',2,'produce:reportWork:list',NULL,40,0,1,1,'2023-06-12 11:24:59','2023-12-23 20:28:35',1);
INSERT INTO `sys_menu` VALUES (1351,'DRILLAMIN','查看',1350,'#',NULL,'#',3,'produce:reportWork:view',NULL,4,0,1,1,'2023-06-12 05:23:38','2024-08-05 16:08:05',1);
INSERT INTO `sys_menu` VALUES (1352,'DRILLAMIN','取消',1037,'#',NULL,'#',3,'device:schedulement:cancel',NULL,5,0,1,1,'2023-06-30 13:54:41','2024-08-05 15:54:54',1);
INSERT INTO `sys_menu` VALUES (1354,'DRILLAMIN','权限修改检验记录',1314,'#',NULL,'#',3,'produce:checkrecords:hasPermissions',NULL,4,0,1,1,'2023-08-02 10:02:26','2024-08-05 16:18:24',1);
INSERT INTO `sys_menu` VALUES (1355,'DRILLAMIN','点检',1035,'#',NULL,'#',3,'device:device:check',NULL,5,0,1,1,'2023-08-04 11:23:41','2024-08-05 15:54:19',1);
INSERT INTO `sys_menu` VALUES (1358,'DRILLAMIN','新增',1319,'#',NULL,'#',3,'device:subject:add',NULL,2,0,1,1,'2023-08-09 14:07:40','2024-09-29 10:21:07',1);
INSERT INTO `sys_menu` VALUES (1359,'DRILLAMIN','修改',1319,'#',NULL,'#',3,'device:subject:edit',NULL,2,0,1,1,'2023-08-09 14:08:03','2024-08-05 15:42:37',1);
INSERT INTO `sys_menu` VALUES (1360,'DRILLAMIN','删除',1319,'#',NULL,'#',3,'device:subject:remove',NULL,4,0,1,1,'2023-08-09 14:08:28','2024-08-05 15:42:49',1);
INSERT INTO `sys_menu` VALUES (1361,'DRILLAMIN','导出',1319,'#',NULL,'#',3,'device:subject:export',NULL,5,0,1,1,'2023-08-09 14:09:01','2024-08-05 15:42:55',1);
INSERT INTO `sys_menu` VALUES (1362,'DRILLAMIN','详情',1037,'#',NULL,'#',3,'device:schedulement:detail',NULL,7,0,1,1,'2023-08-09 14:10:35','2024-08-05 15:55:03',1);
INSERT INTO `sys_menu` VALUES (1363,'DRILLAMIN','发起',1037,'#',NULL,'#',3,'device:schedulement:start',NULL,6,0,1,1,'2023-08-09 14:11:04','2024-08-05 15:54:58',1);
INSERT INTO `sys_menu` VALUES (1364,'DRILLAMIN','新增',1320,'#',NULL,'#',3,'masterData:autocodeRule:add',NULL,2,0,1,1,'2023-08-09 14:12:20','2024-09-29 10:07:41',1);
INSERT INTO `sys_menu` VALUES (1365,'DRILLAMIN','删除',1320,'#',NULL,'#',3,'masterData:autocodeRule:remove',NULL,3,0,1,1,'2023-08-09 14:12:37','2024-08-05 14:01:21',1);
INSERT INTO `sys_menu` VALUES (1366,'DRILLAMIN','复制',1155,'#',NULL,'#',3,'masterData:item:copy',NULL,6,0,1,1,'2023-08-09 14:14:28','2023-08-09 14:14:44',1);
INSERT INTO `sys_menu` VALUES (1367,'DRILLAMIN','重新计算库存',1031,'#',NULL,'#',3,'produce:drillWorkOrder:recalculate',NULL,9,0,1,50,'2023-08-09 15:30:02','2024-08-07 09:52:22',1);
INSERT INTO `sys_menu` VALUES (1368,'DRILLAMIN','跳转任务拖拽',1031,'#',NULL,'#',3,'produce:drillWorkOrder:editDrillTask',NULL,8,0,1,50,'2023-08-09 15:31:01','2024-08-05 16:15:52',1);
INSERT INTO `sys_menu` VALUES (1369,'DRILLAMIN','选中',1031,'#',NULL,'#',3,'produce:drillWorkOrder:openLeft',NULL,7,0,1,50,'2023-08-09 15:33:04','2024-08-07 09:52:55',1);
INSERT INTO `sys_menu` VALUES (1370,'DRILLAMIN','提交',1031,'#',NULL,'#',3,'produce:drillWorkOrder:commit',NULL,6,0,1,50,'2023-08-09 15:36:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1371,'DRILLAMIN','重置任务',1031,'#',NULL,'#',3,'produce:drillWorkOrder:resetTask',NULL,10,0,1,50,'2023-08-09 15:37:03','2024-08-07 09:52:40',1);
INSERT INTO `sys_menu` VALUES (1372,'DRILLAMIN','报工',1350,'#',NULL,'#',3,'produce:reportWork:report',NULL,5,0,1,50,'2023-08-09 15:53:10','2024-08-05 16:08:10',1);
INSERT INTO `sys_menu` VALUES (1373,'DRILLAMIN','修改',1350,'#',NULL,'#',3,'produce:reportWork:edit',NULL,2,0,1,50,'2023-08-09 15:53:24','2024-08-05 16:07:52',1);
INSERT INTO `sys_menu` VALUES (1374,'DRILLAMIN','新增',1350,'#',NULL,'#',3,'produce:reportWork:add',NULL,2,0,1,50,'2023-08-09 15:53:38','2024-09-29 10:14:09',1);
INSERT INTO `sys_menu` VALUES (1375,'DRILLAMIN','删除',1350,'#',NULL,'#',3,'produce:reportWork:remove',NULL,3,0,1,50,'2023-08-09 15:53:52','2024-08-05 16:08:01',1);
INSERT INTO `sys_menu` VALUES (1376,'DRILLAMIN','生成检验记录',1350,'#',NULL,'#',3,'produce:reportWork:checkRecord',NULL,6,0,1,50,'2023-08-09 15:54:47','2024-08-05 16:08:15',1);
INSERT INTO `sys_menu` VALUES (1377,'DRILLAMIN','撤销',1165,'#',NULL,'#',3,'produce:reportRecords:revoke',NULL,7,0,1,50,'2023-08-09 15:59:17','2024-08-05 16:09:26',1);
INSERT INTO `sys_menu` VALUES (1378,'DRILLAMIN','审批',1165,'#',NULL,'#',3,'produce:reportRecords:commit',NULL,6,0,1,50,'2023-08-09 15:59:36','2024-08-06 16:53:14',1);
INSERT INTO `sys_menu` VALUES (1379,'DRILLAMIN','调整路线',1122,'#',NULL,'#',3,'produce:workorder:config',NULL,8,0,1,50,'2023-08-09 16:19:55','2023-09-15 10:18:39',1);
INSERT INTO `sys_menu` VALUES (1380,'DRILLAMIN','跳转甘特图',1029,'#',NULL,'#',3,'produce:schedule:editGantt',NULL,5,0,1,50,'2023-08-09 16:20:48','2024-08-06 17:11:22',1);
INSERT INTO `sys_menu` VALUES (1381,'DRILLAMIN','查看',1320,'#',NULL,'#',3,'masterData:autocodeRule:view',NULL,4,0,1,1,'2023-08-12 17:10:19','2024-08-05 16:20:51',1);
INSERT INTO `sys_menu` VALUES (1383,'DRILLAMIN','查看',1033,'#',NULL,'#',3,'device:deviceType:view',NULL,3,0,1,1,'2023-08-19 14:41:31','2024-08-05 15:43:22',1);
INSERT INTO `sys_menu` VALUES (1384,'DRILLAMIN','查看',1319,'#',NULL,'#',3,'device:subject:view',NULL,3,0,1,1,'2023-08-19 14:42:21','2024-08-05 15:42:43',1);
INSERT INTO `sys_menu` VALUES (1385,'DRILLAMIN','查看',1317,'#',NULL,'#',3,'device:repair:view',NULL,4,0,1,1,'2023-08-19 14:42:53','2024-08-07 10:36:57',1);
INSERT INTO `sys_menu` VALUES (1386,'DRILLAMIN','查看',1037,'#',NULL,'#',3,'device:schedulement:view',NULL,4,0,1,1,'2023-08-19 14:43:25','2024-08-05 15:54:50',1);
INSERT INTO `sys_menu` VALUES (1387,'DRILLAMIN','查看',1038,'#',NULL,'#',3,'alarm:event:view',NULL,4,0,1,1,'2023-08-19 14:43:51','2024-08-07 10:57:24',1);
INSERT INTO `sys_menu` VALUES (1388,'DRILLAMIN','查看',1136,'#',NULL,'#',3,'alarm:setting:view',NULL,4,0,1,1,'2023-08-19 14:44:15','2024-08-05 15:34:58',1);
INSERT INTO `sys_menu` VALUES (1389,'DRILLAMIN','查看',1039,'#',NULL,'#',3,'alarm:warn:view',NULL,4,0,1,1,'2023-08-19 14:44:37','2024-08-05 15:35:19',1);
INSERT INTO `sys_menu` VALUES (1390,'DRILLAMIN','查看',1055,'#',NULL,'#',3,'notify:setting:view',NULL,4,0,1,1,'2023-08-19 14:45:04','2024-08-05 15:18:49',1);
INSERT INTO `sys_menu` VALUES (1391,'DRILLAMIN','查看',1056,'#',NULL,'#',3,'notify:record:view',NULL,4,0,1,1,'2023-08-19 14:45:26','2024-08-05 15:18:59',1);
INSERT INTO `sys_menu` VALUES (1393,'DRILLAMIN','查看',3,'#',NULL,'#',3,'system:role:view',NULL,4,0,1,1,'2023-08-19 14:46:39','2024-08-05 15:14:08',1);
INSERT INTO `sys_menu` VALUES (1394,'DRILLAMIN','查看',4,'#',NULL,'#',3,'system:menu:view',NULL,4,0,1,1,'2023-08-19 14:47:14','2024-08-05 15:13:21',1);
INSERT INTO `sys_menu` VALUES (1395,'DRILLAMIN','查看',5,'#',NULL,'#',3,'system:dept:view',NULL,5,0,1,1,'2023-08-19 14:47:49','2024-08-05 15:11:53',1);
INSERT INTO `sys_menu` VALUES (1396,'DRILLAMIN','查看',6,'#',NULL,'#',3,'system:post:view',NULL,4,0,1,1,'2023-08-19 14:48:12','2024-08-05 15:12:32',1);
INSERT INTO `sys_menu` VALUES (1397,'DRILLAMIN','应用查看',8,'#',NULL,'#',3,'system:app:view',NULL,6,0,1,1,'2023-08-19 14:48:39',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1399,'DRILLAMIN','在线设备',1026,'#','onlineDevice','device/onlineDevice/index',2,'device:onlineDevice:list',NULL,7,0,1,1,'2023-08-21 13:44:42','2023-10-09 13:00:06',1);
INSERT INTO `sys_menu` VALUES (1400,'DRILLAMIN','历史数据',1158,'#','historicalData','wareHouse/historicalData/index',2,'wareHouse:historicalData:list',NULL,1,0,0,1,'2023-08-21 13:54:34','2024-08-09 09:29:38',1);
INSERT INTO `sys_menu` VALUES (1402,'DRILLAMIN','开始',1350,'#',NULL,'#',3,'produce:reportWork:start',NULL,7,0,1,1,'2023-08-28 15:00:29','2024-08-05 16:08:19',1);
INSERT INTO `sys_menu` VALUES (1403,'DRILLAMIN','完成',1350,'#',NULL,'#',3,'produce:reportWork:finish',NULL,8,0,1,1,'2023-08-28 15:01:32','2024-08-05 16:08:23',1);
INSERT INTO `sys_menu` VALUES (1417,'DRILLAMIN','生产任务',1024,'#','tasks','produce/tasks/index',2,'produce:tasks:list',NULL,25,0,1,1,'2023-09-05 10:57:57','2023-12-23 20:29:02',1);
INSERT INTO `sys_menu` VALUES (1419,'DRILLAMIN','任务分布',1024,'#','taskCharts','produce/taskCharts/index',2,'produce:taskCharts:list',NULL,30,0,1,1,'2023-09-08 10:01:19','2023-12-23 20:28:52',1);
INSERT INTO `sys_menu` VALUES (1420,'DRILLAMIN','撤销',1417,'#',NULL,'#',3,'produce:tasks:revoke',NULL,6,0,1,1,'2023-09-13 15:29:47','2024-08-06 16:44:31',1);
INSERT INTO `sys_menu` VALUES (1421,'DRILLAMIN','提交',1417,'#',NULL,'#',3,'produce:tasks:commit',NULL,5,0,1,1,'2023-09-13 15:30:07','2024-08-05 16:35:57',1);
INSERT INTO `sys_menu` VALUES (1423,'DRILLAMIN','查看',1417,'#',NULL,'#',3,'produce:tasks:view',NULL,3,0,1,1,'2023-09-13 15:30:51','2024-08-06 16:44:20',1);
INSERT INTO `sys_menu` VALUES (1424,'DRILLAMIN','新增',1417,'#',NULL,'#',3,'produce:tasks:add',NULL,2,0,1,1,'2023-09-13 15:31:06','2024-09-29 10:13:48',1);
INSERT INTO `sys_menu` VALUES (1425,'DRILLAMIN','删除',1417,'#',NULL,'#',3,'produce:tasks:remove',NULL,2,0,1,1,'2023-09-13 15:31:25','2024-08-06 16:44:17',1);
INSERT INTO `sys_menu` VALUES (1426,'DRILLAMIN','开始',1417,'#',NULL,'#',3,'produce:tasks:start',NULL,7,0,1,1,'2023-09-13 15:33:18','2024-08-06 16:44:39',1);
INSERT INTO `sys_menu` VALUES (1427,'DRILLAMIN','完成',1417,'#',NULL,'#',3,'produce:tasks:finish',NULL,8,0,1,1,'2023-09-13 15:33:37','2024-08-06 16:44:42',1);
INSERT INTO `sys_menu` VALUES (1428,'DRILLAMIN','解除锁定',1399,'#',NULL,'#',3,'device:onlineDevice:unlockIt',NULL,2,0,1,1,'2023-09-22 15:24:35','2024-08-05 16:38:22',1);
INSERT INTO `sys_menu` VALUES (1429,'DRILLAMIN','重置信号',1399,'#',NULL,'#',3,'device:onlineDevice:resetSignal',NULL,3,0,1,1,'2023-09-22 15:24:57','2024-08-05 16:38:28',1);
INSERT INTO `sys_menu` VALUES (1430,'DRILLAMIN','审批',1156,'#',NULL,'#',3,'masterData:productCategory:commit',NULL,6,0,1,1,'2023-09-22 16:17:37','2024-08-05 16:33:23',1);
INSERT INTO `sys_menu` VALUES (1431,'DRILLAMIN','审批',1157,'#',NULL,'#',3,'produce:route:commit',NULL,6,0,1,1,'2023-09-22 16:24:07','2024-08-05 16:34:32',1);
INSERT INTO `sys_menu` VALUES (1432,'DRILLAMIN','解析',1161,'#',NULL,'#',3,'material:drillFile:parse',NULL,6,0,1,1,'2023-09-22 16:48:52','2024-08-05 16:37:58',1);
INSERT INTO `sys_menu` VALUES (1433,'DRILLAMIN','下载文件',1162,'#',NULL,'#',3,'material:atpFile:download',NULL,6,0,1,1,'2023-09-22 16:50:17','2024-08-05 16:38:05',1);
INSERT INTO `sys_menu` VALUES (1434,'DRILLAMIN','排刀',1162,'#',NULL,'#',3,'material:atpFile:rowknives',NULL,5,0,1,1,'2023-09-22 16:50:46','2024-08-05 16:38:09',1);
INSERT INTO `sys_menu` VALUES (1435,'DRILLAMIN','出库',1160,'#',NULL,'#',3,'warehouse:materialStock:outbound',NULL,5,0,1,1,'2023-09-22 17:05:32','2024-08-05 16:41:29',1);
INSERT INTO `sys_menu` VALUES (1436,'DRILLAMIN','撤销',1156,'#',NULL,'#',3,'masterData:productCategory:revoke',NULL,7,0,1,1,'2023-09-25 09:53:43','2024-08-05 16:33:39',1);
INSERT INTO `sys_menu` VALUES (1439,'DRILLAMIN','撤销',1157,'#',NULL,'#',3,'produce:route:revoke',NULL,5,0,1,1,'2023-09-25 09:55:29','2024-08-05 16:34:38',1);
INSERT INTO `sys_menu` VALUES (1440,'DRILLAMIN','调度配置',1026,'#','scheduleConfig','device/scheduleConfig/index',2,'device:scheduleConfig:list',NULL,15,0,1,1,'2023-10-09 15:10:20','2024-09-06 14:30:22',1);
INSERT INTO `sys_menu` VALUES (1442,'DRILLAMIN','关联工艺路线设置',1156,'#',NULL,'#',3,'masterData:productCategory:setRoute',NULL,9,0,1,1,'2023-10-13 09:38:24','2024-08-05 16:33:49',1);
INSERT INTO `sys_menu` VALUES (1443,'DRILLAMIN','关联工艺路线上移',1156,'#',NULL,'#',3,'masterData:productCategory:moveUp',NULL,12,0,1,1,'2023-10-13 09:39:42','2024-08-06 10:23:59',1);
INSERT INTO `sys_menu` VALUES (1444,'DRILLAMIN','关联工艺路线删除',1156,'#',NULL,'#',3,'masterData:productCategory:removeRoute',NULL,10,0,1,1,'2023-10-13 09:41:01','2024-08-06 10:23:49',1);
INSERT INTO `sys_menu` VALUES (1445,'DRILLAMIN','关联工艺路线下移',1156,'#',NULL,'#',3,'masterData:productCategory:moveDown',NULL,11,0,1,1,'2023-10-13 09:41:43','2024-08-05 16:34:00',1);
INSERT INTO `sys_menu` VALUES (1446,'DRILLAMIN','关联工艺路线添加',1156,'#',NULL,'#',3,'masterData:productCategory:addRoute',NULL,8,0,1,1,'2023-10-13 09:44:19','2024-08-05 16:33:44',1);
INSERT INTO `sys_menu` VALUES (1447,'DRILLAMIN','关联工艺路线新增',1153,'#',NULL,'#',3,'masterData:workstation:addRoute',NULL,7,0,1,1,'2023-10-13 09:50:07','2024-08-05 16:34:15',1);
INSERT INTO `sys_menu` VALUES (1448,'DRILLAMIN','关联工艺路线删除',1153,'#',NULL,'#',3,'masterData:workstation:removeRoute',NULL,8,0,1,1,'2023-10-13 09:50:34','2024-08-05 16:34:19',1);
INSERT INTO `sys_menu` VALUES (1449,'DRILLAMIN','新增任务',1029,'#',NULL,'#',3,'produce:schedule:addTask',NULL,6,0,1,1,'2023-10-13 10:02:03','2024-08-06 17:11:25',1);
INSERT INTO `sys_menu` VALUES (1450,'DRILLAMIN','删除任务',1029,'#',NULL,'#',3,'produce:schedule:removeTask',NULL,8,0,1,1,'2023-10-13 10:02:26','2024-08-06 17:11:32',1);
INSERT INTO `sys_menu` VALUES (1451,'DRILLAMIN','修改任务',1029,'#',NULL,'#',3,'produce:schedule:editTask',NULL,7,0,1,1,'2023-10-13 10:02:47','2024-08-06 17:11:28',1);
INSERT INTO `sys_menu` VALUES (1452,'DRILLAMIN','提交任务',1029,'#',NULL,'#',3,'produce:schedule:commitTask',NULL,9,0,1,1,'2023-10-13 10:03:43','2024-08-06 17:13:00',1);
INSERT INTO `sys_menu` VALUES (1453,'DRILLAMIN','撤销任务',1029,'#',NULL,'#',3,'produce:schedule:revokeTask',NULL,10,0,1,1,'2023-10-13 10:04:06','2024-08-06 17:13:05',1);
INSERT INTO `sys_menu` VALUES (1456,'DRILLAMIN','转发任务',1417,'#',NULL,'#',3,'produce:tasks:transferTask',NULL,4,0,1,1,'2023-10-15 17:11:32','2024-08-05 16:37:04',1);
INSERT INTO `sys_menu` VALUES (1457,'DRILLAMIN','系统配置',1,'form','parameter','system/parameter/index',2,'system:parameter:list',NULL,7,0,1,1,'2023-10-18 08:56:24','2024-08-06 09:22:27',1);
INSERT INTO `sys_menu` VALUES (1458,'DRILLAMIN','新增',1457,'#',NULL,'#',3,'system:parameter:add',NULL,10,0,1,1,'2023-10-27 11:38:38','2024-08-05 16:41:40',1);
INSERT INTO `sys_menu` VALUES (1459,'DRILLAMIN','编辑',1457,'#',NULL,'#',3,'system:parameter:edit',NULL,30,0,1,1,'2023-10-27 11:38:56','2024-08-05 16:41:47',1);
INSERT INTO `sys_menu` VALUES (1460,'DRILLAMIN','删除',1457,'#',NULL,'#',3,'system:parameter:remove',NULL,20,0,1,1,'2023-10-27 11:39:13','2024-08-05 16:41:44',1);
INSERT INTO `sys_menu` VALUES (1462,'DRILLAMIN','新增',1440,'#',NULL,'#',3,'device:scheduleConfig:add',NULL,2,0,1,1,'2023-11-03 09:48:35','2024-09-30 11:30:13',1);
INSERT INTO `sys_menu` VALUES (1463,'DRILLAMIN','修改',1440,'#',NULL,'#',3,'device:scheduleConfig:edit',NULL,2,0,1,1,'2023-11-03 09:48:50','2024-08-05 16:39:44',1);
INSERT INTO `sys_menu` VALUES (1464,'DRILLAMIN','删除',1440,'#',NULL,'#',3,'device:scheduleConfig:remove',NULL,3,0,1,1,'2023-11-03 09:49:04','2024-08-05 16:39:16',1);
INSERT INTO `sys_menu` VALUES (1465,'DRILLAMIN','查看',1440,'#',NULL,'#',3,'device:scheduleConfig:view',NULL,4,0,1,1,'2023-11-03 09:49:19','2024-08-05 16:39:21',1);
INSERT INTO `sys_menu` VALUES (1466,'DRILLAMIN','配置路线',1440,'#',NULL,'#',3,'device:scheduleConfig:config',NULL,5,0,1,1,'2023-11-03 09:49:29','2024-08-05 16:39:01',1);
INSERT INTO `sys_menu` VALUES (1468,'DRILLAMIN','调度大屏',1026,'#','dashboard','device/dashboard',2,'device:dashboard:list',NULL,1,0,1,1,'2023-11-08 17:18:42','2024-09-09 13:16:35',1);
INSERT INTO `sys_menu` VALUES (1469,'DRILLAMIN','详情',1399,'#',NULL,'#',3,'device:onlineDevice:detail',NULL,2,0,1,1,'2023-11-15 08:47:18','2024-09-29 10:21:58',1);
INSERT INTO `sys_menu` VALUES (1470,'DRILLAMIN','设备负荷',1026,'#','deviceLoad','device/deviceLoad/index',2,'device:deviceLoad:list',NULL,10,0,1,1,'2023-11-16 13:59:11','2023-12-21 04:14:46',1);
INSERT INTO `sys_menu` VALUES (1471,'DRILLAMIN','查看任务',1470,'#',NULL,'#',3,'device:deviceLoad:view',NULL,2,0,1,1,'2023-11-16 16:32:36','2024-09-29 10:23:03',1);
INSERT INTO `sys_menu` VALUES (1472,'DRILLAMIN','工单分布',1024,'#','ticketCharts','produce/ticketCharts/index',2,'produce:ticketCharts:list',NULL,20,0,1,1,'2023-12-22 01:37:28','2023-12-23 20:29:16',1);
INSERT INTO `sys_menu` VALUES (1473,'DRILLAMIN','设备网关',1026,'#','deviceGateway','device/deviceGateway/index',2,'device:deviceGateway:list',NULL,12,0,1,1,'2023-12-26 20:06:52','2024-03-18 16:02:09',1);
INSERT INTO `sys_menu` VALUES (1476,'DRILLAMIN','新增',1473,'#',NULL,'#',3,'device:deviceGateway:add',NULL,2,0,1,1,'2024-01-11 16:49:28','2024-08-05 16:40:30',1);
INSERT INTO `sys_menu` VALUES (1477,'DRILLAMIN','修改',1473,'#',NULL,'#',3,'device:deviceGateway:edit',NULL,2,0,1,1,'2024-01-11 16:49:43','2024-08-05 16:40:20',1);
INSERT INTO `sys_menu` VALUES (1478,'DRILLAMIN','删除',1473,'#',NULL,'#',3,'device:deviceGateway:remove',NULL,4,0,1,1,'2024-01-11 16:49:54','2024-09-30 11:29:35',1);
INSERT INTO `sys_menu` VALUES (1479,'DRILLAMIN','安装',1473,'#',NULL,'#',3,'device:deviceGateway:install',NULL,4,0,1,1,'2024-01-11 16:50:51','2024-08-05 16:39:59',1);
INSERT INTO `sys_menu` VALUES (1480,'DRILLAMIN','检查更新',1473,'#',NULL,'#',3,'device:deviceGateway:check',NULL,5,0,1,1,'2024-01-11 16:51:19','2024-08-05 16:39:53',1);
INSERT INTO `sys_menu` VALUES (1481,'DRILLAMIN','生成任务',1122,'#',NULL,'#',3,'produce:workorder:generatetasks',NULL,9,0,1,1,'2024-01-16 09:30:52','2024-08-05 16:34:59',1);
INSERT INTO `sys_menu` VALUES (1483,'DRILLAMIN','标记颜色',1122,'#',NULL,'#',3,'produce:workorder:remarkcolor',NULL,6,0,1,1,'2024-02-03 16:18:42','2024-08-05 16:34:50',1);
INSERT INTO `sys_menu` VALUES (1484,'DRILLAMIN','料仓管理',1158,'#','siloManage','wareHouse/siloManage/index',2,'warehouse:siloManage:list',NULL,8,0,1,1,'2024-02-04 09:49:47','2024-08-09 09:32:07',1);
INSERT INTO `sys_menu` VALUES (1485,'DRILLAMIN','库位管理',1158,'#','rackManage','wareHouse/rackManage/index',2,'warehouse:rackManage:list',NULL,9,0,1,1,'2024-02-05 13:37:28','2024-08-09 09:30:48',1);
INSERT INTO `sys_menu` VALUES (1486,'DRILLAMIN','库位看板',1158,'#','rackDashboard','wareHouse/dashboard/rack',2,'warehouse:rackDashboard:list',NULL,2,0,1,1,'2024-02-05 15:10:38','2024-08-09 09:29:52',1);
INSERT INTO `sys_menu` VALUES (1488,'DRILLAMIN','新增',1484,'#',NULL,'#',3,'warehouse:siloManage:add',NULL,2,0,1,1,'2024-02-05 16:51:28','2024-09-30 11:34:27',1);
INSERT INTO `sys_menu` VALUES (1489,'DRILLAMIN','修改',1484,'#',NULL,'#',3,'warehouse:siloManage:edit',NULL,2,0,1,1,'2024-02-05 16:51:56','2024-08-05 16:43:34',1);
INSERT INTO `sys_menu` VALUES (1490,'DRILLAMIN','删除',1484,'#',NULL,'#',3,'warehouse:siloManage:remove',NULL,3,0,1,1,'2024-02-05 16:52:09','2024-08-05 16:43:42',1);
INSERT INTO `sys_menu` VALUES (1492,'DRILLAMIN','修改',1485,'#',NULL,'#',3,'warehouse:rackManage:edit',NULL,2,0,1,1,'2024-02-05 16:54:01','2024-08-05 16:42:40',1);
INSERT INTO `sys_menu` VALUES (1493,'DRILLAMIN','新增',1485,'#',NULL,'#',3,'warehouse:rackManage:add',NULL,2,0,1,1,'2024-02-05 16:54:29','2024-09-30 11:34:48',1);
INSERT INTO `sys_menu` VALUES (1494,'DRILLAMIN','删除',1485,'#',NULL,'#',3,'warehouse:rackManage:remove',NULL,3,0,1,1,'2024-02-05 16:55:05','2024-08-05 16:42:44',1);
INSERT INTO `sys_menu` VALUES (1495,'DRILLAMIN','加载板料',1484,'#',NULL,'#',3,'warehouse:siloManage:load',NULL,6,0,1,1,'2024-02-23 13:47:54','2024-08-07 11:42:51',1);
INSERT INTO `sys_menu` VALUES (1497,'DRILLAMIN','清空',1484,'#',NULL,'#',3,'warehouse:siloManage:clear',NULL,8,0,1,1,'2024-03-01 11:48:15','2024-08-05 16:43:06',1);
INSERT INTO `sys_menu` VALUES (1498,'DRILLAMIN','绑定',1484,'#',NULL,'#',3,'warehouse:siloManage:bind',NULL,7,0,1,1,'2024-03-06 16:13:32','2024-08-05 16:43:01',1);
INSERT INTO `sys_menu` VALUES (1499,'DRILLAMIN','解绑料仓',1486,'#','','#',3,'warehouse:rackDashboard:unbind',NULL,2,0,1,1,'2024-03-08 17:07:06','2024-08-05 16:40:58',1);
INSERT INTO `sys_menu` VALUES (1500,'DRILLAMIN','基本配置',1,'form','config','system/config/index',2,'system:config:list',NULL,8,0,1,1,'2024-03-11 14:25:29','2024-08-07 11:46:24',1);
INSERT INTO `sys_menu` VALUES (1501,'DRILLAMIN','通讯异常',1026,'#','abnormal','device/commAbnormal/index',2,'device:commAbnormal:list',NULL,19,0,1,1,'2024-03-18 15:47:10','2024-03-18 16:01:59',1);
INSERT INTO `sys_menu` VALUES (1502,'DRILLAMIN','导入',1485,'#','','#',3,'warehouse:rackManage:import',NULL,4,0,1,1,'2024-03-21 14:51:48','2024-08-05 16:42:48',1);
INSERT INTO `sys_menu` VALUES (1503,'DRILLAMIN','导入',1484,'#','','#',3,'warehouse:siloManage:import',NULL,5,0,1,1,'2024-03-21 14:52:40','2024-08-07 11:42:55',1);
INSERT INTO `sys_menu` VALUES (1504,'DRILLAMIN','查看',1484,'#','','#',3,'warehouse:siloManage:view',NULL,4,0,1,1,'2024-05-17 20:06:02','2024-08-05 16:43:46',1);
INSERT INTO `sys_menu` VALUES (1505,'DRILLAMIN','启用',1485,'#','','#',3,'warehouse:rackManage:enabled',NULL,5,0,1,1,'2024-05-21 13:31:14','2024-08-05 16:42:52',1);
INSERT INTO `sys_menu` VALUES (1506,'DRILLAMIN','禁用',1485,'#','','#',3,'warehouse:rackManage:disabled',NULL,6,0,1,1,'2024-05-21 13:31:30','2024-08-05 16:42:31',1);
INSERT INTO `sys_menu` VALUES (1507,'DRILLAMIN','绑定料仓',1486,'#','','#',3,'warehouse:rackDashboard:bind',NULL,1,0,1,1,'2024-06-14 09:10:20','2024-08-05 16:40:54',1);
INSERT INTO `sys_menu` VALUES (1508,'DRILLAMIN','启用禁用',1486,'#','','#',3,'warehouse:rackDashboard:changeStatus',NULL,3,0,1,60,'2024-06-14 09:16:47','2024-08-05 16:41:06',1);
INSERT INTO `sys_menu` VALUES (1509,'DRILLAMIN','setMoveInTime',1122,'#','','#',3,'produce:workorder:setMoveInTime',NULL,10,0,1,1,'2024-07-18 09:46:04','2024-08-05 16:35:10',1);
INSERT INTO `sys_menu` VALUES (1510,'DRILLAMIN','setMoveOutTime',1122,'#','','#',3,'produce:workorder:setMoveOutTime',NULL,11,0,1,1,'2024-07-18 09:48:51','2024-08-05 16:35:17',1);
INSERT INTO `sys_menu` VALUES (1511,'DRILLAMIN','setTrackInTime',1122,'#','','#',3,'produce:workorder:setTrackInTime',NULL,12,0,1,1,'2024-07-18 09:49:18','2024-08-05 16:35:23',1);
INSERT INTO `sys_menu` VALUES (1512,'DRILLAMIN','setTrackOutTime',1122,'#','','#',3,'produce:workorder:setTrackOutTime',NULL,13,0,1,1,'2024-07-18 09:49:36','2024-08-05 16:35:28',1);
INSERT INTO `sys_menu` VALUES (1513,'DRILLAMIN','save',1500,'#','','#',3,'system:config:save',NULL,2,0,1,1,'2024-07-18 10:55:28','2024-09-30 11:39:10',1);
INSERT INTO `sys_menu` VALUES (1514,'DRILLAMIN','料仓任务',1026,'#','transportationTask','device/transportationTask/index',2,'device:transportationTask:list',NULL,13,0,1,1,'2024-07-19 17:24:32','2024-09-06 14:30:31',1);
INSERT INTO `sys_menu` VALUES (1516,'DRILLAMIN','叠板看板',1158,'#','pinDashboard','wareHouse/dashboard/pin',2,'warehouse:pinDashboard:list',NULL,3,0,1,1,'2024-08-02 16:53:58','2024-08-07 11:40:36',1);
INSERT INTO `sys_menu` VALUES (1517,'DRILLAMIN','拆板看板',1158,'#','unpinDashboard','wareHouse/dashboard/unpin',2,'wareHouse:unpinDashboard:list',NULL,4,0,1,1,'2024-08-02 17:19:09','2024-08-09 09:30:00',1);
INSERT INTO `sys_menu` VALUES (1518,'DRILLAMIN','板料料架看板',1158,'#','panelSiloShelfDashboard','wareHouse/dashboard/panelSiloShelf',2,'wareHouse:panelSiloShelfDashboard:list',NULL,5,0,1,1,'2024-08-02 17:29:19','2024-08-09 09:30:07',1);
INSERT INTO `sys_menu` VALUES (1519,'DRILLAMIN','禁用',1035,'#','','#',3,'device:device:disabled',NULL,7,0,1,1,'2024-08-05 09:23:42','2024-08-05 16:38:51',1);
INSERT INTO `sys_menu` VALUES (1520,'DRILLAMIN','启用',1035,'#','','#',3,'device:device:enabled',NULL,6,0,1,1,'2024-08-05 09:24:21','2024-08-05 16:38:47',1);
INSERT INTO `sys_menu` VALUES (1521,'DRILLAMIN','板料叉齿看板',1158,'#','panelSiloForkDashboard','wareHouse/dashboard/panelSiloFork',2,'wareHouse:panelSiloForkDashboard:list',NULL,6,0,1,1,'2024-08-05 10:22:03','2024-08-09 09:30:23',1);
INSERT INTO `sys_menu` VALUES (1524,'DRILLAMIN','钻机看板',1026,'#','drillDashboard','device/drillDashboard/index',2,'device:drillDashboard:list',NULL,1,0,1,1,'2024-08-12 10:12:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1525,'DRILLAMIN','休息点列表',1158,'#','agvRest','wareHouse/agvRest/index',2,'wareHouse:agvRest:list',NULL,10,0,1,1,'2024-08-12 17:01:45','2024-08-28 16:02:23',1);
INSERT INTO `sys_menu` VALUES (1526,'DRILLAMIN','休息点配置',1158,'#','agvRestAndPart','wareHouse/agvRestAndPart/index',2,'wareHouse:agvRestAndPart:list',NULL,11,0,1,1,'2024-08-15 10:30:48','2024-08-28 16:02:32',1);
INSERT INTO `sys_menu` VALUES (1527,'DRILLAMIN','查看',1525,'#','','#',3,'wareHouse:agvRest:view',NULL,4,0,1,1,'2024-08-15 15:56:24',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1528,'DRILLAMIN','新增',1525,'#','','#',3,'wareHouse:agvRest:add',NULL,2,0,1,1,'2024-08-15 15:56:57','2024-09-30 11:35:38',1);
INSERT INTO `sys_menu` VALUES (1529,'DRILLAMIN','修改',1525,'#','','#',3,'wareHouse:agvRest:edit',NULL,2,0,1,1,'2024-08-15 15:57:11',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1530,'DRILLAMIN','删除',1525,'#','','#',3,'wareHouse:agvRest:remove',NULL,3,0,1,1,'2024-08-15 15:57:27','2024-08-15 15:57:59',1);
INSERT INTO `sys_menu` VALUES (1531,'DRILLAMIN','新增',1526,'#','','#',3,'wareHouse:agvRestAndPart:add',NULL,2,0,1,1,'2024-08-15 15:59:44','2024-09-30 11:36:02',1);
INSERT INTO `sys_menu` VALUES (1532,'DRILLAMIN','修改',1526,'#','','#',3,'wareHouse:agvRestAndPart:edit',NULL,2,0,1,1,'2024-08-15 16:00:12',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1533,'DRILLAMIN','删除',1526,'#','','#',3,'wareHouse:agvRestAndPart:remove',NULL,3,0,1,1,'2024-08-15 16:00:35',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1534,'DRILLAMIN','数据看板',0,'build','dashboard','#',1,'',NULL,8,0,1,1,'2024-08-21 11:49:02','2024-09-09 14:36:28',1);
INSERT INTO `sys_menu` VALUES (1535,'DRILLAMIN','库位看板',1534,'#','rack','wareHouse/dashboard/rack',2,'dashboard:rack:list',NULL,2,0,1,1,'2024-08-21 11:52:39','2024-10-17 09:17:43',1);
INSERT INTO `sys_menu` VALUES (1536,'DRILLAMIN','叠板看板',1534,'#','pin','wareHouse/dashboard/pin',2,'dashboard:pin:list',NULL,2,0,1,1,'2024-08-21 11:53:47','2024-10-17 09:17:56',1);
INSERT INTO `sys_menu` VALUES (1537,'DRILLAMIN','拆板看板',1534,'#','unpin','wareHouse/dashboard/unpin',2,'dashboard:unpin:list',NULL,3,0,1,1,'2024-08-21 14:26:37','2024-10-17 09:18:12',1);
INSERT INTO `sys_menu` VALUES (1538,'DRILLAMIN','板料料架看板',1534,'#','panelSiloShelf','wareHouse/dashboard/panelSiloShelf',2,'dashboard:panelSiloShelf:list',NULL,4,0,1,1,'2024-08-21 14:28:22','2024-10-17 09:19:26',1);
INSERT INTO `sys_menu` VALUES (1539,'DRILLAMIN','板料叉齿看板',1534,'#','panelSiloFork','wareHouse/dashboard/panelSiloFork',2,'dashboard:panelSiloFork:list',NULL,5,0,1,1,'2024-08-21 14:28:52','2024-10-17 09:19:58',1);
INSERT INTO `sys_menu` VALUES (1540,'DRILLAMIN','钻机看板',1534,'#','drill','device/drillDashboard/index',2,'dashboard:drill:list',NULL,1,0,1,1,'2024-08-21 14:29:45','2024-10-17 09:16:51',1);
INSERT INTO `sys_menu` VALUES (1541,'DRILLAMIN','调度大屏',1534,'#','schedule_dashboard','device/dashboard/index',2,'dashboard:schedule:list',NULL,1,0,1,1,'2024-08-21 14:40:24','2024-10-17 09:17:28',1);
INSERT INTO `sys_menu` VALUES (1542,'DRILLAMIN','导入',1525,'#','','#',3,'wareHouse:agvRest:import',NULL,4,0,1,1,'2024-08-29 09:05:51','2024-08-29 09:05:58',1);
INSERT INTO `sys_menu` VALUES (1543,'DRILLAMIN','导入',1526,'#','','#',3,'wareHouse:agvRestAndPart:import',NULL,4,0,1,1,'2024-08-29 09:08:13',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1546,'DRILLAMIN','提交任务',1122,'#','','#',3,'produce:workorder:commit',NULL,5,0,1,1,'2024-09-24 10:52:15',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1547,'DRILLAMIN','撤销任务',1122,'#','','#',3,'produce:workorder:revoke',NULL,5,0,1,1,'2024-09-24 10:54:08',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1548,'DRILLAMIN','外部工单',1024,'#','externalWorkOrder','produce/externalWorkOrder/index',2,'produce:externalWorkOrder:list',NULL,16,0,1,1,'2024-09-27 10:09:12','2024-09-27 10:22:39',1);
INSERT INTO `sys_menu` VALUES (1549,'DRILLAMIN','查看',1548,'#','','#',3,'produce:externalWorkOrder:view',NULL,2,0,1,1,'2024-09-29 09:07:06','2024-09-29 10:13:11',1);
INSERT INTO `sys_menu` VALUES (1550,'DRILLAMIN','修改',1548,'#','','#',3,'produce:externalWorkOrder:edit',NULL,3,0,1,1,'2024-09-29 09:07:24','2024-09-29 10:13:14',1);
INSERT INTO `sys_menu` VALUES (1551,'DRILLAMIN','删除',1548,'#','','#',3,'produce:externalWorkOrder:remove',NULL,4,0,1,1,'2024-09-29 09:07:40','2024-09-29 10:13:17',1);
INSERT INTO `sys_menu` VALUES (1552,'DRILLAMIN','查询',1149,'#','','#',3,'masterData:unitMeasure:list',NULL,1,0,1,1,'2024-09-29 10:06:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1553,'DRILLAMIN','查询',1320,'#','','#',3,'masterData:autocodeRule:list',NULL,1,0,1,1,'2024-09-29 10:07:35',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1554,'DRILLAMIN','查询',1156,'#','','#',3,'masterData:productCategory:list',NULL,1,0,1,1,'2024-09-29 10:08:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1555,'DRILLAMIN','查询',1154,'#','','#',3,'masterData:itemType:list',NULL,1,0,1,1,'2024-09-29 10:08:35',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1556,'DRILLAMIN','查询',1155,'#','','#',3,'masterData:item:list',NULL,1,0,1,1,'2024-09-29 10:09:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1557,'DRILLAMIN','查询',1152,'#','','#',3,'masterData:workshop:list',NULL,1,0,1,1,'2024-09-29 10:09:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1558,'DRILLAMIN','查询',1153,'#','','#',3,'masterData:workstation:list',NULL,1,0,1,1,'2024-09-29 10:10:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1559,'DRILLAMIN','查询',1150,'#','','#',3,'masterData:client:list',NULL,1,0,1,1,'2024-09-29 10:10:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1560,'DRILLAMIN','查询',1151,'#','','#',3,'masterData:vendor:list',NULL,1,0,1,1,'2024-09-29 10:10:49',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1561,'DRILLAMIN','查询',1128,'#','','#',3,'produce:process:list',NULL,1,0,1,1,'2024-09-29 10:11:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1562,'DRILLAMIN','查询',1157,'#','','#',3,'produce:route:list',NULL,1,0,1,1,'2024-09-29 10:12:05',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1563,'DRILLAMIN','查询',1122,'#','','#',3,'produce:workorder:list',NULL,1,0,1,1,'2024-09-29 10:12:33',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1564,'DRILLAMIN','查询',1548,'#','','#',3,'produce:externalWorkOrder:list',NULL,1,0,1,1,'2024-09-29 10:13:02',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1565,'DRILLAMIN','查询',1417,'#','','#',3,'produce:tasks:list',NULL,1,0,1,1,'2024-09-29 10:13:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1566,'DRILLAMIN','查询',1350,'#','','#',3,'produce:reportWork:list',NULL,1,0,1,1,'2024-09-29 10:14:05',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1567,'DRILLAMIN','查询',1165,'#','','#',3,'produce:reportRecords:list',NULL,1,0,1,1,'2024-09-29 10:14:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1568,'DRILLAMIN','查询',1029,'#','','#',3,'produce:schedule:list',NULL,1,0,1,1,'2024-09-29 10:14:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1569,'DRILLAMIN','查询',1031,'#','','#',3,'produce:drillWorkOrder:list',NULL,1,0,1,1,'2024-09-29 10:15:01',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1570,'DRILLAMIN','查询',1314,'#','','#',3,'produce:checkrecords:list',NULL,1,0,1,1,'2024-09-29 10:15:26',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1571,'DRILLAMIN','查询',1032,'#','','#',3,'material:cutter:list',NULL,1,0,1,1,'2024-09-29 10:18:43',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1572,'DRILLAMIN','查询',1036,'#','','#',3,'material:config:list',NULL,1,0,1,1,'2024-09-29 10:19:12',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1573,'DRILLAMIN','查询',1161,'#','','#',3,'material:drillFile:list',NULL,1,0,1,1,'2024-09-29 10:19:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1574,'DRILLAMIN','查询',1162,'#','','#',3,'material:atpFile:list',NULL,1,0,1,1,'2024-09-29 10:20:02',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1575,'DRILLAMIN','查询',1033,'#','','#',3,'device:deviceType:list',NULL,1,0,1,1,'2024-09-29 10:20:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1576,'DRILLAMIN','查询',1319,'#','','#',3,'device:subject:list',NULL,1,0,1,1,'2024-09-29 10:20:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1577,'DRILLAMIN','查询',1317,'#','','#',3,'device:repair:list',NULL,1,0,1,1,'2024-09-29 10:21:33',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1578,'DRILLAMIN','查询',1399,'#','','#',3,'device:onlineDevice:list',NULL,1,0,1,1,'2024-09-29 10:21:53',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1579,'DRILLAMIN','查询',1037,'#','','#',3,'device:schedulement:list',NULL,1,0,1,1,'2024-09-29 10:22:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1580,'DRILLAMIN','查询',1470,'#','','#',3,'device:deviceLoad:list',NULL,1,0,1,1,'2024-09-29 10:22:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1581,'DRILLAMIN','查询',1035,'#','','#',3,'device:device:list',NULL,1,0,1,1,'2024-09-29 10:23:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1582,'DRILLAMIN','查询',1473,'#','','#',3,'device:deviceGateway:list',NULL,1,0,1,1,'2024-09-30 11:29:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1583,'DRILLAMIN','查询',1440,'#','','#',3,'device:scheduleConfig:list',NULL,1,0,1,1,'2024-09-30 11:29:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1584,'DRILLAMIN','查询',1038,'#','','#',3,'alarm:event:list',NULL,1,0,1,1,'2024-09-30 11:30:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1585,'DRILLAMIN','查询',1136,'#','','#',3,'alarm:setting:list',NULL,1,0,1,1,'2024-09-30 11:31:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1586,'DRILLAMIN','查询',1039,'#','','#',3,'alarm:warn:list',NULL,1,0,1,1,'2024-09-30 11:31:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1587,'DRILLAMIN','查询',1055,'#','','#',3,'notify:setting:list',NULL,1,0,1,1,'2024-09-30 11:32:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1588,'DRILLAMIN','查询',1056,'#','','#',3,'notify:record:list',NULL,1,0,1,1,'2024-09-30 11:32:33',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1589,'DRILLAMIN','查询',1159,'#','','#',3,'warehouse:partition:list',NULL,1,0,1,1,'2024-09-30 11:33:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1590,'DRILLAMIN','查询',1484,'#','','#',3,'warehouse:materialStock:list',NULL,1,0,1,1,'2024-09-30 11:34:23',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1591,'DRILLAMIN','查询',1485,'#','','#',3,'warehouse:rackManage:list',NULL,1,0,1,1,'2024-09-30 11:34:44',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1592,'DRILLAMIN','查询',1030,'#','','#',3,'warehouse:board:list',NULL,1,0,1,1,'2024-09-30 11:35:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1593,'DRILLAMIN','查询',1525,'#','','#',3,'wareHouse:agvRest:list',NULL,1,0,1,1,'2024-09-30 11:35:24','2024-09-30 11:35:35',1);
INSERT INTO `sys_menu` VALUES (1594,'DRILLAMIN','查询',1526,'#','','#',3,'wareHouse:agvRestAndPart:list',NULL,1,0,1,1,'2024-09-30 11:35:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1595,'DRILLAMIN','查询',2,'#','','#',3,'system:user:list',NULL,1,0,1,1,'2024-09-30 11:36:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1596,'DRILLAMIN','查询',3,'#','','#',3,'system:role:list',NULL,1,0,1,1,'2024-09-30 11:36:49',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1597,'DRILLAMIN','查询',4,'#','','#',3,'system:menu:list',NULL,1,0,1,1,'2024-09-30 11:37:07',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1598,'DRILLAMIN','查询',5,'#','','#',3,'system:dept:list',NULL,1,0,1,1,'2024-09-30 11:38:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1599,'DRILLAMIN','查询',6,'#','','#',3,'system:post:list',NULL,1,0,1,1,'2024-09-30 11:38:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1600,'DRILLAMIN','查询',1457,'#','','#',3,'system:parameter:list',NULL,1,0,1,1,'2024-09-30 11:38:47',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1601,'DRILLAMIN','查询',1500,'#','','#',3,'system:config:list',NULL,1,0,1,1,'2024-09-30 11:39:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1602,'DRILLAMIN','海康物料',1148,'','hkItem','masterData/hkItem/index',2,'masterData:hkItem:list',NULL,7,0,1,1,'2024-10-29 14:18:25','2024-10-29 14:19:29',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=41153 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='角色--菜单权限';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_menu_auth`
--

LOCK TABLES `sys_menu_auth` WRITE;
/*!40000 ALTER TABLE `sys_menu_auth` DISABLE KEYS */;
INSERT INTO `sys_menu_auth` VALUES (40509,1,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40510,2,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40511,3,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40512,4,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40513,5,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40514,6,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40515,1001,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40516,1002,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40517,1003,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40518,1004,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40519,1006,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40520,1008,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40521,1009,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40522,1010,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40523,1013,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40524,1014,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40525,1015,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40526,1017,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40527,1018,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40528,1019,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40529,1021,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40530,1022,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40531,1023,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40532,1024,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40533,1026,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40534,1027,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40535,1030,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40536,1035,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40537,1037,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40538,1039,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40539,1058,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40540,1059,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40541,1060,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40542,1061,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40543,1068,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40544,1069,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40545,1070,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40546,1073,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40547,1074,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40548,1075,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40549,1103,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40550,1104,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40551,1105,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40552,1122,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40553,1124,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40554,1125,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40555,1126,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40556,1128,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40557,1130,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40558,1131,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40559,1132,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40560,1136,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40561,1138,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40562,1139,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40563,1140,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40564,1148,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40565,1149,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40566,1150,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40567,1154,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40568,1155,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40569,1156,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40570,1157,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40571,1158,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40572,1159,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40573,1208,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40574,1209,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40575,1210,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40576,1212,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40577,1220,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40578,1221,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40579,1222,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40580,1224,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40581,1232,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40582,1233,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40583,1234,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40584,1236,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40585,1238,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40586,1239,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40587,1240,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40588,1242,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40589,1262,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40590,1263,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40591,1264,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40592,1266,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40593,1268,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40594,1269,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40595,1270,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40596,1272,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40597,1274,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40598,1275,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40599,1276,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40600,1278,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40601,1279,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40602,1293,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40603,1294,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40604,1295,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40605,1296,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40606,1298,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40607,1299,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40608,1300,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40609,1301,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40610,1303,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40611,1304,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40612,1305,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40613,1306,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40614,1307,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40615,1309,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40616,1313,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40617,1324,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40618,1325,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40619,1327,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40620,1329,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40621,1330,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40622,1332,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40623,1336,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40624,1337,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40625,1338,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40626,1352,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40627,1355,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40628,1362,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40629,1363,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40630,1366,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40631,1379,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40632,1386,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40633,1388,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40634,1389,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40635,1393,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40636,1394,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40637,1395,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40638,1396,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40639,1399,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40640,1417,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40641,1419,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40642,1420,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40643,1421,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40644,1423,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40645,1424,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40646,1425,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40647,1426,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40648,1427,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40649,1428,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40650,1429,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40651,1430,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40652,1431,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40653,1436,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40654,1439,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40655,1440,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40656,1442,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40657,1443,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40658,1444,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40659,1445,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40660,1446,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40661,1456,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40662,1457,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40663,1458,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40664,1459,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40665,1460,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40666,1462,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40667,1463,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40668,1464,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40669,1465,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40670,1466,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40671,1468,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40672,1469,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40673,1470,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40674,1471,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40675,1472,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40676,1481,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40677,1483,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40678,1484,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40679,1485,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40680,1486,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40681,1488,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40682,1489,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40683,1490,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40684,1492,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40685,1493,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40686,1494,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40687,1495,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40688,1497,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40689,1498,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40690,1499,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40691,1500,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40692,1502,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40693,1503,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40694,1504,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40695,1505,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40696,1506,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40697,1507,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40698,1508,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40699,1509,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40700,1510,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40701,1511,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40702,1512,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40703,1513,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40704,1519,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40705,1520,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40706,1525,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40707,1526,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40708,1527,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40709,1528,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40710,1529,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40711,1530,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40712,1531,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40713,1532,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40714,1533,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40715,1534,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40716,1536,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40717,1537,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40718,1538,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40719,1539,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40720,1540,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40721,1541,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40722,1542,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40723,1543,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40724,1546,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40725,1547,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40726,1548,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40727,1549,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40728,1550,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40729,1551,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40730,1552,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40731,1554,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40732,1555,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40733,1556,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40734,1559,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40735,1561,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40736,1562,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40737,1563,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40738,1564,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40739,1565,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40740,1578,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40741,1579,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40742,1580,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40743,1581,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40744,1583,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40745,1585,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40746,1586,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40747,1589,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40748,1590,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40749,1591,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40750,1592,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40751,1593,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40752,1594,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40753,1595,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40754,1596,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40755,1597,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40756,1598,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40757,1599,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40758,1600,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40759,1601,14,1,59,'2024-10-19 20:13:49');
INSERT INTO `sys_menu_auth` VALUES (40760,1,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40761,2,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40762,3,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40763,4,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40764,5,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40765,6,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40766,1001,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40767,1002,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40768,1003,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40769,1004,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40770,1006,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40771,1008,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40772,1009,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40773,1010,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40774,1013,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40775,1014,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40776,1015,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40777,1017,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40778,1018,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40779,1019,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40780,1021,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40781,1022,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40782,1023,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40783,1024,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40784,1025,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40785,1026,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40786,1027,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40787,1029,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40788,1030,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40789,1031,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40790,1033,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40791,1035,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40792,1036,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40793,1037,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40794,1038,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40795,1039,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40796,1043,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40797,1044,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40798,1058,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40799,1059,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40800,1060,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40801,1061,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40802,1063,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40803,1064,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40804,1065,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40805,1068,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40806,1069,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40807,1070,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40808,1073,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40809,1074,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40810,1075,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40811,1078,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40812,1079,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40813,1080,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40814,1083,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40815,1084,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40816,1085,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40817,1098,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40818,1099,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40819,1100,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40820,1103,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40821,1104,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40822,1105,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40823,1122,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40824,1124,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40825,1125,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40826,1126,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40827,1128,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40828,1130,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40829,1131,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40830,1132,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40831,1136,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40832,1138,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40833,1139,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40834,1140,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40835,1148,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40836,1149,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40837,1150,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40838,1151,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40839,1152,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40840,1153,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40841,1154,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40842,1155,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40843,1156,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40844,1157,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40845,1158,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40846,1159,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40847,1160,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40848,1161,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40849,1162,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40850,1165,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40851,1167,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40852,1168,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40853,1169,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40854,1172,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40855,1173,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40856,1174,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40857,1176,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40858,1178,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40859,1179,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40860,1180,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40861,1182,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40862,1208,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40863,1209,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40864,1210,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40865,1212,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40866,1214,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40867,1215,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40868,1216,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40869,1218,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40870,1220,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40871,1221,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40872,1222,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40873,1224,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40874,1232,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40875,1233,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40876,1234,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40877,1236,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40878,1238,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40879,1239,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40880,1240,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40881,1242,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40882,1244,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40883,1245,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40884,1246,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40885,1248,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40886,1250,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40887,1251,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40888,1252,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40889,1254,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40890,1256,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40891,1257,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40892,1258,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40893,1260,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40894,1262,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40895,1263,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40896,1264,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40897,1266,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40898,1268,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40899,1269,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40900,1270,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40901,1272,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40902,1274,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40903,1275,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40904,1276,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40905,1278,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40906,1279,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40907,1293,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40908,1294,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40909,1295,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40910,1296,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40911,1298,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40912,1299,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40913,1300,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40914,1301,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40915,1303,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40916,1304,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40917,1305,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40918,1306,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40919,1307,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40920,1309,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40921,1310,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40922,1311,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40923,1312,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40924,1313,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40925,1314,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40926,1317,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40927,1319,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40928,1320,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40929,1323,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40930,1324,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40931,1325,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40932,1326,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40933,1327,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40934,1328,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40935,1329,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40936,1330,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40937,1331,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40938,1332,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40939,1333,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40940,1334,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40941,1335,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40942,1336,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40943,1337,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40944,1338,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40945,1339,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40946,1340,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40947,1341,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40948,1342,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40949,1343,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40950,1345,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40951,1346,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40952,1347,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40953,1348,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40954,1350,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40955,1351,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40956,1352,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40957,1354,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40958,1355,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40959,1358,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40960,1359,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40961,1360,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40962,1361,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40963,1362,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40964,1363,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40965,1364,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40966,1365,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40967,1366,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40968,1367,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40969,1368,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40970,1369,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40971,1370,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40972,1371,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40973,1372,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40974,1373,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40975,1374,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40976,1375,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40977,1376,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40978,1377,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40979,1378,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40980,1379,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40981,1380,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40982,1381,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40983,1383,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40984,1384,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40985,1385,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40986,1386,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40987,1387,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40988,1388,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40989,1389,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40990,1393,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40991,1394,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40992,1395,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40993,1396,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40994,1399,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40995,1402,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40996,1403,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40997,1417,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40998,1419,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (40999,1420,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41000,1421,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41001,1423,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41002,1424,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41003,1425,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41004,1426,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41005,1427,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41006,1428,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41007,1429,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41008,1430,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41009,1431,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41010,1432,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41011,1433,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41012,1434,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41013,1435,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41014,1436,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41015,1439,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41016,1440,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41017,1442,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41018,1443,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41019,1444,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41020,1445,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41021,1446,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41022,1447,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41023,1448,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41024,1449,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41025,1450,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41026,1451,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41027,1452,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41028,1453,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41029,1456,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41030,1457,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41031,1458,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41032,1459,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41033,1460,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41034,1462,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41035,1463,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41036,1464,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41037,1465,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41038,1466,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41039,1468,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41040,1469,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41041,1470,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41042,1471,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41043,1472,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41044,1473,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41045,1476,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41046,1477,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41047,1478,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41048,1479,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41049,1480,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41050,1481,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41051,1483,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41052,1484,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41053,1485,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41054,1486,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41055,1488,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41056,1489,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41057,1490,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41058,1492,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41059,1493,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41060,1494,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41061,1495,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41062,1497,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41063,1498,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41064,1499,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41065,1500,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41066,1501,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41067,1502,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41068,1503,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41069,1504,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41070,1505,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41071,1506,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41072,1507,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41073,1508,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41074,1509,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41075,1510,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41076,1511,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41077,1512,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41078,1513,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41079,1514,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41080,1516,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41081,1517,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41082,1518,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41083,1519,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41084,1520,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41085,1521,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41086,1524,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41087,1525,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41088,1526,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41089,1527,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41090,1528,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41091,1529,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41092,1530,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41093,1531,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41094,1532,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41095,1533,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41096,1534,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41097,1535,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41098,1542,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41099,1543,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41100,1546,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41101,1547,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41102,1548,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41103,1549,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41104,1550,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41105,1551,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41106,1552,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41107,1553,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41108,1554,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41109,1555,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41110,1556,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41111,1557,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41112,1558,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41113,1559,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41114,1560,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41115,1561,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41116,1562,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41117,1563,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41118,1564,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41119,1565,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41120,1566,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41121,1567,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41122,1568,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41123,1569,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41124,1570,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41125,1572,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41126,1573,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41127,1574,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41128,1575,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41129,1576,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41130,1577,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41131,1578,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41132,1579,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41133,1580,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41134,1581,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41135,1582,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41136,1583,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41137,1584,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41138,1585,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41139,1586,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41140,1589,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41141,1590,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41142,1591,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41143,1592,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41144,1593,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41145,1594,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41146,1595,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41147,1596,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41148,1597,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41149,1598,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41150,1599,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41151,1600,17,1,1,'2024-10-28 13:34:37');
INSERT INTO `sys_menu_auth` VALUES (41152,1601,17,1,1,'2024-10-28 13:34:37');
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='角色';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role`
--

LOCK TABLES `sys_role` WRITE;
/*!40000 ALTER TABLE `sys_role` DISABLE KEYS */;
INSERT INTO `sys_role` VALUES (1,'超级管理员',1,0,1,0,'2022-11-08 11:26:24',NULL,NULL);
INSERT INTO `sys_role` VALUES (14,'vega',1,0,1,1,'2023-09-27 10:04:23','2024-10-19 20:13:49',59);
INSERT INTO `sys_role` VALUES (17,'test',1,0,1,1,'2024-09-03 15:19:38','2024-10-28 13:34:37',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='用户';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (1,'admin','07c792bdd39e54e32a1cc5f272d5f6a2','e56707bf53794e19b4391bbe5ac53acf','超级管理员',1,NULL,1,NULL,NULL,'13400000002','13400000002@qq.com',NULL,NULL,1,NULL,NULL,0,1,NULL,'2022-09-27 16:07:39','2023-08-03 08:32:24',1);
INSERT INTO `sys_user` VALUES (59,'vega','72e136b7e2fea3f445d0d1269890a567','29b603453bb04c40a1712e4a93f13029','vega',1,2,0,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,1,'2023-11-24 19:23:01','2024-09-03 11:10:09',1);
INSERT INTO `sys_user` VALUES (60,'test','17f10979925e07d508f59533e5876fb7','0e41baecb133489eb04127f1b2cf8b96','test',2,4,1,NULL,NULL,'','',NULL,NULL,NULL,'',NULL,0,1,1,'2024-06-11 11:19:55','2024-09-19 17:16:39',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='用户对应角色';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (4,1,1,1,'2022-12-26 17:17:36');
INSERT INTO `sys_user_role` VALUES (39,59,14,1,'2023-11-24 19:23:01');
INSERT INTO `sys_user_role` VALUES (40,60,17,1,'2024-06-11 11:19:55');
/*!40000 ALTER TABLE `sys_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_agv_rest`
--

DROP TABLE IF EXISTS `t_agv_rest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_agv_rest` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `code` varchar(200) DEFAULT NULL COMMENT '休息点编码',
  `name` varchar(200) DEFAULT NULL COMMENT '休息点名称',
  `point` varchar(200) DEFAULT NULL COMMENT '物理点位',
  `pre_book_agv` varchar(255) DEFAULT NULL COMMENT '该分区该点预定分配的AGV',
  `pre_book_time` datetime DEFAULT NULL COMMENT '预约时间',
  `current_agv` varchar(255) DEFAULT NULL COMMENT '当前点正占用的AGV',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `restcode_UNIQE` (`code`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='AGV休息点';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_agv_rest`
--

LOCK TABLES `t_agv_rest` WRITE;
/*!40000 ALTER TABLE `t_agv_rest` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_agv_rest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_agv_rest_and_part`
--

DROP TABLE IF EXISTS `t_agv_rest_and_part`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_agv_rest_and_part` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `part_code` varchar(200) DEFAULT NULL COMMENT '分区编码',
  `part_name` varchar(200) DEFAULT NULL COMMENT '分区名称',
  `rest_id` int(11) DEFAULT NULL COMMENT '休息点id',
  `route_code` varchar(200) DEFAULT NULL COMMENT '工艺路线',
  `agv_device_kind` int(11) DEFAULT NULL COMMENT 'AGV 类型  6-提升AGV  10--顶升AGV',
  `priority` int(11) DEFAULT NULL COMMENT '该分区可分派的休息点的优先级  1>>2>>3',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='AGV休息点和分区关联关系';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_agv_rest_and_part`
--

LOCK TABLES `t_agv_rest_and_part` WRITE;
/*!40000 ALTER TABLE `t_agv_rest_and_part` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_agv_rest_and_part` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_agv_transfer_plan`
--

DROP TABLE IF EXISTS `t_agv_transfer_plan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_agv_transfer_plan` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `agv` varchar(100) DEFAULT NULL COMMENT 'agv id',
  `location_code` varchar(100) NOT NULL COMMENT '所要执行的任务的库位',
  `schedule_id` int(11) DEFAULT NULL COMMENT '执行的任务',
  `location_device_id` varchar(100) NOT NULL COMMENT '所要执行的任务的设备id',
  `sequence` int(11) NOT NULL COMMENT '任务编号从0开始',
  `task_no` bigint(20) NOT NULL COMMENT '任务编号(时间ticks)',
  `interaction_sequence` int(8) NOT NULL COMMENT '交互序列',
  `transfer_kind` int(8) NOT NULL COMMENT '类型：空仓、生料、熟料、首件',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `material_kind` int(11) NOT NULL COMMENT '交互物料类型',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `silo_code` varchar(100) NOT NULL COMMENT '料仓号',
  `retry_count` int(11) NOT NULL COMMENT 'agv尝试执行任务失败重试次数',
  `required_agv_kind` int(11) NOT NULL COMMENT '需要的agv类型',
  `allocated_time` datetime DEFAULT NULL COMMENT 'agv分配时间',
  `point_kind` int(11) DEFAULT NULL COMMENT '交接点类型',
  `scheduled_task_status` int(11) DEFAULT NULL COMMENT '调度任务状态',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='agv任务列表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_agv_transfer_plan`
--

LOCK TABLES `t_agv_transfer_plan` WRITE;
/*!40000 ALTER TABLE `t_agv_transfer_plan` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_agv_transfer_plan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_agv_transfer_task_detail`
--

DROP TABLE IF EXISTS `t_agv_transfer_task_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_agv_transfer_task_detail` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `task_no` bigint(20) NOT NULL COMMENT '任务号',
  `master_id` int(11) NOT NULL COMMENT '主表id',
  `agv_plan_received_item_code` varchar(100) NOT NULL COMMENT 'agv计划接收的料号',
  `agv_plan_received_item_quantity` int(11) DEFAULT NULL COMMENT 'agv计划接收的数量',
  `agv_plan_sent_item_code` varchar(100) DEFAULT NULL COMMENT 'agv计划给出去的料号',
  `agv_plan_sent_item_quantity` int(11) DEFAULT NULL COMMENT 'agv计划给出去的料数量',
  `agv_plan_interaction_sequence` int(8) NOT NULL COMMENT '计划交互序列',
  `material_kind` int(11) NOT NULL COMMENT '交互物料类型',
  `agv_real_received_item_code` varchar(100) DEFAULT NULL COMMENT 'agv实际接收到的料号',
  `agv_real_received_item_quantity` int(11) DEFAULT NULL COMMENT 'agv实际接收到的料号数量',
  `agv_real_sent_item_code` varchar(100) DEFAULT NULL COMMENT 'agv实际送出去的料号',
  `agv_real_sent_item_quantity` int(11) DEFAULT NULL COMMENT 'agv实际送出去的数量',
  `agv_real_interaction_sequence` int(11) DEFAULT NULL COMMENT 'agv实际执行的交互',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `allocated_time` datetime DEFAULT NULL COMMENT 'agv分配时间',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='agv运输任务明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_agv_transfer_task_detail`
--

LOCK TABLES `t_agv_transfer_task_detail` WRITE;
/*!40000 ALTER TABLE `t_agv_transfer_task_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_agv_transfer_task_detail` ENABLE KEYS */;
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
  `alarm_name` mediumtext COMMENT '告警名称',
  `alarm_level` tinyint(4) DEFAULT NULL COMMENT '告警级别（0通知、1告警、2紧急、3严重）',
  `device_id` int(11) DEFAULT NULL COMMENT '告警设备ID',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `event_id` int(11) NOT NULL COMMENT '事件Id',
  `event_data` mediumtext CHARACTER SET utf8 COMMENT '事件数据',
  `is_handled` tinyint(4) DEFAULT '0' COMMENT '是否已处理',
  `handle_time` datetime DEFAULT NULL COMMENT '处理时间',
  `sync_id` bigint(20) DEFAULT NULL COMMENT '时间戳，同步ID',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm`
--

LOCK TABLES `t_alarm` WRITE;
/*!40000 ALTER TABLE `t_alarm` DISABLE KEYS */;
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
  `notify_way_ids` mediumtext CHARACTER SET utf8 COMMENT '通知方式',
  `notify_way_names` mediumtext CHARACTER SET utf8 COMMENT '通知方式名称',
  `event_rules` mediumtext CHARACTER SET utf8 COMMENT '触发规则',
  `ancestors` mediumtext COMMENT '所有层级父节点',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='告警配置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_alarm_setting`
--

LOCK TABLES `t_alarm_setting` WRITE;
/*!40000 ALTER TABLE `t_alarm_setting` DISABLE KEYS */;
INSERT INTO `t_alarm_setting` VALUES (2,'123',0,'1231','',1,0,1,1,'2024-07-10 13:25:20','2024-07-10 13:25:20',NULL,20,'BUFFER结束上生料','4','短信','','0');
INSERT INTO `t_alarm_setting` VALUES (6,'334',0,'234','',3,0,1,1,'2024-09-20 14:56:39','2024-09-20 14:58:48',1,42,'AGV状态上报','2','钉钉','','0');
INSERT INTO `t_alarm_setting` VALUES (7,'22',0,'22','',3,0,1,1,'2024-09-20 16:26:54','2024-09-20 16:26:54',NULL,42,'AGV状态上报',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (8,'222',0,'222','',2,0,1,1,'2024-10-18 15:52:08','2024-10-18 15:52:08',NULL,40,'AGV上料仓',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (9,'222',0,'22222','',2,0,1,1,'2024-10-18 15:52:15','2024-10-18 15:52:15',NULL,39,'AGV下料仓',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (10,'222222',0,'222222','',NULL,0,1,1,'2024-10-18 15:52:19','2024-10-18 15:52:19',NULL,0,'',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (11,'33333',0,'333','',NULL,0,1,1,'2024-10-18 15:52:24','2024-10-18 15:52:24',NULL,0,'',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (12,'44444444444444',0,'4444444','',NULL,0,1,1,'2024-10-18 15:52:28','2024-10-18 15:52:28',NULL,0,'',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (13,'555555555555555',0,'55555555','',NULL,0,1,1,'2024-10-18 15:52:35','2024-10-18 15:52:35',NULL,0,'',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (14,'6666666666',0,'66666','',NULL,0,1,1,'2024-10-18 15:52:44','2024-10-18 15:52:44',NULL,0,'',NULL,'','','0');
INSERT INTO `t_alarm_setting` VALUES (15,'777777777',0,'777777','',NULL,0,1,1,'2024-10-18 15:52:47','2024-10-18 15:52:47',NULL,0,'',NULL,'','','0');
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
-- Table structure for table `t_colour`
--

DROP TABLE IF EXISTS `t_colour`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_colour` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '颜色名',
  `code` varchar(20) CHARACTER SET utf8 DEFAULT NULL COMMENT '颜色编码',
  `details` varchar(7) CHARACTER SET utf8 DEFAULT NULL COMMENT '颜色',
  `remark` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '备注',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `status` int(2) DEFAULT '0' COMMENT '状态',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='颜色';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_colour`
--

LOCK TABLES `t_colour` WRITE;
/*!40000 ALTER TABLE `t_colour` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_colour` ENABLE KEYS */;
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
  `tag` mediumtext CHARACTER SET utf8 COMMENT '设备标签, 类似a=b键值对',
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
  `ancestors` mediumtext CHARACTER SET utf8 COMMENT '所有层级父节点',
  `parameters` mediumtext CHARACTER SET utf8 COMMENT '设备参数',
  `device_status` varchar(255) DEFAULT NULL COMMENT '设备状态',
  `device_type_code` varchar(50) DEFAULT NULL COMMENT '设备类型代码',
  `device_kind` int(11) DEFAULT NULL COMMENT 'device kind',
  `spindle_num` int(11) DEFAULT NULL COMMENT '轴数',
  `interaction_position` int(11) DEFAULT NULL COMMENT '交互位置',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备管理';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device`
--

LOCK TABLES `t_device` WRITE;
/*!40000 ALTER TABLE `t_device` DISABLE KEYS */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备关联工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_and_route`
--

LOCK TABLES `t_device_and_route` WRITE;
/*!40000 ALTER TABLE `t_device_and_route` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_and_route` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_device_gateway`
--

DROP TABLE IF EXISTS `t_device_gateway`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_gateway` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '名称',
  `code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '编号',
  `c_version` varchar(50) DEFAULT NULL COMMENT '当前版本',
  `a_version` varchar(50) DEFAULT NULL COMMENT '可用版本',
  `visit_website` varchar(350) DEFAULT NULL,
  `setup_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '安装时间',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `status` int(1) DEFAULT '0' COMMENT '状态',
  `modify_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `is_online` int(11) DEFAULT '0' COMMENT '是否在线',
  `service_name` varchar(200) DEFAULT NULL COMMENT '服务名',
  `installed_location` mediumtext CHARACTER SET utf8 COMMENT '安装位置',
  `parameters` mediumtext CHARACTER SET utf8 COMMENT '参数',
  `app_name` varchar(80) CHARACTER SET utf8 NOT NULL COMMENT '应用名称',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备网关';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_gateway`
--

LOCK TABLES `t_device_gateway` WRITE;
/*!40000 ALTER TABLE `t_device_gateway` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_gateway` ENABLE KEYS */;
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
  `item_code` varchar(50) DEFAULT NULL COMMENT '料号',
  `lot_id` varchar(50) DEFAULT NULL COMMENT 'Lot二维码',
  `batch_code` varchar(50) DEFAULT NULL COMMENT '批次号',
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
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `location_code` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `idx_device_code_of_t_device_panel` (`device_code`),
  KEY `idx_item_code_of_t_device_panel` (`item_code`)
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
  `item_code` varchar(50) DEFAULT NULL COMMENT '料号',
  `lot_id` varchar(50) DEFAULT NULL COMMENT 'Lot二维码',
  `batch_code` varchar(50) DEFAULT NULL COMMENT '批次号',
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
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `location_code` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `idx_panel_code_of_t_device_panel_history` (`panel_code`),
  KEY `idx_device_code_of_t_device_panel_history` (`device_code`)
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
  `parameters` mediumtext CHARACTER SET utf8 COMMENT '参数',
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
-- Table structure for table `t_device_service_invocation`
--

DROP TABLE IF EXISTS `t_device_service_invocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_device_service_invocation` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `message_id` varchar(500) DEFAULT NULL,
  `request_topic` varchar(500) DEFAULT NULL COMMENT '请求',
  `response_topic` varchar(500) DEFAULT NULL COMMENT '响应',
  `payload` mediumtext COMMENT '请求详情',
  `retries` int(11) DEFAULT '0' COMMENT '尝试次数',
  `is_timeout` tinyint(1) DEFAULT '0' COMMENT '是否超时',
  `reason` varchar(500) DEFAULT NULL COMMENT '原因',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `first_invocation_timestamp` datetime DEFAULT NULL COMMENT '首次调用时间',
  `last_invocation_timestamp` datetime DEFAULT NULL COMMENT '末次调用时间',
  `mqtt_quality_of_service_level` int(11) DEFAULT '0' COMMENT '等级',
  `is_urgent` tinyint(4) DEFAULT '0' COMMENT '是否紧急',
  `is_dealed` tinyint(1) DEFAULT '0' COMMENT '是否已处理',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `routing_key` varchar(100) DEFAULT NULL COMMENT '主叫设备的route key',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `message_id` (`message_id`),
  KEY `is_dealed` (`is_dealed`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备服务调用';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_device_service_invocation`
--

LOCK TABLES `t_device_service_invocation` WRITE;
/*!40000 ALTER TABLE `t_device_service_invocation` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_device_service_invocation` ENABLE KEYS */;
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
INSERT INTO `t_device_type` VALUES (7,'钻机',1,0,1,1,'2023-02-28 10:26:41','2023-10-17 10:32:57',1,'drill','0,1',1);
INSERT INTO `t_device_type` VALUES (8,'AGV',1,0,1,1,'2023-02-28 13:27:37','2023-04-01 11:01:25',1,'agv','0,1',0);
INSERT INTO `t_device_type` VALUES (14,'叠板机',1,0,1,1,'2023-03-23 02:45:57','2023-04-01 11:01:48',1,'pin','0,1',1);
INSERT INTO `t_device_type` VALUES (15,'拆板机',1,0,1,1,'2023-03-23 02:46:09','2024-09-27 14:20:31',1,'unpin','0,1',1);
INSERT INTO `t_device_type` VALUES (16,'料架与插齿',1,0,1,1,'2023-03-23 02:46:19','2024-10-28 13:57:18',1,'siloshelves','0,1',0);
INSERT INTO `t_device_type` VALUES (17,'熟料仓暂存台',1,1,0,1,'2023-03-23 02:46:28','2023-07-28 09:30:48',1,'ProcessedStagingDesk','0,1',0);
/*!40000 ALTER TABLE `t_device_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_drill_panel_detail`
--

DROP TABLE IF EXISTS `t_drill_panel_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_drill_panel_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_code` varchar(50) NOT NULL COMMENT '设备编号',
  `item_code` varchar(50) DEFAULT NULL COMMENT '物料编号',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料编号',
  `product_status` int(8) DEFAULT NULL COMMENT '该层板料类型:（38000-等待钻孔, 39000-正在钻孔,40000-钻机完成加工）',
  `pcs` int(8) DEFAULT NULL COMMENT '每叠片数',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `layer` int(11) DEFAULT '0' COMMENT '0钻机, 1生料仓,2熟料仓',
  `splindle_index` int(11) DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `batch_code` varchar(50) DEFAULT NULL COMMENT '批次号',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻机载料明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_drill_panel_detail`
--

LOCK TABLES `t_drill_panel_detail` WRITE;
/*!40000 ALTER TABLE `t_drill_panel_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_drill_panel_detail` ENABLE KEYS */;
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
  `is_urgent` int(11) DEFAULT '0' COMMENT '是否紧急插单',
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COMMENT='编码生成规则';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_encode_build_rules`
--

LOCK TABLES `t_encode_build_rules` WRITE;
/*!40000 ALTER TABLE `t_encode_build_rules` DISABLE KEYS */;
INSERT INTO `t_encode_build_rules` VALUES (1,'UNITMEASURE_CODE','计量单位编码','JL',6,1,'DW','测试编码规则的备注tooltip ',0,1,1,'2023-05-18 02:58:43','2024-09-29 15:56:37',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (2,'ITEMTYPE_CODE','物料产品分类编码','IT',6,1,'BM','',0,1,1,'2023-05-18 02:58:43','2024-09-12 17:22:17',1,1,0,0);
INSERT INTO `t_encode_build_rules` VALUES (3,'ITEM_CODE','物料产品编码','IF',5,1,'A1','',0,1,1,'2023-05-18 02:58:43','2024-07-08 11:03:33',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (4,'WORKSTATION_CODE','工作站编码','WS',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-09-04 09:54:59',1,1,1,0);
INSERT INTO `t_encode_build_rules` VALUES (5,'CLIENT_CODE','客户管理编码','KH',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-08-31 10:33:15',1,0,1,0);
INSERT INTO `t_encode_build_rules` VALUES (6,'VENDOR_CODE','供应商编码','GYS',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-08-31 10:27:00',1,1,0,1);
INSERT INTO `t_encode_build_rules` VALUES (7,'WORKORDER_CODE','生产工单编码','MO',6,0,'','',0,1,1,'2023-05-18 02:58:43','2024-09-02 11:50:40',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (8,'PANEL_CODE','板料追溯编码','P',5,0,'','',0,1,1,'2023-05-18 02:58:43','2024-08-22 17:03:02',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (9,'TASK_CODE','任务编码','TO',6,1,'','',0,1,1,'2023-05-18 02:58:43','2023-09-14 17:02:02',1,1,1,1);
INSERT INTO `t_encode_build_rules` VALUES (10,'PARTITION_CODE','仓库编码','WH',10,1,NULL,'',0,1,1,'2023-05-18 02:58:43','2024-09-03 09:04:02',1,0,1,1);
INSERT INTO `t_encode_build_rules` VALUES (12,'MATERIALSTOCK_IN_CODE','库存入库','MSIN',3,1,NULL,NULL,0,1,1,'2023-06-19 15:52:32','2024-09-19 13:31:18',1,0,1,1);
INSERT INTO `t_encode_build_rules` VALUES (13,'MATERIALSTOCK_OUT_CODE','库存出库','MSOUT',6,1,NULL,NULL,0,1,1,'2023-06-19 15:53:18','2024-09-19 13:31:10',1,0,1,1);
INSERT INTO `t_encode_build_rules` VALUES (14,'WORKSHOP_CODE','车间编码','CJ',6,1,'CJ','',0,1,1,'2023-06-19 15:53:18','2024-09-23 11:24:00',1,1,1,0);
INSERT INTO `t_encode_build_rules` VALUES (15,'RACK_CODE','料架编码','RK',6,1,'','',0,1,1,'2023-05-18 02:58:43','2024-09-18 14:52:03',1,0,1,1);
INSERT INTO `t_encode_build_rules` VALUES (16,'SILO_CODE','料仓编码','SO',6,1,'','',0,1,1,'2023-05-18 02:58:43','2024-09-19 13:31:00',1,0,1,1);
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
  `parameter_json` mediumtext CHARACTER SET utf8 COMMENT '参数',
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
INSERT INTO `t_event_define` VALUES (47,'AGV_REQUEST_CHARGE_EVENT','AGV充电',1,' ',1,'2023-05-23 03:12:27','2024-09-29 16:10:10',1,NULL,0,0);
/*!40000 ALTER TABLE `t_event_define` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_external_work_order`
--

DROP TABLE IF EXISTS `t_external_work_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_external_work_order` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) DEFAULT NULL COMMENT '内部工单编号',
  `name` varchar(50) DEFAULT NULL COMMENT '内部工单名称',
  `order_source` varchar(50) CHARACTER SET utf8 DEFAULT '库存需求' COMMENT '来源类型',
  `source_code` varchar(50) CHARACTER SET utf8 NOT NULL COMMENT '来源单据',
  `client_code` varchar(50) DEFAULT NULL COMMENT '客户编号',
  `client_name` varchar(50) DEFAULT NULL COMMENT '客户名称',
  `item_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品编号',
  `item_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品名称',
  `specification` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '单位',
  `quantity` decimal(20,0) DEFAULT NULL COMMENT '生产数量',
  `panel_count` decimal(20,0) DEFAULT NULL COMMENT '叠板层数',
  `drill_count` decimal(12,0) DEFAULT NULL COMMENT '孔数',
  `route_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工艺路线编号',
  `route_name` varchar(60) CHARACTER SET utf8 DEFAULT NULL COMMENT '工艺路线名称',
  `request_date` datetime DEFAULT NULL COMMENT '需求日期',
  `dispense_machines` decimal(12,0) DEFAULT NULL COMMENT '建议机台数',
  `product_category_code` varchar(50) DEFAULT NULL COMMENT '大类编码',
  `wad_count` decimal(12,0) DEFAULT NULL COMMENT '待排产叠数',
  `is_urgent` tinyint(4) DEFAULT NULL COMMENT '是否紧急',
  `status` int(10) DEFAULT NULL COMMENT '处理标识 ',
  `device_codes` varchar(200) DEFAULT NULL COMMENT '分配的机台设备号',
  `remark` varchar(500) DEFAULT NULL COMMENT '处理结果描述',
  `is_deleted` tinyint(4) DEFAULT NULL,
  `creator_id` int(4) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  `modifier_id` int(11) DEFAULT NULL,
  `modify_time` datetime DEFAULT NULL,
  `inner_order_id` bigint(20) DEFAULT NULL COMMENT '内部工单ID',
  `incode_number` varchar(100) DEFAULT NULL COMMENT '条码',
  `panel_length` decimal(8,2) DEFAULT NULL COMMENT '板料长度',
  `spec_group` varchar(100) DEFAULT NULL COMMENT '工序组',
  `move_in_time` datetime DEFAULT NULL,
  `move_out_time` datetime DEFAULT NULL,
  `track_in_time` datetime DEFAULT NULL,
  `track_out_time` datetime DEFAULT NULL,
  `sign_move_in_time` datetime DEFAULT NULL,
  `sign_move_out_time` datetime DEFAULT NULL,
  `sign_track_in_time` datetime DEFAULT NULL,
  `sign_track_out_time` datetime DEFAULT NULL,
  `before_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换前文件路径',
  `after_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换后文件路径',
  `layer_num` int(11) DEFAULT NULL COMMENT '层数',
  `is_error_data` tinyint(4) DEFAULT '0' COMMENT '是否错误数据',
  `is_in_plan_warehouse` varchar(100) DEFAULT NULL COMMENT '是否在钻孔计划仓',
  `is_hold` varchar(100) DEFAULT NULL COMMENT '是否暂停',
  `is_in_plan_warehouse_remark` varchar(100) DEFAULT NULL COMMENT '是否在钻孔计划仓备注',
  `bar_code` varchar(100) DEFAULT NULL COMMENT '板料二维码',
  PRIMARY KEY (`id`),
  UNIQUE KEY `source_code_UNIQUE` (`source_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='外部工单表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_external_work_order`
--

LOCK TABLES `t_external_work_order` WRITE;
/*!40000 ALTER TABLE `t_external_work_order` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_external_work_order` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COMMENT='生产报工记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_feedback`
--

LOCK TABLES `t_feedback` WRITE;
/*!40000 ALTER TABLE `t_feedback` DISABLE KEYS */;
INSERT INTO `t_feedback` VALUES (10,'自行报工',424,'D10-2849-241','MockDrill0508jingwang',129,'MO202409125','MO202409125',24,'drill','钻孔',1237,'TO20240912000060',14,'WJTEST001','WJTEST001',4,'Panel',NULL,5.00,5.00,3.00,2.00,'超级管理员',NULL,NULL,'2024-09-12 09:58:08','','DRAFT','1',0,1,1,'2024-09-12 09:58:08','2024-09-29 16:00:53',1,1.0000);
INSERT INTO `t_feedback` VALUES (11,'自行报工',419,'D5-2849-241','MockDrill0508jingwang',135,'WJTEST001-0912006','WJTEST001-0912006',24,'drill','钻孔',1273,'TO20240912000096',23,'WJTEST0912001','WJTEST0912001',4,'Panel',NULL,5.00,4.00,2.00,2.00,'超级管理员',NULL,NULL,'2024-09-12 17:20:10','','DRAFT','1',0,1,1,'2024-09-12 17:20:08','2024-09-12 17:21:01',1,1.0000);
INSERT INTO `t_feedback` VALUES (12,'自行报工',421,'WS202405000001','拆板',135,'WJTEST001-0912006','WJTEST001-0912006',26,'unpin','拆板',1275,'TO20240918000001',23,'WJTEST0912001','WJTEST0912001',4,'Panel',NULL,123.00,4.00,0.00,4.00,'超级管理员',NULL,NULL,'2024-09-19 14:33:41','','DRAFT','0',0,1,1,'2024-09-19 14:33:33','2024-09-19 14:33:33',NULL,1.0000);
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
  `incode_number` varchar(100) DEFAULT NULL COMMENT '条码',
  `panel_length` decimal(8,2) DEFAULT NULL COMMENT '板料长度',
  `drill_file_path` varchar(500) DEFAULT NULL COMMENT '钻带文件路径',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `layer_num` int(11) DEFAULT NULL COMMENT '层数',
  `panel_count` int(11) DEFAULT '1' COMMENT '叠板层数',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`)
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
  `atp_parameters` mediumtext CHARACTER SET utf8 COMMENT 'ATP参数',
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='物料产品类别';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_item_type`
--

LOCK TABLES `t_item_type` WRITE;
/*!40000 ALTER TABLE `t_item_type` DISABLE KEYS */;
INSERT INTO `t_item_type` VALUES (1,'全部',0,'00',0,'Y','0',0,1,1,'2023-03-30 05:08:07','2023-04-03 02:31:48',1);
INSERT INTO `t_item_type` VALUES (4,'半成品',1,'02',1,'Y','0,1',0,1,1,'2023-03-30 05:10:36','2023-04-04 02:11:22',1);
INSERT INTO `t_item_type` VALUES (5,'原料',1,'01',1,NULL,'0,1',0,1,1,'2024-05-27 15:15:56','2024-05-27 15:15:56',NULL);
INSERT INTO `t_item_type` VALUES (6,'成品',1,'03',2,NULL,'0,1',0,0,1,'2024-05-27 15:16:14','2024-09-19 14:37:06',1);
INSERT INTO `t_item_type` VALUES (7,'单钻孔',1,'onlydrill',2,NULL,'0,1',0,1,1,'2024-05-27 15:16:53','2024-05-27 15:16:53',NULL);
INSERT INTO `t_item_type` VALUES (8,'铝',5,'lv',1,NULL,'0,1,5',0,1,1,'2024-05-27 15:17:14','2024-05-27 15:17:14',NULL);
INSERT INTO `t_item_type` VALUES (9,'板材',5,'panel',1,NULL,'0,1,5',0,1,1,'2024-05-27 15:17:49','2024-05-27 15:17:49',NULL);
INSERT INTO `t_item_type` VALUES (10,'铜',5,'tong',1,NULL,'0,1,5',0,1,1,'2024-05-27 15:18:01','2024-05-27 15:18:01',NULL);
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
  `notify_msg` mediumtext CHARACTER SET utf8 COMMENT '消息',
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify`
--

LOCK TABLES `t_notify` WRITE;
/*!40000 ALTER TABLE `t_notify` DISABLE KEYS */;
INSERT INTO `t_notify` VALUES (4,'2024-09-10 11:51:05','1111','2222','123少时诵诗书少时诵诗书是撒是撒是撒是撒是撒是撒是撒是撒是撒是撒是撒',2,'钉钉',0,1,1,'2024-09-10 11:51:00','2024-09-29 16:11:30',1,NULL);
INSERT INTO `t_notify` VALUES (5,'2024-10-22 14:01:29','11112','111','2222',3,'大屏告警',0,1,1,'2024-10-22 14:00:38','2024-10-22 14:00:38',NULL,NULL);
INSERT INTO `t_notify` VALUES (6,'2024-10-22 14:01:36','2121','2121','2121',NULL,'',0,1,1,'2024-10-22 14:00:45','2024-10-22 14:00:45',NULL,NULL);
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
  `notify_params` mediumtext CHARACTER SET utf8 COMMENT '参数',
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='通知类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_notify_setting`
--

LOCK TABLES `t_notify_setting` WRITE;
/*!40000 ALTER TABLE `t_notify_setting` DISABLE KEYS */;
INSERT INTO `t_notify_setting` VALUES (1,'微信',0,'WX_WARNING',NULL,NULL,NULL,1,10,0,1,1,'2024-05-28 09:05:36','2024-08-23 10:38:54',1,'0');
INSERT INTO `t_notify_setting` VALUES (2,'钉钉',0,'DING_WARNING',NULL,NULL,NULL,0,10,0,1,1,'2024-05-28 09:05:59','2024-05-28 09:07:23',1,'0');
INSERT INTO `t_notify_setting` VALUES (3,'大屏告警',0,'BIG_SCREEN_WARNING',NULL,NULL,NULL,1,5,0,1,1,'2024-05-28 09:06:32','2024-05-28 09:06:32',NULL,'0');
INSERT INTO `t_notify_setting` VALUES (4,'短信',0,'LIGHT_WARNING',NULL,NULL,NULL,0,10,0,1,1,'2024-05-28 09:07:05','2024-05-28 09:07:05',NULL,'0');
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
  `product_status` int(8) DEFAULT NULL COMMENT '产品状态',
  `pcs` int(11) DEFAULT NULL COMMENT '单叠数量',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `parent_id` bigint(20) DEFAULT '0' COMMENT '父类ID',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `panel_length` decimal(8,2) DEFAULT NULL COMMENT '板料长度',
  `location_code` varchar(100) DEFAULT NULL COMMENT '库位号',
  `silo_code` varchar(100) DEFAULT NULL COMMENT '料仓号',
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
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工序设置';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_process`
--

LOCK TABLES `t_process` WRITE;
/*!40000 ALTER TABLE `t_process` DISABLE KEYS */;
INSERT INTO `t_process` VALUES (24,'钻孔','drill','钻孔要求',0,1,1,'2023-06-25 13:07:09','2024-07-11 11:22:12',1);
INSERT INTO `t_process` VALUES (25,'叠板','pin','',0,1,1,'2024-05-27 15:22:51','2024-09-12 17:21:08',1);
INSERT INTO `t_process` VALUES (26,'拆板','unpin','',0,1,1,'2024-05-27 15:23:07','2024-09-29 15:59:09',1);
INSERT INTO `t_process` VALUES (27,'分板','split','',0,1,1,'2024-05-28 09:42:29','2024-05-28 09:42:50',1);
INSERT INTO `t_process` VALUES (30,'siloshelves','siloshelves',NULL,0,1,1,'2024-08-06 14:50:33','2024-08-06 14:50:33',NULL);
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
  `parameters` mediumtext CHARACTER SET utf8 COMMENT '参数',
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
  `dispense_machines` decimal(12,2) DEFAULT NULL COMMENT '建议机台数',
  `vetting_status` tinyint(4) DEFAULT '0' COMMENT '审批状态(0:未审批;1:已审批)',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COMMENT='产品大类';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_product_category`
--

LOCK TABLES `t_product_category` WRITE;
/*!40000 ALTER TABLE `t_product_category` DISABLE KEYS */;
INSERT INTO `t_product_category` VALUES (1,0,'P01','单面版','默认产品大类 ',0,1,1,'2023-09-18 14:36:44','2024-09-29 15:56:52',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (2,0,'P02','双面板','',0,1,1,'2024-05-27 15:08:05','2024-09-12 17:22:06',1,'0',4.00,1);
INSERT INTO `t_product_category` VALUES (3,0,'P03','多层板','',0,1,1,'2024-05-27 15:08:23','2024-05-28 09:20:51',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (4,0,'P04','刚性电路板','',0,1,1,'2024-05-27 15:08:45','2024-05-28 09:20:55',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (5,0,'P05','柔性电路板','',0,1,1,'2024-05-27 15:13:09','2024-05-28 09:20:55',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (6,0,'P06','刚柔结合电路板','',0,1,1,'2024-05-27 15:13:30','2024-05-28 09:20:55',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (7,0,'P07','高速电路板','',0,1,1,'2024-05-27 15:13:54','2024-05-28 09:20:55',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (8,0,'P08','高频电路板','',0,1,1,'2024-05-27 15:14:05','2024-05-28 09:20:55',1,'0',2.00,1);
INSERT INTO `t_product_category` VALUES (9,0,'P08-01','高速电路板---客户001指定','',0,1,1,'2024-05-27 15:14:44','2024-09-18 14:52:54',1,'0',2.00,1);
/*!40000 ALTER TABLE `t_product_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_rack`
--

DROP TABLE IF EXISTS `t_rack`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_rack` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) DEFAULT NULL COMMENT '料架编号',
  `warehouse_code` varchar(50) DEFAULT NULL COMMENT '所属仓库编号',
  `warehouse_id` int(8) DEFAULT NULL COMMENT '所属仓库Id',
  `is_have_silo` tinyint(2) DEFAULT NULL COMMENT '是否关联料仓',
  `silo_code` varchar(50) DEFAULT NULL COMMENT '料仓编号',
  `inner_point` varchar(255) DEFAULT NULL COMMENT '内点',
  `out_point` varchar(255) DEFAULT NULL COMMENT '外点',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `trans_inner_point` varchar(255) DEFAULT NULL COMMENT '小车内点',
  `trans_out_point` varchar(255) DEFAULT NULL COMMENT '小车外点',
  `device_kind` int(11) DEFAULT NULL COMMENT '设备类别',
  `relate_device_code` varchar(200) DEFAULT NULL COMMENT '关联的设备code',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='料架基本信息表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_rack`
--

LOCK TABLES `t_rack` WRITE;
/*!40000 ALTER TABLE `t_rack` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_rack` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route`
--

LOCK TABLES `t_route` WRITE;
/*!40000 ALTER TABLE `t_route` DISABLE KEYS */;
INSERT INTO `t_route` VALUES (3,'A0001','工艺路线A0001','钻孔00','',1,0,1,'2023-04-05 01:57:50','2024-09-29 15:59:44',1,1);
INSERT INTO `t_route` VALUES (4,'B0002','工艺路线B0002','','',1,0,59,'2024-05-23 11:38:26','2024-05-28 09:41:58',1,1);
INSERT INTO `t_route` VALUES (5,'C0003','工艺路线C0003','','',1,0,1,'2024-05-27 15:24:10','2024-05-28 09:47:18',1,1);
INSERT INTO `t_route` VALUES (7,'Test001','Test001','','',1,0,1,'2024-06-26 09:04:33','2024-06-28 10:36:55',1,1);
INSERT INTO `t_route` VALUES (17,'Test002','Test002','','',1,0,1,'2024-10-18 15:12:21','2024-10-18 15:18:06',1,1);
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
) ENGINE=InnoDB AUTO_INCREMENT=96 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与工序关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_process`
--

LOCK TABLES `t_route_and_process` WRITE;
/*!40000 ALTER TABLE `t_route_and_process` DISABLE KEYS */;
INSERT INTO `t_route_and_process` VALUES (44,3,24,2,'1','#75AF25',40,'0',1,1,0,50,'2023-07-06 14:24:13','2023-12-20 22:23:02',1);
INSERT INTO `t_route_and_process` VALUES (47,2,24,2,'1','#1E96AB',60,'0',0,1,0,50,'2023-07-06 16:32:21','2023-12-20 22:23:23',1);
INSERT INTO `t_route_and_process` VALUES (52,2,21,1,'0','#F24040',40,'0',0,1,0,50,'2023-07-06 16:35:45','2023-10-09 10:37:13',33);
INSERT INTO `t_route_and_process` VALUES (53,2,23,3,'0','#550B0B',60,'1',0,1,0,50,'2023-07-06 16:35:58','2023-10-09 10:38:03',33);
INSERT INTO `t_route_and_process` VALUES (54,46,21,2,'1','#2A1616',1232,'1',1,1,0,1,'2023-07-10 09:26:34','2024-02-27 11:13:00',59);
INSERT INTO `t_route_and_process` VALUES (67,3,21,1,'0','#411111',34,'0',1,1,0,1,'2023-10-13 15:18:09','2024-01-05 02:34:43',1);
INSERT INTO `t_route_and_process` VALUES (68,3,23,3,'0','#A53434',40,'0',1,1,0,1,'2023-11-27 16:06:14','2023-12-27 01:21:10',1);
INSERT INTO `t_route_and_process` VALUES (74,46,23,1,'0','#460A0A',50,'0',0,1,0,59,'2024-02-27 11:11:30','2024-02-27 11:12:52',59);
INSERT INTO `t_route_and_process` VALUES (75,46,28,0,'0','#052571',50,'0',2,1,0,59,'2024-02-27 11:12:06','2024-02-27 11:12:40',59);
INSERT INTO `t_route_and_process` VALUES (76,50,24,1,'1','#26FF00',25,'0',0,1,0,1,'2024-03-14 09:32:01','2024-03-14 09:34:47',1);
INSERT INTO `t_route_and_process` VALUES (77,50,23,2,'0','#41E026',15,'0',0,1,0,1,'2024-03-14 09:32:59','2024-03-14 09:39:01',1);
INSERT INTO `t_route_and_process` VALUES (78,4,24,1,'1','#8D2F2F',40,'0',0,1,0,59,'2024-05-23 11:38:44',NULL,NULL);
INSERT INTO `t_route_and_process` VALUES (79,4,26,2,'0','#421313',23,'0',0,1,0,1,'2024-05-27 15:24:54','2024-05-27 15:24:54',NULL);
INSERT INTO `t_route_and_process` VALUES (80,5,24,1,'1','#D11B95',44,'1',0,1,0,1,'2024-05-27 15:25:23','2024-05-27 15:25:23',NULL);
INSERT INTO `t_route_and_process` VALUES (82,5,26,2,'0','#823131',80,'0',1,1,0,1,'2024-05-28 09:22:21','2024-05-28 09:22:21',NULL);
INSERT INTO `t_route_and_process` VALUES (83,5,27,3,'0','#884747',23,'1',0,1,0,1,'2024-05-28 09:43:27','2024-05-28 09:43:27',NULL);
INSERT INTO `t_route_and_process` VALUES (85,7,24,1,'1','#EB0D0D',50,'0',1,1,0,1,'2024-06-26 09:05:05','2024-06-26 09:05:05',NULL);
INSERT INTO `t_route_and_process` VALUES (89,3,27,3,'0','#230B0B',24,'1',1,1,0,1,'2024-08-30 11:09:21','2024-08-30 11:09:21',NULL);
INSERT INTO `t_route_and_process` VALUES (94,3,25,1,'0','#9C8181',20,'0',0,1,0,1,'2024-09-18 15:21:35','2024-09-18 15:21:35',NULL);
INSERT INTO `t_route_and_process` VALUES (95,17,24,1,'1','#13C616',40,'0',0,1,0,1,'2024-10-18 15:12:46','2024-10-18 15:12:46',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=1261 DEFAULT CHARSET=utf8mb4 COMMENT='工艺路线与产品大类关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_and_product_category`
--

LOCK TABLES `t_route_and_product_category` WRITE;
/*!40000 ALTER TABLE `t_route_and_product_category` DISABLE KEYS */;
INSERT INTO `t_route_and_product_category` VALUES (254,21,77,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (255,21,78,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (256,21,79,NULL,1,0,1,'2023-07-10 09:26:47',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (395,2,84,2,1,0,1,'2023-09-12 13:28:29',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (443,2,72,4,1,0,1,'2023-09-18 09:34:00',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (461,30,87,1,1,0,33,'2023-09-20 16:07:39',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (886,2,97,2,1,0,1,'2023-10-11 15:44:54',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (996,2,74,5,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (997,2,82,9,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (998,2,75,6,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (999,2,76,5,1,0,1,'2023-11-17 10:45:26',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1011,2,81,2,1,0,1,'2024-01-04 22:47:15',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1012,46,74,7,1,0,59,'2024-02-27 11:09:20',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1013,46,76,7,1,0,59,'2024-02-27 11:09:20',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1014,46,77,2,1,0,59,'2024-02-27 11:09:20',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1030,2,86,2,1,0,1,'2024-03-13 09:21:52',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1031,46,86,3,1,0,1,'2024-03-13 09:21:52',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1038,50,81,4,1,0,1,'2024-03-14 09:36:27',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1039,50,75,8,1,0,1,'2024-03-14 09:36:27',NULL,NULL);
INSERT INTO `t_route_and_product_category` VALUES (1043,5,3,0,1,0,1,'2024-05-27 15:26:43','2024-05-27 15:26:43',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1045,5,5,0,1,0,1,'2024-05-27 15:26:43','2024-05-27 15:26:43',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1046,5,6,0,1,0,1,'2024-05-27 15:26:43','2024-05-27 15:26:43',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1047,5,7,0,1,0,1,'2024-05-27 15:26:43','2024-05-27 15:26:43',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1048,5,8,0,1,0,1,'2024-05-27 15:26:43','2024-05-27 15:26:43',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1063,4,10,1,1,0,1,'2024-07-11 11:00:20','2024-07-12 10:07:22',59);
INSERT INTO `t_route_and_product_category` VALUES (1064,5,10,3,1,0,1,'2024-07-11 11:00:20','2024-07-11 11:00:20',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1066,7,10,5,1,0,1,'2024-07-11 11:00:20','2024-07-11 11:00:20',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1076,5,4,2,1,0,1,'2024-07-29 15:30:59','2024-07-29 15:30:59',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1077,4,4,3,1,0,1,'2024-07-29 15:30:59','2024-07-29 15:30:59',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1133,5,2,1,1,0,1,'2024-08-22 10:45:05','2024-08-22 10:45:05',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1134,4,2,2,1,0,1,'2024-08-22 10:45:05','2024-08-22 10:45:05',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1162,7,11,3,1,0,1,'2024-08-23 08:54:58','2024-08-23 08:54:58',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1171,7,12,4,1,0,1,'2024-08-30 09:40:55','2024-08-30 09:41:15',1);
INSERT INTO `t_route_and_product_category` VALUES (1172,5,12,5,1,0,1,'2024-08-30 09:40:55','2024-08-30 09:41:12',1);
INSERT INTO `t_route_and_product_category` VALUES (1173,4,12,3,1,0,1,'2024-08-30 09:40:55','2024-08-30 09:41:15',1);
INSERT INTO `t_route_and_product_category` VALUES (1183,4,13,2,1,0,1,'2024-09-03 09:05:25','2024-09-03 09:05:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1184,5,13,3,1,0,1,'2024-09-03 09:05:25','2024-09-03 09:05:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1185,7,13,4,1,0,1,'2024-09-03 09:05:25','2024-09-03 09:05:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1227,4,1,2,1,0,1,'2024-09-04 14:15:26','2024-09-04 14:15:26',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1235,4,15,2,1,0,1,'2024-09-09 09:27:34','2024-09-09 09:27:34',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1245,5,9,1,1,0,1,'2024-09-18 14:52:48','2024-09-18 14:52:48',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1247,4,9,3,1,0,1,'2024-09-18 14:52:48','2024-09-18 14:52:48',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1248,7,9,4,1,0,1,'2024-09-18 14:52:48','2024-09-18 14:52:48',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1249,3,4,6,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1250,3,5,3,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1251,3,6,3,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1252,3,7,3,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1253,3,8,3,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1254,3,9,5,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1255,3,1,4,1,0,1,'2024-09-18 15:18:45','2024-09-18 15:18:45',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1256,3,20,1,1,0,1,'2024-09-23 11:25:25','2024-09-23 11:25:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1257,4,20,2,1,0,1,'2024-09-23 11:25:25','2024-09-23 11:25:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1258,5,20,3,1,0,1,'2024-09-23 11:25:25','2024-09-23 11:25:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1259,7,20,4,1,0,1,'2024-09-23 11:25:25','2024-09-23 11:25:25',NULL);
INSERT INTO `t_route_and_product_category` VALUES (1260,17,1,5,1,0,1,'2024-10-18 15:18:02','2024-10-18 15:18:02',NULL);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工序与工作站关系表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_route_process_and_work_station`
--

LOCK TABLES `t_route_process_and_work_station` WRITE;
/*!40000 ALTER TABLE `t_route_process_and_work_station` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_route_process_and_work_station` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_schedule`
--

DROP TABLE IF EXISTS `t_schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_schedule` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(500) CHARACTER SET utf8 NOT NULL COMMENT '任务编号',
  `source_device_id` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `require_device_id` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `task_id` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `start_location` mediumtext CHARACTER SET utf8 COMMENT '起点',
  `end_location` mediumtext CHARACTER SET utf8 COMMENT '终点',
  `priority` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '优先级',
  `request_json` mediumtext CHARACTER SET utf8 COMMENT '请求详情',
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
  `warnning_message` mediumtext CHARACTER SET utf8 COMMENT '消息',
  `request_interaction_behavior` varchar(50) DEFAULT NULL COMMENT '交互方式编码',
  `request_interaction_behavior_name` varchar(200) DEFAULT NULL COMMENT '交互方式描述',
  `total_raw_count` int(10) DEFAULT NULL COMMENT '累计已上生料',
  `is_urgent` tinyint(1) DEFAULT '0' COMMENT '是否紧急',
  `is_all_panel_sent` tinyint(1) DEFAULT '1' COMMENT '是否已全部送料',
  `route_id` bigint(20) DEFAULT NULL COMMENT '工艺路线ID',
  `route_code` varchar(64) DEFAULT NULL COMMENT '工艺路线编码',
  `route_name` varchar(255) DEFAULT NULL COMMENT '工艺路线名称',
  `master_schedule_id` bigint(20) DEFAULT NULL COMMENT '关联的主叫调度记录ID',
  `is_auxiliary` tinyint(1) DEFAULT '0' COMMENT '是否是辅助设备请求',
  `interaction_sequence` int(11) DEFAULT NULL COMMENT '呼叫序列',
  `request_device_kind` int(11) DEFAULT NULL COMMENT '呼叫设备kind',
  `is_master` tinyint(1) DEFAULT NULL COMMENT '是否为先执行',
  `sub_device_code` varchar(50) DEFAULT NULL COMMENT '库位编号',
  `agv_payload_panels` mediumtext COMMENT 'AGV板料信息',
  `request_summary_info` mediumtext COMMENT '请求的汇总简略信息',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`),
  KEY `idx_scheduled_task_status_of_t_schedule` (`scheduled_task_status`),
  KEY `idx_source_device_id_of_t_schedule` (`source_device_id`),
  KEY `idx_sub_device_code_of_t_schedule` (`sub_device_code`),
  KEY `idx_request_device_kind_of_t_schedule` (`request_device_kind`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_schedule`
--

LOCK TABLES `t_schedule` WRITE;
/*!40000 ALTER TABLE `t_schedule` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_schedule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_schedule_log`
--

DROP TABLE IF EXISTS `t_schedule_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_schedule_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `master_id` bigint(20) NOT NULL COMMENT '主表Id',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `message` mediumtext CHARACTER SET utf8 COMMENT '调度记录明细',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录明细';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_schedule_log`
--

LOCK TABLES `t_schedule_log` WRITE;
/*!40000 ALTER TABLE `t_schedule_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_schedule_log` ENABLE KEYS */;
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
-- Table structure for table `t_silo`
--

DROP TABLE IF EXISTS `t_silo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_silo` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) DEFAULT NULL COMMENT '料仓编码',
  `size` varchar(20) DEFAULT NULL COMMENT '运载尺寸',
  `floor_count` int(5) DEFAULT NULL COMMENT '层数',
  `location` varchar(50) DEFAULT NULL COMMENT '位置',
  `empty_silo` mediumtext CHARACTER SET utf8 COMMENT '空料仓',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `related_device_kind` int(11) DEFAULT NULL COMMENT '相关设备类别',
  `silo_status` int(11) DEFAULT NULL COMMENT '料仓状态',
  `vendor_id` bigint(20) DEFAULT NULL COMMENT '供应商ID',
  `vendor_code` varchar(100) DEFAULT NULL COMMENT '供应商编码',
  `vendor_name` varchar(255) DEFAULT NULL COMMENT '供应商名称',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='料仓基本信息表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_silo`
--

LOCK TABLES `t_silo` WRITE;
/*!40000 ALTER TABLE `t_silo` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_silo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_silo_detail`
--

DROP TABLE IF EXISTS `t_silo_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_silo_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `silo_code` varchar(50) DEFAULT NULL COMMENT '料仓编号',
  `floor_num` int(11) DEFAULT NULL COMMENT '层号',
  `item_code` varchar(50) DEFAULT NULL COMMENT '物料编号',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料编码',
  `product_status` int(8) DEFAULT NULL COMMENT '该层板料类型:（0-空仓, 30100-生料,40100-熟料）',
  `pcs` int(8) DEFAULT NULL COMMENT '每叠片数',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='料仓载料明细表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_silo_detail`
--

LOCK TABLES `t_silo_detail` WRITE;
/*!40000 ALTER TABLE `t_silo_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_silo_detail` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='点检保养项目';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_subject`
--

LOCK TABLES `t_subject` WRITE;
/*!40000 ALTER TABLE `t_subject` DISABLE KEYS */;
INSERT INTO `t_subject` VALUES (6,'总进气压',0,'1','点检,保养',' ','(6-7)kg/cm²','0',0,1,1,'2023-06-21 15:55:08','2024-09-29 16:09:08',1);
INSERT INTO `t_subject` VALUES (7,'集尘负压',0,'2','点检',NULL,'(1500±500)mH20','0',0,1,1,'2023-06-21 16:01:23','2023-06-21 16:01:58',1);
INSERT INTO `t_subject` VALUES (8,'冰水温度',0,'3','点检,保养',NULL,'(20±2)°c','0',0,1,1,'2023-06-21 16:02:56','2024-06-18 15:21:26',1);
INSERT INTO `t_subject` VALUES (9,'吸屑罩气压',0,'4','点检',NULL,'(0.25-0.3)Mpa','0',0,1,1,'2023-06-21 16:03:44',NULL,NULL);
INSERT INTO `t_subject` VALUES (10,'主轴压头气压',0,'5','点检',NULL,'(0.58-0.65)Mpa','0',0,1,1,'2023-06-21 16:04:59',NULL,NULL);
INSERT INTO `t_subject` VALUES (11,'主轴气压',0,'6','点检',NULL,'(0.55-0.65)Mpa','0',0,1,1,'2023-06-21 16:05:24','2024-09-18 16:22:19',1);
INSERT INTO `t_subject` VALUES (15,'123',0,'123','点检','','123','0',0,1,1,'2024-09-12 17:13:50','2024-09-12 17:22:50',1);
INSERT INTO `t_subject` VALUES (16,'11',0,'11','保养','','111','0',0,1,1,'2024-10-18 16:32:37','2024-10-18 16:32:37',NULL);
INSERT INTO `t_subject` VALUES (17,'前期去去去',0,'亲亲','保养','','凄凄切切','0',0,1,1,'2024-10-22 10:15:06','2024-10-22 10:15:06',NULL);
INSERT INTO `t_subject` VALUES (18,'111',0,'111','保养','1','1','0',0,1,1,'2024-10-22 11:01:38','2024-10-22 11:01:38',NULL);
INSERT INTO `t_subject` VALUES (19,'111',0,'1112','点检','111','111','0',0,1,1,'2024-10-22 11:02:02','2024-10-22 11:02:02',NULL);
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
  `is_urgent` tinyint(4) DEFAULT '0' COMMENT '是否紧急',
  `incode_number` varchar(100) DEFAULT NULL COMMENT '条码',
  `spec_group` varchar(100) DEFAULT NULL COMMENT '工序组',
  `before_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换前文件路径',
  `after_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换后文件路径',
  `is_rebrush` tinyint(4) DEFAULT '0' COMMENT '是否需要重新刷数据',
  `layer_num` int(11) DEFAULT NULL COMMENT '层数',
  `bar_code` varchar(100) DEFAULT NULL COMMENT '板料二维码',
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
-- Table structure for table `t_transportation_task`
--

DROP TABLE IF EXISTS `t_transportation_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_transportation_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) NOT NULL COMMENT '编号',
  `is_urgent` tinyint(4) DEFAULT '0' COMMENT '是否紧急',
  `interaction_sequence` int(8) DEFAULT NULL COMMENT '交互序列',
  `scheduled_task_status` int(11) DEFAULT NULL COMMENT '调度任务状态',
  `transportation_kind` int(8) DEFAULT NULL COMMENT '类型：空仓、生料、熟料、首件',
  `internal_lot_no` varchar(50) DEFAULT NULL COMMENT '内部编号',
  `external_lot_no` varchar(50) DEFAULT NULL COMMENT '外部编号',
  `warehouse_code` varchar(50) DEFAULT NULL COMMENT '库房，库位分区编号',
  `fork_code` varchar(50) DEFAULT NULL COMMENT '料架编号',
  `related_drill_trace` varchar(255) DEFAULT NULL COMMENT '相关钻机追溯',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `allocate_time` datetime DEFAULT NULL COMMENT '分配时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `clinker_count` int(11) DEFAULT NULL COMMENT '熟料数量',
  `raw_ount` int(11) DEFAULT NULL COMMENT '生料数量',
  `silo_code` varchar(100) DEFAULT NULL COMMENT '料仓号',
  `related_route` varchar(100) DEFAULT NULL,
  `related_schedule_id` bigint(20) DEFAULT NULL,
  `relate_device_code` varchar(100) DEFAULT NULL,
  `other_fork_code` varchar(100) DEFAULT NULL,
  `job_id` bigint(20) DEFAULT NULL,
  `plan_id` bigint(20) DEFAULT NULL,
  `hk_response` mediumtext COMMENT 'hk_response',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='料仓任务表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_transportation_task`
--

LOCK TABLES `t_transportation_task` WRITE;
/*!40000 ALTER TABLE `t_transportation_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_transportation_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_transportation_task_log`
--

DROP TABLE IF EXISTS `t_transportation_task_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_transportation_task_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `master_id` bigint(20) NOT NULL COMMENT '主表Id',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `message` mediumtext CHARACTER SET utf8 COMMENT '料仓任务明细',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='料仓任务日志表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_transportation_task_log`
--

LOCK TABLES `t_transportation_task_log` WRITE;
/*!40000 ALTER TABLE `t_transportation_task_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_transportation_task_log` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COMMENT='单位表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_unit_measure`
--

LOCK TABLES `t_unit_measure` WRITE;
/*!40000 ALTER TABLE `t_unit_measure` DISABLE KEYS */;
INSERT INTO `t_unit_measure` VALUES (1,'Panel','单层Panel','Y',34,1.0000,'默认单位',0,1,1,'2024-03-13 09:41:14','2024-07-11 10:46:44',1);
INSERT INTO `t_unit_measure` VALUES (3,'JL05000002DW','克','Y',0,1.0000,'',0,1,1,'2024-05-28 09:50:09','2024-09-30 14:41:34',1);
INSERT INTO `t_unit_measure` VALUES (4,'JL05000003DW','千克','N',3,1000.0000,'',0,1,1,'2024-05-28 09:50:14','2024-09-19 13:15:36',1);
INSERT INTO `t_unit_measure` VALUES (5,'JL05000004DW','公斤','N',3,1000.0000,'',0,1,1,'2024-05-28 09:50:32','2024-08-20 15:45:36',1);
INSERT INTO `t_unit_measure` VALUES (6,'JL05000005DW','吨','N',3,1000000.0000,'',0,1,1,'2024-05-28 09:50:38','2024-08-20 15:46:12',1);
INSERT INTO `t_unit_measure` VALUES (7,'JL0820000001DW','毫克','N',3,0.0010,'',0,1,1,'2024-08-20 15:25:59','2024-08-20 15:38:47',1);
INSERT INTO `t_unit_measure` VALUES (15,'JL20240918000001DW','双层Panel','N',1,2.0000,'',0,1,1,'2024-09-18 14:50:59','2024-09-18 14:50:59',NULL);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='供应商表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_vendor`
--

LOCK TABLES `t_vendor` WRITE;
/*!40000 ALTER TABLE `t_vendor` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_vendor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_warehouse`
--

DROP TABLE IF EXISTS `t_warehouse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_warehouse` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '分区ID',
  `code` varchar(255) NOT NULL COMMENT '分区编码',
  `name` varchar(255) NOT NULL COMMENT '分区名称',
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
  `parent_id` bigint(20) NOT NULL,
  `ancestors` varchar(255) DEFAULT NULL,
  `pre_book_agv` varchar(255) DEFAULT NULL COMMENT '预约AGV',
  `pre_book_time` datetime DEFAULT NULL COMMENT '预约时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COMMENT='分区表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_warehouse`
--

LOCK TABLES `t_warehouse` WRITE;
/*!40000 ALTER TABLE `t_warehouse` DISABLE KEYS */;
INSERT INTO `t_warehouse` VALUES (1,'000','全部',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 10:49:44',1,NULL,NULL,0,'0',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (2,'PartPanelShelf','线边仓',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-23 10:39:28',1,NULL,NULL,1,'0,1',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (3,'PartPin','上Pin',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 11:20:28',1,NULL,NULL,1,'0,1',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (4,'PartUnpin','下Pin',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-10-12 14:23:30',59,NULL,NULL,1,'0,1',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (5,'PartPanelFork','中转区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-09-29 16:14:29',1,NULL,NULL,1,'0,1',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (6,'PartPanelShelf-Raw','生料区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 09:42:17',NULL,NULL,NULL,2,'0,1,2',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (7,'PartPanelShelf-Clinker','熟料区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 09:42:17',NULL,NULL,NULL,2,'0,1,2',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (8,'PartPin-001','上Pin一区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 09:42:17',NULL,NULL,NULL,3,'0,1,3',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (9,'PartPin-002','上Pin二区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 09:42:17',NULL,NULL,NULL,3,'0,1,3',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (10,'PartUnpin-001','下Pin一区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-08 09:42:17',NULL,NULL,NULL,4,'0,1,4',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (11,'PartUnpin-002','下Pin二区',NULL,NULL,'',0,0,1,'2024-08-08 09:42:17','2024-09-19 15:27:33',1,NULL,NULL,4,'0,1,4',NULL,NULL);
INSERT INTO `t_warehouse` VALUES (12,'PartPanelFork-Raw','生料中转区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-10-12 14:23:21',59,NULL,NULL,5,'0,1,5','',NULL);
INSERT INTO `t_warehouse` VALUES (13,'PartPanelFork-Clinker','熟料中转区',NULL,NULL,'',0,1,1,'2024-08-08 09:42:17','2024-08-09 14:26:18',1,NULL,NULL,5,'0,1,5',NULL,NULL);
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
  `is_urgent` tinyint(4) DEFAULT '0' COMMENT '是否紧急',
  `remark_color` varchar(7) DEFAULT NULL,
  `incode_number` varchar(100) DEFAULT NULL COMMENT '条码',
  `move_in_time` datetime DEFAULT NULL,
  `move_out_time` datetime DEFAULT NULL,
  `track_in_time` datetime DEFAULT NULL,
  `track_out_time` datetime DEFAULT NULL,
  `spec_group` varchar(100) DEFAULT NULL COMMENT '工序组',
  `before_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换前文件路径',
  `after_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换后文件路径',
  `is_rebrush` tinyint(4) DEFAULT '0' COMMENT '是否需要重新刷数据',
  `layer_num` int(11) DEFAULT NULL COMMENT '层数',
  `is_external` tinyint(4) DEFAULT '0' COMMENT '是否外部工单导入',
  `bar_code` varchar(100) DEFAULT NULL COMMENT '板料二维码',
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
-- Table structure for table `t_work_order_alter_log`
--

DROP TABLE IF EXISTS `t_work_order_alter_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_work_order_alter_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `work_order_code` varchar(200) DEFAULT NULL COMMENT '工单编码',
  `device_code` varchar(200) DEFAULT NULL COMMENT '设备编码',
  `item_code` varchar(200) DEFAULT NULL COMMENT '物料编码',
  `action_time` datetime DEFAULT NULL COMMENT '操作时间',
  `action_detail` varchar(500) DEFAULT NULL COMMENT '具体操作',
  `before_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换前文件路径',
  `after_drill_file_path` varchar(500) DEFAULT NULL COMMENT '转换后文件路径',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工单变更记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_work_order_alter_log`
--

LOCK TABLES `t_work_order_alter_log` WRITE;
/*!40000 ALTER TABLE `t_work_order_alter_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_work_order_alter_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `t_workorder_and_panel`
--

DROP TABLE IF EXISTS `t_workorder_and_panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `t_workorder_and_panel` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `work_order_code` varchar(50) NOT NULL COMMENT '工单编码',
  `item_code` varchar(50) NOT NULL COMMENT '物料编码',
  `panel_code` varchar(50) NOT NULL COMMENT '板材编码',
  `pcs` int(11) NOT NULL DEFAULT '1' COMMENT '片数',
  `batch_Code` varchar(50) DEFAULT NULL COMMENT '批次号',
  `product_status` int(8) DEFAULT NULL COMMENT '产品状态',
  `board_Location` varchar(50) DEFAULT NULL COMMENT '板料位置',
  `station_id` bigint(20) DEFAULT NULL COMMENT '工位ID',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `task_code` varchar(50) NOT NULL COMMENT '任务号',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `external_worker_order` varchar(50) DEFAULT NULL COMMENT '外部工单号',
  `panel_length` decimal(8,2) DEFAULT NULL COMMENT '板料长度',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `panel_code` (`panel_code`),
  KEY `task_code` (`task_code`),
  KEY `external_worker_order` (`external_worker_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工单板材';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workorder_and_panel`
--

LOCK TABLES `t_workorder_and_panel` WRITE;
/*!40000 ALTER TABLE `t_workorder_and_panel` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_workorder_and_panel` ENABLE KEYS */;
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
  `work_order_id` int(11) NOT NULL COMMENT '工单ID',
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='车间表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workshop`
--

LOCK TABLES `t_workshop` WRITE;
/*!40000 ALTER TABLE `t_workshop` DISABLE KEYS */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工作站表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_workstation`
--

LOCK TABLES `t_workstation` WRITE;
/*!40000 ALTER TABLE `t_workstation` DISABLE KEYS */;
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

-- Dump completed on 2024-11-22 15:17:00
