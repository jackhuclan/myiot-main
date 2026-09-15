<template>
	<view class="uni-sub-menu">
		<view class="uni-sub-menu__title"  :class="{'is-disabled':disabled}" :style="{paddingLeft:paddingLeft}" @click="select">
			<view class="uni-sub-menu__title-sub" :style="{color:disabled?'#999':textColor}">
				<slot name="title"></slot>
			</view>
			<svg t="1744354710423" class="uni-sub-menu__icon" :class="{transition:isOpen}" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="2620" width="48" height="48"><path d="M195.2 320L512 636.16 828.8 320a32 32 0 0 1 45.44 45.44L534.4 704a32 32 0 0 1-45.44 0l-339.2-339.2a32 32 0 0 1 45.44-44.8z" fill="#666" p-id="2621"></path></svg>
		</view>
		<view class="uni-sub-menu__content" :class="{'uni-sub-menu--close':!isOpen}" :style="{'background-color':backgroundColor}">
			<view id="content--hook">
				<slot></slot>
			</view>
		</view>
	</view>
</template>

<script>
	import rootParent from '../uni-nav-menu/mixins/rootParent.js'
	export default {
		name: 'uniSubMenu',
		mixins: [rootParent],
		props: {
			// 唯一标识
			index: {
				type: [String,Object],
				default(){
					return ''
				}
			},
			// TODO 自定义类名
			popperClass: {
				type: String,
				default: ''
			},
			// TODO 是否禁用
			disabled: {
				type: Boolean,
				default: false
			},
			// 展开菜单的背景色
			backgroundColor: {
				type: String,
				default: '#f5f5f5'
			},
		},
		data() {
			return {
				height: 0,
				oldheight: 0,
				isOpen: false,
				textColor:'#303133'
			};
		},
		computed: {
			paddingLeft() {
				return 20 + 20 * this.rootMenu.SubMenu.length + 'px'
			}
		},
		created() {
			this.init()
		},
		destroyed() {
			// 销毁页面后，将当前页面实例从数据中删除
			if (this.$menuParent) {
				const menuIndex = this.$menuParent.subChildrens.findIndex(item => item === this)
				this.$menuParent.subChildrens.splice(menuIndex, 1)
			}
		},
		methods: {
			init() {
				// 所有父元素
				this.rootMenu = {
					NavMenu: [],
					SubMenu: []
				}
				this.childrens = []
				this.indexPath = []
				// 获取直系的所有父元素实例
				this.getParentAll('SubMenu', this)
				// 获取最外层父元素实例
				this.$menuParent = this.getParent('uniNavMenu', this)
				this.textColor = this.$menuParent.textColor
				// 直系父元素 SubMenu
				this.$subMenu = this.rootMenu.SubMenu

				// 将当前插入到menu数组中
				if(this.$menuParent){
					this.$menuParent.subChildrens.push(this)
				}
			},
			select() {
				if(this.disabled) return
				// 手动开关 sunMenu
				this.$menuParent.selectMenu(this)
			},
			open() {
				this.isOpen = true
			},
			close() {
				this.isOpen = false
			}
		}
	}
</script>

<style lang="scss">
	.uni-sub-menu {
		position: relative;
		/* background-color: #FFFFFF; */
	}

	.uni-sub-menu__title {
		display: flex;
		align-items: center;
		padding: 0 20px;
		padding-right: 10px;
		height: 56px;
		line-height: 56px;
		color: #303133;
		cursor: pointer;
		/* border-bottom: 1px #f5f5f5 solid; */
	}

	.uni-sub-menu__title:hover {
		color: #42B983;
		outline: none;
		background-color: #EBEBEB;
	}

	.uni-sub-menu__title-sub {
		display: flex;
		align-items: center;
		flex: 1;
	}

	.uni-sub-menu--close {
		height: 0;
		/* transition: all 0.3s; */
	}

	.uni-sub-menu__content {
		overflow: hidden;
	}

	.uni-sub-menu__icon {
		width: 15px;
		height: 15px;
		font-size: 14px;
		color: #999;
		max-height: auto;
		transition: all 0.2s;
	}

	.transition {
		transform: rotate(-180deg);
	}

	.is-disabled {
		/* background-color: #f5f5f5; */
		color: red;
	}
	.uni-sub-menu__title.is-disabled:hover {
		background-color: inherit;
		color: #999;
		cursor: not-allowed;
	}

</style>
