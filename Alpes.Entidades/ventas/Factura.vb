Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Factura
        Public Property FacturaId As Integer
        Public Property OrdenVentaId As Integer
        Public Property NumFactura As String
        Public Property FechaEmision As DateTime
        Public Property NitFacturacion As String
        Public Property DireccionFacturacionSnapshot As String
        Public Property TotalFacturaSnapshot As Decimal
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace