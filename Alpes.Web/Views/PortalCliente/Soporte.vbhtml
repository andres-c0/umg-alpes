@Code
    ViewData("Title") = "Soporte"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"
End Code

<div class="ps-page">
    <div class="ps-top">
        <a href="@Url.Action("MiPerfil", "PortalCliente")" class="ps-back">
            <i class="bi bi-chevron-left"></i>
        </a>

        <div class="ps-top-center">
            <div class="ps-title">Soporte al cliente</div>
            <div class="ps-subtitle">Agente disponible</div>
        </div>

        <div class="ps-empty"></div>
    </div>

    <div class="ps-hours">
        <i class="bi bi-clock"></i>
        <span>Atención: lunes a viernes de 8:00 AM a 6:00 PM</span>
    </div>

    <div class="ps-quick-actions">
        <button type="button" class="ps-quick-btn">Estado de mi pedido</button>
        <button type="button" class="ps-quick-btn">Problemas con pago</button>
        <button type="button" class="ps-quick-btn">Tiempo de entrega</button>
        <button type="button" class="ps-quick-btn">Cupones y descuentos</button>
    </div>

    <div class="ps-chat" id="psChat">
        <div class="ps-message ps-message--bot">
            <div class="ps-bubble">
                Hola, bienvenido al soporte de Muebles de los Alpes. ¿En qué podemos ayudarte hoy?
            </div>
            <div class="ps-time">09:30</div>
        </div>

        <div class="ps-message ps-message--bot">
            <div class="ps-bubble">
                Puedes preguntarme por pedidos, envíos, pagos, direcciones, devoluciones o descuentos.
            </div>
            <div class="ps-time">09:31</div>
        </div>
    </div>

    <div class="ps-inputbar">
        <button type="button" id="psEmojiBtn" class="ps-emoji-btn" title="Agregar texto rápido">+</button>
        <input type="text" id="psInput" class="ps-input" placeholder="Escribe tu mensaje..." />
        <button type="button" id="psSend" class="ps-send">
            <i class="bi bi-send-fill"></i>
        </button>
    </div>
</div>
<script src="@Url.Content("~/Scripts/portal-soporte.js?v=2")"></script>