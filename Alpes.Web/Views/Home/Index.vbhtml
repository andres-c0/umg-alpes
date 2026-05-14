@Code
    ViewData("Title") = "Inicio"
    Layout = "~/Views/Shared/_PublicLayout.vbhtml"
End Code

<section class="home-public-hero" id="inicio">
    <div class="home-hero-slider" id="homeHeroSlider">
        <article class="home-hero-slide active" data-slide="0">
            <div class="home-hero-bg home-hero-bg-cafe"></div>

            <div class="home-hero-pattern home-hero-pattern-one"></div>
            <div class="home-hero-pattern home-hero-pattern-two"></div>

            <div class="home-hero-content">
                <span class="home-hero-eyebrow">COLECCIÓN ARTESANAL 2025</span>

                <h1 class="home-hero-title-balanced" data-i18n="home.hero1.title">
                    <span>Muebles que</span>
                    <span>cuentan una</span>
                    <span>historia</span>
                </h1>

                <p data-i18n="home.hero1.text">
                    Fabricados a mano con madera guatemalteca seleccionada,
                    acabados elegantes y presencia cálida para cada espacio.
                </p>

                <div class="home-hero-actions">
                    <a href="@Url.Action("CatalogoPublico", "Home")" class="home-btn home-btn-gold">
                        Explorar colección
                        <i class="bi bi-arrow-right"></i>
                    </a>

                    <a href="#productos-destacados" class="home-btn home-btn-light-outline">
                        Ver destacados
                    </a>
                </div>
            </div>
        </article>

        <article class="home-hero-slide" data-slide="1">
            <div class="home-hero-bg home-hero-bg-verde"></div>

            <div class="home-hero-pattern home-hero-pattern-one"></div>
            <div class="home-hero-pattern home-hero-pattern-two"></div>

            <div class="home-hero-content">
                <span class="home-hero-eyebrow">LÍNEA COLONIAL</span>

                <h1 data-i18n="home.hero2.title">
                    Herencia<br />
                    que se<br />
                    siente
                </h1>

                <p data-i18n="home.hero2.text">
                    Diseño colonial con acabados artesanales, materiales resistentes
                    y estilo pensado para hogares con carácter.
                </p>

                <div class="home-hero-actions">
                    <a href="@Url.Action("CatalogoPublico", "Home", New With {.categoria = "Interior"})" class="home-btn home-btn-gold">
                        Ver interior
                        <i class="bi bi-arrow-right"></i>
                    </a>

                    <a href="@Url.Action("Registro", "Home")" class="home-btn home-btn-light-outline">
                        Crear cuenta
                    </a>
                </div>
            </div>
        </article>

        <article class="home-hero-slide" data-slide="2">
            <div class="home-hero-bg home-hero-bg-nogal"></div>

            <div class="home-hero-pattern home-hero-pattern-one"></div>
            <div class="home-hero-pattern home-hero-pattern-two"></div>

            <div class="home-hero-content">
                <span class="home-hero-eyebrow">ENVÍO SIN COSTO</span>

                <h1 data-i18n="home.hero3.title">
                    A toda<br />
                    Guatemala<br />
                    sin cargo
                </h1>

                <p data-i18n="home.hero3.text">
                    Entregamos en los 22 departamentos con una experiencia de compra
                    sencilla, segura y pensada para el cliente.
                </p>

                <div class="home-hero-actions">
                    <a href="@Url.Action("CatalogoPublico", "Home", New With {.categoria = "Exterior"})" class="home-btn home-btn-gold">
                        Ver exterior
                        <i class="bi bi-arrow-right"></i>
                    </a>

                    <a href="@Url.Action("CarritoInvitado", "Home")" class="home-btn home-btn-light-outline">
                        Ver carrito
                    </a>
                </div>
            </div>
        </article>

        <div class="home-hero-indicators" id="homeHeroIndicators">
            <button type="button" class="active" data-target-slide="0" aria-label="Ver banner 1"></button>
            <button type="button" data-target-slide="1" aria-label="Ver banner 2"></button>
            <button type="button" data-target-slide="2" aria-label="Ver banner 3"></button>
        </div>
    </div>
</section>

