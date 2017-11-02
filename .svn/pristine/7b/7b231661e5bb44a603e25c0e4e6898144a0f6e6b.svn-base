var dataTable;
var dataSource;

var fireDT = function(resetRecordOffset) {
    var oState = dataTable.getState(),
                	request,
                	oCallback;

    dataTable.showTableMessage('Loading...');

    if (resetRecordOffset) {
        oState.pagination.recordOffset = 0;
    }

    oCallback = {
        success: dataTable.onDataReturnSetRows,
        failure: dataTable.onDataReturnSetRows,
        argument: oState,
        scope: dataTable
    };

    // Generate a query string
    request = dataTable.get("generateRequest")(oState, dataTable);

    // Fire off a request for new data.
    dataSource.sendRequest(request, oCallback);
}

var requestBuilder = function(oState, oSelf) {
    var sort, dir, startIndex, results;

    oState = oState || { pagination: null, sortedBy: null };

    sort = (oState.sortedBy) ? oState.sortedBy.key : oSelf.getColumnSet().keys[0].getKey();
    dir = (oState.sortedBy && oState.sortedBy.dir === YAHOO.widget.DataTable.CLASS_DESC) ? "desc" : "asc";

    startIndex = (oState.pagination) ? oState.pagination.recordOffset : 0;
    results = (oState.pagination) ? oState.pagination.rowsPerPage : null;

    var querystring = "&results=" + results +
                "&sort=" + sort +
                "&dir=" + dir +
				"&startIndex=" + startIndex +
                "&searchCriteria=" + $('#CatalogSearchValue').val();

    return querystring;
}

var qs = (function (a) {
    if (a == "") return {};
    var b = {};
    for (var i = 0; i < a.length; ++i) {
        var p = a[i].split('=', 2);
        if (p.length == 1)
            b[p[0]] = "";
        else
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
    }
    return b;
})(window.location.search.substr(1).split('&'));

YAHOO.util.Event.addListener(window, "load", function () {

    this.formatUrl = function (elCell, oRecord, oColumn, sData) {
        var cellContent = "<a href='" + oRecord.getData("Url") + "' target='_blank'>" + sData;
        if (oRecord.getData("IsNew")) {
            cellContent += " - <strong>NEW</strong>";
        }
        cellContent += "</a>";
        elCell.innerHTML = cellContent;
    };

    var columnDefs = [
               { key: "ItemName", label: "Item", sortable: true, formatter: this.formatUrl, width: 325 },
               { key: "Line", label: "Line", sortable: true, width: 150 }
            ];

    dataSource = new YAHOO.util.XHRDataSource($('#BasePath').val() + "Catalog/GetCatalogItems?");
    dataSource.responseType = YAHOO.util.DataSource.TYPE_JSON;
    dataSource.connXhrMode = "queueRequests";
    dataSource.responseSchema = {
        resultsList: "data",
        fields: ["ItemName", "Line", "Url", "IsNew"],
        metaFields: { recordCount: "recordCount" }
    };

    // DataTable configuration 
    var myConfigs = {
        initialRequest: "sort=ItemName&dir=asc&startIndex=0&results=40", // Initial request for first page of data 
        initialLoad: false,
        dynamicData: true, // Enables dynamic server-driven data 
        sortedBy: { key: "ItemName", dir: YAHOO.widget.DataTable.CLASS_ASC }, // Sets UI initial sort arrow 
        paginator: new YAHOO.widget.Paginator({ rowsPerPage: 40 }), // Enables pagination  
        generateRequest: requestBuilder
    };

    dataTable = new YAHOO.widget.DataTable("Catalog", columnDefs, dataSource, myConfigs);

    dataTable.handleDataReturnPayload = function (oRequest, oResponse, oPayload) {
        oPayload.totalRecords = oResponse.meta.recordCount;
        return oPayload;
    }

    $('#Search').click(fireDT);

    $('#ResetSearch').click(resetSearch);

    $('input#CatalogSearchValue').keyup(function (e) {
        if (e.keyCode == 13) {
            $('#Search').click();
        }
    });

    fireDT();

    $('#disableDialog').click(function () {
        amplify.store('disableDialog', $(this).is(':checked'));
    });

    $('#disableDialog').attr('checked', amplify.store('disableDialog'));

    $("#catalogDialog").dialog({
        autoOpen: false,
        resizable: true,
        height: 600,
        width: 950,
        modal: true
    });

    // intercept catalog links, and open them in the dialog, as long as the override checkbox isn't checked
    $('.yui-dt-liner a').on('click', function () {        
        var linkAddr = getBaseURL() + $(this).attr("href");
        var linkText = $(this).html();
        amplify.store.sessionStorage('lastCatalogPage', { catalogUrl: [location.protocol, '//', location.host, location.pathname].join(''), pageTitle: linkText, pdfUrl: linkAddr });
        if (!$('#disableDialog').is(':checked')) {
            openCatalog(linkText, linkAddr);
            return false;
        }
        return true;
    });

    // if "restore" querystring key is present, automatically display the catalog page that was last viewed
    var restoreCatalog = qs["restore"];
    if (restoreCatalog) {
        var lastCatalogPage = amplify.store.sessionStorage('lastCatalogPage');
        if (lastCatalogPage) {
            openCatalog(lastCatalogPage.pageTitle, lastCatalogPage.pdfUrl);
        }
    }    
});

function getBaseURL() {
    return location.protocol + "//" + location.hostname +
      (location.port && ":" + location.port);
}

function resetSearch() {
    $('input#CatalogSearchValue').val('');
    fireDT();
}

function openCatalog(catalogTitle, catalogUrl) {
    $("#catalogDialog").dialog('option', 'title', catalogTitle);
    $("#catalogDialog").dialog('open');
    $("#catalogDialog").html('<object id="pdfDocument" data="' + catalogUrl +
        '" type="application/pdf" width="100%" height="97%">Looks like you don\'t have a PDF plug-in installed for this browser. <a href="' + catalogUrl + '">Click here</a> to download the file instead.</object>');        
}
