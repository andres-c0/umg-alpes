(function () {
    'use strict';

    function $(id) { return document.getElementById(id); }

    function escapeHtml(value) {
        return String(value === null || value === undefined ? '' : value)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    }

    function getValue(obj, keys) {
        for (var i = 0; i < keys.length; i += 1) {
            if (obj && obj[keys[i]] !== undefined && obj[keys[i]] !== null) return obj[keys[i]];
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
        setTimeout(function () { toast.classList.add('show'); }, 20);
        setTimeout(function () {
            toast.classList.remove('show');
            setTimeout(function () { toast.remove(); }, 250);
        }, 3500);
    }

    function requestJson(url, options) {
        options = options || {};
        options.credentials = 'same-origin';
        options.headers = options.headers || {};
        options.headers['X-Requested-With'] = 'XMLHttpRequest';
        return fetch(url, options).then(function (res) {
            return res.json().catch(function () { return null; }).then(function (payload) {
                var normalized = normalize(payload);
                if (!res.ok || !normalized.ok) throw new Error(normalized.message || 'No se pudo procesar la solicitud.');
                return normalized;
            });
        });
    }

    function formatCardNumber(tarjeta) {
        var masked = getValue(tarjeta, ['NumeroMascarado', 'numeroMascarado', 'NumeroEnmascarado', 'numeroEnmascarado']);
        var last4 = getValue(tarjeta, ['Ultimos4', 'ultimos4']);
        if (String(masked).trim() !== '') return masked;
        return '**** **** **** ' + (String(last4).trim() || '0000');
    }

    function isDefault(tarjeta) {
        var val = getValue(tarjeta, ['EsPredeterminada', 'esPredeterminada', 'Predeterminada', 'predeterminada']);
        return val === true || val === 1 || val === '1' || String(val).toUpperCase() === 'S' || String(val).toUpperCase() === 'TRUE';
    }

    function renderEmpty() {
        var list = $('tarjetasLista');
        if (!list) return;
        list.innerHTML = '<div class="pc-empty-state pc-empty-state-wide">'
            + '<i class="bi bi-credit-card"></i>'
            + '<h3>No tienes tarjetas registradas</h3>'
            + '<p>Agrega una tarjeta para usarla en el checkout.</p>'
            + '</div>';
    }

    function renderCards(tarjetas) {
        var list = $('tarjetasLista');
        if (!list) return;
        if (!tarjetas || tarjetas.length === 0) {
            renderEmpty();
            return;
        }

        list.innerHTML = tarjetas.map(function (tarjeta) {
            var id = getValue(tarjeta, ['TarjetaClienteId', 'tarjetaClienteId', 'TarjetaId', 'tarjetaId']);
            var titular = getValue(tarjeta, ['Titular', 'titular', 'NombreTitular', 'nombreTitular']) || 'Titular no registrado';
            var marca = String(getValue(tarjeta, ['Marca', 'marca', 'Franquicia', 'franquicia']) || 'TARJETA').toUpperCase();
            var alias = getValue(tarjeta, ['Alias', 'alias']);
            var mes = getValue(tarjeta, ['MesExpiracion', 'mesExpiracion', 'Mes', 'mes']);
            var anio = getValue(tarjeta, ['AnioExpiracion', 'anioExpiracion', 'Anio', 'anio']);
            var pred = isDefault(tarjeta);

            return '<article class="pc-payment-card ' + (marca.indexOf('MASTER') >= 0 ? 'pc-payment-master' : 'pc-payment-visa') + '" data-id="' + escapeHtml(id) + '">'
                + '<div class="pc-payment-top">'
                + '<span>' + escapeHtml(marca) + '</span>'
                + (pred ? '<strong>Predeterminada</strong>' : '')
                + '</div>'
                + '<div class="pc-payment-number">' + escapeHtml(formatCardNumber(tarjeta)) + '</div>'
                + '<div class="pc-payment-bottom">'
                + '<div><small>TITULAR</small><b>' + escapeHtml(titular) + '</b></div>'
                + '<div><small>VENCE</small><b>' + escapeHtml(mes || '--') + '/' + escapeHtml(anio || '--') + '</b></div>'
                + '</div>'
                + (String(alias).trim() !== '' ? '<p class="pc-payment-alias">' + escapeHtml(alias) + '</p>' : '')
                + '<div class="pc-payment-actions">'
                + (pred ? '' : '<button type="button" data-action="default" data-id="' + escapeHtml(id) + '">Predeterminada</button>')
                + '<button type="button" data-action="delete" data-id="' + escapeHtml(id) + '" class="danger">Eliminar</button>'
                + '</div>'
                + '</article>';
        }).join('');
    }

    function loadCards() {
        var list = $('tarjetasLista');
        if (list) list.innerHTML = '<div class="pc-loading-card">Cargando tarjetas...</div>';
        requestJson('/PortalCliente/ObtenerTarjetasData', { method: 'GET' })
            .then(function (res) { renderCards(res.data || []); })
            .catch(function (err) {
                console.error(err);
                renderEmpty();
                showToast(err.message || 'No se pudieron cargar las tarjetas.', 'error');
            });
    }

    function openModal() {
        var modal = $('tarjetaModal');
        if (modal) modal.classList.add('show');
    }

    function closeModal() {
        var modal = $('tarjetaModal');
        if (modal) modal.classList.remove('show');
    }

    document.addEventListener('DOMContentLoaded', function () {
        var page = $('tarjetasClientePage');
        if (!page) return;

        var btnOpen = $('btnAbrirTarjeta');
        var btnClose = $('btnCerrarTarjeta');
        var btnRefresh = $('btnActualizarTarjetas');
        var modal = $('tarjetaModal');
        var form = $('tarjetaForm');
        var list = $('tarjetasLista');

        if (btnOpen) btnOpen.addEventListener('click', openModal);
        if (btnClose) btnClose.addEventListener('click', closeModal);
        if (btnRefresh) btnRefresh.addEventListener('click', loadCards);
        if (modal) modal.addEventListener('click', function (e) { if (e.target === modal) closeModal(); });
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeModal(); });

        var inputNumber = $('tarjetaNumero');
        if (inputNumber) {
            inputNumber.addEventListener('input', function () {
                var digits = inputNumber.value.replace(/\D/g, '').substring(0, 16);
                inputNumber.value = digits.replace(/(.{4})/g, '$1 ').trim();
            });
        }

        if (form) {
            form.addEventListener('submit', function (e) {
                e.preventDefault();
                var btn = $('btnGuardarTarjeta');
                if (btn) { btn.disabled = true; btn.textContent = 'Registrando...'; }

                var payload = {
                    titular: $('tarjetaTitular').value.trim(),
                    numero: $('tarjetaNumero').value.trim(),
                    marca: $('tarjetaMarca').value,
                    mesExpiracion: $('tarjetaMes').value,
                    anioExpiracion: $('tarjetaAnio').value,
                    alias: $('tarjetaAlias').value.trim(),
                    predeterminada: $('tarjetaPredeterminada').checked ? 'true' : 'false'
                };

                requestJson('/PortalCliente/CrearTarjetaData', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify(payload)
                }).then(function (res) {
                    form.reset();
                    closeModal();
                    loadCards();
                    showToast(res.message || 'Tarjeta registrada correctamente.', 'success');
                }).catch(function (err) {
                    showToast(err.message || 'No se pudo registrar la tarjeta.', 'error');
                }).finally(function () {
                    if (btn) { btn.disabled = false; btn.textContent = 'Registrar tarjeta'; }
                });
            });
        }

        if (list) {
            list.addEventListener('click', function (e) {
                var btn = e.target.closest('button[data-action]');
                if (!btn) return;
                var id = btn.getAttribute('data-id');
                var action = btn.getAttribute('data-action');
                var endpoint = action === 'default' ? '/PortalCliente/MarcarTarjetaPredeterminadaData' : '/PortalCliente/DesactivarTarjetaData';

                requestJson(endpoint, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify({ tarjetaId: id })
                }).then(function (res) {
                    loadCards();
                    showToast(res.message || 'Tarjeta actualizada.', 'success');
                }).catch(function (err) {
                    showToast(err.message || 'No se pudo actualizar la tarjeta.', 'error');
                });
            });
        }

        loadCards();
    });
}());
