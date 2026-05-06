document.addEventListener('DOMContentLoaded', function () {
    var recomendacionesContainer = document.getElementById('piRecomendadosContainer');
    var catalogoContainer = document.getElementById('ciCatalogoContainer');

    var inputBuscar = document.getElementById('ciBuscarProducto');
    var filtroCategoria = document.getElementById('ciFiltroCategoria');
    var filtroTipo = document.getElementById('ciFiltroTipo');
    var filtroColor = document.getElementById('ciFiltroColor');
    var filtroMaterial = document.getElementById('ciFiltroMaterial');
    var btnBuscar = document.getElementById('ciBtnBuscar');
    var btnLimpiar = document.getElementById('ciBtnLimpiar');

    var endpointRecomendados = '/PortalCliente/ObtenerRecomendadosData';
    var endpointCatalogo = '/PortalCliente/ObtenerCatalogoData';
    var endpointAgregarFavorito = '/PortalCliente/AgregarFavorito';
    var endpointQuitarFavorito = '/PortalCliente/QuitarFavorito';
    var endpointAgregarCarrito = '/PortalCliente/AgregarAlCarritoData';
    var endpointOrdenes = '/PortalCliente/ObtenerMisOrdenesData';

    var catalogoCompleto = [];

    if (!recomendacionesContainer || !catalogoContainer) {
        return;
    }

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function normalizarTexto(value) {
        return String(value || '')
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .trim();
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

    function mostrarToast(mensaje, tipo) {
        var toast = document.createElement('div');
        toast.className = 'pc-toast ' + (tipo === 'success' ? 'pc-toast--success' : 'pc-toast--error');
        toast.textContent = mensaje || 'Operacion realizada.';
        document.body.appendChild(toast);
        window.setTimeout(function () { toast.classList.add('show'); }, 20);
        window.setTimeout(function () {
            toast.classList.remove('show');
            window.setTimeout(function () { toast.remove(); }, 220);
        }, 3200);
    }

    function renderEmpty(container, message) {
        container.innerHTML = '<div class="pi-empty-card">' + escapeHtml(message) + '</div>';
    }

    function getProductoDesdeItem(item) {
        return item && item.Producto ? item.Producto : (item || {});
    }

    function valorProducto(producto, nombres, defecto) {
        var i;
        for (i = 0; i < nombres.length; i += 1) {
            if (producto[nombres[i]] !== undefined && producto[nombres[i]] !== null && producto[nombres[i]] !== '') {
                return producto[nombres[i]];
            }
        }
        return defecto;
    }

    function formatearMoneda(valor) {
        var numero = Number(valor || 0);
        return 'Q' + numero.toFixed(2);
    }

    function imagenValida(url) {
        var texto = String(url || '').trim();
        if (texto === '') { return false; }
        return texto.indexOf('http://') === 0 || texto.indexOf('https://') === 0 || texto.indexOf('/') === 0 || texto.indexOf('data:image') === 0;
    }

    function construirImagen(producto) {
        var imagen = valorProducto(producto, ['ImagenUrl', 'imagenUrl', 'Imagen', 'UrlImagen'], '');
        if (imagenValida(imagen)) {
            return '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(producto.Nombre || 'Producto') + '" onerror="this.style.display=\'none\';this.parentNode.classList.add(\'sin-imagen\');">';
        }
        return '<div class="pi-no-image"><i class="bi bi-lamp"></i><span>Muebles de los Alpes</span></div>';
    }

    function construirCard(item, mostrarScore) {
        var producto = getProductoDesdeItem(item);
        var descripcion = producto.Descripcion || producto.Tipo || 'Mueble artesanal guatemalteco con detalles de calidad.';
        var productoId = producto.ProductoId || producto.PRODUCTO_ID || 0;
        var precio = valorProducto(producto, ['PrecioActual', 'Precio', 'PrecioUnitario', 'precio'], 0);
        var stock = valorProducto(producto, ['StockDisponible', 'Stock', 'Existencia'], null);
        var categoria = producto.CategoriaNombre || producto.Categoria || producto.Tipo || 'Producto';
        var esFavorito = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        var listaDeseosId = item.ListaDeseosId || 0;

        var scoreHtml = '';
        if (mostrarScore && item.Score !== undefined && item.Score !== null) {
            scoreHtml = '<span><i class="bi bi-stars"></i> Afinidad ' + escapeHtml(item.Score) + '</span>';
        }

        return ''
            + '<article class="pi-card" data-producto-id="' + escapeHtml(productoId) + '">'
            + '  <div class="pi-card-image">'
            + '      <button type="button" class="pi-heart-btn ' + (esFavorito ? 'active' : '') + '" data-action="' + (esFavorito ? 'quitar' : 'agregar') + '" data-id="' + escapeHtml(productoId) + '" data-lista-deseos-id="' + escapeHtml(listaDeseosId) + '">'
            + '          <i class="bi ' + (esFavorito ? 'bi-heart-fill' : 'bi-heart') + '"></i>'
            + '      </button>'
            + construirImagen(producto)
            + '  </div>'
            + '  <div class="pi-card-body">'
            + '      <div class="pi-category">' + escapeHtml(categoria) + '</div>'
            + '      <h3 class="pi-card-title">' + escapeHtml(producto.Nombre || 'Producto sin nombre') + '</h3>'
            + '      <p class="pi-card-desc">' + escapeHtml(descripcion) + '</p>'
            + '      <div class="pi-card-meta">'
            + (producto.Referencia ? '<span><i class="bi bi-tag"></i>' + escapeHtml(producto.Referencia) + '</span>' : '')
            + (producto.Material ? '<span>' + escapeHtml(producto.Material) + '</span>' : '')
            + (producto.Color ? '<span>' + escapeHtml(producto.Color) + '</span>' : '')
            + scoreHtml
            + '      </div>'
            + '      <div class="pi-price-row">'
            + '          <strong>' + formatearMoneda(precio) + '</strong>'
            + '          <span>' + (stock !== null ? escapeHtml(stock) + ' disp.' : 'Disponible') + '</span>'
            + '      </div>'
            + '      <div class="pi-card-actions">'
            + '          <a class="pi-detail-btn" href="/PortalCliente/DetalleProducto/' + escapeHtml(productoId) + '">Ver detalle</a>'
            + '          <button type="button" class="pi-cart-btn" data-action="carrito" data-id="' + escapeHtml(productoId) + '"><i class="bi bi-cart-plus"></i> Agregar</button>'
            + '      </div>'
            + '  </div>'
            + '</article>';
    }

    function renderCards(container, items, mostrarScore, emptyText) {
        if (!items || !items.length) {
            renderEmpty(container, emptyText);
            return;
        }
        container.innerHTML = items.map(function (item) { return construirCard(item, mostrarScore); }).join('');
    }

    function cargarRecomendados() {
        fetch(endpointRecomendados, { method: 'GET', credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (response) { return response.json(); })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                if (!respuesta.ok) { throw new Error(respuesta.message || 'No se pudieron cargar las recomendaciones.'); }
                renderCards(recomendacionesContainer, respuesta.data || [], true, 'Aun no hay productos recomendados disponibles.');
            })
            .catch(function (error) { renderEmpty(recomendacionesContainer, error.message || 'Ocurrio un error al consultar las recomendaciones.'); });
    }

    function cargarCatalogo() {
        fetch(endpointCatalogo, { method: 'GET', credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (response) { return response.json(); })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                if (!respuesta.ok) { throw new Error(respuesta.message || 'No se pudo obtener el catalogo.'); }
                catalogoCompleto = respuesta.data || [];
                cargarOpcionesFiltros(catalogoCompleto);
                aplicarFiltrosLocales();
            })
            .catch(function (error) { renderEmpty(catalogoContainer, error.message || 'Ocurrio un error al cargar el catalogo.'); });
    }


    function setText(id, value) {
        var el = document.getElementById(id);
        if (el) { el.textContent = String(value); }
    }

    function cargarEstadisticasInicio() {
        var totalEl = document.getElementById('ciPedidosTotal');
        if (!totalEl) { return; }

        fetch(endpointOrdenes, {
            method: 'GET',
            credentials: 'same-origin',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (response) { return response.ok ? response.json() : null; })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                var lista = respuesta.ok && Array.isArray(respuesta.data) ? respuesta.data : [];
                var total = lista.length;
                var activos = 0;
                var entregados = 0;
                var comprado = 0;

                lista.forEach(function (item) {
                    var estado = String(item.EstadoUi || item.Estado || '').toUpperCase();
                    comprado += Number(item.Total || 0);
                    if (estado === 'ENTREGADA' || estado === 'ENTREGADO') {
                        entregados += 1;
                    } else if (estado !== 'CANCELADA' && estado !== 'CANCELADO') {
                        activos += 1;
                    }
                });

                setText('ciPedidosTotal', total);
                setText('ciPedidosActivos', activos);
                setText('ciPedidosEntregados', entregados);
                setText('ciTotalComprado', formatearMoneda(comprado));
            })
            .catch(function () {
                setText('ciPedidosTotal', '0');
                setText('ciPedidosActivos', '0');
                setText('ciPedidosEntregados', '0');
                setText('ciTotalComprado', 'Q0.00');
            });
    }

    function agregarOpcion(select, value, label) {
        var option = document.createElement('option');
        option.value = value;
        option.textContent = label;
        select.appendChild(option);
    }

    function cargarOpcionesFiltros(items) {
        if (!filtroCategoria || !filtroTipo || !filtroColor || !filtroMaterial) { return; }
        var categorias = {}, tipos = {}, colores = {}, materiales = {};
        filtroCategoria.innerHTML = '<option value="">Todas las categorias</option>';
        filtroTipo.innerHTML = '<option value="">Todos los tipos</option>';
        filtroColor.innerHTML = '<option value="">Todos los colores</option>';
        filtroMaterial.innerHTML = '<option value="">Todos los materiales</option>';
        items.forEach(function (item) {
            var p = getProductoDesdeItem(item);
            if (p.CategoriaId) { categorias[String(p.CategoriaId)] = p.CategoriaNombre || ('Categoria ' + p.CategoriaId); }
            if (String(p.Tipo || '').trim() !== '') { tipos[String(p.Tipo)] = String(p.Tipo); }
            if (String(p.Color || '').trim() !== '') { colores[String(p.Color)] = String(p.Color); }
            if (String(p.Material || '').trim() !== '') { materiales[String(p.Material)] = String(p.Material); }
        });
        Object.keys(categorias).sort().forEach(function (key) { agregarOpcion(filtroCategoria, key, categorias[key]); });
        Object.keys(tipos).sort().forEach(function (key) { agregarOpcion(filtroTipo, key, tipos[key]); });
        Object.keys(colores).sort().forEach(function (key) { agregarOpcion(filtroColor, key, colores[key]); });
        Object.keys(materiales).sort().forEach(function (key) { agregarOpcion(filtroMaterial, key, materiales[key]); });
    }

    function coincideBusqueda(producto, texto) {
        var q = normalizarTexto(texto);
        if (q === '') { return true; }
        var fuente = [producto.Nombre, producto.Referencia, producto.Descripcion, producto.Tipo, producto.Material, producto.Color].join(' ');
        return normalizarTexto(fuente).indexOf(q) >= 0;
    }

    function aplicarFiltrosLocales() {
        var q = inputBuscar ? inputBuscar.value : '';
        var categoria = filtroCategoria ? filtroCategoria.value : '';
        var tipo = filtroTipo ? filtroTipo.value : '';
        var color = filtroColor ? filtroColor.value : '';
        var material = filtroMaterial ? filtroMaterial.value : '';
        var filtrados = catalogoCompleto.filter(function (item) {
            var p = getProductoDesdeItem(item);
            return coincideBusqueda(p, q)
                && (categoria === '' || String(p.CategoriaId) === String(categoria))
                && (tipo === '' || normalizarTexto(p.Tipo) === normalizarTexto(tipo))
                && (color === '' || normalizarTexto(p.Color) === normalizarTexto(color))
                && (material === '' || normalizarTexto(p.Material) === normalizarTexto(material));
        });
        renderCards(catalogoContainer, filtrados, false, 'No se encontraron productos con los filtros seleccionados.');
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
            headers: { 'Content-Type': 'application/json; charset=utf-8', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        })
            .then(function (response) {
                return response.text().then(function (text) {
                    var data = null;
                    try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
                    var respuesta = normalizarRespuesta(data);
                    if (!response.ok || !respuesta.ok) {
                        throw new Error(respuesta.message || respuesta.mensaje || text || 'No se pudo procesar la solicitud.');
                    }
                    return respuesta;
                });
            });
    }

    function refrescarTodo() { cargarCatalogo(); cargarRecomendados(); }

    function manejarClick(e) {
        var button = e.target.closest('button[data-action]');
        if (!button) { return; }
        var action = button.getAttribute('data-action');
        var productoId = button.getAttribute('data-id');
        var listaDeseosId = button.getAttribute('data-lista-deseos-id');
        if (!productoId) { return; }
        button.disabled = true;
        var promesa;
        if (action === 'agregar') {
            promesa = postJson(endpointAgregarFavorito, { productoId: Number(productoId) });
        } else if (action === 'quitar') {
            promesa = postJson(endpointQuitarFavorito, listaDeseosId && Number(listaDeseosId) > 0 ? { listaDeseosId: Number(listaDeseosId) } : { productoId: Number(productoId) });
        } else if (action === 'carrito') {
            promesa = postJson(endpointAgregarCarrito, { productoId: Number(productoId), cantidad: 1 });
        } else {
            button.disabled = false;
            return;
        }
        promesa.then(function (r) {
            mostrarToast(r.message || 'Operacion realizada correctamente.', 'success');
            if (action === 'carrito') { notificarCarritoActualizado(); }
            if (action !== 'carrito') { refrescarTodo(); }
        }).catch(function (err) {
            mostrarToast(err.message || 'No se pudo completar la operacion.', 'error');
        }).finally(function () { button.disabled = false; });
    }

    if (btnBuscar) { btnBuscar.addEventListener('click', aplicarFiltrosLocales); }
    if (btnLimpiar) {
        btnLimpiar.addEventListener('click', function () {
            if (inputBuscar) { inputBuscar.value = ''; }
            if (filtroCategoria) { filtroCategoria.value = ''; }
            if (filtroTipo) { filtroTipo.value = ''; }
            if (filtroColor) { filtroColor.value = ''; }
            if (filtroMaterial) { filtroMaterial.value = ''; }
            aplicarFiltrosLocales();
        });
    }
    if (inputBuscar) { inputBuscar.addEventListener('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); aplicarFiltrosLocales(); } }); }
    [filtroCategoria, filtroTipo, filtroColor, filtroMaterial].forEach(function (select) { if (select) { select.addEventListener('change', aplicarFiltrosLocales); } });

    recomendacionesContainer.addEventListener('click', manejarClick);
    catalogoContainer.addEventListener('click', manejarClick);

    cargarCatalogo();
    cargarRecomendados();
    cargarEstadisticasInicio();
});
