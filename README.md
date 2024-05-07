---
page_type: sample
languages:
- bash
- csharp
- data-api-builder
- dotnetcli
- dockerfile
- html
- graphql
- javascript
- md
- sql
- tsql
- xml
- yml
products:
- azure-sql-database
- blazor
- github
name: Sample Library App - SQL Server and Data API builder with Blazor
urlFragment: azure-sql-library-app-blazor
description: Library app built with SQL Server and Data API builder as backend, and Blazor as frontend. 
---

# Sample Library App: SQL Server, Data API builder, and Blazor Environment
> This documentation provides an in-depth guide to setting up and utilizing the development environment for the Library App.

[![Open in Remote - Containers](https://img.shields.io/static/v1?label=Remote%20-%20Containers&message=Open&color=blue&logo=visualstudiocode)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/microsoft/vscode-remote-try-dab)


## Overview

The Data API builder and SQL Server dev container template offers a streamlined environment for developing applications with a backend powered by SQL Server and Data API Builder, along with a Blazor frontend. This template provides a consistent development environment across different machines and ensures compatibility with your application stack.

A **development container** is a running [Docker](https://www.docker.com) container with a well-defined tool/runtime stack and its prerequisites. You can try out development containers with **[GitHub Codespaces](https://github.com/features/codespaces)** or **[Visual Studio Code Remote - Containers](https://aka.ms/vscode-remote/containers)**.

This is a sample project that lets you try out either option in a few easy steps. We have a variety of other [vscode-remote-try-*](https://github.com/search?q=org%3Amicrosoft+vscode-remote-try-&type=Repositories) sample projects, too.

> **Note:** If you already have a Codespace or dev container, you can jump to the [About this template](#About-this-template) section.


## Setting up the development container

### GitHub Codespaces

Follow these steps to open this sample in a Codespaces:

1. Click the Code drop-down menu and select the **Codespaces** tab.
1. Click on **Create codespaces on main** at the bottom of the pane.

For more info, check out the [GitHub documentation](https://docs.github.com/en/free-pro-team@latest/github/developing-online-with-codespaces/creating-a-codespace#creating-a-codespace).

### VS Code Dev Containers

If you already have VS Code and Docker installed, you can click the badge above or [here](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/microsoft/vscode-remote-try-sqlserver) to get started. Clicking these links will cause VS Code to automatically install the Dev Containers extension if needed, clone the source code into a container volume, and spin up a dev container for use.

Follow these steps to open this sample in a container using the VS Code Dev Containers extension:

1. If this is your first time using a development container, please ensure your system meets the pre-reqs (i.e. have Docker installed) in the [getting started steps](https://aka.ms/vscode-remote/containers/getting-started).

2. To use this repository, you can either open the repository in an isolated Docker volume:

    - Press <kbd>F1</kbd> and select the **Dev Containers: Try a Sample...** command.
    - Choose the ".NET Core" sample, wait for the container to start, and try things out!
        > **Note:** Under the hood, this will use the **Dev Containers: Clone Repository in Container Volume...** command to clone the source code in a Docker volume instead of the local filesystem. [Volumes](https://docs.docker.com/storage/volumes/) are the preferred mechanism for persisting container data.

   Or open a locally cloned copy of the code:

   - Clone this repository to your local filesystem.
   - Press <kbd>F1</kbd> and select the **Dev Containers: Open Folder in Container...** command.
   - Select the cloned copy of this folder, wait for the container to start, and try things out!

### About this template

This template sets up two containers: one for the Dev Container housing .NET and Data API Builder, and another for Microsoft SQL Server. Upon connection, you'll find yourself in an Ubuntu environment, with easy access to the SQL Server container on localhost port 1433. Within the Dev Container, you'll discover supporting scripts located in the .devcontainer/sql folder, essential for configuring the Library sample database.

The SQL container runs on the latest developer edition of Microsoft SQL 2022. Within VS Code, databases are conveniently accessible through the MSSQL extension, labeled "LocalDev". The default sa user password is `P@ssw0rd!`, and SQL port `1433` is the default, as defined in `.devcontainer/docker-compose.yml`. Additionally, database setup occurs when the Dev Container is started, employing the `.devcontainer/sql/postCreateCommand.sh` file. The `.devcontainer/sql/library.azure-sql.sql` script is utilized to create the library database and its objects. Moreover, a SQL Database project is included, offering the means to locally update the database. Pre and post scripts ensure the utilization of static data for demo purposes.

Data API Builder is seamlessly integrated into the .NET container. Included in this repository is a preconfigured database, utilized by DAB to generate REST and GraphQL endpoints. For manual testing, leverage the `dab_http_request.sh` file found in the `scripts` folder. This script offers multiple HTTP request calls, aiding in understanding Data API Builder's interactions with the SQL server. Data API Builder provides a Swagger UI accessible at `http://127.0.0.1:5001/swagger` for exploring and testing REST endpoints. Additionally, a GraphQL endpoint is available at `http://127.0.0.1:5001/graphql`, powered by Banana Cakepop utility, enabling intuitive interaction with the GraphQL layer.

Furthermore, the Blazor project, a simple web application, serves as the frontend for this development environment. It seamlessly integrates with Data API Builder's GraphQL and REST endpoints, providing a user-friendly interface for interacting with the SQL server. Leveraging the power of Data API Builder, the Blazor project facilitates smooth communication between the frontend and backend components, ensuring efficient data retrieval and manipulation.

By harnessing Data API Builder's capabilities, the Blazor project simplifies the development process, enabling developers to focus on building robust, feature-rich web applications without the complexities of backend infrastructure. Whether it's fetching data from the SQL server, executing complex queries, or performing CRUD operations, the Blazor project provides a seamless and intuitive user experience for interacting with the underlying data layer.

To get started with this project, simple execute the [VS Code Tasks](#VS-Code-Tasks) from the next section section. These tasks will help you to  run Data API builder, also to trust the HTTPS certificate for the Blazor project, and run the Blazor project. 

#### VS Code Tasks

We have added several tasks to this repository to help with common actions. You can access these tasks by opening the Command Palette in VS Code. Here's how:

1. Press <kbd>F1</kbd> or <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>P</kbd> to open the Command Palette.
2. Type "Run Task" and select "Tasks: Run Task".
3. Choose the task you want to run from the list.

##### Execute SQL Query

This task opens the `verifyDatabase.sql` file in your workspace and executes the SQL query in it. It uses the `ms-mssql.mssql` extension to execute the query. This task is part of the build group and is the default task that runs when you run the build task group.

##### Run DAB

This task starts the DAB server with the specified configuration file. It runs the command `dab start --config=dab.config.json --no-https-redirect` in the `dab` directory of your workspace.

##### Build SQL Database project

This task builds the SQL Database project. It runs the command `dotnet build` in the `database/Library` directory of your workspace.

This task is not required to run the application, but it is useful to verify the database schema. You can use this SQL Database project to make changes to the database schema and deploy it to the SQL Server container.

##### Trust HTTPS certificate for Blazor project

This task trusts the HTTPS certificate for the Blazor project. It runs the command `dotnet dev-certs https --trust` in the `app/BlazorLibrary` directory of your workspace.

##### Build Blazor project

This task builds the Blazor project. It runs the command `dotnet build` in the `app/BlazorLibrary` directory of your workspace.

##### Run Blazor project

This task runs the Blazor project. It runs the command `dotnet watch run` in the `app/BlazorLibrary` directory of your workspace.

#### Changing the sa password

To change the `sa` user password, change the value in `.devcontainer/docker-compose.yml` and `.devcontainer/devcontainer.json`.

#### Database deployment

By default, a  demo database is created titled "Library".  To add additional database objects or data through T-SQL during Codespace configuration, edit the file `.devcontainer/sql/library.azure-sql.sql` or place additional `.sql` files in the `.devcontainer/mssql/` folder. *Large numbers of scripts may take a few minutes following container creation to complete, even when the SQL server is available the database(s) may not be available yet.*

You can also the SQL Database Projects extension to deploy the database schema. The `Library.sqlproj` project is located in the `database/Library` folder can be build using the `Build SQL Database project` task. The `verifyDatabase.sql` file in the `database/Library` folder can be used to verify the database schema.

Alternatively, .dacpac files placed in the `./bin/Debug` folder will be published as databases in the container during Dev Container configuration. [SqlPackage](https://docs.microsoft.com/sql/tools/sqlpackage) is used to deploy a database schema from a data-tier application file (dacpac), allowing you to bring your application's database structures into the dev container easily. *The publish process may take a few minutes following container creation to complete, even when the server is available the database(s) may not be available yet.*

### Adding another service

You can add other services to your `.devcontainer/docker-compose.yml` file [as described in Docker's documentation](https://docs.docker.com/compose/compose-file/#service-configuration-reference). However, if you want anything running in this service to be available in the container on localhost, or want to forward the service locally, be sure to add this line to the service config:

```yaml
# Runs the service on the same network as the database container, allows "forwardPorts" in devcontainer.json function.
network_mode: service:db
```

### Using the forwardPorts property

By default, web frameworks and tools often only listen to localhost inside the container. As a result, we recommend using the `forwardPorts` property to make these ports available locally.

This project uses the `5000` and `5001` ports for DAB, and the port `1433` for SQL Server:

```json
"forwardPorts": [5000, 5001, 1433]
```
> **Note:** You can add additional ports to this list as needed.

The `ports` property in `docker-compose.yml` [publishes](https://docs.docker.com/config/containers/container-networking/#published-ports) rather than forwards the port. This will not work in a cloud environment like Codespaces and applications need to listen to `*` or `0.0.0.0` for the application to be accessible externally. Fortunately the `forwardPorts` property does not have this limitation.