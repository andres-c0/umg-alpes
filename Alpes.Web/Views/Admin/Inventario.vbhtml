@Code
    ViewData("Title") = "Inventario"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page inventario-page">

    <div class="inventario-hero admin-card">
        <div class="inventario-hero__icon">
            <i class="bi bi-box-seam-fill"></i>
        </div>

        <div class="inventario-hero__body">
            <div class="inventario-hero__title">Inventario de productos</div>
            <div class="inventario-hero__subtitle">
                Visualiza stock, reserva y detalle del producto en un solo lugar.
            </div>

            <div class="inventario-hero__meta">
                <i class="bi bi-stars"></i>
                <span id="inventarioVisibleCount">0 registros visibles</span>
            </div>
        </div>
    </div>

    <div class="inventario-searchbar">
        <i class="bi bi-search"></i>
        <input type="text"
               id="txtBuscarInventario"
               placeholder="Buscar por nombre, referencia, ID, color, tipo o descripción" />
    </div>

    <div class="inventario-kpis">
        <div class="inventario-kpi inventario-kpi--neutral">
            <div class="inventario-kpi__icon">
                <i class="bi bi-box-seam-fill"></i>
            </div>
            <div class="inventario-kpi__body">
                <div class="inventario-kpi__label">Productos</div>
                <div class="inventario-kpi__value" id="kpiInventarioProductos">0</div>
            </div>
        </div>

        <div class="inventario-kpi inventario-kpi--success">
            <div class="inventario-kpi__icon">
                <i class="bi bi-boxes"></i>
            </div>
            <div class="inventario-kpi__body">
                <div class="inventario-kpi__label">Stock actual</div>
                <div class="inventario-kpi__value" id="kpiInventarioStock">0</div>
            </div>
        </div>

        <div class="inventario-kpi inventario-kpi--warning">
            <div class="inventario-kpi__icon">
                <i class="bi bi-lock-fill"></i>
            </div>
            <div class="inventario-kpi__body">
                <div class="inventario-kpi__label">Reservado</div>
                <div class="inventario-kpi__value" id="kpiInventarioReservado">0</div>
            </div>
        </div>

        <div class="inventario-kpi inventario-kpi--danger">
            <div class="inventario-kpi__icon">
                <i class="bi bi-exclamation-triangle"></i>
            </div>
            <div class="inventario-kpi__body">
                <div class="inventario-kpi__label">Bajo mínimo</div>
                <div class="inventario-kpi__value" id="kpiInventarioBajoMinimo">0</div>
            </div>
        </div>
    </div>

    <div id="inventarioListado" class="inventario-listado">
        <div class="table-empty">Cargando inventario...</div>
    </div>

    <button type="button" class="inventario-fab" id="btnNuevoInventario">
        <i class="bi bi-plus-lg"></i>
        <span>Nuevo</span>
    </button>
</div>

<div class="modal-a" id="modalInventario" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog modal-a__dialog--inventario modal-a__dialog--inventario-edit">
        <div class="modal-sheet-handle"></div>

        <div class="modal-a__header modal-a__header--inventory">
            <h3 id="modalInventarioTitulo">Editar inventario</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalInventarioX">×</button>
        </div>

        <div class="modal-a__body modal-a__body--inventory">
            <input type="hidden" id="hidInventarioId" value="0" />
            <input type="hidden" id="txtProductoIdInventario" value="0" />

            <div class="inventario-edit-summary">
                <div class="inventario-edit-summary__title" id="invEditNombre">Producto</div>
                <div class="inventario-edit-summary__ref" id="invEditReferencia">Ref: --</div>

                <div class="inventario-edit-summary__chips">
                    <span class="inventario-chip inventario-chip--soft" id="invEditTipo">GENERAL</span>
                    <span class="inventario-chip inventario-chip--soft" id="invEditColor">Sin color</span>
                    <span class="inventario-chip inventario-chip--soft" id="invEditDisponible">Disponible 0</span>
                </div>

                <div class="inventario-edit-summary__desc" id="invEditDescripcion">
                    Sin descripción disponible.
                </div>
            </div>

            <div class="inventario-form">
                <div class="inventario-form__group">
                    <label for="txtStockInventario">Stock</label>
                    <input type="number" id="txtStockInventario" class="a-input no-icon" min="0" />
                </div>

                <div class="inventario-form__group">
                    <label for="txtReservadoInventario">Stock Reservado</label>
                    <input type="number" id="txtReservadoInventario" class="a-input no-icon" min="0" />
                </div>

                <div class="inventario-form__group">
                    <label for="txtStockMinimoInventario">Stock Mínimo</label>
                    <input type="number" id="txtStockMinimoInventario" class="a-input no-icon" min="0" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer modal-a__footer--inventory">
            <button type="button" class="btn-a btn-a-primary btn-a-full" id="btnGuardarInventario">
                GUARDAR
            </button>
        </div>
    </div>
</div>




@section scripts
    <script src="~/Scripts/admin-inventario.js?v=13"></script>s
End Section