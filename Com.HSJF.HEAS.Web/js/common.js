var sys_mode = "release";

var getDate = function () {
    var myDate = new Date();
    return myDate.Format("yyyy-MM-dd");
};

var getDateTime = function () {
    var myDate = new Date();
    return myDate.Format("yyyy-MM-dd hh:mm:ss");
};

String.prototype.startWith = function (str) {
    var reg = new RegExp("^" + str);
    return reg.test(this);
};

String.prototype.endWith = function (str) {
    var reg = new RegExp(str + "$");
    return reg.test(this);
};

String.prototype.IsDateStr = function () {
    return /^\/Date\(-?\d+\)\/$/.test(this);
};

String.prototype.format = function (args) {
    if (arguments.length > 0) {
        var result = this;
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                var reg = new RegExp("({" + key + "})", "g");
                result = result.replace(reg, args[key]);
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] == undefined) {
                    return "";
                }
                else {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
        return result;
    }
    else {
        return this;
    }
};

Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

var GetTempID = function () {
    return ("TEMP" + Math.round(new Date().getTime() / 1000).toString());
};

var IsArray = function (obj) {
    if (obj == null) {
        return false;
    }
    else {
        return (obj.length && (typeof (obj) == "object"));
    }
};

var IsObject = function (obj) {
    if (obj == null) {
        return false;
    }
    else {
        var isObj = (typeof (obj) == "object");
        var isArr = obj.length;
        return (isObj && (!isArr));
    }
};

var OpenDialog = function (id, save,type,obj) {
    var size = $("#" + id).attr("dialog-size");
    var title = $("#" + id).attr("dialog-title");

    var dWidth, dHeight;
    if (size == "large") {
        dWidth = "70%";
        dHeight = $(window).height() * 0.9;
    }
    else {
        dWidth = "50%";
        dHeight = "auto";
    }
    var buttom = [];
    var readonly = [{
        text: "关闭",
        "class": "btn btn-xs",
        click: function () {
            $(this).dialog("close");
        }
    }];
    var edit = [
            {
                text: "保存",
                "class": "btn btn-primary btn-xs",
                click: function () {
                    if (ValidateCheck("#" + id, this)) {
                        var isfinished = false;
                        var files = [];
                        for (var i = 0; i < $(".widninty").length; i++) {
                            if ($(".widninty").eq(i).attr("data-percent") != "100%") {
                                isfinished = true;
                            }
                            files.push($(".widninty").eq(i).attr("data-key"));
                        }
                        if (isfinished) {
                            ConfirmBox("还有文件未上传完毕，是否确定放弃未上传完成的文件", function () {
                                for (var i = 0; i < files.length; i++) {
                                    if ($("[data-key=" + files[i] + "]").attr("data-percent") != "100%") {
                                        $("[data-key=" + files[i] + "]").parent().remove();
                                    }
                                }
                                save();
                                $("#" + id).dialog("close");
                            });
                        } else {
                            save();
                            $(this).dialog("close");
                        }
                    }
                }
            },
            {
                text: "取消",
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
    ];
    if(type=="disabled"){
    	buttom = readonly;
    }else{
    	buttom = edit;
    }
    var dialog = $("#" + id).removeClass('hide').dialog({
        modal: true,
        title: "<div class='widget-header widget-header-small'><h5 class='smaller'><i class='icon-file'></i>" + title + "</h5></div>",
        title_html: true,
        width: dWidth,
        height: dHeight,
        draggable: false,
        resizable: false,
        beforeClose: function () {
            reset_form(id);
        },
        buttons: buttom
    });
};

var SaveForm = function (form_id, className) {
    var form = $("#" + form_id);
    var table = $("#" + (form.attr("for")));
    var data = GetJsonData(className, form); //console.log(JSON.stringify(data));
    var arr = [];
    if(form_id=="dialog_person"&&location.href.indexOf("Audit")!=-1){
    	for(var filename in data){
    		if(filename.indexOf("FileName")!=-1){
//  			console.log(filename);
//  			console.log(data[filename]);
    			var json_file = $.parseJSON(data[filename]);
    			for(var file_fir in json_file){
    				if($("#"+file_fir).attr("isdelete")==1){
    					console.log($("#"+file_fir).attr("isdelete"));
    					console.log(file_fir);
    					data[filename].replace(file_fir,file_fir+"|1")
    				}else{
    					console.log($("#"+file_fir).attr("isdelete"));
    					console.log(file_fir);
    					data[filename].replace(file_fir,file_fir+"|0")
    				}
//  				obj_json[file_fir+"|0"]=json_file[file_fir];
    			}
//  			data[filename] = JSON.stringify(obj_json);
    		}
    	}
//		for(var file_name in json_file){
//			if($("#"+file_name).attr("isdelete")==1){
//				obj_json[file_name+"|1"]=json_file[file_name];
//			}else{
//				obj_json[file_name+"|0"]=json_file[file_name];
//			}
//		}
//		data.MarryFileName = JSON.stringify(obj_json);
    }
    console.log(typeof data.IdentificationFileName);
    console.log(data.IdentificationFileName);
    arr.push(data);
    SetJsonData(arr, className, table);
    return;
};

$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    _title: function (title) {
        var $title = this.options.title || '&nbsp;'
        if (("title_html" in this.options) && this.options.title_html == true)
            title.html($title);
        else title.text($title);
    }
}));

//获取Get参数
var $_GET = (function () {
    var url = window.document.location.href.toString();
    url = url.replace("#", "");
    var u = url.split("?");
    if (typeof (u[1]) == "string") {
        u = u[1].split("&");
        var get = {};
        for (var i in u) {
            var j = u[i].split("=");
            get[j[0]] = j[1];
        }
        return get;
    } else {
        return {};
    }
})();

function showAlert(alert_text, style, duration) {
    if (!alert_text) { alert_text = "保存成功"; }
    if (!style) { style = "success"; }
    if (!duration) { duration = '3000'; }

    $.gritter.add({
        text: alert_text,
        time: duration,
        class_name: 'gritter-light gritter-' + style + ' gritter-center'
    });
}

function reset_filter(obj) {
    var field_set = null;
    if (typeof (obj) == "string") {
        field_set = $("#" + obj);
    }
    else if (typeof (obj) == "object") {
        field_set = $("#" + obj.attr("for"));
    }

    field_set.find("input").val("");
    field_set.find("select").val("");
}

function reset_form(id) {
    var obj = $("#" + id);

    obj.find("input:text").val("");
    obj.find("input:radio").removeAttr("checked");
    obj.find("input[type='checkbox']").attr("checked", false);
    obj.find("select").val("");
    obj.find("textarea").val("");
    obj.find("tbody").empty();

    obj.find("[data-type='files']").prev().find("span").text("(0)");
    obj.find("[data-type='files'] .fileupload_list").empty();

    initHint("#" + id);
}

var GetDictionary = function (select, key, func) {
    var ctl = $(select);
    var list = [{ name: "dictype", value: key }];
    $.get("/Other/FindByTypeList", list, function (r) {
        for (var i = 0; i < r.length; i++) {
            ctl.append("<option value='" + r[i].Path + "'>" + r[i].Text + "</option>");
        }

        if (func) { func(); }
    }, "json");
};

var GetJsonData = function (className, target) {
    if (target) {
        target = $(target);
    }
    else {
        target = $("#hs-page-content");
    }

    var json = {};

    //获取所有data-class匹配的元素
    var controls = target.find("[data-class='{0}']".format(className));

    //遍历所有元素
    controls.each(function () {
        var ctl = $(this);
        var dField = ctl.attr("data-field");
        var dChild = ctl.attr("data-child");
        var dType = ctl.attr("data-type");

        //判断是否有【data-child】属性
        if (ctl.is("[data-child]")) {
            //如果元素是table，且有data-child属性，则需遍历其所有tbody tr。
            if (ctl.is("table")) {
                //如果data-child没有值，则返回false
                if (!dChild) {
                    return false;
                }
                else {
                    var rows = ctl.find("tbody:first>tr");
                    var json_arr = [];
                    rows.each(function (index) {
                        var row_data = GetJsonData(dChild, $(this));
                        row_data.Sequence = index + 1;
                        json_arr.push(row_data);
                    });
                    json[dField] = json_arr;
                }
            }
            else {
                var dValue = GetJsonData(dChild, ctl);
                json[dChild] = dValue;
            }
        }
        else {
            var dValue;
            if (ctl.attr("data-type") == "files") { //附件
                var str = "";
                var nameValue = {};
                ctl.find(".fileupload_list>li[complete]").each(function () {
                    var filename = $(this).find("a").text();
                    var isDelete = $(this).attr("isdelete");
                    if(isDelete=="1"){
                    	isDelete = "1";
                    }else{
                    	isDelete = "0";
                    }
                    var id = $(this).attr("id")
                    
                    str += id + ",";
					if(filename.indexOf("|")!=-1){
						nameValue[id] = filename;
					}else{
						nameValue[id] = filename +"|"+isDelete;
					}
                });
                if (str) {
                    str = str.substring(0, str.length - 1);
                }

                dValue = str;

                var nameValueTemp = (nameValue == {}) ? "{}" : JSON.stringify(nameValue);
                json[dField + "Name"] = nameValueTemp;

            }
            else if (ctl.is("select")) {
                dValue = ctl.val();
                json[dField + "Text"] = ctl.find("option:selected").text(); //额外的属性：用以存储字典项Text值。后台不会处理此字段。
            }
            else if (ctl.is("input:text") || ctl.is("textarea") || ctl.is("input:hidden")) {
                if (ctl.attr("onkeyup") != undefined) {
                    if (ctl.attr("onkeyup").indexOf("formoney") != -1) {
                        dValue = rmoney(ctl.val()).toString() == "NaN" ? "" : rmoney(ctl.val());
                    } else {
                        dValue = ctl.val();
                    }
                } else {
                    dValue = ctl.val();
                }
            }
            else if (ctl.is("td") || ctl.is("div")) {
                if (ctl.find("input:radio").length > 0) {
                    dValue = ctl.find("input:radio").eq(0).is(":checked");
                }
                else {
                    dValue = ctl.text();
                }
            }
            else if (ctl.is("input:radio") || ctl.is("input:checkbox")) {
                dValue = ctl.is(":checked");
            }
            else {
                //如果是非预期的元素，则打印警告，并将值设置为null
                //console.log("Warning: unhandled type of elements '{0}' for data extraction'. Function:GetJsonData".format(ctl[0].tagName));
            }

            //将元素控件的值写入json对象
            json[dField] = dValue;
        }
    });

    //如果不能获取ID值，则填入临时ID。
    var obj_id = json["ID"];
    if (!obj_id) {
        json["ID"] = GetTempID();
    }

    //console.log(JSON.stringify(json));
    return json;
}

var SetJsonData = function (data, className, target) {
    if (target == null) {
        target = $("#hs-page-content");
    }
    else {
        target = $(target);
    }
	if(className=="RelationPersonAudits"){
		console.log();
	}
    //当数据源为null时，不执行直接返回false。
    if (data == null) {
        return false;
    }
    //数据源如果是数组，则往table里填。如果不是，则往表单里填。
    if (IsArray(data)) {

        var tbody = target.children("tbody");
        var rows = tbody.children("tr");
        var tempTable = tbody.closest("table");
        var radioName = tempTable.attr("id");

        if (rows.length === 0) {
            radioName += GetTempID();
        }
        else {
            radioName = rows.eq(0).children("td").children("input:radio").eq(0).attr("name");
        }

        for (var i = 0; i < data.length; i++) {
            var dItem = data[i];
            var id = dItem["ID"];

            //查找ID相同的row，如果能找到则修改，如果找不到则新增
            var row = null;
            tbody.find("td[data-field='ID']").each(function () {
                var existId = $(this).text();
                if (id == existId) {
                    row = $(this).closest("tr");
                }
            });

            var template = CreateRowFromJson(dItem, className);

            if (row == null) {
                tbody.append(template);
            }
            else {
                row.replaceWith(template);
            }

            SetJsonData(dItem, className, template);
        }

        //给“默认”单选框设定“name”属性
        var radioButtons = tempTable.children("tbody").children("tr").children("td").children("input:radio");
        radioButtons.each(function () {
            $(this).attr("name", radioName);
            if(location.href.indexOf("ReadonlyBaseCase")!=-1//进件只读
            	||location.href.indexOf("ReadonlyBaseAudit")!=-1//复审
            	||location.href.indexOf("ReadonlyRejectBaseAudit")!=-1//其他状态查看审核部分
            	||location.href.indexOf("EditMortgage")!=-1//签约页面
            	||location.href.indexOf("MortgageConfirmReadonly")!=-1//签约页面
            	||location.href.indexOf("RejectMortgageReadonly")!=-1//签约页面
            	||location.href.indexOf("CaseDetails")!=-1//签约页面
            ){
            	$(this).attr("disabled", true);
            }
        });
        if (radioButtons.length == 1) {
//          radioButtons.eq(0).click();
			radioButtons.eq(0).prop("checked",true);
        }
    }
    else {
        //遍历所有属性
        for (var key in data) {
            var value = data[key];

            //后台Dictionary转换null或空字符串时会报错
            if (key.endsWith("FileName")) {
                if (!value) { value = "{}"; }
            }

            if (value || (value == "")) {
                //转换日期格式
                if ((typeof (value) == "string") && value.IsDateStr()) {
                    var dateStr = value.substring(6, value.length - 2);
                    var dateObj = new Date(parseInt(dateStr));
                    value = dateObj.Format("yyyy-MM-dd");
                }

                var child_class = key; //父级的field等于子集的class
                var child_data = value; //子集的数据源为父级属性的值
                var field_node = target.find("[data-class='{0}'][data-field='{1}']".format(className, key));

                if (field_node.length == 0) {
                    //console.log("Warning: cannot find element where [data-class='{0}'][data-field='{1}'][target='{2}']. Function:SetJsonData".format(className, key, target[0].tagName));
                }
                else if (field_node.length > 1) {
                    //console.log("Warning: duplicate elements where [data-class='{0}'][data-field='{1}']. Function:SetJsonData".format(className, key));
                }
                else {
                    //如果值为数组，则递归调用本身
                    if (IsArray(value)) {
                        SetJsonData(value, key, field_node);
                    }
                    else if (IsObject(value) && field_node.is("[data-child]")) {
                        var fChild = field_node.attr("data-child");

                        SetJsonData(value, fChild, field_node);
                    }
                    else {
                        if (field_node.attr("data-type") == "files") {
                            //附件上传
                            var fileList = field_node.find(".fileupload_list");
                            var idList = value.split(",");
                            var nameList = data[key + "Name"];

                            var nameListObj = null;
                            if (nameList && (typeof (nameList) == "object")) {
                                nameListObj = nameList;
                            }
                            else if (nameList == null || nameList == '') {
                                nameListObj = {};
                            }
                            else {
                                nameListObj = $.parseJSON(nameList);
                            }

                            if (nameList == null) {
                                //                              console.log("Error: cannot find data filename of attachments where [data-class=''][data-field='']. Function:SetJsonData".format(className, key));
                            }

                            if (nameListObj == null) {
                                //                              console.log("Error: cannot find object filename of attachments where [data-class=''][data-field='']. Function:SetJsonData".format(className, key));
                            }

                            fileList.empty();
//                          console.log(nameListObj);
                            for (var i = 0; i < idList.length; i++) {
                                var id = idList[i];
                                var isDelete = 0;
                                if(location.href.indexOf("EditBaseCase")!=-1){
                                	isDelete = 0;
                                }else{
                                	var name_index = 0;
                                	for(var name in nameListObj){
                                		name_index++;
                                		break;
                                	}
                                	var num = 0;
                                	for(var obj in nameListObj){
                                		num++;
                                	}
//                              	console.log($("#"+id).attr("isdelete"));
                                	if(num!=0&&nameListObj != null&&name_index!=0&&nameListObj[id]&&nameListObj[id].indexOf("|")!=-1){
//                              		console.log(nameListObj[id].toString().substring(nameListObj[id].length-1,nameListObj[id].length));
                                		var isd = nameListObj[id].toString().substring(nameListObj[id].length-1,nameListObj[id].length);
                                		isDelete = Number(isd);
                                		if(isDelete==undefined){
                                			console.log(id);
                                			isDelete = 0;
                                		}
                                	}
                                }
                                if (nameListObj != null) {
                                	var name_id = "";
                                	if(nameListObj[id]&&nameListObj[id].indexOf("|")==-1){
                                		name_id = nameListObj[id];
                                	}else{
                                		name_id = nameListObj[id]==undefined?nameListObj[id]:nameListObj[id].substring(0,nameListObj[id].length-2);
                                	}
                                    var item = $("<li><a>{1}</a></li>".format(id, name_id));
                                    item.append("<button class='btn btn-minier btn-info' url='"+"/HelpPage/WebBrowser.aspx?fileid={0}&type=inline".format(id, name_id)+"'" +
                                   		" target='_blank' style='margin-left: 10px;' onclick='openFileWindow(this);'><i class='icon-eye-open'></i>预览</button>");
                                    item.append("<button class='btn btn-minier btn-success-green' onclick='window.open(\"" +
	            						"/HelpPage/WebBrowser.aspx?fileid={0}".format(id, name_id) + "\")'" +
	            						"style='margin-left: 10px;'><i class='icon-reply icon-only'>" +
	            						"</i>下载</button>");
	            					if(isDelete==0){
	            						if(location.href.indexOf("ReadonlyBaseCase")!=-1
	            							||location.href.indexOf("ReadonlyBaseAudit")!=-1
											||location.href.indexOf("ReadonlyRejectBaseAudit")!=-1
											||location.href.indexOf("ReadonlyLending")!=-1
	            							){
	            								
	            						}
//	            						console.log(id);
	            						if(nameListObj&&nameListObj[id]&&nameListObj[id].indexOf("|")!=-1){
	            							if($("#"+id).attr("isdelete")=="1"){
	            							}else{
	            								item.append("<button class='btn btn-minier btn-danger icon_danger' style='margin-left: 10px;' onclick='removeAttachment(this);'><i class='icon-trash'></i>删除</button>");
	            							}
	            							console.log(0);
	            						}else{
//	            							console.log(1);
	            							if(nameListObj&&nameListObj[id]){
	            								if($("#"+id).attr("isdelete")=="1"){
		            							}else{
		            								item.append("<button class='btn btn-minier btn-danger icon_danger' style='margin-left: 10px;' onclick='removeAttachment(this);'><i class='icon-trash'></i>删除</button>");
		            							}
	            							}
	            						}
	            					}
                                    item.attr("id", id);
                                    item.attr("complete", true);
                                    item.attr("isDelete", isDelete);
                                    fileList.append(item);
                                }
                            }

                            var label = field_node.prev();
                            if (label.is("label")) {
                                var length = 0;
                                if (idList[0]) { length = idList.length; }
                                label.children("span").text("({0})".format(length));
                            }
                        }
                        else if (field_node.is("textarea") || field_node.is("select") || field_node.is("input:text") || field_node.is("input:hidden")) {
                            if (field_node.is("input:text")) {
                                //金额和利率的数字格式转换
                                if (field_node.attr("onkeyup") != undefined) {
                                    if (field_node.attr("onkeyup").indexOf("formoney") != -1) {
                                        var attrs = field_node.attr("onkeyup");
                                        if (attrs.indexOf("100") != -1) {
                                            field_node.val(value);
                                            formoney(null, field_node, 2, 100);
                                            if (field_node.attr("action") == "onchange") {
                                                field_node.change();
                                            }
                                        } else {
                                            field_node.val(value);
                                            formoney(null, field_node, 2);
                                            if (field_node.attr("action") == "onchange") {
                                                field_node.change();
                                            }
                                        }
                                    } else {
                                        field_node.val(value);
                                        if (field_node.attr("action") == "onchange") {
                                            field_node.change();
                                        }
                                    }
                                } else {
                                    field_node.val(value);
                                    if (field_node.attr("action") == "onchange") {
                                        field_node.change();
                                    }
                                }
                                if(field_node.attr("data-rate") != undefined){
                                	if (field_node.attr("data-rate")== "rate") {
                                        formoney(null, field_node, 4, 100);
                                    }
                                }
                                if (field_node.attr("valid-type") != undefined && value == "9999-12-31") {
                                    field_node.attr("disabled", true);
                                    if ($("#isEdited").length == 0) {
                                        if ($("#CaseStatusRW").val() == "readonly") {
                                            field_node.siblings().find("[type='checkbox']").attr("checked", true).attr("disabled", true);
                                        } else {
                                            field_node.siblings().find("[type='checkbox']").attr("checked", true);
                                        }
                                    } else {
                                        if ($("#isEdited").val() != "readonly") {
                                            if ($(".tab-content").find(".active").attr("id") == "publicMortgage") {
                                                field_node.siblings().find("[type='checkbox']").attr("checked", true).attr("disabled", true);
                                            } else {
                                                field_node.siblings().find("[type='checkbox']").attr("checked", true);
                                            }
                                        } else {
                                            field_node.siblings().find("[type='checkbox']").attr("checked", true).attr("disabled", true);
                                        }
                                    }
                                } else {
                                    if ($("#isEdited").length == 0) {
//                                      if (window.location.pathname.indexOf("Mortgage/EditMortgage") != -1) {
//                                          if ($("#CaseStatusRW").val() == "readonly") {
//                                              field_node.attr("disabled", true);
//                                          } else {
//                                              field_node.removeAttr("disabled");
//                                          }
//                                      }
                                    } else {
                                        $("[type='file'],.file_btn>.btn").attr("disabled", true);//初始化
                                        if ($("#isEdited").val() != "readonly") {
                                            if ($(".tab-content").find(".active").attr("id") == "publicMortgage") {
                                                field_node.removeAttr("disabled");
                                            } else {
                                                field_node.attr("disabled", true);
                                            }
                                        } else {
                                            field_node.attr("disabled", true);
                                        }
                                    }
                                }
                            } else {
                                field_node.val(value);
                                if (field_node.attr("action") == "onchange") {
                                    field_node.change();
                                }
                            }
                        }
                        else if (field_node.is("td")) {
                            if (field_node.find("input:radio").length > 0) {
                                if (value.toString() === "true") {
                                    field_node.find("input:radio").click();
                                }
                                else {
                                    field_node.find("input:radio").removeAttr("checked");
                                }
                            }
                            else if (typeof (value) == "object") {
                                field_node.text(JSON.stringify(value));
                            }
                            else {
                                field_node.text(value);
                            }
                        }
                        else if (field_node.is("div")) {
                            field_node.text(value);
                        }
                        else if (field_node.is("input:radio")) {
                            if (value.toString() === "true") {
                                field_node.click();
                            }
                            else {
                                field_node.removeAttr("checked");
                            }
                        }
                        else {
                            //如果是非预期的元素，则不处理，并打印警告
                            //console.log("Warning: unhandled type of elements '{0}' for data extraction'. Function:SetJsonData".format(field_node[0].tagName));
                        }
                    }
                }
            }
            else {
                //console.log("Warning: the value of [data-class='{0}'][data-field='{1}'] is null or empty. Function:SetJsonData".format(className, key));
            }
        }
    }
//  if (location.href.indexOf("EditMortgage") != -1) {
//      $(document).find('[data-field="ExpiryDate"]').attr("disabled", true);
//      $(document).find('[data-field="ExpiryDate"]').siblings().find("#Long-term").attr("disabled", true);
//  }
    return true;
};

//通过json数据创建一条没有值的<tr>（但是包含[data-*]属性），返回此<tr>对象。
var CreateRowFromJson = function (data, className) {
    if (sys_mode == "debug") {
//      console.log("[{0}] CreateRowFromJson started, class: {1}, data is array: {2}, data: {3}".format(getDateTime(), className, IsArray(data), JSON.stringify(data)));
    }

    if (typeof (data) != "object") {
//      console.log("Warning: cannot support data type '{0}'. Function:CreateRowFromJson".format(typeof (data)));
        return false;
    }
    else {
        var row = $("<tr></tr>");
		var arr = [];
        for (var key in data) {
            var cell = $("<td></td>");
            var value = data[key];
            var cell_field = key;
            var cell_class = className;
            var cell_child = cell_field;

            //判断此单元格是否显示
            if (FIELD_DISPLAY[cell_class]) {
                if (FIELD_DISPLAY[cell_class][cell_field]) {
                    var isShow = FIELD_DISPLAY[cell_class][cell_field]["Visible"];
                    if (isShow != true) {
                        cell.addClass("hide");
                        //cell.attr("column-index", "999");
                    }
                    else {
                        cell.attr("column-index", FIELD_DISPLAY[cell_class][cell_field]["Index"]);
                        //console.log(cell_class + ", " + cell_field + ", " + FIELD_DISPLAY[cell_class][cell_field]["Index"]);
                    }
                }
                else {
                    cell.addClass("hide");
                    //cell.attr("column-index", "999");
                }
            }
            //判断值如果是Array，则往单元格中插入一个table
            if (IsArray(value)) {
                var child = CreateTableFromJson(value, cell_class, cell_field, cell_child);
                cell.append(child);
            }
            else {
                cell.attr("data-field", key);
                cell.attr("data-class", className);

                //当字段名称以“File”结尾时，认为是一个附件
                if (key.endWith("File")) {
                    cell.attr("data-type", "files");
                    cell.append("<ul class='fileupload_list'></ul>");
                }
                else if (key == "IsDefault") {
                    cell.append("<input type='radio' />");
                }
            }
            arr.push(cell);
        }
        //排序
        var tdList = arr;
        var len = arr.length;
        for (var i = len - 1; i >= 1; i--) {
            for (var j = i - 1; j >= 0; j--) {
                var index1 = tdList[i].attr("column-index");
                var index2 = tdList[j].attr("column-index");
                if (tdList[i].attr("column-index") < tdList[j].attr("column-index")) {
                    var temp1 = tdList[i];
                    var temp2 = tdList[j];
                    tdList[i] = temp2;
                    tdList[j] = temp1;
                }
            }
        }
        for (var i = 0; i < tdList.length; i++) {
            row.append(tdList[i]);
        }
        //插入操作按钮(待修改)
        var td = $("<td></td>").append("<div class='action-buttons'></div>");
        var container = td.find(".action-buttons");
        //判断只读项table
        var current_url = location.href;
        if(current_url.indexOf("ReadonlyBaseCase")!=-1){//进件只读页面判断
        	container.append("<a class='green' href='javascript:;' onclick='editRow(this);'><i class='icon-pencil bigger-130' tooltip='true' title='修改'></i></a>");
        }else if(current_url.indexOf("ReadonlyBaseAudit")!=-1
        	||current_url.indexOf("ReadonlyRejectBaseAudit")!=-1
        	||current_url.indexOf("EditMortgage")!=-1
        	||current_url.indexOf("MortgageConfirmReadonly")!=-1
        	||current_url.indexOf("RejectMortgageReadonly")!=-1
        	||current_url.indexOf("CaseDetails")!=-1
        	){
        	container.append("<a class='green' href='javascript:;' onclick='editRow(this);'><i class='icon-pencil bigger-130' tooltip='true' title='修改'></i></a>");
        }else if(current_url.indexOf("EditBaseCase")!=-1){
        	if (data.IsLocked) {
        		container.append("<a class='green' href='javascript:;' onclick='editRow(this);'><i class='icon-pencil bigger-130' tooltip='true' title='修改'></i></a>");
        		container.attr("disabled","disabled");        		
           	}else{
           		container.append("<a class='green' href='javascript:;' onclick='editRow(this);'><i class='icon-pencil bigger-130' tooltip='true' title='修改'></i></a>");
        		container.append("<a class='red' href='javascript:;' onclick='deleteRow(this);'><i class='icon-trash bigger-130' tooltip='true' title='删除'></i></a>");        
           	}
        }else{
        	container.append("<a class='green' href='javascript:;' onclick='editRow(this);'><i class='icon-pencil bigger-130' tooltip='true' title='修改'></i></a>");
        	container.append("<a class='red' href='javascript:;' onclick='deleteRow(this);'><i class='icon-trash bigger-130' tooltip='true' title='删除'></i></a>");
        }
        row.append(td);
    }

    SetJsonData(data, className, row);

    return row;
};

//通过arr_json数据创建一个没有值的<table>（但是包含[data-*]属性），返回此<table>对象。
var CreateTableFromJson = function (arr_data, className, fieldName, childName) {
    var table = $("<table id='SubTable-{0}-{1}-{2}'></table>".format(className, fieldName, childName));
    var tbody = $("<tbody></tbody>");

    if (!IsArray(arr_data)) {
//      console.log("Warning: cannot support data type '{0}'. Function:CreateTableFromJson".format(typeof (arr_data)));
        return false;
    }
    else {
        table.attr("data-field", fieldName);
        table.attr("data-class", className);
        table.attr("data-child", childName);

        for (var i = 0; i < arr_data.length; i++) {
            var row = CreateRowFromJson(arr_data[i], childName);
            tbody.append(row);
        }
    }

    table.append(tbody);

    //给“默认”单选框设定“name”属性
    var radioName = table.attr("id");
    var radioButtons = table.children("tbody").children("tr").children("td").children("input:radio");
    //radioButtons.each(function () {
    //    console.log("Hello: " + JSON.stringify(arr_data));
    //    $(this).attr("name", radioName);
    //});
    if (radioButtons.length == 1) {
        radioButtons.eq(0).click();
    }

    if (table.find("tbody>tr").length == 0) {
        return false;
    }
    else {
        return table;
    }
};

var addRow = function (dialog_id, className, action,disType, func) {
	$("#ui-datepicker-div").hide();
    bindReferenceList(dialog_id);
//  if (dialog_id == "dialog_person") {
//      $("#" + dialog_id).find('[data-field="ExpiryDate"]').removeAttr("disabled");
//  }
    
    if (location.href.indexOf("EditBaseCase")!=-1) {
    	$("#dialog_person").find("input").removeAttr("disabled");
    	$("#dialog_person").find("label").removeAttr("disabled");
    	$("#dialog_person").find("select").removeAttr("disabled");
    	$("#dialog_person").find("textarea").removeAttr("disabled");
    	$("#dialog_person").find(".new_add").removeClass("hide");
    	$("#ui-datepicker-div").removeClass("hide");
    	//联系方式
    	$("#dialog_contact").find("input").removeAttr("disabled");
    	$("#dialog_contact").find("select").removeAttr("disabled");
    	
    	//地址
    	$("#dialog_address").find("input").removeAttr("disabled");
    	$("#dialog_address").find("select").removeAttr("disabled");
    	//紧急联系人
    	$("#dialog_emergency_contact").find("input").removeAttr("disabled");
    	$("#dialog_emergency_contact").find("select").removeAttr("disabled");
    	//企业信息
    	$("#dialog_company").find("input").removeAttr("disabled");
    	$("#dialog_company").find("label").removeAttr("disabled");
    	//房产信息
    	$("#dialog_facility").find("input").removeAttr("disabled");
    	$("#dialog_facility").find("label").removeAttr("disabled");
    	$("#dialog_facility").find("select").removeAttr("disabled");
    	$("#dialog_facility").find("textarea").removeAttr("disabled");
    }
    OpenDialog(dialog_id, function () {
        SaveForm(dialog_id, className);

        if (func) {
            func();
        }
        else {
            AutoSave();
        }

        //showAlert("保存成功");
    },disType);
//  
    if (location.pathname.indexOf("EditAudit") != -1) {
        if (dialog_id == "dialog_facility") {
            $("#dialog_facility").find('[data-field="CollateralType"]').removeAttr("disabled");
            $("#dialog_facility").find('[data-field="HouseNumber"]').removeAttr("disabled");
            $("[value='-FacilityCategary-MainFacility']").hide();
        }
        if (dialog_id == "dialog_person") {
            $("#dialog_person")
	    		.find("#person-name,[data-field='RelationType'],[data-field='IdentificationType'],#person-docnumber")
	    		.removeAttr("disabled");
            if ($("#person-type").val() == "-PersonType-JieKuanRenPeiOu") {
                $("#person-type").attr("disabled", true);
            } else {
                $("#person-type").removeAttr("disabled");
            }
            var result = true;
            $("#basecase_person_list").find("tr").each(function () {
                $(this).find("td").each(function () {
                    if ($(this).html() == "借款人配偶") {
                        result = false;
                        return false;
                    }
                });
            });
            if (result) {
                $('[value="-PersonType-JieKuanRenPeiOu"]').show();
            } else {
                $('[value="-PersonType-JieKuanRenPeiOu"]').hide();
            }
            if(location.href.indexOf("ReadonlyBaseAudit")==-1){
            	$("#person-birthday").removeAttr("disabled");
            }
    		var resultP = true;
            $("#basecase_person_list").find("tr").each(function () {
                $(this).find("td").each(function () {
                    if ($(this).html() == "借款人") {
                        resultP = false;
                        return false;
                    }
                });
            });
            if (resultP) {
                $('[value="-PersonType-JieKuanRen"]').show();
            } else {
                $('[value="-PersonType-JieKuanRen"]').hide();
            }
        }
        $('[value="-PersonType-JieKuanRenPeiOu"]').hide();
    }
    if (location.pathname.indexOf("EditBaseCase") != -1) {
    	var result = true;
            $("#basecase_person_list").find("tr").each(function () {
                $(this).find("td").each(function () {
                    if ($(this).html() == "借款人") {
                        result = false;
                        return false;
                    }
                });
            });
            if (result) {
                $('[value="-PersonType-JieKuanRen"]').show();
            } else {
                $('[value="-PersonType-JieKuanRen"]').hide();
            }
           
    }
	if(dialog_id=="dialog_person"){
		//出生证明
		var birth_date = new Date($("#person-birthday").val());
		var birth_date_next = new Date((birth_date.getFullYear()+18).toString()+"-"+(birth_date.getMonth()+1)+"-"+birth_date.getDate());
		var now_date = new Date();
		if(now_date<birth_date_next){//不到18周岁
			$("#dialog_person").find('[data-field="BirthFile"]').attr("required",true);
			$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明 <span class='red'>*</span>");
		}else{
			$("#dialog_person").find('[data-field="BirthFile"]').removeAttr("required");
			$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明");
		}
//		//长期
//		if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
//			$("#dialog_person").find('[data-field="ExpiryDate"]').attr("disabled",true);
//		}else{
//			$("#dialog_person").find('[data-field="ExpiryDate"]').removeAttr("disabled");
//		}
		//担保方式，长期输入框，长期radio，婚姻证明，出生日期，出生证明
		$("#diyafangshi").removeAttr("required").siblings("label").html("担保方式 ");
		$("#dialog_person").find('[data-field="ExpiryDate"]').removeAttr("disabled").siblings("span").eq(1).children("#Long-term");
		$("#dialog_person").find('[data-field="MarryFile"]').removeAttr("required").siblings("label").eq(1).siblings("label").eq(0).html("婚姻证明");
		$("#dialog_person").find('[data-field="Birthday"]').removeAttr("disabled");
		$("#dialog_person").find('[data-field="BirthFile"]').removeAttr("required").siblings("label").eq(1).siblings("label").eq(0).html("出生证明");
		$("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).removeAttr("disabled");
		var str = $("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).children(".blue").html();
		$("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).html("点击上传附件 &nbsp;<span class='blue'>"+str+"</span>");
		$("#Long-term").removeAttr("disabled");
		$(".warning_font").hide();
	}
	if(dialog_id=="dialog_facility"){
		$("#dialog_facility").find('[data-field="HouseFile"]').siblings("label").eq(1).removeAttr("disabled").html('点击上传附件 &nbsp;<span class="blue">(0)</span>');
	}
};

var deleteRow = function (obj, func) {
    if (location.pathname.indexOf("EditAudit") != -1) {
        if ($(obj).closest("table").attr("id") == "basecase_facility_list") {
            if ($(obj).closest("tr").find("td").eq(6).html() == "抵押物") {
                showAlert("抵押物已被锁定，不能删除", "error");
                return;
            }
        }
        if ($(obj).closest("table").attr("id") == "basecase_person_list") {
            var result = true;
            var personType = "";
            $(obj).closest("tr").find("td").each(function () {
                if ($(this).html() == "借款人" || $(this).html() == "借款人配偶") {
                    result = false;
                    personType = $(this).html();
                    return false;
                }
            });
            if (!result) {
                showAlert(personType + "已被锁定，不能删除", "error");
                return;
            }
        }
    }
    var tr = $(obj).closest("tr");
    if(tr.children('[data-field="IsFrom"]').length==1&&tr.children('[data-field="IsFrom"]').html()=="1"){
    	showAlert("非本节点添加不能删除", "error");
        return;
    }
    ConfirmBox("删除后无法恢复，是否确定？", function () {
        var tempTable = $(obj).closest("table");

        $(obj).closest("tr").remove();

        //删除“默认”后，剩余第一个自动为默认
        if (tempTable.find("input:radio:checked").length == 0) {
            tempTable.find("input:radio").eq(0).click();
        }

        if (func) {
            func();
        }
        else {
            AutoSave();
        }

        showAlert("删除成功");
    });
};

var editRow = function (obj, func,disType) {
    obj = $(obj);
	$(".warning_font").hide();
    var table = obj.closest("table");
    var row = obj.closest("tr");
    var idCell = row.find("td[data-field='ID']");
    var className = idCell.attr("data-class");
    var dialog_id = table.attr("for");
	var disType = "";
	if(table.attr("disabled")!=undefined){
		disType = "disabled";
	}
	if(obj.closest("div").attr("disabled")=="disabled"){
		disType = "disabled";
	}
//	var isBorrower = false;
	row.prepend(row.children('[data-field="ID"]').clone());
	row.children('[data-field="ID"]').eq(1).remove();
    bindReferenceList(dialog_id);

    var data = GetJsonData(className, row);
//  // 关系人类型为担保人时，担保方式必填
        if (data.RelationType == "-PersonType-DanBaoRen") {
//          $("#houseId").val(tr.eq(i).find('[data-field="HouseNumber"]').html());
			$("#rel_type").html("*");
    		$("#diyafangshi").attr("required","required");
        }else{
        	$("#rel_type").html("");
    		$("#diyafangshi").removeAttr("required");
        }
 //婚姻证明是否必填 
    if (data.MaritalStatus=="-MaritalStatus-Married" || data.MaritalStatus == "-MaritalStatus-Divorced") {

    		$("#marry_status").html("*");
    		$("#mar_status").attr("required","required");
    		$("#mar_files").attr("required","required");
    	}else{
    		$("#marry_status").html("");
    		$("#mar_status").removeAttr("required");
    		$("#mar_files").removeAttr("required");
    	}   
    
    
    
    //  //产证编号
    var tr = $("#basecase_facility_list").find("tbody").find("tr");
    var arr = [];
    for (var i = 0; i < tr.length; i++) {
        if (tr.eq(i).find('[data-field="ID"]').html() == $(row).find('[data-field="CollateralID"]').html()) {
            $("#houseId").val(tr.eq(i).find('[data-field="HouseNumber"]').html());
        }
    }
    
    
    SetJsonData(data, className, $("#" + dialog_id));
    OpenDialog(dialog_id, function () {
    	$("#dialog_person").find(".fileupload_list").each(function(){
        	$(this).children("li").each(function(){
        		if($(this).attr("isdelete")==1){
	        		$(this).find("a").html($(this).find("a").html()+"|1");
	        	}else{
	        		$(this).find("a").html($(this).find("a").html()+"|0");
	        	}
        	})
        });
        
        
        
        
        var editedData = GetJsonData(className, $("#" + dialog_id));
        SaveForm(dialog_id, className);

        if (func) {
            func();
        }
        else {
            AutoSave();
        }

        //showAlert("保存成功");
    },disType);
    //  
    if (location.href.indexOf("EditBaseCase")!=-1) {
    	if (disType == "disabled") {
			//关系人
			$("#dialog_person").find("input").attr("disabled",true);
			$("#dialog_person").find("label").attr("disabled",true);
			$("#dialog_person").find("select").attr("disabled",true);
			$("#dialog_person").find("textarea").attr("disabled",true);
			$("#dialog_person").find(".new_add").addClass("hide");
			$("#ui-datepicker-div").addClass("hide");
			//联系方式
			$("#dialog_contact").find("input").attr("disabled",true);
			$("#dialog_contact").find("select").attr("disabled",true);
			
			//地址
			$("#dialog_address").find("input").attr("disabled",true);
			$("#dialog_address").find("select").attr("disabled",true);
			//紧急联系人
			$("#dialog_emergency_contact").find("input").attr("disabled",true);
			$("#dialog_emergency_contact").find("select").attr("disabled",true);
			//企业信息
			$("#dialog_company").find("input").attr("disabled",true);
			$("#dialog_company").find("label").attr("disabled",true);
			//房产信息
			$("#dialog_facility").find("input").attr("disabled",true);
			$("#dialog_facility").find("label").attr("disabled",true);
			$("#dialog_facility").find("select").attr("disabled",true);
			$("#dialog_facility").find("textarea").attr("disabled",true);
    	}else{
	    	//关系人
	    	$("#dialog_person").find("input").removeAttr("disabled");
	    	$("#dialog_person").find("label").removeAttr("disabled");
	    	$("#dialog_person").find("select").removeAttr("disabled");
	    	$("#dialog_person").find("textarea").removeAttr("disabled");
	    	$("#dialog_person").find(".new_add").removeClass("hide");
	    	$("#ui-datepicker-div").removeClass("hide");
	    	//联系方式
	    	$("#dialog_contact").find("input").removeAttr("disabled");
	    	$("#dialog_contact").find("select").removeAttr("disabled");
	    	
	    	//地址
	    	$("#dialog_address").find("input").removeAttr("disabled");
	    	$("#dialog_address").find("select").removeAttr("disabled");
	    	//紧急联系人
	    	$("#dialog_emergency_contact").find("input").removeAttr("disabled");
	    	$("#dialog_emergency_contact").find("select").removeAttr("disabled");
	    	//企业信息
	    	$("#dialog_company").find("input").removeAttr("disabled");
	    	$("#dialog_company").find("label").removeAttr("disabled");
			//房产信息
	    	$("#dialog_facility").find("input").removeAttr("disabled");
	    	$("#dialog_facility").find("label").removeAttr("disabled");
	    	$("#dialog_facility").find("select").removeAttr("disabled");
	    	$("#dialog_facility").find("textarea").removeAttr("disabled");
	    }
    }
    
    
    //关系人
    if(table.attr("id")=="basecase_person_list"||table.attr("id")=="basecase_facility_list"){
    	//长期
    	if(location.href.indexOf("EditAudit")!=-1){
    		if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
				$("#dialog_person").find('[data-field="ExpiryDate"]').attr("disabled",true);
			}else{
				$("#dialog_person").find('[data-field="ExpiryDate"]').removeAttr("disabled");
			}
    	}
    	if(row.children("td").eq(row.children("td").length-1).find(".icon-trash").length==1){//可编辑状态
    		if(row.find('[data-field="IsFrom"]')&&row.find('[data-field="IsFrom"]').html()=="1"){//isform判断
		        $("#dialog_person").find('[data-field="Name"]').attr("disabled", true);
		        $("#dialog_person").find('[data-field="RelationType"]').attr("disabled", true);
				$("#dialog_person").find('[data-field="IdentificationType"]').attr("disabled",true);
				$("#dialog_person").find('[data-field="IdentificationNumber"]').attr("disabled",true);
				$("#dialog_person").find('[data-field="ExpiryDate"]').attr("disabled", true);
				$("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).attr("disabled", true);
				$("#Long-term").attr("disabled", true);
				var num = $("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).children(".blue").html();
				$("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).html('查看附件 &nbsp;<span class="blue">'+num+'</span>');
				$("#dialog_facility").find('[data-field="CollateralType"]').attr("disabled",true);
				$("#dialog_facility").find('[data-field="HouseNumber"]').attr("disabled",true);
				var numHouse = $("#dialog_facility").find('[data-field="HouseFile"]').siblings("label").eq(1).children(".blue").html();
				$("#dialog_facility").find('[data-field="HouseFile"]').siblings("label").eq(1).attr("disabled",true).html('查看附件 &nbsp;<span class="blue">'+numHouse+'</span>');
		    	if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
		    		$("#Long-term").prop("checked",true).attr("disabled",true);
		    	}else{
		    		$("#Long-term").prop("checked",false).attr("disabled",true);
		    	}
    		}else{
		        $("#dialog_person").find('[data-field="Name"]').removeAttr("disabled");
		        $("#dialog_person").find('[data-field="RelationType"]').removeAttr("disabled");
				$("#dialog_person").find('[data-field="IdentificationType"]').removeAttr("disabled");
				$("#dialog_person").find('[data-field="IdentificationNumber"]').removeAttr("disabled");
				$("#dialog_person").find('[data-field="ExpiryDate"]').removeAttr("disabled");
				$("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).removeAttr("disabled");
				$("#Long-term").removeAttr("disabled");
				var num = $("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).children(".blue").html();
				$("#dialog_person").find('[data-field="IdentificationFile"]').siblings("label").eq(1).removeAttr("disabled").html('点击上传附件 &nbsp;<span class="blue">'+num+'	</span>');
				$("#dialog_facility").find('[data-field="CollateralType"]').removeAttr("disabled");
				$("#dialog_facility").find('[data-field="HouseNumber"]').removeAttr("disabled");
				var numHouse = $("#dialog_facility").find('[data-field="HouseFile"]').siblings("label").eq(1).children(".blue").html();
				$("#dialog_facility").find('[data-field="HouseFile"]').siblings("label").eq(1).removeAttr("disabled").html('点击上传附件 &nbsp;<span class="blue">'+numHouse+'</span>');
		    	if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
		    		$("#Long-term").prop("checked",true);
		    		$("#dialog_person").find('[data-field="ExpiryDate"]').attr("disabled",true);
		    	}else{
		    		$("#Long-term").prop("checked",false);
		    	}
		    	//担保方式
		    	if($("#dialog_person").find('[data-field="RelationType"]').val()=="-PersonType-DanBaoRen"){
		    		$("#diyafangshi").siblings("label").eq(0).html("担保方式 <span class='red'>*</span>");
					$("#diyafangshi").attr("required","required");
		    	}else{
		    		$("#diyafangshi").siblings("label").eq(0).html("担保方式");
					$("#diyafangshi").removeAttr("required");
		    	}
		    	//证件号
		    	if($("#person-doctype").val()=="-DocType-IDCard"){
		    		$("#person-birthday").attr("disabled",true);
		    	}else{
		    		$("#person-birthday").removeAttr("disabled");
		    	}
		    	
		    	
		    }
		    //长期
//		    if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
//		    	$("#dialog_person").find('[data-field="ExpiryDate"]').attr("disabled",true);
//		    	$("#Long-term").prop("checked",true).attr("disabled",true);
//		    }else{
//		    	$("#dialog_person").find('[data-field="ExpiryDate"]').removeAttr("disabled");
//		    	$("#Long-term").prop("checked",false).removeAttr("disabled");
//		    }
		    //出生证明
			var birth_date = new Date($("#person-birthday").val());
			var birth_date_next = new Date((birth_date.getFullYear()+18).toString()+"-"+(birth_date.getMonth()+1)+"-"+birth_date.getDate());
			var now_date = new Date();
			if(now_date<birth_date_next){//不到18周岁
				$("#dialog_person").find('[data-field="BirthFile"]').attr("required",true);
				$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明 <span class='red'>*</span>");
			}else{
				$("#dialog_person").find('[data-field="BirthFile"]').removeAttr("required");
				$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明");
			}
			//长期
	    	if(location.href.indexOf("EditAudit")==-1){
	    		if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
					$("#dialog_person").find('[data-field="ExpiryDate"]').attr("disabled",true);
				}else{
					$("#dialog_person").find('[data-field="ExpiryDate"]').removeAttr("disabled");
				}
	    	}
			
    	}else{// 不可编辑状态
    		if($("#dialog_person").find('[data-field="ExpiryDate"]').val()=="9999-12-31"){
		    		$("#Long-term").prop("checked",true);
		    	}else{
		    		$("#Long-term").prop("checked",false);
		    	}
    		
    	}
    }
    
    
    
    
    
};

var bindReferenceList = function (dialog_id) {
    if (dialog_id == "dialog_IndividualCredits") {
        var data = GetJsonData("BaseAudit", $("#basecase_person_list").parent());
        var list = data["RelationPersonAudits"];
        var ctl = $("#IndividualCredits_Person");

        ctl.empty();
        ctl.append("<option value=''></option>");

        var bData = GetJsonData("BaseAudit");
        if (bData["BorrowerPerson"]) {
            var b_id = bData["BorrowerPerson"]["ID"];
            var b_name = bData["BorrowerPerson"]["Name"];
            var b_type = "借款人"; //bData["BorrowerPerson"]["RelationTypeText"];
            var b_str = "{0}({1})".format(b_name, b_type);
            ctl.append("<option value='{0}'>{1}</option>".format(b_id, b_str));
        }

        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var item_id = item["ID"];
            var item_name = item["Name"];
            var item_type = item["RelationTypeText"];

            var str = "{0}({1})".format(item_name, item_type);
            ctl.append("<option value='{0}'>{1}</option>".format(item_id, str));
        }
    }
    else if (dialog_id == "dialog_EnterpriseCredits") {
        var data = GetJsonData("BaseAudit", $("#basecase_person_list").parent());
        var data1 = data["RelationPersonAudits"];

        var list = [];
        for (var i = 0; i < data1.length; i++) {
            var tmp = data1[i]["RelationEnterpriseAudits"];
            for (var j = 0; j < tmp.length; j++) {
                list.push(tmp[j]);
            }
        }

        var ctl = $("#EnterpriseCredits_Enterprise");

        ctl.empty();
        ctl.append("<option value=''></option>");

        var bData = GetJsonData("BorrowerPerson");
        if (bData["RelationEnterpriseAudits"]) {
            for (var i = 0; i < bData["RelationEnterpriseAudits"].length; i++) {
                var enterprise = bData["RelationEnterpriseAudits"][i];
                var e_id = enterprise["ID"];
                var e_name = enterprise["EnterpriseName"];
                ctl.append("<option value='{0}'>{1}</option>".format(e_id, e_name));
            }
        }

        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var item_id = item["ID"];
            var item_name = item["EnterpriseName"];

            ctl.append("<option value='{0}'>{1}</option>".format(item_id, item_name));
        }
    }
    else if (dialog_id == "dialog_EnforcementPersons") {
        var data = GetJsonData("BaseAudit", $("#basecase_person_list").parent());
        var list = data["RelationPersonAudits"];
        var ctl = $("#EnforcementPersons_Person");

        ctl.empty();
        ctl.append("<option value=''></option>");

        var bData = GetJsonData("BaseAudit");
        if (bData["BorrowerPerson"]) {
            var b_id = bData["BorrowerPerson"]["ID"];
            var b_name = bData["BorrowerPerson"]["Name"];
            var b_type = "借款人"; //bData["BorrowerPerson"]["RelationTypeText"];
            var b_str = "{0}({1})".format(b_name, b_type);
            ctl.append("<option value='{0}'>{1}</option>".format(b_id, b_str));
        }

        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var item_id = item["ID"];
            var item_name = item["Name"];
            var item_type = item["RelationTypeText"];

            var str = "{0}({1})".format(item_name, item_type);
            ctl.append("<option value='{0}'>{1}</option>".format(item_id, str));
        }
    }
    else if (dialog_id == "dialog_IndustryCommerceTaxs") {
        var data = GetJsonData("BaseAudit", $("#basecase_person_list").parent());
        var data1 = data["RelationPersonAudits"];

        var list = [];
        for (var i = 0; i < data1.length; i++) {
            var tmp = data1[i]["RelationEnterpriseAudits"];
            for (var j = 0; j < tmp.length; j++) {
                list.push(tmp[j]);
            }
        }

        var ctl = $("#IndustryCommerceTaxs_Enterprise");

        ctl.empty();
        ctl.append("<option value=''></option>");

        var bData = GetJsonData("BorrowerPerson");
        if (bData["RelationEnterpriseAudits"]) {
            for (var i = 0; i < bData["RelationEnterpriseAudits"].length; i++) {
                var enterprise = bData["RelationEnterpriseAudits"][i];
                var e_id = enterprise["ID"];
                var e_name = enterprise["EnterpriseName"];
                ctl.append("<option value='{0}'>{1}</option>".format(e_id, e_name));
            }
        }

        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var item_id = item["ID"];
            var item_name = item["EnterpriseName"];

            ctl.append("<option value='{0}'>{1}</option>".format(item_id, item_name));
        }
    }
    else if (dialog_id == "dialog_HouseDetails") {
        var data = GetJsonData("BaseAudit", $("#basecase_facility_list").parent());
        var list = data["CollateralAudits"];
        var ctl = $("#HouseDetails_House");

        ctl.empty();
        ctl.append("<option value=''></option>");
          //先遍历tr
      	var houseIdArr = [];
       	$("#audit_HouseDetails_list").children("tbody").children("tr").each(function () {
            $(this).children('[data-field="CollateralID"]').each(function () {
            	houseIdArr.push($(this).html());
            });
        });
//      console.log(houseIdArr);
		var houseIdStr = houseIdArr.join(",");
//      console.log(houseIdStr);
        
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var item_id = item["ID"];
            var item_name = item["BuildingName"];
            var item_text = item["Address"];
            var item_type = item["CollateralTypeText"];

            var str = "{0}-{1}({2})".format(item_name, item_text, item_type);
//          if(houseIdStr.indexOf(item_id)==-1){
            	ctl.append("<option value='{0}'>{1}</option>".format(item_id, str));
//          }
        }
    }

    return true;
};

