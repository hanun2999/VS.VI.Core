Imports isr.Core.Pith
''' <summary> Defines the SCPI Digital Output subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/05/2013" by="David" revision="">            Created based on SCPI 5.1 library. </history>
''' <history date="03/25/2008" by="David" revision="5.0.3004.x">  Port to new SCPI library. </history>
Public MustInherit Class DigitalOutputBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.BitSize = 4
        Me.Level = 15
        Me.AutoClearEnabled = False
        Me._DelayRange = New Range(Of TimeSpan)(TimeSpan.Zero, TimeSpan.FromSeconds(60))
        Me.SafePostPropertyChanged(NameOf(DigitalOutputBase.DelayRange))
        Me.Delay = TimeSpan.FromSeconds(0.0001)
    End Sub

#End Region

#Region " COMMANDS "

    ''' <summary> Gets or sets the Digital Outputs clear command. </summary>
    ''' <value> The Digital Outputs clear command. </value>
    ''' <remarks> SCPI: ":SOUR2:CLE" </remarks>
    Protected Overridable ReadOnly Property ClearCommand As String

    ''' <summary>
    ''' Clears Digital Outputs.
    ''' </summary>
    Public Sub ClearOutput()
        Me.Session.Execute(Me.ClearCommand)
    End Sub

#End Region

#Region " AUTO CLEAR ENABLED "

    Private _AutoClearEnabled As Boolean?
    ''' <summary> Gets or sets the cached Digital Outputs Auto Clear enabled sentinel. </summary>
    ''' <value> <c>null</c> if Digital Outputs Auto Clear enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoClearEnabled As Boolean?
        Get
            Return Me._AutoClearEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoClearEnabled, value) Then
                Me._AutoClearEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Digital Outputs Auto Clear enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if Enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoClearEnabled(value)
        Return Me.QueryAutoClearEnabled()
    End Function

    ''' <summary> Gets the Digital Outputs Auto Clear enabled query command. </summary>
    ''' <value> The Digital Outputs Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR2:CLE:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoClearEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoClearEnabled() As Boolean?
        Me.AutoClearEnabled = Me.Query(Me.AutoClearEnabled, Me.AutoClearEnabledQueryCommand)
        Return Me.AutoClearEnabled
    End Function

    ''' <summary> Gets the Digital Outputs Auto Clear enabled command Format. </summary>
    ''' <value> The Digital Outputs Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR2:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoClearEnabled = Me.Write(value, Me.AutoClearEnabledCommandFormat)
        Return Me.AutoClearEnabled
    End Function

#End Region

#Region " BIT SIZE "

    Private _BitSize As Integer?
    ''' <summary> Gets or sets the cached Digital Outputs Bit Size. </summary>
    ''' <value> The Digital Outputs Bit Size or none if not set or unknown. </value>
    Public Overloads Property BitSize As Integer?
        Get
            Return Me._BitSize
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.BitSize, value) Then
                Me._BitSize = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Digital Outputs Bit Size. </summary>
    ''' <param name="value"> The current Digital Outputs Bit Size. </param>
    ''' <returns> The Digital Outputs Bit Size or none if unknown. </returns>
    Public Function ApplyBitSize(ByVal value As Integer) As Integer?
        Me.WriteBitSize(value)
        Return Me.QueryBitSize()
    End Function

    ''' <summary> Gets the Lower Limit Bit Size query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR2:BSIZ?" </remarks>
    Protected Overridable ReadOnly Property BitSizeQueryCommand As String

    ''' <summary> Queries the current Lower Limit Bit Size. </summary>
    ''' <returns> The Lower Limit Bit Size or none if unknown. </returns>
    Public Function QueryBitSize() As Integer?
        Me.BitSize = Me.Query(Me.BitSize, Me.BitSizeQueryCommand)
        Return Me.BitSize
    End Function

    ''' <summary> Gets the Lower Limit Bit Size query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "::SOUR2:BSIZ {0}" </remarks>
    Protected Overridable ReadOnly Property BitSizeCommandFormat As String

    ''' <summary> Sets back the Lower Limit Bit Size without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit Bit Size. </param>
    ''' <returns> The Lower Limit Bit Size or none if unknown. </returns>
    Public Function WriteBitSize(ByVal value As Integer) As Integer?
        Me.BitSize = Me.Write(value, Me.BitSizeCommandFormat)
        Return Me.BitSize
    End Function

#End Region

#Region " DELAY (PULSE WIDTH) "

    Private _DelayRange As Range(Of TimeSpan)
    ''' <summary> Gets or sets the delay (pulse width) range. </summary>
    ''' <value> The delay range. </value>
    Public Overridable Property DelayRange As Range(Of TimeSpan)
        Get
            Return Me._DelayRange
        End Get
        Set(value As Range(Of TimeSpan))
            If Me.DelayRange <> value Then
                Me._DelayRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The delay. </summary>
    Private _Delay As TimeSpan?

    ''' <summary> Gets or sets the cached output delay (pulse width). </summary>
    ''' <remarks> The delay is used to delay operation in the Source layer. After the programmed
    ''' Source event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The output delay (pulse width) or none if not set or unknown. </value>
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

    ''' <summary> Writes and reads back the output delay (pulse width). </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The output delay (pulse width) or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Me.QueryDelay()
    End Function

    ''' <summary> Gets or sets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets or sets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":SOUR:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the output delay (pulse width) without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The output delay (pulse width) or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " LEVEL "

    Private _Level As Integer?
    ''' <summary> Gets or sets the cached Digital Outputs Level. </summary>
    ''' <value> The Digital Outputs Level or none if not set or unknown. </value>
    Public Overloads Property Level As Integer?
        Get
            Return Me._Level
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.Level, value) Then
                Me._Level = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Digital Outputs Level. </summary>
    ''' <param name="value"> The current Digital Outputs Level. </param>
    ''' <returns> The Digital Outputs Level or none if unknown. </returns>
    Public Function ApplyLevel(ByVal value As Integer) As Integer?
        Me.WriteLevel(value)
        Return Me.QueryLevel()
    End Function

    ''' <summary> Gets the Lower Limit Level query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR2:TTL:ACT?" </remarks>
    Protected Overridable ReadOnly Property LevelQueryCommand As String

    ''' <summary> Queries the current Lower Limit Level. </summary>
    ''' <returns> The Lower Limit Level or none if unknown. </returns>
    Public Function QueryLevel() As Integer?
        Me.Level = Me.Query(Me.Level, Me.LevelQueryCommand)
        Return Me.Level
    End Function

    ''' <summary> Gets the Lower Limit Level query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "::SOUR2:TTL:LEVEL {0}" </remarks>
    Protected Overridable ReadOnly Property LevelCommandFormat As String

    ''' <summary> Sets back the Lower Limit Level without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit Level. </param>
    ''' <returns> The Lower Limit Level or none if unknown. </returns>
    Public Function WriteLevel(ByVal value As Integer) As Integer?
        Me.Level = Me.Write(value, Me.LevelCommandFormat)
        Return Me.Level
    End Function

#End Region

End Class