using System;
using Microsoft.Playwright;
using PlaywrightInstance = Microsoft.Playwright.Playwright;

namespace Unicorn.UI.Web.Driver
{
    internal static class WebDriverFactory
    {
        internal static IBrowserContext Get(BrowserType browser)
        {
            BrowserTypeLaunchOptions options = new BrowserTypeLaunchOptions();

            switch (browser)
            {
                case BrowserType.Chromium:
                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Chromium.LaunchAsync().GetAwaiter().GetResult().
                        NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.Chrome:
                    options.Channel = "chrome";

                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Chromium.LaunchAsync(options).GetAwaiter().GetResult().
                        NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.Edge:
                    options.Channel = "msedge";

                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Chromium.LaunchAsync(options).GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.Firefox:
                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Firefox.LaunchAsync().GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.WebKit:
                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Webkit.LaunchAsync().GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                default:
                    return null;
            }
        }

        internal static IBrowserContext Get(BrowserType browser, BrowserTypeLaunchOptions options)
        {
            switch (browser)
            {
                case BrowserType.Chromium:
                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Chromium.LaunchAsync(options).GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.Chrome:
                    options.Channel = "chrome";

                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Chromium.LaunchAsync(options).GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.Edge:
                    options.Channel = "msedge";

                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Chromium.LaunchAsync(options).GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.Firefox:
                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Firefox.LaunchAsync(options).GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();

                case BrowserType.WebKit:
                    return PlaywrightInstance.CreateAsync().GetAwaiter().GetResult()
                        .Webkit.LaunchAsync(options).GetAwaiter().GetResult()
                        .NewContextAsync(new BrowserNewContextOptions()).GetAwaiter().GetResult();
                default:
                    return null;
            }
        }

        internal static BrowserType GetBrowserType(IBrowserContext pwDriver)
        {
            switch (pwDriver.Browser.BrowserType.Name)
            {
                case Microsoft.Playwright.BrowserType.Chromium:
                    return BrowserType.Chrome;

                case Microsoft.Playwright.BrowserType.Firefox:
                    return BrowserType.Firefox;

                case Microsoft.Playwright.BrowserType.Webkit:
                    return BrowserType.WebKit;

                default:
                    throw new NotSupportedException("Playwright BrowserType type is not supported: " + pwDriver.GetType());
            }
        }
    }
}
