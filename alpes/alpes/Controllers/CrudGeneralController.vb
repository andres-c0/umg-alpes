Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class CrudGeneralController
    Inherits Controller

    Function Index() As ActionResult
        Return View()
    End Function
End Class
