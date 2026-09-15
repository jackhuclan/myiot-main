//npm run build打包前执行此段代码
const path = require("path");
const fs = require("fs");

//返回package的json数据
function getPackageJson() {
  let data = fs.readFileSync("./public/version.json", "utf-8"); //fs读取文件
  if (Boolean(data)) return JSON.parse(data); //转换为json对象
}
let packageData = getPackageJson(); //获取package的json
// 结果为空不执行一下代码
if (!packageData) return;
let arr = packageData.version.split("."); //切割后的版本号数组
let arr1 = packageData.version.split(".");
if (!arr1[3]) {
  arr[3] = 0;
}
arr[1] =
  arr[1] == new Date().getMonth() + 1 ? arr[1] : new Date().getMonth() + 1;
arr[2] = arr[2] == new Date().getDate() ? arr[2] : new Date().getDate();
arr[3] = arr1[2] == new Date().getDate() ? parseInt(arr[3]) + 1 : 1;
packageData.version = arr.join("."); //转换为以"."分割的字符串
//用packageData覆盖package.json内容
fs.writeFile(
  "./public/version.json",
  JSON.stringify(packageData, null, "\t"),
  (err) => {}
);

console.log("Version: ", packageData.version);
