Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Factura_Detalle
        Public Property FacturaDetId As Integer
        Public Property FacturaId As Integer
        Public Property ProductoId As Integer
        Public Property Cantidad As Integer
        Public Property PrecioUnitarioSnapshot As Decimal
        Public Property TotalLinea As Decimal
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace