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

    public SqlConnection connection;

    public IndexModel()
    {
        string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
        connection = new SqlConnection(connectionString);

        connection.Open();
    }

    public IActionResult OnPostGetCash(string name)
    {
        {
            // Run query
            

            string sql = "SELECT AmtOfCash FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Cash = Double.Parse(decimalHolder.ToString());


            m_Cash = Math.Round(m_Cash, 2);

            // Return
            string returning = "Cash: $" + m_Cash.ToString();
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetStocks(string name)
    {
        {
            // Run query
            

            string sql = "SELECT AmtofShares FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Stocks = Double.Parse(decimalHolder.ToString());
            

            // Return
            string returning = "Shares: " + m_Stocks.ToString();
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetTicker(string name)
    {
        {
            // Run query
            

            string sql = "SELECT TickerName FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            m_Ticker = (string)db.ExecuteScalar();

            

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
        
        SqlCommand db = new SqlCommand(clear, connection);
        db.ExecuteNonQuery();

        string randomDate = OnPostRandomDate;

        // Set the base values with the ticker
        string insert = string.Format("INSERT INTO Holding (TickerName, AmtOfCash, AmtofShares, StockDate, StartDate) VALUES ('{0}', 10000, 0, '{1}', '{1}')", ticker, randomDate);
        db = new SqlCommand(insert, connection);
        db.ExecuteNonQuery();

        // Close the connection
        

        return "";
    }

    public IActionResult OnPostGetDate(string name)
    {
        {
            // Run query
            

            string sql = "SELECT StockDate FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            DateTime date = (DateTime)db.ExecuteScalar();

            m_Date = date.ToString("yyyy-MM-dd");

            // Return
            string returning = "Date: " + m_Date;
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetStartDate(string name)
    {
        {
            // Run query


            string sql = "SELECT StartDate FROM Holding";
            SqlCommand db = new SqlCommand(sql, connection);
            DateTime date = (DateTime)db.ExecuteScalar();

            m_Date = date.ToString("yyyy-MM-dd");

            // Return
            string returning = "Start Date: " + m_Date;
            return new JsonResult(returning);
        }
    }

    public IActionResult OnPostGetInvestment(string name)
    {
        decimal tickerPrice = 0;

        // Run query

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

        sql = "SELECT AmtofShares FROM Holding";
        db = new SqlCommand(sql, connection);
        decimal decimalHolder = 0;
        decimalHolder = (decimal)db.ExecuteScalar();
        m_Stocks = Double.Parse(decimalHolder.ToString());

        double investment = m_Stocks * double.Parse(tickerPrice.ToString());

        string returning = "Investment Amount: $" + Math.Round(investment, 2);
        return new JsonResult(returning);
    }

    // Create a function that queries the database for the current price at the current day
    public IActionResult OnPostGetCurrentTickerPrice(string name)
    {
        {
            decimal tickerPrice = 0;

            // Run query
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
            string returning = "Ticker Price: $" + Math.Round(tickerPrice, 2);
            return new JsonResult(returning);
        }
    }

    // Create function that sells a certain amount of stock and adds that money for the price of that day to cash but takes away the shares
    public IActionResult OnPostSellStocks(string amountSold)
    {
        decimal tickerPrice = 0;

        string sql = "SELECT AmtOfCash FROM Holding";
        SqlCommand db = new SqlCommand(sql, connection);
        decimal decimalHolder = 0;
        decimalHolder = (decimal)db.ExecuteScalar();
        m_Cash = Double.Parse(decimalHolder.ToString());

        sql = "SELECT AmtofShares FROM Holding";
        db = new SqlCommand(sql, connection);
        decimalHolder = 0;
        decimalHolder = (decimal)db.ExecuteScalar();

        m_Stocks = Double.Parse(decimalHolder.ToString());

        sql = string.Format("SELECT TickerName FROM Holding");
        db = new SqlCommand(sql, connection);
        m_Ticker = (string)db.ExecuteScalar();

        sql = string.Format("SELECT StockDate FROM Holding");
        db = new SqlCommand(sql, connection);
        DateTime tempDate = (DateTime)db.ExecuteScalar();
        m_Date = tempDate.ToString("yyyy-MM-dd");

        sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
        db = new SqlCommand(sql, connection);

        // Assign value
        tickerPrice = (decimal)db.ExecuteScalar();

        // Divide the amount sold by the ticker price, and subtract that from the total shares
        double totalSold = 0;

        if (amountSold != null)
        {
            totalSold = double.Parse(tickerPrice.ToString()) * double.Parse(amountSold);
        }

        // Add that amount sold to the total cash
        m_Cash += totalSold;

        if (amountSold != null)
        {
            m_Stocks -= double.Parse(amountSold);
        }

        // Update it back
        sql = String.Format("UPDATE Holding SET AmtofShares = {0}", m_Stocks);
        db = new SqlCommand(sql, connection);
        db.ExecuteNonQuery();

        sql = String.Format("UPDATE Holding SET AmtOfCash = {0}", m_Cash);
        db = new SqlCommand(sql, connection);
        db.ExecuteNonQuery();

        string randomDate = OnPostMoveForward;

        // IF == COMPLETE, QUIT INSTEAD
        if(randomDate == "COMPLETE")
        {
            return new JsonResult("COMPLETE");
        }

        sql = String.Format("UPDATE Holding SET StockDate = '{0}'", randomDate);
        db = new SqlCommand(sql, connection);
        db.ExecuteNonQuery();

        return new JsonResult(tickerPrice);
    }

    // Create function that buys a certain amount of stock based on an amount of cash and adds it to shares but takes it from cash
    public IActionResult OnPostBuyStocks(string amountBuy)
    {
        decimal tickerPrice = 0;

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

        // Divide the amount to buy by the ticker price, and add that to the total shares
        double totalShares = 0;

        if (amountBuy != null)
        {
            // Get shares
            sql = "SELECT AmtofShares FROM Holding";
            db = new SqlCommand(sql, connection);
            decimal decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Stocks = Double.Parse(decimalHolder.ToString());

            totalShares = double.Parse(amountBuy) / double.Parse(tickerPrice.ToString());
            m_Stocks += totalShares;

            // Update it back
            sql = String.Format("UPDATE Holding SET AmtofShares = {0}", m_Stocks);
            db = new SqlCommand(sql, connection);
            db.ExecuteNonQuery();

            sql = "SELECT AmtOfCash FROM Holding";
            db = new SqlCommand(sql, connection);
            decimalHolder = 0;
            decimalHolder = (decimal)db.ExecuteScalar();

            m_Cash = Double.Parse(decimalHolder.ToString());

            totalShares = double.Parse(amountBuy) / double.Parse(tickerPrice.ToString());
            m_Cash -= double.Parse(amountBuy);

            // Update it back
            sql = String.Format("UPDATE Holding SET AmtOfCash = {0}", m_Cash);
            db = new SqlCommand(sql, connection);
            db.ExecuteNonQuery();

            // Update it back
            // Make new random date
            string randomDate = OnPostMoveForward;

            // IF == COMPLETE, QUIT INSTEAD
            if (randomDate == "COMPLETE")
            {
                return new JsonResult("COMPLETE");
            }

            sql = String.Format("UPDATE Holding SET StockDate = '{0}'", randomDate);
            db = new SqlCommand(sql, connection);
            db.ExecuteNonQuery();
        }

        // Should I return something else?
        return new JsonResult("Bought");
    }

    // Create function that buys a certain amount of stock based on an amount of cash and adds it to shares but takes it from cash
    public IActionResult OnPostHoldStocks(string amountBuy)
    {
        // Move the date
        string randomDate = OnPostMoveForward;

        // IF == COMPLETE, QUIT INSTEAD
        if (randomDate == "COMPLETE")
        {
            return new JsonResult("COMPLETE");
        }

        string sql = String.Format("UPDATE Holding SET StockDate = '{0}'", randomDate);
        SqlCommand db = new SqlCommand(sql, connection);
        db.ExecuteNonQuery();

        // Should I return something else?
        return new JsonResult("Hold");
    }

    public IActionResult OnPostQuitGame(string amountBuy)
    {
        // Get the date
        string sql = "SELECT StockDate FROM Holding";
        SqlCommand db = new SqlCommand(sql, connection);
        DateTime date = (DateTime)db.ExecuteScalar();

        m_Date = date.ToString("yyyy-MM-dd");

        // Get the amount of shares
        sql = "SELECT AmtofShares FROM Holding";
        db = new SqlCommand(sql, connection);
        decimal decimalHolder = 0;
        decimalHolder = (decimal)db.ExecuteScalar();

        m_Stocks = Double.Parse(decimalHolder.ToString());

        // Get the ticker price
        // Run query
        sql = string.Format("SELECT TickerName FROM Holding");
        db = new SqlCommand(sql, connection);
        m_Ticker = (string)db.ExecuteScalar();

        DateTime tempDate = DateTime.Parse(m_Date);

        sql = string.Format("SELECT ClosePrice FROM Stocks WHERE Ticker = '{0}' AND DayPart = '{1}' AND MonthPart = '{2}' AND YearPart = '{3}' ", m_Ticker, tempDate.ToString("dd"), tempDate.ToString("MM"), tempDate.ToString("yyyy"));
        db = new SqlCommand(sql, connection);

        // Assign value
        decimal tickerPrice = (decimal)db.ExecuteScalar();

        // Add the ticker price * amount of shares to cash
        double new_amount = double.Parse(tickerPrice.ToString()) * m_Stocks;

        // Get cash
        sql = "SELECT AmtOfCash FROM Holding";
        db = new SqlCommand(sql, connection);
        decimalHolder = 0;
        decimalHolder = (decimal)db.ExecuteScalar();

        m_Cash = Double.Parse(decimalHolder.ToString());

        // Add to cash
        m_Cash += new_amount;

        string finalchange = Math.Round(m_Cash - 10000, 2).ToString();

        // Clear amount of shares to 0
        sql = String.Format("UPDATE Holding SET AmtofShares = 0", m_Stocks);
        db = new SqlCommand(sql, connection);
        db.ExecuteNonQuery();

        // Update cash
        sql = String.Format("UPDATE Holding SET AmtOfCash = {0}", m_Cash);
        db = new SqlCommand(sql, connection);
        db.ExecuteNonQuery();

        // Return
        return new JsonResult("Total Change: $" + finalchange);
    }

    // Choose a random date in the last 6 months function
    // TAN
    public string OnPostRandomDate
    {
        get
        {
            string randomDate = "";

            Random gen = new Random();

            // Find a random date 6 month or older
            DateTime start = new DateTime(2021, 7, 7);
            DateTime end = new DateTime(2022, 1, 18);
            int range = (end - start).Days;
            randomDate = start.AddDays(gen.Next(range)).ToString("yyyy-MM-dd");

            if (DateTime.Parse(randomDate).DayOfWeek.ToString() == "Sunday" || DateTime.Parse(randomDate).DayOfWeek.ToString() == "Saturday")
            {
                randomDate = DateTime.Parse(randomDate).AddDays(3).ToString("yyyy-MM-dd");
            }

            return randomDate;
        }
    }
    
    // Move the date forward by 1 month
    public string OnPostMoveForwardMonth
    {
        get
        {
            string sql = string.Format("SELECT StockDate FROM Holding");
            SqlCommand db = new SqlCommand(sql, connection);
            DateTime date = (DateTime)db.ExecuteScalar();
            m_Date = date.ToString("yyyy-MM-dd");
            string tempDate = DateTime.Parse(m_Date).ToString("yyyy-MM-dd");

            DateTime limitDate = new DateTime(2022, 6, 6);

            // Make sure it doesn't go past our oldest date
            if (DateTime.Parse(tempDate) >= limitDate)
            {
                return "cannot go forward for another month";
            }
            else
            {
                // Move forward a week
                string dates = DateTime.Parse(tempDate).AddDays(28).ToString("yyyy-MM-dd");
                return dates;
            }
        }
    }

    // Move the date forward by 1 week
    public string OnPostMoveForward
    {
        get
        {
            string sql = string.Format("SELECT StockDate FROM Holding");
            SqlCommand db = new SqlCommand(sql, connection);
            DateTime date = (DateTime)db.ExecuteScalar();
            m_Date = date.ToString("yyyy-MM-dd");
            string tempDate = DateTime.Parse(m_Date).ToString("yyyy-MM-dd");

            DateTime limitDate = new DateTime(2022, 6, 29);

            // Make sure it doesn't go past our oldest date
            if (DateTime.Parse(tempDate) >= limitDate)
            {
                return "COMPLETE";
            }
            else
            {
                // Move forward a week
                string dates = DateTime.Parse(tempDate).AddDays(7).ToString("yyyy-MM-dd");
                return dates;
            }
        }
    }

    // Move the date forward by 1 day
    public string OnPost1DayMoveForward
    {
        get
        {
            string sql = string.Format("SELECT StockDate FROM Holding");
            SqlCommand db = new SqlCommand(sql, connection);
            DateTime date = (DateTime)db.ExecuteScalar();
            m_Date = date.ToString("yyyy-MM-dd");
            string tempDate = DateTime.Parse(m_Date).ToString("yyyy-MM-dd");

            string randomDate;

            DateTime limitDate = new DateTime(2022, 7, 6);

            if (DateTime.Parse(tempDate) >= limitDate)
            {
                return "cannot go forward for another day";
            }
            else
            {
                // Move forward a day
                if (DateTime.Parse(tempDate).DayOfWeek.ToString() == "Friday")
                {
                    randomDate = DateTime.Parse(tempDate).AddDays(3).ToString("yyyy-MM-dd");
                }
                else
                {
                    randomDate = DateTime.Parse(tempDate).AddDays(1).ToString("yyyy-MM-dd");
                }

                return randomDate;
            }
        }
    }

    public void OnGet()
    {

    }
}