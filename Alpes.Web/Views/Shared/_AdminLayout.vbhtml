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

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    @Styles.Render("~/Content/alpes.css")
</head>
<body>
    <div class="admin-shell">
        <aside class="admin-sidebar">
            <div>
                <div class="admin-sidebar__brand">
                    <div class="admin-sidebar__logo">
                        <i class="bi bi-house-door-fill"></i>
                    </div>
                    <div>
                        <div class="admin-sidebar__title">Muebles de los Alpes</div>
                        <div class="admin-sidebar__subtitle">Panel Administrativo</div>
                    </div>
                </div>

                <nav class="admin-sidebar__nav">
                    <a href="@Url.Action("Index", "Admin")" class="admin-nav__item">
                        <i class="bi bi-speedometer2"></i>
                        <span>Dashboard</span>
                    </a>

                    <a href="@Url.Action("Productos", "Admin")" class="admin-nav__item">
                        <i class="bi bi-box-seam"></i>
                        <span>Productos</span>
                    </a>

                    <a href="@Url.Action("Ordenes", "Admin")" class="admin-nav__item">
                        <i class="bi bi-receipt"></i>
                        <span>Órdenes</span>
                    </a>

                    <a href="@Url.Action("Clientes", "Admin")" class="admin-nav__item">
                        <i class="bi bi-people"></i>
                        <span>Clientes</span>
                    </a>

                    <a href="@Url.Action("Inventario", "Admin")" class="admin-nav__item">
    <i class="bi bi-boxes"></i>
    <span>Inventario</span>
</a>

                    <a href="@Url.Action("Reportes", "Admin")" class="admin-nav__item">
                        <i class="bi bi-bar-chart"></i>
                        <span>Reportes</span>
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