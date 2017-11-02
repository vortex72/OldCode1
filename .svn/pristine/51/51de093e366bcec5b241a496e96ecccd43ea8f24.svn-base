<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>
<div id="KitPrompt" style="display:none" class="dialog">
    <% using(Html.BeginForm("View", "Kit")) { %>
        <% = Html.Hidden("id", Model) %>
        <div>You currently have a kit configuration file saved or in progress.</div>
        <div>What would you like to do with this file?</div>
        <div><% = Html.RadioButton("deleteSavedConfiguration", false, true, new { id = "deleteSavedConfigurationFalse" })%><label for="deleteSavedConfigurationFalse">Reload and continue working with my saved kit configuration</label></div>
        <div><% = Html.RadioButton("deleteSavedConfiguration", true, false, new { id = "deleteSavedConfigurationTrue" })%><label for="deleteSavedConfigurationTrue">Discard my saved configuration and customize this kit</label></div>
        <div class="buttons"><input type="submit" value="Submit" class="ShowIndicator" /></div>
    <% } %>
</div>
