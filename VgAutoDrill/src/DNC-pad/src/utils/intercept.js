import {
	getToken
} from "./auth";

let config = {
	//不需要登录的页面,白名单
	whiteList: ["/pages/error/404"],
	hasTokenList: [
		//   "/pages/login/login",
		"/pages/index/index",
		"/pages/setting/setting",
		"/pages/logs/logs",
		"/pages/mine/pc",
		"/pages/mine/mobile",
		"/pages/mine/info/index",
		"/pages/mine/info/edit",
		"/pages/mine/pwd/index",
		"/pages/mine/setting/index",
		"/pages/mine/about/index",
		//   "/pages/404_h5",
	],
	//登录页
	loginPage: "/pages/login/login",
};
export default function routingIntercept() {
	//  H5路由拦截,用于拦截用户地址栏输入地址
	//   #ifdef H5
	let token = getToken();
	let locationUrl = window.location.href.split("/#")[1];
	if (!token) {
		uni.reLaunch({
			url: "/pages/login/login",
		});
	} else {
		if (locationUrl == "/" || locationUrl == "/pages/login/login") {
			// 跳转首页
			setTimeout(() => {
				uni.switchTab({
					url: "/pages/index/index",
				});
			});
		}
	}

	// #endif
}