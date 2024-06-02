# Meeting Diary System

Meeting Diary System is a task management service developed on the .NET Core platform. It provides functionalities such as authentication, authorization, role management, file upload, logging with Serilog, response handling, pagination, email sending via SmtpClient, and much more.

## Features

- User authentication and authorization
- Role management
- Permission
- File upload
- Logging with Serilog
- Response handling and pagination
- Email sending via SmtpClient
- Using EF Core for database operations
- Creating a WebApi backend service

## Requirements

- .NET Core SDK
- Development environment such as Visual Studio or Visual Studio Code

## Installation

1. Clone the repository to your local machine.
2. Open the project in your development environment.
3. Install the required dependencies by running `dotnet restore`.
4. Configure the configuration files, including appsettings.json files for database connection and mail client settings.
5. Run the project by executing `dotnet run`.

## Usage

After successful startup, the service will be available at `http://localhost:PORT`, where `PORT` is the port specified in the project settings.

Additional documentation on available endpoints and service functionalities can be found in the source code and accompanying comments.
