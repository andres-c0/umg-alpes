(function () {
    var chartVentas = null;
    var chartCompras = null;

    $(document).ready(function () {
        cargarDashboard();
    });

    function cargarDashboard() {
        $.getJSON('/Admin/DashboardData')
            .done(function (res) {
                if (res.success === false) {
                    console.error(res.message || 'Error al cargar dashboard');
                    return;
                }

                $('#kpiVentas').text('Q ' + formatearMonto(res.ventasMes));
                $('#kpiCompras').text('Q ' + formatearMonto(res.comprasMes));
                $('#kpiClientes').text(res.clientes || 0);
                $('#kpiOrdenes').text(res.ordenesActivas || 0);
                $('#kpiStock').text(res.stockBajo || 0);
                $('#kpiNomina').text(res.nominasPendientes || 0);

                renderChartVentas(res.ventasPorMes || []);
                renderChartCompras(res.comprasPorMes || []);
            })
            .fail(function (xhr) {
                console.error(xhr);
            });
    }

    function renderChartVentas(data) {
        var ctx = document.getElementById('chartVentas');
        if (!ctx) return;

        var labels = data.map(function (x) { return nombreMes(x.mes); });
        var valores = data.map(function (x) { return x.total; });

        if (chartVentas) {
            chartVentas.destroy();
        }

        chartVentas = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Ventas',
                    data: valores,
                    tension: 0.3
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { display: true }
                }
            }
        });
    }

    function renderChartCompras(data) {
        var ctx = document.getElementById('chartCompras');
        if (!ctx) return;

        var labels = data.map(function (x) { return nombreMes(x.mes); });
        var valores = data.map(function (x) { return x.total; });

        if (chartCompras) {
            chartCompras.destroy();
        }

        chartCompras = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Compras',
                    data: valores
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { display: true }
                }
            }
        });
    }

    function nombreMes(numero) {
        var meses = [
            '', 'Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
            'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'
        ];
        return meses[numero] || ('Mes ' + numero);
    }

    function formatearMonto(v) {
        var n = parseFloat(v);
        return isNaN(n) ? '0.00' : n.toFixed(2);
    }
})();