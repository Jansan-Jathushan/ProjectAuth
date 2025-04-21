#  ProjectAuth - Secure ASP.NET Core Web API with MongoDB, JWT & Role-Based Access

##  Overview

**ProjectAuth** is a secure and role-based Web API built using ASP.NET Core 9 and MongoDB.  
It features JWT authentication, ASP.NET Core Identity integration with MongoDB, and strict role-based authorization for managing products.

---

##  Features

-  MongoDB-backed ASP.NET Identity
-  JWT Token-based login & authorization
-  Role-based access control (`Admin`, `User`)
-  Product CRUD with Admin-only permissions
-  Full Swagger documentation + token testing

---

##  Tech Stack

- ASP.NET Core 9 (Web API)
- MongoDB (via Atlas or local)
- Identity.MongoDbCore + MongoDbGenericRepository
- JWT Bearer Authentication
- Swashbuckle/Swagger

---

##  Steps to Set Up and Run the Application


1.Install .NET SDK 9.0 or later

Download from: https://dotnet.microsoft.com/download

2.Set up MongoDB

Use MongoDB Atlas or a local MongoDB instance

For Atlas: create a cluster, user, and get your connection string

3.Update appsettings.json

Replace the placeholder values with your actual credentials:

"MongoDBSettings": {
  "ConnectionString": "your-mongo-connection-string",
  "DatabaseName": "Projectauth"
},
"Jwt": {
  "Key": "aSuperSecureJWTKeyWithAtLeast32Characters123!",
  "Issuer": "ProjectauthAPI",
  "Audience": "ProjectauthUsers",
  "DurationInMinutes": 60
}

 Make sure Jwt:Key is at least 32 characters long

4.Restore NuGet packages
dotnet restore

5.Build the project
dotnet build


6.Run the project
dotnet run


7.Open Swagger UI in your browser
http://localhost:2002/swagger



8.Register and login using Swagger

Use POST /api/Auth/register or POST /api/Auth/register-admin

Then login via POST /api/Auth/login

Copy the token from the response

Click  Authorize in Swagger

Paste your token in this format:


Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...


9.Test product APIs

Use token-authenticated endpoints like:

GET /api/Products (User/Admin)

POST /api/Products (Admin only)

PUT /api/Products/{id} (Admin only)

DELETE /api/Products/{id} (Admin only)

## Predefined Roles & Admin Registration
Roles:
Admin: Can perform all product operations (Create, Read, Update, Delete)

User: Can only read product information

Register an Admin:
Use this Swagger endpoint:


POST /api/Auth/register-admin
Example payload:


{
  "firstName": "Admin",
  "lastName": "User",
  "email": "admin@example.com",
  "password": "Admin123!"
}


Then login via:


POST /api/Auth/login
And copy the returned token. Use the  Authorize button in Swagger to test secured endpoints.

 API Usage Examples
 Register a User

POST /api/Auth/register

{
  "firstName": "J",
  "lastName": "D",
  "email": "jd@example.com",
  "password": "Jd123!"
}

 Login (User or Admin)

POST /api/Auth/login

{
  "email": "jd@example.com",
  "password": "Jd123!"
}
Returns:


{
  "token": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...",
  "expiration": "2025-04-22T01:22:12.231Z"
}
Paste this token into Swagger Authorize 

## Product API Endpoints
All endpoints require JWT. Admin-only endpoints require the Admin role.


Method	Endpoint	Access	Description
GET	/api/Products	User/Admin	View all products
POST	/api/Products	Admin only	Create new product
PUT	/api/Products/{id}	Admin only	Update product
DELETE	/api/Products/{id}	Admin only	Delete product
 
 
 
## Token Expiry Test
To test JWT token expiration:

Set "DurationInMinutes": 1 in appsettings.json

Login to generate a token

Use the token in any API →  should work

Wait 2 minutes

Try again →  will return 401 Unauthorized

## 📘 API Documentation

All API endpoints are documented using **Swagger** and available at:

http://localhost:2002/swagger


Features:
- Test endpoints directly in browser
- View request/response schemas
- Use JWT token via  **Authorize** button


###  Clone the repository

```bash
git clone https://github.com/Jansan-Jathushan/ProjectAuth.git
cd Projectauth
