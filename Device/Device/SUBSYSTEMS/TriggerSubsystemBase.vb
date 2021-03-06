Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a Trigger Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class TriggerSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoDelayEnabled = False
        Me.TriggerCount = 1
        Me.Delay = TimeSpan.Zero
        Me.InputLineNumber = 1
        Me.OutputLineNumber = 2
        Me.TimerInterval = TimeSpan.FromSeconds(0.1)
        Me.ContinuousEnabled = False
        Me.TriggerState = VI.TriggerState.None
    End Sub

#End Region

#Region " COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    ''' <remarks> SCPI: ":ABOR". </remarks>
    Protected Overridable ReadOnly Property AbortCommand As String

    ''' <summary> Aborts operations. </summary>
    ''' <remarks> When this action command is sent, the SourceMeter aborts operation and returns to the
    ''' idle state. A faster way to return to idle is to use the DCL or SDC command. With auto output-
    ''' off enabled (:SOURce1:CLEar:AUTO ON), the output will remain on if operation is terminated
    ''' before the output has a chance to automatically turn off. </remarks>
    Public Sub Abort()
        If Not String.IsNullOrWhiteSpace(Me.AbortCommand) Then
            Me.Session.WriteLine(Me.AbortCommand)
        End If
    End Sub

    ''' <summary> Gets the clear command. </summary>
    ''' <remarks> SCPI: ":TRIG:CLE". </remarks>
    ''' <value> The clear command. </value>
    Protected Overridable ReadOnly Property ClearCommand As String

    ''' <summary> Clears the triggers. </summary>
    Public Sub ClearTriggers()
        Me.Write(Me.ClearCommand)
    End Sub

    ''' <summary> Gets the clear  trigger model command. </summary>
    ''' <remarks> SCPI: ":TRIG:LOAD 'EMPTY'". </remarks>
    ''' <value> The clear command. </value>
    Protected Overridable ReadOnly Property ClearTriggerModelCommand As String

    ''' <summary> Clears the trigger model. </summary>
    Public Sub ClearTriggerModel()
        Me.Write(Me.ClearTriggerModelCommand)
    End Sub


    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    ''' <remarks> SCPI: ":INIT". </remarks>
    Protected Overridable ReadOnly Property InitiateCommand As String

    ''' <summary> Initiates operations. </summary>
    ''' <remarks> This command is used to initiate source-measure operation by taking the SourceMeter
    ''' out of idle. The :READ? and :MEASure? commands also perform an initiation. Note that if auto
    ''' output-off is disabled (SOURce1:CLEar:AUTO OFF), the source output must first be turned on
    ''' before an initiation can be performed. The :MEASure? command automatically turns the output
    ''' source on before performing the initiation. </remarks>
    Public Sub Initiate()
        Me.Write(Me.InitiateCommand)
    End Sub

    ''' <summary> Gets the Immediate command. </summary>
    ''' <value> The Immediate command. </value>
    ''' <remarks> SCPI: ":TRIG:IMM". </remarks>
    Protected Overridable ReadOnly Property ImmediateCommand As String

    ''' <summary> Immediately move tot he next layer. </summary>
    Public Sub Immediate()
        Me.Session.Execute(Me.ImmediateCommand)
    End Sub

#End Region

#Region " AUTO DELAY ENABLED "

    Private _AutoDelayEnabled As Boolean?
    ''' <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Delay Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoDelayEnabled As Boolean?
        Get
            Return Me._AutoDelayEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoDelayEnabled, value) Then
                Me._AutoDelayEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Delay Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoDelayEnabled(value)
        Return Me.QueryAutoDelayEnabled()
    End Function

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":TRIG:DEL:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoDelayEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Query(Me.AutoDelayEnabled, Me.AutoDelayEnabledQueryCommand)
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":TRIG:DEL:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoDelayEnabled = Me.Write(value, Me.AutoDelayEnabledCommandFormat)
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " AVERAGING ENABLED "

    Private _AveragingEnabled As Boolean?
    ''' <summary> Gets or sets the cached Averaging Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Averaging Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AveragingEnabled As Boolean?
        Get
            Return Me._AveragingEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AveragingEnabled, value) Then
                Me._AveragingEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Averaging Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAveragingEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAveragingEnabled(value)
        Return Me.QueryAveragingEnabled()
    End Function

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":TRIG:AVER?" </remarks>
    Protected Overridable ReadOnly Property AveragingEnabledQueryCommand As String

    ''' <summary> Queries the Averaging Enabled sentinel. Also sets the
    ''' <see cref="AveragingEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAveragingEnabled() As Boolean?
        Me.AveragingEnabled = Me.Query(Me.AveragingEnabled, Me.AveragingEnabledQueryCommand)
        Return Me.AveragingEnabled
    End Function

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":TRIG:AVER {0:1;1;0}" </remarks>
    Protected Overridable ReadOnly Property AveragingEnabledCommandFormat As String

    ''' <summary> Writes the Averaging Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAveragingEnabled(ByVal value As Boolean) As Boolean?
        Me.AveragingEnabled = Me.Write(value, Me.AveragingEnabledCommandFormat)
        Return Me.AveragingEnabled
    End Function

#End Region

#Region " CONTINUOUS ENABLED "

    Private _ContinuousEnabled As Boolean?
    ''' <summary> Gets or sets the cached Continuous Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Continuous Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ContinuousEnabled As Boolean?
        Get
            Return Me._ContinuousEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ContinuousEnabled, value) Then
                Me._ContinuousEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Continuous Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyContinuousEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteContinuousEnabled(value)
        Return Me.QueryContinuousEnabled()
    End Function

    ''' <summary> Gets the Continuous enabled query command. </summary>
    ''' <value> The Continuous enabled query command. </value>
    ''' <remarks> SCPI: ":INIT:CONT?" </remarks>
    Protected Overridable ReadOnly Property ContinuousEnabledQueryCommand As String

    ''' <summary> Queries the Continuous Enabled sentinel. Also sets the
    ''' <see cref="ContinuousEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryContinuousEnabled() As Boolean?
        Me.ContinuousEnabled = Me.Query(Me.ContinuousEnabled, Me.ContinuousEnabledQueryCommand)
        Return Me.ContinuousEnabled
    End Function

    ''' <summary> Gets the Continuous enabled command Format. </summary>
    ''' <value> The Continuous enabled query command. </value>
    ''' <remarks> SCPI: ":INIT:CONT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property ContinuousEnabledCommandFormat As String

    ''' <summary> Writes the Continuous Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteContinuousEnabled(ByVal value As Boolean) As Boolean?
        Me.ContinuousEnabled = Me.Write(value, Me.ContinuousEnabledCommandFormat)
        Return Me.ContinuousEnabled
    End Function

#End Region

#Region " TRIGGER COUNT "

    Private _TriggerCount As Integer?
    ''' <summary> Gets or sets the cached Trigger TriggerCount. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Trigger TriggerCount or none if not set or unknown. </value>
    Public Overloads Property TriggerCount As Integer?
        Get
            Return Me._TriggerCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.TriggerCount, value) Then
                Me._TriggerCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger TriggerCount. </summary>
    ''' <param name="value"> The current TriggerCount. </param>
    ''' <returns> The TriggerCount or none if unknown. </returns>
    Public Function ApplyTriggerCount(ByVal value As Integer) As Integer?
        Me.WriteTriggerCount(value)
        Return Me.QueryTriggerCount()
    End Function

    ''' <summary> Gets trigger TriggerCount query command. </summary>
    ''' <value> The trigger TriggerCount query command. </value>
    ''' <remarks> SCPI: ":TRIG:COUN?" </remarks>
    Protected Overridable ReadOnly Property TriggerCountQueryCommand As String

    ''' <summary> Queries the current Trigger Count. </summary>
    ''' <returns> The Trigger Count or none if unknown. </returns>
    Public Function QueryTriggerCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.TriggerCountQueryCommand) Then
            Me.TriggerCount = Me.Session.Query(0I, Me.TriggerCountQueryCommand)
        End If
        Return Me.TriggerCount
    End Function

    ''' <summary> Gets trigger TriggerCount command format. </summary>
    ''' <value> The trigger TriggerCount command format. </value>
    ''' <remarks> SCPI: ":TRIG:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property TriggerCountCommandFormat As String

    ''' <summary> Write the Trace PointsTriggerCount without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsTriggerCount. </param>
    ''' <returns> The PointsTriggerCount or none if unknown. </returns>
    Public Function WriteTriggerCount(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.TriggerCountCommandFormat) Then
            Me.Session.WriteLine(Me.TriggerCountCommandFormat, value)
        End If
        Me.TriggerCount = value
        Return Me.TriggerCount
    End Function

