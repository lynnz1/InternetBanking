S3512958 Lingyu Zhang

Github: https://github.com/rmit-wdt-fs-2022/s3512958_a2

*Please run the database migration of the customer application. The data seeding also runs from the customer application

*Code examples given in tutorial and lectorial have been used in this project 

1. Customer Website

The customer web site has four main functionalities. 
- /Customer This is the homepage after user logged in. It shows the customer name and all the accounts. The user can deposit/withdraw/transfer by clicking the buttons on this page.

- /MyStatement This page shows all the account. User can view transaction statements.

- /MyProfile View and edit customer details and update password

- /BillPay Create new billpay or edit/delete existing billpay

2. Web API port(localhost:5500)

GET /API/AdminLogin return the admin username and hashed password

GET /API/Accounts return all accounts of all users

GET /API/Transactions/id Get all transactions of an account with account number, if startdate and enddate is also passed, this would return filtered result.

GET /API/BillPays/id Get all scheduled billpay of an account with account number.

GET /API/BillPay/id Get specific billpay with billpay id

GET /API/Customers Get all customers

GET /API/Customer/id Get specific customer details

GET /API/Login/id Get specific login detail of a customer, search with customer id

PUT /API/BlockBillPay
PUT /API/UnBlockBillPay block and unblock a bill pay, takes in a billpay id.

PUT /API/LockLogin
PUT /API/UnLockLogin lock and unlock a user.

PUT /API/UpdateCustomer update a customer detail

3. Admin Portal 

1. View transaction history of an account. The admin can select an account from the list of all accounts and view transactions. The admin can also block a billpay.

3. The admin can modify user detail and lock an user by selecting a customer from customer table. 





