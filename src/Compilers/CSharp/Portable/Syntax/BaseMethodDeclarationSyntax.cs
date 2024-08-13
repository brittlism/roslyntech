// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.CodeAnalysis.CSharp.Syntax
{
    /// <summary>Base type for method declaration syntax.</summary>
    public abstract partial class BaseMethodDeclarationSyntax : MemberDeclarationSyntax
    {
        internal BaseMethodDeclarationSyntax(InternalSyntax.CSharpSyntaxNode green, SyntaxNode? parent, int position)
          : base(green, parent, position) { }

        public abstract override SyntaxList<AttributeListSyntax> AttributeLists { get; }
        public abstract override SyntaxTokenList Modifiers { get; }

        public bool isExten => ParameterList.Parameters[0].Modifiers.Any(SyntaxKind.ThisKeyword);
        public abstract ParameterListSyntax ParameterList { get; }
        public BaseMethodDeclarationSyntax WithParameterList(ParameterListSyntax parameterList) => WithParameterListCore(parameterList);
        internal abstract BaseMethodDeclarationSyntax WithParameterListCore(ParameterListSyntax parameterList);

        public BaseMethodDeclarationSyntax AddParameterListParameters(params ParameterSyntax[] items) => AddParameterListParametersCore(items);
        internal abstract BaseMethodDeclarationSyntax AddParameterListParametersCore(params ParameterSyntax[] items);

        public abstract BlockSyntax? Body { get; }
        public BaseMethodDeclarationSyntax WithBody(BlockSyntax? body) => WithBodyCore(body);
        internal abstract BaseMethodDeclarationSyntax WithBodyCore(BlockSyntax? body);

        public BaseMethodDeclarationSyntax AddBodyAttributeLists(params AttributeListSyntax[] items) => AddBodyAttributeListsCore(items);
        internal abstract BaseMethodDeclarationSyntax AddBodyAttributeListsCore(params AttributeListSyntax[] items);

        public BaseMethodDeclarationSyntax AddBodyStatements(params StatementSyntax[] items) => AddBodyStatementsCore(items);
        internal abstract BaseMethodDeclarationSyntax AddBodyStatementsCore(params StatementSyntax[] items);

        public abstract ArrowExpressionClauseSyntax? ExpressionBody { get; }
        public BaseMethodDeclarationSyntax WithExpressionBody(ArrowExpressionClauseSyntax? expressionBody) => WithExpressionBodyCore(expressionBody);
        internal abstract BaseMethodDeclarationSyntax WithExpressionBodyCore(ArrowExpressionClauseSyntax? expressionBody);

        /// <summary>Gets the optional semicolon token.</summary>
        public abstract SyntaxToken SemicolonToken { get; }
        public BaseMethodDeclarationSyntax WithSemicolonToken(SyntaxToken semicolonToken) => WithSemicolonTokenCore(semicolonToken);
        internal abstract BaseMethodDeclarationSyntax WithSemicolonTokenCore(SyntaxToken semicolonToken);

        public new BaseMethodDeclarationSyntax WithAttributeLists(SyntaxList<AttributeListSyntax> attributeLists) => (BaseMethodDeclarationSyntax)WithAttributeListsCore(attributeLists);
        public new BaseMethodDeclarationSyntax WithModifiers(SyntaxTokenList modifiers) => (BaseMethodDeclarationSyntax)WithModifiersCore(modifiers);

        public new BaseMethodDeclarationSyntax AddAttributeLists(params AttributeListSyntax[] items) => (BaseMethodDeclarationSyntax)AddAttributeListsCore(items);

        public new BaseMethodDeclarationSyntax AddModifiers(params SyntaxToken[] items) => (BaseMethodDeclarationSyntax)AddModifiersCore(items);
    }
}
