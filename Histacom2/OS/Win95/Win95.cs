﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Histacom2.Engine;
using Histacom2.Engine.Template;
using Histacom2.Engine.Template.Taskbars;
using Histacom2.OS.Win95.Win95Apps;
using Histacom2.OS.Win95.Win95Apps.Story;
using static Histacom2.Engine.SaveSystem;
using Histacom2.OS.Win95.Win95Apps._12padamViruses;
using Histacom2.OS.Win95.Win95Apps._12padamsViruses;

namespace Histacom2.OS.Win95
{
    public partial class Windows95 : Form
    {
        private SoundPlayer startsound;
        public WindowManager wm = new WindowManager();

        public List<WinClassic> nonimportantapps = new List<WinClassic>();
        public WinClassic webchat;
        public WinClassic ie;
        public WinClassic welcome;
        public WinClassicTimeDistorter distort;
        public TaskBarController tb = new TaskBarController();

        public int CurrentAppCount = 0;

        public bool WebChatInstalled = false;

        public bool HiddenPadamsFound = false;

        ListViewItem heldDownItem;
        Point heldDownPoint;

        // Init the form
        public Windows95()
        {
            InitializeComponent();
            //startmenu.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            ProgramsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            AccessoriesToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            CommunicationsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            MultimediaToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            GamesToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            StartUpToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            MSDOSPromptToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            DocumentsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            SettingsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            FindToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackgroundImage = currentTheme.defaultWallpaper;
            foreach (ToolStripMenuItem item in startmenuitems.Items)
            {
                item.MouseEnter += new EventHandler(MenuItem_MouseEnter);
                item.MouseLeave += new EventHandler(MenuItem_MouseLeave);
            }
            foreach (ToolStripMenuItem item in ProgramsToolStripMenuItem.DropDown.Items)
            {
                item.MouseEnter += new EventHandler(MenuItem_MouseEnter);
                item.MouseLeave += new EventHandler(MenuItem_MouseLeave);
            }
        }

        private void MenuItem_MouseEnter(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).ForeColor = Color.White;
        }

