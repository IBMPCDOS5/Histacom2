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
using Histacom2.OS.Win98.Win98Apps;

namespace Histacom2.OS.Win98
{
    public partial class Windows98 : Form
    {
        private SoundPlayer startsound;
        public WindowManager wm = new WindowManager();

        public List<WinClassic> nonimportantapps = new List<WinClassic>();
        public WinClassic webchat;
        public WinClassic ie;

        public TaskBarController tb = new TaskBarController();

        public int currentappcount = 0;

        public bool webchatInstalled = false;

        public bool hiddenpadamsFound = false;

        // Init the form
        public Windows98()
        {
            InitializeComponent();
            startmenu.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            ProgramsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            AccessoriesToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            CommunicationsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            MultimediaToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
            SystemToolsToolStripMenuItem.DropDown.Paint += (sender, args) => Engine.Paintbrush.PaintClassicBorders(sender, args, 2);
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
            UpgradeFileSystem( "98");

            if (currentTheme.defaultWallpaper != null) desktopicons.BackgroundImage = new Bitmap(currentTheme.defaultWallpaper, Width, Height);
            //Start Menu Color - Commented until it works reliably
            //startmenuitems.Renderer = new MyRenderer();
            //ProgramsToolStripMenuItem.DropDown.Renderer = new MyRenderer();

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

            // Update the desktop Icons!

            DesktopController.RefreshDesktopIcons(new ListViewItem[] { new System.Windows.Forms.ListViewItem("My Computer", 0),
            new System.Windows.Forms.ListViewItem("Network Neighborhood", 5),
            new System.Windows.Forms.ListViewItem("Inbox", 3),
            new System.Windows.Forms.ListViewItem("Recycle Bin", 7),
            new System.Windows.Forms.ListViewItem("Internet Explorer", 2),
            new System.Windows.Forms.ListViewItem("Online Services", 1),
            new System.Windows.Forms.ListViewItem("Set Up The Microsoft Network", 4),
            new System.Windows.Forms.ListViewItem("Outlook Express", 6) }, ref desktopicons, Path.Combine(ProfileWindowsDirectory, "Desktop"));
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
            startmenu.BringToFront();
            if (taskbar.Visible) taskbar.BringToFront();
        }

        // Shutdown button
        private void ShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGame();
            Program.ShutdownApplication(currentTheme.stopSound);
        }

        #endregion //Region

        // Give Year Code - NYI
        private void taskbartime_Click(object sender, EventArgs e)
        {
            
        }

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
                }
                else
                {
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
            WinClassic app = wm.Init(opendownload, "Downloader", null, false, true);
            opendownload.appName.Text = "Downloading: Survive The Day";

            AddTaskBarItem(app, app.Tag.ToString(), "Downloader", null);

            app.BringToFront();
            startmenu.Hide();
        }

        private void installerTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Win95Installer openinstaller = new Win95Installer("Testing");
            WinClassic app = wm.Init(openinstaller, "Installer", null, false, true);

            AddTaskBarItem(app, app.Tag.ToString(), "Installer", null);

            app.BringToFront();
            startmenu.Hide();
        }

        private void InternetExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ie != null) { wm.StartInfobox95("Error Opening Internet Explorer", "An instance of Internet Explorer 4 is already open.", InfoboxType.Warning, InfoboxButtons.OK); return; }
            ie = wm.Init(new WinClassicIE4(), "Internet Explorer 4", Properties.Resources.Win95IconIE4, true, true);
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
                        ie = wm.Init(new WinClassicIE4(), "Internet Explorer 4", Properties.Resources.Win95IconIE4, true, true);
                        AddTaskBarItem(ie, ie.Tag.ToString(), "Internet Explorer 4", Properties.Resources.Win95IconIE4);
                        ie.BringToFront();
                        ie.FormClosing += new FormClosingEventHandler(InternetExplorer4_Closing);
                        startmenu.Hide();
                    }
                    else if (objListViewItem.Text == "My Computer")
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
                    else if (objListViewItem.Text == "Set Up The Microsoft Network")
                    {
                        wm.StartInfobox95("Microsoft Network", "The Microsoft Network is already set up!", InfoboxType.Info, InfoboxButtons.OK);
                    }
                    else if (objListViewItem.Text == "Outlook Express")
                    {
                        //wm.StartInfobox95("Win32 Application", "That is not a valid Win32 Application.", Properties.Resources.Win95Error);
                    }
                    else if (objListViewItem.Text == "Inbox")
                    {
                        //wm.StartInfobox95("Win32 Application", "That is not a valid Win32 Application.", Properties.Resources.Win95Error);
                    }
                    else
                    {
                        // It is an actual file on the disk

                        WinClassicWindowsExplorer we = new WinClassicWindowsExplorer();

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
            WinClassic app = wm.StartInfobox95("AShifter's Infobox", "This is the very first Histacom2 Infobox. It's really easy to call, too! \n Just use wm.startInfobox95(string title, string text, InfoboxType type, InfoboxButtons btns)!", InfoboxType.Info, InfoboxButtons.OK);

            app.BringToFront();
            startmenu.Hide();
        }
        private void WebChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebChat1998 wc = new WebChat1998();
            WinClassic app = wm.Init(wc, "Web Chat 1998", null, true, true);

            AddTaskBarItem(app, app.Tag.ToString(), "Web Chat 1998", null);

            app.BringToFront();
            startmenu.Hide();
        }
        public void NonImportantApp_Closing(object sender, FormClosingEventArgs e)
        {
            nonimportantapps.Remove((WinClassic)sender);
        }
        private void InternetExplorer4_Closing(object sender, FormClosingEventArgs e)
        {
            ie = null;
        }

        private void WordPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinClassicWordPad wp = new WinClassicWordPad();
            WinClassic app = wm.Init(wp, "Wordpad", Properties.Resources.Win95IconWordpad, true, true);
            AddTaskBarItem(app, app.Tag.ToString(), "Wordpad", Properties.Resources.Win95IconWordpad);

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

        private void AddressBookToolStripMenuItem_Click(object sender, EventArgs e)
        {

            WinClassic app = wm.Init(new WinClassicAddressBook(), "Address Book", Properties.Resources.WinClassicAddressBook, true, true);
            Program.AddTaskbarItem(app, app.Tag.ToString(), "Address Book", Properties.Resources.WinClassicAddressBook);

            Program.nonimportantapps.Add(app);
            Program.nonimportantapps[Program.nonimportantapps.Count - 1].BringToFront();
            Program.nonimportantapps[Program.nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(Program.NonImportantApp_Closing);

            app.BringToFront();
        }

        private void WindowsExplorerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileDialogBoxManager.IsInOpenDialog = false;
            FileDialogBoxManager.IsInSaveDialog = false;
            WinClassic app = wm.Init(new WinClassicWindowsExplorer(), "Windows Explorer", Properties.Resources.WinClassicFileExplorer, true, true);
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

        private void MSDOSPromptToolStripMenuItem1_Click(object sender, EventArgs e)
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
            WinClassic app = wm.Init(theme, "Themes", null, false, true, false);

            AddTaskBarItem(app, app.Tag.ToString(), "Themes", null);
            app.BringToFront();
            startmenu.Hide();
        }

        private void TimeDistorterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //distort = new Histacom2WinClassicTimeDistorter("1998", "1999", 150, Hack2.StartObjective);
            //WinClassic app = wm.StartWin95(distort, "Time Distorter", null, false, true);
            //AddTaskBarItem(app, app.Tag.ToString(), "Time Distorter", null);
            //app.BringToFront();
            //startmenu.Hide();
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
            WinClassic app = wm.Init(new WinClassicCalculator(), "Calculator", Properties.Resources.WinClassicCalc, false, false);
            AddTaskBarItem(app, app.Tag.ToString(), "Calculator", Properties.Resources.WinClassicCalc);

            nonimportantapps.Add(app);
            nonimportantapps[nonimportantapps.Count - 1].BringToFront();
            nonimportantapps[nonimportantapps.Count - 1].FormClosing += new FormClosingEventHandler(NonImportantApp_Closing);

            app.BringToFront();
            startmenu.Hide();
        }

        private void desktopupdate_Tick(object sender, EventArgs e)
        {
            DesktopController.RefreshDesktopIcons(new ListViewItem[] { new System.Windows.Forms.ListViewItem("My Computer", 0),
            new System.Windows.Forms.ListViewItem("Network Neighborhood", 5),
            new System.Windows.Forms.ListViewItem("Inbox", 3),
            new System.Windows.Forms.ListViewItem("Recycle Bin", 7),
            new System.Windows.Forms.ListViewItem("Internet Explorer", 2),
            new System.Windows.Forms.ListViewItem("Online Services", 1),
            new System.Windows.Forms.ListViewItem("Set Up The Microsoft Network", 4),
            new System.Windows.Forms.ListViewItem("Outlook Express", 6) }, ref desktopicons, Path.Combine(ProfileWindowsDirectory, "Desktop"));
        }

        private void FolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(ProfileWindowsDirectory, "Desktop", "New Folder")))
            {
                wm.StartAboutBox95("Windows Explorer", "A folder called New Folder already exists - please rename it.", Properties.Resources.Win95Error);
            }
            else
            {
                SaveDirectoryInfo(Path.Combine(ProfileWindowsDirectory, "Desktop"), "New Folder", false, "New folder", true);
            }
        }

        private void TextDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(ProfileWindowsDirectory, "Desktop", "New Text Document.txt")))
            {
                wm.StartAboutBox95("Windows Explorer", "A folder called New Text Document already exists - please rename it.", Properties.Resources.Win95Error);
            }
            else
            {
                File.Create(Path.Combine(ProfileWindowsDirectory, "Desktop", "New Text Document.txt"));
            }

            desktopupdate_Tick(null, null); // Update the Desktop Icons
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
                    }
                    else
                    {
                        wm.StartInfobox95("Windows Explorer", "This object cannot be deleted.", InfoboxType.Error, InfoboxButtons.OK);
                    }
                }
            }
        }
    }
    public class MyRenderer : ToolStripProfessionalRenderer
    {
        public MyRenderer() : base(new MyColors()) { }
    }

    public class MyColors : ProfessionalColorTable
    {
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.Navy; }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Navy; }
        }
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.Navy; }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.Navy; }
        }
    }
}

