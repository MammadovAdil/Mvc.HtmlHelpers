using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using Ma.Mvc.HtmlHelpers.Infrastructure;
using System.Collections;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    public static class ListHelpers
    {
        /// <summary>
        /// List select for property, instead of 
        /// drop down list.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>
        /// <returns>Html code of list to select from.</returns>
        public static MvcHtmlString ListSelectorFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source)
        {
            return htmlHelper.ListSelectorFor(expression, source, null);
        }

        /// <summary>
        /// List select for property, instead of 
        /// drop down list with custom attributes.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Html code of list to select from.</returns>
        public static MvcHtmlString ListSelectorFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source,
            object htmlAttributes)
        {
            FluentTagBuilder listSelectorFor = htmlHelper.ConstructListSelector(
                expression, source, false, htmlAttributes);

            return new MvcHtmlString(listSelectorFor.Render());
        }

        /// <summary>
        /// List multi-select for property, instead of 
        /// drop down list with custom attributes.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>
        /// <returns>Html code of multi-select list to select from.</returns>
        public static MvcHtmlString ListMultiSelectorFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source)
        {
            return htmlHelper.ListMultiSelectorFor(expression, source, null);
        }

        /// <summary>
        /// List multi-select for property, instead of 
        /// drop down list with custom attributes.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Html code of multi-select list to select from.</returns>
        public static MvcHtmlString ListMultiSelectorFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source,
            object htmlAttributes)
        {
            FluentTagBuilder listMultiSelectorFor = htmlHelper.ConstructListSelector(
                expression, source, true, htmlAttributes);

            return new MvcHtmlString(listMultiSelectorFor.Render());
        }

        /// <summary>
        /// Construct list selector according to parameters.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>
        /// <param name="enableMultiSelect">Enable selecting multiple items at once.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Constructed list selector as FluentTagBuilder.</returns>
        internal static FluentTagBuilder ConstructListSelector<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source,
            bool enableMultiSelect,
            object htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (source == null
                || source.Items == null)
                throw new ArgumentNullException(nameof(source));

            // Determine main class
            string ulCssClass = enableMultiSelect
                ? "list-multi-selector-for"
                : "list-selector-for";
            string contentContainerCssClass = "content-container";
            string errorCssClass = string.Format("{0}__error", ulCssClass);

            // Get model metadata
            ModelMetadata metadata = ModelMetadata
                .FromLambdaExpression(expression, htmlHelper.ViewData);

            // Declare model data and get it from metadata
            string modelData = null;
            List<string> listModelData = null;

            if (metadata.Model != null)
            {
                if (enableMultiSelect)
                    listModelData = ((IEnumerable)metadata.Model)
                        .Cast<object>()
                        .Where(m => m != null)
                        .Select(m => m.ToString())
                        .ToList();
                else
                    modelData = metadata.Model.ToString();
            }

            // Convert html attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Construct ul tag.
            FluentTagBuilder ulBuilder = new FluentTagBuilder("ul")
                .MergeAttributes(attributes)
                .AddCssClass(ulCssClass);

            // Loop through items and add them to list
            foreach (SelectListItem listItem in source.Items)
            {
                // Initialize icon
                FluentTagBuilder icon = new FluentTagBuilder("i")
                    .AddCssClass("default-icons")
                    .SetInnerHtml("keyboard_arrow_right");

                // Initialize span
                FluentTagBuilder span = new FluentTagBuilder("span")
                    .SetInnerHtml(listItem.Text);

                // Initialize hidden selector field to select.
                // If multiselect enabled, then this field should be
                // checkbox, otherwise it should be radio button.
                string hiddenSelector;
                if (enableMultiSelect)
                {
                    // Get model name form expression
                    string modelName = ExpressionHelper.GetExpressionText(expression);

                    // Get validation attributes
                    IDictionary<string, object> validationAttributes =
                        htmlHelper.GetUnobtrusiveValidationAttributes(modelName, metadata);

                    // Determine if list item is checked
                    bool isChecked = listModelData != null
                        && listModelData.Contains(listItem.Value);

                    FluentTagBuilder hiddenCheckBox = new FluentTagBuilder("input")
                        .MergeAttribute("name", modelName)
                        .MergeAttribute("id", modelName)
                        .MergeAttribute("value", listItem.Value)
                        .MergeAttribute("type", "checkbox")
                        .MergeAttribute("hidden", "hidden")
                        .MergeAttributes(validationAttributes);

                    if (isChecked)
                        hiddenCheckBox.MergeAttribute("checked", "true");

                    hiddenSelector = hiddenCheckBox.Render();
                }
                else
                {
                    hiddenSelector = htmlHelper
                        .RadioButtonFor(expression, listItem.Value, new { hidden = "hidden" })
                        .ToHtmlString();
                }

                // Initialize div and add elements inside.
                FluentTagBuilder containerDiv = new FluentTagBuilder("div")
                    .AddCssClass(contentContainerCssClass)
                    .AppendChild(icon)
                    .AppendChild(span)
                    .AppendChild(hiddenSelector);

                // Initialize li and set inner text
                FluentTagBuilder liBuilder = new FluentTagBuilder("li")
                    .SetInnerHtml(containerDiv.Render());

                // Add li to listContentBuilder
                ulBuilder.AppendChild(liBuilder);
            }

            // Add error
            MvcHtmlString validationMessage = htmlHelper.ValidationMessageFor(expression);

            FluentTagBuilder errorSpan = new FluentTagBuilder("span")
                .AddCssClass(errorCssClass)
                .AppendChild(validationMessage);

            ulBuilder.AppendChild(errorSpan);

            return ulBuilder;
        }

        /// <summary>
        /// Create drop down button for selected property,
        /// with given source.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>        
        /// <param name="optionLabel">Label of drop down button if selection has not been made.</param>
        /// <returns>Drop down button mvc string.</returns>
        public static MvcHtmlString DropDownButtonFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source,
            string optionLabel)
        {
            return htmlHelper.DropDownButtonFor(expression, source, optionLabel, false, null);
        }

        /// <summary>
        /// Create drop down button for selected property,
        /// with given source.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>        
        /// <param name="optionLabel">Label of drop down button if selection has not been made.</param>
        /// <param name="allowDefault">Allow to select default item without value.</param>
        /// <returns>Drop down button mvc string.</returns>
        public static MvcHtmlString DropDownButtonFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source,
            string optionLabel,
            bool allowDefault)
        {
            return htmlHelper.DropDownButtonFor(expression, source, optionLabel, allowDefault, null);
        }

        /// <summary>
        /// Create drop down button for selected property,
        /// with given source.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression or source is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="source">Source of list to select from.</param>        
        /// <param name="optionLabel">Label of drop down button if selection has not been made.</param>
        /// <param name="allowDefault">Allow to select default item without value.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Drop down button mvc string.</returns>
        public static MvcHtmlString DropDownButtonFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList source,
            string optionLabel,
            bool allowDefault,
            object htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (source == null
                || source.Items == null)
                throw new ArgumentNullException(nameof(source));

            string dropDownCssClass = "dropdown-button";
            string buttonContainerCssClass = "dropdown-button-container";
            string headerCssClass = "dropdown-button-header";
            string menuCssClass = "dropdown-menu";
            string errorCssClass = "dropdown-button__error";
            string selectedCssClass = "selected";

            // Get model metadata
            ModelMetadata metadata = ModelMetadata
                .FromLambdaExpression(expression, htmlHelper.ViewData);

            // Convert html attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Get modelValue
            string modelValue = string.Empty;
            if (metadata.Model != null
                && !string.IsNullOrEmpty(metadata.Model.ToString()))
                modelValue = metadata.Model.ToString();

            // Define selected item
            string selectedValue = string.Empty;
            List<SelectListItem> listeItemSource = source
                .Items
                .Cast<SelectListItem>()
                .ToList();

            // Check if selected value explicitly setted in the source
            selectedValue = listeItemSource
                .Where(m => m.Selected)
                .Select(m => m.Value)
                .FirstOrDefault();

            /// If not explicitly set get model value as a selected value
            /// and set appropriate list item as selected
            if (string.IsNullOrEmpty(selectedValue)
                && !string.IsNullOrEmpty(modelValue))
            {
                selectedValue = modelValue;
                SelectListItem listItem = listeItemSource
                    .Where(m => m.Value.Equals(modelValue, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (listItem != null)
                    listItem.Selected = true;
            }

            // Initialize parentDiv and merge attributes
            FluentTagBuilder dropDownDiv = new FluentTagBuilder("div")
                .MergeAttributes(attributes)
                .AddCssClass(dropDownCssClass);

            // Initialize and add css class of button container
            FluentTagBuilder buttonContainerDiv = new FluentTagBuilder("div")
                .AddCssClass(buttonContainerCssClass);

            // If any item has been selected 
            // get text of that item as option label
            string buttonHeaderText = optionLabel;
            if (!string.IsNullOrEmpty(selectedValue))
            {
                string selectedItemText = listeItemSource
                    .Where(m => m.Value.Equals(selectedValue, StringComparison.OrdinalIgnoreCase))
                    .Select(m => m.Text)
                    .FirstOrDefault();

                // If selected item text found set buttonHeaderText to
                // this text
                if (!string.IsNullOrEmpty(selectedItemText))
                    buttonHeaderText = selectedItemText;
            }

            // Initialize down icon
            FluentTagBuilder downIcon = new FluentTagBuilder("i")
                .AddCssClass("default-icons")
                .SetInnerHtml("keyboard_arrow_down");

            // Initialize button header
            FluentTagBuilder buttonHeader = new FluentTagBuilder("span")
                .AddCssClass(headerCssClass)
                .SetInnerText(buttonHeaderText);

            // Add icon and header into container
            buttonContainerDiv.AppendChild(downIcon).AppendChild(buttonHeader);

            // Append button container to content builder of dropdown
            dropDownDiv.AppendChild(buttonContainerDiv);

            // Initialzie drop down menu
            FluentTagBuilder menu = new FluentTagBuilder("ul")
                .AddCssClass(menuCssClass);

            // If allow default set to true add option to
            // select default item
            if (allowDefault)
            {
                FluentTagBuilder defaultItem = htmlHelper.CreateDropDownButtonMenuItem(
                    expression,
                    new SelectListItem { Text = optionLabel, Value = "", Selected = false });

                // If model is null then add selected class to default item
                if (metadata.Model == null
                    || string.IsNullOrEmpty(metadata.Model.ToString()))
                    defaultItem.AddCssClass(selectedCssClass);

                menu.AppendChild(defaultItem);
            }

            // Loop throug list items and add them to menu
            foreach (SelectListItem listItem in listeItemSource)
            {
                // initialize menu item
                FluentTagBuilder menuItem = htmlHelper
                    .CreateDropDownButtonMenuItem(expression, listItem);

                // If model equals to current list item
                // add selected class to menu item
                if (!string.IsNullOrEmpty(selectedValue)
                    && string.Equals(
                        listItem.Value,
                        selectedValue,
                        StringComparison.OrdinalIgnoreCase))
                    menuItem.AddCssClass(selectedCssClass);

                menu.AppendChild(menuItem);
            }

            // Append menu to dropdown
            dropDownDiv.AppendChild(menu);

            // Initialize error span and add css class
            MvcHtmlString validationMessage = htmlHelper.ValidationMessageFor(expression);

            FluentTagBuilder errorSpan = new FluentTagBuilder("span")
                .AddCssClass(errorCssClass)
                .AppendChild(validationMessage);

            // Append error span to dropdown
            dropDownDiv.AppendChild(errorSpan);

            return new MvcHtmlString(dropDownDiv.Render());
        }

        /// <summary>
        /// Create drop down menu item for property
        /// selected by expression using provided list item.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Expression to select needed property.</param>
        /// <param name="listItem">List item to get text and value.</param>       
        /// <returns>List item with radio button and span.</returns>
        internal static FluentTagBuilder CreateDropDownButtonMenuItem<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectListItem listItem)
        {
            FluentTagBuilder menuItem = new FluentTagBuilder("li");

            // Initialize span
            FluentTagBuilder span = new FluentTagBuilder("span")
                .SetInnerText(listItem.Text);

            // Initialize radio button
            string fieldName = ExpressionHelper.GetExpressionText(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(
                expression, htmlHelper.ViewData);

            // Get validation attributes and add hidden attribute
            var htmlAttributes = htmlHelper
                .GetUnobtrusiveValidationAttributes(fieldName);
            htmlAttributes.Add("hidden", "hidden");

            MvcHtmlString radioButton = htmlHelper
                .RadioButton(fieldName, listItem.Value, listItem.Selected, htmlAttributes);

            // Set content of list menu item
            menuItem.AppendChild(span).AppendChild(radioButton);

            return menuItem;
        }
    }
}