Option Strict On
Option Explicit On

Namespace RRHH
    Public Class NominaDetalle
        Public Property NominaDetalleId As Integer
        Public Property NominaId As Integer?
        Public Property Tipo As String
        Public Property Concepto As String
        Public Property Monto As Decimal?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace