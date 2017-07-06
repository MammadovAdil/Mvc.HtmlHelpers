using Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract;
using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Concrete
{
    public class SortingControlBuilder<TModel>
        : ISortingControlBuilder<TModel>
        where TModel : class
    {
        private HtmlHelper<TModel> Helper { get; set; }

        private SortingInfo Info { get; set; }     
        private Func<string, string> SortFieldUrlGenerator { get; set; } 
        private Func<SortOrder, string> SortOrderUrlGenerator { get; set; }

        private string AscendingLabel { get; set; }
        private string DescendingLabel { get; set; }

        // Configured sorting fields
        private IList<ISortingFieldCore<TModel>> SortingFieldCollection { get; set; }

        /// <summary>
        /// Cosntructor.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>
        public SortingControlBuilder(HtmlHelper<TModel> helper)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            // Initialize field collection
            SortingFieldCollection = new List<ISortingFieldCore<TModel>>();

            // Initialize properties
            AscendingLabel = "Asc";
            DescendingLabel = "Desc";

            Helper = helper;
        }

        /// <summary>
        /// Add sorting field to list.
        /// </summary>
        /// <typeparam name="TProperty">Type of property which is selected by expression.</typeparam>
        /// <param name="expression">Expression which selects property.</param>
        /// <returns>Instance of ISortingField.</returns>
        internal ISortingField<TModel> AddField<TProperty>(
            Expression<Func<TModel, TProperty>> expression)
        {
            // Initialize field
            SortingField<TModel, TProperty> sortingField =
                new SortingField<TModel, TProperty>(Helper, expression);

            // Add field to collection
            SortingFieldCollection.Add(sortingField);
            return sortingField;
        }

        /// <summary>
        /// Specify sorting enabled fields and their properties.
        /// </summary>
        /// <param name="fieldBuilder">Field builder to add fields.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        public ISortingControlBuilder<TModel> Fields(
            Action<ISortingFieldBuilder<TModel>> fieldBuilder)
        {
            if (fieldBuilder == null)
                throw new ArgumentNullException(nameof(fieldBuilder));

            SortingFieldBuilder<TModel> sortingFieldBuilder = 
                new SortingFieldBuilder<TModel>(this);
            fieldBuilder(sortingFieldBuilder);

            return this;
        }

        /// <summary>
        /// Provide current sorting info.
        /// </summary>
        /// <param name="info">Current sorting information.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        public ISortingControlBuilder<TModel> WithSortingInfo(SortingInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            Info = info;
            return this;
        }

        /// <summary>
        /// Specify url generator for sort field.
        /// </summary>
        /// <param name="sortFieldUrlGenerator">Url generator for sort field.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        public ISortingControlBuilder<TModel> WithSortFieldAction(
            Func<string, string> sortFieldUrlGenerator)
        {
            if (sortFieldUrlGenerator == null)
                throw new ArgumentNullException(nameof(sortFieldUrlGenerator));

            SortFieldUrlGenerator = sortFieldUrlGenerator;
            return this;
        }

        /// <summary>
        /// Specify url generator for sort order.
        /// </summary>
        /// <param name="sortOrderUrlGenerator">Url generator for sort order.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        public ISortingControlBuilder<TModel> WithSortOrderAction(
            Func<SortOrder, string> sortOrderUrlGenerator)
        {
            if (sortOrderUrlGenerator == null)
                throw new ArgumentNullException(nameof(sortOrderUrlGenerator));

            SortOrderUrlGenerator = sortOrderUrlGenerator;
            return this;
        }

        /// <summary>
        /// Set label for ascending order.
        /// </summary>
        /// <param name="label">Label to set for ascending order.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        public ISortingControlBuilder<TModel> WithAscendingLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            AscendingLabel = label;
            return this;
        }

        /// <summary>
        /// Set label for descending order.
        /// </summary>
        /// <param name="label">Label to set for descending order.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        public ISortingControlBuilder<TModel> WithDescendingLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            DescendingLabel = label;
            return this;
        }

        /// <summary>
        /// Render sorting controller.
        /// </summary>        
        /// <returns>MvcHtmlString.</returns>
        public MvcHtmlString Render()
        {
            return Render(null);
        }

        /// <summary>
        /// Render sorting controller.
        /// </summary>
        /// <param name="htmlAttributes">HtmlAttributes to add to control.</param>
        /// <returns>MvcHtmlString.</returns>
        public MvcHtmlString Render(object htmlAttributes)
        {
            if (SortingFieldCollection == null
                || SortingFieldCollection.Count == 0)
                throw new InvalidOperationException("At list one field must be configured to enable sorting.");

            string sortingControlCss = "sorting-control";
            string controlFieldCss = "sorting-control-field";            
            string controlOrderCss = "sorting-control-order";
            string controlHeaderCss = "sorting-control-header";
            string controlListCss = "sorting-control-list";
            string selectedCss = "selected";


            // Initialize sorting field list
            FluentTagBuilder sortingFieldList = new FluentTagBuilder("ul")
                .AddCssClass(controlListCss);

            // Currently sorted according to this field
            ISortingFieldCore<TModel> currentlySortedField = null;

            // Loop thorugh configured fields and add links for them
            foreach (ISortingFieldCore<TModel> fieldCore 
                in SortingFieldCollection)
            {
                string fieldPropertyName = fieldCore.GetPropertyName();

                FluentTagBuilder link = new FluentTagBuilder("a")
                    .MergeAttribute("href", SortFieldUrlGenerator(fieldPropertyName))
                    .SetInnerText(fieldCore.FieldLabel);

                FluentTagBuilder listItem = new FluentTagBuilder("li")
                    .AppendChild(link);

                if(fieldPropertyName == Info.PropertyName)
                {
                    // Store currently sorted field for further use
                    // and add selected class to it.
                    currentlySortedField = fieldCore;                    
                    listItem.AddCssClass(selectedCss);
                }

                // Append item to list
                sortingFieldList.AppendChild(listItem);                                
            }

            // Initialize down icon
            FluentTagBuilder downIcon = new FluentTagBuilder("i")
                .AddCssClass("default-icons")
                .SetInnerHtml("keyboard_arrow_down");

            // Initialize sorting field header
            FluentTagBuilder fieldHeaderSpan = new FluentTagBuilder("span")
                .SetInnerText(currentlySortedField.FieldLabel);
            FluentTagBuilder fieldHeader = new FluentTagBuilder("div")
                .AddCssClass(controlHeaderCss)
                .AppendChild(fieldHeaderSpan)
                .AppendChild(downIcon);

            // Initialize sorting field
            FluentTagBuilder sortingField = new FluentTagBuilder("div")
                .AddCssClass(controlFieldCss)
                .AppendChild(fieldHeader)
                .AppendChild(sortingFieldList);


            // Initialize order list
            FluentTagBuilder sortingOrderList = new FluentTagBuilder("ul")
                .AddCssClass(controlListCss);

            // Loop through availabel orders and create list items for them
            foreach (SortOrder order in Enum.GetValues(typeof(SortOrder)))
            {
                // Determine label
                string label = order == SortOrder.Asc
                    ? AscendingLabel
                    : DescendingLabel;

                FluentTagBuilder link = new FluentTagBuilder("a")
                    .MergeAttribute("href", SortOrderUrlGenerator(order))
                    .SetInnerText(label);

                FluentTagBuilder listItem = new FluentTagBuilder("li")
                    .AppendChild(link);

                if (order == Info.Order)
                    listItem.AddCssClass(selectedCss);

                // Append item to list
                sortingOrderList.AppendChild(listItem);
            }

            // Initialize sorting order header
            string orderHeaderLabel = Info.Order == SortOrder.Asc
                ? AscendingLabel
                : DescendingLabel;

            FluentTagBuilder orderHeaderSpan = new FluentTagBuilder("span")
                .SetInnerText(orderHeaderLabel);
            FluentTagBuilder orderHeader = new FluentTagBuilder("div")
                .AddCssClass(controlHeaderCss)
                .AppendChild(orderHeaderSpan)
                .AppendChild(downIcon);

            // Initialize sorting order
            FluentTagBuilder sortingOrder = new FluentTagBuilder("div")
                .AddCssClass(controlOrderCss)
                .AppendChild(orderHeader)
                .AppendChild(sortingOrderList);

            // Extract htmlAttribytes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Initialize sorting control, merge attributes and add controls
            FluentTagBuilder sortingControl = new FluentTagBuilder("div")
                .MergeAttributes(attributes)
                .AddCssClass(sortingControlCss)
                .AppendChild(sortingField)
                .AppendChild(sortingOrder);

            return MvcHtmlString.Create(sortingControl.Render());
        }
    }
}
