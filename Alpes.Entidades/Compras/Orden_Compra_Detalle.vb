Option Strict On
Option Explicit On

Namespace Compras
    Public Class Orden_Compra_Detalle
        Public Property OrdenCompraDetId As Integer
        Public Property OrdenCompraId As Integer
        Public Property MpId As Integer
        Public Property MateriaPrimaNombre As String
        Public Property Cantidad As Decimal
        Public Property CostoUnitario As Decimal
        Public Property SubtotalLinea As Decimal
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace