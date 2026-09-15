const Mock = require("mockjs");
const data = Mock.mock({
  "list|33": [
    {
      id: "@id",
      // 下一趟没有排料
      "status|+1": [
        "error",
        "running",
        "waitting",
        // "nothing",
        "running",
        "waitting",
        // "nothing",
        "running",
        "waitting",
        // "nothing",
        "running",
        "waitting",
        // "nothing",
        "running",
        "waitting",
        // "nothing",
        "running",
        "waitting",
        // "nothing",
      ],
      "color|+1": ["green", "red", "orange", "rgb(116, 30, 30)"],
      "not|1": ["1", "nothing"],
      "progress|5-100": 1,
      "duty|5-100": 1,
      "dutyFinish|5-100": 1,
      currItem: "@word(2, 5)",
      currItemNum: "@integer(10000, 300000)",
      lastItem: "@word(2, 5)",
      lastItemNum: "@integer(10000, 300000)",
    },
  ],
});
module.exports = data;
