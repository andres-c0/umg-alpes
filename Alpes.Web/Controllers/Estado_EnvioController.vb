Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Envios

Public Class Estado_EnvioController
    Inherits Controller

    Private ReadOnly _servicio As Estado_EnvioServicio

    Public Sub New()
        _servicio = New Estado_EnvioServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of Estado_Envio) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As Estado_Envio = _servicio.ObtenerPorId(id)

            If entidad Is Nothing Then
                Return Json(New With {
                    .success = False,
                    .message = "No se encontró el registro."
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

    Function Buscar(ByVal criterio As String, ByVal valor As String) As ActionResult
        Try
            Dim lista As List(Of Estado_Envio) = _servicio.Buscar(criterio, valor)

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
            Dim entidad As Estado_Envio = JsonConvert.DeserializeObject(Of Estado_Envio)(jsonBody)

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Estado_Envio insertado correctamente.",
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
            Dim entidad As Estado_Envio = JsonConvert.DeserializeObject(Of Estado_Envio)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Estado_Envio actualizado correctamente."
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
                .message = "Estado_Envio eliminado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

End Class