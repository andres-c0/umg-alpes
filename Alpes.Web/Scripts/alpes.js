/* ═══════════════════════════════════════════════════════
   MUEBLES DE LOS ALPES — Global JS
═══════════════════════════════════════════════════════ */

// ── API Base (cambiar según entorno) ──
var API_BASE = 'http://localhost:3000/api';

// ── Toast ──────────────────────────────────────────────
var AlpesToast = {
    wrap: null,
    init: function () {
        if (!this.wrap) {
            this.wrap = $('<div id="a-toast-wrap"></div>').appendTo('body');
        }
    },
    show: function (msg, type, duration) {
        this.init();
        type = type || 'default'; duration = duration || 3000;
        var cls = type === 'ok' ? 'ok' : type === 'err' ? 'err' : type === 'warn' ? 'warn' : '';
        var icon = type === 'ok' ? 'bi-check-circle-fill'
                 : type === 'err' ? 'bi-x-circle-fill'
                 : type === 'warn' ? 'bi-exclamation-triangle-fill'
                 : 'bi-info-circle-fill';
        var t = $('<div class="a-toast ' + cls + '"><i class="bi ' + icon + '"></i><span></span></div>');
        t.find('span').text(msg);
        this.wrap.append(t);
        setTimeout(function () { t.fadeOut(300, function () { t.remove(); }); }, duration);
    },
    ok:   function (m) { this.show(m, 'ok'); },
    err:  function (m) { this.show(m, 'err'); },
    warn: function (m) { this.show(m, 'warn'); }
};

// ── API Helper ─────────────────────────────────────────
var AlpesAPI = {
    get: function (endpoint) {
        return $.ajax({ url: API_BASE + endpoint, method: 'GET', dataType: 'json' });
    },
    post: function (endpoint, data) {
        return $.ajax({
            url: API_BASE + endpoint, method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data), dataType: 'json'
        });
    },
    put: function (endpoint, data) {
        return $.ajax({
            url: API_BASE + endpoint, method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data), dataType: 'json'
        });
    },
    del: function (endpoint) {
        return $.ajax({ url: API_BASE + endpoint, method: 'DELETE', dataType: 'json' });
    }
};

// ── Session ────────────────────────────────────────────
var AlpesSession = {
    set: function (user) {
        localStorage.setItem('alpes_user', JSON.stringify(user));
    },
    get: function () {
        try { return JSON.parse(localStorage.getItem('alpes_user')); } catch (e) { return null; }
    },
    clear: function () {
        localStorage.removeItem('alpes_user');
    },
    isLoggedIn: function () {
        return this.get() !== null;
    },
    isAdmin: function () {
        var u = this.get();
        return u && u.ROL_ID !== 3;
    },
    getClienteId: function () {
        var u = this.get();
        return u ? (u.CLI_ID || u.cli_id || null) : null;
    }
};

// ── Carrito (localStorage) ─────────────────────────────
var AlpesCarrito = {
    KEY: 'alpes_carrito',
    getItems: function () {
        try { return JSON.parse(localStorage.getItem(this.KEY)) || []; } catch (e) { return []; }
    },
    save: function (items) {
        localStorage.setItem(this.KEY, JSON.stringify(items));
        this.updateBadge();
    },
    agregar: function (producto, cantidad) {
        var items = this.getItems();
        var idx = items.findIndex(function (i) { return i.productoId === producto.productoId; });
        if (idx >= 0) {
            items[idx].cantidad += (cantidad || 1);
        } else {
            items.push({
                productoId: producto.productoId,
                nombre: producto.nombre,
                precio: producto.precio,
                imagenUrl: producto.imagenUrl,
                cantidad: cantidad || 1
            });
        }
        this.save(items);
        AlpesToast.ok('Producto agregado al carrito');
    },
    eliminar: function (productoId) {
        var items = this.getItems().filter(function (i) { return i.productoId !== productoId; });
        this.save(items);
    },
    actualizarCantidad: function (productoId, cant) {
        var items = this.getItems();
        var idx = items.findIndex(function (i) { return i.productoId === productoId; });
        if (idx >= 0) { items[idx].cantidad = cant; }
        this.save(items);
    },
    total: function () {
        return this.getItems().reduce(function (s, i) { return s + (i.precio * i.cantidad); }, 0);
    },
    totalItems: function () {
        return this.getItems().reduce(function (s, i) { return s + i.cantidad; }, 0);
    },
    limpiar: function () {
        localStorage.removeItem(this.KEY);
        this.updateBadge();
    },
    updateBadge: function () {
        var n = this.totalItems();
        var badge = $('.a-cart-count');
        badge.text(n);
        badge.toggle(n > 0);
    }
};

