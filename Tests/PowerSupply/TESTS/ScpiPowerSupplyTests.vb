﻿'''<summary>
'''This is a test class for PowerSupplyTest and is intended
'''to contain all PowerSupplyTest Unit Tests
'''</summary>
<TestClass()>
Public Class ScpiPowerSupplyTests


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    ''' <summary> Select resource name. </summary>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> . </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Friend Function SelectResourceName(ByVal interfaceType As VI.Pith.HardwareInterfaceType) As String
        Select Case interfaceType
            Case VI.Pith.HardwareInterfaceType.Gpib
                Return "GPIB0::5::INSTR"
            Case VI.Pith.HardwareInterfaceType.Tcpip
                Return "TCPIP0::A-N5767A-K4381"
            Case VI.Pith.HardwareInterfaceType.Usb
                Return "USB0::0x0957::0x0807::N5767A-US11K4381H::0::INSTR"
            Case Else
                Return "GPIB0::5::INSTR"
        End Select
    End Function

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Dim usingInterfaceType As VI.Pith.HardwareInterfaceType = VI.Pith.HardwareInterfaceType.Usb
        Using target As PowerSupply.Device = New PowerSupply.Device()
            Dim e As New isr.Core.Pith.ActionEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(usingInterfaceType), "Power Supply", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            target.Session.Clear()
            target.CloseSession()
        End Using
    End Sub

    '''<summary>
    '''A test for ToggleOutput Off
    '''</summary>
    <TestMethod()>
    Public Sub ToggleOutputOffTest()
        Dim expectedBoolean As Boolean = True
        Dim expectedString As String = ""
        Dim actualBoolean As Boolean
        Dim actualString As String = ""
        Using target As PowerSupply.Device = New PowerSupply.Device()
            Dim e As New isr.Core.Pith.ActionEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(VI.Pith.HardwareInterfaceType.Gpib), "Power Supply", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            actualBoolean = target.OutputSubsystem.ApplyOutputOnState(False).GetValueOrDefault(True)
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output Off;")
            expectedBoolean = False
            actualBoolean = target.OutputSubsystem.QueryOutputOnState.GetValueOrDefault(True)
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output State")

            expectedString = "no error"
            target.StatusSubsystem.TrySafeQueryDeviceErrors(e)
            Assert.IsFalse(e.Cancel, "Device error query failed")
            actualString = target.StatusSubsystem.LastDeviceError.ErrorMessage
            Assert.AreEqual(expectedString, actualString, True, Globalization.CultureInfo.CurrentCulture, "Device error mismatch")
        End Using
    End Sub

    '''<summary>
    '''A test for OutputOn
    '''</summary>
    <TestMethod()>
    Public Sub TurnAndKeepOnTest()
        Dim voltage As Double = 28.0!
        Dim currentLimit As Double = 2.0!
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Dim expectedString As String = ""
        Dim actualString As String = ""
        Dim expectedDouble As Double = 0
        Dim actualDouble As Double = 0
        Using target As PowerSupply.Device = New PowerSupply.Device()
            Dim e As New isr.Core.Pith.ActionEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(VI.Pith.HardwareInterfaceType.Gpib), "Power Supply", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            target.ResetClearInit()
            actualBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, "Reset;")
            expectedString = "Agilent"
            actualString = target.StatusSubsystem.Identity.Substring(0, Len(expectedString))
            Assert.AreEqual(expectedString, actualString)
            actualBoolean = target.OutputOn(voltage, currentLimit, 0.1, voltage + 2, TimeSpan.FromMilliseconds(1000))
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output On;")
            expectedBoolean = True
            actualBoolean = target.OutputSubsystem.QueryOutputOnState.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output State;")
            expectedDouble = voltage
            actualDouble = target.MeasureVoltageSubsystem.Measure.GetValueOrDefault(0)
            Assert.AreEqual(expectedDouble, actualDouble, 0.1)
            expectedDouble = 0.1
            actualDouble = target.MeasureCurrentSubsystem.Measure.GetValueOrDefault(0)
            If actualDouble < expectedDouble Then
                Assert.AreEqual(expectedDouble, actualDouble, 0.1, "Actual must be greater")
            End If

            expectedString = "no error"
            target.StatusSubsystem.TrySafeQueryDeviceErrors(e)
            Assert.IsFalse(e.Cancel, "Device error query failed")
            actualString = target.StatusSubsystem.LastDeviceError.ErrorMessage
            Assert.AreEqual(expectedString, actualString, True, Globalization.CultureInfo.CurrentCulture, "Device error mismatch")
        End Using
    End Sub

    '''<summary>
    '''A test for OutputOn and Output Off
    '''</summary>
    <TestMethod()>
    Public Sub OutputOnOffTest()
        Dim voltage As Double = 28.0!
        Dim currentLimit As Double = 1.0!
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Dim expectedString As String = ""
        Dim actualString As String = ""
        Dim expectedDouble As Double = 0
        Dim actualDouble As Double = 0
        Using target As PowerSupply.Device = New PowerSupply.Device()
            Dim e As New isr.Core.Pith.ActionEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(VI.Pith.HardwareInterfaceType.Gpib), "Power Supply", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            target.ResetClearInit()
            actualBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, "Reset;")
            expectedString = "Agilent"
            actualString = target.StatusSubsystem.Identity.Substring(0, Len(expectedString))
            Assert.AreEqual(expectedString, actualString)
            actualBoolean = target.OutputOn(voltage, currentLimit, 0.1, voltage + 2, TimeSpan.FromMilliseconds(1000))
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output On;")
            expectedBoolean = True
            actualBoolean = target.OutputSubsystem.QueryOutputOnState.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output State;")
            expectedDouble = voltage
            actualDouble = target.MeasureVoltageSubsystem.Measure.GetValueOrDefault(0)
            Assert.AreEqual(expectedDouble, actualDouble, 0.1)
            expectedDouble = 0.1
            actualDouble = target.MeasureCurrentSubsystem.Measure.GetValueOrDefault(0)
            If actualDouble < expectedDouble Then
                Assert.AreEqual(expectedDouble, actualDouble, 0.1, "Actual must be greater")
            End If
            actualBoolean = target.OutputSubsystem.ApplyOutputOnState(False).GetValueOrDefault(True)
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output Off;")
            expectedBoolean = False
            actualBoolean = target.OutputSubsystem.QueryOutputOnState.GetValueOrDefault(True)
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output State;")

            expectedString = "no error"
            target.StatusSubsystem.TrySafeQueryDeviceErrors(e)
            Assert.IsFalse(e.Cancel, "Device error query failed")
            actualString = target.StatusSubsystem.LastDeviceError.ErrorMessage
            Assert.AreEqual(expectedString, actualString, True, Globalization.CultureInfo.CurrentCulture, "Device error mismatch")

        End Using
    End Sub

    '''<summary>
    '''A test for MeasureCurrent
    '''</summary>
    <TestMethod()>
    Public Sub MeasureCurrentTest()
        Dim expectedDouble As Double = 0.0!
        Dim actualDouble As Double
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using target As PowerSupply.Device = New PowerSupply.Device()
            Dim e As New isr.Core.Pith.ActionEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(VI.Pith.HardwareInterfaceType.Gpib), "Power Supply", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            actualDouble = target.MeasureCurrentSubsystem.Measure.GetValueOrDefault(0)
            Assert.AreEqual(expectedDouble, actualDouble)
        End Using

    End Sub

End Class
