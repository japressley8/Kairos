using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kairos.Actions;
using Kairos.Actions.Pages;
using Kairos.Triggers;
using Kairos.Triggers.Pages;

namespace Kairos
{
    /// <summary>
    /// Interaction logic for ModuleTabs.xaml
    /// </summary>
    public partial class ModuleTabs : Page
    {
        public List<string> actionType = new List<string>
        {
            "Add a Delay",
            "Launch/Close a Program",
            "Change the Wallpaper",
            "Hide/Show the Taskbar",
            "Hide/Show the Desktop Icons",
            "Change the System Theme",
            "Change the App Theme",
            "Change the Audio Output",
            "Change the Volume",
            "Display a Notification"
        };
        public List<string> triggerType = new List<string>
        {
            "Keyboard Shortcut",
            "During a Time Range",
            "On Startup",
            "On Idle",
            "Any Program is Fullscreen",
            "Specific Program is Running",
            "Computer is Charging"
        };
        public Module module { get; set; }
        private bool isAnd { get; set; }
        public ModuleTabs(Module inModule)
        {
            InitializeComponent();

            module = inModule;
            if (inModule != null)
            {
                InitializeModule();
            }
        }
        //Module
        public void InitializeModule()
        {
            InitializeAttributes();
            InitializeActions();
            InitializeTriggers();
        }
        //Attributes
        private void InitializeAttributes()
        {
            nameBox.Text = module.Name;
            descBox.Text = module.Description;
        }
        private void nameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            module.Name = nameBox.Text;
        }
        private void descBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            module.Description = descBox.Text;
        }
        //Actions
        private void InitializeActions()
        {
            actionList.ItemsSource = module.Actions;
        }
        private void addActionButton_Click(object sender, RoutedEventArgs e)
        {
            if(newActionList.Visibility == Visibility.Visible)
            {
                addActionButton.Content = "Add Action";
                newActionList.Visibility = Visibility.Hidden;
                actionFrame.Visibility = Visibility.Visible;
            }
            else
            {
                addActionButton.Content = "CANCEL";
                newActionList.Visibility = Visibility.Visible;
                actionFrame.Visibility = Visibility.Hidden;
                newActionList.ItemsSource = actionType;
                newActionList.SelectedItem = -1;
            }
        }
        private void newActionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (newActionList.SelectedIndex >= 0)
            {
                addActionButton.Content = "Add Action";
                newAction(newActionList.SelectedItem.ToString());
                actionList.Items.Refresh();
                newActionList.Visibility = Visibility.Hidden;
                actionFrame.Visibility = Visibility.Visible;
                newActionList.SelectedIndex = -1;
            }
        }
        private void newAction(string type)
        {
            BackgroundWorker worker;
            switch (type)
            {
                case "Add a Delay":
                    module.Actions.Add(new Delay());
                    break;
                case "Launch/Close a Program":
                    module.Actions.Add(new ManageProgram());
                    break;
                case "Change the Wallpaper":
                    module.Actions.Add(new Wallpaper());
                    break;
                case "Hide/Show the Taskbar":
                    module.Actions.Add(new Taskbar());
                    break;
                case "Hide/Show the Desktop Icons":
                    module.Actions.Add(new DesktopIcons());
                    break;
                case "Change the System Theme":
                    module.Actions.Add(new SystemTheme());
                    break;
                case "Change the App Theme":
                    module.Actions.Add(new AppTheme());
                    break;
                case "Change the Audio Output":
                    loadingCard.Visibility = Visibility.Visible;
                    worker = new BackgroundWorker();
                    worker.DoWork += OutputHelper;
                    worker.RunWorkerCompleted += WorkerCompleted;
                    worker.RunWorkerAsync();
                    break;
                case "Change the Volume":
                    loadingCard.Visibility = Visibility.Visible;
                    worker = new BackgroundWorker();
                    worker.DoWork += VolumeHelper;
                    worker.RunWorkerCompleted += WorkerCompleted;
                    worker.RunWorkerAsync();
                    break;
                case "Display a Notification":
                    module.Actions.Add(new Notification());
                    break;
            }
        }
        private void OutputHelper(object sender, DoWorkEventArgs e)
        {
            module.Actions.Add(new AudioOutput());
        }
        private void VolumeHelper(object sender, DoWorkEventArgs e)
        {
            module.Actions.Add(new AudioVolume());
        }
        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingCard.Visibility = Visibility.Hidden;
            actionList.Items.Refresh();
        }
        private void actionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!actionList.Items.IsEmpty && actionList.SelectedItem != null)
            {
                switch (((Action)actionList.SelectedItem).Type)
                {
                    case "Add a Delay":
                        actionFrame.Navigate(new DelayPage((Delay)actionList.SelectedItem)); //Can not cast from Kairos.Action to Kairos.Actions.Delay? but only when read in from file
                        break;
                    case "Launch/Close a Program":
                        actionFrame.Navigate(new ManageProgramPage((ManageProgram)actionList.SelectedItem));
                        break;
                    case "Change the Wallpaper":
                        actionFrame.Navigate(new WallpaperPage((Wallpaper)actionList.SelectedItem));
                        break;
                    case "Hide/Show the Taskbar":
                        actionFrame.Navigate(new TaskbarPage((Taskbar)actionList.SelectedItem));
                        break;
                    case "Hide/Show the Desktop Icons":
                        actionFrame.Navigate(new DesktopIconsPage((DesktopIcons)actionList.SelectedItem));
                        break;
                    case "Change the System Theme":
                        actionFrame.Navigate(new SystemThemePage((SystemTheme)actionList.SelectedItem));
                        break;
                    case "Change the App Theme":
                        actionFrame.Navigate(new AppThemePage((AppTheme)actionList.SelectedItem));
                        break;
                    case "Change the Audio Output":
                        actionFrame.Navigate(new AudioOutputPage((AudioOutput)actionList.SelectedItem));
                        break;
                    case "Change the Volume":
                        actionFrame.Navigate(new AudioVolumePage((AudioVolume)actionList.SelectedItem));
                        break;
                    case "Display a Notification":
                        actionFrame.Navigate(new NotificationPage((Notification)actionList.SelectedItem));
                        break;
                }
            }
        }
        private void actionList_RemoveItem(object sender, RoutedEventArgs e)
        {
            module.Actions.Remove((Action)actionList.SelectedItem);
            actionList.Items.Refresh();
            actionFrame.Navigate(null);
            FileMan.Save(module);
        }
        //Triggers
        private void InitializeTriggers()
        {
            DisplayTriggers();
        }
        private void DisplayTriggers()
        {
            containerList.Items.Clear();
            foreach (Container container in module.Containers)
            {
                if (container != module.Containers.First()) //unless it is the first item, add an or button
                {
                    StackPanel item = new StackPanel();
                    Button button = new Button();
                    button.IsManipulationEnabled = false;
                    button.Width = 50;
                    button.Content = "or";

                    item.Children.Add(button);
                    item.IsHitTestVisible = false;
                    containerList.Items.Add(item); //add item to containerlist
                }

                foreach (Trigger trigger in container.Triggers) //put triggers in trigger listbox
                {
                    StackPanel item = new StackPanel();
                    ListBox list = new ListBox(); //create listbox for triggers
                    list.DisplayMemberPath = "Type";
                    list.IsHitTestVisible = false;
                    list.Items.Add(trigger);

                    item.Children.Add(list); //add triggers to item
                    containerList.Items.Add(item); //add item to containerlist
                }
            }
            containerList.Items.Refresh();
        }
        private void andTriggerButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)andTriggerButton.Content == "AND")
            {
                isAnd = true;
                andTriggerButton.Content = "CANCEL";
                orTriggerButton.Content = "OR";
                newTriggerList.Visibility = Visibility.Visible;
                triggerFrame.Visibility = Visibility.Hidden;
                newTriggerList.ItemsSource = triggerType;
            }
            else
            {
                andTriggerButton.Content = "AND";
                orTriggerButton.Content = "OR";
                newTriggerList.Visibility = Visibility.Hidden;
                triggerFrame.Visibility = Visibility.Visible;
            }
        }
        private void orTriggerButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)orTriggerButton.Content == "OR")
            {
                isAnd = false;
                andTriggerButton.Content = "AND";
                orTriggerButton.Content = "CANCEL";
                newTriggerList.Visibility = Visibility.Visible;
                triggerFrame.Visibility = Visibility.Hidden;
                newTriggerList.ItemsSource = triggerType;
            }
            else
            {
                andTriggerButton.Content = "AND";
                orTriggerButton.Content = "OR";
                newTriggerList.Visibility = Visibility.Hidden;
                triggerFrame.Visibility = Visibility.Visible;
            }
        }
        private void newTriggerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (newTriggerList.SelectedIndex != -1) //make sure selection is valid
            {
                if (isAnd && module.Containers.Count > 0) //add new trigger
                {
                    AndTrigger(newTriggerList.SelectedItem.ToString());
                }
                else //add new container
                {
                    OrTrigger(newTriggerList.SelectedItem.ToString());
                }
                DisplayTriggers();
                andTriggerButton.Content = "AND";
                orTriggerButton.Content = "OR";
                newTriggerList.Visibility = Visibility.Hidden;
                newTriggerList.SelectedIndex = -1; //invalidate selection to reset
            }
        }
        private void AndTrigger(string type)
        {
            if (!module.Containers.Last().Closed())
            {
                switch (type)
                {
                    case "Keyboard Shortcut":
                        module.Containers.Last().Triggers.Add(new Hotkey());
                        break;
                    case "During a Time Range":
                        module.Containers.Last().Triggers.Add(new Schedule());
                        break;
                    case "On Startup":
                        OrTrigger(type);
                        break;
                    case "On Idle":
                        module.Containers.Last().Triggers.Add(new Idle());
                        break;
                    case "Any Program is Fullscreen":
                        module.Containers.Last().Triggers.Add(new Fullscreen());
                        break;
                    case "Specific Program is Running":
                        module.Containers.Last().Triggers.Add(new ProgramDetect());
                        break;
                    case "Computer is Charging":
                        module.Containers.Last().Triggers.Add(new Charging());
                        break;
                }
            }
            else
            {
                OrTrigger(type);
            }
        }
        private void OrTrigger(string type)
        {
            switch (type)
            {
                case "Keyboard Shortcut":
                    module.Containers.Add(new Container(new Hotkey()));
                    break;
                case "During a Time Range":
                    module.Containers.Add(new Container(new Schedule()));
                    break;
                case "On Startup":
                    module.Containers.Add(new Container(new Startup()));
                    break;
                case "On Idle":
                    module.Containers.Add(new Container(new Idle()));
                    break;
                case "Any Program is Fullscreen":
                    module.Containers.Add(new Container(new Fullscreen()));
                    break;
                case "Specific Program is Running":
                    module.Containers.Add(new Container(new ProgramDetect()));
                    break;
                case "Computer is Charging":
                    module.Containers.Add(new Container(new Charging()));
                    break;
            }
        }
        private void containerList_RemoveItem(object sender, RoutedEventArgs e)
        {
            if (containerList.Items.Count != 0) //idk how you would get here and this be false, but I did once somehow
            {
                Container container = FindContainer();
                if (container != null)
                {
                    container.Triggers.Remove(FindTrigger()); //remove it
                    if (container.Triggers.Count == 0) //if container has no trigger, remove it
                    {
                        module.Containers.Remove(container);
                    }
                }
            }
            FileMan.Save(module);
            DisplayTriggers();
            triggerFrame.Navigate(null);
        }
        private Container FindContainer()
        {
            Trigger trigger = FindTrigger();
            if (trigger != null)
            {
                foreach (Container container in module.Containers)
                {
                    if (container.Triggers.Contains(trigger))
                    {
                        return container;
                    }
                }
            }
            return null;
        }
        private Trigger FindTrigger()
        {
            StackPanel stack = (StackPanel)containerList.SelectedValue; //get correct stack panel in containerList
            if (stack != null && !stack.Children.OfType<Button>().Any())
            {
                return (Trigger)(stack.Children.OfType<ListBox>().First().Items[0]); //get correct item in list box in stack panel
            }
            return null;
        }
        private void containerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trigger trigger = FindTrigger();
            if (trigger != null)
            {
                switch (trigger.Type)
                {
                    case "Keyboard Shortcut":
                        triggerFrame.Navigate(new HotkeyPage((Hotkey)FindTrigger()));
                        break;
                    case "During a Time Range":
                        triggerFrame.Navigate(new SchedulePage((Schedule)FindTrigger()));
                        break;
                    case "On Startup":
                        triggerFrame.Navigate(new StartupPage((Startup)FindTrigger()));
                        break;
                    case "On Idle":
                        triggerFrame.Navigate(new IdlePage((Idle)FindTrigger()));
                        break;
                    case "Any Program is Fullscreen":
                        triggerFrame.Navigate(new FullscreenPage((Fullscreen)FindTrigger()));
                        break;
                    case "Specific Program is Running":
                        triggerFrame.Navigate(new ProgramDetectPage((ProgramDetect)FindTrigger()));
                        break;
                    case "Computer is Charging":
                        triggerFrame.Navigate(new ChargingPage((Charging)FindTrigger()));
                        break;
                }
                newTriggerList.Visibility = Visibility.Hidden;
                triggerFrame.Visibility = Visibility.Visible;
            }
        }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            FileMan.Update(module);
        }
    }
}
