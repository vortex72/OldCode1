var parentRequestActive = false;

$(function () {
    $('ul.sf-menu').supersubs().superfish({ speed: 0, delay: 400, pathLevels: 0 });

    $('#slideshow').cycle();

    $(document).on('click', 'input.ShowIndicator', function () {
        displayProcessingBlock();
    });

    $(document).on('click', 'a.ShowIndicator', function () {
        displayProcessingBlock();
    });

    $(document).ajaxError(ajaxError);

    $('#UpdateCustomerIDDialog').dialog({ autoOpen: false, width: 250, title: 'Update Customer ID', resizable: false, modal: true });

    $(document).on('keypress', '#UpdateCustomerIDDialog input,#UpdateCustomerIDDialog select', function (e) {
        if (e.which == 13) {
            $(this).closest('form').submit();
            return false;
        }
    });

    $('.numeric').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    $('.UpdateCustomerID').click(function () {
        $('#UpdateCustomerIDDialog').dialog('open');
        $('#UpdateCustomerIDText').focus().select();
    });

    $('#UpdateCustomerIDForm').validate({
        rules: {
            CustomerID: { required: true }
        },
        messages: {
            CustomerID: { required: '*' }
        }
    });

    $('#InvoiceByNumberForm').validate({
        rules: {
            InvoiceNumber: { required: true }
        },
        messages: {
            InvoiceNumber: { required: '*' }
        }
    });

    $('input.overwrite').focus(function () {
        $(this).select();
    });

    $(document).on('click', 'input.close', function () {
        $(this).closest('div.dialog').dialog('close');
    });

    $('input.infield').each(function () {
        var tb = $(this);
        tb.val(tb.attr('title'));
        tb.addClass('infieldlabel');
        tb.focus(function () {
            if (tb.val() === tb.attr('title')) {
                tb.val('');
                tb.select();
            }
            tb.removeClass('infieldlabel');
        });
        tb.blur(function () {
            if (tb.val() === '') {
                tb.val(tb.attr('title'));
                tb.addClass('infieldlabel');
            }
        });
    });

    $('#StockStatusSearchIcon').click(function () {
        $('#StockStatusWidget input.Search').click();
    });

    $('#StockStatusWidget input.Search').click(function () {
        var tb = $('#StockStatusWidget #RequestedItemNumber');
        if (tb.val() == tb.attr('title')) tb.val('');
        displayProcessingBlock();
    });

    $('#StockStatusWidgetForm').submit(function () {
        displayProcessingBlock();
    });

    $('.toggleSection .toggleLink').click(function () {
        $(this).parent().next().toggle('slow');
        //$('.toggleSection .body').toggle('slow');
        return false;
    });

    $(document).on('click', 'a.PrintPage', function () {
        window.print();
        return false;
    });

    // if the user has visited a catalog page, display a link to return to it
    var lastCatalogPage = amplify.store.sessionStorage('lastCatalogPage');
    if (lastCatalogPage) {
        var url = lastCatalogPage.catalogUrl + '?restore=true';
        $('#CatalogLink').html($('<a></a>').attr('href', url).html('Return to ' + lastCatalogPage.pageTitle));
    }

});

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

function displayProcessingBlock() {
    var pageHeight;

    if (window.innerHeight && window.scrollMaxY) // Firefox 
    {
        pageHeight = window.innerHeight + window.scrollMaxY;
    }
    else if (document.body.scrollHeight > document.body.offsetHeight) // all but Explorer Mac
    {
        pageHeight = document.body.scrollHeight;
    }
    else // works in Explorer 6 Strict, Mozilla (not FF) and Safari
    {
        pageHeight = document.body.offsetHeight + document.body.offsetTop;
    }

    $('#divProcessingBlock').css('height', pageHeight/* - 5*/);
    $('#divProcessingBlock').css('display', 'block');
}

function ajaxError(request, textStatus, errorThrown) {
    hideProcessingBlock(true);
    console.log(errorThrown);
    //alert('There was an error communicating with the server. Please refresh this page in your browser. Status code: ' + request.status);
}

function hideProcessingBlock(force) {
    if (!parentRequestActive || force) {
        $('#divProcessingBlock').hide();
    }
}

function handleError(ajaxContext) {
    alert('Sorry, the request failed with status code ' + ajaxContext.get_response().get_statusCode());
}

function handleJqueryAjaxError() {
    alert('Sorry, the request failed.');
}

function displayStatusMessage(message) {
    $('#StatusMessage').html(message).show();
}
