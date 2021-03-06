﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports System.Collections.Immutable
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.CodeAnalysis.RuntimeMembers
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Roslyn.Utilities
Imports TypeKind = Microsoft.CodeAnalysis.TypeKind

Namespace Microsoft.CodeAnalysis.VisualBasic

    ''' <summary>
    ''' A source module binder provides the context associated with a source module.
    ''' </summary>
    Friend Class SourceModuleBinder
        Inherits Binder

        Private m_sourceModule As SourceModuleSymbol

        Public Sub New(containingBinder As Binder, sourceModule As SourceModuleSymbol)
            MyBase.New(containingBinder, sourceModule, sourceModule.ContainingSourceAssembly.DeclaringCompilation)
            m_sourceModule = sourceModule
        End Sub

        Public Overrides Function CheckAccessibility(sym As Symbol,
                                                     <[In], Out> ByRef useSiteDiagnostics As HashSet(Of DiagnosticInfo),
                                                     Optional accessThroughType As TypeSymbol = Nothing,
                                                     Optional basesBeingResolved As ConsList(Of Symbol) = Nothing) As AccessCheckResult
            Return If(IgnoresAccessibility,
                AccessCheckResult.Accessible,
                AccessCheck.CheckSymbolAccessibility(sym, m_sourceModule.ContainingSourceAssembly, useSiteDiagnostics, basesBeingResolved))  ' accessThroughType doesn't matter at assembly level.
        End Function

        Public Overrides Function GetErrorSymbol(name As String,
                                                 errorInfo As DiagnosticInfo,
                                                 candidateSymbols As ImmutableArray(Of Symbol),
                                                 resultKind As LookupResultKind) As ErrorTypeSymbol
            Return New ExtendedErrorTypeSymbol(errorInfo, name, 0, candidateSymbols, resultKind)
        End Function

        Public Overrides ReadOnly Property OptionStrict As OptionStrict
            Get
                Return m_sourceModule.Options.OptionStrict
            End Get
        End Property

        Public Overrides ReadOnly Property OptionInfer As Boolean
            Get
                Return m_sourceModule.Options.OptionInfer
            End Get
        End Property

        Public Overrides ReadOnly Property OptionExplicit As Boolean
            Get
                Return m_sourceModule.Options.OptionExplicit
            End Get
        End Property

        Public Overrides ReadOnly Property OptionCompareText As Boolean
            Get
                Return m_sourceModule.Options.OptionCompareText
            End Get
        End Property

        Public Overrides ReadOnly Property CheckOverflow As Boolean
            Get
                Return m_sourceModule.Options.CheckOverflow
            End Get
        End Property

        Public Overrides ReadOnly Property QuickAttributeChecker As QuickAttributeChecker
            Get
                Return m_sourceModule.QuickAttributeChecker
            End Get
        End Property
    End Class

End Namespace
