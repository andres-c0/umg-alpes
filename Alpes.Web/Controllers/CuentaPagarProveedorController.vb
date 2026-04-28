Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Compras

Public Class CuentaPagarProveedorController
    Inherits Controller

    Private ReadOnly _servicio As CuentaPagarProveedorServicio

    Public Sub New()
        _servicio = New CuentaPagarProveedorServicio()
    End Sub

    Function Index() As ActionResult
        Try
            Dim lista As List(Of CuentaPagarProveedor) = _servicio.Listar()
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
            Dim entidad As CuentaPagarProveedor = _servicio.ObtenerPorId(id)

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
            Dim lista As List(Of CuentaPagarProveedor) = _servicio.Buscar(criterio, valor)

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
            Dim entidad As CuentaPagarProveedor = JsonConvert.DeserializeObject(Of CuentaPagarProveedor)(jsonBody)

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
            Dim entidad As CuentaPagarProveedor = JsonConvert.DeserializeObject(Of CuentaPagarProveedor)(jsonBody)

            If entidad Is Nothing OrElse entidad.CuentaPagarId <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar CuentaPagarId para actualizar."
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

            If jObj("CuentaPagarId") IsNot Nothing Then
                id = Convert.ToInt32(jObj("CuentaPagarId").ToString())
            ElseIf jObj("id") IsNot Nothing Then
                id = Convert.ToInt32(jObj("id").ToString())
            End If

            If id <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar CuentaPagarId o id."
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