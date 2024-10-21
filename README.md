# LogKeeper

LogKeeper is a web-based log storage system designed to work with PostgreSQL. It was originally created for a Unity project but can be easily adapted to other applications. The repository includes Docker Compose files for building containers, either for a self-sufficient stack that deploys both the app and the database, or for running just the app when a database is already deployed elsewhere.
<img alt ="main" src="https://github.com/user-attachments/assets/6eecc849-d8db-4433-a4d4-35dae28a01c4" width="500"> <img alt="localhost" src="https://github.com/user-attachments/assets/67ed358a-73f2-4204-b87a-7bcdbf7490e2" width ="500">

## Features

- Stores logs from Unity or other applications.
- Supports PostgreSQL for log data storage.
- Configurable for different deployment environments.
- Simple API for sending logs, which can be adapted to various projects.

### Additional Features

- **Log Grouping**: Logs can be organized into blocks, allowing for better structuring and management.
- **Filtering**: The application supports filtering logs based on regex criteria and can be easily modified. This helps to quickly locate specific logs.
- **Download Logs**: Logs can be downloaded as files, providing easy access for external review or backup purposes.
<img width="1325" alt="Screenshot 2024-10-21 at 23 58 35" src="https://github.com/user-attachments/assets/b892d37f-aba1-480b-9188-4764d8cd8e39">

## Getting Started

### Prerequisites

- Docker (if using Docker Compose for deployment)
- .NET 8 SDK (if running the application locally)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/AlekseiMishchuk/LogKeeper-PostgreSQL.git
cd LogKeeper-PostgreSQL
cd LogKeeperApp
```
2. To deploy with PostgreSQL, use the provided Docker Compose files:  
-	For self-sufficient deployment (both app and database):
```bash
docker compose -f docker-compose-local-db.yml up
```
- For application only (assuming PostgreSQL is already running):
```bash
docker compose -f docker-compose.yml up
```
3.	Update appsettings.json with your own configuration:
```json
{
  "PassToken": "your-token",
  "ConnectionStrings": {
    "PgConnection": "Host=your-postgres-host;Port=5432;Database=your-db;Username=your-user;Password=your-password"
  },
  "DetailedErrors": "true",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "LogKeeperUrl": "http://your-app-url/"
}
```
4.	Optionally, adjust launchSettings.json for your local environment configuration if needed.


## Unity Integration

To send logs from Unity to LogKeeper, you can use following method. This example sends logs with fields like title, author, project, and contents to the LogKeeper service:
```csharp
public IEnumerator SendRequestToLogKeeper(string title, string author, string project, string contents)
{
    const string host = "http://logkeeper.com";
    const string token = "2r366y82xk3y93xyx2m9(MX#RY#(#MXR";

    var form = new WWWForm();
    form.AddField("token", token);
    form.AddField("title", title.Limit(49));
    form.AddField("author", author.Limit(49));
    form.AddField("project", project.Limit(49));
    form.AddField("contents", contents);

    using var uwr = UnityWebRequest.Post($"{host}/keeper-save", form);
    uwr.timeout = 15;

    yield return uwr.SendWebRequest();
}
```

## Log Format

The logs sent to LogKeeper should include the following fields:

- title: Short description of the log (max 49 characters).
- author: Who is sending the log (max 49 characters).
- 	project: The project name related to the log (max 49 characters).
-  contents: Detailed content of the log.

Logs will be stored in the following order:

1. Title: Descriptive label for the log.
2.	Author: Sender’s name or system.
3.	Project: The related project identifier.
4.	Timestamp: The date and time when the log is received (automatically set by the system).
5.	Contents: Detailed message.

## Configuration

The key configuration file for the application is appsettings.json. Here are the essential fields:

- PassToken: Authorization token for API requests.
- ConnectionStrings: PostgreSQL connection string.
- LogKeeperUrl: Base URL where the application will be running.

Make sure to update these fields based on your environment. If you use Docker Compose, default settings are already configured for the internal PostgreSQL container.

## Contact

For more information, feel free to reach out or create an issue on GitHub.
