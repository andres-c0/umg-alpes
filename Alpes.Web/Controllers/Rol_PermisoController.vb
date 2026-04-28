Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Seguridad

Public Class Rol_PermisoController
    Inherits Controller

    Private ReadOnly _servicio As Rol_PermisoServicio

    Public Sub New()
        _servicio = New Rol_PermisoServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of Rol_Permiso) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As Rol_Permiso = _servicio.ObtenerPorId(id)

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

    Function Buscar(ByVal valor As Integer) As ActionResult
        Try
            Dim lista As List(Of Rol_Permiso) = _servicio.Buscar(valor)

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
            Dim entidad As Rol_Permiso = JsonConvert.DeserializeObject(Of Rol_Permiso)(jsonBody)

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
            Dim entidad As Rol_Permiso = JsonConvert.DeserializeObject(Of Rol_Permiso)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Registro actualizado correctamente."
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
            .message = "Rol_Permiso eliminado correctamente."
        })
        Catch ex As Exception
            Return Json(New With {
            .success = False,
            .message = ex.Message
        })
        End Try
    End Function

End Class