Option Strict On
Option Explicit On

Imports System
Imports System.Web
Imports System.Web.Mvc

Namespace Seguridad

    Public Class SecurityHeadersFilter
        Inherits ActionFilterAttribute

        Public Overrides Sub OnResultExecuting(ByVal filterContext As ResultExecutingContext)
            If filterContext Is Nothing OrElse filterContext.HttpContext Is Nothing Then
                MyBase.OnResultExecuting(filterContext)
                Return
            End If

            Dim response As HttpResponseBase = filterContext.HttpContext.Response
            Dim request As HttpRequestBase = filterContext.HttpContext.Request

            AddOrReplace(response, "X-Frame-Options", "SAMEORIGIN")
            AddOrReplace(response, "X-Content-Type-Options", "nosniff")
            AddOrReplace(response, "Referrer-Policy", "strict-origin-when-cross-origin")
            AddOrReplace(response, "X-Permitted-Cross-Domain-Policies", "none")
            AddOrReplace(response, "Permissions-Policy", "geolocation=(), microphone=(), camera=(), payment=()")
            AddOrReplace(response, "Content-Security-Policy", CrearPoliticaCsp())

            If request IsNot Nothing AndAlso request.IsSecureConnection Then
                AddOrReplace(response, "Strict-Transport-Security", "max-age=31536000; includeSubDomains")
            End If

            RemoveHeader(response, "Server")
            RemoveHeader(response, "X-AspNetMvc-Version")
            RemoveHeader(response, "X-AspNet-Version")

            MyBase.OnResultExecuting(filterContext)
        End Sub

        Private Shared Function CrearPoliticaCsp() As String
            Return String.Join(" ", New String() {
                "default-src 'self';",
                "base-uri 'self';",
                "object-src 'none';",
                "frame-ancestors 'self';",
                "img-src 'self' data: https://res.cloudinary.com https://*.cloudinary.com;",
                "font-src 'self' data: https://fonts.gstatic.com https://cdn.jsdelivr.net;",
                "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net;",
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net;",
                "connect-src 'self';",
                "form-action 'self';"
            })
        End Function

        Private Shared Sub AddOrReplace(ByVal response As HttpResponseBase, ByVal nombre As String, ByVal valor As String)
            Try
                response.Headers.Remove(nombre)
            Catch
            End Try

            Try
                response.Headers.Add(nombre, valor)
            Catch
            End Try
        End Sub

        Private Shared Sub RemoveHeader(ByVal response As HttpResponseBase, ByVal nombre As String)
            Try
                response.Headers.Remove(nombre)
            Catch
            End Try
        End Sub

    End Class

End Namespace
