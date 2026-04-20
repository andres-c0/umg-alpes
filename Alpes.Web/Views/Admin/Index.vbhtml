@Code
    ViewData("Title") = "Dashboard"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Panel Administrativo</h2>
            <div class="admin-page__subtitle">Resumen general del sistema</div>
        </div>
    </div>

    <div class="dashboard-grid">
        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-cash-coin"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiVentas">Q 0.00</div>
                <div class="dashboard-kpi__label">Ventas del mes</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-bag-check"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiCompras">Q 0.00</div>
                <div class="dashboard-kpi__label">Compras del mes</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-people"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiClientes">0</div>
                <div class="dashboard-kpi__label">Clientes</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-receipt"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiOrdenes">0</div>
                <div class="dashboard-kpi__label">Órdenes activas</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-box-seam"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiStock">0</div>
                <div class="dashboard-kpi__label">Stock bajo</div>
            </div>
        </div>

        <div class="dashboard-kpi">
            <div class="dashboard-kpi__icon"><i class="bi bi-wallet2"></i></div>
            <div class="dashboard-kpi__body">
                <div class="dashboard-kpi__value" id="kpiNomina">0</div>
                <div class="dashboard-kpi__label">Nóminas pendientes</div>
            </div>
        </div>
    </div>

    <div class="dashboard-panels">
        <div class="admin-card">
            <div class="panel-title">Ventas por mes</div>
            <canvas id="chartVentas" height="120"></canvas>
        </div>

        <div class="admin-card">
            <div class="panel-title">Compras por mes</div>
            <canvas id="chartCompras" height="120"></canvas>
        </div>
    </div>

    <div class="dashboard-section">
        <div class="dashboard-section__title">Gestión comercial</div>

        <div class="dashboard-shortcuts">
            <a href="@Url.Action("Productos", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-box-seam"></i></div>
                <div class="dashboard-shortcut__label">Productos</div>
            </a>

            <a href="@Url.Action("Inventario", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-house-door"></i></div>
                <div class="dashboard-shortcut__label">Inventario</div>
            </a>

            <a href="@Url.Action("Ordenes", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-receipt"></i></div>
                <div class="dashboard-shortcut__label">Órdenes</div>
            </a>

            <a href="@Url.Action("Clientes", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-people"></i></div>
                <div class="dashboard-shortcut__label">Clientes</div>
            </a>

            <a href="#" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-megaphone"></i></div>
                <div class="dashboard-shortcut__label">Marketing</div>
            </a>

            <a href="@Url.Action("Reportes", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-bar-chart"></i></div>
                <div class="dashboard-shortcut__label">Reportes</div>
            </a>
        </div>
    </div>

    <div class="dashboard-section">
        <div class="dashboard-section__title">Gestión operativa</div>

        <div class="dashboard-shortcuts">
            <a href="@Url.Action("Empleados", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-briefcase-fill"></i></div>
                <div class="dashboard-shortcut__label">Empleados</div>
            </a>

            <a href="@Url.Action("Nomina", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-wallet2"></i></div>
                <div class="dashboard-shortcut__label">Nómina</div>
            </a>

            <a href="@Url.Action("Proveedores", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-truck"></i></div>
                <div class="dashboard-shortcut__label">Proveedores</div>
            </a>

            <a href="@Url.Action("Compras", "Admin")" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-bag"></i></div>
                <div class="dashboard-shortcut__label">Compras</div>
            </a>

            <a href="#" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-building"></i></div>
                <div class="dashboard-shortcut__label">Producción</div>
            </a>

            <a href="#" class="dashboard-shortcut">
                <div class="dashboard-shortcut__icon"><i class="bi bi-gear-fill"></i></div>
                <div class="dashboard-shortcut__label">Config.</div>
            </a>
        </div>
    </div>
</div>

@section scripts
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="~/Scripts/admin-dashboard.js"></script>
End Section