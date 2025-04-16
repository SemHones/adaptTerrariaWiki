using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;

namespace terraria_api.Repository
{
    public class ArmorPiecesController : Controller
    {
        private readonly TerrariaContext _context;

        public ArmorPiecesController(TerrariaContext context)
        {
            _context = context;
        }

        // GET: ArmorPieces
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArmorPieces.ToListAsync());
        }

        // GET: ArmorPieces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armorPiece = await _context.ArmorPieces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (armorPiece == null)
            {
                return NotFound();
            }

            return View(armorPiece);
        }

        // GET: ArmorPieces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArmorPieces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Href,ObtainedFrom,Defense,BodySlot,ToolTip")] ArmorPiece armorPiece)
        {
            if (ModelState.IsValid)
            {
                _context.Add(armorPiece);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(armorPiece);
        }

        // GET: ArmorPieces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armorPiece = await _context.ArmorPieces.FindAsync(id);
            if (armorPiece == null)
            {
                return NotFound();
            }
            return View(armorPiece);
        }

        // POST: ArmorPieces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Href,ObtainedFrom,Defense,BodySlot,ToolTip")] ArmorPiece armorPiece)
        {
            if (id != armorPiece.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(armorPiece);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArmorPieceExists(armorPiece.Id))
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
            return View(armorPiece);
        }

        // GET: ArmorPieces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armorPiece = await _context.ArmorPieces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (armorPiece == null)
            {
                return NotFound();
            }

            return View(armorPiece);
        }

        // POST: ArmorPieces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var armorPiece = await _context.ArmorPieces.FindAsync(id);
            if (armorPiece != null)
            {
                _context.ArmorPieces.Remove(armorPiece);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArmorPieceExists(int id)
        {
            return _context.ArmorPieces.Any(e => e.Id == id);
        }
    }
}
