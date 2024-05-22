# WebApp Project

## Overview

This project is a web application built using ASP.NET Core with Razor Pages. It runs inside a Docker container and interfaces with an API that connects to a NoSQL MongoDB database, also running in Docker containers. The application is configured for automatic deployment to Azure but can be configured to run in other environments.

## Architecture

- **WebApp Container**: Hosts the ASP.NET Core web application with Razor Pages.
- **API Container**: Hosts the API that the web application communicates with.
- **MongoDB Container**: Hosts the NoSQL MongoDB database for data storage.

## Features

- **ASP.NET Core with Razor Pages**: Provides the web interface.
- **Dockerized Deployment**: Ensures consistent environments for running the application.
- **NoSQL MongoDB**: Efficiently stores and manages data.
- **Auto-deployment to Azure**: Facilitates continuous integration and delivery.

## Prerequisites

- Docker
- Azure account (for deployment)
- .NET Core SDK

## Getting Started

### Local Development

1. Clone the repository:
    ```bash
    git clone https://github.com/your-repo/your-project.git
    cd your-project
    ```

2. Build Docker Images:
    ```bash
    docker-compose build
    ```

3. Run Docker Containers:
    ```bash
    docker-compose up
    ```
4. Adjust the API and Mongo Addresses:
    Located in the API.cs and appsettings.json files

5. Access the WebApp:
    Open your browser and go to [http://localhost:8080](http://localhost:8080)

### Deployment to Azure

The application is set up for automatic deployment to Azure. You can use the following steps to configure Azure deployment:

1. Deploy to Azure:
    The project uses GitHub Actions for CI/CD. Ensure your repository is connected to your Azure account and the pipeline is configured correctly.

2. Access the Deployed WebApp:
    An example web application is available at [https://waqqlyjb02.azurewebsites.net/](https://waqqlyjb02.azurewebsites.net/)

## Configuration

To run the application in environments other than Azure, you may need to adjust the configuration settings:

- **API Connection String**: Update the API connection string in the `appsettings.json` file.
- **MongoDB Connection String**: Update the MongoDB connection string in the `appsettings.json` file.


