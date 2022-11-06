public class Pttavm : BaseScraper
{
    public Pttavm(string query)
    {
        this.url = "https://www.pttavm.com/arama?q=" + query;
        this.cardSelector = ".md\\:w-1\\/3";
        this.nameSelector = ".product-list-box-container-info-name";
        this.priceSelector = ".md\\:h-auto";
    }
}