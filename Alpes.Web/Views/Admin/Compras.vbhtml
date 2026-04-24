@Code
    ViewData("Title") = "Compras"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Órdenes de compra</h2>
            <div class="admin-page__subtitle">Gestión de órdenes de compra registradas</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevaCompra">
            <i class="bi bi-plus-lg"></i>
            Nueva orden
        </button>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarCompra" class="a-input no-icon" placeholder="Buscar por número OC..." />
            <button type="button" class="btn-a" id="btnRecargarCompra">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="comprasListado">
            <div class="table-empty">Cargando órdenes de compra...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalCompra" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">
        <div class="modal-a__header">
            <h3 id="modalCompraTitulo">Nueva orden de compra</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalCompraX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <div class="a-form-group form-grid__full">
                    <label for="txtNumOc">No. OC</label>
                    <input type="text" id="txtNumOc" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="selProveedorCompra">Proveedor</label>
                    <select id="selProveedorCompra" class="a-input no-icon"></select>
                </div>

                <div class="a-form-group">
                    <label for="selEstadoOc">Estado orden compra</label>
                    <select id="selEstadoOc" class="a-input no-icon"></select>
                </div>

                <div class="a-form-group">
                    <label for="selCondicionPagoCompra">Condición de pago</label>
                    <select id="selCondicionPagoCompra" class="a-input no-icon"></select>
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtFechaOc">Fecha OC</label>
                    <input type="date" id="txtFechaOc" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtSubtotalCompra">Subtotal</label>
                    <input type="number" step="0.01" id="txtSubtotalCompra" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtImpuestoCompra">Impuesto</label>
                    <input type="number" step="0.01" id="txtImpuestoCompra" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtTotalCompra">Total</label>
                    <input type="number" step="0.01" id="txtTotalCompra" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtObservacionesCompra">Observaciones</label>
                    <textarea id="txtObservacionesCompra" class="a-input no-icon"></textarea>
                </div>
            </div>

            <div class="a-alert error" id="compraError" style="display:none; margin-top:16px;">
                <i class="bi bi-exclamation-circle"></i>
                <span id="compraErrorTexto"></span>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModalCompra">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarCompra">Guardar</button>
        </div>
    </div>
</div>



@section scripts
    <script src="~/Scripts/admin-compras.js"></script>
End Section