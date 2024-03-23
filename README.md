# Trade Cloner WPF Application

The Trade Cloner WPF application is a real-time trading tool built using the Windows Presentation Foundation (WPF) framework in C#. It allows users to replicate trades between two trading accounts, specifically for trading Bank Nifty options. This application interacts with the KiteConnect API for trading operations.

## Features
- **User Management:** Users can log in to their parent and child trading accounts by providing API keys, secrets, and access tokens.
- **Account Operations:** The application establishes connections to the parent and child accounts, subscribes to Bank Nifty options, and monitors order updates.
- **Trade Cloning:** When a Bank Nifty option order is executed in the parent account, the application automatically replicates the same order in the child account.

## Technologies Used
- C#
- Windows Presentation Foundation (WPF)
- KiteConnect API

## How to Use
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Replace placeholder API keys, secrets, and access tokens with your actual credentials.
4. Build and run the Trade Cloner WPF application.
5. Log in to your parent and child trading accounts.
6. Subscribe to Bank Nifty options and monitor order updates.

## Requirements
- Visual Studio (recommended version: Visual Studio 2019)
- .NET Framework

## Contribution
Contributions are welcome! If you have any suggestions, bug fixes, or improvements, feel free to open an issue or submit a pull request.
