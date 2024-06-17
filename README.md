# CryptoPriceAPI

## Overview

CryptoPriceAPI is a .NET Core Web API that provides real-time price information for supported cryptocurrencies using data from CoinAPI. The API supports fetching price information for multiple cryptocurrencies simultaneously and stores this information in a SQL Server database.

## Features

- **Get List of Supported Cryptocurrencies**: Retrieve a list of all supported cryptocurrencies.
- **Store and Update Prices**: Prices are stored in a SQL Server database with the timestamp of the last update.

## Endpoints

1. **Get Supported Cryptocurrencies**
   - **URL**: `/api/cryptocurrency`
   - **Method**: `GET`
   - **Response**: JSON array of supported cryptocurrencies

2. **Get Price Information for Specified Cryptocurrencies**
   - **URL**: `/api/cryptocurrency/prices`
   - **Method**: `GET`
   - **Query Parameters**: `symbols` (list of cryptocurrency symbols, e.g., `BTC,ETH,XRP`)
   - **Response**: JSON array with price information for specified cryptocurrencies

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Visual Studio](https://visualstudio.microsoft.com/) (with Docker support)

## Setup and Launch

### Configuration

1. AppSettings: Configure the connection string and CoinAPI info in `appsettings.json`.
   
2. Docker Compose File: Ensure your `docker-compose.yml` file is configured to include the API service and a Posgresql instance.
    version: '3.4'

### Building and Running the Application

1. **Open Visual Studio**: Open the solution in Visual Studio.

2. **Set Docker Compose as Startup Project**: In the Solution Explorer, right-click the `docker-compose` project and select `Set as Startup Project`.

3. **Build and Run**: Press `F5` or click on the `Start Debugging` button to build and run the application. Visual Studio will use Docker Compose to build the images and start the containers.

4. **Access the API**: Once the application is running, you can access the API at `http://localhost:8080`.

### Swagger Documentation

- The API documentation is available via Swagger UI at `http://localhost:8080/swagger`.

By following these steps, you can set up and launch the CryptoPriceAPI using Docker Compose in Visual Studio.
