Option Strict On
Option Explicit On

Namespace RecursosHumanos
    Public Class Incidente_Laboral
        Public Property incidente_id As Integer
        Public Property emp_id As Integer
        Public Property fecha_incidente As DateTime?
        Public Property descripcion As String
        Public Property gravedad As String
        Public Property acciones_tomadas As String
        Public Property estado As String
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
    End Class
End Namespace