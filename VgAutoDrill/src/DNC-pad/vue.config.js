"use strict";
const path = require("path");

function resolve(dir) {
  return path.join(__dirname, dir);
}

const name = "项目名称";

module.exports = {
  transpileDependencies: ["@dcloudio/uni-ui"],
  configureWebpack: {
    resolve: {
      alias: {
        "@": resolve("src"),
        "@api": resolve("src/api"),
        "@components": resolve("src/components"),
        "@styles": resolve("src/static/style"),
        "@utils": resolve("src/utils"),
        "@pages": resolve("src/pages"),
      },
    },
  },
  devServer: {
    client: {
      //取消全屏提示错误
      overlay: false,
    },
  },
};
