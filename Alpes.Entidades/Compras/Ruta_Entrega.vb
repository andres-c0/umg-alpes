Option Strict On
Option Explicit On

Namespace Logistica
    Public Class Ruta_Entrega
        Public Property RutaEntregaId As Integer
        Public Property VehiculoId As Integer
        Public Property PlacaVehiculo As String
        Public Property FechaRuta As DateTime
        Public Property Descripcion As String
        Public Property EstadoRuta As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace