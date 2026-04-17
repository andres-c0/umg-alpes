Option Strict On
Option Explicit On

Imports System.Web.Mvc

Namespace Controllers
    Public Class AdminController
        Inherits Controller

        ' =========================
        ' DASHBOARD
        ' =========================
        Function Index() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' PRODUCTOS
        ' =========================
        Function Productos() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' ÓRDENES
        ' =========================
        Function Ordenes() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' CLIENTES
        ' =========================
        Function Clientes() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' INVENTARIO
        ' =========================
        Function Inventario() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' REPORTES
        ' =========================
        Function Reportes() As ActionResult
            If Not EsAdmin() Then
                Return RedirectToAction("Login", "Home")
            End If

            Return View()
        End Function

        ' =========================
        ' VALIDACIÓN DE ADMIN
        ' =========================
        Private Function EsAdmin() As Boolean
            If Session("UsuarioId") Is Nothing Then
                Return False
            End If

            If Session("RolId") Is Nothing Then
                Return False
            End If

            Dim rolId As Integer = 0
            Integer.TryParse(Session("RolId").ToString(), rolId)

            ' 👇 AJUSTA ESTE VALOR SEGÚN TU BD
            ' Si RolId = 1 es ADMIN, entonces:
            Return rolId = 1
        End Function

    End Class
End Namespace