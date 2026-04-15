Option Strict On
Option Explicit On

Namespace RRHH
    Public Class Cargo
        Public Property CargoId As Integer
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace