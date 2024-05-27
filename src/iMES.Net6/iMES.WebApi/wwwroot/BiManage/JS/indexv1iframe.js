var model = new Vue({
    el: '#DATABI_YBZZ',
    components: {
        'base-loading': httpVueLoader('/BiManage/Base/Vue/Loading.vue'),
        'base-iframe': httpVueLoader('/BiManage/Base/Vue/Iframe.vue')
    },
    data: {
        userName: ComFunJS.getnowuser(),
        CommonData: [],//消息中心
        PageCode: "base-loading",//需要加载的模板
        PageUrl: "",//需要加载的IframeUrl
        rdm: ComFunJS.getnowdate('yyyy-mm-dd hh:mm'),//随机数
        yytype: "WORK",
        UserData: {},//用户信息
        UserInfo: {},//用户缓存数据
        CompanyData: {},//企业信息
        nowpage: {},
        isshowload: true,
        isiframe: "N",

        isnull: false,//是否有数据
        menutype: "WORK",
        UseYYList: [],
        PageS: [],
        pagedata: {
            PageUrl: "",
            ExtData: "",
            ActionData: "",
            UserInfo: {}
        },
        funid: ComFunJS.getQueryString("funid", "44"),


    },
    methods: {
        changetest: function () {
            model.$options.components["xtgl-index"] = httpVueLoader('/BiManage/AppPage/XTGL/Vue/INDEX.vue');
            model.PageCode1 = "xtgl-index"
        },

        selmenulev2: function (item) {

            model.pagedata.ExtData = item.ExtData;
            model.pagedata.ActionData = item.ActionData;
            model.isiframe = item.isiframe;
            model.PageCode = "base-loading";
            gomenu = function () {
                if (model.isiframe == 'Y') {
                    var pagecode = item.PageCode.indexOf("html") > -1 || item.PageCode.indexOf("aspx") > -1 ? item.PageCode : item.PageCode + ".html";
                    model.PageCode = "base-iframe";
                    model.PageUrl = pagecode;

                } else {
                    var tempcode = "work_" + _.lowerCase(_.last(item.PageCode.split('/'))) + item.ID;
                    model.$options.components[tempcode] = httpVueLoader(item.PageCode + '.vue?v=' + ComFunJS.getnowdate('yyyy-mm-dd'));
                    model.PageCode = tempcode;

                }
                model.nowpage = item;
            }
            setTimeout("gomenu()", 200)


        },
        GetUserData: function () {


            $.getJSON('/api/Auth/ExeAction?Action=GETUSERBYUSERNAME', { P1: ComFunJS.getnowuser() }, function (resultData) {
                if (resultData.ErrorMsg == "" && resultData.Result) {
                    model.UserInfo = resultData.Result;
                    model.UserData = resultData.Result.User;
                    model.CompanyData = resultData.Result.QYinfo;
                    $(document).attr("title", model.CompanyData.QYName);//修改title值
                    ComFunJS.setCookie('fileapi', resultData.Result.QYinfo.FileServerUrl);
                    ComFunJS.setCookie('qycode', resultData.Result.QYinfo.QYCode);
                    ComFunJS.setCookie('userinfo', model.UserData.UserName + "," + model.UserData.UserRealName + "," + model.UserData.BranchCode + "," + model.UserInfo.BranchInfo.DeptName);
                    ComFunJS.setCookie('qxcode', resultData.Result.UserBMQXCode);
                    ComFunJS.setCookie('zid', model.UserData.BranchCode);
                    ComFunJS.setCookie('zname', resultData.Result.BranchInfo.DeptName);

                }
            })


        }, //获取用户信息
        AddView: function (code, Name, ID, pcode, event) {
            if (event) {
                event.stopPropagation();
            }
            if (code == "QYTX" || code == "DXGL") {
                ComFunJS.winviewform("/View/Base/APP_ADD_WF.html?FormCode=" + code, Name, "1000");
            }
            else {
                if (!ID) {
                    ID = "";
                }
                if (pcode == "CRM") {
                    code = pcode + "_" + code;
                }
                ComFunJS.winviewform("/BiManage/AppPage/APP_ADD_WF.html?FormCode=" + code + "&ID=" + ID, Name, "1000");

            }
        },//添加表格
        AddViewNOWF: function (code, Name, ID, pcode, event) {
            if (event) {
                event.stopPropagation();
            }
            if (!ID) {
                ID = "";
            }
            ComFunJS.winviewform("/BiManage/AppPage/APP_ADD.html?FormCode=" + code + "&ID=" + ID, Name, "1000");

        },
        EditViewNOWF: function (code, ID, pid, event) {
            if (event) {
                event.stopPropagation();
            }
            event = event ? event : window.event
            var obj = event.srcElement ? event.srcElement : event.target;
            if ($(obj).hasClass("icon-check") || $(obj).attr("type") == "checkbox") {
                return;
            } else {
                ComFunJS.winviewform("/BiManage/AppPage/APP_ADD.html?FormCode=" + code + "&ID=" + ID, "查看");
            }
        },
        ViewForm: function (code, ID, PIID, event) {
            event = event ? event : window.event
            var obj = event.srcElement ? event.srcElement : event.target;
            if ($(obj).hasClass("icon-check") || $(obj).attr("type") == "checkbox") {
                return;
            } else {
                ComFunJS.winviewform("/BiManage/AppPage/APPVIEW.html?FormCode=" + code + "&ID=" + ID + "&PIID=" + PIID + "&r=" + Math.random(), "查看");

            }
        },//查看表格方法
        ViewFormNew: function (code, ID, PIID, event) {
            event = event ? event : window.event
            var obj = event.srcElement ? event.srcElement : event.target;
            if ($(obj).hasClass("lk")) {
                ComFunJS.winviewform("/BiManage/AppPage/APPVIEW.html?FormCode=" + code + "&ID=" + ID + "&PIID=" + PIID + "&r=" + Math.random(), "查看");

            }
        },//查看表格方法
        EditForm: function (code, ID, PIID, event) {
            if (event) {
                event.stopPropagation();
            }
            ComFunJS.winviewform("/BiManage/AppPage/APP_ADD_WF.html?FormCode=" + code + "&ID=" + ID + "&PIID=" + PIID + "&r=" + Math.random(), "修改", "1000");
        },
        GetYYList: function () {
            $.getJSON('/api/Auth/ExeAction?Action=GETINDEXMENUNEW', {  }, function (resultData) {
                if (resultData.ErrorMsg == "") {
                    for (var i = 0; i < resultData.Result.length; i++) {
                        var mk = resultData.Result[i];
                        for (var m = 0; m < mk.FunData.length; m++) {
                            if (mk.FunData[m].ID == model.funid) {
                                model.selmenulev2(mk.FunData[m]);
                                return
                            }
                        }
                    }
                    // model.selmenulev2(resultData.Result)
                }
            })
        }

    },
    mounted: function () {
        var pro = this;
        pro.$nextTick(function () {
            pro.GetYYList();
            pro.GetUserData();
        })

    },
    watch: {
        PageUrl: { //深度监听，可监听到对象、数组的变化
            handler(newV, oldV) {
                this.pagedata.PageUrl = newV;
            },
            deep: true
        },
        UserInfo: { //深度监听，可监听到对象、数组的变化
            handler(newV, oldV) {
                this.pagedata.UserInfo = newV;
            },
            deep: true
        }
    }
})