Option Strict On
Option Explicit On

Public Class CampanaMarketing
    Public Property CampanaMarketingId As Integer
    Public Property Nombre As String
    Public Property Canal As String
    Public Property Presupuesto As Nullable(Of Decimal)
    Public Property Inicio As DateTime
    Public Property Fin As DateTime
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As Nullable(Of DateTime)
    Public Property Estado As String
End Class
