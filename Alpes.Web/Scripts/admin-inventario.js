(function () {
    var inventario = [];
    var inventarioFiltrado = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarInventario();
    });

    function enlazarEventos() {
        $('#txtBuscarInventario').on('input', function () {
            filtrarInventario();
        });

        $('#btnNuevoInventario').on('click', function () {
            nuevoInventario();
        });

        $('#btnCerrarModalInventarioX, #btnCancelarInventario, #modalInventario .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });

        $('#btnGuardarInventario').on('click', function () {
            guardarInventario();
        });

        $('#txtStockInventario, #txtReservadoInventario, #txtStockMinimoInventario')
            .on('input change keyup', function () {
                $(this).removeClass('input-error');
                actualizarDisponibleEdit();
            });
    }

    function cargarInventario() {
        $('#inventarioListado').html('<div class="table-empty">Cargando inventario...</div>');

        $.getJSON('/Inventario_Producto/Index')
            .done(function (res) {
                if (res && res.success === false) {
                    $('#inventarioListado').html('<div class="table-empty">' + escapeHtml(res.message || 'No fue posible cargar el inventario.') + '</div>');
                    return;
                }

                inventario = $.isArray(res) ? res : (res.data || []);
                inventarioFiltrado = inventario.slice();
                renderInventario();
            })
            .fail(function () {
                $('#inventarioListado').html('<div class="table-empty">Error al cargar inventario.</div>');
            });
    }

    function filtrarInventario() {
        var q = ($('#txtBuscarInventario').val() || '').trim().toLowerCase();

        if (!q) {
            inventarioFiltrado = inventario.slice();
            renderInventario();
            return;
        }

        inventarioFiltrado = inventario.filter(function (x) {
            var texto = [
                getAny(x, ['ProductoNombre', 'NombreProducto', 'Producto', 'Nombre']),
                getAny(x, ['Referencia', 'Codigo', 'Sku', 'SKU']),
                getAny(x, ['ProductoId']),
                getAny(x, ['InvProdId']),
                getAny(x, ['Color']),
                getAny(x, ['Tipo', 'CategoriaNombre', 'Categoria']),
                getAny(x, ['Descripcion', 'Descripción'])
            ].join(' ').toLowerCase();

            return texto.indexOf(q) >= 0;
        });

        renderInventario();
    }

    function renderInventario() {
        if (!inventarioFiltrado.length) {
            actualizarResumen([]);
            $('#inventarioListado').html('<div class="table-empty">No hay registros de inventario.</div>');
            return;
        }

        var html = '';

        inventarioFiltrado.forEach(function (x) {
            var inventarioId = entero(getAny(x, ['InvProdId'], 0));
            var productoId = entero(getAny(x, ['ProductoId'], 0));
            var nombre = valor(getAny(x, ['ProductoNombre', 'NombreProducto', 'Producto', 'Nombre'], 'Producto sin nombre'));
            var referencia = valor(getAny(x, ['Referencia', 'Codigo', 'Sku', 'SKU'], '--'));
            var tipo = valor(getAny(x, ['Tipo', 'CategoriaNombre', 'Categoria'], 'GENERAL'));
            var color = valor(getAny(x, ['Color'], 'Sin color'));
            var descripcion = valor(getAny(x, ['Descripcion', 'Descripción'], 'Sin descripción disponible.'));
            var stock = entero(getAny(x, ['Stock'], 0));
            var reservado = entero(getAny(x, ['Reservado'], 0));
            var stockMinimo = entero(getAny(x, ['StockMinimo'], 0));

            var disponible = Math.max(stock - reservado, 0);
            var estado = obtenerEstadoInventario(disponible, stockMinimo);

            html += '<div class="inventario-card">';
            html += '   <div class="inventario-card__top">';
            html += '       <div class="inventario-card__identity">';
            html += '           <div class="inventario-card__icon"><i class="bi bi-box-seam-fill"></i></div>';
            html += '           <div class="inventario-card__main">';
            html += '               <div class="inventario-card__title">' + escapeHtml(nombre) + '</div>';
            html += '               <div class="inventario-card__chips">';
            html += '                   <span class="inventario-chip">ID producto #' + productoId + '</span>';
            html += '                   <span class="inventario-chip">Inventario #' + inventarioId + '</span>';
            html += '               </div>';
            html += '               <div class="inventario-card__ref">Ref: ' + escapeHtml(referencia) + '</div>';
            html += '               <div class="inventario-card__chips">';
            html += '                   <span class="inventario-chip inventario-chip--soft">' + escapeHtml(tipo.toUpperCase()) + '</span>';
            html += '                   <span class="inventario-chip inventario-chip--soft">' + escapeHtml(color) + '</span>';
            html += '                   <span class="inventario-chip inventario-chip--soft">Disponible ' + disponible + '</span>';
            html += '               </div>';
            html += '           </div>';
            html += '       </div>';
            html += '       <div class="inventario-card__status ' + estado.clase + '">' + estado.texto + '</div>';
            html += '   </div>';

            html += '   <div class="inventario-card__desc">' + escapeHtml(descripcion) + '</div>';

            html += '   <div class="inventario-card__stats">';
            html += '       <div class="inventario-stat inventario-stat--neutral">';
            html += '           <div class="inventario-stat__icon"><i class="bi bi-box-seam-fill"></i></div>';
            html += '           <div class="inventario-stat__label">Stock actual</div>';
            html += '           <div class="inventario-stat__value">' + stock + '</div>';
            html += '       </div>';

            html += '       <div class="inventario-stat inventario-stat--warning">';
            html += '           <div class="inventario-stat__icon"><i class="bi bi-lock-fill"></i></div>';
            html += '           <div class="inventario-stat__label">Reservado</div>';
            html += '           <div class="inventario-stat__value">' + reservado + '</div>';
            html += '       </div>';

            html += '       <div class="inventario-stat inventario-stat--danger">';
            html += '           <div class="inventario-stat__icon"><i class="bi bi-exclamation-triangle"></i></div>';
            html += '           <div class="inventario-stat__label">Bajo mínimo</div>';
            html += '           <div class="inventario-stat__value">' + stockMinimo + '</div>';
            html += '       </div>';
            html += '   </div>';

            html += '   <div class="inventario-card__actions">';
            html += '       <button type="button" class="inventario-btn inventario-btn--edit" onclick="AdminInventario.editar(' + inventarioId + ')">';
            html += '           <i class="bi bi-pencil"></i><span>Editar</span>';
            html += '       </button>';
            html += '       <button type="button" class="inventario-btn inventario-btn--delete" onclick="AdminInventario.eliminar(' + inventarioId + ')">';
            html += '           <i class="bi bi-trash"></i><span>Eliminar</span>';
            html += '       </button>';
            html += '   </div>';

            html += '</div>';
        });

        actualizarResumen(inventarioFiltrado);
        $('#inventarioListado').html(html);
    }

    function actualizarResumen(lista) {
        var productos = lista.length;
        var stock = 0;
        var reservado = 0;
        var bajoMinimo = 0;

        lista.forEach(function (x) {
            var s = entero(getAny(x, ['Stock'], 0));
            var r = entero(getAny(x, ['Reservado'], 0));
            var m = entero(getAny(x, ['StockMinimo'], 0));
            var d = Math.max(s - r, 0);

            stock += s;
            reservado += r;

            if (d <= m) {
                bajoMinimo++;
            }
        });

        $('#inventarioVisibleCount').text(productos + ' registros visibles');
        $('#kpiInventarioProductos').text(productos);
        $('#kpiInventarioStock').text(stock);
        $('#kpiInventarioReservado').text(reservado);
        $('#kpiInventarioBajoMinimo').text(bajoMinimo);
    }

    function nuevoInventario() {
        limpiarFormulario();
        $('#modalInventarioTitulo').text('Nuevo inventario');
        abrirModal();
    }

    function editarInventario(id) {
        var itemListado = inventario.find(function (x) {
            return entero(getAny(x, ['InvProdId'], 0)) === entero(id);
        });

        $.getJSON('/Inventario_Producto/Obtener', { id: id })
            .done(function (res) {
                if (!res || res.success === false) {
                    alert((res && res.message) || 'No se encontró el inventario.');
                    return;
                }

                var x = res.data || res;
                var base = itemListado || x;

                var invProdId = entero(getAny(x, ['InvProdId'], 0));
                var productoId = entero(getAny(x, ['ProductoId'], 0));
                var nombre = valor(getAny(base, ['ProductoNombre', 'NombreProducto', 'Producto', 'Nombre'], 'Producto #' + productoId));
                var referencia = valor(getAny(base, ['Referencia', 'Codigo', 'Sku', 'SKU'], '--'));
                var tipo = valor(getAny(base, ['Tipo', 'CategoriaNombre', 'Categoria'], 'GENERAL'));
                var color = valor(getAny(base, ['Color'], 'Sin color'));
                var descripcion = valor(getAny(base, ['Descripcion', 'Descripción'], 'Sin descripción disponible.'));
                var stock = entero(getAny(x, ['Stock'], 0));
                var reservado = entero(getAny(x, ['Reservado'], 0));
                var stockMinimo = entero(getAny(x, ['StockMinimo'], 0));

                $('#modalInventarioTitulo').text('Editar inventario');
                $('#hidInventarioId').val(invProdId);
                $('#txtProductoIdInventario').val(productoId);

                $('#invEditNombre').text(nombre);
                $('#invEditReferencia').text('Ref: ' + referencia);
                $('#invEditTipo').text((tipo || 'GENERAL').toUpperCase());
                $('#invEditColor').text(color || 'Sin color');
                $('#invEditDescripcion').text(descripcion || 'Sin descripción disponible.');

                $('#txtStockInventario').val(stock);
                $('#txtReservadoInventario').val(reservado);
                $('#txtStockMinimoInventario').val(stockMinimo);

                actualizarDisponibleEdit();
                abrirModal();
            })
            .fail(function () {
                alert('Error al obtener el inventario.');
            });
    }

    function guardarInventario() {
        $('#txtStockInventario, #txtReservadoInventario, #txtStockMinimoInventario').removeClass('input-error');

        var data = {
            InvProdId: entero($('#hidInventarioId').val()),
            ProductoId: entero($('#txtProductoIdInventario').val()),
            Stock: entero($('#txtStockInventario').val()),
            Reservado: entero($('#txtReservadoInventario').val()),
            StockMinimo: entero($('#txtStockMinimoInventario').val())
        };

        if (data.ProductoId <= 0) {
            alert('Debes ingresar un Producto ID válido.');
            return;
        }

        if (data.Stock < 0) {
            $('#txtStockInventario').addClass('input-error');
            return;
        }

        if (data.Reservado < 0) {
            $('#txtReservadoInventario').addClass('input-error');
            return;
        }

        if (data.StockMinimo < 0) {
            $('#txtStockMinimoInventario').addClass('input-error');
            return;
        }

        var url = data.InvProdId > 0
            ? '/Inventario_Producto/Actualizar'
            : '/Inventario_Producto/Insertar';

        var $btnGuardar = $('#btnGuardarInventario');
        $btnGuardar.prop('disabled', true).text('GUARDANDO...');

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        })
            .done(function (res) {
                $btnGuardar.prop('disabled', false).text('GUARDAR');

                if (!res || res.success === false) {
                    alert((res && res.message) || 'No se pudo guardar el inventario.');
                    return;
                }

                cerrarModal();
                cargarInventario();
            })
            .fail(function () {
                $btnGuardar.prop('disabled', false).text('GUARDAR');
                alert('Error al guardar el inventario.');
            });
    }

    function eliminarInventario(id) {
        if (!confirm('¿Deseas eliminar este registro de inventario?')) {
            return;
        }

        $.ajax({
            url: '/Inventario_Producto/Eliminar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: id })
        })
            .done(function (res) {
                if (!res || res.success === false) {
                    alert((res && res.message) || 'No se pudo eliminar.');
                    return;
                }

                cargarInventario();
            })
            .fail(function () {
                alert('Error al eliminar el inventario.');
            });
    }

    function abrirModal() {
        $('#modalInventario').show();
    }

    function cerrarModal() {
        $('#modalInventario').hide();
    }

    function limpiarFormulario() {
        $('#hidInventarioId').val(0);
        $('#txtProductoIdInventario').val(0);

        $('#invEditNombre').text('Producto');
        $('#invEditReferencia').text('Ref: --');
        $('#invEditTipo').text('GENERAL');
        $('#invEditColor').text('Sin color');
        $('#invEditDescripcion').text('Sin descripción disponible.');

        $('#txtStockInventario').val('');
        $('#txtReservadoInventario').val('');
        $('#txtStockMinimoInventario').val('');

        $('#invEditDisponible')
            .removeClass('inventario-chip--danger inventario-chip--ok inventario-chip--soft')
            .addClass('inventario-chip--soft')
            .text('Disponible 0');

        $('#txtStockInventario, #txtReservadoInventario, #txtStockMinimoInventario').removeClass('input-error');
    }

    function actualizarDisponibleEdit() {
        var stock = parseInt($('#txtStockInventario').val(), 10);
        var reservado = parseInt($('#txtReservadoInventario').val(), 10);
        var minimo = parseInt($('#txtStockMinimoInventario').val(), 10);

        stock = isNaN(stock) ? 0 : stock;
        reservado = isNaN(reservado) ? 0 : reservado;
        minimo = isNaN(minimo) ? 0 : minimo;

        var disponible = Math.max(stock - reservado, 0);

        $('#invEditDisponible').text('Disponible ' + disponible);

        if (disponible <= minimo) {
            $('#invEditDisponible')
                .removeClass('inventario-chip--soft inventario-chip--ok inventario-chip--danger')
                .addClass('inventario-chip--danger');
        } else {
            $('#invEditDisponible')
                .removeClass('inventario-chip--soft inventario-chip--ok inventario-chip--danger')
                .addClass('inventario-chip--ok');
        }
    }

    function obtenerEstadoInventario(disponible, minimo) {
        if (disponible <= minimo) {
            return { texto: 'En alerta', clase: 'inventario-card__status--alert' };
        }

        return { texto: 'Disponible', clase: 'inventario-card__status--ok' };
    }

    function getAny(obj, keys, fallback) {
        for (var i = 0; i < keys.length; i++) {
            var k = keys[i];
            if (obj && obj[k] !== undefined && obj[k] !== null && String(obj[k]).trim() !== '') {
                return obj[k];
            }
        }
        return fallback !== undefined ? fallback : '';
    }

    function entero(v) {
        var n = parseInt(v, 10);
        return isNaN(n) ? 0 : n;
    }

    function valor(v) {
        return v == null ? '' : String(v);
    }

    function escapeHtml(texto) {
        return valor(texto)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    window.AdminInventario = {
        editar: editarInventario,
        eliminar: eliminarInventario
    };
})();