<template>
  <edit-form-dialog
    v-model="open"
    title="排刀信息"
    submitText="生成排刀文件"
    submitType="success"
    @submitForm="submitForm"
    width="90%"
  >
    <div class="box">
      <el-form ref="form" :model="form" label-width="100px">
        <el-row>
          <el-col :span="6">
            <el-form-item label="物料编码" prop="itemCode">
              <el-input v-model="form.itemCode" disabled> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="物料名称" prop="itemName">
              <el-input v-model="form.itemName" disabled />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label="刀盘数量" prop="diskCount">
              <el-input v-model="form.diskCount" disabled />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label="单刀盘列数" prop="columnsLimit">
              <el-input v-model="form.columnsLimit" disabled />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label="单刀盘行数" prop="rowsLimit">
              <el-input v-model="form.rowsLimit" disabled /> </el-form-item
          ></el-col>
        </el-row>
      </el-form>
      <el-card>
        <template>
          <el-button
            v-debounce
            plain
            type="primary"
            icon="el-icon-circle-plus-outline"
            @click="openCutterSelect"
            >选择</el-button
          >
          <el-button
            v-debounce
            plain
            type="danger"
            icon="el-icon-delete"
            @click="del"
            >删除</el-button
          >
          <el-button
            plain
            type="warning"
            icon="el-icon-more"
            @click="selectAll"
            >{{ isSelectAll ? "取消全选" : "全选" }}</el-button
          >
        </template>

        <div
          class="table_box"
          id="selectContainer"
          unselectable="on"
          onselectstart="return false"
          style="
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
          "
          @mousedown="handleMouseDown"
        >
          <div
            class="mask"
            v-show="is_show_mask"
            :style="
              'width:' +
              mask_width +
              'left:' +
              mask_left +
              'height:' +
              mask_height +
              'top:' +
              mask_top
            "
          ></div>
          <div>
            <div
              class="bg"
              v-for="(item, index) in multiList"
              :key="'刀具' + index"
            >
              <span class="bg_index">{{ index + 1 }}</span>
              <table border="1" class="cutter_table">
                <tr v-for="(v, ind) in item" :key="'第二层' + ind">
                  <td
                    class="td"
                    v-for="val in v"
                    :key="val.id"
                    @click="handelCutterSelect(val)"
                    :style="{ background: val.color ? val.color : '' }"
                  >
                    <span :style="{ color: val.diameter ? 'red' : '' }">{{
                      val.text
                    }}</span>
                  </td>
                </tr>
              </table>
            </div>
          </div>
        </div>
        <template>
          <el-row style="margin-top: 10px; display: flex; align-items: center">
            <el-col :span="24">
              <span class="select_span">
                当前选中:
                <el-tag
                  class="select_tag"
                  v-for="item in selectList"
                  :key="'选中' + item"
                  >{{ item }}</el-tag
                ><span class="select_span" v-if="selectList.length <= 0"
                  >无</span
                ></span
              >
            </el-col>
          </el-row>
        </template>
      </el-card>
    </div>
    <!-- 刀具 -->
    <cutterSelect
      ref="cutterSelect"
      @onSelected="onCutterSelect"
    ></cutterSelect>
  </edit-form-dialog>
</template>

<script>
import cutterSelect from "@/components/cutterSelect";
import { generateATP } from "@/api/material/atpFile";
import {
  listMultiList,
  updateMultiList,
} from "@/api/material/itemAtpFileDetail";
export default {
  components: {
    cutterSelect,
  },
  data() {
    return {
      // 弹框
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        diameter: undefined,
        orderNum: undefined,
        itemAtpFileId: undefined,
      },
      // 是否全选
      isSelectAll: false,
      // 数据
      multiList: [],
      // 选中后的数据
      selectList: [],
      selectIdList: [],
      form: {},
      is_show_mask: false,
      box_screen_left: 0,
      box_screen_top: 0,
      start_x: 0,
      start_y: 0,
      end_x: 0,
      end_y: 0,
    };
  },
  updated() {
    this.$nextTick(() => {
      const dom_box = document.querySelector(".table_box");
      this.box_screen_left = dom_box?.getBoundingClientRect().left;
      this.box_screen_top = dom_box?.getBoundingClientRect().top;
    });
  },
  computed: {
    mask_width() {
      return `${Math.abs(this.end_x - this.start_x)}px;`;
    },
    mask_height() {
      return `${Math.abs(this.end_y - this.start_y)}px;`;
    },
    mask_left() {
      return `${Math.min(this.start_x, this.end_x) - this.box_screen_left}px;`;
    },
    mask_top() {
      return `${Math.min(this.start_y, this.end_y) - this.box_screen_top}px;`;
    },
  },
  watch: {
    open(val) {
      if (!val) {
        this.selectList = [];
        this.selectIdList = [];
      }
    },
  },
  methods: {
    // 获取数据
    getList() {
      listMultiList(this.queryParams).then((res) => {
        this.multiList = res.data.map((v) => {
          return v.map((item) => {
            return item.map((v1) => {
              return { ...v1, color: "", checked: false };
            });
          });
        });
      });
    },
    // 点击选择打开弹框
    openCutterSelect() {
      if (this.selectList.length <= 0)
        return this.$modal.msgError("请选择刀盘");
      this.$refs.cutterSelect.showFlag = true;
    },
    // 点击刀具选择
    handelCutterSelect(obj, index) {
      // 选择及反选
      if (!obj.checked) {
        obj.color = "#ccc";
        obj.checked = true;
        this.selectList.push(obj.orderNum);
        this.selectList = [...new Set(this.selectList)];
        this.selectIdList.push(obj.id);
        this.selectIdList = [...new Set(this.selectIdList)];
      } else {
        obj.color = "#fff";
        obj.checked = false;
        this.selectList = this.selectList.filter((v) => v !== obj.orderNum);
        this.selectIdList = this.selectIdList.filter((v) => v !== obj.id);
      }
    },
    // 刀具选择
    async onCutterSelect(obj) {
      const results = await this.$modal
        .confirm("确认执行操作？")
        .catch(() => {});
      if (results != "confirm") {
        return;
      }
      let form = [];
      this.selectIdList.forEach((v) => {
        form.push({
          id: v,
          status: undefined,
          itemAtpFileId: undefined,
          code: obj.code,
          diameter: obj.diameter,
          orderNum: 0,
        });
      });
      updateMultiList(form).then((res) => {
        this.getList();
        this.$modal.msgSuccess("配置成功");
      });
      this.selectList = [];
      this.selectIdList = [];
    },
    // 全选
    selectAll() {
      this.isSelectAll = !this.isSelectAll;
      if (this.isSelectAll) {
        this.multiList.map((v) => {
          v.map((item) => {
            item.map((val) => {
              val.color = "#ccc";
              val.checked = true;
              this.selectList.push(val.orderNum);
              this.selectList = [...new Set(this.selectList)].sort(
                (a, b) => a - b
              );
              this.selectIdList.push(val.id);
              this.selectIdList = [...new Set(this.selectIdList)];
            });
          });
        });
      } else {
        this.multiList.map((v) => {
          v.map((item) => {
            item.map((val) => {
              val.color = "#fff";
              val.checked = false;
            });
          });
        });
        this.selectList = [];
        this.selectIdList = [];
      }
    },
    // 删除
    del() {
      let form = [];
      this.selectIdList.forEach((v) => {
        form.push({
          id: v,
          status: undefined,
          itemAtpFileId: undefined,
          code: "",
          diameter: 0,
        });
      });
      updateMultiList(form).then((res) => {
        this.getList();
      });
      this.getList();
      this.selectList = [];
      this.selectIdList = [];
    },

    // 点击生成
    submitForm() {
      generateATP(this.form.id)
        .then((res) => {
          this.$modal.msgSuccess("生成成功");
          this.open = false;
          this.$emit("getList");
        })
        .catch(() => {});
    },
    handleMouseDown(event) {
      this.is_show_mask = true;
      this.start_x = event.clientX;
      this.start_y = event.clientY;
      this.end_x = event.clientX;
      this.end_y = event.clientY;
      document.addEventListener("mousemove", this.handleMouseMove);
      document.addEventListener("mouseup", this.handleMouseUp);
    },
    handleMouseMove(event) {
      if (this.is_show_mask) {
        this.end_x = event.clientX;
        this.end_y = event.clientY;
      }
    },
    handleMouseUp() {
      this.is_show_mask = false;
      document.body.removeEventListener("mousemove", this.handleMouseMove);
      document.body.removeEventListener("mouseup", this.handleMouseUp);
      this.handleDomSelect();
      this.resSetXY();
    },
    handleDomSelect() {
      const dom_mask = window.document.querySelector(".mask");
      const rect_select = dom_mask.getClientRects()[0];
      const add_id_list = [];
      const del_id_list = [];
      const add_list = [];
      const del_list = [];
      document.querySelectorAll(".td").forEach((node, index) => {
        const rects = node.getClientRects()[0];
        if (this.collide(rects, rect_select) === true) {
          const item = this.multiList.flat(Infinity)[index];
          if (this.selectList.includes(item.orderNum)) {
            item.color = "#fff";
            item.checked = true;
            del_list.push(this.multiList.flat(Infinity)[index].orderNum);
            del_id_list.push(this.multiList.flat(Infinity)[index].id);
          } else {
            this.multiList.flat(Infinity)[index].color = "#ccc";
            this.multiList.flat(Infinity)[index].checked = true;
            add_list.push(this.multiList.flat(Infinity)[index].orderNum);
            add_id_list.push(this.multiList.flat(Infinity)[index].id);
          }
        }
      });
      this.selectList = this.selectList
        .concat(add_list)
        .filter((item) => !del_list.includes(item));
      this.selectIdList = this.selectIdList
        .concat(add_id_list)
        .filter((item) => !del_id_list.includes(item));
    },
    collide(rect1, rect2) {
      const maxX = Math.max(rect1?.x + rect1?.width, rect2?.x + rect2?.width);
      const maxY = Math.max(rect1?.y + rect1?.height, rect2?.y + rect2?.height);
      const minX = Math.min(rect1?.x, rect2?.x);
      const minY = Math.min(rect1?.y, rect2?.y);
      if (
        maxX - minX <= rect1?.width + rect2?.width &&
        maxY - minY <= rect1?.height + rect2?.height
      ) {
        return true;
      } else {
        return false;
      }
    },
    resSetXY() {
      this.start_x = 0;
      this.start_y = 0;
      this.end_x = 0;
      this.end_y = 0;
    },
  },
};
</script>

<style lang="scss" scoped>
.table_box {
  width: 100%;
  user-select: none;
  position: relative;
  overflow: auto;
  padding: 20px;
  .mask {
    position: absolute;
    background: #409eff;
    opacity: 0.4;
  }
  > div {
    display: flex;
    align-items: center;
    // flex-wrap: wrap;
    .bg {
      margin: 10px;
      flex-shrink: 0;
      position: relative;
      .bg_index {
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        font-weight: bold;
        color: #241f1f;
        opacity: 0.3;
        font-size: 80px;
      }
    }
  }
}
.cutter_table {
  border-collapse: collapse;

  td {
    width: 40px;
    height: 40px;
    text-align: center;
  }
}
.select_span {
  text-align: center;
  line-height: 30px;
  margin-right: 5px;
  height: 30px;
}
.select_tag {
  margin-right: 4px;
  margin-bottom: 4px;
}
</style>
