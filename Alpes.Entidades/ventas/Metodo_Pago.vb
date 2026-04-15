Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Metodo_Pago
        Public Property MetodoPagoId As Integer
        Public Property Nombre As String
        Public Property RequiereDatosExtra As Integer
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace