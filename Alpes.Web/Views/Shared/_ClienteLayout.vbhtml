@Code
    Dim username As String = ""
    Dim avatarLetter As String = "C"
    Dim displayName As String = "Cliente"

    If Session("Username") IsNot Nothing Then
        username = Session("Username").ToString()
    End If

    If Not String.IsNullOrWhiteSpace(username) Then
        displayName = username
        avatarLetter = username.Substring(0, 1).ToUpper()
    End If
End Code
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData("Title") - Muebles de los Alpes</title>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/alpes.css")
    <link rel="stylesheet" href="@Url.Content("~/Content/cliente-panel.css")" />
</head>
<body>
    <div class="c-shell">
        <aside class="c-sidebar" id="clienteSidebar">
            <div>
                <div class="c-sidebar__brand">
                    <div class="c-sidebar__title">Muebles de los Alpes</div>
                    <div class="c-sidebar__subtitle">Portal del Cliente</div>
                </div>

                <div class="c-nav-section">MI CUENTA</div>

                <nav class="c-nav">
                    <a href="@Url.Action("Index", "PortalCliente")" class="c-nav__item active">
                        <i class="bi bi-grid-fill"></i>
                        <span>Inicio</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-person"></i>
                        <span>Mi perfil</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-credit-card"></i>
                        <span>Mis tarjetas</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-bag"></i>
                        <span>Mis pedidos</span>
                        <span class="c-nav-badge">3</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-geo-alt"></i>
                        <span>Tracking</span>
                    </a>
                </nav>

                <div class="c-nav-section">TIENDA</div>

                <nav class="c-nav">
                    <a href="#" class="c-nav__item">
                        <i class="bi bi-grid"></i>
                        <span>Cat�logo</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-clock-history"></i>
                        <span>Historial</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-bell"></i>
                        <span>Notificaciones</span>
                        <span class="c-nav-badge">2</span>
                    </a>

                    <a href="#" class="c-nav__item">
                        <i class="bi bi-gear"></i>
                        <span>Configuraci�n</span>
                    </a>
                </nav>
            </div>

            <div class="c-sidebar__footer">
                <div class="c-user">
                    <div class="c-user__avatar">@avatarLetter</div>
                    <div class="c-user__meta">
                        <div class="c-user__name">@displayName</div>
                        <div class="c-user__mail">cliente@alpes.com</div>
                    </div>
                </div>

                <a href="@Url.Action("Logout", "Home")" class="c-logout">
                    <i class="bi bi-box-arrow-left"></i>
                    <span>Cerrar sesi�n</span>
                </a>
            </div>
        </aside>

        <div class="c-overlay" id="clienteOverlay"></div>

        <main class="c-main">
            <header class="c-topbar">
                <button class="c-menu-btn" id="btnOpenClienteMenu">
                    <i class="bi bi-list"></i>
                </button>

                <div class="c-topbar__title">@ViewData("Title")</div>
            </header>

            <section class="c-content">
                @RenderBody()
            </section>
        </main>
    </div>

    @Scripts.Render("~/bundles/jquery")
    <script src="@Url.Content("~/Scripts/cliente-panel.js")"></script>
    @RenderSection("scripts", required:=False)
</body>
</html>