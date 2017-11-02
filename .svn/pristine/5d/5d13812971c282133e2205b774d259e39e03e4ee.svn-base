var addingPart;
var partBeingUpdated;
//var deselectionCount = 0;
var deselectionLimit;
var kitType;

$(function () {
    deselectionLimit = $('#deselectionLimit').val();
    kitType = $('#kitType').val();

    $(document).ajaxComplete(function () {
        hideProcessingBlock(false);
    });

    $(document).ajaxStart(function () {
        displayProcessingBlock();
    });

    $.ajaxSetup({ cache: false });

    updatePartMetadata();

    highlightGroups();

    //deselectionCount = getCategoryDeselectionCount();

    $('#btnAddToOrder').click(function () {
        if (checkDeselectionLimit(true)) {
            if ($('#ForceToOrder:checked').is(':checked')) {
                submitOrder(null, false, true);
            }
            else {
                if ($('input.unavailable:checked').length > 0) {
                    $('#AvailabilityIssues').dialog('open');
                }
                else if (($('#confirmingAvailability').val() === 'true') || allGroupsAddressed()) {
                    submitOrder(null);
                }
                else {
                    $('#UnaddressedGroups').dialog('open');
                }
            }
        }
    });

    $('#btnSaveAcesKit').click(function () {
        if (allGroupsAddressed()) {
            $(this).closest('form').submit();
        }
        else {
            $('#UnaddressedGroups').dialog('open');
        }
    });

    $(document).on('click', '#AddAsPreviouslyConfigured', '#AddAsPreviouslyConfigured', function () {
        submitOrder(null, true, true);
    });

    $('#AddToOrderCancel').click(function () {
        $('#UnaddressedGroups').dialog('close');
    });

    $('#AddToOrderOK').click(function () {
        $(this).closest('div.dialog').dialog('close');
        submitOrder(null);
    });

    $('#SaveAcesOK').click(function () {
        $(this).closest('div.dialog').dialog('close');
        $('#SubmitOrderForm').submit();
    });

    $(document).on('click', '#AddWarrantySubmit', function () {
        $(this).closest('div.dialog').dialog('close');
    });

    $('#AvailabilityOK').click(function () {
        $('#AvailabilityIssues').dialog('close');
    });

    $(document).on('click', 'tr.InterchangePart', function () {
        requestInterchange($('#OriginalPartUniqueIdentifier').val(), $(this).children('td.PartNumber').children('span.PartNIPC').text(), parseInt($(this).children('td.InterchangeQuantity').text()), parseInt($(this).children('td.PartNumber').children('span.QuantityOnHand').text()), $(this).children('td.PartNumber').children('span.InterchangeCode').text());
        return false;
    });

    $(document).on('mouseover', 'tr.InterchangePart', function () {
        $(this).addClass('highlighted');
    });

    $(document).on('mouseout', 'tr.InterchangePart', function () {
        $(this).removeClass('highlighted');
    });

    $('#AvailabilityIssues').dialog({ autoOpen: false, width: 300, title: 'Availability Issues', resizable: false });
    $('#UnaddressedGroups').dialog({ autoOpen: false, width: 300, title: 'Kit Configuration Incomplete', resizable: false });
    $('#InterchangeDialog').dialog({ autoOpen: false, width: 600, resizable: false, modal: true });
    $('#WarrantyDialog').dialog({ autoOpen: false, width: 600, resizable: false, modal: true, close: warrantyClosed });
    $('#KitSizeSelection').dialog({ autoOpen: false, width: 600, title: 'Size Selection', resizable: false, modal: true });
    $('#MissingParts').dialog({ width: 300, title: 'Notice', resizable: false, modal: true });

    $('a.ChangeSize').click(function () {
        $('#KitSizeSelection').dialog('open');
        return false;
    });

    $(document).on('click', 'input.MasterKitPart', function () {
        var checkbox = $(this);

        if (!checkbox.prop('checked')) {
            if (!checkDeselectionLimit()) {
                return false;
            }
        }

        // deselect all parts that are supposed to be deselected when this part is clicked
        if (checkbox.prop('checked')) {

            var partsToDeselect = checkbox.data('data').PartsToDeselect.split(',');
            if (partsToDeselect != '') {
                $(partsToDeselect)
                    .each(function() {
                        var partCheckBox = $('#' + this);
                        if (partCheckBox.prop('checked')) {
                            partCheckBox.prop('checked', false);
                        }
                    });
            }
        }

        // if the part is checked, also select all the other parts specified to be selected
        if (checkbox.prop('checked')) {
            var partsToSelect = checkbox.data('data').PartsToSelect.split(',');
            if (partsToSelect != '') {
                $(partsToSelect).each(function () {
                    var partCheckbox = $('#' + this);
                    if (!partCheckbox.prop('checked')) {
                        partCheckbox.prop('checked', true);
                    }
                });
            }
        }

        deselectionCount = getCategoryDeselectionCount();

        highlightGroups();

        var selectedGroupParts = new Array();
        // update the server with part selection data for each part in this part's group
        $('input.' + checkbox.data('data').GroupName + ':checked').each(function () {
            selectedGroupParts.push($(this).attr('id'));
        });

        $('#PriceAsConfigured').html('...');
        $('#PriceAsConfigured').load($('#ajaxBasePath').val() + '/UpdatePartSelections', { 'partSelections': selectedGroupParts, 'group': checkbox.data('data').GroupName });
        //alert(deselectionCount);
    });

    $(document).on('click', 'img.Substitute', function () {
        var part = $(this).nextAll('span.PartCheckbox').find('input.MasterKitPart');
        partBeingUpdated = part;
        parentRequestActive = true;
        displayProcessingBlock();

        if (!part.is(':checked')) {
            //part.attr('checked', true);
            part.trigger('click');
        }

        displayInterchangeDialog(part.data('data').UniqueKitPartIdentifier);
    });

    $(document).on('click', 'img.Revert', function () {
        var part = $(this).nextAll('span.PartCheckbox').find('input.MasterKitPart');
        partBeingUpdated = part;
        parentRequestActive = true;
        displayProcessingBlock();

        $.ajax({
            url: $('#ajaxBasePath').val() + "/RevertInterchange/" + part.data('data').UniqueKitPartIdentifier,
            cache: false,
            success: revertComplete,
            global: false,
            error: ajaxError
        });
    });

    $(document).on('click', '#SubmitInterchange', function () {
        if ($('#ShipmentOptions').valid()) {
            interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#InterchangeNIPC').val(), $('#InterchangeQuantity').val(), 'OtherWarehouse', $('input[name="warehouse"]:checked').val(), $('#InterchangeCode').val());
        }
    });

    $(document).on('click', '#DropshipInterchange', function () {
        interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#InterchangeNIPC').val(), $('#InterchangeQuantity').val(), 'DropShip', 'XXX', $('#InterchangeCode').val());
    });

    $(document).on('click', '#ShowAllInterchanges', function () {
        displayProcessingBlock();
        displayInterchangeDialog($('#OriginalPartUniqueIdentifier').val());
    });

    $(document).on('click', '#AddPartMain', function () {
        interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#RequestedItemNipc').val(), $('#ItemRequestedQuantity').val(), 'MainWarehouse', $('#DefaultWarehouse').val(), null);
        return false;
    });

    $(document).on('click', '#AddPartSecondary', function () {
        interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#RequestedItemNipc').val(), $('#ItemRequestedQuantity').val(), 'SecondaryWarehouse', $('#SecondaryWarehouse').val(), null);
        return false;
    });

    $(document).on('click', '#AddPartDropShip', function () {
        interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#RequestedItemNipc').val(), $('#ItemRequestedQuantity').val(), 'DropShip', 'XXX', null);
        return false;
    });

    $(document).on('click', '#AddPartOther', function () {
        if ($('input[name="warehouse"]:checked').val() === undefined) {
            $('#ShipmentOptionErrors').show();
        } else {
            interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#RequestedItemNipc').val(), $('#ItemRequestedQuantity').val(), 'OtherWarehouse', $('input[name="warehouse"]:checked').val(), null);
        }
        return false;
    });

    $(document).on('click', '#AddPartManual', function () {
        interchangePart($('#OriginalPartUniqueIdentifier').val(), $('#RequestedItemNipc').val(), $('#ItemRequestedQuantity').val(), 'Manual', 'XXX', null);
        return false;
    });

    $(document).on('click', '#CancelInterchange',  function () {
        $('#InterchangeDialog').dialog('close');
    });

    $(document).on('click', '#SubmitOrderWithWarehouse', function () {
        submitOrder($('#warehouse').val());
    });

    $(document).on('click', '#CheckAnotherPart', function (e) {
        // hack: for some reason FF wouldn't allow onsubmit to be triggered directly for this form. Using hidden submit button instead.
        $('#HiddenSubmit').click();
        return false;
    });

    $(document).on('click', '#ReplacementPart', function (e) {
        // if clicking on a replacement part for a superseded part, add the needed information to the form so 
        // the stock status is pre-populated with line, item number, quantity, and size
        $('#NewKitSearchContent').append('<input type="hidden" id="ReplacementItemNumber"/>');
        $('#NewKitSearchContent').append('<input type="hidden" id="ReplacementLineCode" />');
        $('#NewKitSearchContent').append('<input type="hidden" id="ReplacementQuantity" />');
        $('#NewKitSearchContent').append('<input type="hidden" id="ReplacementSize" />');
        $('#ReplacementItemNumber').attr({ name: 'RequestedItemNumber', value: $('#SupersededPartNumber').html() });
        $('#ReplacementLineCode').attr({ name: 'RequestedLineCode', value: $('#SupersededLineCode').html() });
        $('#ReplacementQuantity').attr({ name: 'RequestedQuantity', value: $('#ItemRequestedQuantity').val() });
        if ($('#RequestedSize').length > 0) {
            $('#ReplacementSize').attr({ name: 'RequestedSize', value: $('#RequestedSize').val() });
        }
        $('#HiddenSubmit').click();
        return false;
    });

    $('a.additionalPart').click(function () {
        displayProcessingBlock();
        $.ajax({
            url: $(this).attr('href'),
            cache: false,
            success: addAddlPartComplete,
            global: false,
            error: ajaxError
        });
        return false;
    });

    $(document).on('keypress', '.SearchField', function (e) {
        if (e.which == 13) {
            $('#KitSearch')[0].onsubmit();
            return false;
        }
    });

    $('#customerReference').val($('#previousCustomerReference').val());
});

function addAddlPartComplete(data) {
    $('#InterchangeDialog').html(data);
    addPartComplete();
}

function addPartComplete() {
    hideProcessingBlock();
    $('#InterchangeDialog').dialog('option', 'title', 'Add Part');
    $('#InterchangeDialog').dialog('open');
    $('#RequestedQuantity').focus();
}

function displayInterchangeDialog(id) {
    $.ajax({
        url: $('#ajaxBasePath').val() + "/InterchangePart/" + id,
        cache: false,
        success: showInterchangeDialog,
        global: false,
        error: ajaxError
    });
}

function revertComplete(html) {
    $('#PriceAsConfigured').html(html);
    updateCategory(partBeingUpdated.data('data').CategoryID);
}

function showInterchangeDialog(html) {
    $('#divProcessingBlock').hide();
    parentRequestActive = false;
    var dialog = $('#InterchangeDialog').html(html);
    dialog.html(html);
    dialog.dialog('option', 'title', 'Interchange Part');
    dialog.dialog('open');
}

function requestInterchange(originalPart, partNIPC, interchangeQuantity, quantityOnHand, interchangeType) {
    if (interchangeQuantity <= quantityOnHand) {
        interchangePart(originalPart, partNIPC, interchangeQuantity, 'MainWarehouse', $('#DefaultWarehouse').val(), interchangeType);
    }
    else {
        $.ajax({
            url: $('#ajaxBasePath').val() + "/InterchangeShipmentOptions/" + originalPart,
            data: 'interchangeNIPC=' + partNIPC + '&interchangeQuantity=' + interchangeQuantity + '&sizeCode=' + $('#RequestedSize').val() + '&interchangeCode=' + interchangeType,
            cache: false,
            success: showInterchangeShipmentOptions,
            global: true,
            error: ajaxError
        });
    }
}

