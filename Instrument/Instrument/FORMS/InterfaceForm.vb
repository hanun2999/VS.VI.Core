﻿Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
''' <summary> The Test Panel. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/07/2005" by="David" revision="2.0.2597.x"> Created. </history>
Public Class InterfaceForm
    Inherits ListenerFormBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializingComponents = True
        ' Initialize user components that might be affected by resize or paint actions
        'onInitialize()

        ' This method is required by the Windows Form Designer.
        InitializeComponent()
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

                ' removes all private listeners of the interface panel hosted in this form.
                Me._InterfacePanel?.RemovePrivateListeners()

                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENT HANDLERS "

    ''' <summary> Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event. Releases all
    ''' publishers. </summary>
    ''' <param name="e"> A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the
    ''' event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            If e IsNot Nothing Then
                e.Cancel = False
            End If
        Finally
            Application.DoEvents()
            MyBase.OnClosing(e)
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

            ' set the form caption
            Me.Text = $"{My.Application.Info.ProductName} release {My.Application.Info.Version}"

            ' default to center screen.
            Me.CenterToScreen()

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception loading the form;. {ex.ToFullBlownString}")
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

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Shown" /> event. </summary>
    ''' <param name="e"> A <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnShown(e As System.EventArgs)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

            ' allow form rendering time to complete: process all messages currently in the queue.
            Application.DoEvents()

            If Not Me.DesignMode Then
                Me._InterfacePanel.DisplayInterfaceNames()
            End If

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Ready to open Visa Session;. ")

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception showing the form;. {ex.ToFullBlownString}")
            If DialogResult.Abort = MessageBox.Show(ex.ToFullBlownString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
                                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                                    MessageBoxOptions.DefaultDesktopOnly) Then
                Application.Exit()
            End If
        Finally
            MyBase.OnShown(e)
            Trace.CorrelationManager.StopLogicalOperation()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub


#End Region

#Region " TALKER "

    ''' <summary> Identifies talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    Protected Overloads Sub AddPrivateListeners()
        MyBase.AddPrivateListener(Me._TraceMessagesBox)
        Me._InterfacePanel?.AddPrivateListeners(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As IMessageListener)
        MyBase.AddListener(listener)
        Me._InterfacePanel?.AddListener(listener)
    End Sub

    ''' <summary> Removes the listeners if the talker was not assigned. </summary>
    Public Overrides Sub RemoveListeners()
        MyBase.RemoveListeners()
        Me._InterfacePanel?.RemoveListeners()
    End Sub

    ''' <summary> Applies the trace level type to all talkers. </summary>
    ''' <param name="listenerType"> Type of the trace level. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyTalkerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        MyBase.ApplyTalkerTraceLevel(listenerType, value)
        Me._InterfacePanel?.ApplyTalkerTraceLevel(listenerType, value)
    End Sub

#End Region

#Region " MESSAGE BOX EVENTS "

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If String.Equals(propertyName, NameOf(TraceMessagesBox.StatusPrompt)) Then
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusPrompt, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = sender.StatusPrompt
        End If

    End Sub

    ''' <summary> Trace messages box property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.DeviceBase)}.{e?.PropertyName} change event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf _TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            ElseIf Not Me.InitializingComponents AndAlso sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.HandlePropertyChange(TryCast(sender, Core.Pith.TraceMessagesBox), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                ' used for figuring out what events are raised to cause this. 
                ' possibly the device is sending messages after the control is disposed indicating that the device needs to 
                ' be disposed before the control is disposed. 
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

End Class

