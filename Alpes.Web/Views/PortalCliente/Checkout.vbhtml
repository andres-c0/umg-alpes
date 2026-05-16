@Code
    ViewData("Title") = "Checkout"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="co-page">
    <div class="co-header co-header--hero">
        <div>
            <div class="co-eyebrow">Finalizar compra</div>
            <h1 class="co-title">Checkout</h1>
            <div class="co-subtitle">Confirma tu direccion de entrega, metodo de pago y resumen del pedido.</div>
        </div>
        <a href="@Url.Action("Carrito", "PortalCliente")" class="co-link-back">Volver al carrito</a>
    </div>

    <div class="co-grid">
        <section class="co-form-card" aria-label="Datos del checkout">
            <div class="co-section">
                <div class="co-section-title">Direccion de entrega</div>
                <textarea id="coDireccion" class="co-textarea" rows="4" placeholder="Escribe la direccion exacta para la entrega"></textarea>
            </div>

            <div class="co-section">
                <div class="co-section-title">Metodo de pago</div>
                <div id="coMetodosPago" class="co-methods"></div>
            </div>

            <div class="co-section">
                <div class="co-section-title">Tarjeta guardada</div>
                <div id="coTarjetas" class="co-cards"></div>
            </div>

            <div class="co-section">
                <div class="co-section-title">Cupon de descuento</div>
                <div class="co-coupon-row">
                    <input type="text" id="coCupon" class="co-input" placeholder="Ingresa tu codigo" />
                    <button type="button" id="coAplicarCuponBtn" class="co-secondary-btn">Aplicar</button>
                </div>
                <div id="coCuponMessage" class="co-coupon-message"></div>
            </div>
        </section>

        <aside id="coSummaryContainer" class="co-summary-card" aria-label="Resumen del pedido">
            <div class="co-loading">Cargando checkout...</div>
        </aside>
    </div>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/fusion-carrito-invitado.js?v=1")"></script>
    <script src="@Url.Content("~/Scripts/portal-checkout.js?v=22")"></script>
End Section
