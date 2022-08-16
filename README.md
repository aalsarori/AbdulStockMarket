# Stock Investing Simulator
![homePageStock](https://user-images.githubusercontent.com/55166509/184940295-7ed86b4e-301f-46e9-9db0-05420d2055df.JPG)

# Description
A game that seeks to simulate guessing how to invest the stock market by looking at prior totals.

# Tools Used

.NET: server side functinality used .NET
MYSQL: MYSQL database was used to store data.
Front-End: HTML, CSS, and Javascript was used for Front-End.

# My Contributions

I implemented the Front end sliders. The project uses 4 different UIs to show the user how much money they are investing in vs how much they have left.
I implemented a slider, text box, pie chart and bar chart. 


![UI Stock](https://user-images.githubusercontent.com/55166509/184940328-484a797d-a3ec-4407-b2c9-affce521775b.JPG)

# Challenges

It was new to me to make UIs talk to one another. That is, when a value is changed on one UI, it refelcts on the other 3 UIs. The issue was simple after I learned how 
to work with functions in JS and make them talk to HTML elements. 

# Game Functionality

-Prompt the user to enter a ticker symbol. 
-Start with $10,000 in a bank account.
-Start the game on a random day, at least six months old.  Obtain the ticket symbol price for that day.   
-Ask the user if to buy, sell, hold, or quit.
-Upon sell, allow the user to select how much of owned stock to sell.  Investment income is stored the bank account. 
-Upon buy, use funds in the bank account to purchase stock.  The user selects how much to purchase.  Those funds go the investment account.
-Upon hold, no buy or sell occurs that day.
-Progress to the next day.  Use historical data to obtain the ticker symbol's price that next data.  Display changes to the investment account. 
-Let the program commence for at least 7 days.
-When the user selects to quit, all funds are sold from the investment account and put into the bank account.
-Display the overall gain or loss, and the elapsed time period. 
