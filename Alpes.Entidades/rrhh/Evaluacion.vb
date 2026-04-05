Option Strict On
Option Explicit On

Namespace RecursosHumanos
    Public Class Evaluacion
        Public Property evaluacion_id As Integer
        Public Property emp_id As Integer
        Public Property evaluador_emp_id As Integer
        Public Property fecha_eval As DateTime?
        Public Property puntuacion As Decimal
        Public Property comentarios As String
        Public Property estado As String
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
    End Class
End Namespace