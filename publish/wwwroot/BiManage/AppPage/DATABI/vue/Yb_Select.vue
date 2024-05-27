<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <el-form-item :label="pzoption.title">
            <el-select v-model="pzoption.value" style="width:100%" v-if="!childpro.multiple&&pzoption.wdlist.length>0"  size="small" @change="selchange" filterable clearable>
                <el-option v-for="item in pzoption.dataset" v-if="pzoption.wdlist.length==1"
                           :key="item[pzoption.wdlist[0].colid]"
                           :label="item[pzoption.wdlist[0].colid]"
                           :value="item[pzoption.wdlist[0].colid]+''">
                </el-option>

                <el-option v-for="item in pzoption.dataset" v-if="pzoption.wdlist.length>1"
                           :key="item[pzoption.wdlist[1].colid]"
                           :label="item[pzoption.wdlist[0].colid]"
                           :value="item[pzoption.wdlist[1].colid]+''">
                </el-option>
            </el-select>
            <el-select v-model="childpro.mvalue" style="width:100%" v-if="childpro.multiple" @change="selchange"  size="small" multiple filterable clearable>
                <el-option v-for="item in pzoption.dataset" v-if="pzoption.wdlist.length==1"
                           :key="item[pzoption.wdlist[0].colid]"
                           :label="item[pzoption.wdlist[0].colid]"
                           :value="item[pzoption.wdlist[0].colid]">
                </el-option>

                <el-option v-for="item in pzoption.dataset" v-if="pzoption.wdlist.length>1"
                           :key="item[pzoption.wdlist[1].colid]"
                           :label="item[pzoption.wdlist[0].colid]"
                           :value="item[pzoption.wdlist[1].colid]">
                </el-option>
            </el-select>
        </el-form-item>
        <el-dialog title="组件属性" :visible.sync="dialogInputVisible" width="40%">
            <el-form-item label="是否多选">
                <el-switch v-model="childpro.multiple" size="mini" style="width:100%">
                </el-switch>
            </el-form-item>
            <el-form-item label="默认值">
                <el-input v-model="childpro.defval" autocomplete="off"></el-input>
            </el-form-item>
            <el-form-item label="下拉框更改时执行代码">
                <el-input v-model="childpro.jscode" :autosize="{ minRows: 6, maxRows: 9}" autocomplete="off" type="textarea"></el-input>
            </el-form-item>

        </el-dialog>
    </el-col>

</template>
<script>
    module.exports = {
        props: ['pzoption', 'index'],
        data: function () {
            return {
                dialogInputVisible: false,
                childpro: {
                    placeholder: "请选择",
                    itemtype: "text",
                    mvalue: [],
                    datatype: "0",
                    datasql: "",
                    defval: "",
                    jscode: "",
                    multiple: false
                }
            }
        },
        methods: {
            delWid: function (wigdetcode) {
                // 子组件中触发父组件方法ee并传值cc12345
                this.$root.nowwidget = {};
                _.remove(this.$root.FormData.wigetitems, function (obj) {
                    return obj.wigdetcode == wigdetcode;
                });
            },
            senddata: function () {
                this.pzoption.childpro = JSON.parse(JSON.stringify(this.childpro));
            },
            selchange: function () {
                try {
                    var data = JSON.parse(JSON.stringify(this.pzoption));
                    let jscode = this.childpro.jscode;
                    let func = new Function('data', jscode);
                    func(data)
                } catch (e) {
                    app.$notify({
                        title: '成功',
                        message: '解析JS代码有误',
                        type: 'success'
                    });
                }

            }
        },
        mounted: function () {
            var chi = this;
            chi.$nextTick(function () {
                if (chi.$root.addchildwig) {
                    chi.$root.addchildwig();//不能缺少
                }
                if (chi.pzoption.childpro.itemtype) {
                    //检测新增属性
                    _.forIn(chi.childpro, function (value, key) {
                        if (!_.has(chi.pzoption.childpro, key)) {
                            chi.pzoption.childpro[key] = value;
                        }

                    });
                    chi.childpro = chi.pzoption.childpro;
                    chi.$root.UpdateYBData(chi.pzoption, function (ybdata) {
                        chi.pzoption.value = app.jxparam(chi.childpro.defval);
                    });
                } else {
                    if (chi.pzoption.datatype == '2') {
                        chi.pzoption.staticdata = JSON.stringify([
                            { 'lable': '一月', 'val': 1393 },
                            { 'lable': '二月', 'val': 3530 },
                            { 'lable': '三月', 'val': 2923 },
                            { 'lable': '四月', 'val': 1723 }
                        ])
                        chi.$root.jxfiled(chi.pzoption)
                        chi.$root.addwd("lable", chi.pzoption)
                        chi.$root.addwd("val", chi.pzoption)

                    }
                    chi.pzoption.mdwidth = 6;
                    chi.pzoption.childpro = chi.childpro;

                 
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
            'childpro.mvalue': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV) {
                        this.pzoption.value = newV.join();
                    }
                },
                deep: true
            }
        }
    };
</script>