# CarRentals
I used Moq in this project because im more familiar with it from my current work project. But I am aware of some issues/vulnerabilities in in. And would like to try NSubstitute or other alternatives with time.

To run this app you should probably 
- clone the repo
- open the solution in Visual Studio

run:
- dotnet restore
- dotnet build
- dotnet ef migrations add InitialCreate
- dotnet ef database update

and then you should be able to run the app either from Visual Studio or from the command line with `dotnet run`
