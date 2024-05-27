/*****************************************************************************************
**  Author:COCO 2022
*****************************************************************************************/
//此js文件是用来自定义扩展业务代码，可以扩展一些自定义页面或者重新配置生成的代码
import { h, resolveComponent } from 'vue';
import modelHeader from "./production_extend/ProductModelBody.vue"
import gridFooter from './production_extend/SalesOrderGridFooter.vue';
import FileSaver from 'file-saver';
let extension = {
  components: {
    //查询界面扩展组件
    gridHeader: '',
    gridBody: '',
    gridFooter: gridFooter,
    //新建、编辑弹出框扩展组件
    modelHeader: modelHeader,
    modelBody: {
      render() {
        return [
          h(resolveComponent('el-alert'), {
            style: { 'margin-bottom': '5px' },
            'show-icon': true, type: 'warning',
            closable: false, title: '点击【数量】可以对订单数量直接进行修改'
          }, ''),
        ]
      }
    },
    modelFooter: ''
  },
  tableAction: '', //指定某张表的权限(这里填写表名,默认不用填写)
  buttons: { view: [], box: [], detail: [] }, //扩展的按钮
  methods: {
    onActivated() {
      let productPlanCode = this.$route.query.ProductPlanCode;

      if (productPlanCode) {
        var param = {
          order: "desc",
          page: 1,
          rows: 30,
          sort: "CreateDate",
          wheres: "[{\"name\":\"ProductPlanCode\",\"value\":\"" + productPlanCode + "\",\"displayType\":\"like\"}]"
        };
        this.http.post('/api/Production_ProductPlan/getPageData', param, true).then((result) => {
          this.$refs.table.rowData = result.rows;
        });
      }
    },
     //下面这些方法可以保留也可以删除
    onInit() {  //框架初始化配置前，
        //示例：在按钮的最前面添加一个按钮
        //   this.buttons.unshift({  //也可以用push或者splice方法来修改buttons数组
        //     name: '按钮', //按钮名称
        //     icon: 'el-icon-document', //按钮图标vue2版本见iview文档icon，vue3版本见element ui文档icon(注意不是element puls文档)
        //     type: 'primary', //按钮样式vue2版本见iview文档button，vue3版本见element ui文档button
        //     onClick: function () {
        //       this.$Message.success('点击了按钮');
        //     }
        //   });

        //示例：设置修改新建、编辑弹出框字段标签的长度
        // this.boxOptions.labelWidth = 150;
        //显示序号(默认隐藏)
        this.single = true;
        this.columnIndex = true;
        //点击单元格编辑与结束编辑(默认是点击单元格编辑，鼠标离开结束编辑)
        this.detailOptions.clickEdit = true;
        this.tableMaxHeight = (document.body.clientHeight - 260) / 2;
        this.buttons.splice(3,0,{  //也可以用push或者splice方法来修改buttons数组
          name: '打印', //按钮名称
          icon: 'el-icon-printer', //按钮图标vue2版本见iview文档icon，vue3版本见element ui文档icon(注意不是element puls文档)
          type: 'warning', //按钮样式vue2版本见iview文档button，vue3版本见element ui文档button
          onClick: function () {
            let selectRow =   this.$refs.table.getSelected();
            if (selectRow.length == 0) {
              return this.$error('请选择要编辑的行!');
            }
            if (selectRow.length != 1) {
              return this.$error('只能选择一行数据进行编辑!');
            }
            let html = document.getElementById("collect");
            window.open(this.http.ipAddress + 'Print-Designer/print.html?cat=Production_ProductPlan&id=' + selectRow[0].ProductPlan_Id + "&token=" + this.$store.getters.getToken(),'_blank')
          }
        });
        this.buttons.splice(4,0,{  //也可以用push或者splice方法来修改buttons数组
          name: '模版Excel导出', //按钮名称
          icon: 'el-icon-s-unfold', //按钮图标vue2版本见iview文档icon，vue3版本见element ui文档icon(注意不是element puls文档)
          type: 'warning', //按钮样式vue2版本见iview文档button，vue3版本见element ui文档button
          onClick: function () {
            let selectRow =   this.$refs.table.getSelected();
            if (selectRow.length == 0) {
              return this.$error('请选择要编辑的行!');
            }
            if (selectRow.length != 1) {
              return this.$error('只能选择一行数据进行导出!');
            }
            let urlSales = 'api/Production_ProductPlan/exportExcelTemplate?cat=Production_ProductPlan&ProductPlan_Id=' + selectRow[0].ProductPlan_Id;
            this.http.get(urlSales, {}, true).then((content) => {
              if(content=="")
              {
                this.$error('请先维护模版信息!');
              }
              else
              {
                var URL = this.http.webAddress + "/Excel/" + content // URL 为URL地址
                FileSaver.saveAs(URL, content);
              }
            });
          }
        });
    },
    onInited() {
      //框架初始化配置后
      //如果要配置明细表,在此方法操作
      //this.detailOptions.columns.forEach(column=>{ });
      this.detailOptions.buttons.shift();
      this.detailOptions.buttons.unshift({
        name: '选择产品', //按钮名称
        icon: 'el-icon-plus', //按钮图标，参照iview图标
        hidden: false, //是否隐藏按钮(如果想要隐藏按钮，在onInited方法中遍历buttons，设置hidden=true)
        onClick: function () {
          //触发事件
          this.$refs.modelHeader.open();
        }
      });
    },
    searchBefore(param) {
      //界面查询前,可以给param.wheres添加查询参数
      //返回false，则不会执行查询
      return true;
    },
    searchAfter(result) {
      //查询后，result返回的查询数据,可以在显示到表格前处理表格的值
      this.$nextTick(() => {
        this.$refs.gridFooter.rowClick(result[0], "生产计划");
      });
      return true;
    },
    addBefore(formData) {
      //新建保存前formData为对象，包括明细表，可以给给表单设置值，自己输出看formData的值
      return true;
    },
    updateBefore(formData) {
      //编辑保存前formData为对象，包括明细表、删除行的Id
      return true;
    },
    rowClick({ row, column, event }) {
      //查询界面点击行事件
      // this.$refs.table.$refs.table.toggleRowSelection(row); //单击行时选中当前行;
      this.$refs.table.$refs.table.toggleRowSelection(row); //单击行时选中当前行;
      //调用Doc_Order1GridFooter.vue文件中(订单明细)的查询
      this.$refs.gridFooter.rowClick(row,"生产计划");
    },
    modelOpenAfter(row) {
      //点击编辑、新建按钮弹出框后，可以在此处写逻辑，如，从后台获取数据
      //(1)判断是编辑还是新建操作： this.currentAction=='Add';
      //(2)给弹出框设置默认值
      //(3)this.editFormFields.字段='xxx';
      //如果需要给下拉框设置默认值，请遍历this.editFormOptions找到字段配置对应data属性的key值
      //看不懂就把输出看：console.log(this.editFormOptions)
      this.editFormOptions.forEach(item => {
        item.forEach(x => {
          //如果是编辑设置为只读
          if (x.field == "ProductPlanCode") {
            //disabled是editFormOptions的动态属性，这里只能通过this.$set修改值
            //vue3版本改为设置：x.disabled=isEDIT
            x.placeholder = "请输入，忽略将自动生成";
          }
        })
      })
    }
  }
};
export default extension;
