using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.Models;
using Projeto_AutoMobile.ViewModels.Veiculos;
using System.Diagnostics;

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
        public async Task<IActionResult> Index()
        {
            // Tenta ir buscar a empresa e a sua frota
            var empresa = await _context.Empresas.Include(e => e.Frota).FirstOrDefaultAsync();

            // GARANTIA DA EMPRESA ÚNICA: Se a BD acabou de ser criada e está vazia, cria a empresa automaticamente!
            if (empresa == null)
            {
                empresa = new Empresa(); // O construtor já mete a data de hoje
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
            }

            List<string> alarmesAtivos = new List<string>();

            foreach (var veiculo in empresa.Frota)
            {
                if ((veiculo.Estado == EstadoVeiculo.Alugado || veiculo.Estado == EstadoVeiculo.EmManutencao || veiculo.Estado == EstadoVeiculo.Reservado)
                    && veiculo.DataDisponibilidade.HasValue)
                {
                    // Se a data do simulador é MAIOR OU IGUAL à data de devolução, o alarme fica ativo!
                    if (empresa.DataAtual.Date >= veiculo.DataDisponibilidade.Value.Date)
                    {
                        alarmesAtivos.Add($"ALARME: O veículo ({veiculo.Marca} {veiculo.Modelo} - {veiculo.Matricula}) terminou o período de '{veiculo.Estado}' a {veiculo.DataDisponibilidade.Value.ToShortDateString()}.");
                    }
                }
            }

            // Envia a lista calculada para a View
            ViewBag.Alarmes = alarmesAtivos;

            // Monta a ViewModel usando os veículos que a empresa tem
            var viewModel = new VeiculosDisponiveisViewModel
            {
                DataAtual = empresa.DataAtual,
                Veiculos = empresa.Frota.ToList()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
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
    }
}
