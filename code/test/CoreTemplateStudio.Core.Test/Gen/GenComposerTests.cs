// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    public class GenComposerTests
    {
        private TemplatesFixture _fixture;
        private const string TestPlatform = "test";

        public GenComposerTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(TestPlatform, language);
        }

        [Fact]
        public void GetSupportedProjectTypes()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenComposer.GetSupportedProjectTypes(TestPlatform);

            Assert.Collection(projectTemplates, e1 => e1.Equals("pt1"), e1 => e1.Equals("pt2"), e1 => e1.Equals("pt3"));
        }

        [Fact]
        public void GetSupportedFrameworks()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworks = GenComposer.GetSupportedFx("pt1", TestPlatform);

            Assert.Collection(frameworks, e1 =>
            {
                Assert.Equal("fx1", e1.Name);
                Assert.Equal(FrameworkTypes.FrontEnd, e1.Type);
            });
        }

        [Fact]
        public void GetSupportedFrameworks_FrontAndBackEndFramework()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworks = GenComposer.GetSupportedFx("pt3", TestPlatform);

            Assert.Collection(
                frameworks,
                e1 =>
                {
                    Assert.Equal("fx3", e1.Name);
                    Assert.Equal(FrameworkTypes.FrontEnd, e1.Type);
                },
                e2 =>
                {
                    Assert.Equal("fx4", e2.Name);
                    Assert.Equal(FrameworkTypes.BackEnd, e2.Type);
                });
        }

        [Fact]
        public void GetPages_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var pages = GenComposer.GetPages("pt1", TestPlatform, "fx1");

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

            var pages = GenComposer.GetPages("pt1", TestPlatform, "fx1", "fx3");

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

            var pages = GenComposer.GetPages("pt1", TestPlatform, "fx1", "fx4");

            Assert.Empty(pages);
        }

        [Fact]
        public void GetFeatures_OnlyFrontEndFrameworkFilter()
        {
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var features = GenComposer.GetFeatures("pt1", TestPlatform, "fx1");

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

            var features = GenComposer.GetFeatures("pt1", TestPlatform, "fx1", "fx3");

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

            var features = GenComposer.GetFeatures("pt1", TestPlatform, "fx1", "fx4");

            Assert.Empty(features);
        }
    }
}
