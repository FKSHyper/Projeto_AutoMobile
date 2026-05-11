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
            // Vai buscar a empresa para sabermos qual é a "Data Atual" do Simulador
            var empresa = await _context.Empresas.FirstOrDefaultAsync();

            // Se ainda não houver empresa registada, não rebenta (usa o dia de hoje)
            DateTime dataSimulador = empresa != null ? empresa.DataAtual : DateTime.Now;

            // Vai buscar todos os veículos à Base de Dados
            var frota = await _context.Veiculos.ToListAsync();

            var viewModel = new VeiculosDisponiveisViewModel
            {
                DataAtual = dataSimulador,
                Veiculos = frota
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
