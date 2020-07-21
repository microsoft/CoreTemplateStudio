# Core Template Studio

<img src="https://img.shields.io/badge/platform-linux--64%20%7C%20win--64%20%7C%20osx--64%20-lightgrey.svg" alt="Platforms Supported: MacOSX, Linux, Windows"/> <a href="https://www.repostatus.org/#active"><img src="https://www.repostatus.org/badges/latest/active.svg" alt="Project Status: Active – The project has reached a stable, usable state and is being actively developed." /></a> <a href="LICENSE.md"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License: We are using the MIT License"></a> <a href="CONTRIBUTING.md"><img src="https://img.shields.io/badge/PRs-Welcome-brightgreen.svg" alt="We are welcoming PRS!"></a>

Core Template Studio is a .NET Standard 2.0 project that handles all of the template synchronization and code generation (composition, generation + postaction execution) for **[WindowsTemplateStudio](https://github.com/Microsoft/WindowsTemplateStudio)** and **[WebTemplateStudio](https://github.com/Microsoft/WebTemplateStudio)**. The main project has two parts: a .NET Standard core and its corresponding Cli that exposes the core. There's also a tools folder containing helper tools as the WtsPackagingTool.

## Build Status

| Branch  |                                                                                                                Build Status                                                                                                                |
| :------ | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
| dev     |     [![Build Status](https://winappstudio.visualstudio.com/DefaultCollection/WTS/_apis/build/status/CoreTemplateStudio-%20CI?branchName=dev)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=156&branchName=dev)     |
| release | [![Build Status](https://winappstudio.visualstudio.com/DefaultCollection/WTS/_apis/build/status/CoreTemplateStudio-%20CI?branchName=release)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=156&branchName=release) |

## Branches
Our default branch is dev and all PR's should go to the dev branch. While there are no releases of CoreTS, we still have a release branch on CoreTS as this is where the submodules on Web Template Studio and Windows Template Studio point to. When adding new functionality or changing existing code make sure the changes are compatible with both Windows Template Studio and Web Template Studio. All changes have to be tested in both Windows Template Studio and Web Template Studio before being merged into the release branch.

## Documentation

- [Getting started with the codebase](./docs/getting-started-developers.md).

## Features

- _Synchronizes Templates_: The core has the ability to synchronize with templates, either in debug as a templates folder or in release as a .mstx file. It also builds and refreshes the template cache.
- _Provides information about available templates_: Upon synchronization, the core will provide information on the available project type, framework, page and feature templates.
- _Generate using Templates_: Using the available templates, the core does the composition, generation and postaction execution to create a C# or VB Project.
- _Telemetry_: Performs the majority of telemetry for Windows Template Studio including user selections and generation time.

## Limitations

- Currently .NET Standard does not support Package verification using PackageDigitalSignatureManager. This functionality was moved to the Utilities project (.NET framework) to be consumed from Windows Template Studio. The CLI (.NET Core 3.1) does not allow to download template packages from the CDN due to this limitation. Tracking issue is for adding this in once it is supported is #131. 

## Feedback, Requests and Roadmap

Please use [GitHub issues](https://github.com/Microsoft/CoreTemplateStudio/issues) for feedback, questions or comments.

If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/CoreTemplateStudio/issues) section as well. We would love to see what you are thinking.

This project is a shared project and will change based on the needs of WindowsTemplateStudio and WebTemplateStudio. You can check out WindowsTemplateStudio's [roadmap](https://github.com/microsoft/WindowsTemplateStudio/blob/dev/docs/roadmap.md) for more information. We're still working on a roadmap for WebTemplateStudio.

## Reporting Security Issues

Security issues and bugs should be reported privately, via email, to the Microsoft Security
Response Center (MSRC) at [secure@microsoft.com](mailto:secure@microsoft.com). You should
receive a response within 24 hours. If for some reason you do not, please follow up via
email to ensure we received your original message. Further information, including the
[MSRC PGP](https://technet.microsoft.com/en-us/security/dn606155) key, can be found in
the [Security TechCenter](https://technet.microsoft.com/en-us/security/default).

## Contributing

Do you want to contribute? We would love to have you help out. Here are our [contribution guidelines](CONTRIBUTING.md).

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
