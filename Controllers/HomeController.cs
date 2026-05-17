using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.Models;
using Projeto_AutoMobile.ViewModels.Veiculos;

namespace Projeto_AutoMobile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Projeto_AutoMobileContext _context; //adicionar a base de dados

        public HomeController(ILogger<HomeController> logger, Projeto_AutoMobileContext context) //receber a bd
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(string tipoSelecionado, string estadoSelecionado)
        {
            // Tenta ir buscar a empresa e a sua frota
            var empresa = await _context.Empresas.Include(e => e.Frota).FirstOrDefaultAsync();
            var frota = await _context.Veiculos.ToListAsync();
            var resultados = frota.AsEnumerable();
            // GARANTIA DA EMPRESA ÚNICA: Se a BD acabou de ser criada e está vazia, cria a empresa automaticamente!
            if (empresa == null)
            {
                empresa = new Empresa(); // O construtor já mete a data de hoje
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
            }

            var todasReservas = await _context.Reservas.ToListAsync();

            // Passa as reservas para a View (necessário para calcular os estados em tempo real no HTML)
            ViewBag.TodasReservas = todasReservas;

            List<string> alarmesAtivos = new List<string>();

            foreach (var veiculo in frota)
            {
                var reservaNoPrazoLimite = todasReservas.FirstOrDefault(r => r.VeiculoId == veiculo.Id && empresa.DataAtual.Date >= r.DataFim.Date);

                if (reservaNoPrazoLimite != null)
                {
                    alarmesAtivos.Add($"ALARME: O veículo {veiculo.Marca} {veiculo.Modelo} - {veiculo.Matricula} terminou o período de reserva a {reservaNoPrazoLimite.DataFim.ToShortDateString()}.");
                }

                // Verifica, de forma independente, se o veículo está na oficina e tem uma data de saída definida.
                if (veiculo.Estado == EstadoVeiculo.EmManutencao && veiculo.DataDisponibilidade.HasValue)
                {
                    // Se a data do simulador é MAIOR OU IGUAL à data de devolução, o alarme fica ativo!
                    if (empresa.DataAtual.Date >= veiculo.DataDisponibilidade.Value.Date)
                    {
                        alarmesAtivos.Add($"ALARME: O veículo {veiculo.Marca} {veiculo.Modelo} - {veiculo.Matricula} terminou o período de manutenção a {veiculo.DataDisponibilidade.Value.ToShortDateString()}.");
                    }
                }
            }
            // Envia a lista calculada para a View
            ViewBag.Alarmes = alarmesAtivos;

            if (!string.IsNullOrEmpty(tipoSelecionado) && tipoSelecionado != "Todos")
            {
                resultados = resultados.Where(v => v.GetType().Name == tipoSelecionado);
            }

            // FILTRO DE ESTADO DO VEÍCULO:
            if (!string.IsNullOrEmpty(estadoSelecionado) && estadoSelecionado != "Todos")
            {
                // Converte o estado selecionado para o enum correspondente
                if (Enum.TryParse(typeof(EstadoVeiculo), estadoSelecionado, out var estadoConvertido))
                {
                    resultados = resultados.Where(v => v.Estado == (EstadoVeiculo)estadoConvertido);
                }
            }

            // Montar os dados para o ecrã
            var viewModel = new VeiculosDisponiveisViewModel
            {
                DataAtual = empresa.DataAtual,
                TipoSelecionado = tipoSelecionado ?? "Todos",
                EstadoSelecionado = estadoSelecionado ?? "Todos",
                Veiculos = resultados.ToList()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Disponiveis(string tipoSelecionado, string estadoSelecionado)
        {
            // Vai buscar a empresa para sabermos qual é a "Data Atual" do Simulador
            var empresa = await _context.Empresas.FirstOrDefaultAsync();
            if (empresa == null) return NotFound("Empresa não inicializada.");

            // Vai buscar todos os veículos à Base de Dados
            var frota = await _context.Veiculos.ToListAsync();

            var resultados = frota.AsEnumerable();

            // FILTRO DE TIPO DE VEÍCULO:
            if (!string.IsNullOrEmpty(tipoSelecionado) && tipoSelecionado != "Todos")
            {
                resultados = resultados.Where(v => v.GetType().Name == tipoSelecionado);
            }

            // FILTRO DE ESTADO DO VEÍCULO:
            if (!string.IsNullOrEmpty(estadoSelecionado) && estadoSelecionado != "Todos")
            {
                // Converte o estado selecionado para o enum correspondente
                if (Enum.TryParse(typeof(EstadoVeiculo), estadoSelecionado, out var estadoConvertido))
                {
                    resultados = resultados.Where(v => v.Estado == (EstadoVeiculo)estadoConvertido);
                }
            }

            // Montar os dados para o ecrã
            var viewModel = new VeiculosDisponiveisViewModel
            {
                DataAtual = empresa.DataAtual,
                TipoSelecionado = tipoSelecionado ?? "Todos",
                EstadoSelecionado = estadoSelecionado ?? "Todos",
                Veiculos = resultados.ToList()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarDataSimulador(DateTime dataAtual)
        {
            var empresa = await _context.Empresas.Include(e => e.Frota).FirstOrDefaultAsync();
            if (empresa == null) return NotFound();

            var alarmesAntigos = TempData["Alarmes"] as string[] ?? new string[0];
            List<string> todosAlarmes = alarmesAntigos.ToList();

            // Se a data vinda do JavaScript for no FUTURO, avançamos dia-a-dia para não perder alarmes!
            while (empresa.DataAtual.Date < dataAtual.Date)
            {
                var alarmesDoDia = empresa.AvancarDia();
                todosAlarmes.AddRange(alarmesDoDia); // Junta os alarmes deste dia à lista total
            }

            // Se a data vinda do JavaScript for no PASSADO, recuamos os dias
            while (empresa.DataAtual.Date > dataAtual.Date)
            {
                var alarmesDoDia = empresa.RecuarDia();
                todosAlarmes.AddRange(alarmesDoDia);
            }

            _context.Update(empresa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportarCSV()
        {
            // StringBuilder permite construir texto de forma eficiente sem criar novas strings a cada concatenação.
            var veiculos = await _context.Veiculos.ToListAsync();
            var builder = new StringBuilder();

            // Cabeçalho do CSV, cada coluna separada por ponto e vírgula.
            builder.AppendLine("Id;Tipo;Matricula;Marca;Modelo;Preco/Dia;Estado;Disponibilidade");

            foreach (var v in veiculos)
            {
                string tipoObjeto = v.GetType().Name;

                // Converte o enum EstadoVeiculo para texto legível pelo utilizador.
                string estadoFormatado = v.Estado switch
                {
                    EstadoVeiculo.Disponivel => "Disponível",
                    EstadoVeiculo.Alugado => "Alugado",
                    EstadoVeiculo.Reservado => "Reservado",
                    EstadoVeiculo.EmManutencao => "Em Manutenção",
                    _ => v.Estado.ToString()
                };

                string dataTexto = v.DataDisponibilidade.HasValue
                    ? v.DataDisponibilidade.Value.ToString("dd/MM/yyyy")
                    : "N/A";

                builder.AppendLine($"{v.Id};{tipoObjeto};{v.Matricula};{v.Marca};{v.Modelo};{v.PrecoDia.ToString("F2")};{estadoFormatado};{dataTexto}");
            }

            var csvContent = builder.ToString();

            // Converte o texto do CSV para bytes em UTF-8 para poder ser enviado como ficheiro.
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            return File(bytes, "text/csv", "Frota_Veiculos.csv");
        }
    }
}
