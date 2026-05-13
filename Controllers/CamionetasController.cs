using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Camioneta;


namespace Projeto_AutoMobile.Controllers
{
    public class CamionetasController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public CamionetasController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Camionetas
        public async Task<IActionResult> Index(int? id)
        {
            var projeto_AutoMobileContext = _context.Camionetas.Include(c => c.Empresa);
            ViewBag.SelectedId = id;
            return View(await projeto_AutoMobileContext.ToListAsync());
        }

        // GET: Camionetas/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View();
        }

        // POST: Camionetas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CamionetaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var camioneta = new Camioneta
                {
                    Matricula = viewModel.Matricula,
                    Marca = viewModel.Marca,
                    Modelo = viewModel.Modelo,
                    PrecoDia = viewModel.PrecoDia,
                    MaxPassageiros = viewModel.MaxPassageiros,
                    Eixos = viewModel.Eixos,
                    Estado = EstadoVeiculo.Disponivel,
                    DataDisponibilidade = DateTime.Now,
                    EmpresaId = 1
                };

                try
                {
                    _context.Add(camioneta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx &&
                        (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                    {
                        // Verifica se foi a matrícula duplicada
                        if (sqlEx.Message.Contains("IX_Veiculos_Matricula"))
                        {
                            ModelState.AddModelError("Matricula", "Já existe um veículo com essa matrícula.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro inesperado ao gravar: " + ex.Message);
                    }
                }
            }

            return View(viewModel);
        }

        // GET: Camionetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta == null) return NotFound();

            var viewModel = new CamionetaViewModel
            {
                Matricula = camioneta.Matricula,
                Marca = camioneta.Marca,
                Modelo = camioneta.Modelo,
                PrecoDia = camioneta.PrecoDia,
                MaxPassageiros = camioneta.MaxPassageiros, // Mapeamento direto
                Eixos = camioneta.Eixos, // Mapeamento direto
                Estado = camioneta.Estado,
                DataDisponibilidade = camioneta.DataDisponibilidade
            };

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camioneta.EmpresaId);
            return View(viewModel);
        }

        // POST: Camionetas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CamionetaViewModel viewModel)
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
                    var camionetaOriginal = await _context.Camionetas.FindAsync(id);
                    if (camionetaOriginal == null) return NotFound();

                    camionetaOriginal.Matricula = viewModel.Matricula;
                    camionetaOriginal.Marca = viewModel.Marca;
                    camionetaOriginal.Modelo = viewModel.Modelo;
                    camionetaOriginal.PrecoDia = viewModel.PrecoDia;
                    camionetaOriginal.MaxPassageiros = viewModel.MaxPassageiros;
                    camionetaOriginal.Eixos = viewModel.Eixos;
                    camionetaOriginal.Estado = viewModel.Estado;
                    camionetaOriginal.DataDisponibilidade = viewModel.DataDisponibilidade;

                    _context.Update(camionetaOriginal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamionetaExists(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View(viewModel);
        }

        // POST: Camionetas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta != null)
            {
                _context.Camionetas.Remove(camioneta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CamionetaExists(int id)
        {
            return _context.Camionetas.Any(e => e.Id == id);
        }
    }
}
