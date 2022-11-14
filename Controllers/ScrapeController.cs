using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
namespace enucuz_dotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class ScrapeController : ControllerBase
{
    private readonly ILogger<ScrapeController> _logger;

    public ScrapeController(ILogger<ScrapeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetScraper")]
    public IEnumerable<ScrapeModel> Get([FromQuery] ScrapeQuery queryObj)
    {
        queryObj.website = queryObj.website.Distinct().ToList();
        bool test = true;
        Amazon t = new Amazon(queryObj.query);


        if (queryObj.website.Count == 1 && queryObj.website[0] == null)
        {
            queryObj.website = new List<string>();
        }
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        // Thread.Sleep(10000);

        var result = new List<ScrapeModel>();
        DataTable res = SQLQueries.select(queryObj);
        if (res != null && res.Rows.Count > 0)
        {
            result = res.AsEnumerable().Select(row => new ScrapeModel
            {
                website = row.Field<string>("website"),
                url = row.Field<string>("url"),
                name = row.Field<string>("name"),
                image = row.Field<string>("image"),
                price = row.Field<Double>("price")

            }).ToList();
        }
        else
        {
            if (test)
            {
                result = t.scrape();
            }
            else
            {
                result = new ScraperMain(queryObj).scrape();
            }
        }

        queryObj.priceHigh = queryObj.priceHigh == 0 ? Double.MaxValue : queryObj.priceHigh;

        result = result.OrderBy(x => x.price).Where(x => (x.price >= queryObj.priceLow && x.price <= queryObj.priceHigh)).ToList();

        if (queryObj.page != 0 && queryObj.elementInPage >= 10)
        {
            result = result.Skip((queryObj.page - 1) * queryObj.elementInPage).Take(queryObj.elementInPage).ToList();
        }
        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("RunTime " + elapsedTime);
        return result;
    }
}
