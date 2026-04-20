(function () {
    var compras = [];
    var proveedores = [];
    var estadosOc = [];
    var condicionesPago = [];
    var compraSeleccionada = null;

    $(document).ready(function () {
        enlazarEventos();
        cargarCatalogos().then(function () {
            cargarCompras();
        });
    });

    function enlazarEventos() {
        $('#btnRecargarCompra').on('click', function () {
            cargarCompras();
        });

        $('#txtBuscarCompra').on('input', function () {
            renderCompras();
        });

        $('#btnNuevaCompra').on('click', function () {
            compraSeleccionada = null;
            limpiarFormulario();
            $('#modalCompraTitulo').text('Nueva orden de compra');
            abrirModal();
        });

        $('#btnGuardarCompra').on('click', function () {
            guardarCompra();
        });

        $('#btnCerrarModalCompra, #btnCerrarModalCompraX, #modalCompra .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });
    }

    function cargarCatalogos() {
        var d1 = $.getJSON('/Proveedor/Index').done(function (res) {
            proveedores = normalizar(res);
            llenarProveedores();
        });

        var d2 = $.getJSON('/Estado_Orden_Compra/Index').done(function (res) {
            estadosOc = normalizar(res);
            llenarEstadosOc();
        });

        var d3 = $.getJSON('/CondicionPago/Index').done(function (res) {
            condicionesPago = normalizar(res);
            llenarCondicionesPago();
        });

        return $.when(d1, d2, d3).fail(function () {
            console.warn('No se pudieron cargar todos los catálogos.');
        });
    }

    function cargarCompras() {
        $('#comprasListado').html('<div class="table-empty">Cargando órdenes de compra...</div>');

        $.getJSON('/Orden_Compra/Index')
            .done(function (res) {
                compras = normalizar(res);
                renderCompras();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#comprasListado').html('<div class="table-empty">Error al cargar órdenes de compra.</div>');
            });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderCompras() {
        var filtro = ($('#txtBuscarCompra').val() || '').toLowerCase().trim();

        var lista = compras.filter(function (c) {
            var texto = (
                valor(c.NumOc) + ' ' +
                valor(c.RazonSocial) + ' ' +
                valor(c.EstadoOcCodigo) + ' ' +
                valor(c.CondicionPagoNombre)
            ).toLowerCase();

            return !filtro || texto.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#comprasListado').html('<div class="table-empty">No hay órdenes de compra.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (c) {
            html += '<div class="cliente-card">';
            html += '  <div class="cliente-card__avatar"><i class="bi bi-bag-fill"></i></div>';
            html += '  <div class="cliente-card__body">';
            html += '      <div class="cliente-card__name">' + valor(c.NumOc) + '</div>';
            html += '      <div class="cliente-card__meta">Proveedor: ' + valor(c.RazonSocial) + '</div>';
            html += '      <div class="cliente-card__meta">Fecha: ' + formatearFecha(c.FechaOc) + '</div>';
            html += '      <div class="cliente-card__name">Q ' + formatearMonto(c.Total) + '</div>';
            html += '  </div>';
            html += '  <div class="cliente-card__actions">';
            html += '      <span class="' + claseBadgeEstadoOc(c.EstadoOcCodigo) + '">' + valor(c.EstadoOcCodigo) + '</span>';
            html += '      <button type="button" class="btn-icon" onclick="AdminCompras.editar(' + c.OrdenCompraId + ')"><i class="bi bi-pencil"></i></button>';
            html += '      <button type="button" class="btn-icon btn-icon-danger" onclick="AdminCompras.eliminar(' + c.OrdenCompraId + ')"><i class="bi bi-trash"></i></button>';
            html += '  </div>';
            html += '</div>';
        });

        $('#comprasListado').html(html);
    }

    function editar(id) {
        $.getJSON('/Orden_Compra/Obtener', { id: id })
            .done(function (res) {
                if (!res || !res.OrdenCompraId) {
                    mostrarError('No se encontró la orden.');
                    return;
                }

                compraSeleccionada = res;
                $('#modalCompraTitulo').text('Editar orden');

                $('#txtNumOc').val(valor(res.NumOc));
                $('#selProveedorCompra').val(valor(res.ProvId));
                $('#selEstadoOc').val(valor(res.EstadoOcId));
                $('#selCondicionPagoCompra').val(valor(res.CondicionPagoId));
                $('#txtFechaOc').val(formatearFechaInput(res.FechaOc));
                $('#txtSubtotalCompra').val(valor(res.Subtotal));
                $('#txtImpuestoCompra').val(valor(res.Impuesto));
                $('#txtTotalCompra').val(valor(res.Total));
                $('#txtObservacionesCompra').val(valor(res.Observaciones));

                abrirModal();
            })
            .fail(function (xhr) {
                console.error(xhr);
                mostrarError(obtenerMensaje(xhr, 'Error al obtener la orden.'));
            });
    }

    function guardarCompra() {
        var data = {
            OrdenCompraId: compraSeleccionada ? compraSeleccionada.OrdenCompraId : 0,
            NumOc: $('#txtNumOc').val(),
            ProvId: entero($('#selProveedorCompra').val()),
            EstadoOcId: entero($('#selEstadoOc').val()),
            CondicionPagoId: entero($('#selCondicionPagoCompra').val()),
            FechaOc: $('#txtFechaOc').val(),
            Subtotal: decimal($('#txtSubtotalCompra').val()),
            Impuesto: decimal($('#txtImpuestoCompra').val()),
            Total: decimal($('#txtTotalCompra').val()),
            Observaciones: $('#txtObservacionesCompra').val(),
            Estado: compraSeleccionada ? valor(compraSeleccionada.Estado || 'ACTIVO') : 'ACTIVO'
        };

        if (!data.NumOc) {
            mostrarError('El número de OC es obligatorio.');
            return;
        }

        if (!data.ProvId) {
            mostrarError('Debes seleccionar un proveedor.');
            return;
        }

        if (!data.EstadoOcId) {
            mostrarError('Debes seleccionar el estado de la orden.');
            return;
        }

        if (!data.CondicionPagoId) {
            mostrarError('Debes seleccionar la condición de pago.');
            return;
        }

        var url = compraSeleccionada
            ? '/Orden_Compra/Actualizar'
            : '/Orden_Compra/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    mostrarError(res.message || 'No fue posible guardar la orden.');
                    return;
                }

                cerrarModal();
                cargarCompras();
            })
            .fail(function (xhr) {
                console.error(xhr);
                mostrarError(obtenerMensaje(xhr, 'Error al guardar la orden.'));
            });
    }

    function eliminarCompra(id) {
        if (!confirm('¿Deseas eliminar esta orden de compra?')) {
            return;
        }

        $.ajax({
            url: '/Orden_Compra/Eliminar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ OrdenCompraId: id })
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarCompras();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al eliminar la orden.'));
            });
    }

    function llenarProveedores() {
        var html = '<option value="">Seleccione proveedor</option>';

        proveedores.forEach(function (p) {
            html += '<option value="' + p.ProvId + '">' + valor(p.RazonSocial) + '</option>';
        });

        $('#selProveedorCompra').html(html);
    }

    function llenarEstadosOc() {
        var html = '<option value="">Seleccione estado</option>';

        estadosOc.forEach(function (e) {
            html += '<option value="' + e.EstadoOcId + '">' + valor(e.Codigo) + '</option>';
        });

        $('#selEstadoOc').html(html);
    }

    function llenarCondicionesPago() {
        var html = '<option value="">Seleccione condición</option>';

        condicionesPago.forEach(function (c) {
            html += '<option value="' + c.CondicionPagoId + '">' + valor(c.Nombre) + '</option>';
        });

        $('#selCondicionPagoCompra').html(html);
    }

    function claseBadgeEstadoOc(codigo) {
        var c = valor(codigo).toUpperCase();
        if (c === 'ABIERTA') return 'badge-stock badge-stock--warn';
        if (c === 'CERRADA') return 'badge-stock badge-stock--ok';
        return 'badge-stock';
    }

    function abrirModal() {
        ocultarError();
        $('#modalCompra').show();
    }

    function cerrarModal() {
        $('#modalCompra').hide();
        limpiarFormulario();
    }

    function limpiarFormulario() {
        $('#txtNumOc').val('');
        $('#selProveedorCompra').val('');
        $('#selEstadoOc').val('');
        $('#selCondicionPagoCompra').val('');
        $('#txtFechaOc').val('');
        $('#txtSubtotalCompra').val('');
        $('#txtImpuestoCompra').val('');
        $('#txtTotalCompra').val('');
        $('#txtObservacionesCompra').val('');
        compraSeleccionada = null;
        ocultarError();
    }

    function mostrarError(msg) {
        $('#compraErrorTexto').text(msg);
        $('#compraError').show();
    }

    function ocultarError() {
        $('#compraErrorTexto').text('');
        $('#compraError').hide();
    }

    function formatearMonto(v) {
        var n = parseFloat(v);
        return isNaN(n) ? '0.00' : n.toFixed(2);
    }

    function formatearFecha(valorFecha) {
        if (!valorFecha) return '';

        var matchAspNet = /\/Date\((\d+)\)\//.exec(valorFecha);
        if (matchAspNet) {
            var fechaAsp = new Date(parseInt(matchAspNet[1], 10));
            return fechaValida(fechaAsp) ? fechaAsp.toLocaleDateString('sv-SE') : '';
        }

        var fecha = new Date(valorFecha);
        return fechaValida(fecha) ? fecha.toLocaleDateString('sv-SE') : valor(valorFecha);
    }

    function formatearFechaInput(valorFecha) {
        if (!valorFecha) return '';

        var matchAspNet = /\/Date\((\d+)\)\//.exec(valorFecha);
        if (matchAspNet) {
            var fechaAsp = new Date(parseInt(matchAspNet[1], 10));
            return fechaValida(fechaAsp) ? fechaAsp.toISOString().split('T')[0] : '';
        }

        var fecha = new Date(valorFecha);
        return fechaValida(fecha) ? fecha.toISOString().split('T')[0] : '';
    }

    function fechaValida(fecha) {
        return fecha instanceof Date && !isNaN(fecha.getTime());
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

    function obtenerMensaje(xhr, mensajeDefault) {
        try {
            if (xhr.responseJSON && xhr.responseJSON.message) {
                return xhr.responseJSON.message;
            }
            return mensajeDefault;
        } catch (e) {
            return mensajeDefault;
        }
    }

    window.AdminCompras = {
        editar: editar,
        eliminar: eliminarCompra
    };
})();