var upload_files = [];
var fileUploadFunction = function (obj) {
    obj = $(obj);
    var disType = "";
    if(obj.attr("disabled")!=undefined){
    	disType = "disabled";
    	$("[name=file]").attr("disabled",true);
    	$("[name=file]").parent().attr("disabled",true);
    }else{
    	disType = "";
    	$("[name=file]").removeAttr("disabled");
    	$("[name=file]").parent().removeAttr("disabled");
    }
    
    var fileContainer = obj.next();
    var fileList = fileContainer.find(".fileupload_list");
    var dialogContainer = $("#file_upload_box_content");
    var dialogList = dialogContainer.find(".fileupload_list");
    var uploadWidget = dialogContainer.find(".file_btn>.btn");
    upload_files = [];//文件json初始化
	fileContainer.find(".btn-info").each(function(){
		var ob = {};
		ob.url = $(this).attr("url");
		ob.name = $(this).siblings("a").html();
		upload_files.push(ob);
//		$("#picture_iframe").attr("src",$(this).attr("url"));
	});
    dialogList.empty().append(fileList.html());

    var item = null;

    uploadWidget.fileupload({
        options: {
            maxFileSize: maxfileSize,
            dropZone: $(document),
        },
        url: '/Home/FileUpload',
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|doc|docx|xls|xlsx)$/i,
        add: function (e, data) {
            var index = $(data.fileInput.context.outerHTML).attr("number");
            var name = data.files[0].name;
            var type = name.split(".")[name.split(".").length - 1];
            type = type.toLowerCase();
            if (type != "gif" &&
	            type != "jpg" &&
	            type != "jpeg" &&
	            type != "png" &&
	            type != "pdf" &&
            	type != "doc" &&
            	type != "docx" &&
            	type != "xls" &&
            	type != "xlsx"
            ) {
                showAlert("请上传jpg、jpeg、png、gif、pdf等文件");
            } else {
                for (var i = 0; i < data.files.length; i++) {
                    var progressBar = '<div class="progress progress-striped active widninty" data-percent="0%" data-key="' + index + '" data-key-name="' + name + '">' +
	                "<div class='progress-bar progress-bar-success' style='width: 0%;' data-index='" + index + "' data-index-name='" + name + "'></div></div><button class='btn btn-minier btn-danger icon_danger cancel_btn'" +
	                "style='margin-left: 10px;' data-cancel='" + index + "' data-content='" + name + "'onclick='cancelAttachment(this);'><i class='icon-trash'></i>取消</button>";
                    item = $("<li data_li='" + index + "' data-name='" + name + "'><a href='javascript:return false;' target='_blank'>{0}</a>{1}</li>".format(data.files[i].name, progressBar));
                    dialogList.append(item);
                }
                data.submit();
            }

        },
        done: function (e, data) {
            var url = "/HelpPage/WebBrowser.aspx?fileid={0}&type=inline".format(data.result);
            var index = $(data.fileInput.context.outerHTML).attr("number");
            var name = data.files[0].name;
            var selector_one = "[data_li='" + index + "'][data-name='" + name + "']";
            var selector_two = "[data-cancel='" + index + "'][data-content='" + name + "']";
            $(selector_one).find("a").removeAttr("href").removeAttr("target");
            $(selector_one).find(".progress").removeClass("active");
            $(selector_one).find(".progress").addClass("hide");
            $(selector_one).attr("id", data.result);
            $(selector_one).attr("complete", true);
            $(selector_two).remove();
            $(selector_one).append("<button class='btn btn-minier btn-info' onclick='openFileWindow(this);' url='"+url.toString()+"' target='_blank' style='margin-left: 10px;'><i class='icon-eye-open'></i>预览</button>");
            $(selector_one).append("<button class='btn btn-minier btn-success-green' onclick='window.open(\"" + "/HelpPage/WebBrowser.aspx?fileid={0}".format(data.result).toString() + "\")'xiaza style='margin-left: 10px;'><i class='icon-reply icon-only'></i>下载</button>");
        	$(selector_one).append("<button class='btn btn-minier btn-danger icon_danger' style='margin-left: 10px;' onclick='removeAttachment(this);'><i class='icon-trash'></i>删除</button>");
            item = null;

            showAlert("上传完成");
        },
        progress: function (e, data) {
            var index = $(data.fileInput.context.outerHTML).attr("number");
            var name = data.files[0].name;
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $("[data-key='" + index + "'][data-key-name='" + name + "']").attr("data-percent", progress + "%");
            $("[data-index='" + index + "'][data-index-name='" + name + "']").css("width", progress + "%");
        }
    });

    OpenDialog("file_upload_box", function () {
        fileList.empty();
        fileList.append(dialogList.html());
        var count = fileList.find("li[complete]").length;

        obj.children("span").text("({0})".format(count));

        if (AutoSave) {
            AutoSave();
        }
    },disType,obj);
    
    if(disType=="disabled"){
    	$("#file_upload_box_content").find('[onclick="removeAttachment(this);"]').remove();
    }
};

function removeAttachment(obj) {
    ConfirmBox("删除后不可恢复，是否确认？", function () {
        obj = $(obj);

        var item = obj.parent();
        var id = item.attr("id");

        item.remove();
    });
}

function cancelAttachment(obj) {
    obj = $(obj);
    var item = obj.parent();
    var id = item.attr("id");
    item.remove();
}

function showLoading() {
    $("#hs-cover").removeClass("hide");
    //  //增加请求超时判断，现在
    //  setTimeout(function(){
    //  	if(!$("#hs-cover").hasClass("hide")){
    //  		hideLoading();
    //  		var url = location.href;
    //  		location.href="/Home/Failed?url="+url;
    //  	}
    //  },timeout);
}

function hideLoading() {
    $("#hs-cover").addClass("hide");
}

