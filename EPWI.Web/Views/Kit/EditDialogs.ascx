<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="InterchangeDialog" class="dialog">
</div>
<div id="WarrantyDialog" class="dialog">
</div>
<div id="UnaddressedGroups" class="dialog">
    Warning: Parts have not been selected for the highlighted part groups.
    Click 'OK' to add this kit to your order anyway, or click cancel to
    continue building this kit.
    <br /><br />
    <div class="buttons"><input id="AddToOrderOK" type="button" value="OK" /><input id="AddToOrderCancel" type="button" value="Cancel" /></div>
</div>

<div id="AvailabilityIssues" class="dialog">
Items that are highlighted in red are not available. You need to either uncheck these items indicating that you do not wish to receive them, interchange them,
or revert them. Alternatively, you can check the box below the Add to Order button to add the kit to your order as is and request manual processing.
    <div class="buttons"><input id="AvailabilityOK" type="button" value="OK" /></div>
</div>