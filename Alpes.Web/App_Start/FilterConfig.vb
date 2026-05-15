Imports System.Web
Imports System.Web.Mvc
Imports Alpes.Web.Seguridad

Public Module FilterConfig
    Public Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
        filters.Add(New HandleErrorAttribute())
        filters.Add(New SecurityHeadersFilter())
        filters.Add(New RateLimitFilter())
        filters.Add(New JsonAntiForgeryFilter())
        filters.Add(New SecurityAuthorizeAttribute())
    End Sub
End Module
