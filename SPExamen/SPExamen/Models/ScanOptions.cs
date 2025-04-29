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
    }
}
