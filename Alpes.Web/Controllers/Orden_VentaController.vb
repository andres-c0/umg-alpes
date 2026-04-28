Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Ventas

Public Class Orden_VentaController
    Inherits Controller

    Private ReadOnly _servicio As Orden_VentaServicio

    Public Sub New()
        _servicio = New Orden_VentaServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of Orden_Venta) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Dim entidad As Orden_Venta = _servicio.ObtenerPorId(id)
        Return Json(entidad, JsonRequestBehavior.AllowGet)
    End Function

    Function Buscar(ByVal valor As String) As ActionResult
        Dim lista As List(Of Orden_Venta) = _servicio.Buscar(valor)
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Orden_Venta = JsonConvert.DeserializeObject(Of Orden_Venta)(jsonBody)

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Orden_Venta insertada correctamente.",
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
            Dim entidad As Orden_Venta = JsonConvert.DeserializeObject(Of Orden_Venta)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Orden_Venta actualizada correctamente."
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
                .message = "Orden_Venta eliminada correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

End Class