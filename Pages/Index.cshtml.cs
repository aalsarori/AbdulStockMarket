using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

public class IndexModel : PageModel
{
    // Ticker Symbol
    private string m_Ticker;

    // Remaining Cash
    private double m_Cash;

    // Stock Shares
    private double m_Stocks;

    // Date
    private string m_Date;

    public IndexModel()
    {
        
    }

    public IActionResult OnPostGetCash(string name)
    {
        {
            // Run query
            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT AmtOfCash FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Cash = Double.Parse(decimalHolder.ToString());

            connection.Close();

            // Return
            string returning = "Cash: $" + m_Cash.ToString();
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetStocks(string name)
    {
        {
            // Run query
            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT AmtofShares FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Stocks = Double.Parse(decimalHolder.ToString());
            connection.Close();

            // Return
            string returning = "Shares: " + m_Stocks.ToString();
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetTicker(string name)
    {
        {
            // Run query
            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT TickerName FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            m_Ticker = (string)db.ExecuteScalar();

            connection.Close();

            string returning = "Ticker: " + m_Ticker;
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostSetTicker(string name)
    {
        {
            string returned = StartProject(name);

            if (name != null && name != "")
            {
                m_Ticker = name;
            }

            string returning = "Ticker: " + m_Ticker;
            return new JsonResult(returning);
        }
    }

    public string StartProject(string ticker)
    {
        // Clear the database
        string clear = "DELETE FROM Holding";

        // Run query
        string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand db = new SqlCommand(clear, connection);
        db.ExecuteNonQuery();

        string randomDate = OnPostRandomDate;

        // Set the base values with the ticker
        string insert = string.Format("INSERT INTO Holding (TickerName, AmtOfCash, AmtofShares, StockDate) VALUES ('{0}', 10000, 0, '{1}')", ticker, randomDate);
        db = new SqlCommand(insert, connection);
        db.ExecuteNonQuery();

        // Close the connection
        connection.Close();

        return "";
    }

    public IActionResult OnPostGetDate(string name)
    {
        {
            // Run query
            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT StockDate FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            m_Date = ((DateTime)db.ExecuteScalar()).ToString("yyyy-MM-dd");
            connection.Close();

            // Return
            string returning = "Date: " + m_Date;
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetInvestment(string name)
    {
        decimal tickerPrice = 0;

        // Run query
        string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sql = string.Format("SELECT TickerName FROM Holding");
        SqlCommand db = new SqlCommand(sql, connection);
        m_Ticker = (string)db.ExecuteScalar();

        sql = string.Format("SELECT StockDate FROM Holding");
        db = new SqlCommand(sql, connection);
        DateTime date = (DateTime)db.ExecuteScalar();
        m_Date = date.ToString("yyyy-MM-dd");
        DateTime tempDate = DateTime.Parse(m_Date);
        
        sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
        db = new SqlCommand(sql, connection);

        // Assign value
        tickerPrice = (decimal)db.ExecuteScalar();

        // Close the connection
        connection.Close();

        double investment = m_Stocks * double.Parse(tickerPrice.ToString());

        string returning = "Investment Amount: $" + Math.Round(investment, 2);
        return new JsonResult(returning);
    }

    // Create a function that queries the database for the current price at the current day
    public IActionResult OnPostCurrentTickerPrice
    {
        get
        {
            double tickerPrice = 0;

            // Run query
            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            DateTime tempDate = DateTime.Parse(m_Date);
            string sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
            SqlCommand db = new SqlCommand(sql, connection);

            // Assign value
            tickerPrice = (double)db.ExecuteScalar();

            // Close the connection
            connection.Close();

            return new JsonResult(tickerPrice);
        }
    }

    // Create function that sells a certain amount of stock and adds that money for the price of that day to cash but takes away the shares
    public IActionResult OnPostSellStocks(string amountSold)
    {
        double tickerPrice = 0;

        // Run query
        string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sql = string.Format("SELECT TickerName FROM Holding");
        SqlCommand db = new SqlCommand(sql, connection);
        m_Ticker = (string)db.ExecuteScalar();

        sql = string.Format("SELECT StockDate FROM Holding");
        db = new SqlCommand(sql, connection);
        m_Date = (string)db.ExecuteScalar();
        DateTime tempDate = DateTime.Parse(m_Date);

        sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
        db = new SqlCommand(sql, connection);

        // Assign value
        tickerPrice = (double)db.ExecuteScalar();

        // Close the connection
        connection.Close();


        // Divide the amount sold by the ticker price, and subtract that from the total shares
        double totalSold = 0;

        if (amountSold != null)
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
        return new JsonResult(tickerPrice);
    }

    // Create function that buys a certain amount of stock based on an amount of cash and adds it to shares but takes it from cash
    public IActionResult OnPostBuyStocks(string amountBuy)
    {
        decimal tickerPrice = 0;

        // Run query
        string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sql = string.Format("SELECT TickerName FROM Holding");
        SqlCommand db = new SqlCommand(sql, connection);
        m_Ticker = (string)db.ExecuteScalar();

        sql = string.Format("SELECT StockDate FROM Holding");
        db = new SqlCommand(sql, connection);
        DateTime date = (DateTime)db.ExecuteScalar();
        m_Date = date.ToString("yyyy-MM-dd");
        DateTime tempDate = DateTime.Parse(m_Date);

        sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
        db = new SqlCommand(sql, connection);

        // Assign value
        tickerPrice = (decimal)db.ExecuteScalar();

        // Close the connection
        connection.Close();

        // Divide the amount to buy by the ticker price, and add that to the total shares
        double totalShares = 0;

        if (amountBuy != null)
        {
            // Get shares
            connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            connection = new SqlConnection(connectionString);
            connection.Open();

            sql = "SELECT AmtofShares FROM Holding";
            db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Stocks = Double.Parse(decimalHolder.ToString());

            totalShares = double.Parse(tickerPrice.ToString()) / double.Parse(amountBuy);
            m_Stocks += totalShares;

            // Update it back
            sql = String.Format("UPDATE Holding SET AmtofShares = {0}", m_Stocks);
            db = new SqlCommand(sql, connection);
            db.ExecuteNonQuery();

            connection.Close();
        }

        // Subtract that amount sold to the total cash
        if (amountBuy != null)
        {
            // Get shares
            connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            connection = new SqlConnection(connectionString);
            connection.Open();

            sql = "SELECT AmtOfCash FROM Holding";
            db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Cash = Double.Parse(decimalHolder.ToString());

            totalShares = double.Parse(tickerPrice.ToString()) / double.Parse(amountBuy);
            m_Cash -= double.Parse(amountBuy);

            // Update it back
            sql = String.Format("UPDATE Holding SET AmtOfCash = {0}", m_Cash);
            db = new SqlCommand(sql, connection);
            db.ExecuteNonQuery();

            connection.Close();

        }


        // Should I return something else?
        return new JsonResult("Bought");
    }

    // Sell everything and close the game function
    public IActionResult OnPostQuit
    {
        get
        {
            double tickerPrice = 0;

            // Run query
            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = string.Format("SELECT TickerName FROM Holding");
            SqlCommand db = new SqlCommand(sql, connection);
            m_Ticker = (string)db.ExecuteScalar();

            sql = string.Format("SELECT StockDate FROM Holding");
            db = new SqlCommand(sql, connection);
            m_Date = (string)db.ExecuteScalar();
            DateTime tempDate = DateTime.Parse(m_Date);
            
            sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
            db = new SqlCommand(sql, connection);

            // Assign value
            tickerPrice = (double)db.ExecuteScalar();

            // Close the connection
            connection.Close();

            // Divide the amount sold by the ticker price, and subtract that from the total shares
            double totalSold = 0;

            totalSold = tickerPrice * m_Stocks;

            m_Cash += totalSold;

            // Should I return anything?
            return new JsonResult(tickerPrice);
        }
    }

    // Choose a random date in the last 6 months function
    // TAN
    public string OnPostRandomDate
    {
        get
        {
            string randomDate = "2022-05-02";

            // Find a random date in the last 6 months

            // Return only the day month and year (2022-12-01)

            //DateTime date = DateTime.Now.ToString("yyyy-MM-dd");

            return randomDate;
        }
    }

    // Move the date forward by 1 week or 1 month or so function
    // TAN
    public IActionResult OnPostMoveForward(string date)
    {
        string randomDate = "";

        //string randomDate = DateTime.Parse(date);

        // Find a random date in the last 6 months
        // DateTime.Parse() DateTime.Now DateTime.AddWeeks
        // Move forward a week

        return new JsonResult(randomDate);
    }

    public void OnGet()
    {

    }
}