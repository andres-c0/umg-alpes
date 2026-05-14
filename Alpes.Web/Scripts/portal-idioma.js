(function () {
    "use strict";

    var STORAGE_KEY = "pc_cfg_idioma";
    var isApplying = false;
    var observerTimer = null;

    var dictionary = {
        en: {

            "home.hero1.title": "<span>Furniture that</span><span>tells a</span><span>story</span>",
            "home.hero1.text": "Handcrafted with selected Guatemalan wood, elegant finishes, and a warm presence for every space.",

            "home.hero2.title": "Heritage<br />you can<br />feel",
            "home.hero2.text": "Colonial design with handcrafted finishes, resistant materials, and a style made for homes with character.",

            "home.hero3.title": "All across<br />Guatemala<br />free of charge",
            "home.hero3.text": "We deliver to all 22 departments with a simple, safe, and customer-focused shopping experience.",

            "POR CATEGORÍA": "BY CATEGORY",
            "Encuentra tu estilo": "Find your style",
            "Explora muebles diseñados para interiores elegantes o espacios exteriores con resistencia, comodidad y presencia visual.": "Explore furniture designed for elegant interiors or outdoor spaces with durability, comfort, and visual presence.",
            "Interior": "Indoor",
            "Exterior": "Outdoor",
            "Salas, comedores, dormitorios y espacios cálidos.": "Living rooms, dining rooms, bedrooms, and warm spaces.",
            "Terrazas, jardines y ambientes abiertos.": "Terraces, gardens, and open spaces.",
            "Explorar": "Explore",
            "SELECCIÓN ESPECIAL": "SPECIAL SELECTION",
            "Productos destacados": "Featured products",
            "Ver catálogo completo": "View full catalog",
            "Cargando productos destacados...": "Loading featured products...",
            "12 cuotas de": "12 payments of",
            // GENERAL / LAYOUT
            "Muebles de los Alpes": "Muebles de los Alpes",
            "Artesanía · Calidad": "Craftsmanship · Quality",
            "Artesanía · Calidad · Diseño": "Craftsmanship · Quality · Design",
            "Panel Administrativo": "Admin Panel",
            "Proyecto Base de Datos II.": "Database II Project.",
            "Inicio": "Home",
            "Catálogo": "Catalog",
            "Catalogo": "Catalog",
            "Favoritos": "Favorites",
            "Órdenes": "Orders",
            "Ordenes": "Orders",
            "Carrito": "Cart",
            "Perfil": "Profile",
            "Mi perfil": "My profile",
            "Configuración": "Settings",
            "Soporte": "Support",
            "Soporte Alpes": "Alpes Support",
            "Cerrar sesión": "Log out",
            "Iniciar sesión": "Sign in",
            "Crear cuenta": "Create account",
            "Idioma": "Language",
            "Español": "Spanish",
            "Inglés": "English",
            "Cambiar idioma": "Change language",
            "Seleccionar idioma": "Select language",
            "Elige el idioma en el que deseas ver el sistema.": "Choose the language you want to use for the system.",
            "Elige el idioma en el que deseas ver el panel cliente.": "Choose the language you want to use for the customer panel.",
            "Idioma cambiado a Español": "Language changed to Spanish",
            "Idioma cambiado a Inglés": "Language changed to English",
            "Tienda": "Store",
            "TIENDA": "STORE",
            "MI CUENTA": "MY ACCOUNT",
            "Mi cuenta": "My account",
            "Cuenta": "Account",
            "Contacto": "Contact",
            "Atención al cliente": "Customer service",
            "Soporte de compras": "Shopping support",
            "Guatemala": "Guatemala",

            // HOME PÚBLICO
            "COLECCIÓN ARTESANAL 2025": "ARTISAN COLLECTION 2025",
            "Muebles que cuentan una historia": "Furniture that tells a story",
            "Fabricados a mano con madera guatemalteca seleccionada, acabados elegantes y presencia cálida para cada espacio.": "Handcrafted with selected Guatemalan wood, elegant finishes, and a warm presence for every space.",
            "Explorar colección": "Explore collection",
            "Ver destacados": "View featured items",
            "Descubre piezas únicas": "Discover unique pieces",
            "Productos destacados": "Featured products",
            "No hay productos disponibles": "No products available",
            "Cuando existan productos activos, se mostrarán en esta sección.": "When active products exist, they will be shown in this section.",
            "Envío sin costo": "Free shipping",
            "Compra protegida": "Protected purchase",
            "Muebles artesanales": "Handcrafted furniture",
            "Diseño para interior y exterior": "Design for indoor and outdoor spaces",
            "Creamos muebles con materiales de calidad, acabados elegantes y una experiencia de compra cómoda para nuestros clientes.": "We create furniture with quality materials, elegant finishes, and a comfortable shopping experience for our customers.",

            // LOGIN / REGISTRO
            "Bienvenido de nuevo": "Welcome back",
            "Ingresa a tu cuenta": "Sign in to your account",
            "Accede a tu cuenta": "Access your account",
            "Usuario": "Username",
            "Correo": "Email",
            "Correo electrónico": "Email",
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
            "Regístrate": "Register",
            "Inicia sesión": "Sign in",
            "Nombre": "Name",
            "Nombres": "First name",
            "Apellidos": "Last name",
            "Teléfono": "Phone",
            "Telefono": "Phone",
            "Dirección": "Address",
            "Direccion": "Address",
            "Crear una cuenta": "Create an account",
            "Completa tus datos para comprar más rápido.": "Complete your information to shop faster.",
            "Volver al inicio": "Back to home",
            "Datos personales": "Personal information",
            "Datos de acceso": "Access information",
            "Crear usuario": "Create user",
            "Ya tengo cuenta": "I already have an account",

            // CATÁLOGO PÚBLICO
            "Encuentra el mueble ideal": "Find the ideal furniture",
            "Busca por nombre, referencia, tipo, color, material o categoría. Todo se carga desde la base de datos.": "Search by name, reference, type, color, material, or category. Everything loads from the database.",
            "Buscar muebles, referencias, materiales o colores...": "Search furniture, references, materials, or colors...",
            "Buscar muebles, salas, comedores...": "Search furniture, living rooms, dining sets...",
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
            "Limpiar filtros": "Clear filters",
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
            "Mueble artesanal guatemalteco con detalles de calidad.": "Guatemalan handmade furniture with quality details.",
            "Sin referencia": "No reference",
            "Sin descripcion disponible.": "No description available.",
            "Sin descripción disponible.": "No description available.",
            "Descripción": "Description",
            "Precio": "Price",
            "Referencia": "Reference",
            "Disponible": "Available",
            "Disponibilidad": "Availability",
            "Stock": "Stock",
            "unidad(es) disponibles": "unit(s) available",
            "Ver producto": "View product",
            "Ver detalle": "View detail",
            "Ver detalles": "View details",
            "Ver detalles →": "View details →",
            "Ver detalle →": "View detail →",
            "Ver todos": "View all",
            "Ver todos →": "View all →",
            "Ver todo": "View all",
            "Agregar": "Add",
            "Agregar al carrito": "Add to cart",
            "Agregar producto demo": "Add demo product",
            "Producto agregado al carrito.": "Product added to cart.",
            "No se pudo agregar al carrito.": "Could not add to cart.",

            // CARRITO / CHECKOUT
            "Tu carrito": "Your cart",
            "← Continuar comprando": "← Continue shopping",
            "Continuar comprando": "Continue shopping",
            "Puedes agregar productos sin iniciar sesión. Solo te pediremos ingresar o crear cuenta cuando quieras finalizar la compra.": "You can add products without signing in. We will only ask you to sign in or create an account when you want to complete the purchase.",
            "producto": "product",
            "productos": "products",
            "Productos": "Products",
            "Producto": "Product",
            "Guardados localmente en este navegador": "Saved locally in this browser",
            "Vaciar carrito": "Empty cart",
            "RESUMEN": "SUMMARY",
            "Resumen": "Summary",
            "Resumen de compra": "Purchase summary",
            "estimado": "estimated",
            "Subtotal": "Subtotal",
            "IVA 12%": "VAT 12%",
            "Descuento": "Discount",
            "Discount": "Discount",
            "Total": "Total",
            "Proceder al pago": "Proceed to payment",
            "Seguir comprando": "Keep shopping",
            "Tu carrito está vacío": "Your cart is empty",
            "Tu carrito esta vacio": "Your cart is empty",
            "Agrega productos desde el catálogo para continuar con tu compra.": "Add products from the catalog to continue your purchase.",
            "Agrega productos desde el catalogo para continuar con tu compra.": "Add products from the catalog to continue your purchase.",
            "Ir al catálogo": "Go to catalog",
            "Ir al catalogo": "Go to catalog",
            "Eliminar": "Delete",
            "¿Vaciar todo el carrito?": "Empty the whole cart?",
            "Vaciar": "Empty",
            "No hay productos para eliminar.": "There are no products to delete.",
            "Carrito vaciado correctamente.": "Cart emptied successfully.",
            "No se pudo vaciar el carrito.": "Could not empty the cart.",
            "Carrito actualizado.": "Cart updated.",
            "Producto eliminado del carrito.": "Product removed from cart.",
            "No se pudo cargar el carrito.": "Could not load the cart.",
            "Error al cargar el carrito.": "Error loading the cart.",
            "Finalizar compra": "Checkout",
            "Checkout": "Checkout",
            "Confirma tu dirección de entrega, método de pago y resumen del pedido.": "Confirm your delivery address, payment method, and order summary.",
            "Confirma tu direccion de entrega, metodo de pago y resumen del pedido.": "Confirm your delivery address, payment method, and order summary.",
            "Volver al carrito": "Back to cart",
            "Dirección de entrega": "Delivery address",
            "Direccion de entrega": "Delivery address",
            "Método de pago": "Payment method",
            "Metodo de pago": "Payment method",
            "Tarjeta guardada": "Saved card",
            "Cupón de descuento": "Discount coupon",
            "Cupon de descuento": "Discount coupon",
            "Aplicar": "Apply",
            "Resumen del pedido": "Order summary",
            "Confirmar pedido": "Confirm order",
            "CONFIRMAR PEDIDO": "CONFIRM ORDER",
            "Tu pedido se registrará en órdenes, detalle, pago y envío usando la base de datos.": "Your order will be registered in orders, detail, payment, and shipping using the database.",
            "Tu pedido se registrará en ordenes, detalle, pago y envío usando la base de datos.": "Your order will be registered in orders, detail, payment, and shipping using the database.",

            // PANEL CLIENTE
            "Hola,": "Hello,",
            "Cliente activo": "Active customer",
            "Cliente activo...": "Active customer...",
            "Ver catálogo": "View catalog",
            "Resumen de mi cuenta": "My account summary",
            "Actividad reciente": "Recent activity",
            "Ver perfil →": "View profile →",
            "ORDENES": "ORDERS",
            "ÓRDENES": "ORDERS",
            "ORDENES TOTALES": "TOTAL ORDERS",
            "ÓRDENES TOTALES": "TOTAL ORDERS",
            "TOTALES": "TOTAL",
            "EN": "ON",
            "CAMINO": "THE WAY",
            "ENTREGADOS": "DELIVERED",
            "GASTADO": "SPENT",
            "Compras": "Purchases",
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

            // CONFIGURACIÓN CLIENTE
            "Administra preferencias visuales, notificaciones y accesos de privacidad del panel cliente.": "Manage visual preferences, notifications, and privacy access for the customer panel.",
            "Preferencias": "Preferences",
            "Estas opciones se guardan en este navegador.": "These options are saved in this browser.",
            "Notificaciones": "Notifications",
            "Mostrar avisos de pedidos y carrito.": "Show order and cart alerts.",
            "Modo oscuro del panel": "Dark mode",
            "Apariencia oscura para las secciones de cuenta.": "Dark appearance for account sections.",
            "Privacidad": "Privacy",
            "Consulta cómo se usan tus datos del panel.": "Check how your panel data is used.",
            "Accesos rápidos": "Quick access",
            "Funciones principales de tu cuenta.": "Main functions of your account.",
            "Ver notificaciones": "View notifications",
            "Contactar soporte": "Contact support",
            "Tarjetas guardadas": "Saved cards",
            "Actualmente el panel está optimizado para español.": "The customer panel can be displayed in Spanish or English.",
            "El panel cliente consulta únicamente información relacionada con tu sesión: perfil, carrito, pedidos, favoritos y tarjetas registradas.": "The customer panel only checks information related to your session: profile, cart, orders, favorites, and saved cards.",
            "No se muestran datos de otros clientes.": "No data from other customers is shown.",
            "Entendido": "Got it",
            "Notificaciones activadas": "Notifications enabled",
            "Notificaciones desactivadas": "Notifications disabled",
            "Modo oscuro activado": "Dark mode enabled",
            "Modo oscuro desactivado": "Dark mode disabled",

            // ÓRDENES / TRACKING
            "Pedido": "Order",
            "Orden": "Order",
            "Fecha": "Date",
            "Estado": "Status",
            "Pendiente": "Pending",
            "Pendientes": "Pending",
            "En proceso": "In progress",
            "Delivered": "Delivered",
            "Cancelado": "Canceled",
            "Cancelada": "Canceled",
            "Activo": "Active",
            "ACTIVO": "ACTIVE",
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
            "Estado actual": "Current status",
            "Entrega estimada": "Estimated delivery",
            "LÍNEA DE TIEMPO": "TIMELINE",
            "Línea de tiempo": "Timeline",
            "No disponible": "Not available",
            "Volver": "Back",
            "Volver a mis pedidos": "Back to my orders",
            "Volver al catálogo": "Back to catalog",
            "Pendiente de despacho": "Pending dispatch",
            "Pedido en proceso interno.": "Order in internal process.",
            "Order en proceso interno.": "Order in internal process.",
            "Fecha no disponible": "Date not available",

            // TARJETAS
            "Mis tarjetas": "My cards",
            "Tarjetas": "Cards",
            "Tarjetas registradas": "Registered cards",
            "Pagos guardados": "Saved payments",
            "Consulta, registra y administra tus tarjetas para comprar más rápido.": "View, register, and manage your cards to shop faster.",
            "Agregar tarjeta": "Add card",
            "Actualizar": "Refresh",
            "Predeterminada": "Default",
            "TITULAR": "CARDHOLDER",
            "VENCE": "EXPIRES",
            "DESDE BASE DE DATOS": "FROM DATABASE",
            "Desde base de datos": "From database",

            // NOTIFICACIONES
            "Notifications": "Notifications",
            "CENTRO DE ALERTAS": "ALERT CENTER",
            "Consulta avisos generados desde tus pedidos, carrito y seguimiento de compras.": "View alerts generated from your orders, cart, and purchase tracking.",
            "Ver pedidos": "View orders",
            "Marcar todo": "Mark all",
            "Alertas": "Alerts",
            "Activas": "Active",
            "Información consultada desde la base de datos.": "Information loaded from the database.",
            "Ahora": "Now",

            // SOPORTE
            "Chat de ayuda": "Help chat",
            "Asistente rápido del panel cliente.": "Quick assistant for the customer panel.",
            "Accesos de ayuda": "Help shortcuts",
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

            // ADMIN
            "Dashboard": "Dashboard",
            "Comercial": "Commercial",
            "COMERCIAL": "COMMERCIAL",
            "Operativa": "Operations",
            "OPERATIVA": "OPERATIONS",
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
            "Panel administrativo": "Admin panel",
            "Preferencias del sistema y administración general": "System preferences and general administration",
            "Mi perfil": "My profile",
            "Administrador": "Administrator",
            "Alertas de ventas": "Sales alerts",
            "Notificar al registrar una venta": "Notify when a sale is registered",
            "Stock bajo": "Low stock",
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
            "Guardar cambios": "Save changes",
            "Crear producto": "Create product",
            "Nuevo producto": "New product",
            "Editar producto": "Edit product",
            "Eliminar producto": "Delete product",
            "Crear cliente": "Create customer",
            "Nuevo cliente": "New customer",
            "Editar cliente": "Edit customer",
            "Eliminar cliente": "Delete customer",
            "Crear proveedor": "Create supplier",
            "Nuevo proveedor": "New supplier",
            "Crear empleado": "Create employee",
            "Nuevo empleado": "New employee",
            "Generar reporte": "Generate report",
            "Exportar": "Export",
            "Filtrar": "Filter",
            "Limpiar": "Clear",
            "Buscar por nombre": "Search by name",
            "Código": "Code",
            "Codigo": "Code",
            "Acciones": "Actions",
            "Activo": "Active",
            "Inactivo": "Inactive",
            "Módulos del sistema": "System modules",
            "Gestión de roles": "Role management",
            "Zonas de envío": "Shipping zones",
            "Ver reportes": "View reports",
            "Campañas de marketing": "Marketing campaigns",
            "Cambiar el sistema entre español e inglés": "Switch the system between Spanish and English",

            // GENÉRICOS
            "Nombre": "Name",
            "Correo": "Email",
            "Teléfono": "Phone",
            "Direccion": "Address",
            "Dirección": "Address",
            "Guardar": "Save",
            "Cancelar": "Cancel",
            "Cerrar": "Close",
            "Continuar": "Continue",
            "Siguiente": "Next",
            "Anterior": "Previous",
            "Enviar": "Send",
            "Mensaje": "Message",
            "Descripción": "Description",
            "Cantidad": "Quantity",
            "Referencia": "Reference",
            "N/A": "N/A"
        }
    };

    var replacements = [
        ["Madera de pino", "Pine wood"],
        ["MDF lacado", "Lacquered MDF"],
        ["Tela", "Fabric"],
        ["Interior", "Indoor"],
        ["Exterior", "Outdoor"],
        ["INTERIOR", "INDOOR"],
        ["EXTERIOR", "OUTDOOR"],
        ["12 cuotas de", "12 payments of"],
        ["Mis Órdenes", "My Orders"],
        ["Mis Ordenes", "My Orders"],
        ["Mis órdenes", "My orders"],
        ["Mis ordenes", "My orders"],
        ["Ver detalles", "View details"],
        ["Ver detalle", "View detail"],
        ["Ver todos", "View all"],
        ["Ver todo", "View all"],
        ["Ver pedidos", "View orders"],
        ["Volver a mis pedidos", "Back to my orders"],
        ["Volver al carrito", "Back to cart"],
        ["Volver al catálogo", "Back to catalog"],
        ["Volver al catalogo", "Back to catalog"],
        ["Volver", "Back"],
        ["Pedido activo", "Active order"],
        ["Pedido confirmado", "Order confirmed"],
        ["Orden confirmado", "Order confirmed"],
        ["Orden confirmada", "Order confirmed"],
        ["Preparación del pedido", "Order preparation"],
        ["Preparacion del pedido", "Order preparation"],
        ["Salida a entrega", "Out for delivery"],
        ["Entrega final", "Final delivery"],
        ["Estado actual", "Current status"],
        ["Entrega estimada", "Estimated delivery"],
        ["Fecha no disponible", "Date not available"],
        ["No disponible", "Not available"],
        ["Pendiente de despacho", "Pending dispatch"],
        ["Pendiente", "Pending"],
        ["En proceso", "In progress"],
        ["En producción", "In production"],
        ["En camino", "On the way"],
        ["Entregado", "Delivered"],
        ["Cancelado", "Canceled"],
        ["Activo", "Active"],
        ["Activas", "Active"],
        ["Alertas", "Alerts"],
        ["Pedidos", "Orders"],
        ["Pedido", "Order"],
        ["Orden", "Order"],
        ["ordenes", "orders"],
        ["órdenes", "orders"],
        ["productos", "products"],
        ["producto", "product"],
        ["Carrito", "Cart"],
        ["carrito", "cart"],
        ["Dirección", "Address"],
        ["Direccion", "Address"],
        ["Método de pago", "Payment method"],
        ["Metodo de pago", "Payment method"],
        ["Tarjeta guardada", "Saved card"],
        ["Tarjetas guardadas", "Saved cards"],
        ["Tarjetas registradas", "Registered cards"],
        ["Agregar tarjeta", "Add card"],
        ["Pagos guardados", "Saved payments"],
        ["Desde base de datos", "From database"],
        ["DESDE BASE DE DATOS", "FROM DATABASE"],
        ["Agregar", "Add"],
        ["Eliminar", "Delete"],
        ["Actualizar", "Refresh"],
        ["Vaciar carrito", "Empty cart"],
        ["Seguir comprando", "Keep shopping"],
        ["Continuar comprando", "Continue shopping"],
        ["Proceder al pago", "Proceed to payment"],
        ["Finalizar compra", "Checkout"],
        ["Confirmar pedido", "Confirm order"],
        ["Resumen del pedido", "Order summary"],
        ["Resumen de compra", "Purchase summary"],
        ["Descuento", "Discount"],
        ["Cupón", "Coupon"],
        ["Cupon", "Coupon"],
        ["IVA", "VAT"],
        ["Soporte", "Support"],
        ["Notificaciones", "Notifications"],
        ["Configuración", "Settings"],
        ["Perfil", "Profile"],
        ["Catálogo", "Catalog"],
        ["Catalogo", "Catalog"],
        ["Favoritos", "Favorites"],
        ["Reseñas", "Reviews"],
        ["Privacidad", "Privacy"],
        ["Idioma", "Language"],
        ["Español", "Spanish"],
        ["Inglés", "English"],
        ["Buscar", "Search"],
        ["Búsqueda", "Search"],
        ["Categoría", "Category"],
        ["Categoria", "Category"],
        ["Todas", "All"],
        ["Todos", "All"],
        ["Tipo", "Type"],
        ["Material", "Material"],
        ["Color", "Color"],
        ["Relevancia", "Relevance"],
        ["Nombre", "Name"],
        ["Precio", "Price"],
        ["Disponibilidad", "Availability"],
        ["Cantidad", "Quantity"],
        ["Referencia", "Reference"],
        ["Disponible", "Available"],
        ["Mensaje", "Message"],
        ["Enviar", "Send"],
        ["Ahora", "Now"],
        ["Clientes", "Customers"],
        ["Reportes", "Reports"],
        ["Inventario", "Inventory"],
        ["Proveedores", "Suppliers"],
        ["Compras", "Purchases"],
        ["Empleados", "Employees"],
        ["Nómina", "Payroll"],
        ["Nomina", "Payroll"],
        ["Producción", "Production"],
        ["Produccion", "Production"],
        ["Módulos del sistema", "System modules"],
        ["Gestión de roles", "Role management"],
        ["Zonas de envío", "Shipping zones"],
        ["Ver reportes", "View reports"],
        ["Campañas de marketing", "Marketing campaigns"]
    ];

    replacements.sort(function (a, b) {
        return b[0].length - a[0].length;
    });

    function normalizeLanguage(value) {
        return value === "en" || value === "English" || value === "Inglés" ? "Inglés" : "Español";
    }

    function getLangCode() {
        var saved = localStorage.getItem(STORAGE_KEY) || "Español";
        return saved === "Inglés" || saved === "English" || saved === "en" ? "en" : "es";
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

        out = out.replace(/^Orden\s*#?/i, "Order #");
        out = out.replace(/^Pedido\s*#?/i, "Order #");

        out = out.replace(/^Tu pedido\s+(\S+)\s+está en proceso\.\s*Total:/i, "Your order $1 is in progress. Total:");
        out = out.replace(/^Tu pedido\s+(\S+)\s+esta en proceso\.\s*Total:/i, "Your order $1 is in progress. Total:");

        out = out.replace(/está en proceso interno\.?/i, "is in internal process.");
        out = out.replace(/esta en proceso interno\.?/i, "is in internal process.");
        out = out.replace(/está en proceso/i, "is in progress");
        out = out.replace(/esta en proceso/i, "is in progress");
        out = out.replace(/desde tu sesión activa y la base de datos/i, "from your active session and the database");
        out = out.replace(/desde la base de datos/i, "from the database");

        return out;
    }

    function translateText(value) {
        if (getLangCode() === "es") return value;

        var originalValue = String(value == null ? "" : value);
        var leading = originalValue.match(/^\s*/)[0];
        var trailing = originalValue.match(/\s*$/)[0];
        var text = originalValue.trim();
        var out = text;

        if (!text) return value;

        if (dictionary.en[text]) {
            return leading + dictionary.en[text] + trailing;
        }

        out = applyRegexRules(out);

        replacements.forEach(function (pair) {
            var from = pair[0];
            var to = pair[1];

            if (out.indexOf(from) >= 0) {
                out = out.split(from).join(preserveCase(from, to));
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

        if (el.closest && el.closest("[data-no-translate], .notranslate, .pc-language-menu, .pc-global-language")) {
            return true;
        }

        return false;
    }

    function translateNodeText(root) {
        var walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT, {
            acceptNode: function (node) {
                if (!node.nodeValue || !node.nodeValue.trim()) return NodeFilter.FILTER_REJECT;
                if (shouldIgnoreElement(node.parentElement)) return NodeFilter.FILTER_REJECT;
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
        var elements = root.querySelectorAll ? root.querySelectorAll("*") : [];

        Array.prototype.forEach.call(elements, function (el) {
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

        var label = document.getElementById("cfgIdiomaTexto");

        if (label) {
            label.textContent = lang === "en" ? "English" : "Español";
        }

        if (document.title) {
            if (document.__pcOriginalTitle === undefined) {
                document.__pcOriginalTitle = document.title;
            }

            document.title = lang === "es"
                ? document.__pcOriginalTitle
                : translateText(document.__pcOriginalTitle);
        }
    }

    function activarBotonesIdioma() {
        document.querySelectorAll(".lang-switcher").forEach(function (switcher) {
            var btn = switcher.querySelector(".lang-switcher-btn");

            if (!btn || btn.dataset.ready === "1") return;

            btn.dataset.ready = "1";

            btn.addEventListener("click", function (e) {
                e.preventDefault();
                e.stopPropagation();

                document.querySelectorAll(".lang-switcher.open").forEach(function (item) {
                    if (item !== switcher) {
                        item.classList.remove("open");
                    }
                });

                switcher.classList.toggle("open");
            });
        });

        document.querySelectorAll("[data-lang-option]").forEach(function (option) {
            if (option.dataset.ready === "1") return;

            option.dataset.ready = "1";

            option.addEventListener("click", function (e) {
                e.preventDefault();
                e.stopPropagation();

                var idioma = option.getAttribute("data-lang-option");

                localStorage.setItem(STORAGE_KEY, normalizeLanguage(idioma));

                if (window.PortalIdioma && window.PortalIdioma.set) {
                    window.PortalIdioma.set(idioma);
                }

                document.querySelectorAll(".lang-switcher.open").forEach(function (sw) {
                    sw.classList.remove("open");
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
                translateNodeText(root);
                translateAttributes(root);
            }

            updateLanguageLabels();
        } finally {
            isApplying = false;
        }
    }

    function setLanguage(value) {
        localStorage.setItem(STORAGE_KEY, normalizeLanguage(value));

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