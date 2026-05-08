@Code
    ViewData("Title") = "Búsqueda"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="pb-page">
    <section class="pb-hero">
        <div>
            <div class="ci-badge">Catálogo avanzado</div>
            <h1 class="pb-title">Encuentra el mueble ideal</h1>
            <p class="pb-subtitle">Busca por nombre, referencia, tipo, color, material o categoría. Todo se carga desde la base de datos.</p>
        </div>
        <a href="@Url.Action("Carrito", "PortalCliente")" class="ci-hero-btn ci-hero-btn--light">
            <i class="bi bi-cart3"></i>
            Ver carrito
        </a>
    </section>

    <section class="pb-search-panel">
        <div class="pb-search-main">
            <i class="bi bi-search"></i>
            <input type="text" id="pbBuscar" placeholder="Buscar muebles, referencias, materiales o colores..." autocomplete="off" />
            <button type="button" id="pbBtnBuscar">Buscar</button>
        </div>

        <div class="pb-filters">
            <label>
                <span>Categoría</span>
                <select id="pbFiltroCategoria">
                    <option value="">Todas</option>
                </select>
            </label>

            <label>
                <span>Tipo</span>
                <select id="pbFiltroTipo">
                    <option value="">Todos</option>
                </select>
            </label>

            <label>
                <span>Material</span>
                <select id="pbFiltroMaterial">
                    <option value="">Todos</option>
                </select>
            </label>

            <label>
                <span>Color</span>
                <select id="pbFiltroColor">
                    <option value="">Todos</option>
                </select>
            </label>

            <label>
                <span>Ordenar por</span>
                <select id="pbOrden">
                    <option value="relevancia">Relevancia</option>
                    <option value="nombre_asc">Nombre A-Z</option>
                    <option value="precio_asc">Precio menor a mayor</option>
                    <option value="precio_desc">Precio mayor a menor</option>
                    <option value="stock_desc">Mayor disponibilidad</option>
                </select>
            </label>
        </div>

        <div class="pb-actions-row">
            <div id="pbResumen" class="pb-summary">Cargando productos...</div>
            <button type="button" id="pbBtnLimpiar" class="ci-secondary-btn">Limpiar filtros</button>
        </div>
    </section>

    <section class="pb-content-grid">
        <aside class="pb-side-card">
            <h3>Filtros activos</h3>
            <div id="pbChips" class="pb-chips">
                <span class="pb-chip pb-chip-empty">Sin filtros aplicados</span>
            </div>

            <div class="pb-help-card">
                <i class="bi bi-lightbulb"></i>
                <div>
                    <strong>Consejo</strong>
                    <p>Prueba con términos como sala, comedor, madera, nogal o el color que buscas.</p>
                </div>
            </div>
        </aside>

        <div>
            <div id="pbResultados" class="pi-grid pb-results-grid">
                <div class="pi-empty-card">Cargando catálogo...</div>
            </div>
        </div>
    </section>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-busqueda.js?v=16")"></script>
End Section
