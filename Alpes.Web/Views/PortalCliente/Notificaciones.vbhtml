@Code
    ViewData("Title") = "Notificaciones"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<section class="pc-section pc-notifications-page">
    <div class="pc-hero pc-hero-compact">
        <div>
            <span class="pc-eyebrow">Centro de alertas</span>
            <h1>Notificaciones</h1>
            <p>Consulta avisos generados desde tus pedidos, carrito y seguimiento de compras.</p>
        </div>
        <div class="pc-hero-actions">
            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="pc-btn pc-btn-gold">
                <i class="bi bi-receipt"></i>
                Ver pedidos
            </a>
            <button type="button" class="pc-btn pc-btn-outline-light" id="btnMarcarNotificaciones">
                <i class="bi bi-check2-all"></i>
                Marcar todo
            </button>
        </div>
    </div>

    <div class="pc-notification-summary">
        <div class="pc-stat-card">
            <div class="pc-stat-number" id="notifTotal">0</div>
            <div class="pc-stat-label">Alertas</div>
            <i class="bi bi-bell"></i>
        </div>
        <div class="pc-stat-card">
            <div class="pc-stat-number" id="notifActivas">0</div>
            <div class="pc-stat-label">Activas</div>
            <i class="bi bi-truck"></i>
        </div>
        <div class="pc-stat-card">
            <div class="pc-stat-number" id="notifCarrito">0</div>
            <div class="pc-stat-label">Carrito</div>
            <i class="bi bi-cart3"></i>
        </div>
    </div>

    <div class="pc-card pc-notification-panel">
        <div class="pc-card-header">
            <div>
                <h2>Actividad reciente</h2>
                <p>Información consultada desde la base de datos.</p>
            </div>
            <button type="button" class="pc-btn pc-btn-outline" id="btnActualizarNotificaciones">
                <i class="bi bi-arrow-clockwise"></i>
                Actualizar
            </button>
        </div>

        <div class="pc-notification-list" id="notificacionesLista">
            <div class="pc-loading-card">Cargando notificaciones...</div>
        </div>
    </div>
</section>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-notificaciones.js?v=16")"></script>
End Section
