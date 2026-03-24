Imports System.Web.Mvc
Imports alpes.Servicios
Imports alpes.Entidades

Public Class ProveedorController
    Inherits Controller

    Private ReadOnly _servicio As ProveedorServicio

    Public Sub New()
        _servicio = New ProveedorServicio()
    End Sub

    Function Index() As ActionResult
        Dim modelo = _servicio.Listar()
        Return View(modelo)
    End Function

    Function Create() As ActionResult
        Return View()
    End Function

    <HttpPost>
    Function Create(proveedor As Proveedor) As ActionResult
        If ModelState.IsValid Then
            _servicio.Insertar(proveedor)
            Return RedirectToAction("Index")
        End If

        Return View(proveedor)
    End Function

    Function Edit(id As Integer) As ActionResult
        Dim modelo = _servicio.Buscar("PROV_ID", id.ToString())
        Return View(modelo)
    End Function

    <HttpPost>
    Function Edit(proveedor As Proveedor) As ActionResult
        If ModelState.IsValid Then
            _servicio.Actualizar(proveedor)
            Return RedirectToAction("Index")
        End If

        Return View(proveedor)
    End Function

    Function Delete(id As Integer) As ActionResult
        Dim modelo = _servicio.Buscar("PROV_ID", id.ToString())
        Return View(modelo)
    End Function

    <HttpPost, ActionName("Delete")>
    Function DeleteConfirmed(id As Integer) As ActionResult
        _servicio.Eliminar(id)
        Return RedirectToAction("Index")
    End Function

    <HttpGet>
    Function ApiListar() As JsonResult
        Return Json(_servicio.Listar(), JsonRequestBehavior.AllowGet)
    End Function

    <HttpGet>
    Function ApiBuscar(criterio As String, valor As String) As JsonResult
        Return Json(_servicio.Buscar(criterio, valor), JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost>
    Function ApiCrear(proveedor As Proveedor) As JsonResult
        Dim nuevoId = _servicio.Insertar(proveedor)
        Return Json(New With {.id = nuevoId})
    End Function

    <HttpPost>
    Function ApiActualizar(proveedor As Proveedor) As JsonResult
        _servicio.Actualizar(proveedor)
        Return Json(New With {.ok = True})
    End Function

    <HttpPost>
    Function ApiEliminar(id As Integer) As JsonResult
        _servicio.Eliminar(id)
        Return Json(New With {.ok = True})
    End Function
End Class
