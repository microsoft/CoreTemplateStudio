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
        SPAFS,
        [DisplayName("Multi Page Full Stack Application")]
        [Description("A multi page web application with a connected backend")]
        MPAFS,
        [DisplayName("Single Page Frontend")]
        [Description("A single page frontend only application")]
        SPFE,
        [DisplayName("Multi Page Frontend")]
        [Description("A multi page frontend only application")]
        MPFE,
        [DisplayName("RESTful API")]
        [Description("A REST Application programming interface backend only")]
        REST,
        [DisplayName("Navigation Pane")]
        [Description("A navigation pane is included for navigation between pages")]
        NAVCS,
        [DisplayName("Blank")]
        [Description("The basic project is a blank canvas")]
        BCS,
        [DisplayName("Pivots and Tabs")]
        [Description("Tabs across the top for quickly navigating between pages")]
        PTCS,
        [DisplayName("Navigation Pane")]
        [Description("A navigation pane is included for navigation between pages")]
        NAVVB,
        [DisplayName("Blank")]
        [Description("The basic project is a blank canvas")]
        BVB,
        [DisplayName("Pivots and Tabs")]
        [Description("Tabs across the top for quickly navigating between pages")]
        PTVB,
    }

    public enum Framework
    {
        [DisplayName("ReactJS")]
        [Description("React is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers.")]
        RJS,
        [DisplayName("VueJS")]
        [Description("Vue.js is an open-source JavaScript framework for building user interfaces and single-page applications.")]
        VJS,
        [DisplayName("AngularJS")]
        [Description("AngularJS is a JavaScript - based open - source front - end web application framework mainly maintained by Google and by a community of individuals and corporations.")]
        AJS,
        [DisplayName("NodeJS")]
        [Description("Node.js is an open-source, cross-platform JavaScript run-time environment that executes JavaScript code outside of a browser.")]
        NJS,
        [DisplayName("Django")]
        [Description("Django is a Python-based free and open-source web framework, which follows the model-view-template architectural pattern. It is maintained by the Django Software Foundation.")]
        DJG,
        [DisplayName("SinglePageJS")]
        [Description("A JavaScript framework that supports only singlepage websites.")]
        SPJS,
        [DisplayName("MultiPageJS")]
        [Description("A JavaScript framework that supports only multipage websites.")]
        MPJS,
        [DisplayName("Code Behind")]
        [Description("This is the traditional model for an application where logic is handled in the code-behind.")]
        CBH,
        [DisplayName("MVVM Light")]
        [Description("MVVM Light is a popular, 3rd party framework based on the Model - View - ViewModel pattern.")]
        MVVML,
        [DisplayName("Caliburn.Micro")]
        [Description("Caliburn.Micro is a popular MVVM framework emphasising conventions and composability.")]
        CBM,
        [DisplayName("MVVM Basic")]
        [Description("This is a generic version of a MVVM pattern.The Model - View - ViewModel pattern can be used on all XAML platforms.")]
        MVVMB,
        [DisplayName("Prism")]
        [Description("Prism is a framework for building loosely coupled, maintainable, and testable XAML applications.")]
        PRSM,
    }
}
