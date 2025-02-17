document.addEventListener("DOMContentLoaded", function () {

    // Verifica se a tela de login está presente antes de executar o código relacionado
    var telaLogin = document.getElementById("telaLogin");
    if (telaLogin) {

        let btnLogin = document.getElementById("btnLogin");
        if (btnLogin) {
            btnLogin.addEventListener("click", function (event) {
                event.preventDefault(); // Evita o envio automático do formulário
                let text = document.getElementById("btnText");
                let loading = document.getElementById("btnLoading");

                text.classList.add("d-none");
                loading.classList.remove("d-none");
                btnLogin.disabled = true;

                // Requisição AJAX para login
                let formData = new FormData(document.getElementById("loginForm"));

                fetch("/Login/Autenticar", {
                    method: "POST",
                    body: formData
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.sucesso) {
                            showToast(data.mensagem, "success");
                            setTimeout(() => {
                                window.location.href = "/Home/Index";
                            }, 2000);
                        } else {
                            showToast(data.mensagem, "danger");
                            text.classList.remove("d-none");
                            loading.classList.add("d-none");
                            btnLogin.disabled = false;
                        }
                    })
                    .catch(error => {
                        showToast("Erro ao tentar autenticar. Tente novamente.", "danger");
                        text.classList.remove("d-none");
                        loading.classList.add("d-none");
                        btnLogin.disabled = false;
                    });
            });
        }

        // Alternar a visibilidade da senha
        let toggleSenha = document.getElementById("toggleSenha");
        if (toggleSenha) {
            toggleSenha.addEventListener("click", function () {
                let senhaInput = document.getElementById("senhaInput");
                let icon = this.querySelector("i");

                if (senhaInput.type === "password") {
                    senhaInput.type = "text";
                    icon.classList.remove("fa-eye");
                    icon.classList.add("fa-eye-slash");
                } else {
                    senhaInput.type = "password";
                    icon.classList.remove("fa-eye-slash");
                    icon.classList.add("fa-eye");
                }
            });
        }
    }

    // Função para exibir Toasts no topo da tela
    function showToast(message, type = "danger") {
        const toastContainer = document.getElementById("toastContainer");

        if (!toastContainer) return;

        const toastHtml = document.createElement("div");
        toastHtml.className = `toast align-items-center text-white bg-${type} border-0 show`;
        toastHtml.setAttribute("role", "alert");
        toastHtml.setAttribute("aria-live", "assertive");
        toastHtml.setAttribute("aria-atomic", "true");

        toastHtml.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">${message}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;

        toastContainer.appendChild(toastHtml);
        let toast = new bootstrap.Toast(toastHtml);
        toast.show();

        // Remove o toast automaticamente após 5 segundos
        setTimeout(() => {
            toastHtml.classList.remove("show");
            setTimeout(() => toastHtml.remove(), 500);
        }, 5000);
    }

    let toggleSidebarButton = document.getElementById("toggleSidebar");
    let openSidebarButton = document.getElementById("openSidebar");
    let sidebar = document.getElementById("sidebar");

    function fecharSidebar() {
        sidebar.classList.add("hidden");
        openSidebarButton.classList.remove("d-none"); // Exibe o botão para abrir o menu
    }

    function abrirSidebar() {
        sidebar.classList.remove("hidden");
        openSidebarButton.classList.add("d-none"); // Esconde o botão ao abrir o menu
    }

    if (toggleSidebarButton) {
        toggleSidebarButton.addEventListener("click", fecharSidebar);
    }

    if (openSidebarButton) {
        openSidebarButton.addEventListener("click", abrirSidebar);
    }

    // Se o usuário redimensionar a tela, ajustar a visibilidade do botão corretamente
    window.addEventListener("resize", function () {
        if (window.innerWidth > 768) {
            openSidebarButton.classList.add("d-none");
            sidebar.classList.remove("hidden");
        }
    });
});
