using System;
using System.Collections.Generic;
using Microsoft.Playwright;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    /// <summary>
    /// Describes search context for web controls. Contains variety of methods to search and wait for controls of specified type from current context.
    /// </summary>
    public abstract class WebSearchContext : BaseSearchContext<WebSearchContext>
    {
        // !!! TODO: UGLY!
        private static TimeSpan currentTimeout = TimeoutDefault;

        /// <summary>
        /// Gets or sets Current search context as <see cref="object"/>
        /// </summary>
        protected virtual object SearchContext { get; set; }

        /// <summary>
        /// Gets or sets base <see cref="Type"/> for all web controls (by default is <see cref="WebControl"/>)
        /// </summary>
        protected override Type ControlsBaseType => typeof(WebControl);

        /// <summary>
        /// Gets or sets current implicit wait timeout as a <see cref="TimeSpan"/> for the underlying selenium driver.
        /// </summary>
        protected override TimeSpan ImplicitWaitTimeout
        {
            get => currentTimeout;
            set
            {
                TimeSpan timeout = value.Equals(TimeSpan.Zero) ? TimeSpan.FromMilliseconds(50) : value;
                IPage ipage = SearchContext is IPage page ? page : ((ILocator)SearchContext).Page;
                ipage.SetDefaultTimeout((float)timeout.TotalMilliseconds);
                currentTimeout = timeout;
            }
        }

        #region "Helpers"

        /// <summary>
        /// Wait for typified control by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WebControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped control instance</returns>
        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            ILocator elementToWrap = GetNativeControl(locator);
            return Wrap<T>(elementToWrap, locator);
        }

        /// <summary>
        /// Wait for typified controls list by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WebControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped controls list</returns>
        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList(locator);
            List<T> controlsList = new List<T>();

            foreach (var elementToWrap in elementsToWrap)
            {
                controlsList.Add(Wrap<T>(elementToWrap, null));
            }

            return controlsList;
        }

        /// <summary>
        /// Get first child from current context which has specified control type ignoring implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WebControl"/></typeparam>
        /// <returns>wrapped control instance</returns>
        protected override T GetFirstChildWrappedControl<T>()
        {
            var elementToWrap = GetNativeControlsList(new ByLocator(Using.WebXpath, "./*"))[0];
            return Wrap<T>(elementToWrap, null);
        }

        /// <summary>
        /// Get control instance from current context as <see cref="ILocator"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="ILocator"/> instance</returns>
        protected ILocator GetNativeControl(ByLocator locator) =>
            GetNativeControlFromContext(locator, SearchContext);

        /// <summary>
        /// Get control instance from parent context as <see cref="ILocator"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="ILocator"/> instance</returns>
        protected ILocator GetNativeControlFromParentContext(ByLocator locator) =>
            GetNativeControlFromContext(locator, ParentSearchContext.SearchContext);

        private ILocator GetNativeControlFromContext(ByLocator locator, object context)
        {
            string by = GetNativeLocator(locator);
            ILocator nativeControl = context is IPage ?
                ((IPage)context).Locator(by) :
                ((ILocator)context).Locator(by);

            try
            {
                LocatorWaitForOptions waitOptions = new LocatorWaitForOptions();
                waitOptions.State = WaitForSelectorState.Attached;

                nativeControl.First.WaitForAsync(waitOptions).GetAwaiter().GetResult();
                return nativeControl;
            }
            catch
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }

        private IReadOnlyList<ILocator> GetNativeControlsList(ByLocator locator)
        {
            string by = GetNativeLocator(locator);

            ILocator nativeControls = SearchContext is IPage ?
                ((IPage)SearchContext).Locator(by) :
                ((ILocator)SearchContext).Locator(by);

            return nativeControls.AllAsync().GetAwaiter().GetResult();
        }

        private string GetNativeLocator(ByLocator locator)
        {
            switch (locator.How)
            {
                case Using.WebXpath:
                    return "xpath=" + locator.Locator;
                case Using.WebCss:
                    return locator.Locator;
                case Using.Id:
                    return "#" + locator.Locator;
                case Using.Name:
                    return "[name='" + locator.Locator + "']";
                case Using.Class:
                    return "." + locator.Locator;
                case Using.WebTag:
                    return locator.Locator;
                default:
                    throw new ArgumentException($"Incorrect locator type specified:  {locator.How}");
            }
        }

        private T Wrap<T>(ILocator elementToWrap, ByLocator locator)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((WebControl)(object)wrapper).Instance = elementToWrap;
            ((WebControl)(object)wrapper).ParentSearchContext = this;
            ((WebControl)(object)wrapper).Locator = locator;
            return wrapper;
        }

        #endregion
    }
}
