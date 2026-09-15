const Mock = require("mockjs");
const data = Mock.mock({
  "list|45": [
    {
      id: "@id",
      "title|1": ["钻机", "锣机", "AOI"],
      "status|1": ['在线','加工中','报警中','停止中','待料中'],
      "progress|5-100": 1,
      "progress1|30-50": 1,
      "state|6":[
        {
            id:"@id",
            "bool|1":[true,false],
        }
      ]
    },
  ]
});
module.exports = data;
