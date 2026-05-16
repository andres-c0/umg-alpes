(function () {
    var ordenes = [];
    var productos = [];
    var clientes = [];
    var detallesOrden = [];
    var estadoFiltro = 0;

    $(document).ready(function () {
        enlazarEventos();
        cargarCatalogos().then(function () {
            cargarOrdenes();
        });
    });

    function enlazarEventos() {
        $('#btnRecargarOrden').on('click', function () {
            cargarCatalogos().then(function () {
                cargarOrdenes();
            });
        });

        $('#txtBuscarOrden').on('input', function () {
            renderCards();
        });

        $('#btnNuevaOrden').on('click', function () {
            alert('El formulario de nueva orden será el siguiente paso.');
        });

        $('#btnCerrarDetalleOrden, #btnCerrarDetalleOrdenFooter, #modalDetalleOrden .modal-a__backdrop').on('click', cerrarDetalle);

        $('#btnActualizarEstadoOrden').on('click', actualizarEstadoOrden);
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

        var d3 = $.getJSON('/Orden_Venta_Detalle/Index')
            .done(function (res) {
                detallesOrden = normalizarLista(res);
            });

        return $.when(d1, d2, d3).fail(function () {
            console.warn('No se pudieron cargar productos, clientes o detalle de órdenes.');
        });
    }

    function cargarOrdenes() {
        $('#ordenContainer').html('<div class="table-empty">Cargando órdenes...</div>');

        $.getJSON('/Orden_Venta/Index')
            .done(function (res) {
                ordenes = normalizarLista(res);
                renderCards();
            })
            .fail(function (xhr) {
                console.error('ERROR ORDENES:', xhr.responseText);
                $('#ordenContainer').html('<div class="table-empty">Error al cargar órdenes.</div>');
            });
    }

    function normalizarLista(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderCards() {
        var filtro = ($('#txtBuscarOrden').val() || '').toLowerCase().trim();

        var lista = ordenes.filter(function (o) {
            var num = valor(o.NumOrden).toLowerCase();
            var cliente = nombreCliente(o.CliId).toLowerCase();
            var obs = valor(o.Observaciones).toLowerCase();

            var cumpleTexto = !filtro ||
                num.indexOf(filtro) >= 0 ||
                cliente.indexOf(filtro) >= 0 ||
                obs.indexOf(filtro) >= 0;

            var cumpleEstado = estadoFiltro === 0 || entero(o.EstadoOrdenId) === estadoFiltro;

            return cumpleTexto && cumpleEstado;
        });

        actualizarResumen(lista);

        if (!lista.length) {
            $('#ordenContainer').html('<div class="table-empty">No hay órdenes para mostrar.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (o) {
            var estado = estadoConfig(o.EstadoOrdenId);
            var cliente = nombreCliente(o.CliId);
            var total = formatearMonto(o.Total);
            var subtotal = formatearMonto(o.Subtotal);
            var impuesto = formatearMonto(o.Impuesto);
            var descuento = formatearMonto(o.Descuento);
            var fecha = formatearFecha(o.FechaOrden);
            var direccion = valor(o.DireccionEnvioSnapshot);

            var items = contarItemsOrden(o.OrdenVentaId);
            var lineas = contarLineasOrden(o.OrdenVentaId);

            html += ''
                + '<div class="orden-card" onclick="AdminOrdenes.verDetalle(' + o.OrdenVentaId + ')">'
                + '  <div class="orden-card__main">'
                + '    <div class="orden-card__icon"><i class="bi bi-receipt"></i></div>'
                + '    <div class="orden-card__info">'
                + '      <div class="orden-card__title">' + valor(o.NumOrden) + '</div>'
                + '      <div class="orden-card__client">' + cliente + '</div>'
                + '      <div class="orden-card__date"><i class="bi bi-calendar3"></i> ' + fecha + '</div>'
                + '      <div class="orden-card__chips">'
                + '        <span class="orden-chip"><i class="bi bi-bag"></i> ' + items + ' item' + (items === 1 ? '' : 's') + '</span>'
                + '        <span class="orden-chip"><i class="bi bi-list-ul"></i> ' + lineas + ' línea' + (lineas === 1 ? '' : 's') + '</span>'
                + '      </div>'
                + '    </div>'
                + '    <div class="orden-card__right">'
                + '      <div class="orden-card__total">Q ' + total + '</div>'
                + '      <span class="orden-badge ' + estado.clase + '">' + estado.texto + '</span>'
                + '      <i class="bi bi-chevron-right orden-card__arrow"></i>'
                + '    </div>'
                + '  </div>'
                + '  <div class="orden-card__amounts">'
                + '    <div><span>Subtotal</span><strong>Q ' + subtotal + '</strong></div>'
                + '    <div><span>Impuesto</span><strong>Q ' + impuesto + '</strong></div>'
                + '    <div><span>Descuento</span><strong>Q ' + descuento + '</strong></div>'
                + '  </div>'
                + (direccion ? '<div class="orden-card__address"><i class="bi bi-geo-alt"></i> Envío &nbsp; ' + direccion + '</div>' : '')
                + '</div>';
        });

        $('#ordenContainer').html(html);
    }

    function actualizarResumen(lista) {
        var totalVentas = 0;
        var pendientes = 0;
        var canceladas = 0;
        var itemsVisibles = 0;

        lista.forEach(function (o) {
            totalVentas += decimal(o.Total);
            itemsVisibles += contarItemsOrden(o.OrdenVentaId);

            if (entero(o.EstadoOrdenId) === 30) pendientes++;
            if (entero(o.EstadoOrdenId) === 35) canceladas++;
        });

        $('#resumenVentasFiltradas').text('Q ' + formatearMonto(totalVentas));
        $('#resumenPendientes').text(pendientes);
        $('#resumenItemsVisibles').text(itemsVisibles);
        $('#resumenCanceladas').text(canceladas);
        $('#ordenTotalTexto').text(lista.length + ' orden' + (lista.length === 1 ? '' : 'es'));

        $('#countTodos').text(ordenes.length);
        $('#adminOrdenesBadge').text(ordenes.length);
        $('#countPendiente').text(ordenes.filter(function (x) { return entero(x.EstadoOrdenId) === 30; }).length);
        $('#countProceso').text(ordenes.filter(function (x) { return entero(x.EstadoOrdenId) === 32; }).length);
        $('#countEntregado').text(ordenes.filter(function (x) { return entero(x.EstadoOrdenId) === 34; }).length);
        $('#countCancelado').text(ordenes.filter(function (x) { return entero(x.EstadoOrdenId) === 35; }).length);
    }

    function filtrarEstado(id) {
        estadoFiltro = entero(id);

        $('.orden-filter-pill').removeClass('active');
        $('.orden-filter-pill[data-estado="' + estadoFiltro + '"]').addClass('active');

        renderCards();
    }

    function cargarDetalleOrden(id) {
        $('#detalleOrdenBody').html('<div class="table-empty">Cargando detalle...</div>');
        $('#modalDetalleOrden').show();
        ocultarErrorDetalle();

        $.getJSON('/Orden_Venta/Obtener', { id: id })
            .done(function (orden) {
                if (!orden || !orden.OrdenVentaId) {
                    mostrarErrorDetalle('No se encontró la orden seleccionada.');
                    return;
                }

                llenarCabeceraDetalle(orden);

                var detalles = detallesDeOrden(id);
                renderDetalleProductos(detalles);
            })
            .fail(function () {
                mostrarErrorDetalle('No fue posible obtener la orden.');
            });
    }

    function llenarCabeceraDetalle(orden) {
        var estadoId = entero(orden.EstadoOrdenId);
        var estadoTexto = textoEstadoPorId(estadoId);

        $('#detalleOrdenTitulo').text('Orden #' + valor(orden.OrdenVentaId));
        $('#detalleNumOrden').text(valor(orden.NumOrden));
        $('#detalleFechaOrden').text(formatearFecha(orden.FechaOrden));

        $('#detalleSubtotalOrden').text('Q ' + formatearMonto(orden.Subtotal));
        $('#detalleImpuestoOrden').text('Q ' + formatearMonto(orden.Impuesto));
        $('#detalleDescuentoOrden').text('Q ' + formatearMonto(orden.Descuento));
        $('#detalleTotalOrden').text('Q ' + formatearMonto(orden.Total));

        $('#detalleDireccionOrden').text(valor(orden.DireccionEnvioSnapshot) || 'Sin dirección');
        $('#detalleEstadoOrden').html(badgeEstadoPorId(estadoId));
        $('#detalleNuevoEstado').val(estadoTexto);
        $('#detalleTimelineOrden').html(renderTimelineEstado(estadoId));

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

    function renderTimelineEstado(estadoActualId) {
        var actual = entero(estadoActualId);

        var estados = [
            { id: 30, texto: 'Pendiente', icono: 'bi-clock' },
            { id: 32, texto: 'En proceso', icono: 'bi-tools' },
            { id: 34, texto: 'Entregada', icono: 'bi-check-circle' },
            { id: 35, texto: 'Cancelada', icono: 'bi-x-circle' }
        ];

        var ordenActual = estados.findIndex(function (e) { return e.id === actual; });

        var html = '';

        estados.forEach(function (e, index) {
            var completado = ordenActual >= index && actual !== 35;
            var activo = e.id === actual;
            var cancelado = actual === 35 && e.id === 35;

            var clase = '';
            if (completado) clase = 'is-done';
            if (activo) clase += ' is-active';
            if (cancelado) clase += ' is-cancelled';

            html += ''
                + '<button type="button" class="orden-timeline__step ' + clase + '" onclick="AdminOrdenes.seleccionarEstadoDetalle(\'' + textoEstadoPorId(e.id) + '\')">'
                + '  <span class="orden-timeline__dot"><i class="bi ' + e.icono + '"></i></span>'
                + '  <span class="orden-timeline__label">' + e.texto + '</span>'
                + '</button>';
        });

        return html;
    }

    function cerrarDetalle() {
        $('#modalDetalleOrden').hide();
        $('#detalleOrdenBody').html('<div class="table-empty">Cargando detalle...</div>');
        ocultarErrorDetalle();
    }

    function actualizarEstadoOrden() {
        var ordenId = entero($('#detalleOrdenId').val());
        var estadoTexto = ($('#detalleNuevoEstado').val() || '').toUpperCase();

        var estadoMap = {
            PENDIENTE: 30,
            CONFIRMADO: 31,
            EN_PROCESO: 32,
            ENVIADO: 33,
            ENTREGADO: 34,
            CANCELADO: 35
        };

        var estadoOrdenId = estadoMap[estadoTexto] || 30;

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
                if (orden) orden.EstadoOrdenId = estadoOrdenId;

                $('#detalleEstadoOrden').html(badgeEstadoPorId(estadoOrdenId));
                $('#detalleTimelineOrden').html(renderTimelineEstado(estadoOrdenId));

                renderCards();
                ocultarErrorDetalle();
                alert('Estado actualizado correctamente.');
            })
            .fail(function (xhr) {
                mostrarErrorDetalle(obtenerMensaje(xhr, 'Error al actualizar el estado.'));
            });
    }

    function renderDetalleProductos(detalles) {
        if (!detalles || !detalles.length) {
            $('#detalleProductosTitulo').text('Productos (0)');
            $('#detalleOrdenBody').html('<div class="table-empty">La orden no tiene productos.</div>');
            return;
        }

        $('#detalleProductosTitulo').text('Productos (' + detalles.length + ')');

        var html = '';

        detalles.forEach(function (d) {
            var nombreProd = nombreProducto(d.ProductoId) || ('Producto #' + valor(d.ProductoId));
            var cantidad = entero(d.Cantidad);
            var precio = formatearMonto(d.PrecioUnitarioSnapshot);
            var subtotal = formatearMonto(d.SubtotalLinea);
            var imagen = imagenProducto(d.ProductoId);

            html += ''
                + '<div class="orden-product-item">'
                + '  <div class="orden-product-item__image">'
                + '    <img src="' + imagen + '" alt="' + escaparHtml(nombreProd) + '" onerror="this.src=\'' + placeholderProducto() + '\'" />'
                + '  </div>'
                + '  <div class="orden-product-item__info">'
                + '    <div class="orden-product-item__name">' + nombreProd + '</div>'
                + '    <div class="orden-product-item__meta">Cantidad: ' + cantidad + ' &nbsp; · &nbsp; Precio unit: Q ' + precio + '</div>'
                + '  </div>'
                + '  <div class="orden-product-item__total">Q ' + subtotal + '</div>'
                + '</div>';
        });

        $('#detalleOrdenBody').html(html);
    }

    function detallesDeOrden(ordenId) {
        return detallesOrden.filter(function (d) {
            return entero(d.OrdenVentaId) === entero(ordenId);
        });
    }

    function contarLineasOrden(ordenId) {
        return detallesDeOrden(ordenId).length;
    }

    function contarItemsOrden(ordenId) {
        return detallesDeOrden(ordenId).reduce(function (total, d) {
            return total + entero(d.Cantidad);
        }, 0);
    }

    function nombreCliente(id) {
        var item = clientes.find(function (c) { return c.CliId == id; });
        if (!item) return 'Cliente #' + valor(id);
        return item.NombreCompleto || ((item.Nombres || '') + ' ' + (item.Apellidos || '')).trim();
    }

    function estadoConfig(id) {
        id = entero(id);

        var estados = {
            30: { texto: 'Pendiente', clase: 'orden-badge--pendiente' },
            31: { texto: 'Confirmado', clase: 'orden-badge--confirmado' },
            32: { texto: 'En proceso', clase: 'orden-badge--proceso' },
            33: { texto: 'Enviado', clase: 'orden-badge--proceso' },
            34: { texto: 'Entregado', clase: 'orden-badge--entregado' },
            35: { texto: 'Cancelado', clase: 'orden-badge--cancelado' }
        };

        return estados[id] || { texto: 'Sin estado', clase: 'orden-badge--pendiente' };
    }

    function textoEstadoPorId(id) {
        id = entero(id);
        if (id === 31) return 'CONFIRMADO';
        if (id === 32) return 'EN_PROCESO';
        if (id === 33) return 'ENVIADO';
        if (id === 34) return 'ENTREGADO';
        if (id === 35) return 'CANCELADO';
        return 'PENDIENTE';
    }

    function badgeEstadoPorId(id) {
        var estado = estadoConfig(id);
        return '<span class="orden-badge ' + estado.clase + '">' + estado.texto + '</span>';
    }

    function obtenerProducto(id) {
        return productos.find(function (p) {
            return entero(p.ProductoId) === entero(id);
        }) || null;
    }

    function nombreProducto(id) {
        var item = obtenerProducto(id);
        if (!item) return '';
        return valor(item.Nombre) + (item.Referencia ? ' (' + item.Referencia + ')' : '');
    }

    function imagenProducto(id) {
        var item = obtenerProducto(id);
        return item && item.ImagenUrl ? item.ImagenUrl : placeholderProducto();
    }

    function placeholderProducto() {
        return 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(
            '<svg xmlns="http://www.w3.org/2000/svg" width="80" height="80">' +
            '<rect width="100%" height="100%" rx="14" fill="#f7f2ec"/>' +
            '<text x="50%" y="52%" text-anchor="middle" font-size="22" fill="#c4a882">📦</text>' +
            '</svg>'
        );
    }

    function formatearMonto(valorMonto) {
        var n = parseFloat(valorMonto);
        if (isNaN(n)) return '0.00';

        return n.toLocaleString('es-GT', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function formatearFecha(valorFecha) {
        if (!valorFecha) return '';

        var matchAspNet = /\/Date\((\d+)\)\//.exec(valorFecha);
        if (matchAspNet) {
            var fechaAsp = new Date(parseInt(matchAspNet[1], 10));
            return fechaValida(fechaAsp) ? fechaAsp.toLocaleDateString('es-GT') : '';
        }

        var fecha = new Date(valorFecha);
        return fechaValida(fecha) ? fecha.toLocaleDateString('es-GT') : valor(valorFecha);
    }

    function fechaValida(fecha) {
        return fecha instanceof Date && !isNaN(fecha.getTime());
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

    function escaparHtml(txt) {
        return String(txt || '')
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    }

    window.AdminOrdenes = {
        verDetalle: cargarDetalleOrden,
        seleccionarEstadoDetalle: function (estadoTexto) {
            $('#detalleNuevoEstado').val(estadoTexto);
        }
    };

    window.filtrarEstado = filtrarEstado;
})();