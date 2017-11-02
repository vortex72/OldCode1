$(function () {
    if (!$('#EliteHelp').length) {
        return;
    }
    $('#EliteHelp').dialog({ autoOpen: false, draggable: false, width: 500, title: 'Details', resizable: false });
    $('#EliteHelpLink').click(function () {
        $('#EliteHelp').dialog('open');
        return false;
    });

    $('#CustomerLookup').validate({
        rules: {
            CustomerID: { digits: true }
        },
        messages: {
            CustomerID: ''
        }
    });

    $('#GetStatement').validate({
        rules: {
            year: { minlength: 4 }
        },
        messages: {
            year: '*'
        }
    });

    $('input[name="searchselection"]:radio').click(function () {
        $('#InvoiceSearch').validate().resetForm();
        setValidation();
    });

    $('#InvoiceSearch').validate({
        rules: {
            invnum: { required: "#searchselection[value='number']:checked", number: true },
            invoiceDate: { required: "#searchselection[value='date']:checked", date: true },
            partNum: { required: "#searchselection[value='part']:checked" }
        },
        wrapper: 'div',
        focusInvalid: false,
        focusCleanup: true,
        messages: {
            invnum: 'Inv. Number invalid',
            partNum: 'Part Number required',
            invoiceDate: 'Valid Date required'
        },
        errorPlacement: function (error, element) { error.appendTo('#errorContainer'); }
    });

    $('#invnum').focus(function () {
        $('#searchselection[value="number"]').attr('checked', 'checked');
        $('#InvoiceSearch').validate().resetForm();
        setValidation();
    });

    $('#invoiceDate').click(function () {
        $('#searchselection[value="date"]').attr('checked', 'checked');
        $('#InvoiceSearch').validate().resetForm();
        setValidation();
    });

    $('#partNum').click(function () {
        $('#searchselection[value="part"]').attr('checked', 'checked');
        $('#InvoiceSearch').validate().resetForm();
        setValidation();
    });

    $('#invoiceDate').datepicker({
        beforeShow: function (input, inst) {
            inst.dpDiv.css({ marginLeft: input.offsetWidth + 'px' });
        },
        onSelect: function () {
            $('#InvoiceSearch').validate().resetForm();
        }
    });

    $('#searchInvoices').click(function () {
        if ($('#InvoiceSearch').valid()) {
            displayProcessingBlock();
            $('#InvoiceSearch').submit();
        }
        else {
            return false;
        }
    });

    $('input.invoicesearch').keypress(function (e) {
        if (e.which == 13) {
            $('#searchInvoices').click();
            return false;
        }
    });
});

function setValidation() {
    if ($("#searchselection[value='date']").is(':checked')) {
        $('#invoiceDate').rules('add', { date: true });
    }
    else {
        $('#invoiceDate').rules('remove', 'date');
    }

    if ($("#searchselection[value='number']").is(':checked')) {
        $('#invnum').rules('add', { number: true });
    }
    else {
        $('#invnum').rules('remove', 'number');
    }
}


