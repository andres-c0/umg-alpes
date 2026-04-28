Option Strict On
Option Explicit On

Namespace Compras
    Public Class HistoricoPrecioMateriaPrimaProveedor
        Public Property HistMpProvId As Integer
        Public Property ProvId As Integer
        Public Property MpId As Integer
        Public Property Precio As Decimal
        Public Property VigenciaInicio As DateTime
        Public Property VigenciaFin As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace