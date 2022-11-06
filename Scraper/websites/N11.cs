public class N11 : BaseScraper
{
    public N11(string query)
    {
        this.url = "https://www.n11.com/arama?q=" + query;
        this.cardSelector = "li .columnContent";
        this.nameSelector = ".productName";
        this.priceSelector = "ins";
    }
}