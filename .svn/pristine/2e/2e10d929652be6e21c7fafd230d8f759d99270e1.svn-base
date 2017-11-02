<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitModelBase>" %>
<div class="sideContainer roundedCorners">
    <h2 class="roundedCorners">Kit Information</h2>
    <div class="roundedCornersBody"> 
        <div>Kit: <span class="em"><% = Html.Encode(string.IsNullOrEmpty(Model.Kit.AcesID) ? Model.Kit.KitIdentifier.KitPartNumber : Model.Kit.AcesID) %></span></div>
        <% if (!Model.Kit.IsKTRACK) { %>
            <% if(Model.Editing && !Model.AcesMode) { %>
                <div>Selected Year: <span class="em"><% = Model.Kit.SelectedYear %></span></div>
                <% if (Model.Kit.KitIdentifier.KitType != "CK") { %>
                    <div>Sizes: <a href="#" class="ChangeSize">[Change]</a></div>
                
                    <div>
                        <% if (Model.Kit.KitIdentifier.KitType != "RMK") { %>
                            <span>Bore: <span class="em"><% = Model.Kit.SelectedBoreSize.ToSizeCode()%></span></span>
                        <% } %>
                        <span>Rod: <span class="em"><% = Model.CrankKitSelected ? "CRK" : Model.Kit.SelectedRodBearingSize.ToSizeCode()%></span></span>
                        <% if (Model.Kit.KitIdentifier.KitType != "RR") { %>  
                            <div>
                                <span>Main: <span class="em"><% = Model.CrankKitSelected ? "CRK" : Model.Kit.SelectedMainBearingSize.ToSizeCode() %></span></span>
                                <span>Thrust: <span class="em"><% = Model.CrankKitSelected ? "CRK" : Model.Kit.SelectedThrustWasherSize.ToSizeCode() %></span></span>
                            </div>
                        <% } %>
                    </div>
                <% } %>
            <% } %>
            <div>Applications: 
                    <% foreach(string application in Model.Kit.Applications) { %>
                        <span class="em"><% = Html.Encode(application) %></span><br />
                    <% } %>
            </div>
            <div class="toggleSection">
                <div class="head">
                    <a href="#" class="toggleLink">Kit Details</a>
                </div>
                <div style="display:none" class="body">
                    <div>Cylinders: <span class="em"><% = Html.Encode(Model.Kit.CylinderString) %></span></div>
                    <div>Bore: <span class="em"><% = Html.Encode(Model.Kit.Bore) %></span></div>
                    <div>Stroke: <span class="em"><% = Html.Encode(Model.Kit.Stroke) %></span></div>
                    <div>Year: <span class="em"><% = Html.Encode(Model.Kit.StartYear) %>/<% = Html.Encode(Model.Kit.EndYear) %></span></div>
                    <div>Rod Diameter: <span class="em"><% = Html.Encode(Model.Kit.RodDiameter) %></span></div>
                    <div>Main Diameter: <span class="em"><% = Html.Encode(Model.Kit.MainDiameter) %></span></div>
                </div>
                &nbsp;
            </div>
        <% } %>
    </div>
    <div class="roundedCornersFooter"><p></p></div>
</div>