# BlazorTicketmasterApp


## Project Description

This project is a .NET application that interacts with the Ticketmaster API to fetch and display information about attractions and events. The application includes models for attractions, events, and images, and services to handle API requests and responses.

## App Features

### Key Features Highlight

- **Search Bar: Enables users to search for attractions using the Ticketmaster API.
- **Attractions List: Dynamically shows results from the search query with visual cards.
- **Attraction Details: Displays details of selected attractions, including upcoming events.
- **Responsive UI: Optimized for mobile and desktop using Bootstrap.
- **Blazor Components: Efficient use of Blazor components for reusability and clean code structure.

## Necessary Tools and Frameworks

- **.NET 8.0 SDK**: The project is built using .NET 8.0.
- **Visual Studio 2022**: Recommended IDE for development.
- **Entity Framework Core**: Used for data modeling and database interactions.
- **Newtonsoft.Json**: Used for JSON parsing and serialization.
- **HttpClient**: Used for making HTTP requests to the Ticketmaster API.
- **ILogger**: Used for logging information and errors.

- ## Branches

### Master Branch
- **Branch Name**: `master`
- **Purpose**: This is the default branch where the source code of HEAD always reflects a production-ready state.

### Development Branch
- **Branch Name**: `development`
- **Purpose**: This branch is used for ongoing development. New features and bug fixes are integrated here before being merged into the `master` branch.

## My Branch Workflow

1. **Creating a Feature Branch**:

   - **Branch Name**: `feature/feature-name`
   - **Purpose**: To work on a new feature or bug fix.
   - **Base Branch**: `development`
   - command: `git checkout -b feature/feature-name`

2. **Committing Changes**:

	- git add . git commit -m "Add new feature"

3. **Merging a Feature Branch into Development**:
  
   **Creating a Pull Request**:

   - **Base Branch**: `development`
   - **Compare Branch**: `feature/feature-name`
   - **Purpose**: To review and merge the changes into the `development` branch.

4. **Merging Development into Master**:
	
	 **Creating a Pull Request**:

	 - **Base Branch**: `master`
		- **Compare Branch**: `development`
		- **Purpose**: To review and merge the changes into the `master` branch.

 5. **Deleting a Feature Branch**:
	
	1. **Command**: `git branch -d feature/feature-name`


## Project Structure

The project has been restructured to include the following components:


- **BlazorTicketMasterApp.Shared**: A separate library project that contains shared code and resources.

- **Models**
  - `AttractionDto.cs`: Represents an attraction with associated events.
  - `EventDto.cs`: Represents an event with a foreign key to an attraction.
  - `ImageDto.cs`: Represents an image with a URL property.    
  - `ExternaLldDto.cs`: Represent an external link with a URL property. It is used in the AttractionDto class.

- **Services**
  - `TicketmasterService.cs`: Contains methods to interact with the Ticketmaster API, including fetching attraction details, searching for attractions, and getting events by attraction ID.
  - `TicketmasterResponse.cs`: Contains the response from the Ticketmaster API.


- **BlazorTicketMasterApp**: A separate project dedicated to unit tests.

- **Components**
  - `AttractionDetails.razor`: Displays details about an attraction, including its name, description, and events.
  - `AttractionList.razor`: Displays a list of attractions with their names and images.
  - `SearchBar.razor`: Allows users to search for attractions by keyword.
  - `Index.razor`: The main page of the application that displays the search bar and attraction list. It also handles the search functionality and displays attraction details when an attraction is selected.
	 Parent component of AttractionList and SearchBar components.
- **Pages**
- **Shared**
- **wwwroot**
- **wwwroot/css**
  - Contains the css files for the application.
- **appsettings.json**
  - Contains the configuration settings for the application, including the Ticketmaster API key and base URL.
- **Program.cs**
  - `Program.cs` is the entry point of the application. It creates a new Blazor WebAssembly application and configures the services. Dependency injection is also configured in this file.
- **Dockerfile**
  - `Dockerfile` is used to build a Docker image for the application. It copies the application files, restores the dependencies, and builds the application.

- **BlazorTicketMasterApp.UnitTests**: A separate project dedicated to unit tests.

