
<template>
    <el-col :sm="24">
        <div class="tab-filter-type">
            <div class="input-group" style="width:520PX;margin-bottom:10PX;">
                <div class="input-group-btn">
                    <button type="button" style="min-width:120PX;height: 34px;" class="btn btn-info  dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">{{sear.colname}} <span class="caret"></span></button>
                    <ul class="dropdown-menu">
                        <li v-for="fi in pzoption.collist" @click="sel(fi)"><a href="#" v-text="fi.colname"></a></li>
                    </ul>
                </div>
                <input type="text" class="form-control" style="height: 34px;" v-model="seastr" @input="seardata" placeholder="输入关键字搜索数据">
                <span class="input-group-btn" v-show="seastr">
                    <button class="btn btn-default" style="height: 34px;" @click="reset" type="button">重置</button>
                </span>
            </div>
        </div>
        <div style="width:100%;height:40px; padding-left:10px; line-height:40px; border: 1px solid #EBEEF5;border-bottom: 0px;">
              {{pzoption.title}},共计找到{{datalength}}条数据
            <a class="tabletool" @click.stop="dialogFiledVisible = true">字段筛选</a>
            <a class="tabletool" style="display:none" @click.stop="dialogTableVisible = true">数据过滤</a>
            <a class="tabletool" @click="exportxls()">导出</a>
        </div>
        <pl-table class="qjTable" :height="height" :row-style="{height:'20px'}" :cell-style="{padding:'0px'}" style="font-size: 10px" element-loading-text="拼命加载中" element-loading-spinner="el-icon-loading" element-loading-background="rgba(0, 0, 0, 0.8)" :data="pzoption.dataset" @selection-change="handleSelectionChange" stripe border fit use-virtual big-data-checkbox :row-class-name="tableRowClassName">

            <pl-table-column type="selection" width="45" v-if="pzoption.issel">
            </pl-table-column>
            
            <pl-table-column v-for="col in pzoption.collist" :prop="col.colid" :label="col.colname" :key="col.colid" v-if="col.isshow" :width="col.width" min-width="120" align="center" :colid="col.colid" :show-overflow-tooltip="col.istip" sortable>
                <template slot-scope="scope">
                    <span>{{scope.row[col.colid],col}}</span>
                </template>
            </pl-table-column>


            <pl-table-column type="index" width="60" fixed="left" v-if="pzoption.isxh"> </pl-table-column>
            <pl-table-column label="操作列" min-width="120" align="center" v-if="iscz&&pzoption.collist.length>6" fixed="left">
                <template slot-scope="scope">
                    <el-button v-for="czcol in pzoption.czltabledata" v-if="czcol.isshow" @click="mangcol(scope.row,czcol)" :type="czcol.bttype" size="mini">{{czcol.colname}}</el-button>
                </template>
            </pl-table-column>
            <pl-table-column label="操作列" min-width="120" align="center" v-if="iscz&&pzoption.collist.length<7">
                <template slot-scope="scope">
                    <el-button v-for="czcol in pzoption.czltabledata" v-if="czcol.isshow" @click="mangcol(scope.row,czcol)" :type="czcol.bttype" size="mini">{{czcol.colname}}</el-button>
                </template>
            </pl-table-column>
        </pl-table>

        <el-dialog title="表格筛选" :visible.sync="dialogTableVisible" class="qfiter" center>
            <el-table :data="childpro.gridData" stripe border fit>
                <el-table-column label="筛选字段" min-width="120" align="center">
                    <template slot-scope="scope">
                        <el-select v-model="scope.row.qfiled" placeholder="请选择" size="mini" clearable>
                            <el-option v-for="item in pzoption.collist"
                                       :key="item.colid"
                                       :label="item.colname"
                                       :value="item.colid">
                            </el-option>

                        </el-select>
                    </template>
                </el-table-column>
                <el-table-column label="筛选类型" min-width="120" align="center">
                    <template slot-scope="scope">
                        <el-select v-model="scope.row.cal" placeholder="请选择" size="mini" clearable>
                            <el-option label="包含" value="0"></el-option>
                            <el-option label="小于" value="1"></el-option>
                            <el-option label="大于" value="2"></el-option>
                            <el-option label="不等于" value="3"></el-option>
                            <el-option label="等于" value="4"></el-option>
                            <el-option label="在列表中(逗号隔开)" value="5"></el-option>

                        </el-select>
                    </template>
                </el-table-column>
                <el-table-column label="筛选值" min-width="120" align="center">
                    <template slot-scope="scope">
                        <el-input v-model="scope.row.qvalue" size="mini"></el-input>

                    </template>
                </el-table-column>
                <el-table-column label="操作" fixed="right" align="center" width="100">
                    <template slot-scope="scope">
                        <el-button @click.native.prevent="delRow(scope.$index,childpro.gridData)"
                                   type="text"
                                   size="small">
                            删除
                        </el-button>
                    </template>
                </el-table-column>


            </el-table>
            <el-button type="primary" size="mini" class="mt10 pull-right" @click="addRow">添加过滤<i class="el-icon-plus"></i></el-button>

            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogTableVisible = false">取 消</el-button>
                <el-button type="primary" @click="confiter">确 定</el-button>
            </span>
        </el-dialog>
        <el-dialog title="字段筛选" :visible.sync="dialogFiledVisible" center>
            <el-checkbox v-for="filed in pzoption.collist" v-model="filed.isshow">{{filed.colname}}</el-checkbox>
        </el-dialog>

    </el-col>

