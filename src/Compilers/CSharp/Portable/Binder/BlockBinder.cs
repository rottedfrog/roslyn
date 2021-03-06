﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal sealed class BlockBinder : LocalScopeBinder
    {
        private readonly SyntaxList<StatementSyntax> _statements;

        public BlockBinder(Binder enclosing, SyntaxList<StatementSyntax> statements)
            : this(enclosing, statements, enclosing.Flags)
        {
        }

        public BlockBinder(Binder enclosing, SyntaxList<StatementSyntax> statements, BinderFlags additionalFlags)
            : base(enclosing, enclosing.Flags | additionalFlags)
        {
            _statements = statements;
        }

        protected override ImmutableArray<LocalSymbol> BuildLocals()
        {
            return BuildLocals(_statements);
        }

        protected override ImmutableArray<LabelSymbol> BuildLabels()
        {
            ArrayBuilder<LabelSymbol> labels = null;
            base.BuildLabels(_statements, ref labels);
            return (labels != null) ? labels.ToImmutableAndFree() : ImmutableArray<LabelSymbol>.Empty;
        }

        internal override ImmutableArray<LocalSymbol> GetDeclaredLocalsForScope(CSharpSyntaxNode node)
        {
            if (node.Kind() == SyntaxKind.Block)
            {
                if (((BlockSyntax)node).Statements == _statements)
                {
                    return this.Locals;
                }
            }
            else if (_statements.Count == 1 && _statements.First() == node)
            {
                return this.Locals;
            }

            throw ExceptionUtilities.Unreachable;
        }
    }
}
