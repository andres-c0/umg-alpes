@Code
    Dim username As String = System.Convert.ToString(ViewData("NombreCliente"))

    If String.IsNullOrWhiteSpace(username) AndAlso Session("NombreCliente") IsNot Nothing Then
        username = Session("NombreCliente").ToString()
    End If

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

    Dim primerNombre As String = username
    If Not String.IsNullOrWhiteSpace(username) AndAlso username.Contains(" ") Then
        primerNombre = username.Split(" "c)(0)
    End If

    Dim currentAction As String = ""
    Dim currentController As String = ""
    If ViewContext IsNot Nothing AndAlso ViewContext.RouteData IsNot Nothing Then
        If ViewContext.RouteData.Values("action") IsNot Nothing Then currentAction = ViewContext.RouteData.Values("action").ToString()
        If ViewContext.RouteData.Values("controller") IsNot Nothing Then currentController = ViewContext.RouteData.Values("controller").ToString()
    End If

    Dim esInicio As Boolean = String.Equals(currentAction, "Index", StringComparison.OrdinalIgnoreCase)
    Dim esCatalogo As Boolean = String.Equals(currentAction, "Busqueda", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(currentAction, "DetalleProducto", StringComparison.OrdinalIgnoreCase)
    Dim esFavoritos As Boolean = String.Equals(currentAction, "MisFavoritos", StringComparison.OrdinalIgnoreCase)
    Dim esOrdenes As Boolean = String.Equals(currentAction, "MisOrdenes", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(currentAction, "DetalleOrden", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(currentAction, "Tracking", StringComparison.OrdinalIgnoreCase)
    Dim esCarrito As Boolean = String.Equals(currentAction, "Carrito", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(currentAction, "Checkout", StringComparison.OrdinalIgnoreCase)
    Dim esPerfil As Boolean = String.Equals(currentAction, "MiPerfil", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(currentAction, "MisTarjetas", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(currentAction, "MisResenas", StringComparison.OrdinalIgnoreCase)
    Dim esConfig As Boolean = String.Equals(currentAction, "Configuracion", StringComparison.OrdinalIgnoreCase)
End Code
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData("Title") - Muebles de los Alpes</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700;800;900&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/css")
    <link rel="stylesheet" href="@Url.Content("~/Content/portal-cliente.css?v=18")" />
</head>
<body>
    <div class="pc-layout mapp-shell" data-cli-id="@cliIdTexto">
        <aside class="pc-sidebar mapp-sidebar" id="pcSidebar">
            <div class="mapp-sidebar-bg mapp-sidebar-bg-a"></div>
            <div class="mapp-sidebar-bg mapp-sidebar-bg-b"></div>

            <div class="mapp-sidebar-inner">
                <div class="mapp-brand-row">
                    <div class="mapp-brand-icon"><i class="bi bi-house-heart-fill"></i></div>
                    <div>
                        <div class="mapp-brand-name">Muebles de los Alpes</div>
                        <div class="mapp-brand-caption">Artesanía · Calidad</div>
                    </div>
                </div>

                <div class="mapp-account-card">
                    <div class="mapp-account-avatar">@initial</div>
                    <div class="mapp-account-copy">
                        <strong>@username</strong>
                        <span>Cliente activo · CLI @cliIdTexto</span>
                    </div>
                    <span class="mapp-vip">VIP</span>
                </div>

                <div class="pc-nav-group-title">TIENDA</div>

                <nav class="pc-nav mapp-nav">
                    <a href="@Url.Action("Index", "PortalCliente")"
                       class="pc-nav-item @(If(esInicio, "active", ""))">
                        <i class="bi bi-house"></i>
                        <span>Inicio</span>
                    </a>

                    <div class="pc-nav-dropdown">
                        <button type="button" class="pc-nav-item pc-nav-toggle" onclick="this.parentElement.classList.toggle('open')">
                            <i class="bi bi-grid"></i>
                            <span>Catálogo</span>
                            <i class="bi bi-chevron-down pc-chevron"></i>
                        </button>

                        <div class="pc-nav-submenu">
                            <a href="@Url.Action("Busqueda", "PortalCliente")"
                               class="pc-nav-item @(If(esCatalogo, "active", ""))">
                                <i class="bi bi-grid"></i>
                                <span>Catálogo</span>
                            </a>

                            <a href="@Url.Action("MisFavoritos", "PortalCliente")"
                               class="pc-nav-item @(If(esFavoritos, "active", ""))">
                                <i class="bi bi-heart"></i>
                                <span>Favoritos</span>
                            </a>

                            <a href="@Url.Action("MisOrdenes", "PortalCliente")"
                               class="pc-nav-item @(If(esOrdenes, "active", ""))">
                                <i class="bi bi-receipt"></i>
                                <span>Órdenes</span>
                                <span class="pc-nav-badge" id="pcBadgeOrders">0</span>
                            </a>

                            <a href="@Url.Action("Carrito", "PortalCliente")"
                               class="pc-nav-item @(If(esCarrito, "active", ""))">
                                <i class="bi bi-cart3"></i>
                                <span>Carrito</span>
                                <span class="pc-nav-badge" id="pcBadgeCart">0</span>
                            </a>
                        </div>
                    </div>
                </nav>

                <div class="pc-nav-group-title">MI CUENTA</div>

                <nav class="pc-nav mapp-nav">

                    <div class="perfil-menu">
                        <input type="checkbox" id="perfilToggle" class="perfil-toggle-check" />

                        <label for="perfilToggle" class="pc-nav-item perfil-toggle-label">
                            <i class="bi bi-person"></i>
                            <span>Perfil</span>
                            <i class="bi bi-chevron-down pc-chevron"></i>
                        </label>

                        <div class="perfil-submenu">
                            <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pc-nav-item">
                                <i class="bi bi-person"></i>
                                <span>Perfil</span>
                            </a>

                            <a href="@Url.Action("MisTarjetas", "PortalCliente")" class="pc-nav-item">
                                <i class="bi bi-credit-card"></i>
                                <span>Tarjetas</span>
                            </a>

                            <a href="@Url.Action("MisResenas", "PortalCliente")" class="pc-nav-item">
                                <i class="bi bi-star"></i>
                                <span>Reseñas</span>
                            </a>
                        </div>
                    </div>

                    <a href="@Url.Action("Configuracion", "PortalCliente")" class="pc-nav-item @(If(esConfig, "active", ""))">
                        <i class="bi bi-gear"></i>
                        <span>Configuración</span>
                    </a>

                </nav>
            </div>

            <div class="pc-sidebar-footer mapp-sidebar-footer">
                <a href="@Url.Action("Soporte", "PortalCliente")" class="mapp-help-link"><i class="bi bi-headset"></i><span>Soporte Alpes</span></a>
                <a href="@Url.Action("Logout", "Home")" class="pc-logout"><i class="bi bi-box-arrow-left"></i><span>Cerrar sesión</span></a>
            </div>
        </aside>

        <div class="pc-overlay" id="pcOverlay"></div>

        <main class="pc-main mapp-main">
            <header class="pc-topbar mapp-topbar">
                <button id="pcMenuButton" class="pc-menu-btn mapp-menu-btn modern-menu-btn" type="button" aria-label="Abrir menú">
                    <div class="menu-dots">
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                </button>                <div class="mapp-topbar-copy">
                    <div class="mapp-topbar-hello">Hola, @primerNombre <span class="mapp-topbar-vip">VIP</span></div>
                    <div class="mapp-topbar-sub">Muebles de los Alpes</div>
                </div>
                <div class="pc-topbar-actions mapp-topbar-actions">
                    <a href="@Url.Action("Carrito", "PortalCliente")" class="pc-topbar-icon pc-topbar-icon--cart"><i class="bi bi-cart3"></i><span class="pc-topbar-badge" id="pcTopbarCartBadge">0</span></a>
                    <a href="@Url.Action("Notificaciones", "PortalCliente")" class="pc-topbar-icon"><i class="bi bi-bell"></i></a>
                    <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pc-avatar mapp-avatar">@initial</a>
                </div>
            </header>

            <div class="pc-content mapp-content">
                @RenderBody()
            </div>
        </main>

        <a href="@Url.Action("Soporte", "PortalCliente")" class="mapp-bot" aria-label="Soporte"><i class="bi bi-chat-heart"></i></a>

        <nav class="pc-bottom-nav mapp-bottom-nav" aria-label="Navegación móvil del cliente">
            <a href="@Url.Action("Index", "PortalCliente")" class="pc-bottom-item @(If(esInicio, "active", ""))"><i class="bi bi-house"></i><span>Inicio</span></a>
            <a href="@Url.Action("Busqueda", "PortalCliente")" class="pc-bottom-item @(If(esCatalogo, "active", ""))"><i class="bi bi-grid"></i><span>Catálogo</span></a>
            <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="pc-bottom-item @(If(esFavoritos, "active", ""))"><i class="bi bi-heart"></i><span>Favoritos</span></a>
            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="pc-bottom-item @(If(esOrdenes, "active", ""))"><i class="bi bi-receipt"></i><span>Órdenes</span></a>
            <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pc-bottom-item @(If(esPerfil, "active", ""))"><i class="bi bi-person"></i><span>Perfil</span></a>
        </nav>
    </div>

    @Scripts.Render("~/bundles/jquery")
    <script src="@Url.Content("~/Scripts/portal-cliente.js?v=18")"></script>

    @RenderSection("scripts", required:=False)

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            document.querySelectorAll(".pc-nav-toggle, .pc-profile-toggle").forEach(function (toggle) {
                toggle.addEventListener("click", function () {
                    var dropdown = toggle.closest(".pc-nav-dropdown");
                    if (dropdown) {
                        dropdown.classList.toggle("open");
                    }
                });
            });
        });
    </script>
</body>
</html>