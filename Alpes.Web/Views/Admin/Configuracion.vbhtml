@Code
    ViewData("Title") = "Configuración"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page config-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Configuración</h2>
            <div class="admin-page__subtitle">Preferencias del sistema y administración general</div>
        </div>
    </div>

    <div class="config-section">
        <div class="config-section__title">Mi perfil</div>

        <div class="admin-card config-profile">
            <div class="config-profile__avatar" id="configProfileAvatar">A</div>

            <div class="config-profile__body">
                <div class="config-profile__name" id="configProfileName">admin1</div>
                <div class="config-profile__email" id="configProfileEmail">admin@alpes.com</div>
                <div class="config-profile__role" id="configProfileRole">Administrador</div>
            </div>
        </div>
    </div>

    <div class="config-section">
        <div class="config-section__title">Notificaciones</div>

        <div class="admin-card config-card">
            <div class="config-setting">
                <div class="config-setting__icon"><i class="bi bi-receipt"></i></div>
                <div class="config-setting__body">
                    <div class="config-setting__name">Alertas de ventas</div>
                    <div class="config-setting__desc">Notificar al registrar una venta</div>
                </div>
                <label class="switch-a">
                    <input type="checkbox" id="cfgVentas" checked="checked" />
                    <span class="switch-a__slider"></span>
                </label>
            </div>

            <div class="config-setting">
                <div class="config-setting__icon"><i class="bi bi-archive"></i></div>
                <div class="config-setting__body">
                    <div class="config-setting__name">Stock bajo</div>
                    <div class="config-setting__desc">Alertar cuando el inventario sea bajo</div>
                </div>
                <label class="switch-a">
                    <input type="checkbox" id="cfgStock" checked="checked" />
                    <span class="switch-a__slider"></span>
                </label>
            </div>

            <div class="config-setting config-setting--last">
                <div class="config-setting__icon"><i class="bi bi-bag"></i></div>
                <div class="config-setting__body">
                    <div class="config-setting__name">Nuevos pedidos</div>
                    <div class="config-setting__desc">Notificar al recibir un pedido</div>
                </div>
                <label class="switch-a">
                    <input type="checkbox" id="cfgPedidos" />
                    <span class="switch-a__slider"></span>
                </label>
            </div>
        </div>
    </div>

    <div class="config-section">
        <div class="config-section__title">Módulos del sistema</div>

        <div class="admin-card config-card">
            <a href="javascript:void(0)" class="config-link-row">
                <div class="config-link-row__left">
                    <div class="config-link-row__icon"><i class="bi bi-people"></i></div>
                    <span>Gestión de roles</span>
                </div>
                <i class="bi bi-chevron-right"></i>
            </a>

            <a href="javascript:void(0)" class="config-link-row">
                <div class="config-link-row__left">
                    <div class="config-link-row__icon"><i class="bi bi-truck"></i></div>
                    <span>Zonas de envío</span>
                </div>
                <i class="bi bi-chevron-right"></i>
            </a>

            <a href="@Url.Action("Reportes", "Admin")" class="config-link-row">
                <div class="config-link-row__left">
                    <div class="config-link-row__icon"><i class="bi bi-bar-chart"></i></div>
                    <span>Ver reportes</span>
                </div>
                <i class="bi bi-chevron-right"></i>
            </a>

            <a href="@Url.Action("Marketing", "Admin")" class="config-link-row config-link-row--last">
                <div class="config-link-row__left">
                    <div class="config-link-row__icon"><i class="bi bi-megaphone"></i></div>
                    <span>Campañas de marketing</span>
                </div>
                <i class="bi bi-chevron-right"></i>
            </a>
        </div>
    </div>

    <div class="config-section">
        <div class="config-section__header">
            <div class="config-section__title">Usuarios del sistema</div>

            <button type="button" class="btn-a btn-a-link" id="btnNuevoUsuarioConfig">
                <i class="bi bi-plus-lg"></i>
                Agregar
            </button>
        </div>

        <div class="admin-card config-card" id="configUsuariosListado">
            <div class="table-empty">Cargando usuarios...</div>
        </div>
    </div>

    <a href="@Url.Action("Logout", "Home")" class="config-logout">
        <i class="bi bi-box-arrow-right"></i>
        <span>Cerrar sesión</span>
    </a>
</div>

<div class="modal-a" id="modalUsuarioConfig" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog modal-a__dialog--config">
        <div class="modal-a__header">
            <h3 id="modalUsuarioConfigTitulo">Nuevo usuario</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalUsuarioConfigX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <input type="hidden" id="hidUsuarioConfigId" value="0" />

                <div class="a-form-group">
                    <label for="txtUsuarioConfigUsername">Username</label>
                    <input type="text" id="txtUsuarioConfigUsername" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtUsuarioConfigEmail">Email</label>
                    <input type="email" id="txtUsuarioConfigEmail" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="selUsuarioConfigRol">Rol</label>
                    <select id="selUsuarioConfigRol" class="a-input no-icon">
                        <option value="Administrador">Administrador</option>
                        <option value="Cliente">Cliente</option>
                        <option value="Empleado">Empleado</option>
                    </select>
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCancelarModalUsuarioConfig">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarUsuarioConfig">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-configuracion.js?v=1"></script>
End Section