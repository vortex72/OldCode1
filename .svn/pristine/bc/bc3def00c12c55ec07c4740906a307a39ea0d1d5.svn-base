<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EPWI.Components.Models.Statement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Statement Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="80%">
            <h2>Statement Detail</h2>
          </td>
          <td align="right">
            <% if (User.IsInRole("EMPLOYEE")) { %>
                <a href="#" class="FaxPage">Fax Page</a>&nbsp;
            <% } %>
            <a href="#" class="PrintPage">Print Page</a>
          </td>
        </tr>
    </table>
    
    <% Html.RenderPartial("StatementContent"); %>
    <% Html.RenderPartial("FaxDialog", Model.CurrentUser); %>
    <div id="FaxDefaultSubject" style="display:none">Statement Detail for <% = Html.Encode(Model.StatementDate.ToString("MM/yyyy")) %></div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<% = Url.Content("~/Scripts/Statement.js") %>"></script>
    <script src="<% = Url.Content("~/Scripts/site/PageActions.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
