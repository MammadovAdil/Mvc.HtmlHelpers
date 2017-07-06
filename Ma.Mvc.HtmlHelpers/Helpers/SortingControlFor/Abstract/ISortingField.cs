namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract
{
    public interface ISortingField<TModel>
        where TModel : class
    {
        /// <summary>
        /// Provide label for the sorting field.
        /// </summary>
        /// <param name="label">Label to display for sorting field.</param>
        /// <returns>Instance of ISortingField.</returns>
        ISortingField<TModel> WithLabel(string label);

        /// <summary>
        /// Provide explicit name for propety, which is different than actual
        /// property name.
        /// </summary>
        /// <remarks>
        /// This needed mostly when hierarcy sturucture between actual
        /// model and view model does not match. For example, view model can
        /// have InsuredPerson.Pin whereas actual model have Pin in
        /// InsuredPerson.PartyRole.Party.Person.Pin.
        /// </remarks>
        /// <param name="explicitPropertyName">Explicit property name.</param>
        /// <returns>Instance of ISortingField.</returns>
        ISortingField<TModel> WithExplicitPropertyName(string explicitPropertyName);
    }
}
