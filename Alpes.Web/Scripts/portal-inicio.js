document.addEventListener('DOMContentLoaded', function () {
    var recomendados = document.getElementById('piRecomendadosContainer');
    var catalogo = document.getElementById('ciCatalogoContainer');
    var searchInput = document.getElementById('ciBuscarProducto');
    var searchResults = document.getElementById('mhomeSearchResults');
    var btnBuscar = document.getElementById('ciBtnBuscar');
    var btnLimpiar = document.getElementById('ciBtnLimpiar');
    var filtroCategoria = document.getElementById('ciFiltroCategoria');
    var filtroTipo = document.getElementById('ciFiltroTipo');
    var filtroColor = document.getElementById('ciFiltroColor');
    var filtroMaterial = document.getElementById('ciFiltroMaterial');
    var productos = [];

    if (!recomendados || !catalogo) { return; }

    var endpoints = {
        catalogo: '/PortalCliente/ObtenerCatalogoData?_= ',
        recomendados: '/PortalCliente/ObtenerRecomendadosData?_= ',
        ordenes: '/PortalCliente/ObtenerMisOrdenesData?_= ',
        carrito: '/PortalCliente/AgregarAlCarritoData',
        fav: '/PortalCliente/AgregarFavorito',
        unfav: '/PortalCliente/QuitarFavorito'
    };

    function esc(v) { return String(v == null ? '' : v).replace(/[&<>"']/g, function (c) { return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[c]; }); }
    function money(v) { var n = Number(v || 0); return 'Q' + n.toFixed(2); }
    function norm(v) { return String(v || '').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').trim(); }
    function jsonNorm(p) {
        if (!p) { return { ok: false, data: [], message: 'Respuesta vacía.' }; }
        if (p.ok !== undefined) { return { ok: p.ok === true, data: p.data || [], message: p.message || p.mensaje || '' }; }
        if (p.success !== undefined) { return { ok: p.success === true, data: p.data || [], message: p.message || p.mensaje || '' }; }
        return { ok: true, data: p, message: '' };
    }
    function getP(item) { return item && item.Producto ? item.Producto : (item || {}); }
    function value(p, keys, def) { for (var i = 0; i < keys.length; i++) if (p[keys[i]] !== undefined && p[keys[i]] !== null && p[keys[i]] !== '') return p[keys[i]]; return def; }
    function productoId(p) { return value(p, ['ProductoId', 'PRODUCTO_ID', 'productoId', 'id'], 0); }
    function img(p) { return value(p, ['ImagenUrl', 'imagenUrl', 'Imagen', 'UrlImagen'], ''); }
    function validImg(url) { url = String(url || '').trim(); return url.indexOf('http') === 0 || url.indexOf('/') === 0 || url.indexOf('data:image') === 0; }
    function desc(p) { return value(p, ['Descripcion', 'descripcion'], p.Tipo || 'Mueble artesanal guatemalteco con detalles de calidad.'); }
    function precio(p) { return value(p, ['PrecioActual', 'Precio', 'precio', 'PrecioUnitario'], 0); }
    function toast(msg, ok) {
        var el = document.createElement('div');
        el.className = 'pc-toast ' + (ok ? 'pc-toast--success' : 'pc-toast--error');
        el.textContent = msg || (ok ? 'Listo.' : 'No se pudo procesar la solicitud.');
        document.body.appendChild(el);
        setTimeout(function () { el.classList.add('show'); }, 10);
        setTimeout(function () { el.classList.remove('show'); setTimeout(function () { el.remove(); }, 260); }, 3000);
    }
    function fetchJson(url) { return fetch(url + Date.now(), { credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(function (r) { return r.json(); }).then(jsonNorm); }
    function postJson(url, data) {
        return fetch(url, { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json; charset=utf-8', 'X-Requested-With': 'XMLHttpRequest' }, body: JSON.stringify(data) })
            .then(function (r) { return r.text().then(function (t) { var p = null; try { p = t ? JSON.parse(t) : null; } catch (e) { } var n = jsonNorm(p); if (!r.ok || !n.ok) throw new Error(n.message || t || 'No se pudo procesar la solicitud.'); return n; }); });
    }

    function card(item, horizontal) {
        var p = getP(item), id = productoId(p), im = img(p), fav = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        return '<article class="mprod-card ' + (horizontal ? 'mprod-card--wide' : '') + '" data-producto-id="' + esc(id) + '">' +
            '<div class="mprod-image">' +
            '<button class="mprod-heart ' + (fav ? 'active' : '') + '" data-action="' + (fav ? 'quitar' : 'favorito') + '" data-id="' + esc(id) + '" data-lista="' + esc(item.ListaDeseosId || 0) + '"><i class="bi ' + (fav ? 'bi-heart-fill' : 'bi-heart') + '"></i></button>' +
            (validImg(im) ? '<img src="' + esc(im) + '" alt="' + esc(p.Nombre || 'Producto') + '" onerror="this.remove();this.parentElement.classList.add(\'sin-imagen\')" />' : '<div class="mprod-placeholder"><i class="bi bi-chair"></i><span>Muebles de los Alpes</span></div>') +
            '</div>' +
            '<div class="mprod-body">' +
            '<span class="mprod-type">' + esc(p.Tipo || p.CategoriaNombre || 'INTERIOR') + '</span>' +
            '<h3>' + esc(p.Nombre || 'Producto') + '</h3>' +
            '<p>' + esc(desc(p)) + '</p>' +
            '<div class="mprod-tags">' + (p.Referencia ? '<span><i class="bi bi-tag"></i>' + esc(p.Referencia) + '</span>' : '') + (p.Material ? '<span>' + esc(p.Material) + '</span>' : '') + '</div>' +
            '<div class="mprod-bottom"><strong>' + money(precio(p)) + '</strong><span>' + esc(value(p, ['StockDisponible', 'Stock', 'Existencia'], 'Disponible')) + '</span></div>' +
            '<div class="mprod-actions"><a href="/PortalCliente/DetalleProducto/' + esc(id) + '">Ver detalle</a><button data-action="carrito" data-id="' + esc(id) + '"><i class="bi bi-cart-plus"></i> Agregar</button></div>' +
            '</div></article>';
    }
    function render(container, items, wide, empty) { container.innerHTML = items && items.length ? items.map(function (x) { return card(x, wide); }).join('') : '<div class="mhome-mini-empty">' + esc(empty) + '</div>'; }
    function setText(id, txt) { var e = document.getElementById(id); if (e) e.textContent = txt; }

    function fillFilters(items) {
        var maps = { cat: {}, tipo: {}, color: {}, mat: {} };
        items.forEach(function (it) { var p = getP(it); if (p.CategoriaId) maps.cat[p.CategoriaId] = p.CategoriaNombre || ('Categoria ' + p.CategoriaId); if (p.Tipo) maps.tipo[p.Tipo] = p.Tipo; if (p.Color) maps.color[p.Color] = p.Color; if (p.Material) maps.mat[p.Material] = p.Material; });
        function opts(sel, def, obj) { if (!sel) return; sel.innerHTML = '<option value="">' + def + '</option>'; Object.keys(obj).sort().forEach(function (k) { sel.innerHTML += '<option value="' + esc(k) + '">' + esc(obj[k]) + '</option>'; }); }
        opts(filtroCategoria, 'Todas', maps.cat); opts(filtroTipo, 'Todos los tipos', maps.tipo); opts(filtroColor, 'Colores', maps.color); opts(filtroMaterial, 'Materiales', maps.mat);
    }
    function filtered() {
        var q = norm(searchInput ? searchInput.value : ''), cat = filtroCategoria ? filtroCategoria.value : '', tipo = norm(filtroTipo ? filtroTipo.value : ''), color = norm(filtroColor ? filtroColor.value : ''), mat = norm(filtroMaterial ? filtroMaterial.value : '');
        return productos.filter(function (it) { var p = getP(it); var txt = norm([p.Nombre, p.Descripcion, p.Referencia, p.Tipo, p.Material, p.Color].join(' ')); return (!q || txt.indexOf(q) >= 0) && (!cat || String(p.CategoriaId) === String(cat)) && (!tipo || norm(p.Tipo) === tipo) && (!color || norm(p.Color) === color) && (!mat || norm(p.Material) === mat); });
    }
    function applyFilters() { render(catalogo, filtered(), false, 'No se encontraron productos.'); renderSearchResults(); }
    function renderSearchResults() {
        if (!searchResults || !searchInput) return;
        var q = norm(searchInput.value);
        if (!q) { searchResults.innerHTML = ''; searchResults.classList.remove('show'); return; }
        var list = filtered().slice(0, 5);
        searchResults.innerHTML = list.length ? list.map(function (it) { var p = getP(it), id = productoId(p), im = img(p); return '<a href="/PortalCliente/DetalleProducto/' + esc(id) + '"><div class="mhome-suggest-img">' + (validImg(im) ? '<img src="' + esc(im) + '" />' : '<i class="bi bi-chair"></i>') + '</div><div><strong>' + esc(p.Nombre || 'Producto') + '</strong><span>' + esc(desc(p)) + '</span></div><b>' + money(precio(p)) + '</b></a>'; }).join('') : '<div class="mhome-search-empty">Sin resultados</div>';
        searchResults.classList.add('show');
    }

    function loadCatalogo() { return fetchJson(endpoints.catalogo).then(function (r) { if (!r.ok) throw new Error(r.message); productos = r.data || []; fillFilters(productos); applyFilters(); }); }
    function loadRecomendados() { fetchJson(endpoints.recomendados).then(function (r) { render(recomendados, r.ok ? (r.data || []) : [], true, 'Aún no hay recomendaciones.'); }).catch(function () { render(recomendados, [], true, 'No se pudieron cargar recomendaciones.'); }); }
    function loadOrdenes() {
        fetchJson(endpoints.ordenes).then(function (r) {
            var list = r.ok && Array.isArray(r.data) ? r.data : [], activos = 0, entregados = 0, total = 0;
            list.forEach(function (o) { var e = String(o.EstadoUi || o.Estado || '').toUpperCase(); total += Number(o.Total || 0); if (e.indexOf('ENTREG') >= 0) entregados++; else if (e.indexOf('CANCEL') < 0) activos++; });
            setText('ciPedidosTotal', list.length); setText('ciPedidosActivos', activos); setText('ciPedidosEntregados', entregados); setText('ciTotalComprado', money(total));
            var cont = document.getElementById('mhomeOrdenesRecientes');
            if (cont) cont.innerHTML = list.length ? list.slice(0, 4).map(function (o) { return '<a class="mhome-order-item" href="/PortalCliente/DetalleOrden/' + esc(o.OrdenVentaId || o.ORDEN_VENTA_ID || '') + '"><div><strong>' + esc(o.NumeroOrden || o.Codigo || 'Orden registrada') + '</strong><span>' + esc(o.Fecha || o.FechaOrden || '') + '</span></div><b>' + money(o.Total || 0) + '</b></a>'; }).join('') : '<div class="mhome-mini-empty">Sin pedidos recientes.</div>';
            var tr = document.getElementById('mhomeTrackingActual');
            if (tr) { var active = list.filter(function (o) { var e = String(o.EstadoUi || o.Estado || '').toUpperCase(); return e.indexOf('ENTREG') < 0 && e.indexOf('CANCEL') < 0; })[0]; tr.innerHTML = active ? '<div class="mhome-track-icon"><i class="bi bi-truck"></i></div><strong>' + esc(active.NumeroOrden || 'Pedido activo') + '</strong><span>Tu pedido está en proceso.</span><a href="/PortalCliente/Tracking?ordenVentaId=' + esc(active.OrdenVentaId || '') + '">Rastrear →</a>' : '<div class="mhome-mini-empty">No tienes envíos activos.</div>'; }
        }).catch(function () {});
    }
    function handleClick(e) {
        var b = e.target.closest('[data-action]'); if (!b) return;
        var act = b.getAttribute('data-action'), id = Number(b.getAttribute('data-id') || 0), lista = Number(b.getAttribute('data-lista') || 0); if (!id) return; b.disabled = true;
        var req = act === 'carrito' ? postJson(endpoints.carrito, { productoId: id, cantidad: 1 }) : act === 'quitar' ? postJson(endpoints.unfav, lista > 0 ? { listaDeseosId: lista } : { productoId: id }) : postJson(endpoints.fav, { productoId: id });
        req.then(function (r) { toast(r.message || 'Operación realizada.', true); if (act === 'carrito' && window.PortalClienteActualizarBadges) window.PortalClienteActualizarBadges(); else { loadCatalogo(); loadRecomendados(); } }).catch(function (er) { toast(er.message, false); }).finally(function () { b.disabled = false; });
    }

    document.getElementById('mhomeSaludoHora') && (document.getElementById('mhomeSaludoHora').textContent = (new Date().getHours() < 12 ? 'Buenos días' : new Date().getHours() < 18 ? 'Buenas tardes' : 'Buenas noches'));
    [searchInput, filtroCategoria, filtroTipo, filtroColor, filtroMaterial].forEach(function (el) { if (el) el.addEventListener('input', applyFilters); if (el && el.tagName === 'SELECT') el.addEventListener('change', applyFilters); });
    if (btnBuscar) btnBuscar.addEventListener('click', function (e) { e.preventDefault(); applyFilters(); });
    if (btnLimpiar) btnLimpiar.addEventListener('click', function () { if (searchInput) searchInput.value = ''; [filtroCategoria, filtroTipo, filtroColor, filtroMaterial].forEach(function (s) { if (s) s.value = ''; }); applyFilters(); });
    catalogo.addEventListener('click', handleClick); recomendados.addEventListener('click', handleClick);
    loadCatalogo().catch(function (e) { catalogo.innerHTML = '<div class="mhome-mini-empty">' + esc(e.message || 'No se pudo cargar el catálogo.') + '</div>'; });
    loadRecomendados(); loadOrdenes();
});
