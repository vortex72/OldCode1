<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPWI.Web.Models.KitModelBase>" %>

<table width="700px" class="borderless">
  <tr> 
    <td>
      <% if (!Model.AcesMode) { %>
      <table width="100%">
        <tr> 
          <td width="1%" nowrap="nowrap" align="center" class="kittype"><% = Html.KitLink(new KitIdentifier { KitID = Model.Kit.KitIdentifier.KitID, KitType = "MK" }) %></td>
          <td align="center">&nbsp;
            
          </td>
        </tr>
      </table>
      <% } %>
      <table id="main" cellpadding="2" cellspacing="0" class="container">
        <tr> 
          <td align="center" width="1%" rowspan="4" class="kitsection border">M<br/>A<br/>S<br/>T<br/>E<br/>R<br/>&nbsp;<br/>K<br/>I<br/>T</td>
          <td align="center" width="1%" rowspan="2" class="kitsection border">E<br/>N<br/>G<br/>I<br/>N<br/>E<br/>&nbsp;<br/>K<br/>I<br/>T</td>
          <td align="center" width="1%" colspan="2" class="kitsection border">RE-<br/>RING<br/>KIT</td>
          <td align="center" colspan=2>
            <table cellspacing="0" cellpadding="3" width="100%" class="bottomborder">
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Rings<br/> </td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.Rings) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing, Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.Rings) }); %>
                    </div>
                </td>
                <td align="right" valign="top" width="1%">
                  <% if (!Model.AcesMode) { %>
                  <table cellspacing="0" cellpadding="0" width="1%" >
                    <tr> 
                      <td align="center" class="kittype" nowrap="nowrap"><% = Html.KitLink(new KitIdentifier { KitID = Model.Kit.KitIdentifier.KitID, KitType = "RR" }) %></td>
                    </tr>
                  </table>
                  <% } %>
                </td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Rod Bearings</td>
                <td width="99%" colspan="2" class="kitdata">
                    <% if (Model.Kit.SelectedCrankKitNIPC > 0) { %>
                        Included in Crank Kit                        
                    <% } else { %>
                        <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.RodBearings) %>
                        <div class="KitCategoryDisplay">
                            <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.RodBearings) }); %>
                        </div>
                    <% } %>
                </td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Gasket Set</td>
                <td width="99%" colspan="2" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.GasketSet) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.GasketSet) }); %>
                    </div>                        
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr> 
          <td align="center" colspan="4">
            <table cellspacing="0" cellpadding="3" width="100%" class="bottomborder">
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Pistons<br/> </td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.Pistons) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.Pistons) }); %>
                    </div>
                </td>
                <td align="right" width="1%">
                  <% if (!Model.AcesMode) { %>
                  <table cellspacing="0" cellpadding="0" width="1%" >
                    <tr> 
                      <td align="center" class="kittype" nowrap="nowrap"><% = Html.KitLink(new KitIdentifier { KitID = Model.Kit.KitIdentifier.KitID, KitType = "EK" }) %></td>
                    </tr>
                  </table>
                  <% } %>
                 </td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Main Bearings</td>
                <td width="99%" class="kitdata">
                    <% if (Model.Kit.SelectedCrankKitNIPC > 0) { %>
                        Included in Crank Kit
                    <% } else { %>
                        <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.MainBearings) %>
                        <div class="KitCategoryDisplay">
                            <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.MainBearings) }); %>                        
                        </div>
                    <% } %>
                </td>
                <td align=right width="1%">&nbsp;</td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Cam Bearings</td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.CamBearings) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.CamBearings) }); %>
                    </div>
                </td>
                <td align="right" width="1%">&nbsp;</td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Oil Pump</td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.OilPump) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.OilPump) }); %>
                    </div>
                </td>
                <td align="right" width="1%">&nbsp;</td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Freeze Plugs</td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.FreezePlugs) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.FreezePlugs) }); %>
                    </div>
                </td>
                <td align="right" width="1%">&nbsp;</td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Pin Bushings</td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.PinBushings) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.PinBushings) }); %>
                    </div>
                </td>
                <td align="right" width="1%">&nbsp;</td>
              </tr>
            </table>
          </td>
        </tr>
        <tr> 
          <td align="center" colspan="2" class="kitsection bottomborder rightborder" width="1%">CAM<br/>KIT</td>
          <td align="center" colspan="3">
            <table cellspacing="0" cellpadding="3" width="100%" class="bottomborder">
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Camshaft<br/> </td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.Camshaft) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.Camshaft) }); %>
                    </div>
                </td>
                <td align="right" width="1%">
                  <% if (!Model.AcesMode) { %>
                  <table cellspacing="0" cellpadding="0" width="1%" >
                    <tr> 
                      <td align="center" class="kittype" nowrap="nowrap"><% = Html.KitLink(new KitIdentifier { KitID = Model.Kit.KitIdentifier.KitID, KitType = "CK" }) %>
                      </td>
                    </tr>
                  </table>
                  <% } %>
                </td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Lifters</td>
                <td width="99%" class="kitdata">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.Lifters) %>
                    <div class="KitCategoryDisplay">
                        <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.Lifters) }); %>
                    </div>
                </td>
                <td align="right" width="1%">&nbsp;</td>
              </tr>
            </table>
          </td>
        </tr>
        <tr> 
          <td align="center" colspan="4" class="kitsection rightborder" width="1%">TIMING&nbsp;KIT</td>
          <td align="center">
            <table cellspacing="0" cellpadding="2" width="100%" class="bottomborder">
              <tr> 
                <td class="kitdata bottomborder">
                    <% = Html.KitCategoryNoteLabel(Model.Kit, KitCategory.TimingKit) %>
                    <div>
                        <div class="KitCategoryDisplay">
                            <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, MasterKitCategory = true, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.TimingKit) }); %>
                        </div>
                    </div>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr> 
          <td align="center" colspan="5" class="kitsection border" width="1%">SPECIALTY&nbsp;KITS</td>
          <td align="center">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr> 
                <td>
                  <table cellspacing="0" cellpadding="2" width="100%" class="bottomborder">
                    <tr> 
                      <td width="1%">
                        <% if (!Model.AcesMode) { %>
                        <table cellspacing="0" cellpadding="0" width="1%" >
                          <tr> 
                            <td align="center" class="kittype" nowrap="nowrap"><% = Html.KitLink(new KitIdentifier { KitID = Model.Kit.KitIdentifier.KitID, KitType = "RRP" }) %></td>
                          </tr>
                        </table>
                        <% } %>
                      </td>
                      <td>RE-RING KIT + MAIN BEARINGS</td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr> 
                <td>
                  <table cellspacing="0" cellpadding="2" width="100%" class="bottomborder">
                    <tr> 
                      <td width="1%">
                        <% if (!Model.AcesMode) { %>
                        <table cellspacing="0" cellpadding="0" width="1%" >
                          <tr> 
                            <td align="center" class="kittype" nowrap="nowrap"><% = Html.KitLink(new KitIdentifier { KitID = Model.Kit.KitIdentifier.KitID, KitType = "RMK" }) %></td>
                          </tr>
                        </table>
                        <% } %>
                      </td>
                      <td>ROD BEARINGS + MAIN BEARINGS</td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <% if (Model.Editing && !Model.AcesMode) { %>
        <tr>
          <td align="center" colspan="5" class="kitsection border" width="1%" nowrap="nowrap">
            ADDITIONAL PARTS
          </td> 
          <td class="kitdata">
            <% if (!Model.ConfirmingAvailability) { %>
                <% using(Ajax.BeginForm("KitSearch", "StockStatus",
                            new AjaxOptions
                            {
                                UpdateTargetId = "InterchangeDialog",
                                OnFailure = "ajaxError",
                                OnBegin ="displayProcessingBlock",
                                OnSuccess ="addPartComplete"  })) { %>
                    <% = Html.Hidden("KitNipc", Model.Kit.NIPCCode) %>                    

                    <input type="submit" value="Add Part" id="AddPart" /> 
                <% } %>
            <% } %>
            <div class="AdditionalPartsDisplay KitCategoryDisplay">
                <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing, Kit = Model.Kit, CategoryParts = Model.Kit.GetCategoryParts(KitCategory.AdditionalParts) }); %>
            </div>
          </td>
        </tr>
        <% } %>
      </table>
      &nbsp;<br/><a name="crank"></a>      
      <table class="container" cellspacing="0" cellpadding="4">
        <tr>
          <td align="center" colspan="5" class="kitsection border" width="1%" nowrap="nowrap">
            CRANK KIT
          </td> 
          <td class="kitdata" id="tdCrankKits" name="tdCrankKits">
            <% if (Model.AcesMode) { %>
                <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.MasterKitParts.Where(kp => kp.SequenceNumber >= 900)  }); %>
            <% } else { %>
                <% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.CrankKit) }); %>
            <% } %>
          </td>
        </tr>
      </table>
      &nbsp;<br/>      
      <table cellspacing="0" cellpadding="2" class="container">
        <% if (!Model.AcesMode) { %>
        <tr> 
          <td align="center" width="1%" class="kitsection border">
            <table class="borderless">
              <tr align="center"> 
                <td class="kitsection borderless">R<br/>E<br/>L<br/>A<br/>T<br/>E<br/>D<br/><br/>E<br/>N<br/>G<br/>I<br/>N<br/>E</td>
                <td class="kitsection borderless">P<br/>A<br/>R<br/>T<br/>S</td>
              </tr>
            </table>
          </td>
          <td valign="top">
            <table cellspacing="0" cellpadding="2" width="100%">
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Intake Valves</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.IntakeValves) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Exhaust Valves</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.ExhaustValves) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Valve Springs</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.ValveSprings) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Spring Shims</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.SpringShims) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Valve Guides</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.ValveGuides) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Push Rods</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.PushRods) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Rocker Arms</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.RockerArms) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Oil Pump Screens</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.OilPumpScreens) }); %></td>
              </tr>              
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Connecting Rods</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.ConnectingRods) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Cylinder Sleeves</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.CylinderSleeves) }); %></td>
              </tr>
              <tr> 
                <td width="1%" class="kitcategory" nowrap="nowrap">Balancer Sleeve</td>
                <td class="kitdata"><% Html.RenderPartial("KitCategory", new KitCategoryViewModel { ConfirmingAvailability = Model.ConfirmingAvailability, FulfillmentProcessingResult = Model.FulfillmentProcessingResult, Editing = Model.Editing,  Kit = Model.Kit, CategoryParts = Model.Kit.GetRelatedCategoryParts(RelatedCategory.BalancerSleeve) }); %></td>
              </tr>
            </table>
          </td>
        </tr>
        <% } %>
        <tr> 
          <td align="center" width="1%" class="kitsection border">
            N<br/>
            O<br/>
            T<br/>
            E<br/>
            S</td>
          <td valign="top" class="topborder">
            <% foreach (var note in Model.Kit.CategoryNotes) { %>
                Note <% = note.NoteOrdinal %>: <% = Html.Encode(note.Note) %><br />
            <% } %>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
<br />