@Code
    ViewData("Title") = "Notificaciones"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"
End Code

<div class="pn-page">
    <div class="pn-top">
        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pn-back">
            <i class="bi bi-chevron-left"></i>
        </a>

        <div class="pn-title">Notificaciones (1)</div>

        <a href="#" class="pn-markall">Marcar todo</a>
    </div>

    <div class="pn-list">
        <div class="pn-item pn-item--active">
            <div class="pn-icon">
                <i class="bi bi-truck"></i>
            </div>
            <div class="pn-content">
                <div class="pn-text">Tu pedido <strong>#ORD-0042</strong> está en camino</div>
                <div class="pn-time">Hace 2 horas</div>
            </div>
            <div class="pn-dot"></div>
        </div>

        <div class="pn-item">
            <div class="pn-icon">
                <i class="bi bi-check-circle"></i>
            </div>
            <div class="pn-content">
                <div class="pn-text">Pedido <strong>#ORD-0039</strong> entregado con éxito</div>
                <div class="pn-time">Ayer</div>
            </div>
        </div>

        <div class="pn-item">
            <div class="pn-icon">
                <i class="bi bi-tag"></i>
            </div>
            <div class="pn-content">
                <div class="pn-text">Tienes un cupón de descuento del 10%</div>
                <div class="pn-time">Hace 3 días</div>
            </div>
        </div>

        <div class="pn-item">
            <div class="pn-icon">
                <i class="bi bi-credit-card"></i>
            </div>
            <div class="pn-content">
                <div class="pn-text">Pago confirmado por Q1,890</div>
                <div class="pn-time">Hace 5 días</div>
            </div>
        </div>

        <div class="pn-item">
            <div class="pn-icon">
                <i class="bi bi-star"></i>
            </div>
            <div class="pn-content">
                <div class="pn-text">Califica tu último pedido</div>
                <div class="pn-time">Hace 7 días</div>
            </div>
        </div>
    </div>
</div>