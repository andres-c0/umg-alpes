@Code
    ViewData("Title") = "Checkout"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<div class="co-page">
    <div class="co-header">
        <div>
            <h1 class="co-title">Checkout</h1>
            <div class="co-subtitle">Confirma tu direccion, metodo de pago y resumen del pedido.</div>
        </div>
        <a href="@Url.Action("Carrito", "PortalCliente")" class="co-link-back">Volver al carrito</a>
    </div>

    <div class="co-grid">
        <div class="co-form-card">
            <div class="co-section-title">Direccion de entrega</div>
            <textarea id="coDireccion" class="co-textarea" rows="4" placeholder="Escribe la direccion de entrega"></textarea>

            <div class="co-section-title">Metodo de pago</div>
            <div id="coMetodosPago" class="co-methods"></div>

            <div class="co-section-title">Tarjeta guardada</div>
            <div id="coTarjetas" class="co-cards"></div>

            <div class="co-section-title">Cupon</div>
            <div class="co-coupon-row">
                <input type="text" id="coCupon" class="co-input" placeholder="Ingresa tu codigo" />
                <button type="button" id="coAplicarCuponBtn" class="co-secondary-btn">Aplicar</button>
            </div>

            <div id="coCuponMessage" class="co-coupon-message"></div>
        </div>

        <div id="coSummaryContainer" class="co-summary-card">
            <div class="co-loading">Cargando checkout...</div>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-checkout.js?v=1")"></script>
End Section