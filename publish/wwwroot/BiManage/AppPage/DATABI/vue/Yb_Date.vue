
<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <el-form-item :label="pzoption.title">
            <el-input v-model="pzoption.value" style="display:none">
            </el-input>
            <el-date-picker align="right" size="small"  :type="childpro.itemtype" :placeholder="childpro.placeholder" :value-format="childpro.dateformat" v-model="childpro.chivalue">
            </el-date-picker>
        </el-form-item>
        <el-dialog title="组件属性" :visible.sync="dialogInputVisible">
            <el-form :model="childpro">
                <el-form-item label="默认当前时间">
                    <el-switch v-model="childpro.ishasdefault"></el-switch>
                </el-form-item>
                <el-form-item label="日期类型">
                    <el-radio v-model="childpro.itemtype" label="year">年</el-radio>
                    <el-radio v-model="childpro.itemtype" label="month">月</el-radio>
                    <el-radio v-model="childpro.itemtype" label="date">日</el-radio>
                    <el-radio v-model="childpro.itemtype" label="datetime">时间</el-radio>
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
                    placeholder: "",
                    itemtype: "date",
                    chivalue: "",
                    dateformat: "yyyy-MM-dd",
                    ishasdefault: false

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
                    //浏览模式且有默认值的时候初始化
                    chi.childpro.chivalue = ComFunJS.getnowdate(chi.childpro.dateformat)

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
                    if (newV) {
                        this.pzoption.value = newV;
                    }
                    if (newV == null) {
                        //清空日期时不能为null
                        this.pzoption.value = "";
                        this.childpro.chivalue = "";
                    }
                },
                deep: true
            },
            "childpro.itemtype": { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    switch (newV) {
                        case "year":
                            this.childpro.dateformat = "yyyy";
                            break;
                        case "month":
                            this.childpro.dateformat = "yyyy-MM";
                            break;
                        case "date":
                            this.childpro.dateformat = "yyyy-MM-dd";
                            break;
                        case "datetime":
                            this.childpro.dateformat = "yyyy-MM-dd HH:mm";
                            break;
                        default:
                            this.childpro.dateformat = "yyyy-MM-dd";
                    }
                },
                deep: true
            }



        }
    };
</script>