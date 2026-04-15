Option Strict On
Option Explicit On

Namespace Ventas
    Public Class HistorialPromocion
        Public Property HistorialPromocionId As Integer
        Public Property OrdenVentaId As Integer?
        Public Property PromocionId As Integer?
        Public Property MontoDescuentoSnapshot As Decimal?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace