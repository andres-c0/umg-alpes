Option Strict On
Option Explicit On

Namespace Ventas
    Public Class Estado_Orden
        Public Property EstadoOrdenId As Integer
        Public Property Codigo As String
        Public Property Descripcion As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace