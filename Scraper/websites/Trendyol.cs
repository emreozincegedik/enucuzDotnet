public class Trendyol : BaseScraper
{
    public Trendyol(string query)
    {
        this.url = "https://www.trendyol.com/sr?q=" + query;
        this.cardSelector = ".card-border";
        this.nameSelector = ".prdct-desc-cntnr-name";
        this.priceSelector = ".prc-box-dscntd";
    }
}