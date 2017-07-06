using System.Collections.Generic;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    public interface ISelectField<TModel>
    {
        ISelectField<TModel> WithDataSource(
            IEnumerable<SelectListItem> dataSource);
    }
}