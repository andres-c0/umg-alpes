@Code
    ViewData("Title") = "Configuración"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"
End Code

<div class="cfg-page" id="cfgPage">

    <div class="cfg-top">
        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="cfg-back">
            <i class="bi bi-chevron-left"></i>
        </a>

        <div class="cfg-title">Configuración</div>
    </div>

    <div class="cfg-section">
        <div class="cfg-item">
            <div>
                <strong>Notificaciones</strong>
                <small>Activar o desactivar alertas</small>
            </div>
            <label class="cfg-switch">
                <input type="checkbox" id="cfgNotificaciones" checked />
                <span class="cfg-slider"></span>
            </label>
        </div>

        <div class="cfg-item">
            <div>
                <strong>Modo oscuro</strong>
                <small>Cambiar apariencia</small>
            </div>
            <label class="cfg-switch">
                <input type="checkbox" id="cfgModoOscuro" />
                <span class="cfg-slider"></span>
            </label>
        </div>

        <button type="button" class="cfg-item cfg-item-btn" id="cfgIdiomaBtn">
            <div>
                <strong>Idioma</strong>
                <small id="cfgIdiomaTexto">Español</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </button>

        <button type="button" class="cfg-item cfg-item-btn" id="cfgPrivacidadBtn">
            <div>
                <strong>Privacidad</strong>
                <small>Administrar datos</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </button>
    </div>

    <div class="cfg-message" id="cfgMessage"></div>

    <div class="cfg-overlay" id="cfgOverlayIdioma">
        <div class="cfg-modal">
            <div class="cfg-modal-title">Seleccionar idioma</div>

            <button type="button" class="cfg-modal-option" data-idioma="Español">
                Español
            </button>

            <button type="button" class="cfg-modal-option" data-idioma="Inglés">
                Inglés
            </button>

            <button type="button" class="cfg-modal-close" id="cfgCerrarIdioma">
                Cerrar
            </button>
        </div>
    </div>

    <div class="cfg-overlay" id="cfgOverlayPrivacidad">
        <div class="cfg-modal">
            <div class="cfg-modal-title">Privacidad</div>
            <p class="cfg-modal-text">
                Aquí podrás administrar tus datos, permisos y preferencias de privacidad.
            </p>

            <button type="button" class="cfg-modal-close" id="cfgCerrarPrivacidad">
                Entendido
            </button>
        </div>
    </div>
</div>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        var page = document.getElementById('cfgPage');
        var chkNotificaciones = document.getElementById('cfgNotificaciones');
        var chkModoOscuro = document.getElementById('cfgModoOscuro');
        var idiomaBtn = document.getElementById('cfgIdiomaBtn');
        var idiomaTexto = document.getElementById('cfgIdiomaTexto');
        var privacidadBtn = document.getElementById('cfgPrivacidadBtn');
        var message = document.getElementById('cfgMessage');

        var overlayIdioma = document.getElementById('cfgOverlayIdioma');
        var overlayPrivacidad = document.getElementById('cfgOverlayPrivacidad');
        var cerrarIdioma = document.getElementById('cfgCerrarIdioma');
        var cerrarPrivacidad = document.getElementById('cfgCerrarPrivacidad');
        var idiomaOptions = document.querySelectorAll('.cfg-modal-option');

        function showMessage(text) {
            if (!message) return;
            message.textContent = text;
            message.classList.add('show');

            setTimeout(function () {
                message.classList.remove('show');
            }, 2200);
        }

        function applyDarkMode() {
            if (!chkModoOscuro) return;

            if (chkModoOscuro.checked) {
                page.classList.add('dark');
            } else {
                page.classList.remove('dark');
            }
        }

        var savedNotifications = localStorage.getItem('cfg_notificaciones');
        var savedDarkMode = localStorage.getItem('cfg_modo_oscuro');
        var savedLanguage = localStorage.getItem('cfg_idioma');

        if (savedNotifications !== null && chkNotificaciones) {
            chkNotificaciones.checked = savedNotifications === 'true';
        }

        if (savedDarkMode !== null && chkModoOscuro) {
            chkModoOscuro.checked = savedDarkMode === 'true';
        }

        if (savedLanguage && idiomaTexto) {
            idiomaTexto.textContent = savedLanguage;
        }

        applyDarkMode();

        if (chkNotificaciones) {
            chkNotificaciones.addEventListener('change', function () {
                localStorage.setItem('cfg_notificaciones', chkNotificaciones.checked);
                showMessage(chkNotificaciones.checked ? 'Notificaciones activadas' : 'Notificaciones desactivadas');
            });
        }

        if (chkModoOscuro) {
            chkModoOscuro.addEventListener('change', function () {
                localStorage.setItem('cfg_modo_oscuro', chkModoOscuro.checked);
                applyDarkMode();
                showMessage(chkModoOscuro.checked ? 'Modo oscuro activado' : 'Modo oscuro desactivado');
            });
        }

        if (idiomaBtn && overlayIdioma) {
            idiomaBtn.addEventListener('click', function () {
                overlayIdioma.classList.add('show');
            });
        }

        idiomaOptions.forEach(function (btn) {
            btn.addEventListener('click', function () {
                var idioma = btn.getAttribute('data-idioma');
                idiomaTexto.textContent = idioma;
                localStorage.setItem('cfg_idioma', idioma);
                overlayIdioma.classList.remove('show');
                showMessage('Idioma cambiado a ' + idioma);
            });
        });

        if (cerrarIdioma && overlayIdioma) {
            cerrarIdioma.addEventListener('click', function () {
                overlayIdioma.classList.remove('show');
            });
        }

        if (privacidadBtn && overlayPrivacidad) {
            privacidadBtn.addEventListener('click', function () {
                overlayPrivacidad.classList.add('show');
            });
        }

        if (cerrarPrivacidad && overlayPrivacidad) {
            cerrarPrivacidad.addEventListener('click', function () {
                overlayPrivacidad.classList.remove('show');
            });
        }

        if (overlayIdioma) {
            overlayIdioma.addEventListener('click', function (e) {
                if (e.target === overlayIdioma) {
                    overlayIdioma.classList.remove('show');
                }
            });
        }

        if (overlayPrivacidad) {
            overlayPrivacidad.addEventListener('click', function (e) {
                if (e.target === overlayPrivacidad) {
                    overlayPrivacidad.classList.remove('show');
                }
            });
        }
    });
</script>