// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Composition;
using Xunit;

namespace Microsoft.Templates.Core
{
    [Trait("ExecutionSet", "Minimum")]
    public class DictionaryExtensionsTests
    {
        private readonly Random random;
        private readonly Dictionary<string, string> dictionaryOfStrings;
        private readonly Dictionary<string, QueryableProperty> dictionaryOfQueryable;
        private const int MAX = 100;

        public DictionaryExtensionsTests()
        {
            random = new Random();
            dictionaryOfStrings = new Dictionary<string, string>();
            dictionaryOfQueryable = new Dictionary<string, QueryableProperty>();
        }

        private void SetUpStringData(int items)
        {
            for (var i = 0; i < items; i++)
            {
                dictionaryOfStrings.Add($"key{i + 1}", $"value{i + 1}");
            }
        }

        [Fact]
        public void SafeGet_DictionaryOfStrings_NotFound_ReturnsDefault()
        {
            string expected = null;

            var actual = dictionaryOfStrings.SafeGet("test");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeGet_DictionaryOfStrings_NotFound_ReturnsConfiguredDefault()
        {
            string expected = "default";

            var actual = dictionaryOfStrings.SafeGet("test", "default");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeGet_DictionaryOfStrings_Found_ReturnsItem()
        {
            var randomItemsNumber = random.Next(MAX);
            SetUpStringData(randomItemsNumber);

            var expected = $"value{randomItemsNumber}";

            var actual = dictionaryOfStrings.SafeGet($"key{randomItemsNumber}");

            Assert.Equal(expected, actual);
        }

        private void SetUpQueryableData(int items)
        {
            for (var i = 0; i < items; i++)
            {
                dictionaryOfQueryable.Add($"key{i + 1}", new QueryableProperty($"name{i + 1}", $"value{i + 1}"));
            }
        }

        [Fact]
        public void SafeGet_DictionaryOfQueryable_NotFound_ReturnsDefault()
        {
            QueryableProperty expected = null;

            var actual = dictionaryOfQueryable.SafeGet("test");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeGet_DictionaryOfQueryable_NotFound_ReturnsConfiguredDefault()
        {
            QueryableProperty expected = QueryableProperty.Empty;

            var actual = dictionaryOfQueryable.SafeGet("test", QueryableProperty.Empty);

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Value, actual.Value);
        }

        [Fact]
        public void SafeGet_DictionaryOfQueryable_Found_ReturnsItem()
        {
            var randomItemsNumber = random.Next(MAX);
            SetUpQueryableData(randomItemsNumber);

            var expected = new QueryableProperty($"name{randomItemsNumber}", $"value{randomItemsNumber}");

            var actual = dictionaryOfQueryable.SafeGet($"key{randomItemsNumber}");

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Value, actual.Value);
        }
    }
}
