Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Web.Mvc
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Clientes
Imports Alpes.Entidades.Ventas
Imports Alpes.Entidades.Inventario
Imports Alpes.Entidades.Marketing

Namespace Controllers
    Public Class PortalClienteController
        Inherits Controller

        Private ReadOnly _clienteServicio As ClienteServicio
        Private ReadOnly _listaDeseosServicio As ListaDeseosServicio
        Private ReadOnly _productoServicio As ProductoServicio
        Private ReadOnly _tarjetaClienteServicio As TarjetaClienteServicio
        Private ReadOnly _ordenVentaServicio As Orden_VentaServicio
        Private ReadOnly _ordenVentaDetalleServicio As Orden_Venta_DetalleServicio
        Private ReadOnly _envioServicio As EnvioServicio
        Private ReadOnly _seguimientoEnvioServicio As SeguimientoEnvioServicio
        Private ReadOnly _carritoServicio As CarritoServicio
        Private ReadOnly _carritoDetalleServicio As CarritoDetalleServicio
        Private ReadOnly _metodoPagoServicio As Metodo_PagoServicio
        Private ReadOnly _pagoServicio As PagoServicio
        Private ReadOnly _cuponServicio As CuponServicio
        Private ReadOnly _precioHistoricoServicio As Precio_HistoricoServicio
        Private ReadOnly _estadoOrdenServicio As Estado_OrdenServicio

        Public Sub New()
            _clienteServicio = New ClienteServicio()
            _listaDeseosServicio = New ListaDeseosServicio()
            _productoServicio = New ProductoServicio()
            _tarjetaClienteServicio = New TarjetaClienteServicio()
            _ordenVentaServicio = New Orden_VentaServicio()
            _ordenVentaDetalleServicio = New Orden_Venta_DetalleServicio()
            _envioServicio = New EnvioServicio()
            _seguimientoEnvioServicio = New SeguimientoEnvioServicio()
            _carritoServicio = New CarritoServicio()
            _carritoDetalleServicio = New CarritoDetalleServicio()
            _metodoPagoServicio = New Metodo_PagoServicio()
            _pagoServicio = New PagoServicio()
            _cuponServicio = New CuponServicio()
            _precioHistoricoServicio = New Precio_HistoricoServicio()
            _estadoOrdenServicio = New Estado_OrdenServicio()
        End Sub

        Function Index() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function MiPerfil() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function MisOrdenes() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function MisFavoritos() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function Notificaciones() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function Soporte() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function MisTarjetas() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function Historial() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function Configuracion() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function Carrito() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function Checkout() As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            Return View()
        End Function

        Function DetalleOrden(Optional ByVal id As Integer = 0) As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            ViewData("OrdenVentaId") = id
            Return View()
        End Function

        Function Tracking(Optional ByVal ordenVentaId As Integer = 0) As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            ViewData("OrdenVentaId") = ordenVentaId
            Return View()
        End Function

        Function DetalleProducto(ByVal id As Integer) As ActionResult
            Dim acceso As ActionResult = ValidarSesionCliente()
            If acceso IsNot Nothing Then
                Return acceso
            End If

            ViewData("ProductoId") = id
            Return View()
        End Function

        <HttpGet>
        Function ObtenerPerfilActual() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim cliente As Cliente = _clienteServicio.ObtenerPorId(cliId)

                If cliente Is Nothing Then
                    Return JsonError("No se encontro el perfil del cliente.", 404, JsonRequestBehavior.AllowGet)
                End If

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = New With {
                        .CliId = cliente.CliId,
                        .Nombres = cliente.Nombres,
                        .Apellidos = cliente.Apellidos,
                        .NombreCompleto = (cliente.Nombres & " " & cliente.Apellidos).Trim(),
                        .Email = cliente.Email,
                        .TelResidencia = cliente.TelResidencia,
                        .TelCelular = cliente.TelCelular,
                        .Direccion = cliente.Direccion,
                        .Ciudad = cliente.Ciudad,
                        .Departamento = cliente.Departamento,
                        .Pais = cliente.Pais,
                        .Estado = cliente.Estado
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudo obtener el perfil del cliente: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Function ActualizarPerfil() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()

                Dim nombres As String = ObtenerString(body, "nombres", "Nombres", "nombre", "Nombre")
                Dim apellidos As String = ObtenerString(body, "apellidos", "Apellidos", "apellido", "Apellido")
                Dim email As String = ObtenerString(body, "email", "Email")

                If String.IsNullOrWhiteSpace(nombres) Then
                    Return JsonError("Debe enviar el nombre del cliente.")
                End If

                If String.IsNullOrWhiteSpace(apellidos) Then
                    Return JsonError("Debe enviar el apellido del cliente.")
                End If

                If String.IsNullOrWhiteSpace(email) Then
                    Return JsonError("Debe enviar el email del cliente.")
                End If

                Dim clienteActual As Cliente = _clienteServicio.ObtenerPorId(cliId)

                If clienteActual Is Nothing Then
                    Return JsonError("No se encontro el perfil del cliente.", 404)
                End If

                clienteActual.Nombres = nombres.Trim()
                clienteActual.Apellidos = apellidos.Trim()
                clienteActual.Email = email.Trim()

                _clienteServicio.Actualizar(clienteActual)

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .message = "Perfil actualizado correctamente.",
                    .data = New With {
                        .CliId = clienteActual.CliId,
                        .Nombres = clienteActual.Nombres,
                        .Apellidos = clienteActual.Apellidos,
                        .NombreCompleto = (clienteActual.Nombres & " " & clienteActual.Apellidos).Trim(),
                        .Email = clienteActual.Email
                    }
                })
            Catch ex As Exception
                Return JsonError("No se pudo actualizar el perfil: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpGet>
        Function ObtenerFavoritosData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim favoritos As List(Of ListaDeseos) = _listaDeseosServicio.Buscar("CLI_ID", cliId.ToString())
                Dim resultado As New List(Of Object)()

                For Each favorito As ListaDeseos In favoritos
                    Dim producto As Producto = _productoServicio.ObtenerPorId(favorito.ProductoId)

                    If producto IsNot Nothing Then
                        resultado.Add(New With {
                            .ListaDeseosId = favorito.ListaDeseosId,
                            .CliId = favorito.CliId,
                            .ProductoId = favorito.ProductoId,
                            .Nota = favorito.Nota,
                            .Producto = New With {
                                .ProductoId = producto.ProductoId,
                                .Referencia = producto.Referencia,
                                .Nombre = producto.Nombre,
                                .Descripcion = producto.Descripcion,
                                .Tipo = producto.Tipo,
                                .Material = producto.Material,
                                .Color = producto.Color,
                                .ImagenUrl = producto.ImagenUrl,
                                .CategoriaId = producto.CategoriaId,
                                .Estado = producto.Estado
                            }
                        })
                    End If
                Next

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = resultado
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudieron obtener los favoritos: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Function AgregarFavorito() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim productoId As Integer = ObtenerEntero(body, "productoId", "ProductoId", "idProducto", "IdProducto")
                Dim nota As String = ObtenerString(body, "nota", "Nota")

                If productoId <= 0 Then
                    Return JsonError("Debe enviar un ProductoId valido.")
                End If

                Dim existentes As List(Of ListaDeseos) = _listaDeseosServicio.Buscar("CLI_ID", cliId.ToString())
                Dim yaExiste As ListaDeseos = Nothing

                For Each item As ListaDeseos In existentes
                    If item.ProductoId = productoId Then
                        yaExiste = item
                        Exit For
                    End If
                Next

                If yaExiste IsNot Nothing Then
                    Return Json(New With {
                        .ok = True,
                        .success = True,
                        .message = "El producto ya estaba en favoritos.",
                        .data = New With {
                            .ListaDeseosId = yaExiste.ListaDeseosId,
                            .ProductoId = yaExiste.ProductoId
                        }
                    })
                End If

                Dim nuevo As New ListaDeseos() With {
                    .CliId = cliId,
                    .ProductoId = productoId,
                    .Nota = nota,
                    .Estado = "ACTIVO"
                }

                Dim idGenerado As Integer = _listaDeseosServicio.Insertar(nuevo)

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .message = "Producto agregado a favoritos.",
                    .data = New With {
                        .ListaDeseosId = idGenerado,
                        .ProductoId = productoId
                    }
                })
            Catch ex As Exception
                Return JsonError("No se pudo agregar el favorito: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpPost>
        Function QuitarFavorito() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim listaDeseosId As Integer = ObtenerEntero(body, "listaDeseosId", "ListaDeseosId", "id", "Id")
                Dim productoId As Integer = ObtenerEntero(body, "productoId", "ProductoId", "idProducto", "IdProducto")

                If listaDeseosId <= 0 AndAlso productoId > 0 Then
                    Dim existentes As List(Of ListaDeseos) = _listaDeseosServicio.Buscar("CLI_ID", cliId.ToString())

                    For Each favorito As ListaDeseos In existentes
                        If favorito.ProductoId = productoId Then
                            listaDeseosId = favorito.ListaDeseosId
                            Exit For
                        End If
                    Next
                End If

                If listaDeseosId <= 0 Then
                    Return JsonError("No se encontro el favorito a eliminar.")
                End If

                Dim eliminado As Boolean = _listaDeseosServicio.Eliminar(listaDeseosId)

                Return Json(New With {
                    .ok = eliminado,
                    .success = eliminado,
                    .message = If(eliminado, "Favorito eliminado correctamente.", "No se pudo eliminar el favorito.")
                })
            Catch ex As Exception
                Return JsonError("No se pudo eliminar el favorito: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpGet>
        Function ObtenerRecomendadosData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim favoritos As List(Of ListaDeseos) = _listaDeseosServicio.Buscar("CLI_ID", cliId.ToString())
                Dim todosLosProductos As List(Of Producto) = _productoServicio.Listar()

                Dim idsFavoritos As New HashSet(Of Integer)()
                Dim pesosCategoria As New Dictionary(Of Integer, Decimal)()
                Dim pesosTipo As New Dictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase)

                For Each favorito As ListaDeseos In favoritos
                    idsFavoritos.Add(favorito.ProductoId)

                    Dim productoFavorito As Producto = Nothing

                    For Each p As Producto In todosLosProductos
                        If p.ProductoId = favorito.ProductoId Then
                            productoFavorito = p
                            Exit For
                        End If
                    Next

                    If productoFavorito Is Nothing Then
                        productoFavorito = _productoServicio.ObtenerPorId(favorito.ProductoId)
                    End If

                    If productoFavorito IsNot Nothing Then
                        If Not pesosCategoria.ContainsKey(productoFavorito.CategoriaId) Then
                            pesosCategoria(productoFavorito.CategoriaId) = 0D
                        End If
                        pesosCategoria(productoFavorito.CategoriaId) += 3D

                        If Not String.IsNullOrWhiteSpace(productoFavorito.Tipo) Then
                            Dim tipoClave As String = productoFavorito.Tipo.Trim().ToUpperInvariant()

                            If Not pesosTipo.ContainsKey(tipoClave) Then
                                pesosTipo(tipoClave) = 0D
                            End If

                            pesosTipo(tipoClave) += 1D
                        End If
                    End If
                Next

                Dim candidatos As New List(Of Producto)()

                For Each p As Producto In todosLosProductos
                    If p IsNot Nothing AndAlso String.Equals(p.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) AndAlso Not idsFavoritos.Contains(p.ProductoId) Then
                        candidatos.Add(p)
                    End If
                Next

                Dim recomendados As New List(Of Object)()

                If pesosCategoria.Count = 0 AndAlso pesosTipo.Count = 0 Then
                    Dim contador As Integer = 0

                    For Each p As Producto In candidatos
                        recomendados.Add(New With {
                            .ListaDeseosId = 0,
                            .EsFavorito = False,
                            .Score = 0D,
                            .Producto = New With {
                                .ProductoId = p.ProductoId,
                                .Referencia = p.Referencia,
                                .Nombre = p.Nombre,
                                .Descripcion = p.Descripcion,
                                .Tipo = p.Tipo,
                                .Material = p.Material,
                                .Color = p.Color,
                                .ImagenUrl = p.ImagenUrl,
                                .CategoriaId = p.CategoriaId,
                                .Estado = p.Estado
                            }
                        })

                        contador += 1
                        If contador >= 8 Then
                            Exit For
                        End If
                    Next
                Else
                    Dim puntuados As New List(Of ProductoConScore)()

                    For Each p As Producto In candidatos
                        Dim score As Decimal = CalcularScoreProducto(p, pesosCategoria, pesosTipo)
                        puntuados.Add(New ProductoConScore With {
                            .Producto = p,
                            .Score = score
                        })
                    Next

                    puntuados.Sort(Function(a, b)
                                       Dim comparacion As Integer = b.Score.CompareTo(a.Score)

                                       If comparacion = 0 Then
                                           Return String.Compare(a.Producto.Nombre, b.Producto.Nombre, StringComparison.OrdinalIgnoreCase)
                                       End If

                                       Return comparacion
                                   End Function)

                    Dim contador As Integer = 0

                    For Each item As ProductoConScore In puntuados
                        recomendados.Add(New With {
                            .ListaDeseosId = 0,
                            .EsFavorito = False,
                            .Score = item.Score,
                            .Producto = New With {
                                .ProductoId = item.Producto.ProductoId,
                                .Referencia = item.Producto.Referencia,
                                .Nombre = item.Producto.Nombre,
                                .Descripcion = item.Producto.Descripcion,
                                .Tipo = item.Producto.Tipo,
                                .Material = item.Producto.Material,
                                .Color = item.Producto.Color,
                                .ImagenUrl = item.Producto.ImagenUrl,
                                .CategoriaId = item.Producto.CategoriaId,
                                .Estado = item.Producto.Estado
                            }
                        })

                        contador += 1
                        If contador >= 8 Then
                            Exit For
                        End If
                    Next
                End If

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = recomendados
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudieron obtener los recomendados: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerCatalogoData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim productos As List(Of Producto) = _productoServicio.Listar()
                Dim favoritos As List(Of ListaDeseos) = _listaDeseosServicio.Buscar("CLI_ID", cliId.ToString())
                Dim favoritosMap As New Dictionary(Of Integer, Integer)()

                For Each favorito As ListaDeseos In favoritos
                    If Not favoritosMap.ContainsKey(favorito.ProductoId) Then
                        favoritosMap.Add(favorito.ProductoId, favorito.ListaDeseosId)
                    End If
                Next

                Dim resultado As New List(Of Object)()

                For Each producto As Producto In productos
                    If producto Is Nothing Then
                        Continue For
                    End If

                    If Not String.Equals(producto.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                        Continue For
                    End If

                    Dim listaDeseosId As Integer = 0
                    Dim esFavorito As Boolean = False

                    If favoritosMap.ContainsKey(producto.ProductoId) Then
                        listaDeseosId = favoritosMap(producto.ProductoId)
                        esFavorito = True
                    End If

                    resultado.Add(New With {
                        .ListaDeseosId = listaDeseosId,
                        .EsFavorito = esFavorito,
                        .Score = 0D,
                        .Producto = New With {
                            .ProductoId = producto.ProductoId,
                            .Referencia = producto.Referencia,
                            .Nombre = producto.Nombre,
                            .Descripcion = producto.Descripcion,
                            .Tipo = producto.Tipo,
                            .Material = producto.Material,
                            .Color = producto.Color,
                            .ImagenUrl = producto.ImagenUrl,
                            .CategoriaId = producto.CategoriaId,
                            .Estado = producto.Estado
                        }
                    })
                Next

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = resultado
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudo obtener el catalogo: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerProductoDetalleData(ByVal id As Integer) As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                If id <= 0 Then
                    Return JsonError("Debe enviar un producto valido.", 400, JsonRequestBehavior.AllowGet)
                End If

                Dim producto As Producto = _productoServicio.ObtenerPorId(id)

                If producto Is Nothing Then
                    Return JsonError("No se encontro el producto.", 404, JsonRequestBehavior.AllowGet)
                End If

                Dim favoritos As List(Of ListaDeseos) = _listaDeseosServicio.Buscar("CLI_ID", cliId.ToString())
                Dim listaDeseosId As Integer = 0
                Dim esFavorito As Boolean = False

                For Each favorito As ListaDeseos In favoritos
                    If favorito.ProductoId = producto.ProductoId Then
                        listaDeseosId = favorito.ListaDeseosId
                        esFavorito = True
                        Exit For
                    End If
                Next

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = New With {
                        .ListaDeseosId = listaDeseosId,
                        .EsFavorito = esFavorito,
                        .Producto = New With {
                            .ProductoId = producto.ProductoId,
                            .Referencia = producto.Referencia,
                            .Nombre = producto.Nombre,
                            .Descripcion = producto.Descripcion,
                            .Tipo = producto.Tipo,
                            .Material = producto.Material,
                            .Color = producto.Color,
                            .ImagenUrl = producto.ImagenUrl,
                            .CategoriaId = producto.CategoriaId,
                            .Estado = producto.Estado
                        }
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudo obtener el detalle del producto: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerTarjetasData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim tarjetas As List(Of TarjetaCliente) = _tarjetaClienteServicio.ObtenerPorCliente(cliId)
                Dim resultado As New List(Of Object)()

                For Each t As TarjetaCliente In tarjetas
                    resultado.Add(New With {
                        .TarjetaClienteId = t.TarjetaClienteId,
                        .Titular = t.Titular,
                        .Ultimos4 = t.Ultimos4,
                        .NumeroMascarado = "**** **** **** " & t.Ultimos4,
                        .Marca = t.Marca,
                        .MesExpiracion = t.MesVencimiento,
                        .AnioExpiracion = t.AnioVencimiento,
                        .Alias = t.AliasTarjeta,
                        .EsPredeterminada = (t.Predeterminada = 1),
                        .Estado = t.Estado
                    })
                Next

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .data = resultado
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudieron obtener las tarjetas: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Function CrearTarjetaData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()

                Dim titular As String = ObtenerString(body, "titular", "Titular")
                Dim numero As String = ObtenerString(body, "numero", "Numero")
                Dim marca As String = ObtenerString(body, "marca", "Marca")
                Dim aliasTarjeta As String = ObtenerString(body, "alias", "Alias")
                Dim mesExpiracion As Integer = ObtenerEntero(body, "mesExpiracion", "MesExpiracion")
                Dim anioExpiracion As Integer = ObtenerEntero(body, "anioExpiracion", "AnioExpiracion")
                Dim predeterminadaValor As String = ObtenerString(body, "predeterminada", "Predeterminada")

                numero = SoloDigitos(numero)

                If String.IsNullOrWhiteSpace(titular) Then
                    Return JsonError("Debe ingresar el titular.")
                End If

                If numero.Length < 4 Then
                    Return JsonError("Debe ingresar al menos los ultimos 4 digitos de la tarjeta.")
                End If

                If mesExpiracion <= 0 OrElse mesExpiracion > 12 Then
                    Return JsonError("Debe ingresar un mes de vencimiento valido.")
                End If

                If anioExpiracion <= 0 Then
                    Return JsonError("Debe ingresar un anio de vencimiento valido.")
                End If

                Dim entidad As New TarjetaCliente() With {
                    .CliId = cliId,
                    .Titular = titular.Trim(),
                    .Ultimos4 = numero.Substring(numero.Length - 4),
                    .Marca = If(String.IsNullOrWhiteSpace(marca), "OTRA", marca.Trim()),
                    .MesVencimiento = mesExpiracion,
                    .AnioVencimiento = anioExpiracion,
                    .AliasTarjeta = aliasTarjeta,
                    .Predeterminada = If(String.Equals(predeterminadaValor, "true", StringComparison.OrdinalIgnoreCase), 1, 0)
                }

                _tarjetaClienteServicio.Insertar(entidad)

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .message = "Tarjeta registrada correctamente."
                })
            Catch ex As Exception
                Return JsonError("No se pudo registrar la tarjeta: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpPost>
        Function MarcarTarjetaPredeterminadaData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim tarjetaId As Integer = ObtenerEntero(body, "tarjetaId", "TarjetaId", "id", "Id")

                If tarjetaId <= 0 Then
                    Return JsonError("Debe enviar una tarjeta valida.")
                End If

                Dim tarjeta As TarjetaCliente = _tarjetaClienteServicio.ObtenerPorId(tarjetaId)

                If tarjeta Is Nothing OrElse tarjeta.CliId <> cliId Then
                    Return JsonError("La tarjeta no pertenece al cliente autenticado.", 403)
                End If

                _tarjetaClienteServicio.MarcarPredeterminada(tarjetaId, cliId)

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .message = "Tarjeta marcada como predeterminada."
                })
            Catch ex As Exception
                Return JsonError("No se pudo actualizar la tarjeta: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpPost>
        Function DesactivarTarjetaData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim tarjetaId As Integer = ObtenerEntero(body, "tarjetaId", "TarjetaId", "id", "Id")

                If tarjetaId <= 0 Then
                    Return JsonError("Debe enviar una tarjeta valida.")
                End If

                Dim tarjeta As TarjetaCliente = _tarjetaClienteServicio.ObtenerPorId(tarjetaId)

                If tarjeta Is Nothing OrElse tarjeta.CliId <> cliId Then
                    Return JsonError("La tarjeta no pertenece al cliente autenticado.", 403)
                End If

                _tarjetaClienteServicio.Desactivar(tarjetaId)

                Return Json(New With {
                    .ok = True,
                    .success = True,
                    .message = "Tarjeta eliminada correctamente."
                })
            Catch ex As Exception
                Return JsonError("No se pudo eliminar la tarjeta: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpGet>
        Function ObtenerResumenNavegacionData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return Json(New With {
                    .ok = False,
                    .message = "Sesion no valida."
                }, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim carrito As Carrito = ObtenerCarritoActivoCliente(cliId, False)
                Dim carritoItems As Integer = 0
                Dim ordenesActivas As Integer = 0

                If carrito IsNot Nothing AndAlso carrito.CarritoId > 0 Then
                    Dim detalles As List(Of CarritoDetalle) = FiltrarDetallesCarrito(_carritoDetalleServicio.Listar(), carrito.CarritoId)
                    Dim i As Integer
                    For i = 0 To detalles.Count - 1
                        carritoItems += detalles(i).Cantidad
                    Next
                End If

                Dim ordenes As List(Of Orden_Venta) = _ordenVentaServicio.Listar()
                Dim envios As List(Of Envio) = _envioServicio.Listar()

                For Each orden As Orden_Venta In ordenes
                    If orden Is Nothing OrElse orden.CliId <> cliId Then
                        Continue For
                    End If

                    Dim envio As Envio = BuscarEnvioPorOrden(envios, orden.OrdenVentaId)
                    Dim estadoUi As String = NormalizarEstadoOrden(orden, envio)

                    If estadoUi <> "ENTREGADA" AndAlso estadoUi <> "CANCELADA" Then
                        ordenesActivas += 1
                    End If
                Next

                Return Json(New With {
                    .ok = True,
                    .data = New With {
                        .carritoItems = carritoItems,
                        .ordenesActivas = ordenesActivas
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {
                    .ok = False,
                    .message = LimpiarMensaje(ex.Message)
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerCarritoData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim carrito As Carrito = ObtenerCarritoActivoCliente(cliId, False)

                If carrito Is Nothing OrElse carrito.CarritoId <= 0 Then
                    Return Json(New With {
                        .ok = True,
                        .data = New With {
                            .CarritoId = 0,
                            .Items = New List(Of Object)(),
                            .Subtotal = 0D,
                            .Impuesto = 0D,
                            .Descuento = 0D,
                            .Total = 0D,
                            .Moneda = "GTQ"
                        }
                    }, JsonRequestBehavior.AllowGet)
                End If

                Dim detalles As List(Of CarritoDetalle) = FiltrarDetallesCarrito(_carritoDetalleServicio.Listar(), carrito.CarritoId)
                Dim items As New List(Of Object)()
                Dim subtotal As Decimal = 0D

                For Each det As CarritoDetalle In detalles
                    Dim producto As Producto = _productoServicio.ObtenerPorId(det.ProductoId)
                    Dim precio As Decimal = ObtenerPrecioProducto(det)
                    Dim subtotalLinea As Decimal = Math.Round(precio * det.Cantidad, 2)

                    subtotal += subtotalLinea

                    items.Add(New With {
                        .CarritoDetId = det.CarritoDetId,
                        .ProductoId = det.ProductoId,
                        .Cantidad = det.Cantidad,
                        .PrecioUnitario = precio,
                        .SubtotalLinea = subtotalLinea,
                        .Nombre = If(producto IsNot Nothing, producto.Nombre, "Producto"),
                        .Referencia = If(producto IsNot Nothing, producto.Referencia, ""),
                        .Color = If(producto IsNot Nothing, producto.Color, ""),
                        .Material = If(producto IsNot Nothing, producto.Material, ""),
                        .ImagenUrl = If(producto IsNot Nothing, producto.ImagenUrl, "")
                    })
                Next

                Dim impuesto As Decimal = Math.Round(subtotal * 0.12D, 2)
                Dim descuento As Decimal = 0D
                Dim total As Decimal = Math.Round(subtotal + impuesto - descuento, 2)

                Return Json(New With {
                    .ok = True,
                    .data = New With {
                        .CarritoId = carrito.CarritoId,
                        .Items = items,
                        .Subtotal = subtotal,
                        .Impuesto = impuesto,
                        .Descuento = descuento,
                        .Total = total,
                        .Moneda = "GTQ"
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudo obtener el carrito: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Function AgregarAlCarritoData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim productoId As Integer = ObtenerEntero(body, "productoId", "ProductoId")
                Dim cantidad As Integer = ObtenerEntero(body, "cantidad", "Cantidad")

                If cantidad <= 0 Then
                    cantidad = 1
                End If

                If productoId <= 0 Then
                    Return JsonError("Debe enviar un producto valido.")
                End If

                Dim carrito As Carrito = ObtenerCarritoActivoCliente(cliId, True)
                Dim detalles As List(Of CarritoDetalle) = FiltrarDetallesCarrito(_carritoDetalleServicio.Listar(), carrito.CarritoId)
                Dim existente As CarritoDetalle = Nothing

                For Each det As CarritoDetalle In detalles
                    If det.ProductoId = productoId Then
                        existente = det
                        Exit For
                    End If
                Next

                Dim precioActual As Decimal = ObtenerPrecioActualProducto(productoId)

                If existente IsNot Nothing Then
                    existente.Cantidad += cantidad
                    existente.PrecioUnitarioSnapshot = precioActual
                    _carritoDetalleServicio.Actualizar(existente)
                Else
                    Dim nuevo As New CarritoDetalle() With {
                        .CarritoId = carrito.CarritoId,
                        .ProductoId = productoId,
                        .Cantidad = cantidad,
                        .PrecioUnitarioSnapshot = precioActual,
                        .Estado = "ACTIVO"
                    }

                    _carritoDetalleServicio.Insertar(nuevo)
                End If

                carrito.UltimoCalculoAt = DateTime.Now
                If String.IsNullOrWhiteSpace(carrito.EstadoCarrito) Then
                    carrito.EstadoCarrito = "ABIERTO"
                End If
                _carritoServicio.Actualizar(carrito)

                Return Json(New With {
                    .ok = True,
                    .message = "Producto agregado al carrito correctamente."
                })
            Catch ex As Exception
                Return JsonError("No se pudo agregar al carrito: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpPost>
        Function ActualizarCantidadCarritoData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim carritoDetId As Integer = ObtenerEntero(body, "carritoDetId", "CarritoDetId", "id", "Id")
                Dim cantidad As Integer = ObtenerEntero(body, "cantidad", "Cantidad")

                If carritoDetId <= 0 Then
                    Return JsonError("Debe enviar un detalle de carrito valido.")
                End If

                Dim detalle As CarritoDetalle = _carritoDetalleServicio.ObtenerPorId(carritoDetId)

                If detalle Is Nothing OrElse detalle.CarritoDetId <= 0 Then
                    Return JsonError("No se encontro el detalle del carrito.")
                End If

                Dim carrito As Carrito = _carritoServicio.ObtenerPorId(detalle.CarritoId)

                If carrito Is Nothing OrElse carrito.CliId <> cliId Then
                    Return JsonError("El detalle no pertenece al cliente autenticado.", 403)
                End If

                If cantidad <= 0 Then
                    _carritoDetalleServicio.Eliminar(carritoDetId)
                    Return Json(New With {
                        .ok = True,
                        .message = "Producto eliminado del carrito."
                    })
                End If

                detalle.Cantidad = cantidad
                detalle.PrecioUnitarioSnapshot = ObtenerPrecioActualProducto(detalle.ProductoId)
                _carritoDetalleServicio.Actualizar(detalle)

                Return Json(New With {
                    .ok = True,
                    .message = "Cantidad actualizada correctamente."
                })
            Catch ex As Exception
                Return JsonError("No se pudo actualizar la cantidad: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpPost>
        Function EliminarDelCarritoData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim carritoDetId As Integer = ObtenerEntero(body, "carritoDetId", "CarritoDetId", "id", "Id")

                If carritoDetId <= 0 Then
                    Return JsonError("Debe enviar un detalle valido.")
                End If

                Dim detalle As CarritoDetalle = _carritoDetalleServicio.ObtenerPorId(carritoDetId)

                If detalle Is Nothing OrElse detalle.CarritoDetId <= 0 Then
                    Return JsonError("No se encontro el detalle del carrito.")
                End If

                Dim carrito As Carrito = _carritoServicio.ObtenerPorId(detalle.CarritoId)

                If carrito Is Nothing OrElse carrito.CliId <> cliId Then
                    Return JsonError("El detalle no pertenece al cliente autenticado.", 403)
                End If

                _carritoDetalleServicio.Eliminar(carritoDetId)

                Return Json(New With {
                    .ok = True,
                    .message = "Producto eliminado del carrito."
                })
            Catch ex As Exception
                Return JsonError("No se pudo eliminar del carrito: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpGet>
        Function ObtenerCheckoutData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim carrito As Carrito = ObtenerCarritoActivoCliente(cliId, False)
                Dim cliente As Cliente = _clienteServicio.ObtenerPorId(cliId)
                Dim tarjetas As List(Of TarjetaCliente) = _tarjetaClienteServicio.ObtenerPorCliente(cliId)
                Dim metodos As List(Of Metodo_Pago) = _metodoPagoServicio.Listar()
                Dim detalles As New List(Of CarritoDetalle)()
                Dim items As New List(Of Object)()
                Dim subtotal As Decimal = 0D

                If carrito IsNot Nothing AndAlso carrito.CarritoId > 0 Then
                    detalles = FiltrarDetallesCarrito(_carritoDetalleServicio.Listar(), carrito.CarritoId)
                End If

                For Each det As CarritoDetalle In detalles
                    Dim producto As Producto = _productoServicio.ObtenerPorId(det.ProductoId)
                    Dim precio As Decimal = ObtenerPrecioProducto(det)
                    Dim subtotalLinea As Decimal = Math.Round(precio * det.Cantidad, 2)
                    subtotal += subtotalLinea

                    items.Add(New With {
                        .CarritoDetId = det.CarritoDetId,
                        .ProductoId = det.ProductoId,
                        .Cantidad = det.Cantidad,
                        .PrecioUnitario = precio,
                        .SubtotalLinea = subtotalLinea,
                        .Nombre = If(producto IsNot Nothing, producto.Nombre, "Producto"),
                        .ImagenUrl = If(producto IsNot Nothing, producto.ImagenUrl, "")
                    })
                Next

                Dim impuesto As Decimal = Math.Round(subtotal * 0.12D, 2)
                Dim descuento As Decimal = 0D
                Dim total As Decimal = Math.Round(subtotal + impuesto - descuento, 2)

                Dim tarjetasData As New List(Of Object)()
                For Each t As TarjetaCliente In tarjetas
                    tarjetasData.Add(New With {
                        .TarjetaClienteId = t.TarjetaClienteId,
                        .Marca = t.Marca,
                        .Titular = t.Titular,
                        .NumeroMascarado = "**** **** **** " & t.Ultimos4,
                        .EsPredeterminada = (t.Predeterminada = 1),
                        .Alias = t.AliasTarjeta
                    })
                Next

                Dim metodosData As New List(Of Object)()
                For Each m As Metodo_Pago In metodos
                    If String.Equals(m.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                        metodosData.Add(New With {
                            .MetodoPagoId = m.MetodoPagoId,
                            .Nombre = m.Nombre,
                            .RequiereDatosExtra = m.RequiereDatosExtra
                        })
                    End If
                Next

                Return Json(New With {
                    .ok = True,
                    .data = New With {
                        .Cliente = New With {
                            .CliId = cliId,
                            .Nombre = If(cliente IsNot Nothing, (cliente.Nombres & " " & cliente.Apellidos).Trim(), "Cliente"),
                            .Direccion = If(cliente IsNot Nothing, cliente.Direccion, "")
                        },
                        .CarritoId = If(carrito IsNot Nothing, carrito.CarritoId, 0),
                        .Items = items,
                        .Tarjetas = tarjetasData,
                        .MetodosPago = metodosData,
                        .Subtotal = subtotal,
                        .Impuesto = impuesto,
                        .Descuento = descuento,
                        .Total = total,
                        .Moneda = "GTQ"
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return JsonError("No se pudo obtener el checkout: " & LimpiarMensaje(ex.Message), 500, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Function ValidarCuponCheckoutData() As ActionResult
            If Session("UsuarioId") Is Nothing Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim codigo As String = ObtenerString(body, "codigo", "Codigo", "cupon", "Cupon")

                If String.IsNullOrWhiteSpace(codigo) Then
                    Return JsonError("Debe ingresar un codigo de cupon.")
                End If

                Dim cupones As List(Of Cupon) = _cuponServicio.Listar()
                Dim encontrado As Cupon = Nothing

                For Each c As Cupon In cupones
                    If c IsNot Nothing AndAlso String.Equals(c.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(c.Codigo, codigo.Trim(), StringComparison.OrdinalIgnoreCase) Then
                        Dim vigente As Boolean = (DateTime.Now >= c.VigenciaInicio AndAlso DateTime.Now <= c.VigenciaFin)
                        If vigente Then
                            encontrado = c
                            Exit For
                        End If
                    End If
                Next

                If encontrado Is Nothing Then
                    Return JsonError("El cupon no es valido o no se encuentra vigente.")
                End If

                Return Json(New With {
                    .ok = True,
                    .message = "Cupon valido. La tabla actual no incluye monto o porcentaje de descuento, por eso se aplicara 0.",
                    .data = New With {
                        .Codigo = encontrado.Codigo,
                        .Descuento = 0D
                    }
                })
            Catch ex As Exception
                Return JsonError("No se pudo validar el cupon: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpPost>
        Function ConfirmarPedidoDesdeCheckoutData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return JsonError("Sesion no valida.", 401)
            End If

            Try
                Dim body As JObject = LeerBodyComoJObject()
                Dim direccion As String = ObtenerString(body, "direccion", "Direccion")
                Dim metodoPagoId As Integer = ObtenerEntero(body, "metodoPagoId", "MetodoPagoId")
                Dim tarjetaClienteId As Integer = ObtenerEntero(body, "tarjetaClienteId", "TarjetaClienteId")
                Dim codigoCupon As String = ObtenerString(body, "codigoCupon", "CodigoCupon", "cupon", "Cupon")

                If String.IsNullOrWhiteSpace(direccion) Then
                    Return JsonError("Debe ingresar la direccion de entrega.")
                End If

                If metodoPagoId <= 0 Then
                    Return JsonError("Debe seleccionar un metodo de pago.")
                End If

                Dim carrito As Carrito = ObtenerCarritoActivoCliente(cliId, False)
                If carrito Is Nothing OrElse carrito.CarritoId <= 0 Then
                    Return JsonError("No existe un carrito activo para confirmar.")
                End If

                Dim detalles As List(Of CarritoDetalle) = FiltrarDetallesCarrito(_carritoDetalleServicio.Listar(), carrito.CarritoId)
                If detalles.Count = 0 Then
                    Return JsonError("El carrito no tiene productos.")
                End If

                Dim subtotal As Decimal = 0D
                For Each det As CarritoDetalle In detalles
                    subtotal += Math.Round(ObtenerPrecioProducto(det) * det.Cantidad, 2)
                Next

                Dim impuesto As Decimal = Math.Round(subtotal * 0.12D, 2)
                Dim descuento As Decimal = 0D
                Dim total As Decimal = Math.Round(subtotal + impuesto - descuento, 2)

                If Not String.IsNullOrWhiteSpace(codigoCupon) Then
                    Dim cupones As List(Of Cupon) = _cuponServicio.Listar()
                    For Each c As Cupon In cupones
                        If c IsNot Nothing AndAlso String.Equals(c.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(c.Codigo, codigoCupon.Trim(), StringComparison.OrdinalIgnoreCase) AndAlso DateTime.Now >= c.VigenciaInicio AndAlso DateTime.Now <= c.VigenciaFin Then
                            descuento = 0D
                            Exit For
                        End If
                    Next
                End If

                Dim estadoOrdenId As Integer = ObtenerEstadoOrdenInicialId()
                Dim numeroOrden As String = GenerarNumeroOrden(cliId)

                Dim orden As New Orden_Venta() With {
                    .NumOrden = numeroOrden,
                    .CliId = cliId,
                    .EstadoOrdenId = estadoOrdenId,
                    .FechaOrden = DateTime.Now,
                    .Subtotal = subtotal,
                    .Descuento = descuento,
                    .Impuesto = impuesto,
                    .Total = total,
                    .Moneda = "GTQ",
                    .DireccionEnvioSnapshot = direccion.Trim(),
                    .Observaciones = "Orden creada desde portal cliente web.",
                    .CreatedAt = DateTime.Now,
                    .Estado = "ACTIVO"
                }

                Dim ordenVentaId As Integer = _ordenVentaServicio.Insertar(orden)

                For Each det As CarritoDetalle In detalles
                    Dim precio As Decimal = ObtenerPrecioProducto(det)
                    Dim nuevoDetalle As New Orden_Venta_Detalle() With {
                        .OrdenVentaId = ordenVentaId,
                        .ProductoId = det.ProductoId,
                        .Cantidad = det.Cantidad,
                        .PrecioUnitarioSnapshot = precio,
                        .SubtotalLinea = Math.Round(precio * det.Cantidad, 2),
                        .CreatedAt = DateTime.Now,
                        .Estado = "ACTIVO"
                    }

                    _ordenVentaDetalleServicio.Insertar(nuevoDetalle)
                Next

                Dim referenciaPago As String = "Portal Cliente"
                If tarjetaClienteId > 0 Then
                    referenciaPago &= " - Tarjeta ID " & tarjetaClienteId.ToString()
                End If
                If Not String.IsNullOrWhiteSpace(codigoCupon) Then
                    referenciaPago &= " - Cupon " & codigoCupon.Trim()
                End If

                Dim pago As New Pago() With {
                    .OrdenVentaId = ordenVentaId,
                    .MetodoPagoId = metodoPagoId,
                    .Monto = total,
                    .EstadoPago = "APROBADO",
                    .Referencia = referenciaPago,
                    .PagoAt = DateTime.Now,
                    .CreatedAt = DateTime.Now,
                    .Estado = "ACTIVO"
                }

                _pagoServicio.Insertar(pago)

                carrito.EstadoCarrito = "CERRADO"
                carrito.UltimoCalculoAt = DateTime.Now
                _carritoServicio.Actualizar(carrito)

                Return Json(New With {
                    .ok = True,
                    .message = "Pedido confirmado correctamente.",
                    .data = New With {
                        .OrdenVentaId = ordenVentaId,
                        .NumOrden = numeroOrden
                    }
                })
            Catch ex As Exception
                Return JsonError("No se pudo confirmar el pedido: " & LimpiarMensaje(ex.Message), 500)
            End Try
        End Function

        <HttpGet>
        Function ObtenerMisOrdenesData() As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return Json(New With {
                    .ok = False,
                    .message = "Sesion no valida."
                }, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim ordenes As List(Of Orden_Venta) = _ordenVentaServicio.Listar()
                Dim envios As List(Of Envio) = _envioServicio.Listar()
                Dim data As New List(Of Object)()

                For Each orden As Orden_Venta In ordenes
                    If orden Is Nothing Then
                        Continue For
                    End If

                    If orden.CliId <> cliId Then
                        Continue For
                    End If

                    Dim envio As Envio = BuscarEnvioPorOrden(envios, orden.OrdenVentaId)
                    Dim estadoUi As String = NormalizarEstadoOrden(orden, envio)

                    data.Add(New With {
                        .OrdenVentaId = orden.OrdenVentaId,
                        .NumOrden = orden.NumOrden,
                        .FechaOrdenTexto = orden.FechaOrden.ToString("dd/MM/yyyy HH:mm"),
                        .Total = orden.Total,
                        .Moneda = If(String.IsNullOrWhiteSpace(orden.Moneda), "GTQ", orden.Moneda),
                        .Direccion = ObtenerDireccionOrden(orden, envio),
                        .EstadoUi = estadoUi,
                        .Resumen = ConstruirResumenOrden(orden),
                        .PuedeRastrear = (estadoUi <> "CANCELADA")
                    })
                Next

                Return Json(New With {
                    .ok = True,
                    .data = data
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {
                    .ok = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerDetalleOrdenData(ByVal id As Integer) As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return Json(New With {
                    .ok = False,
                    .message = "Sesion no valida."
                }, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim orden As Orden_Venta = _ordenVentaServicio.ObtenerPorId(id)

                If orden Is Nothing OrElse orden.OrdenVentaId <= 0 Then
                    Return Json(New With {
                        .ok = False,
                        .message = "Orden no encontrada."
                    }, JsonRequestBehavior.AllowGet)
                End If

                If orden.CliId <> cliId Then
                    Return Json(New With {
                        .ok = False,
                        .message = "No tienes acceso a esta orden."
                    }, JsonRequestBehavior.AllowGet)
                End If

                Dim detallesTodos As List(Of Orden_Venta_Detalle) = _ordenVentaDetalleServicio.Listar()
                Dim detalles As List(Of Orden_Venta_Detalle) = FiltrarDetallesPorOrden(detallesTodos, orden.OrdenVentaId)
                Dim envios As List(Of Envio) = _envioServicio.Listar()
                Dim envio As Envio = BuscarEnvioPorOrden(envios, orden.OrdenVentaId)
                Dim items As New List(Of Object)()

                For Each det As Orden_Venta_Detalle In detalles
                    Dim nombreProducto As String = "Producto"
                    Dim imagenUrl As String = String.Empty

                    Try
                        Dim producto As Producto = _productoServicio.ObtenerPorId(det.ProductoId)
                        If producto IsNot Nothing Then
                            nombreProducto = producto.Nombre
                            imagenUrl = producto.ImagenUrl
                        End If
                    Catch
                    End Try

                    items.Add(New With {
                        .OrdenVentaDetId = det.OrdenVentaDetId,
                        .ProductoId = det.ProductoId,
                        .Nombre = nombreProducto,
                        .ImagenUrl = imagenUrl,
                        .Cantidad = det.Cantidad,
                        .PrecioUnitario = det.PrecioUnitarioSnapshot,
                        .SubtotalLinea = det.SubtotalLinea
                    })
                Next

                Return Json(New With {
                    .ok = True,
                    .data = New With {
                        .OrdenVentaId = orden.OrdenVentaId,
                        .NumOrden = orden.NumOrden,
                        .FechaOrdenTexto = orden.FechaOrden.ToString("dd/MM/yyyy HH:mm"),
                        .EstadoUi = NormalizarEstadoOrden(orden, envio),
                        .Moneda = If(String.IsNullOrWhiteSpace(orden.Moneda), "GTQ", orden.Moneda),
                        .Subtotal = orden.Subtotal,
                        .Descuento = orden.Descuento,
                        .Impuesto = orden.Impuesto,
                        .Total = orden.Total,
                        .Direccion = ObtenerDireccionOrden(orden, envio),
                        .TrackingCodigo = If(envio IsNot Nothing, envio.TrackingCodigo, String.Empty),
                        .Observaciones = If(String.IsNullOrWhiteSpace(orden.Observaciones), String.Empty, orden.Observaciones),
                        .Items = items
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {
                    .ok = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Function ObtenerTrackingOrdenData(ByVal ordenVentaId As Integer) As ActionResult
            Dim cliId As Integer = 0

            If Not TryObtenerCliIdAutenticado(cliId) Then
                Return Json(New With {
                    .ok = False,
                    .message = "Sesion no valida."
                }, JsonRequestBehavior.AllowGet)
            End If

            Try
                Dim orden As Orden_Venta = _ordenVentaServicio.ObtenerPorId(ordenVentaId)

                If orden Is Nothing OrElse orden.OrdenVentaId <= 0 Then
                    Return Json(New With {
                        .ok = False,
                        .message = "Orden no encontrada."
                    }, JsonRequestBehavior.AllowGet)
                End If

                If orden.CliId <> cliId Then
                    Return Json(New With {
                        .ok = False,
                        .message = "No tienes acceso a esta orden."
                    }, JsonRequestBehavior.AllowGet)
                End If

                Dim envios As List(Of Envio) = _envioServicio.Listar()
                Dim envio As Envio = BuscarEnvioPorOrden(envios, orden.OrdenVentaId)
                Dim timeline As New List(Of Object)()

                If envio IsNot Nothing AndAlso envio.EnvioId > 0 Then
                    Dim seguimientoTodos As List(Of SeguimientoEnvio) = _seguimientoEnvioServicio.Listar()
                    Dim seguimiento As List(Of SeguimientoEnvio) = FiltrarSeguimientoPorEnvio(seguimientoTodos, envio.EnvioId)
                    OrdenarSeguimientoAsc(seguimiento)

                    If seguimiento.Count > 0 Then
                        timeline = ConstruirTimelineDesdeSeguimiento(seguimiento, NormalizarEstadoOrden(orden, envio))
                    Else
                        timeline = ConstruirTimelineFallback(orden, envio)
                    End If
                Else
                    timeline = ConstruirTimelineFallback(orden, Nothing)
                End If

                Return Json(New With {
                    .ok = True,
                    .data = New With {
                        .OrdenVentaId = orden.OrdenVentaId,
                        .NumOrden = orden.NumOrden,
                        .TrackingCodigo = If(envio IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(envio.TrackingCodigo), envio.TrackingCodigo, "No disponible"),
                        .EstadoUi = NormalizarEstadoOrden(orden, envio),
                        .FechaEntregaEstimadaTexto = If(envio IsNot Nothing AndAlso envio.FechaEntregaEstimada.HasValue, envio.FechaEntregaEstimada.Value.ToString("dd/MM/yyyy"), "No disponible"),
                        .Timeline = timeline
                    }
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {
                    .ok = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <NonAction>
        Private Function ValidarSesionCliente() As ActionResult
            If Session("UsuarioId") Is Nothing Then
                TempData("Error") = "Debe iniciar sesion para acceder al portal del cliente."
                Return RedirectToAction("Login", "Home")
            End If

            Dim rolId As Integer = ObtenerRolIdDesdeSesion()
            If rolId <> 3 Then
                TempData("Error") = "No tiene permisos para acceder al portal del cliente."
                Return RedirectToAction("Login", "Home")
            End If

            Dim cliId As Integer? = ObtenerCliIdDesdeSesion()
            If Not cliId.HasValue Then
                Session.Clear()
                Session.Abandon()
                TempData("Error") = "La sesion del cliente no es valida porque no tiene CLI_ID asociado."
                Return RedirectToAction("Login", "Home")
            End If

            ViewData("UsuarioId") = Convert.ToInt32(Session("UsuarioId"))
            ViewData("RolId") = rolId
            ViewData("CliId") = cliId.Value
            ViewData("Username") = If(Session("Username") IsNot Nothing, Session("Username").ToString(), String.Empty)

            Return Nothing
        End Function

        <NonAction>
        Private Function TryObtenerCliIdAutenticado(ByRef cliId As Integer) As Boolean
            cliId = 0

            If Session("UsuarioId") Is Nothing Then
                Return False
            End If

            If ObtenerRolIdDesdeSesion() <> 3 Then
                Return False
            End If

            Dim cliIdSesion As Integer? = ObtenerCliIdDesdeSesion()
            If Not cliIdSesion.HasValue Then
                Return False
            End If

            cliId = cliIdSesion.Value
            Return True
        End Function

        <NonAction>
        Private Function ObtenerRolIdDesdeSesion() As Integer
            Dim rolId As Integer = 0

            If Session("RolId") IsNot Nothing Then
                Integer.TryParse(Session("RolId").ToString(), rolId)
            End If

            Return rolId
        End Function

        <NonAction>
        Private Function ObtenerCliIdDesdeSesion() As Integer?
            If Session("CliId") Is Nothing Then
                Return Nothing
            End If

            Dim cliId As Integer = 0
            If Integer.TryParse(Session("CliId").ToString(), cliId) AndAlso cliId > 0 Then
                Return cliId
            End If

            Return Nothing
        End Function

        <NonAction>
        Private Function LeerBodyComoJObject() As JObject
            Dim body As String = String.Empty

            If Request IsNot Nothing AndAlso Request.InputStream IsNot Nothing Then
                If Request.InputStream.CanSeek Then
                    Request.InputStream.Position = 0
                End If

                Using reader As New StreamReader(Request.InputStream)
                    body = reader.ReadToEnd()
                End Using
            End If

            If String.IsNullOrWhiteSpace(body) Then
                Return New JObject()
            End If

            Return JObject.Parse(body)
        End Function

        <NonAction>
        Private Function ObtenerString(ByVal body As JObject, ParamArray keys() As String) As String
            For Each key As String In keys
                Dim token As JToken = body(key)
                If token IsNot Nothing AndAlso token.Type <> JTokenType.Null Then
                    Return token.ToString()
                End If
            Next

            Return String.Empty
        End Function

        <NonAction>
        Private Function ObtenerEntero(ByVal body As JObject, ParamArray keys() As String) As Integer
            For Each key As String In keys
                Dim token As JToken = body(key)
                Dim valor As Integer = 0

                If token IsNot Nothing AndAlso Integer.TryParse(token.ToString(), valor) Then
                    Return valor
                End If
            Next

            Return 0
        End Function

        <NonAction>
        Private Function SoloDigitos(ByVal valor As String) As String
            If String.IsNullOrWhiteSpace(valor) Then
                Return String.Empty
            End If

            Dim resultado As New StringBuilder()

            For Each ch As Char In valor
                If Char.IsDigit(ch) Then
                    resultado.Append(ch)
                End If
            Next

            Return resultado.ToString()
        End Function

        <NonAction>
        Private Function CalcularScoreProducto(ByVal producto As Producto,
                                               ByVal pesosCategoria As Dictionary(Of Integer, Decimal),
                                               ByVal pesosTipo As Dictionary(Of String, Decimal)) As Decimal
            Dim score As Decimal = 0D

            If pesosCategoria.ContainsKey(producto.CategoriaId) Then
                score += pesosCategoria(producto.CategoriaId)
            End If

            If Not String.IsNullOrWhiteSpace(producto.Tipo) Then
                Dim tipoClave As String = producto.Tipo.Trim().ToUpperInvariant()
                If pesosTipo.ContainsKey(tipoClave) Then
                    score += pesosTipo(tipoClave)
                End If
            End If

            Return score
        End Function

        <NonAction>
        Private Function BuscarEnvioPorOrden(ByVal envios As List(Of Envio), ByVal ordenVentaId As Integer) As Envio
            If envios Is Nothing Then
                Return Nothing
            End If

            For Each item As Envio In envios
                If item IsNot Nothing AndAlso item.OrdenVentaId = ordenVentaId Then
                    Return item
                End If
            Next

            Return Nothing
        End Function

        <NonAction>
        Private Function FiltrarDetallesPorOrden(ByVal lista As List(Of Orden_Venta_Detalle), ByVal ordenVentaId As Integer) As List(Of Orden_Venta_Detalle)
            Dim resultado As New List(Of Orden_Venta_Detalle)()

            If lista Is Nothing Then
                Return resultado
            End If

            For Each item As Orden_Venta_Detalle In lista
                If item IsNot Nothing AndAlso item.OrdenVentaId = ordenVentaId Then
                    resultado.Add(item)
                End If
            Next

            Return resultado
        End Function

        <NonAction>
        Private Function FiltrarSeguimientoPorEnvio(ByVal lista As List(Of SeguimientoEnvio), ByVal envioId As Integer) As List(Of SeguimientoEnvio)
            Dim resultado As New List(Of SeguimientoEnvio)()

            If lista Is Nothing Then
                Return resultado
            End If

            For Each item As SeguimientoEnvio In lista
                If item IsNot Nothing AndAlso item.EnvioId = envioId Then
                    resultado.Add(item)
                End If
            Next

            Return resultado
        End Function

        <NonAction>
        Private Sub OrdenarSeguimientoAsc(ByVal lista As List(Of SeguimientoEnvio))
            If lista Is Nothing OrElse lista.Count <= 1 Then
                Exit Sub
            End If

            Dim i As Integer
            Dim j As Integer

            For i = 0 To lista.Count - 2
                For j = 0 To lista.Count - 2 - i
                    Dim a As DateTime = If(lista(j).EventoAt.HasValue, lista(j).EventoAt.Value, DateTime.MinValue)
                    Dim b As DateTime = If(lista(j + 1).EventoAt.HasValue, lista(j + 1).EventoAt.Value, DateTime.MinValue)

                    If a > b Then
                        Dim temp As SeguimientoEnvio = lista(j)
                        lista(j) = lista(j + 1)
                        lista(j + 1) = temp
                    End If
                Next
            Next
        End Sub

        <NonAction>
        Private Function ConstruirTimelineDesdeSeguimiento(ByVal seguimiento As List(Of SeguimientoEnvio), ByVal estadoFinal As String) As List(Of Object)
            Dim timeline As New List(Of Object)()
            Dim i As Integer

            For i = 0 To seguimiento.Count - 1
                Dim item As SeguimientoEnvio = seguimiento(i)
                Dim estadoPaso As String = "done"

                If i = seguimiento.Count - 1 AndAlso estadoFinal <> "ENTREGADA" Then
                    estadoPaso = "current"
                End If

                Dim titulo As String = ObtenerPrimerTexto(item.Observacion, item.UbicacionTexto, "Actualizacion " & (i + 1).ToString())
                Dim subtitulo As String = String.Empty

                If item.EventoAt.HasValue Then
                    subtitulo = item.EventoAt.Value.ToString("dd/MM/yyyy HH:mm")
                End If

                If Not String.IsNullOrWhiteSpace(item.UbicacionTexto) Then
                    If subtitulo <> String.Empty Then
                        subtitulo &= " - "
                    End If
                    subtitulo &= item.UbicacionTexto
                End If

                timeline.Add(New With {
                    .Titulo = titulo,
                    .Subtitulo = subtitulo,
                    .EstadoPaso = estadoPaso
                })
            Next

            Return timeline
        End Function

        <NonAction>
        Private Function ConstruirTimelineFallback(ByVal orden As Orden_Venta, ByVal envio As Envio) As List(Of Object)
            Dim timeline As New List(Of Object)()
            Dim estado As String = NormalizarEstadoOrden(orden, envio)

            timeline.Add(New With {
                .Titulo = "Pedido confirmado",
                .Subtitulo = orden.FechaOrden.ToString("dd/MM/yyyy HH:mm"),
                .EstadoPaso = "done"
            })

            timeline.Add(New With {
                .Titulo = "Preparacion del pedido",
                .Subtitulo = If(String.IsNullOrWhiteSpace(orden.Observaciones), "Orden en proceso interno.", orden.Observaciones),
                .EstadoPaso = If(estado = "PENDIENTE", "current", "done")
            })

            timeline.Add(New With {
                .Titulo = "Salida a entrega",
                .Subtitulo = If(envio IsNot Nothing AndAlso envio.FechaEnvio.HasValue, envio.FechaEnvio.Value.ToString("dd/MM/yyyy"), "Pendiente de despacho"),
                .EstadoPaso = If(estado = "EN CAMINO", "current", If(estado = "ENTREGADA", "done", "pending"))
            })

            timeline.Add(New With {
                .Titulo = "Entrega final",
                .Subtitulo = If(envio IsNot Nothing AndAlso envio.FechaEntregaReal.HasValue, envio.FechaEntregaReal.Value.ToString("dd/MM/yyyy"), "Pendiente"),
                .EstadoPaso = If(estado = "ENTREGADA", "done", "pending")
            })

            Return timeline
        End Function

        <NonAction>
        Private Function NormalizarEstadoOrden(ByVal orden As Orden_Venta, ByVal envio As Envio) As String
            Dim observaciones As String = String.Empty

            If orden IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(orden.Observaciones) Then
                observaciones = orden.Observaciones.ToUpperInvariant()
            End If

            If observaciones.Contains("CANCELADA") Then
                Return "CANCELADA"
            End If

            If envio IsNot Nothing AndAlso envio.FechaEntregaReal.HasValue Then
                Return "ENTREGADA"
            End If

            If observaciones.Contains("ENTREGADA") Then
                Return "ENTREGADA"
            End If

            If envio IsNot Nothing AndAlso (envio.FechaEnvio.HasValue OrElse Not String.IsNullOrWhiteSpace(envio.TrackingCodigo)) Then
                Return "EN CAMINO"
            End If

            If observaciones.Contains("EN_PROCESO") OrElse observaciones.Contains("PREPARANDO") OrElse observaciones.Contains("PROCESO") Then
                Return "PROCESANDO"
            End If

            Return "PENDIENTE"
        End Function

        <NonAction>
        Private Function ObtenerDireccionOrden(ByVal orden As Orden_Venta, ByVal envio As Envio) As String
            If envio IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(envio.DireccionEntregaSnapshot) Then
                Return envio.DireccionEntregaSnapshot
            End If

            If orden IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(orden.DireccionEnvioSnapshot) Then
                Return orden.DireccionEnvioSnapshot
            End If

            Return "No disponible"
        End Function

        <NonAction>
        Private Function ConstruirResumenOrden(ByVal orden As Orden_Venta) As String
            If orden Is Nothing Then
                Return "Orden registrada"
            End If

            If Not String.IsNullOrWhiteSpace(orden.Observaciones) Then
                Return orden.Observaciones
            End If

            Return "Orden registrada en el sistema."
        End Function

        <NonAction>
        Private Function ObtenerPrimerTexto(ByVal valor1 As String, ByVal valor2 As String, ByVal valor3 As String) As String
            If Not String.IsNullOrWhiteSpace(valor1) Then
                Return valor1
            End If

            If Not String.IsNullOrWhiteSpace(valor2) Then
                Return valor2
            End If

            Return valor3
        End Function

        <NonAction>
        Private Function ObtenerCarritoActivoCliente(ByVal cliId As Integer, ByVal crearSiNoExiste As Boolean) As Carrito
            Dim carritos As List(Of Carrito) = _carritoServicio.Listar()

            For Each c As Carrito In carritos
                If c Is Nothing Then
                    Continue For
                End If

                If c.CliId = cliId AndAlso String.Equals(c.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    If String.IsNullOrWhiteSpace(c.EstadoCarrito) OrElse Not String.Equals(c.EstadoCarrito, "CERRADO", StringComparison.OrdinalIgnoreCase) Then
                        Return c
                    End If
                End If
            Next

            If crearSiNoExiste Then
                Dim nuevo As New Carrito() With {
                    .CliId = cliId,
                    .EstadoCarrito = "ABIERTO",
                    .UltimoCalculoAt = DateTime.Now,
                    .Estado = "ACTIVO"
                }

                Dim idGenerado As Integer = _carritoServicio.Insertar(nuevo)
                Return _carritoServicio.ObtenerPorId(idGenerado)
            End If

            Return Nothing
        End Function

        <NonAction>
        Private Function FiltrarDetallesCarrito(ByVal lista As List(Of CarritoDetalle), ByVal carritoId As Integer) As List(Of CarritoDetalle)
            Dim resultado As New List(Of CarritoDetalle)()

            For Each det As CarritoDetalle In lista
                If det Is Nothing Then
                    Continue For
                End If

                If det.CarritoId = carritoId AndAlso String.Equals(det.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    resultado.Add(det)
                End If
            Next

            Return resultado
        End Function

        <NonAction>
        Private Function ObtenerPrecioProducto(ByVal detalle As CarritoDetalle) As Decimal
            If detalle IsNot Nothing AndAlso detalle.PrecioUnitarioSnapshot.HasValue AndAlso detalle.PrecioUnitarioSnapshot.Value > 0D Then
                Return detalle.PrecioUnitarioSnapshot.Value
            End If

            If detalle IsNot Nothing Then
                Return ObtenerPrecioActualProducto(detalle.ProductoId)
            End If

            Return 0D
        End Function

        <NonAction>
        Private Function ObtenerPrecioActualProducto(ByVal productoId As Integer) As Decimal
            Dim precios As List(Of Precio_Historico) = _precioHistoricoServicio.Listar()
            Dim vigente As Precio_Historico = Nothing

            For Each p As Precio_Historico In precios
                If p Is Nothing Then
                    Continue For
                End If

                If p.ProductoId <> productoId Then
                    Continue For
                End If

                If Not String.Equals(p.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                Dim vigenteHoy As Boolean = (DateTime.Now >= p.VigenciaInicio AndAlso (Not p.VigenciaFin.HasValue OrElse DateTime.Now <= p.VigenciaFin.Value))
                If Not vigenteHoy Then
                    Continue For
                End If

                If vigente Is Nothing Then
                    vigente = p
                ElseIf p.VigenciaInicio > vigente.VigenciaInicio Then
                    vigente = p
                End If
            Next

            If vigente IsNot Nothing Then
                Return vigente.Precio
            End If

            Return 0D
        End Function

        <NonAction>
        Private Function ObtenerEstadoOrdenInicialId() As Integer
            Dim estados As List(Of Estado_Orden) = _estadoOrdenServicio.Listar()

            For Each e As Estado_Orden In estados
                If e Is Nothing Then
                    Continue For
                End If

                If Not String.Equals(e.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                If String.Equals(e.Codigo, "PENDIENTE", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(e.Codigo, "NUEVA", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(e.Codigo, "REGISTRADA", StringComparison.OrdinalIgnoreCase) Then
                    Return e.EstadoOrdenId
                End If
            Next

            For Each e As Estado_Orden In estados
                If e IsNot Nothing AndAlso String.Equals(e.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    Return e.EstadoOrdenId
                End If
            Next

            Return 1
        End Function

        <NonAction>
        Private Function GenerarNumeroOrden(ByVal cliId As Integer) As String
            Return "OV-" & cliId.ToString() & "-" & DateTime.Now.ToString("yyyyMMddHHmmss")
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
                Return "Ocurrio un error inesperado."
            End If

            Dim partes() As String = message.Replace(vbCrLf, vbLf).Split(ControlChars.Lf)
            Return partes(0).Trim()
        End Function

        Private Class ProductoConScore
            Public Property Producto As Producto
            Public Property Score As Decimal
        End Class
    End Class
End Namespace
