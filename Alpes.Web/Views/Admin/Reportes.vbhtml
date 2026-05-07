@Code
    ViewData("Title") = "Reportes"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="reportes-page">
    <section class="reportes-hero">
        <h1>Reportes</h1>
        <p>Dashboard administrativo de órdenes y rendimiento</p>
    </section>

    <div class="reportes-alert">
        <i class="bi bi-arrow-repeat"></i>
        <div>
            <strong>Sincronización lista</strong>
            <span>Conexión activa con la API. Los indicadores se muestran con la información disponible.</span>
        </div>
    </div>

    <div class="report-section-title">
        <i class="bi bi-graph-up-arrow"></i>
        Resumen ejecutivo
    </div>

    <div class="report-kpi-grid">
        <div class="report-kpi report-kpi--green">
            <i class="bi bi-graph-up-arrow"></i>
            <span>Ventas totales</span>
            <strong id="kpiVentas">Q 0.00</strong>
        </div>

        <div class="report-kpi report-kpi--blue">
            <i class="bi bi-receipt"></i>
            <span>Órdenes</span>
            <strong id="kpiOrdenes">0</strong>
        </div>

        <div class="report-kpi report-kpi--gold">
            <i class="bi bi-wallet2"></i>
            <span>Ticket promedio</span>
            <strong id="kpiTicket">Q 0.00</strong>
        </div>

        <div class="report-kpi report-kpi--purple">
            <i class="bi bi-people-fill"></i>
            <span>Clientes</span>
            <strong id="kpiClientes">0</strong>
        </div>

        <div class="report-kpi report-kpi--darkgreen">
            <i class="bi bi-box-seam"></i>
            <span>Stock bajo ≤5</span>
            <strong id="kpiStockBajo">0</strong>
        </div>

        <div class="report-kpi report-kpi--red">
            <i class="bi bi-x-circle"></i>
            <span>Canceladas</span>
            <strong id="kpiCanceladas">0</strong>
        </div>
    </div>

    <div class="report-section-title">
        <i class="bi bi-graph-up-arrow"></i>
        Tendencia de ventas
    </div>

    <section class="report-card">
        <div class="report-card__head">
            <h3>Ventas por mes y comparación anual</h3>

            <div class="report-actions">
                <button class="report-chart-btn active" data-type="bar">Columnas</button>
                <button class="report-chart-btn" data-type="line">Línea</button>
                <button class="report-chart-btn" data-type="area">Área</button>
            </div>
        </div>

        <canvas id="chartVentas" height="85"></canvas>

        <div class="report-mini-grid">
            <div><span>Mes destacado</span><strong id="repMesDestacado">-</strong></div>
            <div><span>Año comparado</span><strong>2025</strong></div>
            <div><span>Usuarios activos</span><strong id="repUsuariosActivos">0</strong></div>
            <div><span>Usuarios año 2026</span><strong id="repUsuariosAnio">0</strong></div>
            <div><span>Items vendidos</span><strong id="repItemsVendidos">0</strong></div>
        </div>
    </section>

    <div class="report-section-title">
        <i class="bi bi-bar-chart-fill"></i>
        Comparación por trimestre
    </div>

    <section class="report-card">
        <h3>Comparación por trimestre</h3>
        <canvas id="chartTrimestre" height="80"></canvas>
    </section>

    <div class="report-section-title">
        <i class="bi bi-pie-chart-fill"></i>
        Estados de órdenes
    </div>

    <section class="report-card">
        <h3>Distribución general y composición por estado</h3>

        <div class="report-pie-grid">
            <div>
                <h4>Gráfica pie general</h4>
                <canvas id="chartEstadosPie" height="140"></canvas>
            </div>

            <div>
                <h4>Gráfica donut por estado</h4>
                <canvas id="chartEstadosDonut" height="140"></canvas>
            </div>
        </div>

        <div id="estadoChips" class="estado-chips"></div>
    </section>

    <div class="report-section-title">
        <i class="bi bi-exclamation-triangle"></i>
        Inventario en vigilancia
    </div>

    <section class="report-card">
        <h3>Productos con riesgo operativo o necesidad de reposición</h3>
        <div id="inventarioRiesgo" class="inventario-riesgo"></div>
    </section>

    <div class="report-section-title">
        <i class="bi bi-clock-history"></i>
        Últimas órdenes
    </div>

    <section class="report-card">
        <div class="report-card__head">
            <div>
                <h3>Últimas 10 órdenes</h3>
                <p>Monitorea el estado real de las órdenes más recientes.</p>
            </div>

            <button id="btnRecargarReportes" class="btn-a btn-a-primary">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar estados
            </button>
        </div>

        <div class="report-table-wrap">
            <table class="report-table">
                <thead>
                    <tr>
                        <th>Orden</th>
                        <th>Fecha</th>
                        <th>Items</th>
                        <th>Total</th>
                        <th>Estado actual</th>
                    </tr>
                </thead>
                <tbody id="tablaUltimasOrdenes"></tbody>
            </table>
        </div>
    </section>
</div>

@section scripts
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="@Url.Content("~/Scripts/admin-reportes.js")?v=@DateTime.Now.Ticks"></script>
End Section