$(function () {
    $('#RequestHelpMessage').on('keyup', function () {
        limitChars('RequestHelpMessage', 240, 'RequestHelpMessageStatus');
    });

    $('#SubmitHelpRequest').on('click', function () {
        $('#RequestHelpForm').validate({
            rules: {
                CSR: { required: true }
            },
            messages: { CSR: '*' }
        });

        if (!$('#RequestHelpForm').valid()) { return false; }
    });
});
