Imports System.Web.Mvc
Imports Newtonsoft.Json.Linq
Imports alpes.Servicios

<RoutePrefix("api/entidades")>
Public Class ApiEntidadesController
    Inherits Controller

    Private ReadOnly _servicio As EntidadCrudServicio

    Public Sub New()
        _servicio = New EntidadCrudServicio()
    End Sub

    <HttpGet>
    <Route("")>
    Public Function Entidades() As JsonResult
        Return Json(_servicio.EntidadesSoportadas(), JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    <Route("{entidad}/listar")>
    Public Function Listar(entidad As String) As JsonResult
        Return Json(_servicio.Listar(entidad), JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    <Route("{entidad}/obtener/{id:int}")>
    Public Function Obtener(entidad As String, id As Integer) As JsonResult
        Return Json(_servicio.ObtenerPorId(entidad, id), JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    <Route("{entidad}/buscar")>
    Public Function Buscar(entidad As String, criterio As String, valor As String) As JsonResult
        Return Json(_servicio.Buscar(entidad, criterio, valor), JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    <Route("{entidad}/crear")>
    Public Function Crear(entidad As String, payload As JObject) As JsonResult
        Dim nuevoId = _servicio.Insertar(entidad, payload.ToString())
        Return Json(New With {.id = nuevoId})
    End Function

    <HttpPut>
    <Route("{entidad}/actualizar")>
    Public Function Actualizar(entidad As String, payload As JObject) As JsonResult
        _servicio.Actualizar(entidad, payload.ToString())
        Return Json(New With {.ok = True})
    End Function

    <HttpDelete>
    <Route("{entidad}/eliminar/{id:int}")>
    Public Function Eliminar(entidad As String, id As Integer) As JsonResult
        _servicio.Eliminar(entidad, id)
        Return Json(New With {.ok = True}, JsonRequestBehavior.AllowGet)
    End Function

    Protected Overrides Sub OnException(filterContext As ExceptionContext)
        filterContext.ExceptionHandled = True
        filterContext.Result = Json(
            New With {
                .ok = False,
                .error = filterContext.Exception.Message
            },
            JsonRequestBehavior.AllowGet)
    End Sub

End Class
