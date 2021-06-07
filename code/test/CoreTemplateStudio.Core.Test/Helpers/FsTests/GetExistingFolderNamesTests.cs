// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Trait("ExecutionSet", "Minimum")]
    public class GetExistingFolderNamesTests
    {
        [Fact]
        public void GetExistingFolderNames_RootExists_ShouldReturnAllExpectedFolderNamesInAlphabeticalOrder()
        {
            var rootDirectory = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\Fs_GetExistingFolderNames");

            try
            {
                var directoryInfo = Directory.CreateDirectory(rootDirectory);
                for (var i = 0; i < 3; i++)
                {
                    Directory.CreateDirectory($"{rootDirectory}\\One");
                    Directory.CreateDirectory($"{rootDirectory}\\Two");
                    Directory.CreateDirectory($"{rootDirectory}\\Three");
                }

                var expected = new List<string>() { "One", "Three", "Two" };

                var actual = Fs.GetExistingFolderNames(rootDirectory);

                Assert.Equal(3, actual.Count());
                Assert.Equal(expected, actual);
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(rootDirectory, true);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetExistingFolderNames_RootDirectoryDoesNotExist_ShouldReturnEmptyList(string rootDirectory)
        {
            var actual = Fs.GetExistingFolderNames(rootDirectory);

            Assert.Empty(actual);
        }
    }
}
