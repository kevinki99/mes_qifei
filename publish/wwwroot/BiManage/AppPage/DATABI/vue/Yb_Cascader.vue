<template>
    <el-col :sm="24" :md="pzoption.mdwidth">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <el-form-item :label="pzoption.title" :prop="'wigetitems.' + index + '.value'" :rules="childpro.rules">
            <el-input v-model="pzoption.value" style="display:none">
            </el-input>
            <el-cascader v-model="childpro.svalue"
                         :options="childpro.options"
                        
                         :props="childpro.props" v-if="!childpro.props.multiple"
                         @change="handleChange" filterable  collapse-tags ref="refcascader" size="small"></el-cascader>
            <el-cascader v-model="childpro.mvalue"
                         :options="childpro.options"
                      
                         :props="childpro.props" v-if="childpro.props.multiple"
                         @change="handleChange" size="small" filterable  collapse-tags></el-cascader>
        </el-form-item>
        <el-dialog title="组件属性" :visible.sync="dialogInputVisible" width="30%">
            <el-form :model="childpro">
              
                <el-form-item label="是否多选">
                    <el-switch v-model="childpro.props.multiple" size="mini" style="width:100%">
                    </el-switch>
                </el-form-item>
                <el-form-item label="value对应字段">
                    <el-input v-model="childpro.props.value" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="label对应字段">
                    <el-input v-model="childpro.props.label" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="父节点对应字段">
                    <el-input v-model="childpro.props.pvalue" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="默认值">
                    <el-input v-model="childpro.defval" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="选项更改时执行代码">
                    <el-input v-model="childpro.jscode" :autosize="{ minRows: 6, maxRows: 9}" autocomplete="off" type="textarea"></el-input>
                </el-form-item>
            </el-form>
        </el-dialog>
    </el-col>

</template>
<style>
    .el-cascader {
        width: 100%;
    }
</style>
<script>
    module.exports = {
        props: ['pzoption', 'index'],
        data: function () {
            return {
                dialogInputVisible: false,
                childpro: {
                    wigname: "级联选择器",
                    defval: "",
                    jscode: "",
                    multiple: false,
                    mvalue: [],
                    svalue: "",
                    props: {
                        multiple: false,
                        emitPath: false,
                        expandTrigger: "hover",
                        value: "value",
                        label: "label",
                        pvalue: "pval"
                    },
                    options: []
                }
            }
        },
      
        methods: {
            handleChange(value) {

            },
            delWid: function (wigdetcode) {
                // 子组件中触发父组件方法ee并传值cc12345
                this.$root.nowwidget = {};
                _.remove(this.$root.FormData.wigetitems, function (obj) {
                    return obj.wigdetcode == wigdetcode;
                });
            },
            getvatext: function (val) {
                var chi = this;
                var vatext = "";
                var wig = _.find(chi.pzoption.dataset, function (item) {
                    return item[chi.childpro.props.value] == val;
                });
                if (typeof (wig) != "undefined") {
                    var wigp = _.find(chi.pzoption.dataset, function (item) {
                        return item[chi.childpro.props.value] == wig[chi.childpro.props.pvalue];
                    });
                    if (_.isUndefined(wigp)) {
                        vatext = wig[chi.childpro.props.label]
                    } else {
                        vatext = wigp[chi.childpro.props.label] + "/" + wig[chi.childpro.props.label];
                        var pwigp = _.find(chi.pzoption.dataset, function (item) {
                            return item[chi.childpro.props.value] == wigp[chi.childpro.props.pvalue];
                        });
                        if (!_.isUndefined(pwigp)) {
                            vatext = pwigp[chi.childpro.props.label] + "/" + vatext;
                        }
                    }
                }  
                return vatext;
            },
            jsonToTree: function (jsonData, id, pid, children) {
                for (var i = 0; i < jsonData.length; i++) {
                    jsonData[i][id] = jsonData[i][id] + "";
                    jsonData[i][pid] = jsonData[i][pid] + "";

                }//数组里不能为数字
                let result = [],
                    temp = {};
                for (let i = 0; i < jsonData.length; i++) {
                    temp[jsonData[i][id]] = jsonData[i]; // 以id作为索引存储元素，可以无需遍历直接定位元素
                }
                for (let j = 0; j < jsonData.length; j++) {
                    let currentElement = jsonData[j];
                    let tempCurrentElementParent = temp[currentElement[pid]]; // 临时变量里面的当前元素的父元素
                    if (tempCurrentElementParent) {
                        // 如果存在父元素
                        if (!tempCurrentElementParent[children]) {
                            // 如果父元素没有chindren键
                            tempCurrentElementParent[children] = []; // 设上父元素的children键
                        }
                        tempCurrentElementParent[children].push(currentElement); // 给父元素加上当前元素作为子元素
                    } else {
                        // 不存在父元素，意味着当前元素是一级元素
                        result.push(currentElement);
                    }
                }
                return result;
            }
        },
        mounted: function () {
            var pro = this;
            pro.$nextTick(function () {
                if (pro.$root.addchildwig) {
                    pro.$root.addchildwig();//不能缺少,dom加载完成
                }
                if (pro.pzoption.childpro.wigname) {
                    pro.childpro = pro.pzoption.childpro;
                    pro.$root.UpdateYBData(pro.pzoption, function (ybdata) {
                        if (app.isview) {//添加页面
                            pro.pzoption.value = app.jxparam(pro.childpro.defval);
                        }
                    });
                } else {
                    if (pro.pzoption.datatype == '2') {
                        pro.pzoption.staticdata = JSON.stringify([
                            { 'label': '一月', 'value': "1", 'pval': "0" },
                            { 'label': '二月', 'value': "2", 'pval': "1" },
                            { 'label': '三月', 'value': "3", 'pval': "1" },
                            { 'label': '四月', 'value': "4", 'pval': "3" }
                        ]);
                        pro.$root.jxfiled(pro.pzoption);
                        pro.$root.addwd("label", pro.pzoption);
                        pro.$root.addwd("value", pro.pzoption);
                        pro.pzoption.dataset = JSON.parse(pro.pzoption.staticdata);

                    }
                    pro.pzoption.childpro = pro.childpro;
                }



            })

        },
        watch: {
            'pzoption.value': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV) {
                        var pro = this;
                        if (pro.pzoption.childpro.props.multiple) {
                            pro.pzoption.childpro.mvalue = newV.split(',');
                            pro.childpro.mvalue = newV.split(',');
                        } else {
                            pro.pzoption.childpro.svalue = newV;//为了变成字符
                            pro.childpro.svalue = newV;//为了变成字符
                        }
                    }
                },
                deep: true
            },
            'childpro.svalue': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    var pro = this;
                    if (newV && !pro.pzoption.childpro.multiple) {
                        pro.pzoption.value = newV;
                        pro.pzoption.valuetext = pro.getvatext(pro.pzoption.value)
                    } else {
                        pro.pzoption.value = "";
                        pro.pzoption.valuetext = "";
                    }
                    var data = pro.pzoption.value;
                    let jscode = pro.childpro.jscode;
                    let func = new Function('data', jscode);
                    func(data)

                },
                deep: true
            },
            'childpro.mvalue': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    var chi = this;
                    if (newV && chi.pzoption.childpro.multiple&&newV.length != 0 && JSON.stringify(newV) != JSON.stringify(oldV)) {
                        chi.pzoption.value = newV.join();
                        //多选valuetext没空搞
                    }
                },
                deep: true,
                immediate: true//加载初始值
            },
            'pzoption.dataset': { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    var pro = this;
                    if (newV && newV.length != 0 && JSON.stringify(newV) != JSON.stringify(oldV)) {
                        var opidata = pro.jsonToTree(newV, pro.pzoption.childpro.props.value, pro.pzoption.childpro.props.pvalue, "children");
                        pro.childpro.options = opidata;
                    }
                    if (newV.length == 0) {
                        pro.childpro.options = [];
                    }

                },
                deep: true
            },

        }
    };
</script>