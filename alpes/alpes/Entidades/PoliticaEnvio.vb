Option Strict On
Option Explicit On

Public Class PoliticaEnvio
    Public Property PoliticaEnvioId As Integer
    Public Property Titulo As String
    Public Property Descripcion As String
    Public Property VigenciaInicio As Nullable(Of DateTime)
    Public Property VigenciaFin As Nullable(Of DateTime)
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
