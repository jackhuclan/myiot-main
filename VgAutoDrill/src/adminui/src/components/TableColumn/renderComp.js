import Vue from "vue";

export const EventBus = new Vue();
/**
 *  FilterTable 中所有 Render 函数参数统一定义
 *  @params row: Object, index: Number, column: Object
 */
export const renderComp = {
  functional: true,
  props: {
    row: Object,
    render: Function,
    index: Number,
    column: Object,
    sc: Object,
  },
  render: (h, data) => {
    const params = {
      row: data.props.row,
      index: data.props.index,
      column: data.props.column,
      that: data.props.sc._self,
    };
    return data.props.render(h, params);
  },
};
