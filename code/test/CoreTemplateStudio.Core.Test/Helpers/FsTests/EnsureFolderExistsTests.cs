// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Trait("ExecutionSet", "Minimum")]
    public class EnsureFolderExistsTests : IClassFixture<LogFixture>
    {
        private DateTime _logDate;
        private LogFixture _logFixture;

        public EnsureFolderExistsTests(LogFixture logFixture)
        {
            _logFixture = logFixture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact]
        public void EnsureFolderExists_DirectoryDoesNotExists_ShouldCreateDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject_EnsureFolderExists");
            try
            {
                var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Fs.EnsureFolderExists(sourceFolder);

                var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Assert.True(totalNewDirectories > totalOriginalDirectories);
            }
            finally
            {
                // tidy up testing data
                Directory.Delete(sourceFolder);
            }
        }

        [Fact]
        public void EnsureFolderExists_DirectoryAlreadyExists_ShouldNotCreateDirectory()
        {
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\EnsureFolderExists");

            try
            {
                Directory.CreateDirectory(sourceFolder);
                var totalOriginalDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Fs.EnsureFolderExists(sourceFolder);

                var totalNewDirectories = Directory.GetParent(sourceFolder).GetDirectories().Length;

                Assert.True(totalOriginalDirectories == totalNewDirectories);
            }
            finally
            {
                // tidy up test data
                Directory.Delete(sourceFolder);
            }
        }

        [Fact]
        public void EnsureFolderExists_ErrorCreatingDirectory_ShouldLogException()
        {
            // To force an error creating a Directory
            // we create a file with the name of the folder that we want to create
            var sourceFolder = Path.Combine(Environment.CurrentDirectory, "TestData\\EnsureFolderExists");
            Directory.CreateDirectory(sourceFolder);
            var folderPath = Path.Combine(Environment.CurrentDirectory, sourceFolder, "Test_EnsureFolderExists");
            if (File.Exists(_logFixture.LogFile))
            {
                File.Delete(_logFixture.LogFile);
            }

            try
            {
                using var stream = File.Create(folderPath);
                _logDate = DateTime.Now;
                Fs.EnsureFolderExists(folderPath);

                Assert.True(File.Exists(_logFixture.LogFile));

                var logFileLines = File.ReadAllText(_logFixture.LogFile);

                _logFixture.CheckLoggingDataIsExpected(_logDate, "Warning", "Error creating folder");
            }
            finally
            {
                // tidy up testing data
                File.Delete(folderPath);
            }
        }
    }
}
