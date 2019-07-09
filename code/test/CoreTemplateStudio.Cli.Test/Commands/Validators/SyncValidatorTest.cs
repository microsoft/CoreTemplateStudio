// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Validators;
using Microsoft.Templates.Cli.Resources;
using Xunit;

namespace Microsoft.Templates.Cli.Test.Commands.Validators
{
    [Trait("ExecutionSet", "Validators")]
    public class SyncValidatorTest
    {
        // Example how to test validators: https://fluentvalidation.net/testing
        private readonly SyncValidator _validator;
        private readonly string _validPath = Environment.CurrentDirectory;

        public SyncValidatorTest()
        {
            _validator = new Fixture().Create<SyncValidator>();
        }

        [Fact]
        public void SyncValidator_Path_failed_if_is_null_or_empty()
        {
            var command_with_null_path = new SyncCommand(null);
            var command_with_empty_path = new SyncCommand(string.Empty);

            _validator
                .ShouldHaveValidationErrorFor(t => t.Path, command_with_null_path)
                .WithErrorMessage(StringRes.BadReqInvalidPath);

            _validator
                .ShouldHaveValidationErrorFor(t => t.Path, command_with_empty_path)
                .WithErrorMessage(StringRes.BadReqInvalidPath);
        }

        [Fact]
        public void SyncValidator_Path_is_valid_if_is_not_null_or_empty()
        {
            var command = new SyncCommand(_validPath);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Path, command);
        }

        [Fact]
        public void SyncValidator_FullPath_failed_if_is_null_or_empty()
        {
            var command_with_null_path = new SyncCommand(null);
            var command_with_empty_path = new SyncCommand(string.Empty);

            _validator
                .ShouldHaveValidationErrorFor(t => t.FullPath, command_with_null_path)
                .WithErrorMessage(StringRes.BadReqInvalidPath);

            _validator
                .ShouldHaveValidationErrorFor(t => t.FullPath, command_with_empty_path)
                .WithErrorMessage(StringRes.BadReqInvalidPath);
        }

        [Fact]
        public void SyncValidator_FullPath_failed_if_is_invalid_path()
        {
            var command_with_invalid_path = new SyncCommand("invalidPath");

            _validator
                .ShouldHaveValidationErrorFor(t => t.FullPath, command_with_invalid_path)
                .WithErrorMessage(StringRes.BadReqInvalidPath);
        }

        [Fact]
        public void SyncValidator_FullPath_is_valid_if_is_valid_path()
        {
            var command = new SyncCommand(_validPath);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.FullPath, command);
        }

        [Fact]
        public void SyncValidator_Platform_is_valid_with_any_path()
        {
            var command_with_null_path = new SyncCommand(null);
            var command_with_empty_path = new SyncCommand(string.Empty);
            var command_with_valid_path = new SyncCommand(_validPath);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Platform, command_with_null_path);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Platform, command_with_empty_path);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Platform, command_with_valid_path);
        }

        [Fact]
        public void SyncValidator_Language_is_valid_with_any_path()
        {
            var command_with_null_path = new SyncCommand(null);
            var command_with_empty_path = new SyncCommand(string.Empty);
            var command_with_valid_path = new SyncCommand(_validPath);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Language, command_with_null_path);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Language, command_with_empty_path);

            _validator
                .ShouldNotHaveValidationErrorFor(t => t.Language, command_with_valid_path);
        }
    }
}
