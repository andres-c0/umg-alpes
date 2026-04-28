Option Strict On
Option Explicit On

Namespace Compras
    Public Class Control_Calidad
        Public Property ControlCalidadId As Integer
        Public Property Origen As String
        Public Property OrdenProduccionId As Integer?
        Public Property RecepcionMaterialId As Integer?
        Public Property Resultado As String
        Public Property Observacion As String
        Public Property InspeccionAt As DateTime
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace