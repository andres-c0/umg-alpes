Option Strict On
Option Explicit On

Namespace Produccion
    Public Class ListaMaterialesDetalle
        Public Property ListaMaterialesDetId As Integer
        Public Property ListaMaterialesId As Integer
        Public Property MpId As Integer
        Public Property CantidadRequerida As Decimal
        Public Property MermaPct As Decimal?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace