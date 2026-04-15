Option Strict On
Option Explicit On

Namespace Seguridad
    Public Class Sesion
        Public Property SesionId As Integer
        Public Property UsuId As Integer
        Public Property TokenHash As String
        Public Property Ip As String
        Public Property UserAgent As String
        Public Property InicioAt As DateTime
        Public Property FinAt As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
    End Class
End Namespace