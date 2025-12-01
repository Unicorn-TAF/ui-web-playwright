using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    /// <summary>
    /// Default implementation for dropdown described by next structure:<br/>
    /// 
    /// <code>
    /// &lt;select&gt;
    ///   &lt;option/&gt; 
    ///   ...
    ///   &lt;option/&gt; 
    /// &lt;/select&gt;
    /// </code>
    /// Has definitions of of basic methods and properties.
    /// </summary>
    public class Dropdown : WebControl, IDropdown
    {
        /// <summary>
        /// Gets currently selected value.
        /// </summary>
        public string SelectedValue =>
            SelectedOption?.TextContentAsync().GetAwaiter().GetResult()
                ?? throw new ControlNotFoundException("No option is selected");

        private ILocator SelectedOption =>
            Options.AllAsync().GetAwaiter().GetResult().FirstOrDefault(o => IsOptionSelected(o));

        bool IExpandable.Expanded => false;

        bool IExpandable.Collapse() => false;

        bool IExpandable.Expand() => false;

        private ILocator Options => Instance.Locator("option");

        /// <summary>
        /// Selects dropdown option by displayed text.
        /// </summary>
        /// <param name="itemName">item text</param>
        /// <returns>true - if selection was made, false - if the item is already selected</returns>
        /// <exception cref="ControlNotFoundException">if option with specified text was not found</exception>
        public bool Select(string itemName)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException(nameof(itemName), "Item name must not be null");
            }

            ULog.Debug("Select '{0}' item from {1}", itemName, this);

            if (itemName.Equals(SelectedOption?.TextContentAsync().GetAwaiter().GetResult()))
            {
                ULog.Trace("No need to select (the item is selected by default)");
                return false;
            }
            else
            {
                Instance.SelectOptionAsync(new[] { new SelectOptionValue() { Label = itemName } }).GetAwaiter().GetResult();
                ULog.Trace("Item was selected");
                return true;
            }
        }

        /// <summary>
        /// Selects dropdown option by value.
        /// </summary>
        /// <param name="itemValue">item value</param>
        /// <returns>true - if selection was made, false - if the item is already selected</returns>
        /// <exception cref="ControlNotFoundException">if option with specified value was not found</exception>
        public bool SelectByValue(string itemValue)
        {
            if (itemValue == null)
            {
                throw new ArgumentNullException(nameof(itemValue), "Item value must not be null");
            }

            ULog.Debug("Select '{0}' value from {1}", itemValue, this);

            if (itemValue.Equals(SelectedOption?.GetAttributeAsync("value").GetAwaiter().GetResult()))
            {
                ULog.Trace("No need to select (the item is selected by default)");
                return false;
            }
            else
            {
                Instance.SelectOptionAsync(new[] { new SelectOptionValue() { Value = itemValue } }).GetAwaiter().GetResult();
                ULog.Trace("Item has been selected");
                return true;
            }
        }

        /// <summary>
        /// Gets all dropdown options.
        /// </summary>
        /// <returns>string array with options</returns>
        public List<string> GetOptions() =>
            Options.AllTextContentsAsync().GetAwaiter().GetResult().ToList();

        private static bool IsOptionSelected(ILocator locator) =>
            string.Equals(
                locator.EvaluateAsync("o => o.selected").GetAwaiter().GetResult().ToString(), 
                "true",
                StringComparison.InvariantCultureIgnoreCase);
    }
}
