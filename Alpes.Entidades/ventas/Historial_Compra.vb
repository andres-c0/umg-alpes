Option Strict On
Option Explicit On

Namespace Ventas
    Public Class HistorialCompra
        Public Property HistCompraId As Integer
        Public Property CliId As Integer
        Public Property OrdenVentaId As Integer
        Public Property CompraAt As DateTime
        Public Property MontoTotalSnapshot As Decimal
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace
