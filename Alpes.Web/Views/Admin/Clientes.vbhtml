@Code
    ViewData("Title") = "Clientes"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page clientes-page">
    <div class="clientes-mobile-header">
        <div>
            <h2 class="clientes-title">
                <span data-i18n="clientes">Clientes</span>
                <span id="countClientes" class="clientes-count">0</span>
            </h2>
        </div>

        <button type="button" class="clientes-fab" id="btnNuevoCliente">
            <i class="bi bi-person-plus-fill"></i>
            <span data-i18n="nuevoCliente">+ Nuevo cliente</span>
        </button>
    </div>

    <div id="alertClientes" class="clientes-alert" style="display:none;"></div>

    <div class="clientes-search">
        <i class="bi bi-search"></i>
        <input type="text"
               id="txtBuscarCliente"
               placeholder="Buscar por nombre, email, teléfono, país, departamento o ciudad..."
               data-i18n-placeholder="buscarCliente" />
    </div>

    <div class="clientes-total-row">
        <span id="clientesTotalTexto">0 clientes</span>
    </div>

    <div id="clientesListado" class="clientes-listado">
        <div class="table-empty" data-i18n="cargandoClientes">Cargando clientes...</div>
    </div>
</div>

<div class="modal-a" id="modalCliente" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog">
        <div class="modal-a__header">
            <h3 id="modalClienteTitulo" data-i18n="nuevoClienteTitulo">Nuevo cliente</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <div class="a-form-group">
                    <label for="txtTipoDocumento" data-i18n="tipoDocumento">Tipo documento</label>
                    <input type="text" id="txtTipoDocumento" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtNumDocumento" data-i18n="numeroDocumento">No. documento</label>
                    <input type="text" id="txtNumDocumento" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtNombres" data-i18n="nombres">Nombres</label>
                    <input type="text" id="txtNombres" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtApellidos" data-i18n="apellidos">Apellidos</label>
                    <input type="text" id="txtApellidos" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtEmail" data-i18n="email">Email</label>
                    <input type="email" id="txtEmail" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtTelResidencia" data-i18n="telResidencia">Tel. residencia</label>
                    <input type="text" id="txtTelResidencia" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtTelCelular" data-i18n="telCelular">Tel. celular</label>
                    <input type="text" id="txtTelCelular" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtDireccion" data-i18n="direccion">Dirección</label>
                    <input type="text" id="txtDireccion" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtCiudad" data-i18n="ciudad">Ciudad</label>
                    <input type="text" id="txtCiudad" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtDepartamento" data-i18n="departamento">Departamento</label>
                    <input type="text" id="txtDepartamento" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtPais" data-i18n="pais">País</label>
                    <input type="text" id="txtPais" class="a-input no-icon" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModal" data-i18n="cancelar">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarCliente" data-i18n="guardar">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/admin-clientes.js")?v=@DateTime.Now.Ticks"></script>
End Section