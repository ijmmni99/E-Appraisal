window.onscroll = function () { myFunction() };
var header = document.getElementById("myHeader");
var sticky = header.offsetTop;

function openCity(evt, cityName) {
    document.documentElement.scrollTop = 0;
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}

function myFunction() {
    if (window.pageYOffset > sticky) {
        header.classList.add("sticky");
    } else {
        header.classList.remove("sticky");
    }
}

$("body").on("click", "#btnAdd", function () {
    //Reference the Name and Country TextBoxes.
    var txtName = $("#txtName");
    var txtCountry = $("#txtCountry");
    var txtEmp = $("#txtEmp");
    var herm = $("input[name=herm]").val();
    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblCustomers > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add Name cell.
    var cell = $(row.insertCell(-1));
    var btn_obj = $("<textarea />");
    btn_obj.attr("class", "form-control");
    btn_obj.val(txtName.val());
    cell.append(btn_obj);

    if (herm != "") {
        //Add Country cell.
        cell = $(row.insertCell(-1));
        var btnMan = $("<input />");
        btnMan.attr("type", "number");
        btnMan.attr("class", "form-control");
        btnMan.attr("name", "sec2_man_eval");
        btnMan.val(txtCountry.val());
        cell.append(btnMan);

        //Add man cell
        cell = $(row.insertCell(-1));
        var btnEmp_Eval = $("<input />");
        btnEmp_Eval.attr("type", "number");
        btnEmp_Eval.attr("class", "form-control");
        btnEmp_Eval.attr('disabled', true);
        btnEmp_Eval.val(txtEmp.val());
        cell.append(btnEmp_Eval);
    }
    else if (herm == "") {
        //Add Country cell.
        cell = $(row.insertCell(-1));
        var btnMan = $("<input />");
        btnMan.attr("type", "number");
        btnMan.attr("class", "form-control");
        btnMan.attr("name", "sec2_man_eval");
        btnMan.attr('disabled', true);
        btnMan.val(txtCountry.val());
        cell.append(btnMan);

        //Add man cell
        cell = $(row.insertCell(-1));
        var btnEmp_Eval = $("<input />");
        btnEmp_Eval.attr("type", "number");
        btnEmp_Eval.attr("class", "form-control");
        btnEmp_Eval.val(txtEmp.val());
        cell.append(btnEmp_Eval);
    }

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("class", "btn-danger");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtName.val("");
    txtCountry.val("");
    txtEmp.val("");
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("input", row).eq(0).val();
    if (confirm("Do you want to delete this ?")) {
        //Get the reference of the Table.
        var table = $("#tblCustomers")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

function Remove3(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete this ?")) {
        //Get the reference of the Table.
        var table = $("#out_tab")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

function Remove4(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete this ?")) {
        //Get the reference of the Table.
        var table = $("#ijm_tab")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

function PrintElem(elem) {
    var mywindow = window.open('', 'PRINT', 'height=400,width=600');

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + 'Performance Review' + '</h1>');
    mywindow.document.write(document.getElementById(elem).innerHTML);
    mywindow.document.write('<div style=page-break-before:always align="center"><span style="visibility: hidden;">-</span></div>');
    mywindow.document.write(document.getElementById('div5').innerHTML);
    mywindow.document.write('</body></html>');
    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}

document.getElementById("defaultOpen").click();

$("input[name=sec2_man_eval]").bind('keyup mouseup', function () {

    var total = 0;
    var v = 0;
    $("#tblCustomers TBODY TR").each(function () {
        var row = $(this);
        var customer = {};
        customer.Man_Eval = row.find("input").eq(0).val();
        customer.Emp_Eval = row.find("input").eq(1).val();
        if (!isNaN(parseFloat(customer.Man_Eval))) {
            total = total + parseFloat(customer.Man_Eval);
            v = v + 1;
        }
    });
    $('#totalsec2').html(total/v);
});

$("input[name=sec2_man_eval]").ready( function () {

    var total = 0;
    var v = 0;
    $("#tblCustomers TBODY TR").each(function () {
        var row = $(this);
        var customer = {};
        customer.Man_Eval = row.find("input").eq(0).val();
        customer.Emp_Eval = row.find("input").eq(1).val();
        if (!isNaN(parseFloat(customer.Man_Eval))) {
            total = total + parseFloat(customer.Man_Eval);
            v = v + 1;
        }
    });
    $('#totalsec2').html(total / v);
});

//$("input[name=sec2_man_eval2]").bind('keyup mouseup', function () {

//    var total = 0;
//    var v = 0;
//    var value = parseFloat($("input[name=sec2_man_eval2]").val());
//    $("#tblCustomers TBODY TR").each(function () {
//        var row = $(this);
//        var customer = {};
//        customer.Man_Eval = row.find("input").eq(0).val();
//        customer.Emp_Eval = row.find("input").eq(1).val();
//        if (!isNaN(parseFloat(customer.Man_Eval))) {
//            total = total + parseFloat(customer.Man_Eval);
//            v = v + 1;
//        }
//    });
//    if (!isNaN(value)) {
//        total = total + value;
//    }
//    $('#totalsec2').html(total/v);
//});

$("body").on("click", "#btnSaveSec5", function () {

    var data = new Array();
    var dat = {}; var dat2 = {}; var dat3 = {}; var dat4 = {}; var dat5 = {};

    dat.Area = $("input[id=sec5_area_1]").val();
    dat.Outcome = $("input[id=sec5_expect_1]").val();
    dat.Desire = $.trim($("#desire").val());
    dat.Terms = $.trim($("#career").val())
    data.push(dat);

    dat2.Area = $("input[id=sec5_area_2]").val();
    dat2.Outcome = $("input[id=sec5_expect_2]").val();
    dat2.Desire = $.trim($("#desire").val());
    dat2.Terms = $.trim($("#career").val())
    data.push(dat2);

    dat3.Area = $("input[id=sec5_area_3]").val();
    dat3.Outcome = $("input[id=sec5_expect_3]").val();
    dat3.Desire = $.trim($("#desire").val());
    dat3.Terms = $.trim($("#career").val())
    data.push(dat3);

    dat4.Area = $("input[id=sec5_area_4]").val();
    dat4.Outcome = $("input[id=sec5_expect_4]").val();
    dat4.Desire = $.trim($("#desire").val());
    dat4.Terms = $.trim($("#career").val())
    data.push(dat4);

    dat5.Area = $("input[id=sec5_area_5]").val();
    dat5.Outcome = $("input[id=sec5_expect_5]").val();
    dat5.Desire = $.trim($("#desire").val());
    dat5.Terms = $.trim($("#career").val())
    data.push(dat5);

    //Send the JSON array to Controller using AJAX.
    $.ajax({

        type: "POST",
        url: Sec5_url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            alert("Saved");
            openCity(event, 'Section 6');
            $("#opn6").addClass("active");
        }
    });
});

$("body").on("click", "#btnSaveSec1", function () {

        var data = new Array();
        var dat = {}; var dat2 = {}; var dat3 = {}; var dat4 = {}; var dat5 = {}; var dat6 = {}; var dat7 = {};

        dat.Factor = "1";
        dat.Emp_Eval = $("input[name=option1]:checked").val();
        dat.Man_Eval = $("input[name=option1_man]:checked").val();
        dat.Agreed_Score = $('#total1').html();
        dat.Support = $.trim($("textarea[name=input_example1]").val());
        dat.Actions = $.trim($("textarea[name=input_action1]").val());
        if (dat.Emp_Eval == null) {
            alert("Factor 1 not select yet, Please complete all factor before submit");
            return false;
        }
        else {
            data.push(dat);
        }

        dat2.Factor = "2";
        dat2.Emp_Eval = $("input[name=option2]:checked").val();
        dat2.Man_Eval = $("input[name=option2_man]:checked").val();
        dat2.Agreed_Score = $('#total2').html();
        dat2.Support = $.trim($("textarea[name=input_example2]").val());
        dat2.Actions = $.trim($("textarea[name=input_action2]").val());
        if (dat2.Emp_Eval == null) {
            alert("Factor 2 not select yet, Please complete all factor before submit");
            return false;
        }
        else {
            data.push(dat2);
        }

        dat3.Factor = "3";
        dat3.Emp_Eval = $("input[name=option3]:checked").val();
        dat3.Man_Eval = $("input[name=option3_man]:checked").val();
        dat3.Agreed_Score = $('#total3').html();
        dat3.Support = $.trim($("textarea[name=input_example3]").val());
        dat3.Actions = $.trim($("textarea[name=input_action3]").val());
        if (dat3.Emp_Eval == null) {
            alert("Factor 3 not select yet, Please complete all factor before submit");
            return false;
        } else {
            data.push(dat3);
        }

        dat4.Factor = "4";
        dat4.Emp_Eval = $("input[name=option4]:checked").val();
        dat4.Man_Eval = $("input[name=option4_man]:checked").val();
        dat4.Agreed_Score = $('#total4').html();
        dat4.Support = $.trim($("textarea[name=input_example4]").val());
        dat4.Actions = $.trim($("textarea[name=input_action4]").val());
        if (dat4.Emp_Eval == null) {
            alert("Factor 4 not select yet, Please complete all factor before submit");
            return false;
        }
        else {
            data.push(dat4);
        }

        dat5.Factor = "5";
        dat5.Emp_Eval = $("input[name=option5]:checked").val();
        dat5.Man_Eval = $("input[name=option5_man]:checked").val();
        dat5.Agreed_Score = $('#total5').html();
        dat5.Support = $.trim($("textarea[name=input_example5]").val());
        dat5.Actions = $.trim($("textarea[name=input_action5]").val());
        if (dat5.Emp_Eval == null) {
            alert("Factor 5 not select yet, Please complete all factor before submit");
            return false;
        }
        else {
            data.push(dat5);
        }

        dat6.Factor = "6";
        dat6.Emp_Eval = $("input[name=option6]:checked").val();
        dat6.Man_Eval = $("input[name=option6_man]:checked").val();
        dat6.Agreed_Score = $('#total6').html();
        dat6.Support = $.trim($("textarea[name=input_example6]").val());
        dat6.Actions = $.trim($("textarea[name=input_action6]").val());
        if (dat6.Emp_Eval == null) {
            alert("Factor 6 not select yet, Please complete all factor before submit");
            return false;
        }
        else {
            data.push(dat6);
        }

        dat7.Factor = "7";
        dat7.Emp_Eval = $("input[name=option7]:checked").val();
        dat7.Man_Eval = $("input[name=option7_man]:checked").val();
        dat7.Agreed_Score = $('#total7').html();
        dat7.Support = $.trim($("textarea[name=input_example7]").val());
        dat7.Actions = $.trim($("textarea[name=input_action7]").val());
        if (dat7.Emp_Eval == null) {
            alert("Factor 7 not select yet, Please complete all factor before submit");
            return false;
        }
        else {
            data.push(dat7);
        }

    //Send the JSON array to Controller using AJAX.
        $.ajax({
            type: "POST",
            url: Sec1_url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                alert("Saved");
                openCity(event, 'Section 2');
                $("#opn2").addClass("active");
            }
        });
});

$("body").on("click", "#btnSave", function () {
    //Loop through the Table rows and build a JSON array.
    var data = new Array();
    var validate = "";
    $("#tblCustomers TBODY TR").each(function () {
        var row = $(this);
        var customer = {};
        customer.Objective = row.find("textarea").eq(0).val();
        customer.Man_Eval = row.find("input").eq(0).val();
        customer.Emp_Eval = row.find("input").eq(1).val();
        if (customer.Man_Eval > 5 || customer.Man_Eval < 1 && customer.Man_Eval != "") {
            validate = "1";
            return false;
        }
        else if (customer.Emp_Eval > 5 || customer.Emp_Eval < 1 && customer.Emp_Eval != "") {
            validate = "2";
            return false;
        }
        data.push(customer);
    });  

    if (validate == "1") {
        alert("Manager evaluation must between 1 and 5")
    }
    else if (validate == "2") {
        alert("Employee evaluation must between 1 and 5");
    }
    else if (validate == "") {

        //Send the JSON array to Controller using AJAX.
        $.ajax({
            type: "POST",
            url: Sec2_url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                alert("Saved");
                openCity(event, 'Section 3');
                $("#opn3").addClass("active");
            }
        });
    }
});

$("body").on("click", "#btnSave1", function () {
    alert("Saved");
});

$("body").on("click", "#btnSave3", function () {
    //Loop through the Table rows and build a JSON array.
    var data = new Array();
    $("#out_tab TBODY TR").each(function () {
        var row = $(this);
        var arr = {};
        arr.Objective_sec3 = row.find("textarea").eq(0).val();
        arr.Standards = row.find("textarea").eq(1).val();
        arr.Measure = row.find("textarea").eq(2).val();;
        data.push(arr);
    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: Sec3_url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            alert("Saved");
            openCity(event, 'Section 4');
            $("#opn4").addClass("active");
        }
    });
});

$("body").on("click", "#btnSave4", function () {
    //Loop through the Table rows and build a JSON array.
    var data = new Array();
    $("#ijm_tab TBODY TR").each(function () {
        var row = $(this);
        var arr = {};
        arr.Objective_sec4 = row.find("textarea").eq(0).val();
        arr.Standards_sec4 = row.find("textarea").eq(1).val();
        arr.Measure_sec4 = row.find("textarea").eq(2).val();;
        data.push(arr);
    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: Sec4_url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            alert("Saved");
            openCity(event, 'Section 5');
            $("#opn5").addClass("active");
        }
    });
});

$("body").on("click", "#btnSave6", function () {
    //Loop through the Table rows and build a JSON array.
        var data = new Array();
        var arr = {};

        arr.F_Agreed_Score = $('#totalfinal').html();
        arr.Emp_Comment = $.trim($("textarea[name=txtComEMP]").val());
        arr.Man_Comment = $.trim($("textarea[name=txtComMAN]").val());
        if ($('input[name=chkemp]').is(':checked')) {
            arr.Emp_check = 1;
        }
        else {
            arr.Emp_check = 0;
        }

        if ($('input[name=chkman]').is(':checked')) {
            arr.Man_check = 1;
        }
        else {
            arr.Man_check = 0;
        }
        data.push(arr);
        
        if (arr.Man_check == 0 && arr.Emp_check == 0) {
            alert("Click the checkmark first before submit")
        }
        else {
            if (confirm("Change cannot be done after submit, Proceed ?") == true) {
                $.ajax({
                    type: "POST",
                    url: Sec6_url,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        alert("Submitted");
                    }
                });
                window.location.reload();
            }         
        } 
});

