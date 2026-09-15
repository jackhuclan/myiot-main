export default {
  //   baseUrl: "http://192.168.102.178:8003/api",
  // 应用信息
  appInfo: {
    // 应用名称
    name: "DNC-app",
    // 应用版本
    version: "1.2.0",
    // 应用logo
    logo: "/static/logo.png",
    // 公司网站
    url: "http://www.vegacnc.cn",
  },
  login: {
    url: "/pages/login/login", // 登录页面路径
  },
  index: {
    url: "/pages/index/index", // 登录后跳转的第一个页面
  },
  error: {
    url: "/pages/error/404", // 404 Not Found 错误页面路径
  },
  navBar: {
    // 顶部导航
    logo: "/static/logo.png", // 左侧 Logo
    langs: [
      {
        text: "中文简体",
        lang: "zh-Hans",
      },
      {
        text: "中文繁體",
        lang: "zh-Hant",
      },
      {
        text: "English",
        lang: "en",
      },
    ],
    themes: [
      {
        text: "默认",
        value: "default",
      },
      {
        text: "绿柔",
        value: "green",
      },
    ],
  },
  sideBar: {
    // 左侧菜单
    // 配置静态菜单列表（放置在用户被授权的菜单列表下边）
    staticMenu: [
      {
        menu_id: "demo",
        text: "静态功能演示",
        icon: "admin-icons-kaifashili",
        url: "",
        children: [
          {
            menu_id: "icons",
            text: "图标",
            icon: "admin-icons-icon",
            value: "/pages/demo/icons/icons",
          },
          {
            menu_id: "table",
            text: "表格",
            icon: "admin-icons-table",
            value: "/pages/demo/table/table",
          },
        ],
      },
      {
        menu_id: "admim-doc-pulgin",
        text: "文档与插件",
        icon: "admin-icons-eco",
        url: "",
        children: [
          {
            menu_id: "admin-doc",
            icon: "admin-icons-doc",
            text: "uni-admin 框架文档",
            value: "https://uniapp.dcloud.net.cn/uniCloud/admin",
          },
          {
            menu_id: "stat-doc",
            icon: "admin-icons-help",
            text: "uni 统计教程",
            value: "https://uniapp.dcloud.net.cn/uni-stat-v2.html",
          },
          {
            menu_id: "admin-pulgin",
            icon: "admin-icons-pulgin",
            text: "uni-admin 插件",
            value: "https://ext.dcloud.net.cn/?cat1=7&cat2=74",
          },
        ],
      },
    ],
  },
  uniStat: {},
};
