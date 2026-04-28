@Code
    ViewData("Title") = "Carrito"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="cc-page">
    <div class="cc-header">
        <div>
            <h1 class="cc-title">Carrito</h1>
            <div class="cc-subtitle">Revisa tus productos antes de pasar al checkout.</div>
        </div>
        <a href="@Url.Action("Index", "PortalCliente")" class="cc-link-back">Seguir comprando</a>
    </div>

    <div class="cc-grid">
        <div id="ccItemsContainer" class="cc-items-card">
            <div class="cc-loading">Cargando carrito...</div>
        </div>

        <div id="ccSummaryContainer" class="cc-summary-card">
            <div class="cc-loading">Calculando resumen...</div>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-carrito.js?v=1")"></script>
End Section