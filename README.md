# St10407732_Prog7311_part2

**  YOUTUBE LINK**
  https://youtu.be/N_NXPqH5XQs
  
# Agri-Energy Connect Platform (Prototype)
**LOGIN CREDENTIALS FOR EMPLOYEE**
EMAIL: employee@agri.com
pASSWORD: Employee123!

## 📌 Overview

The **Agri-Energy Connect Platform** is a prototype web application designed to connect farmers with energy and agricultural services through a centralized system. This prototype demonstrates enterprise software development practices and showcases role-based functionality for **Farmers** and **Employees**.

---

## 🛠️ Development Environment Setup

### ✅ Prerequisites
- **Visual Studio 2022 or later**
- **.NET 8.0 SDK or later**
- **SQL Server Express / LocalDB**
- **Entity Framework Core**

### 📦 Steps to Set Up

1. **Clone the Repository**  
   Download the zipped source code or clone the repo via Git:
git clone https://github.com/ST10407732/St10407732_Prog7311_part2


2. **Open the Project**  
Launch `AgriEnergyPlatform.sln` in Visual Studio.

3. **Configure the Database Connection**  
Update the `appsettings.json` file with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AgriConnectDb;Trusted_Connection=True;"
}
**Apply Migrations **

 #User Roles & Functionalities
  login as a Farmer after being added by the employee and add product
  View Own Products: See a list of products they’ve added.

  Employee Role**
Login only (pre-seeded).
LOGIN CREDENTIALS
EMAIL: employee@agri.com
pASSWORD: Employee123!

Add Farmer Profiles: Register new farmers into the system.

View Products from All Farmers: Filter products by:
Date range
farmer name
Product type/category
System Design & Architecture


##Architecture: Layered MVC architecture (Presentation, Business Logic, Data Access)

##Design Pattern: Repository Pattern for clean separation of concerns

##Database: SQL Server (LocalDB) — chosen for simplicity and tight Visual Studio integration

##UX & UI Notes
Clean and minimal user interface built with Razor Views.

Clear navigation and role-specific access.

Validations on all form inputs to ensure data integrity.

Responsive layout for different device sizes.

##Development Process
Built iteratively and tested feature by feature.

Emphasis on simplicity and clarity for demo purposes.

Includes error handling for database operations and user input.


**How to Use**
## As a Farmer:
Register using your email and password.

Log in.

Add your products from the Add Product page.

View your added products in the My Products section.

## As an Employee:
Use the seeded credentials to log in.

Navigate to the Add Farmer section to register new farmers.

View products from all farmers using filters by date or type.


**Code Attributions**
Microsoft Identity Integration:

Author: Andy Malone MVP
Link: Microsoft Identity Tutorial
Date Accessed: 05 May 2025
MVC Application Framework:

Author: Microsoft
Link: ASP.NET Core MVC Tutorial
Date Accessed: 06 May 2025
Data Annotation Attributes:

Author: Simplilearn
Link: Data Annotation in ASP.NET MVC
Date Accessed: 06 May 2025
ASP.NET App with SSMS Database:

Author: Andy Malone MVP
Link: Database Connection Tutorial
Date Accessed: 05 May 2025
HTML Resources:

Author: w3schools
Link: HTML Tutorial
Date Accessed: 08 May 2025
CSS Resources:

Author: w3schools
Link: CSS Tutorial
Date Accessed: 08 May 2025
JQuery Resources:

Author: w3schools
Link: JQuery Tutorial
Date Accessed: 08 May 2025
JavaScript Resources:

Author: w3schools
Link: JavaScript Tutorial
Date Accessed: 08 May 2025
REGEX Validation Resources:

Author: Tyler Woodward
Link: Phone Validation Regex: The What, How, and Pros and Cons
Date Accessed: 08 May 2025
Database Implementation:

Author: Microsoft
Link: Working with SQL in ASP.NET Core
Date Accessed: 08 May 2025
LINQ Implementation:

Author: Grant Riordan
Link: How to Use LINQ
Date Accessed: 08 May 2025
Exception Handling:
