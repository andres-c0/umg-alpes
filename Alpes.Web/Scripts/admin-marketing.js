(function () {
    var campanas = [];
    var campanaSeleccionada = null;

    var canalStyles = {
        'INSTAGRAM': {
            cardClass: 'marketing-card--instagram',
            badgeClass: 'marketing-tag--instagram',
            avatarClass: 'marketing-avatar--instagram'
        },
        'FACEBOOK': {
            cardClass: 'marketing-card--facebook',
            badgeClass: 'marketing-tag--facebook',
            avatarClass: 'marketing-avatar--facebook'
        },
        'EMAIL': {
            cardClass: 'marketing-card--email',
            badgeClass: 'marketing-tag--email',
            avatarClass: 'marketing-avatar--email'
        },
        'TV': {
            cardClass: 'marketing-card--tv',
            badgeClass: 'marketing-tag--tv',
            avatarClass: 'marketing-avatar--tv'
        },
        'REDES SOCIALES': {
            cardClass: 'marketing-card--social',
            badgeClass: 'marketing-tag--social',
            avatarClass: 'marketing-avatar--social'
        },
        'DEFAULT': {
            cardClass: 'marketing-card--default',
            badgeClass: 'marketing-tag--default',
            avatarClass: 'marketing-avatar--default'
        }
    };

    $(document).ready(function () {
        enlazarEventos();
        cargarCampanas();
        actualizarPreview();
    });

    function enlazarEventos() {
        $('#btnRecargarCampana').on('click', function () {
            cargarCampanas();
        });

        $('#txtBuscarCampana, #selFiltroCanal').on('input change', function () {
            renderCampanas();
        });

        $('#btnNuevaCampana').on('click', function () {
            campanaSeleccionada = null;
            limpiarFormulario();
            $('#modalCampanaTitulo').text('Nueva campaña');
            abrirModal();
        });

        $('#btnGuardarCampana').on('click', function () {
            guardarCampana();
        });

        $('#btnCerrarModalCampana, #btnCerrarModalCampanaX, #modalCampana .modal-a__backdrop').on('click', function () {
            cerrarModal();
        });

        $('#txtNombreCampana, #selCanalCampana, #txtPresupuestoCampana, #txtInicioCampana, #txtFinCampana').on('input change', function () {
            actualizarPreview();
        });
    }

    function cargarCampanas() {
        $('#marketingListado').html('<div class="table-empty">Cargando campañas...</div>');

        $.getJSON('/Campana_Marketing/Index')
            .done(function (res) {
                campanas = normalizar(res);
                renderCampanas();
                calcularKpis();
            })
            .fail(function (xhr) {
                console.error(xhr);
                $('#marketingListado').html('<div class="table-empty">Error al cargar campañas.</div>');
            });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderCampanas() {
    var filtro = ($('#txtBuscarCampana').val() || '').toLowerCase().trim();
    var canal = ($('#selFiltroCanal').val() || '').toLowerCase().trim();

    var lista = campanas.filter(function (c) {
        var texto = (valor(c.Nombre) + ' ' + valor(c.Canal)).toLowerCase();
        var cumpleTexto = !filtro || texto.indexOf(filtro) >= 0;
        var cumpleCanal = !canal || valor(c.Canal).toLowerCase() === canal;
        return cumpleTexto && cumpleCanal;
    });

    if (!lista.length) {
        $('#marketingListado').html('<div class="table-empty">No hay campañas registradas.</div>');
        calcularKpis();
        return;
    }

    var html = '';

    lista.forEach(function (c) {
        var scheme = obtenerScheme(c.Canal);
        var inicial = obtenerInicial(c.Nombre);

        html += '<div class="marketing-card ' + scheme.cardClass + '">';
        html += '   <div class="marketing-card__avatar ' + scheme.avatarClass + '">' + inicial + '</div>';

        html += '   <div class="marketing-card__content">';
        html += '       <div class="marketing-card__title">' + escapeHtml(valor(c.Nombre)) + '</div>';
        html += '       <div class="marketing-card__tag ' + scheme.badgeClass + '">' + escapeHtml(valor(c.Canal)) + '</div>';
        html += '       <div class="marketing-card__meta">';
        html += '           <span><i class="bi bi-cash-coin"></i> Q ' + formatearMonto(c.Presupuesto) + '</span>';
        html += '           <span><i class="bi bi-calendar-event"></i> ' + formatearFecha(c.Inicio) + ' → ' + formatearFecha(c.Fin) + '</span>';
        html += '       </div>';
        html += '   </div>';

        html += '   <div class="marketing-card__actions">';
        html += '       <button type="button" class="btn-icon" onclick="AdminMarketing.editar(' + entero(c.CampanaMarketingId) + ')"><i class="bi bi-pencil"></i></button>';
        html += '       <button type="button" class="btn-icon btn-icon-danger" onclick="AdminMarketing.eliminar(' + entero(c.CampanaMarketingId) + ')"><i class="bi bi-trash"></i></button>';
        html += '   </div>';
        html += '</div>';
    });

    $('#marketingListado').html(html);
    calcularKpis();
}

    function calcularKpis() {
        var total = 0;

        campanas.forEach(function (c) {
            total += decimal(c.Presupuesto);
        });

        $('#kpiCampanas').text(campanas.length);
        $('#kpiPresupuesto').text('Q ' + formatearMonto(total));
    }

    function editar(id) {
        $.getJSON('/Campana_Marketing/Obtener', { id: id })
            .done(function (res) {
                if (!res || res.success === false || !res.data) {
                    alert((res && res.message) || 'No se encontró la campaña.');
                    return;
                }

                var c = res.data;
                campanaSeleccionada = c;

                $('#modalCampanaTitulo').text('Editar campaña');
                $('#txtNombreCampana').val(valor(c.Nombre));
                $('#selCanalCampana').val(valor(c.Canal));
                $('#txtPresupuestoCampana').val(valor(c.Presupuesto));
                $('#txtInicioCampana').val(formatearFechaInput(c.Inicio));
                $('#txtFinCampana').val(formatearFechaInput(c.Fin));

                actualizarPreview();
                abrirModal();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al obtener la campaña.'));
            });
    }

    function guardarCampana() {
        var data = {
            CampanaMarketingId: campanaSeleccionada ? entero(campanaSeleccionada.CampanaMarketingId) : 0,
            Nombre: $('#txtNombreCampana').val(),
            Canal: $('#selCanalCampana').val(),
            Presupuesto: decimal($('#txtPresupuestoCampana').val()),
            Inicio: $('#txtInicioCampana').val(),
            Fin: $('#txtFinCampana').val()
        };

        if (!data.Nombre) {
            alert('El nombre de la campaña es obligatorio.');
            return;
        }

        if (!data.Canal) {
            alert('Debes seleccionar un canal.');
            return;
        }

        if (!data.Inicio || !data.Fin) {
            alert('Debes indicar fecha de inicio y fecha fin.');
            return;
        }

        if (new Date(data.Fin) < new Date(data.Inicio)) {
            alert('La fecha fin no puede ser menor que la fecha inicio.');
            return;
        }

        var url = campanaSeleccionada
            ? '/Campana_Marketing/Actualizar'
            : '/Campana_Marketing/Insertar';

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible guardar la campaña.');
                    return;
                }

                cerrarModal();
                cargarCampanas();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al guardar la campaña.'));
            });
    }

    function eliminarCampana(id) {
        if (!confirm('¿Deseas eliminar esta campaña?')) {
            return;
        }

        $.ajax({
            url: '/Campana_Marketing/Eliminar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: id })
        })
            .done(function (res) {
                if (res && res.success === false) {
                    alert(res.message || 'No fue posible eliminar.');
                    return;
                }

                cargarCampanas();
            })
            .fail(function (xhr) {
                console.error(xhr);
                alert(obtenerMensaje(xhr, 'Error al eliminar la campaña.'));
            });
    }

    function abrirModal() {
        $('#modalCampana').show();
    }

    function cerrarModal() {
        $('#modalCampana').hide();
        limpiarFormulario();
    }

    function limpiarFormulario() {
        $('#txtNombreCampana').val('');
        $('#selCanalCampana').val('');
        $('#txtPresupuestoCampana').val('');
        $('#txtInicioCampana').val('');
        $('#txtFinCampana').val('');
        campanaSeleccionada = null;
        actualizarPreview();
    }

    function actualizarPreview() {
        var nombre = $('#txtNombreCampana').val() || 'Campaña demo';
        var canal = $('#selCanalCampana').val() || 'Canal';
        var presupuesto = decimal($('#txtPresupuestoCampana').val());
        var inicio = $('#txtInicioCampana').val();
        var fin = $('#txtFinCampana').val();

        var scheme = obtenerScheme(canal);

        $('#marketingPreview')
            .removeClass('marketing-card--instagram marketing-card--facebook marketing-card--email marketing-card--tv marketing-card--social marketing-card--default')
            .addClass('marketing-preview--shell ' + scheme.cardClass);

        $('#marketingPreviewAvatar')
            .removeClass('marketing-avatar--instagram marketing-avatar--facebook marketing-avatar--email marketing-avatar--tv marketing-avatar--social marketing-avatar--default')
            .addClass(scheme.avatarClass)
            .text(obtenerInicial(nombre));

        $('#marketingPreviewCanal')
            .removeClass('marketing-tag--instagram marketing-tag--facebook marketing-tag--email marketing-tag--tv marketing-tag--social marketing-tag--default')
            .addClass('marketing-card__tag ' + scheme.badgeClass)
            .text(canal);

        $('#marketingPreviewNombre').text(nombre);

        var rango = '--';
        if (inicio && fin) {
            rango = inicio + ' → ' + fin;
        }

        $('#marketingPreviewMeta').html('Q ' + formatearMonto(presupuesto) + ' · ' + rango);
    }

    function obtenerScheme(canal) {
        var key = valor(canal).toUpperCase().trim();
        return canalStyles[key] || canalStyles.DEFAULT;
    }

    function obtenerInicial(texto) {
        var limpio = valor(texto).trim();
        if (!limpio) return 'C';
        return limpio.charAt(0).toUpperCase();
    }

    function formatearMonto(valorMonto) {
        var n = decimal(valorMonto);
        return n.toLocaleString('es-GT', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function formatearFecha(valorFecha) {
        var fecha = parseFecha(valorFecha);
        if (!fecha) return '--';

        var y = fecha.getFullYear();
        var m = String(fecha.getMonth() + 1).padStart(2, '0');
        var d = String(fecha.getDate()).padStart(2, '0');

        return y + '-' + m + '-' + d;
    }

    function formatearFechaInput(valorFecha) {
        return formatearFecha(valorFecha);
    }

    function parseFecha(valorFecha) {
        if (!valorFecha) return null;

        if (typeof valorFecha === 'string') {
            var limpio = valorFecha.replace('T00:00:00', '');

            if (/^\d{4}-\d{2}-\d{2}$/.test(limpio)) {
                var partes = limpio.split('-');
                return new Date(parseInt(partes[0], 10), parseInt(partes[1], 10) - 1, parseInt(partes[2], 10));
            }

            var match = /\/Date\((\d+)\)\//.exec(valorFecha);
            if (match) {
                return new Date(parseInt(match[1], 10));
            }

            var fechaJs = new Date(valorFecha);
            if (!isNaN(fechaJs.getTime())) return fechaJs;
        }

        if (valorFecha instanceof Date) return valorFecha;

        return null;
    }

    function decimal(v) {
        var n = parseFloat(v);
        return isNaN(n) ? 0 : n;
    }

    function entero(v) {
        var n = parseInt(v, 10);
        return isNaN(n) ? 0 : n;
    }

    function valor(v) {
        return v == null ? '' : String(v);
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

    function escapeHtml(texto) {
        return valor(texto)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    window.AdminMarketing = {
        editar: editar,
        eliminar: eliminarCampana
    };

})();