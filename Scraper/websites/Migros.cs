public class Migros : BaseScraper
{
    public Migros(string query)
    {
        this.url = "https://www.migros.com.tr/arama?q=" + query;
        this.cardSelector = ".mdc-card";
        this.nameSelector = ".product-name";
        this.priceSelector = ".amount";
    }
}