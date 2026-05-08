@Code
    ViewData("Title") = "Mis Favoritos"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
End Code

<div class="fav-page" data-cli-id="@cliIdTexto">
    <section class="fav-hero">
        <div>
            <span class="fav-kicker">Colección personal</span>
            <h1>Mis favoritos</h1>
            <p>Guarda los muebles que más te gustan y vuelve a ellos cuando quieras comprar.</p>
        </div>
        <a href="@Url.Action("Index", "PortalCliente")#catalogo" class="fav-hero-btn">
            <i class="bi bi-grid"></i>
            Ver catálogo
        </a>
    </section>

    <section class="fav-panel">
        <div class="fav-panel-head">
            <div>
                <h2>Productos guardados</h2>
                <p>Información consultada desde la base de datos.</p>
            </div>
        </div>

        <div class="fav-grid" id="pfFavoritosContainer">
            <div class="fav-empty-card">
                <div class="fav-empty-icon"><i class="bi bi-heart"></i></div>
                <strong>Cargando favoritos...</strong>
                <span>Espera un momento mientras se consulta la base de datos.</span>
            </div>
        </div>
    </section>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-favoritos.js?v=16")"></script>
End Section
