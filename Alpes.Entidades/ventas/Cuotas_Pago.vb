Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Cuotas_Pago
        Public Property CuotaId As Integer
        Public Property PagoId As Integer
        Public Property NumCuota As Integer
        Public Property MontoCuota As Decimal
        Public Property FechaVencimiento As DateTime?
        Public Property FechaPago As DateTime?
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace
