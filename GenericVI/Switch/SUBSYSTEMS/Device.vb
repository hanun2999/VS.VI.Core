﻿''' <summary> Implements a generic switch device. </summary>
''' <remarks> An instrument is defined, for the purpose of this library, as a device with a front
''' panel. </remarks>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/10/2013" by="David" revision="3.0.5001"> Created. </history>
Public Class Device
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Switch.Device" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter()
        AddHandler My.Settings.PropertyChanged, AddressOf Me._Settings_PropertyChanged
    End Sub

#Region "IDisposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_RouteSubsystem", Justification:="@Closing")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' ?listeners must clear, otherwise closing could raise an exception.
                ' Me.Talker.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception {0}", ex.ToFullBlownString)
        Finally


            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        Me.Subsystems.Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#End Region

#Region " SESSION "

    ''' <summary> Allows the derived device to take actions after closing. </summary>
    Protected Overrides Sub OnClosed()
        MyBase.OnClosed()
    End Sub

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        If Me._SystemSubsystem IsNot Nothing Then
            'RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
        End If
        ' If Me._StatusSubsystem IsNot Nothing Then RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
        Me.StatusSubsystem = Nothing
        Me.Subsystems.DisposeItems()
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return

        ' STATUS must be the first subsystem.
        Me.StatusSubsystem = New StatusSubsystem(Me.Session)
        'Me.AddSubsystem(Me.StatusSubsystem)
        'AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged

        Me._SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SystemSubsystem)
        'AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged

        Me._RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.RouteSubsystem)

        Me._TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.TriggerSubsystem)

    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary>
    ''' Gets or sets the Route Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property RouteSubsystem As RouteSubsystem

    ''' <summary> Gets or sets the Trigger Subsystem. </summary>
    ''' <value> The Trigger Subsystem. </value>
    Public Property TriggerSubsystem As TriggerSubsystem

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystem
    ''' <summary>
    ''' Gets or sets the Status Subsystem.
    ''' </summary>
    ''' <value>The Status Subsystem.</value>
    Public Property StatusSubsystem As StatusSubsystem
        Get
            Return Me._StatusSubsystem
        End Get
        Set(value As StatusSubsystem)
            If Me._StatusSubsystem IsNot Nothing Then
                RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.StatusSubsystem)
                Me.StatusSubsystem.Dispose()
                Me._StatusSubsystem = Nothing
            End If
            Me._StatusSubsystem = value
            If Me._StatusSubsystem IsNot Nothing Then
                AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                Me.AddSubsystem(Me.StatusSubsystem)
            End If
            Me.StatusSubsystemBase = value
        End Set
    End Property

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        Me.OnPropertyChanged(CType(subsystem, StatusSubsystem), propertyName)
    End Sub

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As StatusSubsystem, ByVal propertyName As String)
        MyBase.OnPropertyChanged(subsystem, propertyName)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overloads Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As StatusSubsystem = TryCast(sender, StatusSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(StatusSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#Region " SERVICE REQUEST "

    ''' <summary>
    ''' Enables or disables the operation service status registers requesting
    ''' a service request upon a negative transition.
    ''' </summary>
    ''' <param name="isOn">if set to <c>True</c> [is on].</param>
    Public Sub ToggleEndOfScanService(ByVal isOn As Boolean)
        Me.ToggleEndOfScanService(isOn, ServiceRequests.All And (Not ServiceRequests.MessageAvailable))
    End Sub

    ''' <summary>
    ''' Enables or disables the operation service status registers requesting
    ''' a service request upon a negative transition.
    ''' </summary>
    ''' <param name="turnOn">True to turn on or false to turn off the service request.</param>
    ''' <param name="serviceRequestMask">Specifies the
    ''' <see cref="ServiceRequests">service request flags</see></param>
    Public Sub ToggleEndOfScanService(ByVal turnOn As Boolean, ByVal serviceRequestMask As ServiceRequests)
        If turnOn Then
            Me.StatusSubsystem.ApplyOperationNegativeTransitionEventEnableBitmask(OperationTransitionEvents.Settling)
            Me.StatusSubsystem.ApplyOperationPositiveTransitionEventEnableBitmask(OperationTransitionEvents.None)
            Me.StatusSubsystem.ApplyOperationEventEnableBitmask(OperationEvents.Settling)
        Else
            Me.StatusSubsystem.ApplyOperationNegativeTransitionEventEnableBitmask(OperationTransitionEvents.None)
            Me.StatusSubsystem.ApplyOperationPositiveTransitionEventEnableBitmask(OperationTransitionEvents.None)
            Me.StatusSubsystem.ApplyOperationEventEnableBitmask(OperationEvents.None)
        End If
        Me.ToggleServiceRequest(turnOn, serviceRequestMask)
    End Sub

    ''' <summary>
    ''' Enables or disables service requests.
    ''' </summary>
    ''' <param name="turnOn">True to turn on or false to turn off the service request.</param>
    ''' <param name="serviceRequestMask">Specifies the
    ''' <see cref="ServiceRequests">service request flags</see></param>
    Public Sub ToggleServiceRequest(ByVal turnOn As Boolean, ByVal serviceRequestMask As ServiceRequests)
        If turnOn Then Me.StatusSubsystem.EnableServiceRequest(serviceRequestMask)
    End Sub

#End Region

#End Region

#Region " SYSTEM "

    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystem

#End Region

#End Region

#Region " SERVICE REQUEST "

#If False Then
    ''' <summary> Reads the event registers after receiving a service request. </summary>
    ''' <remarks> Handled by the <see cref="DeviceBase"/></remarks>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadEventRegisters()
        If Me.StatusSubsystem.MessageAvailable Then
            ' if we have a message this needs to be processed by the subsystem requesting the message.
            ' Only thereafter the registers should be read.
        End If
        If Not Me.StatusSubsystem.MessageAvailable AndAlso Me.StatusSubsystem.ErrorAvailable Then
            Me.StatusSubsystem.QueryDeviceErrors()
        End If
        If Me.StatusSubsystem.MeasurementAvailable Then
        End If
    End Sub
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "Switch Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Protected Overrides Sub ApplySettings()
        Dim settings As My.MySettings = My.MySettings.Default
        Me.OnSettingsPropertyChanged(settings, NameOf(settings.TraceLogLevel))
        Me.OnSettingsPropertyChanged(settings, NameOf(settings.TraceShowLevel))
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSettingsPropertyChanged(ByVal sender As My.MySettings, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.TraceLogLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Logger, sender.TraceLogLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace log level changed to {sender.TraceLogLevel}")
            Case NameOf(sender.TraceShowLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Display, sender.TraceShowLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace show level changed to {sender.TraceShowLevel}")
        End Select
    End Sub

    ''' <summary> My settings property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Settings_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        Dim settings As My.MySettings = TryCast(sender, My.MySettings)
        If settings Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSettingsPropertyChanged(settings, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception handling Settings.{e.PropertyName} property;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identifies the talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        Mybase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region


End Class
