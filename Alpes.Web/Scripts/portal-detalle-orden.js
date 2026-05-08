document.addEventListener('DOMContentLoaded', function () {
    var page = document.getElementById('odPage');
    var content = document.getElementById('odContent');

    if (!page || !content) {
        return;
    }

    var orderId = page.getAttribute('data-order-id');
    if (!orderId || orderId === '0') {
        content.innerHTML = '<div class="od-error">No se recibió el id del pedido.</div>';
        return;
    }

    var endpoint = '/PortalCliente/ObtenerDetalleOrdenData?id=' + encodeURIComponent(orderId);

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

    function formatearMoneda(valor, moneda) {
        var numero = Number(valor || 0);
        var codigo = String(moneda || 'GTQ').trim().toUpperCase();
        try {
            return new Intl.NumberFormat('es-GT', {
                style: 'currency',
                currency: codigo
            }).format(numero);
        } catch (error) {
            return 'Q' + numero.toFixed(2);
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

        if (normalizado === 'EN CAMINO') {
            return 'od-status od-status--info';
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
                    : '<div class="od-item-no-image"><i class="fa-solid fa-chair"></i></div>')
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
            htmlItems = '<div class="od-empty-lines">No hay productos asociados a este pedido.</div>';
        }

        content.innerHTML = ''
            + '<section class="od-order-header">'
            + '  <div>'
            + '      <span class="orders-kicker">Pedido</span>'
            + '      <h2>' + escapeHtml(data.NumOrden || ('ORD-' + data.OrdenVentaId)) + '</h2>'
            + '      <p>' + escapeHtml(data.FechaOrdenTexto || '') + '</p>'
            + '  </div>'
            + '  <div class="od-order-total">'
            + '      <span>Total pagado</span>'
            + '      <strong>' + escapeHtml(formatearMoneda(data.Total, data.Moneda)) + '</strong>'
            + '      <em class="' + escapeHtml(obtenerClaseEstado(data.EstadoUi)) + '">' + escapeHtml(data.EstadoUi || 'PENDIENTE') + '</em>'
            + '  </div>'
            + '</section>'
            + '<section class="od-content-grid">'
            + '  <div class="od-panel od-panel-wide">'
            + '      <div class="od-section-title">Productos del pedido</div>'
            + htmlItems
            + '  </div>'
            + '  <aside class="od-panel">'
            + '      <div class="od-section-title">Resumen</div>'
            + '      <div class="od-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(data.Subtotal, data.Moneda)) + '</strong></div>'
            + '      <div class="od-row"><span>IVA 12%</span><strong>' + escapeHtml(formatearMoneda(data.Impuesto, data.Moneda)) + '</strong></div>'
            + '      <div class="od-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(data.Descuento, data.Moneda)) + '</strong></div>'
            + '      <div class="od-row od-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(data.Total, data.Moneda)) + '</strong></div>'
            + '      <div class="od-divider"></div>'
            + '      <div class="od-info-block"><strong>Dirección de entrega</strong><span>' + escapeHtml(data.Direccion || 'No disponible') + '</span></div>'
            + '      <div class="od-info-block"><strong>Código de tracking</strong><span>' + escapeHtml(data.TrackingCodigo || 'No disponible') + '</span></div>'
            + '      <div class="od-info-block"><strong>Observaciones</strong><span>' + escapeHtml(data.Observaciones || 'Sin observaciones') + '</span></div>'
            + '  </aside>'
            + '</section>'
            + '<div class="od-actions">'
            + '  <a class="od-btn od-btn--ghost" href="/PortalCliente/MisOrdenes">Volver</a>'
            + '  <a class="od-btn od-btn--primary" href="/PortalCliente/Tracking?ordenVentaId=' + encodeURIComponent(data.OrdenVentaId) + '">Ver seguimiento</a>'
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
                throw new Error('No se pudo obtener el detalle del pedido.');
            }
            return response.json();
        })
        .then(function (payload) {
            var respuesta = normalizarRespuesta(payload);

            if (!respuesta.ok || !respuesta.data) {
                throw new Error(respuesta.message || 'No se pudo obtener el detalle del pedido.');
            }

            render(respuesta.data);
        })
        .catch(function (error) {
            content.innerHTML = '<div class="od-error">' + escapeHtml(error.message || 'Ocurrió un error al cargar el detalle.') + '</div>';
        });
});
