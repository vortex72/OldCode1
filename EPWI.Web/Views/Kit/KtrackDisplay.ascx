<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitModelBase>" %>
<div class="KitCategoryDisplay">
    <% Html.RenderPartial("KitCategory", new KitCategoryViewModel
        {
            ConfirmingAvailability = Model.ConfirmingAvailability,
            FulfillmentProcessingResult = Model.FulfillmentProcessingResult,
            Editing = Model.Editing,
            Kit = Model.Kit,
            MasterKitCategory = true,
            CategoryParts = Model.Kit.GetCategoryParts(KitCategory.Ktrack)

        }); %>
</div>
