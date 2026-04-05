Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Promocion
        Public Property promocion_id As Integer
        Public Property tipo_promocion_id As Integer
        Public Property tipo_promocion As String
        Public Property nombre As String
        Public Property descripcion As String
        Public Property vigencia_inicio As DateTime
        Public Property vigencia_fin As DateTime?
        Public Property prioridad As Integer
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace