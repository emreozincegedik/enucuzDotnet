public class ScrapeQuery
{
    public string query { get; set; }
    public List<string> website { get; set; }

    public Double priceLow { get; set; }
    public Double priceHigh { get; set; }
    public int page { get; set; }
    public int elementInPage { get; set; } = 15;
}