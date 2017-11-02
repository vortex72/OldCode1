<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitBuilderViewModel>" %>
<% using (Html.BeginForm((string)null, (string)null, new { id = Model.Kit.KitIdentifier.KitID, sizesSelected = !Model.ShowSizeDialog }, FormMethod.Post, new { id = "SelectTypeAndYear" })) { %>
    <div class="center">
        Year: <% = Html.DropDownList("SelectedYear", new SelectList(Model.Kit.GetKitYears()), "-YEAR-")%>
        Kit Type: <% = Html.DropDownList("SelectedKitType", new SelectList(KitType.KitTypes, "Type", "Description", Model.Kit.KitIdentifier.KitType))%><br />
        <input type="submit" value="Submit" id="SubmitYear" />
    </div>
<% } %>