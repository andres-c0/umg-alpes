@Code
    ViewData("Title") = "Carrito"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code
<style>
    .cc-confirm-overlay {
        position: fixed;
        inset: 0;
        background: rgba(25, 10, 4, .45);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 9999;
    }

    .cc-confirm-box {
        background: #fff;
        width: 420px;
        max-width: 90%;
        border-radius: 20px;
        padding: 28px;
        box-shadow: 0 24px 60px rgba(0,0,0,.2);
        text-align: center;
    }

    .cc-confirm-icon {
        width: 60px;
        height: 60px;
        margin: 0 auto 16px;
        border-radius: 18px;
        background: #f8e5e5;
        color: #9f3535;
        display: grid;
        place-items: center;
        font-size: 24px;
    }

    .cc-confirm-box h3 {
        margin: 0;
        color: #431406;
        font-size: 24px;
        font-weight: 900;
    }

    .cc-confirm-box p {
        margin: 12px 0 22px;
        color: #7a5c4d;
        font-size: 14px;
    }

    .cc-confirm-actions {
        display: flex;
        gap: 10px;
    }

    .cc-confirm-cancel,
    .cc-confirm-ok {
        flex: 1;
        border: 0;
        border-radius: 12px;
        padding: 13px;
        font-weight: 900;
        cursor: pointer;
    }

    .cc-confirm-cancel {
        background: #f4efea;
        color: #431406;
    }

    .cc-confirm-ok {
        background: #431406;
        color: white;
    }
</style>
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
    <script src="@Url.Content("~/Scripts/portal-carrito.js?v=19")"></script>
End Section