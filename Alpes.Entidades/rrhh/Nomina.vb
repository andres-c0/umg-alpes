Option Strict On
Option Explicit On

Namespace RRHH
    Public Class Nomina
        Public Property NominaId As Integer
        Public Property EmpId As Integer?
        Public Property PeriodoInicio As DateTime?
        Public Property PeriodoFin As DateTime?
        Public Property MontoBruto As Decimal?
        Public Property MontoNeto As Decimal?
        Public Property FechaPago As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace