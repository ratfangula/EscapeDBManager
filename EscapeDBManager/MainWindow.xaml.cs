using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using EscapeDBManager.Data;
using System.Collections.ObjectModel;
using System.Data;

namespace EscapeDBManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Team> _teamList;
        public ObservableCollection<Round> _roundList;

        public MainWindow()
        {
            InitializeComponent();
            _teamList = new ObservableCollection<Team>();
            _teamList.Add(new Team("Seed"));
            dataGridTeam.DataContext = _teamList;

            _roundList = new ObservableCollection<Round>();
            dataGridRound.DataContext = _roundList;
        }

        private void Fetch_Click(object sender, RoutedEventArgs e)
        {
            DBConnect dbMan = new DBConnect();
            //dataGridRoom.DataContext = dbMan.GetRoomList();
            if (textNewGameID.Text == "")
            {
                MessageBox.Show("Enter a gameID to load");
                return;
            }
            int GameID = Convert.ToInt32(textNewGameID.Text);
            dataGridRoom.DataContext = dbMan.GetActiveRoundList(GameID);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            DBConnect dbMan = new DBConnect();
            textNewGameID.Text = dbMan.CreateNewGame(textGameName.Text,Convert.ToInt32(textCustomerID.Text), textGameDescription.Text).ToString();

        }

        private void AddTeamsToGame_Click(object sender, RoutedEventArgs e)
        {
            DBConnect dbMan = new DBConnect();
            int GameID = Convert.ToInt32(textNewGameID.Text);
            foreach (Team t in _teamList)
            {
                dbMan.AddTeamToGame(GameID, t.Name);
            }

            dbMan.GenerateGameRounds(GameID);

            //dataGridRound.DataContext = dbMan.GetRoundList(GameID);
            LoadRoundList(GameID);
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            if(textNewGameID.Text == "")
            {
                MessageBox.Show("Enter a gameID to load");
                return;
            }
            int GameID = Convert.ToInt32(textNewGameID.Text);
            LoadRoundList(GameID);

        }

        private void btnStartRound_Click(object sender, RoutedEventArgs e)
        {
            if(dataGridRound.SelectedItem == null)
            {
                MessageBox.Show("Please select a round to start");
                return;
            }
            int nRoundID = ((Round)dataGridRound.SelectedItem).RoundID;
            //MessageBox.Show(nRoundID.ToString());
            DBConnect dbMan = new DBConnect();
            dbMan.StartRound(nRoundID);
            //Reload Game List
            int GameID = Convert.ToInt32(textNewGameID.Text);
            LoadRoundList(GameID);
        }

        private void LoadRoundList(int GameID)
        {
            DBConnect dbMan = new DBConnect();
            DataSet ds = dbMan.GetRoundList(GameID);
            _roundList.Clear();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                Round newRound = new Round();
                newRound.RoundID = (int)r["RoundID"];
                newRound.GameID = (int)r["GameID"];
                newRound.TeamName = (string)r["TeamName"];
                newRound.RoomName = (string)r["RoomName"];
                if (r["StartTime"].ToString() == "")
                    newRound.StartTime = null;
                else
                    newRound.StartTime = (DateTime?)r["StartTime"];
                if (r["EndTime"].ToString() == "")
                    newRound.EndTime = null;
                else
                    newRound.EndTime = (DateTime?)r["EndTime"];

                if (r["HintsUsed"].ToString() == "")
                    newRound.HintsUsed = 0;
                else
                    newRound.HintsUsed = (int)r["HintsUsed"];
                if (r["PlusTime"].ToString() == "")
                    newRound.PlusTime = null;
                else
                    newRound.PlusTime = (TimeSpan?)r["PlusTime"];

                //                newRound.StartTime = (r["StartTime"]==null)?DateTime.MinValue:(DateTime)r["StartTime"];
                //newRound.EndTime = (r["EndTime"] == null) ? null : (DateTime?)r["EndTime"];

                _roundList.Add(newRound);
            }

            dataGridRound.DataContext = _roundList;
        }

        private void btnEndRound_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridRound.SelectedItem == null)
            {
                MessageBox.Show("Please select a round to start");
                return;
            }
            Round rnd = (Round)dataGridRound.SelectedItem;
            if( rnd.StartTime == null)
            {
                MessageBox.Show("Please select a round that has been started");
                return;

            }
            int nRoundID = rnd.RoundID;
            //MessageBox.Show(nRoundID.ToString());

            int GameID = Convert.ToInt32(textNewGameID.Text);
            DBConnect dbMan = new DBConnect();
            dbMan.EndRound(GameID, nRoundID);
            //Reload Game List
            
            LoadRoundList(GameID);
        }

        private void btnAddHint_Click(object sender, RoutedEventArgs e)
        {
            ModifyHintsUsedCount(1);
        }

        private void btnRemoveHint_Click(object sender, RoutedEventArgs e)
        {
            ModifyHintsUsedCount(-1);
        }

        private void ModifyHintsUsedCount(int nHintsDelta)
        {
            if (dataGridRound.SelectedItem == null)
            {
                MessageBox.Show("Please select a round in the grid");
                return;
            }
            Round rnd = (Round)dataGridRound.SelectedItem;
            int nRoundID = rnd.RoundID;
            int nNewHintCount = rnd.HintsUsed + nHintsDelta;

            int GameID = Convert.ToInt32(textNewGameID.Text);
            DBConnect dbMan = new DBConnect();
            dbMan.SetHintsUsed(GameID, nRoundID, nNewHintCount);

            //Reload Game List
            LoadRoundList(GameID);
        }

        private void btnSetPlusTime_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridRound.SelectedItem == null)
            {
                MessageBox.Show("Please select a round in the grid");
                return;
            }
            int MinutesDelta;
            if (!int.TryParse(textPlusTime.Text, out MinutesDelta))
            {
                
                MessageBox.Show("Please type a number of minutes to Add/Subtract to PlusTime");
            }



            Round rnd = (Round)dataGridRound.SelectedItem;
            int nRoundID = rnd.RoundID;
            
            int GameID = Convert.ToInt32(textNewGameID.Text);
            DBConnect dbMan = new DBConnect();
            dbMan.AddPlusTime(GameID, nRoundID, MinutesDelta);

            //Reload Game List
            LoadRoundList(GameID);
        }

        private void btnNotifyTeam_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridRound.SelectedItem == null)
            {
                MessageBox.Show("Please select a round in the grid");
                return;
            }

            Round rnd = (Round)dataGridRound.SelectedItem;
            int nRoundID = rnd.RoundID;
            string MessageText = "Team " + rnd.TeamName + " is next for "
                + rnd.RoomName + "!" 
                + Environment.NewLine + "Please report to front desk for instructions.";


            int GameID = Convert.ToInt32(textNewGameID.Text);


            DBConnect dbMan = new DBConnect();
            dbMan.SendMessage(GameID, MessageText);
        }
    }
}
