// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Casing;
using Microsoft.Templates.Core.Templates;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("Type", "ProjectGeneration")]
    public class CasingExtensionsTests
    {
        [Fact]
        public void Test_TransformToKebab()
        {
            var kebabCasingService = new TextCasing() { Type = CasingType.Kebab };

            Assert.Equal("master-detail", kebabCasingService.Transform("MasterDetail"));
            Assert.Equal("master-detail", kebabCasingService.Transform("Master_Detail"));
            Assert.Equal("master-detail", kebabCasingService.Transform("Master Detail"));
            Assert.Equal("master-detail", kebabCasingService.Transform(" Master Detail "));
            Assert.Equal("master-detail", kebabCasingService.Transform("master detail"));
            Assert.Equal("master-detail-123-abc", kebabCasingService.Transform("master   -  detail 123 abc"));

            Assert.Equal("master-detail-1", kebabCasingService.Transform("MasterDetail1"));
            Assert.Equal("master-ui", kebabCasingService.Transform("MasterUI"));
            Assert.Equal("master-ui", kebabCasingService.Transform("Master UI"));
            Assert.Equal("master-ui", kebabCasingService.Transform("Master_UI"));
        }

        [Fact]
        public void Test_TransformToSnake()
        {
            var snakeCasingService = new TextCasing() { Type = CasingType.Snake };

            Assert.Equal("master_detail", snakeCasingService.Transform("MasterDetail"));
            Assert.Equal("master_detail", snakeCasingService.Transform("Master_Detail"));
            Assert.Equal("master_detail", snakeCasingService.Transform("Master Detail"));
            Assert.Equal("master_detail", snakeCasingService.Transform(" Master Detail "));
            Assert.Equal("master_detail", snakeCasingService.Transform("master detail"));
            Assert.Equal("master_detail_123_abc", snakeCasingService.Transform("master   -  detail 123 abc"));

            Assert.Equal("master_detail_1", snakeCasingService.Transform("MasterDetail1"));
            Assert.Equal("master_ui", snakeCasingService.Transform("MasterUI"));
            Assert.Equal("master_ui", snakeCasingService.Transform("Master UI"));
            Assert.Equal("master_ui", snakeCasingService.Transform("Master_UI"));
        }

        [Fact]
        public void Test_TransformToPascalCase()
        {
            var pascalCasingService = new TextCasing() { Type = CasingType.Pascal };

            Assert.Equal("MasterDetail", pascalCasingService.Transform("MasterDetail"));
            Assert.Equal("MasterDetail", pascalCasingService.Transform("Master_Detail"));
            Assert.Equal("MasterDetail", pascalCasingService.Transform("Master Detail"));
            Assert.Equal("MasterDetail", pascalCasingService.Transform(" Master Detail "));
            Assert.Equal("MasterDetail1", pascalCasingService.Transform("master detail 1"));

            Assert.Equal("MasterUI", pascalCasingService.Transform("MasterUI"));
            Assert.Equal("MasterUI", pascalCasingService.Transform("master UI"));
            Assert.Equal("MasterUI", pascalCasingService.Transform("Master_UI"));
        }

        [Fact]
        public void Test_TransformToCamelCase()
        {
            var camelCasingService = new TextCasing() { Type = CasingType.Camel };

            Assert.Equal("masterDetail", camelCasingService.Transform("MasterDetail"));
            Assert.Equal("masterDetail", camelCasingService.Transform("Master_Detail"));
            Assert.Equal("masterDetail", camelCasingService.Transform("Master Detail"));
            Assert.Equal("masterDetail", camelCasingService.Transform(" Master Detail "));
            Assert.Equal("masterDetail1", camelCasingService.Transform("master detail 1"));
            Assert.Equal("masterUI", camelCasingService.Transform("MasterUI"));
            Assert.Equal("masterUI", camelCasingService.Transform("master UI"));
            Assert.Equal("masterUI", camelCasingService.Transform("Master_UI"));
        }

        [Fact]
        public void Test_TransformToLowerCase()
        {
            var lowerCasingService = new TextCasing() { Type = CasingType.Lower };

            Assert.Equal("masterdetail", lowerCasingService.Transform("MasterDetail"));
            Assert.Equal("masterdetail", lowerCasingService.Transform("Master_Detail"));
            Assert.Equal("masterdetail", lowerCasingService.Transform("Master Detail"));
            Assert.Equal("masterdetail", lowerCasingService.Transform(" Master Detail "));
            Assert.Equal("masterdetail1", lowerCasingService.Transform("master detail 1"));
            Assert.Equal("masterui", lowerCasingService.Transform("MasterUI"));
            Assert.Equal("masterui", lowerCasingService.Transform("master UI"));
            Assert.Equal("masterui", lowerCasingService.Transform("Master_UI"));
        }
    }
}
