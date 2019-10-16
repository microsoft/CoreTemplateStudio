// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeHandler
    {
        internal const string MacroBeforeMode = "^^";
        internal const string MacroStartGroup = "{[{";
        internal const string MacroEndGroup = "}]}";

        internal const string MacroStartDocumentation = "{**";
        internal const string MacroEndDocumentation = "**}";

        internal const string MacroStartDelete = "{--{";
        internal const string MacroEndDelete = "}--}";

        internal const string MacroStartOptionalContext = "{??{";
        internal const string MacroEndOptionalContext = "}??}";

        private static string[] macros = new string[] { MacroBeforeMode, MacroStartGroup, MacroEndGroup, MacroStartDocumentation, MacroEndDocumentation, MacroStartDelete, MacroEndDelete, MacroStartOptionalContext, MacroEndOptionalContext };

        private const string OpeningBrace = "{";
        private const string ClosingBrace = "}";

        private const string OpeningParentesis = "(";

        private const string CSharpComment = "// ";
        private const string VBComment = "' ";

        private MergeMode mergeMode = MergeMode.Context;

        private List<string> result;

        private bool insertBefore = false;

        private List<string> insertionBuffer = new List<string>();

        private List<string> removalBuffer = new List<string>();

        private int currentContextLineIndex = -1;

        private int lastContextLineIndex = -1;

        public IEnumerable<string> Merge(IEnumerable<string> source, IEnumerable<string> merge, BaseCodeStyleProvider codeFormatter, out string errorLine)
        {
            errorLine = string.Empty;
            result = source.ToList();

            var documentationEndIndex = merge.SafeIndexOf(merge.FirstOrDefault(m => m.Contains(MacroEndDocumentation)), 0);

            var diffTrivia = source.FindDiffLeadingTrivia(merge, documentationEndIndex);

            foreach (var mergeLine in merge)
            {
                // try to find context line
                if (mergeMode == MergeMode.Context || mergeMode == MergeMode.OptionalContext || mergeMode == MergeMode.Remove)
                {
                    if (mergeLine.Contains(codeFormatter.InlineCommentStart + MacroStartGroup + codeFormatter.InlineCommentEnd) && mergeLine.Contains(codeFormatter.InlineCommentStart + MacroEndGroup + codeFormatter.InlineCommentEnd))
                    {
                        currentContextLineIndex = FindAndModifyContextLine(mergeLine, diffTrivia, codeFormatter);
                    }
                    else
                    {
                        currentContextLineIndex = FindContextLine(mergeLine, diffTrivia);
                    }
                }

                // if line is found, add buffer if any
                if (currentContextLineIndex > -1)
                {
                    var linesAdded = TryAddBufferContent(codeFormatter);
                    var linesRemoved = TryRemoveBufferContent();

                    lastContextLineIndex = currentContextLineIndex + linesAdded - linesRemoved;

                    CleanBuffers();
                }

                // get new merge direction or add to buffer
                if (IsMergeDirection(mergeLine))
                {
                    mergeMode = GetMergeMode(mergeLine, mergeMode, codeFormatter?.CommentSymbol);
                }
                else
                {
                    switch (mergeMode)
                    {
                        case MergeMode.InsertBefore:
                        case MergeMode.Insert:
                            insertionBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                            break;
                        case MergeMode.Remove:
                            removalBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                            break;
                        case MergeMode.Context:
                            if (mergeLine == string.Empty)
                            {
                                insertionBuffer.Add(mergeLine);
                            }
                            else if (currentContextLineIndex == -1)
                            {
                                errorLine = mergeLine;
                                return source;
                            }

                            break;
                    }
                }
            }

            // Add remaining buffers before finishing
            TryAddBufferContent(codeFormatter);
            TryRemoveBufferContent();

            return result;
        }

        private void CleanBuffers()
        {
            insertBefore = false;
            insertionBuffer.Clear();
            removalBuffer.Clear();
        }

        private int TryAddBufferContent(BaseCodeStyleProvider codeFormatter)
        {
            if (insertionBuffer.Any() && !BlockExists(insertionBuffer, result, lastContextLineIndex))
            {
                var insertIndex = GetInsertLineIndex(currentContextLineIndex, lastContextLineIndex, insertBefore);

                if (insertIndex <= result.Count && insertIndex > -1)
                {
                    var lastContextLine = insertIndex != 0 ? result[insertIndex - 1] : string.Empty;
                    var nextContextLine = insertIndex != result.Count() ? result[insertIndex] : string.Empty;

                    if (codeFormatter != null)
                    {
                        insertionBuffer = codeFormatter.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);
                    }

                    result.InsertRange(insertIndex, insertionBuffer);
                    return insertionBuffer.Count;
                }
            }

            return 0;
        }

        private int TryRemoveBufferContent()
        {
            if (removalBuffer.Any() && BlockExists(removalBuffer, result, lastContextLineIndex))
            {
                var index = result.SafeIndexOf(removalBuffer[0], lastContextLineIndex, false);
                if (index <= currentContextLineIndex && index > -1)
                {
                    result.RemoveRange(index, removalBuffer.Count);
                    return removalBuffer.Count;
                }
            }

            return 0;
        }

        private static bool IsMergeDirection(string mergeLine)
        {
            return macros.Any(c => mergeLine.Contains(c));
        }

        private MergeMode GetMergeMode(string mergeLine, MergeMode current, string commentSymbol)
        {
            if (mergeLine.Trim().StartsWith(commentSymbol + MacroBeforeMode))
            {
                return MergeMode.InsertBefore;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroStartGroup))
            {
                if (current == MergeMode.InsertBefore)
                {
                    insertBefore = true;
                    return MergeMode.InsertBefore;
                }
                else
                {
                    return MergeMode.Insert;
                }
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroEndGroup))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroStartDocumentation))
            {
                return MergeMode.Documentation;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroEndDocumentation))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroStartDelete))
            {
                return MergeMode.Remove;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroEndDelete))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroStartOptionalContext))
            {
                return MergeMode.OptionalContext;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MacroEndOptionalContext))
            {
                return MergeMode.Context;
            }

            return current;
        }

        private static int GetInsertLineIndex(int currentLine, int lastLine, bool beforeMode)
        {
            if (beforeMode)
            {
                return currentLine;
            }
            else
            {
                return lastLine + 1;
            }
        }

        private int FindContextLine(string mergeLine, int diffTrivia)
        {
            return result.SafeIndexOf(mergeLine.WithLeadingTrivia(diffTrivia), lastContextLineIndex);
        }

        private int FindAndModifyContextLine(string mergeLine, int diffTrivia, BaseCodeStyleProvider formatter)
        {
            var macroInlineAdditionStart = formatter.InlineCommentStart + MacroStartGroup + formatter.InlineCommentEnd;
            var macroInlineAdditionEnd = formatter.InlineCommentStart + MacroEndGroup + formatter.InlineCommentEnd;

            var mergeLineStart = mergeLine.Substring(0, mergeLine.IndexOf(macroInlineAdditionStart));
            var mergeLineEnd = mergeLine.Substring(mergeLine.IndexOf(macroInlineAdditionEnd) + macroInlineAdditionEnd.Length, mergeLine.Length - mergeLine.IndexOf(macroInlineAdditionEnd) - macroInlineAdditionEnd.Length);

            var additionStartIndex = mergeLine.IndexOf(macroInlineAdditionStart) + macroInlineAdditionStart.Length;
            var additionEndIndex = mergeLine.IndexOf(macroInlineAdditionEnd);

            var addition = mergeLine.Substring(additionStartIndex, additionEndIndex - additionStartIndex);
            var lineIndex = result.SafeIndexOf(mergeLineStart.WithLeadingTrivia(diffTrivia), lastContextLineIndex, true, true);
            var insertIndex = result[lineIndex].Length;

            if (lineIndex != -1 && !result[lineIndex].Contains(addition))
            {
                if (formatter != null)
                {
                    var nextChar = string.Empty;

                    if (mergeLineEnd != string.Empty)
                    {
                        insertIndex = result[lineIndex].IndexOf(mergeLineEnd);
                        nextChar = mergeLineEnd.Substring(1);
                    }

                    // Get complete line content until insert index
                    var lineStart = insertIndex > 0 ? result[lineIndex].Substring(0, insertIndex) : string.Empty;

                    addition = formatter.AdaptInlineAddition(addition, lineStart, mergeLineEnd);
                }

                result[lineIndex] = result[lineIndex].Insert(insertIndex, addition);
            }

            return lineIndex;
        }

        private static bool BlockExists(IEnumerable<string> blockBuffer, IEnumerable<string> target, int skip)
        {
            return blockBuffer
                        .Where(b => !string.IsNullOrWhiteSpace(b))
                        .All(b => target.SafeIndexOf(b, skip) > -1);
        }
    }
}
