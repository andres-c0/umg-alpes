@Code
    ViewData("Title") = "Mis tarjetas"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<section class="pc-section pc-cards-page" id="tarjetasClientePage">
    <div class="pc-hero pc-cards-hero">
        <div>
            <span class="pc-eyebrow">Pagos guardados</span>
            <h1>Mis tarjetas</h1>
            <p>Consulta, registra y administra tus tarjetas para comprar más rápido.</p>
        </div>
        <button type="button" class="pc-btn pc-btn-gold" id="btnAbrirTarjeta">
            <i class="bi bi-plus-lg"></i>
            Agregar tarjeta
        </button>
    </div>

    <div class="pc-card">
        <div class="pc-card-header">
            <div>
                <span class="pc-eyebrow">Desde base de datos</span>
                <h2>Tarjetas registradas</h2>
            </div>
            <button type="button" class="pc-btn pc-btn-outline pc-btn-sm" id="btnActualizarTarjetas">
                Actualizar
            </button>
        </div>

        <div class="pc-payment-grid" id="tarjetasLista">
            <div class="pc-loading-card">Cargando tarjetas...</div>
        </div>
    </div>
</section>

<div class="pc-modal-backdrop" id="tarjetaModal">
    <div class="pc-modal pc-card-modal">
        <button type="button" class="pc-modal-close" id="btnCerrarTarjeta" aria-label="Cerrar">
            <i class="bi bi-x-lg"></i>
        </button>
        <span class="pc-eyebrow">Nueva tarjeta</span>
        <h2>Registrar método de pago</h2>

        <form id="tarjetaForm" class="pc-form">
            <div class="pc-form-grid">
                <label class="pc-form-wide">
                    <span>Titular</span>
                    <input type="text" id="tarjetaTitular" required placeholder="Nombre como aparece en la tarjeta" />
                </label>
                <label class="pc-form-wide">
                    <span>Número</span>
                    <input type="text" id="tarjetaNumero" required maxlength="19" placeholder="0000 0000 0000 0000" />
                </label>
                <label>
                    <span>Marca</span>
                    <select id="tarjetaMarca">
                        <option value="VISA">VISA</option>
                        <option value="MASTERCARD">MASTERCARD</option>
                        <option value="AMEX">AMEX</option>
                        <option value="OTRA">OTRA</option>
                    </select>
                </label>
                <label>
                    <span>Mes</span>
                    <input type="number" id="tarjetaMes" min="1" max="12" required placeholder="12" />
                </label>
                <label>
                    <span>Año</span>
                    <input type="number" id="tarjetaAnio" min="2026" max="2050" required placeholder="2029" />
                </label>
                <label>
                    <span>Alias</span>
                    <input type="text" id="tarjetaAlias" placeholder="Principal" />
                </label>
                <label class="pc-check-line pc-form-wide">
                    <input type="checkbox" id="tarjetaPredeterminada" />
                    <span>Usar como tarjeta predeterminada</span>
                </label>
            </div>

            <button type="submit" class="pc-btn pc-btn-primary pc-btn-full" id="btnGuardarTarjeta">
                Registrar tarjeta
            </button>
        </form>
    </div>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-tarjetas.js?v=16")"></script>
End Section
