Imports Alpes.Servicios.Servicios

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

    Function ProbarConexion() As ContentResult
        Dim servicio As New PruebaConexionServicio()
        Dim resultado As String = servicio.Probar()
        Return Content(resultado)
    End Function

End Class