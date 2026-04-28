@Code
    ViewData("Title") = "Proveedores"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Proveedores</h2>
            <div class="admin-page__subtitle">Gestión de proveedores registrados</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevoProveedor">
            <i class="bi bi-truck"></i>
            Nuevo proveedor
        </button>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarProveedor" class="a-input no-icon" placeholder="Buscar proveedor..." />
            <button type="button" class="btn-a" id="btnRecargarProveedor">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="proveedoresListado">
            <div class="table-empty">Cargando proveedores...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalProveedor" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">
        <div class="modal-a__header">
            <h3 id="modalProveedorTitulo">Nuevo proveedor</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalProveedorX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <div class="a-form-group form-grid__full">
                    <label for="txtRazonSocial">Razón social</label>
                    <input type="text" id="txtRazonSocial" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtNit">NIT</label>
                    <input type="text" id="txtNit" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtTelefonoProveedor">Teléfono</label>
                    <input type="text" id="txtTelefonoProveedor" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtEmailProveedor">Email</label>
                    <input type="email" id="txtEmailProveedor" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtDireccionProveedor">Dirección</label>
                    <input type="text" id="txtDireccionProveedor" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtCiudadProveedor">Ciudad</label>
                    <input type="text" id="txtCiudadProveedor" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtPaisProveedor">País</label>
                    <input type="text" id="txtPaisProveedor" class="a-input no-icon" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModalProveedor">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarProveedor">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-proveedores.js"></script>
End Section