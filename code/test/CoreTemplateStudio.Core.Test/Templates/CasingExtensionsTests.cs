// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            Assert.Equal("master-detail", CasingType.Kebab.GetTransform("MasterDetail"));
            Assert.Equal("master-detail", CasingType.Kebab.GetTransform("Master_Detail"));
            Assert.Equal("master-detail", CasingType.Kebab.GetTransform("Master Detail"));
            Assert.Equal("master-detail", CasingType.Kebab.GetTransform(" Master Detail"));
            Assert.Equal("master-detail", CasingType.Kebab.GetTransform("master detail"));

            Assert.Equal("master-detail-1", CasingType.Kebab.GetTransform("MasterDetail1"));
            Assert.Equal("master-ui", CasingType.Kebab.GetTransform("MasterUI"));
            Assert.Equal("master-ui", CasingType.Kebab.GetTransform("Master UI"));
            Assert.Equal("master-ui", CasingType.Kebab.GetTransform("Master_UI"));
        }

        [Fact]
        public void Test_TransformToPascalCase()
        {
            Assert.Equal("MasterDetail", CasingType.Pascal.GetTransform("MasterDetail"));
            Assert.Equal("MasterDetail", CasingType.Pascal.GetTransform("Master_Detail"));
            Assert.Equal("MasterDetail", CasingType.Pascal.GetTransform("Master Detail"));
            Assert.Equal("MasterDetail", CasingType.Pascal.GetTransform(" Master Detail"));
            Assert.Equal("MasterDetail1", CasingType.Pascal.GetTransform("master detail 1"));

            Assert.Equal("MasterUI", CasingType.Pascal.GetTransform("MasterUI"));
            Assert.Equal("MasterUI", CasingType.Pascal.GetTransform("master UI"));
            Assert.Equal("MasterUI", CasingType.Pascal.GetTransform("Master_UI"));
        }

        [Fact]
        public void Test_TransformToCamelCase()
        {
            Assert.Equal("masterDetail", CasingType.Camel.GetTransform("MasterDetail"));
            Assert.Equal("masterDetail", CasingType.Camel.GetTransform("Master_Detail"));
            Assert.Equal("masterDetail", CasingType.Camel.GetTransform("Master Detail"));
            Assert.Equal("masterDetail", CasingType.Camel.GetTransform(" Master Detail"));
            Assert.Equal("masterDetail1", CasingType.Camel.GetTransform("master detail 1"));

            Assert.Equal("masterUI", CasingType.Camel.GetTransform("MasterUI"));
            Assert.Equal("masterUI", CasingType.Camel.GetTransform("master UI"));
            Assert.Equal("masterUI", CasingType.Camel.GetTransform("Master_UI"));
        }
    }
}
