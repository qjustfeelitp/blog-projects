using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ThreadSafeRangeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RA001";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
                                                                                     "RangeAttribute is not thread safe",
                                                                                     "The RangeAttribute is not thread-safe and should be replaced with ParsableRangeAttribute",
                                                                                     "ThreadSafety",
                                                                                     DiagnosticSeverity.Error,
                                                                                     true);

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.Attribute);
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ObjectCreationExpression);
        }

        /// <summary>
        /// Analyzes node for potential range attribute usage.
        /// </summary>
        /// <param name="context">Context</param>
        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (!(context.SemanticModel.GetSymbolInfo(context.Node).Symbol is IMethodSymbol symbol))
            {
                return;
            }

            var containingType = symbol.ContainingType;

            if (containingType.ToString()?.Equals("System.ComponentModel.DataAnnotations.RangeAttribute", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Create a diagnostic error if RangeAttribute is detected
                var diagnostic = Diagnostic.Create(Rule, context.Node.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
