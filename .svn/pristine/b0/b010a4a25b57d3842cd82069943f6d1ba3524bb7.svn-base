<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitBuilderViewModel>" %>
<% using (Html.BeginForm(new { id = Model.Kit.KitIdentifier.KitID, SizesSelected = true, SelectedYear = Model.Kit.SelectedYear, SelectedKitType = Model.Kit.KitIdentifier.KitType} )) { %>
    <div>
        <div>Year: <% = Html.Encode(Model.Kit.SelectedYear) %></div>
        <% if (Model.Kit.KitIdentifier.KitType != "RMK" && Model.Kit.KitIdentifier.KitType != "CK") { %>
                Bore: <% = Html.DropDownList("SelectedBoreSize", new SelectList(Model.Kit.BoreSizes, Model.Kit.SelectedBoreSize), "STD", new { @class = "SizeSelection" })%>
        <% } %>
        <% if (Model.Kit.KitIdentifier.KitType != "CK") { %>   
                Rod Bearings: 
                    <% if (Model.CanSelectSize) { %>
                        <% = Html.DropDownList("SelectedRodBearingSize", new SelectList(Model.Kit.RodBearingSizes, Model.Kit.SelectedRodBearingSize), "STD", new { @class="NotForCrankKit SizeSelection" }) %>
                    <% } else if(Model.CrankKitSelected) { %>
                        <b>CRK</b>
                    <% } %>
        <% } %>

        <% if (Model.Kit.KitIdentifier.KitType != "CK" && Model.Kit.KitIdentifier.KitType != "RR") { %>   
                Main Bearings: 
                <% if (Model.CanSelectSize) { %>
                    <% = Html.DropDownList("SelectedMainBearingSize", new SelectList(Model.Kit.MainBearingSizes, Model.Kit.SelectedMainBearingSize), "STD", new { @class = "NotForCrankKit SizeSelection" })%>
                <% } else if(Model.CrankKitSelected) { %>
                    <b>CRK</b>
                <% } %>
                Thrust Washers: 
                <% if (Model.CanSelectSize) { %>
                    <% = Html.DropDownList("SelectedThrustWasherSize", new SelectList(Model.Kit.ThrustWasherSizes, Model.Kit.SelectedThrustWasherSize), "STD", new { @class = "NotForCrankKit SizeSelection" })%>
                <% } else if(Model.CrankKitSelected) { %>
                    <b>CRK</b>
                <% } %>
        <% } %>
    </div>
    <% if (!Model.SizesSelected && Model.Kit.HasCrankKit && !Model.Editing) { %>
        <div>
            Crank Kit: <% = Html.DropDownList("SelectedCrankKit", new SelectList(Model.Kit.GetRelatedCategoryParts(RelatedCategory.CrankKit), "NIPCCode", "PartString"), "NONE") %>
        </div>
    <% } %>
        <div>
            <input type="submit" value="Submit" class="ShowIndicator" />
        </div>
<% } %>