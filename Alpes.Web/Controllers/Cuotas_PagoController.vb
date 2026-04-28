Option Strict On
Option Explicit On

Imports System.IO
Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Alpes.Servicios.Servicios
Imports Alpes.Entidades.Ventas

Public Class Cuotas_PagoController
    Inherits Controller

    Private ReadOnly _servicio As Cuotas_PagoServicio

    Public Sub New()
        _servicio = New Cuotas_PagoServicio()
    End Sub

    Function Index() As ActionResult
        Dim lista As List(Of Cuotas_Pago) = _servicio.Listar()
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    Function Obtener(ByVal id As Integer) As ActionResult
        Dim entidad As Cuotas_Pago = _servicio.ObtenerPorId(id)
        Return Json(entidad, JsonRequestBehavior.AllowGet)
    End Function

    Function Buscar(ByVal valor As Integer) As ActionResult
        Dim lista As List(Of Cuotas_Pago) = _servicio.Buscar(valor)
        Return Json(lista, JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    Function Insertar() As ActionResult
        Try
            Dim jsonBody As String = New StreamReader(Request.InputStream).ReadToEnd()
            Dim entidad As Cuotas_Pago = JsonConvert.DeserializeObject(Of Cuotas_Pago)(jsonBody)

            Dim idGenerado As Integer = _servicio.Insertar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Cuotas_Pago insertado correctamente.",
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
            Dim entidad As Cuotas_Pago = JsonConvert.DeserializeObject(Of Cuotas_Pago)(jsonBody)

            _servicio.Actualizar(entidad)

            Return Json(New With {
                .success = True,
                .message = "Cuotas_Pago actualizado correctamente."
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
                .message = "Cuotas_Pago eliminado correctamente."
            })
        Catch ex As Exception
            Return Json(New With {
                .success = False,
                .message = ex.Message
            })
        End Try
    End Function

End Class