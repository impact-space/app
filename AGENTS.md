AGENTS.md

This file provides guidance for Codex agents (and other AI assistants) working with this codebase. It defines the environment, conventions, and commands that ensure any automated agent can set up, build, test, and contribute code reliably.

1. Purpose

Describe how to configure the development environment.

List key commands and workflows.

Define coding, testing, and PR conventions.

2. Repository Structure

/                      # Root of the repository
├─ aspnet-core/        # ASP.NET Core solution and projects
│   ├─ ImpactSpace.sln  # Solution file
│   ├─ src/             # Application projects (e.g. Core, API, Blazor)
│   └─ tests/           # xUnit/NUnit test projects
├─ react-native/       # React Native mobile app
├─ static-web/         # Static web assets
├─ etc/                # Configuration (docker-compose, infra)
├─ scripts/            # Setup and utility scripts (e.g., setup-environment.sh)
├─ AGENTS.md           # Agent guidance (this file)
└─ README.md           # Human-facing project overview

3. Environment Configuration

Setup script

./scripts/setup-environment.sh

Major tools installed

.NET 9 SDK (via APT on Ubuntu 20.04/22.04 or dotnet-install.sh fallback)

Node.js 18.x (via NodeSource)

Python 3 with Poetry-managed venv or .venv (as defined by pyproject.toml or requirements.txt)

ABP CLI (dotnet tool install -g Volo.Abp.Cli)

Environment variables (persisted in ~/.bashrc):

export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$HOME/.dotnet:$HOME/.dotnet/tools:$HOME/.local/bin"

4. Common Commands

4.1 Build & Test (ASP.NET Core)

# From repository root:
cd aspnet-core
# Restore NuGet packages and build solution
dotnet restore
dotnet build
# Run all test projects (xUnit/NUnit)
dotnet test

4.2 JavaScript / TypeScript

cd react-native    # if working on mobile app
pnpm install
pnpm run lint      # ESLint + Prettier
pnpm run test      # JS/TS tests

4.3 Python (if present)

# From project root:
poetry install         # creates .venv
poetry run pyright     # type checking
poetry run pytest      # run Python tests

4.4 ABP Framework Workflows

# From aspnet-core folder:
cd aspnet-core
# Use ABP CLI to create a new solution based on best practices:
abp new YourSolutionName -t app --ui mvc --database sqlite
# To add a new module:
abp generate module YourModuleName
# To add an entity to a module:
abp generate crud YourModuleName/Entities/YourEntity
# To run database migrations:
cd src/YourProject.DbMigrator
dotnet run
# To start the host application:
cd src/YourProject.HttpApi.Host
dotnet run

Key ABP CLI commands:

abp help: list all commands

abp update: update packages and templates to latest ABP version

abp lint: check code adherence to ABP guidelines

Agents should prefer ABP’s layered architecture (Domain, Application, HTTP API, Host, EntityFrameworkCore modules), follow the module naming conventions (YourProjectName.ModuleName), and keep shared DTOs in .Shared projects.

5. Coding Conventions

C#: follow the Microsoft .NET naming conventions. Use dotnet format for automated formatting.

JavaScript/TypeScript: use ESLint + Prettier. Prefer functional components and hooks for React, if used.

Python: follow PEP8. Use Black for formatting and Flake8 for linting.

6. Testing Requirements

All new code must include appropriate unit tests.

Use existing test frameworks: xUnit for .NET, pytest for Python, and project-standard JS testing framework.

Ensure tests pass locally before committing.

7. Pull Request Guidelines

Branch naming: feature/<short-description> or fix/<short-description>.

Commit messages: use Conventional Commits (e.g. feat: add user login endpoint).

PR title: concise summary (max 50 chars).

PR description:

What changed and why

How to test

Issue link (if any)

8. Agent Configuration Hierarchy

Codex and other agents will merge instructions from:

~/.codex/AGENTS.md (global)

AGENTS.md at repo root (this file)

AGENTS.md in any subdirectory (overrides root)

To disable project-level docs, set:

export CODEX_DISABLE_PROJECT_DOC=1

End of AGENTS.md
