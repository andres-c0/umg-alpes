@Code
    ViewData("Title") = "Búsqueda"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code
<style>
    .catalog-tabs {
        height: 54px;
        background: #170900;
        display: flex;
        align-items: end;
        gap: 34px;
        padding: 0 18px;
        margin: -16px -20px 10px -20px;
    }

    .catalog-tab {
        color: #8d7b6b;
        text-decoration: none;
        font-size: 13px;
        font-weight: 900;
        padding-bottom: 14px;
        border-bottom: 4px solid transparent;
    }

        .catalog-tab.active {
            color: #fff;
            border-bottom-color: #d6a354;
        }

        .catalog-tab small {
            background: rgba(255,255,255,.12);
            padding: 2px 6px;
            border-radius: 8px;
            margin-left: 4px;
            color: #c7aa70;
        }

    .catalog-bottom-nav {
        position: fixed;
        left: 164px;
        right: 0;
        bottom: 0;
        height: 64px;
        background: white;
        display: grid;
        grid-template-columns: repeat(5, 1fr);
        border-top: 1px solid #eee4d8;
        z-index: 99;
    }

        .catalog-bottom-nav a {
            display: grid;
            place-items: center;
            align-content: center;
            gap: 3px;
            color: #b39a6b;
            text-decoration: none;
            font-size: 11px;
            font-weight: 700;
        }

            .catalog-bottom-nav a i {
                font-size: 18px;
            }

            .catalog-bottom-nav a.active {
                color: #321407;
            }

    .pb-page {
        padding-bottom: 85px;
    }
</style>

<div class="pb-page">
    <div class="catalog-tabs">
        <a href="#" class="catalog-tab active" data-tab="todos">
            Todos <small id="catCountTodos">0</small>
        </a>

        <a href="#" class="catalog-tab" data-tab="exterior">
            Exterior <small id="catCountExterior">0</small>
        </a>

        <a href="#" class="catalog-tab" data-tab="interior">
            Interior <small id="catCountInterior">0</small>
        </a>
        <nav class="catalog-bottom-nav">
            <a href="@Url.Action("Index", "PortalCliente")">
                <i class="bi bi-house"></i>
                <span>Inicio</span>
            </a>

            <a class="active" href="@Url.Action("Busqueda", "PortalCliente")">
                <i class="bi bi-grid"></i>
                <span>Catálogo</span>
            </a>

            <a href="@Url.Action("MisFavoritos", "PortalCliente")">
                <i class="bi bi-heart"></i>
                <span>Favoritos</span>
            </a>

            <a href="@Url.Action("MisOrdenes", "PortalCliente")">
                <i class="bi bi-receipt"></i>
                <span>Órdenes</span>
            </a>

            <a href="@Url.Action("MiPerfil", "PortalCliente")">
                <i class="bi bi-person"></i>
                <span>Perfil</span>
            </a>
        </nav>
    </div>
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
    <script src="@Url.Content("~/Scripts/portal-busqueda.js?v=25")"></script>
End Section
