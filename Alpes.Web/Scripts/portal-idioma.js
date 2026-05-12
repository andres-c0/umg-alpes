(function () {
    "use strict";

    var STORAGE_KEY = "pc_cfg_idioma";

    var dictionary = {
        en: {
            "Inicio": "Home",
            "Catálogo": "Catalog",
            "Favoritos": "Favorites",
            "Órdenes": "Orders",
            "Mis órdenes": "My orders",
            "Perfil": "Profile",
            "Configuración": "Settings",
            "Soporte Alpes": "Alpes Support",
            "Cerrar sesión": "Log out",
            "Hola,": "Hello,",
            "Muebles de los Alpes": "Muebles de los Alpes",
            "Cliente activo": "Active customer",
            "Ver catálogo": "View catalog",
            "Buscar muebles, salas, comedores...": "Search furniture, living rooms, dining sets...",
            "Resumen de mi cuenta": "My account summary",
            "Actividad reciente": "Recent activity",
            "Ver perfil →": "View profile →",
            "ORDENES": "ORDERS",
            "ORDENES TOTALES": "TOTAL ORDERS",
            "TOTALES": "TOTAL",
            "EN": "ON",
            "CAMINO": "THE WAY",
            "ENTREGADOS": "DELIVERED",
            "TOTAL": "TOTAL",
            "GASTADO": "SPENT",
            "Compras": "Purchases",
            "Mis órdenes recientes": "My recent orders",
            "Ver todos →": "View all →",
            "Seguimiento": "Tracking",
            "Tracking activo": "Active tracking",
            "Orden confirmado": "Order confirmed",
            "En producción": "In production",
            "En camino": "On the way",
            "Entregado": "Delivered",
            "Accesos rápidos": "Quick access",
            "Mis reseñas": "My reviews",
            "Para ti": "For you",
            "Ver todo": "View all",
            "Productos": "Products",
            "Catálogo completo": "Full catalog",
            "Agregar": "Add",
            "Producto agregado al carrito.": "Product added to cart.",
            "No se pudo agregar al carrito.": "Could not add to cart.",
            "Sin órdenes recientes.": "No recent orders.",
            "Sin pedidos recientes.": "No recent orders.",
            "Cargando órdenes...": "Loading orders...",
            "Idioma": "Language",
            "Español": "Spanish",
            "Inglés": "English",
            "Cuenta": "Account",
            "Administra preferencias visuales, notificaciones y accesos de privacidad del panel cliente.": "Manage visual preferences, notifications, and privacy access for the customer panel.",
            "Mi perfil": "My profile",
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
            "Seleccionar idioma": "Select language",
            "Actualmente el panel está optimizado para español.": "The panel can be displayed in Spanish or English.",
            "El panel cliente consulta únicamente información relacionada con tu sesión: perfil, carrito, pedidos, favoritos y tarjetas registradas.": "The customer panel only checks information related to your session: profile, cart, orders, favorites, and saved cards.",
            "No se muestran datos de otros clientes.": "No data from other customers is shown.",
            "Entendido": "Got it",
            "Idioma cambiado a Español": "Language changed to Spanish",
            "Idioma cambiado a Inglés": "Language changed to English",
            "Notificaciones activadas": "Notifications enabled",
            "Notificaciones desactivadas": "Notifications disabled",
            "Modo oscuro activado": "Dark mode enabled",
            "Modo oscuro desactivado": "Dark mode disabled",
            "Pedido": "Order",
            "Producto": "Product",
            "Fecha": "Date",
            "Subtotal": "Subtotal",
            "Descuento": "Discount",
            "Confirmar pedido": "Confirm order",
            "CONFIRMAR PEDIDO": "CONFIRM ORDER",
            "Volver al carrito": "Back to cart",
            "Finalizar compra": "Checkout",
            "Dirección de entrega": "Delivery address",
            "Método de pago": "Payment method",
            "Tarjeta guardada": "Saved card",
            "Cupón de descuento": "Discount coupon",
            "Resumen del pedido": "Order summary",
            "Mis Ordenes": "My Orders",
            "Mis Órdenes": "My Orders",
            "Mis órdenes": "My Orders",
            "Mis pedidos": "My orders",
            "38 ordenes": "38 orders",
            "ordenes": "orders",
            "órdenes": "orders",

            "Pendiente": "Pending",
            "En proceso": "In progress",
            "Activo": "Active",
            "ACTIVO": "ACTIVE",
            "Cancelado": "Canceled",
            "Cancelada": "Canceled",
            "Preparacion del pedido": "Order preparation",
            "Preparación del pedido": "Order preparation",
            "Salida a entrega": "Out for delivery",
            "Entrega final": "Final delivery",
            "Pedido confirmado": "Order confirmed",
            "Pedido activo": "Active order",
            "Pedido entregado con éxito": "Order delivered successfully",

            "Ver detalles →": "View details →",
            "Ver detalle": "View detail",
            "Ver detalle →": "View detail →",
            "Volver": "Back",
            "Volver a mis pedidos": "Back to my orders",
            "Volver al carrito": "Back to cart",
            "Volver al catálogo": "Back to catalog",

            "Tracking del pedido": "Order tracking",
            "Seguimiento de entrega": "Delivery tracking",
            "Consulta la línea de tiempo de tu envío y el estado actual de la entrega.": "Check the timeline of your shipment and the current delivery status.",
            "Estado actual": "Current status",
            "Entrega estimada": "Estimated delivery",
            "LÍNEA DE TIEMPO": "TIMELINE",
            "Línea de tiempo": "Timeline",
            "No disponible": "Not available",

            "Mis tarjetas": "My cards",
            "Tarjetas": "Cards",
            "Tarjetas guardadas": "Saved cards",
            "Tarjetas registradas": "Registered cards",
            "Pagos guardados": "Saved payments",
            "Consulta, registra y administra tus tarjetas para comprar más rápido.": "View, register, and manage your cards to shop faster.",
            "Agregar tarjeta": "Add card",
            "Actualizar": "Refresh",
            "Predeterminada": "Default",
            "Eliminar": "Delete",
            "TITULAR": "CARDHOLDER",
            "VENCE": "EXPIRES",

            "Soporte": "Support",
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
            "Compra protegida": "Protected purchase",
            "Tus pedidos se consultan desde tu sesión activa y la base de datos.": "Your orders are checked from your active session and the database.",

            "Tu carrito": "Your cart",
            "Continuar comprando": "Continue shopping",
            "Puedes agregar productos sin iniciar sesión. Solo te pediremos ingresar o crear cuenta cuando quieras finalizar la compra.": "You can add products without signing in. We will only ask you to sign in or create an account when you want to complete the purchase.",
            "producto": "product",
            "productos": "products",
            "Guardados localmente en este navegador": "Saved locally in this browser",
            "Vaciar carrito": "Empty cart",
            "RESUMEN": "SUMMARY",
            "estimado": "estimated",
            "Products": "Products",
            "Productos": "Products",
            "Subtotal": "Subtotal",
            "IVA 12%": "VAT 12%",
            "Discount": "Discount",
            "Descuento": "Discount",
            "Total": "Total",
            "Proceder al pago": "Proceed to payment",
            "Seguir comprando": "Keep shopping",

            "Notificaciones": "Notifications",
            "Notifications": "Notifications",
            "CENTRO DE ALERTAS": "ALERT CENTER",
            "Consulta avisos generados desde tus pedidos, carrito y seguimiento de compras.": "View alerts generated from your orders, cart, and purchase tracking.",
            "Ver pedidos": "View orders",
            "Marcar todo": "Mark all",
            "Alertas": "Alerts",
            "Activas": "Active",
            "Recent activity": "Recent activity",
            "Actividad reciente": "Recent activity",
            "Información consultada desde la base de datos.": "Information loaded from the database.",
            "Fecha no disponible": "Date not available",

            "Home": "Home",
            "Catalog": "Catalog",
            "Profile": "Profile",
            "Settings": "Settings",
            "Alpes Support": "Alpes Support",
            "Log out": "Log out",
            "Favorites": "Favorites",
            "Orders": "Orders",

            "Perfil": "Profile",
            "Reseñas": "Reviews",
            "Mis reseñas": "My reviews",
            "Mis Favoritos": "My Favorites",
            "Mis favoritos": "My favorites",
            "Carrito": "Cart",
            "Catálogo avanzado": "Advanced catalog",
            "Búsqueda": "Search",
            "Buscar": "Search",
            "Detalle del producto": "Product detail",
            "Detalle de pedido": "Order detail",
            "Detalle de compra": "Purchase detail"
        },
        es: {}
    };

    function getLangCode() {
        var saved = localStorage.getItem(STORAGE_KEY) || "Español";
        return saved === "Inglés" || saved === "English" ? "en" : "es";
    }

    function translateText(value) {
        var lang = getLangCode();

        if (lang === "es") return value;

        var text = String(value || "").trim();
        if (!text) return value;

        if (dictionary.en[text]) {
            return dictionary.en[text];
        }

        // Órdenes dinámicas
        if (/^Orden\s+/i.test(text)) {
            return text.replace(/^Orden/i, "Order");
        }

        if (/^Order#ORD/i.test(text)) {
            return text.replace("Order#", "Order #");
        }

        if (/^Pedido\s+/i.test(text)) {
            return text.replace(/^Pedido/i, "Order");
        }

        if (/^Tu pedido\s+/i.test(text)) {
            return text.replace(/^Tu pedido/i, "Your order");
        }

        // Cantidades dinámicas
        if (/^\d+\s+ordenes$/i.test(text) || /^\d+\s+órdenes$/i.test(text)) {
            return text.replace(/ordenes|órdenes/i, "orders");
        }

        if (/^\d+\s+producto$/i.test(text)) {
            return text.replace(/producto/i, "product");
        }

        if (/^\d+\s+productos$/i.test(text)) {
            return text.replace(/productos/i, "products");
        }

        // Estados dinámicos
        if (text.toUpperCase() === "PENDIENTE") return "PENDING";
        if (text.toUpperCase() === "EN PROCESO") return "IN PROGRESS";
        if (text.toUpperCase() === "ENTREGADO") return "DELIVERED";
        if (text.toUpperCase() === "CANCELADO") return "CANCELED";
        if (text.toUpperCase() === "ACTIVO") return "ACTIVE";

        // Frases largas que vienen mezcladas desde JS
        if (text.indexOf("está en proceso") >= 0) {
            return text
                .replace("Tu pedido", "Your order")
                .replace("está en proceso", "is in progress")
                .replace("Total:", "Total:");
        }

        if (text.indexOf("Fecha no disponible") >= 0) {
            return text.replace("Fecha no disponible", "Date not available");
        }

        if (text.indexOf("Entrega estimada:") >= 0) {
            return text.replace("Entrega estimada:", "Estimated delivery:");
        }

        return value;
    }

    function translateNodeText(root) {
        var walker = document.createTreeWalker(
            root,
            NodeFilter.SHOW_TEXT,
            {
                acceptNode: function (node) {
                    if (!node.nodeValue || !node.nodeValue.trim()) {
                        return NodeFilter.FILTER_REJECT;
                    }

                    var parent = node.parentElement;
                    if (!parent) {
                        return NodeFilter.FILTER_REJECT;
                    }

                    var tag = parent.tagName;
                    if (tag === "SCRIPT" || tag === "STYLE" || tag === "TEXTAREA") {
                        return NodeFilter.FILTER_REJECT;
                    }

                    return NodeFilter.FILTER_ACCEPT;
                }
            }
        );

        var nodes = [];
        while (walker.nextNode()) {
            nodes.push(walker.currentNode);
        }

        nodes.forEach(function (node) {
            if (!node.__pcOriginalText) {
                node.__pcOriginalText = node.nodeValue;
            }

            var original = node.__pcOriginalText;
            var trimmed = original.trim();
            var translated = translateText(trimmed);

            if (translated !== trimmed) {
                node.nodeValue = original.replace(trimmed, translated);
            } else if (getLangCode() === "es") {
                node.nodeValue = original;
            }
        });
    }

    function translatePlaceholders(root) {
        root.querySelectorAll("input[placeholder], textarea[placeholder]").forEach(function (el) {
            if (!el.__pcOriginalPlaceholder) {
                el.__pcOriginalPlaceholder = el.getAttribute("placeholder");
            }

            var original = el.__pcOriginalPlaceholder;
            el.setAttribute("placeholder", getLangCode() === "es" ? original : translateText(original));
        });
    }

    function updateLanguageLabels() {
        var lang = getLangCode();

        document.documentElement.lang = lang;

        var label = document.getElementById("cfgIdiomaTexto");
        if (label) {
            label.textContent = lang === "en" ? "English" : "Español";
        }

        document.querySelectorAll("[data-language-label]").forEach(function (el) {
            el.textContent = lang === "en" ? "English" : "Español";
        });
    }

    function applyLanguage(root) {
        root = root || document.body;

        if (!root) return;

        translateNodeText(root);
        translatePlaceholders(root);
        updateLanguageLabels();
    }

    function setLanguage(value) {
        var normalized = value === "en" || value === "English" || value === "Inglés" ? "Inglés" : "Español";
        localStorage.setItem(STORAGE_KEY, normalized);
        applyLanguage(document.body);
    }

    window.PortalIdioma = {
        apply: applyLanguage,
        set: setLanguage,
        get: getLangCode,
        text: translateText
    };

    document.addEventListener("DOMContentLoaded", function () {
        applyLanguage(document.body);

        var translateTimer = null;

        var observer = new MutationObserver(function (mutations) {
            if (getLangCode() !== "en") return;

            clearTimeout(translateTimer);

            translateTimer = setTimeout(function () {
                mutations.forEach(function (mutation) {
                    mutation.addedNodes.forEach(function (node) {
                        if (node.nodeType === 1) {
                            applyLanguage(node);
                        }
                    });
                });
            }, 250);
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    });
})();