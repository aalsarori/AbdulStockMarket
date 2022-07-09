using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    // Ticker Symbol
    private string m_Ticker;

    // Remaining Cash
    private double m_Cash;

    // Stock Shares
    private double m_Stocks;

    // Date
    private string m_Date;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;

        // Initialize
        m_Cash = 10000;
        m_Stocks = 0;
        m_Ticker = "";

        // Call the random date in the last 6 months function
        m_Date = RandomDate;
    }

    public IActionResult OnPostGetAjax(string name)
    {
        return new JsonResult("Hello " + name);
    }

    public IActionResult OnPostDoubleMoney(string description, int value)
    {
        if (description == "please")
        {
            return new JsonResult("Money doubled to: " + (value * 2));
        }
        else
        {
            return new JsonResult("Money multipled to: " + (value * 10));
        }

    }

    public string GetCash
    {
        get
        {
            return m_Cash.ToString();
        }
    }

    public string GetStocks
    {
        get
        {
            return m_Stocks.ToString();
        }
    }

    public string GetTicker
    {
        get
        {
            return m_Ticker;
        }
    }

    public string GetDate
    {
        get
        {
            return m_Date;
        }
    }

    // Create a function that queries the database for the current price at the current day
    public string CurrentTickerPrice(string tickerName)
    {
        double tickerPrice = 0;

        string sql = "SELECT TICKER PRICE FROM TABLE WHERE DATE ";

        // Run query

        // Assign value

        return tickerPrice.ToString();
    }

    // Create function that sells a certain amount of stock and adds that money for the price of that day to cash but takes away the shares
    public string SellStocks(string tickerName, string amountSold)
    {
        double tickerPrice = 0;

        string sql = "SELECT TICKER PRICE FROM TABLE WHERE DATE ";

        // Run query

        // Assign value

        // Divide the amount sold by the ticker price, and subtract that from the total shares
        double totalSold = 0;

        if(amountSold != null)
        {
            totalSold = tickerPrice * double.Parse(amountSold);
        }


        // Add that amount sold to the total cash
        m_Cash += totalSold;

        if (amountSold != null)
        {
            m_Stocks -= double.Parse(amountSold);
        }

        // Should I return anything?
        return tickerPrice.ToString();
    }

    // Create function that buys a certain amount of stock based on an amount of cash and adds it to shares but takes it from cash
    public string BuyStocks(string tickerName, string amountBuy)
    {
        double tickerPrice = 0;

        string sql = "SELECT TICKER PRICE FROM TABLE WHERE DATE ";

        // Run query

        // Assign value

        // Divide the amount to buy by the ticker price, and add that to the total shares
        double totalShares = 0;

        if(amountBuy != null)
        {
            totalShares = tickerPrice / double.Parse(amountBuy);
        }

        // Subtract that amount sold to the total cash
        if (amountBuy != null)
        {
            m_Cash -= double.Parse(amountBuy);
        }

        m_Stocks += totalShares;

        // Should I return something else?
        return tickerPrice.ToString();
    }

    // Choose a random date in the last 6 months function
    // TAN
    public string RandomDate
    {
        get
        {
            string randomDate = "";
            
            // Find a random date in the last 6 months

            // Return only the day month and year (2022-12-01)

            //DateTime date = DateTime.Now.ToString("yyyy-MM-dd");
           
            return randomDate;
        }
    }

    // Move the date forward by 1 week or 1 month or so function
    // TAN
    public string MoveForward(string date)
    {
        string randomDate = "";

        //string randomDate = DateTime.Parse(date);
            
        // Find a random date in the last 6 months
        // DateTime.Parse() DateTime.Now DateTime.AddWeeks
        // Move forward a week
           
        return randomDate;
    }
    

    // Sell everything and close the game function

    public void OnGet()
    {

    }
}