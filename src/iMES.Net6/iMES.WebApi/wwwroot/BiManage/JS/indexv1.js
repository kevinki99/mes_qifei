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
        isgdkd: "N",//是否固定宽度
        XXCount: 0,//消息数量
        QYGGData: [],//企业公告
        YYData: [],
        LMData: [],
        ishasRight: true,//是否要隐藏左侧菜单
        FunData: [],//选中模块
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
        isglz: false,//是否启用门户模式
        TreeVisible: false,
        treedata: null,
        glzname: "",
        defaultProps: {
            highlightcurrent: true
        }
    },
    methods: {
        showtree: function () {
            model.TreeVisible = true;
        },
        clearlocadata: function () {
            localStorage.clear();
            model.$message({
                message: '缓存已清空!!',
                type: 'success'
            });
        },
        clearallcook: function () {
            var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
            if (keys) {
                for (var i = keys.length; i--;)
                    document.cookie = keys[i] + '=0;expires=' + new Date(0).toUTCString()
            }
        },
        changetest: function () {
            model.$options.components["xtgl-index"] = httpVueLoader('/BiManage/AppPage/XTGL/Vue/INDEX.vue');
            model.PageCode1 = "xtgl-index"
        },
        selMenu2: function (item) {
            for (var i = 0; i < model.UseYYList.length; i++) {
                model.UseYYList[i].isactive = false;
            }
            item.isactive = true;
        },
        selTab: function (item, index) {
            model.pagedata.ExtData = item.ExtData;
            model.pagedata.ActionData = item.ActionData;
            for (var i = 0; i < model.PageS.length; i++) {
                model.PageS[i].isactive = false;
            }
            model.isiframe = item.isiframe;
            if (model.isiframe == 'Y') {
                var pagecode = item.PageCode.indexOf("html") > -1 || item.PageCode.indexOf("aspx") > -1 ? item.PageCode : item.PageCode + ".html";
                model.PageCode = "base-iframe";
                model.PageUrl = pagecode;
            } else {
                var tempcode = _.lowerCase(model.SelModel.ModelCode) + "_" + _.lowerCase(_.last(item.PageCode.split('/'))) + item.ID;;
                if (typeof (model.$options.components[tempcode]) == undefined || model.$options.components[tempcode] == null) {
                    model.$options.components[tempcode] = httpVueLoader(item.PageCode + '.vue');
                }
                model.PageCode = tempcode;
            }
            item.isactive = true;
            model.nowpage = item;
            $(".toptab").removeClass("active");
            $(".toptab").eq(index).addClass("active")


        },
        RemoveTab: function (item, index) {
            item.isactive = false;
            model.PageS.splice(index, 1)
            model.selTab(_.last(model.PageS));
        },
        addTab: function (pageurl, pagename, tabdata) {
            for (var i = 0; i < model.PageS.length; i++) {
                model.PageS[i].isactive = false;
                if (model.PageS[i].PageCode == pageurl) {
                    model.PageS.splice(i, 1);
                }
            }
            var tempcode = _.lowerCase(model.SelModel.ModelCode) + "_" + _.lowerCase(_.last(pageurl.split('/')));
            model.$options.components[tempcode] = httpVueLoader(pageurl + '.vue');
            var tabpage = { PageCode: pageurl, isactive: true, isiframe: "N", PageName: pagename, ExtData: tabdata };
            model.PageS.push(tabpage);
            model.PageCode = tempcode;
            model.nowpage = tabpage;
            model.pagedata.ExtData = tabdata;
            if (model.PageS.length > 6) {
                model.PageS.splice(0, 1);

            }
        },
        SelModelMenu: function (item) {

            item.isactive = true;
            var nowTime = new Date().getTime();
            var clickTime = $("body").data("ctime");
            if (clickTime != 'undefined' && (nowTime - clickTime < 1000) && item) {
                console.debug('操作过于频繁，稍后再试');
                return false;
            } else {
                $("body").data("ctime", nowTime);
                model.FunData = [];
                if (item) {
                    model.SelModel = item;
                    model.FunData = item.FunData;
                    model.ishasRight = false;
                } else {
                    model.SelModel = null;

                    model.FunData = [
                        { PageCode: "/BiManage/AppPage/FORMBI/BDSHLIST", ExtData: "", code: "LCSP", PageName: "待办及审批", issel: true, isshow: true, order: 0 }
                    ];
                    localStorage.setItem("WIGETDATAV5", JSON.stringify(model.FunData));
                    model.ishasRight = true;
                }
                model.selmenulev2(model.FunData[0]);
                $('body,html').animate({ scrollTop: 0 }, '500');
            }

        },//选中最左侧事件
        SelModelXX: function () {
            model.FunData = [];
            model.SelModel = null;
            model.FunData = [{ PageCode: "/BiManage/AppPage/XXZX/XXZXLIST", PageName: "消息管理", issel: true, isshow: true, order: 0 }];
            model.ishasRight = false;
            model.selmenulev2(model.FunData[0]);
        },//选中最左侧事件
        SelUserInfo: function () {
            model.FunData = [];
            model.SelModel = null;
            model.FunData = [{ PageCode: "/BiManage/AppPage/XTGL/UserCenter", PageName: "个人中心", issel: true, isshow: true, order: 0 }];
            model.ishasRight = false;
            model.selmenulev2(model.FunData[0]);
        },//选中消息的事件
        selmenulev2: function (item) {
            for (var i = 0; i < model.FunData.length; i++) {
                model.FunData[i].isactive = false;
            }
            // model.PageS = [];

            for (var i = 0; i < model.PageS.length; i++) {
                model.PageS[i].isactive = false;
            }
            if (_.findIndex(model.PageS, function (obj) {
                return obj.PageCode == item.PageCode && obj.ExtData == item.ExtData;
            }) > -1) {
                _.find(model.PageS, function (obj) {
                    return obj.PageCode == item.PageCode && obj.ExtData == item.ExtData;
                }).isactive = true;

            } else {
                item.isactive = true;
                model.PageS.push(item);
            }
            if (model.PageS.length > 6) {
                //超过6个,删除第一个
                model.PageS.splice(0, 1);

            }
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
                    var tempcode = _.lowerCase(model.SelModel.ModelCode) + "_" + _.lowerCase(_.last(item.PageCode.split('/'))) + item.ID;
                    model.$options.components[tempcode] = httpVueLoader(item.PageCode + '.vue?v=' + ComFunJS.getnowdate('yyyy-mm-dd'));
                    model.PageCode = tempcode;

                }
                model.nowpage = item;
            }
            setTimeout("gomenu()", 200)


        },
        //选中二级菜单事件
        ChangePage: function (pagedata) {
            model.selmenulev2(pagedata);
        },
        refpage: function (pagecode) {
            model.rdm = ComFunJS.getnowdate('yyyy-mm-dd hh:mm:ss');
            if (model.isiframe == 'Y') {
                $('#main').attr('src', $('#main').attr('src'));
            } else {
                if (pagecode) {
                    model.$options.components[pagecode] = null;//清除keeplive组件缓存
                    model.selmenulev2(model.nowpage);
                    return;
                } else {
                    model.refpage(model.PageCode)
                }
            }
        },//刷新页面
        initwork: function () {
            localStorage.removeItem("WIGETDATAV5");
            location.reload();
        },


        exit: function () {
            ComFunJS.winconfirm("确认要退出吗？", function () {
                ComFunJS.delCookie('szhlcode');
                model.clearallcook();
                location.href = "/index.html"
            })
        },//退出事件
        refiframe: function () {
            location.reload();

        }, //刷新当前页面
        selyyType: function (item) {

            model.PageS = [];
            model.yytype = item.TYPE;

            var yycount = 0;
            for (var i = 0; i < model.UseYYList.length; i++) {
                if (model.UseYYList[i].PModelCode == model.yytype && model.yytype != "WORK") {
                    yycount++;
                }
                model.UseYYList[i].isactive = false;
            }

            if (yycount == 1) {
                $(".leftlayout ").hide()
                $(".rightlayout ").css({ "margin-left": "0px" })

            } else {
                $(".leftlayout ").show()
                $(".rightlayout ").css({ "margin-left": "210px" })
            }
            for (var i = 0; i < model.LMData.length; i++) {
                model.LMData[i].isactive = false;
            }
            item.isactive = true;

            var inititem = _.find(model.UseYYList, function (obj) {
                return obj.PModelCode == model.yytype;
            })
            model.SelModelMenu(inititem)


        },//应用类别
        GetXXZXList: function () {
            $.getJSON('/api/Auth/ExeAction?Action=GETXXZXIST', {}, function (resultData) {
                if (resultData.ErrorMsg == "") {
                    model.CommonData = resultData.Result;
                    model.XXCount = resultData.Result1;
                }
            })
        },//加载消息中心
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
                    $("#divDeptName").text(resultData.Result.BranchInfo.DeptName)
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
            $.getJSON('/api/Auth/ExeAction?Action=GETINDEXMENUNEW', { P1: "PCINDEX" }, function (resultData) {
                if (resultData.ErrorMsg == "") {
                    var funcode = ComFunJS.getQueryString("fcode");
                    var tempfun = [];
                    if (resultData.Result) {
                        resultData.Result.forEach(function (val) {
                            val.FunData.forEach(function (c) {
                                c.msgcount = 0;
                                if (funcode) {
                                    if (funcode == c.ID) {
                                        c.isshow = true;
                                        tempfun.push(val);
                                    } else {
                                        c.isshow = false;

                                    }
                                }
                            })
                            val.issel = val.issel == "True";
                            val.isactive = false;

                        })
                    }
                    if (tempfun.length > 0) {
                        tempfun[0].FunData = _.filter(tempfun[0].FunData, function (o) { return o.ID == funcode; });
                        resultData.Result = tempfun;
                    }
                    // YYData
                    var temp = [];
                    for (var i = 0; i < resultData.Result.length; i++) {
                        if ($.inArray(resultData.Result[i].ModelType, temp) == -1) {
                            temp.push(resultData.Result[i].ModelType)
                            model.LMData.push({ "TYPE": resultData.Result[i].PModelCode, "NAME": resultData.Result[i].ModelType, "ISSEL": "N", "isactive": false });
                        }

                    }

                    model.UseYYList = resultData.Result;

                    //初始菜单
                    model.yytype = model.LMData[0].TYPE;
                    model.LMData[0].isactive = true;
                    var inititem = _.find(model.UseYYList, function (obj) {
                        return obj.PModelCode == model.LMData[0].TYPE;
                    })
                    inititem.isactive = true;
                    model.SelModelMenu(inititem)

                }
            })
        },
        UploadHeadImage: function () {
            ComFunJS.winviewform("/BiManage/AppPage/XTGL/UploadTX.html", "头像上传", "700", "570");
        },  //上传头像
        ModifyPwd: function (dom) {
            var pwd = $("#newPwd").val();
            var pwd2 = $("#newPwd2").val();
            var retmsg = "";
            if ($("#UpdatePDModal .szhl_require")) {
                $("#UpdatePDModal .szhl_require").each(function () {
                    if ($(this).val() == "") {
                        retmsg = $(this).parent().find("label").text() + "不能为空";
                    }
                })
            }
            if (retmsg !== "") {
                top.layer.tips(retmsg, dom);
                return;
            }
            if (pwd != pwd2) {
                retmsg = "确认密码不一致";
                top.layer.tips(retmsg, dom);
                return;
            }
            $.getJSON("/api/Auth/ExeAction?Action=MODIFYPWD", { P1: pwd, P2: pwd2 }, function (jsonresult) {
                $(dom).removeClass("disabled").find("i").hide();
                if ($.trim(jsonresult.ErrorMsg) == "") {
                    top.ComFunJS.winsuccess("操作成功");
                    $('#UpdatePDModal').modal('hide');
                }
            });
            $("#newPwd").val("");
            $("#newPwd2").val("");
        },  //修改密码  待完善


        GetTypeData: function (P1, callback) {//P1:字典类别，callback:回调函数,p2:字典类别ID
            $.getJSON('/api/Auth/ExeAction?Action=GETZIDIANSLIST', { P1: P1 }, function (resultData) {
                if (resultData.ErrorMsg == "") {
                    if (callback) {
                        return callback.call(this, resultData.Result);
                    }
                    else {
                        model.TypeData = resultData.Result;
                    }
                }
            })
        },

        ViewXXFB: function (xxitem) {
            ComFunJS.winviewform("/BiManage/AppPage/XXFB/XXFBVIEW.html?ID=" + xxitem.ID + "&r=" + Math.random(), "新闻公告");
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
        },
        GetZList: function () {
            $.getJSON('/api/Bll/ExeAction?Action=SFJS_GETGLZINFO', { P1: "" }, function (resultData) {
                if (resultData.ErrorMsg == "") {
                    var tempdata = resultData.Result;
                    if (ComFunJS.getCookie("zid")) {
                        model.glzname = ComFunJS.getCookie("zname");
                    } else {
                        for (var i = 0; i < tempdata.length; i++) {
                            if (tempdata[i].isfz == "Y") {
                                model.glzname = tempdata[i].label;
                                ComFunJS.setCookie("zid", tempdata[i].id);
                                ComFunJS.setCookie("zname", tempdata[i].label);
                                break;
                            }
                        }
                    }
                    if (!model.glzname) {
                        this.$confirm('您没有管理站的权限,请联系管理员授权', '提示', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }).then(() => {
                            model.exit();
                        }).catch(() => {
                            model.exit();
                        });
                    } else {
                        model.treedata = model.jsonToTree(tempdata, "id", "pid", "children");
                    }
                }
            })
        },

        glzchange: function (data, node) {
            var S = data;
            var S1 = node;
        },
        qdglz: function () {
            var data = model.$refs.ztree.getCurrentNode()
            if (data.isfz != "Y") {
                this.$notify({
                    title: '警告',
                    message: '选择一个管理站!',
                    type: 'warning'
                });
            } else {
                model.TreeVisible = false;
                model.glzname = data.label;
                ComFunJS.setCookie("zid", data.id);
                ComFunJS.setCookie("zname", data.label);
                model.refpage();

            }
        }
    },
    mounted: function () {
        var pro = this;
        pro.$nextTick(function () {
            if (model.isglz) {
                pro.GetZList();
            }
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