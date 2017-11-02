<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<EPWI.Components.Models.Slideshow>>" %>
<div class="sideContainer slideshowContainer">
    <div id="slideshow">
        <% var firstSlide = true; foreach (var slide in Model) { %>
        <div class="center" style="<% if (!firstSlide) { %>display:none<% } else { %><%}%>">
            <a target="<% = slide.ExternalLink ? "_blank" : "_self" %>" href="<% = slide.Link  %>" class="slideshowLink"><img src="<% = Url.Action("Slide", new { id = slide.SlideShowID }) %>" class="slideshow" /></a><br />
        </div>
        <% firstSlide = false; } %>
    </div>
</div>
