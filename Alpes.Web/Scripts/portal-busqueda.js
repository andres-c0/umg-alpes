document.addEventListener('DOMContentLoaded', function () {
    var inputBuscar = document.getElementById('pbBuscar');
    var btnBuscar = document.getElementById('pbBtnBuscar');
    var btnLimpiar = document.getElementById('pbBtnLimpiar');
    var filtroCategoria = document.getElementById('pbFiltroCategoria');
    var filtroTipo = document.getElementById('pbFiltroTipo');
    var filtroMaterial = document.getElementById('pbFiltroMaterial');
    var filtroColor = document.getElementById('pbFiltroColor');
    var selectOrden = document.getElementById('pbOrden');
    var resultados = document.getElementById('pbResultados');
    var resumen = document.getElementById('pbResumen');
    var chips = document.getElementById('pbChips');

    var endpointCatalogo = '/PortalCliente/ObtenerCatalogoData';
    var endpointAgregarCarrito = '/PortalCliente/AgregarAlCarritoData';
    var endpointAgregarFavorito = '/PortalCliente/AgregarFavorito';
    var endpointQuitarFavorito = '/PortalCliente/QuitarFavorito';

    var catalogoCompleto = [];

    if (!resultados) { return; }

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
        if (!payload) { return { ok: false, data: null, message: 'Respuesta vacía del servidor.' }; }
        if (payload.ok !== undefined) {
            return { ok: payload.ok === true, data: payload.data || [], message: payload.message || payload.mensaje || '' };
        }
        if (payload.success !== undefined) {
            return { ok: payload.success === true, data: payload.data || [], message: payload.message || payload.mensaje || '' };
        }
        return { ok: true, data: payload, message: '' };
    }

    function mostrarToast(mensaje, tipo) {
        if (window.PortalCliente && typeof window.PortalCliente.mostrarToast === 'function') {
            window.PortalCliente.mostrarToast(mensaje, tipo);
            return;
        }

        var toast = document.createElement('div');
        toast.className = 'pc-toast ' + (tipo === 'success' ? 'pc-toast--success' : 'pc-toast--error');
        toast.textContent = mensaje || 'Operación realizada.';
        document.body.appendChild(toast);
        window.setTimeout(function () { toast.classList.add('show'); }, 20);
        window.setTimeout(function () {
            toast.classList.remove('show');
            window.setTimeout(function () { toast.remove(); }, 220);
        }, 3200);
    }

    function getProductoDesdeItem(item) {
        return item && item.Producto ? item.Producto : (item || {});
    }

    function valorProducto(producto, nombres, defecto) {
        for (var i = 0; i < nombres.length; i += 1) {
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
        return texto !== '' && (texto.indexOf('http://') === 0 || texto.indexOf('https://') === 0 || texto.indexOf('/') === 0 || texto.indexOf('data:image') === 0);
    }

    function construirImagen(producto) {
        var imagen = valorProducto(producto, ['ImagenUrl', 'imagenUrl', 'Imagen', 'UrlImagen'], '');
        if (imagenValida(imagen)) {
            return '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(producto.Nombre || 'Producto') + '" onerror="this.style.display=\'none\';this.parentNode.classList.add(\'sin-imagen\');">';
        }
        return '<div class="pi-no-image"><i class="bi bi-lamp"></i><span>Muebles de los Alpes</span></div>';
    }

    function construirCard(item) {
        var producto = getProductoDesdeItem(item);
        var productoId = producto.ProductoId || producto.PRODUCTO_ID || 0;
        var descripcion = producto.Descripcion || producto.Tipo || 'Mueble artesanal guatemalteco con detalles de calidad.';
        var precio = valorProducto(producto, ['PrecioActual', 'Precio', 'PrecioUnitario', 'precio'], 0);
        var stock = valorProducto(producto, ['StockDisponible', 'Stock', 'Existencia'], null);
        var categoria = producto.CategoriaNombre || producto.Categoria || producto.Tipo || 'Producto';
        var esFavorito = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        var listaDeseosId = item.ListaDeseosId || 0;

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

    function renderEmpty(message) {
        resultados.innerHTML = '<div class="pi-empty-card pb-empty"><i class="bi bi-search"></i><strong>Sin resultados</strong><span>' + escapeHtml(message) + '</span></div>';
    }

    function agregarOpcion(select, value, label) {
        if (!select) { return; }
        var option = document.createElement('option');
        option.value = value;
        option.textContent = label;
        select.appendChild(option);
    }

    function llenarFiltros(items) {
        var categorias = {}, tipos = {}, materiales = {}, colores = {};
        if (filtroCategoria) { filtroCategoria.innerHTML = '<option value="">Todas</option>'; }
        if (filtroTipo) { filtroTipo.innerHTML = '<option value="">Todos</option>'; }
        if (filtroMaterial) { filtroMaterial.innerHTML = '<option value="">Todos</option>'; }
        if (filtroColor) { filtroColor.innerHTML = '<option value="">Todos</option>'; }

        items.forEach(function (item) {
            var p = getProductoDesdeItem(item);
            if (p.CategoriaId) { categorias[String(p.CategoriaId)] = p.CategoriaNombre || ('Categoría ' + p.CategoriaId); }
            if (String(p.Tipo || '').trim() !== '') { tipos[String(p.Tipo)] = String(p.Tipo); }
            if (String(p.Material || '').trim() !== '') { materiales[String(p.Material)] = String(p.Material); }
            if (String(p.Color || '').trim() !== '') { colores[String(p.Color)] = String(p.Color); }
        });

        Object.keys(categorias).sort().forEach(function (key) { agregarOpcion(filtroCategoria, key, categorias[key]); });
        Object.keys(tipos).sort().forEach(function (key) { agregarOpcion(filtroTipo, key, tipos[key]); });
        Object.keys(materiales).sort().forEach(function (key) { agregarOpcion(filtroMaterial, key, materiales[key]); });
        Object.keys(colores).sort().forEach(function (key) { agregarOpcion(filtroColor, key, colores[key]); });
    }

    function coincideBusqueda(producto, texto) {
        var q = normalizarTexto(texto);
        if (q === '') { return true; }
        var fuente = [producto.Nombre, producto.Referencia, producto.Descripcion, producto.Tipo, producto.Material, producto.Color, producto.CategoriaNombre].join(' ');
        return normalizarTexto(fuente).indexOf(q) >= 0;
    }

    function obtenerPrecio(item) {
        var p = getProductoDesdeItem(item);
        return Number(valorProducto(p, ['PrecioActual', 'Precio', 'PrecioUnitario', 'precio'], 0));
    }

    function obtenerStock(item) {
        var p = getProductoDesdeItem(item);
        return Number(valorProducto(p, ['StockDisponible', 'Stock', 'Existencia'], 0));
    }

    function aplicarOrden(lista) {
        var orden = selectOrden ? selectOrden.value : 'relevancia';
        var copia = lista.slice(0);
        if (orden === 'nombre_asc') {
            copia.sort(function (a, b) { return String(getProductoDesdeItem(a).Nombre || '').localeCompare(String(getProductoDesdeItem(b).Nombre || '')); });
        } else if (orden === 'precio_asc') {
            copia.sort(function (a, b) { return obtenerPrecio(a) - obtenerPrecio(b); });
        } else if (orden === 'precio_desc') {
            copia.sort(function (a, b) { return obtenerPrecio(b) - obtenerPrecio(a); });
        } else if (orden === 'stock_desc') {
            copia.sort(function (a, b) { return obtenerStock(b) - obtenerStock(a); });
        }
        return copia;
    }

    function actualizarChips() {
        if (!chips) { return; }
        var valores = [];
        var q = inputBuscar ? inputBuscar.value.trim() : '';
        if (q) { valores.push('Búsqueda: ' + q); }
        [filtroCategoria, filtroTipo, filtroMaterial, filtroColor].forEach(function (select) {
            if (select && select.value) { valores.push(select.options[select.selectedIndex].text); }
        });
        if (selectOrden && selectOrden.value !== 'relevancia') { valores.push(selectOrden.options[selectOrden.selectedIndex].text); }
        if (!valores.length) {
            chips.innerHTML = '<span class="pb-chip pb-chip-empty">Sin filtros aplicados</span>';
            return;
        }
        chips.innerHTML = valores.map(function (v) { return '<span class="pb-chip">' + escapeHtml(v) + '</span>'; }).join('');
    }
    function actualizarCatalogTabs() {
        var total = catalogoCompleto.length;

        var exterior = catalogoCompleto.filter(function (item) {
            var p = getProductoDesdeItem(item);
            var categoria = p.CategoriaNombre || p.Categoria || p.Tipo || '';
            return normalizarTexto(categoria) === 'exterior';
        }).length;

        var interior = catalogoCompleto.filter(function (item) {
            var p = getProductoDesdeItem(item);
            var categoria = p.CategoriaNombre || p.Categoria || p.Tipo || '';
            return normalizarTexto(categoria) === 'interior';
        }).length;

        var t = document.getElementById('catCountTodos');
        var e = document.getElementById('catCountExterior');
        var i = document.getElementById('catCountInterior');

        if (t) { t.textContent = total; }
        if (e) { e.textContent = exterior; }
        if (i) { i.textContent = interior; }
    }
    function aplicarFiltros() {
        var q = inputBuscar ? inputBuscar.value : '';
        var categoria = filtroCategoria ? filtroCategoria.value : '';
        var tipo = filtroTipo ? filtroTipo.value : '';
        var material = filtroMaterial ? filtroMaterial.value : '';
        var color = filtroColor ? filtroColor.value : '';
        var tabActivo = document.querySelector('.catalog-tab.active');
        var categoriaTab = tabActivo ? tabActivo.getAttribute('data-tab') : 'todos';

        var filtrados = catalogoCompleto.filter(function (item) {
            var p = getProductoDesdeItem(item);
            return coincideBusqueda(p, q)
                && (categoria === '' || String(p.CategoriaId) === String(categoria))
                && (tipo === '' || normalizarTexto(p.Tipo) === normalizarTexto(tipo))
                && (material === '' || normalizarTexto(p.Material) === normalizarTexto(material))
                && (color === '' || normalizarTexto(p.Color) === normalizarTexto(color))
                && (categoriaTab === 'todos' || normalizarTexto(p.CategoriaNombre || p.Categoria || p.Tipo || '') === normalizarTexto(categoriaTab));
        });

        filtrados = aplicarOrden(filtrados);
        if (resumen) { resumen.textContent = filtrados.length + ' producto(s) encontrados'; }
        actualizarCatalogTabs();
        actualizarChips();

        if (!filtrados.length) {
            renderEmpty('No encontramos productos con esos filtros. Intenta limpiar la búsqueda o usar otro término.');
            return;
        }

        resultados.innerHTML = filtrados.map(construirCard).join('');
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
        }).then(function (response) {
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

    function manejarClick(e) {
        var button = e.target.closest('button[data-action]');
        if (!button) { return; }
        var action = button.getAttribute('data-action');
        var productoId = Number(button.getAttribute('data-id') || 0);
        var listaDeseosId = Number(button.getAttribute('data-lista-deseos-id') || 0);
        if (!productoId) { return; }
        button.disabled = true;
        var promesa;
        if (action === 'agregar') {
            promesa = postJson(endpointAgregarFavorito, { productoId: productoId });
        } else if (action === 'quitar') {
            promesa = postJson(endpointQuitarFavorito, listaDeseosId > 0 ? { listaDeseosId: listaDeseosId } : { productoId: productoId });
        } else if (action === 'carrito') {
            promesa = postJson(endpointAgregarCarrito, { productoId: productoId, cantidad: 1 });
        } else {
            button.disabled = false;
            return;
        }
        promesa.then(function (r) {
            mostrarToast(r.message || 'Operación realizada correctamente.', 'success');
            if (action === 'carrito') { notificarCarritoActualizado(); return; }
            cargarCatalogo();
        }).catch(function (err) {
            mostrarToast(err.message || 'No se pudo completar la operación.', 'error');
        }).finally(function () { button.disabled = false; });
    }

    function cargarQueryUrl() {
        var params = new URLSearchParams(window.location.search);
        var q = params.get('q') || '';
        if (inputBuscar && q) { inputBuscar.value = q; }
    }

    function cargarCatalogo() {
        if (resumen) { resumen.textContent = 'Cargando productos...'; }
        resultados.innerHTML = '<div class="pi-empty-card">Cargando catálogo...</div>';
        fetch(endpointCatalogo, { method: 'GET', credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (response) { return response.json(); })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                if (!respuesta.ok) { throw new Error(respuesta.message || 'No se pudo obtener el catálogo.'); }
                catalogoCompleto = respuesta.data || [];
                llenarFiltros(catalogoCompleto);
                cargarQueryUrl();
                aplicarFiltros();
            })
            .catch(function (error) {
                if (resumen) { resumen.textContent = 'No se pudo cargar el catálogo'; }
                renderEmpty(error.message || 'Ocurrió un error al cargar los productos.');
            });
    }

    if (btnBuscar) { btnBuscar.addEventListener('click', aplicarFiltros); }
    if (inputBuscar) {
        inputBuscar.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') { e.preventDefault(); aplicarFiltros(); }
        });
        inputBuscar.addEventListener('input', function () {
            window.clearTimeout(inputBuscar._pbTimer);
            inputBuscar._pbTimer = window.setTimeout(aplicarFiltros, 250);
        });
    }
    [filtroCategoria, filtroTipo, filtroMaterial, filtroColor, selectOrden].forEach(function (select) {
        if (select) { select.addEventListener('change', aplicarFiltros); }
    });
    if (btnLimpiar) {
        btnLimpiar.addEventListener('click', function () {
            if (inputBuscar) { inputBuscar.value = ''; }
            [filtroCategoria, filtroTipo, filtroMaterial, filtroColor].forEach(function (select) { if (select) { select.value = ''; } });
            if (selectOrden) { selectOrden.value = 'relevancia'; }
            aplicarFiltros();
        });
    }
    document.querySelectorAll('.catalog-tab').forEach(function (tab) {
        tab.addEventListener('click', function (e) {
            e.preventDefault();

            document.querySelectorAll('.catalog-tab').forEach(function (x) {
                x.classList.remove('active');
            });

            tab.classList.add('active');
            aplicarFiltros();
        });
    });
    resultados.addEventListener('click', manejarClick);
    cargarCatalogo();
});

