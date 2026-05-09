document.addEventListener('DOMContentLoaded', function () {
    var button = document.getElementById('pcMenuButton');
    var sidebar = document.getElementById('pcSidebar');
    var overlay = document.getElementById('pcOverlay');
    var endpointResumen = '/PortalCliente/ObtenerResumenNavegacionData?_=';

    function abrirSidebar() {
        if (sidebar && overlay) {
            sidebar.classList.add('open');
            overlay.classList.add('show');
            document.body.classList.add('pc-sidebar-open');
        }
    }

    function cerrarSidebar() {
        if (sidebar && overlay) {
            sidebar.classList.remove('open');
            overlay.classList.remove('show');
            document.body.classList.remove('pc-sidebar-open');
        }
    }

    function alternarSidebar() {
        if (sidebar && sidebar.classList.contains('open')) {
            cerrarSidebar();
        } else {
            abrirSidebar();
        }
    }

    function normalizar(payload) {
        if (!payload) return { ok: false, data: null };

        if (payload.ok !== undefined) {
            return {
                ok: payload.ok === true,
                data: payload.data || null
            };
        }

        if (payload.success !== undefined) {
            return {
                ok: payload.success === true,
                data: payload.data || null
            };
        }

        return {
            ok: true,
            data: payload
        };
    }

    setBadge('pcBadgeOrders', r.data.totalOrdenes || 0);

    function cargarResumen() {
        fetch(endpointResumen + Date.now(), {
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (r) {
                return r.ok ? r.json() : null;
            })
            .then(function (p) {
                var r = normalizar(p);
                if (!r.ok || !r.data) return;

                

                
                setBadge('pcBadgeCart', r.data.carritoItems || 0);
                setBadge('pcTopbarCartBadge', r.data.carritoItems || 0);
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
        overlay.addEventListener('click', cerrarSidebar);
    }

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            cerrarSidebar();
        }
    });

    window.addEventListener('resize', function () {
        if (window.innerWidth >= 900) {
            cerrarSidebar();
        }
    });

    document.querySelectorAll('.pc-nav-item, .pc-bottom-item').forEach(function (link) {
        link.addEventListener('click', function () {
            if (window.innerWidth < 900) {
                cerrarSidebar();
            }
        });
    });

    window.PortalClienteActualizarBadges = cargarResumen;

    cargarResumen();
});

document.addEventListener('DOMContentLoaded', function () {
    var toggle = document.querySelector('.pc-nav-toggle');
    var dropdown = document.querySelector('.pc-nav-dropdown');

    if (toggle && dropdown) {
        toggle.addEventListener('click', function () {
            dropdown.classList.toggle('open');
        });
    }
});