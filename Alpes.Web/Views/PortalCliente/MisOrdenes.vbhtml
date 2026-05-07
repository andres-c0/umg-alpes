@Code
    ViewData("Title") = "Mis pedidos"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="po-page po-page--desktop">
    <section class="orders-hero">
        <div>
            <span class="orders-kicker">Historial de compras</span>
            <h1>Mis pedidos</h1>
            <p>Consulta tus órdenes, revisa el detalle de cada compra y da seguimiento a tus entregas.</p>
        </div>
        <a class="btn-gold orders-hero-action" href="@Url.Action("Index", "PortalCliente")#catalogo">
            <i class="bi bi-grid"></i>
            Ver catálogo
        </a>
    </section>

    <section class="orders-stats-grid" aria-label="Resumen de pedidos">
        <article class="orders-stat-card">
            <div class="orders-stat-icon"><i class="bi bi-receipt"></i></div>
            <div>
                <strong id="poCountTodasBig">0</strong>
                <span>Total de pedidos</span>
            </div>
        </article>
        <article class="orders-stat-card">
            <div class="orders-stat-icon"><i class="bi bi-truck"></i></div>
            <div>
                <strong id="poCountActivasBig">0</strong>
                <span>Pedidos activos</span>
            </div>
        </article>
        <article class="orders-stat-card">
            <div class="orders-stat-icon"><i class="bi bi-check-circle"></i></div>
            <div>
                <strong id="poCountEntregadasBig">0</strong>
                <span>Entregados</span>
            </div>
        </article>
        <article class="orders-stat-card">
            <div class="orders-stat-icon"><i class="bi bi-cash-stack"></i></div>
            <div>
                <strong id="poTotalComprado">Q0.00</strong>
                <span>Total comprado</span>
            </div>
        </article>
    </section>

    <section class="pc-card po-shell">
        <div class="pc-card-head orders-card-head">
            <div>
                <h2>Órdenes registradas</h2>
                <p>Toda la información se consulta desde la base de datos.</p>
            </div>
            <button type="button" class="po-refresh-btn" id="poRefreshBtn">
                <i class="bi bi-arrow-clockwise"></i>
                Actualizar
            </button>
        </div>

        <div class="po-tabs" id="poTabs">
            <button type="button" class="po-tab active" data-filter="TODAS">Todas <span id="poCountTodas">0</span></button>
            <button type="button" class="po-tab" data-filter="ACTIVAS">Activas <span id="poCountActivas">0</span></button>
            <button type="button" class="po-tab" data-filter="ENTREGADAS">Entregadas <span id="poCountEntregadas">0</span></button>
            <button type="button" class="po-tab" data-filter="CANCELADAS">Canceladas <span id="poCountCanceladas">0</span></button>
        </div>

        <div id="poOrdersGrid" class="po-grid"></div>

        <div id="poEmptyState" class="po-empty-state" style="display:none;">
            <div class="po-empty-icon"><i class="bi bi-box-seam"></i></div>
            <div class="po-empty-title">No hay pedidos para mostrar</div>
            <div class="po-empty-text">Cuando confirmes una compra, aparecerá aquí junto con su estado y seguimiento.</div>
            <a class="po-empty-link" href="@Url.Action("Index", "PortalCliente")#catalogo">Ir al catálogo</a>
        </div>
    </section>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-mis-ordenes.js?v=16")"></script>
End Section
