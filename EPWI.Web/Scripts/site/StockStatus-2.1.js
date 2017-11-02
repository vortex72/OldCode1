// legacy functionality
$(function () {
    $('#divAvailableInterchanges').dialog({ autoOpen: false, width: 650, resizable: false });
    $('#ProductImage').dialog({ autoOpen: false, width: 310, title: 'Product Image', resizable: false });
    $('#OrderOptionsDialog').dialog({ autoOpen: false, width: 400, title: 'Order Options', resizable: false });
    $('#ShowInterchanges').click(function () { $('#divAvailableInterchanges').dialog('option', 'title', 'Available Interchanges for ' + $('#RequestedItemNumber').val() + ' ' + ($('#RequestedSize').length == 0 ? '' : $('#RequestedSize').val())); $('#divAvailableInterchanges').dialog('open'); return false; });
    $('#ViewImage').click(function () { $('#ProductImage').dialog('open'); return false; });
    $('#SwitchView').click(function () { $('#SwitchViewButton').click(); return false; });

    $(document).on('click', 'tr.InterchangePart', function () {
        document.location = $(this).find('.InterchangeUrl').val();
        return false;
    });

    $(document).on('mouseover', 'tr.InterchangePart', function () {
        $(this).addClass('highlighted');
    });

    $(document).on('mouseout', 'tr.InterchangePart', function () {
        $(this).removeClass('highlighted');
    });

    $('#AddToOrderLink').click(function () {
        if ($('input.OrderButton').length == 1) {
            $('input.OrderButton').click();
        } else {
            $('#OrderOptionsDialog').dialog('open');
        }
    });

    $('input.OrderButton').click(function () {
        var form = $(this).parents('form:first');
        if (form.valid()) {
            $('<input type="hidden" name="customerReference">').val($('#customerReference').val()).appendTo(form);
            displayProcessingBlock();
            return true;
        }
        return false;
    });

    if ($('#chkLocalPickup') != null) {
        $('#chkLocalPickup').click(function () { $('.localpickup').val($('#chkLocalPickup').is(':checked')); });
    }

    $('#WarehouseOptions1').validate({
        rules: {
            warehouse1: { required: true }
        },
        messages: {
            warehouse1: 'Please select a warehouse'
        },
        errorLabelContainer: $('#ShipmentOptionErrors')
    });


    $('#WarehouseOptions2').validate({
        rules: {
            warehouse2: { required: true }
        },
        messages: {
            warehouse2: 'Please select a warehouse'
        },
        errorLabelContainer: $('#ShipmentOptionErrors')
    });

    $('select').keypress(function (e) {
        if (e.which == 13) {
            $(this).next(':submit').click();
            return false;
        }
        return true;
    });

    if ($('select#RequestedLineCode').length > 0) {
        $('#RequestedLineCode').focus();
    }
    else if ($('select#RequestedSize').length > 0) {
        $('#RequestedSize').focus();
    }
    else {
        $('#RequestedQuantity').focus();
        $('#RequestedItemNumber').focus();
    }
});
	