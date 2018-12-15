/* For history.js */
//var location = window.history.location || window.location;
var ckeditorOptions = {};

$(document).ready(function() {
    pageSetUp();

    // Menu active class add for parent menu
    $("#left-panel nav").find("li.active").each(function() {
        $(this).parentsUntil("nav", "li").addClass("active");
    });

    // Show flash message
    $("[data-msg]").flashMessage();

    // ckeditor
    if (window.ckeditor) {
        $("[data-content-editor]").ckeditor(ckeditorOptions);
    }
});

(function ($) {

    $.fn.alert = function (msgHtml, type) {

        var typeClass = 'info';
        
        var icon = "";
        switch (type.toString()) {
            case "error": 
            case "0": 
                icon = "times";  
                typeClass = "danger";
                break;
            case "success": 
            case "1": 
                icon = "check"; 
                typeClass = "success";
                break;
            case "info": 
            case "2": 
                icon = "info"; 
                typeClass = "info";
                break;
            case "warning": 
            case "3": 
                icon = "warning"; 
                typeClass = "warning";
                break;
        }

        var html =
            "<div class=\"alert alert-" + typeClass + " fade in\">" +
                "<a data-dismiss=\"alert\" class=\"close\">×</a>" +
                "<i class=\"fa-fw fa fa-" + icon + "\"></i>" +
                msgHtml +
            "</div>";

        $(this).html(html);

        $("html, body").animate({
            scrollTop: $("[data-msg]").offset().top - 50
        }, 500);
    };

}(jQuery));

(function ($) {

    $.fn.clearMessagesInForm = function () {

        // For smart form design
        $(this).find(".state-error").removeClass("state-error");
        $(this).find("div.txt-color-red").remove();
        $(this).find("[data-msg]").html("");

        // For bootstrap form design
        $(this).find(".has-error").children(".form-control-feedback, .help-block").remove();
        $(this).find(".has-error").removeClass("has-error");

    }

}(jQuery));

(function ($) {

    $.showMessages = function (jsonMsg, $form) {

        // if message null or empty return
        if (! jsonMsg) return;
        
        if (jsonMsg.success === false) {
            $.showHeaderMessages(jsonMsg, $form);
            return;
        }

        var errors = jsonMsg.Errors;

        console.log(errors);

        for (var element in errors) {

            if (errors[element].length === 0)
                continue;

            var property = errors[element].PropertyName;
            var message = errors[element].ErrorMessage;

            if ($form.hasClass("smart-form")) {
                $form.find("#" + property).parent("label").addClass("state-error")
                    .after("<div class=\"note txt-color-red\">" + message + "</div>");
            } else if ($form.hasClass("form-horizontal")) {
                
                $form.find("#" + property)
                    .after("<i class=\"form-control-feedback glyphicon glyphicon-remove\"></i>" +
                    "<small class=\"help-block\">" + message + "</small>")
                    .parent("div").addClass("has-error");

                if ($form.find("[data-checkbox]").size() > 0){
                    var elmt = property;

                    if (elmt.indexOf(".") !== -1)
                        elmt = elmt.split(".")[0];

                    $form.find("[data-checkbox='" + elmt + "']")
                        .append("<i class=\"form-control-feedback glyphicon glyphicon-remove\"></i>" +
                        "<small class=\"help-block\">" + message + "</small>")
                        .addClass("has-error");
                }
            }
        }
    }

    $.showHeaderMessages = function (result, $form) {

        // if message null or empty return
        if (result && (!result.Message || result.Message.length === 0)) return;
        
        $form.find("[data-msg]").alert(result.Message, result.MessageType || ( result.Success === false ? "error" : "success"));
    }

}(jQuery));

