﻿using System;
using Microsoft.Languages.Core.Text;
using Microsoft.Languages.Editor;
using Microsoft.Languages.Editor.Controller.Command;
using Microsoft.Languages.Editor.Controller.Constants;
using Microsoft.R.Core.AST;
using Microsoft.R.Editor.Document;
using Microsoft.R.Editor.Document.Definitions;
using Microsoft.R.Editor.Settings;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.R.Editor.Formatting
{
    internal class FormatSelectionCommand : EditingCommand
    {
        ITextBuffer _textBuffer;

        internal FormatSelectionCommand(ITextView textView, ITextBuffer textBuffer)
            : base(textView, new CommandId(VSConstants.VSStd2K, (int)VSConstants.VSStd2KCmdID.FORMATSELECTION))
        {
            _textBuffer = textBuffer;
        }

        #region ICommand
        public override CommandResult Invoke(Guid group, int id, object inputArg, ref object outputArg)
        {
            SnapshotSpan spanToFormat = TextView.Selection.StreamSelectionSpan.SnapshotSpan;

            IREditorDocument document = REditorDocument.TryFromTextBuffer(_textBuffer);
            AstRoot ast;
            if (document == null)
            {
                // For unit test purposes
                ast = inputArg as AstRoot;
            }
            else
            {
                ast = document.EditorTree.AstRoot;
            }

            if (ast != null)
            {
                RangeFormatter.FormatRange(TextView,
                                           new TextRange(spanToFormat.Start.Position, spanToFormat.Length),
                                           ast, REditorSettings.FormatOptions);
            }

            return new CommandResult(CommandStatus.Supported, 0);
        }

        public override CommandStatus Status(Guid group, int id)
        {
            if (TextView.Selection.Mode == TextSelectionMode.Box)
                return CommandStatus.NotSupported;

            return CommandStatus.SupportedAndEnabled;
        }
        #endregion
    }
}
