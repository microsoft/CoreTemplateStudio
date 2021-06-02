// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("ExecutionSet", "Minimum")]
    public class VersionExtensionsTests
    {
        [Fact]
        public void IsZero_IsNullVersion_ShouldReturnTrue()
        {
            Version factData = null;
            var actual = factData.IsNull();

            Assert.True(actual);
        }

        [Fact]
        public void IsZero_NotNullVersion_ShouldReturnFalse()
        {
            Version factData = new Version();
            var actual = factData.IsNull();

            Assert.False(actual);
        }

        [Fact]
        public void IsZero_VersionIsZero_WithMajorMinorBuildAndRevision_ShouldReturnTrue()
        {
            var factData = new Version(0, 0, 0, 0);
            var actual = factData.IsZero();

            Assert.True(actual);
        }

        [Theory]
        [MemberData(nameof(EmptyOrIncompleteData))]
        public void IsZero_EmptyOrIncompleteVersion_ShouldReturnFalse(Version version)
        {
            var actual = version.IsZero();

            Assert.False(actual);
        }

        [Theory]
        [MemberData(nameof(NonZeroVersionData))]
        public void IsZero_NonZeroVersion_ShouldReturnFalse(Version version)
        {
            var actual = version.IsZero();

            Assert.False(actual);
        }

        public static IEnumerable<object[]> EmptyOrIncompleteData => new List<object[]>()
        {
            new object[]
            {
                null,
            },
            new object[]
            {
                new Version(),
            },
            new object[]
            {
                new Version("0.0"),
            },
            new object[]
            {
                new Version(0, 0),
            },
            new object[]
            {
                new Version(0, 0, 0),
            },
        };

        public static IEnumerable<object[]> NonZeroVersionData => new List<object[]>()
        {
            new object[]
            {
                new Version("1.0.0.0"),
            },
            new object[]
            {
                new Version("0.1.0.0"),
            },
            new object[]
            {
                new Version("0.0.1.0"),
            },
            new object[]
            {
                new Version(1, 0, 0, 0),
            },
            new object[]
            {
                new Version(0, 1, 0, 0),
            },
            new object[]
            {
                new Version(0, 0, 0, 1),
            },
        };
    }
}
