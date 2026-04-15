<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewBag.Title - Muebles de los Alpes</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Lato:wght@400;500;600;700&family=Playfair+Display:wght@600;700&display=swap" rel="stylesheet">
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    <header class="site-header">
        <div class="topbar container-fluid">
            <div class="brand-block">
                <a href="@Url.Action("Index", "Home")" class="brand-link">
                    <div class="brand-logo">A</div>
                    <div class="brand-text">
                        <div class="brand-title">MUEBLES DE LOS ALPES</div>
                        <div class="brand-tagline">ARTESANÍA · CALIDAD · ELEGANCIA</div>
                    </div>
                </a>
            </div>

            <nav class="topbar-nav">
                <a href="@Url.Action("Index", "Home")" class="topbar-link">Inicio</a>
                <a href="#" class="topbar-link">Catálogo</a>
                <a href="#" class="topbar-link">Nosotros</a>
                <a href="#" class="topbar-link">Contacto</a>
                <a href="#" class="btn btn-primary topbar-btn">Ingresar</a>
            </nav>
        </div>
    </header>

    <main class="page-shell">
        <div class="main-content">
            @RenderBody()
        </div>
    </main>

    <footer class="site-footer">
        <div class="container">
            <div class="footer-brand">Muebles de los Alpes</div>
            <div class="footer-copy">© 2026 Muebles de los Alpes. Artesanía guatemalteca con elegancia y calidad.</div>
        </div>
    </footer>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required:=False)
</body>
</html>