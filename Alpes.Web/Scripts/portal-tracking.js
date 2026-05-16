document.addEventListener('DOMContentLoaded', function () {
    var page = document.getElementById('trkPage');
    var content = document.getElementById('trkContent');

    if (!page || !content) {
        return;
    }

    var orderId = page.getAttribute('data-order-id');
    if (!orderId || orderId === '0') {
        content.innerHTML = '<div class="trk-error">No se recibió el id del pedido.</div>';
        return;
    }

    var endpoint = '/PortalCliente/ObtenerTrackingOrdenData?ordenVentaId=' + encodeURIComponent(orderId);

    function escapeHtml(value) {
        return String(value === null || value === undefined ? '' : value)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function normalizarRespuesta(payload) {
        if (!payload) {
            return { ok: false, data: null, message: 'Respuesta vacía del servidor.' };
        }

        if (payload.ok !== undefined) {
            return {
                ok: payload.ok === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }

        if (payload.success !== undefined) {
            return {
                ok: payload.success === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }

        return { ok: true, data: payload, message: '' };
    }

    function obtenerClasePaso(estadoPaso) {
        var valor = String(estadoPaso || '').toLowerCase();

        if (valor === 'done') {
            return 'trk-step done';
        }

        if (valor === 'current') {
            return 'trk-step current';
        }

        return 'trk-step pending';
    }

    function iconoPaso(estadoPaso) {
        var valor = String(estadoPaso || '').toLowerCase();

        if (valor === 'done') {
            return 'fa-check';
        }

        if (valor === 'current') {
            return 'fa-truck-fast';
        }

        return 'fa-circle';
    }

    function render(data) {
        var pasos = data.Timeline || [];
        var htmlPasos = '';
        var i;

        for (i = 0; i < pasos.length; i += 1) {
            var paso = pasos[i];

            htmlPasos += ''
                + '<div class="' + escapeHtml(obtenerClasePaso(paso.EstadoPaso)) + '">'
                + '  <div class="trk-dot"><i class="fa-solid ' + escapeHtml(iconoPaso(paso.EstadoPaso)) + '"></i></div>'
                + '  <div class="trk-info">'
                + '      <strong>' + escapeHtml(paso.Titulo || ('Paso ' + (i + 1))) + '</strong>'
                + '      <small>' + escapeHtml(paso.Subtitulo || '') + '</small>'
                + '  </div>'
                + '</div>';
        }

        if (htmlPasos === '') {
            htmlPasos = '<div class="trk-empty">No hay eventos de seguimiento disponibles para este pedido.</div>';
        }

        content.innerHTML = ''
            + '<section class="trk-summary-new">'
            + '  <div>'
            + '      <span class="orders-kicker">Pedido</span>'
            + '      <h2>' + escapeHtml(data.NumOrden || ('ORD-' + data.OrdenVentaId)) + '</h2>'
            + '      <p>' + escapeHtml(data.TrackingCodigo || 'Tracking no disponible') + '</p>'
            + '  </div>'
            + '  <div class="trk-status-card">'
            + '      <span>Estado actual</span>'
            + '      <strong>' + escapeHtml(data.EstadoUi || 'PENDIENTE') + '</strong>'
            + '      <small>Entrega estimada: ' + escapeHtml(data.FechaEntregaEstimadaTexto || 'No disponible') + '</small>'
            + '  </div>'
            + '</section>'
            + '<section class="trk-panel-new">'
            + '  <div class="trk-section-title">Línea de tiempo</div>'
            + htmlPasos
            + '</section>'
            + '<div class="trk-actions">'
            + '  <a class="od-btn od-btn--ghost" href="/PortalCliente/MisOrdenes">Volver</a>'
            + '  <a class="od-btn od-btn--primary" href="/PortalCliente/DetalleOrden?id=' + encodeURIComponent(data.OrdenVentaId) + '">Ver detalle</a>'
            + '</div>';
    }

    fetch(endpoint, {
        method: 'GET',
        credentials: 'same-origin',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error('No se pudo obtener la información de seguimiento.');
            }
            return response.json();
        })
        .then(function (payload) {
            var respuesta = normalizarRespuesta(payload);

            if (!respuesta.ok || !respuesta.data) {
                throw new Error(respuesta.message || 'No se pudo obtener la información de seguimiento.');
            }

            render(respuesta.data);
        })
        .catch(function (error) {
            content.innerHTML = '<div class="trk-error">' + escapeHtml(error.message || 'Ocurrió un error al cargar el seguimiento.') + '</div>';
        });
});
