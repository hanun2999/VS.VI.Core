Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.TimeSpanExtensions
Imports isr.VI.SourceMeasure
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a user interface for the Keithley 2400 Device. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x"> Create based on the 24xx
''' system classes. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<System.ComponentModel.DisplayName("K2400 Panel"),
      System.ComponentModel.Description("Keithley 2400 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(K2400Panel))>
Public Class K2400Panel
    Inherits VI.Instrument.ResourcePanelBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device)
        MyBase.New(device)
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._AssignDevice(device)
        ' note that the caption is not set if this is run inside the On Load function.
        With Me.TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .ContainerPanel = Me._MessagesTabPage
        End With
        With Me._ServiceRequestEnableBitmaskNumeric.NumericUpDownControl
            .Hexadecimal = True
            .Maximum = 255
            .Minimum = 0
            .Value = 0
        End With
        Me._TraceButton.Visible = False
        Me._ReadingsCountLabel.Visible = False
        Me._ClearBufferDisplayButton.Visible = False
        Me.EnableTraceLevelControls()
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    Me._BinningInfo = Nothing
                    Me._ActiveInsulationResistance = Nothing
                    If Me._IsInsulationTestOwner AndAlso Me._InsulationTest IsNot Nothing Then
                        Me._InsulationTest.Dispose()
                        Me._InsulationTest = Nothing
                    End If
                    Me.Device?.RemovePrivateListener(Me.TraceMessagesBox)
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", "Exception {0}", ex.ToFullBlownString)
                End Try
                ' the device gets disposed in the base class!
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me._Device.CaptureSyncContext(Threading.SynchronizationContext.Current)
        Me._Device.AddPrivateListener(Me.TraceMessagesBox)
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overrides Sub ReleaseDevice()
        If Me.IsDeviceOwner Then
            MyBase.ReleaseDevice()
        Else
            Me._Device = Nothing
        End If
    End Sub

    ''' <summary> Gets a reference to the Keithley 2400 Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Private ReadOnly Property Session As VI.Pith.SessionBase
        Get
            Return Me.Device.Session
        End Get
    End Property

    ''' <summary> Executes the device open changed action. </summary>
    ''' <param name="device"> The device. </param>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
        MyBase.OnDeviceOpenChanged(device)
        If Me.IsDeviceOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
            ' Me._SimpleReadWriteControl.ReadEnabled = True
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then Me.RecursivelyEnable(t.Controls, Me.IsDeviceOpen)
        Next
        If Me.IsDeviceOpen Then
            Me._ClearInterfaceMenuItem.Visible = Me.Device.StatusSubsystemBase.SupportsClearInterface
            VI.Pith.SessionBase.ListNotificationLevels(Me._SessionNotificationLevelComboBox.ComboBox)
            AddHandler Me._SessionNotificationLevelComboBox.ComboBox.SelectedIndexChanged, AddressOf Me._SessionNotificationLevelComboBox_SelectedIndexChanged
            VI.Pith.SessionBase.SelectItem(Me._SessionNotificationLevelComboBox, NotifySyncLevel.None)
        End If
    End Sub


    ''' <summary> Handles the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal device As VI.DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(device, propertyName)
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(isr.VI.DeviceBase.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(isr.VI.DeviceBase.MessageNotificationLevel)
                VI.Pith.SessionBase.SelectItem(Me._SessionNotificationLevelComboBox, device.MessageNotificationLevel)
            Case NameOf(isr.VI.DeviceBase.ServiceRequestEnableBitmask)
                Dim value As VI.Pith.ServiceRequests = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableBitmaskNumeric.Value = value
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = $"SRE:0b{Convert.ToString(value, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        AddHandler Me.Device.ArmLayerSubsystem.PropertyChanged, AddressOf Me.ArmLayerSubsystemPropertyChanged
        AddHandler Me.Device.ContactCheckLimit.PropertyChanged, AddressOf Me.ContactCheckLimitPropertyChanged
        AddHandler Me.Device.ComplianceLimit.PropertyChanged, AddressOf Me.ComplianceLimitPropertyChanged
        AddHandler Me.Device.CompositeLimit.PropertyChanged, AddressOf Me.CompositeLimitPropertyChanged
        AddHandler Me.Device.UpperLowerLimit.PropertyChanged, AddressOf Me.UpperLowerLimitPropertyChanged
        AddHandler Me.Device.Calculate2Subsystem.PropertyChanged, AddressOf Me.Calculate2SubsystemPropertyChanged
        AddHandler Me.Device.DigitalOutput.PropertyChanged, AddressOf Me.DigitalOutputPropertyChanged
        AddHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
        AddHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
        AddHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        AddHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
        AddHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
        AddHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
        AddHandler Me.Device.SourceSubsystem.PropertyChanged, AddressOf Me.SourceSubsystemPropertyChanged
        AddHandler Me.Device.SourceCurrentSubsystem.PropertyChanged, AddressOf Me.SourceCurrentSubsystemPropertyChanged
        AddHandler Me.Device.SourceVoltageSubsystem.PropertyChanged, AddressOf Me.SourceVoltageSubsystemPropertyChanged
        AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        Me._MeterTimer = New System.Windows.Forms.Timer() With {.Enabled = False}
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(value)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me._MeterTimer IsNot Nothing Then
            Me._MeterTimer.Enabled = False
            Me._MeterTimer.Dispose()
            Me._MeterTimer = Nothing
        End If
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.ArmLayerSubsystem.PropertyChanged, AddressOf Me.ArmLayerSubsystemPropertyChanged
            RemoveHandler Me.Device.ContactCheckLimit.PropertyChanged, AddressOf Me.ContactCheckLimitPropertyChanged
            RemoveHandler Me.Device.ComplianceLimit.PropertyChanged, AddressOf Me.ComplianceLimitPropertyChanged
            RemoveHandler Me.Device.CompositeLimit.PropertyChanged, AddressOf Me.CompositeLimitPropertyChanged
            RemoveHandler Me.Device.UpperLowerLimit.PropertyChanged, AddressOf Me.UpperLowerLimitPropertyChanged
            RemoveHandler Me.Device.Calculate2Subsystem.PropertyChanged, AddressOf Me.Calculate2SubsystemPropertyChanged
            RemoveHandler Me.Device.DigitalOutput.PropertyChanged, AddressOf Me.DigitalOutputPropertyChanged
            RemoveHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
            RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
            RemoveHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
            RemoveHandler Me.Device.SourceSubsystem.PropertyChanged, AddressOf Me.SourceSubsystemPropertyChanged
            RemoveHandler Me.Device.SourceCurrentSubsystem.PropertyChanged, AddressOf Me.SourceCurrentSubsystemPropertyChanged
            RemoveHandler Me.Device.SourceVoltageSubsystem.PropertyChanged, AddressOf Me.SourceVoltageSubsystemPropertyChanged
            RemoveHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " CALCULATE 2 "

    ''' <summary> Handle the Calculate2 subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As Calculate2Subsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Calculate2 subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inCalculate2ion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Calculate2SubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Calculate2Subsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.Calculate2SubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Calculate2Subsystem), e.PropertyName)
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

#Region " CONTACT CHECK LIMIT "

    ''' <summary> Handle the contact check property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ContactCheckLimit, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.ContactCheckLimit.FailureBits)
                If subsystem.FailureBits.HasValue Then Me._ContactCheckBitPatternNumeric.SafeValueSetter(subsystem.FailureBits.Value)
        End Select
    End Sub

    ''' <summary> Contact Check Limit subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inCalculate2ion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ContactCheckLimitPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ContactCheckLimit)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ContactCheckLimitPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ContactCheckLimit), e.PropertyName)
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

#Region " COMPOSITE LIMIT "

    ''' <summary> Handle the contact check property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As CompositeLimit, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.CompositeLimit.FailureBits)
        End Select
    End Sub

    ''' <summary> Contact Check Limit subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inCalculate2ion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CompositeLimitPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(CompositeLimit)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.CompositeLimitPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, CompositeLimit), e.PropertyName)
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

#Region " COMPLIANCE LIMIT "

    ''' <summary> Handle the contact check property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ComplianceLimit, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.ComplianceLimit.FailureBits)
        End Select
    End Sub

    ''' <summary> Contact Check Limit subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inCalculate2ion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ComplianceLimitPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ComplianceLimit)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ComplianceLimitPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ComplianceLimit), e.PropertyName)
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

#Region " UPPER LOWER LIMIT "

    ''' <summary> Handle the upper lower limit subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As UpperLowerLimit, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Upper lower limit  subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inCalculate2ion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub UpperLowerLimitPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(UpperLowerLimit)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.UpperLowerLimitPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, UpperLowerLimit), e.PropertyName)
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

#Region " DIGITAL OUTPUT "

    ''' <summary> Handle the contact check property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As DigitalOutput, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.DigitalOutput.BitSize)
            Case NameOf(VI.SourceMeasure.DigitalOutput.Delay)
        End Select
    End Sub

    ''' <summary> Contact Check Limit subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inCalculate2ion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DigitalOutputPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(DigitalOutput)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.DigitalOutputPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, DigitalOutput), e.PropertyName)
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

#Region " FORMAT AND READING "

    ''' <summary> Handle the format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.FormatSubsystem.Elements)
                subsystem.ListElements(Me._ReadingComboBox.ComboBox, ReadingTypes.Units)
        End Select
    End Sub

    ''' <summary> Format subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FormatSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(FormatSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.FormatSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, FormatSubsystem), e.PropertyName)
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

#Region " MEASURE "

    ''' <summary> Handles the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(SourceMeasure.MeasureSubsystem.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", value)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(SourceMeasure.MeasureSubsystem.LastActionElapsedTime)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = $"{If(String.IsNullOrWhiteSpace(value), "<last reading>", value)} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
            Case NameOf(SourceMeasure.MeasureSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(SourceMeasure.MeasureSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription
            Case NameOf(SourceMeasure.MeasureSubsystem.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
        End Select
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(MeasureSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.MeasureSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, MeasureSubsystem), e.PropertyName)
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

#Region " ROUTE "

    ''' <summary> Handle the Route subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As RouteSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.RouteSubsystem.TerminalsMode)
                Dim value As Boolean? = New Boolean?
                If subsystem.TerminalsMode.HasValue Then
                    value = subsystem.TerminalsMode.Value = Scpi.RouteTerminalsMode.Front
                End If
                Me._OutputTerminalMenuItem.SafeSilentCheckStateSetter(value.ToCheckState)
        End Select
    End Sub

    ''' <summary> Route subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inRouteion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RouteSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(RouteSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.RouteSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, RouteSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OutputTerminalMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _OutputTerminalMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.ErrorProvider.Clear()
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If Not Me.Device.RouteSubsystem.TerminalsMode.HasValue OrElse
                (Me.Device.RouteSubsystem.TerminalsMode.Value = Scpi.RouteTerminalsMode.Front) <> menuItem.Checked Then
                If menuItem.Checked Then
                    Me.Device.RouteSubsystem.ApplyTerminalsMode(Scpi.RouteTerminalsMode.Front)
                Else
                    Me.Device.RouteSubsystem.ApplyTerminalsMode(Scpi.RouteTerminalsMode.Rear)
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Exception occurred toggling output terminal")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred toggling output terminal;. {0}", ex.ToFullBlownString)
        Finally
            If menuItem IsNot Nothing Then
                If menuItem.CheckState = Windows.Forms.CheckState.Indeterminate Then
                    menuItem.Text = "R/F?"
                Else
                    menuItem.Text = CStr(IIf(menuItem.Checked, "Front", "Rear"))
                End If
            End If
        End Try
    End Sub

#End Region

#Region " SENSE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnSupportedFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.SupportedFunctionModes <> VI.SourceMeasure.SenseFunctionModes.None Then
            With Me._SenseFunctionComboBox
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = GetType(VI.SourceMeasure.SenseFunctionModes).ValueDescriptionPairs(subsystem.SupportedFunctionModes)
                .DisplayMember = "Value"
                .ValueMember = "Key"
                If .Items.Count > 0 Then
                    .SelectedItem = VI.SourceMeasure.SenseFunctionModes.Voltage.ValueDescriptionPair()
                End If
            End With
            With Me._EnabledSenseFunctionsListBox
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = GetType(VI.SourceMeasure.SenseFunctionModes).ValueDescriptionPairs(subsystem.SupportedFunctionModes)
                .DisplayMember = "Value"
                .ValueMember = "Key"
            End With
        End If
    End Sub

    ''' <summary> Gets or sets the check function modes. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    '''                                             null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <value> The check function modes. </value>
    Public Property CheckedFunctionModes As SourceMeasure.SenseFunctionModes
        Get
            Dim checkedModes As SourceMeasure.SenseFunctionModes = SourceMeasure.SenseFunctionModes.None
            With Me._EnabledSenseFunctionsListBox
                For Each item As Object In .CheckedItems
                    Dim mode As SourceMeasure.SenseFunctionModes = CType(CType(item, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.SourceMeasure.SenseFunctionModes)
                    If checkedModes = SourceMeasure.SenseFunctionModes.None Then
                        checkedModes = mode
                    Else
                        checkedModes = checkedModes Or mode
                    End If
                Next
            End With
            Return checkedModes
        End Get
        Set(value As SourceMeasure.SenseFunctionModes)
            With Me._EnabledSenseFunctionsListBox
                For i As Integer = 0 To .Items.Count - 1
                    Dim item As Object = .Items(i)
                    Dim mode As SourceMeasure.SenseFunctionModes = CType(CType(item, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.SourceMeasure.SenseFunctionModes)
                    If (value And mode) = 0 Then
                        .SetItemCheckState(.Items.IndexOf(item), CheckState.Unchecked)
                    Else
                        .SetItemCheckState(.Items.IndexOf(item), CheckState.Checked)
                    End If
                Next
            End With
        End Set
    End Property

    ''' <summary> Executes the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionModes.HasValue Then
            Me.CheckedFunctionModes = subsystem.FunctionModes.Value
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.SenseSubsystem.ConcurrentSenseEnabled)
                If subsystem.ConcurrentSenseEnabled.HasValue Then Me._ConcurrentSenseCheckBox.SafeCheckedSetter(subsystem.ConcurrentSenseEnabled.Value)
                ' if concurrent sense changes, may no longer have concurrent mode.
                subsystem.QueryFunctionModes()
            Case NameOf(VI.SourceMeasure.SenseSubsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._NplcNumeric.SafeValueSetter(CDec(subsystem.PowerLineCycles.Value))
            Case NameOf(SourceMeasure.SenseSubsystem.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", value)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(SourceMeasure.SenseSubsystem.LastActionElapsedTime)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = $"{If(String.IsNullOrWhiteSpace(value), "<last reading>", value)} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
            Case NameOf(SourceMeasure.SenseSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(SourceMeasure.SenseSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription
            Case NameOf(SourceMeasure.SenseSubsystem.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
            Case NameOf(VI.SourceMeasure.SenseSubsystem.SupportedFunctionModes)
                Me.OnSupportedFunctionModesChanged(subsystem)
            Case NameOf(VI.SourceMeasure.SenseSubsystem.FunctionModes)
                Me.OnFunctionModesChanged(subsystem)
        End Select
    End Sub

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySenseFunctionButton_Click(sender As Object, e As EventArgs) Handles _ApplySenseFunctionButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.OutputSubsystem.WriteOutputOnState(False)
            Me.Device.SenseSubsystem.ApplyFunctionModes(Me.SelectedFunctionMode)
            Me.Device.SenseSubsystem.ApplyConcurrentSenseEnabled(Me._ConcurrentSenseCheckBox.Checked)
            Me.Device.SystemSubsystem.ApplyFourWireSenseEnabled(Me._FourWireSenseCheckBox.Checked)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed applying source function")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred applying source function;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedFunctionMode() As SourceMeasure.SenseFunctionModes
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, SourceMeasure.SenseFunctionModes)
        End Get
    End Property

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    Private Sub OnSelectedFunctionChanged(ByVal value As SourceMeasure.SenseFunctionModes)
        Me._SenseFunctionComboBox.SelectedItem = value
        Dim unit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Volt
        Select Case value
            Case VI.SourceMeasure.SenseFunctionModes.Current
                unit = Arebis.StandardUnits.ElectricUnits.Ampere
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 10D
                    .DecimalPlaces = 6
                    .Increment = 0.001D
                End With
                With Device.SenseCurrentSubsystem
                    .QueryPowerLineCycles()
                    .QueryAutoRangeEnabled()
                    .QueryProtectionLevel()
                    .QueryRange()
                End With
            Case VI.SourceMeasure.SenseFunctionModes.Voltage
                unit = Arebis.StandardUnits.ElectricUnits.Volt
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 1100D
                    .DecimalPlaces = 3
                    .Increment = 1D
                End With
                With Device.SenseVoltageSubsystem
                    .QueryPowerLineCycles()
                    .QueryAutoRangeEnabled()
                    .QueryProtectionLevel()
                    .QueryRange()
                End With
            Case VI.SourceMeasure.SenseFunctionModes.Resistance
                unit = Arebis.StandardUnits.ElectricUnits.Ohm
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 1000000000D
                    .DecimalPlaces = 0
                    .Increment = 1000D
                End With
                With Device.SenseResistanceSubsystem
                    .QueryPowerLineCycles()
                    .QueryAutoRangeEnabled()
                    .QueryRange()
                End With
        End Select
        Me._SenseRangeNumericLabel.Text = String.Format(Globalization.CultureInfo.CurrentCulture, "Range [{0}]:", unit.Symbol)
        Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
    End Sub

    ''' <summary> Event handler. Called by _SenseFunctionComboBox for selected index changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="degC")>
    Private Sub _SenseFunctionComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SenseFunctionComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.OnSelectedFunctionChanged(Me.SelectedFunctionMode)
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub ApplySenseSettings(ByVal value As VI.SourceMeasure.SenseFunctionModes)

        Me.Device.ClearExecutionState()

        ' make sure output is off.
        Me.Device.OutputSubsystem.WriteOutputOnState(False)

        If value = VI.SourceMeasure.SenseFunctionModes.CurrentDC Then

            With Me.Device.SenseCurrentSubsystem
                .ApplyPowerLineCycles(Me._NplcNumeric.Value)
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)
                If Not Me._SenseAutoRangeToggle.Checked Then .ApplyRange(Me._SenseRangeNumeric.Value)
            End With

        ElseIf value = VI.SourceMeasure.SenseFunctionModes.Resistance Then

            With Me.Device.SenseResistanceSubsystem
                .ApplyPowerLineCycles(Me._NplcNumeric.Value)
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)
                If Not Me._SenseAutoRangeToggle.Checked Then .ApplyRange(Me._SenseRangeNumeric.Value)
            End With

        ElseIf value = VI.SourceMeasure.SenseFunctionModes.VoltageDC Then

            With Me.Device.SenseVoltageSubsystem
                .ApplyPowerLineCycles(Me._NplcNumeric.Value)
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)
                If Not Me._SenseAutoRangeToggle.Checked Then .ApplyRange(Me._SenseRangeNumeric.Value)
            End With

        End If

    End Sub

    ''' <summary> Event handler. Called by ApplySenseSettingsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySenseSettingsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplySenseSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplySenseSettings(Me.SelectedFunctionMode)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred applying sense settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " SENSE VOLTAGE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseVoltageSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return

        ' apply only if using voltage sense.
        If Me.Device.SenseSubsystem.FunctionMode.GetValueOrDefault(SourceMeasure.SenseFunctionModes.None) = SourceMeasure.SenseFunctionModes.Voltage Then
            Select Case propertyName
                Case NameOf(VI.SourceMeasure.SenseVoltageSubsystem.AutoRangeEnabled)
                    If subsystem.AutoRangeEnabled.HasValue Then Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                Case NameOf(VI.SourceMeasure.SenseVoltageSubsystem.PowerLineCycles)
                    If subsystem.PowerLineCycles.HasValue Then Me._NplcNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                Case NameOf(VI.SourceMeasure.SenseVoltageSubsystem.Range)
                    If subsystem.Range.HasValue Then Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
            End Select
        End If

        ' protection applies only if using a current source.
        If Me.Device.SourceSubsystem.FunctionMode.GetValueOrDefault(Scpi.SourceFunctionModes.None) = Scpi.SourceFunctionModes.Current Then
            Select Case propertyName
                Case NameOf(VI.SourceMeasure.SenseVoltageSubsystem.ProtectionLevel)
                    If subsystem.ProtectionLevel.HasValue Then Me._SourceLimitNumeric.SafeValueSetter(subsystem.ProtectionLevel.Value)
            End Select
        End If

    End Sub

    ''' <summary> Sense voltage subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseVoltageSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseVoltageSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseVoltageSubsystem), e.PropertyName)
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

#Region " SENSE CURRENT "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseCurrentSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return

        ' apply only if using current sense.
        If Me.Device.SenseSubsystem.FunctionMode.GetValueOrDefault(SourceMeasure.SenseFunctionModes.None) = SourceMeasure.SenseFunctionModes.Current Then
            Select Case propertyName
                Case NameOf(VI.SourceMeasure.SenseCurrentSubsystem.AutoRangeEnabled)
                    If subsystem.AutoRangeEnabled.HasValue Then Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                Case NameOf(VI.SourceMeasure.SenseCurrentSubsystem.PowerLineCycles)
                    If subsystem.PowerLineCycles.HasValue Then Me._NplcNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                Case NameOf(VI.SourceMeasure.SenseCurrentSubsystem.Range)
                    If subsystem.Range.HasValue Then Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
            End Select
        End If

        ' protection applies only if using a voltage source.
        If Me.Device.SourceSubsystem.FunctionMode.GetValueOrDefault(Scpi.SourceFunctionModes.None) = Scpi.SourceFunctionModes.Voltage Then
            Select Case propertyName
                Case NameOf(VI.SourceMeasure.SenseCurrentSubsystem.ProtectionLevel)
                    If subsystem.ProtectionLevel.HasValue Then Me._SourceLimitNumeric.SafeValueSetter(subsystem.ProtectionLevel.Value)
            End Select
        End If

    End Sub

    ''' <summary> Sense Current subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseCurrentSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseCurrentSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseCurrentSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseCurrentSubsystem), e.PropertyName)
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

#Region " SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return

        ' apply only if using current sense.
        If Me.Device.SenseSubsystem.FunctionMode.GetValueOrDefault(SourceMeasure.SenseFunctionModes.None) = SourceMeasure.SenseFunctionModes.Resistance Then
            Select Case propertyName
                Case NameOf(VI.SourceMeasure.SenseResistanceSubsystem.AutoRangeEnabled)
                    If subsystem.AutoRangeEnabled.HasValue Then
                        Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                    End If
                Case NameOf(VI.SourceMeasure.SenseResistanceSubsystem.PowerLineCycles)
                    If subsystem.PowerLineCycles.HasValue Then
                        Me._NplcNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                    End If
                Case NameOf(VI.SourceMeasure.SenseResistanceSubsystem.Range)
                    If subsystem.Range.HasValue Then
                        Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                    End If
            End Select
        End If

    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseResistanceSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseResistanceSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseResistanceSubsystem), e.PropertyName)
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

#Region " SOURCE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnSupportedFunctionModesChanged(ByVal subsystem As SourceSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.SupportedFunctionModes <> Scpi.SourceFunctionModes.None Then
            With Me._SourceFunctionComboBox
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = GetType(Scpi.SourceFunctionModes).ValueDescriptionPairs(subsystem.SupportedFunctionModes)
                .DisplayMember = "Value"
                .ValueMember = "Key"
                If .Items.Count > 0 Then
                    .SelectedItem = Scpi.SourceFunctionModes.Voltage.ValueDescriptionPair()
                End If
            End With
        End If
    End Sub

    ''' <summary> Gets or sets the selected source function mode. </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedSourceFunctionMode() As Scpi.SourceFunctionModes
        Get
            Return CType(CType(Me._SourceFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, Scpi.SourceFunctionModes)
        End Get
    End Property

    Private Sub OnSourceFunctionChanged(ByVal subsystem As SourceSubsystem)
        If subsystem Is Nothing Then Return
        Dim unit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Volt
        Dim range As RangeR = New RangeR(-1, 1)
        Dim decimalPlaces As Integer = 3
        Dim increment As Decimal = 0
        Dim limitUnit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Ampere
        Dim limitRange As RangeR = New RangeR(-1, 1)
        Dim limitDecimalPlaces As Integer = 3
        Dim limitIncrement As Decimal = 0
        Select Case subsystem.FunctionMode.Value
            Case Scpi.SourceFunctionModes.Current
                unit = Arebis.StandardUnits.ElectricUnits.Ampere
                range = Me.Device.SourceCurrentSubsystem.LevelRange
                decimalPlaces = 6
                increment = 0.0001D
                limitUnit = Arebis.StandardUnits.ElectricUnits.Volt
                limitRange = Me.Device.SourceVoltageSubsystem.LevelRange
                limitDecimalPlaces = 3
                limitIncrement = 1
            Case Scpi.SourceFunctionModes.Voltage
                range = Me.Device.SourceVoltageSubsystem.LevelRange
                decimalPlaces = 3
                limitRange = Me.Device.SourceCurrentSubsystem.LevelRange
                limitDecimalPlaces = 6
                increment = 1D
                limitIncrement = 0.0001D
        End Select
        With Me._SourceLevelNumeric
            .Maximum = CDec(range.Max)
            .Minimum = CDec(range.Min)
            .DecimalPlaces = decimalPlaces
            .Increment = increment
        End With
        With Me._SourceRangeNumeric
            .Maximum = CDec(range.Max)
            .Minimum = CDec(range.Min)
            .DecimalPlaces = decimalPlaces
            .Increment = increment
        End With
        With Me._SourceLimitNumeric
            .Maximum = CDec(limitRange.Max)
            .Minimum = CDec(limitRange.Min)
            .DecimalPlaces = limitDecimalPlaces
            .Increment = limitIncrement
        End With
        Me._SourceLevelNumericLabel.Text = $"Level [{unit.Symbol}]"
        Me._SourceLevelNumericLabel.Left = Me._SourceLevelNumeric.Left - Me._SourceLevelNumericLabel.Width
        Me._SourceRangeNumericLabel.Text = $"Range [{unit.Symbol}]"
        Me._SourceRangeNumericLabel.Left = Me._SourceRangeNumeric.Left - Me._SourceRangeNumericLabel.Width
        Me._SourceLimitNumericLabel.Text = $"Limit [{limitUnit.Symbol}]"
        Me._SourceLimitNumericLabel.Left = Me._SourceLimitNumeric.Left - Me._SourceLimitNumericLabel.Width

        Select Case subsystem.FunctionMode.Value
            Case Scpi.SourceFunctionModes.Current
                With Me.Device.SourceCurrentSubsystem
                    .QueryLevel()
                    .QueryRange()
                End With
            Case Scpi.SourceFunctionModes.Voltage
                With Me.Device.SourceVoltageSubsystem
                    .QueryLevel()
                    .QueryRange()
                End With
        End Select
    End Sub

    ''' <summary> Handle the Source subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SourceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._SourceRangeTextBox.SafeTextSetter(Me.Device.SourceRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SourceIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.SourceSubsystem.AutoClearEnabled)
                If subsystem.AutoClearEnabled.HasValue Then
                    Me._SourceAutoClearEnabledMenuItem.SafeCheckedSetter(subsystem.AutoClearEnabled.Value)
                End If
            Case NameOf(VI.SourceMeasure.SourceSubsystem.Delay)
                If subsystem.Delay.HasValue Then
                    Me._SourceDelayNumeric.SafeValueSetter(subsystem.Delay.Value.Ticks / TimeSpan.TicksPerMillisecond)
                End If
            Case NameOf(VI.SourceMeasure.SourceSubsystem.FunctionMode)
                If subsystem.FunctionMode.HasValue Then
                    Me._SourceFunctionComboBox.SelectedItem = subsystem.FunctionMode.Value.ValueDescriptionPair
                    Me.OnSourceFunctionChanged(subsystem)
                End If
            Case NameOf(VI.SourceMeasure.SourceSubsystem.SupportedFunctionModes)
                Me.OnSupportedFunctionModesChanged(subsystem)
        End Select
    End Sub

    ''' <summary> Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SourceSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SourceSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SourceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Source automatic clear enabled menu item check state changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SourceAutoClearEnabledMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _SourceAutoClearEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.ErrorProvider.Clear()
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If Not Me.Device.SourceSubsystem.AutoClearEnabled.HasValue OrElse
                Me.Device.SourceSubsystem.AutoClearEnabled.Value <> menuItem.Checked Then
                Me.Device.SourceSubsystem.ApplyAutoClearEnabled(menuItem.Checked)
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Exception occurred setting output auto on mode")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred setting output auto on mode;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Handle the Source Voltage subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SourceVoltageSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If Me.Device.SourceSubsystem.FunctionMode.GetValueOrDefault(Scpi.SourceFunctionModes.None) <> Scpi.SourceFunctionModes.Voltage Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.SourceVoltageSubsystem.Level)
                If subsystem.Level.HasValue Then Me._SourceLevelNumeric.SafeValueSetter(subsystem.Level.Value)
            Case NameOf(VI.SourceMeasure.SourceVoltageSubsystem.Range)
                If subsystem.Range.HasValue Then Me._SourceRangeNumeric.SafeValueSetter(subsystem.Range.Value)
        End Select
    End Sub

    ''' <summary> Source Voltage subsystem property changed. </summary>
    ''' <param name="sender"> Source Voltage of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SourceVoltageSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SourceVoltageSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SourceVoltageSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Handle the Source Current subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SourceCurrentSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If Me.Device.SourceSubsystem.FunctionMode.GetValueOrDefault(Scpi.SourceFunctionModes.None) <> Scpi.SourceFunctionModes.Current Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.SourceCurrentSubsystem.Level)
                If subsystem.Level.HasValue Then Me._SourceLevelNumeric.SafeValueSetter(subsystem.Level.Value)
            Case NameOf(VI.SourceMeasure.SourceCurrentSubsystem.Range)
                If subsystem.Range.HasValue Then Me._SourceRangeNumeric.SafeValueSetter(subsystem.Range.Value)
        End Select
    End Sub

    ''' <summary> Source Current subsystem property changed. </summary>
    ''' <param name="sender"> Source Current of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceCurrentSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SourceCurrentSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SourceCurrentSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SourceCurrentSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySourceFunctionButton_Click(sender As Object, e As EventArgs) Handles _ApplySourceFunctionButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ClearExecutionState()

            ' make sure output is off.
            Me.Device.OutputSubsystem.WriteOutputOnState(False)

            Me.Device.SourceSubsystem.ApplyFunctionMode(Me.SelectedSourceFunctionMode)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed applying source function")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred applying source function;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySourceSettingButton_Click(sender As Object, e As EventArgs) Handles _ApplySourceSettingButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ClearExecutionState()

            ' make sure output is off.
            Me.Device.OutputSubsystem.WriteOutputOnState(False)

            ' get the delay time
            If Me._TriggerDelayNumeric.Value >= 0 Then
                Me.Device.TriggerSubsystem.ApplyDelay(TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me._TriggerDelayNumeric.Value)))
            Else
                Me.Device.TriggerSubsystem.ApplyAutoDelayEnabled(True)
            End If

            Me.Device.SourceSubsystem.ApplyDelay(TimeSpan.FromMilliseconds(Me._SourceDelayNumeric.Value))
            Select Case Me.Device.SourceSubsystem.FunctionMode.Value
                Case Scpi.SourceFunctionModes.Current
                    With Me.Device.SourceCurrentSubsystem
                        .ApplyRange(1.1 * Me._SourceLevelNumeric.Value)
                        .ApplyLevel(Me._SourceLevelNumeric.Value)
                    End With
                    With Me.Device.SenseVoltageSubsystem
                        .ApplyProtectionLevel(Me._SourceLimitNumeric.Value)
                    End With
                Case Scpi.SourceFunctionModes.Voltage
                    With Me.Device.SourceVoltageSubsystem
                        .ApplyRange(1.1 * Me._SourceLevelNumeric.Value)
                        .ApplyLevel(Me._SourceLevelNumeric.Value)
                    End With
                    With Me.Device.SenseCurrentSubsystem
                        .ApplyProtectionLevel(Me._SourceLimitNumeric.Value)
                    End With
            End Select
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed applying source settings")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred applying source settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub OnLastError(ByVal lastError As DeviceError)
        If lastError?.IsError Then
            Me._LastErrorTextBox.ForeColor = Drawing.Color.OrangeRed
        Else
            Me._LastErrorTextBox.ForeColor = Drawing.Color.Aquamarine
        End If
        Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(StatusSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.StatusSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, StatusSubsystem), e.PropertyName)
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

#Region " SYSTEM "

    ''' <summary> Contact check enabled menu item check state changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ContactCheckEnabledMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _ContactCheckEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.ErrorProvider.Clear()
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If Not Me.Device.SystemSubsystem.ContactCheckEnabled.HasValue OrElse Me.Device.SystemSubsystem.ContactCheckEnabled.Value <> menuItem.Checked Then
                Me._ContactCheckEnabledMenuItem.SafeSilentCheckedSetter(Me.Device.SystemSubsystem.ContactCheckEnabled.Value)
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Exception occurred enabling contact check")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred enabling contact check;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.SystemSubsystem.ContactCheckEnabled)
                If subsystem.ContactCheckEnabled.HasValue Then Me._ContactCheckEnabledMenuItem.SafeSilentCheckedSetter(subsystem.ContactCheckEnabled.Value)
            Case NameOf(VI.SourceMeasure.SystemSubsystem.FrontSwitched)
                Me._OutputTerminalMenuItem.SafeSilentCheckStateSetter((Not subsystem.FrontSwitched).ToCheckState)
            Case NameOf(VI.SourceMeasure.SystemSubsystem.FourWireSenseEnabled)
                If subsystem.FourWireSenseEnabled.HasValue Then Me._FourWireSenseCheckBox.SafeCheckedSetter(subsystem.FourWireSenseEnabled.Value)
            Case NameOf(VI.SourceMeasure.SystemSubsystem.SupportsContactCheck)
                Me._ContactCheckToggle.SafeEnabledSetter(subsystem.SupportsContactCheck.GetValueOrDefault(False))
                Me._ContactCheckBitPatternNumeric.SafeEnabledSetter(subsystem.SupportsContactCheck.GetValueOrDefault(False))
        End Select
        Application.DoEvents()
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SystemSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.SystemSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SystemSubsystem), e.PropertyName)
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

#Region " TRIGGER "

    ''' <summary> The device access locker. </summary>
    Private _DeviceAccessLocker As New Object

    Private WithEvents _MeterTimer As System.Windows.Forms.Timer

    ''' <summary>
    ''' Aborts the measurement cycle.
    ''' </summary>
    Private Sub _AbortMeasurement()
        Me._MeterTimer.Enabled = False
        Me.Device.TriggerSubsystem.Abort()
    End Sub

    ''' <summary>
    ''' Aborts the measurement cycle.
    ''' </summary>
    Private Sub AbortMeasurement()
        SyncLock _DeviceAccessLocker
            Me._AbortMeasurement()
        End SyncLock
    End Sub

    ''' <summary>
    ''' Asserts a trigger to emulate triggering for timing measurements.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AssertTriggerToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AssertTriggerToolStripButton.Click
        Me.ErrorProvider.Clear()
        Try
            Dim mode As Scpi.ArmSources = Me.Device.ArmLayerSubsystem.ArmSource.GetValueOrDefault(Scpi.ArmSources.None)
            If (mode = Scpi.ArmSources.Manual) AndAlso Me._MeterTimer.Enabled Then
                Me.AssertTrigger()
            ElseIf mode <> Scpi.ArmSources.Manual Then
                Me.ErrorProvider.Annunciate(sender, $"Manual trigger ignored in '{mode.Description}' mode")
            ElseIf Not Me._MeterTimer.Enabled Then
                Me.ErrorProvider.Annunciate(sender, "Manual trigger ignored -- triggering is not active")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Exception occurred aborting")
            My.Application.Log.WriteException(ex, TraceEventType.Error, "Exception occurred aborting.")
        End Try
    End Sub

    ''' <summary>
    ''' Outputs a trigger to make a measurement.
    ''' </summary>
    Private Sub AssertTrigger()
        SyncLock _DeviceAccessLocker
            Me.Device.Session.AssertTrigger()
        End SyncLock
    End Sub

    Private _AbortRequested As Boolean
    ''' <summary>
    ''' Turns on or aborts waiting for trigger.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")>
    Private Sub ToggleAwaitTrigger(ByVal enabled As Boolean, ByVal checked As Boolean)
        Me.ErrorProvider.SetIconPadding(Me._TriggerToolStrip, -10)
        Me.ErrorProvider.SetError(Me._TriggerToolStrip, "")
        If enabled Then
            Me._AbortRequested = Not checked
            If checked Then
                SyncLock _DeviceAccessLocker
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Preparing instrument for waiting for trigger;. ")
                    Me.TriggerAction = "INITIATING"
                    ' clear execution state before enabling events
                    Me.Device.ClearExecutionState()

                    ' set the service request
                    Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
                    Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.StandardEvents.All And Not VI.Pith.StandardEvents.RequestControl,
                                                                   VI.Pith.ServiceRequests.StandardEvent Or VI.Pith.ServiceRequests.OperationEvent)
                    Me.Device.ClearExecutionState()
                    Me.Device.TriggerSubsystem.Initiate()
                    Me.TriggerAction = "WAITING FOR TRIGGERED MEASUREMENT"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Monitoring instrument for measurements;. ")
                    Me._MeterTimer.Interval = 100
                    Me._MeterTimer.Enabled = True
                End SyncLock
            Else
                Me.TriggerAction = "ABORT REQUESTED"
                Me._AbortMeasurement()
            End If
        End If
        If Me._AwaitTriggerToolStripButton.Checked Then
            Me._AwaitTriggerToolStripButton.Text = "ABORT TRIGGERING"
        Else
            Me._AwaitTriggerToolStripButton.Text = "WAIT FOR A TRIGGER"
        End If
    End Sub

    ''' <summary>
    ''' Turns on or aborts waiting for trigger.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AwaitTriggerToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _AwaitTriggerToolStripButton.CheckedChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.ErrorProvider.Clear()
        Try
            Me.ToggleAwaitTrigger(_AwaitTriggerToolStripButton.Enabled, _AwaitTriggerToolStripButton.Checked)
        Catch ex As Exception
            Me._AbortMeasurement()
            Me.ErrorProvider.Annunciate(sender, "Exception occurred initiating trigger")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating trigger;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Gets or sets the trigger action. </summary>
    ''' <value> The trigger action. </value>
    Private Property TriggerAction As String
        Get
            Return Me._TriggerActionToolStripLabel.Text
        End Get
        Set(value As String)
            If Not String.Equals(Me.TriggerAction, value) Then
                Me._TriggerActionToolStripLabel.Text = value
            End If
        End Set
    End Property

    ''' <summary> Gets the next status bar. </summary>
    ''' <value> The next status bar. </value>
    Private ReadOnly Property NextStatusBar As String
        Get
            Static bar As String = "|"
            If bar = "|" Then
                bar = "/"
            ElseIf bar = "/" Then
                bar = "-"
            ElseIf bar = "-" Then
                bar = "\"
            ElseIf bar = "\" Then
                bar = "|"
            End If
            Return bar
        End Get
    End Property

    ''' <summary>
    ''' Monitors measurements. Once found, reads and displays and restarts the cycle.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeterTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MeterTimer.Tick
        SyncLock _DeviceAccessLocker
            Try
                Me._MeterTimer.Enabled = False
                If Me._AbortRequested Then
                    Me._AbortMeasurement()
                    Return
                End If
                Me.Device.StatusSubsystem.ReadServiceRequestStatus()
                If Me.Device.StatusSubsystem.MeasurementAvailable Then
                    Me.TriggerAction = "READING..."

                    ' update display modalities if changed.
                    Me.Device.MeasureSubsystem.Read()
                    Me.TriggerAction = "DATA AVAILABLE..."

                    Me._TriggerActionToolStripLabel.Text = "PREPARING..."

                    ' clear execution state before enabling events
                    Me.Device.ClearExecutionState()

                    ' set the service request
                    Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
                    Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.StandardEvents.All And Not VI.Pith.StandardEvents.RequestControl,
                                                                   VI.Pith.ServiceRequests.StandardEvent Or VI.Pith.ServiceRequests.OperationEvent)
                    Me.Device.ClearExecutionState()
                    Me.Device.TriggerSubsystem.Initiate()
                    Me.TriggerAction = "WAITING FOR TRIGGRED MEASUREMENT..."
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Monitoring instrument for measurements;. ")
                Else
                    Me.TriggerAction = "WAITING FOR TRIGGERED MEASUREMENT"
                    Me._WaitHourglassLabel.Text = Me.NextStatusBar
                End If
                Me._MeterTimer.Enabled = Not Me._AbortRequested
            Catch ex As Exception
                Me.ErrorProvider.SetError(Me._TriggerToolStrip, "Exception occurred monitoring instrument for data")
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred monitoring instrument for data;. {0}", ex.ToFullBlownString)
                Me._AwaitTriggerToolStripButton.Checked = False
            End Try
        End SyncLock
    End Sub

    ''' <summary> Handle the Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As TriggerSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.TriggerSubsystem.Delay)
                If subsystem.Delay.HasValue Then Me._TriggerDelayNumeric.SafeValueSetter(subsystem.Delay.Value.TotalMilliseconds)
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inTriggerion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(TriggerSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.TriggerSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, TriggerSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Handle the Arm subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ArmLayerSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.ArmLayerSubsystem.InputLineNumber)
            Case NameOf(VI.SourceMeasure.ArmLayerSubsystem.ArmSource)
                Me._ArmSourceComboBox.SelectedItem = subsystem.ArmSource.Value.ValueDescriptionPair
            Case NameOf(VI.SourceMeasure.ArmLayerSubsystem.SupportedArmSources)
                If Me.Device IsNot Nothing AndAlso subsystem.SupportedArmSources <> Scpi.ArmSources.None Then
                    Dim selectedIndex As Integer = Me._ReadingComboBox.SelectedIndex
                    With Me._ArmSourceComboBox
                        .DataSource = Nothing
                        .Items.Clear()
                        .DataSource = GetType(Scpi.ArmSources).ValueDescriptionPairs(subsystem.SupportedArmSources)
                        .DisplayMember = "Value"
                        .ValueMember = "Key"
                        If .Items.Count > 0 Then
                            .SelectedIndex = Math.Max(selectedIndex, 0)
                        End If
                    End With
                End If
        End Select
    End Sub

    ''' <summary> Arm subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inArmion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ArmLayerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ArmLayerSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ArmLayerSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ArmLayerSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Gets or sets the selected arm source. </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedArmSource() As Scpi.ArmSources
        Get
            Return CType(CType(Me._ArmSourceComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, Scpi.ArmSources)
        End Get
    End Property

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: HIPOT "

    Private _IsInsulationTestOwner As Boolean
    Private _InsulationTest As InsulationTest
    ''' <summary> Tests assign insulation. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub AssignInsulationTest()
        Me._InsulationTest = New InsulationTest
        Me._IsInsulationTestOwner = False
        Me.AssignInsulationTest(New InsulationTest)
        Me._IsInsulationTestOwner = True
    End Sub

    ''' <summary> Tests assign insulation. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Sub AssignInsulationTest(ByVal value As InsulationTest)
        If Me._IsInsulationTestOwner AndAlso Me._InsulationTest IsNot Nothing Then
            Me._InsulationTest.Dispose()
            Me._InsulationTest = Nothing
        End If
        Me._IsInsulationTestOwner = False
        If value Is Nothing Then
            Me._BinningInfo = Nothing
            Me._ActiveInsulationResistance = Nothing
        Else
            Me._BinningInfo = value.Binning
            Me._ActiveInsulationResistance = value.Insulation
            value.Publish()
        End If
    End Sub

    Private WithEvents _BinningInfo As VI.Scpi.BinningInfo

    ''' <summary> Gets information describing the binning. </summary>
    ''' <value> Information describing the binning. </value>
    Public ReadOnly Property BinningInfo As VI.Scpi.BinningInfo
        Get
            Return Me._BinningInfo
        End Get
    End Property

    Private Overloads Sub OnPropertyChanged(ByVal sender As VI.Scpi.BinningInfo, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(BinningInfo.ArmSource)
                Me._ArmSourceComboBox.SafeSelectItem(sender.ArmSource.ValueDescriptionPair)
            Case NameOf(BinningInfo.PassBits)
                Me._PassBitPatternNumeric.SafeValueSetter(sender.PassBits)
            Case NameOf(BinningInfo.LowerLimit)
                Me._ResistanceLowLimitNumeric.SafeValueSetter(0.00001 * sender.LowerLimit)
            Case NameOf(BinningInfo.LowerLimitFailureBits)
                Me._FailBitPatternNumeric.SafeValueSetter(sender.LowerLimitFailureBits)
            Case NameOf(BinningInfo.StrobePulseWidth)
                Me._EotStrobeDurationNumeric.SafeValueSetter(sender.StrobePulseWidth.Ticks / TimeSpan.TicksPerMillisecond)
            Case NameOf(BinningInfo.FailureBits)
                Me._FailBitPatternNumeric.Value = sender.FailureBits
            Case NameOf(BinningInfo.UpperLimitFailureBits)
                Me._FailBitPatternNumeric.Value = sender.UpperLimitFailureBits
            Case NameOf(BinningInfo.LowerLimitFailureBits)
                Me._FailBitPatternNumeric.Value = sender.LowerLimitFailureBits
            Case NameOf(BinningInfo.LowerLimit)
                Me._ResistanceLowLimitNumeric.Value = CDec(0.000001 * sender.LowerLimit)
            Case NameOf(BinningInfo.PassBits)
                Me._PassBitPatternNumeric.Value = sender.PassBits
        End Select
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _BinningInfo_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _BinningInfo.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._BinningInfo_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, VI.Scpi.BinningInfo), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling the binning info property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

    Private WithEvents _ActiveInsulationResistance As InsulationResistance

    ''' <summary> Gets or sets the active limit. </summary>
    ''' <value> The active limit. </value>
    Public ReadOnly Property ActiveInsulationResistance As InsulationResistance
        Get
            Return Me._ActiveInsulationResistance
        End Get
    End Property

    Private Overloads Sub OnPropertyChanged(ByVal sender As InsulationResistance, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.InsulationResistance.ContactCheckEnabled)
                Me._ContactCheckToggle.SafeCheckedSetter(sender.ContactCheckEnabled)
            Case NameOf(VI.InsulationResistance.CurrentLimit)
                Me._CurrentLimitNumeric.SafeValueSetter(0.00001 * sender.CurrentLimit)
            Case NameOf(VI.InsulationResistance.DwellTime)
            Case NameOf(VI.InsulationResistance.PowerLineCycles)
                Me._ApertureNumeric.SafeValueSetter(sender.PowerLineCycles)
            Case NameOf(VI.InsulationResistance.ResistanceLowLimit)
                Me._ResistanceLowLimitNumeric.SafeValueSetter(0.00001 * sender.ResistanceLowLimit)
            Case NameOf(VI.InsulationResistance.ResistanceRange)
                Me._ResistanceRangeNumeric.SafeValueSetter(0.000001 * sender.ResistanceRange)
            Case NameOf(VI.InsulationResistance.VoltageLevel)
                Me._VoltageLevelNumeric.SafeValueSetter(sender.VoltageLevel)
        End Select
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ActiveInsulationResistance_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _ActiveInsulationResistance.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._ActiveInsulationResistance_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, InsulationResistance), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling insulation resistance property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Tests configure hipot start. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    '''                                             null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="binning">    The binning info. </param>
    ''' <param name="insulation"> The insulation info. </param>
    '''
    ''' ### <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    '''                                          illegal values. </exception>
    Private Sub ConfigureHipotStartTest(ByVal binning As VI.Scpi.BinningInfo,
                                        ByVal insulation As InsulationResistance)

        If binning Is Nothing Then Throw New ArgumentNullException(NameOf(binning))
        If insulation Is Nothing Then Throw New ArgumentNullException(NameOf(insulation))

        Me.ErrorProvider.Clear()
        Me.Device.ClearExecutionState()
        Me.Device.OutputSubsystem.WriteOutputOnState(False)

        ' ======
        ' CALC2

        ' default: Me.Session.Write(":CALC2:CLIM:BCON IMM")
        ' Me.Session.Write(":CALC2:CLIM:CLE")
        Me.Device.CompositeLimit.ClearLimits()

        ' default: Me.Session.Write(":CALC2:CLIM:CLE:AUTO ON")
        Me.Device.CompositeLimit.ApplyAutoClearEnabled(True)

        ' default: Me.Session.Write(":CALC2:CLIM:MODE GRAD")
        Me.Device.CompositeLimit.ApplyLimitMode(LimitMode.Grading)

        ' Make limit comparisons on resistance reading
        ' Me.Session.Write(":CALC2:FEED RES")
        Me.Device.Calculate2Subsystem.ApplyFeedSource(Scpi.FeedSource.Resistance)

        ' Compliance Limit: send BAD code if measurement is in compliance
        ' default: Me.Session.Write(":CALC2:LIM1:COMP:FAIL IN")
        Me.Device.ComplianceLimit.ApplyIncomplianceCondition(True)

        ' Set fail bin
        ' Me.Session.Write(String.Format(Globalization.CultureInfo.InvariantCulture, ":CALC2:LIM1:COMP:SOUR2 {0}", Me._FailBitPatternNumeric.Value))
        Me.Device.ComplianceLimit.ApplyFailureBits(binning.FailureBits)
        binning.FailureBits = Me.Device.ComplianceLimit.FailureBits.GetValueOrDefault(0)

        ' Me.Session.Write(":CALC2:LIM1:STAT 1")
        Me.Device.ComplianceLimit.ApplyEnabled(binning.Enabled)
        binning.Enabled = Me.Device.ComplianceLimit.Enabled.GetValueOrDefault(False)

        ' Lower Limit
        With Me.Device.UpperLowerLimit
            ' Me.Session.Write(":CALC2:LIM2:UPP 1E+19")
            .ApplyUpperLimit(binning.UpperLimit)
            binning.UpperLimit = .UpperLimit.GetValueOrDefault(0)
            ' ":CALC2:LIM2:UPP:SOUR2 {0}", Me._FailBitPatternNumeric.Value
            .ApplyUpperLimitFailureBits(binning.UpperLimitFailureBits)
            binning.UpperLimitFailureBits = .UpperLimitFailureBits.GetValueOrDefault(0)
            ' ":CALC2:LIM2:LOW {0}", Me._ResistanceLowLimitNumeric.Value
            .ApplyLowerLimit(binning.LowerLimit)
            binning.LowerLimit = .LowerLimit.GetValueOrDefault(0)
            ' ":CALC2:LIM2:LOW:SOUR2 {0}", Me._FailBitPatternNumeric.Value))
            .ApplyLowerLimitFailureBits(binning.LowerLimitFailureBits)
            binning.LowerLimitFailureBits = .LowerLimitFailureBits.GetValueOrDefault(0)
            ' Me.Session.Write(":CALC2:LIM2:STAT 1")
            .ApplyEnabled(binning.Enabled)
            binning.Enabled = .Enabled.GetValueOrDefault(False)
        End With

        ' Send GOOD code if measurement is in range
        With Device.CompositeLimit
            ' ":CALC2:CLIM:PASS:SOUR2 {0}", Me._PassBitPatternNumeric.Value
            .ApplyPassBits(binning.PassBits)
            binning.PassBits = .PassBits.GetValueOrDefault(0)
            ' Me.Session.Write(":CALC2:CLIM:BCON IMM")
            .ApplyBinningControl(BinningControl.Immediate)
        End With

        ' enable contact check
        If insulation.ContactCheckEnabled Then
            With Device.ContactCheckLimit
                ' Me.Session.Write(":CALC2:LIM4:STAT ON")
                .ApplyEnabled(insulation.ContactCheckEnabled)
                insulation.ContactCheckEnabled = .Enabled.GetValueOrDefault(False)
                If Me._ContactCheckBitPatternNumeric.Value > 0 Then
                    .ApplyFailureBits(CInt(Me._ContactCheckBitPatternNumeric.Value))
                End If
            End With
            ' this is done when setting the insulation test: 
            ' Me.Session.Write(":SYSTem:CCH ON")
        Else
            With Device.ContactCheckLimit
                ' Me.Session.Write(":CALC2:LIM4:STAT OFF")
                .ApplyEnabled(insulation.ContactCheckEnabled)
                insulation.ContactCheckEnabled = .Enabled.GetValueOrDefault(True)
            End With
            ' this is done when setting the insulation test: 
            ' Me.Session.Write(":SYSTem:CCH OFF")
        End If

        ' ======
        ' EOT
        ' Set byte size to 3 enabling EOT mode.
        ' Me.Session.Write(":SOUR2:BSIZ 3")
        Me.Device.DigitalOutput.ApplyBitSize(3)

        ' Set the output logic to high
        ' Me.Session.Write(":SOUR2:TTL #b000")
        Me.Device.DigitalOutput.ApplyLevel(0)

        ' Set Digital I/O Mode to EOT
        ' Me.Session.Write(":SOUR2:TTL4:MODE EOT")
        Me.Device.DigitalOutput.ApplyOutputMode(VI.Scpi.OutputMode.EndTest)

        ' Set EOT polarity to HI
        ' Me.Session.Write(":SOUR2:TTL4:BST HI")
        Me.Device.DigitalOutput.ApplyOutputSignalPolarity(VI.Scpi.OutputSignalPolarity.High)

        ' Set the output level to set automatically to the
        ' :TTL level after the pass or fail output bit
        ' pattern or a limit test is sent to the handler.
        ' this can be turned off and the clear command issued on each start.
        ' Me.Session.Write(":SOUR2:CLE")
        Me.Device.DigitalOutput.ClearOutput()
        ' Me.Session.Write(":SOUR2:CLE:AUTO ON")
        Me.Device.DigitalOutput.ApplyAutoClearEnabled(True)

        ' Set the duration of the EOT strobe
        ' Me.Session.Write(":SOUR2:CLE:AUTO:DEL " & CStr(Me._EotStrobeDurationNumeric.Value))
        Me.Device.DigitalOutput.ApplyDelay(TimeSpan.FromMilliseconds(Me._EotStrobeDurationNumeric.Value))

        ' =================
        ' ARM MODEL:

        ' arm to immediate mode.
        ' default is IMM:  Me.Session.Write(":ARM:SOUR IMM")

        ' Set ARM counter to 1
        ' default is 1: Me.Session.Write(":ARM:COUN 1")

        ' Define Trigger layer to interface with the
        ' Trigger Master board:

        ' clear any pending triggers.
        ' Me.Session.Write(":TRIG:CLE")
        Me.Device.TriggerSubsystem.ClearTriggers()

        ' Set TRIGGER input line to Trigger Link line number
        ' default. to be set when starting: Me.Session.Write(":TRIG:ILIN 1")

        ' Set TRIGGER output line to Trigger Link line number
        ' default: Me.Session.Write(":TRIG:OLIN 2")

        ' Set Input trigger to Acceptor
        ' Me.Session.Write(":TRIG:DIR ACC")
        'Me.Device.TriggerSubsystem.ApplyDirection(Direction.Acceptor)

        ' Me.Session.Write(":TRIG:DIR SOUR")
        'Me.Device.TriggerSubsystem.ApplyDirection(Direction.Source)
        Me.Device.TriggerSubsystem.ApplyDirection(binning.TriggerDirection)
        If binning.TriggerDirection <> Me.Device.TriggerSubsystem.Direction.GetValueOrDefault(Scpi.Direction.None) Then
            Throw New VI.Pith.OperationFailedException($"Failed setting trigger direction to {binning.TriggerDirection};. Value set to {Me.Device.TriggerSubsystem.Direction}")
        End If

        ' start with immediate trigger allowing non-triggered measurements.
        ' Me.Session.Write(":TRIG:SOUR IMM")
        ' see below: Me.Device.TriggerSubsystem.ApplyTriggerSource(TriggerSource.Immediate)

        ' no outputs triggers
        ' default is none: Me.Session.Write(":TRIG:OUTP NONE")

        ' A single trigger.
        ' default is 1: Me.Session.Write(":TRIG:COUN 1")

        ' default: Me.Session.Write(":TRIG:INP NONE")

        ' set trigger source to immediate
        Me.Device.TriggerSubsystem.ApplyTriggerSource(binning.TriggerSource)
        If binning.TriggerSource <> Me.Device.TriggerSubsystem.TriggerSource.GetValueOrDefault(Scpi.TriggerSources.None) Then
            Throw New VI.Pith.OperationFailedException($"Failed setting trigger source to {binning.TriggerSource};. Value set to {Me.Device.TriggerSubsystem.TriggerSource}")
        End If

        Me.Device.ArmLayerSubsystem.ApplyDirection(binning.ArmDirection)
        If binning.ArmDirection <> Me.Device.ArmLayerSubsystem.Direction.GetValueOrDefault(Scpi.Direction.None) Then
            Throw New VI.Pith.OperationFailedException($"Failed setting Arm direction to {binning.ArmDirection};. Value set to {Me.Device.ArmLayerSubsystem.Direction}")
        End If

        Me.Device.ArmLayerSubsystem.ApplyArmSource(binning.ArmSource)
        binning.ArmSource = Me.Device.ArmLayerSubsystem.ArmSource.GetValueOrDefault(Scpi.ArmSources.None)

        Me.Device.ArmLayerSubsystem.ApplyArmCount(binning.ArmCount)
        binning.ArmCount = Me.Device.ArmLayerSubsystem.ArmCount.GetValueOrDefault(0)

    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySotSettingsButton_Click(sender As Object, e As EventArgs) Handles _ApplySotSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplyHipotBinningInfo()
            Me.ApplyHipotSettings()
            Me.ConfigureHipotStartTest(Me.BinningInfo, Me.ActiveInsulationResistance)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed applying trigger settings")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred applying trigger settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ApplyHipotBinningInfo()
        With Me.BinningInfo
            .TriggerDirection = Scpi.Direction.Source
            .TriggerSource = Scpi.TriggerSources.Immediate
            .ArmSource = Me.SelectedArmSource
            If .ArmSource = Scpi.ArmSources.Manual Then
                .ArmCount = 1
            Else
                .ArmCount = 0
            End If
            .FailureBits = CInt(Me._FailBitPatternNumeric.Value)
            .UpperLimit = isr.VI.Pith.Scpi.Syntax.Infinity
            .UpperLimitFailureBits = CInt(Me._FailBitPatternNumeric.Value)
            .LowerLimit = 1000000.0 * Me._ResistanceLowLimitNumeric.Value
            .LowerLimitFailureBits = CInt(Me._FailBitPatternNumeric.Value)
            .PassBits = CInt(Me._PassBitPatternNumeric.Value)
            .Enabled = True
        End With
    End Sub

    ''' <summary>
    ''' Configures the high potential measurement.
    ''' </summary>
    Public Sub ConfigureHipot(ByVal hipotSettings As InsulationResistance)
        If hipotSettings Is Nothing Then Throw New ArgumentNullException(NameOf(hipotSettings))

        ' make sure the 2400 output is off
        Me.Device.OutputSubsystem.ApplyOutputOnState(False)

#If False Then
    Me.Session.Write("*RST")
    Me.Session.Write(":OUTP OFF")
    Me.Session.Write(":ROUT:TERM REAR")
    Me.Session.Write(":SENS:FUNC ""RES""")
    Me.Session.Write(":SENS:RES:MODE MAN")
    Me.Session.Write(":SOUR:CLE:AUTO ON")
    Me.Session.Write(":SOUR:DEL 5")
    Me.Session.Write(":SYST:RSEN ON")
    Me.Session.Write(":SOUR:FUNC VOLT")
    Me.Session.Write(":SOUR:VOLT:RANG 5")
    Me.Session.Write(":SOUR:VOLT 5")
    Me.Session.Write(":SENS:CURR:RANG 10e-6")
    Me.Session.Write(":SENS:CURR:PROT 10e-6")
    Me.Session.Write(":SENS:VOLT:NPLC 1")
    Me.Session.Write(":SENS:CURR:NPLC 1")
    Me.Session.Write(":SENS:RES:NPLC 1")
    Me.Session.Write(":SYST:AZER ONCE")
    Me.Session.Write(":FORM:ELEM VOLT,CURR,RES,STATUS")
    Me.Session.Write(":SOUR:CLE:AUTO ON")
    Dim value As String = Me.Session.Query(":READ?")
#End If

        ' initialize the known state.
        Me.Device.ResetClearInit()
        Me.Device.RouteSubsystem.ApplyTerminalsMode(Scpi.RouteTerminalsMode.Rear)
        Me.Device.SenseSubsystem.ApplyFunctionModes(SourceMeasure.SenseFunctionModes.Resistance)

        ' set to manual mode to require manually setting the measurement as voltage source
        Me.Device.SenseResistanceSubsystem.ApplyConfigurationMode(Scpi.ConfigurationMode.Manual)

        ' the source must be set first.
        Me.Device.SourceSubsystem.ApplyAutoClearEnabled(True)

        Me.Device.SourceSubsystem.ApplyDelay(hipotSettings.DwellTime)
        hipotSettings.DwellTime = Me.Device.SourceSubsystem.Delay.GetValueOrDefault(TimeSpan.Zero)

        Me.Device.SourceSubsystem.ApplyFunctionMode(Scpi.SourceFunctionModes.Voltage)

        Me.Device.SourceVoltageSubsystem.ApplyRange(hipotSettings.VoltageLevel)

        Me.Device.SourceVoltageSubsystem.ApplyLevel(hipotSettings.VoltageLevel)
        hipotSettings.VoltageLevel = Me.Device.SourceVoltageSubsystem.Level.GetValueOrDefault(0)

        Me.Device.SenseCurrentSubsystem.ApplyRange(hipotSettings.CurrentRange)

        Me.Device.SenseCurrentSubsystem.ApplyProtectionLevel(hipotSettings.CurrentLimit)
        hipotSettings.CurrentLimit = Me.Device.SenseCurrentSubsystem.ProtectionLevel.GetValueOrDefault(0)

        Me.Device.SenseResistanceSubsystem.ApplyPowerLineCycles(hipotSettings.PowerLineCycles)
        hipotSettings.PowerLineCycles = Me.Device.SenseResistanceSubsystem.PowerLineCycles.GetValueOrDefault(0)

        ' Enable four wire connection
        Me.Device.SystemSubsystem.ApplyFourWireSenseEnabled(True)

        ' force immediate update of auto zero
        Me.Device.SystemSubsystem.ApplyAutoZeroEnabled(True)

        Me.Device.SystemSubsystem.ApplyContactCheckEnabled(hipotSettings.ContactCheckEnabled)
        hipotSettings.ContactCheckEnabled = Me.Device.SystemSubsystem.ContactCheckEnabled.GetValueOrDefault(False)
        Me.Device.ContactCheckLimit.ApplyEnabled(hipotSettings.ContactCheckEnabled)
        hipotSettings.ContactCheckEnabled = Me.Device.ContactCheckLimit.Enabled.GetValueOrDefault(False)

        Me.Device.FormatSubsystem.ApplyElements(ReadingTypes.Voltage Or ReadingTypes.Current Or
                                                ReadingTypes.Resistance Or ReadingTypes.Status)

        ' Me.Device.MeasureSubsystem.Readings.ResistanceReading.LowLimit = 1000000.0 * Me._ResistanceRangeNumeric.Value
        Me.Device.MeasureSubsystem.Readings.ResistanceReading.LowLimit = hipotSettings.ResistanceLowLimit

        ' update the timeout to reflect the dwell time.
        If Me.Device.SourceSubsystem.Delay.HasValue Then
            Dim totalTestTime As TimeSpan = Me.Device.SourceSubsystem.Delay.Value
            totalTestTime = totalTestTime.Add(Me.Device.SenseResistanceSubsystem.IntegrationPeriod.GetValueOrDefault(TimeSpan.Zero))
            totalTestTime = totalTestTime.Add(Me.Device.TriggerSubsystem.Delay.GetValueOrDefault(TimeSpan.Zero))
            ' set this as the minimum timeout
            totalTestTime = totalTestTime.Add(TimeSpan.FromSeconds(2))
            If totalTestTime > Me.Device.Session.Timeout Then
                Me.Device.Session.Timeout = totalTestTime
            End If
        End If
    End Sub

    ''' <summary> Configure hipot change. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="hipotSettings"> The hipot settings. </param>
    Public Sub ConfigureHipotChange(ByVal hipotSettings As InsulationResistance)
        If hipotSettings Is Nothing Then Throw New ArgumentNullException(NameOf(hipotSettings))

        ' make sure the 2400 output is off
        Me.Device.OutputSubsystem.ApplyOutputOnState(False)

        If Not Nullable.Equals(Me.Device.RouteSubsystem.TerminalsMode, Scpi.OutputTerminalsMode.Rear) Then
            Me.Device.RouteSubsystem.ApplyTerminalsMode(Scpi.RouteTerminalsMode.Rear)
        End If
        If Not Nullable.Equals(Me.Device.SenseSubsystem.FunctionMode, SourceMeasure.SenseFunctionModes.Resistance) Then
            Me.Device.SenseSubsystem.ApplyFunctionModes(SourceMeasure.SenseFunctionModes.Resistance)
        End If

        ' set to manual mode to require manually setting the measurement as voltage source
        If Not Nullable.Equals(Me.Device.SenseResistanceSubsystem.ConfigurationMode, Scpi.ConfigurationMode.Manual) Then
            Me.Device.SenseResistanceSubsystem.ApplyConfigurationMode(Scpi.ConfigurationMode.Manual)
        End If


        ' the source must be set first.
        If Not Nullable.Equals(Me.Device.SourceSubsystem.AutoClearEnabled, True) Then
            Me.Device.SourceSubsystem.ApplyAutoClearEnabled(True)
        End If

        If Not Nullable.Equals(Me.Device.SourceSubsystem.Delay, hipotSettings.DwellTime) Then
            Me.Device.SourceSubsystem.ApplyDelay(hipotSettings.DwellTime)
            hipotSettings.DwellTime = Me.Device.SourceSubsystem.Delay.GetValueOrDefault(TimeSpan.Zero)
        End If

        If Not Nullable.Equals(Me.Device.SourceSubsystem.FunctionMode, Scpi.SourceFunctionModes.Voltage) Then
            Me.Device.SourceSubsystem.ApplyFunctionMode(Scpi.SourceFunctionModes.Voltage)
        End If

        If Not Nullable.Equals(Me.Device.SourceVoltageSubsystem.Range, hipotSettings.VoltageLevel) Then
            Me.Device.SourceVoltageSubsystem.ApplyRange(hipotSettings.VoltageLevel)
        End If

        If Not Nullable.Equals(Me.Device.SourceVoltageSubsystem.Level, hipotSettings.VoltageLevel) Then
            Me.Device.SourceVoltageSubsystem.ApplyLevel(hipotSettings.VoltageLevel)
            hipotSettings.VoltageLevel = Me.Device.SourceVoltageSubsystem.Level.GetValueOrDefault(0)
        End If

        If Not Nullable.Equals(Me.Device.SenseCurrentSubsystem.Range, hipotSettings.CurrentRange) Then
            Me.Device.SenseCurrentSubsystem.ApplyRange(hipotSettings.CurrentRange)
        End If

        If Not Nullable.Equals(Me.Device.SenseCurrentSubsystem.ProtectionLevel, hipotSettings.CurrentRange) Then
            Me.Device.SenseCurrentSubsystem.ApplyProtectionLevel(hipotSettings.CurrentLimit)
            hipotSettings.CurrentLimit = Me.Device.SenseCurrentSubsystem.ProtectionLevel.GetValueOrDefault(0)
        End If

        If Not Nullable.Equals(Me.Device.SenseResistanceSubsystem.PowerLineCycles, hipotSettings.PowerLineCycles) Then
            Me.Device.SenseResistanceSubsystem.ApplyPowerLineCycles(hipotSettings.PowerLineCycles)
            hipotSettings.PowerLineCycles = Me.Device.SenseResistanceSubsystem.PowerLineCycles.GetValueOrDefault(0)
        End If

        ' Enable four wire connection
        If Not Nullable.Equals(Me.Device.SystemSubsystem.FourWireSenseEnabled, True) Then
            Me.Device.SystemSubsystem.ApplyFourWireSenseEnabled(True)
        End If

        ' force immediate update of auto zero
        If Not Nullable.Equals(Me.Device.SystemSubsystem.AutoZeroEnabled, True) Then
            Me.Device.SystemSubsystem.ApplyAutoZeroEnabled(True)
        End If

        If Not Nullable.Equals(Me.Device.SystemSubsystem.ContactCheckEnabled, hipotSettings.ContactCheckEnabled) Then
            Me.Device.SystemSubsystem.ApplyContactCheckEnabled(hipotSettings.ContactCheckEnabled)
            hipotSettings.ContactCheckEnabled = Me.Device.SystemSubsystem.ContactCheckEnabled.GetValueOrDefault(False)
        End If

        If Not Nullable.Equals(Me.Device.ContactCheckLimit.Enabled, hipotSettings.ContactCheckEnabled) Then
            Me.Device.ContactCheckLimit.ApplyEnabled(hipotSettings.ContactCheckEnabled)
            hipotSettings.ContactCheckEnabled = Me.Device.ContactCheckLimit.Enabled.GetValueOrDefault(False)
        End If

        Dim elements As ReadingTypes = ReadingTypes.Voltage Or ReadingTypes.Current Or
                                                ReadingTypes.Resistance Or ReadingTypes.Status
        If Not Nullable.Equals(Me.Device.FormatSubsystem.Elements, elements) Then
            Me.Device.FormatSubsystem.ApplyElements(elements)
        End If

        If Not Nullable.Equals(Me.Device.MeasureSubsystem.Readings.ResistanceReading.LowLimit, hipotSettings.ResistanceLowLimit) Then
            Me.Device.MeasureSubsystem.Readings.ResistanceReading.LowLimit = hipotSettings.ResistanceLowLimit
        End If

        ' update the timeout to reflect the dwell time.
        If Me.Device.SourceSubsystem.Delay.HasValue Then
            Dim totalTestTime As TimeSpan = Me.Device.SourceSubsystem.Delay.Value
            totalTestTime = totalTestTime.Add(Me.Device.SenseResistanceSubsystem.IntegrationPeriod.GetValueOrDefault(TimeSpan.Zero))
            totalTestTime = totalTestTime.Add(Me.Device.TriggerSubsystem.Delay.GetValueOrDefault(TimeSpan.Zero))
            ' set this as the minimum timeout
            totalTestTime = totalTestTime.Add(TimeSpan.FromSeconds(2))
            If totalTestTime > Me.Device.Session.Timeout Then
                Me.Device.Session.Timeout = totalTestTime
            End If
        End If
    End Sub

    ''' <summary> Applies the hipot settings. </summary>
    Private Sub ApplyHipotSettings()
        If Me.ActiveInsulationResistance Is Nothing Then Me.AssignInsulationTest()
        With Me.ActiveInsulationResistance
            .DwellTime = TimeSpan.FromSeconds(Me._DwellTimeNumeric.Value)
            .VoltageLevel = Me._VoltageLevelNumeric.Value
            .CurrentLimit = 0.000001 * Me._CurrentLimitNumeric.Value
            .PowerLineCycles = Me._ApertureNumeric.Value
            .ContactCheckEnabled = Me._ContactCheckToggle.Checked
            .ResistanceLowLimit = 1000000.0 * Me._ResistanceLowLimitNumeric.Value
            .ResistanceRange = 1000000.0 * Me._ResistanceRangeNumeric.Value
        End With
    End Sub

    ''' <summary> Applies the hipot settings button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyHipotSettingsButton_Click(sender As Object, e As EventArgs) Handles _ApplyHipotSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplyHipotSettings()
            Me.ConfigureHipot(Me.ActiveInsulationResistance)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed applying high potential settings")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred applying high potential settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Clears interface. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearInterfaceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearInterfaceMenuItem.Click
        Dim activity As String = "clearing interface"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearInterface()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears device (SDC). </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim activity As String = "clearing device active state (SDC)"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearActiveState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears (CLS) the execution state menu item click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearExecutionStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearExecutionStateMenuItem.Click
        Dim activity As String = "clearing the execution state"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearExecutionState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    ''' <summary> Resets (RST) the known state menu item click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetKnownStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ResetKnownState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Initializes to known state menu item click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitKnownStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                'Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                'Me.Device.ResetKnownState()
                activity = "initializing known state"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.InitKnownState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region " TRACE LEVEL "

    ''' <summary> Enables the trace level controls. </summary>
    Private Sub EnableTraceLevelControls()

        TalkerControlBase.ListTraceEventLevels(Me._LogTraceLevelComboBox.ComboBox)
        AddHandler Me._LogTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._LogTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.ListTraceEventLevels(Me._DisplayTraceLevelComboBox.ComboBox)
        AddHandler Me._DisplayTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._DisplayTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.SelectItem(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel)
        TalkerControlBase.SelectItem(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel)

    End Sub

    ''' <summary>
    ''' Event handler. Called by _LogTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LogTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting log trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Logger,
                                            TalkerControlBase.SelectedValue(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Event handler. Called by _DisplayTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DisplayTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting Display trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Display,
                                            TalkerControlBase.SelectedValue(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: SESSION "

    ''' <summary> Handles Reads Status Byte Menu Item click event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadStatusByteMenuItem_Click(ByVal sender As Object, e As System.EventArgs) Handles _ReadStatusByteMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "reading status byte"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.StartElapsedStopwatch()
                Me.ReadServiceRequestStatus()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles session message tracing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionNotificationLevelComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _SessionNotificationLevelComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting session notification level"
        Dim combo As Core.Controls.ToolStripComboBox = TryCast(sender, Core.Controls.ToolStripComboBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If combo IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.MessageNotificationLevel = VI.Pith.SessionBase.SelectedValue(combo, NotifySyncLevel.None)
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the session service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Toggle session service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                        Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, VI.Pith.ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the Device service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _DeviceServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Toggle device service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

    ''' <summary> Selects a new reading to display. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    ''' <returns> The VI.ReadingElements. </returns>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReadingType) Then
            Me._ReadingComboBox.ComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedReadingType
    End Function

    ''' <summary> Gets the type of the selected reading. </summary>
    ''' <value> The type of the selected reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedReadingType() As VI.ReadingTypes
        Get
            Return CType(CType(Me._ReadingComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.ReadingTypes)
        End Get
    End Property

    ''' <summary> Abort button click. </summary>
    ''' <param name="sender"> <see cref="T:System.Object" />
    '''                                             instance of this
    '''                       <see cref="T:System.Windows.Forms.Control" /> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortButton_Click(sender As Object, e As EventArgs) Handles _AbortButton.Click

        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "aborting measurements(s)"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.TriggerSubsystem.Abort()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by InitButton for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateButton.Click
        Dim activity As String = "initiating a measurement"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Reading combo box selected value changed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting reading type"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.MeasureSubsystem.SelectActiveReading(Me.SelectedReadingType)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Dim activity As String = "reading"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.MeasureSubsystem.Read()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary>
    ''' Handles the DataError event of the _dataGridView control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingsDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles _ReadingsDataGridView.DataError
        Try
            ' prevent error reporting when adding a new row or editing a cell
            Dim grid As DataGridView = TryCast(sender, DataGridView)
            If grid IsNot Nothing Then
                If grid.CurrentRow IsNot Nothing AndAlso grid.CurrentRow.IsNewRow Then Return
                If grid.IsCurrentCellInEditMode Then Return
                If grid.IsCurrentRowDirty Then Return
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   $"{e.Exception} occurred editing row {e.RowIndex} column {e.ColumnIndex};. {e.Exception.ToFullBlownString}")
                Me.ErrorProvider.Annunciate(grid, $"{e.Exception} occurred editing table")
            End If
        Catch
        End Try
    End Sub

    ''' <summary> Reads buffer button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceButton_Click(sender As Object, e As EventArgs) Handles _TraceButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "reading readings"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            'Dim values As IEnumerable(Of Readings) = Me.Device.TraceSubsystem.QueryReadings(Me.Device.MeasureSubsystem.Readings)
            'Me._ReadingsCountLabel.Text = values?.Count.ToString
            'Me.Device.TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, values)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears the buffer display button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearBufferDisplayButton_Click(sender As Object, e As EventArgs) Handles _ClearBufferDisplayButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "clearing buffer display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            'Me.Device.TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, New List(Of Readings))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub OnPropertyChanged(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(Instrument.SimpleReadWriteControl.StatusMessage)
                    Me.StatusLabel.Text = sender.StatusMessage
                Case NameOf(Instrument.SimpleReadWriteControl.ServiceRequestValue)
                    Me.StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
            End Select
        End If
    End Sub

    ''' <summary> Event handler. Called by <see crefname="_SimpleReadWriteControl"/> for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling {0} property change;. {1}",
                               e?.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        Me._SimpleReadWriteControl.AssignTalker(talker)
        MyBase.AssignTalker(talker)
        ' My.MyLibrary.Identify(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me._SimpleReadWriteControl.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

End Class

