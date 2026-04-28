Option Strict On
Option Explicit On

Namespace Ventas
    Public Class SeguimientoEnvio
        Public Property SegEnvioId As Integer
        Public Property EnvioId As Integer
        Public Property EstadoEnvioId As Integer
        Public Property EventoAt As Nullable(Of DateTime)
        Public Property UbicacionTexto As String
        Public Property Observacion As String
        Public Property CreatedAt As Nullable(Of DateTime)
        Public Property UpdatedAt As Nullable(Of DateTime)
        Public Property Estado As String
    End Class
End Namespace