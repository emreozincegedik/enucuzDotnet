public class ScraperMain
{
    string query { get; set; }
    List<ScrapeModel> itemList { get; set; }
    List<string> websiteList { get; set; }
    public static List<string> fileList = new List<string> { "amazon", "ciceksepeti", "hepsiburada", "migros", "n11", "pazarama", "pttavm", "trendyol" };
    public ScraperMain(ScrapeQuery queries)
    {

        this.query = queries.query;
        this.itemList = new List<ScrapeModel>();
        this.websiteList = queries.website ?? fileList;
        if (this.websiteList.Count == 0)
        {
            this.websiteList = fileList;
        }
        {
            this.websiteList = fileList;
        }
    }
    public List<ScrapeModel> scrape()
    {
        Parallel.For(0, fileList.Count, i =>
        {
            string website = fileList[i];
            Type elementType = Type.GetType(FirstCharToUpper(website));

            BaseScraper scraper = (BaseScraper)Activator.CreateInstance(elementType, new object[] { this.query });

            List<ScrapeModel> scrapeRes = scraper.scrape();
            Task.Run(() => SQLQueries.insert(scrapeRes, this.query));
            if (this.websiteList.Contains(website))
            {
                itemList.AddRange(scrapeRes);
            }

        });

        return itemList;
    }

    private string FirstCharToUpper(string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
}