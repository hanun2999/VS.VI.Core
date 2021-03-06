﻿#Region " TYPES "

''' <summary>Gets or sets the status byte flags of the measurement event register.</summary>
<System.Flags()>
Public Enum MeasurementEvents
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Limit 1 Failed")> Limit1Failed = 1
    <ComponentModel.Description("Low Limit 2 Failed")> LowLimit2Failed = 2
    <ComponentModel.Description("High Limit 2 Failed")> HighLimit2Failed = 4
    <ComponentModel.Description("Low Limit 3 Failed")> LowLimit3Failed = 8
    <ComponentModel.Description("High Limit 3 Failed")> HighLimit3Failed = 16
    <ComponentModel.Description("Limits Passed")> LimitsPassed = 32
    <ComponentModel.Description("Reading Available")> ReadingAvailable = 64
    <ComponentModel.Description("Reading Overflow")> ReadingOverflow = 128
    <ComponentModel.Description("Buffer Available")> BufferAvailable = 256
    <ComponentModel.Description("Buffer Full")> BufferFull = 512
    <ComponentModel.Description("Contact Check Failed")> ContactCheckFailed = 1024
    <ComponentModel.Description("Interlock Asserted")> InterlockAsserted = 2048
    <ComponentModel.Description("Over Temperature")> OverTemperature = 4096
    <ComponentModel.Description("Over Voltage Protection")> OvervoltageProtection = 8192
    <ComponentModel.Description("Compliance")> Compliance = 16384
    <ComponentModel.Description("Not Used")> NotUsed = 32768
    <ComponentModel.Description("All")> All = 32767
End Enum

''' <summary>Enumerates the status bits of a source measure status word.</summary>
Public Enum StatusWordBit
    ''' <summary>Measurement was made while in over-range</summary>
    <ComponentModel.Description("Over Range")> OverRange = 0
    <ComponentModel.Description("Filter Enabled")> FilterEnabled = 1
    <ComponentModel.Description("Front Terminals")> FrontTerminals = 2
    <ComponentModel.Description("Hit Compliance")> HitCompliance = 3
    <ComponentModel.Description("Hit Voltage Protection")> HitVoltageProtection = 4
    <ComponentModel.Description("Math Expression Enabled")> MathExpressionEnabled = 5
    <ComponentModel.Description("Null Enabled")> NullEnabled = 6
    <ComponentModel.Description("Limits Enabled")> LimitsEnabled = 7
    <ComponentModel.Description("Limit Result Bit 0")> LimitResultBit0 = 8
    <ComponentModel.Description("Limit Result Bit 1")> LimitResultBit1 = 9
    <ComponentModel.Description("Auto Ohms Enabled")> AutoOhmsEnabled = 10
    <ComponentModel.Description("Voltage Measure Enabled")> VoltageMeasureEnabled = 11
    <ComponentModel.Description("Current Measure Enabled")> CurrentMeasureEnabled = 12
    <ComponentModel.Description("Resistance Measure Enabled")> ResistanceMeasureEnabled = 13
    <ComponentModel.Description("Voltage Source Used")> VoltageSourceUsed = 14
    <ComponentModel.Description("Current Source Used")> CurrentSourceUsed = 15
    <ComponentModel.Description("Hit Range Compliance")> HitRangeCompliance = 16
    <ComponentModel.Description("Offset Compensation Ohms Enabled")> OffsetCompensationOhmsEnabled = 17
    <ComponentModel.Description("Failed Contact Check")> FailedContactCheck = 18
    <ComponentModel.Description("Limit Result Bit 2")> LimitResultBit2 = 19
    <ComponentModel.Description("Limit Result Bit 3")> LimitResultBit3 = 20
    <ComponentModel.Description("Limit Result Bit 4")> LimitResultBit4 = 21 '1048576
    <ComponentModel.Description("Four Wire Enabled")> FourWireEnabled = 22
    <ComponentModel.Description("In Pulse Mode")> InPulseMode = 23
End Enum

''' <summary> Values that represent the resistance range mode. </summary>
Public Enum ResistanceRangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("20 ohm range @ 7.2 mA Test Current")> R20 = 20
    <ComponentModel.Description("200 ohm range @ 960 uA Test Current")> R200 = 200
    <ComponentModel.Description("2k ohm range @ 960 uA Test Current")> R2000 = 2000
    <ComponentModel.Description("20k ohm range @ 96 uA Test Current")> R20000 = 20000
    <ComponentModel.Description("200k ohm range @ 9.6 uA Test Current")> R200000 = 200000
    <ComponentModel.Description("2M ohm range @ 1.9 uA Test Current")> R2000000 = 2000000
    <ComponentModel.Description("20M ohm range @ 1.4 uA Test Current")> R20000000 = 20000000
    <ComponentModel.Description("200M ohm range @ 1.4 uA Test Current")> R200000000 = 200000000
    <ComponentModel.Description("1G ohm range @ 1.4 uA Test Current")> R1000000000 = 1000000000
End Enum

''' <summary> Values that represent the four-wire resistance range mode. </summary>
Public Enum FourWireResistanceRangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("20 ohm range @ 7.2 mA Test Current")> R20 = 20
    <ComponentModel.Description("200 ohm range @ 960 uA Test Current")> R200 = 200
    <ComponentModel.Description("2k ohm range @ 960 uA Test Current")> R2000 = 2000
    <ComponentModel.Description("20k ohm range @ 96 uA Test Current")> R20000 = 20000
    <ComponentModel.Description("200k ohm range @ 9.6 uA Test Current")> R200000 = 200000
    <ComponentModel.Description("2M ohm range @ 1.9 uA Test Current")> R2000000 = 2000000
End Enum

#End Region

