namespace SystAnalys_lr1
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBoxMatrix = new System.Windows.Forms.ListBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.особыеДействияСИзображениемToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.расположитьВершиныНаОкружностиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.нарисоватьДополнениеГрафаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.нарисоватьОдинИзОстововToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изобразитьПримерПервообразногоГрафаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьРаскраскуГрафаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьПримерНаибольшегоКликаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изобразитьГрафКликToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.действияВозвратаИОчисткиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очиститьТекстовуюПанельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вернутьИсходноеИзображениеГрафаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.какПользоватьсяПрограммойToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.chainButton = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            this.buttonInc = new System.Windows.Forms.Button();
            this.buttonAdj = new System.Windows.Forms.Button();
            this.deleteALLButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.drawEdgeButton = new System.Windows.Forms.Button();
            this.drawVertexButton = new System.Windows.Forms.Button();
            this.sheet = new System.Windows.Forms.PictureBox();
            this.cycleButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxMatrix
            // 
            this.listBoxMatrix.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxMatrix.FormattingEnabled = true;
            this.listBoxMatrix.HorizontalScrollbar = true;
            this.listBoxMatrix.ItemHeight = 18;
            this.listBoxMatrix.Location = new System.Drawing.Point(733, 101);
            this.listBoxMatrix.Name = "listBoxMatrix";
            this.listBoxMatrix.Size = new System.Drawing.Size(705, 364);
            this.listBoxMatrix.TabIndex = 6;
            // 
            // saveButton
            // 
            this.saveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveButton.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveButton.Location = new System.Drawing.Point(1005, 474);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(144, 61);
            this.saveButton.TabIndex = 13;
            this.saveButton.Text = "Сохранить граф (рисунок)";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Yellow;
            this.button1.Cursor = System.Windows.Forms.Cursors.Help;
            this.button1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(742, 471);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 67);
            this.button1.TabIndex = 14;
            this.button1.Text = "Узнать больше";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Orange;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(1327, 475);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 60);
            this.button2.TabIndex = 15;
            this.button2.Text = "Завершить работу";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Chartreuse;
            this.label1.Cursor = System.Windows.Forms.Cursors.No;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(738, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 19);
            this.label1.TabIndex = 16;
            this.label1.Text = "Простые запросы -->";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Chartreuse;
            this.label2.Cursor = System.Windows.Forms.Cursors.No;
            this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 75);
            this.label2.TabIndex = 17;
            this.label2.Text = "^\r\n|\r\n|\r\nЭлементы\r\nрисования";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.YellowGreen;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.особыеДействияСИзображениемToolStripMenuItem,
            this.действияВозвратаИОчисткиToolStripMenuItem,
            this.какПользоватьсяПрограммойToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.Size = new System.Drawing.Size(1450, 25);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "Особые действия с изображением";
            // 
            // особыеДействияСИзображениемToolStripMenuItem
            // 
            this.особыеДействияСИзображениемToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.расположитьВершиныНаОкружностиToolStripMenuItem,
            this.нарисоватьДополнениеГрафаToolStripMenuItem,
            this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem,
            this.нарисоватьОдинИзОстововToolStripMenuItem,
            this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem,
            this.изобразитьПримерПервообразногоГрафаToolStripMenuItem,
            this.показатьРаскраскуГрафаToolStripMenuItem,
            this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem,
            this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem,
            this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem,
            this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem,
            this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem,
            this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem,
            this.показатьПримерНаибольшегоКликаToolStripMenuItem,
            this.изобразитьГрафКликToolStripMenuItem});
            this.особыеДействияСИзображениемToolStripMenuItem.Name = "особыеДействияСИзображениемToolStripMenuItem";
            this.особыеДействияСИзображениемToolStripMenuItem.Size = new System.Drawing.Size(229, 21);
            this.особыеДействияСИзображениемToolStripMenuItem.Text = "Особые действия с изображением";
            this.особыеДействияСИзображениемToolStripMenuItem.Click += new System.EventHandler(this.особыеДействияСИзображениемToolStripMenuItem_Click);
            // 
            // расположитьВершиныНаОкружностиToolStripMenuItem
            // 
            this.расположитьВершиныНаОкружностиToolStripMenuItem.Name = "расположитьВершиныНаОкружностиToolStripMenuItem";
            this.расположитьВершиныНаОкружностиToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.расположитьВершиныНаОкружностиToolStripMenuItem.Text = "Расположить вершины на окружности / построить изоморфный граф";
            this.расположитьВершиныНаОкружностиToolStripMenuItem.Click += new System.EventHandler(this.расположитьВершиныНаОкружностиToolStripMenuItem_Click);
            // 
            // нарисоватьДополнениеГрафаToolStripMenuItem
            // 
            this.нарисоватьДополнениеГрафаToolStripMenuItem.Name = "нарисоватьДополнениеГрафаToolStripMenuItem";
            this.нарисоватьДополнениеГрафаToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.нарисоватьДополнениеГрафаToolStripMenuItem.Text = "Нарисовать дополнение (исходного) графа";
            this.нарисоватьДополнениеГрафаToolStripMenuItem.Click += new System.EventHandler(this.нарисоватьДополнениеГрафаToolStripMenuItem_Click);
            // 
            // нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem
            // 
            this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem.Name = "нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem";
            this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem.Text = "Нарисовать подграф, порождённый вершинами 1, 2, 3, 4";
            this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem.Click += new System.EventHandler(this.нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem_Click);
            // 
            // нарисоватьОдинИзОстововToolStripMenuItem
            // 
            this.нарисоватьОдинИзОстововToolStripMenuItem.Name = "нарисоватьОдинИзОстововToolStripMenuItem";
            this.нарисоватьОдинИзОстововToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.нарисоватьОдинИзОстововToolStripMenuItem.Text = "Нарисовать один из остовов";
            this.нарисоватьОдинИзОстововToolStripMenuItem.Click += new System.EventHandler(this.нарисоватьОдинИзОстововToolStripMenuItem_Click);
            // 
            // изобразитьПримерГомеоморфногоГрафаToolStripMenuItem
            // 
            this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem.Name = "изобразитьПримерГомеоморфногоГрафаToolStripMenuItem";
            this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem.Text = "Изобразить пример гомеоморфного графа";
            this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem.Click += new System.EventHandler(this.изобразитьПримерГомеоморфногоГрафаToolStripMenuItem_Click);
            // 
            // изобразитьПримерПервообразногоГрафаToolStripMenuItem
            // 
            this.изобразитьПримерПервообразногоГрафаToolStripMenuItem.Name = "изобразитьПримерПервообразногоГрафаToolStripMenuItem";
            this.изобразитьПримерПервообразногоГрафаToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.изобразитьПримерПервообразногоГрафаToolStripMenuItem.Text = "Изобразить пример первообразного графа";
            this.изобразитьПримерПервообразногоГрафаToolStripMenuItem.Click += new System.EventHandler(this.изобразитьПримерПервообразногоГрафаToolStripMenuItem_Click);
            // 
            // показатьРаскраскуГрафаToolStripMenuItem
            // 
            this.показатьРаскраскуГрафаToolStripMenuItem.Name = "показатьРаскраскуГрафаToolStripMenuItem";
            this.показатьРаскраскуГрафаToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.показатьРаскраскуГрафаToolStripMenuItem.Text = "Показать раскраску графа на окружности";
            this.показатьРаскраскуГрафаToolStripMenuItem.Click += new System.EventHandler(this.показатьРаскраскуГрафаToolStripMenuItem_Click);
            // 
            // показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem
            // 
            this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem.Name = "показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem";
            this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem.Text = "Показать раскраску графа на оригинальном изображении";
            this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem.Click += new System.EventHandler(this.показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem_Click);
            // 
            // изобразитьОбъединениеРёберныхБлоковToolStripMenuItem
            // 
            this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem.Name = "изобразитьОбъединениеРёберныхБлоковToolStripMenuItem";
            this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem.Text = "Изобразить объединение рёберных блоков";
            this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem.Click += new System.EventHandler(this.изобразитьОбъединениеРёберныхБлоковToolStripMenuItem_Click);
            // 
            // изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem
            // 
            this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem.Name = "изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem";
            this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem.Text = "Изобразить блок, связанный с точками сочленения";
            this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem.Click += new System.EventHandler(this.изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem_Click);
            // 
            // показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem
            // 
            this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem.Name = "показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem";
            this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem.Text = "Показать пример максимального независимого подмножества";
            this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem.Click += new System.EventHandler(this.показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem_Click);
            // 
            // показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem
            // 
            this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem.Name = "показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem";
            this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem.Text = "Показать пример минимального доминирующего подмножества";
            this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem.Click += new System.EventHandler(this.показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem_Click);
            // 
            // показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem
            // 
            this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem.Name = "показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem";
            this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem.Text = "Показать пример наименьшего вершинного покрытия";
            this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem.Click += new System.EventHandler(this.показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem_Click);
            // 
            // показатьПримерНаибольшегоКликаToolStripMenuItem
            // 
            this.показатьПримерНаибольшегоКликаToolStripMenuItem.Name = "показатьПримерНаибольшегоКликаToolStripMenuItem";
            this.показатьПримерНаибольшегоКликаToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.показатьПримерНаибольшегоКликаToolStripMenuItem.Text = "Показать пример наибольшего клика";
            this.показатьПримерНаибольшегоКликаToolStripMenuItem.Click += new System.EventHandler(this.показатьПримерНаибольшегоКликаToolStripMenuItem_Click);
            // 
            // изобразитьГрафКликToolStripMenuItem
            // 
            this.изобразитьГрафКликToolStripMenuItem.Name = "изобразитьГрафКликToolStripMenuItem";
            this.изобразитьГрафКликToolStripMenuItem.Size = new System.Drawing.Size(495, 22);
            this.изобразитьГрафКликToolStripMenuItem.Text = "Изобразить граф клик";
            this.изобразитьГрафКликToolStripMenuItem.Click += new System.EventHandler(this.изобразитьГрафКликToolStripMenuItem_Click);
            // 
            // действияВозвратаИОчисткиToolStripMenuItem
            // 
            this.действияВозвратаИОчисткиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.очиститьТекстовуюПанельToolStripMenuItem,
            this.вернутьИсходноеИзображениеГрафаToolStripMenuItem});
            this.действияВозвратаИОчисткиToolStripMenuItem.Name = "действияВозвратаИОчисткиToolStripMenuItem";
            this.действияВозвратаИОчисткиToolStripMenuItem.Size = new System.Drawing.Size(195, 21);
            this.действияВозвратаИОчисткиToolStripMenuItem.Text = "Действия возврата и очистки";
            // 
            // очиститьТекстовуюПанельToolStripMenuItem
            // 
            this.очиститьТекстовуюПанельToolStripMenuItem.Name = "очиститьТекстовуюПанельToolStripMenuItem";
            this.очиститьТекстовуюПанельToolStripMenuItem.Size = new System.Drawing.Size(308, 22);
            this.очиститьТекстовуюПанельToolStripMenuItem.Text = "Очистить текстовую панель";
            this.очиститьТекстовуюПанельToolStripMenuItem.Click += new System.EventHandler(this.очиститьТекстовуюПанельToolStripMenuItem_Click);
            // 
            // вернутьИсходноеИзображениеГрафаToolStripMenuItem
            // 
            this.вернутьИсходноеИзображениеГрафаToolStripMenuItem.Name = "вернутьИсходноеИзображениеГрафаToolStripMenuItem";
            this.вернутьИсходноеИзображениеГрафаToolStripMenuItem.Size = new System.Drawing.Size(308, 22);
            this.вернутьИсходноеИзображениеГрафаToolStripMenuItem.Text = "Вернуть исходное изображение графа";
            this.вернутьИсходноеИзображениеГрафаToolStripMenuItem.Click += new System.EventHandler(this.вернутьИсходноеИзображениеГрафаToolStripMenuItem_Click);
            // 
            // какПользоватьсяПрограммойToolStripMenuItem
            // 
            this.какПользоватьсяПрограммойToolStripMenuItem.Name = "какПользоватьсяПрограммойToolStripMenuItem";
            this.какПользоватьсяПрограммойToolStripMenuItem.Size = new System.Drawing.Size(207, 21);
            this.какПользоватьсяПрограммойToolStripMenuItem.Text = "Как пользоваться программой";
            this.какПользоватьсяПрограммойToolStripMenuItem.Click += new System.EventHandler(this.какПользоватьсяПрограммойToolStripMenuItem_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(1272, 43);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 45);
            this.button3.TabIndex = 19;
            this.button3.Text = "P(G,x)";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.NavajoWhite;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(1168, 475);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(143, 60);
            this.button4.TabIndex = 20;
            this.button4.Text = "Перезапустить приложение";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.GreenYellow;
            this.button5.Cursor = System.Windows.Forms.Cursors.Help;
            this.button5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button5.Location = new System.Drawing.Point(848, 471);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(135, 67);
            this.button5.TabIndex = 21;
            this.button5.Text = "Задать граф по матрице смежности";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // chainButton
            // 
            this.chainButton.Image = ((System.Drawing.Image)(resources.GetObject("chainButton.Image")));
            this.chainButton.Location = new System.Drawing.Point(1120, 43);
            this.chainButton.Name = "chainButton";
            this.chainButton.Size = new System.Drawing.Size(70, 45);
            this.chainButton.TabIndex = 10;
            this.chainButton.UseVisualStyleBackColor = true;
            this.chainButton.Click += new System.EventHandler(this.chainButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Image = global::SystAnalys_lr1.Properties.Resources.cursor;
            this.selectButton.Location = new System.Drawing.Point(22, 12);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(45, 45);
            this.selectButton.TabIndex = 9;
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // buttonInc
            // 
            this.buttonInc.Image = global::SystAnalys_lr1.Properties.Resources.inc;
            this.buttonInc.Location = new System.Drawing.Point(1022, 39);
            this.buttonInc.Name = "buttonInc";
            this.buttonInc.Size = new System.Drawing.Size(92, 52);
            this.buttonInc.TabIndex = 8;
            this.buttonInc.UseVisualStyleBackColor = true;
            this.buttonInc.Click += new System.EventHandler(this.buttonInc_Click);
            // 
            // buttonAdj
            // 
            this.buttonAdj.Image = global::SystAnalys_lr1.Properties.Resources.smezh;
            this.buttonAdj.Location = new System.Drawing.Point(924, 39);
            this.buttonAdj.Name = "buttonAdj";
            this.buttonAdj.Size = new System.Drawing.Size(92, 52);
            this.buttonAdj.TabIndex = 7;
            this.buttonAdj.UseVisualStyleBackColor = true;
            this.buttonAdj.Click += new System.EventHandler(this.buttonAdj_Click);
            // 
            // deleteALLButton
            // 
            this.deleteALLButton.Image = global::SystAnalys_lr1.Properties.Resources.deleteAll;
            this.deleteALLButton.Location = new System.Drawing.Point(22, 468);
            this.deleteALLButton.Name = "deleteALLButton";
            this.deleteALLButton.Size = new System.Drawing.Size(45, 45);
            this.deleteALLButton.TabIndex = 5;
            this.deleteALLButton.UseVisualStyleBackColor = true;
            this.deleteALLButton.Click += new System.EventHandler(this.deleteALLButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Image = global::SystAnalys_lr1.Properties.Resources.delete;
            this.deleteButton.Location = new System.Drawing.Point(22, 166);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(45, 45);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // drawEdgeButton
            // 
            this.drawEdgeButton.Image = global::SystAnalys_lr1.Properties.Resources.edge;
            this.drawEdgeButton.Location = new System.Drawing.Point(22, 115);
            this.drawEdgeButton.Name = "drawEdgeButton";
            this.drawEdgeButton.Size = new System.Drawing.Size(45, 45);
            this.drawEdgeButton.TabIndex = 2;
            this.drawEdgeButton.UseVisualStyleBackColor = true;
            this.drawEdgeButton.Click += new System.EventHandler(this.drawEdgeButton_Click);
            // 
            // drawVertexButton
            // 
            this.drawVertexButton.Image = global::SystAnalys_lr1.Properties.Resources.vertex;
            this.drawVertexButton.Location = new System.Drawing.Point(22, 64);
            this.drawVertexButton.Name = "drawVertexButton";
            this.drawVertexButton.Size = new System.Drawing.Size(45, 45);
            this.drawVertexButton.TabIndex = 1;
            this.drawVertexButton.UseVisualStyleBackColor = true;
            this.drawVertexButton.Click += new System.EventHandler(this.drawVertexButton_Click);
            // 
            // sheet
            // 
            this.sheet.BackColor = System.Drawing.SystemColors.Control;
            this.sheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sheet.Location = new System.Drawing.Point(89, 12);
            this.sheet.Name = "sheet";
            this.sheet.Size = new System.Drawing.Size(634, 523);
            this.sheet.TabIndex = 0;
            this.sheet.TabStop = false;
            this.sheet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.sheet_MouseClick);
            // 
            // cycleButton
            // 
            this.cycleButton.Image = global::SystAnalys_lr1.Properties.Resources.cycle;
            this.cycleButton.Location = new System.Drawing.Point(1196, 43);
            this.cycleButton.Name = "cycleButton";
            this.cycleButton.Size = new System.Drawing.Size(70, 45);
            this.cycleButton.TabIndex = 11;
            this.cycleButton.UseVisualStyleBackColor = true;
            this.cycleButton.Click += new System.EventHandler(this.cycleButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Chartreuse;
            this.label3.Cursor = System.Windows.Forms.Cursors.No;
            this.label3.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(33, 361);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 88);
            this.label3.TabIndex = 22;
            this.label3.Text = "Г\r\nР\r\nА\r\nФ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(52, 400);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = ">>>>";
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.White;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button6.Location = new System.Drawing.Point(1356, 43);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(78, 45);
            this.button6.TabIndex = 24;
            this.button6.Text = "Metric";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(12, 305);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 40);
            this.button7.TabIndex = 25;
            this.button7.Text = "Выпрямить рёбра";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.Color.DarkGreen;
            this.textBox1.Location = new System.Drawing.Point(733, 28);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(705, 510);
            this.textBox1.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chartreuse;
            this.ClientSize = new System.Drawing.Size(1450, 547);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.chainButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.buttonInc);
            this.Controls.Add(this.buttonAdj);
            this.Controls.Add(this.listBoxMatrix);
            this.Controls.Add(this.deleteALLButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.drawEdgeButton);
            this.Controls.Add(this.drawVertexButton);
            this.Controls.Add(this.sheet);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cycleButton);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Свойства графов. Дм. Па.";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.ListBox listBoxMatrix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem особыеДействияСИзображениемToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem расположитьВершиныНаОкружностиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem нарисоватьДополнениеГрафаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem действияВозвратаИОчисткиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem очиститьТекстовуюПанельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вернутьИсходноеИзображениеГрафаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem какПользоватьсяПрограммойToolStripMenuItem;
        private System.Windows.Forms.PictureBox sheet;
        private System.Windows.Forms.Button drawVertexButton;
        private System.Windows.Forms.Button drawEdgeButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button deleteALLButton;
        private System.Windows.Forms.Button buttonAdj;
        private System.Windows.Forms.Button buttonInc;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button chainButton;
        private System.Windows.Forms.Button cycleButton;
        private System.Windows.Forms.ToolStripMenuItem нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem нарисоватьОдинИзОстововToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изобразитьПримерГомеоморфногоГрафаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изобразитьПримерПервообразногоГрафаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьРаскраскуГрафаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изобразитьОбъединениеРёберныхБлоковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьПримерНаибольшегоКликаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изобразитьГрафКликToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        public System.Windows.Forms.TextBox textBox1;
    }
}

