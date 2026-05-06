@Code
    ViewData("Title") = "Carrito"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="cc-page">
    <div class="cc-header cc-header--hero">
        <div>
            <div class="cc-eyebrow">Muebles de los Alpes</div>
            <h1 class="cc-title">Carrito de compra</h1>
            <div class="cc-subtitle">Revisa cantidades, precios y subtotales antes de confirmar tu pedido.</div>
        </div>
        <div class="cc-header-actions">
            <a href="@Url.Action("Index", "PortalCliente")" class="cc-link-back">Seguir comprando</a>
        </div>
    </div>

    <div class="cc-grid">
        <section class="cc-items-card" aria-label="Productos en carrito">
            <div class="cc-card-heading">
                <div>
                    <h2>Productos seleccionados</h2>
                    <p>Modifica cantidades o elimina productos.</p>
                </div>
            </div>
            <div id="ccItemsContainer">
                <div class="cc-loading">Cargando carrito...</div>
            </div>
        </section>

        <aside id="ccSummaryContainer" class="cc-summary-card" aria-label="Resumen de compra">
            <div class="cc-loading">Calculando resumen...</div>
        </aside>
    </div>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-carrito.js?v=16")"></script>
End Section