#End Region

#Region " DELAY "

    Private _Delay As TimeSpan?
    ''' <summary> Gets or sets the cached Trigger Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me._Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Return Me.QueryDelay()
    End Function

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":TRIG:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":TRIG:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Trigger Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " INPUT LINE NUMBER "

    Private _InputLineNumber As Integer?
    ''' <summary> Gets or sets the cached Trigger Input Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Trigger InputLineNumber or none if not set or unknown. </value>
    Public Overloads Property InputLineNumber As Integer?
        Get
            Return Me._InputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.InputLineNumber, value) Then
                Me._InputLineNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Input Line Number. </summary>
    ''' <param name="value"> The current Input Line Number. </param>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function ApplyInputLineNumber(ByVal value As Integer) As Integer?
        Me.WriteInputLineNumber(value)
        Return Me.QueryInputLineNumber()
    End Function

    ''' <summary> Gets the Input Line Number query command. </summary>
    ''' <value> The Input Line Number query command. </value>
    ''' <remarks> SCPI: ":TRIG:ILIN?" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberQueryCommand As String

    ''' <summary> Queries the InputLineNumber. </summary>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function QueryInputLineNumber() As Integer?
        Me.InputLineNumber = Me.Query(Me.InputLineNumber, Me.InputLineNumberQueryCommand)
        Return Me.InputLineNumber
    End Function

    ''' <summary> Gets the Input Line Number command format. </summary>
    ''' <value> The Input Line Number command format. </value>
    ''' <remarks> SCPI: ":TRIG:ILIN {0}" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberCommandFormat As String

    ''' <summary> Writes the Trigger Input Line Number without reading back the value from the device. </summary>
    ''' <param name="value"> The current InputLineNumber. </param>
    ''' <returns> The Trigger Input Line Number or none if unknown. </returns>
    Public Function WriteInputLineNumber(ByVal value As Integer) As Integer?
        Me.InputLineNumber = Me.Write(value, Me.InputLineNumberCommandFormat)
        Return Me.InputLineNumber
    End Function

#End Region

#Region " OUTPUT LINE NUMBER "

    Private _OutputLineNumber As Integer?
    ''' <summary> Gets or sets the cached Trigger Output Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Trigger OutputLineNumber or none if not set or unknown. </value>
    Public Overloads Property OutputLineNumber As Integer?
        Get
            Return Me._OutputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OutputLineNumber, value) Then
                Me._OutputLineNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Output Line Number. </summary>
    ''' <param name="value"> The current Output Line Number. </param>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function ApplyOutputLineNumber(ByVal value As Integer) As Integer?
        Me.WriteOutputLineNumber(value)
        Return Me.QueryOutputLineNumber()
    End Function

    ''' <summary> Gets the Output Line Number query command. </summary>
    ''' <value> The Output Line Number query command. </value>
    ''' <remarks> SCPI: ":TRIG:OLIN?" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberQueryCommand As String

    ''' <summary> Queries the OutputLineNumber. </summary>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function QueryOutputLineNumber() As Integer?
        Me.OutputLineNumber = Me.Query(Me.OutputLineNumber, Me.OutputLineNumberQueryCommand)
        Return Me.OutputLineNumber
    End Function

    ''' <summary> Gets the Output Line Number command format. </summary>
    ''' <value> The Output Line Number command format. </value>
    ''' <remarks> SCPI: ":TRIG:OLIN {0}" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberCommandFormat As String

    ''' <summary> Writes the Trigger Output Line Number without reading back the value from the device. </summary>
    ''' <param name="value"> The current OutputLineNumber. </param>
    ''' <returns> The Trigger Output Line Number or none if unknown. </returns>
    Public Function WriteOutputLineNumber(ByVal value As Integer) As Integer?
        Me.OutputLineNumber = Me.Write(value, Me.OutputLineNumberCommandFormat)
        Return Me.OutputLineNumber
    End Function

#End Region

#Region " TIMER TIME SPAN "

    Private _TimerInterval As TimeSpan?
    ''' <summary> Gets or sets the cached Trigger Timer Interval. </summary>
    ''' <remarks> The Timer Interval is used to Timer Interval operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the Timer Interval period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Timer Interval or none if not set or unknown. </value>
    Public Overloads Property TimerInterval As TimeSpan?
        Get
            Return Me._TimerInterval
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Me.TimerInterval.Equals(value) Then
                Me._TimerInterval = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Timer Interval. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Trigger Timer Interval or none if unknown. </returns>
    Public Function ApplyTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteTimerTimeSpan(value)
        Return Me.QueryTimerTimeSpan()
    End Function

    ''' <summary> Gets the Timer Interval query command. </summary>
    ''' <value> The Timer Interval query command. </value>
    ''' <remarks> SCPI: ":TRIG:TIM?" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalQueryCommand As String

    ''' <summary> Gets the Timer Interval format for converting the query to time span. </summary>
    ''' <value> The Timer Interval query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property TimerIntervalFormat As String

    ''' <summary> Queries the Timer Interval. </summary>
    ''' <returns> The Timer Interval or none if unknown. </returns>
    Public Function QueryTimerTimeSpan() As TimeSpan?
        Me.TimerInterval = Me.Query(Me.TimerInterval, Me.TimerIntervalFormat, Me.TimerIntervalQueryCommand)
        Return Me.TimerInterval
    End Function

    ''' <summary> Gets the Timer Interval command format. </summary>
    ''' <value> The query command format. </value>
    ''' <remarks> SCPI: ":TRIG:TIM {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalCommandFormat As String

    ''' <summary> Writes the Trigger Timer Interval without reading back the value from the device. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Trigger Timer Interval or none if unknown. </returns>
    Public Function WriteTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.TimerInterval = Me.Write(value, Me.TimerIntervalQueryCommand)
    End Function

