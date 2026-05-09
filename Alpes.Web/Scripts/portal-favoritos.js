(function () {
    'use strict';

    var container = document.getElementById('pfFavoritosContainer');
    if (!container) return;

    function obtenerValor(obj, keys, fallback) {
        if (!obj) return fallback;

        for (var i = 0; i < keys.length; i += 1) {
            if (obj[keys[i]] !== undefined && obj[keys[i]] !== null && obj[keys[i]] !== '') {
                return obj[keys[i]];
            }
        }

        return fallback;
    }

    function formatoQuetzal(valor) {
        var numero = Number(valor || 0);

        if (Number.isNaN(numero)) {
            numero = 0;
        }

        return 'Q' + numero.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

   

    function renderVacio(mensaje) {
        container.innerHTML =
            '<div class="fav-empty-card">' +
            '<div class="fav-empty-icon"><i class="bi bi-heart"></i></div>' +
            '<strong>No tienes favoritos</strong>' +
            '<span>' + (mensaje || 'Aún no has agregado productos.') + '</span>' +
            '</div>';
    }

    function quitarFavorito(listaDeseosId) {
        return fetch('/PortalCliente/QuitarFavorito', {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({
                listaDeseosId: listaDeseosId
            })
        }).then(function (r) {
            return r.json();
        });
    }

    function renderFavoritos(items) {
        if (!items || !items.length) {
            renderVacio('No tienes productos marcados como favoritos.');
            return;
        }

        var html = '';

        for (var i = 0; i < items.length; i += 1) {
            var item = items[i] || {};
            var producto = item.Producto || item.producto || item;

            var listaDeseosId = obtenerValor(item, [
                'ListaDeseosId',
                'LISTA_DESEOS_ID',
                'listaDeseosId'
            ], '');

            var productoId = obtenerValor(producto, [
                'ProductoId',
                'PRODUCTO_ID',
                'productoId'
            ], '');

            var nombre = obtenerValor(producto, [
                'Nombre',
                'NOMBRE',
                'nombre'
            ], 'Producto');

            var descripcion = obtenerValor(producto, [
                'Descripcion',
                'DESCRIPCION',
                'descripcion'
            ], 'Sin descripción');

            var referencia = obtenerValor(producto, [
                'Referencia',
                'REFERENCIA',
                'referencia'
            ], '');

            var material = obtenerValor(producto, [
                'Material',
                'MATERIAL',
                'material'
            ], '');

            var color = obtenerValor(producto, [
                'Color',
                'COLOR',
                'color'
            ], '');

            var imagen = obtenerValor(producto, [
                'ImagenUrl',
                'imagenUrl',
                'UrlImagen'
            ], '');

            var precio = obtenerValor(producto, [
                'PrecioActual',
                'PRECIO_ACTUAL',
                'Precio',
                'PRECIO',
                'precio'
            ], 0);

            var tipo = obtenerValor(producto, ['Tipo', 'TIPO', 'tipo'], 'INTERIOR');
            var precioTexto = formatoQuetzal(precio);

            html +=
                '<article class="fav-card catalog-card">' +
                '<div class="fav-image catalog-card-image">' +
                '<img src="' + imagen + '" alt="' + nombre + '">' +
                '<button type="button" class="fav-heart active" data-remove="' + listaDeseosId + '" title="Quitar de favoritos">' +
                '<i class="bi bi-heart-fill"></i>' +
                '</button>' +
                '</div>' +

                '<div class="fav-body">' +
                '<span class="fav-category">' + tipo + '</span>' +
                '<h3 class="fav-title">' + nombre + '</h3>' +
                '<p class="fav-desc">' + descripcion + '</p>' +

                '<div class="fav-meta">' +
                (referencia ? '<span class="fav-tag"><i class="bi bi-tag"></i> ' + referencia + '</span>' : '') +
                (material ? '<span class="fav-tag">' + material + '</span>' : '') +
                (color ? '<span class="fav-tag">' + color + '</span>' : '') +
                '</div>' +

                '<strong class="fav-price">' + precioTexto + '</strong>' +
                '<span class="fav-status">Disponible</span>' +

                '<div class="fav-actions">' +
                '<a href="/PortalCliente/DetalleProducto/' + productoId + '" class="fav-btn fav-btn-detail">Ver detalle</a>' +
                '<button type="button" class="fav-btn fav-btn-cart" data-add-cart="' + productoId + '">' +
                '<i class="bi bi-cart"></i> Agregar' +
                '</button>' +
                '</div>' +
                '</div>' +
                '</article>';
        }

        container.innerHTML = html;
    }

    function cargarFavoritos() {
        fetch('/PortalCliente/ObtenerFavoritosData', {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (r) {
                return r.json();
            })
            .then(function (res) {
                var data = res.data || res.Data || res || [];
                renderFavoritos(data);
            })
            .catch(function () {
                renderVacio('No fue posible cargar los favoritos.');
            });
    }

    container.addEventListener('click', function (e) {
        var btn = e.target.closest('[data-remove]');
        var btnCart = e.target.closest('[data-add-cart]');
        if (btnCart) {
            var productoId = btnCart.getAttribute('data-add-cart');

            agregarAlCarrito(productoId)
                .then(function () {
                    mostrarToastFavoritos('Producto agregado al carrito.', 'success');

                    try {
                        document.dispatchEvent(new CustomEvent('pc:cart-updated'));
                    } catch (e) { }

                    actualizarBadgeCarritoFavoritos();
                })
                .catch(function () {
                    mostrarToastFavoritos('No se pudo agregar el producto al carrito.', 'error');
                });

            return;
        }
        if (!btn) return;

        var id = btn.getAttribute('data-remove');
        if (!id) return;

        quitarFavorito(id)
            .then(function () {
                function actualizarBadgeCarritoFavoritos() {
                    fetch('/PortalCliente/ObtenerCarritoData', {
                        method: 'GET',
                        credentials: 'same-origin',
                        headers: {
                            'X-Requested-With': 'XMLHttpRequest'
                        }
                    })
                        .then(function (r) { return r.json(); })
                        .then(function (res) {
                            var data = res.data || res.Data || res;
                            var items = data.Items || data.items || [];
                            var total = 0;

                            for (var i = 0; i < items.length; i += 1) {
                                total += Number(items[i].Cantidad || items[i].cantidad || 0);
                            }

                            var badges = document.querySelectorAll('#pcTopbarCartBadge, #pcBadgeCart, [data-cart-count], .js-cart-count');

                            for (var j = 0; j < badges.length; j += 1) {
                                badges[j].textContent = total;
                                badges[j].style.display = total > 0 ? '' : 'none';
                            }
                        });
                }
                cargarFavoritos();
            })
            .catch(function () {
                alert('No se pudo quitar el favorito.');
            });
    });
    function mostrarToastFavoritos(mensaje, tipo) {
        var toast = document.createElement('div');
        toast.className = 'fav-toast fav-toast--' + (tipo || 'success');
        toast.innerHTML =
            '<div class="fav-toast-icon">' +
            (tipo === 'error' ? '!' : '<i class="bi bi-check-lg"></i>') +
            '</div>' +
            '<div>' +
            '<strong>' + mensaje + '</strong>' +
            '<span>Muebles de los Alpes</span>' +
            '</div>';

        document.body.appendChild(toast);

        setTimeout(function () {
            toast.classList.add('show');
        }, 20);

        setTimeout(function () {
            toast.classList.remove('show');
            setTimeout(function () {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 250);
        }, 2600);
    }
    function agregarAlCarrito(productoId) {
        return fetch('/PortalCliente/AgregarAlCarritoData', {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({
                productoId: Number(productoId),
                cantidad: 1
            })
        })
            .then(function (response) {
                return response.json().catch(function () {
                    return null;
                }).then(function (payload) {
                    if (!response.ok) {
                        throw new Error('No se pudo agregar al carrito.');
                    }

                    if (payload && (payload.ok === false || payload.success === false)) {
                        throw new Error(payload.message || payload.mensaje || 'No se pudo agregar al carrito.');
                    }

                    try {
                        document.dispatchEvent(new CustomEvent('pc:cart-updated'));
                    } catch (e) { }

                    return payload;
                });
            });
    }
    cargarFavoritos();
})();
   