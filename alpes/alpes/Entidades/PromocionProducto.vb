Option Strict On
Option Explicit On

Public Class PromocionProducto
    Public Property PromocionProductoId As Integer
    Public Property PromocionId As Integer
    Public Property ProductoId As Integer
    Public Property LimiteUnidades As Nullable(Of Integer)
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
