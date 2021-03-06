﻿Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Instrument Interface Panel. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/07/2005" by="David" revision="2.0.2597.x"> Created. </history>
<CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
Public Class ResourceConsoleControl
    Inherits TalkerControlBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me.AddPrivateListeners()
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then

                ' removes the private text box listener
                Me.RemovePrivateListener(Me._TraceMessagesBox)

                ' removes the private listeners of the interface panel hosted in this form.
                Me._InterfacePanel?.RemovePrivateListeners()

                ' Free managed resources when explicitly called
                If Me.Session IsNot Nothing Then Me.CloseInstrumentSession()

                Me.StopPollTimer()

                If Me._InstrumentChooser IsNot Nothing Then Me._InstrumentChooser.Dispose() : Me._InstrumentChooser = Nothing

                ' unable to use null conditional because it is not seen by code analysis
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If

        Finally

            ' Invoke the base class dispose method
            MyBase.Dispose(disposing)

        End Try
    End Sub

#End Region

#Region " MODULE DATA MEMBERS "

    ''' <summary> Gets reference to the
    ''' <see cref="VI.Pith.SessionBase">message based session</see>.
    ''' Was Ivi.VI.Interop.IGpib. </summary>
    ''' <value> The session. </value>
    Private Property Session As VI.Pith.SessionBase

    ''' <summary> Gets the last message that was received from the instrument. </summary>
    ''' <value> A Buffer for receive data. </value>
    Private Property ReceiveBuffer As String

    ''' <summary> Gets the last message that was sent to the instrument. </summary>
    ''' <value> A Buffer for transmit data. </value>
    Private Property TransmitBuffer As String

#End Region

#Region " CONNECT / DISCONNECT "

    ''' <summary> Gets the condition for determining if the interface session is open (connected). </summary>
    ''' <value> <c>True</c> if a VISA session exists. </value>
    Private ReadOnly Property IsOpen() As Boolean
        Get
            Return Me.Session IsNot Nothing AndAlso Me.Session.IsSessionOpen
        End Get
    End Property

    ''' <summary> Opens a visa session to the instrument. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenInstrumentSession()

        Dim wasOpen As Boolean
        wasOpen = Me.IsOpen

        ' close the device if open
        If wasOpen Then
            Me.CloseInstrumentSession()
        End If

        Dim lastAction As String = "N/A"
        Try

            ' clear values
            Me.ReceiveBuffer = String.Empty
            Me.TransmitBuffer = String.Empty

            lastAction = "Initializing driver"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               $"{lastAction};. ".ToString(Globalization.CultureInfo.CurrentCulture))
            Dim resourceName As String = Me._InterfacePanel.InstrumentChooser.SessionFactory.SelectedResourceName

            lastAction = $"Opening a VISA Session to {resourceName}"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.Session = isr.VI.SessionFactory.Get.Factory.CreateSession()
            Me.Session.OpenSession(resourceName, Threading.SynchronizationContext.Current)

            If Me.IsOpen Then
                lastAction = "Clearing the device"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.Clear()
                lastAction = $"Connected to {Me.Session.ResourceName}"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "Failed opening a session to {0};. ", resourceName)
            End If

        Catch ex As Exception
            ex.Data.Add("@isr", lastAction)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{lastAction};. {ex.ToFullBlownString}")
            Try
                Me.CloseInstrumentSession()
            Finally
            End Try

        Finally

            ' check if ExecutionState changed.
            If wasOpen <> Me.IsOpen Then
                ' if so, alert on connection changed.
                Me.OnConnectionChanged()
            End If
            Me.Session.RestoreTimeout()

        End Try

    End Sub

    ''' <summary> Closes the device. Performs the necessary termination functions, which will cleanup
    ''' and disconnect the interface connection. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CloseInstrumentSession()

        Dim wasOpen As Boolean
        wasOpen = Me.IsOpen
        StopPollTimer()

        Dim lastAction As String = "N/A"
        Try

            If Me.IsOpen Then

                lastAction = "Disconnecting Instrument"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")

                If _SendDisconnectCommandsCheckBox.Checked Then

                    If Me._DisconnectCommandsTextBox.Lines.Length > 0 Then
                        For Each command As String In Me._DisconnectCommandsTextBox.Lines
                            command = command.Trim
                            If Not String.IsNullOrWhiteSpace(command) Then
                                lastAction = $"Sending '{command}'"
                                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                                Try
                                    Me.Session.WriteLine(command)
                                Catch ex As VI.Pith.NativeException
                                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                                       $"{lastAction} failed;. {ex.ToFullBlownString}")
                                End Try
                            End If
                        Next
                    End If
                End If

                lastAction = "Clearing the device"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.Clear()

                lastAction = "Disabling service request events if any"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.DisableServiceRequestEventHandler()

                lastAction = "Ending the VISA session"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.Dispose()
                Try
                    ' Trying to null the session raises an ObjectDisposedException 
                    ' if session service request handler was not released. 
                    Me.Session = Nothing
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                End Try

                lastAction = "Session closed"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")

            End If

        Catch

            Throw

        Finally

            ' check if ExecutionState changed.
            If wasOpen <> Me.IsOpen Then
                ' if so, alert on connection changed.
                Me.OnConnectionChanged()
            End If

        End Try

    End Sub

    ''' <summary> Updates the connection status. </summary>
    Private Sub OnConnectionChanged()

        If Not Me.IsOpen Then
            Me.StopPollTimer()
        End If

        Me._SendButton.Enabled = Me.IsOpen
        Me._ReadStatusRegisterButton.Enabled = Me.IsOpen
        Me._SendComboCommandButton.Enabled = Me.IsOpen
        Me._SendReceiveControlPanel.Enabled = Me.IsOpen

        ' enable by reading SRQ
        Me._ReceiveButton.Enabled = False

        Me._StatusRegisterLabel.Text = String.Empty

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

            If Not Me.DesignMode Then

                ' allow connecting the resource.
                Me._InstrumentChooser = _InterfacePanel.InstrumentChooser
                Me._InstrumentChooser.AddListeners(Me.Talker)
                Me._InterfacePanel.InstrumentChooser.Connectable = True

                ' allow form rendering time to complete: process all messages currently in the queue.
                Application.DoEvents()

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Displaying interface names;. ")
                Me._InterfacePanel.DisplayInterfaceNames()
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Ready to open Visa Session;. ")
                ' select the first item
                If Me._CommandsComboBox.Items.Count > 0 Then
                    Me._CommandsComboBox.SelectedIndex = 0
                End If
            End If

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception loading the instrument interface form;. {ex.ToFullBlownString}")
            If DialogResult.Abort = MessageBox.Show(ex.ToFullBlownString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
                                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                                    MessageBoxOptions.DefaultDesktopOnly) Then
                Application.Exit()
            End If
        Finally
            MyBase.OnLoad(e)
            Trace.CorrelationManager.StopLogicalOperation()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " INTERFACE EVENT HANDLERS "

    Private WithEvents _PollTimer As Timer

    Private WithEvents _InstrumentChooser As VI.Instrument.ResourceSelectorConnector

    Private Sub _InstrumentChooser_Clear(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InstrumentChooser.Clear
        Dim lastAction As String = "N/A"
        Try
            lastAction = "Clearing the device"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.Session.Clear()
        Catch ex As VI.Pith.NativeException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{lastAction} failed;. {ex.ToFullBlownString}")
        End Try
    End Sub

    Private Sub _InstrumentChooser_Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _InstrumentChooser.Connect
        ' exception handling is done in the resource connector.
        Me.OpenInstrumentSession()
        ' cancel if failed to open
        If Not Me.IsOpen Then e.Cancel = True
    End Sub

    Private Sub _InstrumentChooser_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _InstrumentChooser.Disconnect
        ' exception handling is done in the resource connector.
        Me.CloseInstrumentSession()
        ' cancel if failed to close.
        If Me.IsOpen Then e.Cancel = True
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As InterfacePanel, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(InterfacePanel.InterfaceResourceName)
                Me._StatusLabel.Text = $"Resource {sender.InterfaceResourceName} selected"
            Case NameOf(InterfacePanel.IsInterfaceOpen)
                If sender.IsInterfaceOpen Then
                    Me._StatusLabel.Text = "Select Instrument Resource"
                Else
                    Me._StatusLabel.Text = "Interface Closed"
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InterfacePanel for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfacePanel_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InterfacePanel.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.InterfacePanel)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._InterfacePanel_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Instrument.InterfacePanel), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SERVICE REQUEST MANAGEMENT "

    Private _ServiceRequestBits As Integer

    ''' <summary> Gets or sets the service request bits. </summary>
    ''' <value> The service request bits. </value>
    Private Property ServiceRequestBits As Integer
        Get
            Return Me._ServiceRequestBits
        End Get
        Set(value As Integer)
            If value <> Me.ServiceRequestBits Then
                Me._ServiceRequestBits = value
                Me._StatusRegisterLabel.Text = $"0x{value And &HFF:X2}"
                Dim mAV As Integer = CInt(Me._MessageAvailableBitsNumeric.Value)
                Me.MessageAvailable = (value And mAV) <> 0
            End If
        End Set
    End Property

    Private _MessageAvailable As Boolean

    ''' <summary> Gets or sets the message available sentinel. </summary>
    ''' <value> The message available. </value>
    Private Property MessageAvailable As Boolean
        Get
            Return Me._MessageAvailable
        End Get
        Set(value As Boolean)
            If value <> Me.MessageAvailable OrElse value <> (Me._ReadManualRadioButton.Checked AndAlso value) Then
                Me._MessageAvailable = value
                Me._ReceiveButton.Enabled = Me._ReadManualRadioButton.Checked AndAlso value
            End If
        End Set
    End Property

#End Region

#Region " TIMER HANDLERS "

    ''' <summary> Stops poll timer. </summary>
    Private Sub StopPollTimer()

        If Me._PollTimer IsNot Nothing Then
            Me._PollTimer.Enabled = False
            RemoveHandler Me._PollTimer.Tick, AddressOf Me.PollTimerTick
            Me._PollTimer.Dispose()
            Me._PollTimer = Nothing
        End If

        ' wait for timer to terminate all is actions
        Dim timer As System.Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew
        Do Until timer.ElapsedMilliseconds > 200
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        Loop

        If Me._PollRadioButton.Checked Then
            Me._ReadManualRadioButton.Checked = True
        End If

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary> Reads status register. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ReadStatusRegister()

        Dim lastAction As String = "N/A"
        Try

            lastAction = "Reading SRQ"
            Me._StatusRegisterLabel.Text = ""
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.ServiceRequestBits = Me.Session.ReadServiceRequestStatus
        Catch ex As Exception
            ex.Data.Add("@isr", lastAction)
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by readStatusRegisterButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub ReadStatusRegisterButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ReadStatusRegisterButton.Click
        Me.ReadStatusRegister()
    End Sub

    ''' <summary> Builds a message for the message log appending a line. </summary>
    ''' <param name="message"> Specifies the message to append. </param>
    ''' <returns> The time stamped message. </returns>
    Friend Shared Function BuildTimeStampLine(ByVal message As String) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0:HH:mm:ss.fff}Z {1}{2}", DateTime.UtcNow, message, Environment.NewLine)
    End Function

    ''' <summary> Receives this object. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Receive()

        Dim lastAction As String = "N/A"
        Try
            lastAction = "Receiving data"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")

            ReceiveBuffer = Me.Session.ReadLine()

            If Not String.IsNullOrWhiteSpace(ReceiveBuffer) Then
                ReceiveBuffer = ReceiveBuffer.InsertCommonEscapeSequences
            End If

            If Not String.IsNullOrWhiteSpace(ReceiveBuffer) Then
                With Me._OutputTextBox
                    .SelectionStart = .Text.Length
                    .SelectionLength = 0
                    .SelectedText = ResourceConsoleControl.BuildTimeStampLine(ReceiveBuffer)
                    .SelectionStart = .Text.Length
                End With
                Me._ReceiveButton.Enabled = False
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Received '{0}'.", ReceiveBuffer)
            End If


            ' update the status register information
            lastAction = "Reading status register"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.ReadStatusRegister()

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by receiveButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub ReceiveButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ReceiveButton.Click
        Me.Receive()
    End Sub

    ''' <summary> Send this message. </summary>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Send(ByVal value As String)

        Dim lastAction As String = "N/A"
        Try

            If Not String.IsNullOrWhiteSpace(value) Then
                TransmitBuffer = value.ReplaceCommonEscapeSequences
                If Not String.IsNullOrWhiteSpace(TransmitBuffer) Then
                    lastAction = $"Sending '{TransmitBuffer}'"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{lastAction };. ")
                    Me.Session.WriteLine(TransmitBuffer)
                End If
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Event handler. Called by sendButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub SendButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _SendButton.Click
        Me.Send(_InputTextBox.Text.Trim)
    End Sub

    ''' <summary> Event handler. Called by sendComboCommandButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SendComboCommandButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _SendComboCommandButton.Click

        Dim lastAction As String = "N/A"
        Try

            TransmitBuffer = Me._CommandsComboBox.Text.Trim
            If Not String.IsNullOrWhiteSpace(TransmitBuffer) Then
                lastAction = $"Sending '{TransmitBuffer}'"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.WriteLine(TransmitBuffer)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by _readManualRadioButton for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadManualRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadManualRadioButton.CheckedChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        If Not Me._ReadManualRadioButton.Checked Then
            Exit Sub
        End If
        Try
            Me._ReceiveButton.Enabled = False
            Me._ReadStatusRegisterButton.Enabled = True
            If Me.Session IsNot Nothing Then
                RemoveHandler Me.Session.ServiceRequested, AddressOf Me.SessionServiceRequested
            End If
            StopPollTimer()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by _pollRadioButton for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _PollRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _PollRadioButton.CheckedChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        If Not Me._PollRadioButton.Checked Then
            Exit Sub
        End If
        Try
            Me._ReceiveButton.Enabled = False
            Me._ReadStatusRegisterButton.Enabled = False
            If Me.Session IsNot Nothing Then
                Me.DisableServiceRequestEventHandler()
            End If

            Me._PollTimer = New Timer With {
                .Enabled = False,
                .Interval = CInt(Me._PollIntervalNumericUpDown.Value)
            }
            AddHandler Me._PollTimer.Tick, AddressOf Me.PollTimerTick
            Me._PollTimer.Enabled = True
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Raises the system. event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub PollTimerTick(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Timer)} tick event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.EventArgs)(AddressOf Me.PollTimerTick), New Object() {sender, e})
            Else
                Static pollTimerLocker As New Object
                SyncLock pollTimerLocker
                    Me.ReadStatusRegister()
                    If Me.MessageAvailable Then
                        Me.Receive()
                    End If
                End SyncLock
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Raises the message based session event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SessionServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.SessionBase)} service request"
        Try
            Me.ReadStatusRegister()
            If Me.MessageAvailable Then
                Me.Receive()
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    Private _ServiceRequestEventHandlerEnabled As Boolean

    ''' <summary> Enable service request event handler. </summary>
    Public Sub EnableServiceRequestEventHandler()
        If Me.IsOpen AndAlso Not Me._ServiceRequestEventHandlerEnabled Then
            Me._ServiceRequestEventHandlerEnabled = True
            AddHandler Me.Session.ServiceRequested, AddressOf Me.SessionServiceRequested
            Me.Session.EnableServiceRequest()
        End If
    End Sub

    ''' <summary> Disable service request event handler. </summary>
    Public Sub DisableServiceRequestEventHandler()
        If Me.IsOpen AndAlso Me._ServiceRequestEventHandlerEnabled Then
            Me._ServiceRequestEventHandlerEnabled = False
            Me.Session.DisableServiceRequest()
            RemoveHandler Me.Session.ServiceRequested, AddressOf Me.SessionServiceRequested
        End If
    End Sub

    ''' <summary> Event handler. Called by _serviceRequestReceiveOptionRadioButton for checked changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ServiceRequestReceiveOptionRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ServiceRequestReceiveOptionRadioButton.CheckedChanged
        If Me.InitializingComponents OrElse Not Me._ServiceRequestReceiveOptionRadioButton.Checked Then Return
        Try
            If Me._PollTimer IsNot Nothing Then
                Me._PollTimer.Enabled = False
                RemoveHandler Me._PollTimer.Tick, AddressOf Me.PollTimerTick
                Me._PollTimer.Dispose()
                Me._PollTimer = Nothing
            End If
            If Me.Session IsNot Nothing Then
                Me.EnableServiceRequestEventHandler()
                If Not String.IsNullOrWhiteSpace(Me._SreCommandComboBox.Text) Then
                    Me.Send(Me._SreCommandComboBox.Text & "\n")
                End If
                Me._ReceiveButton.Enabled = False
                Me._ReadStatusRegisterButton.Enabled = False
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by _pollIntervalNumericUpDown for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _PollIntervalNumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _PollIntervalNumericUpDown.ValueChanged

        If Me._PollTimer IsNot Nothing Then
            Me._PollTimer.Interval = CInt(Me._PollIntervalNumericUpDown.Value)
        End If

    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    Protected Overloads Sub AddPrivateListeners()
        Me._InterfacePanel.AddPrivateListeners(Me.Talker)
        MyBase.AddPrivateListener(Me._TraceMessagesBox)
    End Sub


    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As IMessageListener)
        Me._InterfacePanel.AddListener(listener)
        ' My.MyLibrary.Identify(Me.Talker)
        MyBase.AddListener(listener)
    End Sub

    ''' <summary> Removes the listeners if the talker was not assigned. </summary>
    Public Overrides Sub RemoveListeners()
        MyBase.RemoveListeners()
        Me._InterfacePanel.RemoveListeners()
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        If listenerType = Me._TraceMessagesBox.ListenerType Then Me._TraceMessagesBox.ApplyTraceLevel(value)
        Me._InterfacePanel.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the trace level type to all talkers. </summary>
    ''' <param name="listenerType"> Type of the trace level. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyTalkerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me._InterfacePanel.ApplyTalkerTraceLevel(listenerType, value)
        MyBase.ApplyTalkerTraceLevel(listenerType, value)
    End Sub

#End Region

#Region " MESSAGE BOX EVENTS "

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If String.Equals(propertyName, NameOf(isr.Core.Pith.TraceMessagesBox.StatusPrompt)) Then
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusPrompt, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = sender.StatusPrompt
        End If
    End Sub

    ''' <summary> Handles Trace messages box property changed event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Core.Pith.TraceMessagesBox)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Core.Pith.TraceMessagesBox), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

End Class

