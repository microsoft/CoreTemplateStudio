// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "We follow the C# Core Coding Style which avoids using `this` unless absolutely necessary.")]

[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "We follow the C# Core Coding Style which puts using statements outside the namespace.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "It is not a priority and have hight impact in code changes.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "It is not a priority and have hight impact in code changes.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1203:ConstantsMustAppearBeforeFields", Justification = "It is not a priority and have hight impact in code changes.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "It is not a priority and have hight impact in code changes.")]

[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "We follow the C# Core Coding Style which uses underscores as prefixes rather than using `this.`.")]

[assembly: SuppressMessage("StyleCop.CSharp.SpecialRules", "SA0001:XmlCommentAnalysisDisabled", Justification = "Not enabled as we don't want or need XML documentation.")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Not enabled as we don't want or need XML documentation.")]

[assembly: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member", Target = "Microsoft.Templates.Core.Locations.TemplatesSynchronization.#SyncStatusChanged", Justification = "Using an Action<object, SyncStatusEventArgs> does not allow the required notation")]

// Non general supressions
[assembly: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "We need to have the names of these keys in lowercase to be able to compare with the keys becoming form the template json. ContainsKey does not allow StringComparer especification to IgnoreCase", Scope = "member", Target = "Microsoft.Templates.Core.ITemplateInfoExtensions.#GetQueryableProperties(Microsoft.TemplateEngine.Abstractions.ITemplateInfo)")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "We need to have the names of these keys in lowercase to be able to compare with the keys becoming form the template json. ContainsKey does not allow StringComparer especification to IgnoreCase", Scope = "member", Target = "Microsoft.Templates.Core.Composition.CompositionQuery.#Match(System.Collections.Generic.IEnumerable`1<Microsoft.Templates.Core.Composition.QueryNode>,Microsoft.Templates.Core.Composition.QueryablePropertyDictionary)")]

// Localization suppressions
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Casing.StringCasingExtensions.ToKebabCase(System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Casing.StringCasingExtensions.ToSnakeCase(System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Casing.StringCasingExtensions.ToLowerCase(System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~P:Microsoft.Templates.Core.Casing.TextCasing.ParameterName")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Gen.GenComposer.AddInCompositionTemplates(System.Collections.Generic.List{Microsoft.Templates.Core.Gen.GenInfo},Microsoft.Templates.Core.Gen.UserSelection,System.Boolean)~System.Collections.Generic.List{Microsoft.Templates.Core.Gen.GenInfo}")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Gen.UserSelection.ToString~System.String")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Gen.GenComposer.AddProjectParams(Microsoft.Templates.Core.Gen.GenInfo,Microsoft.Templates.Core.Gen.UserSelection)")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Diagnostics.TelemetryTracker.AddPropertiesFromPropertyBag(Microsoft.Templates.Core.Gen.UserSelectionContext,System.Collections.Generic.Dictionary{System.String,System.String})")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:WtsPackagingTool.RemoteSourceWorker.PublishContent(WtsPackagingTool.CommandOptions.RemoteSourcePublishOptions,System.IO.TextWriter,System.IO.TextWriter)")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:WtsPackagingTool.RemoteSource.GetTemplatesSourceConfig(System.String,WtsPackagingTool.EnvEnum)~Microsoft.Templates.Core.Locations.TemplatesSourceConfig")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:WtsPackagingTool.RemoteSourceWorker.PublishUpdatedRemoteSourceConfig(WtsPackagingTool.CommandOptions.RemoteSourcePublishOptions,System.IO.TextWriter)")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Test.Templates.TemplateRepositoryTests.GetFrontendFrameworks")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Need lower case here", Scope = "member", Target = "~M:Microsoft.Templates.Core.Test.Templates.TemplateRepositoryTests.GetBackendFrameworks")]
