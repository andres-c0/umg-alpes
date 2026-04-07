Option Strict On
Option Explicit On

Namespace Ventas
    Public Class ReglaPromocion
        Public Property ReglaPromocionId As Integer
        Public Property PromocionId As Integer?
        Public Property Promocion As String
        Public Property MinCompra As Decimal?
        Public Property MinItems As Integer?
        Public Property AplicaTipoProducto As String
        Public Property TopeDescuento As Decimal?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace