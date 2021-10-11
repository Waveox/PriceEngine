Swagger UI available at http://localhost:5000/swagger/index.html

Default logging at C:\temp\PriceEngine

################################################################

Price Engine
- You will make an API for a price engine and a small calculator.
- The calculator will calculate the price of two different products to the store.
 
- The price calculation API should optimize the price. Ie if you order 25 units and there are 20 units in
each carton you get the price for 1 carton and 5 units.
- One endpoint should give back a list of prices for each product from 1-50 units.
- Another endpoint should be a simple calculator allowing to query for a product + number of units
and / or the number of cartons and give back the price. Think API for supporting a shopping cart at
online store.
 
- The service must have made unit-tests / integration test
- At least part of the price must be in a database (eg Mysql or free choice), but simplifications and
hard coding of price structures on the rest are ok where you think it makes sense. If you have a bad
time, drop database first.
 
- The functionality will be developed in Spring boot / Java or .Net Core
 
The prices are as follows:
- The product #1 has 20 units in each carton. A cardboard box costs 175 EUR, - 
- The product #2 has 5 units in each carton. A cardboard box costs 825 EUR, -
- If you buy units (not whole cartons) then you find the unit price from the carton price and add 30%.
(manual pick)
- If you buy 3 cartons or more, you get a 10% discount on the carton price.
