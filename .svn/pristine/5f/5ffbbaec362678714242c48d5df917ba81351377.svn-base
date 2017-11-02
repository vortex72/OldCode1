<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Catalog
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="<% = Url.Content("~/Content/images/Powered-By.png")%>" alt="Powered by OptiCat" style="float: right; width: 100px;" />
    <p style="display: none" data-bind="visible: years()">
        <label>
            Year:
        </label>
        <select data-bind="options: years, value: selectedYear, optionsCaption: '- Year -'">
        </select>
    </p>
    <p style="display: none" data-bind="visible: selectedYear() && makes().length > 0">
        <label>
            Make:
        </label>
        <select data-bind="options: makes, optionsText: 'Name', optionsValue: 'ID', value: selectedMake, optionsCaption: '- Make -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: selectedMake() && models().length > 0">
        <label>
            Model:
        </label>
        <select data-bind="options: models, optionsText: 'Name', optionsValue: 'ID', value: selectedModel, optionsCaption: '- Model -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: askSubmodel">
        <label>
            Submodel:</label>
        <select data-bind="options: submodels, optionsText: 'Name', optionsValue: 'ID', value: selectedSubmodel, optionsCaption: '- Submodel -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: askEngineConfig">
        <label>
            Engine Configuration:</label>
        <select data-bind="options: engineConfigurations, optionsText: 'Description', optionsValue: 'ID', value: selectedEngineConfig, optionsCaption: '- Engine Configuration -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: selectedEngineConfig() != null && categories().length > 0">
        <label>
            Category:
        </label>
        <select data-bind="options: categories, optionsText: 'Name', optionsValue: 'ID', value: selectedCategory, optionsCaption: '- Category -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: selectedCategory() && subcategories().length > 0">
        <label>
            Subcategory:
        </label>
        <select data-bind="options: subcategories, optionsText: 'Name', optionsValue: 'ID', value: selectedSubcategory, optionsCaption: '- Subcategory -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: selectedSubcategory() && productLines().length > 0">
        <label>
            Product Line:
        </label>
        <select data-bind="options: productLines, optionsText: 'ProductLineName', optionsValue: 'ID', value: selectedProductLine, optionsCaption: '- Product Line -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: selectedProductLine() && selectedProductLine() === -1 && selectedSubcategory() < 40000 && partTypes().length > 0">
        <label>
            Part Type:
        </label>
        <select data-bind="options: partTypes, optionsText: 'Name', optionsValue: 'ID', value: selectedPartType, optionsCaption: '- Part Type -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    <p style="display: none" data-bind="visible: askPosition() && selectedPartType() >= 0 && qualifyingPositions() && qualifyingPositions().length > 0">
        <label>
            Position:
        </label>
        <select data-bind="options: qualifyingPositions, optionsText: 'Name', optionsValue: 'ID', value: selectedPosition, optionsCaption: '- Position -', optionsAfterRender: removeExtraWhiteSpace">
        </select>
    </p>
    

    <div data-bind="template: { name: 'pager-template'}"></div>           

    <table id="parts" class="standard" style="display: none" data-bind="visible: parts().length">
        <tbody data-bind="foreach: parts">
            <tr>
                <td data-bind="partViewer: $data, partViewerOptions: { showPartNumber: true, showPartTypeDescription: $root.selectedProductLine() > 0 } "></td>
            </tr>
        </tbody>
    </table>
    <div data-bind="template: { name: 'pager-template'}" style="margin-bottom:10px;margin-top:5px"></div>           
    <div style="display: none" data-bind="visible: noPartsFound()" class="error">
        No matching parts.
    </div>
    
    <!--
    <pre data-bind="text: ko.toJSON($data, null, 2)" style="width: 100%; border: none;"></pre>
    -->
    
    <script type="text/html" id="pager-template">
        <ul class="pagination" style="display:none" data-bind="visible: parts().length">
            <li><a href="#" data-bind="click:firstPage, css: { disabled: currentPage() === 1 }">&lt;&lt; First Page</a></li>
            <li><a href="#" data-bind="click:prevPage, css: { disabled: currentPage() === 1 }">&lt; Prev Page</a></li>
            <li>Viewing parts <span data-bind="text: startItem"></span> - <span data-bind="text: endItem"></span> of <span data-bind="text: partCount"></span></li>
            <li><a href="#" data-bind="click:nextPage, css: { disabled: currentPage() === pageCount() }">&gt; Next Page</a></li>
            <li><a href="#" data-bind="click:lastPage, css: { disabled: currentPage() === pageCount() }">&gt;&gt; Last Page</a></li>
        </ul> 
    </script>
    <% Html.RenderPartial("PartViewer"); %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FirstScriptContent" runat="server">
    <style type="text/css">
        #parts {
            width: 100%;            
        }
        
        label {
            display: block;
        }
        
        #content {
            overflow: auto;
        }
    </style>
    <script src="<%= Url.Content("~/Scripts/underscore.min.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery-3.0.0.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.2.1.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.12.0-rc.2.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.colorbox-min.js")%>" type="text/javascript"></script>
    <link href="<%= Url.Content("~/Content/colorbox.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/css/Smoothness/jquery-ui-1.12.0-rc.2.css") %>" rel="stylesheet" type="text/css" />
    

    <script type="text/javascript">
        //var newjQuery = jQuery.noConflict();
    </script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-3.0.0.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.colorbox-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/modernizr-2.8.3.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/site/PartViewer.js?v=9") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/site/lookup.js?v=16") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SideBarContent" runat="server">
</asp:Content>
