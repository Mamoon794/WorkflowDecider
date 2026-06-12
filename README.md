# AI-Powered Workflow Orchestrator API

A .NET 8 Minimal Web API that leverages Artificial Intelligence to translate natural language business goals into structured, executable workflow steps. 

This project demonstrates core backend engineering principles, database integration using Entity Framework Core, and the practical application of AI as a force multiplier in enterprise workflow systems.

## Features

- **AI Orchestration:** Integrates with the Gemini LLM via `HttpClient` to dynamically break down abstract goals (e.g., "Onboard a new enterprise client") into sequential workflow tasks.
- **Modern .NET Architecture:** Built using C# and .NET 8 Minimal APIs for lightweight, high-performance endpoint routing.
- **Relational Data Modeling:** Uses Entity Framework (EF) Core to manage a one-to-many relationship between parent `Workflows` and child `WorkflowSteps`.
- **Rapid Prototyping:** Configured with SQLite for immediate local execution without requiring heavy database installations.

## Tech Stack

- **Framework:** .NET 8, C#
- **Database:** SQLite, Entity Framework Core
- **AI Integration:** Google Gemini API
- **Architecture:** Minimal APIs, Dependency Injection, Interface-driven design

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- A [Gemini API Key](https://aistudio.google.com/)

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone [https://github.com/yourusername/WorkflowOrchestrator.git](https://github.com/yourusername/WorkflowOrchestrator.git)
   cd WorkflowOrchestrator
