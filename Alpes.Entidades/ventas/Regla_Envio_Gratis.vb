Option Strict On
Option Explicit On

Namespace Envios
    Public Class Regla_Envio_Gratis
        Public Property regla_envio_gratis_id As Integer
        Public Property zona_envio_id As Integer
        Public Property zona_envio As String
        Public Property monto_minimo As Decimal
        Public Property peso_max_kg As Decimal
        Public Property vigencia_inicio As DateTime
        Public Property vigencia_fin As DateTime?
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace