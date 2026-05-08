@Code
    ViewData("Title") = "Carrito"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="cc-page cc-page-new">

    <div class="cc-top-link">
        <a href="@Url.Action("Busqueda", "PortalCliente")">
            ← Continuar comprando
        </a>
    </div>

    <div class="cc-header cc-header--hero">
        <div>
            <h1 class="cc-title">Tu carrito</h1>
            <div class="cc-subtitle">
                Puedes agregar productos sin iniciar sesión. Solo te pediremos ingresar o crear cuenta cuando quieras finalizar la compra.
            </div>
        </div>
    </div>

    <div class="cc-grid cc-grid-new">
        <section class="cc-items-card" aria-label="Productos en carrito">
            <div class="cc-card-heading">
                <div>
                    <strong id="ccProductCount">Productos</strong>
                    <p>Guardados localmente en este navegador</p>
                </div>

                <button type="button" class="cc-empty-cart-btn" id="ccBtnVaciarCarrito">
                    <i class="bi bi-trash"></i>
                    Vaciar carrito
                </button>
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
    <script src="@Url.Content("~/Scripts/portal-carrito.js?v=17")"></script>
End Section