// ── Favoritos (localStorage) ───────────────────────────
var AlpesFavoritos = {
    KEY: 'alpes_favs',
    get: function () {
        try { return JSON.parse(localStorage.getItem(this.KEY)) || []; } catch (e) { return []; }
    },
    toggle: function (productoId) {
        var favs = this.get();
        var idx = favs.indexOf(productoId);
        if (idx >= 0) {
            favs.splice(idx, 1);
            AlpesToast.show('Eliminado de favoritos');
        } else {
            favs.push(productoId);
            AlpesToast.ok('Agregado a favoritos');
        }
        localStorage.setItem(this.KEY, JSON.stringify(favs));
        return idx < 0;
    },
    isFav: function (productoId) {
        return this.get().indexOf(productoId) >= 0;
    }
};

// ── Sidebar ────────────────────────────────────────────
var AlpesSidebar = {
    init: function () {
        // Toggle mobile
        $(document).on('click', '.a-topbar__menu-btn', function () {
            $('.a-sidebar').addClass('open');
            $('.a-overlay').addClass('show');
        });
        $(document).on('click', '.a-overlay', function () {
            $('.a-sidebar').removeClass('open');
            $('.a-overlay').removeClass('show');
        });

        // Nav collapse
        $(document).on('click', '.a-nav-item[data-children]', function () {
            var key = $(this).data('children');
            var children = $('#nav-ch-' + key);
            var isOpen = children.hasClass('open');
            $(this).toggleClass('open', !isOpen);
            children.toggleClass('open', !isOpen);
        });

        // Active mark
        var path = window.location.pathname;
        $('.a-nav-item[href]').each(function () {
            if ($(this).attr('href') === path) {
                $(this).addClass('active');
            }
        });

        // Populate user info
        var user = AlpesSession.get();
        if (user) {
            var name = user.USERNAME || user.username || 'Usuario';
            var email = user.EMAIL || user.email || '';
            $('.a-sidebar__user-name').text(name);
            $('.a-sidebar__user-email').text(email);
            $('.a-sidebar__avatar').text(name.charAt(0).toUpperCase());
        }

        // Cart badge
        AlpesCarrito.updateBadge();

        // Logout
        $(document).on('click', '.a-sidebar__logout, [data-action="logout"]', function () {
            AlpesSession.clear();
            AlpesCarrito.limpiar();
            window.location.href = '/Home/Login';
        });
    }
};

// ── Format helpers ─────────────────────────────────────
var AlpesUtil = {
    formatQ: function (val) {
        return 'Q' + parseFloat(val || 0).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    },
    formatDate: function (dateStr) {
        if (!dateStr) return '—';
        try {
            var d = new Date(dateStr);
            return d.toLocaleDateString('es-GT', { day: '2-digit', month: 'short', year: 'numeric' });
        } catch (e) { return dateStr; }
    },
    getField: function (obj, field) {
        return obj[field.toUpperCase()] !== undefined
            ? obj[field.toUpperCase()]
            : obj[field.toLowerCase()];
    },
    pillEstado: function (estado) {
        estado = (estado || '').toLowerCase();
        if (estado === 'activo' || estado === 'entregado') return 'pill-ok';
        if (estado === 'pendiente' || estado === 'en proceso') return 'pill-warn';
        if (estado === 'cancelado' || estado === 'inactivo') return 'pill-danger';
        return 'pill-default';
    },
    showLoading: function (container) {
        $(container).html('<div class="a-loading"><span class="a-spin dark"></span> Cargando...</div>');
    },
    showEmpty: function (container, icon, title, text) {
        $(container).html(
            '<div class="a-empty">' +
            '<div class="a-empty__icon"><i class="bi ' + icon + '"></i></div>' +
            '<div class="a-empty__title">' + title + '</div>' +
            '<div class="a-empty__text">' + text + '</div>' +
            '</div>'
        );
    }
};

// ── Init on ready ──────────────────────────────────────
$(function () {
    AlpesSidebar.init();

    // Auth guard
    var publicPaths = ['/Home/Login', '/Home/Registro', '/Home/Index'];
    var path = window.location.pathname;
    var isPublic = publicPaths.some(function (p) { return path.indexOf(p) >= 0; });
    if (!isPublic && !AlpesSession.isLoggedIn()) {
        window.location.href = '/Home/Login';
    }
});
