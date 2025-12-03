using Microsoft.Playwright;

namespace Unicorn.UI.Web.Driver
{
    /// <summary>
    /// Represents Driver for Desktop version of browser and allows to perform search of elements in web pages.
    /// </summary>
    public class DesktopWebDriver : WebDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class with specified browser type, driver options and window maximize state.
        /// </summary>
        public DesktopWebDriver(BrowserType browser, BrowserTypeLaunchOptions driverOptions, bool maximize)
        {
            Browser = browser;

            Playwright = driverOptions == null ?
                WebDriverFactory.Get(browser) :
                WebDriverFactory.Get(browser, driverOptions);

            CurrentPage = Playwright.NewPageAsync().GetAwaiter().GetResult();

            SearchContext = CurrentPage;

            ImplicitlyWait = TimeoutDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class with specified browser type and window maximize state and without driver options.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="maximize"></param>
        public DesktopWebDriver(BrowserType browser, bool maximize) : this(browser, null, maximize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class with specified browser type and maximized window.
        /// </summary>
        /// <param name="browser"></param>
        public DesktopWebDriver(BrowserType browser) : this(browser, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class based on existing instance of <see cref="IBrowserContext"/>.
        /// </summary>
        /// <param name="webDriverInstance">IWebDriver instance</param>
        public DesktopWebDriver(IBrowserContext webDriverInstance)
        {
            Playwright = webDriverInstance;
            SearchContext = Playwright;
            ImplicitlyWait = TimeoutDefault;
            Browser = WebDriverFactory.GetBrowserType(webDriverInstance);
            CurrentPage = Playwright.NewPageAsync().GetAwaiter().GetResult();
        }
    }
}
