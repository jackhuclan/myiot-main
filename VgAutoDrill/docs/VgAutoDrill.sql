/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50740
Source Host           : localhost:3306
Source Database       : vega_dnc

Target Server Type    : MYSQL
Target Server Version : 50740
File Encoding         : 65001

Date: 2022-12-13 09:45:44
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for t_dnc_device
-- ----------------------------
DROP TABLE IF EXISTS `t_dnc_device`;
CREATE TABLE `t_dnc_device` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_name` varchar(200) DEFAULT NULL,
  `status` int(11) DEFAULT NULL,
  `x` int(11) DEFAULT NULL,
  `y` int(11) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for t_dnc_lot
-- ----------------------------
DROP TABLE IF EXISTS `t_dnc_lot`;
CREATE TABLE `t_dnc_lot` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `lot_no` varchar(100) DEFAULT NULL,
  `device_id` bigint(11) DEFAULT NULL,
  `shelf_id` bigint(11) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for t_dnc_lot_detail
-- ----------------------------
DROP TABLE IF EXISTS `t_dnc_lot_detail`;
CREATE TABLE `t_dnc_lot_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `lot_no` varchar(100) DEFAULT NULL,
  `item_no` varchar(100) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for t_dnc_shelf
-- ----------------------------
DROP TABLE IF EXISTS `t_dnc_shelf`;
CREATE TABLE `t_dnc_shelf` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` bigint(20) DEFAULT NULL,
  `shelf_no` varchar(200) DEFAULT NULL,
  `status` int(11) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
