/*****************************************************************************************
**  Author:COCO 2022
*****************************************************************************************/
//此js文件是用来自定义扩展业务代码，可以扩展一些自定义页面或者重新配置生成的代码
import PlanModelBody from './equip_extend/Equip_SpotMaintWorkOrderModelBody'
let extension = {
  components: {
    //查询界面扩展组件
    gridHeader: '',
    gridBody: '',
    gridFooter: '',
    //新建、编辑弹出框扩展组件
    modelHeader: '',
    modelBody: PlanModelBody,
    modelFooter: ''
  },
  tableAction: '', //指定某张表的权限(这里填写表名,默认不用填写)
  buttons: { view: [], box: [], detail: [] }, //扩展的按钮
  methods: {
    getFormOption(field) {
      let option;
      this.editFormOptions.forEach(x => {
        x.forEach(item => {
          if (item.field == field) {
            option = item;
          }
        })
      })
      return option;
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
        this.columnIndex = true;
        this.boxOptions.height = 550;
        this.editFormOptions.forEach(x => {
          x.forEach(item => {
            if (item.field == 'SpotMaintPlanName') {
              //给编辑表单设置[选择数据]操作,extra具体配置见mesform组件api
              item.extra = {
                icon: "el-icon-zoom-out",
                text: "高级选择",
                style: "color:blue;font-size: 14px;cursor: pointer;",
                click: item => {
                  this.$refs.modelBody.openPaper();
                }
              }
            }
          })
        });
    },
    onInited() {
      //框架初始化配置后
      //如果要配置明细表,在此方法操作
      //this.detailOptions.columns.forEach(column=>{ });
    },
    searchBefore(param) {
      //界面查询前,可以给param.wheres添加查询参数
      //返回false，则不会执行查询
      return true;
    },
    searchAfter(result) {
      //查询后，result返回的查询数据,可以在显示到表格前处理表格的值
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
    },
    setSelectRows(_rows){
      this.editFormFields.SpotMaintPlanId = _rows[0].SpotMaintPlanId;
      this.editFormFields.SpotMaintPlanName = _rows[0].SpotMaintPlanName;
      this.editFormFields.SpotMaintPlanCode = _rows[0].SpotMaintPlanCode;
      this.editFormFields.PlanItemType = _rows[0].PlanItemType.toString();
    },
    modelOpenAfter(row) {
      //点击编辑、新建按钮弹出框后，可以在此处写逻辑，如，从后台获取数据
      //(1)判断是编辑还是新建操作： this.currentAction=='Add';
      //(2)给弹出框设置默认值
      //(3)this.editFormFields.字段='xxx';
      //如果需要给下拉框设置默认值，请遍历this.editFormOptions找到字段配置对应data属性的key值
      //看不懂就把输出看：console.log(this.editFormOptions)
      var spotMaintPlanId = this.getFormOption('SpotMaintPlanId');
      spotMaintPlanId.hidden = true;
      this.editFormOptions.forEach(item => {
        item.forEach(x => {
          //如果是编辑设置为只读
          if (x.field == "SpotMaintWorkOrderCode") {
            //disabled是editFormOptions的动态属性，这里只能通过this.$set修改值
            //vue3版本改为设置：x.disabled=isEDIT
            x.placeholder = "请输入，忽略将自动生成";
          }
        })
      })
      let status = this.getFormOption("Status");
      if (this.currentAction == 'Add') {
        this.editFormFields.Status = "1"
        status.readonly = true;
      }
      else
      {
        status.readonly = false;
      }
    }
  }
};
export default extension;
