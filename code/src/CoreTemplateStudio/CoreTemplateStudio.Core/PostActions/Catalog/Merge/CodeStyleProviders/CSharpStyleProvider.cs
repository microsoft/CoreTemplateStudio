// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders
{
    public class CSharpStyleProvider : BaseCodeStyleProvider
    {
        private const string OpeningBrace = "{";
        private const string ClosingBrace = "}";

        public override string CommentSymbol => "//";

        public override List<string> AdaptInsertionBlock(List<string> insertionBuffer, string lastContextLine, string nextContextLine)
        {
            var buffer = base.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            AdaptWhiteLinesToBraces(buffer, lastContextLine, nextContextLine);

            return buffer;
        }

        private static void AdaptWhiteLinesToBraces(List<string> insertionBuffer, string lastContextLine, string nextContextLine)
        {
            if (lastContextLine.Trim().Equals(OpeningBrace) && insertionBuffer.First().Trim() == string.Empty)
            {
                insertionBuffer.RemoveAt(0);
            }

            if (lastContextLine.Trim().Equals(ClosingBrace) && insertionBuffer.First().Trim() != string.Empty)
            {
                insertionBuffer.Insert(0, string.Empty);
            }

            if (nextContextLine.Trim().Equals(ClosingBrace) && insertionBuffer.Last().Trim() == string.Empty)
            {
                insertionBuffer.RemoveAt(insertionBuffer.Count - 1);
            }

            if (!nextContextLine.Trim().Equals(ClosingBrace) && !nextContextLine.Trim().Equals(string.Empty) && insertionBuffer.Last().Trim().Equals(ClosingBrace))
            {
                insertionBuffer.Add(string.Empty);
            }
        }
    }
}
