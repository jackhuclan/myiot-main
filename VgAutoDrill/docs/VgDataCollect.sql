CREATE DATABASE IF NOT EXISTS vg_autodrill_db_ck;

use vg_autodrill_db_ck;

CREATE TABLE vg_autodrill_db_ck.device_property_trace
(

    `device_id` String COMMENT '设备ID',

    `product_id` String COMMENT '产品',

    `properpty_json` String COMMENT '属性数据',

    `id` UUID DEFAULT generateUUIDv4(),

    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备属性记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_status_trace
(
    `device_id` String COMMENT '设备ID',
    
    `product_id` String COMMENT '产品',

    `old_status` String COMMENT '设备状态Online 0 ,Offline 1,Ready,Working,Exception',
    
    `new_status` String COMMENT '设备状态Online 0 ,Offline 1,Ready,Working,Exception',  
        
    `id` UUID DEFAULT generateUUIDv4(),
    
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备状态记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_event_trace
(
    `device_id` String COMMENT '设备ID',
    
    `product_id` String COMMENT '产品',
	   
    `event_id` String COMMENT '事件ID',
    
    `event_name` String COMMENT '事件名称',
    
    `request_json` String COMMENT 'request json',

    `response_json` String COMMENT 'response json',
        
    `id` UUID DEFAULT generateUUIDv4(),
    
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备事件记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_service_trace
(
    `device_id` String COMMENT '设备ID',
    
    `product_id` String COMMENT '产品',

    `service_id` String COMMENT '服务',
    
    `event_id` String COMMENT '事件ID',
    
    `event_name` String COMMENT '事件名称',
    
    `target_device_id` String COMMENT '目标设备ID',
    
    `target_product_id` String COMMENT '目标产品',
    
    `request_json` String COMMENT 'request json',
    
    `response_json` String COMMENT 'response json',
        
    `id` UUID DEFAULT generateUUIDv4(),
    
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备服务记录';