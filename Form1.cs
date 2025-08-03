namespace BasicFileEditor
{
    public partial class Form1 : Form
    {
        // Add this field to the class to fix CS0103 (currentFilePath does not exist)
        private string? currentFilePath;

        public Form1()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog openNewFile = new OpenFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "Open a File"
                };

                if (openNewFile.ShowDialog() == DialogResult.OK)
                {

                    currentFilePath = openNewFile.FileName;

                    using (StreamReader reader = new StreamReader(currentFilePath))
                    {
                        textBox1.Text = reader.ReadToEnd();
                    }

                    textBox2.Text = Path.GetFileName(currentFilePath);
                }
                else
                {
                    MessageBox.Show("No file selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Allow copy if it's a .txt file
                string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null && files.Length > 0 && Path.GetExtension(files[0]).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {

            string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0)
            {
                try
                {
                    string filePath = files[0];
                    currentFilePath = filePath;

                    string content = File.ReadAllText(files[0]);
                    textBox1.Text = content;
                    textBox2.Text = Path.GetFileNameWithoutExtension(filePath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading file:\n" + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentFilePath))
                {
                    MessageBox.Show("No file is currently open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(currentFilePath))
                    {
                        writer.Write(textBox1.Text);
                    }
                    MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Clear();
                textBox2.Clear();
                currentFilePath = null;
            }
            catch
            {
                MessageBox.Show("An error occurred while resetting the form.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
