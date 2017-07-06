using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models.Enums;
using System;
using System.Collections.Generic;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    // Part of ListEditorBuilder class
    public partial class ListEditorBuilder<TModel>
        : IServerListEditorBuilder<TModel>
        where TModel : class
    {
        private Func<TModel, object> DataRowAttributeGenerator { get; set; }

        internal IList<IListEditorActionCore<TModel>> ListActionCollection { get; set; } 
            = new List<IListEditorActionCore<TModel>>();
        internal IList<IListEditorActionCore<TModel>> ListItemActionCollection { get; set; }
            = new List<IListEditorActionCore<TModel>>();

        /// <summary>
        /// Create instance of IActionBuilder and add list editor actions.
        /// </summary>
        /// <param name="actionInitializer">Action to create instance of IActionBuilder.</param>
        /// <returns>Instance of IServerListEditorBuilder.</returns>
        public IServerListEditorBuilder<TModel> Actions(
            Action<IActionBuilder<TModel>> actionInitializer)
        {
            // Initialize action builder
            ActionBuilder<TModel> actionBuilder = new ActionBuilder<TModel>(this);
            actionInitializer(actionBuilder);
            return this;
        }

        /// <summary>
        /// Add list editor action to the action collection.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>Added action.</returns>
        internal IListEditorListAction<TModel> AddListAction(string name)
        {
            ListEditorAction<TModel> action = 
                new ListEditorAction<TModel>(ListEditorActionType.ListAction, name);
            ListActionCollection.Add(action);
            return action;
        }

        /// <summary>
        /// Add list editor item action to the action collection.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>Added action.</returns>
        internal IListEditorItemAction<TModel> AddItemAction(string name)
        {
            ListEditorAction<TModel> action =
                new ListEditorAction<TModel>(ListEditorActionType.ListItemAction, name);
            ListItemActionCollection.Add(action);
            return action;
        }

        /// <summary>
        /// Provide html attribute generator for list data rows.
        /// </summary>
        /// <exception cref="ArgumentNullException">When attribute generator is null.</exception>
        /// <param name="attributeGenerator">Attribute generator function.</param>
        /// <returns>Instance of IServerListEditorBuilder.</returns>
        public IServerListEditorBuilder<TModel> WithDataRowAttributes(Func<TModel, object> attributeGenerator)
        {
            if (attributeGenerator == null)
                throw new ArgumentNullException(nameof(attributeGenerator));

            DataRowAttributeGenerator = attributeGenerator;
            return this;
        }

        /// <summary>
        /// Construct configured actions for list data row which will be
        /// managed at server side..
        /// </summary>
        /// <param name="model">Model object to create links according to.</param>
        /// <returns>List data action container as as FluentTagBuilder.</returns>
        private FluentTagBuilder ConstructServerSideListItemActionContainer(TModel model)
        {
            // Css classes for buttons
            string listDataActionContainerCss = "list-data-action-container";

            // Initialize list data action container if needed
            FluentTagBuilder actionContainer = null;
            if(ListItemActionCollection.Count > 0)
                actionContainer = new FluentTagBuilder("div")
                    .AddCssClass(listDataActionContainerCss);

            foreach (IListEditorActionCore<TModel> action in ListItemActionCollection)
            {
                FluentTagBuilder link = action.GenerateLink(model);
                if (link != null)
                    actionContainer.AppendChild(link);
            }

            return actionContainer;
        }

        /// <summary>
        /// Construct list level action link container for list which will be 
        /// managed at server side.
        /// </summary>
        /// <returns>List action container.</returns>
        private FluentTagBuilder ConstructServerSideListActionContainer()
        {
            if (ListActionCollection == null
                || ListActionCollection.Count == 0)
                return null;
                        
            FluentTagBuilder listActionContainer = new FluentTagBuilder("div");
            foreach (IListEditorActionCore<TModel> action in ListActionCollection)
            {
                FluentTagBuilder listAction = action.GenerateLink();
                if (listAction != null)                
                    listActionContainer.AppendChild(listAction);                
            }

            return listActionContainer;
        }
    }
}
