document.addEventListener('DOMContentLoaded', function () {
    var btnAbrir = document.getElementById('btnAbrirAgregarTarjeta');
    var btnCerrar = document.getElementById('btnCerrarAgregarTarjeta');
    var overlay = document.getElementById('ptOverlay');
    var form = document.getElementById('ptForm');
    var cardsList = document.getElementById('ptCardsList');

    var inputTitular = document.getElementById('ptTitular');
    var inputNumero = document.getElementById('ptNumero');
    var inputMarca = document.getElementById('ptMarca');
    var inputMes = document.getElementById('ptMes');
    var inputAnio = document.getElementById('ptAnio');
    var inputAlias = document.getElementById('ptAlias');
    var inputPredeterminada = document.getElementById('ptPredeterminada');

    var endpointListar = '/PortalCliente/ObtenerTarjetasData';
    var endpointCrear = '/PortalCliente/CrearTarjetaData';
    var endpointPredeterminada = '/PortalCliente/MarcarTarjetaPredeterminadaData';
    var endpointDesactivar = '/PortalCliente/DesactivarTarjetaData';

    if (!btnAbrir || !overlay || !form || !cardsList) {
        return;
    }

    function abrirModal() {
        overlay.classList.add('show');
    }

    function cerrarModal() {
        overlay.classList.remove('show');
    }

    function setLoading(loading) {
        var submitBtn = form.querySelector('button[type="submit"]');
        if (submitBtn) {
            submitBtn.disabled = loading;
            submitBtn.textContent = loading ? 'GUARDANDO...' : 'GUARDAR';
        }
    }

    function escapeHtml(value) {
        return String(value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function normalizarRespuesta(payload) {
        if (!payload) {
            return { ok: false, data: null, message: 'Respuesta vacía del servidor.' };
        }

        if (payload.ok !== undefined) {
            return {
                ok: payload.ok === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }

        if (payload.success !== undefined) {
            return {
                ok: payload.success === true,
                data: payload.data || null,
                message: payload.message || payload.mensaje || ''
            };
        }

        return {
            ok: true,
            data: payload,
            message: ''
        };
    }

    function obtenerValor(obj, posiblesNombres) {
        var i;
        for (i = 0; i < posiblesNombres.length; i += 1) {
            if (obj && obj[posiblesNombres[i]] !== undefined && obj[posiblesNombres[i]] !== null) {
                return obj[posiblesNombres[i]];
            }
        }
        return '';
    }

    function numeroMascaradoDesdeData(tarjeta) {
        var numeroMascarado = obtenerValor(tarjeta, ['NumeroMascarado', 'numeroMascarado', 'NumeroEnmascarado', 'numeroEnmascarado']);
        var ultimos4 = obtenerValor(tarjeta, ['Ultimos4', 'ultimos4']);

        if (String(numeroMascarado).trim() !== '') {
            return String(numeroMascarado);
        }

        if (String(ultimos4).trim() !== '') {
            return '•••• •••• •••• ' + String(ultimos4).trim();
        }

        return '•••• •••• •••• 0000';
    }

    function renderEmptyState() {
        cardsList.innerHTML = ''
            + '<div class="pt-card-wrapper">'
            + '  <div class="pt-card pt-card-empty">'
            + '    <div class="pt-number">No hay tarjetas registradas.</div>'
            + '    <div class="pt-holder">Agrega una tarjeta desde la base de datos.</div>'
            + '  </div>'
            + '</div>';
    }

    function renderTarjetas(tarjetas) {
        if (!tarjetas || !tarjetas.length) {
            renderEmptyState();
            return;
        }

        var html = '';
        var i;

        for (i = 0; i < tarjetas.length; i += 1) {
            var tarjeta = tarjetas[i];
            var tarjetaId = obtenerValor(tarjeta, ['TarjetaClienteId', 'tarjetaClienteId', 'TarjetaId', 'tarjetaId']);
            var titular = obtenerValor(tarjeta, ['Titular', 'titular', 'NombreTitular', 'nombreTitular']);
            var marca = obtenerValor(tarjeta, ['Marca', 'marca', 'Franquicia', 'franquicia']);
            var alias = obtenerValor(tarjeta, ['Alias', 'alias']);
            var mes = obtenerValor(tarjeta, ['MesExpiracion', 'mesExpiracion', 'Mes', 'mes']);
            var anio = obtenerValor(tarjeta, ['AnioExpiracion', 'anioExpiracion', 'Anio', 'anio']);
            var tipoTarjeta = obtenerValor(tarjeta, ['TipoTarjeta', 'tipoTarjeta', 'Tipo', 'tipo']) || 'Crédito';
            var predeterminada = obtenerValor(tarjeta, ['EsPredeterminada', 'esPredeterminada', 'Predeterminada', 'predeterminada']) === true ||
                obtenerValor(tarjeta, ['EsPredeterminada', 'esPredeterminada', 'Predeterminada', 'predeterminada']) === 1 ||
                obtenerValor(tarjeta, ['EsPredeterminada', 'esPredeterminada', 'Predeterminada', 'predeterminada']) === '1' ||
                String(obtenerValor(tarjeta, ['EsPredeterminada', 'esPredeterminada', 'Predeterminada', 'predeterminada'])).toUpperCase() === 'S';

            html += ''
                + '<div class="pt-card-wrapper" data-tarjeta-id="' + escapeHtml(tarjetaId) + '">'
                + (predeterminada ? '<div class="pt-chip-row"><span class="pt-default">Predeterminada</span></div>' : '')
                + '  <div class="pt-card">'
                + '      <div class="pt-brand">' + escapeHtml(marca || 'Tarjeta') + '</div>'
                + '      <div class="pt-number">' + escapeHtml(numeroMascaradoDesdeData(tarjeta)) + '</div>'
                + '      <div class="pt-holder-label">TITULAR</div>'
                + '      <div class="pt-holder">' + escapeHtml(titular) + '</div>'
                + (String(alias).trim() !== '' ? '<div class="pt-alias">' + escapeHtml(alias) + '</div>' : '')
                + '      <div class="pt-footer">'
                + '          <div>'
                + '              <div class="pt-footer-label">TIPO</div>'
                + '              <div class="pt-footer-value">' + escapeHtml(tipoTarjeta) + '</div>'
                + '          </div>'
                + '          <div class="pt-expire-box">'
                + '              <div class="pt-footer-label">VENCE</div>'
                + '              <div class="pt-expire">' + escapeHtml(String(mes)) + '/' + escapeHtml(String(anio)) + '</div>'
                + '          </div>'
                + '      </div>'
                + '      <div class="pt-actions">'
                + (predeterminada ? '' : '<button type="button" class="pt-action-btn" data-action="predeterminada" data-id="' + escapeHtml(tarjetaId) + '">Marcar predeterminada</button>')
                + '          <button type="button" class="pt-action-btn pt-action-btn--danger" data-action="desactivar" data-id="' + escapeHtml(tarjetaId) + '">Eliminar</button>'
                + '      </div>'
                + '  </div>'
                + '</div>';
        }

        cardsList.innerHTML = html;
    }

    function cargarTarjetas() {
        fetch(endpointListar, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudieron obtener las tarjetas.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudieron obtener las tarjetas.');
                }

                renderTarjetas(respuesta.data || []);
            })
            .catch(function (error) {
                console.error(error);
                renderEmptyState();
            });
    }

    function postJson(url, payload) {
        return fetch(url, {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(payload)
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudo procesar la solicitud.');
                }
                return response.json();
            })
            .then(function (payloadRespuesta) {
                var respuesta = normalizarRespuesta(payloadRespuesta);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudo procesar la solicitud.');
                }

                return respuesta;
            });
    }

    btnAbrir.addEventListener('click', function () {
        abrirModal();
    });

    if (btnCerrar) {
        btnCerrar.addEventListener('click', function () {
            cerrarModal();
        });
    }

    overlay.addEventListener('click', function (e) {
        if (e.target === overlay) {
            cerrarModal();
        }
    });

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            cerrarModal();
        }
    });

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        var payload = {
            titular: inputTitular ? inputTitular.value.trim() : '',
            numero: inputNumero ? inputNumero.value.trim() : '',
            marca: inputMarca ? inputMarca.value : '',
            mesExpiracion: inputMes ? inputMes.value.trim() : '',
            anioExpiracion: inputAnio ? inputAnio.value.trim() : '',
            alias: inputAlias ? inputAlias.value.trim() : '',
            predeterminada: inputPredeterminada ? inputPredeterminada.checked : false
        };

        if (payload.titular === '' || payload.numero === '' || payload.mesExpiracion === '' || payload.anioExpiracion === '') {
            alert('Completa los campos obligatorios.');
            return;
        }

        setLoading(true);

        postJson(endpointCrear, payload)
            .then(function (respuesta) {
                form.reset();
                cerrarModal();
                cargarTarjetas();
                alert(respuesta.message || 'Tarjeta registrada correctamente.');
            })
            .catch(function (error) {
                alert(error.message || 'No se pudo registrar la tarjeta.');
            })
            .finally(function () {
                setLoading(false);
            });
    });

    cardsList.addEventListener('click', function (e) {
        var button = e.target.closest('button[data-action]');
        if (!button) {
            return;
        }

        var action = button.getAttribute('data-action');
        var tarjetaId = button.getAttribute('data-id');

        if (!tarjetaId) {
            return;
        }

        if (action === 'predeterminada') {
            postJson(endpointPredeterminada, { tarjetaId: tarjetaId })
                .then(function (respuesta) {
                    cargarTarjetas();
                    alert(respuesta.message || 'Tarjeta marcada como predeterminada.');
                })
                .catch(function (error) {
                    alert(error.message || 'No se pudo actualizar la tarjeta.');
                });
        }

        if (action === 'desactivar') {
            if (!window.confirm('¿Deseas eliminar esta tarjeta?')) {
                return;
            }

            postJson(endpointDesactivar, { tarjetaId: tarjetaId })
                .then(function (respuesta) {
                    cargarTarjetas();
                    alert(respuesta.message || 'Tarjeta eliminada correctamente.');
                })
                .catch(function (error) {
                    alert(error.message || 'No se pudo eliminar la tarjeta.');
                });
        }
    });

    cargarTarjetas();
});