$(document).ready(function () {
    $('#btn_hdn1').on('click', function () {
        $('.hdn1').show();
        $('.hdn2').hide();
        $('.hdn3').hide();
        $('.hdn4').hide();
        $('.hdn5').hide();
        $('.hdn6').hide();
        $('.hdn7').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdn2').on('click', function () {
        $('.hdn2').show();
        $('.hdn1').hide();
        $('.hdn3').hide();
        $('.hdn4').hide();
        $('.hdn5').hide();
        $('.hdn6').hide();
        $('.hdn7').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdn3').on('click', function () {
        $('.hdn3').show();
        $('.hdn2').hide();
        $('.hdn1').hide();
        $('.hdn4').hide();
        $('.hdn5').hide();
        $('.hdn6').hide();
        $('.hdn7').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdn4').on('click', function () {
        $('.hdn4').show();
        $('.hdn2').hide();
        $('.hdn3').hide();
        $('.hdn1').hide();
        $('.hdn5').hide();
        $('.hdn6').hide();
        $('.hdn7').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdn5').on('click', function () {
        $('.hdn5').show();
        $('.hdn2').hide();
        $('.hdn3').hide();
        $('.hdn4').hide();
        $('.hdn1').hide();
        $('.hdn6').hide();
        $('.hdn7').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdn6').on('click', function () {
        $('.hdn6').show();
        $('.hdn2').hide();
        $('.hdn3').hide();
        $('.hdn4').hide();
        $('.hdn5').hide();
        $('.hdn1').hide();
        $('.hdn7').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdn7').on('click', function () {
        $('.hdn7').show();
        $('.hdn2').hide();
        $('.hdn3').hide();
        $('.hdn4').hide();
        $('.hdn5').hide();
        $('.hdn6').hide();
        $('.hdn1').hide();
    });
});

