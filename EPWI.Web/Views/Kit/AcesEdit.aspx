<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Web.Models.KitBuilderViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ACES Kit Builder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>ACES Kit Builder</h2>
    <h3>Configuring Kit <% = Html.Encode(ViewData["NewKitNumber"]) %></h3>
    <input type="hidden" value="<%= Url.Content("~/Kit/") %>" id="ajaxBasePath" />
    <% = Html.Hidden("deselectionLimit", 99 /* Unlimited deselections for ACES */) %>
    <% Html.RenderPartial("KitDisplay", Model); %>
    <div id="UnaddressedGroups" class="dialog">
    Warning: Parts have not been selected for the highlighted part groups.
    Click 'OK' to save this kit configuration anyway, or click cancel to
    continue building this kit.
    <br /><br />
    <div class="buttons"><input id="SaveAcesOK" type="button" value="OK" /><input id="AddToOrderCancel" type="button" value="Cancel" /></div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link href="<% = Url.Content("~/Content/css/kits.css") %>" rel="Stylesheet" />
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/site/KitEdit-2.1.js") %>">   
    </script>
    <script type="text/javascript">
        $(function() {
            $('#Notes').keyup(function() {
                limitChars('Notes', 80, 'NotesStatus');
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
    <div id="KitHeader" class="widget">
        <% Html.RenderPartial("KitHeader"); %>
    </div>
    <div class="widget">
        <div class="sideContainer roundedCorners">
            <h2 class="roundedCorners">ACES Options</h2>
                <% using (Html.BeginForm("AcesSave", "Kit", FormMethod.Post, new { id = "SubmitOrderForm" })) { %>
                <div class="roundedCornersBody">     
                        Notes:<br />
                        <% = Html.TextArea("Notes")%><div id="NotesStatus"></div><br />
                        <% = Html.Hidden("NewKitNumber", ViewData["NewKitNumber"]) %>
                        
                </div>
                <div class="roundedCornersFooter"><p><input id="btnSaveAcesKit" type="button" value="Save ACES Kit" /></p></div>
                <% } %>
        </div>
    </div>
</asp:Content>
