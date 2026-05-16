(function () {
    'use strict';

    var todasLasResenas = [];
    var ratingSeleccionado = 5;
    var productosCatalogo = [];

    function $(id) {
        return document.getElementById(id);
    }

    function formatDate(value) {
        if (!value) return 'Fecha no disponible';

        var date = new Date(value);

        if (isNaN(date.getTime())) {
            return String(value).substring(0, 10);
        }

        return date.toLocaleDateString('es-GT', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    }

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    }

    function getRatingStars(value) {
        var n = Math.max(0, Math.min(5, Math.round(Number(value || 0))));
        var html = '';

        for (var i = 1; i <= 5; i += 1) {
            html += '<i class="bi ' + (i <= n ? 'bi-star-fill' : 'bi-star') + '"></i>';
        }

        return html;
    }

    function showToast(message, type) {
        var existing = document.querySelector('.resena-toast');
        if (existing) existing.remove();

        var toast = document.createElement('div');
        toast.className = 'resena-toast resena-toast--' + (type || 'success');

        toast.innerHTML =
            '<div class="resena-toast-icon">' +
            (type === 'error' ? '!' : '<i class="bi bi-check-lg"></i>') +
            '</div>' +
            '<div>' +
            '<strong>' + escapeHtml(message) + '</strong>' +
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

    function getJson(url, options) {
        return fetch(url, options || {}).then(function (res) {
            return res.json().catch(function () {
                return {
                    ok: false,
                    message: 'Respuesta no válida del servidor.'
                };
            });
        });
    }

    function cargarResenas() {
        var loading = $('resenasLoading');
        var empty = $('resenasEmpty');
        var grid = $('resenasGrid');

        if (loading) loading.style.display = 'flex';
        if (empty) empty.style.display = 'none';
        if (grid) grid.innerHTML = '';

        getJson('/PortalCliente/ObtenerMisResenasData')
            .then(function (json) {
                if (!json || !(json.ok || json.success)) {
                    throw new Error(json && json.message ? json.message : 'No se pudieron cargar las reseñas.');
                }

                todasLasResenas = Array.isArray(json.data) ? json.data : [];
                renderResenas();
            })
            .catch(function (err) {
                todasLasResenas = [];
                renderResenas();
                showToast(err.message || 'No se pudieron cargar las reseñas.', 'error');
            })
            .finally(function () {
                if (loading) loading.style.display = 'none';
            });
    }

    function renderStats(lista) {
        var total = lista.length;
        var suma = 0;
        var activas = 0;

        lista.forEach(function (r) {
            suma += Number(r.Calificacion || r.calificacion || 0);

            var estado = String(r.Estado || r.estado || '').toUpperCase();

            if (!estado || estado === 'ACTIVO' || estado === 'PUBLICADO') {
                activas += 1;
            }
        });

        if ($('resenasTotal')) $('resenasTotal').textContent = total;
        if ($('resenasPromedio')) $('resenasPromedio').textContent = total ? (suma / total).toFixed(1) : '0.0';
        if ($('resenasActivas')) $('resenasActivas').textContent = activas;
    }

    function filtrarResenas() {
        var texto = ($('txtBuscarResena') ? $('txtBuscarResena').value : '').toLowerCase().trim();
        var filtro = $('cmbFiltroResena') ? $('cmbFiltroResena').value : 'TODAS';

        return todasLasResenas.filter(function (r) {
            var producto = r.Producto || r.producto || {};
            var cal = Math.round(Number(r.Calificacion || r.calificacion || 0));
            var estado = String(r.Estado || r.estado || '');

            var contenido = [
                producto.Nombre || producto.nombre || '',
                producto.Referencia || producto.referencia || '',
                estado
            ].join(' ').toLowerCase();

            if (filtro !== 'TODAS' && String(cal) !== filtro) return false;
            if (texto && contenido.indexOf(texto) === -1) return false;

            return true;
        });
    }

    function renderResenas() {
        var grid = $('resenasGrid');
        var empty = $('resenasEmpty');

        if (!grid) return;

        var lista = filtrarResenas();

        renderStats(todasLasResenas);

        if (!lista.length) {
            grid.innerHTML = '';

            if (empty) empty.style.display = 'block';

            return;
        }

        if (empty) empty.style.display = 'none';

        grid.innerHTML = lista.map(function (r) {
            var producto = r.Producto || r.producto || {};
            var productoId = r.ProductoId || r.productoId || producto.ProductoId || producto.productoId || 0;
            var nombre = producto.Nombre || producto.nombre || ('Producto #' + productoId);
            var imagen = producto.ImagenUrl || producto.imagenUrl || '';
            var comentarioDisponible = r.ComentarioTextoDisponible === true || r.comentarioTextoDisponible === true;
            var comentario = comentarioDisponible && (r.Comentario || r.comentario)
                ? (r.Comentario || r.comentario)
                : 'Tu reseña está registrada.';
            var estado = r.Estado || r.estado || 'ACTIVO';
            var resenaId = r.ResenaId || r.resenaId || 0;
            var calificacion = r.Calificacion || r.calificacion || 0;

            return '' +
                '<article class="pc-review-card">' +
                '<div class="pc-review-image">' +
                (imagen ? '<img src="' + escapeHtml(imagen) + '" alt="' + escapeHtml(nombre) + '" onerror="this.style.display=\'none\'; this.nextElementSibling.style.display=\'flex\';" />' : '') +
                '<div class="pc-product-placeholder" style="' + (imagen ? 'display:none;' : '') + '"><i class="bi bi-chair"></i></div>' +
                '</div>' +
                '<div class="pc-review-body">' +
                '<div class="pc-review-head">' +
                '<div>' +
                '<h3>' + escapeHtml(nombre) + '</h3>' +
                '<span>Producto #' + escapeHtml(productoId) + ' · ' + escapeHtml(formatDate(r.ResenaAt || r.resenaAt)) + '</span>' +
                '</div>' +
                '<span class="pc-pill pc-pill-pendiente">' + escapeHtml(estado) + '</span>' +
                '</div>' +
                '<div class="pc-review-stars">' + getRatingStars(calificacion) + '<strong>' + Number(calificacion || 0).toFixed(1) + '</strong></div>' +
                '<p>' + escapeHtml(comentario) + '</p>' +
                '<div class="pc-review-actions">' +
                '<a class="pc-btn pc-btn-outlined" href="/PortalCliente/DetalleProducto/' + encodeURIComponent(productoId) + '">Ver producto</a>' +
                '<button type="button" class="pc-btn pc-btn-danger" data-delete-review="' + escapeHtml(resenaId) + '">Eliminar</button>' +
                '</div>' +
                '</div>' +
                '</article>';
        }).join('');
    }

    function cargarProductosCatalogo() {
        var select = $('cmbProductoResena');

        if (!select) return;

        select.innerHTML = '<option value="">Cargando productos...</option>';

        getJson('/PortalCliente/ObtenerCatalogoData')
            .then(function (json) {
                if (!json || !(json.ok || json.success)) {
                    throw new Error(json && json.message ? json.message : 'No se pudieron cargar los productos.');
                }

                productosCatalogo = Array.isArray(json.data) ? json.data : [];

                if (!productosCatalogo.length) {
                    select.innerHTML = '<option value="">No hay productos disponibles</option>';
                    return;
                }

                select.innerHTML = '<option value="">Seleccione un producto</option>' +
                    productosCatalogo.map(function (item) {
                        var producto = item.Producto || item.producto || item;
                        var id = producto.ProductoId || producto.productoId || '';
                        var nombre = producto.Nombre || producto.nombre || 'Producto';
                        var referencia = producto.Referencia || producto.referencia || '';

                        return '<option value="' + escapeHtml(id) + '">' +
                            escapeHtml(nombre) +
                            (referencia ? ' - ' + escapeHtml(referencia) : '') +
                            '</option>';
                    }).join('');
            })
            .catch(function () {
                select.innerHTML = '<option value="">No se pudieron cargar los productos</option>';
            });
    }

    function setRating(valor) {
        ratingSeleccionado = Math.max(1, Math.min(5, Number(valor || 1)));

        document.querySelectorAll('#ratingInput button[data-rating]').forEach(function (btn) {
            var rating = Number(btn.getAttribute('data-rating') || 0);

            if (rating <= ratingSeleccionado) {
                btn.classList.add('active');
                btn.style.setProperty('background', '#431406', 'important');
                btn.style.setProperty('color', '#ffffff', 'important');
            } else {
                btn.classList.remove('active');
                btn.style.setProperty('background', '#f4eadf', 'important');
                btn.style.setProperty('color', '#deb04f', 'important');
            }

            var icon = btn.querySelector('i');
            if (icon) {
                icon.style.setProperty('color', rating <= ratingSeleccionado ? '#ffffff' : '#deb04f', 'important');
            }
        });

        if ($('txtCalificacionResena')) {
            $('txtCalificacionResena').value = ratingSeleccionado;
        }
    }

    function abrirModal() {
        var modal = $('modalNuevaResena');

        if (modal) modal.classList.add('show');

        setRating(5);
        cargarProductosCatalogo();
    }

    function cerrarModal(id) {
        var modal = $(id);

        if (modal) modal.classList.remove('show');
    }

    function guardarResena(evt) {
        evt.preventDefault();

        var productoId = Number($('cmbProductoResena') ? $('cmbProductoResena').value : 0);
        var comentario = $('txtComentarioResena') ? $('txtComentarioResena').value : '';

        if (!productoId) {
            showToast('Selecciona un producto.', 'error');
            return;
        }

        getJson('/PortalCliente/CrearResenaData', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                productoId: productoId,
                calificacion: ratingSeleccionado,
                comentario: comentario
            })
        })
            .then(function (json) {
                if (!json || !(json.ok || json.success)) {
                    throw new Error(json && json.message ? json.message : 'No se pudo guardar la reseña.');
                }

                showToast(json.message || 'Reseña guardada correctamente.', 'success');
                cerrarModal('modalNuevaResena');

                if ($('formNuevaResena')) $('formNuevaResena').reset();

                setRating(5);
                cargarResenas();
            })
            .catch(function (err) {
                showToast(err.message || 'No se pudo guardar la reseña.', 'error');
            });
    }

    function eliminarResena(resenaId) {
        if (!resenaId) return;
        if (!confirm('¿Eliminar esta reseña?')) return;

        getJson('/PortalCliente/EliminarResenaData', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                resenaId: Number(resenaId)
            })
        })
            .then(function (json) {
                if (!json || !(json.ok || json.success)) {
                    throw new Error(json && json.message ? json.message : 'No se pudo eliminar la reseña.');
                }

                showToast(json.message || 'Reseña eliminada.', 'success');
                cargarResenas();
            })
            .catch(function (err) {
                showToast(err.message || 'No se pudo eliminar la reseña.', 'error');
            });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var ratingInput = $('ratingInput');

        cargarResenas();

        if ($('btnAbrirNuevaResena')) {
            $('btnAbrirNuevaResena').addEventListener('click', abrirModal);
        }

        if ($('formNuevaResena')) {
            $('formNuevaResena').addEventListener('submit', guardarResena);
        }

        if ($('txtBuscarResena')) {
            $('txtBuscarResena').addEventListener('input', renderResenas);
        }

        if ($('cmbFiltroResena')) {
            $('cmbFiltroResena').addEventListener('change', renderResenas);
        }

        document.querySelectorAll('[data-close-modal]').forEach(function (el) {
            el.addEventListener('click', function () {
                cerrarModal(el.getAttribute('data-close-modal'));
            });
        });

        if (ratingInput) {
            ratingInput.addEventListener('click', function (evt) {
                var btn = evt.target.closest('button[data-rating]');

                if (!btn) return;

                evt.preventDefault();
                setRating(Number(btn.getAttribute('data-rating')));
            });
        }

        document.addEventListener('click', function (evt) {
            var btn = evt.target.closest('[data-delete-review]');

            if (btn) {
                eliminarResena(btn.getAttribute('data-delete-review'));
            }
        });

        setRating(5);
    });
}());