$(document).ready(function () {
    $('#btn_hdnall').on('click', function () {
        $('.hdn2').show();
        $('.hdn7').show();
        $('.hdn3').show();
        $('.hdn4').show();
        $('.hdn5').show();
        $('.hdn6').show();
        $('.hdn1').show();
    });
});


$('input[id=option11]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option11]").val());
    var secValue = parseFloat($("input[name=option1_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total1').html(firstValue);
    }
    else{
        $('#total1').html(secValue);
    }
})

$('input[id=option12]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option12]").val());
    var secValue = parseFloat($("input[name=option1_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total1').html(firstValue);
    }
    else {
        $('#total1').html(secValue);
    }
})

$('input[id=option13]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option13]").val());
    var secValue = parseFloat($("input[name=option1_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total1').html(firstValue);
    }
    else {
        $('#total1').html(secValue);
    }
})

$('input[id=option14]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option14]").val());
    var secValue = parseFloat($("input[name=option1_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total1').html(firstValue);
    }
    else {
        $('#total1').html(secValue);
    }
})

$('input[id=option15]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option15]").val());
    var secValue = parseFloat($("input[name=option1_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total1').html(firstValue);
    }
    else {
        $('#total1').html(secValue);
    }
})

$('input[id=option21]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option21]").val());
    var secValue = parseFloat($("input[name=option2_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total2').html(firstValue);
    }
    else {
        $('#total2').html(secValue);
    }
})

