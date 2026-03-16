Option Strict On
Option Explicit On

Public Class ReglaPromocion
    Public Property ReglaPromocionId As Integer
    Public Property PromocionId As Integer
    Public Property MinCompra As Nullable(Of Decimal)
    Public Property MinItems As Nullable(Of Integer)
    Public Property AplicaTipoProducto As String
    Public Property TopeDescuento As Nullable(Of Decimal)
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
