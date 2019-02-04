// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace CoreTemplateStudio.Api.Enumerables
{
    public enum Platform
    {
        [DisplayName("Web")]
        Web,
        [DisplayName("Uwp")]
        Uwp,
    }

    public enum Language
    {
        [DisplayName("Any")]
        Any,
        [DisplayName("Visual Basic")]
        VB,
        [DisplayName("C#")]
        CSharp,
    }

    public enum FrameworkType
    {
        [DisplayName("Frontend")]
        Frontend,
        [DisplayName("Backend")]
        Backend,
        [DisplayName("UwpDesign")]
        UwpDesign,
    }

    public enum ProjectType
    {
        [DisplayName("Single Page Full Stack Application")]
        [Description("A single page web application with a connected backend")]
        SinglePageFull,
        [DisplayName("Multi Page Full Stack Application")]
        [Description("A multi page web application with a connected backend")]
        MultiPageFull,
        [DisplayName("Single Page Frontend")]
        [Description("A single page frontend only application")]
        SinglePageFront,
        [DisplayName("Multi Page Frontend")]
        [Description("A multi page frontend only application")]
        MultiPageFront,
        [DisplayName("RESTful API")]
        [Description("A REST Application programming interface backend only")]
        RESTAPI,
        [DisplayName("Navigation Pane")]
        [Description("A navigation pane is included for navigation between pages")]
        NavPaneCSharp,
        [DisplayName("Blank")]
        [Description("The basic project is a blank canvas")]
        BlankCSharp,
        [DisplayName("Pivots and Tabs")]
        [Description("Tabs across the top for quickly navigating between pages")]
        PivotTabCSharp,
        [DisplayName("Navigation Pane")]
        [Description("A navigation pane is included for navigation between pages")]
        NavPaneVB,
        [DisplayName("Blank")]
        [Description("The basic project is a blank canvas")]
        BlankVB,
        [DisplayName("Pivots and Tabs")]
        [Description("Tabs across the top for quickly navigating between pages")]
        PivotTabVB,
    }

    public enum Framework
    {
        [DisplayName("ReactJS")]
        [Description("React is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers.")]
        ReactJS,
        [DisplayName("VueJS")]
        [Description("Vue.js is an open-source JavaScript framework for building user interfaces and single-page applications.")]
        VueJS,
        [DisplayName("AngularJS")]
        [Description("AngularJS is a JavaScript - based open - source front - end web application framework mainly maintained by Google and by a community of individuals and corporations.")]
        AngularJS,
        [DisplayName("NodeJS")]
        [Description("Node.js is an open-source, cross-platform JavaScript run-time environment that executes JavaScript code outside of a browser.")]
        NodeJS,
        [DisplayName("Django")]
        [Description("Django is a Python-based free and open-source web framework, which follows the model-view-template architectural pattern. It is maintained by the Django Software Foundation.")]
        Django,
        [DisplayName("SinglePageJS")]
        [Description("A JavaScript framework that supports only singlepage websites.")]
        SinglePageJS,
        [DisplayName("MultiPageJS")]
        [Description("A JavaScript framework that supports only multipage websites.")]
        MultiPageJS,
        [DisplayName("Code Behind")]
        [Description("This is the traditional model for an application where logic is handled in the code-behind.")]
        CodeBehind,
        [DisplayName("MVVM Light")]
        [Description("MVVM Light is a popular, 3rd party framework based on the Model - View - ViewModel pattern.")]
        MVVMLight,
        [DisplayName("Caliburn.Micro")]
        [Description("Caliburn.Micro is a popular MVVM framework emphasising conventions and composability.")]
        CaliburnMicro,
        [DisplayName("MVVM Basic")]
        [Description("This is a generic version of a MVVM pattern.The Model - View - ViewModel pattern can be used on all XAML platforms.")]
        MVVMBasic,
        [DisplayName("Prism")]
        [Description("Prism is a framework for building loosely coupled, maintainable, and testable XAML applications.")]
        Prism,
    }
}
