Imports System.Web.Mvc
Imports alpes.Servicios
Imports alpes.Entidades

Public Class ClienteController
    Inherits Controller

    Private ReadOnly _servicio As ClienteServicio

    Public Sub New()
        _servicio = New ClienteServicio()
    End Sub

    Function Index() As ActionResult
        Dim dt = _servicio.Listar()
        Return View(dt)
    End Function

    Function Create() As ActionResult
        Return View()
    End Function

    <HttpPost>
    Function Create(cliente As Cliente) As ActionResult
        If ModelState.IsValid Then
            _servicio.Insertar(cliente)
            Return RedirectToAction("Index")
        End If

        Return View(cliente)
    End Function
End Class