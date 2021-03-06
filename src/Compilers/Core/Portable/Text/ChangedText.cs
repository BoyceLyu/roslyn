﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace Microsoft.CodeAnalysis.Text
{
    internal sealed class ChangedText : SourceText
    {
        private readonly SourceText _oldText;
        private readonly SourceText _newText;
        private readonly ImmutableArray<TextChangeRange> _changes;

        public ChangedText(SourceText oldText, ImmutableArray<TextChangeRange> changeRanges, ImmutableArray<SourceText> segments)
            : base(checksumAlgorithm: oldText.ChecksumAlgorithm)
        {
            Debug.Assert(oldText != null);
            Debug.Assert(!changeRanges.IsDefault);
            Debug.Assert(!segments.IsDefault);

            _oldText = oldText;
            _newText = segments.IsEmpty ? new StringText("", oldText.Encoding, checksumAlgorithm: oldText.ChecksumAlgorithm) : (SourceText)new CompositeText(segments);
            _changes = changeRanges;
        }

        public override Encoding Encoding
        {
            get { return _oldText.Encoding; }
        }

        public SourceText OldText
        {
            get { return _oldText; }
        }

        public SourceText NewText
        {
            get { return _newText; }
        }

        public IEnumerable<TextChangeRange> Changes
        {
            get { return _changes; }
        }

        public override int Length
        {
            get { return _newText.Length; }
        }

        public override char this[int position]
        {
            get { return _newText[position]; }
        }

        public override string ToString(TextSpan span)
        {
            return _newText.ToString(span);
        }

        public override SourceText GetSubText(TextSpan span)
        {
            return _newText.GetSubText(span);
        }

        public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            _newText.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public override IReadOnlyList<TextChangeRange> GetChangeRanges(SourceText oldText)
        {
            if (oldText == null)
            {
                throw new ArgumentNullException("oldText");
            }

            if (ReferenceEquals(_oldText, oldText))
            {
                // check whether the bases are same one
                return _changes;
            }

            if (_oldText.GetChangeRanges(oldText).Count == 0)
            {
                // okay, the bases are different, but the contents might be same.
                return _changes;
            }

            if (this == oldText)
            {
                return TextChangeRange.NoChanges;
            }

            return ImmutableArray.Create(new TextChangeRange(new TextSpan(0, oldText.Length), _newText.Length));
        }
    }
}