function showInterchangeShipmentOptions(html) {
    $('#InterchangeDialog').html(html);
    addShipmentValidator();
}

function addShipmentValidator() {
    $('#ShipmentOptions').validate({
        rules: {
            warehouse: { required: true }
        },
        messages: {
            warehouse: 'Please select a warehouse'
        },
        errorLabelContainer: $('#ShipmentOptionErrors')
    });
}

function interchangePart(originalPart, partNIPC, interchangeQuantity, orderMethod, warehouse, interchangeType) {
    parentRequestActive = true;
    addingPart = (originalPart == null || originalPart == '');
    
    displayProcessingBlock();
    $.ajax({
        url: $('#ajaxBasePath').val() + "/InterchangePart/" + originalPart,
        cache: false,
        data: 'interchangeNIPC=' + partNIPC + '&interchangeQuantity=' + interchangeQuantity + '&orderMethod=' + orderMethod + '&warehouse=' + warehouse + '&confirmingAvailability= ' + $('#confirmingAvailability').val() + '&interchangeType=' + interchangeType,
        type: 'post',
        global: false,
        error: ajaxError,
        success: interchangeComplete
    });
}

function interchangeComplete(html) {
    $('#InterchangeDialog').dialog('close');
    $('#PriceAsConfigured').html(html);
    
    var category = 0;
    if (!addingPart) {
        category = partBeingUpdated.data('data').CategoryID;
    }
    
    updateCategory(category);
}

function categoryUpdateComplete(html) {
    if (addingPart) {
        $('div.AdditionalPartsDisplay').html(html);
        addingPart = false;
    }
    else {
        partBeingUpdated.closest('div.KitCategoryDisplay').html(html);
    }
    parentRequestActive = false;
    updatePartMetadata();
}

function updateCategory(categoryID) {
    $.ajax({
        url: $('#ajaxBasePath').val() + '/UpdateCategory/' + categoryID,
        cache: false,
        global: false,
        error: ajaxError,
        success: categoryUpdateComplete
    });
}

function submitOrder(warehouse, revert, force) {
    displayProcessingBlock();
    $.ajax({
        url: $('#ajaxBasePath').val() + '/AddKitToOrder',
        data: 'selectedWarehouse='+ warehouse
            + '&resultCode='+ $('#resultCode').val()
            + '&revert='+ revert
            + '&force='+ force
            + '&customerReference=' + encodeURIComponent($('#customerReference').val()),
        dataType: 'json',
        type: 'post',
        cache: false,
        global: false,
        success: orderSubmissionComplete,
        error: ajaxError
    });
}

function orderSubmissionComplete(data) {
    if (data.resultCode == 'N') {
        $.ajax({
            url: $('#ajaxBasePath').val() + "/GetWarranties/" + data.kitNipc,
            cache: false,
            data: 'orderItemID=' + data.orderID,
            type: 'post',
            global: false,
            error: ajaxError,
            success: showWarranties
        });
    }
    else {
        window.location.href = $('#ajaxBasePath').val() + '/Edit';
    }
}

function showWarranties(html) {
    $('#WarrantyDialog').html(html);
    if ($('#WarrantyCount').val() > 0)
    {
        $('#WarrantyDialog').dialog('option', 'title', 'Warranty Selection');
        $('#WarrantyDialog').dialog('open');
        hideProcessingBlock();
    }
    else
    {   // don't show warranty dialog if there aren't any warranties
        warrantyClosed();
    }
}

