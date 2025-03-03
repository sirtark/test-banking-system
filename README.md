<<<<<<< HEAD
# test-banking-system
=======
# Test Banking System

## Prerequisites
Ensure you have the following tools installed before proceeding:
- **.NET 8**
- **Node.js v22+**
- **npm v10+**

## Installation and Setup

### Backend Setup
1. Open **PowerShell** in the backend folder and run:
   ```sh
   dotnet restore
   ```
2. Apply the database migrations to create a local SQLite database (`ReSys.db`):
   ```sh
   dotnet ef database update
   ```
3. Start the backend server:
   ```sh
   dotnet run
   ```
   The backend will be running and listening at: `http://localhost:5000`

### Frontend Setup
4. Open another **PowerShell** terminal and navigate to the `frontend` folder. Install the project dependencies:
   ```sh
   npm install
   ```
5. Start the frontend application:
   ```sh
   npm start
   ```
   This will open your browser, displaying a list of services, dates, and times to make reservations.

## Usage
- Access the frontend at: `http://localhost:3000`
- The backend API is available at: `http://localhost:5000`

## License
This project is licensed under the MIT License.
>>>>>>> 7434ee6 (INITIAL)
