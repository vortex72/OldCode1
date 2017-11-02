<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.UserEditViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create User
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create User</h2>

    <div id="validationSummary">
    </div>
    <% using (Html.BeginForm()) {%>
        <% Html.RenderPartial("UserProfile", Model.UserProfile); %>
        <% Html.RenderPartial("UserAdminSettings", Model.UserAdminSettings); %>
        
        <p>
            <input type="submit" value="Create User" />
        </p>
    <% } %>
</asp:Content>


