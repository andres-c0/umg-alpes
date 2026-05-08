(function () {
    const CLAVE_CARRITO_INVITADO = "alpes_carrito_invitado";
    const CLAVE_FUSION_EN_PROCESO = "alpes_fusion_carrito_checkout_en_proceso";

    function obtenerCarritoInvitado() {
        try {
            const carritoRaw = localStorage.getItem(CLAVE_CARRITO_INVITADO);

            if (!carritoRaw) {
                return {
                    items: []
                };
            }

            const carrito = JSON.parse(carritoRaw);

            if (!carrito || !Array.isArray(carrito.items)) {
                return {
                    items: []
                };
            }

            return carrito;
        } catch (error) {
            return {
                items: []
            };
        }
    }

    function limpiarCarritoInvitado() {
        localStorage.setItem(CLAVE_CARRITO_INVITADO, JSON.stringify({
            items: []
        }));
    }

    function construirPayload(carrito) {
        const agrupado = {};

        carrito.items.forEach(function (item) {
            const productoId = Number(item.productoId || item.ProductoId || 0);
            const cantidad = Number(item.cantidad || item.Cantidad || 0);

            if (productoId <= 0 || cantidad <= 0) {
                return;
            }

            if (!agrupado[productoId]) {
                agrupado[productoId] = 0;
            }

            agrupado[productoId] += cantidad;
        });

        return {
            items: Object.keys(agrupado).map(function (productoId) {
                return {
                    productoId: Number(productoId),
                    cantidad: agrupado[productoId]
                };
            })
        };
    }

    function actualizarContadorPublico() {
        const contadorDesktop = document.getElementById("publicCartCount");
        const contadorMobile = document.getElementById("publicMobileCartCount");

        if (contadorDesktop) {
            contadorDesktop.textContent = "0";
        }

        if (contadorMobile) {
            contadorMobile.textContent = "0";
        }
    }

    async function fusionarCarritoInvitado() {
        const carrito = obtenerCarritoInvitado();

        if (!carrito.items || carrito.items.length === 0) {
            sessionStorage.removeItem(CLAVE_FUSION_EN_PROCESO);
            return;
        }

        if (sessionStorage.getItem(CLAVE_FUSION_EN_PROCESO) === "1") {
            return;
        }

        const payload = construirPayload(carrito);

        if (!payload.items || payload.items.length === 0) {
            limpiarCarritoInvitado();
            actualizarContadorPublico();
            sessionStorage.removeItem(CLAVE_FUSION_EN_PROCESO);
            return;
        }

        try {
            sessionStorage.setItem(CLAVE_FUSION_EN_PROCESO, "1");

            const respuesta = await fetch("/PortalCliente/FusionarCarritoInvitadoData", {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "Accept": "application/json",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: JSON.stringify(payload)
            });

            const data = await respuesta.json();

            if (!respuesta.ok || !data || data.ok === false || data.success === false) {
                throw new Error(data && data.message ? data.message : "No se pudo fusionar el carrito invitado.");
            }

            limpiarCarritoInvitado();
            actualizarContadorPublico();

            window.location.reload();

        } catch (error) {
            console.error("Error al fusionar carrito invitado:", error);
            sessionStorage.removeItem(CLAVE_FUSION_EN_PROCESO);
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        fusionarCarritoInvitado();
    });
})();