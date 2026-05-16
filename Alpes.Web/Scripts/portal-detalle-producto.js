document.addEventListener('DOMContentLoaded', function () {
    var page = document.querySelector('.pd-page');
    var container = document.getElementById('pdDetalleContainer');
    if (!page || !container) { return; }

    var productoId = page.getAttribute('data-producto-id');
    var endpointDetalle = '/PortalCliente/ObtenerProductoDetalleData?id=' + encodeURIComponent(productoId);
    var endpointResenas = '/PortalCliente/ObtenerResenasProductoData?productoId=' + encodeURIComponent(productoId);
    var endpointCrearResena = '/PortalCliente/CrearResenaData';
    var endpointEliminarResena = '/PortalCliente/EliminarResenaData';
    var endpointAgregarFavorito = '/PortalCliente/AgregarFavorito';
    var endpointQuitarFavorito = '/PortalCliente/QuitarFavorito';
    var endpointAgregarCarrito = '/PortalCliente/AgregarAlCarritoData';
    var ratingSeleccionado = 5;

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
        if (window.PortalCliente && typeof window.PortalCliente.mostrarToast === 'function') {
            window.PortalCliente.mostrarToast(mensaje, tipo);
            return;
        }

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

    function getJson(url) {
        return fetch(url, {
            method: 'GET',
            credentials: 'same-origin',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
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

        if (isNaN(numero)) {
            numero = 0;
        }

        return 'Q' + numero.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
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

    function ratingStarsHtml(value) {
        var n = Math.max(0, Math.min(5, Math.round(Number(value || 0))));
        var html = '';

        for (var i = 1; i <= 5; i += 1) {
            html += '<i class="bi ' + (i <= n ? 'bi-star-fill' : 'bi-star') + '"></i>';
        }

        return html;
    }

    function obtenerTextoCalificacion(valor) {
        var n = Math.max(1, Math.min(5, Number(valor || 1)));

        switch (n) {
            case 1:
                return 'Muy mala';
            case 2:
                return 'Regular';
            case 3:
                return 'Buena';
            case 4:
                return 'Muy buena';
            case 5:
                return 'Excelente';
            default:
                return 'Buena';
        }
    }

    function setRating(valor) {
        ratingSeleccionado = Math.max(1, Math.min(5, Number(valor || 1)));

        container.querySelectorAll('[data-review-rating]').forEach(function (btn) {
            var rating = Number(btn.getAttribute('data-review-rating') || 0);
            btn.classList.toggle('active', rating <= ratingSeleccionado);
        });

        var hidden = document.getElementById('pdResenaCalificacion');
        if (hidden) { hidden.value = String(ratingSeleccionado); }

        var ratingValue = document.getElementById('pdReviewRatingValue');
        var ratingText = document.getElementById('pdReviewRatingText');

        if (ratingValue) {
            ratingValue.textContent = ratingSeleccionado + '/5';
        }

        if (ratingText) {
            ratingText.textContent = obtenerTextoCalificacion(ratingSeleccionado);
        }
    }

    function renderDetalle(item) {
        var producto = item.Producto || {};
        var esFavorito = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        var listaDeseosId = item.ListaDeseosId || 0;
        var precio = valorProducto(producto, ['PrecioActual', 'Precio', 'PrecioUnitario', 'precio'], 0);
        var stock = valorProducto(producto, ['StockDisponible', 'Stock', 'Existencia'], null);
        var totalResenas = Number(item.TotalResenas || 0);
        var promedio = Number(item.PromedioCalificacion || 0);
        var textoResumen = totalResenas > 0 ? promedio.toFixed(1) + ' / 5 · ' + totalResenas + ' reseña(s)' : 'Sin reseñas todavía';

        container.innerHTML = ''
            + '<section class="pd-gallery">'
            + '  <div class="pd-image-box">' + imagenHtml(producto) + '</div>'
            + '</section>'
            + '<section class="pd-info-box">'
            + '  <div class="pd-category">' + escapeHtml(producto.CategoriaNombre || producto.Tipo || 'Producto') + '</div>'
            + '  <h1 class="pd-title">' + escapeHtml(producto.Nombre || 'Producto') + '</h1>'
            + '  <div class="pd-reference">Ref. ' + escapeHtml(producto.Referencia || 'N/A') + '</div>'
            + '  <div class="pd-review-mini"><span>' + ratingStarsHtml(promedio) + '</span><strong>' + escapeHtml(textoResumen) + '</strong></div>'
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
            + '</section>'
            + '<section class="pd-reviews-box">'
            + '  <div class="pd-reviews-header">'
            + '      <div><span class="pd-category">Opiniones</span><h2>Reseñas del producto</h2></div>'
            + '      <div class="pd-reviews-summary" id="pdReviewsSummary">Cargando reseñas...</div>'
            + '  </div>'
            + '  <form id="pdReviewForm" class="pd-review-form">'
            + '      <div class="pd-review-form-title">'
            + '          <strong>Escribe tu reseña</strong>'
            + '          <span id="pdReviewMode">Tu opinión ayuda a otros clientes.</span>'
            + '      </div>'
            + '      <div class="pd-review-rating-row">'
            + '          <div class="pd-review-rating-input" id="pdReviewRatingInput">'
            + '              <button type="button" data-review-rating="1" aria-label="1 estrella"><i class="bi bi-star-fill"></i></button>'
            + '              <button type="button" data-review-rating="2" aria-label="2 estrellas"><i class="bi bi-star-fill"></i></button>'
            + '              <button type="button" data-review-rating="3" aria-label="3 estrellas"><i class="bi bi-star-fill"></i></button>'
            + '              <button type="button" data-review-rating="4" aria-label="4 estrellas"><i class="bi bi-star-fill"></i></button>'
            + '              <button type="button" data-review-rating="5" aria-label="5 estrellas"><i class="bi bi-star-fill"></i></button>'
            + '          </div>'
            + '          <div class="pd-review-rating-state">'
            + '              <strong id="pdReviewRatingValue">5/5</strong>'
            + '              <span id="pdReviewRatingText">Excelente</span>'
            + '          </div>'
            + '      </div>'
            + '      <input type="hidden" id="pdResenaCalificacion" value="5">'
            + '      <textarea id="pdComentarioResena" maxlength="1000" rows="4" placeholder="Comparte qué te pareció el producto..."></textarea>'
            + '      <div class="pd-review-form-actions">'
            + '          <small>Máximo 1000 caracteres.</small>'
            + '          <button type="submit" class="pd-cart-btn"><i class="bi bi-send"></i> Guardar reseña</button>'
            + '      </div>'
            + '  </form>'            + '  <div id="pdReviewsList" class="pd-reviews-list"><div class="pd-review-empty">Cargando reseñas...</div></div>'
            + '</section>';

        setRating(5);
    }

    function renderResenas(data) {
        var summary = document.getElementById('pdReviewsSummary');
        var list = document.getElementById('pdReviewsList');
        var mode = document.getElementById('pdReviewMode');
        var comentario = document.getElementById('pdComentarioResena');
        var resenas = data && Array.isArray(data.Resenas) ? data.Resenas : [];
        var total = Number(data && data.TotalResenas ? data.TotalResenas : 0);
        var promedio = Number(data && data.PromedioCalificacion ? data.PromedioCalificacion : 0);
        var miResena = null;

        if (summary) {
            summary.innerHTML = total > 0
                ? '<span>' + ratingStarsHtml(promedio) + '</span><strong>' + promedio.toFixed(1) + ' / 5</strong><small>' + total + ' reseña(s)</small>'
                : '<strong>Sin reseñas todavía</strong><small>Sé el primero en opinar.</small>';
        }

        resenas.forEach(function (r) {
            if (r.EsMia === true || r.EsMia === 1 || r.EsMia === '1') {
                miResena = r;
            }
        });

        if (miResena) {
            if (mode) { mode.textContent = 'Ya escribiste una reseña. Puedes actualizar la calificación y guardar un nuevo comentario.'; }
            if (comentario) { comentario.value = ''; }
            setRating(Number(miResena.Calificacion || 5));
        } else {
            if (mode) { mode.textContent = 'Tu opinión ayuda a otros clientes.'; }
            if (comentario) { comentario.value = ''; }
            setRating(5);
        }

        if (!list) { return; }

        if (!resenas.length) {
            list.innerHTML = ''
                + '<div class="pd-review-empty">'
                + '  <i class="bi bi-chat-heart"></i>'
                + '  <strong>Aún no hay reseñas</strong>'
                + '  <span>Cuando los clientes valoren este producto, aparecerán aquí.</span>'
                + '</div>';
            return;
        }

        list.innerHTML = resenas.map(function (r) {
            var calificacion = Number(r.Calificacion || 0);
            var esMia = r.EsMia === true || r.EsMia === 1 || r.EsMia === '1';
            var comentarioTexto = 'Valoración registrada.';

            return ''
                + '<article class="pd-review-card ' + (esMia ? 'pd-review-card--mine' : '') + '">'
                + '  <div class="pd-review-avatar">' + escapeHtml(String(r.ClienteNombre || 'C').substring(0, 1).toUpperCase()) + '</div>'
                + '  <div class="pd-review-content">'
                + '      <div class="pd-review-head">'
                + '          <div><strong>' + escapeHtml(r.ClienteNombre || 'Cliente') + '</strong><span>' + escapeHtml(r.Fecha || 'Fecha no disponible') + '</span></div>'
                + '          <div class="pd-review-stars">' + ratingStarsHtml(calificacion) + '<b>' + calificacion.toFixed(1) + '</b></div>'
                + '      </div>'
                + '      <p>' + escapeHtml(comentarioTexto) + '</p>'
                + (esMia ? '<button type="button" class="pd-review-delete" data-delete-review="' + escapeHtml(r.ResenaId || 0) + '"><i class="bi bi-trash"></i> Eliminar mi reseña</button>' : '')
                + '  </div>'
                + '</article>';
        }).join('');
    }

    function cargarResenas() {
        var list = document.getElementById('pdReviewsList');
        if (list) { list.innerHTML = '<div class="pd-review-empty">Cargando reseñas...</div>'; }

        getJson(endpointResenas)
            .then(function (r) {
                renderResenas(r.data || {});
            })
            .catch(function (error) {
                if (list) {
                    list.innerHTML = '<div class="pd-review-empty">' + escapeHtml(error.message || 'No se pudieron cargar las reseñas.') + '</div>';
                }
            });
    }

    function cargarDetalle() {
        getJson(endpointDetalle)
            .then(function (respuesta) {
                if (!respuesta.data) { throw new Error(respuesta.message || 'No se pudo cargar el detalle del producto.'); }
                renderDetalle(respuesta.data);
                cargarResenas();
            })
            .catch(function (error) {
                container.innerHTML = '<div class="pd-loading">' + escapeHtml(error.message || 'Ocurrio un error al cargar el producto.') + '</div>';
            });
    }

    container.addEventListener('click', function (e) {
        var ratingBtn = e.target.closest('button[data-review-rating]');
        if (ratingBtn) {
            e.preventDefault();
            setRating(Number(ratingBtn.getAttribute('data-review-rating') || 5));
            return;
        }

        var deleteBtn = e.target.closest('[data-delete-review]');
        if (deleteBtn) {
            e.preventDefault();
            if (!confirm('¿Eliminar tu reseña de este producto?')) { return; }

            deleteBtn.disabled = true;
            postJson(endpointEliminarResena, { resenaId: Number(deleteBtn.getAttribute('data-delete-review') || 0) })
                .then(function (r) {
                    mostrarToast(r.message || 'Reseña eliminada correctamente.', 'success');
                    cargarDetalle();
                })
                .catch(function (error) {
                    mostrarToast(error.message || 'No se pudo eliminar la reseña.', 'error');
                })
                .finally(function () { deleteBtn.disabled = false; });
            return;
        }

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

    container.addEventListener('submit', function (e) {
        var form = e.target.closest('#pdReviewForm');
        if (!form) { return; }
        e.preventDefault();

        var comentario = document.getElementById('pdComentarioResena');
        var boton = form.querySelector('button[type="submit"]');

        if (boton) { boton.disabled = true; }

        postJson(endpointCrearResena, {
            productoId: Number(productoId),
            calificacion: ratingSeleccionado,
            comentario: comentario ? comentario.value : ''
        })
            .then(function (r) {
                mostrarToast(r.message || 'Reseña guardada correctamente.', 'success');
                cargarDetalle();
            })
            .catch(function (error) {
                mostrarToast(error.message || 'No se pudo guardar la reseña.', 'error');
            })
            .finally(function () {
                if (boton) { boton.disabled = false; }
            });
    });

    cargarDetalle();
});