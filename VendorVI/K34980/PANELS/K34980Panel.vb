Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
''' <summary> Provides a user interface for the Keithley 27XX Device. </summary>
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
<System.ComponentModel.DisplayName("K34980 Panel"),
      System.ComponentModel.Description("Keysight 34980 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(K34980Panel))>
Public Class K34980Panel
    Inherits VI.Instrument.ResourcePanelBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="device"> The device. </param>
    Protected Sub New(ByVal device As Device)
        MyBase.New(device)
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me._AssignDevice(device)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <remarks> David, 12/22/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", "Exception details: {0}", ex)
                End Try
                ' the device gets disposed in the base class!
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENTS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <remarks> David, 1/4/2016. </remarks>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me.AddListeners()
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
    End Sub

    ''' <summary> Gets a reference to the Keysight 34980 Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    ''' <remarks> David, 3/3/2016. </remarks>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        Dim isOpen As Boolean = CType(device?.IsDeviceOpen, Boolean?).GetValueOrDefault(False)
        If isOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then
                For Each c As Windows.Forms.Control In t.Controls : Me.RecursivelyEnable(c, isOpen) : Next
            End If
        Next
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.IsServiceRequestEventEnabled)
                Me._HandleServiceRequestsCheckBox.Checked = device.IsServiceRequestEventEnabled
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.Device.FormatSubsystem IsNot Nothing Then AddHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        If Me.Device.MeasureSubsystem IsNot Nothing Then AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
        If Me.Device.RouteSubsystem IsNot Nothing Then AddHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
        If Me.Device.SenseSubsystem IsNot Nothing Then AddHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        If Me.Device.SenseVoltageSubsystem IsNot Nothing Then AddHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
        If Me.Device.SenseCurrentSubsystem IsNot Nothing Then AddHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
        If Me.Device.SenseFourWireResistanceSubsystem IsNot Nothing Then AddHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
        If Me.Device.SenseResistanceSubsystem IsNot Nothing Then AddHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
        'AddHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
        'AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <remarks> David, 1/14/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(Title)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            If Me.Device.FormatSubsystem IsNot Nothing Then RemoveHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
            If Me.Device.MeasureSubsystem IsNot Nothing Then RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            If Me.Device.RouteSubsystem IsNot Nothing Then RemoveHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
            If Me.Device.SenseCurrentSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
            If Me.Device.SenseFourWireResistanceSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
            If Me.Device.SenseResistanceSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
            If Me.Device.SenseVoltageSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
            If Me.Device.SenseSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
            'RemoveHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
            'RemoveHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " FORMAT "

    ''' <summary> Handle the format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(subsystem.Elements)
                If Me.Device IsNot Nothing AndAlso subsystem.Elements <> ReadingTypes.None Then
                    Dim selectedIndex As Integer = Me._ReadingComboBox.SelectedIndex
                    With Me._ReadingComboBox
                        .DataSource = Nothing
                        .Items.Clear()
                        .DataSource = GetType(VI.ReadingTypes).ValueDescriptionPairs(subsystem.Elements And Not ReadingTypes.Units)
                        .DisplayMember = "Value"
                        .ValueMember = "Key"
                        If .Items.Count > 0 Then
                            .SelectedIndex = Math.Max(selectedIndex, 0)
                        End If
                    End With
                End If
        End Select
    End Sub

    ''' <summary> Format subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FormatSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, FormatSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Format Subsystem property changed Event;. Failed property {0}. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " MEASURE "

    ''' <summary> Displays the active reading caption and status. </summary>
    ''' <remarks> David, 3/18/2016. </remarks>
    Private Sub DisplayActiveReading()
        Const clear As String = "    "
        Dim caption As String = clear
        Dim failureCaption As String = clear
        Dim failureToolTip As String = clear
        Dim tbdCaption As String = clear
        If Me.Device.MeasureSubsystem Is Nothing OrElse
            Me.Device.MeasureSubsystem.Readings Is Nothing OrElse
            Me.Device.MeasureSubsystem.Readings.ActiveReadingType = ReadingTypes.None Then
            caption = "-.------- :)"
        ElseIf Me.Device.MeasureSubsystem.Readings.IsEmpty Then
            caption = Me.Device.MeasureSubsystem.Readings.ActiveAmountCaption
        Else
            caption = Me.Device.MeasureSubsystem.Readings.ActiveAmountCaption
            Dim metaStatus As MetaStatus = Me.Device.MeasureSubsystem.Readings.ActiveMetaStatus
            If metaStatus.HasValue Then
                failureCaption = $"{metaStatus.ToShortDescription(""),4}"
                failureToolTip = metaStatus.ToLongDescription("")
                If String.IsNullOrEmpty(failureToolTip) Then
                    Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instruments parsed reading elements.")
                Else
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, failureToolTip)
                End If
            End If
        End If
        Me._ReadingToolStripStatusLabel.SafeTextSetter(caption)
        Me._FailureCodeToolStripStatusLabel.SafeTextSetter(failureCaption)
        Me._FailureCodeToolStripStatusLabel.SafeToolTipTextSetter(failureToolTip)
        Me._TbdToolStripStatusLabel.SafeTextSetter(tbdCaption)
    End Sub

    ''' <summary> Handles the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(subsystem.LastReading)
                Me._LastReadingTextBox.SafeTextSetter(subsystem.LastReading)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Measure message: {0}.", subsystem.LastReading.InsertCommonEscapeSequences)
            Case NameOf(subsystem.MeasurementAvailable)
                Me.DisplayActiveReading()
        End Select
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, MeasureSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Measure Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " ROUTE "

    ''' <summary> Handles the ROUTE subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As RouteSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(subsystem.ClosedChannel)
                Me.ClosedChannels = subsystem.ClosedChannel
            Case NameOf(subsystem.ClosedChannels)
                Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

    ''' <summary> ROUTE subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RouteSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, RouteSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling ROUTE Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SENSE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onSupportedFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.SupportedFunctionModes <> VI.Scpi.SenseFunctionModes.None Then
            With Me._SenseFunctionComboBox
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = GetType(VI.Scpi.SenseFunctionModes).ValueDescriptionPairs(subsystem.SupportedFunctionModes)
                .DisplayMember = "Value"
                .ValueMember = "Key"
                If .Items.Count > 0 Then
                    .SelectedItem = VI.Scpi.SenseFunctionModes.VoltageDC.ValueDescriptionPair()
                End If
            End With
        End If
    End Sub

    Private Sub onFunctionModesChanged(ByVal value As SenseFunctionSubsystemBase)
        With Me._SenseRangeNumeric
            .Minimum = CDec(value.ValueRange1.Min)
            .Maximum = CDec(value.ValueRange1.Max)
            .DecimalPlaces = CInt(Math.Max(0, -Math.Log10(value.ValueRange1.Min)))
        End With
        With Me._PowerLineCyclesNumeric
            .Minimum = CDec(value.PowerLineCyclesRange1.Min)
            .Maximum = CDec(value.PowerLineCyclesRange1.Max)
            .DecimalPlaces = CInt(Math.Max(0, -Math.Log10(value.PowerLineCyclesRange1.Min)))
        End With
        With value
            .QueryRange()
            .QueryAutoRangeEnabled()
            .QueryAutoZeroEnabled()
            .QueryPowerLineCycles()
        End With
    End Sub

    Private ReadOnly Property SelectedSenseSubsystem() As SenseFunctionSubsystemBase
        Get
            Return Me.SelectSenseSubsystem(Me.selectedFunctionMode)
        End Get
    End Property

    Private Function SelectSenseSubsystem(ByVal value As VI.Scpi.SenseFunctionModes) As SenseFunctionSubsystemBase
        Dim result As SenseFunctionSubsystemBase = Me.Device.SenseVoltageSubsystem
        Select Case value
            Case VI.Scpi.SenseFunctionModes.CurrentDC
                result = Me.Device.SenseCurrentSubsystem
            Case VI.Scpi.SenseFunctionModes.VoltageDC
                result = Me.Device.SenseVoltageSubsystem
            Case VI.Scpi.SenseFunctionModes.Resistance
                result = Me.Device.SenseResistanceSubsystem
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = Me.Device.SenseFourWireResistanceSubsystem
            Case Else
        End Select
        Return result
    End Function

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    Private Sub onFunctionModesChanged(ByVal value As VI.Scpi.SenseFunctionModes)
        Me.onFunctionModesChanged(Me.SelectSenseSubsystem(value))
    End Sub

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.None)
            If value <> VI.Scpi.SenseFunctionModes.None Then
                If Not VI.Scpi.SenseSubsystemBase.TryParse(value, Me.Device.MeasureSubsystem.Readings.Reading.Unit) Then
                    Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                       "Failed parsing function mode '{0}' to a standard unit.", subsystem.FunctionMode.Value)
                    Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Volt
                End If
                subsystem.Readings.Reading.Unit = Me.Device.MeasureSubsystem.Readings.Reading.Unit
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.Readings.Reading.Unit.Symbol}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
                Me._SenseFunctionComboBox.SafeSelectItem(value, value.Description)
                Me.onFunctionModesChanged(value)
            End If
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.MeasurementAvailable)
                Me.DisplayActiveReading()
            Case NameOf(subsystem.SupportedFunctionModes)
                Me.onSupportedFunctionModesChanged(subsystem)
            Case NameOf(subsystem.FunctionMode)
                Me.onFunctionModesChanged(subsystem)
                Me.DisplayActiveReading()
        End Select
    End Sub

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SENSE VOLTAGE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseVoltageSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense voltage subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseVoltageSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Voltage Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SENSE CURRENT "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseCurrentSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Current subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseCurrentSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseCurrentSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Current Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " FOUR WIRE SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseFourWireResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseFourWireResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseFourWireResistanceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Resistance Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseResistanceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Resistance Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub onLastError(ByVal lastError As DeviceError)
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
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors Then
                    ' if no errors, this clears the error queue.
                    subsystem.QueryDeviceErrors()
                End If
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

    ''' <summary> Reads a service request status. </summary>
    ''' <remarks> David, 12/26/2015. </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystem.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. Details: {0}", ex)
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub


#End Region

#End Region

#Region " DISPLAY SETTINGS: READING "

    ''' <summary> Selects a new reading to display. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    ''' <returns> The VI.ReadingElements. </returns>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso
                (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReadingType) Then
            Me._ReadingComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedReadingType
    End Function

    ''' <summary> Gets the type of the selected reading. </summary>
    ''' <value> The type of the selected reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedReadingType() As VI.ReadingTypes
        Get
            Return CType(CType(Me._ReadingComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                                            Of [Enum], String)).Key, VI.ReadingTypes)
        End Get
    End Property

#End Region

#Region " DEVICE SETTINGS: FUNCTION MODE "

    ''' <summary>
    ''' Selects a new sense mode.
    ''' </summary>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Scpi.SenseFunctionModes)
        If Me.IsDeviceOpen Then
            Me._Device.SenseSubsystem.ApplyFunctionMode(value)
        End If
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property selectedFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Scpi.SenseFunctionModes)
        End Get
    End Property

#End Region

#Region " CHANNELS "

    ''' <summary> Gets or sets the scan list of closed channels. </summary>
    ''' <value> The closed channels. </value>
    Public Property ClosedChannels() As String
        Get
            Return Me._ClosedChannelsTextBox.Text
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Me._ClosedChannelsTextBox.SafeTextSetter("?")
            Else
                Me._ClosedChannelsTextBox.SafeTextSetter(value)
            End If
        End Set
    End Property

    ''' <summary> Adds new items to the combo box. </summary>
    Friend Sub updateChannelListComboBox()
        If Me.Visible Then
            ' check if we are asking for a new channel list
            If Me._ChannelListComboBox IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(Me._ChannelListComboBox.Text) AndAlso
                    Me._ChannelListComboBox.FindString(Me._ChannelListComboBox.Text) < 0 Then
                ' if we have a new string, add it to the channel list
                Me._ChannelListComboBox.Items.Add(Me._ChannelListComboBox.Text)
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called by _closeChannelsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _closeChannelsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CloseChannelsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            updateChannelListComboBox()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(1))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by channel_OpenButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OpenChannelsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenChannelsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            updateChannelListComboBox()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyOpenChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(1))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by CloseOnlyButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CloseOnlyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _CloseOnlyButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            updateChannelListComboBox()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(1))
            Me.Device.RouteSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(1))
            ' this works only if a single channel:
            ' VI.RouteSubsystem.CloseChannels(Me.Device, Me._channelListComboBox.Text)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by OpenAllButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenAllButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(1))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Event handler. Called by _SessionTraceEnableCheckBox for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionTraceEnableCheckBox_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _SessionTraceEnableCheckBox.CheckedChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Not Me.DesignMode AndAlso sender IsNot Nothing Then
                Dim checkBox As Windows.Forms.CheckBox = CType(sender, Windows.Forms.CheckBox)
                Me.Device.SessionMessagesTraceEnabled = checkBox.Checked
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by interfaceClearButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfaceClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InterfaceClearButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "{0} clearing interface;. {1}", Me.ResourceTitle, Me.ResourceName)
            Me.Device.SystemSubsystem.ClearInterface()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred clearing interface;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _SelectiveDeviceClearButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SelectiveDeviceClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SelectiveDeviceClearButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "{0} clearing selective device;. {1}", Me.ResourceTitle, Me.ResourceName)
            Me.Device.SystemSubsystem.ClearDevice()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred sending SDC;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Issue RST. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} resetting known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.ResetKnownState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred resetting known state;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _InitializeKnownStateButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitializeKnownStateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitializeKnownStateButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} resetting known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.ResetKnownState()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} initializing known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.InitKnownState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

    ''' <summary> Event handler. Called by InitButton for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub InitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Reading combo box selected value changed. </summary>
    ''' <remarks> David, 3/18/2016. </remarks>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedValueChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.MeasureSubsystem.Readings.ActiveReadingType = Me.SelectedReadingType
            Me.DisplayActiveReading()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred displaying a measurement;. Details: {0}", ex)
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
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.MeasureSubsystem?.Read()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _HandleServiceRequestsCheckBox for check state changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _HandleServiceRequestsCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _HandleServiceRequestsCheckBox.CheckStateChanged
        If Me._InitializingComponents Then Return
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)
        If checkBox IsNot Nothing AndAlso
                    Not checkBox.Checked = Me.Device.Session.IsServiceRequestEventEnabled Then
            If checkBox IsNot Nothing AndAlso checkBox.Checked Then
                Me.EnableServiceRequestEventHandler()
                Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
            Else
                Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                Me.DisableServiceRequestEventHandler()
            End If
            Me.Device.StatusSubsystem.ReadRegisters()
        End If
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary> Applies the function mode button click. </summary>
    ''' <remarks> David, 6/27/2016. </remarks>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyFunctionModeButton_Click(sender As Object, e As EventArgs) Handles _ApplyFunctionModeButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplyFunctionMode(Me.selectedFunctionMode)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Sense range setter. </summary>
    ''' <remarks> David, 2/10/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub SenseRangeSetter(ByVal value As Double)
        If value <= Me._SenseRangeNumeric.Maximum AndAlso value >= Me._SenseRangeNumeric.Minimum Then Me._SenseRangeNumeric.Value = CDec(value)
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub applySenseSettings()

        With Me.SelectedSenseSubsystem

            If Not Nullable.Equals(.PowerLineCycles, Me._PowerLineCyclesNumeric.Value) Then
                .ApplyPowerLineCycles(Me._PowerLineCyclesNumeric.Value)
            End If

            If Not Nullable.Equals(.AutoRangeEnabled, Me._SenseAutoRangeToggle.Checked) Then
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)
            End If

            If .AutoRangeEnabled Then
                .QueryRange()
            ElseIf Not Nullable.Equals(.Range, Me._SenseRangeNumeric.Value) Then
                .ApplyRange(Me._SenseRangeNumeric.Value)
            End If

        End With

    End Sub

    ''' <summary> Event handler. Called by ApplySenseSettingsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySenseSettingsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplySenseSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.applySenseSettings()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region


#Region " READ AND WRITE "

    ''' <summary> Executes the simple read write control property changed action. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSimpleReadWriteControlPropertyChanged(sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(sender.ReceivedMessage)
                Case NameOf(sender.SentMessage)
                Case NameOf(sender.StatusMessage)
                    Me.StatusLabel.Text = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me.StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
                Case NameOf(sender.ElapsedTime)
            End Select
        End If
    End Sub

    ''' <summary> Simple read write control property changed. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            Me.OnSimpleReadWriteControlPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
        Catch ex As Exception
            Me.StatusLabel.Text = "Exception occurred handling change"
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Simple Read and Write property changed Event;. Failed property {0}. Details: {1}",
                               e?.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds listeners such as current level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Protected Overrides Sub AddListeners()
        MyBase.AddListeners()
        Me._SimpleReadWriteControl.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Adds listeners such as top level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Adds the log listener. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

End Class