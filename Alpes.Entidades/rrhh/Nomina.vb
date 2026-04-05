Option Strict On
Option Explicit On

Namespace RecursosHumanos
    Public Class Nomina
        Public Property nomina_id As Integer
        Public Property emp_id As Integer
        Public Property periodo_inicio As DateTime?
        Public Property periodo_fin As DateTime?
        Public Property monto_bruto As Decimal
        Public Property monto_neto As Decimal
        Public Property fecha_pago As DateTime?
        Public Property estado As String
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
    End Class
End Namespace