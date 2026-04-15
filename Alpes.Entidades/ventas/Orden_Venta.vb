Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Orden_Venta
        Public Property OrdenVentaId As Integer
        Public Property NumOrden As String
        Public Property CliId As Integer
        Public Property EstadoOrdenId As Integer
        Public Property FechaOrden As DateTime
        Public Property Subtotal As Decimal
        Public Property Descuento As Decimal
        Public Property Impuesto As Decimal
        Public Property Total As Decimal
        Public Property Moneda As String
        Public Property DireccionEnvioSnapshot As String
        Public Property Observaciones As String
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace