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

    var dashboardInicialPage = document.getElementById("dashboardInicialPage");
    if (dashboardInicialPage) {

        let graficoFinanceiro = null;

        function atualizarDados() {
            document.getElementById("loadingGrafico").style.display = "block";
            document.getElementById("loadingTabela").style.display = "block";

            let mesSelecionado = document.getElementById("selectMes").value;
            let anoAtual = new Date().getFullYear();

            fetch(`/Home/ObterTransacoes?mes=${mesSelecionado}&ano=${anoAtual}`)
                .then(response => response.json())
                .then(data => {
                    document.getElementById("loadingGrafico").style.display = "none";
                    document.getElementById("loadingTabela").style.display = "none";

                    let totalReceitas = data.totalReceitas || 0;
                    let totalDespesas = data.totalDespesas || 0;
                    let saldoFinal = totalReceitas - totalDespesas;
                    let corSaldo = saldoFinal >= 0 ? "rgba(59, 130, 246, 0.8)" : "rgba(239, 68, 68, 0.8)";

                    document.getElementById("valorReceita").innerText = `R$ ${totalReceitas.toFixed(2)}`;
                    document.getElementById("valorDespesa").innerText = `R$ ${totalDespesas.toFixed(2)}`;

                    let tabela = document.getElementById("tabelaLancamentos");
                    tabela.innerHTML = "";
                    if (!data.transacoes || data.transacoes.length === 0) {
                        tabela.innerHTML = `<tr><td colspan="3" class="text-center">Nenhum lançamento adicionado</td></tr>`;
                    } else {
                        data.transacoes.forEach(transacao => {
                            let valorFormatado = `R$ ${parseFloat(transacao.valor).toFixed(2)}`;
                            tabela.innerHTML += `<tr>
                            <td>${transacao.tipo}</td>
                            <td>${transacao.formaPagamento}</td>
                            <td>${valorFormatado}</td>
                        </tr>`;
                        });
                    }

                    if (graficoFinanceiro) {
                        graficoFinanceiro.destroy();
                    }

                    let ctx = document.getElementById("graficoFinanceiro").getContext("2d");
                    graficoFinanceiro = new Chart(ctx, {
                        type: "bar",
                        data: {
                            labels: ["Receitas", "Despesas", "Saldo Final"],
                            datasets: [
                                {
                                    label: "Receitas",
                                    data: [totalReceitas, 0, 0],
                                    backgroundColor: "rgba(34, 197, 94, 0.8)",
                                    borderRadius: 10
                                },
                                {
                                    label: "Despesas",
                                    data: [0, totalDespesas, 0],
                                    backgroundColor: "rgba(239, 68, 68, 0.8)",
                                    borderRadius: 10
                                },
                                {
                                    label: "Saldo Final",
                                    data: [0, 0, saldoFinal],
                                    backgroundColor: corSaldo,
                                    borderRadius: 10
                                }
                            ]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            scales: {
                                y: {
                                    beginAtZero: false,
                                    suggestedMin: Math.min(saldoFinal, 0) - 500,
                                    suggestedMax: Math.max(totalReceitas, totalDespesas) + 500,
                                    ticks: {
                                        callback: function (value) {
                                            return `R$ ${value.toLocaleString("pt-BR")}`;
                                        }
                                    },
                                    grid: {
                                        color: "rgba(0, 0, 0, 0.1)",
                                        lineWidth: 0.5
                                    }
                                },
                                x: {
                                    grid: {
                                        display: false
                                    }
                                }
                            },
                            plugins: {
                                legend: {
                                    position: "top",
                                    labels: {
                                        font: {
                                            size: 14
                                        }
                                    }
                                },
                                tooltip: {
                                    callbacks: {
                                        label: function (tooltipItem) {
                                            return `R$ ${tooltipItem.raw.toLocaleString("pt-BR")}`;
                                        }
                                    }
                                }
                            }
                        }
                    });
                })
                .catch(error => {
                    console.error("Erro ao buscar os dados:", error);
                    showToast("Erro ao carregar dados. Tente novamente!", "danger");
                    document.getElementById("loadingGrafico").style.display = "none";
                    document.getElementById("loadingTabela").style.display = "none";
                });
        }

        function carregarCategorias() {
            let selectCategoria = document.getElementById("categoriaLancamento");
            selectCategoria.innerHTML = `<option value="">Carregando...</option>`;

            fetch("/Home/ObterCategorias")
                .then(response => response.json())
                .then(categorias => {
                    selectCategoria.innerHTML = "";
                    if (categorias.length === 0) {
                        selectCategoria.innerHTML = `<option value="">Nenhuma categoria disponível</option>`;
                    } else {
                        categorias.forEach(categoria => {
                            selectCategoria.innerHTML += `<option value="${categoria.id}">${categoria.nome}</option>`;
                        });
                    }
                })
                .catch(error => {
                    console.error("Erro ao carregar categorias:", error);
                    selectCategoria.innerHTML = `<option value="">Erro ao carregar</option>`;
                });
        }

        function aplicarMascaraDinheiro() {
            let input = document.getElementById("valorLancamento");

            input.addEventListener("input", function (e) {
                let valor = e.target.value.replace(/\D/g, "");
                let numero = parseFloat(valor) / 100;
                e.target.value = numero.toLocaleString("pt-BR", { minimumFractionDigits: 2 });
            });

            input.addEventListener("blur", function (e) {
                let numero = parseFloat(e.target.value.replace(/\./g, "").replace(",", ".")) || 0;
                e.target.value = numero.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
            });

            input.addEventListener("focus", function (e) {
                let valor = e.target.value.replace(/[^\d,]/g, "");
                e.target.value = valor.replace(",", ".");
            });
        }

        document.getElementById("adicionarLancamento").addEventListener("click", function () {
            carregarCategorias();
            let modal = new bootstrap.Modal(document.getElementById("modalLancamento"));
            modal.show();
        });

        document.getElementById("formLancamento").addEventListener("submit", function (event) {
            event.preventDefault();

            let botaoSubmit = document.querySelector("#formLancamento button[type='submit']");
            botaoSubmit.innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Adicionando...`;
            botaoSubmit.disabled = true;

            let transacaoDto = {
                Valor: parseFloat(document.getElementById("valorLancamento").value.replace(/[^0-9,-]+/g, "").replace(",", ".")),
                Tipo: parseInt(document.getElementById("tipoLancamento").value),
                CategoriaId: parseInt(document.getElementById("categoriaLancamento").value),
                FormaPagamento: parseInt(document.getElementById("formaPagamento").value),
                Descricao: "Lançamento manual"
            };

            fetch("/Home/AdicionarTransacao", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(transacaoDto)
            })
                .then(response => response.json())
                .then(data => {
                    botaoSubmit.innerHTML = "Adicionar";
                    botaoSubmit.disabled = false;

                    if (data.sucesso) {
                        bootstrap.Modal.getInstance(document.getElementById("modalLancamento")).hide();
                        atualizarDados();
                    } else {
                        showToast(data.mensagem, "danger");
                    }
                })
                .catch(error => {
                    console.error("Erro ao adicionar transação:", error);
                    botaoSubmit.innerHTML = "Adicionar";
                    botaoSubmit.disabled = false;
                    showToast("Erro ao adicionar transação. Tente novamente!", "danger");
                });
        });

        aplicarMascaraDinheiro();
        document.getElementById("selectMes").addEventListener("change", atualizarDados);
        atualizarDados();
    }






});
