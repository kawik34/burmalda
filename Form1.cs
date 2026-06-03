using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PhotoWordSearch
{
    public partial class Form1 : Form
    {
        // База фотографий: ключевое слово -> описание + путь
        private Dictionary<string, PhotoItem> photoDatabase;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Заполняем "альбом". Ключ — слово, по которому идёт поиск.
            // !!! Замените FilePath на ваши реальные пути !!!
            photoDatabase = new Dictionary<string, PhotoItem>(StringComparer.OrdinalIgnoreCase)
            {
                { "тюльпаны", new PhotoItem("Тюльпаны весной", @"C:\Users\Колледж-Студент\source\repos\WinFormsApp8\WinFormsApp8\tyulpany-rozovye.jpg") },
                { "горы",     new PhotoItem("Горный пейзаж",     @"C:\Users\Колледж-Студент\source\repos\WinFormsApp8\WinFormsApp8\samye-vysokie-gory-v-mire.jpg") },
                { "закат",    new PhotoItem("Морской закат",     @"C:\Users\Колледж-Студент\source\repos\WinFormsApp8\WinFormsApp8\негр.jpg") },
                { "кот",      new PhotoItem("Милый котёнок",     @"C:\Users\Колледж-Студент\source\repos\WinFormsApp8\WinFormsApp8\кот.jpg") }
            };

            labelCaption.Text = "Введите слово и нажмите «Найти»";
        }

        // Поиск по нажатию кнопки
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SearchPhoto(textBoxSearch.Text.Trim());
        }

        // Поиск по нажатию Enter в поле ввода
        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchPhoto(textBoxSearch.Text.Trim());
                e.SuppressKeyPress = true; // убираем системный звук "бип"
            }
        }

        private void SearchPhoto(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                labelCaption.Text = "Введите ключевое слово";
                pictureBoxPhoto.Image = null;
                return;
            }

            // Ищем слово (регистр не важен благодаря OrdinalIgnoreCase)
            if (photoDatabase.TryGetValue(keyword, out PhotoItem photo))
            {
                if (File.Exists(photo.FilePath))
                {
                    try
                    {
                        // Освобождаем предыдущее изображение
                        if (pictureBoxPhoto.Image != null)
                        {
                            pictureBoxPhoto.Image.Dispose();
                            pictureBoxPhoto.Image = null;
                        }

                        pictureBoxPhoto.Image = Image.FromFile(photo.FilePath);
                        labelCaption.Text = photo.Caption;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Не удалось открыть файл:\n{ex.Message}",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        pictureBoxPhoto.Image = null;
                        labelCaption.Text = "Ошибка загрузки изображения";
                    }
                }
                else
                {
                    pictureBoxPhoto.Image = null;
                    labelCaption.Text = $"Файл не найден: {photo.FilePath}";
                    MessageBox.Show(
                        $"Файл не найден:\n{photo.FilePath}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            else
            {
                pictureBoxPhoto.Image = null;
                labelCaption.Text = $"Фото по запросу «{keyword}» не найдено";
            }
        }
    }

    // Класс-описание фотографии
    public class PhotoItem
    {
        public string Caption { get; set; }
        public string FilePath { get; set; }

        public PhotoItem(string caption, string filePath)
        {
            Caption = caption;
            FilePath = filePath;
        }
    }
}
