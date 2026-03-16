Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class CuponController
    Inherits Controller

    Private ReadOnly _servicio As CuponServicio

    Public Sub New()
        _servicio = New CuponServicio()
    End Sub

    Function Index() As ActionResult
        Dim modelo = _servicio.Listar()
        Return View(modelo)
    End Function

    Function Details(ByVal id As Integer) As ActionResult
        Dim modelo = _servicio.ObtenerPorId(id)
        Return View(modelo)
    End Function

    Function Create() As ActionResult
        Return View()
    End Function

    <HttpPost()>
    Function Create(ByVal entidad As Cupon) As ActionResult
        If ModelState.IsValid Then
            _servicio.Insertar(entidad)
            Return RedirectToAction("Index")
        End If

        Return View(entidad)
    End Function

    Function Edit(ByVal id As Integer) As ActionResult
        Dim modelo = _servicio.ObtenerPorId(id)
        Return View(modelo)
    End Function

    <HttpPost()>
    Function Edit(ByVal entidad As Cupon) As ActionResult
        If ModelState.IsValid Then
            _servicio.Actualizar(entidad)
            Return RedirectToAction("Index")
        End If

        Return View(entidad)
    End Function

    Function Delete(ByVal id As Integer) As ActionResult
        Dim modelo = _servicio.ObtenerPorId(id)
        Return View(modelo)
    End Function

    <HttpPost(), ActionName("Delete")>
    Function DeleteConfirmed(ByVal id As Integer) As ActionResult
        _servicio.Eliminar(id)
        Return RedirectToAction("Index")
    End Function

End Class
