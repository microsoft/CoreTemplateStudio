// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("ExecutionSet", "Minimum")]
    public class DateTimeExtensionsTests
    {
        private readonly DateTime date;

        public DateTimeExtensionsTests()
        {
            date = DateTime.Now;
        }

        [Fact]
        public void FormatAsDateForFilePath_ReturnsCorrectStringDate()
        {
            var factData = date;

            var expected = date.ToString("yyyyMMdd");

            var result = factData.FormatAsDateForFilePath();

            Assert.Equal(expected: expected, actual: result);
        }

        [Fact]
        public void FormatAsFullDateTime_ReturnsCorrectStringDate()
        {
            var factData = date;

            var expected = date.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var result = factData.FormatAsFullDateTime();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void FormatAsTime_ReturnsCorrectStringDate()
        {
            var factData = date;

            var expected = date.ToString("HH:mm:ss.fff");

            var result = factData.FormatAsTime();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void FormatAsShortDateTime_ReturnsCorrectStringDate()
        {
            var factData = date;

            var expected = date.ToString("yyyyMMdd_HHmmss");

            var result = factData.FormatAsShortDateTime();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void FormatAsDateHoursMinutes_ReturnsCorrectStringDate()
        {
            var factData = date;

            var expected = date.ToString("ddHHmmss");

            var result = factData.FormatAsDateHoursMinutes();

            Assert.Equal(result, expected);
        }
    }
}
