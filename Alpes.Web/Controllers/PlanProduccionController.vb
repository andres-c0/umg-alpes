Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Produccion

Public Class PlanProduccionController
    Inherits Controller

    Private ReadOnly _servicio As PlanProduccionServicio

    Public Sub New()
        _servicio = New PlanProduccionServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of PlanProduccion) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As PlanProduccion = _servicio.ObtenerPorId(id)

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
            Dim lista As List(Of PlanProduccion) = _servicio.Buscar(criterio, valor)

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
            Dim entidad As PlanProduccion = JsonConvert.DeserializeObject(Of PlanProduccion)(jsonBody)

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
            Dim entidad As PlanProduccion = JsonConvert.DeserializeObject(Of PlanProduccion)(jsonBody)

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
            Dim payload As Dictionary(Of String, Integer) = JsonConvert.DeserializeObject(Of Dictionary(Of String, Integer))(jsonBody)

            If payload Is Nothing OrElse Not payload.ContainsKey("PlanProduccionId") Then
                Return Json(New With {
                    .success = False,
                    .message = "Debe enviar PlanProduccionId."
                })
            End If

            Dim eliminado As Boolean = _servicio.Eliminar(payload("PlanProduccionId"))

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