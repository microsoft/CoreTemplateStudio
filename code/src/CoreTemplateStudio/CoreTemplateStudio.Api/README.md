# **CoreTemplateStudio.Api**

This document outlines run instructions and documentation for the engine API, which serves as the link between the CoreTemplateStudio Engine and other applications.

## Getting Started

Open the *CoreTemplateStudio.Api.Sln* solution with Visual Studio 2017. Press *F5* to run in debug mode or *Ctrl + F5* to run without debugger.
Alternatively, you can also select *Run* on the menu bar from Visual Studio 2017. This will run the application on *Port 5000* if you selected
*CoreTemplateStudio.Api* from the run dropdown or *Port 50491* if you selected *IIS express* (default). Try the endpoints with cURL or Postman.

## OpenAPI/Endpoint documentation

This repository contains an openapi spec document called [openapi.yaml](https://github.com/Microsoft/CoreTemplateStudio/blob/dev/code/src/CoreTemplateStudio/CoreTemplateStudio.Api/openapi.yaml).
You can either install a VSCode openAPI viewer from extension marketplace or use an online viewer to view and edit this document.
[Swagger online editor](https://editor.swagger.io/) is a free to use open source tool for OpenAPI docs, import the doc file from the menu bar on top left.
This editor is also available to download and run locally.
