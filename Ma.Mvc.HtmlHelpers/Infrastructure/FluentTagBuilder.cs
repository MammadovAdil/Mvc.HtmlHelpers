using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Infrastructure
{
    /// <summary>
    /// Fluent wrapper for tag builder.
    /// </summary>
    public class FluentTagBuilder
    {
        // Current TagBuilder
        private TagBuilder Builder { get; set; }
        private StringBuilder ChildElementBuilder { get; set; }

        // Flag to identify if tag builder has already been rendered.
        private bool alredyRendered = false;

        /// <summary>
        /// Initialize new FluentTagBuilder.
        /// </summary>
        /// <param name="tagName">Name of HTML tag.</param>
        public FluentTagBuilder(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
                throw new ArgumentNullException(nameof(tagName));

            Builder = new TagBuilder(tagName);
            ChildElementBuilder = new StringBuilder();
        }        

        /// <summary>
        /// InnerHtml of element.
        /// </summary>
        public string InnerHtml
        {
            get { return Builder.InnerHtml; }
            set
            {
                Builder.InnerHtml = value;
            }
        }

        /// <summary>
        /// Add css class to element.
        /// </summary>
        /// <param name="cssClass">Name of css class.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder AddCssClass(string cssClass)
        {
            if (string.IsNullOrEmpty(cssClass))
                throw new ArgumentNullException(nameof(cssClass));

            Builder.AddCssClass(cssClass);
            return this;
        }

        /// <summary>
        /// Append child element to current element.
        /// </summary>
        /// <param name="element">Child element to append.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder AppendChild(FluentTagBuilder element)
        {
            if (element != null)
                ChildElementBuilder.AppendLine(element.Render());

            return this;
        }

        /// <summary>
        /// Append child element to current element.
        /// </summary>
        /// <param name="element">Child element to append.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder AppendChild(MvcHtmlString element)
        {
            if(element != null)
                ChildElementBuilder.AppendLine(element.ToHtmlString());

            return this;
        }

        /// <summary>
        /// Append child element to current element.
        /// </summary>
        /// <param name="element">Child element to append.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder AppendChild(string element)
        {
            if (!string.IsNullOrEmpty(element))
                ChildElementBuilder.AppendLine(element);

            return this;
        }

        /// <summary>
        /// Set innert HTML of element.
        /// </summary>
        /// <param name="innerHtml">HTML to set as inner HTML of element.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder SetInnerHtml(string innerHtml)
        {
            if (innerHtml == null)
                throw new ArgumentNullException(nameof(innerHtml));

            Builder.InnerHtml = innerHtml;
            return this;
        }

        /// <summary>
        /// Set inner text of current element.
        /// </summary>
        /// <param name="innerText">Text to set as inner text.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder SetInnerText(string innerText)
        {
            if (string.IsNullOrEmpty(innerText))
                throw new ArgumentNullException(nameof(innerText));

            Builder.SetInnerText(innerText);
            return this;
        }


        /// <summary>
        /// Adds a new attribute to the tag.
        /// </summary>
        /// <param name="key">The key for the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder MergeAttribute(string key, string value)
        {
            return MergeAttribute(key, value, false);
        }
        
        /// <summary>
        /// Adds a new attribute or optionally replaces an
        /// existing attribute in the opening tag.
        /// </summary>
        /// <param name="key">The key for the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <param name="replaceExisting">Replace existing attribute with current.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder MergeAttribute(string key, string value, bool replaceExisting)
        {
            Builder.MergeAttribute(key, value, replaceExisting);
            return this;
        }

        /// <summary>
        /// Adds new attributes to the tag.
        /// </summary>
        /// <typeparam name="TKey">he type of the key object.</typeparam>
        /// <typeparam name="TValue">The type of the value object.</typeparam>
        /// <param name="attributes">The collection of attributes to add.</param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
        {
            return MergeAttributes(attributes, false);
        }

        /// <summary>
        ///  Adds new attributes or optionally replaces existing attributes in the tag.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="attributes">The collection of attributes to add or replace.</param>
        /// <param name="replaceExisting">
        /// For each attribute in attributes, true to replace the attribute if an attribute
        /// already exists that has the same key, or false to leave the original attribute
        /// unchanged.
        /// </param>
        /// <returns>Current builder.</returns>
        public FluentTagBuilder MergeAttributes<TKey, TValue>(
            IDictionary<TKey, TValue> attributes, 
            bool replaceExisting)
        {
            Builder.MergeAttributes(attributes, replaceExisting);
            return this;
        }

        /// <summary>
        /// Render string representation of element as HTML element.
        /// </summary>
        /// <returns>Rendered HTML.</returns>
        public string Render()
        {           
            // If has not been already rendered add child elements 
            if (!alredyRendered
                && ChildElementBuilder.Length > 0)
                Builder.InnerHtml += ChildElementBuilder.ToString();

            alredyRendered = true;
            return Builder.ToString();
        }

        /// <summary>
        /// Render string representation of element as HTML element.
        /// </summary>
        /// <returns>Rendered HTML.</returns>
        public override string ToString()
        {
            return Render();
        }
    }
}
