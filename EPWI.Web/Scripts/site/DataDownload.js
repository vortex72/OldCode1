var SELECTION_LIMIT = 10;

$(function() {
    $('input.lineCB').click(function() {
        if ($('input.lineCB:checked').length > SELECTION_LIMIT) {
            alert('Only up to ' + SELECTION_LIMIT + ' lines may be selected.');
            return false;
        }
    });

    $('#SubmitLines').click(function() {
        if ($('input.lineCB:checked').length == 0) {
            alert('You must select at least one line.');
            return false;
        }
        displayProcessingBlock();
    });
});