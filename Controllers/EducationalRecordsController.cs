using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDashboard;
using CoreDashboard.Models;

namespace CoreDashboard.Controllers
{
    public class EducationalRecordsController : Controller
    {
        private readonly ApplicationContext _context;

        public EducationalRecordsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: EducationalRecords
        public async Task<IActionResult> Index()
        {
            return View((await _context.EducationalRecords.ToListAsync()).Take(500));
        }

        // GET: EducationalRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EducationalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordId,Student,DisciplineName,GroupName,TopicName,TopicScore,Presence,ControlPoint,TotalScore,Rating,StudyDirection,Teacher")] EducationalRecord educationalRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(educationalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(educationalRecord);
        }

        [HttpPost]
        public async Task<string> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                return "File not selected or empty.";
            }

            // Получение расширения файла
            var fileExtension = Path.GetExtension(uploadedFile.FileName);

            if (fileExtension is not ".csv")
            {
                return "File extension is not supported";
            }


            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            string fileContent;
            using (var reader = new StreamReader(filePath))
            {
                fileContent = await reader.ReadToEndAsync();
            }

            await _context.AddRangeAsync(ConvertTextToEducationalRecords(fileContent));
            await _context.SaveChangesAsync();
            return fileContent;
        }

        public static IEnumerable<EducationalRecord> ConvertTextToEducationalRecords(string inputText)
        {
            List<EducationalRecord> educationalRecords = [];

            var lines = inputText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(';');

                educationalRecords.Add(new EducationalRecord
                {
                    Student = values[0],
                    DisciplineName = values[1],
                    GroupName = values[2],
                    TopicName = values[3],
                    TopicScore = string.IsNullOrEmpty(values[4]) ? 0 : Convert.ToDecimal(values[4]),
                    Presence = string.IsNullOrEmpty(values[5]) ? '-' : values[5][0],
                    ControlPoint = string.IsNullOrEmpty(values[6]) ? 0 : Convert.ToDecimal(values[6]),
                    TotalScore = Convert.ToDecimal(values[7]),
                    Rating = values[8],
                    StudyDirection = values[9],
                    Teacher = values[10]
                });
            }

            return educationalRecords;
        }

        //public static IEnumerable<EducationalRecord> ConvertTextToEducationalRecords(string inputText)
        //{
        //    var lines = inputText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        //    for (int i = 1; i < lines.Length; i++)
        //    {
        //        var values = lines[i].Split(',');

        //        yield return new EducationalRecord
        //        {
        //            Student = values[0],
        //            DisciplineName = values[1],
        //            GroupName = values[2],
        //            TopicName = values[3],
        //            TopicScore = Convert.ToDecimal(values[4]),
        //            Presence = string.IsNullOrEmpty(values[5]) ? '\0' : values[5][0],
        //            ControlPoint = string.IsNullOrEmpty(values[6]) ? 0 : Convert.ToDecimal(values[6]),
        //            TotalScore = Convert.ToDecimal(values[7]),
        //            Rating = values[8],
        //            StudyDirection = values[9],
        //            Teacher = values[10]
        //        };
        //    }
        //}


        // GET: EducationalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educationalRecord = await _context.EducationalRecords.FindAsync(id);
            if (educationalRecord == null)
            {
                return NotFound();
            }
            return View(educationalRecord);
        }

        // POST: EducationalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,Student,DisciplineName,GroupName,TopicName,TopicScore,Presence,ControlPoint,TotalScore,Rating,StudyDirection,Teacher")] EducationalRecord educationalRecord)
        {
            if (id != educationalRecord.RecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(educationalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationalRecordExists(educationalRecord.RecordId))
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
            return View(educationalRecord);
        }

        // GET: EducationalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educationalRecord = await _context.EducationalRecords
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (educationalRecord == null)
            {
                return NotFound();
            }

            return View(educationalRecord);
        }

        // POST: EducationalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var educationalRecord = await _context.EducationalRecords.FindAsync(id);
            if (educationalRecord != null)
            {
                _context.EducationalRecords.Remove(educationalRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EducationalRecordExists(int id)
        {
            return _context.EducationalRecords.Any(e => e.RecordId == id);
        }
    }
}
