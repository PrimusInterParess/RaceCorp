## RaceCrop

Asp.Net-Core application

> RC is web based applicaiton for sharing mountain bike rides and races. Users can easely login, register and connect to each other.
> Registered users can create race and ride event where other users could register for. Also registered users could create team and other users could join them.
> Users can connect to each other by sending and receiving request for connection.Can send messages one-another,troughout the chat system they could have live chat communication. Registerd users can upload pictures and manage content on their profile.

Technologies

- .Net Core 6.0
- Asp.Net Core MVC
- MVC Templete structure - https://github.com/NikolayIT/ASP.NET-Core-Template
- EntityFramework Core
- SQL Database
- SignlaR
- Automapper
- SendGrid
- Bootstrap templetes
- Google Auth API
- Google Service API

## **Project Overview**

## Solution
Contains 4 main layers

**1. Data - logic regarding database, configuration, models**

<img width="198" alt="DataLayer" src="https://user-images.githubusercontent.com/77731733/216320648-a2b99e97-67de-417a-83ae-3883a2efea65.png">

> - **RaceCorp.Data** - contains database logins regarding 
>      - Database context
>      - Migrations
>      - Repositories classes
>      - Configuration classes

Database diagram

![db](https://user-images.githubusercontent.com/77731733/214388526-1a473231-72ee-4642-80a7-0552c9a46e58.png)
> -  **RaceCorp.Common** - contains common logic regarding
>      - Abstract classes
>      - Interfaces
> -  **RaceCorp.Models** - contains entity models classes

**2. Services - logic regarding services, AutoMapper, Messages**

<img width="195" alt="Services" src="https://user-images.githubusercontent.com/77731733/216322496-c70b169f-5415-4859-905c-239fe1863bba.png">

> - **RaceCorp.Services** - contains common business logic
>    - Contains constants regarding services
>    - Validation attributes
>  -  RaceCorp.Data - contains main business logic 
>     - Interfaces
>     - Service classes
>   - RaceCorp.Mapping - contains logic regarding AutoMapper
>     - AutoMapper cofinguration class
>     - Interfaces
>     - Custom mapping
> -  RaceCorp.Messages - contains email-sender logic
>     - Interfaces
>     - SendGrid class
>     - NullEmailSender class

**3. Test**

<img width="196" alt="Tests" src="https://user-images.githubusercontent.com/77731733/216324547-bb7be8ef-76f9-4e32-9f77-313a73bf9673.png">

> - **RaceCorp.Services.Data.Tests** - contains business logic tests
> -  **RaceCorp.Web.Tests** - contains web tests

**4.Web**

<img width="192" alt="Web" src="https://user-images.githubusercontent.com/77731733/216325016-9dbe28fa-ab82-4ae1-a122-f5d95013212e.png">

> - **RaceCorp.Web** - contains main web logic
>     - Controllers
>     - API's
>     - Areas
>     - Hubs
>     - Views
>     - App settings
> - **RaceCorp.Web.Infrasructure** - contains common web logic
> - **RaceCorp.Web.ViewModels** - contains all classes regarding UI 

### **Usage**

- There are two main roles - **Admin** and **User** 

> ## **1. Admin account -** 
>  - Email: diesonnekind@gmail.com 
>  - Password: 123456
>  
> ## **2.Users Accounts -** 
>  - Email: kborisova@gmail.com
>  - Password: 123456
> 
>  - Email: kborisov@gmail.com
>  - Password: 123456

 ### **Register**



> **Users can register with their emails or with external login provider - Google.**


<img width="845" alt="register" src="https://user-images.githubusercontent.com/77731733/216332501-1397bb83-b1c0-49b8-8063-ad55d7becf96.png">

<img width="1506" alt="LOGINeXTERNAL" src="https://user-images.githubusercontent.com/77731733/216332510-beb45703-490e-45b7-bf5c-051bb3468d5c.png">

> **When Register or Login Users is directed to the Home Page .Where if you are an Admin you count have access to the Admin Panel**

<img width="992" alt="AdminHome" src="https://user-images.githubusercontent.com/77731733/216333421-94eec577-391c-4054-b35d-039b1b27a427.png">



> **Administrator can manage all the events and can receive messages send from the website contact form.**



<img width="991" alt="AdminPanel" src="https://user-images.githubusercontent.com/77731733/216333899-bad7d2ed-76d7-4900-86f0-3305396d863e.png">


> **On the left there is Side-bar where users can navigate themselves trough the website**


<img width="990" alt="Side bar" src="https://user-images.githubusercontent.com/77731733/216334715-3d4840d3-a010-44cd-bdd0-65b0a2fd27fc.png">

> The **SideBar** contains 
> - Search
> - Hrefs
> - **Users**
   
<img width="992" alt="AllUsers" src="https://user-images.githubusercontent.com/77731733/216335384-a90016a6-ee40-42a8-b347-4cd140e657ce.png">


  - **Teams**
  
<img width="993" alt="teams" src="https://user-images.githubusercontent.com/77731733/216335739-3f8b1469-284b-4650-9333-d1067671defe.png">


  - **Rides**
  
<img width="987" alt="rides" src="https://user-images.githubusercontent.com/77731733/216335919-d41a1ef8-bd32-439a-98e9-548457ec81f4.png">

  - **Races**
  
<img width="993" alt="races" src="https://user-images.githubusercontent.com/77731733/216336129-fea5f3db-fbb1-4798-afa8-114c5684c59b.png">


  - **Towns**

<img width="1082" alt="Towns" src="https://user-images.githubusercontent.com/77731733/216336367-3237bd04-900c-44e1-9d4a-19b5482c5fd5.png">


  - **Upcoming Events ect.**

### Connect

> Users can connect to each other by sending requests and can have **live** conversation between them

<img width="1081" alt="User" src="https://user-images.githubusercontent.com/77731733/216337230-43a80380-cb64-430d-87df-6f92ca60c258.png">

<img width="1081" alt="convers" src="https://user-images.githubusercontent.com/77731733/216337541-646d778c-a458-45b8-ace1-a7d5a7820faf.png">

## Create forms

> After user is registered and logged in, they could **create** 
>
> - **Team** - user can only participate in one team. Users who've crated teams are also team members. Team owners approve request from other users to be add to the   > team. They manage team activity. If team owner leaves the team, the first user added to the team becomes team owner. 

![Create team form](https://user-images.githubusercontent.com/77731733/216419259-fd40365a-0849-4d9d-b093-4c16330839b3.png)

![TeamAllUsers](https://user-images.githubusercontent.com/77731733/216420160-6feb22a0-10ce-4604-90da-75176eb0bfa6.png)


> - **Ride** - user can create ride by filling the form. The field **GPX** expects file with .gpx extension for the ride trace. After the form is submitted and the     > input data is valid, the ride is created and users is redirected to **All rides** page.

![Ride form](https://user-images.githubusercontent.com/77731733/216419060-f96a89b3-8054-41aa-822f-ddd628fa52c1.png)

> - **Race** - user can create race by filling the form. Many traces could be created for one race. Each trace has field **GPX** that expects file with .gpx extension  > for the trace. After the form is submitted and the input data is valid, the ride is created and users is redirected to **All races** page. Race could be created with  > no trace add, it could be added later.

![CreateRace](https://user-images.githubusercontent.com/77731733/216420444-029174a0-6aec-444b-861f-8e12a1ca3d28.png)

## **Register for event**

> Users can regiter for as many events as they want.Altoght they cannnot register for two traces of one race if there is not enough time for them to finish the first   > they have singh up for.

![RaceRegisterd](https://user-images.githubusercontent.com/77731733/216422078-07f0d09e-42c9-44f6-a3e4-800288eb8360.png)


![RaceRegisterd2](https://user-images.githubusercontent.com/77731733/216422075-9a4d5ded-9026-4124-81a2-166df75ef327.png)


