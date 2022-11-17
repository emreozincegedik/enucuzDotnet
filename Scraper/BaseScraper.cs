using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


public class BaseScraper
{

    IWebDriver driver;

    List<ScrapeModel> itemList;
    ChromeOptions options;
    protected string url { get; set; }
    protected string cardSelector { get; set; } = string.Empty;
    protected string nameSelector { get; set; } = string.Empty;
    protected string priceSelector { get; set; } = string.Empty;


    public BaseScraper()
    {


        options = new ChromeOptions();
        options.AddArgument("--disable-background-timer-throttling");
        options.AddArgument("--disable-backgrounding-occluded-windows");
        options.AddArgument("--disable-breakpad");
        options.AddArgument("--disable-component-extensions-with-background-pages");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-features=TranslateUI,BlinkGenPropertyTrees");
        options.AddArgument("--disable-ipc-flooding-protection");
        options.AddArgument("--disable-renderer-backgrounding");
        options.AddArgument("--enable-features=NetworkService,NetworkServiceInProcess");
        options.AddArgument("--force-color-profile=srgb");
        options.AddArgument("--hide-scrollbars");
        options.AddArgument("--metrics-recording-only");
        options.AddArgument("--mute-audio");

        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--start-maximized");
        options.AddArgument("--no-sandbox");

        // options.AddArgument("--headless");

        driver = new ChromeDriver(Directory.GetCurrentDirectory(), options);
        // driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(3);

        itemList = new List<ScrapeModel>();
    }

    public void Dispose()
    {
        if (driver != null)
        {
            driver.Quit();
            driver = null;
        }
    }
    Double priceFix(String price)
    {
        price = price.Replace("TL", "").Trim().Replace(".", "").Replace(",", ".");
        Double t = Double.Parse(price, new CultureInfo("en-EN"));
        return t;
    }
    void scrollToBottom()
    {
        int scroll_page = 0;
        while (scroll_page < 2000)
        {

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "window.scrollTo(0," + scroll_page.ToString() + ");");
            scroll_page += 200;
            System.Threading.Thread.Sleep(25);
        }
    }
    public List<ScrapeModel> scrape()
    {
        List<IWebElement> cardSelectorList;
        while (true)
        {
            try
            {

                driver.Navigate().GoToUrl(url);
                // driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
                System.Threading.Thread.Sleep(1000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(e => e.FindElement(By.CssSelector(this.cardSelector)));
                ((IJavaScriptExecutor)driver).ExecuteScript("window.stop();");
                scrollToBottom();
                cardSelectorList = driver.FindElements(By.CssSelector(this.cardSelector)).ToList<IWebElement>();
                break;
            }
            catch (Exception)
            { }
        }
        Parallel.For(0, cardSelectorList.Count > 20 ? 20 : cardSelectorList.Count, i =>
        {
            ScrapeModel scr = new ScrapeModel();
            scr.website = this.GetType().ToString().ToLower();
            var item = cardSelectorList[i];
            scr.url = item.FindElement(By.CssSelector("a")).GetAttribute("href");
            scr.name = item.FindElement(
                By.CssSelector(nameSelector)).Text;

            try
            {
                scr.price = priceFix(item.FindElement(By.CssSelector(priceSelector)).Text);

            }
            catch (System.Exception)
            {
                scr.price = 0;
            }
            try
            {
                // scr.image = wait.Until(e => e.FindElement(By.CssSelector("img")).GetAttribute("src"));
                scr.image = item.FindElement(By.CssSelector("img")).GetAttribute("src");
                if (scr.image.Contains("gif"))
                {

                    scr.image = item.FindElement(By.CssSelector("img")).GetAttribute("data-src");
                }
            }
            catch (System.Exception e)
            {
                try
                {
                    // scr.image = wait.Until(e => e.FindElement(By.CssSelector("picture")).GetAttribute("srcset"));
                    scr.image = item.FindElement(By.CssSelector("picture")).GetAttribute("srcset");
                }
                catch (System.Exception e2)
                {
                    try
                    {
                        scr.image = item.FindElement(By.CssSelector("source")).GetAttribute("srcset");

                    }
                    catch (System.Exception e3)
                    {
                        scr.image = null;
                        // throw e3;
                    }
                }
            }
            if (scr.price != 0 && itemList.Count <= 20)
            {
                itemList.Add(scr);
            }
        });
        Dispose();
        return itemList;
    }
}