$("form[data-ajax=true]").each(function () {

    $("a[data-save]").click(function(){
        $("form[data-ajax=true]").submit();
    });

    $(this).submit(function (e) {

        var $form = $(this);

        // Set value to textarea back for edited ckeditor
        if ($(".cke").size() > 0) {
            $("[data-content-editor]").each(function() {
                var name = $(this).attr("name");
                CKEDITOR.instances[name].updateElement();
            });
        }

        // Clear all last error messages
        $form.clearMessagesInForm();

        $form.find("[type=\"submit\"]").prop("disabled", true);

        $.ajax({
            url: this.action || window.location.href,
            type: "POST",
            data: $(this).serialize(),
            dataType: "json",
            success: function (result, status, xhr) {

                var ct = xhr.getResponseHeader("content-type") || "";

                if (ct.indexOf("json") > -1) {

                    if (result.Script) {
                        eval(result.Script);
                    }
                
                    if (result.ClearForm) {
                        $form[0].reset();
                    }

                    // If Redriect with Message Then show flash message
                    if (result.Message != null && result.Message.length > 0 && jQuery.trim(result.Redirect) !== "")
                        $.cookie("Flash." + MessageTypeToStr(result.MessageType), result.Message, { path: "/" });

                    if (jQuery.trim(result.Redirect) !== "") {
                        window.location.href = result.Redirect;
                        return;
                    }

                    $.showHeaderMessages(result, $form);
                }

                $form.find("[type=\"submit\"]").prop("disabled", false);

            },
            error: function (jqXhr) {

                switch(jqXhr.status)
                {
                    case 401:
                        //$( location ).prop( 'pathname', 'auth/login' );
                        break;
                    case 422:
                        $.showMessages(jqXhr.responseJSON, $form);
                        break;
                }

                $form.find("[type=\"submit\"]").prop("disabled", false);
            }
        });

        return false;
    });

});


