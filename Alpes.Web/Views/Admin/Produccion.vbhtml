@Code
    ViewData("Title") = "Producción"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page produccion-page">

    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Producción</h2>
            <div class="admin-page__subtitle">Órdenes de producción</div>
        </div>

        <button class="btn-a btn-a-primary" id="btnNuevaOP" type="button">
            <i class="bi bi-plus-lg"></i>
            Nueva orden
        </button>
    </div>

    <div class="produccion-kpis">
        <div class="admin-card produccion-kpi">
            <div class="produccion-kpi__icon">
                <i class="bi bi-calendar-week"></i>
            </div>
            <div>
                <div class="produccion-kpi__value" id="kpiTotal">0</div>
                <div class="produccion-kpi__label">Total órdenes</div>
            </div>
        </div>

        <div class="admin-card produccion-kpi">
            <div class="produccion-kpi__icon icon-blue">
                <i class="bi bi-arrow-repeat"></i>
            </div>
            <div>
                <div class="produccion-kpi__value" id="kpiProceso">0</div>
                <div class="produccion-kpi__label">En proceso</div>
            </div>
        </div>

        <div class="admin-card produccion-kpi">
            <div class="produccion-kpi__icon icon-green">
                <i class="bi bi-check-circle"></i>
            </div>
            <div>
                <div class="produccion-kpi__value" id="kpiCompletadas">0</div>
                <div class="produccion-kpi__label">Completadas</div>
            </div>
        </div>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar produccion-toolbar">
            <input type="text" id="txtBuscarProduccion" class="a-input no-icon" placeholder="Buscar por No. OP o estado..." />
            <button type="button" class="btn-a btn-a-outline" id="btnBuscarProduccion">
                <i class="bi bi-search"></i>
                Buscar
            </button>
            <button type="button" class="btn-a" id="btnRecargarProduccion">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="produccionListado" class="produccion-listado">
            <div class="table-empty">Cargando órdenes...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalOP" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog modal-a__dialog--produccion">
        <div class="modal-a__header">
            <h3 id="modalTituloOP">Nueva orden</h3>
            <button class="modal-a__close" id="btnCerrarModalOP" type="button">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <input type="hidden" id="hidOrdenProduccionId" value="0" />

                <div class="a-form-group form-grid__full">
                    <label for="txtNumeroOP">No. OP</label>
                    <input type="text" id="txtNumeroOP" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtProductoIdOP">Producto ID</label>
                    <input type="number" id="txtProductoIdOP" class="a-input no-icon" min="1" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtCantidadOP">Cantidad planificada</label>
                    <input type="number" id="txtCantidadOP" class="a-input no-icon" min="1" />
                </div>

                <div class="a-form-group">
                    <label for="txtInicioOP">Inicio estimado</label>
                    <input type="date" id="txtInicioOP" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtFinOP">Fin estimado</label>
                    <input type="date" id="txtFinOP" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full" style="display:none;">
                    <label for="txtEstadoProduccionIdOP">EstadoProduccionId</label>
                    <input type="number" id="txtEstadoProduccionIdOP" class="a-input no-icon" min="1" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="selEstadoVisualOP">Estado</label>
                    <select id="selEstadoVisualOP" class="a-input no-icon">
                        <option value="1">PLANIFICADA</option>
                        <option value="2">ENVIADA</option>
                    </select>
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtEstadoRegistroOP">Estado del registro</label>
                    <input type="text" id="txtEstadoRegistroOP" class="a-input no-icon" value="ACTIVO" readonly="readonly" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button class="btn-a" id="btnCancelarOP" type="button">Cancelar</button>
            <button class="btn-a btn-a-primary" id="btnGuardarOP" type="button">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-produccion.js?v=3"></script>
End Section