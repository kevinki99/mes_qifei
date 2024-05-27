<style>
    .el-drawer__header {
        display: none
    }
</style>
<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <i class="iconfont icon-shuaxin pull-right widgetdel hidden-print" title="刷新图表" @click.stop="refchart(pzoption.wigdetcode)"></i>
        <i @click.stop="dialogDataVisible = !dialogDataVisible" class="iconfont icon-fabu pull-right  hidden-print" style="float:right;margin-right:10px"></i>
        <div v-bind:style="{ height: pzoption.wigheight + 'px' }" v-show="!dialogDataVisible">
            <ve-chart :data="childpro.chartData" :id="'qjchat'+pzoption.wigdetcode" :toolbox="childpro.toolbox" :series="childpro.series" :extend="childpro.chartExtend" :settings="childpro.chartSettings" :legend="childpro.legend" :title="childpro.title" style="margin-top:20px;width:100%" :ref="pzoption.wigdetcode+'vechart'" height="100%"></ve-chart>
        </div>
        <pl-table class="qjTable" :height="pzoption.wigheight" v-show="dialogDataVisible" :data="pzoption.dataset" stripe border fit use-virtual big-data-checkbox>
            <pl-table-column type="index" width="60"> </pl-table-column>

            <pl-table-column v-for="col in pzoption.wdlist" :prop="col.colid" :label="col.colname" :key="col.colid" align="center" :colid="col.colid" sortable>
                <template slot-scope="scope">
                    <span>{{scope.row[col.colid]}}</span>
                </template>
            </pl-table-column>
            <pl-table-column v-for="col in pzoption.dllist" :prop="col.colid" :label="col.colname" :key="col.colid" align="center" :colid="col.colid" sortable>
                <template slot-scope="scope">
                    <span>{{scope.row[col.colid]}}</span>
                </template>
            </pl-table-column>

        </pl-table>
        <!--<el-dialog title="组件属性" :visible.sync="dialogInputVisible" style="opacity:.9">

        </el-dialog>-->
        <el-drawer :visible.sync="dialogInputVisible" direction="rtl" style="opacity:.9;" size="40%">
            <el-form :model="childpro" style="padding:20px">
                <el-form-item label="图表类型">
                    <el-radio-group v-model="childpro.chartSettings.type" size="mini" style="width:100%">
                        <el-radio-button label="pie">饼图</el-radio-button>
                        <el-radio-button label="ring">环状图</el-radio-button>
                        <el-radio-button label="line">线图</el-radio-button>
                        <el-radio-button label="histogram">柱状图</el-radio-button>
                        <el-radio-button label="bar">横柱图</el-radio-button>
                        <el-radio-button label="funnel">漏斗图</el-radio-button>
                        <el-radio-button label="radar">雷达图</el-radio-button>
                    </el-radio-group>
                </el-form-item>
                <el-tabs>
                    <el-tab-pane label="基本属性" style="min-height: 360px;">
                        <el-form-item label="图表标题">
                            <el-input v-model="childpro.title.text" autocomplete="off"></el-input>
                        </el-form-item>
                        <el-form-item label="标题布局">
                            <el-radio-group v-model="childpro.title.left" size="mini" style="width:100%">
                                <el-radio-button label="left">居左</el-radio-button>
                                <el-radio-button label="center">居中</el-radio-button>
                                <el-radio-button label="right">居右</el-radio-button>
                            </el-radio-group>
                        </el-form-item>
                        <el-form-item label="标题Top距离">
                            <el-input-number v-model="childpro.title.top" controls-position="right" :min="1" :max="100"></el-input-number>
                        </el-form-item>
                        <el-form-item label="南丁格尔图" v-if="childpro.chartSettings.type=='pie'||childpro.chartSettings.type=='ring'">
                            <el-radio-group v-model="childpro.chartSettings.roseType" size="mini" style="width:100%">
                                <el-radio-button label="">正常饼图</el-radio-button>
                                <el-radio-button label="radius">南丁格尔图</el-radio-button>
                            </el-radio-group>
                        </el-form-item>
                        <el-form-item label="漏斗图顺序" v-if="childpro.chartSettings.type=='funnel'">
                            <el-switch v-model="childpro.chartSettings.ascending" size="mini" style="width:100%">
                            </el-switch>
                        </el-form-item>



                    </el-tab-pane>
                </el-tabs>
                <el-tab-pane label="更多属性">
                </el-tab-pane>
            </el-form>
        </el-drawer>
    </el-col>
