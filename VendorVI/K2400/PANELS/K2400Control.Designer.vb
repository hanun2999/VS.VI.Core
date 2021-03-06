﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K2400Control

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(K2400Control))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._ReadingsDataGridView = New System.Windows.Forms.DataGridView()
        Me._ReadingToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ReadButton = New System.Windows.Forms.ToolStripButton()
        Me._InitiateButton = New System.Windows.Forms.ToolStripButton()
        Me._TraceButton = New System.Windows.Forms.ToolStripButton()
        Me._ReadingsCountLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ReadingComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._AbortButton = New System.Windows.Forms.ToolStripButton()
        Me._ClearBufferDisplayButton = New System.Windows.Forms.ToolStripButton()
        Me._ToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me._SystemToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetSplitButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me._ClearInterfaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadStatusByteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearExecutionStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DisplayTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._SessionServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ServiceRequestHandlersEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TraceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DisplayTraceLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionNotificationLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionNotificationLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DeviceServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._OutputDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me._ContactCheckEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SourceAutoClearEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._OutputTerminalMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ServiceRequestEnableBitmaskNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ServiceRequestEnableBitmaskNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._ResourceSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._SourceTabPage = New System.Windows.Forms.TabPage()
        Me._TriggerDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._TriggerDelayNumericLabel = New System.Windows.Forms.Label()
        Me._ApplySourceFunctionButton = New System.Windows.Forms.Button()
        Me._SourceLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SourceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SourceRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SourceDelayTextBoxLabel = New System.Windows.Forms.Label()
        Me._SourceLevelNumericLabel = New System.Windows.Forms.Label()
        Me._ApplySourceSettingButton = New System.Windows.Forms.Button()
        Me._SourceRangeNumericLabel = New System.Windows.Forms.Label()
        Me._SourceFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me._SourceLimitNumericLabel = New System.Windows.Forms.Label()
        Me._SourceFunctionComboBoxLabel = New System.Windows.Forms.Label()
        Me._SenseTabPage = New System.Windows.Forms.TabPage()
        Me._EnabledSenseFunctionsListBoxLabel = New System.Windows.Forms.Label()
        Me._EnabledSenseFunctionsListBox = New System.Windows.Forms.CheckedListBox()
        Me._ApplySenseFunctionButton = New System.Windows.Forms.Button()
        Me._ConcurrentSenseCheckBox = New System.Windows.Forms.CheckBox()
        Me._FourWireSenseCheckBox = New System.Windows.Forms.CheckBox()
        Me._SenseRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._NplcNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SenseRangeNumericLabel = New System.Windows.Forms.Label()
        Me._NplcNumericLabel = New System.Windows.Forms.Label()
        Me._SenseFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me._SenseFunctionComboBoxLabel = New System.Windows.Forms.Label()
        Me._SenseAutoRangeToggle = New System.Windows.Forms.CheckBox()
        Me._ApplySenseSettingsButton = New System.Windows.Forms.Button()
        Me._HipotTabPage = New System.Windows.Forms.TabPage()
        Me._HipotLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._HipotGroupBox = New System.Windows.Forms.GroupBox()
        Me._ContactCheckToggle = New System.Windows.Forms.CheckBox()
        Me._ApplyHipotSettingsButton = New System.Windows.Forms.Button()
        Me._CurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._CurrentLimitNumericLabel = New System.Windows.Forms.Label()
        Me._VoltageLevelNumericLabel = New System.Windows.Forms.Label()
        Me._VoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ResistanceRangeNumericLabel = New System.Windows.Forms.Label()
        Me._ResistanceLowLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ResistanceRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ResistanceLowLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._DwellTimeNumericLabel = New System.Windows.Forms.Label()
        Me._DwellTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ApertureNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ApertureNumericLabel = New System.Windows.Forms.Label()
        Me._SotTabPage = New System.Windows.Forms.TabPage()
        Me._BinningLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._TriggerToolStrip = New System.Windows.Forms.ToolStrip()
        Me._AwaitTriggerToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me._WaitHourglassLabel = New System.Windows.Forms.ToolStripLabel()
        Me._AssertTriggerToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me._TriggerActionToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._BinningGroupBox = New System.Windows.Forms.GroupBox()
        Me._ArmComboBoxLabel = New System.Windows.Forms.Label()
        Me._ArmSourceComboBox = New System.Windows.Forms.ComboBox()
        Me._ApplySotSettingsButton = New System.Windows.Forms.Button()
        Me._ContactCheckSupportLabel = New System.Windows.Forms.Label()
        Me._FailBitPatternNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PassBitPatternNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ContactCheckBitPatternNumeric = New System.Windows.Forms.NumericUpDown()
        Me._FailBitPatternNumericLabel = New System.Windows.Forms.Label()
        Me._PassBitPatternNumericLabel = New System.Windows.Forms.Label()
        Me._ContactCheckBitPatternNumericLabel = New System.Windows.Forms.Label()
        Me._EotStrobeDurationNumericLabel = New System.Windows.Forms.Label()
        Me._EotStrobeDurationNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._LastErrorTextBox = New System.Windows.Forms.TextBox()
        Me._ReadingStatusStrip = New System.Windows.Forms.StatusStrip()
        Me._FailureToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadingToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StatusRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StandardRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TimingLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._IdentityLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TipsToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._InfoProvider = New isr.Core.Controls.InfoProvider(Me.components)
        Me._LastReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        CType(Me._ReadingsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ReadingToolStrip.SuspendLayout()
        Me._ToolStripPanel.SuspendLayout()
        Me._SystemToolStrip.SuspendLayout()
        Me._SourceTabPage.SuspendLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SourceLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SourceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SourceRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SenseTabPage.SuspendLayout()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._NplcNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._HipotTabPage.SuspendLayout()
        Me._HipotLayout.SuspendLayout()
        Me._HipotGroupBox.SuspendLayout()
        CType(Me._CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._VoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ResistanceRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ResistanceLowLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._DwellTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ApertureNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SotTabPage.SuspendLayout()
        Me._BinningLayout.SuspendLayout()
        Me._TriggerToolStrip.SuspendLayout()
        Me._BinningGroupBox.SuspendLayout()
        CType(Me._FailBitPatternNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PassBitPatternNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ContactCheckBitPatternNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._EotStrobeDurationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._ReadingStatusStrip.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        CType(Me._InfoProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Panel.SuspendLayout()
        Me._Layout.SuspendLayout()
        Me.SuspendLayout()
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._SourceTabPage)
        Me._Tabs.Controls.Add(Me._SenseTabPage)
        Me._Tabs.Controls.Add(Me._HipotTabPage)
        Me._Tabs.Controls.Add(Me._SotTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 71)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.Padding = New System.Drawing.Point(3, 3)
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 340)
        Me._Tabs.TabIndex = 3
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._ReadingsDataGridView)
        Me._ReadingTabPage.Controls.Add(Me._ReadingToolStrip)
        Me._ReadingTabPage.Controls.Add(Me._ToolStripPanel)
        Me._ReadingTabPage.Controls.Add(Me._ResourceSelectorConnector)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 310)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Read"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_ReadingsDataGridView
        '
        Me._ReadingsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._ReadingsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingsDataGridView.Location = New System.Drawing.Point(0, 25)
        Me._ReadingsDataGridView.Name = "_ReadingsDataGridView"
        Me._ReadingsDataGridView.Size = New System.Drawing.Size(356, 230)
        Me._ReadingsDataGridView.TabIndex = 21
        Me._TipsToolTip.SetToolTip(Me._ReadingsDataGridView, "Buffer data")
        '
        '_ReadingToolStrip
        '
        Me._ReadingToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ReadButton, Me._InitiateButton, Me._TraceButton, Me._ReadingsCountLabel, Me._ReadingComboBox, Me._AbortButton, Me._ClearBufferDisplayButton})
        Me._ReadingToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingToolStrip.Name = "_ReadingToolStrip"
        Me._ReadingToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._ReadingToolStrip.TabIndex = 20
        Me._ReadingToolStrip.Text = "ToolStrip1"
        '
        '_ReadButton
        '
        Me._ReadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadButton.Image = CType(resources.GetObject("_ReadButton.Image"), System.Drawing.Image)
        Me._ReadButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(37, 22)
        Me._ReadButton.Text = "Read"
        Me._ReadButton.ToolTipText = "Read single reading"
        '
        '_InitiateButton
        '
        Me._InitiateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._InitiateButton.Image = CType(resources.GetObject("_InitiateButton.Image"), System.Drawing.Image)
        Me._InitiateButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._InitiateButton.Name = "_InitiateButton"
        Me._InitiateButton.Size = New System.Drawing.Size(47, 22)
        Me._InitiateButton.Text = "Initiate"
        '
        '_TraceButton
        '
        Me._TraceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TraceButton.Image = CType(resources.GetObject("_TraceButton.Image"), System.Drawing.Image)
        Me._TraceButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._TraceButton.Name = "_TraceButton"
        Me._TraceButton.Size = New System.Drawing.Size(39, 22)
        Me._TraceButton.Text = "Trace"
        Me._TraceButton.ToolTipText = "Reads the buffer"
        '
        '_ReadingsCountLabel
        '
        Me._ReadingsCountLabel.Name = "_ReadingsCountLabel"
        Me._ReadingsCountLabel.Size = New System.Drawing.Size(13, 22)
        Me._ReadingsCountLabel.Text = "0"
        Me._ReadingsCountLabel.ToolTipText = "Buffer count"
        '
        '_ReadingComboBox
        '
        Me._ReadingComboBox.Name = "_ReadingComboBox"
        Me._ReadingComboBox.Size = New System.Drawing.Size(121, 25)
        Me._ReadingComboBox.ToolTipText = "Select reading type"
        '
        '_AbortButton
        '
        Me._AbortButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._AbortButton.Image = CType(resources.GetObject("_AbortButton.Image"), System.Drawing.Image)
        Me._AbortButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AbortButton.Name = "_AbortButton"
        Me._AbortButton.Size = New System.Drawing.Size(41, 22)
        Me._AbortButton.Text = "Abort"
        Me._AbortButton.ToolTipText = "Aborts active trigger"
        '
        '_ClearBufferDisplayButton
        '
        Me._ClearBufferDisplayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ClearBufferDisplayButton.Font = New System.Drawing.Font("Wingdings", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me._ClearBufferDisplayButton.Image = CType(resources.GetObject("_ClearBufferDisplayButton.Image"), System.Drawing.Image)
        Me._ClearBufferDisplayButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ClearBufferDisplayButton.Name = "_ClearBufferDisplayButton"
        Me._ClearBufferDisplayButton.Size = New System.Drawing.Size(25, 22)
        Me._ClearBufferDisplayButton.Text = """"
        Me._ClearBufferDisplayButton.ToolTipText = "Clears the buffer display"
        '
        '_ToolStripPanel
        '
        Me._ToolStripPanel.Controls.Add(Me._SystemToolStrip)
        Me._ToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ToolStripPanel.Location = New System.Drawing.Point(0, 255)
        Me._ToolStripPanel.Name = "_ToolStripPanel"
        Me._ToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me._ToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me._ToolStripPanel.Size = New System.Drawing.Size(356, 26)
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._SystemToolStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._OutputDropDownButton, Me._ServiceRequestEnableBitmaskNumericLabel, Me._ServiceRequestEnableBitmaskNumeric})
        Me._SystemToolStrip.Location = New System.Drawing.Point(3, 0)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(265, 26)
        Me._SystemToolStrip.TabIndex = 0
        '
        '_ResetSplitButton
        '
        Me._ResetSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ResetSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetMenuItem, Me._ServiceRequestHandlersEnabledMenuItem, Me._TraceMenuItem})
        Me._ResetSplitButton.Image = CType(resources.GetObject("_ResetSplitButton.Image"), System.Drawing.Image)
        Me._ResetSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ResetSplitButton.Name = "_ResetSplitButton"
        Me._ResetSplitButton.Size = New System.Drawing.Size(57, 23)
        Me._ResetSplitButton.Text = "Device"
        Me._ResetSplitButton.ToolTipText = "Opens the device command and settings menu"
        '
        '_ClearInterfaceMenuItem
        '
        Me._ClearInterfaceMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ClearInterfaceMenuItem.Name = "_ClearInterfaceMenuItem"
        Me._ClearInterfaceMenuItem.Size = New System.Drawing.Size(224, 22)
        Me._ClearInterfaceMenuItem.Text = "Clear Interface"
        Me._ClearInterfaceMenuItem.ToolTipText = "Issues an interface clear command"
        '
        '_ClearDeviceMenuItem
        '
        Me._ClearDeviceMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ClearDeviceMenuItem.Name = "_ClearDeviceMenuItem"
        Me._ClearDeviceMenuItem.Size = New System.Drawing.Size(224, 22)
        Me._ClearDeviceMenuItem.Text = "Clear Device (SDC)"
        Me._ClearDeviceMenuItem.ToolTipText = "Issues Selective Device Clear"
        '
        '_ClearExecutionStateMenuItem
        '
        Me._ClearExecutionStateMenuItem.Name = "_ClearExecutionStateMenuItem"
        Me._ClearExecutionStateMenuItem.Size = New System.Drawing.Size(224, 22)
        Me._ClearExecutionStateMenuItem.Text = "Clear Execution State (CLS)"
        Me._ClearExecutionStateMenuItem.ToolTipText = "Clears execution state (CLS)"
        '
        '_ResetKnownStateMenuItem
        '
        Me._ResetKnownStateMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResetKnownStateMenuItem.Name = "_ResetKnownStateMenuItem"
        Me._ResetKnownStateMenuItem.Size = New System.Drawing.Size(224, 22)
        Me._ResetKnownStateMenuItem.Text = "Reset Known State (RST)"
        Me._ResetKnownStateMenuItem.ToolTipText = "Issues *RST"
        '
        '_InitKnownStateMenuItem
        '
        Me._InitKnownStateMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InitKnownStateMenuItem.Name = "_InitKnownStateMenuItem"
        Me._InitKnownStateMenuItem.Size = New System.Drawing.Size(224, 22)
        Me._InitKnownStateMenuItem.Text = "Initialize Known State"
        Me._InitKnownStateMenuItem.ToolTipText = "Initializes to custom known state"
        '
        '_LogTraceLevelComboBox
        '
        Me._LogTraceLevelComboBox.Name = "_LogTraceLevelComboBox"
        Me._LogTraceLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._LogTraceLevelComboBox.Text = "Verbose"
        Me._LogTraceLevelComboBox.ToolTipText = "Log Trace Level"
        '
        '_DisplayTraceLevelComboBox
        '
        Me._DisplayTraceLevelComboBox.Name = "_DisplayTraceLevelComboBox"
        Me._DisplayTraceLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._DisplayTraceLevelComboBox.Text = "Warning"
        Me._DisplayTraceLevelComboBox.ToolTipText = "Display trace level"
        '
        '_SessionServiceRequestHandlerEnabledMenuItem
        '
        Me._SessionServiceRequestHandlerEnabledMenuItem.CheckOnClick = True
        Me._SessionServiceRequestHandlerEnabledMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SessionServiceRequestHandlerEnabledMenuItem.Name = "_SessionServiceRequestHandlerEnabledMenuItem"
        Me._SessionServiceRequestHandlerEnabledMenuItem.Size = New System.Drawing.Size(194, 22)
        Me._SessionServiceRequestHandlerEnabledMenuItem.Text = "Session SRQ Handled"
        Me._SessionServiceRequestHandlerEnabledMenuItem.ToolTipText = "Check to handle Session service requests"
        '
        '_ReadStatusByteMenuItem
        '
        Me._ReadStatusByteMenuItem.Name = "_ReadStatusByteMenuItem"
        Me._ReadStatusByteMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ReadStatusByteMenuItem.Text = "Read Status Byte"
        '
        '_ResetMenuItem
        '
        Me._ResetMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ClearInterfaceMenuItem, Me._ClearDeviceMenuItem, Me._ResetKnownStateMenuItem, Me._InitKnownStateMenuItem, Me._ClearExecutionStateMenuItem, Me._ReadStatusByteMenuItem})
        Me._ResetMenuItem.Name = "_ResetMenuItem"
        Me._ResetMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._ResetMenuItem.Text = "Reset..."
        Me._ResetMenuItem.ToolTipText = "Opens the reset menus"
        '
        '_ServiceRequestHandlersEnabledMenuItem
        '
        Me._ServiceRequestHandlersEnabledMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionServiceRequestHandlerEnabledMenuItem, Me._DeviceServiceRequestHandlerEnabledMenuItem})
        Me._ServiceRequestHandlersEnabledMenuItem.Name = "_ServiceRequestHandlersEnabledMenuItem"
        Me._ServiceRequestHandlersEnabledMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._ServiceRequestHandlersEnabledMenuItem.Text = "SRQ Handlers Enable..."
        Me._ServiceRequestHandlersEnabledMenuItem.ToolTipText = "Opens the SQR Handler Enable menu"
        '
        '_TraceMenuItem
        '
        Me._TraceMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionNotificationLevelMenuItem, Me._LogTraceLevelMenuItem, Me._DisplayTraceLevelMenuItem})
        Me._TraceMenuItem.Name = "_TraceMenuItem"
        Me._TraceMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._TraceMenuItem.Text = "Trace..."
        Me._TraceMenuItem.ToolTipText = "Opens the trace menus"
        '
        '_SessionNotificationLevelMenuItem
        '
        Me._SessionNotificationLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionNotificationLevelComboBox})
        Me._SessionNotificationLevelMenuItem.Name = "_SessionNotificationLevelMenuItem"
        Me._SessionNotificationLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._SessionNotificationLevelMenuItem.Text = "Session Notification Level"
        Me._SessionNotificationLevelMenuItem.ToolTipText = "Shows the session notification level selector"
        '
        '_SessionNotificationLevelComboBox
        '
        Me._SessionNotificationLevelComboBox.Name = "_SessionNotificationLevelComboBox"
        Me._SessionNotificationLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._SessionNotificationLevelComboBox.ToolTipText = "Select the session notification level"
        '
        '_LogTraceLevelMenuItem
        '
        Me._LogTraceLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._LogTraceLevelComboBox})
        Me._LogTraceLevelMenuItem.Name = "_LogTraceLevelMenuItem"
        Me._LogTraceLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._LogTraceLevelMenuItem.Text = "Log Trace Level"
        Me._LogTraceLevelMenuItem.ToolTipText = "Shows the log trace levels"
        '
        '_DisplayTraceLevelMenuItem
        '
        Me._DisplayTraceLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._DisplayTraceLevelComboBox})
        Me._DisplayTraceLevelMenuItem.Name = "_DisplayTraceLevelMenuItem"
        Me._DisplayTraceLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._DisplayTraceLevelMenuItem.Text = "Display Trace Level"
        Me._DisplayTraceLevelMenuItem.ToolTipText = "Shows the display trace levels"
        '
        '_DeviceServiceRequestHandlerEnabledMenuItem
        '
        Me._DeviceServiceRequestHandlerEnabledMenuItem.CheckOnClick = True
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Name = "_DeviceServiceRequestHandlerEnabledMenuItem"
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Size = New System.Drawing.Size(194, 22)
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Text = "Device SRQ Handled"
        Me._DeviceServiceRequestHandlerEnabledMenuItem.ToolTipText = "Check to handle Device service requests"
        '
        '_OutputDropDownButton
        '
        Me._OutputDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._OutputDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ContactCheckEnabledMenuItem, Me._SourceAutoClearEnabledMenuItem, Me._OutputTerminalMenuItem})
        Me._OutputDropDownButton.Image = CType(resources.GetObject("_OutputDropDownButton.Image"), System.Drawing.Image)
        Me._OutputDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._OutputDropDownButton.Name = "_OutputDropDownButton"
        Me._OutputDropDownButton.Size = New System.Drawing.Size(60, 23)
        Me._OutputDropDownButton.Text = "Output"
        Me._OutputDropDownButton.ToolTipText = "Output control options"
        '
        '_ContactCheckEnabledMenuItem
        '
        Me._ContactCheckEnabledMenuItem.CheckOnClick = True
        Me._ContactCheckEnabledMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ContactCheckEnabledMenuItem.Name = "_ContactCheckEnabledMenuItem"
        Me._ContactCheckEnabledMenuItem.Size = New System.Drawing.Size(200, 22)
        Me._ContactCheckEnabledMenuItem.Text = "Contact Check Enabled"
        Me._ContactCheckEnabledMenuItem.ToolTipText = "Check to enable contact check"
        '
        '_SourceAutoClearEnabledMenuItem
        '
        Me._SourceAutoClearEnabledMenuItem.CheckOnClick = True
        Me._SourceAutoClearEnabledMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SourceAutoClearEnabledMenuItem.Name = "_SourceAutoClearEnabledMenuItem"
        Me._SourceAutoClearEnabledMenuItem.Size = New System.Drawing.Size(200, 22)
        Me._SourceAutoClearEnabledMenuItem.Text = "Auto On"
        Me._SourceAutoClearEnabledMenuItem.ToolTipText = "Check to turn on output automatically"
        '
        '_OutputTerminalMenuItem
        '
        Me._OutputTerminalMenuItem.CheckOnClick = True
        Me._OutputTerminalMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OutputTerminalMenuItem.Name = "_OutputTerminalMenuItem"
        Me._OutputTerminalMenuItem.Size = New System.Drawing.Size(200, 22)
        Me._OutputTerminalMenuItem.Text = "Front"
        Me._OutputTerminalMenuItem.ToolTipText = "Check to toggle using rear or front terminals"
        '
        '_ServiceRequestEnableBitmaskNumericLabel
        '
        Me._ServiceRequestEnableBitmaskNumericLabel.Name = "_ServiceRequestEnableBitmaskNumericLabel"
        Me._ServiceRequestEnableBitmaskNumericLabel.Size = New System.Drawing.Size(31, 23)
        Me._ServiceRequestEnableBitmaskNumericLabel.Text = "SRE:"
        '
        '_ServiceRequestEnableBitmaskNumeric
        '
        Me._ServiceRequestEnableBitmaskNumeric.Name = "_ServiceRequestEnableBitmaskNumeric"
        Me._ServiceRequestEnableBitmaskNumeric.Size = New System.Drawing.Size(44, 23)
        Me._ServiceRequestEnableBitmaskNumeric.Text = "0"
        Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = "Service request enable bitmask"
        Me._ServiceRequestEnableBitmaskNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_ResourceSelectorConnector
        '
        Me._ResourceSelectorConnector.BackColor = System.Drawing.Color.Transparent
        Me._ResourceSelectorConnector.ClearToolTipText = "Clear device state"
        Me._ResourceSelectorConnector.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ResourceSelectorConnector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceSelectorConnector.Location = New System.Drawing.Point(0, 281)
        Me._ResourceSelectorConnector.Margin = New System.Windows.Forms.Padding(0)
        Me._ResourceSelectorConnector.Name = "_ResourceSelectorConnector"
        Me._ResourceSelectorConnector.Size = New System.Drawing.Size(356, 29)
        Me._ResourceSelectorConnector.TabIndex = 0
        '
        '_SourceTabPage
        '
        Me._SourceTabPage.Controls.Add(Me._TriggerDelayNumeric)
        Me._SourceTabPage.Controls.Add(Me._TriggerDelayNumericLabel)
        Me._SourceTabPage.Controls.Add(Me._ApplySourceFunctionButton)
        Me._SourceTabPage.Controls.Add(Me._SourceLimitNumeric)
        Me._SourceTabPage.Controls.Add(Me._SourceLevelNumeric)
        Me._SourceTabPage.Controls.Add(Me._SourceRangeNumeric)
        Me._SourceTabPage.Controls.Add(Me._SourceDelayNumeric)
        Me._SourceTabPage.Controls.Add(Me._SourceDelayTextBoxLabel)
        Me._SourceTabPage.Controls.Add(Me._SourceLevelNumericLabel)
        Me._SourceTabPage.Controls.Add(Me._ApplySourceSettingButton)
        Me._SourceTabPage.Controls.Add(Me._SourceRangeNumericLabel)
        Me._SourceTabPage.Controls.Add(Me._SourceFunctionComboBox)
        Me._SourceTabPage.Controls.Add(Me._SourceLimitNumericLabel)
        Me._SourceTabPage.Controls.Add(Me._SourceFunctionComboBoxLabel)
        Me._SourceTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SourceTabPage.Name = "_SourceTabPage"
        Me._SourceTabPage.Size = New System.Drawing.Size(356, 310)
        Me._SourceTabPage.TabIndex = 1
        Me._SourceTabPage.Text = "Source"
        Me._SourceTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerDelayNumeric
        '
        Me._TriggerDelayNumeric.DecimalPlaces = 3
        Me._TriggerDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelayNumeric.Location = New System.Drawing.Point(116, 172)
        Me._TriggerDelayNumeric.Name = "_TriggerDelayNumeric"
        Me._TriggerDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._TriggerDelayNumeric.TabIndex = 22
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(6, 176)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(107, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 21
        Me._TriggerDelayNumericLabel.Text = "Trigger Delay [s]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ApplySourceFunctionButton
        '
        Me._ApplySourceFunctionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySourceFunctionButton.Location = New System.Drawing.Point(269, 4)
        Me._ApplySourceFunctionButton.Name = "_ApplySourceFunctionButton"
        Me._ApplySourceFunctionButton.Size = New System.Drawing.Size(53, 30)
        Me._ApplySourceFunctionButton.TabIndex = 20
        Me._ApplySourceFunctionButton.Text = "Apply"
        Me._TipsToolTip.SetToolTip(Me._ApplySourceFunctionButton, "Apply selected source function")
        Me._ApplySourceFunctionButton.UseVisualStyleBackColor = True
        '
        '_SourceLimitNumeric
        '
        Me._SourceLimitNumeric.DecimalPlaces = 4
        Me._SourceLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SourceLimitNumeric.Location = New System.Drawing.Point(116, 126)
        Me._SourceLimitNumeric.Name = "_SourceLimitNumeric"
        Me._SourceLimitNumeric.Size = New System.Drawing.Size(77, 25)
        Me._SourceLimitNumeric.TabIndex = 19
        Me._TipsToolTip.SetToolTip(Me._SourceLimitNumeric, "Source limit")
        '
        '_SourceLevelNumeric
        '
        Me._SourceLevelNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SourceLevelNumeric.Location = New System.Drawing.Point(116, 95)
        Me._SourceLevelNumeric.Maximum = New Decimal(New Integer() {1100, 0, 0, 0})
        Me._SourceLevelNumeric.Name = "_SourceLevelNumeric"
        Me._SourceLevelNumeric.Size = New System.Drawing.Size(77, 25)
        Me._SourceLevelNumeric.TabIndex = 18
        '
        '_SourceRangeNumeric
        '
        Me._SourceRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SourceRangeNumeric.Location = New System.Drawing.Point(116, 66)
        Me._SourceRangeNumeric.Maximum = New Decimal(New Integer() {1100, 0, 0, 0})
        Me._SourceRangeNumeric.Name = "_SourceRangeNumeric"
        Me._SourceRangeNumeric.Size = New System.Drawing.Size(77, 25)
        Me._SourceRangeNumeric.TabIndex = 18
        '
        '_SourceDelayNumeric
        '
        Me._SourceDelayNumeric.DecimalPlaces = 2
        Me._SourceDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SourceDelayNumeric.Location = New System.Drawing.Point(116, 37)
        Me._SourceDelayNumeric.Name = "_SourceDelayNumeric"
        Me._SourceDelayNumeric.Size = New System.Drawing.Size(77, 25)
        Me._SourceDelayNumeric.TabIndex = 17
        Me._TipsToolTip.SetToolTip(Me._SourceDelayNumeric, "Source delay in milliseconds")
        '
        '_SourceDelayTextBoxLabel
        '
        Me._SourceDelayTextBoxLabel.AutoSize = True
        Me._SourceDelayTextBoxLabel.Location = New System.Drawing.Point(41, 41)
        Me._SourceDelayTextBoxLabel.Name = "_SourceDelayTextBoxLabel"
        Me._SourceDelayTextBoxLabel.Size = New System.Drawing.Size(72, 17)
        Me._SourceDelayTextBoxLabel.TabIndex = 16
        Me._SourceDelayTextBoxLabel.Text = "Delay [ms]:"
        Me._SourceDelayTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SourceLevelNumericLabel
        '
        Me._SourceLevelNumericLabel.AutoSize = True
        Me._SourceLevelNumericLabel.Location = New System.Drawing.Point(53, 99)
        Me._SourceLevelNumericLabel.Name = "_SourceLevelNumericLabel"
        Me._SourceLevelNumericLabel.Size = New System.Drawing.Size(60, 17)
        Me._SourceLevelNumericLabel.TabIndex = 9
        Me._SourceLevelNumericLabel.Text = "Level [V]:"
        Me._SourceLevelNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ApplySourceSettingButton
        '
        Me._ApplySourceSettingButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySourceSettingButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ApplySourceSettingButton.Location = New System.Drawing.Point(289, 270)
        Me._ApplySourceSettingButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ApplySourceSettingButton.Name = "_ApplySourceSettingButton"
        Me._ApplySourceSettingButton.Size = New System.Drawing.Size(58, 30)
        Me._ApplySourceSettingButton.TabIndex = 15
        Me._ApplySourceSettingButton.Text = "&Apply"
        Me._ApplySourceSettingButton.UseVisualStyleBackColor = True
        '
        '_SourceRangeNumericLabel
        '
        Me._SourceRangeNumericLabel.AutoSize = True
        Me._SourceRangeNumericLabel.Location = New System.Drawing.Point(45, 70)
        Me._SourceRangeNumericLabel.Name = "_SourceRangeNumericLabel"
        Me._SourceRangeNumericLabel.Size = New System.Drawing.Size(68, 17)
        Me._SourceRangeNumericLabel.TabIndex = 9
        Me._SourceRangeNumericLabel.Text = "Range [V]:"
        Me._SourceRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SourceFunctionComboBox
        '
        Me._SourceFunctionComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SourceFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SourceFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SourceFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SourceFunctionComboBox.Location = New System.Drawing.Point(116, 6)
        Me._SourceFunctionComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SourceFunctionComboBox.Name = "_SourceFunctionComboBox"
        Me._SourceFunctionComboBox.Size = New System.Drawing.Size(150, 25)
        Me._SourceFunctionComboBox.TabIndex = 14
        '
        '_SourceLimitNumericLabel
        '
        Me._SourceLimitNumericLabel.AutoSize = True
        Me._SourceLimitNumericLabel.Location = New System.Drawing.Point(55, 130)
        Me._SourceLimitNumericLabel.Name = "_SourceLimitNumericLabel"
        Me._SourceLimitNumericLabel.Size = New System.Drawing.Size(58, 17)
        Me._SourceLimitNumericLabel.TabIndex = 11
        Me._SourceLimitNumericLabel.Text = "Limit [A]:"
        Me._SourceLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SourceFunctionComboBoxLabel
        '
        Me._SourceFunctionComboBoxLabel.AutoSize = True
        Me._SourceFunctionComboBoxLabel.Location = New System.Drawing.Point(54, 10)
        Me._SourceFunctionComboBoxLabel.Name = "_SourceFunctionComboBoxLabel"
        Me._SourceFunctionComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SourceFunctionComboBoxLabel.TabIndex = 13
        Me._SourceFunctionComboBoxLabel.Text = "Function:"
        '
        '_SenseTabPage
        '
        Me._SenseTabPage.Controls.Add(Me._EnabledSenseFunctionsListBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._EnabledSenseFunctionsListBox)
        Me._SenseTabPage.Controls.Add(Me._ApplySenseFunctionButton)
        Me._SenseTabPage.Controls.Add(Me._ConcurrentSenseCheckBox)
        Me._SenseTabPage.Controls.Add(Me._FourWireSenseCheckBox)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumeric)
        Me._SenseTabPage.Controls.Add(Me._NplcNumeric)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._NplcNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBox)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseAutoRangeToggle)
        Me._SenseTabPage.Controls.Add(Me._ApplySenseSettingsButton)
        Me._SenseTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SenseTabPage.Name = "_SenseTabPage"
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 248)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_EnabledSenseFunctionsListBoxLabel
        '
        Me._EnabledSenseFunctionsListBoxLabel.AutoSize = True
        Me._EnabledSenseFunctionsListBoxLabel.Location = New System.Drawing.Point(13, 6)
        Me._EnabledSenseFunctionsListBoxLabel.Name = "_EnabledSenseFunctionsListBoxLabel"
        Me._EnabledSenseFunctionsListBoxLabel.Size = New System.Drawing.Size(65, 17)
        Me._EnabledSenseFunctionsListBoxLabel.TabIndex = 14
        Me._EnabledSenseFunctionsListBoxLabel.Text = "Functions:"
        '
        '_EnabledSenseFunctionsListBox
        '
        Me._EnabledSenseFunctionsListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._EnabledSenseFunctionsListBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._EnabledSenseFunctionsListBox.FormattingEnabled = True
        Me._EnabledSenseFunctionsListBox.Location = New System.Drawing.Point(81, 6)
        Me._EnabledSenseFunctionsListBox.Name = "_EnabledSenseFunctionsListBox"
        Me._EnabledSenseFunctionsListBox.Size = New System.Drawing.Size(144, 64)
        Me._EnabledSenseFunctionsListBox.TabIndex = 13
        Me._TipsToolTip.SetToolTip(Me._EnabledSenseFunctionsListBox, "Enabled functions")
        '
        '_ApplySenseFunctionButton
        '
        Me._ApplySenseFunctionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySenseFunctionButton.Location = New System.Drawing.Point(288, 57)
        Me._ApplySenseFunctionButton.Name = "_ApplySenseFunctionButton"
        Me._ApplySenseFunctionButton.Size = New System.Drawing.Size(53, 30)
        Me._ApplySenseFunctionButton.TabIndex = 12
        Me._ApplySenseFunctionButton.Text = "Apply"
        Me._TipsToolTip.SetToolTip(Me._ApplySenseFunctionButton, "Click to apply")
        Me._ApplySenseFunctionButton.UseVisualStyleBackColor = True
        '
        '_ConcurrentSenseCheckBox
        '
        Me._ConcurrentSenseCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ConcurrentSenseCheckBox.AutoSize = True
        Me._ConcurrentSenseCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ConcurrentSenseCheckBox.Location = New System.Drawing.Point(233, 6)
        Me._ConcurrentSenseCheckBox.Name = "_ConcurrentSenseCheckBox"
        Me._ConcurrentSenseCheckBox.Size = New System.Drawing.Size(95, 21)
        Me._ConcurrentSenseCheckBox.TabIndex = 11
        Me._ConcurrentSenseCheckBox.Text = "Concurrent"
        Me._ConcurrentSenseCheckBox.UseVisualStyleBackColor = True
        '
        '_FourWireSenseCheckBox
        '
        Me._FourWireSenseCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._FourWireSenseCheckBox.AutoSize = True
        Me._FourWireSenseCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FourWireSenseCheckBox.Location = New System.Drawing.Point(233, 32)
        Me._FourWireSenseCheckBox.Name = "_FourWireSenseCheckBox"
        Me._FourWireSenseCheckBox.Size = New System.Drawing.Size(88, 21)
        Me._FourWireSenseCheckBox.TabIndex = 10
        Me._FourWireSenseCheckBox.Text = "Four Wire"
        Me._FourWireSenseCheckBox.UseVisualStyleBackColor = True
        '
        '_SenseRangeNumeric
        '
        Me._SenseRangeNumeric.DecimalPlaces = 3
        Me._SenseRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SenseRangeNumeric.Location = New System.Drawing.Point(116, 179)
        Me._SenseRangeNumeric.Maximum = New Decimal(New Integer() {1010, 0, 0, 0})
        Me._SenseRangeNumeric.Name = "_SenseRangeNumeric"
        Me._SenseRangeNumeric.Size = New System.Drawing.Size(76, 25)
        Me._SenseRangeNumeric.TabIndex = 5
        Me._TipsToolTip.SetToolTip(Me._SenseRangeNumeric, "Range")
        Me._SenseRangeNumeric.Value = New Decimal(New Integer() {105, 0, 0, 196608})
        '
        '_NplcNumeric
        '
        Me._NplcNumeric.DecimalPlaces = 3
        Me._NplcNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._NplcNumeric.Location = New System.Drawing.Point(116, 208)
        Me._NplcNumeric.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me._NplcNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me._NplcNumeric.Name = "_NplcNumeric"
        Me._NplcNumeric.Size = New System.Drawing.Size(76, 25)
        Me._NplcNumeric.TabIndex = 3
        Me._TipsToolTip.SetToolTip(Me._NplcNumeric, "Number of power line cycles")
        Me._NplcNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_SenseRangeNumericLabel
        '
        Me._SenseRangeNumericLabel.AutoSize = True
        Me._SenseRangeNumericLabel.Location = New System.Drawing.Point(45, 182)
        Me._SenseRangeNumericLabel.Name = "_SenseRangeNumericLabel"
        Me._SenseRangeNumericLabel.Size = New System.Drawing.Size(68, 17)
        Me._SenseRangeNumericLabel.TabIndex = 4
        Me._SenseRangeNumericLabel.Text = "Range [V]:"
        Me._SenseRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_NplcNumericLabel
        '
        Me._NplcNumericLabel.AutoSize = True
        Me._NplcNumericLabel.Location = New System.Drawing.Point(8, 212)
        Me._NplcNumericLabel.Name = "_NplcNumericLabel"
        Me._NplcNumericLabel.Size = New System.Drawing.Size(105, 17)
        Me._NplcNumericLabel.TabIndex = 2
        Me._NplcNumericLabel.Text = "Aperture [NPLC]:"
        Me._NplcNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(116, 148)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(187, 25)
        Me._SenseFunctionComboBox.TabIndex = 1
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.AutoSize = True
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(57, 152)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SenseFunctionComboBoxLabel.TabIndex = 0
        Me._SenseFunctionComboBoxLabel.Text = "Function:"
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseAutoRangeToggle
        '
        Me._SenseAutoRangeToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseAutoRangeToggle.Location = New System.Drawing.Point(198, 181)
        Me._SenseAutoRangeToggle.Name = "_SenseAutoRangeToggle"
        Me._SenseAutoRangeToggle.Size = New System.Drawing.Size(103, 21)
        Me._SenseAutoRangeToggle.TabIndex = 6
        Me._SenseAutoRangeToggle.Text = "Auto Range"
        Me._SenseAutoRangeToggle.UseVisualStyleBackColor = True
        '
        '_ApplySenseSettingsButton
        '
        Me._ApplySenseSettingsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySenseSettingsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(290, 210)
        Me._ApplySenseSettingsButton.Name = "_ApplySenseSettingsButton"
        Me._ApplySenseSettingsButton.Size = New System.Drawing.Size(58, 30)
        Me._ApplySenseSettingsButton.TabIndex = 9
        Me._ApplySenseSettingsButton.Text = "&Apply"
        Me._ApplySenseSettingsButton.UseVisualStyleBackColor = True
        '
        '_HipotTabPage
        '
        Me._HipotTabPage.Controls.Add(Me._HipotLayout)
        Me._HipotTabPage.Location = New System.Drawing.Point(4, 26)
        Me._HipotTabPage.Name = "_HipotTabPage"
        Me._HipotTabPage.Size = New System.Drawing.Size(356, 248)
        Me._HipotTabPage.TabIndex = 6
        Me._HipotTabPage.Text = "Hipot"
        Me._HipotTabPage.UseVisualStyleBackColor = True
        '
        '_HipotLayout
        '
        Me._HipotLayout.ColumnCount = 3
        Me._HipotLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._HipotLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._HipotLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._HipotLayout.Controls.Add(Me._HipotGroupBox, 1, 1)
        Me._HipotLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._HipotLayout.Location = New System.Drawing.Point(0, 0)
        Me._HipotLayout.Name = "_HipotLayout"
        Me._HipotLayout.RowCount = 3
        Me._HipotLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._HipotLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._HipotLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._HipotLayout.Size = New System.Drawing.Size(356, 248)
        Me._HipotLayout.TabIndex = 3
        '
        '_HipotGroupBox
        '
        Me._HipotGroupBox.Controls.Add(Me._ContactCheckToggle)
        Me._HipotGroupBox.Controls.Add(Me._ApplyHipotSettingsButton)
        Me._HipotGroupBox.Controls.Add(Me._CurrentLimitNumeric)
        Me._HipotGroupBox.Controls.Add(Me._CurrentLimitNumericLabel)
        Me._HipotGroupBox.Controls.Add(Me._VoltageLevelNumericLabel)
        Me._HipotGroupBox.Controls.Add(Me._VoltageLevelNumeric)
        Me._HipotGroupBox.Controls.Add(Me._ResistanceRangeNumericLabel)
        Me._HipotGroupBox.Controls.Add(Me._ResistanceLowLimitNumericLabel)
        Me._HipotGroupBox.Controls.Add(Me._ResistanceRangeNumeric)
        Me._HipotGroupBox.Controls.Add(Me._ResistanceLowLimitNumeric)
        Me._HipotGroupBox.Controls.Add(Me._DwellTimeNumericLabel)
        Me._HipotGroupBox.Controls.Add(Me._DwellTimeNumeric)
        Me._HipotGroupBox.Controls.Add(Me._ApertureNumeric)
        Me._HipotGroupBox.Controls.Add(Me._ApertureNumericLabel)
        Me._HipotGroupBox.Location = New System.Drawing.Point(36, 15)
        Me._HipotGroupBox.Name = "_HipotGroupBox"
        Me._HipotGroupBox.Size = New System.Drawing.Size(283, 217)
        Me._HipotGroupBox.TabIndex = 2
        Me._HipotGroupBox.TabStop = False
        Me._HipotGroupBox.Text = "HIPOT SETTINGS"
        '
        '_ContactCheckToggle
        '
        Me._ContactCheckToggle.AutoSize = True
        Me._ContactCheckToggle.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._ContactCheckToggle.Location = New System.Drawing.Point(19, 187)
        Me._ContactCheckToggle.Name = "_ContactCheckToggle"
        Me._ContactCheckToggle.Size = New System.Drawing.Size(161, 21)
        Me._ContactCheckToggle.TabIndex = 13
        Me._ContactCheckToggle.Text = "Contact check enabled:"
        Me._ContactCheckToggle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._ContactCheckToggle.UseVisualStyleBackColor = True
        '
        '_ApplyHipotSettingsButton
        '
        Me._ApplyHipotSettingsButton.Location = New System.Drawing.Point(197, 183)
        Me._ApplyHipotSettingsButton.Name = "_ApplyHipotSettingsButton"
        Me._ApplyHipotSettingsButton.Size = New System.Drawing.Size(75, 28)
        Me._ApplyHipotSettingsButton.TabIndex = 12
        Me._ApplyHipotSettingsButton.Text = "Apply"
        Me._ApplyHipotSettingsButton.UseVisualStyleBackColor = True
        '
        '_CurrentLimitNumeric
        '
        Me._CurrentLimitNumeric.DecimalPlaces = 1
        Me._CurrentLimitNumeric.Location = New System.Drawing.Point(191, 102)
        Me._CurrentLimitNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me._CurrentLimitNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._CurrentLimitNumeric.Name = "_CurrentLimitNumeric"
        Me._CurrentLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._CurrentLimitNumeric.Size = New System.Drawing.Size(79, 25)
        Me._CurrentLimitNumeric.TabIndex = 7
        Me._CurrentLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._CurrentLimitNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        '_CurrentLimitNumericLabel
        '
        Me._CurrentLimitNumericLabel.AutoSize = True
        Me._CurrentLimitNumericLabel.Location = New System.Drawing.Point(76, 106)
        Me._CurrentLimitNumericLabel.Name = "_CurrentLimitNumericLabel"
        Me._CurrentLimitNumericLabel.Size = New System.Drawing.Size(112, 17)
        Me._CurrentLimitNumericLabel.TabIndex = 6
        Me._CurrentLimitNumericLabel.Text = "Current Limit [uA]:"
        '
        '_VoltageLevelNumericLabel
        '
        Me._VoltageLevelNumericLabel.AutoSize = True
        Me._VoltageLevelNumericLabel.Location = New System.Drawing.Point(80, 79)
        Me._VoltageLevelNumericLabel.Name = "_VoltageLevelNumericLabel"
        Me._VoltageLevelNumericLabel.Size = New System.Drawing.Size(108, 17)
        Me._VoltageLevelNumericLabel.TabIndex = 4
        Me._VoltageLevelNumericLabel.Text = "Voltage Level [V]:"
        '
        '_VoltageLevelNumeric
        '
        Me._VoltageLevelNumeric.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me._VoltageLevelNumeric.Location = New System.Drawing.Point(191, 75)
        Me._VoltageLevelNumeric.Maximum = New Decimal(New Integer() {1100, 0, 0, 0})
        Me._VoltageLevelNumeric.Name = "_VoltageLevelNumeric"
        Me._VoltageLevelNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._VoltageLevelNumeric.Size = New System.Drawing.Size(79, 25)
        Me._VoltageLevelNumeric.TabIndex = 5
        Me._VoltageLevelNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._VoltageLevelNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        '_ResistanceRangeNumericLabel
        '
        Me._ResistanceRangeNumericLabel.AutoSize = True
        Me._ResistanceRangeNumericLabel.Location = New System.Drawing.Point(19, 160)
        Me._ResistanceRangeNumericLabel.Name = "_ResistanceRangeNumericLabel"
        Me._ResistanceRangeNumericLabel.Size = New System.Drawing.Size(169, 17)
        Me._ResistanceRangeNumericLabel.TabIndex = 10
        Me._ResistanceRangeNumericLabel.Text = "Resistance Range [M Ohm]:"
        '
        '_ResistanceLowLimitNumericLabel
        '
        Me._ResistanceLowLimitNumericLabel.AutoSize = True
        Me._ResistanceLowLimitNumericLabel.Location = New System.Drawing.Point(5, 133)
        Me._ResistanceLowLimitNumericLabel.Name = "_ResistanceLowLimitNumericLabel"
        Me._ResistanceLowLimitNumericLabel.Size = New System.Drawing.Size(183, 17)
        Me._ResistanceLowLimitNumericLabel.TabIndex = 8
        Me._ResistanceLowLimitNumericLabel.Text = "Resistance low Limit [M Ohm]:"
        '
        '_ResistanceRangeNumeric
        '
        Me._ResistanceRangeNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me._ResistanceRangeNumeric.Location = New System.Drawing.Point(191, 156)
        Me._ResistanceRangeNumeric.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me._ResistanceRangeNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._ResistanceRangeNumeric.Name = "_ResistanceRangeNumeric"
        Me._ResistanceRangeNumeric.Size = New System.Drawing.Size(79, 25)
        Me._ResistanceRangeNumeric.TabIndex = 11
        Me._ResistanceRangeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ResistanceRangeNumeric.ThousandsSeparator = True
        Me._ResistanceRangeNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        '_ResistanceLowLimitNumeric
        '
        Me._ResistanceLowLimitNumeric.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ResistanceLowLimitNumeric.Location = New System.Drawing.Point(191, 129)
        Me._ResistanceLowLimitNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me._ResistanceLowLimitNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._ResistanceLowLimitNumeric.Name = "_ResistanceLowLimitNumeric"
        Me._ResistanceLowLimitNumeric.Size = New System.Drawing.Size(79, 25)
        Me._ResistanceLowLimitNumeric.TabIndex = 9
        Me._ResistanceLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ResistanceLowLimitNumeric.ThousandsSeparator = True
        Me._ResistanceLowLimitNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        '_DwellTimeNumericLabel
        '
        Me._DwellTimeNumericLabel.AutoSize = True
        Me._DwellTimeNumericLabel.Location = New System.Drawing.Point(98, 52)
        Me._DwellTimeNumericLabel.Name = "_DwellTimeNumericLabel"
        Me._DwellTimeNumericLabel.Size = New System.Drawing.Size(90, 17)
        Me._DwellTimeNumericLabel.TabIndex = 2
        Me._DwellTimeNumericLabel.Text = "Dwell time [S]:"
        '
        '_DwellTimeNumeric
        '
        Me._DwellTimeNumeric.DecimalPlaces = 2
        Me._DwellTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._DwellTimeNumeric.Location = New System.Drawing.Point(191, 48)
        Me._DwellTimeNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._DwellTimeNumeric.Name = "_DwellTimeNumeric"
        Me._DwellTimeNumeric.Size = New System.Drawing.Size(79, 25)
        Me._DwellTimeNumeric.TabIndex = 3
        Me._DwellTimeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._DwellTimeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        '_ApertureNumeric
        '
        Me._ApertureNumeric.DecimalPlaces = 2
        Me._ApertureNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._ApertureNumeric.Location = New System.Drawing.Point(191, 21)
        Me._ApertureNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ApertureNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me._ApertureNumeric.Name = "_ApertureNumeric"
        Me._ApertureNumeric.Size = New System.Drawing.Size(79, 25)
        Me._ApertureNumeric.TabIndex = 1
        Me._ApertureNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ApertureNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_ApertureNumericLabel
        '
        Me._ApertureNumericLabel.AutoSize = True
        Me._ApertureNumericLabel.Location = New System.Drawing.Point(86, 25)
        Me._ApertureNumericLabel.Name = "_ApertureNumericLabel"
        Me._ApertureNumericLabel.Size = New System.Drawing.Size(102, 17)
        Me._ApertureNumericLabel.TabIndex = 0
        Me._ApertureNumericLabel.Text = "Aperture [NPLC]"
        '
        '_SotTabPage
        '
        Me._SotTabPage.Controls.Add(Me._BinningLayout)
        Me._SotTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SotTabPage.Name = "_SotTabPage"
        Me._SotTabPage.Size = New System.Drawing.Size(356, 248)
        Me._SotTabPage.TabIndex = 2
        Me._SotTabPage.Text = "SOT"
        Me._SotTabPage.UseVisualStyleBackColor = True
        '
        '_BinningLayout
        '
        Me._BinningLayout.ColumnCount = 3
        Me._BinningLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._BinningLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._BinningLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._BinningLayout.Controls.Add(Me._TriggerToolStrip, 1, 3)
        Me._BinningLayout.Controls.Add(Me._BinningGroupBox, 1, 1)
        Me._BinningLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._BinningLayout.Location = New System.Drawing.Point(0, 0)
        Me._BinningLayout.Name = "_BinningLayout"
        Me._BinningLayout.RowCount = 5
        Me._BinningLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me._BinningLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._BinningLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me._BinningLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._BinningLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me._BinningLayout.Size = New System.Drawing.Size(356, 248)
        Me._BinningLayout.TabIndex = 0
        '
        '_TriggerToolStrip
        '
        Me._TriggerToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._TriggerToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._AwaitTriggerToolStripButton, Me._WaitHourglassLabel, Me._AssertTriggerToolStripButton, Me._TriggerActionToolStripLabel})
        Me._TriggerToolStrip.Location = New System.Drawing.Point(13, 203)
        Me._TriggerToolStrip.Name = "_TriggerToolStrip"
        Me._TriggerToolStrip.Size = New System.Drawing.Size(270, 25)
        Me._TriggerToolStrip.Stretch = True
        Me._TriggerToolStrip.TabIndex = 6
        Me._TriggerToolStrip.Text = "Trigger Tool Strip"
        '
        '_AwaitTriggerToolStripButton
        '
        Me._AwaitTriggerToolStripButton.CheckOnClick = True
        Me._AwaitTriggerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._AwaitTriggerToolStripButton.Image = CType(resources.GetObject("_AwaitTriggerToolStripButton.Image"), System.Drawing.Image)
        Me._AwaitTriggerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AwaitTriggerToolStripButton.Name = "_AwaitTriggerToolStripButton"
        Me._AwaitTriggerToolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._AwaitTriggerToolStripButton.Size = New System.Drawing.Size(34, 22)
        Me._AwaitTriggerToolStripButton.Text = "Arm"
        Me._AwaitTriggerToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._AwaitTriggerToolStripButton.ToolTipText = "Depress to wait for trigger or release to abort."
        '
        '_WaitHourglassLabel
        '
        Me._WaitHourglassLabel.Name = "_WaitHourglassLabel"
        Me._WaitHourglassLabel.Size = New System.Drawing.Size(20, 22)
        Me._WaitHourglassLabel.Text = "[-]"
        Me._WaitHourglassLabel.ToolTipText = "Waiting for trigger"
        '
        '_AssertTriggerToolStripButton
        '
        Me._AssertTriggerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._AssertTriggerToolStripButton.Image = CType(resources.GetObject("_AssertTriggerToolStripButton.Image"), System.Drawing.Image)
        Me._AssertTriggerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AssertTriggerToolStripButton.Margin = New System.Windows.Forms.Padding(6, 1, 0, 2)
        Me._AssertTriggerToolStripButton.Name = "_AssertTriggerToolStripButton"
        Me._AssertTriggerToolStripButton.Size = New System.Drawing.Size(38, 22)
        Me._AssertTriggerToolStripButton.Text = "*TRG"
        '
        '_TriggerActionToolStripLabel
        '
        Me._TriggerActionToolStripLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TriggerActionToolStripLabel.Name = "_TriggerActionToolStripLabel"
        Me._TriggerActionToolStripLabel.Size = New System.Drawing.Size(160, 22)
        Me._TriggerActionToolStripLabel.Text = "Click wait to 'Arm' for trigger"
        Me._TriggerActionToolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_BinningGroupBox
        '
        Me._BinningGroupBox.Controls.Add(Me._ArmComboBoxLabel)
        Me._BinningGroupBox.Controls.Add(Me._ArmSourceComboBox)
        Me._BinningGroupBox.Controls.Add(Me._ApplySotSettingsButton)
        Me._BinningGroupBox.Controls.Add(Me._ContactCheckSupportLabel)
        Me._BinningGroupBox.Controls.Add(Me._FailBitPatternNumeric)
        Me._BinningGroupBox.Controls.Add(Me._PassBitPatternNumeric)
        Me._BinningGroupBox.Controls.Add(Me._ContactCheckBitPatternNumeric)
        Me._BinningGroupBox.Controls.Add(Me._FailBitPatternNumericLabel)
        Me._BinningGroupBox.Controls.Add(Me._PassBitPatternNumericLabel)
        Me._BinningGroupBox.Controls.Add(Me._ContactCheckBitPatternNumericLabel)
        Me._BinningGroupBox.Controls.Add(Me._EotStrobeDurationNumericLabel)
        Me._BinningGroupBox.Controls.Add(Me._EotStrobeDurationNumeric)
        Me._BinningGroupBox.Location = New System.Drawing.Point(16, 21)
        Me._BinningGroupBox.Name = "_BinningGroupBox"
        Me._BinningGroupBox.Size = New System.Drawing.Size(323, 161)
        Me._BinningGroupBox.TabIndex = 2
        Me._BinningGroupBox.TabStop = False
        Me._BinningGroupBox.Text = "SOT, EOT, BINNING"
        '
        '_ArmComboBoxLabel
        '
        Me._ArmComboBoxLabel.AutoSize = True
        Me._ArmComboBoxLabel.Location = New System.Drawing.Point(73, 23)
        Me._ArmComboBoxLabel.Name = "_ArmComboBoxLabel"
        Me._ArmComboBoxLabel.Size = New System.Drawing.Size(91, 17)
        Me._ArmComboBoxLabel.TabIndex = 0
        Me._ArmComboBoxLabel.Text = "Trigger signal:"
        '
        '_ArmSourceComboBox
        '
        Me._ArmSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._ArmSourceComboBox.FormattingEnabled = True
        Me._ArmSourceComboBox.Location = New System.Drawing.Point(167, 19)
        Me._ArmSourceComboBox.Name = "_ArmSourceComboBox"
        Me._ArmSourceComboBox.Size = New System.Drawing.Size(149, 25)
        Me._ArmSourceComboBox.TabIndex = 1
        Me._TipsToolTip.SetToolTip(Me._ArmSourceComboBox, "Available trigger modes")
        '
        '_ApplySotSettingsButton
        '
        Me._ApplySotSettingsButton.Location = New System.Drawing.Point(253, 124)
        Me._ApplySotSettingsButton.Name = "_ApplySotSettingsButton"
        Me._ApplySotSettingsButton.Size = New System.Drawing.Size(57, 28)
        Me._ApplySotSettingsButton.TabIndex = 11
        Me._ApplySotSettingsButton.Text = "Apply"
        Me._ApplySotSettingsButton.UseVisualStyleBackColor = True
        '
        '_ContactCheckSupportLabel
        '
        Me._ContactCheckSupportLabel.AutoSize = True
        Me._ContactCheckSupportLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ContactCheckSupportLabel.Location = New System.Drawing.Point(3, 141)
        Me._ContactCheckSupportLabel.Name = "_ContactCheckSupportLabel"
        Me._ContactCheckSupportLabel.Size = New System.Drawing.Size(0, 17)
        Me._ContactCheckSupportLabel.TabIndex = 9
        Me._ContactCheckSupportLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_FailBitPatternNumeric
        '
        Me._FailBitPatternNumeric.Location = New System.Drawing.Point(167, 100)
        Me._FailBitPatternNumeric.Maximum = New Decimal(New Integer() {7, 0, 0, 0})
        Me._FailBitPatternNumeric.Name = "_FailBitPatternNumeric"
        Me._FailBitPatternNumeric.Size = New System.Drawing.Size(35, 25)
        Me._FailBitPatternNumeric.TabIndex = 7
        Me._FailBitPatternNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._FailBitPatternNumeric.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        '_PassBitPatternNumeric
        '
        Me._PassBitPatternNumeric.Location = New System.Drawing.Point(167, 73)
        Me._PassBitPatternNumeric.Maximum = New Decimal(New Integer() {7, 0, 0, 0})
        Me._PassBitPatternNumeric.Name = "_PassBitPatternNumeric"
        Me._PassBitPatternNumeric.Size = New System.Drawing.Size(35, 25)
        Me._PassBitPatternNumeric.TabIndex = 5
        Me._PassBitPatternNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._PassBitPatternNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_ContactCheckBitPatternNumeric
        '
        Me._ContactCheckBitPatternNumeric.Location = New System.Drawing.Point(167, 127)
        Me._ContactCheckBitPatternNumeric.Maximum = New Decimal(New Integer() {7, 0, 0, 0})
        Me._ContactCheckBitPatternNumeric.Name = "_ContactCheckBitPatternNumeric"
        Me._ContactCheckBitPatternNumeric.Size = New System.Drawing.Size(35, 25)
        Me._ContactCheckBitPatternNumeric.TabIndex = 9
        Me._ContactCheckBitPatternNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ContactCheckBitPatternNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        '_FailBitPatternNumericLabel
        '
        Me._FailBitPatternNumericLabel.AutoSize = True
        Me._FailBitPatternNumericLabel.Location = New System.Drawing.Point(69, 104)
        Me._FailBitPatternNumericLabel.Name = "_FailBitPatternNumericLabel"
        Me._FailBitPatternNumericLabel.Size = New System.Drawing.Size(95, 17)
        Me._FailBitPatternNumericLabel.TabIndex = 6
        Me._FailBitPatternNumericLabel.Text = "Fail bit pattern:"
        Me._FailBitPatternNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_PassBitPatternNumericLabel
        '
        Me._PassBitPatternNumericLabel.AutoSize = True
        Me._PassBitPatternNumericLabel.Location = New System.Drawing.Point(62, 77)
        Me._PassBitPatternNumericLabel.Name = "_PassBitPatternNumericLabel"
        Me._PassBitPatternNumericLabel.Size = New System.Drawing.Size(102, 17)
        Me._PassBitPatternNumericLabel.TabIndex = 4
        Me._PassBitPatternNumericLabel.Text = "Pass bit pattern:"
        Me._PassBitPatternNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ContactCheckBitPatternNumericLabel
        '
        Me._ContactCheckBitPatternNumericLabel.AutoSize = True
        Me._ContactCheckBitPatternNumericLabel.Location = New System.Drawing.Point(8, 131)
        Me._ContactCheckBitPatternNumericLabel.Name = "_ContactCheckBitPatternNumericLabel"
        Me._ContactCheckBitPatternNumericLabel.Size = New System.Drawing.Size(156, 17)
        Me._ContactCheckBitPatternNumericLabel.TabIndex = 8
        Me._ContactCheckBitPatternNumericLabel.Text = "Contact check bit pattern:"
        Me._ContactCheckBitPatternNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_EotStrobeDurationNumericLabel
        '
        Me._EotStrobeDurationNumericLabel.AutoSize = True
        Me._EotStrobeDurationNumericLabel.Location = New System.Drawing.Point(4, 50)
        Me._EotStrobeDurationNumericLabel.Name = "_EotStrobeDurationNumericLabel"
        Me._EotStrobeDurationNumericLabel.Size = New System.Drawing.Size(160, 17)
        Me._EotStrobeDurationNumericLabel.TabIndex = 2
        Me._EotStrobeDurationNumericLabel.Text = "EOT Strobe duration [mS]:"
        '
        '_EotStrobeDurationNumeric
        '
        Me._EotStrobeDurationNumeric.DecimalPlaces = 3
        Me._EotStrobeDurationNumeric.Location = New System.Drawing.Point(167, 46)
        Me._EotStrobeDurationNumeric.Name = "_EotStrobeDurationNumeric"
        Me._EotStrobeDurationNumeric.Size = New System.Drawing.Size(71, 25)
        Me._EotStrobeDurationNumeric.TabIndex = 3
        Me._EotStrobeDurationNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._EotStrobeDurationNumeric.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 248)
        Me._ReadWriteTabPage.TabIndex = 5
        Me._ReadWriteTabPage.Text = "R/W"
        Me._ReadWriteTabPage.UseVisualStyleBackColor = True
        '
        '_SimpleReadWriteControl
        '
        Me._SimpleReadWriteControl.AutoAppendTermination = Nothing
        Me._SimpleReadWriteControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SimpleReadWriteControl.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SimpleReadWriteControl.Location = New System.Drawing.Point(0, 0)
        Me._SimpleReadWriteControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SimpleReadWriteControl.Name = "_SimpleReadWriteControl"
        Me._SimpleReadWriteControl.ReadEnabled = False
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 248)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 248)
        Me._MessagesTabPage.TabIndex = 3
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_TraceMessagesBox
        '
        Me._TraceMessagesBox.AlertLevel = System.Diagnostics.TraceEventType.Warning
        Me._TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info
        Me._TraceMessagesBox.CaptionFormat = "{0} ≡"
        Me._TraceMessagesBox.CausesValidation = False
        Me._TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TraceMessagesBox.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TraceMessagesBox.Location = New System.Drawing.Point(0, 0)
        Me._TraceMessagesBox.Multiline = True
        Me._TraceMessagesBox.Name = "_TraceMessagesBox"
        Me._TraceMessagesBox.PresetCount = 500
        Me._TraceMessagesBox.ReadOnly = True
        Me._TraceMessagesBox.ResetCount = 1000
        Me._TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._TraceMessagesBox.Size = New System.Drawing.Size(356, 248)
        Me._TraceMessagesBox.TabIndex = 1
        '
        '_LastErrorTextBox
        '
        Me._LastErrorTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastErrorTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastErrorTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._LastErrorTextBox.ForeColor = System.Drawing.Color.OrangeRed
        Me._LastErrorTextBox.Location = New System.Drawing.Point(0, 53)
        Me._LastErrorTextBox.Name = "_LastErrorTextBox"
        Me._LastErrorTextBox.Size = New System.Drawing.Size(364, 18)
        Me._LastErrorTextBox.TabIndex = 2
        Me._LastErrorTextBox.Text = "000, No Errors"
        '
        '_ReadingStatusStrip
        '
        Me._ReadingStatusStrip.BackColor = System.Drawing.Color.Black
        Me._ReadingStatusStrip.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingStatusStrip.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadingStatusStrip.GripMargin = New System.Windows.Forms.Padding(0)
        Me._ReadingStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._FailureToolStripStatusLabel, Me._ReadingToolStripStatusLabel, Me._StatusRegisterLabel, Me._StandardRegisterLabel, Me._TimingLabel})
        Me._ReadingStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingStatusStrip.Name = "_ReadingStatusStrip"
        Me._ReadingStatusStrip.Size = New System.Drawing.Size(364, 37)
        Me._ReadingStatusStrip.SizingGrip = False
        Me._ReadingStatusStrip.TabIndex = 1
        '
        '_FailureToolStripStatusLabel
        '
        Me._FailureToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._FailureToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FailureToolStripStatusLabel.ForeColor = System.Drawing.Color.Red
        Me._FailureToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._FailureToolStripStatusLabel.Name = "_FailureToolStripStatusLabel"
        Me._FailureToolStripStatusLabel.Size = New System.Drawing.Size(16, 37)
        Me._FailureToolStripStatusLabel.Text = "C"
        Me._FailureToolStripStatusLabel.ToolTipText = "Compliance"
        '
        '_ReadingToolStripStatusLabel
        '
        Me._ReadingToolStripStatusLabel.AutoSize = False
        Me._ReadingToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadingToolStripStatusLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._ReadingToolStripStatusLabel.Name = "_ReadingToolStripStatusLabel"
        Me._ReadingToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._ReadingToolStripStatusLabel.Size = New System.Drawing.Size(261, 37)
        Me._ReadingToolStripStatusLabel.Spring = True
        Me._ReadingToolStripStatusLabel.Text = "0.0000000 mV"
        Me._ReadingToolStripStatusLabel.ToolTipText = "Reading"
        '
        '_StatusRegisterLabel
        '
        Me._StatusRegisterLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StatusRegisterLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusRegisterLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._StatusRegisterLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusRegisterLabel.Name = "_StatusRegisterLabel"
        Me._StatusRegisterLabel.Size = New System.Drawing.Size(36, 37)
        Me._StatusRegisterLabel.Text = "0x00"
        Me._StatusRegisterLabel.ToolTipText = "Status Register Value"
        '
        '_StandardRegisterLabel
        '
        Me._StandardRegisterLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StandardRegisterLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StandardRegisterLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._StandardRegisterLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._StandardRegisterLabel.Name = "_StandardRegisterLabel"
        Me._StandardRegisterLabel.Size = New System.Drawing.Size(36, 37)
        Me._StandardRegisterLabel.Text = "0x00"
        Me._StandardRegisterLabel.ToolTipText = "Standard Register Value"
        '
        '_TimingLabel
        '
        Me._TimingLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TimingLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TimingLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._TimingLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._TimingLabel.Name = "_TimingLabel"
        Me._TimingLabel.Size = New System.Drawing.Size(0, 0)
        Me._TimingLabel.ToolTipText = "Elapsed time, ms"
        '
        '_StatusStrip
        '
        Me._StatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusLabel, Me._IdentityLabel})
        Me._StatusStrip.Location = New System.Drawing.Point(0, 411)
        Me._StatusStrip.Name = "_StatusStrip"
        Me._StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me._StatusStrip.ShowItemToolTips = True
        Me._StatusStrip.Size = New System.Drawing.Size(364, 22)
        Me._StatusStrip.TabIndex = 15
        Me._StatusStrip.Text = "StatusStrip1"
        '
        '_StatusLabel
        '
        Me._StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._StatusLabel.Size = New System.Drawing.Size(317, 17)
        Me._StatusLabel.Spring = True
        Me._StatusLabel.Text = "<Status>"
        Me._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._StatusLabel.ToolTipText = "Status"
        '
        '_IdentityLabel
        '
        Me._IdentityLabel.Name = "_IdentityLabel"
        Me._IdentityLabel.Size = New System.Drawing.Size(30, 17)
        Me._IdentityLabel.Text = "<I>"
        Me._IdentityLabel.ToolTipText = "Identity"
        '
        '_InfoProvider
        '
        Me._InfoProvider.ContainerControl = Me
        '
        '_LastReadingTextBox
        '
        Me._LastReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._LastReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._LastReadingTextBox.Location = New System.Drawing.Point(0, 37)
        Me._LastReadingTextBox.Name = "_LastReadingTextBox"
        Me._LastReadingTextBox.Size = New System.Drawing.Size(364, 16)
        Me._LastReadingTextBox.TabIndex = 0
        Me._LastReadingTextBox.Text = "<last reading>"
        Me._LastReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_Panel
        '
        Me._Panel.Controls.Add(Me._Tabs)
        Me._Panel.Controls.Add(Me._LastErrorTextBox)
        Me._Panel.Controls.Add(Me._LastReadingTextBox)
        Me._Panel.Controls.Add(Me._ReadingStatusStrip)
        Me._Panel.Controls.Add(Me._StatusStrip)
        Me._Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Panel.Location = New System.Drawing.Point(0, 17)
        Me._Panel.Margin = New System.Windows.Forms.Padding(0)
        Me._Panel.Name = "_Panel"
        Me._Panel.Size = New System.Drawing.Size(364, 433)
        Me._Panel.TabIndex = 16
        '
        '_Layout
        '
        Me._Layout.ColumnCount = 1
        Me._Layout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._Layout.Controls.Add(Me._TitleLabel, 0, 0)
        Me._Layout.Controls.Add(Me._Panel, 0, 1)
        Me._Layout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Layout.Location = New System.Drawing.Point(0, 0)
        Me._Layout.Margin = New System.Windows.Forms.Padding(0)
        Me._Layout.Name = "_Layout"
        Me._Layout.RowCount = 2
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._Layout.Size = New System.Drawing.Size(364, 450)
        Me._Layout.TabIndex = 17
        '
        '_TitleLabel
        '
        Me._TitleLabel.BackColor = System.Drawing.Color.Black
        Me._TitleLabel.CausesValidation = False
        Me._TitleLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me._TitleLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TitleLabel.ForeColor = System.Drawing.SystemColors.Info
        Me._TitleLabel.Location = New System.Drawing.Point(0, 0)
        Me._TitleLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._TitleLabel.Name = "_TitleLabel"
        Me._TitleLabel.Size = New System.Drawing.Size(364, 17)
        Me._TitleLabel.TabIndex = 17
        Me._TitleLabel.Text = "K2400"
        Me._TitleLabel.UseMnemonic = False
        '
        'K2400Control
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K2400Control"
        Me.Size = New System.Drawing.Size(364, 450)
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        CType(Me._ReadingsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ReadingToolStrip.ResumeLayout(False)
        Me._ReadingToolStrip.PerformLayout()
        Me._ToolStripPanel.ResumeLayout(False)
        Me._ToolStripPanel.PerformLayout()
        Me._SystemToolStrip.ResumeLayout(False)
        Me._SystemToolStrip.PerformLayout()
        Me._SourceTabPage.ResumeLayout(False)
        Me._SourceTabPage.PerformLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SourceLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SourceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SourceRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._NplcNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._HipotTabPage.ResumeLayout(False)
        Me._HipotLayout.ResumeLayout(False)
        Me._HipotGroupBox.ResumeLayout(False)
        Me._HipotGroupBox.PerformLayout()
        CType(Me._CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._VoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ResistanceRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ResistanceLowLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._DwellTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ApertureNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SotTabPage.ResumeLayout(False)
        Me._BinningLayout.ResumeLayout(False)
        Me._BinningLayout.PerformLayout()
        Me._TriggerToolStrip.ResumeLayout(False)
        Me._TriggerToolStrip.PerformLayout()
        Me._BinningGroupBox.ResumeLayout(False)
        Me._BinningGroupBox.PerformLayout()
        CType(Me._FailBitPatternNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PassBitPatternNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ContactCheckBitPatternNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._EotStrobeDurationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._ReadingStatusStrip.ResumeLayout(False)
        Me._ReadingStatusStrip.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        CType(Me._InfoProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Panel.ResumeLayout(False)
        Me._Panel.PerformLayout()
        Me._Layout.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SotTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SourceTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseAutoRangeToggle As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _NplcNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _FailureToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _SenseRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _NplcNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _SourceDelayTextBoxLabel As Windows.Forms.Label
    Private WithEvents _ApplySourceSettingButton As Windows.Forms.Button
    Private WithEvents _SourceRangeNumericLabel As Windows.Forms.Label
    Private WithEvents _SourceFunctionComboBox As Windows.Forms.ComboBox
    Private WithEvents _SourceLimitNumericLabel As Windows.Forms.Label
    Private WithEvents _SourceFunctionComboBoxLabel As Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _ToolStripPanel As Windows.Forms.ToolStripPanel
    Private WithEvents _SystemToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ClearInterfaceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ServiceRequestHandlersEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _TraceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DisplayTraceLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _LogTraceLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionNotificationLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionNotificationLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _SessionServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DeviceServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearDeviceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadStatusByteMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetSplitButton As Windows.Forms.ToolStripDropDownButton
    Private WithEvents _HipotTabPage As Windows.Forms.TabPage
    Private WithEvents _HipotLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _HipotGroupBox As Windows.Forms.GroupBox
    Private WithEvents _CurrentLimitNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _CurrentLimitNumericLabel As Windows.Forms.Label
    Private WithEvents _VoltageLevelNumericLabel As Windows.Forms.Label
    Private WithEvents _VoltageLevelNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ResistanceRangeNumericLabel As Windows.Forms.Label
    Private WithEvents _ResistanceLowLimitNumericLabel As Windows.Forms.Label
    Private WithEvents _ResistanceRangeNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ResistanceLowLimitNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _DwellTimeNumericLabel As Windows.Forms.Label
    Private WithEvents _DwellTimeNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ApertureNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ApertureNumericLabel As Windows.Forms.Label
    Private WithEvents _BinningLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _BinningGroupBox As Windows.Forms.GroupBox
    Private WithEvents _ContactCheckSupportLabel As Windows.Forms.Label
    Private WithEvents _FailBitPatternNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _PassBitPatternNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ContactCheckBitPatternNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _FailBitPatternNumericLabel As Windows.Forms.Label
    Private WithEvents _ContactCheckBitPatternNumericLabel As Windows.Forms.Label
    Private WithEvents _EotStrobeDurationNumericLabel As Windows.Forms.Label
    Private WithEvents _EotStrobeDurationNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _TriggerToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _AwaitTriggerToolStripButton As Windows.Forms.ToolStripButton
    Private WithEvents _WaitHourglassLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _AssertTriggerToolStripButton As Windows.Forms.ToolStripButton
    Private WithEvents _TriggerActionToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ApplyHipotSettingsButton As Windows.Forms.Button
    Private WithEvents _ApplySotSettingsButton As Windows.Forms.Button
    Private WithEvents _ArmSourceComboBox As Windows.Forms.ComboBox
    Private WithEvents _PassBitPatternNumericLabel As Windows.Forms.Label
    Private WithEvents _ArmComboBoxLabel As Windows.Forms.Label
    Private WithEvents _FourWireSenseCheckBox As Windows.Forms.CheckBox
    Private WithEvents _ConcurrentSenseCheckBox As Windows.Forms.CheckBox
    Private WithEvents _SourceLimitNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _SourceRangeNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _SourceDelayNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ApplySenseFunctionButton As Windows.Forms.Button
    Private WithEvents _SourceLevelNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _SourceLevelNumericLabel As Windows.Forms.Label
    Private WithEvents _ApplySourceFunctionButton As Windows.Forms.Button
    Private WithEvents _OutputDropDownButton As Windows.Forms.ToolStripDropDownButton
    Private WithEvents _ContactCheckEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SourceAutoClearEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _OutputTerminalMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ContactCheckToggle As Windows.Forms.CheckBox
    Private WithEvents _EnabledSenseFunctionsListBoxLabel As Windows.Forms.Label
    Private WithEvents _EnabledSenseFunctionsListBox As Windows.Forms.CheckedListBox
    Private WithEvents _TriggerDelayNumericLabel As Windows.Forms.Label
    Private WithEvents _TriggerDelayNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ClearExecutionStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ServiceRequestEnableBitmaskNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ServiceRequestEnableBitmaskNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ReadingsDataGridView As Windows.Forms.DataGridView
    Private WithEvents _ReadingToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ReadButton As Windows.Forms.ToolStripButton
    Private WithEvents _InitiateButton As Windows.Forms.ToolStripButton
    Private WithEvents _TraceButton As Windows.Forms.ToolStripButton
    Private WithEvents _ReadingsCountLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ReadingComboBox As Windows.Forms.ToolStripComboBox
    Private WithEvents _AbortButton As Windows.Forms.ToolStripButton
    Private WithEvents _ClearBufferDisplayButton As Windows.Forms.ToolStripButton
    Private WithEvents _LogTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _DisplayTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _ResourceSelectorConnector As VI.Instrument.ResourceSelectorConnector
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _InfoProvider As isr.Core.Controls.InfoProvider
    Private WithEvents _TipsToolTip As Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _IdentityLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StatusRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StandardRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TimingLabel As System.Windows.Forms.ToolStripStatusLabel
End Class
