$(function() {
    $('#KitPrompt').dialog({ autoOpen: false, draggable: true, title: 'Kit Configuration in Progress', resizable: false, width: 400, modal: true });
    $('#ConfigureKitLink').click(function() {
        $('#KitPrompt').dialog('open');
        return false;
    });
    $('div.dialog').find('form').submit(function() {
        $(this).closest('div.dialog').dialog('close');
    });
});