Option Strict On
Option Explicit On

Public Class ReglaEnvioGratis
    Public Property ReglaEnvioGratisId As Integer
    Public Property ZonaEnvioId As Integer
    Public Property MontoMinimo As Nullable(Of Decimal)
    Public Property PesoMaxKg As Nullable(Of Decimal)
    Public Property VigenciaInicio As Nullable(Of DateTime)
    Public Property VigenciaFin As Nullable(Of DateTime)
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
