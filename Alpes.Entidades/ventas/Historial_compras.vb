Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Historial_Compra
        Public Property hist_compra_id As Integer
        Public Property cli_id As Integer
        Public Property orden_venta_id As Integer
        Public Property compra_at As DateTime
        Public Property monto_total_snapshot As Decimal
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
