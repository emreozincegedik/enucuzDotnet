public class Pazarama : BaseScraper
{
    public Pazarama(string query)
    {
        this.url = "https://www.pazarama.com/arama?q=" + query;
        this.cardSelector = ".\\!w-1\\/4";
        this.nameSelector = ".mb-1\\.5";
        this.priceSelector = ".text-huge";
    }
}