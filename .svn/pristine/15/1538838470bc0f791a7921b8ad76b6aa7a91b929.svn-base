$(function () {

    $('#RegisterDialog').dialog({ width: 400, title: 'Notice', resizable: false, modal: true });
    $('#RegisterDialog').bind('dialogclose', cancelRegistration);

    $('#RegisterOk').click(function () {
        $('#RegisterDialog').unbind('dialogclose', cancelRegistration);
        $('#RegisterDialog').dialog('close');
    });

    $('#RegisterCancel').click(cancelRegistration);
});

function cancelRegistration() {
    window.location = $('#HomePageURL').val();
}
