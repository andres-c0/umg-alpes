@Code
    ViewData("Title") = "Nómina"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Nómina</h2>
            <div class="admin-page__subtitle">Gestión de pagos de nómina</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevaNomina">
            <i class="bi bi-plus-lg"></i>
            Nueva nómina
        </button>
    </div>

    <div class="dashboard-grid">
        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon">
                <i class="bi bi-bank"></i>
            </div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiNominaBruta">Q 0.00</div>
                <div class="dashboard-kpi__label">Total bruto</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon">
                <i class="bi bi-cash-stack"></i>
            </div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiNominaNeta">Q 0.00</div>
                <div class="dashboard-kpi__label">Total neto</div>
            </div>
        </div>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarNomina" class="a-input no-icon" placeholder="Buscar nómina..." />
            <button type="button" class="btn-a" id="btnRecargarNomina">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="nominaListado">
            <div class="table-empty">Cargando nóminas...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalNomina" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">

        <div class="modal-a__header">
            <h3 id="modalNominaTitulo">Nueva nómina</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalNominaX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <div class="a-form-group form-grid__full">
                    <label for="selEmpleadoNomina">Empleado</label>
                    <select id="selEmpleadoNomina" class="a-input no-icon"></select>
                </div>

                <div class="a-form-group">
                    <label for="txtPeriodoInicioNomina">Período inicio</label>
                    <input type="date" id="txtPeriodoInicioNomina" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtPeriodoFinNomina">Período fin</label>
                    <input type="date" id="txtPeriodoFinNomina" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtMontoBrutoNomina">Monto bruto</label>
                    <input type="number" step="0.01" id="txtMontoBrutoNomina" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtMontoNetoNomina">Monto neto</label>
                    <input type="number" step="0.01" id="txtMontoNetoNomina" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtFechaPagoNomina">Fecha de pago</label>
                    <input type="date" id="txtFechaPagoNomina" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="selEstadoNomina">Estado</label>
                    <select id="selEstadoNomina" class="a-input no-icon">
                        <option value="PENDIENTE">PENDIENTE</option>
                        <option value="PAGADO">PAGADO</option>
                    </select>
                </div>
            </div>

            <div class="a-alert error" id="nominaError" style="display:none; margin-top:16px;">
                <i class="bi bi-exclamation-circle"></i>
                <span id="nominaErrorTexto"></span>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModalNomina">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarNomina">Guardar</button>
        </div>

    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-nomina.js"></script>
End Section