@Code
    ViewData("Title") = "Soporte"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<section class="pc-section pc-support-page">
    <div class="pc-hero pc-hero-compact">
        <div>
            <span class="pc-eyebrow">Atención al cliente</span>
            <h1>Soporte</h1>
            <p>Resuelve dudas sobre pedidos, envíos, pagos, dirección de entrega y promociones.</p>
        </div>
        <div class="pc-hero-actions">
            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="pc-btn pc-btn-gold">
                <i class="bi bi-receipt"></i>
                Mis pedidos
            </a>
            <a href="@Url.Action("Tracking", "PortalCliente")" class="pc-btn pc-btn-outline-light">
                <i class="bi bi-truck"></i>
                Tracking
            </a>
        </div>
    </div>

    <div class="pc-support-grid">
        <div class="pc-card pc-support-chat-card">
            <div class="pc-card-header">
                <div>
                    <h2>Chat de ayuda</h2>
                    <p>Asistente rápido del panel cliente.</p>
                </div>
                <span class="pc-status-pill pc-status-entregado">
                    <i class="bi bi-circle-fill"></i>
                    En línea
                </span>
            </div>

            <div class="pc-support-hours">
                <i class="bi bi-clock"></i>
                <span>Horario de atención: lunes a viernes de 8:00 AM a 6:00 PM</span>
            </div>

            <div class="pc-support-quick-actions">
                <button type="button" class="pc-chip-btn" data-message="Quiero consultar el estado de mi pedido">Estado de mi pedido</button>
                <button type="button" class="pc-chip-btn" data-message="Tengo problemas con mi pago">Problemas con pago</button>
                <button type="button" class="pc-chip-btn" data-message="Cuánto tarda la entrega">Tiempo de entrega</button>
                <button type="button" class="pc-chip-btn" data-message="Tengo dudas sobre cupones y descuentos">Cupones</button>
            </div>

            <div class="pc-support-chat" id="soporteChat">
                <div class="pc-support-message bot">
                    <div class="pc-support-bubble">Hola, bienvenido a Muebles de los Alpes. ¿En qué podemos ayudarte hoy?</div>
                    <span>Ahora</span>
                </div>
                <div class="pc-support-message bot">
                    <div class="pc-support-bubble">Puedes preguntarme por pedidos, envíos, pagos, direcciones o descuentos.</div>
                    <span>Ahora</span>
                </div>
            </div>

            <div class="pc-support-inputbar">
                <input type="text" id="soporteInput" class="pc-input" placeholder="Escribe tu mensaje..." autocomplete="off" />
                <button type="button" id="soporteEnviar" class="pc-btn pc-btn-primary">
                    <i class="bi bi-send-fill"></i>
                </button>
            </div>
        </div>

        <aside class="pc-card pc-support-side">
            <div class="pc-card-header">
                <div>
                    <h2>Accesos de ayuda</h2>
                    <p>Opciones rápidas de tu cuenta.</p>
                </div>
            </div>

            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="pc-support-link">
                <span><i class="bi bi-receipt"></i> Revisar mis pedidos</span>
                <i class="bi bi-chevron-right"></i>
            </a>
            <a href="@Url.Action("Carrito", "PortalCliente")" class="pc-support-link">
                <span><i class="bi bi-cart3"></i> Revisar mi carrito</span>
                <i class="bi bi-chevron-right"></i>
            </a>
            <a href="@Url.Action("MiPerfil", "PortalCliente")" class="pc-support-link">
                <span><i class="bi bi-geo-alt"></i> Cambiar dirección</span>
                <i class="bi bi-chevron-right"></i>
            </a>
            <a href="@Url.Action("MisTarjetas", "PortalCliente")" class="pc-support-link">
                <span><i class="bi bi-credit-card"></i> Métodos de pago</span>
                <i class="bi bi-chevron-right"></i>
            </a>

            <div class="pc-support-note">
                <i class="bi bi-shield-check"></i>
                <div>
                    <strong>Compra protegida</strong>
                    <p>Tus pedidos se consultan desde tu sesión activa y la base de datos.</p>
                </div>
            </div>
        </aside>
    </div>
</section>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-soporte.js?v=16")"></script>
End Section
