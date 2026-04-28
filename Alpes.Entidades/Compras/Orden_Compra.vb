Option Strict On
Option Explicit On

Namespace Compras
    Public Class Orden_Compra
        Public Property OrdenCompraId As Integer
        Public Property NumOc As String
        Public Property ProvId As Integer
        Public Property RazonSocial As String
        Public Property EstadoOcId As Integer
        Public Property EstadoOcCodigo As String
        Public Property CondicionPagoId As Integer
        Public Property CondicionPagoNombre As String
        Public Property FechaOc As DateTime
        Public Property Subtotal As Decimal
        Public Property Impuesto As Decimal
        Public Property Total As Decimal
        Public Property Observaciones As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace