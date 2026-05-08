@Code
    ViewData("Title") = "Productos"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="admin-page">
    <div class="admin-page__header">
        <div>
            <h2 class="admin-page__title">Productos</h2>
            <div class="admin-page__subtitle">Gestión de productos registrados</div>
        </div>

        <button type="button" class="btn-a btn-a-primary" id="btnNuevoProducto">
            <i class="bi bi-plus-lg"></i>
            Nuevo producto
        </button>
    </div>

    <div class="admin-card">
        <div class="admin-toolbar">
            <input type="text" id="txtBuscarProducto" class="a-input" placeholder="Buscar por nombre o referencia..." />
            <button type="button" class="btn-a" id="btnRecargarProductos">
                <i class="bi bi-arrow-clockwise"></i>
                Recargar
            </button>
        </div>

        <div class="table-wrap">
            <table class="admin-table" id="tablaProductos">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Referencia</th>
                        <th>Nombre</th>
                        <th>Tipo</th>
                        <th>Material</th>
                        <th>Categoría</th>
                        <th>Unidad</th>
                        <th>Color</th>
                        <th>Estado</th>
                        <th style="width:140px;">Acciones</th>
                    </tr>
                </thead>
                <tbody id="productosBody">
                    <tr>
                        <td colspan="10" class="table-empty">Cargando productos...</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>

<div class="modal-a" id="modalProducto" style="display:none;">
    <div class="modal-a__backdrop"></div>
    <div class="modal-a__dialog modal-a__dialog--lg">
        <div class="modal-a__header">
            <h3 id="modalProductoTitulo">Nuevo producto</h3>
            <button type="button" class="modal-a__close" id="btnCerrarModalProducto">×</button>
        </div>

        <div class="modal-a__body">
            <input type="hidden" id="ProductoId" value="0" />

            <div class="form-grid">
                <div class="a-form-group">
                    <label for="Referencia">Referencia</label>
                    <input type="text" id="Referencia" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="Nombre">Nombre</label>
                    <input type="text" id="Nombre" class="a-input" />
                </div>

                <div class="a-form-group form-grid__full">
                    <label for="Descripcion">Descripción</label>
                    <textarea id="Descripcion" class="a-input" rows="3"></textarea>
                </div>

                <div class="a-form-group">
                    <label for="Tipo">Tipo</label>
                    <input type="text" id="Tipo" class="a-input" placeholder="INTERIOR / EXTERIOR" />
                </div>

                <div class="a-form-group">
                    <label for="Material">Material</label>
                    <input type="text" id="Material" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="AltoCm">Alto cm</label>
                    <input type="number" step="0.01" id="AltoCm" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="AnchoCm">Ancho cm</label>
                    <input type="number" step="0.01" id="AnchoCm" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="ProfundidadCm">Profundidad cm</label>
                    <input type="number" step="0.01" id="ProfundidadCm" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="Color">Color</label>
                    <input type="text" id="Color" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="PesoGramos">Peso gramos</label>
                    <input type="number" step="0.01" id="PesoGramos" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="ImagenUrl">Imagen URL</label>
                    <input type="text" id="ImagenUrl" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="UnidadMedidaId">Unidad de medida</label>
                    <select id="UnidadMedidaId" class="a-input">
                        <option value="">Seleccione...</option>
                    </select>
                </div>

                <div class="a-form-group">
                    <label for="CategoriaId">Categoría</label>
                    <select id="CategoriaId" class="a-input">
                        <option value="">Seleccione...</option>
                    </select>
                </div>

                <div class="a-form-group">
                    <label for="LoteProducto">Lote producto</label>
                    <input type="text" id="LoteProducto" class="a-input" />
                </div>

                <div class="a-form-group">
                    <label for="Estado">Estado</label>
                    <input type="text" id="Estado" class="a-input" placeholder="ACTIVO / INACTIVO" />
                </div>
            </div>

            <div class="a-alert error" id="productoError" style="display:none;">
                <i class="bi bi-exclamation-circle"></i>
                <span id="productoErrorTexto"></span>
            </div>
        </div>

        <div class="modal-a__footer">
            <button type="button" class="btn-a" id="btnCancelarProducto">Cancelar</button>
            <button type="button" class="btn-a btn-a-primary" id="btnGuardarProducto">Guardar</button>
        </div>
    </div>
</div>

@Scripts.Render("~/bundles/jquery")
<script src="~/Scripts/admin-productos.js"></script>