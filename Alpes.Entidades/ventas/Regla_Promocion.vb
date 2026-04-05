Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Regla_Promocion
        Public Property regla_promocion_id As Integer
        Public Property promocion_id As Integer
        Public Property promocion As String
        Public Property min_compra As Decimal
        Public Property min_items As Integer
        Public Property aplica_tipo_producto As String
        Public Property tope_descuento As Decimal
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
