Option Strict On
Option Explicit On

Imports System.Linq
Imports System.Web.Mvc
Imports Alpes.Servicios.Servicios
Imports Alpes.Web.Models
Imports Alpes.Entidades.Seguridad
Imports System.Text.RegularExpressions

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        Private ReadOnly _usuarioServicio As UsuarioServicio

        Public Sub New()
            _usuarioServicio = New UsuarioServicio()
        End Sub

        Function Index() As ActionResult
            Return View()
        End Function

        <HttpGet>
        Function Login() As ActionResult
            If Session("UsuarioId") IsNot Nothing Then
                Dim rolId As Integer = 0

                If Session("RolId") IsNot Nothing Then
                    Integer.TryParse(Session("RolId").ToString(), rolId)
                End If

                If rolId <> 3 Then
                    Return RedirectToAction("Index", "Admin")
                Else
                    Return RedirectToAction("Index", "Cliente")
                End If
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
                Session("Username") = usuario.Username
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

                If usuario.RolId <> 3 Then
                    Return RedirectToAction("Index", "Admin")
                Else
                    Return RedirectToAction("Index", "Cliente")
                End If

            Catch ex As Exception
                ViewData("Error") = "Ocurrió un error al iniciar sesión: " & ex.Message
                Return View(model)
            End Try
        End Function

        <HttpGet>
        Function Registro() As ActionResult
            If Session("UsuarioId") IsNot Nothing Then
                Dim rolId As Integer = 0

                If Session("RolId") IsNot Nothing Then
                    Integer.TryParse(Session("RolId").ToString(), rolId)
                End If

                If rolId <> 3 Then
                    Return RedirectToAction("Index", "Admin")
                Else
                    Return RedirectToAction("Index", "Cliente")
                End If
            End If

            Return View(New RegistroViewModel())
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Registro(ByVal model As RegistroViewModel) As ActionResult
            If model Is Nothing Then
                ViewData("Error") = "Solicitud inválida."
                Return View(New RegistroViewModel())
            End If

            If String.IsNullOrWhiteSpace(model.Username) Then
                ModelState.AddModelError("Username", "El nombre de usuario es requerido.")
            ElseIf model.Username.Trim().Length < 4 Then
                ModelState.AddModelError("Username", "El nombre de usuario debe tener al menos 4 caracteres.")
            End If

            If String.IsNullOrWhiteSpace(model.Email) Then
                ModelState.AddModelError("Email", "El correo electrónico es requerido.")
            ElseIf Not Regex.IsMatch(model.Email.Trim(), "^[^@\s]+@[^@\s]+\.[^@\s]+$") Then
                ModelState.AddModelError("Email", "Ingresa un correo electrónico válido.")
            End If

            If String.IsNullOrWhiteSpace(model.Password) Then
                ModelState.AddModelError("Password", "La contraseña es requerida.")
            ElseIf model.Password.Length < 6 Then
                ModelState.AddModelError("Password", "La contraseña debe tener al menos 6 caracteres.")
            End If

            If String.IsNullOrWhiteSpace(model.ConfirmPassword) Then
                ModelState.AddModelError("ConfirmPassword", "Debes confirmar la contraseña.")
            ElseIf Not String.Equals(model.Password, model.ConfirmPassword, StringComparison.Ordinal) Then
                ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.")
            End If

            If Not String.IsNullOrWhiteSpace(model.Telefono) AndAlso model.Telefono.Trim().Length > 20 Then
                ModelState.AddModelError("Telefono", "El teléfono no puede exceder 20 caracteres.")
            End If

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            Try
                Dim coincidenciasUsuario As List(Of Usuario) = _usuarioServicio.Buscar(model.Username.Trim())
                Dim existeUsuario As Boolean = coincidenciasUsuario.Any(
            Function(u) u.Username IsNot Nothing AndAlso
                        String.Equals(u.Username.Trim(), model.Username.Trim(), StringComparison.OrdinalIgnoreCase)
        )

                If existeUsuario Then
                    ModelState.AddModelError("Username", "Ese nombre de usuario ya existe.")
                    Return View(model)
                End If

                Dim coincidenciasEmail As List(Of Usuario) = _usuarioServicio.Buscar(model.Email.Trim())
                Dim existeEmail As Boolean = coincidenciasEmail.Any(
            Function(u) u.Email IsNot Nothing AndAlso
                        String.Equals(u.Email.Trim(), model.Email.Trim(), StringComparison.OrdinalIgnoreCase)
        )

                If existeEmail Then
                    ModelState.AddModelError("Email", "Ese correo ya está registrado.")
                    Return View(model)
                End If

                Dim nuevoUsuario As New Usuario With {
            .Username = model.Username.Trim(),
            .PasswordHash = model.Password,
            .Email = model.Email.Trim(),
            .Telefono = If(String.IsNullOrWhiteSpace(model.Telefono), Nothing, model.Telefono.Trim()),
            .RolId = 3,
            .CliId = Nothing,
            .EmpId = Nothing,
            .UltimoLoginAt = Nothing,
            .BloqueadoHasta = Nothing,
            .Estado = "ACTIVO"
        }

                _usuarioServicio.Insertar(nuevoUsuario)

                TempData("Success") = "Tu cuenta fue creada correctamente. Ahora puedes iniciar sesión."
                Return RedirectToAction("Login")
            Catch ex As Exception
                ViewData("Error") = "Ocurrió un error al crear la cuenta: " & ex.Message
                Return View(model)
            End Try
        End Function

        Function Logout() As ActionResult
            Session.Clear()
            Session.Abandon()
            Return RedirectToAction("Login")
        End Function
    End Class
End Namespace