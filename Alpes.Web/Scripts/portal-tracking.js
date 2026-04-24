document.addEventListener('DOMContentLoaded', function () {
    var page = document.getElementById('trkPage');
    var content = document.getElementById('trkContent');

    if (!page || !content) {
        return;
    }

    var orderId = page.getAttribute('data-order-id');
    if (!orderId || orderId === '0') {
        content.innerHTML = '<div class="trk-error">No se recibio el id de la orden.</div>';
        return;
    }

    var endpoint = '/PortalCliente/ObtenerTrackingOrdenData?ordenVentaId=' + encodeURIComponent(orderId);

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function normalizarRespuesta(payload) {
        if (!payload) {
            return { ok: false, data: null, message: 'Respuesta vacia del servidor.' };
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

        return 'trk-step';
    }

    function render(data) {
        var pasos = data.Timeline || [];
        var htmlPasos = '';
        var i;

        for (i = 0; i < pasos.length; i += 1) {
            var paso = pasos[i];

            htmlPasos += ''
                + '<div class="' + escapeHtml(obtenerClasePaso(paso.EstadoPaso)) + '">'
                + '  <div class="trk-dot"></div>'
                + '  <div class="trk-info">'
                + '      <strong>' + escapeHtml(paso.Titulo || ('Paso ' + (i + 1))) + '</strong>'
                + '      <small>' + escapeHtml(paso.Subtitulo || '') + '</small>'
                + '  </div>'
                + '</div>';
        }

        if (htmlPasos === '') {
            htmlPasos = '<div class="trk-empty">No hay eventos de tracking disponibles para esta orden.</div>';
        }

        content.innerHTML = ''
            + '<div class="trk-header">'
            + '  <div class="trk-order">' + escapeHtml(data.NumOrden || ('ORD-' + data.OrdenVentaId)) + '</div>'
            + '  <div class="trk-code">' + escapeHtml(data.TrackingCodigo || 'Tracking no disponible') + '</div>'
            + '</div>'
            + '<div class="trk-boxes">'
            + '  <div class="trk-mini-card">'
            + '      <div class="trk-mini-title">Estado actual</div>'
            + '      <div class="trk-mini-text">' + escapeHtml(data.EstadoUi || 'PENDIENTE') + '</div>'
            + '  </div>'
            + '  <div class="trk-mini-card">'
            + '      <div class="trk-mini-title">Entrega estimada</div>'
            + '      <div class="trk-mini-text">' + escapeHtml(data.FechaEntregaEstimadaTexto || 'No disponible') + '</div>'
            + '  </div>'
            + '</div>'
            + '<div class="trk-section">'
            + '  <div class="trk-section-title">Linea de tiempo</div>'
            + htmlPasos
            + '</div>'
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
                throw new Error('No se pudo obtener la informacion de tracking.');
            }
            return response.json();
        })
        .then(function (payload) {
            var respuesta = normalizarRespuesta(payload);

            if (!respuesta.ok || !respuesta.data) {
                throw new Error(respuesta.message || 'No se pudo obtener la informacion de tracking.');
            }

            render(respuesta.data);
        })
        .catch(function (error) {
            content.innerHTML = '<div class="trk-error">' + escapeHtml(error.message || 'Ocurrio un error al cargar el tracking.') + '</div>';
        });
});