$(function () {
    $('a.ViewSelectedInvoices').click(function () {
        $('#ViewMultipleInvoicesForm').submit();
        return false;
    });

    $('#ClearCheckboxes').click(function () {
        $('input:checkbox').prop('checked', false);
        return false;
    });
});