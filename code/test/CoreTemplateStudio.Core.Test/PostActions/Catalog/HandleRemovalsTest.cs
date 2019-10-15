// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("ExecutionSet", "Minimum")]
    public class HandleRemovalsTest
    {
        [Fact]
        public void HandlesSuccessful()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}",
            };
            var merge = new[]
            {
                "public void SomeMethod()",
                "{",
                "//{--{",
                "    yield break;",
                "//}--}",
                "}",
            };
            var expected = new[]
            {
                "public void SomeMethod()",
                "{",
                "}",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge, new CSharpStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SingleRemovalAndMerge()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}",
            };
            var merge1 = new[]
            {
                "public void SomeMethod()",
                "{",
                "    //{[{",
                "    // Merge1",
                "    //}]}",
                "//{--{",
                "    yield break;",
                "//}--}",
                "}",
            };
            var expected = new[]
            {
                "public void SomeMethod()",
                "{",
                "    // Merge1",
                "}",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge1, new CSharpStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultipleRemovalsAndMerges()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}",
            };
            var merge1 = new[]
            {
                "public void SomeMethod()",
                "{",
                "    //{[{",
                "    // Merge1",
                "    //}]}",
                "//{--{",
                "    yield break;",
                "//}--}",
                "}",
            };
            var merge2 = new[]
            {
                "public void SomeMethod()",
                "{",
                "    //{[{",
                "    // Merge2",
                "    //}]}",
                "//{--{",
                "    yield break;",
                "//}--}",
                "}",
            };
            var expected = new List<string>()
            {
                "public void SomeMethod()",
                "{",
                "    // Merge2",
                string.Empty,
                "    // Merge1",
                "}",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge1, new CSharpStyleProvider(), out var errorLine);

            var mergeHandler2 = new MergeHandler();
            result = mergeHandler2.Merge(result, merge2, new CSharpStyleProvider(), out errorLine).ToList();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NothingToRemove()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}",
            };
            var merge = new[]
            {
                "public void SomeOtherMethod()",
                "{",
                "    // Something unrelated to deletion",
                "}",
            };
            var expected = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge, new CSharpStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AlreadyRemoved()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "}",
            };
            var merge = new[]
            {
                "public void SomeMethod()",
                "{",
                "//{--{",
                "    yield break;",
                "//}--}",
                "}",
            };
            var expected = new[]
            {
                "public void SomeMethod()",
                "{",
                "}",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge, new CSharpStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void HandlesSuccessfulVB()
        {
            var source = new[]
            {
                "Public Sub SomeMethod()",
                "    Exit Sub",
                "End Sub",
            };
            var merge = new[]
            {
                "Public Sub SomeMethod()",
                "'{--{",
                "    Exit Sub",
                "'}--}",
                "End Sub",
            };
            var expected = new[]
            {
                "Public Sub SomeMethod()",
                "End Sub",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge, new VBStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SingleRemovalAndMergeVB()
        {
            var source = new[]
            {
                "Public Sub SomeMethod()",
                "    Exit Sub",
                "End Sub",
            };
            var merge1 = new[]
            {
                "Public Sub SomeMethod()",
                "    '{[{",
                "    ' Merge1",
                "    '}]}",
                "'{--{",
                "    Exit Sub",
                "'}--}",
                "End Sub",
            };
            var expected = new[]
            {
                "Public Sub SomeMethod()",
                "    ' Merge1",
                "End Sub",
            };

            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge1, new VBStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultipleRemovalsAndMergesVB()
        {
            var source = new[]
            {
                "Public Sub SomeMethod()",
                "    Exit Sub",
                "End Sub",
            };
            var merge1 = new[]
            {
                "Public Sub SomeMethod()",
                "    '{[{",
                "    ' Merge1",
                "    '}]}",
                "'{--{",
                "    Exit Sub",
                "'}--}",
                "End Sub",
            };
            var merge2 = new[]
            {
                "Public Sub SomeMethod()",
                "    '{[{",
                "    ' Merge2",
                "    '}]}",
                "'{--{",
                "    Exit Sub,",
                "'}--}",
                "End Sub",
            };
            var expected = new[]
            {
                "Public Sub SomeMethod()",
                "    ' Merge2",
                string.Empty,
                "    ' Merge1",
                "End Sub",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge1, new VBStyleProvider(), out var errorLine);

            var mergeHandler2 = new MergeHandler();
            result = mergeHandler2.Merge(result, merge2, new VBStyleProvider(), out errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NothingToRemoveVB()
        {
            var source = new[]
            {
                "Public Sub SomeMethod()",
                "    Exit Sub",
                "End Sub",
            };
            var merge = new[]
            {
                "Public Sub SomeOtherMethod()",
                "    ' Something unrelated to deletion",
                "End Sub",
            };
            var expected = new[]
            {
                "Public Sub SomeMethod()",
                "    Exit Sub",
                "End Sub",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge, new VBStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AlreadyRemovedVB()
        {
            var source = new[]
            {
                "Public Sub SomeMethod()",
                "End Sub",
            };
            var merge = new[]
            {
                "Public Sub SomeMethod()",
                "'{--{",
                "    Exit Sub",
                "'}--}",
                "End Sub",
            };
            var expected = new[]
            {
                "Public Sub SomeMethod()",
                "End Sub",
            };
            var mergeHandler = new MergeHandler();
            var result = mergeHandler.Merge(source, merge, new VBStyleProvider(), out var errorLine);

            Assert.Equal(expected, result);
        }
    }
}
