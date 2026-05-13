using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Camiao;

namespace Projeto_AutoMobile.Controllers
{
    public class CamioesController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public CamioesController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Camioes
        public async Task<IActionResult> Index(int? id)
        {
            var projeto_AutoMobileContext = _context.Camioes.Include(c => c.Empresa);
            ViewBag.SelectedId = id;
            return View(await projeto_AutoMobileContext.ToListAsync());
        }

        // GET: Camioes/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View();
        }

        // POST: Camioes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CamiaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var camiao = new Camiao
                {
                    Matricula = viewModel.Matricula,
                    Marca = viewModel.Marca,
                    Modelo = viewModel.Modelo,
                    PrecoDia = viewModel.PrecoDia,
                    // Mapeamento direto com o novo nome
                    MaxCarga = viewModel.MaxCarga,
                    Estado = EstadoVeiculo.Disponivel,
                    DataDisponibilidade = DateTime.Now,
                    EmpresaId = 1
                };

                _context.Add(camiao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Camioes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var camiao = await _context.Camioes.FindAsync(id);
            if (camiao == null) return NotFound();

            var viewModel = new CamiaoViewModel
            {
                Matricula = camiao.Matricula,
                Marca = camiao.Marca,
                Modelo = camiao.Modelo,
                PrecoDia = camiao.PrecoDia,
                MaxCarga = camiao.MaxCarga, // Mapeamento direto
                Estado = camiao.Estado,
                DataDisponibilidade = camiao.DataDisponibilidade
            };

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camiao.EmpresaId);
            return View(viewModel);
        }

        // POST: Camioes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CamiaoViewModel viewModel)
        {
            if ((viewModel.Estado == EstadoVeiculo.Alugado || viewModel.Estado == EstadoVeiculo.EmManutencao)
                && (viewModel.DataDisponibilidade == null || viewModel.DataDisponibilidade <= DateTime.Now))
            {
                ModelState.AddModelError("DataDisponibilidade", "A data é obrigatória se o estado for Alugado ou Em Manutenção.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var camiaoOriginal = await _context.Camioes.FindAsync(id);
                    if (camiaoOriginal == null) return NotFound();

                    camiaoOriginal.Matricula = viewModel.Matricula;
                    camiaoOriginal.Marca = viewModel.Marca;
                    camiaoOriginal.Modelo = viewModel.Modelo;
                    camiaoOriginal.PrecoDia = viewModel.PrecoDia;
                    camiaoOriginal.MaxCarga = viewModel.MaxCarga; // Mapeamento direto
                    camiaoOriginal.Estado = viewModel.Estado;
                    camiaoOriginal.DataDisponibilidade = viewModel.DataDisponibilidade;

                    _context.Update(camiaoOriginal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamiaoExists(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View(viewModel);
        }

        // POST: Camioes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camiao = await _context.Camioes.FindAsync(id);
            if (camiao != null)
            {
                _context.Camioes.Remove(camiao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CamiaoExists(int id)
        {
            return _context.Camioes.Any(e => e.Id == id);
        }
    }
}
