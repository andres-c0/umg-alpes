Option Strict On
Option Explicit On

Namespace Compras
    Public Class ContratoProveedor
        Public Property ContratoProveedorId As Integer
        Public Property ProvId As Integer
        Public Property Titulo As String
        Public Property VigenciaInicio As DateTime?
        Public Property VigenciaFin As DateTime?
        Public Property UrlDocumento As String
        Public Property Estado As String
    End Class
End Namespace