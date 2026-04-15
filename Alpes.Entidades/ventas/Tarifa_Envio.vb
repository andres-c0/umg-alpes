Option Strict On
Option Explicit On

Namespace Ventas
    Public Class TarifaEnvio
        Public Property TarifaEnvioId As Integer
        Public Property ZonaEnvioId As Integer?
        Public Property ZonaEnvio As String
        Public Property TipoEntregaId As Integer?
        Public Property TipoEntrega As String
        Public Property PesoDesdeKg As Decimal?
        Public Property PesoHastaKg As Decimal?
        Public Property Costo As Decimal?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace