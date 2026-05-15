Option Strict On
Option Explicit On

Imports System
Imports System.Linq
Imports System.Web.Mvc
Imports Alpes.Entidades.Marketing
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades

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




        Function Perfil() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            ViewData("Title") = "Mi perfil"
            Return View()
        End Function


        ' =========================
        ' VALIDACIÓN DE ADMIN
        ' =========================
        Private Function EsAdmin() As Boolean
            If Session("UsuarioId") Is Nothing OrElse Session("RolId") Is Nothing Then
                Return False
            End If

            Dim nombreRolSesion As String = If(Session("RolNombre") Is Nothing, String.Empty, Session("RolNombre").ToString())

            If EsNombreRolAdministrativo(nombreRolSesion) Then
                Return True
            End If

            Dim rolId As Integer = 0
            Integer.TryParse(Session("RolId").ToString(), rolId)

            Dim nombreRolBd As String = ObtenerNombreRolPorId(rolId)

            If EsNombreRolAdministrativo(nombreRolBd) Then
                Session("RolNombre") = nombreRolBd
                Return True
            End If

            Return False
        End Function

        Private Function EsNombreRolAdministrativo(ByVal nombreRol As String) As Boolean
            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return False
            End If

            Dim rolNormalizado As String = nombreRol.Trim().ToUpperInvariant()

            ' REGLA:
            ' CLIENTE no puede entrar al panel admin.
            ' Cualquier otro rol activo sí puede entrar al panel admin.
            Return Not (rolNormalizado = "CLIENTE" OrElse rolNormalizado.Contains("CLIENTE"))
        End Function

        Private Function ObtenerNombreRolPorId(ByVal rolId As Integer) As String
            If rolId <= 0 Then
                Return String.Empty
            End If

            Try
                Dim rolSrv As New RolServicio()
                Dim rol = rolSrv.ObtenerPorId(rolId)

                If rol IsNot Nothing AndAlso
                   rol.RolNombre IsNot Nothing AndAlso
                   (rol.Estado Is Nothing OrElse String.Equals(rol.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase)) Then
                    Return rol.RolNombre.Trim()
                End If
            Catch
                Return String.Empty
            End Try

            Return String.Empty
        End Function


        <HttpGet>
        Function ConfiguracionData() As JsonResult
            Try
                If Not EsAdmin() Then
                    Response.StatusCode = 401
                    Return Json(New With {
                .success = False,
                .message = "No autorizado."
            }, JsonRequestBehavior.AllowGet)
                End If

                Dim usuarioSrv As New UsuarioServicio()
                Dim usuarios = usuarioSrv.Listar()

                Dim usuarioSesionId As Integer = 0
                If Session("UsuarioId") IsNot Nothing Then
                    Integer.TryParse(Session("UsuarioId").ToString(), usuarioSesionId)
                End If

                Dim perfil = usuarios.FirstOrDefault(Function(x) x.UsuId = usuarioSesionId)

                Return Json(New With {
            .success = True,
            .perfil = If(perfil Is Nothing, Nothing, New With {
                .usuarioId = perfil.UsuId,
                .username = perfil.Username,
                .email = perfil.Email,
                .rolId = perfil.RolId
            }),
            .usuarios = usuarios.Select(Function(x) New With {
                .usuarioId = x.UsuId,
                .username = x.Username,
                .email = x.Email,
                .rolId = x.RolId
            }).ToList()
        }, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
            .success = False,
            .message = ex.Message
        }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        Function DashboardData() As JsonResult
            Try
                If Not EsAdmin() Then
                    Response.StatusCode = 401
                    Return Json(New With {
                        .success = False,
                        .message = "No autorizado."
                    }, JsonRequestBehavior.AllowGet)
                End If

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

        ' =========================
        ' CUPONES
        ' =========================
        Function Cupones() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        <HttpGet>
        Function CuponesData() As JsonResult
            Try
                If Not EsAdmin() Then
                    Response.StatusCode = 401
                    Return Json(New With {.success = False, .message = "No autorizado."}, JsonRequestBehavior.AllowGet)
                End If

                Dim srv As New CuponServicio()
                Dim cupones = srv.Listar()

                Dim data = cupones.Select(Function(c) New With {
                    .cuponId = c.CuponId,
                    .codigo = c.Codigo,
                    .descripcion = c.Descripcion,
                    .vigenciaInicio = c.VigenciaInicio,
                    .vigenciaFin = c.VigenciaFin,
                    .limiteUsoTotal = c.LimiteUsoTotal,
                    .limiteUsoPorCliente = c.LimiteUsoPorCliente,
                    .usosActuales = c.UsosActuales,
                    .estado = c.Estado
                }).ToList()

                Return Json(data, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {.success = False, .message = ex.Message}, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Function GuardarCupon() As JsonResult
            Try
                If Not EsAdmin() Then
                    Response.StatusCode = 401
                    Return Json(New With {.success = False, .message = "No autorizado."})
                End If

                Dim cuponId As Integer = 0
                Integer.TryParse(Request.Form("CuponId"), cuponId)

                Dim cupon As New Cupon()
                cupon.CuponId = cuponId
                cupon.Codigo = Request.Form("Codigo")
                cupon.Descripcion = Request.Form("Descripcion")
                cupon.Estado = Request.Form("Estado")

                cupon.VigenciaInicio = DateTime.Parse(Request.Form("VigenciaInicio"))
                cupon.VigenciaFin = DateTime.Parse(Request.Form("VigenciaFin"))

                Dim limiteTotal As Integer = 0
                Integer.TryParse(Request.Form("LimiteUsoTotal"), limiteTotal)
                cupon.LimiteUsoTotal = limiteTotal

                Dim limiteCliente As Integer = 0
                Integer.TryParse(Request.Form("LimiteUsoPorCliente"), limiteCliente)
                cupon.LimiteUsoPorCliente = limiteCliente

                Dim srv As New CuponServicio()

                If cuponId > 0 Then
                    srv.Actualizar(cupon)
                Else
                    cupon.UsosActuales = 0
                    srv.Insertar(cupon)
                End If

                Return Json(New With {.success = True})

            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {.success = False, .message = ex.Message})
            End Try
        End Function

        <HttpPost>
        Function EliminarCupon(ByVal id As Integer) As JsonResult
            Try
                If Not EsAdmin() Then
                    Response.StatusCode = 401
                    Return Json(New With {.success = False, .message = "No autorizado."})
                End If

                Dim srv As New CuponServicio()
                srv.Eliminar(id)

                Return Json(New With {.success = True})

            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {.success = False, .message = ex.Message})
            End Try
        End Function

    End Class
End Namespace