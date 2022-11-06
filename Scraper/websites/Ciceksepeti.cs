public class Ciceksepeti : BaseScraper
{
    public Ciceksepeti(string query)
    {
        this.url = "https://www.ciceksepeti.com/arama?query=" + query;
        this.cardSelector = ".products__item-inner";
        this.nameSelector = ".products__item-title";
        this.priceSelector = ".price--now .price__integer-value";
    }
}