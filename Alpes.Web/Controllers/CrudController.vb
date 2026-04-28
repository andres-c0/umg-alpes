Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class CrudController
    Inherits Controller

    Function Index() As ActionResult
        Return View()
    End Function

End Class