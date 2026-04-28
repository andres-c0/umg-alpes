@Code
    ViewData("Title") = "Empleados"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Empleados</h2>
            <div class="admin-page__subtitle">Gestión de empleados registrados</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevoEmpleado">
            <i class="bi bi-person-plus"></i>
            Nuevo empleado
        </button>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarEmpleado" class="a-input no-icon" placeholder="Buscar empleado..." />
            <button type="button" class="btn-a" id="btnRecargarEmpleado">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="empleadosListado">
            <div class="table-empty">Cargando empleados...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalEmpleado" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">

        <div class="modal-a__header">
            <h3 id="modalEmpleadoTitulo">Nuevo empleado</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalEmpleadoX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">

                <div class="a-form-group">
                    <label>Nombres</label>
                    <input type="text" id="txtNombresEmp" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label>Apellidos</label>
                    <input type="text" id="txtApellidosEmp" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label>Email</label>
                    <input type="email" id="txtEmailEmp" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label>Teléfono</label>
                    <input type="text" id="txtTelefonoEmp" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label>Puesto</label>
                    <input type="text" id="txtPuestoEmp" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label>Estado</label>
                    <select id="selEstadoEmp" class="a-input no-icon">
                        <option value="ACTIVO">Activo</option>
                        <option value="INACTIVO">Inactivo</option>
                    </select>
                </div>

            </div>

            <div class="a-alert error" id="empleadoError" style="display:none;">
                <span id="empleadoErrorTexto"></span>
            </div>

        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModalEmpleado">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarEmpleado">Guardar</button>
        </div>

    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-empleados.js"></script>
End Section