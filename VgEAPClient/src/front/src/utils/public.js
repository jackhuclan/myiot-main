/**
 * @description 表格显隐列columns处理
 * @param {Array} columns 原数据
 * @param {String} page 页面
 * @return {Array} 处理后的数据
 */
export function getColumns(columns, page) {
  const transferColumns = sessionStorage.getItem(page + "-columns");
  if (transferColumns) {
    const arr = columns.map((v, i) => {
      return { ...v, visible: JSON.parse(transferColumns)[i].visible };
    });
    return arr;
  } else {
    return columns;
  }
}

 
 
 