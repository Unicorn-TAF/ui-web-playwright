using System;
using Microsoft.Playwright;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    /// <summary>
    /// Represents Driver for Web UI and allows to perform search of elements in web pages.
    /// </summary>
    public abstract class WebDriver : WebSearchContext, IDriver
    {
        /// <summary>
        /// Gets or sets current playwright <see cref="IPage"/> instance.
        /// </summary>
        public IPage CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets underlying <see cref="IBrowserContext"/> instance.
        /// </summary>
        public IBrowserContext Playwright { get; set; }

        /// <summary>
        /// Gets or sets browser type.
        /// </summary>
        public BrowserType Browser { get; protected set; }

        /// <summary>
        /// Gets current URL.
        /// </summary>
        public string Url => CurrentPage.Url;

        /// <summary>
        /// Gets or sets implicit timeout of waiting for specified element to be existed in elements tree.
        /// </summary>
        public TimeSpan ImplicitlyWait
        {
            get => TimeoutDefault;

            set => Playwright.SetDefaultTimeout((float)value.TotalMilliseconds);
        }

        /// <summary>
        /// Close web driver instance and associated browser.
        /// </summary>
        public void Close()
        {
            ULog.Debug("Close driver");
            Playwright.CloseAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Navigates to specified URL.
        /// </summary>
        /// <param name="url">url to navigate</param>
        public void Get(string url)
        {
            ULog.Debug("Navigate to {0}", url);
            CurrentPage.GotoAsync(url).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Executes JavaScript.
        /// </summary>
        /// <param name="script">script string</param>
        /// <param name="parameters">array of script parameters if any</param>
        /// <returns>result of script execution as <see cref="object"/></returns>
        public object ExecuteJS(string script, params object[] parameters)
        {
            ULog.Debug("Executing JS: \n{0}", script);
            return CurrentPage.EvaluateAsync(script, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Scroll view to specified control position.
        /// </summary>
        /// <param name="control">control instance</param>
        public void ScrollTo(WebControl control)
        {
            ULog.Debug("Scroll to {0}", control);

            CurrentPage.EvaluateAsync(
                "arguments[0].scrollIntoView(true); window.scrollTo(0, arguments[0].getBoundingClientRect().top + window.pageYOffset - (window.innerHeight / 2));",
                control.Instance).GetAwaiter().GetResult();
        }
    }
}
