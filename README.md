# Configuration
## Dependencies 
- Visual studio **2022**
- .NET **4.8** SDK or higher
- dotnet **9**

## Installation
- ```git clone https://github.com/masterriko/UserDataExporter.git```
- open project in visual studio
- open `appsettings.json`
- change ```"Client-id"``` and ```"Client-secret"``` to given keys
- `Build` and `Run` project

## Important notes
- csv file is named `users.csv`
- When running the project csv file will appear under `UserDataExporter\UserDataExporter\bin\Debug`
- After building solution **exe** file will be in the same directory as csv file

# Project Specification

## 1. Overview
**UserDataExporter** project is a console application that retrieves user data from an API and exports the data into a CSV file. 
The application uses OAuth 2.0 authentication. Hence in order to authenticate with the API it needs **Client ID** and **Client Secret** from the configuration file `appsettings.json`. 
Once authenticated, the program calls an API endpoint to retrieve user data and exports it into a CSV file. User schema is named `userDto`.
For more information look at the code.

### Key Features:
- Authenticate with an API using **Client ID** and **Client Secret**.
- Retrieve user data from the API.
- Export user data to a CSV file.

## 2. API Details

### Authentication:
- **Endpoint**: `https://login.allhours.com/connect/token`
- **Method**: `POST`
- **Parameters**:
  - `grant_type`: `client_credentials`
  - `client_id`: Your `ClientId`
  - `client_secret`: Your `ClientSecret`
  - `scope`: `api`

### Retrieve Users:
- **Endpoint**: `https://api4.allhours.com/api/v1/Users`
- **Method**: `GET`
- **Headers**:
  - `Authorization`: `Bearer <AccessToken>`

# Test Scenarios

## Overview
These test scenarios validate the functionality of the **UserDataExporter** solution to ensure that it properly authenticates with the API, retrieves the necessary user data, and exports it to a CSV file.

## Test Scenario 1: Verify Authentication and Token Retrieval

### **Objective**:
To verify that the application correctly authenticates using the `ClientId` and `ClientSecret` from the `appsettings.json` and retrieves an access token.

### **Steps**:
1. Ensure that the `appsettings.json` file contains valid `ClientId` and `ClientSecret`.
2. Launch the application.
3. The application should make a request to the authentication endpoint (`https://login.allhours.com/connect/token`).
4. Verify that the application retrieves a valid access token and prints it to the console.
5. If authentication fails, the application should display an error message (`Failed to get access token`).

### **Expected Results**:
- The application should retrieve a valid access token.
- An error message should be shown if the authentication fails.

---

## Test Scenario 2: Verify API Data Retrieval

### **Objective**:
To verify that the application retrieves the user data from the API and handles any errors related to data retrieval.

### **Steps**:
1. Ensure that the `appsettings.json` file contains valid credentials (`ClientId` and `ClientSecret`).
2. Ensure the API endpoint (`https://api4.allhours.com/api/v1/Users`) is accessible.
3. Launch the application, which should use the retrieved access token to call the users' API endpoint.
4. Verify that the application fetches user data and outputs the data in a readable format.
5. If the API is inaccessible or the token is invalid, the application should output an appropriate error message.

### **Expected Results**:
- The application should successfully fetch user data from the API.
- If there is an issue with the API request (e.g., invalid credentials, unreachable API), the application logs the error which looks like ```"Failed to retrieve users. Response: " + response.StatusCode```.

---

## Test Scenario 3: Verify CSV File Generation

### **Objective**:
To verify that the application generates a CSV file containing user data after successfully retrieving it from the API.

### **Steps**:
1. Ensure that the API authentication and data retrieval work correctly by passing valid credentials.
2. Run the application.
3. After retrieving the user data, the application should generate a `users.csv` file.
4. Open the CSV file and check:
   - The file contains the correct column headers: `ID`, `Ime`, `Priimek`, `Mobil`.
   - The file contains valid user data under each column (e.g., `1`, `Ana`, `Novak`, `041222333`).

### **Expected Results**:
- The CSV file should have the following columns:
  - `ID`
  - `Ime`
  - `Priimek`
  - `Mobil`
- The data in the CSV file should match the user data retrieved from the API.
