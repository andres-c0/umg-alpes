Option Strict On
Option Explicit On

Imports System.Web.Mvc
Imports System.Linq
Imports Alpes.Servicios.Servicios


Namespace Controllers
    Public Class AdminController
        Inherits Controller

        ' =========================
        ' DASHBOARD
        ' =========================
        Function Index() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' PRODUCTOS
        ' =========================
        Function Productos() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' ÓRDENES
        ' =========================
        Function Ordenes() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' CLIENTES
        ' =========================
        Function Clientes() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' INVENTARIO
        ' =========================
        Function Inventario() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' REPORTES
        ' =========================
        Function Reportes() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' PROVEEDORES
        ' =========================
        Function Proveedores() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' COMPRAS
        ' =========================
        Function Compras() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function


        ' =========================
        ' EMPLEADOS
        ' =========================
        Function Empleados() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' NOMINA
        ' =========================
        Function Nomina() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' MARKETING
        ' =========================
        Function Marketing() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' PRODUCCIÓN
        ' =========================
        Function Produccion() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        Function Configuracion() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' VALIDACIÓN DE ADMIN
        ' =========================
        Private Function EsAdmin() As Boolean
            If Session("UsuarioId") Is Nothing Then
                Return False
            End If

            If Session("RolId") Is Nothing Then
                Return False
            End If

            Dim rolId As Integer = 0
            Integer.TryParse(Session("RolId").ToString(), rolId)

            ' 👇 AJUSTA ESTE VALOR SEGÚN TU BD
            ' Si RolId = 1 es ADMIN, entonces:
            Return rolId = 1
        End Function
        <HttpGet>
        Function DashboardData() As JsonResult
            Try
                Dim ordenVentaSrv As New Orden_VentaServicio()
                Dim ordenCompraSrv As New Orden_CompraServicio()
                Dim clienteSrv As New ClienteServicio()
                Dim inventarioSrv As New Inventario_ProductoServicio()
                Dim nominaSrv As New NominaServicio()

                Dim ventas = ordenVentaSrv.Listar()
                Dim compras = ordenCompraSrv.Listar()
                Dim clientes = clienteSrv.Listar()
                Dim inventario = inventarioSrv.Listar()
                Dim nominas = nominaSrv.Listar()

                Dim hoy = DateTime.Now
                Dim mesActual = hoy.Month
                Dim anioActual = hoy.Year

                Dim ventasMes As Decimal = ventas _
                    .Where(Function(x) x.FechaOrden.Year = anioActual AndAlso x.FechaOrden.Month = mesActual) _
                    .Select(Function(x) x.Total) _
                    .DefaultIfEmpty(0D) _
                    .Sum()

                Dim comprasMes As Decimal = compras _
                    .Where(Function(x) x.FechaOc.Year = anioActual AndAlso x.FechaOc.Month = mesActual) _
                    .Select(Function(x) x.Total) _
                    .DefaultIfEmpty(0D) _
                    .Sum()

                Dim ordenesActivas As Integer = ventas _
    .Where(Function(x) x.Estado IsNot Nothing AndAlso x.Estado.ToUpper() = "ACTIVO") _
    .Count()

                Dim stockBajo As Integer = inventario _
    .Where(Function(x) x.StockMinimo.HasValue AndAlso x.Stock <= x.StockMinimo.Value) _
    .Count()

                Dim nominasPendientes As Integer = nominas _
    .Where(Function(x) x.Estado IsNot Nothing AndAlso x.Estado.ToUpper() = "PENDIENTE") _
    .Count()

                Dim ventasPorMes = ventas _
                    .GroupBy(Function(x) x.FechaOrden.Month) _
                    .Select(Function(g) New With {
                        .mes = g.Key,
                        .total = g.Sum(Function(x) x.Total)
                    }) _
                    .OrderBy(Function(x) x.mes) _
                    .ToList()

                Dim comprasPorMes = compras _
                    .GroupBy(Function(x) x.FechaOc.Month) _
                    .Select(Function(g) New With {
                        .mes = g.Key,
                        .total = g.Sum(Function(x) x.Total)
                    }) _
                    .OrderBy(Function(x) x.mes) _
                    .ToList()

                Return Json(New With {
                    .ventasMes = ventasMes,
                    .comprasMes = comprasMes,
                    .clientes = clientes.Count,
                    .ordenesActivas = ordenesActivas,
                    .stockBajo = stockBajo,
                    .nominasPendientes = nominasPendientes,
                    .ventasPorMes = ventasPorMes,
                    .comprasPorMes = comprasPorMes
                }, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function


    End Class
End Namespace