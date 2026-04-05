Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Historial_Promocion
        Public Property historial_promocion_id As Integer
        Public Property orden_venta_id As Integer
        Public Property promocion_id As Integer
        Public Property monto_descuento_snapshot As Decimal
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
