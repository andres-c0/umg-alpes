Imports alpes.Servicios

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        Return View()
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your application description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function

    Function Cruds() As ActionResult
        Dim servicio As New EntidadCrudServicio()
        ViewBag.Entidades = servicio.EntidadesSoportadas()
        Return View()
    End Function
End Class
