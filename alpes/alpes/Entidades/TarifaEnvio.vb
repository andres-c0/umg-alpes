Option Strict On
Option Explicit On

Public Class TarifaEnvio
    Public Property TarifaEnvioId As Integer
    Public Property ZonaEnvioId As Integer
    Public Property TipoEntregaId As Integer
    Public Property PesoDesdeKg As Decimal
    Public Property PesoHastaKg As Decimal
    Public Property Costo As Decimal
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
