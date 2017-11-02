<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/html" id="part-viewer-template">
    <div style="float: right" class="partImageContainer" data-bind="if: ImageUrls().length">
        <img class="part" style="max-width: 150px; max-height: 150px; cursor: pointer" onerror="$(this).hide();"
            data-bind="attr: { src: ImageUrls()[0], 'data-title': ProductLineName() + ' ' + PartNumber() }">
        <div data-bind="if: ImageUrls().length > 1">
            <a href="#" class="moreImages">More Images</a>
        </div>
        <div class="partImages" data-bind="foreach: ImageUrls">
            <a data-bind="attr: {href: $data, rel: $parent.ProductID }"></a>
        </div>
    </div>
    <div data-bind="visible: showPartNumber">
        <a data-bind="attr: { href: Url }"><span data-bind="text: PartNumber"></span></a>
    </div>
    <div data-bind="text: ProductLineName">
    </div>
    <div data-bind="text: PartTypeDescription, visible: showPartTypeDescription && (PartTypeDescription() != Description())">
    </div>
    <span data-bind="text: Description"></span>
    <div data-bind="if: PositionDescription">
        Position: <span data-bind="text: PositionDescription"></span>
    </div>
    <div style="font-style: italic" data-bind="text: Notes">
    </div>    
    <div data-bind="foreach: EngineDescriptions">
        <div data-bind="text: $data">
        </div>
    </div>
    <div>
        <a href="#" data-bind="click: ShowDetails">More Details</a> 
    </div>
    <div data-bind="visible: true">
        <a href="#" data-bind="click: ShowPackaging">Packaging</a> 
    </div>
    <div>
        <a href="#" data-bind="click: ShowBuyersGuide">Buyer's Guide</a>
    </div>
    <div data-bind="if: DownloadUrls().length">
        <div>Downloads:</div>
        <div data-bind="foreach: DownloadUrls">
            <a data-bind="text: 'Download ' + ($index() + 1), attr: { href: $data, target: '_blank' }"></a>
        </div>    
    </div>
    
    <div data-bind="dialog: { isOpen: DetailsVisible, title: PartNumber() + ' Details', modal: true, width:700, resizable: false }">
        <p>
            <strong>Is Hazardous:</strong> <span data-bind="text: IsHazardousMaterial()"></span>
        </p>
        <div>
            <strong>Marketing Description:</strong></div>
        <p data-bind="text: MarketingDescription">
        </p>
        <p data-bind="visible: !MarketingDescription()">Description not available</p>
    </div>
    <div data-bind="dialog: { isOpen: PackagingVisible, title: PartNumber() + ' Packaging', modal: true, width:700, resizable: false }">
        <div data-bind="visible: PackagingInfo().length === 0">
            No packaging information is available for this part.
        </div>
        <div data-bind="foreach: PackagingInfo">
            <!-- ko if: $index() > 0 -->
                <hr/>
            <!-- /ko -->
            <div>
                <span data-bind="text: UOM"></span> (<span data-bind="text: EachesQuantity"></span>)
            </div>
            <div>
                <strong>Weight:</strong> <span data-bind="text: Weight"></span> <span data-bind="text: WeightUOM"></span>
            </div>
            <div>
                <strong>Package Dimensions:</strong> <span data-bind="text: Length"></span>L x <span data-bind="text: Width"></span>W
                x <span data-bind="text: Height"></span>H <span data-bind="text: DimensionsUOM">
                </span>
            </div>
        </div>
    </div>
    <div data-bind="dialog: { isOpen: BuyersGuideVisible, title: PartNumber() + ' Buyer\'s Guide', modal: true, width:700, height: 400, resizable: false }">
        <div data-bind="visible: _.keys(BuyersGuide()).length === 0">
            No buyer's guide is available for this part.
        </div>
        <div data-bind="foreach: _.keys(BuyersGuide()).sort()">
            <div>
                <b>
                    <span data-bind="text: $data"></span>
                </b>                
            </div>
            <div data-bind="foreach: $root.BuyersGuide()[$data]">
                <div>
                    <span data-bind="text: StartYear"></span>-<span data-bind="text: EndYear"></span> <span data-bind="text: ModelName"></span>                
                </div>
            </div>
        </div>
    </div>
</script>