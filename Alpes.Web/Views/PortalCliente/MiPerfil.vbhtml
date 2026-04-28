@Code
    ViewData("Title") = "Mi Perfil"
    Layout = "~/Views/Shared/_PortalClientePerfilLayout.vbhtml"

    Dim cliIdTexto As String = System.Convert.ToString(ViewData("CliId"))
    Dim avatar As String = "C"
End Code

<div class="np-page" data-cli-id="@cliIdTexto">

    <div class="np-header">
        <div class="np-avatar" id="npAvatar">@avatar</div>
        <div class="np-name">Cargando perfil...</div>
        <div class="np-email">Cargando datos...</div>
    </div>

    <div class="np-section-title">Mi cuenta</div>

    <div class="np-card">
        <a href="@Url.Action("MisOrdenes","PortalCliente")" class="np-item">
            <i class="bi bi-bag"></i>
            <div>
                <strong>Mis pedidos</strong>
                <small>Ver historial de compras</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </a>

        <a href="@Url.Action("MisFavoritos","PortalCliente")" class="np-item">
            <i class="bi bi-heart"></i>
            <div>
                <strong>Mis favoritos</strong>
                <small>Productos guardados</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </a>

        <a href="@Url.Action("Tracking","PortalCliente")" class="np-item">
            <i class="bi bi-geo-alt"></i>
            <div>
                <strong>Seguimiento</strong>
                <small>Consultar estado de envíos</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </a>
    </div>

    <div class="np-section-title">Preferencias</div>

    <div class="np-card">
        <a href="@Url.Action("Notificaciones", "PortalCliente")" class="np-item">
            <i class="bi bi-bell"></i>
            <div>
                <strong>Notificaciones</strong>
                <small>Alertas y avisos</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </a>

        <a href="@Url.Action("Soporte", "PortalCliente")" class="np-item">
            <i class="bi bi-question-circle"></i>
            <div>
                <strong>Ayuda y soporte</strong>
                <small>Preguntas frecuentes y asistencia</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </a>

        <a href="@Url.Action("Configuracion", "PortalCliente")" class="np-item">
            <i class="bi bi-info-circle"></i>
            <div>
                <strong>Configuración</strong>
                <small>Preferencias de la cuenta</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </a>
    </div>

    <div class="np-section-title">Configuración de cuenta</div>

    <div class="np-card">
        <button type="button" class="np-item np-item-btn" id="btnAbrirEditarPerfil">
            <i class="bi bi-person"></i>
            <div>
                <strong>Editar perfil</strong>
                <small>Actualizar datos del cliente</small>
            </div>
            <i class="bi bi-chevron-right"></i>
        </button>
    </div>

    <a href="@Url.Action("Logout","Home")" class="np-logout">
        <i class="bi bi-box-arrow-right"></i>
        Cerrar sesión
    </a>

    <div class="ep-overlay" id="epOverlay">
        <div class="ep-modal">
            <div class="ep-title">Editar perfil</div>

            <form id="epForm">
                <input type="hidden" id="epCliId" value="@cliIdTexto" />

                <div class="ep-grid">
                    <div class="ep-field">
                        <label>Nombre</label>
                        <input type="text" id="epNombre" value="" placeholder="Cargando..." />
                    </div>

                    <div class="ep-field">
                        <label>Apellido</label>
                        <input type="text" id="epApellido" value="" placeholder="Cargando..." />
                    </div>
                </div>

                <div class="ep-field">
                    <label>Email</label>
                    <input type="email" id="epEmail" value="" placeholder="Cargando..." />
                </div>

                <button type="submit" class="ep-save">GUARDAR</button>
            </form>

            <button type="button" class="ep-close" id="btnCerrarEditarPerfil">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>
    </div>
</div>

@section scripts
    <script src="@Url.Content("~/Scripts/portal-editar-perfil.js?v=2")"></script>
End Section