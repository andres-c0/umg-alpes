@Code
    ViewData("Title") = "Detalle del producto"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"

    Dim productoIdTexto As String = System.Convert.ToString(ViewData("ProductoId"))
End Code

<div class="pd-page" data-producto-id="@productoIdTexto">
    <div class="pd-topbar">
        <a href="@Url.Action("Index", "PortalCliente")" class="pd-back">
            <i class="bi bi-chevron-left"></i>
            Volver
        </a>
    </div>

    <div id="pdDetalleContainer" class="pd-detail-card">
        <div class="pd-loading">Cargando detalle del producto...</div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-detalle-producto.js?v=1")"></script>
End Section