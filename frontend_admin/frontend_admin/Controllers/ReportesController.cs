using frontend_admin.Data;
using frontend_admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace frontend_admin.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(ApplicationDbContext context, ILogger<ReportesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Cargando la vista de Reportes.");

            var reportes = await _context.Reportes.Include(r => r.Usuario).ToListAsync();
            return View(reportes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Cargando detalles del reporte con ID: {id}");

            var reporte = await _context.Reportes
                                        .Include(r => r.Usuario)
                                        .FirstOrDefaultAsync(r => r.IdReporte == id);

            if (reporte == null)
            {
                _logger.LogWarning($"No se encontró el reporte con ID: {id}");
                return NotFound();
            }

            var impactoIds = ExtractImpactoIds(reporte.Detalles);

            var impactos = await _context.ImpactosBala
                                         .Where(i => impactoIds.Contains(i.IdImpacto))
                                         .ToListAsync();
            ViewBag.ImpactosDetalles = impactos;

            return View(reporte);
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf(int id)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            _logger.LogInformation($"Generando PDF para el reporte con ID: {id}");

            var reporte = await _context.Reportes
                                        .Include(r => r.Usuario)
                                        .FirstOrDefaultAsync(r => r.IdReporte == id);

            if (reporte == null)
            {
                _logger.LogWarning($"No se encontró el reporte con ID: {id}");
                return NotFound();
            }

            var impactoIds = ExtractImpactoIds(reporte.Detalles);
            var impactos = await _context.ImpactosBala
                                         .Where(i => impactoIds.Contains(i.IdImpacto))
                                         .ToListAsync();

            var pdfDocument = new ReportePdfDocument(reporte, impactos);
            var pdfBytes = pdfDocument.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Reporte_{id}.pdf");
        }


        private List<int> ExtractImpactoIds(string detalles)
        {
            var start = detalles.IndexOf('[') + 1;
            var end = detalles.IndexOf(']');
            var idString = detalles.Substring(start, end - start);
            var ids = idString.Split(',')
                              .Select(id => int.Parse(id.Trim()))
                              .ToList();
            return ids;
        }
    }

    public class ReportePdfDocument : IDocument
    {
        private readonly Reportes _reporte;
        private readonly List<ImpactosBala> _impactos;

        public ReportePdfDocument(Reportes reporte, List<ImpactosBala> impactos)
        {
            _reporte = reporte;
            _impactos = impactos;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));

                page.Header().Text($"Reporte ID: {_reporte.IdReporte}").FontSize(18).Bold();

                page.Content().Column(column =>
                {
                    column.Item().Text($"Usuario: {_reporte.Usuario.Nombre}");
                    column.Item().Text($"Fecha del Reporte: {_reporte.FechaReporte}");
                    column.Item().Text($"Total Impactos: {_reporte.TotalImpactos}");
                    column.Item().Text($"Promedio de Precisión: {_reporte.PromedioPrecision}");
                    column.Item().Text($"Detalles: {_reporte.Detalles}");

                    column.Item().Text("Impactos Procesados:").Bold().FontSize(14);

                    if (_impactos.Any())
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID Impacto");
                                header.Cell().Element(CellStyle).Text("Ubicación");
                                header.Cell().Element(CellStyle).Text("Precisión");
                                header.Cell().Element(CellStyle).Text("Fecha");
                            });

                            foreach (var impacto in _impactos)
                            {
                                table.Cell().Element(CellStyle).Text(impacto.IdImpacto.ToString());
                                table.Cell().Element(CellStyle).Text(impacto.Ubicacion);
                                table.Cell().Element(CellStyle).Text(impacto.Precision.ToString());
                                table.Cell().Element(CellStyle).Text(impacto.Fecha.ToShortDateString());
                            }
                        });
                    }
                    else
                    {
                        column.Item().Text("No se encontraron impactos para este reporte.");
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        private IContainer CellStyle(IContainer container)
        {
            return container.Border(1).Padding(5).AlignCenter().AlignMiddle();
        }
    }
}
