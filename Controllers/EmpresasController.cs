using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Empresa;

namespace Projeto_AutoMobile.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public EmpresasController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index(int? id)
        {
            Empresa empresa;

            // Se não passarem ID na barra de endereço, vai buscar a primeira empresa da BD
            if (id == null)
            {
                empresa = await _context.Empresas.FirstOrDefaultAsync();
            }
            else
            {
                empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.Id == id);
            }

            if (empresa == null)
            {
                empresa = new Empresa();
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
            }

            // O ASP.NET Core lida melhor com Arrays no TempData, por isso convertemos
            var alarmesGuardados = TempData["Alarmes"] as string[];

            var ViewModel = new EmpresaViewModel
            {
                EmpresaId = empresa.Id,
                DataAtual = empresa.DataAtual,
                Frota = empresa.ObterVeiculos(),
                Alarmes = alarmesGuardados != null ? alarmesGuardados.ToList() : new List<string>()
            };

            return View(ViewModel);
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataAtual")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataAtual")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Empresas/AvancarDia/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AvancarDia(int id)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(m =>m.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }

            List<string> alarmes = empresa.AvancarDia();

            _context.Update(empresa);
            await _context.SaveChangesAsync();

            if (alarmes != null && alarmes.Count > 0)
            {
                TempData["Alarmes"] = alarmes.ToArray();
            }
            else
            {
                TempData["Alarmes"] = "Avançou um dia no sistema. Sem alarmes a registar.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    }
}
