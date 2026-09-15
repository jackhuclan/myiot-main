# db

``` sql
DROP TABLE IF EXISTS `sysappsecret`;
CREATE TABLE `sysappsecret`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '应用Id',
  `AppSecret` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '应用密钥',
  `AppCode` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '应用Code(唯一值)',
  `AppName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '应用名',
  `IsDeleted` tinyint NOT NULL DEFAULT 0 COMMENT '否已删除',
  `Status` int NOT NULL DEFAULT 1 COMMENT '状态(1:启用;0禁用)',
  `CreatorId` int NULL DEFAULT NULL COMMENT '创建人Id',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `ModifyTime` datetime NULL DEFAULT NULL COMMENT '修改时间',
  `ModifierId` int NULL DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

```