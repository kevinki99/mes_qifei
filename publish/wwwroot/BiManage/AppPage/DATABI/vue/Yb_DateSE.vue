
<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <el-form-item :label="pzoption.title">
            <el-input v-model="pzoption.value" style="display:none">
            </el-input>
            <el-date-picker v-model="childpro.chivalue" v-if="childpro.datetype=='d'"
                            align="right"
                            size="small"
                            type="daterange"
                            range-separator="-"
                            :value-format="childpro.dateformat"
                            format="yy 年 MM 月 dd 日"
                            start-placeholder="开始日期"
                            end-placeholder="结束日期"
                            :picker-options="childpro.pickerOptionsd"
                            unlink-panels >
            </el-date-picker>

            <el-date-picker v-model="childpro.chivalue" v-if="childpro.datetype=='m'"
                            type="monthrange"
                            align="right"
                            size="small"
                            unlink-panels
                            :value-format="childpro.dateformat"
                            range-separator="-"
                            start-placeholder="开始月份"
                            end-placeholder="结束月份"
                            :picker-options="childpro.pickerOptionsd1">
            </el-date-picker>
        </el-form-item>
        <el-dialog title="组件属性" :visible.sync="dialogInputVisible">
            <el-form :model="childpro">
                <el-form-item label="默认当前时间">
                    <el-switch v-model="childpro.ishasdefault"></el-switch>
                </el-form-item>
                <el-form-item label="起止类型">
                    <el-radio-group v-model="childpro.datetype" size="mini" style="width:100%">
                        <el-radio-button label="d">日</el-radio-button>
                        <el-radio-button label="m">月</el-radio-button>
                    </el-radio-group>
                </el-form-item>
            </el-form>
        </el-dialog>
    </el-col>
</template>
<script>
    module.exports = {
        data() {
            return {
                dialogInputVisible: false,
                childpro: {
                    placeholder: "占位符",
                    dateformat: "yyyy-MM-dd",
                    ishasdefault: true,
                    disabled: false,
                    chivalue: [],
                    datetype: "d",
                    pickerOptionsd: {
                        shortcuts: [{
                            text: '最近一周',
                            onClick(picker) {
                                const end = new Date();
                                const start = new Date();
                                start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                                picker.$emit('pick', [start, end]);
                            }
                        }, {
                            text: '最近一个月',
                            onClick(picker) {
                                const end = new Date();
                                const start = new Date();
                                start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                                picker.$emit('pick', [start, end]);
                            }
                        }, {
                            text: '最近三个月',
                            onClick(picker) {
                                debugger;
                                alert(1)
                                const end = new Date();
                                const start = new Date();
                                start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                                picker.$emit('pick', [start, end]);
                            }
                        }]
                    },
                    pickerOptionsd1: {
                        shortcuts: [{
                            text: '本月',
                            onClick(picker) {
                                picker.$emit('pick', [new Date(), new Date()]);
                            }
                        }, {
                            text: '今年至今',
                            onClick(picker) {
                                const end = new Date();
                                const start = new Date(new Date().getFullYear(), 0);
                                picker.$emit('pick', [start, end]);
                            }
                        }, {
                            text: '最近六个月',
                            onClick(picker) {
                                debugger;
                                alert(1)
                                const end = new Date();
                                const start = new Date();
                                start.setMonth(start.getMonth() - 6);
                                picker.$emit('pick', [start, end]);
                            }
                        }]
                    }

                }
            };
        },
        props: ['pzoption', 'index'],
        methods: {
            delWid: function (wigdetcode) {
                this.$root.nowwidget = {};
                _.remove(this.$root.FormData.wigetitems, function (obj) {
                    return obj.wigdetcode == wigdetcode;
                });
            },
            senddata: function () {
                this.pzoption.childpro = JSON.parse(JSON.stringify(this.childpro));
            }
        },
        mounted: function () {
            var chi = this;
            chi.$nextTick(function () {
                chi.childpro.disabled = app.isview;
                if (chi.$root.addchildwig) {
                    chi.$root.addchildwig();//不能缺少
                }
                if (chi.pzoption.childpro.dateformat) {
                    chi.childpro = chi.pzoption.childpro;

                } else {
                    chi.senddata();

                }
                if (chi.childpro.ishasdefault && app.isview) {
                    var nowdate = ComFunJS.getnowdate('yyyy-mm') + "-01";
                    chi.childpro.chivalue = [nowdate, nowdate];
                }
            })

        },
        watch: {
            childpro: { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    this.senddata();
                },
                deep: true
            },
            "childpro.chivalue": {
                handler(newV, oldV) {

                    if (newV && newV.length > 0) {
                        var sdate = newV[0];
                        var temp = this.childpro.datetype;
                        var edate = ComFunJS.format(ComFunJS.DateAdd(ComFunJS.StringToDate(newV[1]), temp, 1), 'yyyy-MM-dd');
                        this.pzoption.value = sdate + "," + edate;
                    }
                    if (newV == null) {
                        //清空日期时不能为null
                        this.childpro.chivalue = "";
                    }
                },
                deep: true
            }
        }
    };
</script>
