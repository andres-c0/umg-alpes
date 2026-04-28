Option Strict On
Option Explicit On

Namespace RRHH
    Public Class IncidenteLaboral
        Public Property IncidenteId As Integer
        Public Property EmpId As Integer?
        Public Property FechaIncidente As DateTime?
        Public Property Descripcion As String
        Public Property Gravedad As String
        Public Property AccionesTomadas As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace