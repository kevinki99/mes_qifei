<template>
    <div>
        <!-- 添加按钮 -->
        <div class="btn-add">
            <a href="###" class="btn btn-info btn-lg" @click="showadd()"><i class="iconfont icon-jiahao ft12 mr5"></i>新建报表</a>
        </div>
        <!-- 按条件选择 -->
        <div class="tab-filter-type hidden">
            <div class="oh mt20">
                <h5 class="pull-left tr">全部类型：</h5>
                <ul class="tab-type ft14">
                    <li v-for="(el,index) in TypeData" @click="SelectType(el)"><span v-bind:class="{active:index==0}" v-text="el"></span></li>
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
                        <h4 class="modal-title">添加报告</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">报表名称</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" id="YBName" placeholder="报表名称" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="branchName" class="col-xs-3 control-label text-right">报表类别</label>
                                <div class="col-xs-7">
                                    <input type="text" class="form-control" id="YBType" placeholder="报表类别" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-info" @click="SaveYB()">确认</button>
                        <button type="button" class="btn btn-success" id="conaddForder" @click="SaveAndUp()">确认并编辑</button>
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
                YBPList: [],//笔记类别
                TypeData: [],
                Alldata: [],
                curLeiBie: "",
                tableop: {
                    title: "仪表盘",
                    issel: false,
                    iscz: true,
                    dataset: [],
                    czltabledata: [
                        {
                            colname: "编辑", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                window.open("/BiManage/AppPage/DATABI/YBPBuild.html?id=" + rowdata.ID, "_blank");
                            },
                        },
                        {
                            colname: "预览", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                window.open("/BiManage/AppPage/DATABI/YBPVIEW.html?ID=" + rowdata.ID, "_blank");
                            },
                        },
                        {
                            colname: "删除", bttype: "text", isshow: true,  mangefun: function (rowdata) {
                                this.$parent.Del(rowdata);
                            }
                        }
                    ],
                    collist: [
                        { colid: "YBType", colname: "类别", isshow: true, istip: true },
                        { colid: "Name", colname: "名称", isshow: true, istip: true },
                        { colid: "CRUser", colname: "创建人", isshow: true, istip: true },
                        { colid: "CRDate", colname: "创建时间", isshow: true, istip: true },

                    ]
                }
            }
        },
        methods: {
            SelectType: function (type) {
                var pro = this;
                pro.curLeiBie = type;
                pro.YBPList = [];
                for (var i = 0; i < pro.Alldata.length; i++) {
                    if (pro.Alldata[i].YBType == type) {
                        pro.YBPList.push(pro.Alldata[i]);
                    }

                }
                pro.tableop.dataset = pro.YBPList;
            },
            showadd: function () {
                $('#YBPModal').modal('show');
            },
            SaveYB: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_SAVEDATA', { P1: $("#YBName").val(), P2: $("#YBType").val() }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        pro.YBPList.push(resultData.Result);
                        $('#YBPModal').modal('hide');
                    }
                })
            },
            SaveAndUp: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_SAVEDATA', { P1: $("#YBName").val(), P2: $("#YBType").val() }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        pro.YBPList.push(resultData.Result);
                        window.open("YBPBuild.html?id=" + resultData.Result.ID);
                    }
                })
            },
            Del: function (item) {
                var pro = this;
                top.ComFunJS.winconfirm("确认要删除吗", function () {
                    $.getJSON('/api/Bll/ExeAction?Action=DATABI_DELYBDATA', { "P1": item.ID }, function (result) {
                        if (result.ErrorMsg == "") {
                            top.ComFunJS.winsuccess("删除成功");
                            pro.InitWigetData();
                        }
                    })
                }, function () { })
            },
            InitWigetData: function () {
                var pro = this;
                $.getJSON('/api/Bll/ExeAction?Action=DATABI_GETYBLISTDATA', {}, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        for (var i = 0; i < resultData.Result.length; i++) {
                            if (pro.TypeData.join().indexOf(resultData.Result[i].YBType) == -1) {
                                pro.TypeData.push(resultData.Result[i].YBType)
                            }
                        }
                        pro.Alldata = resultData.Result;
                        pro.tableop.dataset = resultData.Result;
                        pro.SelectType(pro.Alldata[0].YBType)
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