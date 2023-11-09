# TransAct API

API .NET 6 projects.

# Introduction

Welcome to BankApp, a simple web application that allows users to manage their bank accounts.
With BankApp, users can create new bank accounts in either USD or LBP currency, and perform transactions on them.
This is the backend part of our app, fully unit tested.

# Technologies Used

Our backend is developed using the following technologies:

1. .NET Core: The foundation of our backend application.
2. C#: The primary programming language for implementing the backend logic.
3. Entity Framework Core: A powerful and flexible Object-Relational Mapper (ORM) for working with databases.

# API Documentation

For detailed information on each API endpoint, parameters, request and response formats, refer to the API documentation in the code.

# Getting Started

# Prerequisites

Before you start, make sure you have the following installed:

1. .NET Core SDK: Download and install
2. A Database Server (e.g., MSSQL Server): Ensure you have a database server set up and running.

# Installation

1. Clone github repository:
   git clone https://github.com/Nourahlayhel/bankApp-backend.git

2. Restore dependencies and build
   run dotnet restore to restore all dependencies or simply dotnet build that restores and builds the project

3. Set up the database connection:

Open the appsettings.json file and update the connection string with your database details.
"ConnectionStrings": {
"BankApp": "your-database-connection-string"
},

4. Run your app
