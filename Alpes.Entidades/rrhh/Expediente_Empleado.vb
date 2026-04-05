Option Strict On
Option Explicit On

Namespace RecursosHumanos
    Public Class Expediente_Empleado
        Public Property expediente_empleado_id As Integer
        Public Property emp_id As Integer
        Public Property tipo_documento As String
        Public Property url_documento As String
        Public Property fecha_documento As DateTime?
        Public Property estado As String
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
    End Class
End Namespace