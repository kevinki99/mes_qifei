
$.getJSON = function (url, data, success, opt) {
    data.szhlcode = ComFunJS.getCookie("szhlcode");
    var fn = {
        success: function (data, textStatus) { }
    }
    if (success) {
        fn.success = success;
    }
    $.ajax({
        url: url,
        data: JSON.stringify(data),
        dataType: "json",
        type: "post",
        processData: false,
        contentType: "text/json",
        success: function (data, textStatus) {
            if (data.ErrorMsg) {
                top.ComFunJS.winwarning(data.ErrorMsg)
            }
            if (data.uptoken) {
                ComFunJS.setCookie("szhlcode", data.uptoken);//更新Token
            }
            fn.success(data, textStatus);
        },

        beforeSend: function (XHR) {
            XHR.setRequestHeader("Authorization", ComFunJS.getQueryString('token', '0'));
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (errorThrown === "Unauthorized") {
                top.ComFunJS.winwarning("页面超时!")
                top.window.location.href = "/Login.html";
            }
        },
        complete: function (XHR, TS) {

        }
    });
};

var ComFunJS = new Object({
    getQueryString: function (name, defauval) {//获取URL参数,如果获取不到，返回默认值，如果没有默认值，返回空格
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) { return unescape(r[2]); }
        else {
            return defauval || "";
        }
    },//获取参数中数据
    setCookie: function (name, value) {
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
    },
    getCookie: function (name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

        if (arr = document.cookie.match(reg))

            return unescape(arr[2]);
        else
            return null;
    },
    createScript: function (script) {
        var myScript = document.createElement("script");
        myScript.type = "text/javascript";
        myScript.appendChild(document.createTextNode(script));
        document.body.appendChild(myScript);
    },
    isPC: function () {
        //var userAgentInfo = navigator.userAgent;
        //var Agents = ["Android", "iPhone",
        //    "SymbianOS", "Windows Phone",
        //    "iPad", "iPod"];
        //var flag = true;
        //for (var v = 0; v < Agents.length; v++) {
        //    if (userAgentInfo.indexOf(Agents[v]) > 0) {
        //        flag = false;
        //        break;
        //    }
        //}

        var windowWidth = $(window).width();
        if (windowWidth < 640) {
            return false;
        }
        if (windowWidth >= 640) {
            return true;
        }
    },
    loadScriptString: function (code) {
        var script = document.createElement("script");
        script.type = "text/javascript";
        try {
            // firefox、safari、chrome和Opera
            script.appendChild(document.createTextNode(code));
        } catch (ex) {
            // IE早期的浏览器 ,需要使用script的text属性来指定javascript代码。
            script.text = code;
        }
        document.getElementsByTagName("head")[0].appendChild(script);
    },
    JSONToExcelConvertor: function (JSONData, FileName, ShowLabel) {

        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var excel = '<table>';

        //设置表头
        var row = "<tr>";
        for (var i = 0, l = ShowLabel.length; i < l; i++) {
            row += "<td>" + ShowLabel[i].value + '</td>';
        }

        //换行
        excel += row + "</tr>";

        //设置数据
        for (var i = 0; i < arrData.length; i++) {
            var row = "<tr>";

            for (var j = 0; j < arrData[i].length; j++) {
                var value = arrData[i][j].value === "." ? "" : arrData[i][j].value;
                row += '<td>' + value + '</td>';
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
    DateAdd: function (date, strInterval, Number) {
        var dtTmp = date;
        switch (strInterval) {
            case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));

            case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));

            case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));

            case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));

            case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));

            case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

            case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

            case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        }
        return date;
    },
    getfileurl: function (zyidorurl) {
        var url = zyidorurl;
        if (ComFunJS.getCookie("fileapi")) {
            url = ComFunJS.getCookie("fileapi") + ComFunJS.getCookie("qycode") + /document/ + zyidorurl;
        }
        return url;
    },
    StringToDate: function (DateStr) {
        var converted = Date.parse(DateStr);
        var myDate = new Date(converted);
        if (isNaN(myDate)) {
            //var delimCahar = DateStr.indexOf('/')!=-1?'/':'-';
            var arys = DateStr.split('-');
            myDate = new Date(arys[0], --arys[1], arys[2]);
        }
        return myDate;
    },

    format: function (date, str) {
        str = str.replace(/yyyy|YYYY/, date.getFullYear());
        str = str.replace(/MM/, date.getMonth() >= 9 ? ((date.getMonth() + 1) * 1).toString() : '0' + (date.getMonth() + 1) * 1);
        str = str.replace(/dd|DD/, date.getDate() > 9 ? date.getDate().toString() : '0' + date.getDate());
        return str;
    },
    loadStyleString: function (cssText) {
        var script = document.createElement("script");
        script.type = "text/javascript";
        try {
            // firefox、safari、chrome和Opera
            script.appendChild(document.createTextNode(code));
        } catch (ex) {
            // IE早期的浏览器 ,需要使用script的text属性来指定javascript代码。
            script.text = code;
        }
        document.getElementsByTagName("head")[0].appendChild(script);
    },
    isOffice: function (extname) {
        return $.inArray(extname.toLowerCase(), ['doc', 'docx', 'ppt', 'pptx', 'pdf']) != -1
    },
    isPic: function (extname) {
        return $.inArray(extname.toLowerCase(), ['jpg', 'jpeg', 'gif', 'png', 'bmp']) != -1
    },
    getfile: function (fileid) {
        var url = "/api/File/DFile?szhlcode=" + ComFunJS.getCookie("szhlcode");
        if (fileid) {
            url = url + "&fileId=" + fileid;
        }
        return url;
    },
    requestFullScreen: function () {
        var de = document.documentElement;
        if (de.requestFullscreen) {
            de.requestFullscreen();
        } else if (de.mozRequestFullScreen) {
            de.mozRequestFullScreen();
        } else if (de.webkitRequestFullScreen) {
            de.webkitRequestFullScreen();
        }
    },
    exitFullscreen: function () {
        var de = document;
        if (de.exitFullscreen) {
            de.exitFullscreen();
        } else if (de.mozCancelFullScreen) {
            de.mozCancelFullScreen();
        } else if (de.webkitCancelFullScreen) {
            de.webkitCancelFullScreen();
        }
    },
    //弹出帮助函数
    winsuccess: function (content) {
        app.$notify({
            title: '警告',
            message: content,
            type: 'success'
        });


    },//成功窗口

    winwarning: function (content) {
        app.$notify({
            title: '警告',
            message: content,
            type: 'warning'
        });
    },//警告窗口
    formcomponents: [
        { wigcode: "qjInput", wigname: "输入框", wigurl: "/BiManage/AppPage/FORMBI/vue/qjInput.vue", wigtype: "基础组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjInputNum", wigname: "数字", wigurl: "/BiManage/AppPage/FORMBI/vue/qjInputNum.vue", wigtype: "基础组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjSelect", wigname: "下拉框", wigurl: "/BiManage/AppPage/FORMBI/vue/qjSelect.vue", wigtype: "基础组件", ico: "icon-biaodanzujian-xialakuanglv" },
        { wigcode: "qjDate", wigname: "日期时间", wigurl: "/BiManage/AppPage/FORMBI/vue/qjDate.vue", wigtype: "基础组件", ico: "icon-riqi" },
        { wigcode: "qjCheck", wigname: "选中框", wigurl: "/BiManage/AppPage/FORMBI/vue/qjCheck.vue", wigtype: "基础组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjEdit", wigname: "编辑器", wigurl: "/BiManage/AppPage/FORMBI/vue/qjEdit.vue", wigtype: "基础组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjFile", wigname: "文件上传", wigurl: "/BiManage/AppPage/FORMBI/vue/qjFile.vue", wigtype: "基础组件", ico: "icon-shangchuan" },
        { wigcode: "qjSN", wigname: "流水号", wigurl: "/BiManage/AppPage/FORMBI/vue/qjSN.vue", wigtype: "辅助组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjLine", wigname: "分割线", wigurl: "/BiManage/AppPage/FORMBI/vue/qjLine.vue", wigtype: "辅助组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjTab", wigname: "Tab组件", wigurl: "/BiManage/AppPage/FORMBI/vue/qjTab.vue", wigtype: "辅助组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjTable", wigname: "表格", wigurl: "/BiManage/AppPage/FORMBI/vue/qjTable.vue", wigtype: "高级组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjTree", wigname: "选择树", wigurl: "/BiManage/AppPage/FORMBI/vue/qjTree.vue", wigtype: "高级组件", ico: "icon-biaodanzujian-shurukuang" },
        { wigcode: "qjCascader", wigname: "级联选择", wigurl: "/BiManage/AppPage/FORMBI/vue/qjCascader.vue", wigtype: "高级组件", ico: "icon-biaodanzujian-shurukuang" }


    ],
    jsonToTree: function (jsonData, id, pid, children) {
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
    winviewform: function (url, title, width, height, callbact) {
        var width = width || $("body").width() * 2 / 3;
        var height = height || $(window).height() - 140; //$("body").height();
        var optionwin = {
            type: 2,
            fix: true, //不固定
            area: [width + 'px', height + 'px'],
            maxmin: true,
            content: url,
            title: title,
            shadeClose: false, //加上边框
            scrollbar: false,
            shade: 0.4,
            shift: 0,
            success: function (layero, index) {

            },
            end: function () {
                if (callbact) {
                    return callbact.call(this);
                }
            }
        }
        layer.open(optionwin);
    },
    winbtnwin: function (url, title, width, height, option, btcallbact) {
        var width = width || $("body").width() - 300;
        var height = height || $(window).height() * 0.8; //var height = height || $("#main").height();
        var optionwin = {
            type: 2,
            fix: true, //不固定
            area: [width + 'px', height + 'px'],
            maxmin: true,
            content: url,
            title: title,
            shade: 0.4,
            shift: 0,
            shadeClose: false,
            scrollbar: false,
            success: function (layero, index) {
                if ($(layero).find(".successfoot").length == 0) {
                    var footdv = $('<div class="successfoot" style="border-bottom-width: 1px; padding: 0 20px 0 10px;margin-top: -3px;height:50px;background: #fff;"></div>');
                    var btnConfirm = $("<a href='javascript:void(0)' class='btn btn-sm btn-success' style='float:right; margin-top: 10px;width: 140px;'><i class='fa fa-spinner fa-spin' style='display:none'></i> 确   认</a>");
                    var btnCancel = $("<a href='javascript:void(0)' class='btn btn-sm btn-danger' style='float:right; margin-top: 10px;margin-right: 10px;width: 80px;'>取  消</a>");
                    var msg = $("<input type='hidden' class='r_data' >");

                    btnConfirm.appendTo(footdv).bind('click', function () {
                        return btcallbact.call(this, layero, index, btnConfirm);
                    })
                    btnCancel.appendTo(footdv).bind('click', function () {
                        layer.close(index)
                    })
                    $(layero).append(footdv).append(msg);

                    try {
                    } catch (e) { }
                }

            }
        }
        layer.open(optionwin);
    },//带确认框的窗口
    getnowdate: function (format, date) {
        var now = new Date();
        if (date) {
            now = new Date(Date.parse(date.replace(/-/g, '/')));
        }
        format = format.toLowerCase();
        var year = now.getFullYear();       //年
        var month = now.getMonth() + 1;     //月
        var day = now.getDate();            //日
        var hh = now.getHours();
        var mm = now.getMinutes();
        var ss = now.getSeconds();

        var clock = year + "-";
        if (format == "yyyy") {
            clock = year;
            return clock + "";
        }
        if (format == "yyyy-mm") {
            if (month < 10)
                clock += "0";
            clock += month + "-";
        }

        if (format == "yyyy-mm-dd") {
            if (month < 10)
                clock += "0";
            clock += month + "-";
            if (day < 10) {
                clock += "0";
            }
            clock += day + "-";
        }
        if (format == "yyyy-mm-dd hh:mm") {
            if (month < 10)
                clock += "0";
            clock += month + "-";
            if (day < 10) {
                clock += "0";
            }
            clock += day + " ";

            if (hh < 10)
                clock += "0";
            clock += hh + ":";
            if (mm < 10)
                clock += "0";
            clock += mm + ":";

        }
        if (format == "yyyy-mm-dd hh:mm:ss") {
            if (month < 10)
                clock += "0";
            clock += month + "-";
            if (day < 10) {
                clock += "0";
            }
            clock += day + " ";

            if (hh < 10)
                clock += "0";
            clock += hh + ":";
            if (mm < 10)
                clock += "0";
            clock += mm + ":";
            if (ss < 10)
                clock += "0";
            clock += ss + ":";

        }
        return (clock.substr(0, clock.length - 1));
    },//获取当前时间
    manurl: function (url) {
        if (url.indexOf("/api/Auth/ExeAction") > -1) {
            url = url.replace("/api/Auth/ExeAction?Action=", "/API/VIEWAPI.ashx?Action=Auth_")
        }
        if (url.indexOf("/api/Bll/ExeAction") > -1) {
            url = url.replace("/api/Bll/ExeAction", "/API/VIEWAPI.ashx")
        }
        if (url.indexOf("/api/Bll/PubExeAction") > -1) {
            url = url.replace("/api/Bll/PubExeAction?Action=", "/API/VIEWAPI.ashx?Action=Pub_")
        }
        return url;

    }
})

