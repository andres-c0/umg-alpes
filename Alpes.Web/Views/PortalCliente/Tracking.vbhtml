@Code
    ViewData("Title") = "Tracking"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim ordenVentaId As Integer = 0
    If ViewData("OrdenVentaId") IsNot Nothing Then
        Integer.TryParse(ViewData("OrdenVentaId").ToString(), ordenVentaId)
    End If
End Code

<div id="trkPage" class="pc-card trk-shell" data-order-id="@ordenVentaId">
    <div class="pc-card-head">
        <h2>Tracking de Orden</h2>
        <a class="pc-pill-link" href="@Url.Action("MisOrdenes", "PortalCliente")">Volver a mis ordenes</a>
    </div>

    <div id="trkContent">
        <div class="trk-loading">
            Cargando informacion de tracking...
        </div>
    </div>
</div>

<script src="~/Scripts/portal-tracking.js"></script>