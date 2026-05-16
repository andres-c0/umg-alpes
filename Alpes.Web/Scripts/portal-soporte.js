(function () {
    'use strict';

    var chat = document.getElementById('soporteChat');
    var input = document.getElementById('soporteInput');
    var send = document.getElementById('soporteEnviar');
    var quickButtons = document.querySelectorAll('.pc-chip-btn');
    var resumen = { pedidos: 0, activos: 0, carrito: 0 };

    function normalize(value) {
        return String(value || '').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').trim();
    }

    function getTimeNow() {
        var now = new Date();
        return now.getHours().toString().padStart(2, '0') + ':' + now.getMinutes().toString().padStart(2, '0');
    }

    function addMessage(text, type) {
        if (!chat) return;
        var row = document.createElement('div');
        row.className = 'pc-support-message ' + (type === 'user' ? 'user' : 'bot');
        row.innerHTML = '<div class="pc-support-bubble"></div><span>' + getTimeNow() + '</span>';
        row.querySelector('.pc-support-bubble').textContent = text;
        chat.appendChild(row);
        chat.scrollTop = chat.scrollHeight;
    }

    function addTyping() {
        var row = document.createElement('div');
        row.className = 'pc-support-message bot';
        row.id = 'soporteTyping';
        row.innerHTML = '<div class="pc-support-bubble pc-support-typing"><span></span><span></span><span></span></div>';
        chat.appendChild(row);
        chat.scrollTop = chat.scrollHeight;
    }

    function removeTyping() {
        var typing = document.getElementById('soporteTyping');
        if (typing) typing.remove();
    }

    function getReply(message) {
        var text = normalize(message);
        if (text.indexOf('hola') >= 0 || text.indexOf('buenas') >= 0) {
            return 'Hola, con gusto te ayudo. Puedo orientarte sobre pedidos, pagos, envíos, carrito o descuentos.';
        }
        if (text.indexOf('pedido') >= 0 || text.indexOf('orden') >= 0 || text.indexOf('compra') >= 0) {
            return 'Actualmente tienes ' + resumen.pedidos + ' pedido(s), de los cuales ' + resumen.activos + ' están activos. Puedes entrar a Mis pedidos para ver detalle y tracking.';
        }
        if (text.indexOf('carrito') >= 0 || text.indexOf('producto') >= 0) {
            return 'Tu carrito tiene ' + resumen.carrito + ' producto(s). Puedes revisarlo desde el botón Carrito antes de confirmar la compra.';
        }
        if (text.indexOf('envio') >= 0 || text.indexOf('entrega') >= 0 || text.indexOf('direccion') >= 0) {
            return 'Los envíos se consultan desde Tracking. También puedes actualizar tu dirección desde Mi perfil antes de confirmar un pedido.';
        }
        if (text.indexOf('pago') >= 0 || text.indexOf('tarjeta') >= 0 || text.indexOf('cobro') >= 0) {
            return 'Puedes administrar tus tarjetas desde Mis tarjetas. Al confirmar pedido, el pago queda asociado a tu orden.';
        }
        if (text.indexOf('cupon') >= 0 || text.indexOf('descuento') >= 0 || text.indexOf('promocion') >= 0) {
            return 'Los cupones disponibles se validan durante el checkout. Si hay promociones activas, aparecerán en el resumen.';
        }
        if (text.indexOf('agente') >= 0 || text.indexOf('asesor') >= 0 || text.indexOf('persona') >= 0) {
            return 'Un agente puede apoyarte en horario laboral. Mientras tanto, puedes dejar tu consulta aquí.';
        }
        return 'Gracias por tu mensaje. Para ayudarte mejor, dime si tu consulta es sobre pedidos, envíos, pagos, carrito o perfil.';
    }

    function sendMessage(message) {
        if (!message || !message.trim()) return;
        addMessage(message.trim(), 'user');
        if (input) input.value = '';
        addTyping();
        setTimeout(function () {
            removeTyping();
            addMessage(getReply(message), 'bot');
        }, 550);
    }

    function loadResumen() {
        Promise.all([
            fetch('/PortalCliente/ObtenerMisOrdenesData', { credentials: 'same-origin' }).then(function (r) { return r.json(); }).catch(function () { return {}; }),
            fetch('/PortalCliente/ObtenerCarritoData', { credentials: 'same-origin' }).then(function (r) { return r.json(); }).catch(function () { return {}; })
        ]).then(function (res) {
            var ordenes = Array.isArray(res[0].data) ? res[0].data : [];
            var carrito = res[1].data || [];
            var items = Array.isArray(carrito.items) ? carrito.items : (Array.isArray(carrito) ? carrito : []);
            resumen.pedidos = ordenes.length;
            resumen.activos = ordenes.filter(function (o) {
                var e = normalize(o.Estado || o.estado || o.EstadoNormalizado || o.estadoNormalizado);
                return e.indexOf('entregado') < 0 && e.indexOf('cancel') < 0 && e.indexOf('finalizado') < 0;
            }).length;
            resumen.carrito = items.length;
        });
    }

    if (send) send.addEventListener('click', function () { sendMessage(input ? input.value : ''); });
    if (input) input.addEventListener('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); sendMessage(input.value); } });
    quickButtons.forEach(function (btn) { btn.addEventListener('click', function () { sendMessage(btn.getAttribute('data-message') || btn.textContent); }); });
    document.addEventListener('DOMContentLoaded', loadResumen);
}());
