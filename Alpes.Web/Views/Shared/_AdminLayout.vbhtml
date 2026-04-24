@Code
    Dim username As String = ""
    Dim avatarLetter As String = "A"
    Dim displayName As String = "Administrador"

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
    <link rel="stylesheet" href="~/Content/alpes.css?v=10" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
  
</head>
<body>
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
                        <a href="@Url.Action("Index", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Dashboard", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-speedometer2"></i>
                                <span>Dashboard</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Productos", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Productos", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-box-seam"></i>
                                <span>Productos</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Ordenes", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Órdenes", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-receipt"></i>
                                <span>Órdenes</span>
                            </div>
                            <span class="admin-nav__badge">9</span>
                        </a>

                        <a href="@Url.Action("Clientes", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Clientes", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-people"></i>
                                <span>Clientes</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Reportes", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Reportes", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-bar-chart"></i>
                                <span>Reportes</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>
                    </nav>

                    <div class="admin-sidebar__section admin-sidebar__section--space">Operativa</div>
                    <nav class="admin-sidebar__nav">
                        <a href="@Url.Action("Inventario", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Inventario", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-boxes"></i>
                                <span>Inventario</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Proveedores", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Proveedores", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-truck"></i>
                                <span>Proveedores</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Compras", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Compras", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-bag"></i>
                                <span>Compras</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Empleados", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Empleados", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-person-badge"></i>
                                <span>Empleados</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Nomina", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Nómina", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-cash-stack"></i>
                                <span>Nómina</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>
                        <a href="@Url.Action("Marketing", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Marketing", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-megaphone"></i>
                                <span>Marketing</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                        <a href="@Url.Action("Produccion", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Producción", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-hammer"></i>
                                <span>Producción</span>
                            </div>
                            <i class="bi bi-chevron-right admin-nav__arrow"></i>
                        </a>

                     

                        <a href="@Url.Action("Configuracion", "Admin")" class="admin-nav__item @(If(ViewData("Title")?.ToString() = "Configuración", "active", ""))">
                            <div class="admin-nav__left">
                                <i class="bi bi-gear"></i>
                                <span>Config.</span>
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
                <div class="admin-topbar__title">@ViewData("Title")</div>
            </header>

            <section class="admin-content">
                @RenderBody()
            </section>
        </main>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @RenderSection("scripts", required:=False)
</body>
</html>