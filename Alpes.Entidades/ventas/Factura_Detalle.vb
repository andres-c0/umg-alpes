Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Factura_Detalle
        Public Property factura_det_id As Integer
        Public Property factura_id As Integer
        Public Property producto_id As Integer
        Public Property cantidad As Integer
        Public Property precio_unitario_snapshot As Decimal
        Public Property total_linea As Decimal
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
