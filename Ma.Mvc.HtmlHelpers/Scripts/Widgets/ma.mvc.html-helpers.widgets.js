; (function () {
    var selectedClass = "selected";

    $(document).ready(function () {
        initializeDropDownButtonFor();
        initializeListSelectorFor();
        initializeListMultiSelectorFor();
        initializeComboDateFor();
        initializeListEditorFor();
        initializeSortingControl();
        initializeTreeViewFor();
    })

    /*
     * 
     * 
     * 
     * 
     * Scripts for DropDownButtonFor helper  
     * 
     * 
     * 
     * 
     * 
    */
    var dropDownButtonClasses = new Object();
    dropDownButtonClasses.menu = ".dropdown-menu";
    dropDownButtonClasses.container = ".dropdown-button-container";
    dropDownButtonClasses.header = ".dropdown-button-header";

    function initializeDropDownButtonFor() {
        var items = $(dropDownButtonClasses.container);

        // trigger dropdown
        $(dropDownButtonClasses.container).click(function (e) {
            // Stop event bubling
            e.stopPropagation();

            // Close all drop down buttons first
            closeAllOpenDropDownButtons();

            //e.preventDefault();
            var dropDownMenu = $(this).siblings(dropDownButtonClasses.menu);
            $(this).find(dropDownButtonClasses.header).add(dropDownMenu).toggleClass("open");

            // If there is no space to scale menu to downwards scale it to upwards
            var upwardsCssClass = "upwards";
            var dropDownHeader = $(this).find(dropDownButtonClasses.header);
            var menuOffset = dropDownHeader.offset().top;
            var documentHeight = $(document).height();
            if (documentHeight - menuOffset < dropDownMenu.height()) {
                dropDownMenu.addClass(upwardsCssClass);
            }
        });


        $(dropDownButtonClasses.menu).each(function () {
            // Get checked radio button
            var checkedRadio = $(this).find("li input[type=radio]:checked").first();

            // If there is any, check container list item
            // to add css class and so on.
            if (checkedRadio) {
                // Get contaienr list item
                var listItem = checkedRadio.parent("li");
                selectDropDownButtonItem(listItem);

                var innerHeight = listItem.innerHeight();
                var itemIndex = listItem.index();
                listItem.parent("ul").scrollTop((itemIndex - 5) * innerHeight);
            }
        });

        // Select list item when clicked
        $(dropDownButtonClasses.menu).find("li").click(function () {
            selectDropDownButtonItem($(this));
        });

        // Close drop down when clicked somewhere else in document	
        $(document).click(function () {
            closeAllOpenDropDownButtons();
        });
    }

    function selectDropDownButtonItem(listItem) {
        // Find needed html elements
        var dropDownMenu = listItem.parent(dropDownButtonClasses.menu);
        var dropDownButtonContainer = dropDownMenu.siblings(dropDownButtonClasses.container);
        var dropDownButtonHeader = dropDownButtonContainer.find(dropDownButtonClasses.header);

        // Select list item if needed
        if (!listItem.hasClass(selectedClass)) {
            // Get radio button inside list item to check if needed
            var radioButton = listItem.find("input[type=radio]").first();
            // Get text of list item to set header text
            var options = listItem.find("span").html();
            dropDownButtonHeader.text(options);

            // Find currently selected item and remove selected class
            var currentlySelected = dropDownMenu.find("li." + selectedClass).first();
            if (currentlySelected) {
                currentlySelected.removeClass(selectedClass);
            }

            // Add selected class to clicked item
            listItem.addClass(selectedClass);

            // Check radio button inside list item
            radioButton.prop("checked", true);

            // Get value of radio button
            var val = radioButton.val();

            // If this radio button has default value,
            // then remove "checked" proeprty to bind "nothing"
            if (!val)
                radioButton.prop("checked", false);
        }

        // Close drop down menu if needed
        if (dropDownMenu.add(dropDownButtonHeader).hasClass("open")) {
            dropDownMenu.add(dropDownButtonHeader).removeClass("open");
        }
    }

    function closeAllOpenDropDownButtons() {
        $(dropDownButtonClasses.menu + ".open").each(function () {
            $(this)
                .add($(this)
                .siblings(dropDownButtonClasses.container)
                .find(dropDownButtonClasses.header))
                .removeClass("open");
        });
    }


    /*
     * 
     * 
     * 
     * 
     * Scripts for ListSelectorFor helper  
     * 
     * 
     * 
     * 
     * 
    */
    function initializeListSelectorFor() {
        // Get checked radio button
        var checkedRadio = $(".list-selector-for li input[type=radio]:checked");

        // If there is any, check container list item
        // to add css class and so on.
        if (checkedRadio) {
            // Get contaienr list item
            var listItem = checkedRadio.parent("div").parent("li");
            selectItemOfListSelctor(listItem);
        }

        // Use this method instead of $(".list-selector-for li").click()
        // to enable selecting items loaded by Ajax
        $(document).on("click", ".list-selector-for li", function () {
            selectItemOfListSelctor($(this));
        });
    }

    // Select list item
    function selectItemOfListSelctor(listItem) {
        // Get radio button inisde the li element and check it
        var radioButton = listItem.find("input[type=radio]").first();

        if (!listItem.hasClass(selectedClass)) {
            var list = listItem.parent("ul");
            var selectedItem = list.find("." + selectedClass);
            selectedItem.removeClass(selectedClass);

            listItem.addClass(selectedClass);
            // Check hidden radio button
            radioButton.prop("checked", true);
        }
    }




    /*
     * 
     * 
     * 
     * 
     * Scripts for ListMultiSelectorFor helper  
     * 
     * 
     * 
     * 
     * 
    */
    function initializeListMultiSelectorFor() {
        // Get checked check boxes
        var chekdedCheckBoxes = $(".list-multi-selector-for li input[type=checkbox]:checked");

        // If there is any, check container list item
        // to add css class and so on.
        if (chekdedCheckBoxes) {

            chekdedCheckBoxes.each(function () {
                var listItem = $(this).parent("div").parent("li");
                selectItemOfListMultiSelctor(listItem);
            });
        }

        $(".list-multi-selector-for li").click(function () {
            selectItemOfListMultiSelctor($(this));
        });
    }

    // Select list item
    function selectItemOfListMultiSelctor(listItem) {
        // Get check box inisde the li element and check it
        var checkBox = listItem.find("input[type=checkbox]").first();

        if (!listItem.hasClass(selectedClass)) {
            // Check
            listItem.addClass(selectedClass);
            // Check hidden checkbox
            checkBox.prop("checked", true);
        }
        else {
            // Uncheck
            listItem.removeClass(selectedClass);
            // Check hidden checkbox
            checkBox.prop("checked", false);
        }
    }


    /*
     *
     * 
     * 
     * 
     * 
     * Scripts for ComboDateFor helper
     * 
     * 
     * 
     * 
     * 
     *  
    */
    var comboDateForClasses = new Object();
    comboDateForClasses.comboDate = ".combo-date";
    comboDateForClasses.dayContainer = ".day-container";
    comboDateForClasses.monthContainer = ".month-container";
    comboDateForClasses.yearContainer = ".year-container";

    function initializeComboDateFor() {
        $(comboDateForClasses.comboDate).find("li").click(function () {
            setHiddenInputValueForComboDate($(this).closest(comboDateForClasses.comboDate));
        });
    }

    // Set hidden date input to selected date
    function setHiddenInputValueForComboDate(comboDate) {
        var hiddenInput = $(comboDate).find(":input[type=date]").first();

        var day = $(comboDate).find(
            comboDateForClasses.dayContainer + " input[type=radio]:checked")
            .first().val();

        var month = $(comboDate).find(
            comboDateForClasses.monthContainer + " input[type=radio]:checked")
            .first().val();

        var year = $(comboDate).find(
            comboDateForClasses.yearContainer + " input[type=radio]:checked")
            .first().val();

        // Format number as "00"
        var formatNumber = function (number) {
            return ("0" + number).slice(-2);
        };

        if (day && month && year) {
            var formatted = year + "-" + formatNumber(month) + "-" + formatNumber(day);
            hiddenInput.val(formatted);
        }
    }




    /*
     * 
     * 
     * 
     * 
     * Scripts for ListEditor helper  
     * 
     * 
     * 
     * 
     * 
    */

    // Class names for ListEditorFor
    var listEditorForClasses = new Object();
    listEditorForClasses.listEditorFor = ".list-editor-for"
    listEditorForClasses.listData = ".list-data";
    listEditorForClasses.listEditor = ".list-editor";

    listEditorForClasses.dataTemplate = ".list-editor-data__template";
    listEditorForClasses.dataRow = ".list-data-row";
    listEditorForClasses.rowDisplayField = ".list-data-display__field";

    listEditorForClasses.saveButton = ".list-editor-action__save";
    listEditorForClasses.cancelButton = ".list-editor-action__cancel";
    listEditorForClasses.addButton = ".list-data-action-button__add";
    listEditorForClasses.editButton = ".list-data-action-button__edit";
    listEditorForClasses.deleteButton = ".list-data-action-button__delete";

    // Additional data for ListEditorFor
    var listEditorForData = new Object();
    listEditorForData.indexerInputNamePart = ".index";
    listEditorForData.templateIndex = "_template_index_";
    listEditorForData.editedItemIndex = null;

    // Initialize ListEditorFor for use.
    function initializeListEditorFor() {

        // If listEditorForData.editedItemIndex has been set
        // get it and reset its value to null. 
        // Otherwise create new GUID as index.
        var getCurrentIndex = function () {
            var currentIndex = null;

            if (listEditorForData.editedItemIndex) {
                currentIndex = listEditorForData.editedItemIndex;

                // Reset editedItemIndex to not confuse when
                // editing another item or adding new one.
                listEditorForData.editedItemIndex = null;
            } else {
                // Generate new index
                currentIndex = createGiud();
            }

            return currentIndex;
        };

        // Get data row template.
        var getDataRowTempate = function (listEditor) {
            var rowTemplate = $(listEditor)
                .find(listEditorForClasses.dataTemplate)
                .find(listEditorForClasses.dataRow)
                .clone();

            return rowTemplate;
        };

        // Get hidden input by name similarity.
        var getHiddenInputByName = function (elementContext, inputName) {
            var selector = "input:hidden[name$='" + inputName + "']";
            var input = elementContext.find(selector);
            return input;
        };

        // Get hidden input by name similarity and value.
        var getHiddenInputByNameAndValue = function (elementContext, inputName, inputValue) {
            var selector = "input:hidden[name$='" + inputName + "'][value='" + inputValue + "']";
            var input = elementContext.find(selector);
            return input;
        };

        // Get editor by name
        var getEditorByName = function (elementContext, editorName) {
            var nameFilter = "[name='" + editorName + "']";
            // First try to find input
            var editor = elementContext.find("input" + nameFilter);

            // If input search was unseccessful try to find select
            if (!editor.length) {
                editor = elementContext.find("select" + nameFilter);
            }

            return editor;
        }

        // Set value of hidden input and text of span next to that input
        var setValuesInTemplate = function (template, editor) {
            // Get value and text to set
            var editorValue = null;
            var editorText = null;

            if ($(editor).is("input")) {
                editorValue = $(editor).val();
                editorText = $(editor).val();
            } else {
                // Element is a select
                var selectedOption = $(editor).find("option:selected");

                editorValue = selectedOption.val();
                editorText = selectedOption.text();
            }

            var editorName = editor.attr("name");

            var hiddenInput = getHiddenInputByName(template, "." + editorName);
            var span = $(hiddenInput).siblings("span");

            $(hiddenInput).val(editorValue);
            span.html(editorText);
        };

        // Replace _template_index_ in template with specified index.
        var replaceIndexInTemplate = function (template, index) {
            // Replace index in html
            var templateIndexRegex = new RegExp(listEditorForData.templateIndex, "g");

            var templateHtml = $(template).html()
                .replace(templateIndexRegex, index);
            // Set html of template to replaced html.
            template.html(templateHtml);

            return template;
        }

        // Clear values in editors
        var resetEditors = function (listEditor) {
            listEditor = $(listEditor);

            // Reset input elements
            listEditor.find("input")
                .not(listEditor.find(listEditorForClasses.dataTemplate + " input"))
                .each(function () {
                    $(this).val(null).change();
                });

            // Reset select elements
            listEditor.find("select")
                .not(listEditor.find(listEditorForClasses.dataTemplate + " input"))
                .each(function () {
                    $(this).prop("selectedIndex", 0);
                });
        }

        // Add element to list data when save button is clicked
        $(listEditorForClasses.saveButton).click(function () {
            // Get current list editor
            var listEditor = $(this).closest(listEditorForClasses.listEditor);

            var formElement = listEditor.closest("form");

            // Validate if there is any assigned validator
            if ($.data(formElement, "validator")) {
                var validator = formElement.validate({ ignore: ":hidden" });

                var isValid = validateElementsInContext(validator, listEditor, false);

                // Exit if editors are not valid
                if (!isValid) {
                    return false;
                }
            }

            var rowTemplate = getDataRowTempate(listEditor);

            // Loop throug input and select editors, set value of corresponding
            // hidden input and set text of span.
            listEditor.find("input")
                .add(listEditor.find("select"))
                .not(listEditor.find(listEditorForClasses.dataTemplate + " input"))
                .each(function () {
                    setValuesInTemplate(rowTemplate, $(this));
                });

            // Clear values in editors
            resetEditors(listEditor);

            // Get indexer input and remove disabled attribute to be able to bind it
            var indexerInput = getHiddenInputByName(rowTemplate,
                listEditorForData.indexerInputNamePart);
            indexerInput.removeAttr("disabled");

            var currentIndex = getCurrentIndex();
            replaceIndexInTemplate(rowTemplate, currentIndex);

            // Get list data
            var listData = listEditor.siblings(listEditorForClasses.listData);

            // Look into list data for indexer with the value which 
            // is equal to value of currentIndex. If there is such input,
            // then this editing mode and we have to replace listDataRow
            // which contains this indexer with the consturcted rowTemplate.
            // If there is no such input then this is add mode and we just need
            // to append rowTemplate to listData.
            var existingIndexer = getHiddenInputByNameAndValue(
                listData, listEditorForData.indexerInputNamePart, currentIndex);


            if (existingIndexer.length) {
                // If indexer with the same value exists replace that data row.
                var containerDataRow = existingIndexer.closest(listEditorForClasses.dataRow);
                containerDataRow.html(rowTemplate);
            } else {
                // If the indexer with the same value does not exist add this row
                listData.append(rowTemplate);
            }

            listEditor.hide();

            // Return false to prevent submit.
            return false;
        });

        // Clear editors when cancel button is clicked and 
        // set editedItemIndex to null
        $(listEditorForClasses.cancelButton).click(function () {
            // Get current list editor
            var listEditor = $(this).closest(listEditorForClasses.listEditor);

            resetEditors(listEditor);
            // Set editedItemIndex to null to not confuse when
            // editing another item or adding new one.
            listEditorForData.editedItemIndex = null;
            listEditor.hide();

            // Return false to prevent submiting form
            return false;
        });

        // Show list editor when add button clicked
        $(listEditorForClasses.addButton).click(function () {
            var listEditor = $(this)
                .closest(listEditorForClasses.listData)
                .siblings(listEditorForClasses.listEditor);

            listEditor.show();

            // Return false to prevent submiting form
            return false;
        });

        // Prepare entity for editing
        // Use $(listEditorForClasses.listData).on('click'.....) instead of $(listEditorForClasses.editButton).click(..)
        // to apply this event to dynamically added elements.
        $(listEditorForClasses.listData).on('click',
            listEditorForClasses.editButton, function () {
                // Get list editor
                var listEditor = $(this)
                    .closest(listEditorForClasses.listData)
                    .siblings(listEditorForClasses.listEditor);

                var currentRow = $(this).closest(listEditorForClasses.dataRow);
                var indexerInput = getHiddenInputByName(currentRow, listEditorForData.indexerInputNamePart);

                // Set edited item index to current indexer value to track entity
                listEditorForData.editedItemIndex = indexerInput.val();

                // Regex for getting main input name from indexed input name
                var hiddenInputRegex = /(\w+)\[(.*)\]\.(\w+)/;
                currentRow.find("input").each(function () {
                    var inputName = $(this).attr("name");

                    if (hiddenInputRegex.test(inputName)) {
                        var inputNameParts = hiddenInputRegex.exec(inputName);

                        var mainInputName = inputNameParts[3];

                        // Get corresponding editor for input
                        var editor = getEditorByName(listEditor, mainInputName);
                        // Set value of editor
                        editor.val($(this).val()).change();
                    }
                });

                // Show list editor
                listEditor.show();

                // Return false to prevent submit
                return false;
            });

        // Delete element from list
        // Use $(listEditorForClasses.listData).on('click'.....) instead of $(listEditorForClasses.deleteButton).click(..)
        // to apply this event to dynamically added elements.
        $(listEditorForClasses.listData).on("click",
            listEditorForClasses.deleteButton, function () {
                // Get current row
                var currentRow = $(this).closest(listEditorForClasses.dataRow);
                // Remove current row from list.
                currentRow.remove();

                // Return false to prevent submit
                return false;
            });
    };



    /*
     * 
     * 
     * 
     * 
     * Scripts for SortControlFor helper  
     * 
     * 
     * 
     * 
     * 
    */

    var sortingControlForClasses = new Object();
    sortingControlForClasses.sortingControl = ".sorting-control";
    sortingControlForClasses.sortingControlHeader = ".sorting-control-header";
    sortingControlForClasses.sortingControlList = ".sorting-control-list";

    function initializeSortingControl() {

        // Function to collapse all open sorting controls
        var collapseAllSortingControls = function () {
            $(sortingControlForClasses.sortingControl)
                .find(sortingControlForClasses.sortingControlList + ".open")
                .each(function () {
                    $(this)
                        .add($(this).siblings(sortingControlForClasses.sortingControlHeader + ".open"))
                        .removeClass("open");
                });
        };

        var openSortingControl = function (sortingControl, e) {
            // Stop event bubbling
            e.stopPropagation();

            // Close all open sorting controls
            collapseAllSortingControls();

            var sortingControlList = $(sortingControl).find(sortingControlForClasses.sortingControlList);
            $(sortingControl).find(sortingControlForClasses.sortingControlHeader).add(sortingControlList).toggleClass("open");
        };

        // Open sorting control list when clicked on the header
        $(sortingControlForClasses.sortingControlHeader).click(function (e) {
            var sortingControlContainer = $(this).parent();
            openSortingControl(sortingControlContainer, e);
        });

        // Close all sorting controls when user clicks somewhere else on the document
        $(document).click(function () {
            collapseAllSortingControls();
        });
    };



    /*
     * 
     * 
     * 
     * 
     * Scripts for TreeViewFor helper  
     * 
     * 
     * 
     * 
     * 
    */
    var treeViewClasses = {
        treeView: ".tree-view",
        loaded: ".tv-loaded",
        list: ".tree-view-list",
        listItem: ".tree-view-list-item",
        level: ".level-",
        contentContainer: ".content-container",
        defaultIcons: ".default-icons",
        hasLeaves: ".has-leaves",
        isOpen: ".is-open"
    };

    var treeViewAttributes = {
        model: "data-tv-for",
        sourceUrl: "data-tv-source-url",
        level: "data-tv-level",
        searchExpression: "data-tv-search-exp"
    };

    var treeView;

    function initializeTreeViewFor() {
        // Get tree view
        if (!treeView) {
            treeView = $(treeViewClasses.treeView);
        }

        if (!treeView) {
            return;
        }

        // Get tree view attributes
        var modelName = treeView.attr(treeViewAttributes.model);
        var sourceUrl = treeView.attr(treeViewAttributes.sourceUrl);
        var searchExpression = treeView.attr(treeViewAttributes.searchExpression);

        // Initialize radio button for tree view list item
        var initializeRadioButton = function (radioButtonValue) {
            var radioButton = jQuery("<input />", {
                id: modelName,
                name: modelName,
                type: "radio",
                value: radioButtonValue,
                hidden: "hidden"
            });

            return radioButton;
        };

        // Initialize child list for tree view
        var initializeChildList = function (level) {
            var levelCssClass = treeViewClasses.level.substring(1) + level;
            var childList = jQuery("<ul></ul>", {
                class: treeViewClasses.list.substring(1),
                hidden: "hidden"
            });

            // Add level class and attribute
            childList.addClass(levelCssClass);
            childList.attr(treeViewAttributes.level, level);

            return childList;
        };

        // Initialize list item
        var initializeListItem = function (listItemData) {
            // Initialize list item
            var listItem = jQuery("<li></li>", {
                class: treeViewClasses.listItem.substr(1)
            });

            // Initialize content container
            var contentContainer = jQuery("<div></div>", {
                class: treeViewClasses.contentContainer.substring(1)
            });

            // Initialize icon
            jQuery("<i></i>", {
                class: treeViewClasses.defaultIcons.substring(1),
                text: "keyboard_arrow_right"
            })
            .appendTo(contentContainer);

            // Initialize span
            jQuery("<span></span>", {
                text: listItemData.Value
            })
			.appendTo(contentContainer);

            // Initialize radio button
            initializeRadioButton(listItemData.Id)
                .appendTo(contentContainer);

            // Append content container to list item
            contentContainer.appendTo(listItem);

            // Initialize child list
            initializeChildList(listItemData.Level).appendTo(listItem);

            return listItem;
        };

        // Initialize list items and add them to the provided list
        var initializeAndAddItems = function (listToAppendTo, itemList) {
            if (!listToAppendTo || !itemList) {
                return;
            }

            // Loop through list items, initialize listItem 
            // and append them to the provided list
            $(itemList).each(function (index, listItem) {
                initializeListItem(listItem)
				    .appendTo(listToAppendTo);
            });
        };

        // Select tree view list
        var selectTreeViewLeave = function (listItem) {
            // If list item is not defined, or list item has already
            // been selected, 
            if (!listItem || listItem.hasClass(selectedClass)) {
                return;
            }

            // If there is any other sleected leave remove selected class
            var currentlySelected = treeView.find("li." + selectedClass);
            if (currentlySelected) {
                currentlySelected.removeClass(selectedClass);
            }

            // Get radio button and select it
            var radioButton = $(listItem)
                .children(treeViewClasses.contentContainer)
                .children("input[type=radio]");
            radioButton.prop("checked", true);

            // Add selected class to list item
            $(listItem).addClass(selectedClass);
        };

        // Add on click event
        treeView.on("click", treeViewClasses.listItem,
			function (event) {
			    var currentItem = $(this);
			    var contentContainer = currentItem
                    .children(treeViewClasses.contentContainer);

			    // Get radio button and tree view list from list item
			    var currentRadioButton = contentContainer.children("input[type=radio]");
			    var childList = currentItem.children(treeViewClasses.list);

			    // If list is not loaded try to load it
			    var loadedCssClassName = treeViewClasses.loaded.substring(1);
			    var hasLeavesCssClassName = treeViewClasses.hasLeaves.substring(1);

			    // Initialize function to toggle child list
			    var toggleChildList = function () {
			        // Add is-open class if there is any item inside childList
			        if (childList.hasClass(hasLeavesCssClassName)) {
			            currentItem.toggleClass(treeViewClasses.isOpen.substring(1));

			            // Toggle child list on click
			            childList.toggle();
			        }
			    };

			    // If data has not been loaded, then load data and toggle child list
			    // only after data is loaded, otherwise, just toggle child list.
			    if (!childList.hasClass(loadedCssClassName)) {
			        // Set parentId and searchExpression
			        var data = {
			            parentId: currentRadioButton.val(),
			            searchExpression: searchExpression
			        };

			        // Get child list from source and append them to list
			        $.get(sourceUrl, data, function (result) {
			            initializeAndAddItems(childList, result.ItemList);

			            // Set if has any child item
			            if (result && result.ItemList && result.ItemList.length > 0) {
			                childList.addClass(hasLeavesCssClassName);
			            }

			            toggleChildList();
			        });

			        // Add loaded css class to list
			        childList.addClass(loadedCssClassName);
			    } else {
			        toggleChildList();
			    }

			    // Select clicked list item
			    selectTreeViewLeave(currentItem);

			    // Stop event propagation
			    event.stopPropagation();
			});
    };

    /*
     * 
     * 
     * 
     * 
     * Utility scripts
     * 
     * 
     * 
     * 
     * 
    */

    // Create GUID
    function createGiud() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    // Validate every input element inside div.
    // Return false if any element fails validation.
    function validateElementsInContext(validator, elementContext, validateHiddenInputs) {
        var isContextValid = true;

        if (validateHiddenInputs == undefined
            || validateHiddenInputs == null)
            validateHiddenInputs = true;

        var inputSelector = "input";
        if (!validateHiddenInputs) {
            inputSelector = inputSelector + "[type != 'hidden']";
        }

        // Validate input elemets
        $(elementContext).find(inputSelector).each(function () {
            if (!validator.element($(this))) {
                isContextValid = false;
            }
        });

        // Validate select elements
        $(elementContext).find("select").each(function () {
            if (!validator.element($(this))) {
                isContextValid = false;
            }
        });

        return isContextValid;
    }
}());