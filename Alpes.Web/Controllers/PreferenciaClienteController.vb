Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Ventas

Public Class PreferenciaClienteController
    Inherits Controller

    Private ReadOnly _servicio As PreferenciaClienteServicio

    Public Sub New()
        _servicio = New PreferenciaClienteServicio()
    End Sub

    <HttpGet>
    Public Function Index() As ActionResult
        Try
            Dim lista As List(Of PreferenciaCliente) = _servicio.Listar()

            Return Json(New With {
                .ok = True,
                .success = True,
                .data = lista,
                .message = "Consulta realizada correctamente."
            }, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .ok = False,
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Obtener(ByVal id As Integer) As ActionResult
        Try
            Dim entidad As PreferenciaCliente = _servicio.ObtenerPorId(id)

            If entidad Is Nothing Then
                Return Json(New With {
                    .ok = False,
                    .success = False,
                    .message = "Registro no encontrado."
                }, JsonRequestBehavior.AllowGet)
            End If

            Return Json(New With {
                .ok = True,
                .success = True,
                .data = entidad,
                .message = "Consulta realizada correctamente."
            }, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .ok = False,
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpGet>
    Public Function Buscar(ByVal criterio As String, ByVal valor As String) As ActionResult
        Try
            If String.IsNullOrWhiteSpace(criterio) Then
                Return Json(New With {
                    .ok = False,
                    .success = False,
                    .message = "Debe enviar el criterio de búsqueda."
                }, JsonRequestBehavior.AllowGet)
            End If

            If String.IsNullOrWhiteSpace(valor) Then
                Return Json(New With {
                    .ok = False,
                    .success = False,
                    .message = "Debe enviar el valor de búsqueda."
                }, JsonRequestBehavior.AllowGet)
            End If

            Dim lista As List(Of PreferenciaCliente) = _servicio.Buscar(criterio.Trim(), valor.Trim())

            Return Json(New With {
                .ok = True,
                .success = True,
                .data = lista,
                .message = "Consulta realizada correctamente."
            }, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {
                .ok = False,
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            }, JsonRequestBehavior.AllowGet)
        End Try
    End Function

    <HttpPost>
    Public Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = LeerBody()
            Dim entidad As PreferenciaCliente = JsonConvert.DeserializeObject(Of PreferenciaCliente)(jsonBody)

            If entidad Is Nothing Then
                Throw New Exception("No se recibieron datos válidos para insertar.")
            End If

            ValidarEntidad(entidad, False)

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .ok = True,
                .success = True,
                .message = "Registro insertado correctamente.",
                .id = idGenerado
            })
        Catch ex As Exception
            Return Json(New With {
                .ok = False,
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            })
        End Try
    End Function

    <HttpPost>
    Public Function Actualizar() As ActionResult
        Try
            Dim jsonBody As String = LeerBody()
            Dim entidad As PreferenciaCliente = JsonConvert.DeserializeObject(Of PreferenciaCliente)(jsonBody)

            If entidad Is Nothing Then
                Throw New Exception("No se recibieron datos válidos para actualizar.")
            End If

            ValidarEntidad(entidad, True)

            Dim actualizado As Boolean = _servicio.Actualizar(entidad)

            Return Json(New With {
                .ok = actualizado,
                .success = actualizado,
                .message = If(actualizado, "Registro actualizado correctamente.", "No se pudo actualizar el registro.")
            })
        Catch ex As Exception
            Return Json(New With {
                .ok = False,
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            })
        End Try
    End Function

    <HttpPost>
    Public Function Eliminar() As ActionResult
        Try
            Dim id As Integer = ObtenerPreferenciaIdDesdeRequest()

            If id <= 0 Then
                Throw New Exception("Debe enviar un PreferenciaId válido para eliminar.")
            End If

            Dim eliminado As Boolean = _servicio.Eliminar(id)

            Return Json(New With {
                .ok = eliminado,
                .success = eliminado,
                .message = If(eliminado, "Registro eliminado correctamente.", "No se pudo eliminar el registro.")
            })
        Catch ex As Exception
            Return Json(New With {
                .ok = False,
                .success = False,
                .message = LimpiarMensajeError(ex.Message)
            })
        End Try
    End Function

    Private Sub ValidarEntidad(ByVal entidad As PreferenciaCliente, ByVal requiereId As Boolean)
        If requiereId AndAlso entidad.PreferenciaId <= 0 Then
            Throw New Exception("Debe enviar un PreferenciaId válido.")
        End If

        If entidad.CliId <= 0 Then
            Throw New Exception("Debe enviar un CliId válido.")
        End If

        If entidad.CategoriaId <= 0 Then
            Throw New Exception("Debe enviar un CategoriaId válido.")
        End If

        If entidad.PesoPreferencia < 0D Then
            Throw New Exception("PesoPreferencia no puede ser negativo.")
        End If
    End Sub

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

    Private Function ObtenerPreferenciaIdDesdeRequest() As Integer
        Dim id As Integer = 0

        If Integer.TryParse(Convert.ToString(Request("PreferenciaId")), id) AndAlso id > 0 Then
            Return id
        End If

        If Integer.TryParse(Convert.ToString(Request("preferenciaId")), id) AndAlso id > 0 Then
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
            Dim token As JToken = obj("PreferenciaId")

            If token Is Nothing Then
                token = obj("preferenciaId")
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