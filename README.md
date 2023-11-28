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
1. Go to `appsettings.Production.json`
   - Change your token to a 64-character string and adjust its expiry
     ```bash
     "JwtTokenSettings": {
     "Token": "token-here",
     "MinutesExpiry": 15
     }
   - Adjust your RefreshToken expiry
     ```bash
     "RefreshTokenSettings": {
     "DaysExpiry": 7
      }
   - Adjust your MemoryCache expiry
     ```bash
     "MemoryCacheSettings": {
     "HoursExpiry": 12
      }
   - Adjust your PasswordResetToken expiry
     ```bash
     "PasswordResetTokenSettings": {
     "MinutesExpiry": 15
      }
   - Add your path to the FFMpeg directories and adjust the conversion threads
     ```bash
     "FFMpegConfig": {
     "FFMpegBinaryDirectory": "path-here",
     "TemporaryFilesDirectory": "path-here",
     "VideoConversionThreads": 10,
     "PhotoConversionThreads": 8
     }
   - Change your SQL Connection string
     ```bash
     "ConnectionStrings": {
     "SDR": "connection-string-here"
     }
    - Setup your mail server and configure the MailSettings
        ```bash
       "MailSettings": {
      "Server": "server",
      "Port": 2525,
      "SenderName": "TalkHub",
      "SenderEmail": "email",
      "UserName": "username",
      "Password": "password"
       }
     
3. Navigate to the project directory:
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
dotnet run --environment Production
```
