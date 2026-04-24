document.addEventListener('DOMContentLoaded', function () {
    var container = document.getElementById('pfFavoritosContainer');

    if (!container) {
        return;
    }

    var endpointListar = '/PortalCliente/ObtenerFavoritosData';
    var endpointQuitar = '/PortalCliente/QuitarFavorito';

    function normalizarRespuesta(payload) {
        if (!payload) {
            return { ok: false, data: null, message: 'Respuesta vacía del servidor.' };
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

        return {
            ok: true,
            data: payload,
            message: ''
        };
    }

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function renderVacio(mensaje) {
        container.innerHTML = ''
            + '<div class="pf-card pf-card-empty">'
            + '  <div class="pf-info">'
            + '      <div class="pf-name">Sin favoritos</div>'
            + '      <div class="pf-desc">' + escapeHtml(mensaje || 'No hay productos favoritos para mostrar.') + '</div>'
            + '  </div>'
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
            var item = items[i];
            var producto = item.Producto || {};
            var imagen = producto.ImagenUrl ? escapeHtml(producto.ImagenUrl) : '';
            var imagenHtml = imagen !== ''
                ? '<div class="pf-image"><img src="' + imagen + '" alt="' + escapeHtml(producto.Nombre || 'Producto') + '"></div>'
                : '<div class="pf-image pf-image-empty"><span>Sin imagen</span></div>';

            html += ''
                + '<div class="pf-card" data-lista-deseos-id="' + escapeHtml(item.ListaDeseosId) + '" data-producto-id="' + escapeHtml(producto.ProductoId) + '">'
                + imagenHtml
                + '  <div class="pf-info">'
                + '      <div class="pf-name">' + escapeHtml(producto.Nombre || 'Producto sin nombre') + '</div>'
                + '      <div class="pf-desc">' + escapeHtml(producto.Descripcion || producto.Tipo || 'Sin descripción disponible') + '</div>'
                + '      <div class="pf-meta">'
                + '          <span>' + escapeHtml(producto.Referencia || '') + '</span>'
                + '          <span>' + escapeHtml(producto.Material || '') + '</span>'
                + '          <span>' + escapeHtml(producto.Color || '') + '</span>'
                + '      </div>'
                + '      <div class="pf-actions">'
                + '          <button type="button" class="pf-remove-btn" data-action="remove" data-id="' + escapeHtml(item.ListaDeseosId) + '">Quitar</button>'
                + '      </div>'
                + '  </div>'
                + '</div>';
        }

        container.innerHTML = html;
    }

    function cargarFavoritos() {
        fetch(endpointListar, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudieron obtener los favoritos.');
                }
                return response.json();
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
            return;
        }

        if (!window.confirm('¿Deseas quitar este producto de tus favoritos?')) {
            return;
        }

        quitarFavorito(listaDeseosId)
            .then(function (respuesta) {
                alert(respuesta.message || 'Favorito eliminado correctamente.');
                cargarFavoritos();
            })
            .catch(function (error) {
                alert(error.message || 'No se pudo eliminar el favorito.');
            });
    });

    cargarFavoritos();
});