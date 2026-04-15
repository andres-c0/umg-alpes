Option Strict On
Option Explicit On

Namespace Produccion
    Public Class ListaMateriales
        Public Property ListaMaterialesId As Integer
        Public Property ProductoId As Integer
        Public Property Version As String
        Public Property VigenciaInicio As DateTime?
        Public Property VigenciaFin As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace