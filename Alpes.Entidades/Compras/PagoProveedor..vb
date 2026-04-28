Option Strict On
Option Explicit On

Namespace Compras
    Public Class PagoProveedor
        Public Property PagoProveedorId As Integer
        Public Property CuentaPagarId As Integer
        Public Property Monto As Decimal
        Public Property FechaPago As DateTime
        Public Property Referencia As String
        Public Property Estado As String
    End Class
End Namespace