Option Strict On
Option Explicit On

Namespace Modelos

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

        Public Property VentasAgrupadas As List(Of VentaAgrupadaViewModel)
        Public Property ProductoMasVendido As ProductoTopViewModel
        Public Property CierreCaja As List(Of CierreCajaViewModel)

        Public Property ComprasPorCliente As List(Of CompraClienteViewModel)
        Public Property IndicadoresMensuales As List(Of IndicadorMensualViewModel)
        Public Property ValorPorCliente As Decimal
        Public Property PorcentajeRecompra As Decimal

        Public Property DetalleMensualProductos As List(Of DetalleMensualProductoViewModel)

        Public Property Cohortes As List(Of CohorteViewModel)
        Public Property ClientesCohorte As Integer
        Public Property RetencionM1 As Decimal
        Public Property RetencionM2 As Decimal
        Public Property VentaCohorte As Decimal

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

        Public Property Tipo As String
    End Class

    Public Class VentaAgrupadaViewModel
        Public Property Tipo As String
        Public Property Fecha As DateTime?
        Public Property Producto As String
        Public Property Cantidad As Integer
        Public Property CostoUnitario As Decimal
        Public Property Total As Decimal
    End Class

    Public Class CierreCajaViewModel
        Public Property FormaPago As String
        Public Property Ordenes As Integer
        Public Property Total As Decimal
        Public Property Porcentaje As Decimal
    End Class

    Public Class CompraClienteViewModel
        Public Property FechaCompra As DateTime
        Public Property Cliente As String
        Public Property Valor As Decimal
        Public Property FormaPago As String
        Public Property MueblesIncluidos As String
    End Class

    Public Class IndicadorMensualViewModel
        Public Property Mes As String
        Public Property Ventas As Decimal
        Public Property Ordenes As Integer
        Public Property ClientesActivos As Integer
        Public Property TicketPromedio As Decimal
    End Class

    Public Class DetalleMensualProductoViewModel
        Public Property Mes As String
        Public Property Producto As String
        Public Property Cantidad As Integer
        Public Property Ventas As Decimal
        Public Property PorcentajeTotal As Decimal
    End Class

    Public Class CohorteViewModel
        Public Property Cohorte As String
        Public Property Clientes As Integer
        Public Property M0 As Integer
        Public Property M1 As Integer
        Public Property M2 As Integer
        Public Property M3 As Integer
        Public Property Ventas As Decimal
    End Class

End Namespace