- `CustomHttpMessageHandler.cs`: A custom HTTP message handler that allows mocking HTTP responses in unit tests.This class is used to mock the HttpMessageHandler class and expose the SendAsync method as a Func as part of the public properties
- `TicketmasterServiceTests.cs`: Contains unit tests for the TicketmasterService class. It includes tests for fetching attraction details, searching for attractions, and getting events by attraction ID. This test class uses the CustomHttpMessageHandler class to mock HTTP responses.						
   and more.It was included in the project to demostrate how to write unit tests for the project. It runs as part of the CI Pipeline in GitHub Actions.

## Steps to Deploy the Solution Locally.

1. **Clone the Repository**

2. **Install .NET 8.0 SDK**
   Download and install the .NET 8.0 SDK from the [official .NET website](https://dotnet.microsoft.com/download/dotnet/8.0).

3. **Open the Project in Visual Studio**
   Open Visual Studio 2022 and load the solution file (`.sln`).

4. **Restore NuGet Packages**
   In Visual Studio, go to `Tools` > `NuGet Package Manager` > `Manage NuGet Packages for Solution` and restore the required packages.

5. **Configure API Key**
   Add your Ticketmaster API key to the `appsettings.json` file under the `Ticketmaster` section:
	{ "Ticketmaster": { "ApiKey": "your-api-key-here" } }

6. **Build the Solution**
   Build the solution by clicking on `Build` > `Build Solution` or pressing `Ctrl+Shift+B`.

7. **Run the Application**
   Run the application by clicking on `Debug` > `Start Debugging` or pressing `F5`.

## Continious Integration and Continious Deployment(CI/CD).

This project is configured for Continuous Integration (CI) and Continuous Deployment (CD) using GitHub Actions. Whenever changes are pushed to the `development` branch, the CI pipeline runs tests(There is no Tests in the project due to its simplicity) and builds the project. When changes are merged into the `master` branch, the application is automatically deployed to Azure App Service.

- **Deployment Service**: Azure App Service
- **Deployment Method**: GitHub Actions
- **App URL**: (https://blazorticketmasterapp.azurewebsites.net/)

### Continuous Integration (CI)

1. **Configure GitHub Actions for CI**:
   - Ensure you have a `.github/workflows` directory in your repository.
   - Add a workflow YAML file (`master_blazorticketmasterapp.yml`) to define the CI process.
   - The GitHub Actions workflow has been updated to include unit tests as part of the CI pipeline.
	
	name: Run Unit Tests
    run: dotnet test BlazorTicketMasterApp.UnitTests/BlazorTicketMasterApp.UnitTests.csproj --verbosity normal

	This ensures that every push and pull request to the `Master` branch will trigger the CI pipeline, which includes building the project and running the unit tests.

### Continuous Deployment (CD)

2. **Configure Azure App Service Deployment Center**:
   - An Azure App Service is required to deploy the application.This was done manually in the Azure Portal.
   - In `Deployment Center` > `GitHub`, the deployment source and branch were configured.

## Logging

The application uses `ILogger` for logging HTTP client configurations, API responses, and errors. Logs can be viewed in the Output pane of Visual Studio.

## Error Handling

The application includes error handling for HTTP request errors, task cancellations, JSON deserialization errors, and unexpected exceptions. Errors are logged using `ILogger`.

## Time Tracking

Below is a log of the time spent developing this project:

- **Initial Setup**: 
- *3 hours* This includes setting up the project, Github repository, Development Environment and configuring the Ticketmaster API key.
- **Model Creation**:
- *3 hours*
- **Service Implementation**: 
- *5 hours*. This includes the implementation of the TicketmasterService class, which contains methods to interact with the Ticketmaster API.
- **API Integration**: 
- *5 hours*. This Includes understanding the Ticketmaster API documentation, making HTTP requests, and handling JSON responses.
- **Testing and Debugging**:
- *3 hours*. I was not needed to create unit tests for this project. I tested the application manually by running it and verifying the results.
- **Documentation**: 
- *1 hour*

**Total Time Spent**: **20 hours**

## Conclusion

This project demonstrates how to interact with the Ticketmaster API using .NET, handle JSON responses, and manage data models and services. Follow the steps above to set up and deploy the solution.

