Option Strict On
Option Explicit On

Namespace Compras
    Public Class CuentaPagarProveedor
        Public Property CuentaPagarId As Integer
        Public Property ProvId As Integer
        Public Property RazonSocial As String
        Public Property OrdenCompraId As Integer
        Public Property NumOc As String
        Public Property Saldo As Decimal
        Public Property FechaVencimiento As DateTime?
        Public Property EstadoCp As String
        Public Property Estado As String
    End Class
End Namespace