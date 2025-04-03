# WeatherApp.Api

## Running the Project

1. Ensure you have the following prerequisites installed:
    - [.NET SDK](https://dotnet.microsoft.com/download)
    - SQL Server or a compatible database system
2. Clone the repository:
    ```bash
    git clone https://github.com/your-repo/WeatherApp.Api.git
    cd WeatherApp.Api
    ```
3. Update the `appsettings.json` file with your database connection string and an API key for openweathermap.
4. Restore dependencies:
    ```bash
    dotnet restore
    ```
5. Build the project:
    ```bash
    dotnet build
    ```
6. Run the application:
    ```bash
    dotnet run
    ```

## Setting Up the Database

1. Locate the `CreateDatabase.sql` file in the project directory.
2. Open your preferred SQL Server Management Studio or any SQL client.
3. Connect to your database server.
4. Execute the `CreateDatabase.sql` script to create the required database and tables.

## Additional Notes

- Ensure the database server is running before starting the application.
- For troubleshooting, check the logs in the console or the `logs` directory if configured.
- Feel free to customize the `appsettings.json` file for additional configurations.
- Refer to the documentation for further details on API endpoints and usage.

## Usage

- The application exposes an API that can be inspected using SwaggerUI.
- A hosted service is started at project startup that ingests data from the openweathermap api.

## Remaining Work

- There are no tests in this project. That is a major oversight at the moment.
- There is a bug in the handling of dates. They are serialized as "no locale" at the moment, which means they are misinterpreted by the client.
The intended timezone needs to be set both in the ingestion and in the repository layer. Using DateTimeOffset structs internally would make this easier.