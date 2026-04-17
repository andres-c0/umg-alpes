@Code
    ViewData("Title") = "Inventario"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Inventario</h2>
            <div class="admin-page__subtitle">Gestión de inventario por producto</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevoInventario">
            <i class="bi bi-plus-lg"></i>
            Nuevo inventario
        </button>
    </div>

    <div class="dashboard-grid" style="margin-bottom:18px;">
        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-boxes"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiInventarioTotal">-</div>
                <div class="dashboard-kpi__label">Registros</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-exclamation-triangle"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiStockBajo">-</div>
                <div class="dashboard-kpi__label">Stock bajo</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-shield-exclamation"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiSobreReservado">-</div>
                <div class="dashboard-kpi__label">Sobre reservado</div>
            </div>
        </div>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <select id="filtroEstadoInventario" class="a-input" style="max-width:220px;">
                <option value="">Todos los estados</option>
                <option value="normal">Normal</option>
                <option value="stock-bajo">Stock bajo</option>
                <option value="sobre-reservado">Sobre reservado</option>
            </select>
            <input type="text" id="txtBuscarInventario" class="a-input" placeholder="Buscar por producto..." />
            <button type="button" class="btn-a" id="btnRecargarInventario">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div class="table-wrap">
            <table class="admin-table" id="tablaInventario">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Producto</th>
                        <th>Stock</th>
                        <th>Reservado</th>
                        <th>Mínimo</th>
                        <th>Estado inventario</th>
                        <th style="width:140px;">Acciones</th>
                    </tr>
                </thead>
                <tbody id="inventarioBody">
                    <tr>
                        <td colspan="7" class="table-empty">Cargando inventario...</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>

<div class="modal-a" id="modalInventario" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog">
        <div class="modal-a__header">
            <h3 id="modalInventarioTitulo">Nuevo inventario</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalInventario">×</button>
        </div>

        <div class="modal-a__body">
            <input type="hidden" id="InvProdId" value="0" />

            <div class="form-grid">
                <div class="a-form-group form-grid__full">
                    <label for="ProductoId">Producto</label>
                    <select id="ProductoId" class="a-input">
                        <option value="">Seleccione...</option>
                    </select>
                </div>

                <div class="a-form-group">
                    <label for="Stock">Stock</label>
                    <input type="number" id="Stock" class="a-input" min="0" />
                </div>

                <div class="a-form-group">
                    <label for="StockReservado">Stock reservado</label>
                    <input type="number" id="StockReservado" class="a-input" min="0" />
                </div>

                <div class="a-form-group">
                    <label for="StockMinimo">Stock mínimo</label>
                    <input type="number" id="StockMinimo" class="a-input" min="0" />
                </div>
            </div>

            <div class="a-alert error" id="inventarioError" style="display:none;">
                <i class="bi bi-exclamation-circle"></i>
                <span id="inventarioErrorTexto"></span>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCancelarInventario">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarInventario">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-inventario.js"></script>
End Section