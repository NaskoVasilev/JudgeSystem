# JudgeSystem
"C# MVC Frameworks - ASP.NET Core - June 2019" final Project

## Description
This is a judge system, similar to SoftUni judge but a bit more simple.
Aims of this platform are to be used in school. It can make work of our teachers more easier and also students will be
able to solve more problems and check their solutions faster. I beleive that using this application more students become
passionate about programming and may become successfull developers one day.

## Functionality
### Guest Users
 - Login, Register
 - View all courses and lessons in this course
 - View home page with all active and previous contests
 
### Logged in Users
 - Submit solution and receive instant responese about how many points he has received
 - Submit solutions only in practice mode
 - Activate student profile using special activation key
 - Add, edit and delete resources for specific lecture
 - View information about error, which occurs during excecution only of trial test
 - Cannot view information about error, which occurs during excecution of official test
 - View execution result of tests
 
 ## Users in role "Student"
  - Take part of competitions(Send solutions is practice mode)
  - Take part in exam and receive grade
  - Informtion about all passed exams is available in their profile
  - Participate in all contests available in the home page
 
### Administrators(Teachers in school)
 - Add student profile to the system
 - Create, edit and delete course
  - Each course combines some lectures
 - Create, edit and delete lectures
 - Have access to all contests' results
 - Filter contests' results by username, student class, contest start and end time etc.
 - Create contest for specific lecture with start and end time
 - Create, delete and edit problem for specific lecture
 - Create, edit and delete test for specific problem
 - View input and output data of each test
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
  - 
  ### Lectures
  - Each lecture can be one of the tree types(Homework, Exercise or Exam)
  - Lecture can be added with some password which is really convenient for exam lecture
  
  
  
