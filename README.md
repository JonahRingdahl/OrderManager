This is an application that is used to manage and sort orders.

This application uses EntityFramework Core with Sqlite as the backend to run.

To build from source, clone the repo, have dotnet 10 installed on windows.

Ensure Dotnet Entity Framework core is installed:
dotnet tool install --global dotnet-ef

Run the:
dotnet ef database update

then run:
dotnet run .