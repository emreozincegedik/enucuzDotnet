public class ScrapeQuery
{
    public string query { get; set; }
    public List<string> website { get; set; } = new List<string>();

    public Double priceLow { get; set; } = 0;
    public Double priceHigh { get; set; } = 0;
    public int page { get; set; } = 0;
    public int elementInPage { get; set; } = 15;
}