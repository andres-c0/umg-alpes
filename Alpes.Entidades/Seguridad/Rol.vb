Option Strict On
Option Explicit On

Namespace Seguridad
    Public Class Rol
        Public Property RolId As Integer
        Public Property RolNombre As String
        Public Property Descripcion As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace