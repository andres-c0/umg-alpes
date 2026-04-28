document.addEventListener('DOMContentLoaded', function () {
    var page = document.getElementById('odPage');
    var content = document.getElementById('odContent');

    if (!page || !content) {
        return;
    }

    var orderId = page.getAttribute('data-order-id');
    if (!orderId || orderId === '0') {
        content.innerHTML = '<div class="od-error">No se recibio el id de la orden.</div>';
        return;
    }

    var endpoint = '/PortalCliente/ObtenerDetalleOrdenData?id=' + encodeURIComponent(orderId);

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

    function formatearMoneda(valor, moneda) {
        var numero = Number(valor || 0);
        var codigo = String(moneda || 'GTQ').trim();
        try {
            return new Intl.NumberFormat('es-GT', {
                style: 'currency',
                currency: codigo
            }).format(numero);
        } catch (error) {
            return codigo + ' ' + numero.toFixed(2);
        }
    }

    function obtenerClaseEstado(estado) {
        var normalizado = String(estado || '').toUpperCase();

        if (normalizado === 'ENTREGADA') {
            return 'od-status od-status--success';
        }

        if (normalizado === 'CANCELADA') {
            return 'od-status od-status--danger';
        }

        return 'od-status od-status--warning';
    }

    function render(data) {
        var items = data.Items || [];
        var htmlItems = '';
        var i;

        for (i = 0; i < items.length; i += 1) {
            var item = items[i];
            var imagen = String(item.ImagenUrl || '').trim();

            htmlItems += ''
                + '<div class="od-item">'
                + '  <div class="od-item-image">'
                + (imagen !== ''
                    ? '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(item.Nombre || 'Producto') + '">'
                    : '<div class="od-item-no-image">Sin imagen</div>')
                + '  </div>'
                + '  <div class="od-item-body">'
                + '      <div class="od-item-name">' + escapeHtml(item.Nombre || 'Producto') + '</div>'
                + '      <div class="od-item-meta">Cantidad: ' + escapeHtml(item.Cantidad) + '</div>'
                + '      <div class="od-item-meta">Precio unitario: ' + escapeHtml(formatearMoneda(item.PrecioUnitario, data.Moneda)) + '</div>'
                + '  </div>'
                + '  <div class="od-item-total">' + escapeHtml(formatearMoneda(item.SubtotalLinea, data.Moneda)) + '</div>'
                + '</div>';
        }

        if (htmlItems === '') {
            htmlItems = '<div class="od-empty-lines">No hay productos asociados a esta orden.</div>';
        }

        content.innerHTML = ''
            + '<div class="od-head">'
            + '  <div>'
            + '      <div class="od-order-number">' + escapeHtml(data.NumOrden || ('ORD-' + data.OrdenVentaId)) + '</div>'
            + '      <div class="od-order-date">' + escapeHtml(data.FechaOrdenTexto || '') + '</div>'
            + '  </div>'
            + '  <div class="' + escapeHtml(obtenerClaseEstado(data.EstadoUi)) + '">' + escapeHtml(data.EstadoUi || 'PENDIENTE') + '</div>'
            + '</div>'
            + '<div class="od-total-banner">'
            + '  <span>Total</span>'
            + '  <strong>' + escapeHtml(formatearMoneda(data.Total, data.Moneda)) + '</strong>'
            + '</div>'
            + '<div class="od-section">'
            + '  <div class="od-section-title">Productos</div>'
            + htmlItems
            + '</div>'
            + '<div class="od-grid">'
            + '  <div class="od-summary-card">'
            + '      <div class="od-section-title">Resumen</div>'
            + '      <div class="od-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(data.Subtotal, data.Moneda)) + '</strong></div>'
            + '      <div class="od-row"><span>IVA</span><strong>' + escapeHtml(formatearMoneda(data.Impuesto, data.Moneda)) + '</strong></div>'
            + '      <div class="od-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(data.Descuento, data.Moneda)) + '</strong></div>'
            + '      <div class="od-row od-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(data.Total, data.Moneda)) + '</strong></div>'
            + '  </div>'
            + '  <div class="od-summary-card">'
            + '      <div class="od-section-title">Entrega</div>'
            + '      <div class="od-info-block"><strong>Direccion</strong><span>' + escapeHtml(data.Direccion || 'No disponible') + '</span></div>'
            + '      <div class="od-info-block"><strong>Tracking</strong><span>' + escapeHtml(data.TrackingCodigo || 'No disponible') + '</span></div>'
            + '      <div class="od-info-block"><strong>Observaciones</strong><span>' + escapeHtml(data.Observaciones || 'Sin observaciones') + '</span></div>'
            + '  </div>'
            + '</div>'
            + '<div class="od-actions">'
            + '  <a class="od-btn od-btn--ghost" href="/PortalCliente/MisOrdenes">Volver</a>'
            + '  <a class="od-btn od-btn--primary" href="/PortalCliente/Tracking?ordenVentaId=' + encodeURIComponent(data.OrdenVentaId) + '">Ver tracking</a>'
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
                throw new Error('No se pudo obtener el detalle de la orden.');
            }
            return response.json();
        })
        .then(function (payload) {
            var respuesta = normalizarRespuesta(payload);

            if (!respuesta.ok || !respuesta.data) {
                throw new Error(respuesta.message || 'No se pudo obtener el detalle de la orden.');
            }

            render(respuesta.data);
        })
        .catch(function (error) {
            content.innerHTML = '<div class="od-error">' + escapeHtml(error.message || 'Ocurrio un error al cargar el detalle.') + '</div>';
        });
});