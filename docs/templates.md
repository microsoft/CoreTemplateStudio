# Understanding the Templates

Templates are used to generate the code in [Windows Template Studio](http:aka.ms/wts) and [Web Template Studio](aka.ms/webts).

This documents explains how templates are defined, composed and generated.

## Template Authoring

### The MxN Issue

Windows Template Studio and Web Template Studio both work as a shopping basket where a developer can choose one of the available Frameworks, one of the available projects Project Types, and then add the Pages and Features wanted for the target application. This leads to a complexity issue. Consider we have 3 frameworks (Fx) and 3 project types (Pj), then we will have 9 combinations, that is *Fx x Pj* app configurations. Now, consider we want to have 6 different types of Pages (P), all supported among the different app configurations, so we will need to maintain 9x6 = 54 pages, that is, *Fx x Pj x P* pages, with basically the same code. The same happens for Features (F), considering 6 types of features, we will have 9x6 = 54 features to maintain.

Creating templates linearly is unmanageable, we would require to maintain Fx x Pj x (P + F) *[3 x 3 x (6 + 6) = 108]* different templates just to be able to combine all together under our preferences, but if the page types and/or features grow, then the number templates to maintain grow faster. This is what we call **The MxN issue**.

To avoid the MxN issue, we have designed the Templates as composable items, starting from the template definition from [dotnet Template Engine](https://github.com/dotnet/templating) and extending it to allow to define compositions and post-actions to reduce the template proliferation. The drawback is that the generation becomes more complex, but infinitely more maintainable in the long term.

## Templates authoring principles

We follow these principles for template authoring:

1. **Templates are composable**. In general, the templates should be composable with the existing frameworks and project types. That is, a certain page template should be available to be generated no matter the target framework and project type.
1. **Reduce code duplication as much as possible**. As far as possible, avoid to have templates with the same code for different frameworks and/or project types.
1. **Balance between maintainability and complexity.** Avoiding code duplication benefits the maintainability in long term and is always a benefit. At the same time, reducing code duplication leads to more complexity to handle the composition and the required actions to finish a proper generation. We need to ensure that advanced developers are able to contribute authoring templates so, we need to ensure the right balance between code reusability and templates complexity.
1. **Output takes priority over template simplicity.** When authoring templates it can be tempting to make compromises over the generated project output because it makes the templates simpler. Avoid this. The final output in the generated project is the most important thing and it is always better to put more effort into template construction if it leads to higher quality code being generated.

## Anatomy of templates

A template is just code (some source files and folders structure) with some metadata which drives how the code is generated. The template metadata contains informational data (name, description, licensing, remarks, programming language, type, guids, etc.) as well as data used to replace matching text in the source content by the actual values (think of a class name). The templates definition is based on [dotnet Template Engine](https://github.com/dotnet/templating), you can visit their repo for a deep understanding on how the Template Engine works.

A template is defined by the following elements:

- **Metadata**: a json file within the *".template.config"* folder  information which defines the template and its properties. The metadata includes the replacements to be done.
- **Folder Structure**: A folder structure that will be maintained after the generation is done.
- **Files**: Text files, basically, the source code, where replacements are made.

The metadata drives how the generation is done, let's see a template content sample:

``` layout

├───.template.config
│       description.md      //Rich template description in markdown. Displayed in the wizard.
│       icon.xaml           //SVG XAML definition for the template icon (.png or .jpg are accepted as well).
│       template.json       //Template definition json file
│
├───Strings
│   └───en-us
│           Resources_postaction.resw //Post-Action to be applied after main generation of this template.
│
├───ViewModels
│       BlankViewViewModel.cs         //Source file
│
└───Views
        BlankViewPage.xaml            //Source file
        BlankViewPage.xaml.cs         //Source file
```

If we generate this template using "MyTest" as page name, the result will be as follows:

``` layout

├───Strings
│   └───en-us
│           Resources.resw
│
├───ViewModels
│       MyTestViewModel.cs
│
└───Views
        MyTestPage.xaml
        MyTestPage.xaml.cs
```

You can observe that the folder structure is maintained but in the source files the "BlankView" word has been replaced by "MyTest" (the actual value for the *sourceName* parameter).

The replacements are done based on the configuration established in the `template.json` file. Let's have a look to it:

``` json
{
  "$schema": "http://json.schemastore.org/template",
  "author": "Microsoft",
  "classifications": [
    "Universal"
  ],
  "name": "Blank",                                  // This is the displayed name in the wizard.
  "shortName": "Blank",
  "groupIdentity": "wts.Page.Blank",                // Used for filtering and grouping in the wizard
  "identity": "wts.Page.Blank",                     // Must be unique
  "description": "This is the most basic page.",    // This is the short description displayed in the wizard.
  "tags": {                                         // Tags are used to filter and handled the composition
    "language": "C#",
    "type": "item",                                 // Template output type (project or item)
    "wts.type": "page",                             // Template type (project, page, feature, service, testing or composition)
    "wts.frontendframework": "MVVMBasic|MVVMLight", // Frameworks where this template can be used.
    "wts.projecttype": "",
    "wts.platform": "Uwp",                          // Platform where this template can be used
    "wts.version": "1.0.0",
    "wts.displayOrder": "1",                        // This tag is used to order the templates in the wizard.
    "wts.rightClickEnabled":"true",                 // If set to 'true' then this feature or page is available from right click on an existing project.
    "wts.isHidden": "false",                        // If set to 'true' then not shown in the wizard. Used for dependencies that can't be selected on their own.
    "wts.outputToParent": "true",                   // If set to 'true' then this will be generated one folder above the usual output folder.
    "wts.casing.sourceName":"kebab|camel|pascal",   // Allows to add casing variations from templates sourceName to parameters ( corresponding parameter name will be wts.sourceName.casing.kebab)
    "wts.casing.rootNamespace":"kebab|camel|pascal", // Allows to add casing variations from templates sourceName to parameters ( corresponding parameter name will be wts.rootNamespace.casing.kebab)
    "wts.licenses":"[License name](License url)",
    "wts.dependencies": "wts.Page.Settings",        // Template id's this template depends on
    "wts.requirements" : "",                        // Group identities this templates needs to be added to the user selection before being able to add this template (this is used instead of dependencies if the user has to choose between various options)
    "wts.exclusions" : "",                          // Template ids that are incompatible with this template
    "wts.group": "",                                // Wizard group name the template will be displayed in
    "wts.genGroup":"",                              // Generation group the template is contained in
    "wts.isGroupExclusiveSelection": "",            // Determines if the templates in the same group are mutually exclusive or not
    "wts.defaultInstance":"",                       // Default name for template
    "wts.multipleInstance":"",                      // If set to 'true' various instances of this template can be selected
    "wts.requiredSdks":""                           // Determines if the templates needs any required Sdks to be installed on the machine
  },
  "sourceName": "BlankView",                        // The generation engine will replace any occurrence of "BlankView" by the parameter provided in the source file name.
  "preferNameDirectory": true,
  "PrimaryOutputs": [                               // The primary outputs are the list of items that are returned to the caller after the generation. Use Param_ProjectName for folders containing the projectname
    {
      "path": "Views/BlankViewPage.xaml"
    },
    {
      "path": "Views/BlankViewPage.xaml.cs"
    },
    {
      "path": "ViewModels/BlankViewViewModel.cs"
    }
  ],
  "symbols": {                                      // Symbols define a collection of replacements to be done while generating.
     "wts.projectName": {
      "type": "parameter",
      "replaces": "Param_ProjectName",
      "fileRename": "Param_ProjectName"
    },
    "rootNamespace": {
      "type": "parameter",
      "replaces": "RootNamespace"                   // Each instance of "RootNamespace" in the source files will be replaced by the actual value passed in the "rootNamespace" parameter.
    },
    "itemNamespace": {
      "type": "parameter",
      "replaces": "ItemNamespace"
    },
    "baseclass": {
      "type": "parameter",
      "replaces": "System.ComponentModel.INotifyPropertyChanged"
    },
    "setter": {
      "type": "parameter",
      "replaces": "Set"
    }
  }
}
```

Further documentation on the contents of the `template.json` file can be found on the [dotnet templating wiki](https://github.com/dotnet/templating/wiki/%22Runnable-Project%22-Templates#identity-optional)

### Template Layouts

Project templates can define a default layout of pages to be considered in the wizard. To do so, you need to add a `Layout.json` file within the `.template.config` folder.

By using template layouts, you can determine what pages are automatically added to a certain project type and if those pages are mandatory or can be removed. In other words, layout definition provides a way to pre-configure pages associated to a certain project type.

Layout.json

``` json
[
    {
        "name": "Main",
        "templateGroupIdentity": "wts.Page.Blank",
        "readonly": "true",
        "projecttype": "Blank|SplitView|TabbedNav|MenuBar"
    }
]
```

### Export Parameters

A template can define an "export parameter" that will be handled by the `Composer` by extracting the replacement parameter value from one template) and providing it as parameter to the following templates. Here is a sample of how an export parameter is defined:

``` json
  "tags": {
    "language": "C#",
    "type": "item",
    "wts.type": "composition",
    "wts.version": "1.0.0",
    "wts.compositionFilter": "$framework == MVVMBasic & wts.type==page",
    "wts.export.baseclass": "Observable",
    "wts.export.setter": "Set"
  },
```

This template is defining two export parameters **baseclass** and **setter**. Those parameters will be provided to other templates where they will be used as the values for symbol replacements.

### Filtering supported templates

It's possible to filter the displayed templates based on installed Visual Studio workloads.
This is done by adding the tag `wts.requiredVsWorkload` and specifying the ID of the required workload.
This tag is optional. If specified, the template will be displayed as disabled if the required workload is not installed.
This tag cannot be used with frameworks or project types.

## Composable Templates

As we already have mentioned, templates can be composed to maximize the code reusability. Consider, for example, the Blank page template, the page's source code will remain the same no matter the project type it is used in. Moreover, there will be very few changes in the page source code depending on which framework we rely on. The idea behind having composable templates is to reuse as much as possible the existing code for a certain page or feature no matter the project type or framework used.

Creating composable templates is like when you are developing software and try to generalize something; it fits within the 80-20 rule, meaning that the 80% of the code is common among the callers and easy to be generalized, but the 20% have more dependencies, specific details, etc. and, by the way, it is more complex to be generalized. Considering this, we have two groups of templates in the repository:

1. **Standard templates**: *the 80 part*, these templates are the common part of the source code, corresponding with the shared source code for projects, pages and features. This templates live in the `Projects`, `Pages`, `Features`, `Services` and `Testing` folders of our Templates repository. Through the wizard, a user can select which project type, which pages and which features wants, those selections can be shown as a user adding items to a "generation basket".
1. **Composition templates**: *the 20 part*, these templates are thought to include the specific details required by a concrete template (a page or feature) which is going to be generated in a certain context. The context is determined by the combination of project type and framework selected by the user. Required composition templates are added to the "generation basket" automatically by the `Composer`. The composition templates live in the project `_composition` folder of the Templates repository.

The structure of files and folders within the `_comp` folder is just for organization, to exactly determine which *composition templates* are required to be added to the generation basket, the `Composer` evaluates all the templates available in the `_comp` folder, applying the **composition filter** defined in the `template.json` file (tag `wts.compositionFilter`). All the templates with composition filters resulting in positive matches are added to the generation basket. The following is a sample of composition filter.

``` json
  "tags": {
    "language": "C#",
    "type": "item",
    "wts.type": "composition",
    "wts.platform": "Uwp",
    "wts.version": "1.0.0",
    "wts.compositionFilter": "$framework == CodeBehind|MVVMBasic & identity == wts.Proj.Blank",
    "wts.compositionOrder" : "1"
 },
```

In this case, the template which have this configuration will be added to the generation basket when the following conditions are met:

- The selected framework for the current generation is CodeBehind OR MVVMBasic
- There is a template within the generation basket whose `identity` property is "wts.Proj.Blank".

In other words, this template is designed to be added to the generation basket when we are generating a Blank Project Type with the CodeBehind or MVVMBasic framework.
The wts.compositionOrder can be used to establish the order in which of composition templates are generated where necessary.

We have a basic pseudo-language to define the composition filters. The composition filters must match the following structure:

```pseudo

  <operand field> <operator> <literal> [& <operand field> <operator> <literal options>[...]]

Where

- <operand field> := <queryable property> | <context parameter>
- <literal> := <literal> [|<literal>]
- <queryable property> -> template configuration property (`template.json`) among the following:
  - `name`
  - `identity`
  - `groupIdentity`
  - Any defined tag, i.e `language`, `type`, `language`, `wts.framework`, etc.
- <operators>
  - == -> Equals Equality
  - != -> Not equals
- <context parameter>
  - $framework -> current generation framework.
  - $projectType -> current generation project type.
  - $page -> current selection includes a page with the specified Id.
  - $feature -> current selection includes a feature with the specified Id.

```

Finally, all the templates (*Standard* and *Composition*) are generated using the [dotnet Template Engine](https://github.com/dotnet/templating) standard generation. The standard generation does not support merging code from multiple files to one. For this, we need to take advantage of another mechanism: **Post-Actions**.

## Post Actions

Post-Actions are designed to complement the standard generation enabling certain actions to be executed over the generated files once the generation is done.

Currently we support the following types of [Post-Actions](../code/src/Core/PostActions):
- Defined by template output type:
  - **Add Item To Project**: this post-action is executed for templates with outputtype item to add the "PrimaryOutputs" to the target Visual Studio project (.csproj). The "PrimaryOutputs" are defined in the template.json file. 
  - **Add Project To Solution**: this post-action is executed for templates with outputtype project to add the a generated project to the current Visual Studio solution.

- Defined by the template:
  - **Add Reference To Project** this post-action is executed to add a reference from one project to another project.
  - **Add Nuget Reference To Project**: this post-action is executed to add a NuGet reference to the project.
  - **Add SDK Reference To Project**: this post-action is executed to add a SDK reference to the project.
  - **Add Json Dictionary Item**: this post-action is executed to add an item to an existing json dictionary in a json file.
  - **Generate Test Certificate**: generate the test certificate for the UWP application and configure it in the application manifest.

- Other postactions:
  - **Merge**: merges the source code from one file into another. This Post-Action requires a special (_postaction) configuration in the templates files.
  - **SearchAndReplace**: searches for the source code defined in the postaction file and replaces it with the specified code. This Post-Action requires a special (_searchreplace) configuration in the templates files.
  - **Sort Namespaces**: this post action re-orders the `using` statements at the top of the generated C# source files and the `import` statements in VB files.
  * **Set Default Solution Configuration**: sets the default solution configuration in the Visual Studio sln file. With this post-action we change the default solution configuration from Debug|ARM to Debug|x86.

### Merge Post-Action

If a template includes source files with the **_postaction** suffix, the Post-Action engine will process these files at the end of generation of that template.

For example, consider a SplitView project type with MVVM Basic framework, and adding several pages to the project. At the end, all the pages must be registered in the navigation and added to the navigation menu, some of the final generated code will look like:

```xml

//MVVM Basic, ShellPage.xaml
...
        <NavigationView.MenuItems>
            <!--
              TODO WTS: Change the symbols for each item as appropriate for your app
              More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
              Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/projectTypes/navigationpane.md
              Edit String/en-US/Resources.resw: Add a menu item title for each page
            -->
            <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="views:MainPage" />
            <NavigationViewItem x:Uid="Shell_Map" Icon="Document" helpers:NavHelper.NavigateTo="views:MapPage" />
            <NavigationViewItem x:Uid="Shell_MasterDetail" Icon="Document" helpers:NavHelper.NavigateTo="views:MasterDetailPage" />
            <NavigationViewItem x:Uid="Shell_WebView" Icon="Document" helpers:NavHelper.NavigateTo="views:WebViewPage" />
            <NavigationViewItem x:Uid="Shell_Tabbed" Icon="Document" helpers:NavHelper.NavigateTo="views:TabbedPage" />
        </NavigationView.MenuItems>
...

```

During the generation, each page must add the required code to register itself in the *navigation items*. To achieve this, we rely on the Merge Post-Action to identify files that must be merged to generate the code above. Let see the details of the composition template defined for that purpose.

The `template.json` is defined as follows:

```json
{
  "author": "Microsoft",
  "classifications": [
    "Universal"
  ],
  "name": "MVVMBasic.Project.SplitView.AddNavigationViewItem",
  "tags": {
    "language": "C#",
    "type": "item",
    "wts.type": "composition",
    "wts.platform" : "Uwp",
    "wts.version": "1.0.0",
    "wts.compositionFilter": "wts.type == page & identity != wts.Page.Settings & $framework == MVVMBasic & $projectType == SplitView"
  },

```

As you can see in the composition filter, this template will be applied when the context framework is *MVVMBasic* and the project type is *SplitView* and there is a template in the generation basket with the type equals to page and identity not equal to *wts.Page.Settings*

Here is the template layout:

```layout

├───.template.config
│       template.json
│
└───ViewModels
        ShellPage_postaction.xaml //This indicates that the content of this file must be handled by the Merge Post-Action

```

Here is the content of the ShellPage_postaction.xaml:

```xml

<Page
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">

    <NavigationView
        Background="{ThemeResource SystemControlBackgroundAltHighBrush}">
        <NavigationView.MenuItems>
            <!--^^-->
            <!--{[{-->
            <NavigationViewItem x:Uid="Shell_wts.ItemName" Icon="Document" helpers:NavHelper.NavigateTo="views:wts.ItemNamePage" />
            <!--}]}-->
        </NavigationView.MenuItems>
    </NavigationView>
</Page>

```

The merge post action will do the following:

1. Locate a file called "ShellPage_postaction.xaml" within the generated code.
1. Using a basic source code matching, the post-action will locate content in the `_postaction` file that is not included in the `ShellPage.xaml` file and will insert it in the correct place. In this case:
    - Locate the page tag
    - Then a NavigationView tag
    - Then the NavigationView.MenuItems tag
    - The symbols `//^^` indicates that the merge must be done at the end, just before the closing `</NavigationView.MenuItems>`, without this directive the line would be inserted just below the opening `<NavigationView.MenuItems>`.
1. Once the place to insert the code has been found, the content contained between `{[{` and `}]}` is added in to the original source file.
1. If any of the above directives are not found the merge is aborted and an merge failure is reported.

### Global Merge Post-Action

The global merge postactions work as the normal merge postaction, with the only difference that they are executed once the generation is finished.
You have to use global postactions whenever you need to include changes from various elements into one item generated at the same time.
An example is the `BackgroundTaskService`.
We use the same strategy to integrate methods from Chart and Grid Page into the SampleData Service from right click.

The format for global postactions is `<DestinationFileName>$<FeatureName>_gpostaction.<DestinationFileExtension>` (for example: BackgroundTaskService$BackgroundTaskFeature_gpostaction.cs).
This allows generation of 1 gpostaction file per BackgroundTask selected and merge of all files once the generation has finished.

### Merges Directives

There are different merge directives to drive the code merging. Currently:

- MacroBeforeMode `//^^`: Insert before the next match, instead of after the last match
- MacroStartGroup `//{[{` and MarcoEndGroup `//}]}`: The content between `{[{` and `}]}` is inserted. You can use `/*{[{*/` and `/*}]}*/` do do inline additions.
- MacroStartDelete `//{--{` and MacroEndDelete = `//}--}`: The content between the directives will be removed if it exists within the merge target. If the content does not exist (or has already been deleted as part of merging another file) this will be silently ignored.
- MacroStartDocumentation `//{**` and MacroEndDocumentation `//**}`: The content between `{**` and `**}` is not inserted but shown in the _postaction file. This can be used give the user feedback about was the postaction intended to do when the postaction fails or when integrating right click output manually.
- MacroStartOptionalContext `{??{` and MacroEndOptionalContext `}??}`: The content between `{??{` and `}??}` is optional, if the line is not found the next line is taken as context line.

_The above merge directives all use the C# comment form (`//`) but if included in a VB file should use the VB equivalent (`'`). For xml files (xaml, appxmanifest, resw, resx, config) use (`<!-- -->`).

### Merge Resource Dictionary PostActions

When the _postaction file contains a resource dictionary instead of the *basic* merge postaction a resource dictionary postaction is executed.
This postaction does not work with directives but based on the x:keys contained in the source and _postaction files.

The postaction works in three steps:
1. Locate the source resource dictionary file. (Imagine the postaction file is called Styles/Button_postaction.xaml, the source file would be Styles/Button.xaml)
2. If the file is not found the whole resource dictionary contained in the postaction file is copied to the source file.
3. If the file is found, each element from _postaction file is copied if not already there. In case the key is already defined in the source resource dictionary and the elements are different, a warning is shown.

## Template Name Validation
Name validation rules vary among different platforms and may change with the addition of new templates. Each platforms root template directory contains platform specific name validation configuration files.

Theese config files allow the addition and configuration of different project and item name validators that are used to infer and validate template and project names both in the Wizard and before Generation.

The two files allow to configure the following naming rules:

- Project name (configured in projectNameValidation.config.json):
  - validateEmptyNames: boolean that indicates if empty name validation should be applied. The EmptyNameValidator ensures that the suggested name is not empty.
  - validateExistingNames: boolean that indicates if exisiting name validation should be applied. This is done by checking if a folder with the suggested project name in the suggested path exists. The FolderNameValidator checks the suggested project name against existings project names.
  - reservedNames: defines the reserved names that cannot be used as project names. The ReservedNamesValidator checks the suggested project name against the reserved names. 
  - regexs: defines the regular expressions that have to be met by the project name. The RegExValidator checks the suggested name meets the regular expressions.

Sample projectNameValidation.config.json:
 
 ```
 {  
    "regexs" : [
      {
        "name" : "projectStartWith$",
        "pattern" : "^[^\\$]"
      }
    ],
    "reservedNames" : [
        "ReseredProjectName1",
        "ReseredProjectName2",
    ],
    "validateEmptyNames": true
}
 ```

- Item name (configured in itemNameValidation.config.json):
  - validateEmptyNames : boolean that indicates if empty name validation should be applied. The EmptyNameValidator ensures that the suggested name is not empty.
  - validateDefaultNames : boolean that indicates if default name validation should be applied. DefaultNameValidator is checking the suggest name against the names defined in the tag wts.defaultInstance of all templates.
  - validateExistingNames : boolean that indicates if existing name validation should be applied. ExistingNameValidator is checking the suggested name against all other items in the user selection.
  - reservedNames: defines the reserved names that cannot be used as item names. The ReservedNamesValidator checks the suggested project name against the reserved names. 
  - regexs: define the regular expressions that have to be met by the item name. The RegExValidator checks the suggested name meets the regular expressions.
  
Sample itemNameValidation.config.json:
 
 ```
 {  
      {
        "name" : "itemNameFormat",
        "pattern" : "^((?!\\d)\\w+)$"
      }
    ],
    "reservedNames" : [
      "ReservedItemName1"
      "ReservedItemName2"
      ],
    "validateExistingNames": true,
    "validateDefaultNames": true,
    "validateEmptyNames": true
}
 ```

 
## Learn more

- [Getting started with the CoreTS codebase](./getting-started-developers.md)
- [Windows Template Studio Templates](https://github.com/microsoft/WindowsTemplateStudio/blob/master/docs/templates.md)