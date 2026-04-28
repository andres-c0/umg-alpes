@Code
    ViewData("Title") = "Inicio"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim username As String = System.Convert.ToString(ViewData("Username"))
    If String.IsNullOrWhiteSpace(username) Then
        username = "Cliente"
    End If

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
End Code

<div class="ci-page" data-cli-id="@cliIdTexto">
    <div class="ci-hero">
        <div class="ci-hero-text">
            <div class="ci-badge">Portal cliente</div>
            <h1 class="ci-title">Bienvenido, @username</h1>
            <div class="ci-hero-subtitle">
                Explora productos, guarda tus favoritos y revisa recomendaciones basadas en tus intereses.
            </div>
        </div>

        <div class="ci-hero-actions">
            <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="ci-hero-btn ci-hero-btn--light">
                <i class="bi bi-heart"></i>
                Mis favoritos
            </a>
            <a href="@Url.Action("MiPerfil", "PortalCliente")" class="ci-hero-btn">
                <i class="bi bi-person-circle"></i>
                Mi perfil
            </a>
        </div>
    </div>

    <div class="ci-section">
        <div class="ci-section-title">Accesos rápidos</div>

        <div class="ci-quick-grid">
            <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="ci-quick-card">
                <i class="bi bi-heart-fill"></i>
                <span>Favoritos</span>
            </a>

            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="ci-quick-card">
                <i class="bi bi-receipt"></i>
                <span>Mis pedidos</span>
            </a>

            <a href="@Url.Action("MisTarjetas", "PortalCliente")" class="ci-quick-card">
                <i class="bi bi-credit-card-2-front"></i>
                <span>Tarjetas</span>
            </a>

            <a href="@Url.Action("Tracking", "PortalCliente")" class="ci-quick-card">
                <i class="bi bi-truck"></i>
                <span>Tracking</span>
            </a>

            <a href="@Url.Action("Soporte", "PortalCliente")" class="ci-quick-card">
                <i class="bi bi-headset"></i>
                <span>Soporte</span>
            </a>

            <a href="@Url.Action("Configuracion", "PortalCliente")" class="ci-quick-card">
                <i class="bi bi-gear"></i>
                <span>Configuracion</span>
            </a>
        </div>
    </div>

    <div class="ci-section">
        <div class="ci-section-title">Recomendados para ti</div>
        <div class="ci-section-subtitle">Productos sugeridos segun tus favoritos actuales.</div>

        <div id="piRecomendadosContainer" class="pi-grid">
            <div class="pi-empty-card">Cargando recomendaciones...</div>
        </div>
    </div>

    <div class="ci-section">
        <div class="ci-section-title">Catalogo</div>
        <div class="ci-section-subtitle">Consulta los productos disponibles desde la base de datos.</div>

        <div class="ci-search-card">
            <div class="ci-search-row">
                <input type="text" id="ciBuscarProducto" class="ci-input" placeholder="Buscar por nombre, referencia, tipo, material o color" />
                <button type="button" id="ciBtnBuscar" class="ci-primary-btn">
                    <i class="bi bi-search"></i>
                    Buscar
                </button>
            </div>

            <div class="ci-filters-grid">
                <select id="ciFiltroCategoria" class="ci-select">
                    <option value="">Todas las categorias</option>
                </select>

                <select id="ciFiltroTipo" class="ci-select">
                    <option value="">Todos los tipos</option>
                </select>

                <select id="ciFiltroColor" class="ci-select">
                    <option value="">Todos los colores</option>
                </select>

                <select id="ciFiltroMaterial" class="ci-select">
                    <option value="">Todos los materiales</option>
                </select>
            </div>

            <div class="ci-filter-actions">
                <button type="button" id="ciBtnLimpiar" class="ci-secondary-btn">
                    Limpiar filtros
                </button>
            </div>
        </div>

        <div id="ciCatalogoContainer" class="pi-grid">
            <div class="pi-empty-card">Cargando catalogo...</div>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-inicio.js?v=7")"></script>
End Section