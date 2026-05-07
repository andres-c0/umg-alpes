@Code
    Layout = "~/Views/Shared/_PortalClienteLayout.vbhtml"
    ViewData("Title") = "Mis reseñas"
End Code

<section class="pc-page pc-reviews-page">
    <div class="pc-hero pc-hero--reviews">
        <div>
            <span class="pc-kicker">EXPERIENCIA</span>
            <h1>Mis reseñas</h1>
            <p>Consulta las valoraciones que has realizado y comparte tu opinión sobre los muebles que compraste.</p>
        </div>
        <button type="button" class="pc-btn pc-btn-gold" id="btnAbrirNuevaResena">
            <i class="bi bi-star-fill"></i>
            Nueva reseña
        </button>
    </div>

    <div class="pc-stats-grid pc-review-stats">
        <article class="pc-stat-card">
            <div class="pc-stat-icon"><i class="bi bi-chat-square-heart"></i></div>
            <div>
                <div class="pc-stat-value" id="resenasTotal">0</div>
                <div class="pc-stat-label">Reseñas</div>
            </div>
        </article>
        <article class="pc-stat-card">
            <div class="pc-stat-icon"><i class="bi bi-star-half"></i></div>
            <div>
                <div class="pc-stat-value" id="resenasPromedio">0.0</div>
                <div class="pc-stat-label">Promedio</div>
            </div>
        </article>
        <article class="pc-stat-card">
            <div class="pc-stat-icon"><i class="bi bi-check2-circle"></i></div>
            <div>
                <div class="pc-stat-value" id="resenasActivas">0</div>
                <div class="pc-stat-label">Activas</div>
            </div>
        </article>
    </div>

    <div class="pc-card pc-review-toolbar">
        <div class="pc-input-icon">
            <i class="bi bi-search"></i>
            <input type="text" id="txtBuscarResena" class="pc-input" placeholder="Buscar por producto, comentario o estado" />
        </div>
        <select id="cmbFiltroResena" class="pc-select">
            <option value="TODAS">Todas</option>
            <option value="5">5 estrellas</option>
            <option value="4">4 estrellas</option>
            <option value="3">3 estrellas</option>
            <option value="2">2 estrellas</option>
            <option value="1">1 estrella</option>
        </select>
    </div>

    <div id="resenasLoading" class="pc-loading">
        <div class="pc-spinner"></div>
        <span>Cargando tus reseñas...</span>
    </div>

    <div id="resenasEmpty" class="pc-empty" style="display:none;">
        <i class="bi bi-star"></i>
        <h3>Aún no tienes reseñas</h3>
        <p>Cuando valores tus productos, aparecerán en esta sección.</p>
        <a href="@Url.Action("Busqueda", "PortalCliente")" class="pc-btn pc-btn-primary">Explorar catálogo</a>
    </div>

    <div id="resenasGrid" class="pc-review-grid"></div>
</section>

<div class="pc-modal" id="modalNuevaResena" aria-hidden="true">
    <div class="pc-modal-backdrop" data-close-modal="modalNuevaResena"></div>
    <div class="pc-modal-dialog pc-review-modal">
        <div class="pc-modal-header">
            <div>
                <span class="pc-kicker">TU OPINIÓN</span>
                <h3>Registrar reseña</h3>
            </div>
            <button type="button" class="pc-icon-btn" data-close-modal="modalNuevaResena">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>

        <form id="formNuevaResena" class="pc-form">
            <div class="pc-form-group">
                <label for="txtProductoResena">Producto ID</label>
                <input type="number" id="txtProductoResena" class="pc-input" min="1" placeholder="Ej. 74" required />
                <small>Usa el ID del producto que aparece en el detalle o catálogo.</small>
            </div>

            <div class="pc-form-group">
                <label>Calificación</label>
                <div class="pc-rating-input" id="ratingInput">
                    <button type="button" data-rating="1"><i class="bi bi-star-fill"></i></button>
                    <button type="button" data-rating="2"><i class="bi bi-star-fill"></i></button>
                    <button type="button" data-rating="3"><i class="bi bi-star-fill"></i></button>
                    <button type="button" data-rating="4"><i class="bi bi-star-fill"></i></button>
                    <button type="button" data-rating="5"><i class="bi bi-star-fill"></i></button>
                </div>
                <input type="hidden" id="txtCalificacionResena" value="5" />
            </div>

            <div class="pc-form-group">
                <label for="txtComentarioResena">Comentario</label>
                <textarea id="txtComentarioResena" class="pc-textarea" rows="4" maxlength="500" placeholder="Cuéntanos cómo fue tu experiencia con este mueble"></textarea>
            </div>

            <div class="pc-modal-actions">
                <button type="button" class="pc-btn pc-btn-outlined" data-close-modal="modalNuevaResena">Cancelar</button>
                <button type="submit" class="pc-btn pc-btn-primary" id="btnGuardarResena">Guardar reseña</button>
            </div>
        </form>
    </div>
</div>

@Section scripts
    <script src="@Url.Content("~/Scripts/portal-resenas.js?v=16")"></script>
End Section
