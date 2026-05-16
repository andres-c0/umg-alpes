(function () {
    "use strict";

    var STORAGE_KEY = "pc_cfg_idioma";
    var ADMIN_STORAGE_KEY = "adminLang";
    var isApplying = false;
    var observerTimer = null;

    var dictionary = {
        en: {
            "dashboard": "Dashboard",
            "productos": "products",
            "ordenes": "Orders",
            "clientes": "Customers",
            "reportes": "Reports",
            "inventario": "Inventory",
            "proveedores": "Suppliers",
            "compras": "Purchases",
            "empleados": "Employees",
            "nomina": "Payroll",
            "marketing": "Marketing",
            "produccion": "Production",
            "perfil": "My profile",
            "configuracion": "Settings",
            "cerrarSesion": "Log out",
            "home.hero1.badge": "ARTISAN COLLECTION 2025",
            "home.hero1.title": "<span>Furniture that</span><span>tells a</span><span>story</span>",
            "home.hero1.text": "Handcrafted with selected Guatemalan wood, elegant finishes, and a warm presence for every space.",
            "home.hero2.badge": "COLONIAL LINE",
            "home.hero2.title": "Heritage<br />you can<br />feel",
            "home.hero2.text": "Colonial design with handcrafted finishes, resistant materials, and a style made for homes with character.",
            "home.hero3.badge": "FREE SHIPPING",
            "home.hero3.title": "All across<br />Guatemala<br />free of charge",
            "home.hero3.text": "We deliver to all 22 departments with a simple, safe, and customer-focused shopping experience.",
            "home.category.badge": "BY CATEGORY",
            "home.category.title": "Find your style",
            "home.category.text": "Explore furniture designed for elegant interiors or outdoor spaces with durability, comfort, and visual presence.",
            "home.featured.badge": "SPECIAL SELECTION",
            "home.featured.title": "Featured products",
            "home.featured.button": "View full catalog",
            "home.featured.loading": "Loading featured products...",
            "home.trends.badge": "2025 TRENDS",
            "home.trends.title": "Inspiration for your home",
            "home.trends.text": "Collections designed to transform spaces with balance, tradition, texture, and contemporary design.",
            "home.cta.title": "Transform your home",
            "home.cta.text": "Create your account and access exclusive prices, installments, order tracking, and a personalized shopping experience.",
            "Muebles de los Alpes": "Muebles de los Alpes",
            "Panel Administrativo": "Admin Panel",
            "Portal del Cliente": "Customer Portal",
            "Proyecto Base de Datos II.": "Database II Project.",
            "Artesanía · Calidad": "Craftsmanship · Quality",
            "Artesanía · Calidad · Diseño": "Craftsmanship · Quality · Design",
            "ARTESANÍA · CALIDAD · DISEÑO": "CRAFTSMANSHIP · QUALITY · DESIGN",
            "Inicio": "Home",
            "Catálogo": "Catalog",
            "Catalogo": "Catalog",
            "Cat logo": "Catalog",
            "Favoritos": "Favorites",
            "Órdenes": "Orders",
            "Ordenes": "Orders",
            "ORDENES": "ORDERS",
            "Carrito": "Cart",
            "Perfil": "Profile",
            "Mi perfil": "My profile",
            "Configuración": "Settings",
            "Configuraci n": "Settings",
            "Soporte": "Support",
            "Soporte Alpes": "Alpes Support",
            "Cerrar sesión": "Log out",
            "Cerrar sesi n": "Log out",
            "Iniciar sesión": "Sign in",
            "Crear cuenta": "Create account",
            "Idioma": "Language",
            "Español": "Spanish",
            "Inglés": "English",
            "English": "English",
            "Language": "Language",
            "Seleccionar idioma": "Select language",
            "Actualmente el panel está optimizado para español.": "The customer panel can be displayed in Spanish or English.",
            "Tienda": "Store",
            "TIENDA": "STORE",
            "MI CUENTA": "MY ACCOUNT",
            "Mi cuenta": "My account",
            "Cuenta": "Account",
            "Contacto": "Contact",
            "Atención al cliente": "Customer service",
            "Soporte de compras": "Shopping support",
            "Guatemala": "Guatemala",
            "VIP": "VIP",
            "Comercial": "Commercial",
            "COMERCIAL": "COMMERCIAL",
            "Operativa": "Operations",
            "OPERATIVA": "OPERATIONS",
            "Administrador": "Administrator",
            "Dashboard": "Dashboard",
            "Dashboard administrativo de órdenes y rendimiento": "Administrative dashboard for orders and performance",
            "Productos": "Products",
            "Clientes": "Customers",
            "Reportes": "Reports",
            "Inventario": "Inventory",
            "Proveedores": "Suppliers",
            "Compras": "Purchases",
            "Empleados": "Employees",
            "Nómina": "Payroll",
            "Nomina": "Payroll",
            "Producción": "Production",
            "Produccion": "Production",
            "Marketing": "Marketing",
            "Acciones": "Actions",
            "Activo": "Active",
            "ACTIVO": "ACTIVE",
            "Inactivo": "Inactive",
            "INACTIVO": "INACTIVE",
            "Estado": "Status",
            "Estado actual": "Current status",
            "Fecha": "Date",
            "Nombre": "Name",
            "Nombres": "First name",
            "Apellidos": "Last name",
            "Correo": "Email",
            "Email": "Email",
            "Teléfono": "Phone",
            "Telefono": "Phone",
            "Dirección": "Address",
            "Direccion": "Address",
            "Ciudad": "City",
            "Departamento": "Department",
            "País": "Country",
            "Código": "Code",
            "Codigo": "Code",
            "Descripción": "Description",
            "Descripcion": "Description",
            "Precio": "Price",
            "Cantidad": "Quantity",
            "Referencia": "Reference",
            "Disponible": "Available",
            "Disponibilidad": "Availability",
            "Stock": "Stock",
            "Subtotal": "Subtotal",
            "Total": "Total",
            "Impuesto": "Tax",
            "Descuento": "Discount",
            "Envío": "Shipping",
            "Envio": "Shipping",
            "Método de pago": "Payment method",
            "Metodo de pago": "Payment method",
            "Guardar": "Save",
            "GUARDAR": "SAVE",
            "Guardar cambios": "Save changes",
            "GUARDAR CAMBIOS": "SAVE CHANGES",
            "Cancelar": "Cancel",
            "Cerrar": "Close",
            "Continuar": "Continue",
            "Siguiente": "Next",
            "Anterior": "Previous",
            "Enviar": "Send",
            "Buscar": "Search",
            "Limpiar": "Clear",
            "Limpiar filtros": "Clear filters",
            "Filtrar": "Filter",
            "Generar": "Generate",
            "Generar reporte": "Generate report",
            "Exportar": "Export",
            "Actualizar": "Refresh",
            "Recargar": "Reload",
            "Aplicar": "Apply",
            "Eliminar": "Delete",
            "Editar": "Edit",
            "Nuevo": "New",
            "Nueva": "New",
            "Agregar": "Add",
            "Crear": "Create",
            "COLECCIÓN ARTESANAL 2025": "ARTISAN COLLECTION 2025",
            "LÍNEA COLONIAL": "COLONIAL LINE",
            "ENVÍO SIN COSTO": "FREE SHIPPING",
            "Muebles que cuentan una historia": "Furniture that tells a story",
            "Fabricados a mano con madera guatemalteca seleccionada, acabados elegantes y presencia cálida para cada espacio.": "Handcrafted with selected Guatemalan wood, elegant finishes, and a warm presence for every space.",
            "Explorar colección": "Explore collection",
            "Ver destacados": "View featured items",
            "Ver interior": "View indoor",
            "Ver exterior": "View outdoor",
            "Ver carrito": "View cart",
            "POR CATEGORÍA": "BY CATEGORY",
            "Encuentra tu estilo": "Find your style",
            "Explora muebles diseñados para interiores elegantes o espacios exteriores con resistencia, comodidad y presencia visual.": "Explore furniture designed for elegant interiors or outdoor spaces with durability, comfort, and visual presence.",
            "Interior": "Indoor",
            "Exterior": "Outdoor",
            "INTERIOR": "INDOOR",
            "EXTERIOR": "OUTDOOR",
            "Salas, comedores, dormitorios y espacios cálidos.": "Living rooms, dining rooms, bedrooms, and warm spaces.",
            "Terrazas, jardines y ambientes abiertos.": "Terraces, gardens, and open spaces.",
            "Explorar": "Explore",
            "SELECCIÓN ESPECIAL": "SPECIAL SELECTION",
            "Productos destacados": "Featured products",
            "Ver catálogo completo": "View full catalog",
            "Cargando productos destacados...": "Loading featured products...",
            "No se pudieron cargar los productos": "Products could not be loaded",
            "Envío sin costo": "Free shipping",
            "Compra protegida": "Protected purchase",
            "Muebles artesanales": "Handcrafted furniture",
            "Diseño para interior y exterior": "Design for indoor and outdoor spaces",
            "Creamos muebles con materiales de calidad, acabados elegantes y una experiencia de compra cómoda para nuestros clientes.": "We create furniture with quality materials, elegant finishes, and a comfortable shopping experience for our customers.",
            "TENDENCIAS 2025": "2025 TRENDS",
            "Inspiración para tu hogar": "Inspiration for your home",
            "Colecciones pensadas para transformar espacios con equilibrio, tradición, textura y diseño contemporáneo.": "Collections designed to transform spaces with balance, tradition, texture, and contemporary design.",
            "TENDENCIA": "TREND",
            "Minimalismo": "Minimalism",
            "Cálido": "Warm",
            "Minimalismo Cálido": "Warm Minimalism",
            "Estilo Colonial": "Colonial Style",
            "Ecléctico Moderno": "Modern Eclectic",
            "Transforma tu hogar": "Transform your home",
            "Crear cuenta gratuita": "Create free account",
            "Ya tengo cuenta — Iniciar sesión": "I already have an account — Sign in",
            "Ya tengo cuenta - Iniciar sesión": "I already have an account - Sign in",
            "Sin compromisos": "No commitment",
            "Continuar explorando": "Continue exploring",
            "A toda Guatemala": "Across Guatemala",
            "Garantía de 1 año": "1-year warranty",
            "Garantia de 1 año": "1-year warranty",
            "Por defecto de fábrica": "For factory defects",
            "Por defecto de fabrica": "For factory defects",
            "Artesanal": "Handcrafted",
            "Fabricado a mano": "Handmade",
            "Devolución": "Returns",
            "Devolucion": "Returns",
            "Primeros 30 días": "First 30 days",
            "Primeros 30 dias": "First 30 days",
            "CATÁLOGO PÚBLICO": "PUBLIC CATALOG",
            "CATALOGO PUBLICO": "PUBLIC CATALOG",
            "COMPRA COMO INVITADO": "GUEST CHECKOUT",
            "← Volver al inicio": "← Back to home",
            "Volver al inicio": "Back to home",
            "Explora nuestra colección": "Explore our collection",
            "Encuentra muebles para interior y exterior con materiales de calidad, diseño elegante y una experiencia de compra sencilla.": "Find indoor and outdoor furniture with quality materials, elegant design, and a simple shopping experience.",
            "Productos disponibles": "Available products",
            "Líneas principales": "Main lines",
            "Lineas principales": "Main lines",
            "Catálogo disponible": "Catalog available",
            "Catalogo disponible": "Catalog available",
            "Catalog disponible": "Catalog available",
            "Encuentra el mueble ideal": "Find the ideal furniture",
            "Busca por nombre, referencia, tipo, color, material o categoría. Todo se carga desde la base de datos.": "Search by name, reference, type, color, material, or category. Everything loads from the database.",
            "Buscar por nombre, referencia, material o color": "Search by name, reference, material, or color",
            "Buscar por nombre, referencia, material o color...": "Search by name, reference, material, or color...",
            "Buscar muebles, referencias, materiales o colores...": "Search furniture, references, materials, or colors...",
            "Todos": "All",
            "Todas": "All",
            "Categoría": "Category",
            "Categoria": "Category",
            "Tipo": "Type",
            "Material": "Material",
            "Color": "Color",
            "Ordenar por": "Sort by",
            "Relevancia": "Relevance",
            "Nombre A-Z": "Name A-Z",
            "Precio menor a mayor": "Price low to high",
            "Precio mayor a menor": "Price high to low",
            "Mayor disponibilidad": "Highest availability",
            "Filtros activos": "Active filters",
            "Sin filtros aplicados": "No filters applied",
            "Consejo": "Tip",
            "Prueba con términos como sala, comedor, madera, nogal o el color que buscas.": "Try terms like living room, dining room, wood, walnut, or the color you are looking for.",
            "producto(s) encontrados": "product(s) found",
            "No encontramos productos con esos filtros. Intenta limpiar la búsqueda o usar otro término.": "We could not find products with those filters. Try clearing the search or using another term.",
            "No se pudo obtener el catálogo.": "Could not get the catalog.",
            "No se pudo cargar el catálogo": "Could not load the catalog",
            "Ocurrió un error al cargar los productos.": "An error occurred while loading the products.",
            "Detalle del producto": "Product detail",
            "Cargando detalle del producto...": "Loading product detail...",
            "Producto sin nombre": "Unnamed product",
            "Mueble artesanal de Muebles de los Alpes.": "Handcrafted furniture from Muebles de los Alpes.",
            "Mueble artesanal guatemalteco con detalles de calidad.": "Guatemalan handmade furniture with quality details.",
            "Sin referencia": "No reference",
            "Sin descripcion disponible.": "No description available.",
            "Sin descripción disponible.": "No description available.",
            "unidad(es) disponibles": "unit(s) available",
            "12 cuotas de": "12 payments of",
            "Ver producto": "View product",
            "Ver detalle": "View detail",
            "Ver detalles": "View details",
            "Ver todos": "View all",
            "Ver todo": "View all",
            "Producto agregado al carrito.": "Product added to cart.",
            "No se pudo agregar al carrito.": "Could not add to cart.",
            "Bienvenido de nuevo": "Welcome back",
            "Bienvenido de vuelta": "Welcome back",
            "Ingresa a tu cuenta": "Sign in to your account",
            "Accede a tu cuenta": "Access your account",
            "Usuario": "Username",
            "Username": "Username",
            "Contraseña": "Password",
            "Confirmar contraseña": "Confirm password",
            "Recordarme": "Remember me",
            "¿Olvidaste tu contraseña?": "Forgot your password?",
            "Entrar": "Sign in",
            "Ingresar": "Sign in",
            "Registrarse": "Register",
            "Crear mi cuenta": "Create my account",
            "¿No tienes cuenta?": "Don't have an account?",
            "¿Ya tienes cuenta?": "Already have an account?",
            "Ya tengo cuenta": "I already have an account",
            "Regístrate": "Register",
            "Inicia sesión": "Sign in",
            "Crear una cuenta": "Create an account",
            "Completa tus datos para comprar más rápido.": "Complete your information to shop faster.",
            "Datos personales": "Personal information",
            "Datos de acceso": "Access information",
            "Crear usuario": "Create user",
            "Tu carrito": "Your cart",
            "← Continuar comprando": "← Continue shopping",
            "Continuar comprando": "Continue shopping",
            "Puedes agregar productos sin iniciar sesión. Solo te pediremos ingresar o crear cuenta cuando quieras finalizar la compra.": "You can add products without signing in. We will only ask you to sign in or create an account when you want to complete the purchase.",
            "producto": "product",
            "Producto": "Product",
            "Guardados localmente en este navegador": "Saved locally in this browser",
            "Vaciar carrito": "Empty cart",
            "RESUMEN": "SUMMARY",
            "RESUMEN DE COMPRA": "PURCHASE SUMMARY",
            "Resumen": "Summary",
            "Resumen de compra": "Purchase summary",
            "Orden #estimado": "Estimated order",
            "Order #estimado": "Estimated order",
            "Pedido #estimado": "Estimated order",
            "Por calcular": "To be calculated",
            "Total estimado": "Estimated total",
            "Proceder al pago": "Proceed to payment",
            "Checkout": "Checkout",
            "Seguir comprando": "Keep shopping",
            "Tu carrito está vacío": "Your cart is empty",
            "Tu carrito esta vacio": "Your cart is empty",
            "Agrega productos desde el catálogo para continuar con tu compra.": "Add products from the catalog to continue your purchase.",
            "Agrega productos desde el catalogo para continuar con tu compra.": "Add products from the catalog to continue your purchase.",
            "Ir al catálogo": "Go to catalog",
            "Ir al catalogo": "Go to catalog",
            "¿Vaciar todo el carrito?": "Empty the whole cart?",
            "Deseas vaciar el carrito?": "Do you want to empty the cart?",
            "Vaciar": "Empty",
            "Carrito vaciado correctamente.": "Cart emptied successfully.",
            "No se pudo vaciar el carrito.": "Could not empty the cart.",
            "Carrito actualizado.": "Cart updated.",
            "Producto eliminado del carrito.": "Product removed from cart.",
            "No se pudo cargar el carrito.": "Could not load the cart.",
            "Error al cargar el carrito.": "Error loading the cart.",
            "Finalizar compra": "Checkout",
            "Para confirmar el pedido deberás iniciar sesión o crear una cuenta como cliente.": "To confirm the order, you must sign in or create a customer account.",
            "Compra segura": "Secure purchase",
            "Envío coordinado": "Coordinated shipping",
            "Envio coordinado": "Coordinated shipping",
            "Calidad garantizada": "Guaranteed quality",
            "PRICE": "PRICE",
            "SUBTOTAL": "SUBTOTAL",
            "INDOOR": "INDOOR",
            "OUTDOOR": "OUTDOOR",
            "Confirma tu direccion de entrega, metodo de pago y resumen del pedido.": "Confirm your delivery address, payment method, and order summary.",
            "Direccion de entrega": "Delivery address",
            "Dirección de envío": "Shipping address",
            "Tarjeta guardada": "Saved card",
            "Cupon de descuento": "Discount coupon",
            "Cupón de descuento": "Discount coupon",
            "Resumen del pedido": "Order summary",
            "Confirmar pedido": "Confirm order",
            "Hola,": "Hello,",
            "Hola, @primerNombre": "Hello, @primerNombre",
            "Cliente activo": "Active customer",
            "Cliente activo · CLI @cliIdTexto": "Active customer · CLI @cliIdTexto",
            "Ver catálogo": "View catalog",
            "Resumen de mi cuenta": "My account summary",
            "Actividad reciente": "Recent activity",
            "Ver perfil →": "View profile →",
            "TOTALES": "TOTAL",
            "EN": "ON",
            "CAMINO": "THE WAY",
            "ENTREGADOS": "DELIVERED",
            "GASTADO": "SPENT",
            "Mis órdenes recientes": "My recent orders",
            "Mis ordenes recientes": "My recent orders",
            "Seguimiento": "Tracking",
            "Tracking activo": "Active tracking",
            "Orden confirmado": "Order confirmed",
            "Orden confirmada": "Order confirmed",
            "En producción": "In production",
            "En camino": "On the way",
            "Entregado": "Delivered",
            "Entregados": "Delivered",
            "Accesos rápidos": "Quick access",
            "Mis reseñas": "My reviews",
            "Mis órdenes": "My orders",
            "Mis Ordenes": "My Orders",
            "Mis Órdenes": "My Orders",
            "Mis pedidos": "My orders",
            "Para ti": "For you",
            "Cargando órdenes...": "Loading orders...",
            "Sin órdenes recientes.": "No recent orders.",
            "Sin pedidos recientes.": "No recent orders.",
            "Aún no has realizado ningún pedido": "You have not placed any orders yet",
            "Aún no tienes reseñas": "You do not have any reviews yet",
            "Mis favoritos": "My favorites",
            "Favoritos guardados": "Saved favorites",
            "Productos guardados": "Saved products",
            "Colección personal": "Personal collection",
            "Guarda los muebles que más te gustan y vuelve a ellos cuando quieras comprar.": "Save the furniture you like most and come back to it when you want to buy.",
            "Catálogo completo": "Full catalog",
            "Catálogo avanzado": "Advanced catalog",
            "Administra preferencias visuales, notificaciones y accesos de privacidad del panel cliente.": "Manage visual preferences, notifications, and privacy access for the customer panel.",
            "Preferencias": "Preferences",
            "Estas opciones se guardan en este navegador.": "These options are saved in this browser.",
            "Notificaciones": "Notifications",
            "Mostrar avisos de pedidos y carrito.": "Show order and cart alerts.",
            "Modo oscuro del panel": "Dark panel mode",
            "Apariencia oscura para las secciones de cuenta.": "Dark appearance for account sections.",
            "Privacidad": "Privacy",
            "Consulta cómo se usan tus datos del panel.": "Check how your panel data is used.",
            "Funciones principales de tu cuenta.": "Main functions of your account.",
            "Ver notificaciones": "View notifications",
            "Contactar soporte": "Contact support",
            "Tarjetas guardadas": "Saved cards",
            "El panel cliente consulta únicamente información relacionada con tu sesión: perfil, carrito, pedidos, favoritos y tarjetas registradas.": "The customer panel only checks information related to your session: profile, cart, orders, favorites, and saved cards.",
            "No se muestran datos de otros clientes.": "No data from other customers is shown.",
            "Entendido": "Got it",
            "Notificaciones activadas": "Notifications enabled",
            "Notificaciones desactivadas": "Notifications disabled",
            "Modo oscuro activado": "Dark mode enabled",
            "Modo oscuro desactivado": "Dark mode disabled",
            "Pedido": "Order",
            "Orden": "Order",
            "Pendiente": "Pending",
            "Pendientes": "Pending",
            "En proceso": "In progress",
            "Cancelado": "Canceled",
            "Cancelada": "Canceled",
            "Preparacion del pedido": "Order preparation",
            "Preparación del pedido": "Order preparation",
            "Salida a entrega": "Out for delivery",
            "Entrega final": "Final delivery",
            "Pedido confirmado": "Order confirmed",
            "Pedido activo": "Active order",
            "Pedido entregado con éxito": "Order delivered successfully",
            "Tracking del pedido": "Order tracking",
            "Seguimiento de entrega": "Delivery tracking",
            "Consulta la línea de tiempo de tu envío y el estado actual de la entrega.": "Check the timeline of your shipment and the current delivery status.",
            "Entrega estimada": "Estimated delivery",
            "LÍNEA DE TIEMPO": "TIMELINE",
            "Línea de tiempo": "Timeline",
            "No disponible": "Not available",
            "Volver": "Back",
            "Volver a mis pedidos": "Back to my orders",
            "Volver al carrito": "Back to cart",
            "Volver al catálogo": "Back to catalog",
            "Pendiente de despacho": "Pending dispatch",
            "Fecha no disponible": "Date not available",
            "Historial": "History",
            "Historial y detalle": "History and detail",
            "Revisa productos, totales, dirección de entrega y estado actual de la orden.": "Review products, totals, delivery address, and the current order status.",
            "Detalle de pedido": "Order detail",
            "Detalle de compra": "Purchase detail",
            "Información del cliente": "Customer information",
            "Mis tarjetas": "My cards",
            "Tarjetas": "Cards",
            "Tarjetas registradas": "Registered cards",
            "Pagos guardados": "Saved payments",
            "Métodos guardados": "Saved methods",
            "Consulta, registra y administra tus tarjetas para comprar más rápido.": "View, register, and manage your cards to shop faster.",
            "Agregar tarjeta": "Add card",
            "Registrar tarjeta": "Register card",
            "Nueva tarjeta": "New card",
            "Titular": "Cardholder",
            "VISA": "VISA",
            "MASTERCARD": "MASTERCARD",
            "AMEX": "AMEX",
            "Usar como tarjeta predeterminada": "Use as default card",
            "Registrar método de pago": "Register payment method",
            "Centro de alertas": "Alert center",
            "CENTRO DE ALERTAS": "ALERT CENTER",
            "Consulta avisos generados desde tus pedidos, carrito y seguimiento de compras.": "View alerts generated from your orders, cart, and purchase tracking.",
            "Marcar todo": "Mark all",
            "Alertas": "Alerts",
            "Activas": "Active",
            "Información consultada desde la base de datos.": "Information loaded from the database.",
            "Ahora": "Now",
            "Chat de ayuda": "Help chat",
            "Asistente rápido del panel cliente.": "Quick assistant for the customer panel.",
            "Accesos de ayuda": "Help shortcuts",
            "Opciones rápidas": "Quick options",
            "Opciones rápidas de tu cuenta.": "Quick options for your account.",
            "En línea": "Online",
            "Horario de atención: lunes a viernes de 8:00 AM a 6:00 PM": "Support hours: Monday to Friday from 8:00 AM to 6:00 PM",
            "Estado de mi pedido": "Order status",
            "Problemas con pago": "Payment issues",
            "Tiempo de entrega": "Delivery time",
            "Cupones": "Coupons",
            "Hola, bienvenido a Muebles de los Alpes. ¿En qué podemos ayudarte hoy?": "Hello, welcome to Muebles de los Alpes. How can we help you today?",
            "Puedes preguntarme por pedidos, envíos, pagos, direcciones o descuentos.": "You can ask me about orders, shipments, payments, addresses, or discounts.",
            "Escribe tu mensaje...": "Write your message...",
            "Revisar mis pedidos": "Review my orders",
            "Revisar mi carrito": "Review my cart",
            "Cambiar dirección": "Change address",
            "Métodos de pago": "Payment methods",
            "Tus pedidos se consultan desde tu sesión activa y la base de datos.": "Your orders are checked from your active session and the database.",
            "Resuelve dudas sobre pedidos, envíos, pagos, dirección de entrega y promociones.": "Get help with orders, shipping, payments, delivery address, and promotions.",
            "Ayuda y preguntas": "Help and questions",
            "Resumen general del sistema": "System overview",
            "Conexión activa con la API. Los indicadores se muestran con la información disponible.": "Active API connection. Indicators are shown with the available information.",
            "Últimas 10 órdenes": "Last 10 orders",
            "Últimas órdenes": "Latest orders",
            "Estados de órdenes": "Order statuses",
            "Monitorea el estado real de las órdenes más recientes.": "Monitor the real status of the most recent orders.",
            "Distribución general y composición por estado": "General distribution and composition by status",
            "Gestión comercial": "Commercial management",
            "Gestión operativa": "Operational management",
            "Gestión de productos registrados": "Registered product management",
            "Gestión de clientes registrados": "Registered customer management",
            "Gestión de proveedores registrados": "Registered supplier management",
            "Gestión de empleados registrados": "Registered employee management",
            "Gestión de órdenes de venta": "Sales order management",
            "Gestión de órdenes de compra registradas": "Registered purchase order management",
            "Gestión de pagos de nómina": "Payroll payment management",
            "Gestión de campañas y presupuesto por canal": "Campaign and channel budget management",
            "Gestión de Cupones": "Coupon management",
            "Administra descuentos, vigencias y límites de uso.": "Manage discounts, validity dates, and usage limits.",
            "Nuevo cliente": "New customer",
            "+ Nuevo cliente": "+ New customer",
            "Nuevo producto": "New product",
            "Nuevo proveedor": "New supplier",
            "Nuevo empleado": "New employee",
            "Nueva orden": "New order",
            "Nueva orden de compra": "New purchase order",
            "Nueva nómina": "New payroll",
            "Nueva campaña": "New campaign",
            "Nuevo cupón": "New coupon",
            "Nuevo usuario": "New user",
            "Crear cupón": "Create coupon",
            "Crear producto": "Create product",
            "Crear cliente": "Create customer",
            "Crear proveedor": "Create supplier",
            "Crear empleado": "Create employee",
            "Usuarios del sistema": "System users",
            "Usuarios activos": "Active users",
            "Usuarios año 2026": "Users year 2026",
            "Preferencias del sistema y administración general": "System preferences and general administration",
            "Alertas de ventas": "Sales alerts",
            "Notificar al registrar una venta": "Notify when a sale is registered",
            "Stock bajo": "Low stock",
            "Stock bajo ≤5": "Low stock ≤5",
            "Alertar cuando el inventario sea bajo": "Alert when inventory is low",
            "Nuevos pedidos": "New orders",
            "Notificar al recibir un pedido": "Notify when an order is received",
            "Seguridad": "Security",
            "Sesión activa": "Active session",
            "Permisos": "Permissions",
            "Roles": "Roles",
            "Sistema": "System",
            "Tema visual": "Visual theme",
            "Modo claro": "Light mode",
            "Modo oscuro": "Dark mode",
            "Módulos del sistema": "System modules",
            "Gestión de roles": "Role management",
            "Zonas de envío": "Shipping zones",
            "Ver reportes": "View reports",
            "Campañas de marketing": "Marketing campaigns",
            "Cambiar el sistema entre español e inglés": "Switch the system between Spanish and English",
            "Excel editable": "Editable Excel",
            "PDF profesional": "Professional PDF",
            "Gráfica donut por estado": "Donut chart by status",
            "Gráfica pie general": "General pie chart",
            "Tendencia de ventas": "Sales trend",
            "Ventas por mes": "Sales by month",
            "Ventas por mes y comparación anual": "Sales by month and annual comparison",
            "Ventas totales": "Total sales",
            "Ventas del mes": "Monthly sales",
            "Ventas filtradas": "Filtered sales",
            "Ticket promedio": "Average ticket",
            "Compras del mes": "Monthly purchases",
            "Compras por mes": "Purchases by month",
            "Comparación por trimestre": "Quarterly comparison",
            "Año comparado": "Compared year",
            "Rango de meses": "Month range",
            "Mes inicial": "Start month",
            "Mes final": "End month",
            "Año": "Year",
            "Anual": "Annual",
            "Trimestre": "Quarter",
            "Q1 · Ene-Mar": "Q1 · Jan-Mar",
            "Q2 · Abr-Jun": "Q2 · Apr-Jun",
            "Q3 · Jul-Sep": "Q3 · Jul-Sep",
            "Q4 · Oct-Dic": "Q4 · Oct-Dec",
            "Enero": "January",
            "Febrero": "February",
            "Marzo": "March",
            "Abril": "April",
            "Mayo": "May",
            "Junio": "June",
            "Julio": "July",
            "Agosto": "August",
            "Septiembre": "September",
            "Octubre": "October",
            "Noviembre": "November",
            "Diciembre": "December",
            "Canal": "Channel",
            "Todos los canales": "All channels",
            "Seleccione canal": "Select channel",
            "Formato": "Format",
            "Columnas": "Columns",
            "GENERAL": "GENERAL",
            "EXPERIENCIA": "EXPERIENCE",
            "TU OPINIÓN": "YOUR OPINION",
            "Calificación": "Rating",
            "Comentario": "Comment",
            "Guardar reseña": "Save review",
            "Registrar reseña": "Register review",
            "Nueva reseña": "New review",
            "Consulta las valoraciones que has realizado y comparte tu opinión sobre los muebles que compraste.": "Review the ratings you have submitted and share your opinion about the furniture you purchased.",
            "Cuando valores tus productos, aparecerán en esta sección.": "When you rate your products, they will appear in this section.",
            "1 estrella": "1 star",
            "2 estrellas": "2 stars",
            "3 estrellas": "3 stars",
            "4 estrellas": "4 stars",
            "5 estrellas": "5 stars",
            "ID": "ID",
            "Imagen": "Image",
            "Imagen URL Cloudinary": "Cloudinary image URL",
            "Imagen del producto": "Product image",
            "Pega aquí la URL generada en Cloudinary para visualizarla en el listado.": "Paste the URL generated in Cloudinary to display it in the list.",
            "Peso gramos": "Weight grams",
            "Alto cm": "Height cm",
            "Ancho cm": "Width cm",
            "Profundidad cm": "Depth cm",
            "Unidad": "Unit",
            "Unidad de medida": "Unit of measure",
            "Línea": "Line",
            "Lote producto": "Product batch",
            "Stock actual": "Current stock",
            "Stock Mínimo": "Minimum stock",
            "Stock Reservado": "Reserved stock",
            "Reservado": "Reserved",
            "Inventario de productos": "Product inventory",
            "Inventario en vigilancia": "Inventory under watch",
            "Productos en alerta": "Products on alert",
            "Productos con riesgo operativo o necesidad de reposición": "Products with operational risk or replenishment needs",
            "Visualiza stock, reserva y detalle del producto en un solo lugar.": "View stock, reserve, and product detail in one place.",
            "Editar inventario": "Edit inventory",
            "Recargar estados": "Reload statuses",
            "Toca un estado para cambiar la orden": "Tap a status to change the order",
            "Estado del registro": "Record status",
            "Estado orden compra": "Purchase order status",
            "EstadoProduccionId": "ProductionStatusId",
            "Órdenes activas": "Active orders",
            "Órdenes de Venta": "Sales orders",
            "Órdenes de compra": "Purchase orders",
            "Órdenes de producción": "Production orders",
            "Total órdenes": "Total orders",
            "0 órdenes": "0 orders",
            "0 pedidos": "0 orders",
            "0 clientes": "0 customers",
            "0 registros visibles": "0 visible records",
            "Items": "Items",
            "Items vendidos": "Items sold",
            "Items visibles": "Visible items",
            "No. OC": "PO No.",
            "No. OP": "WO No.",
            "Fecha OC": "PO date",
            "No. documento": "Document No.",
            "Tipo documento": "Document type",
            "NIT": "Tax ID",
            "Razón social": "Business name",
            "Condición de pago": "Payment condition",
            "Observaciones": "Notes",
            "Fecha inicio": "Start date",
            "Fecha fin": "End date",
            "Vigencia inicio": "Start validity",
            "Vigencia fin": "End validity",
            "Presupuesto (Q)": "Budget (Q)",
            "Presupuesto total": "Total budget",
            "Monto bruto": "Gross amount",
            "Monto neto": "Net amount",
            "Total bruto": "Gross total",
            "Total neto": "Net total",
            "PENDIENTE": "PENDING",
            "PAGADO": "PAID",
            "PLANIFICADA": "PLANNED",
            "ENVIADA": "SENT",
            "OTRA": "OTHER",
            "Completadas": "Completed",
            "Canceladas": "Canceled",
            "Confirmado": "Confirmed",
            "Enviado": "Sent",
            "Sin órdenes": "No orders",
            "Sin color": "No color",
            "Sin dirección": "No address",
            "Error": "Error",
            "Error.": "Error.",
            "Error al procesar la solicitud.": "Error processing the request.",
            "Espera un momento mientras se consulta la base de datos.": "Please wait while the database is queried.",
            "Consultando información desde la base de datos.": "Checking information from the database.",
            "Sincronización lista": "Synchronization ready",
            "Calculando resumen...": "Calculating summary...",
            "Cargando carrito...": "Loading cart...",
            "Cargando catálogo...": "Loading catalog...",
            "Cargando checkout...": "Loading checkout...",
            "Cargando clientes...": "Loading customers...",
            "Cargando cupones...": "Loading coupons...",
            "Cargando detalle del pedido...": "Loading order detail...",
            "Cargando detalle...": "Loading detail...",
            "Cargando empleados...": "Loading employees...",
            "Cargando favoritos...": "Loading favorites...",
            "Cargando información de tracking...": "Loading tracking information...",
            "Cargando inventario...": "Loading inventory...",
            "Cargando notificaciones...": "Loading notifications...",
            "Cargando nóminas...": "Loading payroll...",
            "Cargando perfil...": "Loading profile...",
            "Cargando productos...": "Loading products...",
            "Cargando proveedores...": "Loading suppliers...",
            "Cargando seguimiento...": "Loading tracking...",
            "Cargando tarjetas...": "Loading cards...",
            "Cargando tus reseñas...": "Loading your reviews...",
            "Cargando usuarios...": "Loading users...",
            "Cargando órdenes de compra...": "Loading purchase orders...",
        }
    };

    var replacements = [
        ["El panel cliente consulta únicamente información relacionada con tu sesión: perfil, carrito, pedidos, favoritos y tarjetas registradas.", "The customer panel only checks information related to your session: profile, cart, orders, favorites, and saved cards."],
        ["Encuentra muebles para interior y exterior con materiales de calidad, diseño elegante y una experiencia de compra sencilla.", "Find indoor and outdoor furniture with quality materials, elegant design, and a simple shopping experience."],
        ["Puedes agregar productos sin iniciar sesión. Solo te pediremos ingresar o crear cuenta cuando quieras finalizar la compra.", "You can add products without signing in. We will only ask you to sign in or create an account when you want to complete the purchase."],
        ["Explora muebles diseñados para interiores elegantes o espacios exteriores con resistencia, comodidad y presencia visual.", "Explore furniture designed for elegant interiors or outdoor spaces with durability, comfort, and visual presence."],
        ["Creamos muebles con materiales de calidad, acabados elegantes y una experiencia de compra cómoda para nuestros clientes.", "We create furniture with quality materials, elegant finishes, and a comfortable shopping experience for our customers."],
        ["Fabricados a mano con madera guatemalteca seleccionada, acabados elegantes y presencia cálida para cada espacio.", "Handcrafted with selected Guatemalan wood, elegant finishes, and a warm presence for every space."],
        ["Colecciones pensadas para transformar espacios con equilibrio, tradición, textura y diseño contemporáneo.", "Collections designed to transform spaces with balance, tradition, texture, and contemporary design."],
        ["Busca por nombre, referencia, tipo, color, material o categoría. Todo se carga desde la base de datos.", "Search by name, reference, type, color, material, or category. Everything loads from the database."],
        ["Consulta las valoraciones que has realizado y comparte tu opinión sobre los muebles que compraste.", "Review the ratings you have submitted and share your opinion about the furniture you purchased."],
        ["No encontramos productos con esos filtros. Intenta limpiar la búsqueda o usar otro término.", "We could not find products with those filters. Try clearing the search or using another term."],
        ["Administra preferencias visuales, notificaciones y accesos de privacidad del panel cliente.", "Manage visual preferences, notifications, and privacy access for the customer panel."],
        ["Conexión activa con la API. Los indicadores se muestran con la información disponible.", "Active API connection. Indicators are shown with the available information."],
        ["Para confirmar el pedido deberás iniciar sesión o crear una cuenta como cliente.", "To confirm the order, you must sign in or create a customer account."],
        ["Resuelve dudas sobre pedidos, envíos, pagos, dirección de entrega y promociones.", "Get help with orders, shipping, payments, delivery address, and promotions."],
        ["Consulta avisos generados desde tus pedidos, carrito y seguimiento de compras.", "View alerts generated from your orders, cart, and purchase tracking."],
        ["Guarda los muebles que más te gustan y vuelve a ellos cuando quieras comprar.", "Save the furniture you like most and come back to it when you want to buy."],
        ["Prueba con términos como sala, comedor, madera, nogal o el color que buscas.", "Try terms like living room, dining room, wood, walnut, or the color you are looking for."],
        ["Revisa productos, totales, dirección de entrega y estado actual de la orden.", "Review products, totals, delivery address, and the current order status."],
        ["Consulta la línea de tiempo de tu envío y el estado actual de la entrega.", "Check the timeline of your shipment and the current delivery status."],
        ["Puedes preguntarme por pedidos, envíos, pagos, direcciones o descuentos.", "You can ask me about orders, shipments, payments, addresses, or discounts."],
        ["Pega aquí la URL generada en Cloudinary para visualizarla en el listado.", "Paste the URL generated in Cloudinary to display it in the list."],
        ["Confirma tu direccion de entrega, metodo de pago y resumen del pedido.", "Confirm your delivery address, payment method, and order summary."],
        ["Hola, bienvenido a Muebles de los Alpes. ¿En qué podemos ayudarte hoy?", "Hello, welcome to Muebles de los Alpes. How can we help you today?"],
        ["Consulta, registra y administra tus tarjetas para comprar más rápido.", "View, register, and manage your cards to shop faster."],
        ["Tus pedidos se consultan desde tu sesión activa y la base de datos.", "Your orders are checked from your active session and the database."],
        ["Visualiza stock, reserva y detalle del producto en un solo lugar.", "View stock, reserve, and product detail in one place."],
        ["Agrega productos desde el catálogo para continuar con tu compra.", "Add products from the catalog to continue your purchase."],
        ["Agrega productos desde el catalogo para continuar con tu compra.", "Add products from the catalog to continue your purchase."],
        ["Horario de atención: lunes a viernes de 8:00 AM a 6:00 PM", "Support hours: Monday to Friday from 8:00 AM to 6:00 PM"],
        ["Cuando valores tus productos, aparecerán en esta sección.", "When you rate your products, they will appear in this section."],
        ["Productos con riesgo operativo o necesidad de reposición", "Products with operational risk or replenishment needs"],
        ["Espera un momento mientras se consulta la base de datos.", "Please wait while the database is queried."],
        ["Mueble artesanal guatemalteco con detalles de calidad.", "Guatemalan handmade furniture with quality details."],
        ["Monitorea el estado real de las órdenes más recientes.", "Monitor the real status of the most recent orders."],
        ["Buscar muebles, referencias, materiales o colores...", "Search furniture, references, materials, or colors..."],
        ["Actualmente el panel está optimizado para español.", "The customer panel can be displayed in Spanish or English."],
        ["Buscar por nombre, referencia, material o color...", "Search by name, reference, material, or color..."],
        ["Administra descuentos, vigencias y límites de uso.", "Manage discounts, validity dates, and usage limits."],
        ["Dashboard administrativo de órdenes y rendimiento", "Administrative dashboard for orders and performance"],
        ["Salas, comedores, dormitorios y espacios cálidos.", "Living rooms, dining rooms, bedrooms, and warm spaces."],
        ["Preferencias del sistema y administración general", "System preferences and general administration"],
        ["Buscar por nombre, referencia, material o color", "Search by name, reference, material, or color"],
        ["Apariencia oscura para las secciones de cuenta.", "Dark appearance for account sections."],
        ["Consultando información desde la base de datos.", "Checking information from the database."],
        ["Información consultada desde la base de datos.", "Information loaded from the database."],
        ["Distribución general y composición por estado", "General distribution and composition by status"],
        ["Estas opciones se guardan en este navegador.", "These options are saved in this browser."],
        ["Completa tus datos para comprar más rápido.", "Complete your information to shop faster."],
        ["Gestión de campañas y presupuesto por canal", "Campaign and channel budget management"],
        ["Consulta cómo se usan tus datos del panel.", "Check how your panel data is used."],
        ["Ocurrió un error al cargar los productos.", "An error occurred while loading the products."],
        ["Mueble artesanal de Muebles de los Alpes.", "Handcrafted furniture from Muebles de los Alpes."],
        ["Cambiar el sistema entre español e inglés", "Switch the system between Spanish and English"],
        ["Terrazas, jardines y ambientes abiertos.", "Terraces, gardens, and open spaces."],
        ["Gestión de órdenes de compra registradas", "Registered purchase order management"],
        ["No se muestran datos de otros clientes.", "No data from other customers is shown."],
        ["Guardados localmente en este navegador", "Saved locally in this browser"],
        ["Alertar cuando el inventario sea bajo", "Alert when inventory is low"],
        ["Mostrar avisos de pedidos y carrito.", "Show order and cart alerts."],
        ["Toca un estado para cambiar la orden", "Tap a status to change the order"],
        ["No se pudieron cargar los productos", "Products could not be loaded"],
        ["Funciones principales de tu cuenta.", "Main functions of your account."],
        ["Asistente rápido del panel cliente.", "Quick assistant for the customer panel."],
        ["Cargando información de tracking...", "Loading tracking information..."],
        ["Aún no has realizado ningún pedido", "You have not placed any orders yet"],
        ["Gestión de proveedores registrados", "Registered supplier management"],
        ["Ventas por mes y comparación anual", "Sales by month and annual comparison"],
        ["Muebles que cuentan una historia", "Furniture that tells a story"],
        ["Cargando productos destacados...", "Loading featured products..."],
        ["Ya tengo cuenta — Iniciar sesión", "I already have an account — Sign in"],
        ["Ya tengo cuenta - Iniciar sesión", "I already have an account - Sign in"],
        ["Cargando detalle del producto...", "Loading product detail..."],
        ["Cliente activo · CLI @cliIdTexto", "Active customer · CLI @cliIdTexto"],
        ["Usar como tarjeta predeterminada", "Use as default card"],
        ["Gestión de productos registrados", "Registered product management"],
        ["Gestión de empleados registrados", "Registered employee management"],
        ["Notificar al registrar una venta", "Notify when a sale is registered"],
        ["Diseño para interior y exterior", "Design for indoor and outdoor spaces"],
        ["No se pudo obtener el catálogo.", "Could not get the catalog."],
        ["Producto eliminado del carrito.", "Product removed from cart."],
        ["Gestión de clientes registrados", "Registered customer management"],
        ["Error al procesar la solicitud.", "Error processing the request."],
        ["No se pudo agregar al carrito.", "Could not add to cart."],
        ["Carrito vaciado correctamente.", "Cart emptied successfully."],
        ["Opciones rápidas de tu cuenta.", "Quick options for your account."],
        ["Notificar al recibir un pedido", "Notify when an order is received"],
        ["Cargando detalle del pedido...", "Loading order detail..."],
        ["No se pudo cargar el catálogo", "Could not load the catalog"],
        ["Producto agregado al carrito.", "Product added to cart."],
        ["No se pudo vaciar el carrito.", "Could not empty the cart."],
        ["No se pudo cargar el carrito.", "Could not load the cart."],
        ["Cargando órdenes de compra...", "Loading purchase orders..."],
        ["Artesanía · Calidad · Diseño", "Craftsmanship · Quality · Design"],
        ["ARTESANÍA · CALIDAD · DISEÑO", "CRAFTSMANSHIP · QUALITY · DESIGN"],
        ["Sin descripcion disponible.", "No description available."],
        ["Sin descripción disponible.", "No description available."],
        ["Error al cargar el carrito.", "Error loading the cart."],
        ["Notificaciones desactivadas", "Notifications disabled"],
        ["Resumen general del sistema", "System overview"],
        ["Gestión de órdenes de venta", "Sales order management"],
        ["Proyecto Base de Datos II.", "Database II Project."],
        ["Pedido entregado con éxito", "Order delivered successfully"],
        ["Gestión de pagos de nómina", "Payroll payment management"],
        ["Cargando notificaciones...", "Loading notifications..."],
        ["Inspiración para tu hogar", "Inspiration for your home"],
        ["Explora nuestra colección", "Explore our collection"],
        ["Encuentra el mueble ideal", "Find the ideal furniture"],
        ["¿Olvidaste tu contraseña?", "Forgot your password?"],
        ["Deseas vaciar el carrito?", "Do you want to empty the cart?"],
        ["Comparación por trimestre", "Quarterly comparison"],
        ["COLECCIÓN ARTESANAL 2025", "ARTISAN COLLECTION 2025"],
        ["¿Vaciar todo el carrito?", "Empty the whole cart?"],
        ["Notificaciones activadas", "Notifications enabled"],
        ["Registrar método de pago", "Register payment method"],
        ["Gráfica donut por estado", "Donut chart by status"],
        ["Inventario en vigilancia", "Inventory under watch"],
        ["producto(s) encontrados", "product(s) found"],
        ["Modo oscuro desactivado", "Dark mode disabled"],
        ["Información del cliente", "Customer information"],
        ["Inventario de productos", "Product inventory"],
        ["Cargando proveedores...", "Loading suppliers..."],
        ["Cargando seguimiento...", "Loading tracking..."],
        ["Cargando tus reseñas...", "Loading your reviews..."],
        ["Por defecto de fábrica", "For factory defects"],
        ["Por defecto de fabrica", "For factory defects"],
        ["unidad(es) disponibles", "unit(s) available"],
        ["Sin órdenes recientes.", "No recent orders."],
        ["Sin pedidos recientes.", "No recent orders."],
        ["Preparacion del pedido", "Order preparation"],
        ["Preparación del pedido", "Order preparation"],
        ["Seguimiento de entrega", "Delivery tracking"],
        ["Cargando inventario...", "Loading inventory..."],
        ["Ver catálogo completo", "View full catalog"],
        ["Crear cuenta gratuita", "Create free account"],
        ["Productos disponibles", "Available products"],
        ["Sin filtros aplicados", "No filters applied"],
        ["← Continuar comprando", "← Continue shopping"],
        ["Tu carrito está vacío", "Your cart is empty"],
        ["Tu carrito esta vacio", "Your cart is empty"],
        ["Mis órdenes recientes", "My recent orders"],
        ["Mis ordenes recientes", "My recent orders"],
        ["Aún no tienes reseñas", "You do not have any reviews yet"],
        ["Modo oscuro del panel", "Dark panel mode"],
        ["Pendiente de despacho", "Pending dispatch"],
        ["Escribe tu mensaje...", "Write your message..."],
        ["Nueva orden de compra", "New purchase order"],
        ["Campañas de marketing", "Marketing campaigns"],
        ["Imagen URL Cloudinary", "Cloudinary image URL"],
        ["Órdenes de producción", "Production orders"],
        ["Calculando resumen...", "Calculating summary..."],
        ["Cargando empleados...", "Loading employees..."],
        ["Cargando favoritos...", "Loading favorites..."],
        ["Cargando productos...", "Loading products..."],
        ["Muebles de los Alpes", "Muebles de los Alpes"],
        ["Panel Administrativo", "Admin Panel"],
        ["Productos destacados", "Featured products"],
        ["Continuar explorando", "Continue exploring"],
        ["COMPRA COMO INVITADO", "GUEST CHECKOUT"],
        ["Precio menor a mayor", "Price low to high"],
        ["Precio mayor a menor", "Price high to low"],
        ["Mayor disponibilidad", "Highest availability"],
        ["Detalle del producto", "Product detail"],
        ["Bienvenido de vuelta", "Welcome back"],
        ["Confirmar contraseña", "Confirm password"],
        ["Carrito actualizado.", "Cart updated."],
        ["Direccion de entrega", "Delivery address"],
        ["Resumen de mi cuenta", "My account summary"],
        ["Modo oscuro activado", "Dark mode enabled"],
        ["Volver a mis pedidos", "Back to my orders"],
        ["Tarjetas registradas", "Registered cards"],
        ["Usuarios del sistema", "System users"],
        ["0 registros visibles", "0 visible records"],
        ["Sincronización lista", "Synchronization ready"],
        ["Cargando catálogo...", "Loading catalog..."],
        ["Cargando checkout...", "Loading checkout..."],
        ["Cargando clientes...", "Loading customers..."],
        ["Cargando tarjetas...", "Loading cards..."],
        ["Cargando usuarios...", "Loading users..."],
        ["Artesanía · Calidad", "Craftsmanship · Quality"],
        ["Atención al cliente", "Customer service"],
        ["Encuentra tu estilo", "Find your style"],
        ["Muebles artesanales", "Handcrafted furniture"],
        ["Transforma tu hogar", "Transform your home"],
        ["Catálogo disponible", "Catalog available"],
        ["Catalogo disponible", "Catalog available"],
        ["Producto sin nombre", "Unnamed product"],
        ["Bienvenido de nuevo", "Welcome back"],
        ["Ingresa a tu cuenta", "Sign in to your account"],
        ["Continuar comprando", "Continue shopping"],
        ["Calidad garantizada", "Guaranteed quality"],
        ["Hola, @primerNombre", "Hello, @primerNombre"],
        ["Cargando órdenes...", "Loading orders..."],
        ["Favoritos guardados", "Saved favorites"],
        ["Productos guardados", "Saved products"],
        ["Tracking del pedido", "Order tracking"],
        ["Fecha no disponible", "Date not available"],
        ["Historial y detalle", "History and detail"],
        ["Estado de mi pedido", "Order status"],
        ["Revisar mis pedidos", "Review my orders"],
        ["Módulos del sistema", "System modules"],
        ["Gráfica pie general", "General pie chart"],
        ["Tendencia de ventas", "Sales trend"],
        ["Imagen del producto", "Product image"],
        ["Productos en alerta", "Products on alert"],
        ["Estado del registro", "Record status"],
        ["Estado orden compra", "Purchase order status"],
        ["Cargando carrito...", "Loading cart..."],
        ["Cargando cupones...", "Loading coupons..."],
        ["Cargando detalle...", "Loading detail..."],
        ["Cargando nóminas...", "Loading payroll..."],
        ["Portal del Cliente", "Customer Portal"],
        ["Seleccionar idioma", "Select language"],
        ["Soporte de compras", "Shopping support"],
        ["Explorar colección", "Explore collection"],
        ["SELECCIÓN ESPECIAL", "SPECIAL SELECTION"],
        ["Minimalismo Cálido", "Warm Minimalism"],
        ["← Volver al inicio", "← Back to home"],
        ["Líneas principales", "Main lines"],
        ["Lineas principales", "Main lines"],
        ["Catalog disponible", "Catalog available"],
        ["Accede a tu cuenta", "Access your account"],
        ["¿No tienes cuenta?", "Don't have an account?"],
        ["¿Ya tienes cuenta?", "Already have an account?"],
        ["Dirección de envío", "Shipping address"],
        ["Cupon de descuento", "Discount coupon"],
        ["Cupón de descuento", "Discount coupon"],
        ["Resumen del pedido", "Order summary"],
        ["Actividad reciente", "Recent activity"],
        ["Colección personal", "Personal collection"],
        ["Ver notificaciones", "View notifications"],
        ["Tarjetas guardadas", "Saved cards"],
        ["Volver al catálogo", "Back to catalog"],
        ["Problemas con pago", "Payment issues"],
        ["Revisar mi carrito", "Review my cart"],
        ["Últimas 10 órdenes", "Last 10 orders"],
        ["Estados de órdenes", "Order statuses"],
        ["Gestión de Cupones", "Coupon management"],
        ["EstadoProduccionId", "ProductionStatusId"],
        ["Cargando perfil...", "Loading profile..."],
        ["Ecléctico Moderno", "Modern Eclectic"],
        ["Garantía de 1 año", "1-year warranty"],
        ["Garantia de 1 año", "1-year warranty"],
        ["RESUMEN DE COMPRA", "PURCHASE SUMMARY"],
        ["Resumen de compra", "Purchase summary"],
        ["Catálogo completo", "Full catalog"],
        ["Catálogo avanzado", "Advanced catalog"],
        ["Contactar soporte", "Contact support"],
        ["Pedido confirmado", "Order confirmed"],
        ["Volver al carrito", "Back to cart"],
        ["Detalle de pedido", "Order detail"],
        ["Detalle de compra", "Purchase detail"],
        ["Métodos guardados", "Saved methods"],
        ["Registrar tarjeta", "Register card"],
        ["Centro de alertas", "Alert center"],
        ["CENTRO DE ALERTAS", "ALERT CENTER"],
        ["Tiempo de entrega", "Delivery time"],
        ["Cambiar dirección", "Change address"],
        ["Ayuda y preguntas", "Help and questions"],
        ["Gestión comercial", "Commercial management"],
        ["Gestión operativa", "Operational management"],
        ["Usuarios año 2026", "Users year 2026"],
        ["Alertas de ventas", "Sales alerts"],
        ["Todos los canales", "All channels"],
        ["Editar inventario", "Edit inventory"],
        ["Órdenes de compra", "Purchase orders"],
        ["Condición de pago", "Payment condition"],
        ["Presupuesto total", "Total budget"],
        ["Compra protegida", "Protected purchase"],
        ["A toda Guatemala", "Across Guatemala"],
        ["Fabricado a mano", "Handmade"],
        ["Primeros 30 días", "First 30 days"],
        ["Primeros 30 dias", "First 30 days"],
        ["CATÁLOGO PÚBLICO", "PUBLIC CATALOG"],
        ["CATALOGO PUBLICO", "PUBLIC CATALOG"],
        ["Volver al inicio", "Back to home"],
        ["Crear una cuenta", "Create an account"],
        ["Datos personales", "Personal information"],
        ["Pedido #estimado", "Estimated order"],
        ["Proceder al pago", "Proceed to payment"],
        ["Seguir comprando", "Keep shopping"],
        ["Finalizar compra", "Checkout"],
        ["Envío coordinado", "Coordinated shipping"],
        ["Envio coordinado", "Coordinated shipping"],
        ["Tarjeta guardada", "Saved card"],
        ["Confirmar pedido", "Confirm order"],
        ["Orden confirmado", "Order confirmed"],
        ["Orden confirmada", "Order confirmed"],
        ["Salida a entrega", "Out for delivery"],
        ["Entrega estimada", "Estimated delivery"],
        ["Accesos de ayuda", "Help shortcuts"],
        ["Opciones rápidas", "Quick options"],
        ["Usuarios activos", "Active users"],
        ["Gestión de roles", "Role management"],
        ["Ventas filtradas", "Filtered sales"],
        ["Seleccione canal", "Select channel"],
        ["Registrar reseña", "Register review"],
        ["Unidad de medida", "Unit of measure"],
        ["Recargar estados", "Reload statuses"],
        ["Órdenes de Venta", "Sales orders"],
        ["Guardar cambios", "Save changes"],
        ["GUARDAR CAMBIOS", "SAVE CHANGES"],
        ["Limpiar filtros", "Clear filters"],
        ["Generar reporte", "Generate report"],
        ["ENVÍO SIN COSTO", "FREE SHIPPING"],
        ["Envío sin costo", "Free shipping"],
        ["TENDENCIAS 2025", "2025 TRENDS"],
        ["Estilo Colonial", "Colonial Style"],
        ["Sin compromisos", "No commitment"],
        ["Filtros activos", "Active filters"],
        ["Crear mi cuenta", "Create my account"],
        ["Ya tengo cuenta", "I already have an account"],
        ["Datos de acceso", "Access information"],
        ["Orden #estimado", "Estimated order"],
        ["Order #estimado", "Estimated order"],
        ["Tracking activo", "Active tracking"],
        ["Accesos rápidos", "Quick access"],
        ["LÍNEA DE TIEMPO", "TIMELINE"],
        ["Línea de tiempo", "Timeline"],
        ["Pagos guardados", "Saved payments"],
        ["Agregar tarjeta", "Add card"],
        ["Métodos de pago", "Payment methods"],
        ["Últimas órdenes", "Latest orders"],
        ["+ Nuevo cliente", "+ New customer"],
        ["Nuevo proveedor", "New supplier"],
        ["Crear proveedor", "Create supplier"],
        ["PDF profesional", "Professional PDF"],
        ["Ticket promedio", "Average ticket"],
        ["Compras del mes", "Monthly purchases"],
        ["Compras por mes", "Purchases by month"],
        ["Stock Reservado", "Reserved stock"],
        ["Órdenes activas", "Active orders"],
        ["Vigencia inicio", "Start validity"],
        ["Presupuesto (Q)", "Budget (Q)"],
        ["Iniciar sesión", "Sign in"],
        ["Disponibilidad", "Availability"],
        ["Método de pago", "Payment method"],
        ["Metodo de pago", "Payment method"],
        ["LÍNEA COLONIAL", "COLONIAL LINE"],
        ["Ver destacados", "View featured items"],
        ["Sin referencia", "No reference"],
        ["Vaciar carrito", "Empty cart"],
        ["Total estimado", "Estimated total"],
        ["Ir al catálogo", "Go to catalog"],
        ["Ir al catalogo", "Go to catalog"],
        ["Cliente activo", "Active customer"],
        ["Notificaciones", "Notifications"],
        ["Nuevo producto", "New product"],
        ["Nuevo empleado", "New employee"],
        ["Crear producto", "Create product"],
        ["Crear empleado", "Create employee"],
        ["Nuevos pedidos", "New orders"],
        ["Zonas de envío", "Shipping zones"],
        ["Excel editable", "Editable Excel"],
        ["Ventas por mes", "Sales by month"],
        ["Ventas totales", "Total sales"],
        ["Ventas del mes", "Monthly sales"],
        ["Rango de meses", "Month range"],
        ["Guardar reseña", "Save review"],
        ["Profundidad cm", "Depth cm"],
        ["Items vendidos", "Items sold"],
        ["Items visibles", "Visible items"],
        ["Tipo documento", "Document type"],
        ["Configuración", "Settings"],
        ["Configuraci n", "Settings"],
        ["Soporte Alpes", "Alpes Support"],
        ["Cerrar sesión", "Log out"],
        ["Cerrar sesi n", "Log out"],
        ["Administrador", "Administrator"],
        ["Estado actual", "Current status"],
        ["POR CATEGORÍA", "BY CATEGORY"],
        ["Inicia sesión", "Sign in"],
        ["Crear usuario", "Create user"],
        ["Compra segura", "Secure purchase"],
        ["En producción", "In production"],
        ["Mis favoritos", "My favorites"],
        ["Entrega final", "Final delivery"],
        ["Pedido activo", "Active order"],
        ["No disponible", "Not available"],
        ["Nueva tarjeta", "New card"],
        ["Chat de ayuda", "Help chat"],
        ["Nuevo cliente", "New customer"],
        ["Nueva campaña", "New campaign"],
        ["Nuevo usuario", "New user"],
        ["Crear cliente", "Create customer"],
        ["Stock bajo ≤5", "Low stock ≤5"],
        ["Sesión activa", "Active session"],
        ["Año comparado", "Compared year"],
        ["Lote producto", "Product batch"],
        ["Total órdenes", "Total orders"],
        ["No. documento", "Document No."],
        ["Observaciones", "Notes"],
        ["Sin dirección", "No address"],
        ["cerrarSesion", "Log out"],
        ["Crear cuenta", "Create account"],
        ["Departamento", "Department"],
        ["Ver interior", "View indoor"],
        ["Ver exterior", "View outdoor"],
        ["12 cuotas de", "12 payments of"],
        ["Ver producto", "View product"],
        ["Ver detalles", "View details"],
        ["Por calcular", "To be calculated"],
        ["Ver catálogo", "View catalog"],
        ["Ver perfil →", "View profile →"],
        ["Preferencias", "Preferences"],
        ["Mis tarjetas", "My cards"],
        ["Nueva nómina", "New payroll"],
        ["Ver reportes", "View reports"],
        ["Q1 · Ene-Mar", "Q1 · Jan-Mar"],
        ["Q2 · Abr-Jun", "Q2 · Apr-Jun"],
        ["Q3 · Jul-Sep", "Q3 · Jul-Sep"],
        ["Q4 · Oct-Dic", "Q4 · Oct-Dec"],
        ["Calificación", "Rating"],
        ["Nueva reseña", "New review"],
        ["Stock actual", "Current stock"],
        ["Stock Mínimo", "Minimum stock"],
        ["Razón social", "Business name"],
        ["Fecha inicio", "Start date"],
        ["Vigencia fin", "End validity"],
        ["Proveedores", "Suppliers"],
        ["Descripción", "Description"],
        ["Descripcion", "Description"],
        ["Ver carrito", "View cart"],
        ["Minimalismo", "Minimalism"],
        ["Ordenar por", "Sort by"],
        ["Ver detalle", "View detail"],
        ["Registrarse", "Register"],
        ["Seguimiento", "Tracking"],
        ["Mis reseñas", "My reviews"],
        ["Mis órdenes", "My orders"],
        ["Mis Ordenes", "My Orders"],
        ["Mis Órdenes", "My Orders"],
        ["Mis pedidos", "My orders"],
        ["Marcar todo", "Mark all"],
        ["Nueva orden", "New order"],
        ["Nuevo cupón", "New coupon"],
        ["Crear cupón", "Create coupon"],
        ["Tema visual", "Visual theme"],
        ["Modo oscuro", "Dark mode"],
        ["Mes inicial", "Start month"],
        ["EXPERIENCIA", "EXPERIENCE"],
        ["2 estrellas", "2 stars"],
        ["3 estrellas", "3 stars"],
        ["4 estrellas", "4 stars"],
        ["5 estrellas", "5 stars"],
        ["Peso gramos", "Weight grams"],
        ["Monto bruto", "Gross amount"],
        ["Total bruto", "Gross total"],
        ["PLANIFICADA", "PLANNED"],
        ["Completadas", "Completed"],
        ["Sin órdenes", "No orders"],
        ["Inventario", "Inventory"],
        ["Producción", "Production"],
        ["Produccion", "Production"],
        ["Referencia", "Reference"],
        ["Disponible", "Available"],
        ["Actualizar", "Refresh"],
        ["Devolución", "Returns"],
        ["Devolucion", "Returns"],
        ["Relevancia", "Relevance"],
        ["Nombre A-Z", "Name A-Z"],
        ["Contraseña", "Password"],
        ["Recordarme", "Remember me"],
        ["Regístrate", "Register"],
        ["Tu carrito", "Your cart"],
        ["ENTREGADOS", "DELIVERED"],
        ["Entregados", "Delivered"],
        ["Privacidad", "Privacy"],
        ["Pendientes", "Pending"],
        ["En proceso", "In progress"],
        ["MASTERCARD", "MASTERCARD"],
        ["Stock bajo", "Low stock"],
        ["Modo claro", "Light mode"],
        ["Septiembre", "September"],
        ["TU OPINIÓN", "YOUR OPINION"],
        ["Comentario", "Comment"],
        ["1 estrella", "1 star"],
        ["0 clientes", "0 customers"],
        ["Monto neto", "Net amount"],
        ["Total neto", "Net total"],
        ["Canceladas", "Canceled"],
        ["Confirmado", "Confirmed"],
        ["Favoritos", "Favorites"],
        ["Mi perfil", "My profile"],
        ["MI CUENTA", "MY ACCOUNT"],
        ["Mi cuenta", "My account"],
        ["Guatemala", "Guatemala"],
        ["Comercial", "Commercial"],
        ["COMERCIAL", "COMMERCIAL"],
        ["Operativa", "Operations"],
        ["OPERATIVA", "OPERATIONS"],
        ["Dashboard", "Dashboard"],
        ["Productos", "Products"],
        ["Empleados", "Employees"],
        ["Marketing", "Marketing"],
        ["Apellidos", "Last name"],
        ["Dirección", "Address"],
        ["Direccion", "Address"],
        ["Descuento", "Discount"],
        ["Continuar", "Continue"],
        ["Siguiente", "Next"],
        ["TENDENCIA", "TREND"],
        ["Artesanal", "Handcrafted"],
        ["Categoría", "Category"],
        ["Categoria", "Category"],
        ["Ver todos", "View all"],
        ["En camino", "On the way"],
        ["Entregado", "Delivered"],
        ["Entendido", "Got it"],
        ["Pendiente", "Pending"],
        ["Cancelado", "Canceled"],
        ["Cancelada", "Canceled"],
        ["Historial", "History"],
        ["Seguridad", "Security"],
        ["Mes final", "End month"],
        ["Trimestre", "Quarter"],
        ["Noviembre", "November"],
        ["Diciembre", "December"],
        ["Reservado", "Reserved"],
        ["0 órdenes", "0 orders"],
        ["0 pedidos", "0 orders"],
        ["Fecha fin", "End date"],
        ["PENDIENTE", "PENDING"],
        ["Sin color", "No color"],
        ["Catálogo", "Catalog"],
        ["Catalogo", "Catalog"],
        ["Cat logo", "Catalog"],
        ["Language", "Language"],
        ["Contacto", "Contact"],
        ["Clientes", "Customers"],
        ["Reportes", "Reports"],
        ["Acciones", "Actions"],
        ["Inactivo", "Inactive"],
        ["INACTIVO", "INACTIVE"],
        ["Teléfono", "Phone"],
        ["Telefono", "Phone"],
        ["Cantidad", "Quantity"],
        ["Subtotal", "Subtotal"],
        ["Impuesto", "Tax"],
        ["Cancelar", "Cancel"],
        ["Anterior", "Previous"],
        ["Exportar", "Export"],
        ["Recargar", "Reload"],
        ["Eliminar", "Delete"],
        ["Interior", "Indoor"],
        ["Exterior", "Outdoor"],
        ["INTERIOR", "INDOOR"],
        ["EXTERIOR", "OUTDOOR"],
        ["Explorar", "Explore"],
        ["Material", "Material"],
        ["Ver todo", "View all"],
        ["Username", "Username"],
        ["Ingresar", "Sign in"],
        ["Producto", "Product"],
        ["Checkout", "Checkout"],
        ["SUBTOTAL", "SUBTOTAL"],
        ["Tarjetas", "Cards"],
        ["En línea", "Online"],
        ["Permisos", "Permissions"],
        ["Columnas", "Columns"],
        ["Ancho cm", "Width cm"],
        ["Fecha OC", "PO date"],
        ["Órdenes", "Orders"],
        ["Ordenes", "Orders"],
        ["ORDENES", "ORDERS"],
        ["Carrito", "Cart"],
        ["Soporte", "Support"],
        ["Español", "Spanish"],
        ["English", "English"],
        ["Compras", "Purchases"],
        ["Nombres", "First name"],
        ["Guardar", "Save"],
        ["GUARDAR", "SAVE"],
        ["Limpiar", "Clear"],
        ["Filtrar", "Filter"],
        ["Generar", "Generate"],
        ["Aplicar", "Apply"],
        ["Agregar", "Add"],
        ["Consejo", "Tip"],
        ["Usuario", "Username"],
        ["RESUMEN", "SUMMARY"],
        ["Resumen", "Summary"],
        ["OUTDOOR", "OUTDOOR"],
        ["TOTALES", "TOTAL"],
        ["GASTADO", "SPENT"],
        ["Para ti", "For you"],
        ["Titular", "Cardholder"],
        ["Alertas", "Alerts"],
        ["Activas", "Active"],
        ["Cupones", "Coupons"],
        ["Sistema", "System"],
        ["Febrero", "February"],
        ["Octubre", "October"],
        ["Formato", "Format"],
        ["GENERAL", "GENERAL"],
        ["Alto cm", "Height cm"],
        ["ENVIADA", "SENT"],
        ["Enviado", "Sent"],
        ["Inicio", "Home"],
        ["Perfil", "Profile"],
        ["Idioma", "Language"],
        ["Inglés", "English"],
        ["Tienda", "Store"],
        ["TIENDA", "STORE"],
        ["Cuenta", "Account"],
        ["Nómina", "Payroll"],
        ["Nomina", "Payroll"],
        ["Activo", "Active"],
        ["ACTIVO", "ACTIVE"],
        ["Estado", "Status"],
        ["Nombre", "Name"],
        ["Correo", "Email"],
        ["Ciudad", "City"],
        ["Código", "Code"],
        ["Codigo", "Code"],
        ["Precio", "Price"],
        ["Cerrar", "Close"],
        ["Enviar", "Send"],
        ["Buscar", "Search"],
        ["Editar", "Edit"],
        ["Cálido", "Warm"],
        ["Entrar", "Sign in"],
        ["Vaciar", "Empty"],
        ["INDOOR", "INDOOR"],
        ["CAMINO", "THE WAY"],
        ["Pedido", "Order"],
        ["Volver", "Back"],
        ["Agosto", "August"],
        ["Imagen", "Image"],
        ["Unidad", "Unit"],
        ["No. OC", "PO No."],
        ["No. OP", "WO No."],
        ["PAGADO", "PAID"],
        ["Error.", "Error."],
        ["Fecha", "Date"],
        ["Email", "Email"],
        ["Stock", "Stock"],
        ["Total", "Total"],
        ["Envío", "Shipping"],
        ["Envio", "Shipping"],
        ["Nuevo", "New"],
        ["Nueva", "New"],
        ["Crear", "Create"],
        ["Todos", "All"],
        ["Todas", "All"],
        ["Color", "Color"],
        ["PRICE", "PRICE"],
        ["Hola,", "Hello,"],
        ["Orden", "Order"],
        ["Ahora", "Now"],
        ["Roles", "Roles"],
        ["Anual", "Annual"],
        ["Enero", "January"],
        ["Marzo", "March"],
        ["Abril", "April"],
        ["Junio", "June"],
        ["Julio", "July"],
        ["Canal", "Channel"],
        ["Línea", "Line"],
        ["Items", "Items"],
        ["Error", "Error"],
        ["País", "Country"],
        ["Tipo", "Type"],
        ["VISA", "VISA"],
        ["AMEX", "AMEX"],
        ["Mayo", "May"],
        ["OTRA", "OTHER"],
        ["VIP", "VIP"],
        ["Año", "Year"],
        ["NIT", "Tax ID"],
        ["EN", "ON"],
        ["ID", "ID"],
    ];

    replacements.sort(function (a, b) {
        return b[0].length - a[0].length;
    });

    function normalizeLanguage(value) {
        return value === "en" || value === "English" || value === "Inglés" || value === "Ingles" ? "Inglés" : "Español";
    }

    function langToCode(value) {
        return value === "Inglés" || value === "Ingles" || value === "English" || value === "en" ? "en" : "es";
    }

    function getLangCode() {
        var saved = localStorage.getItem(STORAGE_KEY);

        if (!saved) {
            var adminSaved = localStorage.getItem(ADMIN_STORAGE_KEY);
            if (adminSaved) {
                saved = adminSaved;
            }
        }

        return langToCode(saved || "Español");
    }

    function preserveCase(original, translated) {
        if (!original || !translated) return translated;
        if (original === original.toUpperCase()) return translated.toUpperCase();
        return translated;
    }

    function applyRegexRules(text) {
        var out = text;

        out = out.replace(/^(\d+)\s+ordenes$/i, "$1 orders");
        out = out.replace(/^(\d+)\s+órdenes$/i, "$1 orders");
        out = out.replace(/^(\d+)\s+pedidos$/i, "$1 orders");
        out = out.replace(/^(\d+)\s+producto$/i, "$1 product");
        out = out.replace(/^(\d+)\s+productos$/i, "$1 products");

        out = out.replace(/^Orden\s*#?\s*estimado$/i, "Estimated order");
        out = out.replace(/^Order\s*#?\s*estimado$/i, "Estimated order");
        out = out.replace(/^Pedido\s*#?\s*estimado$/i, "Estimated order");

        out = out.replace(/^Orden\s*#?/i, "Order #");
        out = out.replace(/^Pedido\s*#?/i, "Order #");

        out = out.replace(/^Hola,\s*/i, "Hello, ");

        return out;
    }

    function translateText(value) {
        var lang = getLangCode();

        var originalValue = String(value == null ? "" : value);
        var leading = originalValue.match(/^\s*/)[0];
        var trailing = originalValue.match(/\s*$/)[0];
        var text = originalValue.trim();
        var normalizedText = text.replace(/\s+/g, " ");
        var out = text;

        if (!text) return value;

        if (lang === "es") {
            return value;
        }

        if (dictionary.en[text]) {
            return leading + dictionary.en[text] + trailing;
        }

        if (dictionary.en[normalizedText]) {
            return leading + dictionary.en[normalizedText] + trailing;
        }

        out = applyRegexRules(out);

        replacements.forEach(function (pair) {
            var from = pair[0];
            var to = pair[1];

            if (out.indexOf(from) >= 0) {
                out = out.split(from).join(preserveCase(from, to));
            }

            var normalizedOut = out.replace(/\s+/g, " ");
            if (normalizedOut.indexOf(from) >= 0) {
                out = normalizedOut.split(from).join(preserveCase(from, to));
            }
        });

        return leading + out + trailing;
    }

    function shouldIgnoreElement(el) {
        if (!el) return true;

        var tag = el.tagName;

        if (
            tag === "SCRIPT" ||
            tag === "STYLE" ||
            tag === "NOSCRIPT" ||
            tag === "TEXTAREA" ||
            tag === "CODE" ||
            tag === "PRE"
        ) {
            return true;
        }

        if (el.closest && el.closest("[data-no-translate], .notranslate, .pc-language-menu")) {
            return true;
        }

        return false;
    }

    function translateDataI18n(root) {
        var elements = [];

        if (root.nodeType === 1 && root.hasAttribute && root.hasAttribute("data-i18n")) {
            elements.push(root);
        }

        if (root.querySelectorAll) {
            elements = elements.concat(Array.from(root.querySelectorAll("[data-i18n]")));
        }

        elements.forEach(function (el) {
            if (shouldIgnoreElement(el)) return;

            var key = el.getAttribute("data-i18n");
            if (!key) return;

            if (el.__pcOriginalHtml === undefined) {
                el.__pcOriginalHtml = el.innerHTML;
            }

            var translated = translateText(key);

            if (getLangCode() === "es") {
                el.innerHTML = el.__pcOriginalHtml;
                return;
            }

            if (translated && translated !== key) {
                el.innerHTML = translated;
            }
        });
    }

    function translateNodeText(root) {
        var walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT, {
            acceptNode: function (node) {
                if (!node.nodeValue || !node.nodeValue.trim()) return NodeFilter.FILTER_REJECT;
                if (shouldIgnoreElement(node.parentElement)) return NodeFilter.FILTER_REJECT;

                if (node.parentElement && node.parentElement.closest("[data-i18n]")) {
                    return NodeFilter.FILTER_REJECT;
                }

                return NodeFilter.FILTER_ACCEPT;
            }
        });

        var nodes = [];

        while (walker.nextNode()) {
            nodes.push(walker.currentNode);
        }

        nodes.forEach(function (node) {
            if (node.__pcOriginalText === undefined) {
                node.__pcOriginalText = node.nodeValue;
            }

            node.nodeValue = getLangCode() === "es"
                ? node.__pcOriginalText
                : translateText(node.__pcOriginalText);
        });
    }

    function translateAttributes(root) {
        var attrs = ["placeholder", "title", "aria-label", "alt"];
        var elements = root.querySelectorAll ? Array.prototype.slice.call(root.querySelectorAll("*")) : [];

        if (root.nodeType === 1) {
            elements.push(root);
        }

        elements.forEach(function (el) {
            if (shouldIgnoreElement(el)) return;

            attrs.forEach(function (attr) {
                if (!el.hasAttribute(attr)) return;

                var key = "__pcOriginal_" + attr;

                if (el[key] === undefined) {
                    el[key] = el.getAttribute(attr);
                }

                el.setAttribute(attr, getLangCode() === "es" ? el[key] : translateText(el[key]));
            });

            var tag = el.tagName;

            if ((tag === "INPUT" || tag === "BUTTON") && el.hasAttribute("value")) {
                if (el.__pcOriginalValue === undefined) {
                    el.__pcOriginalValue = el.getAttribute("value");
                }

                el.setAttribute("value", getLangCode() === "es" ? el.__pcOriginalValue : translateText(el.__pcOriginalValue));
            }
        });
    }

    function updateLanguageLabels() {
        var lang = getLangCode();

        document.documentElement.lang = lang;

        var cfgLabel = document.getElementById("cfgIdiomaTexto");
        if (cfgLabel) {
            cfgLabel.textContent = lang === "en" ? "English" : "Español";
        }

        var adminLabel = document.getElementById("adminLangLabel");
        if (adminLabel) {
            adminLabel.textContent = lang === "en" ? "EN" : "ES";
        }

        document.querySelectorAll("[data-current-lang]").forEach(function (el) {
            el.textContent = lang === "en" ? "English" : "Español";
        });

        if (document.title) {
            if (document.__pcOriginalTitle === undefined) {
                document.__pcOriginalTitle = document.title;
            }

            document.title = lang === "es"
                ? document.__pcOriginalTitle
                : translateText(document.__pcOriginalTitle);
        }
    }

    function toggleMenu(switcher) {
        var menu = switcher.querySelector(".lang-switcher-menu");
        var isOpen = switcher.classList.contains("open");

        document.querySelectorAll(".lang-switcher.open").forEach(function (item) {
            item.classList.remove("open");
            var itemMenu = item.querySelector(".lang-switcher-menu");
            if (itemMenu) itemMenu.style.display = "none";
        });

        if (!isOpen) {
            switcher.classList.add("open");
            if (menu) menu.style.display = "block";
        }
    }

    function activarBotonesIdioma() {
        document.querySelectorAll(".lang-switcher").forEach(function (switcher) {
            var btn = switcher.querySelector(".lang-switcher-btn");

            if (!btn || btn.dataset.portalIdiomaReady === "1") return;

            btn.dataset.portalIdiomaReady = "1";

            btn.addEventListener("click", function (e) {
                e.preventDefault();
                e.stopPropagation();
                toggleMenu(switcher);
            });
        });

        document.querySelectorAll("[data-lang-option], [data-lang], [data-idioma]").forEach(function (option) {
            if (option.dataset.portalIdiomaReady === "1") return;

            option.dataset.portalIdiomaReady = "1";

            option.addEventListener("click", function (e) {
                var raw = option.getAttribute("data-lang-option") ||
                    option.getAttribute("data-lang") ||
                    option.getAttribute("data-idioma") ||
                    option.textContent;

                if (!raw) return;

                e.preventDefault();
                e.stopPropagation();

                setLanguage(raw);

                document.querySelectorAll(".lang-switcher.open").forEach(function (sw) {
                    sw.classList.remove("open");
                    var menu = sw.querySelector(".lang-switcher-menu");
                    if (menu) menu.style.display = "none";
                });

                setTimeout(function () {
                    window.location.reload();
                }, 250);
            });
        });
    }

    function applyLanguage(root) {
        if (isApplying) return;

        root = root || document.body;

        if (!root) return;

        isApplying = true;

        try {
            if (root.nodeType === 3) {
                if (root.__pcOriginalText === undefined) {
                    root.__pcOriginalText = root.nodeValue;
                }

                root.nodeValue = getLangCode() === "es"
                    ? root.__pcOriginalText
                    : translateText(root.__pcOriginalText);
            } else if (root.nodeType === 1) {
                translateDataI18n(root);
                translateNodeText(root);
                translateAttributes(root);
            }

            updateLanguageLabels();
        } catch (error) {
            console.error("Error aplicando idioma:", error);
        } finally {
            isApplying = false;
        }
    }

    function setLanguage(value) {
        var normalized = normalizeLanguage(value);
        var code = langToCode(normalized);

        localStorage.setItem(STORAGE_KEY, normalized);
        localStorage.setItem(ADMIN_STORAGE_KEY, code);

        applyLanguage(document.body);

        setTimeout(function () {
            applyLanguage(document.body);
        }, 100);
    }

    window.PortalIdioma = {
        apply: applyLanguage,
        set: setLanguage,
        get: getLangCode,
        text: translateText
    };

    document.addEventListener("DOMContentLoaded", function () {
        activarBotonesIdioma();
        applyLanguage(document.body);

        document.addEventListener("click", function () {
            document.querySelectorAll(".lang-switcher.open").forEach(function (sw) {
                sw.classList.remove("open");
                var menu = sw.querySelector(".lang-switcher-menu");
                if (menu) menu.style.display = "none";
            });
        });

        var observer = new MutationObserver(function (mutations) {
            if (getLangCode() !== "en") return;

            clearTimeout(observerTimer);

            observerTimer = setTimeout(function () {
                mutations.forEach(function (mutation) {
                    Array.prototype.forEach.call(mutation.addedNodes, function (node) {
                        if (node.nodeType === 1) {
                            activarBotonesIdioma();
                            applyLanguage(node);
                        }
                    });
                });

                updateLanguageLabels();
            }, 180);
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    });
})();
