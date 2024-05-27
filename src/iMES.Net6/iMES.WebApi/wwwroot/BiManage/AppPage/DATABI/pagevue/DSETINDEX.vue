<template>
    <div>
        <!-- 添加按钮 -->
        <div class="btn-add">
            <button type="button" class="btn btn-info btn-lg" @click="Add()"><i class="iconfont icon-jiahao ft12 mr5"></i>新建数据集</button>
        </div>
        <!-- 按条件选择 -->
        <div class="tab-filter-type hidden">
            <div class="oh mt20">
                <h5 class="pull-left tr">全部类型：</h5>
                <ul class="tab-type ft14">
                    <li><span class="active">我的数据集</span></li>
                </ul>
            </div>
        </div>
        <!-- 表格 -->
        <div class="default-tab ft14 padding20 hover-btn">
            <base-table :pzoption="tableop">
            </base-table>
            <div class="modal fade" id="DModal" role="dialog">
                <div class="modal-dialog" role="document" style="width: 700px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">添加数据集</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <label for="branchName" class="col-xs-3 control-label text-right">数据集名称</label>
                                    <div class="col-xs-7">
                                        <input type="text" class="form-control" id="DSName" placeholder="数据集名称" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="branchName" class="col-xs-3 control-label text-right">数据源</label>
                                    <div class="col-xs-7">
                                        <select id="DTS" class="form-control">
                                            <option value="0">本地数据源</option>
                                            <option v-for="item in DSourData" :value="item.ID">{{item.Name}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                            <button type="button" class="btn btn-info" @click="ConfirmData()">确认</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    module.exports = {
        props: ['pdata'],
        components: {
            'base-table': httpVueLoader('/BiManage/Base/Vue/Table.vue')
        },
        data: function () {
            return {
                DSourData: [],
                DSData: {},
                isedit: 0,
                tableop: {
                    title: "数据集",
                    issel: false,
                    iscz: true,
                    dataset: [],
                    czltabledata: [
                        {
                            colname: "编辑", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                this.$parent.Edit(rowdata);
                            },
                        },
                        {
                            colname: "设计", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                window.open("/BiManage/AppPage/DATABI/DSETEDIT.html?ID=" + rowdata.ID, "_blank");
                            },
                        },
                        {
                            colname: "删除", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                this.$parent.Del(rowdata);
                            }
                        }
                    ],
                    collist: [
                        { colid: "SJY", colname: "数据源", isshow: true, istip: true },
                        { colid: "Name", colname: "数据集", isshow: true, istip: true },
                        { colid: "CRUser", colname: "创建人", isshow: true, istip: true },
                        { colid: "CRDate", colname: "创建时间", isshow: true, istip: true },

                    ]
                }
            }
        },
        methods: {
            Del: function (item) {
                var pro = this;
                top.ComFunJS.winconfirm("确认要删除吗", function () {
                    $.getJSON('/api/Bll/ExeAction?Action=DATABI_DELBIDBSET', { "P1": item.ID }, function (result) {
                        if (result.ErrorMsg == "") {
                            debugger;
                            top.ComFunJS.winsuccess("删除成功");
                            pro.InitWigetData();

                        }
                    })
                }, function () { })
            },
            Add: function () {
                this.isedit = 0;
                $('#DModal').modal('show');
            },
            Edit: function (item) {
                var pro = this;
                pro.isedit = 1;
                pro.DSData = item;
                $("#DTS").val(item.SID)
                $("#DSName").val(item.SName)
                $('#DModal').modal('show');
            },
            ConfirmData: function () {
                var pro = this;
                if (pro.isedit == 0) {
                    pro.DSData = { Name: $("#DSName").val(), SName: $("#DSName").val(), Type: $("#DType").val(), ID: "0", SID: $("#DTS").val() };
                } else {
                    pro.DSData.SID = $("#DTS").val();
                    pro.DSData.SName = $("#DSName").val();
                    pro.DSData.Name = $("#DSName").val();

                }
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_ADDBIDBSET', { P1: JSON.stringify(pro.DSData) }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        top.ComFunJS.winsuccess("操作成功");
                        pro.InitWigetData();
                        $('#DModal').modal('hide');

                    }
                })
            },
            InitWigetData: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_GETBIDBSETLIST', {}, function (resultData) {
                    if (resultData.Result.length > 0) {
                        pro.tableop.dataset = resultData.Result;
                        pro.DSourData = resultData.Result2;

                    }
                })
            }
        },
        mounted: function () {
            var pro = this;
            pro.$nextTick(function () {
                pro.InitWigetData();
            })

        },
        watch: {
            childpro: { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                },
                deep: true
            }
        }
    };
</script>

