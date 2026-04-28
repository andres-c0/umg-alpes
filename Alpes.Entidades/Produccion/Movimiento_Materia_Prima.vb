Option Strict On
Option Explicit On

Namespace Produccion
    Public Class Movimiento_Materia_Prima
        Public Property MovMpId As Integer
        Public Property InvMpId As Integer
        Public Property TipoMov As String
        Public Property Cantidad As Decimal
        Public Property Motivo As String
        Public Property MovAt As DateTime
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace