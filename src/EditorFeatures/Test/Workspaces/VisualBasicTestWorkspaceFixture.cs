// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;

namespace Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces
{
    public class VisualBasicTestWorkspaceFixture : TestWorkspaceFixture
    {
        public VisualBasicTestWorkspaceFixture()
        {
            this.Workspace = VisualBasicWorkspaceFactory.CreateWorkspaceFromFiles(
                new string[] { string.Empty },
                new VisualBasicParseOptions[] { new VisualBasicParseOptions(kind: SourceCodeKind.Regular) },
                new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }
    }
}
