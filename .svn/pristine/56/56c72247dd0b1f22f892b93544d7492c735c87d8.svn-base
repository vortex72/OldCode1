var dataTable;
var userDataSource;

var fireDT = function(resetRecordOffset) {
    var oState = dataTable.getState();


    dataTable.showTableMessage('Loading...');

    if (resetRecordOffset) {
        oState.pagination.recordOffset = 0;
    }

    var oCallback = {
        success: dataTable.onDataReturnSetRows,
        failure: dataTable.onDataReturnSetRows,
        argument: oState,
        scope: dataTable
    };

    // Generate a query string
    var request = dataTable.get("generateRequest")(oState, dataTable);

    // Fire off a request for new data.
    userDataSource.sendRequest(request, oCallback);
}

var booleanFormatter = function (elLiner, oRecord, oColumn, oData) {
    if (oRecord.getData(oColumn.key)) {
        elLiner.innerHTML = '<img src="../Content/images/check.gif" style="width:20px">';
    }
    else {
        elLiner.innerHTML = '';
    }
}

YAHOO.widget.DataTable.Formatter.boolean = this.booleanFormatter;

var requestBuilder = function(oState, oSelf) {
    var sort, dir, startIndex, results;

    oState = oState || { pagination: null, sortedBy: null };

    sort = (oState.sortedBy) ? oState.sortedBy.key : oSelf.getColumnSet().keys[0].getKey();
    dir = (oState.sortedBy && oState.sortedBy.dir === YAHOO.widget.DataTable.CLASS_DESC) ? "desc" : "asc";

    startIndex = (oState.pagination) ? oState.pagination.recordOffset : 0;
    results = (oState.pagination) ? oState.pagination.rowsPerPage : null;

    var querystring = "&results=" + encodeURIComponent(results) +
                "&sort=" + encodeURIComponent(sort) +
                "&dir=" + encodeURIComponent(dir) +
				"&startIndex=" + encodeURIComponent(startIndex) +
			    "&searchCriteria=" + encodeURIComponent($('#UserSearchValue').val()) +
                "&roleFilter=" + encodeURIComponent($('input[name="RoleFilter"]:checked').val());

    return querystring;
}


YAHOO.util.Event.addListener(window, "load", function () {

    var columnDefs = [
               { key: "UserID", hidden: true },
               { key: "UserName", label: "User Name", sortable: true },
               { key: "FirstName", label: "First Name", sortable: true },
               { key: "LastName", label: "Last Name", sortable: true },
               { key: "EmailAddress", label: "Email", sortable: true, hidden:true },
               { key: "CompanyName", label: "Company", sortable: true },
               { key: "CustomerID", label: "User ID", sortable: true },
               { key: "CompanyCode", label: "N/S", sortable: true },
               { key: "Customer", label: "Cust", sortable: true, formatter: "boolean" },
               { key: "CreateDate", label: "Create Date", sortable: true, formatter: YAHOO.widget.DataTable.formatDate, dateOptions: { format: '%m/%d/%y %I:%M %p'} }
            ];

    userDataSource = new YAHOO.util.XHRDataSource("GetUserList?");
    userDataSource.responseType = YAHOO.util.DataSource.TYPE_JSON;
    userDataSource.connXhrMode = "queueRequests";
    userDataSource.responseSchema = {
        resultsList: "data",
        fields: ["UserID", "UserName", "FirstName", "LastName", "EmailAddress", "CompanyName", "CustomerID", "CompanyCode", "Customer", { key: "CreateDate", parser: "date"}],
        metaFields: { recordCount: "recordCount" }
    };

    // DataTable configuration 
    var myConfigs = {
        initialRequest: "sort=UserName&dir=asc&startIndex=0&results=25", // Initial request for first page of data 
        initialLoad: false,
        dynamicData: true, // Enables dynamic server-driven data 
        sortedBy: { key: "CreateDate", dir: YAHOO.widget.DataTable.CLASS_DESC }, // Sets UI initial sort arrow 
        paginator: new YAHOO.widget.Paginator({ rowsPerPage: 25 }), // Enables pagination  
        selectionMode: "single",
        generateRequest: requestBuilder
    };

    dataTable = new YAHOO.widget.DataTable("UserList", columnDefs, userDataSource, myConfigs);
    dataTable.subscribe("rowMouseoutEvent", dataTable.onEventUnhighlightRow);
    dataTable.subscribe("rowMouseoverEvent", dataTable.onEventHighlightRow);
    dataTable.subscribe("rowClickEvent", function (oArgs) {
        var elTarget = oArgs.target;
        var oRecord = this.getRecord(elTarget);
        document.location = $('#EditUserUrl').val() + '/' + oRecord.getData('UserID');
    });

    dataTable.handleDataReturnPayload = function (oRequest, oResponse, oPayload) {
        oPayload.totalRecords = oResponse.meta.recordCount;
        return oPayload;
    }

    $('#Search').click(fireDT);

    $('input[name="RoleFilter"]').click(fireDT);

    $('input#UserSearchValue').keyup(function (e) {
        if (e.keyCode == 13) {
            $('#Search').click();
        }
    });
    fireDT();
});