$('input[id=option22]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option22]").val());
    var secValue = parseFloat($("input[name=option2_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total2').html(firstValue);
    }
    else {
        $('#total2').html(secValue);
    }
})

$('input[id=option23]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option23]").val());
    var secValue = parseFloat($("input[name=option2_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total2').html(firstValue);
    }
    else {
        $('#total2').html(secValue);
    }
})

$('input[id=option24]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option24]").val());
    var secValue = parseFloat($("input[name=option2_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total2').html(firstValue);
    }
    else {
        $('#total2').html(secValue);
    }
})

$('input[id=option25]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option25]").val());
    var secValue = parseFloat($("input[name=option2_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total2').html(firstValue);
    }
    else {
        $('#total2').html(secValue);
    }
})

$('input[id=option31]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option31]").val());
    var secValue = parseFloat($("input[name=option3_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total3').html(firstValue);
    }
    else {
        $('#total3').html(secValue);
    }
})

$('input[id=option32]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option32]").val());
    var secValue = parseFloat($("input[name=option3_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total3').html(firstValue);
    }
    else {
        $('#total3').html(secValue);
    }
})

$('input[id=option33]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option33]").val());
    var secValue = parseFloat($("input[name=option3_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total3').html(firstValue);
    }
    else {
        $('#total3').html(secValue);
    }
})

$('input[id=option34]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option34]").val());
    var secValue = parseFloat($("input[name=option3_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total3').html(firstValue);
    }
    else {
        $('#total3').html(secValue);
    }
})

$('input[id=option35]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option35]").val());
    var secValue = parseFloat($("input[name=option3_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total3').html(firstValue);
    }
    else {
        $('#total3').html(secValue);
    }
})

$('input[id=option41]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option41]").val());
    var secValue = parseFloat($("input[name=option4_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total4').html(firstValue);
    }
    else {
        $('#total4').html(secValue);
    }
})

$('input[id=option42]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option42]").val());
    var secValue = parseFloat($("input[name=option4_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total4').html(firstValue);
    }
    else {
        $('#total4').html(secValue);
    }
})

$('input[id=option43]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option43]").val());
    var secValue = parseFloat($("input[name=option4_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total4').html(firstValue);
    }
    else {
        $('#total4').html(secValue);
    }
})

$('input[id=option44]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option44]").val());
    var secValue = parseFloat($("input[name=option4_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total4').html(firstValue);
    }
    else {
        $('#total4').html(secValue);
    }
})

$('input[id=option45]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option45]").val());
    var secValue = parseFloat($("input[name=option4_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total4').html(firstValue);
    }
    else {
        $('#total4').html(secValue);
    }
})

$('input[id=option51]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option51]").val());
    var secValue = parseFloat($("input[name=option5_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total5').html(firstValue);
    }
    else {
        $('#total5').html(secValue);
    }
})

$('input[id=option52]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option52]").val());
    var secValue = parseFloat($("input[name=option5_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total5').html(firstValue);
    }
    else {
        $('#total5').html(secValue);
    }
})

$('input[id=option53]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option53]").val());
    var secValue = parseFloat($("input[name=option5_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total5').html(firstValue);
    }
    else {
        $('#total5').html(secValue);
    }
})

$('input[id=option54]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option54]").val());
    var secValue = parseFloat($("input[name=option5_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total5').html(firstValue);
    }
    else {
        $('#total5').html(secValue);
    }
})

$('input[id=option55]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option55]").val());
    var secValue = parseFloat($("input[name=option5_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total5').html(firstValue);
    }
    else {
        $('#total5').html(secValue);
    }
})

$('input[id=option61]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option61]").val());
    var secValue = parseFloat($("input[name=option6_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total6').html(firstValue);
    }
    else {
        $('#total6').html(secValue);
    }
})

$('input[id=option62]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option62]").val());
    var secValue = parseFloat($("input[name=option6_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total6').html(firstValue);
    }
    else {
        $('#total6').html(secValue);
    }
})

$('input[id=option63]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option63]").val());
    var secValue = parseFloat($("input[name=option6_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total6').html(firstValue);
    }
    else {
        $('#total6').html(secValue);
    }
})

$('input[id=option64]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option64]").val());
    var secValue = parseFloat($("input[name=option6_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total6').html(firstValue);
    }
    else {
        $('#total6').html(secValue);
    }
})

