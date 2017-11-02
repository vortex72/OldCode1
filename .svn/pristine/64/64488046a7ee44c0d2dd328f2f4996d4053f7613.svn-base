<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KitCategoryViewModel>" %>
<% if (!Model.CategoryParts.Any()) { %>
    <div>&nbsp;</div>
<% } %>
<% var i = 0; %>
<% foreach(var part in Model.CategoryParts) { %>
    <% if (part.IsStartOfGroup) { %><div class="group" id="<% = part.GroupName %>"><% } %>
    <%  i++; %>
    <% if (i > 1 && !(Model.Editing && !Model.MasterKitCategory 
            && RelatedCategoryMapping.Mappings[RelatedCategory.CrankKit].CategoryID.Contains(part.CategoryID))) { %>
        <% if (part.JoinQualifier == "O") { %><div class="joinqualifier">--- OR ---</div><% } %>
        <% if (part.JoinQualifier == "A") { %><div class="joinqualifier">AND</div><% } %>
    <% } %>

    <div style="width:100%"><% = Html.KitPart(Model.Kit, part, Model.MasterKitCategory,  
    Model.Editing, Model.ConfirmingAvailability, Model.FulfillmentProcessingResult) %></div>
   <% if (part.IsEndOfGroup) { %></div><% } %>
<% } %>