function warrantyClosed() {
    displayProcessingBlock();
    window.location.href = $('#stockStatusPath').val();
}

// (un)highlights part groups depending on if they are addressed or not
function highlightGroups() {
    if (!($('#confirmingAvailability').val() === 'true')) {
        $('div.group').each(function() {

            var groupParts = $(this).find('input.MasterKitPart');
            var partParents = groupParts.parent();
            var groupName = $(this).attr('id');

            if (groupParts.filter(':checked').length == 0) {
                partParents.addClass(groupName);
            }
            else {
                partParents.removeClass(groupName);
            }
        });
    }
}

function updatePartMetadata() {
    // get part data and attach it to the checkbox for each part
    $.getJSON($('#ajaxBasePath').val() + '/GetKitClientData', function(partData) {
        $(partData).each(function() {
            $('#' + this.UniqueKitPartIdentifier).data('data', this);
        });
    });
}

function allGroupsAddressed() {
    var allGroupsAddressed = true;
    $('div.group').each(function () {
        if (!isGroupAddressed($(this))) {
            allGroupsAddressed = false;
            return false;
        }
    });

    return allGroupsAddressed;
}

function checkDeselectionLimit(addingToOrder) {
    var deselectionCount = getCategoryDeselectionCount();
    var message;
    if ((addingToOrder && (deselectionCount > deselectionLimit) && !allGroupsAddressed()) || (!addingToOrder && (deselectionCount > deselectionLimit))/* && !($('#confirmingAvailability').val() === 'true')*/) {
        if (addingToOrder) {
            message = 'Parts have not been selected for the highlighted part groups. You must complete the highlighted groups before adding the kit to your order.';
        }
        else if (!addingToOrder) {
            if (deselectionLimit > 0) {
                message = 'Only a maximum of ' + deselectionLimit + ' items may be deselected from a ' + kitType + ' kit for ordering purposes.\n'
                if (!($('#confirmingAvailability').val() === 'true')) {
                    message += 'Please select another item if you wish to deselect this one.';
                }
                else if ($('#AddAsPreviouslyConfigured').length > 0) {
                    message += 'For assistance, click the "Add kit to order as previously configured and call me button.';
                }
            } else {
                if (kitType == '') {
                    message = "This kit";
                }
                else {
                    message = 'A ' + kitType + ' kit';
                }
                message += ' does not allow for any items to be deselected for ordering purposes.';
            }
        }
        alert(message);
        return false;
    }
    else {
        return true;
    }
}

function getCategoryDeselectionCount() {
    var categoriesDeselected = 0;

    // test each category to see if it is considered selected or not
    $('div.KitCategoryDisplay').not('.AdditionalPartsDisplay').each(function () {
        var category = $(this);
        // any unselected standalone part makes the category deselected
        if (category.find('input.standalone:not(:checked)').length > 0) { categoriesDeselected++; return true; }
        // any unaddressed group makes the category deselected
        if (categoryHasUnaddressedGroup(category)) { categoriesDeselected++; return true; }
        // any unselected part ANDed with a selected part makes the category deselected
        category.find('input.AndedPart:checked').each(function () {
            var andGroup = $(this).attr('data-andgroup');
            if (category.find('input.AndedPart:not(:checked)[data-andgroup="' + andGroup + '"]').length > 0) {
                categoriesDeselected++;
                return false;
            }
        });

    });
    return categoriesDeselected;
}

function categoryHasUnaddressedGroup(category)
{
    var hasUnaddressedGroup = false;
    category.find('div.group').each(function() { if (!isGroupAddressed($(this))) { hasUnaddressedGroup = true; return false; } });
    return hasUnaddressedGroup;
}

function isGroupAddressed(e) {
    return e.find('input.MasterKitPart:checked').length > 0;
}