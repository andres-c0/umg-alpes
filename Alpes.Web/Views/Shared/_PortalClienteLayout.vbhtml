@Code
    Dim username As String = System.Convert.ToString(ViewData("Username"))

    If String.IsNullOrWhiteSpace(username) AndAlso Session("Username") IsNot Nothing Then
        username = Session("Username").ToString()
    End If

    If String.IsNullOrWhiteSpace(username) Then
        username = "Cliente"
    End If

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
    If String.IsNullOrWhiteSpace(cliIdTexto) AndAlso Session("CliId") IsNot Nothing Then
        cliIdTexto = Session("CliId").ToString()
    End If

    Dim initial As String = "C"
    If Not String.IsNullOrWhiteSpace(username) Then
        initial = username.Substring(0, 1).ToUpperInvariant()
    End If

    Dim currentAction As String = ""
    Dim currentController As String = ""

    If ViewContext IsNot Nothing AndAlso ViewContext.RouteData IsNot Nothing Then
        If ViewContext.RouteData.Values("action") IsNot Nothing Then
            currentAction = ViewContext.RouteData.Values("action").ToString()
        End If

        If ViewContext.RouteData.Values("controller") IsNot Nothing Then
            currentController = ViewContext.RouteData.Values("controller").ToString()
        End If
    End If

    Dim esPortalCliente As Boolean = String.Equals(currentController, "PortalCliente", StringComparison.OrdinalIgnoreCase)

    Dim claseInicio As String = "pc-nav-item"
    Dim claseCatalogo As String = "pc-nav-item"
    Dim claseFavoritos As String = "pc-nav-item"
    Dim claseMisOrdenes As String = "pc-nav-item"
    Dim claseCarrito As String = "pc-nav-item"
    Dim claseMiPerfil As String = "pc-nav-item"
    Dim claseConfiguracion As String = "pc-nav-item"

    If esPortalCliente Then
        If String.Equals(currentAction, "Index", StringComparison.OrdinalIgnoreCase) Then
            claseInicio &= " active"
        End If

        If String.Equals(currentAction, "DetalleProducto", StringComparison.OrdinalIgnoreCase) Then
            claseCatalogo &= " active"
        End If

        If String.Equals(currentAction, "MisFavoritos", StringComparison.OrdinalIgnoreCase) Then
            claseFavoritos &= " active"
        End If

        If String.Equals(currentAction, "MisOrdenes", StringComparison.OrdinalIgnoreCase) OrElse
           String.Equals(currentAction, "DetalleOrden", StringComparison.OrdinalIgnoreCase) OrElse
           String.Equals(currentAction, "Tracking", StringComparison.OrdinalIgnoreCase) Then
            claseMisOrdenes &= " active"
        End If

        If String.Equals(currentAction, "Carrito", StringComparison.OrdinalIgnoreCase) OrElse
           String.Equals(currentAction, "Checkout", StringComparison.OrdinalIgnoreCase) Then
            claseCarrito &= " active"
        End If

        If String.Equals(currentAction, "MiPerfil", StringComparison.OrdinalIgnoreCase) OrElse
           String.Equals(currentAction, "MisTarjetas", StringComparison.OrdinalIgnoreCase) Then
            claseMiPerfil &= " active"
        End If

        If String.Equals(currentAction, "Configuracion", StringComparison.OrdinalIgnoreCase) Then
            claseConfiguracion &= " active"
        End If
    End If
End Code

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData("Title") - Muebles de los Alpes</title>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/css")
    <link rel="stylesheet" href="@Url.Content("~/Content/portal-cliente.css")" />
</head>
<body>
    <div class="pc-layout" data-cli-id="@cliIdTexto">
        <aside class="pc-sidebar" id="pcSidebar">
            <div>
                <div class="pc-brand">
                    <div class="pc-brand-title">Muebles de los Alpes</div>
                    <div class="pc-brand-subtitle">Panel del cliente</div>
                </div>

                <div class="pc-nav-group-title">COMERCIAL</div>
                <nav class="pc-nav">
                    <a href="@Url.Action("Index", "PortalCliente")" class="@claseInicio">
                        <i class="bi bi-house"></i>
                        <span>Inicio</span>
                    </a>

                    <a href="@Url.Action("Index", "PortalCliente")#catalogo" class="@claseCatalogo">
                        <i class="bi bi-grid"></i>
                        <span>Catalogo</span>
                    </a>

                    <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="@claseFavoritos">
                        <i class="bi bi-heart"></i>
                        <span>Favoritos</span>
                    </a>

                    <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="@claseMisOrdenes">
                        <i class="bi bi-receipt"></i>
                        <span>Mis pedidos</span>
                        <span class="pc-nav-badge" id="pcBadgeOrders">0</span>
                    </a>

                    <a href="@Url.Action("Carrito", "PortalCliente")" class="@claseCarrito">
                        <i class="bi bi-cart3"></i>
                        <span>Carrito</span>
                        <span class="pc-nav-badge" id="pcBadgeCart">0</span>
                    </a>
                </nav>

                <div class="pc-nav-group-title">CUENTA</div>
                <nav class="pc-nav">
                    <a href="@Url.Action("MiPerfil", "PortalCliente")" class="@claseMiPerfil">
                        <i class="bi bi-person"></i>
                        <span>Perfil</span>
                    </a>

                    <a href="@Url.Action("Configuracion", "PortalCliente")" class="@claseConfiguracion">
                        <i class="bi bi-gear"></i>
                        <span>Configuracion</span>
                    </a>
                </nav>
            </div>

            <div class="pc-sidebar-footer">
                <div class="pc-user-box">
                    <div class="pc-user-avatar">@initial</div>
                    <div>
                        <div class="pc-user-name">@username</div>
                        <div class="pc-user-email">CLI_ID: @cliIdTexto</div>
                    </div>
                </div>

                <a href="@Url.Action("Logout", "Home")" class="pc-logout">
                    <i class="bi bi-box-arrow-left"></i>
                    <span>Cerrar sesion</span>
                </a>
            </div>
        </aside>

        <div class="pc-overlay" id="pcOverlay"></div>

        <main class="pc-main">
            <header class="pc-topbar">
                <button id="pcMenuButton" class="pc-menu-btn" type="button" aria-label="Abrir menu">
                    <i class="bi bi-list"></i>
                </button>

                <div class="pc-topbar-title">@ViewData("Title")</div>

                <div class="pc-topbar-actions">
                    <a href="@Url.Action("Notificaciones", "PortalCliente")" class="pc-topbar-icon">
                        <i class="bi bi-bell"></i>
                    </a>

                    <a href="@Url.Action("Carrito", "PortalCliente")" class="pc-topbar-icon pc-topbar-icon--cart">
                        <i class="bi bi-cart3"></i>
                        <span class="pc-topbar-badge" id="pcTopbarCartBadge">0</span>
                    </a>

                    <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pc-avatar">@initial</a>
                </div>
            </header>

            <div class="pc-content">
                @RenderBody()
            </div>
        </main>
    </div>

    @Scripts.Render("~/bundles/jquery")
    <script src="@Url.Content("~/Scripts/portal-cliente.js?v=2")"></script>
    @RenderSection("scripts", required:=False)
</body>
</html>