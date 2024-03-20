# Running the Project Documentation

# Prerequisites:

Ensure you have .NET 7 installed on your machine.
# Setting Up the Project:

Clone the project repository from GitHub.
Navigate to the project directory.
# Setting up the Database:

Open appsettings.json in the Presentation project.
Modify the MySQL connection string under DefaultConnection to match your server configuration.
# Setting up Paystack Integration:

In appsettings.json, replace the SecretKey under PaystackSettings with your Paystack secret key.
# Running the Project:

Build the solution.
Navigate to the Presentation project directory.
Run the project

# First-Time Setup:

run "dotnet ef database update --startup-project Presentation --project Infrastructure" from your terminal to update your database.
Upon first run, two admin accounts will be seeded into the database.
Use the following admin credentials to log in:
Username: john@example.com
Password: 1234
Username: jane@example.com
Password: 5678

# Additional Notes:

Ensure your MySQL server is running before starting the project.
