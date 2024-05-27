<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <el-form-item :label="pzoption.title" :prop="'wigetitems.' + index + '.value'">
            <el-checkbox-group v-model="childpro.mvalue" @change="selchange"  size="small">
                <el-checkbox label="2020"></el-checkbox>
                <el-checkbox label="2019"></el-checkbox>
                <el-checkbox label="2018"></el-checkbox>
                <el-checkbox label="2017"></el-checkbox>
                <el-checkbox label="2016"></el-checkbox>
            </el-checkbox-group>
        </el-form-item>
        <el-dialog title="组件属性" :visible.sync="dialogInputVisible">
            <el-form :model="childpro">

                <el-form-item label="默认值">
                    <el-input v-model="childpro.defval" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="是否多选">
                    <el-switch v-model="childpro.multiple" size="mini" style="width:100%">
                    </el-switch>
                </el-form-item>
                <el-form-item label="勾选框更改时执行事件">
                    <el-input v-model="childpro.jscode" :autosize="{ minRows: 6, maxRows: 9}" autocomplete="off" type="textarea"></el-input>
                </el-form-item>
            </el-form>
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
                    placeholder: "占位符",
                    mvalue: ['2020'],
                    jscode: "",
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
                if (chi.pzoption.childpro.placeholder) {
                    chi.childpro = chi.pzoption.childpro
                }
            })

        },
        watch: {
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