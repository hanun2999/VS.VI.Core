﻿
Partial Friend NotInheritable Class TestInfo

    Partial Public Class TraceMessageQueueCollection
        Inherits ObjectModel.Collection(Of isr.Core.Pith.TraceMessagesQueue)
        Public Sub New()
            MyBase.New
            Me.Add(TestInfo.TraceMessagesQueueListener)
            Me.Add(isr.Core.Pith.My.MyLibrary.UnpublishedTraceMessages)
        End Sub
    End Class

End Class
