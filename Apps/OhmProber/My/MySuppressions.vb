Imports System.Diagnostics.CodeAnalysis
#Region "CA1020:AvoidNamespacesWithFewTypes"

' Namespaces should generally have more than five types. 
' Namespaces, not currently having the prescribed type count, were defined with an eye towards inclusion of additional future types.
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope:="namespace", Target:="isr.VI",
    Justification:="Ignoring this warning...we want these namespaces, but don't have enough classes to go in them to satisfy the rule.")> 

#End Region

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Multi")>
<Assembly: CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="OhmProber")>
