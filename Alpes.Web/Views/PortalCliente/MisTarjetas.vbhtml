@Code
    ViewData("Title") = "Mis Tarjetas"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
End Code

<div class="pt-page" data-cli-id="@cliIdTexto">
    <div class="pt-top">
        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pt-back">
            <i class="bi bi-chevron-left"></i>
        </a>

        <div class="pt-title">MIS TARJETAS</div>

        <div class="pt-empty"></div>
    </div>

    <div class="pt-content">
        <div class="pt-cards-list" id="ptCardsList">
            <div class="pt-card-wrapper">
                <div class="pt-card pt-card-empty">
                    <div class="pt-number">Cargando tarjetas...</div>
                    <div class="pt-holder">Espera un momento mientras se consulta la base de datos.</div>
                </div>
            </div>
        </div>
    </div>

    <button type="button" class="pt-add-btn" id="btnAbrirAgregarTarjeta">
        <i class="bi bi-plus-lg"></i>
    </button>

    <div class="pt-overlay" id="ptOverlay">
        <div class="pt-modal">
            <div class="pt-modal-handle"></div>
            <div class="pt-modal-title">Nueva tarjeta</div>

            <form id="ptForm">
                <div class="pt-field">
                    <label>Numero de tarjeta</label>
                    <input type="text" id="ptNumero" placeholder="0000 0000 0000 0000" />
                </div>

                <div class="pt-field">
                    <label>Nombre del titular</label>
                    <input type="text" id="ptTitular" placeholder="Nombre del titular" />
                </div>

                <div class="pt-field">
                    <label>Marca</label>
                    <select id="ptMarca" class="pt-select">
                        <option value="VISA">VISA</option>
                        <option value="MASTERCARD">MASTERCARD</option>
                        <option value="AMEX">AMEX</option>
                        <option value="TARJETA">OTRA</option>
                    </select>
                </div>

                <div class="pt-grid">
                    <div class="pt-field">
                        <label>Mes</label>
                        <input type="text" id="ptMes" placeholder="MM" />
                    </div>

                    <div class="pt-field">
                        <label>Anio</label>
                        <input type="text" id="ptAnio" placeholder="AAAA" />
                    </div>
                </div>

                <div class="pt-field">
                    <label>Alias (opcional)</label>
                    <input type="text" id="ptAlias" placeholder="Ej. Tarjeta personal" />
                </div>

                <div class="pt-switch-row">
                    <span>Marcar como predeterminada</span>
                    <label class="pt-switch">
                        <input type="checkbox" id="ptPredeterminada" />
                        <span class="pt-slider"></span>
                    </label>
                </div>

                <button type="submit" class="pt-save">GUARDAR TARJETA</button>
            </form>

            <button type="button" class="pt-close" id="btnCerrarAgregarTarjeta">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-tarjetas.js?v=3")"></script>
End Section