<section class="home-public-section home-categories-section">
    <div class="home-section-heading">
        <span>POR CATEGORÍA</span>
        <h2>Encuentra tu estilo</h2>
        <p>
            Explora muebles diseñados para interiores elegantes o espacios exteriores
            con resistencia, comodidad y presencia visual.
        </p>
    </div>

    <div class="home-category-grid">
        <a href="@Url.Action("CatalogoPublico", "Home", New With {.categoria = "Interior"})" class="home-category-card home-category-interior">
            <div class="home-category-overlay"></div>

            <div class="home-category-content">
                <span class="home-category-icon">
                    <i class="bi bi-house-door"></i>
                </span>

                <h3>Interior</h3>
                <p>Salas, comedores, dormitorios y espacios cálidos.</p>

                <strong>
                    Explorar
                    <i class="bi bi-arrow-right"></i>
                </strong>
            </div>
        </a>

        <a href="@Url.Action("CatalogoPublico", "Home", New With {.categoria = "Exterior"})" class="home-category-card home-category-exterior">
            <div class="home-category-overlay"></div>

            <div class="home-category-content">
                <span class="home-category-icon">
                    <i class="bi bi-tree"></i>
                </span>

                <h3>Exterior</h3>
                <p>Terrazas, jardines y ambientes abiertos.</p>

                <strong>
                    Explorar
                    <i class="bi bi-arrow-right"></i>
                </strong>
            </div>
        </a>
    </div>
</section>

<section class="home-public-section home-featured-section" id="productos-destacados">
    <div class="home-featured-heading">
        <div>
            <span>SELECCIÓN ESPECIAL</span>
            <h2>Productos destacados</h2>
        </div>

        <a href="@Url.Action("CatalogoPublico", "Home")">
            Ver catálogo completo
            <i class="bi bi-arrow-right"></i>
        </a>
    </div>

    <div class="home-featured-loading" id="homeProductosLoading">
        <span class="home-loader"></span>
        <p>Cargando productos destacados...</p>
    </div>

    <div class="home-products-row"
         id="homeProductosDestacados"
         data-productos-url="@Url.Action("ObtenerProductosPublicosData", "Home")"
         data-detalle-base-url="@Url.Action("DetalleProducto", "Home")">
        <article class="home-product-card home-product-placeholder">
            <div class="home-product-image">
                <i class="bi bi-armchair"></i>
            </div>

            <div class="home-product-info">
                <span>INTERIOR</span>
                <h3>Sala Colonial Alpes</h3>

                <div class="home-product-price-row">
                    <strong>Q 3,250</strong>

                    <button type="button" class="home-product-cart-btn" data-demo-cart="true" aria-label="Agregar producto demo">
                        <i class="bi bi-cart3"></i>
                    </button>
                </div>

                <small>12 cuotas de Q 271</small>
            </div>
        </article>

        <article class="home-product-card home-product-placeholder">
            <div class="home-product-image">
                <i class="bi bi-lamp"></i>
            </div>

            <div class="home-product-info">
                <span>EXTERIOR</span>
                <h3>Terraza Selva</h3>

                <div class="home-product-price-row">
                    <strong>Q 2,780</strong>

                    <button type="button" class="home-product-cart-btn" data-demo-cart="true" aria-label="Agregar producto demo">
                        <i class="bi bi-cart3"></i>
                    </button>
                </div>

                <small>12 cuotas de Q 232</small>
            </div>
        </article>

        <article class="home-product-card home-product-placeholder">
            <div class="home-product-image">
                <i class="bi bi-layout-sidebar"></i>
            </div>

            <div class="home-product-info">
                <span>PREMIUM</span>
                <h3>Comedor Nogal</h3>

                <div class="home-product-price-row">
                    <strong>Q 4,650</strong>

                    <button type="button" class="home-product-cart-btn" data-demo-cart="true" aria-label="Agregar producto demo">
                        <i class="bi bi-cart3"></i>
                    </button>
                </div>

                <small>12 cuotas de Q 388</small>
            </div>
        </article>
    </div>

    <div class="home-products-empty" id="homeProductosEmpty" style="display:none;">
        <i class="bi bi-box-seam"></i>
        <h3>No hay productos disponibles</h3>
        <p>Cuando existan productos activos, se mostrarán en esta sección.</p>
    </div>
</section>

