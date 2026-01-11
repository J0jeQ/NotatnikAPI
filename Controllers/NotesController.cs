using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotatnikAPI.Data;
using NotatnikAPI.DTOs;
using NotatnikAPI.Models;

namespace NotatnikAPI.Controllers;

[Authorize]
[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NotesController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? int.Parse(claim.Value) : 0;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var userId = GetCurrentUserId();
        return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        var userId = GetCurrentUserId();
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (note == null) return NotFound();

        return note;
    }

    [HttpPost]
    public async Task<ActionResult<Note>> CreateNote(Note note)
    {
        var userId = GetCurrentUserId();
        note.UserId = userId;
        note.Id = 0;

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, Note note)
    {
        var userId = GetCurrentUserId();
        var existingNote = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (existingNote == null) return NotFound();
        existingNote.Content = note.Content;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var userId = GetCurrentUserId();
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (note == null) return NotFound();

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
