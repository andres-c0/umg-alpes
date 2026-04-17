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

@section scripts
    <script src="~/Scripts/admin-ordenes.js"></script>
End Section