$(function() {
    jQuery.validator.addMethod("phoneUS", function(phone_number, element) {
        phone_number = phone_number.replace(/\s+/g, "");
        return this.optional(element) || phone_number.length > 9 &&
		    phone_number.match(/^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/);
    }, "Please specify a valid phone number");

    jQuery.validator.addMethod(
        "multiemail",
         function (value, element) {
             if (this.optional(element)) // return true on optional element 
                 return true;
             var emails = value.split(/[;,]+/); // split element by , and ;
             var valid = true;
             for (var i in emails) {
                 value = emails[i];
                 valid = valid &&
                         jQuery.validator.methods.email.call(this, $.trim(value), element);
             }
             return valid;
         },
        jQuery.validator.messages.email + ' Separate multiple e-mail addresses with commas.'
    );

    $("#CustomerID").change(function () {
        $(".customerIDhidden").val($("#CustomerID").val());
    });

    $("#CompanyCode").change(function () {
        $(".companyCodehidden").val($("#CompanyCode").val());
    });

    $('a.EmailPage').click(function() {
        $('#EmailDialog').dialog('open');
        $('input#EmailRecipientAddress').focus();
        return false;
    });

    $('a.FaxPage').click(function() {
        $('#FaxDialog').dialog('open');
        $('input#FaxRecipientName').focus();
        return false;
    });

    $('#EmailDialog').dialog({ width: 450, resizable: false, modal: true, title: 'Email Page', autoOpen: false });
    $('#FaxDialog').dialog({ width: 450, resizable: false, modal: true, title: 'Fax Page', autoOpen: false });

    $('#SendEmail').click(function() {
        if ($('#EmailForm').valid()) {
            displayProcessingBlock();
            var content = new String($('div.pageContent').html());
            $.ajax({
                url: $('#siteBasePathEmail').val() + 'Email/SendContentEmail',
                success: function() { resetEmailDialog(); $('#EmailDialog').dialog('close'); hideProcessingBlock(); },
                error: function() { alert('There was an error sending the email. Please try again. If the problem persists, please contact EPWI.'); hideProcessingBlock(); },
                data: { data: content, fromName: $('#EmailSenderName').val(), fromAddress: $('#EmailSenderAddress').val(), toAddress: $('#EmailRecipientAddress').val(), subject: $('#EmailSubject').val(), notes: $('#EmailBody').html() + '\r\n' + $('#EmailNotes').val() },
                type: 'POST',
                global: false,
                cache: false
            });
        }
    });

    $('#SendFax').click(function() {
        if ($('#FaxForm').valid()) {
            displayProcessingBlock();
            var content = new String($('div.pageContent').html());
            $.ajax({
                url: $('#siteBasePathFax').val() + 'Email/SendContentFax',
                success: function() { resetFaxDialog(); $('#FaxDialog').dialog('close'); hideProcessingBlock(); },
                error: function() { alert('There was an error sending the fax. Please try again. If the problem persists, please contact EPWI.'); hideProcessingBlock(); },
                data: { data: content, fromName: $('#FaxSenderName').val(), recipientName: $('#FaxRecipientName').val(), recipientCompany: $('#FaxRecipientCompany').val(), toFaxNumber: $('#FaxRecipientNumber').val(), subject: $('#FaxSubject').val(), includeCoverSheet: $('#FaxIncludeCoverSheet').is(':checked'), notes: $('#FaxNotes').val() },
                type: 'POST',
                global: false,
                cache: false
            });
        }
    });

    $('input.cancel').click(function() {
        resetFaxDialog();
        resetEmailDialog();
        $(this).closest('div.dialog').dialog('close');
        $(this).closest('form').validate().resetForm();
    });

    $('#EmailForm').validate({
        rules: {
            EmailSenderAddress: { required: true, email: true },
            EmailRecipientAddress: { required: true, multiemail: true }

        },
        messages: {
            EmailSenderAddress: { required: 'Your Email Address is required' },
            EmailRecipientAddress: { required: 'Recipient Address is required' }
        },
        errorLabelContainer: $('#emailValidationSummary'),
        wrapper: 'div'
    });

    $('#FaxForm').validate({
        rules: {
            FaxRecipientNumber: { required: true, phoneUS: true }
        },
        messages: {
            FaxRecipientNumber: { required: 'Recipient Fax Number is required', phoneUS: 'Recipient Fax Number is invalid' }
        },
        errorLabelContainer: $('#faxValidationSummary'),
        wrapper: 'div'
    });

    $('form.default input').keypress(function(e) {
        if (e.which == 13) {
            $(this).closest('form').find('input.default').click();
            return false;
        }
    });

});

function resetFaxDialog() {
    resetForm('#FaxForm');
    $('#FaxSubject').val($('#FaxDefaultSubject').html());
}

function resetEmailDialog() {
    resetForm('#EmailForm');
    $('#EmailSubject').val($('#EmailDefaultSubject').html());
}

function resetForm(formSelector) {
    $(':input', formSelector)
         .not(':button, :submit, :reset, :hidden, .exclude')
        .val('')
        .removeAttr('checked')
        .removeAttr('selected');
}