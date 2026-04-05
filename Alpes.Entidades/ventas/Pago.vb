Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Pago
        Public Property pago_id As Integer
        Public Property orden_venta_id As Integer
        Public Property metodo_pago_id As Integer
        Public Property monto As Decimal
        Public Property estado_pago As String
        Public Property referencia As String
        Public Property pago_at As DateTime
        Public Property created_at As DateTime
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace