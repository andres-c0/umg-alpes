Option Strict On
Option Explicit On

Namespace Seguridad
    Public Class Usuario
        Public Property UsuId As Integer
        Public Property Username As String
        Public Property PasswordHash As String
        Public Property Email As String
        Public Property Telefono As String
        Public Property RolId As Integer
        Public Property CliId As Integer?
        Public Property EmpId As Integer?
        Public Property UltimoLoginAt As DateTime?
        Public Property BloqueadoHasta As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace