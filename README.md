# TalkHubAPI

## Overview
This is an ASP.NET Web API project that provides various features, including a video player, forum, photomanager, and messenger.
## Getting Started
### Prerequisites
Before you begin, ensure you have the following installed:

1. ASP.NET Core

2. SQL Server

### Installation
1. Navigate to the project directory
```bash
cd TalkHubAPI
```
2. Execute the following command to apply the initial migration and create the database:
```bash
dotnet ef database update
```
3. Run the following command to seed the database:
```bash
dotnet run seeddata
```
4. Run the project
```bash
dotnet run
```
