// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;

using Xunit;

namespace Microsoft.Templates.Core.Test.Templates
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    public class TemplateRepositoryTests
    {
        private TemplatesFixture _fixture;
        private TemplatesRepository _repo;
        private const string TestPlatform = "test";

        public TemplateRepositoryTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(TestPlatform, language);
            _repo = GenContext.ToolBox.Repo;
        }

        [Fact]
        public void GetSupportedProjectTypes()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = _repo.GetProjectTypes(TestPlatform);

            Assert.Collection(projectTemplates, e1 => e1.Equals("ProjectType"));
        }

        [Fact]
        public void GetFrontendFrameworks()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworks = _repo.GetFrontEndFrameworks(TestPlatform, "pt1");

            Assert.Collection(frameworks, e1 =>
            {
                Assert.Equal("TestFramework", e1.Name);
                Assert.Equal(FrameworkTypes.FrontEnd.ToString().ToLower(), e1.Tags["type"]);
            });
        }

        [Fact]
        public void GetBackendFrameworks()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworks = _repo.GetBackEndFrameworks(TestPlatform, "pt3");

            Assert.Collection(frameworks, e1 =>
            {
                Assert.Equal("TestFramework", e1.Name);
                Assert.Equal(FrameworkTypes.BackEnd.ToString().ToLower(), e1.Tags["type"]);
            });
        }

        [Fact]
        public void GetPages_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, TestPlatform, "pt1", "fx1");

            Assert.Collection(
                pages,
                p1 =>
                {
                    Assert.Equal("PageTemplate", p1.Name);
                });
        }

        [Fact]
        public void GetPages_BackendAndFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, TestPlatform, "pt3", "fx1", "fx3");

            Assert.Collection(
                pages,
                p1 =>
                {
                    Assert.Equal("PageTemplate", p1.Name);
                });
        }

        [Fact]
        public void GetPages_NoMatches()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var pages = _repo.GetTemplatesInfo(TemplateType.Page, TestPlatform, "pt1", "fx1", "fx4");

            Assert.Empty(pages);
        }

        [Fact]
        public void GetFeatures_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var features = _repo.GetTemplatesInfo(TemplateType.Feature, TestPlatform, "pt1", "fx1");

            Assert.Collection(
                features,
                f1 =>
                {
                    Assert.Equal("FeatureTemplate", f1.Name);
                });
        }

        [Fact]
        public void GetFeatures_BackendAndFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var features = _repo.GetTemplatesInfo(TemplateType.Feature, TestPlatform, "pt3", "fx1", "fx3");

            Assert.Collection(
                features,
                f1 =>
                {
                    Assert.Equal("FeatureTemplate", f1.Name);
                });
        }

        [Fact]
        public void GetFeatures_NoMatches()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var features = _repo.GetTemplatesInfo(TemplateType.Feature, TestPlatform, "pt1", "fx1", "fx4");

            Assert.Empty(features);
        }
    }
}
