Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Inventario

Public Class Unidad_MedidaController
    Inherits Controller

    Private ReadOnly _servicio As Unidad_MedidaServicio

    Public Sub New()
        _servicio = New Unidad_MedidaServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of Unidad_Medida) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As Unidad_Medida = _servicio.ObtenerPorId(id)

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
            Dim lista As List(Of Unidad_Medida) = _servicio.Buscar(criterio, valor)

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
            Dim entidad As Unidad_Medida = JsonConvert.DeserializeObject(Of Unidad_Medida)(jsonBody)

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Unidad_Medida insertada correctamente.",
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
            Dim entidad As Unidad_Medida = JsonConvert.DeserializeObject(Of Unidad_Medida)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Unidad_Medida actualizada correctamente."
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
                .message = "Unidad_Medida eliminada correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

End Class