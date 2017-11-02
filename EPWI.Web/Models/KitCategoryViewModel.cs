using EPWI.Components.Models;
using System.Collections.Generic;

namespace EPWI.Web.Models
{
    public class KitCategoryViewModel
    {
        public Kit Kit { get; set; }
        public bool MasterKitCategory { get; set; }
        public IEnumerable<KitPart> CategoryParts { get; set; }
        public bool Editing { get; set; }
        public bool ConfirmingAvailability { get; set; }
        public FulfillmentProcessingResult FulfillmentProcessingResult { get; set; }
    }
}
