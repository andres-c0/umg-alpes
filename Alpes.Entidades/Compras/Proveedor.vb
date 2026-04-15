Option Strict On
Option Explicit On

Namespace Compras
    Public Class Proveedor
        Public Property ProvId As Integer
        Public Property RazonSocial As String
        Public Property Nit As String
        Public Property Email As String
        Public Property Telefono As String
        Public Property Direccion As String
        Public Property Ciudad As String
        Public Property Pais As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace