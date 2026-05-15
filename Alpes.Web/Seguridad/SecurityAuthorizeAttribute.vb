Option Strict On
Option Explicit On

Imports System
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Namespace Seguridad

    Public Class SecurityAuthorizeAttribute
        Inherits FilterAttribute
        Implements IAuthorizationFilter

        Private Shared ReadOnly AccionesPublicasHome As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
            "Index",
            "DetalleProducto",
            "CarritoInvitado",
            "CatalogoPublico",
            "ObtenerProductosPublicosData",
            "ObtenerProductoPublicoData",
            "Login",
            "Registro",
            "Logout"
        }

        Public Sub OnAuthorization(ByVal filterContext As AuthorizationContext) Implements IAuthorizationFilter.OnAuthorization
            If filterContext Is Nothing OrElse filterContext.HttpContext Is Nothing Then
                Return
            End If

            If TieneAllowAnonymous(filterContext) Then
                Return
            End If

            Dim controller As String = Convert.ToString(filterContext.RouteData.Values("controller"))
            Dim action As String = Convert.ToString(filterContext.RouteData.Values("action"))

            If String.Equals(controller, "Home", StringComparison.OrdinalIgnoreCase) AndAlso AccionesPublicasHome.Contains(action) Then
                Return
            End If

            If String.Equals(controller, "PortalCliente", StringComparison.OrdinalIgnoreCase) Then
                If Not EstaAutenticado(filterContext.HttpContext) Then
                    Denegar(filterContext, 401, "Sesión requerida.")
                    Return
                End If

                If Not EsCliente(filterContext.HttpContext) Then
                    Denegar(filterContext, 403, "Solo clientes pueden acceder a este recurso.")
                    Return
                End If

                Return
            End If

            If Not EstaAutenticado(filterContext.HttpContext) Then
                Denegar(filterContext, 401, "Sesión requerida.")
                Return
            End If

            If Not EsAdministrador(filterContext.HttpContext) Then
                Denegar(filterContext, 403, "No autorizado.")
                Return
            End If
        End Sub

        Private Shared Function TieneAllowAnonymous(ByVal filterContext As AuthorizationContext) As Boolean
            Try
                Return filterContext.ActionDescriptor.IsDefined(GetType(AllowAnonymousAttribute), True) OrElse
                       filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(GetType(AllowAnonymousAttribute), True)
            Catch
                Return False
            End Try
        End Function

        Private Shared Function EstaAutenticado(ByVal httpContext As HttpContextBase) As Boolean
            Return httpContext IsNot Nothing AndAlso
                   httpContext.Session IsNot Nothing AndAlso
                   httpContext.Session("UsuarioId") IsNot Nothing
        End Function

        Private Shared Function EsCliente(ByVal httpContext As HttpContextBase) As Boolean
            Dim rol As String = ObtenerRolSesion(httpContext)

            If String.IsNullOrWhiteSpace(rol) Then
                Return False
            End If

            Dim cliId As Integer = ObtenerEnteroSesion(httpContext, "CliId")
            Return cliId > 0 AndAlso rol.ToUpperInvariant().Contains("CLIENTE")
        End Function

        Private Shared Function EsAdministrador(ByVal httpContext As HttpContextBase) As Boolean
            Dim rol As String = ObtenerRolSesion(httpContext)

            If String.IsNullOrWhiteSpace(rol) Then
                Return False
            End If

            Dim normalizado As String = rol.Trim().ToUpperInvariant()

            If normalizado.Contains("CLIENTE") Then
                Return False
            End If

            If normalizado.Contains("ADMIN") OrElse
               normalizado.Contains("GERENTE") OrElse
               normalizado.Contains("SUPERVISOR") Then
                Return True
            End If

            Dim fallback As Boolean = False
            Boolean.TryParse(ConfigurationManager.AppSettings("Security:AllowNonClienteAdminFallback"), fallback)

            Return fallback
        End Function

        Private Shared Function ObtenerRolSesion(ByVal httpContext As HttpContextBase) As String
            Try
                If httpContext.Session("RolNombre") Is Nothing Then
                    Return String.Empty
                End If

                Return Convert.ToString(httpContext.Session("RolNombre"))
            Catch
                Return String.Empty
            End Try
        End Function

        Private Shared Function ObtenerEnteroSesion(ByVal httpContext As HttpContextBase, ByVal clave As String) As Integer
            Try
                If httpContext.Session(clave) Is Nothing Then
                    Return 0
                End If

                Return Convert.ToInt32(httpContext.Session(clave))
            Catch
                Return 0
            End Try
        End Function

        Private Shared Sub Denegar(ByVal filterContext As AuthorizationContext, ByVal codigo As Integer, ByVal mensaje As String)
            Dim request As HttpRequestBase = filterContext.HttpContext.Request
            Dim controller As String = Convert.ToString(filterContext.RouteData.Values("controller"))
            Dim action As String = Convert.ToString(filterContext.RouteData.Values("action"))

            SecurityLog.Warn("ACCESS_DENIED", String.Format("{0} {1}/{2} IP={3} Motivo={4}", codigo, controller, action, ObtenerIp(request), mensaje))

            If EsSolicitudJson(request, action) Then
                filterContext.HttpContext.Response.StatusCode = codigo
                filterContext.Result = New JsonResult With {
                    .Data = New With {.ok = False, .success = False, .message = mensaje},
                    .JsonRequestBehavior = JsonRequestBehavior.AllowGet
                }
                Return
            End If

            If codigo = 401 Then
                Dim returnUrl As String = If(request IsNot Nothing, request.RawUrl, String.Empty)
                filterContext.Result = New RedirectToRouteResult(New RouteValueDictionary(New With {
                    .controller = "Home",
                    .action = "Login",
                    .returnUrl = returnUrl
                }))
                Return
            End If

            filterContext.Result = New HttpStatusCodeResult(codigo, mensaje)
        End Sub

        Private Shared Function EsSolicitudJson(ByVal request As HttpRequestBase, ByVal action As String) As Boolean
            If request Is Nothing Then
                Return False
            End If

            Dim aceptaJson As Boolean = Not String.IsNullOrWhiteSpace(request.Headers("Accept")) AndAlso request.Headers("Accept").Contains("application/json")
            Dim tipoJson As Boolean = Not String.IsNullOrWhiteSpace(request.ContentType) AndAlso request.ContentType.Contains("application/json")
            Dim accionData As Boolean = Not String.IsNullOrWhiteSpace(action) AndAlso action.EndsWith("Data", StringComparison.OrdinalIgnoreCase)

            Return request.IsAjaxRequest() OrElse aceptaJson OrElse tipoJson OrElse accionData
        End Function

        Private Shared Function ObtenerIp(ByVal request As HttpRequestBase) As String
            If request Is Nothing Then
                Return String.Empty
            End If

            Dim forwarded As String = request.Headers("X-Forwarded-For")

            If Not String.IsNullOrWhiteSpace(forwarded) Then
                Return forwarded.Split(","c)(0).Trim()
            End If

            Return If(request.UserHostAddress, String.Empty)
        End Function

    End Class

End Namespace
