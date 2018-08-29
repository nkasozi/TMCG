TMCG Online Shopping Site
--------------------------------------

A quick prototype for the TMCG Online Site described in the requirements. Ran out of time though

Requirements
------------
* .NET Framework 4.6.1 and above...Basically VS 2017

System Description
------------
Online Shopping Site with functionality below

* List Items for sale
* Allow one to add an item to a cart
* Pay using a credit card (you can use sandbox from any of your prefered providers)
* Allow one to choose to pay through a bank by cash. Your system would then communicate with the bank's system via a secure interface to get a notification when the payment has been made. This is KEY
* Store keepers should be able login and update stock but can't buy
* An administrator should be able to login to the same system to view some graphical reports

Tools/Frameworks used
-----------------------

* Asp.Net Web Forms with MVC for WebSite
* WCF Services for Rest Web Service Bank Calls
* MSTest for Unit Testing
* SQL Server 2017 for Database
* Castle Active Record used as ORM
* Chart JS is used for Graphs

Software Architecture Overview
-----------------------

* All the core logic is contained in a library 
* Website and WebAPI just display and collect data from end user, library does the validations, transformations, storage etc
* A Rest API is used for System to System notifications e.g the Bank notifying us of a deposit made on the account at the bank
* For a bank to use this API, they need to be created in the system and assigned a username,Password, SecretKey (which is used when they generating digital signatures)
* Hmac Sha256 is used to generate this digital signature
* PayWeb (A Pegasus Card processing Gateway) is used as the Card Processor


*Screenshots*
----------------------------
![Screenshots](https://i.imgur.com/Uo4hyi4.png)
![Screenshots](https://i.imgur.com/RxUunKl.png)
![Screenshots](https://i.imgur.com/gIRijJc.png)
![Screenshots](https://i.imgur.com/IquvMCu.png)
![Screenshots](https://i.imgur.com/IquvMCu.png)
![Screenshots](https://i.imgur.com/SMbG4ZK.png)
![Screenshots](https://i.imgur.com/XzjUHEc.png)
