# Core Template Studio

<img src="https://img.shields.io/badge/platform-linux--64%20%7C%20win--64%20%7C%20osx--64%20-lightgrey.svg" alt="Platforms Supported: MacOSX, Linux, Windows"/> <a href="https://www.repostatus.org/#active"><img src="https://www.repostatus.org/badges/latest/active.svg" alt="Project Status: Active – The project has reached a stable, usable state and is being actively developed." /></a> <a href="LICENSE.md"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License: We are using the MIT License"></a> <a href="CONTRIBUTING.md"><img src="https://img.shields.io/badge/PRs-Welcome-brightgreen.svg" alt="We are welcoming PRS!"></a>

Core Template Studio is a .NET Standard 2.1 project that handles all of the template merging and code generation for **[WindowsTemplateStudio](https://github.com/Microsoft/WindowsTemplateStudio)**. The project has two parts: a .NET Standard core and its corresponding API that exposes the core for non C# Projects.

## Build Status

| Branch  |                                                                                                                                 Build Status                                                                                                                                  |
| :------ | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
| dev     |     [![Build Status](https://microsoftgarage.visualstudio.com/Intern%20GitHub/_apis/build/status/Web-Template-Studio-DevCI/CoreTemplateStudio-CI?branchName=dev)](https://microsoftgarage.visualstudio.com/Intern%20GitHub/_build/latest?definitionId=276&branchName=dev)     |
| staging | [![Build Status](https://microsoftgarage.visualstudio.com/Intern%20GitHub/_apis/build/status/Web-Template-Studio-DevCI/CoreTemplateStudio-CI?branchName=staging)](https://microsoftgarage.visualstudio.com/Intern%20GitHub/_build/latest?definitionId=276&branchName=staging) |
| master  |  [![Build Status](https://microsoftgarage.visualstudio.com/Intern%20GitHub/_apis/build/status/Web-Template-Studio-DevCI/CoreTemplateStudio-CI?branchName=master)](https://microsoftgarage.visualstudio.com/Intern%20GitHub/_build/latest?definitionId=276&branchName=master)  |

# Documentation

- [Getting started with the codebase](./docs/getting-started-developers.md).

## Features

- _Synchronizes Templates_: The core has the ability to synchronize with templates, either in debug as a templates folder or in release as a .mstx file. It also builds and refreshes the template cache.
- _Provides information about available templates_: Upon synchronization, the core will provide information on the available project type, framework, page and feature templates.
- _Generate using Templates_: Using the available templates, the core can do compositions and postactions to create a C# or VB Project.
- _Telemetry_: Performs the majority of telemetry for Windows Template Studio including user selections and generation time.

## Limitations

- Currently does not support Package verification from the CDN.

## Feedback, Requests and Roadmap

Please use [GitHub issues](https://github.com/Microsoft/CoreTemplateStudio/issues) for feedback, questions or comments.

If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/CoreTemplateStudio/issues) section as well. We would love to see what you are thinking.

This project is a shared project and will change based on the needs of WindowsTemplateStudio. You can check out their [roadmap](https://github.com/Microsoft/WindowsTemplateStudio/tree/dev/docs/roadmap.md) for more information.

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
