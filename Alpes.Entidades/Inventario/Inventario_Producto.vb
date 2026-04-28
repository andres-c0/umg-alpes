Option Strict On
Option Explicit On

Namespace Inventario
    Public Class Inventario_Producto
        Public Property InvProdId As Integer
        Public Property ProductoId As Integer
        Public Property NombreProducto As String
        Public Property Stock As Integer
        Public Property StockReservado As Integer
        Public Property StockMinimo As Integer?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace