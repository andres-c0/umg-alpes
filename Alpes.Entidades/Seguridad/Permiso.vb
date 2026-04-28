Option Strict On
Option Explicit On

Namespace Seguridad
    Public Class Permiso
        Public Property PermisoId As Integer
        Public Property Codigo As String
        Public Property Descripcion As String
        Public Property Modulo As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace