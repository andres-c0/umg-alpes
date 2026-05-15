Option Strict On
Option Explicit On

Imports System
Imports System.Web
Imports System.Web.Helpers
Imports System.Web.Mvc

Namespace Seguridad

    Public Class JsonAntiForgeryFilter
        Inherits FilterAttribute
        Implements IAuthorizationFilter

        Public Sub OnAuthorization(ByVal filterContext As AuthorizationContext) Implements IAuthorizationFilter.OnAuthorization
            If filterContext Is Nothing OrElse filterContext.HttpContext Is Nothing Then
                Return
            End If

            Dim request As HttpRequestBase = filterContext.HttpContext.Request

            If request Is Nothing OrElse Not EsMetodoModificacion(request.HttpMethod) Then
                Return
            End If

            If TieneAllowAnonymous(filterContext) Then
                Return
            End If

            Dim tokenFormulario As String = request.Form("__RequestVerificationToken")
            Dim tokenHeader As String = request.Headers("RequestVerificationToken")

            If String.IsNullOrWhiteSpace(tokenHeader) Then
                tokenHeader = request.Headers("X-CSRF-Token")
            End If

            Dim token As String = If(Not String.IsNullOrWhiteSpace(tokenHeader), tokenHeader, tokenFormulario)
            Dim cookieName As String = AntiForgeryConfig.CookieName

            If String.IsNullOrWhiteSpace(cookieName) Then
                cookieName = "__RequestVerificationToken"
            End If

            Dim cookie As HttpCookie = request.Cookies(cookieName)

            If cookie Is Nothing OrElse String.IsNullOrWhiteSpace(token) Then
                Denegar(filterContext, "Token antifalsificación ausente.")
                Return
            End If

            Try
                AntiForgery.Validate(cookie.Value, token)
            Catch ex As HttpAntiForgeryException
                Denegar(filterContext, "Token antifalsificación inválido.")
            End Try
        End Sub

        Private Shared Function EsMetodoModificacion(ByVal metodo As String) As Boolean
            Return String.Equals(metodo, "POST", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(metodo, "PUT", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(metodo, "PATCH", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(metodo, "DELETE", StringComparison.OrdinalIgnoreCase)
        End Function

        Private Shared Function TieneAllowAnonymous(ByVal filterContext As AuthorizationContext) As Boolean
            Try
                Return filterContext.ActionDescriptor.IsDefined(GetType(AllowAnonymousAttribute), True) OrElse
                       filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(GetType(AllowAnonymousAttribute), True)
            Catch
                Return False
            End Try
        End Function

        Private Shared Sub Denegar(ByVal filterContext As AuthorizationContext, ByVal mensaje As String)
            Dim request As HttpRequestBase = filterContext.HttpContext.Request
            SecurityLog.Warn("CSRF_BLOCKED", String.Format("{0} {1}", request.HttpMethod, request.RawUrl))

            filterContext.HttpContext.Response.StatusCode = 403

            If EsSolicitudJson(request) Then
                filterContext.Result = New JsonResult With {
                    .Data = New With {.ok = False, .success = False, .message = mensaje},
                    .JsonRequestBehavior = JsonRequestBehavior.AllowGet
                }
            Else
                filterContext.Result = New HttpStatusCodeResult(403, mensaje)
            End If
        End Sub

        Private Shared Function EsSolicitudJson(ByVal request As HttpRequestBase) As Boolean
            If request Is Nothing Then
                Return False
            End If

            Dim aceptaJson As Boolean = Not String.IsNullOrWhiteSpace(request.Headers("Accept")) AndAlso request.Headers("Accept").Contains("application/json")
            Dim tipoJson As Boolean = Not String.IsNullOrWhiteSpace(request.ContentType) AndAlso request.ContentType.Contains("application/json")

            Return request.IsAjaxRequest() OrElse aceptaJson OrElse tipoJson
        End Function

    End Class

End Namespace
