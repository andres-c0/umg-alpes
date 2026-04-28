Option Strict On
Option Explicit On

Namespace Produccion
    Public Class OrdenProduccionTarea
        Public Property OpTareaId As Integer
        Public Property OrdenProduccionId As Integer
        Public Property NombreTarea As String
        Public Property Orden As Integer?
        Public Property InicioAt As DateTime?
        Public Property FinAt As DateTime?
        Public Property EmpIdResponsable As Integer?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace