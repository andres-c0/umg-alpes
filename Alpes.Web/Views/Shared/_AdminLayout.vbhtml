@Code
    Dim pageTitle As String = "Dashboard"
    Dim username As String = ""
    Dim avatarLetter As String = "A"
    Dim displayName As String = "Administrador"
    Dim userHandle As String = "Administrador"

    If ViewData("Title") IsNot Nothing Then
        pageTitle = ViewData("Title").ToString()
    End If

    If Session("Username") IsNot Nothing Then
        username = Session("Username").ToString()
    End If

    If Not String.IsNullOrWhiteSpace(username) Then
        displayName = username
        userHandle = username
        avatarLetter = username.Substring(0, 1).ToUpper()
    End If
End Code

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@pageTitle - Muebles de los Alpes</title>

    <link rel="stylesheet" href="@Url.Content("~/Content/alpes.css")?v=@DateTime.Now.Ticks" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
</head>

<body>
    @Html.AntiForgeryToken()

    <div class="admin-shell">
        <aside class="admin-sidebar">
            <div class="admin-sidebar__bg admin-sidebar__bg--top"></div>
            <div class="admin-sidebar__bg admin-sidebar__bg--middle"></div>
            <div class="admin-sidebar__bg admin-sidebar__bg--bottom"></div>

            <div class="admin-sidebar__inner">
                <div>
                    <div class="admin-sidebar__brand">
                        <div class="admin-sidebar__logo">
                            <i class="bi bi-shop"></i>
                        </div>
                        <div>
                            <div class="admin-sidebar__title">Muebles de los Alpes</div>
                            <div class="admin-sidebar__subtitle">Panel Administrativo</div>
                        </div>
                    </div>

                    <div class="admin-sidebar__divider"></div>

                    <div class="admin-sidebar__section">Comercial</div>

                    <nav class="admin-sidebar__nav">
                        <a href="@Url.Action("Index", "Admin")" class="admin-nav__item @(If(pageTitle = "Dashboard", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-speedometer2"></i>
                                <span>Dashboard</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Productos", "Admin")" class="admin-nav__item @(If(pageTitle = "Productos", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-box-seam"></i>
                                <span>Productos</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Ordenes", "Admin")" class="admin-nav__item @(If(pageTitle = "Órdenes" Or pageTitle = "Ordenes", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-receipt"></i>
                                <span>Órdenes</span>
                            </div>
                            <span class="admin-nav__badge" id="adminOrdenesBadge">9</span>
                        </a>

                        <a href="@Url.Action("Clientes", "Admin")" class="admin-nav__item @(If(pageTitle = "Clientes", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-people"></i>
                                <span>Clientes</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Reportes", "Admin")" class="admin-nav__item @(If(pageTitle = "Reportes", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-bar-chart"></i>
                                <span>Reportes</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>
                    </nav>

                    <div class="admin-sidebar__section admin-sidebar__section--space">Operativa</div>

                    <nav class="admin-sidebar__nav">
                        <a href="@Url.Action("Inventario", "Admin")" class="admin-nav__item @(If(pageTitle = "Inventario", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-boxes"></i>
                                <span>Inventario</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Proveedores", "Admin")" class="admin-nav__item @(If(pageTitle = "Proveedores", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-truck"></i>
                                <span>Proveedores</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Compras", "Admin")" class="admin-nav__item @(If(pageTitle = "Compras", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-bag"></i>
                                <span>Compras</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Empleados", "Admin")" class="admin-nav__item @(If(pageTitle = "Empleados", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-person-badge"></i>
                                <span>Empleados</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Nomina", "Admin")" class="admin-nav__item @(If(pageTitle = "Nómina" Or pageTitle = "Nomina", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-cash-stack"></i>
                                <span>Nómina</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Marketing", "Admin")" class="admin-nav__item @(If(pageTitle = "Marketing", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-megaphone"></i>
                                <span>Marketing</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Produccion", "Admin")" class="admin-nav__item @(If(pageTitle = "Producción" Or pageTitle = "Produccion", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-hammer"></i>
                                <span>Producción</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Configuracion", "Admin")" class="admin-nav__item @(If(pageTitle = "Configuración" Or pageTitle = "Configuracion", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-gear"></i>
                                <span>Configuración</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>
                    </nav>
                </div>

                <div class="admin-sidebar__footer">
                    <div class="admin-user">
                        <div class="admin-user__avatar">@avatarLetter</div>
                        <div class="admin-user__meta">
                            <div class="admin-user__name">@displayName</div>
                            <div class="admin-user__role">Administrador</div>
                        </div>
                    </div>

                    <a href="@Url.Action("Logout", "Home")" class="admin-logout">
                        <i class="bi bi-box-arrow-left"></i>
                        <span>Cerrar sesión</span>
                    </a>
                </div>
            </div>
        </aside>

        <main class="admin-main">
            <header class="admin-topbar">
                <div class="admin-topbar__title">@pageTitle</div>

                <div class="admin-topbar__actions">
                    <div class="lang-switcher admin-lang-switcher" id="adminLangSwitcher">
                        <button type="button" class="lang-switcher-btn" id="btnAdminLang">
                            <i class="bi bi-translate"></i>
                            <span id="cfgIdiomaTexto">Español</span>
                            <i class="bi bi-chevron-down"></i>
                        </button>

                        <div class="lang-switcher-menu" id="adminLangMenu">
                            <button type="button" data-lang-option="Español">Español</button>
                            <button type="button" data-lang-option="Inglés">English</button>
                        </div>
                    </div>

                    <div class="admin-user-dropdown">
                        <button type="button" class="admin-user-trigger" id="btnAdminUserMenu">
                            <div class="admin-user-trigger__avatar">@avatarLetter</div>
                            <span class="admin-user-trigger__name">@displayName</span>
                            <i class="bi bi-chevron-down admin-user-trigger__icon"></i>
                        </button>

                        <div class="admin-user-menu" id="adminUserMenu">
                            <div class="admin-user-menu__header">
                                <div class="admin-user__avatar">@avatarLetter</div>
                                <div>
                                    <strong>@displayName</strong>
                                    <small>@userHandle</small>
                                </div>
                            </div>

                            <div class="admin-user-menu__divider"></div>

                            <a href="@Url.Action("Perfil", "Admin")" class="admin-user-menu__item">
                                <span><i class="bi bi-person"></i></span>
                                <span>Mi perfil</span>
                            </a>

                            <a href="@Url.Action("Configuracion", "Admin")" class="admin-user-menu__item">
                                <span><i class="bi bi-gear"></i></span>
                                <span>Configuración</span>
                            </a>

                            <div class="admin-user-menu__divider"></div>

                            <a href="@Url.Action("Logout", "Home")" class="admin-user-menu__item admin-user-menu__item--logout">
                                <span><i class="bi bi-box-arrow-left"></i></span>
                                <span>Cerrar sesión</span>
                            </a>
                        </div>
                    </div>
                </div>
            </header>

            <section class="admin-content">
                @RenderBody()
            </section>
        </main>
    </div>

    @Scripts.Render("~/bundles/jquery")

    <script src="@Url.Content("~/Scripts/alpes-security.js")"></script>
    <script src="@Url.Content("~/Scripts/portal-idioma.js?v=21")"></script>

    <script>
        $(function () {
            $.getJSON('/Admin/DashboardData')
                .done(function (res) {
                    if (res && res.success !== false) {
                        $('#adminOrdenesBadge').text(res.ordenesActivas || 0);
                    }
                })
                .fail(function () {
                    console.warn('No se pudo cargar el contador de órdenes.');
                });
        });

        $(document).ready(function () {
            $('#btnAdminUserMenu').on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                $('#adminUserMenu').toggleClass('is-open');
            });

            $('#adminUserMenu').on('click', function (e) {
                e.stopPropagation();
            });

            $(document).on('click', function () {
                $('#adminUserMenu').removeClass('is-open');
            });
        });
    </script>

    @RenderSection("scripts", required:=False)
</body>
</html>