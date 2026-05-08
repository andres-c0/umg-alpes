@Code
    ViewData("Title") = "Seguimiento"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim ordenVentaId As Integer = 0
    If ViewData("OrdenVentaId") IsNot Nothing Then
        Integer.TryParse(ViewData("OrdenVentaId").ToString(), ordenVentaId)
    End If
End Code

<div id="trkPage" class="trk-page-new" data-order-id="@ordenVentaId">
    <section class="trk-hero-new">
        <div>
            <span class="orders-kicker">Seguimiento de entrega</span>
            <h1>Tracking del pedido</h1>
            <p>Consulta la línea de tiempo de tu envío y el estado actual de la entrega.</p>
        </div>
        <a class="od-hero-link" href="@Url.Action("MisOrdenes", "PortalCliente")">
            <i class="fa-solid fa-arrow-left"></i>
            Volver a mis pedidos
        </a>
    </section>

    <div id="trkContent">
        <div class="trk-loading">
            <i class="fa-solid fa-spinner fa-spin"></i>
            Cargando información de tracking...
        </div>
    </div>
</div>

<script src="~/Scripts/portal-tracking.js"></script>
