Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Tarifa_Envio
        Public Property tarifa_envio_id As Integer
        Public Property zona_envio_id As Integer
        Public Property zona_envio As String
        Public Property tipo_entrega_id As Integer
        Public Property tipo_entrega As String
        Public Property peso_desde_kg As Decimal
        Public Property peso_hasta_kg As Decimal
        Public Property costo As Decimal
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
