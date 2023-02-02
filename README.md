# **Project Overview**

## RaceCrop
Asp.Net-Core application

RC is web based applicaiton for sharing mountain bike rides and races. Users can easely login, register and connect to each other.

Create

- Races
- Rides
- Teams
- Connection between users
- Conversation between users

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
- Google for authentication and uploading .gpx files

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

##**Usage**

- [ ] There are two main roles - **Admin** and **User** 

> **### 1. Admin account -** 
>  - Email: diesonnekind@gmail.com 
>  - Password: 123456
>  
>  **### 2.Users Accounts -** 
>  -Email: kborisova@gmail.com
>  -Password: 123456
> 
>  -Email: kborisov@gmail.com
>  -Password: 123456

 **Register**

Users can register with their emails or with external login provider - Google.

<img width="845" alt="register" src="https://user-images.githubusercontent.com/77731733/216332501-1397bb83-b1c0-49b8-8063-ad55d7becf96.png">

<img width="1506" alt="LOGINeXTERNAL" src="https://user-images.githubusercontent.com/77731733/216332510-beb45703-490e-45b7-bf5c-051bb3468d5c.png">

When Register or Login Users is directed to the Home Page .Where if you are an Admin you count have access to the Admin Panel

<img width="992" alt="AdminHome" src="https://user-images.githubusercontent.com/77731733/216333421-94eec577-391c-4054-b35d-039b1b27a427.png">

Administrator can manage all the events and can receive messages send from the website contact form.

<img width="991" alt="AdminPanel" src="https://user-images.githubusercontent.com/77731733/216333899-bad7d2ed-76d7-4900-86f0-3305396d863e.png">



