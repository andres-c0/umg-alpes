Option Strict On
Option Explicit On

Namespace Inventario
    Public Class Movimiento_Inventario
        Public Property MovInvId As Integer
        Public Property InvProdId As Integer
        Public Property TipoMov As String
        Public Property Cantidad As Integer
        Public Property Motivo As String
        Public Property ReferenciaId As Integer?
        Public Property MovAt As DateTime
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace