using Ma.Mvc.HtmlHelpers.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Create combo date selector for date time.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Property selector expression.</param>
        /// <returns>Combo date selector.</returns>
        public static MvcHtmlString ComboDateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, DateTime>> expression)
        {
            return htmlHelper.ComboDateFor(expression, null, null);
        }

        /// <summary>
        /// Create combo date selector for date time.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Property selector expression.</param>
        /// <param name="yearRange">Range of years.</param>
        /// <returns>Combo date selector.</returns>
        public static MvcHtmlString ComboDateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, DateTime>> expression,
            IEnumerable<int> yearRange)
        {
            return htmlHelper.ComboDateFor(expression, yearRange, null);
        }

        /// <summary>
        /// Create combo date selector for date time.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Property selector expression.</param>
        /// <param name="htmlAttributes">Html attributes to add to combo date.</param>
        /// <returns>Combo date selector.</returns>
        public static MvcHtmlString ComboDateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, DateTime>> expression,
            object htmlAttributes)
        {
            return htmlHelper.ComboDateFor(expression, null, htmlAttributes);
        }

        /// <summary>
        /// Create combo date selector for date time.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Property selector expression.</param>
        /// <param name="yearRange">Range of years.</param>
        /// <param name="htmlAttributes">Html attributes to add to combo date.</param>
        /// <returns>Combo date selector.</returns>
        public static MvcHtmlString ComboDateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, DateTime>> expression,
            IEnumerable<int> yearRange,
            object htmlAttributes)

        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            string comboDateCss = "combo-date";
            string dayContainerCss = "day-container";
            string monthContainerCss = "month-container";
            string yearContainerCss = "year-container";
            string errorCss = "combo-date__error";

            string dayText = "Gün";
            string monthText = "Ay";
            string yearText = "İl";

            // Initialize yearRange if has not been provided
            if (yearRange == null)
                yearRange = Enumerable.Range(1900, 200);

            // Get model metadata
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(
                expression,
                htmlHelper.ViewData);

            string modelName = ExpressionHelper.GetExpressionText(expression);

            // Append HtmlFieldPrefix if there is any
            string fieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;

            if (!string.IsNullOrEmpty(fieldPrefix))
                modelName = string.Format("{0}.{1}", fieldPrefix, modelName);

            // Convert html attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Initialize container div
            FluentTagBuilder comboDate = new FluentTagBuilder("div")
                .MergeAttributes(attributes)
                .AddCssClass(comboDateCss);

            // Initialize hidden text box for client side validation
            FluentTagBuilder input = new FluentTagBuilder("input")
                .MergeAttribute("name", modelName)
                .MergeAttribute("id", modelName)
                .MergeAttribute("type", "date")
                .MergeAttribute("hidden", "hidden")
                .MergeAttribute("readonly", "readonly");

            if (metadata.Model != null)
            {
                DateTime value = Convert.ToDateTime(metadata.Model);
                input.MergeAttribute("value", value.ToString("yyyy-MM-dd"));
            }

            //// Get validation attributes
            IDictionary<string, object> validationAttributes =
                htmlHelper.GetUnobtrusiveValidationAttributes(modelName, metadata);

            // Merge validation attributes
            input.MergeAttributes(validationAttributes);

            //contentBuilder.AppendLine(input.ToString());
            comboDate.AppendChild(input);

            // Declare date property selector 
            Expression<Func<TModel, Int32>> datePropertySelector;

            // Select day property of date
            MemberExpression dayExpression = Expression.Property(expression.Body, "Day");
            datePropertySelector = Expression.Lambda<Func<TModel, Int32>>(
                dayExpression,
                expression.Parameters);

            // Create drop down button for day
            MvcHtmlString daySelector = htmlHelper
                .DropDownButtonFor<TModel, int>(
                    datePropertySelector,
                    new SelectList(Enumerable
                        .Range(1, 31)
                        .Select(m => new SelectListItem
                        {
                            Text = m.ToString("00"),
                            Value = m.ToString()
                        })),
                    dayText);

            // Setup day container
            FluentTagBuilder dayContainer = new FluentTagBuilder("div")
                .AddCssClass(dayContainerCss)
                .AppendChild(daySelector);

            //contentBuilder.AppendLine(dayContainer.ToString());

            comboDate.AppendChild(dayContainer);

            // Select month property of date
            MemberExpression monthExpression = Expression.Property(expression.Body, "Month");
            datePropertySelector = Expression.Lambda<Func<TModel, Int32>>(
                monthExpression,
                expression.Parameters);

            // Create drop down button for month
            MvcHtmlString monthSelector = htmlHelper
                .DropDownButtonFor<TModel, int>(
                    datePropertySelector,
                    new SelectList(Enumerable.Range(1, 12)
                        .Select(r => new SelectListItem
                        {
                            Value = r.ToString(),
                            Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(r)
                        })),
                    monthText);

            // Setup month container
            FluentTagBuilder monthContainer = new FluentTagBuilder("div")
                .AddCssClass(monthContainerCss)
                .AppendChild(monthSelector);

            //contentBuilder.AppendLine(monthContainer.ToString());

            comboDate.AppendChild(monthContainer);

            // Select year property of date
            MemberExpression yearExpression = Expression.Property(expression.Body, "Year");
            datePropertySelector = Expression.Lambda<Func<TModel, Int32>>(
                yearExpression,
                expression.Parameters);

            // Create drop down button for month
            MvcHtmlString yearSelector = htmlHelper
                .DropDownButtonFor<TModel, int>(
                    datePropertySelector,
                    new SelectList(yearRange
                        .Select(r => new SelectListItem
                        {
                            Text = r.ToString(),
                            Value = r.ToString()
                        })),
                    yearText);

            // Setup year container
            FluentTagBuilder yearContainer = new FluentTagBuilder("div")
                .AddCssClass(yearContainerCss)
                .AppendChild(yearSelector);

            comboDate.AppendChild(yearContainer);

            // Set up error span
            MvcHtmlString validationMessage = htmlHelper
                .ValidationMessageFor(expression);

            FluentTagBuilder errorSpan = new FluentTagBuilder("span")
                .AddCssClass(errorCss)
                .AppendChild(validationMessage);

            comboDate.AppendChild(errorSpan);

            return new MvcHtmlString(comboDate.Render());
        }


        /// <summary>
        /// Create combo date selector for nullable date time.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When expression is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="expression">Property selector expression.</param>
        /// <param name="yearRange">Range of years.</param>
        /// <param name="htmlAttributes">Html attributes to add to combo date.</param>
        /// <returns>Combo date selector.</returns>
        public static MvcHtmlString ComboDateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, DateTime?>> expression,
            IEnumerable<int> yearRange,
            object htmlAttributes)

        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            string comboDateCss = "combo-date";
            string dayContainerCss = "day-container";
            string monthContainerCss = "month-container";
            string yearContainerCss = "year-container";
            string errorCss = "combo-date__error";

            string dayText = "Gün";
            string monthText = "Ay";
            string yearText = "İl";

            // Initialize yearRange if has not been provided
            if (yearRange == null)
                yearRange = Enumerable.Range(1900, 200);

            // Get model metadata
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(
                expression,
                htmlHelper.ViewData);

            string modelName = ExpressionHelper.GetExpressionText(expression);

            // Append HtmlFieldPrefix if there is any
            string fieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;

            if (!string.IsNullOrEmpty(fieldPrefix))
                modelName = string.Format("{0}.{1}", fieldPrefix, modelName);

            // Convert html attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Initialize container div
            FluentTagBuilder comboDate = new FluentTagBuilder("div")
                .MergeAttributes(attributes)
                .AddCssClass(comboDateCss);

            // Initialize hidden text box for client side validation
            FluentTagBuilder input = new FluentTagBuilder("input")
                .MergeAttribute("name", modelName)
                .MergeAttribute("id", modelName)
                .MergeAttribute("type", "date")
                .MergeAttribute("hidden", "hidden")
                .MergeAttribute("readonly", "readonly");

            if (metadata.Model != null)
            {
                DateTime value = Convert.ToDateTime(metadata.Model);
                input.MergeAttribute("value", value.ToString("yyyy-MM-dd"));
            }

            //// Get validation attributes
            IDictionary<string, object> validationAttributes =
                htmlHelper.GetUnobtrusiveValidationAttributes(modelName, metadata);

            // Merge validation attributes
            input.MergeAttributes(validationAttributes);

            //contentBuilder.AppendLine(input.ToString());
            comboDate.AppendChild(input);

            // Declare date property selector 
            Expression<Func<TModel, Int32>> datePropertySelector;

            // Select day property of date
            // Initialize model to  be able to create drop downs
            bool isModelNull = false;
            if (metadata.Model == null)
            {
                var splitedProps = modelName.Split('.');
                object model = htmlHelper.ViewData.Model;

                for (byte i = 0; i < splitedProps.Length; i++)
                {
                    var propInfo = model.GetType().GetProperty(splitedProps[i]);
                    object value = null;

                    if (i == splitedProps.Length - 1)
                        value = new Nullable<DateTime>(new DateTime());
                    else
                        value = Activator.CreateInstance(propInfo.PropertyType);

                    propInfo.SetValue(model, value);
                    model = value;
                }

                // Set flag to true
                isModelNull = true;
            }

            MemberExpression valueAccess = Expression.Property(expression.Body, "Value");
            MemberExpression dayExpression = Expression.Property(valueAccess, "Day");
            datePropertySelector = Expression.Lambda<Func<TModel, Int32>>(
                dayExpression,
                expression.Parameters);

            // Create drop down button for day
            List<SelectListItem> dayValues = Enumerable
                .Range(1, 31)
                .Select(m => new SelectListItem
                {
                    Text = m.ToString("00"),
                    Value = m.ToString()
                })
                .ToList();
            dayValues.Insert(0, new SelectListItem
            {
                Text = dayText,
                Value = "0",
                Selected = isModelNull
            });
            MvcHtmlString daySelector = htmlHelper
                .DropDownButtonFor<TModel, int>(
                    datePropertySelector,
                    new SelectList(dayValues),
                    dayText);

            // Setup day container
            FluentTagBuilder dayContainer = new FluentTagBuilder("div")
                .AddCssClass(dayContainerCss)
                .AppendChild(daySelector);

            //contentBuilder.AppendLine(dayContainer.ToString());

            comboDate.AppendChild(dayContainer);

            // Select month property of date
            MemberExpression monthExpression = Expression.Property(valueAccess, "Month");
            datePropertySelector = Expression.Lambda<Func<TModel, Int32>>(
                monthExpression,
                expression.Parameters);

            // Create drop down button for month
            List<SelectListItem> monthValues = Enumerable.Range(1, 12)
                    .Select(r => new SelectListItem
                    {
                        Value = r.ToString(),
                        Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(r)
                    })
                    .ToList();
            monthValues.Insert(0, new SelectListItem
            {
                Text = monthText,
                Value = "0",
                Selected = isModelNull
            });
            MvcHtmlString monthSelector = htmlHelper
                .DropDownButtonFor<TModel, int>(
                    datePropertySelector,
                    new SelectList(monthValues),
                    monthText);

            // Setup month container
            FluentTagBuilder monthContainer = new FluentTagBuilder("div")
                .AddCssClass(monthContainerCss)
                .AppendChild(monthSelector);

            //contentBuilder.AppendLine(monthContainer.ToString());

            comboDate.AppendChild(monthContainer);

            // Select year property of date
            MemberExpression yearExpression = Expression.Property(valueAccess, "Year");
            datePropertySelector = Expression.Lambda<Func<TModel, Int32>>(
                yearExpression,
                expression.Parameters);

            // Create drop down button for month
            List<SelectListItem> yearValues = yearRange
                    .Select(r => new SelectListItem
                    {
                        Text = r.ToString(),
                        Value = r.ToString()
                    })
                    .ToList();
            yearValues.Insert(0, new SelectListItem
            {
                Text = yearText,
                Value = "0",
                Selected = isModelNull
            });
            MvcHtmlString yearSelector = htmlHelper
                .DropDownButtonFor<TModel, int>(
                    datePropertySelector,
                    new SelectList(yearValues),
                    yearText);

            // Setup year container
            FluentTagBuilder yearContainer = new FluentTagBuilder("div")
                .AddCssClass(yearContainerCss)
                .AppendChild(yearSelector);

            comboDate.AppendChild(yearContainer);

            // Set up error span
            MvcHtmlString validationMessage = htmlHelper
                .ValidationMessageFor(expression);

            FluentTagBuilder errorSpan = new FluentTagBuilder("span")
                .AddCssClass(errorCss)
                .AppendChild(validationMessage);

            comboDate.AppendChild(errorSpan);

            return new MvcHtmlString(comboDate.Render());
        }
    }
}
