// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers
{
    [Trait("ExecutionSet", "Minimum")]
    public class FsTests
    {
        [Fact]
        public void EnsureFolder_DirectoryExists_ShouldNotCreateDirectory()
        {
            var sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\");
            var originalNumberOfDirectories = Directory.GetDirectories(sourceFile).Length;

            Fs.EnsureFolder(sourceFile);

            var newNumberOfDirectories = Directory.GetDirectories(sourceFile).Length;

            Assert.Equal(originalNumberOfDirectories, newNumberOfDirectories);
        }

        [Fact]
        public void EnsureFolder_ErrorCreatingDirectory_ShouldNotError()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\Test.csproj");

            Fs.EnsureFolder(sourceFolder);
        }

        [Fact]
        public void EnsureFolder_DirectoryDoesNotExists_ShouldCreateDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject2\\");
            var originalDirectoryExists = Directory.Exists(sourceFolder);

            Fs.EnsureFolder(sourceFolder);

            var newlyCreatedDirectoryExists = Directory.Exists(sourceFolder);

            Assert.False(originalDirectoryExists);
            Assert.True(newlyCreatedDirectoryExists);

            // tidy up testing data
            Directory.Delete(sourceFolder);
        }
    }
}
