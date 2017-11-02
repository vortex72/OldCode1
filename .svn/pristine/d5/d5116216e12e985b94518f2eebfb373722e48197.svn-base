<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LineDownloadViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Data Downloads
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Data Downloads</h2>
    <% using(Html.BeginForm()) { %>
    <div>Send product lines as:</div>
    <div><% = Html.RadioButton("FormatOption", 2, true) %>Separate Emails</div>
    <div><% = Html.RadioButton("FormatOption", 1) %>A Single Email</div>
    <br />
    
    <div style="height:300px;overflow:auto;width:50%;border:solid 1px">
    <table>
        <% int i = 0;  foreach (var row in Model.Lines)
           { %>
            <tr>
                <td><% = Html.CheckBox($"line[{i}].Value", new { @class = "lineCB" })%></td>
                <td><% = Html.Encode(row.LINED)%><% = Html.Hidden($"line[{i}].Key", row.LINE.Trim()) %></td>
            </tr>
        <% i++; } %>
    </table>
    </div>
        
    <div><input type="submit" value="Submit Request" id="SubmitLines" /></div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<% = Url.Content("~/Scripts/site/DataDownload.js") %>"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
