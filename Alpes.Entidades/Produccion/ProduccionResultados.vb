Option Strict On
Option Explicit On

Namespace Produccion
    Public Class ProduccionResultados
        Public Property ResultadoId As Integer
        Public Property OrdenProduccionId As Integer
        Public Property UnidadesBuenas As Integer
        Public Property UnidadesMerma As Integer
        Public Property RegistroAt As DateTime
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace