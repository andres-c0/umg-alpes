Option Strict On
Option Explicit On

Namespace Produccion
    Public Class OrdenProduccion
        Public Property OrdenProduccionId As Integer
        Public Property NumOp As String
        Public Property ProductoId As Integer
        Public Property CantidadPlanificada As Integer
        Public Property EstadoProduccionId As Integer
        Public Property InicioEstimado As DateTime?
        Public Property FinEstimado As DateTime?
        Public Property InicioReal As DateTime?
        Public Property FinReal As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace