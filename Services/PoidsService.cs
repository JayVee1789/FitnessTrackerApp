
using Blazored.LocalStorage;

using FitnessTrackerApp.Models;


namespace FitnessTrackerApp.Services
{
    public class PoidsService
    {
        private const string StorageKey = "poids_entries";
        private readonly ILocalStorageService _localStorage;
        private const string EntriesKey = "poids_entries";
        private const string ExercicesKey = "exercise_list";
        private List<string> exercices = new();
        private List<PoidsEntry> poids = new();
        public PoidsService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<PoidsEntry>> GetEntriesAsync() =>
            await _localStorage.GetItemAsync<List<PoidsEntry>>(StorageKey) ?? new();

        public async Task AddEntryAsync(PoidsEntry entry)
        {
            var entries = await GetEntriesAsync();
            entries.RemoveAll(e => e.Exercice == entry.Exercice && e.Date.Date == entry.Date.Date);
            entries.Add(entry);
            await _localStorage.SetItemAsync(StorageKey, entries);
        }

        public async Task OverwriteEntriesAsync(List<PoidsEntry> entries) =>
            await _localStorage.SetItemAsync(StorageKey, entries);

        public async Task RemoveEntryAsync(string exercice, DateTime date)
        {
            var entries = await GetEntriesAsync();
            entries.RemoveAll(e => e.Exercice == exercice && e.Date.Date == date.Date);
            await _localStorage.SetItemAsync(StorageKey, entries);
        }

        public async Task RemoveDateAsync(DateTime date)
        {
            var entries = await GetEntriesAsync();
            entries.RemoveAll(e => e.Date.Date == date.Date);
            await _localStorage.SetItemAsync(StorageKey, entries);
        }

        public async Task RemoveExerciceAsync(string exercice)
        {
            var entries = await GetEntriesAsync();
            entries.RemoveAll(e => e.Exercice == exercice);
            await _localStorage.SetItemAsync(StorageKey, entries);
        }
        public async Task RemoveEntriesForExerciceAsync(string exercice)
        {
            var entries = await GetEntriesAsync();
            entries.RemoveAll(e => e.Exercice == exercice);
            await _localStorage.SetItemAsync("poids_entries", entries);
        }

        public async Task<List<string>> GetExercicesAsync()
        {
            var list = await _localStorage.GetItemAsync<List<string>>(ExercicesKey);

            if (list == null || list.Count == 0)
            {
                list = new() { "Développé couché", "Dips", "Squat" };
                await _localStorage.SetItemAsync(ExercicesKey, list);
            }

            return list;
        }

        //  Sauvegarder la liste des exercices
        public async Task SaveExercicesAsync(List<string> list)
        {
            await _localStorage.SetItemAsync(ExercicesKey, list);
        }
        public async Task RemoveByIdAsync(Guid id)
        {
            var entries = await GetEntriesAsync();
            entries.RemoveAll(e => e.Id == id);
            await _localStorage.SetItemAsync("poids_entries", entries);
        }
        public async Task<List<string>> GetExercicesFromEntriesAsync()
        {
            var entries = await GetEntriesAsync();
            return entries
                .Select(e => e.Exercice)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
    }
}
