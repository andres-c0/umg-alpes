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
        Return Json(_servicio.Listar(), JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(id As Integer) As ActionResult
        Return Json(_servicio.ObtenerPorId(id), JsonRequestBehavior.AllowGet)
    End Function

    Function Buscar(valor As String) As ActionResult
        Return Json(_servicio.Buscar(valor), JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Orden_Venta = JsonConvert.DeserializeObject(Of Orden_Venta)(jsonBody)

            Dim idGenerado = _servicio.Insertar(entidad)

            Return Json(New With {.success = True, .id = idGenerado})
        Catch ex As Exception
            Return Json(New With {.success = False, .message = ex.Message})
        End Try
    End Function

    <HttpPost>
    Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Orden_Venta = JsonConvert.DeserializeObject(Of Orden_Venta)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {.success = True})
        Catch ex As Exception
            Return Json(New With {.success = False, .message = ex.Message})
        End Try
    End Function

    <HttpPost>
    Function Eliminar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim data = JsonConvert.DeserializeObject(Of Dictionary(Of String, Integer))(jsonBody)

            _servicio.Eliminar(data("id"))

            Return Json(New With {.success = True})
        Catch ex As Exception
            Return Json(New With {.success = False, .message = ex.Message})
        End Try
    End Function

End Class