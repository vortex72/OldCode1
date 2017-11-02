$(function() {
    $('#SelectTypeAndYear').validate({
        rules: {
            SelectedYear: 'required'
        },
        messages: {
            SelectedYear: '*'
        }
    });

    $('#SelectedCrankKit').change(function() {
        if ($('#SelectedCrankKit').val() != '') {
            $('select.NotForCrankKit').val('STD');
            $('select.NotForCrankKit').attr('disabled', 'disabled');
        }
        else {
            $('select.NotForCrankKit').removeAttr('disabled');
        }
    });

    $('#TypeAndYearSelection').dialog({ width: 400, resizable: false, modal: true, title: 'Select Kit Type And Year' });

    $('#KitSizeSelection').dialog({ width: 600, resizable: false, modal: true, title: 'Kit Configuration' });

    $('#SubmitYear').click(function() {
        if ($('#SelectTypeAndYear').valid()) {
            displayProcessingBlock();
            return true;
        } else {
            return false;
        }
    });

    $('#ConfigureKitYear').click(function() {
        $('#TypeAndYearSelection').dialog('open');
    });

    $('#ConfigureKitSize').click(function() {
        $('#KitSizeSelection').dialog('open');
    });

    $('div.dialog').find('form').submit(function() {
        $(this).closest('div.dialog').dialog('close');
    });
});