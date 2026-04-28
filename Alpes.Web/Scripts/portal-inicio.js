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

        return {
            ok: true,
            data: payload,
            message: ''
        };
    }

    function renderEmpty(container, message) {
        container.innerHTML = '<div class="pi-empty-card">' + escapeHtml(message) + '</div>';
    }

    function getProductoDesdeItem(item) {
        return item && item.Producto ? item.Producto : {};
    }

    function construirCard(item, mostrarScore) {
        var producto = getProductoDesdeItem(item);
        var imagen = producto.ImagenUrl ? escapeHtml(producto.ImagenUrl) : '';
        var descripcion = producto.Descripcion || producto.Tipo || 'Producto disponible';
        var productoId = producto.ProductoId || 0;
        var esFavorito = item.EsFavorito === true || item.EsFavorito === 1 || item.EsFavorito === '1';
        var listaDeseosId = item.ListaDeseosId || 0;
        var textoBoton = esFavorito ? 'Quitar de favoritos' : 'Agregar a favoritos';
        var claseBoton = esFavorito ? 'pi-fav-btn pi-fav-btn--active' : 'pi-fav-btn';

        var scoreHtml = '';
        if (mostrarScore && item.Score !== undefined && item.Score !== null) {
            scoreHtml = '<div class="pi-score">Afinidad: ' + escapeHtml(item.Score) + '</div>';
        }

        return ''
            + '<div class="pi-card" data-producto-id="' + escapeHtml(productoId) + '">'
            + '  <div class="pi-card-image">'
            + (imagen !== ''
                ? '<img src="' + imagen + '" alt="' + escapeHtml(producto.Nombre || 'Producto') + '">'
                : '<div class="pi-no-image">Sin imagen</div>')
            + '  </div>'
            + '  <div class="pi-card-body">'
            + '      <div class="pi-card-title">' + escapeHtml(producto.Nombre || 'Producto sin nombre') + '</div>'
            + '      <div class="pi-card-desc">' + escapeHtml(descripcion) + '</div>'
            + '      <div class="pi-card-meta">'
            + '          <span>' + escapeHtml(producto.Referencia || '') + '</span>'
            + '          <span>' + escapeHtml(producto.Material || '') + '</span>'
            + '          <span>' + escapeHtml(producto.Color || '') + '</span>'
            + '      </div>'
            + scoreHtml
            + '      <div class="pi-card-actions">'
            + '          <a class="pi-detail-btn" href="/PortalCliente/DetalleProducto/' + escapeHtml(productoId) + '">Ver detalle</a>'
            + '          <button type="button" class="pi-cart-btn" data-action="carrito" data-id="' + escapeHtml(productoId) + '">Agregar al carrito</button>'
            + '          <button type="button"'
            + '                  class="' + claseBoton + '"'
            + '                  data-action="' + (esFavorito ? 'quitar' : 'agregar') + '"'
            + '                  data-id="' + escapeHtml(productoId) + '"'
            + '                  data-lista-deseos-id="' + escapeHtml(listaDeseosId) + '">'
            + textoBoton
            + '          </button>'
            + '      </div>'
            + '  </div>'
            + '</div>';
    }

    function renderCards(container, items, mostrarScore, emptyText) {
        if (!items || !items.length) {
            renderEmpty(container, emptyText);
            return;
        }

        var html = '';
        var i;

        for (i = 0; i < items.length; i += 1) {
            html += construirCard(items[i], mostrarScore);
        }

        container.innerHTML = html;
    }

    function cargarRecomendados() {
        fetch(endpointRecomendados, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudieron cargar las recomendaciones.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudieron cargar las recomendaciones.');
                }

                renderCards(
                    recomendacionesContainer,
                    respuesta.data || [],
                    true,
                    'Aun no hay productos recomendados disponibles.'
                );
            })
            .catch(function (error) {
                renderEmpty(recomendacionesContainer, error.message || 'Ocurrio un error al consultar las recomendaciones.');
            });
    }

    function cargarCatalogo() {
        fetch(endpointCatalogo, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo obtener el catalogo.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudo obtener el catalogo.');
                }

                catalogoCompleto = respuesta.data || [];
                cargarOpcionesFiltros(catalogoCompleto);
                aplicarFiltrosLocales();
            })
            .catch(function (error) {
                renderEmpty(catalogoContainer, error.message || 'Ocurrio un error al cargar el catalogo.');
            });
    }

    function agregarOpcion(select, value, label) {
        var option = document.createElement('option');
        option.value = value;
        option.textContent = label;
        select.appendChild(option);
    }

    function cargarOpcionesFiltros(items) {
        if (!filtroCategoria || !filtroTipo || !filtroColor || !filtroMaterial) {
            return;
        }

        var categorias = {};
        var tipos = {};
        var colores = {};
        var materiales = {};
        var i;

        filtroCategoria.innerHTML = '<option value="">Todas las categorias</option>';
        filtroTipo.innerHTML = '<option value="">Todos los tipos</option>';
        filtroColor.innerHTML = '<option value="">Todos los colores</option>';
        filtroMaterial.innerHTML = '<option value="">Todos los materiales</option>';

        for (i = 0; i < items.length; i += 1) {
            var producto = getProductoDesdeItem(items[i]);

            if (producto.CategoriaId !== undefined && producto.CategoriaId !== null && producto.CategoriaId !== '') {
                categorias[String(producto.CategoriaId)] = 'Categoria ' + producto.CategoriaId;
            }

            if (String(producto.Tipo || '').trim() !== '') {
                tipos[String(producto.Tipo)] = String(producto.Tipo);
            }

            if (String(producto.Color || '').trim() !== '') {
                colores[String(producto.Color)] = String(producto.Color);
            }

            if (String(producto.Material || '').trim() !== '') {
                materiales[String(producto.Material)] = String(producto.Material);
            }
        }

        Object.keys(categorias).sort().forEach(function (key) {
            agregarOpcion(filtroCategoria, key, categorias[key]);
        });

        Object.keys(tipos).sort().forEach(function (key) {
            agregarOpcion(filtroTipo, key, tipos[key]);
        });

        Object.keys(colores).sort().forEach(function (key) {
            agregarOpcion(filtroColor, key, colores[key]);
        });

        Object.keys(materiales).sort().forEach(function (key) {
            agregarOpcion(filtroMaterial, key, materiales[key]);
        });
    }

    function coincideBusqueda(producto, texto) {
        var q = normalizarTexto(texto);
        if (q === '') {
            return true;
        }

        var fuente = [
            producto.Nombre,
            producto.Referencia,
            producto.Descripcion,
            producto.Tipo,
            producto.Material,
            producto.Color
        ].join(' ');

        return normalizarTexto(fuente).indexOf(q) >= 0;
    }

    function aplicarFiltrosLocales() {
        var q = inputBuscar ? inputBuscar.value : '';
        var categoria = filtroCategoria ? filtroCategoria.value : '';
        var tipo = filtroTipo ? filtroTipo.value : '';
        var color = filtroColor ? filtroColor.value : '';
        var material = filtroMaterial ? filtroMaterial.value : '';

        var filtrados = [];
        var i;

        for (i = 0; i < catalogoCompleto.length; i += 1) {
            var item = catalogoCompleto[i];
            var producto = getProductoDesdeItem(item);

            if (!coincideBusqueda(producto, q)) {
                continue;
            }

            if (categoria !== '' && String(producto.CategoriaId) !== String(categoria)) {
                continue;
            }

            if (tipo !== '' && normalizarTexto(producto.Tipo) !== normalizarTexto(tipo)) {
                continue;
            }

            if (color !== '' && normalizarTexto(producto.Color) !== normalizarTexto(color)) {
                continue;
            }

            if (material !== '' && normalizarTexto(producto.Material) !== normalizarTexto(material)) {
                continue;
            }

            filtrados.push(item);
        }

        renderCards(
            catalogoContainer,
            filtrados,
            false,
            'No se encontraron productos con los filtros seleccionados.'
        );
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

    function agregarFavorito(productoId) {
        return postJson(endpointAgregarFavorito, { productoId: Number(productoId) });
    }

    function quitarFavorito(listaDeseosId, productoId) {
        var payload = {};

        if (listaDeseosId && Number(listaDeseosId) > 0) {
            payload.listaDeseosId = Number(listaDeseosId);
        } else {
            payload.productoId = Number(productoId);
        }

        return postJson(endpointQuitarFavorito, payload);
    }

    function agregarAlCarrito(productoId) {
        return postJson(endpointAgregarCarrito, {
            productoId: Number(productoId),
            cantidad: 1
        });
    }

    function refrescarTodo() {
        cargarCatalogo();
        cargarRecomendados();
    }

    function manejarClickFavorito(e) {
        var button = e.target.closest('button[data-action]');
        if (!button) {
            return;
        }

        var action = button.getAttribute('data-action');
        var productoId = button.getAttribute('data-id');
        var listaDeseosId = button.getAttribute('data-lista-deseos-id');

        if (!productoId) {
            return;
        }

        button.disabled = true;
        button.textContent = 'Procesando...';

        var promesa = null;

        if (action === 'agregar') {
            promesa = agregarFavorito(productoId);
        } else if (action === 'quitar') {
            promesa = quitarFavorito(listaDeseosId, productoId);
        } else if (action === 'carrito') {
            promesa = agregarAlCarrito(productoId);
        }

        if (!promesa) {
            button.disabled = false;
            return;
        }

        promesa
            .then(function (respuesta) {
                alert(respuesta.message || 'Operacion realizada correctamente.');
                refrescarTodo();
            })
            .catch(function (error) {
                alert(error.message || 'No se pudo completar la operacion.');
            })
            .finally(function () {
                button.disabled = false;
            });
    }

    if (btnBuscar) {
        btnBuscar.addEventListener('click', aplicarFiltrosLocales);
    }

    if (btnLimpiar) {
        btnLimpiar.addEventListener('click', function () {
            if (inputBuscar) {
                inputBuscar.value = '';
            }
            if (filtroCategoria) {
                filtroCategoria.value = '';
            }
            if (filtroTipo) {
                filtroTipo.value = '';
            }
            if (filtroColor) {
                filtroColor.value = '';
            }
            if (filtroMaterial) {
                filtroMaterial.value = '';
            }

            aplicarFiltrosLocales();
        });
    }

    if (inputBuscar) {
        inputBuscar.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                aplicarFiltrosLocales();
            }
        });
    }

    if (filtroCategoria) {
        filtroCategoria.addEventListener('change', aplicarFiltrosLocales);
    }
    if (filtroTipo) {
        filtroTipo.addEventListener('change', aplicarFiltrosLocales);
    }
    if (filtroColor) {
        filtroColor.addEventListener('change', aplicarFiltrosLocales);
    }
    if (filtroMaterial) {
        filtroMaterial.addEventListener('change', aplicarFiltrosLocales);
    }

    recomendacionesContainer.addEventListener('click', manejarClickFavorito);
    catalogoContainer.addEventListener('click', manejarClickFavorito);

    cargarCatalogo();
    cargarRecomendados();
});