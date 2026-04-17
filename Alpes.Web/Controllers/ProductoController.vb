Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Inventario
Imports System.Linq
Imports Alpes.Web.Models

Public Class ProductoController
    Inherits Controller

    Private ReadOnly _servicio As ProductoServicio

    Public Sub New()
        _servicio = New ProductoServicio()
    End Sub

    <HttpGet>
    Public Function Index() As ActionResult
        Try
            Dim lista As List(Of Producto) = _servicio.Listar()

            Dim categoriaServicio As New CategoriaServicio()
            Dim unidadServicio As New Unidad_MedidaServicio()

            Dim categorias = categoriaServicio.Listar()
            Dim unidades = unidadServicio.Listar()

            Dim resultado = lista.Select(Function(p) New ProductoListadoViewModel With {
            .ProductoId = p.ProductoId,
            .Referencia = p.Referencia,
            .Nombre = p.Nombre,
            .Descripcion = p.Descripcion,
            .Tipo = p.Tipo,
            .Material = p.Material,
            .AltoCm = p.AltoCm,
            .AnchoCm = p.AnchoCm,
            .ProfundidadCm = p.ProfundidadCm,
            .Color = p.Color,
            .PesoGramos = p.PesoGramos,
            .ImagenUrl = p.ImagenUrl,
            .UnidadMedidaId = p.UnidadMedidaId,
            .UnidadMedidaNombre = If(
                p.UnidadMedidaId.HasValue,
                unidades.Where(Function(u) u.UnidadMedidaId = p.UnidadMedidaId.Value).
                         Select(Function(u) u.Nombre).
                         FirstOrDefault(),
                ""
            ),
            .CategoriaId = p.CategoriaId,
            .CategoriaNombre = categorias.Where(Function(c) c.CategoriaId = p.CategoriaId).
                                         Select(Function(c) c.Nombre).
                                         FirstOrDefault(),
            .LoteProducto = p.LoteProducto,
            .Estado = p.Estado
        }).ToList()

            Return Json(resultado, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
            .success = False,
            .message = ex.Message
        }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As Producto = _servicio.ObtenerPorId(id)

            Return Json(entidad, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Buscar(ByVal criterio As String, ByVal valor As String) As ActionResult
        Try
            Dim lista As List(Of Producto) = _servicio.Buscar(criterio, valor)

            Return Json(lista, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Public Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = LeerBody()
            Dim entidad As Producto = JsonConvert.DeserializeObject(Of Producto)(jsonBody)

            If entidad Is Nothing Then
                Throw New Exception("No se recibieron datos válidos para insertar.")
            End If

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Registro insertado correctamente.",
                .id = idGenerado
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

    <HttpPost>
    Public Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = LeerBody()
            Dim entidad As Producto = JsonConvert.DeserializeObject(Of Producto)(jsonBody)

            If entidad Is Nothing Then
                Throw New Exception("No se recibieron datos válidos para actualizar.")
            End If

            If entidad.ProductoId <= 0 Then
                Throw New Exception("Debe enviar un ProductoId válido para actualizar.")
            End If

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Registro actualizado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

    <HttpPost>
    Public Function Eliminar() As ActionResult
        Try
            Dim productoId As Integer = ObtenerProductoIdDesdeRequest()

            If productoId <= 0 Then
                Throw New Exception("Debe enviar un ProductoId válido para eliminar.")
            End If

            _servicio.Eliminar(productoId)

            Return Json(New With {
                .success = True,
                .message = "Registro eliminado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

    Private Function LeerBody() As String
        If Request Is Nothing OrElse Request.InputStream Is Nothing Then
            Return String.Empty
        End If

        If Request.InputStream.CanSeek Then
            Request.InputStream.Position = 0
        End If

        Using reader As New StreamReader(Request.InputStream)
            Return reader.ReadToEnd()
        End Using
    End Function

    Private Function ObtenerProductoIdDesdeRequest() As Integer
        Dim id As Integer = 0

        If Integer.TryParse(Convert.ToString(Request("ProductoId")), id) AndAlso id > 0 Then
            Return id
        End If

        If Integer.TryParse(Convert.ToString(Request("producto_id")), id) AndAlso id > 0 Then
            Return id
        End If

        If Integer.TryParse(Convert.ToString(Request("id")), id) AndAlso id > 0 Then
            Return id
        End If

        Dim jsonBody As String = LeerBody()

        If String.IsNullOrWhiteSpace(jsonBody) Then
            Return 0
        End If

        Try
            Dim obj As JObject = JObject.Parse(jsonBody)
            Dim token As JToken = obj("ProductoId")

            If token Is Nothing Then
                token = obj("producto_id")
            End If

            If token Is Nothing Then
                token = obj("id")
            End If

            If token IsNot Nothing AndAlso Integer.TryParse(token.ToString(), id) AndAlso id > 0 Then
                Return id
            End If
        Catch
        End Try

        Try
            id = JsonConvert.DeserializeObject(Of Integer)(jsonBody)
            If id > 0 Then
                Return id
            End If
        Catch
        End Try

        Return 0
    End Function

End Class