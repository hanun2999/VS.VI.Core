''' <summary> Defines a SCPI Sense Current Subsystem for a generic Source Measure instrument . </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SenseCurrentSubsystem
    Inherits VI.Scpi.SenseFunctionSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SenseCurrentSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.01, 10)
        Me.FunctionRange = New isr.Core.Pith.RangeR(0.001, 1.05)
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.01, 10)
        Dim model As String = Me.StatusSubsystem.VersionInfo.Model
        Select Case True
            Case model.StartsWith("2400", StringComparison.OrdinalIgnoreCase)
                Me.FunctionModeRanges(SenseFunctionModes.CurrentDC).SetRange(0.001, 1.05)
                Me.FunctionModeRanges(SenseFunctionModes.VoltageDC).SetRange(0.001, 40)
            Case model.StartsWith("2410", StringComparison.OrdinalIgnoreCase)
                Me.FunctionModeRanges(SenseFunctionModes.CurrentDC).SetRange(0.001, 1.05)
                Me.FunctionModeRanges(SenseFunctionModes.VoltageDC).SetRange(0.001, 500)
            Case model.StartsWith("242", StringComparison.OrdinalIgnoreCase)
                Me.FunctionModeRanges(SenseFunctionModes.CurrentDC).SetRange(0.001, 3.05)
            Case model.StartsWith("243", StringComparison.OrdinalIgnoreCase)
                Me.FunctionModeRanges(SenseFunctionModes.CurrentDC).SetRange(0.001, 3.15)
                Me.FunctionModeRanges(SenseFunctionModes.VoltageDC).SetRange(0.001, 40)
            Case model.StartsWith("244", StringComparison.OrdinalIgnoreCase)
                Me.FunctionModeRanges(SenseFunctionModes.CurrentDC).SetRange(0.001, 5.25)
                Me.FunctionModeRanges(SenseFunctionModes.VoltageDC).SetRange(0.001, 40)
            Case Else
                Me.FunctionModeRanges(SenseFunctionModes.CurrentDC).SetRange(0.001, 1.05)
                Me.FunctionModeRanges(SenseFunctionModes.VoltageDC).SetRange(0.001, 40)
        End Select
        Me.SafePostPropertyChanged(NameOf(VI.Scpi.SenseFunctionSubsystemBase.FunctionModeRanges))
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SENS:CURR:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SENS:CURR:RANG:AUTO?"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = ":SENS:CURR:NPLC {0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = ":SENS:CURR:NPLC?"

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> Gets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overrides ReadOnly Property ProtectionLevelCommandFormat As String = ":SENS:CURR:PROT {0}"

    ''' <summary> Gets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected Overrides ReadOnly Property ProtectionLevelQueryCommand As String = ":SENS:CURR:PROT?"

#End Region

#Region " RANGE "

    ''' <summary> Gets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SENS:CURR:RANG {0}"

    ''' <summary> Gets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SENS:CURR:RANG?"

#End Region

#End Region

End Class
