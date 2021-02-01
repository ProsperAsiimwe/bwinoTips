


$(function () {

    $('#myBody').on('change', '.cart-disc', function () {
        Update($(this).data("ExclusiveTipId"), $(this).val(), 2, 1);
    });

    $('#myBody').on('change', '.cart-deduct', function () {
        Update($(this).data("ExclusiveTipId"), $(this).val(), 2, 0);
    });

    $('#myBody').on('change', '.cart-qty', function () {
        Update($(this).data("ExclusiveTipId"), $(this).val(), 1);
    });

    $('body').on('change', '#ExcludeDiscount', function () {
        var checkbox = $("#ExcludeDiscount");
        var value = true;
        if (checkbox.is(':checked')) {
            value = true;
        } else {
            value = false;
        }
        UpdateExclusion(value);
    });
    //$(".add-product").click(function () {
    $('body').on('click', '.add-product', function () {

        var value = $(this).closest("tr").find(".quantity").val();
        if (value > 0) {
            AddToCart($(this).data("exclusivetipid"), value);
            $(this).closest("tr").find(".quantity").val("1");
        } else {
            alert("Quantity must be greater 0!");
        }
    });

    $('body').on('click', '#formbtn', function () {
        var ExclusiveTipId = $("#FormExclusiveTipId").val();
        var value = $(this).closest("tr").find("#formquantity").val();
        if (value > 0 && ExclusiveTipId > 0) {
            AddTo(ExclusiveTipId, value);
        } else {
            alert("Quantity must be greater 0!");
        }
    });

    $('body').on('click', '#updatebtn', function () {
        var session = $("#SessionCode").val();
        var ExclusiveTipId = $("#FormExclusiveTipId").val();
        var supplierId = $("#FormSupplierId").val();
        var unitPx = $("#formunitpx").val();
        var quantity = $("#formquantity").val();
        var date = $("#Date").val();
        var importId = $("#ImportId").val();

        if (ExclusiveTipId <= 0) {
            alert("Please select a product");
        }
        if (supplierId <= 0) {
            alert("Please select a supplier");
        }
        if (unitPx <= 0) {
            alert("Please enter the purchase price");
        }
        if (quantity <= 0) {
            alert("Quantity must be greater 0!");
        }
        if (date === "") {
            alert("Please select a date");
        }

        if (importId === "") {
            importId = 0;
        }

        var url = "/StockUpdates/Add/" + ExclusiveTipId + "/" + supplierId + "/" + unitPx + "/" + quantity + "/" + date + "/" + session + "/" + importId;
        $.post(url, function (value) {
            if (jQuery.type(value) === "string" && value.indexOf("exceeds") >= 0) {
                alert(value);
            } else {
                $("#myBody").prepend(value["view"]);
                $("#FormExclusiveTipId").val("");
                $("#FormSupplierId").val("");
                $("#searchitem").val("");
                $("#searchsupplier").val("");
                $("#formunitpx").val("");
            }
            $("#formquantity").val("1");
        });

    });

    $("#total-discount").change(function () {
        Update(0, $(this).val(), 3, 0);
    });

});

function UpdateExclusion(exclude) {
    var url = "/Cart/Exclude/" + exclude;
    $.post(url, function (value) {
        $("#total-discount").val(value["discount"]);
        parseAmount(value["total"], "#cart-total");
    });

}
function AddToCart(ExclusiveTipId, quantity) {

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "showDuration": "800",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    var url;

    if (ExclusiveTipId > 0 && quantity > 0) {
        url = "/Cart/Add/" + ExclusiveTipId + "/" + quantity;
    }
    else {
        url = ""
    }
    //alert(ExclusiveTipId);
    $.post(url, function (value) {
        //alert(value);
            $('#cart_holder').html(value);
            toastr.success("Item added successfully.");
    });
}

function AddTo(ExclusiveTipId, qty) {

    var url;

    if (ExclusiveTipId > 0 && qty > 0) {
        url = "/Cart/AddItem/" + ExclusiveTipId + "/" + qty;
    }
    else {
        url = ""
    }

    $.post(url, function (value) {
        if (jQuery.type(value) === "string" && value.indexOf("exceeds") >= 0) {
            alert(value);
        } else {
            // check if any tr has product id, remove it, and replace with this one.
            $('#myBody tr').each(function () {
                var pdtId = $(this).data("ExclusiveTipId");
                if (parseInt(pdtId) === parseInt(ExclusiveTipId)) {
                    // delete item from table
                    $(this).remove();
                }
            });

            $("#myBody").prepend(value["view"]);
            parseAmount(value["total"], "#cart-total");
            $("#total-discount").val(value["discount"]);
            $("#FormExclusiveTipId").val("");
            $("#searchitem").val("");
            $("#formunitpx").val("");
        }
        $("#formquantity").val("1");
    });
}
function Update(ExclusiveTipId, value, type, percent) {

    var url;
    var updateDisc = 0;
    var oldQty = value;

    if (ExclusiveTipId > 0 && value > -1) {
        if (type === 1) {
            url = "/Cart/" + ExclusiveTipId + "/" + value + "/Qty";
            updateDisc = 1;
        } else {
            url = "/Cart/" + ExclusiveTipId + "/" + value + "/" + percent + "/Disc";
        }
    } else if (type === 3 && value > -1) {
        url = "/Cart/Discount/" + value;
    }
    else {
        url = ""
    }

    $.post(url, function (value) {
        if (jQuery.type(value) === "string" && value.indexOf("exceeds") >= 0) {
            alert(value);
        }
        else {
            if (ExclusiveTipId > 0) {
                parseAmount(value["cost"], "#total-" + ExclusiveTipId);
            }

            if (updateDisc === 1) {
                $("#total-discount").val(value["discount"]);
            }
            parseAmount(value["total"], "#cart-total");
        }

    });
}

function parseAmount(value, targetField) {
    $amount = $(targetField);

    // Clear the previous options 0776343436
    $($amount).empty();
    $($amount).html(value);

}
