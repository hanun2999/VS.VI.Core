﻿Namespace K3700.Tests
    ''' <summary> K3700 Resource Control unit tests. </summary>
    ''' <license>
    ''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="10/10/2017" by="David" revision=""> Created. </history>
    <TestClass()>
    Public Class K3700ControlTests

#Region " CONSTRUCTION + CLEANUP "

        ''' <summary> My class initialize. </summary>
        ''' <param name="testContext"> Gets or sets the test context which provides information about
        '''                            and functionality for the current test run. </param>
        ''' <remarks>Use ClassInitialize to run code before running the first test in the class</remarks>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        <ClassInitialize()>
        Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
            Try
                TestInfo.InitializeTraceListener()
            Catch
                ' cleanup to meet strong guarantees
                Try
                    MyClassCleanup()
                Finally
                End Try
                Throw
            End Try
        End Sub

        ''' <summary> My class cleanup. </summary>
        ''' <remarks> Use ClassCleanup to run code after all tests in a class have run. </remarks>
        <ClassCleanup()>
        Public Shared Sub MyClassCleanup()
        End Sub

        ''' <summary> Initializes before each test runs. </summary>
        <TestInitialize()> Public Sub MyTestInitialize()
            Assert.IsTrue(TestInfo.Exists, $"{GetType(TestInfo)} settings not found")
            Assert.IsTrue(K3700.Tests.K3700SubsystemsInfo.Get.Exists, $"{GetType(K3700.Tests.K3700SubsystemsInfo)} settings not found")
            TestInfo.ClearMessageQueue()
        End Sub

        ''' <summary> Cleans up after each test has run. </summary>
        <TestCleanup()> Public Sub MyTestCleanup()
            TestInfo.AssertMessageQueue()
        End Sub

        '''<summary>
        '''Gets or sets the test context which provides
        '''information about and functionality for the current test run.
        '''</summary>
        Public Property TestContext() As TestContext

#End Region

#Region " ASSIGNED DEVICE TESTS "

        <TestMethod(), TestCategory("VI")>
        Public Sub AssignDeviceTest()
            Using control As VI.Tsp.K3700.K3700Control = New VI.Tsp.K3700.K3700Control
                Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                    device.AddListener(TestInfo.TraceMessagesQueueListener)
                    control.AssignDevice(device, False)
                    K3700Manager.OpenCloseSession(1, control)
                End Using
            End Using
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub AssignOpenDeviceTest()
            Using control As VI.Tsp.K3700.K3700Control = New VI.Tsp.K3700.K3700Control()
                Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                    device.AddListener(TestInfo.TraceMessagesQueueListener)
                    K3700Manager.OpenSession(device)
                    control.AssignDevice(device, False)
                    K3700Manager.OpenCloseSession(1, control)
                End Using
            End Using
        End Sub

#End Region

    End Class
End Namespace
