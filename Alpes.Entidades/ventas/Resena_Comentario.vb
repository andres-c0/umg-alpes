Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Resena_Comentario
        Public Property resena_id As Integer
        Public Property cli_id As Integer
        Public Property producto_id As Integer
        Public Property calificacion As Integer
        Public Property comentario As String
        Public Property resena_at As DateTime
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
