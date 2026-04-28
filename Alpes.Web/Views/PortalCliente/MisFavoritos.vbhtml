@Code
    ViewData("Title") = "Mis Favoritos"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
End Code

<div class="pf-page" data-cli-id="@cliIdTexto">
    <div class="pf-top">
        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pf-back">
            <i class="bi bi-chevron-left"></i>
        </a>

        <div class="pf-title">MIS FAVORITOS</div>

        <div class="pf-empty"></div>
    </div>

    <div class="pf-content" id="pfFavoritosContainer">
        <div class="pf-card pf-card-empty">
            <div class="pf-info">
                <div class="pf-name">Cargando favoritos...</div>
                <div class="pf-desc">
                    Espera un momento mientras se consulta la base de datos.
                </div>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-favoritos.js?v=2")"></script>
End Section