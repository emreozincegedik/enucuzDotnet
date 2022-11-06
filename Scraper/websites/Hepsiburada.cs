public class Hepsiburada : BaseScraper
{
    public Hepsiburada(string query)
    {
        this.url = "https://www.hepsiburada.com/ara?q=" + query;
        this.cardSelector = "li.productListContent-zAP0Y5msy8OHn5z7T_K_";
        this.nameSelector = "h3";
        this.priceSelector = "[data-test-id='price-current-price']";
    }
}