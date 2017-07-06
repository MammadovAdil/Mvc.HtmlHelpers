using Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Concrete
{
    /// <summary>
    /// Sorting field.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    /// <typeparam name="TProperty">Type of property.</typeparam>
    public class SortingField<TModel, TProperty>
        : ISortingFieldCore<TModel>,
        ISortingField<TModel>
        where TModel : class
    {
        private string fieldLabel;

        /// <summary>
        /// Label for sorting field.
        /// </summary>
        public string FieldLabel
        {
            get
            {
                return fieldLabel ?? Metadata.DisplayName ?? Metadata.PropertyName;
            }
            set
            {
                fieldLabel = value;
            }
        }

        /// <summary>
        /// Explicitly set name of selected property.
        /// </summary>
        public string ExplicitPropertyName { get; set; }

        /// <summary>
        /// Get name of selected property.
        /// </summary>
        public string GetPropertyName()
        {
            // If property name has been set explicitly then return it
            if (!string.IsNullOrEmpty(ExplicitPropertyName))
                return ExplicitPropertyName;

            MemberExpression currentExpression = MemberExpression;
            string propertyAccess = currentExpression.Member.Name;

            /// If this is neted MemberAccess call, then construct full path to proeprty.
            while (currentExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                currentExpression = currentExpression.Expression as MemberExpression;
                propertyAccess = string.Format("{0}.{1}",
                    currentExpression.Member.Name,
                    propertyAccess);
            }

            return propertyAccess;
        }

        /// <summary>
        /// Html helper
        /// </summary>
        internal HtmlHelper<TModel> Helper { get; set; }

        /// <summary>
        /// Metadata for model according to expression.
        /// </summary>
        private ModelMetadata Metadata { get; set; }

        /// <summary>
        /// MemberExpression according to expression which selects property.
        /// </summary>
        private MemberExpression MemberExpression { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>
        /// <param name="expression">Field selector expression.</param>
        public SortingField(
            HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            Helper = helper;

            // Get metatadata from lambda expression
            Metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // Get member expression from Expresion
            MemberExpression = (MemberExpression)expression.Body;
        }

        /// <summary>
        /// Provide label for the sorting field.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When label is null or empty.
        /// </exception>
        /// <param name="label">Label to display for sorting field.</param>
        /// <returns>Instance of ISortingField</returns>
        public ISortingField<TModel> WithLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            FieldLabel = label;
            return this;
        }

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
        /// <exception cref="ArgumentNullException">
        /// When explicitPropertyName is null or empty.
        /// </exception>
        /// <param name="explicitPropertyName">Explicit property name.</param>
        /// <returns>Instance of ISortingField.</returns>
        public ISortingField<TModel> WithExplicitPropertyName(
            string explicitPropertyName)
        {
            if (string.IsNullOrEmpty(explicitPropertyName))
                throw new ArgumentNullException(nameof(explicitPropertyName));

            ExplicitPropertyName = explicitPropertyName;
            return this;
        }
    }
}
