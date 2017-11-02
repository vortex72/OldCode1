<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WarrantyViewModel>" %>
<h3>Your kit has been added to your order.</h3>

<div>
    You also have the option to add a warranty to your kit at this time.<br />
    Please choose from one of the following warranty options and then click Continue.
</div>
<br />
<div>I would like to add...</div>
<% using(Html.BeginForm("AddWarranty", "Kit", new { id = Model.OrderItemID })) { %>
    <% = Html.Hidden("WarrantyCount", Model.Warranties.Count()) %>
    <% = Html.RadioButton("warranty", 0, true) %><span class="bold">NO WARRANTY</span> for an additional <span class="bold">$0.00</span> (Total Kit Price = <% = Model.KitPrice.Adjusted(Model.View, Model.CustomerData).ToString("C") %>) 
    <% foreach(var warranty in Model.Warranties) { %>
        <div>
            <% = Html.RadioButton("warranty", warranty.Nipc) %><span class="bold"><% = Html.Encode(warranty.Description) %></span>
            for an additional <span class="bold"><% = warranty.Price.Adjusted(Model.View, Model.CustomerData).ToString("C") %></span>
            (Total Kit Price = <% = (warranty.Price + Model.KitPrice).Adjusted(Model.View, Model.CustomerData).ToString("C") %>)
        </div>
    <% } %>
    <div class="buttons">
            <input type="submit" value="Continue" class="ShowIndicator" id="AddWarrantySubmit" />
    </div>
<% } %>


