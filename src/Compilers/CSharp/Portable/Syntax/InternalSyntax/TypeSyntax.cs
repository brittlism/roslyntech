// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

/// <summary>Provides the base class from which the classes that represent type syntax nodes are derived. This is an abstract class.</summary>
namespace Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax
{
    internal abstract partial class TypeSyntax : ExpressionSyntax
    {
        public bool IsVar => IsIdentifierName("var");
        public bool IsUnmanaged => IsIdentifierName("unmanaged");
        public bool IsNotNull => IsIdentifierName("notnull");
        public bool IsNint => IsIdentifierName("nint");
        public bool IsNuint => IsIdentifierName("nuint");

        private bool IsIdentifierName(string id) => this is IdentifierNameSyntax name && name.Identifier.ToString() == id;

        public bool IsRef => Kind == SyntaxKind.RefType;

        internal TypeSyntax(SyntaxKind kind, DiagnosticInfo[]? diagnostics, SyntaxAnnotation[]? annotations)
          : base(kind, diagnostics, annotations)
        {
        }

        internal TypeSyntax(SyntaxKind kind)
          : base(kind)
        {
        }
    }
}
