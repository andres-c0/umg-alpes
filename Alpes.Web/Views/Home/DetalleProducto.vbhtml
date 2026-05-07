@Code
    ViewData("Title") = "Detalle de producto"
    Layout = "~/Views/Shared/_PublicLayout.vbhtml"

    Dim productoId As Integer = 0

    If ViewData("ProductoId") IsNot Nothing Then
        Integer.TryParse(ViewData("ProductoId").ToString(), productoId)
    End If
End Code

<section class="producto-publico-page"
         id="productoPublicoPage"
         data-producto-id="@productoId"
         data-producto-url="@Url.Action("ObtenerProductoPublicoData", "Home")">

    <div class="producto-publico-loading" id="productoDetalleLoading">
        <span class="home-loader"></span>
        <p>Cargando detalle del producto...</p>
    </div>

    <div class="producto-publico-error" id="productoDetalleError" style="display:none;">
        <i class="bi bi-exclamation-circle"></i>
        <h2>No se pudo cargar el producto</h2>
        <p id="productoDetalleErrorTexto">Intenta nuevamente más tarde.</p>

        <a href="@Url.Action("Index", "Home")" class="home-btn home-btn-dark-outline">
            Volver al inicio
        </a>
    </div>

    <div class="producto-publico-layout" id="productoDetalleContenido" style="display:none;">
        <div class="producto-publico-media">
            <div class="producto-publico-badge" id="productoDetalleTipo">PRODUCTO</div>

            <div class="producto-publico-imagen" id="productoDetalleImagen">
                <i class="bi bi-armchair"></i>
            </div>
        </div>

        <div class="producto-publico-info">
            <a href="@Url.Action("Index", "Home")#productos-destacados" class="producto-publico-back">
                <i class="bi bi-arrow-left"></i>
                Volver a productos
            </a>

            <span class="producto-publico-referencia" id="productoDetalleReferencia">Referencia</span>

            <h1 id="productoDetalleNombre">Producto</h1>

            <p class="producto-publico-descripcion" id="productoDetalleDescripcion">
                Mueble artesanal de Muebles de los Alpes.
            </p>

            <div class="producto-publico-precio">
                <strong id="productoDetallePrecio">Q 0.00</strong>
                <span id="productoDetalleCuota">12 cuotas de Q 0.00</span>
            </div>

            <div class="producto-publico-actions">
                <button type="button" class="home-btn home-btn-gold" id="btnAgregarDetalleCarrito">
                    <i class="bi bi-cart3"></i>
                    Agregar al carrito
                </button>

                <a href="@Url.Action("CarritoInvitado", "Home")" class="home-btn home-btn-dark-outline">
                    Ver carrito
                </a>
            </div>

            <div class="producto-publico-specs">
                <article>
                    <span>Material</span>
                    <strong id="productoDetalleMaterial">No especificado</strong>
                </article>

                <article>
                    <span>Color</span>
                    <strong id="productoDetalleColor">No especificado</strong>
                </article>

                <article>
                    <span>Dimensiones</span>
                    <strong id="productoDetalleDimensiones">No especificado</strong>
                </article>

                <article>
                    <span>Peso</span>
                    <strong id="productoDetallePeso">No especificado</strong>
                </article>
            </div>

            <div class="producto-publico-benefits">
                <div>
                    <i class="bi bi-truck"></i>
                    <span>Envío disponible</span>
                </div>

                <div>
                    <i class="bi bi-shield-check"></i>
                    <span>Compra segura</span>
                </div>

                <div>
                    <i class="bi bi-award"></i>
                    <span>Calidad garantizada</span>
                </div>
            </div>
        </div>
    </div>
</section>

@section scripts
    <script src="@Url.Content("~/Scripts/producto-publico-detalle.js?v=1")"></script>
End Section