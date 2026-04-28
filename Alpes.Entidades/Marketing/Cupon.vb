Option Strict On
Option Explicit On

Namespace Marketing
    Public Class Cupon
        Public Property CuponId As Integer
        Public Property Codigo As String
        Public Property Descripcion As String
        Public Property VigenciaInicio As DateTime
        Public Property VigenciaFin As DateTime
        Public Property LimiteUsoTotal As Integer?
        Public Property LimiteUsoPorCliente As Integer?
        Public Property UsosActuales As Integer
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace