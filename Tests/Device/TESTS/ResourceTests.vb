﻿Imports Microsoft.Win32
''' <summary> A resource tests. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/11/2017" by="David" revision=""> Created. </history>
<TestClass()>
Public Class ResourceTests

#Region " CONSTRUCTION AND CLEANUP "

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
        Assert.IsTrue(TestInfo.Exists, "App.Config not found")
    End Sub

    ''' <summary> Cleans up after each test has run. </summary>
    <TestCleanup()> Public Sub MyTestCleanup()
    End Sub

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext

#End Region

#Region " NULLABLE TESTS "

    '''<summary>
    '''A test for Nullable equality.
    '''</summary>
    <TestMethod()>
    Public Sub NullableBooleanTest()
        Dim bool As Boolean?
        Dim boolValue As Boolean = True
        Assert.AreEqual(True, bool Is Nothing, "Initialized to nothing")
        Assert.AreEqual(True, bool.Equals(New Boolean?), "Nullable not set equals to a new Boolean")
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable not set is not equal to {0}", boolValue)
        Assert.AreEqual(Nothing, bool = boolValue, "Nullable '=' operator yields Nothing")
        boolValue = False
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable not set is not equal to True")
        bool = boolValue
        Assert.AreEqual(True, bool.HasValue, "Has value -- set to {0}", boolValue)

        bool = New Nullable(Of Boolean)()
        Assert.AreEqual(True, bool Is Nothing, "Nullable set to new Boolean is still nothing")
        Assert.AreEqual(True, bool.Equals(New Boolean?), "Nullable set to new Boolean equals to a new Boolean")
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable set to new Boolean not equal to {0}", boolValue)
        boolValue = False
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable set to new Boolean not equal to True")
        bool = boolValue
        Assert.AreEqual(True, bool.HasValue, "Has value -- set to {0}", boolValue)
    End Sub

    '''<summary>
    '''A test for Nullable Integer equality.
    '''</summary>
    <TestMethod()>
    Public Sub NullableIntegerTest()
        Dim integerValue As Integer = 1
        Dim nullInt As Integer?
        Assert.AreEqual(True, nullInt Is Nothing, "Initialized to nothing")
        nullInt = New Nullable(Of Integer)()
        Assert.AreEqual(True, nullInt Is Nothing, "Nullable set to new Boolean is nothing")
        Assert.AreEqual(True, nullInt.Equals(New Integer?), "Nullable set to new Integer equals to a new Integer")
        Assert.AreEqual(False, nullInt.Equals(integerValue), "Nullable set to new Integer not equal to {0}", integerValue)
        Assert.AreEqual(False, nullInt.Equals(integerValue), "Nullable set to new Integer not equal to {0}", integerValue)
        nullInt = integerValue
        Assert.AreEqual(True, nullInt.HasValue, "Set to {0}", integerValue)
        Assert.AreEqual(integerValue, nullInt.Value, "Set to  {0}", integerValue)
    End Sub


    <TestMethod()>
    Public Sub ParseBooleanTest()
        Dim reading As String = "0"
        Dim expectedResult As Boolean = False
        Dim actualResult As Boolean = True
        Dim successParsing As Boolean = False
        successParsing = isr.VI.SessionBase.TryParse(reading, actualResult)
        Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
        Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
        reading = "1"
        expectedResult = True
        successParsing = isr.VI.SessionBase.TryParse(reading, actualResult)
        Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
        Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
    End Sub

#End Region

#Region " VERSION TESTS "

    ''' <summary> (Unit Test Method) validates the visa version test. </summary>
    <TestMethod()>
    Public Sub ValidateVisaVersionTest()
        Dim e As New isr.Core.Pith.CancelDetailsEventArgs
        isr.VI.ResourcesManagerBase.ValidateFoundationSystemFileVersion(e)
        Assert.IsFalse(e.Cancel, $"Invalid foundation VISA System file version: {e.Details}")
        isr.VI.ResourcesManagerBase.ValidateFoundationVisaAssemblyVersion(e)
        Assert.IsFalse(e.Cancel, $"Invalid foundation VISA .NET file version: {e.Details}")
        isr.VI.ResourcesManagerBase.ValidateVendorVersion(e)
        Assert.IsFalse(e.Cancel, $"Invalid national Instrument VISA version: {e.Details}")
    End Sub

#End Region

#Region " VISA RESOURCE TEST "

    ''' <summary> (Unit Test Method) tests locating visa resources. </summary>
    <TestMethod()>
    Public Sub VisaResourcesTest()
        Dim resourcesFilter As String = DeviceBase.BuildMinimalResourcesFilter
        Dim resources As String()
        Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
            resources = rm.FindResources(resourcesFilter).ToArray
        End Using
        Assert.IsTrue(resources.Any, $"VISA Resources {If(resources.Any, "", "not")} found")
    End Sub

#End Region

End Class