@Code
    ViewData("Title") = "Mi perfil"
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
End Code

<section class="pc-section pc-profile-page" id="perfilClientePage">
    <div class="pc-hero pc-profile-hero">
        <div>
            <span class="pc-eyebrow">Cuenta cliente</span>
            <h1>Mi perfil</h1>
            <p>Administra tus datos personales, dirección y teléfonos para tus compras.</p>
        </div>
        <button type="button" class="pc-btn pc-btn-gold" id="btnEditarPerfil">
            <i class="bi bi-pencil-square"></i>
            Editar perfil
        </button>
    </div>

    <div class="pc-profile-grid">
        <article class="pc-card pc-profile-card">
            <div class="pc-profile-avatar" id="perfilAvatar">C</div>
            <h2 id="perfilNombre">Cargando perfil...</h2>
            <p id="perfilEmail">Consultando información desde la base de datos.</p>
            <span class="pc-status-pill pc-status-active" id="perfilEstado">ACTIVO</span>
        </article>

        <article class="pc-card pc-profile-info">
            <div class="pc-card-header">
                <div>
                    <span class="pc-eyebrow">Datos personales</span>
                    <h2>Información del cliente</h2>
                </div>
            </div>

            <div class="pc-info-grid">
                <div class="pc-info-item">
                    <span>Nombres</span>
                    <strong id="infoNombres">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>Apellidos</span>
                    <strong id="infoApellidos">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>Email</span>
                    <strong id="infoEmail">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>Teléfono residencial</span>
                    <strong id="infoTelResidencia">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>Teléfono celular</span>
                    <strong id="infoTelCelular">-</strong>
                </div>
                <div class="pc-info-item pc-info-item-wide">
                    <span>Dirección</span>
                    <strong id="infoDireccion">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>Ciudad</span>
                    <strong id="infoCiudad">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>Departamento</span>
                    <strong id="infoDepartamento">-</strong>
                </div>
                <div class="pc-info-item">
                    <span>País</span>
                    <strong id="infoPais">-</strong>
                </div>
            </div>
        </article>
    </div>

    <div class="pc-card pc-profile-menu-card">
        <div class="pc-card-header">
            <div>
                <span class="pc-eyebrow">Accesos de cuenta</span>
                <h2>Opciones rápidas</h2>
            </div>
        </div>

        <div class="pc-menu-grid">
            <a href="@Url.Action("MisOrdenes", "PortalCliente")" class="pc-menu-tile">
                <i class="bi bi-receipt"></i>
                <strong>Mis pedidos</strong>
                <span>Historial y detalle</span>
            </a>
            <a href="@Url.Action("MisFavoritos", "PortalCliente")" class="pc-menu-tile">
                <i class="bi bi-heart"></i>
                <strong>Favoritos</strong>
                <span>Productos guardados</span>
            </a>
            <a href="@Url.Action("MisTarjetas", "PortalCliente")" class="pc-menu-tile">
                <i class="bi bi-credit-card"></i>
                <strong>Tarjetas</strong>
                <span>Métodos guardados</span>
            </a>
            <a href="@Url.Action("Soporte", "PortalCliente")" class="pc-menu-tile">
                <i class="bi bi-headset"></i>
                <strong>Soporte</strong>
                <span>Ayuda y preguntas</span>
            </a>
        </div>
    </div>
</section>

<div class="pc-modal-backdrop" id="perfilModal">
    <div class="pc-modal pc-profile-modal">
        <button type="button" class="pc-modal-close" id="btnCerrarPerfil" aria-label="Cerrar">
            <i class="bi bi-x-lg"></i>
        </button>
        <span class="pc-eyebrow">Editar perfil</span>
        <h2>Actualizar datos</h2>

        <form id="perfilForm" class="pc-form">
            <div class="pc-form-grid">
                <label>
                    <span>Nombres</span>
                    <input type="text" id="perfilInputNombres" required />
                </label>
                <label>
                    <span>Apellidos</span>
                    <input type="text" id="perfilInputApellidos" required />
                </label>
                <label>
                    <span>Email</span>
                    <input type="email" id="perfilInputEmail" required />
                </label>
                <label>
                    <span>Teléfono residencial</span>
                    <input type="text" id="perfilInputTelResidencia" />
                </label>
                <label>
                    <span>Teléfono celular</span>
                    <input type="text" id="perfilInputTelCelular" />
                </label>
                <label>
                    <span>Ciudad</span>
                    <input type="text" id="perfilInputCiudad" />
                </label>
                <label>
                    <span>Departamento</span>
                    <input type="text" id="perfilInputDepartamento" />
                </label>
                <label>
                    <span>País</span>
                    <input type="text" id="perfilInputPais" value="Guatemala" />
                </label>
                <label class="pc-form-wide">
                    <span>Dirección</span>
                    <textarea id="perfilInputDireccion" rows="3"></textarea>
                </label>
            </div>

            <button type="submit" class="pc-btn pc-btn-primary pc-btn-full" id="btnGuardarPerfil">
                Guardar cambios
            </button>
        </form>
    </div>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-editar-perfil.js?v=16")"></script>
End Section
