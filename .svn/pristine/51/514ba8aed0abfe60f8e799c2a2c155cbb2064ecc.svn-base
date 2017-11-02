$(function () {
    $('#OrderShipMethod').change(function () {
        $('#SubmitChangeOrderShipMethod').click();
    });

    $('#SubmitPOChange').click(function () {
        $('#SetPOIndicator').show();
        $.ajax({ url: $('#SetPOAction').val(), type: 'post', data: ({ poNumber: $('#PONumber').val() }), error: handleJqueryAjaxError, complete: function () { $('#SetPOIndicator').hide(); } });
    });

   /* $('#SubmitNotesChange').on('click', function () {
        $('#SaveNotesIndicator').show();
        $.ajax({ url: $('#SaveNotesAction').val(), type: 'post', data: ({ orderNotes: $('#OrderNotes').val() }), error: handleJqueryAjaxError, complete: function () { $('#SaveNotesIndicator').hide(); } });
    });*/

    $('#ProcessOrderButton').on('click', function () {
        $('#HiddenPO').val($('#PONumber').val());
        $('#HiddenNotes').val($('#OrderNotes').val());
        $('#ProcessOrderForm').submit();
    });

    $('#UseSoldToAddress').click(function () {
        var inputControls = $('#EditAddress input:text,#EditAddress select');
        var checked = $(this).attr('checked');

        if (checked) {
            var validator = $('#UpdateShipToAddress').validate();
            validator.resetForm();
            inputControls.removeClass('input-validation-error');
        }
        inputControls.each(function () {
            if (checked) { $(this).val(''); }
            $(this).attr('disabled', checked);
        });
    });

    $('#OrderNotes').on('keyup', function () {
        limitChars('OrderNotes', 120, 'NotesStatus');
    });

    $('#EditAddress').dialog({ autoOpen: false, draggable: false, width: 500, title: 'Edit Ship To Address', show: 'slide', hide: 'slide', resizable: false });

    $('#SaveOrderAsQuote').dialog({ autoOpen: false, width: 425, height: 480, title: 'Save Order as Quote', resizable: false, modal: true, close: saveOrderAsQuoteClosed });

    $('#QuoteWarning').dialog({ autoOpen: true, width: 400, title: 'Notice', resizable: false, modal: true });

 /*   $('a.EditCustomerReference').on('click', function () {
        toggleCustomerReferenceElements($(this));
        return false;
    });

    $('#toggleCustomerReference').click(function () {
        toggleCustomerReference();
        return false;
    });*/

    $('#ChangeShipToAddress').click(function () { $('#EditAddress').dialog('open'); return false; });

    $('#CancelEditAddress').click(closeEditAddress);

    $('img.ChangeLink').on('click', function () {
        if ($('#SavedKitConfigurationID').val() != '0' && $(this).hasClass('ChangeKit')) {
            if (!confirm('You currently have a kit configuration in progress.\nChanging this kit will overwrite the kit configuration currently in progress.\nClick OK to continue or Cancel to abort.')) {
                return false;
            }
        }

        displayProcessingBlock();
        var form = $(this).siblings('form.Change');
        form.submit();
        return false;
    });

    $('img.DeleteLink').on('click', function () {
        $(this).siblings('form.Delete').find('#DeleteSubmit').click();
        return false;
    });

    $('input#DeleteOrderButton').on('click', function () {
        if (confirm('Are you sure you want to delete this order?')) {
            $('#DeleteOrderForm').submit();
        }
    });

    $('input#SaveOrderAsQuoteButton').on('click', function () {
        displayProcessingBlock();
        $('#HiddenQuotePO').val($('#PONumber').val());
        $('#HiddenQuoteNotes').val($('#OrderNotes').val());
        $('#ExistingQuotes').load($('#GetQuotesAction').val(), getQuotesComplete);
        $('#QuoteDescription').val('');
    });

    $('#ExistingQuotes tr').on('click', function () {
        $('#QuoteDescription').val($(this).find('.QuoteDescription').text());
        $('#SaveQuote').click();
    });

    $('#ExistingQuotes tr').on('mouseover', function () {
        $(this).addClass('highlighted');
    });

    $('#ExistingQuotes tr').on('mouseout', function () {
        $(this).removeClass('highlighted');
    });

/*    $('input.UpdateCustomerReference').on('click', function () {
        var parent = $(this).parents('div.CustomerReference');
        var orderItemId = parent.attr('data-order-item-id');
        var customerReference = parent.find('input:text').val();
        parent.find('input').attr('disabled', 'disabled');
        $.ajax({ url: $('#UpdateCustomerReferenceAction').val(), type: 'post', data: ({ id: orderItemId, customerReference: customerReference }), complete: function () { updateCustomerReferenceComplete(parent); }, success: function (html) { updateCustomerReferenceSuccess(parent, html); } });
    });*/

    $('#SaveQuoteForm').validate({
        rules: {
            QuoteDescription: { required: true }
        },
        messages: {
            QuoteDescription: '*'
        }
    });

    $('#SaveQuote').click(function () {
        var bypassValidation;
        // don't validate the form if we're overwriting the existing quote
        if ($('#overwriteQuoteOption').is(':checked')) {
            $("#SaveQuoteForm").validate().cancelSubmit = true;
            bypassValidation = true;
        }

        var form = $(this).parents('form:first');
        if (form.valid() || bypassValidation) {
            displayProcessingBlock();
            return true;
        }
        return false;
    });

    $('#PostQuoteProcessOrder').on('click', function () {
        $('#ProcessOrderButton').click();
    });

    if ($.browser.msie) {
        $('input[name="SaveOption"]').click(function () {
            this.blur();
            this.focus();
        });
    }

    $('input[name="SaveOption"]').change(function () {
        //alert('toggling');
        $('div.togglePanel').toggle();
        setQuoteListHeight();
    });


    //millionth part code
    $('#MillionthPartForm').validate();
    $('#MillionthPartDialog').dialog({ autoOpen: false, draggable: false, width: 500, title: 'Millionth Part Contest', resizable: false, open: function () { $('#GuessDate').datepicker('enable'); }, close: function () { $('#GuessDate').datepicker('disable'); } });
    $('#GuessDate').datepicker({ duration: '' }).datepicker('disable');
    $('#MillionthPartDialog').dialog('open');
});

function beginSaveOrderAsQuote() {
    if ($('#SaveQuoteForm').validate().form()) {
        displayProcessingBlock();
        return true;
    } else {
        return false;
    }
}

function getQuotesComplete() {
    $('#SaveOrderAsQuote').dialog('open');
    setQuoteListHeight();
    hideProcessingBlock();
}
    
function checkValidation() {
    return $('#UpdateShipToAddress').valid();
}

function closeEditAddress() {
    $('#EditAddress').dialog('close');
}

function setQuoteListHeight() {
    $('#quoteList').height(475 - $('#quoteList').position().top);
}

function saveOrderAsQuoteClosed() {
    if ($('#QuoteSaved').length > 0 && $('#QuoteSaved').val().toLowerCase() === 'true')
    {
        displayProcessingBlock();
        window.location = $('#StockStatusURL').val();
    }
}

function limitChars(textid, limit, infodiv) {
    var text = $('#' + textid).val();
    var textlength = text.length;

    if (textlength > limit) {
        $('#' + infodiv).html('Limit is ' + limit + ' characters!');
        $('#' + textid).val(text.substr(0, limit));
        return false;
    }
    else {
        $('#' + infodiv).html((limit - textlength) + ' characters left.');
        return true;
    }
}

// millionth part code
function contestEntryComplete() {
    $('#MillionthPartDialog').dialog('close');
    $('#ContestInfo').html('<div class="StatusMessage">Your entry has been received. Thanks for playing!</div>');
}

function beginMillionthPartEntry() {
    if ($('#MillionthPartForm').validate().form()) {
        displayProcessingBlock();
        return true;
    } else {
        return false;
    }
}

function itemDeleteSuccess() {
    if ($('.customer-reference-hidden').is(':visible')) {
        // if customer reference is set to hidden, hide the customer references on the order details since it's been refreshed
        $('#OrderDetails').find('.customer-reference-toggle').hide();
    }
}

/*function toggleCustomerReference() {
    $('.customer-reference-toggle').toggle();
}

function updateCustomerReferenceComplete(parent) {
    parent.find('input').removeAttr('disabled');
}

function updateCustomerReferenceSuccess(target, html) {
    target.html(html);
}

function toggleCustomerReferenceElements(src) {
    $(src).parents('.CustomerReference').find('.EditCustomerReference').slideToggle();
}*/