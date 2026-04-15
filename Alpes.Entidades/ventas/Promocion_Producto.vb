Option Strict On
Option Explicit On

Namespace Ventas
    Public Class PromocionProducto
        Public Property PromocionProductoId As Integer
        Public Property PromocionId As Integer?
        Public Property Promocion As String
        Public Property ProductoId As Integer?
        Public Property Referencia As String
        Public Property Producto As String
        Public Property LimiteUnidades As Integer?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace