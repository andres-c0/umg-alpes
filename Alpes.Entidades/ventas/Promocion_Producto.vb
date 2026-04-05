Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Promocion_Producto
        Public Property promocion_producto_id As Integer
        Public Property promocion_id As Integer
        Public Property promocion As String
        Public Property producto_id As Integer
        Public Property referencia As String
        Public Property producto As String
        Public Property limite_unidades As Integer
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace