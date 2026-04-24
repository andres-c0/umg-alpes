Option Strict On
Option Explicit On

Namespace Ventas
    Public Class TarjetaCliente
        Public Property TarjetaClienteId As Integer
        Public Property CliId As Integer
        Public Property Titular As String
        Public Property Ultimos4 As String
        Public Property Marca As String
        Public Property MesVencimiento As Integer
        Public Property AnioVencimiento As Integer
        Public Property AliasTarjeta As String
        Public Property Predeterminada As Integer
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace