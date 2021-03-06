' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Roslyn.Test.Utilities
Imports Xunit

Namespace Microsoft.CodeAnalysis.Editor.VisualBasic.UnitTests.Recommendations.EventHandling
    Public Class AddHandlerKeywordRecommenderTests
        <Fact>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerHelpText()
            VerifyRecommendationDescriptionTextIs(<MethodBody>|</MethodBody>, "AddHandler",
                                                  <Text><![CDATA[
AddHandler statement
Associates an event with an event handler, delegate or lambda expression at run time.
AddHandler <event>, <handler>]]></Text>)
        End Sub

        <Fact>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerInMethodBody()
            VerifyRecommendationsContain(<MethodBody>|</MethodBody>, "AddHandler")
        End Sub

        <Fact>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerAfterStatement()
            VerifyRecommendationsContain(<MethodBody>
Dim x 
|</MethodBody>, "AddHandler")
        End Sub

        <Fact>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerMissingInClassBlock()
            VerifyRecommendationsMissing(<ClassDeclaration>|</ClassDeclaration>, "AddHandler")
        End Sub

        <Fact>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerInSingleLineLambda()
            VerifyRecommendationsContain(<MethodBody>Dim x = Sub() |</MethodBody>, "AddHandler")
        End Sub

        <Fact>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerInSingleLineFunctionLambda()
            VerifyRecommendationsMissing(<MethodBody>Dim x = Function() |</MethodBody>, "AddHandler")
        End Sub

        <Fact>
        <WorkItem(808406)>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub AddHandlerInCustomEvent()
            Dim code = <File>
Public Class Z
    Public Custom Event E As Action
       |
    End Event
End Class</File>

            VerifyRecommendationsContain(code, "AddHandler")
        End Sub

        <Fact>
        <WorkItem(808406)>
        <Trait(Traits.Feature, Traits.Features.KeywordRecommending)>
        Public Sub NotAddHandlerInCustomEventWithAddHandler()
            Dim code = <File>
Public Class Z
    Public Custom Event E As Action
        AddHandler(z as Action)
        End AddHandler
       |
    End Event
End Class</File>

            VerifyRecommendationsMissing(code, "AddHandler")
        End Sub
    End Class
End Namespace