<section class="home-benefits-strip">
    <div class="home-benefit-item">
        <i class="bi bi-truck"></i>
        <strong>Envío sin costo</strong>
        <span>A toda Guatemala</span>
    </div>

    <div class="home-benefit-item">
        <i class="bi bi-award"></i>
        <strong>Garantía de 1 año</strong>
        <span>Por defecto de fábrica</span>
    </div>

    <div class="home-benefit-item">
        <i class="bi bi-tools"></i>
        <strong>Artesanal</strong>
        <span>Fabricado a mano</span>
    </div>

    <div class="home-benefit-item">
        <i class="bi bi-arrow-left-right"></i>
        <strong>Devolución</strong>
        <span>Primeros 30 días</span>
    </div>
</section>

<section class="home-public-section home-editorial-section">
    <div class="home-section-heading">
        <span>TENDENCIAS 2025</span>
        <h2>Inspiración para tu hogar</h2>
        <p>
            Colecciones pensadas para transformar espacios con equilibrio,
            tradición, textura y diseño contemporáneo.
        </p>
    </div>

    <div class="home-editorial-grid">
        <article class="home-editorial-card home-editorial-card-cafe">
            <div class="home-editorial-gradient"></div>

            <div class="home-editorial-content">
                <span>TENDENCIA</span>
                <h3>Minimalismo<br />Cálido</h3>
                <p>Espacios serenos con materiales nobles y tonos naturales.</p>
            </div>

            <i class="bi bi-arrow-right"></i>
        </article>

        <article class="home-editorial-card home-editorial-card-verde">
            <div class="home-editorial-gradient"></div>

            <div class="home-editorial-content">
                <span>TENDENCIA</span>
                <h3>Estilo<br />Colonial</h3>
                <p>Herencia guatemalteca con presencia elegante y artesanal.</p>
            </div>

            <i class="bi bi-arrow-right"></i>
        </article>

        <article class="home-editorial-card home-editorial-card-grafito">
            <div class="home-editorial-gradient"></div>

            <div class="home-editorial-content">
                <span>TENDENCIA</span>
                <h3>Ecléctico<br />Moderno</h3>
                <p>Fusión con carácter para ambientes diferentes y memorables.</p>
            </div>

            <i class="bi bi-arrow-right"></i>
        </article>
    </div>
</section>

<section class="home-final-cta">
    <div class="home-final-decoration">
        <span></span>
        <strong></strong>
        <span></span>
    </div>

    <h2>Transforma tu hogar</h2>

    <p>
        Crea tu cuenta y accede a precios exclusivos, cuotas,
        seguimiento de pedidos y una experiencia de compra personalizada.
    </p>

    <div class="home-final-actions">
        <a href="@Url.Action("Registro", "Home")" class="home-btn home-btn-gold">
            Crear cuenta gratuita
        </a>

        <a href="@Url.Action("Login", "Home")" class="home-btn home-btn-dark-outline">
            Ya tengo cuenta — Iniciar sesión
        </a>
    </div>
</section>

<div class="home-access-modal" id="homeAccessModal" aria-hidden="true">
    <div class="home-access-backdrop" data-close-access-modal="true"></div>

    <div class="home-access-panel">
        <button type="button" class="home-access-close" data-close-access-modal="true" aria-label="Cerrar">
            <i class="bi bi-x-lg"></i>
        </button>

        <div class="home-access-handle"></div>

        <div class="home-access-header">
            <span>
                <i class="bi bi-armchair"></i>
            </span>

            <div>
                <h3>Muebles de los Alpes</h3>
                <p id="homeAccessModalText">Bienvenido de vuelta</p>
            </div>
        </div>

        <a href="@Url.Action("Login", "Home")" class="home-access-option home-access-login">
            <div>
                <strong>Iniciar sesión</strong>
                <small>Ya tengo una cuenta</small>
            </div>

            <i class="bi bi-arrow-right"></i>
        </a>

        <a href="@Url.Action("Registro", "Home")" class="home-access-option home-access-register">
            <div>
                <strong>Crear cuenta gratuita</strong>
                <small>Sin compromisos</small>
            </div>

            <i class="bi bi-arrow-right"></i>
        </a>

        <button type="button" class="home-access-continue" data-close-access-modal="true">
            Continuar explorando
        </button>
    </div>
</div>