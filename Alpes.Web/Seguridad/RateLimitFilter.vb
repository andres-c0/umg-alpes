Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Mvc

Namespace Seguridad

    Public Class RateLimitFilter
        Inherits FilterAttribute
        Implements IAuthorizationFilter

        Private Shared ReadOnly Solicitudes As New ConcurrentDictionary(Of String, Queue(Of DateTime))()

        Public Sub OnAuthorization(ByVal filterContext As AuthorizationContext) Implements IAuthorizationFilter.OnAuthorization
            If filterContext Is Nothing OrElse filterContext.HttpContext Is Nothing Then
                Return
            End If

            Dim request As HttpRequestBase = filterContext.HttpContext.Request

            If request Is Nothing Then
                Return
            End If

            Dim controller As String = Convert.ToString(filterContext.RouteData.Values("controller"))
            Dim action As String = Convert.ToString(filterContext.RouteData.Values("action"))
            Dim ip As String = ObtenerIp(request)
            Dim maximo As Integer = 300
            Dim ventana As TimeSpan = TimeSpan.FromMinutes(1)

            If String.Equals(controller, "Home", StringComparison.OrdinalIgnoreCase) AndAlso
               String.Equals(action, "Login", StringComparison.OrdinalIgnoreCase) AndAlso
               String.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) Then
                maximo = 8
                ventana = TimeSpan.FromMinutes(5)
            ElseIf String.Equals(controller, "Home", StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(action, "RecuperarContrasena", StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) Then
                maximo = 4
                ventana = TimeSpan.FromMinutes(10)
            ElseIf String.Equals(controller, "Home", StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(action, "Registro", StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) Then
                maximo = 5
                ventana = TimeSpan.FromMinutes(10)
            ElseIf EsMetodoModificacion(request.HttpMethod) Then
                maximo = 120
                ventana = TimeSpan.FromMinutes(1)
            End If

            Dim clave As String = String.Format("{0}|{1}|{2}|{3}", ip, controller, action, request.HttpMethod)

            If ExcedeLimite(clave, maximo, ventana) Then
                SecurityLog.Warn("RATE_LIMIT", String.Format("Bloqueado {0} {1}/{2} IP={3}", request.HttpMethod, controller, action, ip))
                filterContext.HttpContext.Response.StatusCode = 429

                If EsSolicitudJson(request, action) Then
                    filterContext.Result = New JsonResult With {
                        .Data = New With {.ok = False, .success = False, .message = "Demasiadas solicitudes. Intenta nuevamente más tarde."},
                        .JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    }
                Else
                    filterContext.Result = New HttpStatusCodeResult(429, "Demasiadas solicitudes. Intenta nuevamente más tarde.")
                End If
            End If
        End Sub

        Private Shared Function ExcedeLimite(ByVal clave As String, ByVal maximo As Integer, ByVal ventana As TimeSpan) As Boolean
            Dim ahora As DateTime = DateTime.UtcNow
            Dim minimo As DateTime = ahora.Subtract(ventana)
            Dim cola As Queue(Of DateTime) = Solicitudes.GetOrAdd(clave, Function(k) New Queue(Of DateTime)())

            SyncLock cola
                While cola.Count > 0 AndAlso cola.Peek() < minimo
                    cola.Dequeue()
                End While

                If cola.Count >= maximo Then
                    Return True
                End If

                cola.Enqueue(ahora)
            End SyncLock

            If Solicitudes.Count > 10000 Then
                LimpiarCache(minimo)
            End If

            Return False
        End Function

        Private Shared Sub LimpiarCache(ByVal minimo As DateTime)
            For Each item In Solicitudes.ToArray()
                Dim cola As Queue(Of DateTime) = item.Value

                SyncLock cola
                    While cola.Count > 0 AndAlso cola.Peek() < minimo
                        cola.Dequeue()
                    End While

                    If cola.Count = 0 Then
                        Dim removido As Queue(Of DateTime) = Nothing
                        Solicitudes.TryRemove(item.Key, removido)
                    End If
                End SyncLock
            Next
        End Sub

        Private Shared Function EsMetodoModificacion(ByVal metodo As String) As Boolean
            Return String.Equals(metodo, "POST", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(metodo, "PUT", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(metodo, "PATCH", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(metodo, "DELETE", StringComparison.OrdinalIgnoreCase)
        End Function

        Private Shared Function EsSolicitudJson(ByVal request As HttpRequestBase, ByVal action As String) As Boolean
            Dim aceptaJson As Boolean = Not String.IsNullOrWhiteSpace(request.Headers("Accept")) AndAlso request.Headers("Accept").Contains("application/json")
            Dim tipoJson As Boolean = Not String.IsNullOrWhiteSpace(request.ContentType) AndAlso request.ContentType.Contains("application/json")
            Dim accionData As Boolean = Not String.IsNullOrWhiteSpace(action) AndAlso action.EndsWith("Data", StringComparison.OrdinalIgnoreCase)

            Return request.IsAjaxRequest() OrElse aceptaJson OrElse tipoJson OrElse accionData
        End Function

        Private Shared Function ObtenerIp(ByVal request As HttpRequestBase) As String
            Dim forwarded As String = request.Headers("X-Forwarded-For")

            If Not String.IsNullOrWhiteSpace(forwarded) Then
                Return forwarded.Split(","c)(0).Trim()
            End If

            Return If(request.UserHostAddress, String.Empty)
        End Function

    End Class

End Namespace
