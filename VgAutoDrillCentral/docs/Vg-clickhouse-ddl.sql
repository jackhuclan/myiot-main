CREATE DATABASE IF NOT EXISTS vg_autodrill_db_ck;

use vg_autodrill_db_ck;

CREATE TABLE vg_autodrill_db_ck.device_property_trace
(
    `id` UUID DEFAULT generateUUIDv4(),
    `device_id` String COMMENT '设备ID',
    `product_id` String COMMENT '产品',
    `properpty_json` String COMMENT '属性数据',
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备属性记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_status_trace
(
    `id` UUID DEFAULT generateUUIDv4(),
    `device_id` String COMMENT '设备ID',
    `product_id` String COMMENT '产品',
    `old_status` String COMMENT '设备状态Online 0 ,Offline 1,Ready,Working,Exception',
    `new_status` String COMMENT '设备状态Online 0 ,Offline 1,Ready,Working,Exception',         
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备状态记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_event_trace
(      
    `id` UUID DEFAULT generateUUIDv4(),
    `device_id` String COMMENT '设备ID',   
    `product_id` String COMMENT '产品',
    `event_id` String COMMENT '事件ID',
    `event_name` String COMMENT '事件名称',
    `request_json` String COMMENT 'request json',
    `response_json` String COMMENT 'response json',
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备事件记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_service_trace
(
    `id` UUID DEFAULT generateUUIDv4(),
    `device_id` String COMMENT '设备ID',
    `product_id` String COMMENT '产品',
    `service_id` String COMMENT '服务',
    `event_id` String COMMENT '事件ID',
    `event_name` String COMMENT '事件名称',
    `target_device_id` String COMMENT '目标设备ID',
    `target_product_id` String COMMENT '目标产品',
    `request_json` String COMMENT 'request json',
    `response_json` String COMMENT 'response json',
    `createtime` DateTime DEFAULT now()
)
ENGINE = MergeTree
ORDER BY device_id
SETTINGS index_granularity = 8192
COMMENT '设备服务记录';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.device_alarm_trace
(
    `id` UInt64 COMMENT '主键ID',
    `device_id` String COMMENT '设备ID',
    `product_id` String COMMENT '产品',
    `event_id` String COMMENT '关联事件ID',
    `trace_id` String COMMENT '关联某次事件TraceID',
    `alarm_code` String COMMENT '告警id',
    `alarm_level` UInt8 COMMENT '告警级别',
    `alarm_json` String COMMENT '告警数据',
    `handled` UInt8 COMMENT '是否处理,0未处理,1已处理,2正在处理',
    `alarm_time` DateTime COMMENT '告警时间',
    `createtime` DateTime DEFAULT now() COMMENT '告警时间'
)
ENGINE = MergeTree
ORDER BY id
SETTINGS index_granularity = 8192
COMMENT '设备告警记录';


CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.logs
(
    sequenceID Int64,
    application String,
    version String,
    level LowCardinality(String),
    message String,
    logger String,
    callSite Nullable(String),
    exception Nullable(String),
    logged DateTime64
)
ENGINE Log
COMMENT '系统日志表';

CREATE TABLE IF NOT EXISTS vg_autodrill_db_ck.schedule_logs
(
    sequenceID Int64,
    level LowCardinality(String),
    message String,
    logged DateTime64
)
ENGINE Log
COMMENT '中控调度日志表';