</template>
<script>
    module.exports = {
        props: {
            index: Number,
            pzoption: Object
        },
        data() {
            return {
                dialogInputVisible: false,
                dialogDataVisible: false,
                childpro: {
                    placeholder: "统计图表",
                    charttabtype: "0",
                    chartSettings: {
                        type: "pie",
                        roseType: '',
                        ascending: false,
                        label: {
                            formatter: '{b}:{c}({d}%)'
                        }
                    },
                    chartExtend: {
                        series: {
                            radius: '50%',
                            center: ['50%', '50%']
                        }
                    },
                    legend: {
                        bottom: 10,
                        left: 'center',
                        show: true
                    },
                    title: {
                        text: "饼图",
                        left: 'center',
                        top: 10,
                        textStyle: {
                            fontSize: 16
                        }
                    },
                    toolbox: {
                        feature: {
                            //dataZoom: {
                            //    yAxisIndex: 'none'
                            //},
                            //dataView: { readOnly: true },
                            //magicType: { type: ['line', 'bar'] },
                            //restore: {},
                            //saveAsImage: {}
                        }
                    },
                    chartData: {

                    }
                }
            };
        },
        methods: {
            delWid: function (wigdetcode) {
                this.$root.nowwidget = { rules: { required: false, message: '请填写信息', trigger: 'blur' } };//没这个删除不掉啊
                _.remove(this.$root.FormData.wigetitems, function (obj) {
                    return obj.wigdetcode == wigdetcode;
                });
            },
            senddata: function () {
                this.pzoption.childpro = JSON.parse(JSON.stringify(this.childpro));

            },
            refchart: function (wigdetcode) {
                this.$refs[wigdetcode + 'vechart'].echarts.resize()
            }

        },
        mounted: function () {
            var chi = this;
            chi.$nextTick(function () {
                if (chi.$root.addchildwig) {
                    chi.$root.addchildwig();//不能缺少
                }
                if (chi.pzoption.childpro.placeholder) {
                    //检测新增属性
                    _.forIn(chi.childpro, function (value, key) {
                        if (!_.has(chi.pzoption.childpro, key)) {
                            chi.pzoption.childpro[key] = value;
                        }

                    });
                    chi.childpro = chi.pzoption.childpro;
                } else {
                    //初始化
                    chi.pzoption.childpro = chi.childpro;
                    chi.senddata();
                    if (chi.pzoption.datatype == '2') {
                        chi.pzoption.staticdata = JSON.stringify([
                            { '月份': '一月', '用户量': 1393, '应用数量': 1093, '点击量': 2093 },
                            { '月份': '二月', '用户量': 3530, '应用数量': 3230, '点击量': 1230 },
                            { '月份': '三月', '用户量': 2923, '应用数量': 2623, '点击量': 1623 },
                            { '月份': '四月', '用户量': 1723, '应用数量': 1423, '点击量': 2423 }
                        ])
                    }
                    chi.childpro.chartSettings.type = chi.pzoption.charttype;
                    chi.$root.UpdateYBData(chi.pzoption);

                }


            })
        },
        watch: {
            'pzoption.dataset': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    var chi = this;
                    chi.$refs[chi.pzoption.wigdetcode + 'vechart'].echarts.clear();//清理画布
                    var chi = chi;
                    if (chi.pzoption.dataset.length > 0) {
                        chi.$refs[chi.pzoption.wigdetcode + 'vechart'].echarts.hideLoading();
                        var keys = [];
                        var tempdata = [];
                        _.forEach(chi.pzoption.dataset, function (obj) {
                            var dataobj = {};
                            _.forEach(chi.pzoption.wdlist, function (wd) {
                                dataobj[wd.colname] = obj[wd.colid];
                            })
                            _.forEach(chi.pzoption.dllist, function (dl) {
                                dataobj[dl.colname] = obj[dl.colid];
                            })
                            tempdata.push(dataobj);
                        })
                      
                        for (var i in tempdata[0]) {
                            keys.push(i);
                        }
                        chi.childpro.chartData = {
                            columns: keys,
                            rows: tempdata
                        }
                    } else {
                        chi.childpro.chartData.rows = [];
                        chi.$refs[chi.pzoption.wigdetcode + 'vechart'].echarts.showLoading({
                            text: '暂无数据',
                            color: '#ffffff',
                            textColor: '#8a8e91',
                            maskColor: 'rgba(255, 255, 255, 0.8)',
                        })

                    }


                },
                deep: true
            },
            childpro: { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    this.refchart(this.pzoption.wigdetcode);
                    this.senddata();

                },
                deep: true
            },
            'childpro.chartSettings.type': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV == "ring") {
                        this.childpro.chartExtend.series.radius = ['40%', '50%'];
                    } else {
                        this.childpro.chartExtend.series.radius = '50%';

                    }
                    if (newV == "funnel") {
                        this.childpro.chartSettings.ascending = false;
                    }
                    if (newV == "line" || newV == "histogram" || newV == "bar") {
                        this.childpro.chartExtend.series.markPoint = {
                            data: [
                                { type: 'max', name: '最大值' },
                                //{ type: 'min', name: '最小值' }
                            ]
                        };
                    }

                },
                deep: true
            }
        }

    };
</script>

