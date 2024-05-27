
<template>
    <div>
        <!-- 添加按钮 -->
        <div class="btn-add">
            <el-button v-for="czcol in plbtns" @click="plmangcol(czcol)" size="medium" :type="czcol.bttype">{{czcol.czname}}</el-button>
            <!--<button v-for="czcol in CZData" class="btn btn-info ft12 mr5"  @click="plmangcol(czcol)">{{czcol.czname}}</button>

            <button type="button" class="btn btn-info ft12 mr5" v-if="syscz[0].isshow" @click="Add()">新增</button>
            <button type="button" class="btn btn-danger" @click="DelMItemData()" v-if="syscz[1].isshow"><i class="iconfont icon-shanchu ft12 mr5"></i>批量删除</button>-->
        </div>
        <!-- 按条件选择 -->
        <div class="tab-filter-type pt20">
            <div class="oh " v-for="item in mrcxdata" v-show="item.lbdata.length>1">
                <h5 class="pull-left tr">{{item.cxname}}：</h5>
                <ul class="tab-type ft14">
                    <li v-for="lb in item.lbdata"><span v-bind:class="{ 'active': lb.issel }" @click="sellb(lb,item)" v-text="lb.itemtext"></span></li>

                </ul>
            </div>

        </div>
        <!-- 表格 -->
        <div class="default-tab ft14 padding20 hover-btn" style="padding-top: 10px;">

            <base-table :pzoption="tableop" @selection-change="handleSelectionChange">
            </base-table>
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

                plbtns: [],
                MXCZData: [],
                syscz: [{ czname: "新增", isshow: false }, { czname: "批量删除", isshow: false }, { czname: "编辑", isshow: false }, { czname: "删除", isshow: false }],
                ActionData: [],
                mrcxdata: [],
                tableop: {
                    title: "自定义应用",
                    tablename: "",
                    issel: true,
                    iscz: true,
                    isadd: true,
                    isedit: true,
                    ispldel: true,
                    isdel: true,
                    dataset: [],
                    pdid: "0",
                    multipleSelection: [],
                    czltabledata: [

                    ],
                    collist: [

                    ]
                }
            }
        },
        methods: {
            Add: function () {
                var pdid = this.tableop.pdid;
                ComFunJS.winviewform("/BiManage/AppPage/FORMBI/FormAdd.html?pdid=" + pdid + "&vtype=2", "添加表单", "1000", "", function () {
                    model.refpage();
                });
            },
            ishasqx: function () {

            },
            plmangcol: function (coldata) {

                if (coldata.czname == "新增") {
                    var pdid = this.tableop.pdid;
                    ComFunJS.winviewform("/BiManage/AppPage/FORMBI/FormAdd.html?pdid=" + pdid + "&vtype=2", "添加表单", "1000", "", function () {
                        model.refpage();
                    });

                } else if (coldata.czname == "批量删除") {
                    this.DelMItemData();
                } else {
                    try {
                        var ids = [];
                        for (var i = 0; i < this.tableop.multipleSelection.length; i++) {
                            ids.push(this.tableop.multipleSelection[i].ID);
                        }
                        let jscode = coldata.jscode;
                        let func = new Function('ids', jscode);
                        func(ids)
                    } catch (e) {
                        this.$notify({
                            title: '成功',
                            message: '解析JS代码有误',
                            type: 'error'
                        });
                    }
                }


            },
            handleSelectionChange(val) {
                this.tableop.multipleSelection = val;
            },
            DelItemData: function (item) {
                var pdid = this.tableop.pdid;
                this.DelData(item.intProcessStanceid)
            },
            DelMItemData: function () {
                var ids = [];
                for (var i = 0; i < this.tableop.multipleSelection.length; i++) {
                    ids.push(this.tableop.multipleSelection[i].intProcessStanceid);
                }
                if (ids.length == 0) {
                    this.$notify.error({
                        title: '错误',
                        message: '至少需要选择一条数据才能操作'
                    });
                    return;
                }
                this.DelData(ids.join());
            },
            DelData: function (ids) {
                var pro = this;
                this.$confirm('此操作将永久删除该数据, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    $.getJSON('/api/Bll/ExeAction?Action=FORMBI_DELWFDATA', { "P2": ids }, function (result) {
                        if (result.ErrorMsg == "") {
                            ComFunJS.winsuccess("删除成功");
                            pro.InitWigetData();
                        }
                    })
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            sellb: function (lb, lbs) {
                _.forEach(lbs.lbdata, function (obj) {
                    obj.issel = false;
                })
                lb.issel = true;
                this.Query();
            },
            Query: function () {
                var pro = this;
                var tablepar = JSON.parse(pro.$root.pagedata.ExtData);
                var querydata = [];
                _.forEach(pro.mrcxdata, function (obj) {
                    var va = _.find(obj.lbdata, function (cx) {
                        return cx.issel == true;
                    })
                    querydata.push({ colid: obj.cxzd, cal: obj.cal, val: va.itemtext });
                })

                $.getJSON('/api/Bll/ExeAction?Action=DATABI_GETVUELISTDATA', { P1: tablepar.value.join(), P2: tablepar.dataqx ? "Y" : "N", WD: JSON.stringify(tablepar.WDData), qdata: JSON.stringify(querydata) }, function (resultData) {
                    if (resultData.ErrorMsg == "") {
                        pro.tableop.dataset = resultData.Result2;
                        pro.tableop.pdid = resultData.Result3;
                    }
                })

            },
            InitWigetData: function () {
                var pro = this;
                var tablepar = JSON.parse(pro.$root.pagedata.ExtData);
                var allcz = tablepar.ActionData;// 操作列表
                var sqdata = pro.$root.pagedata.ActionData.split(',');//授权列表

                pro.tableop.title = tablepar.name;
                if (pro.tableop.collist.length == 0) {
                    pro.tableop.collist = [];
                    for (var i = 0; i < tablepar.WDData.length; i++) {
                        if (tablepar.WDData[i].colname) {
                            pro.tableop.collist.push({ colid: tablepar.WDData[i].colid, colname: tablepar.WDData[i].colname, isshow: tablepar.WDData[i].isshow, istip: true });
                        }
                    }
                }
                pro.plbtns = _.filter(allcz, function (item) { return item.isrowcz == '0' && _.findIndex(sqdata, function (ac) { return ac == item.czname }) > -1 });
                var rczbtns = _.filter(allcz, function (item) { return item.isrowcz == '1' });
                pro.tableop.czltabledata = [];
                _.forEach(rczbtns, function (mxcz) {
                    if (_.findIndex(sqdata, function (ac) { return ac == mxcz.czname }) > -1) {
                        if (mxcz.czname == "编辑") {
                            pro.tableop.czltabledata.push({
                                colname: "编辑", bttype: "text", isshow: true, mangefun: function (rowdata) {
                                    ComFunJS.winviewform("/BiManage/AppPage/FORMBI/FormManage.html?piid=" + rowdata.intProcessStanceid + "&vtype=2", "管理表单", "1000", "", function () {
                                        model.refpage();
                                    });
                                },
                            });
                        } else if (mxcz.czname == "删除") {
                            pro.tableop.czltabledata.push({
                                colname: "删除", bttype: "text", isshow: true, mangefun: function (rowdata) {
                                    this.$parent.DelItemData(rowdata);
                                }
                            });
                        } else {
                            pro.tableop.czltabledata.push({
                                colname: mxcz.czname, bttype: mxcz.bttype, isshow: true, mangefun: function (rowdata) {
                                    if (mxcz.jscode) {
                                        let jscode = mxcz.jscode;
                                        let func = new Function('rowdata', jscode);
                                        func(rowdata)
                                    }
                                },
                            });
                        }

                    }
                });

                _.forEach(tablepar.mrcxdata, function (obj) {
                    var temp = [];
                    _.forEach(obj.cxdata.split(','), function (cx) {
                        temp.push({ issel: false, itemtext: cx })
                    })
                    if (temp.length > 0) {
                        temp[0].issel = true;
                    }
                    obj.lbdata = temp;
                })
                pro.mrcxdata = tablepar.mrcxdata;//分类操作
                pro.Query();


            },
        },
        mounted: function () {
            var pro = this;
            pro.$nextTick(function () {
                pro.InitWigetData();
            })

        }
    };
</script>