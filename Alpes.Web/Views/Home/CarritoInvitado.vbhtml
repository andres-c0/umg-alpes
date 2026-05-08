@Code
    ViewData("Title") = "Carrito"
    Layout = "~/Views/Shared/_PublicLayout.vbhtml"
End Code

<section class="carrito-invitado-page" id="carritoInvitadoPage">
    <div class="carrito-invitado-header">
        <a href="@Url.Action("Index", "Home")#productos-destacados" class="producto-publico-back">
            <i class="bi bi-arrow-left"></i>
            Continuar comprando
        </a>

        <span>COMPRA COMO INVITADO</span>
        <h1>Tu carrito</h1>
        <p>
            Puedes agregar productos sin iniciar sesión. Solo te pediremos ingresar o crear cuenta cuando quieras finalizar la compra.
        </p>
    </div>

    <div class="carrito-invitado-empty" id="carritoInvitadoEmpty" style="display:none;">
        <i class="bi bi-cart-x"></i>
        <h2>Tu carrito está vacío</h2>
        <p>Explora la colección y agrega tus muebles favoritos.</p>

        <a href="@Url.Action("Index", "Home")#productos-destacados" class="home-btn home-btn-gold">
            Ver productos
            <i class="bi bi-arrow-right"></i>
        </a>
    </div>

    <div class="carrito-invitado-layout" id="carritoInvitadoContenido" style="display:none;">
        <div class="carrito-invitado-lista">
            <div class="carrito-invitado-toolbar">
                <div>
                    <strong id="carritoInvitadoCantidadTexto">0 productos</strong>
                    <span>Guardados localmente en este navegador</span>
                </div>

                <button type="button" id="btnVaciarCarritoInvitado">
                    <i class="bi bi-trash3"></i>
                    Vaciar carrito
                </button>
            </div>

            <div id="carritoInvitadoItems"></div>
        </div>

        <aside class="carrito-invitado-resumen">
            <div class="carrito-resumen-card">
                <span>RESUMEN DE COMPRA</span>
                <h2>Pedido estimado</h2>

                <div class="carrito-resumen-linea">
                    <span>Subtotal</span>
                    <strong id="carritoSubtotal">Q 0.00</strong>
                </div>

                <div class="carrito-resumen-linea">
                    <span>Envío</span>
                    <strong>Por calcular</strong>
                </div>

                <div class="carrito-resumen-linea">
                    <span>Impuesto</span>
                    <strong>Por calcular</strong>
                </div>

                <div class="carrito-resumen-total">
                    <span>Total estimado</span>
                    <strong id="carritoTotal">Q 0.00</strong>
                </div>

                <button type="button" class="home-btn home-btn-gold carrito-checkout-btn" id="btnFinalizarCompraInvitado">
                    Finalizar compra
                    <i class="bi bi-arrow-right"></i>
                </button>

                <p class="carrito-resumen-nota">
                    Para confirmar el pedido deberás iniciar sesión o crear una cuenta como cliente.
                </p>
            </div>

            <div class="carrito-beneficios-card">
                <div>
                    <i class="bi bi-shield-check"></i>
                    <span>Compra segura</span>
                </div>

                <div>
                    <i class="bi bi-truck"></i>
                    <span>Envío coordinado</span>
                </div>

                <div>
                    <i class="bi bi-award"></i>
                    <span>Calidad garantizada</span>
                </div>
            </div>
        </aside>
    </div>
</section>

@section scripts
    <script src="@Url.Content("~/Scripts/carrito-invitado.js?v=1")"></script>
End Section