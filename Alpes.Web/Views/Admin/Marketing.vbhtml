@Code
    ViewData("Title") = "Marketing"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<style>
    .marketing-page {
        display: flex;
        flex-direction: column;
        gap: 18px;
    }

    .marketing-kpis {
        display: grid;
        grid-template-columns: repeat(2, minmax(0, 1fr));
        gap: 16px;
    }

    .marketing-kpi {
        display: flex;
        align-items: center;
        gap: 16px;
        min-height: 96px;
        padding: 22px 20px;
    }

    .marketing-kpi__icon {
        width: 52px;
        height: 52px;
        border-radius: 16px;
        background: #f2ede6;
        color: #7b5637;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 22px;
        flex-shrink: 0;
    }

    .marketing-kpi__icon--money {
        background: #f6efe3;
        color: #c28a2f;
    }

    .marketing-kpi__info {
        display: flex;
        flex-direction: column;
        gap: 4px;
    }

    .marketing-kpi__value {
        font-size: 30px;
        font-weight: 800;
        color: #2f1a0f;
        line-height: 1;
    }

    .marketing-kpi__label {
        color: #9a7a55;
        font-size: 14px;
    }

    .marketing-toolbar {
        display: grid;
        grid-template-columns: minmax(0, 1fr) 320px auto;
        gap: 14px;
        margin-bottom: 18px;
        align-items: center;
    }

    .marketing-listado {
        display: flex;
        flex-direction: column;
        gap: 14px;
    }

    .marketing-card {
        display: grid;
        grid-template-columns: 58px minmax(0, 1fr) 58px;
        gap: 16px;
        align-items: center;
        border: 1px solid #eadfce;
        border-radius: 20px;
        background: #fff;
        padding: 18px 18px;
        box-shadow: 0 8px 18px rgba(61, 25, 8, 0.04);
    }

    .marketing-card__avatar {
        width: 56px;
        height: 56px;
        border-radius: 16px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 22px;
        font-weight: 800;
        flex-shrink: 0;
    }

    .marketing-card__content {
        min-width: 0;
    }

    .marketing-card__title {
        font-size: 18px;
        font-weight: 700;
        color: #2e1c12;
        margin-bottom: 8px;
        line-height: 1.2;
    }

    .marketing-card__tag {
        display: inline-flex;
        align-items: center;
        padding: 4px 10px;
        min-height: 26px;
        border-radius: 999px;
        font-size: 12px;
        font-weight: 700;
        margin-bottom: 10px;
    }

    .marketing-card__meta {
        display: flex;
        flex-wrap: wrap;
        gap: 14px;
        font-size: 14px;
        color: #a07b51;
    }

        .marketing-card__meta span {
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

    .marketing-card__actions {
        display: flex;
        flex-direction: column;
        gap: 8px;
        align-items: flex-end;
    }

    .btn-icon {
        width: 38px;
        height: 38px;
        border: 1px solid #e4d3bf;
        background: #fff;
        color: #6a4027;
        border-radius: 12px;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        transition: all .18s ease;
    }

        .btn-icon:hover {
            background: #f8f2eb;
        }

    .btn-icon-danger {
        color: #c04848;
    }

    .modal-a__dialog--marketing {
        width: min(760px, calc(100vw - 32px));
    }

    .marketing-preview {
        margin-top: 18px;
        display: flex;
        align-items: center;
        gap: 14px;
        border-radius: 18px;
        padding: 16px;
        border: 1px solid #eee3d6;
        background: #fcfaf7;
    }

    .marketing-preview__avatar {
        width: 52px;
        height: 52px;
        border-radius: 16px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 24px;
        font-weight: 800;
        flex-shrink: 0;
    }

    .marketing-preview__body {
        min-width: 0;
    }

    .marketing-preview__title {
        font-size: 18px;
        font-weight: 700;
        color: #2f1a0f;
        margin-bottom: 8px;
    }

    .marketing-preview__tag {
        display: inline-flex;
        align-items: center;
        min-height: 26px;
        padding: 4px 10px;
        border-radius: 999px;
        font-size: 13px;
        font-weight: 700;
        margin-bottom: 8px;
    }

    .marketing-preview__meta {
        color: #9a7a55;
        font-size: 14px;
    }

    .marketing-card--instagram {
        border-color: #efd8e7;
        background: linear-gradient(180deg, #fff 0%, #fff8fc 100%);
    }

    .marketing-tag--instagram {
        background: #f8d8ea;
        color: #ca3b86;
    }

    .marketing-avatar--instagram {
        background: #f1dfe9;
        color: #ca3b86;
    }

    .marketing-card--facebook {
        border-color: #d7e5f6;
        background: linear-gradient(180deg, #fff 0%, #f8fbff 100%);
    }

    .marketing-tag--facebook {
        background: #d9e8fb;
        color: #2472e3;
    }

    .marketing-avatar--facebook {
        background: #dce8f5;
        color: #2472e3;
    }

    .marketing-card--email {
        border-color: #dbe7dc;
        background: linear-gradient(180deg, #fff 0%, #f9fdf9 100%);
    }

    .marketing-tag--email {
        background: #dbe9db;
        color: #2f8c46;
    }

    .marketing-avatar--email {
        background: #dde8de;
        color: #2f8c46;
    }

    .marketing-card--tv {
        border-color: #eadfce;
        background: linear-gradient(180deg, #fff 0%, #fffcf7 100%);
    }

    .marketing-tag--tv {
        background: #efe3d2;
        color: #bf7c15;
    }

    .marketing-avatar--tv {
        background: #ece2d5;
        color: #bf7c15;
    }

    .marketing-card--social {
        border-color: #d9e6f1;
        background: linear-gradient(180deg, #fff 0%, #f8fbfd 100%);
    }

    .marketing-tag--social {
        background: #dce8f3;
        color: #2c68b7;
    }

    .marketing-avatar--social {
        background: #dbe6f0;
        color: #2c68b7;
    }

    .marketing-card--default {
        border-color: #eadfce;
        background: #fff;
    }

    .marketing-tag--default {
        background: #f2ede6;
        color: #8c6741;
    }

    .marketing-avatar--default {
        background: #f2ede6;
        color: #8c6741;
    }

    @@media (max-width: 900px) {
        .marketing-kpis {
            grid-template-columns: 1fr;
        }

        .marketing-toolbar {
            grid-template-columns: 1fr;
        }

        .marketing-card {
            grid-template-columns: 56px 1fr;
        }

        .marketing-card__actions {
            grid-column: 1 / -1;
            flex-direction: row;
            justify-content: flex-end;
        }
    }
</style>

<div class="admin-page marketing-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Marketing</h2>
            <div class="admin-page__subtitle">Gestión de campañas y presupuesto por canal</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevaCampana">
            <i class="bi bi-plus-lg"></i>
            Nueva campaña
        </button>
    </div>

    <div class="marketing-kpis">
        <div class="admin-card marketing-kpi">
            <div class="marketing-kpi__icon">
                <i class="bi bi-megaphone"></i>
            </div>
            <div class="marketing-kpi__info">
                <div class="marketing-kpi__value" id="kpiCampanas">0</div>
                <div class="marketing-kpi__label">Campañas</div>
            </div>
        </div>

        <div class="admin-card marketing-kpi">
            <div class="marketing-kpi__icon marketing-kpi__icon--money">
                <i class="bi bi-cash-coin"></i>
            </div>
            <div class="marketing-kpi__info">
                <div class="marketing-kpi__value" id="kpiPresupuesto">Q 0.00</div>
                <div class="marketing-kpi__label">Presupuesto total</div>
            </div>
        </div>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar marketing-toolbar">
            <input type="text" id="txtBuscarCampana" class="a-input no-icon" placeholder="Buscar campaña o canal..." />

            <select id="selFiltroCanal" class="a-input no-icon">
                <option value="">Todos los canales</option>
                <option value="Instagram">Instagram</option>
                <option value="Facebook">Facebook</option>
                <option value="Email">Email</option>
                <option value="TV">TV</option>
                <option value="Redes Sociales">Redes Sociales</option>
            </select>

            <button type="button" class="btn-a btn-a-outline" id="btnRecargarCampana">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div id="marketingListado" class="marketing-listado">
            <div class="table-empty">Cargando campañas...</div>
        </div>
    </div>
</div>

<div class="modal-a" id="modalCampana" style="display:none;">
    <div class="modal-a__backdrop"></div>

    <div class="modal-a__dialog modal-a__dialog--marketing">
        <div class="modal-a__header">
            <h3 id="modalCampanaTitulo">Nueva campaña</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalCampanaX">×</button>
        </div>

        <div class="modal-a__body">
            <div class="form-grid">
                <div class="a-form-group form-grid__full">
                    <label for="txtNombreCampana">Nombre de campaña</label>
                    <input type="text" id="txtNombreCampana" class="a-input no-icon" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="selCanalCampana">Canal</label>
                    <select id="selCanalCampana" class="a-input no-icon">
                        <option value="">Seleccione canal</option>
                        <option value="Instagram">Instagram</option>
                        <option value="Facebook">Facebook</option>
                        <option value="Email">Email</option>
                        <option value="TV">TV</option>
                        <option value="Redes Sociales">Redes Sociales</option>
                    </select>
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="txtPresupuestoCampana">Presupuesto (Q)</label>
                    <input type="number" id="txtPresupuestoCampana" class="a-input no-icon" min="0" step="0.01" />
                </div>

                <div class="a-form-group">
                    <label for="txtInicioCampana">Fecha inicio</label>
                    <input type="date" id="txtInicioCampana" class="a-input no-icon" />
                </div>

                <div class="a-form-group">
                    <label for="txtFinCampana">Fecha fin</label>
                    <input type="date" id="txtFinCampana" class="a-input no-icon" />
                </div>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCerrarModalCampana">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarCampana">Guardar</button>
        </div>
    </div>
</div>

@section scripts
    <script src="~/Scripts/admin-marketing.js"></script>
End Section