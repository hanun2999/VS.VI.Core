﻿''' <summary> Information about the version of a Keithley 7000 instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class VersionInfo
    Inherits isr.VI.VersionInfo

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
    End Sub

    ''' <summary> Parses the instrument firmware revision. </summary>
    ''' <remarks>
    ''' KEITHLEY INSTRUMENTS INC., MODEL 7001, wwwwwww, xxxxx,/yyyyy/zzzzz<para>
    ''' where; wwwwwww Is the serial number</para><para>
    ''' xxxxx Is the firmware revision level of the digital board</para><para>
    ''' yyyyy Is the firmware revision level of the VFD display</para><para>
    ''' zzzzz Is the firmware revision level of the LED display</para>
    ''' </remarks>
    ''' <exception cref="ArgumentNullException" guarantee="strong"> . </exception>
    ''' <param name="revision"> Specifies the instrument <see cref="FirmwareRevisionElements">board
    '''                         revisions</see>
    '''                         e.g., <c>xxxxx,/yyyyy/zzzzz</c> for the digital and display boards. </param>
    Protected Overrides Sub ParseFirmwareRevision(ByVal revision As String)

        If revision Is Nothing Then
            Throw New ArgumentNullException(NameOf(revision))
        ElseIf String.IsNullOrWhiteSpace(revision) Then
            MyBase.ParseFirmwareRevision(revision)
        Else
            MyBase.ParseFirmwareRevision(revision)

            ' get the revision sections
            Dim revSections As Queue(Of String) = New Queue(Of String)(revision.Split("/"c))

            ' Rev: xxxxx,/yyyyy/zzzzz
            If revSections.Any Then Me.FirmwareRevisionElements.Add(FirmwareRevisionElement.Digital.ToString, revSections.Dequeue.Trim.TrimEnd(",".ToCharArray))
            If revSections.Any Then Me.FirmwareRevisionElements.Add(FirmwareRevisionElement.Display.ToString, revSections.Dequeue.Trim)
            If revSections.Any Then Me.FirmwareRevisionElements.Add(FirmwareRevisionElement.LedDisplay.ToString, revSections.Dequeue.Trim)

        End If

    End Sub

End Class

