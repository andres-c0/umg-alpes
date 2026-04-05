Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.RecursosHumanos

Public Class Incidente_LaboralController
    Inherits Controller

    Private ReadOnly _servicio As Incidente_LaboralServicio

    Public Sub New()
        _servicio = New Incidente_LaboralServicio()
    End Sub

    Function Index() As ActionResult
        Return Json(_servicio.Listar(), JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Return Json(_servicio.ObtenerPorId(id), JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Incidente_Laboral = JsonConvert.DeserializeObject(Of Incidente_Laboral)(jsonBody)

            Dim id As Integer = _servicio.Insertar(entidad)

            Return Json(New With {.success = True, .id = id})
        Catch ex As Exception
            Return Json(New With {.success = False, .message = ex.Message})
        End Try
    End Function

    <HttpPost>
    Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Incidente_Laboral = JsonConvert.DeserializeObject(Of Incidente_Laboral)(jsonBody)

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
            Dim data As Dictionary(Of String, Integer) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Integer))(jsonBody)

            _servicio.Eliminar(data("id"))

            Return Json(New With {.success = True})
        Catch ex As Exception
            Return Json(New With {.success = False, .message = ex.Message})
        End Try
    End Function

End Class