# Selenium Form Automation Tester

A robust C# Selenium automation project that tests form fields with multiple fallback strategies to ensure reliability even when element properties change.

## Features

- 🎯 **Multiple Element Location Strategies**: Uses 5+ different methods to find each form field
- 🔄 **Automatic Fallback**: If one strategy fails, automatically tries the next one
- 📝 **Comprehensive Testing**: Tests First Name, Email, and Mobile fields
- 🛡️ **Error Handling**: Graceful handling of missing elements and failures
- 📊 **Detailed Logging**: Clear console output showing which strategies worked

## Technologies Used

- C# .NET 8.0
- Selenium WebDriver 4.15.0
- ChromeDriver
- XPath for element location

## Prerequisites

- .NET 8.0 SDK or later
- Google Chrome browser
- Visual Studio Code (recommended)

## Installation & Running

1. Clone this repository
2. Navigate to project directory
3. Install dependencies: `dotnet restore`
4. Build project: `dotnet build`
5. Run tests: `dotnet run`

## Test Target

This automation tests the CloudQA Practice Form at:
https://app.cloudqa.io/home/AutomationPracticeForm

## Project Structure
SeleniumFormTester/
├── Program.cs              # Main application code
├── SeleniumFormTester.csproj   # Project configuration
├── README.md               # This file
└── .gitignore             # Git ignore rules

## How It Works

The application uses multiple strategies to locate form elements:

1. **Semantic Attributes**: placeholder, type, etc.
2. **Label Association**: Finding inputs by their labels
3. **Name/ID Patterns**: Common naming conventions
4. **Position-based**: Fallback to element position
5. **Content Proximity**: Finding elements near relevant text

This approach ensures the automation continues working even if developers change element properties.
