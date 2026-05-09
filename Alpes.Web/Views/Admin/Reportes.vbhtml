@Code
    ViewData("Title") = "Reportes"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="reportes-page">
    <section class="reportes-hero">
        <button type="button" id="btnAbrirGenerarReporte" class="btn-a btn-a-gold report-generate-btn">
            <i class="bi bi-download"></i>
            Generar reporte
        </button>
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

    <div id="modalGenerarReporte" class="report-export-modal hidden">
        <div class="report-export-backdrop"></div>

        <div class="report-export-dialog">
            <div class="report-export-icon">
                <i class="bi bi-kanban"></i>
            </div>

            <h2>Generar reporte administrativo</h2>
            <p>Muebles de los Alpes</p>

            <label>Tipo de período</label>
            <div class="report-segment">
                <button type="button" class="report-period-btn active" data-periodo="rango">Rango de meses</button>
                <button type="button" class="report-period-btn" data-periodo="trimestre">Trimestre</button>
                <button type="button" class="report-period-btn" data-periodo="anual">Anual</button>
            </div>

            <div class="form-grid mt-16">
                <div class="a-form-group">
                    <label>Año</label>
                    <select id="repExportAnio" class="a-input no-icon">
                        <option value="2026">2026</option>
                        <option value="2025">2025</option>
                    </select>
                </div>

                <div id="repGrupoTrimestre" class="a-form-group hidden">
                    <label>Trimestre</label>
                    <select id="repExportTrimestre" class="a-input no-icon">
                        <option value="1">Q1 · Ene-Mar</option>
                        <option value="2">Q2 · Abr-Jun</option>
                        <option value="3">Q3 · Jul-Sep</option>
                        <option value="4">Q4 · Oct-Dic</option>
                    </select>
                </div>

                <div id="repGrupoMesInicio" class="a-form-group">
                    <label>Mes inicial</label>
                    <select id="repExportMesInicio" class="a-input no-icon">
                        <option value="1">Enero</option>
                        <option value="2">Febrero</option>
                        <option value="3">Marzo</option>
                        <option value="4">Abril</option>
                        <option value="5">Mayo</option>
                        <option value="6">Junio</option>
                        <option value="7">Julio</option>
                        <option value="8">Agosto</option>
                        <option value="9">Septiembre</option>
                        <option value="10">Octubre</option>
                        <option value="11">Noviembre</option>
                        <option value="12">Diciembre</option>
                    </select>
                </div>

                <div id="repGrupoMesFin" class="a-form-group">
                    <label>Mes final</label>
                    <select id="repExportMesFin" class="a-input no-icon">
                        <option value="1">Enero</option>
                        <option value="2">Febrero</option>
                        <option value="3">Marzo</option>
                        <option value="4">Abril</option>
                        <option value="5" selected>Mayo</option>
                        <option value="6">Junio</option>
                        <option value="7">Julio</option>
                        <option value="8">Agosto</option>
                        <option value="9">Septiembre</option>
                        <option value="10">Octubre</option>
                        <option value="11">Noviembre</option>
                        <option value="12">Diciembre</option>
                    </select>
                </div>
            </div>

            <label class="mt-16">Formato</label>
            <div class="report-format-grid">
                <button type="button" class="report-format-btn active" data-formato="pdf">
                    <i class="bi bi-filetype-pdf"></i>
                    PDF profesional
                </button>
                <button type="button" class="report-format-btn" data-formato="excel">
                    <i class="bi bi-table"></i>
                    Excel editable
                </button>
            </div>

            <div class="report-export-actions">
                <button type="button" id="btnCancelarGenerarReporte" class="btn-a btn-a-ghost">
                    Cancelar
                </button>

                <button type="button" id="btnGenerarReporte" class="btn-a btn-a-primary">
                    <i class="bi bi-download"></i>
                    Generar
                </button>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="@Url.Content("~/Scripts/admin-reportes.js")?v=@DateTime.Now.Ticks"></script>
End Section