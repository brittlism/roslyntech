// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.CSharp.Syntax
{
    /// <summary>Base type for parameter list syntax.</summary>
    public abstract partial class BaseParameterListSyntax : CSharpSyntaxNode
    {
        internal BaseParameterListSyntax(InternalSyntax.CSharpSyntaxNode green, SyntaxNode? parent, int position)
          : base(green, parent, position)
        {
        }

        /// <summary>Gets the parameter list.</summary>
        public abstract SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public BaseParameterListSyntax WithParameters(SeparatedSyntaxList<ParameterSyntax> parameters) => WithParametersCore(parameters);
        internal abstract BaseParameterListSyntax WithParametersCore(SeparatedSyntaxList<ParameterSyntax> parameters);

        public BaseParameterListSyntax AddParameters(params ParameterSyntax[] items) => AddParametersCore(items);
        internal abstract BaseParameterListSyntax AddParametersCore(params ParameterSyntax[] items);
    }

    /// <summary>Parameter list syntax.</summary>
    /// <remarks>
    /// <para>This node is associated with the following syntax kinds:</para>
    /// <list type="bullet">
    /// <item><description><see cref="SyntaxKind.ParameterList"/></description></item>
    /// </list>
    /// </remarks>
    public sealed partial class ParameterListSyntax : BaseParameterListSyntax
    {
        private SyntaxNode? parameters;

        internal ParameterListSyntax(InternalSyntax.CSharpSyntaxNode green, SyntaxNode? parent, int position)
          : base(green, parent, position)
        {
        }

        /// <summary>Gets the open paren token.</summary>
        public SyntaxToken OpenParenToken => new SyntaxToken(this, ((InternalSyntax.ParameterListSyntax)this.Green).openParenToken, Position, 0);

        public override SeparatedSyntaxList<ParameterSyntax> Parameters
        {
            get
            {
                var red = GetRed(ref this.parameters, 1);
                return red != null ? new SeparatedSyntaxList<ParameterSyntax>(red, GetChildIndex(1)) : default;
            }
        }
        internal int ParameterCount
        {
            get
            {
                int count = 0;
                foreach (ParameterSyntax parameter in this.Parameters)
                    // __arglist does not affect the parameter count.
                    if (!parameter.IsArgList)
                        count++;
                return count;
            }
        }
        /// <summary>Gets the close paren token.</summary>
        public SyntaxToken CloseParenToken => new SyntaxToken(this, ((InternalSyntax.ParameterListSyntax)this.Green).closeParenToken, GetChildPosition(2), GetChildIndex(2));

        internal override SyntaxNode? GetNodeSlot(int index) => index == 1 ? GetRed(ref this.parameters, 1)! : null;

        internal override SyntaxNode? GetCachedSlot(int index) => index == 1 ? this.parameters : null;

        public override void Accept(CSharpSyntaxVisitor visitor) => visitor.VisitParameterList(this);
        public override TResult? Accept<TResult>(CSharpSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitParameterList(this);

        public ParameterListSyntax Update(SyntaxToken openParenToken, SeparatedSyntaxList<ParameterSyntax> parameters, SyntaxToken closeParenToken)
        {
            if (openParenToken != this.OpenParenToken || parameters != this.Parameters || closeParenToken != this.CloseParenToken)
            {
                var newNode = SyntaxFactory.ParameterList(openParenToken, parameters, closeParenToken);
                var annotations = GetAnnotations();
                return annotations?.Length > 0 ? newNode.WithAnnotations(annotations) : newNode;
            }

            return this;
        }

        public ParameterListSyntax WithOpenParenToken(SyntaxToken openParenToken) => Update(openParenToken, this.Parameters, this.CloseParenToken);
        internal override BaseParameterListSyntax WithParametersCore(SeparatedSyntaxList<ParameterSyntax> parameters) => WithParameters(parameters);
        public new ParameterListSyntax WithParameters(SeparatedSyntaxList<ParameterSyntax> parameters) => Update(this.OpenParenToken, parameters, this.CloseParenToken);
        public ParameterListSyntax WithCloseParenToken(SyntaxToken closeParenToken) => Update(this.OpenParenToken, this.Parameters, closeParenToken);

        internal override BaseParameterListSyntax AddParametersCore(params ParameterSyntax[] items) => AddParameters(items);
        public new ParameterListSyntax AddParameters(params ParameterSyntax[] items) => WithParameters(this.Parameters.AddRange(items));
    }

    /// <summary>Parameter list syntax with surrounding brackets.</summary>
    /// <remarks>
    /// <para>This node is associated with the following syntax kinds:</para>
    /// <list type="bullet">
    /// <item><description><see cref="SyntaxKind.BracketedParameterList"/></description></item>
    /// </list>
    /// </remarks>
    public sealed partial class BracketedParameterListSyntax : BaseParameterListSyntax
    {
        private SyntaxNode? parameters;

        internal BracketedParameterListSyntax(InternalSyntax.CSharpSyntaxNode green, SyntaxNode? parent, int position)
          : base(green, parent, position)
        {
        }

        /// <summary>Gets the open bracket token.</summary>
        public SyntaxToken OpenBracketToken => new SyntaxToken(this, ((InternalSyntax.BracketedParameterListSyntax)this.Green).openBracketToken, Position, 0);
        
        public override SeparatedSyntaxList<ParameterSyntax> Parameters
        {
            get
            {
                var red = GetRed(ref this.parameters, 1);
                return red != null ? new SeparatedSyntaxList<ParameterSyntax>(red, GetChildIndex(1)) : default;
            }
        }

        /// <summary>Gets the close bracket token.</summary>
        public SyntaxToken CloseBracketToken => new SyntaxToken(this, ((InternalSyntax.BracketedParameterListSyntax)this.Green).closeBracketToken, GetChildPosition(2), GetChildIndex(2));

        internal override SyntaxNode? GetNodeSlot(int index) => index == 1 ? GetRed(ref this.parameters, 1)! : null;

        internal override SyntaxNode? GetCachedSlot(int index) => index == 1 ? this.parameters : null;

        public override void Accept(CSharpSyntaxVisitor visitor) => visitor.VisitBracketedParameterList(this);
        public override TResult? Accept<TResult>(CSharpSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitBracketedParameterList(this);

        public BracketedParameterListSyntax Update(SyntaxToken openBracketToken, SeparatedSyntaxList<ParameterSyntax> parameters, SyntaxToken closeBracketToken)
        {
            if (openBracketToken != this.OpenBracketToken || parameters != this.Parameters || closeBracketToken != this.CloseBracketToken)
            {
                var newNode = SyntaxFactory.BracketedParameterList(openBracketToken, parameters, closeBracketToken);
                var annotations = GetAnnotations();
                return annotations?.Length > 0 ? newNode.WithAnnotations(annotations) : newNode;
            }

            return this;
        }

        public BracketedParameterListSyntax WithOpenBracketToken(SyntaxToken openBracketToken) => Update(openBracketToken, this.Parameters, this.CloseBracketToken);
        internal override BaseParameterListSyntax WithParametersCore(SeparatedSyntaxList<ParameterSyntax> parameters) => WithParameters(parameters);
        public new BracketedParameterListSyntax WithParameters(SeparatedSyntaxList<ParameterSyntax> parameters) => Update(this.OpenBracketToken, parameters, this.CloseBracketToken);
        public BracketedParameterListSyntax WithCloseBracketToken(SyntaxToken closeBracketToken) => Update(this.OpenBracketToken, this.Parameters, closeBracketToken);

        internal override BaseParameterListSyntax AddParametersCore(params ParameterSyntax[] items) => AddParameters(items);
        public new BracketedParameterListSyntax AddParameters(params ParameterSyntax[] items) => WithParameters(this.Parameters.AddRange(items));
    }
}
