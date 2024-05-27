
<template>
    <div>
        <!-- 添加按钮 -->
        <div class="btn-add">
            <button type="button" class="btn btn-info btn-lg" @click="Add()"><i class="iconfont icon-jiahao ft12 mr5"></i>新建数据源</button>
        </div>
        <!-- 按条件选择 -->
        <div class="tab-filter-type hidden">
            <div class="oh mt20">
                <h5 class="pull-left tr">全部类型：</h5>
                <ul class="tab-type ft14">
                    <li><span class="active">全部</span></li>
                    <!--<li><span>SQLSERVER</span></li>
                    <li><span>MYSQL</span></li>-->
                </ul>
            </div>
        </div>
        <!-- 表格 -->
        <div class="default-tab ft14 padding20 hover-btn">

            <base-table :pzoption="tableop">
            </base-table>



        </div>

        <div class="modal fade" id="DSModal" role="dialog">
            <div class="modal-dialog" role="document" style="width: 600px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">编辑数据源</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">数据源显示名称</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" v-model="SelDSourceItem.Name" placeholder="数据源名称" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 control-label text-right">数据源类型</label>
                                <div class="col-xs-7">
                                    <select id="roleType" class="form-control" v-model="SelDSourceItem.DBType">
                                        <option value="SQLSERVER">SQLSERVER</option>
                                        <option value="MYSQL">MYSQL</option>

                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 control-label text-right">IP</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" v-model="SelDSourceItem.DBIP" placeholder="数据源IP" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 control-label text-right">端口</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" v-model="SelDSourceItem.Port" placeholder="端口" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 control-label text-right">数据库名</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" v-model="SelDSourceItem.DBName" placeholder="数据库名" />
                                </div>
                            </div>


                            <!--<div class="form-group">
                                <label class="col-xs-3 control-label text-right">架构</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" ms-duplex="SelDSourceItem.Schema" placeholder="架构" />
                                </div>
                            </div>-->
                            <div class="form-group">
                                <label class="col-xs-3 control-label text-right">登录名</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" v-model="SelDSourceItem.DBUser" placeholder="登录名" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 control-label text-right">密码</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" v-model="SelDSourceItem.DBPwd" placeholder="密码" />
                                </div>
                            </div>
                            <!--<div class="form-group">
                                <label for="branchDesc" class="col-xs-3 control-label text-right">数据源描述</label>
                                <div class="col-xs-7">
                                    <textarea class="form-control" ms-duplex="SelDSourceItem.Remark" placeholder="请输入数据源描述"></textarea>
                                </div>
                            </div>-->

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-info" @click="TestCon()">连接测试</button>

                        <button type="button" class="btn btn-success" id="conaddForder" @click="ConfirmData()">确&nbsp;&nbsp;认</button>
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
                VIEWS: [],
                SelDSourceItem: {},
                tableop: {
                    title: "数据集",
                    issel: false,
                    iscz: true,
                    dataset: [],
                    czltabledata: [
                        {
                            colname: "编辑", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                this.$parent.edit(rowdata);
                            },
                        },
                        {
                            colname: "删除", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                this.$parent.DelDSource(rowdata);
                            }
                        }
                    ],
                    collist: [
                        { colid: "DBType", colname: "数据源类型", isshow: true, istip: true },
                        { colid: "Name", colname: "名称", isshow: true, istip: true },
                        { colid: "DBIP", colname: "IP", isshow: true, istip: true },
                        { colid: "Port", colname: "端口", isshow: true, istip: true },
                        { colid: "DBName", colname: "数据库名称", isshow: true, istip: true },
                        { colid: "DBUser", colname: "登录名", isshow: true, istip: true },
                        { colid: "CRUser", colname: "创建人", isshow: false, istip: true },
                        { colid: "CRDate", colname: "创建时间", isshow: false, istip: true },

                    ]
                }
            }
        },
        methods: {
            Add: function () {
                var pro = this;
                pro.SelDSourceItem = { Name: "", DBType: "SQLSERVER", DBIP: "", Port: "1433", DBName: "", Schema: "dbo", DBUser: "", DBPwd: "", ID: "0" };
                $('#DSModal').modal('show');
            },
            edit: function (item) {
                var pro = this;
                pro.SelDSourceItem = {};
                pro.SelDSourceItem = item;
                $('#DSModal').modal('show');
            },
            ConfirmData: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_ADDBIDBSOURCE', { P1: JSON.stringify(pro.SelDSourceItem) }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        top.ComFunJS.winsuccess("操作成功");
                        $('#DSModal').modal('hide');
                        pro.InitWigetData();

                    }
                })
            },
            DelDSource: function (item) {
                ComFunJS.winconfirm("删除数据源的同时同时会删除数据集,确认要删除吗", function () {
                    $.getJSON('/api/Bll/ExeAction?Action=DATABI_DELBIDBSOURCE', { "P1": item.ID }, function (result) {
                        if (result.ErrorMsg == "") {
                            ComFunJS.winsuccess("删除成功");
                        }
                    })
                }, function () { })
            },
            AddDSet: function (DataSet) {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_ADDBISETLIST', { P1: pro.SelDSourceItem.ID, P2: DataSet.name, DsetName: DataSet.name }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        top.ComFunJS.winsuccess("操作成功");
                    }
                })
            },
            TestCon: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_TESTBIDBSOURCE', { P1: JSON.stringify(pro.SelDSourceItem) }, function (resultData) {
                    if (resultData.ErrorMsg == "" && resultData.Result == '1') {
                        top.ComFunJS.winsuccess("连接成功");
                    }
                })
            },
            InitWigetData: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_GETBIDBSOURCELIST', {}, function (resultData) {
                    if (resultData.Result.length > 0) {
                        pro.tableop.dataset = resultData.Result;

                    }
                })

            },
        },
        mounted: function () {
            var pro = this;
            pro.$nextTick(function () {
                pro.InitWigetData();
            })

        },
        watch: {
            "SelDSourceItem.DBType": { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV == "SQLSERVER") {
                        this.SelDSourceItem.Port = "1433";
                    }
                    if (newV == "MYSQL") {
                        this.SelDSourceItem.Port = "3306";
                    }
                },
                deep: true
            }
        }
    };
</script>