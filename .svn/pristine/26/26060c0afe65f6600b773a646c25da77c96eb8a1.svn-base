(function ($, ko, _) {    

    function Part(data, siteBaseUrl) {
        var thisPart = this;
        this.PartNumber = ko.observable(data.PartNumber);
        this.ProductLineMfrCode = ko.observable(data.ProductLineMfrCode);
        this.ProductID = ko.observable(data.ProductID);
        this.AAIABrandID = ko.observable(data.AAIABrandID);
        this.Notes = ko.observable(data.Notes);
        this.PartTypeDescription = ko.observable(data.PartTypeDescription);
        this.ProductLineName = ko.observable(data.ProductLineName);
        this.Description = ko.observable(data.Description);
        this.PositionDescription = ko.observable(data.PositionDescription);
        this.ImageUrls = ko.observable(data.ImageUrls || []);
        this.DownloadUrls = ko.observable(data.DownloadUrls || []);
        this.MarketingDescription = ko.observable();
        this.DetailsLoaded = ko.observable(false);
        this.DetailsVisible = ko.observable(false);
        this.ShowPackaging = ko.observable(false);
        this.PackagingVisible = ko.observable(false);
        this.IsHazardousMaterial = ko.observable(false);
        this.PackagingInfo = ko.observableArray([]);
        this.BuyersGuide = ko.observableArray([]);
        this.BuyersGuideLoaded = ko.observable(false);
        this.BuyersGuideVisible = ko.observable(false);
        this.Quantity = ko.observable(data.Quantity);
        this.EngineDescriptions = ko.observable(data.EngineDescriptions);
        this.SiteBaseUrl = siteBaseUrl;

        this.ApplicableQuantity = ko.computed(function () {
            // if there's only one valid quantity, use it. Otherwise default to one.
            return this.Quantity !== null ? this.Quantity() : 1;
        }, this);

        this.Url = ko.computed(function () {
            return 'StockStatus/Search/' + encodeURIComponent(this.PartNumber()) + '/' + this.ApplicableQuantity() + '/' + this.ProductLineMfrCode() + '?lookup=true';
        }, this);

        this.LoadBuyersGuide = function () {
            var promise = $.Deferred().resolve().promise();
            if (!thisPart.BuyersGuideLoaded()) {
                promise = $.getJSON(this.SiteBaseUrl + "lookup/GetBuyersGuide/" + this.ProductID());
                promise.done(function (buyersGuide) {
                    var groupedBuyersGuide = _.groupBy(buyersGuide, 'MakeName');
                    thisPart.BuyersGuide(groupedBuyersGuide);
                    thisPart.BuyersGuideLoaded(true);
                });
            }
            return promise;
        };

        this.LoadDetails = function () {
            var promise = $.Deferred().resolve().promise();
            if (!thisPart.DetailsLoaded()) {
                promise = $.getJSON(this.SiteBaseUrl + "lookup/GetProductDetails/" + this.ProductID());
                promise.done(function (details) {
                    thisPart.IsHazardousMaterial(details.ExtendedInformation.IsHazardousMaterial ? "Yes" : "No");
                    thisPart.MarketingDescription(details.PartDescriptions.MarketingDescription);
                    thisPart.PackagingInfo(details.PackageInformation);
                    thisPart.DetailsLoaded(true);
                });
            }
            return promise;
        };

        this.ShowBuyersGuide = function() {
            $.when(this.LoadBuyersGuide()).then(function () {
                thisPart.BuyersGuideVisible(true);
            });
        };

        this.ShowDetails = function () {
            $.when(this.LoadDetails()).then(function () {
                thisPart.DetailsVisible(true);
            });
        };

        this.ShowPackaging = function () {
            $.when(this.LoadDetails()).then(function () {
                thisPart.PackagingVisible(true);
            });
        };
    }

    ko.bindingHandlers.partViewer = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var unwrappedValue = ko.unwrap(valueAccessor()),
                options = allBindingsAccessor().partViewerOptions,
                showPartNumber = false,
                showPartTypeDescription = false,
                siteBaseUrl =  $('#siteBaseUrl').val(),
                vm = new Part(unwrappedValue, siteBaseUrl);

            if (options && options.showPartNumber === true) {
                showPartNumber = true;
            }

            if (options && options.showPartTypeDescription === true) {
                showPartTypeDescription = true;
            }

            $(element).on("click", "img.part", function () {
                showColorbox.call(this, 0);
            });

            $(element).on("click", "a.moreImages", function () {
                showColorbox.call(this, 1);
            });

            vm.showPartNumber = showPartNumber;
            vm.showPartTypeDescription = showPartTypeDescription;

            ko.renderTemplate("part-viewer-template", vm, { }, element);

            // Prevent double binding
            return { controlsDescendantBindings: true };
        }
    };

    function showColorbox(imageIndex) {
        var $this = $(this);
        var $partImages = $this.parents('div.partImageContainer').find('div.partImages').find('a');
        $partImages.colorbox({ photo: true, maxWidth: 700, innerWidth: 700, maxHeight: 700 });
        $partImages.eq(imageIndex).click();
    }

})($, ko, _);
