[![.NET Build](https://github.com/impact-space/app/actions/workflows/dotnet.yml/badge.svg)](https://github.com/impact-space/app/actions/workflows/dotnet.yml) [![Last Commit](https://img.shields.io/github/last-commit/impact-space/app)](https://github.com/impact-space/app/commits/master) [![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)



# Impact Space

Impact Space is here to help you streamline your workflows, keep track  of your milestones, and ensure your quests lead to success. Our intuitive platform is designed with your needs in mind, providing you with powerful tools to manage your projects effectively. Our app is built on a robust, secure, and scalable framework, offering  multi-tenancy support to accommodate organizations of all sizes. Our goal is to help you stay on track, meet your deadlines, and ultimately achieve your project objectives.

[**Live Demo**](https://app.impactspace.io)

## Features

With Impact Space, you can:

- Democratically manage projects: Empower your team with a collaborative and democratic approach to managing projects.
- Create and manage milestones: Customize attributes like deadlines, budgets, priority levels, and status types.
- Associate quests or tasks: Ensure a clear and organized workflow with each milestone.
- Collaborate with team members: Allow for transparent communication and efficient delegation.
- Monitor progress: Receive real-time updates on milestone completion, budget usage, and quest statuses.

## Technologies and Libraries
- Blazor
- ASP.NET Core
- .NET 7
- C#
- PostgreSQL
- ABP Framework
- Blazorise

## Requirements

- .NET 7 SDK
- Node.js 18.x
- Volo.Abp.Cli (ABP CLI)
- PostgreSQL

## Community

Impact Space is an open-source project and we welcome contributions from the community. If you'd like to contribute, feel free to open an issue or create a pull request.

## Acknowledgments

A special thanks to [ProgCode](https://progcode.org) for their support in making this project possible.

## License

This project is licensed under the GNU General Public License v3.0. See the [LICENSE](LICENSE) file for more information.

## Getting Started

1. Clone the repository to your local machine.
2. Install the required tools:
   - Install .NET 7 SDK by following the instructions [here](https://dotnet.microsoft.com/download/dotnet/7.0).
   - Install Node.js 18.x by following the instructions [here](https://nodejs.org/en/download/).
   - Install ABP CLI by running `dotnet tool install -g Volo.Abp.Cli`.
   - Install and configure PostgreSQL by following the instructions [here](https://www.postgresql.org/download/).
     - You can also run PostgreSQL in docker.  The docker-compose.yml is located in the /etc folder.

```
bash
dotnet restore
```

4. Use the ABP CLI to install the required libraries for the Blazor app by running:

```
bash
abp install-libs --project-path src/ImpactSpace.Core.Blazor/ImpactSpace.Core.Blazor.csproj
```

5. Update the `appsettings.json` files in the `ImpactSpace.Core.Blazor` and `ImpactSpace.Core.DbMigrator` projects with the correct PostgreSQL connection string and other configuration settings.

6. Build the solution by running the following command in the `aspnet-core` folder:

```
bash
dotnet build
```

7. Run the database migration using the DbMigrator project:

```
bash
cd src/ImpactSpace.Core.DbMigrator
dotnet run
```

8. Start the Blazor app by navigating to the `ImpactSpace.Core.Blazor` project folder and running:

```
bash
cd ../ImpactSpace.Core.Blazor
dotnet run
```

9. Open a web browser and navigate to `https://localhost:44380` to access the Impact Space application.

You should now have the Impact Space application running locally on your machine.
