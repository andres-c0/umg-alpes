document.addEventListener('DOMContentLoaded', function () {
    var button = document.getElementById('pcMenuButton');
    var sidebar = document.getElementById('pcSidebar');
    var overlay = document.getElementById('pcOverlay');

    var badgeOrders = document.getElementById('pcBadgeOrders');
    var badgeCart = document.getElementById('pcBadgeCart');
    var topbarCartBadge = document.getElementById('pcTopbarCartBadge');

    var endpointResumen = '/PortalCliente/ObtenerResumenNavegacionData';

    function abrirSidebar() {
        if (!sidebar || !overlay) {
            return;
        }
        sidebar.classList.add('open');
        overlay.classList.add('show');
    }

    function cerrarSidebar() {
        if (!sidebar || !overlay) {
            return;
        }
        sidebar.classList.remove('open');
        overlay.classList.remove('show');
    }

    function alternarSidebar() {
        if (!sidebar || !overlay) {
            return;
        }

        if (sidebar.classList.contains('open')) {
            cerrarSidebar();
        } else {
            abrirSidebar();
        }
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

    function actualizarBadges(data) {
        var ordenes = data && data.ordenesActivas ? Number(data.ordenesActivas) : 0;
        var carrito = data && data.carritoItems ? Number(data.carritoItems) : 0;

        if (badgeOrders) {
            badgeOrders.textContent = String(ordenes);
        }

        if (badgeCart) {
            badgeCart.textContent = String(carrito);
        }

        if (topbarCartBadge) {
            topbarCartBadge.textContent = String(carrito);
        }
    }

    function cargarResumen() {
        fetch(endpointResumen, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo obtener el resumen.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                if (!respuesta.ok || !respuesta.data) {
                    return;
                }

                actualizarBadges(respuesta.data);
            })
            .catch(function () {
            });
    }

    if (button) {
        button.addEventListener('click', function (e) {
            e.preventDefault();
            alternarSidebar();
        });
    }

    if (overlay) {
        overlay.addEventListener('click', function () {
            cerrarSidebar();
        });
    }

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            cerrarSidebar();
        }
    });

    window.addEventListener('resize', function () {
        if (window.innerWidth >= 992) {
            cerrarSidebar();
        }
    });

    cargarResumen();
});