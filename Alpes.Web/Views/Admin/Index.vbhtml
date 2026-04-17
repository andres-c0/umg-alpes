@Code
    ViewData("Title") = "Dashboard"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Panel Administrativo</h2>
            <div class="admin-page__subtitle">Resumen general de gestión</div>
        </div>

        <a href="@Url.Action("Productos", "Admin")" class="btn-a btn-a-primary">
            <i class="bi bi-box-seam"></i>
            Ir a Productos
        </a>
    </div>

    <div class="dashboard-grid">
        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-box-seam"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiProductos">-</div>
                <div class="dashboard-kpi__label">Productos</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-tags"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiCategorias">-</div>
                <div class="dashboard-kpi__label">Categorías</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-rulers"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiUnidades">-</div>
                <div class="dashboard-kpi__label">Unidades de medida</div>
            </div>
        </div>
    </div>

    <div class="dashboard-panels">
        <div class="admin-card">
            <div class="panel-title">Accesos rápidos</div>
            <div class="quick-grid">
                <a class="quick-card" href="@Url.Action("Productos", "Admin")">
                    <i class="bi bi-box-seam"></i>
                    <span>Productos</span>
                </a>

                <a class="quick-card" href="@Url.Action("Inventario", "Admin")">
                    <i class="bi bi-boxes"></i>
                    <span>Inventario</span>
                </a>

                <a class="quick-card" href="@Url.Action("Ordenes", "Admin")">
                    <i class="bi bi-receipt"></i>
                    <span>Órdenes</span>
                </a>

                <a class="quick-card" href="@Url.Action("Clientes", "Admin")">
                    <i class="bi bi-people"></i>
                    <span>Clientes</span>
                </a>

                <a class="quick-card" href="@Url.Action("Reportes", "Admin")">
                    <i class="bi bi-bar-chart"></i>
                    <span>Reportes</span>
                </a>
            </div>
        </div>

        <div class="admin-card">
            <div class="panel-title">Últimos productos</div>
            <div id="dashboardProductos">
                <div class="table-empty">Cargando productos...</div>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script>
        $(function () {
            $.when(
                $.getJSON('/Producto/Index'),
                $.getJSON('/Categoria/Index'),
                $.getJSON('/Unidad_Medida/Index')
            ).done(function (p, c, u) {
                var productos = $.isArray(p[0]) ? p[0] : [];
                var categorias = $.isArray(c[0]) ? c[0] : [];
                var unidades = $.isArray(u[0]) ? u[0] : [];

                $('#kpiProductos').text(productos.length);
                $('#kpiCategorias').text(categorias.length);
                $('#kpiUnidades').text(unidades.length);

                if (!productos.length) {
                    $('#dashboardProductos').html('<div class="table-empty">No hay productos registrados.</div>');
                    return;
                }

                var top = productos.slice(0, 5);
                var html = '<table class="admin-table"><thead><tr><th>ID</th><th>Referencia</th><th>Nombre</th><th>Tipo</th></tr></thead><tbody>';
                top.forEach(function (x) {
                    html += '<tr>';
                    html += '<td>' + (x.ProductoId || '') + '</td>';
                    html += '<td>' + (x.Referencia || '') + '</td>';
                    html += '<td>' + (x.Nombre || '') + '</td>';
                    html += '<td>' + (x.Tipo || '') + '</td>';
                    html += '</tr>';
                });
                html += '</tbody></table>';

                $('#dashboardProductos').html(html);
            }).fail(function () {
                $('#dashboardProductos').html('<div class="table-empty">No fue posible cargar el resumen.</div>');
            });
        });
    </script>
End Section