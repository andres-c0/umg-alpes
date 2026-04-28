Option Strict On
Option Explicit On

Namespace Ventas
    Public Class ReglaEnvioGratis
        Public Property ReglaEnvioGratisId As Integer
        Public Property ZonaEnvioId As Integer?
        Public Property ZonaEnvio As String
        Public Property MontoMinimo As Decimal?
        Public Property PesoMaxKg As Decimal?
        Public Property VigenciaInicio As DateTime?
        Public Property VigenciaFin As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace