Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Cuotas_Pago
        Public Property cuota_id As Integer
        Public Property pago_id As Integer
        Public Property num_cuota As Integer
        Public Property monto_cuota As Decimal
        Public Property fecha_vencimiento As DateTime?
        Public Property fecha_pago As DateTime?
        Public Property created_at As DateTime
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace