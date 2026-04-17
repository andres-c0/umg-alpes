(function () {
    var ordenes = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarOrdenes();
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
            return !filtro || num.indexOf(filtro) >= 0;
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
            html += '<td>' + valor(o.CliId) + '</td>';
            html += '<td>' + formatearFecha(o.FechaOrden) + '</td>';
            html += '<td>Q ' + formatearMonto(o.Total) + '</td>';
            html += '<td>' + badgeEstado(o.Estado) + '</td>';
            html += '<td>';
            html += '<div class="table-actions">';
            html += '<button class="btn-icon" onclick="AdminOrdenes.verDetalle(' + o.OrdenVentaId + ')"><i class="bi bi-eye"></i></button>';
            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        $('#ordenBody').html(html);
    }

    function badgeEstado(estado) {
        estado = valor(estado).toUpperCase();

        if (estado === 'ACTIVO') {
            return '<span class="badge-stock badge-stock--ok">Activo</span>';
        }

        if (estado === 'INACTIVO') {
            return '<span class="badge-stock badge-stock--danger">Inactivo</span>';
        }

        return '<span class="badge-stock badge-stock--warn">' + valor(estado || 'Sin estado') + '</span>';
    }

    function formatearMonto(valorMonto) {
        var n = parseFloat(valorMonto);
        if (isNaN(n)) return '0.00';
        return n.toFixed(2);
    }

    function formatearFecha(valorFecha) {
        if (!valorFecha) return '';

        // Soporta /Date(1712707200000)/
        var matchAspNet = /\/Date\((\d+)\)\//.exec(valorFecha);
        if (matchAspNet) {
            var fechaAsp = new Date(parseInt(matchAspNet[1], 10));
            return fechaValida(fechaAsp) ? fechaAsp.toLocaleDateString() : '';
        }

        // Soporta ISO u otros formatos parseables
        var fecha = new Date(valorFecha);
        return fechaValida(fecha) ? fecha.toLocaleDateString() : valor(valorFecha);
    }

    function fechaValida(fecha) {
        return fecha instanceof Date && !isNaN(fecha.getTime());
    }

    function valor(v) {
        return v == null ? '' : String(v);
    }

    function verDetalle(id) {
        alert('Aquí abriremos el detalle de la orden #' + id);
    }

    window.AdminOrdenes = {
        verDetalle: verDetalle
    };
})();