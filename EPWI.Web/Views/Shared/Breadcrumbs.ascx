<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%--<% foreach(N2.ContentItem item in N2.Find.EnumerateBetween(N2.Find.StartPage, N2.Find.CurrentPage, false)) { %>
    <% if (!(N2.Find.CurrentPage.AnyParentIsType(typeof(MenuPage), true) && item.ID == N2.Find.StartPage.ID)) {  %>
        <% = N2.Web.Link.To(item) %> > 
    <% } %>
<% } %>
<% if (N2.Find.CurrentPage != null && N2.Find.CurrentPage.ID != N2.Find.RootItem.ID && !(N2.Find.CurrentPage is MenuPage)) { %><% = N2.Utility.Evaluate(N2.Find.CurrentPage, "Title") %><% } %>--%>
