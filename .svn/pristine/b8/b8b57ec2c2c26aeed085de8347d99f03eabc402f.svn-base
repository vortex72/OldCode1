<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.Address>" %>
<div id="EditAddress" class="dialog">
    <div id="validationSummary"> </div>
    <% using (Ajax.BeginForm("UpdateShipToAddress", null, new AjaxOptions { UpdateTargetId = "ShipToAddress", OnBegin = "checkValidation", OnFailure = "handleError", OnSuccess="closeEditAddress" }, new { id = "UpdateShipToAddress" })) { %>
        <fieldset>
            <% =Html.ClientSideValidation<Address>().UseValidationSummary("validationSummary", "Please fix the following errors:") %>
            <p>
                <label for="UseSoldToAddress">
                    Same as Sold To?</label>
                <% = Html.CheckBox("UseSoldToAddress")%>
            </p>           
            <% Html.RenderPartial("ShipToAddress"); %>
            <div class="buttons">
                <input type="submit" value="Save" />
                <input type="button" value="Cancel" id="CancelEditAddress" />
            </div>
       </fieldset>
    <% } %>
</div>

