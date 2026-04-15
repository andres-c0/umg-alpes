Option Strict On
Option Explicit On

Namespace Ventas
    Public Class ResenaComentario
        Public Property ResenaId As Integer
        Public Property CliId As Integer
        Public Property ProductoId As Integer
        Public Property Calificacion As Decimal?
        Public Property Comentario As String
        Public Property ResenaAt As DateTime
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace