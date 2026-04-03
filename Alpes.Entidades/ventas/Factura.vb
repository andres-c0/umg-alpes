Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Factura
        Public Property factura_id As Integer
        Public Property orden_venta_id As Integer
        Public Property num_factura As String
        Public Property fecha_emision As DateTime
        Public Property nit_facturacion As String
        Public Property direccion_facturacion_snapshot As String
        Public Property total_factura_snapshot As Decimal
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace