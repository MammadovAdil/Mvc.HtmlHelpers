using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models.Enums;
using System;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    // Part of ListEditorBuilder class
    public partial class ListEditorBuilder<TModel>
        : IClientListEditorBuilder<TModel>
        where TModel : class
    {
        // Properties defining actions
        private bool IsAddEnabled { get; set; }
        private bool IsEditEnabled { get; set; }
        private bool IsDeleteEnabled { get; set; }
        private bool IsMoreInfoEnabled { get; set; }

        // Properties defining action labels
        private string AddButtonLabel { get; set; }
        private string EditButtonLabel { get; set; }
        private string DeleteButtonLabel { get; set; }
        private string MoreInfoButtonLabel { get; set; }

        private string SaveButtonLabel { get; set; }
        private string CancelButtonLabel { get; set; }
        private string CollectionName { get; set; }


        /// <summary>
        /// Enable adding new items to the list.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableAdd()
        {
            IsAddEnabled = true;
            return this;
        }
        /// <summary>
        /// Enable or disable adding new items to the list.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableAdd(bool enable)
        {
            IsAddEnabled = enable;
            return this;
        }

        /// <summary>
        /// Enable editing items.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableEdit()
        {
            IsEditEnabled = true;
            return this;
        }
        /// <summary>
        /// Enable or disable editing items.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableEdit(bool enable)
        {
            IsEditEnabled = enable;
            return this;
        }

        /// <summary>
        /// Enable deleting items.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableDelete()
        {
            IsDeleteEnabled = true;
            return this;
        }
        /// <summary>
        /// Disable or disable deleting items.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableDelete(bool enable)
        {
            IsDeleteEnabled = enable;
            return this;
        }

        /// <summary>
        /// Enable viewing more information.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableMoreInfo()
        {
            IsMoreInfoEnabled = true;
            return this;
        }
        /// <summary>
        /// Enable or disable viewing more information.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> EnableMoreInfo(bool enable)
        {
            IsMoreInfoEnabled = enable;
            return this;
        }

        /// <summary>
        /// Set label of add button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithAddButtonLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            AddButtonLabel = label;
            return this;
        }
        /// <summary>
        /// Set label of edit button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithEditButtonLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            EditButtonLabel = label;
            return this;
        }
        /// <summary>
        /// Set label of delete button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithDeleteButtonLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            DeleteButtonLabel = label;
            return this;
        }
        /// <summary>
        /// Set label of more info button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithMoreInfoButtonLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            MoreInfoButtonLabel = label;
            return this;
        }

        /// <summary>
        /// Set label of save button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithSaveButtonLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            SaveButtonLabel = label;
            return this;
        }

        /// <summary>
        /// Set label of cancel button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithCancelButtonLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            CancelButtonLabel = label;
            return this;
        }

        /// <summary>
        /// Set the name of bound collection.
        /// </summary>
        /// <param name="collectionName">
        /// Name of collection. Must be same with posted action parameter name.
        /// </param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        public IClientListEditorBuilder<TModel> WithCollectionName(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException(nameof(collectionName));

            CollectionName = collectionName;
            return this;
        }

        /// <summary>
        /// Construct editor according to configured fields.
        /// </summary>
        /// <returns>Constructed editor.</returns>
        private FluentTagBuilder ConstructListEditor()
        {
            // Declare and set css classes
            string listEditorCss = "list-editor";
            string listEditorRowCss = "list-editor-row";
            string listEditorRowLabelCss = "list-editor-row__label";
            string listEditorRowFieldCss = "list-editor-row__field";

            string listEditorDataTemplateCss = "list-editor-data__template";

            string listEditorActionContainerCss = "list-editor-action-container";
            string listEditorSaveActionCss = "list-editor-action__save";
            string listEditorCancelActionCss = "list-editor-action__cancel";

            // Initialize list editor
            FluentTagBuilder listEditor = new FluentTagBuilder("div")
                .AddCssClass(listEditorCss)
                .MergeAttribute("hidden", "hidden");

            foreach (IFieldCore<TModel> fieldCore in FieldCollection)
            {
                // Initialize editor label
                FluentTagBuilder editorLabel = null;

                // If editor type has not been set to Hidden add label for field
                if (fieldCore.EditorType != EditorType.Hidden)
                    editorLabel = new FluentTagBuilder("div")
                    .AddCssClass(listEditorRowLabelCss)
                    .AppendChild(fieldCore.LabelFor());

                // Initialize editor field
                FluentTagBuilder editorField = new FluentTagBuilder("div")
                    .AddCssClass(listEditorRowFieldCss)
                    .AppendChild(fieldCore.EditorFor());

                // Initialize editor row, add label and editor.
                FluentTagBuilder listEditorRow = new FluentTagBuilder("div")
                    .AddCssClass(listEditorRowCss)
                    .AppendChild(editorLabel)
                    .AppendChild(editorField);

                // Add row to list editor
                listEditor.AppendChild(listEditorRow);
            }

            // Initialize actions
            FluentTagBuilder saveAction = new FluentTagBuilder("button")
                .MergeAttribute("type", "button")
                .AddCssClass(listEditorSaveActionCss)
                .SetInnerText(SaveButtonLabel);

            FluentTagBuilder cancelAction = new FluentTagBuilder("button")
                .MergeAttribute("type", "button")
                .AddCssClass(listEditorCancelActionCss)
                .SetInnerText(CancelButtonLabel);

            FluentTagBuilder actionContainer = new FluentTagBuilder("div")
                .AddCssClass(listEditorActionContainerCss)
                .AppendChild(saveAction)
                .AppendChild(cancelAction);

            // Add actions panel to list editor
            listEditor.AppendChild(actionContainer);

            // Initialize data template for adding or editing items.
            string dataTemplateIndex = "_template_index_";
            string htmlFieldPrefix = string.Format(
                "{0}[{1}]", CollectionName, dataTemplateIndex);

            // Use BeginHtmlFieldPrefixScope instead of BeginCollectionItem,
            // because these items must not bound when data is posted,
            // and this index must not be reused by BeginCollectionItem
            // to keep consistency.
            using (Helper.BeginHtmlFieldPrefixScope(htmlFieldPrefix))
            {
                MvcHtmlString hiddenIndexer = Helper.HiddenIndexerForModel(
                    new { disabled = "disabled" });

                FluentTagBuilder listDataRowTemplate = ConstructListDataRow(null)
                    .AppendChild(hiddenIndexer)
                    .AppendChild(ConstructClientSideListItemActionContainer(null));

                FluentTagBuilder dataTemplate = new FluentTagBuilder("div")
                    .MergeAttribute("hidden", "hidden")
                    .AddCssClass(listEditorDataTemplateCss)
                    .AppendChild(listDataRowTemplate);

                // Append dataTemplate to list editor.
                listEditor.AppendChild(dataTemplate);
            }

            return listEditor;
        }

        /// <summary>
        /// Construct configured actions for list data row which will be
        /// managed at client side..
        /// </summary>
        /// <param name="model">Model to construct action button container for.</param>
        /// <returns>List data action container as as FluentTagBuilder.</returns>
        private FluentTagBuilder ConstructClientSideListItemActionContainer(TModel model)
        {
            // Css classes for buttons
            string listDataActionContainerCss = "list-data-action-container";
            string listDataEditActionButtonCss = "list-data-action-button__edit";
            string listDataDeleteActionButtonCss = "list-data-action-button__delete";

            // Initiazlie edit and delete buttons
            FluentTagBuilder editButton = null;
            FluentTagBuilder deleteButton = null;

            if (IsEditEnabled)
                editButton = new FluentTagBuilder("button")
                    .MergeAttribute("type", "button")
                    .AddCssClass(listDataEditActionButtonCss)
                    .SetInnerText(EditButtonLabel);

            if (IsDeleteEnabled)
                deleteButton = new FluentTagBuilder("button")
                    .MergeAttribute("type", "button")
                    .AddCssClass(listDataDeleteActionButtonCss)
                    .SetInnerText(DeleteButtonLabel);

            // Initialize list data action container if needed
            FluentTagBuilder actionContainer = null;

            if (IsEditEnabled || IsDeleteEnabled)
                actionContainer = new FluentTagBuilder("div")
                    .AddCssClass(listDataActionContainerCss)
                    .AppendChild(editButton)
                    .AppendChild(deleteButton);

            return actionContainer;
        }

        /// <summary>
        /// Construct list level action button container for list which will be 
        /// managed at client side.
        /// </summary>
        /// <returns>Add action container.</returns>
        private FluentTagBuilder ConstructClientSideListActionContainer()
        {
            // If Add is not enabled return null
            if (!IsAddEnabled)
                return null;

            string listDataAddActionButtonCss = "list-data-action-button__add";
            FluentTagBuilder addAction = new FluentTagBuilder("button")
                .MergeAttribute("type", "button")
                .AddCssClass(listDataAddActionButtonCss)
                .SetInnerText(AddButtonLabel);

            FluentTagBuilder addActionContainer = new FluentTagBuilder("div")
                    .AppendChild(addAction);

            return addActionContainer;
        }
    }
}
