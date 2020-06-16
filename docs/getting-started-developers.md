# Getting started with the generator codebase

This document will go over the structure and responsibilty of the projects contained in this repo. You should read this document if you are interested in [contributing](../CONTRIBUTING.md) to this repo.

## Repo Solutions

Under the [code/](../code/) folder, the repo has the following solution files:

- **Big.sln**: This is the solution which contains all the projects available, including test projects.
- **Core.sln**: This solution is focussed on the Core assembly. Use this solution when common core code is developed that requires no changes on the CLI.
- **Tools.sln** : This solution is focussed on the Tools contained in the repo. Use this solution to work on Tools as WtsPackagingTool.

## Inside the Code folder

Under the [code/](../code/) folder contents are organized using the following folders:
```layout

├── src
│  ├── CoreTemplateStudio 
│  │ ├── CoreTemplateStudio.Cli: Command line project that allows access to core, more details below
│  │ └── CoreTemplateStudio.Core: Core Project, in charge of managing template repository and generation, more details below
│  └── Utilities: CoreTS functionalities that (for now) require .net framework.
├── test
│ ├── CoreTemplateStudio.Cli.Test: Tests for cli project
│ ├── CoreTemplateStudio.Core.Test: Tests for core project
│ └── Utilities.Test: Tests for utilities project
└── tools: common tooling required for testing / validations / template packaging.
```
## Target Framework and Dependencies

CoreTS's target framework is .NET Standard 2.0. CoreTS CLI's target frameworkk is .NET Core 3.1.

For template cache and generation it relies on [dotnet templating](https://github.com/dotnet/templating).

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

The Cli project is in charge of providing an entry point to the core project for external callers. Communicating between the core and the caller is done via commands using CommandLineParser.

CoreTS Cli exposes the following commands:

### Sync Command: 

The Sync command builds the GenContext and synchronizes the templates available in the provided path. 

`
sync -p path
`

where

- path: 
 should point to the folder the .mstx file is located. 
 (If the cli is launched in debug mode parh should point to a local template's directory, to synchronize a LocalTemplateSource)

Sample usage: 

For release with .mstx in the same folder as cli:

`
sync -p .. 
`

For debug:

`
sync -p C:\Projects\WebTemplateStudio
`


### Get Project Types Command: 

Returns metadata info for all available project types for the current platform.

`
getprojecttypes
`

Sample usage: 

`
getprojecttypes
`


### Get Frameworks Command: 
Returns metadata info for all available frontend and backend frameworks for the current platform and the selected project type.

`
getframeworks -p projectType
`

where

- projectType: Name of the selected project type.

Sample usage: 

`
getframeworks -p FullStackWebApp
`

### Get Pages Command: 
Returns all page templates available for the selected project type, frontend and backend frameworks for the current platform.
Depending on the projectType, you can either pass only frontend or backendframework or both.

`
getpages -p projectType -f frontendFramework -b backendFramework
`

where:
- projectType: Name of the selected project type.
- frontendFramework: Name of the selected frontend framework.
- backendFramework: Name of the selected backend framework.

Sample usage: 

`
getpages -p FullStackWebApp -f React -b Node
`

### Get Features Command: 
Returns all page templates available for the selected project type, frontend and backend frameworks for the current platform.
Depending on the projectType, you can either pass only frontend or backendframework or both.

`
getfeatures -p projectType -f frontendFramework -b backendFramework
`

where:
- projectType: Name of the selected project type.
- frontendFramework: Name of the selected frontend framework.
- backendFramework: Name of the selected backend framework.

Sample usage: 

`
getfeatures -p FullStackWebApp -f React -b Node
`

### Get Services Command: 
Returns all services templates available for the selected project type, frontend and backend frameworks for the current platform.
Depending on the projectType, you can either pass only frontend or backendframework or both.

`
getservices -p projectType -f frontendFramework -b backendFramework
`

where:
- projectType: Name of the selected project type.
- frontendFramework: Name of the selected frontend framework.
- backendFramework: Name of the selected backend framework.

Sample usage: 

`
getservices -p FullStackWebApp -f React -b Node
`

### Get Testings Command: 
Returns all testings templates available for the selected project type, frontend and backend frameworks for the current platform.
Depending on the projectType, you can either pass only frontend or backendframework or both.

`
gettestings -p projectType -f frontendFramework -b backendFramework
`

where:
- projectType: Name of the selected project type.
- frontendFramework: Name of the selected frontend framework.
- backendFramework: Name of the selected backend framework.

Sample usage: 

`
gettestings -p FullStackWebApp -f React -b Node
`

### Get Layouts Command: 
Returns layout information for the selected project type, frontend and backend frameworks for the current platform.
Depending on the projectType, you can either pass only frontend or backendframework or both.

`
getlayouts -p projectType -f frontendFramework -b backendFramework
`

where:
- projectType: Name of the selected project type.
- frontendFramework: Name of the selected frontend framework.
- backendFramework: Name of the selected backend framework.

Sample usage: 

`
getlayouts -p FullStackWebApp -f React -b Node
`

### Generate Command: 
Performs project generation for the provided user selection.

`
generate -d userSelectionJsonData
`

where:
- userSelectionJsonData: Json with the userSelection

Sample usage: 

`
generate -d {"projectName":"testProject","genPath":"C:/Users/MyUser/projects","projectType":"FullStackWebApp","frontendFramework":"React","backendFramework":"Node","language":"Any","platform":"Web","homeName":"Test","pages":[{"name":"Blank","templateid":"wts.Page.React.Blank"},{"name":"Grid","templateid":"wts.Page.React.Grid"},{"name":"Master Detail","templateid":"wts.Page.React.MasterDetail"},{"name":"List","templateid":"wts.Page.React.List"}],"features":[]}
`

### Close Command: close
Closes the Cli 

`
close
`