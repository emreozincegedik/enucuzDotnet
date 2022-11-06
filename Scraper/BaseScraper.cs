using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


public class BaseScraper
{

    IWebDriver driver;
    List<IWebElement> cardSelectorList;
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

        driver = new ChromeDriver("D:\\Torrent\\chromedriver_win32", options);
        // driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(3);



        cardSelectorList = new List<IWebElement>();
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
        Double t = Double.Parse(price);
        return t;
    }
    public List<ScrapeModel> scrape()
    {
        driver.Navigate().GoToUrl(url);
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
        cardSelectorList = wait.Until(e => e.FindElements(By.CssSelector(this.cardSelector))).ToList<IWebElement>();

        Parallel.For(0, cardSelectorList.Count, i =>
        {
            var item = cardSelectorList[i];
            ScrapeModel scr = new ScrapeModel();
            scr.url = item.FindElement(By.CssSelector("a")).GetAttribute("href");
            scr.website = this.GetType().ToString().ToLower();
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
            if (scr.price != 0)
            {
                itemList.Add(scr);
            }
        });
        Dispose();
        return itemList;
    }
}