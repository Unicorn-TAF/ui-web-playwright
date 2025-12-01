using System.Drawing;
using Microsoft.Playwright;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Web.Controls
{
    /// <summary>
    /// Represents basic web control. Contains number of main properties and action under the control.
    /// </summary>
    public class WebControl : WebSearchContext, IControl
    {
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebControl"/> class.
        /// </summary>
        public WebControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebControl"/> class with wraps specific <see cref="ILocator"/>
        /// </summary>
        /// <param name="instance"><see cref="ILocator"/> instance to wrap</param>
        public WebControl(ILocator instance)
        {
            Instance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to cache the control.
        /// Cached control is not searched for on each next call. Not cached control is searched each time (as PageObject control).
        /// </summary>
        public bool Cached { get; set; } = true;

        /// <summary>
        /// Gets or sets locator to find control by.
        /// </summary>
        public ByLocator Locator { get; set; }

        /// <summary>
        /// Gets or sets control name.
        /// </summary>
        public string Name 
        { 
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = $"{GetType().Name} [{Locator?.ToString()}]";
                }

                return name;
            }

            set => name = value;
        }

        /// <summary>
        /// Gets or sets control wrapped instance as <see cref="ILocator"/> which is also current search context.
        /// </summary>
        public ILocator Instance
        {
            get
            {
                return (ILocator)SearchContext;
            }

            set
            {
                SearchContext = value;
                ContainerFactory.InitContainer(this);
            }
        }

        /// <summary>
        /// Gets control text.
        /// </summary>
        public string Text => Instance.TextContentAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Gets a value indicating whether control is enabled in UI.
        /// </summary>
        public bool Enabled => Instance.IsEnabledAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Gets a value indicating whether control is visible (not is Off-screen)
        /// </summary>
        public bool Visible => Instance.IsVisibleAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Gets control location as <see cref="Point"/>
        /// </summary>
        public Point Location
        {
            get
            {
                LocatorBoundingBoxResult bBox = Instance.BoundingBoxAsync().GetAwaiter().GetResult();
                return new Point((int)bBox.X, (int)bBox.Y);
            }
        }

        /// <summary>
        /// Gets control bounding rectangle as <see cref="Rectangle"/>
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                LocatorBoundingBoxResult bBox = Instance.BoundingBoxAsync().GetAwaiter().GetResult();
                return new Rectangle((int)bBox.X, (int)bBox.Y, (int)bBox.Width, (int)bBox.Height);
            }
        }

        /// <summary>
        /// Gets or sets control search context. 
        /// If control is not cached current context is searched from parent context by this control locator.
        /// </summary>
        protected override object SearchContext
        {
            get
            {
                if (!Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(Locator);
                }

                return base.SearchContext;
            }

            set
            {
                base.SearchContext = value;
            }
        }

        /// <summary>
        /// Gets control attribute value as <see cref="string"/>
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <returns>control attribute value as string</returns>
        public string GetAttribute(string attribute) =>
            Instance.GetAttributeAsync(attribute).GetAwaiter().GetResult();

        /// <summary>
        /// Perform click on control.
        /// </summary>
        public virtual void Click()
        {
            ULog.Debug("Click {0}", this);
            Instance.ClickAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Perform JavaScript click on control.
        /// </summary>
        public virtual void JsClick()
        {
            ULog.Debug("JavaScript click {0}", this);
            Instance.EvaluateAsync("control => control.click();").GetAwaiter().GetResult();
        }

        /// <summary>
        /// Perform right click on control.
        /// </summary>
        public virtual void RightClick()
        {
            ULog.Debug("Right click {0}", this);

            LocatorClickOptions options = new LocatorClickOptions();
            options.Button = MouseButton.Right;
            Instance.ClickAsync(options).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets string description of the control.
        /// </summary>
        /// <returns>control description as string</returns>
        public override string ToString() => Name;
    }
}
