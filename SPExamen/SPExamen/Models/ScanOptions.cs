namespace SPExamen.Models
{
    public class ScanOptions
    {
        /// <summary>
        /// Шлях де будемо сканувати
        /// </summary>
        public string Path { get; set; } = String.Empty;
        /// <summary>
        /// Папка куди буде збегігатися результат
        /// </summary>
        public string DirSave { get; set; } = String.Empty;
        /// <summary>
        /// Слова які ми шукаємо
        /// </summary>
        public string SearchWords { get; set; } = String.Empty;
        /// <summary>
        /// Якщо обрали файл
        /// </summary>
        public bool IsFile { get; set; }
        /// <summary>
        /// Файл, який ми обрали
        /// </summary>
        public IFormFile? FileWords { get; set; }
    }
}
