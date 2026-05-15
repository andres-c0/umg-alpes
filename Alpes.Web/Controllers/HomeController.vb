Option Strict On
Option Explicit On

Imports System
Imports System.Linq
Imports System.Web.Mvc
Imports System.Collections.Generic
Imports Alpes.Entidades.Clientes
Imports Alpes.Entidades.Inventario
Imports Alpes.Servicios.Servicios
Imports Alpes.Web.Models
Imports Alpes.Entidades.Seguridad
Imports System.Text.RegularExpressions

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        Private ReadOnly _usuarioServicio As UsuarioServicio
        Private ReadOnly _productoServicio As ProductoServicio
        Private ReadOnly _precioHistoricoServicio As Precio_HistoricoServicio
        Private ReadOnly _clienteServicio As ClienteServicio

        Public Sub New()
            _usuarioServicio = New UsuarioServicio()
            _productoServicio = New ProductoServicio()
            _precioHistoricoServicio = New Precio_HistoricoServicio()
            _clienteServicio = New ClienteServicio()
        End Sub

        Function Index() As ActionResult
            Return View()
        End Function

        Function DetalleProducto(ByVal id As Integer) As ActionResult
            If id <= 0 Then
                Return RedirectToAction("Index", "Home")
            End If

            ViewData("ProductoId") = id

            Return View()
        End Function

        Function CarritoInvitado() As ActionResult
            Return View()
        End Function

        Function CatalogoPublico(Optional ByVal categoria As String = "",
                         Optional ByVal q As String = "") As ActionResult
            Dim categoriaNormalizada As String = If(categoria, String.Empty).Trim().ToUpperInvariant()

            If categoriaNormalizada <> "INTERIOR" AndAlso categoriaNormalizada <> "EXTERIOR" Then
                categoriaNormalizada = String.Empty
            End If

            ViewData("CategoriaInicial") = categoriaNormalizada
            ViewData("BusquedaInicial") = If(q, String.Empty).Trim()

            Return View()
        End Function

        <HttpGet>
        Function ObtenerProductosPublicosData(Optional ByVal tipo As String = "",
                                              Optional ByVal categoria As String = "",
                                              Optional ByVal q As String = "",
                                              Optional ByVal limite As Integer = 0) As ActionResult
            Try
                Dim productos As List(Of Producto) = _productoServicio.Listar()
                Dim precios As List(Of Precio_Historico) = ObtenerPreciosActivosSeguros()
                Dim resultado As New List(Of Object)()

                If productos Is Nothing Then
                    productos = New List(Of Producto)()
                End If

                Dim filtroTipo As String = If(Not String.IsNullOrWhiteSpace(tipo), tipo, categoria)

                If filtroTipo Is Nothing Then
                    filtroTipo = String.Empty
                End If

                filtroTipo = filtroTipo.Trim().ToUpperInvariant()

                If filtroTipo = "TODOS" OrElse filtroTipo = "TODO" OrElse filtroTipo = "ALL" Then
                    filtroTipo = String.Empty
                End If

                Dim busqueda As String = If(q, String.Empty).Trim()

                For Each producto As Producto In productos
                    If producto Is Nothing Then
                        Continue For
                    End If

                    If Not String.Equals(If(producto.Estado, String.Empty).Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                        Continue For
                    End If

                    If Not String.IsNullOrWhiteSpace(filtroTipo) Then
                        If Not String.Equals(If(producto.Tipo, String.Empty).Trim(), filtroTipo, StringComparison.OrdinalIgnoreCase) Then
                            Continue For
                        End If
                    End If

                    If Not CoincideBusquedaProducto(producto, busqueda) Then
                        Continue For
                    End If

                    Dim productoCompleto As Producto = ObtenerProductoCompletoSeguro(producto)
                    resultado.Add(CrearProductoPublicoDto(productoCompleto, precios))

                    If limite > 0 AndAlso resultado.Count >= limite Then
                        Exit For
                    End If
                Next

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .total = resultado.Count,
                    .data = resultado
                }, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Return JsonError("No se pudieron obtener los productos públicos: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerProductoPublicoData(ByVal id As Integer) As ActionResult
            Try
                If id <= 0 Then
                    Return JsonError("Debe enviar un producto válido.", 400, JsonRequestBehavior.AllowGet)
                End If

                Dim producto As Producto = _productoServicio.ObtenerPorId(id)

                If producto Is Nothing OrElse producto.ProductoId <= 0 Then
                    Return JsonError("No se encontró el producto.", 404, JsonRequestBehavior.AllowGet)
                End If

                If Not String.Equals(If(producto.Estado, String.Empty).Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    Return JsonError("El producto no está disponible.", 404, JsonRequestBehavior.AllowGet)
                End If

                Dim precios As List(Of Precio_Historico) = ObtenerPreciosActivosSeguros()

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = CrearProductoPublicoDto(producto, precios)
                }, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Return JsonError("No se pudo obtener el producto: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function Login(Optional ByVal returnUrl As String = "") As ActionResult
            Dim returnUrlSeguro As String = ObtenerReturnUrlSeguro(returnUrl)

            ViewData("ReturnUrl") = returnUrlSeguro

            If TempData("Error") IsNot Nothing Then
                ViewData("Error") = TempData("Error").ToString()
            End If

            If Session("UsuarioId") IsNot Nothing Then
                Dim rolId As Integer = ObtenerRolIdDesdeSesion()

                If Not String.IsNullOrWhiteSpace(returnUrlSeguro) AndAlso EsRolCliente(rolId) Then
                    Return Redirect(returnUrlSeguro)
                End If

                Return RedirigirSegunSesionActual()
            End If

            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Login(ByVal model As LoginViewModel, Optional ByVal returnUrl As String = "") As ActionResult
            If model Is Nothing Then
                model = New LoginViewModel()
            End If

            Dim returnUrlSeguro As String = ObtenerReturnUrlSeguro(returnUrl)

            If String.IsNullOrWhiteSpace(returnUrlSeguro) Then
                returnUrlSeguro = ObtenerReturnUrlSeguro(Request.Form("ReturnUrl"))
            End If

            ViewData("ReturnUrl") = returnUrlSeguro

            Dim usernameForm As String = Request.Form("Username")
            If String.IsNullOrWhiteSpace(usernameForm) Then
                usernameForm = Request.Form("username")
            End If

            Dim passwordForm As String = Request.Form("Password")
            If String.IsNullOrWhiteSpace(passwordForm) Then
                passwordForm = Request.Form("password")
            End If

            If String.IsNullOrWhiteSpace(model.Username) Then
                model.Username = usernameForm
            End If

            If String.IsNullOrWhiteSpace(model.Password) Then
                model.Password = passwordForm
            End If

            If String.IsNullOrWhiteSpace(model.Username) Then
                ModelState.AddModelError("Username", "El usuario es requerido.")
            End If

            If String.IsNullOrWhiteSpace(model.Password) Then
                ModelState.AddModelError("Password", "La contraseña es requerida.")
            End If

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Try
                Dim usernameIngresado As String = model.Username.Trim()
                Dim passwordIngresado As String = model.Password.Trim()

                Dim usuarios As List(Of Usuario) = _usuarioServicio.Buscar(usernameIngresado)

                If usuarios Is Nothing Then
                    usuarios = New List(Of Usuario)()
                End If

                Dim usuario As Usuario = usuarios.FirstOrDefault(
                    Function(u) u.Username IsNot Nothing AndAlso
                                String.Equals(u.Username.Trim(), usernameIngresado, StringComparison.OrdinalIgnoreCase)
                )

                If usuario Is Nothing Then
                    ViewData("Error") = "Usuario o contraseña incorrectos."
                    Return View(model)
                End If

                Dim passwordBd As String = If(usuario.PasswordHash, String.Empty).Trim()

                If Not String.Equals(passwordBd, passwordIngresado, StringComparison.Ordinal) Then
                    ViewData("Error") = "Usuario o contraseña incorrectos."
                    Return View(model)
                End If

                If usuario.Estado IsNot Nothing AndAlso
                   Not String.Equals(usuario.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    ViewData("Error") = "El usuario no está activo."
                    Return View(model)
                End If

                GuardarSesionUsuario(usuario)

                If Not String.IsNullOrWhiteSpace(returnUrlSeguro) AndAlso EsRolCliente(usuario.RolId) Then
                    Return Redirect(returnUrlSeguro)
                End If

                Return RedirigirSegunRolUsuario(usuario)

            Catch ex As Exception
                ViewData("Error") = "Ocurrió un error al iniciar sesión: " & ex.Message
                Return View(model)
            End Try
        End Function

        <HttpGet>
        Function Registro(Optional ByVal returnUrl As String = "") As ActionResult
            Dim returnUrlSeguro As String = ObtenerReturnUrlSeguro(returnUrl)

            If Session("UsuarioId") IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(returnUrlSeguro) AndAlso EsRolCliente(ObtenerRolIdDesdeSesion()) Then
                    Return Redirect(returnUrlSeguro)
                End If

                Return RedirigirSegunSesionActual()
            End If

            Dim model As New RegistroViewModel With {
        .Pais = "Guatemala",
        .ReturnUrl = returnUrlSeguro
    }

            ViewData("ReturnUrl") = returnUrlSeguro

            Return View(model)
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Registro(ByVal model As RegistroViewModel, Optional ByVal returnUrl As String = "") As ActionResult
            If model Is Nothing Then
                ViewData("Error") = "Solicitud inválida."
                Return View(New RegistroViewModel())
            End If

            Dim returnUrlSeguro As String = ObtenerReturnUrlSeguro(returnUrl)

            If String.IsNullOrWhiteSpace(returnUrlSeguro) Then
                returnUrlSeguro = ObtenerReturnUrlSeguro(model.ReturnUrl)
            End If

            If String.IsNullOrWhiteSpace(returnUrlSeguro) Then
                returnUrlSeguro = ObtenerReturnUrlSeguro(Request.Form("ReturnUrl"))
            End If

            model.ReturnUrl = returnUrlSeguro
            ViewData("ReturnUrl") = returnUrlSeguro

            ValidarRegistroCliente(model)

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Try
                If ExisteUsuarioPorUsername(model.Username.Trim()) Then
                    ModelState.AddModelError("Username", "Ese nombre de usuario ya existe.")
                    Return View(model)
                End If

                If ExisteUsuarioPorEmail(model.Email.Trim()) Then
                    ModelState.AddModelError("Email", "Ese correo ya está registrado como usuario.")
                    Return View(model)
                End If

                If ExisteClientePorDocumento(model.NumDocumento.Trim()) Then
                    ModelState.AddModelError("NumDocumento", "Ese número de documento ya está registrado.")
                    Return View(model)
                End If

                If ExisteClientePorEmail(model.Email.Trim()) Then
                    ModelState.AddModelError("Email", "Ese correo ya está registrado como cliente.")
                    Return View(model)
                End If

                Dim rolClienteId As Integer = ObtenerRolIdPorNombre("CLIENTE")

                If rolClienteId <= 0 Then
                    ViewData("Error") = "No existe un rol activo llamado CLIENTE. Crea el rol CLIENTE antes de registrar usuarios del portal."
                    Return View(model)
                End If

                Dim nuevoCliente As New Cliente With {
            .TipoDocumento = model.TipoDocumento.Trim().ToUpperInvariant(),
            .NumDocumento = model.NumDocumento.Trim(),
            .Nit = If(String.IsNullOrWhiteSpace(model.Nit), Nothing, model.Nit.Trim()),
            .Nombres = model.Nombres.Trim(),
            .Apellidos = model.Apellidos.Trim(),
            .Email = model.Email.Trim(),
            .TelResidencia = model.TelResidencia.Trim(),
            .TelCelular = If(String.IsNullOrWhiteSpace(model.TelCelular), Nothing, model.TelCelular.Trim()),
            .Direccion = model.Direccion.Trim(),
            .Ciudad = model.Ciudad.Trim(),
            .Departamento = model.Departamento.Trim(),
            .Pais = model.Pais.Trim(),
            .Profesion = If(String.IsNullOrWhiteSpace(model.Profesion), Nothing, model.Profesion.Trim()),
            .Estado = "ACTIVO"
        }

                Dim cliIdGenerado As Integer = _clienteServicio.Insertar(nuevoCliente)

                If cliIdGenerado <= 0 Then
                    ViewData("Error") = "No se pudo crear el cliente."
                    Return View(model)
                End If

                Dim nuevoUsuario As New Usuario With {
            .Username = model.Username.Trim(),
            .PasswordHash = model.Password.Trim(),
            .Email = model.Email.Trim(),
            .Telefono = model.TelefonoPrincipal,
            .RolId = rolClienteId,
            .CliId = cliIdGenerado,
            .EmpId = Nothing,
            .UltimoLoginAt = Nothing,
            .BloqueadoHasta = Nothing,
            .Estado = "ACTIVO"
        }

                Dim usuIdGenerado As Integer = _usuarioServicio.Insertar(nuevoUsuario)

                If usuIdGenerado > 0 Then
                    nuevoUsuario.UsuId = usuIdGenerado
                End If

                GuardarSesionUsuario(nuevoUsuario)

                If Not String.IsNullOrWhiteSpace(returnUrlSeguro) Then
                    Return Redirect(returnUrlSeguro)
                End If

                Return RedirectToAction("Index", "PortalCliente")

            Catch ex As Exception
                ViewData("Error") = "Ocurrió un error al crear la cuenta: " & LimpiarMensaje(ex.Message)
                Return View(model)
            End Try
        End Function

        Function Logout() As ActionResult
            Session.Clear()
            Session.Abandon()

            Return RedirectToAction("Login", "Home")
        End Function

        Private Sub GuardarSesionUsuario(ByVal usuario As Usuario)
            Session("UsuarioId") = usuario.UsuId
            Session("Username") = If(usuario.Username, String.Empty)
            Session("RolId") = usuario.RolId
            Session("RolNombre") = ObtenerNombreRolPorId(usuario.RolId)

            If usuario.CliId.HasValue AndAlso usuario.CliId.Value > 0 Then
                Session("CliId") = usuario.CliId.Value
            Else
                Session.Remove("CliId")
            End If

            If usuario.EmpId.HasValue AndAlso usuario.EmpId.Value > 0 Then
                Session("EmpId") = usuario.EmpId.Value
            Else
                Session.Remove("EmpId")
            End If
        End Sub

        Private Function RedirigirSegunSesionActual() As ActionResult
            Dim rolId As Integer = ObtenerRolIdDesdeSesion()

            If EsRolCliente(rolId) Then
                Return RedirectToAction("Index", "PortalCliente")
            End If

            If EsRolAdministrativo(rolId) Then
                Return RedirectToAction("Index", "Admin")
            End If

            Session.Clear()
            Session.Abandon()
            TempData("Error") = "La sesión no es válida. El usuario no tiene un rol activo asignado."
            Return RedirectToAction("Login", "Home")
        End Function

        Private Function RedirigirSegunRolUsuario(ByVal usuario As Usuario) As ActionResult
            If usuario Is Nothing Then
                Return RedirectToAction("Login", "Home")
            End If

            If EsRolCliente(usuario.RolId) Then
                Return RedirectToAction("Index", "PortalCliente")
            End If

            If EsRolAdministrativo(usuario.RolId) Then
                Return RedirectToAction("Index", "Admin")
            End If

            ViewData("Error") = "El usuario existe, pero no tiene un rol activo asignado."
            Session.Clear()
            Session.Abandon()
            Return View("Login", New LoginViewModel())
        End Function

        Private Function EsRolCliente(ByVal rolId As Integer) As Boolean
            Dim nombreRol As String = ObtenerNombreRolPorId(rolId)
            Return EsNombreRolCliente(nombreRol)
        End Function

        Private Function EsRolAdministrativo(ByVal rolId As Integer) As Boolean
            Dim nombreRol As String = ObtenerNombreRolPorId(rolId)

            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return False
            End If

            Return Not EsNombreRolCliente(nombreRol)
        End Function

        Private Function EsNombreRolCliente(ByVal nombreRol As String) As Boolean
            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return False
            End If

            Dim rolNormalizado As String = nombreRol.Trim().ToUpperInvariant()
            Return rolNormalizado = "CLIENTE" OrElse rolNormalizado.Contains("CLIENTE")
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

        Private Function ObtenerRolIdPorNombre(ByVal nombreRol As String) As Integer
            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return 0
            End If

            Try
                Dim rolSrv As New RolServicio()
                Dim roles = rolSrv.Buscar(nombreRol.Trim())

                If roles Is Nothing Then
                    roles = New List(Of Rol)()
                End If

                Dim rol = roles.FirstOrDefault(
                    Function(r) r.RolNombre IsNot Nothing AndAlso
                                String.Equals(r.RolNombre.Trim(), nombreRol.Trim(), StringComparison.OrdinalIgnoreCase) AndAlso
                                (r.Estado Is Nothing OrElse String.Equals(r.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase))
                )

                If rol IsNot Nothing Then
                    Return rol.RolId
                End If
            Catch
                Return 0
            End Try

            Return 0
        End Function

        Private Function ObtenerRolIdDesdeSesion() As Integer
            Dim rolId As Integer = 0

            If Session("RolId") IsNot Nothing Then
                Integer.TryParse(Session("RolId").ToString(), rolId)
            End If

            Return rolId
        End Function

        <NonAction>
        Private Function CrearProductoPublicoDto(ByVal producto As Producto,
                                                 ByVal precios As List(Of Precio_Historico)) As Object
            Dim precioActual As Decimal = ObtenerPrecioActualProducto(producto.ProductoId, precios)
            Dim cuota12 As Decimal = 0D

            If precioActual > 0D Then
                cuota12 = Math.Round(precioActual / 12D, 2)
            End If

            Return New With {
                .ProductoId = producto.ProductoId,
                .Referencia = If(producto.Referencia, String.Empty),
                .Nombre = If(producto.Nombre, String.Empty),
                .Descripcion = If(producto.Descripcion, String.Empty),
                .Tipo = If(producto.Tipo, String.Empty),
                .Material = If(producto.Material, String.Empty),
                .Color = If(producto.Color, String.Empty),
                .AltoCm = If(producto.AltoCm.HasValue, producto.AltoCm.Value, 0D),
                .AnchoCm = If(producto.AnchoCm.HasValue, producto.AnchoCm.Value, 0D),
                .ProfundidadCm = If(producto.ProfundidadCm.HasValue, producto.ProfundidadCm.Value, 0D),
                .PesoGramos = If(producto.PesoGramos.HasValue, producto.PesoGramos.Value, 0D),
                .ImagenUrl = If(producto.ImagenUrl, String.Empty),
                .CategoriaId = producto.CategoriaId,
                .Estado = If(producto.Estado, String.Empty),
                .PrecioActual = precioActual,
                .PrecioFormateado = FormatearMoneda(precioActual),
                .Cuota12 = cuota12,
                .Cuota12Formateada = FormatearMoneda(cuota12)
            }
        End Function

        <NonAction>
        Private Function ObtenerPreciosActivosSeguros() As List(Of Precio_Historico)
            Try
                Dim precios As List(Of Precio_Historico) = _precioHistoricoServicio.Listar()

                If precios Is Nothing Then
                    Return New List(Of Precio_Historico)()
                End If

                Return precios
            Catch
                Return New List(Of Precio_Historico)()
            End Try
        End Function

        <NonAction>
        Private Function ObtenerPrecioActualProducto(ByVal productoId As Integer,
                                                     ByVal precios As List(Of Precio_Historico)) As Decimal
            If productoId <= 0 Then
                Return 0D
            End If

            If precios Is Nothing OrElse precios.Count = 0 Then
                Return 0D
            End If

            Dim vigente As Precio_Historico = Nothing
            Dim ahora As DateTime = DateTime.Now

            For Each precio As Precio_Historico In precios
                If precio Is Nothing Then
                    Continue For
                End If

                If precio.ProductoId <> productoId Then
                    Continue For
                End If

                If Not String.IsNullOrWhiteSpace(precio.Estado) AndAlso
                   Not String.Equals(precio.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                Dim inicioOk As Boolean = (precio.VigenciaInicio = DateTime.MinValue OrElse ahora >= precio.VigenciaInicio)
                Dim finOk As Boolean = (Not precio.VigenciaFin.HasValue OrElse ahora <= precio.VigenciaFin.Value)

                If Not inicioOk OrElse Not finOk Then
                    Continue For
                End If

                If vigente Is Nothing Then
                    vigente = precio
                ElseIf precio.VigenciaInicio > vigente.VigenciaInicio Then
                    vigente = precio
                End If
            Next

            If vigente IsNot Nothing Then
                Return vigente.Precio
            End If

            Return 0D
        End Function

        <NonAction>
        Private Function CoincideBusquedaProducto(ByVal producto As Producto,
                                                  ByVal busqueda As String) As Boolean
            If String.IsNullOrWhiteSpace(busqueda) Then
                Return True
            End If

            Dim texto As String = busqueda.Trim()

            If ContieneTexto(producto.Nombre, texto) Then
                Return True
            End If

            If ContieneTexto(producto.Referencia, texto) Then
                Return True
            End If

            If ContieneTexto(producto.Descripcion, texto) Then
                Return True
            End If

            If ContieneTexto(producto.Material, texto) Then
                Return True
            End If

            If ContieneTexto(producto.Color, texto) Then
                Return True
            End If

            If ContieneTexto(producto.Tipo, texto) Then
                Return True
            End If

            Return False
        End Function

        <NonAction>
        Private Function ContieneTexto(ByVal origen As String,
                                       ByVal texto As String) As Boolean
            If String.IsNullOrWhiteSpace(origen) OrElse String.IsNullOrWhiteSpace(texto) Then
                Return False
            End If

            Return origen.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0
        End Function

        <NonAction>
        Private Function FormatearMoneda(ByVal monto As Decimal) As String
            Return "Q " & monto.ToString("N2")
        End Function

        <NonAction>
        Private Function JsonError(ByVal message As String,
                                   Optional ByVal statusCode As Integer = 400,
                                   Optional ByVal behavior As JsonRequestBehavior = JsonRequestBehavior.DenyGet) As ActionResult
            Response.StatusCode = statusCode

            Return Json(New With {
                .ok = False,
                .success = False,
                .message = message
            }, behavior)
        End Function

        <NonAction>
        Private Function LimpiarMensaje(ByVal message As String) As String
            If String.IsNullOrWhiteSpace(message) Then
                Return "Ocurrió un error inesperado."
            End If

            Dim partes() As String = message.Replace(vbCrLf, vbLf).Split(ControlChars.Lf)
            Return partes(0).Trim()
        End Function

        <NonAction>
        Private Function ObtenerProductoCompletoSeguro(ByVal productoListado As Producto) As Producto
            If productoListado Is Nothing OrElse productoListado.ProductoId <= 0 Then
                Return productoListado
            End If

            Try
                Dim productoDetalle As Producto = _productoServicio.ObtenerPorId(productoListado.ProductoId)

                If productoDetalle IsNot Nothing AndAlso productoDetalle.ProductoId > 0 Then
                    If String.IsNullOrWhiteSpace(productoDetalle.Estado) Then
                        productoDetalle.Estado = productoListado.Estado
                    End If

                    If productoDetalle.CategoriaId <= 0 Then
                        productoDetalle.CategoriaId = productoListado.CategoriaId
                    End If

                    If String.IsNullOrWhiteSpace(productoDetalle.Tipo) Then
                        productoDetalle.Tipo = productoListado.Tipo
                    End If

                    If String.IsNullOrWhiteSpace(productoDetalle.Nombre) Then
                        productoDetalle.Nombre = productoListado.Nombre
                    End If

                    If String.IsNullOrWhiteSpace(productoDetalle.Referencia) Then
                        productoDetalle.Referencia = productoListado.Referencia
                    End If

                    Return productoDetalle
                End If
            Catch
            End Try

            Return productoListado
        End Function

        <NonAction>
        Private Function ObtenerReturnUrlSeguro(ByVal returnUrl As String) As String
            If String.IsNullOrWhiteSpace(returnUrl) Then
                Return String.Empty
            End If

            Dim ruta As String = returnUrl.Trim()

            If ruta.StartsWith("//") OrElse ruta.StartsWith("\") Then
                Return String.Empty
            End If

            If Not ruta.StartsWith("/") Then
                Return String.Empty
            End If

            If ruta.Contains("://") Then
                Return String.Empty
            End If

            Return ruta
        End Function

        <NonAction>
        Private Sub ValidarRegistroCliente(ByVal model As RegistroViewModel)
            If String.IsNullOrWhiteSpace(model.Username) Then
                ModelState.AddModelError("Username", "El nombre de usuario es requerido.")
            ElseIf model.Username.Trim().Length < 4 Then
                ModelState.AddModelError("Username", "El nombre de usuario debe tener al menos 4 caracteres.")
            End If

            If String.IsNullOrWhiteSpace(model.Email) Then
                ModelState.AddModelError("Email", "El correo electrónico es requerido.")
            ElseIf Not Regex.IsMatch(model.Email.Trim(), "^[^@\s]+@[^@\s]+\.[^@\s]+$") Then
                ModelState.AddModelError("Email", "Ingresa un correo electrónico válido.")
            End If

            If String.IsNullOrWhiteSpace(model.Password) Then
                ModelState.AddModelError("Password", "La contraseña es requerida.")
            ElseIf model.Password.Length < 6 Then
                ModelState.AddModelError("Password", "La contraseña debe tener al menos 6 caracteres.")
            End If

            If String.IsNullOrWhiteSpace(model.ConfirmPassword) Then
                ModelState.AddModelError("ConfirmPassword", "Debes confirmar la contraseña.")
            ElseIf Not String.Equals(model.Password, model.ConfirmPassword, StringComparison.Ordinal) Then
                ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.")
            End If

            If String.IsNullOrWhiteSpace(model.TipoDocumento) Then
                ModelState.AddModelError("TipoDocumento", "El tipo de documento es requerido.")
            End If

            If String.IsNullOrWhiteSpace(model.NumDocumento) Then
                ModelState.AddModelError("NumDocumento", "El número de documento es requerido.")
            End If

            If String.IsNullOrWhiteSpace(model.Nombres) Then
                ModelState.AddModelError("Nombres", "Los nombres son requeridos.")
            End If

            If String.IsNullOrWhiteSpace(model.Apellidos) Then
                ModelState.AddModelError("Apellidos", "Los apellidos son requeridos.")
            End If

            If String.IsNullOrWhiteSpace(model.TelResidencia) Then
                ModelState.AddModelError("TelResidencia", "El teléfono de residencia es requerido.")
            End If

            If String.IsNullOrWhiteSpace(model.Direccion) Then
                ModelState.AddModelError("Direccion", "La dirección es requerida.")
            End If

            If String.IsNullOrWhiteSpace(model.Ciudad) Then
                ModelState.AddModelError("Ciudad", "La ciudad es requerida.")
            End If

            If String.IsNullOrWhiteSpace(model.Departamento) Then
                ModelState.AddModelError("Departamento", "El departamento es requerido.")
            End If

            If String.IsNullOrWhiteSpace(model.Pais) Then
                ModelState.AddModelError("Pais", "El país es requerido.")
            End If

            If Not String.IsNullOrWhiteSpace(model.TelResidencia) AndAlso model.TelResidencia.Trim().Length > 30 Then
                ModelState.AddModelError("TelResidencia", "El teléfono de residencia no puede exceder 30 caracteres.")
            End If

            If Not String.IsNullOrWhiteSpace(model.TelCelular) AndAlso model.TelCelular.Trim().Length > 30 Then
                ModelState.AddModelError("TelCelular", "El teléfono celular no puede exceder 30 caracteres.")
            End If
        End Sub

        <NonAction>
        Private Function ExisteUsuarioPorUsername(ByVal username As String) As Boolean
            Dim usuarios As List(Of Usuario) = _usuarioServicio.Buscar(username)

            If usuarios Is Nothing Then
                usuarios = New List(Of Usuario)()
            End If

            Return usuarios.Any(
                Function(u) u.Username IsNot Nothing AndAlso
                            String.Equals(u.Username.Trim(), username.Trim(), StringComparison.OrdinalIgnoreCase)
            )
        End Function

        <NonAction>
        Private Function ExisteUsuarioPorEmail(ByVal email As String) As Boolean
            Dim usuarios As List(Of Usuario) = _usuarioServicio.Buscar(email)

            If usuarios Is Nothing Then
                usuarios = New List(Of Usuario)()
            End If

            Return usuarios.Any(
                Function(u) u.Email IsNot Nothing AndAlso
                            String.Equals(u.Email.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase)
            )
        End Function

        <NonAction>
        Private Function ExisteClientePorDocumento(ByVal numDocumento As String) As Boolean
            Try
                Dim clientes As List(Of Cliente) = _clienteServicio.Buscar("NUM_DOCUMENTO", numDocumento)

                If clientes Is Nothing Then
                    clientes = New List(Of Cliente)()
                End If

                Return clientes.Any(
                    Function(c) c.NumDocumento IsNot Nothing AndAlso
                                String.Equals(c.NumDocumento.Trim(), numDocumento.Trim(), StringComparison.OrdinalIgnoreCase)
                )
            Catch
                Return False
            End Try
        End Function

        <NonAction>
        Private Function ExisteClientePorEmail(ByVal email As String) As Boolean
            Try
                Dim clientes As List(Of Cliente) = _clienteServicio.Buscar("EMAIL", email)

                If clientes Is Nothing Then
                    clientes = New List(Of Cliente)()
                End If

                Return clientes.Any(
                    Function(c) c.Email IsNot Nothing AndAlso
                                String.Equals(c.Email.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase)
                )
            Catch
                Return False
            End Try
        End Function

    End Class
End Namespace