using Ma.Mvc.HtmlHelpers.Models.Enums;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Data container for field element.
    /// </summary>
    /// <typeparam name="TModel">Tyoe of view model.</typeparam>
    internal interface IFieldCore<TModel>
    {
        string FieldLabel { get; set; }
        bool IsWithoutLabel { get; set; }
        bool IsAsCaption { get; set; }
        bool IsAsImage { get; set; }
        EditorType EditorType { get; set; }
        IEnumerable<SelectListItem> FieldSource { get; set; }       

        bool ShouldBeRendered(TModel model);
        string Evaluate(TModel model);        

        MvcHtmlString LabelFor();
        MvcHtmlString DisplayFor();
        MvcHtmlString EditorFor();
        MvcHtmlString HiddenFor();
    }
}