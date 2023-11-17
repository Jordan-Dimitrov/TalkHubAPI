# TalkHubAPI

## Overview
This is an ASP.NET Web API project that provides various features, including a video player, forum, photomanager, and messenger.
## Getting Started
### Prerequisites
Before you begin, ensure you have the following installed:

1. [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

2. [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

3. [FFmpeg](https://www.ffmpeg.org/download.html)

### Installation
1. Configure `appsettings.json` according to your preferences
2. Navigate to the project directory:
```bash
cd TalkHubAPI
```
3. Execute the following command to apply the initial migration and create the database:
```bash
dotnet ef database update
```
4. Run the following command to seed the database:
```bash
dotnet run seeddata
```
5. Run the project:
```bash
dotnet run
```
