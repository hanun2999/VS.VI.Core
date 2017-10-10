﻿Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = 1 ' top level identifier. isr.VI.My.ProjectTraceEventId.K3700Console

        Public Const AssemblyTitle As String = "K3700 Driver Console"
        Public Const AssemblyDescription As String = "K3700 Switching Mainframe Virtual Instrument Console"
        Public Const AssemblyProduct As String = "VI.K3700.Console.2017"

        ''' <summary> Gets the identify date. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <value> The identify date. </value>
        Public Shared Property IdentifyDate As Date

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
            If DateTime.Now.Date > MyApplication.IdentifyDate AndAlso talker.Listeners.ContainsListener(isr.Core.Pith.ListenerType.Logger) Then
                talker.Publish(TraceEventType.Information, MyApplication.TraceEventId, MyApplication.Identity)
                MyApplication.IdentifyDate = DateTime.Now.Date
            End If
        End Sub

        ''' <summary> Gets the identity. </summary>
        ''' <value> The identity. </value>
        Public Shared ReadOnly Property Identity() As String
            Get
                Return $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}"
            End Get
        End Property

    End Class

End Namespace

