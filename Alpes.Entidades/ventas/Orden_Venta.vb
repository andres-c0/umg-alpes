Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Orden_Venta
        Public Property orden_venta_id As Integer
        Public Property num_orden As String
        Public Property cli_id As Integer
        Public Property estado_orden_id As Integer
        Public Property fecha_orden As DateTime
        Public Property subtotal As Decimal
        Public Property descuento As Decimal
        Public Property impuesto As Decimal
        Public Property total As Decimal
        Public Property moneda As String
        Public Property direccion_envio_snapshot As String
        Public Property observaciones As String
        Public Property created_at As DateTime
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace