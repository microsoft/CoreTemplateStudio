// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Dispose used for test purposes.")]
    public class LogFixture : IDisposable
    {
        public string LogFile { get; private set; }

        public LogFixture()
        {
            LogFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Configuration.Current.LogFileFolderPath,
                $"WTS_{Configuration.Current.Environment}_{DateTime.Now:yyyyMMdd}.log");

            if (File.Exists(LogFile))
            {
                File.Delete(LogFile);
            }
        }

        public void CheckLoggingDataIsExpected(DateTime logDate, string errorLevel, string errorMessage)
        {
            var logFileLines = File.ReadAllText(LogFile);

            // [2021-06-07 20:51:30.557]...  Warning Error creating folder 'C:\...\bin\Debug\netcoreapp3.1\TestData\EnsureFolderExists\Test_EnsureFolderExists': ..... because a file or directory with the same name already exists.
            Assert.Contains(errorLevel, logFileLines);
            Assert.Contains($"{logDate.Date:yyyy-MM-dd}", logFileLines, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(errorMessage, logFileLines);
        }

        public void Dispose()
        {
            if (File.Exists(LogFile))
            {
                File.Delete(LogFile);
            }
        }
    }
}
