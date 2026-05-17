using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Reserva;

namespace Projeto_AutoMobile.Controllers
{
    public class ReservasController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public ReservasController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index(int? id)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .ToListAsync();

            ViewBag.SelectedId = id;
            return View(reservas);
        }



        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            CarregarDados();
            return View(new ReservaViewModel());
        }

        // POST: CALCULAR PREÇO (botão "Atualizar preço")
        [HttpPost]
        public async Task<IActionResult> CalcularPreco(ReservaViewModel model, int? id)
        {
            CarregarDados();
            ViewBag.SelectedId = id;

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);
            string nomeDaVista = id == null ? "Create" : "Edit";

            if (veiculo == null)
            {
                ModelState.AddModelError("", "Veículo inválido.");
                return View(nomeDaVista, model);
            }

            if (model.DataInicio == default || model.DataFim == default)
            {
                ModelState.AddModelError("", "Preenche as datas.");
                return View(nomeDaVista, model);
            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "A data final não pode ser anterior à inicial.");
                return View(nomeDaVista, model);
            }

            int dias = (model.DataFim - model.DataInicio).Days+1;

            if (dias <= 0)
            {
                ModelState.AddModelError("", "A reserva tem de ter pelo menos 1 dia.");
                return View(nomeDaVista, model);
            }

            model.PrecoEstimado = dias * veiculo.PrecoDia;

            return View(nomeDaVista, model);
        }

        // POST: Create (criar reserva final)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservaViewModel model)
        {
            if (model.DataInicio < DateTime.Today)
            {
                ModelState.AddModelError("", "A data inicial não pode ser anterior a hoje.");

            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "Datas inválidas.");

            }

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);

            if (veiculo == null)
            {
                ModelState.AddModelError("", "Veículo inválido.");

            }

            //Impedir reservas sobrepostas
            var existeReserva = await _context.Reservas
               .Where(r => r.VeiculoId == model.VeiculoId &&
                               r.DataInicio < model.DataFim &&
                               r.DataFim > model.DataInicio)
               .OrderBy(r => r.DataInicio)
               .FirstOrDefaultAsync();

            if (existeReserva != null)
            {
                ModelState.AddModelError("", $"Este veículo já está reservado no período de {existeReserva.DataInicio:dd-MM-yyyy} a {existeReserva.DataFim:dd-MM-yyyy}.");
            }

            if (!ModelState.IsValid)
            {
                CarregarDados();
                return View(model);
            }

            int dias = (model.DataFim - model.DataInicio).Days+1;

            var reserva = new Reserva
            {
                DataInicio = model.DataInicio,
                DataFim = model.DataFim,
                ClienteId = model.ClienteId,
                VeiculoId = model.VeiculoId,
                PrecoEstimado = dias * veiculo.PrecoDia
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            var model = new ReservaViewModel
            {
                DataInicio = reserva.DataInicio,
                DataFim = reserva.DataFim,
                ClienteId = reserva.ClienteId,
                VeiculoId = reserva.VeiculoId
            };

            ViewBag.SelectedId = id;
            CarregarDados();

            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservaViewModel model)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);
            int dias = (model.DataFim - model.DataInicio).Days+1;

            if (reserva == null)
                return NotFound();

            if (model.DataInicio < DateTime.Today)
            {
                ModelState.AddModelError("", "A data inicial não pode ser anterior a hoje.");

            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "Datas inválidas.");

            }

            var existeReserva = await _context.Reservas
               .Where(r => r.VeiculoId == model.VeiculoId &&
                               r.DataInicio < model.DataFim &&
                               r.DataFim > model.DataInicio)
               .OrderBy(r => r.DataInicio)
               .FirstOrDefaultAsync();

            if (existeReserva != null)
            {
                ModelState.AddModelError("", $"Este veículo já está reservado no período de {existeReserva.DataInicio:dd-MM-yyyy} a {existeReserva.DataFim:dd-MM-yyyy}.");
            }


            if (ModelState.IsValid)
            {
                reserva.DataInicio = model.DataInicio;
                reserva.DataFim = model.DataFim;
                reserva.ClienteId = model.ClienteId;
                reserva.VeiculoId = model.VeiculoId;
                reserva.PrecoEstimado = dias * veiculo.PrecoDia;

                _context.Update(reserva);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.SelectedId = id;
            CarregarDados();

            return View(model);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faturacao
        public IActionResult Faturacao()
        {
            return View();
        }

        private void CarregarDados() // Método para carregar os dados dos dropdowns
        {
            var clientesDropdown = _context.Clientes.Select(c => new
            {
                Id = c.Id,
                Descricao = c.Nome + " | " + c.CartaConducao
            });
            ViewBag.Clientes = new SelectList(clientesDropdown, "Id", "Descricao");

            var veiculosFiltrados = _context.Veiculos;
            var veiculosDropdown = veiculosFiltrados.ToList().Select(v => new
            {
                Id = v.Id,
                Descricao = v.Marca + " | " + v.Matricula,
            });
            ViewBag.Veiculos = new SelectList(veiculosDropdown, "Id", "Descricao");
        }

        // POST: Faturacao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Faturacao(DateTime dataInicio, DateTime dataFim)
        {
            if (dataFim < dataInicio)
            {
                ModelState.AddModelError("", "A data final não pode ser anterior à inicial.");
                return View();
            }

            var total = await _context.Reservas
                .Where(r => r.DataInicio >= dataInicio && r.DataFim <= dataFim)
                .SumAsync(r => r.PrecoEstimado);

            ViewBag.TotalFaturacao = total;
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;

            return View();
        }
    }
}