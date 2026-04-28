Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Promocion
        Public Property PromocionId As Integer
        Public Property TipoPromocionId As Integer?
        Public Property TipoPromocion As String
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property VigenciaInicio As DateTime
        Public Property VigenciaFin As DateTime
        Public Property Prioridad As Integer
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace