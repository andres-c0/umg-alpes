Option Strict On
Option Explicit On

Namespace Envios
    Public Class Estado_Envio
        Public Property EstadoEnvioId As Integer
        Public Property Codigo As String
        Public Property Descripcion As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace