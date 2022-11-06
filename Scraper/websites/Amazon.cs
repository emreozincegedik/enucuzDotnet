public class Amazon : BaseScraper
{
    public Amazon(string query)
    {
        this.url = "https://www.amazon.com.tr/s?k=" + query;
        this.cardSelector = ".s-widget-spacing-small .sg-col-inner";
        this.nameSelector = ".a-color-base.a-text-normal";
        this.priceSelector = ".a-price-whole";
    }
}