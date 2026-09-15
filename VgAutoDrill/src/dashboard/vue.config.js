const { defineConfig } = require("@vue/cli-service");
const path = require("path");
const UnusedFilesW5WebpackPlugin = require("useless-files-w5-webpack-plugin");
const port = process.env.port || process.env.npm_config_port || 8888; // 端口
function resolve(dir) {
  return path.join(__dirname, dir);
}
module.exports = defineConfig({
  transpileDependencies: false, //解决IE兼容问题
  lintOnSave: false,
  publicPath: "./",
  outputDir: "dist",
  productionSourceMap: false, // 是否在构建生产包时生成 sourceMap 文件，false将提高构建速度
  pages: {
    index: {
      entry: "src/main.js", // 入口文件
      title: "车间看板",
    },
  },
  devServer: {
    hot: true, //热更新
    host: "0.0.0.0",
    port: port,
    open: true,
    historyApiFallback: true, //解决vue页面刷新history路由丢失
    compress: true, // 是否启动压缩 gzip
  },
  configureWebpack: {
    // 缓存生成的 webpack 模块和 chunk，来改善构建速度
    cache: {
      type: "filesystem",
      allowCollectingMemory: true,
    },
    resolve: {
      alias: {
        "@": path.resolve(__dirname, "./src"),
      },
    },
    plugins: [
      // new UnusedFilesW5WebpackPlugin({
      //   root: ["./src"], // 项目目录
      //   // output: "./fileList.json", // 输出文件列表
      //   clean: false, // 是否删除文件, 不建议开启，手动删除比较好，防止误删
      //   exclude: ["node_modules"], // 排除文件列表
      // }),
    ],
  },
  chainWebpack: (config) => {
    // config.module
    //   .rule('min-image')
    //   .test(/\.(png|jpe?g|gif|svg)(\?.*)?$/)
    //   .use('image-webpack-loader')
    //   .loader('image-webpack-loader')
    //   .options({ disable: process.env.NODE_ENV == 'development' ? true : false })//此处为ture的时候不会启用压缩处理,目的是为了开发模式下调试速度更快,网上错误示例直接写为disable:true,如果不去查看文档肯定是要被坑的
    //   .end()
    if (process.env.NODE_ENV === "production") {
      // 生产
      // 删除console、debugger、注释
      const terser = config.optimization.minimizer("terser");
      terser.tap((args) => {
        const { terserOptions } = args[0];
        Object.assign(terserOptions, {
          compress: {
            ...terserOptions.compress,
            drop_console: true,
            drop_debugger: true,
          },
          format: {
            comments: /@license/i,
          },
        });
        return args;
      });
    }

    config.plugins.delete("prefetch"); //删除预加载 提升首次加载速度
    config.optimization.splitChunks({
      cacheGroups: {
        styles: {
          name: "styles",
          test: /\.(s?css|less|sass)$/,
          chunks: "all",
          priority: 10,
        },
        common: {
          name: "chunk-common",
          chunks: "all",
          minChunks: 2, // 拆分前必须共享模块的最小 chunks 数。
          maxInitialRequests: 5, // 打包后的入口文件加载时，还能同时加载js文件的数量（包括入口文件）
          minSize: 0, // 生成 chunk 的最小体积（≈ 20kb)
          priority: 1, // 优化将优先考虑具有更高 priority（优先级）的缓存组
          reuseExistingChunk: true, // 如果当前 chunk 包含已从主 bundle 中拆分出的模块，则它将被重用，而不是生成新的模块
        },
        vendors: {
          name: "chunk-vendors",
          test: /[\\/]node_modules[\\/]/,
          chunks: "all",
          priority: 2,
          reuseExistingChunk: true,
        },
        lrucache: {
          name: "chunk-lrucache",
          test: /[\\/]node_modules[\\/]_?lru-cache(.*)/,
          chunks: "all",
          priority: 3,
          reuseExistingChunk: true,
        },
      },
    });
  },
});
