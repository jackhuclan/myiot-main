export default{
    inserted(el, binding) {
        //定义一个观察器，entries为状态改变元素的数组
        let observer = new IntersectionObserver((entries) => {
    
          // 遍历
          for (let i of entries) {
            // 如果改元素处于可视区
            if (i.isIntersecting > 0) {
              // 获取该元素
              let img = i.target;
              // 重新设置src值
              img.src = binding.value;
              //取消对该元素的观察
              observer.unobserve(img);
            }
          }
        });
        // 为 img 标签添加一个观察
        observer.observe(el);
      },
}