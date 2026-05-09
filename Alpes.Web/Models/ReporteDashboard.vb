Namespace Models

    Public Class ReporteDashboard

        Public Property VentasTotales As Decimal
        Public Property Ordenes As Integer
        Public Property Clientes As Integer
        Public Property TicketPromedio As Decimal
        Public Property Canceladas As Integer
        Public Property ProductosStockBajo As Integer

        Public Property ItemsVendidos As Integer

        Public Property VentasMensuales As List(Of VentaMensualViewModel)
        Public Property UltimasOrdenes As List(Of OrdenReporteViewModel)
        Public Property ProductosTop As List(Of ProductoTopViewModel)

    End Class

    Public Class VentaMensualViewModel
        Public Property Mes As String
        Public Property Total As Decimal
        Public Property Ordenes As Integer
    End Class

    Public Class OrdenReporteViewModel
        Public Property Codigo As String
        Public Property Cliente As String
        Public Property FechaOrden As DateTime
        Public Property Estado As String
        Public Property Total As Decimal
    End Class

    Public Class ProductoTopViewModel
        Public Property Producto As String
        Public Property Cantidad As Integer
        Public Property Total As Decimal
    End Class

End Namespace