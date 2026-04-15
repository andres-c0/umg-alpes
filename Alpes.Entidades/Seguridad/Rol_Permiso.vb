Option Strict On
Option Explicit On

Namespace Seguridad
    Public Class Rol_Permiso
        Public Property RolPermisoId As Integer
        Public Property RolId As Integer
        Public Property PermisoId As Integer
        Public Property EsPermitido As Integer
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace