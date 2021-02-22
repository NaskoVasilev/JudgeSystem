# Judge System
An open-source judge system for evaluating programmming code and managing problems, lectures, exams and competitions. It is developed to
be used in schools in informatics lessons in order to improve the education and automate some parts of teachers' work.

[![Build Status](https://dev.azure.com/nasko01vasilev/JudgeSystem/_apis/build/status/JudgeSystem-CI?branchName=master)](https://dev.azure.com/nasko01vasilev/JudgeSystem/_build/latest?definitionId=2&branchName=master)

[![Build Status](https://dev.azure.com/nasko01vasilev/JudgeSystem/_apis/build/status/JudgeSystem-CI?branchName=develop)](https://dev.azure.com/nasko01vasilev/JudgeSystem/_build/latest?definitionId=2&branchName=develop)

## Awards
* Final project for the "ASP.NET Core MVC - June 2019" course in SoftUni, completed with excellent grade and the project was part of the five best applications in the course
* NTIT - National Autumn Tournament in Information Technology "John Atanasoff" - first place in "Web Applications" / НТИТ - Национален есенен турнир по информационни технологии „Джон Атанасов“ – първо място в направление „Интернет приложения“ 🏆
* The twentieth student conference of HSSIMI - gold medal and excellent performance / Двадесетата ученическа конференция (УК’20) на УчИМИ – златен медал и отлично преставяне 🏅
* The twentieth Student Section, 49th SMB Spring Conference - Medal for Excellent Performance / Двадесетата ученическа секция, 49.Пролетна конференция на СМБ – медал за отлично представяне 🏅
* Laureate of the National Olympiad in Information Technology / Лауреат на Националната олимпиада по информационни технологии

Click [here](https://github.com/NaskoVasilev/JudgeSystem/tree/master/Documentation/Awards) to see certificates, medals and awards from the contests above.

## Description
Applicaiton can evaluate user's programming code automatically.
Aims of this platform are to be used in schools. It can make the work of our teachers more easier and also students will be
able to solve more problems and check their solutions faster. I beleive that using this application, more students will become
passionate about programming and may become successfull developers one day.

## Documentation
You can find the documentation by navigating to the following path: Documentation/JudgeSystem - Documentation.docx.
Or if you prefer watching to reading. You can watch videos about the system.
* In [this](https://youtu.be/SEKTWCcHH-k) video you can watch how I presnet my JudgeSystem.
* In [this](https://www.youtube.com/embed/FbM2rhNMFVs) video you can watch how users can work in the system.
* In [this](https://www.youtube.com/watch?v=JjZ8iy4g0K0) video you can watch how administrators can work in the system.
* In [this](https://www.youtube.com/watch?v=GqCLoFPXkPs&feature=youtu.be) video you can watch how administrators can create lesson, add problem and resource to it, add tests to the problem and finally create contest for this lesson.
* In [this](https://www.youtube.com/watch?v=pQtEAqjQcIg&feature=youtu.be) video you can watch how users can submit solutions.
* In [this](https://www.youtube.com/watch?v=DA2GwSgnLx4&feature=youtu.be) video you can watch how administrators can review users' results and submissions.
* In [this](https://youtu.be/PUJqR24i65E) video you can watch how to test web project in the system. For example, ASP.NET Core web application.
* In [this](https://youtu.be/2HQStWCF4D0) video you can watch how to create problem which to be tested with automated tests(Unit tests, Integration tests).
* In [this](https://youtu.be/9V-F4xVxpoc) video you can watch how to add multiple tests for specific problem. How to download tests directly from systems like [INFOS](http://www.math.bas.bg/infos/) and import them in the system.

## Next steps
* Develop platfrom which ease integration of the Judge System in the schools and improve the process of learning content development and distribution (JudgeSystemManager)
* The new platform should have the following features:
  1. Learning content development and allow the users to collaborate with each other while developing learning content - GIT methodology should be used. The same techniques which we use while developing and managing code, should be used for learning content. Focus should be on collaboration and resources sharing.
  2. Schools can apply for new instances of the Judge System. They should choose their desired url address and choose which courses to be added in their instance.
  3. Schools should be able to make request about scaling their instances.
  4. When school's application is approved a new instance of the Judge System should be automatically deployed and the selected courses should be added to the newly deployed instance.
* This system is currently in development
* The Judge System should be extended and new functionalities should be added.
* The flow of adding students in the system should be changed
* The JudgeSystem should communicate with the JudgeSystemManager
* Click [here](https://github.com/NaskoVasilev/JudgeSystem/blob/master/Documentation/Next%20steps/How%20to%20improve%20Computer%20science%20education.pptx) to read how to improve Computer science edication.
* Click [here](https://github.com/NaskoVasilev/JudgeSystem/blob/master/Documentation/Next%20steps/Judge%20System%20next%20steps.docx) to read more about JudgeSystemManager.

## Getting Started
### Prerequisites
You will need the following tools:

* [Visual Studio](https://www.visualstudio.com/downloads/)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)

### Setup
Follow these steps to set up your development environmet:

  1. Clone the repository
  2. Create your own [Send Grid](https://sendgrid.com/) account or use existing one. Go to Settings/Api Keys and create new Api Key and then copy ApiKey Name and API Key ID.
  4. If you have some account in [Microsoft Azure](https://azure.microsoft.com/en-us/) create storge account. 
  If you do not want to create storage account in azure ```open StartUp.cs and commnent method ConfigureAzureBlobStorage```.
  You just will not be able to upload files.
  5. If you want to submit java code, install [JDK](https://www.microsoft.com/net/download/dotnet-core/2.2)
  6. Open JudgeSystem.sln file, right click on JudgeSystem.Web -> Add -> New Item. In the search bar search for `app settings` and then add ```App Settings File```. Replce its content with the following one and then replcae each value which starts with ```your```.
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

  7. Open package manager console, choose as Defaut project: JudgeSystem.Data and run the following command: ```update-database```
  8. Press Ctrl + F5

## Technologies
* .NET Core 2.2
* ASP.NET Core 2.2
* ASP.NET Core MVC
* Entity Framework Core 2.2
* Azure
* xUnit, MyTested.AspNetCore.Mvc
* jQuery, Bootstrap, JavaScript
* Automapper, SendGrid

## Languages and compilers
- C#
- Java 11 – compiler javac 11.0.4
- C++ - compiler g++ (MinGW GCC-8.2.0-3) 8.2.0

## Add new programming language
Follow this steps to add Python as another option for programming language
1. Create class ```PythonCompiler``` in project ```JudgeSystem.Comilers``` which implements ```ICompiler``` interface
```
using System.Collections.Generic;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class PythonCompiler : ICompiler
    {
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null)
        {
            var baseCompiler = new Compiler();
            string arguments = "Place Python compilation arguments here";
            return baseCompiler.Compile(arguments);
        }
    }
}
```
2. Add the following line in ```enum ProgrammingLanguages```:
```Python = 4```
3. Add the following code in class ```CompilerFactory```
```
case ProgrammingLanguage.Python:
    return new PythonCompiler();
```
4. Create class ```PythonExecutor ``` in project ```JudgeSystem.Executors``` which implements ```IExecutor``` interface
```
using System;
using System.Threading.Tasks;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class PythonExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            var baseExecutor = new Executor();
            string arguments = "Place Python program execution arguments here";
            return baseExecutor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
```
5. Add the following code in ```ExecutorFactory```
```
case ProgrammingLanguage.Python:
    return new PythonExecutor();
```
6. Add the followong line in ```GlobalConstants ```:
```public const string PythonFileExtension = ".py";```
7. Add the following check in method ```GetFileExtension``` in class ```UtilityService``` in project ```JudgeSystem.Services ```
```
else if(programmingLanguage == ProgrammingLanguage.Python)
{
    return GlobalConstants.PythonFileExtension;
}
```
  
## Project Architecture
![Architecture](Documentation/Architecture.jpg)
* Data access layer - works with the database using Entity Franework Core 2.2, this layer is independent from the others. It consists of two other layers:
  * Domain Layer - contains all entities, enums. Classes which represent tables in the database
  * Persistence Layer - contains database context, all configurations, migrations and data seeding logic. It is responsible for data persistance. Here is implmented Repository desing pattern which help us to accomplish more abstraction between data access logic and business logic. As a result we can our database provider without making so many changes to the code. For example we can chnage MS SQL with MongoDB without changing some business logic.
* Business Layer - main logic of the appliaction. It depends only on Data access layer but it uses repositories to access data so the coupling is very loose. It can be reuced in multiple appliactions. For example if we want to create some mobile version of the system, can reuse logic in this layer and we should also implement the new user intreface.
* Application Layer - consists of those elements that are specific to this application. It do the binding between the application and your business layer. It depends on business layer. It uses specific technologies and conceptions like: ASP.NET CORE, Middlewares and others. In our situation it's main functionality is to receive the request and send response to the clients.
* Presentation Layer -  contains all presentation logic. It used Razor view engine to generate html and also use technologies like: JavaScript, jQuery, AJAX.
* Common Layer - contains all the logic which is shared in the application. Contains global constants, custom exceptions and extension methods.
* Workers - .NET Standard class libraries which contains some more complicated logic. They are used by the business layer in order to keep the code simple there. The most compilacted logic in the appllication is related to complication and execution of submissions. So this logic is implemented here in two differnet projects.
* Tests - the system is tested with a lot of automated tests - unit tests and integration tests. We use libraries like Moq, xUnit and Microsoft.EntityFrameworkCore.InMemory to the all the logic in business layer.
* Code quality - project follows SOLID principles and all other principles of high quality code. Also there are ```.editorconfig``` file in which are defained all code styles and conventions in order the code of the project to be consistent.

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