$('input[id=option65]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option65]").val());
    var secValue = parseFloat($("input[name=option6_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total6').html(firstValue);
    }
    else {
        $('#total6').html(secValue);
    }
})

$('input[id=option71]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option71]").val());
    var secValue = parseFloat($("input[name=option7_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total7').html(firstValue);
    }
    else {
        $('#total7').html(secValue);
    }
})

$('input[id=option72]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option72]").val());
    var secValue = parseFloat($("input[name=option7_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total7').html(firstValue);
    }
    else {
        $('#total7').html(secValue);
    }
})

$('input[id=option73]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option73]").val());
    var secValue = parseFloat($("input[name=option7_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total7').html(firstValue);
    }
    else {
        $('#total7').html(secValue);
    }
})

$('input[id=option74]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option74]").val());
    var secValue = parseFloat($("input[name=option7_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total7').html(firstValue);
    }
    else {
        $('#total7').html(secValue);
    }
})

$('input[id=option75]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option75]").val());
    var secValue = parseFloat($("input[name=option7_man]:checked").val());
    if (isNaN(secValue)) {
        $('#total7').html(firstValue);
    }
    else {
        $('#total7').html(secValue);
    }
});


//manager

$('input[id=option11_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option11_man]").val());
        $('#total1').html(firstValue);
})

$('input[id=option12_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option12_man]").val());
        $('#total1').html(firstValue);
})

$('input[id=option13_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option13_man]").val());
        $('#total1').html(firstValue);
})

$('input[id=option14_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option14_man]").val());
        $('#total1').html(firstValue);
})

$('input[id=option15_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option15_man]").val());
        $('#total1').html(firstValue);
})

$('input[id=option21_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option21_man]").val());
        $('#total2').html(firstValue);
})

$('input[id=option22_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option22_man]").val());
        $('#total2').html(firstValue);
})

$('input[id=option23_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option23_man]").val());
        $('#total2').html(firstValue);
})

$('input[id=option24_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option24_man]").val());
        $('#total2').html(firstValue);
})

$('input[id=option25_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option25_man]").val());
        $('#total2').html(firstValue);
})

$('input[id=option31_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option31_man]").val());
        $('#total3').html(firstValue);
})

$('input[id=option32_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option32_man]").val());
        $('#total3').html(firstValue);
})

$('input[id=option33_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option33_man]").val());
        $('#total3').html(firstValue);
})

$('input[id=option34_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option34_man]").val());
        $('#total3').html(firstValue);
})

$('input[id=option35_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option35_man]").val());
        $('#total3').html(firstValue);
})

$('input[id=option41_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option41_man]").val());
        $('#total4').html(firstValue);
})

$('input[id=option42_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option42_man]").val());
        $('#total4').html(firstValue);
})

$('input[id=option43_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option43_man]").val());
        $('#total4').html(firstValue);
})

$('input[id=option44_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option44_man]").val());
        $('#total4').html(firstValue);
})

$('input[id=option45_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option45_man]").val());
        $('#total4').html(firstValue);
})

$('input[id=option51_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option51_man]").val());
        $('#total5').html(firstValue);
})

$('input[id=option52_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option52_man]").val());
        $('#total5').html(firstValue);
})

$('input[id=option53_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option53_man]").val());
        $('#total5').html(firstValue);
})

$('input[id=option54_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option54_man]").val());
        $('#total5').html(firstValue);
})

$('input[id=option55_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option55_man]").val());
        $('#total5').html(firstValue);
})

$('input[id=option61_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option61_man]").val());
        $('#total6').html(firstValue);
})

$('input[id=option62_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option62_man]").val());
        $('#total6').html(firstValue);
})

$('input[id=option63_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option63_man]").val());
        $('#total6').html(firstValue);
})

$('input[id=option64_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option64_man]").val());
        $('#total6').html(firstValue);
})

$('input[id=option65_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option65_man]").val());
        $('#total6').html(firstValue);
})

$('input[id=option71_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option71_man]").val());
        $('#total7').html(firstValue);
})

$('input[id=option72_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option72_man]").val());
        $('#total7').html(firstValue);
})

$('input[id=option73_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option73_man]").val());
        $('#total7').html(firstValue);
})

$('input[id=option74_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option74_man]").val());
        $('#total7').html(firstValue);
})

$('input[id=option75_man]').on("click", function (e) {
    var firstValue = parseFloat($("input[id=option75_man]").val());
        $('#total7').html(firstValue);
});