</template>
<style>
    .qfiter .el-table td, .qfiter .el-table th {
        padding: 8px 0;
    }

    .el-table__fixed-right {
        height: 100% !important;
    }

    .el-dialog__footer {
        margin-top: 30PX;
    }

    .tableRowClassName {
    }

    .qjTable .el-table td, .qjTable .el-table th {
        HEIGHT: 44PX !IMPORTANT;
        FONT-SIZE: 13PX;
        padding: 6px 0;
    }

    .tabletool {
        float: right;
        margin-right: 10px;
        color: dodgerblue;
        font-size: 13PX;
    }
</style>
<script>
    module.exports = {
        props: ['pzoption'],
        components: {
            'base-loading': httpVueLoader('/BiManage/Base/Vue/Loading.vue')
        },
        data() {
            return {
                datalength:"0",
                sear: {},
                seastr: "",
                dialogFormVisible: false,
                dialogTableVisible: false,
                dialogFiledVisible: false,
                childpro: {
                    gridData: [],
                    multipleSelection: "",
                    locadata: [],
                    height: 480,
                    isxh: false//序号列
                }
            };
        },
        computed: {
            // 计算属性的 getter
            iscz: function () {
                var chi = this;
                return _.findIndex(chi.pzoption.czltabledata, function (col) {
                    return col.isshow
                }) > -1
            },
            height: function () {
                var chi = this;
                return chi.pzoption.height || chi.childpro.height;
            },
            isxh: function () {
                var chi = this;
                return chi.pzoption.isxh && chi.childpro.isxh;
            }
        },
        methods: {
            sel: function (col) {
                var pro = this;
                pro.sear = col;
                pro.seardata();

            },
            seardata: function () {
                var pro = this;
                if (pro.seastr) {
                    var tabledata = JSON.parse(sessionStorage.getItem("tabledata"));
                    const dv = new DataSet.View().source(tabledata);
                    var redata = [];
                    var qfiled = pro.sear.colid;
                    dv.transform({
                        type: 'filter',
                        callback(row) { // 判断某一行是否保留，默认返回true
                            return (row[qfiled] + "").indexOf(pro.seastr) > -1;
                        }
                    });
                    redata = dv.rows;
                    pro.pzoption.dataset = redata;
                } else {
                    pro.pzoption.dataset = JSON.parse(sessionStorage.getItem("tabledata"));
                }
            },
            reset: function () {
                this.seastr = "";
                this.seardata();
            },
            handleSelectionChange(val) {
                this.$emit('selection-change', val)
            },
            addRow: function () {
                var chi = this;
                if (typeof (chi.childpro.gridData) == "undefined") {
                    chi.childpro.gridData = [];
                }
                chi.childpro.gridData.push({
                    qfiled: '',
                    qftype: '',
                    cal: '0',
                    qvalue: ''
                });
            },
            delRow: function (index, rows) {
                rows.splice(index, 1);

            },
            tableRowClassName({ row, rowIndex }) {
                //把每一行的索引放进row
                row.index = rowIndex;
            },

            confiter: function () {
                var chi = this;
                if (_.findIndex(chi.childpro.gridData, function (o) { return !o.qvalue; }) > -1) {
                    this.$notify.error({
                        title: '错误',
                        message: '过滤值不能为空'
                    });
                } else {
                    chi.dialogTableVisible = false;
                    if (chi.childpro.gridData.length > 0) {
                        var tabledata = JSON.parse(sessionStorage.getItem("tabledata"));
                        const dv = new DataSet.View().source(tabledata);
                        var redata = [];
                        _.forEach(chi.childpro.gridData, function (q) {
                            var calval = q.qvalue;
                            if (calval) {
                                switch (q.cal) {
                                    case "0":    //包含
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    return row[q.qfiled].indexOf(calval) > -1;
                                                }
                                            });
                                        }
                                        break;
                                    case "1":    //小于
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    return row[q.qfiled]*1 < calval*1;
                                                }
                                            });
                                        }
                                        break;
                                    case "2":    //大于
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    return row[q.qfiled]*1 > calval*1;
                                                }
                                            });
                                        }
                                        break;
                                    case "3":    //不等于
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    return row[q.qfiled] != calval;
                                                }
                                            });
                                        }
                                        break;
                                    case "4":    //等于
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    return row[q.qfiled] == calval;
                                                }
                                            });
                                        }
                                        break;
                                    case "5":    //在列表中
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    var bl = false;
                                                    for (var i = 0; i < calval.split(",").length; i++) {
                                                        if (row[q.qfiled].indexOf(calval.split(",")[i]) > -1) {
                                                            bl = true;
                                                            break;
                                                        }
                                                    }
                                                    return bl;
                                                }
                                            });
                                        }
                                        break;
                                    default: {

                                    }
                                }
                            }


                        })
                        redata = dv.rows;
                        chi.pzoption.dataset = redata;
                    } else {
                        chi.pzoption.dataset = JSON.parse(sessionStorage.getItem("tabledata"));

                    }
                }
            },
            confiled: function myfunction() {
                var chi = this;
                chi.dialogFiledVisible = false;

            },
            mangcol: function (rowdata, coldata) {
                try {
                    if (coldata.mangefun) {
                        return coldata.mangefun.call(this, rowdata);
                    }
                    if (coldata.jscode) {
                        let jscode = coldata.jscode;
                        let func = new Function('rowdata', jscode);
                        func(rowdata)
                    }
                } catch (e) {

                }
            },
            exportxls: function () {
                var title = [];
                _.forEach(this.pzoption.collist, function (obj) {
                    title.push({ "value": obj.colname, "type": "ROW_HEADER_HEADER", "datatype": "string", "colid": obj.colid })
                })

                this.JSONToExcelConvertor(this.pzoption.dataset, this.pzoption.title + ComFunJS.getnowdate('yyyy-mm-dd'), title);
            },
            JSONToExcelConvertor: function (JSONData, FileName, ShowLabel) {

                var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

                var excel = '<table>';
                //设置表头
                var row = "<tr>";
                for (var i = 0, l = ShowLabel.length; i < l; i++) {
                    //解决身份证问题
                    row += '<td style="' + "mso-number-format:'\@';\"" + ">" + ShowLabel[i].value + '</td>';
                }

                //换行
                excel += row + "</tr>";

                //设置数据
                for (var i = 0; i < arrData.length; i++) {
                    var row = "<tr>";

                    for (var j = 0; j < ShowLabel.length; j++) {
                        //解决身份证问题
                        var value = arrData[i][ShowLabel[j].colid];
                        row += '<td style="' + "mso-number-format:'\@';\"" + ">\t" + value + "</td>";
                    }

                    excel += row + "</tr>";
                }

                excel += "</table>";

                var excelFile = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>";
                excelFile += '<meta http-equiv="content-type" content="application/vnd.ms-excel; charset=UTF-8">';
                excelFile += '<meta http-equiv="content-type" content="application/vnd.ms-excel';
                excelFile += '; charset=UTF-8">';
                excelFile += "<head>";
                excelFile += "<!--[if gte mso 9]>";
                excelFile += "<xml>";
                excelFile += "<x:ExcelWorkbook>";
                excelFile += "<x:ExcelWorksheets>";
                excelFile += "<x:ExcelWorksheet>";
                excelFile += "<x:Name>";
                excelFile += "{worksheet}";
                excelFile += "</x:Name>";
                excelFile += "<x:WorksheetOptions>";
                excelFile += "<x:DisplayGridlines/>";
                excelFile += "</x:WorksheetOptions>";
                excelFile += "</x:ExcelWorksheet>";
                excelFile += "</x:ExcelWorksheets>";
                excelFile += "</x:ExcelWorkbook>";
                excelFile += "</xml>";
                excelFile += "<![endif]-->";
                excelFile += "</head>";
                excelFile += "<body>";
                excelFile += excel;
                excelFile += "</body>";
                excelFile += "</html>";


                var uri = 'data:application/vnd.ms-excel;charset=utf-8,' + encodeURIComponent(excelFile);

                var link = document.createElement("a");
                link.href = uri;

                link.style = "visibility:hidden";
                link.download = FileName + ".xls";

                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        },
        watch: {
            'pzoption.collist': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    var chi = this;
                    if (newV) {

                        localStorage.setItem(chi.$root.PageCode + chi.$root.pagedata.ExtData + "fileddata", JSON.stringify(chi.pzoption.collist));
                        chi.sear = chi.pzoption.collist[0];

                    }
                },
                deep: true
            },
            'pzoption.dataset': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV) {
                        this.datalength = newV.length;
                        if (this.childpro.gridData.length == 0 && !this.seastr) {
                            sessionStorage.setItem("tabledata", JSON.stringify(newV));
                        }
                    }
                },
                deep: true
            },
        },
        mounted: function () {
            var chi = this;
            chi.$nextTick(function () {
                if (localStorage.getItem(chi.$root.PageCode + chi.$root.pagedata.ExtData + "fileddata")) {
                    chi.pzoption.collist = JSON.parse(localStorage.getItem(chi.$root.PageCode + chi.$root.pagedata.ExtData + "fileddata"));
                }
                if (chi.pzoption.collist.length > 0) {
                    chi.sear = chi.pzoption.collist[0];
                }
            })

        }
    };
</script>