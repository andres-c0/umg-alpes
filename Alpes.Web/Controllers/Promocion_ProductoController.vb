Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Ventas

Public Class PromocionProductoController
    Inherits Controller

    Private ReadOnly _servicio As PromocionProductoServicio

    Public Sub New()
        _servicio = New PromocionProductoServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of PromocionProducto) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Dim entidad As PromocionProducto = _servicio.ObtenerPorId(id)
        Return Json(entidad, JsonRequestBehavior.AllowGet)
    End Function

    Function Buscar(ByVal criterio As String, ByVal valor As String) As ActionResult
        Dim lista As List(Of PromocionProducto) = _servicio.Buscar(criterio, valor)
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As PromocionProducto = JsonConvert.DeserializeObject(Of PromocionProducto)(jsonBody)

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Promocion_Producto insertado correctamente.",
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
    Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As PromocionProducto = JsonConvert.DeserializeObject(Of PromocionProducto)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Promocion_Producto actualizado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

    <HttpPost>
    Function Eliminar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim datos = JsonConvert.DeserializeObject(Of Dictionary(Of String, Integer))(jsonBody)

            Dim id As Integer = datos("id")
            _servicio.Eliminar(id)

            Return Json(New With {
                .success = True,
                .message = "Promocion_Producto eliminado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

End Class