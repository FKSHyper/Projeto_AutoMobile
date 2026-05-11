using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.Controllers
{
    public class CarrosController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public CarrosController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Carros
        public async Task<IActionResult> Index(int? id)
        {
            var projeto_AutoMobileContext = _context.Carros.Include(c => c.Empresa);
            ViewBag.SelectedId = id;
            return View(await projeto_AutoMobileContext.ToListAsync());
        }

        // GET: Carros/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View();
        }

        // POST: Carros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projeto_AutoMobile.ViewModels.Carro.CarroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var carro = new Carro
                {
                    Matricula = viewModel.Matricula,
                    Marca = viewModel.Marca,
                    Modelo = viewModel.Modelo,
                    PrecoDia = viewModel.PrecoDia,
                    NrPortas = viewModel.NrPortas,
                    Caixa = viewModel.Caixa,
                    Estado = EstadoVeiculo.Disponivel, //ao criar o carro quero que o estado seja Disponivel
                    DataDisponibilidade = DateTime.Now, //a DataDisponibilidade vai ser o dia em que for criado
                    EmpresaId = 1 //Colocar um valor senao vai dar erro na bd - EmpresaId é FK
                };

                _context.Add(carro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Carros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros.FindAsync(id);
            if (carro == null)
            {
                return NotFound();
            }

            var viewModel = new Projeto_AutoMobile.ViewModels.Carro.CarroViewModel
            {
                Matricula = carro.Matricula,
                Marca = carro.Marca,
                Modelo = carro.Modelo,
                PrecoDia = carro.PrecoDia,
                NrPortas = carro.NrPortas,
                Caixa = carro.Caixa,
                Estado = carro.Estado,
                DataDisponibilidade = carro.DataDisponibilidade,
            };

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", carro.EmpresaId);
            return View(viewModel);
        }

        // POST: Carros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Projeto_AutoMobile.ViewModels.Carro.CarroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var editarCarro = await _context.Carros.FindAsync(id);
                    if (editarCarro == null) return NotFound();

                    editarCarro.Matricula = viewModel.Matricula;
                    editarCarro.Marca = viewModel.Marca;
                    editarCarro.Modelo = viewModel.Modelo;
                    editarCarro.PrecoDia = viewModel.PrecoDia;
                    editarCarro.NrPortas = viewModel.NrPortas;
                    editarCarro.Caixa = viewModel.Caixa;
                    editarCarro.Estado = viewModel.Estado;
                    editarCarro.DataDisponibilidade = viewModel.DataDisponibilidade;
                    editarCarro.EmpresaId = 1;

                    _context.Update(editarCarro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarroExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Carros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carro == null)
            {
                return NotFound();
            }

            return View(carro);
        }

        // POST: Carros/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carro = await _context.Carros.FindAsync(id);
            if (carro != null)
            {
                _context.Carros.Remove(carro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CarroExists(int id)
        {
            return _context.Carros.Any(e => e.Id == id);
        }
    }
}
