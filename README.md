# Recall-Backend
The backend to Recall project.

# How to run locally?
Clone the repository.\
Rename the appsettings1.json to appsettings.json.\
Replace the JWT Secret with some base64 word. You can use https://generate.plus/en/base64 to generate one.\
You need either mssql or postgreSQL database installed on your computer.\
In appsettings.json create your connection string.\
In Startup.cs change the in development services.AddDbContext to UseNpgsql or UseSql depending on what you are using.\
If you are using postgreSQL apply the migrations and you are ready.\
For Sql you have to delete all migrations, create a new one and apply it.\
When you run the project, by default it will go to the values controller, redirect to "/". 

  
