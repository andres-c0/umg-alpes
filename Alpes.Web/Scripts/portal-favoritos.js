document.addEventListener('DOMContentLoaded', function () {
    var container = document.getElementById('pfFavoritosContainer');

    if (!container) {
        return;
    }

    var endpointListar = '/PortalCliente/ObtenerFavoritosData';
    var endpointQuitar = '/PortalCliente/QuitarFavorito';

    function normalizarRespuesta(payload) {
        if (!payload) {
            return { ok: false, data: [], message: 'Respuesta vacía del servidor.' };
        }

        var data = [];
        if (Array.isArray(payload)) {
            data = payload;
        } else if (Array.isArray(payload.data)) {
            data = payload.data;
        } else if (payload.data && Array.isArray(payload.data.favoritos)) {
            data = payload.data.favoritos;
        } else if (payload.favoritos && Array.isArray(payload.favoritos)) {
            data = payload.favoritos;
        }

        if (payload.ok !== undefined || payload.success !== undefined) {
            return {
                ok: payload.ok === true || payload.success === true,
                data: data,
                message: payload.message || payload.mensaje || ''
            };
        }

        return {
            ok: true,
            data: data,
            message: ''
        };
    }

    function escapeHtml(value) {
        return String(value === null || value === undefined ? '' : value)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function obtenerValor(obj, nombres, defecto) {
        var i;
        if (!obj) {
            return defecto || '';
        }
        for (i = 0; i < nombres.length; i += 1) {
            if (obj[nombres[i]] !== undefined && obj[nombres[i]] !== null && obj[nombres[i]] !== '') {
                return obj[nombres[i]];
            }
        }
        return defecto || '';
    }

    function formatoQuetzal(valor) {
        var numero = Number(valor || 0);
        if (Number.isNaN(numero)) {
            numero = 0;
        }
        return 'Q' + numero.toFixed(2);
    }

    function mostrarToast(mensaje, tipo) {
        var toast = document.createElement('div');
        toast.className = 'pc-toast pc-toast--' + (tipo || 'ok');
        toast.textContent = mensaje;
        document.body.appendChild(toast);
        window.setTimeout(function () { toast.classList.add('show'); }, 10);
        window.setTimeout(function () {
            toast.classList.remove('show');
            window.setTimeout(function () { toast.remove(); }, 220);
        }, 2600);
    }

    function renderVacio(mensaje) {
        container.innerHTML = ''
            + '<div class="fav-empty-card">'
            + '  <div class="fav-empty-icon"><i class="bi bi-heart"></i></div>'
            + '  <strong>Sin favoritos</strong>'
            + '  <span>' + escapeHtml(mensaje || 'No hay productos favoritos para mostrar.') + '</span>'
            + '  <a href="/PortalCliente/Index#catalogo" class="fav-empty-link">Explorar catálogo</a>'
            + '</div>';
    }

    function renderFavoritos(items) {
        if (!items || !items.length) {
            renderVacio('No tienes productos marcados como favoritos.');
            return;
        }

        var html = '';
        var i;

        for (i = 0; i < items.length; i += 1) {
            var item = items[i] || {};
            var producto = item.Producto || item.producto || item;
            var listaDeseosId = obtenerValor(item, ['ListaDeseosId', 'LISTA_DESEOS_ID', 'listaDeseosId', 'lista_deseos_id'], '');
            var productoId = obtenerValor(producto, ['ProductoId', 'PRODUCTO_ID', 'productoId', 'producto_id'], '');
            var nombre = obtenerValor(producto, ['Nombre', 'NOMBRE', 'nombre'], 'Producto sin nombre');
            var descripcion = obtenerValor(producto, ['Descripcion', 'DESCRIPCION', 'descripcion', 'Tipo', 'TIPO', 'tipo'], 'Sin descripción disponible');
            var referencia = obtenerValor(producto, ['Referencia', 'REFERENCIA', 'referencia', 'Codigo', 'CODIGO', 'codigo'], '');
            var material = obtenerValor(producto, ['Material', 'MATERIAL', 'material'], '');
            var color = obtenerValor(producto, ['Color', 'COLOR', 'color'], '');
            var imagen = obtenerValor(producto, ['ImagenUrl', 'IMAGEN_URL', 'imagenUrl', 'imagen_url', 'UrlImagen', 'URL_IMAGEN'], '');
            var precio = obtenerValor(producto, ['PrecioActual', 'PRECIO_ACTUAL', 'Precio', 'PRECIO', 'precio'], 0);

            html += ''
                + '<article class="fav-card" data-lista-deseos-id="' + escapeHtml(listaDeseosId) + '" data-producto-id="' + escapeHtml(productoId) + '">'
                + '  <a class="fav-image" href="/PortalCliente/DetalleProducto/' + escapeHtml(productoId) + '">'
                + (imagen !== ''
                    ? '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(nombre) + '">'
                    : '<div class="fav-no-image"><i class="bi bi-image"></i><span>Sin imagen</span></div>')
                + '  </a>'
                + '  <div class="fav-body">'
                + '      <div class="fav-body-top">'
                + '          <h3>' + escapeHtml(nombre) + '</h3>'
                + '          <button type="button" class="fav-remove-icon" data-action="remove" data-id="' + escapeHtml(listaDeseosId) + '" aria-label="Quitar favorito"><i class="bi bi-x-lg"></i></button>'
                + '      </div>'
                + '      <p>' + escapeHtml(descripcion) + '</p>'
                + '      <div class="fav-meta">'
                + (referencia ? '<span>' + escapeHtml(referencia) + '</span>' : '')
                + (material ? '<span>' + escapeHtml(material) + '</span>' : '')
                + (color ? '<span>' + escapeHtml(color) + '</span>' : '')
                + '      </div>'
                + '      <div class="fav-footer">'
                + '          <strong>' + formatoQuetzal(precio) + '</strong>'
                + '          <a href="/PortalCliente/DetalleProducto/' + escapeHtml(productoId) + '" class="fav-detail-btn">Ver detalle</a>'
                + '      </div>'
                + '  </div>'
                + '</article>';
        }

        container.innerHTML = html;
    }

    function cargarFavoritos() {
        fetch(endpointListar + '?_=' + Date.now(), {
            method: 'GET',
            credentials: 'same-origin',
            cache: 'no-store',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                return response.text().then(function (text) {
                    var payload = null;
                    if (text) {
                        try {
                            payload = JSON.parse(text);
                        } catch (e) {
                            throw new Error('La sesión pudo haber expirado o el servidor devolvió HTML en lugar de JSON.');
                        }
                    }
                    if (!response.ok) {
                        var msg = payload && (payload.message || payload.mensaje) ? (payload.message || payload.mensaje) : 'No se pudieron obtener los favoritos.';
                        throw new Error(msg);
                    }
                    return payload;
                });
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudieron obtener los favoritos.');
                }

                renderFavoritos(respuesta.data || []);
            })
            .catch(function (error) {
                renderVacio(error.message || 'Ocurrió un error al consultar los favoritos.');
            });
    }

    function quitarFavorito(listaDeseosId) {
        return fetch(endpointQuitar, {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ listaDeseosId: listaDeseosId })
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo eliminar el favorito.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudo eliminar el favorito.');
                }

                return respuesta;
            });
    }

    container.addEventListener('click', function (e) {
        var button = e.target.closest('button[data-action="remove"]');
        if (!button) {
            return;
        }

        var listaDeseosId = button.getAttribute('data-id');
        if (!listaDeseosId) {
            mostrarToast('No se encontró el identificador del favorito.', 'error');
            return;
        }

        button.disabled = true;
        button.classList.add('is-loading');

        quitarFavorito(listaDeseosId)
            .then(function (respuesta) {
                mostrarToast(respuesta.message || respuesta.mensaje || 'Favorito eliminado correctamente.', 'ok');
                cargarFavoritos();
            })
            .catch(function (error) {
                button.disabled = false;
                button.classList.remove('is-loading');
                mostrarToast(error.message || 'No se pudo eliminar el favorito.', 'error');
            });
    });

    cargarFavoritos();
});
