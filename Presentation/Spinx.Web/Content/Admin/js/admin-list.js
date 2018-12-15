(function ($) {

    var pageSize = 10;

    $.fn.adminList = function () {
        this.each(function () {
            var $main = $(this);
            var $table = $main.find("[data-main-table]");
            if ($table.size() === 0) return;

            var tableType = $table.data('table-type');

            if (tableType === 'select') {

                // no searching
                // no sorting

                // header checkbox for select 
                // action for remove selected records
                // ajax call and sequancing

                // no history push

            } else if (tableType === 'model') {


                // searching
                // sorting
                setSortingClass($table);
                setFormSearch($table, ajax);

                // header checkbox for select 
                // no actions

                // ajax call for update selection and update select list
                setModelSelectEvent($main);

                // no history push
            } else { // tableType default 'table' normal


                setSortingClass($table);
                setFormSearch($table, ajax);

            }

            hidePaging($main);
            setSelectCheckboxes($table);
            btnSubmitActions($main, ajax);
            ajax();

            function ajax(action, deleteId) {
                var url = $table.data("url") || window.location.href;
                var sortColumn = $table.data("sort-column") || "";
                var sortType = $table.data("sort-type") || "";
                var page = $table.data("page") || 1;
                var size = $table.data("size") || pageSize;

                var extraData = "action=" + (action || "");
                extraData += "&ids=" + (deleteId ||
                    $table.find("tbody").find("input:checked").map(function () { return this.value; }).get()
                        .join(","));
                extraData += "&sortColumn=" + sortColumn;
                extraData += "&sortType=" + sortType;
                extraData += "&page=" + page;
                extraData += "&size=" + size;
                extraData += getSerializeValues($table);

                $.ajax({
                    url: url,
                    type: "POST",
                    data: extraData,
                    success: function (result, status, xhr) {
                        var ct = xhr.getResponseHeader("content-type") || "";
                        if (ct.indexOf("json") <= -1) return;

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
                        $.showHeaderMessages(result, $main);
                        
                        if (!result.Success) return;

                        // Reset Sorting
                        setSortingClass($table);

                        // remove header checkbox
                        $table.find("thead tr").eq(1).find("input:checkbox").prop("checked", false);

                        // other table load based on current table changes for data table select and model
                        if (tableType === "select" && action === "remove") {
                            $("table[data-table-type='model']").parents("[data-table='true']").adminList();
                        }

                        

                        if (result.Data.length > 0) {

                            // Sorting - on click set arrow for sorting
                            $table.find('thead tr th[data-name="' + sortColumn + '"]')
                                .removeClass("sorting sorting_asc sorting_desc")
                                .addClass("sorting_" + sortType)
                                .attr("data-sort-type", sortType);

                            $table.find('thead tr').last().children("[data-name]").click(function () {

                                $table.data("sort-column", $(this).attr("data-name"));
                                $table.data("sort-type", $(this).attr("data-sort-type") === "desc" ? "asc" : "desc");
                                $table.data("page", 1);

                                ajax();
                            });

                            // Header pagging show
                            $main.find(".dt-toolbar").show();

                            // Page size set & page size click bind
                            if (result.Paging.Size > 0) {
                                $main.find('[data-page="true"]').show().val(result.Paging.Size)
                                    .unbind("change").change(function () {
                                        $table.data("size", $(this).val());
                                        ajax();
                                    });
                            } else {
                                $main.find('[data-page="true"]').hide();
                            }

                            // Set records information
                            $main.find(".paging-view-text .text-primary").html(result.Paging.Total);
                            $main.find(".pagination-info span").html("Page " + result.Paging.Page + " of " + Math.ceil(result.Paging.Total / result.Paging.Size));

                            /* Paging Event */
                            if (result.Paging.Size > 0) {
                                $main.find(".pagination").show();

                                if (result.Paging.Page !== 1) {
                                    $main.find(".pagination li.previous").removeClass("disabled");
                                    $main.find(".pagination li.previous a").unbind("click").click(function () {
                                        $table.data("page", result.Paging.Page - 1);
                                        ajax();
                                    });
                                } else {
                                    $main.find(".previous").removeClass("disabled").addClass("disabled");
                                    $main.find(".previous a").unbind("click");
                                }

                                if (result.Paging.Page < Math.ceil(result.Paging.Total / result.Paging.Size)) {
                                    $main.find(".pagination li.next").removeClass("disabled");
                                    $main.find(".pagination li.next a").unbind("click").click(function () {
                                        $table.data("page", result.Paging.Page + 1);
                                        ajax();
                                    });
                                } else {
                                    $main.find(".next").removeClass("disabled").addClass("disabled");
                                    $main.find(".next a").unbind("click");
                                }
                            } else {
                                $main.find(".pagination").hide();
                            }

                        } else {

                            /* pagging hide */
                            $main.find(".dt-toolbar").hide();

                            $main.find(".previous, .next").removeClass("disabled").addClass("disabled");
                            $main.find(".previous a, .next a").unbind("click");
                        }

                        /* Template Bind */
                        var source = $main.find("#tplData").html();
                        var template = Handlebars.compile(source);
                        var html = template(result);
                        $table.find("tbody").html(html);

                        /* After template bind code*/

                        // tooltip
                        if ($main.find("[rel=tooltip]").length) {
                            $main.find("[rel=tooltip]").tooltip();
                        }

                        //expand
                        $main.find(".expand").parent().next().hide();
                        $main.find(".expand").click(function () {
                            if ($(this).parent().next().is(":visible")) {
                                $(this).parent().next().hide();
                                $(this).find("i").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                            } else {
                                $(this).parent().next().show();
                                $(this).find("i").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                            }
                        });

                        // checkbox 
                        setSelectCheckboxes($table);

                        // list delete click event
                        $table.find("tbody").find("[data-delete=true]").click(function () {
                            var id = $(this).attr("data-id");

                            $.confirm({
                                text: "Do you want to delete record?",
                                title: "Confirmation required",
                                confirmButtonClass: "btn-danger",
                                confirm: function (button) {
                                    ajax("delete", id);
                                },
                                cancel: function (button) { }
                            });
                        });

                        // list remove click event
                        $table.find("tbody").find("[data-remove=true]").click(function () {
                            var id = $(this).attr("data-id");

                            $.confirm({
                                text: "Do you want to remove record?",
                                title: "Confirmation required",
                                confirmButtonClass: "btn-danger",
                                confirm: function (button) {
                                    ajax("remove", id);
                                },
                                cancel: function (button) { }
                            });
                        });

                        if (typeof $table.attr("data-sequence-url") !== typeof undefined && $table.attr("data-sequence-url") !== false)
                        {
                            $main.find("tbody").sortable({
                                cursor: "n-resize",
                                handle: ".drag",
                                opacity: 0.7,
                                revert: true,
                                update: function() {

                                    var ids = jQuery.map(
                                        $main.find('tbody').find('[type="checkbox"]').map(function() {
                                            return $(this).val();
                                        }), function(n) {
                                            return (n);
                                        }).join(",");

                                    $.post($table.data('sequence-url'), "data=[" + ids + "]");
                                }
                            });
                            $main.find("tbody").disableSelection();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr);
                    }
                });
            }
        });
    };

    function setModelSelectEvent($main) {
        $main.parents(".modal-body").siblings(".modal-footer").find(".btn-primary").unbind('click').click(function() {
            
            var ids = jQuery.map(
                $main.find('tbody').find('input:checked').map(function() {
                    return $(this).val();
                }), function(n) {
                    return (n);
                }).join(",");

            var url = $main.find('table').data("model-save-url");
            $.post(url, "data=[" + ids + "]", function() {
                $("[data-table='true']").adminList();

                $('#modelSelect').modal('toggle');
            });
        });
    }

    function hidePaging($main) {
        $main.find(".dt-toolbar").hide();
        $main.find(".previous, .next").removeClass("disabled").addClass("disabled");
        $main.find(".previous a, .next a").unbind("click");
    }

    function setSortingClass($table) {
        $table.find('thead tr').last().children("[data-name]")
            .removeClass("sorting sorting_asc sorting_desc").addClass("sorting").unbind("click");
    }

    function initDatetimePlugin($table) {
        $table.find("thead tr").eq(0).find(".datepicker").each(function () {

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
    }

    function setSelectCheckboxes($table) {
        var trIndex = $table.find("thead tr").size() === 2 ? 1 : 0;

        $table.find("thead tr").eq(trIndex).find("input:checkbox").attr("autocomplete", "off")
            .unbind("click").click(function () {
                if ($(this).is(":checked"))
                    $table.find("tbody").find('input[type="checkbox"]').prop("checked", true);
                else
                    $table.find("tbody").find('input[type="checkbox"]').prop("checked", false);
            });
    }

    function btnSubmitActions($main, ajax) {
        $main.find("#btnSubmitAction").click(function () {

            $main.clearMessagesInForm();

            if ($main.find("#selAction").val() === "") {
                $main.find("[data-msg]").alert("Select at least one item from action list.", "info");
            } else {
                if ($("#selAction").val() === "export") {
                    document.location.href = window.location.href + (window.location.href.indexOf("?") === -1 ? "?" : "") + "&m=export";
                } else if ($main.find("tbody").find("input:checked").size() === 0) {
                    $main.find("[data-msg]").alert("Select at least one item from list.", "info");
                } else if ($("#selAction").val() === "delete") {
                    $.confirm({
                        text: "Do you want to delete selected record(s)?",
                        title: "Confirmation required",
                        confirmButtonClass: "btn-danger",
                        confirm: function(button) {
                            ajax("delete");
                        },
                        cancel: function(button) {}
                    });
                }  else if ($("#selAction").val() === "remove") {
                    $.confirm({
                        text: "Do you want to remove selected record(s)?",
                        title: "Confirmation required",
                        confirmButtonClass: "btn-danger",
                        confirm: function(button) {
                            ajax("remove");
                        },
                        cancel: function(button) {}
                    });
                } else { // if ($('#selAction').val() === 'active' || ($('#selAction').val() === 'inactive'))
                    ajax($("#selAction").val());
                }
            }
                
        });
    }

    function setFormSearch($table, ajaxCallback) {

        // Only one in head of table means no search then return
        const headRowsCount = $table.find("thead tr").size();
        if (headRowsCount < 2) return;

        // Textbox for enter key event
        $table.find("thead tr").eq(0).find("input").bind("keypress",
            function (event) {
                if (event.which === 13) {
                    event.preventDefault();

                    ajaxCallback();
                }
            });

        // Dropdown for change event
        $table.find("thead tr").eq(0).find("input, select").bind("change",
            function (event) {
                event.preventDefault();

                ajaxCallback();
            });

        initDatetimePlugin($table);
    }



}(jQuery));

$(document).ready(function () {
    $("[data-table='true']").adminList();
});