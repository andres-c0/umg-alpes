@Code
    Dim tituloPagina As String = Convert.ToString(ViewData("Title"))

    If String.IsNullOrWhiteSpace(tituloPagina) Then
        tituloPagina = "Muebles de los Alpes"
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

    Dim esHome As Boolean = String.Equals(currentController, "Home", StringComparison.OrdinalIgnoreCase) AndAlso
                             String.Equals(currentAction, "Index", StringComparison.OrdinalIgnoreCase)

    Dim esCatalogo As Boolean = String.Equals(currentController, "Home", StringComparison.OrdinalIgnoreCase) AndAlso
                                 String.Equals(currentAction, "CatalogoPublico", StringComparison.OrdinalIgnoreCase)

    Dim esCarrito As Boolean = String.Equals(currentController, "Home", StringComparison.OrdinalIgnoreCase) AndAlso
                                String.Equals(currentAction, "CarritoInvitado", StringComparison.OrdinalIgnoreCase)
End Code

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>@tituloPagina - Muebles de los Alpes</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Lato:wght@400;500;600;700;800;900&family=Playfair+Display:wght@600;700;800&display=swap" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    @Styles.Render("~/Content/css")

    <link rel="stylesheet" href="@Url.Content("~/Content/alpes.css?v=1")" />
    <link rel="stylesheet" href="@Url.Content("~/Content/home-publico.css?v=1")" />

    @RenderSection("styles", required:=False)
</head>

<body class="public-body">
    <div class="public-shell">
        <header class="public-header">
            <div class="public-header-inner">
                <a href="@Url.Action("Index", "Home")" class="public-brand">
                    <span class="public-brand-icon">
                        <i class="bi bi-house-heart-fill"></i>
                    </span>

                    <span class="public-brand-copy">
                        <strong>Muebles de los Alpes</strong>
                        <small>Artesanía · Calidad · Diseño</small>
                    </span>
                </a>

                <nav class="public-nav" aria-label="Navegación pública">
                    <a href="@Url.Action("Index", "Home")" class="public-nav-link @(If(esHome, "active", ""))">
                        Inicio
                    </a>

                    <a href="@Url.Action("CatalogoPublico", "Home")" class="public-nav-link @(If(esCatalogo, "active", ""))">
                        Catálogo
                    </a>

                    <a href="@Url.Action("Login", "Home")" class="public-nav-link">
                        Iniciar sesión
                    </a>

                    <a href="@Url.Action("Registro", "Home")" class="public-nav-link public-nav-link-primary">
                        Crear cuenta
                    </a>
                </nav>

                <div class="public-actions">
                    <a href="@Url.Action("CarritoInvitado", "Home")" class="public-cart-link @(If(esCarrito, "active", ""))" aria-label="Ver carrito">
                        <i class="bi bi-cart3"></i>
                        <span id="publicCartCount" class="public-cart-badge">0</span>
                    </a>

                    <button type="button" class="public-menu-button" id="publicMenuButton" aria-label="Abrir menú">
                        <i class="bi bi-list"></i>
                    </button>
                </div>
            </div>

            <div class="public-mobile-menu" id="publicMobileMenu">
                <a href="@Url.Action("Index", "Home")" class="public-mobile-link @(If(esHome, "active", ""))">
                    <i class="bi bi-house"></i>
                    <span>Inicio</span>
                </a>

                <a href="@Url.Action("CatalogoPublico", "Home")" class="public-mobile-link @(If(esCatalogo, "active", ""))">
                    <i class="bi bi-grid"></i>
                    <span>Catálogo</span>
                </a>

                <a href="@Url.Action("CarritoInvitado", "Home")" class="public-mobile-link @(If(esCarrito, "active", ""))">
                    <i class="bi bi-cart3"></i>
                    <span>Carrito</span>
                    <strong id="publicMobileCartCount">0</strong>
                </a>

                <a href="@Url.Action("Login", "Home")" class="public-mobile-link">
                    <i class="bi bi-box-arrow-in-right"></i>
                    <span>Iniciar sesión</span>
                </a>

                <a href="@Url.Action("Registro", "Home")" class="public-mobile-link public-mobile-link-primary">
                    <i class="bi bi-person-plus"></i>
                    <span>Crear cuenta</span>
                </a>
            </div>
        </header>

        <main class="public-main">
            @RenderBody()
        </main>

        <footer class="public-footer">
            <div class="public-footer-inner">
                <div class="public-footer-brand">
                    <div class="public-brand public-brand-footer">
                        <span class="public-brand-icon">
                            <i class="bi bi-house-heart-fill"></i>
                        </span>

                        <span class="public-brand-copy">
                            <strong>Muebles de los Alpes</strong>
                            <small>Diseño para interior y exterior</small>
                        </span>
                    </div>

                    <p>
                        Creamos muebles con materiales de calidad, acabados elegantes y una experiencia de compra cómoda para nuestros clientes.
                    </p>
                </div>

                <div class="public-footer-group">
                    <h4>Tienda</h4>
                    <a href="@Url.Action("Index", "Home")">Inicio</a>
                    <a href="@Url.Action("CatalogoPublico", "Home")">Catálogo</a>
                    <a href="@Url.Action("CarritoInvitado", "Home")">Carrito</a>
                </div>

                <div class="public-footer-group">
                    <h4>Cuenta</h4>
                    <a href="@Url.Action("Login", "Home")">Iniciar sesión</a>
                    <a href="@Url.Action("Registro", "Home")">Crear cuenta</a>
                </div>

                <div class="public-footer-group">
                    <h4>Contacto</h4>
                    <span>Guatemala</span>
                    <span>Atención al cliente</span>
                    <span>Soporte de compras</span>
                </div>
            </div>

            <div class="public-footer-bottom">
                <span>© @DateTime.Now.Year Muebles de los Alpes.</span>
                <span>Proyecto Base de Datos II.</span>
            </div>
        </footer>

        <a href="@Url.Action("Login", "Home")" class="public-floating-button" aria-label="Acceso a cuenta">
            <i class="bi bi-chat-heart"></i>
        </a>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")

    <script src="@Url.Content("~/Scripts/home-publico.js?v=1")"></script>

    @RenderSection("scripts", required:=False)
</body>
</html>