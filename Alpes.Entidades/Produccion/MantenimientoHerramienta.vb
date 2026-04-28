Option Strict On
Option Explicit On

Namespace Produccion
    Public Class MantenimientoHerramienta
        Public Property MantenimientoId As Integer
        Public Property HerramientaId As Integer
        Public Property FechaMantenimiento As DateTime
        Public Property Tipo As String
        Public Property Costo As Decimal?
        Public Property Observacion As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace