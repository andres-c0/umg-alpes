Option Strict On
Option Explicit On

Namespace RRHH
    Public Class ExpedienteEmpleado
        Public Property ExpedienteEmpleadoId As Integer
        Public Property EmpId As Integer?
        Public Property TipoDocumento As String
        Public Property UrlDocumento As String
        Public Property FechaDocumento As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace