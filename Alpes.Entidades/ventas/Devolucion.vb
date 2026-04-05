Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Devolucion
        Public Property devolucion_id As Integer
        Public Property orden_venta_id As Integer
        Public Property cli_id As Integer
        Public Property motivo As String
        Public Property estado_devolucion As String
        Public Property solicitud_at As DateTime
        Public Property resolucion_at As DateTime?
        Public Property created_at As DateTime?
        Public Property updated_at As DateTime?
        Public Property estado As String
    End Class
End Namespace
