-- 0612
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `request_json` VARCHAR(5000) CHARACTER SET 'utf8' NULL COMMENT 'request json' AFTER `priority`,
ADD COLUMN `need_republish` TINYINT(4) NULL DEFAULT '0' COMMENT '是否需要再次发布任务' AFTER `request_json`;

ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `real_start_time` DATETIME NULL COMMENT '实际开始时间' AFTER `ancestors`,
ADD COLUMN `real_end_time` DATETIME NULL COMMENT '实际结束时间' AFTER `real_start_time`,
ADD COLUMN `real_duration` INT(11) NULL COMMENT '实际耗时' AFTER `real_end_time`,
ADD COLUMN `is_started` TINYINT(4) NULL DEFAULT 0 AFTER `real_duration`;

ALTER TABLE `vg_autodrill_db`.`t_drill_work_order` 
COMMENT = '钻孔工单表' ;


-- 0615
ALTER TABLE `vg_autodrill_db`.`t_panel` 
CHANGE COLUMN `finish_status` `product_status` VARCHAR(50) CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '产品状态' ;


-- 0620
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `panel_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '叠板数' AFTER `add_work_order_time`,
ADD COLUMN `wad_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '叠数' AFTER `panel_count`,
ADD COLUMN `drill_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '孔数' AFTER `wad_count`;

ALTER TABLE `vg_autodrill_db`.`t_route_and_product_category` 
CHANGE COLUMN `product_category_id` `product_category_ids` VARCHAR(255) NOT NULL COMMENT '产品大类ID' ;

ALTER TABLE `vg_autodrill_db`.`t_route_and_product_category` 
ADD UNIQUE INDEX `index_unique` (`route_id` ASC, `product_category_ids` ASC);

-- 0630
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `scheduled_task_status` INT(11) NULL DEFAULT 0 COMMENT '调度任务状态' AFTER `modifier_id`;

ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `allocate_time` DATETIME NULL DEFAULT NULL COMMENT '分配时间' AFTER `scheduled_task_status`,
ADD COLUMN `running_time` DATETIME NULL DEFAULT NULL COMMENT '开始调度时间' AFTER `allocate_time`,
ADD COLUMN `completed_time` DATETIME NULL DEFAULT NULL COMMENT '调度完成时间' AFTER `running_time`,
ADD COLUMN `failed_time` DATETIME NULL DEFAULT NULL COMMENT '执行失败时间' AFTER `completed_time`,
ADD COLUMN `canceled_time` DATETIME NULL DEFAULT NULL COMMENT '取消计划时间' AFTER `failed_time`;


-- 0704
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `routing_key` VARCHAR(100) NULL COMMENT '主叫设备的route key' AFTER `canceled_time`;

ALTER TABLE `vg_autodrill_db`.`t_device` 
ADD COLUMN `device_type_code` VARCHAR(50) NULL COMMENT '设备类型代码' AFTER `device_status`;



-- 0705
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
CHANGE COLUMN `panel_count` `panel_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '叠板层数' ;



-- 0706
ALTER TABLE `vg_autodrill_db`.`t_route_and_product_category` 
ADD COLUMN `order_num` INT(11) NULL DEFAULT NULL COMMENT '序号' AFTER `product_category_id`;

ALTER TABLE `vg_autodrill_db`.`t_task` 
DROP COLUMN `quantity_changed`;



-- 0710
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `remark` NVARCHAR(200) NULL COMMENT '备注' AFTER `routing_key`;

ALTER TABLE `vg_autodrill_db`.`t_feedback` 
CHANGE COLUMN `quantity_qualified` `quantity_qualified` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '良品数量' ;


ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `now_wad_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '本次排产叠数' AFTER `is_started`;


-- 0712

ALTER TABLE `vg_autodrill_db`.`t_drill_work_order` 
ADD COLUMN `scheduled_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '已排趟数' AFTER `drill_count`,
ADD COLUMN `usable_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '本次可用叠数' AFTER `scheduled_count`,
CHANGE COLUMN `panel_count` `panel_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '叠板层数' ,
CHANGE COLUMN `wad_count` `wad_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '计划叠数' ,
CHANGE COLUMN `remainder_passes_count` `remainder_passes_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '本次要排趟数' ;



-- 0714
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `route_id` BIGINT(20) NULL COMMENT '工艺路线ID' AFTER `ancestors`,
ADD COLUMN `route_code` VARCHAR(64) NULL COMMENT '工艺路线编码' AFTER `route_id`,
CHANGE COLUMN `id` `id` BIGINT(20) NOT NULL ;

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
CHANGE COLUMN `id` `id` BIGINT(20) NOT NULL AUTO_INCREMENT ;

ALTER TABLE `vg_autodrill_db`.`t_route` 
ADD UNIQUE INDEX `index_unique` (`code` ASC);
;


-- 0717
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `changed_spindles` VARCHAR(200) NULL COMMENT '变化后的Spindles，如null,null,00003,null,null' AFTER `remark`,
ADD COLUMN `changed_behavior` VARCHAR(200) NULL COMMENT '调整后的上下料行为，如-1,0,0,-1,0' AFTER `changed_spindles`;



-- 0718
ALTER TABLE `vg_autodrill_db`.`t_alarm` 
CHANGE COLUMN `alarm_code` `alarm_code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '告警编号' ,
ADD UNIQUE INDEX `alarm_code_UNIQUE` (`alarm_code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_alarm_setting` 
CHANGE COLUMN `code` `code` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_client` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_cutter` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_device` 
CHANGE COLUMN `code` `code` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_device_type` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_item` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_item_type` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '物料类型编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_material_stock` 
CHANGE COLUMN `code` `code` VARCHAR(255) NOT NULL COMMENT '出入库单号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_notify` 
CHANGE COLUMN `notify_code` `notify_code` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL COMMENT '通知类型编号' ,
ADD UNIQUE INDEX `notify_code_UNIQUE` (`notify_code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_process` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_process_recipe` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_product_bom` 
CHANGE COLUMN `code` `code` VARCHAR(64) NOT NULL COMMENT '产品结构编码' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_product_category` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '任务编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_subject` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_task` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_trans_order` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_unit_measure` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;

ALTER TABLE `vg_autodrill_db`.`t_vendor` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_work_order` 
CHANGE COLUMN `code` `code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '编号' ,
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_workshop` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


ALTER TABLE `vg_autodrill_db`.`t_workstation` 
ADD UNIQUE INDEX `code_UNIQUE` (`code` ASC);
;


-- 0725
ALTER TABLE `vg_autodrill_db`.`t_trans_order` 
DROP COLUMN `parent_id`;



-- 0731
ALTER TABLE `vg_autodrill_db`.`t_route_and_process` 
CHANGE COLUMN `required_time` `required_time` INT(11) NULL DEFAULT NULL COMMENT '本工序耗时(Min)' ;


-- 0801
ALTER TABLE `vg_autodrill_db`.`t_drill_work_order` 
ADD COLUMN `is_submited` TINYINT(4) NULL DEFAULT '0' COMMENT '是否已提交' AFTER `modifier_id`,
ADD COLUMN `submit_user` INT(11) NULL DEFAULT NULL COMMENT '提交人' AFTER `is_submited`,
ADD COLUMN `submit_time` DATETIME NULL DEFAULT NULL COMMENT '提交时间' AFTER `submit_user`;


-- 0802
ALTER TABLE `vg_autodrill_db`.`t_material_stock` 
CHANGE COLUMN `quantity_transaction` `quantity_transaction` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '数量(叠数或片数)' ,
CHANGE COLUMN `quantity_onhand` `quantity_onhand` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '在库数量(叠数或片数)' ;

ALTER TABLE `vg_autodrill_db`.`t_material_stock` 
ADD COLUMN `process_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '工序编号' AFTER `parent_code`,
ADD COLUMN `process_name` VARCHAR(50) NULL DEFAULT NULL COMMENT '工序名称' AFTER `process_code`;


CREATE TABLE `t_material_stock_overview` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `item_code` varchar(64) DEFAULT NULL COMMENT '产品物料编码',
  `item_name` varchar(255) DEFAULT NULL COMMENT '产品物料名称',
  `process_name` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序名称',
  `process_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工序编号',
  `quantity_onhand` decimal(12,2) DEFAULT NULL COMMENT '数量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`item_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='库存总览表';


-- 0803
ALTER TABLE `vg_autodrill_db`.`t_task` 
CHANGE COLUMN `task_status` `task_status` VARCHAR(20) NULL DEFAULT 'DRAFT' COMMENT '完工状态(DRAFT/COMMITED/BEGIN/FINISH)' ;



-- 0804
ALTER TABLE `vg_autodrill_db`.`t_material_stock_overview` 
ADD COLUMN `sum_plan` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '计划占用量' AFTER `modifier_id`,
ADD COLUMN `usable_count` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '预计可用数量' AFTER `sum_plan`,
CHANGE COLUMN `quantity_onhand` `sum_onhand` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '在库总数量' ;


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


-- 0809
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`task_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='生产任务表';

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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='生产任务历史表';

ALTER TABLE `vg_autodrill_db`.`t_unit_measure` 
CHANGE COLUMN `change_rate` `change_rate` DECIMAL(12,4) NULL DEFAULT 1 COMMENT '与主单位换算比例，默认为1' ;


-- 0811
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

-- 0812
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `route_name` VARCHAR(255) NULL COMMENT '工艺路线名称' AFTER `route_code`;

ALTER TABLE `vg_autodrill_db`.`t_produce_task` 
ADD COLUMN `start_time` DATETIME NULL COMMENT '开始时间' AFTER `modifier_id`,
ADD COLUMN `end_time` DATETIME NULL COMMENT '结束时间' AFTER `start_time`;


ALTER TABLE `vg_autodrill_db`.`t_produce_task_history` 
ADD COLUMN `start_time` DATETIME NULL COMMENT '开始时间' AFTER `modifier_id`,
ADD COLUMN `end_time` DATETIME NULL COMMENT '结束时间' AFTER `start_time`;


ALTER TABLE `vg_autodrill_db`.`t_material_stock` 
ADD COLUMN `station_code` VARCHAR(64) NULL COMMENT '工作站编码' AFTER `station_id`;

ALTER TABLE `vg_autodrill_db`.`t_workstation` 
ADD COLUMN `process_id` INT(11) NULL COMMENT '默认工序名称' AFTER `remark`,
ADD COLUMN `process_code` VARCHAR(50) NULL COMMENT '默认工序编码' AFTER `process_id`,
ADD COLUMN `process_name` VARCHAR(50) NULL COMMENT '默认工序名称' AFTER `process_code`;

-- 0813
ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `panel_count` DECIMAL(12,4) NULL DEFAULT 1 COMMENT '叠板层数' AFTER `now_wad_count`;
ALTER TABLE `vg_autodrill_db`.`t_feedback` 
ADD COLUMN `panel_count` DECIMAL(12,4) NULL DEFAULT 1 COMMENT '叠板层数';

-- 0815
ALTER TABLE `vg_autodrill_db`.`t_product_category` 
ADD COLUMN `dispense_machines` DECIMAL(12,2) NULL COMMENT '建议机台数' AFTER `ancestors`;

ALTER TABLE `vg_autodrill_db`.`t_drill_work_order` 
CHANGE COLUMN `dispense_machines` `dispense_machines` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '建议机台数' ;


-- 0816
ALTER TABLE `vg_autodrill_db`.`t_encode_build_rules` 
ADD COLUMN `has_year` TINYINT(1) NULL COMMENT '是否包含年份' AFTER `modifier_id`,
ADD COLUMN `has_month` TINYINT(1) NULL COMMENT '是否包含月份' AFTER `has_year`,
ADD COLUMN `has_day` TINYINT(1) NULL COMMENT '是否包含天数' AFTER `has_month`;

ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
ADD COLUMN `work_station_code` VARCHAR(64) NULL COMMENT '工位编码' AFTER `modifier_id`,
ADD COLUMN `work_station_name` VARCHAR(255) NULL COMMENT '工位名称' AFTER `work_station_code`,
CHANGE COLUMN `workStationId` `work_station_id` VARCHAR(500) NULL DEFAULT NULL COMMENT '工位位置' ;

ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
CHANGE COLUMN `work_station_id` `work_station_id` BIGINT(20) NULL DEFAULT NULL COMMENT '工位位置' ;


-- 0823
ALTER TABLE `vg_autodrill_db`.`t_route` 
ADD COLUMN `vetting_status` TINYINT(4) NULL DEFAULT '0' COMMENT '审批状态(0:未审批;1:已审批)' AFTER `modifier_id`;

-- 0824
ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `dispense_machines` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '建议机台数' AFTER `warehouse_name`;

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `dispense_machines` DECIMAL(12,2) NULL DEFAULT NULL COMMENT '建议机台数' AFTER `route_name`;


-- 0825
ALTER TABLE `vg_autodrill_db`.`t_drill_work_order` 
ADD COLUMN `is_add_task` TINYINT(4) NULL DEFAULT '0' COMMENT '是否添加任务' AFTER `submit_time`,
ADD COLUMN `add_task_user` INT(11) NULL DEFAULT NULL COMMENT '添加任务人' AFTER `is_add_task`,
ADD COLUMN `add_task_time` DATETIME NULL DEFAULT NULL COMMENT '添加任务时间' AFTER `add_task_user`;


-- 0904
CREATE TABLE `t_schedulement_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `master_id` bigint(20) NOT NULL COMMENT '主表Id',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `message` varchar(200) CHARACTER SET utf8 DEFAULT NULL COMMENT '调度记录明细',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='调度记录明细';


-- 0911
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `item_id` INT(11) NULL DEFAULT NULL COMMENT '产品Id' AFTER `changed_behavior`,
ADD COLUMN `item_name` VARCHAR(50) CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '产品名称' AFTER `item_id`,
ADD COLUMN `item_code` VARCHAR(50) CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '产品编号' AFTER `item_name`;


-- 0916
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `manu_oder_status` TINYINT(4) NULL DEFAULT 0 COMMENT '生产工单的状态(0-DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)' AFTER `request_date`;

update vg_autodrill_db.t_work_order
set manu_order_status=0
where work_order_status='DRAFT'
and id>0;

update vg_autodrill_db.t_work_order
set manu_order_status=1
where work_order_status='COMMITED'
and id>0
and is_add_work_order=0;

update vg_autodrill_db.t_work_order
set manu_order_status=2
where work_order_status='COMMITED'
and is_add_work_order=1
and id>0;

-- 有一条任务开始，即表示投产
update vg_autodrill_db.t_work_order a, vg_autodrill_db.t_task b
set a.manu_order_status=3
where a.work_order_status='COMMITED'
and a.is_add_work_order=1
and a.id>0
and a.code=b.work_order_code
and b.real_start_time is not null;

-- 已投产，且不存在未完工的任务，表示完工
update vg_autodrill_db.t_work_order a
set a.manu_order_status=4
where a.work_order_status='COMMITED'
and a.is_add_work_order=1
and a.id>0
and a.manu_order_status=3
and not exists
(
select 1
from vg_autodrill_db.t_task b 
where a.code=b.work_order_code
and b.real_end_time is null
);


-- 0918
ALTER TABLE `vg_autodrill_db`.`t_product_category` 
ADD COLUMN `vetting_status` TINYINT(4) NULL DEFAULT '0' COMMENT '审批状态(0:未审批;1:已审批)' AFTER `dispense_machines`;


-- 1009
ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `route_id` BIGINT(20) NULL COMMENT '工艺路线ID' AFTER `panel_count`,
ADD COLUMN `route_code` VARCHAR(64) NULL COMMENT '工艺路线编码' AFTER `route_id`,
ADD COLUMN `route_name` VARCHAR(255) NULL COMMENT '工艺路线名称' AFTER `route_code`;


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

-- 1013
update vg_autodrill_db.t_device a, vg_autodrill_db.t_device_type b
set a.device_type_code= b.code
where a.id>0
and a.device_type_id=b.id
and a.device_type_code is null;

select * from  vg_autodrill_db.t_device a, vg_autodrill_db.t_device_type b where a.device_type_id=b.id and a.device_type_code != b.code;



-- 1015
update vg_autodrill_db.t_task a, vg_autodrill_db.t_work_order b
set a.route_id= b.route_id,
a.route_code =b.route_code,
a.route_name =b.route_name
where a.id>0
and a.work_order_id=b.id
and a.route_id is null;


SELECT * FROM vg_autodrill_db.t_task where route_id is null or route_code is null or route_name is null;


-- 1018
-- 1023 刷新，新加了config_enum_value
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

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('CentralControlSystemIsMaintain', 'false', 'bool', '中控系统是否在维护', '中控系统是否在维护', '0', '1', '1', '2023-10-19 11:40:10', '1');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('CentralControlSystemCreatedTimeout', '300', 'int', '中控系统已上报调度的超时设定(秒)', '中控系统已上报调度的超时设定(秒)', '0', '1', '1', '2023-10-19 11:40:10', '1');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('CentralControlSystemAllocatedTimeout', '300', 'int', '中控系统已分配的调度的超时设定(秒)', '中控系统已分配的调度的超时设定(秒)', '0', '1', '1', '2023-10-19 11:40:10', '1');


-- 1026
ALTER TABLE `vg_autodrill_db`.`t_schedulement_detail` 
CHANGE COLUMN `message` `message` VARCHAR(5000) CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '调度记录明细' ;


-- 1027
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `warnning_code` VARCHAR(200) NULL DEFAULT NULL COMMENT '告警编码' AFTER `item_code`,
ADD COLUMN `warnning_message` VARCHAR(1000) NULL DEFAULT NULL COMMENT '告警详细信息' AFTER `warnning_code`;

-- 1102
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('IsShowTimelyInformation', 'true', 'bool', '是否显示告警等及时信息', '是否显示告警等及时信息', '0', '1', '1', '2023-10-19 11:40:10', '1');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('AGVLatestScheduleCount', '5', 'int', 'AGV最近调度记录条数', 'AGV最近调度记录条数', '0', '1', '1', '2023-11-9 11:40:10', '1');



-- 1110
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `RequestInteractionBehavior` VARCHAR(50) NULL DEFAULT NULL COMMENT '交互方式编码' AFTER `warnning_message`,
ADD COLUMN `RequestInteractionBehaviorName` VARCHAR(200) NULL DEFAULT NULL COMMENT '交互方式描述' AFTER `RequestInteractionBehavior`;


-- 1113
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('DrillTaskShaftCount', '5', 'int', '钻孔任务轴数', '钻孔任务轴数', '0', '1', '1', '2023-11-13 11:40:10', '1');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('DrillDeviceTaskShowCount', '100', 'int', '在线钻机待做任务显示个数', '在线钻机待做任务显示个数', '0', '1', '1', '2023-11-13 11:40:10', '1');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('AGVDeviceSiloInfoShowCount', '20', 'int', '在线AGV板料信息及最近调度记录显示个数', '在线AGV板料信息及最近调度记录显示个数', '0', '1', '1', '2023-11-13 11:40:10', '1');


-- 1116
ALTER TABLE `vg_autodrill_db`.`t_device_type` 
ADD COLUMN `is_manufacture` TINYINT(4) NOT NULL DEFAULT '0' COMMENT '是否属于生产设备' AFTER `ancestors`;

UPDATE `vg_autodrill_db`.`t_device_type` SET `is_manufacture` = '1' WHERE (`id` = '7');
UPDATE `vg_autodrill_db`.`t_device_type` SET `is_manufacture` = '1' WHERE (`id` = '14');
UPDATE `vg_autodrill_db`.`t_device_type` SET `is_manufacture` = '1' WHERE (`id` = '15');


-- 1211
ALTER TABLE `vg_autodrill_db`.`t_item_atp_file_detail` 
ADD UNIQUE INDEX `index_unique` (`item_atp_file_id` ASC, `order_num` ASC);
;

-- 1219
-- 生产任务状态 初始化
select * from t_task a where a.task_status=1;
update t_task  a set a.task_status=10 where a.task_status=1 and id >1;

select * from t_task a where a.task_status=2;
update t_task  a set a.task_status=40 where a.task_status=2 and id >1;

select * from t_task a where a.task_status=3;
update t_task  a set a.task_status=50 where a.task_status=3 and id >1;

-- 1219
-- 工单是否紧急插单
alter table t_work_order add column is_urgent int default 0 COMMENT '是否紧急插单';

-- 1219
-- 工单是否紧急插单 
alter table t_drill_work_order add column is_urgent int default 0 COMMENT '是否紧急插单';
alter table t_task add column  is_urgent int  default 0 COMMENT '是否紧急插单';

-- 240105
-- 添加工单ID
alter table t_workorder_and_workstation add column  work_order_id int  NOT NULL COMMENT '工单ID';

-- 240122
-- 新建设备网关表
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
  `installed_location` varchar(350) DEFAULT NULL COMMENT '安装位置',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备网关'

-- 240126
-- t_device_gateway添加parameters列
alter table t_device_gateway add column  parameters varchar(5000) CHARACTER SET utf8 DEFAULT NULL COMMENT '参数';

-- 240131
-- t_work_order添加remark_color列
alter table t_work_order add column remark_color CHAR(7)   DEFAULT NULL COMMENT '标记颜色';

-- 20240201
-- 创建外部对接工单表
CREATE TABLE `t_external_work_order`  (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '工单编号',
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '工单名称',
  `order_source` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '来源类型',
  `source_code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '来源单据',
  `item_code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品编号',
  `item_name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品名称',
  `specification` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '规格型号',
  `unit_of_measure` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '单位',
  `quantity` decimal(20, 0) NULL DEFAULT NULL COMMENT '生产数量',
  `panel_count` decimal(20, 0) NULL DEFAULT NULL COMMENT '叠板层数',
  `route_code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '工艺路线编号',
  `route_name` varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '工艺路线名称',
  `request_date` datetime NULL DEFAULT NULL COMMENT '需求日期',
  `is_urgent` int(10) NULL DEFAULT NULL COMMENT '是否紧急插单',
  `is_deleted` bit(1) NULL DEFAULT NULL,
  `status` int(10) NULL DEFAULT NULL,
  `creator_id` int(4) NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT NULL,
  `modifier_id` int(11) NULL DEFAULT NULL,
  `modify_time` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '外部工单表' ROW_FORMAT = Dynamic;

-- 240202
-- t_device_gateway添加app_name列
alter table t_device_gateway add column app_name varchar(80) CHARACTER SET utf8 NOT NULL COMMENT '应用名称';

-- 20240203
-- t_external_work_order 添加remark列
alter table t_external_work_order add column remark varchar(150) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '处理结果描述';

-- 20240205
-- 创建料架管理表 by yuchenglong
-- 20240220 追加内点、外点两个字段 by xifajin
CREATE TABLE `t_rack`  (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '料架编号',
  `warehouse_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所属仓库编号',
  `warehouse_id` int(8) NULL DEFAULT NULL COMMENT '所属仓库Id',
  `is_have_silo` tinyint(2) NULL DEFAULT NULL COMMENT '是否关联料仓',
  `silo_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '料仓编号',
  `inner_point` varchar(255) DEFAULT NULL COMMENT '内点',
  `out_point` varchar(255) DEFAULT NULL COMMENT '外点',
  `is_deleted` tinyint(4) NOT NULL DEFAULT 0 COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT 1 COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) NULL DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime NULL DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) NULL DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '料架基本信息表' ROW_FORMAT = Dynamic;

-- 创建料仓管理表
-- 20240205 by yuchenglong
CREATE TABLE `t_silo`  (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '料仓编码',
  `size` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '运载尺寸',
  `floor_count` int(5) NULL DEFAULT NULL COMMENT '层数',
  `supplier` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '供应商',
  `location` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '位置',
  `empty_silo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '空料仓',
  `is_deleted` tinyint(4) NOT NULL DEFAULT 0 COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT 1 COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) NULL DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime NULL DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) NULL DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '料仓基本信息表' ROW_FORMAT = Dynamic;

-- 创建料仓载料明细表
-- 20240222 by yuchenglong
CREATE TABLE `t_silo_detail`  (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `silo_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '料仓编号',
  `floor_num` int(11) NULL DEFAULT NULL COMMENT '层号',
  `item_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '物料编号',
  `panel_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '板料编码',
  `product_status` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '该层板料类型:（0-空仓, 30100-生料,40100-熟料）',
  `pcs` int(8) NULL DEFAULT NULL COMMENT '每叠片数',
  `panel_width` decimal(8, 2) NULL DEFAULT NULL COMMENT '板料宽度',
  `pin_offset` decimal(8, 2) NULL DEFAULT NULL COMMENT '梢钉偏移量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT 0 COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT 1 COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) NULL DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime NULL DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) NULL DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 21 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '料仓载料明细表' ROW_FORMAT = Dynamic;

-- t_external_work_order 添加drill_count列 by yuchenglong 2024-02-23
alter table t_external_work_order add column drill_count decimal(12,0) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '孔数';
alter table t_external_work_order add column product_category_code  varchar(50) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '大类编码';
alter table t_external_work_order add column wad_count decimal(12,0) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '待叠板叠数';
alter table t_external_work_order add column client_name varchar(50) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '客户名称';
alter table t_external_work_order add column client_code varchar(50) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '客户编码';
alter table t_external_work_order add column device_codes varchar(200) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '待分配的机台编号';
--t_external_work_order 添加dispense_machines by yuchenglong 2024-02-28
alter table t_external_work_order add column dispense_machines  decimal(12,0) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '建议机台数';
-- 生产工单表，移除过期的状态字段 by xifajin 20240303
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
DROP COLUMN `work_order_status`;

-- 工单板材表 20240304
CREATE TABLE `t_workOrder_and_panel` (
  id bigint(20) NOT NULL AUTO_INCREMENT,
  work_order_code varchar(50) NOT NULL COMMENT '工单编码',
  item_code varchar(50) NOT NULL COMMENT '物料编码',
  panel_code varchar(50) NOT NULL COMMENT '板材编码',
  pcs int(11) NOT NULL DEFAULT '1' COMMENT '片数',
  batch_Code varchar(50) COMMENT '批次号',
  product_status varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '产品状态',
  board_Location varchar(50) COMMENT '板料位置',
  station_id bigint(20) DEFAULT NULL COMMENT '工位ID',
  is_deleted tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  status int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  creator_id int(11) DEFAULT NULL COMMENT '创建人Id',
  create_time datetime NOT NULL COMMENT '创建时间',
  modify_time datetime DEFAULT NULL COMMENT '修改时间',
  modifier_id int(11) DEFAULT NULL COMMENT '修改人Id',
 
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=158 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='工单板材';

-- 工单板材表 20240307 追加列task_code，索引panel_code，task_code
alter table t_workOrder_and_panel add column panel_width decimal(8,2) DEFAULT NULL COMMENT '板料宽度';
alter table t_workOrder_and_panel add column pin_offset decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量';
alter table t_workOrder_and_panel add column external_worker_order varchar(50) DEFAULT NULL COMMENT '外部工单号';
alter table t_workOrder_and_panel add column task_code varchar(50) not null COMMENT '任务号';
alter table t_workOrder_and_panel add unique (panel_code);
alter table t_workOrder_and_panel add index (task_code);
alter table t_workOrder_and_panel add index (external_worker_order);
alter table t_workOrder_and_panel add index (task_code);

-- 20240307 调度记录表，增加字段 累计已上生料
ALTER TABLE `vg_autodrill_db`.`t_schedulement` 
ADD COLUMN `total_raw_count` INT(11) NULL COMMENT '累计已上生料';

-- 20240312 调度记录表，增加字段 是否紧急
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `is_urgent` TINYINT(4) NULL DEFAULT '0' COMMENT '是否紧急';

-- 20240312 更正生产工单、生产任务的是否紧急字段的定义
ALTER TABLE `vg_autodrill_db`.`t_task` 
CHANGE COLUMN `is_urgent` `is_urgent` TINYINT(4) NULL DEFAULT '0' COMMENT '是否紧急' ;
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
CHANGE COLUMN `is_urgent` `is_urgent` TINYINT(4) NULL DEFAULT '0' COMMENT '是否紧急' ;
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
CHANGE COLUMN `is_urgent` `is_urgent` TINYINT(4) NULL DEFAULT NULL COMMENT '是否紧急' ;


--20240316 新建钻机服务调用表
-- 调用时间 设置为可空
CREATE TABLE `t_device_service_invocation` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `message_id` varchar(50) DEFAULT NULL COMMENT 'MessageId',
  `request_topic` varchar(500) DEFAULT NULL COMMENT '请求',
  `response_topic` varchar(500) DEFAULT NULL COMMENT '响应',
  `payload` varchar(10000) DEFAULT NULL COMMENT '载荷',
  `retries` int(11) DEFAULT '0' COMMENT '尝试次数',
  `is_timeout` tinyint(1) DEFAULT '0' COMMENT '是否超时',
  `reason` varchar(500) DEFAULT NULL COMMENT '原因',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `first_invocation_timestamp` datetime NULL COMMENT '首次调用时间',
  `last_invocation_timestamp` datetime NULL COMMENT '末次调用时间',
  `mqtt_quality_of_service_level` int(11) DEFAULT '0' COMMENT '等级',
  `is_urgent` tinyint(4) DEFAULT '0' COMMENT '是否紧急',
  `is_dealed` tinyint(1) DEFAULT '0' COMMENT '是否已处理',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `message_id` (`message_id`),
  KEY `is_dealed` (`is_dealed`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备服务调用';


alter table t_device_service_invocation add index (MessageId);
alter table t_device_service_invocation add index (Invocation_Status);

--20240316 修改调度记录 表名， request_json 长度

ALTER TABLE  `vg_autodrill_db`.`t_schedulement_detail` RENAME  TO  `vg_autodrill_db`.`t_schedule_log`;
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
CHANGE COLUMN `request_json` `request_json` varchar(10000) NULL DEFAULT NULL COMMENT 'request json' ;


--20240316 sys_config表新加字段
alter table sys_config add column module int(11) comment '模块';
alter table sys_config add column  Is_Quick_Config tinyint(4) DEFAULT '0' COMMENT '是否快速配置';

-- 20240317 增加routing key
ALTER TABLE `vg_autodrill_db`.`t_device_service_invocation` 
add column `routing_key` varchar(100) DEFAULT NULL COMMENT '主叫设备的route key';

-- 20240318 t_device_service_invocation字段长度修改
alter table t_device_service_invocation MODIFY message_id varchar(500) DEFAULT NULL COMMENT '消息编码';
alter table t_device_service_invocation MODIFY payload mediumtext DEFAULT NULL COMMENT '载荷';

-- 20240318 t_schedule字段长度修改
alter table t_schedule MODIFY code varchar(500) CHARACTER SET utf8 NOT NULL COMMENT '任务编号';
alter table t_schedule MODIFY request_json mediumtext CHARACTER SET utf8 DEFAULT NULL COMMENT '请求详情';

-- 20240325 t_schedule 增加是否已全部送料
-- 20240326 rename to  is_all_panel_sent
alter table t_schedule add column is_all_panel_sent tinyint(1) default '1' NULL COMMENT '是否已全部送料';

alter table t_device_service_invocation MODIFY `is_dealed` tinyint(1) DEFAULT '0' COMMENT '是否已处理';

-- 20240326 是否快速配置
ALTER TABLE `vg_autodrill_db`.`sys_config` 
CHANGE COLUMN `Is_Quick_Config` `is_quick_config` TINYINT(1) NULL DEFAULT '0' COMMENT '是否快速配置' ;

-- 20240402 
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `route_id` BIGINT(20) NULL COMMENT '工艺路线ID' ,
ADD COLUMN `route_code` VARCHAR(64) NULL COMMENT '工艺路线编码' ,
ADD COLUMN `route_name` VARCHAR(255) NULL COMMENT '工艺路线名称' ;

update `vg_autodrill_db`.`t_schedule` as a
inner join `vg_autodrill_db`.`t_task` as b on a.task_id=b.code
set a.route_id=b.route_id,a.route_code=b.route_code,a.route_name=b.route_name
where a.id > 0  and a.task_id is not null and a.route_id is null;

-- 20240407
ALTER TABLE `vg_autodrill_db`.`t_panel` 
ADD COLUMN `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
ADD COLUMN `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量';

ALTER TABLE `vg_autodrill_db`.`t_device_panel` 
ADD COLUMN `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
ADD COLUMN `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量';

ALTER TABLE `vg_autodrill_db`.`t_device_panel_history` 
ADD COLUMN `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
ADD COLUMN `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量';

-- 20240413
ALTER TABLE `vg_autodrill_db`.`t_device_panel` 
CHANGE COLUMN `item_no` `item_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '料号' ;

ALTER TABLE `vg_autodrill_db`.`t_device_panel_history` 
CHANGE COLUMN `item_no` `item_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '料号' ;

ALTER TABLE `vg_autodrill_db`.`t_device_panel` 
CHANGE COLUMN `batch_id` `batch_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '批次号' ;

ALTER TABLE `vg_autodrill_db`.`t_device_panel_history` 
CHANGE COLUMN `batch_id` `batch_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '批次号' ;

-- 20240425
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `inner_order_id` BIGINT(20) NULL COMMENT '内部工单ID' AFTER `modify_time`;

-- 20240428
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
CHANGE COLUMN `order_source` `order_source` VARCHAR(50) CHARACTER SET 'utf8' NULL DEFAULT '库存需求' COMMENT '来源类型' ,
CHANGE COLUMN `source_code` `source_code` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL COMMENT '来源单据' ;

ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD UNIQUE INDEX `source_code_UNIQUE` (`source_code` ASC);

-- 20240428
ALTER TABLE `vg_autodrill_db`.`t_silo` 
CHANGE COLUMN `empty_silo` `empty_silo` VARCHAR(2000) NULL DEFAULT NULL COMMENT '空料仓' ;

-- 20240512
ALTER TABLE `vg_autodrill_db`.`t_silo` 
ADD COLUMN `related_device_kind` INT(11) NULL COMMENT '相关设备类别' ,
ADD COLUMN `silo_status` INT(11) NULL COMMENT '料仓状态' ;

update vg_autodrill_db.t_silo
set silo_status = -1
where silo_status is null and id >0;

ALTER TABLE `vg_autodrill_db`.`t_rack` 
ADD COLUMN `trans_inner_point` varchar(255) DEFAULT NULL COMMENT '小车内点',
ADD COLUMN `trans_out_point` varchar(255) DEFAULT NULL COMMENT '小车外点',
ADD COLUMN `device_kind` INT(11) NULL COMMENT '设备类别';

-- 20240520
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `master_schedule_id` BIGINT NULL COMMENT '关联的主叫调度记录ID' ,
ADD COLUMN `is_auxiliary` TINYINT(1) NULL DEFAULT '0' COMMENT '是否是辅助设备请求' ;

update vg_autodrill_db.t_schedule
set is_auxiliary = 0
where is_auxiliary is null and id >0;

-- 20240521
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `incode_number` VARCHAR(100) NULL COMMENT '条码';

ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `incode_number` VARCHAR(100) NULL COMMENT '条码';

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `incode_number` VARCHAR(100) NULL COMMENT '条码';

ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `incode_number` VARCHAR(100) NULL COMMENT '条码';

ALTER TABLE `vg_autodrill_db`.`t_device` 
CHANGE COLUMN `parameters` `parameters` MEDIUMTEXT CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '设备参数' ;

-- 20240523
ALTER TABLE `vg_autodrill_db`.`t_alarm` 
CHANGE COLUMN `event_data` `event_data` MEDIUMTEXT CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '事件数据' ;

ALTER TABLE `vg_autodrill_db`.`t_alarm_setting` 
CHANGE COLUMN  `notify_way_ids` `notify_way_ids` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '通知方式',
CHANGE COLUMN  `notify_way_names` `notify_way_names` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '通知方式名称',
CHANGE COLUMN  `event_rules` `event_rules` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '触发规则',
CHANGE COLUMN  `ancestors` `ancestors` MEDIUMTEXT DEFAULT NULL COMMENT '所有层级父节点';

ALTER TABLE `vg_autodrill_db`.`t_device` 
CHANGE COLUMN `tag` `tag` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '设备标签, 类似a=b键值对',
CHANGE COLUMN  `ancestors` `ancestors` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '所有层级父节点';

ALTER TABLE `vg_autodrill_db`.`t_device_gateway` 
CHANGE COLUMN `installed_location` `installed_location` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '安装位置',
CHANGE COLUMN  `parameters` `parameters` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '参数';

ALTER TABLE `vg_autodrill_db`.`t_device_parameter` 
CHANGE COLUMN  `parameters` `parameters` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '参数';

ALTER TABLE `vg_autodrill_db`.`t_event_define` 
CHANGE COLUMN  `parameter_json` `parameter_json` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '参数';

ALTER TABLE `vg_autodrill_db`.`t_item_atp_file` 
CHANGE COLUMN  `atp_parameters` `atp_parameters` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT 'ATP参数';

ALTER TABLE `vg_autodrill_db`.`t_notify` 
CHANGE COLUMN  `notify_msg` `notify_msg` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '消息';

ALTER TABLE `vg_autodrill_db`.`t_notify_setting` 
CHANGE COLUMN  `notify_params` `notify_params` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '参数';

ALTER TABLE `vg_autodrill_db`.`t_process_recipe` 
CHANGE COLUMN  `parameters` `parameters` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '参数';

ALTER TABLE `vg_autodrill_db`.`t_schedule` 
CHANGE COLUMN  `warnning_message` `warnning_message` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '消息';

ALTER TABLE `vg_autodrill_db`.`t_schedule_log` 
CHANGE COLUMN  `message` `message` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '调度记录明细';

ALTER TABLE `vg_autodrill_db`.`t_silo` 
CHANGE COLUMN  `empty_silo` `empty_silo` MEDIUMTEXT CHARACTER SET utf8 DEFAULT NULL COMMENT '空料仓';

ALTER TABLE `vg_autodrill_db`.`t_silo_detail` 
CHANGE COLUMN `product_status` `product_status` INT(8) NULL DEFAULT NULL COMMENT '该层板料类型:（0-空仓, 30100-生料,40100-熟料）' ;

ALTER TABLE `vg_autodrill_db`.`t_workorder_and_panel` 
CHANGE COLUMN `product_status` `product_status` INT(8) NULL DEFAULT NULL COMMENT '产品状态' ;

-- 需要清除原有的数据
Truncate TABLE `vg_autodrill_db`.`t_panel`;
ALTER TABLE `vg_autodrill_db`.`t_panel` 
CHANGE COLUMN `product_status` `product_status` INT(8) NULL DEFAULT NULL COMMENT '产品状态' ;

-- 20240524
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `panel_length` DECIMAL(8,2) NULL COMMENT '板料长度' ;

ALTER TABLE `vg_autodrill_db`.`t_panel` 
ADD COLUMN `panel_length` DECIMAL(8,2) NULL COMMENT '板料长度';

ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `panel_length` DECIMAL(8,2) NULL COMMENT '板料长度' ;


-- 20240524 后追加
ALTER TABLE `vg_autodrill_db`.`t_workorder_and_panel` 
ADD COLUMN `panel_length` DECIMAL(8,2) NULL COMMENT '板料长度' ;

-- 20240525
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `interaction_sequence` INT(11) NULL COMMENT '呼叫序列' ,
ADD COLUMN `request_device_kind` INT(11) NULL COMMENT '呼叫设备kind' ;

ALTER TABLE `vg_autodrill_db`.`t_schedule` 
CHANGE COLUMN `RequestInteractionBehavior` `request_interaction_behavior` VARCHAR(50) NULL DEFAULT NULL COMMENT '交互方式编码' ,
CHANGE COLUMN `RequestInteractionBehaviorName` `request_interaction_behavior_name` VARCHAR(200) NULL DEFAULT NULL COMMENT '交互方式描述' ;

-- 20240528
ALTER TABLE `vg_autodrill_db`.`t_device` 
ADD COLUMN `device_kind` INT(11) NULL COMMENT 'device kind';

ALTER TABLE `vg_autodrill_db`.`t_device` 
ADD COLUMN `spindle_num` INT(11) NULL COMMENT '轴数';

-- 根据参数值，更新字段
update vg_autodrill_db.t_device a
set a.device_kind=JSON_EXTRACT(parameters, '$.DeviceKind')
where id> 0;

-- 根据参数值，更新字段
update vg_autodrill_db.t_device a
set a.spindle_num=JSON_EXTRACT(parameters, '$.SpindleNum')
where id> 0;

-- 20240531
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
CHANGE COLUMN `start_location` `start_location` MEDIUMTEXT CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '起点' ,
CHANGE COLUMN `end_location` `end_location` MEDIUMTEXT CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '终点' ;

-- 20240602
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `is_master` TINYINT(1) NULL COMMENT '是否为先执行' ;

-- 20240615
ALTER TABLE `vg_autodrill_db`.`t_rack` 
ADD COLUMN `relate_device_code` VARCHAR(200) NULL COMMENT '关联的设备code' ;


-- 20240619
ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `drill_file_path` VARCHAR(500) NULL COMMENT '钻带文件路径';

-- 20240630
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `sub_device_code` VARCHAR(50) NULL COMMENT '库位编号' ;

-- 20240703
ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `panel_width` DECIMAL(8,2) NULL COMMENT '板料宽度' ;


-- 20240711
ALTER TABLE `vg_autodrill_db`.`t_device` 
ADD COLUMN `interaction_position` INT(11) NULL DEFAULT NULL COMMENT '交互位置' AFTER `spindle_num`;


CREATE TABLE `t_drill_panel_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_code` varchar(50) NOT NULL COMMENT '设备编号',
  `item_code` varchar(50) DEFAULT NULL COMMENT '物料编号',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料编号',
  `product_status` int(8) DEFAULT NULL COMMENT '该层板料类型:（38000-等待钻孔, 39000-正在钻孔,40000-钻机完成加工）',
  `pcs` int(8) DEFAULT NULL COMMENT '每叠片数',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻机载料明细表';



UPDATE `vg_autodrill_db`.`t_device_type` SET `ancestors` = '0,1' WHERE (`id` = '7');


-- 20240716
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='料仓任务表';



ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `move_in_time` DATETIME NULL DEFAULT NULL AFTER `incode_number`,
ADD COLUMN `move_out_time` DATETIME NULL DEFAULT NULL AFTER `move_in_time`,
ADD COLUMN `track_in_time` DATETIME NULL DEFAULT NULL AFTER `move_out_time`,
ADD COLUMN `track_out_time` DATETIME NULL DEFAULT NULL AFTER `track_in_time`;

-- 20240716 设备类型，增加料架与插齿
UPDATE `vg_autodrill_db`.`t_device_type` SET `name` = '料架与插齿', `code` = 'siloshelves' WHERE (`id` = '16');
UPDATE `vg_autodrill_db`.`t_device_type` SET `is_deleted` = '1' WHERE (`id` = '17');



-- 20240717
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `spec_group` VARCHAR(100) NULL DEFAULT NULL COMMENT '工序组' AFTER `panel_length`;

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `spec_group` VARCHAR(100) NULL DEFAULT NULL COMMENT '工序组' AFTER `track_out_time`;

ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `spec_group` VARCHAR(100) NULL DEFAULT NULL COMMENT '工序组' AFTER `incode_number`;

ALTER TABLE `vg_autodrill_db`.`t_drill_panel_detail` 
ADD COLUMN `layer` INT(11) NULL DEFAULT '0' COMMENT '0钻机, 1生料仓,2熟料仓' AFTER `panel_width`,
ADD COLUMN `splindleIndex` INT(11) NULL DEFAULT NULL AFTER `layer`;

-- 20240718
INSERT INTO `sys_menu` VALUES (1509,'DRILLAMIN','setMoveInTime',1122,'#','','',3,'produce:workorder:setMoveInTime',NULL,22,0,1,1,'2024-07-18 09:46:04','2024-07-18 09:48:21',1);
INSERT INTO `sys_menu` VALUES (1510,'DRILLAMIN','setMoveOutTime',1122,'#','','',3,'produce:workorder:setMoveOutTime',NULL,23,0,1,1,'2024-07-18 09:48:51',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1511,'DRILLAMIN','setTrackInTime',1122,'#','','',3,'produce:workorder:setTrackInTime',NULL,24,0,1,1,'2024-07-18 09:49:18',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1512,'DRILLAMIN','setTrackOutTime',1122,'#','','',3,'produce:workorder:setTrackOutTime',NULL,25,0,1,1,'2024-07-18 09:49:36',NULL,NULL);

-- 20240719
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `allocate_time` DATETIME NULL DEFAULT NULL COMMENT '分配时间' AFTER `modifier_id`,
ADD COLUMN `running_time` DATETIME NULL DEFAULT NULL COMMENT '开始调度时间' AFTER `allocate_time`,
ADD COLUMN `completed_time` DATETIME NULL DEFAULT NULL COMMENT '调度完成时间' AFTER `running_time`,
ADD COLUMN `failed_time` DATETIME NULL DEFAULT NULL COMMENT '执行失败时间' AFTER `completed_time`,
ADD COLUMN `canceled_time` DATETIME NULL DEFAULT NULL COMMENT '取消计划时间' AFTER `failed_time`;

-- 20240719 x
ALTER TABLE `vg_autodrill_db`.`t_drill_panel_detail` 
CHANGE COLUMN `splindleIndex` `splindle_index` INT(11) NULL DEFAULT NULL ;

ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `clinker_count` INT(11) NULL COMMENT '熟料数量' AFTER `canceled_time`;

-- 20240721 新增菜单权限
INSERT INTO `sys_menu` VALUES (1513,'DRILLAMIN','save',1500,'#','','',3,'system:config:save',NULL,1,0,1,1,'2024-07-18 10:55:28',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1514,'DRILLAMIN','料仓任务',1026,'#','transportationTask','device/transportationTask/index',2,'device:transportationTask:list',NULL,12,0,1,1,'2024-07-19 17:24:32',NULL,NULL);

-- 20240722 yp
ALTER TABLE `vg_autodrill_db`.`t_drill_panel_detail` 
ADD COLUMN `pin_offset` DECIMAL(8,2) NULL DEFAULT NULL COMMENT '梢钉偏移量' AFTER `modifier_id`,
ADD COLUMN `batch_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '批次号' AFTER `pin_offset`;


-- 20240725 yp
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `raw_ount` INT(11) NULL DEFAULT NULL COMMENT '生料数量' AFTER `clinker_count`,
ADD COLUMN `silo_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '料仓号' AFTER `raw_ount`,
ADD COLUMN `related_route` VARCHAR(100) NULL DEFAULT NULL AFTER `silo_code`,
ADD COLUMN `related_schedule_id` VARCHAR(100) NULL DEFAULT NULL AFTER `related_route`,
ADD COLUMN `relate_device_code` VARCHAR(100) NULL DEFAULT NULL AFTER `related_schedule_id`,
ADD COLUMN `other_fork_code` VARCHAR(100) NULL DEFAULT NULL AFTER `relate_device_code`;


ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
CHANGE COLUMN `related_schedule_id` `related_schedule_id` BIGINT(20) NULL DEFAULT NULL ;

-- 20240728 xfj
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`, `is_quick_config`) 
VALUES ('TransferLocationIsMaintain', 'true', 'bool', '中转区是否在维护', '中转区是否在维护', '0', '1', '1', '2024-7-28 15:03', '0', '1');


-- 20240730 yp
update vg_autodrill_db.t_device 
set interaction_position = 0 
where device_type_id =7 
and interaction_position is null 
and id>0;

-- 20240803
update vg_autodrill_db.t_rack
set device_kind=15,relate_device_code='PanelFork'
where code like 'PanelFork%' 
and device_kind is null
and id>0;

update vg_autodrill_db.t_rack
set device_kind=4,relate_device_code='Pin'
where code like 'Pin%' 
and device_kind is null
and id>0;

update vg_autodrill_db.t_rack
set device_kind=5,relate_device_code='UnPin' 
where code like 'UnPin%' 
and device_kind is null
and id>0;

update vg_autodrill_db.t_rack
set device_kind=13,relate_device_code='PanelShelf' 
where code like 'PanelShelf%' 
and device_kind is null
and id>0;

-- 20240805,菜单变更

UPDATE `vg_autodrill_db`.`sys_menu` SET `menu_url` = 'wareHouse/dashboard/rack' WHERE (`id` = '1486');

INSERT INTO `sys_menu` VALUES (1516,'DRILLAMIN','叠板看板',1158,'#','pinDashboard','wareHouse/dashboard/pin',2,'',NULL,3,0,1,1,'2024-08-02 16:53:58','2024-08-05 09:55:36',1);
INSERT INTO `sys_menu` VALUES (1517,'DRILLAMIN','拆板看板',1158,'#','unpinDashboard','wareHouse/dashboard/unpin',2,'',NULL,3,0,1,1,'2024-08-02 17:19:09','2024-08-05 09:55:41',1);
INSERT INTO `sys_menu` VALUES (1518,'DRILLAMIN','板料料架看板',1158,'#','panelSiloShelfDashboard','wareHouse/dashboard/panelSiloShelf',2,'',NULL,3,0,1,1,'2024-08-02 17:29:19','2024-08-05 09:55:46',1);
INSERT INTO `sys_menu` VALUES (1519,'DRILLAMIN','禁用',1035,'#','','',3,'device:device:disabled',NULL,7,0,1,1,'2024-08-05 09:23:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1520,'DRILLAMIN','启用',1035,'#','','',3,'device:device:enabled',NULL,7,0,1,1,'2024-08-05 09:24:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1521,'DRILLAMIN','板料叉齿看板',1158,'#','panelSiloForkDashboard','wareHouse/dashboard/panelSiloFork',2,'',NULL,3,0,1,1,'2024-08-05 10:22:03','2024-08-05 10:23:44',1);

-- 20240805,添加索引
CREATE INDEX idx_panel_code_of_t_device_panel_history ON t_device_panel_history (panel_code);

-- 20240808 yp
ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
ADD COLUMN `parent_id` BIGINT(20) NOT NULL AFTER `work_station_name`;

ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
ADD COLUMN `ancestors` VARCHAR(255) NULL DEFAULT NULL AFTER `parent_id`;

ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
CHANGE COLUMN `id` `id` BIGINT(20) NOT NULL AUTO_INCREMENT COMMENT '分区ID' ,
CHANGE COLUMN `code` `code` VARCHAR(255) NOT NULL COMMENT '分区编码' ,
CHANGE COLUMN `name` `name` VARCHAR(255) NOT NULL COMMENT '分区名称' , COMMENT = '分区表' ;

-- 初始化数据
delete from vg_autodrill_db.t_warehouse where id >0;

INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('1', '000', '全部', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '0', '0');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('2', 'PartPanelShelf', '线边仓', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '1', '0,1');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('3', 'PartPin', '上Pin', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '1', '0,1');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('4', 'PartUnpin', '下Pin', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '1', '0,1');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('5', 'PartPanelFork', '中转区', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '1', '0,1');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('6', 'PartPanelShelf-Raw', '生料区', '0', '0', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '2', '0,1,2');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('7', 'PartPanelShelf-Clinker', '熟料区', '0', '0', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '2', '0,1,2');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('8', 'PartPin-001', '上Pin一区', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '3', '0,1,3');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('9', 'PartPin-002', '上Pin二区', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '3', '0,1,3');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('10', 'PartUnpin-001', '下Pin一区', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '4', '0,1,4');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('11', 'PartUnpin-002', '下Pin二区', '0', '1', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '4', '0,1,4');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('12', 'PartPanelFork-Raw', '生料中转区', '0', '0', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '5', '0,1,5');
INSERT INTO `vg_autodrill_db`.`t_warehouse` (`id`, `code`, `name`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `parent_id`, `ancestors`) VALUES ('13', 'PartPanelFork-Clinker', '熟料中转区', '0', '0', '1', '2024-08-08 09:42:17', '2024-08-08 09:42:17', '5', '0,1,5');

-- 20240808,增加AGV休息点
CREATE TABLE `t_agv_rest` (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `code` varchar(200)  DEFAULT NULL COMMENT '休息点编码',
  `name` varchar(200)  DEFAULT NULL COMMENT '休息点名称',
  `part_code` varchar(200)  DEFAULT NULL COMMENT '分区编码',
  `part_name` varchar(200)  DEFAULT NULL COMMENT '分区名称',
  `route_code` varchar(200)  DEFAULT NULL COMMENT '工艺路线',
  `agv_device_kind` int DEFAULT NULL COMMENT 'AGV 类型  6-提升AGV  10--顶升AGV',
  `point` varchar(200)  DEFAULT NULL COMMENT '物理点位',
  `priority` int DEFAULT NULL COMMENT '该分区可分派的休息点的优先级  1>>2>>3',
  `pre_book_agv` varchar(255)  DEFAULT NULL COMMENT '该分区该点预定分配的AGV',
  `current_agv` varchar(255)  DEFAULT NULL COMMENT '当前点正占用的AGV',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `restcode_UNIQE` (`code`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci ROW_FORMAT=DYNAMIC COMMENT='AGV休息点';

-- 20240809 yp
ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
ADD COLUMN `pre_book_agv` VARCHAR(255) NULL DEFAULT NULL COMMENT '预约AGV' AFTER `ancestors`,
ADD COLUMN `pre_book_time` DATETIME NULL DEFAULT NULL COMMENT '预约时间' AFTER `pre_book_agv`;


-- 20240809,增加AGV休息点和分区关联关系
CREATE TABLE `t_agv_rest_and_part` (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `part_code` varchar(200) DEFAULT NULL COMMENT '分区编码',
  `part_name` varchar(200) DEFAULT NULL COMMENT '分区名称',
  `rest_id` int DEFAULT NULL COMMENT '休息点id',
  `route_code` varchar(200) DEFAULT NULL COMMENT '工艺路线',
  `agv_device_kind` int DEFAULT NULL COMMENT 'AGV 类型  6-提升AGV  10--顶升AGV',
  `point` varchar(200) DEFAULT NULL COMMENT '物理点位',
  `priority` int DEFAULT NULL COMMENT '该分区可分派的休息点的优先级  1>>2>>3',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='AGV休息点和分区关联关系';

-- 20240812,添加索引
CREATE INDEX idx_scheduled_task_status_of_t_schedule ON t_schedule (scheduled_task_status);
CREATE INDEX idx_source_device_id_of_t_schedule ON t_schedule (source_device_id);
CREATE INDEX idx_sub_device_code_of_t_schedule ON t_schedule (sub_device_code);
CREATE INDEX idx_request_device_kind_of_t_schedule ON t_schedule (request_device_kind);

CREATE INDEX idx_device_code_of_t_device_panel ON t_device_panel (device_code);
CREATE INDEX idx_item_code_of_t_device_panel ON t_device_panel (item_code);

CREATE INDEX idx_device_code_of_t_device_panel_history ON t_device_panel_history (device_code);

--20240815 yp
ALTER TABLE `vg_autodrill_db`.`t_agv_rest` 
DROP COLUMN `priority`,
DROP COLUMN `agv_device_kind`,
DROP COLUMN `route_code`,
DROP COLUMN `part_name`,
DROP COLUMN `part_code`;

ALTER TABLE `vg_autodrill_db`.`t_agv_rest_and_part` 
DROP COLUMN `point`;



-- 20240815 new menu
INSERT INTO `sys_menu` VALUES (1524,'DRILLAMIN','钻机看板',1026,'#','drillDashboard','device/drillDashboard/index',2,'device:drillDashboard:list',NULL,1,0,1,1,'2024-08-12 10:12:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1525,'DRILLAMIN','休息点列表',1158,'#','agvRest','wareHouse/agvRest/index',2,'wareHouse:agvRest:list',NULL,9,0,1,1,'2024-08-12 17:01:45','2024-08-15 10:28:55',1);
INSERT INTO `sys_menu` VALUES (1526,'DRILLAMIN','休息点配置',1158,'#','agvRestAndPart','wareHouse/agvRestAndPart/index',2,'wareHouse:agvRestAndPart:list',NULL,9,0,1,1,'2024-08-15 10:30:48','2024-08-15 15:59:54',1);
INSERT INTO `sys_menu` VALUES (1527,'DRILLAMIN','查看',1525,'#','','#',3,'wareHouse:agvRest:view',NULL,4,0,1,1,'2024-08-15 15:56:24',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1528,'DRILLAMIN','新增',1525,'#','','#',3,'wareHouse:agvRest:add',NULL,1,0,1,1,'2024-08-15 15:56:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1529,'DRILLAMIN','修改',1525,'#','','#',3,'wareHouse:agvRest:edit',NULL,2,0,1,1,'2024-08-15 15:57:11',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1530,'DRILLAMIN','删除',1525,'#','','#',3,'wareHouse:agvRest:remove',NULL,3,0,1,1,'2024-08-15 15:57:27','2024-08-15 15:57:59',1);
INSERT INTO `sys_menu` VALUES (1531,'DRILLAMIN','新增',1526,'#','','#',3,'wareHouse:agvRestAndPart:add',NULL,1,0,1,1,'2024-08-15 15:59:44',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1532,'DRILLAMIN','修改',1526,'#','','#',3,'wareHouse:agvRestAndPart:edit',NULL,2,0,1,1,'2024-08-15 16:00:12',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1533,'DRILLAMIN','删除',1526,'#','','#',3,'wareHouse:agvRestAndPart:remove',NULL,3,0,1,1,'2024-08-15 16:00:35',NULL,NULL);


-- 20240820 yp
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `move_in_time` DATETIME NULL DEFAULT NULL AFTER `spec_group`,
ADD COLUMN `move_out_time` DATETIME NULL DEFAULT NULL AFTER `move_in_time`,
ADD COLUMN `track_in_time` DATETIME NULL DEFAULT NULL AFTER `move_out_time`,
ADD COLUMN `track_out_time` DATETIME NULL DEFAULT NULL AFTER `track_in_time`,
ADD COLUMN `sign_move_in_time` DATETIME NULL DEFAULT NULL AFTER `track_out_time`,
ADD COLUMN `sign_move_out_time` DATETIME NULL DEFAULT NULL AFTER `sign_move_in_time`,
ADD COLUMN `sign_track_in_time` DATETIME NULL DEFAULT NULL AFTER `sign_move_out_time`,
ADD COLUMN `sign_track_out_time` DATETIME NULL DEFAULT NULL AFTER `sign_track_in_time`;



-- 20240821 zyy
INSERT INTO `sys_menu` VALUES (1534,'DRILLAMIN','数据看板',0,'build','dashboard','#',1,'',NULL,8,0,1,1,'2024-08-21 11:49:02','2024-08-21 11:54:45',1);
INSERT INTO `sys_menu` VALUES (1535,'DRILLAMIN','库位看板',1534,'#','rack','wareHouse/dashboard/rack',2,'warehouse:rackDashboard:list',NULL,2,0,1,1,'2024-08-21 11:52:39','2024-08-21 14:29:55',1);
INSERT INTO `sys_menu` VALUES (1536,'DRILLAMIN','叠板看板',1534,'#','pin','wareHouse/dashboard/pin',2,'warehouse:pinDashboard:list',NULL,2,0,1,1,'2024-08-21 11:53:47','2024-08-21 13:25:05',1);
INSERT INTO `sys_menu` VALUES (1537,'DRILLAMIN','拆板看板',1534,'#','unpin','wareHouse/dashboard/unpin',2,'wareHouse:unpinDashboard:list',NULL,3,0,1,1,'2024-08-21 14:26:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1538,'DRILLAMIN','板料料架看板',1534,'#','panelSiloShelf','wareHouse/dashboard/panelSiloShelf',2,'wareHouse:panelSiloShelfDashboard:list',NULL,4,0,1,1,'2024-08-21 14:28:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1539,'DRILLAMIN','板料叉齿看板',1534,'#','panelSiloFork','wareHouse/dashboard/panelSiloFork',2,'wareHouse:panelSiloForkDashboard:list',NULL,5,0,1,1,'2024-08-21 14:28:52','2024-08-21 14:29:02',1);
INSERT INTO `sys_menu` VALUES (1540,'DRILLAMIN','钻机看板',1534,'#','drill','device/drillDashboard/index',2,'device:drillDashboard:list',NULL,1,0,1,1,'2024-08-21 14:29:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1541,'DRILLAMIN','调度大屏',1534,'#','schedule_dashboard','device/dashboard/index',2,'device:dashboard:list',NULL,1,0,1,1,'2024-08-21 14:40:24',NULL,NULL);


-- 20240826  yp
ALTER TABLE `vg_autodrill_db`.`t_agv_rest` 
ADD COLUMN `pre_book_time` DATETIME NULL DEFAULT NULL COMMENT '预约时间' AFTER `pre_book_agv`;

ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `agv_payload_panels` MEDIUMTEXT NULL DEFAULT NULL COMMENT 'AGV板料信息' AFTER `sub_device_code`;


-- 20240827  zyy
/*
.INSERT INTO `sys_menu` VALUES (1159,'DRILLAMIN','分区设置',1158,'#','partition','wareHouse/partition/index',2,'warehouse:partition:list',NULL,7,0,1,1,'2023-04-01 02:53:42','2024-08-27 10:35:59',1);
INSERT INTO `sys_menu` VALUES (1208,'DRILLAMIN','新增',1159,'#',NULL,'#',3,'warehouse:partition:add',NULL,2,0,1,1,'2023-04-04 00:51:00','2024-08-27 10:36:17',1);
INSERT INTO `sys_menu` VALUES (1209,'DRILLAMIN','修改',1159,'#',NULL,'#',3,'warehouse:partition:edit',NULL,3,0,1,1,'2023-04-04 00:51:30','2024-08-27 10:36:23',1);
INSERT INTO `sys_menu` VALUES (1210,'DRILLAMIN','删除',1159,'#',NULL,'#',3,'warehouse:partition:remove',NULL,4,0,1,1,'2023-04-04 00:51:58','2024-08-27 10:36:28',1);
INSERT INTO `sys_menu` VALUES (1212,'DRILLAMIN','查看',1159,'#',NULL,'#',3,'warehouse:partition:view',NULL,5,0,1,1,'2023-04-04 00:52:22','2024-08-27 10:36:33',1);
*/
update sys_menu
set path='partition',menu_url='wareHouse/partition/index',authorize='warehouse:partition:list'
where id=1159;

update sys_menu
set authorize='warehouse:partition:list'
where id=1207;
update sys_menu
set authorize='warehouse:partition:add'
where id=1208;

update sys_menu
set authorize='warehouse:partition:edit'
where id=1209;

update sys_menu
set authorize='warehouse:partition:remove'
where id=1210;
update sys_menu
set authorize='warehouse:partition:reset'
where id=1211;
update sys_menu
set authorize='warehouse:partition:view'
where id=1212;

select * from sys_menu where id in (1159,1207,1208,1209,1210,1211,1212);


-- 20240828  add t_agv_transfer_task_master
CREATE TABLE `t_agv_transfer_task_master` (
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
  `transfer_task_status` int(11) NOT NULL COMMENT '转运状态',
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='agv任务列表';


-- 20240829 add t_agv_transfer_task_detail
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





-- 20240828  zyy
INSERT INTO `sys_menu` VALUES (1542,'DRILLAMIN','导入',1525,'#','','#',3,'wareHouse:agvRest:import',NULL,4,0,1,1,'2024-08-29 09:05:51','2024-08-29 09:05:58',1);
INSERT INTO `sys_menu` VALUES (1543,'DRILLAMIN','导入',1526,'#','','#',3,'wareHouse:agvRestAndPart:import',NULL,4,0,1,1,'2024-08-29 09:08:13',NULL,NULL);


--- 20240829 yp
ALTER TABLE `vg_autodrill_db`.`t_panel` 
ADD COLUMN `location_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '库位号' AFTER `panel_length`,
ADD COLUMN `silo_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '料仓号' AFTER `location_code`;

ALTER TABLE `vg_autodrill_db`.`t_silo` 
DROP COLUMN `supplier`,
ADD COLUMN `vendor_id` BIGINT(20) NULL DEFAULT NULL COMMENT '供应商ID' AFTER `silo_status`,
ADD COLUMN `vendor_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '供应商编码' AFTER `vendor_id`,
ADD COLUMN `vendor_name` VARCHAR(255) NULL DEFAULT NULL COMMENT '供应商名称' AFTER `vendor_code`;


-- 20240904 yp
ALTER TABLE `vg_autodrill_db`.`t_device_panel` 
ADD COLUMN `location_code` VARCHAR(50) NULL DEFAULT NULL AFTER `pin_offset`;

ALTER TABLE `vg_autodrill_db`.`t_device_panel_history` 
ADD COLUMN `location_code` VARCHAR(50) NULL DEFAULT NULL AFTER `pin_offset`;

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `before_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换前文件路径' AFTER `spec_group`,
ADD COLUMN `after_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换后文件路径' AFTER `before_drill_file_path`,
ADD COLUMN `is_rebrush` TINYINT(4) NULL DEFAULT '0' COMMENT '是否需要重新刷数据' AFTER `after_drill_file_path`;

ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `before_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换前文件路径' AFTER `spec_group`,
ADD COLUMN `after_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换后文件路径' AFTER `before_drill_file_path`,
ADD COLUMN `is_rebrush` TINYINT(4) NULL DEFAULT '0' COMMENT '是否需要重新刷数据' AFTER `after_drill_file_path`;

ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `before_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换前文件路径' AFTER `sign_track_out_time`,
ADD COLUMN `after_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换后文件路径' AFTER `before_drill_file_path`;

-- 20240905 yp
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


-- 20240909
ALTER TABLE `vg_autodrill_db`.`t_agv_transfer_task_master` 
RENAME TO  `vg_autodrill_db`.`t_agv_transfer_plan` ;

ALTER TABLE `vg_autodrill_db`.`t_agv_transfer_plan` 
DROP COLUMN `transfer_task_status`;

ALTER TABLE `vg_autodrill_db`.`t_agv_transfer_plan` 
ADD COLUMN `point_kind` int(11) NULL DEFAULT NULL COMMENT '交接点类型' AFTER `allocated_time`,
ADD COLUMN `scheduled_task_status` int(11) NULL DEFAULT NULL COMMENT '调度任务状态' AFTER `point_kind`;


-- 20240911
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `layer_num` INT(11) NULL DEFAULT NULL COMMENT '层数' AFTER `after_drill_file_path`;

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `layer_num` INT(11) NULL DEFAULT NULL COMMENT '层数' AFTER `is_rebrush`;

ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `layer_num` INT(11) NULL DEFAULT NULL COMMENT '层数' AFTER `is_rebrush`;

ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `layer_num` INT(11) NULL DEFAULT NULL COMMENT '层数' AFTER `panel_width`;

-- 20240912 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('QueryLayerNumList', '0', 'string', '导入指定层数的外部工单', '导入指定层数的外部工单，分隔符|', '0', '1', '1', '2024-09-11 14:08:43', '1');


-- 20240913 yp
ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `is_external` TINYINT(4) NULL DEFAULT '0' COMMENT '是否外部工单导入' AFTER `layer_num`;




-- 20240924 zyy
INSERT INTO `sys_menu` VALUES (1546,'DRILLAMIN','提交任务',1122,'#','','#',3,'produce:workorder:commit',NULL,5,0,1,1,'2024-09-24 10:52:15',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1547,'DRILLAMIN','撤销任务',1122,'#','','#',3,'produce:workorder:revoke',NULL,5,0,1,1,'2024-09-24 10:54:08',NULL,NULL);
update sys_menu
set authorize='produce:workorder:approval'
where id=1279;

-- 20240926 yp
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `is_error_data` TINYINT(4) NULL DEFAULT '0' COMMENT '是否错误数据' AFTER `layer_num`;

ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
CHANGE COLUMN `remark` `remark` VARCHAR(500) NULL DEFAULT NULL COMMENT '处理结果描述' ;

--20240926 quyang
INSERT INTO `vg_autodrill_db`.`sys_config` ( `config_code`, `config_value`, `config_type`, `config_descript`,  `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,  `is_system`) 
VALUES 
( 'IsCheckSiloUsed', 'false', 'bool', '校验料仓是否被使用',  '校验料仓是否被使用', 0, 1, 1, '2024-09-26 11:27:41', 0);

--20242930 zyy
INSERT INTO `sys_menu` VALUES (1548,'DRILLAMIN','外部工单',1024,'#','externalWorkOrder','produce/externalWorkOrder/index',2,'produce:externalWorkOrder:list',NULL,16,0,1,1,'2024-09-27 10:09:12','2024-09-27 10:22:39',1);
INSERT INTO `sys_menu` VALUES (1549,'DRILLAMIN','查看',1548,'#','','#',3,'produce:externalWorkOrder:view',NULL,2,0,1,1,'2024-09-29 09:07:06','2024-09-29 10:13:11',1);
INSERT INTO `sys_menu` VALUES (1550,'DRILLAMIN','修改',1548,'#','','#',3,'produce:externalWorkOrder:edit',NULL,3,0,1,1,'2024-09-29 09:07:24','2024-09-29 10:13:14',1);
INSERT INTO `sys_menu` VALUES (1551,'DRILLAMIN','删除',1548,'#','','#',3,'produce:externalWorkOrder:remove',NULL,4,0,1,1,'2024-09-29 09:07:40','2024-09-29 10:13:17',1);
INSERT INTO `sys_menu` VALUES (1564,'DRILLAMIN','查询',1548,'#','','#',3,'produce:externalWorkOrder:list',NULL,1,0,1,1,'2024-09-29 10:13:02',NULL,NULL);

--20240930 quyang
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`) 
VALUES 
('LoadScheduleTaskSettingDaysData', '1', 'int', '加载调度记录几天之内的数据', '加载调度记录几天之内的数据', 0, 1, 1, '2024-09-30 16:00:22',  0);

-- 20241012 xifj
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `request_summary_info` MEDIUMTEXT NULL DEFAULT NULL COMMENT '请求的汇总简略信息';

-- 20241014 xifj
INSERT INTO `vg_autodrill_db`.`sys_config` (`id`, `config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`, `is_quick_config`) 
VALUES ('31', 'AgvChargingPosition', '00030|00010', 'string', 'Agv充电点位', 'Agv充电点位，分隔符|', '0', '1', '1', '2024-09-11 14:08:43', '1', '0');


-- 20241015 yp
ALTER TABLE `vg_autodrill_db`.`sys_config` 
ADD COLUMN `category` VARCHAR(20) NULL DEFAULT '0' COMMENT '类别（0-default,1-Kinwong,2-Chognda)' AFTER `is_quick_config` ;

ALTER TABLE `vg_autodrill_db`.`sys_config` 
DROP INDEX `code_UNIQUE` ,
ADD UNIQUE INDEX `code_UNIQUE` (`config_code` ASC, `category` ASC);
;


-- 20241017 yp
ALTER TABLE `vg_autodrill_db`.`sys_config` 
CHANGE COLUMN `category` `category` VARCHAR(20) NULL DEFAULT '1' COMMENT '类别（1-default,2-Kinwong,3-Chognda)' ;

update vg_autodrill_db.sys_config set category ='1' where category ='0' and id >0;



-- 20241017 zyy
update sys_menu
set authorize='dashboard:rack:list'
where id=1535; 

update sys_menu
set authorize='dashboard:pin:list'
where id=1536;

update sys_menu
set authorize='dashboard:unpin:list'
where id=1537;

update sys_menu
set authorize='dashboard:panelSiloShelf:list'
where id=1538;

update sys_menu
set authorize='dashboard:panelSiloFork:list'
where id=1539;

update sys_menu
set authorize='dashboard:drill:list'
where id=1540;

update sys_menu
set authorize='dashboard:schedule:list'
where id=1541;     


-- 20241018 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableKinwongStockInfoQuery', 'false', 'bool', '是否开启库存信息查询', '是否开启库存信息查询', 0, 1, 1, '2024-10-18 16:00:22',  0,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('URLKinwongStockInfoQuery', 'http://10.50.222.68:8182/rcms/services/rest/hikRpcService/stockInfoQuery', 'string', '库存信息查询地址', '库存信息查询地址', 0, 1, 1, '2024-10-18 16:00:22',  0,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('StockInfoQueryTargetPosAreaCode', 'J03-DR-IN-Schedule|J04-DR-IN-Schedule', 'string', '库存信息查询区域或策略编码', '库存信息查询区域或策略编码，分隔符|', 0, 1, 1, '2024-10-18 16:00:22',  0,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('StockInfoQueryTargetPosArea', '${04}', 'string', '库存信息查询目标区域', '库存信息查询目标区域，区域编号${04}，策略编号${02}', 0, 1, 1, '2024-10-18 16:00:22',  0,'2');

ALTER TABLE `vg_autodrill_db`.`sys_config` 
DROP INDEX `code_UNIQUE` ,
ADD UNIQUE INDEX `code_UNIQUE` (`config_code` ASC);
;


-- 20241019 xifj
ALTER TABLE `vg_autodrill_db`.`t_alarm` 
ADD COLUMN `is_handled` TINYINT NULL DEFAULT 0 COMMENT '是否已处理' AFTER `event_data`,
ADD COLUMN `handle_time` DATETIME NULL COMMENT '处理时间' AFTER `is_handled`;

ALTER TABLE `vg_autodrill_db`.`t_alarm` 
ADD COLUMN `sync_id` BIGINT NULL COMMENT '时间戳，同步ID' AFTER `handle_time`;

ALTER TABLE `vg_autodrill_db`.`t_alarm` 
DROP INDEX `alarm_code_UNIQUE` ;

-- 20241021 yp
update vg_autodrill_db.t_external_work_order set remark ='处理成功：根据配置项DirectBuildTask，未自动生成排产任务', status =1
 where remark ='根据配置项DirectBuildTask，未自动生成排产任务' and id >0;


--20241022 yp
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `job_id` BIGINT(20) NULL DEFAULT NULL AFTER `other_fork_code`,
ADD COLUMN `plan_id` BIGINT(20) NULL DEFAULT NULL AFTER `job_id`;

-- 20241022 xifj
drop table t_agv_transfer_plan;
drop table t_agv_transfer_task_detail;

ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
DROP COLUMN `other_fork_code`;


-- 20241023 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('DrillCount', '10000', 'int', '孔数', '生产工单默认钻孔孔数', 0, 1, 1, '2024-10-22 16:00:22',  1,'20');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('SingleTripTime', '40', 'int', '单趟耗时 单位min', '单趟耗时 单位min', 0, 1, 1, '2024-10-22 16:00:22',  1,'20');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('ProduceTaskType', 'WadCount', 'string', '生成钻孔任务的方式：UsableCount可用叠数/WadCount计划叠数', '生成钻孔任务的方式：UsableCount可用叠数/WadCount计划叠数', 0, 1, 1, '2024-10-22 16:00:22',  1,'20');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('RefreshStock', 'true', 'bool', '是否刷新库存', '是否刷新库存', 0, 1, 1, '2024-10-22 16:00:22',  1,'20');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('AutoVettingDrillTask', 'true', 'bool', '是否自动审批钻孔任务', '是否自动审批钻孔任务', 0, 1, 1, '2024-10-22 16:00:22',  1,'20');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('ImportStatus', 'true', 'bool', '导入的数据默认状态为:可用', '导入的数据默认状态为:可用', 0, 1, 1, '2024-10-22 16:00:22',  1,'1');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('DirectBuildTask', 'false', 'bool', '导入外部工单，是否直接生成排产任务', '导入外部工单，是否直接生成排产任务', 0, 1, 1, '2024-10-22 16:00:22',  1,'1');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('DefaultInteractionPosition', '0', 'int', '钻机交互位置 0：后上料；1：前上料', '钻机交互位置 0：后上料；1：前上料', 0, 1, 1, '2024-10-22 16:00:22',  1,'30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangTransferDrillFileEnable', 'false', 'bool', '是否开启获取钻带参数', '是否开启获取钻带参数', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangTransferDrillFileUrl', 'http://192.168.102.68:5001/api/preproc/convstatus?pgm=', 'string', '获取钻带参数请求路径', '获取钻带参数请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JiangXiKinWongWIPEnable', 'false', 'bool', '是否开启导入WIP外部工单', '是否开启导入WIP外部工单', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JiangXiKinWongWIPUrl', 'http://10.50.220.20:9008/MesService.svc/kwDrWipQuery', 'string', '导入WIP外部工单请求路径', '导入WIP外部工单请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetMoveInLotEnable', 'false', 'bool', '是否开启SetMoveInLot', '是否开启SetMoveInLot', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetMoveInLotUrl', 'http://192.168.102.68:18005/api/TestJWangApiController/SignMoveInLot', 'string', 'SetMoveInLot请求路径', 'SetMoveInLot请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetMoveOutLotEnable', 'false', 'bool', '是否开启SetMoveOutLot', '是否开启SetMoveOutLot', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetMoveOutLotUrl', 'http://192.168.102.68:18005/api/TestJWangApiController/SignMoveOutLot', 'string', 'SetMoveOutLot请求路径', 'SetMoveOutLot请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetTrackInLotEnable', 'false', 'bool', '是否开启SetTrackInLot', '是否开启SetTrackInLot', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetTrackInLotUrl', 'http://192.168.102.68:18005/api/TestJWangApiController/SignTrackInLot', 'string', 'SetTrackInLot请求路径', 'SetTrackInLot请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetTrackOutLotEnable', 'false', 'bool', '是否开启SetTrackOutLot', '是否开启SetTrackOutLot', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangSetTrackOutLotUrl', 'http://192.168.102.68:18005/api/TestJWangApiController/SignTrackOutLot', 'string', 'SetTrackOutLot请求路径', 'SetTrackOutLot请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralGetRackPanelsUrl', 'http://192.168.102.178:8001/central/stat/location?code=', 'string', '获取料仓实时板料信息请求路径', '获取料仓实时板料信息请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'10');

-- 20241024 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralAllotsDeviceCommand', 'http://192.168.102.178:8001/v1/central/app/command', 'string', '下发设备指令请求路径', '下发设备指令请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralAutoAllotsPanelDataEnable', 'false', 'bool', '是否自动下发板料数据到设备', '是否自动下发板料数据到设备', 0, 1, 1, '2024-10-22 16:00:22',  1,'10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralAllotsPanelData', 'http://192.168.102.178:8001/v1/central/schedule/AllotsPanelData', 'string', '下发板料数据到设备请求路径', '下发板料数据到设备请求路径', 0, 1, 1, '2024-10-22 16:00:22',  1,'10');

-- 20241025 xifj
ALTER TABLE `vg_autodrill_db`.`t_alarm` 
CHANGE COLUMN `alarm_name` `alarm_name` MEDIUMTEXT NULL DEFAULT NULL COMMENT '告警名称' ;
-- 20241027 yp
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `is_in_plan_warehouse` VARCHAR(100) NULL DEFAULT NULL COMMENT '是否在钻孔计划仓' AFTER `is_error_data`,
ADD COLUMN `is_hold` VARCHAR(100) NULL DEFAULT NULL COMMENT '是否暂停' AFTER `is_in_plan_warehouse`;
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `is_in_plan_warehouse_remark` VARCHAR(100) NULL DEFAULT NULL COMMENT '是否在钻孔计划仓备注' AFTER `is_hold`;

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangGenAgvSchedulingTaskEnable', 'false', 'bool', '是否开启移动料仓', '是否开启移动料仓', 0, 1, 1, '2024-10-25 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangGenAgvSchedulingTaskUrl', 'http://10.50.220.20:8799/MesService.svc/GenAgvSchedulingTaskBatch', 'string', '移动料仓请求路径', '移动料仓请求路径', 0, 1, 1, '2024-10-25 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangGenAgvSchedulingTask_wbCode', 'A_01', 'string', '移动料仓：呼叫站点', '移动料仓：呼叫站点', 0, 1, 1, '2024-10-25 16:00:22',  1,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangGenAgvSchedulingTask_taskTyp', 'P011', 'string', '移动料仓：任务类型', '移动料仓：任务类型，与在RCS-2000端配置的主任务类型编号一致', 0, 1, 1, '2024-10-25 16:00:22',  1,'2');



INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CountOfEmptyForkAtLeast', '3', 'int', '叉齿分区最少空库位数量', '叉齿分区最少空库位数量，默认1', 0, 1, 1, '2024-10-25 16:00:22',  1,'10');


-- 20241028 yp
delete from vg_autodrill_db.sys_config where config_code ='CentralAutoAllotsPanelDataEnable'

-- 20211029 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralGetSimpleLocations', 'http://192.168.102.178:8001/central/stat/location/simple', 'string', '获取料仓数据请求路径', '获取料仓数据请求路径', 0, 1, 1, '2024-10-29 16:00:22',  1,'10');



-- 20211029 zyy
INSERT INTO `sys_menu` VALUES (1602,'DRILLAMIN','海康物料',1148,'','hkItem','masterData/hkItem/index',2,'masterData:hkItem:list',NULL,7,0,1,1,'2024-10-29 14:18:25','2024-10-29 14:19:29',1);

-- 20241029 xifj
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('RejectDrillMockClicker11111', 'false', 'bool', '是否禁止收取11111的熟料', '是否禁止收取11111的熟料', 0, 1, 1, '2024-10-29 20:00:22',  1,'10');

-- 20241030 xifj
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('MinDrilledTrackOutNum', '10', 'int', '中转位熟料最少自动转出数量', '中转位熟料最少自动转出数量', '0', '1', '1', '2023-11-2 11:40:10', '10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('MinFirstDrilledTrackOutNum', '10', 'int', '首件最少自动转出数量', '首件最少自动转出数量', '0', '1', '1', '2023-11-2 11:40:10', '10');


-- 20241031 xifj PartitionSetting
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('PartitionSetting', 'WH01:a,b,c;WH02:d,e,f', 'string', '料仓转运分区设置', '料仓转运分区设置，设置分区与工艺路线的关系', 0, 1, 1, '2024-10-31 16:00:22',  1,'10');
-- todo 崇达需要配置为 同一个工艺路线， 可以去两个叉齿分区 如 WH01:a,b;WH02:a,b；

-- 20241101 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableNotWIPData', 'false', 'bool', '是否显示非WIP外部工单', '是否显示非WIP外部工单', 0, 1, 1, '2024-10-31 16:00:22',  1,'2');

-- 20241101 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('StockInfoQueryWithoutAreaEnable', 'true', 'bool', '是否过滤接口无法识别的传入区域', '是否过滤接口无法识别的传入区域', 0, 1, 1, '2024-10-31 16:00:22',  1,'2');


-- 20241105 xifj
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('AgvStandbyTimeout', '10', 'int', 'Agv等待超时时长(秒)', 'Agv等待超时时长(秒)', '0', '1', '1', '2023-11-5 11:40:10', '10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralVerifyFunction01', 'false', 'bool', 'CentralVerifyFunction01', 'CentralVerifyFunction01', 0, 1, 1, '2024-11-5 00:00:22',  1,'10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralVerifyFunction02', 'false', 'bool', 'CentralVerifyFunction02', 'CentralVerifyFunction02', 0, 1, 1, '2024-11-5 00:00:22',  1,'10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralVerifyFunction03', 'false', 'bool', 'CentralVerifyFunction03', 'CentralVerifyFunction03', 0, 1, 1, '2024-11-5 00:00:22',  1,'10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralVerifyFunction04', 'false', 'bool', 'CentralVerifyFunction04', 'CentralVerifyFunction04', 0, 1, 1, '2024-11-5 00:00:22',  1,'10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CentralVerifyFunction05', 'false', 'bool', 'CentralVerifyFunction05', 'CentralVerifyFunction05', 0, 1, 1, '2024-11-5 00:00:22',  1,'10');

ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `panel_count` INT NULL DEFAULT 1 COMMENT '叠板层数';

update vg_autodrill_db.t_item i join vg_autodrill_db.t_external_work_order eo on i.code=eo.item_code set i.panel_count=eo.panel_count;

ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `hk_response` MEDIUMTEXT NULL COMMENT 'hk_response';

-- 20241106 xifj

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('MaxPartitionEmptySiloNum', '2', 'int', '最多空料仓的数量', '最多空料仓的数量', '0', '1', '1', '2023-11-6 11:40:10', '10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('MinPartitionEmptySiloNum', '1', 'int', '最少空料仓的数量', '最少空料仓的数量', '0', '1', '1', '2023-11-6 11:40:10', '10');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableSystemAnalysis', 'false', 'bool', '是否启用系统分析报告', '是否启用系统分析报告', 0, 1, 1, '2024-11-6 00:00:22',  1,'10');

-- 20241108 xifj
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('AutoTrackOutSilo', 'false', 'bool', '当料仓中没有钻机中相同的料号时是否立即转出', '当料仓中没有钻机中相同的料号时是否立即转出', 0, 1, 1, '2024-11-8 00:00:22',  1,'10');

-- 20241108 yp
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `bar_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '板料二维码' AFTER `is_in_plan_warehouse_remark`;

ALTER TABLE `vg_autodrill_db`.`t_work_order` 
ADD COLUMN `bar_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '板料二维码' AFTER `is_external`;

ALTER TABLE `vg_autodrill_db`.`t_task` 
ADD COLUMN `bar_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '板料二维码' AFTER `layer_num`;

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('XianJinSetBeginLoadPanel', 'http://192.168.102.178:8001/v1/external/xianjin/SetBeginLoadPanel', 'string', '设置上料开始请求路径', '设置上料开始请求路径', 0, 1, 1, '2024-11-07 16:00:22',  1,'4');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('XianJinUnderClinkerPanel', 'http://192.168.102.178:8001/v1/external/xianjin/UnderClinkerPanel?deviceId=', 'string', '下熟料请求路径', '下熟料请求路径', 0, 1, 1, '2024-11-07 16:00:22',  1,'4');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('XianJinSetCompleteUnderClinkerPanel', 'http://192.168.102.178:8001/v1/external/xianjin/SetCompleteUnderClinkerPanel?deviceId=', 'string', '下熟料结束请求路径', '下熟料结束请求路径', 0, 1, 1, '2024-11-07 16:00:22',  1,'4');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('XianJinSendDeviceCommand', 'false', 'bool', '中控是否下发板料校验结果、板料进度', '中控是否下发板料校验结果、板料进度', 0, 1, 1, '2024-11-08 00:00:22',  1,'4');


-- 20241110 hjp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableAgvStatusReport', 'false', 'bool', '是否生成分配任务时agv状态报告', '是否生成分配任务时agv状态报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableAssignTaskForIdleAgvsReport', 'false', 'bool', '是否生成空闲agv任务报告', '是否生成空闲agv任务报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableTrackTransferJobReport', 'false', 'bool', '是否生成转运任务报告', '是否生成转运任务报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableTryFindLocationReport', 'false', 'bool', '是否生成查找库位报告', '是否生成查找库位报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableDrillRequirementByAgvReport', 'false', 'bool', '是否生成根据agv来匹配钻机需求的报告', '是否生成根据agv来匹配钻机需求的报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableTryFindEmptyPayloadLocationReport', 'false', 'bool', '是否生成查找空库位报告', '是否生成查找空库位报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableTryFindArbitraryDrillRequiredLocationReport', 'false', 'bool', '是否生成所有钻机和匹配库位报告', '是否生成所有钻机和匹配库位报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');


-- 20241111 hjp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableAnalyzeDrillRequirementsReport', 'false', 'bool', '是否生成分析钻机Agv库位分析报告', '是否生成分析钻机Agv库位分析报告', 0, 1, 1, '2024-11-10 00:00:22',  1,'10');

-- 20241118 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangkwAGVStockInUrl', 'http://10.50.220.20:8008/MesService.svc/kwAGVStockIn', 'string', '上传mes转仓接口请求路径', '上传mes转仓接口请求路径', 0, 1, 1, '2024-11-14 16:00:22',  1,'2');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangHoldLotUrl', 'http://10.50.220.20:8008/MesService.svc/HoldLot', 'string', '上传mes暂停该lot请求路径', '上传mes暂停该lot请求路径', 0, 1, 1, '2024-11-14 16:00:22',  1,'2');

-- 20241120 yp
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

-- 20241126 quyang
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `master_device_kind` VARCHAR(100) NULL  AFTER `hk_response`,
ADD COLUMN `agv_kind` VARCHAR(100) NULL  AFTER `master_device_kind`,
ADD COLUMN `allocate_agv` VARCHAR(100) NULL  AFTER `agv_kind`;

INSERT INTO `vg_autodrill_db`.`sys_config` ( `config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,`is_system`,`category`) 
VALUES ('ChongDaGenAgvSchedulingTaskUrl', 'http://ip:port/', 'string', '下发海康任务(崇达)', '下发海康任务(崇达)', 0, 1, 1, '2024-11-26 16:35:49',  0, '30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`,  `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,`is_system`,`category`) 
VALUES ( 'ChongDaQueryAgvStatusUrl', 'http://ip:port/', 'string', '查询海康AGV状态(崇达)', '查询海康AGV状态(崇达)', 0, 1, 1, '2024-11-26 16:39:13',  0, '30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`,  `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,`is_system`,`category`) 
VALUES ('ChongDaSyncMapDatasUrl', 'http://ip:port/', 'string', '地图位置信息同步(崇达)', '地图位置信息同步(崇达)', 0, 1, 1, '2024-11-26 16:43:17',  0, '30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`,  `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,`is_system`,`category`) 
VALUES ('ChongDaQueryTaskStatusUrl', 'http://ip:port/', 'string', '查询任务状态(崇达)', '查询任务状态(崇达)', 0, 1, 1, '2024-11-26 16:45:12',  0, '30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`,  `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,`is_system`,`category`) 
VALUES ('ChongDaContinueTaskUrl', 'http://ip:port/', 'string', '继续执行任务(崇达)', '继续执行任务(崇达)', 0, 1, 1, '2024-11-26 16:46:44',  0, '30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`,  `remark`, `is_deleted`, `status`, `creator_id`, `create_time`,`is_system`,`category`) 
VALUES ('ChongDaCancelTaskUrl', 'http://ip:port/', 'string', '取消任务(崇达)', '取消任务(崇达)', 0, 1, 1, '2024-11-26 16:47:53',  0, '30');

-- 20241128 yp
ALTER TABLE `vg_autodrill_db`.`t_schedule_log` 
ADD COLUMN `spindle` INT(11) NULL COMMENT '轴号' AFTER `message`;

--20241201 quyang
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `hk_response_key` VARCHAR(100) NULL  AFTER `allocate_agv`;
--20250218 hjp
ALTER TABLE vg_autodrill_db.t_transportation_task MODIFY COLUMN hk_response_key varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '海康任务号';


-- 20241202 yp
CREATE TABLE `t_device_records` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_code` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '设备编号',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `work_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作时间',
  `wait_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '等待时间',
  `error_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '异常时间',
   `open_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '开机时间',
  `duty` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '稼动率',
  `end_to_start_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '结束到开始总时间',
   `collect_clear_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '清洗夹头总时间',
  `date_string` varchar(100) CHARACTER SET utf8 DEFAULT NULL COMMENT '数据日期',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备记录表';



--20241201 zyy
INSERT INTO `sys_menu` VALUES (1604,'DRILLAMIN','设置紧急',1037,'#','','#',3,'device:schedulement:setUrgent',NULL,8,0,1,1,'2024-12-02 09:20:15',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1605,'DRILLAMIN','导出',1037,'#','','#',3,'device:schedulement:download',NULL,9,0,1,1,'2024-12-02 09:20:35',NULL,NULL);


-- 20241202 hjp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnablePropertiesFlusher', 'false', 'bool', '是否开启定时保存设备属性信息', '是否开启定时保存设备属性信息', 0, 1, 1, '2024-12-02 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('PropertiesFlusherInterval', '10', 'int', '定时保存设备属性信息时间间隔', '定时保存设备属性信息时间间隔(单位分钟)', 0, 1, 1, '2024-12-02 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnablePanelFlusher', 'false', 'bool', '是否开启定时保存设备板料信息', '是否开启定时保存设备板料信息', 0, 1, 1, '2024-12-02 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('PanelFlusherInterval', '10', 'int', '定时保存设备板料信息时间间隔', '定时保存设备板料信息时间间隔(单位分钟)', 0, 1, 1, '2024-12-02 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('EnableCutterFlusher', 'false', 'bool', '是否开启定时保存设备刀具信息', '是否开启定时保存设备刀具信息', 0, 1, 1, '2024-12-02 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('CutterFlusherInterval', '10', 'int', '定时保存设备刀具信息时间间隔', '定时保存设备刀具信息时间间隔(单位分钟)', 0, 1, 1, '2024-12-02 00:00:22',  1,'10');

-- 20241203 quyang
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `fork_location_schedule_id` VARCHAR(100) NULL  AFTER `hk_response_key`;



-- 20241203 zyy 设备报表、休息点列表权限
INSERT INTO `sys_menu` VALUES (1606,'DRILLAMIN','设备报表',1026,'#','deviceRecords','device/deviceRecords/index',2,'device:deviceRecords:list',NULL,1,0,1,1,'2024-12-03 09:06:02','2024-12-03 10:09:53',1);
INSERT INTO `sys_menu` VALUES (1607,'DRILLAMIN','导出',1606,'#','','#',3,'device:deviceRecords:export',NULL,2,0,1,1,'2024-12-03 11:22:21','2024-12-03 11:25:32',1);
INSERT INTO `sys_menu` VALUES (1608,'DRILLAMIN','查询',1606,'#','','#',3,'device:deviceRecords:list',NULL,1,0,1,1,'2024-12-03 11:23:06',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1609,'DRILLAMIN','启用',1525,'#','','#',3,'warehouse:agvRest:enabled',NULL,5,0,1,1,'2024-12-03 14:28:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1610,'DRILLAMIN','禁用',1525,'#','','#',3,'warehouse:agvRest:disabled',NULL,6,0,1,1,'2024-12-03 14:28:59',NULL,NULL);


-- 20241204 yp
ALTER TABLE `vg_autodrill_db`.`t_warehouse` 
ADD COLUMN `partition_kind` tinyint(4) NOT NULL COMMENT '分区类别,默认0未知，1私有分区，2公共分区' AFTER `pre_book_time`;

-- 20241204 hjp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('WipMinDrilledTrackOutNum', '10', 'int', '线边仓熟料最少自动转出数量', '线边仓熟料最少自动转出数量', '0', '1', '1', '2023-11-2 11:40:10', '10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('WipAutoTrackOutSilo', 'false', 'bool', '当线边仓料仓中没有钻机中相同的料号时是否立即转出', '当线边仓料仓中没有钻机中相同的料号时是否立即转出', 0, 1, 1, '2024-11-8 00:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('WipMaxPartitionEmptySiloNum', '2', 'int', '线边仓最多空料仓的数量', '线边仓最多空料仓的数量', '0', '1', '1', '2023-11-6 11:40:10', '10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('WipMinPartitionEmptySiloNum', '1', 'int', '线边仓最少空料仓的数量', '线边仓最少空料仓的数量', '0', '1', '1', '2023-11-6 11:40:10', '10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('WipCountOfEmptyForkAtLeast', '3', 'int', '线边仓最少空库位数量', '线边仓最少空库位数量，默认1', 0, 1, 1, '2024-10-25 16:00:22',  1,'10');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('WipMinFirstDrilledTrackOutNum', '10', 'int', '线边仓首件最少自动转出数量', '线边仓首件最少自动转出数量', '0', '1', '1', '2023-11-2 11:40:10', '10');

-- 20241205 hjp
ALTER TABLE vg_autodrill_db.t_transportation_task ADD start_location_code varchar(100) NULL COMMENT '起始库位';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD start_device_id varchar(100) NULL COMMENT '起始设备id';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD start_schedule_id BIGINT NULL COMMENT '起始调度id';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD end_location_code varchar(100) NULL COMMENT '终点库位';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD end_device_id varchar(100) NULL COMMENT '终点设备id';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD end_schedule_id BIGINT NULL COMMENT '终点调度id';

-- 20241205 yp
update t_warehouse set partition_kind =1 where code ='000';
update t_warehouse set partition_kind =1 where code ='PartPin';
update t_warehouse set partition_kind =1 where code ='PartPanelShelf-Raw';
update t_warehouse set partition_kind =2 where code ='PartUnpin-001';
update t_warehouse set partition_kind =2 where code ='PartPanelFork-Clinker';
update t_warehouse set partition_kind =0 where partition_kind is null and id >0;

-- 20241205 quyang
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `device_location_schedule_id` VARCHAR(100) NULL;

-- 20241206 yp
ALTER TABLE vg_autodrill_db.t_transportation_task ADD transfer_behavior INT NULL COMMENT '转移行为';

ALTER TABLE `vg_autodrill_db`.`t_schedule` 
CHANGE COLUMN `remark` `remark` MEDIUMTEXT CHARACTER SET 'utf8' NULL DEFAULT NULL COMMENT '备注' ;


--- 20241209 zyy
INSERT INTO `sys_menu` VALUES (1611,'DRILLAMIN','新增',1514,'#','','#',3,'device:transportationTask:add',NULL,2,0,1,59,'2024-12-09 09:35:50','2024-12-09 09:36:46',59);
INSERT INTO `sys_menu` VALUES (1612,'DRILLAMIN','删除',1514,'#','','#',3,'device:transportationTask:remove',NULL,4,0,1,59,'2024-12-09 09:36:08','2024-12-09 09:36:49',59);
INSERT INTO `sys_menu` VALUES (1613,'DRILLAMIN','查看',1514,'#','','#',3,'device:transportationTask:view',NULL,1,0,1,59,'2024-12-09 09:36:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1614,'DRILLAMIN','日志详情',1514,'#','','#',3,'device:transportationTask:detail',NULL,5,0,1,59,'2024-12-09 09:37:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1615,'DRILLAMIN','修改',1514,'#','','#',3,'device:transportationTask:edit',NULL,3,0,1,59,'2024-12-09 09:37:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1616,'DRILLAMIN','取消',1514,'#','','#',3,'device:transportationTask:cancel',NULL,6,0,1,59,'2024-12-09 09:38:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1617,'DRILLAMIN','查询',1514,'#','','#',3,'device:transportationTask:list',NULL,1,0,1,59,'2024-12-09 09:38:26',NULL,NULL);

-- 20241211 hjp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('UndrilledItemOccupyLocationIdleTimeout', '30', 'int', '生料占用库位超时时间时长(秒)', '生料占用库位超时时间时长(秒)', '0', '1', '1', '2023-11-5 11:40:10', '10');

-- 20241217 yp
ALTER TABLE `vg_autodrill_db`.`t_transportation_task` 
ADD COLUMN `is_manual` TINYINT(4) NULL DEFAULT '0' COMMENT '是否手动创建' AFTER `transfer_behavior`,
ADD COLUMN `clinker_remark` VARCHAR(100) NULL DEFAULT NULL COMMENT '熟料备注' AFTER `is_manual`;





-- 20241217 quyang
ALTER TABLE t_schedule MODIFY COLUMN request_interaction_behavior_name VARCHAR(500) DEFAULT NULL COMMENT '交互方式描述';

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('MonitorTaskCompleteTimeOut', '20', 'int', '监控任务超时时间', '监控任务完成超时时间(单位分钟)', 0, 1, 1, '2024-12-17 11:23:00',  1,'30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('MonitorTaskFailTimeOut', '20', 'int', '监控任务超时时间', '监控任务完成超时时间(单位分钟)', 0, 1, 1, '2024-12-17 11:23:00',  1,'30');

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('HiktaskTyp', 'WJ02', 'string', '海康接口任务类型模板', '海康接口任务类型模板', 0, 1, 1, '2024-12-17 11:23:00',  1,'30');

-- 20241219 xifj
-- PropertiesFlusherAllowedDevices
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('PropertiesFlusherAllowedDevices', '', 'string', '允许属性上报试运行的设备列表', '允许属性上报试运行的设备列表，none-限制；空白时，相当于禁用属性上报；drill01,drill02, 用逗号分割符号，分割多个设备列表', 0, 1, 1, '2024-12-19 16:48:00',  1,'10');

ALTER TABLE `vg_autodrill_db`.`sys_config` 
CHANGE COLUMN `config_value` `config_value` VARCHAR(1000) NOT NULL COMMENT '配置项的值' ;

-- 20241222 xifj 调度日志，增加索引
alter table t_schedule_log add index (master_id);
alter table t_transportation_task_log add index (master_id);

-- 20241223 yp
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `is_send_move_info` TINYINT(4) NULL DEFAULT '0' COMMENT '是否下发转仓命令' AFTER `bar_code`;



-- 20241230 zyy
INSERT INTO `sys_menu` VALUES (1620,'DRILLAMIN','Mes转仓',1548,'#','','#',3,'produce:externalWorkOrder:kwAGVStockIn',NULL,5,0,1,1,'2024-12-30 17:04:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1621,'DRILLAMIN','Mes暂停',1548,'#','','#',3,'produce:externalWorkOrder:kwHold',NULL,6,0,1,1,'2024-12-30 17:04:43',NULL,NULL);


-- 20250103 yp
CREATE TABLE `t_device_records_summary` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_code` varchar(200) CHARACTER SET utf8 NOT NULL COMMENT '设备编号',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `work_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '工作时间',
  `wait_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '等待时间',
  `error_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '异常时间',
  `open_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '开机时间',
  `duty` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '稼动率',
  `end_to_start_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '结束到开始总时间',
  `collect_clear_time` varchar(50) CHARACTER SET utf8 DEFAULT NULL COMMENT '清洗夹头总时间',
  `date_string` varchar(100) CHARACTER SET utf8 DEFAULT NULL COMMENT '数据日期',
  `sailings` int(11) NOT NULL DEFAULT '0' COMMENT '班次(0-白班，1-中班，2-夜班)',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备记录汇总表';

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('Sailings', '0-8:00|2-20:00', 'string', '班次', '班次(0-白班开始时间|1-中班开始时间|2-夜班开始时间) 用|分割符号，分割多个班次', 0, 1, 1, '2025-01-02 16:48:00',  1,'1');






INSERT INTO `sys_menu` VALUES (1621,'DRILLAMIN','Mes暂停',1548,'#','','#',3,'produce:externalWorkOrder:kwHold',NULL,6,0,1,1,'2024-12-30 17:04:43',NULL,NULL);

--- 20250103
INSERT INTO `sys_menu` VALUES (1623,'DRILLAMIN','看板按钮',0,'#','','#',3,'',NULL,1,0,1,1,'2025-01-02 15:49:36','2025-01-03 10:00:08',1);
INSERT INTO `sys_menu` VALUES (1624,'DRILLAMIN','钻机-启用禁用',1623,'#','','#',3,'dashboard:drill:disabled',NULL,1,0,1,1,'2025-01-02 15:57:46','2025-01-03 09:53:43',1);
INSERT INTO `sys_menu` VALUES (1625,'DRILLAMIN','叠板-启用禁用',1623,'#','','#',3,'dashboard:pin:disabled',NULL,2,0,1,1,'2025-01-02 15:59:31','2025-01-03 09:53:50',1);
INSERT INTO `sys_menu` VALUES (1626,'DRILLAMIN','库位-启用禁用',1623,'#','','#',3,'dashboard:rack:disabled',NULL,3,0,1,1,'2025-01-02 16:03:34','2025-01-03 09:54:13',1);
INSERT INTO `sys_menu` VALUES (1627,'DRILLAMIN','拆板-启用禁用',1623,'#','','#',3,'dashboard:unpin:disabled',NULL,4,0,1,1,'2025-01-02 16:38:46','2025-01-03 09:54:36',1);
INSERT INTO `sys_menu` VALUES (1628,'DRILLAMIN','板料料架-启用禁用',1623,'#','','#',3,'dashboard:panelSiloShelf:disabled',NULL,5,0,1,1,'2025-01-02 16:38:58','2025-01-03 09:55:13',1);
INSERT INTO `sys_menu` VALUES (1629,'DRILLAMIN','板料叉齿-启用禁用',1623,'#','','#',3,'dashboard:panelSiloFork:disabled',NULL,6,0,1,1,'2025-01-02 16:39:09','2025-01-03 09:55:40',1);
INSERT INTO `sys_menu` VALUES (1630,'DRILLAMIN','叠板-操作板料',1623,'#','','#',3,'dashboard:pin:change',NULL,2,0,1,1,'2025-01-03 09:46:43','2025-01-03 09:53:57',1);
INSERT INTO `sys_menu` VALUES (1631,'DRILLAMIN','库位-操作板料',1623,'#','','#',3,'dashboard:rack:change',NULL,3,0,1,1,'2025-01-03 09:47:31','2025-01-03 09:54:19',1);
INSERT INTO `sys_menu` VALUES (1632,'DRILLAMIN','拆板-操作板料',1623,'#','','#',3,'dashboard:unpin:change',NULL,4,0,1,1,'2025-01-03 09:48:46','2025-01-03 09:55:03',1);
INSERT INTO `sys_menu` VALUES (1633,'DRILLAMIN','板料料架-操作板料',1623,'#','','#',3,'dashboard:panelSiloShelf:change',NULL,5,0,1,1,'2025-01-03 09:49:22','2025-01-03 09:55:24',1);
INSERT INTO `sys_menu` VALUES (1634,'DRILLAMIN','板料叉齿-操作板料',1623,'#','','#',3,'dashboard:panelSiloFork:change',NULL,6,0,1,1,'2025-01-03 09:49:46','2025-01-03 09:57:08',1);
INSERT INTO `sys_menu` VALUES (1635,'DRILLAMIN','叠板-重新上报',1623,'#','','#',3,'dashboard:pin:report',NULL,2,0,1,1,'2025-01-03 09:52:20','2025-01-03 09:59:09',1);
INSERT INTO `sys_menu` VALUES (1636,'DRILLAMIN','库位-重新上报',1623,'#','','#',3,'dashboard:rack:report',NULL,3,0,1,1,'2025-01-03 09:52:34','2025-01-03 09:58:57',1);
INSERT INTO `sys_menu` VALUES (1637,'DRILLAMIN','拆板-重新上报',1623,'#','','#',3,'dashboard:unpin:report',NULL,4,0,1,1,'2025-01-03 09:52:52','2025-01-03 09:58:41',1);
INSERT INTO `sys_menu` VALUES (1638,'DRILLAMIN','板料料架-重新上报',1623,'#','','#',3,'dashboard:panelSiloShelf:report',NULL,5,0,1,1,'2025-01-03 09:53:11','2025-01-03 09:58:28',1);
INSERT INTO `sys_menu` VALUES (1639,'DRILLAMIN','板料叉齿-重新上报',1623,'#','','#',3,'dashboard:panelSiloFork:report',NULL,6,0,1,1,'2025-01-03 09:57:35','2025-01-03 09:58:03',1);

INSERT INTO `sys_menu` VALUES (1640,'DRILLAMIN','导出',1037,'#','','#',3,'device:schedulement:export',NULL,1,0,1,1,'2025-01-03 11:15:52',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1641,'DRILLAMIN','异常处理',1037,'#','','#',3,'device:schedulement:abnormal',NULL,9,0,1,1,'2025-01-03 11:17:59','2025-01-03 11:18:16',1);
INSERT INTO `sys_menu` VALUES (1642,'DRILLAMIN','明细新增',1317,'#','','#',3,'device:repairDetails:add',NULL,6,0,1,1,'2025-01-03 11:20:32',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1643,'DRILLAMIN','明细删除',1317,'#','','#',3,'device:repairDetails:remove',NULL,7,0,1,1,'2025-01-03 11:21:11',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1644,'DRILLAMIN','明细修改',1317,'#','','#',3,'device:repairDetails:edit',NULL,8,0,1,1,'2025-01-03 11:21:28',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1645,'DRILLAMIN','导入',1035,'#','','#',3,'deevice:device:import',NULL,1,0,1,1,'2025-01-03 11:27:34',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1646,'DRILLAMIN','详情',1035,'#','','#',3,'device:device:detail',NULL,8,0,1,1,'2025-01-03 11:28:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1647,'DRILLAMIN','详情-同步',1035,'#','','#',3,'device:device:synchronousData',NULL,9,0,1,1,'2025-01-03 11:29:21',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1648,'DRILLAMIN','详情-下发',1035,'#','','#',3,'device:device:issued',NULL,10,0,1,1,'2025-01-03 11:29:44',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1649,'DRILLAMIN','详情-生成',1035,'#','','#',3,'device:device:batchAdd',NULL,11,0,1,1,'2025-01-03 11:30:16','2025-01-03 11:30:45',1);
INSERT INTO `sys_menu` VALUES (1650,'DRILLAMIN','详情-清空',1035,'#','','#',3,'device:device:batchClear',NULL,12,0,1,1,'2025-01-03 11:30:38',NULL,NULL)

INSERT INTO `sys_menu` VALUES (1651,'DRILLAMIN','配置路线',1153,'#','','#',3,'masterData:workstation:config',NULL,9,0,1,1,'2025-01-03 13:17:53',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1652,'DRILLAMIN','合格/不合格',1314,'#','','#',3,'produce:checkrecords:changeCheck',NULL,4,0,1,1,'2025-01-03 14:35:38',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1653,'DRILLAMIN','再次生成',1031,'#','','#',3,'	 produce:drillWorkOrder:again',NULL,11,0,1,1,'2025-01-03 14:37:38',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1654,'DRILLAMIN','查看任务',1029,'#','','#',3,'produce:schedule:viewTask',NULL,7,0,1,1,'2025-01-03 14:42:41',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1655,'DRILLAMIN','重排计划',1419,'#','','#',3,'produce:tasks:batchRationalizeTask',NULL,1,0,1,1,'2025-01-03 14:44:04',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1656,'DRILLAMIN','重排计划',1472,'#','','#',3,'produce:ticketCharts:batchRationalizeTask',NULL,1,0,1,1,'2025-01-03 14:46:51',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1657,'DRILLAMIN','禁用',1159,'#','','#',3,'warehouse:partition:disabled',NULL,6,0,1,1,'2025-01-03 15:29:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1658,'DRILLAMIN','启用',1159,'#','','#',3,'warehouse:partition:enabled',NULL,7,0,1,1,'2025-01-03 15:30:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1659,'DRILLAMIN','查看',1485,'#','','#',3,'warehouse:rackManage:view',NULL,1,0,1,1,'2025-01-03 15:31:59',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1660,'DRILLAMIN','设置手动',1484,'#','','#',3,'warehouse:siloManage:setManual',NULL,9,0,1,1,'2025-01-03 15:35:20',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1661,'DRILLAMIN','设置就绪',1484,'#','','#',3,'warehouse:siloManage:setReady',NULL,10,0,1,1,'2025-01-03 15:35:42',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1662,'DRILLAMIN','同步',1485,'#','','#',3,'warehouse:rackManage:sync',NULL,11,0,1,1,'2025-01-03 15:40:27',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1663,'DRILLAMIN','生成板料',1485,'#','','#',3,'warehouse:rackManage:batchAdd',NULL,12,0,1,1,'2025-01-03 15:40:50',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1664,'DRILLAMIN','清空板料',1485,'#','','#',3,'warehouse:rackManage:batchClear',NULL,13,0,1,1,'2025-01-03 15:42:00',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1665,'DRILLAMIN','移除料仓',1485,'#','','#',3,'warehouse:rackManage:delSilo',NULL,14,0,1,1,'2025-01-03 15:42:43',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1666,'DRILLAMIN','下发',1485,'#','','#',3,'warehouse:rackManage:issued',NULL,15,0,1,1,'2025-01-03 15:43:14',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1667,'DRILLAMIN','设置Reday',1485,'#','','#',3,'warehouse:rackManage:setReday',NULL,16,0,1,1,'2025-01-03 15:43:47',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1668,'DRILLAMIN','绑定料仓',1485,'#','','#',3,'warehouse:rackManage:setSilo',NULL,17,0,1,1,'2025-01-03 15:45:19',NULL,NULL);


-- 20250106 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangTransferDrillFileEnable_WorkOrder', 'false', 'bool', '人工录入工单是否开启获取钻带参数', '人工录入工单是否开启获取钻带参数', 0, 1, 1, '2025-1-6 16:00:22',  1,'2');

-- 20250109 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('XianJinSendCommandDelay', '1', 'int', '先进下发完成上料信息，延迟时长（秒）', '先进下发完成上料信息，延迟时长（秒）', 0, 1, 1, '2025-1-9 00:00:22',  1,'4');

-- 20250113 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('JingWangVerifyItemNum_WorkOrder', 'false', 'bool', '添加工单，校验物料数量是否超过自动工单的数量', '添加工单，校验物料数量是否超过自动工单的数量', 0, 1, 1, '2025-1-13 10:40:22',  1,'2');

-- 20250113 xifj
alter table t_device_records add index (create_time);
alter table t_device_records_summary add index (create_time);
alter table t_device_records add index (device_code);
alter table t_device_records_summary add index (device_code);


-- 20250117 zyy
INSERT INTO `sys_menu` VALUES (1669,'DRILLAMIN','AGV看板',1534,'#','agv','wareHouse/dashboard/agv',2,'dashboard:agv:list',NULL,1,0,1,1,'2025-01-16 14:34:52','2025-01-16 14:35:28',1);
INSERT INTO `sys_menu` VALUES (1670,'DRILLAMIN','AGV看板',1158,'#','agvDashboard','wareHouse/dashboard/agv',2,'warehouse:agvDashboard:list',NULL,1,0,1,1,'2025-01-17 16:14:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1671,'DRILLAMIN','AGV-操作按钮',1623,'#','','#',3,'ashboard:agv:change',NULL,7,0,1,1,'2025-01-17 16:44:29','2025-01-17 16:44:34',1);
INSERT INTO `sys_menu` VALUES (1672,'DRILLAMIN','AGV-重新上报',1623,'#','','#',3,'dashboard:agv:report',NULL,7,0,1,1,'2025-01-17 16:45:04',NULL,NULL);

-- 20250120 zyy
update sys_menu
set menu_type=3
where id=1623; 


-- 20250205 hjp
ALTER TABLE vg_autodrill_db.t_alarm ADD alarm_kind INT NULL COMMENT '告警分类';
ALTER TABLE vg_autodrill_db.t_alarm ADD alarm_content VARCHAR(500) NULL COMMENT '告警内容';
ALTER TABLE vg_autodrill_db.t_alarm ADD token varchar(100) NULL COMMENT '处理令牌';


-- 2025-02-07  zyy
INSERT INTO `sys_menu` VALUES (1673,'DRILLAMIN','稼动率',0,'utilizationRate','utilizationRate','#',1,'',NULL,3,0,1,1,'2025-02-07 11:07:58','2025-02-07 11:15:04',1);
INSERT INTO `sys_menu` VALUES (1674,'DRILLAMIN','标准稼动率',1673,'#','standard','utilizationRate/standard',2,'utilizationRate:standard:list',NULL,1,0,1,1,'2025-02-07 11:09:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1675,'DRILLAMIN','查询',1674,'#','','#',3,'utilizationRate:standard:list',NULL,1,0,1,1,'2025-02-07 11:16:06',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1676,'DRILLAMIN','导出',1674,'#','','#',3,'utilizationRate:standard:export',NULL,2,0,1,1,'2025-02-07 11:16:23',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1677,'DRILLAMIN','查看',1674,'#','','#',3,'utilizationRate:standard:view',NULL,3,0,1,1,'2025-02-07 11:17:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1678,'DRILLAMIN','查看',1606,'#','','#',3,'device:deviceRecords:view',NULL,3,0,1,1,'2025-02-07 11:17:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1679,'DRILLAMIN','实时稼动率',1673,'#','realTime','utilizationRate/realTime',2,'utilizationRate:realTime:list',NULL,2,0,1,1,'2025-02-07 11:46:31',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1680,'DRILLAMIN','班次稼动率',1673,'#','sailings','utilizationRate/sailings',2,'utilizationRate:sailings:list',NULL,3,0,1,1,'2025-02-07 11:47:19',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1681,'DRILLAMIN','稼动率分析',1673,'#','analysis','utilizationRate/analysis',2,'utilizationRate:analysis:list',NULL,4,0,1,1,'2025-02-07 11:48:00',NULL,NULL);


-- 20250208 yp
alter table t_device_panel_history add index (location_code);

-- 20250211 yp
CREATE TABLE `t_drill_rate_factor` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `device_id` varchar(50) NOT NULL COMMENT '设备编号',
  `reason` varchar(500) DEFAULT NULL COMMENT '原因',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  PRIMARY KEY (`id`),
  KEY `device_id` (`device_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='钻机稼动率因素表';


ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `without_task_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '无任务时长' AFTER `sailings`;

-- 20250212 xifj
alter table t_drill_rate_factor add index (start_time);
alter table t_drill_rate_factor add index (end_time);

-- 20250214 yp
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `tool_life_expored_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '刀具异常时长' AFTER `without_task_time`,
ADD COLUMN `without_drill_file_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '没有加载钻带参数时长' AFTER `tool_life_expored_time`,
ADD COLUMN `without_panel_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '无板料生产时长' AFTER `without_drill_file_time`,
ADD COLUMN `alarm_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '设备异常时长' AFTER `without_panel_time`,
ADD COLUMN `run_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '设备运行时长' AFTER `alarm_time`,
ADD COLUMN `buffer_no_board_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'Buffer无生料时长' AFTER `run_time`,
ADD COLUMN `buffer_clinker_exist_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'Buffer有熟料时长' AFTER `buffer_no_board_time`,
ADD COLUMN `device_disable_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '设备禁用时长' AFTER `buffer_clinker_exist_time`;

-- 20250215 yp
ALTER TABLE `vg_autodrill_db`.`t_drill_rate_factor` 
ADD COLUMN `location_code` VARCHAR(50) NULL DEFAULT NULL COMMENT '库位编码' AFTER `modifier_id`,
ADD COLUMN `memo` MEDIUMTEXT NULL DEFAULT NULL COMMENT '备注' AFTER `location_code`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `buffer_raw_complete_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'Buffer有板到上板完成时长' AFTER `device_disable_time`,
ADD COLUMN `buffer_raw_exist_to_run_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'Buffer上板完成到开始打板时长' AFTER `buffer_raw_complete_time`,
ADD COLUMN `buffer_automatic_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'Buffer自动时长' AFTER `buffer_raw_exist_to_run_time`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `drill_board_direction_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '板方向检测时长' AFTER `buffer_automatic_time`,
ADD COLUMN `drill_test_pin_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'pin检测时长' AFTER `drill_board_direction_time`,
ADD COLUMN `drill_tool_evaluation_time` VARCHAR(50) NULL DEFAULT NULL COMMENT '刀具检测时长' AFTER `drill_test_pin_time`;

-- 20250216 xifj
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD COLUMN `cancel_reason` MEDIUMTEXT NULL DEFAULT NULL COMMENT '取消原因',
ADD COLUMN `is_barcode_ok` TINYINT(1) NULL COMMENT '板料检验是否OK';

-- 20250217 yp
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `buffer_manual_time` VARCHAR(50) NULL DEFAULT NULL COMMENT 'Buffer手动时长' AFTER `drill_tool_evaluation_time`;

-- 20250218 yp
ALTER TABLE `vg_autodrill_db`.`t_external_work_order` 
ADD COLUMN `lot_stock_num` DECIMAL(20,0) NULL DEFAULT NULL COMMENT '物料库存数量' AFTER `is_send_move_info`;

-- 20250219 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('StockInfoQueryDrillAreaCode', 'J03-DR-IN-Schedule', 'string', '库存信息钻孔区域编码', '库存信息钻孔区域编码', 0, 1, 1, '2025-2-19 16:00:22',  0,'2');

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
CHANGE COLUMN `work_time` `work_time` INT(11) NULL DEFAULT NULL COMMENT '工作时间' ,
CHANGE COLUMN `wait_time` `wait_time` INT(11) NULL DEFAULT NULL COMMENT '等待时间' ,
CHANGE COLUMN `error_time` `error_time` INT(11) NULL DEFAULT NULL COMMENT '异常时间' ,
CHANGE COLUMN `open_time` `open_time` INT(11) NULL DEFAULT NULL COMMENT '开机时间' ,
CHANGE COLUMN `end_to_start_time` `end_to_start_time` INT(11) NULL DEFAULT NULL COMMENT '结束到开始总时间' ,
CHANGE COLUMN `collect_clear_time` `collect_clear_time` INT(11) NULL DEFAULT NULL COMMENT '清洗夹头总时间' ,
CHANGE COLUMN `without_task_time` `without_task_time` INT(11) NULL DEFAULT NULL COMMENT '无任务时长' ,
CHANGE COLUMN `tool_life_expored_time` `tool_life_expored_time` INT(11) NULL DEFAULT NULL COMMENT '刀具异常时长' ,
CHANGE COLUMN `without_drill_file_time` `without_drill_file_time` INT(11) NULL DEFAULT NULL COMMENT '没有加载钻带参数时长' ,
CHANGE COLUMN `without_panel_time` `without_panel_time` INT(11) NULL DEFAULT NULL COMMENT '无板料生产时长' ,
CHANGE COLUMN `alarm_time` `alarm_time` INT(11) NULL DEFAULT NULL COMMENT '设备异常时长' ,
CHANGE COLUMN `run_time` `run_time` INT(11) NULL DEFAULT NULL COMMENT '设备运行时长' ,
CHANGE COLUMN `buffer_no_board_time` `buffer_no_board_time` INT(11) NULL DEFAULT NULL COMMENT 'Buffer无生料时长' ,
CHANGE COLUMN `buffer_clinker_exist_time` `buffer_clinker_exist_time` INT(11) NULL DEFAULT NULL COMMENT 'Buffer有熟料时长' ,
CHANGE COLUMN `device_disable_time` `device_disable_time` INT(11) NULL DEFAULT NULL COMMENT '设备禁用时长' ,
CHANGE COLUMN `buffer_raw_complete_time` `buffer_raw_complete_time` INT(11) NULL DEFAULT NULL COMMENT 'Buffer有板到上板完成时长' ,
CHANGE COLUMN `buffer_raw_exist_to_run_time` `buffer_raw_exist_to_run_time` INT(11) NULL DEFAULT NULL COMMENT 'Buffer上板完成到开始打板时长' ,
CHANGE COLUMN `buffer_automatic_time` `buffer_automatic_time` INT(11) NULL DEFAULT NULL COMMENT 'Buffer自动时长' ,
CHANGE COLUMN `drill_board_direction_time` `drill_board_direction_time` INT(11) NULL DEFAULT NULL COMMENT '板方向检测时长' ,
CHANGE COLUMN `drill_test_pin_time` `drill_test_pin_time` INT(11) NULL DEFAULT NULL COMMENT 'pin检测时长' ,
CHANGE COLUMN `drill_tool_evaluation_time` `drill_tool_evaluation_time` INT(11) NULL DEFAULT NULL COMMENT '刀具检测时长' ,
CHANGE COLUMN `buffer_manual_time` `buffer_manual_time` INT(11) NULL DEFAULT NULL COMMENT 'Buffer手动时长' ;

-- 20250220 yp
ALTER TABLE `vg_autodrill_db`.`t_alarm` 
ADD COLUMN `location_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '库位编码' AFTER `token`,
ADD COLUMN `partition_code` VARCHAR(100) NULL DEFAULT NULL COMMENT '区域编码' AFTER `location_code`;

-- 20250221 yp
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('RateRecordSummaryRetainTime', '6', 'int', '稼动率汇总表数据，保留时长（单位：月）', '稼动率汇总表数据，保留时长（单位：月）', 0, 1, 1, '2025-2-21 10:24:22',  0,'2');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('RateFactorRetainTime', '1', 'int', '稼动率明细数据，保留时长（单位：月）', '稼动率明细数据，保留时长（单位：月）', 0, 1, 1, '2025-2-21 10:24:22',  0,'2');

-- 20250224 yp
alter table t_work_order_alter_log add index (work_order_code);
alter table t_work_order_alter_log add index (device_code);
alter table t_work_order_alter_log add index (item_code);

ALTER TABLE `vg_autodrill_db`.`t_item` 
ADD COLUMN `spec_group` VARCHAR(100) NULL DEFAULT NULL COMMENT '工艺分组' AFTER `panel_count`,
ADD COLUMN `before_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换前钻带路径' AFTER `spec_group`,
ADD COLUMN `after_drill_file_path` VARCHAR(500) NULL DEFAULT NULL COMMENT '转换后钻带路径' AFTER `before_drill_file_path`;

update vg_autodrill_db.t_item a
left join vg_autodrill_db.t_external_work_order b on a.code = b.item_code
set a.spec_group = b.spec_group,
 a.before_drill_file_path=b.before_drill_file_path,a.after_drill_file_path=b.after_drill_file_path
where a.spec_group is null and a.id>0;

-- 20250225 yp
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `theory_duty` INT(11) NULL DEFAULT NULL COMMENT '理论稼动率' AFTER `buffer_manual_time`,
ADD COLUMN `duty_rate` INT(11) NULL DEFAULT NULL COMMENT '稼动率达成率' AFTER `theory_duty`,
CHANGE COLUMN `duty` `duty` INT(11) NULL DEFAULT NULL COMMENT '稼动率' ;

-- 20250226 yp
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `tool_life_expored_count` INT(11) NULL DEFAULT NULL COMMENT '刀具寿命报警次数' AFTER `duty_rate`;

-- 20250227 yp
ALTER TABLE `vg_autodrill_db`.`t_rack` 
ADD COLUMN `position_code` VARCHAR(200) NULL DEFAULT NULL COMMENT '位置码' AFTER `relate_device_code`;

update vg_autodrill_db.t_rack set position_code = code where id >0

-- 20250228 yp
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
CHANGE COLUMN `buffer_raw_exist_to_run_time` `drill_raw_exist_to_run_time` INT(11) NULL DEFAULT NULL COMMENT '钻机有板到开始打板时长' ;



-- 20250304 zyy
INSERT INTO `sys_menu` VALUES (1682,'DRILLAMIN','稼动率因素',1673,'#','factor','utilizationRate/factor',2,'utilizationRate:factor:list',NULL,1,0,0,1,'2025-02-07 15:40:41','2025-02-13 13:50:43',1);
INSERT INTO `sys_menu` VALUES (1683,'DRILLAMIN','稼动率大屏',1673,'#','dashboard','utilizationRate/dashboard',2,'utilizationRate:dashboard:list',NULL,1,0,1,1,'2025-02-28 17:23:42','2025-02-28 17:23:54',1);
INSERT INTO `sys_menu` VALUES (1684,'DRILLAMIN','临时保养',1673,'#','maintenance','utilizationRate/maintenance',2,'utilizationRate:maintenance:list',NULL,5,0,0,1,'2025-03-04 10:47:37','2025-03-04 15:45:44',1);
INSERT INTO `sys_menu` VALUES (1685,'DRILLAMIN','新增',1684,'#','','#',3,'utilizationRate:maintenance:add',NULL,1,0,1,1,'2025-03-04 15:34:30',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1686,'DRILLAMIN','修改',1684,'#','','#',3,'utilizationRate:maintenance:edit',NULL,2,0,1,1,'2025-03-04 15:34:48',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1687,'DRILLAMIN','查看保养记录',1684,'#','','#',3,'utilizationRate:maintenance:view',NULL,3,0,1,1,'2025-03-04 15:35:09',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1688,'DRILLAMIN','开始',1684,'#','','#',3,'utilizationRate:maintenance:start',NULL,4,0,1,1,'2025-03-04 15:35:29',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1689,'DRILLAMIN','结束',1684,'#','','#',3,'utilizationRate:maintenance:end',NULL,5,0,1,1,'2025-03-04 15:35:44',NULL,NULL);



-- 20250307 zyy

INSERT INTO `sys_menu` VALUES (1690,'DRILLAMIN','设定时间',1673,'#','utilizationRate_config','utilizationRate/config',2,'utilizationRate:config:list',NULL,6,0,0,1,'2025-03-04 15:45:06','2025-03-07 09:03:46',1);
INSERT INTO `sys_menu` VALUES (1691,'DRILLAMIN','理论达成率',1673,'#','achievementRate','utilizationRate/achievementRate',2,'utilizationRate:achievementRate:list',NULL,4,0,0,1,'2025-03-04 16:22:30','2025-03-07 09:03:55',1);



-- 202503011 zyy
INSERT INTO `sys_menu` VALUES (1692,'DRILLAMIN','只读',1690,'#','','#',3,'utilizationRate:config:list',NULL,1,0,1,1,'2025-03-11 11:32:41','2025-03-11 14:21:14',1);
INSERT INTO `sys_menu` VALUES (1693,'DRILLAMIN','修改',1690,'#','','#',3,'utilizationRate:config:edit',NULL,2,0,1,1,'2025-03-11 11:32:56',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1694,'DRILLAMIN','保存',1690,'#','','#',3,'utilizationRate:config:save',NULL,3,0,1,1,'2025-03-11 11:33:22',NULL,NULL);

---20250311 dpf
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `drill_novacuum_count` int NULL COMMENT '吸尘报警次数' AFTER `drill_tool_evaluation_time`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `drill_novacuum_config_time` int NULL COMMENT '吸尘报警配置时长' AFTER `drill_tool_evaluation_time`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `drill_novacuum_real_time` int NULL COMMENT '吸尘报警实际时长' AFTER `drill_tool_evaluation_time`;  





-- 20250312 dpf

CREATE TABLE `t_device_records_summary_extend` (
  `id` int NOT NULL AUTO_INCREMENT,
  `is_deleted` tinyint NOT NULL DEFAULT '0',
  `status` int NOT NULL DEFAULT '1',
  `creator_id` int DEFAULT NULL,
  `create_time` datetime NOT NULL,
  `modify_time` datetime DEFAULT NULL,
  `modifier_id` int DEFAULT NULL,
  `device_code` varchar(100) DEFAULT NULL COMMENT '设备编号',
  `tool_life_expored_change_standard_time` int DEFAULT NULL COMMENT '刀具寿命报警更换标准时长 --单次耗时（秒）',
  `tool_life_expored_change_real_time` int DEFAULT NULL COMMENT '刀具寿命报警更换实际时长 --单次耗时（秒）',
  `tool_life_expored_change_count` int DEFAULT NULL COMMENT '刀具寿命报警更换次数',
  `no_switch_material_tool_change_standard_time` int DEFAULT NULL COMMENT '不切换料号时的换刀标准时长 --单次耗时（秒）',
  `no_switch_material_tool_change_real_time` int DEFAULT NULL COMMENT '不切换料号时的换刀实际时长 --单次耗时（秒）',
  `no_switch_material_tool_change_count` int DEFAULT NULL COMMENT '不切换料号时的换刀次数',
  `switch_material_tool_change_standard_time` int DEFAULT NULL COMMENT '切换料号时的换刀标准时长 --单次耗时（秒）',
  `switch_material_tool_change_real_time` int DEFAULT NULL COMMENT '不切换料号时的换刀实际时长 --单次耗时（秒）',
  `switch_material_tool_change_count` int DEFAULT NULL COMMENT '切换料号时的换刀次数',
  `pin_revise_standard_time` int DEFAULT NULL COMMENT 'PIN校正标准时长 --按班次平摊（秒）',
  `pin_revise_real_time` int DEFAULT NULL COMMENT 'PIN校正实际时长 --按班次平摊（秒）',
  `pin_revise_count` int DEFAULT NULL COMMENT 'PIN校正次数',
  `defect_swing_torque_standard_time` int DEFAULT NULL COMMENT '检测摆幅扭力标准时长 --按班次平摊（秒）',
  `defect_swing_torque_real_time` int DEFAULT NULL COMMENT '检测摆幅扭力实际时长 --按班次平摊（秒）',
  `defect_swing_torque_count` int DEFAULT NULL COMMENT '检测摆幅扭力次数',
  `pressure_foot_change_standard_time` int DEFAULT NULL COMMENT '压力脚更换标准时长 --单次耗时（秒）',
  `pressure_foot_change_real_time` int DEFAULT NULL COMMENT '压力脚更换实际时长 --单次耗时（秒）',
  `pressure_foot_change_count` int DEFAULT NULL COMMENT '压力脚更换次数',
  `min_multilayer_boards_standard_value` int DEFAULT '3' COMMENT '多层板，最小层数设定标准层数，默认3',
  `min_multilayer_boards_real_value` int DEFAULT NULL COMMENT '多层板，最小实设定实际层数',
  `two_boards_wait_first_result_standard_time` int DEFAULT NULL COMMENT '两层板等待首件标准耗时 --单次耗时（秒）',
  `two_boards_wait_first_result_real_time` int DEFAULT NULL COMMENT '两层板等待首件实际耗时 --单次耗时（秒）',
  `two_boards_wait_first_result_count` int DEFAULT NULL COMMENT '两层板等待首件次数',
  `multilayer_boards_wait_first_result_standard_time` int DEFAULT NULL COMMENT '多层板等待首件标准耗时 --单次耗时（秒）',
  `multilayer_boards_wait_first_result_real_time` int DEFAULT NULL COMMENT '多层板等待首件实际耗时 --单次耗时（秒）',
  `multilayer_boards_wait_first_result_count` int DEFAULT NULL COMMENT '多层板等待首件次数',
  UNIQUE KEY `t_device_records_summary_extend_unique` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4  COMMENT='设备维护记录扩展表';


--- 20250313 dpf
INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('DrillNoVacuum', '10', 'int', '吸尘报警--单次耗时（秒）', '', '吸尘报警--单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('ToolLifeExporedChange', '60', 'int', '刀具寿命报警更换 --单次耗时（秒）', '', '刀具寿命报警更换 --单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('NoSwitchMaterialToolChange', '110', 'int', '不切换料号时的换刀时长 --单次耗时（秒）', '', '不切换料号时的换刀时长 --单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('SwitchMaterialToolChange', '210', 'int', '切换料号时的换刀时长 --单次耗时（秒）', '', '切换料号时的换刀时长 --单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('PINRevise', '43', 'int', 'PIN校正 --按班次平摊（秒）', '', 'PIN校正 --按班次平摊（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('DetectSwingTorque', '43', 'int', '检测摆幅扭力时长 --按班次平摊（秒）', '', '检测摆幅扭力时长 --按班次平摊（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('PressureFootChange', '43', 'int', '压力脚更换时长 --单次耗时（秒）', '', '压力脚更换时长 --单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('MinMultilayerBoardsValue', '3', 'int', '多层板，最小层设定', '', '多层板，最小层设定', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('TwoBoardsWaitFirstResult', '100', 'int', '两层板等待首件耗时 --单次耗时（秒）', '', '两层板等待首件耗时 --单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('MultilayerBoardsWaitFirstResult', '520', 'int', '多层板等待首件结果耗时 --单次耗时（秒）', '', '多层板等待首件结果耗时 --单次耗时（秒）', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '100');



-- vg_autodrill_db.t_device_temporary_maintenance_records definition

CREATE TABLE `t_device_temporary_maintenance_records` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主键',
  `device_code` varchar(200) NOT NULL COMMENT '设备code',
  `maintenance_type` int DEFAULT NULL COMMENT '临时保养类型',
  `maintenance_type_name` varchar(200) CHARACTER SET utf8mb4  DEFAULT NULL COMMENT '临时保养类型名称',
  `count_type` int DEFAULT '0' COMMENT '计算类型   0 按频次 ，1 按耗时',
  `start_time` datetime DEFAULT NULL COMMENT '临时保养开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '临时保养结束时间',
  `maintenance_person_id` int DEFAULT NULL COMMENT '保养人员id',
  `maintenance_person_name` varchar(200) DEFAULT NULL COMMENT '保养人名称',
  `is_deleted` tinyint DEFAULT '0' COMMENT '否已删除    默认0',
  `status` int DEFAULT '1' COMMENT '状态(1:启用;0禁用)    默认值: 1',
  `creator_id` int DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modifier_id` int DEFAULT NULL COMMENT '修改人Id',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=MEMORY AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4  COMMENT='设备临时保养';

--- 20250313 zyy
INSERT INTO `sys_menu` VALUES (1695,'DRILLAMIN','删除',1684,'#','','#',3,'utilizationRate:maintenance:delete',NULL,2,0,1,1,'2025-03-14 15:04:46',NULL,NULL);

---20250314   dpf

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `tool_life_expored_change_standard_time` int NULL COMMENT '刀具寿命报警更换标准时长 --单次耗时（秒）' AFTER `drill_novacuum_count`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `tool_life_expored_change_count` int NULL COMMENT '刀具寿命报警更换次数' AFTER `drill_novacuum_count`; 


ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `no_switch_material_tool_change_standard_time` int NULL COMMENT '不切换料号时的换刀标准时长 --单次耗时（秒）' AFTER `drill_novacuum_count`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `no_switch_material_tool_change_count` int NULL COMMENT '不切换料号时的换刀次数' AFTER `drill_novacuum_count`; 


ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `switch_material_tool_change_standard_time` int NULL COMMENT '切换料号时的换刀标准时长 --单次耗时（秒）' AFTER `drill_novacuum_count`;

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `switch_material_tool_change_count` int NULL COMMENT '切换料号时的换刀次数' AFTER `drill_novacuum_count`; 

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `pin_revise_standard_time` int NULL COMMENT 'PIN校正标准时长 --按班次平摊（秒）' AFTER `drill_novacuum_count`;


ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `pin_revise_count` int NULL COMMENT 'PIN校正次数' AFTER `drill_novacuum_count`; 

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `defect_swing_torque_standard_time` int NULL COMMENT 'PIN校正标准时长 --按班次平摊（秒）' AFTER `drill_novacuum_count`;


ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `defect_swing_torque_count` int NULL COMMENT 'PIN校正次数' AFTER `drill_novacuum_count`; 

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `pressure_foot_change_standard_time` int NULL COMMENT '压力脚更换标准时长 --单次耗时（秒）' AFTER `drill_novacuum_count`;


ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `pressure_foot_change_count` int NULL COMMENT '压力脚更换次数' AFTER `drill_novacuum_count`; 

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `min_multilayer_boards_standard_value` int NULL COMMENT '多层板，最小层数设定标准层数   3' AFTER `drill_novacuum_count`;



ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `two_boards_wait_first_result_standard_time` int NULL COMMENT '两层板等待首件标准耗时 --单次耗时（秒）' AFTER `drill_novacuum_count`;



ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `two_boards_wait_first_result_count` int NULL COMMENT '两层板等待首件次数' AFTER `drill_novacuum_count`; 

ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `multilayer_boards_wait_first_result_standard_time` int NULL COMMENT '多层板等待首件标准耗时 --单次耗时（秒）' AFTER `drill_novacuum_count`;



ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `multilayer_boards_wait_first_result_count` int NULL COMMENT '多层板等待首件次数' AFTER `drill_novacuum_count`;



---20250317 zyy
INSERT INTO `sys_menu` VALUES (1696,'DRILLAMIN','断刀信息',1027,'#','brokenKnives','alarm/brokenKnives',2,'alarm:brokenKnives:list',NULL,1,0,0,1,'2025-03-17 13:42:49','2025-03-17 15:57:45',1);
INSERT INTO `sys_menu` VALUES (1697,'DRILLAMIN','导出',1696,'#','','#',3,'alarm:brokenKnives:export',NULL,2,0,1,1,'2025-03-17 15:35:33','2025-03-17 15:35:41',1);
INSERT INTO `sys_menu` VALUES (1698,'DRILLAMIN','查询',1696,'#','','#',3,'alarm:brokenKnives:list',NULL,1,0,1,1,'2025-03-17 15:35:53',NULL,NULL);


------20250319  dpf
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
ADD COLUMN `necessary_time` int NULL COMMENT '必要时间' AFTER `duty_rate`;



-----20250324 dpf
ALTER TABLE vg_autodrill_db.t_warehouse ADD transportation_kind TINYINT NULL COMMENT '料仓类型';
-- 20250328 xfj
alter table t_transportation_task add index (create_time);
-- 20250329 xifj
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('ShowLatestXMinutesWarnings', '5', 'int', '设置显示几分钟前的未处理告警', '设置显示几分钟前的未处理告警', '0', '1', '1', '2025-3-29 17:02:10', '1');

-----20250330 dpf
ALTER TABLE vg_autodrill_db.t_rack ADD feed_agv_inner_point varchar(50) NULL COMMENT '大车内点';
ALTER TABLE vg_autodrill_db.t_rack ADD feed_agv_out_point varchar(50) NULL COMMENT '大车外点';
ALTER TABLE vg_autodrill_db.t_rack ADD feed_agv_rest_point varchar(50) NULL COMMENT '大车休息点';
ALTER TABLE vg_autodrill_db.t_rack ADD trans_agv_inner_point varchar(50) NULL COMMENT '小车内点';
ALTER TABLE vg_autodrill_db.t_rack ADD trans_agv_out_point varchar(50) NULL COMMENT '小车外点';
ALTER TABLE vg_autodrill_db.t_rack ADD trans_agv_rest_point varchar(50) NULL COMMENT '小车休息点';

-----20250330 zyy
INSERT INTO `sys_menu` VALUES (1699,'DRILLAMIN','待排产',1024,'#','toBeScheduled','produce/toBeScheduled',2,'produce:toBeScheduled:list',NULL,17,0,1,1,'2025-03-28 09:55:34','2025-03-30 10:25:56',1);




-----20250331 zyy
INSERT INTO `sys_menu` VALUES (1701,'DRILLAMIN','批量提交',1419,'#','','#',3,'produce:taskCharts:commit',NULL,2,0,1,1,'2025-03-31 11:06:02',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1702,'DRILLAMIN','批量撤销',1419,'#','','#',3,'produce:taskCharts:revoke',NULL,3,0,1,1,'2025-03-31 11:06:37',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1704,'DRILLAMIN','仅查看（只有查询功能）',1419,'#','','#',3,'produce:taskCharts:view',NULL,1,0,1,1,'2025-03-31 11:19:15','2025-03-31 15:32:08',1);
INSERT INTO `sys_menu` VALUES (1705,'DRILLAMIN','仅查看（只有查询功能）',1472,'#','','#',3,'produce:ticketCharts:view',NULL,1,0,1,1,'2025-03-31 15:17:25','2025-03-31 15:32:15',1);
INSERT INTO `sys_menu` VALUES (1706,'DRILLAMIN','标记颜色',1472,'#','','#',3,'produce:ticketCharts:remarkcolor',NULL,3,0,1,1,'2025-03-31 15:22:46',NULL,NULL);

-----20250401 dpf
ALTER TABLE vg_autodrill_db.t_external_work_order ADD estimated_time INT NULL COMMENT '预计运行时间';
ALTER TABLE vg_autodrill_db.t_work_order ADD estimated_time INT NULL COMMENT '预测运行时间';

-----20250411
ALTER TABLE vg_autodrill_db.t_work_order ADD process_code varchar(100) NULL COMMENT '工序编码';
ALTER TABLE vg_autodrill_db.t_external_work_order ADD process_code varchar(100) NULL COMMENT '工序编码';

-- 20250412 xifj
ALTER TABLE `vg_autodrill_db`.`t_device_records_summary` 
CHANGE COLUMN `id` `id` BIGINT NOT NULL AUTO_INCREMENT ,
ADD PRIMARY KEY (`id`);
-----20250413 dpf
ALTER TABLE vg_autodrill_db.t_device_records_summary DROP COLUMN tool_life_expored_change_count;

-- 20250413 xifj
UPDATE `vg_autodrill_db`.`sys_config` SET `config_value` = 'http://10.50.222.71:8001/central/stat/location/simple?deviceKind' WHERE (`id` = '89');

----20250420
ALTER TABLE vg_autodrill_db.t_task ADD dia_file_path varchar(200) NULL COMMENT '钻孔参数文件路径';
ALTER TABLE vg_autodrill_db.t_task ADD program_file_path varchar(200) NULL COMMENT '钻孔程序路径';


-- 20250424 xifj
ALTER TABLE `vg_autodrill_db`.`t_schedule` 
ADD INDEX `idx_agv_and_allocate_time_of_t_schedule` (`require_device_id` ASC, `allocate_time` DESC);


-- 20250428 zyy
INSERT INTO `sys_menu` VALUES (1708,'DRILLAMIN','调度历史',1026,'#','scheduleHistory','device/scheduleHistory/index',2,'device:scheduleHistory:list',NULL,8,0,1,1,'2025-04-28 10:43:00','2025-04-28 10:43:34',1);
INSERT INTO `sys_menu` VALUES (1709,'DRILLAMIN','详情',1708,'#','','#',3,'device:scheduleHistory:detail',NULL,1,0,1,1,'2025-04-28 11:46:22',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1710,'DRILLAMIN','导出',1708,'#','','#',3,'device:scheduleHistory:export',NULL,2,0,1,1,'2025-04-28 13:18:42',NULL,NULL);



----20252504 zyy
INSERT INTO `sys_menu` VALUES (1711,'DRILLAMIN','导出',1691,'#','','#',3,'utilizationRate:achievementRate:export',NULL,1,0,1,1,'2025-05-04 13:39:04',NULL,NULL);


-- 20250519 dpf, 调度记录历史表
CREATE TABLE `t_schedule_his` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `code` varchar(500) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL COMMENT '任务编号',
  `source_device_id` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `require_device_id` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `task_id` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `start_location` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT '起点',
  `end_location` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT '终点',
  `priority` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '优先级',
  `request_json` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT '请求详情',
  `need_republish` tinyint DEFAULT '0' COMMENT '是否需要再次发布任务',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `status` int NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int DEFAULT NULL COMMENT '修改人Id',
  `scheduled_task_status` int DEFAULT '0' COMMENT '调度任务状态',
  `allocate_time` datetime DEFAULT NULL COMMENT '分配时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `routing_key` varchar(100) DEFAULT NULL COMMENT '主叫设备的route key',
  `remark` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT '备注',
  `changed_spindles` varchar(200) DEFAULT NULL COMMENT '变化后的Spindles，如null,null,00003,null,null',
  `changed_behavior` varchar(200) DEFAULT NULL COMMENT '调整后的上下料行为，如-1,0,0,-1,0',
  `item_id` int DEFAULT NULL COMMENT '产品Id',
  `item_name` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '产品名称',
  `item_code` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '产品编号',
  `warnning_code` varchar(200) DEFAULT NULL COMMENT '告警编码',
  `warnning_message` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT '消息',
  `request_interaction_behavior` varchar(200) DEFAULT NULL COMMENT '交互方式',
  `request_interaction_behavior_name` varchar(500) DEFAULT NULL COMMENT '交互方式描述',
  `total_raw_count` int DEFAULT NULL COMMENT '累计已上生料',
  `is_urgent` tinyint(1) DEFAULT '0' COMMENT '是否紧急',
  `is_all_panel_sent` tinyint(1) DEFAULT '1' COMMENT '是否已全部送料',
  `route_id` bigint DEFAULT NULL COMMENT '工艺路线ID',
  `route_code` varchar(64) DEFAULT NULL COMMENT '工艺路线编码',
  `route_name` varchar(255) DEFAULT NULL COMMENT '工艺路线名称',
  `master_schedule_id` bigint DEFAULT NULL COMMENT '关联的主叫调度记录ID',
  `is_auxiliary` tinyint(1) DEFAULT '0' COMMENT '是否是辅助设备请求',
  `interaction_sequence` int DEFAULT NULL COMMENT '呼叫序列',
  `request_device_kind` int DEFAULT NULL COMMENT '呼叫设备kind',
  `is_master` tinyint(1) DEFAULT NULL COMMENT '是否为先执行',
  `sub_device_code` varchar(50) DEFAULT NULL COMMENT '库位编号',
  `agv_payload_panels` mediumtext COMMENT 'AGV板料信息',
  `request_summary_info` mediumtext COMMENT '请求的汇总简略信息',
  `cancel_reason` mediumtext COMMENT '取消原因',
  `is_barcode_ok` tinyint(1) DEFAULT NULL COMMENT '板料检验是否OK',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `code_UNIQUE` (`code`),
  KEY `idx_scheduled_task_status_of_t_schedule` (`scheduled_task_status`),
  KEY `idx_source_device_id_of_t_schedule` (`source_device_id`),
  KEY `idx_sub_device_code_of_t_schedule` (`sub_device_code`),
  KEY `idx_request_device_kind_of_t_schedule` (`request_device_kind`)
) ENGINE=InnoDB AUTO_INCREMENT=261841 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci ROW_FORMAT=DYNAMIC COMMENT='调度记录历史表';


-----20250520 dpf
ALTER TABLE vg_autodrill_db.t_device ADD machine_size varchar(200) NULL COMMENT '机器尺寸';
ALTER TABLE vg_autodrill_db.t_device ADD is_auto tinyint NULL COMMENT '自动机器还是手动机器';
ALTER TABLE vg_autodrill_db.t_device ADD is_duo tinyint NULL COMMENT '是否是DUO机器';

ALTER TABLE vg_autodrill_db.t_task ADD ftp_host varchar(100) NULL COMMENT '文件ftp的host地址';
ALTER TABLE vg_autodrill_db.t_task ADD ftp_port varchar(100) NULL COMMENT '文件ftp的host地址端口';
ALTER TABLE vg_autodrill_db.t_task ADD ftp_username varchar(100) NULL COMMENT '文件ftp地址的用户名';
ALTER TABLE vg_autodrill_db.t_task ADD ftp_password varchar(100) NULL COMMENT '文件ftp地址的密码';


---20250521  dpf 
CREATE TABLE `t_cutter_group` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键id',
  `group_no` varchar(200) DEFAULT NULL COMMENT '配刀组编号',
  `drill_no` varchar(200) DEFAULT NULL COMMENT '钻机编号',
  `drill_name` varchar(200) DEFAULT NULL COMMENT '钻机名称',
  `item_name` varchar(200) DEFAULT NULL COMMENT '料号',
  `planned_time` datetime DEFAULT NULL COMMENT '计划要板时间',
  `round_num` int(11) DEFAULT NULL COMMENT '配刀组计划对应的生产趟数',
  `axis_count` int(11) DEFAULT NULL COMMENT '轴数',
  `end_axis_count` int(11) DEFAULT '0' COMMENT '尾轮轴数，默认:0',
  `group_date` datetime DEFAULT NULL COMMENT '组计划生成时间',
  `cutter_group_status` int(11) DEFAULT NULL COMMENT '配刀状态 0-待配刀 1-配刀锁定',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '否已删除 默认值: 0',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)  默认值: 1',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `round_num_standard` int(11) DEFAULT NULL COMMENT '配刀组计划对应的生产标准趟数',
  `machine_size` varchar(50) DEFAULT NULL COMMENT '机型',
  `cutter_box_num` int(11) DEFAULT NULL COMMENT '单轴的刀盒数量',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COMMENT='配刀组计划表';

-- vg_autodrill_db.t_cutter_group_detail definition


CREATE TABLE `t_cutter_group_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键id',
  `group_no` varchar(200) NOT NULL COMMENT '配刀组计划No',
  `item_code` varchar(200) DEFAULT NULL COMMENT 'lot编号',
  `panel_num` int(11) DEFAULT NULL COMMENT '每趟的实际钻板数',
  `task_code` varchar(200) DEFAULT NULL COMMENT '任务编码',
  `is_first_cutter` tinyint(4) DEFAULT NULL COMMENT '是否首次',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '否已删除 默认值: 0',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)    默认值: 1',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COMMENT='配刀组计划明细表';


-- 20250522 yucl
ALTER TABLE vg_autodrill_db.t_external_work_order ADD cutter_info varchar(255) NULL COMMENT '自动配刀相关内容';
ALTER TABLE vg_autodrill_db.t_work_order ADD cutter_info varchar(255) NULL COMMENT '自动配刀相关内容';




--20250529  dpf
ALTER TABLE vg_autodrill_db.t_task ADD is_cutter TINYINT NULL COMMENT '是否已配刀';

--20250604 dpf
ALTER TABLE vg_autodrill_db.t_device ADD raw_location_code varchar(200) NULL COMMENT '生料库位';
ALTER TABLE vg_autodrill_db.t_device ADD clinker_location_code varchar(200) NULL COMMENT '熟料库位';

--20250605 dpf
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'HaveMultiMachines', 'true', 'bool', '是否启用多机型钻带钻换', NULL, '是否启用多机型钻带钻换', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '0');
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'EnableAutoCutter', 'true', 'bool', '是否启用自动配刀计划', NULL, '是否启用自动配刀计划', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '0');
INSERT INTO vg_autodrill_db.sys_config
(id, config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'MachineTypeConfig', '{"2530":{"DrillMachine":"2530","CutterBoxNum":5},"2537":{"DrillMachine":"2530","CutterBoxNum":6},"2849":{"DrillMachine":"2849","CutterBoxNum":6}}', 'string', '机型对应的单轴的刀盒数', NULL, '机型对应的单轴的刀盒数', 0, 1, 1, '2025-03-13 09:32:41', '2025-03-13 09:32:41', 1, 0, 0, 0, '0');

INSERT INTO vg_autodrill_db.t_encode_build_rules
( rules_code, rules_name, prefix, number_length, is_padded, suffix, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, has_year, has_month, has_day)
VALUES( 'CUTTER_GROUP_CODE', '配刀计划编码', 'CG', 6, 1, NULL, '', 0, 1, 1, '2025-05-24 10:14:18', '2025-05-24 10:14:18', 1, 1, 1, 1);

--20250609 dpf
-- vg_autodrill_db.t_manual_call_agv_log definition

CREATE TABLE `t_manual_call_agv_log` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `item_code` varchar(200) DEFAULT NULL COMMENT '物料号',
  `location_code` varchar(200) DEFAULT NULL COMMENT '库位号',
  `pod_code` varchar(200) DEFAULT NULL COMMENT '托盘号',
  `agv_operate_name` varchar(200) DEFAULT NULL COMMENT 'agv操作类型名字',
  `agv_operate_type` varchar(200) DEFAULT NULL COMMENT 'agv操作类型',
  `clinker_material_num` int(11) DEFAULT NULL COMMENT '熟料数量',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '否已删除  默认值: 0',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)  默认值: 1',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `request_url` varchar(200) DEFAULT NULL COMMENT '请求url',
  `call_back_message` varchar(500) DEFAULT NULL COMMENT '呼叫返回信息',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COMMENT='手动呼叫agv上下料记录表';

--  20250610 quyang
ALTER TABLE vg_autodrill_db.t_warehouse ADD `min_empty_location_num` INT(11) NULL COMMENT '最少空位数';
ALTER TABLE vg_autodrill_db.t_warehouse ADD `min_empty_box_num` INT(11) NULL COMMENT '最少空仓数';
ALTER TABLE vg_autodrill_db.t_warehouse ADD `max_empty_box_num` INT(11) NULL COMMENT '最多空仓数';
ALTER TABLE vg_autodrill_db.t_warehouse ADD `min_drilled_track_out_num` INT(11) NULL COMMENT '熟料最少转出数量';
ALTER TABLE vg_autodrill_db.t_warehouse ADD `is_auto_track_out_drilled_silo` TINYINT(4) NULL DEFAULT '0' COMMENT '料仓中没有钻机中相同的料号时是否立即转出';
ALTER TABLE vg_autodrill_db.t_warehouse ADD `raw_track_out_timeout_time` INT(11) NULL DEFAULT '1800' COMMENT '生料转出超时时间(秒)';
ALTER TABLE vg_autodrill_db.t_warehouse ADD `min_first_track_out_num` INT(11) NULL COMMENT '首件最少转出数量';

INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('EnableStandAlonePartitionSettings', 'false', 'bool', '是否启用独立分区设置', '是否启用独立分区设置', '0', '1', '1', '2023-6-10 17:19:00', '1');



---20250611 dpf

INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'UploadRawMaterial', 'http://192.168.102.178:8001/v1/std/UploadRawMaterial?materialCode={0}&destinationPoint={1}', 'string', '手动呼叫agv上料', NULL, '手动呼叫agv上料', 0, 1, 1, '2024-11-20 10:22:56', '2024-11-20 10:22:56', 1, 0, NULL, 0, '0');
INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'UnloadEmptyFork', 'http://192.168.102.178:8001/v1/std/UnloadEmptyFork?siloCode={0}&destinationPoint={1}', 'string', '手动呼叫agv退空盘', NULL, '手动呼叫agv退空盘', 0, 1, 1, '2024-11-20 10:22:56', '2024-11-20 10:22:56', 1, 0, NULL, 0, '0');
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'UploadEmptyFork', 'http://192.168.102.178:8001/v1/std/UploadEmptyFork?destinationPoint={0}', 'string', '手动呼叫agv上空盘', NULL, '手动呼叫agv上空盘', 0, 1, 1, '2024-11-20 10:22:56', '2024-11-20 10:22:56', 1, 0, NULL, 0, '0');
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'UnloadClinker', 'http://192.168.102.178:8001/v1/std/UnloadClinker?siloCode={0}&materialCode={1}&podLotNum={2}&destinationPoint={3}', 'string', '手动呼叫agv下料', NULL, '手动呼叫agv下料', 0, 1, 1, '2024-11-20 10:22:56', '2024-11-20 10:22:56', 1, 0, NULL, 0, '0');


-- quyang 库区设置初始化配置，从sys_config获取数据
UPDATE t_warehouse SET min_empty_location_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='WipCountOfEmptyForkAtLeast'),0) WHERE partition_kind='2';
UPDATE t_warehouse SET min_empty_box_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='WipMinPartitionEmptySiloNum'),0) WHERE partition_kind='2';
UPDATE t_warehouse SET max_empty_box_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='WipMaxPartitionEmptySiloNum'),0) WHERE partition_kind='2';
UPDATE t_warehouse SET min_drilled_track_out_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='WipMinDrilledTrackOutNum'),0) WHERE partition_kind='2';
UPDATE t_warehouse SET is_auto_track_out_drilled_silo=(SELECT CASE config_value WHEN 'false' THEN 0 ELSE 1 END from sys_config  WHERE config_code='WipAutoTrackOutSilo') WHERE partition_kind='2';
UPDATE t_warehouse SET min_first_track_out_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='WipMinFirstDrilledTrackOutNum'),0) WHERE partition_kind='2';

UPDATE t_warehouse SET min_empty_location_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='CountOfEmptyForkAtLeast'),0) WHERE partition_kind='1';
UPDATE t_warehouse SET min_empty_box_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='MinPartitionEmptySiloNum'),0) WHERE partition_kind='1';
UPDATE t_warehouse SET max_empty_box_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='MaxPartitionEmptySiloNum'),0) WHERE partition_kind='1';
UPDATE t_warehouse SET min_drilled_track_out_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='MinDrilledTrackOutNum'),0) WHERE partition_kind='1';
UPDATE t_warehouse SET is_auto_track_out_drilled_silo=(SELECT CASE config_value WHEN 'false' THEN 0 ELSE 1 END from sys_config  WHERE config_code='AutoTrackOutSilo') WHERE partition_kind='1';
UPDATE t_warehouse SET raw_track_out_timeout_time=IFNULL((SELECT config_value from sys_config  WHERE config_code='UndrilledItemOccupyLocationIdleTimeout'),0) WHERE partition_kind='1';
UPDATE t_warehouse SET min_first_track_out_num=IFNULL((SELECT config_value from sys_config  WHERE config_code='MinFirstDrilledTrackOutNum'),0) WHERE partition_kind='1';


--20250611 xifj
alter table t_schedule_his add index (create_time);

--20250613 quyang
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`) 
VALUES ('EnableNoticeExternalSystemPanelsData', 'false', 'bool', '是否启用通知第三方系统板料与钻机的加工关系数据(博敏)', '是否启用通知第三方系统板料与钻机的加工关系数据(博敏)', '0', '1', '1', '2023-6-10 17:19:00', '1');
INSERT INTO `vg_autodrill_db`.`sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`,`creator_id`, `create_time`,  `is_system`,`category`) 
VALUES ('URLNoticeExternalSystemPanelsData', '', 'string', '通知第三方系统板料与钻机的加工关系数据接口地址(博敏)', '通知第三方系统板料与钻机的加工关系数据接口地址(博敏)', 0, 1, 1, '2025-6-13 14:50:00',  0,'1');

--20250613 dpf
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'BominDrillRecipes', 'http://192.168.42.11:6682/api/thirdparty/Vega/GetDrillRecipes  ', 'string', '博敏获取钻带文件接口', NULL, '', 0, 1, NULL, '2025-04-16 09:32:41.000', '2025-04-16 09:32:41.000', NULL, 0, NULL, 0, '0');


--20250623 dpf
-- vg_autodrill_db.t_manual_call_agv_task definition

CREATE TABLE `t_manual_call_agv_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键id',
  `device_code` varchar(100) DEFAULT NULL COMMENT '钻机编码',
  `location_code` varchar(100) NOT NULL COMMENT '库位号',
  `pod_code` varchar(100) DEFAULT NULL COMMENT '托盘号',
  `clinker_material_num` int(11) DEFAULT NULL COMMENT '熟料数量',
  `item_code` varchar(100) DEFAULT NULL COMMENT '物料lot号',
  `location_type` int(11) DEFAULT NULL COMMENT '库位区域类型',
  `location_type_name` varchar(100) DEFAULT NULL COMMENT '库位区域类型名字',
  `agv_operate_type` int(11) DEFAULT NULL COMMENT 'agv操作类型',
  `agv_operate_name` varchar(100) DEFAULT NULL COMMENT 'agv操作类型名字',
  `is_bind` tinyint(4) DEFAULT NULL COMMENT '是否已绑定',
  `task_status` int(11) DEFAULT NULL COMMENT '任务状态',
  `task_status_description` varchar(100) DEFAULT NULL COMMENT '任务状态描述',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `is_deleted` tinyint(4) DEFAULT '0' COMMENT '否已删除 默认值: 0',
  `status` int(11) DEFAULT '1' COMMENT '状态(1:启用;0禁用)   默认值: 1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COMMENT='手动呼叫agv任务表';

--20250627 dpf
ALTER TABLE vg_autodrill_db.t_manual_call_agv_log ADD device_code varchar(100)  NOT NULL COMMENT '钻机编码';

INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'BindAndUnbindAnd', 'http://192.168.102.178:8001/v1/std/BoxBindFold', 'string', '上机解绑/下机绑定', NULL, '上机解绑/下机绑定', 0, 1, 1, '2023-06-10 17:19:00', '2023-06-10 17:19:00', 1, 0, NULL, 0, '0');


--20250630
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'Agv_Run_Status', 'http://192.168.102.76:8001/v1/std/GetTaskState?locationCode={0}', 'string', '手动呼叫agv模块获取agv运行状态url配置', NULL, '手动呼叫agv模块获取agv运行状态url配置', 0, 1, 1, '2023-10-18 15:35:48', '2023-10-18 15:35:48', 1, 0, NULL, 0, '0');

--20250701
ALTER TABLE vg_autodrill_db.t_schedule MODIFY COLUMN request_interaction_behavior SMALLINT UNSIGNED NULL COMMENT '交互方式';
ALTER TABLE vg_autodrill_db.t_schedule_his MODIFY COLUMN request_interaction_behavior SMALLINT UNSIGNED NULL COMMENT '交互方式';
-- 20250701 xifj
ALTER TABLE vg_autodrill_db.t_transportation_task ADD start_schedule varchar(50) NULL COMMENT '开始库位的调度';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD end_schedule varchar(50) NULL COMMENT '结束库位的调度';

--20250704 dpf 
INSERT INTO vg_autodrill_db.sys_config
( config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('TransferScheduleHistoryDataSwitch', 'false', 'bool', '迁移历史调度数据开关', NULL, '迁移历史调度数据开关', 0, 1, 1, '2023-10-18 15:37:29', '2023-10-18 15:37:29', 1, 0, NULL, 0, '0');

--20250707  dpf

ALTER TABLE vg_autodrill_db.t_warehouse ADD drilled__track_out_time_minutes INT DEFAULT 10 NULL COMMENT '熟料转出超时(分钟，默认10分钟)';
ALTER TABLE vg_autodrill_db.t_warehouse ADD is_controlled_by_server TINYINT DEFAULT false NULL COMMENT '转运任务是否由服务端控制(默认：否)';
ALTER TABLE vg_autodrill_db.t_warehouse ADD related_buffer_code varchar(200) NULL COMMENT '关联的缓冲区代号';

--20250707 xifj
alter table t_work_order add index (item_code);

--20250709  dpf
ALTER TABLE vg_autodrill_db.t_warehouse CHANGE drilled__track_out_time_minutes drilled_track_out_time_minutes int(11) DEFAULT 10 NULL COMMENT '熟料转出超时(分钟，默认10分钟)';

--20250710  zyy 
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1712, 'DRILLAMIN', '配刀计划', 1024, '#', 'cutterGroup', 'produce/cutterGroup', 2, 'produce:cutterGroup:list', NULL, 90, 0, 1, 1, '2025-07-09 16:15:27', NULL, NULL);
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1713, 'DRILLAMIN', '查看', 1712, '#', '', '#', 3, 'produce:cutterGroup:view', NULL, 1, 0, 1, 1, '2025-07-10 10:55:50', NULL, NULL);
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1714, 'DRILLAMIN', '修改', 1712, '#', '', '#', 3, 'produce:cutterGroup:edit', NULL, 2, 0, 1, 1, '2025-07-10 10:57:00', '2025-07-10 11:17:00', 1);
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1715, 'DRILLAMIN', '删除', 1712, '#', '', '#', 3, 'produce:cutterGroup:remove', NULL, 3, 0, 1, 1, '2025-07-10 10:57:19', NULL, NULL);
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1716, 'DRILLAMIN', '详情', 1712, '#', '', '#', 3, 'produce:cutterGroup:detail', NULL, 4, 0, 1, 1, '2025-07-10 10:57:59', NULL, NULL);

--20250711 dpf

CREATE TABLE `t_partition_setting` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `partition_id` bigint(20) DEFAULT NULL COMMENT '分区表id',
  `config_code` varchar(100) NOT NULL COMMENT '配置项编号',
  `config_value` varchar(1000) NOT NULL COMMENT '配置项的值',
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COMMENT='分区配置';

--20250711 dpf

ALTER TABLE vg_autodrill_db.t_transportation_task ADD is_css_controlled TINYINT NULL COMMENT '中控自动运行的任务:true,代理自动运行的任务:false';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD spec_group varchar(100) NULL COMMENT '工序组';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD panel_count INT NULL COMMENT '叠数';
ALTER TABLE vg_autodrill_db.t_transportation_task ADD total_pcs INT NULL COMMENT 'PCS总数';

--20250711 dpf
ALTER TABLE vg_autodrill_db.t_task DROP COLUMN is_cutter;
ALTER TABLE vg_autodrill_db.t_task ADD cutter_group_no varchar(100) NULL COMMENT '配刀组计划No';


---20250717 dpf
-- vg_autodrill_db.t_schedule_location_panel definition

CREATE TABLE `t_schedule_location_panel` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `schedule_id` bigint(20) DEFAULT NULL COMMENT '外键：Schedule表id',
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='库位Panel信息';


---20250724 dpf
ALTER TABLE vg_autodrill_db.t_device ADD max_board_length INT NULL COMMENT '最大板长';

---20250725 dpf
ALTER TABLE vg_autodrill_db.t_cutter_group ADD locked_time DATETIME NULL COMMENT '锁定时间';


---20250726 zyy
INSERT INTO `sys_menu` VALUES (1717,'DRILLAMIN','料仓历史',1026,'#','transportationTaskHistory','device/transportationTaskHistory/index',2,'device:transportationTaskHistory:list',NULL,14,0,1,1,'2025-07-26 10:08:17','2025-07-26 10:12:30',1);

--20250804 dpf 
ALTER TABLE vg_autodrill_db.t_cutter_group ADD apt_file_path varchar(200) NULL COMMENT 'apt文件路径';
ALTER TABLE vg_autodrill_db.t_cutter_group ADD box_nos varchar(200) NULL COMMENT '刀盒二维码集合，逗号分隔';

---20250805 zyy
INSERT INTO `sys_menu` VALUES (1718,'DRILLAMIN','导出',1514,'#','','#',3,'device:transportationTask:export',NULL,1,0,1,1,'2025-08-05 16:15:54',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1719,'DRILLAMIN','导出',1717,'#','','#',3,'device:transportationTaskHistory:export',NULL,1,0,1,1,'2025-08-05 16:16:57',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1720,'DRILLAMIN','查询',1717,'#','','#',3,'device:transportationTaskHistory:list',NULL,1,0,1,1,'2025-08-05 16:17:10',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1721,'DRILLAMIN','查看',1717,'#','','#',3,'device:transportationTaskHistory:view',NULL,2,0,1,1,'2025-08-05 16:17:38',NULL,NULL);
--20250805 dpf
INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES( 'CutterGroupAptFileUrl', 'http://10.54.2.4:8080/Service/EquipmentService.svc/kwDrillKnifeToolInfo', 'string', '获取配刀组计划apt文件路径url', '', '获取配刀组计划apt文件路径url', 0, 1, 1, '2025-08-05 16:46:22', '2025-08-05 16:46:22', NULL, 0, NULL, 0, '0');

--20250806 dpf 
ALTER TABLE vg_autodrill_db.t_cutter_group CHANGE apt_file_path atp_file_path varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT 'atp文件路径';


--20250808 zyy
INSERT INTO `sys_menu` VALUES (1722,'DRILLAMIN','详情',1717,'#','','#',3,'device:transportationTaskHistory:detail',NULL,6,0,1,60,'2025-08-08 17:04:13',NULL,NULL);

--20250814 dpf
CREATE TABLE `t_transportation_history_task` (
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
  `master_device_kind` int(11) DEFAULT NULL,
  `agv_kind` int(11) DEFAULT NULL,
  `allocate_agv` varchar(100) DEFAULT NULL,
  `hk_response_key` varchar(100) DEFAULT NULL,
  `fork_location_schedule_id` varchar(100) DEFAULT NULL,
  `device_location_schedule_id` varchar(100) DEFAULT NULL,
  `start_location_code` varchar(100) DEFAULT NULL COMMENT '起始库位',
  `start_device_id` varchar(100) DEFAULT NULL COMMENT '起始设备id',
  `start_schedule_id` bigint(20) DEFAULT NULL COMMENT '起始调度id',
  `end_location_code` varchar(100) DEFAULT NULL COMMENT '终点库位',
  `end_device_id` varchar(100) DEFAULT NULL COMMENT '终点设备id',
  `end_schedule_id` bigint(20) DEFAULT NULL COMMENT '终点调度id',
  `transfer_behavior` int(11) DEFAULT NULL COMMENT '转移行为',
  `is_manual` tinyint(4) DEFAULT '0' COMMENT '是否手动创建',
  `clinker_remark` varchar(100) DEFAULT NULL COMMENT '熟料备注',
  `start_schedule` varchar(50) DEFAULT NULL COMMENT '开始库位的调度',
  `end_schedule` varchar(50) DEFAULT NULL COMMENT '结束库位的调度',
  `is_css_controlled` tinyint(4) DEFAULT NULL COMMENT '中控自动运行的任务:true,代理自动运行的任务:false',
  `spec_group` varchar(100) DEFAULT NULL COMMENT '工序组',
  `panel_count` int(11) DEFAULT NULL COMMENT '叠数',
  `total_pcs` int(11) DEFAULT NULL COMMENT 'PCS总数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COMMENT='料仓历史任务表';

INSERT INTO vg_autodrill_db.sys_config
(config_code, config_value, config_type, config_descript, config_enum_value, remark, is_deleted, status, creator_id, create_time, modify_time, modifier_id, is_system, module, is_quick_config, category)
VALUES('TransferTransportationHistorySwitch', 'false', 'bool', '迁移料仓历史任务开关', NULL, '迁移料仓历史任务开关', 0, 1, 1, '2023-10-18 15:37:29', '2023-10-18 15:37:29', 1, 0, NULL, 0, '0');

--20250818 zc
ALTER TABLE `vg_autodrill_db`.`t_alarm` 
ADD COLUMN `item_code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品编号';

--20250822 xifj
ALTER TABLE `vg_autodrill_db`.`t_silo_detail` 
ADD COLUMN `panel_length` DECIMAL(8,2) NULL COMMENT '板料长度';

INSERT INTO `sys_menu` VALUES (1707,'DRILLAMIN','维嘉生料',1148,'#','vegaItem','masterData/vegaItem',2,'masterData:vegaItem:list',NULL,1,0,1,1,'2025-04-23 17:01:56','2025-04-29 09:07:54',1);


--20250828 zyy

INSERT INTO `sys_menu` VALUES (1724,'DRILLAMIN','料仓板料追溯',1158,'#','siloPanelTrace','device/siloPanelTrace/index',2,'device:siloPanelTrace:list',NULL,11,0,1,1,'2025-08-26 14:12:49','2025-08-27 13:37:23',1);
INSERT INTO `sys_menu` VALUES (1725,'DRILLAMIN','调度记录详情',1724,'#','','#',3,'device:siloPanelTrace:schedule',NULL,1,0,1,1,'2025-08-28 09:15:45',NULL,NULL);
INSERT INTO `sys_menu` VALUES (1726,'DRILLAMIN','料仓任务详情',1724,'#','','#',3,'device:siloPanelTrace:transportationTask',NULL,2,0,1,1,'2025-08-28 09:16:30',NULL,NULL);


--20250828 zc
--t_silo_panel_trace definition
CREATE TABLE `t_silo_panel_trace` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `code` varchar(50) NOT NULL COMMENT '编号',
  `silo_code` varchar(100) DEFAULT NULL COMMENT '料仓编号',
  `location` varchar(100) NOT NULL COMMENT '位置信息',
  `silo_summary` text NOT NULL COMMENT '料仓信息汇总',
  `undrilled_item` varchar(500) DEFAULT NULL COMMENT '生料',
  `drilled_item` varchar(500) DEFAULT NULL COMMENT '熟料',
  `has_multiple_drilled` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否存在多个已钻孔料件，0-否，1-是，当drilled_item包含多个料件时此字段为1',
  `subject` varchar(500) NOT NULL COMMENT '操作主题描述，记录当前操作的类型（如：转入生料、维护物料等）',
  `schedule_id` bigint(20) DEFAULT NULL COMMENT '调度记录表ID',
  `transportation_task_id` bigint(20) DEFAULT NULL COMMENT '运输任务表ID',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '逻辑删除标识，0-未删除，1-已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '记录状态，1-启用，0-禁用',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人ID',
  `creator_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人ID',
  `modifier_name` varchar(100) DEFAULT NULL COMMENT '修改人姓名',
  PRIMARY KEY (`id`) USING BTREE COMMENT '主键索引',
  UNIQUE KEY `uk_code` (`code`) COMMENT '编号唯一索引，确保编号不重复',
  KEY `idx_silo_code` (`silo_code`) COMMENT '料仓号索引',
  KEY `idx_location` (`location`) COMMENT '位置索引',
  KEY `idx_create_time` (`create_time`) COMMENT '创建时间索引',
  KEY `idx_schedule_id` (`schedule_id`) COMMENT '调度记录表ID索引',
  KEY `idx_creator_name` (`creator_name`) COMMENT '创建人姓名索引'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='料仓板料追溯表';


---20250902 zyy
INSERT INTO `sys_menu` VALUES (1727,'DRILLAMIN','板料信息详情',1724,'#','','#',3,'device:siloPanelTrace:panel',NULL,3,0,1,1,'2025-09-02 11:41:12',NULL,NULL);


--20250902 zc
--t_silo_panel_trace_detail definition
CREATE TABLE `t_silo_panel_trace_detail` (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `master_id` bigint NOT NULL COMMENT '主表ID（关联t_silo_panel_trace表的id字段）',
  `location_code` varchar(100) DEFAULT NULL COMMENT '位置编码',
  `silo_code` varchar(50) DEFAULT NULL COMMENT '料仓编码',
  `item_code` varchar(50) DEFAULT NULL COMMENT '物料编码',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料编码',
  `floor_num` int DEFAULT NULL COMMENT '层数',
  `product_status` int DEFAULT NULL COMMENT '板料类型 30100-生料 40100-熟料',
  `pcs` int DEFAULT NULL COMMENT '每叠块数',
  `panel_width` decimal(18,2) DEFAULT NULL COMMENT '板宽',
  `panel_length` decimal(18,2) DEFAULT NULL COMMENT '板料长度',
  `pin_offset` decimal(18,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `creator_id` int DEFAULT NULL COMMENT '创建人ID',
  `creator_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `modifier_id` int DEFAULT NULL COMMENT '修改人ID',
  `modifier_name` varchar(100) DEFAULT NULL COMMENT '修改人姓名',
  `modify_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '修改时间',
  `status` int DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `is_deleted` tinyint DEFAULT '0' COMMENT '是否已删除(0:未删除;1:已删除)',
  PRIMARY KEY (`id`),
  KEY `idx_master_id` (`master_id`),
  KEY `idx_location_code` (`location_code`),
  KEY `idx_silo_code` (`silo_code`),
  KEY `idx_panel_code` (`panel_code`),
  KEY `idx_create_time` (`create_time`),
  CONSTRAINT `fk_silo_panel_trace_detail_master` FOREIGN KEY (`master_id`) REFERENCES `t_silo_panel_trace` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='料仓板料追溯详细表';


--20250904 zc
ALTER TABLE t_silo_panel_trace 
ADD COLUMN is_warning TINYINT(1) DEFAULT 0 COMMENT '告警标识(0:无告警,1:有告警)',
ADD COLUMN warning_description VARCHAR(500) DEFAULT NULL COMMENT '告警描述',
ADD COLUMN reference_record_id BIGINT DEFAULT NULL COMMENT '关联记录ID';
CREATE INDEX idx_location_silo_code ON t_silo_panel_trace(location, silo_code);
CREATE INDEX idx_location_status_deleted ON t_silo_panel_trace(location, status, is_deleted, create_time DESC);
CREATE INDEX idx_warning_location ON t_silo_panel_trace(is_warning, location);


--20250909 zc
--t_location_detail definition
CREATE TABLE `t_location_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `code` varchar(100) NOT NULL COMMENT '库位编号',
  `panel` varchar(200) NOT NULL COMMENT '板料',
  `item_code` varchar(100) NOT NULL COMMENT '物料编码',
  `batch_code` varchar(100) DEFAULT NULL COMMENT '批次编码',
  `floor_num` int(11) DEFAULT NULL COMMENT '层数',
  `panel_code` varchar(50) DEFAULT NULL COMMENT '板料编码',
  `product_status` int DEFAULT NULL COMMENT '该层板料类型:（0-空仓, 30100-生料,40100-熟料）',
  `pcs` int(8) DEFAULT NULL COMMENT '每叠片数',
  `panel_width` decimal(8,2) DEFAULT NULL COMMENT '板料宽度',
  `panel_length` decimal(8,2) DEFAULT NULL COMMENT '板料长度',
  `pin_offset` decimal(8,2) DEFAULT NULL COMMENT '梢钉偏移量',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `creator_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `modifier_name` varchar(100) DEFAULT NULL COMMENT '修改人姓名',
  PRIMARY KEY (`id`),
  KEY `idx_code` (`code`) COMMENT '库位编号索引',
  KEY `idx_item_code` (`item_code`) COMMENT '物料编码索引',
  KEY `idx_panel_code` (`panel_code`) COMMENT '板料编码索引',
  KEY `idx_batch_code` (`batch_code`) COMMENT '批次编码索引',
  KEY `idx_create_time` (`create_time`) COMMENT '创建时间索引'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='库位明细表';



---20250912 zyy

INSERT INTO `sys_menu` VALUES (1728,'DRILLAMIN','钻机-操作板料',1623,'#','','#',3,'dashboard:drill:change',NULL,1,0,1,60,'2025-09-12 10:10:24',NULL,NULL);

---20250916 zyy
UPDATE `vg_autodrill_db`.`sys_menu` SET `authorize` = 'dashboard:agv:change' WHERE (`id` = '1671');

--20250917 ycl 添加刀盒码校验状态
ALTER TABLE t_cutter_group 
ADD COLUMN check_box TINYINT(1) DEFAULT -1 COMMENT '刀盒码校验状态(-1:没有校验,0:校验失败,1:校验成功)',

--20250919 zc 添加板长校验配置到系统配置表
INSERT INTO `sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `config_enum_value`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`, `is_quick_config`, `category`
) VALUES ('EnablePanelLengthValidation', 'false', 'bool', '是否启用板长校验（钻孔工单和任务移动）', 'true,false', '控制钻孔工单创建和任务移动时的板长校验功能，防止产品板长超过设备最大板长限制', 0, 1, 1, NOW(), 1, 1, '1');

--20250930 zc  为 SiloCode 校验功能添加系统开关配置
INSERT INTO sys_config (config_code,config_value,config_type,config_descript,remark,is_system,is_quick_config,category,creator_id,create_time,status
) VALUES ('ENABLE_SILO_CODE_VALIDATION','true','bool','是否启用料仓编码校验功能','控制是否启用 SiloCode 校验功能，包括缓存校验和告警功能',1,1,40,1,NOW(),1);

--20250930 zc  为 SiloCode 校验缓存过期时间添加系统开关配置
INSERT INTO sys_config (config_code,config_value,config_type,config_descript,remark,is_system,is_quick_config,category,creator_id,create_time,status
) VALUES ('SILOCODE_CACHE_EXPIRATION_MINUTES','30','int','料仓编码校验缓存过期时间(分钟)','控制 SiloCode 校验缓存的过期时间，单位为分钟',1,1,40,1,NOW(),1);

--20250930 zc  silopaneltrace新增是否需要校验字段
ALTER TABLE t_silo_panel_trace 
ADD COLUMN need_validate_silocode TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否需要校验SiloCode(0:不需要校验,1:需要校验)';

--20251013 zc  创建agv任务汇总表
CREATE TABLE `t_device_schedule_summary` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `device_code` varchar(50) NOT NULL COMMENT '设备编码',
  `device_name` varchar(100) DEFAULT NULL COMMENT '设备名称',
  `device_type_code` varchar(50) DEFAULT NULL COMMENT '设备类型编码',
  `stats_date` date NOT NULL COMMENT '统计日期',
  `completed_count` int(11) NOT NULL DEFAULT '0' COMMENT '完成数量',
  `failed_count` int(11) NOT NULL DEFAULT '0' COMMENT '失败数量',
  `canceled_count` int(11) NOT NULL DEFAULT '0' COMMENT '取消数量',
  `total_count` int(11) NOT NULL DEFAULT '0' COMMENT '总数量(完成+失败)',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除(0:否;1:是)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `uk_device_date` (`device_code`,`stats_date`) USING BTREE,
  KEY `idx_stats_date` (`stats_date`) USING BTREE,
  KEY `idx_device_code` (`device_code`) USING BTREE,
  KEY `idx_create_time` (`create_time`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备调度汇总表';

--20251013 zc  创建agv任务详情表
CREATE TABLE `t_device_schedule_detail` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `summary_id` bigint(20) NOT NULL COMMENT '汇总表ID',
  `device_code` varchar(50) NOT NULL COMMENT '设备编码',
  `stats_date` date NOT NULL COMMENT '统计日期',
  `schedule_code` varchar(100) DEFAULT NULL COMMENT '调度任务编号',
  `schedule_id` bigint(20) DEFAULT NULL COMMENT '调度记录ID',
  `start_location` varchar(100) DEFAULT NULL COMMENT '起点位置',
  `end_location` varchar(100) DEFAULT NULL COMMENT '终点位置',
  `route_code` varchar(50) DEFAULT NULL COMMENT '工艺路线编号',
  `route_name` varchar(100) DEFAULT NULL COMMENT '工艺路线名称',
  `item_code` varchar(50) DEFAULT NULL COMMENT '产品编号',
  `item_name` varchar(100) DEFAULT NULL COMMENT '产品名称',
  `scheduled_task_status` int(11) DEFAULT NULL COMMENT '调度任务状态',
  `allocate_time` datetime DEFAULT NULL COMMENT '分配时间',
  `running_time` datetime DEFAULT NULL COMMENT '开始调度时间',
  `completed_time` datetime DEFAULT NULL COMMENT '调度完成时间',
  `failed_time` datetime DEFAULT NULL COMMENT '执行失败时间',
  `canceled_time` datetime DEFAULT NULL COMMENT '取消计划时间',
  `warning_code` varchar(50) DEFAULT NULL COMMENT '告警编码',
  `warning_message` varchar(500) DEFAULT NULL COMMENT '告警详细信息',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否已删除(0:否;1:是)',
  `creator_id` int(11) DEFAULT NULL COMMENT '创建人Id',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  `modifier_id` int(11) DEFAULT NULL COMMENT '修改人Id',
  `modify_time` datetime DEFAULT NULL COMMENT '修改时间',
  `status` int(11) NOT NULL DEFAULT '1' COMMENT '状态(1:启用;0禁用)',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `idx_summary_id` (`summary_id`) USING BTREE,
  KEY `idx_device_date` (`device_code`,`stats_date`) USING BTREE,
  KEY `idx_schedule_id` (`schedule_id`) USING BTREE,
  KEY `idx_stats_date` (`stats_date`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='设备调度详情表';


--20251016 hjl  添加系统管理配置项
INSERT INTO `vg_autodrill_db`.`sys_config` 
( `config_code`, `config_value`, `config_type`, `config_descript`, `config_enum_value`,
 `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `modify_time`, `modifier_id`, 
 `is_system`, `is_quick_config`, `category`)
 VALUES
 ( 'IsCheckSZKWSN', '2', 'int', '深圳景旺上板 是否校验板料二维码,1 校验，2 不校验', 
 '', '深圳景旺上板 是否校验板料二维码,1 校验，2 不校验', '0', '1', '1',
 sysdate(), sysdate(), '1', '0', '0', '1'),
 ( 'IsCheckSZKWSNURL', 'http://192.168.102.178/:8001/MesService.svc/kwDrPnlQuery', 'int', '深圳景旺上板 是否校验板料二维码,1 校验，2 不校验', 
 '', '深圳景旺上板 是否校验板料二维码,1 校验，2 不校验', '0', '1', '1',
 sysdate(), sysdate(), '1', '0', '0', '1');
 


 --20251016 zyy 增加AGV汇总页面
 INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1730, 'DRILLAMIN', 'AGV汇总', 1026, '#', 'agvList', 'device/agvList', 2, 'device:agvList:list', NULL, 111, 0, 1, 1, '2025-10-16 13:55:10', NULL, NULL);
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1731, 'DRILLAMIN', '导出', 1730, '#', '', '#', 3, 'device:agvList:export', NULL, 1, 0, 1, 1, '2025-10-17 15:25:12', NULL, NULL);


--20251017 zc task历史表创建
CREATE TABLE `t_task_his` (
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
  UNIQUE KEY `code_UNIQUE` (`code`),
  KEY `idx_work_order_id` (`work_order_id`) USING BTREE COMMENT '工单ID索引',
  KEY `idx_create_time` (`create_time`) USING BTREE COMMENT '创建时间索引',
  KEY `idx_task_status` (`task_status`) USING BTREE COMMENT '任务状态索引'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='生产任务历史表';


--20251017 zc task表归档开关配置
INSERT INTO `sys_config` 
    (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, 
     `status`, `is_system`, `is_quick_config`, `category`, `create_time`, `modify_time`)
VALUES ( 'TransferTaskHistorySwitch','true','bool','生产任务归档开关','是否启用生产任务归档功能，启用后会将已完成的历史任务定期归档到 t_task_his 表',
1, 1, 1, 1,NOW(),NOW())
ON DUPLICATE KEY UPDATE  `config_value` = VALUES(`config_value`),`config_type` = VALUES(`config_type`),`config_descript` = VALUES(`config_descript`),`remark` = VALUES(`remark`),`modify_time` = NOW();

--20251017 zc task表归档天数配置
INSERT INTO `sys_config` 
    (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `status`, `is_system`, `is_quick_config`, `category`, `create_time`, `modify_time`)
VALUES ('TransferTaskHistoryDays','90','int','生产任务归档天数','已完成任务超过此天数后将被归档到历史表，单位：天，默认90天',
1, 1, 1, 1,NOW(),NOW())
ON DUPLICATE KEY UPDATE `config_value` = VALUES(`config_value`),`config_type` = VALUES(`config_type`),`config_descript` = VALUES(`config_descript`),`remark` = VALUES(`remark`),`modify_time` = NOW();


--20251020 zyy 增加任务历史页面
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1734, 'DRILLAMIN', '任务历史', 1024, '#', 'taskHisList', 'produce/taskHisList', 2, 'produce:taskHisList:list', NULL, 26, 0, 1, 1, '2025-10-20 14:15:29', NULL, NULL);
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1735, 'DRILLAMIN', '导出', 1734, '#', '', '#', 3, 'produce:taskHisList:export', NULL, 1, 0, 1, 1, '2025-10-20 14:56:24', NULL, NULL);
ON DUPLICATE KEY UPDATE `config_value` = VALUES(`config_value`),`config_type` = VALUES(`config_type`),`config_descript` = VALUES(`config_descript`),`remark` = VALUES(`remark`),`modify_time` = NOW();

--20251020 zc 删除开关配置（删除开始时间是3天前包括当天的草稿任务）
INSERT INTO `sys_config` (`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `status`, `is_system`, `is_quick_config`, `category`, `create_time`, `modify_time`)
VALUES
(
'DeleteDraftTaskSwitch','false', 'bool', '定时删除草稿任务开关', '是否启用定时删除草稿任务功能，启用后会定期删除开始时间是3天前（包括当天）的草稿任务',1,1,1,1,NOW(),NOW()
)
ON DUPLICATE KEY UPDATE`config_value` = VALUES(`config_value`),`config_type` = VALUES(`config_type`),`config_descript` = VALUES(`config_descript`),`remark` = VALUES(`remark`),`modify_time` = NOW();

--20251023 zyy  增加维嘉熟料页面
INSERT INTO vg_autodrill_db.sys_menu
(id, app_code, menu_name, parent_id, menu_icon, `path`, menu_url, menu_type, authorize, remark, sort, is_deleted, status, creator_id, create_time, modify_time, modifier_id)
VALUES(1736, 'DRILLAMIN', '维嘉熟料', 1148, '#', 'vegaClinker', 'masterData/vegaDrilledMaterial', 2, 'masterData:vegaClinker:list', NULL, 1, 0, 1, 1, '2025-10-23 10:55:33', NULL, NULL);

--20251024 zc 熟料库存信息查询区域或策略编码
INSERT INTO `vg_autodrill_db`.`sys_config` 
(`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`, `category`) 
VALUES 
('StockInfoQueryDrilledPosAreaCode', 'J03-DR-IN-Schedule|J04-DR-IN-Schedule', 'string', '熟料库存信息查询区域或策略编码', '熟料库存信息查询区域或策略编码，多个区域用管道符|分隔', 0, 1, 1, NOW(), 0, '2')
ON DUPLICATE KEY UPDATE `config_value` = VALUES(`config_value`),`config_descript` = VALUES(`config_descript`),`remark` = VALUES(`remark`),`modify_time` = NOW();

--20251024 zc 熟料库存信息查询目标区域
INSERT INTO `vg_autodrill_db`.`sys_config` 
(`config_code`, `config_value`, `config_type`, `config_descript`, `remark`, `is_deleted`, `status`, `creator_id`, `create_time`, `is_system`, `category`) 
VALUES 
('StockInfoQueryDrilledPosArea', '${04}', 'string', '熟料库存信息查询目标区域', '熟料库存信息查询目标区域，区域编号${04}，策略编号${02}', 0, 1, 1, NOW(), 0, '2')
ON DUPLICATE KEY UPDATE `config_value` = VALUES(`config_value`),`config_descript` = VALUES(`config_descript`),`remark` = VALUES(`remark`),`modify_time` = NOW();