$("[tooltip='true']").tooltip({
	show: {
		effect: "slideDown",
		delay: 250
	}
});

var datePicker = $('.date-picker').datepicker({
    autoclose: true,
    changeMonth: true,
    changeYear: true,
    yearRange: "c-65:c+20",
    dateFormat: 'yy-mm-dd'
}).next().click(function () {
    $(this).prev().focus();
}).css("cursor", "pointer");

var datePickerLimit = $(".datePickerLimit").datepicker({
    autoclose: true,
    changeMonth: true,
    changeYear: true,
    yearRange: "c-65:c+20",
    dateFormat: 'yy-mm-dd',
    minDate:0
}).next().click(function () {
    $(this).prev().focus();
}).css("cursor", "pointer");

//readonly
if($("#isEdited").length==0){
	if ($("#CaseStatusRW").val() == "readonly") {
        datePicker.removeClass("date-picker").attr("disabled","disabled")
			.removeAttr("data-field").removeAttr("data-class").removeAttr("id");
    }
}else{
	if($("#isEdited").val()=="readonly"){
		datePicker.removeClass("date-picker").attr("disabled","disabled")
			.removeAttr("data-field").removeAttr("data-class").removeAttr("id");
	}else{
		if($(".tab-content").find(".active").attr("id")!="publicMortgage"){
			datePicker.removeClass("date-picker").attr("disabled","disabled")
				.removeAttr("data-field").removeAttr("data-class").removeAttr("id");
		}
	}
}
//$("[data-fill]").each(function () {
//  var str = $(this).attr("data-fill");
//  var arr = str.split(":");
//  var type = arr[0];
//  var key = arr[1];
//
//  if ($(this).is("select")) {
//      if (type == "Dict") {
//          GetDictionary(this, key);
//      }
//  }
//});
