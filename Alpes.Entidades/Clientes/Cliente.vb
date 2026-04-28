Option Strict On
Option Explicit On

Namespace Clientes
    Public Class Cliente
        Public Property CliId As Integer
        Public Property TipoDocumento As String
        Public Property NumDocumento As String
        Public Property Nit As String
        Public Property Nombres As String
        Public Property Apellidos As String
        Public Property NombreCompleto As String
        Public Property Email As String
        Public Property TelResidencia As String
        Public Property TelCelular As String
        Public Property Direccion As String
        Public Property Ciudad As String
        Public Property Departamento As String
        Public Property Pais As String
        Public Property Profesion As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace