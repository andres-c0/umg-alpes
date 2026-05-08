Option Strict On
Option Explicit On

Imports System
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

            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Login(ByVal model As LoginViewModel) As ActionResult
            If model Is Nothing Then
                model = New LoginViewModel()
            End If

            Dim usernameForm As String = Request.Form("Username")
            If String.IsNullOrWhiteSpace(usernameForm) Then
                usernameForm = Request.Form("username")
            End If

            Dim passwordForm As String = Request.Form("Password")
            If String.IsNullOrWhiteSpace(passwordForm) Then
                passwordForm = Request.Form("password")
            End If

            If String.IsNullOrWhiteSpace(model.Username) Then
                model.Username = usernameForm
            End If

            If String.IsNullOrWhiteSpace(model.Password) Then
                model.Password = passwordForm
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
                Dim usernameIngresado As String = model.Username.Trim()
                Dim passwordIngresado As String = model.Password.Trim()

                Dim usuarios As List(Of Usuario) = _usuarioServicio.Buscar(usernameIngresado)

                Dim usuario As Usuario = usuarios.FirstOrDefault(
                    Function(u) u.Username IsNot Nothing AndAlso
                                String.Equals(u.Username.Trim(), usernameIngresado, StringComparison.OrdinalIgnoreCase)
                )

                If usuario Is Nothing Then
                    ViewData("Error") = "Usuario o contraseña incorrectos."
                    Return View(model)
                End If

                Dim passwordBd As String = If(usuario.PasswordHash, String.Empty).Trim()

                If Not String.Equals(passwordBd, passwordIngresado, StringComparison.Ordinal) Then
                    ViewData("Error") = "Usuario o contraseña incorrectos."
                    Return View(model)
                End If

                If usuario.Estado IsNot Nothing AndAlso
                   Not String.Equals(usuario.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase) Then
                    ViewData("Error") = "El usuario no está activo."
                    Return View(model)
                End If

                GuardarSesionUsuario(usuario)

                Return RedirigirSegunRolUsuario(usuario)

            Catch ex As Exception
                ViewData("Error") = "Ocurrió un error al iniciar sesión: " & ex.Message
                Return View(model)
            End Try
        End Function

        <HttpGet>
        Function Registro() As ActionResult
            If Session("UsuarioId") IsNot Nothing Then
                Return RedirigirSegunSesionActual()
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

                Dim rolClienteId As Integer = ObtenerRolIdPorNombre("CLIENTE")
                If rolClienteId <= 0 Then
                    ViewData("Error") = "No existe un rol activo llamado CLIENTE. Crea el rol CLIENTE antes de registrar usuarios del portal."
                    Return View(model)
                End If

                Dim nuevoUsuario As New Usuario With {
                    .Username = model.Username.Trim(),
                    .PasswordHash = model.Password.Trim(),
                    .Email = model.Email.Trim(),
                    .Telefono = If(String.IsNullOrWhiteSpace(model.Telefono), Nothing, model.Telefono.Trim()),
                    .RolId = rolClienteId,
                    .CliId = Nothing,
                    .EmpId = Nothing,
                    .UltimoLoginAt = Nothing,
                    .BloqueadoHasta = Nothing,
                    .Estado = "ACTIVO"
                }

                _usuarioServicio.Insertar(nuevoUsuario)

                TempData("Success") = "Tu cuenta fue creada correctamente. Para entrar al portal debe tener un CLI_ID asociado."
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

        Private Sub GuardarSesionUsuario(ByVal usuario As Usuario)
            Session("UsuarioId") = usuario.UsuId
            Session("Username") = If(usuario.Username, String.Empty)
            Session("RolId") = usuario.RolId
            Session("RolNombre") = ObtenerNombreRolPorId(usuario.RolId)

            If usuario.CliId.HasValue AndAlso usuario.CliId.Value > 0 Then
                Session("CliId") = usuario.CliId.Value
            Else
                Session.Remove("CliId")
            End If

            If usuario.EmpId.HasValue AndAlso usuario.EmpId.Value > 0 Then
                Session("EmpId") = usuario.EmpId.Value
            Else
                Session.Remove("EmpId")
            End If
        End Sub

        Private Function RedirigirSegunSesionActual() As ActionResult
            Dim rolId As Integer = ObtenerRolIdDesdeSesion()

            If EsRolCliente(rolId) Then
                Return RedirectToAction("Index", "PortalCliente")
            End If

            If EsRolAdministrativo(rolId) Then
                Return RedirectToAction("Index", "Admin")
            End If

            Session.Clear()
            Session.Abandon()
            TempData("Error") = "La sesión no es válida. El usuario no tiene un rol activo asignado."
            Return RedirectToAction("Login", "Home")
        End Function

        Private Function RedirigirSegunRolUsuario(ByVal usuario As Usuario) As ActionResult
            If usuario Is Nothing Then
                Return RedirectToAction("Login", "Home")
            End If

            If EsRolCliente(usuario.RolId) Then
                Return RedirectToAction("Index", "PortalCliente")
            End If

            If EsRolAdministrativo(usuario.RolId) Then
                Return RedirectToAction("Index", "Admin")
            End If

            ViewData("Error") = "El usuario existe, pero no tiene un rol activo asignado."
            Session.Clear()
            Session.Abandon()
            Return View("Login", New LoginViewModel())
        End Function

        Private Function EsRolCliente(ByVal rolId As Integer) As Boolean
            Dim nombreRol As String = ObtenerNombreRolPorId(rolId)
            Return EsNombreRolCliente(nombreRol)
        End Function

        Private Function EsRolAdministrativo(ByVal rolId As Integer) As Boolean
            Dim nombreRol As String = ObtenerNombreRolPorId(rolId)

            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return False
            End If

            ' REGLA:
            ' Si el rol es CLIENTE, entra al portal de cliente.
            ' Si el rol NO es CLIENTE, entra al panel de administración.
            Return Not EsNombreRolCliente(nombreRol)
        End Function

        Private Function EsNombreRolCliente(ByVal nombreRol As String) As Boolean
            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return False
            End If

            Dim rolNormalizado As String = nombreRol.Trim().ToUpperInvariant()
            Return rolNormalizado = "CLIENTE" OrElse rolNormalizado.Contains("CLIENTE")
        End Function

        Private Function ObtenerNombreRolPorId(ByVal rolId As Integer) As String
            If rolId <= 0 Then
                Return String.Empty
            End If

            Try
                Dim rolSrv As New RolServicio()
                Dim rol = rolSrv.ObtenerPorId(rolId)

                If rol IsNot Nothing AndAlso
                   rol.RolNombre IsNot Nothing AndAlso
                   (rol.Estado Is Nothing OrElse String.Equals(rol.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase)) Then
                    Return rol.RolNombre.Trim()
                End If
            Catch
                Return String.Empty
            End Try

            Return String.Empty
        End Function

        Private Function ObtenerRolIdPorNombre(ByVal nombreRol As String) As Integer
            If String.IsNullOrWhiteSpace(nombreRol) Then
                Return 0
            End If

            Try
                Dim rolSrv As New RolServicio()
                Dim roles = rolSrv.Buscar(nombreRol.Trim())
                Dim rol = roles.FirstOrDefault(
                    Function(r) r.RolNombre IsNot Nothing AndAlso
                                String.Equals(r.RolNombre.Trim(), nombreRol.Trim(), StringComparison.OrdinalIgnoreCase) AndAlso
                                (r.Estado Is Nothing OrElse String.Equals(r.Estado.Trim(), "ACTIVO", StringComparison.OrdinalIgnoreCase))
                )

                If rol IsNot Nothing Then
                    Return rol.RolId
                End If
            Catch
                Return 0
            End Try

            Return 0
        End Function

        Private Function ObtenerRolIdDesdeSesion() As Integer
            Dim rolId As Integer = 0

            If Session("RolId") IsNot Nothing Then
                Integer.TryParse(Session("RolId").ToString(), rolId)
            End If

            Return rolId
        End Function

    End Class
End Namespace