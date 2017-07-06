// Input class names
var materialTextFieldClass = "mdl-textfield__input";
var dropdownButtonClass = "dropdown-button";
var listSelectorClass = "list-selector-for";
var comboDateClass = "combo-date";
// Error class name for text field
var inputErrorClassName = "is-invalid";

$(document).ready(function () {
    setInputValidations();
});


$.validator.setDefaults({
    ignore: []
    // any other default options and/or rules
});


function setInputValidations() {
    $("form").each(function () {
        var validator = $.data($(this)[0], "validator");
        
        if (validator) {
            var settings = validator.settings;

            settings.highlight = function (element, errorClass, validClass) {
                // Add error class to container div for material inputs
                // when they fail valiadtion
                if ($(element).hasClass(materialTextFieldClass)
                        && !$(element).hasClass(inputErrorClassName)) {
                    $(element).parent("div").addClass(inputErrorClassName);
                }
                                
                if (element.type === "radio"
                        && $(element).is(":hidden")) {

                    // Add error class to dropdown-button which contains
                    // this radio button
                    var materialDropDown = $(element).closest("." + dropdownButtonClass);

                    if (materialDropDown) {
                        materialDropDown.addClass(inputErrorClassName);
                    }

                    // Add error class to list-selector-for which contains
                    // this radio button
                    var listSelector = $(element).closest("." + listSelectorClass);

                    if (listSelector) {
                        listSelector.addClass(inputErrorClassName);
                    }
                }

                // Add error class to combo-date which contains
                // this hidden input date field
                if (element.type === "date"
                        && $(element).is(":hidden")) {
                    var comboDate = $(element).closest("." + comboDateClass);

                    if (comboDate) {
                        comboDate.addClass(inputErrorClassName);
                    }
                }

                // Default highlight behaviour
                if (element.type === "radio") {
                    this.findByName(element.name).addClass(errorClass).removeClass(validClass);
                } else {
                    $(element).addClass(errorClass).removeClass(validClass);
                }
            };

            settings.unhighlight = function (element, errorClass, validClass) {
                // Remove error class from container div if material input
                // passes validation
                if ($(element).hasClass(materialTextFieldClass)) {
                    var parentDiv = $(element).parent("div");

                    if (parentDiv &&
                        parentDiv.hasClass(inputErrorClassName)) {
                        parentDiv.removeClass(inputErrorClassName);
                    }
                }

                                
                if (element.type === "radio"
                        && $(element).is(":hidden")) {

                    // Remove error class from dropdown-button which contains
                    // this radio button
                    var materialDropDown = $(element).closest("." + dropdownButtonClass);

                    if (materialDropDown
                        && materialDropDown.hasClass(inputErrorClassName)) {
                        materialDropDown.removeClass(inputErrorClassName);
                    }

                    // Remove error class to list-selector-for which contains
                    // this radio button
                    var listSelector = $(element).closest("." + listSelectorClass);

                    if(listSelector
                        && listSelector.hasClass(inputErrorClassName)) {
                        listSelector.removeClass(inputErrorClassName);
                    }
                }

                // Remove error class from combo-date which contains
                // this hidden input date field
                if (element.type === "date"
                        && $(element).is(":hidden")) {
                    var comboDate = $(element).closest("." + comboDateClass);

                    if (comboDate
                        && comboDate.hasClass(inputErrorClassName)) {
                        comboDate.removeClass(inputErrorClassName);
                    }
                }

                // Default unhiglight behaviour
                if (element.type === "radio") {
                    this.findByName(element.name).removeClass(errorClass).addClass(validClass);
                } else {
                    $(element).removeClass(errorClass).addClass(validClass);
                }
            };
        }        
    });
};