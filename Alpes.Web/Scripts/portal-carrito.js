document.addEventListener('DOMContentLoaded', function () {
    var itemsContainer = document.getElementById('ccItemsContainer');
    var summaryContainer = document.getElementById('ccSummaryContainer');

    if (!itemsContainer || !summaryContainer) {
        return;
    }

    var endpointCarrito = '/PortalCliente/ObtenerCarritoData';
    var endpointActualizar = '/PortalCliente/ActualizarCantidadCarritoData';
    var endpointEliminar = '/PortalCliente/EliminarDelCarritoData';

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

    function postJson(url, payload) {
        return fetch(url, {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(payload)
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo procesar la solicitud.');
                }
                return response.json();
            })
            .then(function (payloadRespuesta) {
                var respuesta = normalizarRespuesta(payloadRespuesta);
                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudo procesar la solicitud.');
                }
                return respuesta;
            });
    }

    function render(data) {
        var items = data.Items || [];
        var moneda = data.Moneda || 'GTQ';

        if (!items.length) {
            itemsContainer.innerHTML = ''
                + '<div class="cc-empty">'
                + '  <div class="cc-empty-title">Tu carrito esta vacio</div>'
                + '  <div class="cc-empty-text">Agrega productos desde el catalogo o el detalle del producto.</div>'
                + '  <a class="cc-primary-btn" href="/PortalCliente/Index">Ir al catalogo</a>'
                + '</div>';

            summaryContainer.innerHTML = ''
                + '<div class="cc-summary-title">Resumen</div>'
                + '<div class="cc-summary-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
                + '<div class="cc-summary-row"><span>IVA</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
                + '<div class="cc-summary-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
                + '<div class="cc-summary-row cc-summary-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>';

            return;
        }

        var html = '';
        var i;

        for (i = 0; i < items.length; i += 1) {
            var item = items[i];
            var imagen = String(item.ImagenUrl || '').trim();

            html += ''
                + '<div class="cc-item" data-id="' + escapeHtml(item.CarritoDetId) + '">'
                + '  <div class="cc-item-image">'
                + (imagen !== ''
                    ? '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(item.Nombre || 'Producto') + '">'
                    : '<div class="cc-item-no-image">Sin imagen</div>')
                + '  </div>'
                + '  <div class="cc-item-body">'
                + '      <div class="cc-item-name">' + escapeHtml(item.Nombre || 'Producto') + '</div>'
                + '      <div class="cc-item-meta">' + escapeHtml(item.Referencia || '') + ' ' + escapeHtml(item.Material || '') + ' ' + escapeHtml(item.Color || '') + '</div>'
                + '      <div class="cc-item-price">' + escapeHtml(formatearMoneda(item.PrecioUnitario, moneda)) + '</div>'
                + '      <div class="cc-qty-row">'
                + '          <button type="button" class="cc-qty-btn" data-action="decrease" data-id="' + escapeHtml(item.CarritoDetId) + '" data-cantidad="' + escapeHtml(item.Cantidad) + '">-</button>'
                + '          <span class="cc-qty-value">' + escapeHtml(item.Cantidad) + '</span>'
                + '          <button type="button" class="cc-qty-btn" data-action="increase" data-id="' + escapeHtml(item.CarritoDetId) + '" data-cantidad="' + escapeHtml(item.Cantidad) + '">+</button>'
                + '      </div>'
                + '  </div>'
                + '  <div class="cc-item-side">'
                + '      <div class="cc-item-subtotal">' + escapeHtml(formatearMoneda(item.SubtotalLinea, moneda)) + '</div>'
                + '      <button type="button" class="cc-remove-btn" data-action="remove" data-id="' + escapeHtml(item.CarritoDetId) + '">Eliminar</button>'
                + '  </div>'
                + '</div>';
        }

        itemsContainer.innerHTML = html;

        summaryContainer.innerHTML = ''
            + '<div class="cc-summary-title">Resumen</div>'
            + '<div class="cc-summary-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(data.Subtotal, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row"><span>IVA</span><strong>' + escapeHtml(formatearMoneda(data.Impuesto, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(data.Descuento, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row cc-summary-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(data.Total, moneda)) + '</strong></div>'
            + '<a href="/PortalCliente/Checkout" class="cc-primary-btn cc-primary-btn--full">Proceder al pago</a>';
    }

    function cargarCarrito() {
        itemsContainer.innerHTML = '<div class="cc-loading">Cargando carrito...</div>';
        summaryContainer.innerHTML = '<div class="cc-loading">Calculando resumen...</div>';

        fetch(endpointCarrito, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo cargar el carrito.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok || !respuesta.data) {
                    throw new Error(respuesta.message || 'No se pudo cargar el carrito.');
                }

                render(respuesta.data);
            })
            .catch(function (error) {
                itemsContainer.innerHTML = '<div class="cc-loading">' + escapeHtml(error.message || 'Error al cargar el carrito.') + '</div>';
                summaryContainer.innerHTML = '<div class="cc-loading">No disponible</div>';
            });
    }

    itemsContainer.addEventListener('click', function (e) {
        var button = e.target.closest('button[data-action]');
        if (!button) {
            return;
        }

        var action = button.getAttribute('data-action');
        var id = Number(button.getAttribute('data-id') || 0);
        var cantidad = Number(button.getAttribute('data-cantidad') || 0);

        if (id <= 0) {
            return;
        }

        if (action === 'increase') {
            postJson(endpointActualizar, {
                carritoDetId: id,
                cantidad: cantidad + 1
            }).then(function () {
                cargarCarrito();
            }).catch(function (error) {
                alert(error.message || 'No se pudo actualizar la cantidad.');
            });
        }

        if (action === 'decrease') {
            postJson(endpointActualizar, {
                carritoDetId: id,
                cantidad: cantidad - 1
            }).then(function () {
                cargarCarrito();
            }).catch(function (error) {
                alert(error.message || 'No se pudo actualizar la cantidad.');
            });
        }

        if (action === 'remove') {
            postJson(endpointEliminar, {
                carritoDetId: id
            }).then(function () {
                cargarCarrito();
            }).catch(function (error) {
                alert(error.message || 'No se pudo eliminar el producto.');
            });
        }
    });

    cargarCarrito();
});