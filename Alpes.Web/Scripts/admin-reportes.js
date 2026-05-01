(function () {
    var ordenes = [];
    var clientes = [];
    var productos = [];
    var inventario = [];
    var detalles = [];

    var chartVentas = null;
    var chartTrimestre = null;
    var chartEstadosPie = null;
    var chartEstadosDonut = null;

    $(document).ready(function () {
        $('#btnRecargarReportes').on('click', cargarTodo);

        $('.report-chart-btn').on('click', function () {
            $('.report-chart-btn').removeClass('active');
            $(this).addClass('active');

            renderChartVentas($(this).data('type'));
        });

        cargarTodo();
    });

    function cargarTodo() {
        Promise.all([
            getJson('/Orden_Venta/Index'),
            getJson('/Cliente/ListarJson'),
            getJson('/Producto/Index'),
            getJson('/Inventario_Producto/Index'),
            getJson('/Orden_Venta_Detalle/Index')
        ]).then(function (res) {
            ordenes = normalizar(res[0]);
            clientes = normalizar(res[1]);
            productos = normalizar(res[2]);
            inventario = normalizar(res[3]);
            detalles = normalizar(res[4]);

            renderTodo();
        }).catch(function (err) {
            console.error('Error cargando reportes:', err);
        });
    }

    function getJson(url) {
        return $.getJSON(url).catch(function () {
            return [];
        });
    }

    function normalizar(res) {
        if ($.isArray(res)) return res;
        if (res && $.isArray(res.data)) return res.data;
        return [];
    }

    function renderTodo() {
        renderKpis();
        renderChartVentas('bar');
        renderChartTrimestre();
        renderEstados();
        renderInventarioRiesgo();
        renderUltimasOrdenes();
    }

    function renderKpis() {
        var totalVentas = ordenes.reduce(function (acc, o) {
            return acc + numero(o.Total || o.TOTAL);
        }, 0);

        var ticket = ordenes.length ? totalVentas / ordenes.length : 0;

        var canceladas = ordenes.filter(function (o) {
            return estadoId(o) === 35;
        }).length;

        var stockBajo = inventario.filter(function (i) {
            return numero(i.Stock || i.STOCK) <= 5;
        }).length;

        $('#kpiVentas').text('Q ' + formatoCompacto(totalVentas));
        $('#kpiOrdenes').text(ordenes.length);
        $('#kpiTicket').text('Q ' + formatoMonto(ticket));
        $('#kpiClientes').text(clientes.length);
        $('#kpiStockBajo').text(stockBajo);
        $('#kpiCanceladas').text(canceladas);

        $('#repUsuariosActivos').text(clientes.length);
        $('#repUsuariosAnio').text(clientes.length);
        $('#repItemsVendidos').text(totalItemsVendidos());
    }

    function renderChartVentas(tipo) {
        var meses = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];
        var actual = ventasPorMes(2026);
        var anterior = ventasPorMes(2025);

        var maxIndex = 0;
        actual.forEach(function (v, i) {
            if (v > actual[maxIndex]) maxIndex = i;
        });

        $('#repMesDestacado').text(meses[maxIndex] + ' 2026');

        var ctx = document.getElementById('chartVentas');

        if (chartVentas) chartVentas.destroy();

        var chartType = tipo === 'bar' ? 'bar' : 'line';

        chartVentas = new Chart(ctx, {
            type: chartType,
            data: {
                labels: meses,
                datasets: [
                    {
                        label: '2026',
                        data: actual,
                        borderColor: '#0f8d70',
                        backgroundColor: tipo === 'area' ? 'rgba(15,141,112,.18)' : '#0f8d70',
                        tension: .35,
                        fill: tipo === 'area'
                    },
                    {
                        label: '2025',
                        data: anterior,
                        borderColor: '#2f7fc1',
                        backgroundColor: tipo === 'area' ? 'rgba(47,127,193,.14)' : '#2f7fc1',
                        tension: .35,
                        fill: false,
                        borderDash: tipo === 'bar' ? [] : [6, 4]
                    }
                ]
            },
            options: opcionesChart()
        });
    }

    function renderChartTrimestre() {
        var ctx = document.getElementById('chartTrimestre');

        if (chartTrimestre) chartTrimestre.destroy();

        chartTrimestre = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Q1', 'Q2', 'Q3', 'Q4'],
                datasets: [
                    {
                        label: '2026',
                        data: ventasPorTrimestre(2026),
                        backgroundColor: '#0f8d70'
                    },
                    {
                        label: '2025',
                        data: ventasPorTrimestre(2025),
                        backgroundColor: '#2f7fc1'
                    }
                ]
            },
            options: opcionesChart()
        });
    }

    function renderEstados() {
        var estados = [
            { id: 30, nombre: 'Pendiente', color: '#c8922a' },
            { id: 32, nombre: 'En proceso', color: '#2f7fc1' },
            { id: 34, nombre: 'Entregado', color: '#2f8a38' },
            { id: 35, nombre: 'Cancelado', color: '#9d3030' }
        ];

        var data = estados.map(function (e) {
            return ordenes.filter(function (o) { return estadoId(o) === e.id; }).length;
        });

        var colors = estados.map(function (e) { return e.color; });

        crearPie('chartEstadosPie', data, estados.map(x => x.nombre), colors, false);
        crearPie('chartEstadosDonut', data, estados.map(x => x.nombre), colors, true);

        var total = ordenes.length || 1;
        var html = '';

        estados.forEach(function (e, i) {
            var porcentaje = Math.round((data[i] / total) * 100);

            html += '<span class="estado-chip">'
                + e.nombre + ': ' + data[i] + ' · ' + porcentaje + '%'
                + '</span>';
        });

        $('#estadoChips').html(html);
    }

    function crearPie(id, data, labels, colors, donut) {
        var ctx = document.getElementById(id);
        var existente = id === 'chartEstadosPie' ? chartEstadosPie : chartEstadosDonut;

        if (existente) existente.destroy();

        var chart = new Chart(ctx, {
            type: donut ? 'doughnut' : 'pie',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: colors
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'right'
                    }
                },
                cutout: donut ? '58%' : undefined
            }
        });

        if (id === 'chartEstadosPie') chartEstadosPie = chart;
        else chartEstadosDonut = chart;
    }

    function renderInventarioRiesgo() {
        var lista = inventario
            .filter(function (i) {
                return numero(i.Stock || i.STOCK) <= Math.max(5, numero(i.StockMinimo || i.STOCK_MINIMO));
            })
            .slice(0, 5);

        if (!lista.length) {
            $('#inventarioRiesgo').html('<div class="table-empty">No hay productos en riesgo.</div>');
            return;
        }

        var html = '';

        lista.forEach(function (i) {
            var productoId = i.ProductoId || i.PRODUCTO_ID;
            var prod = buscarProducto(productoId);
            var nombre = prod.Nombre || prod.NOMBRE || ('Producto #' + productoId);
            var ref = prod.Referencia || prod.REFERENCIA || '--';

            var stock = numero(i.Stock || i.STOCK);
            var reservado = numero(i.StockReservado || i.STOCK_RESERVADO);
            var minimo = numero(i.StockMinimo || i.STOCK_MINIMO);
            var disponible = stock - reservado;

            var cobertura = minimo > 0 ? Math.round((stock / minimo) * 100) : 100;

            html += ''
                + '<div class="riesgo-card">'
                + '  <div class="riesgo-card__icon"><i class="bi bi-exclamation-triangle"></i></div>'
                + '  <div class="riesgo-card__body">'
                + '    <div class="riesgo-card__title">' + nombre + ' · ' + ref + '</div>'
                + '    <div class="riesgo-chips">'
                + '      <span>Stock ' + stock + '</span>'
                + '      <span>Reservado ' + reservado + '</span>'
                + '      <span>Mínimo ' + minimo + '</span>'
                + '      <span>Disponible ' + disponible + '</span>'
                + '    </div>'
                + '    <div class="riesgo-bar"><div style="width:' + Math.min(cobertura, 100) + '%"></div></div>'
                + '    <small>Cobertura actual: ' + cobertura + '% del mínimo esperado.</small>'
                + '  </div>'
                + '  <span class="riesgo-badge">Vigilancia</span>'
                + '</div>';
        });

        $('#inventarioRiesgo').html(html);
    }

    function renderUltimasOrdenes() {
        var lista = ordenes.slice().sort(function (a, b) {
            return parseFechaNet(b.FechaOrden) - parseFechaNet(a.FechaOrden);
        }).slice(0, 10);

        var html = '';

        lista.forEach(function (o) {
            html += ''
                + '<tr>'
                + '  <td><strong>' + valor(o.NumOrden) + '</strong><br><span>Orden registrada</span></td>'
                + '  <td>' + formatoFechaNet(o.FechaOrden) + '</td>'
                + '  <td><span class="item-pill">' + itemsOrden(o.OrdenVentaId) + ' items</span></td>'
                + '  <td>Q ' + formatoMonto(o.Total || o.TOTAL) + '</td>'
                + '  <td>' + badgeEstado(estadoId(o)) + '</td>'
                + '</tr>';
        });

        $('#tablaUltimasOrdenes').html(html);
    }

    function ventasPorMes(anio) {
    var arr = Array(12).fill(0);

    ordenes.forEach(function (o) {
        var f = parseFechaNet(o.FechaOrden);

        if (!f) return;

        if (f.getFullYear() === anio) {
            var total = numero(o.Total || o.TOTAL);
            arr[f.getMonth()] += total;
        }
    });

    console.log("VENTAS", anio, arr);

    return arr;
}

    function parseFechaNet(fecha) {
        if (!fecha) return null;

        // Extrae el número de /Date(XXXX)/
        var match = /Date\((\d+)\)/.exec(fecha);

        if (!match) return null;

        return new Date(parseInt(match[1]));
    }

    function formatoFechaNet(fecha) {
        var f = parseFechaNet(fecha);
        if (!f) return '';
        return f.toLocaleDateString('es-GT');
    }

    function ventasPorTrimestre(anio) {
        var meses = ventasPorMes(anio);

        return [
            meses[0] + meses[1] + meses[2],
            meses[3] + meses[4] + meses[5],
            meses[6] + meses[7] + meses[8],
            meses[9] + meses[10] + meses[11]
        ];
    }

    function totalItemsVendidos() {
        return detalles.reduce(function (acc, d) {
            return acc + numero(d.Cantidad || d.CANTIDAD);
        }, 0);
    }

    function itemsOrden(id) {
        return detalles.filter(function (d) {
            return numero(d.OrdenVentaId || d.ORDEN_VENTA_ID) === numero(id);
        }).length;
    }

    function buscarProducto(id) {
        return productos.find(function (p) {
            return numero(p.ProductoId || p.PRODUCTO_ID) === numero(id);
        }) || {};
    }

    function estadoId(o) {
        return numero(o.EstadoOrdenId || o.ESTADO_ORDEN_ID || o.EstadoId || o.ESTADO_ID);
    }
    function badgeEstado(id) {
        var txt = 'Pendiente';
        var cls = 'pendiente';

        if (id === 32) { txt = 'En proceso'; cls = 'proceso'; }
        if (id === 34) { txt = 'Entregado'; cls = 'entregado'; }
        if (id === 35) { txt = 'Cancelado'; cls = 'cancelado'; }

        return '<span class="report-status report-status--' + cls + '">● ' + txt + '</span>';
    }

    function opcionesChart() {
        return {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top'
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: { color: '#eee4d6' }
                },
                x: {
                    grid: { display: false }
                }
            }
        };
    }

    function formatoMonto(v) {
        var n = numero(v);
        return n.toLocaleString('es-GT', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function formatoCompacto(v) {
        var n = numero(v);
        if (n >= 1000) return (n / 1000).toFixed(1) + 'k';
        return formatoMonto(n);
    }

    function formatoFecha(v) {
        if (!v) return '-';
        var f = new Date(v);
        if (isNaN(f.getTime())) return v;
        return f.toLocaleDateString('es-GT');
    }

    function valor(v) {
        return v == null ? '' : String(v);
    }

    function numero(v) {
        var n = parseFloat(v);
        return isNaN(n) ? 0 : n;
    }
})();