﻿Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.SourceMeasure

        Public Const AssemblyTitle As String = "VI Source Measure Library"
        Public Const AssemblyDescription As String = "Source Measure Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.Source.Measure.2018"

    End Class

End Namespace

