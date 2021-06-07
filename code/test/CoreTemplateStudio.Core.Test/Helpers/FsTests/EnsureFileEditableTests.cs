// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Trait("ExecutionSet", "Minimum")]
    public class EnsureFileEditableTests
    {
        [Fact]
        public void EnsureFileEditable_FileIsReadOnly_ShouldChangeToReadOnly()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\ProtectedTest.csproj");
            var fileInfo = new FileInfo(filePath);
            try
            {
                using var stream = File.Create(filePath);
                fileInfo.IsReadOnly = true;

                Fs.EnsureFileEditable(filePath);
                var newFileInfo = new FileInfo(filePath);

                Assert.False(newFileInfo.IsReadOnly);
            }
            finally
            {
                // tidy up testing data
                fileInfo.Delete();
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EnsureFileEditable_FileDoesntExist_ShouldHandleException(string filePath)
        {
            Fs.EnsureFileEditable(filePath);
        }

        [Fact]
        public void EnsureFileEditable_FileIsNotReadOnly_ShouldNotModifyIsReadOnly()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj");
            var originalFileInfo = new FileInfo(filePath);
            Fs.EnsureFileEditable(filePath);
            var newFileInfo = new FileInfo(filePath);

            Assert.Equal(newFileInfo.IsReadOnly, originalFileInfo.IsReadOnly);
        }
    }
}
