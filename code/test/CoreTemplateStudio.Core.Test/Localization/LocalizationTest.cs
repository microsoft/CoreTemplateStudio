﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using System.Linq;
using Microsoft.Templates.Core.Gen;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    public class LocalizationTest : IDisposable
    {
        private TemplatesFixture _fixture;

        public LocalizationTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(language);
        }

        public void Dispose()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void Load_ProjectTemplates_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();

            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Test Project Type", template.DisplayName);
            Assert.Equal("Test Project Type Summary", template.Summary);
            Assert.Equal("Test Project Type Description", template.Description);
        }

        [Fact]
        public void Load_ProjectTemplates_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Proyecto de prueba", template.DisplayName);
            Assert.Equal("Resumen del proyecto de prueba", template.Summary);
            Assert.Equal("Descripción del proyecto de prueba", template.Description);
        }

        [Fact]
        public void Load_ProjectTemplates_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Test Project Type Base", template.DisplayName);
            Assert.Equal("Test Project Type Base Summary", template.Summary);
            Assert.Equal("Test Project Type Base Description", template.Description);
        }

        [Fact]
        public void Load_ProjectTemplates_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Test Project Type Base", template.DisplayName);
            Assert.Equal("Test Project Type Base Summary", template.Summary);
            Assert.Equal("Test Project Type Base Description", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Test Framework", template.DisplayName);
            Assert.Equal("Test Framework Summary", template.Summary);
            Assert.Equal("Test Framework Description", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Framework de prueba", template.DisplayName);
            Assert.Equal("Resumen de Framework de prueba", template.Summary);
            Assert.Equal("Descripción de Framework de prueba", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Test Framework Base", template.DisplayName);
            Assert.Equal("Test Framework Base Summary", template.Summary);
            Assert.Equal("Test Framework Base Description", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal("Test Framework Base", template.DisplayName);
            Assert.Equal("Test Framework Base Summary", template.Summary);
            Assert.Equal("Test Framework Base Description", template.Description);
        }

        [Fact]
        public void Load_PageTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft España", template.Author);
            Assert.Equal("PageTemplate", template.Name);
            Assert.Equal("Está en Español...", template.Description);
            Assert.Equal("Descripción del proyecto de prueba", template.GetRichDescription());
        }

        [Fact]
        public void Load_PageTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft USA", template.Author);
            Assert.Equal("PageTemplate", template.Name);
            Assert.Equal("US English...", template.Description);
            Assert.Equal("US description", template.GetRichDescription());
        }

        [Fact]
        public void Load_PageTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft", template.Author);
            Assert.Equal("PageTemplate", template.Name);
            Assert.Equal("Generic description...", template.Description);
            Assert.Equal("Generic description", template.GetRichDescription());
        }

        [Fact]
        public void Load_PageTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft", template.Author);
            Assert.Equal("PageTemplate", template.Name);
            Assert.Equal("Generic description...", template.Description);
            Assert.Equal("Generic description", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft España", template.Author);
            Assert.Equal("FeatureTemplate", template.Name);
            Assert.Equal("Está en Español (Feature)...", template.Description);
            Assert.Equal("Descripción de la Feature de prueba", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft USA", template.Author);
            Assert.Equal("FeatureTemplate", template.Name);
            Assert.Equal("Feature US English...", template.Description);
            Assert.Equal("US Feature description", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft", template.Author);
            Assert.Equal("FeatureTemplate", template.Name);
            Assert.Equal("Generic Feature description...", template.Description);
            Assert.Equal("Generic Feature MD description", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal("Microsoft", template.Author);
            Assert.Equal("FeatureTemplate", template.Name);
            Assert.Equal("Generic Feature description...", template.Description);
            Assert.Equal("Generic Feature MD description", template.GetRichDescription());
        }
    }
}