var adminList = {
    original: {
        // sort column name
        sortColumn: null,
        // sort type
        sortType: null
    },

    // url to post data
    urlPost: null,
    // taget div with all required controlles
    $target: $('[data-table="true"]'),
    // tempalte id
    templateId: "#tplData",
    // sort column name
    sortColumn: $('[data-main-table="true"]').attr("data-sort-column") || null,
    // sort type
    sortType: $('[data-main-table="true"]').attr("data-sort-type") || null,
    // allow back button
    allowBackButton: true,
    // allow sequencing
    allowSequencing: false,

    init: function () {
        var $this = this;

        if (arguments.length === 1) {
            var setting = arguments[0];

            // Set url where post data or call
            if (setting.urlPost != null && typeof (setting.urlPost) == "string") {
                this.urlPost = setting.urlPost;
            } else {
                this.urlPost = window.location.href;
            }

            // set target structure for find elements of list page
            if (setting.target != null && typeof (setting.target) == "string") {
                this.$target = $(setting.target);
            }

            // Set get list method name for geting data
            if (setting.templateId != null && typeof (setting.templateId) == "string") {
                this.templateId = setting.templateId;
            }

            // sort column name
            if (setting.sortColumn != null && typeof (setting.sortColumn) == "string") {
                this.sortColumn = this.original.sortColumn = setting.sortColumn;
            }

            // sort type
            if (setting.sortType != null && typeof (setting.sortType) == "string") {
                this.sortType = this.original.sortType = setting.sortType;
            }

            // allow back button
            if (setting.allowBackButton != null && typeof (setting.allowBackButton) == "boolean") {
                this.allowBackButton = setting.sortType;
            }

            // allow back button
            if (setting.allowSequencing != null && typeof (setting.allowSequencing) == "boolean") {
                this.allowSequencing = setting.allowSequencing;
            }
            
        }
        else {
            // code when no argument is there
            this.original.sortColumn = this.sortColumn;
            this.original.sortType = this.sortType;
        }

        if (this.allowBackButton) {
            $(window).on("popstate", function (e) {
                adminList.ajaxLoad(true);
            });    
        }

        if (this.$target.find('[data-main-table="true"]').eq(0).attr("data-sort") == "true") {
            this.allowSequencing = true;
        }

        /* pagging hide */
        this.$target.find(".dt-toolbar").hide();
        this.$target.find(".previous, .next").removeClass("disabled").addClass("disabled");
        this.$target.find(".previous a, .next a").unbind("click");

        // - Set sorting class so user will know which column will sort
        this.$target.find('[data-main-table="true"] thead tr').last().children("[data-name]")
            .removeClass("sorting sorting_asc sorting_desc").addClass("sorting").unbind("click");


        // Form Searching
        this.$target.find("thead tr").eq(0).find("input").bind("keypress", function (event) {
            if (event.which === 13) {
                event.preventDefault();

                $this.urlPost = "?page=" + 1 + "&size=" + $this.result.Paging.Size
                    + "&sortColumn=" + $this.sortColumn + "&sortType=" + $this.sortType + getSerializeValues($this.$target.find('[data-main-table="true"]'));

                history.pushState(null, null, $this.urlPost);
                $this.ajaxLoad();
            }
        });

        this.$target.find("thead tr").eq(0).find("input, select").bind("change", function (event) {

            event.preventDefault();

            $this.urlPost = "?page=" + 1 + "&size=" + $this.result.Paging.Size
                + "&sortColumn=" + $this.sortColumn + "&sortType=" + $this.sortType + getSerializeValues($this.$target.find('[data-main-table="true"]'));

            history.pushState(null, null, $this.urlPost);
            $this.ajaxLoad();

        });

        this.$target.find("thead tr").eq(0).find(".datepicker").each(function () {

            var $that = $(this),
                dataDateFormat = $that.attr("data-dateformat") || "dd.mm.yy";

            $that.datepicker({
                dateFormat: dataDateFormat,
                prevText: '<i class="fa fa-chevron-left"></i>',
                nextText: '<i class="fa fa-chevron-right"></i>',
                onSelect: function (d, i) {
                    if (d !== i.lastVal) {
                        $(this).change();
                    }
                }
            });

            //clear memory reference
            $that = null;
        });

        /* #region Submit action */
        $("#btnSubmitAction").click(function () {

            $this.$target.clearMessagesInForm();

            if ($this.$target.find("#selAction").val() === "") {
                $this.$target.find("[data-msg]").alert("Select at least one item from action list.", "info");
            } else {
                if ($("#selAction").val() === "export") {
                    document.location.href = window.location.href + (window.location.href.indexOf("?") === -1 ? "?" : "") + "&m=export";
                } else if ($this.$target.find("tbody").find("input:checked").size() === 0) {
                    $this.$target.find("[data-msg]").alert("Select at least one item from list.", "info");
                } else if ($("#selAction").val() === "delete") {
                    $.confirm({
                        text: "Do you want to delete selected record(s)?",
                        title: "Confirmation required",
                        confirmButtonClass: "btn-danger",
                        confirm: function(button) {
                            $this.ajaxLoad(false, "delete");
                        },
                        cancel: function(button) {}
                    });
                } else { // if ($('#selAction').val() === 'active' || ($('#selAction').val() === 'inactive'))
                    $this.ajaxLoad(false, $("#selAction").val());
                }
            }
                
        });
        /* #endregion */

        /* #region Selected number of visible records */
        $this.$target.find("thead tr").eq(1).find("input:checkbox").attr("autocomplete", "off").unbind("click").click(function () {
            if ($(this).is(":checked"))
                $this.$target.find("tbody").find('input[type="checkbox"]').prop("checked", true);
            else
                $this.$target.find("tbody").find('input[type="checkbox"]').prop("checked", false);
        });
        /* #endregion */
    },

    ajaxLoad: function (popstate, action, deleteId) {

        // set all params from url
        if (popstate) {
            this.sortColumn = getParameterByName("sortColumn") || this.original.sortColumn;
            this.sortType = getParameterByName("sortType") || this.original.sortType;
        } else {
            this.sortColumn = getParameterByName("sortColumn") || this.sortColumn;
            this.sortType = getParameterByName("sortType") || this.sortType;
        }
        

        // Search fields
        this.$target.find("thead tr").eq(0).find("input, select").each(function () {
            $(this).val(getParameterByName($(this).attr("name")));
        });

        var $this = this;

        var url = popstate ? window.location.href : $this.urlPost;

        var extraData = "";
        if (url == null) {
            extraData += '&sortColumn=' + this.sortColumn;
            extraData += '&sortType=' + this.sortType;
        }

        $.ajax({
            url: popstate ? window.location.href : $this.urlPost,
            type: "POST",
            data: "action=" + (action || "") + "&ids=" 
                + (deleteId || $this.$target.find("tbody").find("input:checked").map(function() {return this.value;}).get().join(","))
                + extraData,
            success: function (result, status, xhr) {

                var ct = xhr.getResponseHeader("content-type") || "";

                if (ct.indexOf("json") <= -1)
                    return;

                $this.result = result;

                // Execute script from server
                if (jQuery.trim(result.Script) !== "")
                    eval(result.Script);

                // If Redriect with Message Then show flash message
                if (result.Message != null && result.Message.length > 0 && jQuery.trim(result.Redirect) !== "")
                    $.cookie("Flash." + MessageTypeToStr(result.MessageType), result.Message, { path: "/" });

                // if requie then redirect from server
                if (jQuery.trim(result.Redirect) !== "") {
                    window.location.href = result.Redirect;
                    return;
                }

                // Show success/error message from server
                $.showHeaderMessages(result, $this.$target);

                if (!result.Success)
                    return;

                // Reset Sorting
                $this.$target.find('[data-main-table="true"] thead tr').last().children("[data-name]")
                    .removeClass("sorting sorting_asc sorting_desc").addClass("sorting").unbind("click");

                // remove header checkbox
                $this.$target.find("thead tr").eq(1).find("input:checkbox").prop("checked", false);

                //if (result.success && result.Paging.Total > 0) 
                if (result.Paging.Total > 0) {

                    // Sorting - on click set login for sorting
                    $this.$target.find('[data-main-table="true"] thead tr th[data-name="' + $this.sortColumn + '"]')
                        .removeClass("sorting sorting_asc sorting_desc")
                        .addClass("sorting_" + $this.sortType)
                        .attr("data-sort-type", $this.sortType);

                    $this.$target.find('[data-main-table="true"] thead tr').last().children("[data-name]").click(function () {

                        $this.sortColumn = $(this).attr("data-name");
                        $this.sortType = $(this).attr("data-sort-type") === "desc" ? "asc" : "desc";

                        $this.urlPost = "?page=" + 1 + "&size=" + $this.result.Paging.Size
                            + "&sortColumn=" + $this.sortColumn + "&sortType=" + $this.sortType + getSerializeValues($this.$target.find('[data-main-table="true"]'));

                        history.pushState({ 'sortName': $this.sortColumn }, null, $this.urlPost);
                        $this.ajaxLoad();
                    });


                    // Header pagging show
                    $this.$target.find(".dt-toolbar").show();

                    // Page size set & page size click bind
                    $this.$target.find('[data-page="true"]').val(result.Paging.Size)
                        .unbind("change").change(function () {

                        $this.urlPost = "?page=" + 1 + "&size=" + $(this).val()
                            + "&sortColumn=" + $this.sortColumn + "&sortType=" + $this.sortType + getSerializeValues($this.$target.find('[data-main-table="true"]'));

                        history.pushState(null, null, $this.urlPost);
                        $this.ajaxLoad();
                    });

                    // Set records information
                    $this.$target.find(".paging-view-text .text-primary").html(result.Paging.Total);
                    $this.$target.find(".pagination-info span").html("Page " + result.Paging.Page + " of " + Math.ceil(result.Paging.Total / result.Paging.Size));

                    /* Paging Event */

                    if (result.Paging.Page !== 1) {
                        $(".pagination li.previous").removeClass("disabled");
                        $(".pagination li.previous a").unbind("click").click(function() {

                            $this.urlPost = "?page=" + ($this.result.Paging.Page - 1) + "&size=" + $this.result.Paging.Size
                                 + "&sortColumn=" + $this.sortColumn + "&sortType=" + $this.sortType + getSerializeValues($this.$target.find('[data-main-table="true"]'));
                                 
                            history.pushState(null, null, $this.urlPost);
                            $this.ajaxLoad();
                        });
                    } else {
                        $this.$target.find(".previous").removeClass("disabled").addClass("disabled");
                        $this.$target.find(".previous a").unbind("click");
                    }

                    if (result.Paging.Page < Math.ceil(result.Paging.Total / result.Paging.Size)) {
                        $(".pagination li.next").removeClass("disabled");
                        $(".pagination li.next a").unbind("click").click(function () {

                            $this.urlPost = "?page=" + ($this.result.Paging.Page + 1) + "&size=" + $this.result.Paging.Size
                                 + "&sortColumn=" + $this.sortColumn + "&sortType=" + $this.sortType + getSerializeValues($this.$target.find('[data-main-table="true"]'));
                            
                            history.pushState(null, null, $this.urlPost);
                            $this.ajaxLoad();
                        });
                    } else {
                        $this.$target.find(".next").removeClass("disabled").addClass("disabled");
                        $this.$target.find(".next a").unbind("click");
                    }
                    
                } else {

                    /* pagging hide */
                    $this.$target.find(".dt-toolbar").hide();

                    $this.$target.find(".previous, .next").removeClass("disabled").addClass("disabled");
                    $this.$target.find(".previous a, .next a").unbind("click");
                }

                /* Template Bind */
                var source = $($this.templateId).html();
                var template = Handlebars.compile(source);
                var html = template(result);
                $this.$target.find("tbody").html(html);

                /* After template bind code*/

                // tooltip
                if ($this.$target.find("[rel=tooltip]").length) {
                    $this.$target.find("[rel=tooltip]").tooltip();
                }

                //expand
                $this.$target.find(".expand").parent().next().hide();
                $this.$target.find(".expand").click(function () {
                    if ($(this).parent().next().is(":visible")) {
                        $(this).parent().next().hide();
                        $(this).find("i").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                    } else {
                        $(this).parent().next().show();
                        $(this).find("i").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                    }
                });

                // checkbox 
                $this.$target.find("tbody").find('input[type="checkbox"]').click(function () {

                    if ($this.$target.find("tbody").find('input[type="checkbox"]').length === $this.$target.find("tbody").find("input:checked").length)
                        $this.$target.find("thead tr").eq(1).find("input:checkbox").attr("checked", true);
                    else
                        $this.$target.find("thead tr").eq(1).find("input:checkbox").attr("checked", false);
                });


                // list delete click event
                $this.$target.find("tbody").find("[data-delete=true]").click(function () {
                    var Id = $(this).attr("data-id");

                    $.confirm({
                        text: "Do you want to delete record?",
                        title: "Confirmation required",
                        confirmButtonClass: "btn-danger",
                        confirm: function (button) {
                            $this.ajaxLoad(false, "delete", Id);
                        },
                        cancel: function (button) { }
                    });
                });

                // sequancing data
                if ($this.allowSequencing)
                {
                    $this.$target.find("tbody").sortable({
                        cursor: "n-resize",
                        handle: ".drag",
                        opacity: 0.7,
                        revert: true,
                        stop: function(event, ui) {

                            var ids = jQuery.map(
                                $this.$target.find("tr[data-id]").map(function() {
                                    return $(this).attr("data-id");
                                }), function(n) {
                                    return (n);
                                }).join(",");

                            $this.ajaxLoad(false, "sequencing", ids);
                        }
                    });
                    $this.$target.find("tbody").disableSelection();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });

    }
};


function getSerializeValues(element) {
    var $element = $(element).find("thead tr").eq(0);

    var $retValue = "";

    $($element).find(":input, select").each(function() {
        $retValue += "&" + $(this).attr("name") + "=" + encodeURIComponent($(this).val());
    });

    return $retValue;
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return "";
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

(function ($) {
    jQuery.fn.focusAtEnd = function () {
        return this.each(function () {
            $(this).focus();

            // If this function exists...
            if (this.setSelectionRange) {
                // ... then use it
                // (Doesn't work in IE)

                // Double the length because Opera is inconsistent about whether a carriage return is one character or two. Sigh.
                var len = $(this).val().length * 2;
                this.setSelectionRange(len, len);
            }
            else {
                // ... otherwise replace the contents with itself
                // (Doesn't work in Google Chrome)
                $(this).val($(this).val());
            }

            // Scroll to the bottom, in case we're in a tall textarea
            // (Necessary for Firefox and Google Chrome)
            this.scrollTop = 999999;
        });
    };
})(jQuery);

if (typeof Handlebars !== "undefined") {
    Handlebars.registerHelper("math", function (lvalue, operator, rvalue, options) {
        lvalue = parseFloat(lvalue);
        rvalue = parseFloat(rvalue);

        return {
            "+": lvalue + rvalue,
            "-": lvalue - rvalue,
            "*": lvalue * rvalue,
            "/": lvalue / rvalue,
            "%": lvalue % rvalue
        }[operator];
    });

    Handlebars.registerHelper("date", function (datetime, options) {
        if (datetime === null)
            return "-";

        var dt = new Date(parseInt(datetime.substr(6)));
        
        return (
            (dt.getMonth() + 1 < 10 ? "0" : "") + (dt.getMonth() + 1)
                + "/" + (dt.getDate() < 10 ? "0" : "") + dt.getDate() + "/"
                + dt.getFullYear());
    });

    Handlebars.registerHelper("datetime", function (datetime, options) {
        if (datetime === null)
            return "-";

        var dt = new Date(parseInt(datetime.substr(6)));
        
        return (
            (dt.getMonth() + 1 < 10 ? "0" : "") + (dt.getMonth() + 1)
            + "/" + (dt.getDate() < 10 ? "0" : "") + dt.getDate() + "/"
            + dt.getFullYear() + " "
            + (dt.getHours() < 10 ? "0" : "") + dt.getHours() + ":"
            + (dt.getMinutes() < 10 ? "0" : "") + dt.getMinutes());
    });

    Handlebars.registerHelper("ifCond", function (v1, operator, v2, options) {

        switch (operator) {
            case "==":
                return (v1 === v2) ? options.fn(this) : options.inverse(this);
            case "===":
                return (v1 === v2) ? options.fn(this) : options.inverse(this);
            case "!=":
                return (v1 !== v2) ? options.fn(this) : options.inverse(this);
            case "<":
                return (v1 < v2) ? options.fn(this) : options.inverse(this);
            case "<=":
                return (v1 <= v2) ? options.fn(this) : options.inverse(this);
            case ">":
                return (v1 > v2) ? options.fn(this) : options.inverse(this);
            case ">=":
                return (v1 >= v2) ? options.fn(this) : options.inverse(this);
            case "&&":
                return (v1 && v2) ? options.fn(this) : options.inverse(this);
            case "||":
                return (v1 || v2) ? options.fn(this) : options.inverse(this);
            default:
                return options.inverse(this);
        }
    });

    Handlebars.registerHelper("gravatar", function(context, options) {
        
        var email = context;
        var size=( typeof(options.hash.size) === "undefined") ? 32 : options.hash.size;
        
        return "https://www.gravatar.com/avatar/" + MD5( email ) + "?s="+ size + "&d=mm";
    });

    Handlebars.registerHelper("virtualDir", function(context, options) {

        if (context === null)
            return "";

        if (context.startsWith("/"))
            context = context.replace(/^[\/]+|[\/]+$/g, "");

        return virtualDir + context;
    });

    Handlebars.registerHelper("dash", function(context, options) {
        var dash = "";

        for (i = 1; i <= context; i++) { 
            dash += "&nbsp; - &nbsp;";
        }

        return dash;
    });

    Handlebars.registerHelper("camelCase", function(context, options) {
        return fixStr(context);
    });

    Handlebars.registerHelper("humanize", function(context, options) {
        if(context) { 
            context = context.split(/(?=[A-Z])/);

            // go through each word in the text and capitalize the first letter
            for (var i in context) {
                var word = context[i];
                word = word.toLowerCase();
                word = word.charAt(0).toUpperCase() + word.slice(1);
                context[i] = word;
            }

            return context.join(" "); 
        }
    });

    Handlebars.registerHelper('numberFormat', function (value, options) {
        // Helper parameters
        var dl = options.hash['decimalLength'] || 2;
        var ts = options.hash['thousandsSep'] || ',';
        var ds = options.hash['decimalSep'] || '.';

        // Parse to float
        var value = parseFloat(value);

        // The regex
        var re = '\\d(?=(\\d{3})+' + (dl > 0 ? '\\D' : '$') + ')';

        // Formats the number with the decimals
        var num = value.toFixed(Math.max(0, ~~dl));

        // Returns the formatted number
        return (ds ? num.replace('.', ds) : num).replace(new RegExp(re, 'g'), '$&' + ts);
    });
}

function fixStr(str) {
    var out = str.replace(/^\s*/, "");  // strip leading spaces
    out = out.replace(/^[a-z]|[^\s][A-Z]/g, function(str, offset) {
        if (offset == 0) {
            return(str.toUpperCase());
        } else {
            return(str.substr(0,1) + " " + str.substr(1).toUpperCase());
        }
    });
    return(out);
}

function slugify(text)
{
  return text.toString().toLowerCase()
    .replace(/\s+/g, "-")           // Replace spaces with -
    .replace(/[^\w\-]+/g, "")       // Remove all non-word chars
    .replace(/\-\-+/g, "-")         // Replace multiple - with single -
    .replace(/^-+/, "")             // Trim - from start of text
    .replace(/-+$/, "");            // Trim - from end of text
}

//$(document).ready(function () {

//    $("[data-table='true']").each(function () {

//        adminList.init();
//        adminList.ajaxLoad(false);

//    });

//});

function MessageTypeToStr(type) {
    switch (type.toString()) {
    case "0":
        return "Danger";
    case "1":
        return "Success";
    case "2":
        return "Info";
    case "3":
        return "Warning";
    }

    return "";
}

function trim (s, c) {
    if (c === "]") c = "\\]";
    if (c === "\\") c = "\\\\";
    return s.replace(new RegExp(
        "^[" + c + "]+|[" + c + "]+$", "g"
    ), "");
}

/* Image Upload Control Code */
//var finderIconImage;
//if (window.CKFinder)
//    finderIconImage = new CKFinder();

//function funSetImagePreview(elmt) {
//    console.log($('#' + elmt));
//    if ($('#' + elmt).val() !== '') {
//        $('#divPreview' + elmt).find('[data-img-preview]').attr('src', $('#' + elmt).val()).show();
//        $('#divPreview' + elmt).find('[data-file-icon]').hide();
//        $('#btnRemove' + elmt).prop('disabled', false);
//    } else {
//        $('#divPreview' + elmt).find('[data-img-preview]').attr('src', '').hide();
//        $('#divPreview' + elmt).find('[data-file-icon]').show();
//        $('#btnRemove' + elmt).prop('disabled', true);
//    }
//}

//function funSetImageUploadClickEvents(elmt) {
//    funSetImagePreview(elmt);
//    $('#btn' + elmt).click(function() {
//        finderIconImage.selectActionFunction = function(fileUrl) {
//            $('#' + elmt).val(fileUrl);
//            funSetImagePreview(elmt);
//        };
//        finderIconImage.popup();
//    });

//    $('#btnRemove' + elmt).click(function() {
//        if (!$('#divPreview' + elmt).has('[data-file-icon]')) return;
            
//        $('#' + elmt).val('');
//        funSetImagePreview(elmt);
//    });
//}

if (window.controller && window.action)
    $('[data-controller="'+controller+'"][data-action="'+action+'"]').parents('li').addClass('active');