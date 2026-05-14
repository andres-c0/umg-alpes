@Code
    ViewData("Title") = "Cupones"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page cupones-page">

    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Gestión de Cupones</h2>
            <div class="admin-page__subtitle">Administra descuentos, vigencias y límites de uso.</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevoCupon">
            <i class="bi bi-plus-lg"></i>
            Nuevo cupón
        </button>
    </div>

    <div id="cuponesListado" class="cupones-listado">
        <div class="table-empty">Cargando cupones...</div>
    </div>
</div>

<div class="modal-a" id="modalCupon" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog modal-a__dialog--config">
        <div class="modal-a__header">
            <h3 id="modalCuponTitulo">Crear cupón</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalCupon">×</button>
        </div>

        <div class="modal-a__body">
            <input type="hidden" id="hidCuponId" value="0" />

            <div class="form-grid">
                <div class="a-form-group">
                    <label>Código</label>
                    <input type="text" id="txtCuponCodigo" class="a-input no-icon" placeholder="Ej: DESC15" />
                </div>

                <div class="a-form-group">
                    <label>Estado</label>
                    <select id="selCuponEstado" class="a-input no-icon">
                        <option value="ACTIVO">Activo</option>
                        <option value="INACTIVO">Inactivo</option>
                    </select>
                </div>

                <div class="a-form-group form-grid__full">
                    <label>Descripción</label>
                    <input type="text" id="txtCuponDescripcion" class="a-input no-icon" placeholder="Descripción del cupón" />
                </div>

                <div class="a-form-group">
                    <label>Vigencia inicio</label>
                    <input type="date" id="txtCuponInicio" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label>Vigencia fin</label>
                    <input type="date" id="txtCuponFin" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label>Límite uso total</label>
                    <input type="number" id="txtCuponLimiteTotal" class="a-input no-icon" min="0" />
                </div>

                <div class="a-form-group">
                    <label>Límite por cliente</label>
                    <input type="number" id="txtCuponLimiteCliente" class="a-input no-icon" min="0" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a btn-a-ghost" id="btnCancelarCupon">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarCupon">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/admin-cupones.js?v=2")"></script>
End Section