﻿Namespace K3700.Tests

    ''' <summary> The 3700 Subsystems Tests Info. </summary>
    ''' <license>
    ''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="2/12/2018" by="David" revision=""> Created. </history>
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0"),
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>
    Friend Class K3700SubsystemsInfo
        Inherits ApplicationSettingsBase

#Region " SINGLETON "

        Private Sub New()
            MyBase.New
        End Sub

        ''' <summary> Opens the settings editor. </summary>
        Public Shared Sub OpenSettingsEditor()
            Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
                f.Text = $"{GetType(K3700SubsystemsInfo)} Editor"
                f.ShowDialog(K3700SubsystemsInfo.Get)
            End Using
        End Sub

        ''' <summary> Gets the locking object to enforce thread safety when creating the singleton
        ''' instance. </summary>
        ''' <value> The sync locker. </value>
        Private Shared Property _SyncLocker As New Object

        ''' <summary> Gets the instance. </summary>
        ''' <value> The instance. </value>
        Private Shared Property _Instance As K3700SubsystemsInfo

        ''' <summary> Instantiates the class. </summary>
        ''' <remarks> Use this property to instantiate a single instance of this class. This class uses
        ''' lazy instantiation, meaning the instance isn't created until the first time it's retrieved. </remarks>
        ''' <returns> A new or existing instance of the class. </returns>
        Public Shared Function [Get]() As K3700SubsystemsInfo
            If _Instance Is Nothing Then
                SyncLock _SyncLocker
                    _Instance = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New K3700SubsystemsInfo()), K3700SubsystemsInfo)
                End SyncLock
            End If
            Return _Instance
        End Function

        ''' <summary> Returns true if an instance of the class was created and not disposed. </summary>
        ''' <value> <c>True</c> if instantiated; otherwise, <c>False</c>. </value>
        Public Shared ReadOnly Property Instantiated() As Boolean
            Get
                SyncLock _SyncLocker
                    Return _Instance IsNot Nothing
                End SyncLock
            End Get
        End Property

#End Region

#Region " CONFIGURATION INFORMATION "

        ''' <summary> Gets the Model of the resource. </summary>
        ''' <value> The Model of the resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")>
        Public ReadOnly Property Exists As Boolean
            Get
                Return Me.AppSettingBoolean()
            End Get
        End Property

        ''' <summary> Gets the verbose. </summary>
        ''' <value> The verbose. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("False")>
        Public ReadOnly Property Verbose As Boolean
            Get
                Return Me.AppSettingBoolean()
            End Get
        End Property

#End Region

#Region " SESSION INFORMATION "

        ''' <summary> Gets the keep alive query command. </summary>
        ''' <value> The keep alive query command. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property KeepAliveQueryCommand As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the keep alive command. </summary>
        ''' <value> The keep alive command. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property KeepAliveCommand As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the read termination enabled. </summary>
        ''' <value> The read termination enabled. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ReadTerminationEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the termination character. </summary>
        ''' <value> The termination character. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property TerminationCharacter As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

#End Region

#Region " STATUS SUBSYSTEM INFORMATION "

        ''' <summary> Gets the Initial power line cycles settings. </summary>
        ''' <value> The power line cycles settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialPowerLineCycles As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

#End Region

#Region " DEVICE ERRORS "

        ''' <summary> Gets the erroneous command. </summary>
        ''' <value> The erroneous command. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ErroneousCommand As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the error available milliseconds delay. </summary>
        ''' <value> The error available milliseconds delay. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ErrorAvailableMillisecondsDelay As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

        ''' <summary> Gets a message describing the expected error. </summary>
        ''' <value> A message describing the expected error. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedErrorMessage As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

#End Region

#Region " SOURCE MEASURE UNIT INFORMATION "

        ''' <summary> Gets the maximum output power of the instrument. </summary>
        ''' <value> The maximum output power . </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property MaximumOutputPower As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        ''' <summary> Gets the line frequency. </summary>
        ''' <value> The line frequency. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property LineFrequency As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

#End Region

#Region " MEASURE SUBSYSTEM INFORMATION "

        ''' <summary> Gets the Initial auto Delay Enabled settings. </summary>
        ''' <value> The auto Delay settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialAutoDelayEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the Initial auto Range enabled settings. </summary>
        ''' <value> The auto Range settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialAutoRangeEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the Initial auto zero Enabled settings. </summary>
        ''' <value> The auto zero settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialAutoZeroEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialSenseFunction As VI.Tsp.MultimeterFunctionMode
            Get
                Return Me.AppSettingEnum(Of VI.Tsp.MultimeterFunctionMode)
            End Get
        End Property

#End Region

    End Class

End Namespace