        private void MenuItem_MouseLeave(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).ForeColor = Color.Black;
        }

        //  When New Game is clicked in TitleScreen.cs
        private void Desktop_Load(object sender, EventArgs e)
        {
            if (currentTheme.defaultWallpaper != null) desktopicons.BackgroundImage = new Bitmap(currentTheme.defaultWallpaper, Width, Height);

            // Make Font Mandatory
            fontLoad();
            
            // Play Windows 95 Start Sound
            Stream audio = currentTheme.startSound;
            startsound = new SoundPlayer(audio);
            startsound.Play();

            // Hide the Startmenu
            startmenu.Hide();

            // Check for and set VM Mode
            if (this.FormBorderStyle != FormBorderStyle.None)
            {
                this.Text = "Histacom2 - VM Mode";
            }

            // Start the ClockTimer
            clockTimer.Start();

            // Set the StartMenu seperator
            startmenuitems.Items.Insert(6, new ToolStripSeparator());

            //nonimportantapps.Capacity = 100;
            this.SendToBack();

            // Update the taskbar
            UpdateTaskbar();

            // Bring to this the front
            this.BringToFront();

            //Check if it is the first time
            if (CurrentSave.FTime95 == false)
            {
                CurrentSave.FTime95 = true;
                SaveGame();
                welcome = wm.Init(new WinClassicWelcome(), "Welcome", null, false, false, resize: false);
                AddTaskBarItem(welcome, welcome.Tag.ToString(), "Welcome", null);

                nonimportantapps.Add(welcome);
                nonimportantapps[nonimportantapps.Count - 1].BringToFront();
                nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

                welcome.BringToFront();
                welcome.Activate();
            }

            // Update the Desktop icons

            DesktopController.RefreshDesktopIcons(new ListViewItem[] { new ListViewItem("My Computer", 0),
            new ListViewItem("Network Neighborhood", 5),
            new ListViewItem("Inbox", 3),
            new ListViewItem("Recycle Bin", 7),
            new ListViewItem("Internet Explorer", 2),
            new ListViewItem("Online Services", 1),
            new ListViewItem("Set Up The Microsoft Network", 4),
            new ListViewItem("Outlook Express", 6) }, ref desktopicons, Path.Combine(ProfileWindowsDirectory, "Desktop"));
            desktopicons.AutoArrange = false;
        }

        private void fontLoad()
        {
            this.taskbartime.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.ProgramsToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.DocumentsToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.SettingsToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FindToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.HelpToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.RunToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.SuspendToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.ShutdownToolStripMenuItem.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.desktopicons.Font = new Font(TitleScreen.pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        }

        #region StartMenu

        // StartButton Click
        private void startbutton_Click(object sender, EventArgs e)
        {
            startmenu.Show();
            if (taskbar.Visible) taskbar.BringToFront();
            startmenu.BringToFront();
        }

        // Shutdown button
        private void ShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGame();
            Program.ShutdownApplication(currentTheme.stopSound);
        }

        #endregion //Region

        // Set the Clock
        private void clockTimer_Tick(object sender, EventArgs e)
        {
            taskbartime.Text = DateTime.Now.ToString("h:mm tt");
        }

        // On Desktop MouseDown
        private void desktop_mousedown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Check if an item is selected and if so show the Delete option

                if (desktopicons.FocusedItem != null)
                {
                    deleteToolStripMenuItem.Visible = true;
                } else {
                    deleteToolStripMenuItem.Visible = false;
                }

                desktopupdate_Tick(null, null); // Update the Desktop Icons

                rightclickbackproperties.Show();
                rightclickbackproperties.BringToFront();
                rightclickbackproperties.Location = MousePosition;
            }

            // If 
            else if (e.Button == MouseButtons.Left)
            {
                rightclickbackproperties.Hide();
                startmenu.Hide();

                heldDownItem = desktopicons.GetItemAt(e.X, e.Y);
                if (heldDownItem != null)
                {
                    heldDownPoint = new Point(e.X - heldDownItem.Position.X,
                                              e.Y - heldDownItem.Position.Y);
                }
            }

            else if (e.Button == MouseButtons.Middle)
            {
                rightclickbackproperties.Hide();
            }
        }

        private void NotePadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassicNotepad wp = new WinClassicNotepad();
            WinClassic app = wm.Init(wp, "Notepad", Properties.Resources.Win95IconNotepad, true, true);
            AddTaskBarItem(app, app.Tag.ToString(), "Notepad", Properties.Resources.Win95IconNotepad);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }
        private void downloaderTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassicDownloader opendownload = new WinClassicDownloader();
            WinClassic app = wm.Init(opendownload, "Downloader", null, false, true, resize: false);
            opendownload.appName.Text = "Downloading: Survive The Day";

            AddTaskBarItem(app, app.Tag.ToString(), "Downloader", null);

            app.BringToFront();
            startmenu.Hide();
        }

        private void installerTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Win95Installer openinstaller = new Win95Installer("Testing");
            WinClassic app = wm.Init(openinstaller, "Installer", null, false, true, resize: false);

            AddTaskBarItem(app, app.Tag.ToString(), "Installer", null);

            app.BringToFront();
            startmenu.Hide();
        }

        private void InternetExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ie != null) { wm.StartInfobox95("Error Opening Internet Explorer", "An instance of Internet Explorer 4 is already open.", InfoboxType.Warning, InfoboxButtons.OK); return; }
            ie = wm.Init(new WinClassicIE3(), "Internet Explorer 4", Properties.Resources.Win95IconIE4, true, true);
            AddTaskBarItem(ie, ie.Tag.ToString(), "Internet Explorer 4", Properties.Resources.Win95IconIE4);
            ie.BringToFront();
            ie.FormClosing += new FormClosingEventHandler(InternetExplorer4_Closing);
            startmenu.Hide();
        }

        private void desktopicons_DoubleClick(object sender, EventArgs e)
        {
            Point objDrawingPoint = desktopicons.PointToClient(Cursor.Position);
            ListViewItem objListViewItem;

            if (objDrawingPoint != null)
            {
                objListViewItem = desktopicons.GetItemAt(objDrawingPoint.X, objDrawingPoint.Y);
                if (objListViewItem != null)
                {
                    if (objListViewItem.Text == "Internet Explorer")
                    {
                        if (ie != null) { wm.StartInfobox95("Error Opening Internet Explorer", "An instance of Internet Explorer 4 is already open.", InfoboxType.Warning, InfoboxButtons.OK); return; }
                        ie = wm.Init(new WinClassicIE3(), "Internet Explorer 4", Properties.Resources.Win95IconIE4, true, true);
                        AddTaskBarItem(ie, ie.Tag.ToString(), "Internet Explorer 4", Properties.Resources.Win95IconIE4);
                        ie.BringToFront();
                        ie.FormClosing += new FormClosingEventHandler(InternetExplorer4_Closing);
                        startmenu.Hide();
                    }
                    else if (objListViewItem.Text == "My Computer") // TODO: Implement slightly limited explorer (with no treeview and a new window each time ya go into a dir)
                    {
                        WinClassic app = wm.Init(new Win95WindowsExplorer(), "Windows Explorer", Properties.Resources.WinClassicFileExplorer, true, true);
                        AddTaskBarItem(app, app.Tag.ToString(), "Windows Explorer", Properties.Resources.WinClassicFileExplorer);
                        app.BringToFront();
                        startmenu.Hide();
                    }
                    else if (objListViewItem.Text == "Network Neighborhood")
                    {
                        // Alex's TODO here

                    }
                    else if (objListViewItem.Text == "Recycle Bin")
                    {
                        // Another thing you may need to digital poke Alex about doing.

                    }
                    else if (objListViewItem.Text == "Set Up The Microsoft Network") {
                        wm.StartInfobox95("Microsoft Network", "The Microsoft Network is already set up!", InfoboxType.Info, InfoboxButtons.OK);
                    } else if (objListViewItem.Text == "Outlook Express") {
                        //wm.StartInfobox95("Win32 Application", "That is not a valid Win32 Application.", Properties.Resources.Win95Error);
                    }
                    else if (objListViewItem.Text == "Inbox")
                    {
                        //wm.StartInfobox95("Win32 Application", "That is not a valid Win32 Application.", Properties.Resources.Win95Error);
                    }
                    else if (objListViewItem.Text == "Online Services")
                    {
                        wm.StartInfobox95("Caught it!", "If you were to run this, the game would crash!\nLuckily, it won't crash this time!", InfoboxType.Error, InfoboxButtons.OK);
                    }
                   
                    else
                    {
                        // It is an actual file on the disk

                        Win95WindowsExplorer we = new Win95WindowsExplorer();

                        // If it is a directory

                        if (Directory.Exists(objListViewItem.Tag.ToString()))
                        {
                            we.CurrentDirectory = objListViewItem.Tag.ToString();

                            WinClassic app = wm.Init(we, "Windows Explorer", Properties.Resources.WinClassicFileExplorer, true, true);
                            AddTaskBarItem(app, app.Tag.ToString(), "Windows Explorer", Properties.Resources.WinClassicFileExplorer);
                            app.BringToFront();
                            startmenu.Hide();
                        }
                        else
                        {
                            // Just open the file...

                            we.OpenFile(objListViewItem.Tag.ToString());
                        }

                    }
                }
            }
        }

        private void infoboxTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassic app = wm.StartInfobox95("AShifter's Infobox", "This is the very first Histacom2 Infobox. It's really easy to call, too! \nJust use wm.startInfobox95(string title, string text, InfoboxType type, InfoboxButtons btns)!", InfoboxType.Info, InfoboxButtons.OK);

            app.BringToFront();
            startmenu.Hide();
        }
        private void WebChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webchat = wm.Init(new WebChat1998(), "Web Chat 1998", null, true, true);

            AddTaskBarItem(webchat, webchat.Tag.ToString(), "Web Chat 1998", null);

            webchat.BringToFront();
            startmenu.Hide();
        }
        public void NonImportantApp_Closing(object sender, FormClosingEventArgs e)
        {
            nonimportantapps.Remove((WinClassic)sender);
        }
        public void InternetExplorer4_Closing(object sender, FormClosingEventArgs e)
        {
            ie = null;
        }

        private void WordPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassicWordPad wp = new WinClassicWordPad();
            WinClassic app = wm.Init(wp, "Wordpad", Properties.Resources.Win95WordpadIcon2, true, true);
            AddTaskBarItem(app, app.Tag.ToString(), "Wordpad", Properties.Resources.Win95WordpadIcon2);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        public void AddTaskBarItem(Form Application, string ApplicationID, string ApplicationName, Image ApplicationIcon)
        {
            taskbarItems = tb.AddTaskbarItem95(ApplicationID, ApplicationName, ApplicationIcon, (UserControl)new Win95TaskBarItem(), taskbarItems);
            Application.FormClosed += new FormClosedEventHandler(UpdateTaskbarFromClosedApplication);
        }

        public void UpdateTaskbarFromClosedApplication(object sender, FormClosedEventArgs e)
        {
            UpdateTaskbar();
        }

        public void UpdateTaskbar()
        {
            // Clears out all the items on the taskbar
            taskbarItems.Controls.Clear();

            // Loops through all the Applications which are open

            foreach (Form form in tb.GetAllOpenApps())
            {
                // Calls that "AddToTaskbar" thing
                taskbarItems = tb.AddTaskbarItem95(form.Tag.ToString(), form.Text.ToString(), (Image)form.Icon.ToBitmap(), (UserControl)new Win95TaskBarItem(), taskbarItems);
            }
        }

        private void WindowsExplorerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileDialogBoxManager.IsInOpenDialog = false;
            FileDialogBoxManager.IsInSaveDialog = false;
            Win95WindowsExplorer we = new Win95WindowsExplorer();
            WinClassic app = wm.Init(we, "Windows Explorer", Properties.Resources.WinClassicFileExplorer, true, true);
            AddTaskBarItem(app, app.Tag.ToString(), "Windows Explorer", Properties.Resources.WinClassicFileExplorer);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void storyTest1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hack1.StartObjective();
        }

        private void temp_for_std(object sender, EventArgs e)
        {
            Win2K.Win2KApps.SurviveTheDay std = new Win2K.Win2KApps.SurviveTheDay();
            WinClassic app = wm.Init(std, "Survive The Day", null, false, false);
            AddTaskBarItem(app, app.Tag.ToString(), "Survive The Day", null);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void MSDOSPromptToolStripMenuItem1_Click (object sender, EventArgs e)
        {
            WinClassicTerminal msdos = new WinClassicTerminal(false);
            WinClassic app = wm.Init(msdos, "MS-DOS Prompt", Properties.Resources.MSDOSPromptToolStripMenuItem1_Image, true, true, false);

            AddTaskBarItem(app, app.Tag.ToString(), "MS-DOS Prompt", Properties.Resources.MSDOSPromptToolStripMenuItem1_Image);
            app.BringToFront();
            startmenu.Hide();
        }

        private void PropertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WinClassicThemePanel theme = new WinClassicThemePanel();
            WinClassic app = wm.Init(theme, "Themes", null, false, true, false, resize: false);

            AddTaskBarItem(app, app.Tag.ToString(), "Themes", null);
            app.BringToFront();
            startmenu.Hide();
        }

        private void TimeDistorterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distort = new WinClassicTimeDistorter("1998", "1999", 150, Hack2.StartObjective);
            WinClassic app = wm.Init(distort, "Time Distorter", null, false, true);
            AddTaskBarItem(app, app.Tag.ToString(), "Time Distorter", null);
            app.BringToFront();
            startmenu.Hide();
        }
        private void FTPClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassic app = wm.Init(new WinClassicFTPClient(), "FTP Client", null, true, true);

            AddTaskBarItem(app, app.Tag.ToString(), "FTP Client", null);
            app.BringToFront();
            startmenu.Hide();
        }

        private void CalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            WinClassic app = wm.Init(new WinClassicCalculator(), "Calculator", Properties.Resources.WinClassicCalc, false, false, resize: false);
            AddTaskBarItem(app, app.Tag.ToString(), "Calculator", Properties.Resources.WinClassicCalc);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void desktopupdate_Tick(object sender, EventArgs e)
        {
            // Update the Desktop icons

            DesktopController.RefreshDesktopIcons(new ListViewItem[] { new System.Windows.Forms.ListViewItem("My Computer", 0),
            new System.Windows.Forms.ListViewItem("Network Neighborhood", 5),
            new System.Windows.Forms.ListViewItem("Inbox", 3),
            new System.Windows.Forms.ListViewItem("Recycle Bin", 7),
            new System.Windows.Forms.ListViewItem("Internet Explorer", 2),
            new System.Windows.Forms.ListViewItem("Online Services", 1),
            new System.Windows.Forms.ListViewItem("Set Up The Microsoft Network", 4),
            new System.Windows.Forms.ListViewItem("Outlook Express", 6) }, ref desktopicons, Path.Combine(ProfileWindowsDirectory, "Desktop"));
        }

        // When add new folder is clicked
        private void FolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(ProfileWindowsDirectory, "Desktop", "New Folder")))
            {
                wm.StartInfobox95("Windows Explorer", "A folder called New Folder already exists - please rename it.", InfoboxType.Error, InfoboxButtons.OK);
            }
            else
            {
                SaveDirectoryInfo(Path.Combine(ProfileWindowsDirectory, "Desktop"), "New Folder", false, "New folder", true);
                desktopupdate_Tick(null, null); // Update the desktop Icons
            }
        }

        private void TextDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(ProfileWindowsDirectory, "Desktop", "New Text Document.txt")))
            {
                wm.StartInfobox95("Windows Explorer", "A folder called New Text Document already exists - please rename it.", InfoboxType.Error, InfoboxButtons.OK);
            }
            else
            {
                File.Create(Path.Combine(ProfileWindowsDirectory, "Desktop", "New Text Document.txt"));
                desktopupdate_Tick(null, null); // Update the desktop Icons
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point objDrawingPoint = desktopicons.PointToClient(Cursor.Position);
            ListViewItem objListViewItem;

            if (objDrawingPoint != null)
            {
                objListViewItem = desktopicons.GetItemAt(objDrawingPoint.X, objDrawingPoint.Y);
                if (objListViewItem != null)
                {
                    if (objListViewItem.Tag != null)
                    {
                        if (Directory.Exists(objListViewItem.Tag.ToString()))
                        {
                            Directory.Delete(objListViewItem.Tag.ToString(), true);
                            desktopupdate_Tick(null, null); // Update the desktop Icons
                        }
                        else
                        {
                            if (File.Exists(objListViewItem.Tag.ToString()))
                            {
                                File.Delete(objListViewItem.Tag.ToString());
                                desktopupdate_Tick(null, null); // Update the desktop Icons
                            }
                            else
                            {
                                wm.StartInfobox95("Windows Explorer", "This object cannot be deleted.", InfoboxType.Error, InfoboxButtons.OK);
                            }
                        }
                    } else {
                        wm.StartInfobox95("Windows Explorer", "This object cannot be deleted.", InfoboxType.Error, InfoboxButtons.OK);
                    }                 
                }
            }
        }

        private void MinsweeperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassic app = wm.Init(new WinClassicMinesweeper(), "Minesweeper", Properties.Resources.WinClassicMinesweeper, false, false, false, false);
            AddTaskBarItem(app, app.Tag.ToString(), "Minesweeper", Properties.Resources.WinClassicMinesweeper);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void desktopicons_MouseMove(object sender, MouseEventArgs e)
        {
            if (heldDownItem != null)
            {
                heldDownItem.Position = new Point(e.Location.X - heldDownPoint.X,
                                                  e.Location.Y - heldDownPoint.Y);
            }
        }

        private void desktopicons_MouseUp(object sender, MouseEventArgs e)
        {
            heldDownItem = null;
        }

        private void GuessTheNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassic app = wm.Init(new GuessTheNumber(), "Guess The Number", Properties.Resources.WinClassicGTNIcon, false, false, false, false);
            AddTaskBarItem(app, app.Tag.ToString(), "Guess The Number", Properties.Resources.WinClassicGTNIcon);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void taskbar_Paint(object sender, PaintEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.Clear(currentTheme.threeDObjectsColor);

            var _lightBack = Paintbrush.GetLightFromColor(currentTheme.threeDObjectsColor);

            gfx.DrawLine(new Pen(_lightBack), 0, 1, taskbar.Width, 1);
        }

        private void clockPanel_Paint(object sender, PaintEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.Clear(currentTheme.threeDObjectsColor);

            var _lightBack = Paintbrush.GetLightFromColor(currentTheme.threeDObjectsColor);
            var _darkBack = Paintbrush.GetDarkFromColor(currentTheme.threeDObjectsColor);

            gfx.DrawLine(new Pen(_lightBack), 0, 1, clockPanel.Width, 1);
            gfx.DrawLine(new Pen(_darkBack), 0, 24, 0, 4);
            gfx.DrawLine(new Pen(_darkBack), 61, 4, 0, 4);
            gfx.DrawLine(new Pen(_lightBack), 62, 4, 62, 25);
            gfx.DrawLine(new Pen(_lightBack), 0, 25, 62, 25);
        }

        private void startmenuitems_Paint(object sender, PaintEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.Clear(currentTheme.threeDObjectsColor);
        }

        private void startmenu_Paint(object sender, PaintEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.Clear(currentTheme.threeDObjectsColor);

            var _lightBack = Paintbrush.GetLightFromColor(currentTheme.threeDObjectsColor);
            var _darkBack = Paintbrush.GetDarkFromColor(currentTheme.threeDObjectsColor);

            gfx.DrawLine(Pens.Black, 0, startmenu.Height - 1, startmenu.Width - 1, startmenu.Height - 1);
            gfx.DrawLine(Pens.Black, startmenu.Width - 1, startmenu.Height - 1, startmenu.Width - 1, 0);
            gfx.DrawLine(new Pen(_darkBack), 1, startmenu.Height - 2, startmenu.Width - 2, startmenu.Height - 2);
            gfx.DrawLine(new Pen(_darkBack), startmenu.Width - 2, 1, startmenu.Width - 2, startmenu.Height - 2);
            gfx.DrawLine(new Pen(_lightBack), 1, startmenu.Height - 3, 1, 1);
            gfx.DrawLine(new Pen(_lightBack), startmenu.Width - 3, 1, 1, 1);
        }

        private void ErrorBlasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassic app = wm.Init(new ErrorBlaster95(), "Welcome to Error Blaster 95!", null, false, false, false, false);
            AddTaskBarItem(app, app.Tag.ToString(), "Welcome to Error Blaster 95!", null);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void FilesOrFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassic app = wm.Init(new StartRunner95(this), "Welcome to Start Runner 95!", null, false, false, false, false);
            AddTaskBarItem(app, app.Tag.ToString(), "Welcome to Start Runner 95!", null);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void StartRunnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wm.Init(new StartRunner95(this), "Welcome to Start Runner 95!", null, false, false, false);
        }
    }
}

