Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Orden_Venta_Detalle
        Public Property OrdenVentaDetId As Integer
        Public Property OrdenVentaId As Integer
        Public Property ProductoId As Integer
        Public Property Cantidad As Integer
        Public Property PrecioUnitarioSnapshot As Decimal
        Public Property SubtotalLinea As Decimal
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace