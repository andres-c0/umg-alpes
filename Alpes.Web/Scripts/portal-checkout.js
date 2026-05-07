document.addEventListener('DOMContentLoaded', function () {
    var direccionInput = document.getElementById('coDireccion');
    var metodosContainer = document.getElementById('coMetodosPago');
    var tarjetasContainer = document.getElementById('coTarjetas');
    var summaryContainer = document.getElementById('coSummaryContainer');
    var cuponInput = document.getElementById('coCupon');
    var cuponBtn = document.getElementById('coAplicarCuponBtn');
    var cuponMessage = document.getElementById('coCuponMessage');
    var paymentPreview = document.getElementById('coPaymentPreview');

    if (!direccionInput || !metodosContainer || !tarjetasContainer || !summaryContainer) {
        return;
    }

    var endpointCheckout = '/PortalCliente/ObtenerCheckoutData';
    var endpointValidarCupon = '/PortalCliente/ValidarCuponCheckoutData';
    var endpointConfirmar = '/PortalCliente/ConfirmarPedidoDesdeCheckoutData';

    var checkoutData = null;
    var cuponActual = '';
    var confirmando = false;
    var tabPagoActual = 'tarjetas';

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function normalizarRespuesta(payload) {
        if (!payload) return { ok: false, data: null, message: 'Respuesta vacía del servidor.' };
        if (payload.ok !== undefined) return { ok: payload.ok === true, data: payload.data || null, message: payload.message || payload.mensaje || '' };
        if (payload.success !== undefined) return { ok: payload.success === true, data: payload.data || null, message: payload.message || payload.mensaje || '' };
        return { ok: true, data: payload, message: '' };
    }

    function formatearMoneda(valor, moneda) {
        var numero = Number(valor || 0);
        var codigo = String(moneda || 'GTQ').trim();
        try { return new Intl.NumberFormat('es-GT', { style: 'currency', currency: codigo }).format(numero); }
        catch (error) { return 'Q' + numero.toFixed(2); }
    }

    function mostrarToast(mensaje, tipo) {
        if (window.portalClienteToast) { window.portalClienteToast(mensaje, tipo || 'success'); return; }
        var toast = document.createElement('div');
        toast.className = 'pc-toast pc-toast--' + (tipo || 'success');
        toast.textContent = mensaje;
        document.body.appendChild(toast);
        window.setTimeout(function () { toast.classList.add('show'); }, 20);
        window.setTimeout(function () {
            toast.classList.remove('show');
            window.setTimeout(function () { if (toast.parentNode) toast.parentNode.removeChild(toast); }, 250);
        }, 2800);
    }

    function postJson(url, payload) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json; charset=utf-8', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payloadRespuesta) {
                var respuesta = normalizarRespuesta(payloadRespuesta);
                if (!response.ok || !respuesta.ok) throw new Error(respuesta.message || 'No se pudo procesar la solicitud.');
                return respuesta;
            });
        });
    }

    function getMetodoSeleccionado() { return document.querySelector('input[name="coMetodoPago"]:checked'); }
    function getTarjetaSeleccionada() { return document.querySelector('input[name="coTarjeta"]:checked'); }

    function actualizarVistaTabs() {
        var tabs = document.querySelectorAll('.co-pay-tab');
        var panels = document.querySelectorAll('.co-pay-panel');
        Array.prototype.forEach.call(tabs, function (tab) { tab.classList.toggle('active', tab.getAttribute('data-pay-tab') === tabPagoActual); });
        Array.prototype.forEach.call(panels, function (panel) { panel.classList.toggle('active', panel.getAttribute('data-pay-panel') === tabPagoActual); });
        actualizarPreviewPago();
    }

    function seleccionarTabPago(tab) { tabPagoActual = tab === 'metodos' ? 'metodos' : 'tarjetas'; actualizarVistaTabs(); }

    function actualizarPreviewPago() {
        if (!paymentPreview) return;
        var tarjetaEl = getTarjetaSeleccionada();
        var metodoEl = getMetodoSeleccionado();
        if (tabPagoActual === 'tarjetas' && tarjetaEl) {
            paymentPreview.innerHTML = '<span class="material-icons">credit_card</span><div><strong>' + escapeHtml(tarjetaEl.getAttribute('data-label') || 'Tarjeta seleccionada') + '</strong><small>' + escapeHtml(tarjetaEl.getAttribute('data-subtitle') || 'Se usará esta tarjeta guardada.') + '</small></div><span class="material-icons co-preview-check">check_circle</span>';
            return;
        }
        if (tabPagoActual === 'metodos' && metodoEl) {
            paymentPreview.innerHTML = '<span class="material-icons">payments</span><div><strong>' + escapeHtml(metodoEl.getAttribute('data-label') || 'Método seleccionado') + '</strong><small>Se registrará el pago con este método.</small></div><span class="material-icons co-preview-check">check_circle</span>';
            return;
        }
        paymentPreview.innerHTML = '<span class="material-icons">credit_card</span><div><strong>Selecciona cómo deseas pagar</strong><small>Puedes usar una tarjeta guardada o elegir otro método disponible.</small></div>';
    }

    function inferirMetodoPagoTarjeta() {
        var metodos = (checkoutData && checkoutData.MetodosPago) ? checkoutData.MetodosPago : [];
        for (var i = 0; i < metodos.length; i += 1) {
            var nombre = String(metodos[i].Nombre || '').toLowerCase();
            if (nombre.indexOf('tarjeta') >= 0 || nombre.indexOf('credito') >= 0 || nombre.indexOf('crédito') >= 0 || nombre.indexOf('debito') >= 0 || nombre.indexOf('débito') >= 0) return Number(metodos[i].MetodoPagoId || 0);
        }
        return metodos.length ? Number(metodos[0].MetodoPagoId || 0) : 0;
    }

    function render(data) {
        checkoutData = data || {};
        direccionInput.value = (data.Cliente && data.Cliente.Direccion) ? data.Cliente.Direccion : '';
        renderMetodos(data.MetodosPago || []);
        renderTarjetas(data.Tarjetas || []);
        seleccionarTabPago((data.Tarjetas || []).length ? 'tarjetas' : 'metodos');
        renderSummary(data);
    }

    function renderMetodos(metodos) {
        var htmlMetodos = '';
        for (var i = 0; i < metodos.length; i += 1) {
            var metodo = metodos[i];
            htmlMetodos += '<label class="co-method-option co-selectable-row"><input type="radio" name="coMetodoPago" value="' + escapeHtml(metodo.MetodoPagoId) + '" data-label="' + escapeHtml(metodo.Nombre || 'Método de pago') + '"' + (i === 0 ? ' checked' : '') + ' /><span class="material-icons co-method-icon">payments</span><span class="co-method-text"><strong>' + escapeHtml(metodo.Nombre || 'Método de pago') + '</strong><small>Seleccionar este método para registrar el pago.</small></span><span class="material-icons co-selected-icon">check_circle</span></label>';
        }
        metodosContainer.innerHTML = htmlMetodos || '<div class="co-empty-inline co-empty-payment"><span class="material-icons">payments</span><strong>Sin métodos disponibles</strong><small>No hay métodos de pago activos en este momento.</small></div>';
        actualizarPreviewPago();
    }

    function renderTarjetas(tarjetas) {
        var htmlTarjetas = '', algunaMarcada = false;
        for (var i = 0; i < tarjetas.length; i += 1) {
            var tarjeta = tarjetas[i];
            var marca = tarjeta.Marca || 'Tarjeta';
            var subtitulo = tarjeta.Alias || tarjeta.Titular || 'Tarjeta guardada';
            var marcaClase = String(marca).toLowerCase().indexOf('master') >= 0 ? 'co-card-option--master' : 'co-card-option--visa';
            var checked = '';
            if (tarjeta.EsPredeterminada === true && !algunaMarcada) { checked = ' checked'; algunaMarcada = true; }
            htmlTarjetas += '<label class="co-card-option co-selectable-row ' + marcaClase + '"><input type="radio" name="coTarjeta" value="' + escapeHtml(tarjeta.TarjetaClienteId) + '" data-label="' + escapeHtml(marca + ' ' + (tarjeta.NumeroMascarado || '****')) + '" data-subtitle="' + escapeHtml(subtitulo) + '"' + checked + ' /><span class="material-icons co-card-icon">credit_card</span><div class="co-card-option-body"><strong>' + escapeHtml(marca + ' ' + (tarjeta.NumeroMascarado || '****')) + '</strong><span>' + escapeHtml(subtitulo) + '</span>' + (tarjeta.EsPredeterminada === true ? '<small>Predeterminada</small>' : '<small>Toca para seleccionar</small>') + '</div><span class="material-icons co-selected-icon">check_circle</span></label>';
        }
        tarjetasContainer.innerHTML = htmlTarjetas || '<div class="co-empty-inline co-empty-payment"><span class="material-icons">credit_card_off</span><strong>No tienes tarjetas guardadas</strong><small>Agrega una tarjeta o usa la pestaña Otros métodos.</small></div>';
        if (tarjetas.length && !algunaMarcada) {
            var primera = tarjetasContainer.querySelector('input[name="coTarjeta"]');
            if (primera) primera.checked = true;
        }
        actualizarPreviewPago();
    }

    function renderSummary(data) {
        var moneda = data.Moneda || 'GTQ', items = data.Items || [], htmlItems = '';
        if (!items.length) {
            summaryContainer.innerHTML = '<div class="co-summary-title">Resumen del pedido</div><div class="co-empty-inline">Tu carrito está vacío.</div><a href="/PortalCliente/Busqueda" class="co-primary-btn co-primary-btn--full">Volver al catálogo</a>';
            return;
        }
        for (var i = 0; i < items.length; i += 1) {
            var item = items[i];
            htmlItems += '<div class="co-summary-item"><span>' + escapeHtml((item.Nombre || 'Producto') + ' x' + item.Cantidad) + '</span><strong>' + escapeHtml(formatearMoneda(item.SubtotalLinea, moneda)) + '</strong></div>';
        }
        summaryContainer.innerHTML = '<div class="co-summary-title">Resumen del pedido</div>' + htmlItems + '<div class="co-summary-row"><span>Subtotal</span><strong>' + escapeHtml(formatearMoneda(data.Subtotal, moneda)) + '</strong></div><div class="co-summary-row"><span>IVA 12%</span><strong>' + escapeHtml(formatearMoneda(data.Impuesto, moneda)) + '</strong></div><div class="co-summary-row"><span>Descuento</span><strong>' + escapeHtml(formatearMoneda(data.Descuento, moneda)) + '</strong></div><div class="co-summary-row co-summary-row--total"><span>Total</span><strong>' + escapeHtml(formatearMoneda(data.Total, moneda)) + '</strong></div><button type="button" id="coConfirmarBtn" class="co-primary-btn co-primary-btn--full">CONFIRMAR PEDIDO</button><div class="co-safe-note">Tu pedido se registrará en ordenes, detalle, pago y envío usando la base de datos.</div>';
    }

    function cargarCheckout() {
        summaryContainer.innerHTML = '<div class="co-loading">Cargando checkout...</div>';
        metodosContainer.innerHTML = '<div class="co-loading co-loading--small">Cargando métodos...</div>';
        tarjetasContainer.innerHTML = '<div class="co-loading co-loading--small">Cargando tarjetas...</div>';
        fetch(endpointCheckout + '?_=' + Date.now(), { method: 'GET', credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (response) { return response.json().catch(function () { return null; }).then(function (payload) { if (!response.ok) { var err = normalizarRespuesta(payload); throw new Error(err.message || 'No se pudo cargar el checkout.'); } return payload; }); })
            .then(function (payload) { var respuesta = normalizarRespuesta(payload); if (!respuesta.ok || !respuesta.data) throw new Error(respuesta.message || 'No se pudo cargar el checkout.'); render(respuesta.data); })
            .catch(function (error) { summaryContainer.innerHTML = '<div class="co-loading">' + escapeHtml(error.message || 'Error al cargar checkout.') + '</div>'; metodosContainer.innerHTML = '<div class="co-empty-inline">No disponible.</div>'; tarjetasContainer.innerHTML = '<div class="co-empty-inline">No disponible.</div>'; mostrarToast(error.message || 'Error al cargar checkout.', 'error'); });
    }

    document.addEventListener('click', function (e) {
        var tab = e.target.closest('.co-pay-tab');
        if (tab) { seleccionarTabPago(tab.getAttribute('data-pay-tab')); return; }
    });

    document.addEventListener('change', function (e) { if (e.target && (e.target.name === 'coTarjeta' || e.target.name === 'coMetodoPago')) actualizarPreviewPago(); });

    if (cuponBtn) {
        cuponBtn.addEventListener('click', function () {
            var codigo = (cuponInput ? cuponInput.value : '').trim();
            if (!cuponMessage) return;
            if (codigo === '') { cuponMessage.textContent = 'Ingresa un código.'; cuponMessage.className = 'co-coupon-message show co-coupon-message--error'; return; }
            cuponBtn.disabled = true; cuponBtn.textContent = 'Validando...';
            postJson(endpointValidarCupon, { codigo: codigo })
                .then(function (respuesta) { cuponActual = codigo; cuponMessage.textContent = respuesta.message || 'Cupón validado.'; cuponMessage.className = 'co-coupon-message show'; mostrarToast('Cupón validado.', 'success'); })
                .catch(function (error) { cuponActual = ''; cuponMessage.textContent = error.message || 'No se pudo validar el cupón.'; cuponMessage.className = 'co-coupon-message show co-coupon-message--error'; mostrarToast(error.message || 'No se pudo validar el cupón.', 'error'); })
                .finally(function () { cuponBtn.disabled = false; cuponBtn.textContent = 'Aplicar'; });
        });
    }

    summaryContainer.addEventListener('click', function (e) {
        var btn = e.target.closest('#coConfirmarBtn');
        if (!btn || confirmando) return;
        var metodoEl = getMetodoSeleccionado(), tarjetaEl = getTarjetaSeleccionada(), direccion = direccionInput.value.trim();
        var metodoPagoId = 0, tarjetaClienteId = 0;
        if (!checkoutData || !(checkoutData.Items || []).length) { mostrarToast('Tu carrito está vacío.', 'error'); return; }
        if (direccion === '') { mostrarToast('Ingresa la dirección de entrega.', 'error'); direccionInput.focus(); return; }
        if (tabPagoActual === 'tarjetas') {
            if (!tarjetaEl) { mostrarToast('Selecciona una tarjeta o usa Otros métodos.', 'error'); return; }
            tarjetaClienteId = Number(tarjetaEl.value || 0);
            metodoPagoId = inferirMetodoPagoTarjeta();
        } else {
            if (!metodoEl) { mostrarToast('Selecciona un método de pago.', 'error'); return; }
            metodoPagoId = Number(metodoEl.value || 0);
        }
        if (metodoPagoId <= 0) { mostrarToast('No hay método de pago válido para confirmar.', 'error'); return; }
        confirmando = true; btn.disabled = true; btn.textContent = 'CONFIRMANDO...';
        postJson(endpointConfirmar, { direccion: direccion, metodoPagoId: metodoPagoId, tarjetaClienteId: tarjetaClienteId, codigoCupon: cuponActual })
            .then(function (respuesta) { mostrarToast(respuesta.message || 'Pedido confirmado correctamente.', 'success'); window.setTimeout(function () { if (respuesta.data && respuesta.data.OrdenVentaId) { window.location.href = '/PortalCliente/DetalleOrden?id=' + encodeURIComponent(respuesta.data.OrdenVentaId); return; } window.location.href = '/PortalCliente/MisOrdenes'; }, 700); })
            .catch(function (error) { mostrarToast(error.message || 'No se pudo confirmar el pedido.', 'error'); })
            .finally(function () { confirmando = false; btn.disabled = false; btn.textContent = 'CONFIRMAR PEDIDO'; });
    });

    cargarCheckout();
});
