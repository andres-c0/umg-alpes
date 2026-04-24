Option Strict On
Option Explicit On

Imports System
Imports System.Linq
Imports System.Web.Mvc
Imports Alpes.Servicios.Servicios
Imports Alpes.Web.Models
Imports Alpes.Entidades.Seguridad

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        Private ReadOnly _usuarioServicio As UsuarioServicio

        Public Sub New()
            _usuarioServicio = New UsuarioServicio()
        End Sub

        Function Index() As ActionResult
            Return RedirectToAction("Login", "Home")
        End Function

        <HttpGet>
        Function Login() As ActionResult
            If TempData("Error") IsNot Nothing Then
                ViewData("Error") = TempData("Error").ToString()
            End If

            If Session("UsuarioId") IsNot Nothing Then
                Return RedirigirSegunSesionActual()
            End If

            Return View(New LoginViewModel())
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Login(ByVal model As LoginViewModel) As ActionResult
            If model Is Nothing Then
                ViewData("Error") = "Solicitud inválida."
                Return View(New LoginViewModel())
            End If

            If String.IsNullOrWhiteSpace(model.Username) Then
                ModelState.AddModelError("Username", "El usuario es requerido.")
            End If

            If String.IsNullOrWhiteSpace(model.Password) Then
                ModelState.AddModelError("Password", "La contraseña es requerida.")
            End If

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Try
                Dim usuarios As List(Of Usuario) = _usuarioServicio.Buscar(model.Username.Trim())

                Dim usuario As Usuario = usuarios.FirstOrDefault(
                    Function(u) u.Username IsNot Nothing AndAlso
                                String.Equals(u.Username.Trim(), model.Username.Trim(), StringComparison.OrdinalIgnoreCase)
                )

                If usuario Is Nothing Then
                    ViewData("Error") = "Usuario o contraseña incorrectos."
                    Return View(model)
                End If

                If String.IsNullOrWhiteSpace(usuario.PasswordHash) OrElse
                   Not String.Equals(usuario.PasswordHash, model.Password, StringComparison.Ordinal) Then

                    ViewData("Error") = "Usuario o contraseña incorrectos."
                    Return View(model)
                End If

                Session("UsuarioId") = usuario.UsuId
                Session("Username") = If(usuario.Username, String.Empty)
                Session("RolId") = usuario.RolId

                If usuario.CliId.HasValue Then
                    Session("CliId") = usuario.CliId.Value
                Else
                    Session.Remove("CliId")
                End If

                If usuario.EmpId.HasValue Then
                    Session("EmpId") = usuario.EmpId.Value
                Else
                    Session.Remove("EmpId")
                End If

                If usuario.RolId = 3 AndAlso Not usuario.CliId.HasValue Then
                    Session.Clear()
                    Session.Abandon()
                    ViewData("Error") = "El usuario cliente no tiene un cliente asociado (CLI_ID)."
                    Return View(model)
                End If

                Return RedirigirSegunRolUsuario(usuario)

            Catch ex As Exception
                ViewData("Error") = "Ocurrió un error al iniciar sesión: " & ex.Message
                Return View(model)
            End Try
        End Function

        Function Registro() As ActionResult
            Return View()
        End Function

        Function Logout() As ActionResult
            Session.Clear()
            Session.Abandon()
            Return RedirectToAction("Login")
        End Function

        Private Function RedirigirSegunSesionActual() As ActionResult
            Dim rolId As Integer = ObtenerRolIdDesdeSesion()
            Dim cliId As Integer? = ObtenerCliIdDesdeSesion()

            If rolId = 3 Then
                If cliId.HasValue AndAlso cliId.Value > 0 Then
                    Return RedirectToAction("Index", "PortalCliente")
                End If

                Session.Clear()
                Session.Abandon()
                TempData("Error") = "La sesión del cliente no es válida porque no tiene CLI_ID asociado."
                Return RedirectToAction("Login", "Home")
            End If

            Return RedirectToAction("Productos", "Admin")
        End Function

        Private Function RedirigirSegunRolUsuario(ByVal usuario As Usuario) As ActionResult
            If usuario Is Nothing Then
                Return RedirectToAction("Login", "Home")
            End If

            If usuario.RolId = 3 Then
                Return RedirectToAction("Index", "PortalCliente")
            End If

            Return RedirectToAction("Productos", "Admin")
        End Function

        Private Function ObtenerRolIdDesdeSesion() As Integer
            Dim rolId As Integer = 0

            If Session("RolId") IsNot Nothing Then
                Integer.TryParse(Session("RolId").ToString(), rolId)
            End If

            Return rolId
        End Function

        Private Function ObtenerCliIdDesdeSesion() As Integer?
            If Session("CliId") Is Nothing Then
                Return Nothing
            End If

            Dim cliId As Integer = 0
            If Integer.TryParse(Session("CliId").ToString(), cliId) AndAlso cliId > 0 Then
                Return cliId
            End If

            Return Nothing
        End Function
    End Class
End Namespace