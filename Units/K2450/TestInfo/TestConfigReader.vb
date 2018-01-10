﻿Imports System.Configuration

Partial Public NotInheritable Class TestInfo

#Region " CONFIGURATION INFORMTION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property Exists As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the verbose. </summary>
    ''' <value> The verbose. </value>
    Public Shared ReadOnly Property Verbose As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets all. </summary>
    ''' <value> all. </value>
    Public Shared ReadOnly Property All As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

#End Region

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property ResourceModel As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared Function StripVisa(ByVal resourceName As String) As String
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Dim resourceElements As String() = resourceName.Split(":"c)
        Return resourceElements(2)
    End Function

    Private Shared _resourceName As String
    Public Shared ReadOnly Property ResourceName As String
        Get
            If String.IsNullOrWhiteSpace(_resourceName) Then
                If My.Computer.Network.Ping(StripVisa(IsrResourceName)) Then
                    _resourceName = IsrResourceName
                Else
                    _resourceName = MicronResourceName
                End If
            End If
            Return _resourceName
        End Get
    End Property

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Shared ReadOnly Property IsrResourceName As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property MicronResourceName As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the Title of the resource. </summary>
    ''' <value> The Title of the resource. </value>
    Public Shared ReadOnly Property ResourceTitle As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the keep alive query command. </summary>
    ''' <value> The keep alive query command. </value>
    Public Shared ReadOnly Property KeepAliveQueryCommand As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the keep alive command. </summary>
    ''' <value> The keep alive command. </value>
    Public Shared ReadOnly Property KeepAliveCommand As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the read termination enabled. </summary>
    ''' <value> The read termination enabled. </value>
    Public Shared ReadOnly Property ReadTerminationEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the termination character. </summary>
    ''' <value> The termination character. </value>
    Public Shared ReadOnly Property TerminationCharacter As Integer
        Get
            Return My.MyAppSettingsReader.AppSettingInt32()
        End Get
    End Property

#End Region

#Region " SOURCE MEASURE UNIT INFORMATION "

    ''' <summary> Gets the maximum output power of the instrument. </summary>
    ''' <value> The maximum output power . </value>
    Public Shared ReadOnly Property MaximumOutputPower As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    ''' <summary> Gets the line frequency. </summary>
    ''' <value> The line frequency. </value>
    Public Shared ReadOnly Property LineFrequency As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

#Region " MEASURE SUBSYSTEM INFORMATION "

    ''' <summary> Gets the Initial auto Delay Enabled settings. </summary>
    ''' <value> The auto Delay settings. </value>
    Public Shared ReadOnly Property InitialAutoDelayEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Initial auto Range enabled settings. </summary>
    ''' <value> The auto Range settings. </value>
    Public Shared ReadOnly Property InitialAutoRangeEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Initial auto zero Enabled settings. </summary>
    ''' <value> The auto zero settings. </value>
    Public Shared ReadOnly Property InitialAutoZeroEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property InitialFrontTerminalsSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property InitialRemoteSenseSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Initial Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property InitialSenseFunction As MeasureFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), MeasureFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property InitialSourceFunction As SourceFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), SourceFunctionMode)
        End Get
    End Property


    ''' <summary> Gets the Initial power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property InitialPowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property InitialSourceLevel As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property
    Public Shared ReadOnly Property InitialSourceLimit As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    ''' <summary> Gets the auto zero Enabled settings. </summary>
    ''' <value> The auto zero settings. </value>
    Public Shared ReadOnly Property AutoZeroEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property AutoRangeEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property FrontTerminalsSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property


    ''' <summary> Gets the Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property SenseFunction As MeasureFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), MeasureFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property SourceFunction As SourceFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), SourceFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property SourceLevel As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    ''' <summary> Gets the power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property PowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property RemoteSenseSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedResistance As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedResistanceEpsilon As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

End Class
