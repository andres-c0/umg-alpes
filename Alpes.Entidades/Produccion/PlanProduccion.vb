Option Strict On
Option Explicit On

Namespace Produccion
    Public Class PlanProduccion
        Public Property PlanProduccionId As Integer
        Public Property ProductoId As Integer
        Public Property Cantidad As Integer
        Public Property PeriodoInicio As DateTime
        Public Property PeriodoFin As DateTime
        Public Property TiempoEstimadoHoras As Decimal?
        Public Property Estado As String
    End Class
End Namespace