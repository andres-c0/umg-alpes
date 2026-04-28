@Code
    ViewData("Title") = "Mis Ordenes"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="pc-card po-shell">
    <div class="pc-card-head">
        <h2>Mis Ordenes</h2>
        <a class="pc-pill-link" href="@Url.Action("Index", "PortalCliente")">Volver al inicio</a>
    </div>

    <div class="po-tabs" id="poTabs">
        <button type="button" class="po-tab active" data-filter="TODAS">
            Todas
            <span id="poCountTodas">0</span>
        </button>
        <button type="button" class="po-tab" data-filter="ACTIVAS">
            Activas
            <span id="poCountActivas">0</span>
        </button>
        <button type="button" class="po-tab" data-filter="ENTREGADAS">
            Entregadas
            <span id="poCountEntregadas">0</span>
        </button>
        <button type="button" class="po-tab" data-filter="CANCELADAS">
            Canceladas
            <span id="poCountCanceladas">0</span>
        </button>
    </div>

    <div class="po-toolbar">
        <div class="po-toolbar-text">
            Consulta tus pedidos registrados desde base de datos.
        </div>
        <button type="button" class="po-refresh-btn" id="poRefreshBtn">Actualizar</button>
    </div>

    <div id="poOrdersGrid" class="po-grid"></div>

    <div id="poEmptyState" class="po-empty-state" style="display:none;">
        <div class="po-empty-icon">
            <i class="fa-solid fa-box-open"></i>
        </div>
        <div class="po-empty-title">No hay ordenes para mostrar</div>
        <div class="po-empty-text">
            Cuando tengas compras registradas, apareceran aqui.
        </div>
        <a class="po-empty-link" href="@Url.Action("Index", "PortalCliente")">Ir al inicio</a>
    </div>
</div>

<script src="~/Scripts/portal-mis-ordenes.js"></script>