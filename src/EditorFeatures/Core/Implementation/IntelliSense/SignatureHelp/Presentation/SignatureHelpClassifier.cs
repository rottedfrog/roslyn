// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Editor.Shared.Extensions;
using Microsoft.CodeAnalysis.Editor.Shared.Utilities;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.Editor.Implementation.IntelliSense.SignatureHelp.Presentation
{
    internal class SignatureHelpClassifier : IClassifier
    {
        private readonly ITextBuffer _subjectBuffer;
        private readonly ClassificationTypeMap _typeMap;

#pragma warning disable 67
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67

        public SignatureHelpClassifier(ITextBuffer subjectBuffer, ClassificationTypeMap typeMap)
        {
            _subjectBuffer = subjectBuffer;
            _typeMap = typeMap;
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            ISignatureHelpSession session;
            if (_subjectBuffer.Properties.TryGetProperty(typeof(ISignatureHelpSession), out session) &&
                session.SelectedSignature is Signature)
            {
                var signature = (Signature)session.SelectedSignature;

                bool usePrettyPrintedContent;
                if (!_subjectBuffer.Properties.TryGetProperty("UsePrettyPrintedContent", out usePrettyPrintedContent))
                {
                    usePrettyPrintedContent = false;
                }

                var content = usePrettyPrintedContent
                    ? signature.PrettyPrintedContent
                    : signature.Content;

                var displayParts = usePrettyPrintedContent
                    ? signature.PrettyPrintedDisplayParts
                    : signature.DisplayParts;

                if (content == _subjectBuffer.CurrentSnapshot.GetText())
                {
                    return displayParts.ToClassificationSpans(span.Snapshot, _typeMap);
                }
            }

            return SpecializedCollections.EmptyList<ClassificationSpan>();
        }
    }
}
