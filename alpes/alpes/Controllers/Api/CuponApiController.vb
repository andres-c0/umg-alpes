Option Strict On
Option Explicit On

Imports System.Data
Imports System.Web.Mvc

<RoutePrefix("api/cupones")>
Public Class CuponApiController
    Inherits CrudApiControllerBase(Of Cupon)

    Private ReadOnly _servicio As CuponServicio

    Public Sub New()
        _servicio = New CuponServicio()
    End Sub

    <HttpGet>
    <Route("")>
    Public Function GetAll() As ActionResult
        Return ListarResultado()
    End Function

    <HttpGet>
    <Route("{id:int}")>
    Public Function GetById(ByVal id As Integer) As ActionResult
        Return ObtenerResultado(id)
    End Function

    <HttpGet>
    <Route("buscar")>
    Public Function Search(ByVal criterio As String, ByVal valor As String) As ActionResult
        Return BuscarResultado(criterio, valor)
    End Function

    <HttpPost>
    <Route("")>
    Public Function Create(ByVal entidad As Cupon) As ActionResult
        Return CrearResultado(entidad)
    End Function

    <HttpPut>
    <Route("{id:int}")>
    Public Function Update(ByVal id As Integer, ByVal entidad As Cupon) As ActionResult
        Return ActualizarResultado(id, entidad)
    End Function

    <HttpDelete>
    <Route("{id:int}")>
    Public Function Delete(ByVal id As Integer) As ActionResult
        Return EliminarResultado(id)
    End Function

    Protected Overrides Function ListarDatos() As DataTable
        Return _servicio.Listar()
    End Function

    Protected Overrides Function ObtenerDatos(ByVal id As Integer) As DataTable
        Return _servicio.ObtenerPorId(id)
    End Function

    Protected Overrides Function BuscarDatos(ByVal criterio As String, ByVal valor As String) As DataTable
        Return _servicio.Buscar(criterio, valor)
    End Function

    Protected Overrides Function InsertarEntidad(ByVal entidad As Cupon) As Integer
        Return _servicio.Insertar(entidad)
    End Function

    Protected Overrides Sub ActualizarEntidad(ByVal entidad As Cupon)
        _servicio.Actualizar(entidad)
    End Sub

    Protected Overrides Sub EliminarEntidad(ByVal id As Integer)
        _servicio.Eliminar(id)
    End Sub

    Protected Overrides Sub AsignarId(ByVal entidad As Cupon, ByVal id As Integer)
        entidad.CuponId = id
    End Sub

End Class
