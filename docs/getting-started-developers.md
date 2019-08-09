# Getting started with the generator codebase

This document will go over how to get set up, and the file structure of the project. Which is useful information if you are interested in [contributing](../CONTRIBUTING.md).

## Repo Solutions

Under the [code/src/CoreTemplateStudio](../code/src/CoreTemplateStudio) folder, the repo has different solutions to aid developers get focused on certain development areas:

- **CoreTemplateStudio.Core.sln**: This solution contains the Core assembly. Use this solution when common core code is developed.
- **CoreTemplateStudi.Cli.sln**: This solution contains the Cli and Core assemblies. Use this solution when the Cli code is developed.

Under the [code/test/](../code/test) folder, the repo has additional solutions related to tests:

- **CoreTemplateStudio.Core.Test.sln**: This solution contains the Core assembly + tests: Use this solution when ensuring that your newly developed code works and doesn't break anything, or if you are creating new tests for the core.

- **CoreTemplateStudio.Cli.Test.sln**: This solution contains the Cli + Core assembly + tests: Use this solution when ensuring that your newly developed code works and doesn't break anything, or if you are creating new tests for the Cli.

## Dependencies

This project depends on the .NET Core 2.1 SDK with ASP.NET Core, and nuget packages from [dotnet templating](https://github.com/dotnet/templating).

The nuget packages can be installed by doing `dotnet restore`. If you are using Visual Studio to develop, this should be done automatically. 

## Core project

The core project is in charge of:

- Checking for new template packages, download, extract and verify template packages and building up and refreshing the templates cache.
- Providing information about available project type, framework, page and feature templates.
- Composition of the generation queue.
- Generation and postaction execution.

During the generation the GenContext class provides access to:

- The Toolbox, with the Templates Repository and the Shell class
- The current context, with information about:
  - The current project name
  - The generation output path
    - Project Path for New Project generations
    - Temporary Path for New Item generations using Right Click
  - The destination path (project Path for both New Project and New Item generations)
  - ProjectInfo: Information about projects to be added to the solution and items and references that will be added to the project once the generation finishes

## Cli Project

The Cli project is in charge of:

- Communicating between the core and the caller given queries or payloads.
- Providing a more abstract layer between the core and the caller.