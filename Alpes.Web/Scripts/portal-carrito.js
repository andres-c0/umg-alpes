document.addEventListener('DOMContentLoaded', function () {
    var itemsContainer = document.getElementById('ccItemsContainer');
    var summaryContainer = document.getElementById('ccSummaryContainer');

    if (!itemsContainer || !summaryContainer) {
        return;
    }

    var endpointCarrito = '/PortalCliente/ObtenerCarritoData';
    var endpointActualizar = '/PortalCliente/ActualizarCantidadCarritoData';
    var endpointEliminar = '/PortalCliente/EliminarDelCarritoData';
    var isBusy = false;

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
            return 'Q' + numero.toFixed(2);
        }
    }

    function mostrarToast(mensaje, tipo) {
        var toast = document.createElement('div');
        toast.className = 'pc-toast pc-toast--' + (tipo || 'success');
        toast.textContent = mensaje;
        document.body.appendChild(toast);

        window.setTimeout(function () {
            toast.classList.add('show');
        }, 20);

        window.setTimeout(function () {
            toast.classList.remove('show');
            window.setTimeout(function () {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 250);
        }, 2600);
    }

    function notificarCarritoActualizado() {
        try {
            document.dispatchEvent(new CustomEvent('pc:cart-updated'));
        } catch (e) {
            var evt = document.createEvent('Event');
            evt.initEvent('pc:cart-updated', true, true);
            document.dispatchEvent(evt);
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
                return response.json().catch(function () {
                    return null;
                }).then(function (payloadRespuesta) {
                    if (!response.ok) {
                        var normalizadaError = normalizarRespuesta(payloadRespuesta);
                        throw new Error(normalizadaError.message || 'No se pudo procesar la solicitud.');
                    }

                    var respuesta = normalizarRespuesta(payloadRespuesta);
                    if (!respuesta.ok) {
                        throw new Error(respuesta.message || 'No se pudo procesar la solicitud.');
                    }

                    return respuesta;
                });
            });
    }

    function renderEmpty(moneda) {
        itemsContainer.innerHTML = ''
            + '<div class="cc-empty">'
            + '  <div class="cc-empty-icon">shopping_cart</div>'
            + '  <div class="cc-empty-title">Tu carrito esta vacio</div>'
            + '  <div class="cc-empty-text">Agrega productos desde el catalogo para continuar con tu compra.</div>'
            + '  <a class="cc-primary-btn" href="/PortalCliente/Index">Ir al catalogo</a>'
            + '</div>';

        summaryContainer.innerHTML = ''
            + '<div class="cc-summary-title">Resumen</div>'
            + '<div class="cc-summary-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row"><span>IVA 12%</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row cc-summary-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(0, moneda)) + '</strong></div>'
            + '<a href="/PortalCliente/Index" class="cc-secondary-btn cc-primary-btn--full">Seguir comprando</a>';
    }

    function render(data) {
        var items = data.Items || [];
        var countLabel = document.getElementById('ccProductCount');
        if (countLabel) {
            countLabel.textContent = items.length + ' producto' + (items.length === 1 ? '' : 's');
        }
        var moneda = data.Moneda || 'GTQ';

        if (!items.length) {
            renderEmpty(moneda);
            actualizarBadgeCarrito(0);
            return;
        }

        var html = '';
        var totalCantidad = 0;
        var i;

        for (i = 0; i < items.length; i += 1) {
            var item = items[i];
            var imagen = String(item.ImagenUrl || '').trim();
            var cantidad = Number(item.Cantidad || 0);
            totalCantidad += cantidad;

            html += ''
                + '<div class="cc-item" data-id="' + escapeHtml(item.CarritoDetId) + '">'
                + '  <div class="cc-item-image">'
                + (imagen !== ''
                    ? '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(item.Nombre || 'Producto') + '">'
                    : '<div class="cc-item-no-image">chair</div>')
                + '  </div>'
                + '  <div class="cc-item-body">'
                + '      <div class="cc-item-kicker">Muebles de los Alpes</div>'
                + '      <div class="cc-item-name">' + escapeHtml(item.Nombre || 'Producto') + '</div>'
                + '      <div class="cc-item-meta">' + escapeHtml([item.Referencia, item.Material, item.Color].filter(Boolean).join(' · ')) + '</div>'
                + '      <div class="cc-item-price">' + escapeHtml(formatearMoneda(item.PrecioUnitario, moneda)) + '</div>'
                + '      <div class="cc-qty-row" aria-label="Control de cantidad">'
                + '          <button type="button" class="cc-qty-btn" data-action="decrease" data-id="' + escapeHtml(item.CarritoDetId) + '" data-cantidad="' + escapeHtml(cantidad) + '">-</button>'
                + '          <span class="cc-qty-value">' + escapeHtml(cantidad) + '</span>'
                + '          <button type="button" class="cc-qty-btn" data-action="increase" data-id="' + escapeHtml(item.CarritoDetId) + '" data-cantidad="' + escapeHtml(cantidad) + '">+</button>'
                + '      </div>'
                + '  </div>'
                + '  <div class="cc-item-side">'
                + '      <div class="cc-item-label">Subtotal</div>'
                + '      <div class="cc-item-subtotal">' + escapeHtml(formatearMoneda(item.SubtotalLinea, moneda)) + '</div>'
                + '      <button type="button" class="cc-remove-btn" data-action="remove" data-id="' + escapeHtml(item.CarritoDetId) + '">Eliminar</button>'
                + '  </div>'
                + '</div>';
        }

        itemsContainer.innerHTML = html;
        actualizarBadgeCarrito(totalCantidad);

        summaryContainer.innerHTML = ''
            + '<div class="cc-summary-title">Resumen</div>'
            + '<div class="cc-summary-row"><span>Productos</span><strong>' + escapeHtml(totalCantidad) + '</strong></div>'
            + '<div class="cc-summary-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(data.Subtotal, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row"><span>IVA 12%</span><strong>' + escapeHtml(formatearMoneda(data.Impuesto, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(data.Descuento, moneda)) + '</strong></div>'
            + '<div class="cc-summary-row cc-summary-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(data.Total, moneda)) + '</strong></div>'
            + '<a href="/PortalCliente/Checkout" class="cc-primary-btn cc-primary-btn--full">Proceder al pago</a>'
            + '<a href="/PortalCliente/Index" class="cc-secondary-btn cc-primary-btn--full">Seguir comprando</a>';
    }

    function actualizarBadgeCarrito(total) {
        var badges = document.querySelectorAll('[data-cart-count], .js-cart-count');
        Array.prototype.forEach.call(badges, function (badge) {
            badge.textContent = String(total || 0);
            badge.style.display = total > 0 ? '' : 'none';
        });
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
                return response.json().catch(function () {
                    return null;
                }).then(function (payload) {
                    if (!response.ok) {
                        var normalizadaError = normalizarRespuesta(payload);
                        throw new Error(normalizadaError.message || 'No se pudo cargar el carrito.');
                    }
                    return payload;
                });
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
                mostrarToast(error.message || 'Error al cargar el carrito.', 'error');
            });
    }
  
    var btnVaciarCarrito = document.getElementById('ccBtnVaciarCarrito');

    if (btnVaciarCarrito) {
        btnVaciarCarrito.addEventListener('click', function () {
            if (isBusy) return;

            mostrarConfirmacionCarrito('¿Vaciar todo el carrito?', function () {
                var botonesEliminar = itemsContainer.querySelectorAll('button[data-action="remove"]');

                if (!botonesEliminar.length) {
                    mostrarToast('No hay productos para eliminar.', 'error');
                    return;
                }

                isBusy = true;
                btnVaciarCarrito.disabled = true;

                var promesas = Array.prototype.map.call(botonesEliminar, function (btn) {
                    var id = Number(btn.getAttribute('data-id') || 0);
                    return postJson(endpointEliminar, { carritoDetId: id });
                });

                Promise.all(promesas)
                    .then(function () {
                        mostrarToast('Carrito vaciado correctamente.', 'success');
                        notificarCarritoActualizado();
                        cargarCarrito();
                    })
                    .catch(function () {
                        mostrarToast('No se pudo vaciar el carrito.', 'error');
                    })
                    .finally(function () {
                        isBusy = false;
                        btnVaciarCarrito.disabled = false;
                    });
            });
        });
    }
   

    itemsContainer.addEventListener('click', function (e) {
        var button = e.target.closest('button[data-action]');
        if (!button || isBusy) {
            return;
        }

        var action = button.getAttribute('data-action');
        var id = Number(button.getAttribute('data-id') || 0);
        var cantidad = Number(button.getAttribute('data-cantidad') || 0);

        if (id <= 0) {
            return;
        }

        var payload = null;
        var endpoint = endpointActualizar;
        var mensajeOk = 'Carrito actualizado.';

        if (action === 'increase') {
            payload = { carritoDetId: id, cantidad: cantidad + 1 };
        }

        if (action === 'decrease') {
            payload = { carritoDetId: id, cantidad: cantidad - 1 };
        }

        if (action === 'remove') {
            endpoint = endpointEliminar;
            payload = { carritoDetId: id };
            mensajeOk = 'Producto eliminado del carrito.';
        }

        if (!payload) {
            return;
        }

        isBusy = true;
        button.disabled = true;

        postJson(endpoint, payload)
            .then(function (respuesta) {
                mostrarToast(respuesta.message || mensajeOk, 'success');
                notificarCarritoActualizado();
                cargarCarrito();
            })
            .catch(function (error) {
                mostrarToast(error.message || 'No se pudo actualizar el carrito.', 'error');
            })
            .finally(function () {
                isBusy = false;
                button.disabled = false;
            });
    });

    function mostrarConfirmacionCarrito(mensaje, onConfirmar) {
        var overlay = document.createElement('div');
        overlay.className = 'cc-confirm-overlay';

        overlay.innerHTML =
            '<div class="cc-confirm-box">' +
            '<div class="cc-confirm-icon"><i class="bi bi-trash"></i></div>' +
            '<h3>Vaciar carrito</h3>' +
            '<p>' + mensaje + '</p>' +
            '<div class="cc-confirm-actions">' +
            '<button type="button" class="cc-confirm-cancel">Cancelar</button>' +
            '<button type="button" class="cc-confirm-ok">Vaciar</button>' +
            '</div>' +
            '</div>';

        document.body.appendChild(overlay);

        overlay.querySelector('.cc-confirm-cancel').addEventListener('click', function () {
            overlay.remove();
        });

        overlay.querySelector('.cc-confirm-ok').addEventListener('click', function () {
            overlay.remove();

            if (typeof onConfirmar === 'function') {
                onConfirmar();
            }
        });
    }

    cargarCarrito();
});