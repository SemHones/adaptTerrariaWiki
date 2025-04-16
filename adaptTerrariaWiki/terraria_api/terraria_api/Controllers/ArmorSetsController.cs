using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;

namespace terraria_api.Controllers
{
    public class ArmorSetsController : Controller
    {
        private readonly TerrariaContext _context;

        public ArmorSetsController(TerrariaContext context)
        {
            _context = context;
        }

        // GET: ArmorSets
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArmorSets.ToListAsync());
        }

        // GET: ArmorSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armorSet = await _context.ArmorSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (armorSet == null)
            {
                return NotFound();
            }

            return View(armorSet);
        }

        // GET: ArmorSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArmorSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Href,SetBonus")] ArmorSet armorSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(armorSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(armorSet);
        }

        // GET: ArmorSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armorSet = await _context.ArmorSets.FindAsync(id);
            if (armorSet == null)
            {
                return NotFound();
            }
            return View(armorSet);
        }

        // POST: ArmorSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Href,SetBonus")] ArmorSet armorSet)
        {
            if (id != armorSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(armorSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArmorSetExists(armorSet.Id))
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
            return View(armorSet);
        }

        // GET: ArmorSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armorSet = await _context.ArmorSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (armorSet == null)
            {
                return NotFound();
            }

            return View(armorSet);
        }

        // POST: ArmorSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var armorSet = await _context.ArmorSets.FindAsync(id);
            if (armorSet != null)
            {
                _context.ArmorSets.Remove(armorSet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArmorSetExists(int id)
        {
            return _context.ArmorSets.Any(e => e.Id == id);
        }
    }
}
