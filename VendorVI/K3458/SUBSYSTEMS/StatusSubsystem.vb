''' <summary> Defines a Status Subsystem for a Keysight K3458 meter. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public Class StatusSubsystem
    Inherits VI.R2D2.StatusSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.Pith.SessionBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal visaSession As VI.Pith.SessionBase)
        MyBase.New(visaSession)
        Me.ServiceRequestEnableBitmask = CType(63, VI.Pith.ServiceRequests)
        Me.OperationCompleted = True
        Me._VersionInfo = New VersionInfo()
    End Sub

    ''' <summary> Creates a new StatusSubsystem. </summary>
    ''' <returns> A StatusSubsystem. </returns>
    Public Shared Function Create() As StatusSubsystem
        Dim subsystem As StatusSubsystem = Nothing
        Try
            subsystem = New StatusSubsystem(isr.VI.SessionFactory.Get.Factory.CreateSession())
        Catch
            If subsystem IsNot Nothing Then
                subsystem.Dispose()
                subsystem = Nothing
            End If
            Throw
        End Try
        Return subsystem
    End Function

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem values to their known execution reset state. Reset is accomplished
    ''' using Clear Active State. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        MyBase.ClearActiveState()
        Me.OperationCompleted = True
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

#Region " ATTRIBUTES "

    ''' <summary> Gets the termination character enabled. </summary>
    ''' <value> The termination character enabled. </value>
    Protected Overrides ReadOnly Property TerminationCharacterEnabled As Boolean = True

    ''' <summary> Gets the termination character. </summary>
    ''' <value> The termination character. </value>
    Protected Overrides ReadOnly Property TerminationCharacter As Byte = isr.Core.Pith.EscapeSequencesExtensions.NewLineValue

#End Region

#Region " EXECUTION STATE "

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <value> The clear execution state command. </value>
    Protected Overrides ReadOnly Property ClearExecutionStateCommand As String = ""

    ''' <summary> Gets the reset known state command. </summary>
    ''' <value> The reset known state command. </value>
    ''' <remarks> reset is done using clear active state. </remarks>
    Protected Overrides ReadOnly Property ResetKnownStateCommand As String = ""

#End Region

#Region " MISC "

    ''' <summary> Gets line frequency query command. </summary>
    ''' <value> The line frequency query command. </value>
    Protected Overrides ReadOnly Property LineFrequencyQueryCommand As String = "LINE?"

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overrides ReadOnly Property ErrorAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.StandardEvent

    ''' <summary> Gets the identity query command. </summary>
    ''' <value> The identity query command. </value>
    Protected Overrides ReadOnly Property IdentityQueryCommand As String = "ID?"

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overrides ReadOnly Property MeasurementAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MeasurementEvent

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overrides ReadOnly Property MessageAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.None

    ''' <summary> Gets the operation completed query command. </summary>
    ''' <value> The operation completed query command. </value>
    ''' <remarks>  not supported. Set to completed.</remarks>
    Protected Overrides ReadOnly Property OperationCompletedQueryCommand As String = ""

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overrides ReadOnly Property StandardEventAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.None

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <value> The standard event status query command. </value>
    Protected Overrides ReadOnly Property StandardEventStatusQueryCommand As String = ""

    ''' <summary> Gets the standard event enable query command. </summary>
    ''' <value> The standard event enable query command. </value> 
    Protected Overrides ReadOnly Property StandardEventEnableQueryCommand As String = ""

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCommandFormat As String = ""

    ''' <summary> Gets the standard service enable and complete command format. </summary>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCompleteCommandFormat As String = ""

    ''' <summary> Gets the service request enable command format. </summary>
    ''' <value> The service request enable command format. </value>
    Protected Overrides ReadOnly Property ServiceRequestEnableCommandFormat As String = "RQS {0}"

#End Region

#End Region

#Region " IDENTITY "

    ''' <summary> Parses version information. </summary>
    ''' <param name="value"> The value. </param>
    Protected Overrides Sub ParseVersionInfo(value As String)
        ' K3458 uses a special version info string.
        Me._VersionInfo = New VersionInfo
        Me.VersionInfo.Parse(value)

        ' this is designed to prevent failures due to non-standard Identity strings.
        MyBase.ParseVersionInfo(Me.VersionInfo.BuildIdentity)
    End Sub

    ''' <summary> Gets the information describing the version. </summary>
    ''' <value> Information describing the version. </value>
    Public Overloads ReadOnly Property VersionInfo As VersionInfo

#End Region

#Region " DEVICE ERRORS "

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = ""

    ''' <summary> Gets the error queue query command. </summary>
    ''' <value> The error queue query command. </value>
    ''' <remarks> Last error supported </remarks>
    Protected Overrides ReadOnly Property NextDeviceErrorQueryCommand As String = "ERRSTR?"

    ''' <summary> Gets the last error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overrides ReadOnly Property DequeueErrorQueryCommand As String = ""

    ''' <summary> Gets the last error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overrides ReadOnly Property DeviceErrorQueryCommand As String = "ERRSTR?"

    ''' <summary> Queue device error. </summary>
    ''' <param name="compoundErrorMessage"> Message describing the compound error. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Protected Overrides Function EnqueueDeviceError(ByVal compoundErrorMessage As String) As VI.DeviceError
        Dim de As DeviceError = New DeviceError
        de.Parse(compoundErrorMessage)
        If de.IsError Then Me.DeviceErrorQueue.Enqueue(de)
        Return de
    End Function

#End Region

End Class
