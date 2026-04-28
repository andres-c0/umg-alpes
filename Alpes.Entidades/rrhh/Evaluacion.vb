Option Strict On
Option Explicit On

Namespace RRHH
    Public Class Evaluacion
        Public Property EvaluacionId As Integer
        Public Property EmpId As Integer?
        Public Property EvaluadorEmpId As Integer?
        Public Property FechaEval As DateTime?
        Public Property Puntuacion As Decimal?
        Public Property Comentarios As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace