// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("Type", "Naming")]
    public class NamingTests
    {
        private TemplatesFixture _fixture;

        public NamingTests(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Infer_SuccessfullyAccountsForExistingNames()
        {
            Func<IEnumerable<string>> getExistingNames = () => { return new string[] { "App" }; };
            var validators = new List<Validator> { new ExistingNamesValidator(getExistingNames) };
            var result = NamingService.Infer("App", validators);

            Assert.Equal("App1", result);
        }

        [Fact]
        public void Infer_SuccessfullyAccountsForReservedNames()
        {
            var validators = new List<Validator> { new ReservedNamesValidator(new string[] { "Page" }) };
            var result = NamingService.Infer("Page", validators);

            Assert.Equal("Page1", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Infer_SuccessfullyAccountsForDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator> { new DefaultNamesValidator() };
            var result = NamingService.Infer("LiveTile", validators);

            Assert.Equal("LiveTile1", result);
        }

        [Fact]
        public void Infer_RemovesInvalidCharacters()
        {
            var result = NamingService.Infer("Blank$Page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void Infer_DoesNotRemoveNonAsciiCharacters()
        {
            var result = NamingService.Infer("ÑäöÜ!Page", new List<Validator>());

            Assert.Equal("ÑäöÜPage", result);
        }

        [Fact]
        public void Infer_SuccessfullyHandlesSpacesAndConversionToTitleCase()
        {
            var result = NamingService.Infer("blank page", new List<Validator>());

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void Validate_SuccessfullyHandles_FileExistsValidator()
        {
            var testDirectory = Path.GetTempPath();
            File.Create(Path.Combine(testDirectory, "TestFile"));
            var result = NamingService.Infer("TestFile", new List<Validator>() { new FileNameValidator(testDirectory) });

            Assert.Equal("TestFile1", result);
        }

        [Fact]
        public void Validate_SuccessfullyHandles_SuggestedDirectoryNameValidator()
        {
            var testDirectory = Path.GetTempPath();

            Directory.CreateDirectory(Path.Combine(testDirectory, "TestDir"));
            var result = NamingService.Infer("TestDir", new List<Validator>() { new FolderNameValidator(testDirectory) });

            Assert.Equal("TestDir1", result);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Validate_RecognizesValidNameAsValid(string language)
        {
            SetUpFixtureForTesting(language);
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
            };
            var result = NamingService.Validate("Blank1", validators);

            Assert.True(result.IsValid);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Validate_RecognizesEmptyStringAsInvalid(string language)
        {
            SetUpFixtureForTesting(language);
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
            };
            var result = NamingService.Validate(string.Empty, validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.EmptyName, result.ErrorType);
        }

        [Fact]
        public void ExistingNamesValidator_NullConfig()
        {
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(null),
            };
            Assert.Throws<ArgumentNullException>(() => NamingService.Validate("Blank", validators));
        }

        [Fact]
        public void Validate_SuccessfullyIdentifiesExistingNames()
        {
            Func<IEnumerable<string>> getExistingNames = () => { return new string[] { "Blank" }; };
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
                new ExistingNamesValidator(getExistingNames),
            };
            var result = NamingService.Validate("Blank", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Theory]
        [MemberData(nameof(GetAllLanguages))]
        public void Validate_SuccessfullyIdentifiesDefaultNames(string language)
        {
            SetUpFixtureForTesting(language);

            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
                new DefaultNamesValidator(),
            };
            var result = NamingService.Validate("LiveTile", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifiesReservedNames()
        {
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
                new ReservedNamesValidator(new string[] { "Page" }),
            };
            var result = NamingService.Validate("Page", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_InvalidChars()
        {
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
            };
            var result = NamingService.Validate("Blank;", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Regex, result.ErrorType);
            Assert.Equal("badFormat", result.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_NamesThatStartWithNumbers()
        {
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
            };
            var result = NamingService.Validate("1Blank", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Regex, result.ErrorType);
            Assert.Equal("badFormat", result.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_InvalidPageSuffix()
        {
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
                new RegExValidator(new RegExConfig() { Name = "itemEndsWithPage", Pattern = ".*(?<!page)$" }),
            };
            var result = NamingService.Validate("BlankPage", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Regex, result.ErrorType);
            Assert.Equal("itemEndsWithPage", result.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_ValidPageSuffix()
        {
            var validators = new List<Validator>()
            {
                new EmptyNameValidator(),
                new RegExValidator(new RegExConfig() { Name = "badFormat", Pattern = "^((?!\\d)\\w+)$" }),
                new RegExValidator(new RegExConfig() { Name = "itemEndsWithPage", Pattern = ".*(?<!page)$" }),
            };
            var result = NamingService.Validate("BlankView", validators);

            Assert.True(result.IsValid);
            Assert.Equal(ValidationErrorType.None, result.ErrorType);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_ReservedProjectName()
        {
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator(new string[] { "Prism" }),
            };
            var result = NamingService.Validate("Prism", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.ReservedName, result.ErrorType);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_ProjectStartsWith()
        {
            var validators = new List<Validator>()
            {
                new RegExValidator(new RegExConfig() { Name = "projectStartWith$", Pattern = "^[^\\$]" }),
            };
            var result = NamingService.Validate("$App", validators);

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Regex, result.ErrorType);
            Assert.Equal("projectStartWith$", result.ValidatorName);
        }

        [Fact]
        public void Validate_SuccessfullyIdentifies_ValidProjectName()
        {
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator(new string[] { "Prism" }),
                new RegExValidator(new RegExConfig() { Name = "projectStartWith$", Pattern = "^[^\\$]" }),
            };
            var result = NamingService.Validate("App", validators);

            Assert.True(result.IsValid);
            Assert.Equal(ValidationErrorType.None, result.ErrorType);
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture("test", language);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                if (language != ProgrammingLanguages.Any)
                {
                    yield return new object[] { language };
                }
            }
        }
    }
}
