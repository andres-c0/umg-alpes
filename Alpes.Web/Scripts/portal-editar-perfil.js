(function () {
    'use strict';

    function $(id) { return document.getElementById(id); }

    function getValue(obj, keys) {
        for (var i = 0; i < keys.length; i += 1) {
            if (obj && obj[keys[i]] !== undefined && obj[keys[i]] !== null) {
                return obj[keys[i]];
            }
        }
        return '';
    }

    function normalize(payload) {
        if (!payload) return { ok: false, data: null, message: 'Respuesta vacía.' };
        if (payload.ok !== undefined || payload.success !== undefined) {
            return {
                ok: payload.ok === true || payload.success === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }
        return { ok: true, data: payload, message: '' };
    }

    function showToast(message, type) {
        var existing = document.querySelector('.pc-toast');
        if (existing) existing.remove();

        var toast = document.createElement('div');
        toast.className = 'pc-toast ' + (type === 'error' ? 'pc-toast-error' : 'pc-toast-success');
        toast.textContent = message;
        document.body.appendChild(toast);
        window.setTimeout(function () { toast.classList.add('show'); }, 20);
        window.setTimeout(function () {
            toast.classList.remove('show');
            window.setTimeout(function () { toast.remove(); }, 250);
        }, 3500);
    }

    function text(id, value) {
        var el = $(id);
        if (el) el.textContent = value && String(value).trim() !== '' ? value : '-';
    }

    function value(id, value) {
        var el = $(id);
        if (el) el.value = value || '';
    }

    function getAntiForgeryToken() {
        var token = document.querySelector('input[name="__RequestVerificationToken"]');
        return token ? token.value : '';
    }

    var currentProfile = null;

    function renderProfile(perfil) {
        currentProfile = perfil || {};

        var nombres = String(getValue(perfil, ['Nombres', 'nombres', 'Nombre', 'nombre'])).trim();
        var apellidos = String(getValue(perfil, ['Apellidos', 'apellidos', 'Apellido', 'apellido'])).trim();
        var email = String(getValue(perfil, ['Email', 'email'])).trim();
        var fullName = String(getValue(perfil, ['NombreCompleto', 'nombreCompleto'])).trim() || (nombres + ' ' + apellidos).trim();

        text('perfilNombre', fullName || 'Cliente');
        text('perfilEmail', email || 'Sin correo registrado');
        text('perfilAvatar', (fullName || nombres || 'C').charAt(0).toUpperCase());
        text('perfilEstado', getValue(perfil, ['Estado', 'estado']) || 'ACTIVO');

        text('infoNombres', nombres);
        text('infoApellidos', apellidos);
        text('infoEmail', email);
        text('infoTelResidencia', getValue(perfil, ['TelResidencia', 'telResidencia']));
        text('infoTelCelular', getValue(perfil, ['TelCelular', 'telCelular']));
        text('infoDireccion', getValue(perfil, ['Direccion', 'direccion']));
        text('infoCiudad', getValue(perfil, ['Ciudad', 'ciudad']));
        text('infoDepartamento', getValue(perfil, ['Departamento', 'departamento']));
        text('infoPais', getValue(perfil, ['Pais', 'pais']));

        value('perfilInputNombres', nombres);
        value('perfilInputApellidos', apellidos);
        value('perfilInputEmail', email);
        value('perfilInputTelResidencia', getValue(perfil, ['TelResidencia', 'telResidencia']));
        value('perfilInputTelCelular', getValue(perfil, ['TelCelular', 'telCelular']));
        value('perfilInputDireccion', getValue(perfil, ['Direccion', 'direccion']));
        value('perfilInputCiudad', getValue(perfil, ['Ciudad', 'ciudad']));
        value('perfilInputDepartamento', getValue(perfil, ['Departamento', 'departamento']));
        value('perfilInputPais', getValue(perfil, ['Pais', 'pais']) || 'Guatemala');
    }

    function requestJson(url, options) {
        options = options || {};
        options.credentials = 'same-origin';
        options.headers = options.headers || {};
        options.headers['X-Requested-With'] = 'XMLHttpRequest';

        var method = String(options.method || 'GET').toUpperCase();
        if (method !== 'GET' && method !== 'HEAD') {
            var token = getAntiForgeryToken();
            if (token) {
                options.headers['RequestVerificationToken'] = token;
            }
        }

        return fetch(url, options)
            .then(function (res) {
                return res.json().catch(function () { return null; }).then(function (payload) {
                    var normalized = normalize(payload);
                    if (!res.ok || !normalized.ok) {
                        throw new Error(normalized.message || 'No se pudo procesar la solicitud.');
                    }
                    return normalized;
                });
            });
    }

    function clearPasswordForm() {
        value('passwordActual', '');
        value('passwordNueva', '');
        value('confirmarPassword', '');
    }

    function loadProfile() {
        requestJson('/PortalCliente/ObtenerPerfilActual', { method: 'GET' })
            .then(function (res) { renderProfile(res.data); })
            .catch(function (err) {
                console.error(err);
                text('perfilNombre', 'No se pudo cargar el perfil');
                text('perfilEmail', err.message || 'Intenta recargar la página');
                showToast(err.message || 'No se pudo cargar el perfil.', 'error');
            });
    }

    function openModal() {
        if (currentProfile) renderProfile(currentProfile);

        var modal = $('perfilModal');

        if (modal) {
            modal.classList.add('show');
            modal.style.display = 'flex';
            modal.style.opacity = '1';
            modal.style.visibility = 'visible';
        }
    }
    function closeModal() {
        var modal = $('perfilModal');

        if (modal) {
            modal.classList.remove('show');
            modal.style.display = 'none';
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        var page = $('perfilClientePage');
        if (!page) return;

        var btnEdit = $('btnEditarPerfil');
        var btnClose = $('btnCerrarPerfil');
        var modal = $('perfilModal');
        var form = $('perfilForm');
        var passwordForm = $('passwordForm');

        if (btnEdit) btnEdit.addEventListener('click', openModal);
        if (btnClose) btnClose.addEventListener('click', closeModal);
        if (modal) modal.addEventListener('click', function (e) { if (e.target === modal) closeModal(); });
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeModal(); });

        if (form) {
            form.addEventListener('submit', function (e) {
                e.preventDefault();
                var btn = $('btnGuardarPerfil');
                if (btn) {
                    btn.disabled = true;
                    btn.textContent = 'Guardando...';
                }

                var payload = {
                    nombres: $('perfilInputNombres').value.trim(),
                    apellidos: $('perfilInputApellidos').value.trim(),
                    email: $('perfilInputEmail').value.trim(),
                    telResidencia: $('perfilInputTelResidencia').value.trim(),
                    telCelular: $('perfilInputTelCelular').value.trim(),
                    direccion: $('perfilInputDireccion').value.trim(),
                    ciudad: $('perfilInputCiudad').value.trim(),
                    departamento: $('perfilInputDepartamento').value.trim(),
                    pais: $('perfilInputPais').value.trim()
                };

                requestJson('/PortalCliente/ActualizarPerfil', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify(payload)
                })
                    .then(function (res) {
                        renderProfile(res.data || payload);
                        closeModal();
                        showToast(res.message || 'Perfil actualizado correctamente.', 'success');
                    })
                    .catch(function (err) {
                        showToast(err.message || 'No se pudo actualizar el perfil.', 'error');
                    })
                    .finally(function () {
                        if (btn) {
                            btn.disabled = false;
                            btn.textContent = 'Guardar cambios';
                        }
                    });
            });
        }



        if (passwordForm) {
            passwordForm.addEventListener('submit', function (e) {
                e.preventDefault();
                var btnPassword = $('btnGuardarPassword');
                if (btnPassword) {
                    btnPassword.disabled = true;
                    btnPassword.textContent = 'Actualizando...';
                }

                var payload = {
                    passwordActual: $('passwordActual').value,
                    passwordNueva: $('passwordNueva').value,
                    confirmarPassword: $('confirmarPassword').value
                };

                requestJson('/PortalCliente/CambiarContrasena', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify(payload)
                })
                    .then(function (res) {
                        clearPasswordForm();
                        showToast(res.message || 'Contraseña actualizada correctamente.', 'success');
                    })
                    .catch(function (err) {
                        showToast(err.message || 'No se pudo cambiar la contraseña.', 'error');
                    })
                    .finally(function () {
                        if (btnPassword) {
                            btnPassword.disabled = false;
                            btnPassword.textContent = 'Actualizar contraseña';
                        }
                    });
            });
        }
        loadProfile();
    });
}());
