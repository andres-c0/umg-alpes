Option Strict On
Option Explicit On

Namespace RecursosHumanos
    Public Class Nomina_Detalle
        Public Property nomina_detalle_id As Integer
        Public Property nomina_id As Integer
        Public Property tipo As String
        Public Property concepto As String
        Public Property monto As Decimal
        Public Property estado As String
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
    End Class
End Namespace
