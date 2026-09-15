export default {
  title: '维嘉智慧工厂全自动平台',
  description: 'Just playing around.',
  themeConfig: {
    sidebar: [
      {
        text: '项目背景介绍',
        items: [
          { text: '项目背景', link: '/introduction' },
          { text: '需求要点', link: '/getting-started' },
        ]
      },
      {
        text: '系统设计',
        items: [
          { text: '系统建模', link: '/design/model' },
          { text: '事件驱动模型', link: '/design/event-driven' },
          { text: '整体架构', link: '/design/architecture' },
          { text: '什么是物模型', link: '/design/tsl' },
          { text: '物模型TSL字段说明', link: '/design/tsl-detail' },
          { text: '物模型支持的数据类型', link: '/design/tsl-datatype' },
          { text: 'Topic规范', link: '/design/topic' },
          { text: 'DeviceAgent实现规范', link: '/design/device-agent' },
          { text: '叠板机与AGV协同工作流程', link: '/design/device-agent' },
          { text: '拆板机与AGV协同工作流程', link: '/design/device-agent' },
          { text: '钻机与AGV协同工作流程', link: '/design/device-agent' },
          { text: '生料区与AGV协同工作流程', link: '/design/device-agent' },
          { text: '熟料区与AGV协同工作流程', link: '/design/device-agent' },
        ]
      },
      {
        text: '系统部署',
        items: [
          { text: '单机部署', link: '/deploy/mono' },
          { text: '集群部署', link: '/deploy/cluster' },
        ]
      },
      {
        text: '项目管理',
        items: [
          { text: '阶段任务', link: '/manage/plan' },
          { text: '任务分配', link: '/manage/assign' },
          { text: '人员管理', link: '/manage/people' },
          { text: '绩效考核', link: '/manage/kpi' },
        ]
      },
      {
        text: '用户手册',
        items: [
          {
            text: '系统管理', 
            items: [
              { text: '用户管理', link: '/sys/user' },
              { text: '角色管理', link: '/sys/role' },
              { text: '菜单管理', link: '/sys/menu' },
              { text: '部门管理', link: '/sys/dept' },
              { text: '岗位管理', link: '/sys/position' },
              { text: '应用管理', link: '/sys/app' },
            ]
          },
          {
            text: '设备管理', 
            items: [
              { text: '仪表盘', link: '/device/gauge' },
              { text: '产品', link: '/device/product' },
              { text: '设备', link: '/device/index' },
            ]
          },
          {
            text: '排产管理', 
            items: [
              { text: '排产管理', link: '/sys/user' },
            ]
          },
          {
            text: '订单管理', 
            items: [
              { text: '订单管理', link: '/sys/user' },
            ]
          },
          {
            text: '通知管理', 
            items: [
              { text: '通知设置', link: '/sys/user' },
              { text: '通知模板', link: '/sys/role' },
            ]
          },
          {
            text: '告警中心', 
            items: [
              { text: '仪表盘', link: '/sys/user' },
              { text: '告警设置', link: '/sys/role' },
              { text: '告警记录', link: '/sys/menu' },
            ]
          },
          {
            text: '运维管理', 
            items: [
              { text: '仪表盘', link: '/sys/user' },
              { text: '远程升级', link: '/sys/role' },
            ]
          },
          {
            text: '物料管理', 
            items: [
              { text: '刀具管理', link: '/sys/user' },
              { text: '配方管理', link: '/sys/user' },
            ]
          },
        ]
      }
    ]
  }
}