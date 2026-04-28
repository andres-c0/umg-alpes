@Code
    ViewData("Title") = "Historial"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"
End Code

<div class="ph-page">
    <div class="ph-top">
        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="ph-back">
            <i class="bi bi-chevron-left"></i>
        </a>

        <div class="ph-top-center">
            <div class="ph-title">Historial</div>
            <div class="ph-subtitle">0 pedidos</div>
        </div>

        <button type="button" class="ph-refresh">
            <i class="bi bi-arrow-clockwise"></i>
        </button>
    </div>

    <div class="ph-tabs">
        <button type="button" class="ph-tab active">Todos</button>
        <button type="button" class="ph-tab">Pendiente</button>
        <button type="button" class="ph-tab">En proceso</button>
        <button type="button" class="ph-tab">Entregado</button>
        <button type="button" class="ph-tab">Cancelado</button>
    </div>

    <div class="ph-empty">
        <div class="ph-empty-icon">
            <i class="bi bi-receipt"></i>
        </div>

        <div class="ph-empty-title">Sin órdenes</div>
        <div class="ph-empty-text">Aún no has realizado ningún pedido</div>

        <a href="#" class="ph-catalog-btn">
            <i class="bi bi-grid"></i>
            <span>Ir al catálogo</span>
        </a>
    </div>

    <div class="ph-bottom-nav">
        <a href="@Url.Action("Index", "PortalCliente")" class="ph-nav-item">
            <i class="bi bi-house"></i>
            <span>Inicio</span>
        </a>

        <a href="#" class="ph-nav-item">
            <i class="bi bi-grid"></i>
            <span>Catálogo</span>
        </a>

        <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="ph-nav-item">
            <i class="bi bi-heart"></i>
            <span>Favoritos</span>
        </a>

        <a href="@Url.Action("Historial", "PortalCliente")" class="ph-nav-item active">
            <i class="bi bi-clock-history"></i>
            <span>Historial</span>
        </a>

        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="ph-nav-item">
            <i class="bi bi-person"></i>
            <span>Perfil</span>
        </a>
    </div>
</div>