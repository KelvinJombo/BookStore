# BookStore
This is an ECommerce application for Sampling and selling Different Books online. Users can Login on the Application through an Authentication and Authorization system and be able to access different endpoints in the Application according to their permissions. It has functionality for AddToCart, Remove FromCart, ViewCart and Checkout among many others
The Source Code for this BookStore Application was developed using Visual Studio 2022 Community Edition, Target Framework is .Net 7.Most NuGet packages are version 7.012.
Applied Entity Framework as Object Relational Mapper and Persistence is in Microsoft SQL Database.
Pull the Source Code into your local machine and set up your connection string for your Database. Then build and run the code in the Visual Studio IDE, its all ready to perform.
Add a User through the User's Controller and then Login as well. I set the EmailConfirmed property of IdentityUser to true due to Time Constraints which couldn't allow me to finish the Confirm-Email functionality. But the Application sends the email confirmation mail to the provided User's Email address.
Once Logged in, then grab you jwt Token which is displayed on successful Login as it will be required for accessing other user EndPoints in the Application.
You can Add a Book to the Store, Add a book to Cart, View Books in the Cart, Remove a Book from the Cart, Chechout a User and lots more functionality implemented. 
