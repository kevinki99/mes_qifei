<template>
    <div>
        <!-- 添加按钮 -->
        <div class="btn-add">
            <a href="###" class="btn btn-info btn-lg" @click="showadd()"><i class="iconfont icon-jiahao ft12 mr5"></i>新建数据表</a>
        </div>
        <!-- 按条件选择 -->
        <div class="tab-filter-type hide">
            <div class="oh mt20">
                <h5 class="pull-left tr">所属应用：</h5>
                <ul class="tab-type ft14">
                    <li  @click="SelectType('')"><span class="active">全部应用</span></li>
                    <li v-for="(el,index) in TypeData" @click="SelectType(el.ID)"><span  v-text="el.ModelName"></span></li>
                </ul>
            </div>
        </div>
        <!-- 表格 -->
        <div class="default-tab ft14 padding20 hover-btn">
            <base-table :pzoption="tableop">
            </base-table>
        </div>

        <div class="modal fade" id="YBPModal" role="dialog">
            <div class="modal-dialog" role="document" style="width: 700px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">添加表</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">数据源</label>
                                <div class="col-xs-7">
                                    <select id="DTS" class="form-control">
                                        <option value="0">本地数据源</option>
                                        <option v-for="item in DSourData" :value="item.ID">{{item.Name}}</option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">所属模块</label>
                                <div class="col-xs-7">
                                    <select id="ssmk" class="form-control">
                                        <option v-for="item in TypeData" :value="item.ID">{{item.ModelName}}</option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">表名称(数据库表名)</label>
                                <div class="col-xs-7">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon1">qj_</span>
                                        <input type="text" class="form-control" id="BName" placeholder="表名称" />

                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">表别名</label>
                                <div class="col-xs-7">
                                    <div class="input-group">
                                        <input type="text" class="form-control" id="BBName" placeholder="表别名" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-info" @click="SaveYB()">确认</button>
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
                YBPList: [],
                TypeData: [],
                DSourData: [],
                curLeiBie: "",
                tableop: {
                    title: "数据表管理",
                    issel: false,
                    iscz: true,
                    dataset: [],
                    czltabledata: [
                        {
                            colname: "设计表", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                window.open("/BiManage/AppPage/DATABI/TABLEEDIT.html?id=" + rowdata.ID, "_blank");
                            },
                        },
                        {
                            colname: "删除", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                this.$parent.Del(rowdata);
                            }
                        }
                    ],
                    collist: [
                        { colid: "ModelName", colname: "类别", isshow: true, istip: true },
                        { colid: "TableName", colname: "表名", isshow: true, istip: true },
                        { colid: "TableDesc", colname: "表别名", isshow: true, istip: true },
                        { colid: "CRUser", colname: "创建人", isshow: true, istip: true },
                        { colid: "CRDate", colname: "创建时间", isshow: true, istip: true },

                    ]
                }
            }
        },
        methods: {
            SelectType: function (type) {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_GETTABLISTDATA', { P1: type}, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        pro.tableop.dataset = resultData.Result;
                    }
                })
            },
            showadd: function () {
                $('#YBPModal').modal('show');
            },
            SaveYB: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_ADDTABDATA', { P1: "qj_" + $("#BName").val(), DSID: $("#DTS").val(), SSMK: $("#ssmk").val(), P2: $("#BBName").val() }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        pro.InitWigetData();
                        top.ComFunJS.winsuccess("添加成功");
                        $('#YBPModal').modal('hide');
                    }
                })
            },
            Del: function (item) {
                var pro = this;
                top.ComFunJS.winconfirm("确认要删除吗", function () {
                    $.getJSON('/api/Bll/ExeAction?Action=DATABI_DELTABLEDATA', { "P1": item.ID }, function (result) {
                        if (result.ErrorMsg == "") {
                            top.ComFunJS.winsuccess("删除成功");
                            pro.InitWigetData();
                        }
                    })
                }, function () { })
            },
            InitWigetData: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_GETTABLISTDATA', {}, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        pro.tableop.dataset = resultData.Result;
                        pro.DSourData = resultData.Result1;
                        pro.TypeData = resultData.Result2;

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