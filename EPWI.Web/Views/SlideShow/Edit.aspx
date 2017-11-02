<%@ import namespace='EPWI.Web.Utility' %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.Slideshow>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Slide
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Slide</h2>
    
    <% using (Html.BeginForm("Edit", "SlideShow", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
        <% = Html.Hidden("SlideShowID")%>

        <p>
            <% if (Model.SlideShowID > 0) { %><% = ImageHelper.Image(Url.Action("UncachedSlide", new { id = Model.SlideShowID }), new { width = 100 })%><% } %>
            <div>Upload new image: <input type="file" name="Image" /> <% = Html.ValidationMessage("Image") %></div> 
        </p>

        <p>
            Url: <% = Html.TextBox("Link")%> <% = Html.ValidationMessage("Link") %>
        </p>

        <p>
            External Link: <% = Html.CheckBox("ExternalLink")%>
        </p>

        <p>
            Enabled: <% = Html.CheckBox("Enabled")%>
        </p>

        <p>
            Registered Users Only: <% = Html.CheckBox("RegisteredOnly")%>
        </p>

        <input type="submit" value="Save" />&nbsp;&nbsp;
        <% = Html.ActionLink("Cancel and return to List", "Index")%>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
