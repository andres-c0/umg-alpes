Option Strict On
Option Explicit On

Namespace Inventario
    Public Class Precio_Historico
        Public Property PrecioHistId As Integer
        Public Property ProductoId As Integer
        Public Property NombreProducto As String
        Public Property Precio As Decimal
        Public Property VigenciaInicio As DateTime
        Public Property VigenciaFin As DateTime?
        Public Property Motivo As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace