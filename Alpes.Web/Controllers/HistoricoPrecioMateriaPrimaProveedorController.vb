Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Compras

Public Class HistoricoPrecioMateriaPrimaProveedorController
    Inherits Controller

    Private ReadOnly _servicio As HistoricoPrecioMateriaPrimaProveedorServicio

    Public Sub New()
        _servicio = New HistoricoPrecioMateriaPrimaProveedorServicio()
    End Sub

    Function Index() As ActionResult
        Try
            Dim lista As List(Of HistoricoPrecioMateriaPrimaProveedor) = _servicio.Listar()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Function ListarPorProveedor(ByVal provId As Integer) As ActionResult
        Try
            Dim lista As List(Of HistoricoPrecioMateriaPrimaProveedor) = _servicio.ListarPorProveedor(provId)

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
            Dim entidad As HistoricoPrecioMateriaPrimaProveedor = JsonConvert.DeserializeObject(Of HistoricoPrecioMateriaPrimaProveedor)(jsonBody)

            Dim insertado As Boolean = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = insertado,
                .message = If(insertado, "Registro insertado correctamente.", "No se pudo insertar el registro.")
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

            If jObj("HistMpProvId") IsNot Nothing Then
                id = Convert.ToInt32(jObj("HistMpProvId").ToString())
            ElseIf jObj("id") IsNot Nothing Then
                id = Convert.ToInt32(jObj("id").ToString())
            End If

            If id <= 0 Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar HistMpProvId o id."
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