Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Envio
        Public Property EnvioId As Integer
        Public Property OrdenVentaId As Integer
        Public Property EstadoEnvioId As Integer
        Public Property TipoEntregaId As Integer
        Public Property ZonaEnvioId As Integer
        Public Property RutaEntregaId As Nullable(Of Integer)
        Public Property DireccionEntregaSnapshot As String
        Public Property CostoEnvioSnapshot As Decimal
        Public Property FechaEnvio As Nullable(Of DateTime)
        Public Property FechaEntregaEstimada As Nullable(Of DateTime)
        Public Property FechaEntregaReal As Nullable(Of DateTime)
        Public Property TrackingCodigo As String
        Public Property Estado As String

        ' Campos descriptivos que pueden venir de SP_LISTAR_ENVIOS / SP_BUSCAR_ENVIOS
        Public Property EstadoEnvioDescripcion As String
        Public Property TipoEntregaNombre As String
        Public Property ZonaEnvioNombre As String
    End Class
End Namespace