document.addEventListener('DOMContentLoaded', function () {
    var direccionInput = document.getElementById('coDireccion');
    var metodosContainer = document.getElementById('coMetodosPago');
    var tarjetasContainer = document.getElementById('coTarjetas');
    var summaryContainer = document.getElementById('coSummaryContainer');
    var cuponInput = document.getElementById('coCupon');
    var cuponBtn = document.getElementById('coAplicarCuponBtn');
    var cuponMessage = document.getElementById('coCuponMessage');

    if (!direccionInput || !metodosContainer || !tarjetasContainer || !summaryContainer) {
        return;
    }

    var endpointCheckout = '/PortalCliente/ObtenerCheckoutData';
    var endpointValidarCupon = '/PortalCliente/ValidarCuponCheckoutData';
    var endpointConfirmar = '/PortalCliente/ConfirmarPedidoDesdeCheckoutData';

    var checkoutData = null;
    var cuponActual = '';

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
        checkoutData = data;

        direccionInput.value = (data.Cliente && data.Cliente.Direccion) ? data.Cliente.Direccion : '';

        var htmlMetodos = '';
        var i;
        for (i = 0; i < (data.MetodosPago || []).length; i += 1) {
            var metodo = data.MetodosPago[i];
            htmlMetodos += ''
                + '<label class="co-method-option">'
                + '  <input type="radio" name="coMetodoPago" value="' + escapeHtml(metodo.MetodoPagoId) + '"' + (i === 0 ? ' checked' : '') + ' />'
                + '  <span>' + escapeHtml(metodo.Nombre || 'Metodo de pago') + '</span>'
                + '</label>';
        }
        metodosContainer.innerHTML = htmlMetodos !== '' ? htmlMetodos : '<div class="co-empty-inline">No hay metodos de pago disponibles.</div>';

        var htmlTarjetas = '';
        var predeterminadaId = 0;

        for (i = 0; i < (data.Tarjetas || []).length; i += 1) {
            var tarjeta = data.Tarjetas[i];

            if (tarjeta.EsPredeterminada === true) {
                predeterminadaId = tarjeta.TarjetaClienteId;
            }

            htmlTarjetas += ''
                + '<label class="co-card-option">'
                + '  <input type="radio" name="coTarjeta" value="' + escapeHtml(tarjeta.TarjetaClienteId) + '"' + (tarjeta.EsPredeterminada === true ? ' checked' : '') + ' />'
                + '  <div class="co-card-option-body">'
                + '      <strong>' + escapeHtml(tarjeta.Marca || 'Tarjeta') + '</strong>'
                + '      <span>' + escapeHtml(tarjeta.NumeroMascarado || '') + '</span>'
                + '      <small>' + escapeHtml(tarjeta.Alias || tarjeta.Titular || '') + '</small>'
                + '  </div>'
                + '</label>';
        }

        tarjetasContainer.innerHTML = htmlTarjetas !== '' ? htmlTarjetas : '<div class="co-empty-inline">No hay tarjetas guardadas. Puedes continuar usando solo el metodo de pago.</div>';

        renderSummary(data);
    }

    function renderSummary(data) {
        var moneda = data.Moneda || 'GTQ';
        var items = data.Items || [];
        var htmlItems = '';
        var i;

        for (i = 0; i < items.length; i += 1) {
            var item = items[i];
            htmlItems += ''
                + '<div class="co-summary-item">'
                + '  <span>' + escapeHtml((item.Nombre || 'Producto') + ' x' + item.Cantidad) + '</span>'
                + '  <strong>' + escapeHtml(formatearMoneda(item.SubtotalLinea, moneda)) + '</strong>'
                + '</div>';
        }

        summaryContainer.innerHTML = ''
            + '<div class="co-summary-title">Resumen del pedido</div>'
            + htmlItems
            + '<div class="co-summary-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(data.Subtotal, moneda)) + '</strong></div>'
            + '<div class="co-summary-row"><span>IVA</span><strong>' + escapeHtml(formatearMoneda(data.Impuesto, moneda)) + '</strong></div>'
            + '<div class="co-summary-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(data.Descuento, moneda)) + '</strong></div>'
            + '<div class="co-summary-row co-summary-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(data.Total, moneda)) + '</strong></div>'
            + '<button type="button" id="coConfirmarBtn" class="co-primary-btn co-primary-btn--full">CONFIRMAR PEDIDO</button>';
    }

    function cargarCheckout() {
        summaryContainer.innerHTML = '<div class="co-loading">Cargando checkout...</div>';

        fetch(endpointCheckout, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo cargar el checkout.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok || !respuesta.data) {
                    throw new Error(respuesta.message || 'No se pudo cargar el checkout.');
                }

                render(respuesta.data);
            })
            .catch(function (error) {
                summaryContainer.innerHTML = '<div class="co-loading">' + escapeHtml(error.message || 'Error al cargar checkout.') + '</div>';
            });
    }

    if (cuponBtn) {
        cuponBtn.addEventListener('click', function () {
            var codigo = (cuponInput ? cuponInput.value : '').trim();

            if (codigo === '') {
                cuponMessage.textContent = 'Ingresa un codigo.';
                cuponMessage.className = 'co-coupon-message show';
                return;
            }

            postJson(endpointValidarCupon, { codigo: codigo })
                .then(function (respuesta) {
                    cuponActual = codigo;
                    cuponMessage.textContent = respuesta.message || 'Cupon validado.';
                    cuponMessage.className = 'co-coupon-message show';
                })
                .catch(function (error) {
                    cuponActual = '';
                    cuponMessage.textContent = error.message || 'No se pudo validar el cupon.';
                    cuponMessage.className = 'co-coupon-message show co-coupon-message--error';
                });
        });
    }

    summaryContainer.addEventListener('click', function (e) {
        var btn = e.target.closest('#coConfirmarBtn');
        if (!btn) {
            return;
        }

        var metodoEl = document.querySelector('input[name="coMetodoPago"]:checked');
        var tarjetaEl = document.querySelector('input[name="coTarjeta"]:checked');

        if (!metodoEl) {
            alert('Selecciona un metodo de pago.');
            return;
        }

        btn.disabled = true;
        btn.textContent = 'CONFIRMANDO...';

        postJson(endpointConfirmar, {
            direccion: direccionInput.value.trim(),
            metodoPagoId: Number(metodoEl.value),
            tarjetaClienteId: tarjetaEl ? Number(tarjetaEl.value) : 0,
            codigoCupon: cuponActual
        })
            .then(function (respuesta) {
                alert(respuesta.message || 'Pedido confirmado correctamente.');

                if (respuesta.data && respuesta.data.OrdenVentaId) {
                    window.location.href = '/PortalCliente/DetalleOrden?id=' + encodeURIComponent(respuesta.data.OrdenVentaId);
                    return;
                }

                window.location.href = '/PortalCliente/MisOrdenes';
            })
            .catch(function (error) {
                alert(error.message || 'No se pudo confirmar el pedido.');
            })
            .finally(function () {
                btn.disabled = false;
                btn.textContent = 'CONFIRMAR PEDIDO';
            });
    });

    cargarCheckout();
});