# Smart Campus Announcement and Notification Management System

This project was developed for the BIL 3204 final assignment.

## Project Description

Smart Campus Announcement System is a web-based announcement and notification management system. It allows creating campus announcements for different user types such as students and teachers. Users can subscribe to notification channels such as Email, SMS, and Push.

The system demonstrates Observer, Factory, and Singleton design patterns in a real working announcement publishing scenario.

## Technologies

### Backend
- ASP.NET Core WebAPI
- .NET 8
- Entity Framework Core
- SQL Server LocalDB
- Layered Architecture

### Frontend
- Angular 17
- Standalone Components
- TailwindCSS

## Design Patterns

### Observer Pattern
Used for notifying different user groups when an announcement is published.

Main files:
- `AnnouncementPublisher.cs`
- `IAnnouncementObserver.cs`
- `AnnouncementObserverBase.cs`
- `StudentAnnouncementObserver.cs`
- `TeacherAnnouncementObserver.cs`

### Factory Pattern
Used for creating announcement models and selecting notification channels.

Main files:
- `AnnouncementFactory.cs`
- `AnnouncementNotificationModelFactory.cs`
- `NotificationChannelFactory.cs`
- `ExamAnnouncement.cs`
- `EventAnnouncement.cs`
- `FoodMenuAnnouncement.cs`
- `LibraryAnnouncement.cs`

### Singleton Pattern
Used for application-wide logging.

Main file:
- `AppLogger.cs`

## Features

- Student and Teacher user types
- Exam, Event, Food Menu, and Library announcement types
- Email, SMS, and Push notification channels
- Announcement publishing flow
- Notification preferences
- Notification logs
- Notification summary report
- Demo scenario endpoint
- Angular frontend dashboard and demo pages

## Demo Video

YouTube demo video:  
[VIDEO_LINK_HERE](https://www.youtube.com/watch?v=IEO0kEXnux0)

## Running the Backend

```bash
cd backend/SmartCampusAnnouncementBackend
dotnet restore
dotnet build
dotnet ef database update --project SmartCampusAnnouncement.Infrastructure --startup-project SmartCampusAnnouncement.WebAPI --context AppDbContext
dotnet run --project SmartCampusAnnouncement.WebAPI --urls "http://localhost:5238"
