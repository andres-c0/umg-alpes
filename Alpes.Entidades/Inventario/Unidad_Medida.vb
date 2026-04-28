Option Strict On
Option Explicit On

Namespace Inventario
    Public Class Unidad_Medida
        Public Property UnidadMedidaId As Integer
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace