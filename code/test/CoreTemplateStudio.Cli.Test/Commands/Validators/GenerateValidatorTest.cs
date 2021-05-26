// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Templates.Cli.Commands;
using Microsoft.Templates.Cli.Commands.Validators;
using Microsoft.Templates.Cli.Resources;
using Microsoft.Templates.Cli.Services.Contracts;
using Microsoft.Templates.Cli.Utilities;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Moq;
using Xunit;

namespace Microsoft.Templates.Cli.Test.Commands.Validators
{
    [Trait("ExecutionSet", "Validators")]
    public class GenerateValidatorTest
    {
        private readonly GenerateValidator _validator;
        private readonly IFixture _fixture;
        private readonly string _testPath = Environment.CurrentDirectory;

        public GenerateValidatorTest()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            _validator = _fixture.Create<GenerateValidator>();
        }

        [Fact]
        public void GenerateValidator_failed_if_gencontext_is_not_initialized()
        {
            GenContext.ToolBox = null;
            var command = new GenerateCommand();

            _validator
                .ShouldHaveValidationErrorFor(t => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Fact]
        public void GenerateValidator_GenerationDataJson_failed_if_is_null_or_empty()
        {
            var commandWithEmptyData = _fixture
                                        .Build<GenerateCommand>()
                                        .With(x => x.GenerationDataJson, new List<string>())
                                        .Create();

            _validator
                .ShouldHaveValidationErrorFor(t => t.GenerationDataJson, commandWithEmptyData)
                .WithErrorMessage(StringRes.BadReqInvalidGenJson);
        }

        [Fact]
        public void Generatevalidator_is_valid_if_gencontext_is_initialized_and_command_has_data()
        {
            InitFakeGenContext();

            var command = _fixture
                            .Build<GenerateCommand>()
                            .With(x => x.GenerationDataJson, new List<string> { "any data" })
                            .Create();

            _validator
                .TestValidate(command)
                .ShouldNotHaveAnyValidationErrors();
        }

        private void InitFakeGenContext()
        {
            var fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

            var messageService = fixture.Freeze<Mock<IMessageService>>();
            var cliGenShell = fixture.Create<CliGenShell>();

            var fakePath = "fakePath";
            var fakePlatform = "fakePlatform";
            var fakeLanguage = "fakeLanguage";
            var fakeVersion = "0.0.0.0";

            GenContext.Bootstrap(
                    new LocalTemplatesSource(
                        fakePath,
                        fakeVersion,
                        string.Empty),
                    cliGenShell,
                    new Version(fakeVersion),
                    fakePlatform,
                    fakeLanguage);
        }
    }
}
