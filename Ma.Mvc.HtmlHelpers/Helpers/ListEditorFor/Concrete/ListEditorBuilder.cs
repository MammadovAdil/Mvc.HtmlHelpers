using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    /// <summary>
    /// Build a list editor for list of model objects.
    /// </summary>
    /// <remarks>
    /// This is partial class. Seperated into parts for simplicity. Other parts are at
    /// ServerListEditorBuilder.cs and ClientListEditorBuilder.cs files.
    /// </remarks>
    /// <typeparam name="TModel">Type of model to render as a list editor.</typeparam>
    public partial class ListEditorBuilder<TModel>
        : IListEditorBuilder<TModel>
        where TModel : class
    {
        private HtmlHelper<TModel> Helper { get; set; }
        private IEnumerable<TModel> Data { get; set; }

        internal IList<IFieldCore<TModel>> FieldCollection { get; set; }        
        
        private RouteValueDictionary DataRowAttibutes { get; set; }        

        // Type of list editor
        private ListEditorBuilderType BuilderType { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="helper">Html helper to render html.</param>
        internal ListEditorBuilder(HtmlHelper<TModel> helper)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            // Initialize field collection
            FieldCollection = new List<IFieldCore<TModel>>();

            // Set default values for properties
            IsAddEnabled = false;
            IsEditEnabled = false;
            IsDeleteEnabled = false;
            IsMoreInfoEnabled = false;

            AddButtonLabel = "+ Add new";
            EditButtonLabel = "Edit";
            DeleteButtonLabel = "Delete";
            MoreInfoButtonLabel = "More info";

            SaveButtonLabel = "Save";
            CancelButtonLabel = "Cancel";

            Helper = helper;
        }

        /// <summary>
        /// Set the data source. 
        /// </summary>
        /// <param name="dataSource">Enumerable list of model objects.</param>
        /// <returns>Instance of ListEditorBuilder</returns>
        internal IListEditorBuilder<TModel> DataSource(
            IEnumerable<TModel> dataSource)
        {
            Data = dataSource;
            return this;
        }

        /// <summary>
        /// Add lambda expression as a field.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to be added as a field.</typeparam>
        /// <param name="expression">Lambda expression identifying the property.</param>
        /// <returns>An instance of field.</returns>
        internal IField<TModel> AddField<TProperty>(
            Expression<Func<TModel, TProperty>> expression)
        {
            Field<TModel, TProperty> field =
                new Field<TModel, TProperty>(Helper, expression);

            FieldCollection.Add(field);
            return field;
        }

        /// <summary>
        /// Create instance of the IFieldBuilder and add fields.
        /// </summary>
        /// <param name="fieldInitializer">Action to create instance of FieldBuilder.</param>
        /// <returns>An instance of LisEditorBuilder.</returns>
        public IListEditorBuilder<TModel> Fields(
            Action<IFieldBuilder<TModel>> fieldInitializer)
        {
            FieldBuilder<TModel> fieldBuilder = new FieldBuilder<TModel>(this);
            fieldInitializer(fieldBuilder);
            return this;
        }

        /// <summary>
        /// Provide html attributes for list data rows.
        /// </summary>
        /// <param name="htmlAttributes">Html attributes to add to list data row.</param>
        /// <returns>An instance of ILisEditorBuilder.</returns>
        public IListEditorBuilder<TModel> WithDataRowAttributes(object htmlAttributes)
        {
            DataRowAttibutes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);
            return this;
        }        

        /// <summary>
        /// Render list editor builder which will be managed at server side.
        /// </summary>
        /// <returns>ILisEditorBuilder as IServerListEditorBuilder.</returns>
        public IServerListEditorBuilder<TModel> ManageAtServerSide()
        {
            BuilderType = ListEditorBuilderType.ManagedAtServerSide;
            return this;
        }

        /// <summary>
        /// Rendered list editor builder which will be managed at client side.
        /// </summary>
        /// <returns>ILisEditorBuilder as IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> ManageAtClientSide()
        {
            BuilderType = ListEditorBuilderType.ManagedAtClientSide;
            return this;
        }


        /// <summary>
        /// Render list editor builder as list.
        /// </summary>
        /// <remarks>
        /// This is implementation of RenderList for
        /// both of IServerListEditorBuilder and IClientListEditorBuilder.
        /// </remarks>
        /// <returns>ListEditorBuilder as MvcHtmlString format.</returns>
        public MvcHtmlString RenderList()
        {
            return RenderList(null);
        }

        /// <summary>
        /// Render list editor builder as list.
        /// </summary>
        /// <remarks>
        /// This is implementation of RenderList(object htmlAttributes) for
        /// both of IServerListEditorBuilder and IClientListEditorBuilder.
        /// </remarks>
        /// <param name="htmlAttributes">Html attributes to add to list.</param>
        /// <returns>ListEditorBuilder as MvcHtmlString format.</returns>
        public MvcHtmlString RenderList(object htmlAttributes)
        {
            // Collection name must be provided for proper binding
            // list editor which is manged at client side
            if (BuilderType == ListEditorBuilderType.ManagedAtClientSide
                && string.IsNullOrEmpty(CollectionName))
                throw new InvalidOperationException(
                    "Name of collection must be provided before rendering.");

            // Declare and set css class names
            string listEditorForCss = "list-editor-for";
            string listDataCss = "list-data";
            string listDataListActionContainerCss = "list-action_container";

            // Get html attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Create div element and merge attributes
            FluentTagBuilder listEditorFor = new FluentTagBuilder("div")
                .MergeAttributes(attributes)
                .AddCssClass(listEditorForCss);

            // If list editor will be managed at client side we will need an editor.
            if (BuilderType == ListEditorBuilderType.ManagedAtClientSide)
            {
                // Construct list editor. 
                // List editor must be constructed prior to list data. 
                // Otherwise validation attributes will be
                // added to hidden input elements inside list data.
                listEditorFor.AppendChild(ConstructListEditor());
            }

            // Initialize list data div
            FluentTagBuilder listData = new FluentTagBuilder("div")
                .AddCssClass(listDataCss);

            // Initialize list action container and add to listData if needed.
            FluentTagBuilder listActionContainer = null;
            if (BuilderType == ListEditorBuilderType.ManagedAtClientSide)
                listActionContainer = ConstructClientSideListActionContainer();
            else if (BuilderType == ListEditorBuilderType.ManagedAtServerSide)
                listActionContainer = ConstructServerSideListActionContainer();

            if (listActionContainer != null)
            {
                listActionContainer.AddCssClass(listDataListActionContainerCss);
                listData.AppendChild(listActionContainer);
            }

            if (Data != null && Data.Count() > 0)
            {
                if (BuilderType == ListEditorBuilderType.ManagedAtClientSide)
                {
                    // If builder will be managed at client side we need
                    // to add indexers to be able to bind correctly
                    // and action buttons for managing items.

                    foreach (TModel model in Data)
                    {
                        using (Helper.BeginCollectionItem(CollectionName, false))
                        {
                            // Append hidden indexer for correctly binding 
                            // data and action container for the row.
                            FluentTagBuilder listDataRow = ConstructListDataRow(model)
                                .AppendChild(Helper.HiddenIndexerForModel())
                                .AppendChild(ConstructClientSideListItemActionContainer(model));

                            // Add listDataRow to listData
                            listData.AppendChild(listDataRow);
                        }
                    }
                }
                else
                {
                    // If builder will be managed at server side we need
                    // to add links for managing items.

                    foreach (TModel model in Data)
                    {
                        FluentTagBuilder listDataRow = ConstructListDataRow(model)
                            .AppendChild(ConstructServerSideListItemActionContainer(model));

                        // Add listDataRow to listData
                        listData.AppendChild(listDataRow);
                    }
                }
            }

            listEditorFor.AppendChild(listData);

            return new MvcHtmlString(listEditorFor.Render());
        }

        /// <summary>
        /// Construct one row of list editor.
        /// </summary>
        /// <param name="model">Model object to construct list row according to.</param>
        /// <returns>Constructed row of list.</returns>
        private FluentTagBuilder ConstructListDataRow(TModel model)
        {
            // Declare and set css class names
            string listDataRowCss = "list-data-row";
            string listDataFieldRowCss = "list-data-field-row";
            string listDataCalptionFieldRowCss = "list-data-field-row__caption";
            string listdataImageFiledRowCss = "list-data-field-row__iamge";
            string listDataLabelCss = "list-data-display__label";
            string listDataDisplayFieldCss = "list-data-display__field";

            // Store current model for further use
            TModel oldModel = Helper.ViewData.Model;

            try
            {
                // Set model object of HtmlHelper to be able 
                // to get correct display, editor and hidden for fields.
                Helper.ViewData.Model = model;

                // Create list data row and merge attributes
                FluentTagBuilder listDataRow = new FluentTagBuilder("div")
                    .MergeAttributes(GetDataRowAttributes(model))
                    .AddCssClass(listDataRowCss);

                // Loop through each configured field and add them to the row.
                foreach (IFieldCore<TModel> fieldCore in FieldCollection)
                {
                    /// Do not render field if provided condition is not met
                    /// when list is managed at server side.
                    if (BuilderType == ListEditorBuilderType.ManagedAtServerSide
                        && !fieldCore.ShouldBeRendered(model))
                        continue;

                    if (fieldCore.EditorType != EditorType.Hidden)
                    {
                        // If EditorType has not been set to Hidden, 
                        // construct normal field row

                        // Initialize field
                        FluentTagBuilder displayField = new FluentTagBuilder("div")
                            .AddCssClass(listDataDisplayFieldCss);

                        // If field should be rendered as image construct image tag
                        // for the field otherwise costruct span for it.
                        if (fieldCore.IsAsImage)
                        {
                            FluentTagBuilder iamgeField = new FluentTagBuilder("img")
                                .MergeAttribute("src", fieldCore.Evaluate(model));

                            displayField.AppendChild(iamgeField);
                        }
                        else
                        {
                            FluentTagBuilder displaySpan = new FluentTagBuilder("span")
                                .AppendChild(fieldCore.DisplayFor());

                            displayField.AppendChild(displaySpan);
                        }

                        // If this list editor will be managed at client side,
                        // add hidden input field to be able to bind correctly.
                        if (BuilderType == ListEditorBuilderType.ManagedAtClientSide)
                            displayField.AppendChild(fieldCore.HiddenFor());

                        // Initialize row for label and field
                        FluentTagBuilder fieldRow = new FluentTagBuilder("div")
                            .AddCssClass(listDataFieldRowCss);

                        // Add caption field row css class if needed
                        if (fieldCore.IsAsCaption)
                            fieldRow.AddCssClass(listDataCalptionFieldRowCss);

                        // Add image filed row css class if needed
                        if (fieldCore.IsAsImage)
                            fieldRow.AddCssClass(listdataImageFiledRowCss);

                        // Add label for field if needed.
                        if (!fieldCore.IsWithoutLabel)
                        {
                            // Initialize label for field
                            FluentTagBuilder label = new FluentTagBuilder("div")
                                .AddCssClass(listDataLabelCss)
                                .AppendChild(fieldCore.LabelFor());

                            fieldRow.AppendChild(label);
                        }

                        // Append display field to the row.
                        fieldRow.AppendChild(displayField);

                        // Add fieldRow to listEditorRow
                        listDataRow.AppendChild(fieldRow);
                    }
                    else
                    {
                        // Initialize row for label and field
                        FluentTagBuilder fieldRow = new FluentTagBuilder("div")
                            .AddCssClass(listDataFieldRowCss)
                            .AppendChild(fieldCore.HiddenFor());

                        // If EditorType has been set to Hidden, 
                        // just construct hidden input for field
                        listDataRow.AppendChild(fieldRow);
                    }
                }

                return listDataRow;
            }
            finally
            {
                // Restore model of helper
                Helper.ViewData.Model = oldModel;
            }
        }

        /// <summary>
        /// Get data row attributes.
        /// </summary>
        /// <param name="model">Model to define attributes according to if needed.</param>
        /// <returns>Data row attributes.</returns>
        private RouteValueDictionary GetDataRowAttributes(TModel model)
        {
            if(DataRowAttributeGenerator != null
                && model != null)
            {
                object anonymousHtmlAttributes = DataRowAttributeGenerator(model);
                RouteValueDictionary htmlAttributes = HtmlHelper
                    .AnonymousObjectToHtmlAttributes(anonymousHtmlAttributes);
                return htmlAttributes;
            }

            return DataRowAttibutes;
        }
    }
}