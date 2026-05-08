@Code
    ViewData("Title") = "Detalle de pedido"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim ordenVentaId As Integer = 0
    If ViewData("OrdenVentaId") IsNot Nothing Then
        Integer.TryParse(ViewData("OrdenVentaId").ToString(), ordenVentaId)
    End If
End Code

<div id="odPage" class="od-page" data-order-id="@ordenVentaId">
    <section class="od-hero">
        <div>
            <span class="orders-kicker">Detalle de compra</span>
            <h1>Detalle de pedido</h1>
            <p>Revisa productos, totales, dirección de entrega y estado actual de la orden.</p>
        </div>
        <a class="od-hero-link" href="@Url.Action("MisOrdenes", "PortalCliente")">
            <i class="fa-solid fa-arrow-left"></i>
            Volver a mis pedidos
        </a>
    </section>

    <div id="odContent">
        <div class="od-loading">
            <i class="fa-solid fa-spinner fa-spin"></i>
            Cargando detalle del pedido...
        </div>
    </div>
</div>

<script src="~/Scripts/portal-detalle-orden.js"></script>
