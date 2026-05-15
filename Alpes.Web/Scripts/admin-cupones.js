(function () {
    var cupones = [];

    $(document).ready(function () {
        enlazarEventos();
        cargarCupones();
    });

    function enlazarEventos() {
        $('#btnNuevoCupon').on('click', function () {
            limpiarModal();
            $('#modalCuponTitulo').text('Crear cupón');
            abrirModal();
        });

        $('#btnCerrarModalCupon, #btnCancelarCupon, #modalCupon .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });

        $('#btnGuardarCupon').on('click', function () {
            guardarCupon();
        });
    }

    function cargarCupones() {
        $('#cuponesListado').html('<div class="table-empty">Cargando cupones...</div>');

        $.getJSON('/Admin/CuponesData')
            .done(function (res) {
                if (res && res.success === false) {
                    $('#cuponesListado').html('<div class="table-empty">' + escapeHtml(res.message) + '</div>');
                    return;
                }

                cupones = normalizarCupones(res || []);
                renderCupones();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#cuponesListado').html('<div class="table-empty">Error al cargar cupones.</div>');
            });
    }

    function renderCupones() {
        if (!cupones.length) {
            $('#cuponesListado').html('<div class="table-empty">No hay cupones registrados.</div>');
            return;
        }

        var html = '';

        cupones.forEach(function (c) {
            var activo = valor(c.Estado).toUpperCase() === 'ACTIVO';

            html += '<div class="cupon-card">';
            html += '   <div class="cupon-card__top">';
            html += '       <div class="cupon-card__left">';
            html += '           <div class="cupon-card__icon"><i class="bi bi-tag-fill"></i></div>';
            html += '           <div>';
            html += '               <div class="cupon-card__code">' + escapeHtml(c.Codigo) + '</div>';
            html += '               <div class="cupon-card__desc">' + escapeHtml(c.Descripcion) + '</div>';
            html += '           </div>';
            html += '       </div>';
            html += '       <span class="cupon-status ' + (activo ? 'cupon-status--activo' : 'cupon-status--inactivo') + '">' + (activo ? 'Activo' : 'Inactivo') + '</span>';
            html += '   </div>';

            html += '   <div class="cupon-card__detail">';
            html += '       <div><span>Vigencia</span><strong>' + fecha(c.VigenciaInicio) + ' - ' + fecha(c.VigenciaFin) + '</strong></div>';
            html += '       <div><span>Uso total</span><strong>' + entero(c.LimiteUsoTotal) + ' usos</strong></div>';
            html += '       <div><span>Por cliente</span><strong>' + entero(c.LimiteUsoPorCliente) + ' usos</strong></div>';
            html += '       <div><span>Usos actuales</span><strong>' + entero(c.UsosActuales) + '</strong></div>';
            html += '   </div>';

            html += '   <div class="cupon-card__actions">';
            html += '       <button type="button" class="btn-a btn-a-gold" onclick="AdminCupones.editar(' + entero(c.CuponId) + ')"><i class="bi bi-pencil-fill"></i> Editar</button>';
            html += '       <button type="button" class="btn-a btn-a-danger" onclick="AdminCupones.eliminar(' + entero(c.CuponId) + ')"><i class="bi bi-trash-fill"></i> Eliminar</button>';
            html += '   </div>';
            html += '</div>';
        });

        $('#cuponesListado').html(html);
    }

    function editar(id) {
        var c = cupones.find(function (x) {
            return entero(x.CuponId) === entero(id);
        });

        if (!c) {
            alert('No se encontró el cupón.');
            return;
        }

        $('#modalCuponTitulo').text('Editar cupón');
        $('#hidCuponId').val(c.CuponId);
        $('#txtCuponCodigo').val(c.Codigo);
        $('#txtCuponDescripcion').val(c.Descripcion);
        $('#txtCuponInicio').val(fechaInput(c.VigenciaInicio));
        $('#txtCuponFin').val(fechaInput(c.VigenciaFin));
        $('#txtCuponLimiteTotal').val(c.LimiteUsoTotal);
        $('#txtCuponLimiteCliente').val(c.LimiteUsoPorCliente);
        $('#selCuponEstado').val(valor(c.Estado).toUpperCase() || 'ACTIVO');

        abrirModal();
    }

    function guardarCupon() {
        var payload = {
            CuponId: entero($('#hidCuponId').val()),
            Codigo: ($('#txtCuponCodigo').val() || '').trim(),
            Descripcion: ($('#txtCuponDescripcion').val() || '').trim(),
            VigenciaInicio: $('#txtCuponInicio').val(),
            VigenciaFin: $('#txtCuponFin').val(),
            LimiteUsoTotal: entero($('#txtCuponLimiteTotal').val()),
            LimiteUsoPorCliente: entero($('#txtCuponLimiteCliente').val()),
            Estado: $('#selCuponEstado').val()
        };

        if (!payload.Codigo) {
            alert('El código es obligatorio.');
            return;
        }

        if (!payload.Descripcion) {
            alert('La descripción es obligatoria.');
            return;
        }

        $.ajax({
            url: '/Admin/GuardarCupon',
            method: 'POST',
            data: payload
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No se pudo guardar el cupón.');
                    return;
                }

                cerrarModal();
                cargarCupones();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert('Error al guardar el cupón.');
            });
    }

    function eliminar(id) {
        if (!confirm('¿Deseas eliminar este cupón?')) {
            return;
        }

        $.ajax({
            url: '/Admin/EliminarCupon',
            method: 'POST',
            data: { id: id }
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No se pudo eliminar el cupón.');
                    return;
                }

                cargarCupones();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert('Error al eliminar el cupón.');
            });
    }

    function abrirModal() {
        $('#modalCupon').show();
    }

    function cerrarModal() {
        $('#modalCupon').hide();
        limpiarModal();
    }

    function limpiarModal() {
        $('#hidCuponId').val(0);
        $('#txtCuponCodigo').val('');
        $('#txtCuponDescripcion').val('');
        $('#txtCuponInicio').val('');
        $('#txtCuponFin').val('');
        $('#txtCuponLimiteTotal').val(100);
        $('#txtCuponLimiteCliente').val(1);
        $('#selCuponEstado').val('ACTIVO');
    }

    function normalizarCupones(lista) {
        return lista.map(function (c) {
            return {
                CuponId: c.CuponId || c.cuponId || c.CUPON_ID || 0,
                Codigo: c.Codigo || c.codigo || c.CODIGO || '',
                Descripcion: c.Descripcion || c.descripcion || c.DESCRIPCION || '',
                VigenciaInicio: c.VigenciaInicio || c.vigenciaInicio || c.VIGENCIA_INICIO || '',
                VigenciaFin: c.VigenciaFin || c.vigenciaFin || c.VIGENCIA_FIN || '',
                LimiteUsoTotal: c.LimiteUsoTotal || c.limiteUsoTotal || c.LIMITE_USO_TOTAL || 0,
                LimiteUsoPorCliente: c.LimiteUsoPorCliente || c.limiteUsoPorCliente || c.LIMITE_USO_POR_CLIENTE || 0,
                UsosActuales: c.UsosActuales || c.usosActuales || c.USOS_ACTUALES || 0,
                Estado: c.Estado || c.estado || c.ESTADO || 'ACTIVO'
            };
        });
    }

    function fecha(v) {
        if (!v) return '--';

        var d = parseFechaAspNet(v);
        if (!d) return '--';

        return d.toLocaleDateString('es-GT');
    }

    function fechaInput(v) {
        if (!v) return '';

        var d = parseFechaAspNet(v);
        if (!d) return '';

        var yyyy = d.getFullYear();
        var mm = String(d.getMonth() + 1).padStart(2, '0');
        var dd = String(d.getDate()).padStart(2, '0');

        return yyyy + '-' + mm + '-' + dd;
    }

    function parseFechaAspNet(v) {
        if (!v) return null;

        var texto = String(v);

        var match = /\/Date\((\d+)\)\//.exec(texto);
        if (match) {
            var dAsp = new Date(parseInt(match[1], 10));
            return isNaN(dAsp.getTime()) ? null : dAsp;
        }

        var d = new Date(texto);
        return isNaN(d.getTime()) ? null : d;
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

    window.AdminCupones = {
        editar: editar,
        eliminar: eliminar
    };
})();