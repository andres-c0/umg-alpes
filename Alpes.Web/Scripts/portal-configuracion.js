document.addEventListener('DOMContentLoaded', function () {
    var page = document.getElementById('cfgPage');
    var chkNotificaciones = document.getElementById('cfgNotificaciones');
    var chkModoOscuro = document.getElementById('cfgModoOscuro');
    var idiomaBtn = document.getElementById('cfgIdiomaBtn');
    var idiomaTexto = document.getElementById('cfgIdiomaTexto');
    var privacidadBtn = document.getElementById('cfgPrivacidadBtn');
    var message = document.getElementById('cfgMessage');

    if (!page) return;

    function showMessage(text) {
        if (!message) return;
        message.textContent = text;
        message.classList.add('show');

        setTimeout(function () {
            message.classList.remove('show');
        }, 2500);
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

    function applyDarkMode() {
        if (!chkModoOscuro) return;

        if (chkModoOscuro.checked) {
            page.classList.add('dark');
        } else {
            page.classList.remove('dark');
        }
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

    if (idiomaBtn && idiomaTexto) {
        idiomaBtn.addEventListener('click', function () {
            var current = idiomaTexto.textContent.trim();
            var next = current === 'Español' ? 'Inglés' : 'Español';
            idiomaTexto.textContent = next;
            localStorage.setItem('cfg_idioma', next);
            showMessage('Idioma cambiado a ' + next);
        });
    }

    if (privacidadBtn) {
        privacidadBtn.addEventListener('click', function () {
            showMessage('Opciones de privacidad disponibles próximamente');
        });
    }
});