document.addEventListener('DOMContentLoaded', function () {
    var grid = document.getElementById('poOrdersGrid');
    var emptyState = document.getElementById('poEmptyState');
    var tabs = document.querySelectorAll('#poTabs .po-tab');
    var refreshBtn = document.getElementById('poRefreshBtn');

    if (!grid || !emptyState) {
        return;
    }

    var endpoint = '/PortalCliente/ObtenerMisOrdenesData';
    var filtroActual = 'TODAS';
    var ordenes = [];

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
            return { ok: false, data: [], message: 'Respuesta vacia del servidor.' };
        }

        if (payload.ok !== undefined) {
            return {
                ok: payload.ok === true,
                data: payload.data || [],
                message: payload.message || payload.mensaje || ''
            };
        }

        if (payload.success !== undefined) {
            return {
                ok: payload.success === true,
                data: payload.data || [],
                message: payload.message || payload.mensaje || ''
            };
        }

        return { ok: true, data: payload, message: '' };
    }

    function formatearMoneda(valor, moneda) {
        var numero = Number(valor || 0);
        var codigo = String(moneda || 'GTQ').trim();
        try {
            return new Intl.NumberFormat('es-GT', {
                style: 'currency',
                currency: codigo
            }).format(numero);
        } catch (error) {
            return codigo + ' ' + numero.toFixed(2);
        }
    }

    function actualizarContadores(lista) {
        var todas = 0;
        var activas = 0;
        var entregadas = 0;
        var canceladas = 0;
        var i;

        for (i = 0; i < lista.length; i += 1) {
            var estado = String(lista[i].EstadoUi || '').toUpperCase();
            todas += 1;

            if (estado === 'ENTREGADA') {
                entregadas += 1;
            } else if (estado === 'CANCELADA') {
                canceladas += 1;
            } else {
                activas += 1;
            }
        }

        var elTodas = document.getElementById('poCountTodas');
        var elActivas = document.getElementById('poCountActivas');
        var elEntregadas = document.getElementById('poCountEntregadas');
        var elCanceladas = document.getElementById('poCountCanceladas');

        if (elTodas) { elTodas.textContent = String(todas); }
        if (elActivas) { elActivas.textContent = String(activas); }
        if (elEntregadas) { elEntregadas.textContent = String(entregadas); }
        if (elCanceladas) { elCanceladas.textContent = String(canceladas); }
    }

    function coincideFiltro(item, filtro) {
        var estado = String(item.EstadoUi || '').toUpperCase();

        if (filtro === 'TODAS') {
            return true;
        }

        if (filtro === 'ACTIVAS') {
            return estado !== 'ENTREGADA' && estado !== 'CANCELADA';
        }

        if (filtro === 'ENTREGADAS') {
            return estado === 'ENTREGADA';
        }

        if (filtro === 'CANCELADAS') {
            return estado === 'CANCELADA';
        }

        return true;
    }

    function obtenerClaseEstado(estado) {
        var normalizado = String(estado || '').toUpperCase();

        if (normalizado === 'ENTREGADA') {
            return 'po-status po-status--success';
        }

        if (normalizado === 'CANCELADA') {
            return 'po-status po-status--danger';
        }

        return 'po-status po-status--warning';
    }

    function render(lista) {
        var html = '';
        var visibles = [];
        var i;

        for (i = 0; i < lista.length; i += 1) {
            if (coincideFiltro(lista[i], filtroActual)) {
                visibles.push(lista[i]);
            }
        }

        if (!visibles.length) {
            grid.innerHTML = '';
            emptyState.style.display = 'flex';
            return;
        }

        emptyState.style.display = 'none';

        for (i = 0; i < visibles.length; i += 1) {
            var item = visibles[i];
            var puedeRastrear = item.PuedeRastrear === true;
            var estado = item.EstadoUi || 'PENDIENTE';

            html += ''
                + '<div class="po-card">'
                + '  <div class="po-card-head">'
                + '      <div>'
                + '          <div class="po-order-id">' + escapeHtml(item.NumOrden || ('ORD-' + item.OrdenVentaId)) + '</div>'
                + '          <div class="po-order-product">' + escapeHtml(item.Resumen || 'Orden registrada') + '</div>'
                + '      </div>'
                + '      <span class="' + escapeHtml(obtenerClaseEstado(estado)) + '">' + escapeHtml(estado) + '</span>'
                + '  </div>'
                + '  <div class="po-row"><span>Fecha</span><strong>' + escapeHtml(item.FechaOrdenTexto || '') + '</strong></div>'
                + '  <div class="po-row"><span>Total</span><strong>' + escapeHtml(formatearMoneda(item.Total, item.Moneda)) + '</strong></div>'
                + '  <div class="po-row"><span>Direccion</span><strong>' + escapeHtml(item.Direccion || 'No disponible') + '</strong></div>'
                + '  <div class="po-actions">'
                + '      <a class="po-btn po-btn--primary" href="/PortalCliente/DetalleOrden?id=' + encodeURIComponent(item.OrdenVentaId) + '">Ver detalle</a>'
                + (puedeRastrear
                    ? '<a class="po-btn po-btn--ghost" href="/PortalCliente/Tracking?ordenVentaId=' + encodeURIComponent(item.OrdenVentaId) + '">Rastrear</a>'
                    : '')
                + '  </div>'
                + '</div>';
        }

        grid.innerHTML = html;
    }

    function setTabActiva(filtro) {
        filtroActual = filtro;

        tabs.forEach(function (tab) {
            if (tab.getAttribute('data-filter') === filtro) {
                tab.classList.add('active');
            } else {
                tab.classList.remove('active');
            }
        });

        render(ordenes);
    }

    function cargarOrdenes() {
        grid.innerHTML = '<div class="po-loading">Cargando ordenes...</div>';
        emptyState.style.display = 'none';

        fetch(endpoint, {
            method: 'GET',
            credentials: 'same-origin',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('No se pudieron cargar las ordenes.');
                }
                return response.json();
            })
            .then(function (payload) {
                var respuesta = normalizarRespuesta(payload);

                if (!respuesta.ok) {
                    throw new Error(respuesta.message || 'No se pudieron cargar las ordenes.');
                }

                ordenes = respuesta.data || [];
                actualizarContadores(ordenes);
                render(ordenes);
            })
            .catch(function (error) {
                grid.innerHTML = '';
                emptyState.style.display = 'flex';
                emptyState.innerHTML = ''
                    + '<div class="po-empty-icon"><i class="fa-solid fa-triangle-exclamation"></i></div>'
                    + '<div class="po-empty-title">No fue posible cargar las ordenes</div>'
                    + '<div class="po-empty-text">' + escapeHtml(error.message || 'Ocurrio un error inesperado.') + '</div>'
                    + '<button type="button" class="po-empty-link" id="poRetryBtn">Intentar de nuevo</button>';

                var retry = document.getElementById('poRetryBtn');
                if (retry) {
                    retry.addEventListener('click', cargarOrdenes);
                }
            });
    }

    tabs.forEach(function (tab) {
        tab.addEventListener('click', function () {
            setTabActiva(tab.getAttribute('data-filter') || 'TODAS');
        });
    });

    if (refreshBtn) {
        refreshBtn.addEventListener('click', cargarOrdenes);
    }

    cargarOrdenes();
});