function AjaxMethod(type, path, params, func, show, color) {
    if (show) {
        if (color) {
            $("#hs-cover").css("background", "transparent");
            showLoading();
        } else {
            $("#hs-cover").css("background", "rgba(0,0,0,0.3)");
            showLoading();
        }
    }
    $.ajax({
        url: path,
        data: params,
        type: type,
        dataType: "json",
        timeout: timeout,
        success: function (r) {
            var iStatus = r["Status"];
            if (iStatus == "Failed") {
            	hideLoading();
                var str = "";
                $(r["Message"]).each(function () {
                    str += "<p>{0}</p>".format(this["Message"]);
                });
                $("#dialog-confirm-box p").html(str);
                if (str.indexOf("借款人配偶") != -1) {
                    if ($("#basecase_person_list").find("tr:last").find('[data-field="ID"]').html().indexOf("TEMP") != -1) {
                        //	            		$("#basecase_person_list").find("tr:last").remove();
                    }
                }
                if (str.indexOf("抵押物") != -1) {
                    if ($("#basecase_facility_list").find("tr:last").find('[data-field="ID"]').html().indexOf("TEMP") != -1) {
                        //	            		$("#basecase_facility_list").find("tr:last").remove();
                    }
                }
                $("#dialog-confirm-box").removeClass('hide').dialog({
                    resizable: false,
                    modal: true,
                    title: "<div class='widget-header'><h4 class='smaller'><i class='icon-warning-sign orange'></i> 错误 </h4></div>",
                    title_html: true,
                    buttons: [{
                        html: "<i class='icon-remove bigger-110'></i>&nbsp; 关闭",
                        "class": "btn btn-xs",
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                    ]
                });
            }
            else if (iStatus == "Success") {
                func(r);
            }
            else {
                var msg = "提交失败";
                if (r.Message != null && r.Message.length != 0) {
                    msg = "";
                    for (var i = 0; i < r.Message.length; i++) {
                        msg += " " + r.Message[i].Message;
                    }
                }
                hideLoading();
                MessageBox(msg, function () {
                    window.location = "/Home/Failed";
                }
	            );
            }
//          hideLoading();
        },
        error: function (r) {
            if (r.statusText && r.statusText != "timeout") {
                if (r.status == 200) {
                    MessageBox("登录超时，请重新登录", function () {
                        window.location = "/Home/Failed";
                    });
                } else {
                    MessageBox("请求超时", function () {
                        window.location = "/Home/Failed";
                    });
                }
                hideLoading();
            }
        },
        complete: function (XMLHttpRequest, status) { //请求完成后最终执行参数
            if (status == 'timeout') {//超时,status还有success,error等值的情况
                //				showAlert("请求超时,请稍后重试","error");
                hideLoading();
                MessageBox("请求超时", function () {
                    window.location = "/Home/Failed";
                });
            }
        }
    })
    //  $[type](path, params, function (r) {
    //
    //      var iStatus = r["Status"];
    //
    //      if (iStatus == "Failed") {
    //          var str = "";
    //
    //          $(r["Message"]).each(function () {
    //              str += "<p>{0}</p>".format(this["Message"]);
    //          });
    //
    //          $("#dialog-confirm-box p").html(str);
    //          $("#dialog-confirm-box").removeClass('hide').dialog({
    //              resizable: false,
    //              modal: true,
    //              title: "<div class='widget-header'><h4 class='smaller'><i class='icon-warning\n-sign orange'></i> 错误 </h4></div>",
    //              title_html: true,
    //              buttons: [{
    //                  html: "<i class='icon-remove bigger-110'></i>&nbsp; 关闭",
    //                  "class": "btn btn-xs",
    //                  click: function () {
    //                      $(this).dialog("close");
    //                  }
    //              }
    //              ]
    //          });
    //      }
    //      else if (iStatus == "Success") {
    //          func(r);
    //      }
    //      else {
    //          window.location = "/Home/Failed";
    //      }
    //
    //
    //      if (show) {
    //          hideLoading();
    //      }
    //  }, "json");
}

//绑定地区
function GetDistrict(show, func) {
    var ctl = $("#DistrictID");
    AjaxMethod("get", "/Sales/GetDistrict", {}, function (r) {
        for (var i = 0; i < r["Data"].length; i++) {
            ctl.append("<option value='" + r["Data"][i].ID + "'>" + r["Data"][i].Name + "</option>");
        }
        //执行回调函数
        if (func) {
            func();
        }
        hideLoading();
    }, show);
}

//地区变更绑定销售团队
function districtChange(show, func) {
    var districtId = $("#DistrictID").val();
    var ctl = $("#SalesGroupID");
    ctl.html("");
    ctl.append("<option value=''></option>");
    var ctlSales = $("#SalesID");
    ctlSales.html("");
    ctlSales.append("<option value=''>&nbsp;</option>");

    AjaxMethod("get", "/Sales/GetSalesGroup", { districtId: districtId }, function (r) {
        if (r["Data"].length > 0) {
            for (var i = 0; i < r["Data"].length; i++) {
                ctl.append("<option value='" + r["Data"][i].ID + "'>" + r["Data"][i].Name + "</option>");
            }
        }

        //执行回调函数
        if (func) {
            func();
        }
        hideLoading();
    }, show, "white");
}

//销售团队变更
function salesGroupChange(show, func) {
    var groupId = $("#SalesGroupID").val();
    var ctl = $("#SalesID");
    ctl.html("");
    ctl.append("<option value=''></option>");

    AjaxMethod("get", "/Sales/GetSales", { groupid: groupId }, function (r) {
        if (r["Data"].length > 0) {
            for (var i = 0; i < r["Data"].length; i++) {
                ctl.append("<option value='" + r["Data"][i].ID + "'>" + r["Data"][i].Name + "</option>");
            }
        }

        //执行回调函数
        if (func) {
            func();
        }
        hideLoading();
    }, show, "white");
}

function ConfirmBox(text, func_confirm) {
    $("#dialog-confirm-box p").html(text);
    $("#dialog-confirm-box").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4 class='smaller'><i class='icon-warning-sign orange'></i> 提示 </h4></div>",
        title_html: true,
        beforeClose: function () {
            reset_form("dialog-confirm-box");
        },
        buttons: [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp; 确定",
                "class": "btn btn-primary btn-xs",
                click: function () {
                    func_confirm();
                    $(this).dialog("close");
                }
            }
            ,
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp; 取消",
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function MessageBox(text, func_confirm) {
    $("#dialog-message-box p").text(text);
    $("#dialog-message-box").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4 class='smaller'><i class='icon-info-sign green'></i> 提示 </h4></div>",
        title_html: true,

        buttons: [
            {
                html: "<i class='icon-ok bigger-110'></i>&nbsp; 确定",
                "class": "btn btn-primary btn-xs",
                click: func_confirm
            }
        ]
    });
}

function filecheck(obj) {
    var fileSize = 0;
    if (!obj.files) {
        var filePath = obj.value;
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;
    } else {
        fileSize = obj.files[0].size;
    }
    var size = fileSize / 1024;
    if (size > maxfileSize / 1024) {
        showAlert("单个附件不能大于" + maxfileSize / 1024 / 1024 + "M");
        obj.value = "";
        return
    }
}

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};

