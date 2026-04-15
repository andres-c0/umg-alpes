Option Strict On
Option Explicit On

Namespace Clientes
    Public Class FAQ
        Public Property FaqId As Integer
        Public Property Pregunta As String
        Public Property Respuesta As String
        Public Property Orden As Integer?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace