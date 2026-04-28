Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Ventas

Public Class CarritoController
    Inherits Controller

    Private ReadOnly _servicio As CarritoServicio

    Public Sub New()
        _servicio = New CarritoServicio()
    End Sub

    Function Index() As ActionResult
        Try
            Dim lista As List(Of Carrito) = _servicio.Listar()
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
            Dim entidad As Carrito = _servicio.ObtenerPorId(id)

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
    Function Buscar(ByVal criterio As String, ByVal valor As String) As ActionResult
        Try
            Dim lista As List(Of Carrito) = _servicio.Buscar(criterio, valor)

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
            Dim entidad As Carrito = JsonConvert.DeserializeObject(Of Carrito)(jsonBody)

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
            Dim entidad As Carrito = JsonConvert.DeserializeObject(Of Carrito)(jsonBody)

            If entidad Is Nothing OrElse entidad.CarritoId <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar CarritoId para actualizar."
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

            If jObj("CarritoId") IsNot Nothing Then
                id = Convert.ToInt32(jObj("CarritoId").ToString())
            ElseIf jObj("id") IsNot Nothing Then
                id = Convert.ToInt32(jObj("id").ToString())
            End If

            If id <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar CarritoId o id."
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