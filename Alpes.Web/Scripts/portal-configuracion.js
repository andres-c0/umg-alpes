(function () {
    'use strict';

    function $(id) { return document.getElementById(id); }
    var page = $('configuracionCliente');
    var chkNotificaciones = $('cfgNotificaciones');
    var chkModoOscuro = $('cfgModoOscuro');
    var idiomaBtn = $('cfgIdiomaBtn');
    var idiomaTexto = $('cfgIdiomaTexto');
    var privacidadBtn = $('cfgPrivacidadBtn');
    var overlayIdioma = $('cfgOverlayIdioma');
    var overlayPrivacidad = $('cfgOverlayPrivacidad');
    var cerrarIdioma = $('cfgCerrarIdioma');
    var cerrarPrivacidad = $('cfgCerrarPrivacidad');
    var aceptarPrivacidad = $('cfgAceptarPrivacidad');

    function toast(message, type) {
        var existing = document.querySelector('.pc-toast');
        if (existing) existing.remove();
        var el = document.createElement('div');
        el.className = 'pc-toast ' + (type === 'error' ? 'pc-toast-error' : 'pc-toast-success');
        el.textContent = message;
        document.body.appendChild(el);
        setTimeout(function () { el.classList.add('show'); }, 10);
        setTimeout(function () { el.classList.remove('show'); }, 2300);
        setTimeout(function () { el.remove(); }, 2700);
    }

    function applyDarkMode() {
        if (!page || !chkModoOscuro) return;
        page.classList.toggle('pc-config-dark', chkModoOscuro.checked);
    }

    function openModal(modal) { if (modal) modal.classList.add('show'); }
    function closeModal(modal) { if (modal) modal.classList.remove('show'); }

    document.addEventListener('DOMContentLoaded', function () {
        var savedNotifications = localStorage.getItem('pc_cfg_notificaciones');
        var savedDarkMode = localStorage.getItem('pc_cfg_modo_oscuro');
        var savedLanguage = localStorage.getItem('pc_cfg_idioma');

        if (savedNotifications !== null && chkNotificaciones) chkNotificaciones.checked = savedNotifications === 'true';
        if (savedDarkMode !== null && chkModoOscuro) chkModoOscuro.checked = savedDarkMode === 'true';
        if (savedLanguage && idiomaTexto) idiomaTexto.textContent = savedLanguage;
        applyDarkMode();

        if (chkNotificaciones) {
            chkNotificaciones.addEventListener('change', function () {
                localStorage.setItem('pc_cfg_notificaciones', chkNotificaciones.checked);
                toast(chkNotificaciones.checked ? 'Notificaciones activadas' : 'Notificaciones desactivadas');
            });
        }

        if (chkModoOscuro) {
            chkModoOscuro.addEventListener('change', function () {
                localStorage.setItem('pc_cfg_modo_oscuro', chkModoOscuro.checked);
                applyDarkMode();
                toast(chkModoOscuro.checked ? 'Modo oscuro activado' : 'Modo oscuro desactivado');
            });
        }

        if (idiomaBtn) idiomaBtn.addEventListener('click', function () { openModal(overlayIdioma); });
        if (privacidadBtn) privacidadBtn.addEventListener('click', function () { openModal(overlayPrivacidad); });
        if (cerrarIdioma) cerrarIdioma.addEventListener('click', function () { closeModal(overlayIdioma); });
        if (cerrarPrivacidad) cerrarPrivacidad.addEventListener('click', function () { closeModal(overlayPrivacidad); });
        if (aceptarPrivacidad) aceptarPrivacidad.addEventListener('click', function () { closeModal(overlayPrivacidad); });

        document.querySelectorAll('.pc-modal-option').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var idioma = btn.getAttribute('data-idioma') || 'Español';
                if (idiomaTexto) idiomaTexto.textContent = idioma;
                localStorage.setItem('pc_cfg_idioma', idioma);
                closeModal(overlayIdioma);
                toast('Idioma cambiado a ' + idioma);
            });
        });

        [overlayIdioma, overlayPrivacidad].forEach(function (modal) {
            if (!modal) return;
            modal.addEventListener('click', function (e) {
                if (e.target === modal) closeModal(modal);
            });
        });
    });
}());
