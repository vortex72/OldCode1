<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Components.Models.MillionthPartViewModel>" %>
<div>Guess when EPWI will sell its ONE MILLIONTH engine part online here at epwi.net.<br />Click on the PROGRAMS tab for details<br />GOOD LUCK!</div><br />
<% = Html.Hidden("CustomerID") %>
<% = Html.Hidden("CompanyCode") %>
<% = Html.Hidden("UserID") %>
<% = Html.Hidden("OrderNumber") %>
<% = Html.Hidden("ValidationCode") %>
<div>Date: <% = Html.TextBox("GuessDate", null, new { @class = "required date" }) %></div><br />
<div>Time: <% = Html.DropDownList("GuessHour", new SelectList(Model.Hours)) %>&nbsp;:
<% = Html.DropDownList("GuessMinute", new SelectList(Model.Minutes)) %>
<% = Html.DropDownList("GuessAmPm", new SelectList(new string[] { "AM", "PM" })) %></div><br />
<input type="submit" value="Submit Entry" id="SubmitEntry" />

