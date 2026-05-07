@Code
    ViewData("Title") = "Órdenes"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page ordenes-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">
                Órdenes de Venta
            </h2>
            <div class="admin-page__subtitle">Gestión de órdenes de venta</div>
        </div>

      
    </div>
</div>
    <div class="admin-card ordenes-toolbar-card">
        <div class="admin-toolbar ordenes-toolbar">
            <input type="text"
                   id="txtBuscarOrden"
                   class="a-input no-icon"
                   placeholder="Buscar por número, cliente, pago u observación..." />

            <button type="button" class="btn-a btn-a-ghost" id="btnRecargarOrden">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div class="ordenes-filtros">
            <button class="orden-filter-pill active" data-estado="0" onclick="filtrarEstado(0)">
                Todos <span id="countTodos">0</span>
            </button>

            <button class="orden-filter-pill orden-filter-pill--pendiente" data-estado="30" onclick="filtrarEstado(30)">
                Pendiente <span id="countPendiente">0</span>
            </button>

            <button class="orden-filter-pill orden-filter-pill--proceso" data-estado="32" onclick="filtrarEstado(32)">
                En proceso <span id="countProceso">0</span>
            </button>

            <button class="orden-filter-pill orden-filter-pill--entregado" data-estado="34" onclick="filtrarEstado(34)">
                Entregado <span id="countEntregado">0</span>
            </button>

            <button class="orden-filter-pill orden-filter-pill--cancelado" data-estado="35" onclick="filtrarEstado(35)">
                Cancelado <span id="countCancelado">0</span>
            </button>
        </div>

    <div class="ordenes-resumen">
        <div class="ordenes-kpi ordenes-kpi--wide">
            <div class="ordenes-kpi__icon">
                <i class="bi bi-cash-stack"></i>
            </div>
            <div>
                <div class="ordenes-kpi__label">Ventas filtradas</div>
                <div class="ordenes-kpi__value" id="resumenVentasFiltradas">Q 0.00</div>
            </div>
        </div>

        <div class="ordenes-kpi">
            <div class="ordenes-kpi__icon ordenes-kpi__icon--warn">
                <i class="bi bi-hourglass-split"></i>
            </div>
            <div>
                <div class="ordenes-kpi__label">Pendientes</div>
                <div class="ordenes-kpi__value" id="resumenPendientes">0</div>
            </div>
        </div>

        <div class="ordenes-kpi">
            <div class="ordenes-kpi__icon ordenes-kpi__icon--box">
                <i class="bi bi-box-seam"></i>
            </div>
            <div>
                <div class="ordenes-kpi__label">Items visibles</div>
                <div class="ordenes-kpi__value" id="resumenItemsVisibles">0</div>
            </div>
        </div>

        <div class="ordenes-kpi">
            <div class="ordenes-kpi__icon ordenes-kpi__icon--danger">
                <i class="bi bi-x-circle"></i>
            </div>
            <div>
                <div class="ordenes-kpi__label">Canceladas</div>
                <div class="ordenes-kpi__value" id="resumenCanceladas">0</div>
            </div>
        </div>
    </div>

    <div class="ordenes-total-row">
        <span id="ordenTotalTexto">0 órdenes</span>
    </div>

    <div id="ordenContainer" class="ordenes-listado"></div>
</div>

<div class="modal-a" id="modalDetalleOrden" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog modal-a__dialog--orden-detalle">
        <div class="orden-detalle-page">

            <div class="orden-detalle-topbar">
                <button type="button" class="orden-detalle-back" id="btnCerrarDetalleOrden">
                    <i class="bi bi-chevron-left"></i>
                </button>

                <div class="orden-detalle-title" id="detalleOrdenTitulo">
                    Orden
                </div>
            </div>

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

            <section class="orden-detalle-hero">
                <div class="orden-detalle-hero__head">
                    <div>
                        <div class="orden-detalle-hero__number" id="detalleNumOrden">-</div>
                        <div class="orden-detalle-hero__date" id="detalleFechaOrden">-</div>
                    </div>

                    <div id="detalleEstadoOrden"></div>
                </div>

                <div class="orden-detalle-money">
                    <div class="orden-detalle-money__item">
                        <i class="bi bi-currency-dollar"></i>
                        <span>Subtotal</span>
                        <strong id="detalleSubtotalOrden">Q 0.00</strong>
                    </div>

                    <div class="orden-detalle-money__item">
                        <i class="bi bi-percent"></i>
                        <span>Impuesto</span>
                        <strong id="detalleImpuestoOrden">Q 0.00</strong>
                    </div>

                    <div class="orden-detalle-money__item">
                        <i class="bi bi-tag"></i>
                        <span>Descuento</span>
                        <strong id="detalleDescuentoOrden">Q 0.00</strong>
                    </div>

                    <div class="orden-detalle-money__item orden-detalle-money__item--total">
                        <i class="bi bi-wallet2"></i>
                        <span>Total</span>
                        <strong id="detalleTotalOrden">Q 0.00</strong>
                    </div>
                </div>

                <div class="orden-detalle-address">
                    <i class="bi bi-geo-alt-fill"></i>
                    <div>
                        <span>Dirección de envío</span>
                        <strong id="detalleDireccionOrden">Sin dirección</strong>
                    </div>
                </div>
            </section>

            <section class="orden-detalle-section">
                <div class="orden-detalle-section__title">
                    <i class="bi bi-activity"></i>
                    Gestión del estado
                </div>

                <div class="orden-detalle-section__subtitle">
                    Toca un estado para cambiar la orden
                </div>

                <div id="detalleTimelineOrden" class="orden-timeline"></div>

                <div class="orden-detalle-state-actions">
                    <select id="detalleNuevoEstado" class="a-input no-icon">
                        <option value="PENDIENTE">Pendiente</option>
                        <option value="CONFIRMADO">Confirmado</option>
                        <option value="EN_PROCESO">En proceso</option>
                        <option value="ENVIADO">Enviado</option>
                        <option value="ENTREGADO">Entregado</option>
                        <option value="CANCELADO">Cancelado</option>
                    </select>

                    <button type="button" class="btn-a btn-a-primary" id="btnActualizarEstadoOrden">
                        Guardar estado
                    </button>
                </div>
            </section>

            <section class="orden-detalle-products-section">
                <div class="orden-detalle-products-title">
                    <i class="bi bi-bag-fill"></i>
                    <span id="detalleProductosTitulo">Productos</span>
                </div>

                <div id="detalleOrdenBody" class="orden-detalle-products">
                    <div class="table-empty">Cargando detalle...</div>
                </div>
            </section>

            <div class="a-alert error" id="detalleOrdenError" style="display:none; margin-top:16px;">
                <i class="bi bi-exclamation-circle"></i>
                <span id="detalleOrdenErrorTexto"></span>
            </div>

            <button type="button" class="btn-a btn-a-ghost btn-a-full" id="btnCerrarDetalleOrdenFooter">
                Cerrar
            </button>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/admin-ordenes.js")?v=@DateTime.Now.Ticks"></script>
End Section