# Contributing to Core Template Studio

**Core Template Studio** handles all of the template merging and code generation for **[WindowsTemplateStudio](https://github.com/Microsoft/WindowsTemplateStudio)**.

Pull Requests must be done against the **[dev branch](https://github.com/Microsoft/CoreTemplateStudio/tree/dev)**.

## Before you begin

While we're grateful for any and all contributions, we don't want you to waste anyone's time. Please consider the following points before you start working on any contribution.

- Please comment on an issue to let us know you're interested in working on something before you start the work. Not only does this avoid multiple people unexpectedly working on the same thing at the same time but it enables us to make sure everyone is clear on what should be done to implement any new functionality. It's less work for everyone, in the long run, to establish this up front.
- Get familiar with the automated tests that are part of the project. With so many possible combinations of output, it's impossible to verify everything manually. You will need to make sure they all pass.

## A good pull request

Every contribution has to come with:

- Before starting coding, **you must open an issue** and start discussing with the community to see if the idea/feature is interesting enough.
- A documentation page in the [documentation folder](https://github.com/Microsoft/CoreTemplateStudio/tree/dev/docs).
- Unit tests (If applicable, or an explanation why they're not).
- Your code shouldn't break cross platform compatibility. If adding a feature through Linux or macOS be sure that it doesn't break Windows 10, and vice versa.
- You've run all existing tests to make sure you've not broken anything.
- PR has to target dev branch.

PR has to be validated by at least three (3) core members before being merged.

## General rules

- DO NOT require that users perform any extensive initialization before they can start programming basic scenarios.
- DO NOT use regions. DO use partial classes instead.
- DO NOT seal controls.
- DO NOT use verbs that are not already used like fetch.
- DO NOT return true or false to give sucess status. Throw exceptions if there was a failure.
- DO provide good defaults for all values associated with parameters, options, etc.
- DO ensure that APIs are intuitive and can be successfully used in basic scenarios without referring to the reference documentation.
- DO communicate incorrect usage of APIs as soon as possible.
- DO design an API by writing code samples for the main scenarios. Only then, you define the object model that supports those code samples.
- DO declare static dependency properties at the top of their file.
- DO use extension methods over static methods where possible.
- DO use verbs like GET.

## Naming conventions

- We are following the coding guidelines of [.NET Core coding style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md).

## Documentation

- DO NOT expect that your code is so well designed that it needs no documentation. No code is that intuitive.
- DO provide great documentation with all new features and code.
- DO use readable and self-documenting identifier names.
- DO use consistent naming and terminology.
- DO provide strongly typed APIs.
- DO use verbose identifier names.

## Files and folders

- DO associate no more than one class per file.
- DO use folders to group classes based on features.
