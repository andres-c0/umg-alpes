Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Orden_Venta_Detalle
        Public Property orden_venta_det_id As Integer
        Public Property orden_venta_id As Integer
        Public Property producto_id As Integer
        Public Property cantidad As Integer
        Public Property precio_unitario_snapshot As Decimal
        Public Property subtotal_linea As Decimal
        Public Property created_at As DateTime
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace