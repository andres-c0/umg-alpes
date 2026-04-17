(function () {
    var productos = [];
    var categorias = [];
    var unidades = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarCatalogos().then(function () {
            cargarProductos();
        });
    });

    function enlazarEventos() {
        $('#btnNuevoProducto').on('click', function () {
            abrirNuevo();
        });

        $('#btnRecargarProductos').on('click', function () {
            cargarProductos();
        });

        $('#btnCerrarModalProducto, #btnCancelarProducto, .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });

        $('#btnGuardarProducto').on('click', function () {
            guardarProducto();
        });

        $('#txtBuscarProducto').on('input', function () {
            renderTabla($(this).val());
        });
    }

    function cargarCatalogos() {
        var d1 = $.getJSON('/Categoria/Index');
        var d2 = $.getJSON('/Unidad_Medida/Index');

        return $.when(d1, d2).done(function (r1, r2) {
            categorias = normalizarLista(r1[0]);
            unidades = normalizarLista(r2[0]);

            llenarCategorias();
            llenarUnidades();
        }).fail(function () {
            console.warn('No se pudieron cargar categorías o unidades.');
        });
    }

    function cargarProductos() {
        $('#productosBody').html('<tr><td colspan="10" class="table-empty">Cargando productos...</td></tr>');

        $.getJSON('/Producto/Index')
            .done(function (res) {
                productos = normalizarLista(res);
                renderTabla($('#txtBuscarProducto').val());
            })
            .fail(function (xhr) {
                $('#productosBody').html('<tr><td colspan="10" class="table-empty">Error al cargar productos.</td></tr>');
                console.error(xhr);
            });
    }

    function normalizarLista(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderTabla(filtro) {
        filtro = (filtro || '').toLowerCase().trim();

        var lista = productos.filter(function (p) {
            var nombre = (p.Nombre || '').toLowerCase();
            var referencia = (p.Referencia || '').toLowerCase();
            return !filtro || nombre.indexOf(filtro) >= 0 || referencia.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#productosBody').html('<tr><td colspan="10" class="table-empty">No hay productos para mostrar.</td></tr>');
            return;
        }

        var html = '';

        lista.forEach(function (p) {
            html += '<tr>';
            html += '<td>' + valor(p.ProductoId) + '</td>';
            html += '<td>' + valor(p.Referencia) + '</td>';
            html += '<td>' + valor(p.Nombre) + '</td>';
            html += '<td>' + valor(p.Tipo) + '</td>';
            html += '<td>' + valor(p.Material) + '</td>';
            html += '<td>' + valor(p.CategoriaNombre) + '</td>';
            html += '<td>' + valor(p.UnidadMedidaNombre) + '</td>';
            html += '<td>' + valor(p.Color) + '</td>';
            html += '<td>' + valor(p.Estado) + '</td>';
            html += '<td>';
            html += '<div class="table-actions">';
            html += '<button class="btn-icon" onclick="AdminProductos.editar(' + p.ProductoId + ')"><i class="bi bi-pencil"></i></button>';
            html += '<button class="btn-icon btn-icon-danger" onclick="AdminProductos.eliminar(' + p.ProductoId + ', \'' + escapar(valor(p.Nombre)) + '\')"><i class="bi bi-trash"></i></button>';
            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        $('#productosBody').html(html);
    }

    function abrirNuevo() {
        limpiarFormulario();
        $('#modalProductoTitulo').text('Nuevo producto');
        $('#modalProducto').show();
    }

    function abrirEditar(producto) {
        limpiarFormulario();

        $('#modalProductoTitulo').text('Editar producto');
        $('#ProductoId').val(producto.ProductoId || 0);
        $('#Referencia').val(producto.Referencia || '');
        $('#Nombre').val(producto.Nombre || '');
        $('#Descripcion').val(producto.Descripcion || '');
        $('#Tipo').val(producto.Tipo || '');
        $('#Material').val(producto.Material || '');
        $('#AltoCm').val(producto.AltoCm || '');
        $('#AnchoCm').val(producto.AnchoCm || '');
        $('#ProfundidadCm').val(producto.ProfundidadCm || '');
        $('#Color').val(producto.Color || '');
        $('#PesoGramos').val(producto.PesoGramos || '');
        $('#ImagenUrl').val(producto.ImagenUrl || '');
        $('#UnidadMedidaId').val(producto.UnidadMedidaId || '');
        $('#CategoriaId').val(producto.CategoriaId || '');
        $('#LoteProducto').val(producto.LoteProducto || '');
        $('#Estado').val(producto.Estado || '');

        $('#modalProducto').show();
    }

    function cerrarModal() {
        $('#modalProducto').hide();
        ocultarError();
    }

    function limpiarFormulario() {
        $('#ProductoId').val(0);
        $('#Referencia').val('');
        $('#Nombre').val('');
        $('#Descripcion').val('');
        $('#Tipo').val('');
        $('#Material').val('');
        $('#AltoCm').val('');
        $('#AnchoCm').val('');
        $('#ProfundidadCm').val('');
        $('#Color').val('');
        $('#PesoGramos').val('');
        $('#ImagenUrl').val('');
        $('#UnidadMedidaId').val('');
        $('#CategoriaId').val('');
        $('#LoteProducto').val('');
        $('#Estado').val('ACTIVO');
        ocultarError();
    }

    function guardarProducto() {
        var producto = {
            ProductoId: entero($('#ProductoId').val()),
            Referencia: $('#Referencia').val(),
            Nombre: $('#Nombre').val(),
            Descripcion: $('#Descripcion').val(),
            Tipo: $('#Tipo').val(),
            Material: $('#Material').val(),
            AltoCm: decimalNullable($('#AltoCm').val()),
            AnchoCm: decimalNullable($('#AnchoCm').val()),
            ProfundidadCm: decimalNullable($('#ProfundidadCm').val(),
            ),
            Color: $('#Color').val(),
            PesoGramos: decimalNullable($('#PesoGramos').val()),
            ImagenUrl: $('#ImagenUrl').val(),
            UnidadMedidaId: enteroNullable($('#UnidadMedidaId').val()),
            CategoriaId: entero($('#CategoriaId').val()),
            LoteProducto: $('#LoteProducto').val(),
            Estado: $('#Estado').val()
        };

        if (!producto.Nombre || !producto.Tipo || !producto.CategoriaId) {
            mostrarError('Nombre, Tipo y Categoría son obligatorios.');
            return;
        }

        var url = producto.ProductoId > 0 ? '/Producto/Actualizar' : '/Producto/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(producto)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    mostrarError(res.message || 'No fue posible guardar el producto.');
                    return;
                }

                cerrarModal();
                cargarProductos();
            })
            .fail(function (xhr) {
                mostrarError(obtenerMensaje(xhr, 'Error al guardar el producto.'));
            });
    }

    function editarProducto(id) {
        $.getJSON('/Producto/Obtener', { id: id })
            .done(function (res) {
                if (!res) {
                    mostrarError('No se encontró el producto.');
                    return;
                }
                abrirEditar(res);
            })
            .fail(function (xhr) {
                mostrarError(obtenerMensaje(xhr, 'Error al obtener el producto.'));
            });
    }

    function eliminarProducto(id, nombre) {
        if (!confirm('¿Deseas eliminar el producto "' + nombre + '"?')) {
            return;
        }

        $.ajax({
            url: '/Producto/Eliminar',
            type: 'POST',
            data: { ProductoId: id }
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarProductos();
            })
            .fail(function (xhr) {
                alert(obtenerMensaje(xhr, 'Error al eliminar el producto.'));
            });
    }

    function llenarCategorias() {
        var html = '<option value="">Seleccione...</option>';
        categorias.forEach(function (c) {
            html += '<option value="' + c.CategoriaId + '">' + valor(c.Nombre) + '</option>';
        });
        $('#CategoriaId').html(html);
    }

    function llenarUnidades() {
        var html = '<option value="">Seleccione...</option>';
        unidades.forEach(function (u) {
            html += '<option value="' + u.UnidadMedidaId + '">' + valor(u.Nombre) + '</option>';
        });
        $('#UnidadMedidaId').html(html);
    }

    function nombreCategoria(id) {
        var item = categorias.find(function (c) { return c.CategoriaId == id; });
        return item ? valor(item.Nombre) : valor(id);
    }

    function nombreUnidad(id) {
        var item = unidades.find(function (u) { return u.UnidadMedidaId == id; });
        return item ? valor(item.Nombre) : '';
    }

    function mostrarError(msg) {
        $('#productoErrorTexto').text(msg);
        $('#productoError').show();
    }

    function ocultarError() {
        $('#productoErrorTexto').text('');
        $('#productoError').hide();
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
        var n = parseInt(v, 10);
        return isNaN(n) ? null : n;
    }

    function decimalNullable(v) {
        if (v === null || v === undefined || v === '') return null;
        var n = parseFloat(v);
        return isNaN(n) ? null : n;
    }

    function escapar(txt) {
        return String(txt || '').replace(/'/g, "\\'");
    }

    window.AdminProductos = {
        editar: editarProducto,
        eliminar: eliminarProducto
    };
})();