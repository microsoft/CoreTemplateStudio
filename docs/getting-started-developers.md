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

The projects target framework is .NET Standard 2.0. For template cache and genration it relies on [dotnet templating](https://github.com/dotnet/templating).

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

- Providing an entry point to the core project for external callers. 
- Communicating between the core and the caller is done via commands.

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
getprojecttypes -p FullStackWebApp
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
getpages -p FullStackWebApp -f ReactJS -b NodeJS
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
getfeatures -p FullStackWebApp -f ReactJS -b NodeJS
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
getservices -p FullStackWebApp -f ReactJS -b NodeJS
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
gettestings -p FullStackWebApp -f ReactJS -b NodeJS
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
generate -d {"projectName":"testProject","genPath":"C:/Users/MyUser/projects","projectType":"FullStackWebApp","frontendFramework":"ReactJS","backendFramework":"NodeJS","language":"Any","platform":"Web","homeName":"Test","pages":[{"name":"Blank","templateid":"wts.Page.React.Blank"},{"name":"Grid","templateid":"wts.Page.React.Grid"},{"name":"Master Detail","templateid":"wts.Page.React.MasterDetail"},{"name":"List","templateid":"wts.Page.React.List"}],"features":[]}
`

### Close Command: close
Closes the Cli 

`
close
`