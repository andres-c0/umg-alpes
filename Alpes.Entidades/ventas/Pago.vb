Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Pago
        Public Property PagoId As Integer
        Public Property OrdenVentaId As Integer
        Public Property MetodoPagoId As Integer
        Public Property Monto As Decimal
        Public Property EstadoPago As String
        Public Property Referencia As String
        Public Property PagoAt As DateTime
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace