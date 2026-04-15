Option Strict On
Option Explicit On

Namespace RRHH
    Public Class HistorialLaboral
        Public Property HistorialLaboralId As Integer
        Public Property EmpId As Integer
        Public Property FechaCambio As DateTime
        Public Property TipoCambio As String
        Public Property Detalle As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace