Namespace Entidades

    Public Class Envio

        Public Property EnvioId As Integer
        Public Property OrdenVentaId As Integer
        Public Property EstadoEnvioId As Integer
        Public Property TipoEntregaId As Integer
        Public Property ZonaEnvioId As Integer
        Public Property RutaEntregaId As Integer?
        Public Property DireccionEntregaSnapshot As String
        Public Property CostoEnvioSnapshot As Decimal
        Public Property FechaEnvio As Date?
        Public Property FechaEntregaEstimada As Date?
        Public Property FechaEntregaReal As Date?
        Public Property TrackingCodigo As String
        Public Property Estado As String

    End Class

End Namespace