//前台校验方法
var ValidateCheck = function (selector, obj) {
//	$(".warning_font").hide();
    $('[role="dialog"]').find(".ui-dialog-content").animate({ scrollTop: 0 }, 0);
    var isValid = true;
    var messages = [];
    var checkList = $(selector).find("[data-field]");
    //  /*0，合法性校验
    //    1，必填校验，合法性校验
    //    2，必填校验
    //  */
    //  var isCheck = $("#isSave").val();//0，保存  1，提交，回退，自动保存， 2，拒绝，关闭

    initHint(selector);

    checkList.each(function () {
        var required = $(this).is("[required]");
        var maxLength = $(this).attr("max-length");
        var minLength = $(this).attr("min-length");
        var max = $(this).attr("max");
        var min = $(this).attr("min");
        var validType = $(this).attr("valid-type");
        var maxDate = $(this).attr("max-date");
        var range = $(this).attr("range");

        //必填
        if (required) {
        	if(!$(this).is("table")){
        		var hasValue;
	            if ($(this).attr("data-type") == "files") {
	                hasValue = $(this).find("ul.fileupload_list>li[complete]").length > 0;
	            }
	            else {
	                hasValue = $(this).val();
	            }
	            isValid = isValid && hasValue;
	            var msg = "此字段为必填，不能为空";
	            if (!hasValue) {
	                messages.push({
	                    "target": $(this),
	                    "msg": msg
	                });
	                passMessage(messages);
	                return true;
	            }
        	}
        }
        //table
        if (required) {
        	if($(this).is("table")){
				if($(this).find("tr").length==1){
					var msg = "至少添加一项，不能为空";
//					console.log($(this).find("tbody").html());
					isValid = false;
	                messages.push({
	                    "target": $(this),
	                    "msg": msg
	                });
	                passMessage(messages);
	                return true;
				}
        	}
        }
        //固定类型校验
        if (validType == "idcode") {
            var reg = /^\d{17}([0-9]|X)$/; //18位身份证号码
            var msg = "身份证号码格式不正确";

            if ($(this).val() && !validateIdCard($(this).val(), false)) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        else if (validType == "date") {
            var reg = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
            var msg = "日期格式不正确（正确格式为：yyyy-mm-dd）";

            if ($(this).val() && !reg.test($(this).val())) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        else if (validType == "float") {
            var msg = "必须为数字";
            var ctlValue = $(this).val();

            if ($(this).val() && isNaN(parseFloat(ctlValue))) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        else if (validType == "int") {
            var msg = "必须为整数";
            var ctlValue = $(this).val();

            if ($(this).val() && (parseInt(ctlValue).toString() != ctlValue.toString())) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }

        //数值范围
        if (range) {
            var inputvalue = parseFloat($(this).val());
            var rangeMin = range.split(":")[0];
            var rangeMax = range.split(":")[1];
            var rangeValid = (inputvalue <= rangeMax) && (inputvalue >= rangeMin);
            var msg = "取值范围为{0}到{1}".format(rangeMin, rangeMax);
            isValid = isValid && rangeValid;

            if (!rangeValid) {
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }

        //最大值
        if (max) {
            var maxValid = parseFloat(rmoney($(this).val())) <= parseFloat(max);
            isValid = isValid && maxValid;
            var msg = "不能大于{0}".format(max);

            if (!maxValid) {
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }

        //最小值
        if (min) {
            var minValid = parseFloat(rmoney($(this).val())) >= parseFloat(min);
            isValid = isValid && minValid;
            var msg = "不能小于{0}".format(min);

            if (!minValid) {
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        //max-date
        if (maxDate == "now" && $(this).val() != "") {
            var valDate = new Date($(this).val());
            var now = new Date();
            var msg = "出生日期必须在今天之前"
            if (now < valDate) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
    });
    //  for (var i = 0; i < messages.length; i++) {
    //      var message = messages[i];
    //      var control = message["target"];
    //      var msg = message["msg"];
    //      var isInputGroup = control.parent().is(".input-group");
    //		hint(control,msg,isInputGroup)
    //  }
    return isValid;
};
//前台保存校验方法
var ValidateCheckSave = function (selector, obj) {
//	$(".warning_font").hide();
    $('[role="dialog"]').find(".ui-dialog-content").animate({ scrollTop: 0 }, 0);
    var isValid = true;
    var messages = [];
    var checkList = $(selector).find("[data-field]");
    initHint(selector);

    checkList.each(function () {
        var required = $(this).is("[required]");
        var maxLength = $(this).attr("max-length");
        var minLength = $(this).attr("min-length");
        var max = $(this).attr("max");
        var min = $(this).attr("min");
        var validType = $(this).attr("valid-type");
        var maxDate = $(this).attr("max-date");
        var range = $(this).attr("range");

        //固定类型校验
        if ($(this).val() != "" && validType == "idcode") {
            var reg = /^\d{17}([0-9]|X)$/; //18位身份证号码
            var msg = "身份证号码格式不正确";

            if ($(this).val() && !validateIdCard($(this).val(), false)) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        else if ($(this).val() != "" && validType == "date") {
            var reg = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
            var msg = "日期格式不正确（正确格式为：yyyy-mm-dd）";

            if ($(this).val() && !reg.test($(this).val())) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        else if ($(this).val() != "" && validType == "float") {
            var msg = "必须为数字";
            var ctlValue = $(this).val();

            if ($(this).val() && isNaN(parseFloat(ctlValue))) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        else if ($(this).val() != "" && validType == "int") {
            var msg = "必须为整数";
            var ctlValue = $(this).val();

            if ($(this).val() && (parseInt(ctlValue).toString() != ctlValue.toString())) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }

        //数值范围
        if ($(this).val() != "" && range) {
            var inputvalue = parseFloat($(this).val());
            var rangeMin = range.split(":")[0];
            var rangeMax = range.split(":")[1];
            var rangeValid = (inputvalue <= rangeMax) && (inputvalue >= rangeMin);
            var msg = "取值范围为{0}到{1}".format(rangeMin, rangeMax);
            isValid = isValid && rangeValid;

            if (!rangeValid) {
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }

        //最大值
        if ($(this).val() != "" && max) {
            var maxValid = parseFloat(rmoney($(this).val())) <= parseFloat(max);
            isValid = isValid && maxValid;
            var msg = "不能大于{0}".format(max);

            if (!maxValid) {
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }

        //最小值
        if ($(this).val() != "" && min) {
            var minValid = parseFloat(rmoney($(this).val())) >= parseFloat(min);
            isValid = isValid && minValid;
            var msg = "不能小于{0}".format(min);

            if (!minValid) {
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
        //max-date
        if (maxDate == "now" && $(this).val() != "") {
            var valDate = new Date($(this).val());
            var now = new Date();
            var msg = "出生日期必须在今天之前"
            if (now < valDate) {
                isValid = false;
                messages.push({
                    "target": $(this),
                    "msg": msg
                });
                passMessage(messages);
                return true;
            }
        }
    });
    //  for (var i = 0; i < messages.length; i++) {
    //      var message = messages[i];
    //      var control = message["target"];
    //      var msg = message["msg"];
    //      var isInputGroup = control.parent().is(".input-group");
    //		hint(control,msg,isInputGroup)
    //  }
    return isValid;
};
function passMessage(messages) {
    for (var i = 0; i < messages.length; i++) {
        var message = messages[i];
        var control = message["target"];
        var msg = message["msg"];
        var isInputGroup = control.parent().is(".input-group");
        if(control.is("table")){
        	hint("#"+control.attr("id"), msg, isInputGroup)
        }else{
        	hint(control, msg, isInputGroup)
        }
        
    }
}
var IDType_Changed = function (ctl, code_id, birthday_id) {
    if ($("#CaseStatusRW").val() == "readonly") {
        if ($("#isEdited").length == 1) {
            if ($("#isEdited").val() == "readonly") {
                $('.date-picker').attr("disabled", "disabled");
            } else {
                if ($(".tab-content").find(".active").attr("id") != "publicMortgage") {
                    $("#basic").find('.date-picker').attr("disabled", "disabled");
                    $("#borrower").find('.date-picker').attr("disabled", "disabled");
                    $("#facility").find('.date-picker').attr("disabled", "disabled");
                }
            }
        } else {
            $('.date-picker').attr("disabled", "disabled");
        }
    } else {
        if ($(ctl).val() == "-DocType-IDCard") {
            $("#" + birthday_id).attr("disabled", true);
            $("#" + code_id).attr("valid-type", "idcode").blur(function () {
                CalcBirthDay(code_id, birthday_id);
            }).blur();
        }
        else {
            $("#" + birthday_id).attr("disabled", false);
            $("#" + code_id).unbind("blur").removeAttr("valid-type");
        }
    }
};

var CalcBirthDay = function (code_id, birthday_id) {
    var code = $("#" + code_id).val();
    if (code.length == 18) {
        var year = code.substr(6, 4);
        var month = code.substr(10, 2);
        var day = code.substr(12, 2);

        $("#" + birthday_id).val("{0}-{1}-{2}".format(year, month, day));
        var birth_date = new Date($("#" + birthday_id).val());
		var birth_date_next = new Date((birth_date.getFullYear()+18).toString()+"-"+(birth_date.getMonth()+1)+"-"+birth_date.getDate());
		var now_date = new Date();
		if(now_date<birth_date_next){//不到18周岁
			$("#dialog_person").find('[data-field="BirthFile"]').attr("required",true);
			$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明 <span class='red'>*</span>");
		}else{
			$("#dialog_person").find('[data-field="BirthFile"]').removeAttr("required");
			$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明");
		}
    }
    else {
        $("#" + birthday_id).val("");
        $("#dialog_person").find('[data-field="BirthFile"]').removeAttr("required");
		$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明");
    }
};

function readonly() {
    if ($("#CaseStatusRW").val() == "readonly") {
        $(".form-control,.new_add,.green,.red,.file_btn>.btn,.fileupload,.ui-button-text,.date-picker,#Long-term").attr("disabled", true);
    }
}

//提示
function hint(selector, str, isInputGroup) {
    var content = '<div class="tooltip fade top in hint_body">' +
				'<div class="tooltip-arrow hint_arrow"></div>' +
				'<div class="tooltip-inner hint_inner" style="background-color: #dd5a43 !important;">' + str + '</div>' +
			'</div>';
	if($(selector).is("table")){
	  	$(selector).parent().addClass("border-red");
	  	$('[data-in="'+selector.replace("#","")+'"]').show();
	}else{
	 	if (isInputGroup) {
	        $(selector).addClass("border-red").parent().after(content);
	    } else {
	        $(selector).addClass("border-red").after(content);
	    }
	}
    var id = $(selector).parent().parent().parent().attr("id");
    if (id == undefined) {
        id = $(selector).parent().parent().parent().parent().attr("id");
        if ($("#checkflag").length == 0) {
            $("body").append("<input type='hidden' id='checkflag' value='" + id + "'/>");
            if ($(selector).parent().parent().parent().parent().hasClass("tab-pane")) {
                $("[href='#" + id + "']").click();
            }
        } else {
            if ($("#checkflag").val() == "") {
                $("#checkflag").val(id);
                if ($(selector).parent().parent().parent().parent().hasClass("tab-pane")) {
                    $("[href='#" + id + "']").click();
                }
            }
        }
    } else {
        if ($("#checkflag").length == 0) {
            $("body").append("<input type='hidden' id='checkflag' value='" + id + "'/>");
            if ($(selector).parent().parent().parent().hasClass("tab-pane")) {
                $("[href='#" + id + "']").click();
            }
        } else {
            if ($("#checkflag").val() == "") {
                $("#checkflag").val(id);
                if ($(selector).parent().parent().parent().hasClass("tab-pane")) {
                    $("[href='#" + id + "']").click();
                }
            }
        }
    }
}
//提示初始化
function initHint(selector) {
    $("#checkflag").val("");
    $(selector).find(".border-red").removeClass("border-red");
    $(selector).find(".hint_body").remove();
}

var validateIdCard = function (id, backInfo) {
    var info = {
        y: "1900",
        m: "01",
        d: "01",
        sex: "male",
        valid: false,
        length: 0
    },
    initDate = function (length) {
        info.length = length;
        var a = length === 15 ? 0 : 2,  // 15:18
          temp;
        info.y = (a ? "" : "19") + id.substring(6, 8 + a);
        info.m = id.substring(8 + a, 10 + a);
        info.d = id.substring(10 + a, 12 + a);
        info.sex = id.substring(14, 15 + a) % 2 === 0 ? "female" : "male";
        temp = new Date(info.y, info.m - 1, info.d);
        return (temp.getFullYear() == info.y * 1)
         && (temp.getMonth() + 1 == info.m * 1)
         && (temp.getDate() == info.d * 1);
    },
    back = function () {
        return backInfo ? info : info.valid;
    };
    if (typeof id !== "string") return back();
    // 18
    if (/^\d{17}[0-9x]$/i.test(id)) {
        if (!initDate(18)) return back();
        id = id.toLowerCase().split("");
        var wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2],
          y = "10x98765432".split(""),
          sum = 0;
        for (var i = 0; i < 17; i++) sum += wi[i] * id[i];
        if (y[sum % 11] === id.pop().toLowerCase()) info.valid = true;
        return back();
    }
        // 15
    else if (/^\d{15}$/.test(id)) {
        if (initDate(15)) info.valid = true;
        return back();
    }
    else {
        return back();
    }
};

function checkNavugator() {
    var nav = navigator.userAgent;
    if (nav.indexOf("Chrome") == -1 && nav.indexOf("Firefox") == -1 && nav.indexOf("Safari") == -1) {
        alert("本系统暂时只支持谷歌、火狐以及safari浏览器\n对其他浏览器用户使用造成不便敬请谅解！");
        window.location = "/Home/Error";
    }
};

checkNavugator();

function dateFormat(time) {
    if (time == null) {
        return "";
    } else {
        var date = new Date(Number(time.replace("/Date(", "").replace(")/", "")));
        var minute = "";
        if (date.getMinutes().toString().length == 1) {
            minute = "0" + date.getMinutes();
        } else {
            minute = date.getMinutes().toString();
        }
        var second = date.getSeconds().toString();
        if (second.length == 1) {
            second = "0" + second;
        }
        return date.getFullYear() + "/"
			+ (date.getMonth() + 1) + "/"
			+ date.getDate() + " "
			+ date.getHours() + ":"
			+ minute + ":"
			+ second;
    }
}

function closeAll() {
    setTimeout(closeWin, 3000);
}

function closeWin() {
    window.close();
}

function tohead() {
    var obj = {
        PageIndex: 1,
        PageSize: Number($("#PageSize").val()),
        BorrowerName: $("#txtBrrownName").val(),
        CaseNum: $("#CaseNum").val(),
        CaseStatus: $("#drpCaseStatus").val()
    };
    query(obj);
}

function tofoot() {
    var obj = {
        PageIndex: Number($("#TotalPage").html()),
        PageSize: Number($("#PageSize").val()),
        BorrowerName: $("#txtBrrownName").val(),
        CaseNum: $("#CaseNum").val(),
        CaseStatus: $("#drpCaseStatus").val()
    };
    query(obj);
}

function fmoney(s, n) {
    var xs = true;
    if (s.toString().indexOf(".") != -1) {//是否有小数点
        xs = false;
    }
    if (!n) {
        n = 2;
    }
    n = n > 0 && n <= 20 ? n : 2;
    var l = s.split(".")[0].split("").reverse();
    if (s.split(".")[1] != null) {
		r = s.split(".")[1].substring(0, n);    	
    }
    t = "";
    for (i = 0; i < l.length; i++) {
        t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");
    }
    if (!xs) {
        return t.split("").reverse().join("") + "." + r;
    } else {
        return t.split("").reverse().join("");
    }
}

function formoney(e, obj, point, max_val) {
    $(obj).val($(obj).val().replace(/[^0-9\.]/g, ''));// = obj.value.replace(/,/g,'');
    if ($(obj).val() == "") {
        //		obj.value = 0;
        //		fmoney(obj.value);
    } else {
        if (isNaN(Number($(obj).val()))) {
            $(obj).val($(obj).val().substring(0, $(obj).val().length - 1));
        } else {
            if ($(obj).val().indexOf(".") != -1) {
                $(obj).val($(obj).val().split(".")[0] + "." + $(obj).val().split(".")[1].substring(0, point));
            }
        }
    }
    if (max_val) {
        if (Number($(obj).val()) > max_val) {
            $(obj).val($(obj).val().substring(0, $(obj).val().length - 1));
        }
    }
    if (fmoney($(obj).val(), point).toString() == "NaN") {
        $(obj).val("");
    } else {
        $(obj).val(fmoney($(obj).val(), point));
    }
}

function rmoney(s) {
    return parseFloat(s.replace(/[^\d\.-]/g, ""));
}
function checkSelectNull(obj){
	if(obj.HouseDetails&&obj.HouseDetails.length>0){
    	for(var i=0;i<obj.HouseDetails.length;i++){
    		if(obj.HouseDetails[i].IsDamage == ""){
    			obj.HouseDetails[i].IsDamage = null;
    		}
    		if(obj.HouseDetails[i].Man2Wei1 == ""){
    			obj.HouseDetails[i].Man2Wei1 = null;
    		}
    		if(obj.HouseDetails[i].SpecialResident == ""){
    			obj.HouseDetails[i].SpecialResident = null;
    		}
    	}
    }
	return obj;
}
function forIframe(width,height){
	$("#preview_width").val(width);
	$("#preview_height").val(height);
	var clientHeight = document.documentElement.clientHeight;
	var clientWidth = document.documentElement.clientWidth;
//	$("#picture_iframe").css("max-height",clientHeight*0.8);//最大高度浏览器可视部分80%
	if($("#preview_type").val()!="pdf"){
		if(clientHeight*0.8>=height){
//			$("#picture_iframe").css("height",height+7);
			$("#picture_iframe").css("top",(clientHeight-height)/2);
		}else{
//			$("#picture_iframe").css("height",clientHeight*0.8);
			$("#picture_iframe").css("top",clientHeight/10);
		}
		$("#picture_iframe").css("max-width",clientWidth*0.8);//最大宽度浏览器可视部分80%
		if(clientWidth*0.8>=width){
			$("#picture_iframe").css("width",width+7);
//			$("#picture_iframe").css("margin-left","-"+width/2+"px");
		}else{
			$("#picture_iframe").css("width",clientWidth*0.8);
//			$("#picture_iframe").css("margin-left","-"+clientWidth*0.8/2+"px");
		}
	}
}
function watch(obj){
	var onclick_next = $("#next_picture").attr("onclick");
	$("#next_picture").attr("onclick","").css("cursor","not-allowed")
	setTimeout(function(){$("#next_picture").attr("onclick",onclick_next);$("#next_picture").css("cursor","pointer");},3000);
	var onclick_preview = $("#preview_picture").attr("onclick");
	$("#preview_picture").attr("onclick","").css("cursor","not-allowed")
	setTimeout(function(){$("#preview_picture").attr("onclick",onclick_preview);$("#preview_picture").css("cursor","pointer");},3000);
	var clientHeight = document.documentElement.clientHeight;
	var clientWidth = document.documentElement.clientWidth;
	var type = $(obj).attr("id");
	var now_preivew = $("#picture_iframe").attr("src");
	var preview_index = -1;
	for(var i=0;i<upload_files.length;i++){
		if(upload_files[i].url == now_preivew){
			preview_index = i;
		}
	}
	if(upload_files.length>1){
		if(type=="preview_picture"){
			if(preview_index==0){
				if(upload_files[upload_files.length-1].name.indexOf('pdf')!=-1){
//			 		$("#picture_iframe").css("height",600);
//					$("#picture_iframe").css("width",900);
					$("#picture_iframe").css("top",(clientHeight-600)/2);
//					$("#picture_iframe").css("margin-left","-"+(clientWidth-900)/2+"px");
			 	}else{
//			 		$("#picture_iframe").css($("#preview_width").val());
//					$("#picture_iframe").css($("#preview_height").val());
			 	}
			 	$("#picture_iframe").attr("src",upload_files[upload_files.length-1].url);
			 	$("#picture_name").html(upload_files[upload_files.length-1].name);
			}else{
				if(upload_files[preview_index-1].name.indexOf('pdf')!=-1){
//			 		$("#picture_iframe").css("height",600);
//					$("#picture_iframe").css("width",900);
					$("#picture_iframe").css("top",(clientHeight-600)/2);
//					$("#picture_iframe").css("margin-left","-"+(clientWidth-900)/2+"px");
			 	}
				else{
//			 		$("#picture_iframe").css($("#preview_width").val());
//					$("#picture_iframe").css($("#preview_height").val());
			 	}
				$("#picture_iframe").attr("src",upload_files[preview_index-1].url);
				$("#picture_name").html(upload_files[preview_index-1].name);
			}
			
		}
		if(type=="next_picture"){
			if(preview_index==(upload_files.length-1)){
				if(upload_files[0].name.indexOf('pdf')!=-1){
//			 		$("#picture_iframe").css("height",600);
//					$("#picture_iframe").css("width",900);
					$("#picture_iframe").css("top",(clientHeight-600)/2);
//					$("#picture_iframe").css("margin-left","-"+(clientWidth-900)/2+"px");
			 	}
				else{
//			 		$("#picture_iframe").css($("#preview_width").val());
//					$("#picture_iframe").css($("#preview_height").val());
			 	}
				$("#picture_iframe").attr("src",upload_files[0].url);
				$("#picture_name").html(upload_files[0].name);
			}else{
				if(upload_files[preview_index+1].name.indexOf('pdf')!=-1){
//			 		$("#picture_iframe").css("height",600);
//					$("#picture_iframe").css("width",900);
					$("#picture_iframe").css("top",(clientHeight-600)/2);
//					$("#picture_iframe").css("margin-left","-"+(clientWidth-900)/2+"px");
			 	}
				else{
//			 		$("#picture_iframe").css($("#preview_width").val());
//					$("#picture_iframe").css($("#preview_height").val());
			 	}
				$("#picture_iframe").attr("src",upload_files[preview_index+1].url);
				$("#picture_name").html(upload_files[preview_index+1].name);
			}
		}
		
	}
	
}
function openFileWindow(obj){
//	console.log(obj);
	$("#picture_iframe").css("height","100%");
	$("#picture_iframe").css("width","100%");
	upload_files = [];
	$(obj).siblings("a").eq(0)
	var ul_length = $(obj).closest("ul");
	if(ul_length.length>0){
		for(var i =0;i<ul_length.children("li").length;i++){
			var li = ul_length.children("li").eq(i);
//			if(li.children("a").eq(0).html().indexOf("doc")==-1
//				&&li.children("a").eq(0).html().indexOf("docx")==-1
//				&&li.children("a").eq(0).html().indexOf("xls")==-1
//				&&li.children("a").eq(0).html().indexOf("xlsx")==-1
//			){
				var ob = {
					name:li.children("a").eq(0).html(),
					url:li.children("button").eq(0).attr("url")
				}
				upload_files.push(ob);
//			}
		}
	}
	var suffix = $(obj).siblings("a").html().split(".")[$(obj).siblings("a").html().split(".").length-1];
	var clientHeight = document.documentElement.clientHeight;
	var clientWidth = document.documentElement.clientWidth;
	if(suffix=="pdf"){
//		$("#picture_iframe").css("height",600);
//		$("#picture_iframe").css("width",900);
		$("#picture_iframe").css("top",(clientHeight-600)/2);
//		$("#picture_iframe").css("margin-left","-"+(clientWidth-900)/2+"px");
	}
	$("#preview").css("top",clientHeight/2-30);
	$("#next").css("top",clientHeight/2-30);
	$("#preview_type").val(suffix);
	$("#picture_iframe").attr("src",$(obj).attr("url"));
	$("#picture_name").html($(obj).siblings("a").html());
	$("#picture_preview").show();
}
function close_preview(){
//	files = [];
	$("#picture_preview").hide();
}

function changeAccount(obj){
	$(obj).siblings('[data-field="OpeningSite"]').val($(obj).val());
}
//账户名称
function selAccount(){
	var arr = [];
	var id = $("#basic").find('[data-field="OpeningSite"]').val();
	if (id == null) {
	    var id = $("#publicMortgage").find('[data-field="OpeningSite"]').val();
	}
	$("#basecase_person_list").children("tbody").children("tr").each(function(){
	    if ($(this).find('[data-field="RelationTypeText"]').html() == "借款人"
			||$(this).find('[data-field="IsCoBorrower"]').html()=="1"
		){
			var obj = {
				id:$(this).find('[data-field="RelationTypeText"]').siblings('[data-field="IdentificationNumber"]').html(),
				name:$(this).find('[data-field="RelationTypeText"]').siblings('[data-field="Name"]').html(),
				type:$(this).find('[data-field="RelationTypeText"]').html()
			}
			arr.push(obj);
		}
	});
	var options = "<option> </option>";
	if(arr.length>0){
		for(var i=0;i<arr.length;i++){
			options = options + "<option value='"+arr[i].id+"'>"+arr[i].name+"("+arr[i].type+")</option>";
		}
	}
	$("#OpeningSite").html(options);
	$("#OpeningSite").val(id);
//		$("#basic").find('[data-field="OpeningSite"]').html(options);
//		$("#basic").find('[data-field="OpeningSite"]').val(id);

}

function rate(){
	$('[data-rate="rate"]').keyup(function(e){
//		$(this).val(formoney(e, $(this), 4, 100));
		formoney(e, $(this), 4, 99.9999)
	});
}

function birithday(obj){
	var birth_date = new Date($(obj).val());
	var birth_date_next = new Date((birth_date.getFullYear()+18).toString()+"-"+(birth_date.getMonth()+1)+"-"+birth_date.getDate());
	var now_date = new Date();
	if(now_date<birth_date_next){//不到18周岁
		$("#dialog_person").find('[data-field="BirthFile"]').attr("required",true);
		$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明 <span class='red'>*</span>");
	}else{
		$("#dialog_person").find('[data-field="BirthFile"]').removeAttr("required");
		$("#dialog_person").find('[data-field="BirthFile"]').siblings("label").eq(0).html("出生证明");
	}
}
function guanxiren(obj) {
    var relationType = $(obj).val();
//     -PersonType-DanBaoRen
	if ( relationType == "-PersonType-DanBaoRen") {
//  		$("#rel_type").html("*");
		$("#diyafangshi").siblings("label").eq(0).html("担保方式 <span class='red'>*</span>");
		$("#diyafangshi").attr("required","required");
	}else{
		$("#diyafangshi").siblings("label").eq(0).html("担保方式");
//  		$("#rel_type").html("");
		$("#diyafangshi").removeAttr("required");
	}
}

function changeMarry(obj) {
    var marryStatu = $(obj).val();
    //  	-MaritalStatus-Divorced 离异
    //  	-MaritalStatus-Married 已婚
	if (marryStatu=="-MaritalStatus-Unmarried"||marryStatu==""||marryStatu==undefined) {
		$("#dialog_person").find('[data-field="MarryFile"]').removeAttr("required");
		$("#dialog_person").find('[data-field="MarryFile"]').siblings("label").eq(0).html("婚姻证明");
	}else{
		$("#dialog_person").find('[data-field="MarryFile"]').attr("required",true);
		$("#dialog_person").find('[data-field="MarryFile"]').siblings("label").eq(0).html("婚姻证明 <span class='red'>*</span>");
	}
}
//bug180的改动
function dictionaryHide(){
	$("option").each(function(){
    	if($(this).attr("data-show")=="False"){
    		$(this).hide();
    	}else{
    		$(this).show();
    	}
    });
}
//获取市区
function getCity(){
	var id = $("#province").val();
	$("#area").parent().hide();
	$("#construction").parent().hide();
	$("#building").parent().hide();
	$("#house").parent().hide();
	$("#keyword").val("").parent().parent().hide();
	$("#gujiaSub").parent().parent().hide();
	$.ajax({
		url:getCitybyId+id,
		type:"get",
    	dataType:"json",
    	success:function(response){
    		var opt = "<option></option>";
    		for(var i=0;i<response.Data.length;i++){
    			opt = opt + "<option value='"+response.Data[i].CityId+"'>"+response.Data[i].CityName+"</option>"
    		}
    		$("#city").html(opt);
    		$("#city").parent().show();
    	},
    	error:function(response){
    		alert("请求失败");
    	}
	})
}
//获取区县
function getArea(){
	var id = $("#city").val();
//	$("#area").html("<option></option>");
	$("#construction").parent().hide();
	$("#building").parent().hide();
	$("#house").parent().hide();
	$("#keyword").val("");
	$("#gujiaSub").parent().parent().hide();
	$.ajax({
		url:getAreaById+id,
		type:"get",
    	dataType:"json",
    	success:function(response){
    		var opt = "<option></option>";
    		for(var i=0;i<response.Data.length;i++){
    			opt = opt + "<option value='"+response.Data[i].AreaId+"'>"+response.Data[i].AreaName+"</option>"
    		}
    		$("#area").html(opt);
    		$("#area").parent().show();
    		$("#keyword").parent().parent().show();
    	},
    	error:function(response){
    		alert("请求失败");
    	}
	})
}
//获取楼盘
function getConstruction(){
	$("#house").parent().hide();
	$("#building").parent().hide();
	$("#gujiaSub").parent().parent().hide();
	if($("#area").val()==""){
		showAlert("请先选择区");
		return;
	}
	if($("#keyword").val()==""){
		showAlert("请先输入关键字");
		return;
	}
	$.ajax({
		url:getConstructionById+"?Keyword="+$("#keyword").val()+"&cityID="+$("#city").val(),
		type:"get",
    	dataType:"json",
    	success:function(response){
    		if(response.Status=="Success"){
    		var opt = "<option></option>";
    		for(var i=0;i<response.Data.length;i++){
    			opt = opt + "<option value='"+response.Data[i].ConstructionId+"'>"+response.Data[i].ConstructionName+"</option>"
    		}
    		$("#construction").html(opt);
    		$("#construction").parent().show();
    		}else{
    			showAlert(response.Message);
    		}
    	},
    	error:function(response){
    		alert("请求失败");
    	}
	})
}
function getBuilding(){
	$("#house").parent().hide();
	$("#gujiaSub").parent().parent().hide();
	$.ajax({
		url:getBuildingById+"?ConstructionID="+$("#construction").val()+"&cityID="+$("#city").val(),
		type:"get",
    	dataType:"json",
    	success:function(response){
    		if(response.Status=="Success"){
    		var opt = "<option></option>";
    		for(var i=0;i<response.Data.length;i++){
    			opt = opt + "<option value='"+response.Data[i].BuildingId+"'>"+response.Data[i].BuildingName+"</option>"
    		}
    		$("#building").html(opt);
    		$("#building").parent().show();
    		}else{
    			showAlert(response.Message);
    		}
    	},
    	error:function(response){
    		alert("请求失败");
    	}
	})
}

function getHouse(){
	$.ajax({
		url:getHouseById+"?buildingID="+$("#building").val()+"&cityID="+$("#city").val(),
		type:"get",
    	dataType:"json",
    	success:function(response){
    		if(response.Status=="Success"){
    		var opt = "<option></option>";
    		for(var i=0;i<response.Data.length;i++){
    			opt = opt + "<option value='"+response.Data[i].HouseId+"'>"+response.Data[i].HouseName+"</option>"
    		}
    		$("#house").html(opt);
    		$("#house").parent().show();
    		}else{
    			showAlert(response.Message);
    		}
    	},
    	error:function(response){
    		alert("请求失败");
    	}
	})
}

function getAutoPrice(){
	$.ajax({
		url:getAutoPriceByInfo+
			"?cityID="+$("#city").val()+
			"&constructionID="+$("#construction").val()+
			"&buildingID="+$("#building").val()+
			"&houseID="+$("#house").val()+
			"&size="+$("#dialog_HouseDetails").find('[data-field="HouseSize"]').val(),
		type:"get",
    	dataType:"json",
    	success:function(response){
    		if(response.Status=="Success"){
    			$("#gujiaVal").val(response.Data.UnitPrice);
//  			$("#dialog_EstimateSources").find('[data-field="RushEstimate"]').val(response.Data.UnitPrice);
//  			$(".gujiaDialog").hide();
				$("#gujiaSub").parent().parent().show();
    		}
    	},
    	error:function(response){
    		alert("请求失败");
    	}
	})
}

function getByArea(){
	if($("#keyword").val()==""){
		return;
	}else{
		$("#house").parent().hide();
		$("#building").parent().hide();
		$("#gujiaSub").parent().parent().hide();
		$.ajax({
			url:getConstructionById+"?Keyword="+$("#keyword").val()+"&cityID="+$("#city").val(),
			type:"get",
	    	dataType:"json",
	    	success:function(response){
	    		if(response.Status=="Success"){
	    		var opt = "<option></option>";
	    		for(var i=0;i<response.Data.length;i++){
	    			opt = opt + "<option value='"+response.Data[i].ConstructionId+"'>"+response.Data[i].ConstructionName+"</option>"
	    		}
	    		$("#construction").html(opt);
	    		$("#construction").parent().show();
	    		}else{
	    			showAlert(response.Message);
	    		}
	    	},
	    	error:function(response){
	    		alert("请求失败");
	    	}
		})
	}
}
