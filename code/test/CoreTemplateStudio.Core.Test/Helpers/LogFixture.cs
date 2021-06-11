// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Testing purposes only")]
    public class LogFixture : IDisposable
    {
        public string LogFile { get; private set; }

        public string TestFolderPath { get; private set; }

        public LogFixture()
        {
            TestFolderPath = Path.Combine(Environment.CurrentDirectory, "TempTestData");
            Directory.CreateDirectory(TestFolderPath);

            LogFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Configuration.Current.LogFileFolderPath,
                $"WTS_{Configuration.Current.Environment}_{DateTime.Now:yyyyMMdd}.log");

            if (File.Exists(LogFile))
            {
                File.Delete(LogFile);
            }
        }

        private bool CheckLoggingDataIsExpected(DateTime logDate, string errorLevel, string errorMessage)
        {
            // Sample of Logging line: [2021-06-07 20:51:30.557]...  Warning Error creating folder 'C:\...\bin\Debug\netcoreapp3.1\TestData\EnsureFolderExists\Test_EnsureFolderExists': ..... because a file or directory with the same name already exists.
            var logFileLines = File.ReadAllText(LogFile);

            var errorDateFormat = $"{logDate:yyyy\\-MM\\-dd}";
            var errorMatchPattern = $"^\\[({errorDateFormat}.*)({errorLevel}).*({errorMessage})";
            var errorRegex = new Regex(errorMatchPattern, RegexOptions.Compiled | RegexOptions.Multiline);

            return errorRegex.IsMatch(logFileLines);
        }

        public bool IsErrorMessageInLogFile(DateTime logDate, string errorLevel, string errorMessage)
        {
            if (File.Exists(LogFile))
            {
                var lastModifiedWriteTime = File.GetLastWriteTime(LogFile);

                var timeSinceLastEdit = logDate - lastModifiedWriteTime;
                if (timeSinceLastEdit.Seconds < 30)
                {
                    return CheckLoggingDataIsExpected(logDate, errorLevel, errorMessage);
                }
            }

            return false;
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            if (Directory.Exists(TestFolderPath))
            {
                Directory.Delete(TestFolderPath, true);
            }
        }
    }

    [SuppressMessage("StyleCop", "SA1402", Justification = "This class does not have implementation")]
    [CollectionDefinition("Unit Test Logs")]
    public class LogCollection : ICollectionFixture<LogFixture>
    {
    }
}
