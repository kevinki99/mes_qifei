<style>
    /* 背景色 */
    .bg-0 {
        background-color: #48b0f7 !important;
        color: #fff !important;
    }

    .bg-4 {
        background-color: #57c7d4 !important;
        color: #fff !important;
    }

    .bg-7 {
        background-color: #33cabb !important;
        color: #fff !important;
    }

    .bg-2 {
        background-color: #fcc525 !important;
        color: #fff !important;
    }

    .bg-3 {
        background-color: #15c377 !important;
        color: #fff !important;
    }



    .bg-5 {
        background-color: #faa64b !important;
        color: #fff !important;
    }

    .bg-6 {
        background-color: #f96868 !important;
        color: #fff !important;
    }

    .bg-1 {
        background-color: #f96197 !important;
        color: #fff !important;
    }

    .bg-8 {
        background-color: #926dde !important;
        color: #fff !important;
    }

    .bg-9 {
        background-color: #8d6658 !important;
        color: #fff !important;
    }

    .el-card {
        cursor: pointer;
    }
    .write {
        color: #fff !important;
    }
        .el-card.is-always-shadow, .el-card.is-hover-shadow:focus, .el-card.is-hover-shadow:hover {
            -webkit-box-shadow: 0 2px 12px 0 rgba(0,0,0,.1);
            box-shadow: 0 6px 12px 0 rgba(0,0,0,.1);
        }
</style>
<template>


    <el-col :sm="24" :md="pzoption.mdwidth" v-bind:style="{ height: pzoption.wigheight + 'px' }" style="overflow: auto; overflow-x: hidden;">
        <i class="iconfont icon-shezhi pull-right widgetset hidden-print" @click.stop="dialogInputVisible = true"></i>
        <i class="iconfont icon-shanchu pull-right widgetdel hidden-print" @click.stop="delWid(pzoption.wigdetcode)"></i>
        <el-popover placement="right"
                    width="400"
                    trigger="hover">
            <p style="font-weight:bold">组件数据</p>
            <p v-text="JSON.stringify(pzoption.dataset)"></p>
            <i slot="reference" class="iconfont icon-fabu pull-right widgetdel hidden-print" style="float:right;margin-right:10px"></i>
        </el-popover>
        <p style="FONT-SIZE: 16PX; MARGIN-BOTTOM: 5PX;height:22px" v-text="pzoption.title"></p>
        <el-row :gutter="12">
            <el-col :xs="24" :sm="12" :md="childpro.md" :lg="childpro.md" v-for="(item,index) in pzoption.dataset" :key="index" class="mt10" v-if="pzoption.wdlist.length>0">
                <div @click="kblink(item,childpro.kbcode)">
                    <el-card shadow="hover" class="write" v-bind:style="{'background-color': childpro.kbcolor}">
                        <i class="el-icon-s-data" style="float: right;font-size: 22px;opacity: .9;"></i>
                        <div v-bind:style="{ fontSize: childpro.fz + 'px' }" v-if="pzoption.wdlist.length==1">
                            {{item[pzoption.dllist[0].colid]}}
                            <span style="font-size: 14px;"> {{childpro.dwname}}</span>
                        </div>
                        <div v-bind:style="{ fontSize: childpro.fz + 'px' }" v-if="pzoption.wdlist.length>1">
                            {{item[pzoption.wdlist[1].colid]}}
                            <span style="font-size: 14px;"> {{childpro.dwname}}</span>
                        </div>


                        <div style="font-size:14px"> {{item[pzoption.wdlist[0].colid]}}</div>
                    </el-card>
                </div>
            </el-col>
            <el-col :xs="24" :sm="12" :md="childpro.md" :lg="childpro.md" v-for="(item,index) in pzoption.dataset"  :key="index" class="mt10" v-if="pzoption.wdlist.length==0&&pzoption.dllist.length>0">
                <div @click="kblink(item,childpro.kbcode)">
                    <el-card shadow="hover" class="write"  v-bind:style="{'background-color': childpro.kbcolor}">
                        <i class="el-icon-s-data" style="float: right;font-size: 22px;opacity: .9;"></i>
                        <div v-bind:style="{ fontSize: childpro.fz + 'px' }">{{item[pzoption.dllist[0].colid]}}<span style="font-size: 14px;"> {{childpro.dwname}}</span></div>
                        <div style="font-size:14px"> {{childpro.placeholder}}</div>

                    </el-card>
                </div>
            </el-col>
        </el-row>
        <el-drawer :visible.sync="dialogInputVisible" direction="rtl" style="opacity:.9;" size="40%">

            <el-form :model="childpro" style="padding:20px">
                <el-form-item label="单位">
                    <el-input v-model="childpro.dwname" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="标题" style="display:none">
                    <el-input v-model="childpro.placeholder" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item label="背景色">
                    <el-color-picker v-model="childpro.kbcolor"  style="width: 100%;"
                                     show-alpha
                                     :predefine="predefineColors">
                    </el-color-picker>
                </el-form-item>



                <el-form-item label="每行显示看板数量">
                    <el-radio-group v-model="childpro.md" size="mini" style="width:100%">
                        <el-radio-button :label="24">1个</el-radio-button>
                        <el-radio-button :label="12">2个</el-radio-button>
                        <el-radio-button :label="8">3个</el-radio-button>
                        <el-radio-button :label="6">4个</el-radio-button>
                        <el-radio-button :label="4">6个</el-radio-button>

                    </el-radio-group>
                </el-form-item>

                <el-form-item label="标题字体大小">
                    <el-radio-group v-model="childpro.fz" size="mini" style="width:100%">
                        <el-radio-button :label="32">32px</el-radio-button>
                        <el-radio-button :label="28">28px</el-radio-button>
                        <el-radio-button :label="24">24px</el-radio-button>
                        <el-radio-button :label="18">18px</el-radio-button>
                    </el-radio-group>
                </el-form-item>
                <el-form-item label="点击执行代码（JS）">
                    <el-input v-model="childpro.kbcode" autocomplete="off" type="textarea" :rows="3"></el-input>
                </el-form-item>

            </el-form>
        </el-drawer>
    </el-col>


</template>

<script>
    module.exports = {
        props: ['pzoption', 'index'],
        methods: {
            delWid: function (wigdetcode) {
                this.$root.nowwidget = {};//没这个删除不掉啊
                _.remove(this.$root.FormData.wigetitems, function (obj) {
                    return obj.wigdetcode == wigdetcode;
                });
            },
            kblink: function (rowdata, kbcode) {
                if (kbcode) {
                    try {
                        let func = new Function('rowdata', kbcode);
                        func(rowdata)
                    } catch (e) {
                        app.$notify({
                            title: '成功',
                            message: '解析JS代码有误',
                            type: 'success'
                        });
                    }
                }

            },
            senddata: function () {
                this.pzoption.childpro = JSON.parse(JSON.stringify(this.childpro));
            }
        },
        data: function () {
            return {
                dialogInputVisible: false,
                predefineColors: [
                    '#ff4500',
                    '#ff8c00',
                    '#ffd700',
                    '#90ee90',
                    '#00ced1',
                    '#1e90ff',
                    '#c71585',
                    'rgba(255, 69, 0, 0.68)',
                    'rgb(255, 120, 0)',
                    'hsv(51, 100, 98)',
                    'hsva(120, 40, 94, 0.5)',
                    'hsl(181, 100%, 37%)',
                    'hsla(209, 100%, 56%, 0.73)',
                    '#c7158577'
                ],
                childpro: {
                    kbcolor: 'rgba(31, 147, 255, 0.73)',
                    placeholder: "占位符",
                    dwname: "单位",
                    issign: false,
                    fz: 32,
                    md: 24,
                    kbcode: ""

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
                    //检测新增属性
                    _.forIn(chi.childpro, function (value, key) {
                        if (!_.has(chi.pzoption.childpro, key)) {
                            chi.pzoption.childpro[key] = value;
                        }

                    });
                    chi.childpro = chi.pzoption.childpro;
                } else {
                    chi.pzoption.childpro = chi.childpro;
                    if (chi.pzoption.datatype == '2') {
                        chi.pzoption.staticdata = JSON.stringify([
                            { '月份': '一月', '用户量': 1393 }
                        ])
                        chi.$root.jxfiled(chi.pzoption)
                        chi.$root.addwd("月份", chi.pzoption)
                        chi.$root.addwd("用户量", chi.pzoption)
                    }
                    chi.pzoption.wigheight = 160;
                    chi.$root.UpdateYBData(chi.pzoption);
                }
            })

        },
        watch: {
            childpro: { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    this.senddata();
                },
                deep: true
            }
        },
        computed: {
            // 仅读取
            chiwid: function () {

            }
        }
    };
</script>