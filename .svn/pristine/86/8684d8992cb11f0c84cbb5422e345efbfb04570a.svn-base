(function ($, ko, amplify) {
    var LookupViewModel = function () {

        var self = this;

        self.baseVehicleId = ko.observable();
        self.noPartsFound = ko.observable(false);
        // used to track whether or not a saved search is currently being restored
        self.restoringSearch = ko.observable();

        // Years
        self.years = ko.observableArray([]);
        self.years.load = function () {
            return $.getJSON("lookup/years").done(function (data) {
                self.years(data);
            });
        };
        self.selectedYear = ko.observable();

        // Makes
        self.makes = ko.observableArray([]);
        self.makes.load = function () {
            $.getJSON("lookup/makes?year=" + self.selectedYear()).done(function (data) {
                self.makes(data);
            });
        };
        self.selectedMake = ko.observable();

        // Models
        self.models = ko.observableArray([]);
        self.models.load = function () {
            $.getJSON("lookup/models?year=" + self.selectedYear() + "&makeId=" + self.selectedMake()).done(function (data) {
                self.models(data);
            });
        };
        self.selectedModel = ko.observable();
        self.selectedModel.subscribe(function () {
            if (!self.restoringSearch()) {
                if (self.selectedModel() != null) {
                    self.selectedSubmodel(null);
                    self.submodels.load();
                } else {
                    self.askSubmodel(false);
                    self.selectedSubmodel(null);
                    self.parts([]);
                    self.noPartsFound(false);
                }
            }
        });

        //Submodel
        self.askSubmodel = ko.observable();
        self.submodels = ko.observableArray([]);
        self.submodels.load = function () {
            self.askSubmodel(false);
            $.getJSON("lookup/submodels?year=" + self.selectedYear() + "&makeId=" + self.selectedMake() + "&modelId=" + self.selectedModel()).done(function (data) {
                self.baseVehicleId(data.baseVehicleId);
                processSubmodels(data.submodels);
                if (!self.askSubmodel()) {
                    self.getEngineConfigurations();
                }
            });
        };
        self.selectedSubmodel = ko.observable();
        self.selectedSubmodel.subscribe(function () {
            if (!self.restoringSearch()) {
                if (self.selectedSubmodel() != null) {
                    self.selectedEngineConfig(null);
                    self.getEngineConfigurations();
                } else {
                    self.askEngineConfig(false);
                    self.selectedEngineConfig(null);
                }
            }
        });  

        // Engine Configuration
        self.askEngineConfig = ko.observable();
        self.engineConfigurations = ko.observableArray([]);
        self.getEngineConfigurations = function () {
            self.askEngineConfig(false);
            $.getJSON("lookup/engines?baseVehicleId=" + self.baseVehicleId() + "&submodelId=" + self.selectedSubmodel()).done(function (data) {
                processEngineConfigurations(data);
                if (!self.askEngineConfig()) {
                    self.categories.load();
                }
            });
        };
        self.selectedEngineConfig = ko.observable();
        self.selectedEngineConfig.subscribe(function () {
            if (!self.restoringSearch()) {
                if (self.selectedEngineConfig() != null) {
                    self.selectedCategory(null);
                    self.categories.load();
                } else {
                    self.selectedCategory(null);
                    self.parts([]);
                    self.noPartsFound(false);
                }
            }
        });  

        // Categories
        self.categories = ko.observableArray([]);
        self.categories.load = function () {
            $.getJSON("lookup/categories?baseVehicleId=" + self.baseVehicleId() + "&submodelId=" + self.selectedSubmodel() + "&engineConfigId=" + self.selectedEngineConfig()).done(function (data) {
                self.categories(data);
                // select the option if there is only one
                if (data.length === 1) {
                    self.selectedCategory(data[0].ID);
                }
            });
        };
        self.selectedCategory = ko.observable();

        // Subcategories
        self.subcategories = ko.observableArray([]);
        self.subcategories.load = function () {
            $.getJSON("lookup/subcategories?baseVehicleId=" + self.baseVehicleId() + "&categoryId=" + self.selectedCategory() + "&submodelId=" + self.selectedSubmodel() + "&engineConfigId=" + self.selectedEngineConfig()).done(function (data) {
                self.subcategories(data);
            });
        };
        self.selectedSubcategory = ko.observable();

        // Product lines
        self.productLines = ko.observableArray([]);
        self.productLines.load = function () {
            $.getJSON("lookup/productlines?baseVehicleId=" + self.baseVehicleId() + "&categoryId=" + self.selectedCategory() + "&subcategoryId=" + self.selectedSubcategory() + "&submodelId=" + self.selectedSubmodel() + "&engineConfigId=" + self.selectedEngineConfig()).done(function (data) {
                processProductLines(data);
            });
        };
        self.selectedProductLine = ko.observable();
        self.selectedProductLine.subscribe(function () {
            if (!self.restoringSearch()) {
                if (self.selectedProductLine() && self.selectedProductLine() !== -1) {
                    self.getParts();
                }
            }
        });

        // Part types
        self.partTypes = ko.observableArray([]);
        self.partTypes.load = function () {
            if (self.selectedProductLine() === -1) {
                $.getJSON("lookup/partTypes?baseVehicleId=" + self.baseVehicleId() + "&categoryId=" + self.selectedCategory() + "&subcategoryId=" + self.selectedSubcategory() + "&submodelId=" + self.selectedSubmodel() + "&engineConfigId=" + self.selectedEngineConfig()).done(function (data) {
                    processPartTypes(data);
                    if (self.selectedSubcategory() >= 40000) {
                        // default to No Preference with an EPWI subcategory
                        self.selectedPartType(0);                      
                    }                    
                });
            }
        };
        self.selectedPartType = ko.observable();

        // Positions
        self.qualifyingPositions = ko.observable();
        self.qualifyingPositions.load = function () {
            self.askPosition(false);
            $.getJSON("lookup/positionQualifiers?baseVehicleId=" + self.baseVehicleId() + "&categoryId=" + self.selectedCategory() + "&subcategoryId=" + self.selectedSubcategory() + "&partTypeId=" + self.selectedPartType() + "&submodelId=" + self.selectedSubmodel() + "&engineConfigId=" + self.selectedEngineConfig()).done(function (data) {
                processPositionQualifiers(data);
                if (!self.askPosition()) {
                    self.getParts();
                }
            });
        };
        self.askPosition = ko.observable();
        self.displayPosition = ko.observable();
        self.selectedPosition = ko.observable();
        self.selectedPosition.subscribe(function () {
            if (!self.restoringSearch()) {
                if (self.selectedPosition() != null) {                    
                    self.getParts();
                } else {
                    self.parts([]);
                    self.noPartsFound(false);
                }
            }
        });

        // Part applications
        self.parts = ko.observable([]);      
        self.partCount = ko.observable();
        self.pageSize = 10;
        self.currentPage = ko.observable().extend({ notify: 'always' });
        self.startItem = ko.computed(function() {
            return (self.currentPage() - 1) * self.pageSize + 1;
        });
        self.endItem = ko.computed(function() {
            return _.min([self.startItem() + self.pageSize - 1, self.partCount()]);
        });
        self.pageCount = ko.computed(function() {
            return Math.ceil(self.partCount() / self.pageSize);
        });
        self.getParts = function () {
            self.currentPage(1);
        };
        self.prevPage = function () {
            if (self.currentPage() > 1) {
                self.currentPage(self.currentPage() - 1);
            }
        };
        self.nextPage = function () {
            if (self.currentPage() < self.pageCount()) {
                self.currentPage(self.currentPage() + 1);
            }
        };
        self.firstPage = function () {
            if (self.currentPage() > 1) {
                self.currentPage(1);
            }
        };
        self.lastPage = function () {
            if (self.currentPage() < self.pageCount()) {
                self.currentPage(self.pageCount());
            }
        };

        self.currentPage.subscribe(function () {
            var selectedEngine = _.find(self.engineConfigurations(), function(engineConfig) { return engineConfig.ID === self.selectedEngineConfig(); });
            var engineConfigId = selectedEngine ? selectedEngine.ID : 0;
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "lookup/parts",
                traditional: true,
                data:
                {
                    baseVehicleId: self.baseVehicleId(),
                    productLineId: self.selectedProductLine(),
                    categoryId: self.selectedCategory(),
                    subcategoryId: self.selectedSubcategory(),
                    partTypeId: self.selectedPartType(),
                    positionId: self.selectedPosition(),
                    submodelId: self.selectedSubmodel(),
                    engineConfigId: engineConfigId,
                    startItem: self.startItem(),
                    pageSize: self.pageSize
                }
            }).done(function (data) {
                self.partCount(data.totalCount);
                self.noPartsFound(data.totalCount === 0);
                self.parts(data.partList);                
            });
        });        

        // configures the cascading between dropdowns
        var cascade = function (target, options) {
            target.subscribe(function (newValue) {
                if (!self.restoringSearch()) {
                    // if selection is changed, clear the child selection and load the new child options
                    options.selectedChild(null);

                    if (newValue != null) {
                        options.children.load();
                    } else {
                        self.parts([]);
                        self.noPartsFound(false);
                    }
                }
            });

            var predicate = options.predicate || function () { return true; };

            options.children.subscribe(function (items) {
                if (predicate() && !self.restoringSearch()) {
                    handleAvailableItems(items, options.selectedChild);
                }
            });

            return target;
        };

        function processProductLines(data) {
            // Add a "See All" option
            data.unshift({ ProductLineName: 'SEE ALL', ID: -1 });
            self.productLines(data);
            self.selectedProductLine(-1);
        }

        function processPositionQualifiers(data) {
            self.askPosition(data.AskPosition);
            // if there's more than one available position qualifier, add a "No Preference" option
            if (data.QualifyingPositions && data.QualifyingPositions.length > 1) {
                data.QualifyingPositions.push({ Name: 'No Preference', ID: 0 });
            }
            self.qualifyingPositions(data.QualifyingPositions || []);
            self.displayPosition(data.DisplayPosition);
        }

        function processSubmodels(data) {
            self.askSubmodel(data.length);
            // if there's more than one available submodel, add a "No Preference" option
            if (data.length > 1 ) {
                data.push({ Name: 'No Preference', ID: 0 });
            }
            self.submodels(data || []);
            // select the option if there is only one
            if (data.length === 1) {
                self.selectedSubmodel(data[0].ID);
            }
        }

        function processEngineConfigurations(data) {
            // if there's more than one available engine configuration, add a "No Preference" option
            if (data /*&& data.length > 1*/) {
                data.push({ Description: 'No Preference', ID: 0 });
            }                        
            self.engineConfigurations(data || []);
            self.askEngineConfig(data.length);
                        
            // select the option if there is only one
            if (data.length === 1) {
                self.selectedEngineConfig(data[0].ID);
            }
        }

        function processPartTypes(data) {            
            if (data && data.length > 1) {
                data.push({ Name: 'No Preference', ID: 0 });
           }
           self.partTypes(data);
        }

        function handleAvailableItems(items, selectedItem) {
            // if there aren't any available choices, there are no parts to be found
            if (!items || items.length === 0) {
                self.noPartsFound(true);
                return;
            }
            // select the option if there is only one
            if (items.length === 1) {
                selectedItem(items[0].ID);
            }
            self.parts([]);
            self.noPartsFound(false);
        }

        // stores changes to dropdown selections
        function trackChanges(observable, key) {
            observable.subscribe(function () {
                if (!self.restoringSearch()) {
                    var value;
                    var storedSearch = amplify.store('storedSearch') || {};
                    value = observable();
                    storedSearch[key] = value;
                    amplify.store('storedSearch', storedSearch);
                }
            });
        }

        // check to see if the selected item is one of the available options
        function validateSelectedItem(selected, options) {
            var matchingItem = ko.utils.arrayFirst(options, function (item) { return item.ID === selected; });
            return matchingItem ? selected : null;
        }

        self.removeExtraWhiteSpace = function (option) {
            // IE 8 doesn't collapse whitespace in option text, so do it manually.
            var $option = $(option);
            $option.text($option.text().replace(/\s+/g, " "));
        };

        // restores the saved search
        self.restoreSearch = function () {
            var savedSearch = amplify.store('storedSearch');
            if (savedSearch && savedSearch.selectedYear) {
                self.restoringSearch(true);

                $.ajax({
                    type: "POST",
                    url: "lookup/restoreSearch",
                    data: savedSearch
                }).done(function (data) {
                    if (!data) return;

                    self.makes(data.makes || []);
                    self.models(data.models || []);
                    processSubmodels(data.submodels || []);
                    processEngineConfigurations(data.engineConfigurations || []);
                    self.categories(data.categories || []);
                    self.subcategories(data.subcategories || []);
                    processPartTypes(data.partTypes || []);
                    self.baseVehicleId(data.baseVehicleId);
                    processProductLines(data.productLines || []);
                    processPositionQualifiers(data.positionQualifiers || {});                    
                    
                    // restore the selected items from the saved search, but only if they are still valid
                    self.selectedYear(ko.utils.arrayFirst(self.years(), function (item) { return item === savedSearch.selectedYear; }) || null);
                    self.selectedMake(validateSelectedItem(savedSearch.selectedMake, self.makes()));
                    self.selectedModel(validateSelectedItem(savedSearch.selectedModel, self.models()));
                    self.selectedSubmodel(validateSelectedItem(savedSearch.selectedSubmodel, self.submodels()));                    
                    self.selectedEngineConfig(validateSelectedItem(savedSearch.selectedEngineConfig, self.engineConfigurations()));
                    self.selectedCategory(validateSelectedItem(savedSearch.selectedCategory, self.categories()));
                    self.selectedSubcategory(validateSelectedItem(savedSearch.selectedSubcategory, self.subcategories()));
                    self.selectedProductLine(validateSelectedItem(savedSearch.selectedProductLine, self.productLines()));
                    self.selectedPartType(validateSelectedItem(savedSearch.selectedPartType, self.partTypes()));
                    self.selectedPosition(validateSelectedItem(savedSearch.selectedPosition, self.qualifyingPositions()));

                    // if rules for performing search are met, do it!
                    if (((self.selectedPartType() >= 0 || self.selectedSubcategory() >= 40000) && (self.selectedPosition() != null || !self.askPosition())) ||  
                            self.selectedProductLine() > 0) {    
                        self.getParts();
                    }
                }).always(function() {
                    self.restoringSearch(false);
                });
            }
        };

        // Set up the cascading between selections, where the default cascading behavior works
        // Cascading is manually defined above for relationships that have special logic (such as Make -> Submodel)
        cascade(self.selectedYear, { children: self.makes, selectedChild: self.selectedMake });
        cascade(self.selectedMake, { children: self.models, selectedChild: self.selectedModel });
        cascade(self.selectedCategory, { children: self.subcategories, selectedChild: self.selectedSubcategory });
        cascade(self.selectedSubcategory, { children: self.productLines, selectedChild: self.selectedProductLine });
        cascade(self.selectedProductLine, { children: self.partTypes, selectedChild: self.selectedPartType, predicate: function() { return self.selectedProductLine() === -1; } });        
        cascade(self.selectedPartType, { children: self.qualifyingPositions, selectedChild: self.selectedPosition, predicate: self.askPosition });
        
        // track certain values in local storage
        trackChanges(self.selectedYear, "selectedYear");
        trackChanges(self.selectedMake, "selectedMake");
        trackChanges(self.selectedModel, "selectedModel");
        trackChanges(self.selectedSubmodel, "selectedSubmodel");
        trackChanges(self.selectedEngineConfig, "selectedEngineConfig");
        trackChanges(self.selectedCategory, "selectedCategory");
        trackChanges(self.selectedSubcategory, "selectedSubcategory");
        trackChanges(self.selectedProductLine, "selectedProductLine");
        trackChanges(self.selectedPartType, "selectedPartType");        
        trackChanges(self.selectedPosition, "selectedPosition");        
    };

    $(function () {

        $(document).ajaxStart(displayProcessingBlock);
        $(document).ajaxStop(hideProcessingBlock);
        $(document).ajaxError(function (request, textStatus, errorThrown) {
            hideProcessingBlock(true);
            console.log(errorThrown);
            //alert('There was an error communicating with the server. Please refresh this page in your browser.');
        });

        $.ajaxSetup({
            cache: false
        });       

        var vm = new LookupViewModel();

        $.when(vm.years.load()).then(function () {
            ko.applyBindings(vm);
            vm.restoreSearch();
        });

    });   

    window.onerror = function() {
        alert('There was an error processing your request.');
        // clear the stored search since it may be invalid
        amplify.store('storedSearch', null);
        location.reload();
    }

})($, ko, amplify);