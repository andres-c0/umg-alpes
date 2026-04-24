document.addEventListener('DOMContentLoaded', function () {
    var page = document.querySelector('.pd-page');
    var container = document.getElementById('pdDetalleContainer');

    if (!page || !container) {
        return;
    }

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

    function postJson(url, payload) {
        return fetch(url, {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(payload)
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo procesar la solicitud.');
                }
                return response.json();
            })
            .then(function (payloadRespuesta) {
                var respuesta = normalizarRespuesta(payloadRespuesta);
                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudo procesar la solicitud.');
                }
                return respuesta;
            });
    }

    function renderDetalle(item) {
        var producto = item.Producto || {};
        var esFavorito = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        var listaDeseosId = item.ListaDeseosId || 0;

        container.innerHTML = ''
            + '<div class="pd-image-box">'
            + (producto.ImagenUrl
                ? '<img src="' + escapeHtml(producto.ImagenUrl) + '" alt="' + escapeHtml(producto.Nombre || 'Producto') + '">'
                : '<div class="pd-no-image">Sin imagen</div>')
            + '</div>'
            + '<div class="pd-info-box">'
            + '  <div class="pd-reference">' + escapeHtml(producto.Referencia || '') + '</div>'
            + '  <h1 class="pd-title">' + escapeHtml(producto.Nombre || 'Producto') + '</h1>'
            + '  <p class="pd-description">' + escapeHtml(producto.Descripcion || 'Sin descripcion disponible.') + '</p>'
            + '  <div class="pd-tags">'
            + '      <span>' + escapeHtml(producto.Tipo || '') + '</span>'
            + '      <span>' + escapeHtml(producto.Material || '') + '</span>'
            + '      <span>' + escapeHtml(producto.Color || '') + '</span>'
            + '      <span>Categoria ' + escapeHtml(producto.CategoriaId || '') + '</span>'
            + '  </div>'
            + '  <div class="pd-actions">'
            + '      <button type="button" class="pd-cart-btn" data-action="carrito" data-id="' + escapeHtml(producto.ProductoId || 0) + '">Agregar al carrito</button>'
            + '      <button type="button" class="pd-fav-btn ' + (esFavorito ? 'pd-fav-btn--active' : '') + '"'
            + '              data-action="' + (esFavorito ? 'quitar' : 'agregar') + '"'
            + '              data-id="' + escapeHtml(producto.ProductoId || 0) + '"'
            + '              data-lista-deseos-id="' + escapeHtml(listaDeseosId) + '">'
            + (esFavorito ? 'Quitar de favoritos' : 'Agregar a favoritos')
            + '      </button>'
            + '  </div>'
            + '</div>';
    }

    function cargarDetalle() {
        fetch(endpointDetalle, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo cargar el detalle del producto.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);
                if (!respuesta.ok || !respuesta.data) {
                    throw new Error(respuesta.message || 'No se pudo cargar el detalle del producto.');
                }

                renderDetalle(respuesta.data);
            })
            .catch(function (error) {
                container.innerHTML = '<div class="pd-loading">' + escapeHtml(error.message || 'Ocurrio un error al cargar el producto.') + '</div>';
            });
    }

    container.addEventListener('click', function (e) {
        var button = e.target.closest('button[data-action]');
        if (!button) {
            return;
        }

        var action = button.getAttribute('data-action');
        var productoId = button.getAttribute('data-id');
        var listaDeseosId = button.getAttribute('data-lista-deseos-id');

        button.disabled = true;
        button.textContent = 'Procesando...';

        var promesa;

        if (action === 'agregar') {
            promesa = postJson(endpointAgregarFavorito, {
                productoId: Number(productoId)
            });
        } else if (action === 'quitar') {
            if (listaDeseosId && Number(listaDeseosId) > 0) {
                promesa = postJson(endpointQuitarFavorito, {
                    listaDeseosId: Number(listaDeseosId)
                });
            } else {
                promesa = postJson(endpointQuitarFavorito, {
                    productoId: Number(productoId)
                });
            }
        } else if (action === 'carrito') {
            promesa = postJson(endpointAgregarCarrito, {
                productoId: Number(productoId),
                cantidad: 1
            });
        }

        promesa
            .then(function (respuesta) {
                alert(respuesta.message || 'Operacion realizada correctamente.');
                cargarDetalle();
            })
            .catch(function (error) {
                alert(error.message || 'No se pudo completar la operacion.');
            })
            .finally(function () {
                button.disabled = false;
            });
    });