// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
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
    public class GetItemsValidatorTest
    {
        private readonly IFixture _fixture;

        public GetItemsValidatorTest()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Theory]
        [AutoData]
        public void GetProjectTypesValidator_failed_if_gencontext_is_not_initialized(GetProjectTypesValidator validator, GetProjectTypesCommand command)
        {
            ClearFakeGenContext();

            validator
                .ShouldHaveValidationErrorFor(x => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Theory]
        [AutoData]
        public void GetProjectTypesValidator_is_valid_if_gencontext_is_initialized(GetProjectTypesValidator validator, GetProjectTypesCommand command)
        {
            InitFakeGenContext();

            validator
                .ShouldNotHaveValidationErrorFor(x => GenContext.ToolBox, command);
        }

        [Theory]
        [AutoData]
        public void GetFrameworksValidator_failed_if_gencontext_is_not_initialized(GetFrameworksValidator validator, GetFrameworksCommand command)
        {
            ClearFakeGenContext();

            validator
                .ShouldHaveValidationErrorFor(x => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Theory]
        [AutoData]
        public void GetFrameworksValidator_failed_if_project_type_is_null_or_empty(GetFrameworksValidator validator)
        {
            var command = _fixture
                            .Build<GetFrameworksCommand>()
                            .With(x => x.ProjectType, string.Empty)
                            .Create();

            validator
                .ShouldHaveValidationErrorFor(t => t.ProjectType, command)
                .WithErrorMessage(StringRes.BadReqInvalidProjectType);
        }

        [Theory]
        [AutoData]
        public void GetFrameworksValidator_is_valid_if_gencontext_is_initialized_and_has_ProjectType(GetFrameworksValidator validator, GetFrameworksCommand command)
        {
            InitFakeGenContext();

            validator
                .TestValidate(command)
                .ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [AutoData]
        public void GetPagesValidator_failed_if_gencontext_is_not_initialized(GetPagesValidator validator, GetPagesCommand command)
        {
            ClearFakeGenContext();

            validator
                .ShouldHaveValidationErrorFor(x => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Theory]
        [AutoData]
        public void GetPagesValidator_is_valid_if_gencontext_is_initialized(GetPagesValidator validator, GetPagesCommand command)
        {
            InitFakeGenContext();

            validator
                .ShouldNotHaveValidationErrorFor(x => GenContext.ToolBox, command);
        }

        [Theory]
        [AutoData]
        public void GetPagesValidator_failed_if_project_type_is_null_or_empty(GetPagesValidator validator)
        {
            var command = _fixture
                            .Build<GetPagesCommand>()
                            .With(x => x.ProjectType, string.Empty)
                            .Create();

            validator
                .ShouldHaveValidationErrorFor(t => t.ProjectType, command)
                .WithErrorMessage(StringRes.BadReqInvalidProjectType);
        }

        [Theory]
        [AutoData]
        public void GetPagesValidator_failed_if_frontend_and_backend_frameworks_are_null_or_empty(GetPagesValidator validator)
        {
            var command = _fixture
                            .Build<GetPagesCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .TestValidate(command)
                .ShouldHaveAnyValidationError()
                .WithErrorMessage(StringRes.BadReqNoBackendOrFrontend);
        }

        [Theory]
        [AutoData]
        public void GetPagesValidator_is_valid_if_contain_frontend_or_backend_framework(GetPagesValidator validator)
        {
            var command_without_frontend_framework = _fixture
                            .Build<GetPagesCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .Create();

            var command_without_backend_framework = _fixture
                            .Build<GetPagesCommand>()
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .ShouldNotHaveValidationErrorFor(t => t.FrontendFramework, command_without_frontend_framework);

            validator
                .ShouldNotHaveValidationErrorFor(t => t.BackendFramework, command_without_backend_framework);
        }

        [Theory]
        [AutoData]
        public void GetFeaturesValidator_failed_if_gencontext_is_not_initialized(GetFeaturesValidator validator, GetFeaturesCommand command)
        {
            ClearFakeGenContext();

            validator
                .ShouldHaveValidationErrorFor(x => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Theory]
        [AutoData]
        public void GetFeaturesValidator_is_valid_if_gencontext_is_initialized(GetFeaturesValidator validator, GetFeaturesCommand command)
        {
            InitFakeGenContext();

            validator
                .ShouldNotHaveValidationErrorFor(x => GenContext.ToolBox, command);
        }

        [Theory]
        [AutoData]
        public void GetFeaturesValidator_failed_if_project_type_is_null_or_empty(GetFeaturesValidator validator)
        {
            var command = _fixture
                            .Build<GetFeaturesCommand>()
                            .With(x => x.ProjectType, string.Empty)
                            .Create();

            validator
                .ShouldHaveValidationErrorFor(t => t.ProjectType, command)
                .WithErrorMessage(StringRes.BadReqInvalidProjectType);
        }

        [Theory]
        [AutoData]
        public void GetFeaturesValidator_failed_if_frontend_and_backend_frameworks_are_null_or_empty(GetFeaturesValidator validator)
        {
            var command = _fixture
                            .Build<GetFeaturesCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .TestValidate(command)
                .ShouldHaveAnyValidationError()
                .WithErrorMessage(StringRes.BadReqNoBackendOrFrontend);
        }

        [Theory]
        [AutoData]
        public void GetFeaturesValidator_is_valid_if_contain_frontend_or_backend_framework(GetFeaturesValidator validator)
        {
            var command_without_frontend_framework = _fixture
                            .Build<GetFeaturesCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .Create();

            var command_without_backend_framework = _fixture
                            .Build<GetFeaturesCommand>()
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .ShouldNotHaveValidationErrorFor(t => t.FrontendFramework, command_without_frontend_framework);

            validator
                .ShouldNotHaveValidationErrorFor(t => t.BackendFramework, command_without_backend_framework);
        }

        [Theory]
        [AutoData]
        public void GetServicesValidator_failed_if_gencontext_is_not_initialized(GetServicesValidator validator, GetServicesCommand command)
        {
            ClearFakeGenContext();

            validator
                .ShouldHaveValidationErrorFor(x => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Theory]
        [AutoData]
        public void GetServicesValidator_is_valid_if_gencontext_is_initialized(GetServicesValidator validator, GetServicesCommand command)
        {
            InitFakeGenContext();

            validator
                .ShouldNotHaveValidationErrorFor(x => GenContext.ToolBox, command);
        }

        [Theory]
        [AutoData]
        public void GetServicesValidator_failed_if_project_type_is_null_or_empty(GetServicesValidator validator)
        {
            var command = _fixture
                            .Build<GetServicesCommand>()
                            .With(x => x.ProjectType, string.Empty)
                            .Create();

            validator
                .ShouldHaveValidationErrorFor(t => t.ProjectType, command)
                .WithErrorMessage(StringRes.BadReqInvalidProjectType);
        }

        [Theory]
        [AutoData]
        public void GetServicesValidator_failed_if_frontend_and_backend_frameworks_are_null_or_empty(GetServicesValidator validator)
        {
            var command = _fixture
                            .Build<GetServicesCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .TestValidate(command)
                .ShouldHaveAnyValidationError()
                .WithErrorMessage(StringRes.BadReqNoBackendOrFrontend);
        }

        [Theory]
        [AutoData]
        public void GetServicesValidator_is_valid_if_contain_frontend_or_backend_framework(GetServicesValidator validator)
        {
            var command_without_frontend_framework = _fixture
                            .Build<GetServicesCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .Create();

            var command_without_backend_framework = _fixture
                            .Build<GetServicesCommand>()
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .ShouldNotHaveValidationErrorFor(t => t.FrontendFramework, command_without_frontend_framework);

            validator
                .ShouldNotHaveValidationErrorFor(t => t.BackendFramework, command_without_backend_framework);
        }

        [Theory]
        [AutoData]
        public void GetTestingsValidator_failed_if_gencontext_is_not_initialized(GetTestingsValidator validator, GetTestingsCommand command)
        {
            ClearFakeGenContext();

            validator
                .ShouldHaveValidationErrorFor(x => GenContext.ToolBox, command)
                .WithErrorMessage(StringRes.BadReqNotSynced);
        }

        [Theory]
        [AutoData]
        public void GetTestingsValidator_is_valid_if_gencontext_is_initialized(GetTestingsValidator validator, GetTestingsCommand command)
        {
            InitFakeGenContext();

            validator
                .ShouldNotHaveValidationErrorFor(x => GenContext.ToolBox, command);
        }

        [Theory]
        [AutoData]
        public void GetTestingsValidator_failed_if_project_type_is_null_or_empty(GetTestingsValidator validator)
        {
            var command = _fixture
                            .Build<GetTestingsCommand>()
                            .With(x => x.ProjectType, string.Empty)
                            .Create();

            validator
                .ShouldHaveValidationErrorFor(t => t.ProjectType, command)
                .WithErrorMessage(StringRes.BadReqInvalidProjectType);
        }

        [Theory]
        [AutoData]
        public void GetTestingsValidator_failed_if_frontend_and_backend_frameworks_are_null_or_empty(GetTestingsValidator validator)
        {
            var command = _fixture
                            .Build<GetTestingsCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .TestValidate(command)
                .ShouldHaveAnyValidationError()
                .WithErrorMessage(StringRes.BadReqNoBackendOrFrontend);
        }

        [Theory]
        [AutoData]
        public void GetTestingsValidator_is_valid_if_contain_frontend_or_backend_framework(GetTestingsValidator validator)
        {
            var command_without_frontend_framework = _fixture
                            .Build<GetTestingsCommand>()
                            .With(x => x.FrontendFramework, string.Empty)
                            .Create();

            var command_without_backend_framework = _fixture
                            .Build<GetTestingsCommand>()
                            .With(x => x.BackendFramework, string.Empty)
                            .Create();

            validator
                .ShouldNotHaveValidationErrorFor(t => t.FrontendFramework, command_without_frontend_framework);

            validator
                .ShouldNotHaveValidationErrorFor(t => t.BackendFramework, command_without_backend_framework);
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

        private void ClearFakeGenContext() => GenContext.ToolBox = null;
    }
}
