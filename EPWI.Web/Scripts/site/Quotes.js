var loadQuoteUrl;

$(function () {
    $('#OpenOrderDialog').dialog({ autoOpen: false, width: 400, title: 'Open Order Exists', resizable: false });
    $('#RequestHelpDialog').dialog({ autoOpen: false, width: 400, title: 'Request Help', resizable: false });

    $('a.DeleteQuote').click(function () {
        return confirm('Are you sure you want to delete this quote?');
    });

    $('a.LoadQuote').click(function () {
        if ($('#OpenOrderExists').val().toLowerCase() === 'true') {
            $('#OpenOrderDialog').dialog('open');
            loadQuoteUrl = $(this).attr('href');
            return false;
        } else {
            return true;
        }
    });

    $('#LoadQuote').click(function () {
        displayProcessingBlock();
        window.location.href = loadQuoteUrl;
    });

    $('#SearchByCustomerForm').validate({ messages: { CustomerID: { required: '*'}} });
    $('#SearchByQuoteIDForm').validate({ messages: { QuoteID: { required: '*'}} });
});

function showRequestHelp()
{
    $('#RequestHelpDialog').dialog('open');
}