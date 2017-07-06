using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    /// <summary>
    /// Helpers for list editors.
    /// </summary>
    /// <remarks>
    /// List editor helpers created by Adil Mammadov
    /// according to following web post:
    /// http://aspnet.wikidot.com/page:creating-custom-html-helper-render-table
    /// </remarks>
    public static class ListEditorHelpers
    {
        public static IListEditorBuilder<TProperty> ListEditorFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, IEnumerable<TProperty>>> expression)
            where TModel : class
            where TProperty : class
        {
            // Compile the expression to get the data source.
            Func<TModel, IEnumerable<TProperty>> retrieveDataSource = expression.Compile();

            // Retrieve data source from model
            IEnumerable<TProperty> dataSource = retrieveDataSource(helper.ViewData.Model);            

            // Initialize strongly typed helper for selected property
            HtmlHelper<TProperty> htmlHelper = new HtmlHelper<TProperty>(
                helper.ViewContext, new ViewPage());
            // Set TemplateInfo for indexes.
            htmlHelper.ViewData.TemplateInfo = helper.ViewData.TemplateInfo;

            // Initialize list editor builder
            ListEditorBuilder<TProperty> builder =
                new ListEditorBuilder<TProperty>(htmlHelper);

            // Set the data source of builder
            builder.DataSource(dataSource);

            return builder;
        }
    }
}