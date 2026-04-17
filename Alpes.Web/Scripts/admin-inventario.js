(function () {
    var inventarios = [];
    var productos = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarCatalogos().then(function () {
            cargarInventario();
        });
    });

    function enlazarEventos() {
        $('#btnNuevoInventario').on('click', function () {
            abrirNuevo();
        });

        $('#btnRecargarInventario').on('click', function () {
            cargarInventario();
        });

        $('#btnCerrarModalInventario, #btnCancelarInventario, #modalInventario .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });

        $('#btnGuardarInventario').on('click', function () {
            guardarInventario();
        });

        $('#txtBuscarInventario').on('input', function () {
            renderTabla();
        });

        $('#filtroEstadoInventario').on('change', function () {
            renderTabla();
        });
    }

    function cargarCatalogos() {
        return $.getJSON('/Producto/Index')
            .done(function (res) {
                productos = normalizarLista(res);
                llenarProductos();
            })
            .fail(function () {
                console.warn('No se pudieron cargar productos.');
            });
    }

    function cargarInventario() {
        $('#inventarioBody').html('<tr><td colspan="7" class="table-empty">Cargando inventario...</td></tr>');

        $.getJSON('/Inventario_Producto/Index')
            .done(function (res) {
                inventarios = normalizarLista(res);
                actualizarKPIs();
                renderTabla();
            })
            .fail(function () {
                $('#inventarioBody').html('<tr><td colspan="7" class="table-empty">Error al cargar inventario.</td></tr>');
            });
    }

    function normalizarLista(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderTabla() {
        var filtro = ($('#txtBuscarInventario').val() || '').toLowerCase().trim();
        var estado = $('#filtroEstadoInventario').val() || '';

        var lista = inventarios.filter(function (x) {
            var nombre = (x.NombreProducto || nombreProducto(x.ProductoId) || '').toLowerCase();
            var coincideTexto = !filtro || nombre.indexOf(filtro) >= 0;
            var coincideEstado = !estado || tipoEstadoInventario(x) === estado;
            return coincideTexto && coincideEstado;
        });

        if (!lista.length) {
            $('#inventarioBody').html('<tr><td colspan="7" class="table-empty">No hay registros para mostrar.</td></tr>');
            return;
        }

        var html = '';

        lista.forEach(function (x) {
            html += '<tr class="' + claseFilaInventario(x) + '">';
            html += '<td>' + valor(x.InvProdId) + '</td>';
            html += '<td>' + valor(x.NombreProducto || nombreProducto(x.ProductoId)) + '</td>';
            html += '<td>' + stockDecorado(x) + '</td>';
            html += '<td>' + valor(x.StockReservado) + '</td>';
            html += '<td>' + valor(x.StockMinimo) + '</td>';
            html += '<td>' + badgeInventario(x) + '</td>';
            html += '<td>';
            html += '<div class="table-actions">';
            html += '<button class="btn-icon" onclick="AdminInventario.editar(' + x.InvProdId + ')"><i class="bi bi-pencil"></i></button>';
            html += '<button class="btn-icon btn-icon-danger" onclick="AdminInventario.eliminar(' + x.InvProdId + ', \'' + escapar(valor(x.NombreProducto || nombreProducto(x.ProductoId))) + '\')"><i class="bi bi-trash"></i></button>';
            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        $('#inventarioBody').html(html);
    }

    function actualizarKPIs() {
        var total = inventarios.length;
        var stockBajo = inventarios.filter(function (x) { return tipoEstadoInventario(x) === 'stock-bajo'; }).length;
        var sobreReservado = inventarios.filter(function (x) { return tipoEstadoInventario(x) === 'sobre-reservado'; }).length;

        $('#kpiInventarioTotal').text(total);
        $('#kpiStockBajo').text(stockBajo);
        $('#kpiSobreReservado').text(sobreReservado);
    }

    function abrirNuevo() {
        limpiarFormulario();
        $('#modalInventarioTitulo').text('Nuevo inventario');
        $('#modalInventario').show();
    }

    function abrirEditar(item) {
        limpiarFormulario();

        $('#modalInventarioTitulo').text('Editar inventario');
        $('#InvProdId').val(item.InvProdId || 0);
        $('#ProductoId').val(item.ProductoId || '');
        $('#Stock').val(item.Stock || 0);
        $('#StockReservado').val(item.StockReservado || 0);
        $('#StockMinimo').val(item.StockMinimo == null ? '' : item.StockMinimo);

        $('#modalInventario').show();
    }

    function cerrarModal() {
        $('#modalInventario').hide();
        ocultarError();
    }

    function limpiarFormulario() {
        $('#InvProdId').val(0);
        $('#ProductoId').val('');
        $('#Stock').val(0);
        $('#StockReservado').val(0);
        $('#StockMinimo').val('');
        ocultarError();
    }

    function guardarInventario() {
        var item = {
            InvProdId: entero($('#InvProdId').val()),
            ProductoId: entero($('#ProductoId').val()),
            Stock: entero($('#Stock').val()),
            StockReservado: entero($('#StockReservado').val()),
            StockMinimo: enteroNullable($('#StockMinimo').val())
        };

        if (!item.ProductoId) {
            mostrarError('Debes seleccionar un producto.');
            return;
        }

        if (item.Stock < 0 || item.StockReservado < 0) {
            mostrarError('Stock y stock reservado no pueden ser negativos.');
            return;
        }

        if (item.StockMinimo !== null && item.StockMinimo < 0) {
            mostrarError('El stock mínimo no puede ser negativo.');
            return;
        }

        if (item.StockReservado > item.Stock) {
            mostrarError('El stock reservado no puede ser mayor que el stock.');
            return;
        }

        var url = item.InvProdId > 0 ? '/Inventario_Producto/Actualizar' : '/Inventario_Producto/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(item)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    mostrarError(res.message || 'No fue posible guardar el inventario.');
                    return;
                }

                cerrarModal();
                cargarInventario();
            })
            .fail(function (xhr) {
                mostrarError(obtenerMensaje(xhr, 'Error al guardar el inventario.'));
            });
    }

    function editarInventario(id) {
        $.getJSON('/Inventario_Producto/Obtener', { id: id })
            .done(function (res) {
                if (!res) {
                    mostrarError('No se encontró el registro.');
                    return;
                }
                abrirEditar(res);
            })
            .fail(function (xhr) {
                mostrarError(obtenerMensaje(xhr, 'Error al obtener el inventario.'));
            });
    }

    function eliminarInventario(id, nombre) {
        if (!confirm('¿Deseas eliminar el inventario de "' + nombre + '"?')) {
            return;
        }

        $.ajax({
            url: '/Inventario_Producto/Eliminar',
            type: 'POST',
            data: { InvProdId: id }
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarInventario();
            })
            .fail(function (xhr) {
                alert(obtenerMensaje(xhr, 'Error al eliminar el inventario.'));
            });
    }

    function llenarProductos() {
        var html = '<option value="">Seleccione...</option>';

        productos.forEach(function (p) {
            var label = valor(p.Nombre);
            if (p.Referencia) {
                label += ' (' + p.Referencia + ')';
            }
            html += '<option value="' + p.ProductoId + '">' + label + '</option>';
        });

        $('#ProductoId').html(html);
    }

    function nombreProducto(id) {
        var item = productos.find(function (p) { return p.ProductoId == id; });
        if (!item) return '';
        return item.Nombre + (item.Referencia ? ' (' + item.Referencia + ')' : '');
    }

    function tipoEstadoInventario(x) {
        var stock = entero(x.Stock);
        var reservado = entero(x.StockReservado);
        var minimo = enteroNullable(x.StockMinimo);

        if (reservado > stock) {
            return 'sobre-reservado';
        }

        if (minimo !== null && stock <= minimo) {
            return 'stock-bajo';
        }

        return 'normal';
    }

    function badgeInventario(x) {
        var estado = tipoEstadoInventario(x);

        if (estado === 'sobre-reservado') {
            return '<span class="badge-stock badge-stock--danger">Sobre reservado</span>';
        }

        if (estado === 'stock-bajo') {
            return '<span class="badge-stock badge-stock--warn">Stock bajo</span>';
        }

        return '<span class="badge-stock badge-stock--ok">Normal</span>';
    }

    function claseFilaInventario(x) {
        var estado = tipoEstadoInventario(x);

        if (estado === 'sobre-reservado') {
            return 'row-stock-danger';
        }

        if (estado === 'stock-bajo') {
            return 'row-stock-warn';
        }

        return '';
    }

    function stockDecorado(x) {
        var stock = entero(x.Stock);
        var estado = tipoEstadoInventario(x);

        if (estado === 'sobre-reservado') {
            return '<strong style="color:#b3261e;">' + stock + '</strong>';
        }

        if (estado === 'stock-bajo') {
            return '<strong style="color:#9a5b00;">' + stock + '</strong>';
        }

        return stock;
    }

    function mostrarError(msg) {
        $('#inventarioErrorTexto').text(msg);
        $('#inventarioError').show();
    }

    function ocultarError() {
        $('#inventarioErrorTexto').text('');
        $('#inventarioError').hide();
    }

    function obtenerMensaje(xhr, defaultMsg) {
        try {
            var r = JSON.parse(xhr.responseText);
            return r.message || defaultMsg;
        } catch (e) {
            return defaultMsg;
        }
    }

    function valor(v) {
        return v == null ? '' : v;
    }

    function entero(v) {
        var n = parseInt(v, 10);
        return isNaN(n) ? 0 : n;
    }

    function enteroNullable(v) {
        if (v === null || v === undefined || v === '') return null;
        var n = parseInt(v, 10);
        return isNaN(n) ? null : n;
    }

    function escapar(txt) {
        return String(txt || '').replace(/'/g, "\\'");
    }

    window.AdminInventario = {
        editar: editarInventario,
        eliminar: eliminarInventario
    };
})();