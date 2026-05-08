document.addEventListener('DOMContentLoaded', function () {
    var page = document.querySelector('.pd-page');
    var container = document.getElementById('pdDetalleContainer');
    if (!page || !container) { return; }

    var productoId = page.getAttribute('data-producto-id');
    var endpointDetalle = '/PortalCliente/ObtenerProductoDetalleData?id=' + encodeURIComponent(productoId);
    var endpointAgregarFavorito = '/PortalCliente/AgregarFavorito';
    var endpointQuitarFavorito = '/PortalCliente/QuitarFavorito';
    var endpointAgregarCarrito = '/PortalCliente/AgregarAlCarritoData';

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function normalizarRespuesta(payload) {
        if (!payload) { return { ok: false, data: null, message: 'Respuesta vacia del servidor.' }; }
        if (payload.ok !== undefined) {
            return { ok: payload.ok === true, data: payload.data || null, message: payload.message || payload.mensaje || '' };
        }
        if (payload.success !== undefined) {
            return { ok: payload.success === true, data: payload.data || null, message: payload.message || payload.mensaje || '' };
        }
        return { ok: true, data: payload, message: '' };
    }

    function mostrarToast(mensaje, tipo) {
        var toast = document.createElement('div');
        toast.className = 'pc-toast ' + (tipo === 'success' ? 'pc-toast--success' : 'pc-toast--error');
        toast.textContent = mensaje || 'Operacion realizada.';
        document.body.appendChild(toast);
        setTimeout(function () { toast.classList.add('show'); }, 20);
        setTimeout(function () {
            toast.classList.remove('show');
            setTimeout(function () { toast.remove(); }, 220);
        }, 3600);
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

    function imagenHtml(producto) {
        var imagen = valorProducto(producto, ['ImagenUrl', 'imagenUrl', 'Imagen', 'UrlImagen'], '');
        if (imagenValida(imagen)) {
            return '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(producto.Nombre || 'Producto') + '" onerror="this.style.display=\'none\';this.parentNode.classList.add(\'sin-imagen\');">';
        }
        return '<div class="pd-no-image"><i class="bi bi-lamp"></i><span>Muebles de los Alpes</span></div>';
    }

    function renderDetalle(item) {
        var producto = item.Producto || {};
        var esFavorito = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        var listaDeseosId = item.ListaDeseosId || 0;
        var precio = valorProducto(producto, ['PrecioActual', 'Precio', 'PrecioUnitario', 'precio'], 0);
        var stock = valorProducto(producto, ['StockDisponible', 'Stock', 'Existencia'], null);

        container.innerHTML = ''
            + '<section class="pd-gallery">'
            + '  <div class="pd-image-box">' + imagenHtml(producto) + '</div>'
            + '</section>'
            + '<section class="pd-info-box">'
            + '  <div class="pd-category">' + escapeHtml(producto.CategoriaNombre || producto.Tipo || 'Producto') + '</div>'
            + '  <h1 class="pd-title">' + escapeHtml(producto.Nombre || 'Producto') + '</h1>'
            + '  <div class="pd-reference">Ref. ' + escapeHtml(producto.Referencia || 'N/A') + '</div>'
            + '  <p class="pd-description">' + escapeHtml(producto.Descripcion || 'Sin descripcion disponible.') + '</p>'
            + '  <div class="pd-price">' + formatearMoneda(precio) + '</div>'
            + '  <div class="pd-stock">' + (stock !== null ? escapeHtml(stock) + ' unidad(es) disponibles' : 'Disponible') + '</div>'
            + '  <div class="pd-qty-box">'
            + '      <span>Cantidad</span>'
            + '      <button type="button" data-qty="menos">-</button>'
            + '      <input id="pdCantidad" type="number" min="1" value="1">'
            + '      <button type="button" data-qty="mas">+</button>'
            + '  </div>'
            + '  <div class="pd-actions">'
            + '      <button type="button" class="pd-cart-btn" data-action="carrito" data-id="' + escapeHtml(producto.ProductoId || 0) + '"><i class="bi bi-cart-plus"></i> Agregar al carrito</button>'
            + '      <a class="pd-view-cart" href="/PortalCliente/Carrito"><i class="bi bi-cart"></i> Ver carrito</a>'
            + '      <button type="button" class="pd-fav-btn ' + (esFavorito ? 'pd-fav-btn--active' : '') + '" data-action="' + (esFavorito ? 'quitar' : 'agregar') + '" data-id="' + escapeHtml(producto.ProductoId || 0) + '" data-lista-deseos-id="' + escapeHtml(listaDeseosId) + '">'
            + (esFavorito ? '<i class="bi bi-heart-fill"></i> Quitar favorito' : '<i class="bi bi-heart"></i> Guardar favorito')
            + '      </button>'
            + '  </div>'
            + '</section>'
            + '<section class="pd-spec-box">'
            + '  <h2>Especificaciones</h2>'
            + '  <div class="pd-spec-grid">'
            + '      <div><span>Material</span><strong>' + escapeHtml(producto.Material || 'N/A') + '</strong></div>'
            + '      <div><span>Color</span><strong>' + escapeHtml(producto.Color || 'N/A') + '</strong></div>'
            + '      <div><span>Tipo</span><strong>' + escapeHtml(producto.Tipo || 'N/A') + '</strong></div>'
            + '      <div><span>Dimensiones</span><strong>' + escapeHtml(producto.Dimensiones || 'N/A') + '</strong></div>'
            + '      <div><span>Peso</span><strong>' + escapeHtml(producto.Peso || 'N/A') + '</strong></div>'
            + '      <div><span>Estado</span><strong>' + escapeHtml(producto.Estado || 'ACTIVO') + '</strong></div>'
            + '  </div>'
            + '</section>';
    }

    function cargarDetalle() {
        fetch(endpointDetalle, { method: 'GET', credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (response) { return response.json(); })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                if (!respuesta.ok || !respuesta.data) { throw new Error(respuesta.message || 'No se pudo cargar el detalle del producto.'); }
                renderDetalle(respuesta.data);
            })
            .catch(function (error) {
                container.innerHTML = '<div class="pd-loading">' + escapeHtml(error.message || 'Ocurrio un error al cargar el producto.') + '</div>';
            });
    }

    container.addEventListener('click', function (e) {
        var qtyBtn = e.target.closest('button[data-qty]');
        if (qtyBtn) {
            var input = document.getElementById('pdCantidad');
            var actual = Number(input && input.value ? input.value : 1);
            if (qtyBtn.getAttribute('data-qty') === 'menos') { actual = Math.max(1, actual - 1); }
            else { actual += 1; }
            if (input) { input.value = String(actual); }
            return;
        }

        var button = e.target.closest('button[data-action]');
        if (!button) { return; }
        var action = button.getAttribute('data-action');
        var id = button.getAttribute('data-id');
        var listaDeseosId = button.getAttribute('data-lista-deseos-id');
        var cantidadInput = document.getElementById('pdCantidad');
        var cantidad = Math.max(1, Number(cantidadInput && cantidadInput.value ? cantidadInput.value : 1));

        button.disabled = true;
        var promesa;
        if (action === 'agregar') {
            promesa = postJson(endpointAgregarFavorito, { productoId: Number(id) });
        } else if (action === 'quitar') {
            promesa = postJson(endpointQuitarFavorito, listaDeseosId && Number(listaDeseosId) > 0 ? { listaDeseosId: Number(listaDeseosId) } : { productoId: Number(id) });
        } else if (action === 'carrito') {
            promesa = postJson(endpointAgregarCarrito, { productoId: Number(id), cantidad: cantidad });
        } else {
            button.disabled = false;
            return;
        }

        promesa.then(function (r) {
            mostrarToast(r.message || 'Operacion realizada correctamente.', 'success');
            if (action === 'carrito') { notificarCarritoActualizado(); }
            if (action !== 'carrito') { cargarDetalle(); }
        }).catch(function (error) {
            mostrarToast(error.message || 'No se pudo completar la operacion.', 'error');
        }).finally(function () { button.disabled = false; });
    });

    cargarDetalle();
});
