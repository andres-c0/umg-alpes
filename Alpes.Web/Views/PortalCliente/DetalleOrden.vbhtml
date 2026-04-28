@Code
    ViewData("Title") = "Detalle de Orden"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim ordenVentaId As Integer = 0
    If ViewData("OrdenVentaId") IsNot Nothing Then
        Integer.TryParse(ViewData("OrdenVentaId").ToString(), ordenVentaId)
    End If
End Code

<div id="odPage" class="pc-card od-shell" data-order-id="@ordenVentaId">
    <div class="pc-card-head">
        <h2>Detalle de Orden</h2>
        <a class="pc-pill-link" href="@Url.Action("MisOrdenes", "PortalCliente")">Volver a mis ordenes</a>
    </div>

    <div id="odContent">
        <div class="od-loading">
            Cargando detalle de la orden...
        </div>
    </div>
</div>

<script src="~/Scripts/portal-detalle-orden.js"></script>