<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>    
<p>
    <label for="Name">
        Name:</label>
    <% = Html.TextBox("Name", null, new { maxlength = "30", @class="name" })%>
</p>
<p>
    <label for="StreetAddress1">
        Address:</label>
    <% = Html.TextBox("StreetAddress1", null, new { maxlength = "30", @class="streetaddress" })%>
</p>
<p>
    <label for="City">
        City, State ZIP:</label>
    <% = Html.TextBox("City", null, new { maxlength = "20", @class="city" })%>
    <% = Html.DropDownList("State", new SelectList(Constants.States), "--", null) %>
    <% = Html.TextBox("Zip", null, new { maxlength = "5", @class="zip" })%>-<% = Html.TextBox("Zip4", null, new { maxlength = "4", @class="zip4" })%>
</p>
<p>
    <label for="Phone">
        Phone:</label>
    <% = Html.TextBox("Phone", null, new { maxlength = "12", @class="phone" })%>
</p>


