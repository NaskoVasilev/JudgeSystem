# JudgeSystem
"C# MVC Frameworks - ASP.NET Core - June 2019" final Project

## Description
This is a judge system, similar to SoftUni judge but a bit more simple.
It can evaluate user' programming code automatically.
Aims of this platform are to be used in school. It can make work of our teachers more easier and also students will be
able to solve more problems and check their solutions faster. I beleive that using this application more students will become
passionate about programming and may become successfull developers one day.

# Getting Started
### Prerequisites
You will need the following tools:

* [Visual Studio](https://www.visualstudio.com/downloads/)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)

### Setup
Follow these steps to set up your development environmet:

  1. Clone the repository
  2. Create your own [Send Grid](https://sendgrid.com/) account or use existing one. Go to Settings/Api Keys and create new Api Key and then copy ApiKey Name and API Key ID.
  4. If you have some account in [Microsoft Azure](https://azure.microsoft.com/en-us/) create storge account. If you do not follow this step you will be able to set up the application but functinality about uploading files will not work.
  5. If you want to test java code, install [JDK](https://www.microsoft.com/net/download/dotnet-core/2.2)
  6. Open JudgeSystem.sln file right click on JudgeSystem.Web -> Add -> New Item. In the search bar search for `app settings` and then add ```App Settings File```. Replce its content with the following one and then replcae each value which starts with ```your```.
     ```
     {
        "ConnectionStrings": {
          "DefaultConnection": "Server=your server name;Database=JudgeSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
        },
        "Logging": {
            "IncludeScopes": false,
             "LogLevel": {
                  "Default": "Warning"
             }
          },
          "SendGrid": {
            "SendGridKey": "your API Key ID from SendGrid",
            "SendGridUser": "your ApiKey Name from Send Grid"
          },
          "AzureBlob": {
              "StorageConnectionString": "azure storage connection string",
              "AccountKey": " your azure storage api key",
              "AccountName": "yourazure storage acount name",
              "ContainerName": "your azure storage container name"
          },
          "Email": {
            "Name": "your first name",
            "Surname": "your last name",
            "Username": "your email"
          },
          "App": {
            "Name": "Judge System"
          },
          "Admin": {
            "Username": "your admin name",
            "Password": "your admin password",
            "Email": "your admin email",
            "Name": "your admin first name",
            "Surname": "your admin last name"
          },
          "Compilers": {
            "Java": "your path to javac.exe and java.exe. For exmaple C:\\Program Files\\Java\\jdk1.8.0_181\\bin",
            "CPlusPlus": "C:\\Users\\Nasko\\Desktop\\JudgeSystem\\Web\\JudgeSystem.Web\\wwwroot\\Compilers\\MinGW\\bin"
          }
      } 
       ```

  8. Open package manager console, choose as Defaut project: JudgeSystem.Data and run the following command: ```update-database```
  8. Press Ctrl + F5


## Functionality
### Guest Users
 - Login, Register
 - View all courses and lessons in this course
 - View home page with all active and previous contests
 
### Logged in Users
 - Submit solution and receive instant responese about how many points he has received
 - Submit solutions only in practice mode
 - Activate student profile using special activation key
 - View information about error, which occurs during excecution only of trial tests
 - View input and output data only of trial tests
 - Cannot view information about error, which occurs during excecution of official test
 - View execution result of tests
 - View their practice results
 - Download resources
 
 ## Users in role "Student"
  - All the functionalities of logged in user
  - Take part of competitions(Send solutions in compete mode)
  - Take part in exam and receive grade
  - Informtion about all passed exams is available in their profile
  - Participate in all contests available in the home page
  - View their compete and practice results
 
### Administrators(Teachers in school)
 - Add student profile to the system
 - Create, edit and delete course (Each course combines some lectures)
 - Create, edit and delete lectures
 - Have access to all contests' results
 - Filter contests' results by username, student class, contest start and end time etc.
 - Create contest for specific lecture with start and end time
 - Edit and delete contests
 - Create, delete and edit problem for specific lecture
 - Add, edit and delete resources for specific lecture
 - Create, edit and delete test for specific problem
 - View input and output data of each tests
 - View information about error, which occurs during excecution of some test

## Breif description of main functionalities
### Student profile
 - When student profile is added to the database, activation key is automatically generated, that is sent to the student's email
 - When the user enter this activation key, he becomes student and role "Student" is added to his roles
 - In this way he obtain full name, student email, number in class, name of class and some other privileges
 
 ### Submissions
  - If there is compile time error, the user can see what is the error
  - If solution is compiled successfully, all tests for this problem are executed over this solution
  - Execution results of tests are: (Success, Run time error, Memory limit, Time limit).
  - User can receive points in range 0 to problem's max points for his solution
  - The system finds user's best solution when process contests' results
  
  ### Lectures
  - Each lecture can be one of the tree types(Homework, Exercise or Exam)
  - Lecture can be added with some password which is really convenient for exam lecture
