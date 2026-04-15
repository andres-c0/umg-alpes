Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Produccion

Public Class HerramientaController
    Inherits Controller

    Private ReadOnly _servicio As HerramientaServicio

    Public Sub New()
        _servicio = New HerramientaServicio()
    End Sub

    Function Index() As ActionResult
        Try
            Dim lista As List(Of Herramienta) = _servicio.Listar()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As Herramienta = _servicio.ObtenerPorId(id)

            If entidad Is Nothing Then
                Return Json(New With {
                    .success = False,
                    .message = "Registro no encontrado."
                }, JsonRequestBehavior.AllowGet)
            End If

            Return Json(New With {
                .success = True,
                .data = entidad
            }, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Function Buscar(ByVal valor As String) As ActionResult
        Try
            Dim lista As List(Of Herramienta) = _servicio.Buscar(valor)

            Return Json(New With {
                .success = True,
                .data = lista
            }, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Herramienta = JsonConvert.DeserializeObject(Of Herramienta)(jsonBody)

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
    Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Herramienta = JsonConvert.DeserializeObject(Of Herramienta)(jsonBody)

            If entidad Is Nothing OrElse entidad.HerramientaId <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar HerramientaId para actualizar."
                })
            End If

            Dim actualizado As Boolean = _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = actualizado,
                .message = If(actualizado, "Registro actualizado correctamente.", "No se pudo actualizar el registro.")
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
            Dim jObj As JObject = JsonConvert.DeserializeObject(Of JObject)(jsonBody)

            If jObj Is Nothing Then
                Return Json(New With {
                    .success = False,
                    .message = "No se recibió información para eliminar."
                })
            End If

            Dim id As Integer = 0

            If jObj("HerramientaId") IsNot Nothing Then
                id = Convert.ToInt32(jObj("HerramientaId").ToString())
            ElseIf jObj("id") IsNot Nothing Then
                id = Convert.ToInt32(jObj("id").ToString())
            End If

            If id <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar HerramientaId o id."
                })
            End If

            Dim eliminado As Boolean = _servicio.Eliminar(id)

            Return Json(New With {
                .success = eliminado,
                .message = If(eliminado, "Registro eliminado correctamente.", "No se pudo eliminar el registro.")
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

End Class