#End Region

#Region " TRIGGER STATE "

    ''' <summary> Monitor active trigger state. </summary>
    ''' <param name="pollPeriod"> The poll period. </param>
    Public Sub MonitorActiveTriggerState(ByVal pollPeriod As TimeSpan)
        Me.ApplyCapturedSyncContext()
        Me.QueryTriggerState()
        Do While Me.IsTriggerStateActive
            Threading.Thread.Sleep(pollPeriod)
            Windows.Forms.Application.DoEvents()
            Me.QueryTriggerState()
        Loop
    End Sub

    ''' <summary> Asynchronous monitor trigger state. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    ''' <param name="pollPeriod">  The poll period. </param>
    ''' <returns> A Threading.Tasks.Task. </returns>
    Public Async Function AsyncMonitorTriggerState(ByVal syncContext As Threading.SynchronizationContext, ByVal pollPeriod As TimeSpan) As Threading.Tasks.Task
        Me.CaptureSyncContext(syncContext)
        Await Threading.Tasks.Task.Run(Sub() Me.MonitorActiveTriggerState(pollPeriod))
    End Function

    Private _TriggerState As TriggerState?
    ''' <summary> Gets or sets the cached State TriggerState. </summary>
    ''' <value> The <see cref="TriggerState">State Trigger State</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property TriggerState As TriggerState?
        Get
            Return Me._TriggerState
        End Get
        Protected Set(ByVal value As TriggerState?)
            If Not Me.TriggerState.Equals(value) Then
                Me._TriggerState = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _TriggerBlockState As TriggerState?
    ''' <summary> Gets or sets the cached State TriggerState. </summary>
    ''' <value> The <see cref="TriggerBlockState">State Trigger State</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property TriggerBlockState As TriggerState?
        Get
            Return Me._TriggerBlockState
        End Get
        Protected Set(ByVal value As TriggerState?)
            If Not Me.TriggerBlockState.Equals(value) Then
                Me._TriggerBlockState = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _TriggerStateBlockNumber As Integer?
    ''' <summary> Gets or sets the cached trigger state block number. </summary>
    ''' <value> The block number of the trigger state. </value>
    Public Overloads Property TriggerStateBlockNumber As Integer?
        Get
            Return Me._TriggerStateBlockNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Me.TriggerStateBlockNumber.Equals(value) Then
                Me._TriggerStateBlockNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Query if this object is trigger state done. </summary>
    ''' <returns> <c>true</c> if trigger state done; otherwise <c>false</c> </returns>
    Public Function IsTriggerStateDone() As Boolean
        Dim result As Boolean = False
        If Me.TriggerState.HasValue Then
            Dim value As TriggerState = Me.TriggerState.Value
            result = (value = VI.TriggerState.Aborted) OrElse
                     (value = VI.TriggerState.Failed) OrElse
                     (value = VI.TriggerState.Empty) OrElse
                     (value = VI.TriggerState.Idle) OrElse
                     (value = VI.TriggerState.None)
        Else
            Return False
        End If
        Return result
    End Function

    ''' <summary> Queries if a trigger state is active. </summary>
    ''' <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    Public Function IsTriggerStateActive() As Boolean
        Dim result As Boolean = False
        If Me.TriggerState.HasValue Then
            Dim value As TriggerState = Me.TriggerState.Value
            result = (value = VI.TriggerState.Running) OrElse
                     (value = VI.TriggerState.Waiting)
        Else
            Return False
        End If
        Return result
    End Function

    ''' <summary> Queries if a trigger state is aborting. </summary>
    ''' <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    Public Function IsTriggerStateAborting() As Boolean
        Dim result As Boolean = False
        If Me.TriggerState.HasValue Then
            Dim value As TriggerState = Me.TriggerState.Value
            result = (value = VI.TriggerState.Aborting)
        Else
            Return False
        End If
        Return result
    End Function

    ''' <summary> Queries if a trigger state is Failed. </summary>
    ''' <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    Public Function IsTriggerStateFailed() As Boolean
        Dim result As Boolean = False
        If Me.TriggerState.HasValue Then
            Dim value As TriggerState = Me.TriggerState.Value
            result = (value = VI.TriggerState.Failed)
        Else
            Return False
        End If
        Return result
    End Function

    ''' <summary> Queries if a trigger state is Idle. </summary>
    ''' <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    Public Function IsTriggerStateIdle() As Boolean
        Dim result As Boolean = False
        If Me.TriggerState.HasValue Then
            Dim value As TriggerState = Me.TriggerState.Value
            result = (value = VI.TriggerState.Idle)
        Else
            Return False
        End If
        Return result
    End Function

    ''' <summary> Gets the Trigger State query command. </summary>
    ''' <value> The Trigger State query command. </value>
    ''' <remarks> SCPI: ":TRIG:SOUR?" </remarks>
    Protected Overridable ReadOnly Property TriggerStateQueryCommand As String

    ''' <summary> Queries the Trigger State. </summary>
    ''' <returns> The <see cref="TriggerState">Trigger State</see> or none if unknown. </returns>
    Public Function QueryTriggerState() As TriggerState?
        Dim currentValue As String = Me.TriggerState.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        If Not String.IsNullOrWhiteSpace(Me.TriggerStateQueryCommand) Then
            currentValue = Me.Session.QueryTrimEnd(Me.TriggerStateQueryCommand)
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.TriggerState = New TriggerState?
        Else
            Dim values As New Queue(Of String)(currentValue.Split(";"c))
            If values.Any Then
                Me.TriggerState = CType([Enum].Parse(GetType(VI.TriggerState), values.Dequeue, True), VI.TriggerState)
            End If
            If values.Any Then
                Me.TriggerBlockState = CType([Enum].Parse(GetType(VI.TriggerState), values.Dequeue, True), VI.TriggerState)
            End If
            If values.Any Then
                Me.TriggerStateBlockNumber = Integer.Parse(values.Dequeue)
            End If
        End If
        Return Me.TriggerState
    End Function

#End Region

End Class

''' <summary> Values that represent trigger status. </summary>
Public Enum TriggerState
    <ComponentModel.Description("Idle (trigger.STATE_NONE)")> None
    <ComponentModel.Description("Idle (trigger.STATE_IDLE)")> Idle
    <ComponentModel.Description("Running (trigger.STATE_RUNNING)")> Running
    <ComponentModel.Description("Waiting (trigger.STATE_WAITING)")> Waiting
    <ComponentModel.Description("Empty (trigger.STATE_EMPTY)")> Empty
    <ComponentModel.Description("Building (trigger.STATE_BUILDING)")> Building
    <ComponentModel.Description("Failed (trigger.STATE_FAILED)")> Failed
    <ComponentModel.Description("Aborting (trigger.STATE_ABORTING)")> Aborting
    <ComponentModel.Description("Aborted (trigger.STATE_ABORTED)")> Aborted
End Enum

