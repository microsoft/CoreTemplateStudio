// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders
{
    public class BaseCodeStyleProvider
    {
        private List<string> contextCharsThatNeedCommaSeparatorForAdditions = new List<string>() { "(", ":" };

        public virtual string CommentSymbol => "//";

        public virtual List<string> AdaptInsertionBlock(List<string> insertionBuffer, string lastContextLine, string nextContextLine)
        {
            EnsureWhiteLineBeforeComment(insertionBuffer, nextContextLine);

            return insertionBuffer;
        }

        public virtual string AdaptInlineAddition(string addition, string lastContextChar, string lastChar, string nextChar)
        {
            if (contextCharsThatNeedCommaSeparatorForAdditions.Contains(lastContextChar))
            {
                addition = EnsureCommaSeparator(addition, lastChar, nextChar);
            }

            return addition;
        }

        private void EnsureWhiteLineBeforeComment(List<string> insertionBuffer, string nextContextLine)
        {
            if (nextContextLine.Trim().StartsWith(CommentSymbol + string.Empty) && insertionBuffer.Last().Trim() != string.Empty)
            {
                insertionBuffer.Add(string.Empty);
            }
        }

        private string EnsureCommaSeparator(string addition, string lastChar, string nextChar)
        {
            if (!contextCharsThatNeedCommaSeparatorForAdditions.Contains(lastChar) && !addition.StartsWith(", "))
            {
                return addition = $", {addition}";
            }

            return addition;
        }
    }
}
