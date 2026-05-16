(function () {
    var ordenes = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarOrdenes();
    });

    function enlazarEventos() {
        $('#btnNuevaOP').on('click', function () {
            nuevaOrden();
        });

        $('#btnCerrarModalOP, #btnCancelarOP, #modalOP .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });

        $('#btnGuardarOP').on('click', function () {
            guardarOrden();
        });

        $('#btnRecargarProduccion').on('click', function () {
            cargarOrdenes();
        });

        $('#btnBuscarProduccion').on('click', function () {
            buscarOrdenes();
        });

        $('#txtBuscarProduccion').on('keypress', function (e) {
            if (e.which === 13) {
                buscarOrdenes();
            }
        });
    }

    function cargarOrdenes() {
        $('#produccionListado').html('<div class="table-empty">Cargando órdenes...</div>');

        $.getJSON('/OrdenProduccion/Index')
            .done(function (res) {
                if (res && res.success === false) {
                    $('#produccionListado').html('<div class="table-empty">' + escapeHtml(res.message || 'Error al cargar órdenes.') + '</div>');
                    return;
                }

                ordenes = $.isArray(res) ? res : (res.data || []);
                renderOrdenes();
            })
            .fail(function () {
                $('#produccionListado').html('<div class="table-empty">Error al cargar órdenes.</div>');
            });
    }

    function buscarOrdenes() {
        var texto = ($('#txtBuscarProduccion').val() || '').trim();

        if (!texto) {
            cargarOrdenes();
            return;
        }

        $.getJSON('/OrdenProduccion/Buscar', {
            criterio: 'NUM_OP',
            valor: texto
        })
            .done(function (res) {
                if (!res || res.success === false) {
                    $('#produccionListado').html('<div class="table-empty">' + escapeHtml((res && res.message) || 'No se encontraron resultados.') + '</div>');
                    return;
                }

                ordenes = res.data || [];
                renderOrdenes();
            })
            .fail(function () {
                $('#produccionListado').html('<div class="table-empty">Error al buscar órdenes.</div>');
            });
    }

    function renderOrdenes() {
        var html = '';
        var total = ordenes.length;
        var proceso = 0;
        var completadas = 0;

        ordenes.forEach(function (o) {
            var estadoVisual = obtenerEstadoVisual(o.EstadoProduccionId);

            if (estadoVisual === 'ENVIADA') {
                proceso++;
            }

            html += '<div class="produccion-card">';
            html += '   <div class="produccion-card__icon"><i class="bi bi-building"></i></div>';
            html += '   <div class="produccion-card__body">';
            html += '       <div class="produccion-card__title">' + escapeHtml(valor(o.NumOp)) + '</div>';
            html += '       <div class="produccion-card__sub">Producto #' + entero(o.ProductoId) + ' · Cant: ' + entero(o.CantidadPlanificada) + '</div>';
            html += '       <div class="produccion-card__meta">' + formatearFecha(o.InicioEstimado) + ' → ' + formatearFecha(o.FinEstimado) + '</div>';
            html += '   </div>';
            html += '   <div class="produccion-card__right">';
            html += '       <span class="badge-estado ' + obtenerClaseEstadoVisual(estadoVisual) + '">' + estadoVisual + '</span>';
            html += '       <div class="produccion-card__actions">';
            html += '           <button type="button" class="btn-icon" onclick="AdminProduccion.editar(' + entero(o.OrdenProduccionId) + ')"><i class="bi bi-pencil"></i></button>';
            html += '           <button type="button" class="btn-icon btn-icon-danger" onclick="AdminProduccion.eliminar(' + entero(o.OrdenProduccionId) + ')"><i class="bi bi-trash"></i></button>';
            html += '       </div>';
            html += '   </div>';
            html += '</div>';
        });

        if (!html) {
            html = '<div class="table-empty">No hay órdenes de producción registradas.</div>';
        }

        $('#produccionListado').html(html);
        $('#kpiTotal').text(total);
        $('#kpiProceso').text(proceso);
        $('#kpiCompletadas').text(completadas);
    }

    function nuevaOrden() {
        limpiarFormulario();
        $('#modalTituloOP').text('Nueva orden');
        $('#txtEstadoProduccionIdOP').val('1');
        $('#selEstadoVisualOP').val('1');
        abrirModal();
    }

    function editar(id) {
        $.getJSON('/OrdenProduccion/Obtener', { id: id })
            .done(function (res) {
                if (!res || !res.success || !res.data) {
                    alert((res && res.message) || 'No se encontró la orden.');
                    return;
                }

                var x = res.data;
                var estadoId = entero(x.EstadoProduccionId);

                $('#modalTituloOP').text('Editar orden');
                $('#hidOrdenProduccionId').val(entero(x.OrdenProduccionId));
                $('#txtNumeroOP').val(valor(x.NumOp));
                $('#txtProductoIdOP').val(entero(x.ProductoId));
                $('#txtCantidadOP').val(entero(x.CantidadPlanificada));
                $('#txtInicioOP').val(formatearFechaInput(x.InicioEstimado));
                $('#txtFinOP').val(formatearFechaInput(x.FinEstimado));
                $('#txtEstadoProduccionIdOP').val(estadoId > 0 ? estadoId : 1);
                $('#selEstadoVisualOP').val(String(estadoId > 0 ? estadoId : 1));

                abrirModal();
            })
            .fail(function () {
                alert('Error al obtener la orden.');
            });
    }

    function guardarOrden() {
        var estadoProduccionId = entero($('#selEstadoVisualOP').val());

        $('#txtEstadoProduccionIdOP').val(estadoProduccionId);

        var data = {
            OrdenProduccionId: entero($('#hidOrdenProduccionId').val()),
            NumOp: ($('#txtNumeroOP').val() || '').trim(),
            ProductoId: entero($('#txtProductoIdOP').val()),
            CantidadPlanificada: entero($('#txtCantidadOP').val()),
            EstadoProduccionId: estadoProduccionId,
            InicioEstimado: valor($('#txtInicioOP').val()),
            FinEstimado: valor($('#txtFinOP').val()),
            InicioReal: null,
            FinReal: null,
            Estado: 'ACTIVO'
        };

        if (!data.NumOp) {
            alert('El No. OP es obligatorio.');
            return;
        }

        if (data.ProductoId <= 0) {
            alert('Debes indicar un Producto ID válido.');
            return;
        }

        if (data.CantidadPlanificada <= 0) {
            alert('La cantidad planificada debe ser mayor a cero.');
            return;
        }

        if (data.EstadoProduccionId <= 0) {
            alert('Debes seleccionar un estado válido.');
            return;
        }

        if (!data.InicioEstimado || !data.FinEstimado) {
            alert('Debes completar inicio y fin estimado.');
            return;
        }

        if (new Date(data.FinEstimado) < new Date(data.InicioEstimado)) {
            alert('La fecha fin no puede ser menor que la fecha inicio.');
            return;
        }

        var url = data.OrdenProduccionId > 0
            ? '/OrdenProduccion/Actualizar'
            : '/OrdenProduccion/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        })
            .done(function (res) {
                if (!res || res.success === false) {
                    alert((res && res.message) || 'No se pudo guardar.');
                    return;
                }

                cerrarModal();
                cargarOrdenes();
            })
            .fail(function () {
                alert('Error al guardar la orden.');
            });
    }

    function eliminarOrden(id) {
        if (!confirm('¿Deseas eliminar esta orden de producción?')) {
            return;
        }

        $.ajax({
            url: '/OrdenProduccion/Eliminar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ OrdenProduccionId: id })
        })
            .done(function (res) {
                if (!res || res.success === false) {
                    alert((res && res.message) || 'No se pudo eliminar.');
                    return;
                }

                cargarOrdenes();
            })
            .fail(function () {
                alert('Error al eliminar la orden.');
            });
    }

    function abrirModal() {
        $('#modalOP').show();
    }

    function cerrarModal() {
        $('#modalOP').hide();
    }

    function limpiarFormulario() {
        $('#hidOrdenProduccionId').val(0);
        $('#txtNumeroOP').val('');
        $('#txtProductoIdOP').val('');
        $('#txtCantidadOP').val('');
        $('#txtInicioOP').val('');
        $('#txtFinOP').val('');
        $('#txtEstadoProduccionIdOP').val('1');
        $('#selEstadoVisualOP').val('1');
    }

    function obtenerEstadoVisual(estadoProduccionId) {
        var id = entero(estadoProduccionId);

        switch (id) {
            case 2:
                return 'ENVIADA';
            case 1:
            default:
                return 'PLANIFICADA';
        }
    }

    function obtenerClaseEstadoVisual(estadoVisual) {
        switch (estadoVisual) {
            case 'ENVIADA':
                return 'badge-enviada';
            case 'PLANIFICADA':
            default:
                return 'badge-planificada';
        }
    }

    function formatearFecha(valorFecha) {
        if (!valorFecha) return '--';

        if (typeof valorFecha === 'string') {
            var limpio = valorFecha.replace('T00:00:00', '');

            if (/^\d{4}-\d{2}-\d{2}/.test(limpio)) {
                return limpio.substring(0, 10);
            }

            var match = /\/Date\((\d+)\)\//.exec(valorFecha);
            if (match) {
                var fechaMs = new Date(parseInt(match[1], 10));
                return fechaMs.getFullYear() + '-' +
                    String(fechaMs.getMonth() + 1).padStart(2, '0') + '-' +
                    String(fechaMs.getDate()).padStart(2, '0');
            }
        }

        return valor(valorFecha);
    }

    function formatearFechaInput(valorFecha) {
        return formatearFecha(valorFecha);
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

    window.AdminProduccion = {
        editar: editar,
        eliminar: eliminarOrden
    };
})();