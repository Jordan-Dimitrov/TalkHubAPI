# TalkHubAPI

## Overview
This is an ASP.NET Web API project that provides various features, including a video player, forum, photomanager, and messenger.
## Getting Started
### Prerequisites
Before you begin, ensure you have the following installed:

1. ASP.NET Core

2. SQL Server

3. FFmpeg

### Installation
1. Change the FFmpegConfig in the appsettings.json
```bash
    "FFmpegConfig": {
    "FFmpegBinaryDirectory": "path",
    "TemporaryFilesDirectory": "path"
  },
```
2. Navigate to the project directory
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
5. Run the project
```bash
dotnet run
```
