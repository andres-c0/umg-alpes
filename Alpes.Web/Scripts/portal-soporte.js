document.addEventListener('DOMContentLoaded', function () {
    var chat = document.getElementById('psChat');
    var input = document.getElementById('psInput');
    var send = document.getElementById('psSend');
    var emojiBtn = document.getElementById('psEmojiBtn');
    var quickButtons = document.querySelectorAll('.ps-quick-btn');

    if (!chat || !input || !send) {
        return;
    }

    function getTimeNow() {
        var now = new Date();
        var hours = now.getHours().toString().padStart(2, '0');
        var minutes = now.getMinutes().toString().padStart(2, '0');
        return hours + ':' + minutes;
    }

    function scrollToBottom() {
        chat.scrollTop = chat.scrollHeight;
    }

    function addMessage(text, type) {
        var wrapper = document.createElement('div');
        wrapper.className = 'ps-message ' + (type === 'user' ? 'ps-message--user' : 'ps-message--bot');

        var bubble = document.createElement('div');
        bubble.className = 'ps-bubble';
        bubble.textContent = text;

        var time = document.createElement('div');
        time.className = 'ps-time';
        time.textContent = getTimeNow();

        wrapper.appendChild(bubble);
        wrapper.appendChild(time);
        chat.appendChild(wrapper);

        scrollToBottom();
    }

    function addTyping() {
        var wrapper = document.createElement('div');
        wrapper.className = 'ps-message ps-message--bot';
        wrapper.id = 'psTypingMessage';

        var bubble = document.createElement('div');
        bubble.className = 'ps-bubble';

        var typing = document.createElement('div');
        typing.className = 'ps-typing';
        typing.innerHTML = '<span></span><span></span><span></span>';

        bubble.appendChild(typing);
        wrapper.appendChild(bubble);
        chat.appendChild(wrapper);

        scrollToBottom();
    }

    function removeTyping() {
        var typing = document.getElementById('psTypingMessage');
        if (typing) {
            typing.remove();
        }
    }

    function normalizeText(value) {
        return String(value || '')
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .trim();
    }

    function getAutoReply(message) {
        var text = normalizeText(message);

        if (
            text.includes('hola') ||
            text.includes('buenas') ||
            text.includes('buenos dias') ||
            text.includes('buenas tardes')
        ) {
            return 'Hola, con gusto te ayudo. Puedes consultarme sobre pedidos, pagos, envios o descuentos.';
        }

        if (
            text.includes('pedido') ||
            text.includes('orden') ||
            text.includes('compra')
        ) {
            return 'Puedes revisar el estado de tus pedidos en la seccion "Mis pedidos". Si quieres, tambien puedo orientarte sobre tiempos de entrega.';
        }

        if (
            text.includes('envio') ||
            text.includes('direccion') ||
            text.includes('entrega')
        ) {
            return 'Los envios se procesan de lunes a viernes. Tambien puedes gestionar tus direcciones desde tu perfil.';
        }

        if (
            text.includes('pago') ||
            text.includes('tarjeta') ||
            text.includes('cobro')
        ) {
            return 'Si tu pago fue aprobado, deberia reflejarse en tu pedido. Tambien puedes revisar tus metodos de pago desde tu perfil.';
        }

        if (
            text.includes('descuento') ||
            text.includes('cupon') ||
            text.includes('promocion')
        ) {
            return 'Cuando haya promociones activas o cupones disponibles, te apareceran en notificaciones o durante la compra.';
        }

        if (text.includes('gracias')) {
            return 'Con gusto. Si necesitas algo mas, aqui estare para ayudarte.';
        }

        if (
            text.includes('agente') ||
            text.includes('asesor') ||
            text.includes('persona')
        ) {
            return 'Un agente puede apoyarte dentro del horario de atencion. Mientras tanto, puedo resolver dudas frecuentes.';
        }

        return 'Gracias por tu mensaje. Un agente te respondera en breve. Tambien puedes preguntarme por pedidos, pagos, envios o descuentos.';
    }

    function handleMessage(message) {
        if (!message || message.trim() === '') {
            return;
        }

        addMessage(message.trim(), 'user');
        input.value = '';

        addTyping();

        setTimeout(function () {
            removeTyping();
            var reply = getAutoReply(message);
            addMessage(reply, 'bot');
        }, 700);
    }

    send.addEventListener('click', function () {
        handleMessage(input.value);
    });

    input.addEventListener('keydown', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            handleMessage(input.value);
        }
    });

    if (emojiBtn) {
        emojiBtn.addEventListener('click', function () {
            input.value += ' ';
            input.focus();
        });
    }

    quickButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            handleMessage(btn.textContent);
        });
    });

    scrollToBottom();
});