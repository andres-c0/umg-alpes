Option Strict On
Option Explicit On

Namespace Seguridad
    Public Class Historial_Cambio_Contrasena
        Public Property HcId As Integer
        Public Property UsuId As Integer
        Public Property CambioAt As DateTime
        Public Property Motivo As String
        Public Property Ip As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
    End Class
End Namespace