Option Strict On
Option Explicit On

Imports System.Collections.Generic

Namespace Models
    Public Class FusionarCarritoInvitadoViewModel
        Public Property Items As List(Of FusionarCarritoInvitadoItemViewModel)
    End Class

    Public Class FusionarCarritoInvitadoItemViewModel
        Public Property ProductoId As Integer
        Public Property Cantidad As Integer
    End Class
End Namespace