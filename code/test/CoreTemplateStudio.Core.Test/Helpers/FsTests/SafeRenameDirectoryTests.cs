// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Trait("ExecutionSet", "Minimum")]
    public class SafeRenameDirectoryTests
    {
        [Fact]
        public void SafeRenameDirectory_ValidData_ShouldMoveDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\SafeRenameDirectory");
            var newFolder = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject_Copy");
            Directory.CreateDirectory(sourceFolder);
            try
            {
                var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Fs.SafeRenameDirectory(sourceFolder, newFolder);

                var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                var sameNumberOfDirectories = totalNewDirectories == totalOriginalDirectories;
                Assert.True(sameNumberOfDirectories);
                var oldDirectoryHasBeenMovedToNewDirectory = Directory.Exists(newFolder) && !Directory.Exists(sourceFolder);
                Assert.True(oldDirectoryHasBeenMovedToNewDirectory);
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(newFolder, true);
            }
        }

        [Theory]
        [InlineData("", "anything")]
        [InlineData(null, "anything")]
        public void SafeRenameDirectory_DoesNotExist_ShouldHandleException(string rootDir, string newRootDir)
        {
            Fs.SafeRenameDirectory(rootDir, newRootDir);
        }

        [Fact]
        public void SafeRenameDirectory_WrongFolder_ShouldHandleException()
        {
            var rootDir = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\");
            var newDir = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\Test.csproj");

            Fs.SafeRenameDirectory(rootDir, newDir);
        }

        [Fact]
        public void SafeRenameDirectory_WrongFolder_WarnOnFailureFalse_ShouldHandleException()
        {
            var rootDir = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\");
            var newDir = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\Test.csproj");

            Fs.SafeRenameDirectory(rootDir, newDir, false);
        }
    }
}
