Option Strict On
Option Explicit On

Public Class Cupon
    Public Property CuponId As Integer
    Public Property Codigo As String
    Public Property Descripcion As String
    Public Property VigenciaInicio As DateTime
    Public Property VigenciaFin As DateTime
    Public Property LimiteUsoTotal As Nullable(Of Integer)
    Public Property LimiteUsoPorCliente As Nullable(Of Integer)
    Public Property UsosActuales As Integer
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
