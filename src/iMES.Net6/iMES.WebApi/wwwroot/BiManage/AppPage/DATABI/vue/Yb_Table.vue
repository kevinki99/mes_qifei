
<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" style="line-height:40px;    margin-right: 10px;" @click.stop="dialogFormVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" style="line-height:40px" @click.stop="delWid(pzoption.wigdetcode)"></i>

        <div class="tab-filter-type"  v-show="childpro.isexport">
            <div class="input-group col-xs-12 col-md-6 col-sm-6" style="margin-bottom:10PX;">
                <div class="input-group-btn">
                    <button type="button" style="min-width:120PX;height: 34px;" class="btn btn-info  dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">{{sear.colname}} <span class="caret"></span></button>
                    <ul class="dropdown-menu">
                        <li v-for="fi in pzoption.wdlist" @click="sel(fi)"><a href="#" v-text="fi.colname"></a></li>
                        <li v-for="fi in pzoption.dllist" @click="sel(fi)"><a href="#" v-text="fi.colname"></a></li>
                    </ul>
                </div>
                <el-select placeholder="请选择筛选类型" size="mini" v-model="searitem.cal" class="form-control filter"  style="height: 34px;width:30%;padding:0px">
                    <el-option label="包含" value="0"></el-option>
                    <el-option label="小于" value="1"></el-option>
                    <el-option label="大于" value="2"></el-option>
                    <el-option label="不等于" value="3"></el-option>
                    <el-option label="等于" value="4"></el-option>
                    <el-option label="在列表中(逗号隔开)" value="5"></el-option>

                </el-select>
                <input type="text" class="form-control" style="height: 34px;width:65%;border-left-width: 0px;"  v-model="searitem.qvalue" placeholder="输入关键字搜索数据">
                <span class="input-group-btn" v-show="searitem.qvalue">
                    <button class="btn btn-warning" style="height: 34px;" @click="reset" type="button">重置</button>
                </span>
            </div>
        </div>
        <div style="width:100%;height:40px; padding-left:10px; line-height:40px; border: 1px solid #EBEEF5;border-bottom: 0px;">
            {{pzoption.title}},共计找到{{datalength}}条数据
            <a class="tabletool" @click.stop="dialogFiledVisible = true" v-show="childpro.isexport">字段筛选</a>
            <!--<a class="tabletool"  v-show="childpro.isexport">数据过滤</a>-->
            <a class="tabletool hidden-xs" @click="exportxls()" v-show="childpro.isexport">导出</a>

            <!--<el-tooltip content="导出Excel" placement="top">
        <i class="el-icon-share" v-if="childpro.isexport" style="float:right;margin-right:10px;line-height:40px;cursor:pointer" @click="exportxls()"></i>
    </el-tooltip>
    <el-tooltip content="搜索" placement="top">
        <i class="el-icon-search" @click.stop="dialogTableVisible = true" style="float:right;margin-right:10px;line-height:40px;cursor:pointer"></i>
    </el-tooltip>-->
            <!--<el-button type="primary" size="mini" v-if="childpro.isexport" icon="el-icon-edit" style="float:right;margin-top: 5px;margin-right:10px" @click="exportxls()"></el-button>-->
        </div>
        <pl-table class="qjTable" :row-style="{height:'20px'}" :cell-style="{padding:'0px'}" style="font-size: 10px" v-loading="childpro.loading" element-loading-text="拼命加载中" element-loading-spinner="el-icon-loading" element-loading-background="rgba(0, 0, 0, 0.8)" :data="pzoption.dataset" @selection-change="handleSelectionChange" stripe border fit use-virtual big-data-checkbox @header-dragend="widchange" :row-class-name="tableRowClassName" :summary-method="getSummaries" :show-summary="pzoption.ishj" :height="pzoption.wigheight">

            <pl-table-column type="selection" width="45" v-if="childpro.issel">
            </pl-table-column>
            <pl-table-column v-for="col in pzoption.wdlist" :prop="col.colid" :label="col.colname" :key="col.colid" v-if="col.ishide" :width="col.width" min-width="120" align="center" :colid="col.colid" :show-overflow-tooltip="col.istip" sortable>
                <template slot-scope="scope">
                    <span>{{mang(scope.row[col.colid],col)}}</span>
                </template>
            </pl-table-column>
            <pl-table-column v-for="col in pzoption.dllist" :prop="col.colid" :label="col.colname" :key="col.colid" v-if="col.ishide" :width="col.width" min-width="120" align="center" :colid="col.colid" :show-overflow-tooltip="col.istip" sortable>
                <template slot-scope="scope">
                    <span>{{mang(scope.row[col.colid],col)}}</span>
                </template>
            </pl-table-column>

            <pl-table-column type="index" width="60" fixed="left"> </pl-table-column>
            <pl-table-column label="操作列" min-width="120" align="center" v-if="childpro.iscz" fixed="left" :width="childpro.czwidth">
                <template slot-scope="scope">
                    <el-button v-for="(czcol,index) in childpro.czltabledata" :key="index" ms-if="czcol.colname" @click="mangcol(scope.row,czcol)" :type="czcol.bttype" size="mini">{{czcol.colname}}</el-button>
                </template>
            </pl-table-column>
        </pl-table>
        <el-pagination background @current-change="handleCurrentChange" :page-size="childpro.pagesize" :current-page="childpro.nowpage" layout="prev, pager, next" :total="pzoption.datatotal" v-if="pzoption.datatype=='0'&&childpro.ispagination" style="text-align:center;margin-top:8px"></el-pagination>
        <el-drawer :visible.sync="dialogFormVisible" direction="rtl" style="opacity:.9;" size="40%">
            <div class="padding20">
                <el-form :model="childpro" style="padding:20px" label-position="top" label-width="80px">

                    <el-form-item label="高级表格(添加筛选,导出,字段选择功能)">
                        <el-switch v-model="childpro.isexport"></el-switch>
                    </el-form-item>
                    <el-form-item label="是否分页">
                        <el-switch v-model="childpro.ispagination"></el-switch>
                    </el-form-item>

                    <el-form-item label="是否添加check列">
                        <el-switch v-model="childpro.issel"></el-switch>
                    </el-form-item>
                    <el-form-item label="每页显示数量" v-if="childpro.ispagination">
                        <el-input-number v-model="childpro.pagesize"></el-input-number>
                    </el-form-item>
                    <el-form-item label="是否添加操作列">
                        <el-switch v-model="childpro.iscz"></el-switch>
                    </el-form-item>

                </el-form>
                <el-table :data="childpro.czltabledata" style="width:100%;min-height:300px;margin-top:10px" class="data-table" v-if="childpro.iscz" stripe border>


                    <el-table-column label="操作列">

                        <el-table-column fixed="right"
                                         label="样式"
                                         width="80">
                            <template slot-scope="scope">
                                <el-button @click="delczl(scope.$index, childpro.czltabledata)" type="danger" size="mini">删除</el-button>

                            </template>
                        </el-table-column>
                        <el-table-column fixed="right"
                                         label="样式"
                                         width="120">
                            <template slot-scope="scope">
                                <el-select v-model="scope.row.bttype" placeholder="请选择">
                                    <el-option value="primary"></el-option>
                                    <el-option value="success"></el-option>
                                    <el-option value="info"></el-option>
                                    <el-option value="danger"></el-option>
                                    <el-option value="text"></el-option>
                                </el-select>
                            </template>
                        </el-table-column>
                        <el-table-column fixed="right"
                                         label="操作名称"
                                         width="120">
                            <template slot-scope="scope">
                                <el-input v-model="scope.row.colname" placeholder="请输入名称"></el-input>
                            </template>
                        </el-table-column>
                        <el-table-column fixed="right" label="操作Code(JS)">
                            <template slot-scope="scope">
                                <el-input v-model="scope.row.jscode" placeholder="请输入点击操作按钮后执行得JS代码" type="textarea" :autosize="{ minRows: 2, maxRows: 4}"></el-input>
                            </template>
                        </el-table-column>
                    </el-table-column>

                </el-table>
                <el-button v-if="childpro.iscz" @click="addczl()" type="success" size="mini">添加操作列</el-button>

            </div>
        </el-drawer>

        <el-dialog title="表格筛选" :visible.sync="dialogTableVisible" class="qfiter" center>
            <el-table :data="childpro.gridData" stripe border fit>
                <el-table-column label="筛选字段" min-width="120" align="center">
                    <template slot-scope="scope">
                        <el-select v-model="scope.row.qfiled" placeholder="请选择" size="mini" clearable>
                            <el-option v-for="item in pzoption.wdlist"
                                       :key="item.colid"
                                       :label="item.colname"
                                       :value="item.colid">
                            </el-option>
                            <el-option v-for="item in pzoption.dllist"
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

            <el-checkbox v-for="(filed,index) in pzoption.wdlist" :key="index"  v-model="filed.ishide">{{filed.colname}}</el-checkbox>
        </el-dialog>

    </el-col>

</template>
<style>
    .qfiter .el-table td, .qfiter .el-table th {
        padding: 8px 0;
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

    .filter .el-input__inner {
        height: 32px;
        line-height: 32px;
        border: 0px;
    }
</style>
<script>
    module.exports = {
        props: ['pzoption', 'index'],
        data() {
            return {
                datalength: "0",
                dialogFormVisible: false,
                dialogTableVisible: false,
                dialogFiledVisible: false,
                loading: true,
                sear: {},
                nowcell: { rowindex: "0", colindex: "0", rowspan: "1", colspan: "1" },
                searitem: {
                    qfiled: "",
                    cal: "0",
                    qvalue: ""
                },
                childpro: {
                    gridData: [],
                    multipleSelection: "",
                    isexport: true,
                    czltabledata: [{ colname: "操作一", jscode: "", bttype: "primary" }],//操作列数据
                    ispagination: false,
                    iscz: false,
                    issel: false,
                    loading: false,
                    nowpage: 1,
                    pagesize: 0,
                    locadata: [],
                    czwidth: "120"

                }
            };
        },
        methods: {
            sel: function (col) {
                var pro = this;
                pro.sear = col;
                pro.searitem.qfiled = col.colid;
               // pro.seardata();

            },
            seardata: function () {
                var pro = this;
                var q = pro.searitem;

                if (q.qvalue) {
                    var tabledata = JSON.parse(sessionStorage.getItem("tabledata"));
                    const dv = new DataSet.View().source(tabledata);
                    var redata = [];
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
                                            return row[q.qfiled] < calval;
                                        }
                                    });
                                }
                                break;
                            case "2":    //大于
                                {
                                    dv.transform({
                                        type: 'filter',
                                        callback(row) { // 判断某一行是否保留，默认返回true
                                            return row[q.qfiled] > calval;
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
                    redata = dv.rows;
                    pro.pzoption.dataset = redata;
                } else {
                    pro.pzoption.dataset = JSON.parse(sessionStorage.getItem("tabledata"));
                }
            },
            reset: function () {
                this.searitem.qvalue = "";
               // this.seardata();
            },
            senddata: function () {
                this.pzoption.childpro = JSON.parse(JSON.stringify(this.childpro));
            },
            handleSelectionChange(val) {
                this.childpro.multipleSelection = val;
            },

            addczl: function () {
                this.childpro.czltabledata.push({ colname: "操作" + this.childpro.czltabledata.length, jscode: "", bttype: "primary" });
            },
            delczl: function (index, rows) {
                rows.splice(index, 1);
            },
            initpagedata: function (datalen) {
                this.childpro.pagecount = datalen;
            },
            handleCurrentChange: function (val) {
                var chi = this;
                chi.childpro.nowpage = val;
                chi.pzoption.childpro.nowpage = val;
                chi.$root.UpdateYBData(chi.pzoption);
            },
            tableRowClassName({ row, rowIndex }) {
                //把每一行的索引放进row
                row.index = rowIndex;
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
                                                    return row[q.qfiled] < calval;
                                                }
                                            });
                                        }
                                        break;
                                    case "2":    //大于
                                        {
                                            dv.transform({
                                                type: 'filter',
                                                callback(row) { // 判断某一行是否保留，默认返回true
                                                    return row[q.qfiled] > calval;
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
                    let jscode = coldata.jscode;
                    let func = new Function('rowdata', jscode);
                    func(rowdata)
                } catch (e) {
                    app.$notify({
                        title: '成功',
                        message: '解析JS代码有误',
                        type: 'success'
                    });
                }
            },
            mang: function (val, col) {
                var ysval = val;
                _.forEach(col.mapdata, function (obj) {
                    if (val == obj.val) {
                        ysval = obj.ysval;
                    }
                })
                return ysval;
            },
            delWid: function (wigdetcode) {
                this.$root.nowwidget = { rules: { required: false, message: '请填写信息', trigger: 'blur' } };
                _.remove(this.$root.FormData.wigetitems, function (obj) {
                    return obj.wigdetcode == wigdetcode;
                });
            },
            exportxls: function () {
                var title = [];
                _.forEach(this.pzoption.wdlist, function (obj) {
                    title.push({ "value": obj.colname, "type": "ROW_HEADER_HEADER", "datatype": "string", "colid": obj.colid })
                })
                _.forEach(this.pzoption.dllist, function (obj) {
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
            },
            objectSpanMethod: function ({ row, column, rowIndex, columnIndex }) {
                if (column.hasOwnProperty("label")) {
                    try {
                        var temparr = [];
                        temparr = _.concat(temparr, this.pzoption.wdlist, this.pzoption.dllist);
                        var temp = _.find(temparr, function (obj) {
                            return obj.colname == column.label
                        }).hbdata;
                        var hbs = temp.split(",");
                        for (var i = 0; i < hbs.length; i++) {
                            var hb = hbs[i].split('-');
                            if (columnIndex == hb[0]) {
                                if (rowIndex == hb[1]) {
                                    return {
                                        rowspan: hb[2],
                                        colspan: 1
                                    };
                                }
                                var tempar = [];
                                for (var m = 1; m < hb[2] * 1; m++) {
                                    tempar.push(hb[1] * 1 + m);
                                }
                                if (tempar.indexOf(rowIndex) > -1) {
                                    return {
                                        rowspan: 0,
                                        colspan: 0
                                    };
                                }
                            }
                        }

                    } catch (e) {

                    }

                }


            },
            getSummaries: function (param) {

                const { columns, data } = param;
                const sums = [];

                columns.forEach((column, index) => {
                    if (index === 0) {
                        sums[index] = '合计';
                        return;
                    }

                    var temparr = [];
                    temparr = _.concat(temparr, this.pzoption.wdlist, this.pzoption.dllist);

                    var col = _.find(temparr, function (obj) {
                        return obj.colname == column.label && obj.ishj == true;
                    })

                    if (col) {
                        var temphj = 0;
                        _.forEach(data, function (obj) {
                            if (obj[col.colid]) {
                                temphj = temphj * 1 + obj[col.colid] * 1;
                            }
                        })
                        sums[index] = temphj;
                    } else {
                        sums[index] = "";
                    }

                });

                return sums;
            },
            widchange: function (newWidth, oldWidth, column, event) {
                var pro = this;
                _.forEach(this.$root.FormData.wigetitems, function (wiget) {
                    var temp = {};
                    if (wiget.wigdetcode == pro.pzoption.wigdetcode) {
                        _.forEach(wiget.wdlist, function (col) {
                            if (column.label == col.colname) {
                                col.width = newWidth;
                            }
                        })
                        _.forEach(wiget.dllist, function (col) {
                            if (column.label == col.colname) {
                                col.width = newWidth;
                            }
                        })
                        if (column.label == "操作列") {
                            pro.childpro.czwidth = newWidth;
                        }
                    }
                })

            }
        },
        watch: {
            childpro: { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    //  this.senddata();
                },
                deep: true
            },
            'pzoption.dataset': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV) {
                        this.datalength = newV.length;
                        if (this.childpro.gridData.length == 0 && !this.searitem.qvalue) {
                            sessionStorage.setItem("tabledata", JSON.stringify(newV))
                        }
                    }
                },
                deep: true
            },
            'childpro.ispagination': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV) {
                        this.childpro.pagesize = 20;
                    } else {
                        this.childpro.pagesize = 0;
                    }
                },
                deep: true
            },
            'pzoption.wdlist': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV) {
                        localStorage.setItem(this.pzoption.wigdetcode + "fileddata", JSON.stringify(this.pzoption.wdlist));
                    }
                },
                deep: true
            },
            'searitem': {
                handler(newV, oldV) {
                    if (newV) {
                       // debugger;
                        var s = newV;
                        this.seardata();

                    }
                },
                deep: true
            }

        },
        mounted: function () {
            var chi = this;
            chi.$nextTick(function () {
                if (chi.$root.addchildwig) {
                    chi.$root.addchildwig();//不能缺少
                }
                if (chi.pzoption.childpro.nowpage) {
                    //检测新增属性
                    _.forIn(chi.childpro, function (value, key) {
                        if (!_.has(chi.pzoption.childpro, key)) {
                            chi.pzoption.childpro[key] = value;
                        }

                    });
                    chi.childpro = chi.pzoption.childpro;
                    if (localStorage.getItem(chi.pzoption.wigdetcode + "fileddata")) {
                        chi.pzoption.wdlist = JSON.parse(localStorage.getItem(chi.pzoption.wigdetcode + "fileddata"))
                    }
                } else {
                    chi.pzoption.childpro = chi.childpro;
                    if (chi.pzoption.datatype == '2') {
                        chi.pzoption.staticdata = JSON.stringify([
                            { '月份': '一月', '用户量': 1393, '应用数量': 1093, '点击量': 2093 },
                            { '月份': '二月', '用户量': 3530, '应用数量': 3230, '点击量': 1230 },
                            { '月份': '三月', '用户量': 2923, '应用数量': 2623, '点击量': 1623 },
                            { '月份': '四月', '用户量': 1723, '应用数量': 1423, '点击量': 2423 }
                        ])
                        chi.$root.jxfiled(chi.pzoption)
                        chi.$root.addwd("月份", chi.pzoption);
                        chi.$root.addwd("用户量", chi.pzoption);
                        chi.$root.addwd("应用数量", chi.pzoption);
                        chi.$root.addwd("点击量", chi.pzoption);
                    }
                    chi.pzoption.wigheight = 438;
                    chi.$root.UpdateYBData(chi.pzoption);
                }
                if (chi.pzoption.wdlist.length > 0) {
                    chi.sear = chi.pzoption.wdlist[0];
                    chi.searitem.qfiled = chi.sear.colid;

                }

            })

        }
    };
</script>