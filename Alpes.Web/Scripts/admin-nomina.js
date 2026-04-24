(function () {
    var nominas = [];
    var empleados = [];
    var nominaSeleccionada = null;

    $(document).ready(function () {
        enlazarEventos();
        cargarCatalogos().then(function () {
            cargarNominas();
        });
    });

    function enlazarEventos() {
        $('#btnRecargarNomina').on('click', function () {
            cargarNominas();
        });

        $('#txtBuscarNomina').on('input', function () {
            renderNominas();
        });

        $('#btnNuevaNomina').on('click', function () {
            nominaSeleccionada = null;
            limpiarFormulario();
            $('#modalNominaTitulo').text('Nueva nómina');
            abrirModal();
        });

        $('#btnGuardarNomina').on('click', function () {
            guardarNomina();
        });

        $('#btnCerrarModalNomina, #btnCerrarModalNominaX, #modalNomina .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });
    }

    function cargarCatalogos() {
        return $.getJSON('/Empleado/Index')
            .done(function (res) {
                empleados = normalizar(res);
                llenarEmpleados();
            })
            .fail(function () {
                console.warn('No se pudieron cargar empleados.');
            });
    }

    function cargarNominas() {
        $('#nominaListado').html('<div class="table-empty">Cargando nóminas...</div>');

        $.getJSON('/Nomina/Index')
            .done(function (res) {
                nominas = normalizar(res);
                renderNominas();
                calcularKpis();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#nominaListado').html('<div class="table-empty">Error al cargar nóminas.</div>');
            });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderNominas() {
        var filtro = ($('#txtBuscarNomina').val() || '').toLowerCase().trim();

        var lista = nominas.filter(function (n) {
            var texto = (
                nombreEmpleado(n.EmpId) + ' ' +
                valor(n.PeriodoInicio) + ' ' +
                valor(n.PeriodoFin) + ' ' +
                valor(n.Estado)
            ).toLowerCase();

            return !filtro || texto.indexOf(filtro) >= 0;
        });

        if (!lista.length) {
            $('#nominaListado').html('<div class="table-empty">No hay nóminas.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (n) {
            html += '<div class="cliente-card">';
            html += '  <div class="cliente-card__avatar"><i class="bi bi-wallet2"></i></div>';
            html += '  <div class="cliente-card__body">';
            html += '      <div class="cliente-card__name">' + valor(nombreEmpleado(n.EmpId)) + '</div>';
            html += '      <div class="cliente-card__meta">' + formatearFecha(n.PeriodoInicio) + ' – ' + formatearFecha(n.PeriodoFin) + '</div>';
            html += '      <div class="cliente-card__meta">Bruto: Q ' + formatearMonto(n.MontoBruto) + ' &nbsp;&nbsp; Neto: Q ' + formatearMonto(n.MontoNeto) + '</div>';
            html += '  </div>';
            html += '  <div class="cliente-card__actions">';
            html += '      <span class="' + claseEstadoNomina(n.Estado) + '">' + valor(n.Estado) + '</span>';
            html += '      <button type="button" class="btn-icon" onclick="AdminNomina.editar(' + n.NominaId + ')"><i class="bi bi-pencil"></i></button>';
            html += '      <button type="button" class="btn-icon btn-icon-danger" onclick="AdminNomina.eliminar(' + n.NominaId + ')"><i class="bi bi-trash"></i></button>';
            html += '  </div>';
            html += '</div>';
        });

        $('#nominaListado').html(html);
    }

    function calcularKpis() {
        var bruto = 0;
        var neto = 0;

        nominas.forEach(function (n) {
            bruto += decimal(n.MontoBruto);
            neto += decimal(n.MontoNeto);
        });

        $('#kpiNominaBruta').text('Q ' + formatearMonto(bruto));
        $('#kpiNominaNeta').text('Q ' + formatearMonto(neto));
    }

    function editar(id) {
        $.getJSON('/Nomina/Obtener', { id: id })
            .done(function (res) {
                if (!res || !res.NominaId) {
                    mostrarError('No se encontró la nómina.');
                    return;
                }

                nominaSeleccionada = res;
                $('#modalNominaTitulo').text('Editar nómina');

                $('#selEmpleadoNomina').val(valor(res.EmpId));
                $('#txtPeriodoInicioNomina').val(formatearFechaInput(res.PeriodoInicio));
                $('#txtPeriodoFinNomina').val(formatearFechaInput(res.PeriodoFin));
                $('#txtMontoBrutoNomina').val(valor(res.MontoBruto));
                $('#txtMontoNetoNomina').val(valor(res.MontoNeto));
                $('#txtFechaPagoNomina').val(formatearFechaInput(res.FechaPago));
                $('#selEstadoNomina').val(valor(res.Estado));

                abrirModal();
            })
            .fail(function (xhr) {
                console.error(xhr);
                mostrarError(obtenerMensaje(xhr, 'Error al obtener la nómina.'));
            });
    }

    function guardarNomina() {
        var data = {
            NominaId: nominaSeleccionada ? nominaSeleccionada.NominaId : 0,
            EmpId: entero($('#selEmpleadoNomina').val()),
            PeriodoInicio: $('#txtPeriodoInicioNomina').val(),
            PeriodoFin: $('#txtPeriodoFinNomina').val(),
            MontoBruto: decimal($('#txtMontoBrutoNomina').val()),
            MontoNeto: decimal($('#txtMontoNetoNomina').val()),
            FechaPago: $('#txtFechaPagoNomina').val(),
            Estado: $('#selEstadoNomina').val()
        };

        if (!data.EmpId) {
            mostrarError('Debes seleccionar un empleado.');
            return;
        }

        if (!data.PeriodoInicio || !data.PeriodoFin) {
            mostrarError('Debes indicar el período.');
            return;
        }

        if (!data.FechaPago) {
            mostrarError('Debes indicar la fecha de pago.');
            return;
        }

        var url = nominaSeleccionada
            ? '/Nomina/Actualizar'
            : '/Nomina/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    mostrarError(res.message || 'No fue posible guardar la nómina.');
                    return;
                }

                cerrarModal();
                cargarNominas();
            })
            .fail(function (xhr) {
                console.error(xhr);
                mostrarError(obtenerMensaje(xhr, 'Error al guardar la nómina.'));
            });
    }

    function eliminarNomina(id) {
        if (!confirm('¿Deseas eliminar esta nómina?')) {
            return;
        }

        $.ajax({
            url: '/Nomina/Eliminar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ NominaId: id })
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarNominas();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al eliminar la nómina.'));
            });
    }

    function llenarEmpleados() {
        var html = '<option value="">Seleccione empleado</option>';

        empleados.forEach(function (e) {
            var nombre = valor(e.Nombres) + ' ' + valor(e.Apellidos);
            html += '<option value="' + e.EmpId + '">' + nombre.trim() + ' (#' + e.EmpId + ')</option>';
        });

        $('#selEmpleadoNomina').html(html);
    }

    function nombreEmpleado(empId) {
        var item = empleados.find(function (e) { return e.EmpId == empId; });
        if (!item) return 'Empleado #' + valor(empId);
        return (valor(item.Nombres) + ' ' + valor(item.Apellidos)).trim() + ' (#' + item.EmpId + ')';
    }

    function claseEstadoNomina(estado) {
        var e = valor(estado).toUpperCase();
        if (e === 'PAGADO') return 'badge-stock badge-stock--ok';
        if (e === 'PENDIENTE') return 'badge-stock badge-stock--warn';
        return 'badge-stock';
    }

    function abrirModal() {
        ocultarError();
        $('#modalNomina').show();
    }

    function cerrarModal() {
        $('#modalNomina').hide();
        limpiarFormulario();
    }

    function limpiarFormulario() {
        $('#selEmpleadoNomina').val('');
        $('#txtPeriodoInicioNomina').val('');
        $('#txtPeriodoFinNomina').val('');
        $('#txtMontoBrutoNomina').val('');
        $('#txtMontoNetoNomina').val('');
        $('#txtFechaPagoNomina').val('');
        $('#selEstadoNomina').val('PENDIENTE');
        nominaSeleccionada = null;
        ocultarError();
    }

    function mostrarError(msg) {
        $('#nominaErrorTexto').text(msg);
        $('#nominaError').show();
    }

    function ocultarError() {
        $('#nominaErrorTexto').text('');
        $('#nominaError').hide();
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

    window.AdminNomina = {
        editar: editar,
        eliminar: eliminarNomina
    };
})();