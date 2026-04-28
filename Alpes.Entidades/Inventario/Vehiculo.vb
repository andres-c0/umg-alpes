Option Strict On
Option Explicit On

Namespace Logistica
    Public Class Vehiculo
        Public Property VehiculoId As Integer
        Public Property Placa As String
        Public Property Tipo As String
        Public Property CapacidadKg As Decimal?
        Public Property CapacidadM3 As Decimal?
        Public Property Activo As Integer
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
    End Class
End Namespace