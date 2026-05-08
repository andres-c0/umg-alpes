(function () {
    'use strict';

    var lista = document.getElementById('notificacionesLista');
    var btnActualizar = document.getElementById('btnActualizarNotificaciones');
    var btnMarcar = document.getElementById('btnMarcarNotificaciones');

    function q(value) {
        var n = Number(value || 0);
        return 'Q' + n.toFixed(2);
    }

    function normalize(value) {
        return String(value || '').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');
    }

    function getId(item, keys) {
        for (var i = 0; i < keys.length; i += 1) {
            if (item && item[keys[i]] !== undefined && item[keys[i]] !== null) return item[keys[i]];
        }
        return '';
    }

    function getFecha(item) {
        var value = getId(item, ['FechaOrden', 'fechaOrden', 'Fecha', 'fecha', 'CreatedAt', 'createdAt']);
        if (!value) return 'Fecha no disponible';
        try {
            return new Date(value).toLocaleDateString('es-GT');
        } catch (e) {
            return String(value);
        }
    }

    function apiGet(url) {
        return fetch(url, { credentials: 'same-origin' })
            .then(function (r) { return r.json(); })
            .catch(function () { return { ok: false, data: [] }; });
    }

    function buildNotifications(ordenes, carrito) {
        var items = [];
        var cartItems = Array.isArray(carrito && carrito.items) ? carrito.items : (Array.isArray(carrito) ? carrito : []);

        if (cartItems.length > 0) {
            items.push({
                type: 'cart',
                icon: 'bi-cart3',
                title: 'Tienes productos pendientes en el carrito',
                text: 'Puedes continuar tu compra cuando quieras. Productos: ' + cartItems.length + '.',
                time: 'Carrito activo',
                url: '/PortalCliente/Carrito',
                active: true
            });
        }

        ordenes.forEach(function (orden) {
            var estado = normalize(getId(orden, ['Estado', 'estado', 'EstadoOrden', 'estadoOrden', 'EstadoNormalizado', 'estadoNormalizado']));
            var numero = getId(orden, ['NumeroOrden', 'numeroOrden', 'OrdenCodigo', 'ordenCodigo', 'CodigoOrden', 'codigoOrden', 'OrdenVentaId', 'ordenVentaId']);
            var total = Number(getId(orden, ['Total', 'total', 'TotalOrden', 'totalOrden']) || 0);
            var ordenId = getId(orden, ['OrdenVentaId', 'ordenVentaId', 'ORDEN_VENTA_ID', 'OrdenId', 'ordenId']);
            var fecha = getFecha(orden);

            if (estado.indexOf('entregado') >= 0 || estado.indexOf('finalizado') >= 0) {
                items.push({
                    type: 'delivered',
                    icon: 'bi-check-circle',
                    title: 'Pedido entregado con éxito',
                    text: 'Tu pedido ' + numero + ' fue entregado. Total: ' + q(total) + '.',
                    time: fecha,
                    url: ordenId ? '/PortalCliente/DetalleOrden/' + ordenId : '/PortalCliente/MisOrdenes',
                    active: false
                });
            } else if (estado.indexOf('cancel') >= 0) {
                items.push({
                    type: 'cancelled',
                    icon: 'bi-x-circle',
                    title: 'Pedido cancelado',
                    text: 'Tu pedido ' + numero + ' aparece cancelado.',
                    time: fecha,
                    url: ordenId ? '/PortalCliente/DetalleOrden/' + ordenId : '/PortalCliente/MisOrdenes',
                    active: false
                });
            } else {
                items.push({
                    type: 'active',
                    icon: 'bi-truck',
                    title: 'Pedido activo',
                    text: 'Tu pedido ' + numero + ' está en proceso. Total: ' + q(total) + '.',
                    time: fecha,
                    url: ordenId ? '/PortalCliente/Tracking?ordenVentaId=' + ordenId : '/PortalCliente/MisOrdenes',
                    active: true
                });
            }
        });

        if (items.length === 0) {
            items.push({
                type: 'info',
                icon: 'bi-stars',
                title: 'Sin novedades por ahora',
                text: 'Cuando tengas pedidos, envíos o productos en carrito, aparecerán aquí.',
                time: 'Ahora',
                url: '/PortalCliente/Index#catalogo',
                active: false
            });
        }

        return items;
    }

    function render(items) {
        if (!lista) return;
        lista.innerHTML = '';
        items.forEach(function (n) {
            var item = document.createElement('a');
            item.href = n.url || '#';
            item.className = 'pc-notification-item' + (n.active ? ' active' : '');
            item.innerHTML = '' +
                '<div class="pc-notification-icon"><i class="bi ' + n.icon + '"></i></div>' +
                '<div class="pc-notification-body">' +
                    '<strong>' + n.title + '</strong>' +
                    '<p>' + n.text + '</p>' +
                    '<span>' + n.time + '</span>' +
                '</div>' +
                (n.active ? '<div class="pc-notification-dot"></div>' : '<i class="bi bi-chevron-right pc-notification-arrow"></i>');
            lista.appendChild(item);
        });

        var total = document.getElementById('notifTotal');
        var activas = document.getElementById('notifActivas');
        var carrito = document.getElementById('notifCarrito');
        if (total) total.textContent = items.length;
        if (activas) activas.textContent = items.filter(function (x) { return x.active; }).length;
        if (carrito) carrito.textContent = items.filter(function (x) { return x.type === 'cart'; }).length;
    }

    function load() {
        if (lista) lista.innerHTML = '<div class="pc-loading-card">Cargando notificaciones...</div>';
        Promise.all([
            apiGet('/PortalCliente/ObtenerMisOrdenesData'),
            apiGet('/PortalCliente/ObtenerCarritoData')
        ]).then(function (responses) {
            var ordenesResp = responses[0] || {};
            var carritoResp = responses[1] || {};
            var ordenes = Array.isArray(ordenesResp.data) ? ordenesResp.data : [];
            var carritoData = carritoResp.data || [];
            render(buildNotifications(ordenes, carritoData));
        });
    }

    if (btnActualizar) btnActualizar.addEventListener('click', load);
    if (btnMarcar) btnMarcar.addEventListener('click', function () {
        var items = document.querySelectorAll('.pc-notification-item.active');
        items.forEach(function (x) { x.classList.remove('active'); });
        var activas = document.getElementById('notifActivas');
        if (activas) activas.textContent = '0';
    });

    document.addEventListener('DOMContentLoaded', load);
}());
