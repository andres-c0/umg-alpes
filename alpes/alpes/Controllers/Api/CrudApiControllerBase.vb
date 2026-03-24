Option Strict On
Option Explicit On

Imports System.Data
Imports System.Linq
Imports System.Net
Imports System.Web.Mvc
Imports Newtonsoft.Json

Public MustInherit Class CrudApiControllerBase(Of TEntity)
    Inherits Controller

    Protected MustOverride Function ListarDatos() As DataTable
    Protected MustOverride Function ObtenerDatos(ByVal id As Integer) As DataTable
    Protected MustOverride Function BuscarDatos(ByVal criterio As String, ByVal valor As String) As DataTable
    Protected MustOverride Function InsertarEntidad(ByVal entidad As TEntity) As Integer
    Protected MustOverride Sub ActualizarEntidad(ByVal entidad As TEntity)
    Protected MustOverride Sub EliminarEntidad(ByVal id As Integer)
    Protected MustOverride Sub AsignarId(ByVal entidad As TEntity, ByVal id As Integer)

    Protected Function ListarResultado() As ActionResult
        Try
            Return JsonPayload(New With {
                .success = True,
                .data = ApiSerializationHelper.ToCollection(ListarDatos())
            })
        Catch ex As Exception
            Return JsonError(ex.Message, HttpStatusCode.InternalServerError)
        End Try
    End Function

    Protected Function ObtenerResultado(ByVal id As Integer) As ActionResult
        Try
            Dim item = ApiSerializationHelper.ToItem(ObtenerDatos(id))

            If item Is Nothing Then
                Return JsonError("Registro no encontrado.", HttpStatusCode.NotFound)
            End If

            Return JsonPayload(New With {
                .success = True,
                .data = item
            })
        Catch ex As Exception
            Return JsonError(ex.Message, HttpStatusCode.InternalServerError)
        End Try
    End Function

    Protected Function BuscarResultado(ByVal criterio As String, ByVal valor As String) As ActionResult
        Try
            Return JsonPayload(New With {
                .success = True,
                .data = ApiSerializationHelper.ToCollection(BuscarDatos(criterio, valor))
            })
        Catch ex As Exception
            Return JsonError(ex.Message, HttpStatusCode.InternalServerError)
        End Try
    End Function

    Protected Function CrearResultado(ByVal entidad As TEntity) As ActionResult
        If entidad Is Nothing Then
            Return JsonError("El cuerpo de la solicitud es obligatorio.", HttpStatusCode.BadRequest)
        End If

        If Not ModelState.IsValid Then
            Return JsonError("La entidad enviada no es válida.", HttpStatusCode.BadRequest, ObtenerErroresModelo())
        End If

        Try
            Dim id = InsertarEntidad(entidad)
            Response.StatusCode = CInt(HttpStatusCode.Created)

            Return JsonPayload(New With {
                .success = True,
                .id = id
            })
        Catch ex As Exception
            Return JsonError(ex.Message, HttpStatusCode.InternalServerError)
        End Try
    End Function

    Protected Function ActualizarResultado(ByVal id As Integer, ByVal entidad As TEntity) As ActionResult
        If entidad Is Nothing Then
            Return JsonError("El cuerpo de la solicitud es obligatorio.", HttpStatusCode.BadRequest)
        End If

        If Not ModelState.IsValid Then
            Return JsonError("La entidad enviada no es válida.", HttpStatusCode.BadRequest, ObtenerErroresModelo())
        End If

        Try
            AsignarId(entidad, id)
            ActualizarEntidad(entidad)

            Return JsonPayload(New With {
                .success = True,
                .message = "Registro actualizado correctamente."
            })
        Catch ex As Exception
            Return JsonError(ex.Message, HttpStatusCode.InternalServerError)
        End Try
    End Function

    Protected Function EliminarResultado(ByVal id As Integer) As ActionResult
        Try
            EliminarEntidad(id)

            Return JsonPayload(New With {
                .success = True,
                .message = "Registro eliminado correctamente."
            })
        Catch ex As Exception
            Return JsonError(ex.Message, HttpStatusCode.InternalServerError)
        End Try
    End Function

    Protected Function JsonPayload(ByVal payload As Object) As ContentResult
        Return Content(JsonConvert.SerializeObject(payload), "application/json")
    End Function

    Protected Function JsonError(ByVal message As String, ByVal statusCode As HttpStatusCode, Optional ByVal details As Object = Nothing) As ContentResult
        Response.StatusCode = CInt(statusCode)

        Return JsonPayload(New With {
            .success = False,
            .message = message,
            .details = details
        })
    End Function

    Private Function ObtenerErroresModelo() As Dictionary(Of String, List(Of String))
        Return ModelState.Where(Function(item) item.Value.Errors.Count > 0).
            ToDictionary(
                Function(item) item.Key,
                Function(item) item.Value.Errors.Select(Function(errorItem)
                                                            If(errorItem.Exception IsNot Nothing, errorItem.Exception.Message, errorItem.ErrorMessage)
                                                        End Function).Where(Function(message) Not String.IsNullOrWhiteSpace(message)).ToList())
    End Function

End Class
