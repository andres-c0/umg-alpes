@Code
    ViewData("Title") = "Configuración"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<style>
    .pc-hero h1 {
        color: #ffffff !important;
        -webkit-text-fill-color: #ffffff !important;
    }

    .pc-hero p {
        color: #ffffff !important;
        -webkit-text-fill-color: #ffffff !important;
    }

    .pc-hero .pc-eyebrow {
        color: #431406 !important;
    }
</style>

<section class="pc-section pc-config-page" id="configuracionCliente">
    <div class="pc-hero pc-hero-compact">
        <div>
            <span class="pc-eyebrow">Cuenta</span>
            <h1>Configuración</h1>
            <p>Administra preferencias visuales, notificaciones y accesos de privacidad del panel cliente.</p>
        </div>

        <div class="pc-hero-actions">
            <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pc-btn pc-btn-gold">
                <i class="bi bi-person"></i>
                Mi perfil
            </a>
        </div>
    </div>

    <div class="pc-config-grid">
        <div class="pc-card">
            <div class="pc-card-header">
                <div>
                    <h2>Preferencias</h2>
                    <p>Estas opciones se guardan en este navegador.</p>
                </div>
            </div>

            <div class="pc-config-list">
                <div class="pc-config-item">
                    <div class="pc-config-icon">
                        <i class="bi bi-bell"></i>
                    </div>

                    <div class="pc-config-text">
                        <strong>Notificaciones</strong>
                        <small>Mostrar avisos de pedidos y carrito.</small>
                    </div>

                    <label class="pc-switch">
                        <input type="checkbox" id="cfgNotificaciones" checked />
                        <span></span>
                    </label>
                </div>

                <div class="pc-config-item">
                    <div class="pc-config-icon">
                        <i class="bi bi-moon-stars"></i>
                    </div>

                    <div class="pc-config-text">
                        <strong>Modo oscuro del panel</strong>
                        <small>Apariencia oscura para las secciones de cuenta.</small>
                    </div>

                    <label class="pc-switch">
                        <input type="checkbox" id="cfgModoOscuro" />
                        <span></span>
                    </label>
                </div>

                <button type="button" class="pc-config-item pc-config-button" id="cfgIdiomaBtn">
                    <div class="pc-config-icon">
                        <i class="bi bi-translate"></i>
                    </div>

                    <div class="pc-config-text">
                        <strong>Idioma</strong>
                        <small id="cfgIdiomaTexto">Español</small>
                    </div>

                    <i class="bi bi-chevron-right"></i>
                </button>

                <button type="button" class="pc-config-item pc-config-button" id="cfgPrivacidadBtn">
                    <div class="pc-config-icon">
                        <i class="bi bi-shield-lock"></i>
                    </div>

                    <div class="pc-config-text">
                        <strong>Privacidad</strong>
                        <small>Consulta cómo se usan tus datos del panel.</small>
                    </div>

                    <i class="bi bi-chevron-right"></i>
                </button>
            </div>
        </div>

        <div class="pc-card pc-config-account-card">
            <div class="pc-card-header">
                <div>
                    <h2>Accesos rápidos</h2>
                    <p>Funciones principales de tu cuenta.</p>
                </div>
            </div>

            <div class="pc-config-actions">
                <a href="@Url.Action("Notificaciones", "PortalCliente")" class="pc-support-link">
                    <span>
                        <i class="bi bi-bell"></i>
                        Ver notificaciones
                    </span>
                    <i class="bi bi-chevron-right"></i>
                </a>

                <a href="@Url.Action("Soporte", "PortalCliente")" class="pc-support-link">
                    <span>
                        <i class="bi bi-headset"></i>
                        Contactar soporte
                    </span>
                    <i class="bi bi-chevron-right"></i>
                </a>

                <a href="@Url.Action("MisTarjetas", "PortalCliente")" class="pc-support-link">
                    <span>
                        <i class="bi bi-credit-card"></i>
                        Tarjetas guardadas
                    </span>
                    <i class="bi bi-chevron-right"></i>
                </a>

                <a href="@Url.Action("Logout", "Home")" class="pc-support-link danger">
                    <span>
                        <i class="bi bi-box-arrow-left"></i>
                        Cerrar sesión
                    </span>
                    <i class="bi bi-chevron-right"></i>
                </a>
            </div>
        </div>
    </div>

    <div class="pc-modal-overlay" id="cfgOverlayIdioma">
        <div class="pc-modal pc-small-modal">
            <button type="button" class="pc-modal-close" id="cfgCerrarIdioma">
                <i class="bi bi-x-lg"></i>
            </button>

            <h2>Seleccionar idioma</h2>
            <p>Actualmente el panel está optimizado para español.</p>

            <button type="button" class="pc-btn pc-btn-primary pc-modal-option" data-idioma="Español">
                Español
            </button>

            <button type="button" class="pc-btn pc-btn-outline pc-modal-option" data-idioma="Inglés">
                Inglés
            </button>
        </div>
    </div>

    <div class="pc-modal-overlay" id="cfgOverlayPrivacidad">
        <div class="pc-modal pc-small-modal">
            <button type="button" class="pc-modal-close" id="cfgCerrarPrivacidad">
                <i class="bi bi-x-lg"></i>
            </button>

            <h2>Privacidad</h2>
            <p>El panel cliente consulta únicamente información relacionada con tu sesión: perfil, carrito, pedidos, favoritos y tarjetas registradas.</p>
            <p>No se muestran datos de otros clientes.</p>

            <button type="button" class="pc-btn pc-btn-primary" id="cfgAceptarPrivacidad">
                Entendido
            </button>
        </div>
    </div>
</section>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-configuracion.js?v=16")"></script>
End Section