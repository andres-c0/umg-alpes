Imports System
Imports System.Web.Mvc
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports ClosedXML.Excel
Imports Alpes.Datos.Repositorios
Imports Alpes.Datos.Modelos

Namespace Controllers

    Public Class ReportesController
        Inherits Controller

        Private Shared ReadOnly CafeOscuro As New BaseColor(44, 24, 16)
        Private Shared ReadOnly CafeMedio As New BaseColor(92, 61, 30)
        Private Shared ReadOnly Oro As New BaseColor(212, 168, 83)
        Private Shared ReadOnly Crema As New BaseColor(247, 243, 238)
        Private Shared ReadOnly Pergamino As New BaseColor(232, 224, 213)
        Private Shared ReadOnly Verde As New BaseColor(15, 141, 112)
        Private Shared ReadOnly Azul As New BaseColor(47, 127, 193)
        Private Shared ReadOnly Rojo As New BaseColor(157, 48, 48)

        Public Function Exportar(periodo As String,
                                 formato As String,
                                 anio As Integer,
                                 Optional mesInicio As Integer = 1,
                                 Optional mesFin As Integer = 12,
                                 Optional trimestre As Integer = 1) As ActionResult

            If formato IsNot Nothing AndAlso formato.ToLower() = "excel" Then
                Return ExportarExcel(periodo, anio, mesInicio, mesFin, trimestre)
            End If

            Return ExportarPdf(periodo, anio, mesInicio, mesFin, trimestre)
        End Function

        Private Function ExportarPdf(periodo As String,
                                     anio As Integer,
                                     mesInicio As Integer,
                                     mesFin As Integer,
                                     trimestre As Integer) As ActionResult

            Dim repo As New ReporteDatos()
            Dim data = repo.ObtenerReporte(periodo, anio, mesInicio, mesFin, trimestre)

            Using ms As New MemoryStream()
                Dim doc As New Document(PageSize.A4, 32, 32, 34, 34)
                Dim writer = PdfWriter.GetInstance(doc, ms)
                writer.PageEvent = New ReporteFooter()

                doc.Open()

                AgregarHeader(doc, periodo, anio)
                AgregarKpis(doc, data)
                AgregarVentasMensualesGrafico(doc, data)
                AgregarUltimasOrdenes(doc, data)

                doc.NewPage()
                AgregarProductosTop(doc, data)
                AgregarLecturaRapida(doc, data)

                doc.NewPage()
                AgregarVentasAgrupadas(doc, data, periodo, anio)

                doc.NewPage()
                AgregarPaginaComprasIndicadores(doc, data)

                doc.NewPage()
                AgregarPaginaRankingProductos(doc, data)

                doc.NewPage()
                AgregarPaginaCohorte(doc, data)

                doc.Add(New Paragraph(" "))
                doc.Close()

                Dim bytes = ms.ToArray()
                Dim nombre = "reporte_muebles_de_los_alpes_" & periodo & "_" & anio & ".pdf"

                Return File(bytes, "application/pdf", nombre)
            End Using
        End Function

        Private Sub AgregarHeader(doc As Document, periodo As String, anio As Integer)
            Dim tabla As New PdfPTable(3)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {1.1F, 5.4F, 1.8F})

            Dim logo As New PdfPCell(New Phrase("MA", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.WHITE)))
            logo.BackgroundColor = Oro
            logo.HorizontalAlignment = Element.ALIGN_CENTER
            logo.VerticalAlignment = Element.ALIGN_MIDDLE
            logo.FixedHeight = 58
            logo.Border = Rectangle.NO_BORDER
            logo.Padding = 8
            tabla.AddCell(logo)

            Dim titulo = New Phrase()
            titulo.Add(New Chunk("MUEBLES DE LOS ALPES" & vbCrLf, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.WHITE)))
            titulo.Add(New Chunk("Reporte administrativo ejecutivo · " & periodo & " " & anio, FontFactory.GetFont(FontFactory.HELVETICA, 10, New BaseColor(238, 220, 195))))

            Dim tituloCell As New PdfPCell(titulo)
            tituloCell.BackgroundColor = CafeOscuro
            tituloCell.Border = Rectangle.NO_BORDER
            tituloCell.VerticalAlignment = Element.ALIGN_MIDDLE
            tituloCell.Padding = 12
            tabla.AddCell(tituloCell)

            Dim fechaCell As New PdfPCell(New Phrase("Generado" & vbCrLf & DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE)))
            fechaCell.BackgroundColor = CafeOscuro
            fechaCell.HorizontalAlignment = Element.ALIGN_RIGHT
            fechaCell.VerticalAlignment = Element.ALIGN_MIDDLE
            fechaCell.Border = Rectangle.NO_BORDER
            fechaCell.Padding = 12
            tabla.AddCell(fechaCell)

            doc.Add(tabla)
            doc.Add(New Paragraph(" "))
        End Sub

        Private Sub AgregarKpis(doc As Document, data As ReporteDashboard)
            Dim tabla As New PdfPTable(3)
            tabla.WidthPercentage = 100
            tabla.SpacingAfter = 14
            tabla.SetWidths(New Single() {1, 1, 1})

            tabla.AddCell(KpiCell("Ventas totales", "Q " & data.VentasTotales.ToString("N2"), "Ingreso comercial", Verde))
            tabla.AddCell(KpiCell("Órdenes", data.Ordenes.ToString(), "Operaciones registradas", Azul))
            tabla.AddCell(KpiCell("Ticket promedio", "Q " & data.TicketPromedio.ToString("N2"), "Promedio por orden", Oro))
            tabla.AddCell(KpiCell("Clientes únicos", data.Clientes.ToString(), "Clientes atendidos", New BaseColor(111, 51, 160)))
            tabla.AddCell(KpiCell("Items vendidos", data.ItemsVendidos.ToString(), "Unidades registradas", Verde))
            tabla.AddCell(KpiCell("Canceladas", data.Canceladas.ToString(), "No concretadas", Rojo))

            doc.Add(tabla)
        End Sub

        Private Sub AgregarLecturaRapida(doc As Document, data As ReporteDashboard)

            Dim mejorMes As String = "Sin datos"
            Dim mejorMesTotal As Decimal = 0D

            If data.VentasMensuales IsNot Nothing AndAlso data.VentasMensuales.Count > 0 Then
                For Each item In data.VentasMensuales
                    If item.Total > mejorMesTotal Then
                        mejorMesTotal = item.Total
                        mejorMes = item.Mes
                    End If
                Next
            End If

            Dim tasaCancelacion As Decimal = 0D
            If data.Ordenes > 0 Then
                tasaCancelacion = (data.Canceladas / data.Ordenes) * 100D
            End If

            Dim productoLider As String = "Sin productos registrados"
            If data.ProductosTop IsNot Nothing AndAlso data.ProductosTop.Count > 0 Then
                productoLider = data.ProductosTop(0).Producto
            End If

            Dim lectura As New PdfPTable(2)
            lectura.WidthPercentage = 100
            lectura.SetWidths(New Single() {1.2F, 4.8F})
            lectura.SpacingAfter = 16

            Dim titulo As New PdfPCell(New Phrase("Lectura rápida", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE)))
            titulo.BackgroundColor = CafeOscuro
            titulo.Border = Rectangle.NO_BORDER
            titulo.Padding = 10
            titulo.Colspan = 2
            lectura.AddCell(titulo)

            AddLecturaCell(lectura, "Mejor mes", mejorMes & " · Q " & mejorMesTotal.ToString("N2"), Verde)
            AddLecturaCell(lectura, "Producto líder", productoLider, Oro)
            AddLecturaCell(lectura, "Cancelación", tasaCancelacion.ToString("N1") & "% del total de órdenes", Rojo)
            AddLecturaCell(lectura, "Actividad", data.Ordenes.ToString() & " órdenes y " & data.Clientes.ToString() & " clientes únicos", Azul)

            doc.Add(lectura)

        End Sub

        Private Sub AddLecturaCell(tabla As PdfPTable,
                           etiqueta As String,
                           valor As String,
                           color As BaseColor)

            Dim tag As New PdfPCell(New Phrase(etiqueta.ToUpper(), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7.5F, color)))
            tag.BackgroundColor = Crema
            tag.BorderColor = Pergamino
            tag.Padding = 7
            tag.VerticalAlignment = Element.ALIGN_MIDDLE
            tabla.AddCell(tag)

            Dim body As New PdfPCell(New Phrase("• " & valor, FontFactory.GetFont(FontFactory.HELVETICA, 8.5F, New BaseColor(55, 45, 35))))
            body.BackgroundColor = BaseColor.WHITE
            body.BorderColor = Pergamino
            body.Padding = 7
            body.VerticalAlignment = Element.ALIGN_MIDDLE
            tabla.AddCell(body)

        End Sub

        Private Sub AgregarVentasMensualesGrafico(doc As Document, data As ReporteDashboard)

            doc.Add(TituloSeccion("Tendencia mensual de ventas"))

            Dim contenedor As New PdfPTable(1)
            contenedor.WidthPercentage = 100
            contenedor.SpacingAfter = 18

            Dim celda As New PdfPCell()
            celda.FixedHeight = 190
            celda.BorderColor = Pergamino
            celda.Padding = 10
            celda.BackgroundColor = BaseColor.WHITE
            celda.CellEvent = New VentasMensualesChart(data.VentasMensuales, data.VentasTotales)

            contenedor.AddCell(celda)
            doc.Add(contenedor)

        End Sub

        Private Sub AgregarProductosTop(doc As Document, data As ReporteDashboard)

            If data.ProductosTop Is Nothing OrElse data.ProductosTop.Count = 0 Then
                Return
            End If

            doc.Add(TituloSeccion("Items vendidos principales"))

            Dim tabla As New PdfPTable(3)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {4.0F, 1.0F, 1.4F})
            tabla.SpacingAfter = 16

            AddHeaderCell(tabla, "Producto")
            AddHeaderCell(tabla, "Cant.")
            AddHeaderCell(tabla, "Ventas")

            For Each p In data.ProductosTop
                AddBodyCell(tabla, p.Producto)
                AddBodyCell(tabla, p.Cantidad.ToString())
                AddBodyCell(tabla, "Q " & p.Total.ToString("N2"))
            Next

            doc.Add(tabla)

        End Sub

        Private Function KpiCell(titulo As String, valor As String, subtexto As String, color As BaseColor) As PdfPCell

            Dim frase = New Phrase()

            frase.Add(New Chunk(titulo.ToUpper() & vbCrLf,
        FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7.5F, New BaseColor(105, 85, 65))))

            frase.Add(New Chunk(valor & vbCrLf,
        FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, CafeOscuro)))

            frase.Add(New Chunk(subtexto,
        FontFactory.GetFont(FontFactory.HELVETICA, 7.5F, New BaseColor(135, 110, 88))))

            Dim cell As New PdfPCell(frase)
            cell.BackgroundColor = BaseColor.WHITE
            cell.BorderColor = Pergamino
            cell.PaddingTop = 12
            cell.PaddingBottom = 12
            cell.PaddingLeft = 12
            cell.PaddingRight = 10
            cell.MinimumHeight = 72
            cell.BorderWidth = 0.6F
            cell.VerticalAlignment = Element.ALIGN_MIDDLE

            cell.CellEvent = New KpiAccent(color)

            Return cell

        End Function

        Private Sub AgregarVentasMensuales(doc As Document, data As ReporteDashboard)
            doc.Add(TituloSeccion("Tendencia mensual de ventas"))

            Dim tabla As New PdfPTable(3)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {2.2F, 2.2F, 1.2F})
            tabla.SpacingAfter = 16

            AddHeaderCell(tabla, "Mes")
            AddHeaderCell(tabla, "Ventas")
            AddHeaderCell(tabla, "Órdenes")

            For Each item In data.VentasMensuales
                AddBodyCell(tabla, item.Mes)
                AddBodyCell(tabla, "Q " & item.Total.ToString("N2"))
                AddBodyCell(tabla, item.Ordenes.ToString())
            Next

            doc.Add(tabla)
        End Sub

        Private Sub AgregarUltimasOrdenes(doc As Document, data As ReporteDashboard)

            doc.Add(TituloSeccion("Órdenes recientes del periodo"))
            doc.Add(ParrafoInfo("Información extraída directamente de las órdenes incluidas en el rango."))

            Dim tabla As New PdfPTable(5)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {1.45F, 1.05F, 1.8F, 1.55F, 1.0F})
            tabla.SpacingAfter = 12

            AddHeaderCell(tabla, "Orden")
            AddHeaderCell(tabla, "Fecha")
            AddHeaderCell(tabla, "Cliente")
            AddHeaderCell(tabla, "Estado")
            AddHeaderCell(tabla, "Total")

            Dim fila As Integer = 0

            For Each o In data.UltimasOrdenes
                Dim zebra As Boolean = fila Mod 2 = 1

                AddBodyCell(tabla, o.Codigo, 7, zebra)
                AddBodyCell(tabla, o.FechaOrden.ToString("dd/MM/yyyy"), 7, zebra)
                AddBodyCell(tabla, o.Cliente, 7, zebra)
                AddEstadoCell(tabla, o.Estado, zebra)
                AddBodyCell(tabla, "Q " & o.Total.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)

                fila += 1
            Next

            doc.Add(tabla)

        End Sub

        Private Sub AgregarVentasAgrupadas(doc As Document,
                                   data As ReporteDashboard,
                                   periodo As String,
                                   anio As Integer)

            doc.Add(TituloSeccion("Reporte de ventas agrupadas por tipo de mueble"))
            doc.Add(ParrafoInfo("Fecha generación: " & DateTime.Now.ToString("dd/MM/yyyy") &
                        " · Periodo: " & periodo &
                        " · Año: " & anio.ToString() &
                        " · Ciudad: Todas las ciudades"))

            If data.VentasAgrupadas Is Nothing OrElse data.VentasAgrupadas.Count = 0 Then
                doc.Add(ParrafoInfo("No se encontraron ventas agrupadas para el periodo seleccionado."))
                Return
            End If

            Dim tabla As New PdfPTable(6)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {1.1F, 1.2F, 3.5F, 0.9F, 1.25F, 1.25F})
            tabla.SpacingAfter = 14

            AddHeaderCell(tabla, "Tipo")
            AddHeaderCell(tabla, "Fecha")
            AddHeaderCell(tabla, "Nombre")
            AddHeaderCell(tabla, "Cant.")
            AddHeaderCell(tabla, "Costo unit.")
            AddHeaderCell(tabla, "Costo total")

            Dim fila As Integer = 0

            For Each v In data.VentasAgrupadas
                Dim zebra As Boolean = fila Mod 2 = 1

                AddBodyCell(tabla, If(String.IsNullOrWhiteSpace(v.Tipo), "GENERAL", v.Tipo), 7, zebra)
                AddBodyCell(tabla, If(v.Fecha.HasValue, v.Fecha.Value.ToString("dd/MM/yyyy"), "Sin fecha"), 7, zebra)
                AddBodyCell(tabla, v.Producto, 7, zebra)
                AddBodyCell(tabla, v.Cantidad.ToString(), 7, zebra, Element.ALIGN_CENTER)
                AddBodyCell(tabla, "Q " & v.CostoUnitario.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)
                AddBodyCell(tabla, "Q " & v.Total.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)

                fila += 1
            Next

            doc.Add(tabla)

            AgregarProductoMasVendido(doc, data)
            AgregarCierreCaja(doc, data)
        End Sub

        Private Sub AgregarProductoMasVendido(doc As Document, data As ReporteDashboard)

            If data.ProductoMasVendido Is Nothing Then
                Return
            End If

            doc.Add(TituloSeccion("Reporte del producto más vendido"))

            Dim porcentaje As Decimal = 0D

            If data.VentasTotales > 0 Then
                porcentaje = (data.ProductoMasVendido.Total / data.VentasTotales) * 100D
            End If

            Dim info As New PdfPTable(4)
            info.WidthPercentage = 100
            info.SetWidths(New Single() {1.3F, 2.0F, 1.5F, 3.2F})
            info.SpacingAfter = 8

            AddHeaderCell(info, "Fecha generación")
            AddHeaderCell(info, "Ciudad")
            AddHeaderCell(info, "Tipo mueble")
            AddHeaderCell(info, "Nombre")

            AddBodyCell(info, DateTime.Now.ToString("dd/MM/yyyy"), 7)
            AddBodyCell(info, "Todas las ciudades", 7)
            AddBodyCell(info, data.ProductoMasVendido.Tipo, 7)
            AddBodyCell(info, data.ProductoMasVendido.Producto, 7)

            doc.Add(info)

            Dim kpis As New PdfPTable(3)
            kpis.WidthPercentage = 100
            kpis.SetWidths(New Single() {1, 1, 1})
            kpis.SpacingAfter = 14

            kpis.AddCell(KpiCell("Cantidad vendida", data.ProductoMasVendido.Cantidad.ToString(), "Unidades", Verde))
            kpis.AddCell(KpiCell("Ventas generadas", "Q " & data.ProductoMasVendido.Total.ToString("N2"), "Contribución directa", Oro))
            kpis.AddCell(KpiCell("% del total", porcentaje.ToString("N1") & "%", "Participación", Azul))

            doc.Add(kpis)

        End Sub

        Private Sub AgregarCierreCaja(doc As Document, data As ReporteDashboard)

            If data.CierreCaja Is Nothing OrElse data.CierreCaja.Count = 0 Then
                Return
            End If

            doc.Add(TituloSeccion("Reporte de cierre de cajas"))
            doc.Add(ParrafoInfo("Totales por forma de pago dentro del periodo seleccionado"))

            Dim tabla As New PdfPTable(4)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {2.3F, 1.0F, 1.4F, 1.0F})
            tabla.SpacingAfter = 14

            AddHeaderCell(tabla, "Forma de pago")
            AddHeaderCell(tabla, "Órdenes")
            AddHeaderCell(tabla, "Total caja")
            AddHeaderCell(tabla, "% total")

            Dim fila As Integer = 0

            For Each c In data.CierreCaja
                Dim zebra As Boolean = fila Mod 2 = 1

                AddBodyCell(tabla, c.FormaPago, 8, zebra)
                AddBodyCell(tabla, c.Ordenes.ToString(), 8, zebra, Element.ALIGN_CENTER)
                AddBodyCell(tabla, "Q " & c.Total.ToString("N2"), 8, zebra, Element.ALIGN_RIGHT)
                AddBodyCell(tabla, c.Porcentaje.ToString("N1") & "%", 8, zebra, Element.ALIGN_RIGHT)

                fila += 1
            Next

            doc.Add(tabla)

        End Sub


        Private Function TituloSeccion(texto As String) As Paragraph
            Dim p As New Paragraph(texto, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, CafeOscuro))
            p.SpacingBefore = 10
            p.SpacingAfter = 6
            Return p
        End Function

        Private Function ParrafoInfo(texto As String) As Paragraph
            Dim p As New Paragraph(texto, FontFactory.GetFont(FontFactory.HELVETICA, 9, New BaseColor(120, 100, 80)))
            p.SpacingAfter = 8
            Return p
        End Function

        Private Sub AddHeaderCell(tabla As PdfPTable, texto As String)
            Dim cell As New PdfPCell(New Phrase(texto, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE)))
            cell.BackgroundColor = CafeOscuro
            cell.Padding = 6
            cell.BorderColor = Pergamino
            tabla.AddCell(cell)
        End Sub

        Private Sub AddBodyCell(tabla As PdfPTable,
                        texto As String,
                        Optional size As Integer = 9,
                        Optional zebra As Boolean = False,
                        Optional align As Integer = Element.ALIGN_LEFT)

            Dim cell As New PdfPCell(New Phrase(If(texto, ""), FontFactory.GetFont(FontFactory.HELVETICA, size, New BaseColor(55, 45, 35))))
            cell.PaddingTop = 5
            cell.PaddingBottom = 5
            cell.PaddingLeft = 6
            cell.PaddingRight = 6
            cell.BorderColor = Pergamino
            cell.BorderWidth = 0.5F
            cell.HorizontalAlignment = align
            cell.VerticalAlignment = Element.ALIGN_MIDDLE

            If zebra Then
                cell.BackgroundColor = Crema
            Else
                cell.BackgroundColor = BaseColor.WHITE
            End If

            tabla.AddCell(cell)

        End Sub


        Private Sub AddEstadoCell(tabla As PdfPTable,
                          estado As String,
                          Optional zebra As Boolean = False)

            Dim estadoTexto As String = If(estado, "No especificado")
            Dim estadoLower As String = estadoTexto.ToLower()

            Dim colorTexto As BaseColor = CafeMedio
            Dim fondo As BaseColor = If(zebra, Crema, BaseColor.WHITE)

            If estadoLower.Contains("entregado") Then
                colorTexto = New BaseColor(36, 120, 54)
                fondo = New BaseColor(232, 246, 235)
            ElseIf estadoLower.Contains("cancel") Then
                colorTexto = New BaseColor(150, 45, 45)
                fondo = New BaseColor(252, 232, 232)
            ElseIf estadoLower.Contains("pendiente") OrElse estadoLower.Contains("confirm") Then
                colorTexto = New BaseColor(163, 111, 25)
                fondo = New BaseColor(255, 246, 223)
            ElseIf estadoLower.Contains("fabric") OrElse estadoLower.Contains("proceso") Then
                colorTexto = New BaseColor(40, 95, 150)
                fondo = New BaseColor(229, 241, 252)
            End If

            Dim cell As New PdfPCell(New Phrase("● " & estadoTexto, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6.8F, colorTexto)))
            cell.PaddingTop = 5
            cell.PaddingBottom = 5
            cell.PaddingLeft = 6
            cell.PaddingRight = 6
            cell.BorderColor = Pergamino
            cell.BorderWidth = 0.5F
            cell.BackgroundColor = fondo
            cell.VerticalAlignment = Element.ALIGN_MIDDLE

            tabla.AddCell(cell)

        End Sub


        Private Function ExportarExcel(periodo As String,
                       anio As Integer,
                       mesInicio As Integer,
                       mesFin As Integer,
                       trimestre As Integer) As ActionResult

            Dim repo As New ReporteDatos()
            Dim data = repo.ObtenerReporte(periodo, anio, mesInicio, mesFin, trimestre)

            Using wb As New XLWorkbook()

                ' ===================== RESUMEN EJECUTIVO =====================
                Dim ws = wb.Worksheets.Add("Resumen")
                PrepararHojaExcel(ws)
                AgregarTituloExcel(ws, "MUEBLES DE LOS ALPES", "Reporte administrativo ejecutivo · " & periodo & " " & anio, 8)

                ws.Cell(5, 1).Value = "Periodo"
                ws.Cell(5, 2).Value = periodo
                ws.Cell(5, 3).Value = "Año"
                ws.Cell(5, 4).Value = anio
                ws.Cell(5, 5).Value = "Generado"
                ws.Cell(5, 6).Value = DateTime.Now
                ws.Cell(5, 6).Style.DateFormat.Format = "dd/mm/yyyy hh:mm"

                With ws.Range("A5:F5")
                    .Style.Fill.BackgroundColor = XLColor.FromHtml("#F7F3EE")
                    .Style.Font.FontColor = XLColor.FromHtml("#5C3D1E")
                    .Style.Font.Bold = True
                    .Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Style.Border.OutsideBorderColor = XLColor.FromHtml("#D8C8B4")
                End With

                CrearKpiExcel(ws, 7, 1, "Ventas totales", data.VentasTotales, "Q #,##0.00", "#0F8D70")
                CrearKpiExcel(ws, 7, 3, "Órdenes", data.Ordenes, "#,##0", "#2F7FC1")
                CrearKpiExcel(ws, 7, 5, "Clientes únicos", data.Clientes, "#,##0", "#D4A853")
                CrearKpiExcel(ws, 7, 7, "Ticket promedio", data.TicketPromedio, "Q #,##0.00", "#5C3D1E")

                CrearKpiExcel(ws, 11, 1, "Items vendidos", data.ItemsVendidos, "#,##0", "#0F8D70")
                CrearKpiExcel(ws, 11, 3, "Canceladas", data.Canceladas, "#,##0", "#9D3030")
                CrearKpiExcel(ws, 11, 5, "Valor por cliente", data.ValorPorCliente, "Q #,##0.00", "#2F7FC1")
                CrearKpiExcel(ws, 11, 7, "Recompra", data.PorcentajeRecompra / 100D, "0.0%", "#D4A853")

                ws.Cell(16, 1).Value = "Lectura rápida"
                EstiloTituloSeccionExcel(ws.Range("A16:H16"))

                Dim mejorMes As String = "Sin datos"
                Dim mejorMesTotal As Decimal = 0D

                If data.VentasMensuales IsNot Nothing AndAlso data.VentasMensuales.Count > 0 Then
                    For Each item In data.VentasMensuales
                        If item.Total > mejorMesTotal Then
                            mejorMesTotal = item.Total
                            mejorMes = item.Mes
                        End If
                    Next
                End If

                Dim productoLider As String = "Sin productos registrados"
                If data.ProductosTop IsNot Nothing AndAlso data.ProductosTop.Count > 0 Then
                    productoLider = data.ProductosTop(0).Producto
                End If

                ws.Cell(18, 1).Value = "Mejor mes"
                ws.Cell(18, 2).Value = mejorMes
                ws.Cell(18, 3).Value = "Ventas mejor mes"
                ws.Cell(18, 4).Value = mejorMesTotal
                ws.Cell(18, 5).Value = "Producto líder"
                ws.Cell(18, 6).Value = productoLider
                ws.Range("A18:F18").Style.Fill.BackgroundColor = XLColor.FromHtml("#F7F3EE")
                ws.Range("A18:F18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                ws.Range("A18:F18").Style.Border.OutsideBorderColor = XLColor.FromHtml("#D8C8B4")
                ws.Range("A18:F18").Style.Font.FontColor = XLColor.FromHtml("#2C1810")
                ws.Cell(18, 4).Style.NumberFormat.Format = "Q #,##0.00"

                ' ===================== VENTAS MENSUALES =====================
                Dim wsMeses = wb.Worksheets.Add("Ventas Mensuales")
                PrepararHojaExcel(wsMeses)
                AgregarTituloExcel(wsMeses, "VENTAS MENSUALES", "Tendencia de ventas y órdenes por mes", 4)

                wsMeses.Cell(5, 1).Value = "Mes"
                wsMeses.Cell(5, 2).Value = "Ventas"
                wsMeses.Cell(5, 3).Value = "Órdenes"

                Dim fila As Integer = 6
                If data.VentasMensuales IsNot Nothing Then
                    For Each item In data.VentasMensuales
                        wsMeses.Cell(fila, 1).Value = item.Mes
                        wsMeses.Cell(fila, 2).Value = item.Total
                        wsMeses.Cell(fila, 3).Value = item.Ordenes
                        fila += 1
                    Next
                End If

                EstilizarHoja(wsMeses, "A5:C" & Math.Max(fila - 1, 5).ToString(), 5)
                wsMeses.Column(2).Style.NumberFormat.Format = "Q #,##0.00"

                ' ===================== ÓRDENES =====================
                Dim wsOrdenes = wb.Worksheets.Add("Ordenes")
                PrepararHojaExcel(wsOrdenes)
                AgregarTituloExcel(wsOrdenes, "ÓRDENES RECIENTES", "Detalle de órdenes incluidas en el periodo", 5)

                wsOrdenes.Cell(5, 1).Value = "Orden"
                wsOrdenes.Cell(5, 2).Value = "Fecha"
                wsOrdenes.Cell(5, 3).Value = "Cliente"
                wsOrdenes.Cell(5, 4).Value = "Estado"
                wsOrdenes.Cell(5, 5).Value = "Total"

                fila = 6
                If data.UltimasOrdenes IsNot Nothing Then
                    For Each o In data.UltimasOrdenes
                        wsOrdenes.Cell(fila, 1).Value = o.Codigo
                        wsOrdenes.Cell(fila, 2).Value = o.FechaOrden
                        wsOrdenes.Cell(fila, 3).Value = o.Cliente
                        wsOrdenes.Cell(fila, 4).Value = o.Estado
                        wsOrdenes.Cell(fila, 5).Value = o.Total
                        AplicarEstadoExcel(wsOrdenes.Cell(fila, 4), o.Estado)
                        fila += 1
                    Next
                End If

                EstilizarHoja(wsOrdenes, "A5:E" & Math.Max(fila - 1, 5).ToString(), 5)
                wsOrdenes.Column(2).Style.DateFormat.Format = "dd/mm/yyyy"
                wsOrdenes.Column(5).Style.NumberFormat.Format = "Q #,##0.00"

                ' ===================== PRODUCTOS =====================
                Dim wsProductos = wb.Worksheets.Add("Productos")
                PrepararHojaExcel(wsProductos)
                AgregarTituloExcel(wsProductos, "RANKING DE PRODUCTOS", "Productos con mayor contribución en ventas", 6)

                wsProductos.Cell(5, 1).Value = "#"
                wsProductos.Cell(5, 2).Value = "Producto"
                wsProductos.Cell(5, 3).Value = "Tipo"
                wsProductos.Cell(5, 4).Value = "Cantidad"
                wsProductos.Cell(5, 5).Value = "Ventas"
                wsProductos.Cell(5, 6).Value = "% total"

                fila = 6
                Dim pos As Integer = 1

                If data.ProductosTop IsNot Nothing Then
                    For Each p In data.ProductosTop
                        Dim porcentaje As Decimal = 0D
                        If data.VentasTotales > 0 Then
                            porcentaje = p.Total / data.VentasTotales
                        End If

                        wsProductos.Cell(fila, 1).Value = pos
                        wsProductos.Cell(fila, 2).Value = p.Producto
                        wsProductos.Cell(fila, 3).Value = p.Tipo
                        wsProductos.Cell(fila, 4).Value = p.Cantidad
                        wsProductos.Cell(fila, 5).Value = p.Total
                        wsProductos.Cell(fila, 6).Value = porcentaje

                        fila += 1
                        pos += 1
                    Next
                End If

                EstilizarHoja(wsProductos, "A5:F" & Math.Max(fila - 1, 5).ToString(), 5)
                wsProductos.Column(5).Style.NumberFormat.Format = "Q #,##0.00"
                wsProductos.Column(6).Style.NumberFormat.Format = "0.0%"

                ' ===================== COMPRAS CLIENTE =====================
                Dim wsCompras = wb.Worksheets.Add("Compras Cliente")
                PrepararHojaExcel(wsCompras)
                AgregarTituloExcel(wsCompras, "COMPRAS POR CLIENTE", "Historial de compras del periodo", 5)

                wsCompras.Cell(5, 1).Value = "Fecha compra"
                wsCompras.Cell(5, 2).Value = "Cliente"
                wsCompras.Cell(5, 3).Value = "Valor"
                wsCompras.Cell(5, 4).Value = "Forma pago"
                wsCompras.Cell(5, 5).Value = "Muebles incluidos"

                fila = 6
                If data.ComprasPorCliente IsNot Nothing Then
                    For Each c In data.ComprasPorCliente
                        wsCompras.Cell(fila, 1).Value = c.FechaCompra
                        wsCompras.Cell(fila, 2).Value = c.Cliente
                        wsCompras.Cell(fila, 3).Value = c.Valor
                        wsCompras.Cell(fila, 4).Value = c.FormaPago
                        wsCompras.Cell(fila, 5).Value = c.MueblesIncluidos
                        fila += 1
                    Next
                End If

                EstilizarHoja(wsCompras, "A5:E" & Math.Max(fila - 1, 5).ToString(), 5)
                wsCompras.Column(1).Style.DateFormat.Format = "dd/mm/yyyy"
                wsCompras.Column(3).Style.NumberFormat.Format = "Q #,##0.00"
                wsCompras.Column(5).Style.Alignment.WrapText = True

                ' ===================== INDICADORES =====================
                Dim wsIndicadores = wb.Worksheets.Add("Indicadores")
                PrepararHojaExcel(wsIndicadores)
                AgregarTituloExcel(wsIndicadores, "INDICADORES COMERCIALES", "Métricas mensuales calculadas desde la base de datos", 5)

                wsIndicadores.Cell(5, 1).Value = "Mes"
                wsIndicadores.Cell(5, 2).Value = "Ventas"
                wsIndicadores.Cell(5, 3).Value = "Órdenes"
                wsIndicadores.Cell(5, 4).Value = "Clientes activos"
                wsIndicadores.Cell(5, 5).Value = "Ticket promedio"

                fila = 6
                If data.IndicadoresMensuales IsNot Nothing Then
                    For Each m In data.IndicadoresMensuales
                        wsIndicadores.Cell(fila, 1).Value = m.Mes
                        wsIndicadores.Cell(fila, 2).Value = m.Ventas
                        wsIndicadores.Cell(fila, 3).Value = m.Ordenes
                        wsIndicadores.Cell(fila, 4).Value = m.ClientesActivos
                        wsIndicadores.Cell(fila, 5).Value = m.TicketPromedio
                        fila += 1
                    Next
                End If

                EstilizarHoja(wsIndicadores, "A5:E" & Math.Max(fila - 1, 5).ToString(), 5)
                wsIndicadores.Column(2).Style.NumberFormat.Format = "Q #,##0.00"
                wsIndicadores.Column(5).Style.NumberFormat.Format = "Q #,##0.00"

                ' ===================== DETALLE PRODUCTOS =====================
                Dim wsDetalle = wb.Worksheets.Add("Detalle Productos")
                PrepararHojaExcel(wsDetalle)
                AgregarTituloExcel(wsDetalle, "DETALLE MENSUAL POR PRODUCTO", "Ventas por producto y participación mensual", 5)

                wsDetalle.Cell(5, 1).Value = "Mes"
                wsDetalle.Cell(5, 2).Value = "Producto"
                wsDetalle.Cell(5, 3).Value = "Cantidad"
                wsDetalle.Cell(5, 4).Value = "Ventas"
                wsDetalle.Cell(5, 5).Value = "% total"

                fila = 6
                If data.DetalleMensualProductos IsNot Nothing Then
                    For Each d In data.DetalleMensualProductos
                        wsDetalle.Cell(fila, 1).Value = d.Mes
                        wsDetalle.Cell(fila, 2).Value = d.Producto
                        wsDetalle.Cell(fila, 3).Value = d.Cantidad
                        wsDetalle.Cell(fila, 4).Value = d.Ventas
                        wsDetalle.Cell(fila, 5).Value = d.PorcentajeTotal / 100D
                        fila += 1
                    Next
                End If

                EstilizarHoja(wsDetalle, "A5:E" & Math.Max(fila - 1, 5).ToString(), 5)
                wsDetalle.Column(4).Style.NumberFormat.Format = "Q #,##0.00"
                wsDetalle.Column(5).Style.NumberFormat.Format = "0.0%"

                ' ===================== COHORTE =====================
                Dim wsCohorte = wb.Worksheets.Add("Cohorte")
                PrepararHojaExcel(wsCohorte)
                AgregarTituloExcel(wsCohorte, "ANÁLISIS DE COHORTE", "Recompra y retención de clientes por cohorte", 7)

                wsCohorte.Cell(5, 1).Value = "Cohorte"
                wsCohorte.Cell(5, 2).Value = "Clientes"
                wsCohorte.Cell(5, 3).Value = "M0"
                wsCohorte.Cell(5, 4).Value = "M+1"
                wsCohorte.Cell(5, 5).Value = "M+2"
                wsCohorte.Cell(5, 6).Value = "M+3"
                wsCohorte.Cell(5, 7).Value = "Ventas"

                fila = 6
                If data.Cohortes IsNot Nothing Then
                    For Each c In data.Cohortes
                        wsCohorte.Cell(fila, 1).Value = c.Cohorte
                        wsCohorte.Cell(fila, 2).Value = c.Clientes
                        wsCohorte.Cell(fila, 3).Value = c.M0
                        wsCohorte.Cell(fila, 4).Value = c.M1
                        wsCohorte.Cell(fila, 5).Value = c.M2
                        wsCohorte.Cell(fila, 6).Value = c.M3
                        wsCohorte.Cell(fila, 7).Value = c.Ventas
                        fila += 1
                    Next
                End If

                EstilizarHoja(wsCohorte, "A5:G" & Math.Max(fila - 1, 5).ToString(), 5)
                wsCohorte.Column(7).Style.NumberFormat.Format = "Q #,##0.00"

                For Each hoja In wb.Worksheets
                    hoja.Columns().AdjustToContents()
                    hoja.Rows().AdjustToContents()
                    hoja.SheetView.FreezeRows(5)
                    hoja.Style.Font.FontName = "Segoe UI"

                    For c As Integer = 1 To hoja.LastColumnUsed().ColumnNumber()
                        If hoja.Column(c).Width > 42 Then
                            hoja.Column(c).Width = 42
                        End If
                    Next
                Next

                Using ms As New MemoryStream()
                    wb.SaveAs(ms)

                    Dim bytes = ms.ToArray()
                    Dim nombre = "reporte_muebles_de_los_alpes_" & periodo & "_" & anio & ".xlsx"

                    Return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nombre)
                End Using
            End Using
        End Function

        Private Sub PrepararHojaExcel(ws As IXLWorksheet)
            ws.Style.Font.FontName = "Segoe UI"
            ws.Style.Font.FontSize = 10
            ws.Style.Fill.BackgroundColor = XLColor.FromHtml("#FBF7F1")
            ws.ShowGridLines = False
        End Sub

        Private Sub AgregarTituloExcel(ws As IXLWorksheet,
                               titulo As String,
                               subtitulo As String,
                               ultimaColumna As Integer)

            ws.Range(1, 1, 1, ultimaColumna).Merge()
            ws.Cell(1, 1).Value = titulo

            With ws.Range(1, 1, 1, ultimaColumna)
                .Style.Fill.BackgroundColor = XLColor.FromHtml("#2C1810")
                .Style.Font.FontColor = XLColor.White
                .Style.Font.Bold = True
                .Style.Font.FontSize = 20
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                .Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            End With

            ws.Row(1).Height = 36

            ws.Range(2, 1, 2, ultimaColumna).Merge()
            ws.Cell(2, 1).Value = subtitulo

            With ws.Range(2, 1, 2, ultimaColumna)
                .Style.Fill.BackgroundColor = XLColor.FromHtml("#2C1810")
                .Style.Font.FontColor = XLColor.FromHtml("#EEDCC3")
                .Style.Font.Bold = True
                .Style.Font.FontSize = 11
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                .Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            End With

            ws.Row(2).Height = 24

            ws.Range(3, 1, 3, ultimaColumna).Merge()
            ws.Cell(3, 1).Value = ""

            With ws.Range(3, 1, 3, ultimaColumna)
                .Style.Fill.BackgroundColor = XLColor.FromHtml("#D4A853")
            End With

            ws.Row(3).Height = 4
        End Sub

        Private Sub CrearKpiExcel(ws As IXLWorksheet,
                          fila As Integer,
                          col As Integer,
                          titulo As String,
                          valor As Object,
                          formato As String,
                          colorHex As String)

            Dim rango = ws.Range(fila, col, fila + 2, col + 1)
            rango.Merge()

            With rango
                .Style.Fill.BackgroundColor = XLColor.White
                .Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                .Style.Border.OutsideBorderColor = XLColor.FromHtml("#D8C8B4")
                .Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            End With

            ws.Cell(fila, col).Value = titulo.ToUpper()
            ws.Cell(fila, col).Style.Font.FontColor = XLColor.FromHtml("#8A6A45")
            ws.Cell(fila, col).Style.Font.Bold = True
            ws.Cell(fila, col).Style.Font.FontSize = 9

            If TypeOf valor Is Decimal Then
                ws.Cell(fila + 1, col).Value = Convert.ToDecimal(valor)
            ElseIf TypeOf valor Is Double Then
                ws.Cell(fila + 1, col).Value = Convert.ToDouble(valor)
            ElseIf TypeOf valor Is Integer Then
                ws.Cell(fila + 1, col).Value = Convert.ToInt32(valor)
            Else
                ws.Cell(fila + 1, col).Value = valor.ToString()
            End If
            ws.Cell(fila + 1, col).Style.Font.FontColor = XLColor.FromHtml("#2C1810")
            ws.Cell(fila + 1, col).Style.Font.Bold = True
            ws.Cell(fila + 1, col).Style.Font.FontSize = 18
            ws.Cell(fila + 1, col).Style.NumberFormat.Format = formato

            ws.Cell(fila + 2, col).Value = "Indicador del periodo"
            ws.Cell(fila + 2, col).Style.Font.FontColor = XLColor.FromHtml("#9A7B55")
            ws.Cell(fila + 2, col).Style.Font.FontSize = 8

            ws.Range(fila, col, fila + 2, col).Style.Border.LeftBorder = XLBorderStyleValues.Thick
            ws.Range(fila, col, fila + 2, col).Style.Border.LeftBorderColor = XLColor.FromHtml(colorHex)
        End Sub

        Private Sub EstiloTituloSeccionExcel(rango As IXLRange)
            rango.Merge()

            With rango
                .Style.Fill.BackgroundColor = XLColor.FromHtml("#2C1810")
                .Style.Font.FontColor = XLColor.White
                .Style.Font.Bold = True
                .Style.Font.FontSize = 12
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                .Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            End With

            rango.FirstRow().WorksheetRow().Height = 26
        End Sub

        Private Sub EstilizarHoja(ws As IXLWorksheet, rango As String, headerRow As Integer)

            Dim usedRange = ws.Range(rango)

            usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
            usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin
            usedRange.Style.Border.OutsideBorderColor = XLColor.FromHtml("#D8C8B4")
            usedRange.Style.Border.InsideBorderColor = XLColor.FromHtml("#E8DED0")
            usedRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            Dim header = ws.Range(headerRow, usedRange.FirstColumn().ColumnNumber(), headerRow, usedRange.LastColumn().ColumnNumber())

            With header
                .Style.Font.Bold = True
                .Style.Font.FontColor = XLColor.White
                .Style.Fill.BackgroundColor = XLColor.FromHtml("#2C1810")
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                .Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            End With

            ws.Row(headerRow).Height = 24

            Dim primeraData As Integer = headerRow + 1
            Dim ultimaData As Integer = usedRange.LastRow().RowNumber()
            Dim primeraCol As Integer = usedRange.FirstColumn().ColumnNumber()
            Dim ultimaCol As Integer = usedRange.LastColumn().ColumnNumber()

            If ultimaData >= primeraData Then
                For r As Integer = primeraData To ultimaData
                    If r Mod 2 = 0 Then
                        ws.Range(r, primeraCol, r, ultimaCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#FCF8F2")
                    Else
                        ws.Range(r, primeraCol, r, ultimaCol).Style.Fill.BackgroundColor = XLColor.White
                    End If
                Next
            End If

            If usedRange.RowCount() > 1 Then
                usedRange.SetAutoFilter()
            End If
        End Sub

        Private Sub AplicarEstadoExcel(cell As IXLCell, estado As String)
            Dim estadoTexto As String = If(estado, "").ToLower()

            cell.Style.Font.Bold = True

            If estadoTexto.Contains("entregado") Then
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F6EB")
                cell.Style.Font.FontColor = XLColor.FromHtml("#247836")
            ElseIf estadoTexto.Contains("cancel") Then
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FCE8E8")
                cell.Style.Font.FontColor = XLColor.FromHtml("#963030")
            ElseIf estadoTexto.Contains("pendiente") OrElse estadoTexto.Contains("confirm") Then
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFF6DF")
                cell.Style.Font.FontColor = XLColor.FromHtml("#A36F19")
            ElseIf estadoTexto.Contains("fabric") OrElse estadoTexto.Contains("proceso") Then
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#E5F1FC")
                cell.Style.Font.FontColor = XLColor.FromHtml("#285F96")
            End If
        End Sub


        Private Class KpiAccent
            Implements IPdfPCellEvent

            Private ReadOnly _color As BaseColor

            Public Sub New(color As BaseColor)
                _color = color
            End Sub

            Public Sub CellLayout(cell As PdfPCell, position As Rectangle, canvases() As PdfContentByte) Implements IPdfPCellEvent.CellLayout
                Dim cb = canvases(PdfPTable.LINECANVAS)
                cb.SaveState()
                cb.SetColorStroke(_color)
                cb.SetLineWidth(4)
                cb.MoveTo(position.Left + 10, position.Top - 9)
                cb.LineTo(position.Left + 42, position.Top - 9)
                cb.Stroke()
                cb.RestoreState()
            End Sub
        End Class


        Private Class VentasMensualesChart
            Implements IPdfPCellEvent

            Private ReadOnly _ventas As List(Of VentaMensualViewModel)
            Private ReadOnly _total As Decimal

            Public Sub New(ventas As List(Of VentaMensualViewModel), total As Decimal)
                _ventas = ventas
                _total = total
            End Sub

            Public Sub CellLayout(cell As PdfPCell,
                          position As Rectangle,
                          canvases() As PdfContentByte) Implements IPdfPCellEvent.CellLayout

                Dim cb = canvases(PdfPTable.BACKGROUNDCANVAS)

                Dim fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, CafeOscuro)
                Dim fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 7, New BaseColor(95, 75, 55))
                Dim fontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, CafeOscuro)
                Dim fontTotal = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, Verde)

                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT,
                                   New Phrase("Ventas por mes", fontTitulo),
                                   position.Left + 14,
                                   position.Top - 18,
                                   0)

                ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT,
                                   New Phrase("Total: Q " & _total.ToString("N2"), fontTotal),
                                   position.Right - 14,
                                   position.Top - 18,
                                   0)

                If _ventas Is Nothing OrElse _ventas.Count = 0 Then
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER,
                                       New Phrase("Sin datos para el periodo seleccionado", fontSmall),
                                       position.Left + (position.Width / 2),
                                       position.Bottom + 85,
                                       0)
                    Return
                End If

                Dim maximo As Decimal = _ventas.Max(Function(x) x.Total)
                If maximo <= 0 Then maximo = 1D

                Dim chartLeft As Single = position.Left + 26
                Dim chartBottom As Single = position.Bottom + 42
                Dim chartWidth As Single = position.Width - 52
                Dim chartHeight As Single = 96
                Dim espacio As Single = chartWidth / _ventas.Count

                cb.SaveState()
                cb.SetColorStroke(New BaseColor(238, 230, 218))
                cb.SetLineWidth(0.5F)

                For g As Integer = 0 To 3
                    Dim gy As Single = chartBottom + (chartHeight / 3.0F * g)
                    cb.MoveTo(chartLeft, gy)
                    cb.LineTo(chartLeft + chartWidth, gy)
                Next

                cb.Stroke()
                cb.RestoreState()

                For i As Integer = 0 To _ventas.Count - 1
                    Dim item = _ventas(i)
                    Dim altoBarra As Single = CSng(chartHeight * (item.Total / maximo))

                    If item.Total > 0 AndAlso altoBarra < 16 Then
                        altoBarra = 16
                    End If

                    Dim barWidth As Single = Math.Min(34, espacio * 0.38F)
                    Dim x As Single = chartLeft + (i * espacio) + ((espacio - barWidth) / 2)
                    Dim y As Single = chartBottom

                    Dim colorBarra As BaseColor = If(item.Total = maximo, Verde, If(i Mod 2 = 0, Verde, New BaseColor(199, 143, 34)))

                    cb.SaveState()
                    cb.SetColorFill(colorBarra)
                    cb.RoundRectangle(x, y, barWidth, altoBarra, 7)
                    cb.Fill()
                    cb.RestoreState()

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER,
                                       New Phrase("Q " & item.Total.ToString("N0"), fontSmall),
                                       x + (barWidth / 2),
                                       y + altoBarra + 8,
                                       0)

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER,
                                       New Phrase(MesCorto(item.Mes), fontBold),
                                       x + (barWidth / 2),
                                       position.Bottom + 24,
                                       0)

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER,
                                       New Phrase(item.Ordenes.ToString() & " ord.", fontSmall),
                                       x + (barWidth / 2),
                                       position.Bottom + 13,
                                       0)
                Next
            End Sub

            Private Function MesCorto(mes As String) As String
                If String.IsNullOrWhiteSpace(mes) Then Return "-"
                mes = mes.Trim()
                If mes.Length <= 3 Then Return mes
                Return mes.Substring(0, 3)
            End Function

        End Class


        Private Sub AgregarPaginaComprasIndicadores(doc As Document, data As ReporteDashboard)

            doc.Add(TituloSeccion("Compras realizadas por cliente"))
            doc.Add(ParrafoInfo("Órdenes del periodo ordenadas por fecha de compra"))

            Dim tablaCompras As New PdfPTable(5)
            tablaCompras.WidthPercentage = 100
            tablaCompras.SetWidths(New Single() {1.1F, 2.1F, 1.1F, 1.3F, 3.2F})
            tablaCompras.SpacingAfter = 14

            AddHeaderCell(tablaCompras, "Fecha compra")
            AddHeaderCell(tablaCompras, "Cliente")
            AddHeaderCell(tablaCompras, "Valor")
            AddHeaderCell(tablaCompras, "Forma pago")
            AddHeaderCell(tablaCompras, "Muebles incluidos")

            If data.ComprasPorCliente IsNot Nothing Then
                Dim fila As Integer = 0

                For Each c In data.ComprasPorCliente
                    Dim zebra As Boolean = fila Mod 2 = 1

                    AddBodyCell(tablaCompras, c.FechaCompra.ToString("dd/MM/yyyy"), 6.5F, zebra)
                    AddBodyCell(tablaCompras, c.Cliente, 6.5F, zebra)
                    AddBodyCell(tablaCompras, "Q " & c.Valor.ToString("N2"), 6.5F, zebra, Element.ALIGN_RIGHT)
                    AddBodyCell(tablaCompras, c.FormaPago, 6.5F, zebra)
                    AddBodyCell(tablaCompras, c.MueblesIncluidos, 6.2F, zebra)

                    fila += 1
                Next
            End If

            doc.Add(tablaCompras)

            doc.Add(TituloSeccion("Indicadores comerciales reales"))
            doc.Add(ParrafoInfo("Métricas calculadas únicamente con órdenes, clientes y ventas reales de la base de datos"))

            Dim indicadores As New PdfPTable(3)
            indicadores.WidthPercentage = 100
            indicadores.SetWidths(New Single() {1, 1, 1})
            indicadores.SpacingAfter = 14

            indicadores.AddCell(KpiCell("Valor por cliente", "Q " & data.ValorPorCliente.ToString("N2"), "Ventas reales / clientes únicos", Verde))
            indicadores.AddCell(KpiCell("Actividad comercial", data.Ordenes.ToString(), "Órdenes reales del periodo", Azul))
            indicadores.AddCell(KpiCell("Recompra", data.PorcentajeRecompra.ToString("N1") & "%", "Clientes con más de una orden", New BaseColor(111, 51, 160)))

            doc.Add(indicadores)

            Dim tablaMensual As New PdfPTable(5)
            tablaMensual.WidthPercentage = 100
            tablaMensual.SetWidths(New Single() {1.4F, 1.4F, 1.1F, 1.5F, 1.6F})
            tablaMensual.SpacingAfter = 12

            AddHeaderCell(tablaMensual, "Mes")
            AddHeaderCell(tablaMensual, "Ventas")
            AddHeaderCell(tablaMensual, "Órdenes")
            AddHeaderCell(tablaMensual, "Clientes activos")
            AddHeaderCell(tablaMensual, "Ticket promedio")

            If data.IndicadoresMensuales IsNot Nothing Then
                Dim filaMes As Integer = 0

                For Each m In data.IndicadoresMensuales
                    Dim zebra As Boolean = filaMes Mod 2 = 1

                    AddBodyCell(tablaMensual, m.Mes, 7, zebra)
                    AddBodyCell(tablaMensual, "Q " & m.Ventas.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)
                    AddBodyCell(tablaMensual, m.Ordenes.ToString(), 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tablaMensual, m.ClientesActivos.ToString(), 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tablaMensual, "Q " & m.TicketPromedio.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)

                    filaMes += 1
                Next
            End If

            doc.Add(tablaMensual)

        End Sub


        Private Sub AgregarPaginaRankingProductos(doc As Document, data As ReporteDashboard)

            doc.Add(TituloSeccion("Ranking de productos"))
            doc.Add(ParrafoInfo("Productos con mayor contribución dentro del periodo"))

            Dim tablaRanking As New PdfPTable(5)
            tablaRanking.WidthPercentage = 100
            tablaRanking.SetWidths(New Single() {0.45F, 3.8F, 1.0F, 1.4F, 1.0F})
            tablaRanking.SpacingAfter = 16

            AddHeaderCell(tablaRanking, "#")
            AddHeaderCell(tablaRanking, "Producto")
            AddHeaderCell(tablaRanking, "Cantidad")
            AddHeaderCell(tablaRanking, "Ventas")
            AddHeaderCell(tablaRanking, "% total")

            If data.ProductosTop IsNot Nothing Then
                Dim posicion As Integer = 1

                For Each p In data.ProductosTop
                    Dim porcentaje As Decimal = 0D
                    If data.VentasTotales > 0 Then
                        porcentaje = Math.Round((p.Total / data.VentasTotales) * 100D, 1)
                    End If

                    Dim zebra As Boolean = posicion Mod 2 = 0

                    AddBodyCell(tablaRanking, posicion.ToString(), 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tablaRanking, p.Producto, 7, zebra)
                    AddBodyCell(tablaRanking, p.Cantidad.ToString(), 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tablaRanking, "Q " & p.Total.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)
                    AddBodyCell(tablaRanking, porcentaje.ToString("N1") & "%", 7, zebra, Element.ALIGN_RIGHT)

                    posicion += 1
                Next
            End If

            doc.Add(tablaRanking)

            doc.Add(TituloSeccion("Detalle mensual por producto"))
            doc.Add(ParrafoInfo("Ventas calculadas contra el total real de las órdenes del periodo"))

            Dim tablaDetalle As New PdfPTable(5)
            tablaDetalle.WidthPercentage = 100
            tablaDetalle.SetWidths(New Single() {1.1F, 4.0F, 0.8F, 1.4F, 1.0F})
            tablaDetalle.SpacingAfter = 12

            AddHeaderCell(tablaDetalle, "Mes")
            AddHeaderCell(tablaDetalle, "Producto")
            AddHeaderCell(tablaDetalle, "Cant.")
            AddHeaderCell(tablaDetalle, "Ventas")
            AddHeaderCell(tablaDetalle, "% total")

            If data.DetalleMensualProductos IsNot Nothing Then
                Dim fila As Integer = 0

                For Each d In data.DetalleMensualProductos
                    Dim zebra As Boolean = fila Mod 2 = 1

                    AddBodyCell(tablaDetalle, d.Mes, 7, zebra)
                    AddBodyCell(tablaDetalle, d.Producto, 7, zebra)
                    AddBodyCell(tablaDetalle, d.Cantidad.ToString(), 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tablaDetalle, "Q " & d.Ventas.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)
                    AddBodyCell(tablaDetalle, d.PorcentajeTotal.ToString("N1") & "%", 7, zebra, Element.ALIGN_RIGHT)

                    fila += 1
                Next
            End If

            doc.Add(tablaDetalle)

        End Sub

        Private Sub AgregarPaginaCohorte(doc As Document, data As ReporteDashboard)

            doc.Add(TituloSeccion("Reporte de cohorte"))
            doc.Add(ParrafoInfo("Clientes agrupados por primera orden observada en el periodo y recompra posterior calculada con órdenes reales"))

            Dim kpis As New PdfPTable(4)
            kpis.WidthPercentage = 100
            kpis.SetWidths(New Single() {1, 1, 1, 1})
            kpis.SpacingAfter = 14

            kpis.AddCell(KpiCell("Clientes cohorte", data.ClientesCohorte.ToString(), "Clientes reales agrupados", Verde))
            kpis.AddCell(KpiCell("Retención M+1", data.RetencionM1.ToString("N1") & "%", "Recompra al mes siguiente", Azul))
            kpis.AddCell(KpiCell("Retención M+2", data.RetencionM2.ToString("N1") & "%", "Clientes activos dos meses después", Oro))
            kpis.AddCell(KpiCell("Venta cohorte", "Q " & data.VentaCohorte.ToString("N2"), "Ingresos reales M0-M3", New BaseColor(111, 51, 160)))

            doc.Add(kpis)

            doc.Add(TituloSeccion("Gráfica de retención por cohorte"))

            Dim grafica As New PdfPTable(1)
            grafica.WidthPercentage = 100
            grafica.SpacingAfter = 16

            Dim celdaGrafica As New PdfPCell()
            celdaGrafica.FixedHeight = 120
            celdaGrafica.BorderColor = Pergamino
            celdaGrafica.BackgroundColor = BaseColor.WHITE
            celdaGrafica.CellEvent = New RetencionCohorteChart(data.RetencionM1, data.RetencionM2)

            grafica.AddCell(celdaGrafica)
            doc.Add(grafica)

            doc.Add(TituloSeccion("Mapa de cohorte por recompra mensual"))

            Dim tabla As New PdfPTable(7)
            tabla.WidthPercentage = 100
            tabla.SetWidths(New Single() {1.7F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.5F})
            tabla.SpacingAfter = 12

            AddHeaderCell(tabla, "Cohorte")
            AddHeaderCell(tabla, "Clientes")
            AddHeaderCell(tabla, "M0")
            AddHeaderCell(tabla, "M+1")
            AddHeaderCell(tabla, "M+2")
            AddHeaderCell(tabla, "M+3")
            AddHeaderCell(tabla, "Ventas")

            If data.Cohortes IsNot Nothing Then
                Dim fila As Integer = 0

                For Each c In data.Cohortes
                    Dim zebra As Boolean = fila Mod 2 = 1

                    AddBodyCell(tabla, c.Cohorte, 7, zebra)
                    AddBodyCell(tabla, c.Clientes.ToString(), 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tabla, c.M0.ToString() & vbCrLf & "100.0%", 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tabla, c.M1.ToString() & vbCrLf & "0.0%", 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tabla, c.M2.ToString() & vbCrLf & "0.0%", 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tabla, c.M3.ToString() & vbCrLf & "0.0%", 7, zebra, Element.ALIGN_CENTER)
                    AddBodyCell(tabla, "Q " & c.Ventas.ToString("N2"), 7, zebra, Element.ALIGN_RIGHT)

                    fila += 1
                Next
            End If

            doc.Add(tabla)

            doc.Add(ParrafoInfo("Validación general: este reporte utiliza únicamente datos reales disponibles en la base de datos para el rango seleccionado. Las ventas, órdenes, clientes, productos, cierre de caja, métricas comerciales y cohorte se calculan desde órdenes, detalles, totales y fechas registradas; cuando un campo no existe, se muestra como no especificado."))

        End Sub

        Private Class RetencionCohorteChart
            Implements IPdfPCellEvent

            Private ReadOnly _m1 As Decimal
            Private ReadOnly _m2 As Decimal

            Public Sub New(m1 As Decimal, m2 As Decimal)
                _m1 = m1
                _m2 = m2
            End Sub

            Public Sub CellLayout(cell As PdfPCell,
                          position As Rectangle,
                          canvases() As PdfContentByte) Implements IPdfPCellEvent.CellLayout

                Dim cb = canvases(PdfPTable.BACKGROUNDCANVAS)
                Dim font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, CafeOscuro)
                Dim fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 7, New BaseColor(95, 75, 55))

                Dim labels() As String = {"Mes 0", "Mes +1", "Mes +2", "Mes +3"}
                Dim values() As Decimal = {100D, _m1, _m2, 0D}

                Dim left As Single = position.Left + 24
                Dim top As Single = position.Top - 24
                Dim barMaxWidth As Single = position.Width - 115
                Dim barHeight As Single = 14

                For i As Integer = 0 To labels.Length - 1
                    Dim y As Single = top - (i * 24)
                    Dim width As Single = CSng(barMaxWidth * (values(i) / 100D))

                    cb.SaveState()
                    cb.SetColorFill(New BaseColor(244, 238, 229))
                    cb.RoundRectangle(left + 60, y - 3, barMaxWidth, barHeight, 5)
                    cb.Fill()
                    cb.RestoreState()

                    cb.SaveState()
                    cb.SetColorFill(If(i = 0, Verde, Oro))
                    cb.RoundRectangle(left + 60, y - 3, Math.Max(width, 2), barHeight, 5)
                    cb.Fill()
                    cb.RestoreState()

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT,
                                       New Phrase(labels(i), font),
                                       left,
                                       y,
                                       0)

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT,
                                       New Phrase(values(i).ToString("N1") & "%", fontSmall),
                                       position.Right - 24,
                                       y,
                                       0)
                Next

            End Sub
        End Class

        Private Class ReporteFooter
            Inherits PdfPageEventHelper

            Public Overrides Sub OnEndPage(writer As PdfWriter, document As Document)
                Dim cb = writer.DirectContent
                Dim font = FontFactory.GetFont(FontFactory.HELVETICA, 8, New BaseColor(120, 100, 80))
                ColumnText.ShowTextAligned(cb,
                                           Element.ALIGN_LEFT,
                                           New Phrase("Muebles de los Alpes · Reporte administrativo", font),
                                           document.LeftMargin,
                                           document.PageSize.GetBottom(20),
                                           0)
                ColumnText.ShowTextAligned(cb,
                                           Element.ALIGN_RIGHT,
                                           New Phrase("Página " & writer.PageNumber.ToString(), font),
                                           document.PageSize.GetRight(document.RightMargin),
                                           document.PageSize.GetBottom(20),
                                           0)
            End Sub
        End Class

    End Class

End Namespace