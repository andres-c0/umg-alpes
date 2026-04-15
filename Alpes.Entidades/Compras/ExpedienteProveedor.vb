Option Strict On
Option Explicit On

Namespace Compras
    Public Class ExpedienteProveedor
        Public Property ExpedienteProveedorId As Integer
        Public Property ProvId As Integer
        Public Property TipoDocumento As String
        Public Property UrlDocumento As String
        Public Property FechaDocumento As DateTime?
        Public Property Estado As String
    End Class
End Namespace