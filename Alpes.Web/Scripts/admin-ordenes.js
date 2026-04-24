(function () {
    var ordenes = [];
    var productos = [];
    var clientes = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarCatalogos().then(function () {
            cargarOrdenes();
        });
    });

    function enlazarEventos() {
        $('#btnRecargarOrden').on('click', function () {
            cargarOrdenes();
        });

        $('#txtBuscarOrden').on('input', function () {
            renderTabla();
        });

        $('#btnNuevaOrden').on('click', function () {
            alert('El formulario de nueva orden será el siguiente paso.');
        });

        $('#btnCerrarDetalleOrden, #btnCerrarDetalleOrdenFooter, #modalDetalleOrden .modal-a__backdrop').on('click', function () {
            cerrarDetalle();
        });

        $('#btnActualizarEstadoOrden').on('click', function () {
            actualizarEstadoOrden();
        });
    }

    function cargarCatalogos() {
        var d1 = $.getJSON('/Producto/Index')
            .done(function (res) {
                productos = normalizarLista(res);
            });

        var d2 = $.getJSON('/Cliente/ListarJson')
            .done(function (res) {
                clientes = normalizarLista(res);
            });

        return $.when(d1, d2).fail(function () {
            console.warn('No se pudieron cargar productos o clientes.');
        });
    }

    function cargarOrdenes() {
        $('#ordenBody').html('<tr><td colspan="7" class="table-empty">Cargando órdenes...</td></tr>');

        $.getJSON('/Orden_Venta/Index')
            .done(function (res) {
                ordenes = normalizarLista(res);
                renderTabla();
            })
            .fail(function () {
                $('#ordenBody').html('<tr><td colspan="7" class="table-empty">Error al cargar órdenes.</td></tr>');
            });
    }

    function normalizarLista(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderTabla() {
        var filtro = ($('#txtBuscarOrden').val() || '').toLowerCase().trim();

        var lista = ordenes.filter(function (o) {
            var num = valor(o.NumOrden).toLowerCase();
            var cliente = nombreCliente(o.CliId).toLowerCase();
            return !filtro || num.indexOf(filtro) >= 0 || cliente.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#ordenBody').html('<tr><td colspan="7" class="table-empty">No hay órdenes para mostrar.</td></tr>');
            return;
        }

        var html = '';

        lista.forEach(function (o) {
            html += '<tr>';
            html += '<td>' + valor(o.OrdenVentaId) + '</td>';
            html += '<td>' + valor(o.NumOrden) + '</td>';
            html += '<td>' + valor(nombreCliente(o.CliId)) + '</td>';
            html += '<td>' + formatearFecha(o.FechaOrden) + '</td>';
            html += '<td>Q ' + formatearMonto(o.Total) + '</td>';
            html += '<td>' + badgeEstadoPorId(o.EstadoOrdenId) + '</td>';
            html += '<td>';
            html += '<div class="table-actions">';
            html += '<button class="btn-icon" onclick="AdminOrdenes.verDetalle(' + o.OrdenVentaId + ')"><i class="bi bi-eye"></i></button>';
            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        $('#ordenBody').html(html);
    }

    function cargarDetalleOrden(id) {
        $('#detalleOrdenBody').html('<tr><td colspan="4" class="table-empty">Cargando detalle...</td></tr>');
        $('#modalDetalleOrden').show();
        ocultarErrorDetalle();

        $.getJSON('/Orden_Venta/Obtener', { id: id })
            .done(function (orden) {
                if (!orden || !orden.OrdenVentaId) {
                    mostrarErrorDetalle('No se encontró la orden seleccionada.');
                    return;
                }

                llenarCabeceraDetalle(orden);

                $.getJSON('/Orden_Venta_Detalle/Buscar', { valor: id })
                    .done(function (res) {
                        var detalles = normalizarLista(res);
                        renderDetalleProductos(detalles);
                    })
                    .fail(function () {
                        mostrarErrorDetalle('No fue posible cargar el detalle de la orden.');
                        $('#detalleOrdenBody').html('<tr><td colspan="4" class="table-empty">No fue posible cargar el detalle.</td></tr>');
                    });
            })
            .fail(function () {
                mostrarErrorDetalle('No fue posible obtener la orden.');
            });
    }

    function llenarCabeceraDetalle(orden) {
        $('#detalleOrdenTitulo').text('Orden #' + valor(orden.OrdenVentaId));
        $('#detalleNumOrden').text(valor(orden.NumOrden));
        $('#detalleFechaOrden').text(formatearFecha(orden.FechaOrden));
        $('#detalleTotalOrden').text('Q ' + formatearMonto(orden.Total));
        $('#detalleClienteOrden').val(nombreCliente(orden.CliId));
        $('#detalleEstadoOrden').html(badgeEstadoPorId(orden.EstadoOrdenId));
        $('#detalleNuevoEstado').val(textoEstadoPorId(orden.EstadoOrdenId));

        $('#detalleOrdenId').val(orden.OrdenVentaId || 0);
        $('#detalleOrdenCliId').val(orden.CliId || 0);
        $('#detalleOrdenNumOrden').val(orden.NumOrden || '');
        $('#detalleOrdenFecha').val(orden.FechaOrden || '');
        $('#detalleOrdenSubtotal').val(orden.Subtotal || 0);
        $('#detalleOrdenDescuento').val(orden.Descuento || 0);
        $('#detalleOrdenImpuesto').val(orden.Impuesto || 0);
        $('#detalleOrdenTotal').val(orden.Total || 0);
        $('#detalleOrdenMoneda').val(orden.Moneda || '');
        $('#detalleOrdenDireccionEnvioSnapshot').val(orden.DireccionEnvioSnapshot || '');
        $('#detalleOrdenObservaciones').val(orden.Observaciones || '');
        $('#detalleOrdenEstadoRegistro').val(orden.Estado || '');
    }

    function cerrarDetalle() {
        $('#modalDetalleOrden').hide();
        $('#detalleOrdenBody').html('<tr><td colspan="4" class="table-empty">Cargando detalle...</td></tr>');
        ocultarErrorDetalle();
    }

    function actualizarEstadoOrden() {
        var ordenId = entero($('#detalleOrdenId').val());
        var estadoTexto = ($('#detalleNuevoEstado').val() || '').toUpperCase();

        var estadoMap = {
            "PENDIENTE": 1,
            "COMPLETADA": 2
        };

        var estadoOrdenId = estadoMap[estadoTexto] || 1;

        if (!ordenId) {
            mostrarErrorDetalle('No se encontró el ID de la orden.');
            return;
        }

        var payload = {
            OrdenVentaId: ordenId,
            NumOrden: $('#detalleOrdenNumOrden').val(),
            CliId: entero($('#detalleOrdenCliId').val()),
            EstadoOrdenId: estadoOrdenId,
            FechaOrden: $('#detalleOrdenFecha').val(),
            Subtotal: decimal($('#detalleOrdenSubtotal').val()),
            Descuento: decimal($('#detalleOrdenDescuento').val()),
            Impuesto: decimal($('#detalleOrdenImpuesto').val()),
            Total: decimal($('#detalleOrdenTotal').val()),
            Moneda: $('#detalleOrdenMoneda').val(),
            DireccionEnvioSnapshot: $('#detalleOrdenDireccionEnvioSnapshot').val(),
            Observaciones: $('#detalleOrdenObservaciones').val(),
            Estado: $('#detalleOrdenEstadoRegistro').val()
        };

        $.ajax({
            url: '/Orden_Venta/Actualizar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(payload)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    mostrarErrorDetalle(res.message || 'No fue posible actualizar el estado.');
                    return;
                }

                var orden = ordenes.find(function (x) { return x.OrdenVentaId == ordenId; });
                if (orden) {
                    orden.EstadoOrdenId = estadoOrdenId;
                }

                $('#detalleEstadoOrden').html(badgeEstadoPorId(estadoOrdenId));
                renderTabla();
                ocultarErrorDetalle();
                alert('Estado actualizado correctamente.');
            })
            .fail(function (xhr) {
                mostrarErrorDetalle(obtenerMensaje(xhr, 'Error al actualizar el estado.'));
            });
    }

    function renderDetalleProductos(detalles) {
        if (!detalles || !detalles.length) {
            $('#detalleOrdenBody').html('<tr><td colspan="4" class="table-empty">La orden no tiene productos.</td></tr>');
            return;
        }

        var html = '';

        detalles.forEach(function (d) {
            var nombreProd = nombreProducto(d.ProductoId);

            html += '<tr>';
            html += '<td>' + valor(nombreProd || ('Producto #' + valor(d.ProductoId))) + '</td>';
            html += '<td>' + valor(d.Cantidad) + '</td>';
            html += '<td>Q ' + formatearMonto(d.PrecioUnitarioSnapshot) + '</td>';
            html += '<td>Q ' + formatearMonto(d.SubtotalLinea) + '</td>';
            html += '</tr>';
        });

        $('#detalleOrdenBody').html(html);
    }

    function nombreCliente(id) {
        var item = clientes.find(function (c) { return c.CliId == id; });
        if (!item) return 'Cliente #' + valor(id);
        return item.NombreCompleto || ((item.Nombres || '') + ' ' + (item.Apellidos || '')).trim();
    }

    function textoEstadoPorId(id) {
        if (entero(id) === 2) return 'COMPLETADA';
        return 'PENDIENTE';
    }

    function badgeEstadoPorId(id) {
        id = entero(id);

        if (id === 1) {
            return '<span class="badge-stock badge-stock--warn">Pendiente</span>';
        }

        if (id === 2) {
            return '<span class="badge-stock badge-stock--ok">Completada</span>';
        }

        return '<span class="badge-stock badge-stock--warn">Sin estado</span>';
    }

    function formatearMonto(valorMonto) {
        var n = parseFloat(valorMonto);
        if (isNaN(n)) return '0.00';
        return n.toFixed(2);
    }

    function formatearFecha(valorFecha) {
        if (!valorFecha) return '';

        var matchAspNet = /\/Date\((\d+)\)\//.exec(valorFecha);
        if (matchAspNet) {
            var fechaAsp = new Date(parseInt(matchAspNet[1], 10));
            return fechaValida(fechaAsp) ? fechaAsp.toLocaleDateString() : '';
        }

        var fecha = new Date(valorFecha);
        return fechaValida(fecha) ? fecha.toLocaleDateString() : valor(valorFecha);
    }

    function fechaValida(fecha) {
        return fecha instanceof Date && !isNaN(fecha.getTime());
    }

    function nombreProducto(id) {
        var item = productos.find(function (p) { return p.ProductoId == id; });
        if (!item) return '';
        return item.Nombre + (item.Referencia ? ' (' + item.Referencia + ')' : '');
    }

    function mostrarErrorDetalle(msg) {
        $('#detalleOrdenErrorTexto').text(msg);
        $('#detalleOrdenError').show();
    }

    function ocultarErrorDetalle() {
        $('#detalleOrdenErrorTexto').text('');
        $('#detalleOrdenError').hide();
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
        return v == null ? '' : String(v);
    }

    function entero(v) {
        var n = parseInt(v, 10);
        return isNaN(n) ? 0 : n;
    }

    function decimal(v) {
        var n = parseFloat(v);
        return isNaN(n) ? 0 : n;
    }

    window.AdminOrdenes = {
        verDetalle: cargarDetalleOrden
    };
})();