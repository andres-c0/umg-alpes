@Code
    ViewData("Title") = "Catálogo"
    Layout = "~/Views/Shared/_PublicLayout.vbhtml"

    Dim categoriaInicial As String = ""

    If ViewData("CategoriaInicial") IsNot Nothing Then
        categoriaInicial = ViewData("CategoriaInicial").ToString()
    End If

    Dim busquedaInicial As String = ""

    If ViewData("BusquedaInicial") IsNot Nothing Then
        busquedaInicial = ViewData("BusquedaInicial").ToString()
    End If
End Code

<section class="catalogo-publico-page"
         id="catalogoPublicoPage"
         data-productos-url="@Url.Action("ObtenerProductosPublicosData", "Home")"
         data-categoria-inicial="@categoriaInicial"
         data-busqueda-inicial="@busquedaInicial">

    <section class="catalogo-hero">
        <div class="catalogo-hero-content">
            <a href="@Url.Action("Index", "Home")" class="producto-publico-back catalogo-back-link">
                <i class="bi bi-arrow-left"></i>
                Volver al inicio
            </a>

            <span>CATÁLOGO PÚBLICO</span>

            <h1>Explora nuestra colección</h1>

            <p>
                Encuentra muebles para interior y exterior con materiales de calidad,
                diseño elegante y una experiencia de compra sencilla.
            </p>

            <div class="catalogo-hero-stats">
                <article>
                    <strong id="catalogoTotalProductos">0</strong>
                    <span>Productos disponibles</span>
                </article>

                <article>
                    <strong>2</strong>
                    <span>Líneas principales</span>
                </article>

                <article>
                    <strong>24/7</strong>
                    <span>Catálogo disponible</span>
                </article>
            </div>
        </div>
    </section>

    <section class="catalogo-toolbar">
        <div class="catalogo-search-box">
            <i class="bi bi-search"></i>
            <input type="text"
                   id="catalogoBusqueda"
                   value="@busquedaInicial"
                   placeholder="Buscar por nombre, referencia, material o color..." />
        </div>

        <div class="catalogo-filtros" role="group" aria-label="Filtros de catálogo">
            <button type="button" class="catalogo-filtro-btn active" data-catalogo-filtro="">
                Todos
            </button>

            <button type="button" class="catalogo-filtro-btn" data-catalogo-filtro="INTERIOR">
                Interior
            </button>

            <button type="button" class="catalogo-filtro-btn" data-catalogo-filtro="EXTERIOR">
                Exterior
            </button>
        </div>
    </section>

    <section class="catalogo-content">
        <div class="catalogo-loading" id="catalogoLoading">
            <span class="home-loader"></span>
            <p>Cargando productos...</p>
        </div>

        <div class="catalogo-empty" id="catalogoEmpty" style="display:none;">
            <i class="bi bi-box-seam"></i>
            <h2>No encontramos productos</h2>
            <p>Prueba con otro filtro o cambia el texto de búsqueda.</p>

            <button type="button" class="home-btn home-btn-dark-outline" id="catalogoLimpiarFiltros">
                Limpiar filtros
            </button>
        </div>

        <div class="catalogo-grid" id="catalogoGrid"></div>
    </section>
</section>

@section scripts
    <script src="@Url.Content("~/Scripts/catalogo-publico.js?v=1")"></script>
End Section