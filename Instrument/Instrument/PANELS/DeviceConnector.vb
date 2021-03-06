﻿Imports isr.Core.Controls.ControlExtensions
Imports isr.VI.ExceptionExtensions

''' <summary> A device connector. A model for the resource connector viewer. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="4/19/2018" by="David" revision=""> Created. </history>
Public Class DeviceConnector
    Inherits Core.Pith.PropertyPublisherTalkerBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New(ByVal device As VI.DeviceBase, ByVal isDeviceOwner As Boolean)
        Me.New(device, isDeviceOwner, ResourceSelectorConnector.Create, True)
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="connector"> The connector. </param>
    Public Sub New(ByVal device As VI.DeviceBase, ByVal isDeviceOwner As Boolean, ByVal connector As ResourceSelectorConnector, ByVal isConnectorOwner As Boolean)
        MyBase.New()
        Me.AssignConnector(connector, isConnectorOwner)
        Me.AssignDevice(device, isDeviceOwner)
        Me.Initialize()
    End Sub

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(ResourceSelectorConnector.Create, True)
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="connector"> The connector. </param>
    Public Sub New(ByVal connector As ResourceSelectorConnector, ByVal isConnectorOwner As Boolean)
        MyBase.New()
        Me.AssignConnector(connector, isConnectorOwner)
        Me.Initialize()
    End Sub

    ''' <summary> Creates a new DeviceConnector. </summary>
    ''' <param name="device">        The device. </param>
    ''' <param name="isDeviceOwner"> The is device owner. </param>
    ''' <returns> A DeviceConnector. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Shared Function Create(ByVal device As VI.DeviceBase, ByVal isDeviceOwner As Boolean) As DeviceConnector
        Dim result As DeviceConnector = Nothing
        Try
            result = New DeviceConnector(device, isDeviceOwner)
        Catch ex As Exception
            If result IsNot Nothing Then
                result.Dispose()
                result = Nothing
            End If
        End Try
        Return result
    End Function

    ''' <summary> Initializes this object. </summary>
    Private Sub Initialize()
        Me._ElapsedTimeStopwatch = New Stopwatch
        Me._DefaultOpenResourceTitleFormat = VI.Pith.My.MySettings.Default.DefaultOpenResourceTitleFormat
        Me._OpenResourceTitleFormat = Me._DefaultOpenResourceTitleFormat
        Me._DefaultClosedResourceTitleFormat = VI.Pith.My.MySettings.Default.DefaultClosedResourceTitleFormat
        Me._ClosedResourceTitleFormat = Me.DefaultClosedResourceTitleFormat
        Me._DefaultResourceTitle = "<closed>"
        Me._DefaultResourceTitle = VI.Pith.My.MySettings.Default.DefaultResourceTitle
        Me._Status = "Find and select a resource."
        Me._Identity = ""
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.DeviceBase?.IsDeviceOpen Then
                    Try
                        If Me.IsDeviceOwner Then
                            ' if the device owner, then close the session
                            Me.DeviceBase.CloseSession()
                        Else
                            ' if not device owner, just release the event handlers associated with this panel. 
                            Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                        End If
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                If Me.DeviceBase IsNot Nothing Then
                    Try
                        Me.ReleaseDevice()
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred releasing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                Me._ElapsedTimeStopwatch = Nothing
                Me.AssignConnector(Nothing, True)
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
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

#Region " STATUS REPORTING "

    Private _Status As String
    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    Public Property Status As String
        Get
            Return Me._Status
        End Get
        Set(value As String)
            If Not String.Equals(Me.Status, value) Then
                Me._Status = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _Identity As String
    ''' <summary> Gets or sets the Identity. </summary>
    ''' <value> The Identity. </value>
    Public Property Identity As String
        Get
            Return Me._Identity
        End Get
        Set(value As String)
            If Not String.Equals(Me.Identity, value) Then
                Me._Identity = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The unknown register value. </summary>
    Public Shared Property UnknownRegisterValue As Integer = -1

    ''' <summary> The unknown register value caption. </summary>
    Public Shared Property UnknownRegisterValueCaption As String = "0x.."

    ''' <summary> The default register value format. </summary>
    Public Shared Property DefaultRegisterValueFormat As String = "0x{0:X2}"

    Private _StatusRegisterCaption As String
    ''' <summary> Gets or sets the StatusRegisterCaption. </summary>
    ''' <value> The StatusRegisterCaption. </value>
    Public Property StatusRegisterCaption As String
        Get
            Return Me._StatusRegisterCaption
        End Get
        Set(value As String)
            If Not String.Equals(Me.StatusRegisterCaption, value) Then
                Me._StatusRegisterCaption = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StatusRegisterStatus As Integer

    ''' <summary> Gets or sets the status register status. </summary>
    ''' <value> The status register status. </value>
    Public Property StatusRegisterStatus As Integer
        Get
            Return Me._StatusRegisterStatus
        End Get
        Set(value As Integer)
            If value <> Me.StatusRegisterStatus Then
                Me._StatusRegisterStatus = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Displays the status register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStatusRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StatusRegisterStatus) Then
            Me._StatusRegisterStatus = value
            Me.StatusRegisterCaption = String.Format(format, value)
        End If
    End Sub

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overridable Sub DisplayStatusRegisterStatus(ByVal value As Integer)
        If value = DeviceConnector.UnknownRegisterValue Then
            Me.StatusRegisterCaption = DeviceConnector.UnknownRegisterValueCaption
        Else
            Me.DisplayStatusRegisterStatus(value, DeviceConnector.DefaultRegisterValueFormat)
        End If
    End Sub

    Private _StandardRegisterCaption As String
    ''' <summary> Gets or sets the StandardRegisterCaption. </summary>
    ''' <value> The StandardRegisterCaption. </value>
    Public Property StandardRegisterCaption As String
        Get
            Return Me._StandardRegisterCaption
        End Get
        Set(value As String)
            If Not String.Equals(Me.StandardRegisterCaption, value) Then
                Me._StandardRegisterCaption = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StandardRegisterStatus As Integer

    ''' <summary> Gets or sets the Standard register status. </summary>
    ''' <value> The Standard register status. </value>
    Public Property StandardRegisterStatus As Integer
        Get
            Return Me._StandardRegisterStatus
        End Get
        Set(value As Integer)
            If value <> Me.StandardRegisterStatus Then
                Me._StandardRegisterStatus = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Displays the Standard register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStandardRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StandardRegisterStatus) Then
            Me._StandardRegisterStatus = value
            Me.StandardRegisterCaption = String.Format(format, value)
        End If
    End Sub

    ''' <summary> Displays the Standard register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overridable Sub DisplayStandardRegisterStatus(ByVal value As Integer)
        If value = DeviceConnector.UnknownRegisterValue Then
            Me.StandardRegisterCaption = DeviceConnector.UnknownRegisterValueCaption
        Else
            Me.DisplayStandardRegisterStatus(value, DeviceConnector.DefaultRegisterValueFormat)
        End If
    End Sub

    ''' <summary> The unknown register value caption. </summary>
    Public Shared Property UnknownRegisterBitmaskCaption As String = "0b.."

    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The standard register caption. </value>
    Public Overridable Property ServiceRequestEnableBitmaskCaption As String

    Private _ServiceRequestEnableBitmask As Integer
    ''' <summary> Gets or sets the standard register status. </summary>
    ''' <value> The standard register status. </value>
    Public Overridable Property ServiceRequestEnableBitmask As Integer
        Get
            Return Me._ServiceRequestEnableBitmask
        End Get
        Set(value As Integer)
            If value <> Me.ServiceRequestEnableBitmask Then
                Me._ServiceRequestEnableBitmask = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overridable Sub DisplayServiceRequestEnableBitmask(ByVal value As Integer)
        If value = DeviceConnector.UnknownRegisterValue Then
            Me.ServiceRequestEnableBitmaskCaption = DeviceConnector.UnknownRegisterBitmaskCaption
        Else
            Me.ServiceRequestEnableBitmaskCaption = $"0b{Convert.ToString(value, 2),8}".Replace(" ", "0")
        End If
    End Sub

#End Region

#Region " RESOURCE NAME "

    ''' <summary> Attempts to select resource name from the given data. </summary>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <param name="e">            Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TrySelectResourceName(ByVal resourceName As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"trying to select resource '{resourceName}'"
        Try
            If String.IsNullOrWhiteSpace(resourceName) Then
                e.RegisterCancellation($"{activity} canceled because name is empty")
            ElseIf Me.Connector Is Nothing Then
                e.RegisterCancellation($"{activity} canceled because the connector was disposed")
            Else
                activity = $"displaying resource names"
                Me.SessionFactory.EnumerateResources()
                activity = $"trying to select resource '{resourceName}'"
                If Me.SessionFactory.TrySelectResource(resourceName, e) Then
                End If
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Return Not e.Cancel
    End Function

    Private _DefaultOpenResourceTitleFormat As String
    ''' <summary> Gets or sets the default Open resource title format. </summary>
    ''' <value> The default Open resource title format. </value>
    Public Property DefaultOpenResourceTitleFormat As String
        Get
            Return Me._DefaultOpenResourceTitleFormat
        End Get
        Set(value As String)
            Me._DefaultOpenResourceTitleFormat = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    Private _OpenResourceTitleFormat As String

    ''' <summary> Gets or sets the open resource title format. </summary>
    ''' <value> The open resource title format. </value>
    Public Property OpenResourceTitleFormat As String
        Get
            Return Me._OpenResourceTitleFormat
        End Get
        Set(value As String)
            Me._OpenResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
            Me.Title = Me.BuildTitle
        End Set
    End Property

    Private _DefaultClosedResourceTitleFormat As String
    ''' <summary> Gets or sets the default closed resource title format. </summary>
    ''' <value> The default closed resource title format. </value>
    Public Property DefaultClosedResourceTitleFormat As String
        Get
            Return Me._DefaultClosedResourceTitleFormat
        End Get
        Set(value As String)
            Me._DefaultClosedResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    Private _ClosedResourceTitleFormat As String
    ''' <summary> Gets or sets the closed resource title format. </summary>
    ''' <value> The title. </value>
    Public Property ClosedResourceTitleFormat As String
        Get
            Return Me._ClosedResourceTitleFormat
        End Get
        Set(value As String)
            Me._ClosedResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
            Me.Title = Me.BuildTitle
        End Set
    End Property

    Private _DefaultResourceTitle As String
    ''' <summary> Gets or sets the default resource title. </summary>
    ''' <value> The default resource title. </value>
    Public Property DefaultResourceTitle As String
        Get
            Return Me._DefaultResourceTitle
        End Get
        Set(value As String)
            Me._DefaultResourceTitle = value
            Me.SafeSendPropertyChanged()
            Me.Title = Me.BuildTitle
        End Set
    End Property

    Private _Title As String
    ''' <summary> Gets or sets the Title. </summary>
    ''' <value> The Title. </value>
    Public Property Title As String
        Get
            Return Me._Title
        End Get
        Set(value As String)
            If Not String.Equals(Me.Title, value) Then
                Me._Title = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Builds the title. </summary>
    ''' <returns> A String. </returns>
    Public Function BuildTitle() As String
        Dim result As String = ""
        If Me.IsDeviceOpen Then
            If String.IsNullOrWhiteSpace(Me.OpenResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle) Then
                result = ""
            Else
                result = String.Format(Me.OpenResourceTitleFormat, Me.DeviceBase.ResourceTitle, Me.DeviceBase.Session.ResourceName)
            End If
        Else
            If String.IsNullOrWhiteSpace(Me.ClosedResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle) Then
                result = ""
            Else
                result = String.Format(Me.ClosedResourceTitleFormat, Me.DeviceBase.ResourceTitle)
            End If
        End If
        Return result
    End Function

#End Region

#Region " STOPWATCH "

    ''' <summary> Gets or sets the elapsed time stop watch. </summary>
    ''' <value> The elapsed time stop watch. </value>
    Private ReadOnly Property ElapsedTimeStopwatch As Stopwatch

    ''' <summary> Reads elapsed time. </summary>
    ''' <param name="stopRequested"> True if stop requested. </param>
    ''' <returns> The elapsed time. </returns>
    Public Function ReadElapsedTime(ByVal stopRequested As Boolean) As TimeSpan
        If stopRequested AndAlso Me.ElapsedTimeStopwatch.IsRunning Then
            Me._ElapsedTimeCount -= 1
            If Me.ElapsedTimeCount <= 0 Then Me.ElapsedTimeStopwatch.Stop()
        End If
        Return Me.ElapsedTimeStopwatch.Elapsed
    End Function

    Private _ElapsedTimeCount As Integer
    ''' <summary> Gets the number of elapsed times. Some action require two cycles to get the full elapsed time. </summary>
    ''' <value> The number of elapsed times. </value>
    Public Property ElapsedTimeCount As Integer
        Get
            Return Me._ElapsedTimeCount
        End Get
        Set(value As Integer)
            If value <> Me.ElapsedTimeCount Then
                Me._ElapsedTimeCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Starts elapsed stopwatch. </summary>
    Public Sub StartElapsedStopwatch(ByVal count As Integer)
        Me._ElapsedTimeCount = count
        Me.ElapsedTimeStopwatch.Restart()
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Gets or sets the sentinel indicating if this panel owns the device and, therefore, needs to 
    '''           dispose of this device. </summary>
    ''' <value> The is device owner. </value>
    Public ReadOnly Property IsDeviceOwner As Boolean

    ''' <summary> Gets the is device assigned. </summary>
    ''' <value> The is device assigned. </value>
    Public Overridable ReadOnly Property IsDeviceAssigned As Boolean
        Get
            Return Me.DeviceBase IsNot Nothing AndAlso Not Me.DeviceBase.IsDisposed
        End Get
    End Property

    Private _DeviceBase As VI.DeviceBase
    ''' <summary> Gets or sets reference to the VISA <see cref="VI.DeviceBase">device</see>
    ''' interfaces. </summary>
    ''' <value> The connectable resource. </value>
    Public ReadOnly Property DeviceBase() As VI.DeviceBase
        Get
            Return Me._DeviceBase
        End Get
    End Property

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Sub AssignDevice(ByVal value As VI.DeviceBase, ByVal isDeviceOwner As Boolean)
        If Me.DeviceBase IsNot Nothing Then
            If Me.DeviceBase.Talker IsNot Nothing Then Me.DeviceBase.Talker.RemoveListeners()
            RemoveHandler Me.DeviceBase.Session.ResourceNameInfo.PropertyChanged, AddressOf Me.ResourceNameInfoPropertyChanged
            RemoveHandler Me.DeviceBase.PropertyChanged, AddressOf Me.DevicePropertyChanged
            RemoveHandler Me.DeviceBase.Opening, AddressOf Me.DeviceOpening
            RemoveHandler Me.DeviceBase.Opened, AddressOf Me.DeviceOpened
            RemoveHandler Me.DeviceBase.Closing, AddressOf Me.DeviceClosing
            RemoveHandler Me.DeviceBase.Closed, AddressOf Me.DeviceClosed
            RemoveHandler Me.DeviceBase.Initialized, AddressOf Me.DeviceInitialized
            RemoveHandler Me.DeviceBase.Initializing, AddressOf Me.DeviceInitializing
            ' note that service request is released when device closes.
            Me.StatusSubsystem = Nothing
            ' this also closes the session. 
            If Me.IsDeviceOwner Then Me._DeviceBase.Dispose() : Me._DeviceBase = Nothing
        End If
#If False Then
        If Me.Connector IsNot Nothing Then
            Me.SessionFactory.ResourcesFilter = ""
            Me.Connector.Clearable = True
            Me.Connector.Connectable = True
            Me.Connector.Searchable = True
        End If
#End If
        Me._StatusRegisterStatus = DeviceConnector.UnknownRegisterValue
        Me._StandardRegisterStatus = DeviceConnector.UnknownRegisterValue
        Me._DeviceBase = value
        If value IsNot Nothing Then
            Me.DeviceBase.CaptureSyncContext(Threading.SynchronizationContext.Current)
            AddHandler Me.DeviceBase.Session.ResourceNameInfo.PropertyChanged, AddressOf Me.ResourceNameInfoPropertyChanged
            AddHandler Me.DeviceBase.PropertyChanged, AddressOf Me.DevicePropertyChanged
            AddHandler Me.DeviceBase.Opening, AddressOf Me.DeviceOpening
            AddHandler Me.DeviceBase.Opened, AddressOf Me.DeviceOpened
            AddHandler Me.DeviceBase.Closing, AddressOf Me.DeviceClosing
            AddHandler Me.DeviceBase.Closed, AddressOf Me.DeviceClosed
            AddHandler Me.DeviceBase.Initialized, AddressOf Me.DeviceInitialized
            AddHandler Me.DeviceBase.Initializing, AddressOf Me.DeviceInitializing
            Me._Connector.SessionFactory.ResourcesFilter = Me.DeviceBase.Session.ResourceNameInfo.ResourcesFilter
            Me._Connector.IsConnected = value.IsDeviceOpen
            Me.StatusSubsystem = value.StatusSubsystemBase
            ' publish device state.  This should also report the open status and set the 
            ' control open/close button state.
            Me.DeviceBase.Publish()
            Me._AssignTalker(value.Talker)
            Me._ApplyListenerTraceLevel(Core.Pith.ListenerType.Display, value.Talker.TraceShowLevel)
        End If
        Me._IsDeviceOwner = isDeviceOwner
    End Sub

    ''' <summary> Gets the sentinel indicating if the device is open. </summary>
    ''' <value> The is device open. </value>
    Public ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.DeviceBase IsNot Nothing AndAlso Me.DeviceBase.IsDeviceOpen
        End Get
    End Property

    ''' <summary> Releases the device. </summary>
    Public Overridable Sub ReleaseDevice()
        ' this also releases the device event handlers associated with this panel. 
        Me.AssignDevice(Nothing, False)
    End Sub

    ''' <summary> Releases the device and reassigns the default device. </summary>
    Public Overridable Sub RestoreDevice(ByVal value As VI.DeviceBase, ByVal isDeviceOwner As Boolean)
        ' release the device
        Me.ReleaseDevice()
        Me.AssignDevice(value, isDeviceOwner)
    End Sub

#End Region

#Region " DEVICE: SERVICE REQUESTED "

    ''' <summary> Event handler. Called when the device is requesting service. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Message based session event information. </param>
    Public Overridable Sub DeviceServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private _ServiceRequestHandlerAdded As Boolean
    ''' <summary> Gets the sentinel indicating if service requests handler was added for this connector. </summary>
    ''' <value> The service request event handler add sentinel. </value>
    Public Property ServiceRequestHandlerAdded As Boolean
        Get
            Return Me._ServiceRequestHandlerAdded
        End Get
        Set(value As Boolean)
            If Me.DeviceServiceRequestHandlerAdded <> value Then
                Me._ServiceRequestHandlerAdded = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _DeviceServiceRequestHandlerAdded As Boolean
    ''' <summary> Gets or sets the device service request enabled. </summary>
    ''' <value> The device service request enabled. </value>
    Public Property DeviceServiceRequestHandlerAdded As Boolean
        Get
            Return Me._DeviceServiceRequestHandlerAdded
        End Get
        Set(value As Boolean)
            If Me.DeviceServiceRequestHandlerAdded <> value Then
                Me._DeviceServiceRequestHandlerAdded = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Adds a panel service request handler. </summary>
    ''' <remarks>
    ''' The service request for the session must be enabled for handling service requests on the device. 
    ''' The panel can add the service request handler if: (1) is owner or; (2) if not owner but service request handler was not
    ''' added on the device. In case 2, the panel could also remove the device handler.
    ''' </remarks>
    Public Overridable Sub AddServiceRequestEventHandler()
        If Not Me.ServiceRequestHandlerAdded Then
            AddHandler Me.DeviceBase.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner Then
                Me.DeviceBase.AddServiceRequestEventHandler()
            ElseIf Not Me.DeviceBase.DeviceServiceRequestHandlerAdded Then
                Me.DeviceBase.AddServiceRequestEventHandler()
                Me.DeviceServiceRequestHandlerAdded = True
            End If
            Me.ServiceRequestHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the service request event. </summary>
    Public Overridable Sub RemoveServiceRequestEventHandler()
        If Me.ServiceRequestHandlerAdded Then
            RemoveHandler Me.DeviceBase.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner OrElse Me._DeviceServiceRequestHandlerAdded Then Me.DeviceBase.RemoveServiceRequestEventHandler()
            Me.ServiceRequestHandlerAdded = False
            Me.DeviceServiceRequestHandlerAdded = False
        End If
    End Sub

#End Region

#Region " DEVICE: PROPERTY CHANGE "

    ''' <summary> Recursively enable. </summary>
    ''' <param name="controls"> The controls. </param>
    ''' <param name="value">    The value. </param>
    Public Sub RecursivelyEnable(ByVal controls As System.Windows.Forms.Control.ControlCollection, ByVal value As Boolean)
        If controls IsNot Nothing Then
            For Each c As System.Windows.Forms.Control In controls
                If c IsNot Me.Connector Then
                    c.RecursivelyEnable(value)
                End If
            Next
        End If
    End Sub

    ''' <summary> Executes the device open changed action. 
    '''           The open event occurs after all subsystems are created. </summary>
    Public Overridable Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
        If Me.Connector IsNot Nothing Then Me.Connector.IsConnected = Me.IsDeviceOpen
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="resourceNameInfo"> The resource name info. </param>
    ''' <param name="propertyName">     Name of the property. </param>
    Public Overridable Overloads Sub HandlePropertyChange(ByVal resourceNameInfo As VI.Pith.ResourceNameInfo, ByVal propertyName As String)
        If resourceNameInfo Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.Pith.ResourceNameInfo.ResourcesFilter)
                If Connector IsNot Nothing Then Me.SessionFactory.ResourcesFilter = resourceNameInfo.ResourcesFilter
            Case NameOf(VI.Pith.ResourceNameInfo.ResourceTitle)
                Me.Title = Me.BuildTitle
            Case NameOf(VI.Pith.ResourceNameInfo.ResourceName)
                Me.Title = Me.BuildTitle
        End Select
    End Sub

    ''' <summary> Event handler. Called for property changed events. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ResourceNameInfoPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.ResourceNameInfo)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, VI.Pith.ResourceNameInfo), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub


    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Public Overridable Overloads Sub HandlePropertyChange(ByVal device As VI.DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.IsDeviceOpen)
                ' the open sentinel is turned on after all subsystems are set
                Me.OnDeviceOpenChanged(device)
            Case NameOf(isr.VI.DeviceBase.ResourceTitle)
                Me.Title = Me.BuildTitle()
            Case NameOf(isr.VI.DeviceBase.ServiceRequestEnableBitmask)
                Me.ServiceRequestEnableBitmask = device.ServiceRequestEnableBitmask
        End Select
        ' propagate the property change messages
        Me.SafeSendPropertyChanged(propertyName)
    End Sub

    ''' <summary> Event handler. Called for property changed events. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DevicePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.DeviceBase)}.{e?.PropertyName} change event"
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.HandlePropertyChange(TryCast(sender, VI.DeviceBase), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                ' used for figuring out what events are raised to cause this. 
                ' possibly the device is sending messages after the control is disposed indicating that the device needs to 
                ' be disposed before the control is disposed. 
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DEVICE: OPENING / OPEN ACTIONS AND EVENTS "

    ''' <summary> Attempts to open a session to the device using the specified resource name. </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The title. </param>
    ''' <param name="e">             Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Public Function TryOpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"[{resourceTitle}] trying to open {resourceName} VISA session"
        Try
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
            If Me.DeviceBase.TryOpenSession(resourceName, resourceTitle, e) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Done {activity};. ")
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. ")
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Connector.IsConnected = Me.IsDeviceOpen
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary>
    ''' Event handler. Called upon device opening so as to instantiated all subsystems.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub DeviceOpening(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Me._StatusRegisterStatus = DeviceConnector.UnknownRegisterValue
        Me._StandardRegisterStatus = DeviceConnector.UnknownRegisterValue
    End Sub

    ''' <summary> Event handler. Called upon device opening so as to instantiated all subsystems. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling  device opening event"
        Try
            Me.DeviceOpening(TryCast(sender, DeviceBase), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Event handler. Called after the device opened and all subsystems were defined.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub DeviceOpened(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
    End Sub

    ''' <summary>
    ''' Event handler. Called after the device opened and all subsystems were defined.
    ''' </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device opened event"
        Try
            Me.DeviceOpened(TryCast(sender, DeviceBase), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DEVICE: INITALIZING / INITIALIZED  "

    ''' <summary> Device initializing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub DeviceInitializing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
    End Sub

    ''' <summary> Device initializing. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceInitializing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device initializing event"
        Try
            Me.DeviceInitializing(TryCast(sender, DeviceBase), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub DeviceInitialized(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Me.ReadServiceRequestStatus()
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceInitialized(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device initialized event"
        Try
            Me.DeviceInitialized(TryCast(sender, DeviceBase), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try

    End Sub

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Dim activity As String = $"reading {Me.DeviceBase?.ResourceNameCaption} service request"
        Try
            Me.DeviceBase.StatusSubsystemBase.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. {0}", ex.ToFullBlownString)
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DEVICE: CLOSING / CLOSED "

    ''' <summary> Attempts to close session from the given data. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Public Function TryCloseSession(ByVal e As Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"closing VISA session {Me.DeviceBase?.ResourceNameCaption}"
        Try
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
            If Me.DeviceBase.IsDeviceOpen Then
                If Me.DeviceBase.TryCloseSession() Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity} processed;. ")
                Else
                    e.RegisterCancellation($"Failed {activity};. ")
                End If
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Connector.IsConnected = Me.IsDeviceOpen
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub DeviceClosing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.DeviceBase Is Nothing OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.RemoveServiceRequestEventHandler()
        Me.DeviceBase.Session?.DisableServiceRequest()
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.DeviceBase Is Nothing OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device closing event"
        Try
            Me.DeviceClosing(TryCast(sender, DeviceBase), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Public Overridable Sub DeviceClosed(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        If Me.DeviceBase Is Nothing OrElse Me.DeviceBase.Session Is Nothing OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim message As Core.Pith.TraceMessage = Nothing
        If Me.DeviceBase Is Nothing OrElse Me.DeviceBase.Session Is Nothing Then
            message = New Core.Pith.TraceMessage(TraceEventType.Information, My.MyLibrary.TraceEventId, "Disconnected; Device disposed;. ")
        ElseIf Me.DeviceBase.Session.IsSessionOpen Then
            message = New Core.Pith.TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{Me.DeviceBase.Session.ResourceName} closed but session still open;. ")
        ElseIf Me.DeviceBase.Session.IsDeviceOpen Then
            message = New Core.Pith.TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Device closed but emulated session still open;. ")
        Else
            message = New Core.Pith.TraceMessage(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Disconnected; Session closed;. ")
        End If
        If Me.Talker Is Nothing Then
            If message.EventType <= TraceEventType.Warning Then isr.Core.Pith.My.MyLibrary.LogUnpublishedMessage(message)
        Else
            Me.Talker.Publish(message)
        End If
    End Sub


    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim activity As String = "handling device closed event"
        Try
            Me.DeviceClosed(TryCast(sender, DeviceBase), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SESSION FACTORY "

    ''' <summary> Gets the session factory. </summary>
    ''' <value> The session factory. </value>
    Public ReadOnly Property SessionFactory As VI.SessionFactory
        Get
            Return Me.Connector.SessionFactory
        End Get
    End Property

#End Region

#Region " CONNECTOR SELECTOR EVENT HANDLERS "

    ''' <summary> Gets or sets the is connector that owns this item. </summary>
    ''' <value> The is connector owner. </value>
    Public Property IsConnectorOwner As Boolean

    Private _Connector As ResourceSelectorConnector

    ''' <summary> Gets or sets the connector. </summary>
    ''' <value> The connector. </value>
    Public Overridable ReadOnly Property Connector As ResourceSelectorConnector
        Get
            Return Me._Connector
        End Get
    End Property

    ''' <summary> Assign connector. </summary>
    ''' <param name="value"> The connector value. </param>
    Public Sub AssignConnector(ByVal value As ResourceSelectorConnector, ByVal isConnectorOwner As Boolean)
        If Me._Connector IsNot Nothing Then
            RemoveHandler Me._Connector.Connect, AddressOf Me.Connect
            RemoveHandler Me._Connector.Disconnect, AddressOf Me.Disconnect
            RemoveHandler Me._Connector.Clear, AddressOf Me.ClearDevice
            RemoveHandler Me._Connector.FindNames, AddressOf Me.FindResourceNames
            RemoveHandler Me._Connector.PropertyChanged, AddressOf Me.ConnectorPropertyChanged
            If Me.IsConnectorOwner Then Me._Connector.Dispose()
        End If
        Me._Connector = value
        If value IsNot Nothing Then
            AddHandler Me._Connector.Connect, AddressOf Me.Connect
            AddHandler Me._Connector.Disconnect, AddressOf Me.Disconnect
            AddHandler Me._Connector.Clear, AddressOf Me.ClearDevice
            AddHandler Me._Connector.FindNames, AddressOf Me.FindResourceNames
            AddHandler Me._Connector.PropertyChanged, AddressOf Me.ConnectorPropertyChanged
        End If
        Me.IsConnectorOwner = isConnectorOwner
    End Sub

    ''' <summary> Attempts to connect from the given data. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Private Function TryConnect(ByVal sender As ResourceSelectorConnector, ByVal e As Core.Pith.ActionEventArgs) As Boolean
        Dim activity As String = "handling device opened event"
        If String.IsNullOrWhiteSpace(Me.SessionFactory.SelectedResourceName) Then
            e.RegisterCancellation($"Failed {activity}; selected resource name is empty;. ")
        Else
            activity = $"trying to connect '{sender.SessionFactory.SelectedResourceName}' {If(String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle), "unknown", Me.DeviceBase.ResourceTitle)}"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
            Me.TryOpenSession(sender.SessionFactory.SelectedResourceName, Me.DeviceBase.ResourceTitle, e)
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Connects. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling connector connect event"
        Try
            Dim args As New Core.Pith.ActionEventArgs
            If Not Me.TryConnect(TryCast(sender, ResourceSelectorConnector), args) Then
                Dim message As New Core.Pith.TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. {args.Details}")
                If Me.Talker Is Nothing Then
                    If message.EventType <= TraceEventType.Warning Then
                        isr.Core.Pith.My.MyLibrary.LogUnpublishedMessage(message)
                    End If
                Else
                    Me.Talker.Publish(message)
                End If
            End If
            e.Cancel = args.Cancel
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Disconnects this object. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub Disconnect(ByVal sender As ResourceSelectorConnector, ByVal e As System.ComponentModel.CancelEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling connector disconnect event"
        Dim args As New Core.Pith.ActionEventArgs
        If Not Me.TryCloseSession(args) Then
            Dim message As New Core.Pith.TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. {args.Details}")
            If Me.Talker Is Nothing Then
                If message.EventType <= TraceEventType.Warning Then
                    isr.Core.Pith.My.MyLibrary.LogUnpublishedMessage(message)
                End If
            Else
                Me.Talker.Publish(message)
            End If
        End If
        e.Cancel = args.Cancel
    End Sub

    ''' <summary> Disconnects this object. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Disconnect(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs)
        Dim activity As String = "handling connector disconnect event"
        Try
            Me.Disconnect(TryCast(sender, ResourceSelectorConnector), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Cancel event information. </param>
    Public Overridable Sub ClearDevice(ByVal sender As ResourceSelectorConnector, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Me.DeviceBase.ResetClearInit()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ClearDevice(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling connector clear event"
        Try
            Me.ClearDevice(TryCast(sender, ResourceSelectorConnector), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Gets the number of internal resource names. </summary>
    ''' <value> The number of internal resource names. </value>
    Public ReadOnly Property ResourceNamesCount As Integer
        Get
            Return Me.Connector.InternalResourceNamesCount
        End Get
    End Property

    ''' <summary> Gets the name of the internal selected resource. </summary>
    ''' <value> The name of the internal selected resource. </value>
    Public ReadOnly Property SelectedResourceName As String
        Get
            Return Me.Connector.InternalSelectedResourceName
        End Get
    End Property

    ''' <summary> Gets a list of names of the has resources. </summary>
    ''' <value> A list of names of the has resources. </value>
    Public ReadOnly Property HasResourceNames As Boolean
        Get
            Return Me.SessionFactory.HasResources
        End Get
    End Property

    ''' <summary> Gets the connector is connected. </summary>
    ''' <value> The connector is connected. </value>
    Public ReadOnly Property IsConnected As Boolean
        Get
            Return Me.Connector.IsConnected
        End Get
    End Property

    ''' <summary> Gets the selected resource exists. </summary>
    ''' <value> The selected resource exists. </value>
    Public ReadOnly Property SelectedResourceExists As Boolean
        Get
            Return Me.SessionFactory.SelectedResourceExists
        End Get
    End Property

    Public Overridable Sub FindResourceNames(ByVal sender As ResourceSelectorConnector, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse sender.SessionFactory Is Nothing OrElse e Is Nothing Then Return
        sender.SessionFactory.ResourcesFilter = Me.DeviceBase.Session.ResourceNameInfo.ResourcesFilter
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Displaying {Me.SessionFactory.ResourcesFilter} resources")
        sender.SessionFactory.EnumerateResources()
    End Sub

    ''' <summary> Displays available instrument names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FindResourceNames(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Finding resource names"
        Try
            Me.FindResourceNames(TryCast(sender, ResourceSelectorConnector), e)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Handles the property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Public Overridable Overloads Sub HandlePropertyChange(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SessionFactory.SelectedResourceName)
                If sender.SessionFactory.SelectedResourceName IsNot Nothing Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                      $"Resource {If(sender.IsConnected, "connected", "selected")};. {sender.SessionFactory.SelectedResourceName}")
                End If
        End Select
        Me.SafePostPropertyChanged(propertyName)
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ConnectorPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        ' the connector keeps sending events even after it is disposed and set to nothing!
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ResourceSelectorConnector)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, ResourceSelectorConnector), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystemBase

    ''' <summary> Gets or sets the status subsystem. </summary>
    ''' <value> The status subsystem. </value>
    Public Property StatusSubsystem As StatusSubsystemBase
        Get
            Return Me._StatusSubsystem
        End Get
        Private Set(value As VI.StatusSubsystemBase)
            If Me._StatusSubsystem IsNot Nothing Then
                RemoveHandler Me._StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
            Me._StatusSubsystem = value
            If value IsNot Nothing Then
                AddHandler Me._StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                Me.Identity = value.Identity
            End If
        End Set
    End Property

    ''' <summary> Reports the last error. </summary>
    Public Overridable Sub OnLastError(ByVal lastError As DeviceError)
    End Sub

    ''' <summary> Handles the subsystem property change. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Public Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Dim message As Core.Pith.TraceMessage = Nothing
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
            Case NameOf(StatusSubsystemBase.MessageAvailable)
                If subsystem.MessageAvailable Then
                    message = New Core.Pith.TraceMessage(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Message available;. ")
                End If
            Case NameOf(StatusSubsystemBase.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    message = New Core.Pith.TraceMessage(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measurement available;. ")
                End If
            Case NameOf(StatusSubsystemBase.ReadingDeviceErrors)
                If subsystem.ReadingDeviceErrors Then
                    message = New Core.Pith.TraceMessage(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading device errors;. ")
                End If
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ServiceRequestStatus)
                Me.StatusRegisterStatus = subsystem.ServiceRequestStatus
            Case NameOf(StatusSubsystemBase.StandardEventStatus)
#Disable Warning IDE0030
                Me.StandardRegisterStatus = If(subsystem.StandardEventStatus.HasValue, subsystem.StandardEventStatus.Value, DeviceConnector.UnknownRegisterValue)
#Enable Warning IDE0030
        End Select
        If message IsNot Nothing Then Me.Talker?.Publish(message)
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(StatusSubsystemBase)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, StatusSubsystemBase), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Assign talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Private Sub _AssignTalker(talker As Core.Pith.ITraceMessageTalker)
        Me._Connector.AssignTalker(talker)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As Core.Pith.ITraceMessageTalker)
        Me._AssignTalker(talker)
        Me.Connector.AssignTalker(talker)
        MyBase.AssignTalker(talker)
    End Sub

    Private Sub _ApplyListenerTraceLevel(ByVal listenerType As Core.Pith.ListenerType, ByVal value As TraceEventType)
        Me._Connector.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As Core.Pith.ListenerType, ByVal value As TraceEventType)
        Me._ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

End Class
