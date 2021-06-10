// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Collection("Unit Test Logs")]
    [Trait("ExecutionSet", "Minimum")]
    public class GetExistingFolderNamesTests
    {
        private readonly LogFixture _logFixture;

        public GetExistingFolderNamesTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void GetExistingFolderNames_RootExists_ShouldReturnAllExpectedFolderNamesInAlphabeticalOrder()
        {
            var rootDirectory = $"{_logFixture.TestFolderPath}\\TestProject\\Fs_GetExistingFolderNames";

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
