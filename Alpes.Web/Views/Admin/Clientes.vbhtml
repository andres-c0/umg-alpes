@Code
    ViewData("Title") = "Clientes"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Clientes</h2>
            <div class="admin-page__subtitle">Gestión de clientes registrados</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevoCliente">
            <i class="bi bi-person-plus"></i>
            Nuevo cliente
        </button>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarCliente" class="a-input no-icon" placeholder="Buscar cliente..." />
            <button type="button" class="btn-a" id="btnRecargarCliente">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="clientesListado">
            <div class="table-empty">Cargando clientes...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalCliente" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">
        <div class="modal-a__header">
            <h3 id="modalClienteTitulo">Nuevo cliente</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <div class="a-form-group">
                    <label for="txtTipoDocumento">Tipo documento</label>
                    <input type="text" id="txtTipoDocumento" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtNumDocumento">No. documento</label>
                    <input type="text" id="txtNumDocumento" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtNombres">Nombres</label>
                    <input type="text" id="txtNombres" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtApellidos">Apellidos</label>
                    <input type="text" id="txtApellidos" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtEmail">Email</label>
                    <input type="email" id="txtEmail" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtDireccion">Dirección</label>
                    <input type="text" id="txtDireccion" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtCiudad">Ciudad</label>
                    <input type="text" id="txtCiudad" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtDepartamento">Departamento</label>
                    <input type="text" id="txtDepartamento" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtPais">País</label>
                    <input type="text" id="txtPais" class="a-input no-icon" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModal">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarCliente">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-clientes.js"></script>
End Section