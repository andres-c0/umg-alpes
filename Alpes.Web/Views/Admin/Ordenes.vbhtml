@Code
    ViewData("Title") = "Órdenes"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Órdenes</h2>
            <div class="admin-page__subtitle">Gestión de órdenes de venta</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevaOrden">
            <i class="bi bi-plus-lg"></i>
            Nueva orden
        </button>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarOrden" class="a-input" placeholder="Buscar por número de orden..." />
            <button type="button" class="btn-a" id="btnRecargarOrden">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div class="table-wrap">
            <table class="admin-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Número</th>
                        <th>Cliente</th>
                        <th>Fecha</th>
                        <th>Total</th>
                        <th>Estado</th>
                        <th style="width:120px;">Acciones</th>
                    </tr>
                </thead>
                <tbody id="ordenBody">
                    <tr>
                        <td colspan="7" class="table-empty">Cargando órdenes...</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>

<div class="modal-a" id="modalDetalleOrden" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">
        <div class="modal-a__header">
            <h3 id="detalleOrdenTitulo">Detalle de orden</h3>
            <button type="button" class="modal-a__close" id="btnCerrarDetalleOrden">×</button>
        </div>

        <div class="modal-a__body">
            <div class="dashboard-grid" style="margin-bottom:18px;">
                <div class="dashboard-kpi">
                    <div class="dashboard-kpi__icon"><i class="bi bi-receipt"></i></div>
                    <div class="dashboard-kpi__body">
                        <div class="dashboard-kpi__value" id="detalleNumOrden">-</div>
                        <div class="dashboard-kpi__label">Número de orden</div>
                    </div>
                </div>

                <div class="dashboard-kpi">
                    <div class="dashboard-kpi__icon"><i class="bi bi-calendar-event"></i></div>
                    <div class="dashboard-kpi__body">
                        <div class="dashboard-kpi__value" id="detalleFechaOrden" style="font-size:20px;">-</div>
                        <div class="dashboard-kpi__label">Fecha</div>
                    </div>
                </div>

                <div class="dashboard-kpi">
                    <div class="dashboard-kpi__icon"><i class="bi bi-cash-stack"></i></div>
                    <div class="dashboard-kpi__body">
                        <div class="dashboard-kpi__value" id="detalleTotalOrden">Q 0.00</div>
                        <div class="dashboard-kpi__label">Total</div>
                    </div>
                </div>
            </div>

            <div class="admin-card" style="margin-bottom:18px;">
                <div class="panel-title">Información general</div>

                <input type="hidden" id="detalleOrdenId" value="0" />
                <input type="hidden" id="detalleOrdenCliId" value="0" />
                <input type="hidden" id="detalleOrdenNumOrden" value="" />
                <input type="hidden" id="detalleOrdenFecha" value="" />
                <input type="hidden" id="detalleOrdenSubtotal" value="0" />
                <input type="hidden" id="detalleOrdenDescuento" value="0" />
                <input type="hidden" id="detalleOrdenImpuesto" value="0" />
                <input type="hidden" id="detalleOrdenTotal" value="0" />
                <input type="hidden" id="detalleOrdenMoneda" value="" />
                <input type="hidden" id="detalleOrdenDireccionEnvioSnapshot" value="" />
                <input type="hidden" id="detalleOrdenObservaciones" value="" />
                <input type="hidden" id="detalleOrdenEstadoRegistro" value="" />

                <div class="form-grid">
                    <div class="a-form-group">
                        <label>Cliente</label>
                        <input type="text" id="detalleClienteOrden" class="a-input" readonly="readonly" />
                    </div>

                    <div class="a-form-group">
                        <label>Estado actual</label>
                        <div id="detalleEstadoOrden"></div>
                    </div>

                    <div class="a-form-group form-grid__full">
                        <label for="detalleNuevoEstado">Cambiar estado</label>
                        <select id="detalleNuevoEstado" class="a-input">
                            <option value="PENDIENTE">Pendiente</option>
                            <option value="COMPLETADA">Completada</option>
                        </select>
                    </div>
                </div>

                <div style="margin-top:16px;">
                    <button type="button" class="btn-a btn-a-primary" id="btnActualizarEstadoOrden">
                        Actualizar estado
                    </button>
                </div>
            </div>

            <div class="admin-card">
                <div class="panel-title">Productos de la orden</div>
                <div class="table-wrap">
                    <table class="admin-table">
                        <thead>
                            <tr>
                                <th>Producto</th>
                                <th>Cantidad</th>
                                <th>Precio unitario</th>
                                <th>Subtotal</th>
                            </tr>
                        </thead>
                        <tbody id="detalleOrdenBody">
                            <tr>
                                <td colspan="4" class="table-empty">Cargando detalle...</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="a-alert error" id="detalleOrdenError" style="display:none; margin-top:16px;">
                <i class="bi bi-exclamation-circle"></i>
                <span id="detalleOrdenErrorTexto"></span>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarDetalleOrdenFooter">Cerrar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-ordenes.js"></script>
End Section