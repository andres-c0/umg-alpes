Option Strict On
Option Explicit On

Namespace Produccion
    Public Class ConsumoMateriaPrima
        Public Property ConsumoId As Integer
        Public Property OrdenProduccionId As Integer
        Public Property MpId As Integer
        Public Property Cantidad As Decimal
        Public Property ConsumoAt As DateTime
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace