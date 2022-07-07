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


    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
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

        // Add that amount sold to the total cash

        // Should I return something else?

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

        // Subtract that amount sold to the total cash

        // Should I return something else?

        return tickerPrice.ToString();
    }

    public void OnGet()
    {

    }
}