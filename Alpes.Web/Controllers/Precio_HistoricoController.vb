Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Inventario

Public Class Precio_HistoricoController
    Inherits Controller

    Private ReadOnly _servicio As Precio_HistoricoServicio

    Public Sub New()
        _servicio = New Precio_HistoricoServicio()
    End Sub

    <HttpGet>
    Public Function Index() As ActionResult
        Try
            Dim lista As List(Of Precio_Historico) = _servicio.Listar()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As Precio_Historico = _servicio.ObtenerPorId(id)
            Return Json(entidad, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Buscar(ByVal criterio As String, ByVal valor As String) As ActionResult
        Try
            Dim lista As List(Of Precio_Historico) = _servicio.Buscar(criterio, valor)
            Return Json(lista, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Public Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = LeerBody()
            Dim entidad As Precio_Historico = JsonConvert.DeserializeObject(Of Precio_Historico)(jsonBody)

            If entidad Is Nothing Then
                Throw New Exception("No se recibieron datos válidos para insertar.")
            End If

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Registro insertado correctamente.",
                .id = idGenerado
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            })
        End Try
    End Function

    <HttpPost>
    Public Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = LeerBody()
            Dim entidad As Precio_Historico = JsonConvert.DeserializeObject(Of Precio_Historico)(jsonBody)

            If entidad Is Nothing Then
                Throw New Exception("No se recibieron datos válidos para actualizar.")
            End If

            If entidad.PrecioHistId <= 0 Then
                Throw New Exception("Debe enviar un PrecioHistId válido para actualizar.")
            End If

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Registro actualizado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            })
        End Try
    End Function

    <HttpPost>
    Public Function Eliminar() As ActionResult
        Try
            Dim precioHistId As Integer = ObtenerPrecioHistIdDesdeRequest()

            If precioHistId <= 0 Then
                Throw New Exception("Debe enviar un PrecioHistId válido para eliminar.")
            End If

            _servicio.Eliminar(precioHistId)

            Return Json(New With {
                .success = True,
                .message = "Registro eliminado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            })
        End Try
    End Function

    Private Function LeerBody() As String
        If Request Is Nothing OrElse Request.InputStream Is Nothing Then
            Return String.Empty
        End If

        If Request.InputStream.CanSeek Then
            Request.InputStream.Position = 0
        End If

        Using reader As New StreamReader(Request.InputStream)
            Return reader.ReadToEnd()
        End Using
    End Function

    Private Function ObtenerPrecioHistIdDesdeRequest() As Integer
        Dim id As Integer = 0

        If Integer.TryParse(Convert.ToString(Request("PrecioHistId")), id) AndAlso id > 0 Then
            Return id
        End If

        If Integer.TryParse(Convert.ToString(Request("precio_hist_id")), id) AndAlso id > 0 Then
            Return id
        End If

        If Integer.TryParse(Convert.ToString(Request("id")), id) AndAlso id > 0 Then
            Return id
        End If

        Dim jsonBody As String = LeerBody()

        If String.IsNullOrWhiteSpace(jsonBody) Then
            Return 0
        End If

        Try
            Dim obj As JObject = JObject.Parse(jsonBody)
            Dim token As JToken = obj("PrecioHistId")

            If token Is Nothing Then
                token = obj("precio_hist_id")
            End If

            If token Is Nothing Then
                token = obj("id")
            End If

            If token IsNot Nothing AndAlso Integer.TryParse(token.ToString(), id) AndAlso id > 0 Then
                Return id
            End If
        Catch
        End Try

        Try
            id = JsonConvert.DeserializeObject(Of Integer)(jsonBody)
            If id > 0 Then
                Return id
            End If
        Catch
        End Try

        Return 0
    End Function

    Private Function LimpiarMensajeError(ByVal mensaje As String) As String
        If String.IsNullOrWhiteSpace(mensaje) Then
            Return "Ocurrió un error inesperado."
        End If

        Dim lineas() As String = mensaje.Replace(vbCrLf, vbLf).Split(ControlChars.Lf)

        If lineas.Length > 0 AndAlso Not String.IsNullOrWhiteSpace(lineas(0)) Then
            Return lineas(0).Trim()
        End If

        Return mensaje.Trim()
    End Function

End Class