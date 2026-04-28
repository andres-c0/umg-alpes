Option Strict On
Option Explicit On

Public Class CrudTablaConfig
    Public Property TableName As String
    Public Property Pk As String
    Public Property Endpoint As String
    Public Property Campos As List(Of CrudCampoConfig)
End Class